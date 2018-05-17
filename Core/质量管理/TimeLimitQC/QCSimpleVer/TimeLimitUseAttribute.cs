using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.Core.TimeLimitQC
{
    /// <summary>
    /// 定义时限设置时可以使用的属性
    /// </summary>
    public class TimeLimitUseAttribute:Attribute
    {
        bool _canuse;
        string _description = string.Empty;

        /// <summary>
        /// CanUse
        /// </summary>
        public bool CanUse
        {
            get { return _canuse; }
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="description">属性描述</param>
        public TimeLimitUseAttribute(string description)
        {
            _canuse = true;
            _description = description;
        }
    }
}
