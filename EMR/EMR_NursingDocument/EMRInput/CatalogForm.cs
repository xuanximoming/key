using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.Emr.Util;
using System.Xml;
using DrectSoft.Common.Eop;
using DrectSoft.FrameWork.WinForm.Plugin;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DrectSoft.Core;

//modified by zhouhui 2012-6-5 个人模板读取源已修改为DOCTOREMRTEMPLET   Modified by wwj 由于线上反应强烈，所有还原原有的修改
//取消科室模板

namespace DrectSoft.Core.EMR_NursingDocument.EMRInput.Table
{
    public partial class CatalogForm : Form
    {
        /// <summary>
        /// 病程标志位,默认非病程
        /// </summary>
        bool m_IsDailyEmr = false;
        /// <summary>
        /// 首程的标志位，默认非首次病程
        /// </summary>
        bool m_IsFristDailyEmr = false;

        //todo：暂时用名字来判断是否是病程
        //const string c_FristDailyEmr = " ( mr_name like '%首程%' or mr_name like '%首次病程%' ) ";
        //const string c_DailyEmr = " ( mr_name not like '%首程%' AND mr_name not like '%首次病程%' ) ";

        IEmrHost m_app;

        //判断首次病程
        const string c_FirstDailyEmr = " isfirstdaily = '1' ";
        const string c_DailyEmr = " isfirstdaily <> '1' or isfirstdaily is null ";

        public EmrModel CommitModel
        {
            get { return _commitModel; }
        }
        private EmrModel _commitModel;
        private RecordDal m_RecordDal;
        private Employee m_DoctorEmployee;

        public Inpatient CurrentInpatient
        { get; set; }
        //历史病历业务逻辑部分
        private HistoryEMRBLL m_HistoryEMRBLL;

        public CatalogForm(DataTable template, DataTable templatePerson, RecordDal recordDal, Employee doctorEmployee, IEmrHost app, string sortID, Inpatient currentInpatient)
        {
            InitializeComponent();
            SetShowType();
            Bind(template, templatePerson);
            m_RecordDal = recordDal;
            m_DoctorEmployee = doctorEmployee;
            m_app = app;
            CurrentInpatient = currentInpatient;
            m_HistoryEMRBLL = new HistoryEMRBLL(app, currentInpatient, recordDal, sortID);
            xtraTabPageHistoryEMR.PageVisible = false;
        }

        private void SetShowType()
        {
            string newEmrShowType = AppConfigReader.GetAppConfig("NewEmrShowType").Config;//1:按照病种分组  2:不进行分组
            if (newEmrShowType != "1")//不进行分组
            {
                colkIND.GroupIndex = -1;
                gridView1.OptionsView.ShowGroupPanel = false;
            }
        }


        private void Bind(DataTable templateData, DataTable templatePersonData)
        {
            //bind events
            this.btn_OK.Click += new EventHandler(btn_OK_Click);
            this.btn_Cancel.Click += new EventHandler(btn_Cancel_Click);

            //绑定常用模板
            gridControl1.DataSource = templateData;

            //绑定个人模板
            DataTable dtPerson = templatePersonData.Copy();
            for (int i = 0; i < dtPerson.Rows.Count; i++)
            {
                if (dtPerson.Rows[i]["type"].ToString() == "2")//排除科室模板
                {
                    dtPerson.Rows.RemoveAt(i);
                }
            }
            dtPerson.AcceptChanges(); 
            gridControlTemplatePerson.DataSource = dtPerson;

            //绑定科室模板
            DataTable dtDepartment = templatePersonData.Copy();
            for (int i = 0; i < dtDepartment.Rows.Count; i++)
            {
                if (dtDepartment.Rows[i]["type"].ToString() == "1")//排除个人模板
                {
                    dtDepartment.Rows.RemoveAt(i);
                }
            }
            dtDepartment.AcceptChanges();
            gridControlDepartment.DataSource = dtDepartment;
        }



