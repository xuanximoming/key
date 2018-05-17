using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Core.DSReportManager;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Data;
using System.Windows.Forms;

namespace DrectSoft.Core.ReportManager
{
    public partial class PatientListForNew : DevBaseForm
    {
        IEmrHost m_Host;
        private SqlHelper m_SqlHelper;
        public string cardisshow;   //add   jxh   判断哪张报告卡显示
        SqlHelper SqlHelper
        {
            get
            {
                if (m_SqlHelper == null)
                    m_SqlHelper = new SqlHelper(m_Host.SqlHelper);
                return m_SqlHelper;
            }
            set { m_SqlHelper = value; }
        }


        bool m_IsAllInpatient = false;
        public PatientListForNew(IEmrHost host)
        {
            InitializeComponent();
            m_Host = host;
            if (SqlHelper == null)
            {
                SqlHelper = new SqlHelper(m_Host.SqlHelper);
            }
        }

        public PatientListForNew(IEmrHost host, bool isAllInpatient)
            : this(host)
        {
            m_IsAllInpatient = isAllInpatient;
        }

        /// <summary>
        /// 窗体加载事件
        /// edit by Yanqiao.Cai 2012-11-16
        /// 1、add try ... catch
        /// 2、加载性别图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PatientListForNew_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dataSource = new DataTable();

                if (!m_IsAllInpatient)
                {
                    //新增的时候抓取指定科室的病人
                    if (cardisshow == "2")    //edit  jxh  
                    {
                        dataSource = m_SqlHelper.GetPatientListForNewTo(m_Host.User.CurrentDeptId);
                    }
                    else if (cardisshow == "3")
                    {
                        dataSource = m_SqlHelper.GetPatientListForNewToother(m_Host.User.CurrentDeptId);
                    }
                    else if (cardisshow == "4")
                    {
                        //edit by ywk 2013年8月22日 11:18:02
                        dataSource = m_SqlHelper.GetPatientListForCARDIOVAS(m_Host.User.CurrentDeptId);
                    }
                }
                else
                {
                    //补录的时候抓取全院患者
                    dataSource = m_SqlHelper.GetAllPatientListForNewTo();
                    //checkEditOutHospital.Visible = false;//补录可看到出院的人，edit  by ywk 2012年8月9日14:41:41
                }
                //加载性别图片
                DS_Common.InitializeImage_XB(repositoryItemImageXB, imageListXB);

                gridControlPatientList.DataSource = dataSource.DefaultView;
                RefreshPatientList();
                gridControlPatientList.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void textEditName_TextChanged(object sender, EventArgs e)
        {
            RefreshPatientList();
        }

        private void textEditPatID_TextChanged(object sender, EventArgs e)
        {
            RefreshPatientList();
        }

        private void textEditBedNo_TextChanged(object sender, EventArgs e)
        {
            RefreshPatientList();
        }

        private void checkEditOutHospital_CheckedChanged(object sender, EventArgs e)
        {
            RefreshPatientList();
        }

        private void RefreshPatientList()
        {
            DataView dv = gridControlPatientList.DataSource as DataView;
            if (dv != null)
            {
                string name = textEditName.Text.Trim();
                string bedNo = textEditBedNo.Text.Trim();
                string patID = textEditPatID.Text.Trim();
                string strwhere = " 1=1";
                if (!string.IsNullOrEmpty(name))
                {
                    strwhere += " and name like '%" + name + "%' ";
                }

                if (!string.IsNullOrEmpty(bedNo))
                {
                    strwhere += " and bedno like '%" + bedNo + "%'";
                }

                if (!string.IsNullOrEmpty(patID))
                {
                    strwhere += " and patid like '%" + patID + "%'";
                }

                if (checkEditOutHospital.Checked)
                {
                    strwhere += " and status = '1503'";
                }
                else
                {
                    strwhere += " and status <> '1503'";
                }
                dv.RowFilter = strwhere;
            }
        }

        private void textEditName_KeyDown(object sender, KeyEventArgs e)
        {
            ResetFocus(e);
            if (e.KeyCode == Keys.Enter)
            {
                textEditPatID.Focus();
            }
        }

        private void textEditPatID_KeyDown(object sender, KeyEventArgs e)
        {
            ResetFocus(e);
            if (e.KeyCode == Keys.Enter)
            {
                textEditBedNo.Focus();
            }
        }

        private void textEditBedNo_KeyDown(object sender, KeyEventArgs e)
        {
            ResetFocus(e);
            if (e.KeyCode == Keys.Enter)
            {
                gridControlPatientList.Focus();
            }
        }

        private void ResetFocus(KeyEventArgs args)
        {
            if (args.KeyCode == Keys.Down)
            {
                gridControlPatientList.Focus();
            }
        }

        public DataRow GetSelectedRow()
        {
            int rowIndex = gridViewPatientList.FocusedRowHandle;
            if (rowIndex >= 0)
            {
                DataRow dr = gridViewPatientList.GetDataRow(rowIndex);
                return dr;
            }
            return null;
        }


        private void gridControlPatientList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        private void simpleButtonOK_Click(object sender, EventArgs e)
        {

        }

        private void gridControlPatientList_DoubleClick(object sender, EventArgs e)
        {
            GridHitInfo hitInfo = gridViewPatientList.CalcHitInfo(gridControlPatientList.PointToClient(Cursor.Position));
            if (hitInfo.RowHandle >= 0)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        /// <summary>
        /// 序号
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-30</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewPatientList_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 复选框回车事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-30</date>
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