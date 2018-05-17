using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Common.Eop;
using DrectSoft.Emr.Util;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DrectSoft.Core.EMR_NursingDocument.EMRInput.Table;

namespace DrectSoft.Core.EMR_NursingDocument.EMRInput.HistoryEMR
{
    public partial class HistoryEmrBatchInForm : DevExpress.XtraEditors.XtraForm
    {
        private Inpatient m_CurrentInpatient;

        private IEmrHost m_App;

        private RecordDal m_RecordDal;

        private PatRecUtil m_patUtil;

        public HistoryEmrBatchInForm(IEmrHost app, RecordDal recordDal, Inpatient currentInpatient, PatRecUtil patUtil)
        {
            InitializeComponent();
            m_App = app;
            m_RecordDal = recordDal;
            m_CurrentInpatient = currentInpatient;
            m_patUtil = patUtil;
        }

        private void HistoryEmrBatchInForm_Load(object sender, EventArgs e)
        {
            DataTable dt = m_RecordDal.GetHistoryInpatient((int)m_CurrentInpatient.NoOfFirstPage);
            gridControlHistoryEmr.DataSource = dt;
        }

        private void gridViewHistoryEmr_DoubleClick(object sender, EventArgs e)
        {
            GridHitInfo hitinfo = gridViewHistoryEmr.CalcHitInfo(gridControlHistoryEmr.PointToClient(Cursor.Position));
            if (hitinfo.RowHandle >= 0)
            {
                EMRBatchIn();
            }
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            EMRBatchIn();
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 批量导入病历
        /// </summary>
        /// <param name="noofinpat"></param>
        private void EMRBatchIn()
        {
            DataRow dr = gridViewHistoryEmr.GetDataRow(gridViewHistoryEmr.FocusedRowHandle);
            if (dr != null)
            {
                if (m_App.CustomMessageBox.MessageShow("确定导入病历吗?", DrectSoft.Core.CustomMessageBoxKind.QuestionYesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    string noofinpat = dr["noofinpat"].ToString();
                    if (!string.IsNullOrEmpty(noofinpat))
                    {
                        HistoryEmrBatchIn bachIn = new HistoryEmrBatchIn(noofinpat, m_CurrentInpatient, m_App, m_RecordDal, m_patUtil);
                        bachIn.ExecuteBatchIn();
                        RefreshEMRMainPad();
                        this.Close();
                        m_App.CustomMessageBox.MessageShow("历史病历导入成功!", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                    }
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// 重新加载整个文书录入界面
        /// </summary>
        private void RefreshEMRMainPad()
        {
            m_App.ChoosePatient(m_CurrentInpatient.NoOfFirstPage);
            m_App.LoadPlugIn("DrectSoft.Core.EMR_NursingDocument.EMRInput.Table.dll", "DrectSoft.Core.EMR_NursingDocument.EMRInput.Table.MainForm");
        }
    }
}