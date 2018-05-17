using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using YidanSoft.FrameWork.WinForm.Plugin;

namespace YidanSoft.Core.Consultation
{
    public partial class FormRecordForOne : DevExpress.XtraEditors.XtraForm
    {
        private string m_NoOfFirstPage = string.Empty;
        private IYidanEmrHost m_Host;
        private string m_ConsultApplySn = string.Empty;

        public FormRecordForOne()
        {
            InitializeComponent();
        }

        public FormRecordForOne(string noOfFirstPage, IYidanEmrHost host, string consultApplySn)
            : this()
        {
            m_NoOfFirstPage = noOfFirstPage;
            m_Host = host;
            m_ConsultApplySn = consultApplySn;
            InitInner(false);
        }

        private void InitInner(bool isNew)
        {
            UCRecordForOne recordForOne = new UCRecordForOne(m_NoOfFirstPage, m_Host, m_ConsultApplySn);
            recordForOne.Location = new Point(20, 15);
            this.Controls.Add(recordForOne);
        }

        public void ReadOnlyControl()
        {
            foreach (Control control in this.Controls)
            {
                UCRecordForOne recordForOne = control as UCRecordForOne;
                recordForOne.ReadOnlyControl();
                break;
            }
        }
    }
}