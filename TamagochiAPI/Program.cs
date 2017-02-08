using Microsoft.Owin.Hosting;
using System;
using System.Diagnostics;
using Topshelf;
using Topshelf.Ninject;
using Topshelf.ServiceConfigurators;

namespace TamagochiAPI
{
	public class HostConfiguration
	{
		private IDisposable _webApplication;

		public void Start()
		{
			Trace.WriteLine("Starting the service");
			_webApplication = WebApp.Start<Startup>("http://localhost:8080");
		}

		public void Stop()
		{
			_webApplication.Dispose();
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
				x.UseNinject();

				x.Service<HostConfiguration>(service =>
				{
					ServiceConfigurator<HostConfiguration> s = service;
					s.ConstructUsing(() => new HostConfiguration());
					s.WhenStarted(a => a.Start());
					s.WhenStopped(a => a.Stop());
				});

				x.RunAsLocalSystem();
				x.SetDescription("Owin + Webapi as Windows service");
				x.SetDisplayName("owin.webapi.test");
				x.SetServiceName("owin.webapi.test");
			});

			return (int)exitCode;
		}
	}
}