using System;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading;

namespace monit
{
	public abstract class Metrics
	{

		[StructLayout(LayoutKind.Sequential)]
		private class MEMORYSTATUSEX
		{
			public uint dwLength;
			public uint dwMemoryLoad;
			public ulong ullTotalPhys;
			public ulong ullAvailPhys;
			public ulong ullTotalPageFile;
			public ulong ullAvailPageFile;
			public ulong ullTotalVirtual;
			public ulong ullAvailVirtual;
			public ulong ullAvailExtendedVirtual;
			public MEMORYSTATUSEX()
			{
				this.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
			}
		}
		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool GlobalMemoryStatusEx([In, Out]MEMORYSTATUSEX lpBuffer);

		static Metrics()
		{
			update();
		}

		public static long totalMemory { get; private set; } = 0;
		public static long totalVirtualMemory { get; private set; } = 0;
		public static long freeMemory { get; private set; } = 0;
		public static long freeVirtualMemory { get; private set; } = 0;
		public static string OSName { get; private set; } = Environment.OSVersion.Platform.ToString();
		public static string OSVersion { get; private set; } = Environment.OSVersion.Version.ToString();

		public static string OSFullVersion
		{
			get
			{
				return Environment.OSVersion.VersionString;
			}
		}

		public static string systemName
		{
			get
			{
				return Environment.MachineName;
			}
		}

		public static int processorCount
		{
			get
			{
				return Environment.ProcessorCount;
			}
		}

		public static string arch
		{
			get
			{
				switch (System.Runtime.InteropServices.RuntimeInformation.OSArchitecture)
				{
					case System.Runtime.InteropServices.Architecture.X86:
						return "x86";
					case System.Runtime.InteropServices.Architecture.X64:
						return "x86_64";
					case System.Runtime.InteropServices.Architecture.Arm:
						return "arm";
					case System.Runtime.InteropServices.Architecture.Arm64:
						return "arm64";
					default:
						return "";
				}
			}
		}

		public static Tuple<float, float> cpuUsage
		{
			get
			{

				ManagementClass mc = new ManagementClass("Win32_PerfFormattedData_PerfOS_Processor");
				ManagementObjectCollection moc = mc.GetInstances();
				foreach (ManagementObject item in moc)
				{
					if (item.Properties["Name"].Value.ToString() != "_Total")
						continue;
					float cpuIdle = Convert.ToSingle(item.Properties["PercentIdleTime"].Value);
					float cpuUser = Convert.ToSingle(item.Properties["PercentUserTime"].Value);
					float cpuTotal = 100.0f - cpuIdle;
					float cpuSystem = cpuTotal - cpuUser;
					return new Tuple<float, float>(cpuUser, cpuSystem);
				}

				return new Tuple<float, float>(0, 0);
			}
		}

		public static void update()
		{
			ManagementClass mc = new ManagementClass("Win32_OperatingSystem");
			ManagementObjectCollection moc = mc.GetInstances();
			foreach (ManagementObject item in moc)
			{
				OSName = item.Properties["Caption"].Value.ToString();
				OSVersion = item.Properties["Version"].Value.ToString();
			}

			MEMORYSTATUSEX stat = new MEMORYSTATUSEX();
			GlobalMemoryStatusEx(stat);

			totalMemory = Convert.ToInt64(stat.ullTotalPhys / 1024);
			freeMemory = Convert.ToInt64(stat.ullAvailPhys / 1024);
			totalVirtualMemory = Convert.ToInt64(stat.ullTotalPageFile / 1024);
			freeVirtualMemory = Convert.ToInt64(stat.ullAvailPageFile / 1024);

			Tuple<float, float> cpuTemp = cpuUsage;
		}
	}
}
