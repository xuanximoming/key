using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace DrectSoft.Core.ZymosisReport.UCControls
{
    public partial class UCHuJiAddress : DevExpress.XtraEditors.XtraUserControl,IZymosisReport
    {
        /// <summary>
        /// 户籍地址 联合下拉框
        /// </summary>
        public UCHuJiAddress()
        {
            InitializeComponent();
        }




        #region IZymosisReport 成员

        public void InitValue(string strValues)
        {
            throw new NotImplementedException();
        }

        public string GetValue()
        {
            throw new NotImplementedException();
        }

        public void SetFocus()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
