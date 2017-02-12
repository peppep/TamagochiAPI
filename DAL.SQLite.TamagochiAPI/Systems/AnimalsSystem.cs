using System;
using System.Collections.Generic;
using System.Linq;
using TamagochiAPI.Common.Models;

namespace TamagochiAPI.DAL.SQLite.Systems
{
	public interface IAnimalsSystem
	{
		IEnumerable<Animal> GetAnimals();
		Animal GetAnimalById(uint animalId);
		Animal GetAnimalByName(string animalName);
		void AddAnimal(Animal animal);
		void Feed(uint animalId, int hungerLevel);
		void Play(uint animalId, int happinessLevel);
	}

	public class AnimalsSystem : IAnimalsSystem
	{
		public void AddAnimal(Animal animal)
		{
			var cmd = string.Format(
				"insert into animals(name, type, owner_id, happines_level, last_play_time, hungry_level, last_feed_time)" +
				"values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')",
				animal.Name,
				(int)animal.Type,
				animal.OwnerId,
				animal.HappinessLevel,
				animal.LastPlayTime.ToString(),
				animal.HungryLevel,
				animal.LastFeedTime.ToString()
				);
			DBConnection.ExecuteNonQuery(cmd);
		}

		public void Feed(uint animalId, int hungerLevel)
		{
			var cmd = string.Format("update animals set hungry_level = {0}, last_feed_time = '{1}' where id = {2}", hungerLevel, DateTime.UtcNow, animalId);
			DBConnection.ExecuteNonQuery(cmd);
		}

		public Animal GetAnimalById(uint animalId)
		{
			var cmd = string.Format("select * from animals where id = {0}", animalId.ToString());
			return DBConnection.ExecuteReader<Animal>(cmd).Result.FirstOrDefault();
		}

		public Animal GetAnimalByName(string animalName)
		{
			var cmd = string.Format("select * from animals where name = '{0}'", animalName);
			return DBConnection.ExecuteReader<Animal>(cmd).Result.FirstOrDefault();
		}

		public IEnumerable<Animal> GetAnimals()
		{
			var cmd = "select * from animals";
			return DBConnection.ExecuteReader<Animal>(cmd).Result;
		}

		public void Play(uint animalId, int happinessLevel)
		{
			var cmd = string.Format("update animals set happines_level = {0}, last_play_time = '{1}' where id = {2}", happinessLevel, DateTime.UtcNow, animalId);
			DBConnection.ExecuteNonQuery(cmd);
		}
	}
}