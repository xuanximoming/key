using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.Common.Ctrs.FORM;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Repository;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DevExpress.Utils;

namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    public partial class RecordHistoryForm :DevBaseForm
    {
        public RecordHistoryForm(DataTable dt, Dictionary<string, DataElementEntity> dataElementList)
        {
            try
            {
                InitializeComponent();
                SetDataToGrid(dt, dataElementList);
                gridControl1.DataSource = dt;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void SetDataToGrid(DataTable dt, Dictionary<string, DataElementEntity> dataElementList)
        {

            gridView1.Columns.Clear();
            for (int i = 0; i < dt.Columns.Count; i++)
            {

                GridColumn gridColumn = new DevExpress.XtraGrid.Columns.GridColumn();

                gridColumn.FieldName = dt.Columns[i].ColumnName;
                gridColumn.OptionsFilter.AllowAutoFilter = false;
                gridColumn.OptionsFilter.AllowFilter = false;
                gridColumn.OptionsColumn.AllowSort = DefaultBoolean.False;
                gridColumn.Caption = dt.Columns[i].Caption;
                gridColumn.OptionsColumn.AllowEdit = false;
                gridColumn.AppearanceHeader.Options.UseTextOptions = true;
                gridColumn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                if (dt.Columns[i].ColumnName == "groupFlow")
                {
                    gridColumn.Visible = false;
                }
                else
                {
                    gridColumn.Visible = true;
                    gridColumn.VisibleIndex = i;
                }
                if (dt.Columns[i].ColumnName == "jlsj" || dt.Columns[i].ColumnName == "xgsj")  //设置记录时间用的内容控件
                {
                    gridColumn.Width = 141;
                }
                if (dt.Columns[i].ColumnName == "jlr")
                {
                    gridColumn.Width = 100;
                }

                if (dataElementList.ContainsKey(dt.Columns[i].ColumnName))
                {
                    DataElementEntity dataElementEntity = dataElementList[dt.Columns[i].ColumnName];
                    if (dataElementEntity == null) continue;
                    SetGridColumnWidth(gridColumn, dataElementEntity);
                }
                RepositoryItemMemoEdit repositoryItem = new RepositoryItemMemoEdit();
                repositoryItem.AutoHeight = true;
                gridColumn.ColumnEdit = repositoryItem;

                gridView1.Columns.Add(gridColumn);
                gridView1.OptionsMenu.EnableColumnMenu = false;
            }
        }


        /// 设置列的宽度
        /// </summary>
        /// <param name="gridColumn"></param>
        /// <param name="dataElementEntity"></param>
        private void SetGridColumnWidth(GridColumn gridColumn, DataElementEntity dataElementEntity)
        {
            try
            {
                gridColumn.Width = 6 + 13 * gridColumn.Caption.Length;
                if (gridColumn.Width < 40)
                {
                    gridColumn.Width = 40;
                }
                string elementType = dataElementEntity.ElementType;
                if (elementType == "S4")
                {
                    if (gridColumn.Width < 200)
                    {
                        gridColumn.Width = 200;
                    }
                }
                else if (elementType == "D")
                {
                    gridColumn.Width = 80;
                }
                else if (elementType == "DT")
                {
                    gridColumn.Width = 141;
                }
                else if (elementType == "T")
                {
                    gridColumn.Width = 78;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }



    }
}
