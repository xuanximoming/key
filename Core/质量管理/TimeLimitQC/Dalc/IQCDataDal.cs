using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DrectSoft.Core.TimeLimitQC
{
    /// <summary>
    /// 统一条件和操作的数据访问方法
    /// </summary>
    internal interface IQCDataDal
    {
        /// <summary>
        /// 返回数据
        /// </summary>
        /// <returns></returns>
        DataSet GetDataSet();

        /// <summary>
        /// 数据行到时限控制对象
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        QcObject DataRow2QcObj(DataRow row);

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="o"></param>
        void SaveRecord(QcObject o);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="o"></param>
        void DeleteRecord(QcObject o);
    }
}
