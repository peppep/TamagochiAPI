using Newtonsoft.Json;
using RestSharp;
using System;
using System.Linq;
using TamagochiAPI.Client.Utils;
using TamagochiAPI.Common.Models;
using TamagochiAPI.Common.OutputData;

namespace TamagochiAPI.Client
{
	internal partial class Game
	{
		[Command(ConsoleKey.U)]
		public void GetUserInfo(uint userId)
		{
			var client = new RestClient(Program.Address);
			var request = new RestRequest("api/User/", Method.GET);
			var response = client.Execute(request);
			var res = JsonConvert.DeserializeObject<ResultInfo<User>>(response.Content);

			var filteredResult = res.ResultData.Where(r => r.UserId == m_userId).ToList();

			ConsoleUtils.PrintInfo(filteredResult);
			ConsoleUtils.ShowFlowTip();
		}

		[Command(ConsoleKey.A)]
		public void GetAnimals(uint userId)
		{
			var client = new RestClient(Program.Address);
			var request = new RestRequest("api/Animal/", Method.GET);
			var response = client.Execute(request);
			var res = JsonConvert.DeserializeObject<ResultInfo<Animal>>(response.Content);

			var filteredResult = res.ResultData.Where(r => r.OwnerId == m_userId).ToList();

			ConsoleUtils.PrintInfo(filteredResult);
			ConsoleUtils.ShowFlowTip();
		}

		[Command(ConsoleKey.L)]
		public void Login(uint userId)
		{
			var client = new RestClient(Program.Address);
			var request = new RestRequest("api/User/{id}", Method.PUT);
			request.AddUrlSegment("id", m_userId.ToString());
			var response = client.Execute(request);
			var res = JsonConvert.DeserializeObject<ResultInfo<EmptyResultData>>(response.Content);

			Console.WriteLine(res.ToString());
			ConsoleUtils.ShowFlowTip();
		}

		[Command(ConsoleKey.P)]
		public void Play(uint userId)
		{
			var client = new RestClient(Program.Address);
			var request = new RestRequest("api/User/{id}/play/{animalId}", Method.PUT);
			request.AddUrlSegment("id", m_userId.ToString());
			var response = client.Execute(request);
			var res = JsonConvert.DeserializeObject<ResultInfo<Animal>>(response.Content);

			Console.WriteLine(res.ToString());
			ConsoleUtils.ShowFlowTip();
		}

		private ResultInfo<T> Get<T>(string urlChunk)
		{
			var client = new RestClient(Program.Address);
			var request = new RestRequest(urlChunk, Method.GET);
			var response = client.Execute(request);

			return JsonConvert.DeserializeObject<ResultInfo<T>>(response.Content);
		}
	}
}