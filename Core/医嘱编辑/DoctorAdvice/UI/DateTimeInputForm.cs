using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace DrectSoft.Core.DoctorAdvice
{
   public partial class DateTimeInputForm : XtraForm
   {
      /// <summary>
      /// 输入的时间
      /// </summary>
      public DateTime InputDateTime
      {
         get 
         { 
            return dateEdtInput.DateTime.Date + timeEdtInput.Time.TimeOfDay; 
         }
         set 
         { 
            dateEdtInput.DateTime = value;
            timeEdtInput.Time = value;
         }
      }

      public IEmrHost App
      {
         get { return _app; }
         set { _app = value; }
      }
      private IEmrHost _app;

      public DateTimeInputForm()
      {
         InitializeComponent();
      }

      private void btnOk_Click(object sender, EventArgs e)
      {
         if (InputDateTime < DateTime.Now)
            App.CustomMessageBox.MessageShow(ConstMessages.CheckCeaseDateBeforeNow, CustomMessageBoxKind.WarningOk);
         else
            DialogResult = DialogResult.OK;
      }

      private void btnCancel_Click(object sender, EventArgs e)
      {
         DialogResult = DialogResult.Cancel;
      }
   }
}