using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace monit
{
	[RunInstaller(true)]
	public partial class ProjectInstaller : System.Configuration.Install.Installer
	{
		public ProjectInstaller()
		{
			InitializeComponent();
			serviceInstaller1.AfterInstall += ServiceInstaller1_AfterInstall;
		}

		private void ServiceInstaller1_AfterInstall(object sender, InstallEventArgs e)
		{
			ServiceController[] services = ServiceController.GetServices();
			foreach (ServiceController service in services)
			{
				if (service.ServiceName.ToLowerInvariant() == serviceInstaller1.ServiceName.ToLowerInvariant())
					service.Start();
			}
		}
	}
}
