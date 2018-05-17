using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Common.Library;
using DrectSoft.Wordbook;
using System.Xml;

namespace IemMainPageExtension
{
    /// <summary>
    /// 下拉框用户控件
    /// Add by xlb 2013-04-12
    /// </summary>
    public partial class UcLookUpEdit : UserControl,IIemMainPageExcept
    {
        private IemMainPageExcept iemEntity;/*扩展维护对象*/
        private IemMainPageExceptUse iemExceptUse;/*使用对象*/

        #region Methods Add by xlb 2013-05-14

        /// <summary>
        /// 构造函数
        /// </summary>
        public UcLookUpEdit()
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
        /// 构造重载
        /// </summary>
        /// <param name="iemMainPageExcept"></param>
        public UcLookUpEdit(IemMainPageExcept iemMainPageExcept/*扩展维护对象*/, IemMainPageExceptUse iemUse/*使用对象*/)
            : this()
        {
            try
            {
                if (iemMainPageExcept == null || iemUse == null)
                {
                    return;
                }
                this.iemEntity = iemMainPageExcept;
                this.iemExceptUse = iemUse;
                this.Width =lookUpEditorIem.Width+5;
                InitLookUpEditor(iemEntity.DateElement);
                InitData();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 初始化下拉列表
        /// </summary>
        /// <param name="dataElement"></param>
        private void InitLookUpEditor(DateElementEntity dataElement)
        {
            try
            {
                LookUpWindow lookUpWindow = new LookUpWindow();
                lookUpEditorIem.Kind = WordbookKind.Sql;
                lookUpEditorIem.ListWindow = lookUpWindow;
                DataTable dt = DateElementEntity.GetDataSorce(dataElement);
                dt.Columns["ID"].Caption = "编号";
                dt.Columns["NAME"].Caption = "名称";
                Dictionary<string, int> colWidths = new Dictionary<string, int>();
                colWidths.Add("ID", 40);
                colWidths.Add("NAME", 50);
                SqlWordbook wordBook = new SqlWordbook("ApplyEmployee", dt, "ID", "NAME", colWidths, "ID//NAME");
                lookUpEditorIem.SqlWordbook = wordBook;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化 填值 没选择则填充默认值
        /// </summary>
        private void InitData()
        {
            try
            {
                if (!string.IsNullOrEmpty(iemExceptUse.IemexUseId))
                {
                    //当前使用对象已存在该记录则选择相应值 支持手输文本
                    lookUpEditorIem.CodeValue = iemExceptUse.Value;
                    if (string.IsNullOrEmpty(lookUpEditorIem.CodeValue))
                    {
                        lookUpEditorIem.Text = iemExceptUse.Value;
                    }
                }
                else
                {
                    Dictionary<string, string> dicstr = DateElementEntity.GetDefaultValue(iemEntity.DateElement);
                    if (dicstr == null)
                    {
                        return;
                    }
                    if (!dicstr.ContainsKey("IsDefault"))
                    {
                        return;
                    }
                    lookUpEditorIem.CodeValue = dicstr["IsDefault"];
                }
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
                //iemExceptUse.Value=lookUpEditorIem.CodeValue == null ? lookUpEditorIem.Text : lookUpEditorIem.CodeValue;
                if (string.IsNullOrEmpty(lookUpEditorIem.CodeValue))
                {
                    iemExceptUse.Value = lookUpEditorIem.Text;
                }
                else
                {
                    iemExceptUse.Value = lookUpEditorIem.CodeValue;
                }
                iemExceptUse.IemexId = iemEntity.IemExId;
                return iemExceptUse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
