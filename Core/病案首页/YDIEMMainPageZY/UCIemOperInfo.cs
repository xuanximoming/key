using DevExpress.XtraEditors;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Eop;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DrectSoft.Core.IEMMainPageZY
{
    public partial class UCIemOperInfo : UserControl
    {
        private IemMainPageInfo m_IemInfo;
        /// <summary>
        /// 病案首页病患信息
        /// </summary>
        public IemMainPageInfo IemInfo
        {
            get
            {
                m_IemInfo = new IemMainPageInfo();
                GetUI();
                return m_IemInfo;
            }
        }

        private IEmrHost m_App;

        private DataTable m_DataTableDiag = null;
        public UCIemOperInfo()
        {
            InitializeComponent();

        }

        private void UCIemOperInfo_Load(object sender, EventArgs e)
        {
        }


        public void FillUI(IemMainPageInfo info, IEmrHost app)
        {
            try
            {
                m_App = app;
                m_IemInfo = info;

                //(new FillUIDelegate(FillUIInner)).BeginInvoke(null, null);
                FillUIInner();
            }
            catch (Exception)
            {
                throw;
            }
        }

        delegate void FillUIDelegate();

        private void FillUIInner()
        {
            try
            {
                #region
                if (m_IemInfo.IemBasicInfo.Iem_Mainpage_NO == "")
                {
                    //to do 病患基本信息
                    //病案首页中无数据，关于颅内昏迷的几个时间默认显示为---  edit  by ywk 2012年5月30日 10:19:12
                    txtBeforeHosComaDay.Text = m_IemInfo.IemBasicInfo.BeforeHosComaDay;
                    txtBeforeHosComaHour.Text = m_IemInfo.IemBasicInfo.BeforeHosComaHour;
                    txtBeforeHosComaMinute.Text = m_IemInfo.IemBasicInfo.BeforeHosComaMinute;

                    txtLaterHosComaDay.Text = m_IemInfo.IemBasicInfo.LaterHosComaDay;
                    txtLaterHosComaHour.Text = m_IemInfo.IemBasicInfo.LaterHosComaHour;
                    txtLaterHosComaMinute.Text = m_IemInfo.IemBasicInfo.LaterHosComaMinute;
                }
                else
                {

                    //手术
                    //DataTable dataTableOper = new DataTable();
                    //foreach (Iem_MainPage_Operation im in m_IemInfo.IemOperInfo)
                    //{
                    //    if (m_OperInfoFrom == null)
                    //        m_OperInfoFrom = new IemNewOperInfo(m_App);
                    //    m_OperInfoFrom.IemOperInfo = im;
                    //    DataTable dataTable = m_OperInfoFrom.DataOper;
                    //    if (dataTableOper.Rows.Count == 0)
                    //        dataTableOper = dataTable.Clone();
                    //    foreach (DataRow row in dataTable.Rows)
                    //    {
                    //        dataTableOper.ImportRow(row);
                    //    }
                    //    dataTableOper.AcceptChanges();
                    //}
                    //this.gridControl1.DataSource = dataTableOper;
                    this.gridControl1.DataSource = null;
                    this.gridControl1.DataSource = m_IemInfo.IemOperInfo.Operation_Table;

                    m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewOper);

                    //txtXRay.Text = m_IemInfo.IemBasicInfo.Xay_Sn;
                    //txtCT.Text = m_IemInfo.IemBasicInfo.Ct_Sn;
                    //txtMri.Text = m_IemInfo.IemBasicInfo.Mri_Sn;
                    //txtDsa.Text = m_IemInfo.IemBasicInfo.Dsa_Sn;

                    //gridControl2.EndUpdate();
                    //gridControl3.EndUpdate();

                    #region 新增项目信息

                    if (m_IemInfo.IemBasicInfo.OutHosType == "1")
                        chkOutHosType1.Checked = true;
                    else if (m_IemInfo.IemBasicInfo.OutHosType == "2")
                        chkOutHosType2.Checked = true;
                    else if (m_IemInfo.IemBasicInfo.OutHosType == "3")
                        chkOutHosType3.Checked = true;
                    else if (m_IemInfo.IemBasicInfo.OutHosType == "4")
                        chkOutHosType4.Checked = true;
                    else if (m_IemInfo.IemBasicInfo.OutHosType == "5")
                        chkOutHosType5.Checked = true;
                    else if (m_IemInfo.IemBasicInfo.OutHosType == "9")
                        chkOutHosType9.Checked = true;

                    txtReceiveHosPital.Text = m_IemInfo.IemBasicInfo.ReceiveHosPital;
                    txtReceiveHosPital2.Text = m_IemInfo.IemBasicInfo.ReceiveHosPital2;

                    if (m_IemInfo.IemBasicInfo.AgainInHospital == "1")
                        chkAgainInHospital1.Checked = true;
                    else if (m_IemInfo.IemBasicInfo.AgainInHospital == "2")
                        chkAgainInHospital2.Checked = true;

                    txtAgainInHospitalReason.Text = m_IemInfo.IemBasicInfo.AgainInHospitalReason;

                    txtBeforeHosComaDay.Text = m_IemInfo.IemBasicInfo.BeforeHosComaDay;
                    txtBeforeHosComaHour.Text = m_IemInfo.IemBasicInfo.BeforeHosComaHour;
                    txtBeforeHosComaMinute.Text = m_IemInfo.IemBasicInfo.BeforeHosComaMinute;

                    txtLaterHosComaDay.Text = m_IemInfo.IemBasicInfo.LaterHosComaDay;
                    txtLaterHosComaHour.Text = m_IemInfo.IemBasicInfo.LaterHosComaHour;
                    txtLaterHosComaMinute.Text = m_IemInfo.IemBasicInfo.LaterHosComaMinute;
                    //ASA分级评分
                    txtScore.Text = m_IemInfo.IemBasicInfo.AsaScore;

                    #endregion
                }
                #endregion
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 右键删除
        /// </summary>
        private void DeleteItem()
        {
            try
            {
                if (gridViewOper.FocusedRowHandle < 0)
                {
                    return;
                }

                DataRow dataRow = gridViewOper.GetDataRow(gridViewOper.FocusedRowHandle);
                if (dataRow == null)
                {
                    return;
                }
                if (MyMessageBox.Show("您确认删除吗？", "提示", MyMessageBoxButtons.OkCancel) == DialogResult.Cancel)
                {
                    return;
                }
                DataTable dataTableOper = this.gridControl1.DataSource as DataTable;

                dataTableOper.Rows.Remove(dataRow);

                this.gridControl1.BeginUpdate();
                this.gridControl1.DataSource = dataTableOper;
                m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewOper);
                this.gridControl1.EndUpdate();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// GET UI
        /// </summary>
        private void GetUI()
        {
            try
            {
                if (this.gridControl1.DataSource != null)
                {
                    //手术

                    DataTable dtOperation = m_IemInfo.IemOperInfo.Operation_Table.Clone();
                    dtOperation.Rows.Clear();

                    DataTable dataTable = this.gridControl1.DataSource as DataTable;
                    foreach (DataRow row in dataTable.Rows)
                    {
                        DataRow imOut = dtOperation.NewRow();
                        //Iem_MainPage_Operation imOut = new Iem_MainPage_Operation();

                        imOut["Operation_Code"] = row["Operation_Code"].ToString();
                        imOut["Operation_Name"] = row["Operation_Name"].ToString();
                        imOut["Operation_Date"] = row["Operation_Date"].ToString();

                        imOut["operation_level"] = row["operation_level"].ToString();
                        imOut["operation_level_Name"] = row["operation_level_Name"].ToString();

                        imOut["Execute_User1"] = row["Execute_User1"].ToString();
                        imOut["Execute_User1_Name"] = row["Execute_User1_Name"];
                        imOut["Execute_User2"] = row["Execute_User2"].ToString();
                        imOut["Execute_User2_Name"] = row["Execute_User2_Name"].ToString();
                        imOut["Execute_User3"] = row["Execute_User3"].ToString();
                        imOut["Execute_User3_Name"] = row["Execute_User3_Name"].ToString();
                        imOut["Anaesthesia_Type_Id"] = row["Anaesthesia_Type_Id"].ToString();
                        imOut["Anaesthesia_Type_Name"] = row["Anaesthesia_Type_Name"].ToString();
                        imOut["Close_Level"] = row["Close_Level"].ToString();
                        imOut["Close_Level_Name"] = row["Close_Level_Name"].ToString();
                        imOut["Anaesthesia_User"] = row["Anaesthesia_User"].ToString();
                        imOut["Anaesthesia_User_Name"] = row["Anaesthesia_User_Name"].ToString();
                        imOut["OperInTimes"] = row["OperInTimes"].ToString();
                        dtOperation.Rows.Add(imOut);
                    }

                    m_IemInfo.IemOperInfo.Operation_Table = dtOperation;

                }

                #region 新增项目信息

                if (chkOutHosType1.Checked)
                    m_IemInfo.IemBasicInfo.OutHosType = "1";
                else if (chkOutHosType2.Checked)
                    m_IemInfo.IemBasicInfo.OutHosType = "2";
                else if (chkOutHosType3.Checked)
                    m_IemInfo.IemBasicInfo.OutHosType = "3";
                else if (chkOutHosType4.Checked)
                    m_IemInfo.IemBasicInfo.OutHosType = "4";
                else if (chkOutHosType5.Checked)
                    m_IemInfo.IemBasicInfo.OutHosType = "5";
                else if (chkOutHosType9.Checked)
                    m_IemInfo.IemBasicInfo.OutHosType = "9";
                else
                    m_IemInfo.IemBasicInfo.OutHosType = "";

                m_IemInfo.IemBasicInfo.ReceiveHosPital = txtReceiveHosPital.Text;
                m_IemInfo.IemBasicInfo.ReceiveHosPital2 = txtReceiveHosPital2.Text;

                if (chkAgainInHospital1.Checked)
                    m_IemInfo.IemBasicInfo.AgainInHospital = "1";
                else if (chkAgainInHospital2.Checked)
                    m_IemInfo.IemBasicInfo.AgainInHospital = "2";
                else
                    m_IemInfo.IemBasicInfo.AgainInHospital = "";

                m_IemInfo.IemBasicInfo.AgainInHospitalReason = txtAgainInHospitalReason.Text;

                m_IemInfo.IemBasicInfo.BeforeHosComaDay = txtBeforeHosComaDay.Text;
                m_IemInfo.IemBasicInfo.BeforeHosComaHour = txtBeforeHosComaHour.Text;
                m_IemInfo.IemBasicInfo.BeforeHosComaMinute = txtBeforeHosComaMinute.Text;

                m_IemInfo.IemBasicInfo.LaterHosComaDay = txtLaterHosComaDay.Text;
                m_IemInfo.IemBasicInfo.LaterHosComaHour = txtLaterHosComaHour.Text;
                m_IemInfo.IemBasicInfo.LaterHosComaMinute = txtLaterHosComaMinute.Text;
                //ASA分级评分
                m_IemInfo.IemBasicInfo.AsaScore = txtScore.Text;

                #endregion

            }
            catch (Exception)
            {
                throw;
            }
        }

        IemNewOperInfo m_OperInfoFrom = null;

        /// <summary>
        /// 新增手术记录事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddDiagnose_Click(object sender, EventArgs e)
        {
            try
            {
                m_OperInfoFrom = new IemNewOperInfo(m_App, "new", null);

                m_OperInfoFrom.ShowDialog();

                if (m_OperInfoFrom.DialogResult == DialogResult.OK)
                {
                    m_OperInfoFrom.IemOperInfo = null;
                    DataTable dataTable = m_OperInfoFrom.DataOper;

                    DataTable dataTableOper = new DataTable();
                    if (this.gridControl1.DataSource != null)
                    {
                        dataTableOper = this.gridControl1.DataSource as DataTable;
                    }
                    if (dataTableOper.Rows.Count == 0)
                    {
                        dataTableOper = dataTable.Clone();
                    }
                    DataRow newRow = dataTableOper.NewRow();
                    foreach (DataColumn item in dataTableOper.Columns)
                    {
                        DataRow dataRow = dataTable.Rows[0];
                        if (dataTable.Columns.Contains(item.ColumnName))
                        {
                            newRow[item.ColumnName] = dataRow[item.ColumnName].ToString();
                        }
                    }
                    dataTableOper.Rows.Add(newRow);
                    //foreach (DataRow row in dataTable.Rows)
                    //{
                    //    dataTableOper.ImportRow(row);
                    //}
                    gridControl1.BeginUpdate();
                    this.gridControl1.DataSource = dataTableOper;

                    gridControl1.EndUpdate();
                    m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewOper);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 编辑手术信息界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditOperation_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewOper.FocusedRowHandle < 0)
                {
                    return;
                }
                DataRow dataRow = gridViewOper.GetDataRow(gridViewOper.FocusedRowHandle);
                if (dataRow == null)
                {
                    return;
                }
                DataTable dataTableOper = this.gridControl1.DataSource as DataTable;
                DataTable dataTable = new DataTable();
                dataTable = dataTableOper.Clone();
                dataTable.ImportRow(dataRow);
                m_OperInfoFrom = new IemNewOperInfo(m_App, "edit", dataTable);
                if (m_OperInfoFrom.ShowDialog() == DialogResult.OK)
                {
                    m_OperInfoFrom.IemOperInfo = null;
                    DataTable dtReturn = m_OperInfoFrom.DataOper;
                    //遍历选中行所在表的列若返回表中有该行则更新选中行的该列
                    foreach (DataColumn item in dataRow.Table.Columns)
                    {
                        DataRow rowOper = m_OperInfoFrom.DataOper.Rows[0];
                        if (dtReturn.Columns.Contains(item.ColumnName))
                        {
                            dataRow[item.ColumnName] = rowOper[item.ColumnName].ToString();
                        }
                    }
                    gridControl1.BeginUpdate();
                    this.gridControl1.DataSource = dataTableOper;

                    gridControl1.EndUpdate();
                    m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewOper);
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 重绘事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCIemOperInfo_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                #region 已注销 by xlb 重做了textEdit控件
                //foreach (Control control in this.Controls)
                //{
                //    if (control is LabelControl)
                //    {
                //        control.Visible = false;
                //        e.Graphics.DrawString(control.Text, control.Font, Brushes.Black, control.Location);

                //    }

                //    if (control is TextEdit)
                //    {
                //        e.Graphics.DrawLine(Pens.Black, new Point(control.Location.X, control.Location.Y + control.Height),
                //            new Point(control.Width + control.Location.X, control.Height + control.Location.Y));
                //    }
                //}

                //e.Graphics.DrawLine(Pens.Black, new Point(0, 0), new Point(0, this.Height));
                //e.Graphics.DrawLine(Pens.Black, new Point(this.Width - 1, 0), new Point(this.Width - 1, this.Height));
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 右键删除事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_del_Operinfo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                DeleteItem();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void barManager1_QueryShowPopupMenu(object sender, DevExpress.XtraBars.QueryShowPopupMenuEventArgs e)
        {
            try
            {
                if (e.Control == this.gridControl1)
                {
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void gridControl1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (gridViewOper.FocusedRowHandle < 0)
                    {
                        return;
                    }

                    DataRow dataRow = gridViewOper.GetDataRow(gridViewOper.FocusedRowHandle);
                    this.popupMenu1.ShowPopup(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                    this.btn_operafter_diag.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    this.btn_del_operbefore_diag.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    this.btn_del_Operinfo.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private Inpatient CurrentInpatient;//add by ywk 

        /// <summary>
        /// 保存事件保存数据关闭窗体
        /// Modify by xlb 2013-05-27
        /// 保存不关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_OK_Click(object sender, EventArgs e)
        {
            try
            {
                GetUI();
                //((ShowUC)this.Parent).Close(true, m_IemInfo);

                //点击确认按钮就将数据更新到数据库
                CurrentInpatient = m_App.CurrentPatientInfo;
                CurrentInpatient.ReInitializeAllProperties();
                IemMainPageManger manger = new IemMainPageManger(m_App, CurrentInpatient);
                manger.SaveData(m_IemInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 关闭窗体事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, EventArgs e)
        {
            try
            {
                ((ShowUC)this.Parent).Close(false, null);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }


        /// <summary>
        /// 更改，选中后可消除选择
        /// add by ywk 2012年7月23日 08:54:53
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chk_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckEdit chkEdit = GetCheckEdit(((Control)sender).Name);
                if (chkEdit.Checked)
                {
                    chkEdit.Checked = false;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 根据名称返回控件
        /// </summary>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        private CheckEdit GetCheckEdit(string ControlName)
        {
            try
            {
                foreach (Control control in this.Controls)
                {
                    if (control.Name == ControlName)
                    {
                        return (CheckEdit)control;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除事件
        /// Add by xlb 2013-05-20
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteItem();
            }
            catch (Exception ex)
            {

                MyMessageBox.Show(1, ex);
            }
        }
    }
}