        private void Commit()
        {
            if (xtraTabControl1.SelectedTabPage == xtraTabPageCommon)
            {
                //通用模板
                DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                if (row == null) return;
                _commitModel = new EmrModel();
                _commitModel.InstanceId = -1;
                _commitModel.TempIdentity = row["TEMPLET_ID"].ToString();
                _commitModel.ModelName = row["MR_NAME"].ToString().Replace(" ", "-");
                _commitModel.ModelCatalog = row["MR_CLASS"].ToString();
                _commitModel.FirstDailyEmrModel = m_IsFristDailyEmr; //首次病程
                _commitModel.IsNewPage = row["NEW_PAGE_FLAG"].ToString() == "1" ? true : false;

                _commitModel.FileName = row["FILE_NAME"].ToString();
                _commitModel.IsShowFileName = row["ISSHOWFILENAME"].ToString();
                _commitModel.IsYiHuanGouTong = row["ISYIHUANGOUTONG"].ToString();
                _commitModel.NewPageEnd = row["NEW_PAGE_END"].ToString() == "1" ? true : false;
                _commitModel.IsReadConfigPageSize = row["ISCONFIGPAGESIZE"].ToString() == "1" ? true : false;

                if (!string.IsNullOrEmpty(_commitModel.TempIdentity))
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
            if (xtraTabControl1.SelectedTabPage == xtraTabPageTemplatePerson)
            {
                //个人模板
                DataRow row = TemplatePerson.GetDataRow(TemplatePerson.FocusedRowHandle);
                if (row == null) return;
                _commitModel = new EmrModel();
                _commitModel.InstanceId = -1;
                _commitModel.TempIdentity = row["templateid"].ToString();
                _commitModel.ModelName = row["MR_NAME"].ToString().Replace(" ", "-");
                _commitModel.ModelCatalog = row["SORTID"].ToString();

                XmlDocument dom = new XmlDocument();
                dom.LoadXml(RecordDal.UnzipEmrXml(m_RecordDal.GetTemplatePersonContent(row["ID"].ToString())));
                _commitModel.ModelContent = dom;
                _commitModel.FirstDailyEmrModel = m_IsFristDailyEmr; //首次病程
                _commitModel.IsNewPage = false;

                _commitModel.IsReadConfigPageSize = row["ISCONFIGPAGESIZE"].ToString() == "1" ? true : false;

                if (!string.IsNullOrEmpty(_commitModel.ModelContent.OuterXml))
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
            if (xtraTabControl1.SelectedTabPage == xtraTabPageDepartment)
            {
                //科室模板
                DataRow row = gridViewDepartment.GetDataRow(gridViewDepartment.FocusedRowHandle);
                if (row == null) return;
                _commitModel = new EmrModel();
                _commitModel.InstanceId = -1;
                _commitModel.TempIdentity = row["templateid"].ToString();
                _commitModel.ModelName = row["MR_NAME"].ToString().Replace(" ", "-");
                _commitModel.ModelCatalog = row["SORTID"].ToString();

                XmlDocument dom = new XmlDocument();
                dom.LoadXml(RecordDal.UnzipEmrXml(m_RecordDal.GetTemplatePersonContent(row["ID"].ToString())));
                _commitModel.ModelContent = dom;
                _commitModel.FirstDailyEmrModel = m_IsFristDailyEmr; //首次病程
                _commitModel.IsNewPage = false;

                _commitModel.IsReadConfigPageSize = row["ISCONFIGPAGESIZE"].ToString() == "1" ? true : false;

                if (!string.IsNullOrEmpty(_commitModel.ModelContent.OuterXml))
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
            if (xtraTabControl1.SelectedTabPage == xtraTabPageHistoryEMR)
            {
                //病人历史病历
                DataRow row = gridViewHistoryEMR.GetDataRow(gridViewHistoryEMR.FocusedRowHandle);
                if (row == null) return;
                _commitModel = new EmrModel();
                _commitModel.InstanceId = -1;
                _commitModel.TempIdentity = row["templateid"].ToString();
                _commitModel.ModelName = row["MR_NAME"].ToString().Replace(" ", "-");
                _commitModel.ModelCatalog = row["SORTID"].ToString();

                if (row["content"].ToString() == "")
                {
                    string recorddetailid = row["ID"].ToString();
                    string emrContent = m_RecordDal.GetEmrContentByID(recorddetailid);
                    XmlDocument dom = new XmlDocument();
                    dom.PreserveWhitespace = true;
                    dom.LoadXml(emrContent);
                    _commitModel.ModelContentHistory = dom;
                }
                else
                {
                    string emrContent = row["content"].ToString();
                    XmlDocument dom = new XmlDocument();
                    dom.PreserveWhitespace = true;
                    dom.LoadXml(emrContent);
                    _commitModel.ModelContentHistory = dom;
                }

                _commitModel.FirstDailyEmrModel = m_IsFristDailyEmr; //首次病程
                _commitModel.IsNewPage = row["NEW_PAGE_FLAG"].ToString() == "1" ? true : false;

                _commitModel.FileName = row["FILE_NAME"].ToString();
                _commitModel.IsShowFileName = row["ISSHOWFILENAME"].ToString();
                _commitModel.IsYiHuanGouTong = row["ISYIHUANGOUTONG"].ToString();
                _commitModel.NewPageEnd = row["NEW_PAGE_END"].ToString() == "1" ? true : false;
                _commitModel.IsReadConfigPageSize = row["ISCONFIGPAGESIZE"].ToString() == "1" ? true : false;

                if (!string.IsNullOrEmpty(_commitModel.TempIdentity))
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
        }

        void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        void btn_OK_Click(object sender, EventArgs e)
        {
            Commit();
        }

        /// <summary>
        /// 双击通用模板中的记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            GridHitInfo hitInfo = gridView1.CalcHitInfo(gridControl1.PointToClient(Cursor.Position));
            if (hitInfo.RowHandle >= 0)
            {
                Commit();
            }
        }

        /// <summary>
        /// 双击个人模板中的记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TemplatePerson_DoubleClick(object sender, EventArgs e)
        {
            Commit();
        }

        private void textEditModelName_TextChanged(object sender, EventArgs e)
        {
            SetGridShowData();
        }

        private void SetGridShowData()
        {
            string filter = string.Format(" (MR_NAME like '%{0}%' or PY like '%{0}%' or WB like '%{0}%')  ", textEditModelName.Text.Trim());
            string filterTemplatePerson = string.Format(" (MR_NAME like '%{0}%' or PY like '%{0}%' or WB like '%{0}%')  ", textEditModelName.Text.Trim());

            //对病程做特殊处理， 每份病程记录第一个病程是“首次病程”
            if (m_IsDailyEmr)
            {
                if (m_IsFristDailyEmr)//首程
                {
                    filter += " AND (" + c_FirstDailyEmr + ") ";
                }
                else
                {
                    filter += " AND (" + c_DailyEmr + ") ";
                }
            }

            DataTable dt = gridControl1.DataSource as DataTable;
            if (dt != null)
            {
                dt.DefaultView.RowFilter = filter;
            }

            DataTable dtTempPerson = gridControlTemplatePerson.DataSource as DataTable;
            if (dtTempPerson != null)
            {
                dtTempPerson.DefaultView.RowFilter = filterTemplatePerson;
            }

            DataTable dtTempDepartment = gridControlDepartment.DataSource as DataTable;
            if (dtTempDepartment != null)
            {
                dtTempDepartment.DefaultView.RowFilter = filterTemplatePerson;
            }
        }

        /// <summary>
        /// 设置首程标志位
        /// </summary>
        public void SetFristDailyEmrFlag()
        {
            m_IsFristDailyEmr = true;
            m_IsDailyEmr = true;
        }

        /// <summary>
        /// 设置病程标志位
        /// </summary>
        public void SetDailyEmrFlag()
        {
            m_IsDailyEmr = true;
        }


        private void textEditModelName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (xtraTabControl1.SelectedTabPage == xtraTabPageCommon)
                {
                    gridControl1.Focus();
                }
                else if (xtraTabControl1.SelectedTabPage == xtraTabPageTemplatePerson)
                {
                    gridControlTemplatePerson.Focus();
                }
                else if (xtraTabControl1.SelectedTabPage == xtraTabPageDepartment)
                {
                    gridControlDepartment.Focus();
                }
            }
        }

        private void gridControl1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToInt32(e.KeyChar) == 13)//按下回车
            {
                Commit();
            }
        }

