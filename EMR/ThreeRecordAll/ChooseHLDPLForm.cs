using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Core.CommonTableConfig;

namespace DrectSoft.EMR.ThreeRecordAll
{
    public partial class ChooseHLDPLForm :DevBaseForm
    {

        public CommonNoteEntity SelectCommonNoteEntity;
        public ChooseHLDPLForm(List<CommonNoteEntity> commonNoteEntityListPL)
        {
            InitializeComponent();
            gridControl1.DataSource = commonNoteEntityListPL;
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GetCommonNote();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void GetCommonNote()
        {
            try
            {
                SelectCommonNoteEntity = gridView1.GetFocusedRow() as CommonNoteEntity;
                if (SelectCommonNoteEntity != null)
                {
                    this.DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                GetCommonNote();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void BtnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}