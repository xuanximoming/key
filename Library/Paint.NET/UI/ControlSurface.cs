using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace DrectSoft.Basic.Paint.NET
{
    public class ControlSurface
        :UserControl, IShapeSurface
    {

        #region Fields
        private IShapeSource _source;
        private IActionBuilder _builder;
        private Stack<IAction> _undoList;
        private Stack<IAction> _redoList;
        private SurfaceMouseModifyHelper _smmh;
        private SurfaceMouseNewHelper _smnh;
        private Color _surfaceDrawColor;
        private Color _surfaceFillColor;
        private bool _surfaceIsFillColor;
        private HatchStyle _surfaceHatch;
        private bool _surfaceIsHatch;
        private float _surfaceLineWidth;
        private bool _surfaceIsTexture;
        private string _surfaceTexture;
        private bool _readOnly;
       private TextEditorForm _textEditor;
        #endregion

        #region Ctors

        public ControlSurface()
        {
            _undoList = new Stack<IAction>();
            _redoList = new Stack<IAction>();
            _smmh = new SurfaceMouseModifyHelper(this);
            _smnh = new SurfaceMouseNewHelper(this);
            _surfaceDrawColor = Color.Black;
            _surfaceFillColor = Color.Black;
            _surfaceIsFillColor = false;
            _surfaceHatch = HatchStyle.Percent50;
            _surfaceIsHatch = false;
            _surfaceLineWidth = 1f;
            Source = new ShapeSource();
            base.DoubleBuffered = true;
        }

        #endregion

        #region IShapeSurface Members

        public IShapeSource Source
        {
            get { return _source; }
            set
            {
                if (!object.ReferenceEquals(_source, value))
                {
                    UnbindSource(_source);
                    _source = value;
                    _source.Surface = this;
                    BindSource(value);
                }
            }
        }
        
        public IActionBuilder Builder
        {
            get
            {
                if (_builder == null)
                    _builder = new ActionBuilder(Source);
                return _builder;
            }
        }

        public bool ReadOnly
        {
            get { return _readOnly; }
            set { _readOnly = value; }
        }

        public int CurrentStep
        {
            get { return _undoList.Count; }
        }

        public int MaxStep
        {
            get { return _undoList.Count + _redoList.Count; }
        }

        public Color SurfaceDrawColor
        {
            get { return _surfaceDrawColor; }
            set { _surfaceDrawColor = value; }
        }

        public Color SurfaceFillColor
        {
            get { return _surfaceFillColor; }
            set { _surfaceFillColor = value; }
        }

        public bool SurfaceIsFillColor
        {
            get { return _surfaceIsFillColor; }
            set { _surfaceIsFillColor = value; }
        }

        public HatchStyle SurfaceHatch
        {
            get { return _surfaceHatch; }
            set { _surfaceHatch = value; }
        }

        public bool SurfaceIsHatch
        {
            get { return _surfaceIsHatch; }
            set { _surfaceIsHatch = value; }
        }

        public bool SurfaceIsTexture
        {
            get { return _surfaceIsTexture; }
            set { _surfaceIsTexture = value; }
        }

        public string SurfaceTexture
        {
            get { return _surfaceTexture; }
            set { _surfaceTexture = value; }
        }

        public float SurfaceLineWidth
        {
            get { return _surfaceLineWidth; }
            set { _surfaceLineWidth = value; }
        }

       public TextEditorForm TextEditor
       {
          get
          {
             if (null == _textEditor)
             {
                _textEditor = new TextEditorForm(Source);
                _textEditor.StartPosition = FormStartPosition.Manual;
                _textEditor.Visible = false;
             }
             return _textEditor;
          }
       }

        public void Clear()
        {
            NewGhost(ShapeType.None);
            _undoList.Clear();
            _redoList.Clear();
            Source.Clear();
        }

        public void NewGhost(ShapeType type)
        {
            IAction action;
            if (ReadOnly)
                return;
            ProcessOldGhost();
            if (type == ShapeType.None)
            {
                if (Source.Ghost.State == GhostState.NotAvailable)
                    return;
                action = Builder.Build(ActionBuildType.Deselect, null);
            }
            else
            {
                ShapeData data = new ShapeData(type);
                data.DrawColor = SurfaceDrawColor;
                data.FillColor = SurfaceFillColor;
                data.IsFillColor = SurfaceIsFillColor;
                data.Hatch = SurfaceHatch;
                data.IsHatch = SurfaceIsHatch;
                data.IsTexture = SurfaceIsTexture;
                data.TextureName = SurfaceTexture;
                data.LineWidth = SurfaceLineWidth;
                IShape shape = Source.Factory.Create(data);
                action = Builder.Build(
                    ActionBuildType.CreateNewShape, shape);
            }
            _redoList.Clear();
            ActionExecutor.Execute(action, Source, false);
            _undoList.Push(action);
        }

        public void Redo()
        {
            if (ReadOnly)
                return;
            RedoAction();
            if (Source.Dirty)
                source_CurrentChanged(null, null);
        }

        public void Undo()
        {
            if (ReadOnly)
                return;
            UndoAction();
            if (Source.Dirty)
                source_CurrentChanged(null, null);
        }

        #region Event CurrentStepChanged
        private static readonly object EventKey_CurrentStepChanged = new object();

        protected virtual void OnCurrentStepChanged(EventArgs e)
        {
            EventHandler handler =
                base.Events[EventKey_CurrentStepChanged] as EventHandler;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler CurrentStepChanged
        {
            add { base.Events.AddHandler(EventKey_CurrentStepChanged, value); }
            remove { base.Events.RemoveHandler(EventKey_CurrentStepChanged, value); }
        }
        #endregion

        #region Event MaxStepChanged
        private static readonly object EventKey_MaxStepChanged = new object();

        protected virtual void OnMaxStepChanged(EventArgs e)
        {
            EventHandler handler =
                base.Events[EventKey_MaxStepChanged] as EventHandler;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler MaxStepChanged
        {
            add { base.Events.AddHandler(EventKey_MaxStepChanged, value); }
            remove { base.Events.RemoveHandler(EventKey_MaxStepChanged, value); }
        }
        #endregion

        #endregion

        #region Overrides

        protected override void OnMouseDown(MouseEventArgs e)
        {
            ProcessMouse(e.Location, e.Button, MouseStates.MouseDown);
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            ProcessMouse(e.Location, e.Button, MouseStates.MouseMove);
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            ProcessMouse(e.Location, e.Button, MouseStates.MouseUp);
            base.OnMouseUp(e);
        }

		  protected override void OnDoubleClick(EventArgs e)
		  {
           LabelShape shape = Source.Ghost.Shape as LabelShape;
			  if (GhostState == GhostState.Modity
				  && shape !=null)
			  {
              ((ShapeSource)Source).PreLabelModify(shape);
			  }
			  base.OnDoubleClick(e);
		  }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.White,
                new Rectangle(Point.Empty, this.Size));
            if (this.BackgroundImage != null)
                e.Graphics.DrawImage(this.BackgroundImage, new Rectangle(Point.Empty, Source.Size));
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if (Source != null &&
                Source.Ghost.State != GhostState.NotAvailable)
                Source.Ghost.Draw(pe.Graphics, Source.NamedTextureStyle);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!ReadOnly)
            {
                if (keyData == Keys.Delete)
                {
                    switch (GhostState)
                    {
                        case GhostState.New:
                            Source.Ghost.Shape.Reset();
                            return true;
                        case GhostState.Modity:
                            IAction action = Builder.Build(
                                ActionBuildType.Delete, Source.Ghost.Shape);
                            NewAction(action);
                            return true;
                        case GhostState.NotAvailable:
                        default:
                            break;
                    }
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion

        #region Private

        private GhostState GhostState
        {
            get { return Source.Ghost.State; }
        }

        private void UnbindSource(IShapeSource source)
        {
            if (source != null)
            {
                source.CurrentChanged -= new EventHandler(source_CurrentChanged);
                source.Loading -= new EventHandler(source_Loading);
                source.Loaded -= new EventHandler(source_Loaded);
                source.GhostChanged -= new EventHandler(source_GhostChanged);
            }
        }

        private void BindSource(IShapeSource source)
        {
            if (source != null)
            {
                source.Loading += new EventHandler(source_Loading);
                source.Loaded += new EventHandler(source_Loaded);
                source.GhostChanged += new EventHandler(source_GhostChanged);
                if (source.IsLoaded)
                    source.CurrentChanged += new EventHandler(source_CurrentChanged);
            }
        }

        private void ProcessMouse(Point pt,
            MouseButtons button, MouseStates mouseState)
        {
            if (ReadOnly)
                return;
            IAction action = null;
            ActionBuildType type = ActionBuildType.None;
            SetCursor(pt, button, mouseState);
            switch (GhostState)
            {
                case GhostState.New:
                    switch (mouseState)
                    {
                        case MouseStates.MouseDown:
                            type = _smnh.ProcessMouseDown(pt, button);
                            break;
                        case MouseStates.MouseMove:
                            type = _smnh.ProcessMouseMove(pt, button);
                            break;
                        case MouseStates.MouseUp:
                            type = _smnh.ProcessMouseUp(pt, button);
                            break;
                        default:
                            break;
                    }
                    if (type != ActionBuildType.None)
                        action = Builder.Build(type, Source.Ghost.Shape);
                    break;
                case GhostState.Modity:
                    switch (mouseState)
                    {
                        case MouseStates.MouseDown:
                            type = _smmh.ProcessMouseDown(pt, button);
                            break;
                        case MouseStates.MouseMove:
                            type = _smmh.ProcessMouseMove(pt, button);
                            break;
                        case MouseStates.MouseUp:
                            type = _smmh.ProcessMouseUp(pt, button);
                            break;
                        default:
                            break;
                    }
                    if (type != ActionBuildType.None)
                        action = Builder.Build(type, Source.Ghost.Shape);
                    break;
                case GhostState.NotAvailable:
                    if (mouseState == MouseStates.MouseDown)
                    {
                        if (button == MouseButtons.Right)
                        {
                            _source.Selector.AdvanceSelect(pt, new ShapeSelectorCallback(ShapeSelector_Callback));
                        }
                        else if (button == MouseButtons.Left)
                        {
                            int index;
                            type = _source.Selector.Select(pt, out index);
                            if (type != ActionBuildType.None)
                                action = Builder.Build(type, Source.Shapes[index]);
                        }
                    }
                    break;
                default:
                    break;
            }
            NewAction(action);
            if (Source.Ghost != null && Source.Ghost.Dirty)
                this.Invalidate();
        }

        private void NewAction(IAction action)
        {
            if (ReadOnly)
                return;
            if (action != null)
            {
                _redoList.Clear();
                ActionExecutor.Execute(action, Source, false);
                _undoList.Push(action);
                if (action.Action == ActionType.NewShape ||
                    action.Action == ActionType.ModifyShape ||
                    action.Action == ActionType.DeleteShape)
                {
                    action = Builder.Build(ActionBuildType.Deselect, null);
                    ActionExecutor.Execute(action, Source, false);
                    _undoList.Push(action);
                }
            }
        }

        private void UndoAction()
        {
            if (_undoList.Count == 0)
                return;
            IAction action = _undoList.Pop();
            ActionExecutor.Execute(action, Source, true);
            _redoList.Push(action);
        }

        private void RedoAction()
        {
            if (_redoList.Count == 0)
                return;
            IAction action = _redoList.Pop();
            ActionExecutor.Execute(action, Source, false);
            _undoList.Push(action);
        }

        private void SetCursor(Point pt,
            MouseButtons button, MouseStates state)
        {
            if (ReadOnly)
                return;
            if (state != MouseStates.MouseMove)
                return;
            if (button != MouseButtons.None)
                return;
            if (GhostState == GhostState.NotAvailable)
            {
                this.Cursor = Cursors.Arrow;
                return;
            }
            if (GhostState == GhostState.New)
            {
                this.Cursor = Cursors.Cross;
                return;
            }
            if (Source.Ghost.Shape.Data.Bounds.Contains(pt))
            {
                int index = Source.Ghost.GetPointIndex(pt, GhostState);
                if (index == -1)
                {
                    this.Cursor = Cursors.SizeAll;
                    return;
                }
                this.Cursor = Cursors.Cross;
                return;
            }
            this.Cursor = Cursors.Arrow;
            return;
        }

        private void ProcessOldGhost()
        {
            IGhost ghost = Source.Ghost;
            IAction action;
            switch (ghost.State)
            {
                case GhostState.New:
                    if (!ghost.Shape.Completed)
                    {
                        Point[] pts = ghost.Shape.Data.Points;
                        if (pts != null &&
                            pts.Length >= ghost.Shape.MinPoints &&
                            pts.Length <= ghost.Shape.MaxPoints)
                        {
                            ghost.Shape.Complete();
                        }
                    }
                    if (ghost.Shape.Completed)
                    {
                        action = Builder.Build(ActionBuildType.AddNew, ghost.Shape);
                        NewAction(action);
                    }
                    return;
                case GhostState.Modity:
                    int index = Source.Shapes.IndexOf(ghost.Shape);
                    action = Builder.Build(ActionBuildType.Motify, ghost.Shape);
                    NewAction(action);
                    return;
                case GhostState.NotAvailable:
                default:
                    return;
            }
        }

        private void ShapeSelector_Callback(ActionBuildType type, int index)
        {
            if (type == ActionBuildType.None)
                return;
            IAction action = Builder.Build(type, Source.Shapes[index]);
            NewAction(action);
        }

        #endregion

        #region Event Handlers

        private void source_CurrentChanged(object sender, EventArgs e)
        {
            this.SuspendLayout();
            this.BackgroundImage = null;
            this.BackgroundImage = _source.Current;
            this.ResumeLayout();
        }

        private void source_Loading(object sender, EventArgs e)
        {
            Source.CurrentChanged -= new EventHandler(source_CurrentChanged);
        }

        private void source_Loaded(object sender, EventArgs e)
        {
            _undoList.Clear();
            _redoList.Clear();
            Source.CurrentChanged += new EventHandler(source_CurrentChanged);
            source_CurrentChanged(null, null);
        }

        private void source_GhostChanged(object sender, EventArgs e)
        {
            Source.Ghost.RequestRedraw -= new EventHandler(Ghost_RequestRedraw);
            Source.Ghost.RequestRedraw += new EventHandler(Ghost_RequestRedraw);
        }

        private void Ghost_RequestRedraw(object sender, EventArgs e)
        {
            if (Source.IsLoaded)
                this.Invalidate();
        }

        #endregion

        private void InitializeComponent() {
            this.SuspendLayout();
            // 
            // ControlSurface
            // 
            this.Name = "ControlSurface";
            this.Size = new System.Drawing.Size(153, 150);
            this.ResumeLayout(false);

        }

    }
}
