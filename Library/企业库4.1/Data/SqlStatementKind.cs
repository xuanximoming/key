using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    /// <summary>
    /// 标记DbCommand中语句的类型
    /// </summary>
    public enum SqlStatementKind
    {
        /// <summary>
        /// 执行插入操作的Command
        /// </summary>
        Insert,
        /// <summary>
        /// 执行更新操作的Command
        /// </summary>
        Update,
        /// <summary>
        /// 执行删除操作的Command
        /// </summary>
        Delete
    }
}
