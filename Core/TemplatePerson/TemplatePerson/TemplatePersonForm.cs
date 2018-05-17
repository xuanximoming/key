using DevExpress.XtraBars;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Wordbook;

namespace DrectSoft.Core.TemplatePerson
{
    public partial class TemplatePersonForm : Form
    {
        private int m_NodeID = 0;
        private IEmrHost m_App;
        public TemplatePersonForm()
        {
            InitializeComponent();
        }
        public TemplatePersonForm(IEmrHost app)
            : this()
        {
            this.m_App = app;
            DataAccess.App = app;
            this.InitPersonModel();
            this.GetPersonTemplate();
            this.SetGridControlColor();
            this.treeListTemplatePerson.ExpandAll();
            this.InitLookUpEditorData();
        }

        private void InitPersonModel()
        {
            this.treeListTemplatePerson.Nodes.Clear();
            this.ModelName.OptionsColumn.AllowEdit = false;
            this.TemplatePersonID.OptionsColumn.AllowEdit = false;
            this.TemplateID.OptionsColumn.AllowEdit = false;
        }

        private void gridControlTemplatePerson_MouseDown(object sender, MouseEventArgs e)
        {
            this.hitInfo = this.gridViewTemplatePerson.CalcHitInfo(new Point(e.X, e.Y));
        }

