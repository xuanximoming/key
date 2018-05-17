using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace DrectSoft.EMR.ThreeRecordAll.Controls
{
    public partial class UCTextGroupBox : DevExpress.XtraEditors.XtraUserControl
    {
        private string m_Befor = "";//人工辅助前大便次数
        private string m_After = "";//人工辅助后大便次数
        private string m_Labour = "";//人工辅助大便次数

        bool m_Transparent = false;//是否透明

        public TextEdit TxtBefor
        {
            get { return this.txtBefor; }
        }

        public TextEdit TxtAfter
        {
            get { return this.txtAfter; }
        }

        public TextEdit TxtLabour
        {
            get { return this.txtLabour; }
        }

        public UCTextGroupBox()
        {
            InitializeComponent();
        }

        #region 设置背景色透明色
        /// <summary>
        /// 设置背景色透明色
        /// </summary>
        public bool IsTransparent
        {
            set
            {
                m_Transparent = value;
                if (m_Transparent)
                {

                    txtBefor.BackColor = Color.Transparent;
                    txtAfter.BackColor = Color.Transparent;
                    txtLabour.BackColor = Color.Transparent;
                    panelControl1.BackColor = Color.Transparent;

                    textEdit1.Visible = false;
                    textEdit2.Visible = false;

                    txtBefor.Visible = false;
                    txtAfter.Visible = false;
                    txtLabour.Visible = false;

                }
                else
                {
                    txtBefor.BackColor = Color.White;
                    txtAfter.BackColor = Color.White;
                    txtLabour.BackColor = Color.White;
                    panelControl1.BackColor = Color.White;

                    textEdit1.Visible = true;
                    textEdit2.Visible = true;

                    txtBefor.Visible = true;
                    txtAfter.Visible = true;
                    txtLabour.Visible = true;

                }
            }
        }
        #endregion

        #region 设置控件是否能用
        /// <summary>
        /// 设置控件是否能用
        /// </summary>
        /// <param name="Index">复选框索引</param>
        private void SetCheckeEnabled(int index)
        {
            if (index == 1)
            {
                chkLetOffShit1.Checked = true;
                chkLetOffShit2.Checked = false;
                txtAfter.Enabled = false;
                txtBefor.Enabled = false;
                txtLabour.Enabled = false;
                textEdit1.Enabled = false;
                textEdit2.Enabled = false;
                textEdit1.BackColor = txtLabour.BackColor;
                textEdit2.BackColor = txtLabour.BackColor;

            }
            else if (index == 2)
            {
                chkLetOffShit1.Checked = false;
                chkLetOffShit2.Checked = true;
                txtAfter.Enabled = false;
                txtBefor.Enabled = false;
                txtLabour.Enabled = false;
                textEdit1.Enabled = false;
                textEdit2.Enabled = false;
                textEdit1.BackColor = txtLabour.BackColor;
                textEdit2.BackColor = txtLabour.BackColor;
            }
            else
            {
                chkLetOffShit1.Checked = false;
                chkLetOffShit2.Checked = false;
                txtAfter.Enabled = true;
                txtBefor.Enabled = true;
                txtLabour.Enabled = true;
                textEdit1.Enabled = false;
                textEdit2.Enabled = false;
                textEdit1.BackColor = Color.White;
                textEdit2.BackColor = Color.White;
            }
        }
        #endregion

        #region 排便次数
        /// <summary>
        /// 排便次数
        /// </summary>
        public string Shit
        {
            get
            {
                if (chkLetOffShit1.Checked)
                {
                    m_Befor = "☆";
                    //m_Labour = "☆";// edit by tj 2012-10-30 青龙山医院需求
                    //m_After = "☆"; //edit by tj 2012-10-30 青龙山医院需求
                    return m_Befor;  //edit by tj 2012-10-30 青龙山医院需求
                }
                else if (chkLetOffShit2.Checked)
                {
                    m_Befor = "※";
                    //m_Labour = "※";// edit by tj 2012-10-30 青龙山医院需求
                    //m_After = "※";// edit by tj 2012-10-30 青龙山医院需求
                    return m_Befor;  //edit by tj 2012-10-30 青龙山医院需求
                }
                else
                {
                    //string pattern = @"^[0-9]*$";
                    //Match m = Regex.Match(this.txtBefor.Text.Trim(), pattern);   // 匹配正则表达式           
                    //if (!m.Success)//不是数字
                    //{
                    //    PublicMethod.RadAlterBox("人工辅助前大便次数请输入数字", "提示");
                    //}
                    //string pattern = @"^[0-9]*$";
                    //Match m = Regex.Match(this.txtBefor.Text.Trim(), pattern);   // 匹配正则表达式           
                    //if (!m.Success)//不是数字
                    //{
                    //    PublicMethod.RadAlterBox("人工辅助前大便次数请输入数字", "提示");
                    //}
                    if (IsInput)
                    {
                        m_Befor = txtBefor.Text.Trim();
                        m_Labour = txtLabour.Text.Trim();
                        m_After = txtAfter.Text.Trim();
                    }
                    else
                    {
                        m_Befor = "";
                        m_Labour = "";
                        m_After = "";

                        txtBefor.Text = m_Befor;
                        txtLabour.Text = m_Labour;
                        txtAfter.Text = m_After;

                    }
                }


                //return m_Befor + ":" + m_Labour + ":" + m_After; //edit by tj 2012-10-30 青龙山医院需求
                if (string.IsNullOrEmpty(m_Labour) && string.IsNullOrEmpty(m_After))
                    return m_Befor;
                else
                return m_Befor + " " + m_Labour + "/" + m_After; //add by tj 2012-10-30 青龙山医院需求
            }

            set
            {
                //if (value == "☆:☆:☆")
                if (value == "☆:☆:☆" || value == "☆")
                {
                    m_Befor = "☆";
                    m_Labour = "";
                    m_After = "";

                    SetCheckeEnabled(1);

                }
                //else if (value == "※:※:※")
                else if (value == "※:※:※" || value == "※")
                {
                    m_Befor = "※";
                    m_Labour = "";
                    m_After = "";

                    SetCheckeEnabled(2);

                }
                else
                {
                    //string[] str = value.Split(':'); //edit by tj 2012-10-30 青龙山医院需求
                    try  //处理旧数据
                    {
                        string[] str = value.Split(' ');
                        if (str.Length == 1 && !str[0].Contains("/"))
                        {
                            m_Befor = str[0];
                        }
                        else if (str.Length == 1 && str[0].Contains("/"))
                        {
                            string[] subStrs = str[0].Split('/');
                            m_Labour = subStrs[0];
                            m_After = subStrs[1];
                        }
                        else
                        {
                            string[] subStrs = str[1].Split('/');
                            if (str.Length == 2 && subStrs.Length == 2 && value != " /")
                            {
                                m_Befor = str[0];
                                m_Labour = subStrs[0];
                                m_After = subStrs[1];

                            }
                            else
                            {
                                m_Befor = "";
                                m_Labour = "";
                                m_After = "";

                            }
                        }

                        txtBefor.Text = m_Befor;
                        txtLabour.Text = m_Labour;
                        txtAfter.Text = m_After;
                        SetCheckeEnabled(-1);
                    }
                    catch (Exception)
                    {
                        m_Befor = "";
                        m_Labour = "";
                        m_After = "";
                        txtBefor.Text = m_Befor;
                        txtLabour.Text = m_Labour;
                        txtAfter.Text = m_After;
                    }
                }


            }
        }
        #endregion

        #region 判断是否输入数据
        /// <summary>
        /// 判断是否输入数据,true有数据，false无数据
        /// </summary>
        public bool IsInput
        {
            get
            {
                if (chkLetOffShit1.Checked || chkLetOffShit2.Checked)
                {
                    return true;
                }
                else
                {
                    //if ((txtBefor.Text.Trim()!= "" && txtAfter.Text.Trim() != "" && txtLabour.Text.Trim() != "") ||
                    //    (txtBefor.Text.Trim() != "" && txtAfter.Text.Trim() == "" && txtLabour.Text.Trim() == "") ) 
                    //if (
                    //   (txtAfter.Text.Trim() == "" && txtLabour.Text.Trim() == "")) 
                    //{
                    //    return false;
                    //}
                    //else
                    //{
                    return true;
                    //}
                }
            }
        }
        #endregion

        #region 数据重置
        /// <summary>
        /// 数据重置
        /// </summary>
        public void DateTextReset()
        {
            m_Befor = "";
            m_Labour = "";
            m_After = "";

            txtBefor.Text = m_Befor;
            txtLabour.Text = m_Labour;
            txtAfter.Text = m_After;

            SetCheckeEnabled(-1);

        }
        #endregion


        private void chkLetOffShit_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckEdit).Checked && (sender as CheckEdit).Name.Equals("chkLetOffShit1"))
            {
                chkLetOffShit2.Checked = false;
                SetCheckeEnabled(1);
            }
            else if ((sender as CheckEdit).Checked && (sender as CheckEdit).Name.Equals("chkLetOffShit2"))
            {
                chkLetOffShit1.Checked = false;
                SetCheckeEnabled(2);
            }
            else
            {
                SetCheckeEnabled(-1);
            }
        }

        private void UCTextGroupBox_Load(object sender, EventArgs e)
        {
            txtBefor.ToolTip = "正常排便次数";
            txtAfter.ToolTip = "灌肠后排便次数";
            txtLabour.ToolTip = "灌肠次数，如2E为两次灌肠";
            chkLetOffShit1.ToolTip = "“☆”表示人工肛门";
            lblLetOffShit1.ToolTip = "“☆”表示人工肛门";
            chkLetOffShit2.ToolTip = "“※”表示大便失禁";
            lblLetOffShit2.ToolTip = "“※”表示大便失禁";
        }

      

    }
}
