using System;
using System.Drawing;
using System.Windows.Forms;

namespace DrectSoft.Core.BirthProcess
{
    public partial class BirthProcessImage : Form
    {
        DrawHepler m_DrawHelper = new DrawHepler();

        public BirthProcessImage()
        {
            InitializeComponent();
        }

        private void BirthProcessImage_Load(object sender, EventArgs e)
        {
            m_DrawHelper.DrawDataImage();
            pictureBoxImage.BackgroundImage = DrawHepler.m_dataImage;
            BirthProcessImage_Resize(null, null);
        }

        private void BirthProcessImage_Resize(object sender, EventArgs e)
        {
            pictureBoxImage.Left = (this.Width - DrawHepler.dataIamgeSize.Width) / 2;
            pictureBoxImage.Top = panel1.Top;
            pictureBoxImage.Size = new Size(DrawHepler.dataIamgeSize.Width, DrawHepler.dataIamgeSize.Height);
            pictureBoxImage.Height = DrawHepler.dataIamgeSize.Height;
        }
    }
}