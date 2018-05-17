using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraTreeList.Nodes;
using DrectSoft.Emr.Util;
using System.Collections;
using DrectSoft.Library.EmrEditor.Src.Document;

namespace DrectSoft.Core.EMR_NursingDocument.EMRInput.Table
{
    public partial class DailyTemplateForm : Form
    {
        string m_labelControlInfoText = string.Empty;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="lastTime"></param>
        /// <param name="title"></param>
        /// <param name="isFirstDailyEmr">是否是首次病程，对于首次病程没有时间限制</param>
        public DailyTemplateForm(DateTime lastTime, string title, bool isFirstDailyEmr)
        {
            InitializeComponent();
            if (!isFirstDailyEmr)
            {
                labelControl_Info.Text = lastTime.ToString("yyyy-MM-dd HH:mm:ss");
                m_labelControlInfoText = labelControl_Info.Text;
                this.labelControl_Info.Text = "病程日期不能小于 " + m_labelControlInfoText;
            }
            else
            {
                labelControl_Info.Text = "";
                labelControl3.Text = "";
                m_labelControlInfoText = "";
            }
            dateEdit_Date.DateTime = DateTime.Now.Date;
            timeEdit_Time.Time = DateTime.Now;
            textEdit_Name.Text = title;
        }

        public DateTime CommitDateTime
        {
            get { return DateTime.Parse(dateEdit_Date.DateTime.Date.ToString("yyyy-MM-dd")+" "+timeEdit_Time.Time.ToString("HH:mm:ss")); }
        }

        public string CommitTitle
        {
            get { return textEdit_Name.Text; }
        }

        private void BindBasicInfo()
        {
            //


        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (m_AllDisplayDateTime.Contains(CommitDateTime.ToString("yyyy-MM-dd HH:mm:ss")))
            {
                this.labelControl_Info.Text = "病程日期已存在";
                return;
            }

            if (m_labelControlInfoText != "" && (CommitDateTime - Convert.ToDateTime(m_labelControlInfoText)).TotalMinutes < 0)
            {
                this.labelControl_Info.Text = "病程日期不能小于 " + m_labelControlInfoText;
                return;
            }
            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }

        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private List<string> m_AllDisplayDateTime = new List<string>();

        /// <summary>
        /// 设置病程的所有时间
        /// </summary>
        /// <param name="node"></param>
        public void SetAllDisplayDateTime(PadForm padForm)
        {
            try
            {
                if (padForm != null)
                {
                    ArrayList al = new ArrayList();
                    padForm.zyEditorControl1.EMRDoc.GetAllSpecElement(al, padForm.zyEditorControl1.EMRDoc.RootDocumentElement, ElementType.Text, "记录日期");
                    if (al.Count > 0)
                    {
                        foreach (ZYText ele in al)
                        {
                            m_AllDisplayDateTime.Add(DateTime.Parse(ele.Text).ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
