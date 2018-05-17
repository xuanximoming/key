using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraScheduler;

namespace DrectSoft.JobManager
{
    public partial class FrmTimeSetting : UserControl
    {
        #region properties & var
        /// <summary>
        /// 计划设置
        /// </summary>
        public JobPlan JobPlanSetting
        {
            get { return _jobPlanSetting; }
        }
        private JobPlan _jobPlanSetting;

        public Job CurrentJob
        {
            get { return m_Job; }
        }
        private Job m_Job;


        #endregion

        #region ctor
        public FrmTimeSetting()
        {
            InitializeComponent();
            InitializeControl();
        }
        #endregion

        #region public methods

        public void CallJobPlanSet(Job job)
        {
            if (job == null)
                return;
            if (String.IsNullOrEmpty(job.Name))
                Text = "任务计划设置";
            else
                Text = "任务计划设置 －" + job.Name;

            m_Job = job;
            _jobPlanSetting = job.JobSchedule;

            SetControlValueBySetting();

        }
        #endregion

        #region private method
        private void InitializeControl()
        {
            selJobType.SelectedIndex = 0;
            dateEditDurationEnd.DateTime = DateTime.Now;
            dateEditDurationStart.DateTime = DateTime.Now;
            dateEditExecOnce.DateTime = DateTime.Now;
            timeEditStart.Time = new DateTime(1, 1, 1, 0, 0, 0);
            timeEditEnd.Time = new DateTime(1, 1, 1, 23, 59, 59);
            timeEditExecOncePerDay.Time = DateTime.Now;
            ckWeekDays.WeekDays = WeekDays.WorkDays;
        }

        private void SetControlValueBySetting()
        {
            if (JobPlanSetting == null)
                _jobPlanSetting = new JobPlan();

            // 仅作赋值操作，控件的启用通过下拉框和单选按钮来触发
            ckEnabled.Checked = m_Job.Enable;
            // 计划类型
            if (JobPlanSetting.JobType == JobPlanType.JustOnce)
                selJobType.SelectedIndex = 0;
            else
                selJobType.SelectedIndex = 1;
            // 执行一次
            if (JobPlanSetting.DateTimeOfExecOnce > DateTime.MinValue)
            {
                dateEditExecOnce.DateTime = JobPlanSetting.DateTimeOfExecOnce;
                timeEditExecOnce.Time = JobPlanSetting.DateTimeOfExecOnce;
            }
            // 频率
            if (JobPlanSetting.Frequency.IntervalUnit == JobExecIntervalUnit.Day)
                selFrequencyUnit.SelectedIndex = 0;
            else
            {
                selFrequencyUnit.SelectedIndex = 1;
                ckWeekDays.WeekDays = ConvertToWeekDays(JobPlanSetting.Frequency.WeekDays);
            }
            edtIntervalDay.Value = JobPlanSetting.Frequency.Interval;
            edtIntervalWeek.Value = JobPlanSetting.Frequency.Interval;
            // 每天的频率
            radBtnExecOncePerDay.Checked = !JobPlanSetting.FrequencyPerDay.Repeatly;
            timeEditExecOncePerDay.Time = DateTime.Now.Date + JobPlanSetting.FrequencyPerDay.TimeOfExecOnce;
            radBtnInterval.Checked = JobPlanSetting.FrequencyPerDay.Repeatly;
            edtIntervalPerDay.Value = JobPlanSetting.FrequencyPerDay.Interval;
            if (JobPlanSetting.FrequencyPerDay.IntervalUnit == JobExecIntervalUnit.Minute)
                selIntervalUnitPerDay.SelectedIndex = 0;
            else
                selIntervalUnitPerDay.SelectedIndex = 1;
            timeEditStart.Time = DateTime.Now.Date + JobPlanSetting.FrequencyPerDay.StartTime;
            timeEditEnd.Time = DateTime.Now.Date + JobPlanSetting.FrequencyPerDay.EndTime;
            // 持续时间
            dateEditDurationStart.DateTime = JobPlanSetting.Duration.StartDate;
            radBtnEndDate.Checked = JobPlanSetting.Duration.HasEndDate;
            if (JobPlanSetting.Duration.EndDate > DateTime.MinValue)
                dateEditDurationEnd.DateTime = JobPlanSetting.Duration.EndDate;
            else
                dateEditDurationEnd.DateTime = DateTime.Now;
            radBtnNotLimited.Checked = !JobPlanSetting.Duration.HasEndDate;
        }

        private static WeekDays ConvertToWeekDays(string weekDaysString)
        {
            WeekDays result = 0;
            for (int i = 0; i < weekDaysString.Length; i++)
            {
                if (weekDaysString[i] == '1')
                    result |= (WeekDays)(i + 1);
            }
            return result;
        }

