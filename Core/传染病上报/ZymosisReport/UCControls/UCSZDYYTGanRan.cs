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
    public partial class UCSZDYYTGanRan : DevExpress.XtraEditors.XtraUserControl, IZymosisReport
    {
        /// <summary>
        /// xll 沙眼衣原体感染控件
        /// </summary>
        public UCSZDYYTGanRan()
        {
            try
            {

                InitializeComponent();
                InitDateSourse();
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        private void InitDateSourse()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ID");
                dt.Columns.Add("NAME");
             DataRow dr1=   dt.NewRow();
             dr1["ID"] = "1";
             dr1["NAME"] = "确诊病例";
             DataRow dr2= dt.NewRow();
             dr2["ID"] = "2";
             dr2["NAME"] = "无症状感染";
             dt.Rows.Add(dr1);
             dt.Rows.Add(dr2);
             lpkSZDGanRan.Properties.DataSource = dt;
             lpkSZDGanRan.Properties.ValueMember = "ID";
             lpkSZDGanRan.Properties.DisplayMember = "NAME";
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public void InitValue(string strValues)
        {
            try
            {
                if (string.IsNullOrEmpty(strValues)) return;
                lpkSZDGanRan.EditValue = strValues.Split('，', ',')[0];
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

                if (lpkSZDGanRan.EditValue == null) return "";
                string strValue = lpkSZDGanRan.EditValue.ToString() + "," + lpkSZDGanRan.Text;
                return strValue;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public void SetFocus()
        {
            try
            {
                lpkSZDGanRan.Focus();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
    }
}
