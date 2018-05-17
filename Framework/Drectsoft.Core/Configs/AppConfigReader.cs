using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.Core
{
    /// <summary>
    /// 配置读取类
    /// </summary>
    public class AppConfigReader:IAppConfigReader
    {
        AppConfigDalc _acd = new AppConfigDalc();
        static AppConfigReader _instance = new AppConfigReader();

        /// <summary>
        /// 构造
        /// </summary>
        public AppConfigReader()
        {
        }

        #region read config

        /// <summary>
        /// 读取用户配置
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public EmrUserConfig GetConfig(string userId, string key)
        {
            return null;
        }

        /// <summary>
        /// 读取配置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public EmrAppConfig GetConfig(string key)
        {
            return _acd.SelectAppConfig(key);
        }

        /// <summary>
        /// 读取用户配置对象
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetConfigObj(string userId, string key)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        private object InternalGetConfigObj(EmrAppConfig eac)
        {
            //string assInfo = eac.DesignClass;
            //IAppConfigDesign iacd = null;
            //if (string.IsNullOrEmpty(assInfo))
            //{
            //    //用通用的设置类，返回string类型的config值
            //    return eac.Config;
            //}
            //else
            //{
            //    Type t = Type.GetType(assInfo);
            //    if (t != null)
            //    {
            //        iacd = (IAppConfigDesign)Activator.CreateInstance(t);
            //    }
            //    if (iacd == null) throw new ArgumentException("design assembly load fail! ");
            //    Dictionary<string, EmrAppConfig> dics = new Dictionary<string, EmrAppConfig>();
            //    dics.Add(eac.Key, eac);
            //    iacd.Load(null, dics);
            //    return iacd.ConfigObj;
            //}
            //todo
            return null;
        }

        /// <summary>
        /// 读取配置对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetConfigObj(string key)
        {
            EmrAppConfig eac = _acd.SelectAppConfig(key);
            return InternalGetConfigObj(eac);
        }

        /// <summary>
        /// 读取一组用户配置
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public Dictionary<string, EmrUserConfig> GetConfigs(string userId, params string[] keys)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// 读取一组配置
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public Dictionary<string, EmrAppConfig> GetConfigs(params string[] keys)
        {
            return _acd.SelectAppConfigs(keys);
        }
        #endregion

        #region Static Methods Interface

        /// <summary>
        /// 取得指定的配置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static EmrAppConfig GetAppConfig(string key)
        {
            return _instance.GetConfig(key);
        }

        /// <summary>
        /// 取得指定的配置对象实例
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetAppConfigObj(string key)
        {
            return _instance.GetConfigObj(key);
        }

        #endregion

    }
}
