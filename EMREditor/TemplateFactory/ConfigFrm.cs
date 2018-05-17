using DrectSoft.Common;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Core;
using DrectSoft.Library.EmrEditor.Src.Common;
using DrectSoft.Library.EmrEditor.Src.Gui;
using System;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

#pragma warning disable 0618
namespace DrectSoft.Emr.TemplateFactory
{
    public partial class ConfigFrm : DevBaseForm
    {
        public ConfigFrm()
        {
            InitializeComponent();
        }

        public string LineSpace = string.Empty;

        private XmlDocument m_XmlDocument = new XmlDocument();

        private EmrAppConfig m_EmrAppConfig;

        /// <summary>
        /// 确定事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// 1、add try ... catch
        /// 2、添加提示
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-06</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定使用此默认设置吗？该操作将影响全局。", "设置默认设置", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.Cancel)
                {
                    return;
                }
                LineSpace = Convert.ToString(this.comboBox3.SelectedIndex * 0.5f);
                ZYEditorControl.ElementStyle = this.comboBox4.Text;
                ZYEditorControl.ElementBackColor = this.labelColor.BackColor;

                //将页面设置的值保存到数据库中
                SaveXML();
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                //this.Dispose();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 取消事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-06</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void ConfigFrm_Load(object sender, EventArgs e)
        {

            //加字体
            int selIndex = 0;

            this.comboBox1.Items.AddRange(FontCommon.FontList.ToArray());
            this.comboBox1.SelectedIndex = selIndex;

            //添加字号
            this.comboBox2.DataSource = FontCommon.FontSizeList;
            this.comboBox2.DisplayMember = "FontSizeName";
            this.comboBox2.ValueMember = "FontSize";

            //foreach (string fontSize in allFontSizeName)
            //{
            //    this.comboBox2.Items.Add(fontSize);
            //}


            //读取注册表设置值

            GetDefaultSeting();
            //this.comboBox1.SelectedItem = ZYEditorControl.GetDefaultSettings("fontname");
            //this.comboBox2.Text  = ZYEditorControl.GetDefaultSettings("fontsize");
            //this.comboBox3.SelectedIndex = (int)(float.Parse(ZYEditorControl.GetDefaultSettings("linespace")) * 2);
            //this.comboBox4.SelectedItem = ZYEditorControl.GetDefaultSettings("elestyle");
            //this.labelColor.BackColor = ColorTranslator.FromHtml(ZYEditorControl.GetDefaultSettings("elecolor"));

            if (this.labelColor.BackColor == Color.Transparent)
            {
                this.checkBox1.Checked = true;
            }

        }

        /// <summary>
        /// 选择事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-06</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                using (ColorDialog cdia = new ColorDialog())
                {
                    if (cdia.ShowDialog() == DialogResult.OK)
                    {
                        this.labelColor.BackColor = cdia.Color;
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 恢复默认事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// 1、add try ... catch
        /// 2、添加提示
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-06</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要恢复默认设置吗？该操作将影响全局。", "恢复默认设置", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.Cancel)
                {
                    return;
                }
                this.comboBox1.SelectedItem = "宋体";
                this.comboBox2.Text = "小四";
                this.comboBox3.SelectedItem = "1倍";
                this.comboBox4.SelectedItem = "背景色";
                this.labelColor.BackColor = Color.SkyBlue;
                this.checkBox1.Checked = false;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.button4.Enabled = !this.checkBox1.Checked;

            if (this.checkBox1.Checked)
            {
                this.labelColor.BackColor = Color.Transparent;
            }
            else
            {
                this.labelColor.BackColor = Color.SkyBlue;
            }
        }

        private void GetDefaultSeting()
        {
            AppConfigDalc _AppConfigDalc = new AppConfigDalc();
            m_EmrAppConfig = _AppConfigDalc.SelectAppConfig("EmrDefaultSet");
            //XMLCommon;
            if (m_EmrAppConfig == null)
            {
                button3_Click(null, null);
            }
            m_XmlDocument.LoadXml(m_EmrAppConfig.Config);


            //XMLCommon _XMLCommon = new XMLCommon();
            this.comboBox1.SelectedItem = m_XmlDocument.GetElementsByTagName("fontname").Item(0).InnerText;
            this.comboBox2.Text = m_XmlDocument.GetElementsByTagName("fontsize").Item(0).InnerText;
            this.comboBox3.SelectedIndex = Convert.ToInt32(Convert.ToDouble(m_XmlDocument.GetElementsByTagName("linespace").Item(0).InnerText) * 2);
            this.comboBox4.SelectedItem = m_XmlDocument.GetElementsByTagName("elestyle").Item(0).InnerText;
            this.labelColor.BackColor = ColorTranslator.FromHtml(m_XmlDocument.GetElementsByTagName("elecolor").Item(0).InnerText);
            this.checkBox1.Checked = false;


        }

        /// <summary>
        /// 保存
        /// </summary>
        private void SaveXML()
        {
            //string defaultXML = @"<?xml version="1.0" encoding="UTF-8"?><root><fontname>宋体</fontname><fontsize>9</fontsize><linespace>1</linespace><elestyle>背景色</elestyle><elecolor>Aqua</elecolor></root>";

            XmlNodeList nodeList = m_XmlDocument.SelectSingleNode("root").ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                if (node.Name == "fontname")
                    node.InnerText = this.comboBox1.SelectedItem.ToString();
                else if (node.Name == "fontsize")
                    node.InnerText = this.comboBox2.Text.ToString();
                else if (node.Name == "linespace")
                    node.InnerText = Convert.ToString(this.comboBox3.SelectedIndex * 0.5f);
                else if (node.Name == "elestyle")
                    node.InnerText = this.comboBox4.SelectedItem.ToString();
                else if (node.Name == "elecolor")
                    node.InnerText = ColorTranslator.ToHtml(this.labelColor.BackColor);
            }

            string config = m_XmlDocument.InnerXml;


            m_EmrAppConfig.Config = config;
            m_EmrAppConfig.Valid1 = "1";

            //AppConfigDalc _AppConfigDalc = new AppConfigDalc();
            //_AppConfigDalc.UpdateEmrAppConfig(m_EmrAppConfig);
            IDataAccess _sqlHelper = DataAccessFactory.GetSqlDataAccess();
            OracleConnection conn = new OracleConnection(_sqlHelper.GetDbConnection().ConnectionString);
            OracleCommand cmd = conn.CreateCommand();
            conn.Open();
            cmd.CommandText = "update appcfg set value = :data where configkey ='EmrDefaultSet'";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Clear();
            OracleParameter paraClob = new OracleParameter("data", OracleType.Clob);
            paraClob.Value = config;
            cmd.Parameters.Add(paraClob);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 回车光标后移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void win_KeyPress(object sender, KeyPressEventArgs e)
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

        /// <summary>
        /// 复选框回车事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-06</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chb_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == 13)
                {
                    DS_Common.cbx_KeyPress(sender);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
    }

}
