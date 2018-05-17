using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.Core
{
   /// <summary>
   /// 权限模块功能列表接口
   /// </summary>
   public interface IRBSFunction
   {
      /// <summary>
      /// 功能Id
      /// </summary>
      string FunctionId { get;}

      /// <summary>
      /// 功能名称
      /// </summary>
      string FunctionName { get;}

      /// <summary>
      /// 功能描述
      /// </summary>
      string FunctionDescription { get;}
   }

   /// <summary>
   /// 权限模块列表接口
   /// </summary>
   public interface IRBSModule
   {
      /// <summary>
      /// 模块Id
      /// </summary>
      string ModuleId { get;}

      /// <summary>
      /// 模块名称
      /// </summary>
      string ModuleName { get;}

      /// <summary>
      /// 模块描述
      /// </summary>
      string ModuleDescription { get;}

      
      /// <summary>
      /// 模块功能列表
      /// </summary>
      Collection<IRBSFunction> FunctionList { get;}

      /// <summary>
      /// 增加一个授权功能
      /// </summary>
      /// <param name="function"></param>
      void AddAFunction(IRBSFunction function);

      /// <summary>
      /// 增加一个授权功能并返回索引
      /// </summary>
      /// <param name="functionId"></param>
      /// <param name="functionName"></param>
      /// <param name="functionDescription"></param>
      /// <returns></returns>
      int AddAFunction(string functionId, string functionName, string functionDescription);
   }

   /// <summary>
   /// 访问权限接口
   /// </summary>
   public interface IPermission
   {
      /// <summary>
      /// 角色Id
      /// </summary>
      string RoleId { get; }

      /// <summary>
      /// 角色名称
      /// </summary>
      string RoleName { get; set; }
      
      
      /// <summary>
      /// 模块列表
      /// </summary>
      Collection<IRBSModule> ModuleList { get;}

      /// <summary>
      /// 增加模块权限
      /// </summary>
      /// <param name="module"></param>
      void AddAModule(IRBSModule module);

      /// <summary>
      /// 增加新的模块权限
      /// </summary>
      /// <param name="moduleId"></param>
      /// <param name="moduleName"></param>
      /// <param name="moduleDescription"></param>
      /// <param name="system"></param>
      /// <returns></returns>
      int AddAModule(string moduleId, string moduleName, string moduleDescription);

      /// <summary>
      /// 角色Id改变事件
      /// </summary>
      event EventHandler<RoleIdChangedEventArgs> RoleIdChanged;

      /// <summary>
      /// 角色Name改变事件
      /// </summary>
      event EventHandler<RoleNameChangedEventArgs> RoleNameChanged;


      /// <summary>
      /// 判断一个岗位是否为管理员权限,目前默认00
      /// </summary>
      bool IsAdministrators { get;}
   }

   /// <summary>
   /// RoleIdChanged,角色Id改变事件参数类
   /// </summary>
   public class RoleIdChangedEventArgs : EventArgs
   {
      private string _oldId;
      private string _newId;

      /// <summary>
      /// 原Id
      /// </summary>
      public string OldId
      {
         get { return _oldId; }
         set { _oldId = value; }
      }

      /// <summary>
      /// 新Id
      /// </summary>
      public string NewId
      {
         get { return _newId; }
         set { _newId = value; }
      }

      /// <summary>
      /// ctor,传入新老Id
      /// </summary>
      /// <param name="oldId"></param>
      /// <param name="newId"></param>
      public RoleIdChangedEventArgs(string oldId, string newId)
      {
         _oldId = oldId;
         _newId = newId;
      }
   }

   /// <summary>
   /// RoleNameChanged,角色Name改变事件参数类
   /// </summary>
   public class RoleNameChangedEventArgs : EventArgs
   {
      private string _oldName;
      private string _newName;

      /// <summary>
      /// 原Name
      /// </summary>
      public string OldName
      {
         get { return _oldName; }
      }

      /// <summary>
      /// 新Name
      /// </summary>
      public string NewName
      {
         get { return _newName; }
      }

      /// <summary>
      /// ctor,传入新老Name
      /// </summary>
      /// <param name="oldName"></param>
      /// <param name="newName"></param>
      public RoleNameChangedEventArgs(string oldName, string newName)
      {
         _oldName = oldName;
         _newName = newName;
      }
   }  
   
}

