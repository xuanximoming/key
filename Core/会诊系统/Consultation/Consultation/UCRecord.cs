using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Wordbook;
using DrectSoft.Common;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DrectSoft.Common.Eop;
using DrectSoft.Core.Consultation.NEW;
using DrectSoft.Common.Ctrs.DLG;

namespace DrectSoft.Core.Consultation
{
    public partial class UCRecord : DevExpress.XtraEditors.XtraUserControl
    {
        private IEmrHost m_App;
        public UCRecord()
        {
            InitializeComponent();
        }

        public UCRecord(IEmrHost app)
            : this()
        {
            m_App = app;
            BindLookUpEditorData();
            InitGridControl();
            InitDateEdit();
            //Search();
            textEditName.Focus();
        }

        private void InitGridControl()
        {
            gridViewList.OptionsSelection.EnableAppearanceFocusedRow = true;
            gridViewList.OptionsSelection.EnableAppearanceFocusedCell = false; ;
            gridViewList.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            gridViewList.DoubleClick += new EventHandler(gridViewList_DoubleClick);
        }

        private void InitDateEdit()
        {
            dateEditConsultDateBegin.EditValue = System.DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd");
            dateEditConsultDateEnd.EditValue = System.DateTime.Now.ToString("yyyy-MM-dd");
        }

        private void Search()
        {
            //string consultTimeBegin = dateEditConsultDateBegin.Text;
            //string consultTimeEnd = dateEditConsultDateEnd.Text;
            //string consultType = "";
            //string urgency = lookUpEditorUrgency.CodeValue;
            //string name = textEditName.Text;
            //string patientSN = textEditPatientSN.Text;
            //string bedCode = lookUpEditorBed.CodeValue;

            //DataTable dt = Dal.DataAccess.GetConsultationData("23", consultTimeBegin, consultTimeEnd, consultType, urgency, name, patientSN, bedCode, m_App.User.CurrentDeptId);
            //gridControlList.DataSource = dt;

            //m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewList);
            SearchRecord();
        }

