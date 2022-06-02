using System;
using System.Text.RegularExpressions;

namespace jlib
{

	class Time
	{

		public static readonly DateTime UnixEpoch       = new DateTime(1970, 1, 1, 0, 0, 0);
		
		public static readonly string StandardTimeRegex = "^[0-9][0-9][0-9][0-9]-[0-9][0-9]-[0-9][0-9] [0-9][0-9]:[0-9][0-9]:[0-9][0-9]";

		public static readonly string StandardFormatter = "yyyy-MM-dd HH:mm:ss";

		public static DateTime GetDateTime()
		{
			return DateTime.Now.ToUniversalTime();
		}

		/// Get local timezone milli seconds
		public static long GetMilliSeconds()
		{
			return Convert(DateTime.Now);
		}

		public static long Convert(DateTime dt)
		{
			return ((dt - UnixEpoch).Ticks / TimeSpan.TicksPerSecond) * 1000;
		}

		///    utc(ms) ->    utc(DateTime)
		/// +8zone(ms) -> +8zone(DateTime)
		public static DateTime Convert(long ms)
		{
			return TimeZoneInfo.ConvertTime(UnixEpoch, TimeZoneInfo.FindSystemTimeZoneById(TimeZoneInfo.Local.Id)).AddSeconds(ms / 1000);
		}
		
		public static string Format(long ms)
		{
			return Convert(ms).ToString(StandardFormatter);
		}

		public static bool Match(string formatter, string s)
		{
			MatchCollection mc = Regex.Matches(s, formatter);
			foreach (Match m in mc)
			{
				return true;
			}
			return false;
		}

		/// @param  s : datetime string, like 1970-01-01 00:00:00
		/// @return   : Get utc milli seconds
		public static long Parse(string s)
		{
			if (s.Length != 19 || !Match(StandardTimeRegex, s))
			{
				throw new Exception($"Datetime format error: {s}, expect \"{StandardFormatter}\".");
			}
			string replaced = $"{s.Substring(5, 2)}/{s.Substring(8, 2)}/{s.Substring(0, 4)} {s.Substring(11, 8)}";
			return Convert(DateTime.Parse(replaced).ToUniversalTime());
		}

	}

}