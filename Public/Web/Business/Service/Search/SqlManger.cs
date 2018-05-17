using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DrectSoft.Core;
using DrectSoft.FrameWork;
using System.Data;


namespace DrectSoft.Emr.Web.Business.Service
{
    public class Search
    {
        IDataAccess SqlHelper = DataAccessFactory.GetSqlDataAccess();
        /// <summary>
        ///药品说明书搜索
        /// </summary>
        /// <returns></returns>
        public DataTable GetMedicaineDirect(String sql)
        {
            DataTable dataTable = null;
            dataTable = SqlHelper.ExecuteDataTable(sql);
            return dataTable;
        }

        /// <summary>
        /// 获取药品
        /// </summary>
        /// <param name="OneName"></param>
        /// <returns></returns>
        public DataTable GetMedicaine(String sql)
        {
            DataTable dataTable = null;
            dataTable = SqlHelper.ExecuteDataTable(sql);//@"select * from Medicine a ");
            return dataTable;
        }

        
    }
}
