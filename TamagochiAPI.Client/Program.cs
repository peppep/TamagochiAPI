using Newtonsoft.Json;
using RestSharp;
using System;
using TamagochiAPI.Common.Models;
using TamagochiAPI.Common.OutputData;

namespace TamagochiAPI.Client
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var address = "http://localhost:8080";

			var client = new RestClient(address);

			var request = new RestRequest("api/User/", Method.GET);

			//var resp = client.ExecuteAsync<User>(request, r =>
			//{
			//	Console.WriteLine(r.Data.UserId);
			//});

			var rr = client.Execute<User>(request);
			Console.WriteLine(rr.Data.Name);

			var result = JsonConvert.DeserializeObject<ResultInfo<User>>(rr.Content);

			//Console.WriteLine(resp);
		}
	}
}