using DevExpress.LookAndFeel;
using DevExpress.Utils.WXPaint;
using DevExpress.XtraBars;
using System;

namespace ReportDesigner
{
	public class BarLookAndFeelUseWindowsXPThemeItem : BarLookAndFeelItem
	{
		public BarLookAndFeelUseWindowsXPThemeItem(BarManager manager, bool privateItem) : base(manager, privateItem)
		{
		}

		public override void UpdateState(UserLookAndFeel lookAndFeel)
		{
			this.Down = (lookAndFeel.ActiveStyle == ActiveLookAndFeelStyle.WindowsXP);
			this.Enabled = Painter.ThemesEnabled;
		}

		public override void ApplyChanges(UserLookAndFeel lookAndFeel)
		{
			lookAndFeel.UseWindowsXPTheme = true;
		}
	}
}
