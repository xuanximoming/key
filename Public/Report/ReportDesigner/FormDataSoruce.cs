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
using System.Configuration;

namespace YidanSoft.Common.Report
{
    public partial class FormDataSoruce : XtraForm
    {
        IDataAccess m_SqlHelper;
        private DataSet m_ReportDataSoruce = new DataSet();
        public DataSet ReportDataSoruce
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

        private void InitDB()
        {
            if (ConfigurationManager.ConnectionStrings.Count < 0) return;

            foreach (ConnectionStringSettings str in ConfigurationManager.ConnectionStrings)
            {
                comboBoxEdit1.Properties.Items.Add(str.Name);
            }
            comboBoxEdit1.SelectedText = "EMRDB";

        }



        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonExec_Click(object sender, EventArgs e)
        {
            if (comboBoxEdit1.SelectedItem == null) return; ;

            m_SqlHelper = DataAccessFactory.GetSqlDataAccess(comboBoxEdit1.Text);

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
                        m_ReportDataSoruce = m_SqlHelper.ExecuteDataSet(strSql);
                        m_Path = this.textEditPath.Text.Trim();
                        //m_ReportDataSoruce.TableName = m_TableName;
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
                        m_SqlHelper = DataAccessFactory.GetSqlDataAccess(comboBoxEdit1.SelectedText);
                        m_ReportDataSoruce = m_SqlHelper.ExecuteDataSet(strSql, CommandType.StoredProcedure);

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


        private void FormDataSoruce_Load(object sender, EventArgs e)
        {
            InitDB();
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