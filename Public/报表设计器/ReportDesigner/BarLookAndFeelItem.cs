using DevExpress.LookAndFeel;
using DevExpress.XtraBars;

namespace ReportDesigner
{
    [System.ComponentModel.DesignTimeVisible(false), System.ComponentModel.ToolboxItem(false)]
    public class BarLookAndFeelItem : BarButtonItem
    {
        public BarLookAndFeelItem()
        {

        }
        public BarLookAndFeelItem(BarManager manager, bool privateItem)
        {
            this.fIsPrivateItem = privateItem;
            base.Manager = manager;
            this.ButtonStyle = BarButtonStyle.Check;
        }

        public virtual void UpdateState(UserLookAndFeel lookAndFeel)
        {
        }

        public virtual void ApplyChanges(UserLookAndFeel lookAndFeel)
        {
        }
    }
}
