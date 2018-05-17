using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using MedicalRecordManage.Object;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Wordbook;
using DrectSoft.DSSqlHelper;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.Service;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common;

namespace MedicalRecordManage.UCControl
{
    /// <summary>
    /// 用于批量编辑病案首页信息然后进行归档操作
    /// add by ywk 2013年7月30日 10:20:40 
    /// </summary>
    public partial class MedIemArchive : DevExpress.XtraEditors.XtraUserControl
    {
        #region 构造函数
        public MedIemArchive()
        {
            InitializeComponent();
            if (!DesignMode)
            {
                DS_SqlHelper.CreateSqlHelper();
            }
        }

        public MedIemArchive(IEmrHost iEmrHost)
        {
            // TODO: Complete member initialization

            m_APP = iEmrHost;
            InitializeComponent();
        }

        #endregion


        #region 属性，字段
        IEMREditor editor;
        private IEmrHost m_APP;

        #endregion

        #region 方法
        /// <summary>
        /// 初始化各控件的值
        /// </summary>
        private void InitContorlValue()
        {
            try
            {
                this.dateLeaveStart.DateTime = System.DateTime.Today.AddMonths(-6);
                this.dateLeaveEnd.DateTime = System.DateTime.Today;
                InitDepartment();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("错误信息！" + ex.Message);
                return;
            }
        }
        /// <summary>
        /// 初始化科室
        /// </summary>
        private void InitDepartment()
        {
            try
            {
                lookUpEditorDepartment.Kind = WordbookKind.Sql;
                lookUpEditorDepartment.ListWindow = lookUpWindowDepartment;

                string sqlInitdepart = @"select * from department  where  valid='1'";
                DataTable dtDepartment = DS_SqlHelper.ExecuteDataTable(sqlInitdepart, CommandType.Text);
                for (int i = 0; i < dtDepartment.Columns.Count; i++)
                {
                    if (dtDepartment.Columns[i].ColumnName.ToUpper().Trim() == "ID")
                    {
                        dtDepartment.Columns[i].Caption = "科室代码";
                    }
                    else if (dtDepartment.Columns[i].ColumnName.ToUpper().Trim() == "NAME")
                    {
                        dtDepartment.Columns[i].Caption = "科室名称";
                    }
                }
                Dictionary<string, int> colWidths = new Dictionary<string, int>();
                colWidths.Add("ID", 60);
                colWidths.Add("NAME", 70);
                SqlWordbook wordBook = new SqlWordbook("Department", dtDepartment, "ID", "NAME", colWidths, "ID//NAME//PY//WB");
                lookUpEditorDepartment.SqlWordbook = wordBook;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 根据查询按钮后归档状态控制归档按钮
        /// </summary>
        private void InitBtnStatus()
        {
            try
            {
                int lockstatus = ComponentCommand.GetLockStatusValue(this.cbxLockStatus.Text);

                if (lockstatus == 4705 || lockstatus == 4707)
                {//未归档
                    this.btn_reback.Enabled = true;
                    this.btnCancel.Enabled = false;
                }
                else if (lockstatus == 4701)
                {//已归档
                    this.btn_reback.Enabled = false;
                    this.btnCancel.Enabled = true;
                }
                else if (lockstatus == 4702)
                {//撤销归档
                    this.btn_reback.Enabled = true;
                    this.btnCancel.Enabled = false;
                }
                else
                {
                    this.btn_reback.Enabled = true;
                    this.btnCancel.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("错误信息！" + ex.Message);
                return;
            }
        }
        /// <summary>
        /// 根据条件查询 
        /// </summary>
        private void LoadDataByTerm()
        {
            try
            {

                if (this.dateLeaveStart.DateTime > this.dateLeaveEnd.DateTime)
                {
                    MessageBox.Show("入院起始日期不能大于结束日期", "信息提示");
                    this.dateLeaveStart.Focus();
                    return;
                }
                int lockstatus = ComponentCommand.GetLockStatusValue(this.cbxLockStatus.Text);
                string initsql = @"select OUTBED,patid,name,noofinpat,outhosdept,admitdept,islock from inpatient where status in (1502,1503) and islock='" + lockstatus + "'   ";
                //xll 未归档的应包括4700和null的值 2013-08-06
                if (lockstatus == 4700)
                {
                    initsql = @"select OUTBED, patid, name, noofinpat, outhosdept, admitdept, islock from inpatient where status in (1502, 1503) and( islock = '4700' or islock is null) ";
                }
                if (!string.IsNullOrEmpty(this.lookUpEditorDepartment.CodeValue))
                {
                    initsql += string.Format(" and outhosdept='{0}' ", this.lookUpEditorDepartment.CodeValue);
                }
                if (!string.IsNullOrEmpty(txtName.Text))
                {
                    initsql += string.Format(" and name like '%{0}%'", txtName.Text.ToString().Trim());
                }
                if (this.dateLeaveStart.Text.Trim() != null && this.dateLeaveStart.Text.Trim() != "")
                {
                    string ds = this.dateLeaveStart.DateTime.ToString("yyyy-MM-dd 00:00:00");
                    string de = this.dateLeaveEnd.DateTime.ToString("yyyy-MM-dd 23:59:59");
                    initsql += string.Format(" and OUTHOSDATE<= '{0}' and OUTHOSDATE>='{1}'", de, ds);
                }

                //新增住院号查询 add by ywk 2013年8月14日 10:00:27
                if (!string.IsNullOrEmpty(txtpatid.Text.ToString().Trim()))
                {
                    initsql += string.Format(" and patid like '%{0}%'", txtpatid.Text.ToString().Trim());
                }

                initsql += "order by name";
                DataTable dt = DS_SqlHelper.ExecuteDataTable(initsql, CommandType.Text);
                this.gcInpatient.DataSource = dt;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("错误信息！" + ex.Message);
                return;
            }
        }
        /// <summary>
        /// 右侧展示此病人的病案首页信息
        /// add by ywk 2013年7月30日 11:45:56
        /// </summary>
        /// <param name="drInpatient"></param>
        private void LoadIemMainPage(DataRow drInpatient)
        {
            try
            {
                if (string.IsNullOrEmpty(drInpatient["Noofinpat"].ToString()))
                {
                    return;
                }
                string noofinpat = drInpatient["Noofinpat"].ToString();
                string search = " select mname from dict_catalog where ccode='AA' ";
                DataTable dt = DS_SqlHelper.ExecuteDataTable(search, CommandType.Text);

                editor = (IEMREditor)Activator.CreateInstance(Type.GetType(dt.Rows[0]["mname"].ToString()), new object[] { noofinpat });
                editor.DesignUI.Dock = DockStyle.Fill;
                scrolInpIemInfo.Controls.Clear();
                scrolInpIemInfo.Controls.Add(editor.DesignUI);
                m_APP.CurrentPatientInfo = new DrectSoft.Common.Eop.Inpatient(Convert.ToDecimal(noofinpat));
                m_APP.CurrentPatientInfo.ReInitializeAllProperties();
                editor.Load(m_APP);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("错误信息！" + ex.Message);
                return;
            }
        }

        #endregion

        #region 事件
        /// <summary>
        /// 序號列add by  ywk 2013年8月7日 13:54:49
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvInpatient_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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
        /// 窗体加载事件
        /// add  by ywk  2013年7月30日 10:47:34
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MedIemArchive_Load(object sender, EventArgs e)
        {
            try
            {
                InitContorlValue();
                DrectSoft.Common.DS_Common.CancelMenu(this.panelControl2, contextMenuStrip1);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("错误信息！" + ex.Message);
                return;
            }
        }

        /// <summary>
        /// 归档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_reback_Click(object sender, EventArgs e)
        {
            try
            {
                int fouceRowIndex = gvInpatient.FocusedRowHandle;
                if (fouceRowIndex < 0)
                {
                    gcInpatient.Focus();
                    SqlUtil.App.CustomMessageBox.MessageShow("请选择一条病人记录");
                    return;
                }
                DataRow foucesRow = gvInpatient.GetDataRow(fouceRowIndex);
                if (null == foucesRow || null == foucesRow["NOOFINPAT"])
                {
                    return;
                }
                int noofinpat = int.Parse(foucesRow["NOOFINPAT"].ToString().Trim());
                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要归档 " + foucesRow["NAME"] + " 的病历吗？", "归档病历", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
                // YD_BaseService.CheckRecordRebacked(noofinpat.ToString())
                if (foucesRow["islock"].ToString() != "4701")
                {
                    int num = DS_SqlService.SetRecordsRebacked(noofinpat.ToString());
                    if (num > 0)
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("归档成功");
                        gvInpatient.DeleteRow(gvInpatient.FocusedRowHandle);
                        return;
                    }
                }
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该病人已归档。");
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("归档操作失败" + ex.Message);
                return;
            }


        }

        /// <summary>
        /// 撤销归档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                int fouceRowIndex = gvInpatient.FocusedRowHandle;
                if (fouceRowIndex < 0)
                {
                    gcInpatient.Focus();
                    SqlUtil.App.CustomMessageBox.MessageShow("请选择一条病人记录");
                    return;
                }
                DataRow foucesRow = gvInpatient.GetDataRow(fouceRowIndex);
                if (null == foucesRow || null == foucesRow["NOOFINPAT"])
                {
                    return;
                }
                int noofinpat = int.Parse(foucesRow["NOOFINPAT"].ToString().Trim());
                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要撤销 " + foucesRow["NAME"] + " 的病历归档吗？", "撤销归档病历", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
                //YD_BaseService.CheckRecordRebacked(noofinpat.ToString())
                if (foucesRow["islock"].ToString() == "4701")
                {
                    int num = DS_SqlService.SetRecordsCancel(noofinpat.ToString());
                    if (num > 0)
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("撤销归档成功");
                        gvInpatient.DeleteRow(gvInpatient.FocusedRowHandle);
                        return;
                    }
                }
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该病人未归档无法撤销。");
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("撤销归档失败！" + ex.Message);
                return;
            }

        }

        /// <summary>
        /// 查询操作 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                InitBtnStatus();
                LoadDataByTerm();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("错误信息！" + ex.Message);
                return;
            }
        }
        /// <summary>
        /// 双击列表右侧展示病案首页信息
        /// add by ywk 2013年7月30日 11:44:22
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvInpatient_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                var gvInpatient = (sender as DevExpress.XtraGrid.Views.Grid.GridView);
                if (gvInpatient == null) { return; }
                DataRow drInpatient = gvInpatient.GetFocusedDataRow();
                if (drInpatient == null || drInpatient["NOOFINPAT"] == null) return;
                LoadIemMainPage(drInpatient);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("错误信息！" + ex.Message);
                return;
            }
        }



        #endregion





    }
}
