using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;

namespace DrectSoft.Common
{
    /// <summary> 
    /// 实体转换辅助类 
    /// </summary> 
    public class DataTableToList<T> where T : new()
    {
        /// <summary>
        /// 数据集转换成对象集合
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <returns>集合</returns>
        public static List<T> ConvertToModel(DataTable dt)
        {
            try
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
                            if (!pi.CanWrite)
                            {
                                continue;
                            }
                            object value = dr[tempName];
                            if (value != DBNull.Value)
                            {
                                pi.SetValue(t, value, null);
                            }
                        }
                    }
                    ts.Add(t);
                }
                return ts;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 数据集行转换成对象
        /// </summary>
        /// <param name="dt">表</param>
        /// <returns></returns>
        public static T ConvertToModelOne(DataTable dt)
        {
            try
            {
                // 定义对象 
                T tone = new T();
                if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
                {
                    return tone;
                }
                // 获得此模型的类型 
                Type type = typeof(T);
                string tempName = "";

                // 获得此模型的公共属性 
                PropertyInfo[] propertys = tone.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;
                    // 检查DataTable是否包含此列 
                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter 
                        if (!pi.CanWrite)
                        {
                            continue;
                        }
                        object value = dt.Rows[0][tempName];
                        if (value != DBNull.Value)
                        {
                            pi.SetValue(tone, value, null);
                        }
                    }
                }
                return tone;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
