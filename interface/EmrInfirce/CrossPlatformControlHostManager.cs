using System;
using System.Drawing;
using System.Windows.Forms;

namespace EmrInfirce
{
    internal class CrossPlatformControlHostManager
    {
        private IntPtr _ControlHandle = IntPtr.Zero;
        private IntPtr _ContainerHandle = IntPtr.Zero;
        private DockStyle _Dock = DockStyle.Fill;

        public IntPtr ControlHandle
        {
            get
            {
                return this._ControlHandle;
            }
            set
            {
                this._ControlHandle = value;
            }
        }

        public IntPtr ContainerHandle
        {
            get
            {
                return this._ContainerHandle;
            }
            set
            {
                this._ContainerHandle = value;
            }
        }

        public DockStyle Dock
        {
            get
            {
                return this._Dock;
            }
            set
            {
                this._Dock = value;
            }
        }

        public bool UpdateLayout()
        {
            WindowInformation windowInformation = new WindowInformation(this.ControlHandle);
            WindowInformation windowInformation2 = new WindowInformation(this.ContainerHandle);
            bool result;
            if (!windowInformation.CheckHandle() || !windowInformation2.CheckHandle())
            {
                result = false;
            }
            else
            {
                if (windowInformation.ParentHandle != windowInformation2.Handle)
                {
                    if (!windowInformation.SetParent(windowInformation2.Handle))
                    {
                        result = false;
                        return result;
                    }
                }
                Rectangle clientBounds = windowInformation2.ClientBounds;
                Rectangle bounds = windowInformation.Bounds;
                Rectangle rectangle = bounds;
                switch (this.Dock)
                {
                    case DockStyle.Top:
                        rectangle = new Rectangle(0, 0, clientBounds.Width, bounds.Height);
                        break;
                    case DockStyle.Bottom:
                        rectangle = new Rectangle(0, clientBounds.Height - bounds.Height, clientBounds.Width, bounds.Height);
                        break;
                    case DockStyle.Left:
                        rectangle = new Rectangle(0, 0, bounds.Width, clientBounds.Height);
                        break;
                    case DockStyle.Right:
                        rectangle = new Rectangle(clientBounds.Width - bounds.Width, 0, bounds.Width, clientBounds.Height);
                        break;
                    case DockStyle.Fill:
                        rectangle = clientBounds;
                        break;
                }
                if (rectangle != bounds)
                {
                    windowInformation.Bounds = rectangle;
                }
                result = true;
            }
            return result;
        }
    }
}
