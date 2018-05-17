using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using YidanSoft.Library.EmrEditor.Src.Document;

/*
 ********************************************************** 
 *****************   此月经表达式已经作废  *****************
 **********使用最新的月经表达式 MensesFormulaFrmNew ********
 **********************************************************
 */

namespace YidanSoft.Library.EmrEditor.Src.Gui
{
    [Obsolete("此月经表达式已经作废, 使用最新的月经表达式 MensesFormulaFrmNew", true)]
    public partial class MensesFormulaFrm : Form
    {
        public MensesFormulaFrm(ZYTextElement o)
        {
            InitializeComponent();
            Point p = Control.MousePosition;
            this.Location = p;
            num = (ZYMensesFormula)o;
            string info = "";

            this.textBox1.Text = num.Last;
            this.textBox2.Text = num.Period;
        }

        ZYMensesFormula num = null;
        private void buttonCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text.Length == 0)
            {
                this.toolTip1.Show( "请输入持续天数",this.textBox1,1000);
                return;
            }
            if (this.textBox2.Text.Length == 0)
            {
                this.toolTip1.Show( "请输入周期天数",this.textBox2,1000);
                return;
            }

            num.Last = this.textBox1.Text;
            num.Period = this.textBox2.Text;

            this.Close();

            num.OwnerDocument.RefreshSize();
            num.OwnerDocument.ContentChanged();          
            num.OwnerDocument.OwnerControl.Refresh();

        }


    }
}
