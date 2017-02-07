using Microsoft.Owin.Hosting;
using System;
using System.Diagnostics;
using Topshelf;

namespace TamagochiAPI
{
	public class HostingConfiguration : ServiceControl
	{
		private IDisposable _webApplication;

		public bool Start(HostControl hostControl)
		{
			Trace.WriteLine("Starting the service");
			_webApplication = WebApp.Start<Startup>("http://localhost:8080");
			return true;
		}

		public bool Stop(HostControl hostControl)
		{
			_webApplication.Dispose();
			return true;
		}
	}

	public class Program
	{
		static int Main(string[] argv)
		{
			//var address = "http://localhost:8080";
			//using (var owin = WebApp.Start<Startup>(address))
			//{
			//	System.Console.ReadKey();
			//}
			var exitCode = HostFactory.Run(x =>
			{
				x.Service<HostingConfiguration>();
				x.RunAsLocalSystem();
				x.SetDescription("Owin + Webapi as Windows service");
				x.SetDisplayName("owin.webapi.test");
				x.SetServiceName("owin.webapi.test");
			});
			return (int)exitCode;
		}
	}
}