/*****************************************************************************\
**             DrectSoft Software Co. Ltd.                          **
**                                                                           **
**  字典类的基类.                                                             **
**  提供默认的字典类属性和公共方法,子类在类的构造函数中为属性进行初始化.         ** 
**                                                                           **
\*****************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Collections;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using DevExpress.XtraGrid.Columns;
using System.Xml.Serialization;
using System.Diagnostics;
using DrectSoft.Common.Eop;

namespace DrectSoft.Wordbook
{
   /// <summary>
   /// 代码字典基类
   /// </summary>  
   [TypeConverter(typeof(NormalWordbookConverter))]
   public abstract class BaseWordbook
   {
      #region readonly property
      /// <summary>
      /// 字典名称（显示名称）
      /// </summary>
      [Description("字典显示名称"), ReadOnly(true)]
      public string Caption
      {
         get { return _caption; }
         internal set { _caption = value; }
      }
      private string _caption;

      /// <summary>
      /// 字典名称（上级命名空间+类名，可以作为数据集的名称）
      /// </summary>
      [Description("字典名称"), ReadOnly(true)]
      public string WordbookName
      {
         get { return _wordbookName; }
         internal set { _wordbookName = value; }
      }
      private string _wordbookName;

      /// <summary>
      /// 默认代码字段列名。通过代码反查名称时需要
      /// </summary>
      [Description("默认代码字段列名"), ReadOnly(true)]
      public string CodeField
      {
         get { return _codeField; }
         internal set { _codeField = value; }
      }
      private string _codeField;

      /// <summary>
      /// 默认代码字段列名。通过代码反查名称时需要
      /// </summary>
      [Description("代码字段数据类型是否是字符型"), ReadOnly(true)]
      public bool CodeFieldIsString
      {
         get { return _codeFieldIsString; }
         internal set { _codeFieldIsString = value; }
      }
      private bool _codeFieldIsString;

      /// <summary>
      /// 默认显示字段列名。该列的内容显示在编辑框中。
      /// </summary>
      [Description("默认显示字段列名"), ReadOnly(true)]
      public string NameField
      {
         get { return _nameField; }
         internal set { _nameField = value; }
      }
      private string _nameField;

      /// <summary>
      /// 默认查询代码字段列名。有些基础数据需要支持别名，所以在病历中需要用Code来定位记录，用QueryCode作为查询的条件。
      /// </summary>
      [Description("默认查询代码字段列名"), ReadOnly(true)]
      public string QueryCodeField
      {
         get
         {
            if (String.IsNullOrEmpty(_queryCodeField))
               return _codeField;
            else
               return _queryCodeField;
         }
         internal set { _queryCodeField = value; }
      }
      private string _queryCodeField;

      /// <summary>
      /// 查询数据的语句。
      /// 普通的查询语句，不用像以前一样包含 "py like :py" ，可以包含排序语句。
      /// 如果需要外部传入的参数，则在Where语句中预留参数的位置，形式是"@"+"参数名"，不管参数是何类型，都不要加引号。
      /// </summary>
      [Browsable(false)]
      public string QuerySentence
      {
         get { return _querySentence; }
         internal set { _querySentence = value; }
      }
      private string _querySentence;

      /// <summary>
      /// 当前使用的选择列字段名集合（根据需要经过了排序）
      /// </summary>
      [Browsable(false)]
      public Collection<string> CurrentMatchFields
      {
         get { return _currentMatchFields; }
         internal set { _currentMatchFields = value; }
      }
      private Collection<string> _currentMatchFields;

      /// <summary>
      /// 供选择的显示方案集合
      /// </summary>
      [Browsable(false)]
      public Collection<GridColumnStyleCollection> ShowStyles
      {
         get { return _showStyles; }
         internal set { _showStyles = value; }
      }
      private Collection<GridColumnStyleCollection> _showStyles;

      /// <summary>
      /// 过滤参数的集合。可以设置参数时候启用、增加参数、修改参数值
      /// </summary>
      [Browsable(false)]
      public FilterParameterCollection Parameters
      {
         get { return _parameters; }
         internal set { _parameters = value; }
      }
      private FilterParameterCollection _parameters;
      #endregion

      #region property
      /// <summary>
      /// 用来匹配数据的的字段名的组合，多个字段时用"//"隔开，排在前面的优先级高
      /// </summary>
      [Browsable(false), DesignOnly(true), Description(@"用来匹配数据的的字段名的组合,多个字段时用'//'隔开,排在前面的优先级高")]
      public string MatchFieldComb
      {
         get
         {
            if (_currentMatchFields.Count > 0)
            {
               StringBuilder values = new StringBuilder(_currentMatchFields[0]);
               for (int index = 1; index < _currentMatchFields.Count; index++)
               {
                  values.Append(SeparatorSign.ListSeparator);
                  values.Append(_currentMatchFields[index]);
               }
               return values.ToString();
            }
            else
               return "";
         }
         set
         {
            if ((value == null) || (value.Length == 0))
               throw new ArgumentNullException("必须指定代码列");

            string[] separator = new string[] { SeparatorSign.ListSeparator };
            string[] values = value.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            if (values.Length > _defaultFilterFields.Count)
               throw new ArgumentOutOfRangeException("指定的代码列数量超出默认代码的定义");

            // _filterField = value;
            _currentMatchFields.Clear();
            for (int i = 0; i < values.Length; i++)
            {
               _currentMatchFields.Add(values[i]);
            }
         }
      }

      /// <summary>
      /// 默认的作为代码列的字段名
      /// </summary>
      internal Collection<string> DefaultFilterFields
      {
         get { return _defaultFilterFields; }
         set { _defaultFilterFields = value; }
      }
      private Collection<string> _defaultFilterFields;

      /// <summary>
      /// 当前选中的显示方案编号
      /// </summary>
      [Description("默认的显示方案编号")]
      public int SelectedStyleIndex
      {
         get { return _selectedStyleIndex; }
         set
         {
            // 换显示方案时清空原先的Column集合
            if (_selectedStyleIndex != value)
            {
               ClearGridColumns();
            }

            if (value < 0)
               _selectedStyleIndex = 0;
            else if (value >= _showStyles.Count)
               _selectedStyleIndex = _showStyles.Count - 1;
            else
               _selectedStyleIndex = value;
         }
      }
      private int _selectedStyleIndex;

      /// <summary>
      /// 过滤参数的默认值组合。以“参数名//值”的形式组合
      /// </summary>
      [Browsable(false), DesignOnly(true), Description(@"过滤参数的默认值组合。以“参数名//值”的形式组合")]
      public string ParameterValueComb
      {
         get
         {
            if ((_parameters == null) || (_parameters.Count == 0))
               return "";
            else
            {
               StringBuilder values = new StringBuilder();
               for (int index = 0; index < _parameters.Count; index++)
               {
                  if (!_parameters[index].Enabled)
                     continue;
                  if (values.Length > 0)
                     values.Append(SeparatorSign.ListSeparator);
                  values.Append(_parameters[index].Caption);
                  values.Append(SeparatorSign.ListSeparator);
                  values.Append(_parameters[index].Value);
               }
               return values.ToString();
            }
         }
         set
         {
            if (value == null)
               throw new ArgumentNullException(MessageStringManager.GetString("NullParameter"));
            if ((_parameters != null) && (_parameters.Count > 0))
            {
               foreach (FilterParameter para in _parameters)
                  para.Enabled = false;
               if ((value == null) || (value.Length == 0))
               {
                  return;
               }
               string[] separator = new string[] { SeparatorSign.ListSeparator };
               string[] values = value.Split(separator, StringSplitOptions.None);

               int indexP;
               string temp;
               for (int i = 0; i < values.Length; i++)
               {
                  indexP = _parameters.IndexOf(values[i++]);
                  if (indexP >= 0)
                  {
                     _parameters[indexP].Enabled = true;
                     // 现在不管参数是何种类型，参数值都不用加单引号
                     // 因为已有的数据中字符型的参数值都已加上引号，所以在这里用代码去掉引号
                     if (_parameters[indexP].IsString)
                     {
                        temp = values[i];
                        if ((temp.Length > 1)
                           && (temp[0] == '\'') && (temp[temp.Length - 1] == '\''))
                           _parameters[indexP].Value = temp.Substring(1, temp.Length - 2);
                        else
                           _parameters[indexP].Value = temp;
                     }
                     else
                        _parameters[indexP].Value = values[i];
                  }
               }
            }
         }
      }

      /// <summary>
      /// 过滤数据的附加SQL条件
      /// </summary>
      [Browsable(false), Description("过滤数据的附加SQL条件(用在DataTable过虑条件中)")]
      public string ExtraCondition
      {
         get { return _extraCondition; }
         set { _extraCondition = value; }
      }
      private string _extraCondition;

      /// <summary>
      /// 字典数据在ShowList窗口中的缓存时间，单位秒，0表示无限制。
      /// </summary>
      [
        Description("字典数据在ShowList窗口中的缓存时间(单位:秒)，-1:不缓存, 0: 无限制。"),
        DefaultValue(0)
      ]
      public int CacheTime
      {
         get { return _cacheTime; }
         set
         {
            if (value < -1)
               _cacheTime = -1;
            else
               _cacheTime = value;
         }
      }
      private int _cacheTime;

      /// <summary>
      /// 根据当前选中的字典记录生成其对应的对象实例.
      /// 如果对象类中有对数据库的操作,要记得给对象的SqlHelper属性赋值
      /// </summary>
      [Browsable(false)]
      public EPBaseObject PersistentObject
      {
         get
         {
            if (_persistentObject == null)
            {
               // 创建对象
               return CreatePersistentWordbook();
            }
            else
               return _persistentObject;
         }
      }
      private EPBaseObject _persistentObject;

      /// <summary>
      /// 用来初始化当前选中字典记录对象的DataRow
      /// </summary>
      [Browsable(false), Description("用来初始化当前选中字典记录对象的DataRow")]
      public DataRow CurrentRow
      {
         get { return _currentRow; }
         set
         {
            if (value == null)
               _currentRow = null;
            else // 复制DataRow的值和结构
            {
               _currentRow = value.Table.NewRow();
               _currentRow.ItemArray = value.ItemArray;
            }
            _persistentObject = null;
         }
      }
      private DataRow _currentRow;
      #endregion

      #region internal variable
      /// <summary>
      /// 与当前显示方案一致的DataGridViewColumn集合
      /// </summary>
      internal DataGridViewColumn[] m_GridColumns;
      internal GridColumn[] m_DevGridColumns;
      ///// <summary>
      ///// CustomDraw设置集合
      ///// </summary>
      //internal Collection<CustomDrawSetting> m_DrawConditions;
      #endregion

      #region private methods
      /// <summary>
      /// 清空已创建的GridColumn
      /// </summary>
      private void ClearGridColumns()
      {
         if (m_GridColumns != null)
         {
            for (int i = 0; i < m_GridColumns.Length; i++)
               m_GridColumns[i].Dispose();
         }
         m_GridColumns = null;
      }

      private void ClearDxGridColumns()
      {
         if (m_DevGridColumns != null)
         {
            for (int i = 0; i < m_DevGridColumns.Length; i++)
               m_DevGridColumns[i].Dispose();
         }
         m_DevGridColumns = null;
      }

      /// <summary>
      /// 分解操作符为In的过滤参数，返回组合好的过滤条件表达式
      /// </summary>
      /// <param name="para"></param>
      private static string GenerateConditionFromInParameter(FilterParameter para)
      {
         string formatString = " {0} {1} {2}";
         StringBuilder expressions = new StringBuilder();
         // 先用','作为分隔符，拆分出独立的条件
         string[] separator1 = new string[] { "," };
         string[] separator2 = new string[] { "～" };
         string[] values = para.ParameterValue.ToString().Split(separator1, StringSplitOptions.None);
         string[] rangs;
         StringBuilder inValues = new StringBuilder(); // 保存In条件的单个值

         foreach (string condition in values)
         {
            if (condition.Contains("～")) // 范围型转换为">=" 和 "<="两个条件
            {
               if (expressions.Length > 0)
                  expressions.Append(" OR ");
               rangs = condition.Split(separator2, StringSplitOptions.None);
               expressions.AppendFormat(CultureInfo.CurrentCulture
                  , " ({0} >= {1} AND {0} <= {2})"
                  , para.FieldName
                  , rangs[0], rangs[1]);
            }
            else if (condition.Contains("%")) // 含有"%"的转换为"like"条件
            {
               if (expressions.Length > 0)
                  expressions.Append(" OR ");
               expressions.Append(String.Format(CultureInfo.CurrentCulture
                  , formatString
                  , para.FieldName
                  , CommonOperation.GetOperatorSign(CompareOperator.Like)
                  , condition));
            }
            else //其它的单个值保存下来作为In条件的值,最后统一加
            {
               if (inValues.Length > 0)
                  inValues.Append(',');
               inValues.Append(condition);
            }
         }
         if (inValues.Length > 0)
         {
            if (expressions.Length > 0)
               expressions.Append(" OR ");
            expressions.Append(String.Format(CultureInfo.CurrentCulture
               , "{0} in ({1})"
               , para.FieldName
               , inValues.ToString()));
         }
         return "(" + expressions.ToString() + ")";
      }

      /// <summary>
      /// 根据当前选中的DataRow生成PersistentWordbook实例
      /// </summary>
      /// <returns></returns>
      protected abstract EPBaseObject CreatePersistentWordbook();
      #endregion

      #region ctors
      /// <summary>
      /// 创建字典类实例
      /// </summary>
      protected BaseWordbook()
      { }

      /// <summary>
      /// 在DrectSoftWordbooks.XML中根据指定的字典名搜索定义数据，创建字典实例
      /// XML文件的数据要符合Wordbook.XSD的定义。下面的处理流程是按XSD的定义进行的。
      /// </summary>
      /// <param name="name">字典名称</param>
      protected BaseWordbook(string name)
      {
         Schema.Wordbook source = WordbookStaticHandle.GetSourceWordbookByName(name);

         if (String.IsNullOrEmpty(source.WordbookName))
            throw new ArgumentException("没有正确初始化字典，请检查基础数据");

         // 开始处理字典的属性  
         _wordbookName = source.WordbookName;
         _caption = source.Caption;
         _querySentence = PersistentObjectFactory.GetQuerySentenceByName(source.QuerySentence);
         _codeField = source.CodeField;
         _nameField = source.NameField;
         _queryCodeField = source.QueryCodeField;
         _codeFieldIsString = source.CodeFieldIsString;

         // 过滤参数
         _parameters = new FilterParameterCollection();
         _showStyles = new Collection<GridColumnStyleCollection>();
         _defaultFilterFields = new Collection<string>();

         if (source.FilterFieldCollection != null)
            foreach (string field in source.FilterFieldCollection)
               _defaultFilterFields.Add(field);

         if (source.ParameterCollection != null)
            _parameters.AddRange(source.ParameterCollection);

         GridColumnStyleCollection columnStyle;
         foreach (GridColumnStyle[] style in source.ViewStyleCollection)
         {
            columnStyle = new GridColumnStyleCollection();
            columnStyle.AddRange(style);
            _showStyles.Add(columnStyle);
         }

         // 下列属性使用默认值
         SelectedStyleIndex = 0;
         ExtraCondition = "";

         _currentMatchFields = new Collection<string>();
         foreach (string field in _defaultFilterFields)
            _currentMatchFields.Add(field);
      }

      /// <summary>
      /// 创建字典类对象的同时，初始化允许设置的属性
      /// </summary>
      /// <param name="name">要创建的字典的名称</param>param 
      /// <param name="filters">可以作为代码列的字段名,多个字段时用"\n"隔开，排在前面的优先级高</param>
      /// <param name="gridStyleIndex">默认选中的Grid显示方案</param>
      /// <param name="filterComb">输入参数的默认值,多个参数时用"\n"隔开</param>
      /// <param name="extraCondition">附件的查询条件</param>
      /// <param name="cacheTime">缓存时间</param>
      protected BaseWordbook(string name, string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : this(name)
      {
         this.MatchFieldComb = filters;
         this.ParameterValueComb = filterComb;
         this.SelectedStyleIndex = gridStyleIndex;
         this.ExtraCondition = extraCondition;
         this.CacheTime = cacheTime;
      }
      #endregion

      #region public methods
      /// <summary>
      /// 获取字典对象的 Expression（如果存在的话）
      /// </summary>
      /// <returns></returns>
      public override string ToString()
      {
         return this._wordbookName;
      }

      /// <summary>
      /// 获取当前字典类默认的过滤条件
      /// </summary>
      /// <returns></returns>
      public string GenerateFilterExpression()
      {
         string formatString = " {0} {1} {2}"; // 字段名 操作符 值
         StringBuilder expressions = new StringBuilder();
         // 增加附加条件
         if ((ExtraCondition != null) && (ExtraCondition.Length > 0))
            expressions.Append(ExtraCondition);

         // 增加参数条件
         foreach (FilterParameter para in _parameters)
         {
            if (!para.Enabled)
               continue;
            if (expressions.Length > 0)
               expressions.Append(" AND ");
            // 如果操作符是IN，则特殊处理(参数值可能是范围型的)
            if (para.Operator == CompareOperator.In)
               expressions.Append(GenerateConditionFromInParameter(para));
            else
               expressions.Append(String.Format(CultureInfo.CurrentCulture
                  , formatString
                  , para.FieldName
                  , CommonOperation.GetOperatorSign(para.Operator)
                  , CommonOperation.TransferCondition(para.ParameterValue, para.Operator)));
         }

         return expressions.ToString();
      }

      /// <summary>
      /// 根据当前显示方案，生成DataGridViewColumn集合
      /// </summary>
      /// <returns></returns>      
      public DataGridViewColumn[] GenerateGridColumnCollection()
      {
         if (m_GridColumns == null)
         {
            m_GridColumns = new DataGridViewColumn[_showStyles[_selectedStyleIndex].Count];

            DataGridViewCellStyle styleNormal = new DataGridViewCellStyle();
            styleNormal.NullValue = "";

            DataGridViewTextBoxColumn newColumn;
            for (int i = 0; i < m_GridColumns.Length; i++)
            {
               newColumn = new DataGridViewTextBoxColumn();

               newColumn.DataPropertyName = _showStyles[_selectedStyleIndex][i].FieldName;
               newColumn.DefaultCellStyle = styleNormal;
               newColumn.HeaderText = _showStyles[_selectedStyleIndex][i].Caption;
               newColumn.Name = "Col" + _showStyles[_selectedStyleIndex][i].FieldName;
               newColumn.Width = _showStyles[_selectedStyleIndex][i].Width;

               m_GridColumns[i] = newColumn;
            }
         }

         return m_GridColumns;
      }

      /// <summary>
      /// 根据当前显示方案，生成DevXtraGridColumn集合
      /// </summary>
      /// <returns></returns>      
      public GridColumn[] GenerateDevGridColumnCollection()
      {
         if (m_DevGridColumns == null)
         {
            m_DevGridColumns = new GridColumn[_showStyles[_selectedStyleIndex].Count];

            GridColumn newColumn;
            for (int i = 0; i < m_DevGridColumns.Length; i++)
            {
               newColumn = new GridColumn();

               newColumn.Caption = _showStyles[_selectedStyleIndex][i].Caption;
               newColumn.FieldName = _showStyles[_selectedStyleIndex][i].FieldName;
               newColumn.Name = "Col" + _showStyles[_selectedStyleIndex][i].FieldName;
               newColumn.Visible = true;
               newColumn.VisibleIndex = i;
               newColumn.Width = _showStyles[_selectedStyleIndex][i].Width;

               m_DevGridColumns[i] = newColumn;
            }
         }
         else // 去掉Column的排序等属性
         {
            foreach (GridColumn col in m_DevGridColumns)
            {
               col.ClearFilter();
               col.SortIndex = -1;
               col.SortOrder = DevExpress.Data.ColumnSortOrder.None;
            }
         }

         return m_DevGridColumns;
      }
      #endregion
   }
}
