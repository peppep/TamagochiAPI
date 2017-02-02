using TamagochiAPI.DAL.SQLite.Models;

namespace TamagochiAPI.RESTModelBinding
{
	public class AnimalDataModelBinding
	{
		public string Name { get; set; }
		public uint OwnerId { get; set; }
		public AnimalType Type { get; set; }
	}
}