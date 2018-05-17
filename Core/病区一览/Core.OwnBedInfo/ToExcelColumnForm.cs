using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Common.Ctrs.FORM;

using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;

namespace DrectSoft.Core.OwnBedInfo
{
    /// <summary>
    /// 导出EXCEL列选择窗体
    /// add by zjy 2013-6-20
    /// </summary>
    public partial class ToExcelColumnForm : DevBaseForm
    {
        public GridControl dbgrid;       
        public GridView dbgridview;
        public List<ListItem> lists=new List<ListItem>();//存储隐藏的窗体
        public int m_iCommandFlag = 0;//1=提交，0=关闭
        public ToExcelColumnForm(GridControl grid,GridView gridview)
        {
            InitializeComponent();
            dbgrid = grid;
            dbgridview = gridview;
            InitBD();
        }


        private void InitBD()
        {
            try
            {
                foreach (GridColumn c in dbgridview.Columns)
                {
                    if (c.Visible == true)
                    {
                        this.lbxNoSelect.Items.Add(new ListItem(c.Name, c.Caption));
                    }
                } 
            }
            catch (Exception)
            {
                
                throw;
            }        
          
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbxNoSelect.SelectedItem != null)
                {
                    int index = lbxNoSelect.SelectedIndex;
                    ListItem it = (ListItem)lbxNoSelect.SelectedItem;
                    if (CheckColumn(it))
                    {
                        this.lbxSelect.Items.Add(it);
                       
                    }
                    if (index >= 0)
                    {
                        index++;
                        if (index < lbxNoSelect.Items.Count)
                        {
                            lbxNoSelect.SelectedIndex = index;
                        }
                    }
                }
           
            }
            catch (Exception)
            {
                
                throw;
            }
         
        }
        //删除
        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbxSelect.SelectedItem != null)
                {
                    int index = lbxSelect.SelectedIndex;

                    lbxSelect.Items.RemoveAt(index);
                    if (index >= 0 && index < lbxSelect.Items.Count)
                    {
                        lbxSelect.SelectedIndex =index++;
                    }
                }         

            }
            catch (Exception)
            {

                throw;
            }

        }
        //添加全部
        private void btnAddAll_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (ListItem it in this.lbxNoSelect.Items)
                {
                    if (CheckColumn(it))
                    {
                        this.lbxSelect.Items.Add(it);
                        
                    }
                }
              
            }
            catch (Exception)
            {

                throw;
            }

        }
        //删除全部
        private void btnDelAll_Click(object sender, EventArgs e)
        {
            try
            {
                this.lbxSelect.Items.Clear();
          
            }
            catch (Exception)
            {
                
                throw;
            }

            
        }
        //验证是否存在列
        private bool CheckColumn(ListItem item)
        {
            try
            {
                foreach (ListItem it in this.lbxSelect.Items)
                {
                    if (it.Value == item.Value)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception)
            {
                
                throw;
            }          
         
            
        }

        #region 对象实体ListItem
        public class ListItem
        {
            private string m_sText = string.Empty;
            private string m_sValue = string.Empty;

            /// <summary>
            /// 创建ListItem项
            /// </summary>
            /// <param name="value">值</param>
            /// <param name="text">文本</param>
            public ListItem(string value, string text)
            {
                this.m_sValue = value;
                this.m_sText = text;
            }

            /// <summary>
            /// 比较
            /// </summary>
            /// <param name="obj">比较的对象</param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                if (base.GetType().Equals(obj.GetType()))
                {
                    ListItem item = (ListItem)obj;
                    return this.m_sText.Equals(item.Value);
                }
                return false;
            }

            /// <summary>
            /// 返回散列表
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return this.m_sValue.GetHashCode();
            }

            /// <summary>
            /// 转成字符串
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return this.m_sText;
            }

            /// <summary>
            /// 显示的文本
            /// </summary>
            public string Text
            {
                get
                {
                    return this.m_sText;
                }
            }

            /// <summary>
            /// 值
            /// </summary>
            public string Value
            {
                get
                {
                    return this.m_sValue;
                }
            }
        }
        #endregion
        //关闭
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                m_iCommandFlag = 0;
                this.Close();
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }
        //提交
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (this.lbxSelect.Items.Count > 0)
            {
                foreach (GridColumn c in dbgridview.Columns)
                {
                    if (c.Visible == true)
                    {
                        if (IsColumn(c.Name))
                        {
                            c.Visible = false;
                            lists.Add(new ListItem(c.Name, c.Caption));
                        }
                    }
                }
                m_iCommandFlag = 1;
                this.Close();
            }
            else
            {
                MessageBox.Show("请选择需要导出的列！");  
            }

        }
        //验证是否是显示列
        private bool IsColumn(string name)
        {
            try
            {
                foreach (ListItem it in this.lbxSelect.Items)
                {
                    if (it.Value == name)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception)
            {
                
                throw;
            }

          
        }

      
    }

   
}