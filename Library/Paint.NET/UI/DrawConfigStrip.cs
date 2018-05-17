using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using DrectSoft.Resources;

namespace DrectSoft.Basic.Paint.NET
{

    public class DrawConfigStrip
        : ToolStrip
    {

        #region Fields
        private static readonly string noneBrushText = "透明";
        private static readonly string solidBrushText = "实心";
        private static Dictionary<string, Color> _colors;
        private ToolStripDropDownButton drawColorButton;
        private ToolStripDropDownButton fillColorButton;
        private ToolStripComboBox lineWidthComboBox;
        private ToolStripLabel lineWidthLabel;
        private int[] lineWidths;
        private ToolStripLabel fillStyleLabel;
        private ToolStripComboBox fillStyleComboBox;
        private SurfaceStyleSettingHelper _sssh;
        private Image _orgLineColorImage;
        private Image _currentLineColorImage;
        private Color _currentLineColor;
        private Image _orgFillColorImage;
        private Image _currentFillColorImage;
        private Color _currentFillColor;
        #endregion

        #region Ctors

        public DrawConfigStrip()
        {
            this._sssh = new SurfaceStyleSettingHelper();
            this.lineWidths = new int[]
            { 
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12, 15, 20, 
                25, 30, 35, 40, 50, 60, 70, 80, 90, 100
             };
            this.InitializeComponent();
            AddColor(this.drawColorButton);
            AddColor(this.fillColorButton);
            this.fillStyleComboBox.Items.Add(noneBrushText);
            this.fillStyleComboBox.Items.Add(solidBrushText);
            this.fillStyleComboBox.SelectedIndex = 0;
            this.lineWidthComboBox.ComboBox.SuspendLayout();
            foreach (int i in this.lineWidths)
                this.lineWidthComboBox.Items.Add(i.ToString());
            this.lineWidthComboBox.ComboBox.ResumeLayout(false);
            this.lineWidthComboBox.SelectedIndex = 0;
            this.lineWidthComboBox.Size = new Size(50, 24);
            this.fillStyleComboBox.Size = new Size(150, 24);
            ResetUI();
        }

        private void InitializeComponent()
        {
            this.fillStyleLabel = new ToolStripLabel();
            this.fillStyleComboBox = new ToolStripComboBox();
            this.lineWidthLabel = new ToolStripLabel();
            this.lineWidthComboBox = new ToolStripComboBox();
            this.drawColorButton = new ToolStripDropDownButton();
            this.fillColorButton = new ToolStripDropDownButton();

            base.SuspendLayout();

            this.fillStyleLabel.Name = "fillStyleLabel";
            this.fillStyleLabel.Text = "刷子类型";
            this.fillStyleLabel.ToolTipText = "刷子类型";
            this.fillStyleComboBox.Name = "fillStyleComboBox";
            this.fillStyleComboBox.ToolTipText = "选择刷子类型";
            this.fillStyleComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.fillStyleComboBox.DropDownWidth = 0xea;
            this.fillStyleComboBox.ComboBox.DrawMode = DrawMode.OwnerDrawVariable;
            this.fillStyleComboBox.ComboBox.MeasureItem += new MeasureItemEventHandler(this.fillStyleComboBox_MeasureItem);
            this.fillStyleComboBox.ComboBox.SelectedValueChanged += new EventHandler(this.fillStyleComboBox_SelectedValueChanged);
            this.fillStyleComboBox.ComboBox.DrawItem += new DrawItemEventHandler(this.fillStyleComboBox_DrawItem);

            this.lineWidthLabel.Name = "lineWidthLabel";
            this.lineWidthLabel.Text = "线宽";
            this.lineWidthLabel.ToolTipText = "选择线宽";
            this.lineWidthComboBox.Validating += new CancelEventHandler(this.brushSizeComboBox_Validating);
            this.lineWidthComboBox.TextChanged += new EventHandler(this.sizeComboBox_TextChanged);
            this.lineWidthComboBox.AutoSize = false;
            this.lineWidthComboBox.Width = 0x2c;

            this.drawColorButton.Name = "drawColorButton";
            this.drawColorButton.Image = null;
            this.drawColorButton.ImageTransparentColor = Color.Maroon;
            this.drawColorButton.ToolTipText = "线条颜色";

            this.fillColorButton.Name = "fillColorButton";
            this.fillColorButton.Image = null;
            this.fillColorButton.ImageTransparentColor = Color.Maroon;
            this.fillColorButton.ToolTipText = "填充颜色";

            this.Dock = DockStyle.None;
            this.GripStyle = ToolStripGripStyle.Hidden;
            this.Items.Add(this.drawColorButton);
            this.Items.Add(this.lineWidthLabel);
            this.Items.Add(this.lineWidthComboBox);
            this.Items.Add(this.fillStyleLabel);
            this.Items.Add(this.fillStyleComboBox);
            this.Items.Add(this.fillColorButton);

            base.ResumeLayout(false);
        }

        private void AddColor(ToolStripDropDownButton tsddb)
        {
            ToolStripDropDown tsdd = tsddb.DropDown;
            ToolStripButton stb;
            Bitmap bmp;
            Graphics g;
            tsdd.AutoSize = false;
            tsdd.Size = new Size(120, 80);
            FlowLayoutSettings fls = tsdd.LayoutSettings as FlowLayoutSettings;
            fls.FlowDirection = FlowDirection.LeftToRight;
            fls.WrapContents = true;
            foreach (KeyValuePair<string, Color> c in Colors)
            {
                stb = new ToolStripButton();
                stb.Name = c.Key;
                stb.DisplayStyle = ToolStripItemDisplayStyle.Image;
                bmp = new Bitmap(16, 16);
                using (g = Graphics.FromImage(bmp))
                using (Brush br = new SolidBrush(c.Value))
                    g.FillRectangle(br, 0, 0, 16, 16);
                stb.Image = bmp;
                stb.Text = c.Key;
                stb.Tag = c.Value;
                stb.Click += new EventHandler(stb_Click);
                tsdd.Items.Add(stb);
            }
        }

        #endregion

        #region Members

        #region Public

        public IShapeSurface Surface
        {
            get { return Sssh.Surface; }
            set
            {
                if (DesignMode)
                    Sssh.Surface = value;
                else
                {
                    this.SuspendLayout();
                    DetachSurface();
                    Sssh.Surface = value;
                    AttachSurface();
                    this.ResumeLayout();
                }
            }
        }

        public bool FillStyleVisible
        {
            get { return fillStyleComboBox.Visible; }
            set
            {
                fillStyleComboBox.Visible = value;
                fillStyleLabel.Visible = value;
            }
        }

        #endregion

        #region Event Handlers

        private void brushSizeComboBox_Validating(object sender, CancelEventArgs e)
        {
            float width;
            bool isFloat = float.TryParse(
                this.lineWidthComboBox.Text, out width);
            if (!isFloat)
            {
                this.lineWidthComboBox.BackColor = Color.Red;
                this.lineWidthComboBox.ToolTipText = "PenConfigWidget.Error.InvalidNumber";
            }
            else if (width < 1f)
            {
                this.lineWidthComboBox.BackColor = Color.Red;
                this.lineWidthComboBox.ToolTipText = "PenConfigWidget.Error.TooSmall";
            }
            else if (width > 100f)
            {
                this.lineWidthComboBox.BackColor = Color.Red;
                this.lineWidthComboBox.ToolTipText = "PenConfigWidget.Error.TooLarge";
            }
            else
            {
                this.lineWidthComboBox.BackColor = SystemColors.Window;
                this.lineWidthComboBox.ToolTipText = string.Empty;
                Sssh.LineWidth = width;
            }
        }

        private void fillStyleComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            Rectangle rectangle1 = e.Bounds;
            if (e.Index == -1)
            {
                return;
            }
            string text1 = (string)this.fillStyleComboBox.Items[e.Index];
            if (text1 != noneBrushText && text1 != solidBrushText)
            {
                Rectangle rectangle2 = rectangle1;
                rectangle2.Width = rectangle2.Left + 0x19;
                rectangle1.X = rectangle2.Right;
                using (Brush brush1 = UICommon.GetBrush(
                    text1, e.ForeColor, e.BackColor,
                    Surface.Source.NamedHatchStyle,
                    Surface.Source.NamedTextureStyle))
                {
                    e.Graphics.FillRectangle(brush1, rectangle2);
                }
                StringFormat format1 = new StringFormat();
                format1.Alignment = StringAlignment.Near;
                using (SolidBrush brush2 = new SolidBrush(Color.White))
                {
                    if ((e.State & DrawItemState.Focus) == DrawItemState.None)
                    {
                        brush2.Color = SystemColors.Window;
                        e.Graphics.FillRectangle(brush2, rectangle1);
                        brush2.Color = SystemColors.WindowText;
                        e.Graphics.DrawString(text1, this.Font, brush2, (RectangleF) rectangle1, format1);
                        goto Label_027B;
                    }
                    brush2.Color = SystemColors.Highlight;
                    e.Graphics.FillRectangle(brush2, rectangle1);
                    brush2.Color = SystemColors.HighlightText;
                    e.Graphics.DrawString(text1, this.Font, brush2, (RectangleF) rectangle1, format1);
                    goto Label_027B;
                }
            }
            using (SolidBrush brush3 = new SolidBrush(Color.White))
            {
                if ((e.State & DrawItemState.Focus) == DrawItemState.None)
                {
                    brush3.Color = SystemColors.Window;
                    e.Graphics.FillRectangle(brush3, e.Bounds);
                    string text3 = this.fillStyleComboBox.Items[e.Index].ToString();
                    brush3.Color = SystemColors.WindowText;
                    e.Graphics.DrawString(text3, this.Font, brush3, (RectangleF) e.Bounds);
                }
                else
                {
                    brush3.Color = SystemColors.Highlight;
                    e.Graphics.FillRectangle(brush3, e.Bounds);
                    string text4 = this.fillStyleComboBox.Items[e.Index].ToString();
                    brush3.Color = SystemColors.HighlightText;
                    e.Graphics.DrawString(text4, this.Font, brush3, (RectangleF) e.Bounds);
                }
            }
        Label_027B:
            e.DrawFocusRectangle();
        }

