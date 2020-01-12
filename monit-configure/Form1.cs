using Microsoft.Win32;
using System;
using System.Net;
using System.ServiceProcess;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace monit
{
	public partial class Form1 : Form
	{
		private string instanceId;

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\monit"))
			{
				serverText.Text = key.GetValue("server")?.ToString() ?? "";
				userText.Text = key.GetValue("username")?.ToString() ?? "";
				instanceId = key.GetValue("instanceId")?.ToString() ?? "";
				object ignoreSslErrors = key.GetValue("ignoreSslErrors");
				if (ignoreSslErrors == null)
					sslCheckBox.Checked = false;
				else
					sslCheckBox.Checked = Convert.ToBoolean(ignoreSslErrors);

				if (string.IsNullOrWhiteSpace(instanceId))
				{
					instanceId = Monit.generateInstanceId();
					key.SetValue("instanceId", instanceId);
				}

				string password = key.GetValue("password")?.ToString() ?? "";
				if (string.IsNullOrWhiteSpace(password))
					passwordText.Text = "";
				else
				{
					try
					{
						passwordText.Text = Password.DecryptString(instanceId, password);
					}
					catch { passwordText.Text = ""; }
				}
			}
		}

		private static ServiceController GetService(string serviceName)
		{
			ServiceController[] services = ServiceController.GetServices();
			foreach(ServiceController service in services)
			{
				Console.WriteLine(service.ServiceName);
				if (service.ServiceName.ToLowerInvariant() == serviceName.ToLowerInvariant())
					return service;
			}
			return null;
		}

		private delegate void ShowMessageDelegate(string text);
		private void ShowMessage(string text)
		{
			if (this.InvokeRequired)
			{
				var d = new ShowMessageDelegate(ShowMessage);
				this.Invoke(d, new object[] { text });
			}
			else
				MessageBox.Show(text);
		}

		private void saveButton_Click(object sender, EventArgs e)
		{
			if (Regex.Match(serverText.Text, "^https?://").Success == false)
				serverText.Text = string.Format("http://{0}", serverText.Text);
			serverText.Text = Regex.Replace(serverText.Text, "/*$", "");

			try
			{
				using (WebClient wc = new WebClient())
				{
					wc.DownloadString(serverText.Text);
				}
			}
			catch (Exception ex)
			{
				ShowMessage(ex.Message);
				return;
			}
			using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\monit"))
			{
				key.SetValue("server", serverText.Text);
				key.SetValue("username", userText.Text);
				key.SetValue("password", Password.EncryptString(instanceId, passwordText.Text));
				key.SetValue("ignoreSslErrors", sslCheckBox.Checked, RegistryValueKind.DWord);
			}

			new Thread(() =>
			{

				try
				{
					ServiceController service = GetService("monit");
					if (service != null)
					{
						if (service.Status == ServiceControllerStatus.Running)
						{
							service.Stop();
							service.WaitForStatus(ServiceControllerStatus.Stopped);
						}
						service.Start();
						service.WaitForStatus(ServiceControllerStatus.Running);
					}
				}
				catch (Exception ex)
				{
					ShowMessage(ex.Message);
					return;
				}
				Application.Exit();
			}).Start();
		}
	}
}
