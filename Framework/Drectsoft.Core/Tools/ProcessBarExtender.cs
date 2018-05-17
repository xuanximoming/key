using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace DrectSoft.Core
{
    public partial class ProcessBarExtender : Control
    {

        #region Constructor
        private const String CategoryName = "ProcessBarExtender";
        public ProcessBarExtender()
        {
            InitializeComponent();
            this.Visible = true;
            //Start();
        }
        #endregion



        #region members

        private Color m_Color1 = Color.FromArgb(170, 240, 170);

        private Color m_Color2 = Color.FromArgb(10, 150, 10);

        private Color m_ColorBackGround = Color.White;

        private Color m_ColorText = Color.Black;

        private Image m_DobleBack = null;

        private GradientMode m_GradientStyle = GradientMode.VerticalCenter;

        private int m_Max = 100;

        private int m_Min = 0;

        private int m_Position = 50;

        private byte m_SteepDistance = 2;

        private byte m_SteepWidth = 6;

        private BackgroundWorker m_Worker = null;

        private Timer m_Timer = null;

        #endregion

        #region Dispose

        //protected override void Dispose(bool disposing)
        //{
        //}

        #endregion

        #region 颜色

        [Category(CategoryName)]
        [Description("The Back Color of the Progress Bar")]
        public Color ColorBackGround
        {
            get { return m_ColorBackGround; }
            set
            {
                m_ColorBackGround = value;
                this.InvalidateBuffer(true);
            }
        }

        [Category(CategoryName)]
        [Description("The Border Color of the gradient in the Progress Bar")]
        public Color ColorBarBorder
        {
            get { return m_Color1; }
            set
            {
                m_Color1 = value;
                this.InvalidateBuffer(true);
            }
        }

        [Category(CategoryName)]
        [Description("The Center Color of the gradient in the Progress Bar")]
        public Color ColorBarCenter
        {
            get { return m_Color2; }
            set
            {
                m_Color2 = value;
                this.InvalidateBuffer(true);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description("Set to TRUE to reset all colors like the Windows XP Progress Bar ?")]
        [Category(CategoryName)]
        [DefaultValue(false)]
        public bool ColorsXP
        {
            get { return false; }
            set
            {
                ColorBarBorder = Color.FromArgb(170, 240, 170);
                ColorBarCenter = Color.FromArgb(10, 150, 10);
                ColorBackGround = Color.White;
            }
        }

        [Category(CategoryName)]
        [Description("The Color of the text displayed in the Progress Bar")]
        public Color ColorText
        {
            get { return m_ColorText; }
            set
            {
                m_ColorText = value;

                if (this.Text != String.Empty)
                {
                    this.Invalidate();
                }
            }
        }

        #endregion

        #region 进度条位置

        [RefreshProperties(RefreshProperties.Repaint)]
        [Category(CategoryName)]
        [Description("The Current Position of the Progress Bar")]
        [DefaultValue(0)]
        public int Position
        {
            get { return m_Position; }
            set
            {
                if (value > m_Max)
                {
                    m_Position = m_Max;
                }
                else if (value < m_Min)
                {
                    m_Position = m_Min;
                }
                else
                {
                    m_Position = value;
                }
                this.Invalidate();
            }
        }

        [RefreshProperties(RefreshProperties.Repaint)]
        [Category(CategoryName)]
        [Description("The Max Position of the Progress Bar")]
        public int PositionMax
        {
            get { return m_Max; }
            set
            {
                if (value > m_Min)
                {
                    m_Max = value;

                    if (m_Position > m_Max)
                    {
                        Position = m_Max;
                    }

                    this.InvalidateBuffer(true);
                }
            }
        }

        [RefreshProperties(RefreshProperties.Repaint)]
        [Category(CategoryName)]
        [Description("The Min Position of the Progress Bar")]
        public int PositionMin
        {
            get { return m_Min; }
            set
            {
                if (value < m_Max)
                {
                    m_Min = value;

                    if (m_Position < m_Min)
                    {
                        Position = m_Min;
                    }
                    this.InvalidateBuffer(true);
                }
            }
        }

        [Category(CategoryName)]
        [Description("The number of Pixels between two Steeps in Progress Bar")]
        [DefaultValue((byte)2)]
        public byte SteepDistance
        {
            get { return m_SteepDistance; }
            set
            {
                if (value >= 0)
                {
                    m_SteepDistance = value;
                    this.InvalidateBuffer(true);
                }
            }
        }

        #endregion

        #region  样式

        [Category(CategoryName)]
        [Description("The Style of the gradient bar in Progress Bar")]
        [DefaultValue(GradientMode.VerticalCenter)]
        public GradientMode GradientStyle
        {
            get { return m_GradientStyle; }
            set
            {
                if (m_GradientStyle != value)
                {
                    m_GradientStyle = value;
                    CreatePaintElements();
                    this.Invalidate();
                }
            }
        }

        [Category(CategoryName)]
        [Description("The number of Pixels of the Steeps in Progress Bar")]
        [DefaultValue((byte)6)]
        public byte SteepWidth
        {
            get { return m_SteepWidth; }
            set
            {
                if (value > 0)
                {
                    m_SteepWidth = value;
                    this.InvalidateBuffer(true);
                }
            }
        }

        #endregion

        #region 背景图

        [RefreshProperties(RefreshProperties.Repaint)]
        [Category(CategoryName)]
        public override Image BackgroundImage
        {
            get { return base.BackgroundImage; }
            set
            {
                base.BackgroundImage = value;
                InvalidateBuffer();
            }
        }

        #endregion

        #region 进度条显示文本

        [Category(CategoryName)]
        [Description("The Text displayed in the Progress Bar")]
        [DefaultValue("")]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (base.Text != value)
                {
                    base.Text = value;
                    this.Invalidate();
                }
            }
        }

        #endregion

        #region 文字SHADOW

        private bool mTextShadow = true;

        [Category(CategoryName)]
        [Description("Set the Text shadow in the Progress Bar")]
        [DefaultValue(true)]
        public bool TextShadow
        {
            get { return mTextShadow; }
            set
            {
                mTextShadow = value;
                this.Invalidate();
            }
        }

        #endregion

        #region  文字阴影效果

        private byte mTextShadowAlpha = 150;

        [Category(CategoryName)]
        [Description("Set the Alpha Channel of the Text shadow in the Progress Bar")]
        [DefaultValue((byte)100)]
        public byte TextShadowAlpha
        {
            get { return mTextShadowAlpha; }
            set
            {
                if (mTextShadowAlpha != value)
                {
                    mTextShadowAlpha = value;
                    //this.TextShadow = true;
                }
            }
        }

        #endregion

        #region 重新绘制

        #region "  OnPaint  "

        protected override void OnPaint(PaintEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("Paint " + this.Name + "  Pos: "+this.Position.ToString());
            if (!this.IsDisposed)
            {
                int mSteepTotal = m_SteepWidth + m_SteepDistance;
                float mUtilWidth = this.Width - 6 + m_SteepDistance;

                if (m_DobleBack == null)
                {
                    mUtilWidth = this.Width - 6 + m_SteepDistance;
                    int mMaxSteeps = (int)(mUtilWidth / mSteepTotal);
                    this.Width = 6 + mSteepTotal * mMaxSteeps;

                    m_DobleBack = new Bitmap(this.Width, this.Height);

                    Graphics g2 = Graphics.FromImage(m_DobleBack);

                    CreatePaintElements();

                    g2.Clear(m_ColorBackGround);

                    if (this.BackgroundImage != null)
                    {
                        TextureBrush textuBrush = new TextureBrush(this.BackgroundImage, WrapMode.Tile);
                        g2.FillRectangle(textuBrush, 0, 0, this.Width, this.Height);
                        textuBrush.Dispose();
                    }
                    //				g2.DrawImage()

                    g2.DrawRectangle(mPenOut2, outnnerRect2);
                    g2.DrawRectangle(mPenOut, outnnerRect);
                    g2.DrawRectangle(mPenIn, innerRect);
                    g2.Dispose();

                }

                Image ima = new Bitmap(m_DobleBack);

                Graphics gtemp = Graphics.FromImage(ima);

                int mCantSteeps = (int)((((float)m_Position - m_Min) / (m_Max - m_Min)) * mUtilWidth / mSteepTotal);

                for (int i = 0; i < mCantSteeps; i++)
                {
                    DrawSteep(gtemp, i);
                }

                if (this.Text != String.Empty)
                {
                    gtemp.TextRenderingHint = TextRenderingHint.AntiAlias;
                    DrawCenterString(gtemp, this.ClientRectangle);
                }

                e.Graphics.DrawImage(ima, e.ClipRectangle.X, e.ClipRectangle.Y, e.ClipRectangle, GraphicsUnit.Pixel);
                ima.Dispose();
                gtemp.Dispose();

            }

        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        #endregion

        #region "  OnSizeChange  "

        protected override void OnSizeChanged(EventArgs e)
        {
            if (!this.IsDisposed)
            {
                if (this.Height < 12)
                {
                    this.Height = 12;
                }

                base.OnSizeChanged(e);
                this.InvalidateBuffer(true);
            }

        }

        protected override Size DefaultSize
        {
            get { return new Size(100, 29); }
        }


        #endregion

        #region "  More Draw Methods  "

        private void DrawSteep(Graphics g, int number)
        {
            g.FillRectangle(mBrush1, 4 + number * (m_SteepDistance + m_SteepWidth), mSteepRect1.Y + 1, m_SteepWidth, mSteepRect1.Height);
            g.FillRectangle(mBrush2, 4 + number * (m_SteepDistance + m_SteepWidth), mSteepRect2.Y + 1, m_SteepWidth, mSteepRect2.Height - 1);
        }

        private void InvalidateBuffer()
        {
            InvalidateBuffer(false);
        }

        private void InvalidateBuffer(bool InvalidateControl)
        {
            if (m_DobleBack != null)
            {
                m_DobleBack.Dispose();
                m_DobleBack = null;
            }

            if (InvalidateControl)
            {
                this.Invalidate();
            }
        }

        private void DisposeBrushes()
        {
            if (mBrush1 != null)
            {
                mBrush1.Dispose();
                mBrush1 = null;
            }

            if (mBrush2 != null)
            {
                mBrush2.Dispose();
                mBrush2 = null;
            }

        }

        private void DrawCenterString(Graphics gfx, Rectangle box)
        {
            SizeF ss = gfx.MeasureString(this.Text, this.Font);

            float left = box.X + (box.Width - ss.Width) / 2;
            float top = box.Y + (box.Height - ss.Height) / 2;

            if (mTextShadow)
            {
                SolidBrush mShadowBrush = new SolidBrush(Color.FromArgb(mTextShadowAlpha, Color.Black));
                gfx.DrawString(this.Text, this.Font, mShadowBrush, left + 1, top + 1);
                mShadowBrush.Dispose();
            }
            SolidBrush mTextBrush = new SolidBrush(m_ColorText);
            gfx.DrawString(this.Text, this.Font, mTextBrush, left, top);
            mTextBrush.Dispose();

        }

        #endregion

        #region "  CreatePaintElements   "

        private Rectangle innerRect;
        private LinearGradientBrush mBrush1;
        private LinearGradientBrush mBrush2;
        private Pen mPenIn = new Pen(Color.FromArgb(239, 239, 239));

        private Pen mPenOut = new Pen(Color.FromArgb(104, 104, 104));
        private Pen mPenOut2 = new Pen(Color.FromArgb(190, 190, 190));

        private Rectangle mSteepRect1;
        private Rectangle mSteepRect2;
        private Rectangle outnnerRect;
        private Rectangle outnnerRect2;

        private void CreatePaintElements()
        {
            DisposeBrushes();

            switch (m_GradientStyle)
            {
                case GradientMode.VerticalCenter:

                    mSteepRect1 = new Rectangle(
                        0,
                        2,
                        m_SteepWidth,
                        this.Height / 2 + (int)(this.Height * 0.05));
                    mBrush1 = new LinearGradientBrush(mSteepRect1, m_Color1, m_Color2, LinearGradientMode.Vertical);

                    mSteepRect2 = new Rectangle(
                        0,
                        mSteepRect1.Bottom - 1,
                        m_SteepWidth,
                        this.Height - mSteepRect1.Height - 4);
                    mBrush2 = new LinearGradientBrush(mSteepRect2, m_Color2, m_Color1, LinearGradientMode.Vertical);
                    break;

                case GradientMode.Vertical:
                    mSteepRect1 = new Rectangle(
                        0,
                        3,
                        m_SteepWidth,
                        this.Height - 7);
                    mBrush1 = new LinearGradientBrush(mSteepRect1, m_Color1, m_Color2, LinearGradientMode.Vertical);
                    mSteepRect2 = new Rectangle(
                        -100,
                        -100,
                        1,
                        1);
                    mBrush2 = new LinearGradientBrush(mSteepRect2, m_Color2, m_Color1, LinearGradientMode.Horizontal);
                    break;


                case GradientMode.Horizontal:
                    mSteepRect1 = new Rectangle(
                        0,
                        3,
                        m_SteepWidth,
                        this.Height - 7);

                    //					mBrush1 = new LinearGradientBrush(rTemp, m_Color1, m_Color2, LinearGradientMode.Horizontal);
                    mBrush1 = new LinearGradientBrush(this.ClientRectangle, m_Color1, m_Color2, LinearGradientMode.Horizontal);
                    mSteepRect2 = new Rectangle(
                        -100,
                        -100,
                        1,
                        1);
                    mBrush2 = new LinearGradientBrush(mSteepRect2, Color.Red, Color.Red, LinearGradientMode.Horizontal);
                    break;


                case GradientMode.HorizontalCenter:
                    mSteepRect1 = new Rectangle(
                        0,
                        3,
                        m_SteepWidth,
                        this.Height - 7);
                    //					mBrush1 = new LinearGradientBrush(rTemp, m_Color1, m_Color2, LinearGradientMode.Horizontal);
                    mBrush1 = new LinearGradientBrush(this.ClientRectangle, m_Color1, m_Color2, LinearGradientMode.Horizontal);
                    mBrush1.SetBlendTriangularShape(0.5f);

                    mSteepRect2 = new Rectangle(
                        -100,
                        -100,
                        1,
                        1);
                    mBrush2 = new LinearGradientBrush(mSteepRect2, Color.Red, Color.Red, LinearGradientMode.Horizontal);
                    break;


                case GradientMode.Diagonal:
                    mSteepRect1 = new Rectangle(
                        0,
                        3,
                        m_SteepWidth,
                        this.Height - 7);
                    //					mBrush1 = new LinearGradientBrush(rTemp, m_Color1, m_Color2, LinearGradientMode.ForwardDiagonal);
                    mBrush1 = new LinearGradientBrush(this.ClientRectangle, m_Color1, m_Color2, LinearGradientMode.ForwardDiagonal);
                    //					((LinearGradientBrush) mBrush1).SetBlendTriangularShape(0.5f);

                    mSteepRect2 = new Rectangle(
                        -100,
                        -100,
                        1,
                        1);
                    mBrush2 = new LinearGradientBrush(mSteepRect2, Color.Red, Color.Red, LinearGradientMode.Horizontal);
                    break;

                default:
                    mBrush1 = new LinearGradientBrush(mSteepRect1, m_Color1, m_Color2, LinearGradientMode.Vertical);
                    mBrush2 = new LinearGradientBrush(mSteepRect2, m_Color2, m_Color1, LinearGradientMode.Vertical);
                    break;

            }

            innerRect = new Rectangle(
                this.ClientRectangle.X + 2,
                this.ClientRectangle.Y + 2,
                this.ClientRectangle.Width - 4,
                this.ClientRectangle.Height - 4);
            outnnerRect = new Rectangle(
                this.ClientRectangle.X,
                this.ClientRectangle.Y,
                this.ClientRectangle.Width - 1,
                this.ClientRectangle.Height - 1);
            outnnerRect2 = new Rectangle(
                this.ClientRectangle.X + 1,
                this.ClientRectangle.Y + 1,
                this.ClientRectangle.Width,
                this.ClientRectangle.Height);

        }

        #endregion

        #endregion

        public void Start()
        {
            if (m_Timer != null)
                return;
            m_Timer = new Timer();
            m_Timer.Tick += new EventHandler(m_Timer_Tick);
            m_Timer.Start();
            this.Text = String.Empty;
        }

        private void m_Timer_Tick(object sender, EventArgs e)
        {
            if (this.Position < this.PositionMax)
            {
                this.Position += 1;
            }
            else
            {
                this.Position = 10;
            }
        }

        public void Close()
        {
            if (m_Timer != null)
            {
                m_Timer.Stop();
                this.m_Timer.Dispose();
            }
            this.Text = String.Empty; ;
            this.Position =10;
        }

        //[DllImport("user32.dll")]
        //static extern void BlockInput(bool Block);
    }

    #region "  Gradient Mode  "

    public enum GradientMode
    {
        Vertical,
        VerticalCenter,
        Horizontal,
        HorizontalCenter,
        Diagonal
    } ;

    #endregion
}