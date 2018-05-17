using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace DrectSoft.Core.QCReport.controls
{
    /// <summary>
    /// 封装一个用于输入月份的控件 
    /// add by ywk 2013年8月2日 08:53:30
    /// </summary>
    public partial class InputMonth : TextBox, IControlDataInit
    {
        public InputMonth()
        {
            InitializeComponent();
            this.MaxLength = 2;
           
        }

        public InputMonth(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
            this.MaxLength = 2;
        }

        public string Value
        {
            get
            {
                if (this.Text.Length == 1)
                {
                    this.Text = "0" + this.Text;
                }
                else if (this.Text.Length>2)
                {
                    this.Text = this.Text.Substring(0,2);
                }
                else
                {
                    this.Text = this.Text;
                }
                return this.Text;
            }

        }

        public void InitControlBindData()
        {
            this.Text = DateTime.Now.Month.ToString("00");
        }
        /// <summary>
        /// 判断输入的是否为月份
        /// </summary>
        /// <param name="inputstr"></param>
        /// <returns></returns>
        public bool IsMonth(string inputstr)
        {
            if (inputstr.StartsWith("0"))
            {
                return true;
            }
            string pattern = "^[1-9]$|^1[0-2]$";
            Regex rx = new Regex(pattern);
            return rx.IsMatch(inputstr);
        }
        //protected override void OnMouseLeave(EventArgs e)
        //{

        //    if (!IsMonth(this.Text.ToString()) && !string.IsNullOrEmpty(this.Text.ToString()))
        //    {
        //        Common.Ctrs.DLG.MessageBox.Show("请输入正确的月份");
        //        return;
        //    }
        //    base.OnMouseLeave(e);
        //}

        protected override void OnTextChanged(EventArgs e)
        {

            if (!IsMonth(this.Text.ToString()) && !string.IsNullOrEmpty(this.Text.ToString()))
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请输入正确的月份");
                this.Text = "";
                return;
            }
            base.OnTextChanged(e);
        }
        public void Reset()
        {
            this.Text = DateTime.Now.Month.ToString("00");
        }
    }
}
