using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using DrectSoft.Core.Properties;

namespace DrectSoft.Core {
    /// <summary>
    /// 获取系统级的基础配置。（从数据库中获取，不再通过文件方式读取）
    /// 特殊处理：
    ///   由于ShowList组件在窗口设计器中使用时，仍需要读取相应的配置。
    ///   若要从数据库读取配置，则需要改devenv.exe.config.
    ///   所以，这里使用判断AppDomain的方式来进行特殊处理。
    /// 一般返回Stream对象
    /// </summary>
    public static class BasicSettings {
        #region public const KeyName
        /// <summary>
        /// 医嘱设置
        /// </summary>
        public const string DoctorAdviceSetting = "DoctorAdviceSetting";
        /// <summary>
        /// ORM映射
        /// </summary>
        public const string ORMappingSetting = "ORMapping";
        /// <summary>
        /// 预定义SQL语句
        /// </summary>
        public const string PreDefineSqlSetting = "PreDefineSQL";
        /// <summary>
        /// 数据字典配置
        /// </summary>
        public const string WordbookSetting = "Wordbook";
        /// <summary>
        /// 界面样式
        /// </summary>
        public const string DevSkinConfigKey = "DevSkin";
        /// <summary>
        /// 编辑器默认样式
        /// </summary>
        public const string EmrDefaultSetting = "EmrDefaultSet";
        #endregion

        #region private const variables
        private const string DEVENVAPPDOMAINMANAGER = "Microsoft.VisualStudio.CommonIDE.VsAppDomainManager";

        private const string OrmSettingFile = "DrectSoftORMappings.xml";
        private const string SqlSentenceFile = "PreDefSqlSentence.xml";
        private const string WordbookFile = "DrectSoftWordbooks.xml";
        private const string FrameworkPath = @"D:\SHIS2008\SourceCode\系统框架\Plugins\SHIS\EmrObjectPersistent\XmlFile\";
        private const string WordbookPath = @"D:\SHIS2008\SourceCode\公共系统\数据选择组件\Wordbook\XmlFile\";
        #endregion

        #region private properties
        private static string FullOrmSettingFileName {
            get { return CheckAndGetDataFileFullPath(OrmSettingFile, FrameworkPath); }
        }

        private static string FullSqlSentenceFileName {
            get { return CheckAndGetDataFileFullPath(SqlSentenceFile, FrameworkPath); }
        }

        private static string FullWordbookFileName {
            get {
                return CheckAndGetDataFileFullPath(WordbookFile, WordbookPath);
            }
        }

        private static AppConfigDalc ConfigDalc {
            get {
                if (_configDalc == null)
                    _configDalc = new AppConfigDalc();
                return _configDalc;

            }
        }
        private static AppConfigDalc _configDalc;
        #endregion

        #region private variables
        private static Dictionary<string, object> m_Configs = new Dictionary<string, object>();
        #endregion

        /// <summary>
        /// 获取指定键值对应的参数设置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Stream GetConfig(string key) {

            EmrAppConfig config = ConfigDalc.SelectAppConfig(key);
            if (config != null)
                return new MemoryStream(System.Text.Encoding.UTF8.GetBytes(config.Config));

            return null;
        }

        /// <summary>
        /// 获取String类型的配置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetStringConfig(string key) {
            if (!m_Configs.ContainsKey(key)) {
                EmrAppConfig config = ConfigDalc.SelectAppConfig(key);
                if (config != null)
                    m_Configs.Add(key, config.Config);
                else
                    return String.Empty;
            }

            return m_Configs[key] as string;
        }

        #region private methods
        private static Stream HandleSettingFile(string key) {
            switch (key) {
                //case PreDefineSqlSetting:
                //   return ReadSettingsFile(FullSqlSentenceFileName);
                //case WordbookSetting :
                //   return ReadSettingsFile(FullWordbookFileName);
                case PreDefineSqlSetting:
                    return new MemoryStream(Encoding.UTF8.GetBytes(Resources.PreDefSqlSentence));
                case WordbookSetting:
                    //  string tmppath = System.IO.Path.GetTempFileName();
                    //  FileStream tmpfile = new FileStream(tmppath, FileMode.Create);
                    //byte[] wbs =Encoding.Default.GetBytes(DrectSoft.Core.Properties.Resources.DrectSoftWordbooks);
                    //  tmpfile.Write(wbs, 0, wbs.Length);
                    //  tmpfile.Flush();
                    //  tmpfile.Close();
                    //System.Windows.Forms.MessageBox.Show(tmppath);
                    //  return ReadSettingsFile(tmppath);
                    //System.Windows.Forms.MessageBox.Show(typeof(BasicSettings).Assembly.Location);
                    return new MemoryStream(Encoding.UTF8.GetBytes(Resources.DrectSoftWordbooks));
                case ORMappingSetting:
                    return ReadSettingsFile(FullOrmSettingFileName);
                default:
                    return null;
            }
        }

        private static Stream ReadSettingsFile(string fullFileName) {
            FileStream file = new FileStream(fullFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            return file;
        }

        private static string CheckAndGetDataFileFullPath(string fileName, string path) {
            string fullName = path + fileName;
            // 检查XML文件是否存在
            if (File.Exists(fullName))
                return fullName;
            else
                throw new FileNotFoundException("参数配置文件:" + fullName);
        }
        #endregion
    }

}