        private void gridControlTemplatePerson_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.hitInfo != null)
            {
                if (e.Button == MouseButtons.Left)
                {
                    Rectangle rectangle = new Rectangle(new Point(this.hitInfo.HitPoint.X - SystemInformation.DragSize.Width / 2, this.hitInfo.HitPoint.Y - SystemInformation.DragSize.Height / 2), SystemInformation.DragSize);
                    if (!rectangle.Contains(new Point(e.X, e.Y)))
                    {
                        if (this.hitInfo.HitTest == GridHitTest.RowEdge || this.hitInfo.HitTest == GridHitTest.RowCell)
                        {
                            this.gridControlTemplatePerson.DoDragDrop(this.gridViewTemplatePerson.GetRow(this.hitInfo.RowHandle), DragDropEffects.Copy);
                        }
                    }
                }
            }
        }

        private void treeListTemplatePerson_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void treeListTemplatePerson_DragDrop(object sender, DragEventArgs e)
        {
            DataRowView dragNode = this.GetDragNode(e.Data);
            if (dragNode != null)
            {
                TreeListHitInfo treeListHitInfo = this.treeListTemplatePerson.CalcHitInfo(this.treeListTemplatePerson.PointToClient(new Point(e.X, e.Y)));
                object[] nodeData = new object[]
				{
					dragNode.Row["Name"].ToString(),
					dragNode.Row["TemplateID"].ToString(),
					Convert.ToInt32(dragNode.Row["ID"])
				};
                TreeListNode treeListNode = treeListHitInfo.Node;
                if (treeListHitInfo.Node != null)
                {
                    if (treeListHitInfo.Node.ParentNode != null)
                    {
                        if (treeListHitInfo.Node.GetValue("TemplateID").ToString() != "")
                        {
                            treeListNode = this.treeListTemplatePerson.AppendNode(nodeData, treeListHitInfo.Node.ParentNode.Id);
                        }
                        else
                        {
                            treeListNode = this.treeListTemplatePerson.AppendNode(nodeData, treeListHitInfo.Node.Id);
                        }
                    }
                    else if (treeListHitInfo.Node.ParentNode == null)
                    {
                        if (treeListHitInfo.Node.GetValue("TemplateID").ToString() != "")
                        {
                            treeListNode = this.treeListTemplatePerson.AppendNode(nodeData, -1);
                        }
                        else
                        {
                            treeListNode = this.treeListTemplatePerson.AppendNode(nodeData, treeListHitInfo.Node.Id);
                        }
                    }
                }
                else
                {
                    treeListNode = this.treeListTemplatePerson.AppendNode(nodeData, -1);
                }
                treeListNode.SelectImageIndex = 1;
                treeListNode.ImageIndex = 1;
                if (treeListNode.ParentNode != null)
                {
                    treeListNode.ParentNode.Expanded = true;
                }
                this.treeListTemplatePerson.Focus();
                this.treeListTemplatePerson.FocusedNode = treeListNode;
                this.UpdateUsedField();
            }
        }

        private DataRowView GetDragNode(IDataObject data)
        {
            return data.GetData(typeof(DataRowView)) as DataRowView;
        }

        private void treeListTemplatePerson_MouseUp(object sender, MouseEventArgs e)
        {
            TreeListHitInfo treeListHitInfo = this.treeListTemplatePerson.CalcHitInfo(new Point(e.X, e.Y));
            this.barButtonItemDelete.Visibility = BarItemVisibility.Always;
            this.barButtonItemRename.Visibility = BarItemVisibility.Always;
            this.barButtonItemAddFolder.Visibility = BarItemVisibility.Always;
            this.barButtonItemUnExpendNode.Visibility = BarItemVisibility.Always;
            this.barButtonItemExpendNode.Visibility = BarItemVisibility.Always;
            if (e.Button == MouseButtons.Right)
            {
                if (treeListHitInfo.Node != null)
                {
                    this.barButtonItemDelete.Visibility = BarItemVisibility.Always;
                    this.barButtonItemRename.Visibility = BarItemVisibility.Always;
                    if (treeListHitInfo.Node.GetValue("TemplateID").ToString() == "")
                    {
                        this.barButtonItemAddFolder.Visibility = BarItemVisibility.Always;
                    }
                    else
                    {
                        this.barButtonItemAddFolder.Visibility = BarItemVisibility.Never;
                    }
                    if (treeListHitInfo.Node.Nodes.Count > 0)
                    {
                        if (treeListHitInfo.Node.Expanded)
                        {
                            this.barButtonItemUnExpendNode.Visibility = BarItemVisibility.Always;
                            this.barButtonItemExpendNode.Visibility = BarItemVisibility.Never;
                        }
                        else
                        {
                            this.barButtonItemUnExpendNode.Visibility = BarItemVisibility.Never;
                            this.barButtonItemExpendNode.Visibility = BarItemVisibility.Always;
                        }
                    }
                    else
                    {
                        this.barButtonItemUnExpendNode.Visibility = BarItemVisibility.Never;
                        this.barButtonItemExpendNode.Visibility = BarItemVisibility.Never;
                    }
                    this.treeListTemplatePerson.FocusedNode = treeListHitInfo.Node;
                    this.treeListTemplatePerson.Focus();
                }
                else
                {
                    this.barButtonItemUnExpendNode.Visibility = BarItemVisibility.Never;
                    this.barButtonItemExpendNode.Visibility = BarItemVisibility.Never;
                    this.barButtonItemDelete.Visibility = BarItemVisibility.Never;
                    this.barButtonItemRename.Visibility = BarItemVisibility.Never;
                    this.treeListTemplatePerson.FocusedNode = null;
                }
                this.popupMenuTreeList.ShowPopup(this.treeListTemplatePerson.PointToScreen(new Point(e.X, e.Y)));
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (treeListHitInfo.Node == null)
                {
                    this.ModelName.OptionsColumn.AllowEdit = false;
                    this.treeListTemplatePerson.FocusedNode = null;
                }
            }
        }

        private void barButtonItemAddFolder_ItemClick(object sender, ItemClickEventArgs e)
        {
            object[] nodeData = new object[]
			{
				"新增文件夹",
				"",
				-1
			};
            TreeListNode treeListNode;
            if (this.treeListTemplatePerson.FocusedNode == null)
            {
                treeListNode = this.treeListTemplatePerson.AppendNode(nodeData, -1);
            }
            else
            {
                treeListNode = this.treeListTemplatePerson.AppendNode(nodeData, this.treeListTemplatePerson.FocusedNode.Id);
            }
            treeListNode.SelectImageIndex = 2;
            treeListNode.ImageIndex = 2;
            this.treeListTemplatePerson.Focus();
            this.treeListTemplatePerson.FocusedNode = treeListNode;
        }

        private void barButtonItemRename_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.treeListTemplatePerson.FocusedNode != null)
            {
                this.ModelName.OptionsColumn.AllowEdit = true;
                this.m_FocusNode = this.treeListTemplatePerson.FocusedNode;
            }
        }

        private void treeListTemplatePerson_DoubleClick(object sender, EventArgs e)
        {
            if (this.treeListTemplatePerson.FocusedNode != null)
            {
                this.ModelName.OptionsColumn.AllowEdit = true;
                this.m_FocusNode = this.treeListTemplatePerson.FocusedNode;
            }
        }

        private void barButtonItemDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.treeListTemplatePerson.FocusedNode != null)
            {
                if (this.treeListTemplatePerson.FocusedNode.GetValue("TemplateID").ToString() == "")
                {
                    if (this.treeListTemplatePerson.FocusedNode.Nodes.Count == 0)
                    {
                        if (this.m_App.CustomMessageBox.MessageShow("是否删除此文件夹?", DrectSoft.Core.CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
                        {
                            TreeListNode focusedNode = this.treeListTemplatePerson.FocusedNode;
                            this.treeListTemplatePerson.Nodes.Remove(focusedNode);
                        }
                    }
                    else if (this.treeListTemplatePerson.FocusedNode.Nodes.Count > 0)
                    {
                        if (this.m_App.CustomMessageBox.MessageShow("是否删除此文件夹下所有的文件?", DrectSoft.Core.CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
                        {
                            TreeListNode focusedNode = this.treeListTemplatePerson.FocusedNode;
                            this.treeListTemplatePerson.Nodes.Remove(focusedNode);
                        }
                    }
                }
                else if (this.m_App.CustomMessageBox.MessageShow("是否删除此文件?", DrectSoft.Core.CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
                {
                    TreeListNode focusedNode = this.treeListTemplatePerson.FocusedNode;
                    this.treeListTemplatePerson.Nodes.Remove(focusedNode);
                }
                this.UpdateUsedField();
            }
        }

        private void treeListPersonModel_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            if (this.m_FocusNode != null && this.m_FocusNode != this.treeListTemplatePerson.FocusedNode)
            {
                this.ModelName.OptionsColumn.AllowEdit = false;
                this.m_FocusNode = null;
            }
        }

        private void barButtonItemExpendNode_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.treeListTemplatePerson.FocusedNode.Expanded = true;
        }

        private void barButtonItemUnExpendNode_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.treeListTemplatePerson.FocusedNode.Expanded = false;
        }

        private void simpleButtonConfirm_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("NodeName");
            dataTable.Columns.Add("NodeID");
            dataTable.Columns.Add("ParentNodeID");
            dataTable.Columns.Add("TemplatePersonID");
            this.GetPersonalModelTreeNode(this.treeListTemplatePerson.Nodes, dataTable);
            this.SavePersonalModel(dataTable);
            this.SaveTemplatePerson();
            this.m_App.CustomMessageBox.MessageShow("保存完成!", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
        }

        private void GetPersonalModelTreeNode(TreeListNodes nodes, DataTable data)
        {
            this.m_NodeID = 0;
            this.GetPersonalModelTreeNode(nodes, data, -1);
        }

        private void GetPersonalModelTreeNode(TreeListNodes nodes, DataTable data, int nodeID)
        {
            foreach (TreeListNode treeListNode in nodes)
            {
                DataRow dataRow = data.NewRow();
                dataRow["NodeName"] = treeListNode.GetValue("ModelName");
                dataRow["NodeID"] = this.m_NodeID++;
                if (treeListNode.ParentNode != null)
                {
                    dataRow["ParentNodeID"] = nodeID;
                }
                else
                {
                    dataRow["ParentNodeID"] = -1;
                }
                if (treeListNode.GetValue("TemplatePersonID").ToString() != "")
                {
                    dataRow["TemplatePersonID"] = treeListNode.GetValue("TemplatePersonID");
                }
                else
                {
                    dataRow["TemplatePersonID"] = -1;
                }
                data.Rows.Add(dataRow);
                if (treeListNode.Nodes.Count > 0)
                {
                    this.GetPersonalModelTreeNode(treeListNode.Nodes, data, Convert.ToInt32(dataRow["NodeID"]));
                }
            }
        }

        private void SavePersonalModel(DataTable dt)
        {
            DataAccess.SavePersonalModel(dt);
        }

        private void SaveTemplatePerson()
        {
            DataTable dataTable = this.gridControlTemplatePerson.DataSource as DataTable;
            if (dataTable != null)
            {
                DataAccess.SaveTemplatePerson(dataTable);
            }
        }

        private void simpleButtonRecover_Click(object sender, EventArgs e)
        {
            if (this.m_App.CustomMessageBox.MessageShow("是否还原到保存前的状态?", DrectSoft.Core.CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
            {
                this.GetPersonTemplate();
                this.treeListTemplatePerson.ExpandAll();
            }
        }

        public void GetPersonTemplate()
        {
            this.treeListTemplatePerson.Nodes.Clear();
            DataSet templatePersonGroup = DataAccess.GetTemplatePersonGroup();
            DataTable dataTable = templatePersonGroup.Tables[0];
            DataTable dataTableTemplatePerson = templatePersonGroup.Tables[1];
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow dataRow = dataTable.Rows[i];
                string text = dataRow["NodeName"].ToString();
                int num = Convert.ToInt32(dataRow["NodeID"]);
                int parentNodeId = Convert.ToInt32(dataRow["ParentNodeID"]);
                string text2 = dataRow["TemplateID"].ToString();
                int num2 = Convert.ToInt32(dataRow["TemplatePersonID"]);
                object[] nodeData = new object[]
				{
					text,
					text2,
					num2
				};
                TreeListNode treeListNode = this.treeListTemplatePerson.AppendNode(nodeData, parentNodeId);
                if (text2 == "")
                {
                    treeListNode.ImageIndex = 2;
                    treeListNode.SelectImageIndex = 2;
                }
                else
                {
                    treeListNode.ImageIndex = 1;
                    treeListNode.SelectImageIndex = 1;
                }
            }
            this.m_DataTableTemplatePerson = dataTableTemplatePerson;
            this.gridControlTemplatePerson.DataSource = this.m_DataTableTemplatePerson.Copy();
            this.UpdateUsedField();
        }

        private void SetGridControlColor()
        {
            StyleFormatCondition styleFormatCondition = new StyleFormatCondition(FormatConditionEnum.Equal, this.gridViewTemplatePerson.Columns["Used"], null, "否");
            styleFormatCondition.Appearance.BackColor = Color.Red;
            styleFormatCondition.Appearance.ForeColor = Color.White;
            this.gridViewTemplatePerson.FormatConditions.Add(styleFormatCondition);
        }

        private void simpleButtonExit_Click(object sender, EventArgs e)
        {
            if (this.m_App.CustomMessageBox.MessageShow("是否关闭?", DrectSoft.Core.CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
            {
                base.Close();
            }
        }

        private void UpdateUsedField()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("NodeName");
            dataTable.Columns.Add("NodeID");
            dataTable.Columns.Add("ParentNodeID");
            dataTable.Columns.Add("TemplatePersonID");
            this.GetPersonalModelTreeNode(this.treeListTemplatePerson.Nodes, dataTable);
            DataTable dataTable2 = this.gridControlTemplatePerson.DataSource as DataTable;
            List<int> list = new List<int>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                int item = Convert.ToInt32(dataRow["TemplatePersonID"]);
                if (!list.Contains(item))
                {
                    list.Add(item);
                }
            }
            foreach (DataRow dataRow in dataTable2.Rows)
            {
                int item = Convert.ToInt32(dataRow["ID"]);
                if (!list.Contains(item))
                {
                    dataRow["Used"] = "否";
                }
                else
                {
                    dataRow["Used"] = "是";
                }
            }
        }

        private void InitLookUpEditorData()
        {
            this.lookUpWindowTemplateName.SqlHelper = this.m_App.SqlHelper;
            Dictionary<string, int> dictionary = new Dictionary<string, int>(1);
            dictionary.Add("Name", 395);
            SqlWordbook sqlWordbook = new SqlWordbook("queryTemp", this.m_DataTableTemplatePerson, "ID", "Name", dictionary, true);
            this.lookUpEditorTemplateName.SqlWordbook = sqlWordbook;
            this.lookUpEditorTemplateName.ListWindow = this.lookUpWindowTemplateName;
        }

        private void lookUpEditorTemplateName_CodeValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < this.gridViewTemplatePerson.RowCount; i++)
            {
                DataRowView dataRowView = this.gridViewTemplatePerson.GetRow(i) as DataRowView;
                if (dataRowView != null)
                {
                    string b = dataRowView["ID"].ToString();
                    if (this.lookUpEditorTemplateName.CodeValue == b)
                    {
                        this.gridViewTemplatePerson.FocusedRowHandle = i;
                        break;
                    }
                }
            }
        }
    }
}
