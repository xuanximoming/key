using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Drawing.Text;
using DrectSoft.FrameWork.WinForm.Plugin;


namespace DrectSoft.Emr.QcManager
{
    /// <summary>
    /// 病历评分表的自定义控件
    /// add by ywk 2012年4月5日16:20:22
    /// </summary>
    public partial class UcEmrPainetScore : DevExpress.XtraEditors.XtraUserControl
    {

        #region 变量
        //医院名称
        string m_FontFamily = "宋体"; //标题名称文字的字体
        int m_FontSizeHospitalName = 15;//标题名称文字的字体大小
        int m_HospitalNameY = 4; //标题名称所在Y轴方向上的距离

        //表单名称
        string m_HeaderName = "病 历 评 分 表";
        int m_HeaderNameY = 40; //Y轴方向上的距离
        int m_FontSizeHeaderName = 15;
        #endregion
        #region 属性或字段
        //private IEmrHost m_App;
        //SqlManger m_SqlManager;
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
        #endregion

        #region 绘制表格相关
        PatientInfoLocation patientInfoLocation = new PatientInfoLocation();//病人基本信息栏位所在位置
        #endregion

        #region 页面大小设定
        int m_PageWidth = 710;//用于设定PictureBox的宽度
        int m_PageHeight = 700;//用于设定PictureBox的高度
        #endregion

        #region 构造函数内，初始化界面元素
        public UcEmrPainetScore()
        {
            //m_App = app;
            InitializeComponent();
            //m_SqlManager = new SqlManger(m_App);
            pictureBoxMeasureTable.Paint += new PaintEventHandler(picture_Paint);
            LoadDataDefault();
        }
        #endregion

        #region Paint 事件，用于绘制整个用户控件
        void picture_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            PictureBox pictureBox = sender as PictureBox;

            if (pictureBox != null)
            {
                PaintScoreDocument(pictureBox, g);
            }
        }
        /// <summary>
        /// 具体开始绘制的内容
        /// </summary>
        /// <param name="pictureBox"></param>
        /// <param name="g"></param>
        private void PaintScoreDocument(PictureBox pictureBox, Graphics g)
        {
            //绘制标题
            PaintTitle(pictureBox, g);
            //绘制标题下面的科室、患者姓名、住院号等基本信息  
            PainetBaseInfo(pictureBox, g);
            //绘制表格第一行（内容，扣分，扣分理由）
            PainetFirstRow(pictureBox, g);
            //绘制下面的各个评分项的行
            PainetOtherRow(pictureBox, g);
            //画最后的其他内容（应得分，实得分等...）
            PainetLastContent(pictureBox, g);

        }
        #region 绘制表格最下方的内容
        private void PainetLastContent(PictureBox pictureBox, Graphics g)
        {

        }
        #endregion

