using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Threading;
using System.Collections;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Core;

namespace DrectSoft.MainFrame
{
    public partial class UCMessageWindow : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 消息窗体的父窗体
        /// </summary>
        FormMain m_ParentMainForm;
        /// <summary>
        /// 窗体状态
        /// </summary>
        MessageWindowState m_WindowState = MessageWindowState.Bottom;
        public MessageWindowState WindowState
        {
            get
            {
                return m_WindowState;
            }
        }

        /// <summary>
        /// 等待多少毫秒后下移窗体
        /// </summary>
        readonly int m_WaitTime = 200;
        /// <summary>
        /// 已经等待的毫秒数
        /// </summary>
        int m_HaveWaiteTime = 0;
        /// <summary>
        /// 表示鼠标是否悬停在提示窗体上
        /// </summary>
        bool m_IsHover = false;
        /// <summary>
        /// 是否点击关闭按钮
        /// </summary>
        bool m_IsClickCloseButton = false;
        /// <summary>
        /// 偏移量
        /// </summary>
        int m_OffsetHeight = 20;

        /// <summary>
        /// 列表中的数据源
        /// </summary>
        DataTable m_DataSource = new DataTable();

        readonly string r_ID = "ID";//序号ID
        readonly string r_GCTypeID = "TYPEID";//消息类型ID
        readonly string r_GCTypeName = "TYPENAME";//消息类型名称
        readonly string r_GCContent = "CONTENT";//消息内容
        readonly string r_GCFlag = "FLAG";//消息表示 用于判断是否新消息的栏位

        readonly string r_OLD = "OLD";
        readonly string r_NEW = "NEW";

        StringFormat m_sf = new StringFormat();

        bool? isDownToBottomImmediate = null;//是否直接下移到最低部
        bool? IsDownToBottomImmediate
        {
            get
            {
                if (isDownToBottomImmediate == null)
                {
                    isDownToBottomImmediate = false;
                    string openMessageWindowConfig = new AppConfigReader().GetConfig("IsOpenMessageWindow").Config;
                    if (openMessageWindowConfig.Split(',').Length >= 4)
                    {
                        if (openMessageWindowConfig.Split(',')[3].ToString() == "1")
                        {
                            isDownToBottomImmediate = true;
                        }
                    }
                    return isDownToBottomImmediate;
                }
                else
                {
                    return isDownToBottomImmediate; 
                }
            }
        }

        #region .ctor
        private UCMessageWindow()
        {
            InitializeComponent();
        }

