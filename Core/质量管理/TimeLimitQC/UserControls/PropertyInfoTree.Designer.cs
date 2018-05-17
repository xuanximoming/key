namespace DrectSoft.Core.TimeLimitQC
{
    partial class PropertyInfoTree
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
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
            components = new System.ComponentModel.Container();
            DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxOp;
            repositoryItemComboBoxOp = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            repositoryItemComboBoxOp.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumnOp = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn2 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn3 = new DevExpress.XtraTreeList.Columns.TreeListColumn();

            this.OptionsBehavior.Editable = true;
            this.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn1, this.treeListColumnOp, this.treeListColumn2, this.treeListColumn3});
            this.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[]{
                repositoryItemComboBoxOp});

            this.treeListColumn1.Caption = "名称";
            this.treeListColumn1.FieldName = "名称";
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.OptionsColumn.AllowEdit = false;
            this.treeListColumn1.VisibleIndex = 0;

            this.treeListColumnOp.ColumnEdit = repositoryItemComboBoxOp;
            this.treeListColumnOp.Caption = "算符";
            this.treeListColumnOp.FieldName = "算符";
            this.treeListColumnOp.Name = "treeListColumnOp";
            this.treeListColumnOp.OptionsColumn.AllowEdit = true;
            this.treeListColumnOp.VisibleIndex = 1;

            this.treeListColumn2.Caption = "值";
            this.treeListColumn2.FieldName = "值";
            this.treeListColumn2.Name = "treeListColumn2";
            this.treeListColumn2.OptionsColumn.AllowEdit = true;
            this.treeListColumn2.VisibleIndex = 2;

            this.treeListColumn3.Caption = "id";
            this.treeListColumn3.FieldName = "id";
            this.treeListColumn3.Name = "treeListColumn3";
            this.treeListColumn3.VisibleIndex = -1;

            repositoryItemComboBoxOp.AutoHeight = false;
            repositoryItemComboBoxOp.Items.AddRange(new object[] {
            DrectSoft.Core.TimeLimitQC.CompareOp.Equal,
            DrectSoft.Core.TimeLimitQC.CompareOp.Like});
            repositoryItemComboBoxOp.Name = "repositoryItemComboBoxOp";
        }
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn2;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn3;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumnOp;

        #endregion
    }
}