        public void ResetPlanSetting()
        {
            if (JobPlanSetting == null)
                _jobPlanSetting = new JobPlan();

            m_Job.Enable = ckEnabled.Checked;
            // 计划类型         
            if (selJobType.SelectedIndex == 0)// 执行一次
            {
                JobPlanSetting.JobType = JobPlanType.JustOnce;
                JobPlanSetting.DateTimeOfExecOnce = dateEditExecOnce.DateTime.Date + timeEditExecOnce.Time.TimeOfDay;
            }
            else
            {
                JobPlanSetting.JobType = JobPlanType.Repeat;
                // 频率
                if (selFrequencyUnit.SelectedIndex == 0) // 按天
                {
                    JobPlanSetting.Frequency.IntervalUnit = JobExecIntervalUnit.Day;
                    JobPlanSetting.Frequency.Interval = Convert.ToInt16(edtIntervalDay.Value);
                }
                else // 按周
                {
                    JobPlanSetting.Frequency.IntervalUnit = JobExecIntervalUnit.Week;
                    JobPlanSetting.Frequency.Interval = Convert.ToInt16(edtIntervalWeek.Value);
                    JobPlanSetting.Frequency.WeekDays = ConvertWeekDaysToString(ckWeekDays.WeekDays);
                }
                // 每天的频率
                JobPlanSetting.FrequencyPerDay.Repeatly = radBtnInterval.Checked;
                if (JobPlanSetting.FrequencyPerDay.Repeatly)
                {
                    JobPlanSetting.FrequencyPerDay.Interval = Convert.ToUInt16(edtIntervalPerDay.Value);
                    if (selIntervalUnitPerDay.SelectedIndex == 0)
                        JobPlanSetting.FrequencyPerDay.IntervalUnit = JobExecIntervalUnit.Minute;
                    else
                        JobPlanSetting.FrequencyPerDay.IntervalUnit = JobExecIntervalUnit.Hour;
                    JobPlanSetting.FrequencyPerDay.StartTime = timeEditStart.Time.TimeOfDay;
                    JobPlanSetting.FrequencyPerDay.EndTime = timeEditEnd.Time.TimeOfDay;
                }
                else
                {
                    JobPlanSetting.FrequencyPerDay.TimeOfExecOnce = timeEditExecOncePerDay.Time.TimeOfDay;
                }
                // 持续时间
                JobPlanSetting.Duration.StartDate = dateEditDurationStart.DateTime;
                JobPlanSetting.Duration.HasEndDate = !radBtnEndDate.Enabled;
                JobPlanSetting.Duration.EndDate = dateEditDurationEnd.DateTime;
            }
        }

        private static string ConvertWeekDaysToString(WeekDays weekDays)
        {
            StringBuilder result = new StringBuilder();
            if ((weekDays & WeekDays.Sunday) > 0)
                result.Append('1');
            else
                result.Append('0');
            if ((weekDays & WeekDays.Monday) > 0)
                result.Append('1');
            else
                result.Append('0');
            if ((weekDays & WeekDays.Tuesday) > 0)
                result.Append('1');
            else
                result.Append('0');
            if ((weekDays & WeekDays.Wednesday) > 0)
                result.Append('1');
            else
                result.Append('0');
            if ((weekDays & WeekDays.Thursday) > 0)
                result.Append('1');
            else
                result.Append('0');
            if ((weekDays & WeekDays.Friday) > 0)
                result.Append('1');
            else
                result.Append('0');
            if ((weekDays & WeekDays.Saturday) > 0)
                result.Append('1');
            else
                result.Append('0');
            return result.ToString();
        }
        #endregion

        #region event handle

        private void radBtnDurationEndDate_CheckedChanged(object sender, EventArgs e)
        {
            dateEditDurationEnd.Enabled = radBtnEndDate.Checked;
        }

        private void selJobType_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupOnce.Enabled = (selJobType.SelectedIndex == 0);
            groupFrequency.Enabled = !groupOnce.Enabled;
            groupFrequencyPerDay.Enabled = groupFrequency.Enabled;
            groupDuration.Enabled = groupFrequency.Enabled;
        }

        private void selFrequencyUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            //groupFrequency.AutoSize = false;
            if (selFrequencyUnit.SelectedIndex == 0)
            {
                panelDay.Visible = true;
                panelWeek.Visible = false;
                panelDay.Top = panelFrequency.Top + panelFrequency.Height;
            }
            else
            {
                panelDay.Visible = false;
                panelWeek.Visible = true;
                panelWeek.Top = panelFrequency.Top + panelFrequency.Height;
            }
            //groupFrequency.AutoSize = true;
        }

        private void radBtnInterval_CheckedChanged(object sender, EventArgs e)
        {
            timeEditExecOncePerDay.Enabled = radBtnExecOncePerDay.Checked;
            panelExecRepeatlyPerDay.Enabled = radBtnInterval.Checked;
        }

        private void ckEnabled_CheckedChanged(object sender, EventArgs e)
        {
            panelClient.Enabled = (ckEnabled.Checked);
            if (ckEnabled.Checked)
                selJobType_SelectedIndexChanged(this, new EventArgs());
        }
        #endregion

    }
}