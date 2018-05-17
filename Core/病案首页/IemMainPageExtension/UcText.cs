using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Common.Ctrs.DLG;

namespace IemMainPageExtension
{
    /// <summary>
    /// 普通文本编辑用户控件
    /// Add by xlb 2013-04-11
    /// </summary>
    public partial class UcText : UserControl, IIemMainPageExcept
    {
        IemMainPageExcept iemExcept;/*维护对象实体*/
        IemMainPageExceptUse myIemMainPageExceptUse; //使用对象

        #region Methods Add by xlb 2013-04-11

        /// <summary>
        /// 构造函数
        /// </summary>
        public UcText()
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
        /// 构造函数重载
        /// </summary>
        /// <param name="iem"></param>
        public UcText(IemMainPageExcept iem, IemMainPageExceptUse iemMainPageExceptUse)
            : this()
        {
            try
            {
                if (iem == null || iemMainPageExceptUse == null)
                {
                    return;
                }
                this.iemExcept = iem;
                myIemMainPageExceptUse = iemMainPageExceptUse;

                if (string.IsNullOrEmpty(iemExcept.IemOtherName))
                {
                    iemExcept.IemOtherName = "未指定列";
                }
                this.Width =textEdit1.Width;
                InitData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化数据
        /// 有默认值则填充默认值
        /// </summary>
        private void InitData()
        {
            try
            {
                if (!string.IsNullOrEmpty(myIemMainPageExceptUse.IemexUseId))
                {
                    this.textEdit1.Text = myIemMainPageExceptUse.Value;
                }
                else
                {
                    Dictionary<string, string> dicStr = DateElementEntity.GetDataSource(iemExcept.DateElement);
                    if (dicStr == null)
                    {
                        return;
                    }
                    //默认值
                    string defaultValue = dicStr["DefaultValue"];
                    this.textEdit1.Text = defaultValue;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 接口方法
        /// 返回使用对象
        /// </summary>
        /// <returns></returns>
        public IemMainPageExceptUse GetIemMainPageExceptUse()
        {
            try
            {
                myIemMainPageExceptUse.Value = textEdit1.Text;
                myIemMainPageExceptUse.IemexId = iemExcept.IemExId;
                return myIemMainPageExceptUse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
