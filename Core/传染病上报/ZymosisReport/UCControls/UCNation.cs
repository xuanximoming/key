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
    public partial class UCNation : DevExpress.XtraEditors.XtraUserControl,IZymosisReport
    {
        /// <summary>
        /// add by ck 2013-8-15 民族控件
        /// </summary>
        public UCNation()
        {
            try
            {
                InitializeComponent();
                InitData();
                InitValue(null);
            }
            catch (Exception)
            {
            }
        }

        public void InitData()
        {
            try
            {
                string sql = @"SELECT detailid AS ID, NAME, py, memo FROM dictionary_detail 
WHERE categoryid = '42' AND valid = 1";
                DataTable dt = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sql);
                lkp_minzu.Properties.DataSource = dt;
                lkp_minzu.Properties.ValueMember = "ID";
                lkp_minzu.Properties.DisplayMember = "NAME";
                //lkp_minzu.EditValue = "HA";
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
                if (string.IsNullOrEmpty(strValues))
                {
                    lkp_minzu.EditValue = "HA";
                }
                else
                {
                    lkp_minzu.EditValue = strValues.Split(',')[0];
                }
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
                if (lkp_minzu.EditValue == null) return "";
                string strValue = lkp_minzu.EditValue.ToString()+","+lkp_minzu.Text;
                return strValue;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public void SetFocus()
        {
            lkp_minzu.Focus();
        }

        #endregion
    }
}
