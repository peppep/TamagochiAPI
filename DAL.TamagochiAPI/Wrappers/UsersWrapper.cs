using TamagochiAPI.DAL.SQLite.Systems;
using System;
using System.Collections.Generic;
using TamagochiAPI.DAL.SQLite.Models;

namespace TamagochiAPI.DAL.Wrappers
{
	public interface IUsersWrapper
	{
		User GetUserInfoById(uint userId);
		User GetUserInfoByNick(string nickname);
		IEnumerable<User> GetUsersInfo();
		void AddUser(User userInfo);
		void UpdateLoginTime(uint userId, DateTime timeStamp);
	}

	public class UsersWrapper : IUsersWrapper
	{
		private readonly IUserSystem m_userSystem;

		public UsersWrapper(IUserSystem userSystem)
		{
			m_userSystem = userSystem;
		}

		public void AddUser(User userInfo)
		{
			m_userSystem.AddUser(userInfo);
		}

		public User GetUserInfoByNick(string nickname)
		{
			return m_userSystem.GetUserInfoByNick(nickname);
		}

		public User GetUserInfoById(uint userId)
		{
			return m_userSystem.GetUserInfoById(userId);
		}

		public IEnumerable<User> GetUsersInfo()
		{
			return m_userSystem.GetUsersInfo();
		}
		
		public void UpdateLoginTime(uint userId, DateTime timeStamp)
		{
			m_userSystem.UpdateLoginTime(userId, timeStamp);
		}
	}
}