using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DrectSoft.Core.TimeLimitQC;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Service;

namespace DrectSoft.Core.TimeLimitQC
{
    /// <summary>
    /// 时限显示组件
    /// </summary>
    public partial class UserCtrlTimeQcInfo : UserControl
    {
        #region fields

        const string CstTimeLimitInfoDataSetName = "DataSetTimeLimitInfo";
        const string CstTimeLimitInfoDataTableName = "DataTableTimeLimitInfo";
        Qcsv _qcsv;

        public IEmrHost App { get; set; }

        #endregion

        #region ctor

        /// <summary>
        /// 构造
        /// </summary>
        public UserCtrlTimeQcInfo()
        {
            InitializeComponent();
            advBandedGridView1.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(advBandedGridView1_RowCellStyle);
            SetHeaderCenter();
            this.ActiveControl = simpleButtonShouSuo;
        }
        #endregion

        #region properties

        #endregion

        #region event handler

        void advBandedGridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.RowHandle != advBandedGridView1.FocusedRowHandle
                && e.Column.FieldName == "tipwarninfo")
            {
                int tip = (int)advBandedGridView1.GetRowCellValue(e.RowHandle, advBandedGridView1.Columns["tipstatus"]);
                RuleRecordState recState = (RuleRecordState)tip;
                switch (recState)
                {
                    case RuleRecordState.None:
                    case RuleRecordState.DoIntime:
                    case RuleRecordState.UndoIntime:
                        e.Appearance.ForeColor = Color.Teal;
                        break;
                    case RuleRecordState.DoOuttime:
                    case RuleRecordState.UndoOuttime:
                        e.Appearance.ForeColor = Color.Red;
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion

        #region procedures

        void AddDataColumn(DataTable table, string fieldName,
            string colName, Type datatype)
        {
            DataColumn dcHzxm = new DataColumn(fieldName, datatype);
            dcHzxm.Caption = colName;
            table.Columns.Add(dcHzxm);
        }

        DataSet CreateInfoData()
        {
            DataSet timeLimitInfo = new DataSet(CstTimeLimitInfoDataSetName);

            DataTable subDoctorInfo = new DataTable(CstTimeLimitInfoDataTableName);
            AddDataColumn(subDoctorInfo, ConstRes.cstFieldDoctorId, ConstRes.cstFieldCaptionDoctorId, typeof(string));
            AddDataColumn(subDoctorInfo, ConstRes.cstFieldFirstPage, ConstRes.cstFieldCaptionFirstPage, typeof(int));
            AddDataColumn(subDoctorInfo, ConstRes.cstFieldPatNameHospNo, ConstRes.cstFieldCaptionPatNameHospNo, typeof(string));
            AddDataColumn(subDoctorInfo, ConstRes.cstFieldInfo, ConstRes.cstFieldCaptionInfo, typeof(string));
            AddDataColumn(subDoctorInfo, ConstRes.cstFieldTipStatus, ConstRes.cstFieldCaptionTipStatus, typeof(int));
            AddDataColumn(subDoctorInfo, ConstRes.cstFieldTimeLimit, ConstRes.cstFieldCaptionTimeLimit, typeof(string));
            timeLimitInfo.Tables.Add(subDoctorInfo);

            return timeLimitInfo;
        }

        void SetGridView(DataSet infos, bool groupbypat)
        {
            for (int i = 0; i < advBandedGridView1.Columns.Count; i++)
            {
                string fieldName = advBandedGridView1.Columns[i].FieldName;
                string caption = infos.Tables[CstTimeLimitInfoDataTableName].Columns[fieldName].Caption;
                advBandedGridView1.Columns[i].Caption = caption;
                if (advBandedGridView1.Columns[i] != advBandedGridView1.Columns["doctorid"])
                    advBandedGridView1.Columns[i].OwnerBand = gridBand1;
            }

            advBandedGridView1.Columns[ConstRes.cstFieldDoctorId].OwnerBand = gridBand2;

            advBandedGridView1.Columns[ConstRes.cstFieldFirstPage].Visible = false;
            advBandedGridView1.Columns[ConstRes.cstFieldTipStatus].Visible = false;
            advBandedGridView1.Columns[ConstRes.cstFieldInfo].RowIndex = 1;
            advBandedGridView1.Columns[ConstRes.cstFieldInfo].ColVIndex = 0;
            advBandedGridView1.Columns[ConstRes.cstFieldTimeLimit].RowIndex = 1;
            advBandedGridView1.Columns[ConstRes.cstFieldTimeLimit].ColVIndex = 1;
            advBandedGridView1.Columns[ConstRes.cstFieldTimeLimit].MinWidth = 115;

            if (groupbypat)
            {
                advBandedGridView1.Columns[ConstRes.cstFieldDoctorId].RowCount = 2;
                gridBand2.Visible = false;
                advBandedGridView1.Columns[ConstRes.cstFieldDoctorId].Visible = false;
                advBandedGridView1.Columns[ConstRes.cstFieldPatNameHospNo].Visible = false;
                advBandedGridView1.Columns[ConstRes.cstFieldPatNameHospNo].Group();
            }
            else
            {
                advBandedGridView1.Columns[ConstRes.cstFieldPatNameHospNo].Visible = false;
                advBandedGridView1.Columns[ConstRes.cstFieldDoctorId].Group();
            }
            advBandedGridView1.ExpandAllGroups();
        }

        /// <summary>
        /// 查看医生的时限信息
        /// </summary>
        /// <param name="doctorId">需要查看时限信息的医生代码</param>
        /// <param name="includeSub">是否包含下级医生的时限信息</param>
        public void CheckDoctorTime(string doctorId, bool includeSub)
        {
            DataSet infos = CreateInfoData();
            _qcsv = new Qcsv();
            DataTable dtTimeLimit = _qcsv.SelectDoctorRuleRecords(doctorId);
            SetGridViewDisplay(infos, dtTimeLimit);
            SetGridView(infos, true);
            m_DoctorID = doctorId;
            m_IncludeSub = includeSub;
            m_MethodName = "CheckDoctorTime".ToUpper();
            SetHeaderCenter();
        }

        /// <summary>
        /// 查看病人的相关时限信息
        /// </summary>
        /// <param name="patid">病人id(住院首页序号)</param>
        public void CheckPatientTime(int patid)
        {
            DataSet infos = CreateInfoData();
            _qcsv = new Qcsv();
            DataTable dtTimeLimit = _qcsv.SelectPatientRuleRecords(patid);
            SetGridViewDisplay(infos, dtTimeLimit);
            SetGridView(infos, false);
            m_PatID = patid;
            m_MethodName = "CheckPatientTime".ToUpper();
        }

        void SetGridViewDisplay(DataSet infos, DataTable dtTimeLimit)
        {
            for (int i = 0; i < dtTimeLimit.Rows.Count; i++)
            {
                DataRow row = dtTimeLimit.Rows[i];
                RuleRecordState recordstate = (RuleRecordState)int.Parse(row[ConstRes.cstFieldTipStatus2].ToString());
                DateTime conditionTime = DateTime.Parse(row[ConstRes.cstFieldCondTime].ToString());
                if (recordstate == RuleRecordState.None ||
                    recordstate == RuleRecordState.DoIntime ||
                    conditionTime > DateTime.Now)
                    continue;
                DataRow inforow = infos.Tables[CstTimeLimitInfoDataTableName].NewRow();
                inforow[ConstRes.cstFieldDoctorId] = row[ConstRes.cstFieldDoctorName].ToString().Trim() + "(" + row[ConstRes.cstFieldDoctorId2].ToString().Trim() + ")";
                inforow[ConstRes.cstFieldFirstPage] = row[ConstRes.cstFieldFirstPage];
                inforow[ConstRes.cstFieldPatNameHospNo] = row[ConstRes.cstFieldPatName].ToString().Trim();// + "(" + row[ConstRes.cstFieldPatHospNo].ToString().Trim() + ")";
                //预算与今日的差距
                DateTime dtResultTime = DateTime.Parse(row[ConstRes.cstFieldResultTime].ToString());
                DateTime dtConditionTime = DateTime.Parse(row[ConstRes.cstFieldCondTime].ToString());
                TimeSpan tsTimeLimit = (TimeSpan)(dtResultTime - dtConditionTime);
                double dblTimeLimit = double.Parse(row[ConstRes.cstFieldTimeLimit].ToString());
                TimeSpan tsDefTimeLimit = new TimeSpan(0, 0, 0, Convert.ToInt32(dblTimeLimit));
                switch (recordstate)
                {
                    case RuleRecordState.UndoIntime:
                        tsTimeLimit = DateTime.Now - dtConditionTime;
                        if (tsTimeLimit > tsDefTimeLimit)
                            recordstate = RuleRecordState.UndoOuttime;
                        break;
                    case RuleRecordState.DoIntime:
                        break;
                    case RuleRecordState.UndoOuttime:
                        break;
                    case RuleRecordState.DoOuttime:
                        break;
                    default:
                        break;
                }
                if (tsTimeLimit > tsDefTimeLimit)
                {
                    inforow[ConstRes.cstFieldTimeLimit] = ConstRes.cstOverTime + TimeSpan2Chn(tsTimeLimit - tsDefTimeLimit);
                }
                else
                {
                    inforow[ConstRes.cstFieldTimeLimit] = ConstRes.cstOnTime + TimeSpan2Chn(tsDefTimeLimit - tsTimeLimit);
                }
                //edit by cyq 2012-11-08 去掉警告/提示信息中开头的“警告：”、"提示："、"病人"字眼
                if (recordstate == RuleRecordState.UndoIntime)
                {
                    //inforow[ConstRes.cstFieldInfo] = row[ConstRes.cstFieldTipInfo];
                    string infoStr = null == row[ConstRes.cstFieldTipInfo] ? "" : row[ConstRes.cstFieldTipInfo].ToString();
                    if (infoStr.StartsWith("提示：病人"))
                    {
                        infoStr = infoStr.Substring(3, infoStr.Length - 5);
                    }
                    else if (infoStr.StartsWith("提示："))
                    {
                        infoStr = infoStr.Substring(3, infoStr.Length - 3);
                    }
                    else if (infoStr.StartsWith("病人"))
                    {
                        infoStr = infoStr.Substring(3, infoStr.Length - 2);
                    }
                    inforow[ConstRes.cstFieldInfo] = infoStr;
                }
                if (recordstate == RuleRecordState.DoOuttime || recordstate == RuleRecordState.UndoOuttime)
                {
                    //inforow[ConstRes.cstFieldInfo] = row[ConstRes.cstFieldWarnInfo];
                    string infoStr = null == row[ConstRes.cstFieldWarnInfo] ? "" : row[ConstRes.cstFieldWarnInfo].ToString();
                    if (infoStr.StartsWith("警告：病人"))
                    {
                        infoStr = infoStr.Substring(3, infoStr.Length - 5);
                    }
                    else if (infoStr.StartsWith("警告："))
                    {
                        infoStr = infoStr.Substring(3, infoStr.Length - 3);
                    }
                    else if (infoStr.StartsWith("病人"))
                    {
                        infoStr = infoStr.Substring(3, infoStr.Length - 2);
                    }
                    inforow[ConstRes.cstFieldInfo] = infoStr;
                }
                inforow[ConstRes.cstFieldTipStatus] = (int)recordstate;
                infos.Tables[CstTimeLimitInfoDataTableName].Rows.Add(inforow);
            }

            gridControlTimeLimitInfo.DataSource = infos;
            gridControlTimeLimitInfo.DataMember = CstTimeLimitInfoDataTableName;
        }

        string TimeSpan2Chn(TimeSpan span)
        {
            int day = span.Days;
            int hour = span.Hours;
            int min = span.Minutes;
            string chnspan = string.Empty;
            if (day > 0) chnspan += day.ToString() + ConstRes.cstDayChn;
            if (hour > 0) chnspan += hour.ToString() + ConstRes.cstHourChn;
            if (min > 0) chnspan += min.ToString() + ConstRes.cstMinuteChn;
            return chnspan;
        }
        #endregion

        private void advBandedGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (advBandedGridView1.FocusedRowHandle < 0) return;
            if (App == null) return;
            DataRow dr = advBandedGridView1.GetDataRow(advBandedGridView1.FocusedRowHandle);
            if (dr == null) return;
            decimal patid = Convert.ToDecimal(dr[ConstRes.cstFieldFirstPage]);
            if (patid < 0) return;

            App.ChoosePatient(patid);
            App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());

        }

