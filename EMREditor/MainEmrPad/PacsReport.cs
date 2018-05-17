using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Core.MainEmrPad
{
    public partial class PacsReport : DevBaseForm
    {
        public PacsReport()
        {
            InitializeComponent();
        }

        public string GetResultStringType()
        {
            char[] c = { '\n', '\r' };
            return memoEditResult.Text.Trim().Trim(c);
        }

        public List<Image> GetResultImageType()
        {
            List<Image> list = new List<Image>();
            Image image1 = pictureBox1.BackgroundImage;
            Image image2 = pictureBox2.BackgroundImage;
            list.Add(image1);
            list.Add(image2);
            return list;
        }
    }
}