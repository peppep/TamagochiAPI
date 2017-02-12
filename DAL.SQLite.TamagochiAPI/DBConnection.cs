using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace TamagochiAPI.DAL.SQLite
{
	using Logger = Common.Log.Log;

	public class DBConnection
	{
		private const string DbPath = "..\\..\\db_scripts\\tamagochi.db;";

		public static string ConnectionString =
			"Data Source="
			+ AppDomain.CurrentDomain.BaseDirectory
			+ DbPath
			+ " Version=3; Foreign Keys=True;";

		private static DBConnection m_dbConnection;
		private static object m_lock = new object();

		private DBConnection()
		{
		}

		public SQLiteConnection SQLiteConnection
		{
			get
			{
				var connection = new SQLiteConnection(ConnectionString);
				connection.Open();
				return connection;
			}
		}

		private static DBConnection DbConnection
		{
			get
			{
				if (m_dbConnection == null)
				{
					lock (m_lock)
					{
						if (m_dbConnection == null)
						{
							m_dbConnection = new DBConnection();
						}
					}
				}

				return m_dbConnection;
			}
		}

		public static async void ExecuteNonQuery(string query)
		{
			using (var conn = DbConnection.SQLiteConnection)
			{
				using (var cmd = new SQLiteCommand(query, conn))
				{
					try
					{
						var ret = await cmd.ExecuteNonQueryAsync();
					}
					catch (SQLiteException ex)
					{
						Logger.Error(ex.Message);
						throw ex;
					}
				}
			}
		}

		public static async Task<IEnumerable<T>> ExecuteReader<T>(string query) where T : IDBSerializer<T>, new()
		{
			var res = new List<T>();
			using (var conn = DbConnection.SQLiteConnection)
			{
				using (var cmd = new SQLiteCommand(query, conn))
				{
					try
					{
						using (var reader = await cmd.ExecuteReaderAsync())
						{
							while (reader.Read())
							{
								var record = new T();
								record.Deserialize(reader);
								res.Add(record);
							}
						}
					}
					catch (SQLiteException ex)
					{
						Logger.Error(ex.Message);
					}
				}
			}

			return res;
		}
	}
}