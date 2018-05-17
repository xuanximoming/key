using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace DrectSoft.Common.Report
{
    /// <summary>
    /// 报表类
    /// TODO: 有关STREAM的处理待改进
    /// </summary>
    public class XReport : IDisposable
    {
        //private static string TEXTFmt = "报表设计器 - {0}";
        /// <summary>
        /// 设计时自动保存
        /// </summary>
        private static bool AUTOSAVE = true;

        #region fields
        /// <summary>
        /// 是：保存到文件中。否：保存到Stream中
        /// </summary>
        private bool m_Save2File;
        /// <summary>
        /// 需要向设计器的工具箱中插入的新控件
        /// </summary>
        private Collection<Type> m_NewControls;
        #endregion

        #region properties
        private string _fileName;
        /// <summary>
        /// 报表文件名，应该包含路径信息
        /// 注意：改变文件名将改变报表的布局,将改变报表对象<see cref="CurrentReport"/>
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set
            {
                m_Save2File = true;
                _fileName = value;
                SetFileReport(out _currentReport);
            }
        }

        /// <summary>
        /// 报表流
        /// </summary>
        public Stream ReportStream
        {
            get
            {
                if (_reportStream == null)
                    _reportStream = new MemoryStream();
                return _reportStream;
            }
            set
            {
                m_Save2File = false;
                _reportStream = value;
                SetStreamReport(out _currentReport);
            }
        }
        private Stream _reportStream;

        /// <summary>
        /// 经过压缩的报表模板
        /// </summary>
        public string CompressReportStream
        {
            get
            {
                Stream report = ReportStream;
                report.Position = 0;
                byte[] buffer = new byte[report.Length];
                report.Read(buffer, 0, buffer.Length);

                MemoryStream ms = new MemoryStream();
                DeflateStream compressedzipStream = new DeflateStream(ms, CompressionMode.Compress, true);

                compressedzipStream.Write(buffer, 0, buffer.Length);
                compressedzipStream.Close();

                ms.Position = 0;
                byte[] buffZipXml = new byte[ms.Length];
                ms.Read(buffZipXml, 0, buffZipXml.Length);
                return Convert.ToBase64String(buffZipXml);
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    ReportStream = new MemoryStream();
                else
                {
                    string s = value;
                    byte[] buffer = Convert.FromBase64String(s);
                    MemoryStream ms = new MemoryStream(buffer);
                    ms.Position = 0;

                    DeflateStream dfs = new DeflateStream(ms, CompressionMode.Decompress, true);
                    StreamReader sr = new StreamReader(dfs, Encoding.UTF8);
                    string sXml = sr.ReadToEnd();
                    sr.Close();
                    dfs.Close();
                    ReportStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(sXml));
                }
            }
        }

        /// <summary>
        /// 当前的报表，修改<see cref="FileName"/>文件名将自动修改报表      
        /// </summary>
        public XtraReport CurrentReport
        {
            get { return _currentReport; }
            //set { _currentReport = value; }
        }
        private XtraReport _currentReport;

        private DataSet _ds;
        /// <summary>
        /// 设置/取得当前报表<see cref="CurrentReport"/>的数据源      
        /// </summary>
        public DataSet DataSource
        {
            get { return _ds; }
            set
            {
                _ds = value;
                if (_currentReport != null)
                    _currentReport.DataSource = _ds;
            }
        }
        #endregion

        #region ctors
        /// <summary>
        /// 指定要打印的数据集合和文件名，文件名可以是不存在的（自动在第一次使用时根据文件名创建文件）
        /// </summary>
        /// <param name="dt">数据集合</param>
        /// <param name="filename">文件名</param>
        public XReport(DataTable dt, string filename)
            : this(filename)
        {
            if (_ds == null)
                _ds = new DataSet("数据源");
            if (dt == null)
                throw new ArgumentNullException("dt", "数据表为空");
            _ds.Tables.Add(dt);
            _currentReport.DataSource = _ds;

        }

        /// <summary>
        /// 指定要打印的数据集合和文件名
        /// ，文件名可以是不存在的
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="filename">文件名</param>
        public XReport(DataSet ds, string filename)
            : this(filename)
        {
            if (ds == null)
                throw new ArgumentNullException("ds", "数据集为空");
            _ds = ds;
            _currentReport.DataSource = _ds;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="reportStream"></param>
        public XReport(DataTable dt, Stream reportStream)
            : this()
        {
            _ds.Tables.Add(dt);
            ReportStream = reportStream;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="compressReportStream"></param>
        /// <param name="ds"></param>
        public XReport(string compressReportStream, DataSet ds)
            : this()
        {
            _ds = ds;
            CompressReportStream = compressReportStream;
            _currentReport.DataSource = _ds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="compressReportStream"></param>
        /// <param name="dt"></param>
        public XReport(string compressReportStream, DataTable dt)
            : this()
        {
            _ds.Tables.Add(dt);
            _currentReport.DataSource = _ds;
            CompressReportStream = compressReportStream;
        }

        private XReport(string filename)
            : this()
        {
            FileName = filename;
        }

        private XReport()
        {
            _ds = new DataSet("数据源");
        }
        #endregion

        #region private methods
        /// <summary>
        /// 试图从文件中创建报表，若不能则创建新的报表
        /// 不绑定数据集
        /// </summary>
        /// <param name="rp">输出报表</param>
        /// <returns>返回值为真表示文件不存在，是新创建的文件</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private bool SetFileReport(out XtraReport rp)
        {
            if (string.IsNullOrEmpty(_fileName) || string.IsNullOrEmpty(_fileName.Trim()))
            {
                throw new ArgumentNullException(_fileName, "文件名为空");
            }
            bool runDesign = true;
            if (File.Exists(_fileName))
            {
                runDesign = true;
                rp = new XtraReport();
                try
                {
                    rp.LoadLayout(_fileName);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("检查文件名是否有效\n", ex);
                }
            }
            else
            {
                try
                {
                    rp = XtraReport.FromFile(_fileName, true);
                    runDesign = false;
                }
                catch
                {
                    rp = new XtraReport();
                    runDesign = true;
                }
            }
            return runDesign;
        }

        /// <summary>
        /// 从流中创建报表，若不能创建新的报表，不绑定数据集
        /// </summary>
        /// <param name="rp"></param>
        /// <returns></returns>
        private bool SetStreamReport(out XtraReport rp)
        {
            bool runDesign;
            if (_reportStream == null)
            {
                runDesign = true;
                rp = new XtraReport();
                try
                {

                    rp.LoadLayout(ReportStream);
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                try
                {
                    rp = XtraReport.FromStream(_reportStream, true);
                    runDesign = false;
                }
                catch
                {
                    rp = new XtraReport();
                    runDesign = true;
                }
            }
            return runDesign;
        }
        #endregion

        #region public methods
        /// <summary>
        /// 向报表设计界面的工具箱中插入自定义的控件(需要在显示Design前调用)
        /// </summary>
        /// <param name="newControls"></param>
        public void ExpandToolBox(Collection<Type> newControls)
        {
            m_NewControls = newControls;
        }

        /// <summary>
        /// 给出预览界面
        /// </summary>
        public void ShowPreview()
        {
            _currentReport.DataSource = _ds;
            _currentReport.ShowPreview();
        }

        ///// <summary>
        ///// 给出设计界面
        ///// </summary>
        //public void Design()
        //{
        //    //初始化报表设计界面    
        //    if (m_frm == null)
        //    {
        //        m_frm = new XRDesignForm();
        //        //m_frm.ShowInTaskbar = false;
        //        m_frm.WindowState = FormWindowState.Maximized;
        //        if (AUTOSAVE)
        //        {
        //            m_frm.ReportStateChanged += new ReportStateEventHandler(m_frm_ReportStateChanged);
        //            m_frm.Closing += new System.ComponentModel.CancelEventHandler(m_frm_Closing);
        //        }
        //        m_frm.TextChanged += new EventHandler(m_frm_TextChanged);
        //    }
        //    m_frm.FileName = _fileName;
        //    CurrentReport.DataSource = _ds;
        //    if ((m_NewControls != null) && (m_NewControls.Count > 0))
        //        CurrentReport.DesignerLoaded += new DesignerLoadedEventHandler(CurrentReport_DesignerLoaded);
        //    m_frm.OpenReport(_currentReport);
        //    m_frm.ShowDialog();
        //}

        private void CurrentReport_DesignerLoaded(object sender, DesignerLoadedEventArgs e)
        {
            IToolboxService ts = (IToolboxService)e.DesignerHost.GetService(typeof(IToolboxService));

            foreach (Type type in m_NewControls)
                ts.AddToolboxItem(new ToolboxItem(type));
        }

        /// <summary>
        /// 将报表转换成图片
        /// </summary>
        /// <returns></returns>
        public Image ConvertReportToImage()
        {
            Stream imageStream = new MemoryStream();
            _currentReport.DataSource = _ds;
            CurrentReport.ExportToImage(imageStream, ImageFormat.Png);
            return Image.FromStream(imageStream);
        }
        #endregion

        #region event handle

        #endregion

        #region IDisposable Members
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            //if (m_frm != null)
            //{
            //   m_frm.Dispose();
            //   m_frm = null;
            //}
            if (_currentReport != null)
            {
                _currentReport.Dispose();
                _currentReport = null;
            }
            if (_ds != null)
            {
                _ds.Dispose();
                _ds = null;
            }
        }

        #endregion
    }
}