        #region 动态判断绘制下面的各个评分项的行
        private void PainetOtherRow(PictureBox pictureBox, Graphics g)
        {
            ComputeTable computeTable = new ComputeTable();
            int SumrowCount = computeTable.GetSumRowCount();//大行的行数
            DataTable dtParentRows = EmrScoreData.GetSumRowCount();//获得大项的名称 
            for (int i = 0; i < dtParentRows.Rows.Count; i++)
            {
                //得到每个大项里相应的对应的小项的个数（即小项的行数）
                int ChildrowCount = computeTable.GetChildSocreByCode(dtParentRows.Rows[i]["ccode"].ToString()).Rows.Count;
                //if (ChildrowCount > 1)//有小项动态画表格的行 
                //{
                //    //PaintMultiyRows(ChildrowCount, g, pictureBox, dtParentRows.Rows[i]["cname"].ToString(), i);

                //    PaintMultiyRows(ChildrowCount, g, pictureBox, dtParentRows.Rows[i]["cname"].ToString());
                //}
                //else if (ChildrowCount == 1)//无小项，它自身就要成为一行
                //{
                //    //PaintSingleRows();
                //}
                int DrawedRowHeight;
                Point UpTablePoint;
                //int NewDrawedRowHeight = DrawedRowHeight;

                PaintMultiyRows(ChildrowCount, g, pictureBox, dtParentRows.Rows[i]["cname"].ToString(), out DrawedRowHeight, out UpTablePoint);
            }
        }
        /// <summary>
        /// 画多行的 , int index
        /// </summary>
        private void PaintMultiyRows(int m_childrowCount, Graphics g, PictureBox pictureBox, string cname, out int HasDrawRowHeight, out Point uptbpoint)
        {
            //HasDrawRowHeight = 25;
            //uptbpoint = new Point(10, 90);
            int myHeight = 0;
            Point myPoint = new Point(0, 0);
            if (m_childrowCount > 1)//包含多个评分项
            {
                //m_TableStartPointX = uptbpoint.X;
                //m_TableStartPointY = uptbpoint.Y;
                //m_FirstRowHeight = HasDrawRowHeight;
                //Pen pen = Pens.Black;
                //Font font = this.Font;
                //int ParentRowHeiht = m_childrowCount * m_FirstRowHeight * 2;//整个大行的高度

                ////左侧竖线(此单元格外侧竖线)
                //g.DrawLine(pen, m_TableStartPointX, m_TableStartPointY, m_TableStartPointX, ParentRowHeiht);
                ////拆分单元格的竖线
                //g.DrawLine(pen, m_TableStartPointX + 80, m_TableStartPointY + m_FirstRowHeight, m_TableStartPointX + 80, ParentRowHeiht);
                //g.DrawString(cname, font, Brushes.Black, m_TableStartPointX + 20, m_TableStartPointY + m_FirstRowHeight + 100);//画字
                ////下方横线
                //g.DrawLine(pen, m_TableStartPointX, ParentRowHeiht, pictureBox.Width - 65, ParentRowHeiht);
                ////右侧竖线
                //g.DrawLine(pen, pictureBox.Width - 65, m_TableStartPointY, pictureBox.Width - 65, ParentRowHeiht);
                ////无论是不是有效评分项。大行与小行的每列都是一样的(第一列)
                //g.DrawLine(pen, m_FirstColumnWidth, m_TableStartPointY, m_FirstColumnWidth, ParentRowHeiht);//画线
                ////第二列
                //g.DrawLine(pen, m_SecondColumnWidth, m_TableStartPointY, m_SecondColumnWidth, ParentRowHeiht);//画线

                //Point LeftXLocation = new Point(m_TableStartPointX, m_TableStartPointY);
                //myHeight = ParentRowHeiht;
                //myPoint = LeftXLocation;
            }
            else//画单个评分项行的
            {
                //m_TableStartPointX = uptbpoint.X;
                //m_TableStartPointY = uptbpoint.Y;
                //m_FirstRowHeight = HasDrawRowHeight;

                Pen pen = Pens.Black;
                Font font = this.Font;
                int m_SingleRowPointX = 10;
                int m_SingleRowPointY = 100;
                int SingleRowHeight = 50;
                //左侧竖线(此单元格外侧竖线)
                g.DrawLine(pen, m_SingleRowPointX, m_SingleRowPointY, m_SingleRowPointX, m_SingleRowPointY + SingleRowHeight);
                //拆分单元格的竖线
                g.DrawLine(pen, m_SingleRowPointX + 220, m_SingleRowPointY + SingleRowHeight, m_SingleRowPointX + 220, SingleRowHeight + 50);
                g.DrawString(cname, font, Brushes.Black, m_SingleRowPointX + 20, SingleRowHeight + 80);//画字
                //右侧竖线
                g.DrawLine(pen, pictureBox.Width - 65, m_SingleRowPointY, pictureBox.Width - 65, m_SingleRowPointY + SingleRowHeight);
                //下方横线
                g.DrawLine(pen, m_SingleRowPointX, m_SingleRowPointY + 50, pictureBox.Width - 65, m_SingleRowPointY + 50);

                Point LeftXLocation = new Point(m_SingleRowPointX, m_SingleRowPointY);
                myHeight = SingleRowHeight;
                myPoint = LeftXLocation;
            }

            HasDrawRowHeight = myHeight;
            uptbpoint = myPoint;
        }

