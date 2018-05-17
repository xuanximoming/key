using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace DrectSoft.Core.EMR_NursingDocument.EMRInput.Table
{
    public partial class ReportPacs : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 返回值
        /// </summary>
        public StringBuilder CommitValue { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="examItemName">检查项目</param>
        /// <param name="examTime">检查时间</param>
        /// <param name="examStatus">检查状态</param>
        /// <param name="desc">描述</param>
        /// <param name="diagnisis">诊断</param>
        /// <param name="memo">建议</param>
        /// <param name="docName">检查医师</param>
        /// <param name="finalExamTime">出报告时间</param>
        public ReportPacs(string examItemName , string examTime , string examStatus , 
            string desc, string diagnisis, string memo, string docName, string finalExamTime)
            : this()
        {
            textEditExamName.Text = examItemName.Trim();
            textEditExamTime.Text = examTime.Trim();
            textEditExamStatus.Text = examStatus.Trim();
            memoEditDescribe.Text = desc.Trim();
            memoEditDiagnosis.Text = diagnisis.Trim();
            memoEditJianYi.Text = memo.Trim();
            textEditDocName.Text = docName.Trim();
            textEditFinalTime.Text = finalExamTime.Trim();
        }

        public ReportPacs()
        {
            InitializeComponent();
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            Commit();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ReportPacs_Load(object sender, EventArgs e)
        {
            btn_Save.Focus();
        }

        private void Commit()
        {
            CommitValue = new StringBuilder();
            CommitValue = CommitValue.Append(textEditExamName.Text);

            if (checkBox描述.Checked)
            {
                CommitValue = CommitValue.Append(memoEditDescribe.Text);
            }

            if (checkBox诊断.Checked)
            {
                CommitValue = CommitValue.Append(memoEditDiagnosis.Text);
            }

            if (checkBox建议.Checked)
            {
                CommitValue = CommitValue.Append(memoEditJianYi.Text);
            }
        }
    }
}