        public void HideGridHeader()
        {
            this.advBandedGridView1.OptionsView.ShowGroupPanel = false;
            this.advBandedGridView1.OptionsView.ShowIndicator = false;
            this.advBandedGridView1.OptionsView.ShowColumnHeaders = false;
            this.gridBand2.Visible = false;
        }

        public void HideGridViewGroup()
        {
            for (int i = 0; i < advBandedGridView1.Columns.Count; i++)
            {
                advBandedGridView1.Columns[i].GroupIndex = -1;
            }
        }

        /// <summary>
        /// 收缩事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonShouSuo_Click(object sender, EventArgs e)
        {
            try
            {
                if (null == advBandedGridView1.GetDataRow(advBandedGridView1.FocusedRowHandle) || advBandedGridView1.FocusedRowHandle > 0)
                {
                    advBandedGridView1.CollapseAllGroups();
                }
                else
                {
                    advBandedGridView1.CollapseGroupRow(advBandedGridView1.FocusedRowHandle);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 展开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonZhanKai_Click(object sender, EventArgs e)
        {
            try
            {
                if (null == advBandedGridView1.GetDataRow(advBandedGridView1.FocusedRowHandle) || advBandedGridView1.FocusedRowHandle > 0)
                {
                    advBandedGridView1.ExpandAllGroups();
                }
                else
                {
                    advBandedGridView1.ExpandGroupRow(advBandedGridView1.FocusedRowHandle);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private string m_DoctorID = string.Empty;
        private bool m_IncludeSub = false;
        private int m_PatID = -1;
        /// <summary>
        /// 进行更新时进行的操作名
        /// </summary>
        private string m_MethodName = string.Empty;
        private void simpleButtonRefresh_Click(object sender, EventArgs e)
        {
            if (m_MethodName == "CheckDoctorTime".ToUpper())
            {
                CheckDoctorTime(m_DoctorID, m_IncludeSub);
            }
            else if (m_MethodName == "CheckPatientTime".ToUpper())
            {
                CheckPatientTime(m_PatID);
            }
        }

        /// <summary>
        /// 设置数据列表小标题居中显示
        /// </summary>
        private void SetHeaderCenter()
        {
            if (null != advBandedGridView1.Columns && advBandedGridView1.Columns.Count > 0)
            {
                for (int i = 0; i < advBandedGridView1.Columns.Count; i++)
                {
                    advBandedGridView1.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                }
            }
        }
    }
}
