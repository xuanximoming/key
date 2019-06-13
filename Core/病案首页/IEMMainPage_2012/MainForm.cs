using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using YidanSoft.FrameWork.WinForm.Plugin;

namespace YidanSoft.Core.IEMMainPage
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        IYidanEmrHost m_Host;
        DrawMainPageUtil util;
        IemMainPageManger manger;
        IemMainPageInfo info;

        public MainForm(IYidanEmrHost host)
        {
            InitializeComponent();
            m_Host = host;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            manger = new IemMainPageManger(m_Host);
            info = manger.GetIemInfo();
            util = new DrawMainPageUtil(info);

            pictureBox1.BackgroundImage = util.MF1;
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;

            pictureBox2.BackgroundImage = util.MF2;
            pictureBox2.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            ReLocationPicture();
        }

        /// <summary>
        /// 重新定位PictureBox
        /// </summary>
        private void ReLocationPicture()
        {
            if (pictureBox1.Height < panelContainer.Height - 21)
            {
                pictureBox1.Location = new Point(panelContainer.Width / 2 - pictureBox1.Width - 20, panelContainer.AutoScrollPosition.Y);
                pictureBox2.Location = new Point(panelContainer.Width / 2 + 20, panelContainer.AutoScrollPosition.Y);
            }
            else
            {
                pictureBox1.Location = new Point((panelContainer.Width - pictureBox1.Width) / 2, panelContainer.AutoScrollPosition.Y);
                pictureBox2.Location = new Point((panelContainer.Width - pictureBox1.Width) / 2, pictureBox1.Location.Y + pictureBox1.Height + 20);
            }

            ReLocationEditButton();
        }


        #region 编辑Button

        const float percentHeight1 = 0.1334f;
        const float percentHeight2 = 0.3286f;
        const float percentHeight3 = 0.5381f;
        const float percentHeight4 = 0.1905f;
        const float percentHeight5 = 0.4286f - 0.1905f;
        const float percentHeight6 = 0.3519f;
        private void ReLocationEditButton()
        {
            simpleButton1.Visible = false;
            simpleButton1.Height = Convert.ToInt32(percentHeight1 * pictureBox1.Height);
            simpleButton1.Location = new Point(pictureBox1.Location.X - simpleButton1.Width, pictureBox1.Location.Y);

            simpleButton2.Height = Convert.ToInt32(percentHeight2 * pictureBox1.Height);
            simpleButton2.Location = new Point(pictureBox1.Location.X - simpleButton1.Width, pictureBox1.Location.Y + simpleButton1.Height);

            simpleButton3.Height = Convert.ToInt32(percentHeight3 * pictureBox1.Height);
            simpleButton3.Location = new Point(pictureBox1.Location.X - simpleButton1.Width, pictureBox1.Location.Y + simpleButton1.Height + simpleButton2.Height);

            simpleButton4.Height = Convert.ToInt32(percentHeight4 * pictureBox2.Height);
            simpleButton4.Location = new Point(pictureBox2.Location.X - simpleButton1.Width, pictureBox2.Location.Y);

            if (info.IemObstetricsBaby != null)
            {
                simpleButton5.Height = Convert.ToInt32(percentHeight5 * pictureBox2.Height);
                simpleButton5.Location = new Point(pictureBox2.Location.X - simpleButton1.Width, pictureBox2.Location.Y + simpleButton4.Height);

                simpleButton6.Height = Convert.ToInt32(percentHeight6 * pictureBox2.Height);
                simpleButton6.Location = new Point(pictureBox2.Location.X - simpleButton1.Width, pictureBox2.Location.Y + simpleButton4.Height + simpleButton5.Height);
            }
            else
            {
                simpleButton6.Height = Convert.ToInt32(pictureBox2.Height - simpleButton4.Height);
                simpleButton6.Location = new Point(pictureBox2.Location.X - simpleButton1.Width, pictureBox2.Location.Y + simpleButton4.Height + simpleButton5.Height);
            }

        }

        private bool IsFocusButton()
        {
            if (simpleButton1.Focused)
            {
                return true;
            }
            if (simpleButton2.Focused)
            {
                return true;
            }
            if (simpleButton3.Focused)
            {
                return true;
            }
            if (simpleButton4.Focused)
            {
                return true;
            }
            if (simpleButton5.Focused)
            {
                return true;
            }
            if (simpleButton6.Focused)
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}