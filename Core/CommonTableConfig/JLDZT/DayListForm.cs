using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Core.CommonTableConfig.CommonNoteUse;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DrectSoft.Core.CommonTableConfig.JLDZT
{
    public partial class DayListForm : DevBaseForm
    {
        CommonNoteEntity m_CommonNoteEntity;
        CommonNoteBiz m_CommonNoteBiz;
        InCommonNoteBiz m_InCommonNoteBiz;
        IEmrHost m_app;

        WaitDialogForm m_WaitDialog;//等待框

        Dictionary<String, DataElementEntity> dataElementList;   //所涉及的数据元
        Dictionary<string, List<InCommonNoteItemEntity>> dicitemList;  //当天数据进行的整合
        List<InCommonNoteItemEntity> InCommonNoteItemEntityListAll;  //当天所有的数据
        Dictionary<string, List<InCommonNoteItemEntity>> dicitemListChanged = new Dictionary<string, List<InCommonNoteItemEntity>>();     //改变的数据
        Dictionary<string, List<InCommonNoteItemEntity>> dicitemListDel = new Dictionary<string, List<InCommonNoteItemEntity>>();  //删除的数据

        /// <summary>
        /// 构造方法
        /// edit by xlb 2013-02-01
        /// 加上了等待窗
        /// </summary>
        /// <param name="commonNoteEntity"></param>
        /// <param name="app"></param>
        public DayListForm(CommonNoteEntity commonNoteEntity, IEmrHost app)
        {
            m_WaitDialog = new WaitDialogForm("正在打开" + "" + commonNoteEntity.CommonNoteName + "", "请稍后...");
            this.m_CommonNoteEntity = commonNoteEntity;
            this.m_app = app;
            this.m_CommonNoteBiz = new CommonNoteBiz(this.m_app);
            this.m_InCommonNoteBiz = new InCommonNoteBiz(this.m_app);
            InitializeComponent();
            #region ----------------已注销 by xlb 2013.02.04--------
            if (m_CommonNoteEntity.CommonNote_TabList == null)
            {
                CommonNoteBiz commonNoteBiz = new DrectSoft.Core.CommonTableConfig.CommonNoteBiz(m_app);
                m_CommonNoteEntity = commonNoteBiz.GetDetailCommonNote(m_CommonNoteEntity.CommonNoteFlow);
            }
            #endregion
            dateEdit1.DateTime = DateTime.Now;
        }

        /// <summary>
        /// 序号列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDataElement_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            #region --------已注销 by xlb 2013.02.04------------
            //if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            //{
            //    e.Info.DisplayText = (e.RowHandle + 1).ToString();
            //}
            #endregion
            try
            {
                DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 从数据库获取数据 并展现数据。
        /// </summary>
        private void GetData()
        {
            try
            {
                //m_WaitDialog = new WaitDialogForm("正在加载数据", "请稍后...");
                DS_Common.SetWaitDialogCaption(m_WaitDialog, "正在加载数据");
                DateTime date = dateEdit1.DateTime;
                if (date == null)
                {
                    return;
                }
                string dateStr = date.ToString("yyyy-MM-dd");
                InCommonNoteItemEntityListAll = m_InCommonNoteBiz.GetIncommNoteItemByDay(m_CommonNoteEntity.CommonNoteFlow, dateStr);
                BindDate();
                //m_WaitDialog.Hide();
                DS_Common.HideWaitDialog(m_WaitDialog);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 展现数据
        /// </summary>
        private void BindDate()
        {
            dataElementList = new Dictionary<string, DataElementEntity>();
            dicitemList = new Dictionary<string, List<InCommonNoteItemEntity>>();
            if (m_CommonNoteEntity == null || m_CommonNoteEntity.CommonNote_TabList == null) return;
            if (m_CommonNoteEntity.CommonNote_TabList[0].ShowType == "单列")
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该单据的第一个表格不是所需表格格式，无法进行批量录入");
                return;
            }
            DataTable dt = m_InCommonNoteBiz.GetDateTable(m_CommonNoteEntity.CommonNote_TabList[0], out dataElementList);
            m_InCommonNoteBiz.SetValueToDataTable(dt, out dicitemList, InCommonNoteItemEntityListAll, dataElementList);
            InitGridControlForDateTable(dt);
            dt.DefaultView.Sort = "OUTBED ASC";
            //dt = dt.DefaultView.ToTable();
            gridControl1.DataSource = dt;
        }

        /// <summary>
        /// 根据DateTable生成GridView的列
        /// Modify by xlb 2013-05-30
        /// </summary>
        /// <param name="dt"></param>
        private void InitGridControlForDateTable(DataTable dt)
        {
            try
            {
                gridView1.Columns.Clear();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    GridColumn gridColumn = new DevExpress.XtraGrid.Columns.GridColumn();
                    gridColumn.FieldName = dt.Columns[i].ColumnName;
                    gridColumn.OptionsFilter.AllowAutoFilter = false;
                    gridColumn.OptionsFilter.AllowFilter = false;
                    gridColumn.Caption = dt.Columns[i].Caption;
                    gridColumn.OptionsColumn.AllowEdit = true;
                    gridColumn.Visible = true;
                    gridColumn.VisibleIndex = i;
                    gridColumn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    #region 列的一些特殊处理
                    if (dt.Columns[i].ColumnName == "groupFlow")
                    {
                        gridColumn.Visible = false;
                    }

                    if (dt.Columns[i].ColumnName == "jlsj")  //设置记录时间用的内容控件
                    {
                        RepositoryItemDateEdit repositoryItemDateEdit = new RepositoryItemDateEdit();
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
                        int width = 0;
                        foreach (GridColumn item in gridView1.Columns)
                        {
                            if (item.Visible == false) continue;
                            width += item.Width;
                        }
                        int jlrwidth = gridControl1.Width - width - gridView1.IndicatorWidth;
                        if (jlrwidth > 75)
                        {
                            gridColumn.Width = jlrwidth;
                        }
                        if (m_CommonNoteEntity.UsingPicSign == "1")
                        {
                            gridColumn.OptionsColumn.AllowEdit = false;
                        }
                        else
                        {
                            gridColumn.OptionsColumn.AllowEdit = true;
                        }
                    }

                    if (dt.Columns[i].ColumnName == "NAME"
                        || dt.Columns[i].ColumnName == "PATID"
                        || dt.Columns[i].ColumnName == "OUTBED")
                    {
                        gridColumn.AppearanceCell.Options.UseBackColor = true;
                        gridColumn.AppearanceCell.BackColor = Color.FromArgb(224, 224, 244); ;
                        gridColumn.OptionsColumn.AllowEdit = false;
                    }

                    #endregion

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
                    gridView1.Columns.Add(gridColumn);
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
                foreach (var item in m_CommonNoteEntity.CommonNote_TabList[0].CommonNote_ItemList)
                {
                    if (item.CommonNote_Item_Flow == commonNoteItemFlow)
                    {
                        return item.IsValidate;
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
                return repositoryItemSpinEdit;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        #endregion

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
                    if (dt.Columns.Count == 4)
                    {
                        gridColumn.Width = 500;
                    }
                    else
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

        private void gridViewTab_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                GridView gridView = (sender as GridView);
                SetValueToInCommonNoteItem(gridView);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void SetValueToInCommonNoteItem(GridView gridView)
        {
            DataRow dtRow = gridView.GetFocusedDataRow();
            if ((dtRow == null) || !dicitemList.ContainsKey(dtRow["groupFlow"].ToString()))
            {
                return;
            }
            List<InCommonNoteItemEntity> inCommonNoteItemList = dicitemList[dtRow["groupFlow"].ToString()];
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
                if (elementType.ToUpper() == "S1"
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                string messageAll = "";
                bool hasSave = true;

                //保存修改的数据
                foreach (List<InCommonNoteItemEntity> itemList in dicitemListChanged.Values)
                {
                    string hisrowflow = Guid.NewGuid().ToString();
                    foreach (var item in itemList)
                    {
                        string message = "";
                        bool itemresult = m_InCommonNoteBiz.SaveIncommonNoteItem(item, ref message);
                        if (!itemresult)
                        {
                            hasSave = false;
                            messageAll = message;
                        }
                        InCommonNoteBiz.AddInCommonColHistory(item, hisrowflow);
                    }
                    InCommonNoteBiz.AddInCommonRowHistory(itemList[0], hisrowflow);
                }
                if (hasSave == false)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("保存失败!" + messageAll, 1);
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
                //        bool itemresult = m_InCommonNoteBiz.SaveIncommonNoteItem(item, ref message);
                //        if (!itemresult)
                //        {
                //            hasSave = false;
                //            messageAll = message;
                //        }
                //    }
                //}

                if (hasSave == false)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("保存失败!" + messageAll, 1);
                }
                else
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("保存成功!", 1);
                    GetData();
                    txtSearch_EditValueChanged(null, null);
                    dicitemListChanged = new Dictionary<string, List<InCommonNoteItemEntity>>();
                    dicitemListDel = new Dictionary<string, List<InCommonNoteItemEntity>>();
                }
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.StackTrace);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                int index = gridView1.FocusedRowHandle;
                DataRow dataRow = gridView1.GetFocusedDataRow();
                if (dataRow == null)
                {
                    m_app.CustomMessageBox.MessageShow("未选中要删除的记录");
                    return;
                }
                DialogResult diaResult = m_app.CustomMessageBox.MessageShow("确定要删除选中数据吗？", DrectSoft.Core.CustomMessageBoxKind.QuestionYesNo);
                if (diaResult == DialogResult.No) return;
                string groupflow = dataRow["groupFlow"].ToString();
                List<InCommonNoteItemEntity> inCommonNoteItemEntityList = dicitemList[groupflow];

                if (m_CommonNoteEntity.UsingPicSign == "1" && inCommonNoteItemEntityList[0].RecordDoctorId != m_app.User.DoctorId)
                {
                    m_app.CustomMessageBox.MessageShow("只有记录者才可删除该记录");
                    return;
                }


                //如果改变的集合中有改数据 移除删除的数据
                if (dicitemListChanged.ContainsKey(groupflow))
                {
                    dicitemListChanged.Remove(groupflow);
                }

                //直接从数据库中删除
                string message = "";
                string hisrowflow = Guid.NewGuid().ToString();
                foreach (var item in inCommonNoteItemEntityList)
                {
                    if (string.IsNullOrEmpty(item.InCommonNote_Item_Flow)) continue;
                    item.Valide = "0";
                    bool itemresult = m_InCommonNoteBiz.SaveIncommonNoteItem(item, ref message);
                    InCommonNoteBiz.AddInCommonColHistory(item, hisrowflow);
                }

                InCommonNoteBiz.AddInCommonRowHistory(inCommonNoteItemEntityList[0], hisrowflow);


                //将删除的集合添加到删除的集合中
                //if (!dicitemListDel.ContainsKey(groupflow))
                //{
                //    dicitemListDel.Add(groupflow, inCommonNoteItemEntityList);
                //}

                //m_InCommonNoteTab中的项目中移除删除的项目
                foreach (InCommonNoteItemEntity item in inCommonNoteItemEntityList)
                {
                    InCommonNoteItemEntityListAll.Remove(item);
                }
                BindDate();
                txtSearch_EditValueChanged(null, null);
                gridView1.MoveBy(index);

            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.StackTrace);
            }
        }

        private void dateEdit1_DateTimeChanged(object sender, EventArgs e)
        {
            try
            {
                GetData();
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        /// edit by xlb
        /// 2013-01-29
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearch_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtSearch.Text.Contains("[") || txtSearch.Text.Contains("]") || txtSearch.Text.Contains("*") || txtSearch.Text.Contains("'"))
                {
                    throw new Exception("请不要输入特殊字符");
                }
                string filter = string.Format("name like '%{0}%' or patid like '%{0}%' or  outbed like '%{0}%'", txtSearch.Text);
                DataTable dt = gridControl1.DataSource as DataTable;
                if (dt != null)
                {
                    dt.DefaultView.RowFilter = filter;
                }

            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 批量录入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPLAdd_Click(object sender, EventArgs e)
        {
            try
            {
                AddFrom addFrom = new AddFrom(m_CommonNoteEntity, m_app);
                //addFrom.FormClosed += new FormClosedEventHandler(addFrom_FormClosed);
                addFrom.ShowDialog();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        /// 刷新事件
        /// xlb 2013-02-01
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                m_WaitDialog = new WaitDialogForm("正在刷新数据", "请稍后...");
                GetData();
                m_WaitDialog.Hide();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        #region 已注销 xlb 2013-02-01 批量新增界面关闭刷新数据带来的未响应现象，改为手动刷新
        ///// <summary>
        ///// 批量新增数据窗体关闭
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void addFrom_FormClosed(object sender, FormClosedEventArgs e)
        //{
        //    try
        //    {
        //        GetData();
        //    }
        //    catch (Exception ex)
        //    {

        //        MessageBox.Show(ex.Message + ex.StackTrace);
        //    }
        //}
        #endregion

        /// <summary>
        /// 列表选中行改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (m_CommonNoteEntity.UsingPicSign != "1")
                {
                    return;
                }
                DataRow dr = gridView1.GetFocusedDataRow();
                SetDataRowEdit(dr);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        /// 更具是否启用图片签名 对权限进行判断
        /// </summary>
        /// <param name="dr"></param>
        private void SetDataRowEdit(DataRow dr)
        {
            try
            {
                if (dr == null) return;
                List<InCommonNoteItemEntity> inCommonNoteItemList = dicitemList[dr["groupFlow"].ToString()];
                if (inCommonNoteItemList[0].RecordDoctorId == m_app.User.DoctorId)
                {
                    foreach (GridColumn item in gridView1.Columns)
                    {
                        if (item.FieldName == "jlr"
                            || item.FieldName == "NAME"
                            || item.FieldName == "PATID"
                            || item.FieldName == "OUTBED")
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
                    foreach (GridColumn item in gridView1.Columns)
                    {
                        item.OptionsColumn.AllowEdit = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}