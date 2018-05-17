using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraTreeList.Nodes;
using DrectSoft.Emr.Util;
using System.Collections;
using DrectSoft.Library.EmrEditor.Src.Document;
using DrectSoft.Common;
using DrectSoft.Service;
using DrectSoft.DSSqlHelper;
using DrectSoft.Common.Eop;
using System.Data.SqlClient;
using System.Data.Common;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Core.MainEmrPad
{
    public partial class DailyTemplateForm : DevBaseForm
    {
        string m_labelControlInfoText = string.Empty;
        string beginTime = string.Empty;
        private int noofInpat = -1;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="lastTime"></param>
        /// <param name="title"></param>
        /// <param name="isFirstDailyEmr">是否是首次病程，对于首次病程没有时间限制</param>
        public DailyTemplateForm(DateTime lastTime, string title, bool isFirstDailyEmr)
        {
            InitializeComponent();
            if (!isFirstDailyEmr)
            {
                labelControl_Info.Text = lastTime.ToString("yyyy-MM-dd HH:mm:ss");
                m_labelControlInfoText = labelControl_Info.Text;
                this.labelControl_Info.Text = "病程时间应大于 " + m_labelControlInfoText;
                this.labelControl_Info.ToolTip = "病程时间应大于 " + m_labelControlInfoText;
            }
            else
            {
                labelControl_Info.Text = "";
                labelControl_Info.ToolTip = "";
                labelControl3.Text = "";
                m_labelControlInfoText = "";
            }
            dateEdit_Date.DateTime = DateTime.Now.Date;
            timeEdit_Time.Time = DateTime.Now;
            textEdit_Name.Text = title;
        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="lastTime"></param>
        /// <param name="title"></param>
        /// <param name="isFirstDailyEmr">是否是首次病程，对于首次病程没有时间限制</param>
        public DailyTemplateForm(DateTime lastTime, string title, bool isFirstDailyEmr,List<EmrModel> modelList,int noofinpat)
        {
            try
            {
                InitializeComponent();
                noofInpat = noofinpat;
                m_labelControlInfoText = lastTime.ToString("yyyy-MM-dd HH:mm:ss");

                if (null != modelList && modelList.Count() > 0)
                {
                    beginTime = GetDailyBeginTime(noofinpat, modelList, isFirstDailyEmr);
                }

                if (!string.IsNullOrEmpty(m_labelControlInfoText))
                {
                    labelControl3.Visible = true;
                    if (!string.IsNullOrEmpty(beginTime) && DateTime.Parse(beginTime) > lastTime)
                    {
                        m_labelControlInfoText = beginTime;
                        this.labelControl_Info.Text = "病程时间应大于 " + m_labelControlInfoText;
                        this.labelControl_Info.ToolTip = "病程时间应大于 " + m_labelControlInfoText;
                    }
                    else
                    {
                        this.labelControl_Info.Text = "病程时间应大于 " + m_labelControlInfoText;
                        this.labelControl_Info.ToolTip = "病程时间应大于 " + m_labelControlInfoText;
                    }
                }
                else
                {
                    labelControl_Info.Text = "";
                    labelControl_Info.ToolTip = "";
                    labelControl3.Visible = false;
                    m_labelControlInfoText = "";
                }

                dateEdit_Date.DateTime = DateTime.Now.Date;
                timeEdit_Time.Time = DateTime.Now;
                textEdit_Name.Text = title;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        public DateTime CommitDateTime
        {
            get { return DateTime.Parse(dateEdit_Date.DateTime.Date.ToString("yyyy-MM-dd")+" "+timeEdit_Time.Time.ToString("HH:mm:ss")); }
        }

        public string CommitTitle
        {
            get { return textEdit_Name.Text; }
        }

        private void BindBasicInfo()
        {
            //


        }

        /// <summary>
        /// 画面检查
        /// edit by Yanqiao.Cai 2012-11-23
        /// 1、add try ... catch
        /// 2、优化提示
        /// </summary>
        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_AllDisplayDateTime.Contains(CommitDateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                {
                    this.labelControl_Info.Text = "该病程时间已存在，请选择其它时间。";
                    this.labelControl_Info.ToolTip = "该病程时间已存在，请选择其它时间。";
                    dateEdit_Date.Focus();
                    return;
                }

                if (m_labelControlInfoText != "" && (CommitDateTime - Convert.ToDateTime(m_labelControlInfoText)).TotalMinutes < 0)
                {
                    if (!string.IsNullOrEmpty(beginTime) && DateTime.Parse(beginTime) == DateTime.Parse(m_labelControlInfoText))
                    {
                        this.labelControl_Info.Text = "病程时间应大于 " + m_labelControlInfoText;
                        this.labelControl_Info.ToolTip = "病程时间应大于 " + m_labelControlInfoText;
                    }
                    else
                    {
                        this.labelControl_Info.Text = "病程时间应大于 " + m_labelControlInfoText;
                        this.labelControl_Info.ToolTip = "病程时间应大于 " + m_labelControlInfoText;
                    }
                    dateEdit_Date.Focus();
                    return;
                }
                //注释 by cyq 2013-01-04 修复历史病历导入后无法新增病历的bug
                //else if (CommitDateTime > DateTime.Now)
                //{
                //    this.labelControl_Info.Text = "病程时间不能大于当前时间";
                //    this.labelControl_Info.ToolTip = "病程时间不能大于当前时间";
                //    dateEdit_Date.Focus();
                //    return;
                //}
                else
                {
                    //查看数据库中是否存在该时间的有效病历
                    ///1、查找所有病人的有效病历
                    DataTable theRecords = DS_SqlService.GetRecordsByNoofinpat(noofInpat);
                    if (null != theRecords && theRecords.Rows.Count > 0 && theRecords.Select(" 1=1 ").Any(p => DateTime.Parse(p["captiondatetime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") == CommitDateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("数据库中已存在时间为 " + CommitDateTime.ToString("yyyy-MM-dd HH:mm:ss") + " 的病历，请选择其它的时间。");
                        return;
                    }
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private List<string> m_AllDisplayDateTime = new List<string>();

        /// <summary>
        /// 设置病程的所有时间
        /// </summary>
        /// <param name="node"></param>
        public void SetAllDisplayDateTime(PadForm padForm)
        {
            try
            {
                if (padForm != null)
                {
                    ArrayList al = new ArrayList();
                    padForm.zyEditorControl1.EMRDoc.GetAllSpecElement(al, padForm.zyEditorControl1.EMRDoc.RootDocumentElement, ElementType.Text, "记录日期");
                    if (al.Count > 0)
                    {
                        foreach (ZYText ele in al)
                        {
                            m_AllDisplayDateTime.Add(DateTime.Parse(ele.Text).ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 设置病程的所有时间
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-08</date>
        /// <param name="node">当前节点</param>
        public void SetAllDisplayDateTime(TreeListNode node)
        {
            try
            {
                if (null == node || null == node.ParentNode)
                {
                    return;
                }
                var nodeList = node.ParentNode.Nodes;
                if (null == nodeList || nodeList.Count == 0)
                {
                    return;
                }
                foreach (TreeListNode nd in nodeList)
                {
                    if (null != nd.Tag && nd.Tag is EmrModel)
                    {
                        EmrModel model = (EmrModel)nd.Tag;
                        if (null == model)
                        {
                            continue;
                        }
                        m_AllDisplayDateTime.Add(model.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                }
                m_AllDisplayDateTime = m_AllDisplayDateTime.OrderBy(q => DateTime.Parse(q)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 回车切换焦点
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-11-23</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Win_KeyPress(object sender, KeyPressEventArgs e)
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
        /// 获取病历开始时间
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <param name="modelList">编辑器传递节点集合</param>
        /// <param name="isFirstDailyEmr">是否首程</param>
        /// <returns></returns>
        private string GetDailyBeginTime(int noofinpat, List<EmrModel> modelList, bool isFirstDailyEmr)
        {
            #region 已注释 by cyq 2013-01-17
            /**
            try
            {
                string starttime = string.Empty;
                //获取该病人的所有病历
                DataTable dialyDt = YD_SqlService.GetRecordsByNoofinpat(noofinpat);
                if (null == dialyDt && dialyDt.Rows.Count == 0)
                {
                    return starttime;
                }
                //获取该病人所有首次病程
                var firstDailyRecords = dialyDt.Select(" firstdailyflag = 1 ");
                if (null == firstDailyRecords && firstDailyRecords.Count() == 0)
                {//不存在首次病程
                    return starttime;
                }
                //当前(科室)首次病程(最后一个)
                DataRow theFirstDialy = firstDailyRecords.Where(p => p["departcode"].ToString().Trim() == YD_Common.currentUser.CurrentDeptId).OrderByDescending(q => DateTime.Parse(q["captiondatetime"].ToString())).FirstOrDefault();
                if (null == theFirstDialy)
                {
                    DataRow preFirstRecord = firstDailyRecords.Where(p => p["departcode"].ToString() != YD_Common.currentUser.CurrentDeptId).OrderByDescending(q => DateTime.Parse(q["captiondatetime"].ToString())).FirstOrDefault();
                    if (null == preFirstRecord)
                    {
                        return starttime;
                    }
                    else
                    {
                        DataRow otherDialy = dialyDt.Select(" 1=1 ").Where(p => p["departcode"].ToString() != YD_Common.currentUser.CurrentDeptId).OrderByDescending(q => q["captiondatetime"]).FirstOrDefault();
                        if (null != otherDialy)
                        {
                            starttime = otherDialy["captiondatetime"].ToString();
                        }
                        return otherDialy["captiondatetime"].ToString();
                    }
                }
                //最后一个当前科室的首程不是最后一个首程(即转回当前科室却没有添加首程)
                if (firstDailyRecords.Any(p => DateTime.Parse(p["captiondatetime"].ToString()) > DateTime.Parse(theFirstDialy["captiondatetime"].ToString())))
                {
                    DataRow lastRow = dialyDt.Select(" 1=1 ").OrderByDescending(p => DateTime.Parse(p["captiondatetime"].ToString())).FirstOrDefault();
                    if (null != lastRow)
                    {
                        return lastRow["captiondatetime"].ToString();
                    }
                }
                string currentTime = theFirstDialy["captiondatetime"].ToString();
                //获取当前首程之前的第一个非本科室的病历
                DataRow preDialy = dialyDt.Select(" 1=1 ").Where(p => p["departcode"].ToString() != YD_Common.currentUser.CurrentDeptId && DateTime.Parse(p["captiondatetime"].ToString()) <= DateTime.Parse(currentTime)).OrderByDescending(q => q["captiondatetime"]).FirstOrDefault();
                if (null != preDialy)
                {
                    starttime = preDialy["captiondatetime"].ToString();
                }
                return starttime;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            **/
            #endregion

            try
            {
                string starttime = string.Empty;
                //该病人当前所属科室
                string currentDeptID = string.Empty;
                DataTable inpatientDt = DS_SqlService.GetInpatientByID(noofinpat);
                if (null != inpatientDt && inpatientDt.Rows.Count > 0)
                {
                    currentDeptID = null == inpatientDt.Rows[0]["outhosdept"] ? "" : inpatientDt.Rows[0]["outhosdept"].ToString();
                }
                if (string.IsNullOrEmpty(currentDeptID))
                {
                    currentDeptID = DS_Common.currentUser.CurrentDeptId;
                }
                //该病人的所有病历(页面节点)
                if (null == modelList && modelList.Count() == 0)
                {
                    return starttime;
                }
                //最后一个病历
                EmrModel lastRecord = modelList.OrderByDescending(q => q.DisplayTime).FirstOrDefault();
                if (null == lastRecord)
                {//不存在病历
                    return starttime;
                }
                //获取该病人所有首次病程
                var firstDailyRecords = modelList.Where(p => p.FirstDailyEmrModel);
                if (null == firstDailyRecords && firstDailyRecords.Count() == 0)
                {//不存在首次病程
                    return starttime;
                }
                if (lastRecord.DepartCode != currentDeptID)
                {
                    return lastRecord.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
                //当前(科室)首次病程(最后一个)
                EmrModel theFirstDialy = firstDailyRecords.Where(p => p.DepartCode == currentDeptID).OrderByDescending(q => q.DisplayTime).FirstOrDefault();
                if (null != theFirstDialy)
                {
                    return theFirstDialy.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss");
                }

                return starttime;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
