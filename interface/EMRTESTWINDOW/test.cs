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
                string patNum = "20181231";
                int i = ichangepat.InitEmr("00", "管理员", "000", "00", "401");
                //uc = ichangepat.ChangePatient(patNum);
                ichangepat.ChangePatientOut(this.Handle.ToString(), patNum);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

        }
    }
}