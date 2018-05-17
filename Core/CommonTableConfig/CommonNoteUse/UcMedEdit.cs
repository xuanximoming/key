using DrectSoft.Common.Ctrs.FORM;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    /// <summary>
    /// <title>S4大文本编辑</title>
    /// <auth>xuliangliang</auth>
    /// <date>2012-10-31</date>
    /// </summary>
    public partial class UcMedEdit : DevBaseForm
    {
        #region 方法
        string OrginalValue = string.Empty;//原始值
        public string LaterValue = string.Empty;//修改后的值

        /// <summary>
        /// Add try catch by xlb 2013-03-12
        /// </summary>
        /// <param name="valueStr"></param>
        public UcMedEdit(string valueStr)
        {
            try
            {
                InitializeComponent();
                memoEditValue.Text = valueStr == null ? "" : valueStr;
                OrginalValue = valueStr == null ? "" : valueStr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取文本值
        /// Add try catch by xlb 2013-03-12
        /// </summary>
        /// <returns></returns>
        public string GetValue()
        {
            try
            {
                return this.memoEditValue.Text;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 事件
        /// <summary>
        /// 触发拖动控件事件
        /// Add try catch by xlb 2013-03-12
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void memoEditValue_DragEnter(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(typeof(KeyValuePair<string, object>)))
                {
                    KeyValuePair<string, object> keyvalue
                        = (KeyValuePair<string, object>)(e.Data.GetData(typeof(KeyValuePair<string, object>)));
                    if (keyvalue.Value.ToString().ToUpper() == "TEXT")
                    {
                        e.Effect = DragDropEffects.Copy;
                        memoEditValue.Focus();
                        //memoEditValue.SelectionStart = memoEditValue.Text.Length;
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 移动事件完成
        /// Add try catch by xlb 2013-03-12
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void memoEditValue_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(typeof(KeyValuePair<string, object>)))
                {
                    KeyValuePair<string, object> keyvalue = (KeyValuePair<string, object>)(e.Data.GetData(typeof(KeyValuePair<string, object>)));
                    if (keyvalue.Value.ToString().ToUpper() != "TEXT") return;
                    string strInsertText = keyvalue.Key.ToString();
                    int start = this.memoEditValue.SelectionStart;
                    this.memoEditValue.Text = this.memoEditValue.Text.Insert(start, strInsertText);
                    this.memoEditValue.SelectionStart = this.memoEditValue.Text.Length;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 取消事件
        /// Add try catch by xlb 2013-03-12
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancle_Click(object sender, EventArgs e)
        {
            try
            {
                //退出操作不改变原始文本
                LaterValue = OrginalValue;
                DialogResult dialogResliut = DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("是否退出？", "提示", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo);
                if (dialogResliut == DialogResult.No)
                {
                    return;
                }
                this.Close();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 确定事件 Add by xlb 2013-03-12
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                LaterValue = memoEditValue.Text.Trim();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        #endregion

    }
}
