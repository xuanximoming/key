namespace DrectSoft.Emr.QcManager
{
    partial class InWardPatRecInfo
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage_Master = new DevExpress.XtraTab.XtraTabPage();
            this.gridControl_Master = new DevExpress.XtraGrid.GridControl();
            this.gridViewMain = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.col_DEPARTID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_DEPTNAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTotalPat = new DevExpress.XtraGrid.Columns.GridColumn();
            this.COL_TOTALBEDS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_DanagerPats = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_PATS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_NoPatRec = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_Spot = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_AVERAGE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btn_PrintMaster = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Refresh = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Export = new DevExpress.XtraEditors.SimpleButton();
            this.btn_ViewDetail = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControlInfo = new DevExpress.XtraEditors.LabelControl();
            this.xtraTabPage_Detail = new DevExpress.XtraTab.XtraTabPage();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridViewDetail = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.col_Status = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_PatID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.COL_PATNAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.COL_PATSEX = new DevExpress.XtraGrid.Columns.GridColumn();
            this.COL_AGE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.COL_PATBED = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_INCOUNT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.COL_INHOSPITAL = new DevExpress.XtraGrid.Columns.GridColumn();
            this.COL_OUTTIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_OUTFILE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.COL_PATFILES = new DevExpress.XtraGrid.Columns.GridColumn();
            this.COL_PATDIAG = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btn_Return = new DevExpress.XtraEditors.SimpleButton();
            this.btn_ViewPatRec = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControlPATRECS = new DevExpress.XtraEditors.LabelControl();
            this.labNoRecord = new DevExpress.XtraEditors.LabelControl();
            this.labPatCount = new DevExpress.XtraEditors.LabelControl();
            this.labelControlTotalPats = new DevExpress.XtraEditors.LabelControl();
            this.labDeptName = new DevExpress.XtraEditors.LabelControl();
            this.labelControlDept = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage_Master.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_Master)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.xtraTabPage_Detail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage_Master;
            this.xtraTabControl1.Size = new System.Drawing.Size(838, 635);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage_Master,
            this.xtraTabPage_Detail});
            // 
            // xtraTabPage_Master
            // 
            this.xtraTabPage_Master.Controls.Add(this.gridControl_Master);
            this.xtraTabPage_Master.Controls.Add(this.panelControl1);
            this.xtraTabPage_Master.Name = "xtraTabPage_Master";
            this.xtraTabPage_Master.Size = new System.Drawing.Size(832, 607);
            this.xtraTabPage_Master.Text = "xtraTabPage1";
            // 
            // gridControl_Master
            // 
            this.gridControl_Master.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl_Master.Location = new System.Drawing.Point(0, 45);
            this.gridControl_Master.MainView = this.gridViewMain;
            this.gridControl_Master.Name = "gridControl_Master";
            this.gridControl_Master.Size = new System.Drawing.Size(832, 562);
            this.gridControl_Master.TabIndex = 1;
            this.gridControl_Master.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewMain});
            this.gridControl_Master.DoubleClick += new System.EventHandler(this.gridViewMain_DoubleClick);
            // 
            // gridViewMain
            // 
            this.gridViewMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.col_DEPARTID,
            this.col_DEPTNAME,
            this.colTotalPat,
            this.COL_TOTALBEDS,
            this.col_DanagerPats,
            this.col_PATS,
            this.col_NoPatRec,
            this.col_Spot,
            this.col_AVERAGE});
            this.gridViewMain.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridViewMain.GridControl = this.gridControl_Master;
            this.gridViewMain.GroupSummary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Count, "DEPTID", this.col_DEPARTID, "总计{0}", 0)});
            this.gridViewMain.Name = "gridViewMain";
            this.gridViewMain.OptionsBehavior.Editable = false;
            this.gridViewMain.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewMain.OptionsView.ShowFooter = true;
            this.gridViewMain.OptionsView.ShowGroupPanel = false;
            this.gridViewMain.OptionsView.ShowIndicator = false;
            this.gridViewMain.DoubleClick += new System.EventHandler(this.gridViewMain_DoubleClick);
            // 
            // col_DEPARTID
            // 
            this.col_DEPARTID.Caption = "gridColumn1";
            this.col_DEPARTID.FieldName = "DEPTID";
            this.col_DEPARTID.Name = "col_DEPARTID";
            this.col_DEPARTID.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count;
            // 
            // col_DEPTNAME
            // 
            this.col_DEPTNAME.Caption = "科室名称";
            this.col_DEPTNAME.FieldName = "DEPTNAME";
            this.col_DEPTNAME.Name = "col_DEPTNAME";
            this.col_DEPTNAME.Visible = true;
            this.col_DEPTNAME.VisibleIndex = 0;
            // 
            // colTotalPat
            // 
            this.colTotalPat.Caption = "在院病人";
            this.colTotalPat.FieldName = "TOTALPAT";
            this.colTotalPat.Name = "colTotalPat";
            this.colTotalPat.Visible = true;
            this.colTotalPat.VisibleIndex = 1;
            // 
            // COL_TOTALBEDS
            // 
            this.COL_TOTALBEDS.Caption = "床位数";
            this.COL_TOTALBEDS.FieldName = "TOTALBED";
            this.COL_TOTALBEDS.Name = "COL_TOTALBEDS";
            this.COL_TOTALBEDS.Visible = true;
            this.COL_TOTALBEDS.VisibleIndex = 2;
            // 
            // col_DanagerPats
            // 
            this.col_DanagerPats.Caption = "危重患者";
            this.col_DanagerPats.FieldName = "DANAGERPATS";
            this.col_DanagerPats.Name = "col_DanagerPats";
            this.col_DanagerPats.Visible = true;
            this.col_DanagerPats.VisibleIndex = 3;
            // 
            // col_PATS
            // 
            this.col_PATS.Caption = "未确诊病历";
            this.col_PATS.FieldName = "DIAGPATS";
            this.col_PATS.Name = "col_PATS";
            this.col_PATS.Visible = true;
            this.col_PATS.VisibleIndex = 4;
            // 
            // col_NoPatRec
            // 
            this.col_NoPatRec.Caption = "未写病历患者";
            this.col_NoPatRec.FieldName = "NOPATREC";
            this.col_NoPatRec.Name = "col_NoPatRec";
            this.col_NoPatRec.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count;
            this.col_NoPatRec.Visible = true;
            this.col_NoPatRec.VisibleIndex = 5;
            // 
            // col_Spot
            // 
            this.col_Spot.Caption = "抽查病历数";
            this.col_Spot.FieldName = "SPOTPATREC";
            this.col_Spot.Name = "col_Spot";
            this.col_Spot.Visible = true;
            this.col_Spot.VisibleIndex = 6;
            // 
            // col_AVERAGE
            // 
            this.col_AVERAGE.Caption = "抽查平均分";
            this.col_AVERAGE.FieldName = "SPOTPATREC";
            this.col_AVERAGE.Name = "col_AVERAGE";
            this.col_AVERAGE.Visible = true;
            this.col_AVERAGE.VisibleIndex = 7;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btn_PrintMaster);
            this.panelControl1.Controls.Add(this.btn_Refresh);
            this.panelControl1.Controls.Add(this.btn_Export);
            this.panelControl1.Controls.Add(this.btn_ViewDetail);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.labelControlInfo);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(832, 45);
            this.panelControl1.TabIndex = 0;
            // 
            // btn_PrintMaster
            // 
            this.btn_PrintMaster.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_PrintMaster.Location = new System.Drawing.Point(655, 10);
            this.btn_PrintMaster.Name = "btn_PrintMaster";
            this.btn_PrintMaster.Size = new System.Drawing.Size(75, 23);
            this.btn_PrintMaster.TabIndex = 4;
            this.btn_PrintMaster.Text = "打印";
            // 
            // btn_Refresh
            // 
            this.btn_Refresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Refresh.Location = new System.Drawing.Point(742, 10);
            this.btn_Refresh.Name = "btn_Refresh";
            this.btn_Refresh.Size = new System.Drawing.Size(75, 23);
            this.btn_Refresh.TabIndex = 3;
            this.btn_Refresh.Text = "刷新";
            // 
            // btn_Export
            // 
            this.btn_Export.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Export.Location = new System.Drawing.Point(568, 10);
            this.btn_Export.Name = "btn_Export";
            this.btn_Export.Size = new System.Drawing.Size(75, 23);
            this.btn_Export.TabIndex = 2;
            this.btn_Export.Text = "导出";
            this.btn_Export.Click += new System.EventHandler(this.btn_Export_Click);
            // 
            // btn_ViewDetail
            // 
            this.btn_ViewDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_ViewDetail.Location = new System.Drawing.Point(459, 10);
            this.btn_ViewDetail.Name = "btn_ViewDetail";
            this.btn_ViewDetail.Size = new System.Drawing.Size(97, 23);
            this.btn_ViewDetail.TabIndex = 1;
            this.btn_ViewDetail.Text = "科室明细";
            this.btn_ViewDetail.Click += new System.EventHandler(this.btn_ViewDetail_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl1.Location = new System.Drawing.Point(80, 17);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(24, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "全院";
            // 
            // labelControlInfo
            // 
            this.labelControlInfo.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.labelControlInfo.Location = new System.Drawing.Point(26, 17);
            this.labelControlInfo.Name = "labelControlInfo";
            this.labelControlInfo.Size = new System.Drawing.Size(36, 14);
            this.labelControlInfo.TabIndex = 0;
            this.labelControlInfo.Text = "科室：";
            // 
            // xtraTabPage_Detail
            // 
            this.xtraTabPage_Detail.Controls.Add(this.gridControl1);
            this.xtraTabPage_Detail.Controls.Add(this.panelControl2);
            this.xtraTabPage_Detail.Name = "xtraTabPage_Detail";
            this.xtraTabPage_Detail.Size = new System.Drawing.Size(832, 607);
            this.xtraTabPage_Detail.Text = "xtraTabPage2";
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 45);
            this.gridControl1.MainView = this.gridViewDetail;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(832, 562);
            this.gridControl1.TabIndex = 1;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewDetail});
            this.gridControl1.DoubleClick += new System.EventHandler(this.gridViewDetail_DoubleClick);
            // 
            // gridViewDetail
            // 
            this.gridViewDetail.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.col_Status,
            this.col_PatID,
            this.COL_PATNAME,
            this.COL_PATSEX,
            this.COL_AGE,
            this.COL_PATBED,
            this.col_INCOUNT,
            this.COL_INHOSPITAL,
            this.COL_OUTTIME,
            this.col_OUTFILE,
            this.COL_PATFILES,
            this.COL_PATDIAG});
            this.gridViewDetail.GridControl = this.gridControl1;
            this.gridViewDetail.Name = "gridViewDetail";
            this.gridViewDetail.OptionsBehavior.Editable = false;
            this.gridViewDetail.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewDetail.OptionsView.ShowGroupPanel = false;
            this.gridViewDetail.OptionsView.ShowIndicator = false;
            this.gridViewDetail.DoubleClick += new System.EventHandler(this.gridViewDetail_DoubleClick);
            // 
            // col_Status
            // 
            this.col_Status.Caption = "病历缺陷";
            this.col_Status.FieldName = "PATRECSTATUS";
            this.col_Status.Name = "col_Status";
            this.col_Status.Visible = true;
            this.col_Status.VisibleIndex = 0;
            // 
            // col_PatID
            // 
            this.col_PatID.Caption = "住院号";
            this.col_PatID.FieldName = "PATID";
            this.col_PatID.Name = "col_PatID";
            this.col_PatID.Visible = true;
            this.col_PatID.VisibleIndex = 2;
            // 
            // COL_PATNAME
            // 
            this.COL_PATNAME.Caption = "患者姓名";
            this.COL_PATNAME.FieldName = "PATNAME";
            this.COL_PATNAME.Name = "COL_PATNAME";
            this.COL_PATNAME.Visible = true;
            this.COL_PATNAME.VisibleIndex = 3;
            // 
            // COL_PATSEX
            // 
            this.COL_PATSEX.Caption = "性别";
            this.COL_PATSEX.FieldName = "PATSEX";
            this.COL_PATSEX.Name = "COL_PATSEX";
            this.COL_PATSEX.Visible = true;
            this.COL_PATSEX.VisibleIndex = 4;
            // 
            // COL_AGE
            // 
            this.COL_AGE.Caption = "患者年龄";
            this.COL_AGE.FieldName = "PATAGE";
            this.COL_AGE.Name = "COL_AGE";
            this.COL_AGE.Visible = true;
            this.COL_AGE.VisibleIndex = 5;
            // 
            // COL_PATBED
            // 
            this.COL_PATBED.Caption = "床位";
            this.COL_PATBED.FieldName = "PATBED";
            this.COL_PATBED.Name = "COL_PATBED";
            this.COL_PATBED.Visible = true;
            this.COL_PATBED.VisibleIndex = 1;
            // 
            // col_INCOUNT
            // 
            this.col_INCOUNT.Caption = "住院次数";
            this.col_INCOUNT.FieldName = "INHOSPITAL";
            this.col_INCOUNT.Name = "col_INCOUNT";
            this.col_INCOUNT.Visible = true;
            this.col_INCOUNT.VisibleIndex = 6;
            // 
            // COL_INHOSPITAL
            // 
            this.COL_INHOSPITAL.Caption = "住院天数";
            this.COL_INHOSPITAL.FieldName = "INCOUNT";
            this.COL_INHOSPITAL.Name = "COL_INHOSPITAL";
            this.COL_INHOSPITAL.Visible = true;
            this.COL_INHOSPITAL.VisibleIndex = 7;
            // 
            // COL_OUTTIME
            // 
            this.COL_OUTTIME.Caption = "超时病历";
            this.COL_OUTTIME.FieldName = "OUTTIME";
            this.COL_OUTTIME.Name = "COL_OUTTIME";
            this.COL_OUTTIME.Visible = true;
            this.COL_OUTTIME.VisibleIndex = 8;
            // 
            // col_OUTFILE
            // 
            this.col_OUTFILE.Caption = "缺失病历";
            this.col_OUTFILE.FieldName = "OUTFILES";
            this.col_OUTFILE.Name = "col_OUTFILE";
            this.col_OUTFILE.Visible = true;
            this.col_OUTFILE.VisibleIndex = 9;
            // 
            // COL_PATFILES
            // 
            this.COL_PATFILES.Caption = "病历数";
            this.COL_PATFILES.FieldName = "PATFILES";
            this.COL_PATFILES.Name = "COL_PATFILES";
            this.COL_PATFILES.Visible = true;
            this.COL_PATFILES.VisibleIndex = 10;
            // 
            // COL_PATDIAG
            // 
            this.COL_PATDIAG.Caption = "入院诊断";
            this.COL_PATDIAG.FieldName = "PATDIAGNAME";
            this.COL_PATDIAG.Name = "COL_PATDIAG";
            this.COL_PATDIAG.Visible = true;
            this.COL_PATDIAG.VisibleIndex = 11;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.btn_Return);
            this.panelControl2.Controls.Add(this.btn_ViewPatRec);
            this.panelControl2.Controls.Add(this.simpleButton1);
            this.panelControl2.Controls.Add(this.simpleButton2);
            this.panelControl2.Controls.Add(this.labelControlPATRECS);
            this.panelControl2.Controls.Add(this.labNoRecord);
            this.panelControl2.Controls.Add(this.labPatCount);
            this.panelControl2.Controls.Add(this.labelControlTotalPats);
            this.panelControl2.Controls.Add(this.labDeptName);
            this.panelControl2.Controls.Add(this.labelControlDept);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl2.Location = new System.Drawing.Point(0, 0);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(832, 45);
            this.panelControl2.TabIndex = 0;
            // 
            // btn_Return
            // 
            this.btn_Return.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Return.Location = new System.Drawing.Point(460, 14);
            this.btn_Return.Name = "btn_Return";
            this.btn_Return.Size = new System.Drawing.Size(75, 23);
            this.btn_Return.TabIndex = 8;
            this.btn_Return.Text = "返回科室列表";
            this.btn_Return.Click += new System.EventHandler(this.btn_Return_Click);
            // 
            // btn_ViewPatRec
            // 
            this.btn_ViewPatRec.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_ViewPatRec.Location = new System.Drawing.Point(541, 14);
            this.btn_ViewPatRec.Name = "btn_ViewPatRec";
            this.btn_ViewPatRec.Size = new System.Drawing.Size(75, 23);
            this.btn_ViewPatRec.TabIndex = 7;
            this.btn_ViewPatRec.Text = "详细信息";
            this.btn_ViewPatRec.Click += new System.EventHandler(this.btn_ViewPatRec_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton1.Location = new System.Drawing.Point(721, 14);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 23);
            this.simpleButton1.TabIndex = 6;
            this.simpleButton1.Text = "打印";
            // 
            // simpleButton2
            // 
            this.simpleButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton2.Location = new System.Drawing.Point(634, 14);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(75, 23);
            this.simpleButton2.TabIndex = 5;
            this.simpleButton2.Text = "导出";
            // 
            // labelControlPATRECS
            // 
            this.labelControlPATRECS.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.labelControlPATRECS.Location = new System.Drawing.Point(330, 14);
            this.labelControlPATRECS.Name = "labelControlPATRECS";
            this.labelControlPATRECS.Size = new System.Drawing.Size(64, 14);
            this.labelControlPATRECS.TabIndex = 3;
            this.labelControlPATRECS.Text = "无病历患者:";
            // 
            // labNoRecord
            // 
            this.labNoRecord.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.labNoRecord.Location = new System.Drawing.Point(404, 14);
            this.labNoRecord.Name = "labNoRecord";
            this.labNoRecord.Size = new System.Drawing.Size(24, 14);
            this.labNoRecord.TabIndex = 2;
            this.labNoRecord.Text = "人数";
            // 
            // labPatCount
            // 
            this.labPatCount.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.labPatCount.Location = new System.Drawing.Point(279, 14);
            this.labPatCount.Name = "labPatCount";
            this.labPatCount.Size = new System.Drawing.Size(24, 14);
            this.labPatCount.TabIndex = 2;
            this.labPatCount.Text = "人数";
            // 
            // labelControlTotalPats
            // 
            this.labelControlTotalPats.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.labelControlTotalPats.Location = new System.Drawing.Point(228, 14);
            this.labelControlTotalPats.Name = "labelControlTotalPats";
            this.labelControlTotalPats.Size = new System.Drawing.Size(40, 14);
            this.labelControlTotalPats.TabIndex = 2;
            this.labelControlTotalPats.Text = "总人数:";
            // 
            // labDeptName
            // 
            this.labDeptName.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labDeptName.Location = new System.Drawing.Point(68, 14);
            this.labDeptName.Name = "labDeptName";
            this.labDeptName.Size = new System.Drawing.Size(24, 14);
            this.labDeptName.TabIndex = 1;
            this.labDeptName.Text = "科室";
            // 
            // labelControlDept
            // 
            this.labelControlDept.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.labelControlDept.Location = new System.Drawing.Point(26, 14);
            this.labelControlDept.Name = "labelControlDept";
            this.labelControlDept.Size = new System.Drawing.Size(36, 14);
            this.labelControlDept.TabIndex = 1;
            this.labelControlDept.Text = "科室：";
            // 
            // InWardPatRecInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.xtraTabControl1);
            this.Name = "InWardPatRecInfo";
            this.Size = new System.Drawing.Size(838, 635);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage_Master.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_Master)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            this.xtraTabPage_Detail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage_Master;
        private DevExpress.XtraGrid.GridControl gridControl_Master;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewMain;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btn_Refresh;
        private DevExpress.XtraEditors.SimpleButton btn_Export;
        private DevExpress.XtraEditors.SimpleButton btn_ViewDetail;
        private DevExpress.XtraEditors.LabelControl labelControlInfo;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage_Detail;
        private DevExpress.XtraGrid.Columns.GridColumn col_DEPTNAME;
        private DevExpress.XtraGrid.Columns.GridColumn colTotalPat;
        private DevExpress.XtraGrid.Columns.GridColumn COL_TOTALBEDS;
        private DevExpress.XtraGrid.Columns.GridColumn col_DanagerPats;
        private DevExpress.XtraGrid.Columns.GridColumn col_PATS;
        private DevExpress.XtraGrid.Columns.GridColumn col_NoPatRec;
        private DevExpress.XtraGrid.Columns.GridColumn col_Spot;
        private DevExpress.XtraGrid.Columns.GridColumn col_AVERAGE;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewDetail;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btn_PrintMaster;
        private DevExpress.XtraGrid.Columns.GridColumn col_Status;
        private DevExpress.XtraGrid.Columns.GridColumn col_PatID;
        private DevExpress.XtraGrid.Columns.GridColumn COL_PATNAME;
        private DevExpress.XtraGrid.Columns.GridColumn COL_PATSEX;
        private DevExpress.XtraGrid.Columns.GridColumn COL_AGE;
        private DevExpress.XtraGrid.Columns.GridColumn COL_PATBED;
        private DevExpress.XtraGrid.Columns.GridColumn col_INCOUNT;
        private DevExpress.XtraGrid.Columns.GridColumn COL_INHOSPITAL;
        private DevExpress.XtraGrid.Columns.GridColumn COL_OUTTIME;
        private DevExpress.XtraGrid.Columns.GridColumn col_OUTFILE;
        private DevExpress.XtraGrid.Columns.GridColumn COL_PATFILES;
        private DevExpress.XtraGrid.Columns.GridColumn COL_PATDIAG;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.LabelControl labelControlPATRECS;
        private DevExpress.XtraEditors.LabelControl labelControlTotalPats;
        private DevExpress.XtraEditors.LabelControl labelControlDept;
        private DevExpress.XtraGrid.Columns.GridColumn col_DEPARTID;
        private DevExpress.XtraEditors.SimpleButton btn_ViewPatRec;
        private DevExpress.XtraEditors.SimpleButton btn_Return;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labNoRecord;
        private DevExpress.XtraEditors.LabelControl labPatCount;
        private DevExpress.XtraEditors.LabelControl labDeptName;
    }
}
