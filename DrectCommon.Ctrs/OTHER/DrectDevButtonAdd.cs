/***********************************************************************************************************************别文进插件*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors;
using System.Windows.Forms;
using DevExpress.Utils;
using System.Drawing;

namespace DrectSoft.Common.Ctrs.OTHER
{
    /// <summary>
    /// 功能描述：封装一个确定的新增按钮
    ///    图标确定+快捷键确定(Alt+A)
    /// 创 建 者：bwj
    /// 创建日期：20121015
    /// </summary>
    [ToolboxBitmap(typeof(DevButtonAdd))]
    [Description("功能描述：封装一个确定的新增按钮\r\n-----------------------\r\nbwj  20121015")]
    public partial class DevButtonAdd : SimpleButton
    {
        public DevButtonAdd()
        {

            try
            {
                InitializeComponent();
                
                
                System.Drawing.Icon img=DrectSoft.Common.Ctrs.Properties.Resources.新增 ;
                this.Image = img.ToBitmap();
                
                this.Width = 80;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DevButtonAdd(IContainer container)
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
                this.Text = "新增(&A)";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }
}
