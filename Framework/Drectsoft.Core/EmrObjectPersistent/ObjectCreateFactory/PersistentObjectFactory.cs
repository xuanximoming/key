using DrectSoft.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Xml.Serialization;

namespace DrectSoft.Common.Eop
{
    /// <summary>
    /// 创建持久类工厂
    /// </summary>
    public static class PersistentObjectFactory
    {
        #region static & const string
        private const string EOPNameSpace = "DrectSoft.Common.Eop.";
        #endregion

        #region properties
        private static ORMCollection OrmSettings
        {
            get
            {
                if (_ormSettings == null)
                    ResetORMSettings();

                return _ormSettings;
            }
        }
        private static ORMCollection _ormSettings;

        /// <summary>
        /// 预定义的Sql语句
        /// </summary>
        public static PreDefineSqlCollection SqlSentences
        {
            get
            {
                if (_sqlSentences == null)
                    GetPreDefineSqlSentence();

                return _sqlSentences;
            }
        }
        private static PreDefineSqlCollection _sqlSentences;

        /// <summary>
        /// 数据访问对象
        /// </summary>
        public static IDataAccess SqlExecutor
        {
            get
            {
                if (_sqlExecutor == null)
                    _sqlExecutor = DataAccessFactory.DefaultDataAccess;
                return _sqlExecutor;
            }
            set { _sqlExecutor = value; }
        }
        private static IDataAccess _sqlExecutor;

        #endregion

        #region private variables
        private static Dictionary<string, PropertyInfo[]> m_PropertyCache = new Dictionary<string, PropertyInfo[]>();
        private static Dictionary<string, ConstructorInfo[]> m_ConstructorCache = new Dictionary<string, ConstructorInfo[]>();
        private static Dictionary<string, MethodInfo[]> m_MethodCache = new Dictionary<string, MethodInfo[]>();
        #endregion

        #region private common methods
        private static PropertyInfo[] GetProperties(Type objType)
        {
            if (!m_PropertyCache.ContainsKey(objType.FullName))
                m_PropertyCache.Add(objType.FullName
                   , objType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));

            return m_PropertyCache[objType.FullName];
        }

        private static ConstructorInfo[] GetConstructors(Type objType)
        {
            if (!m_ConstructorCache.ContainsKey(objType.FullName))
                m_ConstructorCache.Add(objType.FullName, objType.GetConstructors());

            return m_ConstructorCache[objType.FullName];
        }

        private static MethodInfo[] GetMethods(Type objType)
        {
            if (!m_MethodCache.ContainsKey(objType.FullName))
                m_MethodCache.Add(objType.FullName
                   , objType.GetMethods(BindingFlags.Public | BindingFlags.Instance));
            return m_MethodCache[objType.FullName];
        }

        private static MethodInfo GetMethod(Type objType, string methodName)
        {
            MethodInfo[] methods = GetMethods(objType);
            if ((methods != null) && (!String.IsNullOrEmpty(methodName)))
            {
                foreach (MethodInfo info in methods)
                    if (info.Name == methodName)
                        return info;
            }
            return null;
        }

        private static void MergeParentAndChildSetting()
        {
            if (_ormSettings == null)
                return;

            // 循环处理Normal类的设置
            foreach (ORMapping orm in _ormSettings.ORMappings)
            {
                if (!String.IsNullOrEmpty(orm.ParentClass))
                {
                    DoMergeParentAndChildSetting(orm);
                }
            }
        }

