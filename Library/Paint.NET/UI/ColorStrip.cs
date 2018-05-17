using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DrectSoft.Resources;

namespace DrectSoft.Basic.Paint.NET
{
    public class ColorStrip
        : ToolStrip
    {

        #region Fields
        private static Dictionary<string, Color> _colors;
        private SurfaceStyleSettingHelper _sssh;
        private Image _orgLineColorImage;
        private Image _currentLineColorImage;
        private Color _currentLineColor;
        private Image _orgFillColorImage;
        private Image _currentFillColorImage;
        private Color _currentFillColor;
        private ToolStripDropDownButton drawColorButton;
        private ToolStripDropDownButton fillColorButton;
        #endregion

        #region Ctors

        public ColorStrip()
        {
            _sssh = new SurfaceStyleSettingHelper();
            Init();
        }

        private void Init()
        {
            SuspendLayout();
            this.drawColorButton = new ToolStripDropDownButton();
            this.fillColorButton = new ToolStripDropDownButton();

            this.drawColorButton.AutoSize = false;
            this.drawColorButton.DropDownDirection = ToolStripDropDownDirection.AboveRight;
            this.drawColorButton.Name = "drawColorButton";
            this.drawColorButton.Image = null;
            this.drawColorButton.ImageTransparentColor = Color.Maroon;
            this.drawColorButton.Size = new Size(48, 18);
            this.drawColorButton.ToolTipText = "线条颜色";

            this.fillColorButton.AutoSize = false;
            this.fillColorButton.DropDownDirection = ToolStripDropDownDirection.AboveRight;
            this.fillColorButton.Name = "fillColorButton";
            this.fillColorButton.Image = null;
            this.fillColorButton.ImageTransparentColor = Color.Maroon;
            this.fillColorButton.Size = new Size(48, 18);
            this.fillColorButton.ToolTipText = "填充颜色";

            this.AutoSize = false;
            this.Dock = DockStyle.None;
            this.GripStyle = ToolStripGripStyle.Hidden;
            this.Items.Add(this.drawColorButton);
            this.Items.Add(this.fillColorButton);
            this.Size = new Size(50, 50);
            this.LayoutStyle = ToolStripLayoutStyle.Flow;
            (this.LayoutSettings as FlowLayoutSettings).FlowDirection = FlowDirection.LeftToRight;
            (this.LayoutSettings as FlowLayoutSettings).WrapContents = true;

            AddColor(this.drawColorButton);
            AddColor(this.fillColorButton);
            ResetUI();
            ResumeLayout(false);
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

        #endregion

        #region Event Handlers

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
            this.SuspendLayout();
            ResetUI();
            this.ResumeLayout();
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
                    colors.Add("透明", Color.Transparent);
                    colors.Add("红色", Color.Red);
                    colors.Add("蓝色", Color.Blue);
                    colors.Add("绿色", Color.Green);
                    colors.Add("黄色", Color.Yellow);
                    colors.Add("紫色", Color.Purple);
                    colors.Add("粉色", Color.Pink);
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
            if (Surface != null)
            {
                Surface.Source.Loaded -= new EventHandler(Source_Loaded);
                Surface.Source.GhostChanged -= new EventHandler(Source_GhostChanged);
            }
        }

        private void AttachSurface()
        {
            this.Enabled = Surface != null && !Surface.ReadOnly;
            if (Surface != null)
            {
                Surface.Source.Loaded += new EventHandler(Source_Loaded);
                Surface.Source.GhostChanged += new EventHandler(Source_GhostChanged);
                if (Surface.Source.IsLoaded)
                    Source_Loaded(null, null);
            }
        }

        private void ResetUI()
        {
            RefreshLineButton();
            RefreshFillButton();
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
