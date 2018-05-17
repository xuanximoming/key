using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Diagnostics;
using System.Xml.Serialization;

namespace DrectSoft.Wordbook
{
   /// <summary>
   /// 过滤参数类定义
   /// </summary>
   [SerializableAttribute()]
   [DebuggerStepThroughAttribute()]
   [XmlTypeAttribute(AnonymousType = true)]
   [CLSCompliantAttribute(true)]
   public class FilterParameter
   {
      #region properties
      /// <summary>
      /// 参数对应的数据集字段名称
      /// </summary>
      public string FieldName
      {
         get { return _fieldName; }
         set { _fieldName = value; }
      }
      private string _fieldName;

      /// <summary>
      /// 参数名称。如果允许用户在运行时设置参数值，名称应取中文名称
      /// </summary>
      public string Caption
      {
         get { return _caption; }
         set { _caption = value; }
      }
      private string _caption;

      /// <summary>
      /// 标记参数是否是字符型
      /// </summary>
      public bool IsString
      {
         get { return _isString; }
         set { _isString = value; }
      }
      private bool _isString;

      /// <summary>
      /// 对参数的描述
      /// </summary>
      public string Description
      {
         get { return _description; }
         set { _description = value; }
      }
      private string _description;

      /// <summary>
      /// 此参数的取值来源于指定的数据类别（允许用户设置参数值的时候需要此信息来提供辅助功能）
      /// </summary>
      public int DataCatalog
      {
         get { return _dataCatalog; }
         set { _dataCatalog = value; }
      }
      private int _dataCatalog;

      /// <summary>
      /// 允许普通用户编辑参数值
      /// </summary>
      public bool AllowUserEdit
      {
         get { return _allowUserEdit; }
         set { _allowUserEdit = value; }
      }
      private bool _allowUserEdit;

      /// <summary>
      /// 参数是否有效
      /// </summary>
      public bool Enabled
      {
         get { return _enabled; }
         set { _enabled = value; }
      }
      private bool _enabled;

      /// <summary>
      /// 运算符
      /// </summary>
      [XmlElementAttribute("OperatorName")]
      public CompareOperator Operator
      {
         get { return _operator; }
         set { _operator = value; }
      }
      private CompareOperator _operator;
            
      /// <summary>
      /// 参数值。在设置参数值时最好能保证格式和逻辑都正确。
      /// 现支持以范围形式设置条件(如: "id1"或"id1,id2,id3"或"id1,id2～id3,id4%",
      /// 其中使用半角的","和"%"、全角的"～"，"%"只对字符型参数有效)，运算符必须是"In"
      /// </summary>
      [XmlElementAttribute("DefaultValue", typeof(string))]
      public object Value
      {
         get { return this._value; }
         set 
         {
            string temp;
            if (value == null)
               temp = "";
            else
               temp = value.ToString();
            // 先对输入的值进行检查
            CheckValue(temp);

            if ((!IsString)
               && (CommonOperation.GetStringType(temp.ToString()) == StringType.Empty))
               _value = "-1"; // 非字符型的参数值不能为空，默认为-1
            else
               _value = value;
            //// 对参数值进行简单的检查和替换
            //if (IsString)
            //{
            //   if (temp[0] != '\'')
            //      _value = "'" + value;
            //   else 
            //      _value = value;

            //   if (!temp.EndsWith("'"))
            //      _value = _value + "'";
            //}
            //else
            //{
            //   if (CommonOperation.GetStringType(temp) == StringType.Empty)
            //      temp = "-1";

            //   // 如果使用In运算符，则允许出现","，否则都产生异常
            //   if ((CommonOperation.GetStringType(temp) != StringType.Numeric)
            //      && ((OperatorName != CompareOperator.In)
            //      || (CommonOperation.GetStringType(temp.Replace(',', (char)0)) == StringType.Numeric)))
            //      throw new ArithmeticException("将字符串赋值给数值型参数");
            //   _value = temp; 
            //}
         }
      }
      private object _value;

