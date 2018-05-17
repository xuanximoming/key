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
    /// 大文本编辑界面
    /// Add By xlb 2013-04-12
    /// </summary>
    public partial class UcMemoEdit : UserControl, IIemMainPageExcept
    {
        private IemMainPageExcept iemExceptEntity;/*扩展维护对象*/
        private IemMainPageExceptUse iemExceptUse;/*使用对象*/

        #region Methods Add by xlb2013-04-12

        /// <summary>
        /// 构造函数
        /// </summary>
        public UcMemoEdit()
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
        /// <param name="iemExcept">扩展维护对象</param>
        /// <param name="iemUseEntity">使用对象信息</param>
        public UcMemoEdit(IemMainPageExcept iemExcept,IemMainPageExceptUse iemUseEntity)
            : this()
        {
            try
            {
                this.iemExceptEntity = iemExcept;
                iemExceptUse = iemUseEntity;
                this.Width =memoEdit1.Width;
                InitDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化
        /// 没有填充值则填充数据元XML指定默认值
        /// </summary>
        private void InitDefault()
        {
            try
            {
                Dictionary<string, string> dicstr = DateElementEntity.GetDataSource(iemExceptEntity.DateElement);
                if (dicstr == null)
                {
                    return;
                }
                string defaultValue = dicstr["DefaultValue"];
                this.memoEdit1.Text = defaultValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 实现接口方法返回相应对象信息
        /// </summary>
        /// <returns></returns>
        public IemMainPageExceptUse GetIemMainPageExceptUse()
        {
            try
            {
                iemExceptUse.Value = memoEdit1.Text;
                iemExceptUse.IemexId = iemExceptEntity.IemExId;
                return iemExceptUse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Events Add by xlb 2013-04-12

        /// <summary>
        /// 重绘方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UcMemoEdit_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                foreach (Control control in this.Controls)
                {
                    if (control is LabelControl)
                    {
                        control.Visible = false;
                        e.Graphics.DrawString(control.Text, control.Font, Brushes.Black, control.Location);
                    }
                    else if (control is MemoEdit)
                    {
                        e.Graphics.DrawLine(Pens.Black, new Point(control.Location.X, control.Location.Y + control.Height), new Point(control.Location.X + control.Width, control.Location.Y + control.Height));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
