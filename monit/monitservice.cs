using Microsoft.Win32;
using System;
using System.Net;
using System.ServiceProcess;
using System.Threading;

namespace monit
{
	public partial class monitservice : ServiceBase
	{
		private bool m_run = true;
		private ManualResetEvent stopEvent = new ManualResetEvent(false);
		private string server;
		private string username;
		private string password;
		private string instanceId;

		public monitservice()
		{
			InitializeComponent();
		}

		protected override void OnStart(string[] args)
		{
			if (InitializeService() == false)
			{
#if !DEBUG
				Stop();
#endif
				return;
			}
			new Thread(() => StartService()).Start();
		}

		protected override void OnStop()
		{
			StopService();
		}

		public bool InitializeService()
		{
			try
			{
				using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\monit"))
				{
					object ignoreSslErrors = key.GetValue("ignoreSslErrors");
					if (ignoreSslErrors != null)
					{
						if (Convert.ToBoolean(ignoreSslErrors))
							ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, errors) => true;
					}

					server = key.GetValue("server")?.ToString() ?? "";
					username = key.GetValue("username")?.ToString() ?? "";
					instanceId = key.GetValue("instanceId")?.ToString() ?? "";
					string password = key.GetValue("password")?.ToString() ?? "";
					if (string.IsNullOrWhiteSpace(password) == false)
						this.password = Password.DecryptString(instanceId, password);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}

			if (string.IsNullOrEmpty(server) ||
				string.IsNullOrEmpty(username) ||
				string.IsNullOrEmpty(password) ||
				string.IsNullOrEmpty(instanceId))
			{
				Console.WriteLine("Service is not configured");
				return false;
			}
			return true;
		}

		public void StartService()
		{
			Notification notifications = new Notification(server, username, password, instanceId);

			Console.WriteLine("Starting service, press ^C to exit...");
			notifications.start();

			while (m_run)
			{
				notifications.update();
				Console.WriteLine("Waiting...");

				if (stopEvent.WaitOne(60000))
					break;
			}
			Console.WriteLine("Exiting service...");
			notifications.stop();
		}

		public void StopService()
		{
			m_run = false;
			stopEvent.Set();
		}
	}
}
