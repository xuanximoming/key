using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Core.MainEmrPad
{
    public partial class RowsInsert : DevBaseForm
    {
        public RowsInsert()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region 公开窗口属性
        /// <summary>
        /// 要插入的行数
        /// </summary>
        public int RowNum
        {
            get
            {
                return Convert.ToInt32(this.nudNumber.Value);
            }
        }
        /// <summary>
        /// 插入行的位置是否在插入点的后面
        /// </summary>
        public bool IsBack
        {
            get
            {
                return this.rbAfter.Checked;
            }
        }

        #endregion
    }
}
