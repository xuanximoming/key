using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DrectSoft.Wordbook;
using DrectSoft.Common.Library;

using DrectSoft.FrameWork.WinForm.Plugin;

namespace DrectSoft.Core.IEMMainPage
{
    class DataHelper
    {
        private IDataAccess m_SqlHelper = DataAccessFactory.DefaultDataAccess;

        private string m_GetHospitalInfo = "SELECT h.ID, h.Name FROM HospitalInfo h";

        public string GetHospitalName()
        {
            DataTable dt = m_SqlHelper.ExecuteDataTable(m_GetHospitalInfo, CommandType.Text);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["name"].ToString();
            }
            return "";
        }

        /// <summary>
        /// 根据病人首页好获取病人所在科室
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        public string GetInpatientDept(string noofinpat)
        {
            string sql = string.Format(@"select inp.outhosdept from inpatient inp where inp.noofinpat = '{0}'", noofinpat);
            DataTable dt = m_SqlHelper.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
                return dt.Rows[0][0].ToString();
            else
                return "";
        }

        /// <summary>
        /// 得到配置信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetConfigValueByKey(string key)
        {
            string sql1 = " select * from appcfg where configkey = '" + key + "'; ";
            DataTable dt = m_SqlHelper.ExecuteDataTable(sql1, CommandType.Text);
            string config = string.Empty;
            if (dt.Rows.Count > 0)
            {
                config = dt.Rows[0]["value"].ToString();
            }
            return config;
        }

        /// <summary>
        /// 根据员工工号获取员工姓名
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-23</date>
        /// </summary>
        public string GetUserNameByID(string userID)
        {
            try
            {
                string userName = string.Empty;
                if (!string.IsNullOrEmpty(userID))
                {
                    string sql = " select * from users where id = '" + userID + "'";
                    DataTable dt = m_SqlHelper.ExecuteDataTable(sql, CommandType.Text);
                    if (null != dt && dt.Rows.Count > 0)
                    {
                        userName = dt.Rows[0]["NAME"].ToString();
                    }
                }
                return userName;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable GetDataTableBySql(string sqlStr)
        {
            try
            {
                if (!string.IsNullOrEmpty(sqlStr))
                {
                    return m_SqlHelper.ExecuteDataTable(sqlStr, CommandType.Text);
                }
                return new DataTable();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据住院流水号得到视图MRMSBASE_CHECK_FLAG中的static_save信息
        /// </summary>
        /// <param name="inpatient_no"></param>
        /// <returns></returns>
        public string GetStatic_SaveValue(string inpatient_no)
        {
            string sql1 = "select static_save from MRMSBASE_CHECK_FLAG where  INPATIENT_NO = '" + inpatient_no + "'; ";
            DataTable dt = m_SqlHelper.ExecuteDataTable(sql1, CommandType.Text);
            string static_save = "";
            if (dt.Rows.Count > 0)
            {
                static_save = dt.Rows[0][0].ToString();
            }
            return static_save;
        }
    
    }
}
