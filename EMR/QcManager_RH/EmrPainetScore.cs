using DevExpress.XtraPrinting;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;

namespace DrectSoft.Emr.QcManager
{
    /// <summary>
    /// 病历评分页面
    /// add by ywk
    /// </summary>
    public partial class EmrPainetScore : DevExpress.XtraEditors.XtraForm
    {
        #region 属性或字段
        string m_HospitalName = "";
        private string HospitalName
        {
            get
            {
                if (string.IsNullOrEmpty(m_HospitalName))
                {
                    //DataTable dt = m_SqlManager.GetPatInfoByNo(m_NoOfInpat);
                    //得到医院信息
                    GetHospitalName();
                }
                return m_HospitalName;
            }
        }
        private IEmrHost m_App;
        DrawHeadRectangle m_DrawHeadRectangle = new DrawHeadRectangle();
        SqlManger m_SqlManager;
        DataRow patientRow;
        private string m_NoOfInpat = string.Empty;

        #endregion

        #region 构造函数
        public EmrPainetScore()
        {
            InitializeComponent();
        }
        public EmrPainetScore(IEmrHost app, DataRow patientRow)
        {
            InitializeComponent();
            m_App = app;
            m_SqlManager = new SqlManger(app);
            m_NoOfInpat = patientRow["NOOFINPAT"].ToString();
            this.patientRow = patientRow;
            EmrScoreData.App = app;

            //在构造函数中取得病人的基本信息，绑定到用户控件里
            //DataTable dtpaientInfo = EmrScoreData.GetPatientInfoForPoint(m_NoOfInpat);
            //this.ucEmrPainetScore2.SetPatientInfo(dtpaientInfo);
            //this.ucEmrPainetScore2.LoadData();
        }
        #endregion

        #region  方法
        /// <summary>
        /// 得到医院名称
        /// </summary>
        private void GetHospitalName()
        {
            if (this.DesignMode == false)
            {
                m_HospitalName = InitHospitalName(QcManager.EmrScoreData.GetHospitalName());
            }
        }
        /// <summary>
        /// 初始化医院名称 
        /// </summary>
        /// <param name="hospitalName"></param>
        /// <returns></returns>
        private string InitHospitalName(string hospitalName)
        {
            string name = string.Empty;

            for (int i = 0; i < hospitalName.Length - 1; i++)
            {
                name += hospitalName[i] + " ";
            }
            name += hospitalName[hospitalName.Length - 1];
            return name;
        }
        /// <summary>
        /// 调整用户控件的位置
        /// </summary>
        private void ReSetUCLocaton()
        {
            //Int32 pointX = (this.Width - this.ucEmrPainetScore2.Width) / 2;
            //Int32 pointY = ucEmrPainetScore2.Location.Y;
            //this.ucEmrPainetScore2.Location = new Point(pointX, pointY);
        }
        PrintingSystem ps = new PrintingSystem();

