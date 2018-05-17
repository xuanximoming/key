using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.FrameWork.Config {
    /// <summary>
    /// 服务包信息列表
    /// </summary>
    public class ServicePackageListInfo {
        List<ServicePackageInfo> infos;
        string uri;

        /// <summary>
        /// ctor
        /// </summary>
        public ServicePackageListInfo() {
            infos = new List<ServicePackageInfo>();
        }


        /// <summary>
        /// 服务器报列表(具体信息)
        /// </summary>
        public List<ServicePackageInfo> Infos {
            get { return infos; }
        }

        /// <summary>
        /// 服务访问uri
        /// </summary>
        public string Uri {
            get { return uri; }
            set { uri = value; }
        }
    }
}
