/***********************************************************************************************************************别文进插件*/
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
    /// 功能描述：封装Dev风格的按钮
    ///        图标确定+快捷键(Alt+P)
    /// 创 建 者：bwj
    /// 创建日期：20121016
    /// </summary>
    [ToolboxBitmap(typeof(DevButtonPrint))]
    [Description("功能描述：封装一个确定的打印按钮\r\n-----------------------\r\nbwj  20121016")]
    public partial class DevButtonPrint : SimpleButton
    {
        public DevButtonPrint()
        {

            try
            {
                InitializeComponent();
                System.Drawing.Icon img = DrectSoft.Common.Ctrs.Properties.Resources.打印;
                this.Image = img.ToBitmap();

                this.Width = 80;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DevButtonPrint(IContainer container)
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
                this.Text = "打印(&P)";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
