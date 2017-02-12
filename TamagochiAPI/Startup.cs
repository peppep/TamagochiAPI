using Microsoft.Owin;
using Ninject;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using Owin;
using System.Web.Http;
using TamagochiAPI.Configs;
using TamagochiAPI.DAL.SQLite.Systems;
using TamagochiAPI.DAL.Wrappers;
using TamagochiAPI.Services;

[assembly: OwinStartup(typeof(TamagochiAPI.Startup))]

namespace TamagochiAPI
{
	using Logger = Common.Log.Log;

	public partial class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			Logger.Info("Configuring...");
			var config = new HttpConfiguration();

			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
								name: "DefaultApi",
								routeTemplate: "api/{controller}/{id}",
								defaults: new { id = RouteParameter.Optional }
						);

			app.UseNinjectMiddleware(CreateKernel).UseNinjectWebApi(config);
		}

		private static IKernel CreateKernel()
		{
			var kernel = new StandardKernel();
			try
			{
				RegisterServices(kernel);

				return kernel;
			}
			catch
			{
				kernel.Dispose();
				throw;
			}
		}

		private static void RegisterServices(IKernel kernel)
		{
			Logger.Info("Registering dependencies...");
			kernel.Bind<IUserService>().To<UsersService>();
			kernel.Bind<IAnimalService>().To<AnimalService>();
			kernel.Bind<IConfigService>().To<ConfigService>();

			//DAL
			kernel.Bind<IUsersWrapper>().To<UsersWrapper>();
			kernel.Bind<IAnimalsWrapper>().To<AnimalsWrapper>();

			//DAL.SQLite
			kernel.Bind<IUserSystem>().To<UsersSystem>();
			kernel.Bind<IAnimalsSystem>().To<AnimalsSystem>();
		}
	}
}