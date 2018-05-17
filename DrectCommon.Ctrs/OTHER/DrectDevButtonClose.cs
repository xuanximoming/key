using DevExpress.XtraEditors;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

/***********************************************************************************************************************别文进插件*/
namespace DrectSoft.Common.Ctrs.OTHER
{
    /// <summary>
    /// 功能描述：封装Dev风格的按钮
    ///        图标确定+快捷键(Alt+T)
    /// 创 建 者：bwj
    /// 创建日期：20121016
    /// </summary>
    [ToolboxBitmap(typeof(DevButtonClose))]
    [Description("功能描述：封装一个确定的关闭按钮\r\n-----------------------\r\nbwj  20121016")]
    public partial class DevButtonClose : SimpleButton
    {
        public DevButtonClose()
        {

            try
            {
                InitializeComponent();
                System.Drawing.Icon img = DrectSoft.Common.Ctrs.Properties.Resources.关闭;
                this.Image = img.ToBitmap();

                this.Width = 80;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DevButtonClose(IContainer container)
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
                this.Text = "关闭(&T)";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