      /// <summary>
      /// 用在条件表达式中的值（如果是字符型的参数，将对参数值加上单引号）
      /// </summary>
      [XmlIgnoreAttribute()]
      public string ParameterValue
      {
         get
         {
            if (!IsString)
               return Value.ToString(); // 数值型不需要处理

            if (Operator != CompareOperator.In)
               return "'" + Value.ToString().Trim() + "'";
            else
            { 
               // 先去掉空格和末尾的“,”；然后将“,”和“～”分别替换为“','”和“'～'”
               string temp = Value.ToString().Replace(" ", "");
               if (temp[temp.Length - 1] == ',')
                  temp = temp.Substring(0, temp.Length - 1);
               temp = temp.Replace(",", "','");
               temp = temp.Replace("～", "'～'");
               return "'" + temp + "'";
            }
         }
      }
      #endregion

      /// <summary>
      /// 检查传入的参数值是否正确
      /// </summary>
      /// <param name="paraValue"></param>
      private void CheckValue(object paraValue)
      {
         string temp = paraValue.ToString();
         // 先用','作为分隔符，拆分出独立的条件
         string[] separator1 = new string[] { "," };
         string[] separator2 = new string[] { "～" };
         string[] values = temp.Split(separator1, StringSplitOptions.None);
         string[] rangs;
         if ((values.Length > 1) && (Operator != CompareOperator.In))
            throw new ArgumentException(MessageStringManager.GetString("CoulNotSetParameterRange"));

         foreach (string condition in values)
         {
            // 检查范围设置是否正确
            if (condition.Contains("～"))
            {
               rangs = condition.Split(separator2, StringSplitOptions.None);
               if (rangs.Length != 2)
                  throw new ArgumentException(MessageStringManager.GetString("WrongParameterRange"));
               if (condition.Contains("%"))
                  throw new ArgumentException(MessageStringManager.GetString("CouldNotUseWildcardInRange"));
               CheckValueType(rangs[0]);
               CheckValueType(rangs[1]);
            }
            else // 检查单个的条件是否满足要求
               CheckValueType(condition);
         }
      }

      /// <summary>
      /// 检查传入的参数值是否符合参数类型的要求
      /// （主要是防止将字符型数据赋给数值型参数）
      /// </summary>
      /// <param name="paraValue"></param>
      private void CheckValueType(string paraValue)
      {
         if ((!IsString)
            && (CommonOperation.GetStringType(paraValue) != StringType.Numeric)
            && (CommonOperation.GetStringType(paraValue) != StringType.Empty))
            throw new ArithmeticException(MessageStringManager.GetString("CouldNotSetStringToDigital"));
      }

      #region ctors
      /// <summary>
      /// 
      /// </summary>
      public FilterParameter()
      { }

      /// <summary>
      /// 创建过滤参数
      /// </summary>
      /// <param name="fieldName">参数对应的数据集字段名</param>
      /// <param name="name">参数的名称</param>
      /// <param name="isString">标记参数是否是字符型</param>
      /// <param name="description">参数的描述</param>
      /// <param name="allowUserEdit">允许普通用户编辑参数值</param>
      /// <param name="sign">运算符</param>
      public FilterParameter(string fieldName, string name, bool isString
         , string description, bool allowUserEdit, CompareOperator sign )
      {
         if (String.IsNullOrEmpty(fieldName))
            throw new ArgumentNullException(MessageStringManager.GetString("NullFieldName"));
         if (String.IsNullOrEmpty(name))
            throw new ArgumentNullException(MessageStringManager.GetString("NullParameterName"));
         if ((!isString) && (sign == CompareOperator.Like))
            throw new ArgumentException(MessageStringManager.GetString("UseLikeOperatprOnDigitalParameter"));

         _fieldName = fieldName;
         _caption = name;
         _isString = isString;
         _description = description;
         _allowUserEdit = allowUserEdit;
         _operator = sign;
         _dataCatalog = -1;

         //_enable = false;
      }

      /// <summary>
      /// 创建过滤参数，并设置初始值
      /// </summary>
      /// <param name="fieldName">参数对应的数据集字段名</param>
      /// <param name="name">参数的名称</param>
      /// <param name="isString">标记参数是否是字符型</param>
      /// <param name="description">参数的描述</param>
      /// <param name="allowUserEdit">允许普通用户编辑参数值</param>
      /// <param name="sign">运算符</param>
      /// <param name="value">初始值</param>
      public FilterParameter(string fieldName, string name, bool isString
         , string description, bool allowUserEdit, CompareOperator sign, object value) 
         : this(fieldName, name, isString, description, allowUserEdit, sign)
      {
         Value = value;
      }

