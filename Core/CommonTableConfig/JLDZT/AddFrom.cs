using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
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
    /// <summary>
    /// 批量录入新增数据界面
    /// </summary>
    public partial class AddFrom : DevBaseForm
    {


        CommonNoteEntity m_CommonNoteEntity;
        CommonNoteBiz m_CommonNoteBiz;
        InCommonNoteBiz m_InCommonNoteBiz;
        IEmrHost m_app;
        Dictionary<String, DataElementEntity> dataElementList;   //所涉及的数据元
        Dictionary<string, List<InCommonNoteItemEntity>> dicitemList;  //当天数据进行的整合
        DataTable dtInpatient; //科室病区病人
        public AddFrom(CommonNoteEntity commonNoteEntity, IEmrHost app)
        {
            this.m_CommonNoteEntity = commonNoteEntity;
            this.m_app = app;
            this.m_CommonNoteBiz = new CommonNoteBiz(this.m_app);
            this.m_InCommonNoteBiz = new InCommonNoteBiz(this.m_app);
            InitializeComponent();
            #region ---已注销 by xlb 2013.02.04--------------------
            //if (m_CommonNoteEntity.CommonNote_TabList == null)
            //{
            //    CommonNoteBiz commonNoteBiz = new DrectSoft.Core.CommonTableConfig.CommonNoteBiz(m_app);
            //    m_CommonNoteEntity = commonNoteBiz.GetDetailCommonNote(m_CommonNoteEntity.CommonNoteFlow);
            //}
            #endregion
            dateTimeAdd.DateTime = DateTime.Now;
        }

        /// <summary>
        /// 1、获取病人
        /// 2、根据病人生成批量的datatable
        /// </summary>
        private void GetDateTable()
        {
            WaitDialogForm waitForm = new WaitDialogForm("正在生成病人列表...", "请稍等");
            try
            {
                if (dateTimeAdd.DateTime == null)
                {
                    return;
                }
                dataElementList = new Dictionary<string, DataElementEntity>();
                dicitemList = new Dictionary<string, List<InCommonNoteItemEntity>>();
                if (dtInpatient == null)
                {
                    dtInpatient = m_InCommonNoteBiz.GetInPatientByDepart();
                }
                DataTable dtPL = m_InCommonNoteBiz.GetDateTable(m_CommonNoteEntity.CommonNote_TabList[0], out dataElementList);
                DataColumn dc = new DataColumn("check", typeof(bool));
                dc.Caption = "选择";
                dtPL.Columns.Add(dc);
                dtPL.Columns["check"].SetOrdinal(1);
                m_InCommonNoteBiz.GetDataTablePLSetValue(dtInpatient, dtPL, m_CommonNoteEntity, dateTimeAdd.DateTime, dataElementList, dicitemList);
                InitGridControlForDateTable(dtPL);
                gridControl1.DataSource = dtPL;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                waitForm.Close();
            }
        }


        /// <summary>
        /// 根据DateTable甚至GridView的列
        /// </summary>
        /// <param name="dt"></param>
        private void InitGridControlForDateTable(DataTable dt)
        {
            try
            {
                gridView1.Columns.Clear();
                //GridColumn gridCheck = new GridColumn();
                //gridCheck.OptionsFilter.AllowAutoFilter = false;
                //gridCheck.OptionsFilter.AllowFilter = false;
                //gridCheck.Caption = "选择";
                //gridCheck.Width = 40;

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

                    #region 列的一些特殊处理

                    if (dt.Columns[i].ColumnName == "NAME"
                        || dt.Columns[i].ColumnName == "PATID"
                        || dt.Columns[i].ColumnName == "OUTBED")
                    {
                        gridColumn.OptionsColumn.AllowEdit = false;

                        gridColumn.AppearanceCell.BackColor = Color.FromArgb(224, 224, 244);
                        gridColumn.AppearanceCell.Options.UseBackColor = true;
                    }

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
                    if (dt.Columns[i].ColumnName == "check") //选择
                    {
                        RepositoryItemCheckEdit repositoryItemCheckEdit = new RepositoryItemCheckEdit();
                        gridColumn.ColumnEdit = repositoryItemCheckEdit;
                        gridColumn.Width = 40;
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
                        gridColumn.OptionsColumn.AllowEdit = m_CommonNoteEntity.UsingPicSign == "1" ? false : true;
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
                if (e.Column.FieldName == "check")
                {
                    //DataTable dt=  gridControl1.DataSource as DataTable;
                    //dt.DefaultView.Sort = "check DESC";
                    // gridControl1.DataSource = dt;
                    SetRowEdit();
                }
                else
                {
                    GridView gridView = (sender as GridView);
                    SetValueToInCommonNoteItem(gridView);
                }
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

        /// <summary>
        /// 序号列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDataElement_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        /// <summary>
        /// 保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            WaitDialogForm waitForm = new WaitDialogForm("正在批量保存数据...", "请稍等");
            try
            {
                string messageAll = "";
                bool hasSave = true;
                DataTable dataTablePL = gridControl1.DataSource as DataTable;
                if (dataTablePL == null || dataTablePL.Rows.Count <= 0)
                {
                    return;
                }
                foreach (DataRow item in dataTablePL.Rows)
                {
                    if ((bool)item["check"] != true)
                    {
                        continue;
                    }
                    string hisrowflow = Guid.NewGuid().ToString();
                    string groupflow = item["groupflow"].ToString();
                    foreach (var inItem in dicitemList[groupflow])
                    {
                        string message = "";
                        bool itemresult = m_InCommonNoteBiz.SaveIncommonNoteItem(inItem, ref message);
                        if (!itemresult)
                        {
                            hasSave = false;
                            messageAll = message;
                        }
                        InCommonNoteBiz.AddInCommonColHistory(inItem, hisrowflow);
                    }
                    InCommonNoteBiz.AddInCommonRowHistory(dicitemList[groupflow][0], hisrowflow);
                }
                if (hasSave == false)
                {
                    MyMessageBox.Show("保存失败!" + messageAll, 1);
                }
                else
                {
                    MyMessageBox.Show("保存成功!", 1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
            finally
            {
                waitForm.Close();
            }
        }

        private void dateTimeAdd_DateTimeChanged(object sender, EventArgs e)
        {
            try
            {
                GetDateTable();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void SetGridColumnEdit(bool isEdit)
        {
            if (isEdit == true)
            {

                foreach (GridColumn item in gridView1.Columns)
                {
                    if (item.FieldName.ToUpper() == "NAME"
                    || item.FieldName.ToUpper() == "PATID"
                    || item.FieldName.ToUpper() == "OUTBED")
                    {
                        item.OptionsColumn.AllowEdit = false;
                    }
                    else if (item.FieldName.ToUpper() == "JLR")
                    {
                        item.OptionsColumn.AllowEdit = m_CommonNoteEntity.UsingPicSign == "1" ? false : true;
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
                    if (item.FieldName.ToUpper() == "CHECK")
                    {
                        item.OptionsColumn.AllowEdit = true;
                    }
                    else
                    {
                        item.OptionsColumn.AllowEdit = false;
                    }
                }
            }
        }


        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {


                SetGridColumnEdit(checkEdit1.Checked);
                DataTable dataTablePL = gridControl1.DataSource as DataTable;
                if (dataTablePL == null) return;
                foreach (DataRow item in dataTablePL.Rows)
                {
                    item["check"] = checkEdit1.Checked;
                }
                gridControl1.DataSource = dataTablePL;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            SetRowEdit();
        }

        private void SetRowEdit()
        {
            try
            {
                DataRow dataRow = gridView1.GetFocusedDataRow();
                if (dataRow == null) return;
                SetGridColumnEdit((bool)dataRow["check"]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void AddFrom_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                //清理生成的空表单 2013-05-17
                foreach (List<InCommonNoteItemEntity> item in dicitemList.Values)
                {
                    if (item.Count <= 0) continue;
                    string sql = string.Format(@"select count(*) from incommonnote_row r where r.incommonnoteflow='{0}' and r.valide='1'", item[0].InCommonNoteFlow);
                    DataTable dt = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sql, CommandType.Text);
                    if (dt == null || dt.Rows == null || dt.Rows.Count <= 0)
                    {
                        continue;
                    }
                    if (Convert.ToInt32(dt.Rows[0][0]) <= 0)
                    {
                        string updateSql = string.Format(@"update incommonnote i set i.valide='0'where i.incommonnoteflow='{0}'", item[0].InCommonNoteFlow);
                        DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteNonQuery(updateSql, CommandType.Text);
                    }
                }
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }


        }


    }
}