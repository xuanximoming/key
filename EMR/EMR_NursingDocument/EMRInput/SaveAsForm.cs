using DrectSoft.Emr.Util;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Windows.Forms;

namespace DrectSoft.Core.EMR_NursingDocument.EMRInput.Table
{
    public partial class SaveAsForm : DevExpress.XtraEditors.XtraForm
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
                if (m_Model == null) return;

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
                    m_app.CustomMessageBox.MessageShow("保存成功！");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow("保存失败！" + ex.Message);
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