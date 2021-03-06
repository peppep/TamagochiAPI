﻿using System.Collections.Generic;
using TamagochiAPI.Common.Models;
using TamagochiAPI.DAL.SQLite.Systems;

namespace TamagochiAPI.DAL.Wrappers
{
	public interface IAnimalsWrapper
	{
		void AddAnimal(Animal animal);
		IEnumerable<Animal> GetAnimals();
		Animal GetAnimalByName(string name);
		Animal GetAnimalById(uint animalId);
		void Feed(uint animalId, int hungerLevel);
		void Play(uint animalId, int happinessLevel);
	}

	public class AnimalsWrapper : IAnimalsWrapper
	{
		private readonly IAnimalsSystem m_animalSystem;

		public AnimalsWrapper(IAnimalsSystem userSystem)
		{
			m_animalSystem = userSystem;
		}

		public void AddAnimal(Animal animal)
		{
			m_animalSystem.AddAnimal(animal);
		}

		public void Feed(uint animalId, int hungerLevel)
		{
			m_animalSystem.Feed(animalId, hungerLevel);
		}

		public Animal GetAnimalById(uint animalId)
		{
			return m_animalSystem.GetAnimalById(animalId);
		}

		public Animal GetAnimalByName(string name)
		{
			return m_animalSystem.GetAnimalByName(name);
		}

		public IEnumerable<Animal> GetAnimals()
		{
			return m_animalSystem.GetAnimals();
		}

		public void Play(uint animalId, int happinessLevel)
		{
			m_animalSystem.Play(animalId, happinessLevel);
		}
	}
}