using System;
using System.Collections.Generic;
using System.Text;
using DrectSoft.Common.Eop;
using System.Data;

namespace DrectSoft.Common.Eop
{
   /// <summary>
   /// 地区代码--区县
   /// </summary>
   public class ZoneCountry : EPBaseObject
   {
      #region properties
      /// <summary>
      /// 所属地区
      /// </summary>
      public ZoneProvince ParentZone
      {
         get { return _parentZone; }
         set { _parentZone = value; }
      }
      private ZoneProvince _parentZone;

      /// <summary>
      /// 级别
      /// </summary>
      public ZoneGrade Grade
      {
         get { return _grade; }
         set { _grade = value; }
      }
      private ZoneGrade _grade;

      /// <summary>
      /// 与实体类匹配的、读取DB中数据的SQL语句
      /// </summary>
      public override string InitializeSentence
      {
         get { return GetQuerySentenceFromXml("SelectZone"); }
      }

      /// <summary>
      /// 在DataTable中按主键值过滤记录的条件
      /// </summary>
      public override string FilterCondition
      {
         get
         {
             return String.Format("{0} and Category = {1}", FormatFilterString("AreaID", Code), (int)ZoneGrade.Country); 
         }
      }
      #endregion

      #region ctors
      /// <summary>
      /// 
      /// </summary>
      public ZoneCountry()
         : base()
      { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="code"></param>
      public ZoneCountry(string code)
         : base(code)
      { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="code"></param>
      /// <param name="name"></param>
      public ZoneCountry(string code, string name)
         : base(code, name)
      { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sourceRow"></param>
      public ZoneCountry(DataRow sourceRow)
         : base(sourceRow)
      { }
      #endregion

      #region public methods

      /// <summary>
      /// 初始化所有的属性，包括引用类型的属性自己的属性
      /// </summary>
      public override void ReInitializeAllProperties()
      {
         ReInitializeProperties();
      }
      #endregion
   }
}
