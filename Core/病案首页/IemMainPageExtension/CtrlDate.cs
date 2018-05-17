using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrectSoft.Common.Ctrs.OTHER;

namespace IemMainPageExtension
{
    /// <summary>
    /// Add by xlb 2013-05-17
    /// </summary>
    public class CtrlDate:DSDateEdit,IIemMainPageExcept
    {
        private IemMainPageExcept iemEntity;/*扩展维护对象*/
        private IemMainPageExceptUse iemExceptUse;/*使用对象*/

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iemExcept"></param>
        /// <param name="iemUse"></param>
        public CtrlDate(IemMainPageExcept iemExcept, IemMainPageExceptUse iemUse/*使用对象*/)
        {
            try
            {
                this.iemEntity = iemExcept;
                this.iemExceptUse = iemUse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化数据 已存在该记录则填冲记录值 否则默认当前时间
        /// </summary>
        private void InitData()
        {
            try
            {
                if (!string.IsNullOrEmpty(iemExceptUse.IemexUseId))
                {
                    this.Text = iemExceptUse.Value;
                }
                else
                {
                    this.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 返回当前使用对象信息
        /// </summary>
        /// <returns></returns>
        public IemMainPageExceptUse GetIemMainPageExceptUse()
        {
            try
            {
                iemExceptUse.IemexId = iemEntity.IemExId;
                iemExceptUse.Value = this.Text;
                return iemExceptUse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
