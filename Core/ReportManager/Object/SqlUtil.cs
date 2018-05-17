using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using YidanSoft.FrameWork.WinForm.Plugin;


namespace YidanSoft.Core.YDReportManager
{
    abstract class SqlUtil
    {
        static YidanSoft.Core.IDataAccess sql_Helper;
        static IYidanEmrHost m_app;
              
        public static IYidanEmrHost App
        {
            get { return m_app; }
            set
            { 
                m_app = value;
                sql_Helper = m_app.SqlHelper;

            }
        }
    }
}
