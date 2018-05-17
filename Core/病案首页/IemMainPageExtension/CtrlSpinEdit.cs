using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrectSoft.Common.Ctrs.OTHER;

namespace IemMainPageExtension
{
    public class CtrlSpinEdit : DSSpinEdit,IIemMainPageExcept
    {
        private IemMainPageExcept iemExceptEntity;/*维护病案对象*/
        private IemMainPageExceptUse iemMainPageExceptUse;/*使用对象*/

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iemExcept"></param>
        /// <param name="iemExceptUse"></param>
        public CtrlSpinEdit(IemMainPageExcept iemExcept, IemMainPageExceptUse iemExceptUse)
        {
            try
            {
                this.iemExceptEntity = iemExcept;
                this.iemMainPageExceptUse = iemExceptUse;
                InitData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化控件数据源
        /// </summary>
        private void InitData()
        {
            try
            {
                Dictionary<string, string> dicstr = DateElementEntity.GetDataSource(iemExceptEntity.DateElement);
                if (dicstr == null)
                {
                    return;
                }
                decimal minValue;/*最小值*/
                decimal maxValue;/*最大值*/
                decimal defaultValue;/*默认值*/
                bool isMaxvalue = decimal.TryParse(dicstr["MaxValue"], out maxValue);
                bool isMinValue = decimal.TryParse(dicstr["MinValue"], out minValue);
                //转换失败则默认选择0
                if (!decimal.TryParse(dicstr["DefaultValue"], out defaultValue))
                {
                    defaultValue = 0;
                }
                if (isMaxvalue && isMaxvalue)
                {
                    this.Properties.MaxValue = maxValue;
                    this.Properties.MinValue = minValue;
                }

                this.Value = defaultValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 接口方法 返回使用对象信息
        /// </summary>
        /// <returns></returns>
        public IemMainPageExceptUse GetIemMainPageExceptUse()
        {
            try
            {
                iemMainPageExceptUse.Value = this.Value.ToString();
                iemMainPageExceptUse.IemexId = iemExceptEntity.IemExId;
                return iemMainPageExceptUse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