        private void fillStyleComboBox_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            string text1 = this.fillStyleComboBox.Items[e.Index].ToString();
            SizeF ef1 = e.Graphics.MeasureString(text1, this.Font);
            e.ItemHeight = (int) ef1.Height;
            e.ItemWidth = (int) ef1.Width;
        }

        private void fillStyleComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            switch (this.fillStyleComboBox.SelectedIndex)
            {
                case -1:
                    return;
                case 0:
                    Sssh.IsFillColor = false;
                    Sssh.IsHatch = false;
                    return;
                case 1:
                    Sssh.IsFillColor = true;
                    Sssh.IsHatch = false;
                    return;
                default:
                    Sssh.IsFillColor = true;
                    Sssh.IsHatch = true;
                    NamedHatchStyles nhs = Surface.Source.NamedHatchStyle;
                    Sssh.Hatch = nhs.GetHatchStyle(
                        this.fillStyleComboBox.SelectedItem as string) ?? default(HatchStyle);
                    break;
            }
        }

        private void sizeComboBox_TextChanged(object sender, EventArgs e)
        {
            this.brushSizeComboBox_Validating(this, new CancelEventArgs());
        }

        private void stb_Click(object sender, EventArgs e)
        {
            ToolStripButton tsb = sender as ToolStripButton;
            ToolStripItem tsi = ((ToolStripDropDown)tsb.Owner).OwnerItem;
            if (tsi == drawColorButton)
            {
                Sssh.DrawColor = (Color)tsb.Tag;
                RefreshLineButton();
            }
            else if (tsi == fillColorButton)
            {
                Sssh.FillColor = (Color)tsb.Tag;
                RefreshFillButton();
            }
        }

        private void Source_Loaded(object sender, EventArgs e)
        {
            this.fillStyleComboBox.Items.Clear();
            this.fillStyleComboBox.Items.Add(noneBrushText);
            this.fillStyleComboBox.Items.Add(solidBrushText);
            this.fillStyleComboBox.SelectedIndex = 0;
            string[] textArray1 = Surface.Source.NamedHatchStyle.GetNames();
            Array.Sort<string>(textArray1);
            this.fillStyleComboBox.Items.AddRange(textArray1);
        }

        private void Source_GhostChanged(object sender, EventArgs e)
        {
            this.SuspendLayout();
            ResetUI();
            this.ResumeLayout();
        }

        #endregion

        #region Private

        private Dictionary<string, Color> Colors
        {
            get
            {
                if (_colors == null)
                {
                    Dictionary<string, Color> colors =
                        new Dictionary<string, Color>();
                    colors.Add("黑色", Color.Black);
                    colors.Add("灰色", Color.Gray);
                    colors.Add("白色", Color.White);
                    colors.Add("红色", Color.Red);
                    colors.Add("蓝色", Color.Blue);
                    colors.Add("绿色", Color.Green);
                    colors.Add("粉红色", Color.Pink);
                    colors.Add("黄色", Color.Yellow);
                    colors.Add("紫色", Color.Purple);
                    _colors = colors;
                }
                return _colors;
            }
        }

        private SurfaceStyleSettingHelper Sssh
        {
            get { return _sssh; }
        }

        private Image OrgLineColorImage
        {
            get
            {
                if (_orgLineColorImage == null)
                {
                    _orgLineColorImage = ResourceManager.GetSmallIcon(ResourceNames.LineColor, IconType.Normal);
                }
                return _orgLineColorImage;
            }
        }

        private Image CurrentLineColorImage
        {
            get
            {
                Color c = Sssh.DrawColor;
                if (CurrentLineColor != c ||
                    _currentLineColorImage == null)
                {
                    Bitmap bmp = new Bitmap(OrgLineColorImage);
                    bmp.MakeTransparent(Color.Blue);
                    _currentLineColorImage = new Bitmap(OrgLineColorImage.Width, OrgLineColorImage.Height);
                    using (Graphics g = Graphics.FromImage(_currentLineColorImage))
                    {
                        using (Brush br = new SolidBrush(c))
                            g.FillRectangle(br, new Rectangle(Point.Empty, OrgLineColorImage.Size));
                        g.DrawImageUnscaled(bmp, Point.Empty);
                    }
                    CurrentLineColor = c;
                }
                return _currentLineColorImage;
            }
        }

        private Color CurrentLineColor
        {
            get { return _currentLineColor; }
            set { _currentLineColor = value; }
        }

        private Image OrgFillColorImage
        {
            get
            {
                if (_orgFillColorImage == null)
                {
                    _orgFillColorImage = ResourceManager.GetSmallIcon("FillColor", IconType.Normal);
                }
                return _orgFillColorImage;
            }
        }

        private Image CurrentFillColorImage
        {
            get
            {
                Color c = Sssh.FillColor;
                if (CurrentFillColor != c ||
                    _currentFillColorImage == null)
                {
                    Bitmap bmp = new Bitmap(OrgFillColorImage);
                    bmp.MakeTransparent(Color.Blue);
                    _currentFillColorImage = new Bitmap(OrgFillColorImage.Width, OrgFillColorImage.Height);
                    using (Graphics g = Graphics.FromImage(_currentFillColorImage))
                    {
                        using (Brush br = new SolidBrush(c))
                            g.FillRectangle(br, new Rectangle(Point.Empty, OrgFillColorImage.Size));
                        g.DrawImageUnscaled(bmp, Point.Empty);
                    }
                    CurrentFillColor = c;
                }
                return _currentFillColorImage;
            }
        }

        private Color CurrentFillColor
        {
            get { return _currentFillColor; }
            set { _currentFillColor = value; }
        }

        private void DetachSurface()
        {
            this.fillStyleComboBox.Items.Clear();
            if (Surface != null)
            {
                Surface.Source.Loaded -= new EventHandler(Source_Loaded);
                Surface.Source.GhostChanged -= new EventHandler(Source_GhostChanged);
            }
        }

        private void AttachSurface()
        {
            this.fillStyleComboBox.Items.Add(noneBrushText);
            this.fillStyleComboBox.Items.Add(solidBrushText);
            this.fillStyleComboBox.SelectedIndex = 0;
            this.Enabled = Surface != null && !Surface.ReadOnly;
            if (Surface != null)
            {
                string[] textArray1;
                textArray1 = Surface.Source.NamedHatchStyle.GetNames();
                Array.Sort<string>(textArray1);
                this.fillStyleComboBox.Items.AddRange(textArray1);

                textArray1 = Surface.Source.NamedTextureStyle.GetNames();
                Array.Sort<string>(textArray1);
                this.fillStyleComboBox.Items.AddRange(textArray1);

                Surface.Source.Loaded += new EventHandler(Source_Loaded);
                Surface.Source.GhostChanged += new EventHandler(Source_GhostChanged);
                if (Surface.Source.IsLoaded)
                    Source_Loaded(null, null);
            }
        }

        private Color? ChooseColor(Color initColor)
        {
            using (ColorDialog cd = new ColorDialog())
            {
                cd.Color = initColor;
                DialogResult dr = cd.ShowDialog();
                if (dr == DialogResult.Cancel)
                    return null;
                else
                    return cd.Color;
            }
        }

        private void ResetUI()
        {
            this.drawColorButton.Image = CurrentLineColorImage;
            this.drawColorButton.ImageTransparentColor = Color.Magenta;
            this.fillColorButton.Image = CurrentFillColorImage;
            this.fillColorButton.ImageTransparentColor = Color.Magenta;
            this.lineWidthComboBox.Text = Sssh.LineWidth.ToString();
            if (!Sssh.IsFillColor)
                this.fillStyleComboBox.SelectedItem = noneBrushText;
            else if (!Sssh.IsHatch)
                this.fillStyleComboBox.SelectedItem = solidBrushText;
            else
            {
                string name;
                name = Surface.Source.NamedHatchStyle.GetName(Sssh.Hatch);
                this.fillStyleComboBox.SelectedItem = name;
            }
        }

        private void RefreshLineButton()
        {
            this.drawColorButton.Image = CurrentLineColorImage;
            this.drawColorButton.ImageTransparentColor = Color.Magenta;
        }

        private void RefreshFillButton()
        {
            this.fillColorButton.Image = CurrentFillColorImage;
            this.fillColorButton.ImageTransparentColor = Color.Magenta;
        }

        #endregion

        #endregion

    }
}

