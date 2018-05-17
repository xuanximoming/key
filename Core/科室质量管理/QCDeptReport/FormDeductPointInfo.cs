using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Common.Report;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Core.QCDeptReport
{
    public partial class FormDeductPointInfo : DevBaseForm, DrectSoft.FrameWork.IStartPlugIn
    {
        #region 变量
        
        
        /// <summary>
        /// 创建插件的应用程序接口
        /// </summary>
        private IEmrHost m_App;
        /// <summary>
        /// 创建插件的应用程序接口
        /// </summary>
        public IEmrHost App
        {
            get { return m_App; }
            set { m_App = value; }
        }

        /// <summary>
        /// 控件中开始时间
        /// </summary>
        public DateTime m_Begintime;

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime m_Endtime;

        /// <summary>
        /// 科室比例失分点明细
        /// </summary>
        private UCDeductPointInfo m_UCDeductPointInfo;

        public String PointID = "";

        #endregion


        public FormDeductPointInfo(IEmrHost app)
        {
            InitializeComponent();
            m_App = app;
        }

        /// <summary>
        /// 加载控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _FormDeductPointInfo_Load(object sender, EventArgs e)
        {
            if (panelControlDeductPointInfo.Controls.Count > 1)
                return;
            m_UCDeductPointInfo = new UCDeductPointInfo(m_App);
            m_UCDeductPointInfo.Dock = DockStyle.Fill;
            m_UCDeductPointInfo.PointID = PointID;
            
            m_UCDeductPointInfo.m_Begintime = m_Begintime;
            m_UCDeductPointInfo.m_Endtime = m_Endtime;
            
            m_UCDeductPointInfo.m_form = this;
            panelControlDeductPointInfo.Controls.Add(m_UCDeductPointInfo);
            m_UCDeductPointInfo.Query_Click();
        }


        #region IStartPlugIn 成员

        public DrectSoft.FrameWork.IPlugIn Run(DrectSoft.FrameWork.WinForm.Plugin.IEmrHost host)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}