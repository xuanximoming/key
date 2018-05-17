using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace DrectSoft.Basic.Paint.NET
{
    public partial class PaintPanel
        : UserControl, IPaintPanel
    {

        #region Ctors

        public PaintPanel()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            controlSurface1.Source.Loaded += new EventHandler(Source_Loaded);
        }

        #endregion

        #region Methods

        public Control GetControl()
        {
            return this;
        }

        public void LoadImage(Stream stream)
        {
            try
            {
                XmlSurfaceStorage xss = new XmlSurfaceStorage();
                xss.Load(controlSurface1, stream);
            }
            catch (Exception)
            {
                SurfaceStorage ss = new SurfaceStorage();
                ss.Load(controlSurface1, stream);
            }
        }


        public void LoadOriginalImage(byte[] image)
        {
            try
            {

                Picture pic = new Picture(image);
                controlSurface1.Source.Load(pic, pic.GetImage().Size, null, null, null);
            }
            catch
            {
                controlSurface1.Source.Load(null, new Size(10, 10),
                    null, null, null);
            }
        }

        public void SaveImage(Stream stream)
        {
            try
            {
                XmlSurfaceStorage xss = new XmlSurfaceStorage();
                xss.Save(controlSurface1, stream);
            }
            catch (Exception)
            {
                SurfaceStorage ss = new SurfaceStorage();
                ss.Save(controlSurface1, stream);
            }
        }

        #endregion

        #region Properties

        public bool ReadOnly
        {
            get { return controlSurface1.ReadOnly; }
            set { controlSurface1.ReadOnly = value; }
        }

        #endregion

        #region Event Handlers

        private void Source_Loaded(object sender, EventArgs e)
        {
            int width = controlSurface1.Source.Size.Width;
            int height = controlSurface1.Source.Size.Height;
            controlSurface1.Size = new Size(width + 1, height + 1);
            this.Size = new Size(width + 95, Math.Max(height + 55, 270));
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            OnExitWithoutSave(EventArgs.Empty);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            OnExitWithSave(EventArgs.Empty);
        }

        #endregion

        #region Events

        #region Event ExitWithSave
        private static readonly object EventKey_ExitWithSave = new object();

        protected virtual void OnExitWithSave(EventArgs e)
        {
            EventHandler handler =
                base.Events[EventKey_ExitWithSave] as EventHandler;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler ExitWithSave
        {
            add { base.Events.AddHandler(EventKey_ExitWithSave, value); }
            remove { base.Events.RemoveHandler(EventKey_ExitWithSave, value); }
        }
        #endregion

        #region Event ExitWithoutSave
        private static readonly object EventKey_ExitWithoutSave = new object();

        protected virtual void OnExitWithoutSave(EventArgs e)
        {
            EventHandler handler =
                base.Events[EventKey_ExitWithoutSave] as EventHandler;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler ExitWithoutSave
        {
            add { base.Events.AddHandler(EventKey_ExitWithoutSave, value); }
            remove { base.Events.RemoveHandler(EventKey_ExitWithoutSave, value); }
        }
        #endregion

        #endregion


        #region IPaintPanel ≥…‘±

        public Picture BackGrond
        {
            get
            {
                if (controlSurface1 != null)
                    return controlSurface1.Source.Background;
                else
                    return null;
            }
        }

        #endregion


        public void LoadImage2(Image stream)
        {

        }


        public Image CurrentImage
        {
            get
            {
                if (controlSurface1 != null)
                    return controlSurface1.Source.Current;
                else
                    return null;
            }
        }
    }
}
