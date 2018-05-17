using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Common
{
    /// <summary>
    /// 编辑状态
    /// <auth>Yanqiao.Cai</auth>
    /// <date>2011-11-08</date>
    /// </summary>
    public enum EditState
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 1,
        /// <summary>
        /// 新增
        /// </summary>
        Add = 2,
        /// <summary>
        /// 编辑
        /// </summary>
        Edit = 4,
        /// <summary>
        /// 删除
        /// </summary>
        Delete = 6,
        /// <summary>
        /// 视图
        /// </summary>
        View = 8
    }

    /// <summary>
    /// 病历权限状态
    /// <auth>Yanqiao.Cai</auth>
    /// <date>2011-12-06</date>
    /// </summary>
    public enum FloderState
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default = 0,

        /// <summary>
        /// 所有权限
        /// </summary>
        All = 1,

        /// <summary>
        /// 医生权限
        /// </summary>
        Doctor = 2,

        /// <summary>
        /// 护士权限
        /// </summary>
        Nurse = 3,

        /// <summary>
        /// 病案首页权限
        /// </summary>
        FirstPage = 4,

        /// <summary>
        /// 非三级检诊权限
        /// </summary>
        NoneAudit = 5,

        /// <summary>
        /// 无权限
        /// </summary>
        None = 10
    }

    /// <summary>
    /// 病历模型类别
    /// </summary>
    public enum EmrModelType
    {
        /// <summary>
        /// 病历容器
        /// </summary>
        EmrModelContainer = 1,
        /// <summary>
        /// 科室容器
        /// </summary>
        EmrModelDeptContainer = 2,
        /// <summary>
        /// 病历
        /// </summary>
        EmrModel = 3
    }


}
