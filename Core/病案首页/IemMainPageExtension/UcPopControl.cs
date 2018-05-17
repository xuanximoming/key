using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using DevExpress.XtraEditors.Controls;
using DrectSoft.Common.Ctrs.DLG;

namespace IemMainPageExtension
{
    /// <summary>
    /// 多选控件用户控件
    /// Add by xlb 2013-04-15
    /// </summary>
    public partial class UcPopControl : UserControl,IIemMainPageExcept
    {
        private IemMainPageExcept iemMainExcept;/*病案扩展维护对象*/
        private IemMainPageExceptUse iemMainPageExceptUse;/*使用对象*/

        #region Methods　 Add by xlb 2013-04-15

        /// <summary>
        /// 构造函数
        /// </summary>
        public UcPopControl()
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
        public UcPopControl(
            IemMainPageExcept iemExcept,
            IemMainPageExceptUse iemExceptUse)
            : this()
        {
            try
            {
                this.iemMainExcept = iemExcept;
                this.iemMainPageExceptUse = iemExceptUse;
                this.Width =popupContainerEditDX.Width;
                InitData();
                RegisterEvent();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData()
        {
            try
            {
                DataTable dataTable = DateElementEntity.GetDataSorce(iemMainExcept.DateElement);
                chkListBoxControlDX.Items.Clear();

                foreach (DataRow row in dataTable.Rows)
                {
                    CheckedListBoxItem chkListBoxItem = new CheckedListBoxItem(row, row["NAME"].ToString());
                    chkListBoxControlDX.Items.Add(chkListBoxItem);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 注册事件方法
        /// </summary>
        private void RegisterEvent()
        {
            try
            {
                chkListBoxControlDX.ItemCheck += new DevExpress.XtraEditors.Controls.ItemCheckEventHandler(chkListBoxControlDX_ItemCheck);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 实现接口方法 返回使用对象信息
        /// </summary>
        /// <returns></returns>
        public IemMainPageExceptUse GetIemMainPageExceptUse()
        {
            try
            {
                string value = "";
                foreach (CheckedListBoxItem item in chkListBoxControlDX.Items)
                {
                    if (item.CheckState == CheckState.Checked)
                    {
                        value += (item.Value as DataRow)["Id"];
                        value += ",";
                    }
                }

                iemMainPageExceptUse.Value = value;
                iemMainPageExceptUse.IemexId = iemMainExcept.IemExId;
                return iemMainPageExceptUse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region   Events    Add by xlb 2013-04-15

        /// <summary>
        /// 多选事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void chkListBoxControlDX_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            try
            {
                string valueStr = "";
                foreach (CheckedListBoxItem item in chkListBoxControlDX.Items)
                {
                    if (item.CheckState == CheckState.Checked)
                    {
                        valueStr += (item.Value as DataRow)["NAME"].ToString()+";";
                    }
                }
                popupContainerEditDX.EditValue = valueStr;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #endregion
    }
}
