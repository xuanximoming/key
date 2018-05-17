using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using YidanSoft.FrameWork;
using YidanSoft.FrameWork.WinForm;
using YidanSoft.FrameWork.WinForm.Plugin;

namespace YidanSoft.Core.MaintainBasicInfo
{
    public partial class Form1 : Form, IStartPlugIn
    {
        IYidanEmrHost m_app;
        public Form1()
        {
            InitializeComponent();
        }
        #region IStartPlugIn 成员
        public IPlugIn Run(FrameWork.WinForm.Plugin.IYidanEmrHost host)
        {

            IPlugIn plg = new PlugIn(this.GetType().ToString(), this);
            m_app = host;

            return plg;
        }
        #endregion
    }
}