        #endregion
        #region 表格的相关参数的设置
        int m_TableStartPointX = 10;  //表格左上角的X轴方向上的坐标
        int m_TableStartPointY = 90; //表格左上角的Y轴方向上的坐标
        int m_FirstColumnWidth = 230;//表格第一列的宽度--内容列
        int m_SecondColumnWidth = 230 + 80;//第二列宽度（横坐标）
        int m_FirstRowHeight = 25;//表格每行的高度（小评分的行高度）
        int m_TableRowsCount = 3;//表格的列数3列
        #endregion

        #region 绘制表格第一行
        private void PainetFirstRow(PictureBox pictureBox, Graphics g)
        {
            //Pen penBold = new Pen(Color.Black,1);
            Pen pen = Pens.Black;
            Font font = this.Font;
            //第一行上方横线
            g.DrawLine(pen, m_TableStartPointX, m_TableStartPointY, pictureBox.Width - 65, m_TableStartPointY);
            //第一行下方横线
            g.DrawLine(pen, m_TableStartPointX, m_TableStartPointY + 25, pictureBox.Width - 65, m_TableStartPointY + 25);
            //左侧的竖线
            g.DrawLine(pen, m_TableStartPointX, m_TableStartPointY, m_TableStartPointX, m_TableStartPointY + m_FirstRowHeight);
            //右侧的竖线
            g.DrawLine(pen, pictureBox.Width - 65, m_TableStartPointY, pictureBox.Width - 65, m_TableStartPointY + m_FirstRowHeight);

            //画第一列（内容）
            g.DrawLine(pen, m_FirstColumnWidth, m_TableStartPointY, m_FirstColumnWidth, m_TableStartPointY + m_FirstRowHeight);//画线
            g.DrawString("内容", font, Brushes.Black, m_TableStartPointX + 90, m_TableStartPointY + 6);//画字
            //画第二列（扣分）
            g.DrawLine(pen, m_SecondColumnWidth, m_TableStartPointY, m_SecondColumnWidth, m_TableStartPointY + m_FirstRowHeight);//画线
            g.DrawString("扣分", font, Brushes.Black, m_TableStartPointX + 90 + 155, m_TableStartPointY + 6);//画字
            //画第三列（扣分理由）
            g.DrawString("扣分理由", font, Brushes.Black, m_TableStartPointX + 90 + 155 + 185, m_TableStartPointY + 6);//画字
        }
        #endregion

        #endregion

        #region 绘制标题部分
        private void PaintTitle(PictureBox pictureBox, Graphics g)
        {
            //医院名称
            FontFamily headFontFamily = new FontFamily(m_FontFamily);
            Font font = new Font(headFontFamily, m_FontSizeHospitalName, FontStyle.Bold);
            g.DrawString(HospitalName, font, Brushes.Black, (pictureBox.Width - TextRenderer.MeasureText(m_HospitalName, font).Width) / 2 - 35, m_HospitalNameY);

            //标题，“病历评分表”
            font = new Font(headFontFamily, m_FontSizeHeaderName, FontStyle.Bold);
            g.DrawString(m_HeaderName, font, Brushes.Black, (pictureBox.Width - TextRenderer.MeasureText(m_HeaderName, font).Width) / 2 - 35, m_HeaderNameY);
        }
        #endregion

