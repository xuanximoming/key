using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Common.Ctrs.FORM;
using System.IO;
using DrectSoft.FrameWork.WinForm.Plugin;
using O2S.Components.PDFRender4NET;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.DSSqlHelper;
using DevExpress.Utils;
using DrectSoft.Service;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Net;
using DevExpress.XtraGrid.Views.Grid;

namespace DrectSoft.Core.MainEmrPad.New
{
    /// <summary>
    /// 调阅病理结果的窗体（包括展示病理结果列表及PDF浏览功能）
    /// add by ywk 2013年8月9日 16:20:08
    /// </summary>
    public partial class ShowPathologic : DevBaseForm
    {
        private IEmrHost m_App;
        public ShowPathologic()
        {
            InitializeComponent();
        }
        public ShowPathologic(IEmrHost app)
            : this()
        {
            m_App = app;
            DS_SqlHelper.CreateSqlHelper();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowPathologic_Load(object sender, EventArgs e)
        {
            try
            {
                //this.xtraScrollableControl1.MouseWheel += new MouseEventHandler(xtraScrollableControl1_MouseWheel);
                BindPathLoginByInp(m_App.CurrentPatientInfo.NoOfHisFirstPage);
                DeletePDF();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }


        }
        /// <summary>
        /// 启动前先删除原来生成的PDF文件
        /// add by ywk 2013年8月13日 16:35:17
        /// </summary>
        private void DeletePDF()
        {
            if (Directory.Exists("C:\\PatBLReport"))
            {
                DirectoryInfo dirInfo = new DirectoryInfo("C:\\PatBLReport");
                foreach (FileInfo fi in dirInfo.GetFiles("*.pdf"))
                {
                    try
                    {
                        fi.Delete();
                    }
                    catch (Exception ex)
                    { }
                }

            }
        }
        /// <summary>
        /// 根据住院号取得此患者的病理诊断结果 
        /// </summary>
        /// <param name="p"></param>
        private void BindPathLoginByInp(string patnoofhis)
        {
            try
            {
                string sql = string.Format(@"select * from pathologicreport where F_BRBH='{0}'", patnoofhis);
                DataTable dtSource = DS_SqlHelper.ExecuteDataTable(sql, CommandType.Text);
                if (dtSource != null)
                {
                    //还要再进行处理一下
                    grdPathologic.DataSource = dtSource;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }

        }
        private string newcgpath = string.Empty;
        private string newbcpath = string.Empty;

        Bitmap pageImage;
        /// <summary>
        /// 绑定PDF用于显示
        /// </summary>
        /// 根据病理号和住院号获得病人的诊断PDF
        /// PDFLJ1常规报告路径  PDFLJ2 补充报告路径
        private void BindPDF(string blh, string zyh)
        {
            try
            {
               

                #region 采用插件形式(已废弃)
                //AxAcroPDF acroPDF = new AxAcroPDF();
                //((System.ComponentModel.ISupportInitialize)(acroPDF)).BeginInit();
                //acroPDF.Location = new Point(0, 24);
                //acroPDF.Size = new Size(300, 300);
                //acroPDF.Dock = DockStyle.Fill;
                //this.Controls.Add(acroPDF);
                //((System.ComponentModel.ISupportInitialize)(acroPDF)).EndInit();
                //acroPDF.setShowToolbar(false);
                ////acroPDF.
                //acroPDF.setShowScrollbars(false);
                //acroPDF.setPageMode("pages only");//pages only, pages and thumbnails, or pages and bookmarks
                //acroPDF.LoadFile(@"D:\扫描0028.pdf");
                #endregion

                string[] pathinfo = DS_SqlService.GetConfigValueByKey("PathologicPathInfo").Split(',');
                string ippath = string.Empty;
                string username = string.Empty;
                string pwd = string.Empty;
                if (pathinfo.Length >= 3)
                {
                    ippath = pathinfo[0].ToString().Trim();
                    username = pathinfo[1].ToString().Trim();
                    pwd = pathinfo[2].ToString().Trim();
                }
                DataTable PathDT = new DataTable();
                PathDT = DS_SqlHelper.ExecuteDataTable(string.Format("select PDFLJ1,PDFLJ2 from PATHOLOGICREPORT where F_BLH='{0}' and F_BRBH='{1}'", blh, zyh));
                string cgportpath = string.Empty;//常规报告路径
                string bcportpath = string.Empty;//补充报告路径
                if (PathDT != null && PathDT.Rows.Count > 0)
                {
                    cgportpath = PathDT.Rows[0]["PDFLJ1"].ToString();
                    bcportpath = PathDT.Rows[0]["PDFLJ2"].ToString();
                }

                if (!Directory.Exists("C:\\PatBLReport"))
                {
                    Directory.CreateDirectory("C:\\PatBLReport");
                }
                #region 绑定常规报告图片
                if (!string.IsNullOrEmpty(cgportpath))
                {
                    try
                    {

                        WebClient request = new WebClient();
                        request.Credentials = new NetworkCredential(username, pwd);
                        byte[] newFileData = request.DownloadData(cgportpath.ToString());
                        newcgpath = "C:\\PatBLReport\\" + m_App.CurrentPatientInfo.NoOfHisFirstPage +Guid.NewGuid().ToString().Substring(0,10)+ "changgui.pdf";
                        using (FileStream fs = new FileStream(newcgpath, FileMode.OpenOrCreate))
                        {
                            fs.Write(newFileData, 0, newFileData.Length);
                            fs.Close();
                        }
                        PDFFile pdfFile = PDFFile.Open(newcgpath);
                        pageImage = pdfFile.GetPageImage(0, 56 * 10);
                        //pageImage.Save("c:\\myBitmap1.bmp");
                        //pageImage.
                        //picCG.Width = pageImage.Width;
                        //picCG.Height = pageImage.Height;
                        //picCG.Location = new Point(0, 0);
                        //this.picCG.BackgroundImage = pageImage;
                        //picCG.CreateGraphics().DrawImage(pageImage, new Point(0, 0));
                        this.picCG.Image = pageImage;
                        //this.picCG.Focus();

                        //picCG.CreateGraphics().DrawImage(pageImage, new Point(0, 0));
                        //picCG.SizeMode = PictureBoxSizeMode.Zoom;
                        pdfFile.Dispose();
                        //pageImage.Dispose();
                    }
                    catch (Exception)
                    {

                    }
                }

                #endregion

                #region 绑定补充报告图片
                if (!string.IsNullOrEmpty(bcportpath))
                {
                    try
                    {
                        WebClient request1 = new WebClient();
                        request1.Credentials = new NetworkCredential(username, pwd);
                        byte[] newFileDatabc = request1.DownloadData(bcportpath.ToString());
                        //声明GUID标识，防止访问时正在使用中
                        newbcpath = "C:\\PatBLReport\\" + m_App.CurrentPatientInfo.NoOfHisFirstPage + Guid.NewGuid().ToString().Substring(0, 10) + "buchong.pdf";
                        using (FileStream fs1 = new FileStream(newbcpath, FileMode.OpenOrCreate))
                        {
                            fs1.Write(newFileDatabc, 0, newFileDatabc.Length);
                            //foreach (Byte b in newFileDatabc)
                            //{
                            //    fs1.WriteByte(b);//报内存溢出估计就是这里循环的在写入字节
                            //edit by ywk 2013年9月2日 15:20:25
                            //}
                            //fs1.Write(newFileDatabc, 0, newFileData.Length);
                            fs1.Close();
                        }
                        PDFFile pdfFileBC = PDFFile.Open(newbcpath);
                        Bitmap pageBCImage = pdfFileBC.GetPageImage(0, 56 * 10);
                        this.picBC.Image = pageBCImage;

                        //this.picBC.Focus();

                        //pageBCImage.Save("c:\\myBitmap2.bmp");
                        pdfFileBC.Dispose();
                        //pageBCImage.Dispose();
                    }
                    catch (Exception)
                    {
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
                //if (this.picBC.Image != null)
                //{
                //    this.picBC.Image.Dispose(); this.picBC.Image = null;
                //}
                //if (this.picCG.Image != null)
                //{
                //    this.picCG.Image.Dispose(); this.picCG.Image = null;
                //}
                //DeletePDF();
            }

        }

        private WaitDialogForm m_WaitDialog;//查询等待窗体add by ywk 2013年7月17日 11:29:48 
        /// <summary>
        /// 双击带出报告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdPathologic_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.picBC.Image != null)
                {
                    this.picBC.Image.Dispose(); this.picBC.Image = null;
                }
                if (this.picCG.Image != null)
                {
                    this.picCG.Image.Dispose(); this.picCG.Image = null;
                }
                DeletePDF();
                //GridHitInfo hitInfo = gridView1.CalcHitInfo(grdPathologic.PointToClient(Cursor.Position));

                GridHitInfo hi = gridView1.CalcHitInfo((sender as Control).PointToClient(Control.MousePosition));
                if (!hi.InColumn)
                {
                    if (gridView1.FocusedRowHandle < 0)
                    {
                        MessageBox.Show("该患者无病理诊断数据");
                        return;
                    }
                    m_WaitDialog = new WaitDialogForm("正在加载该患者病理诊断报告...", "请稍候");
                    DataRow dataRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);

                    string m_blh = dataRow["F_BLH"].ToString();//病理号
                    string m_zyh = dataRow["F_BRBH"].ToString();//住院号

                    BindPDF(m_blh, m_zyh);
                }
                else
                {
                    MessageBox.Show("您尚未选中要查看的病理诊断数据");
                    return;
                }
                //this.xtraScrollableControl1.MouseWheel += new MouseEventHandler(xtraScrollableControl1_MouseWheel);
                //ScrollHelper scrol = new ScrollHelper(this.xtraScrollableControl1);
                //scrol.EnableScrollOnMouseWheel();
                //scrol.SubscribeToMouseWheel(this.xtraScrollableControl1.Controls);
                HideWaitDialog();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
                //if (this.picBC.Image != null)
                //{
                //    this.picBC.Image.Dispose(); this.picBC.Image = null;
                //}
                //if (this.picCG.Image != null)
                //{
                //    this.picCG.Image.Dispose(); this.picCG.Image = null;
                //}
                //DeletePDF();
            }

        }

        void xtraScrollableControl1_MouseWheel(object sender, MouseEventArgs e)
        {
            DevExpress.Utils.DXMouseEventArgs.GetMouseArgs(e).Handled = true;

            int scrollValue = xtraScrollableControl1.VerticalScroll.Value;

            int largeChange = xtraScrollableControl1.VerticalScroll.LargeChange;

            if (e.Delta < 0)

                xtraScrollableControl1.VerticalScroll.Value += xtraScrollableControl1.VerticalScroll.LargeChange;

            else

                if (scrollValue < largeChange)

                    xtraScrollableControl1.VerticalScroll.Value = 0;

                else

                    xtraScrollableControl1.VerticalScroll.Value -= largeChange;
        }
        public void HideWaitDialog()
        {
            if (m_WaitDialog != null)
                m_WaitDialog.Hide();
        }

        private void ShowPathologic_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void picCG_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void xtraScrollableControl1_Scroll(object sender, XtraScrollEventArgs e)
        {
            //MessageBox.Show("sad");
            //int largeChange = xtraScrollableControl1.VerticalScroll.LargeChange;
            //xtraScrollableControl1.VerticalScroll.Value += xtraScrollableControl1.VerticalScroll.LargeChange;

        }

        private void picCG_MouseDown(object sender, MouseEventArgs e)
        {
            this.picCG.Focus();

        }

        private void picBC_MouseDown(object sender, MouseEventArgs e)
        {
            this.picBC.Focus();

        }

       

    }


