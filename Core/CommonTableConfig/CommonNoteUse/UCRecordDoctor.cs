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
    public partial class UCRecordDoctor : DevExpress.XtraEditors.XtraUserControl
    {
        public UCRecordDoctor(string recorderDoctor)
        {
            InitializeComponent();
            InitDate(recorderDoctor);

        }
        public void InitDate(string recorderDoctor)
        {
            txtRecordDoctor.Text = recorderDoctor;
        }
        /// <summary>
        /// 获取记录人姓名
        /// </summary>
        /// <returns></returns>
        public string GetDoc()
        {
            return txtRecordDoctor.Text;
        }
    }
}
