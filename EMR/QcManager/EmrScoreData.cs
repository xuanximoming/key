using DrectSoft.Common.Eop;
using DrectSoft.Core;
using DrectSoft.FrameWork.WinForm.Plugin;
using System.Data;
using System.Data.SqlClient;

namespace DrectSoft.Emr.QcManager
{
    /// <summary>
    /// 与数据相关操作 
    /// </summary>
    abstract class EmrScoreData
    {
        static IDataAccess sql_Helper;
        static IEmrHost m_app;
        static Inpatient m_CurrentInPatient;
        public static IEmrHost App
        {
            get { return m_app; }
            set
            {
                m_app = value;
                sql_Helper = m_app.SqlHelper;
            }
        }

        /// <summary>
        /// 当前患者
        /// </summary>
        public static Inpatient CurrentInPatient
        {
            get
            {
                return m_CurrentInPatient;
            }
            set
            {
                m_CurrentInPatient = value;
            }
        }
        /// <summary>
        /// 得到医院信息
        /// </summary>
        /// <returns></returns>
        private static DataTable GetHospitalInfo()
        {
            return sql_Helper.ExecuteDataTable("usp_GetHospitalInfo", new SqlParameter[] { }, CommandType.StoredProcedure);
        }

        public static string GetHospitalName()
        {
            DataTable dt = GetHospitalInfo();
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["name"].ToString();
                //return DrectSoft.MainFrame.PublicClass.getHosName();
            }
            return "";
        }
        /// <summary>
        /// 获得病人相关信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static DataTable GetPatientInfoForPoint(string noOfInpat)
        {
            string serchsql = string.Format(@"  
                select b.name as deptname,a.name as patname,a.patid
                ,c.name as indocname,d.name as updocname from inpatient a
               left join department b  on a.outhosdept=b.id 
               left join users c on a.resident=c.id  
               left join users d on  d.id=a.chief
                where a.noofinpat='{0}' ", noOfInpat);
            return m_app.SqlHelper.ExecuteDataTable(serchsql);
        }
        /// <summary>
        /// 计算用户配置的评分大项共有几个
        /// </summary>
        /// <returns></returns>
        public static DataTable GetSumRowCount()
        {
            string sql = string.Format(@" 
                select h.ccode,a.cname from emr_configpoint h join dict_catalog a
                on h.ccode=a.ccode where h.valid=1 
                group by h.ccode,a.cname ");
            return m_app.SqlHelper.ExecuteDataTable(sql);
        }
        /// <summary>
        /// 根据大项的名称得到评分小项的相关数据信息
        /// </summary>
        /// <param name="ccode"></param>
        /// <returns></returns>
        public static DataTable GetChildPointByCode(string ccode)
        {
            string sql = string.Format(@" select b.id,b.childname,a.cname from emr_configpoint b 
                join dict_catalog a on a.ccode=b.ccode
                where b.ccode='{0}' and b.valid='1' ", ccode);
            return m_app.SqlHelper.ExecuteDataTable(sql);
        }
    }
}
