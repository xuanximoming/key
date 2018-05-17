using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors;
using System.Windows.Forms;

namespace IemMainPageExtension
{
    class CtrlPopControl : PopupContainerEdit
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public CtrlPopControl()
        {
            try
            {
                Init();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化器
        /// </summary>
        private void Init()
        {
            try
            {
                PopupContainerControl popupContainerControl1=new PopupContainerControl();
                CheckedListBoxControl chkListBoxControlDX=new CheckedListBoxControl();
                popupContainerControl1.Controls.Add(chkListBoxControlDX);
                chkListBoxControlDX.Dock = DockStyle.Fill;
                this.Properties.PopupControl =popupContainerControl1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
    }
}
