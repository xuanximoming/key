using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Emr.TemplateFactory
{
    public partial class FooterFrm : DevBaseForm
    {
        public FooterFrm()
        {
            InitializeComponent();
        }

        private void FooterFrm_Load(object sender, EventArgs e)
        {
          
            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(AppGlobal.editorfrm.pnlText.EMRDoc.FooterString);
            //XmlNode node = doc.SelectSingleNode("footer/p/span");
            //this.textBox1.Text = node.InnerText;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(AppGlobal.editorfrm.pnlText.EMRDoc.FooterString);
            //XmlNode node = doc.SelectSingleNode("footer/p/span");
            //node.InnerText = this.textBox1.Text;

            //AppGlobal.editorfrm.pnlText.EMRDoc.FooterString = doc.OuterXml;

            //this.Close();

        }

        /// <summary>
        /// 回车光标后移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                DS_Common.win_KeyPress(e);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
    }
}
