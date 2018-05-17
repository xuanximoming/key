using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DrectSoft.Core.TimeLimitQC;

namespace DrectSoft.Core.TimeLimitQC
{
    /// <summary>
    /// 时限控制规则条件数据访问类
    /// </summary>
    public class QCConditionDal : IQCDataDal
    {
        #region fields

        IDataAccess _sqlHelper;

        const string SQL_SelectAllConditions =
            " select Code, Category, Description, Condition, TimeInstall, Valid, Memo "
            + " from QCCondition "
            + " where Valid=1";
        const string SQL_SelectConditionsByKind =
            " select Code, Category, Description, Condition, TimeInstall, Valid, Memo "
            + " from QCCondition "
            + " where Category=@Category and Valid=1 ";
        const string SQL_SelectConditionById =
            " select Code, Category, Description, Condition, TimeInstall, Valid, Memo "
            + " from QCCondition "
            + " where Code=@Code and Valid=1 ";
        const string SQL_InsertCondition =
            " insert into QCCondition (Code, Category, Description, Condition, TimeInstall, Valid, Memo) "
            + " values(@Code, @Category, @Description, @Condition, @TimeInstall, @Valid, @Memo) ";
        const string SQL_DeleteCondition =
            " delete from QCCondition where tjdm=@tjdm ";
        const string SQL_UpdateCondition =
            " update QCCondition set Category=@Category, Description=@Description "
            + " , Condition=@Condition, TimeInstall=@TimeInstall, Valid=@Valid, Memo=@Memo "
            + " where tjdm=@tjdm ";
        const string tab_zlkztjk = "QCCondition";
        const string col_tjdm = "Code";
        const string col_tjfl = "Category";
        const string col_tjms = "Description";
        const string col_tjsz = "Condition";
        const string col_sjsz = "TimeInstall";
        const string col_yxjl = "Valid";
        const string col_memo = "Memo";
        const string param_tjdm = "Code";
        const string param_tjfl = "Category";
        const string param_tjms = "Description";
        const string param_tjsz = "Condition";
        const string param_sjsz = "TimeInstall";
        const string param_yxjl = "Valid";
        const string param_memo = "Memo";

        #endregion

        #region ctor

        /// <summary>
        /// 构造
        /// </summary>
        public QCConditionDal()
        {
            _sqlHelper = DataAccessFactory.GetSqlDataAccess();
        }

        #endregion

        #region dalc procedure

        /// <summary>
        /// 取得时限条件数据集
        /// </summary>
        /// <returns></returns>
        public DataSet GetConditionsDataSet()
        {
            return _sqlHelper.ExecuteDataSet(SQL_SelectAllConditions);
        }

        /// <summary>
        /// 取得时限条件集合
        /// </summary>
        /// <returns></returns>
        public IList<QCCondition> GetConditionsList()
        {
            IList<QCCondition> condList = new List<QCCondition>();
            DataSet dsConditions = GetConditionsDataSet();
            if (dsConditions != null && dsConditions.Tables.Count > 0)
            {
                DataTable dtConditions = dsConditions.Tables[0];
                for (int i = 0; i < dtConditions.Rows.Count; i++)
                    condList.Add(DataRow2QCCondition(dtConditions.Rows[i]));
            }
            return condList;
        }

        /// <summary>
        /// 数据行 -> 时限条件
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public QCCondition DataRow2QCCondition(DataRow row)
        {
            if (row == null) throw new ArgumentNullException("row");
            string condId = row[col_tjdm].ToString();
            string condDescription = row[col_tjms].ToString();
            QCCondition qcc = new QCCondition(condId, condDescription);
            qcc.JudgeSetting = row[col_tjsz].ToString();
            qcc.TimeSetting = row[col_sjsz].ToString();
            if (row[col_tjfl] == DBNull.Value)
                qcc.ConditionType = QCConditionType.None;
            else
                qcc.ConditionType = (QCConditionType)(int.Parse(row[col_tjfl].ToString()) - ConstRes.cstCondTypeNo);
            return qcc;
        }

        /// <summary>
        /// 取得指定的规则条件
        /// </summary>
        /// <returns></returns>
        public QCCondition GetConditionById(string condId)
        {
            SqlParameter paramConditionId = new SqlParameter(param_tjdm, SqlDbType.VarChar, 64);
            paramConditionId.Value = condId;
            DataTable dtCondition = _sqlHelper.ExecuteDataTable(SQL_SelectConditionById
                , new SqlParameter[] { paramConditionId });
            if (dtCondition == null || dtCondition.Rows.Count == 0) return null;
            DataRow drCondition = dtCondition.Rows[0];
            return DataRow2QCCondition(drCondition);
        }

        /// <summary>
        /// 保存时限规则条件到数据库
        /// </summary>
        /// <param name="condition"></param>
        public void SaveCondition(QCCondition condition)
        {
            if (condition == null) throw new ArgumentNullException("condition");
            SqlParameter[] sqlparams = InitQCConditionParams(condition);
            if (condition.IsNew)
                _sqlHelper.ExecuteNoneQuery(SQL_InsertCondition, sqlparams);
            else
                _sqlHelper.ExecuteNoneQuery(SQL_UpdateCondition, sqlparams);
        }

        SqlParameter[] InitQCConditionParams(QCCondition condition)
        {
            SqlParameter paramTjdm = new SqlParameter(param_tjdm, SqlDbType.VarChar, 64);
            SqlParameter paramTjfl = new SqlParameter(param_tjfl, SqlDbType.Int);
            SqlParameter paramTjms = new SqlParameter(param_tjms, SqlDbType.VarChar, 64);
            SqlParameter paramTjsz = new SqlParameter(param_tjsz, SqlDbType.VarChar, 1024);
            SqlParameter paramSjsz = new SqlParameter(param_sjsz, SqlDbType.VarChar, 1024);
            SqlParameter paramYxjl = new SqlParameter(param_yxjl, SqlDbType.Int);
            SqlParameter paramMemo = new SqlParameter(param_memo, SqlDbType.VarChar, 64);

            paramTjdm.Value = condition.Id;
            paramTjfl.Value = (int)condition.ConditionType + ConstRes.cstCondTypeNo;
            paramTjms.Value = condition.Name;
            paramTjsz.Value = condition.JudgeSetting;
            paramSjsz.Value = condition.TimeSetting;
            paramYxjl.Value = 1;
            paramMemo.Value = string.Empty;
            return new SqlParameter[]{
                paramTjdm, paramTjfl, paramTjms, paramTjsz, paramSjsz, paramYxjl, paramMemo
            };
        }

        #endregion

        #region IQCDataDal Members

        public DataSet GetDataSet()
        {
            return GetConditionsDataSet();
        }

        public QcObject DataRow2QcObj(DataRow row)
        {
            return DataRow2QCCondition(row);
        }

        public void SaveRecord(QcObject o)
        {
            if (o is QCCondition) SaveCondition(o as QCCondition);
            else
            {
                System.Diagnostics.Trace.WriteLine("试图用时限条件数据访问类保存非时限条件对象");
            }
        }

        public void DeleteRecord(QcObject o)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
