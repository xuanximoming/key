using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System.Drawing;
using System.Windows.Forms;

namespace DrectSoft.Common.Ctrs.OTHER
{
    /// <summary>
    /// 功能描述：重绘文本控件底部加线条
    ///           用于病案首页编辑界面
    /// 创建人：项令波
    /// 创建时间：2013-04-22
    /// </summary>
    [ToolboxBitmap(typeof(DSTextBox))]
    [Description("功能描述：封装重绘文本框\r\n-----------------------\r\nxlb 20130422")]
    public partial class DSTextBox : TextEdit
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern IntPtr GetWindowDC(IntPtr hWnd);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        protected override void WndProc(ref Message m)
        {
            try
            {
                base.WndProc(ref m);

                //去边框
                this.BorderStyle = BorderStyles.NoBorder;
                if (m.Msg == 0xf || m.Msg == 0x133)
                {
                    IntPtr hDC = GetWindowDC(m.HWnd);
                    if (hDC.ToInt32() == 0)
                    {
                        return;
                    }
                    //只有在边框样式为FixedSingle时自定义边框样式才有效 

                    //绘制边框 
                    System.Drawing.Graphics g = Graphics.FromHdc(hDC);
                    g.DrawLine(new Pen(Brushes.Black), new Point(0, this.Height - 2), new Point(this.Width, this.Height - 2));

                    //返回结果 
                    m.Result = IntPtr.Zero;
                    //释放 
                    ReleaseDC(m.HWnd, hDC);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
