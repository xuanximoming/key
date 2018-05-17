using System;
using System.Windows.Forms;
using DrectSoft.MainFrame;

namespace MainFrame
{
	internal class StartupClass : MarshalByRefObject
	{
		public StartupClass(bool skiplogin, string loadmenu)
		{
			FormMain mainForm = new FormMain(false, "file.menu", false);
			Application.Run(mainForm);
		}
	}
}
