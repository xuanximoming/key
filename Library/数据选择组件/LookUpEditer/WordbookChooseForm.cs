using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using DrectSoft.Core;
using DrectSoft.Wordbook;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace DrectSoft.Common.Library
{
    /// <summary>
    /// 选择并设置预定义字典的窗口
    /// </summary>
    [ToolboxBitmapAttribute(typeof(DrectSoft.Common.Library.WordbookChooseForm), "Images.WordbookChooseForm.ico")]
    public partial class WordbookChooseForm : Form
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

        /// <summary>
        /// 连接数据库的对象
        /// </summary>
        public IDataAccess SqlExecutor
        {
            get { return _sqlExecutor; }
            set
            {
                _sqlExecutor = value;
                btnPreviewData.Enabled = (_sqlExecutor != null);
            }
        }
        private IDataAccess _sqlExecutor;
        #endregion

        #region private variable
        private DataTable m_PreviewTable;
        private DataTable m_ParameterTable;
        private DataTable m_DataCatalogTable;
        private Dictionary<int, RepositoryItemLookUpEdit> m_RepItemSelDataCatalogs;
        #endregion

        /// <summary>
        /// 创建字典选择窗口实例
        /// </summary>
        public WordbookChooseForm()
        {
            InitializeComponent();

            // 此处要改进！！！
            // 设计时调用，需要预览数据、验证SQL语句时，则首先出现数据连接向导，提供两种方式：
            //    1）指定配置文件，选择Database
            //    2）配置ADO连接串 

            //splitContainer1.Panel1.MinSize = 170;
            //splitContainer1.Panel2.MinSize = 420;
            btnPreviewData.Enabled = false;
            // 暂时在代码内部创建数据连接，以后要改成从外部传入！！！
            SqlExecutor = new SqlDataAccess("EMRDB");
            if (SqlExecutor != null)
            {
                string command = "select ID, Name, CategoryID, Py, Wb, Memo from CategoryDetail order by CategoryID, ID";

                m_DataCatalogTable = SqlExecutor.ExecuteDataTable(command);

            }
        }

        #region public method
        /// <summary>
        /// 以默认方式调用预定义字典选择窗口
        /// </summary>
        /// <returns>选中字典的关键信息组成的字符串</returns>
        public string ShowForm()
        {
            return CallSelectForm();
        }

        /// <summary>
        /// 用传入的字典关键信息初始化选中字典，并调用预定义字典选择窗口
        /// </summary>
        /// <param name="keyInfo">字典的关键信息组成的字符串</param>
        /// <returns>选中字典的关键信息组成的字符串</returns>
        public string ShowForm(string keyInfo)
        {
            _wordbook = WordbookStaticHandle.GetWordbookByString(keyInfo);
            return CallSelectForm();
        }

        #endregion

        #region private methods
        /// <summary>
        /// 显示预定义字典选择窗口
        /// </summary>
        /// <returns>选中字典的关键信息组成的字符串</returns>
        private string CallSelectForm()
        {
            InitializeEditBox();
            if ((this.ShowDialog() == DialogResult.OK) && (_wordbook != null))
            {
                return _wordbook.Caption + SeparatorSign.OtherSeparator
                   + _wordbook.WordbookName + SeparatorSign.OtherSeparator
                   + _wordbook.ParameterValueComb;
            }
            else
            {
                _wordbook = null;
                return "";
            }
        }

        /// <summary>
        /// 用当前选中的字典类的信息初始化编辑界面
        /// </summary>
        private void InitializeEditBox()
        {
            gridViewData.Columns.Clear();
            gridCtrlData.DataSource = null;
            if (_wordbook == null)
            {
                // 基本信息
                tBoxName.Text = "";
                gridCtrlPara.DataSource = null;
                m_ParameterTable = null;
                m_PreviewTable = null;
            }
            else
            {
                // 基本信息
                tBoxName.Text = _wordbook.Caption;

                m_ParameterTable = _wordbook.Parameters.Convert2DataTable();
                m_ParameterTable.DefaultView.RowFilter = "AllowUserEdit = 1";

                gridCtrlPara.DataSource = m_ParameterTable;
                // 生成预览数据
                if (_sqlExecutor != null)
                {
                    string commandText = _wordbook.QuerySentence; // .GenerateSqlSentence();
                    m_PreviewTable = SqlExecutor.ExecuteDataTable(commandText, CommandType.Text);
                    m_PreviewTable.DefaultView.RowFilter = _wordbook.GenerateFilterExpression();

                    gridViewData.Columns.Clear();
                    // gridViewData.Columns.AddRange(_wordbook.GenerateDevGridColumnCollection());

                    gridCtrlData.DataSource = m_PreviewTable;
                }
            }
        }
        #endregion

        #region events

        private void wordbookTree_TreeDoubleClick(object sender, EventArgs e)
        {
            if (WordbookTree.CurrentBookInfo.TypeName != null)
            {
                _wordbook = WordbookStaticHandle.GetWordbook(WordbookTree.CurrentBookInfo.TypeName);
                // 用选中的字典初始化界面
                InitializeEditBox();
            }
        }

        private void btnPreviewData_Click(object sender, EventArgs e)
        {
            m_PreviewTable.DefaultView.RowFilter = _wordbook.GenerateFilterExpression();
        }
        #endregion

        private void gridCtrlPara_Leave(object sender, EventArgs e)
        {
            // 保存用户修改过的参数值
            if (m_ParameterTable == null)
                return;

            foreach (DataRow row in m_ParameterTable.Rows)
            {
                if (Convert.ToBoolean(row["Enable"], CultureInfo.CurrentCulture))
                {
                    _wordbook.Parameters[row["Name"].ToString().Trim()].Enabled = true;
                    _wordbook.Parameters[row["Name"].ToString().Trim()].Value = row["Value"].ToString();
                }
                else
                {
                    _wordbook.Parameters[row["Name"].ToString().Trim()].Enabled = false;
                }
            }
        }

        private int GetDataCatalogOfRow(int rowHandle)
        {
            if (rowHandle < 0)
                return -1;
            DataRow sourceRow = gridViewPara.GetDataRow(rowHandle);
            if ((Convert.ToBoolean(sourceRow["IsString"], CultureInfo.CurrentCulture))
               || (sourceRow["DataSort"].ToString().Length == 0))
                return -1;

            return Convert.ToInt32(sourceRow["DataSort"], CultureInfo.CurrentCulture);
        }

        private void gridViewPara_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            if (e.Column.FieldName == "Value")
            {
                int dataSort = GetDataCatalogOfRow(e.RowHandle);
                if (dataSort >= 0)
                {
                    if (m_DataCatalogTable != null)
                        e.RepositoryItem = GetRepositoryItem(dataSort);
                }
            }
        }

        private RepositoryItem GetRepositoryItem(int dataSort)
        {
            if (dataSort < 0)
                return null;

            if (m_DataCatalogTable == null)
                return null;

            if (m_RepItemSelDataCatalogs == null)
                m_RepItemSelDataCatalogs = new Dictionary<int, RepositoryItemLookUpEdit>();

            if (m_RepItemSelDataCatalogs.ContainsKey(dataSort))
                return m_RepItemSelDataCatalogs[dataSort];

            m_DataCatalogTable.DefaultView.RowFilter = "CategoryID = " + dataSort.ToString();

            RepositoryItemLookUpEdit newRepItem = new RepositoryItemLookUpEdit();
            newRepItem.BeginInit();

            newRepItem.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            newRepItem.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ID", "", 40, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.Ascending),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name", "", 100, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None)});
            newRepItem.DisplayMember = "Name";
            newRepItem.DataSource = m_DataCatalogTable.DefaultView.ToTable();
            newRepItem.ValueMember = "ID";
            gridCtrlPara.RepositoryItems.Add(newRepItem);

            newRepItem.EndInit();
            m_RepItemSelDataCatalogs.Add(dataSort, newRepItem);
            return newRepItem;
        }
    }
}