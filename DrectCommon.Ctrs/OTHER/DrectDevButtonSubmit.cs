using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors;
using System.Drawing;
using System.Windows.Forms;

namespace DrectSoft.Common.Ctrs.OTHER
{
    /// <summary>
    /// 功能描述:封装Dev风格的按钮
    /// 图标确定+快捷键(Alt+Z)
    /// 创建者：项令波
    /// 创建时间：2013-03-01
    /// </summary>
    [ToolboxBitmap(typeof(DevButtonSubmit))]
    [Description("功能描述：封装提交按钮\r\n-----------------------\r\nxlb  20130301")]
    public partial class DevButtonSubmit : SimpleButton
    {
        public DevButtonSubmit()
        {
            try
            {
                InitializeComponent();
                System.Drawing.Icon img = DrectSoft.Common.Ctrs.Properties.Resources.提交;
                this.Image = img.ToBitmap();

                this.Width = 80;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DevButtonSubmit(IContainer container)
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

        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            try
            {
                base.OnValidated(e);
                this.Text = "提交(&Z)";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    
    }
}
