using System.Web.Http;
using TamagochiAPI.Common;
using TamagochiAPI.DAL.SQLite.Models;
using TamagochiAPI.RESTModelBinding;
using TamagochiAPI.Services;

namespace TamagochiAPI.Controllers
{
	public class AnimalController : ApiController
	{
		private readonly IAnimalService m_animalService;

		public AnimalController(IAnimalService animalService)
		{
			m_animalService = animalService;
		}

		// GET: api/Animal
		//get all animals
		public ResultInfo<Animal> Get()
		{
			return m_animalService.GetAnimals();
		}

		// GET: api/Animal/5
		// get animalInfo
		public ResultInfo<Animal> Get(int id)
		{
			return m_animalService.GetAnimal((uint)id);
		}

		//POST: api/Animal
		// add new animal
		//{'Name' : 'puup',
		//'OwnerId' : 1,
		//'Type': 3}
		public ResultInfo<EmptyResultData> Post(AnimalDataModelBinding animal)
		{
			return m_animalService.AddAnimal(animal.Name, animal.OwnerId, animal.Type);
		}
	}
}