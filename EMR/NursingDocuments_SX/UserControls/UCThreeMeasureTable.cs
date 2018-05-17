/*
 * 【1】修改人员：wwj 2012-07-24
 *      解决泗县中医院中三测单病人同一时间点多个状态的问题
 *
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using System.Drawing.Text;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections;
using DevExpress.XtraEditors;
using DrectSoft.Resources;
using DrectSoft.Core.NursingDocuments.UserControls;
using System.Drawing.Printing;
using System.IO;
using DevExpress.Utils;
using System.Xml;
using DrectSoft.Core.NursingDocuments.PublicSet;

namespace DrectSoft.Core.NursingDocuments.UserControls
{
    public partial class UCThreeMeasureTable : DevExpress.XtraEditors.XtraUserControl
    {
        #region 枚举
        /// <summary>
        /// 生命体征曲线图中需要测量的类型
        /// </summary>
        public enum VitalSignsType
        {
            TiWen = 1,          //体温
            MaiBo = 2,          //脉搏
            HuXi = 3,           //呼吸
            XinLv = 4,          //心率
            WuLiJiangWen = 5,   //物理降温
            XueYa = 6,          //血压
            ZongRuLiang = 7,    //总入量
            ZongChuLiang = 8,   //总出量
            YinLiuLiang = 9,    //引流量
            DaBianCiShu = 10,    //大便次数
            ShenGao = 11,       //身高
            TiZhong = 12,       //体重
            GuoMingYaoWu = 13,  //过敏药物
            TeShuZhiLiao = 14,  //特殊治疗
            //Other1 = 15,        //其他1
            PainInfo = 15,        //其他1 改为 疼痛栏位
            Other2 = 16,        //其他2
            //add by ywk 
            WuLiShengWen = 17       //其他2  更改为 物理升温

            //PaiChuLiang=18//排出量(包含大便次数，尿量，和一个空格行)ywk 2012年5月17日 16:41:00


        }

        /// <summary>
        /// 体温测量的方式
        /// </summary>
        public enum VitalSignsTiWenType
        {
            //1: 口温  2: 腋温 3: 肛温
            KouWen = 8802,
            YeWen = 8801,
            GangWen = 8803
        }

        /// <summary>
        /// 线段类型
        /// </summary>
        public enum LineType
        {
            SolidLine = 1, //直线
            DashLine = 2   //虚线
        }

        #endregion

        #region 变量

        #region 标题部分设定
        //医院名称
        string m_FontFamily = "宋体"; //标题名称文字的字体
        int m_FontSizeHospitalName = 18;//标题名称文字的字体大小
        int m_HospitalNameY = 24; //标题名称所在Y轴方向上的距离20

        //表单名称
        string m_HeaderName = "体  温  单";
        int m_HeaderNameY = 65; //Y轴方向上的距离//60
        int m_FontSizeHeaderName = 18;

        //病人基本信息
        DataTable m_DataTableTableBaseLine = new DataTable(); //体温表上部基本行的数据，即小方格曲线行以上行的数据
        #endregion

        #region 页面大小设定
        int m_PageWidth = 840;//用于设定PictureBox的宽度
        int m_PageHeight = 1160;//用于设定PictureBox的高度 1160
        #endregion

        #region 表格参数设定

        int m_TableStartPointX = 10;  //表格左上角的X轴方向上的坐标
        int m_TableStartPointY = 120; //表格左上角的Y轴方向上的坐标120

        int m_FirstColumnWidth = 100;//表格第一列的宽度【也就是“日期”这一列的宽度】
        int m_FirstColumnHasSubColumnCount = 0;//表示生命体征，即“时间”下的一行第一列有几个子列, 根据m_ArrayListVitalSigns.count来决定【默认包括3列：体温、脉搏、呼吸】

        int m_LineHeight1 = 20;//除生命体征外其他地方的行高【即曲线图以外的表格的行高】20

        int m_Days = 7;//体温表一次显示几天【默认显示一周的数据】

        int m_DayTimePoint = 6; //每天的时间点数,默认为6个时间点【默认情况下时间点是 2，6，10，14，18，22 所以每天的时间点数为6 *****此处时间点在数据库中配置******】
        DataTable m_DataTableDayTimePoint = new DataTable();//每天具体的时间点【默认情况下时间点是 2，6，10，14，18，22 *****此处时间点在数据库中配置******】
        int m_LineHeight2 = 14;//生命体征每小格高度（由于是正方形所以高度和宽度相等）【即曲线图中每一小格的高度、宽度】

        int m_CellCount = 45; //记录曲线图中Y轴方向一共有多少个小格子--------------40
        int m_CellCountInEveryDegree = 5; //两条粗的黑线之间有多少个小方格

        ArrayList m_ArrayListVitalSigns = new ArrayList(); //需要显示在曲线图中的体征【通过此变量来决定哪些体征应该显示在曲线图中,默认情况包括：呼吸、脉搏、体温】
        ArrayList m_ArrayListOther = new ArrayList();//体温表底部区域有哪些行，即小方格曲线行以下的行（包括：血压，入量，出量等），不包括呼吸的行，呼吸的资料保存在PatientInfo.DataTableHuXi表中）

        int m_DayTimePointXuYa = 2;//每天血压的时间点数【每天测量血压的次数】

        DateTime m_DataTimeAllocate = System.DateTime.Now;//可以指定某一天，一般设置系统当前时间，与病人的入院时间进行比较，得到体温表第一行日期的值
        DataTable m_DateTimeEveryColumnDateTime = new DataTable();//保存体温表中每列显示的时间【每份体温表的第一天都显示“年-月-日”,包含下个月的日期时显示“月-日”，其他显示“日”】

        //float m_PaintEventInformationPosition = 42f; //患者入院、转入、手术、分娩、出院、死亡等信息填写的位置【默认为从43度开始的位置向下开始写】

        int m_RowCaptionIndent =10; //生命体征曲线图下方，每行标题的缩进值 Add By wwj 2012-05-15

        int m_ID = 0;//用于表示在绘制各个点时，为每个点分配的唯一ID,方便查找VitalSignsPosition【曲线图中每个节点都会分配一个ID，用于唯一标示】
        ArrayList m_ArrayListPoint = new ArrayList();//曲线图中各个点的坐标
        ArrayList m_ArrayListPointLine = new ArrayList();//曲线图中各个点之间的连线

        int m_Distance = 2;//相距10个像素的时候将两个点认为是合并到一起
        ArrayList m_ArrayListConflictPoint = new ArrayList();//曲线图中有重合的点

        float m_TableHeight = 0;//体温表的总高度

        bool m_IsShowTiWen = true; //是否显示体温,用于CheckBox
        bool m_IsShowMaiBo = true; //是否显示脉搏,用于CheckBox
        bool m_IsShowHuXi = true;  //是否显示呼吸,用于CheckBox


        PatientInfoLocation patientInfoLocation = new PatientInfoLocation();//病人基本信息栏位所在位置

        Picture m_Picture = new Picture();//所要绘制的图片设定

        bool m_IsComputeLocation = false;//表示是否计算过曲线图中点的坐标

        List<DataRow> m_EventSetting;//事件设置，包括是否显示时间、显示位置
        #endregion

        #endregion

        #region Property && Field

        string m_HospitalName = "";
        private string HospitalName
        {
            get
            {
                if (string.IsNullOrEmpty(m_HospitalName))
                {
                    //得到医院信息
                    GetHospitalName();
                }
                return m_HospitalName;
            }
        }

        private string m_Times = "";
        /// <summary>
        /// 获得体征录入的时间点，动态获得 ywk 
        /// </summary>
        public string[] TimesArray
        {
            get
            {
                if (string.IsNullOrEmpty(m_Times))
                {
                    m_Times = PublicSet.MethodSet.GetConfigValueByKey("VITALSIGNSRECORDTIME");
                }
                string[] timeArray = m_Times.Split(',');
                return timeArray;
            }

        }

        //Add by wwj 2012-06-05 根据配置决定栏位的显示

        /// <summary>
        /// 默认打印A4纸
        /// </summary>
        private string m_DefaultPrintSize = "A4";

        /// <summary>
        /// 默认在曲线图中显示 呼吸的显示样式 1.曲线图中显示 2.非曲线图中显示
        /// </summary>
        private string m_HuXiShowType = "1";

        /// <summary>
        /// 默认显示疼痛指数
        /// </summary>
        private string m_IsShowTengTongZhiShu = "1";

        /// <summary>
        /// 默认不是12小时制
        /// </summary>
        private string m_Is12小时制 = "0";

        /// <summary>
        /// 默认第一个呼吸显示在上方 1：上方 0：下方
        /// </summary>
        private string m_FirstHuXiUp = "1";

        /// <summary>
        /// 病人的某个状态导致曲线图节点断开的情况 
        /// 护士在绘制三测单曲线时
        /// 如果病人在某一时间点不在，那么这一点的生命体征就不予填写，在绘制三测单曲线时将断开这一点（即此点的前后两点不能连接在一起）
        /// 比如：2、6、10三点，如果6点没有生命体征，那么2、10两点不予连接。
        /// </summary>
        private List<string> m_断开状态 = new List<string>();

        /// <summary>
        /// 记录需要断开连线的X方向上的坐标
        /// </summary>
        private List<int> m_Need断开LocationX = new List<int>();

        /// <summary>
        /// 初始化三测单配置 Add by wwj 2012-06-05 根据配置决定栏位的显示
        /// </summary>
        public void InitThreeMeasureTableSetting()
        {
            string setting = PublicSet.MethodSet.GetConfigValueByKey("ThreeMeasureTableSetting");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(setting);
            m_DefaultPrintSize = doc.GetElementsByTagName("DefaultPageSize")[0].InnerText.Trim();
            //m_HuXiShowType = doc.GetElementsByTagName("HuXiShowType")[0].InnerText.Trim();
            m_IsShowTengTongZhiShu = doc.GetElementsByTagName("TengTongZhiShuShow")[0].InnerText.Trim();
            m_Is12小时制 = doc.GetElementsByTagName("Is12小时制")[0].InnerText.Trim();
            m_FirstHuXiUp = doc.GetElementsByTagName("FirstHuXiUp")[0].InnerText.Trim();
            string[] statuses = doc.GetElementsByTagName("断开状态")[0].InnerText.Trim().Split(',');
            m_断开状态.Clear();
            foreach (string status in statuses) m_断开状态.Add(status);
        }
        #endregion

        #region 构造函数, 界面初始化Load
        public UCThreeMeasureTable()
        {
            InitializeComponent();
            pictureBoxMeasureTable.Paint += new PaintEventHandler(picture_Paint);
            LoadDataDefault();
        }

        public void LoadDataDefault()
        {
            try
            {
                if (this.DesignMode == true)
                {
                    return;
                }
                m_IsComputeLocation = true;

                PictureBox picture = this.pictureBoxMeasureTable;
                ThreeMeasureTableConfig threeMeasureTableConfig = new ThreeMeasureTableConfig();

                this.Font = new Font("宋体", 9.0f, FontStyle.Regular);

                picture.Width = m_PageWidth;
                picture.Height = m_PageHeight;
                picture.BackColor = Color.White;
                picture.Location = new Point(10, 10);

                //决定“度量值”以上有哪些行
                m_DataTableTableBaseLine = threeMeasureTableConfig.InitTableHead();

                //初始化每天时间点的信息
                m_DataTableDayTimePoint = threeMeasureTableConfig.GetTimePointDefault();
                m_DayTimePoint = m_DataTableDayTimePoint.Rows.Count;

                //初始化 呼吸，脉搏， 体温 等生命体征的设置数据
                m_ArrayListVitalSigns = threeMeasureTableConfig.InitFirstColumnHasSubColumn();
                m_FirstColumnHasSubColumnCount = m_ArrayListVitalSigns.Count;

                //初始化 血压，总入量，总出量，引流量，大便次数，身高，体重，过敏药物的设置数据
                m_ArrayListOther = threeMeasureTableConfig.InitVitalSignsOtherAtTableBottom(m_DayTimePointXuYa);

                //动态生成Button
                CreateCheckBox();
            }
            catch (Exception e)
            {
                //todo  log ex

            }
        }

        public void LoadData()
        {
            if (this.DesignMode == true)
            {
                return;
            }

            //表示在绘制曲线的时候重新计算曲线的坐标点
            m_IsComputeLocation = false;

            PictureBox picture = this.pictureBoxMeasureTable;
            ThreeMeasureTableConfig threeMeasureTableConfig = new ThreeMeasureTableConfig();

            this.Font = new Font("宋体", 9.0f, FontStyle.Regular);

            picture.Width = m_PageWidth;
            picture.Height = m_PageHeight;
            picture.BackColor = Color.White;
            picture.Location = new Point(80, 10);

            //初始化三测单配置
            InitThreeMeasureTableSetting();

            //初始化病人数据源
            PatientInfo patientInfo = new PatientInfo();
            patientInfo.InitData();


            //给病人数据设置数值
            DataTable dataTimeForColumns = GetDateTimeForColumns(Convert.ToDateTime(PatientInfo.InTime), m_DataTimeAllocate);
            if (dataTimeForColumns.Rows.Count > 0)
            {
                patientInfo.GetNursingRecordByDate(dataTimeForColumns.Rows[0][0].ToString(),
                    dataTimeForColumns.Rows[dataTimeForColumns.Rows.Count - 1][0].ToString());
            }

            //决定“度量值”以上有哪些行
            m_DataTableTableBaseLine = threeMeasureTableConfig.InitTableHead();

            //初始化每天时间点的信息
            m_DataTableDayTimePoint = PublicSet.MethodSet.GetTimePoint();
            m_DayTimePoint = m_DataTableDayTimePoint.Rows.Count;

            //初始化 呼吸，脉搏， 体温 等生命体征的设置数据
            m_ArrayListVitalSigns = threeMeasureTableConfig.InitFirstColumnHasSubColumn();
            if (m_HuXiShowType == "2")//根据配置决定呼吸显示的位置
            {
                foreach (VitalSigns vs in m_ArrayListVitalSigns)
                {
                    if (vs.Name == UCThreeMeasureTable.VitalSignsType.HuXi.ToString())
                    {
                        m_ArrayListVitalSigns.Remove(vs);
                        break;
                    }
                }
            }
            m_FirstColumnHasSubColumnCount = m_ArrayListVitalSigns.Count;


            //初始化 血压，总入量，总出量，引流量，大便次数，身高，体重，过敏药物的设置数据
            m_ArrayListOther = threeMeasureTableConfig.InitVitalSignsOtherAtTableBottom(m_DayTimePointXuYa);

            //得到不需要显示时间的事件，如手术、外出、请假等不需要显示具体时间
            m_EventSetting = PublicSet.MethodSet.GetNotShowTimeEvent();

            //动态生成Button
            CreateCheckBox();

        }

        #region 动态生成Button
        /// <summary>
        /// 动态生成Button
        /// </summary>
        private void CreateCheckBox()
        {
            int xPoint = m_TableStartPointX + m_FirstColumnWidth + 1 + m_DayTimePoint * (m_LineHeight2 + 1) * m_Days + 14;

            for (int i = 0; i < m_ArrayListVitalSigns.Count; i++)
            {
                VitalSigns vitalSigns = m_ArrayListVitalSigns[i] as VitalSigns;
                if (vitalSigns != null)
                {
                    DevExpress.XtraEditors.CheckEdit checkEdit = new CheckEdit();
                    checkEdit.Location = new Point(xPoint, 200 + 40 * i);
                    checkEdit.Name = "checkEdit1";
                    checkEdit.Properties.Caption = GetVitalSignsTypeName(vitalSigns.Name);
                    checkEdit.Size = new System.Drawing.Size(75, 19);
                    checkEdit.Checked = true;
                    checkEdit.CheckedChanged += new EventHandler(checkEdit_CheckedChanged);
                    checkEdit.BringToFront();
                    this.pictureBoxMeasureTable.Controls.Add(checkEdit);
                }
            }
        }
        #endregion

        void checkEdit_CheckedChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.CheckEdit checkEdit = sender as DevExpress.XtraEditors.CheckEdit;
            if (checkEdit != null)
            {
                if (checkEdit.Text == "体温")
                {
                    m_IsShowTiWen = checkEdit.Checked;
                }
                else if (checkEdit.Text == "呼吸")
                {
                    m_IsShowHuXi = checkEdit.Checked;
                }
                else if (checkEdit.Text == "脉搏")
                {
                    m_IsShowMaiBo = checkEdit.Checked;
                }
            }
            pictureBoxMeasureTable.Refresh();
        }

        #endregion

        #region Paint 事件，用于绘制整个用户控件
        void picture_Paint(object sender, PaintEventArgs e)
        {
            #region 开始绘图之前先清空用于记录的变量
            m_ID = 0;
            #endregion

            Graphics g = e.Graphics;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            PictureBox pictureBox = sender as PictureBox;

            if (pictureBox != null)
            {
                PaintNurseDocument(pictureBox, g);
            }
        }

        private void PaintNurseDocument(PictureBox pictureBox, Graphics g)
        {
            //绘制标题
            PaintTitle(pictureBox, g);

            //绘制病人基本信息
            PaintPatientBaseInformation(pictureBox, g);

            //绘制表格的第一部分(小方格曲线图区域上面的部分)
            PaintFirstPartOfTable(pictureBox, g);

            //绘制表格的第二部分(小方格曲线图区域中)
            PaintSecondPartOfTable(pictureBox, g);

            //绘制表格的第三部分(小方格曲线图区域下面的部分)
            PaintThirdPartOfTable(pictureBox, g);

            //绘制提示部分
            //PaintImagePrompt(pictureBox, g);
            PaintImagePromptBottom(pictureBox, g);
        }
        #endregion

        #region 表格上面的部分

        #region 绘制标题
        /// <summary>
        /// 绘制标题
        /// </summary>
        /// <param name="pictureBox"></param>
        /// <param name="g"></param>
        private void PaintTitle(PictureBox pictureBox, Graphics g)
        {
            //医院名称，“某某医院”
            FontFamily headFontFamily = new FontFamily(m_FontFamily);
            Font font = new Font(headFontFamily, m_FontSizeHospitalName, FontStyle.Regular);
            g.DrawString(HospitalName, font, Brushes.Black, (pictureBox.Width - TextRenderer.MeasureText(m_HospitalName, font).Width) / 2 - 35, m_HospitalNameY);

            //标题，“体温表”
            font = new Font(headFontFamily, m_FontSizeHeaderName, FontStyle.Bold);
            g.DrawString(m_HeaderName, font, Brushes.Black, (pictureBox.Width - TextRenderer.MeasureText(m_HeaderName, font).Width) / 2 - 35, m_HeaderNameY);

        }
        #endregion

        #region 绘制病人基本信息（位于标题的下面和表格的上面）
        /// <summary>
        /// 绘制病人基本信息（位于标题的下面和表格的上面）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="g"></param>
        private void PaintPatientBaseInformation(PictureBox pictureBox, Graphics g)
        {
            Font font = this.Font;
            string str = "姓名:";
            g.DrawString(str, font, Brushes.Black, patientInfoLocation.InpatientNameX, patientInfoLocation.InpatientInformationY);
            g.DrawString(PatientInfo.InpatientName, font, Brushes.Black, TextRenderer.MeasureText(str, font).Width + patientInfoLocation.InpatientNameX, patientInfoLocation.InpatientInformationY);

            str = "年龄:";//针对婴儿的三测单的显示，姓名栏位加宽 eidt by ywk 2012年8月30日 08:59:28
            g.DrawString(str, font, Brushes.Black, patientInfoLocation.AgeX+8, patientInfoLocation.InpatientInformationY);
            g.DrawString(PatientInfo.Age, font, Brushes.Black, TextRenderer.MeasureText(str, font).Width + patientInfoLocation.AgeX, patientInfoLocation.InpatientInformationY);

            str = "性别:";
            g.DrawString(str, font, Brushes.Black, patientInfoLocation.GenderX, patientInfoLocation.InpatientInformationY);
            g.DrawString(PatientInfo.Gender, font, Brushes.Black, TextRenderer.MeasureText(str, font).Width + patientInfoLocation.GenderX, patientInfoLocation.InpatientInformationY);

            str = "科别:";
            g.DrawString(str, font, Brushes.Black, patientInfoLocation.SectionX, patientInfoLocation.InpatientInformationY);
            g.DrawString(PatientInfo.Section, font, Brushes.Black, TextRenderer.MeasureText(str, font).Width + patientInfoLocation.SectionX, patientInfoLocation.InpatientInformationY);

            str = "床号:";
            g.DrawString(str, font, Brushes.Black, patientInfoLocation.BedCodeX, patientInfoLocation.InpatientInformationY);
            g.DrawString(PatientInfo.BedCode, font, Brushes.Black, TextRenderer.MeasureText(str, font).Width + patientInfoLocation.BedCodeX, patientInfoLocation.InpatientInformationY);

            str = "入院日期:";
            g.DrawString(str, font, Brushes.Black, patientInfoLocation.InTimeX, patientInfoLocation.InpatientInformationY);
            g.DrawString(PatientInfo.InTime.Split(' ')[0], font, Brushes.Black, TextRenderer.MeasureText(str, font).Width + patientInfoLocation.InTimeX, patientInfoLocation.InpatientInformationY);


            //如果是婴儿这里显示的婴儿母亲的住院病历号，而不是数据库中存储的GUID编码 add by ywk 2012年7月30日 13:43:08 
            if (PatientInfo.IsBaby == "1")//如果是婴儿
            {
                str = "住院病历号:";
                g.DrawString(str, font, Brushes.Black, patientInfoLocation.InpatientNoX, patientInfoLocation.InpatientInformationY);
                string babyPatid = PublicSet.MethodSet.GetPatData(PatientInfo.Mother).Rows[0]["Patid"].ToString();
                g.DrawString(babyPatid, font, Brushes.Black, TextRenderer.MeasureText(str, font).Width + patientInfoLocation.InpatientNoX, patientInfoLocation.InpatientInformationY);
            }
            else
            {
                str = "住院病历号:";
                g.DrawString(str, font, Brushes.Black, patientInfoLocation.InpatientNoX, patientInfoLocation.InpatientInformationY);
                g.DrawString(PatientInfo.InpatientNo, font, Brushes.Black, TextRenderer.MeasureText(str, font).Width + patientInfoLocation.InpatientNoX, patientInfoLocation.InpatientInformationY);
            }
        }
        #endregion

        #endregion

        #region 绘制表格部分

        #region 绘制表格的第一部分（即体温表中小方格子上面的几行，一般包括：日期、住院天数、手术后天数、时间等，需要根据m_TableBaseLineCount来决定）
        /// <summary>
        /// 绘制表格的第一部分（即体温表中小方格子上面的几行，一般包括：日期、住院天数、手术后天数、时间等，需要根据m_TableBaseLineCount来决定）
        /// </summary>
        /// <param name="pictureBox"></param>
        /// <param name="g"></param>
        private void PaintFirstPartOfTable(PictureBox pictureBox, Graphics g)
        {
            Pen penBold = new Pen(Color.Black, 2);
            Pen pen = Pens.Black;
            Font font = this.Font;

            //此宽度不包括表格最左边1像素，包括表格最右边2像素
            int tableWidth = m_FirstColumnWidth/*第一列的宽度*/ + 1 + (m_LineHeight2 * m_DayTimePoint + m_DayTimePoint) * m_Days + 1;

            //记录下“时间”下面一条横线的位置信息
            int currentLinePointY = m_TableStartPointY + (m_LineHeight1 + 1) * m_DataTableTableBaseLine.Rows.Count; //记录下当前横线的Y轴上的值
            int cruuentLineEndLinePintX = m_TableStartPointX + tableWidth; //记录下横线终点X轴方向的值

            #region 绘制横线和竖线

            //绘制横线
            g.DrawLine(penBold, m_TableStartPointX, m_TableStartPointY, m_TableStartPointX + tableWidth, m_TableStartPointY);//绘制表格从上到下的第一条横线
            for (int i = 1; i <= m_DataTableTableBaseLine.Rows.Count; i++)
            {
                int pointY = m_TableStartPointY + (m_LineHeight1 + 1) * i;
                g.DrawLine(pen, m_TableStartPointX, pointY, m_TableStartPointX + tableWidth, pointY);
            }

            //绘制竖线

            #region 计算出表格的高度
            float tableHeight = m_DataTableTableBaseLine.Rows.Count * (m_LineHeight1 + 1) +
                (m_CellCount / m_CellCountInEveryDegree) * ((m_LineHeight2 + 1) * m_CellCountInEveryDegree + 1) +
                (m_CellCount % m_CellCountInEveryDegree) * (m_LineHeight2 + 1) +
                m_ArrayListOther.Count * (m_LineHeight1)+15;//+1 ywk    23


            //判断呼吸是以一行显示，还是在曲线图中表示出来，如果以一行显示，这里要加上呼吸这一行的高度
            if (checkIsContainHuXiInCurve() == false)
            {
                tableHeight += m_LineHeight2 * 2 + 1;//+1
            }

            m_TableHeight = tableHeight;
            #endregion


            g.DrawLine(penBold, m_TableStartPointX, m_TableStartPointY, m_TableStartPointX, m_TableStartPointY + tableHeight);//绘制表格从左到右的第一条竖线
            for (int i = 0; i < m_Days; i++)
            {
                int pointX = m_TableStartPointX + m_FirstColumnWidth + 1 + (m_DayTimePoint * m_LineHeight2 + m_DayTimePoint) * i;
                g.DrawLine(pen, pointX, m_TableStartPointY, pointX, m_TableStartPointY + tableHeight);
            }

            //绘制表格从左到右的最后一条竖线
            g.DrawLine(penBold, m_TableStartPointX + tableWidth, m_TableStartPointY, m_TableStartPointX + tableWidth, m_TableStartPointY + tableHeight);


            //绘制时间点一行的竖线
            if (CheckIsNeed(m_DataTableTableBaseLine, "时   间") != -1)
            {
                //如果有时间这一行，则将每天的各个时间点显示出来
                for (int i = 0; i < m_DayTimePoint * m_Days; i++)
                {
                    float xValue = m_TableStartPointX + m_FirstColumnWidth + 1 + (m_LineHeight2 + 1) * i;
                    float yValue = m_TableStartPointY + (m_DataTableTableBaseLine.Rows.Count - 1) * (1 + m_LineHeight1);
                    g.DrawLine(Pens.Black, xValue, yValue, xValue, yValue + m_LineHeight1);
                }
            }
            #endregion

            #region 绘制文字和数字

            //根据m_DataTableTableBaseLine在表格中居中显示如“日期”，“住院天数”， “术后天数”，“时间”等
            StringFormat centerFormat = new StringFormat();
            centerFormat.Alignment = StringAlignment.Center;//横向居中
            centerFormat.LineAlignment = StringAlignment.Center;//纵向居中

            for (int i = 0; i < m_DataTableTableBaseLine.Rows.Count; i++)
            {
                g.DrawString(m_DataTableTableBaseLine.Rows[i][0].ToString(), font, Brushes.Black,
                    new RectangleF(m_TableStartPointX + 1, m_TableStartPointY + 1 + (m_LineHeight1 + 1) * i, m_FirstColumnWidth, m_LineHeight1 + 2), centerFormat);
            }

            //绘制 日期行 中的数字
            int rowIndex = 0;
            if (CheckIsNeed(m_DataTableTableBaseLine, "日   期") != -1)
            {
                m_DateTimeEveryColumnDateTime = GetDateTimeForColumns(Convert.ToDateTime(PatientInfo.InTime), m_DataTimeAllocate);

                if (m_DateTimeEveryColumnDateTime.Rows.Count > 0)
                {
                    for (int i = 0; i < m_Days; i++)
                    {
                        RectangleF rect = new RectangleF(m_TableStartPointX + m_FirstColumnWidth + 1 + (m_LineHeight2 + 1) * m_DayTimePoint * i + 1,
                            m_TableStartPointY + 1, (m_LineHeight2 + 1) * m_DayTimePoint, m_LineHeight1 + 1);
                        g.DrawString(m_DateTimeEveryColumnDateTime.Rows[i][1].ToString(), font, Brushes.Black, rect, centerFormat);
                    }
                }

                rowIndex++;
            }

            //绘制 住院天数 中的数字
            if (CheckIsNeed(m_DataTableTableBaseLine, "住院天数") != -1)
            {
                //if (m_DateTimeEveryColumnDateTime.Rows.Count > 0)
                //{
                //    for (int i = 0; i < m_Days; i++)
                //    {
                //        RectangleF rect = new RectangleF(m_TableStartPointX + m_FirstColumnWidth + 1 + (m_LineHeight2 + 1) * m_DayTimePoint * i + 1,
                //            m_TableStartPointY + m_LineHeight1 + 1 + 1, (m_LineHeight2 + 1) * m_DayTimePoint, m_LineHeight1 + 1);

                //        int inDays = (Convert.ToDateTime(m_DateTimeEveryColumnDateTime.Rows[i][0]) - Convert.ToDateTime(PatientInfo.InTime.Split(' ')[0])).Days + 1;
                //        g.DrawString(inDays.ToString(), font, Brushes.Black, rect, centerFormat);
                //    }
                //}

                foreach (DataRow dr in PatientInfo.DataTableDayOfHospital.Rows)
                {
                    DateTime dateTime = Convert.ToDateTime(dr["DateTime"].ToString());
                    string timePoint = dr["TimePoint"].ToString();
                    string value = dr["Value"].ToString();

                    float xPoint = computeLocationBottomX(dateTime, timePoint);
                    RectangleF rect = new RectangleF(xPoint, m_TableStartPointY + rowIndex * (m_LineHeight1 + 1) + 1, (m_LineHeight2 + 1) * m_DayTimePoint, m_LineHeight1 + 1);
                    g.DrawString(value, font, Brushes.Black, rect, centerFormat);
                }

                rowIndex++;
            }

            //绘制 手术后天数 中的数字
            if (CheckIsNeed(m_DataTableTableBaseLine, "手术后天数") != -1)
            {
                foreach (DataRow dr in PatientInfo.DataTableOperationTime.Rows)
                {
                    DateTime dateTime = Convert.ToDateTime(dr["DateTime"].ToString());
                    string timePoint = dr["TimePoint"].ToString();
                    string value = dr["Value"].ToString();

                    float xPoint = computeLocationBottomX(dateTime, timePoint);
                    RectangleF rect = new RectangleF(xPoint, m_TableStartPointY + rowIndex * (m_LineHeight1 + 1) + 1, (m_LineHeight2 + 1) * m_DayTimePoint, m_LineHeight1 + 1);
                    g.DrawString(value, font, Brushes.Red, rect, centerFormat);
                }

                rowIndex++;
            }


            //绘制时间点中的数字
            if (CheckIsNeed(m_DataTableTableBaseLine, "时   间") != -1)
            {
                DataTable Newdt = m_DataTableDayTimePoint.Clone();
                for (int i = 0; i < m_DataTableDayTimePoint.Rows.Count; i++)
                {
                    Newdt.ImportRow(m_DataTableDayTimePoint.Rows[i]);
                }
                //泗县中医院需求，3 19 23 时间点字体为红色 
                for (int j = 0; j < Newdt.Rows.Count; j++)
                {
                    if (Newdt.Rows[j]["TimePoint"].ToString() == "3" || Newdt.Rows[j]["TimePoint"].ToString() == "19" || Newdt.Rows[j]["TimePoint"].ToString() == "23")
                    {
                        Newdt.Rows[j]["Color"] = "red";
                    }
                }

                for (int i = 0; i < m_DayTimePoint * m_Days; i++)
                {
                    float xValue = m_TableStartPointX + m_FirstColumnWidth + 1 + (m_LineHeight2 + 1) * i;
                    float yValue = m_TableStartPointY + (m_DataTableTableBaseLine.Rows.Count - 1) * (1 + m_LineHeight1);
                    RectangleF rect = new RectangleF(xValue - 1, yValue, m_LineHeight2 + 4, m_LineHeight1 + 1);

                    Font fontTimePoint = new Font(this.Font.FontFamily, 8);
                    int index = i % m_DayTimePoint;

                    //根据配置决定是否开启12小时制 Add by wwj 2012-06-12
                    int timePoint = Convert.ToInt32(Newdt.Rows[index][0]);//m_DataTableDayTimePoint
                    if (m_Is12小时制 == "1" && timePoint > 12)
                    {
                        timePoint -= 12;
                    }

                    g.DrawString(timePoint.ToString(), fontTimePoint,
                        GetBrushByColorName(Newdt.Rows[index][1].ToString()), rect, centerFormat);
                }
            }

            #endregion
        }
        #endregion

        #region 绘制表格的第二部分（即体温表中小方格所有的行）
        /// <summary>
        /// 绘制表格的第二部分（即体温表中小方格所有的行）
        /// </summary>
        /// <param name="pictureBox"></param>
        /// <param name="g"></param>
        private void PaintSecondPartOfTable(PictureBox pictureBox, Graphics g)
        {
            Font font = this.Font;
            Pen penBold = new Pen(Color.Black, 2);//宽度为2的黑笔
            Pen pen = Pens.Black;//宽度为1的黑笔
            Pen penLightBlue = Pens.LightBlue;//宽度为1的蓝笔

            //此宽度不包括表格最左边1像素，包括表格最右边2像素
            int tableWidth = m_FirstColumnWidth/*第一列的宽度*/ + 1 + (m_LineHeight2 * m_DayTimePoint + m_DayTimePoint) * m_Days + 1;

            //记录下“时间”下面一条横线的位置信息
            int currentLinePointY = m_TableStartPointY + (m_LineHeight1 + 1) * m_DataTableTableBaseLine.Rows.Count; //记录下当前横线的Y轴上的值
            int cruuentLineEndLinePintX = m_TableStartPointX + tableWidth; //记录下横线终点X轴方向的值


            #region **********************************************************画涉及到温度的小方格子********************************************************

            //画时间段的分割线(竖直方向)
            for (int i = 0; i < m_Days; i++) //循环每天
            {
                int xPoint = 0;
                for (int j = 1; j <= m_DayTimePoint - 1; j++)//循环每个时间点
                {
                    xPoint = m_TableStartPointX + m_FirstColumnWidth + 1 + (m_LineHeight2 + 1) * j + (m_DayTimePoint + m_DayTimePoint * m_LineHeight2) * i;
                    g.DrawLine(penLightBlue, xPoint, currentLinePointY + 1, xPoint,
                        currentLinePointY + m_CellCount / m_CellCountInEveryDegree * (m_CellCountInEveryDegree * m_LineHeight2 + m_CellCountInEveryDegree + 1) - 1);//蓝色的线段
                }

                if (i != m_Days - 1)
                {
                    xPoint = m_TableStartPointX + m_FirstColumnWidth + 1 + (m_LineHeight2 + 1) * m_DayTimePoint + (m_DayTimePoint + m_DayTimePoint * m_LineHeight2) * i;
                    g.DrawLine(Pens.Red, xPoint, currentLinePointY + 1, xPoint,
                        currentLinePointY + m_CellCount / m_CellCountInEveryDegree * (m_CellCountInEveryDegree * m_LineHeight2 + m_CellCountInEveryDegree + 1) - 1);//红色的线段
                }
            }

            //画时间段的分割线(横向)
            for (int i = 0; i < m_CellCount / m_CellCountInEveryDegree; i++)//度与度之间的粗线
            {
                int yPoint = 0;

                for (int j = 1; j <= m_CellCountInEveryDegree - 1; j++) //每一度之间的细线
                {
                    yPoint = currentLinePointY + (m_LineHeight2 + 1) * j + (m_CellCountInEveryDegree * m_LineHeight2 + m_CellCountInEveryDegree + 1) * i;
                    g.DrawLine(penLightBlue, m_TableStartPointX + m_FirstColumnWidth + 2, yPoint, cruuentLineEndLinePintX - 2, yPoint);//蓝色的线段
                }

                if (i != m_CellCount / m_CellCountInEveryDegree - 1)
                {
                    yPoint = currentLinePointY + (m_LineHeight2 + 1) * m_CellCountInEveryDegree + 1/*粗线宽度为2，导致的问题*/ + (m_CellCountInEveryDegree * m_LineHeight2 + m_CellCountInEveryDegree + 1) * i;
                    g.DrawLine(penBold, m_TableStartPointX + m_FirstColumnWidth + 2, yPoint, cruuentLineEndLinePintX - 2 + 1/*由于粗线比细线要短1个像素，所以这里要加1*/, yPoint);//小方格区域中加粗的黑色线段
                }
                else
                {
                    //if (i == 3)
                    //{
                    //    yPoint = currentLinePointY + (m_LineHeight2 + 1) * m_CellCountInEveryDegree + (m_CellCountInEveryDegree * m_LineHeight2 + m_CellCountInEveryDegree + 1) * i;
                    //    g.DrawLine(Pens.Red, m_TableStartPointX + 1, yPoint, cruuentLineEndLinePintX - 2, yPoint);//小方格区域中最后一根正常的黑色线段

                    //}
                    //else
                    //{
                    yPoint = currentLinePointY + (m_LineHeight2 + 1) * m_CellCountInEveryDegree + (m_CellCountInEveryDegree * m_LineHeight2 + m_CellCountInEveryDegree + 1) * i;
                    g.DrawLine(Pens.Black, m_TableStartPointX + 1, yPoint, cruuentLineEndLinePintX - 2, yPoint);//小方格区域中最后一根正常的黑色线段

                    //}
                }
                Pen penBold1 = new Pen(Color.Red, 2);//宽度为2的黑笔
                if (i == 5)//当温度为43，i为3ThreeMeasureTableConfig中  xll当温度为42 可将i改为4 实现37度为红线
                {
                    yPoint = currentLinePointY + (m_LineHeight2 + 1) * m_CellCountInEveryDegree + (m_CellCountInEveryDegree * m_LineHeight2 + m_CellCountInEveryDegree + 1) * i;
                    g.DrawLine(penBold1, m_TableStartPointX + m_FirstColumnWidth + 2, yPoint, cruuentLineEndLinePintX - 2 + 1, yPoint);//小方格区域中最后一根正常的黑色线段
                }
            }

            //画时间段的分割线(竖直方向),用于覆盖由于画横线导致竖线被分断
            for (int i = 1; i < m_Days; i++) //循环每天
            {
                int xPoint = m_TableStartPointX + m_FirstColumnWidth + 1 + (m_DayTimePoint + m_DayTimePoint * m_LineHeight2) * i;
                g.DrawLine(Pens.Black, xPoint, currentLinePointY + 1, xPoint,
                    currentLinePointY + m_CellCount / m_CellCountInEveryDegree * (m_CellCountInEveryDegree * m_LineHeight2 + m_CellCountInEveryDegree + 1) - 2);//红色线段纵向的线段，泗县需求改为黑色
            }

            #endregion

            #region 绘制小方格左边的“呼吸”，“脉搏”，“体温”等
            //分割呼吸，脉搏，体温 等
            for (int i = 1; i <= m_FirstColumnHasSubColumnCount - 1; i++)
            {
                int xPoint = m_TableStartPointX + (m_FirstColumnWidth / m_FirstColumnHasSubColumnCount) * i;
                g.DrawLine(Pens.Black, xPoint, currentLinePointY + 1, xPoint,
                    currentLinePointY + m_CellCount / m_CellCountInEveryDegree * (2 + m_CellCountInEveryDegree * m_LineHeight2 + m_CellCountInEveryDegree - 1) - 2);
            }

            //画“呼吸”，“脉搏”，“体温”等的文字
            Font fontSmall = new Font(this.Font.FontFamily, 8);
            for (int i = 0; i < m_ArrayListVitalSigns.Count; i++)
            {
                VitalSigns vs = m_ArrayListVitalSigns[i] as VitalSigns;

                if (vs != null)
                {
                    //绘制名称，如“体温”
                    string name = GetVitalSignsTypeName(vs.Name);
                    RectangleF rectangleFVitalSigns = new RectangleF(m_TableStartPointX - m_CellCountInEveryDegree - 1 + (m_FirstColumnWidth / m_FirstColumnHasSubColumnCount) * i, currentLinePointY + 2,
                        m_FirstColumnWidth / m_FirstColumnHasSubColumnCount + 14,
                        (TextRenderer.MeasureText("name", this.Font).Height) * 1.5f);

                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    g.DrawString(name, font, Brushes.Black, rectangleFVitalSigns, sf);

                    //绘制单位，如“（摄氏度）”
                    name = "(" + vs.Unit + ")";
                    rectangleFVitalSigns = new RectangleF(m_TableStartPointX - m_CellCountInEveryDegree - 1 + (m_FirstColumnWidth / m_FirstColumnHasSubColumnCount) * i,
                        currentLinePointY + (TextRenderer.MeasureText("name", this.Font).Height) * 1.5f,
                        m_FirstColumnWidth / m_FirstColumnHasSubColumnCount + 14,
                        (TextRenderer.MeasureText("name", this.Font).Height) * 1.5f);
                    g.DrawString(name, fontSmall, Brushes.Black, rectangleFVitalSigns, sf);
                }
            }

            //画“呼吸”，“脉搏”，“体温”等的度量卡的尺度
            for (int i = 0; i < m_ArrayListVitalSigns.Count; i++)
            {
                VitalSigns vs = m_ArrayListVitalSigns[i] as VitalSigns;

                float maxValue = 0f;
                float value = 0f;
                float cellValue = 0f;

                if (vs != null)
                {
                    cellValue = vs.CellValue;
                    maxValue = vs.MaxValue;


                    float xPoint = m_TableStartPointX + (m_FirstColumnWidth / m_FirstColumnHasSubColumnCount + 1) * i;
                    float yPoint = 0f;

                    for (int j = 1; j < m_CellCount / m_CellCountInEveryDegree; j++)
                    {
                        value = maxValue - cellValue * m_CellCountInEveryDegree * j;

                        yPoint = currentLinePointY + ((m_LineHeight2 + 1) * m_CellCountInEveryDegree + 1) * j - TextRenderer.MeasureText(value.ToString(), this.Font).Height / 2;

                        RectangleF rectangleF = new RectangleF(xPoint, yPoint, m_FirstColumnWidth / m_FirstColumnHasSubColumnCount, TextRenderer.MeasureText(value.ToString(), this.Font).Height);
                        StringFormat sf = new StringFormat();
                        sf.LineAlignment = StringAlignment.Center;
                        sf.Alignment = StringAlignment.Center;

                        g.DrawString(value.ToString(), this.Font, Brushes.Black, rectangleF, sf);
                    }
                }
            }
            #endregion

            #region 绘制 患者入院、转入、手术、分娩、出院、死亡等,除手术不写具体时间外，其余均按24小时制，精确到分钟(PatientInfo.DateTableEventInformation)
            PatientInfo patientInfo = new PatientInfo();
            int pointYOffset = 0; //Y轴方向上的偏移量
            string lastTimePoint = "-1"; //记录上一个事件所在的时间点，用于处理在同一时间点的事件在一列显示，而不会重合在一起
            DateTime lastDateTime = DateTime.MinValue;
            m_Need断开LocationX = new List<int>();
            //对于婴儿不显示入科时间
            string m_noofinpat = PatientInfo.NoOfInpat;//取得病人的首页序号
            string m_isbaby = string.Empty;//是否是婴儿 
            DataTable m_Dt = MethodSet.GetPatData(m_noofinpat);
            if (m_Dt != null && m_Dt.Rows != null && m_Dt.Rows.Count>0)
            {
                m_isbaby = m_Dt.Rows[0]["isbaby"].ToString();
            }
            for (int i = 0; i < PatientInfo.DateTableEventInformation.Rows.Count; i++)
            {
                DateTime dateTime = Convert.ToDateTime(PatientInfo.DateTableEventInformation.Rows[i]["DateTime"]);
                string timePoint = patientInfo.GetTimePointInTime(m_DataTableDayTimePoint, dateTime.ToString("yyyy-H").Split('-')[1]);
                string eventInformation = PatientInfo.DateTableEventInformation.Rows[i]["EventInformation"].ToString();
                string eventInformationTemp = eventInformation.TrimEnd('-');
                //string statusid = PatientInfo.DateTableEventInformation.Rows[i]["StatusID"].ToString();
                //if (m_isbaby == "1")//如果是婴儿 add by ywk
                //{
                //    eventInformation = "";
                //    eventInformationTemp = "";
                //}
                float eventLocation = GetEventLocation(eventInformation.TrimEnd('-'));
                if (eventLocation <= 35)
                {
                    pointYOffset = 0;
                }

                if (lastTimePoint != timePoint || lastDateTime.Date != dateTime.Date)
                {
                    lastTimePoint = timePoint;
                    lastDateTime = dateTime;
                    pointYOffset = 0;
                }

                if (!CheckEventIsShowTime(eventInformation.TrimEnd('-')))
                {
                    eventInformation = eventInformation.TrimEnd('-');
                }
                else if (m_Is12小时制 == "0") //根据配置决定是否开启12小时制 Add by wwj 2012-06-12
                {
                    eventInformation += patientInfo.GetHourAndMinute(dateTime);
                }
                else if (m_Is12小时制 == "1") //根据配置决定是否开启12小时制 Add by wwj 2012-06-12
                {
                    eventInformation += patientInfo.GetHourAndMinute2(dateTime);
                }

                //if (m_isbaby == "1" && eventInformation.Contains("入院"))//如果是婴儿 add by ywk 2012年7月26日11:32:29
                //仅仅是入院不显示时间，对应入院娩出要显示的edit by ywk 2012年8月30日 09:16:05
                    if (m_isbaby == "1" && eventInformation.Equals("入院"))//如果是婴儿 add by ywk 2012年7月26日11:32:29
                {
                    eventInformation = "";
                    eventInformationTemp = "";
                }
                #region 计算出 需要绘制的事件 的 日期 和 时间点
                int daySpan = 0;//发生的事件距离体温单上第一列中日期的间隔天数
                if (m_DateTimeEveryColumnDateTime.Rows.Count > 0)
                {
                    DateTime firstDateTime = Convert.ToDateTime(m_DateTimeEveryColumnDateTime.Rows[0][0]);
                    daySpan = (dateTime - firstDateTime).Days;
                    //if (dateTime.Day - firstDateTime.Day > 0)
                    //{
                    //    continue;
                    //}
                    //上面计算，当相隔一天，时间相减不够一天时或者状态时间等于当前表格第一列日期的显示错误
                    //加层判断add by ywk
                    if (daySpan < 0 || daySpan >= m_Days || daySpan == 0 && !firstDateTime.ToString("yyyy-MM-dd").Equals(dateTime.ToString("yyyy-MM-dd"))) //表示这个事件已经超出了这张体温单的日期范围，所以要排除
                    {
                        continue;
                    }
                }

                int eventTimePointSerialNumber = 0;//发生事件的时间点在一天中的序号
                string eventTimePoint = timePoint;
                for (int j = 0; j < m_DataTableDayTimePoint.Rows.Count; j++)
                {
                    if (m_DataTableDayTimePoint.Rows[j][0].ToString() == eventTimePoint)
                    {
                        eventTimePointSerialNumber = j;
                        break;
                    }
                }
                #endregion

                #region 通过体温的最大值,及每个小方格代表的值，来决定事件描述绘制的位置
                int paintLocation = 0; //表示小方格从上到下的绘制事件信息的起始位置，即小方格数
                for (int j = 0; j < m_ArrayListVitalSigns.Count; j++)
                {
                    VitalSigns vs = m_ArrayListVitalSigns[j] as VitalSigns;
                    if (vs != null)
                    {
                        if (vs.Name.ToLower() == "TiWen".ToLower())
                        {
                            float maxValue = vs.MaxValue;
                            float cellValue = vs.CellValue;

                            //paintLocation = Convert.ToInt32((maxValue - m_PaintEventInformationPosition) / cellValue);
                            paintLocation = Convert.ToInt32((maxValue - eventLocation) / cellValue);
                        }
                    }
                }
                #endregion

                #region 计算开始绘制的起始坐标，判断并记录下需要断开节点的坐标

                //开始绘制的起始坐标
                int xPointEvent = m_TableStartPointX + m_FirstColumnWidth + 1 + (m_LineHeight2 + 1) * m_DayTimePoint * daySpan + (m_LineHeight2 + 1) * eventTimePointSerialNumber;
                int yPointEvent = currentLinePointY + (paintLocation / m_CellCountInEveryDegree) * ((m_LineHeight2 + 1) * m_CellCountInEveryDegree + 1)
                    + (paintLocation % m_CellCountInEveryDegree) * (m_LineHeight2 + 1)
                    + 2/*这里是因为效果不好所以+2，使其向下移动一点*/
                    + pointYOffset;

                if (m_断开状态.Contains(eventInformationTemp))
                {
                    m_Need断开LocationX.Add(xPointEvent/* + m_LineHeight2 / 2*/);
                }

                #endregion

                #region 竖向一个字一个字绘制
                Font fontEvent = new Font(this.Font.FontFamily, 8);
                int yPointIncrease = 0;//每次Y坐标增加的距离

                for (int j = 1; j <= eventInformation.Length; j++)
                {
                    if (eventInformation[j - 1].ToString() == "-")
                    {
                        //绘制事件与事件时间之间的竖线  xll 2012-10-30红线 换成黑线  武汉十三医院
                        g.DrawLine(Pens.Black, xPointEvent + m_LineHeight2 / 2 + 1, yPointEvent + yPointIncrease + 1,
                            xPointEvent + m_LineHeight2 / 2 + 1, yPointEvent + yPointIncrease + m_LineHeight2 - 1);
                    }
                    else
                    {

                        //g.DrawString(eventInformation[j - 1].ToString(), fontEvent, Brushes.Red, xPointEvent, yPointEvent + yPointIncrease);

                        //请假和外出的状态用黑色字体书写edit by ywk 2012年7月27日 16:02:10
                        if (eventInformation.Contains("请假") || eventInformation.Contains("外出"))
                        {
                            g.DrawString(eventInformation[j - 1].ToString(), fontEvent, Brushes.Black, xPointEvent, yPointEvent + yPointIncrease);
                        }
                        else   //xll 2012-10-30红线 换成黑线  武汉十三医院
                        {
                            g.DrawString(eventInformation[j - 1].ToString(), fontEvent, Brushes.Black, xPointEvent, yPointEvent + yPointIncrease);
                        }

                    }

                    //由于每一度之间是以粗线分割，所有这里要判断遇到粗线要多加1
                    if ((paintLocation + j) % m_CellCountInEveryDegree == 0)
                    {
                        yPointIncrease += m_LineHeight2 + 2;
                    }
                    else
                    {
                        yPointIncrease += m_LineHeight2 + 1;
                    }
                }
                pointYOffset += yPointIncrease + m_LineHeight2 + ((paintLocation + eventInformation.Length + 1) % m_CellCountInEveryDegree == 0 ? 2 : 1);
                #endregion
            }
            #endregion

            PaintCurve(g, currentLinePointY);
        }

        #region 绘制曲线
        /// <summary>
        /// 绘制曲线
        /// </summary>
        /// <param name="g"></param>
        /// <param name="currentLinePointY"></param>
        private void PaintCurve(Graphics g, int currentLinePointY/*体温表中小方格区域最左上角的Y轴方向上的坐标*/)
        {
            SmoothingMode smoothingMode = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.HighQuality;

            if (m_IsComputeLocation == false)//m_IsComputeLocation等于false表示要重新计算曲线图中各个点的坐标
            {
                //清空用于记录点和线的数组
                m_ArrayListPointLine.Clear();
                m_ArrayListPoint.Clear();

                //设置标志位
                m_IsComputeLocation = true;

                for (int i = 0; i < m_ArrayListVitalSigns.Count; i++)
                {
                    VitalSigns vitalSigns = m_ArrayListVitalSigns[i] as VitalSigns;
                    if (vitalSigns != null)
                    {
                        if (vitalSigns.Name == VitalSignsType.TiWen.ToString())
                        {
                            //计算要绘制的体温
                            ComputeTiWen(currentLinePointY, g);

                            //计算要绘制的物理降温
                            ComputeWuLiJiangWen(currentLinePointY, g);

                            ////计算要绘制的物理升温
                            ComputeWuLiShengWen(currentLinePointY, g);
                        }
                        else if (vitalSigns.Name == VitalSignsType.MaiBo.ToString())
                        {
                            //计算要绘制的脉搏
                            ComputeMaiBo(currentLinePointY, g);

                            //计算要绘制的心率
                            ComputeXinLv(currentLinePointY, g);

                            //当心率比脉搏大的时候，用实线把心率和脉搏连起来  Add By wwj 2012-06-25
                            ComputeXinLvMaiBoLine(currentLinePointY, g);
                        }
                        else if (vitalSigns.Name == VitalSignsType.HuXi.ToString())
                        {
                            //计算要绘制的呼吸
                            ComputeHuXi(currentLinePointY, g);
                        }
                    }
                }

                //计算要绘制的冲突的点
                ComputeConflictPoint();
            }

            //先绘制点与点之间的线段，然后再绘制点
            PaintPointLine(g, m_ArrayListPointLine);
            PaintPoint(g, m_ArrayListPoint, m_LineHeight2);

            g.SmoothingMode = smoothingMode;
        }


        #region 计算要绘制的"体温","脉搏","心率","呼吸","物理降温","新增的物理升温" 并把其点的坐标信息保存到m_ArrayListPoint中,点与点之间连线的坐标信息保存到m_ArrayListPointLine中

        #region 计算要绘制的体温
        /// <summary>
        /// 计算要绘制的体温
        /// </summary>
        /// <param name="currentLinePointY"></param>
        /// <param name="g"></param>
        private void ComputeTiWen(int currentLinePointY, Graphics g)
        {
            PointF pointLast = new PointF(-100, -100);
            string linkNextLast = string.Empty;

            for (int i = 0; i < PatientInfo.DataTableTemperature.Rows.Count; i++)
            {
                m_ID++;

                DateTime dateTime = Convert.ToDateTime(PatientInfo.DataTableTemperature.Rows[i]["DateTime"]);
                string timePoint = PatientInfo.DataTableTemperature.Rows[i]["TimePoint"].ToString().Trim();
                string typeID = PatientInfo.DataTableTemperature.Rows[i]["TypeID"].ToString().Trim();//温度的类型 1: 口温  2: 腋温 3: 肛温
                string value = PatientInfo.DataTableTemperature.Rows[i]["Value"].ToString().Trim();
                string memo = PatientInfo.DataTableTemperature.Rows[i]["Memo"].ToString().Trim();
                string linkNext = PatientInfo.DataTableTemperature.Rows[i]["LinkNext"].ToString().Trim();

                PointF currentPoint = new PointF();

                #region 当没有值得时候结束本次循环
                if (value != "")
                {
                    currentPoint = ComupteLocaton(currentLinePointY, dateTime, timePoint, value, VitalSignsType.TiWen.ToString());
                }
                else
                {
                    continue;
                }
                #endregion


                if (i != 0 /*当循环到第一个点的时候，由于不知道第二个点的信息，所有这里将第一个点略过*/ && linkNextLast == "Y")
                {
                    #region 记录下要绘制的点与点之间的线段的信息
                    VitalSignsLine vitalSignsLine = new VitalSignsLine();
                    vitalSignsLine.MainType = VitalSignsType.TiWen.ToString();
                    vitalSignsLine.StartPointF = new PointF(pointLast.X + m_LineHeight2 / 2, pointLast.Y + m_LineHeight2 / 2);
                    vitalSignsLine.StartPointID = m_ID - 1;

                    vitalSignsLine.EndPointF = new PointF(currentPoint.X + m_LineHeight2 / 2, currentPoint.Y + m_LineHeight2 / 2);
                    vitalSignsLine.EndPointID = m_ID;

                    vitalSignsLine.ColorName = "Blue"; //体温用蓝线相连接
                    vitalSignsLine.Type = Convert.ToString((int)LineType.SolidLine); //绘制实线
                    m_ArrayListPointLine.Add(vitalSignsLine);
                    #endregion
                }

                pointLast = currentPoint;
                linkNextLast = linkNext;

                #region 记录下要绘制的点的坐标和类型
                VitalSignsPosition vitalSignsPosition = new VitalSignsPosition();
                vitalSignsPosition.PointF = currentPoint;
                vitalSignsPosition.ID = m_ID;
                vitalSignsPosition.Type = VitalSignsType.TiWen.ToString();
                vitalSignsPosition.SubType = typeID;

                vitalSignsPosition.Date = dateTime;
                vitalSignsPosition.TimePoint = timePoint;

                m_ArrayListPoint.Add(vitalSignsPosition);
                #endregion
            }
        }
        #endregion

        #region 计算要绘制的脉搏
        /// <summary>
        /// 计算要绘制的脉搏
        /// </summary>
        /// <param name="currentLinePointY"></param>
        /// <param name="g"></param>
        private void ComputeMaiBo(int currentLinePointY, Graphics g)
        {
            PointF pointLast = new PointF(-100, -100);
            string linkNextLast = string.Empty;
            m_MaiBoList = new List<VitalSignsPosition>();

            for (int i = 0; i < PatientInfo.DataTableMaiBo.Rows.Count; i++)
            {
                m_ID++;

                DateTime dateTime = Convert.ToDateTime(PatientInfo.DataTableMaiBo.Rows[i]["DateTime"]);
                string timePoint = PatientInfo.DataTableMaiBo.Rows[i]["TimePoint"].ToString().Trim();
                string value = PatientInfo.DataTableMaiBo.Rows[i]["Value"].ToString().Trim();
                string memo = PatientInfo.DataTableMaiBo.Rows[i]["Memo"].ToString().Trim();
                string linkNext = PatientInfo.DataTableMaiBo.Rows[i]["LinkNext"].ToString().Trim();

                PointF currentPoint = new PointF();

                #region 当没有值得时候结束本次循环
                if (value != "")
                {
                    currentPoint = ComupteLocaton(currentLinePointY, dateTime, timePoint, value, VitalSignsType.MaiBo.ToString());
                }
                else
                {
                    continue;
                }
                #endregion


                if (i != 0 /*当循环到第一个点的时候，由于不知道第二个点的信息，所有这里将第一个点略过*/ && linkNextLast == "Y")
                {
                    #region 记录下要绘制的点与点之间的线段的信息
                    VitalSignsLine vitalSignsLine = new VitalSignsLine();
                    vitalSignsLine.MainType = VitalSignsType.MaiBo.ToString();
                    vitalSignsLine.StartPointF = new PointF(pointLast.X + m_LineHeight2 / 2, pointLast.Y + m_LineHeight2 / 2);
                    vitalSignsLine.StartPointID = m_ID - 1;

                    vitalSignsLine.EndPointF = new PointF(currentPoint.X + m_LineHeight2 / 2, currentPoint.Y + m_LineHeight2 / 2);
                    vitalSignsLine.EndPointID = m_ID;

                    vitalSignsLine.ColorName = "Red";//脉搏用红线相连接
                    vitalSignsLine.Type = Convert.ToString((int)LineType.SolidLine); //绘制实线
                    m_ArrayListPointLine.Add(vitalSignsLine);
                    #endregion
                }

                pointLast = currentPoint;
                linkNextLast = linkNext;

                #region 记录下要绘制的点的坐标和类型
                VitalSignsPosition vitalSignsPosition = new VitalSignsPosition();
                vitalSignsPosition.Value = value;
                vitalSignsPosition.PointF = currentPoint;
                vitalSignsPosition.ID = m_ID;
                vitalSignsPosition.Type = VitalSignsType.MaiBo.ToString();
                vitalSignsPosition.SubType = "";

                vitalSignsPosition.Date = dateTime;
                vitalSignsPosition.TimePoint = timePoint;

                m_ArrayListPoint.Add(vitalSignsPosition);
                m_MaiBoList.Add(vitalSignsPosition);
                #endregion
            }
        }
        #endregion

        #region 计算要绘制的心率

        private void ComputeXinLv(int currentLinePointY, Graphics g)
        {
            PointF pointLast = new PointF(-100, -100);
            string linkNextLast = string.Empty;
            m_XinLvList = new List<VitalSignsPosition>();

            for (int i = 0; i < PatientInfo.DataTableXinLv.Rows.Count; i++)
            {
                m_ID++;

                DateTime dateTime = Convert.ToDateTime(PatientInfo.DataTableXinLv.Rows[i]["DateTime"]);
                string timePoint = PatientInfo.DataTableXinLv.Rows[i]["TimePoint"].ToString().Trim();
                string value = PatientInfo.DataTableXinLv.Rows[i]["Value"].ToString().Trim();
                string memo = PatientInfo.DataTableXinLv.Rows[i]["Memo"].ToString().Trim();
                string linkNext = PatientInfo.DataTableXinLv.Rows[i]["LinkNext"].ToString().Trim();
                string isSpecial = PatientInfo.DataTableXinLv.Rows[i]["IsSpecial"].ToString().Trim();

                PointF currentPoint = new PointF();

                #region 当没有值得时候结束本次循环
                if (value != "")
                {
                    currentPoint = ComupteLocaton(currentLinePointY, dateTime, timePoint, value, VitalSignsType.MaiBo.ToString());
                }
                else
                {
                    continue;
                }
                #endregion


                if (i == 0)
                {
                    #region 找到第一个心率与脉搏相连的线

                    bool isBreak = false;
                    //for (int j = 0; j < m_ArrayListPoint.Count; j++)
                    for (int j = m_ArrayListPoint.Count - 1; j >= 0; j--)
                    {
                        VitalSignsPosition vsp = m_ArrayListPoint[j] as VitalSignsPosition;
                        if (vsp != null)
                        {
                            if (vsp.Type == VitalSignsType.MaiBo.ToString())
                            {
                                if (vsp.Date < dateTime || vsp.Date == dateTime && Convert.ToInt32(vsp.TimePoint) <= Convert.ToInt32(timePoint))//通过心率找到其对应的脉搏
                                {
                                    int id = vsp.ID;

                                    #region 找到与脉搏相连的前一个脉搏

                                    //心率有对应的脉搏
                                    if (vsp.Date == dateTime && vsp.TimePoint == timePoint)
                                    {
                                        #region 心率有对应的脉搏
                                        for (int m = 0; m < m_ArrayListPointLine.Count; m++)
                                        {
                                            VitalSignsLine vitalSignsLine = m_ArrayListPointLine[m] as VitalSignsLine;
                                            if (vitalSignsLine.EndPointID == id)
                                            {
                                                VitalSignsLine vsl = new VitalSignsLine();
                                                vsl.MainType = VitalSignsType.XinLv.ToString();
                                                vsl.StartPointF = vitalSignsLine.StartPointF;
                                                vsl.StartPointID = vitalSignsLine.StartPointID;

                                                vsl.EndPointF = new PointF(currentPoint.X + m_LineHeight2 / 2, currentPoint.Y + m_LineHeight2 / 2);
                                                vsl.EndPointID = m_ID;

                                                vsl.ColorName = "Red";//脉搏和心率用红线相连接
                                                vsl.Type = Convert.ToString((int)LineType.SolidLine); //绘制虚线 xll将绘制虚线改为实线
                                                m_ArrayListPointLine.Add(vsl);

                                                isBreak = true;
                                                break;
                                            }
                                        }
                                        //add by ywk 
                                        //无论心率有没有对应的脉搏，都只要只有一个心率就要和其时间点最近的那个脉搏相连
                                        if (PatientInfo.DataTableXinLv.Rows.Count == 1)
                                        {
                                            DateTime xinlv_date = Convert.ToDateTime(PatientInfo.DataTableXinLv.Rows[0]["DateTime"]);
                                            VitalSignsLine vsl1 = new VitalSignsLine();
                                            vsl1.MainType = VitalSignsType.XinLv.ToString();

                                            vsl1.StartPointF = new PointF(currentPoint.X + m_LineHeight2 / 2, currentPoint.Y + m_LineHeight2 / 2);
                                            vsl1.StartPointID = vsp.ID;

                                            string spiteTime = xinlv_date.ToString("yyyy-MM-dd");
                                            DataRow[] dr = PatientInfo.DataTableMaiBo.Select(" DateTime='" + spiteTime + "' and TimePoint <>'" + vsp.TimePoint + "' ", " TimePoint ASC");
                                            //DataTable newDt = PatientInfo.DataTableMaiBo.Clone();
                                            DataTable newDt = new DataTable();
                                            newDt.Columns.Add("YY", typeof(Int32));
                                            newDt.Columns.Add("TimePoint");
                                            newDt.Columns.Add("DateTime");
                                            newDt.Columns.Add("Value");
                                            newDt.Columns.Add("Memo");
                                            newDt.Columns.Add("LinkNext");
                                            for (int u = 0; u < dr.Length; u++)
                                            {
                                                DataRow row = newDt.NewRow();
                                                row["YY"] = dr[u]["TimePoint"].ToString();
                                                row["DateTime"] = dr[u]["DateTime"].ToString();
                                                row["Value"] = dr[u]["Value"].ToString();
                                                row["TimePoint"] = dr[u]["TimePoint"].ToString();
                                                row["Memo"] = dr[u]["Memo"].ToString();
                                                row["LinkNext"] = dr[u]["LinkNext"].ToString();
                                                //要取比当前时间点大的位置
                                                if (Int32.Parse(dr[u]["TimePoint"].ToString()) > Int32.Parse(timePoint))
                                                {
                                                    newDt.Rows.Add(row);
                                                    DataColumn[] cols = new DataColumn[] { newDt.Columns["YY"] };
                                                    newDt.PrimaryKey = cols;
                                                    newDt.DefaultView.Sort = "YY ASC";
                                                    DataRow[] dr1 = newDt.Select(" 1=1 ", " YY ASC");

                                                    PointF currentPoint1 = ComupteLocaton(currentLinePointY, xinlv_date, dr1[0]["TimePoint"].ToString(), dr1[0]["Value"].ToString
                                                        (), VitalSignsType.MaiBo.ToString());
                                                    //VitalSignsLine vimtalSignsLine = m_ArrayListPointLine[1] as VitalSignsLine;
                                                    vsl1.EndPointF = new PointF(currentPoint1.X + 6, currentPoint1.Y + 6);
                                                    vsl1.EndPointID = 2;
                                                    vsl1.ColorName = "Red";//脉搏和心率用红线相连接
                                                    vsl1.Type = Convert.ToString((int)LineType.SolidLine); //绘制虚线 xll改实线
                                                }
                                                //如果心率是最后一个点，后面没有脉搏
                                                if (Int32.Parse(dr[u]["TimePoint"].ToString()) < Int32.Parse(timePoint))
                                                {
                                                }
                                            }

                                            m_ArrayListPointLine.Add(vsl1);
                                        }
                                        #endregion
                                    }
                                    else//心率没有对应的脉搏
                                    {
                                        #region 心率没有对应的脉搏
                                        VitalSignsLine vsl = new VitalSignsLine();
                                        vsl.MainType = VitalSignsType.XinLv.ToString();
                                        vsl.StartPointF = new PointF(vsp.PointF.X + m_LineHeight2 / 2, vsp.PointF.Y + m_LineHeight2 / 2);
                                        vsl.StartPointID = vsp.ID;

                                        vsl.EndPointF = new PointF(currentPoint.X + m_LineHeight2 / 2, currentPoint.Y + m_LineHeight2 / 2);
                                        vsl.EndPointID = m_ID;

                                        vsl.ColorName = "Red";//脉搏和心率用红线相连接
                                        vsl.Type = Convert.ToString((int)LineType.SolidLine); //绘制虚线 xll改实线
                                        m_ArrayListPointLine.Add(vsl);

                                        //如果只有一个心率,要和最近时间点的脉搏相连接 
                                        //add by ywk 
                                        if (PatientInfo.DataTableXinLv.Rows.Count == 1)
                                        {
                                            DateTime xinlv_date = Convert.ToDateTime(PatientInfo.DataTableXinLv.Rows[0]["DateTime"]);
                                            VitalSignsLine vsl1 = new VitalSignsLine();
                                            vsl1.MainType = VitalSignsType.XinLv.ToString();

                                            vsl1.StartPointF = new PointF(currentPoint.X + m_LineHeight2 / 2, currentPoint.Y + m_LineHeight2 / 2);
                                            vsl1.StartPointID = vsp.ID;

                                            string spiteTime = xinlv_date.ToString("yyyy-MM-dd");

                                            //此处根据时间点筛选出的DataTable会随着数据的变化排序，（因为没有主键，他按照数据添加顺序排序）
                                            DataRow[] dr = PatientInfo.DataTableMaiBo.Select(" DateTime='" + spiteTime + "'", "TimePoint DESC");
                                            //所以此处新生成一个table，并设置主键为整形!
                                            DataTable newDt = new DataTable();
                                            newDt.Columns.Add("YY", typeof(Int32));
                                            newDt.Columns.Add("TimePoint");
                                            newDt.Columns.Add("DateTime");
                                            newDt.Columns.Add("Value");
                                            newDt.Columns.Add("Memo");
                                            newDt.Columns.Add("LinkNext");
                                            for (int u = 0; u < dr.Length; u++)
                                            {
                                                DataRow row = newDt.NewRow();
                                                row["YY"] = dr[u]["TimePoint"].ToString();
                                                row["DateTime"] = dr[u]["DateTime"].ToString();
                                                row["Value"] = dr[u]["Value"].ToString();
                                                row["TimePoint"] = dr[u]["TimePoint"].ToString();
                                                row["Memo"] = dr[u]["Memo"].ToString();
                                                row["LinkNext"] = dr[u]["LinkNext"].ToString();
                                                //要取比当前时间点大的位置
                                                if (Int32.Parse(dr[u]["TimePoint"].ToString()) > Int32.Parse(timePoint))
                                                {
                                                    newDt.Rows.Add(row);
                                                    DataColumn[] cols = new DataColumn[] { newDt.Columns["YY"] };
                                                    newDt.PrimaryKey = cols;
                                                    newDt.DefaultView.Sort = "YY ASC";
                                                    DataRow[] dr1 = newDt.Select(" 1=1 ", " YY ASC");
                                                    PointF currentPoint1 = ComupteLocaton(currentLinePointY, xinlv_date, dr1[0]["TimePoint"].ToString(), dr1[0]["Value"].ToString
                                                        (), VitalSignsType.MaiBo.ToString());
                                                    //VitalSignsLine vimtalSignsLine = m_ArrayListPointLine[1] as VitalSignsLine;
                                                    vsl1.EndPointF = new PointF(currentPoint1.X + 6, currentPoint1.Y + 6);
                                                    vsl1.EndPointID = 2;
                                                    vsl1.ColorName = "Red";//脉搏和心率用红线相连接
                                                    vsl1.Type = Convert.ToString((int)LineType.SolidLine); //绘制虚线 xll改实线
                                                }
                                                //心率后面没有脉搏，不处理，跟原来一样
                                                if (Int32.Parse(dr[u]["TimePoint"].ToString()) < Int32.Parse(timePoint))
                                                {

                                                }
                                            }
                                            m_ArrayListPointLine.Add(vsl1);
                                        }
                                        isBreak = true;
                                        break;
                                        #endregion
                                    }
                                    #endregion
                                }
                            }
                        }

                        if (isBreak == true)
                        {
                            break;
                        }
                    }

                    #endregion
                }
                else if (linkNextLast == "Y")
                {
                    #region 记录下要绘制的点与点之间的线段的信息
                    VitalSignsLine vitalSignsLine = new VitalSignsLine();
                    vitalSignsLine.MainType = VitalSignsType.XinLv.ToString();
                    vitalSignsLine.StartPointF = new PointF(pointLast.X + m_LineHeight2 / 2, pointLast.Y + m_LineHeight2 / 2);
                    vitalSignsLine.StartPointID = m_ID - 1;

                    vitalSignsLine.EndPointF = new PointF(currentPoint.X + m_LineHeight2 / 2, currentPoint.Y + m_LineHeight2 / 2);
                    vitalSignsLine.EndPointID = m_ID;

                    vitalSignsLine.ColorName = "Red";//心率用红线相连接
                    vitalSignsLine.Type = Convert.ToString((int)LineType.SolidLine); //绘制虚线 xll改实线
                    m_ArrayListPointLine.Add(vitalSignsLine);
                    #endregion

                    #region 找到最后一个心率与脉搏相连的线

                    if (i == PatientInfo.DataTableXinLv.Rows.Count - 1)
                    {
                        bool isBreak = false;
                        for (int j = 0; j < m_ArrayListPoint.Count; j++)
                        {
                            VitalSignsPosition vsp = m_ArrayListPoint[j] as VitalSignsPosition;
                            if (vsp != null)
                            {
                                if (vsp.Type == VitalSignsType.MaiBo.ToString())
                                {
                                    if (vsp.Date > dateTime || vsp.Date == dateTime && Convert.ToInt32(vsp.TimePoint) >= Convert.ToInt32(timePoint))//通过心率找到其对应的脉搏
                                    {
                                        int id = vsp.ID;

                                        #region 找到与脉搏相连的后一个脉搏

                                        //心率有对应的脉搏
                                        if (vsp.Date == dateTime && vsp.TimePoint == timePoint)
                                        {
                                            #region 心率有对应的脉搏
                                            for (int m = 0; m < m_ArrayListPointLine.Count; m++)
                                            {
                                                VitalSignsLine line = m_ArrayListPointLine[m] as VitalSignsLine;
                                                if (line.StartPointID == id)
                                                {
                                                    VitalSignsLine vsl = new VitalSignsLine();
                                                    vsl.MainType = VitalSignsType.XinLv.ToString();
                                                    vsl.StartPointF = line.EndPointF;
                                                    vsl.StartPointID = line.EndPointID;

                                                    vsl.EndPointF = new PointF(currentPoint.X + m_LineHeight2 / 2, currentPoint.Y + m_LineHeight2 / 2);
                                                    vsl.EndPointID = m_ID;

                                                    vsl.ColorName = "Red";//脉搏和心率用红线相连接
                                                    vsl.Type = Convert.ToString((int)LineType.SolidLine); //绘制虚线 xll改实线
                                                    m_ArrayListPointLine.Add(vsl);

                                                    isBreak = true;
                                                    break;
                                                }
                                            }
                                            #endregion
                                        }
                                        else//心率没有对应的脉搏
                                        {
                                            #region 心率没有对应的脉搏

                                            VitalSignsLine vsl = new VitalSignsLine();
                                            vsl.MainType = VitalSignsType.XinLv.ToString();
                                            vsl.StartPointF = new PointF(vsp.PointF.X + m_LineHeight2 / 2, vsp.PointF.Y + m_LineHeight2 / 2);
                                            vsl.StartPointID = vsp.ID;

                                            vsl.EndPointF = new PointF(currentPoint.X + m_LineHeight2 / 2, currentPoint.Y + m_LineHeight2 / 2);
                                            vsl.EndPointID = m_ID;

                                            vsl.ColorName = "Red";//脉搏和心率用红线相连接
                                            vsl.Type = Convert.ToString((int)LineType.SolidLine); //绘制虚线 xll改实线
                                            m_ArrayListPointLine.Add(vsl);

                                            isBreak = true;
                                            break;

                                            #endregion
                                        }
                                        #endregion
                                    }
                                }
                            }

                            if (isBreak == true)
                            {
                                break;
                            }
                        }
                    }
                    #endregion
                }

                pointLast = currentPoint;
                linkNextLast = linkNext;

                #region 记录下要绘制的点的坐标和类型
                VitalSignsPosition vitalSignsPosition = new VitalSignsPosition();
                vitalSignsPosition.Value = value;
                vitalSignsPosition.PointF = currentPoint;
                vitalSignsPosition.ID = m_ID;
                vitalSignsPosition.Type = VitalSignsType.XinLv.ToString();
                vitalSignsPosition.SubType = "";
                vitalSignsPosition.IsSpecial = isSpecial;

                vitalSignsPosition.Date = dateTime;
                vitalSignsPosition.TimePoint = timePoint;

                m_ArrayListPoint.Add(vitalSignsPosition);
                m_XinLvList.Add(vitalSignsPosition);
                #endregion
            }
        }

        #endregion

        #region 当心率比脉搏大的时候，用实线把心率和脉搏连起来 Add By wwj 2012-06-25

        private void ComputeXinLvMaiBoLine(int currentLinePointY, Graphics g)
        {
            foreach (VitalSignsPosition xinlvPosition in m_XinLvList)//心率
            {
                foreach (VitalSignsPosition maiboPosition in m_MaiBoList)//脉搏
                {
                    if (xinlvPosition.Date == maiboPosition.Date && xinlvPosition.TimePoint == maiboPosition.TimePoint)
                    {
                        //心率比脉搏大
                        if (Convert.ToDouble(xinlvPosition.Value) > Convert.ToDouble(maiboPosition.Value))
                        {
                            VitalSignsLine vsl = new VitalSignsLine();
                            vsl.MainType = VitalSignsType.XinLv.ToString();
                            vsl.StartPointF = new PointF(xinlvPosition.PointF.X + m_LineHeight2 / 2, xinlvPosition.PointF.Y + m_LineHeight2 / 2); ;
                            vsl.StartPointID = xinlvPosition.ID;

                            vsl.EndPointF = new PointF(maiboPosition.PointF.X + m_LineHeight2 / 2, maiboPosition.PointF.Y + m_LineHeight2 / 2); ; ;
                            vsl.EndPointID = maiboPosition.ID;

                            vsl.ColorName = "Red";//脉搏和心率用红线相连接
                            vsl.Type = Convert.ToString((int)LineType.SolidLine); //绘制虚线
                            m_ArrayListPointLine.Add(vsl);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 记录需要绘制的心率的位置
        /// </summary>
        private List<VitalSignsPosition> m_XinLvList;

        /// <summary>
        /// 记录需要绘制的脉搏的位置
        /// </summary>
        private List<VitalSignsPosition> m_MaiBoList;

        #endregion

        #region 计算要绘制的呼吸
        /// <summary>
        /// 计算要绘制的脉搏
        /// </summary>
        /// <param name="currentLinePointY"></param>
        /// <param name="g"></param>
        private void ComputeHuXi(int currentLinePointY, Graphics g)
        {
            PointF pointLast = new PointF(-100, -100);
            string linkNextLast = string.Empty;

            for (int i = 0; i < PatientInfo.DataTableHuXi.Rows.Count; i++)
            {
                m_ID++;

                DateTime dateTime = Convert.ToDateTime(PatientInfo.DataTableHuXi.Rows[i]["DateTime"]);
                string timePoint = PatientInfo.DataTableHuXi.Rows[i]["TimePoint"].ToString().Trim();
                string value = PatientInfo.DataTableHuXi.Rows[i]["Value"].ToString().Trim();
                string memo = PatientInfo.DataTableHuXi.Rows[i]["Memo"].ToString().Trim();
                string linkNext = PatientInfo.DataTableHuXi.Rows[i]["LinkNext"].ToString().Trim();
                string IsSpecial = PatientInfo.DataTableHuXi.Rows[i]["IsSpecial"].ToString().Trim();

                PointF currentPoint = new PointF();

                #region 当没有值得时候结束本次循环
                if (value != "")
                {
                    currentPoint = ComupteLocaton(currentLinePointY, dateTime, timePoint, value, VitalSignsType.HuXi.ToString());
                }
                else
                {
                    continue;
                }
                #endregion


                if (i != 0 /*当循环到第一个点的时候，由于不知道第二个点的信息，所有这里将第一个点略过*/ && linkNextLast == "Y")
                {
                    #region 记录下要绘制的点与点之间的线段的信息
                    VitalSignsLine vitalSignsLine = new VitalSignsLine();

                    vitalSignsLine.MainType = VitalSignsType.HuXi.ToString();
                    vitalSignsLine.StartPointF = new PointF(pointLast.X + m_LineHeight2 / 2, pointLast.Y + m_LineHeight2 / 2);
                    vitalSignsLine.StartPointID = m_ID - 1;

                    vitalSignsLine.EndPointF = new PointF(currentPoint.X + m_LineHeight2 / 2, currentPoint.Y + m_LineHeight2 / 2);
                    vitalSignsLine.EndPointID = m_ID;

                    vitalSignsLine.ColorName = "Blue";//呼吸用蓝线相连接
                    vitalSignsLine.Type = Convert.ToString((int)LineType.SolidLine); //绘制实线
                    m_ArrayListPointLine.Add(vitalSignsLine);
                    #endregion
                }

                pointLast = currentPoint;
                linkNextLast = linkNext;

                #region 记录下要绘制的点的坐标和类型
                VitalSignsPosition vitalSignsPosition = new VitalSignsPosition();
                vitalSignsPosition.PointF = currentPoint;
                vitalSignsPosition.ID = m_ID;
                vitalSignsPosition.Type = VitalSignsType.HuXi.ToString();
                vitalSignsPosition.SubType = "";

                vitalSignsPosition.Date = dateTime;
                vitalSignsPosition.TimePoint = timePoint;

                vitalSignsPosition.IsSpecial = IsSpecial;
                m_ArrayListPoint.Add(vitalSignsPosition);
                #endregion
            }
        }
        #endregion



        #region 计算要绘制的物理升温
        /// <summary>
        /// 计算要绘制的物理升温 addd by ywk 
        /// </summary>
        /// <param name="currentLinePointY"></param>
        /// <param name="g"></param>
        private void ComputeWuLiShengWen(int currentLinePointY, Graphics g)
        {
            for (int i = 0; i < PatientInfo.DataTableWuLiShengWen.Rows.Count; i++)
            {
                m_ID++;

                DateTime dateTime = Convert.ToDateTime(PatientInfo.DataTableWuLiShengWen.Rows[i]["DateTime"]);
                string timePoint = PatientInfo.DataTableWuLiShengWen.Rows[i]["TimePoint"].ToString().Trim();
                string value = PatientInfo.DataTableWuLiShengWen.Rows[i]["Value"].ToString().Trim();
                string linkNext = PatientInfo.DataTableWuLiShengWen.Rows[i]["LinkNext"].ToString().Trim();

                PointF currentPoint = new PointF();

                #region 当没有值的时候结束本次循环
                if (value != "")
                {
                    currentPoint = ComupteLocaton(currentLinePointY, dateTime, timePoint, value, VitalSignsType.TiWen.ToString());
                }
                else
                {
                    continue;
                }
                #endregion

                #region 记录下要绘制的点与点之间的线段的信息，在这里是找到物理降温前的温度
                VitalSignsLine vitalSignsLine = new VitalSignsLine();

                for (int j = 0; j < m_ArrayListPoint.Count; j++)
                {
                    VitalSignsPosition vsp = m_ArrayListPoint[j] as VitalSignsPosition;
                    if (vsp.Type == VitalSignsType.TiWen.ToString())
                    {
                        if (vsp.Date == dateTime && vsp.TimePoint == timePoint)
                        {
                            vitalSignsLine.MainType = VitalSignsType.WuLiShengWen.ToString();
                            vitalSignsLine.StartPointF = new PointF(vsp.PointF.X + m_LineHeight2 / 2, vsp.PointF.Y + m_LineHeight2 / 2); //物理升温前体温的坐标
                            vitalSignsLine.StartPointID = vsp.ID;

                            vitalSignsLine.EndPointF = new PointF(currentPoint.X + m_LineHeight2 / 2, currentPoint.Y + m_LineHeight2 / 2);
                            vitalSignsLine.EndPointID = m_ID;

                            vitalSignsLine.ColorName = "Blue";//物理升温用蓝线相连接
                            vitalSignsLine.Type = Convert.ToString((int)LineType.DashLine); //绘制虚线
                            m_ArrayListPointLine.Add(vitalSignsLine);

                            break;
                        }
                    }
                }
                #endregion

                #region 记录下要绘制的点的坐标和类型

                VitalSignsPosition vitalSignsPosition = new VitalSignsPosition();
                vitalSignsPosition.PointF = currentPoint;
                vitalSignsPosition.ID = m_ID;
                vitalSignsPosition.Type = VitalSignsType.WuLiShengWen.ToString();
                vitalSignsPosition.SubType = "";

                vitalSignsPosition.Date = dateTime;
                vitalSignsPosition.TimePoint = timePoint;

                m_ArrayListPoint.Add(vitalSignsPosition);

                #endregion
            }
        }
        #endregion

        #region 计算要绘制的物理降温
        /// <summary>
        /// 计算要绘制的物理降温
        /// </summary>
        /// <param name="currentLinePointY"></param>
        /// <param name="g"></param>
        private void ComputeWuLiJiangWen(int currentLinePointY, Graphics g)
        {
            for (int i = 0; i < PatientInfo.DataTableWuLiJiangWen.Rows.Count; i++)
            {
                m_ID++;

                DateTime dateTime = Convert.ToDateTime(PatientInfo.DataTableWuLiJiangWen.Rows[i]["DateTime"]);
                string timePoint = PatientInfo.DataTableWuLiJiangWen.Rows[i]["TimePoint"].ToString().Trim();
                string value = PatientInfo.DataTableWuLiJiangWen.Rows[i]["Value"].ToString().Trim();
                string linkNext = PatientInfo.DataTableWuLiJiangWen.Rows[i]["LinkNext"].ToString().Trim();

                PointF currentPoint = new PointF();

                #region 当没有值得时候结束本次循环
                if (value != "")
                {
                    currentPoint = ComupteLocaton(currentLinePointY, dateTime, timePoint, value, VitalSignsType.TiWen.ToString());
                }
                else
                {
                    continue;
                }
                #endregion

                #region 记录下要绘制的点与点之间的线段的信息，在这里是找到物理降温前的温度
                VitalSignsLine vitalSignsLine = new VitalSignsLine();

                for (int j = 0; j < m_ArrayListPoint.Count; j++)
                {
                    VitalSignsPosition vsp = m_ArrayListPoint[j] as VitalSignsPosition;
                    if (vsp.Type == VitalSignsType.TiWen.ToString())
                    {
                        if (vsp.Date == dateTime && vsp.TimePoint == timePoint)
                        {
                            vitalSignsLine.MainType = VitalSignsType.WuLiJiangWen.ToString();
                            vitalSignsLine.StartPointF = new PointF(vsp.PointF.X + m_LineHeight2 / 2, vsp.PointF.Y + m_LineHeight2 / 2); //物理降温前体温的坐标
                            vitalSignsLine.StartPointID = vsp.ID;

                            vitalSignsLine.EndPointF = new PointF(currentPoint.X + m_LineHeight2 / 2, currentPoint.Y + m_LineHeight2 / 2);
                            vitalSignsLine.EndPointID = m_ID;

                            vitalSignsLine.ColorName = "Red";//物理降温用红线相连接
                            vitalSignsLine.Type = Convert.ToString((int)LineType.DashLine); //绘制虚线
                            m_ArrayListPointLine.Add(vitalSignsLine);

                            break;
                        }
                    }
                }
                #endregion

                #region 记录下要绘制的点的坐标和类型

                VitalSignsPosition vitalSignsPosition = new VitalSignsPosition();
                vitalSignsPosition.PointF = currentPoint;
                vitalSignsPosition.ID = m_ID;
                vitalSignsPosition.Type = VitalSignsType.WuLiJiangWen.ToString();
                vitalSignsPosition.SubType = "";

                vitalSignsPosition.Date = dateTime;
                vitalSignsPosition.TimePoint = timePoint;

                m_ArrayListPoint.Add(vitalSignsPosition);

                #endregion
            }
        }
        #endregion

        #region 计算曲线图中值的坐标
        /// <summary>
        /// 计算曲线图中值的坐标
        /// </summary>
        /// <param name="currentLinePointY"></param>
        /// <param name="currentDateTime"></param>
        /// <param name="testTimePoint"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private PointF ComupteLocaton(int currentLinePointY/*体温表中小方格区域最左上角的Y轴方向上的坐标*/, DateTime currentDateTime, string testTimePoint, string value, string type)
        {
            #region 计算出 需要绘制的事件 的 日期 和 时间点
            int daySpan = 0;//温度测量时间距离体温单上第一列中日期的间隔天数
            if (m_DateTimeEveryColumnDateTime.Rows.Count > 0)
            {
                DateTime firstDateTime = Convert.ToDateTime(m_DateTimeEveryColumnDateTime.Rows[0][0]);
                daySpan = (currentDateTime - firstDateTime).Days;

                if (daySpan < 0 || daySpan >= m_Days) //已经超出了这张体温单的日期范围，所以要排除
                {
                    return new PointF(-100, -100);
                }
            }

            int eventTimePointSerialNumber = 0;//温度测量的时间点在一天中的序号
            for (int j = 0; j < m_DataTableDayTimePoint.Rows.Count; j++)
            {
                if (m_DataTableDayTimePoint.Rows[j][0].ToString() == testTimePoint)
                {
                    eventTimePointSerialNumber = j;
                    break;
                }
            }
            #endregion

            float xPoint = m_TableStartPointX + m_FirstColumnWidth + 1 + (m_LineHeight2 + 1) * m_DayTimePoint * daySpan + (m_LineHeight2 + 1) * eventTimePointSerialNumber;
            float yPoint = 0;
            float maxValue = GetMaxValue(type);//得到最大值
            float cellValue = GetCellValue(type); //得到每个小方格代表的值

            if (value.Trim() != "")
            {
                float currentValue = (float)Convert.ToDouble(value);

                int cellCount = Convert.ToInt32(Math.Floor((maxValue - currentValue) / cellValue)); //从最大温度值到当前温度值有多少个小格子
                float excess = (maxValue - currentValue) % cellValue;

                yPoint = currentLinePointY
                    + (cellCount / m_CellCountInEveryDegree) * ((m_LineHeight2 + 1) * m_CellCountInEveryDegree + 1) //每两条粗线之间
                    + (cellCount % m_CellCountInEveryDegree) * (m_LineHeight2 + 1) //小格子之间
                    + (excess / cellValue) * m_LineHeight2; //小格子中
            }

            //return new PointF(xPoint + 1/*由于效果不好，所以在这里加2进行微调*/, yPoint - m_LineHeight2 / 2 + 1/*由于效果不好，所以在这里加2进行调整*/);
            return new PointF(xPoint + 5, yPoint - m_LineHeight2 / 2 + 5);//edit by ywk
        }
        #endregion

        #region 计算要绘制的重合的部分
        private void ComputeConflictPoint()
        {
            VitalSignsPosition vsp = new VitalSignsPosition();
            m_ArrayListConflictPoint = vsp.GetmConflictPoint(m_ArrayListPoint, m_ArrayListPointLine, m_LineHeight2, m_Distance);
        }

        #endregion

        #endregion

        #region 绘制曲线中的各个点,以及各个点之间的连线

        /// <summary>
        /// 绘制曲线中的各个点
        /// </summary>
        /// <param name="g">Graphics</param>
        /// <param name="arrayListPoint">保存各个点信息的数组</param>
        /// <param name="cellHeight">小方格的高（宽）度</param>
        private void PaintPoint(Graphics g, ArrayList arrayListPoint, int cellHeight)
        {
            //绘制指定图片的点
            #region 绘制没有重合的点
            for (int i = 0; i < arrayListPoint.Count; i++)
            {
                VitalSignsPosition vitalSignsPosition = arrayListPoint[i] as VitalSignsPosition;
                if (vitalSignsPosition != null)
                {
                    RectangleF rect = new RectangleF(new PointF(vitalSignsPosition.PointF.X + 2, vitalSignsPosition.PointF.Y + 2), new SizeF(cellHeight - 4, cellHeight - 4));
                    RectangleF rectSmall = new RectangleF(new PointF(vitalSignsPosition.PointF.X + 1 + 2, vitalSignsPosition.PointF.Y + 1 + 2),
                        new SizeF(cellHeight - 2 - 4, cellHeight - 2 - 4));

                    if (vitalSignsPosition.Type == VitalSignsType.TiWen.ToString())//体温
                    {
                        if (m_IsShowTiWen == true)
                        {
                            string typeID = vitalSignsPosition.SubType;
                            if (typeID == Convert.ToString((int)VitalSignsTiWenType.KouWen)) //口温
                            {
                                //g.DrawImage(m_Picture.BitmapT1, rect);
                                m_Picture.DrawKouWen(g, rect.X, rect.Y);
                            }
                            else if (typeID == Convert.ToString((int)VitalSignsTiWenType.YeWen)) //腋温
                            {
                                //g.DrawImage(m_Picture.BitmapT2, rectSmall);
                                m_Picture.DrawYeXia(g, rect.X, rect.Y);
                            }
                            else if (typeID == Convert.ToString((int)VitalSignsTiWenType.GangWen)) //肛温
                            {
                                //g.DrawImage(m_Picture.BitmapT3, rect);
                                m_Picture.DrawGangWen(g, rect.X, rect.Y);
                            }
                        }
                    }
                    else if (vitalSignsPosition.Type == VitalSignsType.MaiBo.ToString()) //脉搏
                    {
                        if (m_IsShowMaiBo == true)
                        {
                            //g.DrawImage(m_Picture.BitmapMaiBo, rect);
                            m_Picture.DrawMaiBo(g, rect.X, rect.Y);
                        }
                    }
                    else if (vitalSignsPosition.Type == VitalSignsType.XinLv.ToString()) //心率
                    {
                        if (m_IsShowMaiBo == true)
                        {
                            if (vitalSignsPosition.IsSpecial == "Y") //表示使用了起搏器
                            {
                                //g.DrawImage(m_Picture.BitmapQiBoQi, rect);
                                m_Picture.DrawQiBoQi(g, rect.X, rect.Y);
                            }
                            else
                            {
                                //g.DrawImage(m_Picture.BitmapXinlv, rect);
                                m_Picture.DrawXinLv(g, rect.X, rect.Y);
                            }
                        }
                    }
                    else if (vitalSignsPosition.Type == VitalSignsType.HuXi.ToString()) //呼吸
                    {
                        if (m_IsShowHuXi == true)
                        {
                            if (vitalSignsPosition.IsSpecial == "Y") //表示使用了呼吸机
                            {
                                //g.DrawImage(m_Picture.BitmapHuXiSpecial, rect);
                                m_Picture.DrawHuXiJi(g, rect.X, rect.Y);
                            }
                            else
                            {
                                //g.DrawImage(m_Picture.BitmapHuXi, rect);
                                m_Picture.DrawHuXi(g, rect.X, rect.Y);
                            }
                        }
                    }
                    else if (vitalSignsPosition.Type == VitalSignsType.WuLiJiangWen.ToString()) //物理降温
                    {
                        if (m_IsShowTiWen == true)
                        {
                            //g.DrawImage(m_Picture.BitmapWuLiJiangWen, rect);
                            m_Picture.DrawWuLiJiangWen(g, rect.X, rect.Y);
                        }
                    }

                        //新增物理升温
                    else if (vitalSignsPosition.Type == VitalSignsType.WuLiShengWen.ToString()) //物理升温
                    {
                        if (m_IsShowTiWen == true)
                        {
                            //g.DrawImage(m_Picture.BitmapWuLiJiangWen, rect);
                            m_Picture.DrawWuLiShengWen(g, rect.X, rect.Y);
                        }
                    }
                }
            }
            #endregion

            #region 绘制有重合部分的点
            for (int i = 0; i < m_ArrayListConflictPoint.Count; i++)
            {
                VitalSignsPosition vitalSignsPosition = m_ArrayListConflictPoint[i] as VitalSignsPosition;
                if (vitalSignsPosition != null)
                {
                    //判断这个点是否需要绘出来
                    if (vitalSignsPosition.IsDraw == false)
                    {
                        continue;
                    }

                    RectangleF rect = new RectangleF(new PointF(vitalSignsPosition.PointF.X + 2, vitalSignsPosition.PointF.Y + 2),
                        new SizeF(cellHeight - 4, cellHeight - 4));

                    if (vitalSignsPosition.Type == VitalSignsPosition.ConflictPointType.MaiBoTiWenKou.ToString())//脉搏与体温(口腔)重叠
                    {
                        if (m_IsShowMaiBo == true && m_IsShowTiWen == true)
                        {
                            //g.DrawImage(m_Picture.BitmapNursMaiBoTiWenKou, rect);
                            m_Picture.DrawMaiBoTiWenKou(g, rect.X, rect.Y);
                        }
                    }
                    else if (vitalSignsPosition.Type == VitalSignsPosition.ConflictPointType.MaiBoTiWenGang.ToString())//脉搏与体温(肛门)重叠
                    {
                        if (m_IsShowMaiBo == true && m_IsShowTiWen == true)
                        {
                            //g.DrawImage(m_Picture.BitmapNursMaiBoTiWenGang, rect);
                            m_Picture.DrawMaiBoTiWenGang(g, rect.X, rect.Y);
                        }
                    }
                    else if (vitalSignsPosition.Type == VitalSignsPosition.ConflictPointType.MaiBoTiWenYe.ToString())//脉搏与体温(腋下)重叠
                    {
                        if (m_IsShowMaiBo == true && m_IsShowTiWen == true)
                        {
                            //g.DrawImage(m_Picture.BitmapNursMaiBoTiWenYe, rect);
                            m_Picture.DrawMaiBoTiWenYe(g, rect.X, rect.Y);
                        }
                    }
                    else if (vitalSignsPosition.Type == VitalSignsPosition.ConflictPointType.HuXiMaiBo.ToString())//呼吸与脉搏相遇
                    {
                        if (m_IsShowHuXi == true && m_IsShowMaiBo == true)
                        {
                            //g.DrawImage(m_Picture.BitmapNursHuXiMaiBo, rect);
                            m_Picture.DrawHuXiMaiBo(g, rect.X, rect.Y);
                        }
                    }
                    else if (vitalSignsPosition.Type == VitalSignsPosition.ConflictPointType.TiWenHuXiMaiBo.ToString())//体温、呼吸、脉搏均在一个点上
                    {
                        if (m_IsShowMaiBo == true && m_IsShowHuXi == true && m_IsShowTiWen == true)
                        {
                            //g.DrawImage(m_Picture.BitmapNursTiWenHuXiMaiBo, rect);
                            m_Picture.DrawTiWenHuXiMaiBo(g, rect.X, rect.Y);
                        }
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// 绘制曲线中各个点之间的连线
        /// </summary>
        /// <param name="g"></param>
        /// <param name="arrayListPointLine"></param>
        private void PaintPointLine(Graphics g, ArrayList arrayListPointLine)
        {
            PointF startPoint = new PointF();
            PointF endPoint = new PointF();
            string type = string.Empty;

            for (int i = 0; i < arrayListPointLine.Count; i++)
            {
                VitalSignsLine vitalSignsLine = arrayListPointLine[i] as VitalSignsLine;
                if (vitalSignsLine != null)
                {
                    startPoint = vitalSignsLine.StartPointF;
                    endPoint = vitalSignsLine.EndPointF;

                    #region 判断线段是否需要绘制
                    bool isContinue = false;

                    //if (startPoint.Y != endPoint.Y)
                    //{
                        foreach (int pointX in m_Need断开LocationX)
                        {
                            if (startPoint.X <= pointX && pointX + m_LineHeight2 <= endPoint.X)
                            {
                                isContinue = true;
                            }
                        }
                    //}
                    if (isContinue)
                    {
                        continue;
                    }
                    #endregion

                    type = vitalSignsLine.Type;
                    Pen pen = new Pen(GetBrushByColorName(vitalSignsLine.ColorName));

                    //绘制点于点之间的直线
                    //此处有物理降温??
                    if ((vitalSignsLine.MainType.ToString() == VitalSignsType.TiWen.ToString() || vitalSignsLine.MainType.ToString() == VitalSignsType.WuLiJiangWen.ToString() ||
                        vitalSignsLine.MainType.ToString() == VitalSignsType.WuLiShengWen.ToString())
                        && m_IsShowTiWen == false)
                    {
                        continue;
                    }
                    if ((vitalSignsLine.MainType.ToString() == VitalSignsType.MaiBo.ToString() || vitalSignsLine.MainType.ToString() == VitalSignsType.XinLv.ToString())
                        && m_IsShowMaiBo == false)
                    {
                        continue;
                    }
                    if (vitalSignsLine.MainType.ToString() == VitalSignsType.HuXi.ToString() && m_IsShowHuXi == false)
                    {
                        continue;
                    }

                    if (type == Convert.ToString((int)LineType.SolidLine))
                        //g.DrawLine(pen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);edit by ywk 
                        g.DrawLine(pen, startPoint.X - 3, startPoint.Y - 3, endPoint.X - 3, endPoint.Y - 3);
                    else if (type == Convert.ToString((int)LineType.DashLine))
                    {
                        pen.DashPattern = new float[] { 3f, 3f };
                        //g.DrawLine(pen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);edit by ywk 
                        g.DrawLine(pen, startPoint.X - 3, startPoint.Y - 3, endPoint.X - 3, endPoint.Y - 3);
                    }
                }
            }
        }

        #endregion

        #endregion

        #endregion

        #region 绘制表格的第三部分
        /// <summary>
        /// 绘制表格的第三部分
        /// </summary>
        /// <param name="pictureBox"></param>
        /// <param name="g"></param>
        private void PaintThirdPartOfTable(PictureBox pictureBox, Graphics g)
        {
            //记录小方格曲线图下第一行Y轴方向的值
            int currentLinePointY = m_TableStartPointY + (m_LineHeight1 + 1) * m_DataTableTableBaseLine.Rows.Count
                + (m_CellCount / m_CellCountInEveryDegree) * ((m_LineHeight2 + 1) * m_CellCountInEveryDegree + 1)
                + (m_CellCount % m_CellCountInEveryDegree) * (m_LineHeight2 + 1);

            //记录小方格曲线图下第一行X轴方向的值
            int currentLinePointX = m_TableStartPointX;

            //此宽度不包括表格最左边1像素，包括表格最右边2像素
            int tableWidth = m_FirstColumnWidth/*第一列的宽度*/ + 1 + (m_LineHeight2 * m_DayTimePoint + m_DayTimePoint) * m_Days + 1;

            #region 字体 画刷 居中
            Pen penBlack = Pens.Black;
            Pen penBlackBold = new Pen(Brushes.Black, 2);
            Brush brushBlack = Brushes.Black;
            Brush brushRed = Brushes.Red;
            Font fontNormal = this.Font;
            Font fontSmall = new Font(fontNormal.FontFamily, fontNormal.Size - 1.5f);

            StringFormat centerFormat = new StringFormat();
            centerFormat.Alignment = StringAlignment.Center;
            centerFormat.LineAlignment = StringAlignment.Center;

            StringFormat leftFormat = new StringFormat();
            leftFormat.Alignment = StringAlignment.Near;
            leftFormat.LineAlignment = StringAlignment.Center;
            #endregion

            #region 呼吸-------如果呼吸不在曲线图中显示，则需要以独立的一行显示

            //if (PatientInfo.DataTableHuXi.Rows.Count > 0)
            {

                //判断呼吸是否显示曲线图中
                bool isHasHuXi = checkIsContainHuXiInCurve();

                //由于呼吸不在曲线图中，所以要在下面的一行中将呼吸的数据显示出来
                if (isHasHuXi == false)
                {

                    #region 绘制横线 竖线 左边的文字

                    //在单独的一行中显示呼吸的数据
                    g.DrawLine(penBlack, currentLinePointX, currentLinePointY + m_LineHeight2 * 2, currentLinePointX + tableWidth, currentLinePointY + m_LineHeight2 * 2);

                    //绘制呼吸行中各个时间段的竖线
                    for (int i = 1; i < m_DayTimePoint * m_Days; i++)
                    {
                        if (i % m_DayTimePoint != 0)
                        {
                            int startPointX = currentLinePointX + m_FirstColumnWidth + 1 + (m_LineHeight2 + 1) * i;
                            int startPointY = currentLinePointY;
                            int endPointX = startPointX;
                            int endPointY = startPointY + m_LineHeight2 * 2;

                            g.DrawLine(penBlack, new Point(startPointX, startPointY), new Point(endPointX, endPointY));
                        }
                    }

                    //绘制呼吸名称以及单位
                    g.DrawString("呼吸(次/分)", fontNormal, brushBlack, new RectangleF(currentLinePointX + m_RowCaptionIndent, currentLinePointY + 2, m_FirstColumnWidth, m_LineHeight2 * 2), leftFormat);

                    #endregion

                    #region 先计算绘制的位置，然后再将其绘制出来
                    //将呼吸的值绘制到相应的位置上
                    DateTime currentDateTime = DateTime.MinValue;//用于记录当前循环的值

                    ArrayList list = new ArrayList();

                    //记录当前呼吸的索引，用于判断上下显示呼吸的位置
                    int drawHuXiIndex = 0;

                    //通过配置决定第一个呼吸的显示位置，是上方还是下方
                    int modValue = 0;//余数，用于判断呼吸位置的上下
                    if (m_FirstHuXiUp == "0")
                    {
                        modValue = 1;
                    }

                    for (int i = 0; i < PatientInfo.DataTableHuXi.Rows.Count; i++)
                    {
                        currentDateTime = Convert.ToDateTime(PatientInfo.DataTableHuXi.Rows[i][0]);

                        #region 计算一天记录呼吸的次数
                        int sameDateCount = 1;
                        for (int j = i + 1; j < PatientInfo.DataTableHuXi.Rows.Count; j++)
                        {
                            DateTime dt = Convert.ToDateTime(PatientInfo.DataTableHuXi.Rows[j][0]);
                            if (currentDateTime == dt)
                            {
                                sameDateCount++;
                            }
                        }
                        #endregion

                        //每日记录呼吸2次以上，应当在相应的栏目内上下交错记录，第1次呼吸应当记录在上方
                        int m = 0;
                        for (; m < sameDateCount; m++)
                        {
                            currentDateTime = Convert.ToDateTime(PatientInfo.DataTableHuXi.Rows[i + m]["DateTime"]);
                            string timePoint = PatientInfo.DataTableHuXi.Rows[i + m]["TimePoint"].ToString().Trim();
                            string value = PatientInfo.DataTableHuXi.Rows[i + m]["Value"].ToString().Trim();
                            string memo = PatientInfo.DataTableHuXi.Rows[i + m]["Memo"].ToString().Trim();
                            string linkNext = PatientInfo.DataTableHuXi.Rows[i + m]["LinkNext"].ToString().Trim();
                            string IsSpecial = PatientInfo.DataTableHuXi.Rows[i + m]["IsSpecial"].ToString().Trim();

                            float locationPointX = computeLocationBottomX(currentDateTime, timePoint);//呼吸数据在X轴方向上的坐标
                            {
                                RectangleF rectForHuXiJi = new RectangleF(locationPointX + 1, currentLinePointY + 1, m_LineHeight2 - 1, m_LineHeight2 - 1);
                                if (IsSpecial == "Y")//绘制呼吸机
                                {
                                    //g.DrawImage(m_Picture.BitmapHuXiSpecial, rectForHuXiJi);//顶格显示呼吸机
                                    m_Picture.DrawHuXiJi(g, rectForHuXiJi.X, rectForHuXiJi.Y);
                                }
                                else
                                {

                                    if (drawHuXiIndex % 2 == modValue)//每天第1次呼吸应当记录在上方，其他的依次交错记录 xll改 一张表的所有呼吸上下排序
                                    {
                                        g.DrawString(value, fontSmall, brushBlack, locationPointX, currentLinePointY + m_LineHeight2 - TextRenderer.MeasureText(value, fontSmall).Height);//显示在上方
                                    }
                                    else
                                    {
                                        g.DrawString(value, fontSmall, brushBlack, locationPointX, currentLinePointY + m_LineHeight2 * 2 - TextRenderer.MeasureText(value, fontSmall).Height);//显示在下方
                                    }
                                    drawHuXiIndex++;
                                }
                            }
                        }
                        i += m - 1;
                    }
                    #endregion

                    currentLinePointY += m_LineHeight2 * 2 + 1;
                }
            }

            #endregion

            #region 绘制血压，总入量，总出量，引流量，身高，体重，过敏药物，特殊治疗，其他1， 其他2，参数1，参数2

            //Add by wwj 2012-06-05 根据配置决定栏位的显示
            int needAddCount = 0;
            for (int i = 0; i < m_ArrayListOther.Count; i++)
            {
                VitalSignsOther vso = m_ArrayListOther[i] as VitalSignsOther;
                if (vso != null)
                {
                    if (vso.Name == VitalSignsType.XueYa.ToString()) //血压
                    {
                        #region 血压
                        int timePointOfDay = vso.TimePointOfDay;//每天记录血压的时间点的数目
                        int xuYaCellWidth = (m_LineHeight2 + 1) * m_DayTimePoint / timePointOfDay;//血压每一小格的宽度
                        string bloodPressureFlag = MethodSet.GetConfigValueByKey("BloodPressureFlag");

                        //绘制血压的横线
                        g.DrawLine(penBlack, currentLinePointX, currentLinePointY + m_LineHeight1 + 1, currentLinePointX + tableWidth, currentLinePointY + m_LineHeight1 + 1);
                        #region "绘制血压每天隔开的竖线 - 根据配置"
                        if (bloodPressureFlag == "0")
                        {
                            for (int m = 0; m < m_Days; m++)
                            {
                                for (int j = 1; j < timePointOfDay; j++)
                                {
                                    int startPointX = currentLinePointX + m_FirstColumnWidth + 1 + (m_LineHeight2 + 1) * m_DayTimePoint * m + xuYaCellWidth * j;
                                    int startPointY = currentLinePointY;
                                    int endPointX = startPointX;
                                    int endPointY = currentLinePointY + m_LineHeight1;

                                    g.DrawLine(penBlack, startPointX, startPointY, endPointX, endPointY);
                                }
                            }
                        }

                        #endregion
                        //绘制“血压”
                        string unit = vso.Unit.Trim().Length > 0 ? "(" + vso.Unit + ")" : "";
                        g.DrawString(GetVitalSignsTypeName(vso.Name) + unit, fontNormal, brushBlack, new RectangleF(currentLinePointX + m_RowCaptionIndent, currentLinePointY + 2, m_FirstColumnWidth, m_LineHeight1), leftFormat);

                        //绘制血压的值
                        //by cyq 2012-09-06 17:00
                        #region "绘制血压的值"
                        var xueYaEnu = PatientInfo.DateTableXueYa.AsEnumerable();
                        for (int m = 0; m < PatientInfo.DateTableXueYa.Rows.Count; m++)
                        {
                            DateTime dateTime = Convert.ToDateTime(PatientInfo.DateTableXueYa.Rows[m]["DateTime"]);
                            string timePoint = PatientInfo.DateTableXueYa.Rows[m]["TimePoint"].ToString();
                            int count = xueYaEnu.Where(p => DateTime.Parse(p["DateTime"].ToString()).ToString("yyyy-MM-dd") == dateTime.ToString("yyyy-MM-dd")).CopyToDataTable().Rows.Count;
                            //绘制竖线 by cyq 2012-09-07 09:30
                            if (count == 2 && bloodPressureFlag != "0")
                            {
                                int num = new PatientInfo().GetDayOfCurrentDays(m_DataTimeAllocate, dateTime) - 1;
                                for (int j = 1; j < timePointOfDay; j++)
                                {
                                    int startPointX = currentLinePointX + m_FirstColumnWidth + 1 + (m_LineHeight2 + 1) * m_DayTimePoint * num + xuYaCellWidth * j;
                                    int startPointY = currentLinePointY;
                                    int endPointX = startPointX;
                                    int endPointY = currentLinePointY + m_LineHeight1;

                                    g.DrawLine(penBlack, startPointX, startPointY, endPointX, endPointY);
                                }
                            }

                            string value = PatientInfo.DateTableXueYa.Rows[m]["Value"].ToString();
                            float width = (m_DayTimePoint * (m_LineHeight2 + 1)) / m_DayTimePointXuYa + 2;
                            float startPointXX = computeLocationBottomX(dateTime, timePoint) - 1;
                            if (bloodPressureFlag != "0" && count == 1)//下午
                            {
                                //by cyq 2012-09-07 09:30
                                //当这一天只有半天的记录时
                                //1、将格子宽度设为这一天格子的宽度
                                //2、如果这天记录为下午的记录，将起始位置减去这天格子宽度的一半
                                width = width * 2;
                                if (int.Parse(timePoint) >= 12)
                                {
                                    startPointXX -= (m_LineHeight2 + 1) * (m_DayTimePoint / 2);
                                }
                            }
                            float startPointYY = currentLinePointY;
                            g.DrawString(value, fontSmall, brushBlack, new RectangleF(startPointXX, startPointYY, width, m_LineHeight1), centerFormat);
                        }
                        #endregion

                        currentLinePointY += m_LineHeight1 + 1;
                        #endregion
                    }
                    else
                    {
                        #region 绘制 总入量，总出量，引流量，大便次数，身高，体重，过敏药物，特殊治疗，其他1，其他2

                        DataTable data = new DataTable();

                        if (vso.Name == VitalSignsType.ZongRuLiang.ToString())    //总入量
                        {
                            data = PatientInfo.DataTableZongRuLiang;
                        }
                        else if (vso.Name == VitalSignsType.ZongChuLiang.ToString())//总出量
                        {
                            data = PatientInfo.DataTableZongChuLiang;
                        }
                        else if (vso.Name == VitalSignsType.YinLiuLiang.ToString())//引流量
                        {
                            data = PatientInfo.DataTableYinLiuLiang;
                        }
                        else if (vso.Name == VitalSignsType.DaBianCiShu.ToString())//大便次数
                        {
                            data = PatientInfo.DataTableDaBianCiShu;
                        }
                        else if (vso.Name == VitalSignsType.ShenGao.ToString())//身高
                        {
                            data = PatientInfo.DataTableShenGao;
                        }
                        else if (vso.Name == VitalSignsType.TiZhong.ToString())//体重
                        {
                            data = PatientInfo.DataTableTiZhong;
                        }
                        else if (vso.Name == VitalSignsType.GuoMingYaoWu.ToString())//过敏药物
                        {
                            data = PatientInfo.DataTableGuoMinYaoWu;
                        }
                        else if (vso.Name == VitalSignsType.TeShuZhiLiao.ToString())//特殊治疗
                        {
                            data = PatientInfo.DataTableTeShuZhiLiao;
                        }
                        //else if (vso.Name == VitalSignsType.Other1.ToString())//其他1
                        //{
                        //    data = PatientInfo.DataTableOther1;
                        //}
                        //else if (vso.Name == VitalSignsType.PainInfo.ToString())//其他1 现在改为疼痛
                        //{
                        //    data = PatientInfo.DataTablePainInfo;
                        //}
                        else if (vso.Name == VitalSignsType.Other2.ToString())//其他2 改为了尿量，此处注释掉
                        {
                            data = PatientInfo.DataTableOther2;
                        }

                        int timePointOfDay = vso.TimePointOfDay;//每天记录总入量的时间点的数目
                        int xuYaCellWidth = (m_LineHeight2 + 1) * m_DayTimePoint / timePointOfDay;//总入量中每一小格的宽度

                        //绘制横线
                        g.DrawLine(penBlack, currentLinePointX, currentLinePointY + m_LineHeight1 + 5,
                            currentLinePointX + tableWidth,
                            currentLinePointY + m_LineHeight1 + 5);//3  血压的横线绘制

                        #region 疼痛放这判断
                        //疼痛栏位放在血压下面---------- add by ywk
                        if (vso.Name == VitalSignsType.PainInfo.ToString()) //对疼痛的处理
                        {
                            #region  疼痛

                            #region 现在的方法
                            int TengTongCellWidth = (m_LineHeight2 + 1) * m_DayTimePoint / timePointOfDay;//疼痛每一小格的宽度

                            //绘制疼痛的横线
                            g.DrawLine(penBlack, currentLinePointX, currentLinePointY + m_LineHeight1 + 5, currentLinePointX + tableWidth, currentLinePointY + m_LineHeight1 + 5);//+1 ywk   疼痛的横线绘制

                            //绘制“疼痛”
                            string unit = vso.Unit.Trim().Length > 0 ? "(" + vso.Unit + ")" : "";
                            g.DrawString(GetVitalSignsTypeName(vso.Name) + unit, fontNormal, brushBlack, new RectangleF(currentLinePointX + m_RowCaptionIndent, currentLinePointY + 4, m_FirstColumnWidth, m_LineHeight1), leftFormat);


                            //绘制疼痛每天隔开的竖线
                            for (int m = 0; m < m_Days; m++)
                            {
                                for (int j = 1; j < timePointOfDay; j++)
                                {
                                    int startPointX = currentLinePointX + m_FirstColumnWidth + 1 + (m_LineHeight2 + 1) * m_DayTimePoint * m + TengTongCellWidth * j;
                                    int startPointY = currentLinePointY;
                                    int endPointX = startPointX;
                                    int endPointY = currentLinePointY + m_LineHeight1 + 5;

                                    g.DrawLine(penBlack, startPointX, startPointY, endPointX, endPointY);
                                }
                            }

                            //绘制疼痛的值
                            for (int m = 0; m < PatientInfo.DataTablePainInfo.Rows.Count; m++)
                            {
                                DateTime dateTime = Convert.ToDateTime(PatientInfo.DataTablePainInfo.Rows[m]["DateTime"]);
                                string timePoint = PatientInfo.DataTablePainInfo.Rows[m]["TimePoint"].ToString();
                                //int indx = -1;
                                //if (PatientInfo.DataTablePainInfo.Rows[m]["Indx"] == null || PatientInfo.DataTablePainInfo.Rows[m]["Indx"].ToString() == "")
                                //{
                                //    continue;
                                //}
                                //indx = int.Parse(PatientInfo.DataTablePainInfo.Rows[m]["Indx"].ToString());
                                string value = PatientInfo.DataTablePainInfo.Rows[m]["Value"].ToString();

                                float startPointX = computeLocationBottomX(dateTime, timePoint) - 1;
                                float startPointY = currentLinePointY;

                                //float width = (m_DayTimePoint * (m_LineHeight2 + 1)) / m_DayTimePointXuYa + 2;

                                if (timePoint == TimesArray[1].ToString() || timePoint == TimesArray[3].ToString() || timePoint == TimesArray[5].ToString())//第1个记录在下方，其他的依次交错记录(m)3,7,11,15,19,23(此处先这样限制)7  15  23
                                //if (indx == 1 || indx == 3 || indx == 5)//第1个记录在下方，其他的依次交错记录(m)3,7,11,15,19,23(此处先这样限制)7  15  23
                                {
                                    g.DrawString(value, fontNormal, brushRed, startPointX + 3, startPointY + m_LineHeight2 - TextRenderer.MeasureText(value, fontNormal).Height);//显示在上方
                                }
                                else
                                {
                                    g.DrawString(value, fontNormal, brushRed, startPointX + 3, startPointY + m_LineHeight2 * 2 - TextRenderer.MeasureText(value, fontNormal).Height);//显示在下方
                                }
                                //DateTime currentDateTime = DateTime.MinValue;//用于记录当前循环的值
                                //currentDateTime = Convert.ToDateTime(PatientInfo.DataTablePainInfo.Rows[i][0]);

                                //#region 计算一天记录疼痛的次数
                                //int sameDateCount = 1;
                                //for (int j = i + 1; j < PatientInfo.DataTablePainInfo.Rows.Count; j++)
                                //{
                                //    DateTime dt = Convert.ToDateTime(PatientInfo.DataTablePainInfo.Rows[j][0]);
                                //    if (currentDateTime == dt)
                                //    {
                                //        sameDateCount++;
                                //    }
                                //}
                                //#endregion

                                ////上下交错记录，第1次应当记录在下方
                                //int h = 0;
                                //for (; h < sameDateCount; h++)
                                //{
                                //    currentDateTime = Convert.ToDateTime(PatientInfo.DataTablePainInfo.Rows[i + h]["DateTime"]);
                                //    string timePoint = PatientInfo.DataTablePainInfo.Rows[i +h]["TimePoint"].ToString().Trim();
                                //    string value = PatientInfo.DataTablePainInfo.Rows[i + h]["Value"].ToString().Trim();
                                //    //float locationPointX = computeLocationBottomX(currentDateTime, timePoint);//疼痛数据在X轴方向上的坐标
                                //    float startPointX = computeLocationBottomX(currentDateTime, timePoint) - 1;
                                //    float startPointY = currentLinePointY;                                  
                                //    if (h % 2 == 0)//先下后上，交错记录
                                //    {
                                //        g.DrawString(value, fontNormal, brushRed, startPointX + 3, startPointY + m_LineHeight2 - TextRenderer.MeasureText(value, fontNormal).Height - 2);//显示在上方
                                //    }
                                //    else
                                //    {
                                //        g.DrawString(value, fontNormal, brushRed, startPointX + 3, startPointY + m_LineHeight2 * 2 - TextRenderer.MeasureText(value, fontNormal).Height - 2);//显示在下方
                                //    }
                                //}
                                //i += h - 1;
                            }
                            #endregion

                            #region 呼吸的做法
                            //在单独的一行中显示疼痛的数据
                            //g.DrawLine(penBlack, currentLinePointX, currentLinePointY + m_LineHeight2 * 2, currentLinePointX + tableWidth, currentLinePointY + m_LineHeight2 * 2);

                            ////绘制疼痛行中各个时间段的竖线
                            //for (int n = 1; n < m_DayTimePoint * m_Days; n++)
                            //{
                            //    if (n % m_DayTimePoint != 0)
                            //    {
                            //        int startPointX = currentLinePointX + m_FirstColumnWidth + 1 + (m_LineHeight2 + 1) * n;
                            //        int startPointY = currentLinePointY;
                            //        int endPointX = startPointX;
                            //        int endPointY = startPointY + m_LineHeight2 * 2;

                            //        g.DrawLine(penBlack, new Point(startPointX, startPointY), new Point(endPointX, endPointY));
                            //    }
                            //}

                            ////绘制呼吸名称以及单位
                            //g.DrawString("疼痛指数", fontNormal, brushBlack, new RectangleF(currentLinePointX + m_RowCaptionIndent, currentLinePointY + 1, m_FirstColumnWidth, m_LineHeight2 * 2), leftFormat);



                            //DateTime currentDateTime = DateTime.MinValue;//用于记录当前循环的值

                            //ArrayList list = new ArrayList();
                            //for (int k = 0; k < PatientInfo.DataTableHuXi.Rows.Count; k++)
                            //{
                            //    currentDateTime = Convert.ToDateTime(PatientInfo.DataTableHuXi.Rows[k][0]);

                            //    #region 计算一天记录呼吸的次数
                            //    int sameDateCount = 1;
                            //    for (int j = k + 1; j < PatientInfo.DataTableHuXi.Rows.Count; j++)
                            //    {
                            //        DateTime dt = Convert.ToDateTime(PatientInfo.DataTableHuXi.Rows[j][0]);
                            //        if (currentDateTime == dt)
                            //        {
                            //            sameDateCount++;
                            //        }
                            //    }
                            //    #endregion

                            //    //每日记录疼痛2次以上，应当在相应的栏目内上下交错记录，第1次疼痛应当记录在上方
                            //    int m = 0;
                            //    for (; m < sameDateCount; m++)
                            //    {
                            //        currentDateTime = Convert.ToDateTime(PatientInfo.DataTablePainInfo.Rows[k + m]["DateTime"]);
                            //        string timePoint = PatientInfo.DataTablePainInfo.Rows[k + m]["TimePoint"].ToString().Trim();
                            //        string value = PatientInfo.DataTablePainInfo.Rows[k + m]["Value"].ToString().Trim();


                            //        float locationPointX = computeLocationBottomX(currentDateTime, timePoint);//疼痛数据在X轴方向上的坐标

                            //        if (sameDateCount == 1)//一天记录疼痛一次，则在栏目中居中显示
                            //        {
                            //            g.DrawString(value, fontSmall, brushRed, new RectangleF(locationPointX - 1, currentLinePointY, m_LineHeight2 + 2, m_LineHeight2 * 2), centerFormat);//居中显示
                            //        }
                            //        else
                            //        {
                            //            RectangleF rectForHuXiJi = new RectangleF(locationPointX + 1, currentLinePointY + 1, m_LineHeight2 - 1, m_LineHeight2 - 1);
                            //            //if (IsSpecial == "Y")//绘制呼吸机
                            //            //{
                            //            //    //g.DrawImage(m_Picture.BitmapHuXiSpecial, rectForHuXiJi);//顶格显示呼吸机
                            //            //    m_Picture.DrawHuXiJi(g, rectForHuXiJi.X, rectForHuXiJi.Y);
                            //            //}
                            //            //else
                            //            //{
                            //                if (m % 2 == 0)//每天的第1次疼痛应当记录在上方，其他的依次交错记录
                            //                {
                            //                    g.DrawString(value, fontSmall, brushRed, locationPointX, currentLinePointY + m_LineHeight2 - TextRenderer.MeasureText(value, fontSmall).Height);//显示在上方
                            //                }
                            //                else
                            //                {
                            //                    g.DrawString(value, fontSmall, brushRed, locationPointX, currentLinePointY + m_LineHeight2 * 2 - TextRenderer.MeasureText(value, fontSmall).Height);//显示在下方
                            //                }
                            //            //}
                            //        }
                            //    }
                            //    i += m - 1;
                            //}
                            #endregion
                            #endregion
                        }

                        #endregion

                        //绘制栏位名称(疼痛不放在最下面，新需求改为放在血压下面)
                        if (vso.Name != VitalSignsType.PainInfo.ToString() && vso.Name != VitalSignsType.XueYa.ToString())//其他
                        //if (vso.Name != VitalSignsType.Other2.ToString())//其他（去除其他2的判断）
                        {
                            string unit = vso.Unit.Trim().Length > 0 ? "(" + vso.Unit + ")" : "";
                            g.DrawString(GetVitalSignsTypeName(vso.Name) + unit, fontNormal, brushBlack,
                                new RectangleF(currentLinePointX + m_RowCaptionIndent, currentLinePointY + 6, m_FirstColumnWidth, m_LineHeight1), leftFormat);//Y+2  +4

                            //绘制数值
                            for (int m = 0; m < data.Rows.Count; m++)
                            {
                                DateTime dateTime = Convert.ToDateTime(data.Rows[m]["DateTime"]);
                                string timePoint = data.Rows[m]["TimePoint"].ToString();
                                //int indx = -1;
                                //if (data.Rows[m]["Indx"] == null || data.Rows[m]["Indx"].ToString() == "")
                                //{
                                //    continue;
                                //}
                                //indx = int.Parse(data.Rows[m]["Indx"].ToString());
                                string value = data.Rows[m]["Value"].ToString();


                                float startPointX = computeLocationBottomX(dateTime, timePoint);
                                float startPointY = currentLinePointY;

                                float width = m_DayTimePoint * (m_LineHeight2 + 1);

                                if (vso.Name == VitalSignsType.DaBianCiShu.ToString())//大便次数
                                {
                                    #region 绘制“大便次数”
                                    if (value.Split(':').Length == 3)
                                    {
                                        string[] valueDaBianXiShuX = value.Split(':');
                                        string valueTemp = valueDaBianXiShuX[0] + valueDaBianXiShuX[1] + "/" + valueDaBianXiShuX[2];
                                        /*
                                         * 由于大便次数比较特殊，所以这里要进行特殊处理
                                         * 特殊情况：患者无大便，以“0”表示；灌肠后大便以“E”表示，分子记录大便次数
                                         * 例：1/E表示灌肠后大便1次；0/E表示灌肠后无排便；11/E表示自行排便1次灌肠后又排便1次；
                                         * “※”表示大便失禁，“☆”表示人工肛门
                                         */
                                        if (value.Trim().StartsWith("※") || value.Trim().StartsWith("☆"))
                                        {
                                            g.DrawString(valueDaBianXiShuX[0].ToString(), fontNormal, brushBlack, new RectangleF(startPointX, startPointY + 2, width, m_LineHeight1), centerFormat);
                                        }
                                        else if (value.Trim() != "::")
                                        {
                                            float startPaintDaBianXiShuX = startPointX + (width - TextRenderer.MeasureText(valueTemp, fontNormal).Width) / 2;
                                            int increaseWidth;

                                            if (valueDaBianXiShuX.Length == 3)
                                            {
                                                if (string.IsNullOrEmpty(valueDaBianXiShuX[1].ToString()) && string.IsNullOrEmpty(valueDaBianXiShuX[2].ToString()))
                                                {

                                                    increaseWidth = TextRenderer.MeasureText(valueDaBianXiShuX[0], fontNormal).Width - 4;
                                                    g.DrawString(valueDaBianXiShuX[0], fontNormal, brushBlack, startPaintDaBianXiShuX + increaseWidth, startPointY + 9);
                                                }
                                                else
                                                {
                                                    g.DrawString(valueDaBianXiShuX[0], fontNormal, brushBlack, startPaintDaBianXiShuX, startPointY + 9);
                                                    increaseWidth = TextRenderer.MeasureText(valueDaBianXiShuX[0], fontNormal).Width - 4;

                                                    g.DrawString(valueDaBianXiShuX[2], fontSmall, brushBlack, startPaintDaBianXiShuX + increaseWidth, startPointY + 8);
                                                    increaseWidth += TextRenderer.MeasureText(valueDaBianXiShuX[2], fontSmall).Width - 5;

                                                    g.DrawString("/", fontNormal, brushBlack, startPaintDaBianXiShuX + increaseWidth + 2, startPointY + 9);//--调整/的位置
                                                    increaseWidth += TextRenderer.MeasureText("/", fontNormal).Width - 4;

                                                    g.DrawString(valueDaBianXiShuX[1], fontNormal, brushBlack, startPaintDaBianXiShuX + increaseWidth, startPointY + 9);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //当为“：：”时，什么都不显示
                                        }
                                    }
                                    #endregion
                                }
                                else if (vso.Name == VitalSignsType.GuoMingYaoWu.ToString())//过敏药物
                                {
                                    //过敏药物，超出单元格内容就延后显示。不超过就居中显示
                                    //add by ywk 2012年5月21日 09:46:02
                                    int valueWidth = TextRenderer.MeasureText(value, fontNormal).Width;
                                    if (valueWidth > 77)//超出单元格长度的处理
                                    {
                                        //g.DrawString(value, fontNormal, brushBlack, new RectangleF(startPointX, startPointY + 5, width * m_Days, m_LineHeight1 + 2), leftFormat);//+2调整栏位里面数值的位置    
                                        g.DrawString(value, fontNormal, brushBlack, new RectangleF(startPointX, startPointY + 5, width * m_Days, m_LineHeight1 + 2), leftFormat);//+2调整栏位里面数值的位置  
                                    }
                                    else//居中显示
                                    {
                                        g.DrawString(value, fontNormal, brushBlack, new RectangleF(startPointX, startPointY + 5, width, m_LineHeight1 + 2), centerFormat);//+2调整栏位里面数值的位置
                                    }
                                }
                                else
                                {
                                    g.DrawString(value, fontNormal, brushBlack, new RectangleF(startPointX, startPointY + 5, width, m_LineHeight1 + 2), centerFormat);//+2调整栏位里面数值的位置
                                }
                            }
                        }
                        currentLinePointY += m_LineHeight1 + 2;//2
                        #endregion
                    }
                }

            }

            //绘制表格的最后一条粗线
            g.DrawLine(penBlackBold, currentLinePointX, currentLinePointY + 4, currentLinePointX + tableWidth, currentLinePointY + 4);//Y+3  +4调整表格底部的横线

            #endregion

            #region 注释 绘制血压，总入量，总出量，引流量，身高，体重，过敏药物，特殊治疗，其他1， 其他2

            ////Add by wwj 2012-06-05 根据配置决定栏位的显示
            //for (int i = m_ArrayListOther.Count - 1; i >= 0; i--)
            //{
            //    VitalSignsOther vso = m_ArrayListOther[i] as VitalSignsOther;
            //    if (vso.Name == VitalSignsType.PainInfo.ToString() && m_IsShowTengTongZhiShu == "0")
            //    {
            //        m_ArrayListOther.RemoveAt(i);
            //    }
            //}

            ////画”排出量“
            //g.DrawString("排", fontNormal, brushBlack, new RectangleF(currentLinePointX + 20, currentLinePointY + 4, m_FirstColumnWidth, m_LineHeight1));
            //g.DrawString("出", fontNormal, brushBlack, new RectangleF(currentLinePointX + 20, currentLinePointY + 26, m_FirstColumnWidth, m_LineHeight1));
            //g.DrawString("量", fontNormal, brushBlack, new RectangleF(currentLinePointX + 20, currentLinePointY + 46, m_FirstColumnWidth, m_LineHeight1));

            //for (int i = 0; i < m_ArrayListOther.Count; i++)
            //{
            //    VitalSignsOther vso = m_ArrayListOther[i] as VitalSignsOther;
            //    if (vso != null)
            //    {
            //        //先画排出量（大便次数，尿量,还有个空格行）add by ywk 2012年5月17日 17:02:21
            //        if (vso.Name == VitalSignsType.DaBianCiShu.ToString() || vso.Name == VitalSignsType.ZongChuLiang.ToString())//判断为大便次数时，就画
            //        {
            //            //先画竖线
            //            int xPoint = m_TableStartPointX + (m_FirstColumnWidth / m_FirstColumnHasSubColumnCount);
            //            g.DrawLine(Pens.Black, xPoint, currentLinePointY - 20, xPoint,
            //                currentLinePointY + m_LineHeight1 * 2 + 3);//15  17

            //            //画排出量最下面的横线 
            //            g.DrawLine(penBlack, currentLinePointX, currentLinePointY + m_LineHeight1 * 3 + 4, currentLinePointX + tableWidth, currentLinePointY + m_LineHeight1 * 3 + 4);
            //            //画排出量单元行内的三行内容（大便次数、尿量、空格行）
            //            for (int k = 1; k < 2; k++)
            //            {
            //                int pointY = currentLinePointY + (m_LineHeight1) * k + 1;
            //                g.DrawLine(penBlack, currentLinePointX + 58, pointY, currentLinePointX + tableWidth, pointY);
            //            }

            //            //画”大便次数“尿量的文字
            //            DataTable data = new DataTable();
            //            if (vso.Name == VitalSignsType.DaBianCiShu.ToString())
            //            {
            //                data = PatientInfo.DataTableDaBianCiShu;
            //            }
            //            else if (vso.Name == VitalSignsType.ZongChuLiang.ToString())
            //            {
            //                data = PatientInfo.DataTableZongChuLiang;//总出量作为尿量
            //            }
            //            g.DrawString(GetVitalSignsTypeName(vso.Name), fontNormal, brushBlack, new RectangleF(currentLinePointX + 84, currentLinePointY + 2, m_FirstColumnWidth, m_LineHeight1), leftFormat);
            //            //画上相应的值 
            //            for (int j = 0; j < data.Rows.Count; j++)
            //            {
            //                DateTime dateTime = Convert.ToDateTime(data.Rows[j]["DateTime"]);
            //                string timePoint = data.Rows[j]["TimePoint"].ToString();
            //                string value = data.Rows[j]["Value"].ToString();
            //                float startPointX = computeLocationBottomX(dateTime, timePoint);
            //                float startPointY = currentLinePointY;
            //                float width = m_DayTimePoint * (m_LineHeight2 + 1);

            //                if (vso.Name == VitalSignsType.DaBianCiShu.ToString())//大便次数
            //                {
            //                    #region 绘制“大便次数”
            //                    if (value.Split(':').Length == 3)
            //                    {
            //                        string[] valueDaBianXiShuX = value.Split(':');
            //                        string valueTemp = valueDaBianXiShuX[0] + valueDaBianXiShuX[1] + "/" + valueDaBianXiShuX[2];
            //                        /*
            //                         * 由于大便次数比较特殊，所以这里要进行特殊处理
            //                         * 特殊情况：患者无大便，以“0”表示；灌肠后大便以“E”表示，分子记录大便次数
            //                         * 例：1/E表示灌肠后大便1次；0/E表示灌肠后无排便；11/E表示自行排便1次灌肠后又排便1次；
            //                         * “※”表示大便失禁，“☆”表示人工肛门
            //                         */
            //                        if (value.Trim().StartsWith("※") || value.Trim().StartsWith("☆"))
            //                        {
            //                            g.DrawString(valueDaBianXiShuX[0].ToString(), fontNormal, brushBlack, new RectangleF(startPointX, startPointY + 2, width, m_LineHeight1), centerFormat);
            //                        }
            //                        else if (value.Trim() != "::")
            //                        {
            //                            float startPaintDaBianXiShuX = startPointX + (width - TextRenderer.MeasureText(valueTemp, fontNormal).Width) / 2;
            //                            int increaseWidth;

            //                            if (valueDaBianXiShuX.Length == 3)
            //                            {
            //                                if (string.IsNullOrEmpty(valueDaBianXiShuX[1].ToString()) && string.IsNullOrEmpty(valueDaBianXiShuX[2].ToString()))
            //                                {

            //                                    increaseWidth = TextRenderer.MeasureText(valueDaBianXiShuX[0], fontNormal).Width - 4;
            //                                    g.DrawString(valueDaBianXiShuX[0], fontNormal, brushBlack, startPaintDaBianXiShuX + increaseWidth, startPointY + 4);//9
            //                                }
            //                                else
            //                                {
            //                                    g.DrawString(valueDaBianXiShuX[0], fontNormal, brushBlack, startPaintDaBianXiShuX, startPointY + 4);//9
            //                                    increaseWidth = TextRenderer.MeasureText(valueDaBianXiShuX[0], fontNormal).Width - 4;

            //                                    g.DrawString(valueDaBianXiShuX[2], fontSmall, brushBlack, startPaintDaBianXiShuX + increaseWidth, startPointY + 3);//Y+8
            //                                    increaseWidth += TextRenderer.MeasureText(valueDaBianXiShuX[2], fontSmall).Width - 5;

            //                                    g.DrawString("/", fontNormal, brushBlack, startPaintDaBianXiShuX + increaseWidth + 2, startPointY + 4);//--调整/的位置//9
            //                                    increaseWidth += TextRenderer.MeasureText("/", fontNormal).Width - 4;

            //                                    g.DrawString(valueDaBianXiShuX[1], fontNormal, brushBlack, startPaintDaBianXiShuX + increaseWidth, startPointY + 4);//9
            //                                }
            //                            }
            //                        }
            //                        else
            //                        {
            //                            //当为“：：”时，什么都不显示
            //                        }
            //                    }
            //                    #endregion
            //                }
            //                else//此处就是尿量
            //                {
            //                    g.DrawString(value, fontNormal, brushBlack, new RectangleF(startPointX, startPointY + 1, width, m_LineHeight1 + 2), centerFormat);//+5调整栏位里面数值的位置
            //                }
            //            }
            //            currentLinePointY += m_LineHeight1 + 1;
            //        }


            //        #region 注释掉的血压
            //        //if (vso.Name == VitalSignsType.XueYa.ToString()) //血压
            //        //{
            //        //    #region 血压
            //        //    int timePointOfDay = vso.TimePointOfDay;//每天记录血压的时间点的数目
            //        //    int xuYaCellWidth = (m_LineHeight2 + 1) * m_DayTimePoint / timePointOfDay;//血压每一小格的宽度

            //        //    //绘制血压的横线
            //        //    g.DrawLine(penBlack, currentLinePointX, currentLinePointY + m_LineHeight1 + 1, currentLinePointX + tableWidth, currentLinePointY + m_LineHeight1 + 1);

            //        //    //绘制“血压”
            //        //    string unit = vso.Unit.Trim().Length > 0 ? "(" + vso.Unit + ")" : "";
            //        //    g.DrawString(GetVitalSignsTypeName(vso.Name) + unit, fontNormal, brushBlack, new RectangleF(currentLinePointX + m_RowCaptionIndent, currentLinePointY + 2, m_FirstColumnWidth, m_LineHeight1), leftFormat);

            //        //    //绘制血压每天隔开的竖线
            //        //    for (int m = 0; m < m_Days; m++)
            //        //    {
            //        //        for (int j = 1; j < timePointOfDay; j++)
            //        //        {
            //        //            int startPointX = currentLinePointX + m_FirstColumnWidth + 1 + (m_LineHeight2 + 1) * m_DayTimePoint * m + xuYaCellWidth * j;
            //        //            int startPointY = currentLinePointY;
            //        //            int endPointX = startPointX;
            //        //            int endPointY = currentLinePointY + m_LineHeight1;

            //        //            g.DrawLine(penBlack, startPointX, startPointY, endPointX, endPointY);
            //        //        }
            //        //    }

            //        //    //绘制血压的值
            //        //    for (int m = 0; m < PatientInfo.DateTableXueYa.Rows.Count; m++)
            //        //    {
            //        //        DateTime dateTime = Convert.ToDateTime(PatientInfo.DateTableXueYa.Rows[m]["DateTime"]);
            //        //        string timePoint = PatientInfo.DateTableXueYa.Rows[m]["TimePoint"].ToString();
            //        //        string value = PatientInfo.DateTableXueYa.Rows[m]["Value"].ToString();

            //        //        float startPointX = computeLocationBottomX(dateTime, timePoint) - 1;
            //        //        float startPointY = currentLinePointY;

            //        //        float width = (m_DayTimePoint * (m_LineHeight2 + 1)) / m_DayTimePointXuYa + 2;
            //        //        g.DrawString(value, fontSmall, brushBlack, new RectangleF(startPointX, startPointY, width, m_LineHeight1), centerFormat);
            //        //    }

            //        //    currentLinePointY += m_LineHeight1 + 1;
            //        //    #endregion
            //        //}
            //        #endregion
            //        else
            //        {
            //            #region 绘制液体入量
            //            DataTable data = new DataTable();

            //            if (vso.Name == VitalSignsType.ZongRuLiang.ToString())    //液体入量
            //            {
            //                data = PatientInfo.DataTableZongRuLiang;
            //                g.DrawString(GetVitalSignsTypeName(vso.Name), fontNormal, brushBlack,
            //                 new RectangleF(currentLinePointX + m_RowCaptionIndent, currentLinePointY + 24, m_FirstColumnWidth, m_LineHeight1), leftFormat);//ywk Y+6 (调整栏位名称的文字)

            //                for (int m = 0; m < data.Rows.Count; m++)
            //                {
            //                    DateTime dateTime = Convert.ToDateTime(data.Rows[m]["DateTime"]);
            //                    string timePoint = data.Rows[m]["TimePoint"].ToString();
            //                    string value = data.Rows[m]["Value"].ToString();
            //                    float startPointX = computeLocationBottomX(dateTime, timePoint);
            //                    float startPointY = currentLinePointY + 22;
            //                    float width = m_DayTimePoint * (m_LineHeight2 + 1);
            //                    g.DrawString(value, fontNormal, brushBlack, new RectangleF(startPointX, startPointY + 1, width, m_LineHeight1 + 2), centerFormat);//+5调整栏位里面数值的位置
            //                }
            //                //currentLinePointY += m_LineHeight1 + 1;
            //            }
            //            #endregion

            //            #region 绘制血压
            //            else if (vso.Name == VitalSignsType.XueYa.ToString()) //血压
            //            {
            //                #region 血压
            //                int timePointOfDay = vso.TimePointOfDay;//每天记录血压的时间点的数目
            //                int xuYaCellWidth = (m_LineHeight2 + 1) * m_DayTimePoint / timePointOfDay;//血压每一小格的宽度

            //                //绘制血压的横线
            //                g.DrawLine(penBlack, currentLinePointX, currentLinePointY + m_LineHeight1 * 5 + 7, currentLinePointX + tableWidth, currentLinePointY + m_LineHeight1 * 5 + 7);

            //                //绘制“血压”
            //                string unit = vso.Unit.Trim().Length > 0 ? "(" + vso.Unit + ")" : "";
            //                g.DrawString(GetVitalSignsTypeName(vso.Name) + unit, fontNormal, brushBlack, new RectangleF(currentLinePointX + m_RowCaptionIndent, currentLinePointY + 87, m_FirstColumnWidth, m_LineHeight1), leftFormat);

            //                //绘制血压每天隔开的竖线
            //                for (int m = 0; m < m_Days; m++)
            //                {
            //                    for (int j = 1; j < timePointOfDay; j++)
            //                    {
            //                        int startPointX = currentLinePointX + m_FirstColumnWidth + 1 + (m_LineHeight2 + 1) * m_DayTimePoint * m + xuYaCellWidth * j;
            //                        int startPointY = currentLinePointY + 86;
            //                        int endPointX = startPointX;
            //                        int endPointY = currentLinePointY + 86 + m_LineHeight1;

            //                        g.DrawLine(penBlack, startPointX, startPointY, endPointX, endPointY);
            //                    }
            //                }

            //                //绘制血压的值
            //                for (int m = 0; m < PatientInfo.DateTableXueYa.Rows.Count; m++)
            //                {
            //                    DateTime dateTime = Convert.ToDateTime(PatientInfo.DateTableXueYa.Rows[m]["DateTime"]);
            //                    string timePoint = PatientInfo.DateTableXueYa.Rows[m]["TimePoint"].ToString();
            //                    string value = PatientInfo.DateTableXueYa.Rows[m]["Value"].ToString();

            //                    float startPointX = computeLocationBottomX(dateTime, timePoint);
            //                    float startPointY = currentLinePointY + 86;

            //                    float width = (m_DayTimePoint * (m_LineHeight2 + 1)) / m_DayTimePointXuYa + 2;
            //                    g.DrawString(value, fontSmall, brushBlack, new RectangleF(startPointX, startPointY, width, m_LineHeight1), centerFormat);
            //                }
            //                //currentLinePointY += m_LineHeight1 + 1;
            //                #endregion
            //            }
            //            #endregion

            //            #region 绘制  体重，过敏药物，其他1，其他2
            //            if (vso.Name == VitalSignsType.TiZhong.ToString())//体重
            //            {
            //                data = PatientInfo.DataTableTiZhong;
            //                g.DrawString(GetVitalSignsTypeName(vso.Name), fontNormal, brushBlack, new RectangleF(currentLinePointX + m_RowCaptionIndent, currentLinePointY + 67, m_FirstColumnWidth, m_LineHeight1), leftFormat);
            //                for (int m = 0; m < data.Rows.Count; m++)
            //                {
            //                    DateTime dateTime = Convert.ToDateTime(data.Rows[m]["DateTime"]);
            //                    string timePoint = data.Rows[m]["TimePoint"].ToString();
            //                    string value = data.Rows[m]["Value"].ToString();
            //                    float startPointX = computeLocationBottomX(dateTime, timePoint);
            //                    float startPointY = currentLinePointY + 67;
            //                    float width = m_DayTimePoint * (m_LineHeight2 + 1);
            //                    g.DrawString(value, fontNormal, brushBlack, new RectangleF(startPointX, startPointY + 1, width, m_LineHeight1 + 2), centerFormat);//+5调整栏位里面数值的位置
            //                }
            //                g.DrawLine(penBlack, currentLinePointX, currentLinePointY + 67 + (m_LineHeight1), currentLinePointX + tableWidth, currentLinePointY + 67 + (m_LineHeight1));
            //            }
            //            if (vso.Name == VitalSignsType.GuoMingYaoWu.ToString())//药物过敏
            //            {
            //                //float width = m_DayTimePoint * (m_LineHeight2 + 1);//+1

            //                data = PatientInfo.DataTableGuoMinYaoWu;
            //                g.DrawString(GetVitalSignsTypeName(vso.Name), fontNormal, brushBlack, new RectangleF(currentLinePointX + m_RowCaptionIndent, currentLinePointY + 89, m_FirstColumnWidth, m_LineHeight1), leftFormat);


            //                for (int m = 0; m < data.Rows.Count; m++)
            //                {
            //                    DateTime dateTime = Convert.ToDateTime(data.Rows[m]["DateTime"]);
            //                    string timePoint = data.Rows[m]["TimePoint"].ToString();
            //                    string value = data.Rows[m]["Value"].ToString();
            //                    float startPointX = computeLocationBottomX(dateTime, timePoint);
            //                    float startPointY = currentLinePointY + 88;
            //                    //float width = m_DayTimePoint * (m_LineHeight2 + 1);
            //                    //g.DrawString(value, fontNormal, brushBlack, new RectangleF(startPointX, startPointY + 1, width , m_LineHeight1 + 2), centerFormat);//+5调整栏位里面数值的位置
            //                    //对于超出单元格的内容处理
            //                    int valueWidth = TextRenderer.MeasureText(value, fontNormal).Width;
            //                    if (valueWidth > 77)
            //                    {
            //                        //int eventTimePointSerialNumber = 0;//温度测量的时间点在一天中的序号
            //                        //for (int j = 0; j < m_DataTableDayTimePoint.Rows.Count; j++)
            //                        //{
            //                        //    if (m_DataTableDayTimePoint.Rows[j][0].ToString() == timePoint)
            //                        //    {
            //                        //        eventTimePointSerialNumber = j;
            //                        //        break;
            //                        //    }
            //                        //}
            //                        //int daySpan = 0;//温度测量时间距离体温单上第一列中日期的间隔天数
            //                        //DateTime firstDateTime = Convert.ToDateTime(m_DateTimeEveryColumnDateTime.Rows[0][0]);
            //                        //daySpan = (dateTime - firstDateTime).Days;

            //                        float width = m_DayTimePoint * (m_LineHeight2 + 1);
            //                        //float xPoint1 = /*m_TableStartPointX + */m_FirstColumnWidth  + (m_LineHeight2 +1) * m_DayTimePoint * daySpan + (m_LineHeight2 ) * eventTimePointSerialNumber;
            //                        g.DrawString(value, fontNormal, brushBlack, new RectangleF(startPointX, startPointY + 1, width * m_Days, m_LineHeight1 + 2), leftFormat);//+5调整栏位里面数值的位置
            //                    }
            //                    else
            //                    {
            //                        float width = m_DayTimePoint * (m_LineHeight2 + 1);
            //                        g.DrawString(value, fontNormal, brushBlack, new RectangleF(startPointX, startPointY + 1, width, m_LineHeight1 + 2), centerFormat);//+5调整栏位里面数值的位置
            //                    }
            //                }
            //            }
            //            //剩余的画最后5个横线
            //            for (int l = 1; l < 3; l++)
            //            {
            //                int pointY = currentLinePointY + 131 + (m_LineHeight1) * l;//109
            //                g.DrawLine(penBlack, currentLinePointX, pointY, currentLinePointX + tableWidth, pointY);
            //            }
            //            #region 注释掉的
            //            //g.DrawString(GetVitalSignsTypeName(vso.Name), fontNormal, brushBlack, new RectangleF(currentLinePointX + 84, currentLinePointY + 2, m_FirstColumnWidth, m_LineHeight1), leftFormat);
            //            //DataTable data = new DataTable();

            //            //if (vso.Name == VitalSignsType.ZongRuLiang.ToString())    //总入量
            //            //{
            //            //    data = PatientInfo.DataTableZongRuLiang;
            //            //}
            //            //else if (vso.Name == VitalSignsType.ZongChuLiang.ToString())//总出量
            //            //{
            //            //    data = PatientInfo.DataTableZongChuLiang;
            //            //}

            //            //else if (vso.Name == VitalSignsType.DaBianCiShu.ToString())//大便次数
            //            //{
            //            //    data = PatientInfo.DataTableDaBianCiShu;
            //            //}


            //            //else if (vso.Name == VitalSignsType.Other2.ToString())//其他2 改为了物理升温，此处注释掉
            //            //{
            //            //    data = PatientInfo.DataTableOther2;
            //            //}


            //            //int timePointOfDay = vso.TimePointOfDay;//每天记录总入量的时间点的数目
            //            //int xuYaCellWidth = (m_LineHeight2 + 1) * m_DayTimePoint / timePointOfDay;//总入量中每一小格的宽度

            //            ////绘制横线
            //            //g.DrawLine(penBlack, currentLinePointX, currentLinePointY + m_LineHeight1 + 5,
            //            //    currentLinePointX + tableWidth,
            //            //    currentLinePointY + m_LineHeight1 + 5);//5  血压的横线绘制s

            //            ////绘制栏位名称(疼痛不放在最下面，新需求改为放在血压下面)
            //            //if (vso.Name != VitalSignsType.PainInfo.ToString() && vso.Name != VitalSignsType.Other2.ToString() && vso.Name != VitalSignsType.XueYa.ToString())//其他
            //            //{
            //            //    string unit = vso.Unit.Trim().Length > 0 ? "(" + vso.Unit + ")" : "";
            //            //    g.DrawString(GetVitalSignsTypeName(vso.Name) + unit, fontNormal, brushBlack,
            //            //        new RectangleF(currentLinePointX + m_RowCaptionIndent, currentLinePointY + 5, m_FirstColumnWidth, m_LineHeight1), leftFormat);//ywk Y+6 (调整栏位名称的文字)

            //            //    //绘制数值
            //            //    for (int m = 0; m < data.Rows.Count; m++)
            //            //    {
            //            //        DateTime dateTime = Convert.ToDateTime(data.Rows[m]["DateTime"]);
            //            //        string timePoint = data.Rows[m]["TimePoint"].ToString();
            //            //        string value = data.Rows[m]["Value"].ToString();


            //            //        float startPointX = computeLocationBottomX(dateTime, timePoint);
            //            //        float startPointY = currentLinePointY;

            //            //        float width = m_DayTimePoint * (m_LineHeight2 + 1);//+1

            //            //        if (vso.Name == VitalSignsType.DaBianCiShu.ToString())//大便次数
            //            //        {
            //            //            #region 绘制“大便次数”(先注释掉)
            //            //            //if (value.Split(':').Length == 3)
            //            //            //{
            //            //            //    string[] valueDaBianXiShuX = value.Split(':');
            //            //            //    string valueTemp = valueDaBianXiShuX[0] + valueDaBianXiShuX[1] + "/" + valueDaBianXiShuX[2];
            //            //            //    /*
            //            //            //     * 由于大便次数比较特殊，所以这里要进行特殊处理
            //            //            //     * 特殊情况：患者无大便，以“0”表示；灌肠后大便以“E”表示，分子记录大便次数
            //            //            //     * 例：1/E表示灌肠后大便1次；0/E表示灌肠后无排便；11/E表示自行排便1次灌肠后又排便1次；
            //            //            //     * “※”表示大便失禁，“☆”表示人工肛门
            //            //            //     */
            //            //            //    if (value.Trim().StartsWith("※") || value.Trim().StartsWith("☆"))
            //            //            //    {
            //            //            //        g.DrawString(valueDaBianXiShuX[0].ToString(), fontNormal, brushBlack, new RectangleF(startPointX, startPointY + 2, width, m_LineHeight1), centerFormat);//+2
            //            //            //    }
            //            //            //    else if (value.Trim() != "::")
            //            //            //    {
            //            //            //        float startPaintDaBianXiShuX = startPointX + (width - TextRenderer.MeasureText(valueTemp, fontNormal).Width) / 2;
            //            //            //        int increaseWidth;

            //            //            //        if (valueDaBianXiShuX.Length == 3)
            //            //            //        {
            //            //            //            if (string.IsNullOrEmpty(valueDaBianXiShuX[1].ToString()) && string.IsNullOrEmpty(valueDaBianXiShuX[2].ToString()))
            //            //            //            {

            //            //            //                increaseWidth = TextRenderer.MeasureText(valueDaBianXiShuX[0], fontNormal).Width - 4;
            //            //            //                g.DrawString(valueDaBianXiShuX[0], fontNormal, brushBlack, startPaintDaBianXiShuX + increaseWidth, startPointY + 5);//+6
            //            //            //            }
            //            //            //            else
            //            //            //            {
            //            //            //                g.DrawString(valueDaBianXiShuX[0], fontNormal, brushBlack, startPaintDaBianXiShuX, startPointY + 6);
            //            //            //                increaseWidth = TextRenderer.MeasureText(valueDaBianXiShuX[0], fontNormal).Width - 4;

            //            //            //                g.DrawString(valueDaBianXiShuX[2], fontSmall, brushBlack, startPaintDaBianXiShuX + increaseWidth, startPointY + 5);
            //            //            //                increaseWidth += TextRenderer.MeasureText(valueDaBianXiShuX[2], fontSmall).Width - 5;

            //            //            //                g.DrawString("/", fontNormal, brushBlack, startPaintDaBianXiShuX + increaseWidth + 2, startPointY + 6);//--调整/的位置
            //            //            //                increaseWidth += TextRenderer.MeasureText("/", fontNormal).Width - 4;

            //            //            //                g.DrawString(valueDaBianXiShuX[1], fontNormal, brushBlack, startPaintDaBianXiShuX + increaseWidth, startPointY + 6);
            //            //            //            }
            //            //            //        }
            //            //            //    }
            //            //            //    else
            //            //            //    {
            //            //            //        //当为“：：”时，什么都不显示
            //            //            //    }
            //            //            //}
            //            //            #endregion
            //            //        }
            //            //        else if (vso.Name == VitalSignsType.GuoMingYaoWu.ToString())//过敏药物
            //            //        {
            //            //            g.DrawString(value, fontNormal, brushBlack, new RectangleF(startPointX, startPointY + 5, width * m_Days, m_LineHeight1 + 2), leftFormat);//+2调整栏位里面数值的位置
            //            //        }
            //            //        else
            //            //        {
            //            //            g.DrawString(value, fontNormal, brushBlack, new RectangleF(startPointX, startPointY + 5, width, m_LineHeight1 + 2), centerFormat);//+2调整栏位里面数值的位置
            //            //        }
            //            //    }
            //            //}
            //            //currentLinePointY += m_LineHeight1 + 2;//2
            //            #endregion
            //            #endregion
            //        }
            //    }

            //}
            ////绘制表格的最后一条粗线
            ////g.DrawLine(penBlackBold, currentLinePointX, currentLinePointY + 4, currentLinePointX + tableWidth, currentLinePointY + 4);//Y+3  +4调整表格底部的横线
            //g.DrawLine(penBlackBold, currentLinePointX, currentLinePointY + 172, currentLinePointX + tableWidth, currentLinePointY + 172);//多加一行，用于排出量的补录 （调整了表格高度）(最后的横线)172
            #endregion
        }

        #region 绘制曲线图下面区域中的呼吸这一行
        /*
        /// <summary>
        /// 绘制曲线图下面区域中的呼吸一行
        /// </summary>
        /// <param name="g"></param>
        /// <param name="penBlack"></param>
        /// <param name="brushBlack"></param>
        /// <param name="brushRed"></param>
        /// <param name="fontNormal"></param>
        /// <param name="fontSmall"></param>
        /// <param name="currentLinePointX"></param>
        /// <param name="currentLinePointY"></param>
        /// <param name="tableWidth"></param>
        /// <param name="centerFormat"></param>
        private void PaintHuXiAtBottom(Graphics g, Pen penBlack, Brush brushBlack, Brush brushRed, Font fontNormal, Font fontSmall,
            int currentLinePointX, int currentLinePointY, int tableWidth, StringFormat centerFormat)
        {

            #region 绘制横线 竖线 左边的文字

            //在单独的一行中显示呼吸的数据
            g.DrawLine(penBlack, currentLinePointX, currentLinePointY + m_LineHeight2 * 2, currentLinePointX + tableWidth, currentLinePointY + m_LineHeight2 * 2);

            //绘制呼吸行中各个时间段的竖线
            for (int i = 1; i < m_DayTimePoint * m_Days; i++)
            {
                if (i % m_DayTimePoint != 0)
                {
                    int startPointX = currentLinePointX + m_FirstColumnWidth + 1 + (m_LineHeight2 + 1) * i;
                    int startPointY = currentLinePointY;
                    int endPointX = startPointX;
                    int endPointY = startPointY + m_LineHeight2 * 2;

                    g.DrawLine(penBlack, new Point(startPointX, startPointY), new Point(endPointX, endPointY));
                }
            }

            //绘制呼吸名称以及单位
            g.DrawString("呼吸(次/分)", fontNormal, brushBlack, new RectangleF(currentLinePointX, currentLinePointY + 1, m_FirstColumnWidth, m_LineHeight2 * 2), centerFormat);

            #endregion

            #region 先计算绘制的位置，然后再将其绘制出来
            //将呼吸的值绘制到相应的位置上
            DateTime currentDateTime = DateTime.MinValue;//用于记录当前循环的值

            ArrayList list = new ArrayList();
            for (int i = 0; i < PatientInfo.DataTableHuXi.Rows.Count; i++)
            {
                currentDateTime = Convert.ToDateTime(PatientInfo.DataTableHuXi.Rows[i][0]);

                #region 计算一天记录呼吸的次数
                int sameDateCount = 1;
                for (int j = i + 1; j < PatientInfo.DataTableHuXi.Rows.Count; j++)
                {
                    DateTime dt = Convert.ToDateTime(PatientInfo.DataTableHuXi.Rows[j][0]);
                    if (currentDateTime == dt)
                    {
                        sameDateCount++;
                    }
                }
                #endregion

                //每日记录呼吸2次以上，应当在相应的栏目内上下交错记录，第1次呼吸应当记录在上方
                int m = 0;
                for (; m < sameDateCount; m++)
                {
                    currentDateTime = Convert.ToDateTime(PatientInfo.DataTableHuXi.Rows[i + m][0]);
                    string timePoint = PatientInfo.DataTableHuXi.Rows[i + m][1].ToString().Trim();
                    string value = PatientInfo.DataTableHuXi.Rows[i + m][2].ToString().Trim();
                    string memo = PatientInfo.DataTableHuXi.Rows[i + m][3].ToString().Trim();
                    string linkNext = PatientInfo.DataTableHuXi.Rows[i + m][4].ToString().Trim();
                    string IsSpecial = PatientInfo.DataTableHuXi.Rows[i + m][5].ToString().Trim();

                    float locationPointX = computeLocationBottomX(currentDateTime, timePoint);//呼吸数据在X轴方向上的坐标

                    if (sameDateCount == 1)//一天记录呼吸一次，则在栏目中居中显示
                    {
                        g.DrawString(value, fontSmall, brushRed, new RectangleF(locationPointX - 1, currentLinePointY, m_LineHeight2 + 2, m_LineHeight2 * 2), centerFormat);//居中显示
                    }
                    else
                    {
                        RectangleF rectForHuXiJi = new RectangleF(locationPointX + 1, currentLinePointY + 1, m_LineHeight2 - 1, m_LineHeight2 - 1);
                        if (IsSpecial == "Y")//绘制呼吸机
                        {
                            g.DrawImage(m_Picture.BitmapHuXiSpecial, rectForHuXiJi);//顶格显示呼吸机
                        }
                        else
                        {
                            if (m % 2 == 0)//每天的第1次呼吸应当记录在上方，其他的依次交错记录
                            {
                                g.DrawString(value, fontSmall, brushRed, locationPointX, currentLinePointY + m_LineHeight2 - TextRenderer.MeasureText(value, fontSmall).Height);//显示在上方
                            }
                            else
                            {
                                g.DrawString(value, fontSmall, brushRed, locationPointX, currentLinePointY + m_LineHeight2 * 2 - TextRenderer.MeasureText(value, fontSmall).Height);//显示在下方
                            }
                        }
                    }
                }
                i += m - 1;
            }
            #endregion
        }
        */
        #endregion

        #region  计算曲线图下面栏位内容的的横坐标，包括（血压，入量，出量，身高，体重等，可能包括呼吸, 入院日期, 手术后天数）

        /// <summary>
        /// 计算曲线图下面栏位内容的的横坐标，包括（血压，入量，出量等，可能包括呼吸）
        /// </summary>
        /// <param name="currentLinePointY"></param>
        /// <param name="currentDateTime"></param>
        /// <param name="testTimePoint"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private float computeLocationBottomX(DateTime currentDateTime/*测量的日期*/, string testTimePoint/*测量的时间点*/)
        {
            #region 计算出 需要绘制的事件 的 日期 和 时间点

            int daySpan = 0;//温度测量时间距离体温单上第一列中日期的间隔天数
            if (m_DateTimeEveryColumnDateTime.Rows.Count > 0)
            {
                DateTime firstDateTime = Convert.ToDateTime(m_DateTimeEveryColumnDateTime.Rows[0][0]);
                daySpan = (currentDateTime - firstDateTime).Days;

                if (daySpan < 0 || daySpan >= m_Days) //已经超出了这张体温单的日期范围，所以要排除
                {
                    return -100;
                }
            }

            int eventTimePointSerialNumber = 0;//温度测量的时间点在一天中的序号
            for (int j = 0; j < m_DataTableDayTimePoint.Rows.Count; j++)
            {
                if (m_DataTableDayTimePoint.Rows[j][0].ToString() == testTimePoint)
                {
                    eventTimePointSerialNumber = j;
                    break;
                }
            }
            #endregion

            float xPoint = m_TableStartPointX + m_FirstColumnWidth + 1 + (m_LineHeight2 + 1) * m_DayTimePoint * daySpan + (m_LineHeight2 + 1) * eventTimePointSerialNumber;

            return xPoint;
        }
        #endregion


        #endregion

        #region 绘制提示部分

        /// <summary>
        /// 绘制最右边的提示部分
        /// </summary>
        /// <param name="pictureBox"></param>
        /// <param name="g"></param>
        private void PaintImagePrompt(PictureBox pictureBox, Graphics g)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;

            int xPoint = m_TableStartPointX + m_FirstColumnWidth + 1 + m_DayTimePoint * (m_LineHeight2 + 1) * m_Days + 14;
            int yPoint = 350;

            VitalSigns vs = new VitalSigns();
            vs.PaintImagePrompt(g, xPoint, yPoint, this.Font);
            PaintPageCount(pictureBox, g);
        }

        /// <summary>
        /// 绘制底部的提示部分，在页数的上面 表格的下面
        /// </summary>
        /// <param name="pictureBox"></param>
        /// <param name="g"></param>
        private void PaintImagePromptBottom(PictureBox pictureBox, Graphics g)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;

            int xPoint = m_TableStartPointX;
            int yPoint = Convert.ToInt32(m_TableStartPointY + m_TableHeight + 3);//+30 ywk  +6

            VitalSigns vs = new VitalSigns();
            Font m_font = new Font(this.Font.FontFamily, this.Font.Size * 0.959f, FontStyle.Regular);
            vs.PaintImagePrompt2(g, xPoint, yPoint, m_font);//this.Font修改图标大小 edit by ywk 
            //PaintPageCount(pictureBox, g);//绘制底部的页数去掉 yw 2012年5月18日 11:53:41
            //绘制第几周 add by  ywk 2012年5月17日 15:21:00
            PaintWeekCount(pictureBox, g);
        }
        /// <summary>
        /// 新增的在三测单右上角绘制“第几周”
        /// add by  ywk 2012年5月17日 15:21:31
        /// </summary>
        /// <param name="pictureBox"></param>
        /// <param name="g"></param>
        private void PaintWeekCount(PictureBox pictureBox, Graphics g)
        {
            string currentPageCount = Convert.ToString((m_DataTimeAllocate - Convert.ToDateTime(PatientInfo.InTime.Split(' ')[0])).Days / m_Days + 1);

            int width = m_FirstColumnWidth + 1 + m_DayTimePoint * (m_LineHeight2 + 1) * m_Days + 14;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            Font font = new Font(this.Font.FontFamily, this.Font.Size + 1, FontStyle.Regular);
            g.DrawString("第  " + currentPageCount + "  周", font, Brushes.Black, pictureBox.Width - TextRenderer.MeasureText(m_HeaderName, font).Width - 110, m_HeaderNameY + 5);
        }

        #endregion

        #region 绘制页数

        /// <summary>
        /// 绘制页数
        /// </summary>
        /// <param name="pictureBox"></param>
        /// <param name="g"></param>
        private void PaintPageCount(PictureBox pictureBox, Graphics g)
        {
            string currentPageCount = Convert.ToString((m_DataTimeAllocate - Convert.ToDateTime(PatientInfo.InTime.Split(' ')[0])).Days / m_Days + 1);

            int width = m_FirstColumnWidth + 1 + m_DayTimePoint * (m_LineHeight2 + 1) * m_Days + 14;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            Font font = new Font(this.Font.FontFamily, this.Font.Size + 1, FontStyle.Regular);
            g.DrawString("第  " + currentPageCount + "  页", font, Brushes.Black,
                new RectangleF(m_TableStartPointX, m_TableStartPointY + m_TableHeight + 25, width, 20), sf);//+30 ywk +35
        }
        #endregion

        #endregion

        #region 方法(private)

        private void ClearVariable()
        {

        }

        /// <summary>
        /// 判断曲线图中是否显示呼吸，如果不显示呼吸，那么呼吸在曲线格子区域下面以一行的形式显示
        /// </summary>
        /// <returns></returns>
        private bool checkIsContainHuXiInCurve()
        {
            for (int i = 0; i < m_ArrayListVitalSigns.Count; i++)
            {
                VitalSigns vs = m_ArrayListVitalSigns[i] as VitalSigns;

                if (vs != null)
                {
                    if (vs.Name == VitalSignsType.HuXi.ToString())//呼吸
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 得到体温中一小格代表的值
        /// </summary>
        /// <returns></returns>
        private float GetCellValue(string type)
        {
            float cellValue = 0;
            //得到温度设定的最大值
            for (int j = 0; j < m_ArrayListVitalSigns.Count; j++)
            {
                VitalSigns vs = (VitalSigns)m_ArrayListVitalSigns[j];
                if (vs.Name == type)
                {
                    cellValue = vs.CellValue;
                }
                else if (vs.Name == type)
                {
                    cellValue = vs.CellValue;
                }
                else if (vs.Name == type)
                {
                    cellValue = vs.CellValue;
                }
            }
            return cellValue;
        }

        /// <summary>
        /// 得到体温的最大值
        /// </summary>
        /// <returns></returns>
        private float GetMaxValue(string type)
        {
            float maxValue = 0;
            //得到温度设定的最大值
            for (int j = 0; j < m_ArrayListVitalSigns.Count; j++)
            {
                VitalSigns vs = (VitalSigns)m_ArrayListVitalSigns[j];
                if (vs.Name == type)
                {
                    maxValue = vs.MaxValue;
                }
                else if (vs.Name == type)
                {
                    maxValue = vs.MaxValue;
                }
                else if (vs.Name == type)
                {
                    maxValue = vs.MaxValue;
                }
            }
            return maxValue;
        }

        /// <summary>
        /// 根据颜色得到画刷
        /// </summary>
        /// <param name="colorName"></param>
        /// <returns></returns>
        private Brush GetBrushByColorName(string colorName)
        {
            Brush brush;
            if (colorName.ToLower() == "red")
            {
                brush = Brushes.Red;
            }
            else if (colorName.ToLower() == "blue")
            {
                brush = Brushes.Blue;
            }
            else
            {
                brush = Brushes.Black;
            }
            return brush;
        }

        /// <summary>
        /// 判断数据源中是否有这列
        /// </summary>
        /// <param name="dataTableTableBaseLine"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        private int CheckIsNeed(DataTable dataTableTableBaseLine, string columnName)
        {
            for (int i = 0; i < dataTableTableBaseLine.Rows.Count; i++)
            {
                if (dataTableTableBaseLine.Rows[i][0].ToString() == columnName)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 确定体温单中每列的日期(即根据入院时间和指定时间计算出体温表中每列显示的日期)
        /// </summary>
        /// <param name="inDate">入院日期</param>
        /// <param name="allocateDate">指定的日期</param>
        /// <returns></returns>
        private DataTable GetDateTimeForColumns(DateTime inDate, DateTime allocateDate)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("BeforeConvertDate"); //转换前的值
            dt.Columns.Add("ConvertedDate"); //转换后的值

            string str1 = PatientInfo.InTime.Split(' ')[0];//入院
            inDate = Convert.ToDateTime(str1);

            allocateDate = Convert.ToDateTime(allocateDate.ToString("yyyy-MM-dd"));

            DateTime dateTimeBeginTime = new DateTime(inDate.Year, inDate.Month, inDate.Day);
            int weeks = (allocateDate - inDate).Days / m_Days;
            dateTimeBeginTime = dateTimeBeginTime.AddDays(m_Days * weeks);

            for (int i = 0; i < m_Days; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = dateTimeBeginTime;
                dr[1] = "";
                dateTimeBeginTime = dateTimeBeginTime.AddDays(1);
                dt.Rows.Add(dr);
            }

            if (Convert.ToDateTime(dt.Rows[0][0]).ToString("yyyy-MM-dd") == inDate.ToString("yyyy-MM-dd") //住院日期首页第1日 需填写年-月-日（如：2010－03－26）
                || Convert.ToDateTime(dt.Rows[0][0]).ToString("MM-dd") == "01-01") //跨年度第1日需填写年-月-日（如：2010－03－26）
            {
                dt.Rows[0][1] = Convert.ToDateTime(dt.Rows[0][0]).ToString("yyyy-MM-dd");
            }
            else //其余页的体温单的第1日 填写月-日（如03-26）
            {
                dt.Rows[0][1] = Convert.ToDateTime(dt.Rows[0][0]).ToString("MM-dd");
            }

            for (int i = dt.Rows.Count - 1; i > 0; i--)
            {
                DateTime dateTime1 = Convert.ToDateTime(dt.Rows[i][0]);
                DateTime dateTime2 = Convert.ToDateTime(dt.Rows[i - 1][0]);

                if (dateTime1.Year > dateTime2.Year)
                {
                    dt.Rows[i][1] = dateTime1.ToString("yyyy-MM-dd");
                }
                else if (dateTime1.Month > dateTime2.Month)
                {
                    dt.Rows[i][1] = dateTime1.ToString("MM-dd");
                }
                else if (dateTime1.Day > dateTime2.Day)
                {
                    dt.Rows[i][1] = dateTime1.ToString("dd");
                }
            }

            return dt;
        }

        /// <summary>
        /// 通过生命体征的类别得到具体的名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetVitalSignsTypeName(string type)
        {
            string returnValue = string.Empty;

            if (type == "TiWen")
            {
                returnValue = "体温";
            }
            else if (type == "MaiBo")
            {
                returnValue = "脉搏";
            }
            else if (type == "HuXi")
            {
                returnValue = "呼吸";
            }
            else if (type == "LinLv")
            {
                returnValue = "心率";
            }
            else if (type == "WuLiJiangWen")
            {
                returnValue = "物理降温";
            }
            else if (type == "XueYa")
            {
                returnValue = "血 压";
            }
            else if (type == "ZongRuLiang")
            {
                returnValue = "入量";//总入量改为液体入量
            }
            else if (type == "ZongChuLiang")
            {
                returnValue = "尿量";// 作为尿量
            }
            else if (type == "YinLiuLiang")
            {
                returnValue = "引流量";
            }
            else if (type == "DaBianCiShu")
            {
                returnValue = "大便次数";
            }
            else if (type == "ShenGao")
            {
                returnValue = "身高";
            }
            else if (type == "TiZhong")
            {
                returnValue = "体 重";
            }
            else if (type == "GuoMingYaoWu")
            {
                returnValue = "药 物 过 敏";
            }
            else if (type == "TeShuZhiLiao")
            {
                returnValue = "特殊治疗";
            }
            else if (type == "Other1")
            {
                returnValue = "其他1";
            }
            else if (type == "Other2")
            {
                returnValue = "出量";
           }

            else if (type == "PainInfo")
            {
                returnValue = "疼痛指数";
            }

            return returnValue;

            /*
            TiWen = 1,          //体温
            MaiBo = 2,          //脉搏
            HuXi = 3,           //呼吸
            XinLv = 4,          //心率
            WuLiJiangWen = 5,   //物理降温
            XueYa = 6,          //血压
            ZongRuLiang = 7,    //总入量
            ZongChuLiang = 8,   //总出量
            YinLiuLiang = 9,    //引流量
            DaBianCiShu = 10,    //大便次数
            ShenGao = 11,       //身高
            TiZhong = 12,       //体重
            GuoMingYaoWu = 13,  //过敏药物
            TeShuZhiLiao = 14,  //特殊治疗
            Other1 = 15,        //其他1
            Other2 = 16         //其他2
            */
        }

        /// <summary>
        /// 通过该病人入院日期和指定日期得到病人在指定日期时的病床信息
        /// </summary>
        private void GetBedIDAndDeptName(DateTime inTime, DateTime allocateDateTime, string noOfInpat)
        {
            //由于病人入院后的科室病房或病区或科室可能发生变化，所以这里要等到算出某张体温表中第一天的日期后
            //通过BedInfo得到当前病人在这个时间点的科室和病床号
            DataTable dataTableDate = GetDateTimeForColumns(inTime, allocateDateTime);
            DateTime firstDateOfTable = DateTime.MinValue; //某张体温表中第一天的日期
            if (dataTableDate.Rows.Count > 0)
            {
                firstDateOfTable = Convert.ToDateTime(dataTableDate.Rows[0][0]);
                DataTable dt = PublicSet.MethodSet.GetPatientBedInfoByDate(noOfInpat, firstDateOfTable);
                if (dt.Rows.Count > 0)
                {
                    PatientInfo.BedCode = dt.Rows[0]["FormerBedID"].ToString();
                    PatientInfo.Section = dt.Rows[0]["DeptName"].ToString();
                    //this.m_HospitalName = InitHospitalName(dt.Rows[0]["HospitalName"].ToString());
                }
            }
        }

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

        private void GetHospitalName()
        {
            if (this.DesignMode == false)
            {
                m_HospitalName = InitHospitalName(PublicSet.MethodSet.GetHospitalName());
            }
        }

        /// <summary>
        /// 判断时间是否需要显示时间
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        private bool CheckEventIsShowTime(string eventName)
        {
            foreach (DataRow dr in m_EventSetting)
            {
                if (dr["name"].ToString().Trim() == eventName.Trim())
                {
                    if (dr["isshowtime"].ToString() == "1")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 得到事件所在位置
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        private float GetEventLocation(string eventName)
        {
            foreach (DataRow dr in m_EventSetting)
            {
                if (dr["name"].ToString().Trim() == eventName.Trim())
                {
                    return float.Parse(dr["showlocation"].ToString());
                }
            }
            return 42;
        }
        #endregion

        #region 方法(public)

        /// <summary>
        /// 判断“上一周”是否显示的逻辑
        /// </summary>
        /// <returns></returns>
        public bool DateTimeLogicForLastWeek()
        {
            DataTable dataTableForColumns = GetDateTimeForColumns(Convert.ToDateTime(PatientInfo.InTime), m_DataTimeAllocate);
            if (dataTableForColumns.Rows.Count > 0)
            {
                DateTime dt1 = Convert.ToDateTime(dataTableForColumns.Rows[0][0]).AddDays(-m_Days);
                DateTime dt2 = Convert.ToDateTime(PatientInfo.InTime);

                if ((dt1 - dt2).Days >= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// 设置体温表的显示的时间，如果参数1为今天，则体温表中显示的日期范围包括今日
        /// </summary>
        /// <param name="dateTimeAllocate"></param>
        public void SetAllocateDateTime(DateTime dateTimeAllocate)
        {
            m_DataTimeAllocate = dateTimeAllocate;

            GetBedIDAndDeptName(Convert.ToDateTime(PatientInfo.InTime), m_DataTimeAllocate, PatientInfo.NoOfInpat); ;
        }

        public void SetAllocateDateTime()
        {
            m_DataTimeAllocate = Convert.ToDateTime(PatientInfo.InTime);

            GetBedIDAndDeptName(Convert.ToDateTime(PatientInfo.InTime), m_DataTimeAllocate, PatientInfo.NoOfInpat); ;
        }

        /// <summary>
        /// 设置体温表的显示的时间，如果参数1为今天，则体温表中显示的日期范围包括今日
        /// </summary>
        /// <param name="days"></param>
        public void SetAllocateDateTime(int days)
        {
            m_DataTimeAllocate = m_DataTimeAllocate.AddDays(days);

            GetBedIDAndDeptName(Convert.ToDateTime(PatientInfo.InTime), m_DataTimeAllocate, PatientInfo.NoOfInpat);
        }

        /// <summary>
        /// 对于存在转科转床病人的床位和科室的处理
        /// </summary>
        /// <param name="dataTablePatientInfo"></param>
        public void SetPatientInfo(DataTable dataTablePatientInfo)
        {
            if (dataTablePatientInfo.Rows.Count > 0)
            {
                PatientInfo patientInfo = new PatientInfo();
                patientInfo.InitPatientInfo(dataTablePatientInfo);

                GetBedIDAndDeptName(Convert.ToDateTime(PatientInfo.InTime), m_DataTimeAllocate, PatientInfo.NoOfInpat);
            }
        }

        /// <summary>
        /// 打印
        /// </summary>
        public void PrintDocument()
        {
            PrintForm printDocumentForm = new PrintForm();
            printDocumentForm.DefaultPageSize = m_DefaultPrintSize;

            //将三测单绘制到图片上
            Bitmap bmp = new Bitmap(this.pictureBoxMeasureTable.Width - 60
                , pictureBoxMeasureTable.Height
                , System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmp);

            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            string filePath = "C:\\" + Guid.NewGuid().ToString() + ".wmf";
            Metafile mf = new Metafile(filePath, g.GetHdc(), rect, MetafileFrameUnit.Pixel);

            g = Graphics.FromImage(mf);
            PaintNurseDocument(pictureBoxMeasureTable, g);
            g.Save();
            g.Dispose();

            try
            {
                printDocumentForm.InitPreviewControl(mf, bmp, this);
                printDocumentForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                mf.Dispose();
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// 获取打印的bitmap对象 xll 用于导出
        /// </summary>
        /// <returns></returns>
        public Bitmap GetbitMap()
        {
            try
            {
                //将三测单绘制到图片上
                Bitmap bmp = new Bitmap(this.pictureBoxMeasureTable.Width - 60
                    , pictureBoxMeasureTable.Height
                    , System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(bmp);
                g = Graphics.FromImage(bmp);
                PaintNurseDocument(pictureBoxMeasureTable, g);
                g.Save();
                g.Dispose();
                return bmp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 批量打印
        /// </summary>
        /// <param name="dtFrom"></param>
        /// <param name="dtTo"></param>
        public void PrintAllDocument()
        {
            DateTime patientInTime = Convert.ToDateTime(PatientInfo.InTime);
            PrintAllForm printAllDocumentForm = new PrintAllForm(patientInTime);
            printAllDocumentForm.DefaultPageSize = m_DefaultPrintSize;
            printAllDocumentForm.StartPosition = FormStartPosition.CenterScreen;
            DialogResult result = printAllDocumentForm.ShowDialog();
            if (result != DialogResult.OK) return;

            SetWaitDialogCaption("正在获取数据！");
            DateTime dtFrom = printAllDocumentForm.DateTimeFrom;
            DateTime dtTo = printAllDocumentForm.DateTimeTo;

            int fromDay = (dtFrom - patientInTime).Days;
            int toDay = (dtTo - patientInTime).Days;

            int fromWeek = Convert.ToInt32(Math.Floor(fromDay / 7.0f));
            int toWeek = Convert.ToInt32(Math.Floor(toDay / 7.0f));

            List<MetaFileInfo> list = new List<MetaFileInfo>();
            for (int indexWeek = fromWeek; indexWeek <= toWeek; indexWeek++)
            {
                SetWaitDialogCaption("正在获取" + patientInTime.AddDays(indexWeek * 7).ToString("yyyy-MM-dd") + "的数据！");

                SetAllocateDateTime(patientInTime.AddDays(indexWeek * 7));
                LoadData();
                this.Refresh();

                //将三测单绘制到图片上
                Bitmap bmp = new Bitmap(this.pictureBoxMeasureTable.Width - 60
                    , pictureBoxMeasureTable.Height
                    , System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(bmp);

                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                string filePath = "C:\\" + Guid.NewGuid().ToString() + ".wmf";
                Metafile mf = new Metafile(filePath, g.GetHdc(), rect, MetafileFrameUnit.Pixel);
                g = Graphics.FromImage(mf);

                PaintNurseDocument(pictureBoxMeasureTable, g);

                g.Save();
                g.Dispose();
                list.Add(new MetaFileInfo(filePath, bmp, mf));
            }

            SetWaitDialogCaption("正在批量打印！");
            printAllDocumentForm.Print(list);
            HideWaitDialog();
        }
        #endregion

        private void UCThreeMeasureTable_Click(object sender, EventArgs e)
        {
            simpleButton1.Focus();
        }

        private void pictureBoxMeasureTable_Click(object sender, EventArgs e)
        {
            simpleButton1.Focus();
        }

        WaitDialogForm m_WaitDialog;
        private void SetWaitDialogCaption(string caption)
        {
            if (m_WaitDialog == null)
                m_WaitDialog = new WaitDialogForm("正在加载数据......", "请您稍后！");
            if (!m_WaitDialog.Visible)
                m_WaitDialog.Visible = true;
            m_WaitDialog.Caption = caption;


        }

        private void HideWaitDialog()
        {
            if (m_WaitDialog != null)
                m_WaitDialog.Hide();
        }
    }

}
