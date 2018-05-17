using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    /// <summary>	
    /// <title>对违为指定列名进行指定</title>
    /// <auth>xuliangliang</auth>
    /// <date></date>
    /// </summary>
    public partial class ZhiDingLieMing : DevBaseForm
    {
        List<InCommonNoteItemEntity> m_inCommonNoteItemList;
        public ZhiDingLieMing(List<InCommonNoteItemEntity> inCommonNoteItemList)
        {
            InitializeComponent();
            m_inCommonNoteItemList = inCommonNoteItemList;
            InitAll();
        }

        private void InitAll()
        {
            scrolLable.Controls.Clear();
            for (int i = 0; i < m_inCommonNoteItemList.Count; i++)
            {
                UClableText uClableText = new UClableText(m_inCommonNoteItemList[i]);
                uClableText.Location = new Point(0, 0 + i * 32);
                if (i == 0)
                {
                    uClableText.Focus();
                }
                scrolLable.Controls.Add(uClableText);
            }
            this.Height = 32 * m_inCommonNoteItemList.Count + 80;

        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //无需校验
            //foreach (var item in scrolLable.Controls)
            //{
            //    if (item is UClableText)
            //    {
            //      bool isValidate=  (item as UClableText).Validate();
            //      if (!isValidate)
            //      {
            //          Common.Ctrs.DLG.MessageBox.Show("有未指定的列名");
            //          return;
            //      }
            //    }
            //}
            foreach (var item in scrolLable.Controls)
            {
                if (item is UClableText)
                {
                    (item as UClableText).GetOtherName();
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}