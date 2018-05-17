using System.Xml.Serialization;
using System.ComponentModel;
using System.Diagnostics;
using System;

namespace DrectSoft.Core
{
   /// <summary>
   /// 帐户权限管理配置
   /// </summary>
   /// <remarks>设置医生权限的管理模式等（不同于模块的使用权限设置）</remarks>
   [SerializableAttribute()]
   [DebuggerStepThroughAttribute()]
   [DesignerCategoryAttribute("code")]
   [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.DrectSoft.com/")]
   [XmlRootAttribute(Namespace = "http://www.DrectSoft.com/", IsNullable = false)]
   public class AccountManager
   {
      /// <summary>
      /// 医生权限管理级别
      /// </summary>
      public AccountPermissionLevel PermissionLevel
      {
         get { return _permissionLevel; }
         set { _permissionLevel = value; }
      }
      private AccountPermissionLevel _permissionLevel;

      /// <summary>
      /// 同一病区、不同科室的病人合并显示
      /// </summary>
      public bool MergeSameWard
      {
         get { return _mergeSameWard; }
         set { _mergeSameWard = value; }
      }
      private bool _mergeSameWard;
   }

   /// <summary>
   /// 帐号管理安全级别
   /// </summary>
   [SerializableAttribute()]
   [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.DrectSoft.com/")]
   [TypeConverter(typeof(LocalizedEnumConverter))]
   public enum AccountPermissionLevel
   {
      /// <summary>
      /// 按职工科室对应表的设置管理
      /// </summary>
      UserDeptMapping = 1,
      /// <summary>
      /// 按职工表中的科室管理
      /// </summary>
      UserDept = 2,
      /// <summary>
      /// 按二级科室管理
      /// </summary>
      DeptClassTwo = 3,
      /// <summary>
      /// 按一级科室管理
      /// </summary>
      DeptClassOne = 4
   }


}