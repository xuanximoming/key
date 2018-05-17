using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace DrectSoft.Core.TimeLimitQC
{
    /// <summary>
    /// 时限控制对象类型
    /// </summary>
    public enum QcObjType
    { 
        /// <summary>
        /// 空
        /// </summary>
        None = 0,

        /// <summary>
        /// 时限控制规则条件
        /// </summary>
        Condition = 1,

        /// <summary>
        /// 时限控制规则操作
        /// </summary>
        Result = 2,
    }

    /// <summary>
    /// 时限控制对象
    /// </summary>
    public abstract class QcObject
    {
        string _id;
        string _name;
        bool _isNew;
        string _judgeSetting;
        string _timeSetting;
        QcObjType _objType;

        protected QcObject(string id, string name, bool isNew)
        {
            _id = id;
            _name = name;
            _isNew = isNew;
        }

        #region properties

        /// <summary>
        /// 时限对象类型
        /// </summary>
        [Browsable(false)]
        public QcObjType ObjType
        {
            get { return _objType; }
            set { _objType = value; }
        }

        /// <summary>
        /// 时限规则条件Id
        /// </summary>
        [DisplayName("代码")]
        public string Id
        {
            get { return _id; }
            internal set { _id = value; }
        }

        /// <summary>
        /// 时限规则条件名称
        /// </summary>
        [DisplayName("名称")]
        public string Name
        {
            get { return _name; }
            internal set { _name = value; }
        }

        /// <summary>
        /// 时限规则条件设置
        /// </summary>
        [DisplayName("时限设置")]
        public string JudgeSetting
        {
            get { return _judgeSetting; }
            set { _judgeSetting = value; }
        }

        /// <summary>
        /// 时间设置(暂时不用)
        /// </summary>
        [Browsable(false)]
        public string TimeSetting
        {
            get { return _timeSetting; }
            set { _timeSetting = value; }
        }

        /// <summary>
        /// 是否新建
        /// </summary>
        [Browsable(false)]
        public bool IsNew
        {
            get { return _isNew; }
        }

        #endregion

        public abstract void SetQcObjInnerKind(object innerKind);

        public abstract object GetQcObjInnerKind();

        public virtual QcObject Clone()
        {
            QcObject qco = QcObjectFactory.CreateQcObjectByType(_objType, _id, _name);
            if (qco != null)
            {
                qco.JudgeSetting = _judgeSetting;
                qco.TimeSetting = _timeSetting;
            }
            return qco;
        }
    }

    /// <summary>
    /// 时限控制对象工厂
    /// </summary>
    public static class QcObjectFactory
    {
        /// <summary>
        /// 构造时限控制对象
        /// </summary>
        /// <param name="otype"></param>
        /// <returns></returns>
        public static QcObject CreateQcObjectByType(QcObjType otype)
        {
            switch (otype)
            {
                case QcObjType.Condition:
                    return new QCCondition();
                case QcObjType.Result:
                    return new QCResult();
                default:
                    return null;
            }
        }

        /// <summary>
        /// 构造时限控制对象,指定代码名称
        /// </summary>
        /// <param name="otype"></param>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static QcObject CreateQcObjectByType(QcObjType otype, string id, string name)
        {
            switch (otype)
            {
                case QcObjType.Condition:
                    return new QCCondition(id, name);
                case QcObjType.Result:
                    return new QCResult(id,name);
                default:
                    return null;
            }
        }
    }
}
