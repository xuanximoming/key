using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DrectSoft.Resources;

namespace DrectSoft.Basic.Paint.NET {
    public class ShapeStrip
        : ToolStrip {

        #region Fields
        private ToolStripButton _pointer;
        private ToolStripButton _label;
        private ToolStripButton _line;
        private ToolStripButton _lines;
        private ToolStripButton _rectangle;
        private ToolStripButton _ellipse;
        private ToolStripButton _bezier;
        private ToolStripButton _curve;
        private ToolStripButton _closedCurve;
        private ToolStripButton _polygon;
        private IShapeSurface _surface;
        #endregion

        #region Ctors

        public ShapeStrip() {
            Init();
        }

        private void Init() {
            this._pointer = new ToolStripButton();
            this._label = new ToolStripButton();
            this._line = new ToolStripButton();
            this._lines = new ToolStripButton();
            this._rectangle = new ToolStripButton();
            this._ellipse = new ToolStripButton();
            this._bezier = new ToolStripButton();
            this._curve = new ToolStripButton();
            this._closedCurve = new ToolStripButton();
            this._polygon = new ToolStripButton();
            this.SuspendLayout();

            // 
            // Pointer
            // 
            this._pointer.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._pointer.Image = ResourceManager.GetSmallIcon(ResourceNames.Point, IconType.Normal);
            this._pointer.ImageTransparentColor = Color.Magenta;
            this._pointer.Name = "Pointer";
            this._pointer.Size = new Size(23, 22);
            this._pointer.Text = "选定";
            this._pointer.Click += new EventHandler(CheckedChanged);
            this._label.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._label.Image = ResourceManager.GetSmallIcon(ResourceNames.Information, IconType.Normal);
            this._label.ImageTransparentColor = Color.Magenta;
            this._label.Name = "Label";
            this._label.Size = new Size(23, 22);
            this._label.Text = "文本";
            this._label.Click += new EventHandler(CheckedChanged);
            // 
            // Line
            // 
            this._line.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._line.Image = ResourceManager.GetSmallIcon(ResourceNames.Line, IconType.Normal);
            this._line.ImageTransparentColor = Color.Magenta;
            this._line.Name = "Line";
            this._line.Size = new Size(23, 22);
            this._line.Text = "直线";
            this._line.Click += new EventHandler(CheckedChanged);
            // 
            // Lines
            // 
            this._lines.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._lines.Image = ResourceManager.GetSmallIcon("Pencil", IconType.Normal);
            this._lines.ImageTransparentColor = Color.Magenta;
            this._lines.Name = "Lines";
            this._lines.Size = new Size(23, 22);
            this._lines.Text = "铅笔";
            this._lines.Click += new EventHandler(CheckedChanged);
            // 
            // Rectangle
            // 
            this._rectangle.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._rectangle.Image =ResourceManager.GetSmallIcon("Rectangle", IconType.Normal);
            this._rectangle.ImageTransparentColor = Color.Magenta;
            this._rectangle.Name = "Rectangle";
            this._rectangle.Size = new Size(23, 22);
            this._rectangle.Text = "矩形";
            this._rectangle.Click += new EventHandler(CheckedChanged);
            // 
            // Ellipse
            // 
            this._ellipse.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._ellipse.Image =ResourceManager.GetSmallIcon(ResourceNames.Ellipse, IconType.Normal);
            this._ellipse.ImageTransparentColor = Color.Magenta;
            this._ellipse.Name = "Ellipse";
            this._ellipse.Size = new Size(23, 22);
            this._ellipse.Text = "椭圆";
            this._ellipse.Click += new EventHandler(CheckedChanged);
            // 
            // Bezier
            // 
            this._bezier.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._bezier.Image =ResourceManager.GetSmallIcon(ResourceNames.Bezier, IconType.Normal);
            this._bezier.ImageTransparentColor = Color.Magenta;
            this._bezier.Name = "Bezier";
            this._bezier.Size = new Size(23, 22);
            this._bezier.Text = "Bezier";
            this._bezier.Visible = false;
            this._bezier.Click += new EventHandler(CheckedChanged);
            // 
            // Curve
            // 
            this._curve.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._curve.Image =ResourceManager.GetSmallIcon(ResourceNames.Curve, IconType.Normal);
            this._curve.ImageTransparentColor = Color.Magenta;
            this._curve.Name = "Curve";
            this._curve.Size = new Size(23, 22);
            this._curve.Text = "曲线";
            this._curve.Click += new EventHandler(CheckedChanged);
            // 
            // ClosedCurve
            // 
            this._closedCurve.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._closedCurve.Image = ResourceManager.GetSmallIcon(ResourceNames.ClosedCurve, IconType.Normal);
            this._closedCurve.ImageTransparentColor = Color.Magenta;
            this._closedCurve.Name = "ClosedCurve";
            this._closedCurve.Size = new Size(23, 22);
            this._closedCurve.Text = "封闭曲线";
            this._closedCurve.Click += new EventHandler(CheckedChanged);
            // 
            // Polygon
            // 
            this._polygon.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._polygon.Image =ResourceManager.GetSmallIcon(ResourceNames.Polygon, IconType.Normal);
            this._polygon.ImageTransparentColor = Color.Magenta;
            this._polygon.Name = "Polygon";
            this._polygon.Size = new Size(23, 22);
            this._polygon.Text = "多边形";
            this._polygon.Click += new EventHandler(CheckedChanged);

            // 
            // this
            // 
            this.Dock = DockStyle.None;
            this.GripStyle = ToolStripGripStyle.Hidden;
            this.AutoSize = false;
            this.Size = new Size(50, 98);
            this.LayoutStyle = ToolStripLayoutStyle.Flow;
            (this.LayoutSettings as FlowLayoutSettings).FlowDirection = FlowDirection.LeftToRight;
            (this.LayoutSettings as FlowLayoutSettings).WrapContents = true;
            this.Items.AddRange(new ToolStripItem[]
            {
                this._pointer,this._label, this._lines, this._line, this._curve,
                this._rectangle, this._ellipse, this._closedCurve, this._polygon,
                this._bezier
            });

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        #region Members

        #region Properties

        public IShapeSurface Surface {
            get { return _surface; }
            set {
                if (DesignMode)
                    _surface = value;
                else {
                    this.SuspendLayout();
                    DetachSurface();
                    _surface = value;
                    AttachSurface();
                    this.ResumeLayout();
                }
            }
        }

        #endregion

        #region Event Handlers

        private void CheckedChanged(object sender, EventArgs e) {
            ToolStripButton btn = sender as ToolStripButton;
            CheckButton(btn);
            ShapeType type = GetShapeType(btn);
            SetSurfaceType(type);
        }

        private void Source_GhostChanged(object sender, EventArgs e) {
            if (Surface == null)
                return;
            ShapeType type = Surface.Source.Ghost.Shape.Data.Type;
            SetStripType(type);
        }

        #endregion

        #region Privates

        private void CheckButton(ToolStripButton btn) {
            foreach (ToolStripButton b in this.Items)
                b.Checked = b == btn;
        }

        private ShapeType GetShapeType(ToolStripButton btn) {
            if (btn == this._label)
                return ShapeType.Label;
            else if (btn == this._line)
                return ShapeType.Line;
            else if (btn == this._lines)
                return ShapeType.Lines;
            else if (btn == this._rectangle)
                return ShapeType.Rectangle;
            else if (btn == this._ellipse)
                return ShapeType.Ellipse;
            else if (btn == this._bezier)
                return ShapeType.Bezier;
            else if (btn == this._curve)
                return ShapeType.Curve;
            else if (btn == this._closedCurve)
                return ShapeType.ClosedCurve;
            else if (btn == this._polygon)
                return ShapeType.Polygon;
            else
                return ShapeType.None;
        }

        private void SetSurfaceType(ShapeType type) {
            if (Surface != null &&
                Surface.Source.Ghost.Shape.Data.Type != type) {
                Surface.NewGhost(type);
            }
        }

        private void SetStripType(ShapeType type) {
            switch (type) {
                case ShapeType.Line:
                    CheckButton(this._line);
                    return;
                case ShapeType.Lines:
                    CheckButton(this._lines);
                    return;
                case ShapeType.Rectangle:
                    CheckButton(this._rectangle);
                    return;
                case ShapeType.Ellipse:
                    CheckButton(this._ellipse);
                    return;
                case ShapeType.Bezier:
                    CheckButton(this._bezier);
                    return;
                case ShapeType.Curve:
                    CheckButton(this._curve);
                    return;
                case ShapeType.ClosedCurve:
                    CheckButton(this._closedCurve);
                    return;
                case ShapeType.Polygon:
                    CheckButton(this._polygon);
                    return;
                case ShapeType.None:
                default:
                    CheckButton(this._pointer);
                    return;
            }
        }

        private void AttachSurface() {
            bool surfaceNotNull = Surface != null;
            this.Enabled = surfaceNotNull && !Surface.ReadOnly;
            if (surfaceNotNull) {
                _surface.Source.GhostChanged += new EventHandler(Source_GhostChanged);
                Source_GhostChanged(null, null);
            }
        }

        private void DetachSurface() {
            if (_surface != null)
                _surface.Source.GhostChanged -= new EventHandler(Source_GhostChanged);
        }

        #endregion

        #endregion

    }
}
