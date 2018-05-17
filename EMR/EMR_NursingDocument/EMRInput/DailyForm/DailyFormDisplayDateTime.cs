using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DevExpress.XtraTreeList.Nodes;
using DrectSoft.Emr.Util;
using System.Collections;

namespace DrectSoft.Core.EMR_NursingDocument.EMRInput.DailyForm
{
    public partial class DailyFormDisplayDateTime : DevExpress.XtraEditors.XtraForm
    {
        IEmrHost m_App;
        DateTime m_MinDateTime = DateTime.MinValue;
        DateTime m_MaxDateTime = DateTime.MinValue;

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
                }
            }
        }

        /// <summary>
        /// 根据时间范围确定提示信息
        /// </summary>
        private void ShowTip()
        {
            if (m_MinDateTime == DateTime.MinValue && m_MaxDateTime != DateTime.MinValue)
            {
                labelControlTip.Text = "注意：病程时间不能大于 " + m_MaxDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else if (m_MinDateTime != DateTime.MinValue && m_MaxDateTime == DateTime.MinValue)
            {
                labelControlTip.Text = "注意：病程时间不能小于 " + m_MinDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else if (m_MinDateTime != DateTime.MinValue && m_MaxDateTime != DateTime.MinValue)
            {
                labelControlTip.Text = "病程时间大于 " + m_MinDateTime.ToString("yyyy-MM-dd HH:mm:ss") + " 小于 " + m_MaxDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else if (m_MinDateTime == DateTime.MinValue && m_MaxDateTime == DateTime.MinValue)
            {
                labelControlTip.Text = "请输入病程时间";
            }
        }

        private void simpleButtonOK_Click(object sender, EventArgs e)
        {
            if (Check())
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            }
        }

        private bool Check()
        {
            string dateTime = GetDiaplayDateTime();

            if (m_MinDateTime != DateTime.MinValue && m_MaxDateTime != DateTime.MinValue)
            {
                if (Convert.ToDateTime(dateTime) <= m_MinDateTime || Convert.ToDateTime(dateTime) >= m_MaxDateTime)
                {
                    m_App.CustomMessageBox.MessageShow("请输入指定范围内的时间", DrectSoft.Core.CustomMessageBoxKind.ErrorOk);
                    return false;
                }
            }
            else if (m_MinDateTime != DateTime.MinValue && m_MaxDateTime == DateTime.MinValue)
            {
                if (Convert.ToDateTime(dateTime) <= m_MinDateTime)
                {
                    m_App.CustomMessageBox.MessageShow("请输入指定范围内的时间", DrectSoft.Core.CustomMessageBoxKind.ErrorOk);
                    return false;
                }
            }
            else if (m_MinDateTime == DateTime.MinValue && m_MaxDateTime != DateTime.MinValue)
            {
                if (Convert.ToDateTime(dateTime) >= m_MaxDateTime)
                {
                    m_App.CustomMessageBox.MessageShow("请输入指定范围内的时间", DrectSoft.Core.CustomMessageBoxKind.ErrorOk);
                    return false;
                }
            }
            return true;
        }

        private void simpleButtonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        public string GetDiaplayDateTime()
        {
            return dateEditData.DateTime.ToString("yyyy-MM-dd") + " " + timeEditTime.Time.ToString("HH:mm:ss");
        }
    }
}