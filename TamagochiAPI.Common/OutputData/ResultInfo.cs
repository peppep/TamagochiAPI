using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TamagochiAPI.Common.OutputData
{
	public enum ResultCode
	{
		Ok,
		UserNotFound,
		AnimalNotFound,
		NameRestricted,
		Timeout,
		SessionClosed,
		NotBelongsToUser
	}

	public interface IOperationStatus
	{
		ResultCode ResultCode { get; set; }
	}

	[DataContract]
	public class ResultInfo<T> : IOperationStatus
	{
		public ResultInfo()
		{
			ResultCode = ResultCode.Ok;
			ResultData = new List<T>();
		}

		[DataMember]
		public ResultCode ResultCode { get; set; }
		[DataMember]
		public IList<T> ResultData { get; set; }

		public T GetFirst { get { return ResultData.FirstOrDefault(); } }

		public void AddData(T data)
		{
			if (data == null)
			{
				return;
			}

			ResultData.Add(data);
		}

		public bool IsEmpty()
		{
			return !ResultData.Any();
		}

		public override string ToString()
		{
			var res = new StringBuilder();

			res.AppendFormat("\nResultCode: {0}\n", ResultCode);
			res.AppendLine("Obtained data:");
			foreach (var data in ResultData)
			{
				res.AppendLine(data.ToString());
			}

			return res.ToString();
		}
	}
}