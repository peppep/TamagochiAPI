using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TamagochiAPI.Client.Utils;

namespace TamagochiAPI.Client
{
	internal partial class Game
	{
		private uint m_userId;
		private Random m_random;
		private RequestRepository m_requestRepository;
		private IDictionary<ConsoleKey, Action<uint>> m_commands;

		internal Game(uint userId)
		{
			m_userId = userId;
			m_random = new Random(DateTime.UtcNow.Millisecond);
			m_requestRepository = new RequestRepository();
			InitCommands();
		}

		internal void Run()
		{
			var key = new ConsoleKeyInfo();
			do
			{
				ConsoleUtils.ShowHelp();
				key = Console.ReadKey();

				if (m_commands.ContainsKey(key.Key))
				{
					m_commands[key.Key](m_userId);
				}
			}
			while (key.Key != ConsoleKey.Escape);
		}

		private void InitCommands()
		{
			var methods = Assembly.GetExecutingAssembly().GetTypes()
				.SelectMany(t => t.GetMethods())
				.Where(m => m.GetCustomAttributes(typeof(CommandAttribute), false).Length > 0)
				.ToArray();

			m_commands = new Dictionary<ConsoleKey, Action<uint>>();

			foreach (var method in methods)
			{
				var k = method.GetCustomAttribute<CommandAttribute>().AssociatedKey;
				if (m_commands.ContainsKey(k))
				{
					throw new Exception("You want to add command associated with same ConsoleKey twice");
				}

				m_commands[k] = (Action<uint>)Delegate.CreateDelegate(typeof(Action<uint>), this, method);
			}
		}
	}
}