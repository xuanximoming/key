using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using DrectSoft.Common.Eop;
using System.Globalization;
using System.Data.SqlClient;
using System.Windows.Forms;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace DrectSoft.Core.DoctorAdvice
{
   /// <summary>
   /// 处理成套医嘱表和医嘱表之间的转换以及数据保存
   /// </summary>
   public class SuiteOrderHandle
   {
      #region public properties
      /// <summary>
      /// 成套主记录表
      /// </summary>
      public DataTable SuiteMasterTable
      {
         get
         {
            if (_suiteDataSet == null)
               return null;
            else
               return _suiteDataSet.Tables[0];
         }
      }

      /// <summary>
      /// 成套明细数据表
      /// </summary>
      public DataTable SuiteDetailTable
      {
         get
         {
            if (_suiteDataSet == null)
               return null;
            else
               return _suiteDataSet.Tables[1];
         }
      }
      private DataSet _suiteDataSet;

      /// <summary>
      /// 当前处理的成套医嘱的序号
      /// </summary>
      public decimal CurrentSuiteNo
      {
         get { return _currentSuiteNo; }
         set
         {
            _currentSuiteNo = value;
            DoAfterSwitchSuite();
         }
      }
      private decimal _currentSuiteNo;

      /// <summary>
      /// 转成临时医嘱的成套医嘱数据（同时也是修改后的数据）
      /// </summary>
      public DataTable TempOrderTable
      {
         get { return _tempOrderTable; }
      }
      private DataTable _tempOrderTable;

      /// <summary>
      /// 转成长期医嘱的成套医嘱数据（同时也是修改后的数据）
      /// </summary>
      public DataTable LongOrderTable
      {
         get { return _longOrderTable; }
      }
      private DataTable _longOrderTable;
      #endregion

      #region private variables & properties
      private IDataAccess m_SqlExecutor;
      private ICustomMessageBox m_MessageBox;
      private GenerateShortCode m_GenShortCode;
      private bool m_QueryOnly; // 是否仅是查询功能(影响到初始化数据的处理)
      private DataTable m_TransTable; // 成套明细和医嘱表转换的中间表

      private SqlParameter[] InsertMasterParas
      {
         get
         {
            if (_insertMasterParas == null)
            {
               _insertMasterParas = new SqlParameter[] {
                   new SqlParameter(ConstSchemaNames.SuiteColName, SqlDbType.VarChar)
                  ,new SqlParameter(ConstSchemaNames.SuiteColPy, SqlDbType.VarChar)
                  ,new SqlParameter(ConstSchemaNames.SuiteColWb, SqlDbType.VarChar)
                  ,new SqlParameter(ConstSchemaNames.SuiteColDeptCode, SqlDbType.VarChar)
                  ,new SqlParameter(ConstSchemaNames.SuiteColWardCode, SqlDbType.VarChar)
                  ,new SqlParameter(ConstSchemaNames.SuiteColDoctorId, SqlDbType.VarChar)
                  ,new SqlParameter(ConstSchemaNames.SuiteColApplyRange, SqlDbType.Int)
                  ,new SqlParameter(ConstSchemaNames.SuiteColMemo, SqlDbType.VarChar)
               };
            }

            return _insertMasterParas;
         }
      }
      private SqlParameter[] _insertMasterParas;
      #endregion

      #region ctor
      /// <summary>
      /// 
      /// </summary>
      /// <param name="app"></param>
      /// <param name="queryOnly">是否仅是查询功能(影响到初始化数据的处理)</param>
      public SuiteOrderHandle(IEmrHost app, bool queryOnly)
      {
         m_SqlExecutor = app.SqlHelper;
         m_MessageBox = app.CustomMessageBox;
         m_GenShortCode = new GenerateShortCode(app.SqlHelper);
         m_QueryOnly = queryOnly;
          
         InsertMasterParas[3].Value = app.User.CurrentDeptId;
         InsertMasterParas[4].Value = app.User.CurrentWardId;
         InsertMasterParas[5].Value = app.User.DoctorId;

         //GenerateSuiteData();
         //InitializeTableSchema();
      }
      #endregion

      #region public methods
      /// <summary>
      /// 将传入的成套医嘱对象同步到DataRow中，并保存到数据
      /// </summary>
      /// <param name="serialNo"></param>
      /// <param name="suiteObject"></param>
      public void SynchAndSaveMasterData(decimal serialNo, SuiteOrder suiteObject)
      {
         if (suiteObject == null)
            return;
         DataRow[] matchRows = SuiteMasterTable.Select(ConstSchemaNames.SuiteDetailColSuiteSerialNo + " = " + serialNo);
         if (matchRows.Length == 1)
         {
            //if (String.IsNullOrEmpty(suiteObject.Py))
            {
               // 重新生成拼音、五笔缩写
               string[] shortCodes = m_GenShortCode.GenerateStringShortCode(suiteObject.Name);
               suiteObject.Py = shortCodes[0];
               suiteObject.Wb = shortCodes[1];
            }
            // 同步属性和DataRow的值
            PersistentObjectFactory.SetDataRowValueFromObject(matchRows[0], suiteObject);
            // 将修改同步到数据库中
            m_SqlExecutor.UpdateTable(SuiteMasterTable, ConstSchemaNames.SuiteTableName, false);
            //SuiteMasterTable.AcceptChanges();
         }
         else
            m_MessageBox.MessageShow("成套医嘱数据有错误，请退出程序重新进入！", CustomMessageBoxKind.ErrorOk);
      }

      /// <summary>
      /// 增加指定类型的新成套医嘱主记录
      /// </summary>
      /// <returns></returns>
      public decimal AddNewMasterRecord(DataApplyRange suiteType)
      {
         // 在当前分类下插入一条新记录（到数据库中）
         string insertCmd = String.Format(CultureInfo.CurrentCulture
            , ConstSqlSentences.FormatInsertSuite
            , ConstSchemaNames.SuiteTableName);
         string newName = String.Format(CultureInfo.CurrentCulture
            , "({0}) {1}", "新成套", SuiteMasterTable.Rows.Count + 1);

         InsertMasterParas[0].Value = newName;
         InsertMasterParas[1].Value = "";
         InsertMasterParas[2].Value = "";
         InsertMasterParas[6].Value = Convert.ToInt32(suiteType);
         InsertMasterParas[7].Value = "";

         int newSuiteSerialNo;
         m_SqlExecutor.ExecuteNoneQuery(insertCmd, InsertMasterParas, out newSuiteSerialNo);
         // 从数据库中读出新插入的记录，合并到当前的Master表中
         DataTable newRecords = m_SqlExecutor.ExecuteDataTable(String.Format(CultureInfo.CurrentCulture
            , ConstSqlSentences.FormatSelectSuite
            , ConstSchemaNames.SuiteTableName, newSuiteSerialNo));
         SuiteMasterTable.Merge(newRecords, true, MissingSchemaAction.Ignore);

         return (decimal)newSuiteSerialNo;
      }

      /// <summary>
      /// 删除指定的成套医嘱
      /// </summary>
      /// <param name="serialNo"></param>
      public void DeleteMasterRecord(decimal serialNo)
      {
         if ((serialNo > 0)
            && (m_MessageBox.MessageShow("确定要删除当前记录吗？", CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes))
         {
            // 删除对应明细, 删除主记录
            string delCmd = String.Format(CultureInfo.CurrentCulture
               , ConstSqlSentences.FormatDeleteSuiteData
               , ConstSchemaNames.SuiteDetailTableName
               , ConstSchemaNames.SuiteTableName
               , serialNo);
            try
            {
               m_SqlExecutor.ExecuteNoneQuery(delCmd);
               // 从DataTable中移除
               DataRow[] matchRows = SuiteMasterTable.Select(ConstSchemaNames.SuiteDetailColSuiteSerialNo + " = " + serialNo);
               foreach (DataRow row in matchRows)
                  row.Delete();
               matchRows = SuiteDetailTable.Select(ConstSchemaNames.SuiteDetailColSuiteSerialNo + " = " + serialNo);
               foreach (DataRow row in matchRows)
                  row.Delete();
            }
            catch
            {
               m_MessageBox.MessageShow("删除记录出错，请重试!", CustomMessageBoxKind.ErrorYes);
            }
         }
      }

      /// <summary>
      /// 保存当前成套医嘱的明细数据
      /// </summary>
      public void SaveSuiteDetailData()
      {
         //先清除老的数据
         DataRow[] delRows = SuiteDetailTable.Select(ConstSchemaNames.SuiteDetailColSuiteSerialNo + " = " + CurrentSuiteNo);
         foreach (DataRow row in delRows)
            SuiteDetailTable.Rows.Remove(row);

         // 将医嘱表数据合并到成套明细表(因为需要将合并后的行状态都为新行，所以采用AddRow的方式)
         foreach (DataRow row in TempOrderTable.Rows)
            AddNewRowToDetailTable(row, true);
         foreach (DataRow row in LongOrderTable.Rows)
            AddNewRowToDetailTable(row, false);

         // 删除数据库中此序号对应的明细记录
         m_SqlExecutor.ExecuteNoneQuery(String.Format(ConstSqlSentences.FormatDeleteSuiteDetail, ConstSchemaNames.SuiteDetailTableName, CurrentSuiteNo));
         // 保存
         m_SqlExecutor.UpdateTable(SuiteDetailTable, ConstSchemaNames.SuiteDetailTableName, false);
         //SuiteDetailTable.AcceptChanges();
      }
      #endregion

      #region custom event handler
      /// <summary>
      /// 切换当前处理的成套医嘱事件
      /// </summary>
      public event EventHandler AfterSwitchSuite
      {
         add { onAfterSwitchSuite = (EventHandler)Delegate.Combine(onAfterSwitchSuite, value); }
         remove { onAfterSwitchSuite = (EventHandler)Delegate.Remove(onAfterSwitchSuite, value); }
      }
      private EventHandler onAfterSwitchSuite;

      private void FireAfterSwitchSuite()
      {
         if (onAfterSwitchSuite != null)
            onAfterSwitchSuite(this, new EventArgs());
      }
      #endregion

      #region private methods
      private void GenerateSuiteData()
      {
         // 读取数据, 设置ReleationShip
         //string selMasterCmd = String.Format("select * from {0} where syfw = 2900 or (syfw = 2901 and e.ksdm = @ksdm) or (syfw = 2903 and e.ysdm = @ysdm) order by name"
         //   , ConstSchemaNames.SuiteTableName);
         //string selDetailCmd = String.Format("select a.* from {0} a, {1} b where a.ctyzxh = b.ctyzxh and (b.syfw = 2900 or (b.syfw = 2901 and b.ksdm = @ksdm) or (b.syfw = 2903 and b.ysdm = @ysdm)) order by ctmxxh"
         //   , ConstSchemaNames.SuiteDetailTableName, ConstSchemaNames.SuiteTableName);

         SqlParameter[] paras = new SqlParameter[] { 
             InsertMasterParas[3]
            ,InsertMasterParas[4]
            ,InsertMasterParas[5]
            , new SqlParameter("yzlr", SqlDbType.Int)
         };
         if (m_QueryOnly)
            paras[3].Value = 1;
         else
            paras[3].Value = 0;

         _suiteDataSet = m_SqlExecutor.ExecuteDataSet(ConstSchemaNames.ProcQueryOrderSuite, paras, CommandType.StoredProcedure);
         SuiteMasterTable.TableName = ConstSchemaNames.SuiteTableName;
         if (!m_QueryOnly)
         {
            m_SqlExecutor.ResetTableSchema(SuiteMasterTable, ConstSchemaNames.SuiteTableName);
            m_SqlExecutor.ResetTableSchema(SuiteDetailTable, ConstSchemaNames.SuiteDetailTableName);
         }
         //DataColumn[] keyCols = new DataColumn[] { SuiteMasterTable.Columns["ctyzxh"], SuiteMasterTable.Columns["yzbz"] };
         //DataColumn[] foreignKeyCols = new DataColumn[] { SuiteDetailTable.Columns["ctyzxh"], SuiteDetailTable.Columns["yzbz"] };
         //_suiteDataSet.Relations.Add("SuiteDetail", keyCols, foreignKeyCols);
      }

      private void DoAfterSwitchSuite()
      {
         TransformSuiteDetailData(true, TempOrderTable);
         TransformSuiteDetailData(false, LongOrderTable);
         FireAfterSwitchSuite();
      }

      private void TransformSuiteDetailData(bool isTemp, DataTable orderTable)
      {
         string orderSerialNoField;
         if (isTemp)
         {
            orderSerialNoField = ConstSchemaNames.OrderTempColSerialNo;
            SuiteDetailTable.DefaultView.RowFilter = String.Format(CultureInfo.CurrentCulture
               , ConstSqlSentences.FormatSuiteDetailFilter
               , CurrentSuiteNo
               , OrderManagerKind.ForTemp);
         }
         else
         {
            orderSerialNoField = ConstSchemaNames.OrderLongColSerialNo;
            SuiteDetailTable.DefaultView.RowFilter = String.Format(CultureInfo.CurrentCulture
               , ConstSqlSentences.FormatSuiteDetailFilter 
               , CurrentSuiteNo
               ,OrderManagerKind.ForLong);
         }

         // 成套医嘱的明细数据合并到指定的医嘱表中
         m_TransTable.Clear();
         m_TransTable.Merge(SuiteDetailTable.DefaultView.ToTable(), false, MissingSchemaAction.Ignore);
         orderTable.Clear();
         orderTable.Merge(m_TransTable, false, MissingSchemaAction.Ignore);

         // 处理分组序号和医嘱状态
         GroupPositionKind gpKind;
         object newGroupSerialNo = -1;
         for (int index = 0; index < orderTable.Rows.Count; index++)
         {
             gpKind = (GroupPositionKind)Convert.ToInt32(orderTable.Rows[index]["GroupFlag"]);

            if ((gpKind == GroupPositionKind.SingleOrder) || (gpKind == GroupPositionKind.GroupStart))
               newGroupSerialNo = orderTable.Rows[index][orderSerialNoField];            
            orderTable.Rows[index][ConstSchemaNames.OrderColGroupSerialNo] = newGroupSerialNo;

            //orderTable.Rows[index]["yzzt"] = (int)OrderState.New;
         }
      }

      private void InitializeTableSchema()
      {
         _tempOrderTable = CreateAndSetOrderTable( ConstSchemaNames.TempOrderTableName);
         _longOrderTable = CreateAndSetOrderTable(ConstSchemaNames.LongOrderTableName);
         
         m_TransTable = SuiteDetailTable.Clone();
         // 补充在医嘱表中不允许为空的列，并设置默认值
         m_TransTable.Columns.AddRange(new DataColumn[] {
             new DataColumn("syxh", typeof(decimal))
            ,new DataColumn("fzxh", typeof(decimal))
            ,new DataColumn("bqdm", typeof(string))
            ,new DataColumn("ksdm", typeof(string))
            ,new DataColumn("lrysdm", typeof(string))
            ,new DataColumn("lrrq", typeof(string))
            ,new DataColumn("yzzt", typeof(int))
            ,new DataColumn("tsbj", typeof(int))
            });
         m_TransTable.Columns["syxh"].DefaultValue = 1;
         m_TransTable.Columns["fzxh"].DefaultValue = 1;
         m_TransTable.Columns["bqdm"].DefaultValue = "";
         m_TransTable.Columns["ksdm"].DefaultValue = "";
         m_TransTable.Columns["lrysdm"].DefaultValue = "00";
         m_TransTable.Columns["lrrq"].DefaultValue = "";
         m_TransTable.Columns["yzzt"].DefaultValue = (int)OrderState.New;
         m_TransTable.Columns["tsbj"].DefaultValue = 0;
      }

      private DataTable CreateAndSetOrderTable(string tableName)
      {
         DataTable result = m_SqlExecutor.ExecuteDataTable("select * from " + tableName + " where 1 = 2");
         m_SqlExecutor.ResetTableSchema(result, tableName);
         return result;
      }

      private void AddNewRowToDetailTable(DataRow sourceRow, bool isTemp)
      {
         if ((sourceRow.RowState == DataRowState.Deleted) || (sourceRow.RowState == DataRowState.Detached))
            return;

         DataRow newRow = SuiteDetailTable.NewRow();

         foreach (DataColumn col in sourceRow.Table.Columns)
            if (SuiteDetailTable.Columns.Contains(col.ColumnName))
               newRow[col.ColumnName] = sourceRow[col];

         newRow[ConstSchemaNames.SuiteDetailColSuiteSerialNo] = CurrentSuiteNo;
         if (isTemp)
            newRow[ConstSchemaNames.SuiteDetailColOrderFlag] = OrderManagerKind.ForTemp;
         else
            newRow[ConstSchemaNames.SuiteDetailColOrderFlag] = OrderManagerKind.ForLong;

         SuiteDetailTable.Rows.Add(newRow);
      }
      #endregion
   }
}
