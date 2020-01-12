using System;
using System.Threading;

namespace monit
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				Console.WriteLine("Hostname: {0}", Metrics.systemName);
				Console.WriteLine("OS Name: {0}", Metrics.OSName);
				Console.WriteLine("OS Version: {0}", Metrics.OSVersion);
				Console.WriteLine("OS Version Details: {0}", Metrics.OSFullVersion);
				Console.WriteLine("CPU Architecture: {0}", Metrics.arch);
				Console.WriteLine("Processor Count: {0}", Metrics.processorCount);
				Console.WriteLine("Memory: {0} / Virtual: {1}", Metrics.totalMemory, Metrics.totalVirtualMemory);
				Console.WriteLine("Free Memory: {0} / Virtual: {1}", Metrics.freeMemory, Metrics.freeVirtualMemory);
				Console.WriteLine("Used Memory: {0} / Virtual: {1}", Metrics.totalMemory - Metrics.freeMemory, Metrics.totalVirtualMemory - Metrics.freeVirtualMemory);
				double memPercent = ((double)Metrics.freeMemory / (double)Metrics.totalMemory) * 100.0;
				double virtPercent = ((double)Metrics.freeVirtualMemory / (double)Metrics.totalVirtualMemory) * 100.0;
				Console.WriteLine("Free Memory: {0} / Virtual: {1}", memPercent, virtPercent);
				memPercent = ((double)(Metrics.totalMemory - Metrics.freeMemory) / (double)Metrics.totalMemory) * 100.0;
				virtPercent = ((double)(Metrics.totalVirtualMemory - Metrics.freeVirtualMemory) / (double)Metrics.totalVirtualMemory) * 100.0;
				Console.WriteLine("Used Memory: {0} / Virtual: {1}", memPercent, virtPercent);
				//for (int i = 0; i < 10; i++)
				{
					var usage = Metrics.cpuUsage;
					Console.WriteLine("CPU %: User: {0} + System: {1} = Total: {2}", usage.Item1, usage.Item2, usage.Item1 + usage.Item2);
					Thread.Sleep(500);
				}

				Console.WriteLine("press ENTER to continue");
				Console.ReadLine();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}
