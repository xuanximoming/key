#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace DrectSoft.Core
{
    /// <summary>
    /// 用户类
    /// </summary>
    [Serializable]
    public class Users : IUser
    {
        #region 私有变量

        private string m_UserId = "";
        private string m_UserName = "";
        private string m_Password = "";
        private string m_UserDoctorId = "";
        private string m_UserDoctorName = "";
        private string m_GWCodes = string.Empty;
        private bool m_Available = true;
        private List<DeptWardInfo> m_relateDepts = new List<DeptWardInfo>();
        private DeptWardInfo m_currentDeptWard;

        // 用户的密钥
        private string _passwordKey = "";

        #endregion

        #region 公共事件

        ///// <summary>
        ///// 科室改变事件
        ///// </summary>
        //public event EventHandler CurrentDeptChanged;

        ///// <summary>
        ///// 病区改变事件
        ///// </summary>
        //public event EventHandler CurrentWardChanged;

        /// <summary>
        /// 当前科室病区被改变事件
        /// </summary>
        public event EventHandler CurrentDeptWardChanged;

        /// <summary>
        /// 用户改变事件
        /// </summary>
        public event EventHandler UserChanged;

        #endregion 公共事件

        #region User的构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userID">用户代码</param>
        /// <param name="userName">用户名称</param>
        public Users(string userID, string userName)
        {
            SetUser(userID, userName);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Users()
            : this("", "")
        {
        }
        #endregion

        #region 公共函数

        /// <summary>
        /// 设置用户的代码和名称
        /// </summary>
        /// <param name="userId">用户代码</param>
        /// <param name="userName">用户名称</param>
        public void SetUser(string userId, string userName)
        {
            if (m_UserId == userId)
                return;
            m_UserId = userId;
            m_UserName = userName;
            if (this.UserChanged != null)
                UserChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// 设置用户代码名称,和职工代码名称
        /// </summary>
        /// <param name="userDoctorId"></param>
        /// <param name="userDoctorName"></param>
        public void SetUserDoctor(string userDoctorId, string userDoctorName)
        {
            if (m_UserDoctorId == userDoctorId) return;
            m_UserDoctorId = userDoctorId;
            m_UserDoctorName = userDoctorName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deptId"></param>
        /// <param name="deptName"></param>
        /// <param name="wardId"></param>
        /// <param name="wardName"></param>
        public void SetCurrentDeptWard(string deptId, string deptName, string wardId, string wardName)
        {
            SetCurrentDeptWard(new DeptWardInfo(deptId, deptName, wardId, wardName));
        }

        /// <summary>
        /// 设置当前科室病区的代码和名称
        /// </summary>
        /// <param name="newDwi"></param>
        public void SetCurrentDeptWard(DeptWardInfo newDwi)
        {
            if (m_currentDeptWard == newDwi) return;
            m_currentDeptWard = newDwi;
            if (CurrentDeptWardChanged != null)
                CurrentDeptWardChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// 设置当前操作员的角色字串
        /// </summary>
        /// <param name="roleIds"></param>
        public void SetRoleId(string roleIds)
        {
            m_GWCodes = roleIds;
        }

        /// <summary>
        /// 验证密码是否正确
        /// </summary>
        /// <param name="password">密码字符串</param>
        /// <returns>正确返回true,否则返回false</returns>
        public bool ComparePassword(string password)
        {
            try
            {
                string encryptPasswordBase64 = HisEncryption.EncodeString(_passwordKey, HisEncryption.PasswordLength, password);
                return (encryptPasswordBase64 == m_Password);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        ///// <summary>
        ///// 设置相关科室信息
        ///// </summary>
        ///// <param name="depts"></param>
        //public void SetRelateDepts(IList<DeptWardInfo> depts)
        //{
        //    RelateDepts.Clear();
        //    foreach (DeptWardInfo dept in depts)
        //        RelateDepts.Add(dept);
        //}

        #endregion 公共函数

        #region 公共属性

        /// <summary>
        /// 带教老师ID
        /// </summary>
        public string MasterID
        {
            get { return _masterID; }
            set { _masterID = value; }
        }
        private string _masterID;

        /// <summary>
        /// 登记时间
        /// </summary>
        public string PasswordKey
        {
            set { _passwordKey = value; }
        }

        /// <summary>
        /// 获得和设置用户密码
        /// </summary>
        /// <value></value>
        public string Password
        {
            get
            {
                return m_Password;
            }

            set
            {
                m_Password = value;
            }
        }

        #endregion 公共属性

        #region IUser的成员

        /// <summary>
        /// 用户代码
        /// </summary>
        /// <value></value>
        public string Id
        {
            get { return m_UserId; }
        }

        /// <summary>
        /// 用户名称
        /// </summary>
        /// <value></value>
        public string Name
        {
            get { return m_UserName; }
        }

        /// <summary>
        /// 用户医生代码
        /// </summary>
        /// <value></value>
        public string DoctorId
        {
            get { return m_UserDoctorId; }
        }

        /// <summary>
        /// 用户医生名称
        /// </summary>
        /// <value></value>
        public string DoctorName
        {
            get { return m_UserDoctorName; }
        }

        /// <summary>
        /// 当前科室代码
        /// </summary>
        /// <value></value>
        public string CurrentDeptId
        {
            get { return m_currentDeptWard.DeptId; }
        }

        /// <summary>
        /// 当前科室名称
        /// </summary>
        /// <value></value>
        public string CurrentDeptName
        {
            get { return m_currentDeptWard.DeptName; }
        }

        /// <summary>
        /// 当前病区代码
        /// </summary>
        /// <value></value>
        public string CurrentWardId
        {
            get { return m_currentDeptWard.WardId; }
        }

        /// <summary>
        /// 当前病区名称
        /// </summary>
        /// <value></value>
        public string CurrentWardName
        {
            get { return m_currentDeptWard.WardName; }
        }

        /// <summary>
        /// 岗位代码字串（形式：01,02,03,）
        /// </summary>
        public string GWCodes
        {
            get { return m_GWCodes; }
        }

        /// <summary>
        /// 获取该用户是否有效
        /// </summary>
        public bool Available
        {
            get { return m_Available; }
            set { m_Available = value; }
        }

        /// <summary>
        /// 获取用户相关科室病区信息(形式:deptid,deptname/wardid,wardname)
        /// </summary>
        public IList<DeptWardInfo> RelateDeptWards
        {
            get
            {
                if (m_relateDepts == null) m_relateDepts = new List<DeptWardInfo>();
                return m_relateDepts;
            }
        }

        /// <summary>
        /// 当前科室病区
        /// </summary>
        public DeptWardInfo CurrentDeptWard
        {
            get { return m_currentDeptWard; }
        }

        /// <summary>
        /// 用户状态
        /// </summary>
        public int Status { get; set; }
        #endregion
    }
}
