using System;
using TamagochiAPI.DAL.Wrappers;
using TamagochiAPI.DAL.SQLite.Models;
using TamagochiAPI.Configs;
using TamagochiAPI.Common;
using System.Linq;

namespace TamagochiAPI.Services
{
	public interface IAnimalService
	{
		ResultInfo<EmptyResultData> AddAnimal(string name, uint ownerId, AnimalType type);
		ResultInfo<Animal> GetAnimals();
		ResultInfo<Animal> GetAnimal(uint animalId);
		ResultInfo<KeyValue> Feed(uint userId, uint animalId);
		ResultInfo<KeyValue> Play(uint userId, uint animalId);
	}

	enum OperationType
	{
		Feed,
		Play
	}

	public class AnimalService : IAnimalService
	{
		private readonly IAnimalsWrapper m_animalsWrapper;
		private readonly IUserService m_usersService;
		private readonly IConfigService m_configService;

		public AnimalService(IAnimalsWrapper animalWrapper, IUserService userService, IConfigService configService)
		{
			m_animalsWrapper = animalWrapper;
			m_usersService = userService;
			m_configService = configService;
		}

		public ResultInfo<KeyValue> Feed(uint userId, uint animalId)
		{
			var res = new ResultInfo<KeyValue>();

			var animalInfo = GetInfoWithRestrictions(res, animalId, userId);
			if(animalInfo == null)
			{
				return res;
			}

			var hungerStep = m_configService.GetConfigValue<int>(
				ConfigKeys.HungerStep,
				animalInfo.Type);

			var stat = CalculateStat(animalInfo, hungerStep, OperationType.Feed);

			m_animalsWrapper.Feed(animalId, stat.Value);

			res.AddData(stat);
			return res;
		}

		public ResultInfo<KeyValue> Play(uint userId, uint animalId)
		{
			var res = new ResultInfo<KeyValue>();

			var animalInfo = GetInfoWithRestrictions(res, animalId, userId);
			if (animalInfo == null)
			{
				return res;
			}

			var happinessStep = m_configService.GetConfigValue<int>(
				ConfigKeys.HappinessStep,
				animalInfo.Type);
			
			var stat = CalculateStat(animalInfo, happinessStep, OperationType.Play);

			m_animalsWrapper.Play(animalId, stat.Value);

			res.AddData(stat);
			return res;
		}

		public ResultInfo<Animal> GetAnimals()
		{
			return new ResultInfo<Animal>()
			{
				ResultData = m_animalsWrapper.GetAnimals().ToList()
			};
		}

		public ResultInfo<Animal> GetAnimal(uint animalId)
		{
			var res = new ResultInfo<Animal>();

			var animalInfo = m_animalsWrapper.GetAnimalById(animalId);
			if (animalInfo == null)
			{
				res.ResultCode = ResultCode.AnimalNotFound;
				return res;
			}

			res.AddData(animalInfo);
			return res;
		}

		public ResultInfo<EmptyResultData> AddAnimal(string name, uint ownerId, AnimalType type)
		{
			var res = new ResultInfo<EmptyResultData>();

			var userResult = m_usersService.GetUser(ownerId, needValidate:true);
			if (userResult.ResultCode != ResultCode.Ok)
			{
				res.ResultCode = userResult.ResultCode;
				return res;
			}

			var animalInfo = m_animalsWrapper.GetAnimalByName(name);
			if (animalInfo != null)
			{
				res.ResultCode = ResultCode.NameRestricted;
				return res;
			}

			var animal = CreateAnimal(name, ownerId, type);

			m_animalsWrapper.AddAnimal(animal);
			return res;
		}

		private Animal GetInfoWithRestrictions(IOperationStatus res, uint animalId, uint userId = 0)
		{
			var userResult = m_usersService.GetUser(userId, needValidate:true);
			if (userResult.ResultCode != ResultCode.Ok)
			{
				res.ResultCode = userResult.ResultCode;
				return null;
			}

			var animalInfo = m_animalsWrapper.GetAnimalById(animalId);
			if (animalInfo == null)
			{
				res.ResultCode = ResultCode.AnimalNotFound;
				return null;
			}
			
			if (userId != 0 && animalInfo.OwnerId != userId)
			{
				res.ResultCode = ResultCode.NotBelongsToUser;
				return null;
			}

			return animalInfo;
		}

		private Animal CreateAnimal(string name, uint ownerId, AnimalType type)
		{
			return new Animal
			{
				Name = name,
				OwnerId = ownerId,
				Type = type,
				HappinessLevel = m_configService.GetConfigValue<int>(ConfigKeys.DefaultHappinessLevel),
				HungryLevel = m_configService.GetConfigValue<int>(ConfigKeys.DefaultHungryLevel),
				LastFeedTime = DateTime.UtcNow,
				LastPlayTime = DateTime.UtcNow
			};
		}

		private KeyValue CalculateStat(Animal animalInfo, int statsStep, OperationType type)
		{
			var res = new KeyValue { Name = type.ToString() };

			var minLevel = m_configService.GetConfigValue<int>(ConfigKeys.StatsMinLevel);
			var maxLevel = m_configService.GetConfigValue<int>(ConfigKeys.StatsMaxLevel);

			int resultStat = 0;
			if (type == OperationType.Feed)
			{
				var timeSinceLastAction = DateTime.UtcNow - animalInfo.LastFeedTime;
				var changePoints = timeSinceLastAction.TotalSeconds / statsStep;
				resultStat = (int)(animalInfo.HungryLevel - changePoints + statsStep);
			}
			else if (type == OperationType.Play)
			{
				var timeSinceLastAction = DateTime.UtcNow - animalInfo.LastPlayTime;
				var changePoints = timeSinceLastAction.TotalSeconds / statsStep;
				resultStat = (int)(animalInfo.HappinessLevel - changePoints + statsStep);
			}

			if (resultStat < minLevel)
			{
				resultStat = minLevel;
			}
			if (resultStat > maxLevel)
			{
				resultStat = maxLevel;
			}

			res.Value = resultStat;

			return res;
		}
	}
}