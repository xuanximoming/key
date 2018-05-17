using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using YiDanCommon.Ctrs.FORM;
using YiDanCommon;
using YiDanCommon.Ctrs.DLG;

namespace YiDanSoft.Core.CommonTableConfig.CommonNoteUse
{
    public partial class PrintHistoryForm : DevBaseForm
    {
        public PrintHistoryForm()
        {
            InitializeComponent();
        }


        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                YD_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                YiDanMessageBox.Show(1, ex);
            }
        }
    }
}