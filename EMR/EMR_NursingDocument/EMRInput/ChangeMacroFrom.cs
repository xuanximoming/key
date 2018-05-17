using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Xml;
using System.Collections;
using DrectSoft.Library.EmrEditor.Src.Document;
using DrectSoft.Common.Eop;
using DrectSoft.Emr.Util;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace DrectSoft.Core.EMR_NursingDocument.EMRInput.Table
{
    public partial class ChangeMacroFrom : DevExpress.XtraEditors.XtraForm
    {
        PadForm m_CurrentPadForm;
        Inpatient m_CurrentInpatient;
        IEmrHost m_App;

        /// <summary>
        /// .ctor
        /// </summary>
        public ChangeMacroFrom(IEmrHost app, PadForm padForm, Inpatient inpatient)
        {
            InitializeComponent();
            m_App = app;
            m_CurrentPadForm = padForm;
            m_CurrentInpatient = inpatient;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (m_App.CustomMessageBox.MessageShow("确定替换界面中指定的常用词吗？", DrectSoft.Core.CustomMessageBoxKind.QuestionYesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                FillModelMacro();
                this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                this.Close();
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
            this.Close();
        }

        private void FillModelMacro()
        {
            PadForm padForm = m_CurrentPadForm;
            if (padForm != null)
            {
                string deptName = textBoxDept.Text.Trim();
                string wardName = textBoxWard.Text.Trim();

                bool isModified = padForm.zyEditorControl1.EMRDoc.Modified;

                //根据病历的不同状态，调用程序在此处初始化宏的值。
                //替换标题中的宏
                XmlDocument headerdoc = new XmlDocument();
                headerdoc.LoadXml(padForm.zyEditorControl1.EMRDoc.HeadString);
                XmlNodeList nodes = headerdoc.SelectNodes("header/p/macro");
                foreach (XmlNode node in nodes)
                {
                    if (node.Attributes["name"].Value.Trim() == "科室" && !string.IsNullOrEmpty(deptName))
                    {
                        node.InnerText = deptName;
                    }
                    else if (node.Attributes["name"].Value.Trim() == "病区" && !string.IsNullOrEmpty(wardName))
                    {
                        node.InnerText = wardName;
                    }
                }
                padForm.zyEditorControl1.EMRDoc.HeadString = headerdoc.OuterXml;

                //替换文档中的宏
                //获得所有宏元素列表
                ArrayList al = new ArrayList();
                ZYTextDocument doc = padForm.zyEditorControl1.EMRDoc;
                doc.GetAllSpecElement(al, doc.RootDocumentElement, ElementType.Macro, null);

                //循环每个宏元素，根据宏元素的Name属性，查询并赋值线Text属性
                foreach (ZYMacro m in al)
                {
                    if (m.Name == "科室" && !string.IsNullOrEmpty(deptName))
                    {
                        m.Text = deptName;
                    }
                    else if (m.Name == "病区" && !string.IsNullOrEmpty(wardName))
                    {
                        m.Text = wardName;
                    }
                }

                doc.RefreshSize();
                doc.ContentChanged();
                doc.OwnerControl.Refresh();
                doc.UpdateCaret();

                padForm.zyEditorControl1.EMRDoc.Modified = isModified;
            }
        }
    }
}