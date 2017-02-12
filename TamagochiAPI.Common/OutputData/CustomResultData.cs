namespace TamagochiAPI.Common.OutputData
{
	public abstract class EmptyResultData
	{
	}

	public class KeyValue
	{
		public string Name { get; set; }
		public int Value { get; set; }

		public override string ToString()
		{
			return string.Format("OperationName: {0}; Value: {1}", Name, Value);
		}
	}
}