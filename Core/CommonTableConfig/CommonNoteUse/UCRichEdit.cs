using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    public partial class UCRichEdit : DevExpress.XtraEditors.XtraUserControl
    {
        InCommonNoteItemEntity inCommonNoteItemEntity;
        public UCRichEdit(InCommonNoteItemEntity inCommonNoteItemEntity)
        {
            
            InitializeComponent();
            if (string.IsNullOrEmpty(this.inCommonNoteItemEntity.OtherName))
            {
                lblName.Text = "未指定列：";
            }
            else
            {
                lblName.Text = this.inCommonNoteItemEntity.OtherName + "：";
            }
        }
    }
}
