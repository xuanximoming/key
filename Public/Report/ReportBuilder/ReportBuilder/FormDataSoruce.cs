using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using YidanSoft.Core;
using YidanSoft.FrameWork.BizBus;

namespace YidanSoft.Common.Report
{
    public partial class FormDataSoruce : DevExpress.XtraEditors.XtraForm
    {
        IDataAccess m_SqlHelper;
        IYidanSoftLog m_Logger;
        IBizBus m_BizBus;
        private DataTable m_ReportDataSoruce = new DataTable();
        public DataTable ReportDataSoruce
        {
            get { return m_ReportDataSoruce; }
            set { m_ReportDataSoruce = value; }
        }
        private string m_Path = string.Empty;
        public string Path
        {
            get { return m_Path; }
            set { m_Path = value; }
        }

        private string m_TableName = "NewReport";

        public FormDataSoruce()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonExec_Click(object sender, EventArgs e)
        {
            string strSql = memoEditSql.Text.Trim();
            if (strSql == string.Empty)
                return;
            else
            {
                try
                {
                    this.labelControlWarning.Text = string.Empty;
                    if (this.textEditPath.Text.Trim() != string.Empty)
                    {
                        m_ReportDataSoruce = m_SqlHelper.ExecuteDataTable(strSql);
                        m_Path = this.textEditPath.Text.Trim();
                        m_ReportDataSoruce.TableName = m_TableName;
                        this.DialogResult = DialogResult.OK;
                    }
                }
                catch (Exception ex)
                {
                    this.labelControlWarning.Text = "执行错误！";
                    return;
                }
            }
        }

        /// <summary>
        /// 存储过程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string strSql = memoEditSql.Text.Trim();
            if (strSql == string.Empty)
                return;
            else
            {
                try
                {
                    if (this.textEditPath.Text.Trim() != string.Empty)
                    {
                        m_ReportDataSoruce = m_SqlHelper.ExecuteDataTable(strSql, CommandType.StoredProcedure);
                        m_ReportDataSoruce.TableName = strSql;
                        m_Path = this.textEditPath.Text.Trim();
                        this.DialogResult = DialogResult.OK;
                    }
                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }

        private void Inti()
        {
            m_SqlHelper = DataAccessFactory.DefaultDataAccess;
            m_BizBus = BusFactory.GetBus();
            m_Logger = m_BizBus.BuildUp<IYidanSoftLog>(new string[] { "报表设计器" });
        }

        private void FormDataSoruce_Load(object sender, EventArgs e)
        {
            Inti();
        }

        private void simpleButtonSelect_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.FileName = this.textEditPath.Text;
            DialogResult rt = this.openFileDialog1.ShowDialog();
            if (rt == DialogResult.OK || rt == DialogResult.Yes)
            {
                this.textEditPath.Text = this.openFileDialog1.FileName;
                try
                {
                    m_TableName = this.openFileDialog1.SafeFileName.Split('.')[0].ToString();
                }
                catch
                {
                    m_TableName = "NewReport";
                }
            }
        }
    }
}