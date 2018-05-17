using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;

namespace DrectSoft.Core.EMR_NursingDocument.PublicMethod
{

    /// <summary> 
    /// 实体转换辅助类 
    /// </summary> 
    public class NurseService
    {
        /// <summary>
        /// check数据集中是否存在空记录
        /// </summary>
        /// <param name="lst">数据集</param>
        /// <returns></returns>
        public bool CheckListBeforeSave(List<NurseRecordEntity> lst)
        {
            bool boo = false;
            foreach (NurseRecordEntity item in lst)
            {
                if (String.IsNullOrEmpty(item.RECORD_DATETIME)
                    && String.IsNullOrEmpty(item.TIWEN)
                    && String.IsNullOrEmpty(item.MAIBO)
                    && String.IsNullOrEmpty(item.HUXI)
                    && String.IsNullOrEmpty(item.XUEYA)
                    && String.IsNullOrEmpty(item.YISHI)
                    && String.IsNullOrEmpty(item.XYBHD)
                    && String.IsNullOrEmpty(item.QKFL)
                    && String.IsNullOrEmpty(item.SYPF)
                    && String.IsNullOrEmpty(item.OTHER_ONE) 
                    && String.IsNullOrEmpty(item.OTHER_TWO)
                    && String.IsNullOrEmpty(item.OTHER_THREE)
                    && String.IsNullOrEmpty(item.OTHER_FOUR) 
                    && String.IsNullOrEmpty(item.ZTKDX)
                    && String.IsNullOrEmpty(item.YTKDX)
                    && String.IsNullOrEmpty(item.TKDGFS)
                    && String.IsNullOrEmpty(item.WOWEI)
                    && String.IsNullOrEmpty(item.JMZG)
                    && String.IsNullOrEmpty(item.DGJYLG_ONE) 
                    && String.IsNullOrEmpty(item.DGJYLG_TWO)
                    && String.IsNullOrEmpty(item.DGJYLG_THREE)
                    && String.IsNullOrEmpty(item.In_ITEM)
                    && String.IsNullOrEmpty(item.In_VALUE)
                    && String.IsNullOrEmpty(item.OUT_ITEM)
                    && String.IsNullOrEmpty(item.OUT_VALUE)
                    && String.IsNullOrEmpty(item.OUT_COLOR)
                    && String.IsNullOrEmpty(item.OUT_STATUE)
                    && String.IsNullOrEmpty(item.OTHER)
                    && String.IsNullOrEmpty(item.HXMS)
                    && String.IsNullOrEmpty(item.FCIMIAO) 
                    && String.IsNullOrEmpty(item.XRYND)
                    && String.IsNullOrEmpty(item.CGSD)
                    && String.IsNullOrEmpty(item.LXXZYTQ)
                    && String.IsNullOrEmpty(item.BDG)
                    && String.IsNullOrEmpty(item.SINGE_DOCTOR)
                    && String.IsNullOrEmpty(item.SINGE_DOCTORID))
                {
                    boo = true;
                    break;
                }
            }

            return boo;
        }

    }
}
