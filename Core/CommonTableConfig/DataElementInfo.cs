using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using System.Xml;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Common.Ctrs.DLG;
namespace DrectSoft.Core.CommonTableConfig
{
    public partial class DataElementInfo : DevBaseForm
    {
        /// <summary>
        /// edit by xlb 2012-12-29
        /// 全部加上了try catch
        /// </summary>
        public DataElementEntity m_dataElementEntity;
        IEmrHost m_app;
        public DataElementInfo(DataElementEntity dataElementEntity, IEmrHost app)
        {
            try
            {
                m_dataElementEntity = dataElementEntity;
                m_app = app;
                InitializeComponent();
                InitData();
                InitDataElement();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化控件的值
        /// </summary>
        private void InitData()
        {
            try
            {
                cboElementType.DataSource = CommonTabHelper.GetAllDataElementType();
                cboElementType.DisplayMember = "Name";
                cboElementClass.DataSource = CommonTabHelper.GetAllDataElemnetClass();
                cboElementClass.DisplayMember = "Name";
                if (string.IsNullOrEmpty(m_dataElementEntity.ElementFlow))
                {
                    txtElementId.Enabled = true;
                }
                else
                {
                    txtElementId.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// DataElementEntity初始化到界面上
        /// </summary>
        private void InitDataElement()
        {
            try
            {
                txtElementId.Text = m_dataElementEntity.ElementId;
                txtElementName.Text = m_dataElementEntity.ElementName;
                txtElementForm.Text = m_dataElementEntity.ElementForm;
                medOptins.Text = m_dataElementEntity.ElementRange;
                medElementDescribe.Text = m_dataElementEntity.ElementDescribe;

                List<BaseDictory> baseDictoryTyoe = cboElementType.DataSource as List<BaseDictory>;
                foreach (var item in baseDictoryTyoe)
                {
                    if (item.Id == m_dataElementEntity.ElementType)
                    {
                        cboElementType.SelectedItem = item;
                        break;
                    }
                }

                List<BaseDictory> baseDictoryList = cboElementClass.DataSource as List<BaseDictory>;
                foreach (var item in baseDictoryList)
                {
                    if (item.Id == m_dataElementEntity.ElementClass)
                    {
                        cboElementClass.SelectedItem = item;
                        break;
                    }
                }
                chboxIsDataElement.Checked = m_dataElementEntity.IsDataElemet == "1" ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool ValidateInfo(ref string message)
        {
            try
            {
                if (string.IsNullOrEmpty(txtElementId.Text))
                {
                    message = "数据元Id不能为空";
                    txtElementId.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(txtElementName.Text))
                {
                    message = "数据元名称不能为空";
                    txtElementName.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(cboElementType.Text))
                {
                    message = "数据类型不能为空";
                    cboElementType.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(cboElementClass.Text))
                {
                    message = "所属类别不能为空";
                    cboElementClass.Focus();
                    return false;
                }
                else if (!string.IsNullOrEmpty(medOptins.Text))
                {
                    try
                    {
                        XmlDocument xmldoc = new XmlDocument();
                        xmldoc.LoadXml(medOptins.Text);
                    }
                    catch (Exception ex)
                    {
                        message = "XML格式错误。" + ex.Message;
                        medOptins.Focus();
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveDataElement()
        {
            try
            {
                string message = "";
                bool valResult = ValidateInfo(ref message);
                if (valResult == false)
                {
                    m_app.CustomMessageBox.MessageShow(message);
                    return false;
                }
                m_dataElementEntity.ElementId = txtElementId.Text;
                m_dataElementEntity.ElementName = txtElementName.Text;
                m_dataElementEntity.ElementForm = txtElementForm.Text;
                m_dataElementEntity.ElementRange = medOptins.Text;
                m_dataElementEntity.ElementDescribe = medElementDescribe.Text;

                BaseDictory baseitemType = cboElementType.SelectedItem as BaseDictory;
                m_dataElementEntity.ElementType = baseitemType.Id;

                BaseDictory baseitem = cboElementClass.SelectedItem as BaseDictory;
                m_dataElementEntity.ElementClass = baseitem.Id;
                m_dataElementEntity.IsDataElemet = chboxIsDataElement.Checked == true ? "1" : "0";
                DataElemntBiz dataElemntBiz = new DataElemntBiz(m_app);

                bool result = dataElemntBiz.SaveDataElement(m_dataElementEntity, ref message);
                if (!result)
                {
                    m_app.CustomMessageBox.MessageShow(message);
                }
                else
                {
                    m_app.CustomMessageBox.MessageShow("保存成功");
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //类型发生变化时
        //edit by xlb 2012-12-29
        private void cboElementClass_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtElementId.Text))
                {
                    return;
                }
                txtElementId.Text = "DE";
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void hyLoadType_Click(object sender, EventArgs e)
        {
            try
            {
                BaseDictory baseDictory = cboElementType.SelectedItem as BaseDictory;
                if (baseDictory == null)
                {
                    return;
                }
                if (string.IsNullOrEmpty(medOptins.Text))
                {
                    if (baseDictory.Id == "S2" || baseDictory.Id == "S3" || baseDictory.Id == "S9")
                    {
                        SetS2S3S9Xml();
                    }
                    else if (baseDictory.Id == "S1" || baseDictory.Id == "S4")
                    {
                        SetS1S4Xml();
                    }
                    else if (baseDictory.Id == "N")
                    {
                        SetNXml();
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 生成单选或多选XML文本方法
        /// </summary>
        private void SetS2S3S9Xml()
        {
            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml("<ValueRange></ValueRange>");
                XmlElement xmlElement = xdoc.CreateElement("Option");
                xmlElement.InnerText = "选项1";

                XmlAttribute xmlAttribute = xdoc.CreateAttribute("Id");
                xmlAttribute.Value = "01";

                XmlAttribute xmlAttributeDefault = xdoc.CreateAttribute("IsDefault");
                xmlAttributeDefault.Value = "true";


                xmlElement.Attributes.Append(xmlAttribute);
                xmlElement.Attributes.Append(xmlAttributeDefault);


                XmlElement xmlElement2 = xdoc.CreateElement("Option");
                XmlAttribute xmlAttribute2 = xdoc.CreateAttribute("Id");
                xmlAttribute2.Value = "02";
                xmlElement2.InnerText = "选项2";
                xmlElement2.Attributes.Append(xmlAttribute2)
                    ;
                xdoc.DocumentElement.AppendChild(xmlElement as XmlNode);
                xdoc.DocumentElement.AppendChild(xmlElement2 as XmlNode);
                medOptins.Text = xdoc.OuterXml;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 生成文本XML方法
        /// </summary>
        private void SetS1S4Xml()
        {
            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml("<ValueRange></ValueRange>");
                XmlElement xmlElement = xdoc.CreateElement("DefaultValue");
                xmlElement.InnerText = "无";
                xdoc.DocumentElement.AppendChild(xmlElement as XmlNode);
                medOptins.Text = xdoc.OuterXml;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 生成数值XML方法
        /// add by xlb
        /// 2013-01-24
        /// </summary>
        private void SetNXml()
        {
            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml("<ValueRange></ValueRange>");

                XmlElement xmlElement = xdoc.CreateElement("DefaultValue");
                xmlElement.InnerText = "37";

                XmlElement xmlElement2 = xdoc.CreateElement("MinValue");
                xmlElement2.InnerText = "34";

                XmlElement xmlElement3 = xdoc.CreateElement("MaxValue");
                xmlElement3.InnerText = "42";
               
                //XmlElement xmlElement4 = xdoc.CreateElement("StepValue");
                //xmlElement4.InnerText = "1";

                xdoc.DocumentElement.AppendChild(xmlElement as XmlNode);
                xdoc.DocumentElement.AppendChild(xmlElement2 as XmlNode);
                xdoc.DocumentElement.AppendChild(xmlElement3 as XmlNode);
                //xdoc.DocumentElement.AppendChild(xmlElement4 as XmlNode);

                medOptins.Text = xdoc.OuterXml;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void UCDataElementInfo_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

    }
}
