using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace EmrInfirce
{
    internal class WindowInformation : IWin32Window
    {
        private class Win32Handle : IWin32Window
        {
            public IntPtr handle = IntPtr.Zero;

            public IntPtr Handle
            {
                get
                {
                    return this.handle;
                }
            }

            public Win32Handle()
            {
            }

            public Win32Handle(IntPtr h)
            {
                this.handle = h;
            }
        }

        public struct WINDOWPLACEMENT
        {
            public int length;

            public int flags;

            public int showCmd;

            public int ptMinPosition_x;

            public int ptMinPosition_y;

            public int ptMaxPosition_x;

            public int ptMaxPosition_y;

            public int rcNormalPosition_left;

            public int rcNormalPosition_top;

            public int rcNormalPosition_right;

            public int rcNormalPosition_bottom;
        }

        private delegate bool EnumDesktopWindowsProc(IntPtr desktopHandle, IntPtr lParam);

        private struct RECT
        {
            public int left;

            public int top;

            public int right;

            public int bottom;
        }

        private const int GCLP_HICON = -14;

        private const int ICON_SMALL = 0;

        private const int ICON_BIG = 1;

        private const int ICON_SMALL2 = 2;

        private const int WM_GETICON = 127;

        private const int WM_SETICON = 128;

        private const int SW_HIDE = 0;

        private const int SW_SHOWNORMAL = 1;

        private const int SW_NORMAL = 1;

        private const int SW_SHOWMINIMIZED = 2;

        private const int SW_SHOWMAXIMIZED = 3;

        private const int SW_MAXIMIZE = 3;

        private const int SW_SHOWNOACTIVATE = 4;

        private const int SW_SHOW = 5;

        private const int SW_MINIMIZE = 6;

        private const int SW_SHOWMINNOACTIVE = 7;

        private const int SW_SHOWNA = 8;

        private const int SW_RESTORE = 9;

        private const int SW_SHOWDEFAULT = 10;

        private const int SW_FORCEMINIMIZE = 11;

        private const int SW_MAX = 11;

        private const int GWL_WNDPROC = -4;

        private const int GWL_HINSTANCE = -6;

        private const int GWL_HWNDPARENT = -8;

        private const int GWL_STYLE = -16;

        private const int GWL_EXSTYLE = -20;

        private const int GWL_USERDATA = -21;

        private const int GWL_ID = -12;

        private const int GCL_MENUNAME = -8;

        private const int GCL_HBRBACKGROUND = -10;

        private const int GCL_HCURSOR = -12;

        private const int GCL_HICON = -14;

        private const int GCL_HMODULE = -16;

        private const int GCL_CBWNDEXTRA = -18;

        private const int GCL_CBCLSEXTRA = -20;

        private const int GCL_WNDPROC = -24;

        private const int GCL_STYLE = -26;

        private const int GCW_ATOM = -32;

        private bool bolThrowWin32Exception = false;

        private Icon myIcon = null;

        [NonSerialized]
        private IWin32Window myControl = null;

        private static ArrayList myDesktopWindows = null;

        public static WindowInformation DeskTop
        {
            get
            {
                IntPtr desktopWindow = WindowInformation.GetDesktopWindow();
                return new WindowInformation(desktopWindow);
            }
        }

        public bool ThrowWin32Exception
        {
            get
            {
                return this.bolThrowWin32Exception;
            }
            set
            {
                this.bolThrowWin32Exception = value;
            }
        }

        public IntPtr Handle
        {
            get
            {
                IntPtr result;
                if (this.CheckHandle())
                {
                    result = this.myControl.Handle;
                }
                else
                {
                    result = IntPtr.Zero;
                }
                return result;
            }
        }

        public string ClassName
        {
            get
            {
                string result;
                if (this.CheckHandle())
                {
                    byte[] array = new byte[1000];
                    int className = WindowInformation.GetClassName(this.myControl.Handle, array, array.Length);
                    if (className != 0)
                    {
                        string @string = Encoding.Unicode.GetString(array, 0, className * 2);
                        result = @string;
                        return result;
                    }
                    this.InnerThrowWin32Exception();
                }
                result = null;
                return result;
            }
        }

        public string Text
        {
            get
            {
                string result;
                if (this.CheckHandle())
                {
                    int windowTextLength = WindowInformation.GetWindowTextLength(this.myControl.Handle);
                    if (windowTextLength > 0)
                    {
                        StringBuilder stringBuilder = new StringBuilder(windowTextLength + 1);
                        if (WindowInformation.GetWindowText(this.myControl.Handle, stringBuilder, stringBuilder.Capacity) == 0)
                        {
                            this.InnerThrowWin32Exception();
                        }
                        result = stringBuilder.ToString();
                        return result;
                    }
                    if (windowTextLength == 0)
                    {
                        this.InnerThrowWin32Exception();
                    }
                }
                result = "";
                return result;
            }
            set
            {
                if (this.CheckHandle())
                {
                    if (value == null)
                    {
                        value = "";
                    }
                    if (!WindowInformation.SetWindowText(this.myControl.Handle, value))
                    {
                        this.InnerThrowWin32Exception();
                    }
                }
            }
        }

        public Rectangle ClientBounds
        {
            get
            {
                Rectangle result;
                if (this.CheckHandle())
                {
                    WindowInformation.RECT rECT = default(WindowInformation.RECT);
                    if (!WindowInformation.GetClientRect(this.myControl.Handle, ref rECT))
                    {
                        this.InnerThrowWin32Exception();
                    }
                    result = new Rectangle(rECT.left, rECT.top, rECT.right - rECT.left, rECT.bottom - rECT.top);
                }
                else
                {
                    result = Rectangle.Empty;
                }
                return result;
            }
        }

        public Rectangle Bounds
        {
            get
            {
                Rectangle result;
                if (this.CheckHandle())
                {
                    WindowInformation.RECT rECT = default(WindowInformation.RECT);
                    if (!WindowInformation.GetWindowRect(this.myControl.Handle, ref rECT))
                    {
                        this.InnerThrowWin32Exception();
                    }
                    result = new Rectangle(rECT.left, rECT.top, rECT.right - rECT.left, rECT.bottom - rECT.top);
                }
                else
                {
                    result = Rectangle.Empty;
                }
                return result;
            }
            set
            {
                if (this.CheckHandle())
                {
                    if (!WindowInformation.SetWindowPos(this.myControl.Handle, IntPtr.Zero, value.Left, value.Top, value.Width, value.Height, 20))
                    {
                        this.InnerThrowWin32Exception();
                    }
                }
            }
        }

        public bool Enabled
        {
            get
            {
                return this.CheckHandle() && WindowInformation.IsWindowEnabled(this.myControl.Handle);
            }
            set
            {
                if (this.CheckHandle())
                {
                    WindowInformation.EnableWindow(this.myControl.Handle, value);
                }
            }
        }

        public bool Visible
        {
            get
            {
                return this.CheckHandle() && WindowInformation.IsWindowVisible(this.myControl.Handle);
            }
            set
            {
                if (this.CheckHandle())
                {
                    WindowInformation.SetWindowPos(this.myControl.Handle, IntPtr.Zero, 0, 0, 0, 0, 23 | (value ? 64 : 128));
                }
            }
        }

        public int WindowStyle
        {
            get
            {
                int result;
                if (this.CheckHandle())
                {
                    int num = (int)WindowInformation.GetWindowLong(this.myControl.Handle, -16);
                    if (num == 0)
                    {
                        this.InnerThrowWin32Exception();
                    }
                    result = num;
                }
                else
                {
                    result = 0;
                }
                return result;
            }
            set
            {
                if (this.CheckHandle())
                {
                    if (WindowInformation.SetWindowLong(this.myControl.Handle, -16, new IntPtr(value)) == IntPtr.Zero)
                    {
                        this.InnerThrowWin32Exception();
                    }
                }
            }
        }

        public int WindowExStyle
        {
            get
            {
                int result;
                if (this.CheckHandle())
                {
                    int num = (int)WindowInformation.GetWindowLong(this.myControl.Handle, -20);
                    if (num == 0)
                    {
                        this.InnerThrowWin32Exception();
                    }
                    result = num;
                }
                else
                {
                    result = 0;
                }
                return result;
            }
            set
            {
                if (this.CheckHandle())
                {
                    if (WindowInformation.SetWindowLong(this.Handle, -20, new IntPtr(value)) == IntPtr.Zero)
                    {
                        this.InnerThrowWin32Exception();
                    }
                }
            }
        }

        public IntPtr ParentHandle
        {
            get
            {
                IntPtr result;
                if (this.CheckHandle())
                {
                    IntPtr parent = WindowInformation.GetParent(this.Handle);
                    if (parent == IntPtr.Zero)
                    {
                        this.InnerThrowWin32Exception();
                    }
                    result = parent;
                }
                else
                {
                    result = IntPtr.Zero;
                }
                return result;
            }
        }

        public FormWindowState WindowState
        {
            get
            {
                FormWindowState result;
                if (this.CheckHandle())
                {
                    WindowInformation.WINDOWPLACEMENT wINDOWPLACEMENT = default(WindowInformation.WINDOWPLACEMENT);
                    int windowPlacement = WindowInformation.GetWindowPlacement(this.Handle, ref wINDOWPLACEMENT);
                    if (windowPlacement == 0)
                    {
                        this.InnerThrowWin32Exception();
                    }
                    int showCmd = wINDOWPLACEMENT.showCmd;
                    if (showCmd == 1 || showCmd == 4 || showCmd == 5 || showCmd == 9)
                    {
                        result = FormWindowState.Normal;
                        return result;
                    }
                    if (showCmd == 2 || showCmd == 6 || showCmd == 7)
                    {
                        result = FormWindowState.Minimized;
                        return result;
                    }
                    if (showCmd == 3)
                    {
                        result = FormWindowState.Maximized;
                        return result;
                    }
                    if (WindowInformation.IsIconic(this.Handle))
                    {
                        result = FormWindowState.Minimized;
                        return result;
                    }
                }
                result = FormWindowState.Minimized;
                return result;
            }
            set
            {
                if (this.CheckHandle())
                {
                    switch (value)
                    {
                        case FormWindowState.Normal:
                            WindowInformation.ShowWindow(this.Handle, 1);
                            break;
                        case FormWindowState.Minimized:
                            WindowInformation.ShowWindow(this.Handle, 6);
                            break;
                        case FormWindowState.Maximized:
                            WindowInformation.ShowWindow(this.Handle, 3);
                            break;
                    }
                }
            }
        }

        public WindowInformation Parent
        {
            get
            {
                WindowInformation result;
                if (this.CheckHandle())
                {
                    IntPtr parent = WindowInformation.GetParent(this.Handle);
                    if (parent == IntPtr.Zero)
                    {
                        this.InnerThrowWin32Exception();
                        result = null;
                    }
                    else
                    {
                        result = new WindowInformation(parent);
                    }
                }
                else
                {
                    result = null;
                }
                return result;
            }
        }

        public Icon Icon
        {
            get
            {
                Icon result;
                if (this.CheckHandle())
                {
                    if (this.myIcon == null)
                    {
                        IntPtr intPtr = IntPtr.Zero;
                        if (Environment.OSVersion.Version >= new Version("6.1"))
                        {
                            intPtr = WindowInformation.SendMessage(this.Handle, 127, 2, 0);
                        }
                        if (intPtr == IntPtr.Zero)
                        {
                            intPtr = WindowInformation.SendMessage(this.Handle, 127, 0, 0);
                        }
                        if (intPtr == IntPtr.Zero)
                        {
                            intPtr = WindowInformation.SendMessage(this.Handle, 127, 1, 0);
                        }
                        if (intPtr == IntPtr.Zero)
                        {
                            intPtr = WindowInformation.GetClassLong(this.Handle, -14);
                        }
                        if (intPtr != IntPtr.Zero)
                        {
                            Icon icon = Icon.FromHandle(intPtr);
                            if (icon.Width == 0 || icon.Height == 0)
                            {
                                icon.Dispose();
                                result = null;
                                return result;
                            }
                            this.myIcon = icon;
                        }
                    }
                    result = this.myIcon;
                }
                else
                {
                    result = null;
                }
                return result;
            }
            set
            {
                if (this.CheckHandle())
                {
                    WindowInformation.SendMessage(this.Handle, 128, 0, (value == null) ? 0 : value.Handle.ToInt32());
                    this.myIcon = value;
                }
            }
        }

        public static WindowInformation[] DesktopWindows
        {
            get
            {
                IntPtr threadDesktop = WindowInformation.GetThreadDesktop(WindowInformation.GetCurrentThreadId());
                if (threadDesktop != IntPtr.Zero)
                {
                    lock (typeof(WindowInformation))
                    {
                        WindowInformation.myDesktopWindows = new ArrayList();
                        WindowInformation.EnumDesktopWindows(threadDesktop, new WindowInformation.EnumDesktopWindowsProc(WindowInformation.MyEnumDesktopWindows), threadDesktop);
                        WindowInformation.CloseDesktop(threadDesktop);
                        WindowInformation[] result = (WindowInformation[])WindowInformation.myDesktopWindows.ToArray(typeof(WindowInformation));
                        WindowInformation.myDesktopWindows = null;
                        return result;
                    }
                }
                throw new Win32Exception();
            }
        }

        public static WindowInformation GetFocusWindow()
        {
            IntPtr focus = WindowInformation.GetFocus();
            WindowInformation result;
            if (focus != IntPtr.Zero)
            {
                result = new WindowInformation(focus);
            }
            else
            {
                result = null;
            }
            return result;
        }

        public static WindowInformation FromHandle(IntPtr handle)
        {
            WindowInformation result;
            if (WindowInformation.CheckHandle(handle))
            {
                WindowInformation windowInformation = new WindowInformation(handle);
                result = windowInformation;
            }
            else
            {
                result = null;
            }
            return result;
        }

        public WindowInformation(IntPtr handle)
        {
            this.myControl = new WindowInformation.Win32Handle(handle);
        }

        public WindowInformation(IWin32Window win32)
        {
            this.myControl = win32;
        }

        public bool IsParent(IntPtr parentHandle)
        {
            bool result;
            if (parentHandle == IntPtr.Zero || !WindowInformation.IsWindow(parentHandle))
            {
                result = false;
            }
            else
            {
                if (this.CheckHandle())
                {
                    IntPtr intPtr = this.Handle;
                    while (intPtr != IntPtr.Zero)
                    {
                        intPtr = WindowInformation.GetParent(intPtr);
                        if (intPtr == parentHandle)
                        {
                            result = true;
                            return result;
                        }
                        if (intPtr == IntPtr.Zero)
                        {
                            break;
                        }
                    }
                }
                result = false;
            }
            return result;
        }

        public bool SetParent(IntPtr newParentHandle)
        {
            bool result;
            if (newParentHandle == IntPtr.Zero || !WindowInformation.IsWindow(newParentHandle))
            {
                result = false;
            }
            else if (this.CheckHandle())
            {
                IntPtr value = WindowInformation.SetParent(this.Handle, newParentHandle);
                if (value == IntPtr.Zero)
                {
                    this.InnerThrowWin32Exception();
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            else
            {
                result = false;
            }
            return result;
        }

        public Process GetOwnerProcess()
        {
            Process result;
            if (this.CheckHandle())
            {
                int num = 0;
                int windowThreadProcessId = WindowInformation.GetWindowThreadProcessId(this.Handle, ref num);
                if (num != 0)
                {
                    result = Process.GetProcessById(num);
                    return result;
                }
            }
            result = null;
            return result;
        }

        public WindowInformation GetTopLevelParentWindow()
        {
            WindowInformation[] desktopWindows = WindowInformation.DesktopWindows;
            WindowInformation result;
            for (WindowInformation windowInformation = this; windowInformation != null; windowInformation = windowInformation.Parent)
            {
                WindowInformation[] array = desktopWindows;
                for (int i = 0; i < array.Length; i++)
                {
                    WindowInformation windowInformation2 = array[i];
                    if (windowInformation2.Handle == windowInformation.Handle)
                    {
                        result = windowInformation2;
                        return result;
                    }
                }
            }
            result = null;
            return result;
        }

        public void Close()
        {
            if (this.CheckHandle())
            {
                WindowInformation.SendMessage(this.Handle, 16, 0, 0);
            }
        }

        public void BringToTop()
        {
            if (this.CheckHandle())
            {
                if (!WindowInformation.BringWindowToTop(this.Handle))
                {
                    this.InnerThrowWin32Exception();
                }
            }
        }

        public void Activate()
        {
            if (this.CheckHandle())
            {
                WindowInformation.SetForegroundWindow(this.Handle);
            }
        }

        public void Flash()
        {
            if (this.CheckHandle())
            {
                WindowInformation.FlashWindow(this.Handle, true);
            }
        }

        public bool CheckHandle()
        {
            return !(this.myControl.Handle == IntPtr.Zero) && WindowInformation.IsWindow(this.myControl.Handle);
        }

        public static bool CheckHandle(IntPtr handle)
        {
            return !(handle == IntPtr.Zero) && WindowInformation.IsWindow(handle);
        }

        private static bool MyEnumDesktopWindows(IntPtr hwnd, IntPtr lParam)
        {
            IntPtr windowThreadProcessId = WindowInformation.GetWindowThreadProcessId(hwnd, 0);
            IntPtr threadDesktop = WindowInformation.GetThreadDesktop(windowThreadProcessId);
            WindowInformation.CloseDesktop(threadDesktop);
            bool result;
            if (threadDesktop == lParam)
            {
                WindowInformation windowInformation = new WindowInformation(hwnd);
                if (!windowInformation.Visible)
                {
                    result = true;
                    return result;
                }
                string text = windowInformation.Text;
                if (text == null || text.Length == 0)
                {
                    result = true;
                    return result;
                }
                WindowInformation.myDesktopWindows.Add(windowInformation);
            }
            result = true;
            return result;
        }

        private void InnerThrowWin32Exception()
        {
            int lastWin32Error = Marshal.GetLastWin32Error();
            if (lastWin32Error != 0)
            {
                Win32Exception ex = new Win32Exception(lastWin32Error);
                if (this.ThrowWin32Exception)
                {
                    throw ex;
                }
            }
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetFocus();

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern int GetWindowPlacement(IntPtr hWnd, ref WindowInformation.WINDOWPLACEMENT placement);

        [DllImport("user32.dll")]
        private static extern int GetWindowThreadProcessId(IntPtr hwnd, ref int PorcessId);

        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hwnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowThreadProcessId(IntPtr hwnd, int p);

        [DllImport("user32.dll")]
        private static extern IntPtr GetThreadDesktop(IntPtr dwThreadId);

        [DllImport("user32.dll")]
        private static extern bool CloseDesktop(IntPtr hDesktop);

        [DllImport("Kernel32.dll")]
        private static extern IntPtr GetCurrentThreadId();

        [DllImport("user32.dll")]
        private static extern bool EnumDesktopWindows(IntPtr hwnd, WindowInformation.EnumDesktopWindowsProc lpfn, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern bool BringWindowToTop(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern bool FlashWindow(IntPtr hWnd, bool invert);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern bool EnableWindow(IntPtr hWnd, bool enable);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern bool IsWindowEnabled(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int flags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll")]
        private static extern IntPtr GetClassLong(IntPtr hwnd, int param);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern bool GetWindowRect(IntPtr hWnd, ref WindowInformation.RECT rect);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern bool GetClientRect(IntPtr hWnd, ref WindowInformation.RECT rect);

        [DllImport("user32.dll")]
        private static extern bool IsWindow(IntPtr hwnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetClassName(IntPtr hWnd, byte[] lpString, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool SetWindowText(IntPtr hWnd, string text);
    }
}
