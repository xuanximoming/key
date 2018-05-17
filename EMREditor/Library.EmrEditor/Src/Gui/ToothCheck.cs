using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Library.EmrEditor.Src.Document;

namespace DrectSoft.Library.EmrEditor.Src.Gui
{
    /// <summary>
    /// 新增牙齿检查功能
    ///      |
    ///   1  |   2
    /// ------------
    ///      |    
    ///   3  |    4
    /// add by ywk 2012年11月27日11:35:36 
    /// 青龙山精神病院需求
    /// </summary>
    public partial class ToothCheck : DevExpress.XtraEditors.XtraForm
    {

        ZYToothCheck zcheck = null;
        public ToothCheck()
        {
            InitializeComponent();
        }
        public ToothCheck(ZYTextElement zyelement)
        {
            InitializeComponent();
            Point p = Control.MousePosition;
            this.Location = p;
            zcheck = (ZYToothCheck)zyelement;

            this.txtLeftUp.Text = zcheck.LeftUp;
            this.txtleftDown.Text = zcheck.LeftDown;
            this.txtrightup.Text = zcheck.RightUp;
            this.txtrightDown.Text = zcheck.RightDown;
        }
        /// <summary>
        /// 确定按钮 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonOK_Click(object sender, EventArgs e)
        {
            zcheck.LeftUp = txtLeftUp.Text;
            zcheck.LeftDown = txtleftDown.Text;
            zcheck.RightUp = txtrightup.Text;
            zcheck.RightDown = txtrightDown.Text;
            this.Close();
            zcheck.OwnerDocument.RefreshSize();
            zcheck.OwnerDocument.ContentChanged();
            zcheck.OwnerDocument.OwnerControl.Refresh();
        }
        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 同取消操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToothCheck_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }



    }
}