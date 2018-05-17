using System;
using System.Globalization;
using System.Windows.Forms;

namespace DrectSoft.Wordbook
{
    /// <summary>
    /// SqlWordbook属性的编辑窗口
    /// </summary>
    public partial class FormSqlWordbook : Form
    {
        #region properties
        /// <summary>
        /// 当前选择的字典类
        /// </summary>
        public SqlWordbook Wordbook
        {
            get { return _wordbook; }
            set
            {
                _wordbook = value;
                // 用选中的字典初始化界面
                InitializeEditBox();
            }
        }
        private SqlWordbook _wordbook;
        #endregion

        /// <summary>
        /// 创建Sql字典设置窗口
        /// </summary>
        public FormSqlWordbook()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 用当前选中的字典类的信息初始化编辑界面
        /// </summary>
        private void InitializeEditBox()
        {
            if (_wordbook == null)
                return;

            // 基本信息
            tBoxName.Text = _wordbook.WordbookName;
            tBoxQuery.Text = _wordbook.QuerySentence;
            tBoxCodeField.Text = _wordbook.CodeField;
            tBoxNameField.Text = _wordbook.NameField;
            tBoxFilter.Text = _wordbook.MatchFieldComb;
            lvColumnStyles.Items.Clear();
            foreach (GridColumnStyle style in _wordbook.DefaultGridStyle)
                lvColumnStyles.Items.Add(new ListViewItem(new string[] { 
               style.FieldName, style.Caption, style.Width.ToString(CultureInfo.CurrentCulture)}));
            tBoxFieldName.Text = String.Empty;
            tBoxCaption.Text = String.Empty;
            tBoxWidth.Text = String.Empty;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // 保存编辑过的值
            SaveChanges();
        }

        /// <summary>
        /// 保存对类做过的修改
        /// </summary>
        private void SaveChanges()
        {
            try
            {
                CheckValue();
            }
            catch (ArgumentNullException ae)
            {
                MessageBox.Show(ae.Message, "提示", MessageBoxButtons.OK
                   , MessageBoxIcon.Information
                   , MessageBoxDefaultButton.Button1
                   , MessageBoxOptions.DefaultDesktopOnly);
                return;
            }
            catch
            {
                throw;
            }

            GridColumnStyleCollection styleCollection = new GridColumnStyleCollection();
            if (lvColumnStyles.Items.Count > 0)
            {
                foreach (ListViewItem item in lvColumnStyles.Items)
                    styleCollection.Add(new GridColumnStyle(item.Text
                       , item.SubItems[1].Text
                       , Convert.ToInt32(item.SubItems[2].Text, CultureInfo.CurrentCulture)));
            }
            _wordbook = new SqlWordbook(tBoxName.Text.Trim()
               , tBoxQuery.Text.Trim()
               , tBoxCodeField.Text.Trim()
               , tBoxNameField.Text.Trim()
               , styleCollection
               , tBoxFilter.Text.Trim());
        }

        /// <summary>
        /// 检查输入值是否合法
        /// </summary>
        /// <returns></returns>
        private void CheckValue()
        {
            if (tBoxName.Text.Trim().Length == 0)
            {
                throw new ArgumentNullException(MessageStringManager.GetString("SqlWordbookNeedName"));
            }

            if (tBoxQuery.Text.Trim().Length == 0)
            {
                throw new ArgumentNullException(MessageStringManager.GetString("SqlWordbookNeedQurySentence"));
            }

            if (tBoxCodeField.Text.Trim().Length == 0)
            {
                throw new ArgumentNullException(MessageStringManager.GetString("SqlWordbookNeedCodeFieldName"));
            }

            if (tBoxNameField.Text.Trim().Length == 0)
            {
                throw new ArgumentNullException(MessageStringManager.GetString("SqlWordbookNeedNameFieldName"));
            }
        }

        private void lvColumnStyles_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDelete.Enabled = (lvColumnStyles.SelectedItems != null);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string field = tBoxFieldName.Text.Trim();
            string caption = tBoxCaption.Text.Trim();
            string width = tBoxWidth.Text.Trim();
            if ((field.Length == 0) || (caption.Length == 0))
                return;
            try
            {
                Convert.ToInt32(width, CultureInfo.CurrentCulture);
            }
            catch
            {
                return;
            }

            lvColumnStyles.Items.Add(new ListViewItem(new string[] {
            field, caption, width}, -1));
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lvColumnStyles.SelectedItems.Count > 0)
            {
                lvColumnStyles.Items.Remove(lvColumnStyles.SelectedItems[0]);
            }
        }

    }
}