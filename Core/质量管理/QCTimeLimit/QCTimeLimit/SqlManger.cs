using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YiDanSqlHelper;
using System.Data;
using YidanSoft.Emr.QCTimeLimit.QCEntity;

namespace YidanSoft.Emr.QCTimeLimit
{
  public  class SqlManger
    {
      const string ruleSql = "select q.RULECODE,q.DESCRIPTION,q.SORTCODE,rc.DESCRIPTION as NAME from QCRULE q join RULECATEGORY rc on q.SORTCODE=rc.CODE where q.valid='1'";

      public SqlManger()
      {
          YD_SqlHelper.CreateSqlHelper();
      }

      /// <summary>
      /// 取得时限规则数据集
      /// by xlb 2013-01-05
      /// </summary>
      /// <returns></returns>
      public DataTable GetAllDataRule()
      {
          DataTable dt = YD_SqlHelper.ExecuteDataTable(ruleSql, CommandType.Text);
          return dt;
      
      }
      public IList<QCRule> GetRuleList(IList<QCCondition> allconditions, IList<QCRecord> allRecords)
      {
          try
          {
              IList<QCRule> resultList = new List<QCRule>();
              DataTable dtRuleList = GetAllDataRule();
              if (dtRuleList == null || dtRuleList.Rows.Count <= 0)
              {
                  return null;
              }
              for (int i = 0; i < dtRuleList.Rows.Count; i++)
              { 
              
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }
      public IList<>
    }
}
