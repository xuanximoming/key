using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;

namespace DrectSoft.Common.Eop
{
   internal static class MessageStringManager
   {
      private static ResourceManager StringManager
      {
         get
         {
            if (_stringManager == null)
            {
                _stringManager = new ResourceManager("DrectSoft.Core.Properties.Resources", Assembly.GetExecutingAssembly()); 
            }

            return _stringManager;
         }
      }
      private static ResourceManager _stringManager;

      public static string GetString(string name)
      {
         if (String.IsNullOrEmpty(name))
            throw new ArgumentNullException("name");
         return StringManager.GetString(name, CultureInfo.CurrentCulture);
      }

      public static string GetString(string name, object para)
      {
         if (String.IsNullOrEmpty(name))
            throw new ArgumentNullException("name");
         return String.Format(CultureInfo.CurrentCulture, GetString(name), para);
      }
   }
}
