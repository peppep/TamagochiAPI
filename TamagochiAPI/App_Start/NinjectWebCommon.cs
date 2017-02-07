[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(TamagochiAPI.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(TamagochiAPI.App_Start.NinjectWebCommon), "Stop")]

namespace TamagochiAPI.App_Start
{
	using Ninject.Web.Common;
	public static class NinjectWebCommon 
{
		private static readonly Bootstrapper bootstrapper = new Bootstrapper();

		/// <summary>
		/// Starts the application
		/// </summary>
		public static void Start() 
		{
		}
        
		/// <summary>
		/// Stops the application.
		/// </summary>
		public static void Stop()
		{
		}
	}
}