        private static void DoMergeParentAndChildSetting(ORMapping child)
        {
            // 找到其父类
            // 如果父类的属性在子类中没有，则加入到子类中
            ORMapping parent;
            parent = FindParentORMapping(child.ParentClass);
            if (parent == null)
                throw new ArgumentOutOfRangeException("ORM设置不正确(缺少"
                   + child.ClassName + "的父类定义)");

            // 做一个子类属性名的List，用来判断属性定义是否存在
            Collection<string> properties = new Collection<string>();

            if (parent.OneOnes != null)
                DoMergeOneOnes(child, parent, properties);
            if (parent.States != null)
                DoMergeSates(child, parent, properties);
            if (parent.Structures != null)
                DoMergeStructures(child, parent, properties);
            if (parent.ObjectClasses != null)
                DoMergeObjectClasses(child, parent, properties);
            if (parent.SubClasses != null)
                DoMergeSubClasses(child, parent, properties);
            if (parent.DefaultValues != null)
                DoMergeDefaultValues(child, parent);
        }
        /// <summary>
        /// 二次修改加捕获及验证完善
        /// add by ywk 2013年3月13日9:42:04 
        /// </summary>
        /// <param name="child"></param>
        /// <param name="parent"></param>
        /// <param name="properties"></param>
        private static void DoMergeOneOnes(ORMapping child, ORMapping parent, Collection<string> properties)
        {
            try
            {
                if (child.OneOnes == null)
                {
                    child.OneOnes = new Collection<OneToOneType>();
                }
                else
                {
                    //edit by ywk 2013年3月13日9:41:37 
                    //foreach (OneToOneType one in child.OneOnes)
                    //{
                    //    if (!properties.Contains(one.Property))
                    //    {
                    //        properties.Add(one.Property);
                    //    }
                    //}
                    //改为for循环 ywk 
                    for (int i = 0; i < child.OneOnes.Count; i++)
                    {
                        if (!properties.Contains(child.OneOnes[i].Property))
                        {
                            properties.Add(child.OneOnes[i].Property);
                        }
                    }
                }

                //foreach (OneToOneType one in parent.OneOnes)
                //{
                //    //先判断properties长度
                //    if (properties.Count >= 0 && !properties.Contains(one.Property))
                //        child.OneOnes.Add(one.Clone());
                //}
                //改为for 循环 edit by ywk 2013年3月13日10:19:37 
                for (int i = 0; i < parent.OneOnes.Count; i++)
                {
                    if (!properties.Contains(parent.OneOnes[i].Property))
                    {
                        child.OneOnes.Add(parent.OneOnes[i].Clone());
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("DoMergeOneOnes方法出错：" + ex.Message);
                //throw;
            }

        }

        private static void DoMergeSates(ORMapping child, ORMapping parent, Collection<string> properties)
        {
            if (child.States == null)
                child.States = new Collection<OneToStateType>();
            else
                foreach (OneToStateType state in child.States)
                    properties.Add(state.Property);

            foreach (OneToStateType state in parent.States)
            {
                if (!properties.Contains(state.Property))
                    child.States.Add(state.Clone());
            }
        }

        private static void DoMergeStructures(ORMapping child, ORMapping parent, Collection<string> properties)
        {
            try
            {
                if (child.Structures == null)
                    child.Structures = new Collection<ManyToStructureType>();
                else
                    foreach (ManyToStructureType structure in child.Structures)
                        properties.Add(structure.Property);

                foreach (ManyToStructureType structure in parent.Structures)
                {
                    if (!properties.Contains(structure.Property))
                        child.Structures.Add(structure.Clone());
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("DoMergeStructures方法出错：" + ex.Message);
                //throw;
            }

        }

        private static void DoMergeObjectClasses(ORMapping child, ORMapping parent, Collection<string> properties)
        {
            if (child.ObjectClasses == null)
                child.ObjectClasses = new Collection<ManyToObjectClassType>();
            else
                foreach (ManyToObjectClassType objClass in child.ObjectClasses)
                    properties.Add(objClass.Property);

            foreach (ManyToObjectClassType objClass in parent.ObjectClasses)
            {
                if (!properties.Contains(objClass.Property))
                    child.ObjectClasses.Add(objClass.Clone());
            }
        }

        private static void DoMergeSubClasses(ORMapping child, ORMapping parent, Collection<string> properties)
        {
            if (child.SubClasses == null)
                child.SubClasses = new Collection<ManyToSubClassType>();
            else
                foreach (ManyToSubClassType subObj in child.SubClasses)
                    properties.Add(subObj.Property);

            foreach (ManyToSubClassType subObj in parent.SubClasses)
            {
                if (!properties.Contains(subObj.Property))
                    child.SubClasses.Add(subObj.Clone());
            }
        }

        private static void DoMergeDefaultValues(ORMapping child, ORMapping parent)
        {
            Collection<string> defValues = new Collection<string>();
            if (child.DefaultValues == null)
                child.DefaultValues = new Collection<DefaultValueType>();
            else
                foreach (DefaultValueType defValue in child.DefaultValues)
                    defValues.Add(defValue.Column);

            foreach (DefaultValueType defValue in parent.DefaultValues)
            {
                if (!defValues.Contains(defValue.Column))
                    child.DefaultValues.Add(defValue.Clone());
            }
        }

        private static ORMapping FindParentORMapping(string className)
        {
            if ((_ormSettings != null) && (_ormSettings.ParentORMappings != null))
            {
                foreach (ORMapping temp in _ormSettings.ParentORMappings)
                {
                    if (temp.ClassName == className)
                        return temp;
                }
            }
            return null;
        }

        private static ORMapping FindORMapping(string className)
        {
            if (OrmSettings.ORMappings != null)
            {
                foreach (ORMapping temp in OrmSettings.ORMappings)
                {
                    if (temp.ClassName == className)
                        return temp;
                }
            }
            return null;
        }

        private static int LocateProperty(PropertyInfo[] properties, string propName)
        {
            if (properties == null)
                return -1;
            if (String.IsNullOrEmpty(propName))
                throw new ArgumentNullException();

            for (int index = 0; index < properties.Length; index++)
                if (properties[index].Name == propName)
                    return index;
            return -1;
        }

        /// <summary>
        /// 在对应表中根据原始列名获得映射的列名或缺省值
        /// </summary>
        /// <param name="originalCol"></param>
        /// <param name="colMaps"></param>
        /// <returns>{列名, 缺省值}，两个 Cell 不会同时非空</returns>
        private static string[] GetTrueColumnAndDefaultValue(string originalCol, Dictionary<string, ColumnToColumn> colMaps)
        {
            if ((colMaps == null) || (colMaps.Count == 0))
                return new string[] { originalCol, "" };
            else
            {
                if ((originalCol != null) && colMaps.ContainsKey(originalCol))
                    return new string[] { colMaps[originalCol].SourceColumn, colMaps[originalCol].DefaultValue };
                else
                    return new string[] { "", "" };
            }
        }

        /// <summary>
        /// 在对应表中根据原始列名获得映射的列名
        /// </summary>
        /// <param name="originalCol"></param>
        /// <param name="colMaps"></param>
        /// <returns></returns>
        private static string GetTrueColumn(string originalCol, Dictionary<string, string> colMaps)
        {
            if ((colMaps == null) || (colMaps.Count == 0))
                return originalCol;
            else
            {
                if ((originalCol != null) && colMaps.ContainsKey(originalCol))
                    return colMaps[originalCol];
                else
                    return "";
            }
        }

        private static object GetTargetColumnValue(DataRow sourceRow, string targetColumn, Dictionary<string, ColumnToColumn> colMaps)
        {
            string[] trueCol = GetTrueColumnAndDefaultValue(targetColumn, colMaps);

            if (String.IsNullOrEmpty(trueCol[0])) // 在已匹配的列中未找到当前列的对应列
            {
                if (!String.IsNullOrEmpty(trueCol[1])) // 有缺省值，则返回缺省值
                    return trueCol[1];
                else
                    return null;
            }
            else if (sourceRow.Table.Columns.Contains(trueCol[0]))
                return sourceRow[trueCol[0]];
            else
                return null;
        }

        private static object GetPropertyValue(object obj, PropertyInfo[] properties, string property)
        {
            int index = LocateProperty(properties, property);
            if (index > -1)
                return properties[index].GetValue(obj, null);
            return null;
        }

        private static bool CheckSpecialAttribute(object[] attrs, MethodSpecialKind targeAttr)
        {
            if (attrs.Length > 0)
            {
                SpecialMethodAttribute specAttr;
                foreach (object attr in attrs)
                {
                    specAttr = attr as SpecialMethodAttribute;
                    if ((attr != null) && (specAttr.Flag == targeAttr))
                        return true;
                }
            }
            return false;
        }

        private static string CastClassNameFromFullName(string typeFullName)
        {
            int lastIndex = typeFullName.LastIndexOf(Type.Delimiter);
            return typeFullName.Substring(lastIndex + 1, typeFullName.Length - lastIndex - 1);
        }

        private static object ConvertObjectValueBaseOnTrueType(object objValue, Type trueType)
        {
            bool nullValue;
            if ((objValue.GetType() == typeof(DBNull))
               || (objValue == null)
               || (String.IsNullOrEmpty(objValue.ToString().Trim())))
                nullValue = true;
            else
                nullValue = false;

            if (trueType == typeof(bool))
            {
                if (nullValue)
                    return false;
                else
                    //return Convert.ToBoolean(objValue, CultureInfo.CurrentCulture); modified by wwj 2012-07-18

                    if (objValue == null)
                        return Convert.ToBoolean(objValue, CultureInfo.CurrentCulture);
                    else
                        return Convert.ToBoolean(Convert.ToInt32(objValue), CultureInfo.CurrentCulture);
            }
            if (trueType == typeof(int))
            {
                if (nullValue)
                    return -1;
                else
                    return Convert.ToInt32(objValue, CultureInfo.CurrentCulture);
            }
            if (trueType == typeof(double))
            {
                if (nullValue)
                    return -1d;
                else
                    return Convert.ToDouble(objValue, CultureInfo.CurrentCulture);
            }
            if (trueType == typeof(decimal))
            {
                if (nullValue)
                    return -1m;
                else
                    return Convert.ToDecimal(objValue, CultureInfo.CurrentCulture);
            }
            if (trueType == typeof(string))
            {
                if (nullValue)
                    return "";
                else
                    return objValue;
            }
            if (trueType == typeof(DateTime))
            {
                if (nullValue)
                    return Convert.ToDateTime(null, CultureInfo.CurrentCulture);
                else
                {
                    try
                    {
                        return Convert.ToDateTime(objValue, CultureInfo.CurrentCulture);
                    }
                    catch (Exception)
                    {
                        return null;
                    }

                }

            }
            if (trueType.BaseType == typeof(Enum))
            {
                if (nullValue)
                    return 0;
                else
                    return Convert.ToInt32(objValue, CultureInfo.CurrentCulture);//Add By zhouhui 2011-06-24 针对Enum不能被转换的问题
            }

            return objValue;
        }
        #endregion

        #region private methods
        private static void GetPreDefineSqlSentence()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PreDefineSqlCollection));

            //FileStream fileStream = new FileStream(FullSqlSentenceFileName, FileMode.Open);

            _sqlSentences = (PreDefineSqlCollection)serializer.Deserialize(BasicSettings.GetConfig(BasicSettings.PreDefineSqlSetting));

            //fileStream.Close();
        }

