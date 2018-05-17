using System;
using System.Collections.Generic;
using System.Text;
using AspectSharp.Builder;
using AspectSharp;
using AopAlliance.Intercept;
using System.Diagnostics;

namespace DrectSoft.FrameWork.WinForm
{
    /// <summary>
    /// 
    /// </summary>
   public class PlugInDynamicProxy
   {
      private AspectLanguageEngineBuilder builder;
      private AspectEngine engine;

      private PlugInDynamicProxy()
      {
         

         builder = new AspectLanguageEngineBuilder(string.Empty);
         engine = builder.Build();
      }

      public static object CreateDynamicProxyClass(Type classType)
      {
         PlugInDynamicProxy dynamicProxyFac = new PlugInDynamicProxy();
         return dynamicProxyFac.engine.WrapClass(classType);
      }
   }

   public class LoggerInterceptor : IMethodInterceptor
   {
      private string _loggerHeader = string.Empty;

      public LoggerInterceptor()
      {
         _loggerHeader = "Interceptor Logger Header:";
      }

      static bool HasMethodPermission(string assemblyName, string className, string functionName)
      {
         return true;
         //return FormMain.HasPermission(assemblyName, className, functionName);
      }

      static void ProcessException(Exception ex)
      {
         Trace.TraceError(ex.ToString());
         //FormMain.AppMessageShow(ex.Message, CustomMessageBoxKind.ErrorOk);
      }

      #region IMethodInterceptor Members

      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      [CLSCompliant(false)]
      public object Invoke(IMethodInvocation invocation)
      {
         if (invocation == null) throw new ArgumentNullException("invocation", "未传入调用方法");
         string methodName;
         methodName = invocation.Method.Name;
         System.Reflection.ParameterInfo[] param = invocation.Method.GetParameters();
         for (int i = 0; i < param.Length; i++)
         {
            if (i == 0)
               methodName = methodName + "(";
            else
               methodName = methodName + ",";
            methodName = methodName + param[i].ParameterType.ToString();
            if (i == param.Length - 1)
               methodName = methodName + ")";
         }
         methodName = "M:" + invocation.This.GetType().BaseType.ToString() + "." + methodName;
         Trace.WriteLine(_loggerHeader + "\r\n"
             + invocation.This.GetType().ToString() + "\r\n"
             + "enter " + methodName);

         string assemblyName = invocation.This.GetType().BaseType.Module.Name;
         string className = invocation.This.GetType().BaseType.ToString();
         if (!HasMethodPermission(assemblyName, className, methodName))
         {
            Trace.WriteLine("没有调用权限");
            ProcessException(new Exception("没有调用权限"));
            return null;
         }

         try
         {
            return invocation.Proceed();
         }
         catch (Exception ex)
         {
            ProcessException(ex);
            return null;
         }
      }

      #endregion
   }
}
