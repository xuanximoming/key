using EmrInfirce;
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
                ChangePat changepat = new ChangePat();
                IChangePat ichangepat = changepat;
                string patNum = "0106713_1";
                int i = ichangepat.InitEmr("003322");
                tabPage1.Controls.Add(ichangepat.ChangePatient(patNum));
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

        }
    }
}