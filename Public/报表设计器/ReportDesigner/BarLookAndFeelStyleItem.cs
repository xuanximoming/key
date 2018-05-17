using DevExpress.LookAndFeel;
using DevExpress.XtraBars;
using System;

namespace ReportDesigner
{
	public class BarLookAndFeelStyleItem : BarLookAndFeelItem
	{
		private ActiveLookAndFeelStyle activeStyle;

		private LookAndFeelStyle style;

		public BarLookAndFeelStyleItem(BarManager manager, bool privateItem, ActiveLookAndFeelStyle activeStyle, LookAndFeelStyle style) : base(manager, privateItem)
		{
			this.style = style;
			this.activeStyle = activeStyle;
		}

		public override void UpdateState(UserLookAndFeel lookAndFeel)
		{
			this.Down = (lookAndFeel.ActiveStyle == this.activeStyle);
		}

		public override void ApplyChanges(UserLookAndFeel lookAndFeel)
		{
			lookAndFeel.UseWindowsXPTheme = false;
			lookAndFeel.Style = this.style;
		}
	}
}
