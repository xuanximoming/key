using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace DrectSoft.FrameWork.BizBus.Service
{
   /// <summary>
   /// 服务包列表
   /// </summary>
   public class ServicePackageList:ICollection<IServicePackage>
   {
      private List<IServicePackage> packages;

      /// <summary>
      /// 构造函数
      /// </summary>
      public ServicePackageList()
      {
         packages = new List<IServicePackage>();
      }

      #region ICollection<IServicePackage> Members

      /// <summary>
      /// 添加服务包
      /// </summary>
      /// <param name="item"></param>
      public void Add(IServicePackage item)
      {
         packages.Add(item);
      }

      /// <summary>
      /// 清除服务包列表
      /// </summary>
      public void Clear()
      {
         packages.Clear();
      }

      /// <summary>
      /// 查询是否包含服务包
      /// </summary>
      /// <param name="item"></param>
      /// <returns></returns>
      public bool Contains(IServicePackage item)
      {
         return packages.Contains(item);
      }

      /// <summary>
      /// 拷贝服务包
      /// </summary>
      /// <param name="array"></param>
      /// <param name="arrayIndex"></param>
      public void CopyTo(IServicePackage[] array, int arrayIndex)
      {
         packages.CopyTo(array, arrayIndex);
      }

      /// <summary>
      /// 获取服务包数量
      /// </summary>
      public int Count
      {
         get { return packages.Count; }
      }

      /// <summary>
      /// 获取列表是否为只读
      /// </summary>
      public bool IsReadOnly
      {
         get { return false; }
      }

      /// <summary>
      /// 移除服务包
      /// </summary>
      /// <param name="item"></param>
      /// <returns></returns>
      public bool Remove(IServicePackage item)
      {
         return packages.Remove(item);
      }

      #endregion

      #region IEnumerable<IServicePackage> Members

       /// <summary>
       /// 
       /// </summary>
       /// <returns></returns>
      public IEnumerator<IServicePackage> GetEnumerator()
      {
         return packages.GetEnumerator();
      }

      #endregion

      #region IEnumerable Members

      System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
      {
         return packages.GetEnumerator();
      }

      #endregion
   }
}
