using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;

namespace DrectSoft.Common.Ctrs.OTHER
{
    /// <summary>
    /// 功能描述：封装一个复合控件，含有textbox以及button
    /// 创建者：lx
    /// 创建日期：20130104
    /// </summary>
    [ToolboxBitmap(typeof(DevTextBox))]
    [Description("功能描述：封装一个复合控件\r\n------\r\nbwj  20130104")]
    public partial class DevTextBox : SimpleButton
    {
        private bool _IsEnterKeyToNextControl = false;
        private bool _IsSaveNextControl = false;
        // private bool _IsEnterChangeBgColor = false;
        private Color _oldBgColor;

        /// <summary>
        /// 按回车键是弹出combox下拉列表
        /// </summary>
        //[Description("值为true时，按Enter键 将弹出医生诊断的数据录入\r\nbwj 20130104")]
        //[Browsable(true)]
        public bool IsEnterKeyToNextControl
        {
            get
            {
                return _IsEnterKeyToNextControl;
            }
            set
            {
                //if (value)
                //{
                //    this.KeyUp += new KeyEventHandler(txt_KeyUp);
                //}
                //else
                //{
                //    this.KeyUp -= new KeyEventHandler(txt_KeyUp);
                //}
                _IsEnterKeyToNextControl = value;
            }
        }

        public DevTextBox()
        {
            try
            {
                InitializeComponent();
                System.Drawing.Icon img = DrectSoft.Common.Ctrs.Properties.Resources.保存;
                this.Image = img.ToBitmap();
                this.Width = 80;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DevTextBox(IContainer container)
        {
            try
            {
                container.Add(this);
                InitializeComponent();
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //protected override void OnPaint(PaintEventArgs pe)
        //{
        //    base.OnPaint(pe);
        //}

        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            try
            {
                base.OnInvalidated(e);
                this.Text = "保存(&S)";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form frm = new Form();
            frm.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                Form frm = new Form();
                frm.Show();
            }
        }
    }
}
