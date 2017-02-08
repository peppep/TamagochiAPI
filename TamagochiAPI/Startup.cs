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
	public partial class Startup
  {
    public void Configuration(IAppBuilder app)
    {
			var config = new HttpConfiguration();

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