using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.Utils;
using System.Globalization;
using DrectSoft.Common.Eop;
using DrectSoft.Wordbook;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using System.Reflection;
using DevExpress.XtraGrid.Views.Base;

using DrectSoft.Common.Library;

namespace DrectSoft.Core.DoctorAdvice
{
   /// <summary>
   /// 编辑选中的一组医嘱
   /// </summary>
   public partial class OrderSuiteEditForm : Form
   {
      #region public properties
      public object[,] SelectedContents
      {
         get { return _selectedContents; }
      }
      private object[,] _selectedContents;
      #endregion

      #region private variables
      private ICustomMessageBox m_MessageBox;
      private DataTable m_SuiteTable;
      private bool m_EditOrderData;
      private OrderFrequencyBook m_FrenqucyBook;
      private OrderUsageBook m_UsageBook;
      #endregion

      public OrderSuiteEditForm()
      {
         InitializeComponent();

         foreach (AppearanceObject ap in gridViewSuiteDetail.Appearance)
            ap.Font = this.Font;
      }

      #region public CallMethodes
      public void InitializeProperty(IDataAccess sqlHelper, ICustomMessageBox messageBox)
      {
         showListWindow1.SqlHelper = sqlHelper;
         m_MessageBox = messageBox;
         m_UsageBook = new OrderUsageBook();
         m_FrenqucyBook = new OrderFrequencyBook();

         CreateSuiteTable(sqlHelper);
      }

      /// <summary>
      /// 插入成套医嘱前调用此窗口，编辑要粘贴的医嘱内容的数量、频次、用法数据
      /// </summary>
      /// <param name="isEditData">标记是编辑医嘱数据还是做另存为成套操作</param>
      /// <param name="suiteDetailTable">初始的成套医嘱明细数据集</param>
      /// <param name="frequencyBook">包含可用频次的选择字典</param>
      /// <returns></returns>
      [CLSCompliantAttribute(false)]
      public DialogResult CallOrderSuiteEditForm(bool isEditData, DataTable suiteDetailTable, OrderFrequencyBook frequencyBook)
      {
         m_EditOrderData = isEditData;
         m_FrenqucyBook.ExtraCondition = frequencyBook.ExtraCondition;
         SetSuiteTableData(suiteDetailTable);
         //ReviseFrequencyData();
         return ShowDialog();
      }

      /// <summary>
      /// 粘贴医嘱前调用此窗口，编辑要粘贴的医嘱内容的数量、频次、用法数据
      /// </summary>
      /// <param name="isEditData">标记是编辑医嘱数据还是做另存为成套操作</param>
      /// <param name="selectedContents">被选中的医嘱内容及其分组情况</param>
      /// <param name="frequencyBook">包含可用频次的选择字典</param>
      /// <returns></returns>
      [CLSCompliantAttribute(false)]
      public DialogResult CallOrderSuiteEditForm(bool isEditData, object[,] selectedContents, OrderFrequencyBook frequencyBook)
      {
         m_EditOrderData = isEditData;
         m_FrenqucyBook.ExtraCondition = frequencyBook.ExtraCondition;
         SetSuiteTableData(selectedContents);
         //ReviseFrequencyData();
         return ShowDialog();
      }
      #endregion

      #region private methods
      private void CreateSuiteTable(IDataAccess sqlHelper)
      {
         // 以成套明细表为基础进行构造
         m_SuiteTable = sqlHelper.ExecuteDataTable(ConstSqlSentences.SelectOrderSuiteDetailSchema);

         // 添加频次、用法和医嘱类别名称字段和分组符号字段
         m_SuiteTable.Columns.AddRange(new DataColumn[] { 
             new DataColumn(ConstSchemaNames.SuiteDetailViewColUsageName, typeof(String))
            ,new DataColumn(ConstSchemaNames.SuiteDetailViewColFrequencyName, typeof(String))
            ,new DataColumn(ConstSchemaNames.SuiteDetailViewColOrderCatalogName, typeof(String))
            ,new DataColumn(ConstSchemaNames.SuiteDetailViewColGroupSymbol, typeof(String))
         });

         //m_SuiteTable = new DataTable();
         //m_SuiteTable.Locale = CultureInfo.CurrentCulture;
         //m_SuiteTable.Columns.AddRange(new DataColumn[] { 
         //    new DataColumn("cdxh", typeof(Decimal))
         //   ,new DataColumn("ypdm", typeof(String))
         //   ,new DataColumn("xmmc", typeof(String)) 
         //   ,new DataColumn("ypjl", typeof(Decimal))
         //   ,new DataColumn("xmdw", typeof(String))
         //   ,new DataColumn("yfdm", typeof(String))
         //   ,new DataColumn("yfmc", typeof(String))
         //   ,new DataColumn("pcdm", typeof(String))
         //   ,new DataColumn("pcmc", typeof(String))
         //   ,new DataColumn("memo", typeof(String))
         //   ,new DataColumn("fzbz", typeof(int))
         //   ,new DataColumn("fzfh", typeof(String))
         //});
      }

