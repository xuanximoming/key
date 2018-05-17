using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using YidanSoft.FrameWork.WinForm.Plugin;
using YiDanCommon.Ctrs.FORM;

namespace YindanSoft.Emr.QcManagerNew
{
    public partial class QCScoreTypeEdit : DevBaseForm
    {
        IYidanEmrHost m_app;
        public QCScoreType_DataEntity m_ScoreType;

        /// <summary>
        /// 如果保存成功刷新父页面数据
        /// </summary>
        public bool ISRefresh = false;

        public QCScoreTypeEdit(IYidanEmrHost app)
        {
            InitializeComponent();
            m_app = app;
           
        }

        private void QCScoreTypeEdit_Load(object sender, EventArgs e)
        {
            BindDataToPage();
            this.txttypename.Focus();
        }

        private void BindDataToPage()
        {
            if (m_ScoreType == null)
            {
                ClearPage();
            }
            else
            {
                BinPageByEntity(m_ScoreType);
            }
        }

        /// <summary>
        /// 清空页面值
        /// </summary>
        private void ClearPage()
        {
            this.txttypename.Text = "";
            this.txttypeinstruction.Text = "";
            this.cmbtypecategory.SelectedIndex = -1;
            this.txttypeorder.Value = 0;
            this.txttypememo.Text = "";
        }

        /// <summary>
        /// 将实体中值填写到页面中
        /// </summary>
        /// <param name="scoreType"></param>
        private void BinPageByEntity(QCScoreType_DataEntity scoreType)
        {
            if (scoreType != null)
            {
                this.txttypename.Text = scoreType.Typename;
                this.txttypeinstruction.Text = scoreType.Typeinstruction;
                this.cmbtypecategory.SelectedIndex = scoreType.Typecategory;
                this.txttypeorder.Value = scoreType.Typeorder;
                this.txttypememo.Text = scoreType.Typememo;
            }
        }

        /// <summary>
        /// 将页面值塞入到实体中
        /// </summary>
        private QCScoreType_DataEntity SetEntity()
        {
            string messagestr = "";
            if (this.txttypename.Text.Trim() == "")
            {
                messagestr += "请填写项目名称！";
            }

            if (this.txttypeinstruction.Text.Trim() == "")
            {
                messagestr += "请填写项目说明！";
            }

            if (this.cmbtypecategory.SelectedIndex == -1)
            {
                messagestr += "请选择项目类型！";
            }

            if (messagestr != "")
            {
                m_app.CustomMessageBox.MessageShow(messagestr);
                return null;
            }

            QCScoreType_DataEntity scoreType = new QCScoreType_DataEntity();

            scoreType.Typename = this.txttypename.Text;
            scoreType.Typeinstruction = this.txttypeinstruction.Text;
            scoreType.Typecategory = this.cmbtypecategory.SelectedIndex;
            scoreType.Typeorder = (int)this.txttypeorder.Value;
            scoreType.Typememo = this.txttypememo.Text;

            return scoreType;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            QCScoreType_DataEntity socretypeentity = SetEntity();
            if (socretypeentity == null)
                return;
            string edittype = "";
            //表示为新增
            if (m_ScoreType == null)
            {
                edittype = "0";
            }
            else
            {
                edittype = "1";
                socretypeentity.Typecode = m_ScoreType.Typecode;
            }

            SqlManger m_sqlmanger = new SqlManger(m_app);
            m_sqlmanger.EditQCTypeScore(edittype, socretypeentity);

            m_app.CustomMessageBox.MessageShow("保存成功！");
            ISRefresh = true;
            this.Close();

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearPage();
        }

        private void KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == (char)13)
                {
                    SendKeys.Send("{Tab}");
                    SendKeys.Flush();
                }
            }
            catch (Exception ex)
            {

            }
        }

    }
}