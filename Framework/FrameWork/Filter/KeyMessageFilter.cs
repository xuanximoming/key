using System;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors;

namespace DrectSoft.FrameWork.Filter
{
    /// <summary>
    /// 此Filter用来解决Enter键为切换光标的问题
    /// 注：只对windows控件有效
    /// </summary>
    public class KeyMessageFilter : IMessageFilter
    {
        private readonly static IList StopControlTypes = new Type[] {typeof(TextBox),typeof(TextEdit), typeof(LookUpEdit)};
        private const int WM_KEYDOWN = 0x100;

        public KeyMessageFilter(){}
        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_KEYDOWN && Control.ModifierKeys == Keys.None && m.WParam.ToInt32() == (int)Keys.Enter)
            {
                Control ctr = Control.FromHandle(m.HWnd);
                if (ctr == null)
                {
                    ctr = Control.FromChildHandle(m.HWnd);
                }

                if (ctr != null && StopControlTypes.IndexOf(ctr.GetType()) >= 0)
                {
                    SendKeys.Send("{Tab}");
                    return true;
                }
            }

            return false;
       }
    }
}
