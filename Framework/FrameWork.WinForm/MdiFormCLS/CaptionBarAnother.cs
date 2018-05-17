#region Using directives

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using DrectSoft.Resources;

#endregion

namespace DrectSoft.FrameWork.WinForm
{
    /// <summary>
    /// 窗口标题栏
    /// </summary>
    public partial class CaptionBarAnother : ToolStrip
    {
        #region private variable

        private bool _minButtonVisible;
        private bool _maxButtonVisible;
        private string _caption;
        private ToolStripMenuItem _maxToolStripItem;
        private ToolStripMenuItem _minToolStripItem;
        private ToolStripMenuItem _closeToolStripItem;
        private ToolStripLabel _captionLabel;

        private const string CloseIconName = ResourceNames.CloseWindow;
        private const string MaxIconName = ResourceNames.Maximize;
        private const string MinIconName = ResourceNames.Minimize;

        private Image _closeIcon;
        private Image _restoreIcon;
        private Image _maxIcon;
        private Image _minIcon;
        private Image _formIcon;

        #endregion

        /// <summary>
        /// 最小化按钮是否显示
        /// </summary>
        public bool MinButtonVisible
        {
            get { return _minButtonVisible; }
            set
            {
                _minButtonVisible = value;
                if (_minToolStripItem != null) _minToolStripItem.Visible = value;
            }
        }

        /// <summary>
        /// 最大化按钮是否显示
        /// </summary>
        public bool MaxButtonVisible
        {
            get { return _maxButtonVisible; }
            set
            {
                _maxButtonVisible = value;
                if (_maxToolStripItem != null) _maxToolStripItem.Visible = value;
            }
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Caption
        {
            get { return _caption; }
            set
            {
                _caption = value;
                if (_captionLabel != null) _captionLabel.Text = value;
            }
        }

        private void toolStripMenuItemClose_Click(object sender, EventArgs e)
        {
            this.FindForm().Close();
        }

        private void toolStripMenuItemMin_Click(object sender, EventArgs e)
        {
            this.FindForm().WindowState = FormWindowState.Minimized;
        }

        private void toolStripMenuItemMax_Click(object sender, EventArgs e)
        {
            if (!_maxButtonVisible) return;
            if (this.FindForm().WindowState == FormWindowState.Maximized)
            {
                _maxToolStripItem.Image = _maxIcon;
                this.FindForm().WindowState = FormWindowState.Normal;
            }
            else
            {
                _maxToolStripItem.Image = _restoreIcon;
                this.FindForm().WindowState = FormWindowState.Maximized;
            }
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public CaptionBarAnother()
        {
            InitializeComponent();

            this.AllowMerge = false;

            _minButtonVisible = true;
            _maxButtonVisible = true;

            try
            {
                _closeIcon = ResourceManager.GetSmallIcon(CloseIconName, IconType.Normal);
                _restoreIcon = ResourceManager.GetSmallIcon(MaxIconName, IconType.Disable);
                _maxIcon = ResourceManager.GetSmallIcon(MaxIconName, IconType.Highlight);
                _minIcon = ResourceManager.GetSmallIcon(MinIconName, IconType.Normal);
                _formIcon = ResourceManager.GetNormalIcon(ResourceNames.IconDrectSoftLogoLarge);
            }
            catch
            {
            }

            _closeToolStripItem = new ToolStripMenuItem();
            _closeToolStripItem.Name = "closeToolStripItem";
            _closeToolStripItem.Text = "X";
            _closeToolStripItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
            _closeToolStripItem.Click += new EventHandler(toolStripMenuItemClose_Click);
            _closeToolStripItem.Alignment = ToolStripItemAlignment.Right;
            _closeToolStripItem.Size = new Size(24, 24);
            if (_closeIcon != null)
            {
                _closeToolStripItem.ImageTransparentColor = Color.Magenta;
                _closeToolStripItem.Image = _closeIcon;
            }

            _maxToolStripItem = new ToolStripMenuItem();
            _maxToolStripItem.Name = "maxToolStripItem";
            _maxToolStripItem.Text = "[]";
            _maxToolStripItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
            _maxToolStripItem.Click += new EventHandler(toolStripMenuItemMax_Click);
            _maxToolStripItem.Alignment = ToolStripItemAlignment.Right;
            _maxToolStripItem.Size = new Size(24, 24);
            if (_maxIcon != null)
            {
                _maxToolStripItem.ImageTransparentColor = Color.Magenta;
                _maxToolStripItem.Image = _maxIcon;
            }

            _minToolStripItem = new ToolStripMenuItem();
            _minToolStripItem.Name = "minToolStripItem";
            _minToolStripItem.Text = "_";
            _minToolStripItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
            _minToolStripItem.Click += new EventHandler(toolStripMenuItemMin_Click);
            _minToolStripItem.Alignment = ToolStripItemAlignment.Right;
            _minToolStripItem.Size = new Size(24, 24);
            if (_minIcon != null)
            {
                _minToolStripItem.ImageTransparentColor = Color.Magenta;
                _minToolStripItem.Image = _minIcon;
            }

            _minToolStripItem.Visible = _minButtonVisible;
            _maxToolStripItem.Visible = _maxButtonVisible;

            _captionLabel = new ToolStripLabel();
            _captionLabel.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            _captionLabel.TextImageRelation = TextImageRelation.ImageBeforeText;
            _captionLabel.Text = _caption;
            _captionLabel.Alignment = ToolStripItemAlignment.Left;
            _captionLabel.DoubleClickEnabled = true;
            _captionLabel.DoubleClick += new EventHandler(_captionLabel_DoubleClick);
            if (_formIcon != null)
            {
                _captionLabel.ImageTransparentColor = Color.Magenta;
                _captionLabel.Image = _formIcon;
            }

            this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this._closeToolStripItem,
                this._maxToolStripItem,
                this._minToolStripItem,
                this._captionLabel
            });

            this.Dock = DockStyle.Top;
            this.CanOverflow = false;
            this.GripStyle = ToolStripGripStyle.Hidden;

            //this.Renderer = new CaptionBarRenderer();
        }

