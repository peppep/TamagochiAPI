[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(TamagochiAPI.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(TamagochiAPI.App_Start.NinjectWebCommon), "Stop")]

namespace TamagochiAPI.App_Start
{
	public static class NinjectWebCommon
	{
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