using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DrectSoft.Core.DoctorAdvice
{
   /// <summary>
   /// 皮试结果输入窗口
   /// </summary>
   internal partial class SkinTestInputForm : Form
   {
      /// <summary>
      /// 选中的皮试结果
      /// </summary>
      public SkinTestResultKind SkinTestResult
      {
         get
         {
            switch (selResult.SelectedIndex)
            {
               case 0:
                  return SkinTestResultKind.Plus;
               case 1:
                  return SkinTestResultKind.Minus;
               default:
                  return SkinTestResultKind.MinusTreeDay;
            }
         }
      }

      /// <summary>
      /// 标记是否已选择数据
      /// </summary>
      public bool HadChoiceOne
      {
         get { return _hadChoiceOne; }
      }
      private bool _hadChoiceOne;

      /// <summary>
      /// 标记是否只显示提示消息
      /// </summary>
      public bool ShowMessageOnly
      {
         get { return _showMessageOnly; }
         set
         {
            _showMessageOnly = value;
            //panelSelect.Visible = !value;
            //if (!value)
            //   panelSelect.BringToFront();
         }
      }
      private bool _showMessageOnly;

      /// <summary>
      /// 需要显示出来的消息内容
      /// </summary>
      public string Message
      {
         get { return labMessage.Text; }
         set { labMessage.Text = value; }
      }

      public SkinTestInputForm()
      {
         InitializeComponent();
      }

      /// <summary>
      /// 已经做了选择操作
      /// </summary>
      public event EventHandler HadMadeChoice
      {
         add
         {
            onHadMadeChoice = (EventHandler)Delegate.Combine(onHadMadeChoice, value);
         }
         remove
         {
            onHadMadeChoice = (EventHandler)Delegate.Remove(onHadMadeChoice, value);
         }
      }
      private EventHandler onHadMadeChoice;

      private void DoHadMadeChoice()
      {
         if (onHadMadeChoice != null)
            onHadMadeChoice(this, new EventArgs());
      }

      private void btnOk_Click(object sender, EventArgs e)
      {
         _hadChoiceOne = !ShowMessageOnly;
         DialogResult = DialogResult.OK;
         DoHadMadeChoice();
      }

      private void btnCancel_Click(object sender, EventArgs e)
      {
         _hadChoiceOne = false;
         DialogResult = DialogResult.Cancel;
         DoHadMadeChoice();
      }

      private void SkinTestInputForm_Shown(object sender, EventArgs e)
      {
         selResult.SelectedIndex = 0;
         _hadChoiceOne = false;
      }
   }
}
