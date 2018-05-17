using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace IemMainPageExtension
{
    /// <summary>
    /// 时间类型维护界面
    /// </summary>
    public partial class UcDate : UserControl,IIemMainPageExcept
    {
        private IemMainPageExcept iemEntity;/*扩展维护对象*/
        private IemMainPageExceptUse iemExceptUse;/*使用对象*/

        /// <summary>
        /// 构造函数
        /// </summary>
        public UcDate()
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
        public UcDate(IemMainPageExcept iemExcept,IemMainPageExceptUse iemUse/*使用对象*/):this()
        {
            try
            {
                this.iemEntity = iemExcept;
                this.iemExceptUse = iemUse;

                this.Width =  dateEdit1.Width;
                InitData();
            }
            catch (Exception)
            {
                throw;
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
                    dateEdit1.Text = iemExceptUse.Value;
                }
                else
                {
                    dateEdit1.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
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
                iemExceptUse.Value = dateEdit1.Text;
                return iemExceptUse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