      private void SetSuiteTableData(DataTable sourceTable)
      {
         m_SuiteTable.Clear();
         if (sourceTable != null)
         {
            m_SuiteTable.Merge(sourceTable, false, MissingSchemaAction.Ignore);
         }
         ResetSuiteTableNameFieldValue();
      }

      private void SetSuiteTableData(object[,] contents)
      {
         m_SuiteTable.Clear();
         if (contents != null)
         {
            DataRow newRow;
            OrderContent content;
            for (int index = 0; index <= contents.GetUpperBound(0); index++)
            {
               newRow = m_SuiteTable.NewRow();
               content = contents[index, 0] as OrderContent;
               PersistentObjectFactory.SetDataRowValueFromObject(newRow, content);
               //if ((content.Item.Kind == ItemKind.WesternMedicine)
               //   || (content.Item.Kind == ItemKind.PatentMedicine)
               //   || (content.Item.Kind == ItemKind.HerbalMedicine))
               //   newRow["cdxh"] = content.Item.KeyValue;
               //else
               //   newRow["cdxh"] = -1;
               //newRow["ypdm"] = content.Item.Code;
               //newRow["xmmc"] = content.Item.Name;
               //newRow["ypjl"] = content.Amount;
               //newRow["xmdw"] = content.CurrentUnit.Name;
               //if ((content.ItemUsage != null) && (content.ItemUsage.KeyInitialized))
               //{
               //   newRow["yfdm"] = content.ItemUsage.Code;
               //   newRow["yfmc"] = content.ItemUsage.Name;
               //}
               //else
               //{
               //   newRow["yfdm"] = "";
               //   newRow["yfmc"] = "";
               //}
               //if ((content.ItemFrequency != null) && (content.ItemFrequency.KeyInitialized))
               //{
               //   newRow["pcdm"] = content.ItemFrequency.Code;
               //   newRow["pcmc"] = content.ItemFrequency.Name;
               //}
               //else
               //{
               //   newRow["pcdm"] = "";
               //   newRow["pcmc"] = "";
               //}
               //newRow["memo"] = content.EntrustContent;

               newRow[ConstSchemaNames.SuiteDetailColGroupFlag] = contents[index, 1];
               //newRow["fzfh"] = GetGroupFlag(Convert.ToInt32(newRow["fzbz"]));

               m_SuiteTable.Rows.Add(newRow);
            }

            ResetSuiteTableNameFieldValue();
         }
      }

      private void ResetSuiteTableNameFieldValue()
      {
         OrderContentKind contentKind;
         string[] defFrequency = showListWindow1.ValidateWordbookHasOneRecord(m_FrenqucyBook, WordbookKind.Normal);

         // 重新设置后加的名称字段的内容
         foreach (DataRow row in m_SuiteTable.Rows)
         {
            row[ConstSchemaNames.SuiteDetailViewColUsageName] = GetDisplayValueByCode(m_UsageBook, row[ConstSchemaNames.SuiteDetailColUsageCode].ToString());
            row[ConstSchemaNames.SuiteDetailViewColFrequencyName] = GetDisplayValueByCode(m_FrenqucyBook, row[ConstSchemaNames.SuiteDetailColFrequecyCode].ToString());

            if (String.IsNullOrEmpty(row[ConstSchemaNames.SuiteDetailViewColFrequencyName].ToString()) && (defFrequency != null))
            {
               row[ConstSchemaNames.SuiteDetailColFrequecyCode] = defFrequency[0];
               row[ConstSchemaNames.SuiteDetailViewColFrequencyName] = defFrequency[1];
            }

            row[ConstSchemaNames.SuiteDetailViewColGroupSymbol] = GetGroupFlag((GroupPositionKind)Convert.ToInt32(row[ConstSchemaNames.SuiteDetailColGroupFlag]));
            contentKind = (OrderContentKind)Convert.ToInt32(row[ConstSchemaNames.SuiteDetailColOrderCatalog]);
            row[ConstSchemaNames.SuiteDetailViewColOrderCatalogName] = GetContentKindName(contentKind);

            // 粘贴时不会是出院带药，所以直接禁用天数
            gridColDays.Visible = (contentKind == OrderContentKind.OutDruggery);
         }
      }

