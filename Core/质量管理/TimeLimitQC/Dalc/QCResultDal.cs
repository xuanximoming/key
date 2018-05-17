using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using DrectSoft.Core.TimeLimitQC;

namespace DrectSoft.Core.TimeLimitQC
{
    /// <summary>
    /// 时限控制规则结果数据访问类
    /// </summary>
    public class QCResultDal : IQCDataDal
    {
        #region fields

        IDataAccess _sqlHelper;

        const string SQL_SelectAllResults =
            " select Code, Category, Description, Result, Time, Valid, Memo,QCCode "
            + " from QCResult "
            + " where Valid=1";
        const string SQL_SelectResultsByKind =
            " select Code, Category, Description, Result, Time, Valid, Memo,QCCode "
            + " from QCResult "
            + " where Category=@Category and Valid=1 ";
        const string SQL_SelectResultById =
            " select Code, Category, Description, Result, Time, Valid, Memo,QCCode "
            + " from QCResult "
            + " where Code=@Code and Valid=1 ";
        const string SQL_InsertResult =
            " insert into QCResult (Code, Category, Description, Result, Time, Valid, Memo,QCCode) "
            + " values(@Code, @Category, @Description, @Result, @Time, @Valid, @Memo,@QCCode) ";
        const string SQL_DeleteResult =
            " delete from QCResult where tjdm=@tjdm ";
        const string SQL_UpdateResult =
            " update QCResult set Category=@Category, Description=@Description "
            + " , Result=@Result, Time=@Time, Valid=@Valid, Memo=@Memo,QCCode=@QCCode "
            + " where Code=@Code ";
        const string tab_zlkzjgk = "QCResult";
        const string col_Code = "Code";
        const string col_Category = "Category";
        const string col_Description = "Description";
        const string col_Result = "Result";
        const string col_Time = "Time";
        const string col_Valid = "Valid";
        const string col_Memo = "Memo";
        const string col_QCCode = "QCCode";
        const string param_Code = "Code";
        const string param_Category = "Category";
        const string param_Description = "Description";
        const string param_Result = "Result";
        const string param_Time = "Time";
        const string param_Valid = "Valid";
        const string param_Memo = "Memo";
        const string param_QCCode = "QCCode";

        #endregion

        #region ctor

        /// <summary>
        /// 构造
        /// </summary>
        public QCResultDal()
        {
            _sqlHelper = DataAccessFactory.GetSqlDataAccess();
        }

        #endregion

        #region dalc procedure

        /// <summary>
        /// 取得时限结果数据集
        /// </summary>
        /// <returns></returns>
        public DataSet GetResultsDataSet()
        {
            return _sqlHelper.ExecuteDataSet(SQL_SelectAllResults);
        }

        /// <summary>
        /// 取得时限结果集合
        /// </summary>
        /// <returns></returns>
        public IList<QCResult> GetResultsList()
        {
            IList<QCResult> resultList = new List<QCResult>();
            DataSet dsResults = GetResultsDataSet();
            if (dsResults != null && dsResults.Tables.Count > 0)
            {
                DataTable dtResults = dsResults.Tables[0];
                for (int i = 0; i < dtResults.Rows.Count; i++)
                    resultList.Add(DataRow2QCResult(dtResults.Rows[i]));
            }
            return resultList;
        }

        /// <summary>
        /// 数据行 -> 时限结果
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public QCResult DataRow2QCResult(DataRow row)
        {
            if (row == null) throw new ArgumentNullException("row");
            string resultId = row[col_Code].ToString();
            string resultDescription = row[col_Description].ToString();
            QCResult qcr = new QCResult(resultId, resultDescription);
            qcr.JudgeSetting = row[col_Result].ToString();
            qcr.TimeSetting = row[col_Time].ToString();
            if (row[col_Category] == DBNull.Value)
                qcr.ResultType = QCResultType.None;
            else
                qcr.ResultType = (QCResultType)(int.Parse(row[col_Category].ToString()) - ConstRes.cstResultTypeNo);
            qcr.QCCode = row.IsNull(col_QCCode) ? string.Empty : row[col_QCCode].ToString();
            return qcr;
        }

        /// <summary>
        /// 取得指定的规则结果
        /// </summary>
        /// <returns></returns>
        public QCResult GetResultById(string resultId)
        {
            SqlParameter paramResultId = new SqlParameter(param_Code, SqlDbType.VarChar, 64);
            paramResultId.Value = resultId;
            DataTable dtResult = _sqlHelper.ExecuteDataTable(SQL_SelectResultById
                , new SqlParameter[] { paramResultId });
            if (dtResult == null || dtResult.Rows.Count == 0) return null;
            DataRow drResult = dtResult.Rows[0];
            return DataRow2QCResult(drResult);
        }

        /// <summary>
        /// 保存指定规则结果
        /// </summary>
        /// <param name="result"></param>
        public void SaveResult(QCResult result)
        {
            if (result == null) throw new ArgumentNullException("result");
            SqlParameter[] sqlparams = InitQCResultParams(result);
            if (result.IsNew)
                _sqlHelper.ExecuteNoneQuery(SQL_InsertResult, sqlparams);
            else
                _sqlHelper.ExecuteNoneQuery(SQL_UpdateResult, sqlparams);
        }

        SqlParameter[] InitQCResultParams(QCResult result)
        {
            SqlParameter paramJgdm = new SqlParameter(param_Code, SqlDbType.VarChar, 64);
            SqlParameter paramJgfl = new SqlParameter(param_Category, SqlDbType.Int);
            SqlParameter paramJgms = new SqlParameter(param_Description, SqlDbType.VarChar, 64);
            SqlParameter paramJgsz = new SqlParameter(param_Result, SqlDbType.VarChar, 1024);
            SqlParameter paramSjsz = new SqlParameter(param_Time, SqlDbType.VarChar, 1024);
            SqlParameter paramYxjl = new SqlParameter(param_Valid, SqlDbType.Int);
            SqlParameter paramMemo = new SqlParameter(param_Memo, SqlDbType.VarChar, 64);
            SqlParameter paramQCCode = new SqlParameter(param_QCCode,SqlDbType.VarChar,64);

            paramJgdm.Value = result.Id;
            paramJgfl.Value = (int)result.ResultType + ConstRes.cstResultTypeNo;
            paramJgms.Value = result.Name;
            paramJgsz.Value = result.JudgeSetting;
            paramSjsz.Value = result.TimeSetting;
            paramYxjl.Value = 1;
            paramMemo.Value = string.Empty;
            paramQCCode.Value = result.QCCode;

            return new SqlParameter[]{
                paramJgdm, paramJgfl, paramJgms, paramJgsz, paramSjsz, paramYxjl, paramMemo,paramQCCode
            };
        }
        #endregion


        #region IQCDataDal Members

        public DataSet GetDataSet()
        {
            return GetResultsDataSet();
        }

        public QcObject DataRow2QcObj(DataRow row)
        {
            return DataRow2QCResult(row);
        }

        public void SaveRecord(QcObject o)
        {
            if (o is QCResult) SaveResult(o as QCResult);
            else
            {
                System.Diagnostics.Trace.WriteLine("试图用时限操作数据访问类保存非时限操作对象");
            }
        }

        public void DeleteRecord(QcObject o)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
