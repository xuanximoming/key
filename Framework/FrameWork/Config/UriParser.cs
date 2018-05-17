using System;
using System.Collections.Generic;
using System.Text;
using DrectSoft.FrameWork.BizBus.Service;

namespace DrectSoft.FrameWork.Config
{
    /// <summary>
    /// 服务URI辅助类
    /// </summary>
   public class UriUtil
   {
       /// <summary>
       /// 获取服务URI地址
       /// </summary>
       /// <param name="baseuripath"></param>
       /// <param name="packageinfo"></param>
       /// <param name="desc"></param>
       /// <returns></returns>
      public static string GetServiceUri(string baseuripath, ServicePackageInfo packageinfo, ServiceDesc desc)
      {
         //服务的URI地址＝基础URI路径+服务包名称＋服务关键字
         return baseuripath + packageinfo.Name + "/" + desc.Key;
      }

       /// <summary>
       /// 获取指定服务URI
       /// </summary>
       /// <param name="baseuripath"></param>
       /// <param name="packagename"></param>
       /// <param name="servicekey"></param>
       /// <returns></returns>
      public static string GetServiceUri(string baseuripath, string packagename, string servicekey)
      {
         return baseuripath + packagename +"/"+ servicekey;
      }
   }
}
