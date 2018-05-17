using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace DrectSoft.FrameWork.WinForm
{
   /// <summary>
   /// 用于创建无标题栏的窗口
   /// </summary>
   public abstract class NativeMethods
   {
      /// <summary>
      /// CallBack
      /// </summary>
      /// <param name="hwnd"></param>
      /// <param name="lParam"></param>
      /// <returns></returns>
      public delegate bool CallBack(int hwnd, int lParam);

      /// <summary>
      /// SetWindowLong
      /// </summary>
      /// <param name="hwnd"></param>
      /// <param name="index"></param>
      /// <param name="dwNew"></param>
      /// <returns></returns>
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern int SetWindowLong(IntPtr hwnd, int index, int dwNew);

      /// <summary>
      /// GetWindowLong
      /// </summary>
      /// <param name="hwnd"></param>
      /// <param name="index"></param>
      /// <returns></returns>
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern int GetWindowLong(IntPtr hwnd, int index);

      /// <summary>
      /// SendMessage
      /// </summary>
      /// <param name="hwnd"></param>
      /// <param name="msg"></param>
      /// <param name="wParam"></param>
      /// <param name="lParam"></param>
      /// <returns></returns>
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      [CLSCompliant(false)]
      public static extern IntPtr SendMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

      /// <summary>
      /// ReleaseCapture
      /// </summary>
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static extern bool ReleaseCapture();

      /// <summary>
      /// GetSystemMenu
      /// </summary>
      /// <param name="hwnd"></param>
      /// <param name="bRevert"></param>
      /// <returns></returns>
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern int GetSystemMenu(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)]bool bRevert);

      /// <summary>
      /// TrackPopupMenu
      /// </summary>
      /// <param name="hmenu"></param>
      /// <param name="uFlags"></param>
      /// <param name="x"></param>
      /// <param name="y"></param>
      /// <param name="nReserved"></param>
      /// <param name="hwnd"></param>
      /// <param name="prcRect"></param>
      /// <returns></returns>
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      [CLSCompliant(false)]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static extern bool TrackPopupMenu(IntPtr hmenu, uint uFlags, int x, int y, int nReserved, IntPtr hwnd, IntPtr prcRect);

      /// <summary>
      /// EnumWindows
      /// </summary>
      /// <param name="x"></param>
      /// <param name="y"></param>
      /// <returns></returns>
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static extern bool EnumWindows(CallBack x, IntPtr y);

      /// <summary>
      /// ShowWindow
      /// </summary>
      /// <param name="hwnd"></param>
      /// <param name="nCmdShow"></param>
      /// <returns></returns>
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern int ShowWindow(int hwnd, int nCmdShow);

      /// <summary>
      /// SetActiveWindow
      /// </summary>
      /// <param name="hwnd"></param>
      /// <returns></returns>
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern int SetActiveWindow(int hwnd);

      /// <summary>
      /// GetLastActivePopup
      /// </summary>
      /// <param name="hwndOwnder"></param>
      /// <returns></returns>
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern int GetLastActivePopup(int hwndOwnder);

      /// <summary>
      /// 
      /// </summary>
      /// <param name="hwnd"></param>
      /// <returns></returns>
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern int SetForegroundWindow(int hwnd);
      /// <summary>
      /// GWL_STYLE
      /// </summary>
      public const int GwlStyle = -16;

      /// <summary>
      /// WM_NCCREATE
      /// </summary>
      public const int WMNccreate = 0x0081;

      /// <summary>
      /// WM_NCACTIVATE
      /// </summary>
      public const int WMNcactivate = 0x0086;

      /// <summary>
      /// WS_DLGFRAME
      /// </summary>
      public const int WSDlgFrame = 0x400000;

      /// <summary>
      /// WM_CREATE
      /// </summary>
      public const int WMCreate = 0x0001;

      /// <summary>
      /// WM_PAINT
      /// </summary>
      public const int WMPaint = 0x000F;

      /// <summary>
      /// WM_SHOWWINDOW
      /// </summary>
      public const int WMShowWindow = 0x0018;

      /// <summary>
      /// HTCAPTION
      /// </summary>
      public const int HTCaption = 2;

      /// <summary>
      /// WM_NCLBUTTONDOWN
      /// </summary>
      public const int WMNclbuttonDown = 0x00A1;

      /// <summary>
      /// WS_SIZEBOX
      /// </summary>
      public const int WSSizeBox = 0x40000;

      /// <summary>
      /// WS_CAPTION
      /// </summary>
      public const int WSCaption = 0xC00000;

      /// <summary>
      /// WS_SYSMENU
      /// </summary>
      public const int WSSysmenu = 0x80000;

      /// <summary>
      /// WS_MINIMIZEBOX
      /// </summary>
      public const int WSMinimizeBox = 0x20000;

      /// <summary>
      /// WS_VISIBLE
      /// </summary>
      public const int WSVisible = 0x10000000;

      /// <summary>
      /// SWMINIMIZE
      /// </summary>
      public const int SWMinimize = 6;

      /// <summary>
      /// SWMaximize
      /// </summary>
      public const int SWMaximize = 3;
   }
}
