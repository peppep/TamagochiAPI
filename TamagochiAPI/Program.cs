using Microsoft.Owin.Hosting;

namespace TamagochiAPI
{
	public class Program
	{
		static void Main(string[] argv)
		{
			var address = "http://localhost:8080";
			using (var owin = WebApp.Start<Startup>(address))
			{
				System.Console.ReadKey();
			}
		}
	}
}