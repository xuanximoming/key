using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Common.Eop;
using DrectSoft.Emr.Util;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace DrectSoft.Core.MainEmrPad.HistoryEMR
{
    public partial class HistoryEmrBatchInForm : DevBaseForm
    {
        private Inpatient m_CurrentInpatient;

        private IEmrHost m_App;

        private RecordDal m_RecordDal;

        private PatRecUtil m_patUtil;

        HistoryEMRBLL m_HistoryEmrBll;

        public HistoryEmrBatchInForm(IEmrHost app, RecordDal recordDal, Inpatient currentInpatient, PatRecUtil patUtil)
        {
            try
            {
                InitializeComponent();
                m_App = app;
                m_RecordDal = recordDal;
                m_CurrentInpatient = currentInpatient;
                m_patUtil = patUtil;
                RegisterEvent();//建立事件机制 xlb 2013-01-12
                InitDateEdit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 加载事件
        /// edit by xlb 2013-01-04
        /// try catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HistoryEmrBatchInForm_Load(object sender, EventArgs e)
        {
            try
            {
                InitDataHistory(null, null);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewHistoryEmr_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridHitInfo hitinfo = gridViewHistoryEmr.CalcHitInfo(gridControlHistoryEmr.PointToClient(Cursor.Position));
                if (hitinfo.RowHandle >= 0)
                {
                    EMRBatchIn();
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 确定事件
        /// xlb 2013-01-11
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                EMRBatchIn();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 取消事件
        /// xlb 2013-01-11
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
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
        /// 批量导入病历
        /// edit by Yanqiao.Cai 2012-12-18 
        /// 1、add try ... catch
        /// 2、优化提示
        /// xlb 2013-01-12
        /// </summary>
        /// <param name="noofinpat"></param>
        private void EMRBatchIn()
        {
            try
            {
                //转科限制 edit by cyq 2013-03-01
                string checkDept = CheckIfCurrentDept(int.Parse(m_CurrentInpatient.NoOfFirstPage.ToString()));
                if (!string.IsNullOrEmpty(checkDept))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(checkDept);
                    return;
                }

                DataRow dr = gridViewHistoryEmr.GetDataRow(gridViewHistoryEmr.FocusedRowHandle);
                if (dr != null)
                {
                    #region
                    //if (m_App.CustomMessageBox.MessageShow("您确定要导入病历吗？", DrectSoft.Core.CustomMessageBoxKind.QuestionYesNo) == System.Windows.Forms.DialogResult.Yes)
                    //{
                    //    string noofinpat = dr["noofinpat"].ToString();
                    //    if (!string.IsNullOrEmpty(noofinpat))
                    //    {
                    //        HistoryEmrBatchIn bachIn = new HistoryEmrBatchIn(noofinpat, m_CurrentInpatient, m_App, m_RecordDal, m_patUtil);
                    //        bachIn.ExecuteBatchIn();
                    //        RefreshEMRMainPad();
                    //        this.Close();
                    //        m_App.CustomMessageBox.MessageShow("导入历史病历成功", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                    //    }
                    //}
                    //else
                    //{
                    //    return;
                    //}
                    #endregion
                    if (MyMessageBox.Show("您确定要导入病历吗？", "提示", MyMessageBoxButtons.OkCancel, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.QuestionIcon) == DialogResult.Cancel)
                    {
                        return;
                    }
                    string noofinpat = dr["noofinpat"].ToString();
                    if (string.IsNullOrEmpty(noofinpat))
                    {
                        return;
                    }
                    HistoryEmrBatchIn bachIn = new HistoryEmrBatchIn(noofinpat, m_CurrentInpatient, m_App, m_RecordDal, m_patUtil);
                    bool boo = bachIn.ExecuteBatchIn();
                    if (boo)
                    {
                        RefreshEMRMainPad();
                        this.Close();
                        MyMessageBox.Show("历史病历导入成功", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.InformationIcon);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 重新加载整个文书录入界面
        /// </summary>
        private void RefreshEMRMainPad()
        {
            try
            {
                m_App.ChoosePatient(m_CurrentInpatient.NoOfFirstPage);
                m_App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 初始化方法
        /// by xlb 2013-01-04
        /// </summary>
        private void InitDateEdit()
        {
            try
            {
                dateEditBegin.EditValue = DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd");
                dateEditEnd.EditValue = DateTime.Now.ToString("yyyy-MM-dd");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 建立事件触发的方法
        /// xlb 2013-01-11
        /// </summary>
        private void RegisterEvent()
        {
            try
            {
                gridControlHistoryEmr.DoubleClick += new EventHandler(gridViewHistoryEmr_DoubleClick);//add by xlb 2013-01-04
                btnOk.Click += new EventHandler(btnOk_Click);
                btnCancel.Click += new EventHandler(btnCancel_Click);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// add by xlb 20123-01-04
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                Search();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 重置事件
        /// add by xlb 2013-01-04
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                InitDateEdit();
                InitDataHistory(dateEditBegin.Text.ToString(), dateEditEnd.Text.ToString());
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 查询方法
        /// xlb 2013-01-16
        /// </summary>
        private void Search()
        {
            try
            {
                string dateEnd = dateEditEnd.Text.ToString();
                string dateBegin = dateEditBegin.Text.ToString();
                if (dateEditBegin.DateTime > dateEditEnd.DateTime)
                {
                    MyMessageBox.Show("开始日期不能大于结束日期", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                    dateEditBegin.Focus();
                    return;
                }
                else if (dateEditBegin.DateTime > DateTime.Now)
                {
                    MyMessageBox.Show("开始日期不能大于当前日期", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                    dateEditBegin.Focus();
                    return;
                }
                else if (dateEditEnd.DateTime > DateTime.Now)
                {
                    MyMessageBox.Show("结束日期不能大于当前日期", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                    dateEditEnd.Focus();
                    return;
                }

                DataTable dt = m_HistoryEmrBll.GetHistoryInpatient((int)m_CurrentInpatient.NoOfFirstPage, dateBegin, dateEnd);
                gridControlHistoryEmr.DataSource = dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化历史病人记录
        /// xlb 2013-01-16
        /// </summary>
        private void InitDataHistory(string dateBegin, string dateEnd)
        {
            try
            {
                m_HistoryEmrBll = new HistoryEMRBLL(m_App, m_CurrentInpatient, m_RecordDal);
                DataTable dt = m_HistoryEmrBll.GetHistoryInpatient((int)m_CurrentInpatient.NoOfFirstPage, dateBegin, dateEnd);
                gridControlHistoryEmr.DataSource = dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 检查病人当前科室与医生科室(包含权限科室)是否一致
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-01</date>
        /// <param name="noofinpat">首页序号</param>
        /// <returns></returns>
        private string CheckIfCurrentDept(int noofinpat)
        {
            try
            {
                DataTable inpDt = DS_SqlService.GetInpatientByID(noofinpat, 2);
                if (null == inpDt || inpDt.Rows.Count == 0)
                {
                    return "该病人不存在，请刷新数据重试。";
                }
                string dept = null == inpDt.Rows[0]["outhosdept"] ? "" : inpDt.Rows[0]["outhosdept"].ToString().Trim();
                if (dept != DS_Common.currentUser.CurrentDeptId)
                {//该病人已转科
                    if (string.IsNullOrEmpty(dept.Trim()))
                    {
                        return "该病人所属科室异常，请联系管理员。";
                    }
                    string deptName = DS_BaseService.GetDeptNameByID(dept);
                    List<string[]> list = DS_BaseService.GetDeptAndWardInRight(DS_Common.currentUser.Id);
                    if (null != list && list.Count > 0 && list.Any(p => p[0] == dept))
                    {//转科后科室在医生权限范围内
                        return "该病人已转至 " + deptName + "(" + dept + ")" + "，请切换科室。";
                    }
                    else
                    {
                        return "该病人已转至 " + deptName + "(" + dept + ")" + "，您无权操作。";
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}