        /// <summary>
        /// 窗体加载事件查出该病人的信息 
        /// add ywk
        /// </summary>
        /// <param name="m_NoOfInpat"></param>
        private void BindGridData(string m_NoOfInpat)
        {
            #region old按用户的添加创建DataTable(现改为先取出Table结构再塞值)
            //DataTable dtpat = m_SqlManager.GetPainetData(m_NoOfInpat);
            //DataTable newtable = OperateDTData(dt);
            //gridControlPaint.DataSource = newtable;
            #endregion

            #region 各评分项先固定，根据用户数据进行填充扣分内容
            //(***另种方法也可以把几个关于文件夹的评分项先加进Table结构中*)
            DataTable dt = m_SqlManager.GetConfigPoint();
            DataColumn colREDPOINT = new DataColumn();
            colREDPOINT.ColumnName = "REDPOINT";
            DataColumn colKOUFENLIYOU = new DataColumn();
            colKOUFENLIYOU.ColumnName = "KOUFENLIYOU";
            dt.Columns.Add(colREDPOINT);
            dt.Columns.Add(colKOUFENLIYOU);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //先在此处给空值，后面就要根据病人的Noofinpat来进行向相应的位置填值
                dt.Rows[i]["REDPOINT"] = "";
                dt.Rows[i]["KOUFENLIYOU"] = "";
            }
            //可在此，加上固定的那几个一项的大类评分项（ 病历封面，出院小结，首次病程记录。。。。）
            //对应usp_GetPointClass中写为上面的几个大分类
            DataTable newtable = OperatePatPoint(dt, m_NoOfInpat);
            DataTable OutTable = GetResultTable(newtable);//对可能包含对文件夹评分的且多次评分的table再次处理
            gridControlPaint.DataSource = OutTable;
            #endregion
            GetPanientData();
            GetPoint();//此处要取此病人的分数

        }
        /// <summary>
        /// 最终要得到的DataTable(处理可能包含对文件夹的评分）
        /// </summary>
        /// <param name="newtable"></param>
        /// <returns></returns>
        private DataTable GetResultTable(DataTable newtable)
        {
            DataTable NewDT = newtable.Clone();
            List<string> IdList = new List<string>();
            for (int i = 0; i < newtable.Rows.Count; i++)
            {
                DataRow newrows = NewDT.NewRow();
                DataRow[] filterRows = newtable.Select("ID='" + newtable.Rows[i]["ID"].ToString() + "'");
                if (filterRows.Length > 1)//包含相同的评分项
                {
                    double sumpoint = 0;
                    for (int k = 0; k < filterRows.Length; k++)
                    {
                        newrows["KOUFENLIYOU"] += filterRows[k]["KOUFENLIYOU"].ToString() + "\r\n";
                        newrows["CHILDNAME"] = filterRows[k]["CHILDNAME"].ToString();
                        newrows["ID"] = filterRows[k]["ID"].ToString();
                        newrows["CNAME"] = filterRows[k]["CNAME"].ToString();
                        sumpoint += Convert.ToDouble(filterRows[k]["redpoint"].ToString() == "" ? "0" : filterRows[k]["redpoint"].ToString());
                        newrows["REDPOINT"] = sumpoint.ToString();
                    }
                    NewDT.Rows.Add(newrows);
                }
                else
                {
                    NewDT.ImportRow(filterRows[0]);
                }
            }
            DataTable resDt = NewDT.Clone();
            for (int j = 0; j < NewDT.Rows.Count; j++)
            {
                string id = NewDT.Rows[j]["id"].ToString();
                if (!IdList.Contains(id))
                {
                    IdList.Add(id);
                    resDt.ImportRow(NewDT.Rows[j]);
                }
            }
            return resDt;
        }
        /// <summary>
        /// （NEW）将传过来的这个病人的评分信息，填入到评分表的相应位置
        /// add by ywk 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private DataTable OperatePatPoint(DataTable dt, string mNoofinapt)
        {
            string rhqcTableId = this.patientRow["ID"].ToString();
            DataTable ResultDt = dt.Copy();//最终要处理此Table
            DataTable dtpat = m_SqlManager.GetPainetData(mNoofinapt, rhqcTableId);
            ArrayList notInId = new ArrayList();//记录没有在评分配置表中的配置的项（包括那些根节点）
            //对ResultDt循环，相应的塞值
            for (int i = 0; i < ResultDt.Rows.Count; i++)
            {
                for (int j = 0; j < dtpat.Rows.Count; j++)
                {
                    if (ResultDt.Rows[i]["ID"].ToString() == dtpat.Rows[j]["ID"].ToString())
                    {
                        double sumpoint = 0;//各项的总扣分
                        string koufenly = string.Empty;//扣分理由
                        DataRow[] filterow = dtpat.Select("ID='" + ResultDt.Rows[i]["ID"].ToString() + "'");
                        for (int k = 0; k < filterow.Length; k++)
                        {
                            koufenly += filterow[k]["KOUFENLIYOU"].ToString() + "\r\n";
                            sumpoint += Double.Parse(filterow[k]["REDPOINT"].ToString());
                            ResultDt.Rows[i]["REDPOINT"] = sumpoint.ToString();
                            ResultDt.Rows[i]["KOUFENLIYOU"] = koufenly;
                        }
                    }
                }
            }

            #region old
            //bool isfind = false;//是否找到
            //循环完成后，处理对于在配置中没有配置的选项，比如病案首页，会诊记录等跟节点时，评分表加上它的数据
            //for (int h = 0; h < dtpat.Rows.Count; h++)
            //{
            //    int my = 0;
            //    for (int l = 0; l < ResultDt.Rows.Count; l++)
            //    {
            //        my++;
            //        if (dtpat.Rows[h]["ID"] == ResultDt.Rows[l]["ID"])
            //        {
            //            isfind = false;
            //            break;//找到就break
            //        }
            //        else
            //        {
            //            isfind = true;
            //            if (dtpat.Rows.Count != my)
            //            {
            //                continue;//没找到接着找
            //            }
            //        }
            //    }
            //    if (isfind)
            //    {
            //        notInId.Add(dtpat.Rows[h]["ID"].ToString());
            //    }
            //    break;
            //}
            #endregion
            //ID 小于或者等于0的均是对文件夹评分的（病案首页，会诊记录...）
            DataRow[] findrow = dtpat.Select(" ID<=0");
            for (int s = 0; s < findrow.Length; s++)
            {
                string id = findrow[s]["ID"].ToString();
                if (!notInId.Contains(id))
                {
                    notInId.Add(id);
                }
            }
            //通过上面存取的ID取得评分内容，加到最终要显示ResultTable中
            for (int n = 0; n < notInId.Count; n++)
            {
                DataRow[] m_row = dtpat.Select("ID='" + notInId[n].ToString() + "'");
                for (int m = 0; m < m_row.Length; m++)
                {
                    ResultDt.ImportRow(m_row[m]);
                }
            }
            return ResultDt;//此DataTable可能包含对文件夹评分的情况，（且可能包含重复项数据）
        }
        private int SumPoint { get; set; }//满分值。通过配置中取得 ywk 
        /// <summary>
        /// 得到总得分(处理分数相关)
        /// </summary>
        private void GetPoint()
        {
            labelControl9.Text = "  应得分：" + SumPoint + "分";
            DataTable dt = gridControlPaint.DataSource as DataTable;
            if (dt != null)
            {
                double totalPoint = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    double point = Convert.ToDouble(dr["REDPOINT"].ToString() == "" ? "0" : dr["redpoint"].ToString());
                    totalPoint += point;
                }
                labelControl7.Text = "     实得分：" + Convert.ToString(SumPoint - totalPoint);
                //等级根据配置中显示
                if (SumPoint < 90)
                {
                    labelControl8.Text = "                                甲级≥80分       乙级≥65分       丙级<65分      ";
                }
                if (SumPoint > 90)
                {
                    labelControl8.Text = "                                甲级≥90分       乙级≥75分       丙级<75分      ";
                }
            }
        }
        /// <summary>
        /// 获取病人信息(处理病人信息相关)
        /// </summary>
        private void GetPanientData()
        {
            DataTable dtpaientInfo = EmrScoreData.GetPatientInfoForPoint(m_NoOfInpat);
            if (dtpaientInfo.Rows.Count > 0)
            {
                EmrPointInfo emrPointInfo = new EmrPointInfo();
                emrPointInfo.InitPatientInfo(dtpaientInfo);
            }
            lbldeptname.Text = "科室：" + EmrPointInfo.DeptName;
            lblpatname1.Text = "患者姓名：" + EmrPointInfo.InpatientName;
            lblpatid1.Text = "住院号：" + EmrPointInfo.InpatientNo;
            lblzhuyuandoc.Text = "住院医师：" + EmrPointInfo.ResidentDoc;
            lblupdoc.Text = "上级医师：" + EmrPointInfo.ChiefDoc;
            lblHospital.Text = HospitalName;
        }
        /// <summary>
        /// 创建表头标题和底部的信息
        /// </summary>
        private void CreateHeaderAndFoot()
        {
            //PrintingSystem ps = new DevExpress.XtraPrinting.PrintingSystem();
            //DevExpress.XtraPrinting.PrintableComponentLink link = null;
            //link = new DevExpress.XtraPrinting.PrintableComponentLink(ps);
            //ps.Links.Add(link);
            //link.Component = gridControlPaint;
            ////实现IPrintable接口的控件都可以赋值
            //string strPrintHeader = "                      病 历 评 分 表\r\n";
            //string strPainetInfo = "科室：" + EmrPointInfo.DeptName + "  " + "患者姓名：" + EmrPointInfo.InpatientName +
            //    "  " + "住院号：" + EmrPointInfo.InpatientNo + "  " + "住院医师：" + EmrPointInfo.ResidentDoc +
            //    "  " + "上级医师：" + EmrPointInfo.ChiefDoc + " ";
            //string strHeaderHospital = "                   " + HospitalName + " \r\n " + strPrintHeader;
            //PageHeaderFooter phf = link.PageHeaderFooter as PageHeaderFooter;
            //phf.Header.Content.Clear();
            ////加医院名称
            //// phf.Header.Content.AddRange(new string[] { "", strHeaderHospital, "" });
            //phf.Header.Content.Add(strHeaderHospital);
            //phf.Header.Font = new System.Drawing.Font("宋体", 15, System.Drawing.FontStyle.Bold);
            //phf.Header.LineAlignment = BrickAlignment.Center;

            ////加患者信息内容
            ////PageHeaderFooter phf1 = link.PageHeaderFooter as PageHeaderFooter;
            ////phf.Header.Content.Add(strPainetInfo);
            ////phf.Header.Font = new System.Drawing.Font("宋体", 10, System.Drawing.FontStyle.Bold);
            ////phf.Header.LineAlignment = BrickAlignment.Far;
            ////HeaderFooterForm dsd = new HeaderFooterForm();
            ////dsd.Text = "dasdsa";

            //link.CreateDocument(); //建立文档
            //ps.PreviewFormEx.Show();//预览 


        }
        /// <summary>
        /// (OLD原来的动态取数据的处理)此方法中要处理，小项重复，扣分要相加，返回处理后的DataTable
        /// </summary>
        /// <param name="dt"></param>
        private DataTable OperateDTData(DataTable dt)
        {
            string rhqcTableId = this.patientRow["ID"].ToString();
            DataTable NewDT = dt.Clone();
            //  包含大类的 
            string sqlchildid = string.Format(@"select distinct b.emrpointid as ID,b.sortid  from   
            emr_rhpoint b  where b. noofinpat='{0}'and rhqc_table_id='{1}' and b.valid='1' order by b.sortid", m_NoOfInpat, rhqcTableId);

            DataTable dtChildID = m_App.SqlHelper.ExecuteDataTable(sqlchildid);
            if (dtChildID.Rows.Count > 0)
            {
                for (int i = 0; i < dtChildID.Rows.Count; i++)
                {
                    DataRow[] filterRows = dt.Select("ID='" + dtChildID.Rows[i]["ID"].ToString() + "'");
                    if (filterRows.Length > 1)//说明这些行可以合并为一行数据
                    {
                        DataRow newrows = NewDT.NewRow();
                        double sumpoint = 0;
                        for (int k = 0; k < filterRows.Length; k++)
                        {
                            newrows["KOUFENLIYOU"] += filterRows[k]["KOUFENLIYOU"].ToString() + "\r\n";
                            newrows["NOOFINPAT"] = filterRows[k]["NOOFINPAT"].ToString();
                            newrows["PATID"] = filterRows[k]["PATID"].ToString();
                            newrows["PATNAME"] = filterRows[k]["PATNAME"].ToString();
                            newrows["CHILDNAME"] = filterRows[k]["CHILDNAME"].ToString();
                            newrows["ID"] = filterRows[k]["ID"].ToString();
                            newrows["DEPTNAME"] = filterRows[k]["DEPTNAME"].ToString();
                            newrows["CNAME"] = filterRows[k]["CNAME"].ToString();
                            sumpoint += Double.Parse(filterRows[k]["REDPOINT"].ToString());
                            newrows["REDPOINT"] = sumpoint.ToString();
                        }
                        NewDT.Rows.Add(newrows);
                    }
                    else
                    {
                        NewDT.ImportRow(filterRows[0]);
                    }
                }
            }
            return NewDT;
        }
        /// <summary>
        /// 显示表头标题的内容
        /// </summary>
        /// <param name="gr"></param>
        private void CreatePageHeader(BrickGraphics gr)
        {
            //string format = "三峡大学仁和医院";
            //gr.Font = gr.DefaultFont;
            //gr.BackColor = Color.Transparent;
            //gr.Modifier = BrickModifier.MarginalHeader;

            //RectangleF r = new RectangleF(0, 0, 0, gr.Font.Height);
            //gr.DrawString(format, r);

            //PageInfoBrick brick = gr.DrawPageInfo(PageInfo.NumberOfTotal, format, Color.Black, r, BorderSide.None);
            //brick.Alignment = BrickAlignment.Far;
            //brick.AutoWidth = true;

            //brick = gr.DrawPageInfo(PageInfo.DateTime, string.Empty, Color.Black, r, BorderSide.None);
            //brick.Alignment = BrickAlignment.Near;
            //brick.AutoWidth = true;

        }
        #endregion

        #region  事件

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 控制Grid头部内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewAnalyse_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            Graphics g = e.Graphics;
            string caption = string.Empty;
            if (e.Column.Tag != null)
            {
                caption = e.Column.Tag.ToString();
            }
            else
            {
                caption = e.Column.Caption;
            }
            m_DrawHeadRectangle.AddBound(caption, e.Bounds);

            Rectangle rect = new Rectangle(m_DrawHeadRectangle.Bound.Location.X, m_DrawHeadRectangle.Bound.Location.Y, m_DrawHeadRectangle.Bound.Width - 1, m_DrawHeadRectangle.Bound.Height - 1);
            g.FillRectangle(new SolidBrush(Color.FromArgb(235, 236, 239)), rect);
            g.DrawRectangle(SystemPens.ControlDark, rect);
            g.DrawString(m_DrawHeadRectangle.ColumnName, this.Font, Brushes.Black, rect, sf);

            e.Handled = true;
        }
        /// <summary>
        /// 控制grid各单元格的绘制方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewAnalyse_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            //e.Appearance.BorderColor = Color.Transparent;
            //e.Appearance.BorderColor = Color.Red;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            Graphics g = e.Graphics;
            string caption = string.Empty;
            if (e.CellValue == null)
            {
                caption = "";
            }
            else
            {
                caption = e.CellValue.ToString();
            }
            m_DrawHeadRectangle.AddBound(caption, e.Bounds);
            Rectangle rect = new Rectangle(m_DrawHeadRectangle.Bound.Location.X, m_DrawHeadRectangle.Bound.Location.Y, m_DrawHeadRectangle.Bound.Width, m_DrawHeadRectangle.Bound.Height);//-2
            //g.DrawRectangle(Pens.LightBlue, rect);
            g.FillRectangle(new SolidBrush(Color.White), rect);
            //g.DrawRectangle(SystemPens.ControlDarkDark, rect);
            g.DrawString(m_DrawHeadRectangle.ColumnName, this.Font, Brushes.Black, rect, sf);
            //Point point=new Point (m_DrawHeadRectangle.Bound.X,m_DrawHeadRectangle.Bound.Y);

            g.DrawLine(Pens.Gray, m_DrawHeadRectangle.Bound.Location.X - 2, m_DrawHeadRectangle.Bound.Location.Y - 5, m_DrawHeadRectangle.Bound.Location.X - 2, m_DrawHeadRectangle.Bound.Location.Y + 200);
            e.Handled = true;
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EmrPainetScore_Load(object sender, EventArgs e)
        {
            //UcEmrPainetScore ucEmrpainet = new UcEmrPainetScore(m_App);
            //ucEmrpainet.Width = panelControl3.Width;
            //ucEmrpainet.Height = panelControl3.Height;
            //panelControl3.Controls.Add(ucEmrpainet);
            //SumPoint = Int32.Parse(m_SqlManager.GetConfigValueByKey("EmrPointConfig"));
            string status = this.patientRow["status"].ToString();
            if (status == "1501")
            { SumPoint = 85; }
            else
            { SumPoint = 100; }
            BindGridData(m_NoOfInpat);
            this.MinimizeBox = false;
            this.MaximizeBox = false;
        }

        /// <summary>
        /// 打印操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            //this.layoutControl1.ShowPrintPreview();
            //this.layoutControl1.Print();
            //layoutControl1.ExportToXls("C:\\1.xls");
            //声明PrintingSystem
            PrintingSystem ps = new DevExpress.XtraPrinting.PrintingSystem();
            DevExpress.XtraPrinting.PrintableComponentLink link = null;
            link = new DevExpress.XtraPrinting.PrintableComponentLink(ps);
            ps.Links.Add(link);
            link.Component = layoutControl1;//将layoutcontrol控件和打印组件连接
            link.CreateDocument(); //建立文档
            ps.PageSettings.PaperKind = PaperKind.A4;

            //设置表格边距
            ps.PageSettings.TopMargin = 2;
            ps.PageSettings.BottomMargin = 2;
            ps.PageSettings.LeftMargin = 2;
            ps.PageSettings.RightMargin = 2;
            //ps.PrintDlg();//调打印设置窗体
            //ps.PreviewFormEx.Show();//预览窗体 
            ps.Print();
        }

        /// <summary>
        /// 窗体大小改变触发的事件调整用户控件的大小 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EmrPainetScore_SizeChanged(object sender, EventArgs e)
        {
            ReSetUCLocaton();
        }
        #endregion
    }

    #region 控制表头类
    /// <summary>
    /// 用于控制表头
    /// </summary>
    public class DrawHeadRectangle
    {
        public Rectangle Bound;
        public string ColumnName;

        public void AddBound(string columnName, Rectangle rect)
        {
            if (string.IsNullOrEmpty(ColumnName))
            {
                Bound = rect;
                ColumnName = columnName;
            }
            else
            {
                if (ColumnName == columnName)
                {
                    Bound = new Rectangle(Bound.Location, new Size(Bound.Width + rect.Width, Bound.Height));
                }
                else
                {
                    Bound = rect;
                    ColumnName = columnName;
                }
            }
        }
    }
    #endregion
}