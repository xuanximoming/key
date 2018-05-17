using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Xml;

namespace IemMainPageExtension
{
    /// <summary>
    /// 数值框用户控件
    /// Add by xlb 2013-04-15
    /// </summary>
    public partial class UcSpinEdit : UserControl,IIemMainPageExcept
    {
        private IemMainPageExcept iemExceptEntity;/*维护病案对象*/
        private IemMainPageExceptUse iemMainPageExceptUse;/*使用对象*/

        #region Methods Add by xlb 2013-04-15

        /// <summary>
        /// 构造函数
        /// </summary>
        public UcSpinEdit()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 重载构造函数
        /// </summary>
        /// <param name="iemExcept"></param>
        public UcSpinEdit(IemMainPageExcept iemExcept,IemMainPageExceptUse iemExceptUse)
            : this()
        {
            try
            {
                this.iemExceptEntity = iemExcept;
                this.iemMainPageExceptUse = iemExceptUse;

                this.Width =spinEditIem.Width;
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
                bool isMaxvalue=decimal.TryParse(dicstr["MaxValue"], out maxValue);
                bool isMinValue=decimal.TryParse(dicstr["MinValue"], out minValue);
                //转换失败则默认选择0
                if(!decimal.TryParse(dicstr["DefaultValue"],out defaultValue))
                {
                    defaultValue=0;
                }
                if (isMaxvalue&&isMaxvalue)
                {
                    spinEditIem.Properties.MaxValue = maxValue;
                    spinEditIem.Properties.MinValue = minValue;
                }

                spinEditIem.Value = defaultValue;
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
                iemMainPageExceptUse.Value = spinEditIem.Value.ToString();
                iemMainPageExceptUse.IemexId = iemExceptEntity.IemExId;
                return iemMainPageExceptUse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
