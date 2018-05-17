using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace DrectSoft.Core.ZymosisReport.UCControls
{
    public partial class UCEducationLevel : DevExpress.XtraEditors.XtraUserControl,IZymosisReport
    {
        /// <summary>
        /// add by ck 2013-08-13 文化程度控件
        /// </summary>
        public UCEducationLevel()
        {

            try
            {
                InitializeComponent();
                InitData();
            }
            catch (Exception)
            {
            }
        }

        public void InitData()
        {
            try
            {
                string sql = @"select DETAILID ID,t.name from dictionary_detail t where t.categoryid='aa'";
                DataTable dt = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sql);
                lkp_wenhua.Properties.DataSource = dt;
                lkp_wenhua.Properties.ValueMember = "ID";
                lkp_wenhua.Properties.DisplayMember = "NAME";
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        #region IZymosisReport 成员

        public void InitValue(string strValues)
        {
            try
            {
                if (string.IsNullOrEmpty(strValues)) return;
                string[] strValueList = strValues.Split(',');
                lkp_wenhua.EditValue = strValueList[0];
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public string GetValue()
        {
            try
            {
                string strValue = "";
                if (lkp_wenhua.EditValue != null)
                {
                    strValue = lkp_wenhua.EditValue.ToString() + "," + lkp_wenhua.Text;
                }
                return strValue;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void SetFocus()
        {
            lkp_wenhua.Focus();
        }

        #endregion
    }
}
