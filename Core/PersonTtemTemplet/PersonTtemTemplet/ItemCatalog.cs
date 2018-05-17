using System;
using System.Collections.Generic;
using System.ComponentModel;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm.Plugin;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.Core;
using DrectSoft.Wordbook;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Common;

namespace DrectSoft.Core.PersonTtemTemplet
{
    public partial class ItemCatalog : DevBaseForm
    {
        IEmrHost m_app;
        /// <summary>
        /// 模板名称
        /// </summary>
        public string ITEMNAME { get; set; }

        public string Memo { get; set; }

        public string ContainerCode { get; set; }

        //是否为个人模块
        public string IsPerson { get; set; }

        //创建人
        public string CreateUser { get; set; }
        //区别大分类和小分类的标识  ywk 2012年6月11日 16:03:40
        public string BigOrSmallItem { get; set; }
        //区别适应病历夹和模板类型是否可修改(添加提示)
        public bool CanChangeFlag { get; set; }
        
        public ItemCatalog(IEmrHost app)
        {
            InitializeComponent();
            m_app = app;
            CanChangeFlag = true;
        }

        /// <summary>
        /// 初始化页面塞值
        /// </summary>
        public void InitPageValue(string itemname, string memo, string containercode, string isPerson, string createUser)
        {
            this.txtITEMNAME.Text = ITEMNAME;
            this.memoEdit.Text = Memo;
            this.lookUpEditor1.CodeValue = containercode;
            if (isPerson == "1")
                chkperson.Checked = true;
            else
                chkdept.Checked = true;
            //新增大分类可选择，新增小分类默认选中，不可编辑
            if (BigOrSmallItem == "small")
            {
                //跳到此处就已经赋值了选中科室还是个人小模板
                chkperson.Enabled = false;
                chkdept.Enabled = false;
            }
            if (BigOrSmallItem == "big")
            {
                //跳到此处就已经赋值了选中科室还是个人小模板
                chkperson.Enabled = true;
                chkdept.Enabled = true;
            }
            this.txtCreateUser.Text = createUser;
        }

        private void InitCatalog()
        {
            DataTable table = DataAccessFactory.DefaultDataAccess.ExecuteDataTable("select CCODE,CNAME from dict_catalog where ctype=2");
            //根据妇科提出需求，在科室小模板中添加通用类型
            DataRow dr = table.NewRow();
            dr["CCODE"] = "99";
            dr["CNAME"] = "通用";
            table.Rows.Add(dr);
            table.Columns["CCODE"].Caption = "编码";
            table.Columns["CNAME"].Caption = "名称";
            Dictionary<string, int> colwidth = new Dictionary<string, int>();
            colwidth.Add("CCODE", 50);
            colwidth.Add("CNAME", 108);
            SqlWordbook sqlwordbook = new SqlWordbook("query", table, "CCODE", "CNAME", colwidth);
            lookUpEditor1.SqlWordbook = sqlwordbook;
            lookUpWindow1.SqlHelper = DataAccessFactory.DefaultDataAccess;

        }

        private void ItemCatalog_Load(object sender, EventArgs e)
        {
            InitCatalog();

            this.txtITEMNAME.Text = ITEMNAME;
            this.memoEdit.Text = Memo;
            this.lookUpEditor1.CodeValue = ContainerCode;

            if (IsPerson == "1")
                chkperson.Checked = true;
            else
                chkdept.Checked = true;
            this.txtCreateUser.Text = CreateUser;

            txtITEMNAME.Properties.AllowFocused = true;
            txtITEMNAME.Focus();
            simpleButton1.AllowFocus = false;
            simpleButton2.AllowFocus = false;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtITEMNAME.Text))
            {
                m_app.CustomMessageBox.MessageShow("分类名称不能为空");
                txtITEMNAME.Focus();
                return;
            }
            if (string.IsNullOrEmpty(this.lookUpEditor1.Text))
            {
                m_app.CustomMessageBox.MessageShow("适应病历夹不能为空");
                lookUpEditor1.Focus();
                return;
            }
            if (Tool.GetByteLength(this.txtITEMNAME.Text) > 64)
            {
                m_app.CustomMessageBox.MessageShow("分类名称的长度不能超过64，请重新输入。");
                txtITEMNAME.Focus();
                return;
            }
            if (Tool.GetByteLength(this.memoEdit.Text) > 255)
            {
                m_app.CustomMessageBox.MessageShow("备注的长度不能超过255，请重新输入。");
                memoEdit.Focus();
                return;
            }
            ITEMNAME = this.txtITEMNAME.Text;
            Memo = this.memoEdit.Text;
            ContainerCode = this.lookUpEditor1.CodeValue;
            IsPerson = chkdept.Checked ? "0" : "1";
            CreateUser = this.txtCreateUser.Text;

            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 设置适应适应病历夹的编辑状态
        /// </summary>
        public void SetContainerCodeEditFlag(bool flag)
        {
            this.lookUpEditor1.Enabled = flag;
        }
        /// <summary>
        /// 设置模板类型的编辑状态
        /// 科室小模板&个人小模板
        /// </summary>
        public void SetIsPersonEditFlag(bool flag)
        {
            this.chkdept.Enabled = flag;
            this.chkperson.Enabled = flag;
        }

        #region "工具提示事件" 
        private void lookUpEditor1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!CanChangeFlag)
            {
                toolTipController1.SetToolTip(lookUpEditor1, "该分类包含子集，不可修改适应病历夹");
                toolTipController1.SetToolTipIconType(lookUpEditor1, DevExpress.Utils.ToolTipIconType.Exclamation);
                toolTipController1.ShowBeak = true;
                toolTipController1.ShowShadow = true;
                toolTipController1.Rounded = true;
                toolTipController1.ShowHint("该分类包含子集，不可修改适应病历夹", lookUpEditor1.PointToScreen(e.Location));
            }
        }
        private void chkdept_MouseMove(object sender, MouseEventArgs e)
        {
            if (!CanChangeFlag)
            {
                InitToolTipChk("该分类包含子集，不可修改模板类型", chkdept, chkdept.PointToScreen(e.Location));
            }
        }
        private void chkperson_MouseMove(object sender, MouseEventArgs e)
        {
            if (!CanChangeFlag)
            {
                InitToolTipChk("该分类包含子集，不可修改模板类型", chkperson, chkdept.PointToScreen(e.Location));
            }
        }
        //设置工具提示
        private void InitToolTipChk(string displayName,DevExpress.XtraEditors.CheckEdit item, Point point)
        {
            if (displayName.Trim() != "")
            {
                toolTipController1.SetToolTip(item, displayName);
                toolTipController1.SetToolTipIconType(item, DevExpress.Utils.ToolTipIconType.Exclamation);
                toolTipController1.ShowBeak = true;
                toolTipController1.ShowShadow = true;
                toolTipController1.Rounded = true;
                toolTipController1.ShowHint(displayName, point);
            }
        }
        #endregion

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
