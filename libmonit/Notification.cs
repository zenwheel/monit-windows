using System;
using System.Net;
using System.Text;

namespace monit
{
	public class Notification
	{
		private readonly long incarnation = Monit.now;
		private string server;
		private string auth;
		private string xmlPrefix;
		private readonly string xmlSuffix = "</monit>";

		public Notification(string server, string username, string password, string instanceId)
		{
			this.server = server;

			auth = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", username, password)));
			xmlPrefix = string.Format("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?><monit id=\"{0}\" incarnation=\"{1}\" version=\"{2}\">", instanceId, incarnation, Monit.version);
		}

		private string serverSection
		{
			get
			{
				return string.Format("<server><uptime>0</uptime><poll>60</poll><startdelay>0</startdelay><localhostname>{0}</localhostname><controlfile>/etc/monitrc</controlfile></server>", Metrics.systemName);
			}
		}

		private string platformSection
		{
			get
			{
				return string.Format("<platform><name>{0}</name><release>{1}</release><version>{2}</version><machine>{3}</machine><cpu>{4}</cpu><memory>{5}</memory><swap>{6}</swap></platform>",
					Metrics.OSName,
					Metrics.OSVersion,
					Metrics.OSFullVersion,
					Metrics.arch,
					Metrics.processorCount,
					Metrics.totalMemory, Metrics.totalVirtualMemory);
			}
		}

		private string servicesSection
		{
			get
			{
				long memUsed = Metrics.totalMemory - Metrics.freeMemory;
				long virtUsed = Metrics.totalVirtualMemory - Metrics.freeVirtualMemory;
				double memPercent = ((double)memUsed / (double)Metrics.totalMemory) * 100.0;
				double virtPercent = ((double)virtUsed / (double)Metrics.totalVirtualMemory) * 100.0;

				return string.Format("<services><service name=\"{0}\"><type>5</type><collected_sec>{1}</collected_sec><collected_usec>{2}</collected_usec><status>0</status><status_hint>0</status_hint><monitor>1</monitor><monitormode>0</monitormode><onreboot>0</onreboot><pendingaction>0</pendingaction><system><cpu><user>{3}</user><system>{4}</system></cpu><memory><percent>{5}</percent><kilobyte>{6}</kilobyte></memory><swap><percent>{7}</percent><kilobyte>{8}</kilobyte></swap></system></service></services>",
					Metrics.systemName, Monit.now, Monit.usec,
					Metrics.cpuUsage.Item1, Metrics.cpuUsage.Item2,
					memPercent, memUsed,
					virtPercent, virtUsed);
			}
		}

		private string initialServicesSection
		{
			get
			{
				return string.Format("<services><service name=\"{0}\"><type>5</type><collected_sec>{1}</collected_sec><collected_usec>{2}</collected_usec><status>0</status><status_hint>0</status_hint><monitor>2</monitor><monitormode>0</monitormode><onreboot>0</onreboot><pendingaction>0</pendingaction></service></services>",
					Metrics.systemName, Monit.now, Monit.usec);
			}
		}

		private string serviceGroupsSection = "<servicegroups/>";

		private string startEvent
		{
			get
			{
				return string.Format("<event><collected_sec>{0}</collected_sec><collected_usec>{1}</collected_usec><service>Monit</service><type>5</type><id>65536</id><state>2</state><action>6</action><message><![CDATA[Monit 5.26.0 started]]></message></event>", Monit.now, Monit.usec);
			}
		}

		private string stopEvent
		{
			get
			{
				return string.Format("<event><collected_sec>{0}</collected_sec><collected_usec>{1}</collected_usec><service>Monit</service><type>5</type><id>65536</id><state>2</state><action>3</action><message><![CDATA[Monit 5.26.0 stopped]]></message></event>", Monit.now, Monit.usec);
			}
		}

		private enum UpdateType
		{
			start,
			stop,
			normal
		}

		private void update(UpdateType updateType)
		{
			Metrics.update();

			using (WebClient wc = new WebClient())
			{
				StringBuilder xmlBuilder = new StringBuilder();

				xmlBuilder.Append(xmlPrefix);
				xmlBuilder.Append(serverSection);
				xmlBuilder.Append(platformSection);
				switch (updateType)
				{
					case UpdateType.start:
						xmlBuilder.Append(initialServicesSection);
						break;
					default:
						xmlBuilder.Append(servicesSection);
						break;
				}
				xmlBuilder.Append(serviceGroupsSection);
				switch (updateType)
				{
					case UpdateType.start:
						xmlBuilder.Append(startEvent);
						break;
					case UpdateType.stop:
						xmlBuilder.Append(stopEvent);
						break;
					default:
						break;

				}
				xmlBuilder.Append(xmlSuffix);

				string xml = xmlBuilder.ToString();

				Uri url = new Uri(string.Format("{0}/collector", server));
				wc.Headers[HttpRequestHeader.Host] = url.Host;
				wc.Headers[HttpRequestHeader.ContentType] = "text/xml";
				wc.Headers[HttpRequestHeader.Pragma] = "no-cache";
				wc.Headers[HttpRequestHeader.Accept] = "*/*";
				wc.Headers[HttpRequestHeader.UserAgent] = string.Format("Monit/{0}", Monit.version);
				wc.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", auth);

				Console.WriteLine("<-- Request: {0}", xml);

				string result = wc.UploadString(url, xml);
				if (string.IsNullOrEmpty(result) == false)
					Console.WriteLine("--> Result: {0}", result);
			}
		}

		public void start()
		{
			update(UpdateType.start);
		}

		public void update()
		{
			update(UpdateType.normal);
		}

		public void stop()
		{
			update(UpdateType.stop);
		}
	}
}
