using System;
using System.Windows.Forms;
using DrectSoft.FrameWork;

namespace DrectSoft.MainFrame
{
	public class LoadLogin : IFrameStartup
	{
		public bool Run()
		{
			FormLogin formLogin = new FormLogin();
			return DialogResult.OK == formLogin.ShowDialog();
		}
	}
}
