using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;

namespace DrectSoft.Core.EMR_NursingDocument.PublicMethod
{

    /// <summary> 
    /// 实体转换辅助类 
    /// </summary> 
    public class DataTableToList<T> where T : new()
    {
        public static List<T> ConvertToModel(DataTable dt)
        {
            // 定义集合 
            List<T> ts = new List<T>();
            // 获得此模型的类型 
            Type type = typeof(T);
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                // 获得此模型的公共属性 
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;
                    // 检查DataTable是否包含此列 
                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter 
                        if (!pi.CanWrite) continue;
                        object value = dr[tempName];
                        if (value != DBNull.Value)
                            pi.SetValue(t, value, null);
                    }
                }
                ts.Add(t);
            }
            return ts;
        }

    }
}
