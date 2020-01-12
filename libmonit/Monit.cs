using System;
using System.Text;

namespace monit
{
	public abstract class Monit
	{
		public static string version { get { return "5.26.0"; } }

		public static string generateInstanceId()
		{
			Random r = new Random();
			byte[] b = new byte[16];
			r.NextBytes(b);
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < b.Length; i++)
				sb.AppendFormat("{0:x2}", b[i]);
			return sb.ToString();
		}

		public static long now
		{
			get
			{
				return ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();

			}
		}

		public static long usec
		{
			get
			{
				return Convert.ToInt64(DateTime.UtcNow.ToString("ffffff"));

			}
		}
	}
}
