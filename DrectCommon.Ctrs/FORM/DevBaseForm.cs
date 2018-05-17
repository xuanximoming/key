using DevExpress.LookAndFeel;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DrectSoft.Common.Ctrs.FORM
{
    public partial class DevBaseForm : DevExpress.XtraEditors.XtraForm
    {
        public DevBaseForm()
        {
            InitializeComponent();
            try
            {
                //窗体初始化
                initForm();
                ;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        private void DevBaseForm_Load(object sender, EventArgs e)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Icon\\DrectSoftLogo.ico"))
            {
                Icon myicon = new System.Drawing.Icon(AppDomain.CurrentDomain.BaseDirectory + "\\Icon\\DrectSoftLogo.ico", 32, 32);
                this.Icon = myicon;
            }
        }

        /// <summary>
        /// 设置界面皮肤
        /// Add by wwj 2013-05-28
        /// </summary>
        public void SetSkin()
        {
            try
            {
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Skin.txt"))
                {
                    File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Skin.txt", "DevExpress Style");
                }
                string skin = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Skin.txt");
                if (skin != "")
                {
                    DevExpress.UserSkins.TouchSkins.Register();
                    DevExpress.Skins.SkinManager.EnableFormSkins();
                    UserLookAndFeel.Default.SetSkinStyle(skin);
                }
            }
            catch (Exception e)
            { }
        }

        /// <summary>
        /// 窗体初始化
        /// </summary>
        private void initForm()
        {
            try
            {
                //do初始化代码

                //这样不方便后期更换图片，直接将图标放置程序启动文件夹下
                //edit by ywk 2013年3月8日12:42:01 
                //this.Icon = Common.Ctrs.Properties.Resources.logo_20;

            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        /// <summary>
        /// 回车键处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if ((int)e.KeyCode == 13)
                {
                    SendKeys.Send("{Tab}");
                    SendKeys.Flush();
                }
            }
            catch (Exception ce)
            {
                MessageBox.Show(ce.Message.ToString(), "志扬软件");
                return;
            }
        }

    }
}