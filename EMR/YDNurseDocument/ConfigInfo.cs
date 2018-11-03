using DrectSoft.DSSqlHelper;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Xml;
namespace DrectSoft.Core.NurseDocument
{
    /// <summary>
    /// 存储一些配置信息
    /// </summary>
    public abstract class ConfigInfo
    {
        public static string MetafilePath = ".\\PrintImage\\three.wmf";
        public static string m_xmlFilePath = "";
        public static XmlDocument xmlDoc = new XmlDocument();

        //public static Brush m_patientStateTextColor = null;//绘制病人状态事件的文本颜色
        //public static Brush m_specialPatientStateTextColor = null;//绘制特殊病人状态事件的文本颜色

        //针对事件的状态颜色，普通做一组，特殊做一组  add by ywk 2013年4月17日9:36:07 
        //所有状态对应各自的颜色 
        public static Brush m_patientStateTextColor = null;//绘制病人普通状态事件的文本颜色
        public static Brush m_InPatientStateTextColor = null;//绘制病人特殊状态事件的文本颜色


        public static Rectangle smallGridBound;//数据小方格范围区域
        public static RectangleF m_pageBound;//页号(周数)数据显示区域
        public static float smallGridHeight = 0, smallGridWidth = 0;//数据小方格高度，宽度
        public static XmlNode m_specialNode = null;
        public static Size dataIamgeSize;//显示数据的图片的Size       
        public static float m_eventYStart = 0, m_eventYEnd = 0;//绘制病人状态文本的起始,结束纵坐标
        public static float m_specialEventYStart = 0, m_specialEventYEnd = 0;//绘制特殊病人状态文本的起始,结束纵坐标
        public static int m_captionSize;
        public static Size m_lineIconSize;
        public static int pulseHeartRate = 0; //脉搏之间有心率 脉搏是否断开   0:不断开 1：断开
        public static int heartRatePulse = 0;//心率之间有心率脉搏，心率是否断开
        public static int temperatureAndHotting = 0;//体温点有升温 
        public static string breakLinkLine = "0";//当遇到特殊病人状态(如外出，手术，请假等)，体征数据线是否断开 0：不断开 1：断开

        //public static string inUsed = "0";//当遇到特殊病人状态(如外出，手术，请假等)，是否需要在其他区域绘制这些数据点
        public static string configName = string.Empty;//add zxy 配置文件名 2012-12-28 
        public static int subMoveX = -3;//x轴水平微调量 正数表示向右平移，反之向左平移，0表示不微调
        public static int lineMoveX = 0;//连线平移
        public static int editable = 1;
        public static int editDays = 1;
        public static bool isAdminDate = true;
        public static bool isInHospital = false;
        public static string version = string.Empty;
        public static string PluseAndHeartRate;//脉搏和心率重叠时显示脉搏或心率1表示脉搏2表示心率Add by xlb 2013-06-05
        public static string InHospitalCode = "";//入院状态的对应编码针对多家医院存在编码不一致的情况Add by xlb 2013-06-09
        public static string ShowStatus = "";//病人入院状态的显示文本Add by xlb 2013-06-09

        #region 配置参考信息的集合 如“坐标”“行数据代表的字段”等   Key:表字段名称
        public static Dictionary<string, VerticalCoordinate> dicVerticalCoordinate = new Dictionary<string, VerticalCoordinate>();//纵坐标集合 字段名称->坐标对象
        public static Dictionary<string, DataFieldRowConfigInfo> dicDataFieldStartPositin = new Dictionary<string, DataFieldRowConfigInfo>();//存放将要绘制的字段值的起点位置坐标
        public static Dictionary<string, string> dicVitalSignsLineColor = new Dictionary<string, string>();//点间连线的颜色
        public static Dictionary<string, string> dicVitalSignsLineType = new Dictionary<string, string>();//add zyx 2012-12-28 点间连线的类型
        public static Dictionary<string, VocateText> dicVocateValue = new Dictionary<string, VocateText>();//占位符信息字段值 如“病人信息”
        public static Dictionary<string, SpecialRowCellDisplay> dicSpecialRowCellDisplay = new Dictionary<string, SpecialRowCellDisplay>();//字段数据的特殊显示设置
        public static Dictionary<DataPoint, string> StateDataListFilter = new Dictionary<DataPoint, string>();//存储需要过滤体征数据的数据点(描述 病人状态)
        public static Dictionary<DataPoint, EnumReclos> ReclosingAssemble = new Dictionary<DataPoint, EnumReclos>();

