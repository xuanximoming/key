using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;

namespace DrectSoft.Library.EmrEditor.Src.Win32API
{
    public class Imm32
    {

        #region 声明处理输入法的Win32API函数及类型 ************************************************

        [DllImport("imm32.dll", CharSet = CharSet.Auto)]
        public static extern int ImmCreateContext();

        [DllImport("imm32.dll", CharSet = CharSet.Auto)]
        public static extern bool ImmDestroyContext(int hImc);

        [DllImport("imm32.dll", CharSet = CharSet.Auto)]
        public static extern bool ImmSetCandidateWindow(int hImc, ref CandidateForm frm);

        [DllImport("imm32.dll", CharSet = CharSet.Auto)]
        public static extern bool ImmSetStatusWindowPos(int hImc, ref Point pos);

        [DllImport("imm32.dll", CharSet = CharSet.Auto)]
        public static extern int ImmGetContext(int hwnd);

        [DllImport("imm32.dll", CharSet = CharSet.Auto)]
        public static extern bool ImmReleaseContext(int hwnd, int hImc);

        [DllImport("imm32.dll", CharSet = CharSet.Auto)]
        public static extern bool ImmGetOpenStatus(int hImc);

        [DllImport("imm32.dll", CharSet = CharSet.Auto)]
        public static extern bool ImmSetCompositionWindow(int hImc, ref CompositionForm frm);


        public struct CandidateForm
        {
            public int dwIndex;
            public int dwStyle;
            public Point ptCurrentPos;
            public DrectSoft.Library.EmrEditor.Src.Gui.DocumentViewControl.RECT rcArea;
        }

        public enum CandidateFormStyle
        {
            CFS_DEFAULT = 0x0000,
            CFS_RECT = 0x0001,
            CFS_POINT = 0x0002,
            CFS_FORCE_POSITION = 0x0020,
            CFS_CANDIDATEPOS = 0x0040,
            CFS_EXCLUDE = 0x0080
        }
        public struct CompositionForm
        {
            public int Style;
            public Point CurrentPos;
            public DrectSoft.Library.EmrEditor.Src.Gui.DocumentViewControl.RECT Area;
        }


        #endregion

        /// <summary>
        /// 判断指定的窗口中输入法是否打开
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <returns>输入法是否打开</returns>
        public static bool isImmOpen(int hWnd)
        {
            int hImc = ImmGetContext(hWnd);
            if (hImc == 0)
                return false;
            bool bolReturn = ImmGetOpenStatus(hImc);
            ImmReleaseContext(hWnd, hImc);
            return bolReturn;
        }

        /// <summary>
        /// 为指定的窗口设置输入法的位置
        /// </summary>
        /// <param name="hWnd">窗体句柄</param>
        /// <param name="x">输入法位置的X坐标</param>
        /// <param name="y">输入法位置的Y坐标</param>
        public static void SetImmPos(int hWnd, int x, int y)
        {
            bool bolReturn = false;
            //uint iError = 0;
            int hImc = ImmGetContext(hWnd);
            if (hImc != 0)
            {
                CompositionForm frm2 = new CompositionForm();
                frm2.CurrentPos.X = x;
                frm2.CurrentPos.Y = y;
                frm2.Style = (int)CandidateFormStyle.CFS_POINT;
                bolReturn = ImmSetCompositionWindow(hImc, ref frm2);
                //iError = Kernel32.GetLastError();
                ImmReleaseContext(hWnd, hImc);
            }
        }// void SetImmPos
    }// public class Imm32
}
