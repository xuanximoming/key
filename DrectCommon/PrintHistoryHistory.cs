using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DrectSoft.Common
{
    public class PrintHistoryHistory
    {
        public static List<PrintHistoryEntity> GetPrintHistory(string inCommnoteflow)
        {
            try
            {
                string strsql = string.Format("@select * from printhistory p where p.printrecordflow='{0}'", inCommnoteflow);
                DataTable dt = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(strsql, CommandType.Text);
                List<PrintHistoryEntity> printHistoryList = DataTableToList<PrintHistoryEntity>.ConvertToModel(dt);
                return printHistoryList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 添加打印记录
        /// </summary>
        /// <param name="printHistoryEntity"></param>
        public static void AddrintHistory(PrintHistoryEntity printHistoryEntity)
        {
            try
            {
                printHistoryEntity.PHFlow = Guid.NewGuid().ToString();
                printHistoryEntity.PrintDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                printHistoryEntity.PrintDocId = DS_Common.currentUser.DoctorId;
                SqlParameter[] sqlParams = new SqlParameter[]
                {
                new SqlParameter("@phflow",SqlDbType.VarChar,50),
                new SqlParameter("@printrecordflow",SqlDbType.VarChar,50),
                new SqlParameter("@startpage",SqlDbType.Int,50),
                new SqlParameter("@endpage",SqlDbType.Int,50),
                new SqlParameter("@printpages",SqlDbType.Int,50),
                new SqlParameter("@printdocid",SqlDbType.VarChar,50),
                new SqlParameter("@printdatetime",SqlDbType.VarChar,50),
                new SqlParameter("@printtype",SqlDbType.VarChar,50)
                    };
                sqlParams[0].Value = printHistoryEntity.PHFlow;
                sqlParams[1].Value = printHistoryEntity.PrintRecordFlow;
                sqlParams[2].Value = printHistoryEntity.StartPage;
                sqlParams[3].Value = printHistoryEntity.EndPage;
                sqlParams[4].Value = printHistoryEntity.PrintPages;
                sqlParams[5].Value = printHistoryEntity.PrintDocId;
                sqlParams[6].Value = printHistoryEntity.PrintDateTime;
                sqlParams[7].Value = printHistoryEntity.PrintType;

                DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteNonQuery("emr_commonnote.usp_AddPrintHistory", sqlParams, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


    }
}
