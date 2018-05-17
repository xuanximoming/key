using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrectSoft.Common.Eop;
using DrectSoft.Core;

namespace DrectSoft.Common
{
    /// <summary>
    /// <title>全局公共对象 保存一些全局的属性或对象 后期可逐渐替代IEmrHost 接口中某些对象和熟悉</title>
    /// <auth>tianjie</auth>
    /// <date>2012-09-24</date>
    /// </summary>
    public abstract class CommonObjects
    {
        private static Inpatient _currentPatient;

        /// <summary>
        /// 当前选择的病人
        /// </summary>
        public static Inpatient CurrentPatient
        {
            get { return _currentPatient; }
            set { _currentPatient = value; }
        }

        private static IUser _currentUser;

        /// <summary>
        /// 当前登录用户
        /// </summary>
        public static IUser CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; }
        }

        private static IDataAccess _sqlHelper;
        /// <summary>
        /// 数据库查询操作类
        /// </summary>
        public static IDataAccess SqlHelper
        {
            get { return CommonObjects._sqlHelper; }
            set { CommonObjects._sqlHelper = value; }
        }

        private static bool _IsNeedVerifyInConsultation = true;
        /// <summary>
        /// 是否需要在会诊模块中启用审核流程
        /// </summary>
        public static bool IsNeedVerifyInConsultation
        {
            get { return _IsNeedVerifyInConsultation; }
            set { _IsNeedVerifyInConsultation = value; }
        }

        //临时数据 供乐辰专用，使用后删除 edit by tj 2012-11-6
        public static string m_PID = string.Empty; //存放列表中所选病人的身份证号
        //--------------------------

    }
}
