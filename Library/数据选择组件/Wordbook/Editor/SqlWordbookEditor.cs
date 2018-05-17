using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DrectSoft.Wordbook
{
   /// <summary>
   /// SqlWordbook属性的编辑器
   /// </summary>
   [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode), PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust"), PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
   public class SqlWordbookEditor : System.Drawing.Design.UITypeEditor
   {
      /// <summary>
      /// 创建Sql字典编辑器
      /// </summary>
      public SqlWordbookEditor()
      { }

      /// <summary>
      /// 获取由 EditValue 方法使用的编辑器样式。
      /// </summary>
      /// <param name="context"></param>
      /// <returns></returns>
      public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
      {
         if (context != null)
         {
            return UITypeEditorEditStyle.Modal;
         }
         return base.GetEditStyle(context);
      }

      /// <summary>
      /// 使用 GetEditStyle 指示的编辑器样式编辑指定的对象值。
      /// </summary>
      /// <param name="context"></param>
      /// <param name="provider"></param>
      /// <param name="value"></param>
      /// <returns></returns>
      public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
      {
         if ((context != null) && (provider != null))
         {
            // Access the property browser's UI display service, IWindowsFormsEditorService
            IWindowsFormsEditorService editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (editorService != null)
            {
               // 创建 UI editor 的实例
               FormSqlWordbook modalEditor = new FormSqlWordbook();

               // 传入当前属性的值
               modalEditor.Wordbook = (SqlWordbook)value;

               if (editorService.ShowDialog(modalEditor) == DialogResult.OK)
               {
                  // 返回属性的新值
                  return modalEditor.Wordbook;
               }
            }
         }
         return base.EditValue(context, provider, value);
      }
   }
}
