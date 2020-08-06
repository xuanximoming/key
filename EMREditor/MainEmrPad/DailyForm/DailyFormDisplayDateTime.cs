using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Core.MainEmrPad.New;
using DrectSoft.Emr.Util;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace DrectSoft.Core.MainEmrPad.DailyForm
{
    public partial class DailyFormDisplayDateTime : DevBaseForm
    {
        IEmrHost m_App;
        DateTime m_MinDateTime = DateTime.MinValue;
        DateTime m_MaxDateTime = DateTime.MaxValue;
        private EmrModel m_CurrentEmrModel;
        private EmrModel m_FirstEmrModel;
        private TreeList m_TreeList;

        public DailyFormDisplayDateTime()
        {
            InitializeComponent();
        }

        public DailyFormDisplayDateTime(string datetime, IEmrHost app, TreeListNode node)
            : this()
        {
            if (datetime.Split(' ').Length != 2)
            {
                dateEditData.EditValue = System.DateTime.Now.ToString("yyyy-MM-dd");
                timeEditTime.EditValue = System.DateTime.Now.ToString("HH:mm:ss");
            }
            else
            {
                dateEditData.EditValue = datetime.Split(' ')[0];
                timeEditTime.EditValue = datetime.Split(' ')[1];
            }
            m_App = app;
            ComputeDateTimeMinAndMax(node, datetime);
            ShowTip();
        }

        #region 新版文书录入调用 Add by wwj 2013-04-08
        /// <summary>
        /// 
        /// </summary>
        /// <param name="datetime">当前所修改的病历时间</param>
        /// <param name="app"></param>
        /// <param name="firstEmrModel">首次病程</param>
        public DailyFormDisplayDateTime(IEmrHost app, UCEmrInputBody emrInputBody)
            : this()
        {
            if (null == emrInputBody)
            {
                return;
            }
            m_App = app;
            m_CurrentEmrModel = emrInputBody.CurrentModel;
            m_FirstEmrModel = Util.GetFirstDailyEmrModel(emrInputBody.CurrentTreeList);
            m_TreeList = emrInputBody.CurrentTreeList;

            if (null != m_CurrentEmrModel)
            {
                dateEditData.EditValue = m_CurrentEmrModel.DisplayTime.ToString("yyyy-MM-dd");
                timeEditTime.EditValue = m_CurrentEmrModel.DisplayTime.ToString("HH:mm:ss");
            }
            else
            {
                dateEditData.EditValue = System.DateTime.Now.ToString("yyyy-MM-dd");
                timeEditTime.EditValue = System.DateTime.Now.ToString("HH:mm:ss");
            }
            List<EmrModel> modelList = Util.GetAllEmrModels(emrInputBody.CurrentTreeList.Nodes, new List<EmrModel>());
            ComputeDateTime(m_CurrentEmrModel, m_FirstEmrModel, modelList);
        }

        /// <summary>
        /// 设置时间提示信息
        /// </summary>
        /// <param name="currentEmrModel"></param>
        /// <param name="firstEmrModel"></param>
        private void ComputeDateTime(EmrModel currentEmrModel, EmrModel firstEmrModel, List<EmrModel> modelList)
        {
            try
            {
                if (null != currentEmrModel && null != firstEmrModel)
                {
                    if (currentEmrModel.FirstDailyEmrModel)
                    {///修改的病历是首程
                        m_MinDateTime = DS_BaseService.GetInHostTime((int)m_App.CurrentPatientInfo.NoOfFirstPage);
                        this.labelControlTip.Text = "病程时间应大于入院时间 " + m_MinDateTime.ToString("yyyy-MM-dd HH:mm:ss");

                        EmrModel firstModel = modelList.Where(p => !p.FirstDailyEmrModel).OrderByDescending(q => q.DisplayTime).FirstOrDefault();
                        if (null != firstModel)
                        {
                            m_MaxDateTime = firstModel.DisplayTime;
                            this.labelControlTip.Text += "，小于下一个病历时间 " + firstModel.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") + "。";
                        }
                    }
                    else
                    {///修改的病历不是首程
                        m_MinDateTime = firstEmrModel.DisplayTime;
                        this.labelControlTip.Text = "病程时间应大于首次病程时间 " + m_MinDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }
                else
                {
                    m_MinDateTime = DS_BaseService.GetInHostTime((int)m_App.CurrentPatientInfo.NoOfFirstPage);
                    this.labelControlTip.Text = "病程时间应大于入院时间 " + m_MinDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// 計算病程可以修改的時間範圍
        /// </summary>
        /// <param name="node"></param>
        private void ComputeDateTimeMinAndMax(TreeListNode node, string datetime)
        {
            DateTime currentDateTime = Convert.ToDateTime(datetime);

            if (node != null && node.Tag != null)
            {
                EmrModel emrModel = node.Tag as EmrModel;
                if (emrModel != null && emrModel.DailyEmrModel)
                {
                    for (int i = 0; i < node.ParentNode.Nodes.Count; i++)
                    {
                        TreeListNode subNode = node.ParentNode.Nodes[i];
                        EmrModel dailyEmrModel = subNode.Tag as EmrModel;
                        if (dailyEmrModel.DisplayTime < currentDateTime)
                        {
                            if (m_MinDateTime == DateTime.MinValue || m_MinDateTime < dailyEmrModel.DisplayTime)
                            {
                                m_MinDateTime = dailyEmrModel.DisplayTime;
                            }
                        }
                        else if (currentDateTime < dailyEmrModel.DisplayTime)
                        {
                            if (m_MaxDateTime == DateTime.MinValue || dailyEmrModel.DisplayTime < m_MaxDateTime)
                            {
                                m_MaxDateTime = dailyEmrModel.DisplayTime;
                            }
                        }
                    }

                    //首程修改时间限制
                    if (emrModel.FirstDailyEmrModel)
                    {
                        DataTable allRecordsDT = DS_SqlService.GetRecordsByNoofinpatContainDel((int)m_App.CurrentPatientInfo.NoOfFirstPage);
                        var allRecords = allRecordsDT.Select(" 1=1 ");
                        if (null != allRecords && allRecords.Count() > 0)
                        {
                            if (emrModel.InstanceId == -1)
                            {//新增
                                DataRow lastRecord = allRecords.OrderByDescending(p => DateTime.Parse(p["captiondatetime"].ToString())).FirstOrDefault();
                                if (null != lastRecord && null != lastRecord["captiondatetime"] && DateTime.Parse(lastRecord["captiondatetime"].ToString()) > m_MinDateTime)
                                {
                                    m_MinDateTime = DateTime.Parse(lastRecord["captiondatetime"].ToString());
                                }
                            }
                            else
                            {//编辑
                                //比当前病历时间小的最近一个首程或其它科室病历(包含无效)
                                DataRow preOtherRecord = allRecords.Where(p => DateTime.Parse(p["captiondatetime"].ToString()) < emrModel.DisplayTime && (p["departcode"].ToString().Trim() != emrModel.DepartCode || p["firstdailyflag"].ToString().Trim() == "1")).OrderByDescending(q => DateTime.Parse(q["captiondatetime"].ToString())).FirstOrDefault();
                                if (null != preOtherRecord && null != preOtherRecord["captiondatetime"] && DateTime.Parse(preOtherRecord["captiondatetime"].ToString()) > m_MinDateTime)
                                {
                                    m_MinDateTime = DateTime.Parse(preOtherRecord["captiondatetime"].ToString());
                                }
                                //编辑首程不能越过其它有效病历(上限)
                                DataRow preValidRecord = allRecords.Where(p => DateTime.Parse(p["captiondatetime"].ToString()) < emrModel.DisplayTime && int.Parse(p["valid"].ToString()) == 1).OrderByDescending(q => DateTime.Parse(q["captiondatetime"].ToString())).FirstOrDefault();
                                if (null != preValidRecord && null != preValidRecord["captiondatetime"] && DateTime.Parse(preValidRecord["captiondatetime"].ToString()) > m_MinDateTime)
                                {
                                    m_MinDateTime = DateTime.Parse(preValidRecord["captiondatetime"].ToString());
                                }
                                //比当前病历时间大的最近一个首程或其它科室病历
                                DataRow nextOtherRecord = allRecords.Where(p => DateTime.Parse(p["captiondatetime"].ToString()) > emrModel.DisplayTime && (p["departcode"].ToString().Trim() != emrModel.DepartCode || p["firstdailyflag"].ToString().Trim() == "1")).OrderBy(q => DateTime.Parse(q["captiondatetime"].ToString())).FirstOrDefault();
                                if (null != nextOtherRecord && null != nextOtherRecord["captiondatetime"] && (m_MaxDateTime < DateTime.Parse("1900-01-01 00:00:00") || DateTime.Parse(nextOtherRecord["captiondatetime"].ToString()) < m_MaxDateTime))
                                {
                                    m_MaxDateTime = DateTime.Parse(nextOtherRecord["captiondatetime"].ToString());
                                }
                                //编辑首程不能越过其它有效病历(下限)
                                DataRow nextValidRecord = allRecords.Where(p => DateTime.Parse(p["captiondatetime"].ToString()) > emrModel.DisplayTime && int.Parse(p["valid"].ToString()) == 1).OrderBy(q => DateTime.Parse(q["captiondatetime"].ToString())).FirstOrDefault();
                                if (null != nextValidRecord && null != nextValidRecord["captiondatetime"] && (m_MaxDateTime < DateTime.Parse("1900-01-01 00:00:00") || DateTime.Parse(nextValidRecord["captiondatetime"].ToString()) < m_MaxDateTime))
                                {
                                    m_MaxDateTime = DateTime.Parse(nextValidRecord["captiondatetime"].ToString());
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 根据时间范围确定提示信息
        /// edit by Yanqiao.Cai 2013-01-04
        /// 1、add try ... catch
        /// 2、优化提示信息
        /// </summary>
        private void ShowTip()
        {
            try
            {
                if (m_MinDateTime == DateTime.MinValue && m_MaxDateTime != DateTime.MinValue)
                {
                    labelControlTip.Text = "病程记录时间应小于 " + m_MaxDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else if (m_MinDateTime != DateTime.MinValue && m_MaxDateTime == DateTime.MinValue)
                {
                    labelControlTip.Text = "病程记录时间应大于 " + m_MinDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else if (m_MinDateTime != DateTime.MinValue && m_MaxDateTime != DateTime.MinValue)
                {
                    labelControlTip.Text = "病程记录时间应大于 " + m_MinDateTime.ToString("yyyy-MM-dd HH:mm:ss") + " 小于 " + m_MaxDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else if (m_MinDateTime == DateTime.MinValue && m_MaxDateTime == DateTime.MinValue)
                {
                    labelControlTip.Text = "请选择病程记录时间";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 确定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonOK_Click(object sender, EventArgs e)
        {
            if (Check())
            {
                this.DialogResult = DialogResult.Yes;
            }
        }

        /// <summary>
        /// edit by Yanqiao.Cai 2013-01-04
        /// 1、add try ... catch
        /// 2、优化提示信息
        /// </summary>
        private bool Check()
        {
            try
            {

                if (CommitDateTime < m_MinDateTime || CommitDateTime > m_MaxDateTime)
                {///时间范围验证
                    MessageBox.Show(labelControlTip.Text);
                    return false;
                }
                List<EmrModel> allOtherModels = GetAllEmrModels(m_TreeList.Nodes, null);
                if (allOtherModels.Any(p => p.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") == CommitDateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                {
                    MessageBox.Show("病历时间为 " + CommitDateTime.ToString("yyyy-MM-dd HH:mm:ss") + " 的病历已存在，请修改病历时间。");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void simpleButtonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        public DateTime CommitDateTime
        {
            get { return DateTime.Parse(dateEditData.DateTime.Date.ToString("yyyy-MM-dd") + " " + timeEditTime.Time.ToString("HH:mm:ss")); }
        }

        /// <summary>
        /// 获取页面所有病程记录(不包含当前选中的病程)
        /// 注：只针对病程记录
        /// </summary>
        /// <returns></returns>
        public List<EmrModel> GetAllEmrModels(TreeListNodes nodes, List<EmrModel> list)
        {
            try
            {
                if (null == nodes || nodes.Count == 0)
                {
                    return list;
                }
                if (null == list)
                {
                    list = new List<EmrModel>();
                }
                foreach (TreeListNode node in nodes)
                {
                    if (null != node.Tag && node.Tag is EmrModel && node != m_TreeList.FocusedNode)
                    {
                        EmrModel modelevey = (EmrModel)node.Tag;
                        if (modelevey.DailyEmrModel)
                        {
                            list.Add(modelevey);
                        }
                    }
                    else if (null != node.Nodes && node.Nodes.Count > 0)
                    {
                        list = GetAllEmrModels(node.Nodes, list);
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 弃用
        public string GetDiaplayDateTime()
        {
            return dateEditData.DateTime.ToString("yyyy-MM-dd") + " " + timeEditTime.Time.ToString("HH:mm:ss");
        }
        #endregion
    }
}