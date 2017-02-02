using System.Data.Common;
using System.Data.SQLite;

namespace TamagochiAPI.DAL.SQLite
{
	public interface IDBSerializer<T>
	{
		void Deserialize(DbDataReader reader);
	}
}