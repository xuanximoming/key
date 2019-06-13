using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Core.IEMMainPageZY
{
    /// <summary>
    /// 校验病案首页信息窗体
    /// Add by xlb 2013-07-08
    /// </summary>
    public partial class ShowVerify : DevBaseForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="str">提醒信息</param>
        public ShowVerify(StringBuilder str)
        {
            try
            {
                InitializeComponent();
                this.richTextBoxVerify.Text = str.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
