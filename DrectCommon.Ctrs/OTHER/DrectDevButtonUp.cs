using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Drawing;
using DevExpress.XtraEditors;
using System.Windows.Forms;

namespace DrectSoft.Common.Ctrs.OTHER
{
    /// <summary>
    /// 功能描述：封装Dev风格按钮
    ///           图标上移加+快捷键(ALT+N)
    /// 创建者：项令波
    /// 创建时间：2013-04-25
    /// </summary>
    [ToolboxBitmap(typeof(DevButtonUp))]
    [Description("功能描述：封装上移按钮按钮\r\n-----------------------\r\nbxlb  20130425")]
    public partial class DevButtonUp : SimpleButton
    {
        public DevButtonUp()
        {
            try
            {
                InitializeComponent();
                Icon img = DrectSoft.Common.Ctrs.Properties.Resources.上移;
                this.Image = img.ToBitmap();
                this.Width = 80;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DevButtonUp(IContainer container)
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
                base.OnInvalidated(e);
                this.Text = "上移(&N)";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
