using System;
using System.Runtime.InteropServices;
using DrectSoft.Common.Eop;
using DrectSoft.Core;

namespace DrectSoft
{
    /// <summary>
    /// system auto lock
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct LastInputInfo
    {
        [MarshalAs(UnmanagedType.U4)]
        public int cbSize;
        [MarshalAs(UnmanagedType.U4)]
        public uint dwTime;
    }

    public static class Util
    {


        #region 系统自动锁定
        [DllImport("user32.dll")]
        static extern bool GetLastInputInfo(ref LastInputInfo plii);

        /// <summary>
        /// get inactive time (min)
        /// </summary>
        /// <returns></returns>
        public static Double GetLastInputTime()
        {
            LastInputInfo vLastInputInfo = new LastInputInfo();
            vLastInputInfo.cbSize = Marshal.SizeOf(vLastInputInfo);
            if (!GetLastInputInfo(ref vLastInputInfo)) return 0;
            return (double)(Environment.TickCount - (long)vLastInputInfo.dwTime) / 1000 / 60;
        }

        #endregion


        #region 更新本地器时间

        [System.Runtime.InteropServices.DllImport("kernel32", SetLastError = true)]
        private static extern bool GetSystemTime(out SystemTime systemTime);
        [System.Runtime.InteropServices.DllImport("kernel32", SetLastError = true)]
        private static extern bool SetSystemTime(ref SystemTime systemTime);

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

        /// <summary>
        /// 更改本机时间
        /// </summary>
        /// <param name="utcTime"></param>
        public static void Set2LocalSystemTime(DateTime utcTime)
        {
            SystemTime stt;
            stt.wYear = (ushort)utcTime.Year;
            stt.wMonth = (ushort)utcTime.Month;
            stt.wDayOfWeek = (ushort)utcTime.DayOfWeek;
            stt.wDay = (ushort)utcTime.Day;
            stt.wHour = (ushort)utcTime.Hour;
            stt.wMinute = (ushort)utcTime.Minute;
            stt.wSecond = (ushort)utcTime.Second;
            stt.wMilliseconds = (ushort)utcTime.Millisecond;
            SetSystemTime(ref stt);
        }
        #endregion


        static AppConfigReader m_cfgreader = new AppConfigReader();

        /// <summary>
        /// 设置编辑器默认值
        /// </summary>
        public static EmrDefaultSetting InitEmrDefaultSet()
        {
            EmrAppConfig eac = m_cfgreader.GetConfig(BasicSettings.EmrDefaultSetting);
            if (eac != null)
            {
                return new EmrDefaultSetting(eac.Config);
            }
            return null;
        }

        /// <summary>
        /// 设置皮肤
        /// </summary>
        public static void InitSkin()
        {
            //string skinName = ConfigurationManager.AppSettings["DevSkin"];
            EmrAppConfig eac = m_cfgreader.GetConfig(BasicSettings.DevSkinConfigKey);
            if (eac != null)
            {
                SetSkin(eac.Config);
                return;
            }
            SetSkin(string.Empty);
        }

        public static void InitSkin(string userId)
        {
            EmrUserConfig euc = m_cfgreader.GetConfig(userId, BasicSettings.DevSkinConfigKey);
            if (euc != null)
            {
                EmrAppConfig eac = euc.GetDefaultConfig();
                if (eac != null)
                {
                    SetSkin(eac.Config);
                    return;
                }
            }
            SetSkin(string.Empty);
        }

        internal static void SetSkin(string skinName)
        {
            if (string.IsNullOrEmpty(skinName))
            {
                SetDevStdStyle(DevStyle.Default);
            }
            else
            {
                int istyle;
                if (int.TryParse(skinName, out istyle))
                {
                    DevStyle style = (DevStyle)istyle;
                    SetDevStdStyle(style);
                }
                else
                    SetSkinStyle(skinName);
            }
        }

        internal static void SetSkinStyle(string skinName)
        {
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(skinName);
        }

        internal static void SetDevStdStyle(DevStyle devstyle)
        {
            switch (devstyle)
            {
                case DevStyle.Default:
                    DevExpress.LookAndFeel.UserLookAndFeel.Default.SetDefaultStyle();
                    break;
                case DevStyle.WindowsXP:
                    DevExpress.LookAndFeel.UserLookAndFeel.Default.SetWindowsXPStyle();
                    break;
                case DevStyle.OfficeXP:
                    DevExpress.LookAndFeel.UserLookAndFeel.Default.SetUltraFlatStyle();
                    break;
                case DevStyle.Office2000:
                    DevExpress.LookAndFeel.UserLookAndFeel.Default.SetFlatStyle();
                    break;
                case DevStyle.Office2003:
                    DevExpress.LookAndFeel.UserLookAndFeel.Default.SetOffice2003Style();
                    break;
                default:
                    break;
            }
        }

    }

    /// <summary>
    /// dev的非Skin样式
    /// </summary>
    public enum DevStyle
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default = 0,

        /// <summary>
        /// WindowsXP
        /// </summary>
        WindowsXP = 1,

        /// <summary>
        /// OfficeXP
        /// </summary>
        OfficeXP = 2,

        /// <summary>
        /// Office2000
        /// </summary>
        Office2000 = 3,

        /// <summary>
        /// Office2003
        /// </summary>
        Office2003 = 4,
    }
}
