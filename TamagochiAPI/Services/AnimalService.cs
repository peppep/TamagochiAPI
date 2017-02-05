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

			var hungryLevelStep = m_configService.GetConfigValue<int>(
				ConfigKeys.HungryDecreaseStepSec,
				animalInfo.Type);

			var minLevel = m_configService.GetConfigValue<int>(ConfigKeys.MinThreshold);

			var timeSinceLastFeed = DateTime.UtcNow - animalInfo.LastFeedTime;
			var pointsToDecrease = timeSinceLastFeed.TotalSeconds / hungryLevelStep;

			int currentHungryLevel = (int)(animalInfo.HungryLevel - pointsToDecrease + hungryLevelStep);
			if(currentHungryLevel < minLevel)
			{
				currentHungryLevel = minLevel;
			}

			m_animalsWrapper.Feed(animalId, currentHungryLevel);

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
	}
}