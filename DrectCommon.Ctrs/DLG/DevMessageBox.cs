using DevExpress.XtraEditors;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml;
namespace DrectSoft.Common.Ctrs.DLG
{

    partial class DevMessageBox : DevExpress.XtraEditors.XtraForm
    {

        #region const
        private const int InitTop = 10;//初始
        private const int InitLeft = 10;
        private const int InitGap = 5;
        private const string s_OkCaption = "确定(&O)";
        private const string s_CancelCaption = "取消(&C)";
        private const string s_YesCaption = "是(&Y)";
        private const string s_NoCaption = "否(&N)";

        private const int s_MinBoxWidth = 250;
        private const int s_MinBoxHeight = 134;
        private const int s_DefaultPad = 3;
        #endregion

        #region fields
        private int m_MaxWidth;
        private int m_MaxHeight;
        private int m_MaxLayoutWidth;
        private int m_MaxLayoutHeight;
        private int m_MinLayoutWidth;
        private int m_MinLayoutHeight;
        private bool m_iconshow = false; //bwj 20121017 为消息居中而增加
        public int m_iMsg = 0;//2013-02-19,WangGuojin
        #endregion

        public string m_sMessage;
        public string m_sSysMessage;
        public void GetErrorMessage(int iMsg)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                //xll 2013-02-26 路径必须用绝对路径
                string flieStr = AppDomain.CurrentDomain.BaseDirectory + "message.xml";
                doc.Load(flieStr);
                foreach (XmlNode node in doc.SelectNodes("messages/message"))
                {
                    int i = int.Parse(node.Attributes["msgid"].Value.ToString().Trim());
                    if (i == iMsg)
                    {
                        m_sMessage = node.Attributes["value"].Value.ToString().Trim();
                        break;
                    }
                }
            }
            catch (System.Xml.XmlException Exception)
            {
                MyMessageBox.Show("Open message.xml file fault:" + Exception.Message.ToString());
            }

        }

        public DevMessageBox()
        {
            InitializeComponent();
            try
            {
                initForm();//初始化



            }
            catch (Exception ce)
            {
                throw ce;
            }
        }



        /// <summary>
        /// 鼠标拖动窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void groupControlContainer_MouseDown(object sender, MouseEventArgs e)
        {
            NativeMethods.ReleaseCapture();
            NativeMethods.SendMessage((IntPtr)this.FindForm().Handle, NativeMethods.WMNclbuttonDown, (IntPtr)NativeMethods.HTCaption, IntPtr.Zero);
        }

        public DevMessageBox(string _msg, string _caption,
            MyMessageBoxButtons _btns,
            MessageBoxIcon _icontype)
        {
            InitializeComponent();
            try
            {
                initForm();//初始化
                this.SetIconInfo(_icontype);
                this.SetMessageInfo(_msg);
                this.SetButtonInfo(_btns);
                this.Text = _caption;
                this.groupControlContainer.Text = this.Text;

            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        public DevMessageBox(string _msg, string _caption,
            MyMessageBoxButtons _btns)
        {
            InitializeComponent();
            try
            {
                initForm();//初始化
                //this.SetIconInfo(_icontype);
                this.SetMessageInfo(_msg);
                this.SetButtonInfo(_btns);
                this.Text = _caption;
                this.groupControlContainer.Text = this.Text;

            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        public DevMessageBox(string _msg, string _caption)
        {
            InitializeComponent();
            try
            {
                initForm();//初始化
                //this.SetIconInfo(_icontype);
                this.SetMessageInfo(_msg);
                this.SetButtonInfo(MyMessageBoxButtons.Ok);
                this.Text = _caption;
                this.groupControlContainer.Text = this.Text;

            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        public DevMessageBox(string _msg)
        {
            InitializeComponent();
            try
            {
                initForm();//初始化
                //this.SetIconInfo(_icontype);
                this.SetMessageInfo(_msg);
                this.SetButtonInfo(MyMessageBoxButtons.Ok);
                this.Text = "提示";
                this.groupControlContainer.Text = this.Text;

            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        //013-02-20,WangGuojin
        public DevMessageBox(string _msg, MessageBoxIcon _icontype)
        {
            InitializeComponent();
            try
            {
                initForm();//初始化
                this.SetIconInfo(_icontype);
                this.SetMessageInfo(_msg);
                this.SetButtonInfo(MyMessageBoxButtons.Ok);
                this.Text = "提示";
                this.groupControlContainer.Text = this.Text;

            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        /// <summary>
        /// 提醒窗体 指定时间后关闭
        /// </summary>
        /// <param name="_msg"></param>
        /// <param name="timerSecond"></param>
        public DevMessageBox(string _msg, int timerSecond)
        {
            InitializeComponent();
            try
            {
                if (timerSecond <= 0)
                {
                    new Exception("弹出时间必须大于0");
                }
                initForm();//初始化
                //this.SetIconInfo(MessageBoxIcon.InformationIcon);
                this.SetMessageInfo(_msg);
                //this.SetButtonInfo(Common.Ctrs.DLG.MyMessageBoxButtons.Ok);
                this.Text = "提示";
                this.groupControlContainer.Text = this.Text;
                Timer timer = new Timer();
                timer.Interval = timerSecond * 1000;
                timer.Tick += new EventHandler(timer_Tick);
                timer.Start();

                panelButtonInfo.Visible = false;
                this.Height = this.Height - 56; //删除消息框中按钮高度
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                (sender as Timer).Stop();
                this.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void DevMessageBox_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void initForm()
        {
            try
            {
                this.Text = "提示";
                this.KeyDown += new KeyEventHandler(MessageBox_KeyDown);

                this.Icon = DrectSoft.Common.Ctrs.Properties.Resources.logo_20;
                this.KeyPreview = true;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        /// <summary>
        /// 按Ctrl+C复制ErrMsg至剪粘板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Control && e.KeyCode == Keys.C)
                {
                    Clipboard.SetText(labelMessage.Text);
                }
            }
            catch (Exception ce)
            {
                MyMessageBox.Show(ce.Message.ToString(), "提示");
            }
        }
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="messages"></param>
        public void WriteMessageLog(string messages)
        {
            string LogName = "Emr_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            ///指定日志文件的目录
            string fname = Directory.GetCurrentDirectory() + "\\log\\" + LogName;

            ///定义文件信息对象
            FileInfo finfo = new FileInfo(fname);

            if (!finfo.Exists)
            {
                FileStream fs;
                fs = File.Create(fname);
                fs.Close();
                finfo = new FileInfo(fname);
            }
            ///判断文件是否存在以及是否大于2K
            if (finfo.Length > 1024 * 1024 * 10)
            {
                ///文件超过10MB则重命名
                File.Move(fname, fname + DateTime.Now.Hour.ToString());
            }

            ///创建只写文件流

            using (FileStream fs = finfo.OpenWrite())
            {

                ///根据上面创建的文件流创建写数据流
                StreamWriter w = new StreamWriter(fs);

                ///设置写数据流的起始位置为文件流的末尾
                w.BaseStream.Seek(0, SeekOrigin.End);

                ///写入“Log Entry : ”
                w.Write("\n\rLog Entry : ");


                ///写入当前系统时间并换行
                w.Write("{0} {1} \n\r", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());

                ///写入日志内容并换行
                w.Write(messages + "\n\r");

                ///写入------------------------------------“并换行
                w.Write("------------------------------------\n\r");

                ///清空缓冲区内容，并把缓冲区内容写入基础流
                w.Flush();

                ///关闭写数据流
                w.Close();
            }
        }
        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="messages"></param>
        public void SetMessageInfo(string messages)
        {
            try
            {
                if (messages == null)
                {
                    labelMessage.Text = String.Empty;
                }
                else
                {

                    labelMessage.Text = messages;
                    WriteMessageLog(messages);
                }

                // 计算消息框的合适尺寸
                ResetBoxMaxSize();

                ResetMessageSizeRange();

                Font messageFont = labelMessage.Font; //new Font("SimSun", 11);
                Graphics g = Graphics.FromHwnd(panelMessageInfo.Handle);
                SizeF stringSize = g.MeasureString(labelMessage.Text, messageFont, m_MaxLayoutWidth);

                ResetBoxSize(stringSize);
                labelMessage.Text = labelMessage.Text.Trim();
                // 只有一行时居中显示
                SizeF singleSize = g.MeasureString("中", messageFont);

                labelMessage.TextAlign = ContentAlignment.MiddleLeft;

                int ileft = 10;


                if (!m_iconshow)
                {
                    ileft = (int)(labelMessage.Width - stringSize.Width) / 2;

                }



                labelMessage.Padding = new Padding(ileft, 5,
                   0, 5);
                this.groupControlContainer.Width = this.Width;
                this.groupControlContainer.Height = this.Height;
                this.groupControlContainer.Refresh();

            }
            catch (Exception ce)
            {
                throw ce;
            }

        }

        private void ResetBoxSize(SizeF stringSize)
        {
            try
            {
                int newHeight = EnsureSizeInRange((int)stringSize.Height, m_MinLayoutHeight, m_MaxLayoutHeight);
                int newWidth = EnsureSizeInRange((int)stringSize.Width + 30, m_MinLayoutWidth, m_MaxLayoutWidth);

                this.Height = newHeight + panelButtonInfo.Height + s_DefaultPad + 23;
                if (m_iconshow)
                {
                    this.Width = newWidth + panelIconInfo.Width + s_DefaultPad;
                }
                else
                {
                    this.Width = newWidth + s_DefaultPad;
                }
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        private int EnsureSizeInRange(int value, int minValue, int maxValue)
        {
            try
            {
                if (value < minValue)
                {
                    return minValue;
                }
                else if (value > maxValue)
                {
                    return maxValue;
                }
                else
                {
                    return value;
                }
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        /// <summary>
        /// 设置消息部分的最大、最小尺寸
        /// </summary>
        private void ResetMessageSizeRange()
        {
            try
            {
                // 高度 = 对话框高度 - 按钮区域高度 - 预留边距
                // 宽度 = 对话框宽度 - icon区域宽度 - 预留边距

                int heightMargin = panelButtonInfo.Height + s_DefaultPad;
                int widthMargin = 0;
                if (m_iconshow)
                {
                    widthMargin = panelIconInfo.Width + s_DefaultPad;
                }
                else
                {
                    widthMargin = s_DefaultPad;
                }
                // 最小尺寸
                m_MinLayoutHeight = s_MinBoxHeight - heightMargin;
                m_MinLayoutWidth = s_MinBoxWidth - widthMargin;
                // 最大的尺寸
                m_MaxLayoutHeight = m_MaxHeight - heightMargin;
                m_MaxLayoutWidth = m_MaxWidth - widthMargin;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        /// <summary>
        /// 每次弹出时获取消息窗口最大可显示的尺寸(和当前屏幕有关)
        /// </summary>
        private void ResetBoxMaxSize()
        {
            try
            {
                Rectangle rect;
                if (Parent == null)
                {
                    rect = SystemInformation.WorkingArea;
                }
                else
                {
                    rect = Screen.GetWorkingArea(this.Parent);
                }
                m_MaxHeight = (int)(rect.Height * 0.90);
                m_MaxWidth = (int)(rect.Width * 0.60);
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }


        #region set ButtonInfo
        private void DrawButtonInfo(Collection<SimpleButton> controls)
        {
            try
            {
                this.SuspendLayout();
                panelButtonInfo.Controls.Clear();

                if (controls == null) return;

                int controlCount = controls.Count;
                if (controlCount < 0) return;

                int allControlWidth = 0;
                int addInitLeft = 0;
                for (int i = 0; i < controlCount; i++)
                {
                    allControlWidth += controls[i].Width;
                }
                allControlWidth += (controlCount - 1) * InitGap;
                if (allControlWidth < panelButtonInfo.Width - 2 * InitLeft)
                {
                    addInitLeft = (panelButtonInfo.Width - 2 * InitLeft - allControlWidth) / 2;
                }

                int controlLeft = InitLeft + addInitLeft;
                for (int i = 0; i < controlCount; i++)
                {
                    controls[i].AutoSize = true;
                    panelButtonInfo.Controls.Add(controls[i]);
                    controls[i].Top = InitTop;
                    controls[i].Left = controlLeft;
                    controlLeft = controlLeft + controls[i].Width + InitGap;
                }

                this.ResumeLayout();
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        public void SetButtonInfo(MyMessageBoxButtons kind)
        {
            try
            {
                Collection<SimpleButton> buttonInfoControls = new Collection<SimpleButton>();
                switch (kind)
                {
                    case MyMessageBoxButtons.Ok:
                        buttonInfoControls.Add(CreateButton(s_OkCaption, DialogResult.OK));
                        break;
                    case MyMessageBoxButtons.OkCancel:
                        buttonInfoControls.Add(CreateButton(s_OkCaption, DialogResult.OK));
                        buttonInfoControls.Add(CreateButton(s_CancelCaption, DialogResult.Cancel));
                        break;
                    case MyMessageBoxButtons.YesNo:
                        buttonInfoControls.Add(CreateButton(s_YesCaption, DialogResult.Yes));
                        buttonInfoControls.Add(CreateButton(s_NoCaption, DialogResult.No));
                        break;
                    case MyMessageBoxButtons.YesNoCancel:
                        buttonInfoControls.Add(CreateButton(s_YesCaption, DialogResult.Yes));
                        buttonInfoControls.Add(CreateButton(s_NoCaption, DialogResult.No, false));
                        buttonInfoControls.Add(CreateButton(s_CancelCaption, DialogResult.Cancel));
                        break;
                    case MyMessageBoxButtons.Yes:
                        buttonInfoControls.Add(CreateButton(s_YesCaption, DialogResult.Yes));
                        break;
                    default:
                        break;
                }
                DrawButtonInfo(buttonInfoControls);
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        private SimpleButton CreateButton(string text, DialogResult dialogResult)
        {
            try
            {
                return CreateButton(text, dialogResult, true);
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        private SimpleButton CreateButton(string text, DialogResult dialogResult, bool setAcceptCancelButton)
        {
            try
            {
                SimpleButton btn = new SimpleButton();
                btn.Text = text;
                btn.DialogResult = dialogResult;
                if (setAcceptCancelButton)
                {
                    if (dialogResult == DialogResult.Yes || dialogResult == DialogResult.OK)
                        this.AcceptButton = btn;
                    if (dialogResult == DialogResult.No || dialogResult == DialogResult.Cancel)
                        this.CancelButton = btn;
                }
                return btn;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        #endregion

        #region set icon info
        private void SetIconInfo(MessageBoxIcon iconType)
        {
            try
            {
                Image icon = null;
                switch (iconType)
                {
                    case MessageBoxIcon.ErrorIcon:

                        icon = DrectSoft.Common.Ctrs.Properties.Resources.MessageBoxError;
                        break;
                    case MessageBoxIcon.InformationIcon:

                        icon = DrectSoft.Common.Ctrs.Properties.Resources.MessageBoxMessage;
                        break;
                    case MessageBoxIcon.QuestionIcon:

                        icon = DrectSoft.Common.Ctrs.Properties.Resources.MessageBoxPrompt;
                        break;
                    case DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon:

                        icon = DrectSoft.Common.Ctrs.Properties.Resources.MessageBoxWarning;
                        break;
                    default:
                        break;
                }
                if (icon == null) return;
                Bitmap bmp = new Bitmap(icon);
                if (bmp != null)
                {
                    bmp.MakeTransparent(Color.Magenta);

                    pictureBoxIcon.BackgroundImage = bmp;
                    pictureBoxIcon.BackgroundImageLayout = ImageLayout.Stretch;
                }
                else
                {
                    pictureBoxIcon.Image = icon;

                }
                this.panelIconInfo.Visible = true;
                m_iconshow = true; //bwj 20121017 为消息居中而增加
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }


        #endregion
        //2013-02-19,WangGuojin,add,read messageinfo from xml file and show it.
        private void pictureBoxIcon_Click(object sender, EventArgs e)
        {
            //this.labelSubMessage.Text = this.m_sSysMessage;
            if (m_sSysMessage != null && m_sSysMessage != "")
            {
                DevSysMessageBox box = new DevSysMessageBox(this.m_sSysMessage);
                box.TopMost = true;
                box.ShowDialog();
            }
            //MessageBox.Show("System Error Message:" + this.m_sSysMessage);
        }
    }

    public abstract class NativeMethods
    {

        /// <summary>
        /// CallBack
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public delegate bool CallBack(int hwnd, int lParam);

        /// <summary>
        /// SetWindowLong
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="index"></param>
        /// <param name="dwNew"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetWindowLong(IntPtr hwnd, int index, int dwNew);

        /// <summary>
        /// GetWindowLong
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowLong(IntPtr hwnd, int index);

        /// <summary>
        /// SendMessage
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        [CLSCompliant(false)]
        public static extern IntPtr SendMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// ReleaseCapture
        /// </summary>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ReleaseCapture();

        /// <summary>
        /// GetSystemMenu
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="bRevert"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetSystemMenu(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)]bool bRevert);

        /// <summary>
        /// TrackPopupMenu
        /// </summary>
        /// <param name="hmenu"></param>
        /// <param name="uFlags"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="nReserved"></param>
        /// <param name="hwnd"></param>
        /// <param name="prcRect"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        [CLSCompliant(false)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool TrackPopupMenu(IntPtr hmenu, uint uFlags, int x, int y, int nReserved, IntPtr hwnd, IntPtr prcRect);

        /// <summary>
        /// EnumWindows
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumWindows(CallBack x, IntPtr y);

        /// <summary>
        /// ShowWindow
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="nCmdShow"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int ShowWindow(int hwnd, int nCmdShow);

        /// <summary>
        /// SetActiveWindow
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetActiveWindow(int hwnd);

        /// <summary>
        /// GetLastActivePopup
        /// </summary>
        /// <param name="hwndOwnder"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetLastActivePopup(int hwndOwnder);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetForegroundWindow(int hwnd);
        /// <summary>
        /// GWL_STYLE
        /// </summary>
        public const int GwlStyle = -16;

        /// <summary>
        /// WM_NCCREATE
        /// </summary>
        public const int WMNccreate = 0x0081;

        /// <summary>
        /// WM_NCACTIVATE
        /// </summary>
        public const int WMNcactivate = 0x0086;

        /// <summary>
        /// WS_DLGFRAME
        /// </summary>
        public const int WSDlgFrame = 0x400000;

        /// <summary>
        /// WM_CREATE
        /// </summary>
        public const int WMCreate = 0x0001;

        /// <summary>
        /// WM_PAINT
        /// </summary>
        public const int WMPaint = 0x000F;

        /// <summary>
        /// WM_SHOWWINDOW
        /// </summary>
        public const int WMShowWindow = 0x0018;

        /// <summary>
        /// HTCAPTION
        /// </summary>
        public const int HTCaption = 2;

        /// <summary>
        /// WM_NCLBUTTONDOWN
        /// </summary>
        public const int WMNclbuttonDown = 0x00A1;

        /// <summary>
        /// WS_SIZEBOX
        /// </summary>
        public const int WSSizeBox = 0x40000;

        /// <summary>
        /// WS_CAPTION
        /// </summary>
        public const int WSCaption = 0xC00000;

        /// <summary>
        /// WS_SYSMENU
        /// </summary>
        public const int WSSysmenu = 0x80000;

        /// <summary>
        /// WS_MINIMIZEBOX
        /// </summary>
        public const int WSMinimizeBox = 0x20000;

        /// <summary>
        /// WS_VISIBLE
        /// </summary>
        public const int WSVisible = 0x10000000;

        /// <summary>
        /// SWMINIMIZE
        /// </summary>
        public const int SWMinimize = 6;

        /// <summary>
        /// SWMaximize
        /// </summary>
        public const int SWMaximize = 3;

    }


    /// <summary>
    /// 自定义消息提示框的类型
    /// </summary>
    public enum MyMessageBoxButtons
    {
        /// <summary>
        /// Default value
        /// </summary>
        None = 0,

        /// <summary>
        ///  ok button
        /// </summary>
        Ok = 1,

        /// <summary>
        /// ok + cancel button
        /// </summary>
        OkCancel = 2,

        /// <summary>
        ///  yes + no button
        /// </summary>
        YesNo = 3,

        /// <summary>
        ///  yes + no + cancel button
        /// </summary>
        YesNoCancel = 4,
        /// <summary>
        ///  yes button
        /// </summary>
        Yes = 5
    }
    /// <summary>
    /// 自定义消息提示框的图标
    /// </summary>

    public enum MessageBoxIcon
    {
        None = 0,
        WarningIcon = 1,// ! icon
        ErrorIcon = 2,// x icon
        QuestionIcon = 3,// ? icon
        InformationIcon = 4// i icon
    }


    public class MyMessageBox
    {

        public MyMessageBox()
        {
            //
        }

        public string GetMessageConfig(int iFlag)
        {
            XmlDocument doc = new XmlDocument();
            string info = "";
            try
            {
                //xll 2013-02-26
                string flieStr = AppDomain.CurrentDomain.BaseDirectory + "message.xml";
                doc.Load(flieStr);
                foreach (XmlNode node in doc.SelectNodes("messages/message"))
                {
                    int i = int.Parse(node.Attributes["msgid"].Value.ToString().Trim());
                    if (i == iFlag)
                    {
                        info = node.Attributes["value"].Value.ToString().Trim();
                        break;
                    }
                }
            }
            catch (System.Xml.XmlException Exception)
            {
                info = "System Error: can not open message.xml file!";
            }
            return info;
        }
        public static DialogResult Show(string msg)
        {
            try
            {
                DevMessageBox aDevMessageBox = new DevMessageBox(msg);
                aDevMessageBox.WriteMessageLog(msg);
                DialogResult dr = aDevMessageBox.ShowDialog();

                return dr;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        //2013-02-19,WangGuojin
        public static DialogResult Show(int msgID, string exmsg)
        {
            try
            {
                MyMessageBox yBox = new MyMessageBox();
                string msg = yBox.GetMessageConfig(msgID);
                MessageBoxIcon icontype = MessageBoxIcon.ErrorIcon;
                DevMessageBox aDevMessageBox = new DevMessageBox(msg, icontype);
                aDevMessageBox.WriteMessageLog(exmsg);
                aDevMessageBox.m_sSysMessage = exmsg;
                DialogResult dr = aDevMessageBox.ShowDialog();
                return dr;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        //2013-02-19,WangGuojin
        //xll 1EMR 2HIS 3Pacs 4lis
        public static DialogResult Show(int msgID, Exception ex)
        {
            try
            {
                MyMessageBox yBox = new MyMessageBox();
                string msg = yBox.GetMessageConfig(msgID);
                MessageBoxIcon icontype = MessageBoxIcon.ErrorIcon;
                DevMessageBox aDevMessageBox = new DevMessageBox(msg, icontype);
                aDevMessageBox.WriteMessageLog(ex.Message);
                aDevMessageBox.m_sSysMessage = ex.Message + ex.StackTrace;
                DialogResult dr = aDevMessageBox.ShowDialog();
                return dr;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        public static DialogResult Show(string msg, string caption)
        {
            try
            {
                DevMessageBox aDevMessageBox = new DevMessageBox(msg, caption);
                aDevMessageBox.WriteMessageLog(msg);

                DialogResult dr = aDevMessageBox.ShowDialog();

                return dr;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        public static DialogResult Show(string msg, string caption, MyMessageBoxButtons btns)
        {
            try
            {
                DevMessageBox aDevMessageBox = new DevMessageBox(msg, caption, btns);

                DialogResult dr = aDevMessageBox.ShowDialog();

                return dr;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }


        public static DialogResult Show(string msg, string caption,
            MyMessageBoxButtons btns,
            MessageBoxIcon icontype)
        {
            try
            {
                DevMessageBox aDevMessageBox = new DevMessageBox(msg, caption, btns, icontype);

                DialogResult dr = aDevMessageBox.ShowDialog();

                return dr;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        /// <summary>
        /// xll 静态方法 弹出框 指定时间后关闭
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="timerSecond"></param>
        /// <returns></returns>
        public static DialogResult Show(string msg, int timerSecond)
        {
            try
            {
                DevMessageBox aDevMessageBox = new DevMessageBox(msg, timerSecond);


                DialogResult dr = aDevMessageBox.ShowDialog();

                return dr;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
    }
}