        private static void SetPropertyValueOfSubClass(object obj, PropertyInfo[] properties, DataRow sourceRow, ManyToSubClassType subObj)
        {
            // 使用类名或用工厂类取得实际的类名
            string className;

            if (!String.IsNullOrEmpty(subObj.KindColumn))
            {
                if (subObj.ClassName == "OrderContentFactory")
                    className = OrderContentFactory.GetOrderContentClassName(sourceRow[subObj.KindColumn]);
                else
                    throw new ArgumentException(MessageStringManager.GetString("ClassNotImplement"));
            }
            else
                className = subObj.ClassName;

            Object newObj = CreateAndIntializeObject(className, sourceRow);
            SetPropertyValue(obj, properties, newObj, subObj.Property);
        }
        /// <summary>
        /// 二次修改 TYR Catch
        /// add by ywk 2013年3月13日10:22:29 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="properties"></param>
        /// <param name="sourceRow"></param>
        /// <param name="objClass"></param>
        /// <param name="colMaps"></param>
        private static void SetPropertyValueOfLinkObject(object obj, PropertyInfo[] properties, DataRow sourceRow, ManyToObjectClassType objClass, Dictionary<string, ColumnToColumn> colMaps)
        {
            try
            {
                // 使用类名或用工厂类取得实际的类名
                string className;

                if (String.IsNullOrEmpty(objClass.KindColumn))
                    className = objClass.ClassName;
                else
                    throw new ArgumentException(MessageStringManager.GetString("ClassNotImplement"));

                object newObj = null;
                // 某些情况下在同一张表中用相同的字段保存不同情况下的数据，然后用某个标志位进行判断当前的数据表示何种情况。
                //   在映射到对象时，每种情况都可能是一种单独的类。所以在配置中会将列和不同对象进行映射的关系都给出，由标志字段的值来决定取哪一种配置。
                foreach (LinkedObject linkObj in objClass.LinkedObjects)
                {
                    if ((String.IsNullOrEmpty(className)) || (linkObj.ClassName == className))
                    {
                        // 有可能存在级联的情况(表的一组列关联到Link对象，而Link对象中的部分字段又关联到另外的对象)
                        // ，所以要得到最终的原始列和关联列的关系( a -> b, b -> c ==> a -> c)
                        Collection<ColumnToColumn> newMaps;
                        if ((colMaps != null) && (colMaps.Count > 0))
                        {
                            newMaps = new Collection<ColumnToColumn>();
                            ColumnToColumn newCol2Col;
                            foreach (ColumnToColumn linkColMap in linkObj.ColumnToColumns)
                            {
                                newCol2Col = new ColumnToColumn();
                                newCol2Col.TargetColumn = linkColMap.TargetColumn;// 第三个表的列名
                                if (colMaps.ContainsKey(linkColMap.SourceColumn))
                                {
                                    newCol2Col.SourceColumn = colMaps[linkColMap.SourceColumn].SourceColumn;
                                    newCol2Col.DefaultValue = colMaps[linkColMap.SourceColumn].DefaultValue;
                                }
                                else if (!String.IsNullOrEmpty(linkColMap.DefaultValue))
                                {
                                    // 中间断开了，则表示不能建立级联关系
                                    newCol2Col.SourceColumn = ""; // linkColMap.SourceColumn;
                                    newCol2Col.DefaultValue = linkColMap.DefaultValue;
                                }
                                else
                                    continue;
                                newMaps.Add(newCol2Col);
                            }
                        }
                        else
                            newMaps = linkObj.ColumnToColumns;

                        newObj = CreateAndIntializeObject(linkObj.ClassName, sourceRow, newMaps);
                        break;
                    }
                }
                SetPropertyValue(obj, properties, newObj, objClass.Property);
            }
            catch (Exception ex)
            {

                //MessageBox.Show("SetPropertyValueOfLinkObject方法出错" + ex.Message);
            }

        }