        public static List<Column> m_columnList = new List<Column>();
        public static List<Row> m_rowList = new List<Row>();
        public static Dictionary<int, string[]> dic_HourOfDay = new Dictionary<int, string[]>();//存储时段配置信息
        public static Dictionary<int, string> dic_Timelot = new Dictionary<int, string>();//存储时段名称
        public static List<FilterState> StateValueTextList = new List<FilterState>();//特殊状态集合，对应XMl中的<patientstatefilter>节点
        public static XmlNode temperatureChangedNode = null;//保存温度变化一些配置节点数据

        #endregion

        #region 常量
        public static int m_Days = 7;//每周七天
        public static int subMove = 8;//微调像素
        public const string m_fontName = "新宋体";
        public const float m_hourcaptionSize = 7;
        public const int m_subHourCountOfDay = 6;//在天数据区域显示时段时的拆分列数
        #endregion


        //加载配置数据病保存至内存
        public static void LoadTableConfig(IEmrHost app, decimal currInpatient)
        {
            try
            {
                ClearGlobalVariable();
                m_xmlFilePath = GetXMLPath(currInpatient.ToString());
                xmlDoc = new XmlDocument();
                xmlDoc.Load(m_xmlFilePath);

                XmlNode nodeElement = xmlDoc.GetElementsByTagName("columns")[0];
                XmlNodeList nodeList = nodeElement.ChildNodes;

                dicDataFieldStartPositin.Clear();
                dicVocateValue.Clear();

                foreach (XmlNode node in nodeList)
                {
                    Column col = new Column();
                    col.index = Int32.Parse(node.Attributes["index"].Value);
                    col.width = Int32.Parse(node.Attributes["width"].Value);
                    m_columnList.Add(col);
                }
                nodeElement = xmlDoc.GetElementsByTagName("rows")[0];
                nodeList = nodeElement.ChildNodes;
                foreach (XmlNode node in nodeList)
                {
                    Row row = new Row();
                    row.index = Int32.Parse(node.Attributes["index"].Value);
                    row.height = Int32.Parse(node.Attributes["height"].Value);
                    row.startY = Int32.Parse(node.Attributes["y"].Value);
                    if (!node.Attributes["datafield"].Value.Equals(""))
                    {
                        DataFieldRowConfigInfo dataConfig = new DataFieldRowConfigInfo();
                        dataConfig.FieldName = node.Attributes["datafield"].Value;
                        dataConfig.height = Int32.Parse(node.Attributes["height"].Value);
                        dataConfig.PointStartPosition = new PointF(float.Parse(node.Attributes["x"].Value) + float.Parse(node.Attributes["width"].Value), float.Parse(node.Attributes["y"].Value));
                        dataConfig.DataShowType = Int32.Parse(node.Attributes["showType"].Value);
                        dataConfig.VerAlign = Int32.Parse(node.Attributes["verAlign"].Value);
                        dataConfig.FontSize = Int32.Parse(string.IsNullOrEmpty(node.Attributes["size"].Value) ? "0" : node.Attributes["size"].Value);
                        dataConfig.textColor = (node.Attributes["textColor"] == null || node.Attributes["textColor"].Value == "") ? "黑" : node.Attributes["textColor"].Value;
                        dataConfig.updown = node.Attributes["updown"] == null || node.Attributes["updown"].Value == "" || node.Attributes["updown"].Value == "1" ? true : false;
                        dicDataFieldStartPositin.Add(node.Attributes["datafield"].Value, dataConfig);
                    }
                    m_rowList.Add(row);
                }

                m_specialNode = xmlDoc.GetElementsByTagName("rows")[0];
                //----------------------------一般病人状态事件-------------------------------
                nodeElement = xmlDoc.GetElementsByTagName("EventYStart")[0];
                if (nodeElement != null)
                {
                    m_eventYStart = float.Parse((nodeElement.Attributes["startY"] == null || string.IsNullOrEmpty(nodeElement.Attributes["startY"].Value)) ? "0" : nodeElement.Attributes["startY"].Value);
                    m_eventYEnd = float.Parse((nodeElement.Attributes["endY"] == null || string.IsNullOrEmpty(nodeElement.Attributes["endY"].Value)) ? "0" : nodeElement.Attributes["endY"].Value);
                    //add by ywk 2013年4月17日9:40:02 注释
                    //m_patientStateTextColor = GetColorBrush((nodeElement.Attributes["color"] == null || string.IsNullOrEmpty(nodeElement.Attributes["color"].Value)) ? "红" : nodeElement.Attributes["color"].Value);
                    m_patientStateTextColor = GetColorBrush("红");//一般状态为红色
                }
                else
                {
                    throw new Exception("请检查<EventYStart>节点");
                }
                //----------------------------特殊病人状态事件-------------------------------
                nodeElement = xmlDoc.GetElementsByTagName("SpecialEventYStart")[0];
                if (nodeElement != null)
                {
                    m_specialEventYStart = float.Parse((nodeElement.Attributes["startY"] == null || string.IsNullOrEmpty(nodeElement.Attributes["startY"].Value)) ? "0" : nodeElement.Attributes["startY"].Value);
                    m_specialEventYEnd = float.Parse((nodeElement.Attributes["endY"] == null || string.IsNullOrEmpty(nodeElement.Attributes["endY"].Value)) ? "0" : nodeElement.Attributes["endY"].Value);
                    //add by ywk 2013年4月17日9:40:21 注释
                    //m_specialPatientStateTextColor = GetColorBrush((nodeElement.Attributes["color"] == null || string.IsNullOrEmpty(nodeElement.Attributes["color"].Value)) ? "红" : nodeElement.Attributes["color"].Value);
                    //inUsed = (nodeElement.Attributes["used"] == null || string.IsNullOrEmpty(nodeElement.Attributes["used"].Value)) ? "0" : nodeElement.Attributes["used"].Value;
                }
                else
                {
                    throw new Exception("请检查<SpecialEventYStart>节点");
                }
                //----------------------------带有占位符的信息-------------------------------
                nodeElement = xmlDoc.GetElementsByTagName("Vocate")[0];
                nodeList = nodeElement.ChildNodes;
                foreach (XmlNode node in nodeList)
                {
                    if (!node.Attributes["datafield"].Value.Equals(""))
                    {
                        VocateText vocate = new VocateText();
                        vocate.X = int.Parse(node.Attributes["startX"].Value);
                        vocate.Y = int.Parse(node.Attributes["startY"].Value);
                        vocate.Width = int.Parse(node.Attributes["width"].Value);
                        vocate.Height = int.Parse(node.Attributes["height"].Value);
                        vocate.DataFiled = node.Attributes["datafield"].Value;
                        if (vocate.DataFiled.Trim().ToUpper().Equals("INWARDDATE"))
                        {
                            isAdminDate = false;
                        }
                        else if (vocate.DataFiled.Trim().ToUpper().Equals("ADMITDATE"))
                        {
                            isAdminDate = true;
                        }
                        vocate.Caption = node.InnerText;
                        dicVocateValue.Add(node.Attributes["datafield"].Value, vocate);
                    }
                }
                //-----------------------------页号------------------------------
                nodeElement = xmlDoc.GetElementsByTagName("page")[0];
                m_pageBound = new Rectangle();
                m_pageBound.X = Int32.Parse(nodeElement.Attributes["startX"].Value);
                m_pageBound.Y = Int32.Parse(nodeElement.Attributes["startY"].Value);
                m_pageBound.Width = Int32.Parse(nodeElement.Attributes["width"].Value);
                m_pageBound.Height = Int32.Parse(nodeElement.Attributes["height"].Value);
                //-----------------------------时段------------------------------
                nodeElement = xmlDoc.GetElementsByTagName("HourOfday")[0];
                nodeList = nodeElement.ChildNodes;
                dic_HourOfDay.Clear();
                dic_Timelot.Clear();
                int tempIndex = -1;
                foreach (XmlNode node in nodeList)
                {
                    tempIndex = Int32.Parse(node.Attributes["index"].Value);
                    string[] hourPoints = node.Attributes["hourpoint"].Value.Split(',');
                    dic_HourOfDay.Add(tempIndex, hourPoints);
                    dic_Timelot.Add(tempIndex, node.Attributes["timeslotvalue"].Value);
                }
                //-----------------------------字段数据的特殊显示设置------------------------------
                dicSpecialRowCellDisplay.Clear();
                nodeElement = xmlDoc.GetElementsByTagName("rowSpecilaDispalySet")[0];
                nodeList = nodeElement.ChildNodes;
                foreach (XmlNode node in nodeList)
                {
                    if (!node.Attributes["datafield"].Value.Equals(""))
                    {
                        SpecialRowCellDisplay specialDiasplay = new SpecialRowCellDisplay();
                        specialDiasplay.FieldName = node.Attributes["datafield"] == null ? "" : node.Attributes["datafield"].Value;
                        specialDiasplay.ValueKey = node.Attributes["valueKey"] == null ? "" : node.Attributes["valueKey"].Value;
                        specialDiasplay.TextColor = node.Attributes["dispalyColor"] == null ? "" : node.Attributes["displayColor"].Value;
                        specialDiasplay.TextSize = node.Attributes["size"] == null ? 0f : float.Parse(node.Attributes["size"].Value);
                        dicSpecialRowCellDisplay.Add(specialDiasplay.FieldName, specialDiasplay);
                    }
                }
                //-----------------------------监护曲线数据点图标大小------------------------------
                nodeList = xmlDoc.GetElementsByTagName("fonticonsize");
                if (nodeList == null || nodeList.Count == 0)
                {
                    throw new Exception("请检查<fonticonsize>节点");
                }
                nodeElement = nodeList[0];
                m_lineIconSize.Width = int.Parse(nodeElement.Attributes["width"] == null ? "0" : nodeElement.Attributes["width"].Value);
                m_lineIconSize.Height = int.Parse(nodeElement.Attributes["height"] == null ? "0" : nodeElement.Attributes["height"].Value);
                subMoveX = int.Parse(nodeElement.Attributes["subMoveX"] == null || nodeElement.Attributes["subMoveX"].Value == "" ? "-3" : nodeElement.Attributes["subMoveX"].Value);
                lineMoveX = int.Parse(nodeElement.Attributes["lineMoveX"] == null || nodeElement.Attributes["lineMoveX"].Value == "" ? "0" : nodeElement.Attributes["lineMoveX"].Value);
                //-----------------------------填充数据的字号大小------------------------------
                nodeList = xmlDoc.GetElementsByTagName("fontsize");
                if (nodeList == null || nodeList.Count == 0)
                {
                    throw new Exception("请检查<fontsize>节点");
                }
                nodeElement = nodeList[0];
                m_captionSize = int.Parse(nodeElement.Attributes["size"] == null ? "0" : nodeElement.Attributes["size"].Value);
                //-----------------------------过滤体征点的病人状态集合------------------------------
                nodeElement = xmlDoc.GetElementsByTagName("patientstatefilter")[0];
                if (nodeElement != null)
                {
                    // breakLinkLine = (nodeElement.Attributes["breakLinkLine"] == null || string.IsNullOrEmpty(nodeElement.Attributes["breakLinkLine"].Value)) ? "1" : nodeElement.Attributes["breakLinkLine"].Value;
                    nodeList = nodeElement.ChildNodes;
                    foreach (XmlNode node in nodeList)
                    {
                        string caption = node.Attributes["valueText"] == null ? "" : node.Attributes["valueText"].Value;
                        string breakLine = node.Attributes["breakLinkLine"] == null || string.IsNullOrEmpty(node.Attributes["breakLinkLine"].Value) ? "0" : node.Attributes["breakLinkLine"].Value;
                        if (breakLine.Equals("1"))
                        {
                            breakLinkLine = "1";
                        }
                        string position = node.Attributes["position"] == null || string.IsNullOrEmpty(node.Attributes["position"].Value) ? "0" : node.Attributes["position"].Value;
                        string showtime = node.Attributes["showtime"] == null || string.IsNullOrEmpty(node.Attributes["showtime"].Value) ? "1" : node.Attributes["showtime"].Value;

                        //新增的对于各个状态的显示颜色的定义取值实现， add by ywk 2013年4月17日9:44:31 
                        string showcolor = node.Attributes["color"] == null || string.IsNullOrEmpty(node.Attributes["color"].Value) ? "红" : node.Attributes["color"].Value;
                        StateValueTextList.Add(new FilterState(caption, position, breakLine, showtime, showcolor));
                        //在循环各个状态时将颜色相应的加进去
                    }
                }
                else
                {
                    throw new Exception("请检查<patientstatefilter>节点");
                }
                //-----------------------------脉搏 心率 之间连线规则------------------------------
                nodeList = xmlDoc.GetElementsByTagName("specialLinkline");
                if (nodeList == null || nodeList.Count == 0)
                {
                    throw new Exception("请检查<specialLinkline>节点");
                }
                nodeElement = nodeList[0];
                pulseHeartRate = int.Parse(nodeElement.Attributes["pulseHeartRate"] == null ? "0" : nodeElement.Attributes["pulseHeartRate"].Value);
                //脉搏心率重叠时优先显示脉搏或心率 Add by xlb 2013-05-31
                PluseAndHeartRate = nodeElement.Attributes["showType"] == null || nodeElement.Attributes["showType"].Value == "" ? "1" : nodeElement.Attributes["showType"].Value;
                heartRatePulse = int.Parse(nodeElement.Attributes["heartRatePulse"] == null ? "0" : nodeElement.Attributes["heartRatePulse"].Value);
                //-----------------------------温度变化规则配置------------------------------
                nodeList = xmlDoc.GetElementsByTagName("TemperatureChanged");
                if (nodeList == null || nodeList.Count == 0)
                {
                    throw new Exception("请检查<TemperatureChanged>节点");
                }
                temperatureChangedNode = nodeList[0];

                //-----------------------------术后天数是否可编辑------------------------------
                nodeElement = xmlDoc.GetElementsByTagName("DateEditable")[0];
                if (nodeElement == null)
                {
                    editable = 1;
                }
                else
                {
                    editable = int.Parse(nodeElement.Attributes["editable"] == null || nodeElement.Attributes["editable"].Value == "" ? "1" : nodeElement.Attributes["editable"].Value);
                }
                //--------------------------住院天数是否可编辑------------------------------
                nodeElement = xmlDoc.GetElementsByTagName("DateHospitalEdit")[0];
                if (nodeElement == null)
                {
                    editDays = 1;
                }
                else
                {
                    editDays = int.Parse(nodeElement.Attributes["editable"] == null || nodeElement.Attributes["editable"].Value == "" ? "1" : nodeElement.Attributes["editable"].Value);
                }
                //--------------------------是否自动添加入院状态------------------------------
                nodeElement = xmlDoc.GetElementsByTagName("ShowHospital")[0];
                if (nodeElement == null)
                {
                    isInHospital = false;
                    ShowStatus = "入院";
                    InHospitalCode = "7008";
                }
                else
                {
                    isInHospital = nodeElement.Attributes["isShow"] == null || nodeElement.Attributes["isShow"].Value == "" || nodeElement.Attributes["isShow"].Value.Trim() == "0" ? false : true;
                    //病人入院状态显示文本(Add by xlb 2013-06-09)
                    ShowStatus = nodeElement.Attributes["showText"] == null || nodeElement.Attributes["showText"].Value == "" ? "入院" : nodeElement.Attributes["showText"].Value;
                    //入院状态对应的代码需匹配各家医院(Add by xlb 2013-06-09)
                    InHospitalCode = nodeElement.Attributes["inHospitalCode"] == null || nodeElement.Attributes["inHospitalCode"].Value == "" ? "7008" : nodeElement.Attributes["inHospitalCode"].Value;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\r\n" + "尝试检查配置文件是否配置合法");
            }
        }

        /// <summary>
        /// 获得图片大小
        /// </summary>
        /// <returns></returns>
        public static Size GetImagePageBound()
        {
            try
            {
                XmlNodeList nodeList = xmlDoc.GetElementsByTagName("size");
                if (nodeList == null || nodeList.Count == 0)
                {
                    throw new Exception("请检查<size>节点");
                }
                XmlNode nodeElement = nodeList[0];
                Size size = new Size();
                size.Width = int.Parse(nodeElement.Attributes["width"] == null ? "0" : nodeElement.Attributes["width"].Value);
                size.Height = int.Parse(nodeElement.Attributes["height"] == null ? "0" : nodeElement.Attributes["height"].Value);
                return size;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取体征录入界面使用版本
        /// </summary>
        /// <param name="currInpatient"></param>
        public static string GetNurseMeasureVersion(decimal currInpatient)
        {
            try
            {
                string path = GetXMLPath(currInpatient.ToString());
                xmlDoc.Load(path);
                XmlNode xmlNode = xmlDoc.GetElementsByTagName("NurseMeasure")[0];
                if (xmlNode == null)
                {
                    version = "DrectSoft.Core.NurseDocument.Controls.NursingRecord";
                }
                else
                {
                    version = xmlNode.Attributes["src"] == null ? "DrectSoft.Core.NurseDocument.Controls.NursingRecord" : xmlNode.Attributes["src"].Value;
                }
                return version;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int GetNurseRecordSize(decimal currInpatient)
        {
            try
            {
                string path = GetXMLPath(currInpatient.ToString());
                xmlDoc.Load(path);
                XmlNode xmlNode = xmlDoc.GetElementsByTagName("NurseMeasure")[0];
                /*兼容原XML结构*/
                if (xmlNode == null)
                {
                    return 575;
                }
                int height = int.Parse(xmlNode.Attributes["height"] == null ? "600" : xmlNode.Attributes["height"].Value);
                return height;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Brush GetColorBrush(string color)
        {
            try
            {
                switch (color)
                {
                    case "红":
                        return Brushes.Red;
                    case "蓝":
                        return Brushes.Blue;
                    case "黑":
                        return Brushes.Black;
                    case "黄"://新增几个 
                        return Brushes.Yellow;
                    case "紫"://新增几个 
                        return Brushes.Purple;
                    case "橙"://新增几个 
                        return Brushes.Orange;
                    case "灰"://新增几个 
                        return Brushes.Gray;
                    case "粉":
                        return Brushes.Pink;
                    case "绿":
                        return Brushes.Green;
                    default:
                        return Brushes.Red;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Pen GetColorPen(string color)
        {
            try
            {
                Pen pen = null;
                switch (color)
                {
                    case "红":
                        pen = new Pen(Brushes.Red);
                        break;
                    case "蓝":
                        pen = new Pen(Brushes.Blue);
                        break;
                    case "黑":
                        pen = new Pen(Brushes.Black);
                        break;
                    case "绿":
                        pen = new Pen(Brushes.Green);
                        break;
                    default:
                        pen = new Pen(Brushes.Red);
                        break;
                }
                return pen;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //获得时段的Index  1-6
        public static int GetHourDuringIndex(string datetime)
        {
            try
            {
                DateTime _datetime = DateTime.Parse(datetime);
                string hour = _datetime.Hour.ToString();
                foreach (KeyValuePair<int, string[]> pair in dic_HourOfDay)
                {
                    if (pair.Value.Contains(hour))
                        return pair.Key;
                }
            }
            catch (Exception ex) { throw ex; }
            return -1;
        }

        //得到时段的索引
        public static int GetTimelotIndex(string timelot)
        {
            try
            {
                foreach (KeyValuePair<int, string> pair in dic_Timelot)
                {
                    if (pair.Value.Contains(timelot))
                        return pair.Key;
                }
            }
            catch (Exception ex) { throw ex; }
            return -1;
        }

        /// <summary>
        /// 根据配置文件获取一个画笔
        /// <auth>zyx</auth>
        /// <date>2012-12-28</date>
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Pen GetPen(string fileName)
        {
            try
            {
                string color = dicVitalSignsLineColor[fileName];
                Pen pen = new Pen(Brushes.Red);
                switch (color)
                {
                    case "红":
                        pen.Brush = Brushes.Red;
                        break;
                    case "蓝":
                        pen.Brush = Brushes.Blue;
                        break;
                    case "黑":
                        pen.Brush = Brushes.Black;
                        break;
                    case "绿":
                        pen.Brush = Brushes.Green;
                        break;
                    default:
                        pen.Brush = Brushes.Red;
                        break;
                }

                string lineType = dicVitalSignsLineType[fileName];

                switch (lineType)
                {
                    case "dotted":
                        pen.DashStyle = DashStyle.Custom;
                        pen.DashPattern = new float[] { 6, 2 };
                        break;
                    default:
                        pen.DashStyle = DashStyle.Solid;
                        break;
                }
                return pen;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 获取体温单配置路径
        /// <auth>zyx</uath>
        /// <date>2012-12-28</date>
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static string GetXMLPath(string currentInpatient)
        {
            try
            {
                if (currentInpatient != null && currentInpatient != "")
                {
                    DS_SqlHelper.CreateSqlHelper();
                    string noofinpat = currentInpatient.ToString();
                    string sql = "select * from Inpatient i where ISBABY='1' and noofinpat=@noofinpat";
                    SqlParameter[] parms = new SqlParameter[] 
                    {
                        new SqlParameter("@noofinpat",SqlDbType.NVarChar)
                    };
                    parms[0].Value = noofinpat;
                    DataTable dt = DS_SqlHelper.ExecuteDataTable(sql, parms, CommandType.Text);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Sheet\\Configbaby.xml"))
                        {
                            configName = "Configbaby.xml";
                            return AppDomain.CurrentDomain.BaseDirectory + "Sheet\\Configbaby.xml";
                        }
                        else
                        {
                            configName = "Config.xml";
                            return AppDomain.CurrentDomain.BaseDirectory + "Sheet\\Config.xml";
                        }
                    }
                    else
                    {
                        configName = "Config.xml";
                        return AppDomain.CurrentDomain.BaseDirectory + "Sheet\\Config.xml";
                    }
                }
                else
                {
                    return AppDomain.CurrentDomain.BaseDirectory + "Sheet\\" + configName;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        /// <summary>
        /// xll 2013-2-22 获取体温单的默认大小
        /// 无论是婴儿体温单还是成人体温单 都是同一个默认值
        /// 先取成人的  再取婴儿的
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultPrintSize()
        {
            try
            {
                xmlDoc = new XmlDocument();
                string conxml = "";
                if (File.Exists(GetXMLPath(null)))
                {
                    conxml = GetXMLPath(null);
                }
                else
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("体温单配置文件不存在");
                    return null;
                }
                xmlDoc.Load(conxml);
                XmlNode xnode = xmlDoc.SelectSingleNode("/root/defaultprint");
                if (xnode == null)
                {
                    return null;
                }
                else
                {
                    return xnode.InnerText;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        /// <summary>
        /// 清空集合信息
        /// <auth>zyx</auth>
        /// <date>2012-12-28</date>
        /// </summary>
        public static void ClearGlobalVariable()
        {
            try
            {
                dicVerticalCoordinate.Clear();
                dicDataFieldStartPositin.Clear();
                dicVitalSignsLineColor.Clear();
                dicVocateValue.Clear();
                dicSpecialRowCellDisplay.Clear();
                StateDataListFilter.Clear();
                m_columnList.Clear();
                m_rowList.Clear();
                dic_HourOfDay.Clear();
                dic_Timelot.Clear();
                StateValueTextList.Clear();
                temperatureChangedNode = null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 根据状态名称获取此状态应该显示的颜色
        /// add by ywk 2013年4月17日10:10:05 
        /// </summary>
        /// <param name="statename"></param>
        /// <returns></returns>
        public static Brush GetColorByStateName(string statename)
        {
            Brush m_brush = Brushes.Red;//默认红色
            if (xmlDoc == null)
            {
                xmlDoc.Load(GetXMLPath(null));
            }

            XmlNode nodeElement = xmlDoc.GetElementsByTagName("columns")[0];
            nodeElement = xmlDoc.GetElementsByTagName("patientstatefilter")[0];
            XmlNodeList nodeList = nodeElement.ChildNodes;

            foreach (XmlNode node in nodeList)
            {
                string caption = node.Attributes["valueText"] == null ? "" : node.Attributes["valueText"].Value;
                if (statename == caption)
                {
                    m_brush = GetColorBrush(node.Attributes["color"] == null ? "红" : node.Attributes["color"].Value);
                    break;
                }

            }
            return m_brush;
        }
    }

    public struct Column
    {
        public int index;
        public float width;
    }

    public struct Row
    {
        public int index;
        public float height;
        public float startY;
    }

    public struct FilterState
    {
        /// <summary>
        /// 是否断开体温连线
        /// </summary>
        public string BreakLine;
        /// <summary>
        /// 是否显示时间
        /// </summary>
        public string ShowTime;
        /// <summary>
        /// 事件状态名称
        /// </summary>
        public string stateName;
        /// <summary>
        /// 显示位置 上方或下方
        /// </summary>
        public string position;

        /// <summary>
        ///状态显示颜色  add by ywk 2013年4月17日9:42:38 增加颜色
        /// <summary>
        public string ShowColor;

        public FilterState(string _stateName, string _position, string _breakLine, string _showTime, string _showcolor)
        {
            this.stateName = _stateName;
            this.position = _position;
            this.BreakLine = _breakLine;
            this.ShowTime = _showTime;
            this.ShowColor = _showcolor;//add by ywk 2013年4月17日9:43:38  
        }
    }
}
