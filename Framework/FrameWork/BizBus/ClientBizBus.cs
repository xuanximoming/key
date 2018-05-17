using DrectSoft.FrameWork.BizBus.Service;
using DrectSoft.FrameWork.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder;

namespace DrectSoft.FrameWork.BizBus
{
    /// <summary>
    /// 业务总线，负责管理所有的
    /// </summary>
    public class ClientBizBus : IBizBus
    {
        private DrectSoft.FrameWork.ObjectBuilder.Builder builder;             //构建器
        internal ServicePackageList packages = new ServicePackageList();      //服务包
        private DrectLocator locator;                                      //定位器

        //Helper
        private PolicyListBuilder policylistbuilder = new PolicyListBuilder();
        private PackageListSearcher packagesearcher = new PackageListSearcher();

        /// <summary>
        /// 构造函数
        /// </summary>
        internal ClientBizBus()
        {
            InitializeBuilder();
        }

        private void InitializeBuilder()
        {
            builder = new DrectSoft.FrameWork.ObjectBuilder.Builder();
            locator = new DrectLocator();
            LifetimeContainer lifetime = new LifetimeContainer();
            locator.Add(typeof(ILifetimeContainer), lifetime);
        }

        #region IBizBus Members

        /// <summary>
        /// 创建类型T，根据服务关键字创建
        /// </summary>
        /// <typeparam name="T">需创建的类型</typeparam>
        /// <param name="key">服务关键字</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public T BuildUp<T>(string key, params object[] parameters)
        {
            ServiceKey servicekey = new ServiceKey(typeof(T), key);
            ServiceDesc desc = this.packagesearcher.SearchPackage(servicekey);
            return InternalBuildUp<T>(desc, false, parameters);
        }

        /// <summary>
        /// 通过缺省服务实现，如无缺省服务，返回空
        /// </summary>
        /// <typeparam name="T">创建的类型</typeparam>
        /// <returns></returns>
        public T BuildUp<T>(params object[] parameters)
        {
            ServiceDesc desc = this.packagesearcher.SearchDefault(typeof(T));
            if (desc == null)
                return default(T);
            return InternalBuildUp<T>(desc, false, parameters);
        }

        /// <summary>
        /// 构建服务，保存对象在总线中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T BuildUpAndSaveObject<T>(string key, params object[] parameters)
        {
            ServiceKey servicekey = new ServiceKey(typeof(T), key);
            ServiceDesc desc = this.packagesearcher.SearchPackage(servicekey);
            return InternalBuildUp<T>(desc, true, parameters);
        }

        /// <summary>
        /// 构建服务，保存对象在总线中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T BuildUpAndSaveObject<T>(params object[] parameters)
        {
            ServiceDesc desc = this.packagesearcher.SearchDefault(typeof(T));
            if (desc == null)
                return default(T);
            return InternalBuildUp<T>(desc, true, parameters);
        }

        /// <summary>
        /// 定位对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Locate<T>(string id)
        {
            DependencyResolutionLocatorKey locatorkey = new DependencyResolutionLocatorKey(typeof(T), id);
            return locator.Get<T>(locatorkey);
        }

        /// <summary>
        /// 定位对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Locate<T>()
        {
            ServiceDesc desc = packagesearcher.SearchDefault(typeof(T));
            if (desc == null)
                return default(T);
            ServiceKey key = new ServiceKey(desc);
            //构造定位器关键字
            DependencyResolutionLocatorKey locatorkey = new DependencyResolutionLocatorKey(desc.ImpleType, key.ID);
            if (this.locator.Contains(locatorkey))
                return this.locator.Get<T>(locatorkey);
            return default(T);
        }

        private T InternalBuildUp<T>(ServiceDesc desc, bool issave, params object[] parameters)
        {
            PolicyList[] pls = policylistbuilder.BuildPolicy(desc, issave, parameters);
            if (issave)
                return builder.BuildUp<T>(locator, desc.Key, null, pls);
            else
                return builder.BuildUp<T>(null, desc.Key, null, pls);
        }

        /// <summary>
        /// 对服务包列表添加索引
        /// </summary>
        internal void IndexPackage()
        {
            packagesearcher.IndexPackageList(this.packages);
        }

        /// <summary>
        /// /保存指定业务对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void SaveObject<T>(string key, T obj)
        {
            DependencyResolutionLocatorKey dkey = new DependencyResolutionLocatorKey(typeof(T), key);
            locator.Add(dkey, obj);
        }

        /// <summary>
        /// 加载指定业务对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T LoadObject<T>(string key)
        {
            DependencyResolutionLocatorKey dkey = new DependencyResolutionLocatorKey(typeof(T), key);
            return locator.Get<T>(dkey);
        }

        /// <summary>
        /// 更新业务对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void UpdateObject<T>(string key, T obj)
        {
            DependencyResolutionLocatorKey dkey = new DependencyResolutionLocatorKey(typeof(T), key);
            if (locator.Contains(dkey)) locator.Remove(dkey);
            locator.Add(dkey, obj);
        }

        #endregion
    }
}