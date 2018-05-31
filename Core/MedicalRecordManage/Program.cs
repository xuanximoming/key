using System;
using System.Windows.Forms;
namespace MedicalRecordManage
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            initStartInfo();
            Application.Run(new FormMain());
        }
        static bool initStartInfo()
        {
            try
            {
                bool result = true;
                DrectSoft.DSSqlHelper.DS_SqlHelper.CreateSqlHelper();
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace.ToString(), "提示");
                return false;
            }
        }
    }
}
