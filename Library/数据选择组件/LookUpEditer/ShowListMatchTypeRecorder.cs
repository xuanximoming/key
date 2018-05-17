using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace DrectSoft.Common.Library {
    /// <summary>
    /// showlist 匹配方式记录器
    /// 按用户记录对不同字典的查询方式的设置，保存在注册表中(在主程序中，在登录后，应设置当前用户属性)
    /// </summary>
    public static class ShowListMatchTypeRecorder {
        #region const variables
        private const string ApplicationRegKey = @"SOFTWARE\ & DrectSoft\ShowList";
        #endregion

        #region properties
        /// <summary>
        /// 当前使用ShowList的用户
        /// </summary>
        public static string CurrentUserId {
            get { return _currentUserId; }
            set {
                _currentUserId = value;
            }
        }
        private static string _currentUserId;

        private static Dictionary<string, ShowListMatchTypeSetting> CurrentUserSetting {
            get {
                if (!m_SettingCache.ContainsKey(CurrentUserId)) {
                    m_SettingCache.Add(CurrentUserId, new Dictionary<string, ShowListMatchTypeSetting>());

                    RegistryKey regKey = Registry.LocalMachine.OpenSubKey(CurrentUserRegFullKey);
                    try {
                        if (regKey == null) {
                            regKey = Registry.LocalMachine.CreateSubKey(CurrentUserRegFullKey);
                        }

                        foreach (string valueName in regKey.GetValueNames())
                            m_SettingCache[CurrentUserId].Add(valueName, new ShowListMatchTypeSetting(regKey.GetValue(valueName).ToString()));
                    }
                    finally {
                        regKey.Close();
                    }
                }
                return m_SettingCache[CurrentUserId];
            }
        }

        private static string CurrentUserRegFullKey { get { return ApplicationRegKey + "\\" + CurrentUserId; } }

        private static ShowListMatchTypeSetting DefaultMatchSetting {
            get {
                if (_defaultMatchSetting == null)
                    _defaultMatchSetting = new ShowListMatchTypeSetting();
                return _defaultMatchSetting;
            }
        }
        private static ShowListMatchTypeSetting _defaultMatchSetting;
        #endregion

        #region fields
        private static Dictionary<string, Dictionary<string, ShowListMatchTypeSetting>> m_SettingCache = new Dictionary<string, Dictionary<string, ShowListMatchTypeSetting>>();
        #endregion

        #region internal methods
        /// <summary>
        /// 读取指定字典对应的默认设置
        /// </summary>
        /// <param name="wordbookName"></param>
        /// <returns></returns>
        internal static ShowListMatchTypeSetting ReadDefaultSetting(string wordbookName) {
            if (String.IsNullOrEmpty(CurrentUserId) || String.IsNullOrEmpty(wordbookName))
                return DefaultMatchSetting;
            // 首先检查缓存，如果没有则使用默认值(每个用户的设置是一次读入的)
            if (CurrentUserSetting.ContainsKey(wordbookName))
                return CurrentUserSetting[wordbookName];
            else
                return DefaultMatchSetting;
        }

        /// <summary>
        /// 保存指定字典对应的默认设置。如果与默认的设置相同则不保存
        /// </summary>
        /// <param name="wordbookName"></param>
        /// <param name="matchType"></param>
        /// <param name="isDynamic"></param>
        internal static void WriteSetting(string wordbookName, ShowListMatchType matchType, bool isDynamic) {
            if (String.IsNullOrEmpty(CurrentUserId) || String.IsNullOrEmpty(wordbookName))
                return;

            if ((matchType == DefaultMatchSetting.MatchType) && (isDynamic == DefaultMatchSetting.IsDynamic))
                RemoveSetting(wordbookName);
            else {
                // 缓存中不存在，则添加缓存，插入到注册表
                if (!CurrentUserSetting.ContainsKey(wordbookName)) {
                    CurrentUserSetting.Add(wordbookName, new ShowListMatchTypeSetting(matchType, isDynamic));
                    WriteSettingToReg(wordbookName, CurrentUserSetting[wordbookName]);
                }
                // 检查与缓存中的数据是否一致，不一致时修改缓存，再更新注册表
                else {
                    if ((CurrentUserSetting[wordbookName].IsDynamic != isDynamic)
                       || (CurrentUserSetting[wordbookName].MatchType != matchType)) {
                        CurrentUserSetting[wordbookName].IsDynamic = isDynamic;
                        CurrentUserSetting[wordbookName].MatchType = matchType;
                        WriteSettingToReg(wordbookName, CurrentUserSetting[wordbookName]);
                    }
                }
            }
        }
        #endregion

        #region private methods

        //private static ShowListMatchTypeSetting ReadSettingFromReg(string wordbookName)
        //{
        //   string regValue = Registry.GetValue(CurrentUserRegFullKey, wordbookName, String.Empty);

        //   CurrentUserSetting.Add(wordbookName, new ShowListMatchTypeSetting(regValue));

        //   return CurrentUserSetting[wordbookName];
        //}

        private static void WriteSettingToReg(string wordbookName, ShowListMatchTypeSetting setting) {
            try {
                Registry.SetValue(Registry.LocalMachine + "\\" + CurrentUserRegFullKey, wordbookName, setting.ToString());
            }
            catch { }
        }

        private static void RemoveSetting(string wordbookName) {
            if (!CurrentUserSetting.ContainsKey(wordbookName)) {
                RegistryKey regKey = Registry.LocalMachine.OpenSubKey(CurrentUserRegFullKey, true);
                if (regKey == null) return;
                try {

                    regKey.DeleteValue(wordbookName);
                }
                catch { }
                finally {
                    regKey.Close();
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// showList 匹配方式设置类
    /// </summary>
    internal class ShowListMatchTypeSetting {

        /// <summary>
        /// 匹配模式
        /// </summary>
        public ShowListMatchType MatchType {
            get { return _matchType; }
            set { _matchType = value; }
        }
        private ShowListMatchType _matchType;

        /// <summary>
        /// 动态过滤
        /// </summary>
        public bool IsDynamic {
            get { return _isDynamic; }
            set { _isDynamic = value; }
        }
        private bool _isDynamic;

        /// <summary>
        /// 
        /// </summary>
        public ShowListMatchTypeSetting() {
            MatchType = ShowListMatchType.Any;
            IsDynamic = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matchType"></param>
        /// <param name="isDynamic"></param>
        public ShowListMatchTypeSetting(ShowListMatchType matchType, bool isDynamic) {
            _matchType = matchType;
            _isDynamic = isDynamic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public ShowListMatchTypeSetting(string value)
            : this() {
            if (!String.IsNullOrEmpty(value)) {
                // 拆分保存值
                string[] settings = value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (settings.Length == 2) {
                    _matchType = (ShowListMatchType)Enum.Parse(typeof(ShowListMatchType), settings[0]);
                    _isDynamic = Convert.ToBoolean(settings[1]);
                    return;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return MatchType.ToString() + ";" + IsDynamic.ToString();
        }
    }
}