        private void gridControlTemplatePerson_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToInt32(e.KeyChar) == 13)//按下回车
            {
                Commit();
            }
        }

        private void gridControlDepartment_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToInt32(e.KeyChar) == 13)//按下回车
            {
                Commit();
            }
        }

        private void CatalogForm_Load(object sender, EventArgs e)
        {
            HideTemplatePerson();
            SetGridShowData();
        }

        private void HideTemplatePerson()
        {
            if (m_IsDailyEmr)
            {
                xtraTabPageTemplatePerson.PageVisible = false;
                xtraTabPageDepartment.PageVisible = false;
            }
        }

        private void simpleButtonPYWB_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 删除科室模版
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_deldepttemp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridViewDepartment.FocusedRowHandle < 0) return;
            DataRow foucesRow = gridViewDepartment.GetDataRow(gridViewDepartment.FocusedRowHandle);
            if (foucesRow == null)
                return;
            if (foucesRow.IsNull("TEMPLATEID"))
                return;

            PatRecUtil util = new PatRecUtil(m_app, m_app.CurrentPatientInfo);
            util.DelTemplatePerson(foucesRow["ID"].ToString());


            foucesRow.Delete();
        }

        /// <summary>
        /// 删除个人模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_DelPersonTemplate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (TemplatePerson.FocusedRowHandle < 0) return;
            DataRow foucesRow = TemplatePerson.GetDataRow(TemplatePerson.FocusedRowHandle);
            if (foucesRow == null)
                return;
            if (foucesRow.IsNull("TEMPLATEID"))
                return;

            PatRecUtil util = new PatRecUtil(m_app, m_app.CurrentPatientInfo);
            util.DelTemplatePerson(foucesRow["ID"].ToString());


            foucesRow.Delete();

            //gridViewDepartment.DataSource.AcceptChanges();


        }

        private void gridViewDepartment_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.popupMenu1.ShowPopup(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                btn_DelPersonTemplate.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                btn_deldepttemp.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
        }

        private void TemplatePerson_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.popupMenu1.ShowPopup(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                btn_deldepttemp.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                btn_DelPersonTemplate.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (e.Page == xtraTabPageHistoryEMR)
            {
                if (gridControlHistoryEMR.DataSource == null)
                {
                    DataTable dtHistoryEmrBLL = m_HistoryEMRBLL.GetHistoryEmrRecord();
                    gridControlHistoryEMR.DataSource = dtHistoryEmrBLL;
                }

                DataTable dtTempHistory = gridControlHistoryEMR.DataSource as DataTable;
                if (dtTempHistory != null)
                {
                    string filterHistoryEMR = string.Format(" (MR_NAME like '%{0}%' or PY like '%{0}%' or WB like '%{0}%') ", textEditModelName.Text.Trim());

                    if (m_IsDailyEmr)
                    {
                        if (m_IsFristDailyEmr)//首程
                        {
                            filterHistoryEMR += " AND (" + c_FirstDailyEmr + ") ";
                        }
                        else
                        {
                            filterHistoryEMR += " AND (" + c_DailyEmr + ") ";
                        }
                    }

                    dtTempHistory.DefaultView.RowFilter = filterHistoryEMR;
                }
            }
            textEditModelName.Focus();
        }

        private void gridViewHistoryEMR_DoubleClick(object sender, EventArgs e)
        {
            Commit();
        }

        private void gridViewDepartment_DoubleClick(object sender, EventArgs e)
        {
            Commit();
        }

        private void CatalogForm_Activated(object sender, EventArgs e)
        {
            textEditModelName.Focus();
        }
    }
}
