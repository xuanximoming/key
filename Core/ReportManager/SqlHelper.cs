using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Data.Common;
using YiDanSqlHelper;

namespace YidanSoft.Core.YDReportManager
{

    /// <summary>
    /// 传染病上报卡状态
    /// </summary>
    public enum ReportState
    {
        /// <summary>
        /// 新增单据
        /// </summary>
        None = 0,

        /// <summary>
        /// 保存
        /// </summary>
        Save = 1,

        /// <summary>
        /// 提交
        /// </summary>
        Submit = 2,

        /// <summary>
        /// 填卡人撤回
        /// </summary>
        Withdraw = 3,

        /// <summary>
        /// 审核通过
        /// </summary>
        Approv = 4,

        /// <summary>
        /// 退回，审核未通过
        /// </summary>
        UnPassApprove = 5,

        /// <summary>
        /// 上报
        /// </summary>
        Report = 6,

        /// <summary>
        /// 作废
        /// </summary>
        Cancel = 7
    }

    public  class SqlHelper
    {
        private string m_SqlGetPatientListForNew = @"select i.name, i.sexid,
                    (select name from dictionary_detail where detailid = i.sexid and categoryid = '3') as gender,
                    i.agestr, i.patid, i.outbed as bedno, i.status,
                    (select d.name from diagnosis d where d.icd = i.admitdiagnosis and d.valid = '1') as diagnosis,
                    i.noofinpat
                    from inpatient i 
                    where (i.admitdept = '{0}' or i.outhosdept = '{0}') --and i.status in ('1501', '1502', '1504', '1505', '1506', '1507')
                    order by i.outbed";

        private string m_SqlGetAllPatientListForNew = @"select i.name, i.sexid,
                    (select name from dictionary_detail where detailid = i.sexid and categoryid = '3') as gender,
                    i.agestr, i.patid, i.outbed as bedno, i.status,
                    (select d.name from diagnosis d where d.icd = i.admitdiagnosis and d.valid = '1') as diagnosis,
                    i.noofinpat
                    from inpatient i 
                    order by i.outbed";

        private string m_SqlDeptList = @"select distinct ID, NAME from department a ,dept2ward b where a.id = b.deptid ";

        public  SqlHelper()
        {
            YD_SqlHelper.CreateSqlHelper();
        }
       
        /// <summary>
        /// 返回报告卡类别列表
        /// </summary>
        /// <returns></returns>
        public  DataTable GetReportCategoryList()
        {
            DataTable dt = YD_SqlHelper.ExecuteDataTable("SELECT ID,Name FROM reportcategory where VALID=1 ORDER BY ORDERID ASC");
            return dt;           
        }

        #region 肿瘤病例报告卡：表THERIOMAREPORTCARD

        public  void AddTherioma()
        { 
        
        }

        #endregion


        /// <summary>
        /// 获得新增病人时弹出的病人列表
        /// </summary>
        /// <param name="deptNO"></param>
        /// <returns></returns>
        public DataTable GetPatientListForNew(string deptNO)
        {
            string sql = string.Format(m_SqlGetPatientListForNew, deptNO);
            DataTable dt = YD_SqlHelper.ExecuteDataTable(sql);
            return dt;
        }

        /// <summary>
        /// 获得新增病人时弹出的所有病人的列表
        /// </summary>
        /// <param name="deptNO"></param>
        /// <returns></returns>
        public DataTable GetAllPatientListForNew()
        {
            DataTable dt = YD_SqlHelper.ExecuteDataTable(m_SqlGetAllPatientListForNew);
            return dt;
        }

    }
}
