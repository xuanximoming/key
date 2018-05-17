using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using DrectSoft.Core;

namespace DrectSoft.Core
{
    /// <summary>
    /// 帐户权限
    /// </summary>
    public class AccountPermission
    {
        private string _userId = string.Empty;
        private string _roleIds = string.Empty;
        private Collection<IPermission> _permission = new Collection<IPermission>();
        private PermissionDal _permissionDal;

        /// <summary>
        /// 用户代码
        /// </summary>
        public string UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        /// <summary>
        /// 角色代码列表
        /// </summary>
        public string RoleIds
        {
            get { return _roleIds; }
            set { _roleIds = value; }
        }

        /// <summary>
        /// Ctor
        /// </summary>
        private AccountPermission(Users user)
        {
            _userId = user.Id;
            _roleIds = user.GWCodes;
            _permissionDal = new PermissionDal();
        }

        private void GetPermission()
        {
            _permission = _permissionDal.GetAllRolePermission(_roleIds);
        }

        /// <summary>
        /// 返回一个用户的所有权限集合
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static Collection<IPermission> GetUserPermission(Users user)
        {
            AccountPermission instance = new AccountPermission(user);
            instance.GetPermission();
            return instance._permission;
        }

    }
}
