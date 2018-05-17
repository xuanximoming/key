using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Common;

namespace DrectSoft.Core.PersonTtemTemplet
{
    public partial class ItemContentForm : DevBaseForm
    {
        IEmrHost m_app;
        public TempletItem MyItem;

        public ItemContentForm(IEmrHost app)
        {
            InitializeComponent();
            m_app = app;
            MyItem = new TempletItem();
            itemDisplayContent1.SetTemplateEditFlag(true);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(itemDisplayContent1.ItemName))
            {
                m_app.CustomMessageBox.MessageShow("模板名称不能为空");
                itemDisplayContent1.InitFocus();
                return;
            }
            else if (string.IsNullOrEmpty(itemDisplayContent1.Content))
            {
                m_app.CustomMessageBox.MessageShow("模板内容不能为空");
                itemDisplayContent1.SetContentFocus();
                return;
            }
            else if (Tool.GetByteLength(itemDisplayContent1.ItemName) > 80)
            {
                m_app.CustomMessageBox.MessageShow("模板名称最大长度为80，请重新输入。");
                itemDisplayContent1.InitFocus();
                return;
            }
            else if (Tool.GetByteLength(itemDisplayContent1.ItemName) > 80)
            {
                m_app.CustomMessageBox.MessageShow("模板内容不能为空");
                itemDisplayContent1.SetContentFocus();
                return;
            }

            MyItem.Content = itemDisplayContent1.Content;
            MyItem.ItemName = itemDisplayContent1.ItemName;
            MyItem.ParentID = itemDisplayContent1.ParentID;
            MyItem.CatalogName = itemDisplayContent1.CatalogName;
            MyItem.Code = itemDisplayContent1.Code;
            MyItem.IsPerson = itemDisplayContent1.ISPserson;
            MyItem.CreateUser = itemDisplayContent1.CreateUser;
            //MyItem.Container = itemDisplayContent1.ContainerCode;
            this.DialogResult = DialogResult.OK;
            this.Clear();
        }

        private void ItemContentForm_Load(object sender, EventArgs e)
        {

            itemDisplayContent1.Content = MyItem.Content;
            itemDisplayContent1.ItemName = MyItem.ItemName;
            itemDisplayContent1.ParentID = MyItem.ParentID;
            itemDisplayContent1.CatalogName = MyItem.CatalogName;
            itemDisplayContent1.Code = MyItem.Code;
            itemDisplayContent1.ISPserson = MyItem.IsPerson;
            itemDisplayContent1.CreateUser = MyItem.CreateUser;
            //itemDisplayContent1.ContainerCode = MyItem.Container;
        }

        public void Clear()
        {
            itemDisplayContent1.Clear();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
