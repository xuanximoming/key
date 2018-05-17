using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using DrectSoft.FrameWork.WinForm.Plugin;
#pragma warning disable 0618

namespace DrectSoft.Core.CommonTableConfig
{
    class LoadPrintBiz
    {
        private IEmrHost m_app;
        public LoadPrintBiz(IEmrHost app)
        {
            this.m_app = app;
        }

        /// <summary>
        /// 获取打印模板的列表
        /// </summary>
        /// <returns></returns>
        public List<EmrPrintTempEntity> GetPrintTempList()
        {
            string sql = @"select * from EMR_PRINTTEMP e where e.IsValide='1'";
            DataTable dtPrintList = m_app.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
            List<EmrPrintTempEntity> emrPrintTempEntityList = DataTableToList<EmrPrintTempEntity>.ConvertToModel(dtPrintList);
            return emrPrintTempEntityList;
        }

        public bool AddOrModelPrintTempEntity(EmrPrintTempEntity emrPrintTempEntity)
        {
            bool result = false;
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[] { 
                  new SqlParameter("@PrintFlow", SqlDbType.VarChar,500),
                  new SqlParameter("@PrintName", SqlDbType.VarChar,500),
                  new SqlParameter("@PrintFileName", SqlDbType.VarChar,500),
                  new SqlParameter("@CreateDoctorID", SqlDbType.VarChar,500),
                  new SqlParameter("@CreateDoctorName", SqlDbType.VarChar,500),
                  new SqlParameter("@CreateDateTime", SqlDbType.VarChar,500),
                  new SqlParameter("@IsValide", SqlDbType.VarChar,1)
            };
                sqlParams[0].Value = emrPrintTempEntity.PrintFlow;
                sqlParams[1].Value = emrPrintTempEntity.PrintName;
                sqlParams[2].Value = emrPrintTempEntity.PrintFileName;
                sqlParams[3].Value = emrPrintTempEntity.CreateDoctorID;
                sqlParams[4].Value = emrPrintTempEntity.CreateDoctorName;
                sqlParams[5].Value = emrPrintTempEntity.CreateDateTime;
                sqlParams[6].Value = emrPrintTempEntity.IsValide;

                m_app.SqlHelper.ExecuteNoneQuery("EMR_CommonNote.usp_AddorModEmrPrintTemp", sqlParams, CommandType.StoredProcedure);
                UpdateContent(emrPrintTempEntity);
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;


        }


        private void UpdateContent(EmrPrintTempEntity emrPrintTempEntity)
        {

            OracleConnection conn = new OracleConnection(m_app.SqlHelper.GetDbConnection().ConnectionString);
            conn.Open();
            OracleCommand cmd = conn.CreateCommand();
            byte[] printContent = emrPrintTempEntity.PrintContentbyte;
            try
            {
                cmd.CommandText = "update EMR_PRINTTEMP e set e.printcontent=:data where e.printflow=:printflow;";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                OracleParameter paraClob = new OracleParameter("data", OracleType.Clob);
                OracleParameter paraflow = new OracleParameter("printflow", OracleType.Clob);

                paraClob.Value = Convert.ToBase64String(printContent);
                paraflow.Value = emrPrintTempEntity.PrintFlow;
                cmd.Parameters.Add(paraClob);
                cmd.Parameters.Add(paraflow);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
                conn.Dispose();
            }


        }






    }
}
