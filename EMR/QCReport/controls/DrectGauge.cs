using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace DrectSoft.Core.QCReport
{
    /// <summary>志扬仪表盘控件
    /// <auth>Shan.OuYang</auth>
    /// <date>2013-03-11</date>
    public partial class Gauge : Control
    {
        #region Private Attributes

        private float minValue = 0;
        private float maxValue = 100;
        private float threshold;
        private float currentValue;
        private float recommendedValue;
        private int noOfDivisions;
        private int noOfSubDivisions;
        private string dialText;
        //private Color dialTextFont = Font;
        private Color dialTextColor = Color.Black;
        private Color dialBackColor = Color.Lavender;
        private Color dialBackColor2 = Color.Lavender;
        private Color dialForeColor = Color.Black;
        private float glossinessAlpha = 25;
        private int oldWidth, oldHeight;
        int x, y, width, height;
        private bool enableTransparentBackground;
        private bool requiresRedraw;
        private Image backgroundImg;
        private Rectangle rectImg;

        private bool arc1Draw = true;
        private bool arc2Draw = true;
        private bool arc3Draw = true;

        private Color arc1Color = Color.LawnGreen;
        private Color arc2Color = Color.Yellow;
        private Color arc3Color = Color.Red;

        private float arc1A = 0.0f;
        private float arc1Length = 0.0f;
        private float arc2A = 0.0f;
        private float arc2Length = 0.0f;
        private float arc3A = 0.0f;
        private float arc3Length = 0.0f;

        private DataSet ds;

        private bool drawDigital = false;
        private bool drawDigitalBack = false;


        float fromAngle;
        float toAngle;

        float fromAngleBack;
        float toAngleBack;

        float fromAngleCenter;
        float toAngleCenter;

        float angleFrom = 135F;
        float angleTo = 270F;

        float angleBackFrom = 0F;
        float angleBackTo = 360F;

        float angleCenterFrom = 0F;
        float angleCenterTo = 360F;

        bool drawOneRulerString = true;
        bool drawEndRulerString = true;

        bool centerClear = false;
        int centerClearLength = 50;

        #endregion
        private bool _selected = false;
        public bool Selected { get { return this._selected; } set { this._selected = value; this.Invalidate(); } }//是否选择
        /// <summary>
        /// 构造函数
        /// </summary>
        public Gauge()
        {
            InitializeComponent();
            try
            {
                x = 5;
                y = 5;
                width = this.Width - 10;
                height = this.Height - 10;
                this.noOfDivisions = 10;
                this.noOfSubDivisions = 3;
                this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                this.SetStyle(ControlStyles.ResizeRedraw, true);
                this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                this.SetStyle(ControlStyles.UserPaint, true);
                this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
                this.BackColor = Color.Transparent;
                this.Resize += new EventHandler(AquaGauge_Resize);
                requiresRedraw = true;
                this.Invalidate();
                this.Refresh();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Public Properties
        /// <summary>
        /// 最小值
        /// Mininum value on the scale
        /// </summary>
        [DefaultValue(0)]
        [Description("最小值 Mininum value on the scale")]
        public float MinValue
        {
            get
            {
                try
                {
                    return minValue;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    if (value < maxValue)
                    {
                        minValue = value;
                        if (currentValue < minValue)
                            currentValue = minValue;
                        if (recommendedValue < minValue)
                            recommendedValue = minValue;
                        requiresRedraw = true;
                        this.Invalidate();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 最大值
        /// Maximum value on the scale
        /// </summary>
        [DefaultValue(100)]
        [Description("最大值 Maximum value on the scale")]
        public float MaxValue
        {
            get
            {
                try
                {
                    return maxValue;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    if (value > minValue)
                    {
                        maxValue = value;
                        if (currentValue > maxValue)
                            currentValue = maxValue;
                        if (recommendedValue > maxValue)
                            recommendedValue = maxValue;
                        requiresRedraw = true;
                        this.Invalidate();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /*/// <summary>
        /// Gets or Sets the Threshold area from the Recommended Value. (1-99%)
        /// </summary>
        [DefaultValue(25)]
        [Description("绿色弧范围 Gets or Sets the Threshold area from the Recommended Value. (1-99%)")]
        public float ThresholdPercent
        {
            get
            {
                try
                {
                    return threshold;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    if (value > 0 && value < 100)
                    {
                        threshold = value;
                        requiresRedraw = true;
                        this.Invalidate();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }*/

        /// <summary>
        /// ？？？(不详)
        /// Threshold value from which green area will be marked.
        /// </summary>
        [DefaultValue(25)]
        [Description("？？？(不详) Threshold value from which green area will be marked.")]
        public float RecommendedValue
        {
            get
            {
                try
                {
                    return recommendedValue;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    if (value > minValue && value < maxValue)
                    {
                        recommendedValue = value;
                        requiresRedraw = true;
                        this.Invalidate();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 数值
        /// Value where the pointer will point to.
        /// </summary>
        [DefaultValue(0)]
        [Description("数值 Value where the pointer will point to.")]
        public float Value
        {
            get
            {
                try
                {
                    return currentValue;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    if (value >= minValue && value <= maxValue)
                    {
                        currentValue = value;
                        requiresRedraw = true;
                        this.Refresh();
                        this.Invalidate();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 字体颜色
        /// </summary>
        [DefaultValue(typeof(Color), "#000000")]
        [Description("字体颜色")]
        public Color DialTextColor
        {
            get
            {
                try
                {
                    return dialTextColor;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    dialTextColor = value;
                    requiresRedraw = true;
                    this.Invalidate();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 背景颜色
        /// Background color of the dial
        /// </summary>
        [DefaultValue(typeof(Color), "Lavender")]
        [Description("背景颜色 Background color of the dial")]
        public Color DialBackColor
        {
            get
            {
                try
                {
                    return dialBackColor;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    dialBackColor = value;
                    requiresRedraw = true;
                    this.Invalidate();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 背景颜色2
        /// Background color of the dial
        /// </summary>
        [DefaultValue(typeof(Color), "Lavender")]
        [Description("背景颜色2 Background color of the dial")]
        public Color DialBackColor2
        {
            get
            {
                try
                {
                    return dialBackColor2;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    dialBackColor2 = value;
                    requiresRedraw = true;
                    this.Invalidate();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 前景颜色
        /// </summary>
        [DefaultValue(typeof(Color), "#000000")]
        [Description("前景颜色")]
        public Color DialForeColor
        {
            get
            {
                try
                {
                    return dialForeColor;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    dialForeColor = value;
                    requiresRedraw = true;
                    this.Invalidate();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 镜面发光透明特效深度 0-100
        /// Glossiness strength. Range: 0-100
        /// </summary>
        [DefaultValue(72)]
        [Description("镜面发光透明特效深度 Glossiness strength. Range: 0-100")]
        public float Glossiness
        {
            get
            {
                try
                {
                    return (glossinessAlpha * 100) / 220;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    float val = value;
                    if (val > 100)
                        value = 100;
                    if (val < 0)
                        value = 0;
                    glossinessAlpha = (value * 220) / 100;
                    this.Refresh();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 除起始刻度外的大刻度数 默认10
        /// Get or Sets the number of Divisions in the dial scale.
        /// </summary>
        [DefaultValue(10)]
        [Description("除起始刻度外的大刻度数 Get or Sets the number of Divisions in the dial scale.")]
        public int NoOfDivisions
        {
            get
            {
                try
                {
                    return this.noOfDivisions;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    if (value > 1 && value < 25)
                    {
                        this.noOfDivisions = value;
                        requiresRedraw = true;
                        this.Invalidate();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 两个大刻度中间小刻度数 默认3
        /// Gets or Sets the number of Sub Divisions in the scale per Division.
        /// </summary>
        [DefaultValue(3)]
        [Description("两个大刻度中间小刻度数 Gets or Sets the number of Sub Divisions in the scale per Division.")]
        public int NoOfSubDivisions
        {
            get
            {
                try
                {
                    return this.noOfSubDivisions;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    if (value > 0 && value <= 10)
                    {
                        this.noOfSubDivisions = value;
                        requiresRedraw = true;
                        this.Invalidate();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 中间文字内容
        /// Gets or Sets the Text to be displayed in the dial
        /// </summary>
        [Description("中间文字内容 Gets or Sets the Text to be displayed in the dial")]
        public string DialText
        {
            get
            {
                try
                {
                    return this.dialText;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    this.dialText = value;
                    requiresRedraw = true;
                    this.Invalidate();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 背景是否透明
        /// Enables or Disables Transparent Background color.
        /// Note: Enabling this will reduce the performance and may make the control flicker.
        /// </summary>
        [DefaultValue(false)]
        [Description("背景是否透明 Enables or Disables Transparent Background color. Note: Enabling this will reduce the performance and may make the control flicker.")]
        public bool EnableTransparentBackground
        {
            get
            {
                try
                {
                    return this.enableTransparentBackground;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    this.enableTransparentBackground = value;
                    this.SetStyle(ControlStyles.OptimizedDoubleBuffer, !enableTransparentBackground);
                    requiresRedraw = true;
                    this.Refresh();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 是否绘制弧1
        /// </summary>
        [DefaultValue(true)]
        [Description("是否绘制弧1")]
        public bool Arc1Draw
        {
            get
            {
                try
                {
                    return this.arc1Draw;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    this.arc1Draw = value;
                    requiresRedraw = true;
                    this.Refresh();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 是否绘制弧2
        /// </summary>
        [DefaultValue(true)]
        [Description("是否绘制弧2")]
        public bool Arc2Draw
        {
            get
            {
                try
                {
                    return this.arc2Draw;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    this.arc2Draw = value;
                    requiresRedraw = true;
                    this.Refresh();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 是否绘制弧3
        /// </summary>
        [DefaultValue(true)]
        [Description("是否绘制弧3")]
        public bool Arc3Draw
        {
            get
            {
                try
                {
                    return this.arc3Draw;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    this.arc3Draw = value;
                    requiresRedraw = true;
                    this.Refresh();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 弧1颜色
        /// </summary>
        [DefaultValue(typeof(Color), "LawnGreen")]
        [Description("弧1颜色")]
        public Color Arc1Color
        {
            get
            {
                try
                {
                    return arc1Color;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    arc1Color = value;
                    requiresRedraw = true;
                    this.Refresh();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 弧2颜色
        /// </summary>
        [DefaultValue(typeof(Color), "Yellow")]
        [Description("弧1颜色")]
        public Color Arc2Color
        {
            get
            {
                try
                {
                    return arc2Color;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    arc2Color = value;
                    requiresRedraw = true;
                    this.Refresh();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 弧3颜色
        /// </summary>
        [DefaultValue(typeof(Color), "Red")]
        [Description("弧1颜色")]
        public Color Arc3Color
        {
            get
            {
                try
                {
                    return arc3Color;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    arc3Color = value;
                    requiresRedraw = true;
                    this.Refresh();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 弧1开始点度数
        /// </summary>
        [DefaultValue(0.0f)]
        [Description("弧1开始点度数")]
        public float Arc1A
        {
            get
            {
                try
                {
                    return arc1A;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    arc1A = value;
                    requiresRedraw = true;
                    this.Refresh();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 弧1结束点度数
        /// </summary>
        [DefaultValue(0.0f)]
        [Description("弧1结束点度数")]
        public float Arc1Length
        {
            get
            {
                try
                {
                    return arc1Length;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    arc1Length = value;
                    requiresRedraw = true;
                    this.Refresh();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 弧2开始点度数
        /// </summary>
        [DefaultValue(0.0f)]
        [Description("弧2开始点度数")]
        public float Arc2A
        {
            get
            {
                try
                {
                    return arc2A;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    arc2A = value;
                    requiresRedraw = true;
                    this.Refresh();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 弧2结束点度数
        /// </summary>
        [DefaultValue(0.0f)]
        [Description("弧2结束点度数")]
        public float Arc2Length
        {
            get
            {
                try
                {
                    return arc2Length;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    arc2Length = value;
                    requiresRedraw = true;
                    this.Refresh();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 弧3开始点度数
        /// </summary>
        [DefaultValue(0.0f)]
        [Description("弧3开始点度数")]
        public float Arc3A
        {
            get
            {
                try
                {
                    return arc3A;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    arc3A = value;
                    requiresRedraw = true;
                    this.Refresh();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 弧3结束点度数
        /// </summary>
        [DefaultValue(0.0f)]
        [Description("弧3结束点度数")]
        public float Arc3Length
        {
            get
            {
                try
                {
                    return arc3Length;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    arc3Length = value;
                    requiresRedraw = true;
                    this.Refresh();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 绘制底部数字
        /// </summary>
        [DefaultValue(false)]
        [Description("绘制底部数字")]
        public bool DrawDigital
        {
            get
            {
                try
                {
                    return drawDigital;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    drawDigital = value;
                    requiresRedraw = true;
                    this.Refresh();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 绘制底部数字背景
        /// </summary>
        [DefaultValue(false)]
        [Description("绘制底部数字背景")]
        public bool DrawDigitalBack
        {
            get
            {
                try
                {
                    return drawDigitalBack;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    drawDigitalBack = value;
                    requiresRedraw = true;
                    this.Refresh();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 指示 gGauge 控件的数据源。
        /// </summary>
        [Description("指示 gGauge 控件的数据源。")]
        public DataSet DataSource
        {
            get
            {
                try
                {
                    return ds;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    ds = value;
                    if ((ds != null) && (ds.Tables[0] != null))
                    {
                        MinValue = 0;
                        Value = ds.Tables[0].Rows.Count;
                    }
                    else
                    {
                        MinValue = 0;
                        Value = 0;
                    }
                    requiresRedraw = true;
                    this.Refresh();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        int coloredRimPenWidth = 30;
        int coloredRimLocation = 65;
        int coloredRimSize = 105;

        /// <summary>
        /// 中间弧形区域厚度
        /// </summary>
        [DefaultValue(30)]
        [Description("中间弧形区域厚度")]
        public int ColoredRimPenWidth
        {
            get
            {
                try
                {
                    return coloredRimPenWidth;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    coloredRimPenWidth = value;
                    requiresRedraw = true;
                    this.Invalidate();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /*/// <summary>
        /// 中间弧形区域位置
        /// </summary>
        [DefaultValue(65)]
        [Description("中间弧形区域位置")]
        public int ColoredRimLocation
        {
            get
            {
                try
                {
                    return coloredRimLocation;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    //int intTemp = coloredRimLocation - value;
                    //ColoredRimSize += intTemp * 2;

                    coloredRimLocation = value;
                    requiresRedraw = true;
                    this.Invalidate();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }*/

        /// <summary>
        /// 中间弧形区域大小
        /// </summary>
        [DefaultValue(105)]
        [Description("中间弧形区域大小")]
        public int ColoredRimSize
        {
            get
            {
                try
                {
                    return coloredRimSize;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    //int intTemp = value - coloredRimSize;
                    //coloredRimLocation -= intTemp / 2;

                    coloredRimSize = value;
                    coloredRimLocation = (this.Width / 2) - (coloredRimSize / 2);
                    requiresRedraw = true;
                    this.Invalidate();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 刻度起始位置
        /// </summary>
        [DefaultValue(135)]
        [Description("刻度起始位置")]
        public float AngleFrom
        {
            get
            {
                try
                {
                    return angleFrom;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    angleFrom = value;
                    fromAngle = angleFrom;
                    toAngle = angleTo;
                    toAngle += fromAngle;
                    requiresRedraw = true;
                    this.Invalidate();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 刻度结束位置
        /// </summary>
        [DefaultValue(270)]
        [Description("刻度结束位置")]
        public float AngleTo
        {
            get
            {
                try
                {
                    return angleTo;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    angleTo = value;
                    fromAngle = angleFrom;
                    toAngle = angleTo;
                    toAngle += fromAngle;
                    requiresRedraw = true;
                    this.Invalidate();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 背景圆起始位置
        /// </summary>
        [DefaultValue(0)]
        [Description("背景圆起始位置")]
        public float AngleBackFrom
        {
            get
            {
                try
                {
                    return angleBackFrom;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    angleBackFrom = value;
                    fromAngleBack = angleBackFrom;
                    toAngleBack = angleBackTo;
                    toAngleBack += fromAngleBack;
                    requiresRedraw = true;
                    this.Invalidate();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 背景圆结束位置
        /// </summary>
        [DefaultValue(360)]
        [Description("背景圆结束位置")]
        public float AngleBackTo
        {
            get
            {
                try
                {
                    return angleBackTo;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    angleBackTo = value;
                    fromAngleBack = angleBackFrom;
                    toAngleBack = angleBackTo;
                    toAngleBack += fromAngleBack;
                    requiresRedraw = true;
                    this.Invalidate();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 中心圆起始位置
        /// </summary>
        [DefaultValue(0)]
        [Description("中心圆起始位置")]
        public float AngleCenterFrom
        {
            get
            {
                try
                {
                    return angleCenterFrom;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    angleCenterFrom = value;
                    fromAngleCenter = angleCenterFrom;
                    toAngleCenter = angleCenterTo;
                    toAngleCenter += fromAngleCenter;
                    requiresRedraw = true;
                    this.Invalidate();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 中心圆结束位置
        /// </summary>
        [DefaultValue(360)]
        [Description("中心圆结束位置")]
        public float AngleCenterTo
        {
            get
            {
                try
                {
                    return angleCenterTo;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    angleCenterTo = value;
                    fromAngleCenter = angleCenterFrom;
                    toAngleCenter = angleCenterTo;
                    toAngleCenter += fromAngleCenter;
                    requiresRedraw = true;
                    this.Invalidate();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 是否绘制首刻度文字
        /// </summary>
        [DefaultValue(true)]
        [Description("是否绘制首刻度文字")]
        public bool DrawOneRulerString
        {
            get
            {
                try
                {
                    return drawOneRulerString;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    drawOneRulerString = value;
                    requiresRedraw = true;
                    this.Invalidate();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 是否绘制尾刻度文字
        /// </summary>
        [DefaultValue(true)]
        [Description("是否绘制尾刻度文字")]
        public bool DrawEndRulerString
        {
            get
            {
                try
                {
                    return drawEndRulerString;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    drawEndRulerString = value;
                    requiresRedraw = true;
                    this.Invalidate();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 是否中间切除
        /// </summary>
        [DefaultValue(false)]
        [Description("是否中间切除")]
        public bool CenterClear
        {
            get
            {
                try
                {
                    return centerClear;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    centerClear = value;
                    requiresRedraw = true;
                    this.Invalidate();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 中间切除长度
        /// </summary>
        [DefaultValue(50)]
        [Description("中间切除长度")]
        public int CenterClearLength
        {
            get
            {
                try
                {
                    return centerClearLength;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                try
                {
                    centerClearLength = value;
                    requiresRedraw = true;
                    this.Invalidate();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #endregion

        #region Overriden Control methods
        /// <summary>
        /// 中心整体绘制（包括实心圆和指针）调用函数
        /// Draws the pointer.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {

                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                if (centerClear)
                {
                    Rectangle rectThisImg = new Rectangle(rectImg.X + centerClearLength, rectImg.Y + centerClearLength, rectImg.Width - (centerClearLength * 2), rectImg.Height - (centerClearLength * 2));
                    Rectangle rectThis = new Rectangle(x + centerClearLength, y + centerClearLength, width - (centerClearLength * 2), height - (centerClearLength * 2));
                    SolidBrush outlineBrush = new SolidBrush(Color.FromArgb(100, Color.SlateGray));
                    Pen outline = new Pen(outlineBrush, (float)((width - (centerClearLength * 2) + 20) * .03));
                    if (toAngleBack - fromAngleBack >= 360)
                    {
                        e.Graphics.DrawEllipse(outline, rectThisImg);
                    }
                    else
                    {
                        e.Graphics.DrawPie(outline, rectThisImg, fromAngleBack, toAngleBack - fromAngleBack);
                    }
                    Pen darkRim = new Pen(Color.SlateGray);
                    if (toAngleBack - fromAngleBack >= 360)
                    {
                        e.Graphics.DrawEllipse(darkRim, rectThis);
                    }
                    else
                    {
                        e.Graphics.DrawPie(darkRim, rectThis, fromAngleBack, toAngleBack - fromAngleBack);
                    }

                    e.Graphics.FillEllipse(
                        new SolidBrush(Parent.BackColor),
                        rectThis);
                }

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                width = this.Width - x * 2;
                height = this.Height - y * 2;
                DrawPointer(e.Graphics, ((width) / 2) + x, ((height) / 2) + y);

                ///绘制选择时的边框
                if (this._selected)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Blue,1), this.ClientRectangle.X, this.ClientRectangle.Y, this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1);
                }
                else
                {
                    e.Graphics.DrawRectangle(new Pen(Brushes.White,1), this.ClientRectangle.X, this.ClientRectangle.Y, this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 背景绘制
        /// Draws the dial background.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            try
            {
                if (!enableTransparentBackground)
                {
                    base.OnPaintBackground(e);
                }

                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                e.Graphics.FillRectangle(new SolidBrush(Color.Transparent), new Rectangle(0, 0, Width, Height));
                if (backgroundImg == null || requiresRedraw)
                {
                    //初始化
                    backgroundImg = new Bitmap(this.Width, this.Height);
                    Graphics g = Graphics.FromImage(backgroundImg);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    width = this.Width - x * 2;
                    height = this.Height - y * 2;
                    rectImg = new Rectangle(x, y, width, height);

                    //Draw background color
                    //绘制背景颜色
                    Point StartPoint = new Point(x, 0);
                    Point EndPoint = new Point(x + width, 0);
                    LinearGradientBrush backGroundBrush = new LinearGradientBrush(StartPoint, EndPoint, dialBackColor, dialBackColor2);
                    //Brush backGroundBrush = new SolidBrush(Color.FromArgb(120, dialBackColor));
                    if (enableTransparentBackground && this.Parent != null)
                    {
                        float gg = width / 60;
                        //长久g.FillEllipse(new SolidBrush(this.Parent.BackColor), -gg, -gg, this.Width+gg*2, this.Height+gg*2);
                    }
                    if (toAngleBack - fromAngleBack >= 360)
                    {
                        g.FillEllipse(backGroundBrush, x, y, width, height);
                    }
                    else
                    {
                        g.FillPie(backGroundBrush, x, y, width, height, fromAngleBack, toAngleBack - fromAngleBack);
                    }

                    //Draw Rim
                    //绘制灰色圈
                    SolidBrush outlineBrush = new SolidBrush(Color.FromArgb(100, Color.SlateGray));
                    Pen outline = new Pen(outlineBrush, (float)(width * .03));
                    if (toAngleBack - fromAngleBack >= 360)
                    {
                        g.DrawEllipse(outline, rectImg);
                    }
                    else
                    {
                        g.DrawPie(outline, rectImg, fromAngleBack, toAngleBack - fromAngleBack);
                    }
                    Pen darkRim = new Pen(Color.SlateGray);
                    if (toAngleBack - fromAngleBack >= 360)
                    {
                        g.DrawEllipse(darkRim, x, y, width, height);
                    }
                    else
                    {
                        g.DrawPie(darkRim, x, y, width, height, fromAngleBack, toAngleBack - fromAngleBack);
                    }

                    //Draw Colored Rim
                    //绘制被填充弧
                    Pen colorPen = new Pen(Color.FromArgb(190, Color.Gainsboro), this.Width / 40);
                    Pen blackPen = new Pen(Color.FromArgb(250, Color.Black), this.Width / 200);
                    int gap = (int)(this.Width * 0.03F);
                    Rectangle rectg = new Rectangle(rectImg.X + gap, rectImg.Y + gap, rectImg.Width - gap * 2, rectImg.Height - gap * 2);
                    g.DrawArc(colorPen, rectg, angleFrom, angleTo);

                    //int int50 = 5;

                    /*int intPenWidth = 10;
                    int intTemp123 = intPenWidth / 2;//(gap + (this.Width / int50)) - 30

                    int intThis_X = 5 + intTemp123;//rectImg.X + intTemp123;
                    int intThis_Y = 5 + intTemp123;//rectImg.Y + intTemp123;
                    int intThis_Width = rectImg.Width - (intPenWidth * 2);
                    int intThis_Height = rectImg.Height - (intPenWidth * 2);*/

                    //Draw Threshold
                    //绘制绿色填充弧
                    rectg = new Rectangle(
                        coloredRimLocation,
                        coloredRimLocation,
                        coloredRimSize,
                        coloredRimSize
                        );

                    /*rectg = new Rectangle(
                        rectImg.X + gap + (this.Width / int50),
                        rectImg.Y + gap + (this.Width / int50), 
                        100,
                        100
                        );*/

                    float val = MaxValue - MinValue;
                    val = (100 * (this.recommendedValue - MinValue)) / val;
                    val = ((toAngle - fromAngle) * val) / 100;
                    val += fromAngle;
                    float stAngle = val - ((angleTo * threshold) / 200);
                    if (stAngle <= angleFrom)
                    {
                        stAngle = angleFrom;
                    }
                    float sweepAngle = ((angleTo * threshold) / 100);
                    if (stAngle + sweepAngle > 405)
                    {
                        sweepAngle = 405 - stAngle;
                    }

                    float floatOne = (angleTo / (maxValue - this.minValue));

                    float float1A = arc1A;
                    float float1Length = arc1Length;

                    float float2A = arc1Length + arc1A + arc2A;
                    float float2Length = arc2Length;

                    float float3A = arc1Length + arc2Length + arc1A + arc2A + arc3A;
                    float float3Length = arc3Length;

                    if (arc3Draw)
                    {
                        Pen colorPen_Red = new Pen(Color.FromArgb(200, arc3Color), coloredRimPenWidth);
                        g.DrawArc(colorPen_Red, rectg, float3A * floatOne + angleFrom, float3Length * floatOne);
                    }
                    if (arc2Draw)
                    {
                        Pen colorPen_Yellow = new Pen(Color.FromArgb(200, arc2Color), coloredRimPenWidth);
                        g.DrawArc(colorPen_Yellow, rectg, float2A * floatOne + angleFrom, float2Length * floatOne);
                    }
                    if (arc1Draw)
                    {
                        Pen colorPen_Green = new Pen(Color.FromArgb(200, arc1Color), coloredRimPenWidth);
                        g.DrawArc(colorPen_Green, rectg, float1A * floatOne + angleFrom, float1Length * floatOne);
                    }

                    //Draw Digital Value
                    //绘制底部数字背景
                    if (drawDigitalBack)
                    {
                        RectangleF digiRect = new RectangleF((float)this.Width / 2F - (float)this.width / 5F, (float)this.height / 1.2F, (float)this.width / 2.5F, (float)this.Height / 9F);
                        g.FillRectangle(new SolidBrush(Color.FromArgb(30, Color.Gray)), digiRect);
                    }
                    //绘制底部数字
                    if (drawDigital)
                    {
                        RectangleF digiFRect = new RectangleF(this.Width / 2 - this.width / 7, (int)(this.height / 1.18), this.width / 4, this.Height / 12);
                        DisplayNumber(g, this.currentValue, digiFRect);
                    }

                    //绘制中间文字
                    SizeF textSize = g.MeasureString(this.dialText, this.Font);
                    RectangleF digiFRectText = new RectangleF(this.Width / 2 - textSize.Width / 2, (int)(this.height / 1.5), textSize.Width, textSize.Height);
                    g.DrawString(dialText, this.Font, new SolidBrush(dialTextColor), digiFRectText);

                    //Draw Callibration
                    //绘制刻度
                    DrawCalibration(g, rectImg, ((width) / 2) + x, ((height) / 2) + y);

                    requiresRedraw = false;
                }
                e.Graphics.DrawImage(backgroundImg, rectImg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// CreateParams
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                try
                {
                    CreateParams cp = base.CreateParams;
                    cp.ExStyle |= 0x20;
                    return cp;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// 中心整体绘制（包括实心圆和指针）
        /// Draws the Pointer.
        /// </summary>
        /// <param name="gr"></param>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        private void DrawPointer(Graphics gr, int cx, int cy)
        {
            try
            {
                float radius = this.Width / 2 - (this.Width * .12F);
                float val = MaxValue - MinValue;

                Image img = new Bitmap(this.Width, this.Height);
                Graphics g = Graphics.FromImage(img);
                g.SmoothingMode = SmoothingMode.AntiAlias;

                val = (100 * (this.currentValue - MinValue)) / val;
                val = ((toAngle - fromAngle) * val) / 100;
                val += fromAngle;

                float angle = GetRadian(val);
                float gradientAngle = angle;

                PointF[] pts = new PointF[5];

                pts[0].X = (float)(cx + radius * Math.Cos(angle));
                pts[0].Y = (float)(cy + radius * Math.Sin(angle));

                pts[4].X = (float)(cx + radius * Math.Cos(angle - 0.02));
                pts[4].Y = (float)(cy + radius * Math.Sin(angle - 0.02));

                angle = GetRadian((val + 20));
                pts[1].X = (float)(cx + (this.Width * .09F) * Math.Cos(angle));
                pts[1].Y = (float)(cy + (this.Width * .09F) * Math.Sin(angle));

                pts[2].X = cx;
                pts[2].Y = cy;

                angle = GetRadian((val - 20));
                pts[3].X = (float)(cx + (this.Width * .09F) * Math.Cos(angle));
                pts[3].Y = (float)(cy + (this.Width * .09F) * Math.Sin(angle));

                Brush pointer = new SolidBrush(Color.Black);
                g.FillPolygon(pointer, pts);

                PointF[] shinePts = new PointF[3];
                angle = GetRadian(val);
                shinePts[0].X = (float)(cx + radius * Math.Cos(angle));
                shinePts[0].Y = (float)(cy + radius * Math.Sin(angle));

                angle = GetRadian(val + 20);
                shinePts[1].X = (float)(cx + (this.Width * .09F) * Math.Cos(angle));
                shinePts[1].Y = (float)(cy + (this.Width * .09F) * Math.Sin(angle));

                shinePts[2].X = cx;
                shinePts[2].Y = cy;

                LinearGradientBrush gpointer = new LinearGradientBrush(shinePts[0], shinePts[2], Color.SlateGray, Color.Black);
                g.FillPolygon(gpointer, shinePts);

                Rectangle rect = new Rectangle(x, y, width, height);
                DrawCenterPoint(g, rect, ((width) / 2) + x, ((height) / 2) + y);

                DrawGloss(g);

                gr.DrawImage(img, 0, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 绘制镜面效果
        /// Draws the glossiness.
        /// </summary>
        /// <param name="g"></param>
        private void DrawGloss(Graphics g)
        {
            try
            {
                RectangleF glossRect = new RectangleF(
                   x + (float)(width * 0.10),
                   y + (float)(height * 0.07),
                   (float)(width * 0.80),
                   (float)(height * 0.7));
                LinearGradientBrush gradientBrush =
                    new LinearGradientBrush(glossRect,
                    Color.FromArgb((int)glossinessAlpha, Color.White),
                    Color.Transparent,
                    LinearGradientMode.Vertical);
                if (toAngleBack - fromAngleBack >= 360)
                {
                    g.FillEllipse(gradientBrush, glossRect);
                }
                else
                {
                    g.FillPie(gradientBrush, glossRect.X, glossRect.Y, glossRect.Width, glossRect.Height, fromAngleBack, toAngleBack - fromAngleBack);
                }

                //TODO: Gradient from bottom
                glossRect = new RectangleF(
                   x + (float)(width * 0.25),
                   y + (float)(height * 0.77),
                   (float)(width * 0.50),
                   (float)(height * 0.2));
                int gloss = (int)(glossinessAlpha / 3);
                gradientBrush =
                    new LinearGradientBrush(glossRect,
                    Color.Transparent, Color.FromArgb(gloss, this.BackColor),
                    LinearGradientMode.Vertical);
                if (toAngleBack - fromAngleBack >= 360)
                {
                    g.FillEllipse(gradientBrush, glossRect);
                }
                else
                {
                    g.FillPie(gradientBrush, glossRect.X, glossRect.Y, glossRect.Width, glossRect.Height, fromAngleBack, toAngleBack - fromAngleBack);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 中心实心圆
        /// Draws the center point.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <param name="cX"></param>
        /// <param name="cY"></param>
        private void DrawCenterPoint(Graphics g, Rectangle rect, int cX, int cY)
        {
            try
            {
                float shift = Width / 5;
                RectangleF rectangle = new RectangleF(cX - (shift / 2), cY - (shift / 2), shift, shift);
                LinearGradientBrush brush = new LinearGradientBrush(rect, Color.Black, Color.FromArgb(100, this.dialBackColor), LinearGradientMode.Vertical);
                //if (toAngleCenter - fromAngleCenter >= 360)
                {
                    g.FillEllipse(brush, rectangle);
                }
                //else
                {
                    //g.FillPie(brush, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, fromAngleCenter, toAngleCenter - fromAngleCenter);
                }
                shift = Width / 7;
                rectangle = new RectangleF(cX - (shift / 2), cY - (shift / 2), shift, shift);
                brush = new LinearGradientBrush(rect, Color.SlateGray, Color.Black, LinearGradientMode.ForwardDiagonal);
                //if (toAngleCenter - fromAngleCenter >= 360)
                {
                    g.FillEllipse(brush, rectangle);
                }
                //else
                {
                    //g.FillPie(brush, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, fromAngleCenter, toAngleCenter - fromAngleCenter);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 刻度绘制
        /// Draws the Ruler
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <param name="cX"></param>
        /// <param name="cY"></param>
        private void DrawCalibration(Graphics g, Rectangle rect, int cX, int cY)
        {
            try
            {
                int noOfParts = this.noOfDivisions + 1;
                int noOfIntermediates = this.noOfSubDivisions;
                float currentAngle = GetRadian(fromAngle);
                int gap = (int)(this.Width * 0.01F);
                float shift = this.Width / 25;
                Rectangle rectangle = new Rectangle(rect.Left + gap, rect.Top + gap, rect.Width - gap, rect.Height - gap);

                float x, y, x1, y1, tx, ty, radius;
                radius = rectangle.Width / 2 - gap * 5;
                float totalAngle = toAngle - fromAngle;
                float incr = GetRadian(((totalAngle) / ((noOfParts - 1) * (noOfIntermediates + 1))));

                Pen thickPen = new Pen(Color.Black, Width / 50);
                Pen thinPen = new Pen(Color.Black, Width / 100);
                float rulerValue = MinValue;
                for (int i = 0; i <= noOfParts; i++)
                {
                    //Draw Thick Line
                    x = (float)(cX + radius * Math.Cos(currentAngle));
                    y = (float)(cY + radius * Math.Sin(currentAngle));
                    x1 = (float)(cX + (radius - Width / 20) * Math.Cos(currentAngle));
                    y1 = (float)(cY + (radius - Width / 20) * Math.Sin(currentAngle));
                    //绘制大刻度
                    g.DrawLine(thickPen, x, y, x1, y1);

                    //Draw Strings
                    StringFormat format = new StringFormat();
                    tx = (float)(cX + (radius - Width / 10) * Math.Cos(currentAngle));
                    ty = (float)(cY - shift + (radius - Width / 10) * Math.Sin(currentAngle));
                    Brush stringPen = new SolidBrush(dialForeColor);
                    StringFormat strFormat = new StringFormat(StringFormatFlags.NoClip);
                    strFormat.Alignment = StringAlignment.Center;
                    Font f = new Font(this.Font.FontFamily, (float)(this.Width / 23), this.Font.Style);
                    //绘制刻度标尺
                    if ((i == 0) && (drawOneRulerString == false))
                    {
                        //g.DrawString("aaa", f, stringPen, new PointF(tx, ty), strFormat);
                    }
                    else if ((i == (noOfParts - 1)) && (drawEndRulerString == false))
                    {
                        //g.DrawString("bbb", f, stringPen, new PointF(tx, ty), strFormat);
                    }
                    else
                    {
                        g.DrawString(rulerValue.ToString(), f, stringPen, new PointF(tx, ty), strFormat);
                    }
                    rulerValue += (float)((MaxValue - MinValue) / (noOfParts - 1));
                    rulerValue = (float)Math.Round(rulerValue, 2);

                    //currentAngle += incr;
                    if (i == noOfParts - 1)
                        break;
                    for (int j = 0; j <= noOfIntermediates; j++)
                    {
                        //Draw thin lines 
                        currentAngle += incr;
                        x = (float)(cX + radius * Math.Cos(currentAngle));
                        y = (float)(cY + radius * Math.Sin(currentAngle));
                        x1 = (float)(cX + (radius - Width / 50) * Math.Cos(currentAngle));
                        y1 = (float)(cY + (radius - Width / 50) * Math.Sin(currentAngle));
                        //绘制小刻度
                        g.DrawLine(thinPen, x, y, x1, y1);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// Converts the given degree to radian.
        /// </summary>
        /// <param name="theta"></param>
        /// <returns></returns>
        public float GetRadian(float theta)
        {
            try
            {
                return theta * (float)Math.PI / 180F;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 显示底部数字
        /// Displays the given number in the 7-Segement format.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="number"></param>
        /// <param name="drect"></param>
        private void DisplayNumber(Graphics g, float number, RectangleF drect)
        {
            try
            {
                string num = number.ToString("000.00");
                num.PadLeft(3, '0');
                float shift = 0;
                if (number < 0)
                {
                    shift -= width / 17;
                }
                bool drawDPS = false;
                char[] chars = num.ToCharArray();
                for (int i = 0; i < chars.Length; i++)
                {
                    char c = chars[i];
                    if (i < chars.Length - 1 && chars[i + 1] == '.')
                        drawDPS = true;
                    else
                        drawDPS = false;
                    if (c != '.')
                    {
                        if (c == '-')
                        {
                            DrawDigit(g, -1, new PointF(drect.X + shift, drect.Y), drawDPS, drect.Height);
                        }
                        else
                        {
                            DrawDigit(g, Int16.Parse(c.ToString()), new PointF(drect.X + shift, drect.Y), drawDPS, drect.Height);
                        }
                        shift += 15 * this.width / 250;
                    }
                    else
                    {
                        shift += 2 * this.width / 250;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 底部数字显示绘制
        /// Draws a digit in 7-Segement format.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="number"></param>
        /// <param name="position"></param>
        /// <param name="dp"></param>
        /// <param name="height"></param>
        private void DrawDigit(Graphics g, int number, PointF position, bool dp, float height)
        {
            try
            {

                float width;
                width = 10F * height / 13;

                Pen outline = new Pen(Color.FromArgb(40, this.dialBackColor));
                Pen fillPen = new Pen(this.ForeColor);

                #region Form Polygon Points
                //Segment A
                PointF[] segmentA = new PointF[5];
                segmentA[0] = segmentA[4] = new PointF(position.X + GetX(2.8F, width), position.Y + GetY(1F, height));
                segmentA[1] = new PointF(position.X + GetX(10, width), position.Y + GetY(1F, height));
                segmentA[2] = new PointF(position.X + GetX(8.8F, width), position.Y + GetY(2F, height));
                segmentA[3] = new PointF(position.X + GetX(3.8F, width), position.Y + GetY(2F, height));

                //Segment B
                PointF[] segmentB = new PointF[5];
                segmentB[0] = segmentB[4] = new PointF(position.X + GetX(10, width), position.Y + GetY(1.4F, height));
                segmentB[1] = new PointF(position.X + GetX(9.3F, width), position.Y + GetY(6.8F, height));
                segmentB[2] = new PointF(position.X + GetX(8.4F, width), position.Y + GetY(6.4F, height));
                segmentB[3] = new PointF(position.X + GetX(9F, width), position.Y + GetY(2.2F, height));

                //Segment C
                PointF[] segmentC = new PointF[5];
                segmentC[0] = segmentC[4] = new PointF(position.X + GetX(9.2F, width), position.Y + GetY(7.2F, height));
                segmentC[1] = new PointF(position.X + GetX(8.7F, width), position.Y + GetY(12.7F, height));
                segmentC[2] = new PointF(position.X + GetX(7.6F, width), position.Y + GetY(11.9F, height));
                segmentC[3] = new PointF(position.X + GetX(8.2F, width), position.Y + GetY(7.7F, height));

                //Segment D
                PointF[] segmentD = new PointF[5];
                segmentD[0] = segmentD[4] = new PointF(position.X + GetX(7.4F, width), position.Y + GetY(12.1F, height));
                segmentD[1] = new PointF(position.X + GetX(8.4F, width), position.Y + GetY(13F, height));
                segmentD[2] = new PointF(position.X + GetX(1.3F, width), position.Y + GetY(13F, height));
                segmentD[3] = new PointF(position.X + GetX(2.2F, width), position.Y + GetY(12.1F, height));

                //Segment E
                PointF[] segmentE = new PointF[5];
                segmentE[0] = segmentE[4] = new PointF(position.X + GetX(2.2F, width), position.Y + GetY(11.8F, height));
                segmentE[1] = new PointF(position.X + GetX(1F, width), position.Y + GetY(12.7F, height));
                segmentE[2] = new PointF(position.X + GetX(1.7F, width), position.Y + GetY(7.2F, height));
                segmentE[3] = new PointF(position.X + GetX(2.8F, width), position.Y + GetY(7.7F, height));

                //Segment F
                PointF[] segmentF = new PointF[5];
                segmentF[0] = segmentF[4] = new PointF(position.X + GetX(3F, width), position.Y + GetY(6.4F, height));
                segmentF[1] = new PointF(position.X + GetX(1.8F, width), position.Y + GetY(6.8F, height));
                segmentF[2] = new PointF(position.X + GetX(2.6F, width), position.Y + GetY(1.3F, height));
                segmentF[3] = new PointF(position.X + GetX(3.6F, width), position.Y + GetY(2.2F, height));

                //Segment G
                PointF[] segmentG = new PointF[7];
                segmentG[0] = segmentG[6] = new PointF(position.X + GetX(2F, width), position.Y + GetY(7F, height));
                segmentG[1] = new PointF(position.X + GetX(3.1F, width), position.Y + GetY(6.5F, height));
                segmentG[2] = new PointF(position.X + GetX(8.3F, width), position.Y + GetY(6.5F, height));
                segmentG[3] = new PointF(position.X + GetX(9F, width), position.Y + GetY(7F, height));
                segmentG[4] = new PointF(position.X + GetX(8.2F, width), position.Y + GetY(7.5F, height));
                segmentG[5] = new PointF(position.X + GetX(2.9F, width), position.Y + GetY(7.5F, height));

                //Segment DP
                #endregion

                #region Draw Segments Outline
                g.FillPolygon(outline.Brush, segmentA);
                g.FillPolygon(outline.Brush, segmentB);
                g.FillPolygon(outline.Brush, segmentC);
                g.FillPolygon(outline.Brush, segmentD);
                g.FillPolygon(outline.Brush, segmentE);
                g.FillPolygon(outline.Brush, segmentF);
                g.FillPolygon(outline.Brush, segmentG);
                #endregion

                #region Fill Segments
                //Fill SegmentA
                if (IsNumberAvailable(number, 0, 2, 3, 5, 6, 7, 8, 9))
                {
                    g.FillPolygon(fillPen.Brush, segmentA);
                }

                //Fill SegmentB
                if (IsNumberAvailable(number, 0, 1, 2, 3, 4, 7, 8, 9))
                {
                    g.FillPolygon(fillPen.Brush, segmentB);
                }

                //Fill SegmentC
                if (IsNumberAvailable(number, 0, 1, 3, 4, 5, 6, 7, 8, 9))
                {
                    g.FillPolygon(fillPen.Brush, segmentC);
                }

                //Fill SegmentD
                if (IsNumberAvailable(number, 0, 2, 3, 5, 6, 8, 9))
                {
                    g.FillPolygon(fillPen.Brush, segmentD);
                }

                //Fill SegmentE
                if (IsNumberAvailable(number, 0, 2, 6, 8))
                {
                    g.FillPolygon(fillPen.Brush, segmentE);
                }

                //Fill SegmentF
                if (IsNumberAvailable(number, 0, 4, 5, 6, 7, 8, 9))
                {
                    g.FillPolygon(fillPen.Brush, segmentF);
                }

                //Fill SegmentG
                if (IsNumberAvailable(number, 2, 3, 4, 5, 6, 8, 9, -1))
                {
                    g.FillPolygon(fillPen.Brush, segmentG);
                }
                #endregion

                //Draw decimal point
                if (dp)
                {
                    RectangleF rectF = new RectangleF(
                        position.X + GetX(10F, width),
                        position.Y + GetY(12F, height),
                        width / 7,
                        width / 7);
                    g.FillEllipse(fillPen.Brush, rectF);
                    //g.FillPie(fillPen.Brush, rectF.X, rectF.Y, rectF.Width, rectF.Height, fromAngle, toAngle - fromAngle);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获得X
        /// Gets Relative X for the given width to draw digit
        /// </summary>
        /// <param name="x"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        private float GetX(float x, float width)
        {
            try
            {
                return x * width / 12;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获得Y
        /// Gets relative Y for the given height to draw digit
        /// </summary>
        /// <param name="y"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private float GetY(float y, float height)
        {
            try
            {
                return y * height / 15;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// Returns true if a given number is available in the given list.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="listOfNumbers"></param>
        /// <returns></returns>
        private bool IsNumberAvailable(int number, params int[] listOfNumbers)
        {
            try
            {
                if (listOfNumbers.Length > 0)
                {
                    foreach (int i in listOfNumbers)
                    {
                        if (i == number)
                            return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 控件大小变化后执行函数
        /// Restricts the size to make sure the height and width are always same.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AquaGauge_Resize(object sender, EventArgs e)
        {
            try
            {
                if (this.Width < 136)
                {
                    this.Width = 136;
                }
                if (oldWidth != this.Width)
                {
                    this.Height = this.Width;
                    oldHeight = this.Width;
                }
                if (oldHeight != this.Height)
                {
                    this.Width = this.Height;
                    oldWidth = this.Width;
                }
                requiresRedraw = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AquaGauge_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                coloredRimLocation = (this.Width / 2) - (coloredRimSize / 2);
                requiresRedraw = true;
                this.Invalidate();
                this.Refresh();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AquaGauge_Load(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AquaGauge_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                //if (ds != null)
                //{
                //    new DataSourceDetailForm(ds).ShowDialog();
                //}
                //else
                //{
                //    MessageBox.Show("DataSource 空值");
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Gauge_FontChanged(object sender, EventArgs e)
        {
            try
            {
                requiresRedraw = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Gauge_ForeColorChanged(object sender, EventArgs e)
        {
            try
            {
                requiresRedraw = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