        private static void SetPropertyValueOfStruct(object obj, PropertyInfo[] properties, DataRow sourceRow, ManyToStructureType structure, Dictionary<string, ColumnToColumn> colMaps)
        {
            if ((structure.PropertyToColumn == null)
               || (structure.PropertyToColumn.Count == 0))
                throw new ArgumentNullException();

            // 取得构造函数的信息，创建实例
            Type objType = Type.GetType(EOPNameSpace + structure.ClassName);
            ConstructorInfo[] ctors = GetConstructors(objType);
            ParameterInfo[] ctorParameters;
            object objValue;
            if (ctors != null)
            {
                object[] attrs;
                foreach (ConstructorInfo ctor in ctors)
                {
                    attrs = ctor.GetCustomAttributes(false);
                    if (CheckSpecialAttribute(attrs, MethodSpecialKind.DefaultCtor))
                    {
                        ctorParameters = ctor.GetParameters();
                        object[] values = new object[structure.PropertyToColumn.Count];
                        int index = 0;
                        object colValue;
                        for (; index < structure.PropertyToColumn.Count; index++)
                        {
                            if (String.IsNullOrEmpty(structure.PropertyToColumn[index].Column))
                            {
                                objValue = ConvertObjectValueBaseOnTrueType(
                                   structure.PropertyToColumn[index].DefaultValue
                                   , ctorParameters[index].ParameterType);
                            }
                            else
                            {
                                colValue = GetTargetColumnValue(sourceRow, structure.PropertyToColumn[index].Column, colMaps);
                                if (colValue == null)
                                    break;
                                else
                                    objValue = ConvertObjectValueBaseOnTrueType(colValue, ctorParameters[index].ParameterType);
                            }
                            values[index] = objValue;
                        }
                        if (index == structure.PropertyToColumn.Count)
                        {
                            Object structureObj = ctor.Invoke(values);
                            SetPropertyValue(obj, properties, structureObj, structure.Property);
                        }
                        break;
                    }
                }
            }
        }

