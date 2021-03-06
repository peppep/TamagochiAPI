﻿using System;
using System.Data.Common;
using TamagochiAPI.Common.Utils;

namespace TamagochiAPI.Common.Models
{
	public enum AnimalType
	{
		Dog,
		Cat,
		Hamster,
		Fish,
		Snake
	}

	public class Animal : IDBSerializer<Animal>
	{
		public uint Id { get; set; }
		public string Name { get; set; }
		public uint OwnerId { get; set; }
		public AnimalType Type { get; set; }
		public int HappinessLevel { get; set; }
		public DateTime LastPlayTime { get; set; }
		public int HungryLevel { get; set; }
		public DateTime LastFeedTime { get; set; }

		public void Deserialize(DbDataReader reader)
		{
			Id = uint.Parse(reader[0].ToString());
			Name = reader[1].ToString();
			OwnerId = uint.Parse(reader[2].ToString());
			Type = (AnimalType)uint.Parse(reader[3].ToString());
			HappinessLevel = int.Parse(reader[4].ToString());
			LastPlayTime = new DateTime().FromString(reader[5].ToString());
			HungryLevel = int.Parse(reader[6].ToString());
			LastFeedTime = new DateTime().FromString(reader[7].ToString());
		}

		public override string ToString()
		{
			return string.Format(
				"animalId: {0}; Name: {1}; OwnerId: {2}; AnimalType: {3}; HappinessLevel: {4}; HungryLevel: {5}",
				Id,
				Name,
				OwnerId,
				Type,
				HappinessLevel,
				HungryLevel);
		}
	}
}