        /// <summary>
        /// 查询记录  获取与系统登录人相关的待会诊、会诊记录保存的记录  然后在点击显示的会诊记录时判断会诊意见的填写情况 Add by wwj 2013-02-27
        /// </summary>
        private void SearchRecord()
        {
            try
            {
                DateTime consultTimeBegin = dateEditConsultDateBegin.DateTime;
                DateTime consultTimeEnd = dateEditConsultDateEnd.DateTime;
                string urgency = lookUpEditorUrgency.CodeValue;
                string name = textEditName.Text;
                string patientSN = textEditPatientSN.Text;
                string bedCode = lookUpEditorBed.CodeValue;

                Employee emp = new Employee(m_App.User.Id);
                emp.ReInitializeProperties();
                DoctorGrade grade = (DoctorGrade)Enum.Parse(typeof(DoctorGrade), emp.Grade);

                DataTable dt = Dal.DataAccess.GetWaitConsult(
                   consultTimeBegin,consultTimeEnd,
                    name, patientSN, urgency, bedCode, m_App.User.Id, emp.Grade);

                gridControlList.DataSource = dt;
                m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BindLookUpEditorData()
        {
            BindUrgency();
            BindBed();
        }

        #region 绑定会诊紧急程度
        private void BindUrgency()
        {
            lookUpEditorUrgency.Kind = WordbookKind.Sql;
            lookUpEditorUrgency.ListWindow = lookUpWindowUrgency;
            BindUrgencyWordBook(GetConsultationData("6", "66"));
        }

        private void BindUrgencyWordBook(DataTable dataTableData)
        {
            for (int i = 0; i < dataTableData.Columns.Count; i++)
            {
                if (dataTableData.Columns[i].ColumnName.ToLower().Trim() == "name")
                {
                    dataTableData.Columns[i].Caption = "紧急度";
                }
            }

            Dictionary<string, int> colWidths = new Dictionary<string, int>();
            colWidths.Add("NAME", 120);
            SqlWordbook wordBook = new SqlWordbook("Urgency", dataTableData, "ID", "NAME", colWidths, "ID//NAME//PY//WB");
            lookUpEditorUrgency.SqlWordbook = wordBook;
        }
        #endregion

        #region 绑定当前病区的床位
        private void BindBed()
        {
            lookUpEditorBed.Kind = WordbookKind.Sql;
            lookUpEditorBed.ListWindow = lookUpWindowBed;
            BindBedWordBook(GetConsultationData("7", m_App.User.CurrentWardId));
        }

        private void BindBedWordBook(DataTable dataTableData)
        {
            for (int i = 0; i < dataTableData.Columns.Count; i++)
            {
                if (dataTableData.Columns[i].ColumnName.ToLower().Trim() == "id")
                {
                    dataTableData.Columns[i].Caption = "床位号";
                }
            }

            Dictionary<string, int> colWidths = new Dictionary<string, int>();
            colWidths.Add("ID", 120);
            SqlWordbook wordBook = new SqlWordbook("Bed", dataTableData, "ID", "ID", colWidths, "ID");
            lookUpEditorBed.SqlWordbook = wordBook;
        }
        #endregion

        /// <summary>
        /// 校验方法
        /// Add by  xlb 2013-03-13
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool ValidateDate(ref string message)
        {
            try
            {
                DateTime dateBegin = dateEditConsultDateBegin.DateTime.Date;
                DateTime dateEnd = dateEditConsultDateEnd.DateTime.Date;
                if (dateBegin > dateEnd)//会诊起始时间大于结束时间
                {
                    message = "起始时间不能大于结束时间";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataTable GetConsultationData(string typeID, string param)
        {
            if (Dal.DataAccess.App == null)
            {
                Dal.DataAccess.App = m_App;
            }
            DataTable dataTableConsultationData = new DataTable();
            dataTableConsultationData = Dal.DataAccess.GetConsultationData("", typeID, param);
            return dataTableConsultationData;
        }

        /// <summary>
        /// 查询事件
        /// Edit by xlb 2013-03-13
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void simpleButtonSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string message = "";
                if (!ValidateDate(ref message))
                {
                    MessageBox.Show(message);
                    return;
                }
                Search();
                if (((DataTable)gridControlList.DataSource).Rows.Count == 0 
                    || gridControlList.DataSource == null)
                {
                    MessageBox.Show("没有符合条件的数据");
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 双击列表事件
        /// edit by Yanqiao.Cai 2012-11-05
        /// add try ... catch
        /// edit by xlb 2013-03-08
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gridViewList_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                //edit by Yanqiao.Cai 2012-11-05 双击标题事件(直接返回)
                GridHitInfo hitInfo = gridViewList.CalcHitInfo(gridControlList.PointToClient(Cursor.Position));
                if (hitInfo.RowHandle < 0)
                {
                    return;
                }

                if (gridViewList.FocusedRowHandle >= 0)
                {
                    DataRow dr = gridViewList.GetDataRow(gridViewList.FocusedRowHandle);
                    if (dr != null)
                    {
                        string noOfFirstPage = dr["NoOfInpat"].ToString();
                        string consultTypeID = dr["ConsultTypeID"].ToString();
                        string consultApplySn = dr["ConsultApplySn"].ToString();

                        //if (consultTypeID == Convert.ToString((int)ConsultType.One))
                        //{
                        //    FormRecordForOne formRecordForOne = new FormRecordForOne(noOfFirstPage, m_App, consultApplySn);
                        //    formRecordForOne.StartPosition = FormStartPosition.CenterParent;
                        //    formRecordForOne.ShowDialog();
                        //}
                        //else
                        //{
                        //FormRecordForMultiply formRecrodForMultiply = new FormRecordForMultiply(noOfFirstPage, m_App, consultApplySn);
                        //formRecrodForMultiply.StartPosition = FormStartPosition.CenterParent;
                        //if (dr["APPLYUSER"].ToString() != m_App.User.Id)
                        //    formRecrodForMultiply.ReadOnlyControl();
                        //formRecrodForMultiply.ShowDialog();
                        //}
                        ProcessClickConsultatonListLogic processConsult = new ProcessClickConsultatonListLogic(m_App, noOfFirstPage);
                        processConsult.ProcessLogic(m_App.User.Id, consultApplySn);
                        Search();
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 序号
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-25</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewList_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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
        /// 重置事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-13</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_reset_Click(object sender, EventArgs e)
        {
            try
            {
                Reset();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 重置方法
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-13</date>
        /// </summary>
        private void Reset()
        {
            try
            {
                InitDateEdit();
                textEditName.Text = string.Empty;
                textEditPatientSN.Text = string.Empty;
                lookUpEditorUrgency.CodeValue = string.Empty;
                lookUpEditorBed.CodeValue = string.Empty;
                textEditName.Focus();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
