using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Wordbook;
using DrectSoft.Core;

namespace DrectSoft.Core.PersonTtemTemplet
{
    public partial class ItemDisplayContent : UserControl
    {
        public string Content
        {
            get { return memoEdit.Text; }
            set { memoEdit.Text = value; }
        }

        public string Code { get; set; }

        public string ParentID { get; set; }

        public string ItemName
        {
            get { return textEditItem.Text; }
            set { textEditItem.Text = value; }
        }

        public string CatalogName
        {
            get { return textEditCatalog.Text; }
            set { textEditCatalog.Text = value; }
        }

        /// <summary>
        /// 是否为个人小模板
        /// </summary>
        public string ISPserson
        {
            //这里给科室小模板和个人小模板的选择有问题，不应该用ifesle分支判断
            //edit by ywk 
            get
            {
                if (this.chkdept.Checked)
                    return "0";
                //else
                else if (chkperson.Checked)
                    return "1";
                else
                    return "";

            }
            set
            {
                if (value == "1")
                    this.chkperson.Checked = true;
                //else
                else if (value == "0")
                    this.chkdept.Checked = true;
                //else
                //    this.chkdept.Checked = false;
                //     this.chkperson.Checked = false;
            }
        }

        public string CreateUser
        {
            get
            {
                return txtCreateUser.Text;
            }
            set
            {
                txtCreateUser.Text = value;
            }
        }

        private void InitCatalog()
        {

            //DataTable table = DataAccessFactory.DefaultDataAccess.ExecuteDataTable("select CCODE,CNAME from dict_catalog where ctype=2");
            //Dictionary<string, int> colwidth = new Dictionary<string, int>();
            //colwidth.Add("CCODE", 100);
            //colwidth.Add("CNAME", 150);
            //SqlWordbook sqlwordbook = new SqlWordbook("query", table, "CCODE", "CNAME", colwidth);
            //lookUpEditor1.SqlWordbook = sqlwordbook;
            //lookUpWindow1.SqlHelper = DataAccessFactory.DefaultDataAccess;
        }

        public ItemDisplayContent()
        {
            InitializeComponent();
            InitFocus();
        }

        private void ItemDisplayContent_Load(object sender, EventArgs e)
        {
            InitCatalog();

        }

        public void Clear()
        {
            textEditCatalog.Text = "";
            textEditItem.Text = "";
            txtCreateUser.Text = "";
            memoEdit.Text = "";
        }

        /// <summary>
        /// 设置内容是否可编辑
        /// </summary>
        /// <param name="flag"></param>
        public void SetTemplateEditFlag(bool flag)
        {
            this.textEditItem.Enabled = flag;
            this.memoEdit.Enabled = flag;
        }
        //清空内容
        public void btn_clear()
        {
            textEditItem.Text = string.Empty;
            memoEdit.Text = string.Empty;
        }
        //设置模板名称焦点
        public void InitFocus()
        {
            this.textEditItem.Focus();
        }
        //设置模板内容焦点
        public void SetContentFocus()
        {
            this.memoEdit.Focus();
        }
    }
}
