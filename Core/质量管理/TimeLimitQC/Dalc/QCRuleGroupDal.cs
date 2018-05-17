using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;

namespace DrectSoft.Core.TimeLimitQC
{
    /// <summary>
    /// 规则分类数据访问类
    /// </summary>
    public class QCRuleGroupDal
    {
        IDataAccess _sqlHelper;

        const string SQL_SelectAllRuleGroups =
            " select Code,Description,Py,Wb,Valid, Memo "
            + " from RuleCategory "
            + " where Valid=1";

        const string SQL_SelectRuleGroupsById =
            " select Code,Description,Py,Wb,Valid, Memo "
            + " from RuleCategory "
            + " where Code=@Code and Valid=1";

        const string col_fldm = "Code";
        const string col_flms = "Description";
        const string col_py = "Py";
        const string col_wb = "Wb";
        const string col_yxjl = "Valid";
        const string col_memo = "Memo";
        const string param_fldm = "Code";
        const string param_flms = "Description";
        const string param_py = "Py";
        const string param_wb = "Wb";
        const string param_yxjl = "Valid";
        const string param_memo = "Memo";

        public QCRuleGroupDal()
        {
            _sqlHelper = DataAccessFactory.GetSqlDataAccess();
        }

        /// <summary>
        /// 取得时限条件数据集
        /// </summary>
        /// <returns></returns>
        public DataSet GetRuleGroupsDataSet()
        {
            return _sqlHelper.ExecuteDataSet(SQL_SelectAllRuleGroups);
        }

        /// <summary>
        /// 取得时限规则分类集合
        /// </summary>
        /// <returns></returns>
        public IList<QCRuleGroup> GetRuleGroupsList()
        {
            IList<QCRuleGroup> groupList = new List<QCRuleGroup>();
            DataSet dsGroups = GetRuleGroupsDataSet();
            if (dsGroups != null && dsGroups.Tables.Count > 0)
            {
                DataTable dtGroups = dsGroups.Tables[0];
                for (int i = 0; i < dtGroups.Rows.Count; i++)
                    groupList.Add(DataRow2QCRuleGroup(dtGroups.Rows[i]));
            }
            return groupList;
        }

        /// <summary>
        /// 取得指定的规则分类
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public QCRuleGroup GetRuleGroupById(string groupId)
        {
            SqlParameter paramGroupId = new SqlParameter(param_fldm, SqlDbType.VarChar, 64);
            paramGroupId.Value = groupId;
            DataTable dtRuleGroup = _sqlHelper.ExecuteDataTable(SQL_SelectRuleGroupsById
                , new SqlParameter[] { paramGroupId });
            if (dtRuleGroup == null || dtRuleGroup.Rows.Count == 0) return null;
            DataRow drRuleGroup = dtRuleGroup.Rows[0];
            return DataRow2QCRuleGroup(drRuleGroup);
        }

        QCRuleGroup DataRow2QCRuleGroup(DataRow dataRow)
        {
            if (dataRow == null) throw new ArgumentNullException("dataRow");
            string groupId = dataRow[col_fldm].ToString();
            string groupDescript = dataRow[col_flms].ToString();
            QCRuleGroup qcrg = new QCRuleGroup(groupId, groupDescript);
            return qcrg;
        }
    }
}
