using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Core.NursingDocuments.PublicSet;

namespace DrectSoft.Core.NursingDocuments.UserControls
{
    public partial class UCTextSeparateBox : DevExpress.XtraEditors.XtraUserControl
    {
        bool m_IsTransparent=false;

        public UCTextSeparateBox()
        {
            InitializeComponent();
        }

        private void UCTextSeparateBox_Load(object sender, EventArgs e)
        {
            txtSBP.ToolTip = "收缩压范围：1mmHg至250mmHg";
            txtDBP.ToolTip = "舒张压范围：1mmHg至200mmHg";
        }

        /// <summary>
        /// 背景是否透明
        /// </summary>
        public bool IsTransparent
        {
            get
            { 
                return m_IsTransparent;
            }

            set 
            {
                m_IsTransparent = value;
                if (m_IsTransparent)
                {
                    txtDBP.BackColor = Color.Transparent;
                    txtSBP.BackColor = Color.Transparent;
                    textEdit3.BackColor = Color.Transparent;
                    labelControl1.BackColor = Color.Transparent;
                    //this.BackColor = Color.Transparent;
                    
                }
                else
                {
                    txtDBP.BackColor = Color.White;
                    txtSBP.BackColor = Color.White;
                    textEdit3.BackColor = Color.White;
                    labelControl1.BackColor = Color.White;
                    //this.BackColor = Color.White;
                }
            }
        }

        /// <summary>
        /// 判断输入是否为空,true非空，false空(只要其中一个为空)
        /// </summary>
        public bool IsInput
        {
            get
            {
                #region 判断如果输入有非数字，则非空  edit by wyt 2012-11-09
                if (txtDBP.Text.Trim() != "" || txtSBP.Text.Trim() != "")
                {
                    foreach (char c in this.txtDBP.Text.Trim())
                    {
                        if (c < '0' || c > '9')
                        {
                            return true;
                        }
                    }
                    foreach (char c in this.txtSBP.Text.Trim())
                    {
                        if (c < '0' || c > '9')
                        {
                            return true;
                        }
                    }
                }
                #endregion
                return !(txtDBP.Text.Trim() == "" || txtSBP.Text.Trim() == "");  //true非空，false空
            }
        }

        /// <summary>
        /// 血压值
        /// </summary>
        public string BP
        {
            get 
            {
                if (IsInput)
                {
                    #region 如果只输入一项，则返回第一项  edit by wyt 2012-11-09
                    if (txtDBP.Text.Trim() == "" || txtSBP.Text.Trim() == "")
                    {
                        return txtSBP.Text.Trim();
                    }
                    #endregion
                    return txtSBP.Text.Trim() + "/" + txtDBP.Text.Trim();
                }
                else
                {
                    txtSBP.Text = "";
                    txtDBP.Text = "";
                    return "";
                }
            }

            set 
            {
                DateTextReset();
                #region 如果只有一项，则设置为第一项  edit by wyt 2012-11-09
                if (value.Contains("/"))
                {
                    string[] BPArray = value.Split(new char[] { '/' });
                    if (BPArray.Length == 2)
                    {
                        txtSBP.Text = BPArray[0];
                        txtDBP.Text = BPArray[1];
                    }
                }
                else
                {
                    txtSBP.Text = value;
                }
                #endregion
            }
        }

        /// <summary>
        /// 数据重置
        /// </summary>
        public void DateTextReset()
        {
            txtDBP.Text = "";
            txtSBP.Text = "";
        }

        /// <summary>
        /// 检测数据
        /// </summary>
        /// <returns></returns>
        public bool CheckData()
        {
            if (!IsInput) return true;//空退出

            //edit by wyt 2012-11-05 修改使血压可录入中文(根据配置)
            string IsHidePainHeight = MethodSet.GetConfigValueByKey("IsHidePainHeight_NursingRecord");
            if (IsHidePainHeight.Substring(35, 1) == "0")
            {

                double intBp;

                if (!Dataprocessing.IsNumber(txtSBP.Text.Trim().ToString(), 0))
                {
                    MethodSet.App.CustomMessageBox.MessageShow("收缩压必须为数字");
                    txtSBP.Focus();
                    return false;
                }
                else
                {
                    intBp = double.Parse(txtSBP.Text.Trim());
                    if (!(intBp > 0 && intBp <= 300))//仁和需求 add by ywk 
                    {
                        MethodSet.App.CustomMessageBox.MessageShow("收缩压必须在1mmHg至300mmHg之间。");
                        txtSBP.Focus();
                        return false;
                    }
                }

                if (!Dataprocessing.IsNumber(txtDBP.Text.Trim(), 0))
                {
                    MethodSet.App.CustomMessageBox.MessageShow("舒张压必须为数字");
                    txtDBP.Focus();
                    return false;
                }
                else
                {
                    intBp = Convert.ToInt32(txtDBP.Text.Trim());
                    if (!(intBp > 0 && intBp <= 200))
                    {
                        MethodSet.App.CustomMessageBox.MessageShow("舒张压必须在1mmHg至200mmHg之间。");
                        txtDBP.Focus();
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
