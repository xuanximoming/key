using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using YidanSoft.FrameWork.WinForm.Plugin;
using YidanSoft.Wordbook;
using YiDanCommon.Ctrs.FORM;

namespace YindanSoft.Emr.QcManagerNew
{
    public partial class QCScoreItemEdit : DevBaseForm
    {
        IYidanEmrHost m_app;

        public QCScoreItem_DataEntity m_scoreitem;

        SqlManger m_sqlManger;

        /// <summary>
        /// 如果保存成功刷新父页面数据
        /// </summary>
        public bool ISRefresh = false;

        public QCScoreItemEdit(IYidanEmrHost app)
        {
            InitializeComponent();
            m_app = app;
            
        }

        private void QCScoreItemEdit_Load(object sender, EventArgs e)
        {
            m_sqlManger = new SqlManger(m_app);
            BindScoreType();

            BindDataToPage();

            this.txtname.Focus();
        }

        /// <summary>
        /// 绑定页面下拉框
        /// </summary>
        private void BindScoreType()
        {
            lookUpWindowType.SqlHelper = m_app.SqlHelper;

            DataTable Dept = m_sqlManger.GetQCTypeScore();

            Dept.Columns["TYPECODE"].Caption = "科室代码";
            Dept.Columns["TYPENAME"].Caption = "科室名称";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("TYPECODE", 65);
            cols.Add("TYPENAME", 160);

            SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "TYPECODE", "TYPENAME", cols, "TYPECODE//TYPENAME");
            lookUpEditorType.SqlWordbook = deptWordBook;
        }

        /// <summary>
        /// 将页面值塞入到实体中
        /// </summary>
        private QCScoreItem_DataEntity SetEntity()
        {
            string messagestr = "";
            if (this.txtname.Text.Trim() == "")
            {
                messagestr += "请填写项目名称！";
            }

            if (this.lookUpEditorType.CodeValue == "")
            {
                messagestr += "请选择对应大项！";
            }

            if (this.cmbcategory.SelectedIndex == -1)
            {
                messagestr += "请选择项目类型！";
            }

            if (messagestr != "")
            {
                m_app.CustomMessageBox.MessageShow(messagestr);
                return null;
            }

            QCScoreItem_DataEntity scoreItem = new QCScoreItem_DataEntity();

            //scoreItem.Itemcode = dataRow["ItemCode"].ToString();
            scoreItem.Itemname = this.txtname.Text.Trim();
            scoreItem.Iteminstruction = this.txtInstruction.Text.Trim();
            scoreItem.Itemdefaultscore = (int)this.spinDefaultScore.Value;
            scoreItem.Itemstandardscore = (int)this.spinStandardScore.Value;
            scoreItem.Itemcategory = (int)this.cmbcategory.SelectedIndex;
            scoreItem.Itemdefaulttarget = (int)this.spinDefaultTarget.Value;
            scoreItem.Itemtargetstandard = (int)this.spinTargetStandard.Value;
            scoreItem.Itemscorestandard = (int)this.spinScoreStandard.Value;
            scoreItem.Itemorder = (int)this.spinOrder.Value;
            scoreItem.Typecode = this.lookUpEditorType.CodeValue;

            scoreItem.Itemmemo = this.txtmemo.Text;

            return scoreItem;

        }

        /// <summary>
        /// 清空页面值
        /// </summary>
        private void ClearPage()
        {
            this.txtname.Text = "";
            this.txtInstruction.Text = "";
            this.spinDefaultScore.Value = 0;
            this.spinStandardScore.Value = 0;
            this.cmbcategory.SelectedIndex = -1;
            this.spinDefaultTarget.Value = 0;
            this.spinTargetStandard.Value = 0;
            this.spinScoreStandard.Value = 0;
            this.spinOrder.Value = 0;
            this.lookUpEditorType.CodeValue = "";

            this.txtmemo.Text = "";
        }

        /// <summary>
        /// 将实体中值填写到页面中
        /// </summary>
        /// <param name="scoreType"></param>
        private void BinPageByEntity(QCScoreItem_DataEntity scoreItem)
        {
            if (scoreItem != null)
            {
                this.txtname.Text = scoreItem.Itemname;
                this.txtInstruction.Text = scoreItem.Iteminstruction;
                this.spinDefaultScore.Value = scoreItem.Itemdefaultscore;
                this.spinStandardScore.Value = scoreItem.Itemstandardscore;
                this.cmbcategory.SelectedIndex = scoreItem.Itemcategory;
                this.spinDefaultTarget.Value = scoreItem.Itemdefaulttarget;
                this.spinTargetStandard.Value = scoreItem.Itemtargetstandard;
                this.spinScoreStandard.Value = scoreItem.Itemscorestandard;
                this.spinOrder.Value = scoreItem.Itemorder;
                this.lookUpEditorType.CodeValue = scoreItem.Typecode;

                this.txtmemo.Text = scoreItem.Itemmemo;
            }
        }

        private void BindDataToPage()
        {
            if (m_scoreitem == null)
            {
                ClearPage();
            }
            else
            {
                BinPageByEntity(m_scoreitem);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            QCScoreItem_DataEntity socreItementity = SetEntity();
            if (socreItementity == null)
                return;
            string edittype = "";
            //表示为新增
            if (m_scoreitem == null)
            {
                edittype = "0";
            }
            else
            {
                edittype = "1";
                socreItementity.Itemcode = m_scoreitem.Itemcode;
            }

            SqlManger m_sqlmanger = new SqlManger(m_app);
            m_sqlmanger.EditQCItemScore(edittype, socreItementity);

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