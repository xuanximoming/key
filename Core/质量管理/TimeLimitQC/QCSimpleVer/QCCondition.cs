using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Xml;
using System.IO;

using System.Reflection;
using System.ComponentModel;

namespace DrectSoft.Core.TimeLimitQC
{
    /// <summary>
    /// 规则前提条件
    /// </summary>
    public class QCCondition:QcObject
    {
        #region fields

        QCConditionType _conditionType;
        static IList<QCCondition> _allConditions = new List<QCCondition>();

        #endregion

        #region properties

        public static QCConditionDal Dal = new QCConditionDal();

        [DisplayName("条件分类")]
        public QCConditionType ConditionType
        {
            get { return _conditionType; }
            set { _conditionType = value; }
        }

        #endregion

        #region ctor

        /// <summary>
        /// 构造新时限条件
        /// </summary>
        public QCCondition()
            : this(string.Empty, string.Empty, true)
        {
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="id">条件代码</param>
        /// <param name="name">条件描述</param>
        public QCCondition(string id, string name)
            : this(id, name, false)
        {
        }

        /// <summary>
        /// 构造时限规则条件,指定是否新建
        /// </summary>
        public QCCondition(string id, string name, bool isnew)
            : base(id, name, isnew)
        {
            ObjType = QcObjType.Condition;
        }

        #endregion

        public override void SetQcObjInnerKind(object innerKind)
        {
            if (innerKind is QCConditionType)
            {
                _conditionType = (QCConditionType)innerKind;
            }
        }

        public override object GetQcObjInnerKind()
        {
            return _conditionType;
        }

        /// <summary>
        /// 复制条件
        /// </summary>
        /// <returns></returns>
        public new QCCondition Clone()
        {
            return base.Clone() as QCCondition;
        }

        /// <summary>
        /// 对象字符串显示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// 得到所有的规则条件
        /// </summary>
        /// <returns></returns>
        public static IList<QCCondition> AllConditions
        {
            get
            {
                _allConditions = Dal.GetConditionsList();
                return _allConditions;
            }
        }

        /// <summary>
        /// 判断条件Id是否已经存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IdIsExisted(string id)
        {
            foreach (QCCondition qcc in AllConditions)
            {
                if (qcc.Id == id) return true;
            }
            return false;
        }

        /// <summary>
        /// 得到指定代码的时限规则条件
        /// </summary>
        /// <param name="conditionId"></param>
        /// <returns></returns>
        public static QCCondition SelectQCCondition(string conditionId)
        {
            foreach (QCCondition qcc in _allConditions)
            {
                if (qcc.Id == conditionId) return qcc;
            }
            return null;
        }

        /// <summary>
        /// 通过指定的条件类别和对象定位条件
        /// </summary>
        /// <param name="conditionType"></param>
        /// <param name="conditionObject"></param>
        /// <returns></returns>
        public static QCCondition SelectQCCondition(QCConditionType conditionType, object conditionObject)
        {
            IList<QCCondition> kindConditions = new List<QCCondition>();
            foreach (QCCondition qcc in _allConditions)
            {
                if (qcc._conditionType == conditionType)
                    kindConditions.Add(qcc.Clone());
            }

            string objtype = Qcsv.ConditionType2String(conditionType);
            if (string.IsNullOrEmpty(objtype))
            {
                foreach (QCCondition qcc in kindConditions)
                {
                    if (qcc.JudgeSetting == conditionObject.ToString())
                        return qcc;
                }
            }
            else
            {
                foreach (QCCondition qcc in kindConditions)
                {
                    QCParams qcp = new QCParams(qcc.JudgeSetting);
                    if (Qcsv.JudgeObjIsEqualProps(Type.GetType(objtype), conditionObject, qcp))
                        return qcc;
                }
            }
            return null;
        }

        /// <summary>
        /// 保存时限规则条件
        /// </summary>
        /// <param name="condition"></param>
        public static void SaveQCCondition(QCCondition condition)
        {
            Dal.SaveCondition(condition);
        }
    }
}
