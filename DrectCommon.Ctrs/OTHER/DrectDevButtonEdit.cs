/***********************************************************************************************************************别文进插件*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors;
using System.Windows.Forms;
using System.Drawing;

namespace DrectSoft.Common.Ctrs.OTHER
{
    /// <summary>
    /// 功能描述：封装Dev风格的按钮
    ///        图标确定+快捷键(Alt+E)
    /// 创 建 者：bwj
    /// 创建日期：20121015
    /// </summary>
    [ToolboxBitmap(typeof(DevButtonEdit))]
    [Description("功能描述：封装一个确定的编辑按钮\r\n-----------------------\r\nbwj  20121015")]
    public partial class DevButtonEdit : SimpleButton
    {
        public DevButtonEdit()
        {

            try
            {
                InitializeComponent();
                System.Drawing.Icon img = DrectSoft.Common.Ctrs.Properties.Resources.编辑;
                this.Image = img.ToBitmap();

                this.Width = 80;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DevButtonEdit(IContainer container)
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
                this.Text = "编辑(&E)";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
