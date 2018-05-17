using System;
using System.Collections.Generic;
using System.Text;
using DrectSoft.Core.TimeLimitQC;

namespace DrectSoft.Core.TimeLimitQC
{
    /// <summary>
    /// 参数比较符
    /// </summary>
    public enum CompareOp
    {
        /// <summary>
        /// 空
        /// </summary>
        None = 0,

        /// <summary>
        /// 相等
        /// </summary>
        Equal = 1,

        /// <summary>
        /// 相似
        /// </summary>
        Like = 2,
    }

    /// <summary>
    /// 时限设置参数
    /// </summary>
    public struct QCParam
    {
        /// <summary>
        /// 属性名称
        /// </summary>
        public string Prop;
        /// <summary>
        /// 比较
        /// </summary>
        public CompareOp Op;
        /// <summary>
        /// 属性比较值
        /// </summary>
        public string PropValue;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="op"></param>
        /// <param name="propValue"></param>
        public QCParam(string prop, CompareOp op, string propValue)
        {
            Prop = prop;
            PropValue = propValue;
				if (!string.IsNullOrEmpty(propValue))
					Op = op;
				else
					Op = CompareOp.None;
        }

        /// <summary>
        /// 比较传入值
        /// </summary>
        /// <param name="cmpValue"></param>
        /// <returns></returns>
        public bool Check(string cmpValue)
        {
            switch (Op)
            {
                case CompareOp.Equal:
                    return PropValue == cmpValue;
                case CompareOp.Like:
                    return cmpValue.StartsWith(PropValue);
                default:
                    return false;
            }
        }
    }

    /// <summary>
    /// 设置的条件或结果参数
    /// </summary>
    public class QCParams
    {
        IDictionary<string, QCParam> _settings = new Dictionary<string, QCParam>();

        /// <summary>
        /// 参数字典
        /// </summary>
        public IDictionary<string, QCParam> Settings
        {
            get { return _settings; }
        }

        /// <summary>
        /// 构造
        /// </summary>
        public QCParams() { }

        /// <summary>
        /// 构造2
        /// </summary>
        /// <param name="paramstring"></param>
        public QCParams(string paramstring)
        {
            if (!string.IsNullOrEmpty(paramstring))
            {
                string[] paramcollection = paramstring.Split(new string[] { ConstRes.cstAndOper }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < paramcollection.Length; i++)
                {
                    QCParam qcp = String2QCParam(paramcollection[i]);
                    if (!string.IsNullOrEmpty(qcp.Prop))
                        _settings.Add(qcp.Prop, qcp);
                }
            }
        }

        /// <summary>
        /// override tostring()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string display = string.Empty;
            foreach (string key in _settings.Keys)
            {
					if (string.IsNullOrEmpty(_settings[key].PropValue))
						continue;
                string oneparam = QCParam2String(_settings[key]);
                display += ConstRes.cstAndOper + oneparam;
            }
            return display;
        }

        /// <summary>
        /// 比较符至字符串(保证可以用来标记分隔)
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        public static string CompareOp2String(CompareOp op)
        {
            return "<" + op.ToString() + ">";
        }

        /// <summary>
        /// 时限设置参数至字符串
        /// </summary>
        /// <param name="qcparam"></param>
        /// <returns></returns>
        public static string QCParam2String(QCParam qcparam)
        {
            return qcparam.Prop + CompareOp2String(qcparam.Op) + qcparam.PropValue;
        }

        /// <summary>
        /// 字符串至时限设置参数
        /// </summary>
        /// <param name="qcpstring"></param>
        /// <returns></returns>
        public static QCParam String2QCParam(string qcpstring)
        {
            string[] oneparam = new string[0];
            CompareOp op = CompareOp.None;
            if (qcpstring.Contains(CompareOp2String(CompareOp.Equal)))
            {
                op = CompareOp.Equal;
                oneparam = qcpstring.Split(new string[] { CompareOp2String(CompareOp.Equal) }, StringSplitOptions.RemoveEmptyEntries);
            }
            if (qcpstring.Contains(CompareOp2String(CompareOp.Like)))
            {
                op = CompareOp.Like;
                oneparam = qcpstring.Split(new string[] { CompareOp2String(CompareOp.Like) }, StringSplitOptions.RemoveEmptyEntries);
            }
            if (oneparam.Length == 2)
                return new QCParam(oneparam[0], op, oneparam[1]);
            else
                return new QCParam();
        }
    }
}
