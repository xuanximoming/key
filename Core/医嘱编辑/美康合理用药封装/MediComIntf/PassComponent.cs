using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace DrectSoft.Common.MediComIntf
{
   /// <summary>
   ///
   /// </summary>
   public partial class PassComponent : Component
   {
      private PassCheckHelper _passCheckHelper;
      private PassMenu _passContextMenu;

      /// <summary>
      ///
      /// </summary>
      public PassMenu PassContextMenu
      {
         get { return _passContextMenu; }
      }

      /// <summary>
      ///
      /// </summary>
      public PassCheckHelper PassCheckHelper
      {
         get { return _passCheckHelper; }
      }

      /// <summary>
      ///
      /// </summary>
      public PassComponent()
      {
         InitializeComponent();
      }

      /// <summary>
      ///
      /// </summary>
      public PassComponent(IContainer container)
      {
         container.Add(this);
         InitializeComponent();
      }

      /// <summary>
      ///
      /// </summary>
      public bool InitializePassIntf(string deptId, string deptName,
          string doctorId, string doctorName)
      {
         _passCheckHelper = new PassCheckHelper(deptId, deptName, doctorId, doctorName);
         _passCheckHelper.PassRegisterAndInit();
         _passContextMenu = new PassMenu(_passCheckHelper);
         _passContextMenu.Opened += new EventHandler(_passContextMenu_Opened);
         return _passCheckHelper.PassEnable();
      }

      /// <summary>
      ///
      /// </summary>
      void _passContextMenu_Opened(object sender, EventArgs e)
      {
         ContextMenuStrip cms = (sender as ContextMenuStrip);
         if (cms == null) return;
         for (int i = 0; i < cms.Items.Count; i++)
         {
            ToolStripMenuItem tsi = cms.Items[i] as ToolStripMenuItem;
            if (tsi == null) continue;
            if (tsi.Tag == null) continue;
            int commandId = Convert.ToInt32(tsi.Tag);
            if (_passCheckHelper.PassPopMenuEnable(commandId.ToString()))
               tsi.Enabled = true;
            else
               tsi.Enabled = false;
            for (int j = 0; j < tsi.DropDownItems.Count; j++)
            {
               ToolStripMenuItem tsisub = tsi.DropDownItems[j] as ToolStripMenuItem;
               if (tsisub.Tag == null) continue;
               int commandIdsub = Convert.ToInt32(tsisub.Tag);
               if (_passCheckHelper.PassPopMenuEnable(commandIdsub.ToString()))
                  tsisub.Enabled = true;
               else
                  tsisub.Enabled = false;
            }
         }
      }
   }
}