      //private void ReviseFrequencyData()
      //{
      //   if (m_SuiteTable != null)
      //   {
      //      string[] defFrequency = showListWindow1.ValidateWordbookHasOneRecord(m_FrenqucyBook, WordbookKind.Normal);

      //      foreach (DataRow row in m_SuiteTable.Rows)
      //      {
      //         showListWindow1.CallLookUpWindow(m_FrenqucyBook
      //            , WordbookKind.Normal
      //            , row["pcdm"].ToString());
      //         if (showListWindow1.ResultRows.Count > 0)
      //         {
      //            row["pcdm"] = showListWindow1.CodeValue;
      //            row["pcmc"] = showListWindow1.DisplayValue;
      //         }
      //         else
      //         {
      //            if (defFrequency != null)
      //            {
      //               row["pcdm"] = defFrequency[0];
      //               row["pcmc"] = defFrequency[1];
      //            }
      //            else
      //            {
      //               row["pcdm"] = "";
      //               row["pcmc"] = "";
      //            }
      //         }
      //      }
      //   }
      //}

      private object GetContentKindName(OrderContentKind contentKind)
      {
         switch (contentKind)
         {
            case OrderContentKind.Druggery:
               return ConstNames.ContentDruggery;
            case OrderContentKind.ChargeItem:
               return ConstNames.ContentChargeItem;
            case OrderContentKind.GeneralItem:
               return ConstNames.ContentGeneralItem;
            case OrderContentKind.ClinicItem:
               return ConstNames.ContentClinicItem;
            case OrderContentKind.OutDruggery:
               return ConstNames.ContentOutDruggery;
            case OrderContentKind.Operation:
               return ConstNames.ContentOperation;
            case OrderContentKind.TextNormal:
               return ConstNames.ContentTextNormal;
            case OrderContentKind.TextShiftDept:
               return ConstNames.ContentTextShiftDept;
            case OrderContentKind.TextAfterOperation:
               return ConstNames.ContentTextAfterOperation;
            case OrderContentKind.TextLeaveHospital:
               return ConstNames.ContentTextLeaveHospital;
            default :
               return "";
         }
      }

      private string GetGroupFlag(GroupPositionKind groupKind)
      {
         switch (groupKind)
         {
            case GroupPositionKind.GroupStart:
               return "┏";
            case GroupPositionKind.GroupMiddle:
               return "┃";
            case GroupPositionKind.GroupEnd:
               return "┗";
            default:
               return "";
         }
      }

      private string GetDisplayValueByCode(BaseWordbook wordbook, string codeValue)
      {
         showListWindow1.CallLookUpWindow(wordbook, WordbookKind.Normal, codeValue);
         return showListWindow1.DisplayValue;
      }

      private void ShowAndSelectCellValue(int focusedRowHandle, GridColumn focusedColumn, object inputValue, string codeField, string nameField, BaseWordbook wordbook)
      {
         DataRow row = gridViewSuiteDetail.GetDataRow(focusedRowHandle);
         Rectangle cellRange = CustomDrawOperation.GetGridCellRect(gridViewSuiteDetail, focusedRowHandle, focusedColumn);
         string searchText;
         if (inputValue == null)
            searchText = "";
         else
            searchText = inputValue.ToString();
         showListWindow1.CallLookUpWindow(wordbook
            , WordbookKind.Normal
            , searchText
            , ShowListFormMode.Concision
            , this.PointToScreen(cellRange.Location)
            , cellRange.Size
            , Screen.GetBounds(this)
            , ShowListCallType.Normal);
         if (showListWindow1.ResultRows.Count > 0)
         {
            row[codeField] = showListWindow1.CodeValue;
            row[nameField] = showListWindow1.DisplayValue;
         }
         else
         {
            row[codeField] = "";
            row[nameField] = "";
         }
      }

