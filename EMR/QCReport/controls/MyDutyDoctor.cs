using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors;
using System.Collections;
using DrectSoft.Common.Ctrs.DLG;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace DrectSoft.Core.QCReport.controls
{
    public partial class MyDutyDoctor : System.Windows.Forms.ComboBox, IControlDataInit
    {
        private List<IDNAME> m_list = new List<IDNAME>();

        public IDNAME idname = null;//当前选择的Item对象

        protected override void OnEnter(EventArgs e)
        {
            m_list.Clear();
            foreach (IDNAME obj in this.Items)
            {
                m_list.Add(obj);
            }
            base.OnEnter(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            this.Items.Clear();
            this.Items.AddRange(m_list.ToArray());
            base.OnLeave(e);
        }

        protected override void OnTextUpdate(EventArgs e)
        {
            while (this.Items.Count > 0)
            {
                this.Items.RemoveAt(0);
            }
            foreach (object o in this.m_list)
            {
                if (GetChineseSpell(o.ToString()).ToLower().Contains(this.Text.ToLower()))
                {
                    this.Items.Add(o);
                }
            }
            this.DroppedDown = true;
            this.Cursor = Cursors.Default;
            base.OnTextUpdate(e);
        }

        static public string GetChineseSpell(string strText)
        {
            int len = strText.Length;
            string myStr = "";
            for (int i = 0; i < len; i++)
            {
                myStr += getSpell(strText.Substring(i, 1));
            }
            return myStr;
        }

        static public string getSpell(string cnChar)
        {
            byte[] arrCN = Encoding.Default.GetBytes(cnChar);
            if (arrCN.Length > 1)
            {
                int area = (short)arrCN[0];
                int pos = (short)arrCN[1];
                int code = (area << 8) + pos;
                int[] areacode = { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481 };
                for (int i = 0; i < 26; i++)
                {
                    int max = 55290;
                    if (i != 25) max = areacode[i + 1];
                    if (areacode[i] <= code && code < max)
                    {
                        return Encoding.Default.GetString(new byte[] { (byte)(65 + i) });
                    }
                }
                return "*";
            }
            else
                return cnChar;
        }

        public void InitControlBindData()
        {
            try
            {
                DrectSoft.DSSqlHelper.DS_SqlHelper.CreateSqlHelper();
                string sql = "select u.id,u.name,u.py,u.wb from users u ";
                DataTable Dept = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sql,CommandType.Text);
                for (int i = 0; i < Dept.Rows.Count; i++)
                {
                    IDNAME obj = new IDNAME(Dept.Rows[i]["NAME"].ToString(), Dept.Rows[i]["ID"].ToString());
                    m_list.Add(obj);
                    this.Items.Add(obj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string Value
        {
            get
            {
                if (idname == null)
                {
                    return "";
                }
                else if (idname.Tag == null)
                {
                    return "";
                }
                else
                {
                    return idname.Tag.ToString();
                }
            }
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            try
            {
                idname = this.SelectedItem as IDNAME;
                base.OnSelectedIndexChanged(e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        public void Reset()
        {
            try
            {
                this.Text = "";
                idname = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
