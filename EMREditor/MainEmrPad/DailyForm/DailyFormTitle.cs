using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;

namespace DrectSoft.Core.MainEmrPad.DailyForm
{
    public partial class DailyFormTitle : DevBaseForm
    {
        IEmrHost m_App;
        public DailyFormTitle()
        {
            InitializeComponent();
        }

        public DailyFormTitle(string title, IEmrHost app)
            : this()
        {
            this.textEditTitleName.Text = title;
            this.m_App = app;
        }

        private void simpleButtonOK_Click(object sender, EventArgs e)
        {
            if (textEditTitleName.Text.Trim() == "")
            {
                textEditTitleName.Text = "   ";
            }
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
        }

        private void simpleButtonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        public string GetTitle()
        {
            return textEditTitleName.Text;
        }
    }
}