using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Emr.Util;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace DrectSoft.Core.MainEmrPad.New
{
    public partial class DailyTemplateForm : DevBaseForm
    {
        private IEmrHost m_app;
        private DateTime m_BeginTime;
        private List<EmrModel> modelList;
        private UCEmrInputBody m_UCEmrInputBody;
        private EmrModel m_EmrModel;

        public DateTime CommitDateTime
        {
            get { return DateTime.Parse(dateEdit_Date.DateTime.Date.ToString("yyyy-MM-dd") + " " + timeEdit_Time.Time.ToString("HH:mm:ss")); }
        }
        public string CommitTitle
        {
            get { return textEdit_Name.Text; }
        }

        #region 构造器
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public DailyTemplateForm(IEmrHost app)
        {
            try
            {
                m_app = app;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="model"></param>
        /// <param name="firstDailyTime">首次病程时间</param>
        public DailyTemplateForm(IEmrHost app, EmrModel model, DateTime? firstDailyTime, List<EmrModel> list, UCEmrInputBody inputBody)
        {
            try
            {
                InitializeComponent();
                m_app = app;
                m_EmrModel = model;
                modelList = list;
                m_UCEmrInputBody = inputBody;
                Init(model, firstDailyTime, list);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init(EmrModel model, DateTime? firstDailyTime, List<EmrModel> list)
        {
            try
            {
                string titleName = model.IsShowFileName == "1" ? (string.IsNullOrEmpty(model.FileName.Trim()) ? model.Description.Trim() : model.FileName.Trim()) : string.Empty;
                if (null == firstDailyTime)
                {
                    DataTable dt = DS_SqlService.GetInpatientByID((int)m_app.CurrentPatientInfo.NoOfFirstPage);
                    if (null != dt && dt.Rows.Count > 0)
                    {
                        string config = DS_SqlService.GetConfigValueByKey("EmrInputConfig");
                        if (!string.IsNullOrEmpty(config))
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(config);
                            XmlNodeList nodeList = doc.GetElementsByTagName("InHosTimeType");
                            if (null != nodeList && nodeList.Count > 0)
                            {
                                string cfgValue = null == nodeList[0].InnerText ? "" : nodeList[0].InnerText.Trim();
                                if (cfgValue == "0" && !string.IsNullOrEmpty(dt.Rows[0]["admitdate"].ToString().Trim()))
                                {//入院
                                    m_BeginTime = DateTime.Parse(dt.Rows[0]["admitdate"].ToString());
                                }
                                else if (!string.IsNullOrEmpty(dt.Rows[0]["inwarddate"].ToString().Trim()))
                                {//入科
                                    m_BeginTime = DateTime.Parse(dt.Rows[0]["inwarddate"].ToString());
                                }
                            }
                            else
                            {
                                m_BeginTime = (null != dt.Rows[0]["inwarddate"] && dt.Rows[0]["inwarddate"].ToString().Trim() != "") ? DateTime.Parse(dt.Rows[0]["inwarddate"].ToString()) : DateTime.Parse(dt.Rows[0]["admitdate"].ToString());
                            }
                        }
                        this.labelControl_Info.Text = "病程时间应大于入院时间 " + m_BeginTime.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }
                else
                {
                    m_BeginTime = (DateTime)firstDailyTime;
                    this.labelControl_Info.Text = "病程时间应大于首次病程时间 " + m_BeginTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
                dateEdit_Date.DateTime = DateTime.Now.Date;
                timeEdit_Time.Time = DateTime.Now;
                textEdit_Name.Text = titleName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                string errorStr = CheckItem();
                if (!string.IsNullOrEmpty(errorStr))
                {
                    MyMessageBox.Show(errorStr, "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                    return;
                }

                //Add by wwj 2013-04-27
                m_EmrModel.IsShowDailyEmrPreView = checkEditIsShowDailyEmrPreView.Checked ? true : false;

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 取消事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Cancel_Click(object sender, EventArgs e)
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

        /// <summary>
        /// 画面检查
        /// </summary>
        /// <returns></returns>
        private string CheckItem()
        {
            try
            {
                if (string.IsNullOrEmpty(this.dateEdit_Date.Text))
                {
                    return "请选择病程日期";
                }
                else if (string.IsNullOrEmpty(this.timeEdit_Time.Text))
                {
                    return "请选择病程时间";
                }
                else if (CommitDateTime < m_BeginTime)
                {
                    return this.labelControl_Info.Text;
                }
                else
                {
                    if (null != modelList && modelList.Any(p => p.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") == CommitDateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                    {
                        return "该病程时间已存在，请选择其它时间。";
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DailyTemplateForm_Load(object sender, EventArgs e)
        {
            InitCheckEdit();
        }

        private void InitCheckEdit()
        {
            try
            {
                //checkEditIsShowDailyEmrPreView.Visible = false;
                //labelControlShowDailyEmrPreView.Visible = false;

                //Add By wwj 2013-04-27
                Dictionary<string, UCEmrInputPreView> collection = m_UCEmrInputBody.TempDailyPreViewCollection;
                //判断该病历对应的病程预览区有没有保存在TempDailyPreViewCollection，如果已经在集合中保存则不显示
                if (collection.ContainsKey(m_EmrModel.DeptChangeID))
                {
                    checkEditIsShowDailyEmrPreView.Visible = false;
                    labelControlShowDailyEmrPreView.Visible = false;
                }
                else
                {
                    string config = DS_SqlService.GetConfigValueByKey("EmrSetting");
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(config);
                    XmlNode node = doc.SelectSingleNode("Emr/IsShowDailyPreViewWhenNewDailyEmr");
                    string isCheck = null != node ? node.InnerText : string.Empty;
                    if (!string.IsNullOrEmpty(isCheck.Trim()) && isCheck == "1")
                    {
                        checkEditIsShowDailyEmrPreView.Checked = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
