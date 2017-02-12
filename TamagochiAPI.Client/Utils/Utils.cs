using System;
using System.Collections.Generic;
using System.Linq;

namespace TamagochiAPI.Client.Utils
{
	internal static class ConsoleUtils
	{
		internal static void ShowHelp()
		{
			Console.Clear();

			Console.WriteLine("Esc - exit; U - user info; L - login; A - get animals; I - add animal; F - feed; P - play");
			Console.SetCursorPosition(0, 1);
		}

		internal static void ShowFlowTip()
		{
			Console.WriteLine("\nPress any key to continue...");
			Console.ReadKey();
		}

		internal static void PrintInfo<T>(IEnumerable<T> result)
		{
			Console.Write("\n");
			if (result.Any())
			{
				foreach (var a in result)
				{
					Console.WriteLine(a.ToString());
				}
			}
			else
			{
				Console.WriteLine("Can't get info for user with your id specified in command line");
			}
		}
	}
}