      /// <summary>
      /// 创建取值来源于指定的数据类别的过滤参数。
      /// 此类参数默认为非字符串型，并允许普通用户编辑值
      /// </summary>
      /// <param name="fieldName">参数对应的数据集字段名</param>
      /// <param name="name">参数的名称</param>
      /// <param name="dataSort">指定的数据类别来源</param>
      /// <param name="description">参数的描述</param>
      /// <param name="sign">运算符</param>
      public FilterParameter(string fieldName, string name, int dataSort
         , string description, CompareOperator sign)
         : this(fieldName, name, false, description, true, sign)
      {
         _dataCatalog = dataSort;
      }

      /// <summary>
      /// 创建取值来源于指定的数据类别的过滤参数，并设置初值。
      /// 此类参数默认为非字符串型，并允许普通用户编辑值
      /// </summary>
      /// <param name="fieldName">参数对应的数据集字段名</param>
      /// <param name="name">参数的名称</param>
      /// <param name="dataSort">指定的数据类别来源</param>
      /// <param name="description">参数的描述</param>
      /// <param name="sign">运算符</param>
      /// <param name="value">初始值</param>
      public FilterParameter(string fieldName, string name, int dataSort
         , string description, CompareOperator sign, object value)
         : this(fieldName, name, dataSort, description, sign)
      {
         Value = value;
      }
      #endregion

      /// <summary>
      /// 重载"=="
      /// </summary>
      /// <param name="para1"></param>
      /// <param name="para2"></param>
      /// <returns></returns>
      public static bool operator ==(FilterParameter para1, FilterParameter para2)
      {
         // If one is null, but not both, return false.
         if ((object)para1 == null)
            return false;
         if ((object)para2 == null)
            return false;
         // If both are null, or both are same instance, return true.
         if (Object.ReferenceEquals(para1, para2))
            return true;
         // Otherwise, compare values and return:
         else
         {
            return (para1.FieldName == para2.FieldName)
            && (para1.Caption == para2.Caption)
            && (para1.IsString == para2.IsString)
            && (para1.Operator == para2.Operator);
         }
      }

      /// <summary>
      /// 重载"!="
      /// </summary>
      /// <param name="para1"></param>
      /// <param name="para2"></param>
      /// <returns></returns>
      public static bool operator !=(FilterParameter para1, FilterParameter para2)
      {
         return !(para1 == para2);
      }
      
      /// <summary>
      /// 确定两个FilterParameter对象是否相同
      /// </summary>
      /// <param name="obj"></param>
      /// <returns></returns>
      public override bool Equals(Object obj)
      {
         // If parameter is null, or cannot be cast to FilterParameter,
         // return false.
         if (obj == null) 
            return false;
         FilterParameter p = obj as FilterParameter;
         if ((object)p == null) 
            return false;
         // Return true if the fields match:
         return (FieldName == p.FieldName)
            && (Caption == p.Caption)
            && (IsString == p.IsString)
            && (Operator == p.Operator);
      }

      /// <summary>
      /// 返回该实例的哈希代码
      /// </summary>
      /// <returns></returns>
      public override int GetHashCode()
      {
         return FieldName.GetHashCode() 
            ^ Caption.GetHashCode() 
            ^ IsString.GetHashCode() 
            ^ Operator.GetHashCode();
      }

      /// <summary>
      /// Clone
      /// </summary>
      /// <returns></returns>
      public FilterParameter Clone()
      {
         FilterParameter cln = new FilterParameter();
         cln.FieldName = this.FieldName;
         cln.Caption = this.Caption;
         cln.AllowUserEdit = this.AllowUserEdit;
         cln.DataCatalog = this.DataCatalog;
         cln.Description = this.Description;
         cln.Enabled = this.Enabled;
         cln.Operator = this.Operator;
         //cln.ParameterValue = this.ParameterValue;
         cln.Value = this.Value;
         cln.IsString = this.IsString;

         return cln;
      }

   }
}
