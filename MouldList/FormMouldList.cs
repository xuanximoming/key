using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraTab;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.FrameWork.Plugin.Manager;
using System.Collections.ObjectModel;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.Resources;
using System.Drawing.Drawing2D;
using System.Xml;

namespace DrectSoft.Core.MouldList
{
    public partial class FormMouldList : Form, IStartPlugIn
    {
        private Boolean m_IsLoaded = false;
        public FormMouldList()
            : this(null)
        {
        }

        public FormMouldList(IEmrHost app)
        {
            //双缓冲
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            InitializeComponent();
            m_App = app;


        }


        Panel group = new Panel();
        private IEmrHost m_App;
        //设置Groupbox控件文本的字体
        Font myFont = new Font("宋体", 10, FontStyle.Bold, GraphicsUnit.Point);

        //根据分辨率得到的界面实际高度
        private Int32 m_ActualWidth;
        private Int32 m_ActualHeight;

        //标题列表
        private List<String> m_ListCaption = new List<string>();
        private List<PictureBox> m_ListCaptionPictureBox = new List<PictureBox>();

        //标题图片(背景)
        private List<Image> m_ListImage = new List<Image>();

        //标题图片(正常情况)
        private List<Image> m_ListImageNormal = new List<Image>();

        //标题图片(鼠标移入情况)
        private List<Image> m_ListImageMouseEntry = new List<Image>();

        //标题图片(鼠标点击情况)
        private List<Image> m_ListImageMouseClick = new List<Image>();



        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            //myCount += 1;
            if (m_IsLoaded)
                return;
            m_IsLoaded = true;

            InitMainFormLayout();

            InitTab();
            vScrollBarRight.Scroll += new ScrollEventHandler(vScrollBarRight_Scroll);
            vScrollBarRight.MouseUp += new MouseEventHandler(vScrollBarRight_MouseUp);
        }

