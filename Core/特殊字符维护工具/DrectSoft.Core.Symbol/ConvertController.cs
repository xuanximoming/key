using System;
using System.Collections.Generic;
using System.Text;

namespace YidanSoft.Core.Symbol
{
    /// <summary>
    /// 对象抽像转换控制器，提供对象抽象转换基本框架
    /// </summary>
    public abstract class ConvertController<S, T>
    {
        /// <summary>
        /// 执行DoConvert，将S类型的origianl转化为类型T
        /// 若origianl为空，返回T类型的默认值
        /// </summary>
        /// <param name="origianl"></param>
        /// <returns>T</returns>
        public T DoConvert(S origianl)
        {
            try
            {
                if (object.Equals(origianl, default(S)))
                    return default(T);
                return DoSTConvert(origianl);
            }
            catch (Exception e) { throw e; }
        }
        /// <summary>
        /// DoConvert核心实际
        /// </summary>
        /// <param name="origianl"></param>
        /// <returns></returns>
        protected abstract T DoSTConvert(S origianl);
    }
}
