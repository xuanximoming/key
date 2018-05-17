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
    /// 功能描述：封装下移按钮
    ///           下移图标加快捷键(ALT+X)
    /// 创建人：项令波
    /// 创建时间:2013-04-25
    /// </summary>
    [ToolboxBitmap(typeof(DevButtonDown))]
    [Description("功能描述：封装下移按钮按钮\r\n-----------------------\r\nbxlb  20130425")]
    public partial class DevButtonDown : SimpleButton
    {
        public DevButtonDown()
        {
            try
            {
                InitializeComponent();
                Icon img=DrectSoft.Common.Ctrs.Properties.Resources.下移;
                this.Image = img.ToBitmap();
                this.Width = 80;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DevButtonDown(IContainer container)
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
                this.Text = "下移(&X)";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
