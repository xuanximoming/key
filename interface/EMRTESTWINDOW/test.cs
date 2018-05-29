using System;
using System.Reflection;
using System.Windows.Forms;

namespace EMRTESTWINDOW
{
    public partial class test : DevExpress.XtraEditors.XtraForm
    {
        public test()
        {
            InitializeComponent();
        }
        Assembly emr = null;
        UserControl uc = null;
        MethodInfo mthod = null;
        private void test_Load(object sender, EventArgs e)
        {
            try
            {
                string patNum = "002623_1";
                emr = Assembly.LoadFile(Application.StartupPath.ToString() + "\\EmrInfirce.dll");
                object emrUC = emr.CreateInstance("EmrInfirce.UCEmr");
                mthod = emrUC.GetType().GetMethod("Shuaxin");
                uc = (UserControl)emrUC;
                uc.Dock = DockStyle.Fill;
                mthod.Invoke(uc, new object[] { patNum });
                tabPage1.Controls.Add(uc);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

        }
    }
}