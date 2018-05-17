using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace DrectSoft.Basic.Paint.NET
{
    public sealed class SurfaceStyleSettingHelper
    {

        #region Fields
        private IShapeSurface _surface;
        #endregion

        #region Ctors

        public SurfaceStyleSettingHelper()
            : this(null) { }

        public SurfaceStyleSettingHelper(IShapeSurface surface)
        {
            _surface = surface;
        }

        #endregion

        #region Members

        #region Public

        public IShapeSurface Surface
        {
            get { return _surface; }
            set { _surface = value; }
        }

        public Color DrawColor
        {
            get
            {
                if (Ghost == null)
                    return Color.Black;
                else if (Ghost.State == GhostState.NotAvailable)
                    return Surface.SurfaceDrawColor;
                else
                    return Ghost.Shape.Data.DrawColor;
            }
            set
            {
                if (Ghost == null)
                    return;
                else if (Ghost.State == GhostState.NotAvailable)
                    Surface.SurfaceDrawColor = value;
                else
                {
                    Ghost.Shape.Data.DrawColor = value;
                    Ghost.NotifyDirty();
                }
            }
        }

        public Color FillColor
        {
            get
            {
                if (Ghost == null)
                    return Color.Black;
                else if (Ghost.State == GhostState.NotAvailable)
                    return Surface.SurfaceFillColor;
                else
                    return Ghost.Shape.Data.FillColor;
            }
            set
            {
                if (Ghost == null)
                    return;
                else if (Ghost.State == GhostState.NotAvailable)
                    Surface.SurfaceFillColor = value;
                else
                {
                    Ghost.Shape.Data.FillColor = value;
                    Ghost.NotifyDirty();
                }
            }
        }

        public bool IsFillColor
        {
            get
            {
                if (Ghost == null)
                    return false;
                else if (Ghost.State == GhostState.NotAvailable)
                    return Surface.SurfaceIsFillColor;
                else
                    return Ghost.Shape.Data.IsFillColor;
            }
            set
            {
                if (Ghost == null)
                    return;
                else if (Ghost.State == GhostState.NotAvailable)
                    Surface.SurfaceIsFillColor = value;
                else
                {
                    Ghost.Shape.Data.IsFillColor = value;
                    Ghost.NotifyDirty();
                }
            }
        }

        public bool IsHatch
        {
            get
            {
                if (Ghost == null)
                    return false;
                else if (Ghost.State == GhostState.NotAvailable)
                    return Surface.SurfaceIsHatch;
                else
                    return Ghost.Shape.Data.IsHatch;
            }
            set
            {
                if (Ghost == null)
                    return;
                else if (Ghost.State == GhostState.NotAvailable)
                    Surface.SurfaceIsHatch = value;
                else
                {
                    Ghost.Shape.Data.IsHatch = value;
                    Ghost.NotifyDirty();
                }
            }
        }

        public HatchStyle Hatch
        {
            get
            {
                if (Ghost == null)
                    return default(HatchStyle);
                else if (Ghost.State == GhostState.NotAvailable)
                    return Surface.SurfaceHatch;
                else
                    return Ghost.Shape.Data.Hatch;
            }
            set
            {
                if (Ghost == null)
                    return;
                else if (Ghost.State == GhostState.NotAvailable)
                    Surface.SurfaceHatch = value;
                else
                {
                    Ghost.Shape.Data.Hatch = value;
                    Ghost.NotifyDirty();
                }
            }
        }

        public bool IsTexture
        {
            get
            {
                if (Ghost == null)
                    return false;
                else if (Ghost.State == GhostState.NotAvailable)
                    return Surface.SurfaceIsTexture;
                else
                    return Ghost.Shape.Data.IsTexture;
            }
            set
            {
                if (Ghost == null)
                    return;
                else if (Ghost.State == GhostState.NotAvailable)
                    Surface.SurfaceIsTexture = value;
                else
                {
                    Ghost.Shape.Data.IsTexture = value;
                    Ghost.NotifyDirty();
                }
            }
        }

        public string TextureName
        {
            get
            {
                if (Ghost == null)
                    return string.Empty;
                else if (Ghost.State == GhostState.NotAvailable)
                    return Surface.SurfaceTexture;
                else
                    return Ghost.Shape.Data.TextureName;
            }
            set
            {
                if (Ghost == null)
                    return;
                else if (Ghost.State == GhostState.NotAvailable)
                    Surface.SurfaceTexture = value;
                else
                {
                    Ghost.Shape.Data.TextureName = value;
                    Ghost.NotifyDirty();
                }
            }
        }

        public float LineWidth
        {
            get
            {
                if (Ghost == null)
                    return 1;
                else if (Ghost.State == GhostState.NotAvailable)
                    return Surface.SurfaceLineWidth;
                else
                    return Ghost.Shape.Data.LineWidth;
            }
            set
            {
                if (Ghost == null)
                    return;
                else if (Ghost.State == GhostState.NotAvailable)
                    Surface.SurfaceLineWidth = value;
                else
                {
                    Ghost.Shape.Data.LineWidth = value;
                    Ghost.NotifyDirty();
                }
            }
        }

        #endregion

        #region Privates

        private IGhost Ghost
        {
            get
            {
                if (Surface == null || Surface.Source == null)
                    return null;
                else
                    return Surface.Source.Ghost;
            }
        }

        #endregion

        #endregion

    }
}
