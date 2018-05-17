using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Common
{
    /// <summary>
    /// 功能描述：定义字典表中的ID
    /// 创 建 者：wyt
    /// 创建日期：2012-11-13
    /// </summary>
    public class CommonDict
    {
        public CommonDict()
        { 
        }

        #region 科室类别 对应CATEGORYDETAIL表中CATEGORYID(1)
        //add by wyt 2012-11-13
        /// <summary>
        /// 科室类别：临床
        /// </summary>
        public const int DEPTSORT_LINCHUANG = 101;  
        /// <summary>
        /// 科室类别：医技
        /// </summary>
        public const int DEPTSORT_YIJI = 102; 
        /// <summary>
        /// 科室类别：药剂
        /// </summary>
        public const int DEPTSORT_YAOJI = 103;
        /// <summary>
        /// 科室类别：机关
        /// </summary>
        public const int DEPTSORT_JIGUAN = 104;
        /// <summary>
        /// 科室类别：其他
        /// </summary>
        public const int DEPTSORT_QITA = 105;
        #endregion
    }
}
