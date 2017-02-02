using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace TamagochiAPI.Common
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
			if(data == null)
			{
				return;
			}

			ResultData.Add(data);
		}

		public bool IsEmpty()
		{
			return !ResultData.Any();
		}
	}
}