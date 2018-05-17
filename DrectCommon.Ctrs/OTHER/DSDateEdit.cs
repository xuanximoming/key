using DevExpress.XtraEditors;
using System;
using System.ComponentModel;
using System.Drawing;

namespace DrectSoft.Common.Ctrs.OTHER
{
    /// <summary>
    /// 功能描述：封装时间选择控件
    ///           用于解决病案首页重绘导致的闪屏和速度问题
    ///           创建人：项令波
    ///           创建时间：2013-04-24
    /// </summary>
    [ToolboxBitmap(typeof(DSDateEdit))]
    [Description("功能描述：封装时间选择控件\r\n-----------------------\r\nxlb  20130424")]
    public partial class DSDateEdit : DateEdit
    {
        public DSDateEdit()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DSDateEdit(IContainer container)
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

        /// <summary>
        /// 重写重绘事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            try
            {
                base.OnPaint(e);
                e.Graphics.DrawLine(new Pen(Brushes.Black), new Point(0, this.Height - 2), new Point(this.Width, this.Height - 2));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
