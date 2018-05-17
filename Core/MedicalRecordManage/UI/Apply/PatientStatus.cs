using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.FORM;
using MedicalRecordManage.Object;

namespace MedicalRecordManage.UI
{
    /// <summary>
    /// 如果有的病人有小孩，此窗体弹出选择患者还是其孩子
    /// add by  ywk 2012年6月8日 11:46:33
    /// </summary>
    public partial class PatientStatus : DevBaseForm
    {
        IEmrHost m_app;
        public string NOOfINPAT { get; set; }
        public PatientStatus()
        {
            InitializeComponent();
        }
        public PatientStatus(IEmrHost myapp, string m_noofinpat)
        {
            m_app = myapp;
            NOOfINPAT = m_noofinpat;
            InitializeComponent();
        }
        #region 事件
        /// <summary>
        ///确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
  
        public bool GoToEmrInupt { get; set; }//声明变量，双击进入文书录入 add ywk 
         /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
        StringFormat sf = new StringFormat();
        /// <summary>
        /// 标注母亲和子女
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <summary>
        /// 界面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChoosePatOrBaby_Load(object sender, EventArgs e)
        {
            LoadPatAndBaby();
        }
        #endregion
        
        #region 方法
        /// <summary>
        /// 此列表应显示患者和她的孩子
        /// edit by Yanqiao.Cai 2012-11-15
        /// 1、add try ... catch
        /// 2、加载性别图片
        /// </summary>
        private void LoadPatAndBaby()
        {
            try
            {
                //加载性别图片
           
                DataTable dt = SqlUtil.GetPatAndBaby(NOOfINPAT);
                            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        /// <summary>
        /// 序号
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-12</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
      
        private void btn_search_Click(object sender, EventArgs e)
        {
            m_app = SqlUtil.App;
            string sql = "select * from patientRecordStatusQuery where 1=1";
                if(rb_ID.Checked)
                {
                    sql += "and patid='"+txt_search.Text.Trim()+"'";
                }
                else if (rb_name.Checked)
                {
                    sql += "and NAME='" + txt_search.Text.Trim() + "'";
                }
                else if (rb_recordID.Checked)
                {
                    sql += "and noofrecord='" + txt_search.Text.Trim() + "'";
                }
                DataTable dt = m_app.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
                if (dt.Rows.Count == 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("无对应病人信息");
                    return;
                }
                else if (dt.Rows.Count==1)
                {
                    txt_name.Text =dt.Rows[0]["name"].ToString();
                    txt_patientId.Text = dt.Rows[0]["patid"].ToString();
                    txt_RecordNo.Text = dt.Rows[0]["noofrecord"].ToString();
                    txt_recordStatus.Text = dt.Rows[0]["gdzt"].ToString();
                    txt_VisitId.Text = dt.Rows[0]["incount"].ToString();
                }
                else if (dt.Rows.Count > 1)
                {
                    QueryPatientStatus qps = new QueryPatientStatus( dt);
                    if (qps.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        DataRow[] dr = dt.Select("patid='"+qps.NOOfINPAT+"' and incount="+qps.Incount);
                        if (dr.Length > 0)
                        {
                            txt_name.Text = dr[0]["name"].ToString();
                            txt_patientId.Text = dr[0]["patid"].ToString();
                            txt_RecordNo.Text = dr[0]["noofrecord"].ToString();
                            txt_recordStatus.Text = dr[0]["gdzt"].ToString();
                            txt_VisitId.Text = dr[0]["incount"].ToString();
                        }
                    }
                }
        }

        void ClearText()
        {
            txt_name.Text = "";
            txt_patientId.Text = "";
            txt_RecordNo.Text = "";
            txt_recordStatus.Text = "";
            txt_VisitId.Text = "";
        }

       
    }

}