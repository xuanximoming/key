#region Using directives

using DrectSoft.Core.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
#endregion

namespace DrectSoft.Core
{
    /// <summary>
    /// 帐户信息有关的数据访问逻辑组件
    /// </summary>
    public class AccountDalc
    {
        #region 私有变量
        /*
        private const string UpdateCZRYK = "update Users set Passwd ='{0}' ,RegDate = '{1}' where ID ='{2}'";
        private const string SelectCZRYK = @" select d.ID userid,d.Name userName,d.Passwd,d.DeptId,d.WardID, b.Name DeptName, c.Name WardName, d.RegDate ,d.ID,d.JobID ,d.Valid 
                                                from Users d left join Department b on d.DeptId = b.ID left join Ward c on d.WardID = c.ID 
                                                where d.ID='{0}'";

        // 取在院病人的所有科室、病区对应关系
        private const string SelectAdminCanSelectedDeptWard = @"select distinct a.OutHosDept DeptId, b.Name DeptName, a.OutHosWard WardId, c.Name WardName
                                                                from InPatient a, Department b, Ward c
                                                                where a.Status not in (1500, 1503, 1508, 1509) and a.OutHosDept = b.ID and a.OutHosWard = c.ID
                                                                order by DeptId, WardId";

        // 取职工科室对应设置, 若未指定病区，则通过指定的科室关联出所有的病区
        private const string SelectRelateDepts = @"select a.DeptId, b.Name DeptName , a.WardId,c.Name WardName  from User2Dept a
                                                  left join Department b on a.DeptId=b.ID
                                                  left join Ward c on a.WardId=b.ID
                                                  where UserId = '{0}' and DeptId <> ''
                                                  union
                                                  select distinct a.DeptId, b.Name DeptName, c.OutHosWard WardId, d.Name WardName
                                                  from User2Dept a, Department b, InPatient c, Ward d
                                                  where a.UserId = '{1}' and a.DeptId = b.ID and (a.WardId is null or a.WardId = '')
                                                  and c.OutHosDept = a.DeptId and c.Status not in (1500, 1503, 1508, 1509) and c.OutHosWard = d.ID
                                                  order by DeptName, WardName";
        */

        private const string UpdateCzryInfo = "update YY_ZGDMK set  Name='{0}', DeptId='{1}', WardID='{2}', JobID='{3}' where ID='{4}'";

        private IDataAccess sql_helper;

        #endregion 私有变量

        #region 构造函数

        /// <summary>
        /// Ctor
        /// </summary>
        public AccountDalc()
        {
            sql_helper = DataAccessFactory.DefaultDataAccess;
        }


        #endregion 构造函数

        #region 公共函数

        /// <summary>
        /// 根据输入的员工号得到员工的帐户信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        private DataTable GetUserAccount(string userID)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@id", SqlDbType.VarChar)
            };
            sqlParam[0].Value = userID;

            #region "临时员工权限控制 - by cyq(2012-08-16 11:00)"
            if (userID.Contains("用户已过期"))
            {
                throw new InvalidUserIdException("用户已过期");
            }
            DataTable userTmp = sql_helper.ExecuteDataTable(string.Format(@" select * from tempusers where userid='{0}' ", userID));
            if (userTmp.Rows.Count > 0)
            {
                if (!(DateTime.Parse(userTmp.Rows[0]["startdate"].ToString() + " 00:00:00") <= DateTime.Now && DateTime.Parse(userTmp.Rows[0]["enddate"].ToString() + " 00:00:00") >= DateTime.Now))
                {
                    throw new InvalidUserIdException("用户已过期");
                }
            }
            #endregion

