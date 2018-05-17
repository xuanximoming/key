using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Core.IEMMainPage
{
    /// <summary>
    /// 验证信息窗体
    /// </summary>
    public partial class ShowVerify : DevBaseForm
    {
        public ShowVerify(StringBuilder msg)
        {
            try
            {
                InitializeComponent();
                this.richTextBoxVerify.Text = msg.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}