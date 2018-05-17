using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DrectSoft.Basic.Paint.NET
{
    public class LineWidthStrip
        : ToolStrip
    {

        #region Fields
        private readonly int[] LineWidth = new int[] { 1, 2, 3, 5, 8 };
        private readonly int ImageWidth = 45;
        private readonly int ImageHeight = 12;
        private SurfaceStyleSettingHelper _sssh;
        #endregion

        #region Ctors

        public LineWidthStrip()
            : base()
        {
            this.SuspendLayout();
            this.Dock = DockStyle.None;
            this.GripStyle = ToolStripGripStyle.Hidden;
            this.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.ImageScalingSize = new Size(ImageWidth, ImageHeight);
            _sssh = new SurfaceStyleSettingHelper();
            InitButtons();
            ResetUI();
            this.ResumeLayout(false);
        }

        private void InitButtons()
        {
            ToolStripButton btn;
            Bitmap bmp;
            int lineTop = ImageHeight >> 1;
            string wText;
            foreach (int w in LineWidth)
            {
                bmp = new Bitmap(ImageWidth, ImageHeight);
                using (Graphics g = Graphics.FromImage(bmp))
                using (Pen p = new Pen(Color.Black, w))
                {
                    g.DrawLine(p, 1, lineTop, ImageWidth - 2, lineTop);
                }
                wText = w.ToString();
                btn = new ToolStripButton();
                btn.DisplayStyle = ToolStripItemDisplayStyle.Image;
                btn.Image = bmp;
                btn.ImageTransparentColor = Color.White;
                btn.Name = "LineWidth" + wText;
                btn.Tag = w;
                btn.ToolTipText = "Ïß¿í" + wText;
                btn.Click += new EventHandler(btn_Click);
                this.Items.Add(btn);
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

        private void btn_Click(object sender, EventArgs e)
        {
            ToolStripButton btn = sender as ToolStripButton;
            SuspendLayout();
            Sssh.LineWidth = (int)btn.Tag;
            foreach (ToolStripButton b in this.Items)
                b.Checked = b == btn;
            ResumeLayout();
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

        private SurfaceStyleSettingHelper Sssh
        {
            get { return _sssh; }
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
            int w = (int)Sssh.LineWidth;
            foreach (ToolStripButton b in this.Items)
                b.Checked = w == (int)b.Tag;
        }

        #endregion

        #endregion

    }
}
