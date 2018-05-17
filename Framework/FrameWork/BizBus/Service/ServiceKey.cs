using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.FrameWork.BizBus.Service {
    /// <summary>
    /// 服务key，用来唯一标示服务，每一个服务都可以用该key唯一标示
    /// （实现类型使用fullname来作为key
    /// </summary>
    public class ServiceKey {
        private Type type;      //服务类型
        private string id;      //服务id

        /// <summary>
        /// 构造函数
        /// </summary>
        public ServiceKey()
            : this(null, null) {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type">服务类型</param>
        /// <param name="id">服务id</param>
        public ServiceKey(Type type, string id) {
            this.type = type;
            this.id = id;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="desc">服务描述</param>
        public ServiceKey(ServiceDesc desc) {
            type = desc.ServiceType;
            id = desc.Key;
        }

        /// <summary>
        /// 获取服务ID
        /// </summary>
        public string ID {
            get { return id; }
        }

        /// <summary>
        /// 获取服务类型
        /// </summary>
        public Type ServiceType {
            get { return type; }
        }

        /// <summary>
        /// 判等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            ServiceKey other = obj as ServiceKey;

            if (other == null)
                return false;

            return (Equals(type, other.type) && Equals(id, other.id));
        }

        /// <summary>
        /// 哈希值
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            int hashForType = type == null ? 0 : type.GetHashCode();
            int hashForID = id == null ? 0 : id.GetHashCode();
            return hashForType ^ hashForID;
        }
    }
}
