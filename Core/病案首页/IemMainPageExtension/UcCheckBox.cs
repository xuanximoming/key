using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;

namespace IemMainPageExtension
{
    public partial class UcCheckBox : UserControl,IIemMainPageExcept
    {
        IemMainPageExcept iemExceptEntity;/*扩展维护对象*/
        IemMainPageExceptUse iemExceptUseEntity;/*使用对象*/

        /// <summary>
        /// 构造函数
        /// </summary>
        public UcCheckBox()
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
        /// 重载构造
        /// </summary>
        /// <param name="iemExcept"></param>
        /// <param name="iemExceptUse"></param>
        public UcCheckBox(IemMainPageExcept iemExcept, IemMainPageExceptUse iemExceptUse)
            : this()
        {
            try
            {
                if (iemExcept == null || iemExceptUse == null)
                {
                    return;
                }
                this.iemExceptEntity=iemExcept;
                this.iemExceptUseEntity=iemExceptUse;
                if (string.IsNullOrEmpty(iemExcept.IemOtherName))
                {
                    iemExcept.IemOtherName = "未指定列";
                }
                this.checkEdit1.Text = iemExcept.IemOtherName + ":";
                int width = TextRenderer.MeasureText(checkEdit1.Text, checkEdit1.Font).Width+5;
                this.Width = width;
                InitDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化 如果对应使用对象属性有值则填充实际值否则默认不选
        /// </summary>
        private void InitDefault()
        {
            try
            {
                if (!string.IsNullOrEmpty(iemExceptUseEntity.IemexUseId))
                {
                    this.checkEdit1.Checked = iemExceptUseEntity.Value == "0" ? false : true;
                }
                else
                {
                    this.checkEdit1.Checked = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 返回使用对象信息
        /// </summary>
        /// <returns></returns>
        public IemMainPageExceptUse GetIemMainPageExceptUse()
        {
            try
            {
                iemExceptUseEntity.Value = checkEdit1.Checked == true ? "1" : "0";
                iemExceptUseEntity.IemexId = iemExceptEntity.IemExId;
                return iemExceptUseEntity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
