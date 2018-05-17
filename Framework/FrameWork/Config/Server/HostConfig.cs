using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace DrectSoft.FrameWork.Config {
    /// <summary>
    /// 宿主配置
    /// </summary>
    public class HostConfig : IHostConfig {
        #region IHostConfig Members
        /// <summary>
        /// 获取宿主配置
        /// </summary>
        /// <returns></returns>
        public string GetConfigString() {
            XmlDocument doc = new XmlDocument();
            doc.Load("DrectSoft.FrameWork.config");
            return doc.OuterXml;
        }

        #endregion
    }
}
