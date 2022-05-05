﻿using DevExpress.XtraEditors;
using DrectSoft.Common.Eop;
//

using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DrectSoft.Core.IEMMainPage
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


        public void FillUI(IemMainPageInfo info, IEmrHost app)
        {
            try
            {
                m_App = app;
                m_IemInfo = info;
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
                #region 病患基本信息
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
                    this.gridControl1.DataSource = null;
                    this.gridControl1.DataSource = m_IemInfo.IemOperInfo.Operation_Table;

                    m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewOper);

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

                    if (m_IemInfo.IemBasicInfo.Antibacterial_Drugs == "1")
                        chkANTIBACTERIAL1.Checked = true;
                    else if (m_IemInfo.IemBasicInfo.Antibacterial_Drugs == "2")
                        chkANTIBACTERIAL2.Checked = true;

                    if (m_IemInfo.IemBasicInfo.Combined_Medication == "1")
                        chkCOMBINED1.Checked = true;
                    else if (m_IemInfo.IemBasicInfo.Combined_Medication == "2")
                        chkCOMBINED2.Checked = true;
                    txtDURATIONDATE.Text = m_IemInfo.IemBasicInfo.Durationdate;
                    /// <summary>
                    /// 是否实施临床路径管理
                    /// </summary>
                    if (m_IemInfo.IemBasicInfo.Pathway_Flag == "1")
                        chkPATHWAY_FLAG1.Checked = true;
                    else if (m_IemInfo.IemBasicInfo.Pathway_Flag == "2")
                        chkPATHWAY_FLAG2.Checked = true;

                    /// <summary>
                    /// 是否完成临床路径
                    /// </summary>
                    if (m_IemInfo.IemBasicInfo.Pathway_Over == "1")
                        chkPATHWAY_OVER1.Checked = true;
                    else if (m_IemInfo.IemBasicInfo.Pathway_Over == "2")
                        chkPATHWAY_OVER2.Checked = true;

                    /// <summary>
                    /// 是否变异
                    /// </summary>
                    if (m_IemInfo.IemBasicInfo.Variation_Flag == "1")
                        chkVARIATION_FLAG1.Checked = true;
                    else if (m_IemInfo.IemBasicInfo.Variation_Flag == "2")
                        chkVARIATION_FLAG2.Checked = true;

                    /// <summary>
                    /// 是否因同一病种再入院
                    /// </summary>
                    if (m_IemInfo.IemBasicInfo.Rehospitalization == "1")
                        chkREHOSPITALIZATION1.Checked = true;
                    else if (m_IemInfo.IemBasicInfo.Rehospitalization == "2")
                        chkREHOSPITALIZATION2.Checked = true;


                    txtpath_out.Text = m_IemInfo.IemBasicInfo.Path_Out_Reason;
                    txtvariation.Text = m_IemInfo.IemBasicInfo.Variation_Reason;
                    txtintervaldate.Text = m_IemInfo.IemBasicInfo.Intervaldate;

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


                    #region 四川医院信息
                    textEditct.Text = m_IemInfo.IemBasicInfo.Inspect_CT.ToString();
                    textEditPETCT.Text = m_IemInfo.IemBasicInfo.Inspect_PETCT.ToString();
                    textEdittoct.Text = m_IemInfo.IemBasicInfo.Inspect_TOCT.ToString();
                    textEditx.Text = m_IemInfo.IemBasicInfo.Inspect_X.ToString();
                    textEdituc.Text = m_IemInfo.IemBasicInfo.Inspect_UC.ToString();
                    textEditmri.Text = m_IemInfo.IemBasicInfo.Inspect_MRI.ToString();
                    textEditb.Text = m_IemInfo.IemBasicInfo.Inspect_B.ToString();
                    textEditISOTOPE.Text = m_IemInfo.IemBasicInfo.Inspect_ISOTOPE.ToString();
                    #endregion
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
        /// GET UI
        /// </summary>
        private void GetUI()
        {
            try
            {
                #region 手术信息
                if (this.gridControl1.DataSource != null)
                {
                    //手术

                    DataTable dtOperation = m_IemInfo.IemOperInfo.Operation_Table.Clone();
                    dtOperation.Rows.Clear();
                    int i = 0;
                    DataTable dataTable = this.gridControl1.DataSource as DataTable;
                    foreach (DataRow row in dataTable.Rows)
                    {
                        DataRow imOut = dtOperation.NewRow();

                        imOut["Operation_Code"] = row["Operation_Code"].ToString();
                        imOut["Operation_Name"] = row["Operation_Name"].ToString();
                        imOut["Operation_Date"] = row["Operation_Date"].ToString();
                        imOut["Operation_EndDate"] = row["Operation_EndDate"].ToString();
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
                        imOut["Complication_Code"] = row["Complication_Code"].ToString();
                        imOut["Complication_Name"] = row["Complication_Name"].ToString();
                        imOut["MAINOPERATION"] = row["MAINOPERATION"].ToString();
                        imOut["IATROGENIC"] = row["IATROGENIC"].ToString();
                        imOut["ISCHOOSEDATE"] = row["ISCHOOSEDATE"].ToString();

                        dtOperation.Rows.Add(imOut);
                    }

                    m_IemInfo.IemOperInfo.Operation_Table = dtOperation;

                }
                #endregion


                #region 2012新增项目信息

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
                //Ⅰ类手术切口预防性应用抗菌药物
                if (chkANTIBACTERIAL1.Checked)
                    m_IemInfo.IemBasicInfo.Antibacterial_Drugs = "1";
                else if (chkANTIBACTERIAL2.Checked)
                    m_IemInfo.IemBasicInfo.Antibacterial_Drugs = "2";
                else
                    m_IemInfo.IemBasicInfo.Antibacterial_Drugs = "";
                //联合用药
                if (chkCOMBINED1.Checked)
                    m_IemInfo.IemBasicInfo.Combined_Medication = "1";
                else if (chkCOMBINED2.Checked)
                    m_IemInfo.IemBasicInfo.Combined_Medication = "2";
                else
                    m_IemInfo.IemBasicInfo.Combined_Medication = "";
                //使用持续时间
                m_IemInfo.IemBasicInfo.Durationdate = txtDURATIONDATE.Text;

                if (chkPATHWAY_FLAG1.Checked)
                    m_IemInfo.IemBasicInfo.Pathway_Flag = "1";
                else if (chkPATHWAY_FLAG2.Checked)
                    m_IemInfo.IemBasicInfo.Pathway_Flag = "2";
                else
                    m_IemInfo.IemBasicInfo.Pathway_Flag = "";


                if (chkPATHWAY_OVER1.Checked)
                    m_IemInfo.IemBasicInfo.Pathway_Over = "1";
                else if (chkPATHWAY_OVER2.Checked)
                    m_IemInfo.IemBasicInfo.Pathway_Over = "2";
                else
                    m_IemInfo.IemBasicInfo.Pathway_Over = "";


                if (chkVARIATION_FLAG1.Checked)
                    m_IemInfo.IemBasicInfo.Variation_Flag = "1";
                else if (chkVARIATION_FLAG2.Checked)
                    m_IemInfo.IemBasicInfo.Variation_Flag = "2";
                else
                    m_IemInfo.IemBasicInfo.Variation_Flag = "";


                if (chkREHOSPITALIZATION1.Checked)
                    m_IemInfo.IemBasicInfo.Rehospitalization = "1";
                else if (chkREHOSPITALIZATION2.Checked)
                    m_IemInfo.IemBasicInfo.Rehospitalization = "2";
                else
                    m_IemInfo.IemBasicInfo.Rehospitalization = "";


                m_IemInfo.IemBasicInfo.Path_Out_Reason = txtpath_out.Text;
                m_IemInfo.IemBasicInfo.Variation_Reason = txtvariation.Text;
                m_IemInfo.IemBasicInfo.Intervaldate = txtintervaldate.Text;


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


                #endregion

                #region 四川医院信息
                m_IemInfo.IemBasicInfo.Inspect_CT = Convert.ToInt32(textEditct.Text);
                m_IemInfo.IemBasicInfo.Inspect_PETCT = Convert.ToInt32(textEditPETCT.Text);
                m_IemInfo.IemBasicInfo.Inspect_TOCT = Convert.ToInt32(textEdittoct.Text);
                m_IemInfo.IemBasicInfo.Inspect_X = Convert.ToInt32(textEditx.Text);
                m_IemInfo.IemBasicInfo.Inspect_UC = Convert.ToInt32(textEdituc.Text);
                m_IemInfo.IemBasicInfo.Inspect_MRI = Convert.ToInt32(textEditmri.Text);
                m_IemInfo.IemBasicInfo.Inspect_B = Convert.ToInt32(textEditb.Text);
                m_IemInfo.IemBasicInfo.Inspect_ISOTOPE = Convert.ToInt32(textEditISOTOPE.Text);
                #endregion

            }
            catch (Exception)
            {
                throw;
            }
        }

        IemNewOperInfo m_OperInfoFrom = null;

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
                        dataTableOper = this.gridControl1.DataSource as DataTable;
                    if (dataTableOper.Rows.Count == 0)
                        dataTableOper = dataTable.Clone();
                    foreach (DataRow row in dataTable.Rows)
                    {
                        dataTableOper.ImportRow(row);
                    }
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
        /// 解决点编辑记录蹿位置问题
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
                MessageBox.Show(ex.Message);
            }
        }

        private void UCIemOperInfo_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                foreach (Control control in this.Controls)
                {
                    if (control is LabelControl)
                    {
                        control.Visible = false;
                        e.Graphics.DrawString(control.Text, control.Font, Brushes.Black, control.Location);

                    }

                    if (control is TextEdit)
                    {
                        e.Graphics.DrawLine(Pens.Black, new Point(control.Location.X, control.Location.Y + control.Height),
                            new Point(control.Width + control.Location.X, control.Height + control.Location.Y));
                    }
                }

                //e.Graphics.DrawLine(Pens.Black, new Point(0, 0), new Point(0, this.Height));
                //e.Graphics.DrawLine(Pens.Black, new Point(this.Width - 1, 0), new Point(this.Width - 1, this.Height));
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private void btn_del_Operinfo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (gridViewOper.FocusedRowHandle < 0)
                    return;
                else
                {
                    DataRow dataRow = gridViewOper.GetDataRow(gridViewOper.FocusedRowHandle);
                    if (dataRow == null)
                        return;

                    DataTable dataTableOper = this.gridControl1.DataSource as DataTable;

                    dataTableOper.Rows.Remove(dataRow);

                    this.gridControl1.BeginUpdate();
                    this.gridControl1.DataSource = dataTableOper;
                    m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewOper);
                    this.gridControl1.EndUpdate();

                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private void gridControl1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (gridViewOper.FocusedRowHandle < 0)
                        return;
                    else
                    {
                        DataRow dataRow = gridViewOper.GetDataRow(gridViewOper.FocusedRowHandle);
                        this.popupMenu1.ShowPopup(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                        this.btn_operafter_diag.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                        this.btn_del_operbefore_diag.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                        this.btn_del_Operinfo.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }


        private Inpatient CurrentInpatient;//add by ywk 
        private void btn_OK_Click(object sender, EventArgs e)
        {
            try
            {
                GetUI();
                //点击确认按钮就将数据更新到数据库
                CurrentInpatient = m_App.CurrentPatientInfo;
                CurrentInpatient.ReInitializeAllProperties();
                IemMainPageManger manger = new IemMainPageManger(m_App, CurrentInpatient);
                manger.SaveData(m_IemInfo);
                btn_Close_Click(sender, e);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            try
            {
                ((ShowUC)this.Parent).Close(false, null);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
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
            catch (Exception)
            {
                throw;
            }
        }

    }
}
