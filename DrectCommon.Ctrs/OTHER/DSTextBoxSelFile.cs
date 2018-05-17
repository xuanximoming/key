using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DrectSoft.Common.Ctrs.OTHER
{
    /// <summary>
    /// 功能描述：封装一个特殊文本框
    /// 创 建 者：项令波
    /// 创建日期：2013-05-06
    /// </summary>
    [ToolboxBitmap(typeof(DevButtonAdd))]
    [Description("功能描述：封装一个特殊文本框\r\n-----------------------\r\nxlb  20130506")]
    public partial class DSTextBoxSelFile : TextBox
    {
        public DSTextBoxSelFile()
        {
            try
            {
                InitializeComponent();
                this.BorderStyle = BorderStyle.Fixed3D;
                this.ReadOnly = true;
                this.BackColor = Color.White;
                this.Controls.Add(btnLoadFile);
                btnLoadFile.Dock = DockStyle.Right;
                btnLoadFile.Cursor = Cursors.Hand;
                btnLoadFile.TabStop = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
