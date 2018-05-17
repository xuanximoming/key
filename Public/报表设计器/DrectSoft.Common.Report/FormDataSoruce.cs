using DevExpress.XtraEditors;
using System;
using System.Configuration;
using System.Data;
using DrectSoft.Core;

namespace DrectSoft.Common.Report
{
    public partial class FormDataSoruce : XtraForm
    {
        private IDataAccess m_SqlHelper;

        private DataSet m_ReportDataSoruce = new DataSet();

        private string m_Path = string.Empty;

        private string m_TableName = "NewReport";

        public DataSet ReportDataSoruce
        {
            get
            {
                return this.m_ReportDataSoruce;
            }
            set
            {
                this.m_ReportDataSoruce = value;
            }
        }

        public string Path
        {
            get
            {
                return this.m_Path;
            }
            set
            {
                this.m_Path = value;
            }
        }

        public FormDataSoruce()
        {
            this.InitializeComponent();
        }

        private void InitDB()
        {
            if (ConfigurationManager.ConnectionStrings.Count >= 0)
            {
                foreach (ConnectionStringSettings connectionStringSettings in ConfigurationManager.ConnectionStrings)
                {
                    this.comboBoxEdit1.Properties.Items.Add(connectionStringSettings.Name);
                }
                this.comboBoxEdit1.SelectedText = "EMRDB";
            }
        }

        private void simpleButtonExec_Click(object sender, System.EventArgs e)
        {
            if (this.comboBoxEdit1.SelectedItem != null)
            {
                this.m_SqlHelper = DataAccessFactory.GetSqlDataAccess(this.comboBoxEdit1.Text);
                string text = this.memoEditSql.Text.Trim();
                if (!(text == string.Empty))
                {
                    try
                    {
                        this.labelControlWarning.Text = string.Empty;
                        if (this.textEditPath.Text.Trim() != string.Empty)
                        {
                            this.m_ReportDataSoruce = this.m_SqlHelper.ExecuteDataSet(text);
                            this.m_Path = this.textEditPath.Text.Trim();
                            base.DialogResult = System.Windows.Forms.DialogResult.OK;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.labelControlWarning.Text = "执行错误！";
                        throw ex;
                    }
                }
            }
        }

        private void simpleButton2_Click(object sender, System.EventArgs e)
        {
            string text = this.memoEditSql.Text.Trim();
            if (!(text == string.Empty))
            {
                try
                {
                    if (this.textEditPath.Text.Trim() != string.Empty)
                    {
                        this.m_SqlHelper = DataAccessFactory.GetSqlDataAccess(this.comboBoxEdit1.SelectedText);
                        this.m_ReportDataSoruce = this.m_SqlHelper.ExecuteDataSet(text, CommandType.StoredProcedure);
                        this.m_Path = this.textEditPath.Text.Trim();
                        base.DialogResult = System.Windows.Forms.DialogResult.OK;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private void FormDataSoruce_Load(object sender, System.EventArgs e)
        {
            this.InitDB();
        }

        private void simpleButtonSelect_Click(object sender, System.EventArgs e)
        {
            this.openFileDialog1.FileName = this.textEditPath.Text;
            System.Windows.Forms.DialogResult dialogResult = this.openFileDialog1.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK || dialogResult == System.Windows.Forms.DialogResult.Yes)
            {
                this.textEditPath.Text = this.openFileDialog1.FileName;
                try
                {
                    this.m_TableName = this.openFileDialog1.SafeFileName.Split(new char[]
					{
						'.'
					})[0].ToString();
                }
                catch
                {
                    this.m_TableName = "NewReport";
                }
            }
        }
    }
}
