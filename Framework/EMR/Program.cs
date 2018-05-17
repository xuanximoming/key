using System;
using System.Configuration;
using System.Reflection;
using System.Windows.Forms;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.Plugin.Manager;

namespace MainFrame
{
	internal static class Program
	{
		public static string m_StrUserId;

		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			try
			{
				DateTime dateTime = DateTime.Parse("2013-8-15");
				string fullName = Assembly.GetEntryAssembly().FullName;
				AppDomain customAppDomain = Program.GetCustomAppDomain();
				customAppDomain.UnhandledException += new UnhandledExceptionEventHandler(Program.CurrentDomain_UnhandledException);
				customAppDomain.CreateInstanceAndUnwrap(fullName, typeof(StartupClass).FullName, false, BindingFlags.Instance | BindingFlags.Public, null, new object[]
				{
					false,
					"file.menu"
				}, null, null, null);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			MessageBox.Show("应用程序运行异常,请查看日志!");
			if (e.IsTerminating)
			{
				Application.Exit();
			}
		}

		private static AppDomain GetCustomAppDomain()
		{
			PlugInLoadHelper @object = new PlugInLoadHelper(Application.StartupPath);
			DrectSoftConfigurationSectionHandler.SetConfigurationDelegate(new DelegateReadConfiguration(@object.ReadPlugInLoadConfiguration));
			PlugInLoadConfiguration plugInLoadConfiguration = (PlugInLoadConfiguration)ConfigurationManager.GetSection("plugInLoadSettings");
			AppDomainSetup appDomainSetup = new AppDomainSetup();
			appDomainSetup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
			appDomainSetup.ApplicationName = "DrectSoftEMR";
			appDomainSetup.ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
			appDomainSetup.PrivateBinPath = plugInLoadConfiguration.AllPath;
			if (plugInLoadConfiguration.UseShadowCopy)
			{
				appDomainSetup.ShadowCopyFiles = "true";
				appDomainSetup.CachePath = plugInLoadConfiguration.CachePath;
			}
			return AppDomain.CreateDomain("DrectSoftEMR", null, appDomainSetup);
		}
	}
}
