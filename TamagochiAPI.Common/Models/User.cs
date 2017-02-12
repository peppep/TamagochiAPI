using System;
using System.Data.Common;
using TamagochiAPI.Common.Utils;

namespace TamagochiAPI.Common.Models
{
	public class User : IDBSerializer<User>
	{
		public uint UserId { get; set; }
		public string Name { get; set; }
		public DateTime LastLogin { get; set; }

		public void Deserialize(DbDataReader reader)
		{
			UserId = uint.Parse(reader[0].ToString());
			Name = reader[1].ToString();
			LastLogin = new DateTime().FromString(reader[2].ToString());
		}

		public override string ToString()
		{
			return string.Format("Id: {0}; Name: {1}; LastLogin: {2}", UserId, Name, LastLogin);
		}
	}
}