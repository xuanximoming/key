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
    /// 封装一个用于输入年份的控件
    /// add by ywk 2013年8月1日 17:18:45
    /// </summary>
    public partial class InputYear : TextBox, IControlDataInit
    {
        public InputYear()
        {
            InitializeComponent();
        }

        public InputYear(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public string Value
        {
            get { return this.Text; }
            
        }

        public void InitControlBindData()
        {
            this.Text = DateTime.Now.Year.ToString();
        }
        /// <summary>
        /// 是否输入的为数字
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool IsNum(string input)
        {
            string pattern = "^[0-9]*$";
            Regex rx = new Regex(pattern);
            return rx.IsMatch(input);
        }


        protected override void OnTextChanged(EventArgs e)
        {
            if (!IsNum(this.Text.ToString()) && !string.IsNullOrEmpty(this.Text.ToString()))
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请输入正确的年份");
                this.Text = "";
                return;
            }
            base.OnTextChanged(e);
        }

        public void Reset()
        {
            this.Text = DateTime.Now.Year.ToString();
        }
    }
}
