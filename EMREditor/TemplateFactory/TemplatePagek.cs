using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.FrameWork;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.FrameWork.WinForm;

namespace DrectSoft.Emr.TemplateFactory
{
    public partial class TemplatePagek : DevBaseForm
    {
        IEmrHost m_app;
        public TemplatePagek(IEmrHost app)
        {
            InitializeComponent();
            m_app = app;
        }

        private void Sb_save_Click(object sender, EventArgs e)
        {
            try
            {
                string StPackgeName = null;
                StPackgeName = Te_name.Text;
                SQLUtil SQLUtil = new SQLUtil(m_app);
                SQLUtil.SaveTempltepackge(StPackgeName);
                MessageBox.Show("保存成功!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
