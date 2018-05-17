using DrectSoft.FrameWork.Log;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DrectSoft.Core.TemplatePerson
{
    internal abstract class DataAccess
    {
        private static DrectSoft.Core.IDataAccess m_SqlHelper;

        private static IEmrHost m_App;

        public static IEmrHost App
        {
            get
            {
                return DataAccess.m_App;
            }
            set
            {
                DataAccess.m_App = value;
                DataAccess.m_SqlHelper = DataAccess.m_App.SqlHelper;
            }
        }

        public static void InsertTemplatePersonGroup(int nodeID, int parentNodeID, int templatePersonID, string name, string userID)
        {
            SqlParameter[] array = new SqlParameter[]
            {
                new SqlParameter("@UserID", SqlDbType.VarChar),
                new SqlParameter("@NodeID", SqlDbType.Int),
                new SqlParameter("@TemplatePersonID", SqlDbType.Int),
                new SqlParameter("@ParentNodeID", SqlDbType.Int),
                new SqlParameter("@Name", SqlDbType.VarChar),
                new SqlParameter("@TypeID", SqlDbType.Int)
            };
            array[0].Value = userID;
            array[1].Value = nodeID;
            array[2].Value = templatePersonID;
            array[3].Value = parentNodeID;
            array[4].Value = name;
            array[5].Value = 1;
            DataAccess.m_SqlHelper.ExecuteNoneQuery("usp_InsertTemplatePersonGroup", array, CommandType.StoredProcedure);
        }

        public static void CancelTemplatePersonGroup(string userID)
        {
            SqlParameter[] array = new SqlParameter[]
            {
                new SqlParameter("@UserID", SqlDbType.VarChar),
                new SqlParameter("@TypeID", SqlDbType.Int)
            };
            array[0].Value = userID;
            array[1].Value = 2;
            DataAccess.m_SqlHelper.ExecuteNoneQuery("usp_InsertTemplatePersonGroup", array, CommandType.StoredProcedure);
        }

        public static DataSet GetTemplatePersonGroup()
        {
            string iD = DataAccess.m_App.User.Id;
            SqlParameter[] array = new SqlParameter[]
            {
                new SqlParameter("@UserID", SqlDbType.VarChar)
            };
            array[0].Value = iD;
            return DataAccess.m_SqlHelper.ExecuteDataSet("usp_GetTemplatePersonGroup", array, CommandType.StoredProcedure);
        }

        public static void SavePersonalModel(DataTable dataTable)
        {
            try
            {
                DataAccess.m_SqlHelper.BeginTransaction();
                string iD = DataAccess.m_App.User.Id;
                DataAccess.CancelTemplatePersonGroup(iD);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int nodeID = Convert.ToInt32(dataRow["NodeID"]);
                    int parentNodeID = Convert.ToInt32(dataRow["ParentNodeID"]);
                    int templatePersonID = Convert.ToInt32(dataRow["TemplatePersonID"]);
                    string name = dataRow["NodeName"].ToString();
                    DataAccess.InsertTemplatePersonGroup(nodeID, parentNodeID, templatePersonID, name, iD);
                }
            }
            catch (Exception ex)
            {
                DataAccess.m_SqlHelper.RollbackTransaction();
                Log log = new Log();
                log.Write(ex.ToString());
            }
            finally
            {
                DataAccess.m_SqlHelper.CommitTransaction();
            }
        }

        public static void SaveTemplatePerson(DataTable dt)
        {
            try
            {
                DataAccess.m_SqlHelper.BeginTransaction();
                string iD = DataAccess.m_App.User.Id;
                foreach (DataRow dataRow in dt.Rows)
                {
                    string name = dataRow["Name"].ToString();
                    string memo = dataRow["Memo"].ToString();
                    string id = dataRow["ID"].ToString();
                    DataAccess.InsertTemplatePerson(name, memo, iD, id);
                }
            }
            catch (Exception ex)
            {
                DataAccess.m_SqlHelper.RollbackTransaction();
                Log log = new Log();
                log.Write(ex.ToString());
            }
            finally
            {
                DataAccess.m_SqlHelper.CommitTransaction();
            }
        }

        private static void InsertTemplatePerson(string name, string memo, string userID, string id)
        {
            SqlParameter[] array = new SqlParameter[]
            {
                new SqlParameter("@UserID", SqlDbType.VarChar),
                new SqlParameter("@TypeID", SqlDbType.Int),
                new SqlParameter("@TemplatePersonName", SqlDbType.VarChar),
                new SqlParameter("@TemplatePersonMemo", SqlDbType.VarChar),
                new SqlParameter("@TemplatePersonID", SqlDbType.Int)
            };
            array[0].Value = userID;
            array[1].Value = 3;
            array[2].Value = name;
            array[3].Value = memo;
            array[4].Value = Convert.ToInt32(id);
            DataAccess.m_SqlHelper.ExecuteNoneQuery("usp_InsertTemplatePersonGroup", array, CommandType.StoredProcedure);
        }
    }
}
