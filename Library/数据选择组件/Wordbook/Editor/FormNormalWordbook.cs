using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace DrectSoft.Wordbook
{
   /// <summary>
   /// BaseWordbook属性的编辑窗口
   /// </summary>
   public partial class FormNormalWordbook : Form
   {
      #region properties
      /// <summary>
      /// 当前选择的字典类
      /// </summary>
      public BaseWordbook Wordbook
      {
         get { return _wordbook; }
         set 
         {
            _wordbook = value; 
            // 用选中的字典初始化界面
            InitializeEditBox();
         }
      }
      private BaseWordbook _wordbook;
      #endregion

      /// <summary>
      /// 创建普通字典对象设置窗口
      /// </summary>
      public FormNormalWordbook()
      {
         InitializeComponent();

         dgvInputParas.AutoGenerateColumns = false;
         splitContainer1.Panel1MinSize = 170;
         splitContainer1.Panel2MinSize = 420;
         tabControl1.SelectedTab = tabPageBasic;
      }

      #region private methods
      /// <summary>
      /// 用当前选中的字典类的信息初始化编辑界面
      /// </summary>
      private void InitializeEditBox()
      {
         if (_wordbook == null)
            return;

         // 基本信息
         tBoxName.Text = _wordbook.Caption;
         tBoxClassName.Text = _wordbook.WordbookName;
         tBoxQuery.Text = _wordbook.QuerySentence;
         tBoxExtra.Text = _wordbook.ExtraCondition;
         // 初始值
         treeViewFields.Nodes.Clear();
         TreeNode node;
         foreach (string field in _wordbook.CurrentMatchFields)
         { 
            node = new TreeNode(field);
            treeViewFields.Nodes.Add(node);
         }

         dgvInputParas.DataSource = _wordbook.Parameters.Convert2DataTable();
         tBoxDefValue.Text = _wordbook.ParameterValueComb;
         // 列显示方案(根据显示方案动态生成控件)
         GenerateShowStylePlans();
      }

      /// <summary>
      /// 根据显示方案动态生成RadioButton和DataGridView的组合
      /// </summary>
      private void GenerateShowStylePlans()
      {
         // 首先清除动态生成的控件
         DataGridView gridView;
         DataGridViewColumnCollection cols;
         for (int i = tabPageShow.Controls.Count - 1; i >= 0; i--)
         {
            // 如果是DataGridView，则先释放创建的Column
            gridView = tabPageShow.Controls[i] as DataGridView;
            if (gridView == null)
            {
               tabPageShow.Controls[i].Dispose();
            }
            else
            {
               cols = gridView.Columns;
               foreach (DataGridViewColumn col in cols)
                  col.Dispose();
               gridView.Dispose();
            }
         }

         RadioButton radBtn;
         DataGridViewColumn newCol;
         for (int index = 0; index < _wordbook.ShowStyles.Count; index++)
         {
            // 创建RadioButton
            radBtn = new RadioButton();
            radBtn.AutoSize = true;
            radBtn.FlatStyle = FlatStyle.Popup;
            radBtn.Location = new Point(5, 12 + 44 * index);
            radBtn.Name = "rdBtn_" + index.ToString(CultureInfo.CurrentCulture);
            radBtn.Text = "";
            tabPageShow.Controls.Add(radBtn);
            // 默认选中第一个方案
            if (index == 0)
               radBtn.Checked = true;

            // 创建DataGridView
            gridView = new DataGridView();
            gridView.Name = "gridView_" + index.ToString(CultureInfo.CurrentCulture);
            gridView.AllowUserToAddRows = false;
            gridView.ReadOnly = true;
            gridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            gridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            gridView.Location = new Point(22, 2 + 44 * index);
            gridView.RowHeadersWidth = 4;
            gridView.RowTemplate.Height = 23;
            gridView.Size = new System.Drawing.Size(380, 42);
            gridView.ScrollBars = ScrollBars.Both;
   
            // 创建DataGridView的Column集合
            //cols = new DataGridViewColumnCollection(gridView);
            foreach (GridColumnStyle style in _wordbook.ShowStyles[index])
            {
               newCol = new DataGridViewTextBoxColumn();
               newCol.Frozen = true;
               newCol.ReadOnly = true;
               //newCol.DefaultCellStyle = dgvInputParas.DefaultCellStyle;
               newCol.HeaderText = style.Caption;
               newCol.Width = style.Width;
               gridView.Columns.Add(newCol);
            }
            tabPageShow.Controls.Add(gridView);
         }
      }

      /// <summary>
      /// 交换代码字段树中两个字段的位置
      /// </summary>
      /// <param name="isUp">向上</param>
      private void ChangeFieldsRowPosition(bool isUp)
      {
         TreeNode node = treeViewFields.SelectedNode;
         int index = node.Index;

         if (((index <= 0) && (isUp))
            || ((index >= (treeViewFields.Nodes.Count - 1)) && (!isUp)))
            return;

         treeViewFields.Nodes.Remove(node);
         if (isUp)
            treeViewFields.Nodes.Insert(index - 1, node);
         else
            treeViewFields.Nodes.Insert(index + 1, node);
         treeViewFields.SelectedNode = node;
      }

      /// <summary>
      /// 保存对类做过的修改
      /// </summary>
      private void SaveChanges()
      {
         if (_wordbook == null)
            return;

         // 附加条件
         if (tBoxExtra.Text.Trim().Length > 0)
            _wordbook.ExtraCondition = tBoxExtra.Text.Trim();

         // 作为代码列的字段
         if (treeViewFields.Nodes.Count > 0)
         {
            StringBuilder values = new StringBuilder(treeViewFields.Nodes[0].Text);
            for (int index = 1; index < treeViewFields.Nodes.Count; index++ )
            {
               values.Append(SeparatorSign.ListSeparator);
               values.Append(treeViewFields.Nodes[index].Text);
            }
            _wordbook.MatchFieldComb = values.ToString();
         }

         // 参数默认值
         if (tBoxDefValue.Text.Trim().Length > 0)
            _wordbook.ParameterValueComb = tBoxDefValue.Text.Trim();

         // 显示方案
         RadioButton radBtn;
         foreach (Control ctrl in tabPageShow.Controls)
         {
            radBtn = ctrl as RadioButton;
            if (radBtn != null)
            {
               if (radBtn.Checked)
               {
                  string[] splits = radBtn.Name.Split('_');
                  _wordbook.SelectedStyleIndex = Convert.ToInt32(splits[1], CultureInfo.CurrentCulture);
                  break;
               }
            }
         }
      }
      #endregion

      #region events
      private void btnOk_Click(object sender, EventArgs e)
      {
         // 保存编辑过的值
         SaveChanges();
      }

      private void btnUp_Click(object sender, EventArgs e)
      {
         ChangeFieldsRowPosition(true);
      }

      private void btnDown_Click(object sender, EventArgs e)
      {
         ChangeFieldsRowPosition(false);
      }

      private void dgvInputParas_Leave(object sender, EventArgs e)
      {
         StringBuilder values = new StringBuilder();
         DataRowView sourceRow;
         
         // 组合设置了默认值的参数         
         foreach(DataGridViewRow row in dgvInputParas.Rows)
         {
            sourceRow = row.DataBoundItem as DataRowView;
            if (Convert.ToBoolean(sourceRow["Enable"], CultureInfo.CurrentCulture))
            {
               if (values.Length > 0)
                  values.Append(SeparatorSign.ListSeparator);
               values.Append(sourceRow["Name"].ToString().Trim());
               values.Append(SeparatorSign.ListSeparator);
               values.Append(row.Cells["ColValue"].EditedFormattedValue);
            }
         }
         tBoxDefValue.Text = values.ToString();
      }

      private void wordbookTree_TreeDoubleClick(object sender, EventArgs e)
      {
         if (wordbookTree.CurrentBookInfo.TypeName != null)
         {
            _wordbook = WordbookStaticHandle.GetWordbook(wordbookTree.CurrentBookInfo.TypeName);
            // 用选中的字典初始化界面
            InitializeEditBox();
         }
      }
      #endregion
   }
}