using System;
using System.Collections.Generic;
using System.Text;
using DrectSoft.Core;

namespace DrectSoft.JobManager
{
    public interface ISynchApplication
    {
        IDataAccess DefaultSqlAccess { get; }
        IDataAccess GetSqlAccess(string dbName);
        void WriteLog(JobExecuteInfoArgs e);
    }
}
