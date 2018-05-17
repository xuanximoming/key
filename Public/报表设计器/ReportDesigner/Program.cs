using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;
namespace ReportDesigner
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序入口函数
        /// </summary>
        /// <param name="args">传入的参数</param>
        [System.STAThread]
        private static void Main(string[] args)
        {
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-cn");
            BonusSkins.Register();
            SkinManager.EnableFormSkins();
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            UserLookAndFeel.Default.UseWindowsXPTheme = false;
            UserLookAndFeel.Default.Style = LookAndFeelStyle.Skin;
            MainForm mainForm = new MainForm();
            System.Windows.Forms.Application.Run(mainForm);
        }
    }
}
