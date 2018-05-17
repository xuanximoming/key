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
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common;

namespace DrectSoft.EMR.ThreeRecordAll
{
    /// <summary>	
    /// <title>功能描述</title>
    /// <auth>xuliangliang</auth>
    /// <date></date>
    /// </summary>
    public partial class InPatListForm : DevBaseForm
    {
        IEmrHost m_app;
        public InPatListForm(IEmrHost app)
        {
            this.m_app = app;
            InitializeComponent();
            SearchInpatient();
        }

        /// <summary>
        /// dategrid中的病人
        /// </summary>
        public void SearchInpatient()
        {
            string departcode = m_app.User.CurrentDeptId;
            string wardcode = m_app.User.CurrentWardId;
            string patid = txtPatId.Text;
            //xll 2013-1-31调整
            string sql = @"select noofinpat,patid,name,outbed,isbaby,mother,py,wb  from inpatient i where i.outhosward=@wardcode and status in ('1501','1500') order by outbed;";
            SqlParameter[] sqlParams = new SqlParameter[]
           {
               // new SqlParameter("@departCode",SqlDbType.VarChar,50),
                new SqlParameter("@wardcode",SqlDbType.VarChar,50)
           };
            //sqlParams[0].Value = departcode;
            sqlParams[0].Value = wardcode;
            DataTable dtInpatient = m_app.SqlHelper.ExecuteDataTable(sql, sqlParams, CommandType.Text);
            SetChildPatId(dtInpatient);
            gcInpatient.DataSource = dtInpatient;
        }


        /// <summary>
        /// xll 2013-1-31 设置小孩的住院号为母亲的住院号
        /// </summary>
        /// <param name="dtInpatient"></param>
        private void SetChildPatId(DataTable dtInpatient)
        {
            if (dtInpatient == null || dtInpatient.Rows.Count <= 0) return;
            for (int i = 0; i < dtInpatient.Rows.Count; i++)
            {
                if (dtInpatient.Rows[i]["ISBABY"].ToString() == "1")
                {
                    string sql = @"select patid from inpatient i where i.noofinpat=@noofinpat";
                    SqlParameter[] sqlParams = new SqlParameter[]
                    {
                         new SqlParameter("@noofinpat",SqlDbType.VarChar,50)
                    };
                    sqlParams[0].Value = dtInpatient.Rows[i]["MOTHER"].ToString();
                    DataTable dtmat = m_app.SqlHelper.ExecuteDataTable(sql, sqlParams, CommandType.Text);
                    if (dtmat == null || dtmat.Rows.Count <= 0) continue;
                    dtInpatient.Rows[i]["PATID"] = dtmat.Rows[0]["PATID"];
                }
            }
        }

        private void txtPatId_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string searchKey = txtPatId.Text.Trim();
                searchKey = searchKey.Replace("[", "[[ ")
                                     .Replace("]", " ]]")
                                     .Replace("*", "[*]")
                                     .Replace("%", "[%]")
                                     .Replace("[[ ", "[[]")
                                     .Replace(" ]]", "[]]")
                                     .Replace("\'", "''");//zyx 2013-01-29
                string filter = string.Format("name like '%{0}%' or  py like '%{0}%' or  wb like '%{0}%' or  patid like '%{0}%' or  outbed like '%{0}%'", searchKey);
                DataTable dtInpatient = gcInpatient.DataSource as DataTable;
                if (dtInpatient != null && dtInpatient.Rows.Count > 0)
                {
                    dtInpatient.DefaultView.RowFilter = filter;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 序号列
        /// Edit by xlb 2013-03-25
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDataElement_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            //if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            //{
            //    e.Info.DisplayText = (e.RowHandle + 1).ToString();
            //}
            try
            {
                DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void txtPatId_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            gcInpatient.Focus();
            gvInpatient.MoveBy(0);

        }

        private void InPatListForm_Load(object sender, EventArgs e)
        {
            //txtPatId.Focus();
        }



    }
}
