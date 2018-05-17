using DrectSoft.FrameWork.Plugin.Manager;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace DrectSoft.Core.MouldList
{
    public partial class UCMould : UserControl
    {
        private PlugInConfiguration m_PlugInfo;

        /// <summary>
        /// 正常尺寸的图片
        /// </summary>
        private Image m_NormalImage;

        /// <summary>
        /// 大尺寸的图片
        /// </summary>
        private Image m_LargeImage;

        /// <summary>
        /// 鼠标移入标志位
        /// </summary>
        private bool m_IsMoveIn = false;

        /// <summary>
        /// 设置通过单击或双击进入对应的功能模块 1:单击 2:双击
        /// </summary>
        public int EnterSingleOrDoubleClick { get; set; }

        public UCMould(PlugInConfiguration info)
        {
            //双缓冲
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            InitializeComponent();
            m_PlugInfo = info;
            this.Tag = info;
        }

        private void UCMould_Load(object sender, EventArgs e)
        {
            m_IsOK = false;
            this.labModuleName.Text = m_PlugInfo.MenuCaption;
            if (m_PlugInfo.Icon != null)
            {
                this.pictureEdit1.Image = (Image)m_PlugInfo.Icon.Clone();//修改登录时，对象正在使用中错误 2012年5月8日16:03:42
            }
            InitForm();
        }

        private bool m_IsOK = false;

        private void InitForm()
        {
            if (this.pictureEdit1.Image != null)
            {
                SetImage(this.pictureEdit1.Image);
            }

            this.labModuleName.Font = new Font(labModuleName.Font.FontFamily, this.labModuleName.Font.Size, FontStyle.Bold);
            this.BackColor = Color.Transparent;
            this.DoubleBuffered = true;
            InitToolTipController();
            this.MouseLeave += new EventHandler(UCMould_MouseLeave);
            this.Paint += new PaintEventHandler(UCMould_Paint);
            m_IsOK = true;
        }

        /// <summary>
        /// 初始化ToolTip
        /// </summary>
        private void InitToolTipController()
        {
            this.toolTipController1.Rounded = true;
            this.toolTipController1.RoundRadius = 5;
            this.toolTipController1.ToolTipType = DevExpress.Utils.ToolTipType.Standard;
            this.toolTipController1.ToolTipLocation = DevExpress.Utils.ToolTipLocation.RightCenter;
            this.toolTipController1.InitialDelay = 500;
            this.toolTipController1.AutoPopDelay = 4000;
            if (EnterSingleOrDoubleClick == 2)
            {
                this.toolTipController1.SetToolTip(this, m_PlugInfo.MenuCaption + "  双击进入");
            }
            else if (EnterSingleOrDoubleClick == 1)
            {
                this.toolTipController1.SetToolTip(this, m_PlugInfo.MenuCaption + "  单击进入");
            }
        }

        #region events
        public delegate void MouldFunClickEventHandler(object sendr, MouldEventArgs e);

        public event MouldFunClickEventHandler MouldFunClick;

        protected virtual void OnMouldFunClick(MouldEventArgs e)
        {
            if (MouldFunClick != null)
            {
                MouldFunClick(this, e);
            }
        }

        private void labModuleName_DoubleClick(object sender, EventArgs e)
        {
            //OnMouldFunClick(new MouldEventArgs(m_PlugInfo));
        }

        private void pictureEdit1_DoubleClick(object sender, EventArgs e)
        {
            //OnMouldFunClick(new MouldEventArgs(m_PlugInfo));
        }

        private void UCMould_DoubleClick(object sender, EventArgs e)
        {
            if (EnterSingleOrDoubleClick == 2)//双击进入文书录入 Add by wwj 2013-05-27
            {
                OnMouldFunClick(new MouldEventArgs(m_PlugInfo));
            }
        }

        void UCMould_Click(object sender, System.EventArgs e)
        {
            if (EnterSingleOrDoubleClick == 1)//单击进入文书录入 Add by wwj 2013-05-27
            {
                OnMouldFunClick(new MouldEventArgs(m_PlugInfo));
            }
        }
        #endregion

        void UCMould_Paint(object sender, PaintEventArgs e)
        {
            if (m_IsOK)
            {
                e.Graphics.SmoothingMode = SmoothingMode.HighSpeed;
                PaintForm(e.Graphics);
            }
        }

        SolidBrush m_MyBrush = new SolidBrush(Color.FromArgb(213, 230, 249));
        private void PaintForm(Graphics g)
        {
            if (m_IsMoveIn == false)//正常情况，鼠标移出
            {
                int x = (pictureEdit1.Width - m_NormalImage.Width) / 2;
                int y = (pictureEdit1.Height - m_NormalImage.Height) / 2;

                //绘制模块图片
                g.DrawImage(m_NormalImage, pictureEdit1.Location.X + x, pictureEdit1.Location.Y + y,
                    m_NormalImage.Width, m_NormalImage.Height);

                TextRenderer.DrawText(g, this.labModuleName.Text, new Font("宋体", 14f, FontStyle.Regular), new Point(this.labModuleName.Location.X + 10, this.labModuleName.Location.Y), Color.Blue);
            }
            else//鼠标移入
            {
                int x = (pictureEdit1.Width - m_LargeImage.Width) / 2;
                int y = (pictureEdit1.Height - m_LargeImage.Height) / 2;
                g.FillPath(m_MyBrush, CreateRoundedRectanglePath(new Rectangle(2, 3, this.Width - 12, this.Height - 2), 10));
                //绘制模块图片
                g.DrawImage(m_LargeImage, pictureEdit1.Location.X + x, pictureEdit1.Location.Y + y);
                TextRenderer.DrawText(g, this.labModuleName.Text, new Font("宋体", 14f, FontStyle.Regular), new Point(this.labModuleName.Location.X + 10, this.labModuleName.Location.Y), Color.Blue);
            }
        }

        /// <summary>
        /// 圆角矩形全部
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="cornerRadius"></param>
        /// <returns></returns>
        private GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int cornerRadius)
        {
            GraphicsPath roundedRect = new GraphicsPath();

            roundedRect.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
            //右上角
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
            //右下角
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
            //左下角
            roundedRect.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
            roundedRect.CloseFigure();
            return roundedRect;
        }

        ///// <summary>
        ///// 圆角矩形的上半部
        ///// </summary>
        ///// <param name="rect"></param>
        ///// <param name="cornerRadius"></param>
        ///// <returns></returns>
        //private GraphicsPath CreateRoundedRectanglePathUp(Rectangle rect, int cornerRadius)
        //{
        //    GraphicsPath roundedRect = new GraphicsPath();
        //    roundedRect.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
        //    roundedRect.AddLine(rect.X + cornerRadius, rect.Y, rect.Right - cornerRadius * 2, rect.Y);
        //    roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
        //    roundedRect.AddLine(rect.Right, rect.Y + cornerRadius * 2, rect.Right, (rect.Y + rect.Height - cornerRadius * 2) / 2);
        //    roundedRect.AddLine(rect.Right, (rect.Y + rect.Height - cornerRadius * 2) / 2,
        //        rect.X, (rect.Y + rect.Height - cornerRadius * 2) / 2);
        //    roundedRect.CloseFigure();
        //    return roundedRect;
        //}

        ///// <summary>
        ///// 圆角矩形的下半部
        ///// </summary>
        ///// <param name="rect"></param>
        ///// <param name="cornerRadius"></param>
        ///// <returns></returns>
        //private GraphicsPath CreateRoundedRectanglePathDown(Rectangle rect, int cornerRadius)
        //{
        //    GraphicsPath roundedRect = new GraphicsPath();
        //    roundedRect.AddLine(rect.Right, rect.Y + cornerRadius * 2 + (rect.Y + rect.Height - cornerRadius * 2) / 2,
        //        rect.Right, rect.Y + rect.Height - cornerRadius * 2);
        //    roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
        //    roundedRect.AddLine(rect.Right - cornerRadius * 2, rect.Bottom, rect.X + cornerRadius * 2, rect.Bottom);
        //    roundedRect.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
        //    roundedRect.AddLine(rect.X, rect.Bottom - cornerRadius * 2,
        //        rect.X, rect.Y + cornerRadius * 2 + (rect.Y + rect.Height - cornerRadius * 2) / 2);
        //    roundedRect.CloseFigure();
        //    return roundedRect;
        //}

        #region 设置tooltip

        //鼠标移入
        private void UCMould_MouseEnter(object sender, EventArgs e)
        {
            m_IsMoveIn = true;
            this.Cursor = Cursors.Hand;
            this.Refresh();
        }

        //鼠标移出
        void UCMould_MouseLeave(object sender, EventArgs e)
        {
            m_IsMoveIn = false;
            this.Cursor = Cursors.Arrow;
            this.Refresh();
        }

        private void pictureEdit1_MouseEnter(object sender, EventArgs e)
        {
        }

        private void labModuleName_MouseEnter(object sender, EventArgs e)
        {
        }
        #endregion

        /// <summary>
        /// 设置图片
        /// </summary>
        /// <param name="image"></param>
        private void SetImage(Image image)
        {
            m_NormalImage = image;
            m_LargeImage = ChangePicture(image);

        }

        /// <summary>
        /// 改变图片大小
        /// </summary>
        private Image ChangePicture(Image image)
        {
            if (image != null)
            {
                Bitmap myBitmap = new Bitmap(image);

                int width, high;
                width = myBitmap.Width;
                high = myBitmap.Width;
                //原来放大1.2倍 2012年2月26日17:33:38
                Bitmap savepic = new Bitmap(myBitmap, Convert.ToInt32(width * 1.1), Convert.ToInt32(high * 1.1));
                MemoryStream me = new MemoryStream();
                savepic.Save(me, System.Drawing.Imaging.ImageFormat.Png);//保存绘制的新图片的格式到内存流里

                return savepic;
            }
            else
            {
                return null;
            }
        }
    }

    public class MouldEventArgs : EventArgs
    {
        private PlugInConfiguration m_PlugInfo;
        public PlugInConfiguration PlugInfo
        { get { return m_PlugInfo; } }
        public MouldEventArgs(PlugInConfiguration info)
        {
            m_PlugInfo = info;
        }

    }
}
