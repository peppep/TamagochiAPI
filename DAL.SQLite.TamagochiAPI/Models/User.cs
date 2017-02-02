using System;
using System.Data.Common;
using System.Data.SQLite;
using TamagochiAPI.DAL.SQLite.Utils;

namespace TamagochiAPI.DAL.SQLite.Models
{
	public class User : IDBSerializer<User>
	{
		public uint UserId { get; set; }
		public string Name { get; set; }
		public DateTime LastLogin { get; set; }

		public void Deserialize(SQLiteDataReader reader)
		{
			UserId = uint.Parse(reader[0].ToString());
			Name = reader[1].ToString();
			LastLogin = new DateTime().FromString(reader[2].ToString());
		}

		public void Deserialize(DbDataReader reader)
		{
			UserId = uint.Parse(reader[0].ToString());
			Name = reader[1].ToString();
			LastLogin = new DateTime().FromString(reader[2].ToString());
		}
	}
}