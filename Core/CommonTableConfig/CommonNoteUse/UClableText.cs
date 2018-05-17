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
    public partial class UClableText : DevExpress.XtraEditors.XtraUserControl
    {
        InCommonNoteItemEntity m_inCommonNoteItem;
        public UClableText(InCommonNoteItemEntity inCommonNoteItem) 
        {
            InitializeComponent();
            m_inCommonNoteItem = inCommonNoteItem;
            label1.Text = m_inCommonNoteItem.DataElementName+"：";
            textEdit1.Text = inCommonNoteItem.OtherName;
        }

        /// <summary>
        /// 保存该值
        /// </summary>
        public void GetOtherName()
        {
           m_inCommonNoteItem.OtherName= textEdit1.Text.Trim();
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            if (string.IsNullOrEmpty(textEdit1.Text))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