        private static void SetPropertyValueOfState(object obj, PropertyInfo[] properties, DataRow sourceRow, OneToStateType state, Dictionary<string, ColumnToColumn> colMaps)
        {
            if (String.IsNullOrEmpty(state.Column))
                return;

            object value = GetTargetColumnValue(sourceRow, state.Column, colMaps);

            if (value == null)
                return;

            // 创建实例
            Type objType = Type.GetType(EOPNameSpace + state.ClassName);
            Object stateObj = Activator.CreateInstance(objType);

            MethodInfo[] methods = GetMethods(objType);
            if (methods != null)
            {
                object[] attrs;
                foreach (MethodInfo method in methods)
                {
                    attrs = method.GetCustomAttributes(false);
                    if (CheckSpecialAttribute(attrs, MethodSpecialKind.StateInitValueMethod))
                    {
                        method.Invoke(stateObj, new object[1] { value });

                        SetPropertyValue(obj, properties, stateObj, state.Property);
                        break;
                    }
                }
            }
        }

        private static void SetPropertyValueOfNormal(object obj, PropertyInfo[] properties, DataRow sourceRow, OneToOneType one, Dictionary<string, ColumnToColumn> colMaps)
        {
            if (String.IsNullOrEmpty(one.Column))
                return;

            object value = GetTargetColumnValue(sourceRow, one.Column, colMaps);

            if (value != null)
                SetPropertyValue(obj, properties, value, one.Property);
        }

        private static void SetPropertyValue(object obj, PropertyInfo[] properties, object objValue, string propertyName)
        {
            int index = LocateProperty(properties, propertyName);
            if (index > -1)
            {
                properties[index].SetValue(obj
                   , ConvertObjectValueBaseOnTrueType(objValue, properties[index].PropertyType)
                   , null);
            }
        }

        private static void SetColumnValueFromSubClass(DataRow targetRow, object obj, PropertyInfo[] properties, ManyToSubClassType subObject)
        {
            object linkObj = GetPropertyValue(obj, properties, subObject.Property);

            if (linkObj != null)
                SetDataRowValueFromObject(targetRow, linkObj);
        }