      private void CommitSelected()
      {
         DataRow[] selectedRows = m_SuiteTable.Select(ConstSchemaNames.SuiteDetailColAmount + " > 0");
         _selectedContents = new object[selectedRows.Length, 2];

         bool needCalcTotalAmount = (gridColDays.Visible) ;
         string checkMsg;
         DataRow row;
         OrderContent content;
         //Druggery druggery;
         OutDruggeryContent outDruggery;
         for (int index = 0; index < selectedRows.Length; index++)
         {
            row = selectedRows[index];
            _selectedContents[index, 1] = (GroupPositionKind)Convert.ToInt32(row["GroupFlag"]);
            content = PersistentObjectFactory.CreateAndIntializeObject(
               OrderContentFactory.GetOrderContentClassName(row[ConstSchemaNames.SuiteDetailColOrderCatalog]), row) as OrderContent;
            content.ProcessCreateOutputeInfo = new OrderContent.GenerateOutputInfo(CustomDrawOperation.CreateOutputeInfo);

            checkMsg = content.CheckProperties();
            if (!String.IsNullOrEmpty(checkMsg))
               m_MessageBox.MessageShow(checkMsg, CustomMessageBoxKind.InformationOk);

            if (needCalcTotalAmount)
            {
               outDruggery = content as OutDruggeryContent;
               if (outDruggery != null)
                  outDruggery.ReCalcTotalAmount();
            }

            //// 赋其它属性
            //if (Convert.ToInt32(row["cdxh"]) == -1)
            //{
            //   content = new ChargeItemOrderContent();
            //   content.BeginInit();
            //   content.Item = new ChargeItem(row["ypdm"].ToString().Trim());
            //   content.Item.ReInitializeProperties();
            //   content.CurrentUnit = content.Item.BaseUnit;
            //}
            //else
            //{
            //   content = new DruggeryOrderContent();
            //   content.BeginInit();
            //   druggery = new Druggery(Convert.ToDecimal(row["cdxh"]));
            //   content.Item = druggery;
            //   content.Item.ReInitializeProperties();
            //   content.CurrentUnit = druggery.WardUnit;
            //}
            //content.Amount = Convert.ToDecimal(row["ypjl"]);
            //content.ItemUsage = new OrderUsage(row["yfdm"].ToString().Trim(), row["yfmc"].ToString().Trim());
            //content.ItemFrequency = new OrderFrequency(row["pcdm"].ToString().Trim(), row["pcmc"].ToString().Trim());
            //content.ProcessCreateOutputeInfo =
            //   new OrderContent.GenerateOutputInfo(CustomDrawOperation.CreateOutputeInfo);
            //content.EndInit();
            _selectedContents[index, 0] = content;
         }
      }

      private bool CheckHasNoPieceOfGroup()
      {
         DataRow row;
         GroupPositionKind groupKind;
         int lastGroupIndex = -1;

         for (int index = 0; index < m_SuiteTable.Rows.Count; index++)
         {
            row = m_SuiteTable.Rows[index];
            //if ((Convert.ToInt32(row["cdxh"]) != -1) && (row["yfdm"].ToString() == ""))
            //   return false;
            //if (row["pcdm"].ToString() == "")
            //   return false;

            // 检查同组的是否都选中了
            if (Convert.ToInt32(row[ConstSchemaNames.SuiteDetailColAmount]) <= 0)
            {
               if (lastGroupIndex != -1)
                  return false;
               else
                  continue;
            }
            groupKind = (GroupPositionKind)Convert.ToInt32(row["GroupFlag"]);
            switch (groupKind)
            {
               case GroupPositionKind.GroupStart:
                  if (lastGroupIndex != -1)
                     return false;
                  else
                     lastGroupIndex = index;
                  break;
               case GroupPositionKind.GroupMiddle:
                  if ((lastGroupIndex == -1) || (index - lastGroupIndex != 1))
                     return false;
                  else
                     lastGroupIndex = index;
                  break;
               case GroupPositionKind.GroupEnd:
                  if ((lastGroupIndex == -1) || (index - lastGroupIndex != 1))
                     return false;
                  else
                     lastGroupIndex = -1;
                  break;
               default:
                  break;
            }
         }
         return (lastGroupIndex == -1);
      }
      #endregion

      #region event handle
      private void btnOk_Click(object sender, EventArgs e)
      {
         if (CheckHasNoPieceOfGroup())
         {
            CommitSelected();
            DialogResult = DialogResult.OK;
         }
         else
         {
            m_MessageBox.MessageShow(ConstMessages.CheckSuiteData
               , CustomMessageBoxKind.InformationOk);
         }
      }

      private void btnCancel_Click(object sender, EventArgs e)
      {
         DialogResult = DialogResult.Cancel;
      }

      private void OrderSuiteEditForm_Shown(object sender, EventArgs e)
      {
         gridCtrlSuiteDetail.DataSource = m_SuiteTable;
      }

      private void gridViewSuiteDetail_CellValueChanged(object sender, CellValueChangedEventArgs e)
      {
         if (e.Column == gridColUsage)
            ShowAndSelectCellValue(e.RowHandle, e.Column, e.Value, ConstSchemaNames.SuiteDetailColUsageCode, ConstSchemaNames.SuiteDetailViewColUsageName, m_UsageBook);
         else if (e.Column == gridColFrequency)
            ShowAndSelectCellValue(e.RowHandle, e.Column, e.Value, ConstSchemaNames.SuiteDetailColFrequecyCode, ConstSchemaNames.SuiteDetailViewColFrequencyName, m_FrenqucyBook);
      }
      #endregion
   }
}