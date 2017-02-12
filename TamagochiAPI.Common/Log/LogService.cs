using NLog;

namespace TamagochiAPI.Common.Log
{
	public static class Log
	{
		private static Logger m_logger = LogManager.GetCurrentClassLogger();

		public static void Info(string format, params object[] args)
		{
			m_logger.Info(format, args);
		}

		public static void Warning(string format, params object[] args)
		{
			m_logger.Warn(format, args);
		}

		public static void Error(string format, params object[] args)
		{
			m_logger.Error(format, args);
		}
	}
}