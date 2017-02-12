using System.Web.Http;
using TamagochiAPI.Common;
using TamagochiAPI.DAL.SQLite.Models;
using TamagochiAPI.RESTModelBinding;

using TamagochiAPI.Services;

namespace TamagochiAPI.Controllers
{
	public class UserController : ApiController
	{
		private readonly IUserService m_userService;
		private readonly IAnimalService m_animalService;

		public UserController(IUserService userService, IAnimalService animalService)
		{
			m_userService = userService;
			m_animalService = animalService;
		}

		// GET: api/User
		public ResultInfo<User> Get()
		{
			return m_userService.GetUsers();
		}

		// GET: api/User/5
		public ResultInfo<User> Get(uint id)
		{
			return m_userService.GetUser(id);
		}

		// POST: api/User
		// create user
		//{'Name' : 'puup'}
		public ResultInfo<EmptyResultData> Post([FromBody]UserNicknameModelBinding user)
		{
			return m_userService.CreateUser(user.Nickname);
		}

		// PUT: api/User/5
		// update login date
		[HttpPut]
		public ResultInfo<EmptyResultData> Put(int id)
		{
			return m_userService.Login((uint)id);
		}

		[HttpPut]
		[Route("api/User/{userId:int}/feed/{animalId:int}")]
		public ResultInfo<KeyValue> Feed(uint userId, uint animalId)
		{
			return m_animalService.Feed(userId, animalId);
		}

		[HttpPut]
		[Route("api/User/{userId:int}/play/{animalId:int}")]
		public ResultInfo<KeyValue> Play(uint userId, uint animalId)
		{
			return m_animalService.Play(userId, animalId);
		}
	}
}