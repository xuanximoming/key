using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MedicalRecordManage.UI;
namespace MedicalRecordManage
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
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

                //Local Database
                //SqlHelper.YD_SqlHelper.CreateSqlHelper("System.Data.OracleClient",
                //    "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.2.167)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=ORCL)));User Id=work;Password=orcl;");
                //DrectSoft Server Database
                DrectSoft.DSSqlHelper.DS_SqlHelper.CreateSqlHelper("System.Data.OracleClient",
                    "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.2.13)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=emr)));User Id=dba;Password=sa;");

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
