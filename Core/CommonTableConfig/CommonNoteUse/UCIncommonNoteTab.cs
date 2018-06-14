using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    public partial class UCIncommonNoteTab : DevExpress.XtraEditors.XtraUserControl
    {
        InCommonNoteEnmtity m_inCommonNote;
        InCommonNoteTabEntity m_InCommonNoteTab;
        CommonNote_TabEntity m_commonNote_TabEntity;
        IEmrHost m_app;
        InCommonNoteBiz inCommonNoteBiz;
        DataElemntBiz m_DataElemntBiz;
        Dictionary<string, DataElementEntity> dataElementList; //当前所有列的列名和数据元素对象
        bool m_canEdit;//设置是否可编辑
        bool m_CanSign = false;  //是否可进行图片签名
        int AddRowCount = 0;
        //进行分组后的单据项目
        Dictionary<string, List<InCommonNoteItemEntity>> dicitemList;

        //进行分组后的值发生改变了的单据行项目
        Dictionary<string, List<InCommonNoteItemEntity>> dicitemListChanged = new Dictionary<string, List<InCommonNoteItemEntity>>();

        //进行分组后的值发生删除的单据行项目
        Dictionary<string, List<InCommonNoteItemEntity>> dicitemListDel = new Dictionary<string, List<InCommonNoteItemEntity>>();

        UCRecordDateTime uCRecordDateTime;
        Dictionary<string, ucLabText> ucLabTextList;
        UCRecordDoctor uCRecordDoctor;
        string value = "";//当前单元格内容 by xlb 2013-03-12
        int allcount = 0;  //单据表格存在的行数 包括数据库中的和当前界面的
        bool isNurseHeard = false; //是否是护士长
        CommonNoteCountEntity m_commonNoteCountEntity;
        public UCIncommonNoteTab(InCommonNoteTabEntity inCommonNoteTab, CommonNote_TabEntity commonNote_TabEntity, InCommonNoteEnmtity inCommonNote, IEmrHost app, bool canEdit)
        {
            try
            {
                m_inCommonNote = inCommonNote;
                m_InCommonNoteTab = inCommonNoteTab;
                m_commonNote_TabEntity = commonNote_TabEntity;
                m_app = app;
                m_canEdit = canEdit;
                m_DataElemntBiz = new DataElemntBiz(m_app);
                inCommonNoteBiz = new InCommonNoteBiz(m_app);
                inCommonNoteBiz.SaveIncommonType(m_InCommonNoteTab.InCommonNote_Tab_Flow, m_commonNote_TabEntity);
                InitializeComponent();
                SetBtnEnable();

                dtStart.DateTime = DateTime.Now.AddDays(-7);
                dtEnd.DateTime = DateTime.Now;
                inCommonNoteBiz = new InCommonNoteBiz(m_app);
                SetbtnAddColNameVisible();
                SetTongjiBtn();
                RegisterEvent();
                IsNurseHead();
                //BindData();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// xll 2013-06-25
        /// </summary>
        private void SetTongjiBtn()
        {
            try
            {
                m_commonNoteCountEntity = CommonNoteBiz.GetCommonNoteCount(m_commonNote_TabEntity.CommonNoteFlow);
                if (m_commonNoteCountEntity == null || string.IsNullOrEmpty(m_commonNoteCountEntity.ItemCount) || m_commonNoteCountEntity.Valide == "0")
                {
                    btnTongJi.Visible = false;
                }
                else
                {
                    btnTongJi.Visible = true;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void SetBtnEnable()
        {

            btnHis.Enabled = btnDelHis.Enabled = btnTongJi.Enabled = btnTopAdd.Enabled = btnAddNew.Enabled = btnRemove.Enabled = btnTopSave.Enabled = btnAddColName.Enabled = btnEdit.Enabled = m_canEdit;
            gridViewTab.OptionsBehavior.ReadOnly = !m_canEdit;
        }

        //判断用户是不是护士长
        private void IsNurseHead()
        {
            string headNurseStr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("HeadNurse"); //获取护士长IDs
            if (string.IsNullOrEmpty(headNurseStr))
            {
                isNurseHeard = false;
            }
            string[] nurses = headNurseStr.Split(',');
            if (nurses.Contains(m_app.User.DoctorId))
            {
                isNurseHeard = true;
            }
        }

        /// <summary>
        /// 设置指定列的显示
        /// </summary>
        private void SetbtnAddColNameVisible()
        {
            if (m_commonNote_TabEntity == null || m_commonNote_TabEntity.CommonNote_ItemList == null || m_commonNote_TabEntity.CommonNote_ItemList.Count <= 0) return;
            foreach (var item in m_commonNote_TabEntity.CommonNote_ItemList)
            {
                if (item.OtherName == null || item.OtherName.Trim() == "")
                {
                    btnAddColName.Visible = true;
                    break;
                }
            }
        }

        private void BindData()
        {
            WaitDialogForm waitDialog = new WaitDialogForm("正在绘制表格……", "请稍等。");
            try
            {
                DataTable dt = GetDateTable();
                SetValueToDataTable(dt);
                InitGridControlForDateTable(dt);
                gridControl1.DataSource = dt;
                //if (dt == null || dt.Rows == null || dt.Rows.Count <= 0)
                //{
                //    btnAddColName.Visible = false;
                //}
                //else
                //{
                //    btnAddColName.Visible = false;
                //    foreach (var item in m_commonNote_TabEntity.CommonNote_ItemList)
                //    {
                //        if (string.IsNullOrEmpty(item.OtherName))
                //        {
                //            btnAddColName.Visible = true;
                //            break;
                //        }
                //    }

                //}
                waitDialog.Hide();
                waitDialog.Close();
            }
            catch (Exception ex)
            {
                waitDialog.Hide();
                waitDialog.Close();
                throw ex;
            }

            //gridViewTab.MoveBy(dt.Rows.Count - 1);
            //PrintInCommonTabView printInCommonTabView = new CommonNoteUse.PrintInCommonTabView();
            //List<string> strNames = InCommonNoteBiz.ConvertInCommonTabToPrint(m_InCommonNoteTab, printInCommonTabView, out dicitemList, m_commonNote_TabEntity, m_app);
            //gridViewTab.Columns.Clear();

            //GridColumn gridColumnData = new DevExpress.XtraGrid.Columns.GridColumn();
            //gridColumnData.OptionsFilter.AllowAutoFilter = false;
            //gridColumnData.OptionsFilter.AllowFilter = false;
            //gridColumnData.Caption = "记录时间";
            //gridColumnData.Width = 110;
            //gridColumnData.VisibleIndex = gridViewTab.Columns.Count;
            //gridColumnData.FieldName = "DateTimeShow";
            //gridViewTab.Columns.Add(gridColumnData);
            //for (int i = 1; i <= strNames.Count; i++)
            //{
            //    GridColumn gridColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            //    gridColumn.OptionsFilter.AllowAutoFilter = false;
            //    gridColumn.OptionsFilter.AllowFilter = false;
            //    gridColumn.Caption = strNames[i - 1];
            //    gridColumn.VisibleIndex = gridViewTab.Columns.Count;
            //    gridColumn.FieldName = "Value" + i;
            //    gridColumn.Width = 6 + 13 * strNames[i - 1].Length;
            //    gridViewTab.Columns.Add(gridColumn);
            //}
            //GridColumn gridColumnDoc = new DevExpress.XtraGrid.Columns.GridColumn();
            //gridColumnDoc.OptionsFilter.AllowAutoFilter = false;
            //gridColumnDoc.OptionsFilter.AllowFilter = false;
            //gridColumnDoc.Caption = "记录人";
            //gridColumnDoc.Width = 55;
            //gridColumnDoc.VisibleIndex = gridViewTab.Columns.Count;
            //gridColumnDoc.FieldName = "RecordDoctorName";
            //gridViewTab.Columns.Add(gridColumnDoc);
            //gridControl1.DataSource = printInCommonTabView.PrintInCommonItemViewList;
        }


        /// <summary>
        /// 初始化列表
        /// edit by xlb 2013-01-30
        /// </summary>
        /// <param name="dt"></param>
        private void InitGridControlForDateTable(DataTable dt)
        {
            try
            {
                gridViewTab.Columns.Clear();
                for (int i = 0; i < dt.Columns.Count; i++)
                {

                    GridColumn gridColumn = new DevExpress.XtraGrid.Columns.GridColumn();

                    gridColumn.FieldName = dt.Columns[i].ColumnName;
                    gridColumn.OptionsFilter.AllowAutoFilter = false;
                    gridColumn.OptionsFilter.AllowFilter = false;
                    gridColumn.Caption = dt.Columns[i].Caption;
                    gridColumn.OptionsColumn.AllowEdit = true;

                    //gridColumn
                    //使用该属性(xlb 2013-01-30)
                    gridColumn.AppearanceHeader.Options.UseTextOptions = true;
                    //列头居中(xlb 2013-01-30)
                    gridColumn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    if (dt.Columns[i].ColumnName == "groupFlow" || dt.Columns[i].ColumnName == "xgnum")
                    {
                        gridColumn.Visible = false;
                    }
                    else
                    {
                        gridColumn.Visible = true;
                        gridColumn.VisibleIndex = i;
                    }
                    if (dt.Columns[i].ColumnName == "jlsj")  //设置记录时间用的内容控件
                    {
                        RepositoryItemDateEdit repositoryItemDateEdit = new RepositoryItemDateEdit();
                        repositoryItemDateEdit.AllowNullInput = DefaultBoolean.False;
                        repositoryItemDateEdit.VistaDisplayMode = DefaultBoolean.True; ;
                        repositoryItemDateEdit.VistaEditTime = DefaultBoolean.True;
                        repositoryItemDateEdit.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
                        repositoryItemDateEdit.EditFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
                        repositoryItemDateEdit.EditMask = "yyyy-MM-dd HH:mm:ss";
                        gridColumn.ColumnEdit = repositoryItemDateEdit;
                        gridColumn.Width = 141;


                    }
                    if (dt.Columns[i].ColumnName == "jlr")
                    {
                        gridColumn.Width = 100;
                        //int width = 0;
                        //foreach (GridColumn item in gridViewTab.Columns)
                        //{
                        //    if (item.Visible == false) continue;
                        //    width += item.Width;
                        //}
                        //int jlrwidth = gridControl1.Width - width - gridViewTab.IndicatorWidth;
                        //if (jlrwidth > 75)
                        //{
                        //    gridColumn.Width = jlrwidth;
                        //}
                        m_CanSign = inCommonNoteBiz.CanUsingSign(m_inCommonNote.CommonNoteFlow);
                        gridColumn.OptionsColumn.AllowEdit = !m_CanSign;
                    }
                    if (dataElementList.ContainsKey(dt.Columns[i].ColumnName))
                    {
                        string isValide = GetIsValide(dt.Columns[i].ColumnName);
                        if (isValide == "是")
                        {
                            DataElementEntity dataElementEntity = dataElementList[dt.Columns[i].ColumnName];
                            if (dataElementEntity == null) continue;
                            RepositoryItem repositoryItem = SetGridRepositoryItem(dataElementEntity);
                            if (repositoryItem == null) continue;
                            gridColumn.ColumnEdit = repositoryItem;
                            SetGridColumnWidth(gridColumn, dataElementEntity, dt);
                        }
                    }

                    gridViewTab.Columns.Add(gridColumn);
                    gridViewTab.OptionsMenu.EnableColumnMenu = false;
                    //gridViewTab.OptionsMenu.EnableFooterMenu = false;
                    //gridViewTab.OptionsMenu.EnableGroupPanelMenu = false;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 设置列的宽度
        /// </summary>
        /// <param name="gridColumn"></param>
        /// <param name="dataElementEntity"></param>
        private void SetGridColumnWidth(GridColumn gridColumn, DataElementEntity dataElementEntity, DataTable dt)
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


        /// <summary>
        /// 获取当前元素是否要进行校验
        /// </summary>
        /// <param name="commonNoteItemFlow"></param>
        /// <returns></returns>
        private string GetIsValide(string commonNoteItemFlow)
        {
            try
            {
                if (m_InCommonNoteTab == null || m_InCommonNoteTab.InCommonNoteItemList == null || m_InCommonNoteTab.InCommonNoteItemList.Count <= 0)
                {
                    foreach (var item in m_commonNote_TabEntity.CommonNote_ItemList)
                    {
                        if (item.CommonNote_Item_Flow == commonNoteItemFlow)
                        {
                            return item.IsValidate;
                        }
                    }

                }
                else
                {
                    foreach (var item in m_InCommonNoteTab.InCommonNoteItemList)
                    {
                        if (item.CommonNote_Item_Flow == commonNoteItemFlow)
                        {
                            return item.IsValidate;
                        }
                    }
                }
                return "否";
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private RepositoryItem SetGridRepositoryItem(DataElementEntity dateElement)
        {
            try
            {
                if (dateElement == null) { return null; }
                RepositoryItem repositoryItem = new RepositoryItemTextEdit();
                switch (dateElement.ElementType.ToUpper())
                {
                    case "S1":
                        repositoryItem = new RepositoryItemTextEdit();
                        break;
                    case "S2":
                        repositoryItem = SetRepositoryItemComboBox(dateElement);
                        break;
                    case "S3":
                        repositoryItem = SetRepositoryItemComboBox(dateElement);
                        break;
                    case "S4":
                        repositoryItem = SetRepositoryItemMemoEdit(dateElement);
                        break;
                    case "S9":
                        repositoryItem = SetRepositoryItemCheckedComboBoxEdit(dateElement);
                        break;
                    case "DT":
                        repositoryItem = SetRepositoryItemDateEdit(dateElement);
                        break;
                    case "D":
                        repositoryItem = SetRepositoryItemDateEdit(dateElement);
                        break;
                    case "T":
                        repositoryItem = SetRepositoryItemTimeEdit(dateElement);
                        break;
                    case "N":
                        repositoryItem = SetRepositoryItemSpinEdit(dateElement);
                        break;
                    case "L":
                        repositoryItem = SetRepositoryItemCheckEdit(dateElement);
                        break;
                }
                return repositoryItem;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #region 通过dateElement生成控件 xll
        /// <summary>
        /// 单选
        /// </summary>
        /// <param name="dateElement"></param>
        /// <returns></returns>
        private RepositoryItemComboBox SetRepositoryItemComboBox(DataElementEntity dateElement)
        {
            try
            {
                RepositoryItemComboBox repositoryItemComboBox = new RepositoryItemComboBox();
                foreach (var item in dateElement.BaseOptionList)
                {
                    ComboBoxItem comboBoxItem = new ComboBoxItem(item.Name);
                    repositoryItemComboBox.Items.Add(comboBoxItem);
                }
                return repositoryItemComboBox;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 大文本
        /// </summary>
        /// <param name="dateElement"></param>
        /// <returns></returns>
        private RepositoryItemMemoEdit SetRepositoryItemMemoEdit(DataElementEntity dateElement)
        {
            try
            {
                RepositoryItemMemoEdit repositoryItemMemoEdit = new RepositoryItemMemoEdit();
                repositoryItemMemoEdit.AutoHeight = true;
                repositoryItemMemoEdit.DragDrop += new DragEventHandler(repositoryItemMemoEdit_DragDrop);
                repositoryItemMemoEdit.DragEnter += new DragEventHandler(repositoryItemMemoEdit_DragEnter);
                return repositoryItemMemoEdit;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        void repositoryItemMemoEdit_DragEnter(object sender, DragEventArgs e)
        {
            try
            {
                RepositoryItemMemoEdit repositoryItemMemoEdit = (sender as RepositoryItemMemoEdit);
                if (e.Data.GetDataPresent(typeof(KeyValuePair<string, object>)))
                {
                    KeyValuePair<string, object> keyvalue
                        = (KeyValuePair<string, object>)(e.Data.GetData(typeof(KeyValuePair<string, object>)));
                    if (keyvalue.Value.ToString().ToUpper() == "TEXT")
                    {
                        e.Effect = DragDropEffects.Copy;
                        //repositoryItemMemoEdit.
                        //repositoryItemMemoEdit.SelectionStart = txtValue.Text.Length;
                    }
                    else
                    {

                        e.Effect = DragDropEffects.None;
                    }
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.StackTrace);
            }
        }

        void repositoryItemMemoEdit_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                RepositoryItemMemoEdit repositoryItemMemoEdit = (sender as RepositoryItemMemoEdit);
                if (e.Data.GetDataPresent(typeof(KeyValuePair<string, object>)))
                {
                    KeyValuePair<string, object> keyvalue = (KeyValuePair<string, object>)(e.Data.GetData(typeof(KeyValuePair<string, object>)));
                    if (keyvalue.Value.ToString().ToUpper() != "TEXT") return;
                    string strInsertText = keyvalue.Key.ToString();

                    //this.txtValue.Text = this.txtValue.Text.Insert(start, strInsertText);
                    //this.txtValue.SelectionStart = this.txtValue.Text.Length;
                }
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.StackTrace);
            }
        }


        /// <summary>
        /// 多选
        /// </summary>
        /// <param name="dateElement"></param>
        /// <returns></returns>
        private RepositoryItemCheckedComboBoxEdit SetRepositoryItemCheckedComboBoxEdit(DataElementEntity dateElement)
        {
            try
            {
                RepositoryItemCheckedComboBoxEdit repositoryItemCheckedComboBoxEdit = new RepositoryItemCheckedComboBoxEdit();
                repositoryItemCheckedComboBoxEdit.DataSource = dateElement.BaseOptionList;
                repositoryItemCheckedComboBoxEdit.SelectAllItemCaption = "选中所有";
                repositoryItemCheckedComboBoxEdit.ShowButtons = false;
                repositoryItemCheckedComboBoxEdit.DisplayMember = "Name";
                repositoryItemCheckedComboBoxEdit.ValueMember = "Id";
                return repositoryItemCheckedComboBoxEdit;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        /// <summary>
        /// 日期型
        /// </summary>
        /// <param name="dateElement"></param>
        /// <returns></returns>
        private RepositoryItemDateEdit SetRepositoryItemDateEdit(DataElementEntity dateElement)
        {
            try
            {
                RepositoryItemDateEdit repositoryItemDateEdit = new RepositoryItemDateEdit();
                if (dateElement.ElementType == "DT")
                {
                    repositoryItemDateEdit.VistaDisplayMode = DefaultBoolean.True; ;
                    repositoryItemDateEdit.VistaEditTime = DefaultBoolean.True;
                    repositoryItemDateEdit.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
                    repositoryItemDateEdit.EditFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
                    repositoryItemDateEdit.EditMask = "yyyy-MM-dd HH:mm:ss";
                }
                return repositoryItemDateEdit;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 时间型
        /// </summary>
        /// <param name="dateElement"></param>
        /// <returns></returns>
        private RepositoryItemTimeEdit SetRepositoryItemTimeEdit(DataElementEntity dateElement)
        {
            RepositoryItemTimeEdit repositoryItemTimeEdit = new RepositoryItemTimeEdit();
            return repositoryItemTimeEdit;
        }


        /// <summary>
        /// 布尔型
        /// </summary>
        /// <param name="dateElement"></param>
        /// <returns></returns>
        private RepositoryItemCheckEdit SetRepositoryItemCheckEdit(DataElementEntity dateElement)
        {
            RepositoryItemCheckEdit repositoryItemCheckEdit = new RepositoryItemCheckEdit();
            return repositoryItemCheckEdit;
        }

        /// <summary>
        /// 数值类型
        /// Edit by xlb 2013-03-11
        /// </summary>
        /// <param name="dateElement"></param>
        /// <returns></returns>
        private RepositoryItemSpinEdit SetRepositoryItemSpinEdit(DataElementEntity dateElement)
        {
            try
            {

                RepositoryItemSpinEdit repositoryItemSpinEdit = new RepositoryItemSpinEdit();
                Dictionary<string, string> dicStrList = DataElementEntity.GetMaxMinDefStr(dateElement.ElementRange);
                if (dicStrList == null || !dicStrList.ContainsKey("MaxValue") || !dicStrList.ContainsKey("MinValue")) return repositoryItemSpinEdit;
                decimal maxValue;
                decimal minValue;
                bool ismax = Decimal.TryParse(dicStrList["MaxValue"], out maxValue);
                bool ismin = Decimal.TryParse(dicStrList["MinValue"], out minValue);
                if (ismax && ismin)
                {
                    repositoryItemSpinEdit.MaxValue = maxValue;
                    repositoryItemSpinEdit.MinValue = minValue;
                }
                //屏蔽右键菜单
                repositoryItemSpinEdit.ContextMenuStrip = contextMenuStrip1;
                return repositoryItemSpinEdit;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        #endregion


        /// <summary>
        /// 将数据库字段转换成DateTable但未对DateTable赋值
        /// </summary>
        /// <returns></returns>
        private DataTable GetDateTable()
        {
            try
            {
                dataElementList = new Dictionary<string, DataElementEntity>();
                DataTable dt = new DataTable();
                DataColumn dc = null;

                //xll 2013-08-19 无论有无数据 都从维护处取表结构
                //   if (m_InCommonNoteTab == null || m_InCommonNoteTab.InCommonNoteItemList == null || m_InCommonNoteTab.InCommonNoteItemList.Count <= 0)
                //  {

                dc = new DataColumn("xgnum");
                dc.Caption = "修改次数";
                dc.DataType = typeof(int);
                dt.Columns.Add(dc);

                dc = new DataColumn("groupFlow");
                dc.Caption = "流水号";
                dc.DataType = typeof(String);
                dt.Columns.Add(dc);

                dc = new DataColumn("jlsj");
                dc.Caption = "记录时间";
                dc.DataType = typeof(DateTime);
                dt.Columns.Add(dc);
                inCommonNoteBiz.SetCommonTabOtherName(m_commonNote_TabEntity, m_InCommonNoteTab.InCommonNote_Tab_Flow);
                foreach (var item in m_commonNote_TabEntity.CommonNote_ItemList)
                {
                    dc = new DataColumn(item.CommonNote_Item_Flow);
                    if (string.IsNullOrEmpty(item.OtherName))
                    {
                        dc.Caption = "未指定列";
                    }
                    else
                    {
                        dc.Caption = item.OtherName;
                    }

                    if (item.DataElement == null)
                    {
                        item.DataElement = m_DataElemntBiz.GetDataElement(item.DataElementFlow);
                    }
                    dataElementList.Add(item.CommonNote_Item_Flow, item.DataElement);
                    Type type = GetDatetype(item.DataElement.ElementType);
                    if (type == null) continue;
                    dc.DataType = type;
                    dt.Columns.Add(dc);
                }

                dc = new DataColumn("jlr");
                dc.Caption = "记录人";
                dc.DataType = typeof(string);
                dt.Columns.Add(dc);
                //}
                //else
                //{
                //    dc = new DataColumn("xgnum");
                //    dc.Caption = "修改次数";
                //    dc.DataType = typeof(int);
                //    dt.Columns.Add(dc);

                //    dc = new DataColumn("groupFlow");
                //    dc.Caption = "流水号";
                //    dc.DataType = typeof(String);
                //    dt.Columns.Add(dc);

                //    dc = new DataColumn("jlsj");
                //    dc.Caption = "记录时间";
                //    dc.DataType = typeof(DateTime);
                //    dt.Columns.Add(dc);
                //    string groupflow = "";

                //    for (int i = 0; i < m_InCommonNoteTab.InCommonNoteItemList.Count; i++)
                //    {
                //        var inComItem = m_InCommonNoteTab.InCommonNoteItemList[i];
                //        if (i == 0)
                //        {
                //            groupflow = inComItem.GroupFlow;
                //        }
                //        if (m_InCommonNoteTab.InCommonNoteItemList[i].GroupFlow == groupflow)
                //        {
                //            dc = new DataColumn(inComItem.CommonNote_Item_Flow);
                //            if (string.IsNullOrEmpty(inComItem.OtherName))
                //            {
                //                dc.Caption = "未指定列";
                //            }
                //            else
                //            {
                //                dc.Caption = inComItem.OtherName;
                //            }

                //            if (inComItem.DataElement == null)
                //            {
                //                inComItem.DataElement = m_DataElemntBiz.GetDataElement(inComItem.DataElementFlow);
                //            }
                //            dataElementList.Add(inComItem.CommonNote_Item_Flow, inComItem.DataElement);
                //            Type type;
                //            if (inComItem.IsValidate == "否")//如果不对数据元格式进行校验 则都是String
                //            {
                //                type = typeof(String);
                //            }
                //            else
                //            {
                //                type = GetDatetype(inComItem.DataElement.ElementType);
                //            }
                //            if (type == null) continue;
                //            dc.DataType = type;
                //            dt.Columns.Add(dc);
                //        }
                //    }

                //    dc = new DataColumn("jlr");
                //    dc.Caption = "记录人";
                //    dc.DataType = typeof(string);
                //    dt.Columns.Add(dc);
                //}
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        /// <summary>
        /// 通过对象设置该对象的DataTableValue的值
        /// </summary>
        /// <param name="inCommonNoteItemEntity"></param>
        private void SetDataTableValue(InCommonNoteItemEntity inCommonNoteItemEntity)
        {
            try
            {
                string elementType = inCommonNoteItemEntity.DataElement.ElementType;
                inCommonNoteItemEntity.DataTableValue = "";
                if (inCommonNoteItemEntity.IsValidate == "否")
                {
                    if (inCommonNoteItemEntity.BaseValueList != null && inCommonNoteItemEntity.BaseValueList.Count == 1)
                    {
                        inCommonNoteItemEntity.DataTableValue = inCommonNoteItemEntity.BaseValueList[0].Name;
                    }
                    return;
                }
                #region 对不同类型的值进行赋值
                if (elementType.ToUpper() == "S2"
                    || elementType.ToUpper() == "S3"
                     || elementType.ToUpper() == "S1"
                     || elementType.ToUpper() == "S4")
                {
                    if (inCommonNoteItemEntity.BaseValueList != null && inCommonNoteItemEntity.BaseValueList.Count == 1)
                    {
                        inCommonNoteItemEntity.DataTableValue = inCommonNoteItemEntity.BaseValueList[0].Name;
                    }
                }
                else if (elementType.ToUpper() == "S9")
                {
                    if (inCommonNoteItemEntity.BaseValueList != null && inCommonNoteItemEntity.BaseValueList.Count > 0)
                    {

                        foreach (var item in inCommonNoteItemEntity.BaseValueList)
                        {
                            inCommonNoteItemEntity.DataTableValue += item.Id + ",";
                        }
                        if (inCommonNoteItemEntity.DataTableValue.ToString().Length > 0)
                        {
                            inCommonNoteItemEntity.DataTableValue = inCommonNoteItemEntity.DataTableValue.ToString().Substring(0, inCommonNoteItemEntity.DataTableValue.ToString().Length - 1);
                        }
                    }
                }
                else if (elementType.ToUpper() == "DT" || elementType.ToUpper() == "D" || elementType.ToUpper() == "T")
                {
                    string datatimestr = "";
                    if (inCommonNoteItemEntity.BaseValueList != null && inCommonNoteItemEntity.BaseValueList.Count == 1)
                    {
                        datatimestr = inCommonNoteItemEntity.BaseValueList[0].Name;
                    }
                    DateTime dt;
                    bool isdt = DateTime.TryParse(datatimestr, out dt);
                    if (isdt)
                    {
                        inCommonNoteItemEntity.DataTableValue = dt;
                    }
                    else
                    {
                        inCommonNoteItemEntity.DataTableValue = DateTime.Now;
                    }
                }
                else if (elementType.ToUpper() == "N")
                {
                    string decStr = "";
                    if (inCommonNoteItemEntity.BaseValueList != null && inCommonNoteItemEntity.BaseValueList.Count == 1)
                    {
                        decStr = inCommonNoteItemEntity.BaseValueList[0].Name;
                    }
                    Decimal dec;
                    bool isdec = Decimal.TryParse(decStr, out dec);
                    if (isdec)
                    {
                        inCommonNoteItemEntity.DataTableValue = dec;
                    }
                    else
                    {
                        inCommonNoteItemEntity.DataTableValue = null;
                    }
                }
                else if (elementType.ToUpper() == "L")
                {
                    string boolStr = "";
                    if (inCommonNoteItemEntity.BaseValueList != null && inCommonNoteItemEntity.BaseValueList.Count == 1)
                    {
                        boolStr = inCommonNoteItemEntity.BaseValueList[0].Name;
                    }
                    inCommonNoteItemEntity.DataTableValue = boolStr == "是" ? true : false;
                }
                #endregion
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 将值插入到dataTable中
        /// </summary>
        /// <param name="dt"></param>
        private void SetValueToDataTable(DataTable dt)
        {
            try
            {
                dicitemList = new Dictionary<string, List<InCommonNoteItemEntity>>();
                if (m_InCommonNoteTab == null
                      || m_InCommonNoteTab.InCommonNoteItemList == null
                      || m_InCommonNoteTab.InCommonNoteItemList.Count <= 0)
                {
                    return;
                }

                DicAddIncommitem(m_InCommonNoteTab.InCommonNoteItemList);

                //foreach (var item in m_InCommonNoteTab.InCommonNoteItemList)  //将同一组的数据放在一个里面
                //{
                //    if (!dicitemList.Keys.Contains(item.GroupFlow))
                //    {
                //        dicitemList.Add(item.GroupFlow, new List<InCommonNoteItemEntity>());
                //    }
                //    dicitemList[item.GroupFlow].Add(item);
                //}

                foreach (List<InCommonNoteItemEntity> itemList in dicitemList.Values)
                {
                    //xll 20130319
                    AddDataTableRow(itemList, dt);
                    //DataRow dtRow = dt.NewRow();
                    //string dateTime = itemList[0].RecordDate + " " + itemList[0].RecordTime;
                    //dtRow["jlsj"] = Convert.ToDateTime(dateTime);
                    //dtRow["groupFlow"] = itemList[0].GroupFlow;
                    //dtRow["jlr"] = itemList[0].RecordDoctorName;
                    //for (int i = 0; i < itemList.Count; i++)
                    //{
                    //    if (dtRow[itemList[i].CommonNote_Item_Flow] == null) continue;
                    //    itemList[i].DataElement = dataElementList[itemList[i].CommonNote_Item_Flow];
                    //    SetDataTableValue(itemList[i]);
                    //    if (itemList[i].DataTableValue != null && itemList[i].DataTableValue.ToString() != "")
                    //    {
                    //        dtRow[itemList[i].CommonNote_Item_Flow] = itemList[i].DataTableValue;
                    //    }

                    //}
                    //dt.Rows.Add(dtRow);

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        /// <summary>
        /// 通过数据元素的类型来得到真实的数据类型
        /// </summary>
        /// <param name="elementType"></param>
        /// <returns></returns>
        private Type GetDatetype(string elementType)
        {

            try
            {
                if (string.IsNullOrEmpty(elementType)) return null;
                Type type = null;
                switch (elementType.ToUpper())
                {
                    case "S1":
                        type = typeof(String);
                        break;
                    case "S2":
                        type = typeof(String);
                        break;
                    case "S3":
                        type = typeof(String);
                        break;
                    case "S4":
                        type = typeof(String);
                        break;
                    case "S9":
                        type = typeof(String);
                        break;
                    case "DT":
                        type = typeof(DateTime);
                        break;
                    case "D":
                        type = typeof(DateTime);
                        break;
                    case "T":
                        type = typeof(DateTime);
                        break;
                    case "N":
                        type = typeof(Decimal);
                        break;
                    case "L":
                        type = typeof(bool);
                        break;

                }
                return type;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        private void gridViewTab_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                DataRow dr = gridViewTab.GetFocusedDataRow();
                SetColEdit(dr);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
            //scorlInfo.Controls.Clear();
            //PrintInCommonItemView printInCommonItemView = gridViewTab.GetFocusedRow() as PrintInCommonItemView;
            //if (printInCommonItemView == null) return;
            //List<InCommonNoteItemEntity> inCommonNoteItemEntityList = dicitemList[printInCommonItemView.GroupFlow];
            //Point point = new Point(0, 0);
            //#region 如果控件不存在
            //if (uCRecordDateTime == null && ucLabTextList == null && uCRecordDoctor == null)
            //{
            //    for (int i = 0; i < inCommonNoteItemEntityList.Count; i++)
            //    {
            //        if (i == 0)
            //        {
            //            uCRecordDateTime = new UCRecordDateTime(inCommonNoteItemEntityList[i].RecordDate, inCommonNoteItemEntityList[i].RecordTime);
            //            point = new Point(0, 0);
            //            uCRecordDateTime.Location = point;
            //            uCRecordDateTime.Name = "uCRecordDateTime";
            //            scorlInfo.Controls.Add(uCRecordDateTime);
            //        }

            //        if (inCommonNoteItemEntityList[i].DataElement == null)
            //        {
            //            DataElemntBiz datebiz = new DataElemntBiz(m_app);
            //            inCommonNoteItemEntityList[i].DataElement = datebiz.GetDataElement(inCommonNoteItemEntityList[i].DataElementFlow);
            //        }
            //        ucLabText ucLabText = new ucLabText(inCommonNoteItemEntityList[i], m_app);
            //        ucLabText.Height = 35;
            //        ucLabText.Width = 300;
            //        int row = (i + 1) / 3;
            //        int colmn = (i + 1) % 3;
            //        point.X = ucLabText.Width * colmn;
            //        point.Y = ucLabText.Height * row;
            //        ucLabText.Location = point;
            //        scorlInfo.Controls.Add(ucLabText);
            //        if (ucLabTextList == null)
            //        {
            //            ucLabTextList = new Dictionary<string, ucLabText>();
            //        }
            //        ucLabTextList.Add(inCommonNoteItemEntityList[i].CommonNote_Item_Flow, ucLabText);
            //        if (i == inCommonNoteItemEntityList.Count - 1)
            //        {
            //            if (string.IsNullOrEmpty(inCommonNoteItemEntityList[i].RecordDoctorName)
            //                && string.IsNullOrEmpty(inCommonNoteItemEntityList[i].InCommonNote_Item_Flow))
            //            {
            //                inCommonNoteItemEntityList[i].RecordDoctorName = m_app.User.DoctorName;
            //            }
            //            uCRecordDoctor = new UCRecordDoctor(inCommonNoteItemEntityList[i].RecordDoctorName);
            //            uCRecordDoctor.Height = 35;
            //            uCRecordDoctor.Width = 300;
            //            uCRecordDoctor.Name = "uCRecordDoctor";
            //            int row1 = (i + 2) / 3;
            //            int colmn1 = (i + 2) % 3;
            //            point.X = uCRecordDoctor.Width * colmn1;
            //            point.Y = uCRecordDoctor.Height * row1;
            //            uCRecordDoctor.Location = point;
            //            scorlInfo.Controls.Add(uCRecordDoctor);
            //        }
            //    }
            //    dockPanelInfo.Height = uCRecordDoctor.Location.Y + 100; //设置容器的高度
            //}
            //#endregion
            //else
            //{
            //    for (int i = 0; i < inCommonNoteItemEntityList.Count; i++)
            //    {
            //        if (i == 0)
            //        {
            //            uCRecordDateTime.InitData(inCommonNoteItemEntityList[i].RecordDate, inCommonNoteItemEntityList[i].RecordTime);
            //        }
            //        ucLabText ucLabText = ucLabTextList[inCommonNoteItemEntityList[i].CommonNote_Item_Flow];
            //        if (ucLabText != null)
            //        {
            //            ucLabText.InitDate(inCommonNoteItemEntityList[i], m_app);
            //        }

            //        if (i == inCommonNoteItemEntityList.Count - 1)
            //        {
            //            if (string.IsNullOrEmpty(inCommonNoteItemEntityList[i].RecordDoctorName)
            //                && string.IsNullOrEmpty(inCommonNoteItemEntityList[i].InCommonNote_Item_Flow))
            //            {
            //                inCommonNoteItemEntityList[i].RecordDoctorName = m_app.User.DoctorName;
            //            }
            //            uCRecordDoctor.InitDate(inCommonNoteItemEntityList[i].RecordDoctorName);

            //        }
            //    }
            //}

        }


        /// <summary>
        /// 根据当前行的记录人判断是否可编辑
        /// </summary>
        /// <param name="dr"></param>
        private void SetColEdit(DataRow dr)
        {
            try
            {
                if (dr == null || m_CanSign == false) return;
                if (dicitemList == null || dicitemList.Count <= 0) return;
                List<InCommonNoteItemEntity> InCommonNoteItemList = dicitemList[dr["groupFlow"].ToString()];
                if (InCommonNoteItemList == null || InCommonNoteItemList.Count <= 0) return;
                //护士长可编辑所有列
                if (isNurseHeard)
                {
                    foreach (GridColumn item in gridViewTab.Columns)
                    {
                        if (item.FieldName == "jlr")
                        {
                            item.OptionsColumn.AllowEdit = false;
                        }
                        else
                        {
                            item.OptionsColumn.AllowEdit = true;
                        }
                    }
                }
                else
                {
                    if (m_app.User.DoctorId == InCommonNoteItemList[0].RecordDoctorId || string.IsNullOrEmpty(InCommonNoteItemList[0].RecordDoctorName)) //记录人相同或没有记录人时
                    {
                        foreach (GridColumn item in gridViewTab.Columns)
                        {
                            if (item.FieldName == "jlr")
                            {
                                item.OptionsColumn.AllowEdit = false;
                            }
                            else
                            {
                                item.OptionsColumn.AllowEdit = true;
                            }
                        }
                    }
                    else
                    {
                        foreach (GridColumn item in gridViewTab.Columns)
                        {
                            item.OptionsColumn.AllowEdit = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 新增触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {

                bool hasMax = HasMaxRow(m_InCommonNoteTab.InCommonNote_Tab_Flow);
                if (hasMax)
                {
                    return;
                }
                string groupFlow = "";
                if (dicitemList == null || dicitemList.Count == 0)
                {
                    AddCommonItemToInCommonItem(m_commonNote_TabEntity, m_InCommonNoteTab, m_app, ref groupFlow);
                    bool hasZhidinglie = ZhiDingWeiZhiDingLie(m_InCommonNoteTab);
                    if (!hasZhidinglie)
                    {
                        m_InCommonNoteTab.InCommonNoteItemList = null;
                        return;
                    }
                    AddRowCount++;
                    BindData();
                }
                else
                {
                    List<InCommonNoteItemEntity> inCommonNoteItemList = new List<InCommonNoteItemEntity>();
                    foreach (var item in dicitemList)
                    {
                        inCommonNoteItemList = item.Value;
                        break;
                    }
                    var incommnoteitemList = AddCommonItemToInCommonItem(m_InCommonNoteTab, inCommonNoteItemList, m_app, ref groupFlow, DateTime.Now);
                    if (incommnoteitemList == null)
                    {
                        return;
                    }
                    AddRowCount++;
                    DataTable dataTable = gridControl1.DataSource as DataTable;
                    DicAddIncommitem(incommnoteitemList);
                    AddDataTableRow(incommnoteitemList, dataTable);
                }
                gridViewTab.MoveBy(dicitemList.Count - 1);
                gridViewTab.SelectRow(dicitemList.Count);
                SetValueToInCommonNoteItem(gridViewTab);
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        /// xll 20130319 向一个集合中添加分组数据 便于获取
        /// </summary>
        /// <param name="itemList"></param>
        private void DicAddIncommitem(List<InCommonNoteItemEntity> itemList)
        {

            foreach (var item in itemList)  //将同一组的数据放在一个里面
            {
                if (!dicitemList.Keys.Contains(item.GroupFlow))
                {
                    dicitemList.Add(item.GroupFlow, new List<InCommonNoteItemEntity>());
                }
                dicitemList[item.GroupFlow].Add(item);
            }
        }

        /// <summary>
        /// xll 20130319 优化添加方法 使得添加一行是提高效率
        /// </summary>
        /// <param name="itemList"></param>
        /// <param name="dt"></param>
        private void AddDataTableRow(List<InCommonNoteItemEntity> itemList, DataTable dt)
        {
            try
            {
                if (itemList == null || itemList.Count == 0) return;
                DataRow dtRow = dt.NewRow();
                string dateTime = itemList[0].RecordDate + " " + itemList[0].RecordTime;
                dtRow["jlsj"] = Convert.ToDateTime(dateTime);
                dtRow["xgnum"] = Convert.ToInt32(itemList[0].Xgnum);
                dtRow["groupFlow"] = itemList[0].GroupFlow;
                dtRow["jlr"] = itemList[0].RecordDoctorName;
                for (int i = 0; i < itemList.Count; i++)
                {

                    if (dtRow[itemList[i].CommonNote_Item_Flow] == null) continue;
                    itemList[i].DataElement = dataElementList[itemList[i].CommonNote_Item_Flow];
                    SetDataTableValue(itemList[i]);
                    if (itemList[i].DataTableValue != null && itemList[i].DataTableValue.ToString() != "")
                    {
                        dtRow[itemList[i].CommonNote_Item_Flow] = itemList[i].DataTableValue;
                    }

                }
                dt.Rows.Add(dtRow);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            // dt.DefaultView.Sort = "jlsj asc";
        }

        /// <summary>
        /// 注册右键菜单事件
        /// Add by xlb 2013-03-11
        /// </summary>
        private void RegisterEvent()
        {
            try
            {
                barButtonItemEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(barButtonItemEdit_ItemClick);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 将配置的项目转换成病人的项目 适用于改tab下没有病人数据项时
        /// </summary>
        private void AddCommonItemToInCommonItem(
            CommonNote_TabEntity commonNoteTab,
            InCommonNoteTabEntity inCommonNoteTab, IEmrHost m_app, ref string gruopflow)
        {
            if (commonNoteTab.CommonNote_ItemList == null) return;
            List<InCommonNoteItemEntity> inCommonNoteItemList = new List<InCommonNoteItemEntity>();
            gruopflow = Guid.NewGuid().ToString();
            foreach (var commonitem in commonNoteTab.CommonNote_ItemList)
            {
                InCommonNoteItemEntity inCommonNoteItem = new InCommonNoteItemEntity();
                inCommonNoteItem.CommonNote_Item_Flow = commonitem.CommonNote_Item_Flow;
                inCommonNoteItem.CommonNote_Tab_Flow = commonitem.CommonNote_Tab_Flow;
                inCommonNoteItem.CommonNoteFlow = commonitem.CommonNoteFlow;
                inCommonNoteItem.DataElementFlow = commonitem.DataElementFlow;
                inCommonNoteItem.DataElementId = commonitem.DataElementId;
                inCommonNoteItem.DataElementName = commonitem.DataElementName;
                inCommonNoteItem.DataElement = commonitem.DataElement;
                inCommonNoteItem.IsValidate = commonitem.IsValidate;
                inCommonNoteItem.OrderCode = commonitem.OrderCode;
                inCommonNoteItem.OtherName = commonitem.OtherName;
                inCommonNoteItem.GroupFlow = gruopflow;
                inCommonNoteItem.InCommonNote_Tab_Flow = inCommonNoteTab.InCommonNote_Tab_Flow;
                inCommonNoteItem.InCommonNoteFlow = inCommonNoteTab.InCommonNoteFlow;
                inCommonNoteItem.RecordDate = DateUtil.getDateTime(DateTime.Now.ToString(), DateUtil.NORMAL_SHORT);
                inCommonNoteItem.RecordTime = DateUtil.getDateTime(DateTime.Now.ToString(), DateUtil.NORMAL_LONG).Substring(11, 8);
                inCommonNoteItem.RecordDoctorName = m_app.User.DoctorName;
                inCommonNoteItem.RecordDoctorId = m_app.User.DoctorId;
                inCommonNoteItem.Valide = "1";
                SetDefalutValue(inCommonNoteItem);
                inCommonNoteItemList.Add(inCommonNoteItem);
            }
            if (inCommonNoteTab.InCommonNoteItemList == null)
                inCommonNoteTab.InCommonNoteItemList = new List<InCommonNoteItemEntity>();
            inCommonNoteTab.InCommonNoteItemList.AddRange(inCommonNoteItemList);

        }

        /// <summary>
        /// 设置新增项目的默认值
        /// </summary>
        /// <param name="inCommonNoteItem"></param>
        private void SetDefalutValue(InCommonNoteItemEntity inCommonNoteItem)
        {
            try
            {
                if (inCommonNoteItem.DataElement == null)
                {
                    inCommonNoteItem.DataElement = dataElementList[inCommonNoteItem.CommonNote_Item_Flow];
                }
                if (inCommonNoteItem.DataElement.ElementType == "S2"
                    || inCommonNoteItem.DataElement.ElementType == "S3"
                    || inCommonNoteItem.DataElement.ElementType == "S9")
                {
                    List<BaseDictory> baseList = DataElementEntity.GetDefaultOption(inCommonNoteItem.DataElement.ElementRange);
                    inCommonNoteItem.ValueXml = InCommonNoteItemEntity.ConvertBaseToXml(baseList);
                }
                else if (inCommonNoteItem.DataElement.ElementType == "N")
                {
                    Dictionary<string, string> dicStr = DataElementEntity.GetMaxMinDefStr(inCommonNoteItem.DataElement.ElementRange);
                    if (dicStr == null) return;
                    if (dicStr.ContainsKey("DefaultValue"))
                    {
                        inCommonNoteItem.ValueXml = InCommonNoteItemEntity.ConvertStrToXml(dicStr["DefaultValue"]);
                    }
                    else if (dicStr.ContainsKey("MinValue"))
                    {
                        inCommonNoteItem.ValueXml = InCommonNoteItemEntity.ConvertStrToXml(dicStr["MinValue"]);
                    }
                    else
                    {
                        inCommonNoteItem.ValueXml = InCommonNoteItemEntity.ConvertStrToXml("");
                    }
                    if (dicStr.ContainsKey("StepValue") && dicStr.ContainsKey("DefaultValue"))
                    {
                        int stepint;
                        int defint;
                        bool isint = int.TryParse(dicStr["StepValue"], out stepint);
                        bool isintdef = int.TryParse(dicStr["DefaultValue"], out defint);
                        if (!isint || !isintdef) return;
                        int rows = 0;
                        if (dicitemList != null)
                        {
                            rows = dicitemList.Count;
                        }
                        string valueNow = (defint + stepint * rows).ToString();
                        inCommonNoteItem.ValueXml = InCommonNoteItemEntity.ConvertStrToXml(valueNow);

                    }
                }
                else
                {
                    Dictionary<string, string> dicStr = DataElementEntity.GetMaxMinDefStr(inCommonNoteItem.DataElement.ElementRange);
                    if (dicStr == null) return;
                    if (dicStr.ContainsKey("DefaultValue"))
                    {
                        inCommonNoteItem.ValueXml = InCommonNoteItemEntity.ConvertStrToXml(dicStr["DefaultValue"]);
                    }
                    if (dicStr.ContainsKey("StepValue"))
                    {
                        int stepint;
                        int defint;
                        bool isint = int.TryParse(dicStr["StepValue"], out stepint);
                        bool isintdef = int.TryParse(dicStr["DefaultValue"], out defint);
                        if (!isint || !isintdef) return;
                        int rows = 0;
                        if (dicitemList != null)
                        {
                            rows = dicitemList.Count;
                        }
                        string valueNow = (defint + stepint * rows).ToString();
                        inCommonNoteItem.ValueXml = InCommonNoteItemEntity.ConvertStrToXml(valueNow);

                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        /// <summary>
        /// 将配置的项目转换成病人的项目 适用于改tab下有病人数据项时
        /// </summary>
        /// <param name="inCommonNoteTab"></param>
        /// <param name="InCommonNoteItemCopy">用于复制新数据的列</param>
        /// <param name="m_app"></param>
        /// <param name="groupFlow"></param>
        /// <param name="IsAddNext">是否是追加的记录 时间和前面时间相同</param>
        private List<InCommonNoteItemEntity> AddCommonItemToInCommonItem(
            InCommonNoteTabEntity inCommonNoteTab, List<InCommonNoteItemEntity> InCommonNoteItemCopy, IEmrHost m_app, ref string groupFlow, DateTime datetime)
        {
            List<InCommonNoteItemEntity> inCommonNoteItemList = new List<InCommonNoteItemEntity>();
            groupFlow = Guid.NewGuid().ToString();
            foreach (var commonitem in InCommonNoteItemCopy)
            {
                InCommonNoteItemEntity inCommonNoteItem = new InCommonNoteItemEntity();
                inCommonNoteItem.CommonNote_Item_Flow = commonitem.CommonNote_Item_Flow;
                inCommonNoteItem.CommonNote_Tab_Flow = commonitem.CommonNote_Tab_Flow;
                inCommonNoteItem.CommonNoteFlow = commonitem.CommonNoteFlow;
                inCommonNoteItem.DataElementFlow = commonitem.DataElementFlow;
                inCommonNoteItem.DataElementId = commonitem.DataElementId;
                inCommonNoteItem.DataElementName = commonitem.DataElementName;
                inCommonNoteItem.DataElement = commonitem.DataElement;
                inCommonNoteItem.IsValidate = commonitem.IsValidate;
                inCommonNoteItem.OrderCode = commonitem.OrderCode;
                inCommonNoteItem.OtherName = commonitem.OtherName;
                inCommonNoteItem.GroupFlow = groupFlow;
                inCommonNoteItem.InCommonNote_Tab_Flow = inCommonNoteTab.InCommonNote_Tab_Flow;
                inCommonNoteItem.InCommonNoteFlow = inCommonNoteTab.InCommonNoteFlow;


                inCommonNoteItem.RecordDate = DateUtil.getDateTime(datetime.ToString(), DateUtil.NORMAL_SHORT);
                inCommonNoteItem.RecordTime = DateUtil.getDateTime(datetime.ToString(), DateUtil.NORMAL_LONG).Substring(11, 8);


                inCommonNoteItem.RecordDoctorName = m_app.User.DoctorName;
                inCommonNoteItem.RecordDoctorId = m_app.User.DoctorId;
                SetDefalutValue(inCommonNoteItem);
                inCommonNoteItemList.Add(inCommonNoteItem);
            }
            if (inCommonNoteTab.InCommonNoteItemList == null)
                inCommonNoteTab.InCommonNoteItemList = new List<InCommonNoteItemEntity>();
            inCommonNoteTab.InCommonNoteItemList.AddRange(inCommonNoteItemList);
            return inCommonNoteItemList;
        }

        private bool HasMaxRow(string incommnotetabFlow)
        {

            //设置每张单据最多可添加多少条记录
            int maxrows = 0;
            int.TryParse(m_commonNote_TabEntity.MaxRows, out maxrows);
            int rowcount = inCommonNoteBiz.GetRowCount(incommnotetabFlow);  //数据库中行数
            if (maxrows > 0 && (rowcount + AddRowCount) >= maxrows)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该单据最多只可添加" + maxrows + "条记录，请重新新增单据");
                return true;
            }
            return false;
        }


        /// <summary>
        /// 指定未指定的列
        /// </summary>
        /// <param name="m_InCommonNoteTab"></param>
        private bool ZhiDingWeiZhiDingLie(InCommonNoteTabEntity m_InCommonNoteTab)
        {
            try
            {
                if (m_InCommonNoteTab == null || m_InCommonNoteTab.InCommonNoteItemList == null) return true;
                List<InCommonNoteItemEntity> inCommonNoother = new List<InCommonNoteItemEntity>();
                foreach (var item in m_InCommonNoteTab.InCommonNoteItemList)
                {
                    if (string.IsNullOrEmpty(item.OtherName))
                    {
                        inCommonNoother.Add(item);
                    }
                }
                if (inCommonNoother == null || inCommonNoother.Count == 0) return true;
                ZhiDingLieMing zhiDingLieMing = new ZhiDingLieMing(inCommonNoother);
                DialogResult diaResult = zhiDingLieMing.ShowDialog();
                if (diaResult == DialogResult.OK)
                {
                    inCommonNoteBiz.updateIncommonType(inCommonNoother);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        /// <summary>
        /// 删除触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow dataRow = gridViewTab.GetFocusedDataRow();
                if (dataRow == null)
                {
                    m_app.CustomMessageBox.MessageShow("未选中要删除的记录");
                    return;
                }

                string groupflow = dataRow["groupFlow"].ToString();
                List<InCommonNoteItemEntity> inCommonNoteItemEntityList = dicitemList[groupflow];

                if (m_CanSign == true && inCommonNoteItemEntityList[0].RecordDoctorId != m_app.User.DoctorId && !isNurseHeard)
                {
                    m_app.CustomMessageBox.MessageShow("只有记录者和护士长才可删除该记录");
                    return;
                }

                DialogResult diaResult = m_app.CustomMessageBox.MessageShow("确定要删除选中数据吗？", DrectSoft.Core.CustomMessageBoxKind.QuestionYesNo);
                if (diaResult == DialogResult.No) return;


                //如果改变的集合中有改数据 移除删除的数据
                if (dicitemListChanged.ContainsKey(groupflow))
                {
                    dicitemListChanged.Remove(groupflow);
                }
                if (dicitemList.ContainsKey(groupflow))
                {
                    dicitemList.Remove(groupflow);
                }


                //直接数据库删除
                string message = "";
                string rowhisFlow = Guid.NewGuid().ToString();
                foreach (var item in inCommonNoteItemEntityList)
                {
                    item.Valide = "0";
                    if (string.IsNullOrEmpty(item.InCommonNote_Item_Flow)) continue;
                    bool itemresult = inCommonNoteBiz.SaveIncommonNoteItem(item, ref message);
                    InCommonNoteBiz.AddInCommonColHistory(item, rowhisFlow);
                }
                InCommonNoteBiz.AddInCommonRowHistory(inCommonNoteItemEntityList[0], rowhisFlow);
                if (inCommonNoteItemEntityList[0].InCommonNote_Item_Flow == null)
                {
                    AddRowCount--;
                }

                //将删除的集合添加到删除的集合中
                //if (!dicitemListDel.ContainsKey(groupflow))
                //{
                //    dicitemListDel.Add(groupflow, inCommonNoteItemEntityList);
                //}

                //m_InCommonNoteTab中的项目中移除删除的项目
                foreach (InCommonNoteItemEntity item in inCommonNoteItemEntityList)
                {
                    m_InCommonNoteTab.InCommonNoteItemList.Remove(item);
                }

                //if (inCommonNoteBiz == null)
                //    inCommonNoteBiz = new CommonNoteUse.InCommonNoteBiz(m_app);
                //string message = "";
                //foreach (var item in inCommonNoteItemEntityList)
                //{
                //    item.Valide = "0";
                //    bool itemResult = inCommonNoteBiz.SaveIncommonNoteItem(item, ref message);
                //}
                //inCommonNoteBiz.GetDetaliInCommonNote(ref m_inCommonNote);
                //m_InCommonNoteTab = m_inCommonNote.InCommonNoteTabList.Find(a => a.InCommonNote_Tab_Flow == m_InCommonNoteTab.InCommonNote_Tab_Flow);
                (gridControl1.DataSource as DataTable).Rows.Remove(dataRow);
                //BindData();
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.StackTrace);
            }
        }

        //保存
        public void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.Focus();
                if (inCommonNoteBiz == null)
                    inCommonNoteBiz = new InCommonNoteBiz(m_app);
                string messageAll = "";
                bool hasSave = true;

                //数据库中数据行数
                int datarowcount = inCommonNoteBiz.GetRowCount(m_InCommonNoteTab.InCommonNote_Tab_Flow);
                if (datarowcount <= 0)
                {
                    inCommonNoteBiz.SaveIncommonType(m_InCommonNoteTab.InCommonNote_Tab_Flow, m_commonNote_TabEntity);
                }
                int maxrows = 0;  //单据最大支持数据行数
                int.TryParse(m_commonNote_TabEntity.MaxRows, out maxrows);
                if (maxrows > 0 && AddRowCount > 0)
                {
                    if ((datarowcount + AddRowCount) > maxrows)
                    {
                        MessageBox.Show("已存在" + datarowcount + "条记录。无法再添加记录");
                        return;
                    }
                }
                //保存修改的数据
                foreach (List<InCommonNoteItemEntity> itemList in dicitemListChanged.Values)
                {
                    string rowhisflow = Guid.NewGuid().ToString(); //每次生成的历史行序号
                    foreach (var item in itemList)
                    {
                        string message = "";
                        bool itemresult = inCommonNoteBiz.SaveIncommonNoteItem(item, ref message);
                        if (!itemresult)
                        {
                            hasSave = false;
                            messageAll = message;
                        }
                        InCommonNoteBiz.AddInCommonColHistory(item, rowhisflow);
                    }
                    InCommonNoteBiz.AddInCommonRowHistory(itemList[0], rowhisflow);
                }
                AddRowCount = 0;
                if (hasSave == false)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("保存失败!" + messageAll);
                    return;
                }
                //保存删除的数据
                //foreach (var itemDelList in dicitemListDel.Values)
                //{
                //    foreach (var item in itemDelList)
                //    {
                //        string message = "";
                //        if (string.IsNullOrEmpty(item.InCommonNote_Item_Flow)) continue;
                //        item.Valide = "0";
                //        bool itemresult = inCommonNoteBiz.SaveIncommonNoteItem(item, ref message);
                //        if (!itemresult)
                //        {
                //            hasSave = false;
                //            messageAll = message;
                //        }
                //    } 
                //}
                if (hasSave == false)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("保存失败!" + messageAll);
                }
                else
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("保存成功");
                    //inCommonNoteBiz.GetDetaliInCommonNote(ref m_inCommonNote);
                    //m_InCommonNoteTab = m_inCommonNote.InCommonNoteTabList.Find(a => a.InCommonNote_Tab_Flow == m_InCommonNoteTab.InCommonNote_Tab_Flow);
                    //BindData();

                    dicitemListChanged = new Dictionary<string, List<InCommonNoteItemEntity>>();
                    dicitemListDel = new Dictionary<string, List<InCommonNoteItemEntity>>();
                }
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }

        }

        /// <summary>
        /// 序号列事件
        /// Add by xlb 2013-03-11
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewTab_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                if (e.Info.Kind == DevExpress.Utils.Drawing.IndicatorKind.Header)
                {
                    e.Info.DisplayText = "序号";
                }
                else if (e.Info.Kind == DevExpress.Utils.Drawing.IndicatorKind.Row)
                {
                    DataTable dataTable = gridControl1.DataSource as DataTable;
                    if (dataTable.Rows[e.RowHandle] != null
                        && Convert.ToInt32(dataTable.Rows[e.RowHandle]["xgnum"]) > 1)
                    {
                        e.Info.DisplayText = (e.RowHandle + 1).ToString() + "*";
                    }
                    else
                    {
                        e.Info.DisplayText = (e.RowHandle + 1).ToString();
                    }

                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 单元格值改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewTab_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                GridView gridView = (sender as GridView);
                SetValueToInCommonNoteItem(gridView);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 填充值方法
        /// </summary>
        /// <param name="gridView"></param>
        private void SetValueToInCommonNoteItem(GridView gridView)
        {

            try
            {
                DataRow dtRow = gridView.GetFocusedDataRow();
                //string columnName = gridView.FocusedColumn.Name;  //是列名 也是 commonNoteItem的流水号
                SetValueToIncommonItem(dtRow);

                IPrintNurse iPrintNurse = AbstractorFactry.GetNurseRecord(m_inCommonNote.PrinteModelName);
                if (iPrintNurse != null)
                {
                    List<InCommonNoteItemEntity> inCommonNoteItemList = dicitemList[dtRow["groupFlow"].ToString()];
                    DataTable dt = gridControl1.DataSource as DataTable;
                    iPrintNurse.SetDataRowZongLiang(inCommonNoteItemList, dtRow, dt);
                }

                SetValueToIncommonItem(dtRow);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetValueToIncommonItem(DataRow dtRow)
        {
            try
            {
                if ((dtRow == null) || !dicitemList.ContainsKey(dtRow["groupFlow"].ToString()))
                {
                    return;
                }
                List<InCommonNoteItemEntity> inCommonNoteItemList = dicitemList[dtRow["groupFlow"].ToString()];

                CommonNote_ItemEntity commonNote_ItemEntity = null;

                foreach (var commonNoteitem in m_commonNote_TabEntity.CommonNote_ItemList)
                {
                    commonNote_ItemEntity = commonNoteitem;
                    foreach (var item in inCommonNoteItemList)
                    {
                        if (commonNoteitem.CommonNote_Item_Flow == item.CommonNote_Item_Flow)
                        {
                            // 存在 就无需添加
                            commonNote_ItemEntity = null;
                            break;
                        }
                    }
                    if (commonNote_ItemEntity != null)
                    {
                        InCommonNoteItemEntity inCommonNoteItem = new InCommonNoteItemEntity();
                        inCommonNoteItem.CommonNote_Item_Flow = commonNote_ItemEntity.CommonNote_Item_Flow;
                        inCommonNoteItem.CommonNote_Tab_Flow = commonNote_ItemEntity.CommonNote_Tab_Flow;
                        inCommonNoteItem.CommonNoteFlow = commonNote_ItemEntity.CommonNoteFlow;
                        inCommonNoteItem.DataElementFlow = commonNote_ItemEntity.DataElementFlow;
                        inCommonNoteItem.DataElementId = commonNote_ItemEntity.DataElementId;
                        inCommonNoteItem.DataElementName = commonNote_ItemEntity.DataElementName;
                        inCommonNoteItem.DataElement = commonNote_ItemEntity.DataElement;
                        inCommonNoteItem.IsValidate = commonNote_ItemEntity.IsValidate;
                        inCommonNoteItem.OrderCode = commonNote_ItemEntity.OrderCode;
                        inCommonNoteItem.OtherName = commonNote_ItemEntity.OtherName;
                        inCommonNoteItem.GroupFlow = inCommonNoteItemList[0].GroupFlow;
                        inCommonNoteItem.InCommonNote_Tab_Flow = inCommonNoteItemList[0].InCommonNote_Tab_Flow;
                        inCommonNoteItem.InCommonNoteFlow = inCommonNoteItemList[0].InCommonNoteFlow;
                        inCommonNoteItem.RecordDate = DateUtil.getDateTime(DateTime.Now.ToString(), DateUtil.NORMAL_SHORT);
                        inCommonNoteItem.RecordTime = DateUtil.getDateTime(DateTime.Now.ToString(), DateUtil.NORMAL_LONG).Substring(11, 8);
                        inCommonNoteItem.RecordDoctorName = m_app.User.DoctorName;
                        inCommonNoteItem.RecordDoctorId = m_app.User.DoctorId;
                        inCommonNoteItem.Valide = "1";
                        SetDefalutValue(inCommonNoteItem);
                        dicitemList[dtRow["groupFlow"].ToString()].Add(inCommonNoteItem);
                    }
                }

                foreach (var item in inCommonNoteItemList)
                {
                    item.RecordDoctorName = dtRow["jlr"].ToString();
                    item.RecordDate = DateUtil.getDateTime(dtRow["jlsj"].ToString(), DateUtil.NORMAL_SHORT);
                    item.RecordTime = DateUtil.getDateTime(dtRow["jlsj"].ToString(), DateUtil.NORMAL_LONG).Substring(11);
                    SetChangedVaule(item, dtRow);
                }

                if (!dicitemListChanged.ContainsKey(dtRow["groupFlow"].ToString()))
                {
                    dicitemListChanged.Add(dtRow["groupFlow"].ToString(), dicitemList[dtRow["groupFlow"].ToString()]);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void SetChangedVaule(InCommonNoteItemEntity item, DataRow dtRow)
        {
            try
            {
                if (item == null || dtRow == null) return;
                if (item.DataElement == null && dataElementList.ContainsKey(item.CommonNote_Item_Flow))
                {
                    item.DataElement = dataElementList[item.CommonNote_Item_Flow];
                }
                string elementType = item.DataElement.ElementType;
                if (item.IsValidate == "否")  //不做类型校验时
                {
                    item.ValueXml = InCommonNoteItemEntity.ConvertStrToXml(dtRow[item.CommonNote_Item_Flow].ToString());
                }
                else if (elementType.ToUpper() == "S1"
                     || elementType.ToUpper() == "S4")
                {
                    item.ValueXml = InCommonNoteItemEntity.ConvertStrToXml(dtRow[item.CommonNote_Item_Flow].ToString());
                }
                else if (elementType.ToUpper() == "S2"
                   || elementType.ToUpper() == "S3")
                {
                    string dtrowcolvalue = dtRow[item.CommonNote_Item_Flow].ToString();
                    if (string.IsNullOrEmpty(dtrowcolvalue))
                    {
                        item.ValueXml = "";
                    }
                    else
                    {

                        BaseDictory baseDictory = new BaseDictory();
                        bool hasBase = false;
                        foreach (var itembase in item.DataElement.BaseOptionList)
                        {
                            if (itembase.Name == dtrowcolvalue)
                            {
                                baseDictory.Id = itembase.Id;
                                baseDictory.Name = itembase.Name;
                                hasBase = true;
                                break;
                            }
                        }
                        if (hasBase == true)
                        {
                            item.ValueXml = InCommonNoteItemEntity.ConvertBaseToXml(baseDictory);
                        }
                        else
                        {
                            item.ValueXml = InCommonNoteItemEntity.ConvertStrToXml(dtrowcolvalue);
                        }
                    }

                }
                else if (elementType.ToUpper() == "S9")
                {
                    string dtrowcolvalue = dtRow[item.CommonNote_Item_Flow].ToString();
                    if (string.IsNullOrEmpty(dtrowcolvalue))
                    {
                        item.ValueXml = "";
                    }
                    else
                    {
                        string[] strlist = dtrowcolvalue.Split(',');
                        List<BaseDictory> baseValueList = new List<BaseDictory>();
                        foreach (string itemstr in strlist)
                        {
                            foreach (var itembase in item.DataElement.BaseOptionList)
                            {
                                if (itembase.Id.Trim() == itemstr.Trim())
                                {
                                    BaseDictory baseDictory = new BaseDictory();
                                    baseDictory.Id = itembase.Id;
                                    baseDictory.Name = itembase.Name;
                                    baseValueList.Add(baseDictory);
                                    break;
                                }
                            }
                        }
                        item.ValueXml = InCommonNoteItemEntity.ConvertBaseToXml(baseValueList);

                    }
                }
                else if (elementType.ToUpper() == "DT" || elementType.ToUpper() == "D" || elementType.ToUpper() == "T")
                {
                    string dtrowcolvalue = dtRow[item.CommonNote_Item_Flow].ToString();
                    if (string.IsNullOrEmpty(dtrowcolvalue))
                    {
                        item.ValueXml = "";
                    }
                    else
                    {
                        if (elementType.ToUpper() == "DT")
                        {
                            item.ValueXml = InCommonNoteItemEntity.ConvertStrToXml(DateUtil.getDateTime(dtrowcolvalue, DateUtil.NORMAL_LONG));
                        }
                        else if (elementType.ToUpper() == "D")
                        {
                            item.ValueXml = InCommonNoteItemEntity.ConvertStrToXml(DateUtil.getDateTime(dtrowcolvalue, DateUtil.NORMAL_SHORT));
                        }
                        else if (elementType.ToUpper() == "T")
                        {
                            item.ValueXml = InCommonNoteItemEntity.ConvertStrToXml(DateUtil.getDateTime(dtrowcolvalue, DateUtil.NORMAL_LONG).Substring(11));
                        }
                    }
                }
                else if (elementType.ToUpper() == "N")
                {
                    item.ValueXml = InCommonNoteItemEntity.ConvertStrToXml(dtRow[item.CommonNote_Item_Flow].ToString());
                }
                else if (elementType.ToUpper() == "L")
                {
                    string dtrowcolvalue = dtRow[item.CommonNote_Item_Flow].ToString().ToUpper();
                    BaseDictory basedic = new BaseDictory();
                    if (dtrowcolvalue == "TRUE")
                    {
                        basedic.Id = "1";
                        basedic.Name = "是";
                    }
                    else
                    {
                        basedic.Id = "0";
                        basedic.Name = "否";
                    }
                    item.ValueXml = InCommonNoteItemEntity.ConvertBaseToXml(basedic);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void UCIncommonNoteTab_Load(object sender, EventArgs e)
        {
            try
            {
                GetDate(radToday);
                // BindData();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// 判断是否有值未保存
        /// </summary>
        /// <returns></returns>
        public bool HasSave()
        {
            if ((dicitemListChanged == null || dicitemListChanged.Count == 0)
                && (dicitemListDel == null || dicitemListDel.Count == 0))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Modify by xlb 2013-07-16
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControl1_DragEnter(object sender, DragEventArgs e)
        {
            //Point my_tempPoint = this.gridControl1.PointToClient(new Point(e.X, e.Y));

            //GridHitInfo my_tempHit = this.gridViewTab.CalcHitInfo(new Point(my_tempPoint.X, my_tempPoint.Y));

            //if (my_tempHit.RowHandle < 0) return;//放置在行内容位置  

            //string filename = my_tempHit.Column.FieldName;   //列绑定的列名
            //if (string.IsNullOrEmpty(filename)) return;
            //if (!dataElementList.Keys.Contains(filename)) return;
            //if (dataElementList[filename].ElementType != "S1" || dataElementList[filename].ElementType != "S4") return;
            //gridViewTab.FocusedColumn.AppearanceCell.BackColor = Color.FromArgb(105, 141, 189);
            if (e.Data.GetDataPresent(typeof(KeyValuePair<string, object>)))
            {
                KeyValuePair<string, object> keyvalue
                    = (KeyValuePair<string, object>)(e.Data.GetData(typeof(KeyValuePair<string, object>)));
                if (keyvalue.Value.ToString().ToUpper() == "TEXT")
                {
                    e.Effect = DragDropEffects.Copy;
                    //memoEditValue.Focus();
                    //memoEditValue.SelectionStart = memoEditValue.Text.Length;
                }
                else
                {

                    e.Effect = DragDropEffects.None;
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void gridControl1_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (!e.Data.GetDataPresent(typeof(KeyValuePair<string, object>))) return;
                KeyValuePair<string, object> keyvalue = (KeyValuePair<string, object>)(e.Data.GetData(typeof(KeyValuePair<string, object>)));
                if (keyvalue.Value == null || keyvalue.Value.ToString().ToUpper() != "TEXT") return;

                Point my_tempPoint = this.gridControl1.PointToClient(new Point(e.X, e.Y));
                if (my_tempPoint == null) return;
                GridHitInfo my_tempHit = this.gridViewTab.CalcHitInfo(new Point(my_tempPoint.X, my_tempPoint.Y));
                if (my_tempHit == null) return;
                if (my_tempHit.RowHandle < 0) return;//放置在行内容位置  
                if (my_tempHit.Column == null) return;
                string filename = my_tempHit.Column.FieldName;   //列绑定的列名
                if (string.IsNullOrEmpty(filename)) return;
                if (!dataElementList.Keys.Contains(filename)) return;
                if (dataElementList[filename].ElementType != "S1" && dataElementList[filename].ElementType != "S4") return;
                string strInsertText = keyvalue.Key.ToString();
                DataTable dataTable = gridControl1.DataSource as DataTable;
                //目标位置内容  
                DataRow my_targetDataRow = gridViewTab.GetDataRow(my_tempHit.RowHandle);
                InCommonNoteItemEntity inCommonnoteItem = dicitemList[my_targetDataRow["GROUPFLOW"].ToString()][0];
                if (inCommonnoteItem.RecordDoctorId != m_app.User.Id && !isNurseHeard)
                {
                    return;
                }
                my_targetDataRow[filename] = my_targetDataRow[filename].ToString() + strInsertText;
                SetValueToIncommonItem(my_targetDataRow);
                gridViewTab.BeginUpdate();
                gridControl1.DataSource = dataTable;
                gridViewTab.EndUpdate();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }







        }

        private void btnAddColName_Click(object sender, EventArgs e)
        {
            List<InCommonNoteItemEntity> inCommonNoteItemList = null;
            if (dicitemList == null || dicitemList.Count <= 0)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("单据没有内容，请先通过新增指定列名");
                return;
            }
            foreach (var item in dicitemList.Values)
            {
                inCommonNoteItemList = item;
                break;
            }
            List<InCommonNoteItemEntity> inCommonNoteItemListNoother = new List<InCommonNoteItemEntity>();
            CommonNoteBiz commonNoteBiz = new CommonNoteBiz(m_app);
            CommonNoteEntity commonNoteEntity = commonNoteBiz.GetDetailCommonNote(m_commonNote_TabEntity.CommonNoteFlow);
            CommonNote_TabEntity commonNote_TabEntity = commonNoteEntity.CommonNote_TabList.Find(a => a.CommonNote_Tab_Flow == m_commonNote_TabEntity.CommonNote_Tab_Flow);
            foreach (var citem in commonNote_TabEntity.CommonNote_ItemList)
            {
                if (citem.OtherName == null || citem.OtherName.Trim() == "")
                {
                    var incomItem = inCommonNoteItemList.Find(a => a.CommonNote_Item_Flow == citem.CommonNote_Item_Flow);
                    if (incomItem == null) continue;
                    inCommonNoteItemListNoother.Add(incomItem);
                }
            }

            //foreach (var inCommitem in inCommonNoteItemList)
            //{
            //    if (inCommitem.OtherName == null || inCommitem.OtherName.Trim() == "")
            //    {
            //        inCommonNoteItemListNoother.Add(inCommitem);
            //    }
            //}

            ZhiDingLieMing zhiDingLieMing = new ZhiDingLieMing(inCommonNoteItemListNoother);
            DialogResult diaResult = zhiDingLieMing.ShowDialog();
            if (diaResult == DialogResult.OK)
            {
                //foreach (var itemChange in inCommonNoteItemListNoother)
                //{
                //    if (itemChange.OtherName == null || string.IsNullOrEmpty(itemChange.OtherName.Trim())) continue;
                //    foreach (List<InCommonNoteItemEntity> itemList in dicitemList.Values)
                //    {
                //        foreach (var item in itemList)
                //        {
                //            if (item.CommonNote_Item_Flow == itemChange.CommonNote_Item_Flow)
                //            {
                //                item.OtherName = itemChange.OtherName;
                //                string message = "";
                //                inCommonNoteBiz.SaveIncommonNoteItem(item, ref message);
                //            }
                //        }
                //    }
                //}

                inCommonNoteBiz.updateIncommonType(inCommonNoteItemListNoother);
                BindData();
            }
            else
            {
                return;
            }

        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                bool hasMax = HasMaxRow(m_InCommonNoteTab.InCommonNote_Tab_Flow);
                if (hasMax)
                {
                    return;
                }

                string groupFlow = "";
                if (dicitemList == null || dicitemList.Count == 0)
                {
                    //AddCommonItemToInCommonItem(m_commonNote_TabEntity, m_InCommonNoteTab, m_app, ref groupFlow);
                    //bool hasZhidinglie = ZhiDingWeiZhiDingLie(m_InCommonNoteTab);
                    //if (!hasZhidinglie)
                    //{
                    //    m_InCommonNoteTab.InCommonNoteItemList = null;
                    //    return;
                    //}
                    //AddRowCount++;
                    //BindData();
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("无数据，无法追加");
                    return;
                }
                else
                {
                    List<InCommonNoteItemEntity> inCommonNoteItemList = new List<InCommonNoteItemEntity>();
                    foreach (var item in dicitemList)
                    {
                        inCommonNoteItemList = item.Value;
                        break;
                    }
                    DataRow dr = gridViewTab.GetDataRow(dicitemList.Count - 1);
                    DateTime datatime = Convert.ToDateTime(dr["jlsj"]);
                    List<InCommonNoteItemEntity> itemList;
                    if (datatime != null)
                    {
                        itemList = AddCommonItemToInCommonItem(m_InCommonNoteTab, inCommonNoteItemList, m_app, ref groupFlow, datatime);
                    }
                    else
                    {
                        itemList = AddCommonItemToInCommonItem(m_InCommonNoteTab, inCommonNoteItemList, m_app, ref groupFlow, DateTime.Now);
                    }
                    if (itemList == null) return;
                    AddRowCount++;
                    DataTable dataTable = gridControl1.DataSource as DataTable;
                    DicAddIncommitem(itemList);
                    AddDataTableRow(itemList, dataTable);
                }

                gridViewTab.MoveBy(dicitemList.Count - 1);
                gridViewTab.SelectRow(dicitemList.Count);
                SetValueToInCommonNoteItem(gridViewTab);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 鼠标事件
        /// Add by xlb 2013-03-11
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewTab_MouseUp(object sender, MouseEventArgs e)
        {
            //try
            //{
            //    if (!m_canEdit)
            //    {
            //        return;
            //    }
            //    barButtonItemEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

            //    if (e.Button == MouseButtons.Right)//判断是否右键
            //    {
            //        //当前位置
            //        GridHitInfo gridHit = gridViewTab.CalcHitInfo(e.Location);
            //        if (gridHit.RowHandle >= 0)
            //        {
            //            DataRowView dataRowView = gridViewTab.GetRow(gridHit.RowHandle) as DataRowView;
            //            if (gridHit.Column == null)
            //            {
            //                return;
            //            }
            //            //数据元中包含当前位置列名
            //            if (dataElementList.ContainsKey(gridHit.Column.FieldName))
            //            {
            //                //获取数据元
            //                DataElementEntity dataElementEntity = dataElementList[gridHit.Column.FieldName];
            //                //数据元类型是大文本则右键菜单可使用
            //                if (dataElementEntity.ElementType.ToUpper().Trim() == "S4")
            //                {
            //                    barButtonItemEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            //                    value = dataRowView[gridHit.Column.FieldName].ToString();
            //                }
            //            }
            //            //展示右键菜单
            //            popupMenu1.ShowPopup(gridControl1.PointToScreen(new Point(e.X, e.Y)));
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MyMessageBox.Show(1, ex);
            //}
        }

        /// <summary>
        /// 右键菜单编辑事件
        /// Add by xlb 2013-03-11
        /// Modify by xlb 2013-03-26
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                #region 注销 不要xlb 单据中存在多列大文本类型时更替该单元格内容 故使用全局变量记录鼠标所在位置单元格内容
                //DataRow dataRow = gridViewTab.GetDataRow(gridViewTab.FocusedRowHandle) as DataRow;
                //string value="";
                //for (int i = 0; i < gridViewTab.Columns.Count; i++)
                //{
                //    if (dataElementList.ContainsKey(gridViewTab.Columns[i].FieldName))
                //    {
                //        DataElementEntity dataElementEntity = dataElementList[gridViewTab.Columns[i].FieldName];
                //        if (dataElementEntity.ElementType.ToUpper().Trim() == "S4")
                //        {
                //             value = dataRow[gridViewTab.Columns[i].FieldName].ToString();
                //        }
                //    }
                //}
                #endregion
                //value保存当前鼠标所在位置单元格内容
                UcMedEdit ucMemoEdit = new UcMedEdit(value);
                if (ucMemoEdit == null)
                {
                    return;
                }
                ucMemoEdit.StartPosition = FormStartPosition.CenterScreen;
                ucMemoEdit.Show(this);
                //大文本编辑界面关闭后填充内容
                ucMemoEdit.FormClosed += delegate(object senders, FormClosedEventArgs cs)
                {
                    gridViewTab.SetFocusedValue(ucMemoEdit.LaterValue);
                };
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 编辑事件
        /// Add by xlb 2013-03-12
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                //焦点行
                DataRow dataRow = gridViewTab.GetFocusedDataRow();
                if (dataRow == null)
                {
                    MessageBox.Show("请选择需要编辑的行");
                    return;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void radToday_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GetDate(sender);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }


        private void GetDate(Object control)
        {
            if (dicitemListChanged != null && dicitemListChanged.Count > 0)
            {
                DialogResult dr = MessageBox.Show("存在未保存的数据，是否保存？", "提示", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    btnSave_Click(null, null);
                }
                dicitemListChanged = new Dictionary<string, List<InCommonNoteItemEntity>>();
                dicitemListDel = new Dictionary<string, List<InCommonNoteItemEntity>>();
            }


            WaitDialogForm waitDialog = new WaitDialogForm("正在获取数据……", "请稍等。");
            try
            {
                if (radToday.Checked == true && control == radToday)
                {
                    inCommonNoteBiz.GetDetaliInCommonNoteByDay(ref m_inCommonNote, ref m_InCommonNoteTab, DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"));
                    BindData();
                }
                else if (radAll.Checked == true && control == radAll)
                {

                    inCommonNoteBiz.GetDetaliInCommonNote(ref m_inCommonNote);
                    string tabflow = m_InCommonNoteTab.InCommonNote_Tab_Flow;
                    m_InCommonNoteTab = m_inCommonNote.InCommonNoteTabList.Find(a => a.InCommonNote_Tab_Flow == tabflow);
                    BindData();
                }
                else if (radFromDate.Checked == true && control == radFromDate)
                {
                    string startDate = dtStart.DateTime.ToString("yyyy-MM-dd");
                    string startEnd = dtEnd.DateTime.ToString("yyyy-MM-dd");
                    inCommonNoteBiz.GetDetaliInCommonNoteByDay(ref m_inCommonNote, ref m_InCommonNoteTab, startDate, startEnd);
                    BindData();
                }
                waitDialog.Hide();
                waitDialog.Close();
            }
            catch (Exception ex)
            {
                waitDialog.Hide();
                waitDialog.Close();
                throw ex;
            }
        }

        /// <summary>
        /// 获取该病人该单据的所有数据 供打印
        /// </summary>
        /// <returns></returns>
        public InCommonNoteEnmtity GetAllDetailDate()
        {
            try
            {
                // if (radAll.Checked == true)
                //  {
                //    return m_inCommonNote;
                // }
                // else
                //{
                InCommonNoteEnmtity inCommonNoteEnmtity = new CommonNoteUse.InCommonNoteEnmtity();
                inCommonNoteEnmtity.InCommonNoteFlow = m_inCommonNote.InCommonNoteFlow;
                inCommonNoteBiz.GetDetaliInCommonNote(ref inCommonNoteEnmtity);
                return inCommonNoteEnmtity;

                // }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void dtStart_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtStart.DateTime == null || dtEnd.DateTime == null)
                {
                    return;
                }
                GetDate(radFromDate);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void btnTongJi_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dataTable = gridControl1.DataSource as DataTable;
                if (dataTable == null || dataTable.Rows == null || dataTable.Rows.Count <= 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("界面没有数据，无法统计");
                    return;
                }
                TongJiForm tongJiForm = new TongJiForm(dataTable, m_commonNote_TabEntity.CommonNoteFlow, m_commonNoteCountEntity);
                tongJiForm.ShowDialog();
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void btnHis_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow dataRow = gridViewTab.GetFocusedDataRow();

                if (dataRow == null)
                {
                    m_app.CustomMessageBox.MessageShow("未选中记录");
                    return;
                }

                string groupflow = dataRow["groupFlow"].ToString();
                List<InCommonNoteItemEntity> inCommonNoteItemEntityList = InCommonNoteBiz.GetIncommHisInfo(groupflow);
                if (inCommonNoteItemEntityList == null || inCommonNoteItemEntityList.Count <= 0)
                {
                    m_app.CustomMessageBox.MessageShow("无修改历史");
                    return;
                }
                DataTable dt = InCommonNoteBiz.GetDateTable(inCommonNoteItemEntityList, m_commonNote_TabEntity.CommonNote_ItemList);
                if (dt == null || dt.Rows == null || dt.Rows.Count <= 1)
                {
                    m_app.CustomMessageBox.MessageShow("无修改历史");
                    return;
                }
                RecordHistoryForm recordHistoryForm = new RecordHistoryForm(dt, dataElementList);
                recordHistoryForm.Text = "护理记录修改历史查询（第" + (gridViewTab.FocusedRowHandle + 1) + "条记录）";
                recordHistoryForm.ShowDialog();
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void btnDelHis_Click(object sender, EventArgs e)
        {
            try
            {
                List<InCommonNoteItemEntity> inCommonNoteItemEntityList = InCommonNoteBiz.GetDelInCommonHisInfo(m_InCommonNoteTab.InCommonNote_Tab_Flow);
                if (inCommonNoteItemEntityList == null || inCommonNoteItemEntityList.Count <= 0)
                {
                    m_app.CustomMessageBox.MessageShow("无删除历史");
                    return;
                }
                DataTable dt = InCommonNoteBiz.GetDelDateTable(inCommonNoteItemEntityList, m_commonNote_TabEntity.CommonNote_ItemList);
                RecordHistoryForm recordHistoryForm = new RecordHistoryForm(dt, dataElementList);
                recordHistoryForm.Text = "护理记录删除历史查询";
                recordHistoryForm.ShowDialog();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
    }
}
