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
                    return  txtSBP.Text.Trim() + "/" + txtDBP.Text.Trim();
                else
                    txtSBP.Text = "";
                    txtDBP.Text ="";
                    return "";
            }

            set 
            {
                DateTextReset();
                
                string[] BPArray = value.Split(new char[] { '/' });
                if (BPArray.Length == 2)
                {
                    txtSBP.Text = BPArray[0];
                    txtDBP.Text = BPArray[1];
                }
               
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

            int intBp;

            if (!Dataprocessing.IsNumber(txtSBP.Text.Trim().ToString(), 0))
            {
                MethodSet.App.CustomMessageBox.MessageShow("收缩压必须为数字!");
                return false;
            }
            else
            {
                intBp = Convert.ToInt32(txtSBP.Text.Trim().ToString());
                if (!(intBp >0 && intBp <= 250))
                {
                    MethodSet.App.CustomMessageBox.MessageShow("收缩压必须在1mmHg至250mmHg之间!");
                    return false;
                }
             }


            if (!Dataprocessing.IsNumber(txtDBP.Text.Trim().ToString(), 0))
            {
                MethodSet.App.CustomMessageBox.MessageShow("舒张压必须为数字!");
                return false;
            }
            else
            {
                intBp = Convert.ToInt32(txtDBP.Text.Trim().ToString());
                if (!(intBp >0 && intBp <= 200))
                {
                    MethodSet.App.CustomMessageBox.MessageShow("舒张压必须在1mmHg至200mmHg之间!");
                    return false;
                }                
            }

            return true;
        }
    }
}
