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
    ///        图标确定+快捷键(Alt+I)
    /// 创 建 者：bwj
    /// 创建日期：20121023
    /// </summary>
    [ToolboxBitmap(typeof(DevButtonImportExcel))]
    [Description("功能描述：封装一个确定的导出Excel按钮\r\n-----------------------\r\nbwj  20121023")]
    public partial class DevButtonImportExcel : SimpleButton
    {
        private ImageList aImageList = new System.Windows.Forms.ImageList();
        public enum ImageTypes
        {
            ImportExcel,
            ImportImage,
            ImportOther
        }
        private ImageTypes _ImageSelect = ImageTypes.ImportExcel;
        /// <summary>
        /// 选择按钮图标
        /// </summary>
        [Description("选择按钮图标\r\nbwj 20121024")]
        [Browsable(true)]
        public ImageTypes ImageSelect
        {
            get
            {
                return _ImageSelect;
            }
            set
            {
                _ImageSelect = value;
                ResetImageList();
                this.Image = aImageList.Images.Count > 0 ? aImageList.Images[(int)_ImageSelect] : DrectSoft.Common.Ctrs.Properties.Resources.导出Excel.ToBitmap();
               
            }
        }

        protected void ResetImageList()
        {
            try
            {
                aImageList.Images.Clear();
                aImageList.Images.AddRange(
                        new Image[] {
                        DrectSoft.Common.Ctrs.Properties.Resources.导出Excel.ToBitmap() ,
                        DrectSoft.Common.Ctrs.Properties.Resources.导出图片.ToBitmap(),
                        DrectSoft.Common.Ctrs.Properties.Resources.导出.ToBitmap() 
                     }
                        );
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        protected override void OnCreateControl()
        {
            try
            {
                base.OnCreateControl();
                ResetImageList();

                this.Image = aImageList.Images.Count > 0 ? aImageList.Images[(int)_ImageSelect] : DrectSoft.Common.Ctrs.Properties.Resources.导出Excel.ToBitmap();
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        public DevButtonImportExcel()
        {
            
            try
            {
                InitializeComponent();

                ResetImageList();
                
                this.Image = aImageList.Images.Count > 0 ? aImageList.Images[(int)_ImageSelect] : DrectSoft.Common.Ctrs.Properties.Resources.导出Excel.ToBitmap();
                this.Width = 80;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DevButtonImportExcel(IContainer container)
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
                this.Text = "导出(&I)";
               
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
    }
}