            return sql_helper.ExecuteDataTable("usp_GetUserAccount", sqlParam, CommandType.StoredProcedure);
        }

        /// <summary>
        /// 获得一个DrectSoft.Security.User对象
        /// </summary>
        /// <param name="userId">用户代码</param>
        /// <returns>DrectSoft.Security.User对象</returns>
        public Users GetUser(string userId)
        {

            try
            {
                //****************************Modified By wwj 2011-06-07********************************
                //DataTable dsUser = sql_helper.ExecuteDataTable(string.Format(SelectCZRYK, userId));
                DataTable dsUser = GetUserAccount(userId);
                //**************************************************************************************

                if (dsUser == null || dsUser.Rows.Count < 1)
                {
                    throw new InvalidUserIdException();
                }

                DataRow dataRow = dsUser.Rows[0];
                Users user = new Users(dataRow["userid"].ToString().Trim()
                            , dataRow["username"].ToString().Trim());


                string deptid = dataRow.IsNull("DeptId") ? string.Empty : dataRow["DeptId"].ToString();
                string warid = dataRow.IsNull("WardID") ? string.Empty : dataRow["WardID"].ToString();
                string deptname = dataRow.IsNull("DeptName") ? string.Empty : dataRow["DeptName"].ToString();
                string wardname = dataRow.IsNull("WardName") ? string.Empty : dataRow["WardName"].ToString();

                user.SetUserDoctor(dataRow["userid"].ToString().Trim()
                            , dataRow["username"].ToString().Trim());

                user.SetCurrentDeptWard(deptid, deptname, warid, wardname);
                user.PasswordKey = dataRow["RegDate"].ToString().Trim();
                user.Password = dataRow["Passwd"].ToString();
                user.SetRoleId(dataRow["JobID"].ToString().Trim());
                user.Status = dataRow.IsNull("Status") ? 0 : Convert.ToInt32(dataRow["Status"]);
                user.Available = (dataRow["Valid"].ToString().Trim() != "0");
                user.MasterID = dataRow["MasterID"].ToString().Trim(); //Add By wwj 2011-06-07
                user.RelateDeptWards.Clear();
                // 如果是超级用户，则取所有科室、病区对应关系
                DataTable dtDepts;
                if (Account.IsAdministrator(user.GWCodes))
                    dtDepts = GetAdminCanSelectDepts();
                else
                    dtDepts = GetUserCanSelectDepts(userId);

                if (dtDepts != null)
                {
                    Dictionary<string, string> wardDepts = new Dictionary<string, string>();

                    for (int i = 0; i < dtDepts.Rows.Count; i++)
                    {
                        DataRow drDepts = dtDepts.Rows[i];
                        DeptWardInfo dwi = new DeptWardInfo(
                            drDepts["DeptId"].ToString(), drDepts["DeptName"].ToString(),
                            drDepts["WardId"].ToString(), drDepts["WardName"].ToString());
                        user.RelateDeptWards.Add(dwi);
                    }
                }
                return user;

            }
            catch (SqlException ex)
            {
                throw new InvalidUserIdException(ex.Message);

            }

        }

        /// <summary>
        /// 改变用户密码
        /// </summary>
        /// <param name="user">User对象</param>
        /// <param name="oldPassword">原密码</param>
        /// <param name="newPassword">新密码</param>
        public void ChangeUserPassword(Users user, string oldPassword, string newPassword)
        {
            try
            {
                if (user == null)
                {
                    throw new ArgumentNullException("user", Resources.UserInfoIsNull);
                }
                if (user.ComparePassword(oldPassword))
                {
                    DateTime now = DateTime.Now;
                    string encryptDateTime = now.ToString("yyyyMMdd") + now.ToString("T");
                    string encryptNewPassword = HisEncryption.EncodeString(
                        encryptDateTime, HisEncryption.PasswordLength, newPassword);
                    //***********************************Modified By wwj 2011-06-07*************************************
                    //DataAccessFactory.DefaultDataAccess.ExecuteNoneQuery(string.Format(UpdateCZRYK, encryptNewPassword, encryptDateTime, user.ID));

                    SqlParameter[] sqlParam = new SqlParameter[] 
                    { 
                        new SqlParameter("@ID", SqlDbType.VarChar),
                        new SqlParameter("@Passwd", SqlDbType.VarChar),
                        new SqlParameter("@RegDate", SqlDbType.VarChar)
                    };
                    sqlParam[0].Value = user.Id;
                    sqlParam[1].Value = encryptNewPassword;
                    sqlParam[2].Value = encryptDateTime;
                    sql_helper.ExecuteNoneQuery("usp_UpdateUserPassword", sqlParam, CommandType.StoredProcedure);
                    //***************************************************************************************************

                    user.Password = newPassword;
                    user.PasswordKey = encryptNewPassword;
                }
                else
                {
                    throw new InvalidUserPasswordException("原密码不正确");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 创建用户, not Implemented
        /// </summary>
        /// <param name="user"></param>
        public static void CreateUser(Users user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 更新用户信息, not Implemented
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUserInfo(Users user)
        {
            if (user == null) throw new ArgumentNullException("user", Resources.UserInfoIsNull);

            sql_helper.ExecuteNoneQuery(string.Format(UpdateCzryInfo, user.Name, user.CurrentDeptId, user.CurrentWardId, user.GWCodes, user.Id));

        }

        internal void UpdateLoginInfo(Users user)
        {
            //SqlParameter paramZgdm = new SqlParameter("@userId", SqlDbType.VarChar, 6);
            //paramZgdm.Value = user.Id;
            //SqlParameter paramLogtime = new SqlParameter("@login_time", SqlDbType.DateTime);
            //paramLogtime.Value = DateTime.Now;
            //SqlParameter paramIpAddr = new SqlParameter("@ip_addr", SqlDbType.VarChar, 32);
            //PhysicalAddress[] pas = GetPhysicalAddresses();
            //if (pas != null) paramIpAddr.Value = Environment.MachineName + ":"
            //    + pas[0].ToString();

            //m_AdoHelper.ExecuteNoneQuery(UpdateCZRYKLogTime, new SqlParameter[]{
            //        paramZgdm, paramLogtime, paramIpAddr
            //    });
            //TODO
        }


        /// <summary>
        /// 登录/登出 信息跟踪
        /// add by xjt,for acount module transfer
        /// </summary>
        /// <param name="id">登录者ID</param>
        /// <param name="moduleId">模块ID，可传空</param>
        /// <param name="macAddr">mac地址</param>
        /// <param name="clientIp">机器IP</param>
        /// <param name="reasonId">登录/登出 reason 0/null 正常，1强制</param>
        /// <param name="createUser">创建者</param>
        public void InsertUserLogIn(string id, string moduleId, string macAddr, string clientIp, string reasonId, string createUser)
        {
            IDataAccess sqlHelper = DataAccessFactory.GetSqlDataAccess();
            SqlParameter[] sqlParams = new SqlParameter[] { 
                  new SqlParameter("ID", SqlDbType.VarChar, 4),
                  new SqlParameter("ModuleId", SqlDbType.VarChar, 255),
                  new SqlParameter("HostName", SqlDbType.VarChar, 255),
                  new SqlParameter("MACADDR", SqlDbType.VarChar, 255),
                  new SqlParameter("Client_ip", SqlDbType.VarChar, 255),
                  new SqlParameter("Reason_id", SqlDbType.VarChar, 1),
                  new SqlParameter("Create_user", SqlDbType.VarChar,4)};
            sqlParams[0].Value = id;
            sqlParams[1].Value = moduleId;
            sqlParams[2].Value = System.Net.Dns.GetHostName();
            sqlParams[3].Value = macAddr;
            sqlParams[4].Value = clientIp;
            sqlParams[5].Value = reasonId;
            sqlParams[6].Value = createUser;
            sqlHelper.ExecuteNoneQuery("usp_InsertUserLogIn", sqlParams, CommandType.StoredProcedure);
        }



        #endregion 公共函数

        #region 内部函数

        private DataTable GetAdminCanSelectDepts()
        {
            //******************************Modified By wwj 2011-06-07********************************
            //return sql_helper.ExecuteDataTable(SelectAdminCanSelectedDeptWard, CommandType.Text);
            return sql_helper.ExecuteDataTable("usp_GetUserOutDeptAndWard", CommandType.StoredProcedure);
            //****************************************************************************************
        }

        private DataTable GetUserCanSelectDepts(string userId)
        {
            //*****************************Modified By wwj 2011-06-07************************************
            //return sql_helper.ExecuteDataTable(string.Format(SelectRelateDepts, userId, userId));
            SqlParameter[] sqlParam = new SqlParameter[] { 
                  new SqlParameter("@UserID", SqlDbType.VarChar, 4)
            };
            sqlParam[0].Value = userId;
            return sql_helper.ExecuteDataTable("usp_GetUserDeptAndWard", sqlParam, CommandType.StoredProcedure);
            //********************************************************************************************


            //System.Data.SqlClient.SqlParameter param = new System.Data.SqlClient.SqlParameter("userID", SqlDbType.VarChar, 12);
            //param.Value = userId;

            //switch (AccountSetting.PermissionLevel)
            //{
            //    case AccountPermissionLevel.UserDeptMapping:
            //        return m_AdoHelper.ExecuteDataTable(SelectRelateDepts, new SqlParameter[] { param });
            //    case AccountPermissionLevel.UserDept:
            //        return m_AdoHelper.ExecuteDataTable(SelectUserDeptMappings, new SqlParameter[] { param });
            //    case AccountPermissionLevel.DeptClassTwo:
            //        return m_AdoHelper.ExecuteDataTable(SelectDeptClassTwoMappings, new SqlParameter[] { param });
            //    case AccountPermissionLevel.DeptClassOne:
            //        return m_AdoHelper.ExecuteDataTable(SelectDeptClassOneMappings, new SqlParameter[] { param });
            //    default:
            //        throw new IndexOutOfRangeException("不支持的管理级别");
            //}
        }


        PhysicalAddress[] GetPhysicalAddresses()
        {
            IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            if (nics == null || nics.Length < 1)
            {
                Console.WriteLine("  No network interfaces found.");
                return null;
            }

            PhysicalAddress[] addresses = new PhysicalAddress[nics.Length];
            int i = 0;
            foreach (NetworkInterface adapter in nics)
            {
                IPInterfaceProperties properties = adapter.GetIPProperties();
                PhysicalAddress address = adapter.GetPhysicalAddress();
                byte[] bytes = address.GetAddressBytes();
                PhysicalAddress newAddress = new PhysicalAddress(bytes);
                addresses[i++] = newAddress;
            }
            return addresses;
        }
        #endregion
    }
}
