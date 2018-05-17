using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace DrectSoft.Core.EMR_NursingDocument.EMRInput.HelpForm
{
    public partial class DictionaryDataSerach : DevExpress.XtraEditors.XtraForm
    {
        #region field && Property

        /// <summary>
        /// 西医所有诊断
        /// </summary>
        private const string SqlAllDiag = " SELECT markid, icd, mapid, standardcode, name, py, wb, " + 
            " tumorid, statist, innercategory, category, othercategroy, valid, memo " +
            " FROM diagnosis where valid = '1' ";

        /// <summary>
        /// 中医所有诊断
        /// </summary>
        private const string SqlAllDiagChinese = " select  py, wb, name, id icd  from diagnosisofchinese where valid='1' union select py, wb, name, icdid from diagnosischiothername where valid='1'; ";

        private const string PYFilter = " py like '%{0}%' ";
        private const string WBFilter = " wb like '%{0}%' ";
        private const string NameFilter = " name like '%{0}%' ";
        private const string ICDFilter = " icd like '%{0}%' ";

        private IEmrHost m_App;
        private DataTable m_DiagDataSource;

        private string m_DiagName = string.Empty;
        private string m_DiagICD = string.Empty;

        delegate void DeleteLoadData();

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

        /// <summary>
        /// 诊断名称
        /// </summary>
        private string m_DiagTypeName = string.Empty;

        #endregion

        #region .ctor
        public DictionaryDataSerach()
        {
            InitializeComponent();
        }
        public DictionaryDataSerach(IEmrHost app, string diagTypeName)
            : this()
        {
            m_App = app;
            m_DiagTypeName = diagTypeName;
        }
        #endregion

        #region Method
        private void LoadData()
        {
            DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;//解决第三方控件异步报错的问题

            m_DiagName = "";
            m_DiagICD = "";
            if (m_DiagTypeName.IndexOf("中医") >= 0)
            {
                m_DiagDataSource = m_App.SqlHelper.ExecuteDataTable(SqlAllDiagChinese, CommandType.Text);
            }
            else
            {
                m_DiagDataSource = m_App.SqlHelper.ExecuteDataTable(SqlAllDiag, CommandType.Text);
            }
            ProcLoadData();

            DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = false;//解决第三方控件异步报错的问题
        }

        private void ProcLoadData()
        {
            m_DiagName = "";
            m_DiagICD = "";
            gridControlDiag.BeginUpdate();
            gridControlDiag.DataSource = m_DiagDataSource;
            gridControlDiag.EndUpdate();
            m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDiag);
            textEditInput.Text = "";
            m_DiagDataSource.DefaultView.RowFilter = "";
        }
        #endregion

        #region EventHandler 
        private void DictionaryDataSerach_Load(object sender, EventArgs e)
        {
            if (m_DiagDataSource == null)
            {
                (new DeleteLoadData(LoadData)).BeginInvoke(null, null);
            }
            else
            {
                ProcLoadData();
            }
        }

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

        private void gridControlDiag_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void textEditInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                gridControlDiag.Focus();
                gridViewDiag.FocusedRowHandle = 0;
            }
        }

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

        private void gridControlDiag_DoubleClick(object sender, EventArgs e)
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
    }
}