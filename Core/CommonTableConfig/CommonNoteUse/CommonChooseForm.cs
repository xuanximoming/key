using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    public partial class CommonChooseForm : DevBaseForm
    {
        private IEmrHost m_app;
        List<CommonNoteEntity> commonNoteEntityList;
        public CommonNoteEntity SelectCommonNoteEntity;
        string formName = string.Empty;//窗体名用来接收前一级窗体信息
        public CommonChooseForm(IEmrHost app)
        {
            m_app = app;
            InitializeComponent();
            InitDate();
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="formCaption"></param>
        /// <param name="app"></param>
        public CommonChooseForm(string formCaption,IEmrHost app)
        {
            try
            {
                m_app = app;
                InitializeComponent();
                //接收窗体传值
                formName = formCaption;
                InitDate();
                this.Text = formName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化模板列表信息
        /// </summary>
        private void InitDate()
        {
            try
            {
                CommonNoteBiz commonNoteBiz = new CommonNoteBiz(m_app);
                commonNoteEntityList = commonNoteBiz.GetCommonNoteByDeptWard(m_app.User.CurrentWardId, "02");
                foreach (var item in commonNoteEntityList)
                {
                    item.CreateDateTime = DrectSoft.Common.DateUtil.getDateTime(item.CreateDateTime, DrectSoft.Common.DateUtil.NORMAL_LONG);
                }
                gridControl1.DataSource = commonNoteEntityList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            string namestr = txtName.Text.Trim();
            List<CommonNoteEntity> commonNoteListFilt = commonNoteEntityList.FindAll(a => a.CommonNoteName.Contains(namestr) || a.WBM.Contains(namestr) || a.PYM.Contains(namestr)) as List<CommonNoteEntity>;
            gridControl1.DataSource = commonNoteListFilt;
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            commit();
        }

        /// <summary>
        /// 修改单据名
        /// edit by xlb
        /// 2013-01-31
        /// </summary>
        private void commit()
        {
            try
            {
                SelectCommonNoteEntity = gridView1.GetFocusedRow() as CommonNoteEntity;
                if (SelectCommonNoteEntity != null)
                {
                    ChangeNameForm ChangeNameForm = new ChangeNameForm(SelectCommonNoteEntity);
                    DialogResult dResult = ChangeNameForm.ShowDialog();
                    if (dResult == DialogResult.OK)
                    {
                        this.DialogResult = DialogResult.OK;
                        //选择的模板对象名改为修改后的名称
                        SelectCommonNoteEntity.CommonNoteName = ChangeNameForm.m_CommonNoteEntity.CommonNoteName;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            }

        /// <summary>
        /// 确定事件
        /// xlb 2013-01-29 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                commit();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 取消事件
        /// xlb 2013-01-29
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = System.Windows.Forms.DialogResult.No;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
    }
}