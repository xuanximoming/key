using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraReports.UserDesigner;
using System.Windows.Forms;
using System.IO;
using DevExpress.XtraReports.UI;
using YidanSoft.Common.Report;
using System.Data;
using XtraReportsDemos;
using YidanSoft.FrameWork;

namespace YidanSoft.Common.Report
{
    public class ReportDesign
    {
        private static string TEXTFmt = "南京一丹软件报表设计器 - {0}";
        /// <summary>
        /// 设计时自动保存
        /// </summary>
        private static bool AUTOSAVE = false;
        private CustomDesignForm m_frm;
        /// <summary>
        /// 是否需要在调用浏览时先调用一次设计界面
        /// </summary>
        //private bool m_NeedRunDesign;
        /// <summary>
        /// 是：保存到文件中。否：保存到Stream中
        /// </summary>
        private bool m_Save2File = false;

        /// <summary>
        /// 当前的报表，修改<see cref="FileName"/>文件名将自动修改报表      
        /// </summary>
        public XReport ReportUtil
        {
            get { return _reportUtil; }
            //set { _currentReport = value; }
        }
        private XReport _reportUtil;

        private DataSet _templateSource;
        private string _templateStr;
        private bool m_FromStream = false;

        public ReportDesign()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateFile">报表文件</param>
        /// <param name="ds">数据集</param>
        public ReportDesign(string templateFile, DataSet ds)
        {
            _templateSource = ds;
            _templateStr = templateFile;

        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="templateFile">报表文件</param>
        ///// <param name="ds">数据集</param>
        //public ReportDesign(string templateFile, DataSet ds, XRDesignFormExBase form)
        //{
        //    _templateSource = ds;
        //    _templateStr = templateFile;
        //    m_frm = form;
        //    Design();
        //}

        #region FormDataSoruce
        //private DataTable m_ReportDataSoruce = new DataTable();
        //private string m_Path = string.Empty;
        //private void m_frm_Load(object sender, EventArgs e)
        //{
        //    m_frm.Visible = true;
        //    FormDataSoruce formSoruce = new FormDataSoruce();
        //    if (formSoruce.ShowDialog() == DialogResult.OK)
        //    {
        //        m_ReportDataSoruce = formSoruce.ReportDataSoruce.Copy();
        //        m_Path = formSoruce.Path;
        //        DataSet ds = new DataSet();
        //        ds.Tables.Add(m_ReportDataSoruce);
        //        m_frm.Visible = true;
        //        ReportDesign rp = new ReportDesign(m_Path, ds);
        //        rp.Design();
        //    }
        //    else
        //    {
        //        m_frm.Close();
        //    }
        //}




        //#endregion
        #endregion


        /// <summary>
        /// 给出设计界面
        /// </summary>
        public void Design()
        {
            //初始化报表设计界面 
            if (m_FromStream)
                _reportUtil = new XReport(_templateStr, _templateSource);
            else
                _reportUtil = new XReport(_templateSource, _templateStr);
            if (m_frm == null)
            {
                m_frm = new CustomDesignForm();
                m_frm.SkipQueryData = true;
                m_frm.WindowState = FormWindowState.Maximized;
            }
            if (AUTOSAVE)
            {
                m_frm.ReportStateChanged += new ReportStateEventHandler(m_frm_ReportStateChanged);
                m_frm.Closing += new System.ComponentModel.CancelEventHandler(m_frm_Closing);
            }
            m_frm.TextChanged += new EventHandler(m_frm_TextChanged);
            m_frm.FileName = _reportUtil.FileName;
            ReportUtil.DataSource = _templateSource;
            ReportUtil.DataSource.DataSetName = "ReportDesign";
            m_frm.OpenReport(_reportUtil.CurrentReport);

            m_frm.ShowDialog();
        }

        void m_frm_TextChanged(object sender, EventArgs e)
        {
            ReportUtil.DataSource = _templateSource;
            XRDesignFormExBase frm = sender as XRDesignFormExBase;
            string text = frm.Text;
            int pos = text.IndexOf(" - ");
            if (pos > 0)
            {
                text = text.Substring(pos + 3);
                frm.TextChanged -= new EventHandler(m_frm_TextChanged);
                frm.Text = string.Format(TEXTFmt, text);
                frm.TextChanged += new EventHandler(m_frm_TextChanged);
            }

        }

        /// <summary>
        /// 设计时若自动保存出发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_frm_ReportStateChanged(object sender, ReportStateEventArgs e)
        {
            ReportUtil.DataSource = _templateSource;
            if (e.ReportState == ReportState.Changed)
            {
                if (m_Save2File)
                {
                    XRDesignFormExBase frm = sender as XRDesignFormExBase;
                    frm.SaveReport(frm.FileName);
                }
            }
        }

        /// <summary>
        /// 设计时若自动保存出发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_frm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!m_FromStream)
            {
                XRDesignFormExBase frm = sender as XRDesignFormExBase;
                frm.SaveReport(frm.FileName);
            }
            else
            {

                ReportUtil.ReportStream = new MemoryStream();
                ReportUtil.CurrentReport.SaveLayout(ReportUtil.ReportStream);
            }
        }
    }
}
