using System.Data;


namespace DrectSoft.Core.IEMMainPageZY
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

        ///// <summary>
        ///// 得到整个界面的绑定的出诊的中西医库的值
        ///// </summary>
        ///// <returns></returns>
        //public DataTable GetFormLoadData(string MZDiagType)
        //{
        //    if(MZDiagType == "ZHONGYI")
        //    {
        //      string SqlAllDiagChinese = @"select id icd, name, py, wb from diagnosisofchinese where valid='1'";
        //        DataTable dt =m_SqlHelper.ExecuteDataTable(SqlAllDiagChinese,CommandType.Text);
        //        return dt;
        //    }
        //    if (MZDiagType == "XIYI")
        //    {
        //        string SqlAllDiag = @"select py, wb, name, icd from diagnosis where valid='1'";
        //        DataTable dt = m_SqlHelper.ExecuteDataTable(SqlAllDiag, CommandType.Text);
        //        return dt;
        //    }
        //}
    }
}
