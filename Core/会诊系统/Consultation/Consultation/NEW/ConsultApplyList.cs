using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Common.Ctrs.FORM;
using System.Data.SqlClient;
using DrectSoft.DSSqlHelper;

namespace Consultation.NEW
{
    /// <summary>
    /// 会诊申请记录列表界面
    /// Add xlb 2013-03-06
    /// 暂时不使用
    /// </summary>
    public partial class ConsultApplyList :DevBaseForm
    {
        string noofinpat = string.Empty;//病人病案号

        #region  === 方法   Add by xlb 2013-03-06====

        /// <summary>
        /// 构造方法
        /// </summary>
        public ConsultApplyList()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ConsultApplyList(string nOofinpat):this()
        {
            try
            {
                noofinpat = nOofinpat==null?"":nOofinpat;
                InitDataList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化会诊申请列表
        /// Add xlb 2013-03-06
        /// </summary>
        private void InitDataList()
        {
            try
            {
                DS_SqlHelper.CreateSqlHelper();
                string sql = @"select consultapplysn,urgencytypeid,abstract,purpose,applyuser,
                applytime,director from consultapply c where c.noofinpat=@nOofinpat";
                SqlParameter[] sps = { new SqlParameter("@nOofinpat", noofinpat) };
                DataTable dt = DS_SqlHelper.ExecuteDataTable(sql, sps, CommandType.Text);
                if (dt == null || dt.Rows.Count <= 0)
                {
                    return;
                }
                gridControlApplyList.DataSource = dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}