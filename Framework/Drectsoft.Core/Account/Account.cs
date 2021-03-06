using System;
using System.Collections.ObjectModel;
using System.Security.Permissions;

namespace DrectSoft.Core
{
    /// <summary>
    /// 操作员账户
    /// </summary>
    public class Account : MarshalByRefObject, IAccount
    {
        #region 私有变量

        private Users _user;

        private Collection<IPermission> _permission;

        #endregion 私有变量

        #region 构造函数

        /// <summary>
        /// Ctor
        /// </summary>
        public Account()
        {
            _user = new Users();
        }

        #endregion 构造函数

        #region 私有函数

        /// <summary>
        /// 通过用户代码获得User对象
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        private static Users GetUser(string userID)
        {
            AccountDalc dalcAccount = CreateAccountDALC();
            Users user = dalcAccount.GetUser(userID);

            //if ((_user == null) || (!_user.Id.Equals(userID)))
            //    _user = GetUser(userID);
            //_permission = AccountPermission.GetUserPermission(_user);


            return user;
        }

        /// <summary>
        /// 创建一个新的AccountDALC对象
        /// </summary>
        /// <returns>AccountDALC对象</returns>
        private static AccountDalc CreateAccountDALC()
        {
            return new AccountDalc();
        }

        #endregion 私有函数

        #region 重载函数
        /// <summary>
        /// 重载InitializeLifetimeService
        /// </summary>
        /// <returns></returns>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
        public override Object InitializeLifetimeService()
        {
            return null;
        }
        #endregion 重载函数

        #region IAccount Members
        /// <summary>
        /// 用户登录函数
        /// </summary>
        /// <param name="userId">用户代码</param>
        /// <param name="password">密码</param>
        ///  /// <param name="type">登录方式0：通过登录界面登录，1：医生工作站跳转</param>
        /// <returns>一个包含了用户信息的xml字符串</returns>
        public IUser Login(string userId, string password, int type)
        {
            try
            {
                Users user = new AccountDalc().GetUser(userId);
                if (null == user)
                {
                    throw new InvalidUserIdException("用户名不正确");
                }
                if ((_user == null) || (!_user.Id.Equals(userId)))
                    _user = GetUser(userId);
                if (type == 0)
                {
                    if ((_user as Users).ComparePassword(password))
                    {
                        _permission = AccountPermission.GetUserPermission(_user);
                        return _user;
                    }
                    else
                    {
                        throw new InvalidUserPasswordException("密码不正确");
                    }
                }
                else
                {
                    _permission = AccountPermission.GetUserPermission(_user);
                    return _user;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 更改密码
        /// </summary>
        /// <param name="userId">用户代码</param>
        /// <param name="oldPassword">老密码</param>
        /// <param name="newPassword">新密码</param>
        public void ChangePassword(string userId, string oldPassword, string newPassword)
        {
            try
            {
                Users user = GetUser(userId);
                AccountDalc dalcAccount = CreateAccountDALC();
                dalcAccount.ChangeUserPassword(user, oldPassword, newPassword);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 创建一个新用户
        /// </summary>
        /// <param name="user">IUser接口对象</param>
        /// <param name="initPassword">初始密码</param>
        public void CreateNewUser(IUser user, string initPassword)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 更新用户的信息
        /// </summary>
        /// <param name="user">IUser接口对象</param>
        public void UpdateUserInfo(IUser user)
        {
            AccountDalc dalcAccount = CreateAccountDALC();
            dalcAccount.UpdateUserInfo((Users)user);
        }

        /// <summary>
        /// InfoAddedDelegate
        /// </summary>
        public event EventHandler<TraceEventArgs> InfoAddedDelegate
        {
            add { _infoAddedDelegate += value; }
            remove { _infoAddedDelegate -= value; }
        }
        private event EventHandler<TraceEventArgs> _infoAddedDelegate;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public void BroadcastMessage(string msg)
        {
            SafeInvokeEvent(msg);
        }

        private void SafeInvokeEvent(string msg)
        {
        }

        /// <summary>
        /// 判断帐户是否具有模块权限
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public bool HasPermission(string assemblyName, string className)
        {
            return HasPermission(assemblyName, className, string.Empty);
        }

        /// <summary>
        /// 判断帐户是否具有方法权限
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="className"></param>
        /// <param name="functionName"></param>
        /// <returns></returns>
        public bool HasPermission(string assemblyName, string className, string functionName)
        {
            if (IsAdministrator())
            {
                return true;
            }
            if (_permission == null) return false;
            for (int i = 0; i < _permission.Count; i++)
            {
                IPermission role = _permission[i];
                if (role.IsAdministrators) return true;
                for (int j = 0; j < role.ModuleList.Count; j++)
                {
                    IRBSModule module = role.ModuleList[j];


                    if (string.IsNullOrEmpty(functionName))
                    {
                        if (module.ModuleId.Trim() == className.Trim() + "_" + assemblyName.Trim())
                            return true;
                    }
                    else
                        for (int k = 0; k < module.FunctionList.Count; k++)
                        {
                            IRBSFunction function = module.FunctionList[k];
                            if (function.FunctionId == functionName)
                                return true;
                        }
                }
            }
            return false;
        }


        /// <summary>
        /// 判断是否为系统管理员
        /// </summary>
        /// <returns></returns>
        public bool IsAdministrator()
        {
            if (_user == null || string.IsNullOrEmpty(_user.GWCodes)) return false;
            //if (_user.GWCodes.IndexOf("00,") != -1)
            //    return true;
            //else
            //    return false;
            return IsAdministrator(_user.GWCodes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobCodes"></param>
        /// <returns></returns>
        public static bool IsAdministrator(string jobCodes)
        {
            if (String.IsNullOrEmpty(jobCodes))
                return false;
            string[] codes = jobCodes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string c in codes)
            {
                if (c.Equals("00"))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 帐户登录的用户
        /// </summary>
        public IUser User
        {
            get { return _user; }
        }


        /// <summary>
        /// 通过用户Id取得用户姓名
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="available"></param>
        /// <returns></returns>
        public string GetAUserName(string userId, out bool available)
        {
            _user = GetUser(userId);

            //在判断是否登录系统的权限
            available = _user.Available;
            return _user.Name;
        }

        /// <summary>
        /// 初始化带教老师用户信息 Add By wwj 2011-06-07
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public Users InitMaster(string userID)
        {
            //在临时账户登录时由于进此方法使得_permission变为null所以无菜单
            //重新判断获得edit by ywk
            if ((_user == null) || (!_user.Id.Equals(userID)))
            {
                _user = GetUser(userID);

                _permission = AccountPermission.GetUserPermission(_user);
            }

            if (!string.IsNullOrEmpty(userID))
            {
                return GetUser(userID);
            }
            return null;
        }
        #endregion
    }
}
