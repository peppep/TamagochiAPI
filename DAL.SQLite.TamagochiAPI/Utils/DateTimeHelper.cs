using System;

namespace TamagochiAPI.DAL.SQLite.Utils
{
	public static class DateTimeHelper
	{
		public static DateTime FromString(this DateTime date, string serialized)
		{
			DateTime res;
			if(DateTime.TryParse(serialized, out res))
			{
				return res;
			}

			return DateTime.MinValue;
		}
	}
}