        public UCMessageWindow(FormMain formMain, string offsetHeight, string waitTime) : this()
        {
            try
            {
                m_ParentMainForm = formMain;
                m_OffsetHeight = Convert.ToInt32(offsetHeight.Trim());
                m_WaitTime = Convert.ToInt32(waitTime.Trim());
                InitVariable();
                InitDataSource();
                InitMessageWindow(formMain);
                timerMoving.Start();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 初始化GridControl中数据源的结构
        /// </summary>
        private void InitDataSource()
        {
            try
            {
                if (m_DataSource.Columns.Count == 0)
                {
                    m_DataSource.Columns.Add(r_ID);
                    m_DataSource.Columns.Add(r_GCTypeID);
                    m_DataSource.Columns.Add(r_GCTypeName);
                    m_DataSource.Columns.Add(r_GCContent);
                    m_DataSource.Columns.Add(r_GCFlag);
                    gridControlMessage.DataSource = m_DataSource;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化变量
        /// </summary>
        private void InitVariable()
        {
            m_sf.Alignment = StringAlignment.Center;
            m_sf.LineAlignment = StringAlignment.Center;
        }

        /// <summary>
        /// 初始化消息窗体的位置
        /// </summary>
        /// <param name="parentForm"></param>
        private void InitMessageWindow(Form parentForm)
        {
            try
            {
                if (!parentForm.Controls.Contains(this))
                {
                    parentForm.Controls.Add(this);

                    //右侧
                    //Point p = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width - 8
                    //    , Screen.PrimaryScreen.WorkingArea.Height + m_OffsetHeight);

                    //左侧
                    Point p = new Point(1, Screen.PrimaryScreen.WorkingArea.Height + m_OffsetHeight);

                    //this.PointToScreen(p);
                    this.Location = p;
                    this.BringToFront();
                    m_WindowState = MessageWindowState.Bottom;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Paint
        private void groupControlMessageForm_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                e.Graphics.DrawString("欢迎进入志扬电子病历系统!", this.Font, new SolidBrush(Color.Black), new RectangleF(0f, 20f, this.Width, this.Height - 20f), m_sf);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #endregion

        #region 显示消息窗体

        delegate void ShowMessageWindowDelegate(Form parentForm, DataTable dt, bool isClear);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentForm"></param>
        /// <param name="dt"></param>
        /// <param name="isClear"></param>
        public void ShowMessageWindow(Form parentForm, DataTable dt, bool isClear)
        {
            try
            {
                if (gridControlMessage.InvokeRequired)
                {
                    //(new ShowMessageWindowDelegate(ShowMessageWindowInner)).Invoke(parentForm, dt, isClear);
                    gridControlMessage.Invoke(new ShowMessageWindowDelegate(ShowMessageWindowInner), parentForm, dt, isClear);
                }
                else
                {
                    ShowMessageWindowInner(parentForm, dt, isClear);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 显示消息窗体
        /// </summary>
        /// <param name="parentForm"></param>
        /// <param name="dt"></param>
        /// <param name="isClear"></param>
        private void ShowMessageWindowInner(Form parentForm, DataTable dt, bool isClear)
        {
            try
            {
                if (isClear)
                {
                    m_DataSource.Clear();
                }
                else
                {
                    gridControlMessage.Visible = true;
                }

                if (dt == null || dt.Rows.Count == 0) return;

                EnsureHasSpecifyCol(dt);
                foreach (DataRow dr in m_DataSource.Rows)
                {
                    dr[r_GCFlag] = r_OLD;
                }

                //新数据插入到数据源中
                InitDataSource();
                foreach (DataRow dr in dt.Rows)
                {
                    dr[r_GCFlag] = r_NEW;
                    DataRow row = m_DataSource.NewRow();
                    row.ItemArray = dr.ItemArray;
                    m_DataSource.Rows.Add(row);
                }
                gridViewMessage.MoveLast();
                m_WindowState = MessageWindowState.MovingUp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 验证DataTable中是否包含FLAG标志位
        /// </summary>
        /// <param name="dt"></param>
        void EnsureHasSpecifyCol(DataTable dt)
        {
            try
            {
                if (!dt.Columns.Contains(r_GCFlag))
                {
                    dt.Columns.Add(r_GCFlag);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 关闭窗体
        private void pictureBoxExit_Click(object sender, EventArgs e)
        {
            try
            {
                m_WindowState = MessageWindowState.MovingDown;
                m_IsClickCloseButton = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 鼠标的移进和移出  在鼠标移进时消息窗体不会消失，鼠标移出消息窗体的一段时间后消息窗体会消失
        private void groupControlMessageForm_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                m_IsHover = true;
                if (!m_IsClickCloseButton)
                {
                    m_WindowState = MessageWindowState.MovingUp;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void groupControlMessageForm_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                m_IsHover = false;
                m_HaveWaiteTime = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 定时器中移动消息窗体
        private void timerMoving_Tick(object sender, EventArgs e)
        {
            try
            {
                this.BringToFront();

                if (m_WindowState == MessageWindowState.MovingUp)//上移
                {
                    #region 上移
                    m_HaveWaiteTime = 0;
                    if (this.Location.Y > Screen.PrimaryScreen.WorkingArea.Height - this.Height - 20)
                    {
                        Point p = this.Location;
                        this.Location = new Point(p.X, p.Y - 3);
                    }
                    else
                    {
                        m_WindowState = MessageWindowState.Top;
                    }
                    #endregion
                }
                else if (m_WindowState == MessageWindowState.MovingDown)//下移
                {
                    #region 下移
                    m_HaveWaiteTime = 0;

                    if (IsDownToBottomImmediate.Value)
                    {
                        this.Location = new Point(this.Location.X, Screen.PrimaryScreen.WorkingArea.Height + m_OffsetHeight - 28);
                        m_WindowState = MessageWindowState.Bottom;
                        m_IsClickCloseButton = false;
                    }
                    else
                    {
                        if (this.Location.Y < Screen.PrimaryScreen.WorkingArea.Height + m_OffsetHeight - 28)
                        {
                            if (!m_IsHover || m_IsClickCloseButton)
                            {
                                Point p = this.Location;
                                this.Location = new Point(p.X, p.Y + 3);
                            }
                        }
                        else
                        {
                            m_WindowState = MessageWindowState.Bottom;
                            m_IsClickCloseButton = false;
                        }
                    }
                    #endregion
                }
                else if (m_WindowState == MessageWindowState.Top)//达到顶点
                {
                    #region 达到顶点
                    if (m_HaveWaiteTime >= m_WaitTime)
                    {
                        m_WindowState = MessageWindowState.MovingDown;
                    }
                    else
                    {
                        if (!m_IsHover)
                        {
                            m_HaveWaiteTime++;
                        }
                        else
                        {
                            m_HaveWaiteTime = 0;
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 区分新数据和旧数据的颜色
        private void gridViewMessage_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                DataRowView drv = gridViewMessage.GetRow(e.RowHandle) as DataRowView;
                if (drv != null)
                {
                    if (drv[r_GCFlag].ToString() == r_OLD)
                    {
                        e.Appearance.BackColor = Color.White;
                        e.Appearance.ForeColor = Color.Gray;
                    }
                    else if (drv[r_GCFlag].ToString() == r_NEW)
                    {
                        e.Appearance.BackColor = Color.Blue;
                        e.Appearance.ForeColor = Color.Yellow;
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        #endregion
    }

    public enum MessageWindowState
    {
        /// <summary>
        /// 停止在底部
        /// </summary>
        Bottom,
        /// <summary>
        /// 正在上移
        /// </summary>
        MovingUp,
        /// <summary>
        /// 停止在顶部
        /// </summary>
        Top,
        /// <summary>
        /// 正在下移
        /// </summary>
        MovingDown
    }
}
