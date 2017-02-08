using Microsoft.Owin.Hosting;

namespace TamagochiAPI.Client
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var address = "http://localhost:8080";

			using (var owin = WebApp.Start<Startup>(address))
			{

			}
		}
	}
}
