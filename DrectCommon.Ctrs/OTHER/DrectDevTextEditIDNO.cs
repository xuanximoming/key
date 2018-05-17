using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors;
using System.Windows.Forms;
using DrectSoft.Common.Ctrs.DLG;
using System.Drawing;
using DevExpress.XtraEditors.Controls;

namespace DrectSoft.Common.Ctrs.OTHER
{
    /// <summary>
    /// 功能描述：封装Dev的TextEdit，主要用于身份证号码
    ///          目标解决：1、限制字符长度为18
    /// 创 建 者：jxh
    /// 创建日期：20130731
    /// </summary>
    [ToolboxBitmap(typeof(TextEdit))]
    [Description("功能描述：封装Dev的TextEdit\r\n-----------------------\r\njxh  20170731")]
    public partial class DevTextEditIDNO : DevExpress.XtraEditors.TextEdit
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern IntPtr GetWindowDC(IntPtr hWnd);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        public DevTextEditIDNO()
        {
            InitializeComponent();
            this.Properties.MaxLength = 18;
        }

        public DevTextEditIDNO(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

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
