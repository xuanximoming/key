using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DrectSoft.Core.EMR_NursingDocument.EMRInput.Table
{
    public partial class ColumnsInsert : Form
    {
        public ColumnsInsert()
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

        #region 公开的Form属性

        /// <summary>
        /// 要插入的列数
        /// </summary>
        public int ColumnNum
        {
            get
            {
                return Convert.ToInt32(nudNumber.Value);
            }
        }

        /// <summary>
        /// 是否在插入点的后面插入列
        /// </summary>
        public bool IsBack
        {
            get
            {
                return rbAfter.Checked;
            }
        }

        #endregion
    }
}