    public class ScrollHelper
    {

        XtraScrollableControl scrollableControl;



        public ScrollHelper(XtraScrollableControl scrollableControl)
        {

            this.scrollableControl = scrollableControl;

        }



        public void EnableScrollOnMouseWheel()
        {

            scrollableControl.VisibleChanged += OnVisibleChanged;

        }



        void OnVisibleChanged(object sender, EventArgs e)
        {

            scrollableControl.Select();

            UnsubscribeFromMouseWheel(scrollableControl.Controls);

            SubscribeToMouseWheel(scrollableControl.Controls);

        }



        public void SubscribeToMouseWheel(Control.ControlCollection controls)
        {

            foreach (Control ctrl in controls)
            {

                ctrl.MouseWheel += OnMouseWheel;

                SubscribeToMouseWheel(ctrl.Controls);

            }

        }



        private void UnsubscribeFromMouseWheel(Control.ControlCollection controls)
        {

            foreach (Control ctrl in controls)
            {

                ctrl.MouseWheel -= OnMouseWheel;

                UnsubscribeFromMouseWheel(ctrl.Controls);

            }

        }



        void OnMouseWheel(object sender, MouseEventArgs e)
        {

            DevExpress.Utils.DXMouseEventArgs.GetMouseArgs(e).Handled = true;

            int scrollValue = scrollableControl.VerticalScroll.Value;

            int largeChange = scrollableControl.VerticalScroll.LargeChange;

            if (e.Delta < 0)

                scrollableControl.VerticalScroll.Value += scrollableControl.VerticalScroll.LargeChange;

            else

                if (scrollValue < largeChange)

                    scrollableControl.VerticalScroll.Value = 0;

                else

                    scrollableControl.VerticalScroll.Value -= largeChange;

        }



        public void DisableScrollOnMouseWheel()
        {

            scrollableControl.VisibleChanged -= OnVisibleChanged;

            UnsubscribeFromMouseWheel(scrollableControl.Controls);

        }

    }

}