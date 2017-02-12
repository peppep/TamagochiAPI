using System;

namespace TamagochiAPI.Client
{
	public class Program
	{
		internal static string Address = "http://localhost:8080";

		public static void Main(string[] args)
		{
			if (args.Length < 1)
			{
				Console.WriteLine("Not enough args to run. Sample: TamagochiAPI.Client.exe 1\nPress any key...");
				Console.ReadKey();
				return;
			}
			var userId = uint.Parse(args[0]);

			var gameInstance = new Game(userId);
			gameInstance.Run();
		}
	}
}