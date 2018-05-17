using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Emr.Util;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Windows.Forms;

namespace DrectSoft.Core.MainEmrPad
{
    public partial class SaveAsForm : DevBaseForm
    {
        internal IEmrHost m_app;
        EmrModel m_Model;
        RecordDal m_RecordDal;
        string m_Content;

        public SaveAsForm()
        {
            InitializeComponent();
        }

        public SaveAsForm(EmrModel model, IEmrHost app, RecordDal recordDal, string content)
            : this()
        {
            m_Model = model;
            m_app = app;
            m_RecordDal = recordDal;
            m_Content = content;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void simpleButtonSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void simpleButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Save()
        {
            try
            {
                if (m_Model == null)
                {
                    return;
                }
                if (Tool.GetByteLength(memoEditMemo.Text) > 2000)
                {
                    MyMessageBox.Show("备注最大字符长度为2000，请重新输入。", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                    return;
                }
                string type = "1";
                if (checkBoxPersonal.Checked)
                {
                    type = "1";//个人模板
                }
                else
                {
                    type = "2";//科室模板
                }

                bool isOK = m_RecordDal.InsertTemplatePerson(m_Model, m_app.User.Id, memoEditMemo.Text, m_Content, type, m_app.User.CurrentDeptId);
                if (isOK)
                {
                    MyMessageBox.Show("保存成功", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.InformationIcon);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show("保存失败" + ex.Message, "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.ErrorIcon);
            }
        }

        private void SaveAsForm_Load(object sender, EventArgs e)
        {
            InitForm();
        }

        private void InitForm()
        {
            textEditName.Text = m_Model.ModelName.Trim();
            textEditType.Text = m_RecordDal.GetCatalog(m_Model.ModelCatalog);
        }

        private void checkBoxPersonal_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxPersonal.Checked)
            {
                checkBoxDepartment.Checked = false;
            }
            else
            {
                checkBoxDepartment.Checked = true;
            }
        }

        private void checkBoxDepartment_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxDepartment.Checked)
            {
                checkBoxPersonal.Checked = false;
            }
            else
            {
                checkBoxPersonal.Checked = true;
            }
        }
    }
}