        void tabModule_KeyDown(object sender, KeyEventArgs e)
        {
            //屏蔽在界面按下左右方向键
            if (e.KeyCode == Keys.Left)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Right)
            {
                e.Handled = true;
            }
        }

        #region 初始化TabControl

        private void SetImage()
        {
            //设置标题部分
            //pictureBoxHeadLeft.BackgroundImage = Resources.ResourceManager.GetImage(Resources.ResourceNames.HeadLeft);

            //pictureBoxTool.BackgroundImage = Resources.ResourceManager.GetImage(Resources.ResourceNames.ToolNoButton);

            if (m_ListImage.Count > 0)
            {
                return;
            }

            //正常情况
            //Image image = Image.FromFile(@"图片\TabPageHeadBtn1.png");
            Image image = Resources.ResourceManager.GetImage(Resources.ResourceNames.ModuleListTabHeaderNormal);
            m_ListImage.Add(image);

            //鼠标移上
            //image = Image.FromFile(@"图片\TabPageHeadBtn2.png");
            image = Resources.ResourceManager.GetImage(Resources.ResourceNames.ModuleListTabHeaderMouseMove);
            m_ListImage.Add(image);

            //鼠标点击
            //image = Image.FromFile(@"图片\TabPageHeadBtn3.png");
            image = Resources.ResourceManager.GetImage(Resources.ResourceNames.ModuleListTabHeaderMouseClick);
            m_ListImage.Add(image);
        }

        /// <summary>
        /// 初始化TabControl
        /// </summary>
        private void InitTab()
        {
            if (m_App == null) return;

            //设置图片
            SetImage();

            //设置每个小模块的名字
            SetModuleItemName();

            m_ActualWidth = panelZoom.Width;
            m_ActualHeight = panelZoom.Height;

            //生成groupbox的控制,填充Panel
            SetPanelZoom();
        }
        /// <summary>
        /// 生成groupbox的控制，填充到Panelzoom的控制  2012年2月27日13:23:31
        /// </summary>
        private void SetPanelZoom()
        {
            if (m_App == null) return;

            this.group.Controls.Clear();//消除切换用户绘出控件重叠的BUG 2012年2月29日11:04:44
            //最外圈的groupbox控制
            group.Margin = new Padding(0, 0, 0, 0);
            group.Location = new Point();
            group.BackColor = Color.Transparent;
            panelZoom.BackColor = m_GroupBackgroundBrush.Color;
            vScrollBarRight.Value = 0;
            vScrollBarRight.Width = m_ScrollBarWidth;
            InitGroup();
            if (!panelZoom.Controls.Contains(group))
            {
                panelZoom.Controls.Add(group);
            }
        }

        /// <summary>
        /// 加载界面图片
        /// </summary>
        private void InitGroup()
        {
            try
            {
                if (m_ListCaption == null)
                    m_ListCaption = new List<string>();
                if (m_ListCaption.Count == 0)
                {
                    m_ListCaption.Add("");
                    m_ListCaption.Add("");
                    m_ListCaption.Add("");
                }
                if (m_ListCaption.Count == 1)
                {
                    m_ListCaption.Add("");
                    m_ListCaption.Add("");
                }
                if (m_ListCaption.Count == 2)
                {
                    m_ListCaption.Add("");
                }
                var listModuleTemp0 = from info in m_App.PrivilegeMenu
                                      where info.MenuParentCaption.Equals(m_ListCaption[0])
                                      select info;
                var listModuleTemp1 = from info in m_App.PrivilegeMenu
                                      where info.MenuParentCaption.Equals(m_ListCaption[1])
                                      select info;
                var listModuleTemp2 = from info in m_App.PrivilegeMenu
                                      where info.MenuParentCaption.Equals(m_ListCaption[2])
                                      select info;

                List<PlugInConfiguration> list0 = listModuleTemp0.ToList();
                List<PlugInConfiguration> list1 = listModuleTemp1.ToList();
                List<PlugInConfiguration> list2 = listModuleTemp2.ToList();

                GroupBox GroupBox0 = new GroupBox();
                GroupBox GroupBox1 = new GroupBox();
                GroupBox GroupBox2 = new GroupBox();

                GroupBox0.BackColor = Color.Transparent;
                GroupBox1.BackColor = Color.Transparent;
                GroupBox2.BackColor = Color.Transparent;

                //group.AutoScroll = true;
                if (list0 != null && list0.Count > 0)
                {
                    #region 设置GroupBox0
                    GroupBox0.Text = m_ListCaption[0];
                    GroupBox0.Font = myFont;
                    for (int i = 0; i < list0.Count; i++)
                    {
                        PlugInConfiguration plugInfo = list0[i];
                        if (!String.IsNullOrEmpty(plugInfo.IconName))
                            plugInfo.Icon = DrectSoft.Resources.ResourceManager.GetMiddleIcon(plugInfo.IconName, IconType.Normal);

                        UCMould moule = new UCMould(plugInfo);
                        moule.EnterSingleOrDoubleClick = m_EnterSingleOrDoubleClick;
                        moule.MouldFunClick += new UCMould.MouldFunClickEventHandler(moule_MouldFunClick);
                        GroupBox0.Controls.Add(moule);
                    }
                    SetGroupBox(GroupBox0);
                    #endregion
                }
                if (list1 != null && list1.Count > 0)
                {
                    #region 设置GroupBox1
                    GroupBox1.Text = m_ListCaption[1];
                    GroupBox1.Font = myFont;
                    for (int i = 0; i < list1.Count; i++)
                    {
                        PlugInConfiguration plugInfo = list1[i];
                        if (!String.IsNullOrEmpty(plugInfo.IconName))
                            plugInfo.Icon = DrectSoft.Resources.ResourceManager.GetMiddleIcon(plugInfo.IconName, IconType.Normal);

                        UCMould moule = new UCMould(plugInfo);
                        moule.EnterSingleOrDoubleClick = m_EnterSingleOrDoubleClick;
                        moule.MouldFunClick += new UCMould.MouldFunClickEventHandler(moule_MouldFunClick);
                        GroupBox1.Controls.Add(moule);
                    }

                    SetGroupBox(GroupBox1);
                    #endregion
                }
                if (list2 != null && list2.Count > 0)
                {
                    #region 设置GroupBox2
                    GroupBox2.Text = m_ListCaption[2];
                    GroupBox2.Font = myFont;
                    for (int i = 0; i < list2.Count; i++)
                    {
                        PlugInConfiguration plugInfo = list2[i];
                        if (!String.IsNullOrEmpty(plugInfo.IconName))
                            plugInfo.Icon = DrectSoft.Resources.ResourceManager.GetMiddleIcon(plugInfo.IconName, IconType.Normal);

                        UCMould moule = new UCMould(plugInfo);
                        moule.EnterSingleOrDoubleClick = m_EnterSingleOrDoubleClick;
                        moule.MouldFunClick += new UCMould.MouldFunClickEventHandler(moule_MouldFunClick);
                        GroupBox2.Controls.Add(moule);
                    }

                    SetGroupBox(GroupBox2);
                    #endregion
                }
                ProcessGroupBox();
            }
            catch (Exception)
            {
                throw;
            }
        }

        int m_OffsetXGroupBox = 15;//GroupBox的X轴方向的偏移量
        int m_OffsetXUCMould = 20;//UCMould的X轴方向的偏移量(在GroupBox中的偏移量),最后还要对UCMould的偏移量进行调整
        int m_OffsetYUCMould = 10;//第一行和最后一行的UCMould与其GroupBox的间距
        int m_IntervalHeightBetweenGroupBoxes = 15;//GroupBox之间的距离
        int m_IntervalWidthBetweenMoulds = 5;//UCMould之间的横向距离
        int m_IntervalHeightBetweenMoulds = 4;//UCMould之间的纵向距离
        int m_WidthMould = 250;//UCMould的宽度
        int m_HeightMould = 80;//UCMould的高度
        int m_ScrollBarWidth = 30;//右侧滚动条的宽度
        int m_IsShowRightBottomImage = 1;//是否显示右下角区域的图片 0:不显示 1:显示
        int m_MoveStyle = 2;//界面的滑动方式 1:随着滚动条的移动进行联动 2:鼠标松开时移动位置
        int m_EnterSingleOrDoubleClick = 2;//设置通过单击或双击进入对应的功能模块 1:单击 2:双击

        private void InitMainFormLayout()
        {
            try
            {
                string openMessageWindowConfig = new AppConfigReader().GetConfig("MainFormLayout").Config;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(openMessageWindowConfig);
                XmlNodeList nodeList = doc.GetElementsByTagName("OffsetXGroupBox");
                if (nodeList.Count > 0)
                {
                    m_OffsetXGroupBox = Convert.ToInt32(nodeList[0].InnerText);
                }
                nodeList = doc.GetElementsByTagName("OffsetXUCMould");
                if (nodeList.Count > 0)
                {
                    m_OffsetXUCMould = Convert.ToInt32(nodeList[0].InnerText);
                }
                nodeList = doc.GetElementsByTagName("OffsetYUCMould");
                if (nodeList.Count > 0)
                {
                    m_OffsetYUCMould = Convert.ToInt32(nodeList[0].InnerText);
                }
                nodeList = doc.GetElementsByTagName("IntervalHeightBetweenGroupBoxes");
                if (nodeList.Count > 0)
                {
                    m_IntervalHeightBetweenGroupBoxes = Convert.ToInt32(nodeList[0].InnerText);
                }
                nodeList = doc.GetElementsByTagName("IntervalWidthBetweenMoulds");
                if (nodeList.Count > 0)
                {
                    m_IntervalWidthBetweenMoulds = Convert.ToInt32(nodeList[0].InnerText);
                }
                nodeList = doc.GetElementsByTagName("IntervalHeightBetweenMoulds");
                if (nodeList.Count > 0)
                {
                    m_IntervalHeightBetweenMoulds = Convert.ToInt32(nodeList[0].InnerText);
                }
                nodeList = doc.GetElementsByTagName("WidthMould");
                if (nodeList.Count > 0)
                {
                    m_WidthMould = Convert.ToInt32(nodeList[0].InnerText);
                }
                nodeList = doc.GetElementsByTagName("HeightMould");
                if (nodeList.Count > 0)
                {
                    m_HeightMould = Convert.ToInt32(nodeList[0].InnerText);
                }
                nodeList = doc.GetElementsByTagName("ScrollBarWidth");
                if (nodeList.Count > 0)
                {
                    m_ScrollBarWidth = Convert.ToInt32(nodeList[0].InnerText);
                }
                nodeList = doc.GetElementsByTagName("IsShowRightBottomImage");
                if (nodeList.Count > 0)
                {
                    m_IsShowRightBottomImage = Convert.ToInt32(nodeList[0].InnerText);
                }
                nodeList = doc.GetElementsByTagName("MoveStyle");
                if (nodeList.Count > 0)
                {
                    m_MoveStyle = Convert.ToInt32(nodeList[0].InnerText);
                }
                nodeList = doc.GetElementsByTagName("EnterSingleOrDoubleClick");
                if (nodeList.Count > 0)
                {
                    m_EnterSingleOrDoubleClick = Convert.ToInt32(nodeList[0].InnerText);
                }
            }
            catch (Exception)
            {
            }
        }

        private void SetGroupBox(GroupBox gb)
        {
            try
            {
                if (group.Controls.Count > 0)
                {
                    gb.Location = new Point(m_OffsetXGroupBox,
                        group.Controls[group.Controls.Count - 1].Location.Y +
                        group.Controls[group.Controls.Count - 1].Height + m_IntervalHeightBetweenGroupBoxes);
                }
                else
                {
                    gb.Location = new Point(m_OffsetXGroupBox, m_IntervalHeightBetweenGroupBoxes);
                }

                //计算一行中可以包含多少个UCMould
                int mouldCountInLine = (m_ActualWidth - m_OffsetXGroupBox * 2 - m_OffsetXUCMould * 2 - m_IntervalWidthBetweenMoulds) / (m_WidthMould + m_IntervalWidthBetweenMoulds);

                if (mouldCountInLine == 0) return;

                //Groupbox中的行数
                int lineCount = Convert.ToInt32(Math.Ceiling(gb.Controls.Count / (mouldCountInLine * 1.0)));
                gb.Height = m_OffsetYUCMould * 2 + (m_HeightMould + m_IntervalHeightBetweenMoulds) * lineCount - m_IntervalHeightBetweenMoulds;
                gb.Width = m_ActualWidth - m_OffsetXGroupBox * 2;
                gb.Tag = mouldCountInLine + " " + gb.Controls.Count;//在Tag中记录此GroupBox中一行可以容纳多少UCMould 和 此GroupBox中包含的所有的UCMould

                for (int rowIndex = 0; rowIndex < lineCount; rowIndex++)
                {
                    for (int columnIndex = 0; columnIndex < mouldCountInLine; columnIndex++)
                    {
                        if (rowIndex * mouldCountInLine + columnIndex < gb.Controls.Count)
                        {
                            UCMould mould = gb.Controls[rowIndex * mouldCountInLine + columnIndex] as UCMould;
                            if (mould != null)
                            {
                                int startPosition = m_OffsetXUCMould + m_IntervalWidthBetweenMoulds / 2;
                                int positionX = startPosition + columnIndex * (m_WidthMould + m_IntervalWidthBetweenMoulds);
                                int positionY = m_OffsetYUCMould + (m_HeightMould + m_IntervalHeightBetweenMoulds) * rowIndex;
                                mould.Location = new Point(positionX, positionY);
                            }
                        }
                    }
                }

                group.Controls.Add(gb);
                group.Width = m_ActualWidth;
                group.Height = gb.Location.Y + gb.Height + m_IntervalHeightBetweenGroupBoxes;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 重新处理GroupBox中的UCMould的位置
        /// </summary>
        private void ProcessGroupBox()
        {
            try
            {
                if (group.Height > panelZoom.Height)
                {
                    vScrollBarRight.Visible = true;
                    vScrollBarRight.Maximum = group.Height - panelZoom.Height + 80;
                    vScrollBarRight.LargeChange = 80;
                    vScrollBarRight.SmallChange = 20;

                    group.Controls.OfType<GroupBox>().ToList().ForEach(gb =>
                    {
                        gb.Width = gb.Width - vScrollBarRight.Width;
                    });
                }
                else
                {
                    vScrollBarRight.Visible = false;
                }

                //获取需要重新处理的GroupBox
                GroupBox groupBoxNeedProcess = group.Controls.OfType<GroupBox>().Where((gb) =>
                {
                    if (gb.Tag != null && gb.Tag.ToString().Split(' ').Length >= 2)
                    {
                        string value = gb.Tag.ToString();
                        int mouldCountInLine = Convert.ToInt32(value.Split(' ')[0]);//一行中UCMould的数目
                        int mouldCount = Convert.ToInt32(value.Split(' ')[1]);//GroupBox中包含的所有UCMould的数目
                        if (mouldCount >= mouldCountInLine)
                        {
                            return true;
                        }
                    }
                    return false;
                }).FirstOrDefault();
                if (groupBoxNeedProcess != null)
                {
                    string value = groupBoxNeedProcess.Tag.ToString();
                    int mouldCountInLine = Convert.ToInt32(value.Split(' ')[0]);//一行中UCMould的数目
                    int mouldLineBeginLocationX = groupBoxNeedProcess.Controls[0].Location.X;
                    int mouldLineEndLocationX = groupBoxNeedProcess.Controls[mouldCountInLine - 1].Location.X
                        + groupBoxNeedProcess.Controls[mouldCountInLine - 1].Width;
                    int groupBoxWidth = groupBoxNeedProcess.Width;
                    int offsetX = (groupBoxWidth - mouldLineEndLocationX - mouldLineBeginLocationX) / 2;

                    group.Controls.OfType<GroupBox>().ToList().ForEach(gb =>
                    {
                        gb.Controls.OfType<UCMould>().ToList().ForEach(mould =>
                            {
                                mould.Location = new Point(mould.Location.X + offsetX, mould.Location.Y);
                            });
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        void vScrollBarRight_Scroll(object sender, ScrollEventArgs e)
        {
            if (m_MoveStyle == 1)
            {
                group.Top = -vScrollBarRight.Value;
            }
        }

        void vScrollBarRight_MouseUp(object sender, MouseEventArgs e)
        {
            if (m_MoveStyle == 2)
            {
                group.Top = -vScrollBarRight.Value;
            }
        }
        #endregion

        #region InitTab函数中用到的方法

        private void SelectFirstPage()
        {
            if (m_ListCaptionPictureBox.Count > 0)
            {
                if (m_ListCaptionPictureBox.Count > 0)
                {
                    PictureBox pb = m_ListCaptionPictureBox[0];
                    pb_Click((object)pb, EventArgs.Empty);
                }
            }

        }

        /// <summary>
        /// 设置每个小模块的名字
        /// </summary>
        private void SetModuleItemName()
        {
            m_ListCaption = new List<string>();
            foreach (PlugInConfiguration info in m_App.PrivilegeMenu)
            {
                if (!m_ListCaption.Contains(info.MenuParentCaption))
                {
                    m_ListCaption.Add(info.MenuParentCaption);
                }
            }
        }

        private void DrawInImage(Image image, string caption, Brush brush)
        {
            using (Graphics g = Graphics.FromImage(image))
            {
                Font font = new Font("黑体", 18, FontStyle.Regular);
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                g.DrawString(caption, font, brush, new RectangleF(0, 10, image.Width, image.Height), sf);
            }
        }

        void pb_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        void pb_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        void pb_Click(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            if (pb == null)
            {
                return;
            }

            //改变图片
            for (int i = 0; i < m_ListCaptionPictureBox.Count; i++)
            {
                PictureBox pictureBox = m_ListCaptionPictureBox[i];

                if (pictureBox.Tag.ToString() == pb.Tag.ToString())
                {
                    pictureBox.BackgroundImage = m_ListImageMouseClick[i];
                }
                else
                {
                    pictureBox.BackgroundImage = m_ListImageNormal[i];
                }
            }
        }
        #endregion

        #region Paint事件

        Image m_GroupBackgroundImg = Resources.ResourceManager.GetImage(Resources.ResourceNames.BackGroudImage);
        SolidBrush m_GroupBackgroundBrush = new SolidBrush(Color.FromArgb(241, 248, 255));
        int m_GroupBackgroundImgWidth = 542;
        int m_GroupBackgroundImgHeight = 451;

        private void panelZoom_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighSpeed;
            //将背景图片画上去
            if (m_IsShowRightBottomImage == 1)
            {
                Rectangle right1 = new Rectangle(this.Width - m_GroupBackgroundImgWidth + 1, this.Height - m_GroupBackgroundImgHeight,
                    m_GroupBackgroundImgWidth, m_GroupBackgroundImgHeight);
                e.Graphics.DrawImage(m_GroupBackgroundImg, right1);
            }
        }

        #endregion

        #region 模块点击事件
        /// <summary>
        /// 点击某个小模块
        /// </summary>
        /// <param name="sendr"></param>
        /// <param name="e"></param>
        private void moule_MouldFunClick(object sendr, MouldEventArgs e)
        {
            UCMould mould = sendr as UCMould;
            if ((mould == null) || (mould.Tag == null)) return;
            DrectSoft.FrameWork.IPlugIn plg = m_App.Manager.Runner.LoadPlugIn(e.PlugInfo.AssemblyName, e.PlugInfo.AssemblyStartupClass, true);
            if (plg == null) m_App.CustomMessageBox.MessageShow("加载" + ((PlugInConfiguration)mould.Tag).MenuCaption + "失败！");
        }
        #endregion

        #region 窗体关闭事件
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
        }
        #endregion

        #region IStartPlugIn 成员

        public IPlugIn Run(IEmrHost host)
        {
            if (host == null)
                throw new ArgumentNullException("application");
            m_App = host;
            PlugIn plg = new PlugIn(this.GetType().ToString(), this);
            plg.UserChanged += new UsersChangedHandeler(plg_UserChanged);
            return plg;
        }

        /// <summary>
        /// 切换用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        void plg_UserChanged(object sender, UserArgs arg)
        {
            InitTab();
        }

        #endregion

        private void FormMouldList_SizeChanged(object sender, EventArgs e)
        {
            m_ActualWidth = panelZoom.Width;
            m_ActualHeight = panelZoom.Height;

            //生成groupbox的控制,填充Panel
            SetPanelZoom();

            group.Invalidate();
        }
    }
}
