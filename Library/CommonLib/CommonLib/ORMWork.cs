using DrectSoft.DSSqlHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CommonLib
{
    public class ORMWork
    {
        public enum DataBaseOpTypes
        {
            Insert,
            Update,
            Delete
        }

        public string[] SqlTypes = new string[]
		{
			"BigInt",
			"Binary",
			"Bit",
			"Char",
			"DateTime",
			"Decimal",
			"Float",
			"Image",
			"Int",
			"Money",
			"NChar",
			"NText",
			"NVarChar",
			"Real",
			"SmallDateTime",
			"SmallInt",
			"SmallMoney",
			"Structured",
			"Text",
			"Timestamp",
			"TinyInt",
			"UniqueIdentifier",
			"VarBinary",
			"VarChar",
			"Variant"
		};

        public string[] SqlTypeLens = new string[]
		{
			"8",
			"50",
			"1",
			"10",
			"8",
			"18,2",
			"8",
			"16",
			"4",
			"8",
			"10",
			"16",
			"50",
			"4",
			"4",
			"2",
			"4",
			"",
			"16",
			"8",
			"1",
			"16",
			"50",
			"50",
			""
		};

        public string[] OracleTypes = new string[]
		{
			"Blob",
			"Char",
			"Clob",
			"DateTime",
			"Double",
			"Float",
			"Int16",
			"Int32",
			"NChar",
			"NClob",
			"Number",
			"VarChar",
			"NVarChar"
		};

        public string[] OracleTypeLens = new string[]
		{
			"",
			"50",
			"",
			"",
			"",
			"",
			"",
			"",
			"50",
			"",
			"18,2",
			"50",
			"50"
		};

        public string[] SqlTranDataTypeString = new string[]
		{
			"int",
			"byte[]",
			"bool",
			"string",
			"DateTime",
			"decimal",
			"double",
			"byte[]",
			"int",
			"decimal",
			"string",
			"string",
			"string",
			"float",
			"DateTime",
			"short",
			"decimal",
			"object",
			"string",
			"DateTime",
			"byte",
			"Guid",
			"byte[]",
			"string",
			"object"
		};

        public string[] OracleTranDataTypeString = new string[]
		{
			"string",
			"string",
			"string",
			"DateTime",
			"double",
			"float",
			"int",
			"int",
			"string",
			"string",
			"decimal",
			"string",
			"string"
		};

        public string GetBindTableName(object _obj)
        {
            string result;
            try
            {
                if (_obj == null)
                {
                    throw new Exception("GetBindTableName:实体对象为空!");
                }
                if (_obj.GetType().GetCustomAttributes(true) == null || _obj.GetType().GetCustomAttributes(true)[1] == null)
                {
                    throw new Exception("GetBindTableName:实体对象没有映射到表!");
                }
                BindTableAttribute bindTableAttribute = _obj.GetType().GetCustomAttributes(true)[1] as BindTableAttribute;
                string name = bindTableAttribute.Name;
                result = name;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public string GetBindFields(object _obj, ORMWork.DataBaseOpTypes _DataBaseOpTypes)
        {
            string result;
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                if (_obj == null)
                {
                    throw new Exception("GetBindFields:实体对象为空!");
                }
                for (int i = 0; i < _obj.GetType().GetProperties().Length; i++)
                {
                    PropertyInfo propertyInfo = _obj.GetType().GetProperties()[i];
                    if (propertyInfo.GetCustomAttributes(true) == null || propertyInfo.GetCustomAttributes(true)[0] == null)
                    {
                        throw new Exception("GetBindFields:实体对象中属性没有映射到表中的列!");
                    }
                    BindFieldAttribute bindFieldAttribute = propertyInfo.GetCustomAttributes(true)[0] as BindFieldAttribute;
                    if (!(bindFieldAttribute.Name.Trim() == string.Empty))
                    {
                        if (_DataBaseOpTypes != ORMWork.DataBaseOpTypes.Insert || (!(bindFieldAttribute.DefaultValue.ToUpper() == "SYS_GUID()") && !(bindFieldAttribute.DefaultValue.ToUpper() == "SYSDATE") && !(bindFieldAttribute.DefaultValue.ToLower() == "newid()") && !(bindFieldAttribute.DefaultValue.ToLower() == "getdate()")))
                        {
                            if (ORMWork.DataBaseOpTypes.Update != _DataBaseOpTypes || (!bool.Parse(bindFieldAttribute.Key.ToString()) && !(bindFieldAttribute.Name.ToUpper() == "ID") && !(bindFieldAttribute.Name.ToUpper() == "INPUTEMPID") && !(bindFieldAttribute.Name.ToUpper() == "INPUTTIME")))
                            {
                                if (ORMWork.DataBaseOpTypes.Delete != _DataBaseOpTypes)
                                {
                                    if (stringBuilder.Length > 0)
                                    {
                                        stringBuilder.Append(",");
                                    }
                                    stringBuilder.Append(bindFieldAttribute.Name.Trim());
                                }
                            }
                        }
                    }
                }
                result = stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public object GetDbType(string _typename, string _DbProviderName)
        {
            object result;
            try
            {
                object obj = null;
                if (_typename == string.Empty)
                {
                    throw new Exception("GetDbType:传入类型示指定");
                }
                if (_DbProviderName == "System.Data.OracleClient")
                {
                    for (int i = 0; i < this.OracleTypes.Length; i++)
                    {
                        if (this.OracleTypes[i].ToLower() == _typename.ToLower())
                        {
                            obj = Enum.Parse(typeof(OracleType), _typename);
                            break;
                        }
                    }
                }
                else if (_DbProviderName == "System.Data.SqlClient")
                {
                    for (int i = 0; i < this.SqlTypes.Length; i++)
                    {
                        if (this.SqlTypes[i].ToLower() == _typename.ToLower())
                        {
                            obj = Enum.Parse(typeof(SqlDbType), _typename);
                            break;
                        }
                    }
                }
                result = obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public BindFieldAttribute GetAnBindFieldAttributeByFieldName(object _obj, string _fieldname)
        {
            BindFieldAttribute result;
            try
            {
                BindFieldAttribute bindFieldAttribute = null;
                if (_obj == null)
                {
                    throw new Exception("GetAnBindFieldAttributeByFieldName:实体对象为空!");
                }
                if (_fieldname == string.Empty)
                {
                    throw new Exception("GetAnBindFieldAttributeByFieldName:待查名称为空!");
                }
                for (int i = 0; i < _obj.GetType().GetProperties().Length; i++)
                {
                    if (!(_obj.GetType().GetProperties()[i].Name != _fieldname))
                    {
                        PropertyInfo propertyInfo = _obj.GetType().GetProperties()[i];
                        if (propertyInfo.GetCustomAttributes(true) == null || propertyInfo.GetCustomAttributes(true)[0] == null)
                        {
                            throw new Exception("GetAnBindFieldAttributeByFieldName:实体对象中属性没有映射到表中的列!");
                        }
                        BindFieldAttribute bindFieldAttribute2 = propertyInfo.GetCustomAttributes(true)[0] as BindFieldAttribute;
                        bindFieldAttribute = bindFieldAttribute2;
                    }
                }
                result = bindFieldAttribute;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DbParameter[] CreateParameters(object _obj, ORMWork.DataBaseOpTypes _DataBaseOpTypes, out string _fieldpars, out string _where)
        {
            DbParameter[] result;
            try
            {
                _fieldpars = "";
                _where = "";
                string text = "";
                List<DbParameter> list = new List<DbParameter>();
                if (_obj == null)
                {
                    throw new Exception("CreateParameters:实体对象为空");
                }
                list.Clear();
                for (int i = 0; i < _obj.GetType().GetProperties().Length; i++)
                {
                    BindFieldAttribute anBindFieldAttributeByFieldName = this.GetAnBindFieldAttributeByFieldName(_obj, _obj.GetType().GetProperties()[i].Name);
                    if (_DataBaseOpTypes != ORMWork.DataBaseOpTypes.Insert || (!(anBindFieldAttributeByFieldName.DefaultValue.ToUpper() == "SYS_GUID()") && !(anBindFieldAttributeByFieldName.DefaultValue.ToUpper() == "SYSDATE") && !(anBindFieldAttributeByFieldName.DefaultValue.ToLower() == "newid()") && !(anBindFieldAttributeByFieldName.DefaultValue.ToLower() == "getdate()")))
                    {
                        if (_DataBaseOpTypes != ORMWork.DataBaseOpTypes.Insert || (!(anBindFieldAttributeByFieldName.DefaultValue.ToUpper() == "SYS_GUID()") && !(anBindFieldAttributeByFieldName.DefaultValue.ToUpper() == "SYSDATE") && !(anBindFieldAttributeByFieldName.DefaultValue.ToLower() == "newid()") && !(anBindFieldAttributeByFieldName.DefaultValue.ToLower() == "getdate()")))
                        {
                            if (ORMWork.DataBaseOpTypes.Update == _DataBaseOpTypes && (bool.Parse(anBindFieldAttributeByFieldName.Key.ToString()) || anBindFieldAttributeByFieldName.Name.ToUpper() == "ID"))
                            {
                                if (text.Trim() != "")
                                {
                                    text += " AND ";
                                }
                                text = anBindFieldAttributeByFieldName.Name + "=@" + anBindFieldAttributeByFieldName.Name;
                                list.Add(this.CreateAnParameter(anBindFieldAttributeByFieldName));
                            }
                            else
                            {
                                if (ORMWork.DataBaseOpTypes.Delete == _DataBaseOpTypes && (bool.Parse(anBindFieldAttributeByFieldName.Key.ToString()) || anBindFieldAttributeByFieldName.Name.ToUpper() == "ID"))
                                {
                                    if (text.Trim() != "")
                                    {
                                        text += " AND ";
                                    }
                                    text = anBindFieldAttributeByFieldName.Name + "=@" + anBindFieldAttributeByFieldName.Name;
                                    list.Add(this.CreateAnParameter(anBindFieldAttributeByFieldName));
                                    break;
                                }
                                if (_fieldpars.Trim() != "")
                                {
                                    _fieldpars += ",";
                                }
                                _fieldpars = _fieldpars + "@" + anBindFieldAttributeByFieldName.Name;
                                list.Add(this.CreateAnParameter(anBindFieldAttributeByFieldName));
                            }
                        }
                    }
                }
                if (text != "")
                {
                    text = " WHERE " + text;
                }
                _where = text;
                result = list.ToArray<DbParameter>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DbParameter CreateAnParameter(BindFieldAttribute _BindFieldAttribute)
        {
            DbParameter result;
            try
            {
                DbParameter dbParameter = null;
                if (_BindFieldAttribute == null)
                {
                    throw new Exception("CreateAnParameter:对象BindFieldAttribute为空!");
                }
                if (DS_SqlHelper.DbProviderName == "System.Data.OracleClient")
                {
                    dbParameter = new OracleParameter
                    {
                        ParameterName = "@" + _BindFieldAttribute.Name,
                        OracleType = (OracleType)this.GetDbType(_BindFieldAttribute.DbType, DS_SqlHelper.DbProviderName)
                    };
                }
                else if (DS_SqlHelper.DbProviderName == "System.Data.SqlClient")
                {
                    dbParameter = new SqlParameter
                    {
                        ParameterName = "@" + _BindFieldAttribute.Name,
                        SqlDbType = (SqlDbType)this.GetDbType(_BindFieldAttribute.DbType, DS_SqlHelper.DbProviderName)
                    };
                }
                result = dbParameter;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public void SetParameterValues(object _obj, DbParameter[] pars)
        {
            try
            {
                if (_obj == null)
                {
                    throw new Exception("SetParameterValues:实体对象为空");
                }
                if (pars == null)
                {
                    throw new Exception("SetParameterValues:参数对象数组为空");
                }
                for (int i = 0; i < pars.Length; i++)
                {
                    for (int j = 0; j < _obj.GetType().GetProperties().Length; j++)
                    {
                        BindFieldAttribute anBindFieldAttributeByFieldName = this.GetAnBindFieldAttributeByFieldName(_obj, _obj.GetType().GetProperties()[j].Name);
                        if ("@" + anBindFieldAttributeByFieldName.Name == pars[i].ParameterName)
                        {
                            pars[i].Value = _obj.GetType().GetProperties()[j].GetValue(_obj, null);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string MakeInserSql(object _obj, out DbParameter[] pars)
        {
            string result;
            try
            {
                if (_obj == null)
                {
                    throw new Exception("MakeInserSql:实体对象为空");
                }
                StringBuilder stringBuilder = new StringBuilder();
                string text = "";
                string bindTableName = this.GetBindTableName(_obj);
                string bindFields = this.GetBindFields(_obj, ORMWork.DataBaseOpTypes.Insert);
                string value = "";
                pars = this.CreateParameters(_obj, ORMWork.DataBaseOpTypes.Insert, out value, out text);
                stringBuilder.Append("INSERT INTO ");
                stringBuilder.Append(bindTableName.ToUpper() + " ");
                stringBuilder.Append("( ");
                stringBuilder.Append(bindFields);
                stringBuilder.Append(") ");
                stringBuilder.Append(" VALUES ");
                stringBuilder.Append(" (");
                stringBuilder.Append(value);
                stringBuilder.Append(" )");
                result = stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public string MakeUpdateSql(object _obj, out DbParameter[] pars)
        {
            string result;
            try
            {
                if (_obj == null)
                {
                    throw new Exception("MakeUpdateSql:实体对象为空");
                }
                StringBuilder stringBuilder = new StringBuilder();
                string bindTableName = this.GetBindTableName(_obj);
                string text = "";
                string text2 = "";
                pars = this.CreateParameters(_obj, ORMWork.DataBaseOpTypes.Update, out text, out text2);
                stringBuilder.Append("UPDATE ");
                stringBuilder.Append(bindTableName.ToUpper() + " ");
                StringBuilder stringBuilder2 = new StringBuilder();
                string[] array = text.Split(new char[]
				{
					','
				});
                for (int i = 0; i < array.Length; i++)
                {
                    if (stringBuilder2.Length > 0)
                    {
                        stringBuilder2.Append(" , ");
                    }
                    stringBuilder2.Append(array[i].Replace("@", "").ToUpper() + "=" + array[i]);
                }
                stringBuilder.Append(" SET ");
                stringBuilder.Append(stringBuilder2);
                if (text2 != "")
                {
                    stringBuilder.Append(text2);
                }
                result = stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public string MakeDeleteSql(object _obj, out DbParameter[] pars)
        {
            string result;
            try
            {
                if (_obj == null)
                {
                    throw new Exception("MakeDeleteSql:实体对象为空");
                }
                StringBuilder stringBuilder = new StringBuilder();
                string bindTableName = this.GetBindTableName(_obj);
                string text = "";
                string text2 = "";
                pars = this.CreateParameters(_obj, ORMWork.DataBaseOpTypes.Delete, out text, out text2);
                stringBuilder.Append("DELETE ");
                stringBuilder.Append(" FROM ");
                stringBuilder.Append(bindTableName.ToUpper() + " ");
                string[] array = text.Split(new char[]
				{
					','
				});
                if (text2 != "")
                {
                    stringBuilder.Append(text2);
                }
                result = stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public virtual int InserAnObject(object _obj)
        {
            int result;
            try
            {
                if (_obj == null)
                {
                    throw new Exception("InserAnObject:实体对象为空");
                }
                DbParameter[] array = null;
                string text = this.MakeInserSql(_obj, out array);
                this.SetParameterValues(_obj, array);
                int num = DS_SqlHelper.ExecuteNonQuery(text, array, CommandType.Text);
                result = num;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public virtual bool InserAnObjectInTran(object _obj)
        {
            bool result;
            try
            {
                bool flag = true;
                if (_obj == null)
                {
                    throw new Exception("InserAnObjectInTran:实体对象为空");
                }
                DbParameter[] array = null;
                string text = this.MakeInserSql(_obj, out array);
                this.SetParameterValues(_obj, array);
                if (DS_SqlHelper.AppDbTransaction == null)
                {
                    throw new Exception("InserAnObjectInTran:YiDanSqlHelper.DS_SqlHelper没有创建事务!");
                }
                DS_SqlHelper.ExecuteNonQueryInTran(text, array, CommandType.Text);
                result = flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public virtual bool UpdateAnObjectInTran(object _obj)
        {
            bool result;
            try
            {
                bool flag = true;
                if (_obj == null)
                {
                    throw new Exception("UpdateAnObjectInTran:实体对象为空");
                }
                DbParameter[] array = null;
                string text = this.MakeUpdateSql(_obj, out array);
                this.SetParameterValues(_obj, array);
                if (DS_SqlHelper.AppDbTransaction == null)
                {
                    throw new Exception("UpdateAnObjectInTran:YiDanSqlHelper.DS_SqlHelper没有创建事务!");
                }
                DS_SqlHelper.ExecuteNonQueryInTran(text, array, CommandType.Text);
                result = flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public virtual int UpdateAnObject(object _obj)
        {
            int result;
            try
            {
                if (_obj == null)
                {
                    throw new Exception("UpdateAnObject:实体对象为空");
                }
                DbParameter[] array = null;
                string text = this.MakeUpdateSql(_obj, out array);
                this.SetParameterValues(_obj, array);
                int num = DS_SqlHelper.ExecuteNonQuery(text, array, CommandType.Text);
                result = num;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public virtual int DeleteAnObject(object _obj)
        {
            int result;
            try
            {
                if (_obj == null)
                {
                    throw new Exception("DeleteAnObject:实体对象为空");
                }
                DbParameter[] array = null;
                string text = this.MakeDeleteSql(_obj, out array);
                this.SetParameterValues(_obj, array);
                int num = DS_SqlHelper.ExecuteNonQuery(text, array, CommandType.Text);
                result = num;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public virtual bool DeleteAnObjectInTran(object _obj)
        {
            bool result;
            try
            {
                bool flag = true;
                if (_obj == null)
                {
                    throw new Exception("DeleteAnObjectInTran:实体对象为空");
                }
                DbParameter[] array = null;
                string text = this.MakeDeleteSql(_obj, out array);
                this.SetParameterValues(_obj, array);
                if (DS_SqlHelper.AppDbTransaction == null)
                {
                    throw new Exception("DeleteAnObjectInTran:DrectSoft.DSSqlHelper.DS_SqlHelper没有创建事务!");
                }
                DS_SqlHelper.ExecuteNonQueryInTran(text, array, CommandType.Text);
                result = flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public virtual object GetAnObject(object _hasidobj)
        {
            object result;
            try
            {
                if (_hasidobj == null)
                {
                    throw new Exception("GetAnObject:实体对象为空");
                }
                string bindTableName = this.GetBindTableName(_hasidobj);
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(string.Format("SELECT * FROM {0}", bindTableName));
                stringBuilder.Append(" WHERE ");
                stringBuilder.Append(" ID=@ID ");
                string text = stringBuilder.ToString();
                DbParameter[] array = new DbParameter[]
				{
					this.CreateAnParameter(this.GetAnBindFieldAttributeByFieldName(_hasidobj, "ID"))
				};
                this.SetParameterValues(_hasidobj, array);
                DataTable dataTable = DS_SqlHelper.ExecuteDataTable(text, array, CommandType.Text);
                if (dataTable == null || dataTable.Rows.Count == 0)
                {
                    result = _hasidobj;
                }
                else
                {
                    for (int i = 0; i < _hasidobj.GetType().GetProperties().Length; i++)
                    {
                        BindFieldAttribute anBindFieldAttributeByFieldName = this.GetAnBindFieldAttributeByFieldName(_hasidobj, _hasidobj.GetType().GetProperties()[i].Name);
                        if (dataTable.Rows[0][anBindFieldAttributeByFieldName.Name] != null)
                        {
                            if (_hasidobj.GetType().GetProperties()[i].PropertyType.ToString().ToLower() == "System.DateTime".ToLower() && dataTable.Rows[0][anBindFieldAttributeByFieldName.Name].ToString() != "")
                            {
                                _hasidobj.GetType().GetProperties()[i].SetValue(_hasidobj, DateTime.Parse(dataTable.Rows[0][anBindFieldAttributeByFieldName.Name].ToString()), null);
                            }
                            else if (_hasidobj.GetType().GetProperties()[i].PropertyType.ToString().ToLower() == "System.Int16".ToLower() && dataTable.Rows[0][anBindFieldAttributeByFieldName.Name].ToString() != "")
                            {
                                _hasidobj.GetType().GetProperties()[i].SetValue(_hasidobj, short.Parse(dataTable.Rows[0][anBindFieldAttributeByFieldName.Name].ToString()), null);
                            }
                            else if (_hasidobj.GetType().GetProperties()[i].PropertyType.ToString().ToLower() == "System.Int32".ToLower() && dataTable.Rows[0][anBindFieldAttributeByFieldName.Name].ToString() != "")
                            {
                                _hasidobj.GetType().GetProperties()[i].SetValue(_hasidobj, int.Parse(dataTable.Rows[0][anBindFieldAttributeByFieldName.Name].ToString()), null);
                            }
                            else if (_hasidobj.GetType().GetProperties()[i].PropertyType.ToString().ToLower() == "System.Int64".ToLower() && dataTable.Rows[0][anBindFieldAttributeByFieldName.Name].ToString() != "")
                            {
                                _hasidobj.GetType().GetProperties()[i].SetValue(_hasidobj, long.Parse(dataTable.Rows[0][anBindFieldAttributeByFieldName.Name].ToString()), null);
                            }
                            else if (_hasidobj.GetType().GetProperties()[i].PropertyType.ToString().ToLower() == "System.Decimal".ToLower() && dataTable.Rows[0][anBindFieldAttributeByFieldName.Name].ToString() != "")
                            {
                                _hasidobj.GetType().GetProperties()[i].SetValue(_hasidobj, decimal.Parse(dataTable.Rows[0][anBindFieldAttributeByFieldName.Name].ToString()), null);
                            }
                            else if (_hasidobj.GetType().GetProperties()[i].PropertyType.ToString().ToLower() == "System.Double".ToLower() && dataTable.Rows[0][anBindFieldAttributeByFieldName.Name].ToString() != "")
                            {
                                _hasidobj.GetType().GetProperties()[i].SetValue(_hasidobj, double.Parse(dataTable.Rows[0][anBindFieldAttributeByFieldName.Name].ToString()), null);
                            }
                            else if (_hasidobj.GetType().GetProperties()[i].PropertyType.ToString().ToLower() == "System.Single".ToLower() && dataTable.Rows[0][anBindFieldAttributeByFieldName.Name].ToString() != "")
                            {
                                _hasidobj.GetType().GetProperties()[i].SetValue(_hasidobj, float.Parse(dataTable.Rows[0][anBindFieldAttributeByFieldName.Name].ToString()), null);
                            }
                            else if (dataTable.Rows[0][anBindFieldAttributeByFieldName.Name].ToString() != "")
                            {
                                _hasidobj.GetType().GetProperties()[i].SetValue(_hasidobj, dataTable.Rows[0][anBindFieldAttributeByFieldName.Name], null);
                            }
                        }
                    }
                    result = _hasidobj;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}
