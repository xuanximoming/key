using DrectSoft.Common.Ctrs.FORM;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    /// <summary>
    /// 护理单据选中编辑界面
    /// Add by xlb 2013-03-12
    /// </summary>
    public partial class RowEditForm : DevBaseForm
    {
        List<InCommonNoteItemEntity> myInCommonNoteItemEntityList;
        public RowEditForm(List<InCommonNoteItemEntity> inCommonNoteItemEntityList)
        {
            try
            {
                this.myInCommonNoteItemEntityList = inCommonNoteItemEntityList;
                InitializeComponent();
                InitControl();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化控件并展示数据
        /// </summary>
        private void InitControl()
        {
            UCRecordDateTime ucRecordDateTime = new CommonNoteUse.UCRecordDateTime(myInCommonNoteItemEntityList[0].RecordDate, myInCommonNoteItemEntityList[0].RecordTime);
            flpRowEdit.Controls.Add(ucRecordDateTime);
            for (int i = 0; i < myInCommonNoteItemEntityList.Count; i++)
            {
                var item = myInCommonNoteItemEntityList[i];
                if (item.IsValidate == "是" && item.DataElement.ElementType == "S4")
                {
                    UCRichEdit uCRichEdit = new UCRichEdit(item);
                    flpRowEdit.Controls.Add(uCRichEdit);
                }
                else
                {
                    ucLabText mlabText = new ucLabText(item);
                    flpRowEdit.Controls.Add(mlabText);
                }
            }

            UCRecordDoctor UCRecordDoctor = new UCRecordDoctor(myInCommonNoteItemEntityList[0].RecordDoctorName);
            flpRowEdit.Controls.Add(ucRecordDateTime);
        }

        /// <summary>
        /// 窗体重绘事件
        /// Add by xlb 2013-03-12
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
    }
}
