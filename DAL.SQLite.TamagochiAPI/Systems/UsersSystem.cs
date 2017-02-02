using System;
using System.Collections.Generic;
using System.Linq;
using TamagochiAPI.DAL.SQLite.Models;

namespace TamagochiAPI.DAL.SQLite.Systems
{
	public interface IUserSystem
	{
		IEnumerable<User> GetUsersInfo();
		User GetUserInfoByNick(string nickname);
		User GetUserInfoById(uint userId);
		void AddUser(User userInfo);
		void UpdateLoginTime(uint userId, DateTime timeStamp);
	}

	public class UsersSystem : IUserSystem
	{
		public void AddUser(User userInfo)
		{
			var cmd = string.Format("insert into users(name, last_login) values('{0}', '{1}')", userInfo.Name, userInfo.LastLogin.ToString());
			DBConnection.ExecuteNonQuery(cmd);
		}

		public User GetUserInfoById(uint userId)
		{
			var cmd = string.Format("select * from users where id = {0}", userId.ToString());
			return DBConnection.ExecuteReader<User>(cmd).Result.FirstOrDefault();
		}

		public User GetUserInfoByNick(string nickname)
		{
			var cmd = string.Format("select * from users where name = {0}", nickname);
			return DBConnection.ExecuteReader<User>(cmd).Result.FirstOrDefault();
		}

		public IEnumerable<User> GetUsersInfo()
		{
			var cmd = "select * from users;";
			return DBConnection.ExecuteReader<User>(cmd).Result;
		}

		public void UpdateLoginTime(uint userId, DateTime timeStamp)
		{
			var cmd = string.Format("update users set last_login = '{0}' where id = {1}", timeStamp.ToString(), userId.ToString());
			DBConnection.ExecuteNonQuery(cmd);
		}
	}
}