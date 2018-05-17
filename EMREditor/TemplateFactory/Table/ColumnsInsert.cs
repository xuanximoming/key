using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Emr.TemplateFactory
{
    public partial class ColumnsInsert : DevBaseForm
    {
        public ColumnsInsert()
        {
            InitializeComponent();
            this.nudNumber.Focus();
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

        /// <summary>
        /// 回车切换焦点
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-11-07</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void win_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                DS_Common.win_KeyPress(e);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
