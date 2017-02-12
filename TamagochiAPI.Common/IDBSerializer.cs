using System.Data.Common;

namespace TamagochiAPI.Common
{
	public interface IDBSerializer<T>
	{
		void Deserialize(DbDataReader reader);
	}
}