        #region 绘制标题下方的基本信息部分
        private void PainetBaseInfo(PictureBox pictureBox, Graphics g)
        {
            Font font = this.Font;
            string str = "科室:";
            g.DrawString(str, font, Brushes.Black, patientInfoLocation.DeptNameX, patientInfoLocation.InpatientInformationY);
            g.DrawString(EmrPointInfo.DeptName, font, Brushes.Black, TextRenderer.MeasureText(str, font).Width + patientInfoLocation.DeptNameX, patientInfoLocation.InpatientInformationY);

            str = "患者姓名:";
            g.DrawString(str, font, Brushes.Black, patientInfoLocation.PainetNameX, patientInfoLocation.InpatientInformationY);
            g.DrawString(EmrPointInfo.InpatientName, font, Brushes.Black, TextRenderer.MeasureText(str, font).Width + patientInfoLocation.PainetNameX, patientInfoLocation.InpatientInformationY);

            str = "住院号:";
            g.DrawString(str, font, Brushes.Black, patientInfoLocation.InpatientNoX, patientInfoLocation.InpatientInformationY);
            g.DrawString(EmrPointInfo.InpatientNo, font, Brushes.Black, TextRenderer.MeasureText(str, font).Width + patientInfoLocation.InpatientNoX, patientInfoLocation.InpatientInformationY);

            str = "住院医师:";
            g.DrawString(str, font, Brushes.Black, patientInfoLocation.ResidentDocX, patientInfoLocation.InpatientInformationY);
            g.DrawString(EmrPointInfo.ResidentDoc, font, Brushes.Black, TextRenderer.MeasureText(str, font).Width + patientInfoLocation.ResidentDocX, patientInfoLocation.InpatientInformationY);

            str = "上级医师:";
            g.DrawString(str, font, Brushes.Black, patientInfoLocation.ChiefDocX, patientInfoLocation.InpatientInformationY);
            g.DrawString(EmrPointInfo.ChiefDoc, font, Brushes.Black, TextRenderer.MeasureText(str, font).Width + patientInfoLocation.ChiefDocX, patientInfoLocation.InpatientInformationY);

        }
        #endregion
        #region 方法
        /// <summary>
        /// 设置默认加载的页面元素
        /// </summary>
        private void LoadDataDefault()
        {
            if (this.DesignMode == true)
            {
                return;
            }
            PictureBox picture = this.pictureBoxMeasureTable;
            this.Font = new Font("宋体", 9.0f, FontStyle.Regular);
            picture.Width = m_PageWidth;
            picture.Height = m_PageHeight;
            picture.BackColor = Color.White;
            picture.Location = new Point(10, 10);
        }

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
        /// 根据查出的数据跟病人实体赋值
        /// </summary>
        /// <param name="paientInfo"></param>
        public void SetPatientInfo(DataTable dtpaientInfo)
        {
            if (dtpaientInfo.Rows.Count > 0)
            {
                EmrPointInfo emrPointInfo = new EmrPointInfo();
                emrPointInfo.InitPatientInfo(dtpaientInfo);
            }
        }
        /// <summary>
        /// 加载数据
        /// </summary>
        public void LoadData()
        {
            throw new NotImplementedException();
        }
        #endregion



    }
    #region 用于动态计算表格行数列数的类
    public class ComputeTable
    {
        /// <summary>
        /// 用于返回计算的有几个大类别的行（住院志，病程记录等...）
        /// </summary>
        /// <returns></returns>
        public int GetSumRowCount()
        {
            int allrowcount = 0;
            DataTable dt = QcManager.EmrScoreData.GetSumRowCount();
            if (dt.Rows.Count > 0)
            {
                allrowcount = dt.Rows.Count;
            }
            return allrowcount;
        }
        /// <summary>
        /// 根据大项的名称得到评分小项的相关数据信息
        /// </summary>
        /// <param name="ccode"></param>
        /// <returns></returns>
        public DataTable GetChildSocreByCode(string ccode)
        {
            DataTable dt = EmrScoreData.GetChildPointByCode(ccode);
            return dt;
        }
    }
    #endregion

}
