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
			var res = m_requestRepository.Get<User>("api/User/");

			var filteredResult = res.ResultData.Where(r => r.UserId == m_userId).ToList();

			ConsoleUtils.PrintInfo(filteredResult);
			ConsoleUtils.ShowFlowTip();
		}

		[Command(ConsoleKey.A)]
		public void GetAnimals(uint userId)
		{
			var res = m_requestRepository.Get<Animal>("api/Animal/");

			var filteredResult = res.ResultData.Where(r => r.OwnerId == m_userId).ToList();

			ConsoleUtils.PrintInfo(filteredResult);
			ConsoleUtils.ShowFlowTip();
		}

		[Command(ConsoleKey.L)]
		public void Login(uint userId)
		{
			var url = string.Format("api/User/{0}", m_userId);
			var res = m_requestRepository.Put<EmptyResultData>(url);

			Console.WriteLine(res.ToString());
			ConsoleUtils.ShowFlowTip();
		}

		[Command(ConsoleKey.P)]
		public void Play(uint userId)
		{
			var randomAnimalId = GetUsersRandomAnimal();

			var url = string.Format("api/User/{0}/play/{1}", m_userId, randomAnimalId ?? 0);
			var res = m_requestRepository.Put<KeyValue>(url);

			Console.WriteLine(res.ToString());
			ConsoleUtils.ShowFlowTip();
		}

		[Command(ConsoleKey.F)]
		public void Feed(uint userId)
		{
			var randomAnimalId = GetUsersRandomAnimal();

			var url = string.Format("api/User/{0}/feed/{1}", m_userId, randomAnimalId ?? 0);
			var res = m_requestRepository.Put<KeyValue>(url);

			Console.WriteLine(res.ToString());
			ConsoleUtils.ShowFlowTip();
		}

		private uint? GetUsersRandomAnimal()
		{
			var usersAnimals = m_requestRepository.Get<Animal>("api/Animal/");
			var filteredResult = usersAnimals.ResultData.Where(r => r.OwnerId == m_userId).ToList();

			if (!filteredResult.Any())
			{
				Console.WriteLine("You don't own any animal");
				ConsoleUtils.ShowFlowTip();
				return null;
			}
			return filteredResult.OrderBy(a => m_random.Next()).First().Id;
		}

		private string GetData()
		{
			return string.Empty;
		}
	}
}