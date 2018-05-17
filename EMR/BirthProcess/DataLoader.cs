using System;
using System.Data;
using System.Data.SqlClient;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace DrectSoft.Core.BirthProcess
{
    /// <summary>
    /// 数据加载业务类
    /// </summary>
    public class DataLoader
    {
        public static DateTime m_currentDate = DateTime.Now;
        IEmrHost m_app;
        IDataAccess m_sql;

        public DataLoader(IEmrHost app)
        {
            try
            {
                m_app = app;
                m_sql = app.SqlHelper;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetBirthProcessData()
        {
            try
            {
                DataTable dt = null;
                SqlParameter[] sqlParams = null;
                string sqlStr = string.Format(@"select * from birthprocess bp1,birthprocessstate bp2 where bp1.id=bp2.starttime and bp1.valid='1' and bp2.valid='1' and bp1.noofinpat={0}", DrectSoft.Common.CommonObjects.CurrentPatient.NoOfFirstPage);
                dt = m_sql.ExecuteDataTable(sqlStr, sqlParams, CommandType.Text);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveBirthProcessData()
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetDateTimeForColumns(DateTime DateTime)
        {
            throw new NotImplementedException();
        }
    }
}
