using System.Data;
using DrectSoft.Core;

namespace DrectSoft.Emr.Web.Business.Service
{

    public class Public
    {

        public static string UserId = string.Empty;
        public static string UserName = string.Empty;
        /// <summary>
        /// DB操作相关
        /// </summary>
        public static IDataAccess m_SqlHelper = DataAccessFactory.GetSqlDataAccess();

        public bool IsUserOrNot(string userId, string strPwd, out string strErrorMessage)
        {
            strErrorMessage = "";
            string sql = "SELECT ID,Name,Passwd,RegDate FROM Users WHERE ID='" + userId + "'  AND Valid = '1'";
            DataTable dataTableUser = m_SqlHelper.ExecuteDataTable(sql);
            if (dataTableUser.Rows.Count == 0)
            {
                strErrorMessage = "用户不存在！";
                return false;
            }
            else
            {
                string encryptPasswordBase64 = HisEncryption.EncodeString(dataTableUser.Rows[0]["RegDate"].ToString(), HisEncryption.PasswordLength, strPwd);
                if (encryptPasswordBase64 == dataTableUser.Rows[0]["Passwd"].ToString())
                {
                    UserName = dataTableUser.Rows[0]["Name"].ToString();
                    return true;
                }
                else
                {
                    strErrorMessage = "密码错误！";
                    return false;
                }
            }
        }

        #region navbar
        /// <summary>
        /// 初始化navbar group
        /// </summary>
        /// <returns></returns>
        public DataTable GetNavBarGroupData()
        {
            string sql = "select ID,Name,Url,ParentId from WebTree Where Valid = '1' and ParentId is null";
            DataTable dataTableGroup = m_SqlHelper.ExecuteDataTable(sql);
            return dataTableGroup;
        }

        /// <summary>
        /// 初始化navbar item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetNavBarItemData(int id)
        {
            string sql = "select ID,Name,Url,ParentId,OrderValue from WebTree Where Valid = '1' and ParentId=" + id + " order by OrderValue";
            DataTable dataTableItem = m_SqlHelper.ExecuteDataTable(sql);
            return dataTableItem;
        }
        public DataTable GetData(string sql)
        {
            DataTable dataTableItem = m_SqlHelper.ExecuteDataTable(sql);
            return dataTableItem;
        }
        #endregion

    }

    /// <summary>
    /// 统计资料类型
    /// </summary>
    public enum QCStatType : int
    {
        /// <summary>
        /// 在院患者
        /// </summary>
        QCInPatient = 1,
        /// <summary>
        /// 出院以提交
        /// </summary>
        QCOutPatientSub = 2,
        /// <summary>
        /// 当日违规
        /// </summary>
        QCBrokeRule = 3,
        /// <summary>
        /// 危重病人
        /// </summary>
        QCCritical = 4,
        /// <summary>
        /// 手术病人
        /// </summary>
        QCSurgery = 5,
        /// <summary>
        /// 死亡病人
        /// </summary>
        QCDeath = 6,
        /// <summary>
        /// 住院超过30天
        /// </summary>
        QCOver30 = 7,
        /// <summary>
        /// 出院未提交
        /// </summary>
        QCOutPatientUnSub = 8,
        /// <summary>
        /// 归档病历
        /// </summary>
        QCArchives = 9
    }

    /// <summary>
    /// 数据操作
    /// </summary>
    public enum QCDataOperator : int
    {
        /// <summary>
        /// 新增
        /// </summary>
        ADD = 0,
        /// <summary>
        /// 更新
        /// </summary>
        Updae = 1,
        /// <summary>
        /// 删除
        /// </summary>
        Del = 2
    }
}