        /// <summary>
        /// 双击图标关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _captionLabel_DoubleClick(object sender, EventArgs e)
        {
            this.FindForm().Close();
        }

        ///// <summary>
        ///// 处理标题点击时系统菜单显示
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void _captionLabel_Click(object sender, EventArgs e)
        //{
        //    //int hMenu = NativeMethods.GetSystemMenu(this.FindForm().Handle.ToInt32(), false);
        //    //NativeMethods.TrackPopupMenu(hMenu, 0, Cursor.Position.X, Cursor.Position.Y, 0, this.FindForm().Handle.ToInt32(), 0);
        //}

        /// <summary>
        /// ctor2
        /// </summary>
        /// <param name="container"></param>
        public CaptionBarAnother(IContainer container)
            : this()
        {
            if (container != null) container.Add(this);
            //InitializeComponent();
        }

        /// <summary>
        /// 鼠标拖动标题栏
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            int caplabelimgwid = 0;
            if (_captionLabel.Image != null) caplabelimgwid = _captionLabel.Image.Width;

            if ((e.X < this.Right - this.Margin.Right
                - _minToolStripItem.Width
                - _maxToolStripItem.Width
                - _closeToolStripItem.Width)
                && (e.Button == MouseButtons.Left) && (e.Clicks == 1)
                && (e.X > this.Left + this.Margin.Left + caplabelimgwid))
            {
                NativeMethods.ReleaseCapture();
                NativeMethods.SendMessage((IntPtr)this.FindForm().Handle, NativeMethods.WMNclbuttonDown, (IntPtr)NativeMethods.HTCaption, IntPtr.Zero);
            }
            base.OnMouseDown(e);
        }

        /// <summary>
        /// 鼠标双击标题栏
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            try
            {
                if (!(e.X < this.Left + this.Margin.Left + this._captionLabel.Image.Width))
                    this._maxToolStripItem.PerformClick();
            }
            catch
            { }
            finally
            {
                base.OnMouseDoubleClick(e);
            }
        }

        ///// <summary>
        ///// 重置CaptionBar的宽度
        ///// </summary>
        ///// <param name="e"></param>
        //protected override void OnResize(EventArgs e)
        //{
        //    this.Height = 20;
        //    if (this.Parent != null)
        //    {
        //        this.Width = this.Parent.ClientSize.Width;
        //    }
        //    base.OnResize(e);
        //}

        /// <summary>
        /// 设置窗口标题栏
        /// </summary>
        /// <param name="form"></param>
        /// <param name="canMin"></param>
        /// <param name="canMax"></param>
        /// <param name="setFormBorderNone"></param>
        public static void SetFormCaption(Control form, bool canMin, bool canMax, bool setFormBorderNone)
        {
            if (form == null)
                throw new ArgumentNullException("form");

            Form f = form as Form;
            CaptionBarAnother _instance;
            NoCaptionBarForm ncf = form as NoCaptionBarForm;
            if (ncf != null)
                _instance = ncf.CaptionBar;
            else
            {
                _instance = new CaptionBarAnother();
                f.Controls.Add(_instance);
            }

            if (!canMin)
            {
                _instance.MinButtonVisible = false;
            }
            if (!canMax)
            {
                _instance.MaxButtonVisible = false;
            }
            _instance.Caption = form.Text;

            if (setFormBorderNone)
            {
                //f.FormBorderStyle = FormBorderStyle.None;
                //SetFormNoCaptionBar(f);
            }
            if (canMax)
            {
                //if (f.WindowState != FormWindowState.Maximized)
                //{
                //    f.WindowState = FormWindowState.Maximized;
                //    _instance.Visible = false;
                //}
            }
            else
            {
                if (f.WindowState == FormWindowState.Minimized)
                    f.WindowState = FormWindowState.Normal;
            }

            //f.SizeChanged += new EventHandler(_instance.Form_SizeChanged);
        }

        /// <summary>
        /// 设置窗口标题栏 2
        /// </summary>
        /// <param name="form"></param>
        public static void SetFormCaption(Control form)
        {
            SetFormCaption(form, true, true);
        }

        /// <summary>
        /// 设置窗口标题栏 3
        /// </summary>
        /// <param name="form"></param>
        /// <param name="canMin"></param>
        /// <param name="canMax"></param>
        public static void SetFormCaption(Control form, bool canMin, bool canMax)
        {
            SetFormCaption(form, canMin, canMax, true);
        }

        //private void Form_SizeChanged(object sender, EventArgs e)
        //{
        //    Form f = sender as Form;

        //    if ((f.WindowState == FormWindowState.Maximized)
        //         && (f.MdiParent != null))
        //    {
        //        if (this.Visible)
        //            this.Visible = false;
        //    }
        //    else
        //    {
        //        if (!this.Visible)
        //            this.Visible = true;
        //    }
        //}

    }
}
