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
                int i = ichangepat.InitEmr("100032", "管理员", "000", "00", "00");
                //uc = ichangepat.ChangePatient(patNum);
                ichangepat.ChangePatient(this.Handle.ToString(), patNum);
                //if (uc == null)
                //{
                //    return;
                //}

                // this.Controls.Clear();
                // this.Controls.Add(uc);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

        }
    }
}