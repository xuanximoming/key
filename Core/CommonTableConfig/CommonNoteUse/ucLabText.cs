using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Common;

namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    public partial class ucLabText : DevExpress.XtraEditors.XtraUserControl
    {
        public InCommonNoteItemEntity inCommonNoteItemEntity;
        Point point = new Point(90, 13);
        public ucLabText(InCommonNoteItemEntity inCommonNoteItemEntity)
        {
            this.inCommonNoteItemEntity = inCommonNoteItemEntity;
            InitializeComponent();
            InitDate();
        }

        //加载数据
        public void InitDate(InCommonNoteItemEntity inCommonNoteItemEntity, IEmrHost app)
        {
            this.inCommonNoteItemEntity = inCommonNoteItemEntity;
            InitDate();
        }

        private void InitDate()
        {
            SetControlShow();
            if (string.IsNullOrEmpty(this.inCommonNoteItemEntity.OtherName))
            {
                this.inCommonNoteItemEntity.OtherName = "未指定列";
            }

            lblName.Text = this.inCommonNoteItemEntity.OtherName + "：";
        }

        /// <summary>
        /// 保存项目 直接在对象的xml之中
        /// </summary>
        public bool GetInCommonNoteItemSave(ref string message)
        {
            if (this.inCommonNoteItemEntity.IsValidate == "否")
            {
                this.inCommonNoteItemEntity.ValueXml = InCommonNoteItemEntity.ConvertStrToXml(txtValue.Text);
            }
            else
            {
                string elementType = this.inCommonNoteItemEntity.DataElement.ElementType;
                switch (elementType)
                {
                    case "S1":
                        this.inCommonNoteItemEntity.ValueXml = InCommonNoteItemEntity.ConvertStrToXml(txtValue.Text);
                        break;
                    case "S2":
                        string strText = cboS2S3.Text;
                        this.inCommonNoteItemEntity.ValueXml = InCommonNoteItemEntity.ConvertStrToXml(strText);
                        //选中时赋值的
                        break;
                    case "S3":
                        string strTextS3 = cboS2S3.Text;
                        this.inCommonNoteItemEntity.ValueXml = InCommonNoteItemEntity.ConvertStrToXml(strTextS3);
                        //选中时赋值的
                        break;
                    case "S4":
                        this.inCommonNoteItemEntity.ValueXml = InCommonNoteItemEntity.ConvertStrToXml(txtValue.Text);
                        break;
                    case "S9":
                        List<BaseDictory> baseDictoryList = new List<BaseDictory>();
                        foreach (CheckedListBoxItem item in chkListValue.Items)
                        {
                            if (item.CheckState == CheckState.Checked)
                            {
                                baseDictoryList.Add(item.Value as BaseDictory);
                            }
                        }
                        this.inCommonNoteItemEntity.BaseValueList = baseDictoryList;
                        this.inCommonNoteItemEntity.ValueXml = InCommonNoteItemEntity.ConvertBaseToXml(baseDictoryList);
                        //选中时赋值的
                        break;
                    case "L":
                        BaseDictory basedicL = new BaseDictory();
                        basedicL.Id = chboxValue.Checked == true ? "1" : "0";
                        basedicL.Name = chboxValue.Checked == true ? "是" : "否";
                        this.inCommonNoteItemEntity.ValueXml = InCommonNoteItemEntity.ConvertBaseToXml(basedicL);
                        break;
                    case "N":
                        this.inCommonNoteItemEntity.ValueXml = InCommonNoteItemEntity.ConvertStrToXml(spinEditValue.Value.ToString());
                        break;
                    case "D":
                        string dateV10 = DateUtil.getDateTime(dateEditValue.DateTime.ToString(), DateUtil.NORMAL_SHORT);
                        if (dateV10 == "0001-01-01")
                            dateV10 = "";
                        this.inCommonNoteItemEntity.ValueXml = InCommonNoteItemEntity.ConvertStrToXml(dateV10);
                        break;
                    case "DT":
                        string dateV19 = DateUtil.getDateTime(dateEditValue.DateTime.ToString(), DateUtil.NORMAL_LONG);
                        if (dateV19 == "0001-01-01 00:00:00")
                            dateV19 = "";
                        this.inCommonNoteItemEntity.ValueXml = InCommonNoteItemEntity.ConvertStrToXml(dateV19);
                        break;
                    case "T":
                        string dateV8 = DateUtil.getDateTime(timeEditValue.Time.ToString(), DateUtil.NORMAL_LONG).Substring(11, 8);
                        if (dateV8 == "00:00:00")
                            dateV8 = "";
                        this.inCommonNoteItemEntity.ValueXml = InCommonNoteItemEntity.ConvertStrToXml(dateV8);
                        break;
                    default:
                        this.inCommonNoteItemEntity.ValueXml = InCommonNoteItemEntity.ConvertStrToXml(txtValue.Text);
                        break;
                }
            }
            return true;
        }

        /// <summary>
        /// 设置显示控件
        /// 
        /// </summary>
        private void SetControlShow()
        {
            SetControlVisible();
            if (this.inCommonNoteItemEntity.IsValidate == "否")
            {
                txtValue.Visible = true;
                SetControlLocation(txtValue);
                txtValue.Text = this.inCommonNoteItemEntity.ValuesShow;
                return;
            }
            string elementType = this.inCommonNoteItemEntity.DataElement.ElementType;
            switch (elementType)
            {
                case "S1":
                    SetControlS1S4();
                    break;
                case "S2":
                    cboS2S3.Visible = true;
                    SetControlLocation(cboS2S3);
                    SetControlPopOne();
                    break;
                case "S3":
                     cboS2S3.Visible = true;
                    SetControlLocation(cboS2S3);
                    SetControlPopOne();
                    break;
                case "S4":
                    SetControlS1S4();
                    picS4.Visible = true;
                    break;
                case "S9":
                    popupEditMoreValue.Visible = true;
                    SetControlLocation(popupEditMoreValue);
                    SetControlPopMore();
                    break;
                case "L":
                    chboxValue.Visible = true;
                    SetControlLocation(chboxValue);
                    if (this.inCommonNoteItemEntity.BaseValueList != null
                        && this.inCommonNoteItemEntity.BaseValueList.Count > 0)
                        chboxValue.Checked = this.inCommonNoteItemEntity.BaseValueList[0].Id == "0" ? false : true;
                    break;
                case "N":
                    spinEditValue.Visible = true;
                    SetControlLocation(spinEditValue);

                    if (string.IsNullOrEmpty(this.inCommonNoteItemEntity.InCommonNote_Item_Flow))
                    {
                        Dictionary<string, string> dicList = DataElementEntity.GetMaxMinDefStr(this.inCommonNoteItemEntity.DataElement.ElementRange);
                        if (dicList != null && dicList.ContainsKey("MaxValue"))
                        {
                            decimal decMax;
                            bool isdecMax = decimal.TryParse(dicList["MaxValue"], out decMax);
                            if (isdecMax)
                            {
                                spinEditValue.Properties.MaxValue = decMax;
                            }
                        }

                        if (dicList != null && dicList.ContainsKey("MinValue"))
                        {
                            decimal decMin;
                            bool isdecMin = decimal.TryParse(dicList["MinValue"], out decMin);
                            if (isdecMin)
                            {
                                spinEditValue.Properties.MinValue = decMin;
                            }
                        }
                        if (dicList != null && dicList.ContainsKey("DefaultValue"))
                        {
                            decimal defValue;
                            bool isdefValue = decimal.TryParse(dicList["DefaultValue"], out defValue);
                            if (isdefValue)
                            {
                                spinEditValue.Value = defValue;
                            }
                        }
                    }
                    else
                    {
                        decimal num;
                        bool isfloat = decimal.TryParse(this.inCommonNoteItemEntity.ValuesShow, out num);
                        if (isfloat)
                        {
                            spinEditValue.Value = num;
                        }
                    }
                    break;
                case "D":
                    dateEditValue.Visible = true;
                    dateEditValue.EditValue = "";
                    dateEditValue.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.False;
                    dateEditValue.Properties.VistaEditTime = DevExpress.Utils.DefaultBoolean.False;
                    dateEditValue.Properties.DisplayFormat.FormatString = "d";
                    dateEditValue.Properties.EditFormat.FormatString = "d";
                    dateEditValue.Properties.EditMask = "d";

                    SetControlLocation(dateEditValue);
                    if (!string.IsNullOrEmpty(this.inCommonNoteItemEntity.ValuesShow))
                    {
                        dateEditValue.DateTime = DateUtil.getDateTime(this.inCommonNoteItemEntity.ValuesShow);
                    }
                    break;
                case "DT":
                    dateEditValue.Visible = true;
                    dateEditValue.EditValue = "";
                    dateEditValue.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.True;
                    dateEditValue.Properties.VistaEditTime = DevExpress.Utils.DefaultBoolean.True;
                    dateEditValue.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
                    dateEditValue.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
                    dateEditValue.Properties.EditMask = "yyyy-MM-dd HH:mm:ss";
                    SetControlLocation(dateEditValue);
                    if (!string.IsNullOrEmpty(this.inCommonNoteItemEntity.ValuesShow))
                    {
                        dateEditValue.DateTime = DateUtil.getDateTime(this.inCommonNoteItemEntity.ValuesShow);
                    }
                    break;
                case "T":
                    timeEditValue.Visible = true;
                    timeEditValue.EditValue = "";
                    SetControlLocation(timeEditValue);
                    if (!string.IsNullOrEmpty(this.inCommonNoteItemEntity.ValuesShow))
                        timeEditValue.Time = DateUtil.getDateTime(this.inCommonNoteItemEntity.ValuesShow);
                    break;
                default:
                    txtValue.Visible = true;
                    SetControlLocation(txtValue);
                    txtValue.Text = this.inCommonNoteItemEntity.ValuesShow;
                    break;
            }
        }

        /// <summary>
        /// S1类型和S4类型的设置
        /// </summary>
        private void SetControlS1S4()
        {
            txtValue.Visible = true;
            SetControlLocation(txtValue);
            if (string.IsNullOrEmpty(this.inCommonNoteItemEntity.InCommonNote_Item_Flow))
            {
                Dictionary<string, string> dicList = DataElementEntity.GetMaxMinDefStr(this.inCommonNoteItemEntity.DataElement.ElementRange);
                if (dicList != null && dicList.ContainsKey("DefaultValue"))
                {
                    txtValue.Text = dicList["DefaultValue"];
                }
            }
            else
            {
                txtValue.Text = this.inCommonNoteItemEntity.ValuesShow;
            }
        }

        //设置单选控件
        private void SetControlPopOne()
        {
            List<BaseDictory> baseDic = this.inCommonNoteItemEntity.DataElement.BaseOptionList;
            cboS2S3.Properties.Items.Add("");
            foreach (var item in baseDic)
            {
                  cboS2S3.Properties.Items.Add(item.Name);
            }
            if (this.inCommonNoteItemEntity.BaseValueList != null && this.inCommonNoteItemEntity.BaseValueList.Count > 0)
            {
                foreach (var item in baseDic)
                {
                    cboS2S3.Text = item.Name;
                }
            }
            else
            {
                //如果没有选择 选择默认值
                List<BaseDictory> baseDefault = DataElementEntity.GetDefaultOption(this.inCommonNoteItemEntity.DataElement.ElementRange);
                if (baseDefault != null && baseDefault.Count > 0)
                {
                    foreach (var item in baseDic)
                    {
                        if (item.Name == baseDefault[0].Name)
                        {
                            cboS2S3.Text = item.Name;
                            break;
                        }
                    }
                }
            }
            //popupEditOneValue.EditValue = this.inCommonNoteItemEntity.ValuesShow;
        }


        //设置多选控件 并对存在值进行赋值
        private void SetControlPopMore()
        {
            List<BaseDictory> baseDic = this.inCommonNoteItemEntity.DataElement.BaseOptionList;
            chkListValue.Items.Clear();
            foreach (var item in baseDic)
            {
                CheckedListBoxItem checkedListBoxItem = new CheckedListBoxItem(item, item.Name);
                chkListValue.Items.Add(checkedListBoxItem);
                if (this.inCommonNoteItemEntity.BaseValueList != null && this.inCommonNoteItemEntity.BaseValueList.Count > 0)
                {
                    foreach (var itemchk in this.inCommonNoteItemEntity.BaseValueList)
                    {
                        if (itemchk.Id == item.Id)
                        {
                            checkedListBoxItem.CheckState = CheckState.Checked;
                            break;
                        }
                    }
                }
                else
                {
                    //如果没有选择 选择默认值
                    List<BaseDictory> baseDefault = DataElementEntity.GetDefaultOption(this.inCommonNoteItemEntity.DataElement.ElementRange);
                    foreach (var itemdef in baseDefault)
                    {
                        if (itemdef.Id == item.Id)
                        {
                            checkedListBoxItem.CheckState = CheckState.Checked;
                            break;
                        }
                    }
                }
            }

            //popupEditMoreValue.Text = this.inCommonNoteItemEntity.ValuesShow;
        }

        private void SetControlLocation(Control control)
        {
            control.Location = point;
        }


        private void SetControlVisible()
        {
            txtValue.Visible = false;
            txtValue.Text = "";
            dateEditValue.Visible = false;
            dateEditValue.DateTime = DateTime.MinValue;
            timeEditValue.Visible = false;
            timeEditValue.Time = DateTime.MinValue;
            cboS2S3.Visible = false;
            cboS2S3.Text = "";
            popupEditMoreValue.Visible = false;
            popupEditMoreValue.EditValue = null;
            chboxValue.Visible = false;
            chboxValue.Checked = false;
            spinEditValue.Visible = false;
            spinEditValue.Value = 0;
            picS4.Visible = false;
        }

        /// <summary>
        /// 单数据选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxValue_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        /// <summary>
        /// 多数据选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkListValue_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            string valueList = "";
            foreach (CheckedListBoxItem item in chkListValue.Items)
            {
                if (item.CheckState == CheckState.Checked)
                {
                    valueList += (item.Value as BaseDictory).Name + ";";
                }
            }
            popupEditMoreValue.EditValue = valueList;
        }


        void ucMedEdit_FormClosed(object sender, FormClosedEventArgs e)
        {
            hasOpen = false;
            //txtValue.Text = (sender as UcMedEdit).GetValue();
        }

        private void txtValue_DragDrop(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(typeof(KeyValuePair<string, object>)))
            {
                KeyValuePair<string, object> keyvalue = (KeyValuePair<string, object>)(e.Data.GetData(typeof(KeyValuePair<string, object>)));
                if (keyvalue.Value.ToString().ToUpper() != "TEXT") return;
                string strInsertText = keyvalue.Key.ToString();
                int start = this.txtValue.SelectionStart;
                this.txtValue.Text = this.txtValue.Text.Insert(start, strInsertText);
                this.txtValue.SelectionStart = this.txtValue.Text.Length;
            }
        }

        private void txtValue_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(KeyValuePair<string, object>)))
            {
                KeyValuePair<string, object> keyvalue
                    = (KeyValuePair<string, object>)(e.Data.GetData(typeof(KeyValuePair<string, object>)));
                if (keyvalue.Value.ToString().ToUpper() == "TEXT")
                {
                    e.Effect = DragDropEffects.Copy;
                    txtValue.Focus();
                    txtValue.SelectionStart = txtValue.Text.Length;
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



        bool hasOpen = false;  //控制只显示一个大文本框
        private void OpenMedEdit()
        {

            if (inCommonNoteItemEntity.DataElement == null || inCommonNoteItemEntity.DataElement.ElementType != "S4")
            { return; }
            if (hasOpen == true) return;
            UcMedEdit ucMedEdit = new UcMedEdit(txtValue.Text);
            ucMedEdit.Text = "编辑" + inCommonNoteItemEntity.OtherName;
            ucMedEdit.FormClosed += new FormClosedEventHandler(ucMedEdit_FormClosed);
            ucMedEdit.btnOk.Click += delegate(object sender, EventArgs e)
            {
                hasOpen = false;
                txtValue.Text = ucMedEdit.GetValue();
                ucMedEdit.Close();
            };
            int iActulaWidth = Screen.PrimaryScreen.Bounds.Width;
            int iActulaHeight = Screen.PrimaryScreen.Bounds.Height;
            ucMedEdit.StartPosition = FormStartPosition.Manual;
            ucMedEdit.Location = new Point((iActulaWidth - ucMedEdit.Width) / 2, iActulaHeight - 250);
            ucMedEdit.Show(this);
            hasOpen = true;
        }


        private void picS4_Click_1(object sender, EventArgs e)
        {
            OpenMedEdit();
        }


    }
}
