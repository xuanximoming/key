using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors;
using System.Drawing;

namespace DrectSoft.Common.Ctrs.OTHER
{
    /// <summary>
    /// 功能描述：封装复选框控件
    ///           用于病案首页
    ///           创建人：项令波
    ///           创建时间：2013-07-03
    /// </summary>
    [ToolboxBitmap(typeof(DSDateEdit))]
    [Description("功能描述：封装时间选择控件\r\n-----------------------\r\nxlb  20130703")]
    public partial class DSCheckEdit : CheckEdit
    {
        public DSCheckEdit()
        {
            InitializeComponent();
        }

        public DSCheckEdit(IContainer container)
        {
            try
            {
                container.Add(this);

                InitializeComponent();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
