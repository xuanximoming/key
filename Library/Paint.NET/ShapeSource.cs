using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DrectSoft.Basic.Paint.NET
{
    public class ShapeSource
        : MarshalByRefObject, IShapeSource
    {

        #region Fields
        private Picture _background;
        private ShapeList _shapes;
        private Bitmap _current;
        private Graphics _graphics;
        private object _tag;
        private Size _size;
        private IGhost _ghost;
        private IShapeSelector _selector;
        private NamedHatchStyles _namedHatchStyle;
        private NamedTextureStyles _namedTextureStyle;
        private bool _isLoaded;
        private bool _dirty;
        private bool _drawing;
        private bool _currentChanged;
       private IShapeSurface _surface;
        #endregion

        #region Ctors

        public ShapeSource()
        {
            _ghost = NotAvailableGhost.Instance;
        }

        #endregion

        #region Other Members

        private Bitmap CurrentPrivate
        {
            get { return _current; }
            set
            {
                if (!object.ReferenceEquals(_current, value))
                {
                    _current = value;
                    _currentChanged = true;
                    if (!Drawing)
                        OnCurrentChanged(EventArgs.Empty);
                }
            }
        }

        private bool Drawing
        {
            get { return _drawing; }
            set
            {
                _drawing = value;
                if (!value && _currentChanged)
                    OnCurrentChanged(EventArgs.Empty);
            }
        }

        public IGhost GhostPrivate
        {
            get { return _ghost; }
            set
            {
                _ghost = value;
                OnGhostChanged(EventArgs.Empty);
            }
        }

        protected virtual void OnCurrentChanged(EventArgs args)
        {
            _currentChanged = false;
            if (CurrentChanged != null)
                CurrentChanged(this, args);
        }

        protected virtual void OnLoading(EventArgs args)
        {
            if (Loading != null)
                Loading(this, args);
        }

        protected virtual void OnLoaded(EventArgs args)
        {
            if (Loaded != null)
                Loaded(this, args);
        }

        protected virtual void OnGhostChanged(EventArgs args)
        {
            if (GhostChanged != null)
                GhostChanged(this, args);
        }

        private ShapeList ShapesPrivate
        {
            get { return _shapes; }
            set { _shapes = value; }
        }

        private Graphics G
        {
            get
            {
                if (_graphics == null && CurrentPrivate != null)
                {
                    _graphics = Graphics.FromImage(Current);
                    _graphics.SmoothingMode = SmoothingMode.AntiAlias;
                }
                return _graphics;
            }
        }

        private bool CheckSize(bool throwOnError)
        {
            bool result = Size.Width > 0 && Size.Height > 0;
            if (!result && throwOnError)
                throw new ArgumentOutOfRangeException("size",
                    "Height and Width must great than 0.");
            return result;
        }

        private void ClearCurrent()
        {
            if (_graphics != null)
            {
                _graphics.Dispose();
                _graphics = null;
            }
            if (CurrentPrivate != null)
            {
                CurrentPrivate.Dispose();
                CurrentPrivate = null;
            }
        }

        private void RedrawCore()
        {
            Bitmap newone;
            ClearCurrent();
            if (Background != null)
            {
                Image img = Background.GetImage();
                if (img == null)
                    newone = new Bitmap(Size.Width, Size.Height);
                else
                    newone = new Bitmap(img, Size);
            }
            else
            {
                newone = new Bitmap(Size.Width, Size.Height);
            }
            CurrentPrivate = newone;
            foreach (IShape shape in ShapesPrivate)
                DrawShapeCore(shape);
        }

        private void DrawShapeCore(IShape shape)
        {
            if (!shape.Data.Enable || !shape.Data.IsBackground)
                return;
            shape.Draw(G, NamedTextureStyle);
            Dirty = true;
        }

        public void Add(IShape shape)
        {
            CheckSize(true);
            AddImpl(shape);
        }

        public void Add(params IShape[] shapes)
        {
            CheckSize(true);
            Graphics g = Graphics.FromImage(Current);
            foreach (IShape shape in shapes)
                AddImpl(shape);
        }

        public void Add(IEnumerable<IShape> shapes)
        {
            CheckSize(true);
            foreach (IShape shape in shapes)
                AddImpl(shape);
        }

        private void AddImpl(IShape shape)
        {
            if (shape == null)
                return;
             //增加文本形状时交互
             if (shape is LabelShape)
                PreLabelModify(shape);

            shape.Data.Enable = true;
            ShapesPrivate.Add(shape);
            DrawShapeCore(shape);
        }

        #endregion

        #region IShapeSource Members

        public IList<IShape> Shapes
        {
            get { return ShapesPrivate.AsReadOnly(); }
        }

        public IShapeFactory Factory
        {
            get { return ShapeFactory.Instance; }
        }

       public IShapeSurface Surface
       {
          get { return _surface; }
          set { this._surface = value; }
       }

        public IGhost Ghost
        {
            get { return GhostPrivate; }
        }

        public IShapeSelector Selector
        {
            get
            {
                if (_selector == null)
                    _selector = new ShapeSelector(this);
                return _selector;
            }
        }

        public NamedHatchStyles NamedHatchStyle
        {
            get
            {
                if (_namedHatchStyle == null)
                    return DefaultNamedHatchStyles.Instance;
                return _namedHatchStyle;
            }
        }

        public NamedTextureStyles NamedTextureStyle
        {
            get
            {
                if (_namedTextureStyle == null)
                    return DefaultNamedTextureStyles.Instance;
                return _namedTextureStyle;
            }
        }

        public Picture Background
        {
            get { return _background; }
        }

        public Image Current
        {
            get { return CurrentPrivate; }
        }

        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public Size Size
        {
            get { return _size; }
        }

        public bool IsLoaded
        {
            get { return _isLoaded; }
        }

        public bool Dirty
        {
            get { return _dirty; }
            set { _dirty = value; }
        }

        public void Add(ShapeData data)
        {
            CheckSize(true);
            IShape shape = Factory.Create(data);
            AddImpl(shape);
            GhostPrivate.Shape.Data = data;
        }

        public void Modify(int index, ShapeData data)
        {
            CheckSize(true);
            IShape shape = ShapesPrivate[index];
            shape.Data = data;
            RedrawCore();
        }

        public void Remove()
        {
            CheckSize(true);
            ShapesPrivate.RemoveAt(ShapesPrivate.Count - 1);
            RedrawCore();
        }

        public void Enable(int index)
        {
            IShape shape = ShapesPrivate[index];
            shape.Data.Enable = true;
            if (Ghost.Shape.Equals(shape))
                Ghost.Shape.Data.Enable = true;
            RedrawCore();
        }

        public void Disable(int index)
        {
            IShape shape = ShapesPrivate[index];
            shape.Data.Enable = false;
            if (Ghost.Shape.Equals(shape))
                Ghost.Shape.Data.Enable = false;
            RedrawCore();
        }

        public void Select(int index)
        {
            CheckSize(true);
            IShape shape = ShapesPrivate.Select(index);
            GhostPrivate = shape.GetGhost();
            GhostPrivate.State = GhostState.Modity;
            RedrawCore();
        }

        public void Deselect()
        {
            CheckSize(true);
            if (GhostPrivate != null)
            {
                IShape shape = GhostPrivate.Shape;
                if (shape != null &&
                    shape.Data != null &&
                    shape.Data.Points != null)
                {
                    if (!shape.Completed &&
                        shape.Data.Points.Length >= shape.MinPoints &&
                        shape.Data.Points.Length <= shape.MaxPoints)
                    {
                        shape.Complete();
                    }
                }
                ShapesPrivate.Deselect(GhostPrivate.Shape);
                // careful: not same instance
                GhostPrivate.Shape.Data.IsBackground = true;
                DrawShapeCore(GhostPrivate.Shape);
                GhostPrivate = NotAvailableGhost.Instance;
                OnCurrentChanged(EventArgs.Empty);
            }
        }

        public void Load(Picture background, Size size,
            NamedHatchStyles namedHatchStyle,
            NamedTextureStyles namedTextureStyle,
            IEnumerable<IShape> shapes)
        {
            Clear();
            OnLoading(EventArgs.Empty);
            _size = size;
            CheckSize(true);
            _background = background;
            if (shapes == null)
                ShapesPrivate = new ShapeList();
            else
                ShapesPrivate = new ShapeList(shapes);
            _namedHatchStyle = namedHatchStyle;
            _namedTextureStyle = namedTextureStyle;
            RedrawCore();
            _isLoaded = true;
            OnLoaded(EventArgs.Empty);
        }

        public void Clear()
        {
            _isLoaded = false;
            ClearCurrent();
            _size = Size.Empty;
            _background = null;
            _shapes = null;
        }

        public void Redraw()
        {
            CheckSize(true);
            RedrawCore();
        }

        public void SetNewShape(ShapeData data)
        {
            GhostPrivate = Factory.Create(data).GetGhost();
            Dirty = true;
        }

        public void ResetGhost()
        {
            GhostPrivate = NotAvailableGhost.Instance;
            Dirty = true;
        }

        public event EventHandler CurrentChanged;

        public event EventHandler Loading;

        public event EventHandler Loaded;

        public event EventHandler GhostChanged;

       public void PreLabelModify(IShape shape)
       {
          ControlSurface surface = (ControlSurface)Surface;
          surface.TextEditor.Location =
             surface.RectangleToScreen(shape.Data.Bounds).Location + new Size(0, 2);
          surface.TextEditor.Size = shape.Data.Bounds.Size;
          surface.TextEditor.SetShapeValue((LabelShape)shape);
          surface.TextEditor.Activate();
          surface.TextEditor.Visible = true;
       }

        #endregion

    }
}
