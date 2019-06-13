using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Common.Library;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.DSSqlHelper;
using DrectSoft.Wordbook;
using DrectSoft.Common.Ctrs.DLG;

namespace DrectSoft.Core.IEMMainPage
{
    /// <summary>
    /// 疾病诊断编码窗体
    /// Add by xlb 2013-03-21
    /// </summary>
    public partial class IemDialogForm :DevBaseForm
    {
        IEmrHost m_app;
        private DataRow dataRow;

        #region 方法 Add by xlb 2013-03-21

        /// <summary>
        /// 构造函数
        /// </summary>
        public IemDialogForm()
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
        /// 构造函数
        /// </summary>
        /// <param name="host"></param>
        public IemDialogForm(IEmrHost host)
            : this()
        {
            try
            {
                m_app = host;
                YD_SqlHelper.CreateSqlHelper();
                InitLookUpEdit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 数据行记录疾病编码和名称
        /// </summary>
        public DataRow DataRow
        {
            get {
                GetDataRow();
                return dataRow;
                }
        }

        /// <summary>
        /// 初始化LookUpEdit控件
        /// Add by xlb 2013-03-21
        /// </summary>
        private void InitLookUpEdit()
        {

            try
            {
                InitDialogData(lookUpEditDialog);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化控件数据源
        /// Add by xlb 2013-03-22
        /// </summary>
        /// <param name="lookUpEdit"></param>
        private void InitDialogData(LookUpEditor lookUpEdit)
        {
            try
            {
                LookUpWindow lookUpWindow = new LookUpWindow();
                lookUpEdit.ListWindow = lookUpWindow;
                lookUpEdit.Kind = WordbookKind.Sql;
                DataTable dt = GetDialogData();

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName.ToUpper().Trim() == "ICD")
                    {
                        dt.Columns[i].Caption = "编码";
                    }
                    else if (dt.Columns[i].ColumnName.ToUpper().Trim() == "NAME")
                    {
                        dt.Columns[i].Caption = "名称";
                    }
                }

                Dictionary<string, int> dictionary = new Dictionary<string, int>();
                dictionary.Add("ICD", 70);
                dictionary.Add("NAME", 130);
                SqlWordbook sqlWordBook = new SqlWordbook("Dialog",dt,"ICD","NAME",dictionary,"ICD//NAME//PY//WB");
                lookUpEdit.SqlWordbook = sqlWordBook;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 返回数据行
        /// </summary>
        /// <returns></returns>
        private DataRow GetDataRow()
        {
            try
            {
                DataTable dt = new DataTable();
                if (!dt.Columns.Contains("Dialog_ICD"))
                {
                    dt.Columns.Add("Dialog_ICD");
                }
                if (!dt.Columns.Contains("Dialog_NAME"))
                {
                    dt.Columns.Add("Dialog_NAME");
                }
                DataRow Row = dt.NewRow();
                Row["Dialog_ICD"]= lookUpEditDialog.CodeValue;
                Row["Dialog_NAME"] = lookUpEditDialog.Text;
                dataRow = Row;
                return dataRow;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 抓取病历诊断编码
        /// Add by xlb 2013-03-21
        /// </summary>
        private DataTable GetDialogData()
        {
            try
            {
                string sql = "select ICD,NAME,PY,WB from  diagnosis_xt_bj where valid='1'";
                DataTable dtDialog = YD_SqlHelper.ExecuteDataTable(sql, CommandType.Text);
                return dtDialog;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 事件 Add by xlb 2013-03-22

        /// <summary>
        /// 确定事件
        /// Add by xlb 2013-03-22
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(lookUpEditDialog.CodeValue))
                {
                    MessageBox.Show("请选择疾病诊断编码");
                    return;
                }
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 取消事件
        /// Add by xlb 2013-03-22
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

         #endregion
    }
}