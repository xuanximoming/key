using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using DrectSoft.Common.Eop;
using System.Data.SqlClient;

namespace DrectSoft.Core.DoctorAdvice
{
   /// <summary>
   /// 记录医嘱发送日志
   /// </summary>
   internal class LogSendRecord
   {
      private const string InsertMainSql = "insert into BL_YZFSJLK(syxh, hissyxh, fsczyh, fssj) values(@syxh, @hissyxh, @fsczyh, @fssj)";
      private const string InsertDetailSql = "insert into BL_YZFSJLMXK(fsxh, yzxh, lsyz) values(@fsxh, @yzxh, @lsyz)";

      #region properties & fields
      /// <summary>
      /// 当前病人
      /// </summary>
      public Inpatient CurrentPatient
      {
         get {return _currentPatient;}
         set {_currentPatient = value;}
      }
      private Inpatient _currentPatient;

      /// <summary>
      /// 插入主记录的参数
      /// </summary>
      private SqlParameter[] MainParas
      {
         get
         {
            if (_mainParas == null)
            {
               _mainParas = new SqlParameter[] { 
                  new SqlParameter("syxh", SqlDbType.Decimal)
                  , new SqlParameter("hissyxh", SqlDbType.Decimal)
                  , new SqlParameter("fsczyh", SqlDbType.VarChar)
                  , new SqlParameter("fssj", SqlDbType.VarChar)
               };
            }
            return _mainParas;
         }
      }
      private SqlParameter[] _mainParas;

      /// <summary>
      /// 插入明细记录的参数
      /// </summary>
      private SqlParameter[] DetailParas
      {
         get
         {
            if (_detailParas == null)
            {
               _detailParas = new SqlParameter[] { 
                  new SqlParameter("fsxh", SqlDbType.Decimal)
                  , new SqlParameter("yzxh", SqlDbType.Decimal)
                  , new SqlParameter("lsyz", SqlDbType.SmallInt)
               };
            }
            return _detailParas;
         }
      }
      private SqlParameter[] _detailParas;

      private IDataAccess m_SqlHelper;
      #endregion

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sqlHelper"></param>
      public LogSendRecord(IDataAccess sqlHelper)
      {
         m_SqlHelper = sqlHelper;
      }

      /// <summary>
      /// 保存发送到HIS的医嘱信息记录到表中
      /// </summary>
      /// <param name="changedTable"></param>
      /// <param name="isTempOrder"></param>
      public void SaveRecord(DataTable changedTable, bool isTempOrder, string operatorCode, DateTime sendTime)
      {
         if ((CurrentPatient == null) || (changedTable.Rows.Count == 0))
            return;

         int serialNo;

         // 先插主记录
         MainParas["syxh"].Value = CurrentPatient.NoOfFirstPage;
         MainParas["hissyxh"].Value = CurrentPatient.NoOfHisFirstPage;
         MainParas["fsczyh"].Value = operatorCode;
         MainParas["fssj"].Value = sendTime;

         m_SqlHelper.ExecuteNoneQuery(InsertMainSql, MainParas, out serialNo);

         if (serialNo <= 0)
            throw new Exception("插入主记录失败");

         // 再插入明细记录
         string OrderSerialNoField = CoreBusinessLogic.GetSerialNoField(isTempOrder);

         DetailParas["fsxh"].Value = serialNo;
         if (isTempOrder)
            DetailParas["lsyz"].Value = 1;
         else
            DetailParas["lsyz"].Value = 0;

         foreach(DataRow row in changedTable.Rows)
         {
            DetailParas["yzxh"].Value = row[OrderSerialNoField];
            m_SqlHelper.ExecuteNoneQuery(InsertDetailSql, DetailParas);
         }
      }
   }
}
