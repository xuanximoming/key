using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.Core.NursingDocuments.PublicSet;

namespace DrectSoft.Core.NursingDocuments
{
    public partial class MainNursingDocuments : DevExpress.XtraEditors.XtraForm, IStartPlugIn
    {
        public MainNursingDocuments()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            NursingRecord frm = new NursingRecord();
            //frm.ShowNursingRecord();
            frm.ShowNursingRecord(new string[]
                {
                   "2011-3-10 12:14:00",
                   "2011-3-19 12:14:00",
                   "2011-3-21",
                   "2011-3-25 00:1:24"
                }
                );
           
            
        }

        #region IStartPlugIn ≥…‘±

        public IPlugIn Run(IEmrHost host)
        {
            PlugIn plg = new PlugIn(this.GetType().ToString(), this);

            MethodSet.App = host;

            return plg;
        }

        #endregion
    }
}