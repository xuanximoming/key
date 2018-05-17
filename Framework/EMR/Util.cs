using DevExpress.LookAndFeel;
using System;
using System.Runtime.InteropServices;
using DrectSoft.Common.Eop;
using DrectSoft.Core;

namespace DrectSoft
{
	public static class Util
	{
		internal struct SystemTime
		{
			internal ushort wYear;

			internal ushort wMonth;

			internal ushort wDayOfWeek;

			internal ushort wDay;

			internal ushort wHour;

			internal ushort wMinute;

			internal ushort wSecond;

			internal ushort wMilliseconds;
		}

		private static AppConfigReader m_cfgreader = new AppConfigReader();

		[DllImport("user32.dll")]
		private static extern bool GetLastInputInfo(ref LastInputInfo plii);

		public static double GetLastInputTime()
		{
			LastInputInfo lastInputInfo = default(LastInputInfo);
			lastInputInfo.cbSize = Marshal.SizeOf(lastInputInfo);
			double result;
			if (!Util.GetLastInputInfo(ref lastInputInfo))
			{
				result = 0.0;
			}
			else
			{
				result = (double)((long)Environment.TickCount - (long)((ulong)lastInputInfo.dwTime)) / 1000.0 / 60.0;
			}
			return result;
		}

		[DllImport("kernel32", SetLastError = true)]
		private static extern bool GetSystemTime(out Util.SystemTime systemTime);

		[DllImport("kernel32", SetLastError = true)]
		private static extern bool SetSystemTime(ref Util.SystemTime systemTime);

		public static void Set2LocalSystemTime(DateTime utcTime)
		{
			Util.SystemTime systemTime;
			systemTime.wYear = (ushort)utcTime.Year;
			systemTime.wMonth = (ushort)utcTime.Month;
			systemTime.wDayOfWeek = (ushort)utcTime.DayOfWeek;
			systemTime.wDay = (ushort)utcTime.Day;
			systemTime.wHour = (ushort)utcTime.Hour;
			systemTime.wMinute = (ushort)utcTime.Minute;
			systemTime.wSecond = (ushort)utcTime.Second;
			systemTime.wMilliseconds = (ushort)utcTime.Millisecond;
			Util.SetSystemTime(ref systemTime);
		}

		public static EmrDefaultSetting InitEmrDefaultSet()
		{
			EmrAppConfig config = Util.m_cfgreader.GetConfig("EmrDefaultSet");
			EmrDefaultSetting result;
			if (config != null)
			{
				result = new EmrDefaultSetting(config.Config);
			}
			else
			{
				result = null;
			}
			return result;
		}

		public static void InitSkin()
		{
			EmrAppConfig config = Util.m_cfgreader.GetConfig("DevSkin");
			if (config != null)
			{
				Util.SetSkin(config.Config);
			}
			else
			{
				Util.SetSkin(string.Empty);
			}
		}

		public static void InitSkin(string userId)
		{
			EmrUserConfig config = Util.m_cfgreader.GetConfig(userId, "DevSkin");
			if (config != null)
			{
				EmrAppConfig defaultConfig = config.GetDefaultConfig();
				if (defaultConfig != null)
				{
					Util.SetSkin(defaultConfig.Config);
					return;
				}
			}
			Util.SetSkin(string.Empty);
		}

		internal static void SetSkin(string skinName)
		{
			int num;
			if (string.IsNullOrEmpty(skinName))
			{
				Util.SetDevStdStyle(DevStyle.Default);
			}
			else if (int.TryParse(skinName, out num))
			{
				DevStyle devStdStyle = (DevStyle)num;
				Util.SetDevStdStyle(devStdStyle);
			}
			else
			{
				Util.SetSkinStyle(skinName);
			}
		}

		internal static void SetSkinStyle(string skinName)
		{
			UserLookAndFeel.Default.SetSkinStyle(skinName);
		}

		internal static void SetDevStdStyle(DevStyle devstyle)
		{
			switch (devstyle)
			{
			case DevStyle.Default:
				UserLookAndFeel.Default.SetDefaultStyle();
				break;
			case DevStyle.WindowsXP:
				UserLookAndFeel.Default.SetWindowsXPStyle();
				break;
			case DevStyle.OfficeXP:
				UserLookAndFeel.Default.SetUltraFlatStyle();
				break;
			case DevStyle.Office2000:
				UserLookAndFeel.Default.SetFlatStyle();
				break;
			case DevStyle.Office2003:
				UserLookAndFeel.Default.SetOffice2003Style();
				break;
			}
		}
	}
}
