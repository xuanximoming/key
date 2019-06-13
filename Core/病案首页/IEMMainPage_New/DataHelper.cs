using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using YidanSoft.Wordbook;
using YidanSoft.Common.Library;

using YidanSoft.FrameWork.WinForm.Plugin;

namespace YidanSoft.Core.IEMMainPage
{
    class DataHelper
    {
        private IDataAccess m_SqlHelper = DataAccessFactory.DefaultDataAccess;

        private string m_GetHospitalInfo = "SELECT h.ID, h.Name FROM HospitalInfo h";

        public string GetHospitalName()
        {
            DataTable dt = m_SqlHelper.ExecuteDataTable(m_GetHospitalInfo, CommandType.Text);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["name"].ToString();
            }
            return "";
        }
    
    }
}
