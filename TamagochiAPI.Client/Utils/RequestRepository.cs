using Newtonsoft.Json;
using RestSharp;
using TamagochiAPI.Common.OutputData;

namespace TamagochiAPI.Client.Utils
{
	internal class RequestRepository
	{
		internal ResultInfo<T> Get<T>(string urlChunk)
		{
			var client = new RestClient(Program.Address);
			var request = new RestRequest(urlChunk, Method.GET);
			var response = client.Execute(request);

			return JsonConvert.DeserializeObject<ResultInfo<T>>(response.Content);
		}

		internal ResultInfo<T> Put<T>(string urlChunk)
		{
			var client = new RestClient(Program.Address);
			var request = new RestRequest(urlChunk, Method.PUT);
			var response = client.Execute(request);
			return JsonConvert.DeserializeObject<ResultInfo<T>>(response.Content);
		}
	}
}