using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DrectSoft.DSSqlHelper;
using System.Data.SqlClient;
using DrectSoft.Common.Ctrs.DLG;

namespace DrectSoft.Emr.QCTimeLimit.QCEntity
{
    /// <summary>
    /// 质量控制条件库
    /// QCCONDITION表对应的实体类
    /// </summary>
    public class QCCondition
    {
        #region Property
        /// <summary>
        /// 条件代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 条件描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 需要配置的表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// TABLENAME表中对应的列名
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// TABLENAME表中COLUMNNAME对应列的具体值
        /// </summary>
        public string ColumnValue { get; set; }

        /// <summary>
        /// COLUMNNAME对应的时间字段名
        /// </summary>
        public string TimeColumnName { get; set; }

        /// <summary>
        /// TIMECOLUMNNAME对应的时间范围(以秒计算)
        /// </summary>
        public int TimeRange { get; set; }

        /// <summary>
        /// TABLENAME表中病人序号字段
        /// </summary>
        public string PatNoColumnName { get; set; }

        /// <summary>
        /// TABLENAME所在的数据库 分为：EMR、HIS
        /// </summary>
        public string DBLink { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public string Valide { get; set; }
        #endregion

        const string c_EMRDB = "EMRDB";
        const string c_SqlQCCondition = "select * from QCCondition";//需要产生最大序号
        const string c_SqlQcCondition2 = @"select code,description,valid,memo,tablename,"
            + "columnname,columnvalue,timecolumnname,timerange,patnocolumnname,"
            + "dblink from QCCondition where valid = '1'and (code like '%'||@code||'%' and Description like '%'||@description||'%') ";
        const string c_SqlInpatient = "select noofinpat,patnoofhis from Inpatient where patnoofhis in ({0})";

        #region 获得所有病历质量条件库
        /// <summary>
        /// 获得所有病历质量条件库 DataTable -> Dictionary<string, QCCondition>
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, QCCondition> GetAllQCCondition()
        {
            try
            {
                DataTable dataTableCondition = GetAllQCConditions();
                return GetAllQCCondition(dataTableCondition);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取得有效时限规则条件集合
        /// add by xlb 2013-01-06
        /// </summary>
        /// <returns></returns>
        public static List<QCCondition> getAllConditions(QCCondition _qcCondition)
        {
            try
            {
                List<QCCondition> qcConditionList = new List<QCCondition>();
                DataTable dtTableCondition = GetAllQcCondition2(_qcCondition);
                foreach (DataRow dr in dtTableCondition.Rows)
                {
                    QCCondition qcConditon = ConvertToQCConditionFromDataRow(dr);
                    qcConditionList.Add(qcConditon);
                }
                return qcConditionList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 新增时限条件
        /// xlb 2013-01-06
        /// </summary>
        /// <param name="qcCondition"></param>
        public static void InsertCondition(QCCondition qcCondition)
        {
            try
            {
                string sqlCondtiion = "insert into QCCondition(code,description,valid,memo,tablename,"
                + "columnname,columnvalue,timecolumnname,timerange,patnocolumnname,dblink) values(@code,"
                + "@description,@valid,@memo,@tablename,@columnname,@columnvalue,@timecolumnname,@timerange,"
                + "@patnocolumnname,@dblink)";
                SqlParameter[] sps ={
                              new SqlParameter("@code",qcCondition.Code),
                              new SqlParameter("@description",qcCondition.Description),
                              new SqlParameter("@valid","1"),
                              new SqlParameter("@memo",qcCondition.Memo),
                              new SqlParameter("@tablename",qcCondition.TableName),
                              new SqlParameter("@columnname",qcCondition.ColumnName),
                              new SqlParameter("@columnvalue",qcCondition.ColumnValue),
                              new SqlParameter("@timecolumnname",qcCondition.TimeColumnName),
                              new SqlParameter("@timerange",qcCondition.TimeRange),
                              new SqlParameter("@patnocolumnname",qcCondition.PatNoColumnName),
                              new SqlParameter("@dblink",qcCondition.DBLink)
                              };
                DS_SqlHelper.ExecuteNonQuery(sqlCondtiion, sps, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 修改时限条件具体信息
        /// by xlb 2013-01-07
        /// </summary>
        /// <param name="qcCondition"></param>
        public static void UpdateCondition(QCCondition qcCondition)
        {
            try
            {
                string sqlUpdateCondition = @"update QCCondition set description=@description,"
                + "memo=@memo,tablename=@tablename,columnname=@columnname,"
                + "columnvalue=@columnvalue,timecolumnname=@timecolumn,timerange=@timerange,"
                + "patnocolumnname=@patnocolumnname,dblink=@dblink where code=@code";
                SqlParameter[] sps ={
                                   new SqlParameter("@description",qcCondition.Description),
                                   new SqlParameter("@memo",qcCondition.Memo),
                                   new SqlParameter("@tablename",qcCondition.TableName),
                                   new SqlParameter("@columnname",qcCondition.ColumnName),
                                   new SqlParameter("@columnvalue",qcCondition.ColumnValue),
                                   new SqlParameter("@timecolumn",qcCondition.TimeColumnName),
                                   new SqlParameter("@timerange",qcCondition.TimeRange),
                                   new SqlParameter("@patnocolumnname",qcCondition.PatNoColumnName),
                                   new SqlParameter("@dblink",qcCondition.DBLink),
                                   new SqlParameter("@code",qcCondition.Code)
                                  };
                DS_SqlHelper.ExecuteNonQuery(sqlUpdateCondition, sps, CommandType.Text);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 数据行转化为时限规则条件对象  datarow->QCCondition
        /// xlb 2013-01-06
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private QCCondition ConvertToQcCondition(DataRow dataRow)
        {
            try
            {
                QCCondition qcConditon = new QCCondition();

                qcConditon.Code = dataRow["CODE"].ToString();
                qcConditon.Description = dataRow["DESCRIPTION"].ToString();
                qcConditon.Memo = dataRow["MEMO"].ToString();
                qcConditon.TableName = dataRow["TABLENAME"].ToString();
                qcConditon.ColumnName = dataRow["COLUMNNAME"].ToString();
                qcConditon.ColumnValue = dataRow["COLUMNVALUE"].ToString();
                qcConditon.TimeColumnName = dataRow["TIMECOLUMNNAME"].ToString();
                qcConditon.TimeRange = (int)dataRow["TIMERANGE"];
                qcConditon.PatNoColumnName = dataRow["PATNOCOLUMNNAME"].ToString();
                qcConditon.DBLink = dataRow["DBLINK"].ToString();
                qcConditon.Valide = dataRow["VALID"].ToString();

                return qcConditon;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获得病历质量条件库 DataTable -> Dictionary<string, QCCondition>
        /// </summary>
        /// <param name="dataTableCondition"></param>
        /// <returns></returns>
        public static Dictionary<string, QCCondition> GetAllQCCondition(DataTable dataTableCondition)
        {
            try
            {
                Dictionary<string, QCCondition> dictCondition = new Dictionary<string, QCCondition>();
                foreach (DataRow dr in dataTableCondition.Rows)
                {
                    QCCondition condition = ConvertToQCConditionFromDataRow(dr);
                    dictCondition.Add(condition.Code, condition);
                }
                return dictCondition;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 从数据行对象转换为条件对象，即 DataRow -> QCCondition
        /// </summary>
        /// <param name="dataRowCondition"></param>
        /// <returns></returns>
        static QCCondition ConvertToQCConditionFromDataRow(DataRow dataRowCondition)
        {
            try
            {
                int result;
                QCCondition condition = new QCCondition();

                #region QCCondition实例赋值
                condition.Code = dataRowCondition["CODE"].ToString();
                condition.Description = dataRowCondition["DESCRIPTION"].ToString();
                condition.TableName = dataRowCondition["TABLENAME"].ToString();
                condition.ColumnName = dataRowCondition["COLUMNNAME"].ToString();
                condition.ColumnValue = dataRowCondition["COLUMNVALUE"].ToString();
                condition.TimeColumnName = dataRowCondition["TimeColumnName"].ToString();
                condition.TimeRange = 0;
                if (int.TryParse(dataRowCondition["TIMERANGE"].ToString(), out result))
                {
                    condition.TimeRange = result;
                }
                condition.PatNoColumnName = dataRowCondition["PATNOCOLUMNNAME"].ToString();
                condition.DBLink = dataRowCondition["DBLINK"].ToString();
                condition.Memo = dataRowCondition["MEMO"].ToString();
                condition.Valide = dataRowCondition["VALID"].ToString();
                #endregion

                return condition;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取所有时限规则条件
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllQCConditions()
        {
            try
            {
                DataTable dtQCCondition = DS_SqlHelper.ExecuteDataTable(c_SqlQCCondition, CommandType.Text);
                return dtQCCondition;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取得所有有效时限条件
        /// xlb 2013-01-06
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllQcCondition2(QCCondition _qcCondition)
        {
            try
            {
                if (_qcCondition == null)
                {
                    return null;
                }
                if (_qcCondition.Code == null)
                {
                    _qcCondition.Code = "";
                }
                if (_qcCondition.Description == null)
                {
                    _qcCondition.Description = "";
                }
                SqlParameter[] sps ={
                                     new SqlParameter("@code",_qcCondition.Code),
                                     new SqlParameter("@description",_qcCondition.Description)
                                   };
                DataTable dtQCCondition = DS_SqlHelper.ExecuteDataTable(c_SqlQcCondition2, sps, CommandType.Text);
                return dtQCCondition;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 通过QCCondition表中的配置数据获取相应表的结果集
        /// <summary>
        /// 获得配置数据的结果集
        /// </summary>
        /// <returns></returns>
        public static DataTable GetConfigDataResult(QCCondition condition)
        {
            try
            {
                DS_SqlHelper.CreateSqlHelperByDBNameFormDA(condition.DBLink);
                DataTable dtConfigResult = DS_SqlHelper.ExecuteDataTable(GetSqlByQCCondition(condition), CommandType.Text);
                DS_SqlHelper.CreateSqlHelperByDBNameFormDA("EMRDB");

                CheckConfigResult(dtConfigResult, condition.PatNoColumnName);

                #region【特殊处理】如果连接的不是电子病历的数据库，需要通过病人流水号得到电子病历中的首页序号inpatient.noofinpat
                if (condition.DBLink != c_EMRDB)
                {
                    //通过病人流水号获得病人首页序号
                    List<string> patNoList = new List<string>();
                    string patNos = string.Empty;
                    foreach (DataRow dr in dtConfigResult.Rows)
                    {
                        string patNo = dr[condition.PatNoColumnName].ToString();
                        if (!patNoList.Contains(patNo))
                        {
                            patNoList.Add(patNo);
                            patNos += "'" + patNo + "',";
                        }
                    }
                    patNos = patNos.Trim(',');
                    DataTable dtInpatient = DS_SqlHelper.ExecuteDataTable(string.Format(c_SqlInpatient, patNos), CommandType.Text);

                    //把病人流水号替换为病人首页序号
                    foreach (DataRow dr in dtConfigResult.Rows)
                    {
                        string patNo = dr[condition.PatNoColumnName].ToString();
                        DataRow drPat = (from DataRow drInpatient in dtInpatient.Rows
                                         where drInpatient["patnoofhis"].ToString() == patNo
                                         select drInpatient).FirstOrDefault();
                        if (drPat != null)
                        {
                            dr[condition.PatNoColumnName] = drPat["noofinpat"].ToString();
                        }
                    }
                }
                #endregion

                return dtConfigResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 检查通过配置得到的数据集中是否包含指定的字段
        /// </summary>
        /// <param name="dtConfigResult"></param>
        /// <param name="condition"></param>
        static void CheckConfigResult(DataTable dtConfigResult, string columnName)
        {
            try
            {
                int columnCount = (from DataColumn column in dtConfigResult.Columns
                                   where column.ColumnName.ToUpper() == columnName.ToUpper()
                                   select column).Count();
                if (columnCount == 0)
                {
                    throw new Exception("通过配置得到的数据集中没有包括 " + columnName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 通过条件获取SQL
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        static string GetSqlByQCCondition(QCCondition condition)
        {
            try
            {
                string tableName = condition.TableName.Trim();
                string columnName = condition.ColumnName.Trim();
                string columnValue = condition.ColumnValue.Trim();
                string timeColumnName = condition.TimeColumnName.Trim();
                string timeRange = Convert.ToString(Convert.ToInt32(condition.TimeRange) / 60 / 60 / 24);//秒 -> 天
                string sql = string.Empty;
                if (timeColumnName != "" && timeRange != "" && timeRange != "0")
                {
                    sql = string.Format(" SELECT * FROM {0} WHERE {1} {2} AND {3} >= SYSDATE - {4} ", tableName, columnName, columnValue, timeColumnName, timeRange);
                }
                else
                {
                    sql = string.Format(" SELECT * FROM {0} WHERE {1} {2} ", tableName, columnName, columnValue);
                }
                return sql;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除时限规则条件方法
        /// xlb 2013-01-06
        /// </summary>
        /// <param name="_qcCondition"></param>
        public static void DeleteCondition(QCCondition _qcCondition)
        {
            try
            {
                if (_qcCondition == null || string.IsNullOrEmpty(_qcCondition.Code))
                {
                    throw new Exception("没有删除的数据");
                }
                string _sqlCondtionDelete = "update QCCondition set valid=0 where code=@cCode ";
                SqlParameter[] sps = { new SqlParameter("@cCode", SqlDbType.NVarChar) };
                sps[0].Value = _qcCondition.Code;
                DS_SqlHelper.ExecuteNonQuery(_sqlCondtionDelete, sps, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


    }
}
