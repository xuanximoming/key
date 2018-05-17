using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.FrameWork.Plugin {
    /// <summary>
    /// 业务逻辑插件
    /// </summary>
    public class BizPluginInfo : PluginInfo {
        BizPluginType type;
        string bizName;
        bool isCore;

        /// <summary>
        /// 构造函数
        /// </summary>
        public BizPluginInfo() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bizname">业务逻辑插件名称</param>
        /// <param name="type">业务逻辑插件类型</param>
        /// <param name="iscore">是否为核心插件</param>
        public BizPluginInfo(string bizname, BizPluginType type, bool iscore) {
            this.bizName = bizname;
            this.type = type;
        }

        /// <summary>
        /// 设置或获取业务逻辑名称
        /// </summary>
        public string BizName {
            get { return bizName; }
            set { bizName = value; }
        }

        /// <summary>
        /// 设置或获取业务逻辑类型
        /// </summary>
        public BizPluginType BizType {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// 设置或获取是否是核心插件
        /// </summary>
        public bool IsCore {
            get { return isCore; }
            set { isCore = value; }
        }
    }

    /// <summary>
    /// 业务插件类别
    /// </summary>
    public enum BizPluginType {
        /// <summary>
        /// 接口
        /// </summary>
        Interface = 0,
        /// <summary>
        /// 实现
        /// </summary>
        Implement = 1
    }
}
