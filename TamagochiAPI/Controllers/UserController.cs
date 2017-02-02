using System.Web.Http;
using TamagochiAPI.RESTModelBinding;
using TamagochiAPI.DAL.SQLite.Models; //<-- need to move to other namespace
																			//using TamagochiAPI.Models;
using TamagochiAPI.Services;
using TamagochiAPI.Common;
using System.Threading.Tasks;

namespace TamagochiAPI.Controllers
{
	public class UserController : ApiController
  {
		private readonly IUserService m_userService;

		public UserController(IUserService userService)
		{
			m_userService = userService;
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
		public ResultInfo<EmptyResultData> Post(UserNicknameModelBinding user)
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
	}
}