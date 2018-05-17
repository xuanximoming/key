using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IemMainPageExtension
{
    /// <summary>
    /// 简单工厂根据对象类型返回对应的控件
    /// <auth>XLB</auth>
    /// <date>2013-06-06</date>
    /// </summary>
    public class CreateControlFactory
    {
        /// <summary>
        /// 根据数据元类型返回相应的用户控件
        /// </summary>
        /// <param name="iemExcept">扩展对象</param>
        /// <param name="dataEntity">数据元对象实体</param>
        /// <param name="iemMainPageExceptUse">扩展适用对象</param>
        /// <returns>对应用户控件</returns>
        public static UserControl CreateControl(IemMainPageExcept iemExcept, DateElementEntity dataEntity, IemMainPageExceptUse iemMainPageExceptUse)
        {
            try
            {
                if (dataEntity == null || string.IsNullOrEmpty(dataEntity.ElementId))
                {
                    return null;
                }
                UserControl userControl = new UserControl();
                string elementType = dataEntity.ElementType.ToUpper().Trim();
                switch (elementType.ToLower())
                {
                    case "s1":/*普通文本*/
                        userControl = new UcText(iemExcept, iemMainPageExceptUse);
                        break;
                    case "s2":/*单选类型*/
                    case "s3":/*单选类型*/
                        userControl = new UcLookUpEdit(iemExcept, iemMainPageExceptUse);
                        break;
                    case "s4":/*大文本型*/
                        userControl = new UcMemoEdit(iemExcept, iemMainPageExceptUse);
                        break;
                    case "d":/*日期型*/
                        userControl = new UcDate(iemExcept, iemMainPageExceptUse);
                        break;
                    case "n":/*数值型*/
                        userControl = new UcSpinEdit(iemExcept, iemMainPageExceptUse);
                        break;
                    case "s9":/*多选型*/
                        userControl = new UcPopControl(iemExcept, iemMainPageExceptUse);
                        break;
                }
                return userControl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
