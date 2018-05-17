using DevExpress.LookAndFeel;
using DevExpress.XtraBars;

namespace ReportDesigner
{
    public class BarLookAndFeelSkinNameItem : BarLookAndFeelItem
    {
        private string skinName;

        public BarLookAndFeelSkinNameItem(BarManager manager, bool privateItem, string skinName)
            : base(manager, privateItem)
        {
            this.skinName = skinName;
        }

        public override void UpdateState(UserLookAndFeel lookAndFeel)
        {
            this.Down = (lookAndFeel.ActiveStyle == ActiveLookAndFeelStyle.Skin && lookAndFeel.SkinName == this.skinName);
        }

        public override void ApplyChanges(UserLookAndFeel lookAndFeel)
        {
            lookAndFeel.UseWindowsXPTheme = false;
            lookAndFeel.Style = LookAndFeelStyle.Skin;
            lookAndFeel.SkinName = this.skinName;
        }
    }
}
