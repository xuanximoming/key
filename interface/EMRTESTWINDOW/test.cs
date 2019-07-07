using DrectSoft.Core;
using System;
using System.Data;
using System.Data.SqlClient;
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
                //ichangepat.ChangePatientOut(this.Handle.ToString(), patNum);
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
                DataTable data = m_EmrHelper.ExecuteDataTable("select * from RECORDDETAIL t where t.createtime >= (select max(t.createtime) from ANALYSISDOC t )", CommandType.Text);
                foreach (DataRow row in data.Rows)
                {
                    int flag = 1;
                    xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(row["content"].ToString());
                    XmlNodeList xmlNode = xmlDoc.GetElementsByTagName("selement");
                    SqlParameter[] sqlParams = new SqlParameter[] { 
                  new SqlParameter("@id", SqlDbType.VarChar, 50),
                  new SqlParameter("@recid", SqlDbType.Int, 9),
                  new SqlParameter("@noofinpat", SqlDbType.Int, 9),
                  new SqlParameter("@sortid", SqlDbType.VarChar, 50),
                  new SqlParameter("@elementnames", SqlDbType.VarChar, 50),
                  new SqlParameter("@elementcode", SqlDbType.VarChar, 50),
                  new SqlParameter("@elementvalue", SqlDbType.VarChar, 50),
                  new SqlParameter("@createtime", SqlDbType.VarChar, 50),
                  new SqlParameter("@flag", SqlDbType.Int, 1)
                };
                    sqlParams[1].Value = Decimal.Parse(row["ID"].ToString());
                    sqlParams[2].Value = int.Parse(row["NOOFINPAT"].ToString());
                    sqlParams[3].Value = row["SORTID"].ToString();
                    sqlParams[7].Value = row["CREATETIME"].ToString();
                    foreach (XmlNode node in xmlNode)
                    {
                        sqlParams[8].Value = flag;
                        sqlParams[0].Value = Guid.NewGuid().ToString();
                        sqlParams[4].Value = node.Attributes["name"].Value;
                        sqlParams[5].Value = node.Attributes["code"].Value;
                        sqlParams[6].Value = node.Attributes["text"].Value;
                        m_EmrHelper.ExecuteDataTable("AnalysisDocPak.usp_updateOrInsertAnalysisDoc", sqlParams, CommandType.StoredProcedure);
                        flag = 0;
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