using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.Core;

namespace DrectSoft.Core.EMR_NursingDocument.EMRInput.Table
{
    public partial class ChoiceForm : Form
    {
        private DataTable m_SourceTable;

        public DataRow CommitRow { get; set; }

        private IDataAccess m_sqlHelper;

        public ChoiceForm(IDataAccess sqlHelper, DataTable sourceTable)
        {
            InitializeComponent();
            m_sqlHelper = sqlHelper;
            m_SourceTable = sourceTable;

        }

        private void BindSource()
        {

            //Dictionary<string, int> cols = new Dictionary<string, int>();
            //cols.Add("D_NAME", 150);
            //cols.Add("INPUT", 150);
            //m_SourceTable.Columns["D_NAME"].Caption = "名称";
            //m_SourceTable.Columns["INPUT"].Caption = "拼音";
            //lookUpWindow1.SqlHelper = m_sqlHelper;
            //DrectSoft.Wordbook.SqlWordbook sqlbook = new DrectSoft.Wordbook.SqlWordbook("query", m_SourceTable, "D_NAME", "D_NAME", cols, true);
            //lookUpEditor1.Kind = DrectSoft.Wordbook.WordbookKind.Sql;
            //lookUpEditor1.SqlWordbook = sqlbook;
            gridControlTool.DataSource = m_SourceTable;

        }

        private void lookUpEditor1_CodeValueChanged(object sender, EventArgs e)
        {
            //if (lookUpEditor1.HadGetValue)
            //{
            //    CommitRow = lookUpEditor1.ListWindow.ResultRows[0];
            //    this.DialogResult = System.Windows.Forms.DialogResult.OK;
            //}
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            CommitRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (CommitRow != null)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;

            }
        }

        private void ChoiceForm_Load(object sender, EventArgs e)
        {
            BindSource();
        }

        private void textEditName_TextChanged(object sender, EventArgs e)
        {
            string rowFilter = " D_NAME like '%{0}%' or INPUT like '%{0}%'";
            m_SourceTable.DefaultView.RowFilter = string.Format(rowFilter, textEditName.Text.Trim());
        }




    }
}
