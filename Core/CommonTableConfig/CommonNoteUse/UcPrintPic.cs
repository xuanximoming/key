using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    public partial class UcPrintPic : DevExpress.XtraEditors.XtraUserControl
    {
        public UcPrintPic(Bitmap bitMap)
        {
            InitializeComponent();
            //picImg.Image = bitMap;
            picImg.BackgroundImage = bitMap;
            picImg.Width = bitMap.Width;
            picImg.Height = bitMap.Height;
            picImg.BackgroundImageLayout = ImageLayout.Stretch;
        }

    }
}
