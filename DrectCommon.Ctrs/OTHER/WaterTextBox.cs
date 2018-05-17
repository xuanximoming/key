using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using DrectSoft.Common.Ctrs.DLG;
using DevExpress.XtraEditors;

namespace DrectSoft.Common.Ctrs.OTHER
{
    public partial class WaterTextBox : TextBox
    {
        private readonly Label lblWaterText = new Label();
        public WaterTextBox()
        {
            //InitializeComponent();
            lblWaterText.BorderStyle = BorderStyle.None;
            lblWaterText.Enabled = false;
            lblWaterText.BackColor = Color.White;
            lblWaterText.AutoSize = false;
            lblWaterText.Top = 1;
            lblWaterText.Left = 0;
            Controls.Add(lblWaterText);
        }
        /// <summary>
        /// 显示的水印文字属性
        /// </summary>
        [Category("扩展属性"), Description("显示的提示信息")]
        public string WaterText
        {
            get { return this.lblWaterText.Text; }
            set { this.lblWaterText.Text = value; }
        }
        /// <summary>
        /// 重写父类中的text属性
        /// </summary>
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (value != string.Empty)
                {
                    lblWaterText.Visible = false;
                }
                else
                {
                    lblWaterText.Visible = true;
                }
                base.Text = value;
            }
        }
        /// <summary>
        /// 重写父类中的OnSizeChanged事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSizeChanged(EventArgs e)
        {
            if (Multiline && (ScrollBars == ScrollBars.Vertical || ScrollBars == ScrollBars.Both))
            {
                lblWaterText.Width = Width - 20;
            }
            else
            {
                lblWaterText.Width = Width;
            }
            lblWaterText.Height = Height - 2;
            base.OnSizeChanged(e);
        }
        /// <summary>
        /// 重写父类中的OnEnter事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnEnter(EventArgs e)
        {
            lblWaterText.Visible = false;
            base.OnEnter(e);
        }
        /// <summary>
        /// 重写父类中的OnLeave事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLeave(EventArgs e)
        {
            if (base.Text == string.Empty)
            {
                lblWaterText.Visible = true;
            }
            base.OnLeave(e);
        }
    }
}
