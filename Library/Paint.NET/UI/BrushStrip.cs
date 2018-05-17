using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DrectSoft.Basic.Paint.NET
{
    public class BrushStrip
        : ToolStrip
    {

        #region Fields
        private static readonly object _syncRoot = new object();
        private static int HatchImageWidth = 22;
        private ToolStripButton _noneBrush;
        private ToolStripButton _solidBrush;
        private ToolStripSeparator _separator;
        private List<ToolStripButton> _styleBrushList;
        private SurfaceStyleSettingHelper _sssh;
        #endregion

        #region Ctors

        public BrushStrip()
            : base()
        {
            InitializeComponent();
            _sssh = new SurfaceStyleSettingHelper();
        }

        private void InitializeComponent()
        {
            this._noneBrush = new ToolStripButton();
            this._solidBrush = new ToolStripButton();
            this._separator = new ToolStripSeparator();
            this._styleBrushList = new List<ToolStripButton>();

            base.SuspendLayout();

            this._noneBrush.Name = "noneBrush";
            this._noneBrush.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            this._noneBrush.Image = GetNoneBrushImage();
            this._noneBrush.Text = "透明";
            this._noneBrush.ToolTipText = "透明";

            this._solidBrush.Name = "solidBrush";
            this._solidBrush.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            this._solidBrush.Image = GetSolidBrushImage();
            this._solidBrush.Text = "实心";
            this._solidBrush.ToolTipText = "实心";

            this._separator.Name = "_separator";

            this.GripStyle = ToolStripGripStyle.Hidden;
            this.Items.Add(this._noneBrush);
            this.Items.Add(this._solidBrush);
            this.Items.Add(this._separator);
            this.ItemClicked += new ToolStripItemClickedEventHandler(BrushStrip_ItemClicked);

            base.ResumeLayout(false);
        }

        #endregion

        #region Members

        #region Event Handlers

        private void Source_Loaded(object sender, EventArgs e)
        {
            if (sender != null)
                this.SuspendLayout();
            RemoveStyleButton();
            AddStyleButton();
            if (sender != null)
                this.ResumeLayout();
        }

        private void Source_GhostChanged(object sender, EventArgs e)
        {
            SetStripStyle();
        }

        private void btn_Paint(object sender, PaintEventArgs e)
        {
            Rectangle clipRectangle = e.ClipRectangle;
            int hatchImageWidth = BrushStrip.HatchImageWidth;
            Point pt = clipRectangle.Location;
            Rectangle rect = new Rectangle(clipRectangle.X,
                clipRectangle.Y, hatchImageWidth, clipRectangle.Height);
            rect.X += 2;
            rect.Y += 2;
            rect.Width -= 4;
            rect.Height -= 4;
            ToolStripButton btn = sender as ToolStripButton;
            using (Brush br = UICommon.GetBrush(
                btn.Name, Color.Black, Color.White,
                Surface.Source.NamedHatchStyle,
                Surface.Source.NamedTextureStyle))
                e.Graphics.FillRectangle(br, rect);
        }

        private void BrushStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripButton btn;
            bool btnchecked;
            foreach (ToolStripItem item in this.Items)
            {
                btn = item as ToolStripButton;
                if (btn == null)
                    continue;
                btnchecked = btn == e.ClickedItem;
                btn.Checked = btnchecked;
                if (btnchecked)
                    SetSurfaceStyle(btn);
            }
        }

        #endregion

        #region Properties

        public IShapeSurface Surface
        {
            get { return Sssh.Surface; }
            set
            {
                if (DesignMode)
                    _sssh.Surface = value;
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

        #region Private

        private static Image GetNoneBrushImage()
        {
            Bitmap bmp = new Bitmap(16, 16);
            using (Graphics g = Graphics.FromImage(bmp))
                g.DrawRectangle(Pens.Black, 2, 2, 12, 12);
            return bmp;
        }

        private static Image GetSolidBrushImage()
        {
            Bitmap bmp = new Bitmap(16, 16);
            using (Graphics g = Graphics.FromImage(bmp))
                g.FillRectangle(Brushes.Black, 2, 2, 12, 12);
            return bmp;
        }

        private SurfaceStyleSettingHelper Sssh
        {
            get { return _sssh; }
        }

        private void AttachSurface()
        {
            this.Enabled = Surface != null && !Surface.ReadOnly;
            if (Surface != null)
            {
                Surface.Source.Loaded += new EventHandler(Source_Loaded);
                if (Surface.Source.IsLoaded)
                    Source_Loaded(null, null);
            }
        }

        private void DetachSurface()
        {
            if (Surface != null)
            {
                Surface.Source.Loaded -= new EventHandler(Source_Loaded);
                RemoveStyleButton();
            }
        }

        private void AddStyleButton()
        {
            Graphics g = Graphics.FromHwnd(this.Handle);
            string[] names;
            names = Surface.Source.NamedHatchStyle.GetNames();
            foreach (string name in names)
                CreateButton(g, name);
            names = Surface.Source.NamedTextureStyle.GetNames();
            foreach (string name in names)
                CreateButton(g, name);

            Surface.Source.GhostChanged += new EventHandler(Source_GhostChanged);
        }

        private void CreateButton(Graphics g, string name)
        {
            ToolStripButton btn;
            btn = new ToolStripButton();
            btn.Name = name;
            btn.Image = BrushStrip.TransparentImage;
            btn.Text = name;
            btn.ToolTipText = name;
            btn.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            btn.Width = BrushStrip.HatchImageWidth + 1 +
                (int)g.MeasureString(name, this.Font).Width;
            btn.Paint += new PaintEventHandler(btn_Paint);
            _styleBrushList.Add(btn);
            this.Items.Add(btn);
        }

        private void RemoveStyleButton()
        {
            Surface.Source.GhostChanged -= new EventHandler(Source_GhostChanged);
            foreach (ToolStripButton btn in _styleBrushList)
            {
                this.Items.Remove(btn);
                btn.Dispose();
            }
            _styleBrushList.Clear();
        }

        private void SetSurfaceStyle(ToolStripButton btn)
        {
            if (btn == this._noneBrush)
            {
                Sssh.IsFillColor = false;
                Sssh.IsHatch = false;
            }
            else if (btn == this._solidBrush)
            {
                Sssh.IsFillColor = true;
                Sssh.IsHatch = false;
                Sssh.IsTexture = false;
            }
            else
            {
                HatchStyle? hatch = Surface.Source.NamedHatchStyle.GetHatchStyle(btn.Name);
                if (hatch.HasValue)
                {
                    Sssh.IsFillColor = true;
                    Sssh.IsHatch = true;
                    Sssh.IsTexture = false;
                    Sssh.Hatch = hatch.Value;
                }
                else
                {
                    Sssh.IsFillColor = true;
                    Sssh.IsHatch = false;
                    Sssh.IsTexture = true;
                    Sssh.TextureName = btn.Name;
                }
            }
        }

        private void SetStripStyle()
        {
            ToolStripButton btn;
            if (!Sssh.IsFillColor)
            {
                btn = this._noneBrush;
            }
            else if (Sssh.IsHatch)
            {
                string name = Surface.Source.NamedHatchStyle.GetName(Sssh.Hatch);
                btn = GetButtonFromName(name);
            }
            else if (Sssh.IsTexture)
            {
                btn = GetButtonFromName(Sssh.TextureName);
            }
            else
            {
                btn = this._solidBrush;
            }
            SetCheckedButton(btn);
        }

        private void SetCheckedButton(ToolStripButton button)
        {
            bool btnchecked;
            foreach (ToolStripItem item in this.Items)
            {
                ToolStripButton btn = item as ToolStripButton;
                if (btn == null)
                    continue;
                btnchecked = btn == button;
                btn.Checked = btnchecked;
            }
        }

        private ToolStripButton GetButtonFromName(string name)
        {
            foreach (ToolStripItem item in this.Items)
                if (item.Name == name)
                    return item as ToolStripButton;
            return null;
        }

        #endregion

        #region Singleton Instance
        private static Image _transparentImage;
        private static Image TransparentImage
        {
            get
            {
                if (_transparentImage == null)
                    lock (_syncRoot)
                        if (_transparentImage == null)
                        {
                            _transparentImage = new Bitmap(16,
                                BrushStrip.HatchImageWidth);
                            Graphics g = Graphics.FromImage(_transparentImage);
                            g.FillRectangle(Brushes.Transparent,
                                new Rectangle(Point.Empty, _transparentImage.Size));
                        }
                return _transparentImage;
            }
        }
        #endregion

        #endregion

    }
}
