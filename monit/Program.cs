using System;
using System.Runtime.InteropServices;
using System.ServiceProcess;

namespace monit
{
	static class Program
	{
#if DEBUG
		private static monitservice service = new monitservice();

		[DllImport("Kernel32")]
		private static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);
		private delegate bool HandlerRoutine(CtrlTypes CtrlType);

		private enum CtrlTypes
		{
			CTRL_C_EVENT = 0,
			CTRL_BREAK_EVENT,
			CTRL_CLOSE_EVENT,
			CTRL_LOGOFF_EVENT = 5,
			CTRL_SHUTDOWN_EVENT
		}

		private static bool ConsoleCtrlCheck(CtrlTypes ctrlType)
		{
			service.StopService();
			return true;
		}

#endif

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main()
		{
#if DEBUG
			SetConsoleCtrlHandler(new HandlerRoutine(ConsoleCtrlCheck), true);
			if (service.InitializeService())
				service.StartService();
			Console.WriteLine("Press ENTER to continue...");
			Console.ReadLine();
#else
			ServiceBase[] ServicesToRun;
			ServicesToRun = new ServiceBase[]
			{
				new monitservice()
			};
			ServiceBase.Run(ServicesToRun);
#endif
		}
	}
}
