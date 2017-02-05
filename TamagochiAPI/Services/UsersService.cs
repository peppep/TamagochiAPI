using System;
using TamagochiAPI.Common;
using TamagochiAPI.Configs;
using TamagochiAPI.DAL.SQLite.Models; // <-- Think about this!!!
using TamagochiAPI.DAL.Wrappers;

namespace TamagochiAPI.Services
{
	using System.Linq;
	using Log = Log.Log;

	public interface IUserService
	{
		ResultInfo<User> GetUsers();
		ResultInfo<User> GetUser(uint userId, bool needValidate = false);
		ResultInfo<EmptyResultData> CreateUser(string nickname);
		ResultInfo<EmptyResultData> Login(uint userId);
	}

	public class UsersService : IUserService
	{
		private readonly IUsersWrapper m_userWrapper;
		private readonly IConfigService m_configService;

		public UsersService(IUsersWrapper usersWrapper, IConfigService configService)
		{
			m_userWrapper = usersWrapper;
			m_configService = configService;
		}

		public ResultInfo<EmptyResultData> Login(uint userId)
		{
			var res = new ResultInfo<EmptyResultData>();

			var userResult = GetUser(userId);
			if(userResult.IsEmpty())
			{
				res.ResultCode = ResultCode.UserNotFound;
				Log.Warning("Trying to login with userId: {0}. User not found", userId);
				return res;
			}

			m_userWrapper.UpdateLoginTime(userId, DateTime.UtcNow);
			Log.Info("User userId: {0} session started", userId);

			return new ResultInfo<EmptyResultData>();
		}

		public ResultInfo<EmptyResultData> CreateUser(string nickname)
		{
			var res = new ResultInfo<EmptyResultData>();
			var userInfo = m_userWrapper.GetUserInfoByNick(nickname);

			if (userInfo == null)
			{
				var user = new User
				{
					Name = nickname,
					LastLogin = DateTime.UtcNow
				};

				m_userWrapper.AddUser(user);
				Log.Info("User with nickname: {0} added", nickname);
				return res;
			}

			res.ResultCode = ResultCode.NameRestricted;
			Log.Warning("Trying create user with name: {0}. The name is in use", nickname);
			return res;
		}

		public ResultInfo<User> GetUsers()
		{
			return new ResultInfo<User>
			{
				ResultData = m_userWrapper.GetUsersInfo().ToList()
			};
		}

		public ResultInfo<User> GetUser(uint userId, bool needValidate = false)
		{
			var result = new ResultInfo<User>();

			var userInfo = m_userWrapper.GetUserInfoById(userId);
			result.AddData(userInfo);

			if (!needValidate)
			{
				return result;
			}

			result.ResultCode = Validate(userInfo, userId);
			return result;
		}

		private ResultCode Validate(User userInfo, uint userId)
		{
			if (userInfo == null)
			{
				Log.Warning("User with userId: {0} not found", userId);
				return ResultCode.UserNotFound;
			}

			var sessionTimeoutSec = m_configService.GetConfigValue<uint>(ConfigKeys.SessionTimeSec);
			var diff = DateTime.UtcNow - userInfo.LastLogin;

			if (diff.TotalSeconds > sessionTimeoutSec)
			{
				Log.Warning("Trying to get userInfo for userId: {0}. Session is closed", userId);
				return ResultCode.SessionClosed;
			}

			return ResultCode.Ok;
		}
	}
}