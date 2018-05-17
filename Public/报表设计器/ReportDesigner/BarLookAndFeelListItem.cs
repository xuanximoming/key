using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.XtraBars;

namespace ReportDesigner
{
    public class BarLookAndFeelListItem : BarLinkContainerItem
    {
        private UserLookAndFeel lookAndFeel;

        private BarSubItem skinSubMenuItem;

        [System.ComponentModel.Browsable(false), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public override LinksInfo LinksPersistInfo
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        [System.ComponentModel.Browsable(false), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public override BarItemLinkCollection ItemLinks
        {
            get
            {
                return base.ItemLinks;
            }
        }

        public BarLookAndFeelListItem(UserLookAndFeel lookAndFeel)
        {
            this.lookAndFeel = lookAndFeel;
            this.skinSubMenuItem = new BarSubItem();
            this.skinSubMenuItem.Caption = "Skin";
        }

        protected override void OnManagerChanged()
        {
            base.OnManagerChanged();
            if (base.Manager != null)
            {
                this.BeginUpdate();
                this.ClearLinks();
                try
                {
                    this.skinSubMenuItem.ClearLinks();
                    this.AddBarLookAndFeelItem(this, new BarLookAndFeelUseWindowsXPThemeItem(base.Manager, true), "Use WindowsXP Theme");
                    this.AddBarLookAndFeelItem(this, new BarLookAndFeelStyleItem(base.Manager, true, ActiveLookAndFeelStyle.Flat, LookAndFeelStyle.Flat), "Flat Style");
                    this.AddBarLookAndFeelItem(this, new BarLookAndFeelStyleItem(base.Manager, true, ActiveLookAndFeelStyle.Office2003, LookAndFeelStyle.Office2003), "Office2003 Style");
                    this.AddBarLookAndFeelItem(this, new BarLookAndFeelStyleItem(base.Manager, true, ActiveLookAndFeelStyle.Style3D, LookAndFeelStyle.Style3D), "Style3D");
                    this.AddBarLookAndFeelItem(this, new BarLookAndFeelStyleItem(base.Manager, true, ActiveLookAndFeelStyle.UltraFlat, LookAndFeelStyle.UltraFlat), "UltraFlat Style");
                    this.AddItem(this.skinSubMenuItem);
                    foreach (SkinContainer skinContainer in SkinManager.Default.Skins)
                    {
                        this.AddBarLookAndFeelItem(this.skinSubMenuItem, new BarLookAndFeelSkinNameItem(base.Manager, true, skinContainer.SkinName), skinContainer.SkinName);
                    }
                }
                finally
                {
                    base.CancelUpdate();
                }
            }
        }

        protected override void OnGetItemData()
        {
            this.UpdateLookAndFeelItemsState(this);
            base.OnGetItemData();
        }

        private void UpdateLookAndFeelItemsState(BarLinksHolder holder)
        {
            foreach (BarItemLink barItemLink in holder.ItemLinks)
            {
                if (barItemLink.Item is BarLookAndFeelItem)
                {
                    ((BarLookAndFeelItem)barItemLink.Item).UpdateState(this.lookAndFeel);
                }
                if (barItemLink.Item is BarLinksHolder)
                {
                    this.UpdateLookAndFeelItemsState(barItemLink.Item as BarLinksHolder);
                }
            }
        }

        private void AddBarLookAndFeelItem(BarLinksHolder holder, BarLookAndFeelItem item, string caption)
        {
            item.Caption = caption;
            item.ItemClick += new ItemClickEventHandler(this.OnItemClick);
            item.UpdateState(this.lookAndFeel);
            holder.AddItem(item);
        }

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            BarLookAndFeelItem barLookAndFeelItem = e.Item as BarLookAndFeelItem;
            if (barLookAndFeelItem != null)
            {
                barLookAndFeelItem.ApplyChanges(this.lookAndFeel);
            }
        }
    }
}