        private static void SetColumnValueFromLinkObject(DataRow targetRow, object obj, PropertyInfo[] properties, ManyToObjectClassType objClass, Dictionary<string, string> colMaps)
        {
            object linkObj = GetPropertyValue(obj, properties, objClass.Property);
            if (linkObj == null)
                return;

            Type objType = linkObj.GetType();

            foreach (LinkedObject link in objClass.LinkedObjects)
            {
                if (link.ClassName == CastClassNameFromFullName(objType.FullName))
                {
                    Collection<ColumnToColumn> newMaps;
                    if ((colMaps != null) && (colMaps.Count > 0))
                    {
                        newMaps = new Collection<ColumnToColumn>();
                        ColumnToColumn newCol2Col;
                        foreach (ColumnToColumn colMap in link.ColumnToColumns)
                        {
                            newCol2Col = new ColumnToColumn();
                            newCol2Col.TargetColumn = colMap.TargetColumn;
                            if (colMaps.ContainsKey(colMap.SourceColumn))
                                newCol2Col.SourceColumn = colMaps[colMap.SourceColumn];
                            else
                                newCol2Col.SourceColumn = colMap.SourceColumn;
                            newMaps.Add(newCol2Col);
                        }
                    }
                    else
                        newMaps = link.ColumnToColumns;

                    SetDataRowValueFromObject(targetRow, linkObj, newMaps);
                }
            }
        }

        private static void SetColumnValueFromStruct(DataRow targetRow, object obj, PropertyInfo[] properties, ManyToStructureType structure, Dictionary<string, string> colMaps)
        {
            if ((structure.PropertyToColumn == null)
               || (structure.PropertyToColumn.Count == 0))
                throw new ArgumentNullException();

            object objStruct = GetPropertyValue(obj, properties, structure.Property);
            if (objStruct == null)
                return;

            Type objType = objStruct.GetType();
            PropertyInfo[] structProps = GetProperties(objType);
            string trueColumn;
            object objValue;
            foreach (StructPropertyMap structMap in structure.PropertyToColumn)
            {
                trueColumn = GetTrueColumn(structMap.Column, colMaps);
                objValue = GetPropertyValue(objStruct, structProps, structMap.Property);

                if ((!String.IsNullOrEmpty(structMap.ActualProperty)) && (objValue != null))
                    objValue = GetPropertyValue(objValue
                       , objValue.GetType().GetProperties()
                       , structMap.ActualProperty);

                SetDataColumnValue(targetRow, trueColumn, objValue);
            }
        }

        private static void SetColumnValueFromState(DataRow targetRow, object obj, PropertyInfo[] properties, OneToStateType state, Dictionary<string, string> colMaps)
        {
            //if (String.IsNullOrEmpty(state.Column))
            //   throw new ArgumentNullException();

            object objState = GetPropertyValue(obj, properties, state.Property);
            if (objState == null)
                return;

            // 调用生成返回值的方法
            Type objType = objState.GetType();
            MethodInfo[] methods = GetMethods(objType);
            if (methods != null)
            {
                object[] attrs;
                foreach (MethodInfo method in methods)
                {
                    attrs = method.GetCustomAttributes(false);
                    if (CheckSpecialAttribute(attrs, MethodSpecialKind.StateGetValueMethod))
                    {
                        string trueCol = GetTrueColumn(state.Column, colMaps);
                        object objValue = method.Invoke(objState, null);
                        SetDataColumnValue(targetRow, trueCol, objValue);
                        break;
                    }
                }
            }
        }

        private static void SetColumnValueFromNormal(DataRow targetRow, object obj, PropertyInfo[] properties, OneToOneType one, Dictionary<string, string> colMaps)
        {
            object objValue = GetPropertyValue(obj, properties, one.Property);

            string column = GetTrueColumn(one.Column, colMaps);

            SetDataColumnValue(targetRow, column, objValue);
        }

