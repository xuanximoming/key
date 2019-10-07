using DrectSoft.Core;
using System;
using System.Data;
using System.Windows.Forms;
using System.Xml;

namespace EMRTESTWINDOW
{
    public partial class test : DevExpress.XtraEditors.XtraForm
    {
        private IDataAccess m_EmrHelper;
        private static XmlDocument xmlDoc = new XmlDocument();
        public test()
        {
            InitializeComponent();
        }
        UserControl uc = null;
        private void test_Load(object sender, EventArgs e)
        {
            try
            {

                //ChangePat changepat = new ChangePat();
                //IChangePat ichangepat = changepat;
                //string patNum = "20181231";
                //int i = ichangepat.InitEmr("00", "管理员", "000", "00", "401");
                //uc = ichangepat.ChangePatient(patNum);
                //ichangepat.ChangePatient(this.Handle.ToString(), patNum);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

        }

        private void testbt_Click(object sender, EventArgs e)
        {
            try
            {
                m_EmrHelper = DataAccessFactory.DefaultDataAccess;
                DataTable data = m_EmrHelper.ExecuteDataTable("select * from RECORDDETAIL t where id='1090751'", CommandType.Text);
                foreach (DataRow row in data.Rows)
                {
                    xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(row["content"].ToString());
                    XmlNodeList xmlNode = xmlDoc.ChildNodes;
                    XmlNodeList xmlNodes = xmlNode.Item(0).ChildNodes;
                    foreach (XmlNode node in xmlNodes)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
    }
}