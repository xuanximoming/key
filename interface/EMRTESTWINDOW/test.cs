using DrectSoft.Core;
using System;
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
                //AnalysisXML xml = new AnalysisXML();
                //DataTable dtdate = xml.GetMedicalInsurance("AC,AB", "006425", "主诉,现病史,既往史");
                //richTextBox1.Text = "";
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
                System.Diagnostics.Process.Start(".\\app\\adcemr.exe", "00");
                //m_EmrHelper = DataAccessFactory.DefaultDataAccess;
                //string getImgStr = @"SELECT PicData FROM PicTable where ID='1' and PicID='1'";
                //DataTable data = m_EmrHelper.ExecuteDataTable(getImgStr, CommandType.Text);
                //byte[] bytes = (byte[])data.Rows[0]["PicData"];
                ////byte[] bytes = System.Text.Encoding.Default.GetBytes(imgStr);
                //MemoryStream ms = new System.IO.MemoryStream(bytes);
                //Image img = System.Drawing.Image.FromStream(ms);
                //pictureBox1.Image = img;
                //foreach (DataRow row in data.Rows)
                //{
                //    xmlDoc = new XmlDocument();
                //    xmlDoc.LoadXml(row["content"].ToString());
                //    XmlNodeList xmlNode = xmlDoc.ChildNodes;
                //    XmlNodeList xmlNodes = xmlNode.Item(0).ChildNodes;
                //    foreach (XmlNode node in xmlNodes)
                //    {

                //    }
                //}
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
    }
}