        private static void SetDataColumnValue(DataRow row, string columnName, object objValue)
        {
            if (row.Table.Columns.Contains(columnName))
            {
                if (objValue != null)
                {
                    if (objValue.GetType() == typeof(DateTime))
                        row[columnName] = ((DateTime)objValue).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture);
                    else
                        row[columnName] = objValue;
                }
                else
                {
                    if (row.Table.Columns[columnName].AllowDBNull)
                        row[columnName] = DBNull.Value;
                    else
                        row[columnName] = 1;
                }
            }
        }

        private static void SetObjectPropertyFromDataRow(object targetObj, DataRow sourceRow, Collection<ColumnToColumn> colMaps)
        {
            try
            {
                if (targetObj == null)
                {
                    return;
                }
                Type objType = targetObj.GetType();

                ORMapping orm = FindORMapping(CastClassNameFromFullName(objType.FullName));
                if (orm == null)
                    throw new ArgumentOutOfRangeException("未找到指定类的定义");

                // 获取类所有Public属性
                PropertyInfo[] properties = GetProperties(objType);

                // 如果有BeginInit方法则调用
                MethodInfo beginInitMethod = GetMethod(objType, "BeginInit");
                if (beginInitMethod != null)
                    beginInitMethod.Invoke(targetObj, null);

                Dictionary<string, ColumnToColumn> colMapDic = new Dictionary<string, ColumnToColumn>(); // 原始表中列名－当前表中列名
                if (colMaps != null)
                {
                    for (int i = 0; i < colMaps.Count; i++)
                    {
                        if (!colMapDic.ContainsKey(colMaps[i].TargetColumn))//先判断是否添加了相同的KEY
                        {
                            colMapDic.Add(colMaps[i].TargetColumn, colMaps[i]);
                        }
                    }

                }

                // 给属性赋值
                if (orm.OneOnes != null)
                {
                    foreach (OneToOneType oneOne in orm.OneOnes)
                        SetPropertyValueOfNormal(targetObj, properties, sourceRow, oneOne, colMapDic);
                }
                if (orm.States != null)
                {
                    foreach (OneToStateType state in orm.States)
                        SetPropertyValueOfState(targetObj, properties, sourceRow, state, colMapDic);
                }
                if (orm.Structures != null)
                {
                    foreach (ManyToStructureType structure in orm.Structures)
                        SetPropertyValueOfStruct(targetObj, properties, sourceRow, structure, colMapDic);
                }
                if (orm.ObjectClasses != null)
                {
                    foreach (ManyToObjectClassType objClass in orm.ObjectClasses)
                        SetPropertyValueOfLinkObject(targetObj, properties, sourceRow, objClass, colMapDic);
                }
                if (orm.SubClasses != null)
                {
                    foreach (ManyToSubClassType subObj in orm.SubClasses)
                        SetPropertyValueOfSubClass(targetObj, properties, sourceRow, subObj);
                }

                // 如果有EndInit方法则调用
                MethodInfo endInitMethod = GetMethod(objType, "EndInit");
                if (endInitMethod != null)
                    endInitMethod.Invoke(targetObj, null);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("SetObjectPropertyFromDataRow方法出错：" + ex.Message);
                //throw;
            }

        }
        #endregion

        #region public methods
        /// <summary>
        /// 创建并初始化指定的类实例
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="sourceRow">用来初始化数据的数据行</param>
        /// <returns></returns>
        public static Object CreateAndIntializeObject(string objectName, DataRow sourceRow)
        {
            return CreateAndIntializeObject(objectName, sourceRow, null);
        }

        /// <summary>
        /// 创建并初始化指定的类实例
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="sourceRow">用来初始化数据的数据行</param>
        /// <param name="colMaps">如果是通过数据行中的部分字段创建引用的类，则需要提供当前数据集与类默认数据集的字段映射关系</param>
        /// <returns></returns>
        public static Object CreateAndIntializeObject(string objectName, DataRow sourceRow, Collection<ColumnToColumn> colMaps)
        {
            if (String.IsNullOrEmpty(objectName))
                throw new ArgumentNullException();
            if (sourceRow == null)
                throw new ArgumentNullException();

            // 根据传入对象名创建实例
            Type objType = Type.GetType(EOPNameSpace + objectName);
            object obj = Activator.CreateInstance(objType);

            SetObjectPropertyFromDataRow(obj, sourceRow, colMaps);
            return obj;
        }

        /// <summary>
        /// 用DataRow初始化对象的属性
        /// </summary>
        /// <param name="targetObj"></param>
        /// <param name="sourceRow"></param>
        public static void InitializeObjectProperty(object targetObj, DataRow sourceRow)
        {
            if (targetObj == null)
                throw new ArgumentNullException();

            SetObjectPropertyFromDataRow(targetObj, sourceRow, null);
        }

        /// <summary>
        /// 用对象属性给DataRow中对应的字段赋值
        /// </summary>
        /// <param name="targetRow">需要赋值的数据行</param>
        /// <param name="obj">用来赋值的对象</param>
        public static void SetDataRowValueFromObject(DataRow targetRow, object obj)
        {
            SetDataRowValueFromObject(targetRow, obj, null);
        }

        /// <summary>
        /// 用对象属性给DataRow中对应的字段赋值
        /// </summary>
        /// <param name="targetRow">需要赋值的数据行</param>
        /// <param name="obj">用来赋值的对象</param>
        /// <param name="colMaps">如果要使用关联类的属性来赋值，则需要提供当前数据集与类默认数据集的字段映射关系</param>
        public static void SetDataRowValueFromObject(DataRow targetRow, object obj, Collection<ColumnToColumn> colMaps)
        {
            if (obj == null)
                throw new ArgumentNullException();
            if (targetRow == null)
                throw new ArgumentNullException();

            // 根据传入对象名创建实例
            Type objType = obj.GetType();

            ORMapping orm = FindORMapping(CastClassNameFromFullName(objType.FullName));
            if (orm == null)
                throw new ArgumentOutOfRangeException("未找到指定类的定义");

            // 获取类所有Public属性
            PropertyInfo[] properties = GetProperties(objType);

            // 简化处理：先统一赋字段的默认值，再根据属性赋值
            if (orm.DefaultValues != null)
            {
                foreach (DefaultValueType defVal in orm.DefaultValues)
                    if (targetRow.Table.Columns.Contains(defVal.Column))
                        targetRow[defVal.Column] = defVal.Value;
            }

            Dictionary<string, string> colMapDic = new Dictionary<string, string>(); // 原始表中列名－当前表中列名
            if (colMaps != null)
            {
                foreach (ColumnToColumn colCol in colMaps)
                {
                    if (!String.IsNullOrEmpty(colCol.SourceColumn))
                        colMapDic.Add(colCol.TargetColumn, colCol.SourceColumn);
                }
            }

            // 给字段赋值
            if (orm.OneOnes != null)
            {
                foreach (OneToOneType one in orm.OneOnes)
                    SetColumnValueFromNormal(targetRow, obj, properties, one, colMapDic);
            }
            if (orm.States != null)
            {
                foreach (OneToStateType state in orm.States)
                    SetColumnValueFromState(targetRow, obj, properties, state, colMapDic);
            }
            if (orm.Structures != null)
            {
                foreach (ManyToStructureType structure in orm.Structures)
                    SetColumnValueFromStruct(targetRow, obj, properties, structure, colMapDic);
            }
            if (orm.ObjectClasses != null)
            {
                foreach (ManyToObjectClassType objClass in orm.ObjectClasses)
                    SetColumnValueFromLinkObject(targetRow, obj, properties, objClass, colMapDic);
            }
            if (orm.SubClasses != null)
            {
                foreach (ManyToSubClassType subObj in orm.SubClasses)
                    SetColumnValueFromSubClass(targetRow, obj, properties, subObj);
            }
        }

        /// <summary>
        /// 重新调用ORM的设置数据
        /// </summary>
        public static void ResetORMSettings()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ORMCollection));

            //FileStream fileStream = new FileStream(FullOrmSettingFileName, FileMode.Open);

            _ormSettings = (ORMCollection)serializer.Deserialize(BasicSettings.GetConfig(BasicSettings.ORMappingSetting));
            // 合并父类和子类的设置
            MergeParentAndChildSetting();

            //fileStream.Close();
        }

        /// <summary>
        /// 以传入对象为母版克隆对象
        /// </summary>
        /// <param name="sourceObj"></param>
        /// <returns></returns>
        public static object CloneEopBaseObject(object sourceObj)
        {
            if (sourceObj == null)
                return null;

            Type objType = sourceObj.GetType();

            if (objType.IsValueType)
                return null;

            object newObj = Activator.CreateInstance(objType);
            // 复制属性值

            // 取出所有Public属性，如果有Set方法，则赋值
            // 如果是Class则递归调用，如果是结构体，则要用默认构造创建

            // 获取类所有Public属性
            PropertyInfo[] properties = GetProperties(objType);

            // 如果有BeginInit方法则调用
            MethodInfo beginInitMethod = GetMethod(objType, "BeginInit");
            if (beginInitMethod != null)
                beginInitMethod.Invoke(newObj, null);

            object objValue;
            foreach (PropertyInfo prop in properties)
            {
                if (!prop.CanWrite)
                    continue;

                objValue = prop.GetValue(sourceObj, null);

                if ((prop.PropertyType.IsValueType) || (prop.PropertyType == typeof(string)))
                {
                    prop.SetValue(newObj, objValue, null);
                }
                else if (prop.PropertyType.IsClass)
                {
                    if (prop.PropertyType.BaseType == typeof(MulticastDelegate))
                        prop.SetValue(newObj, objValue, null);
                    else
                    {
                        object obj = PersistentObjectFactory.CloneEopBaseObject(objValue);
                        prop.SetValue(newObj, obj, null);
                    }
                }
            }

            // 如果有EndInit方法则调用
            MethodInfo endInitMethod = GetMethod(objType, "EndInit");
            if (endInitMethod != null)
                endInitMethod.Invoke(newObj, null);

            return newObj;
        }

        /// <summary>
        /// 提取指定名称的查询语句
        /// </summary>
        /// <param name="sqlName">查询语句名称</param>
        /// <returns>查询语句</returns>
        public static string GetQuerySentenceByName(string sqlName)
        {
            foreach (SelectSentence sqlStatement in SqlSentences.Sentences)
                if (sqlStatement.Name == sqlName)
                    return sqlStatement.QuerySentence;

            throw new ArgumentOutOfRangeException(sqlName);
        }
        #endregion

    }
}
