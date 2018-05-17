using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace DrectSoft.FrameWork.Config
{
    /// <summary>
    /// 异常包裹信息
    /// </summary>
   [DataContract]
   public class ErrPackageInfo
   {
      private List<ErrServiceInfo> errServiceInfos;
      private ErrAssemblyInfo errAssemblyInfo;

       /// <summary>
       /// ctor
       /// </summary>
      public ErrPackageInfo() 
      {
         errServiceInfos = new List<ErrServiceInfo>();
         errAssemblyInfo = new ErrAssemblyInfo();
      }

       /// <summary>
       /// 异常错误列表
       /// </summary>
      public List<ErrServiceInfo> ErrServiceInfoList
      {
         get { return errServiceInfos; }
      }

      /// <summary>
      /// 所属的异常信息
      /// </summary>
      public ErrAssemblyInfo ErrAssemblyInfo
      {
         get { return errAssemblyInfo; }
         set { errAssemblyInfo = value; }
      }
   }
}
