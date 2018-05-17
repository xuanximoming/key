using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors;
using System.Drawing;

namespace DrectSoft.Common.Ctrs.OTHER
{
    /// <summary>
    /// 功能描述：重绘数字框底部线条
    ///           用于病案首页的需要
    /// 创建人：项令波
    /// 创建时间：2013-04-24
    /// </summary>
    public partial class DSSpinEdit : SpinEdit
    {
        public DSSpinEdit()
        {
            InitializeComponent();
        }

        public DSSpinEdit(IContainer container)
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
        /// 重写绘制事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            try
            {
                base.OnPaint(e);
                e.Graphics.DrawLine(new Pen(Brushes.Black),new Point(0,this.Height-2),new Point(this.Width,this.Height-2));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
