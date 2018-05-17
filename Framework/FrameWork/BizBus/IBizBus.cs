using System;
using System.Collections.Generic;
using System.Text;
using DrectSoft.FrameWork.BizBus.Service;

namespace DrectSoft.FrameWork.BizBus {
    /// <summary>
    /// 业务总线接口
    /// </summary>
    public interface IBizBus {
        /// <summary>
        /// 构建对象，按照服务关键字构建
        /// </summary>
        /// <typeparam name="T">构建类型</typeparam>
        /// <param name="servicekey">关键字</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        T BuildUp<T>(string servicekey, params object[] parameters);

        ///// <summary>
        ///// 构建对象
        ///// </summary>
        ///// <typeparam name="T">构建类型</typeparam>
        ///// <param name="type">实现类型</param>
        ///// <returns></returns>
        //T BuildUp<T>(Type impletype, params object[] parameters);

        /// <summary>
        /// 构建对象,默认构建缺省对象
        /// </summary>
        /// <typeparam name="T">构建类型</typeparam>
        /// <returns></returns>
        T BuildUp<T>(params object[] parameters);

        /// <summary>
        /// 构建对象
        /// </summary>
        /// <typeparam name="T">构建类型</typeparam>
        /// <param name="servicekey">关键字</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        T BuildUpAndSaveObject<T>(string servicekey, params object[] parameters);

        ///// <summary>
        ///// 构建对象
        ///// </summary>
        ///// <typeparam name="T">构建类型</typeparam>
        ///// <param name="type">实现类型</param>
        ///// <returns></returns>
        //T BuildUpAndSaveObject<T>(Type impletype,  params object[] parameters);

        /// <summary>
        /// 构建对象,默认构建缺省对象
        /// </summary>
        /// <typeparam name="T">构建类型</typeparam>
        /// <returns></returns>
        T BuildUpAndSaveObject<T>(params object[] parameters);

        /// <summary>
        /// 定位类型(指定业务对象)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="servicekey"></param>
        /// <returns></returns>
        T Locate<T>(string servicekey);

        /// <summary>
        /// 定位类型(默认业务对象)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Locate<T>();

        /// <summary>
        /// 缓存服务对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        void SaveObject<T>(string key, T obj);

        /// <summary>
        /// 更新缓存服务对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        void UpdateObject<T>(string key, T obj);
    }
}