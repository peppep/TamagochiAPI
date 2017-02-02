﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading.Tasks;
using System.Web;

namespace TamagochiAPI.DAL.SQLite
{
	public class DBConnection
	{
		private const string DbPath = "..\\db_scripts\\tamagochi.db;";

		public static string ConnectionString = "Data Source=" + HttpRuntime.AppDomainAppPath + DbPath + " Version=3; Foreign Keys=True;";

		private static DBConnection m_dbConnection;
		private static object m_lock = new object();

		private DBConnection()
		{
		}

		public SQLiteConnection Connection
		{
			get
			{
				var connection = new SQLiteConnection(ConnectionString);
				connection.Open();
				return connection;
			}
		}

		public static void ExecuteScalar(string query)
		{
			using (var conn = new SQLiteConnection(ConnectionString))
			{
				var cmd = conn.CreateCommand();
				cmd.CommandText = query;
				var res = cmd.ExecuteScalar();
			}
		}

		public static T ExecuteScalar<T>(string query) where T : IDBSerializer<T>, new()
		{
			var res = new T();
			using (var conn = GetConnection().Connection)
			{
				using (var cmd = new SQLiteCommand(query, conn))
				{
					try
					{
						cmd.ExecuteScalar();
					}
					catch (SQLiteException ex)
					{
						Console.WriteLine(ex.Message);
					}
				}
			}
			return res;
		}

		public static async void ExecuteNonQuery(string query)
		{
			using (var conn = GetConnection().Connection)
			{
				using (var cmd = new SQLiteCommand(query, conn))
				{
					try
					{
						var ret = await cmd.ExecuteNonQueryAsync();
					}
					catch (SQLiteException ex)
					{
						Console.WriteLine(ex.Message);
						throw ex;
					}
				}
			}
		}

		public static async Task<IEnumerable<T>> ExecuteReader<T>(string query) where T : IDBSerializer<T>, new()
		{
			var res = new List<T>();
			using (var conn = GetConnection().Connection)
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
						Console.WriteLine(ex.Message);
					}
				}
			}

			return res;
		}

		private static DBConnection GetConnection()
		{
			if(m_dbConnection == null)
			{
				lock(m_lock)
				{
					if(m_dbConnection == null)
					{
						m_dbConnection = new DBConnection();
					}
				}
			}

			return m_dbConnection;
		}
	}
}