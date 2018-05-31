using EmrInfirce;
using System;
using System.Windows.Forms;

namespace EMRTESTWINDOW
{
    public partial class test : DevExpress.XtraEditors.XtraForm
    {
        public test()
        {
            InitializeComponent();
        }
        UserControl uc = null;
        private void test_Load(object sender, EventArgs e)
        {
            try
            {
                ChangePat changepat = new ChangePat();
                IChangePat ichangepat = changepat;
                string patNum = "0106713_1";
                int i = ichangepat.InitEmr("003322");
                uc = ichangepat.ChangePatient(patNum);
                if (uc == null)
                {
                    return;
                }
                tabPage1.Controls.Clear();
                tabPage1.Controls.Add(uc);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

        }
    }
}