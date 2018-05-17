using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Core.MainEmrPad.HelpForm
{
    public partial class DoctorCustomForm : DevBaseForm
    {
        #region SQL
        private const string SqlAllDiag = " SELECT diagnosis.icd icd,diagnosis.py py,diagnosis.wb wb, doctorcustom.icdname name " + 
            " FROM doctorcustom, diagnosis " + 
            " WHERE doctorcustom.typename = '{0}' AND (userid = '{1}' or deptid = '{2}') AND doctorcustom.icd = diagnosis.icd and diagnosis.valid = '1' ";

        private const string SqlDoctorCustomType = " select distinct typename from doctorcustom ";
        #endregion

        #region FIELD && PROPERTY
        private const string PYFilter = " py like '%{0}%' ";
        private const string WBFilter = " wb like '%{0}%' ";
        private const string NameFilter = " name like '%{0}%' ";
        private const string ICDFilter = " icd like '%{0}%' ";

        private IEmrHost m_App;
        private DataTable m_DiagDataSource;

        private string m_Type = string.Empty;
        private string m_DiagName = string.Empty;
        private string m_DiagICD = string.Empty;


        /// <summary>
        /// 选中的诊断名
        /// </summary>
        public string DiagName
        {
            get
            {
                return m_DiagName;
            }
        }

        /// <summary>
        /// 选中的诊断的ICD编码
        /// </summary>
        public string DiagICD
        {
            get
            {
                return m_DiagICD;
            }
        }
        #endregion

        #region .ctor
        public DoctorCustomForm()
        {
            InitializeComponent();
        }

        public DoctorCustomForm(IEmrHost app, string type)
            : this()
        {
            m_App = app;
            m_Type = type;
        }
        #endregion

        #region 初始化界面并绑定数据源
        private void DoctorCustomForm_Load(object sender, EventArgs e)
        {
            InitData();
            ProcLoadData();
        }

        private void InitData()
        {
            m_DiagName = "";
            m_DiagICD = "";

            if (lookUpEditType.Properties.DataSource == null)
            {
                DataTable dt = m_App.SqlHelper.ExecuteDataTable(SqlDoctorCustomType, CommandType.Text);
                lookUpEditType.Properties.ValueMember = "TYPENAME";
                lookUpEditType.Properties.DisplayMember = "TYPENAME";
                lookUpEditType.Properties.DataSource = dt;
                if (m_Type != "")
                {
                    lookUpEditType.EditValue = m_Type;
                    lookUpEditType.Enabled = false;
                }
            }
        }

        private void ProcLoadData()
        {
            m_DiagName = "";
            m_DiagICD = "";

            if (m_DiagDataSource == null)
            {
                m_DiagDataSource = m_App.SqlHelper.ExecuteDataTable(string.Format(SqlAllDiag, m_Type, m_App.User.Id, m_App.User.CurrentDeptId), CommandType.Text);
            }
            else
            {
                m_DiagDataSource.DefaultView.RowFilter = "";
            }
            gridControlDiag.BeginUpdate();
            gridControlDiag.DataSource = m_DiagDataSource;
            gridControlDiag.EndUpdate();
            textEditInput.Text = "";
            m_DiagDataSource.DefaultView.RowFilter = "";
        }
        #endregion

        #region gridControlDiag的keyDown和MouseDoubleClick事件
        private void gridControlDiag_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                gridControlDiag.Focus();
                gridViewDiag.FocusedRowHandle = 0;
            }
        }

        private void gridControlDiag_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DataRowView dr = gridViewDiag.GetRow(gridViewDiag.FocusedRowHandle) as DataRowView;
            if (dr != null)
            {
                m_DiagICD = dr["icd"].ToString();
                m_DiagName = dr["name"].ToString();
                this.Close();
            }
        }
        #endregion

        #region CheckBoxChange事件
        private void checkEditPY_CheckedChanged(object sender, EventArgs e)
        {
            textEditInput.Focus();
        }

        private void checkEditWB_CheckedChanged(object sender, EventArgs e)
        {
            textEditInput.Focus();
        }

        private void checkEditName_CheckedChanged(object sender, EventArgs e)
        {
            textEditInput.Focus();
        }

        private void checkEditICD_CheckedChanged(object sender, EventArgs e)
        {
            textEditInput.Focus();
        }
        #endregion

        #region TextChanged事件
        private void textEditInput_TextChanged(object sender, EventArgs e)
        {
            string filter = string.Empty;
            if (checkEditPY.Checked)
            {
                filter += "OR " + string.Format(PYFilter, textEditInput.Text.Trim());
            }
            if (checkEditWB.Checked)
            {
                filter += "OR " + string.Format(WBFilter, textEditInput.Text.Trim());
            }
            if (checkEditName.Checked)
            {
                filter += "OR " + string.Format(NameFilter, textEditInput.Text.Trim());
            }
            if (checkEditICD.Checked)
            {
                filter += "OR " + string.Format(ICDFilter, textEditInput.Text.Trim());
            }
            if (m_DiagDataSource != null)
            {
                m_DiagDataSource.DefaultView.RowFilter = filter.Substring(2);
            }
        }
        #endregion
    }
}