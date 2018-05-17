using DrectSoft.DSSqlHelper;
using System;
using System.Data;
using System.Text;

namespace CommonLib
{
    public class ORMScript : ORMScriptTempletInfo
    {
        public string GetCreateTableScript(DataTableForCreateScript _dt)
        {
            string result;
            try
            {
                if (_dt == null)
                {
                    throw new Exception("GetCreateTableScript:传入_dt为空");
                }
                StringBuilder stringBuilder = new StringBuilder();
                StringBuilder stringBuilder2 = new StringBuilder();
                StringBuilder stringBuilder3 = new StringBuilder();
                for (int i = 0; i < _dt.Rows.Count; i++)
                {
                    string text = "";
                    string text2 = "";
                    string text3 = "";
                    this.MakeTableInfo(_dt.Rows[i], out text, out text2, out text3);
                    if (!(text.Trim() == ""))
                    {
                        if (stringBuilder.Length != 0)
                        {
                            stringBuilder.AppendLine(",");
                        }
                        stringBuilder.Append(text);
                        if (text2.Trim() != "")
                        {
                            if (stringBuilder2.Length != 0)
                            {
                                stringBuilder2.Append(",");
                            }
                            stringBuilder2.Append(text2);
                        }
                        if (text3.Trim() != "")
                        {
                            stringBuilder3.AppendLine(text3);
                        }
                    }
                }
                StringBuilder stringBuilder4 = new StringBuilder();
                stringBuilder4.AppendLine(string.Format("CREATE TABLE {0} ", ORMScriptTempletInfo.TablenamePlaceholder));
                stringBuilder4.AppendLine("(");
                stringBuilder4.AppendLine(stringBuilder.ToString());
                stringBuilder4.Append(")");
                stringBuilder4.AppendLine(";");
                if (stringBuilder2.Length != 0)
                {
                    stringBuilder2.Insert(0, string.Concat(new string[]
					{
						"alter table ",
						ORMScriptTempletInfo.TablenamePlaceholder,
						" add constraint PK_",
						ORMScriptTempletInfo.TablenamePlaceholder,
						" primary key ("
					}));
                    stringBuilder2.Append(")");
                    stringBuilder2.Append(";");
                    stringBuilder4.AppendLine(stringBuilder2.ToString());
                }
                if (stringBuilder3.Length != 0)
                {
                    stringBuilder4.AppendLine(stringBuilder3.ToString());
                }
                string text4 = stringBuilder4.ToString();
                result = text4;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private void MakeTableInfo(DataRow _dr, out string _tablebody, out string _key, out string _comment)
        {
            try
            {
                _tablebody = "";
                _key = "";
                _comment = "";
                if (_dr != null)
                {
                    if (_dr["COLUMNNAME"] == null || _dr["COLUMNNAME"].ToString() == "")
                    {
                        throw new Exception("列名不能为空");
                    }
                    if (_dr["DATATYPE"] == null || _dr["DATATYPE"].ToString().Trim() == "")
                    {
                        throw new Exception("类型不能为空");
                    }
                    _tablebody += "\t";
                    _tablebody = _tablebody + _dr["COLUMNNAME"].ToString().ToUpper() + " ";
                    _tablebody += _dr["DATATYPE"].ToString();
                    if (_dr["DATALEN"] != null && _dr["DATALEN"].ToString().Trim() != "")
                    {
                        _tablebody = _tablebody + "(" + _dr["DATALEN"].ToString() + ")";
                    }
                    _tablebody += " ";
                    if (_dr["DEFUALTVALUE"] != null && _dr["DEFUALTVALUE"].ToString().Trim() != "")
                    {
                        _tablebody = _tablebody + "DEFUALT " + _dr["DEFUALTVALUE"].ToString() + " ";
                    }
                    if (_dr["ALLOWNULL"] != null && _dr["ALLOWNULL"].ToString().Trim() != "0")
                    {
                        _tablebody += " NOT NULL ";
                    }
                    if (_dr["ISPRIMARY"] != null && _dr["ISPRIMARY"].ToString().Trim() != "0" && _dr["ALLOWNULL"] != null && _dr["ALLOWNULL"].ToString().Trim() != "0")
                    {
                        _key += _dr["COLUMNNAME"].ToString();
                    }
                    if (DS_SqlHelper.DbProviderName == "System.Data.OracleClient")
                    {
                        string text = "";
                        if (_dr["SHORTNAME"] != null && _dr["SHORTNAME"].ToString().Trim() != "")
                        {
                            text += _dr["SHORTNAME"].ToString();
                        }
                        if (_dr["REMARK"] != null && _dr["REMARK"].ToString().Trim() != "")
                        {
                            if (text != "")
                            {
                                text += ",";
                            }
                            text += _dr["REMARK"].ToString();
                        }
                        if (text.Trim() != "")
                        {
                            _comment += string.Format("comment on column {0}.{1} is '{2}';", ORMScriptTempletInfo.TablenamePlaceholder, _dr["COLUMNNAME"].ToString(), text);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetCreateClassScript(DataTableForCreateScript _dt)
        {
            string result;
            try
            {
                if (_dt == null)
                {
                    throw new Exception("GetCreateClassScript:传入_dt为空");
                }
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < _dt.Rows.Count; i++)
                {
                    stringBuilder.AppendLine(this.MakePropertyInfo(_dt.Rows[i]));
                }
                string text = stringBuilder.ToString();
                result = text;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public string MakePropertyInfo(DataRow _dr)
        {
            string result;
            try
            {
                if (_dr == null)
                {
                    throw new Exception("MakePropertyInfo:传入_dr为空");
                }
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine(ORMScriptTempletInfo.TempletField);
                stringBuilder.AppendLine(ORMScriptTempletInfo.TempletPropertySummary);
                stringBuilder.AppendLine(ORMScriptTempletInfo.TempletPropertyBindField);
                stringBuilder.AppendLine(ORMScriptTempletInfo.TempletProperty);
                if (_dr["COLUMNNAME"] == null || _dr["COLUMNNAME"].ToString() == "")
                {
                    throw new Exception("字段名不存在或空");
                }
                if (_dr["DATATYPE"] == null || _dr["DATATYPE"].ToString() == "")
                {
                    throw new Exception("数据类型不存在或空");
                }
                stringBuilder.Replace(ORMScriptTempletInfo.PropertyNamePlaceholder, _dr["COLUMNNAME"].ToString());
                stringBuilder.Replace(ORMScriptTempletInfo.PropertyDbTypePlaceholder, _dr["DATATYPE"].ToString());
                stringBuilder.Replace(ORMScriptTempletInfo.PropertyChnamePlaceholder, (_dr["SHORTNAME"] != null) ? _dr["SHORTNAME"].ToString() : "");
                stringBuilder.Replace(ORMScriptTempletInfo.PropertyDefaultValuePlaceholder, (_dr["DEFUALTVALUE"] != null) ? (_dr["DATATYPE"].ToString().ToUpper().Contains("CHAR") ? ((_dr["DEFUALTVALUE"].ToString().Trim() == "") ? "''" : _dr["DEFUALTVALUE"].ToString()) : _dr["DEFUALTVALUE"].ToString()) : "");
                stringBuilder.Replace(ORMScriptTempletInfo.PropertyKeyPlaceholder, (_dr["ISPRIMARY"] != null) ? ((_dr["ISPRIMARY"].ToString() == "1") ? "True" : "False") : "False");
                string text = "";
                if (_dr["SHORTNAME"] != null && _dr["SHORTNAME"].ToString().Trim() != "")
                {
                    text += _dr["SHORTNAME"].ToString();
                }
                if (_dr["REMARK"] != null && _dr["REMARK"].ToString().Trim() != "")
                {
                    if (text != "")
                    {
                        text += ",";
                    }
                    text += _dr["REMARK"].ToString();
                }
                stringBuilder.Replace(ORMScriptTempletInfo.PropertyRemarkPlaceholder, text);
                string newValue = "";
                if (_dr["DATATYPE"].ToString().ToLower().Contains("char") && _dr["DEFUALTVALUE"].ToString().Trim() == "")
                {
                    newValue = "=\"\"";
                }
                stringBuilder.Replace(ORMScriptTempletInfo.FieldInitValuePlaceholder, newValue);
                stringBuilder.Replace(ORMScriptTempletInfo.PropertyDatatypePlaceholder, this.GetDataTypeString(_dr["DATATYPE"].ToString()));
                string text2 = stringBuilder.ToString();
                result = text2;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private string GetDataTypeString(string _dbtype)
        {
            string result;
            try
            {
                string text = "";
                if (_dbtype == null || _dbtype.Trim() == "")
                {
                    throw new Exception("GetDataTypeString:传入参数为空");
                }
                ORMWork oRMWork = new ORMWork();
                if (DS_SqlHelper.DbProviderName == "System.Data.OracleClient")
                {
                    for (int i = 0; i < oRMWork.OracleTypes.Length; i++)
                    {
                        if (oRMWork.OracleTypes[i].ToLower().Trim() == _dbtype.ToLower())
                        {
                            text = oRMWork.OracleTranDataTypeString[i];
                            break;
                        }
                    }
                }
                else if (DS_SqlHelper.DbProviderName == "System.Data.SqlClient")
                {
                    for (int i = 0; i < oRMWork.SqlTypes.Length; i++)
                    {
                        if (oRMWork.SqlTypes[i].ToLower().Trim() == _dbtype.ToLower())
                        {
                            text = oRMWork.SqlTranDataTypeString[i];
                            break;
                        }
                    }
                }
                result = text;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}
