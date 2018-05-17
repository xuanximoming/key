using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DrectSoft.Core
{
   ///// <summary>
   ///// 系统设置接口
   ///// </summary>
   //public interface IAppConfigDesign
   //{
   //   /// <summary>
   //   /// 设置界面控件
   //   /// </summary>
   //   Control DesignUI { get;}

   //   /// <summary>
   //   /// 加载设置集合
   //   /// </summary>
   //   /// <param name="account"></param>
   //   /// <param name="configs"></param>
   //   void Load(IAccount account, Dictionary<string, EmrAppConfig> configs);

   //   /// <summary>
   //   /// 接口内保存更改的设置到ChangedConfigs
   //   /// 如果接口内即时更新ChangedConfigs,此方法无需实现(不要抛出未实现异常)
   //   /// </summary>
   //   void Save();

   //   /// <summary>
   //   /// 更新设置集合
   //   /// </summary>
   //   Dictionary<string, EmrAppConfig> ChangedConfigs { get;}

   //   /// <summary>
   //   /// 设置对象(如果有返回,没有则null)
   //   /// </summary>
   //   object ConfigObj { get;}
   //}

   /// <summary>
   /// 系统配置
   /// </summary>
   public class EmrAppConfig
   {
      string _key;
      string _name;
      string _config;
      string _descript;
      ConfigParamType _ptype;
      IList<string> _keyset = new List<string>();
      ConfigChoiceType _ctype;
      string _showlistDict;
      string _designClass;
      ConfigParamLevel _plevel;
      bool _valid;
      bool _canSetUserLevel;
      string _roleId = string.Empty;
      string _userId = string.Empty;
      string ishide;
      string valid;//是否有效

      public string Valid1
      {
          get { return valid; }
          set { valid = value; }
      }
       /// <summary>
       /// 隐藏标志
       /// </summary>
      public string IsHide
      {
          get { return ishide; }
          set { ishide = value; }
      }
      Dictionary<string, EmrAppConfig> _subConfigs = new Dictionary<string, EmrAppConfig>();

      /// <summary>
      /// 组配置集合
      /// </summary>
      public Dictionary<string, EmrAppConfig> SubConfigs
      {
         get { return _subConfigs; }
         set { _subConfigs = value; }
      }

      /// <summary>
      /// 配置是否可以设置到用户级别
      /// </summary>
      public bool CanSetUserLevel
      {
         get { return _canSetUserLevel; }
         set { _canSetUserLevel = value; }
      }

      /// <summary>
      /// 配置是否有效
      /// </summary>
      public bool Valid
      {
         get { return _valid; }
         set { _valid = value; }
      }

      /// <summary>
      /// 配置级别
      /// </summary>
      public ConfigParamLevel Plevel
      {
         get { return _plevel; }
         set { _plevel = value; }
      }

      /// <summary>
      /// 配置设计类
      /// </summary>
      public string DesignClass
      {
         get { return _designClass; }
         set { _designClass = value; }
      }

      /// <summary>
      /// 选择值字典
      /// </summary>
      public string ShowlistDict
      {
         get { return _showlistDict; }
         set { _showlistDict = value; }
      }

      /// <summary>
      /// 配置值选择类型
      /// </summary>
      public ConfigChoiceType Ctype
      {
         get { return _ctype; }
         set { _ctype = value; }
      }

      /// <summary>
      /// 组配置包含的键值列表
      /// </summary>
      public IList<string> Keyset
      {
         get { return _keyset; }
         set { _keyset = value; }
      }

      /// <summary>
      /// 配置值类型
      /// </summary>
      public ConfigParamType Ptype
      {
         get { return _ptype; }
         set { _ptype = value; }
      }

      /// <summary>
      /// 配置描述
      /// </summary>
      public string Descript
      {
         get { return _descript; }
         set { _descript = value; }
      }

      /// <summary>
      /// 配置值
      /// </summary>
      public string Config
      {
         get { return _config; }
         set { _config = value; }
      }

      /// <summary>
      /// 配置键值
      /// </summary>
      public string Key
      {
         get { return _key; }
         set { _key = value; }
      }

      /// <summary>
      /// 配置名称
      /// </summary>
      public string Name
      {
         get { return _name; }
         set { _name = value; }
      }

      /// <summary>
      /// 构造
      /// </summary>
      public EmrAppConfig()
      {
      }

      /// <summary>
      /// 岗位代码
      /// </summary>
      public string RoleId
      {
         get { return _roleId; }
         set { _roleId = value; }
      }

      /// <summary>
      /// 职工代码
      /// </summary>
      public string UserId
      {
         get { return _userId; }
         set { _userId = value; }
      }
   }

   /// <summary>
   /// 用户配置(系统配置集合)
   /// </summary>
   public class EmrUserConfig
   {
      IList<EmrAppConfig> _configs = new List<EmrAppConfig>();
      string _userId;

      /// <summary>
      /// 用户代码
      /// </summary>
      public string UserId
      {
         get { return _userId; }
      }

      /// <summary>
      /// 配置集合
      /// </summary>
      public IList<EmrAppConfig> Configs
      {
         get { return _configs; }
      }

      /// <summary>
      /// 构造
      /// </summary>
      /// <param name="userId"></param>
      public EmrUserConfig(string userId)
      {
         _userId = userId;
      }

      /// <summary>
      /// 取得指定岗位代码的配置
      /// </summary>
      /// <param name="roleId"></param>
      /// <returns>存在返回配置,否则返回Null</returns>
      public EmrAppConfig GetRoleConfig(string roleId)
      {
         foreach (EmrAppConfig config in _configs)
         {
            if (config.Plevel == ConfigParamLevel.Role
                && config.RoleId == roleId)
               return config;
         }
         return null;
      }

      /// <summary>
      /// 取得用户配置
      /// </summary>
      /// <returns></returns>
      public EmrAppConfig GetUserConfig()
      {
         foreach (EmrAppConfig config in _configs)
         {
            if (config.Plevel == ConfigParamLevel.User
                && config.UserId == _userId)
               return config;
         }
         return null;
      }

      /// <summary>
      /// 取得系统默认配置
      /// </summary>
      /// <returns></returns>
      public EmrAppConfig GetAppConfig()
      {
         foreach (EmrAppConfig config in _configs)
            if (config.Plevel == ConfigParamLevel.System) return config;
         return null;
      }

      /// <summary>
      /// 取得用户的默认配置
      /// 优先级(高->低): 用户配置 -> 岗位配置(默认第一个) -> 系统配置 
      /// </summary>
      /// <returns></returns>
      public EmrAppConfig GetDefaultConfig()
      {
         EmrAppConfig defconfig = null;
         bool isFirstRole = true;
         foreach (EmrAppConfig config in _configs)
         {
            if (config.Plevel == ConfigParamLevel.User)
            {
               defconfig = config;
               break;
            }
            if (config.Plevel == ConfigParamLevel.System) defconfig = config;
            if (config.Plevel == ConfigParamLevel.Role && isFirstRole)
            {
               defconfig = config;
               isFirstRole = false;
            }
         }
         return defconfig;
      }
   }

   /// <summary>
   /// 设置参数类型
   /// </summary>
   public enum ConfigParamType
   {
      /// <summary>
      /// Variant(Object)
      /// </summary>
      Var = 0,

      /// <summary>
      /// String
      /// </summary>
      String = 1,

      /// <summary>
      /// Int
      /// </summary>
      Integer = 2,

      /// <summary>
      /// Double
      /// </summary>
      Double = 3,

      /// <summary>
      /// Bool
      /// </summary>
      Boolean = 4,

      /// <summary>
      /// Xml
      /// </summary>
      Xml = 5,

      /// <summary>
      /// Color
      /// </summary>
      Color = 6,

      /// <summary>
      /// Set
      /// </summary>
      Set = 7,
   }

   /// <summary>
   /// 配置参数选择型
   /// </summary>
   public enum ConfigChoiceType
   {
      /// <summary>
      /// 非选择型
      /// </summary>
      None = 0,

      /// <summary>
      /// 单选择
      /// </summary>
      Single = 1,

      /// <summary>
      /// 多选择
      /// </summary>
      Multi = 2,
   }

   /// <summary>
   /// 配置级别
   /// </summary>
   public enum ConfigParamLevel
   {
      /// <summary>
      /// 系统级
      /// </summary>
      System = 0,

      /// <summary>
      /// 岗位级
      /// </summary>
      Role = 1,

      /// <summary>
      /// 用户级
      /// </summary>
      User = 2,
   }

   /// <summary>
   /// 配置读取接口
   /// </summary>
   public interface IAppConfigReader
   {
      /// <summary>
      /// 取得配置参数字典
      /// </summary>
      /// <param name="keys"></param>
      /// <returns></returns>
      Dictionary<string, EmrAppConfig> GetConfigs(params string[] keys);

      /// <summary>
      /// 取得指定用户配置参数字典
      /// </summary>
      /// <param name="userId"></param>
      /// <param name="keys"></param>
      /// <returns></returns>
      Dictionary<string, EmrUserConfig> GetConfigs(string userId, params string[] keys);

      /// <summary>
      ///  取得指定的单一配置
      /// </summary>
      /// <param name="key"></param>
      /// <returns></returns>
      EmrAppConfig GetConfig(string key);

      /// <summary>
      /// 取得指定用户的单一配置(可能有多个,系统默认,岗位默认等)
      /// </summary>
      /// <param name="userId"></param>
      /// <param name="key"></param>
      /// <returns></returns>
      EmrUserConfig GetConfig(string userId, string key);

      /// <summary>
      /// 取得指定的配置对象
      /// </summary>
      /// <param name="key"></param>
      /// <returns></returns>
      object GetConfigObj(string key);

      /// <summary>
      /// 取得指定用户的配置对象
      /// </summary>
      /// <param name="userId"></param>
      /// <param name="key"></param>
      /// <returns></returns>
      object GetConfigObj(string userId, string key);
   }
}
