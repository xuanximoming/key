using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Text;
using DrectSoft.Common.Eop;

namespace DrectSoft.Wordbook
{
   /// <summary>
   /// StringList代码字典类。可以利用StringList动态创建字典，以便用ShowList组件代替ComboBox
   /// </summary>
   public sealed class ListWordbook : BaseWordbook
   {
      #region properties
      ///// <summary>
      ///// 字典类名）
      ///// </summary>
      //public new string WordbookName
      //{
      //   get { return _wordbookName; }
      //   set 
      //   {
      //      Debug.Assert(value != null, "类名不能为NULL");
      //      Debug.Assert(value.Length > 0, "类名不能为空");
      //      _wordbookName = value; 
      //      // 字典名称与类名设成一样
      //      _title = value;
      //   }
      //}
      //private new string _wordbookName;

      /// <summary>
      /// 作为数据源的StringList
      /// </summary>
      public Collection<string> Items
      {
         get { return _items; }
      }
      private Collection<string> _items;
      #endregion 

      #region value properties
      /// <summary>
      /// 序列号
      /// </summary>
      public int SerialNo
      {
         get { return _serialNo; }
      }
      private int _serialNo;

      /// <summary>
      /// 名称
      /// </summary>
      public string Name
      {
         get { return _name; }
      }
      private string _name;

      /// <summary>
      /// 拼音
      /// </summary>
      public string PingYin
      {
         get { return _pingYin; }
      }
      private string _pingYin;

      /// <summary>
      /// 五笔
      /// </summary>
      public string FiveStrokes
      {
         get { return _fiveStrokes; }
      }
      private string _fiveStrokes;
      #endregion

      /// <summary>
      /// 用指定的字符串集合创建List型字典类实例
      /// </summary>
      /// <param name="uniqueName">局部唯一的名称</param>
      /// <param name="valueList">用来创建类的字符串集合</param>
      public ListWordbook(string uniqueName, Collection<string> valueList)
         : base()
      {
         Debug.Assert(uniqueName != null, "类名不能为NULL");
         Debug.Assert(uniqueName.Length > 0, "类名不能为空");
         WordbookName = uniqueName;
         Caption = WordbookName;
         //_name = "List字典";
         if (valueList != null)
            _items = new Collection<string>(valueList);
         else
            _items = new Collection<string>(); 
         
         ExtraCondition = "";
         QuerySentence = "";
         // 下列属性的设置不能修改，其它地方处理时可能会默认这些属性的值没有变过
         CodeField = "name";
         NameField = "name";
         QueryCodeField = CodeField;
         CodeFieldIsString = true;

         DefaultFilterFields = new Collection<string>();
         DefaultFilterFields.Add("xh");
         DefaultFilterFields.Add("py");
         DefaultFilterFields.Add("wb");
         DefaultFilterFields.Add("name");

         CurrentMatchFields = new Collection<string>();
         foreach (string field in DefaultFilterFields)
            CurrentMatchFields.Add(field);

         Parameters = new FilterParameterCollection();

         ShowStyles = new Collection<GridColumnStyleCollection>();
         ShowStyles.Add(new GridColumnStyleCollection());
         ShowStyles[0].AddRange(new GridColumnStyle[]{
             new GridColumnStyle("xh", "序号", 40)
            ,new GridColumnStyle("name", "名称", 80)
            ,new GridColumnStyle("py", "拼音", 70)
            ,new GridColumnStyle("wb", "五笔", 70)});
         SelectedStyleIndex = 0;

         //m_DrawConditions = new Collection<CustomDrawSetting>();
      }

      /// <summary>
      /// 清除属性字段的值
      /// </summary>
      public void ClearValueFields()
      {
         _name = "";
         _pingYin = "";
         _fiveStrokes = "";
         _serialNo = -1;
      }

      /// <summary>
      /// 用选中的记录初始化属性字段
      /// </summary>
      /// <param name="sourceRow"></param>
      public void InitValueFields(DataRow sourceRow)
      {
         ClearValueFields();

         if (sourceRow != null)
         { 
            DataColumnCollection cols = sourceRow.Table.Columns;
            if (cols.Contains("xh"))
               _serialNo = (int)sourceRow["xh"];
            if (cols.Contains("name"))
               _name = sourceRow["name"].ToString();
            if (cols.Contains("py"))
               _pingYin = sourceRow["py"].ToString();
            if (cols.Contains("wb"))
               _fiveStrokes = sourceRow["wb"].ToString();
         }
      }

      /// <summary>
      /// 用字典实例创建 类型持久对象
      /// </summary>
      /// <returns></returns>
      protected override EPBaseObject CreatePersistentWordbook()
      {
         throw new NotImplementedException();
      }
}
}
