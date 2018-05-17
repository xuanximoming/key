namespace DrectSoft.Wordbook
{
   partial class UCtrlWordbookTree
   {
      /// <summary> 
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary> 
      /// Clean up any resources being used.
      /// </summary>
      protected override void Dispose(bool disposing)
      {
         if ( disposing && ( components != null ) )
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.components = new System.ComponentModel.Container();
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCtrlWordbookTree));
         this.imageListWordbookSort = new System.Windows.Forms.ImageList(this.components);
         this.navBarControl1 = new DevExpress.XtraNavBar.NavBarControl();
         this.navGroupHospital = new DevExpress.XtraNavBar.NavBarGroup();
         this.navBarGroupControlContainer1 = new DevExpress.XtraNavBar.NavBarGroupControlContainer();
         this.navBarGroupControlContainer2 = new DevExpress.XtraNavBar.NavBarGroupControlContainer();
         this.listBoxWordbook = new DevExpress.XtraEditors.ListBoxControl();
         this.navBarGroupControlContainer3 = new DevExpress.XtraNavBar.NavBarGroupControlContainer();
         this.navBarGroupControlContainer4 = new DevExpress.XtraNavBar.NavBarGroupControlContainer();
         this.navBarGroupControlContainer5 = new DevExpress.XtraNavBar.NavBarGroupControlContainer();
         this.navGroupClinc = new DevExpress.XtraNavBar.NavBarGroup();
         this.navGroupNormal = new DevExpress.XtraNavBar.NavBarGroup();
         this.navGroupDiagnose = new DevExpress.XtraNavBar.NavBarGroup();
         this.navGroupOthers = new DevExpress.XtraNavBar.NavBarGroup();
         ((System.ComponentModel.ISupportInitialize)(this.navBarControl1)).BeginInit();
         this.navBarControl1.SuspendLayout();
         this.navBarGroupControlContainer2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.listBoxWordbook)).BeginInit();
         this.SuspendLayout();
         // 
         // imageListWordbookSort
         // 
         this.imageListWordbookSort.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListWordbookSort.ImageStream")));
         this.imageListWordbookSort.TransparentColor = System.Drawing.Color.Transparent;
         this.imageListWordbookSort.Images.SetKeyName(0, "Clinic");
         this.imageListWordbookSort.Images.SetKeyName(1, "Diagnose");
         this.imageListWordbookSort.Images.SetKeyName(2, "Normal");
         this.imageListWordbookSort.Images.SetKeyName(3, "Hospital");
         this.imageListWordbookSort.Images.SetKeyName(4, "Medicare");
         this.imageListWordbookSort.Images.SetKeyName(5, "Other");
         // 
         // navBarControl1
         // 
         this.navBarControl1.ActiveGroup = this.navGroupOthers;
         this.navBarControl1.Controls.Add(this.navBarGroupControlContainer2);
         this.navBarControl1.Controls.Add(this.navBarGroupControlContainer4);
         this.navBarControl1.Controls.Add(this.navBarGroupControlContainer3);
         this.navBarControl1.Controls.Add(this.navBarGroupControlContainer5);
         this.navBarControl1.Controls.Add(this.navBarGroupControlContainer1);
         this.navBarControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.navBarControl1.Groups.AddRange(new DevExpress.XtraNavBar.NavBarGroup[] {
            this.navGroupHospital,
            this.navGroupClinc,
            this.navGroupNormal,
            this.navGroupDiagnose,
            this.navGroupOthers});
         this.navBarControl1.LargeImages = this.imageListWordbookSort;
         this.navBarControl1.Location = new System.Drawing.Point(0, 0);
         this.navBarControl1.Name = "navBarControl1";
         this.navBarControl1.NavigationPaneGroupClientHeight = 50;
         this.navBarControl1.NavigationPaneMaxVisibleGroups = 0;
         this.navBarControl1.Size = new System.Drawing.Size(216, 312);
         this.navBarControl1.SmallImages = this.imageListWordbookSort;
         this.navBarControl1.TabIndex = 4;
         this.navBarControl1.Text = "预定义字典列表";
         this.navBarControl1.View = new DevExpress.XtraNavBar.ViewInfo.SkinNavigationPaneViewInfoRegistrator();
         this.navBarControl1.ActiveGroupChanged += new DevExpress.XtraNavBar.NavBarGroupEventHandler(this.navBarControl1_ActiveGroupChanged);
         // 
         // navGroupHospital
         // 
         this.navGroupHospital.Caption = "医院基本设置";
         this.navGroupHospital.ControlContainer = this.navBarGroupControlContainer1;
         this.navGroupHospital.GroupClientHeight = 238;
         this.navGroupHospital.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.ControlContainer;
         this.navGroupHospital.LargeImageIndex = 3;
         this.navGroupHospital.Name = "navGroupHospital";
         this.navGroupHospital.SmallImageIndex = 3;
         // 
         // navBarGroupControlContainer1
         // 
         this.navBarGroupControlContainer1.Name = "navBarGroupControlContainer1";
         this.navBarGroupControlContainer1.Size = new System.Drawing.Size(212, 238);
         this.navBarGroupControlContainer1.TabIndex = 5;
         // 
         // navBarGroupControlContainer2
         // 
         this.navBarGroupControlContainer2.Controls.Add(this.listBoxWordbook);
         this.navBarGroupControlContainer2.Name = "navBarGroupControlContainer2";
         this.navBarGroupControlContainer2.Size = new System.Drawing.Size(212, 238);
         this.navBarGroupControlContainer2.TabIndex = 1;
         // 
         // listBoxWordbook
         // 
         this.listBoxWordbook.Dock = System.Windows.Forms.DockStyle.Fill;
         this.listBoxWordbook.Location = new System.Drawing.Point(0, 0);
         this.listBoxWordbook.Name = "listBoxWordbook";
         this.listBoxWordbook.Size = new System.Drawing.Size(212, 238);
         this.listBoxWordbook.TabIndex = 3;
         this.listBoxWordbook.DoubleClick += new System.EventHandler(this.listBoxWordbook_DoubleClick);
         // 
         // navBarGroupControlContainer3
         // 
         this.navBarGroupControlContainer3.Name = "navBarGroupControlContainer3";
         this.navBarGroupControlContainer3.Size = new System.Drawing.Size(212, 238);
         this.navBarGroupControlContainer3.TabIndex = 2;
         // 
         // navBarGroupControlContainer4
         // 
         this.navBarGroupControlContainer4.Name = "navBarGroupControlContainer4";
         this.navBarGroupControlContainer4.Size = new System.Drawing.Size(212, 238);
         this.navBarGroupControlContainer4.TabIndex = 3;
         // 
         // navBarGroupControlContainer5
         // 
         this.navBarGroupControlContainer5.Name = "navBarGroupControlContainer5";
         this.navBarGroupControlContainer5.Size = new System.Drawing.Size(212, 238);
         this.navBarGroupControlContainer5.TabIndex = 4;
         // 
         // navGroupClinc
         // 
         this.navGroupClinc.Caption = "临床字典";
         this.navGroupClinc.ControlContainer = this.navBarGroupControlContainer2;
         this.navGroupClinc.GroupClientHeight = 238;
         this.navGroupClinc.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.ControlContainer;
         this.navGroupClinc.LargeImageIndex = 0;
         this.navGroupClinc.Name = "navGroupClinc";
         this.navGroupClinc.SmallImageIndex = 0;
         // 
         // navGroupNormal
         // 
         this.navGroupNormal.Caption = "一般常用的字典";
         this.navGroupNormal.ControlContainer = this.navBarGroupControlContainer3;
         this.navGroupNormal.GroupClientHeight = 238;
         this.navGroupNormal.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.ControlContainer;
         this.navGroupNormal.LargeImageIndex = 2;
         this.navGroupNormal.Name = "navGroupNormal";
         this.navGroupNormal.SmallImageIndex = 2;
         // 
         // navGroupDiagnose
         // 
         this.navGroupDiagnose.Caption = "诊断字典";
         this.navGroupDiagnose.ControlContainer = this.navBarGroupControlContainer4;
         this.navGroupDiagnose.GroupClientHeight = 238;
         this.navGroupDiagnose.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.ControlContainer;
         this.navGroupDiagnose.LargeImageIndex = 1;
         this.navGroupDiagnose.Name = "navGroupDiagnose";
         this.navGroupDiagnose.SmallImageIndex = 1;
         // 
         // navGroupOthers
         // 
         this.navGroupOthers.Caption = "其它字典";
         this.navGroupOthers.ControlContainer = this.navBarGroupControlContainer5;
         this.navGroupOthers.Expanded = true;
         this.navGroupOthers.GroupClientHeight = 238;
         this.navGroupOthers.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.ControlContainer;
         this.navGroupOthers.LargeImageIndex = 5;
         this.navGroupOthers.Name = "navGroupOthers";
         this.navGroupOthers.SmallImageIndex = 5;
         // 
         // UCtrlWordbookTree
         // 
         this.Controls.Add(this.navBarControl1);
         this.Name = "UCtrlWordbookTree";
         this.Size = new System.Drawing.Size(216, 312);
         ((System.ComponentModel.ISupportInitialize)(this.navBarControl1)).EndInit();
         this.navBarControl1.ResumeLayout(false);
         this.navBarGroupControlContainer2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.listBoxWordbook)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.ImageList imageListWordbookSort;
      private DevExpress.XtraNavBar.NavBarControl navBarControl1;
      private DevExpress.XtraNavBar.NavBarGroup navGroupHospital;
      private DevExpress.XtraNavBar.NavBarGroup navGroupClinc;
      private DevExpress.XtraNavBar.NavBarGroup navGroupNormal;
      private DevExpress.XtraNavBar.NavBarGroup navGroupDiagnose;
      private DevExpress.XtraNavBar.NavBarGroup navGroupOthers;
      private DevExpress.XtraNavBar.NavBarGroupControlContainer navBarGroupControlContainer2;
      private DevExpress.XtraNavBar.NavBarGroupControlContainer navBarGroupControlContainer3;
      private DevExpress.XtraNavBar.NavBarGroupControlContainer navBarGroupControlContainer4;
      private DevExpress.XtraEditors.ListBoxControl listBoxWordbook;
      private DevExpress.XtraNavBar.NavBarGroupControlContainer navBarGroupControlContainer5;
      private DevExpress.XtraNavBar.NavBarGroupControlContainer navBarGroupControlContainer1;
   }
}
