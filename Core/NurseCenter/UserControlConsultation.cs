using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraNavBar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Core.Consultation.NEW;
using DrectSoft.Core.Consultation.NEW.Enum;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.DSSqlHelper;

//Add by wwj 2013-02-18 用于护士工作站中右侧的会诊功能
//只捞取待会诊（6730）、会诊记录保存（6740）、会诊完成的记录（6741）

namespace DrectSoft.Emr.NurseCenter
{
    public partial class UserControlConsultation : DevExpress.XtraEditors.XtraUserControl
    {
        #region Field
        IEmrHost m_App;
        NavBarGroup m_NavBarGroup;
        StringFormat m_StringFormat = new StringFormat();
        NurseForm m_NurseForm;
        GridView m_GridViewMain;

        string m_IsFeeState = "-1";//会诊缴费情况 -1：不需要收费
        #endregion

        #region Property
        bool BtnRemindEnabled
        {
            get
            {
                return simpleButtonRemind.Enabled;
            }
            set
            {
                simpleButtonRemind.Enabled = value;
                barButtonItemRemind.Visibility = value ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;
            }
        }

        bool BtnConsultFeeEnable
        {
            get
            {
                return simpleButtonConsultFee.Enabled;
            }
            set
            {
                simpleButtonConsultFee.Enabled = value;
                if (m_IsFeeState == "-1")//会诊缴费未开启，则收费按钮不显示
                {
                    barButtonItemFee.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
                else
                {
                    barButtonItemFee.Visibility = value ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;
                }
            }
        }

        //bool BtnCancelConsultEnable
        //{
        //    get
        //    {
        //        return simpleButtonCancelConsult.Enabled;
        //    }
        //    set
        //    {
        //        simpleButtonCancelConsult.Enabled = value;
        //        barButtonItemCancel.Visibility = value ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;
        //    }
        //}

        bool BtnSignEnable
        {
            get
            {
                return simpleButtonSign.Enabled;
            }
            set
            {
                simpleButtonSign.Enabled = value;
                barButtonItemSign.Visibility = value ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;
            }
        }

        bool BtnPrintEnable
        {
            get
            {
                return simpleButtonPrint.Enabled;
            }
            set
            {
                simpleButtonPrint.Enabled = value;
                barButtonItemPrint.Visibility = value ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;
            }
        }
        #endregion

        #region .ctor
        public UserControlConsultation()
        {
            InitializeComponent();
        }

        public UserControlConsultation(IEmrHost app, NavBarGroup barGroup, NurseForm form, GridView gv)
            : this()
        {
            m_StringFormat.Alignment = StringAlignment.Center;
            m_StringFormat.LineAlignment = StringAlignment.Center;
            m_App = app;
            m_NavBarGroup = barGroup;
            m_NurseForm = form;
            m_GridViewMain = gv;
        }
        #endregion

        #region Load
        private void UserControlConsultation_Load(object sender, EventArgs e)
        {
            try
            {
                InitConsultSetting();
                SearchConsultation();
                SetButtonEnable(false);
                SetBtnVisible();
                SetBtnFeeInterfaceVisible();
                //禁掉取消会诊右键
                barButtonItemCancel.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 初始化会诊设置信息
        /// </summary>
        private void InitConsultSetting()
        {
            try
            {
                DataTable dtConsultState = GetAllConsultState();
                lookUpEditConsultState.Properties.DisplayMember = "NAME";
                lookUpEditConsultState.Properties.ValueMember = "ID";
                lookUpEditConsultState.Properties.DataSource = dtConsultState;
                lookUpEditConsultState.ItemIndex = 0;

                dateEditConsultDateFrom.Text = System.DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
                dateEditConsultDateTo.Text = System.DateTime.Now.ToString("yyyy-MM-dd");

                linkLabelUnFee.Text = "已会诊未缴费记录【" + GetAlreadyConsultNotFee().Rows.Count + "】个 (点击查看详细)";

                m_IsFeeState = GetAppcfgConsult("ConsultFeeTime");//会诊缴费时机的配置
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获得所有的会诊状态 只包括“待会诊”、“会诊记录保存”、“会诊记录完成”
        /// </summary>
        /// <returns></returns>
        private DataTable GetAllConsultState()
        {
            try
            {
                DataTable dt = DS_SqlHelper.ExecuteDataTable(
                    @"SELECT to_char(ID) ID, NAME FROM categorydetail c 
                       WHERE c.categoryid = 67 AND c.id in (6730,6740,6741)
                       ORDER BY ID",
                    CommandType.Text);
                DataRow dr = dt.NewRow();
                dr["ID"] = "";
                dr["NAME"] = "全部";
                dt.Rows.InsertAt(dr, 0);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 查询
        private void simpleButtonConsultSearch_Click(object sender, EventArgs e)
        {
            try
            {
                SearchConsultation();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 查询会诊信息
        /// </summary>
        private void SearchConsultation()
        {
            try
            {
                gridControlMyDept.DataSource = null;
                gridControlOtherDept.DataSource = null;

                DataTable dtConsultationNurse = GetConsultationNurse();
                RefreshConsultation(dtConsultationNurse);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private DataTable GetConsultationNurse()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] sqlParams = new SqlParameter[] { 
                    new SqlParameter("@Deptids", SqlDbType.VarChar),
                    new SqlParameter("@ConsultDateFrom", SqlDbType.Date),
                    new SqlParameter("@ConsultDateTo", SqlDbType.Date),
                    new SqlParameter("@ConsultState", SqlDbType.VarChar),
                    new SqlParameter("@result",SqlDbType.Structured)
                };
                sqlParams[0].Value = m_App.User.CurrentDeptId.Trim(); //用户当前所在科室
                sqlParams[1].Value = Convert.ToDateTime(dateEditConsultDateFrom.EditValue.ToString());
                sqlParams[2].Value = Convert.ToDateTime(dateEditConsultDateTo.EditValue.ToString());
                sqlParams[3].Value = lookUpEditConsultState.EditValue.ToString();
                sqlParams[4].Direction = ParameterDirection.Output;
                dt = DS_SqlHelper.ExecuteDataTable("emr_consultation.usp_GetConsultionForNurse", sqlParams, CommandType.StoredProcedure);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 刷新会诊列表
        /// </summary>
        /// <param name="dtFilter"></param>
        private void RefreshConsultation(DataTable dtConsultation)
        {
            try
            {
                List<DataRow> drList = new List<DataRow>();

                #region 过滤缴费情况
                if (checkEditFee.Checked && !checkEditUnFee.Checked)//已缴费
                {
                    drList = dtConsultation.Select().Where((dr) =>
                    {
                        if (dr["ISPAY"].ToString() == "1")
                            return true;
                        else
                            return false;
                    }).ToList<DataRow>();
                }
                else if (!checkEditFee.Checked && checkEditUnFee.Checked)//未缴费
                {
                    drList = dtConsultation.Select().Where((dr) =>
                    {
                        if (dr["ISPAY"].ToString() != "1")
                            return true;
                        else
                            return false;
                    }).ToList<DataRow>();
                }
                else//不分
                {
                    drList = dtConsultation.Select().ToList<DataRow>();
                }
                #endregion

                if (drList.Count > 0)
                {
                    //本科室
                    DataRow[] drMyDept = drList.Where((dr) =>
                    {
                        if (dr["OUTHOSDEPT"].ToString() == m_App.User.CurrentDeptId)
                            return true;
                        else
                            return false;
                    }).ToArray();
                    if (drMyDept.Length > 0)
                    {
                        gridControlMyDept.DataSource = drMyDept.CopyToDataTable();
                    }

                    //其他系统
                    DataRow[] drOtherDept = drList.Where((dr) =>
                    {
                        if (dr["OUTHOSDEPT"].ToString() != m_App.User.CurrentDeptId)
                            return true;
                        else
                            return false;
                    }).ToArray();
                    if (drOtherDept.Length > 0)
                    {
                        gridControlOtherDept.DataSource = drOtherDept.CopyToDataTable();
                    }

                    //待会诊记录数据
                    int waitConsultationCount = drList.Where((dr) =>
                    {
                        if (dr["STATEID"].ToString() == Convert.ToString((int)ConsultStatus.WaitConsultation))
                            return true;
                        else
                            return false;
                    }).Count();

                    m_NavBarGroup.Caption = "会诊信息【待会诊：" + waitConsultationCount + "】";
                }
            }
            catch (Exception ex)
            {
                m_App.CustomMessageBox.MessageShow(ex.Message);
                throw ex;
            }
        }
        #endregion

        #region 控制GridControl中“紧急度”和“缴费”情况的字体颜色
        private void gridView3_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                if (e.CellValue == null) return;
                GridView gv = sender as GridView;

                if (gv == null) return;

                DataRowView drv = gv.GetRow(e.RowHandle) as DataRowView;

                string payvalue = drv["mypay"].ToString().Trim();
                string ispay = drv["ispay"].ToString();
                if (e.CellValue.ToString() == "紧急")
                {
                    e.Appearance.ForeColor = Color.Red;
                }

                if (e.Column.Caption == "缴费")
                {
                    //if (payvalue == "已缴费")
                    if (ispay == "1")
                    {
                        e.Graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, e.Bounds.Width, e.Bounds.Height));
                        e.Graphics.DrawString(payvalue, e.Appearance.Font, Brushes.Green, e.Bounds, m_StringFormat);//已交费要绿色字体
                        e.Handled = true;
                    }
                    else
                    {
                        e.Graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, e.Bounds.Width, e.Bounds.Height));
                        e.Graphics.DrawString(payvalue, e.Appearance.Font, Brushes.Red, e.Bounds, m_StringFormat);//未交费要红色字体
                        e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region 控制“本科室”、“其他科室”的可见性
        private void checkEditMyDept_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!checkEditMyDept.Checked)
                {
                    checkEditOtherDept.Checked = true;
                }
                ControlGridControlVisible();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private void checkEditOtherDept_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!checkEditOtherDept.Checked)
                {
                    checkEditMyDept.Checked = true;
                }
                ControlGridControlVisible();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private void ControlGridControlVisible()
        {
            try
            {
                if (checkEditMyDept.Checked && !checkEditOtherDept.Checked)
                {
                    gridControlMyDept.Visible = true;
                    gridControlMyDept.Dock = DockStyle.Fill;
                    gridControlOtherDept.Visible = false;
                }
                else if (!checkEditMyDept.Checked && checkEditOtherDept.Checked)
                {
                    gridControlOtherDept.Visible = true;
                    gridControlOtherDept.Dock = DockStyle.Fill;
                    gridControlMyDept.Visible = false;
                }
                else
                {
                    gridControlMyDept.Visible = true;
                    gridControlMyDept.Dock = DockStyle.Fill;
                    gridControlOtherDept.Visible = true;
                    gridControlOtherDept.Dock = DockStyle.Right;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 已会诊未缴费的记录
        /// <summary>
        /// 获得已会诊未缴费的记录
        /// </summary>
        /// <returns></returns>
        public DataTable GetAlreadyConsultNotFee()
        {
            try
            {
                //已会诊未缴费的记录
                SqlParameter[] sqlParams1 = new SqlParameter[] { 
                    new SqlParameter("@Deptids", SqlDbType.VarChar),
                    new SqlParameter("@result",SqlDbType.Structured)
                };
                sqlParams1[0].Value = m_App.User.CurrentDeptId; // 科室代码
                sqlParams1[1].Direction = ParameterDirection.Output;
                return DS_SqlHelper.ExecuteDataTable("emr_consultation.usp_GetAlreadyConsultNotFee", sqlParams1, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void linkLabelUnFee_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                PayConsultion payCon = new PayConsultion(m_App, GetAlreadyConsultNotFee(), m_NurseForm);
                payCon.HaveConsultationNotFee = GetAlreadyConsultNotFee;
                payCon.Pay = GoPay;
                payCon.StartPosition = FormStartPosition.CenterParent;
                payCon.ShowDialog();
                linkLabelUnFee.Text = "已会诊未缴费记录【" + GetAlreadyConsultNotFee().Rows.Count + "】个 (点击查看详细)";
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region 取消会诊
        private void simpleButtonCancelConsult_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewMyDept.FocusedRowHandle < 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择需要取消的会诊记录！");
                    return;
                }
                DataRow foucesRow = gridViewMyDept.GetDataRow(gridViewMyDept.FocusedRowHandle);
                if (foucesRow == null) return;
                if (foucesRow.IsNull("NOOFINPAT")) return;

                //插入的User是可以接收到消息的人ID
                string typeid = "2";//类型编号
                string userid = foucesRow["applyuser"].ToString();//会诊申请人工号
                string username = foucesRow["applyusername"].ToString();//申请人姓名
                string tcontent = "时间：" + foucesRow["consulttime"].ToString();//通知内容
                int painentid = int.Parse(foucesRow["NOOFINPAT"].ToString());//病人编号
                string inpatientname = foucesRow["inpatientname"].ToString();//病人姓名
                string applysn = foucesRow["consultapplysn"].ToString();//会诊申请单表编号
                int valid = 1;//是否有效

                //验证是否可以取消费用，即是否已经交过费用
                if (!CheckIsCanCancelFee(applysn, foucesRow))
                {
                    return;
                }

                if (MessageBox.Show("是否取消对患者【" + inpatientname + "】会诊?", "提示", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    #region 提醒申请人
                    //先插入申请人
                    string insertsql = string.Format(
                        @" insert into nurse_withinformation(id, typeid, userid, username, tcontent, painetid, valid, painetname) 
                           values(seq_Nurse_WithInformation_ID.NEXTVAL,'{0}','{1}','{2}','{3}','{4}','{5}','{6}') ",
                           typeid, userid, username, tcontent, painentid, valid, inpatientname);
                    DS_SqlHelper.ExecuteNonQuery(insertsql);
                    #endregion

                    #region 提醒受邀医师
                    //获取受邀医师
                    string applySn = foucesRow["consultapplysn"].ToString();
                    string sqlGetShouYao = @" select * from consultrecorddepartment where consultapplysn = '{0}' and valid = '1' ";
                    DataTable dtApplyDepartment = DS_SqlHelper.ExecuteDataTable(string.Format(sqlGetShouYao, applySn));
                    foreach (DataRow dr in dtApplyDepartment.Rows)
                    {
                        //受邀人为空就插入同一级别的，不为空就插入受邀人
                        if (string.IsNullOrEmpty(dr["employeecode"].ToString()))
                        {
                            #region 提醒同一级别的所有医师
                            string sylevel = dr["employeelevelid"].ToString();//受邀人级别
                            string syDeptID = dr["departmentcode"].ToString();//受邀人部门
                            string searchusers = string.Format(@"select id ,name from users where grade = '{0}' and deptid = '{1}' ", sylevel, syDeptID);
                            DataTable dt = DS_SqlHelper.ExecuteDataTable(searchusers);
                            if (dt.Rows.Count > 0)
                            {
                                string myuserid = "";//属于同级别人员编号
                                string myusername = "";//属于同级别人员姓名
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    myuserid = dt.Rows[i]["id"].ToString();
                                    myusername = dt.Rows[i]["name"].ToString();
                                    string insertsql11 = string.Format(
                                        @" insert into nurse_withinformation(id, typeid, userid, username, tcontent, painetid, valid, painetname) 
                                           values (seq_Nurse_WithInformation_ID.NEXTVAL,'{0}','{1}','{2}','{3}','{4}','{5}','{6}') ",
                                           typeid, myuserid, myusername, tcontent, painentid, valid, inpatientname);
                                    DS_SqlHelper.ExecuteNonQuery(insertsql11);
                                }
                            }
                            #endregion
                        }
                        else//不为空就插入受邀人
                        {
                            #region 提醒指定的受邀医师
                            string shouyaouserid = dr["employeecode"].ToString();//受邀人编号
                            string shouyaousername = dr["employeename"].ToString();//受邀人姓名
                            string insertsql1 = string.Format(
                                @" insert into nurse_withinformation(id, typeid, userid, username, tcontent, painetid, valid, painetname)
                                   values (seq_Nurse_WithInformation_ID.NEXTVAL,'{0}','{1}','{2}','{3}','{4}','{5}','{6}') ",
                                   typeid, shouyaouserid, shouyaousername, tcontent, painentid, valid, inpatientname);
                            DS_SqlHelper.ExecuteNonQuery(insertsql1);
                            #endregion
                        }
                    }
                    #endregion

                    string feeID = "2";//会诊取消费用的ID固定是2
                    string resultMessage = string.Empty;

                    if (/*验证取消缴费接口的有效性*/CheckIsOpenFee(feeID, true, out resultMessage)
                        && /*验证成功后调用费用接口程序*/!CallConsultInterface(applysn, out resultMessage, feeID))
                    {
                        MessageBox.Show(resultMessage);
                    }
                    else
                    {
                        string updatesql = string.Format(@"update ConsultApply set stateid='6770',canceluser='{0}',canceltime='{1}' where consultapplysn='{2}' and valid='1'", m_App.User.Id, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), applysn);
                        DS_SqlHelper.ExecuteNonQuery(updatesql);

                        foucesRow.Delete();
                        gridViewMyDept.FocusedRowHandle = -1;
                        gridViewOtherDept.FocusedRowHandle = -1;

                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("会诊取消成功！");
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private void barButtonItemCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                simpleButtonCancelConsult_Click(null, null);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 验证是否可以取消此次会诊单的费用
        /// </summary>
        /// <param name="consultApplySn"></param>
        /// <returns></returns>
        private bool CheckIsCanCancelFee(string consultApplySn, DataRow dr)
        {
            try
            {
                string sqlGetIsPay = string.Format(@"select c.ispay from consultapply c where c.consultapplysn = '{0}' and c.valid = '1'", consultApplySn);
                DataTable dt = DS_SqlHelper.ExecuteDataTable(sqlGetIsPay);
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("此会诊记录已经作废！");
                    dr.Delete();
                    return false;
                }
                else
                {
                    if (dt.Rows[0]["ISPAY"].ToString() != "1")
                    {
                        //MessageBox.Show("此会诊记录没有缴费不能取消缴费！");
                        dr["ISPAY"] = "0";
                        dr["MYPAY"] = "未缴费";
                        BtnConsultFeeEnable = true;
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 会诊提醒
        private void simpleButtonRemind_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow foucesRow = simpleButtonRemind.Tag as DataRow;
                if (foucesRow == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择选择需要提醒的会诊记录！");
                    return;
                }

                #region 提醒申请会诊的医师
                string typeid = "1";//类型编号
                string userid = foucesRow["applyuser"].ToString();//会诊申请人工号
                string username = foucesRow["applyusername"].ToString();//申请人姓名
                string tcontent = "时间：" + foucesRow["consulttime"].ToString();//通知内容
                int painentid = int.Parse(foucesRow["NOOFINPAT"].ToString());//病人编号
                string inpatientname = foucesRow["inpatientname"].ToString();//病人姓名
                int valid = 1;//是否有效

                //要插入申请人和受邀人和同一级别的人
                string insertsql = string.Format(@" insert into nurse_withinformation(id, typeid, userid, username, tcontent, painetid, valid, painetname)  
                    values(seq_Nurse_WithInformation_ID.NEXTVAL,'{0}','{1}','{2}','{3}','{4}','{5}','{6}') ",
                    typeid, userid, username, tcontent, painentid, valid, inpatientname);
                DS_SqlHelper.ExecuteNonQuery(insertsql);
                #endregion

                #region 提醒会诊受邀医师
                string applySn = foucesRow["consultapplysn"].ToString();
                string sqlGetShouYao = @" select * from consultrecorddepartment where consultapplysn = '{0}' and valid = '1' ";
                DataTable dtApplyDepartment = DS_SqlHelper.ExecuteDataTable(string.Format(sqlGetShouYao, applySn));

                foreach (DataRow dr in dtApplyDepartment.Rows)
                {
                    //受邀人为空就插入同一级别的，不为空就插入受邀人
                    if (string.IsNullOrEmpty(dr["employeecode"].ToString()))
                    {
                        #region 通知同一个科室，同一级别的医师
                        string sylevel = dr["employeelevelid"].ToString();//受邀人级别
                        string syDeptID = dr["departmentcode"].ToString();//受邀人部门
                        string searchusers = string.Format(@"select id ,name from users where grade = '{0}' and deptid = '{1}' ", sylevel, syDeptID);
                        DataTable dt = DS_SqlHelper.ExecuteDataTable(searchusers);
                        if (dt.Rows.Count > 0)
                        {
                            string myuserid = "";//属于同级别人员编号
                            string myusername = "";//属于同级别人员姓名
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                myuserid = dt.Rows[i]["id"].ToString();
                                myusername = dt.Rows[i]["name"].ToString();
                                string insertsql11 = string.Format(@" insert into nurse_withinformation(id, typeid, userid, username, tcontent, painetid, valid, painetname)  
                                    values(seq_Nurse_WithInformation_ID.NEXTVAL,'{0}','{1}','{2}','{3}','{4}','{5}','{6}') ",
                                    typeid, myuserid, myusername, tcontent, painentid, valid, inpatientname);
                                DS_SqlHelper.ExecuteNonQuery(insertsql11);
                            }
                        }
                        #endregion
                    }
                    else//不为空就插入受邀人
                    {
                        #region 通知指定的受邀医师
                        string shouyaouserid = dr["employeecode"].ToString();//受邀人编号
                        string shouyaousername = dr["employeename"].ToString();//受邀人姓名
                        string insertsql1 = string.Format(@" insert into nurse_withinformation(id, typeid, userid, username, tcontent, painetid, valid, painetname)  
                            values(seq_Nurse_WithInformation_ID.NEXTVAL,'{0}','{1}','{2}','{3}','{4}','{5}','{6}') ",
                            typeid, shouyaouserid, shouyaousername, tcontent, painentid, valid, inpatientname);
                        DS_SqlHelper.ExecuteNonQuery(insertsql1);
                        #endregion
                    }
                }
                #endregion

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("会诊信息提醒成功");
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private void barButtonItemRemind_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                simpleButtonRemind_Click(null, null);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        #endregion



        #region 设置按钮的样式
        /// <summary>
        /// 设置按钮可用性
        /// </summary>
        /// <param name="isEnable"></param>
        private void SetButtonEnable(bool isEnable)
        {
            try
            {
                BtnRemindEnabled = isEnable;
                BtnConsultFeeEnable = isEnable;
                //BtnCancelConsultEnable = isEnable;
                BtnSignEnable = isEnable;
                BtnPrintEnable = isEnable;

                if (!isEnable)
                {
                    SetFocusedRowToBtnTag(null);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 控制按钮样式 会诊记录保存、会诊完成状态时不能进行会诊提醒和会诊取消操作
        /// </summary>
        /// <param name="status"></param>
        private void ControlButtonStyle(DataRow focusRow)
        {
            try
            {
                string consultStatus = focusRow["stateid"].ToString();//会诊状态
                string isPay = focusRow["ispay"].ToString();//是否缴费
                ConsultStatus status = (ConsultStatus)Enum.Parse(typeof(ConsultStatus), consultStatus);
                SetButtonEnable(true);
                switch (status)
                {
                    //待会诊
                    case ConsultStatus.WaitConsultation:
                        break;
                    //会诊记录保存
                    case ConsultStatus.RecordeSave:
                        BtnRemindEnabled = false;//会诊提醒禁掉
                        // BtnCancelConsultEnable = false;//会诊取消禁掉
                        break;
                    //会诊完成
                    case ConsultStatus.RecordeComplete:
                        BtnRemindEnabled = false;//会诊提醒禁掉
                        //BtnCancelConsultEnable = false;//会诊取消禁掉
                        BtnSignEnable = false;//医师签到禁掉
                        break;
                    default:
                        SetButtonEnable(false);
                        break;
                }

                //是否缴费
                if (isPay == "1")
                {
                    BtnConsultFeeEnable = false;
                }

                SetFocusedRowToBtnTag(focusRow);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 把选中的行记录到对应Button的Tag属性中
        /// </summary>
        /// <param name="dr"></param>
        private void SetFocusedRowToBtnTag(DataRow dr)
        {
            try
            {
                simpleButtonRemind.Tag = dr;
                simpleButtonConsultFee.Tag = dr;
                //simpleButtonCancelConsult.Tag = dr;
                simpleButtonSign.Tag = dr;
                simpleButtonPrint.Tag = dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 如果不需要会诊缴费，则缴费相关的按钮不显示
        /// </summary>
        private void SetBtnVisible()
        {
            try
            {
                if (m_IsFeeState == "-1")//如果不需要会诊缴费，则缴费按钮不显示
                {
                    simpleButtonConsultFee.Visible = false;
                    barButtonItemFee.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    linkLabelUnFee.Visible = false;
                    checkEditFee.Enabled = false;
                    checkEditUnFee.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 控制缴费和未缴费不能为空
        private void checkEditUnFee_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkEditUnFee.Checked && !checkEditFee.Checked)
            {
                checkEditFee.Checked = true;
            }
        }

        private void checkEditFee_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkEditUnFee.Checked && !checkEditFee.Checked)
            {
                checkEditUnFee.Checked = true;
            }
        }
        #endregion

        #region 右键弹出菜单
        private void gridControlMyDept_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                GridControlMouseDown(gridControlMyDept, gridViewMyDept, e);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private void gridControlOtherDept_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                GridControlMouseDown(gridControlOtherDept, gridViewOtherDept, e);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 列表鼠标方法
        /// </summary>
        /// <param name="gc"></param>
        /// <param name="gv"></param>
        /// <param name="e"></param>
        private void GridControlMouseDown(GridControl gc, GridView gv, MouseEventArgs e)
        {
            try
            {
                //本科室列表焦点行改变
                this.gridViewMyDept.FocusedRowChanged -= new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridViewMyDept_FocusedRowChanged);
                //其他科室焦点行改变
                this.gridViewOtherDept.FocusedRowChanged -= new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(gridViewOtherDept_FocusedRowChanged);

                if (gv == gridViewMyDept)
                {
                    gridViewOtherDept.FocusedRowHandle = -1;
                }
                else if (gv == gridViewOtherDept)
                {
                    gridViewMyDept.FocusedRowHandle = -1;
                }

                Point position = Cursor.Position;
                GridHitInfo hitInfo = gv.CalcHitInfo(gc.PointToClient(position));
                DataRow focusRow = hitInfo.RowHandle >= 0 ? gv.GetDataRow(hitInfo.RowHandle) : null;
                if (focusRow == null)
                {
                    SetButtonEnable(false);
                    gv.FocusedRowHandle = -1;
                }
                else
                {
                    //控制按钮的样式
                    ControlButtonStyle(focusRow);
                }
                ChangeConsultRecord(focusRow);

                if (hitInfo.RowHandle >= 0 && e.Button == MouseButtons.Right)
                {
                    this.popupMenu1.ShowPopup(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.gridViewMyDept.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridViewMyDept_FocusedRowChanged);
                this.gridViewOtherDept.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(gridViewOtherDept_FocusedRowChanged);
            }
        }
        #endregion

        #region 会诊单打印

        private void simpleButtonPrint_Click(object sender, EventArgs e)
        {
            try
            {
                Print();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }


        private void barButtonItemPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                Print();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 打印会诊单
        /// </summary>
        private void Print()
        {
            try
            {
                DataRow dataRow = simpleButtonPrint.Tag as DataRow;
                if (dataRow == null)
                {
                    return;
                }
                string noofinapat = dataRow["NOOFINPAT"].ToString();
                string consultapplysn = dataRow["CONSULTAPPLYSN"].ToString();
                string consulttype = dataRow["consulttypeid"].ToString();
                string printconsulttime = dataRow["printconsulttime"].ToString();
                if (!string.IsNullOrEmpty(printconsulttime))//已经打印
                {
                    if (MessageBox.Show("此会诊单已经打印，是否重新打印？", "提示", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }
                }
                DrectSoft.Core.Consultation.ConsultationEntity consultationEntity = GetConsultationEntity(consultapplysn, noofinapat, consulttype);
                DrectSoft.Core.Consultation.PrintForm PrintF = new DrectSoft.Core.Consultation.PrintForm(consultationEntity, true, m_App);
                PrintF.Show();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据会诊单号获得有关会诊的实体信息，用于打印会诊单的操作
        /// add by ywk 2012年7月4日 10:21:23
        /// </summary>
        /// <param name="consultapplysn"></param>
        /// <returns></returns>
        public DrectSoft.Core.Consultation.ConsultationEntity GetConsultationEntity(string consultapplysn, string noofinapat, string consulttype)
        {
            try
            {
                DrectSoft.Core.Consultation.ConsultationEntity consultationEntity = new DrectSoft.Core.Consultation.ConsultationEntity();
                DataTable dt = GetConsultationTable(consultapplysn);
                DataTable dtinp = GetRedactPatientInfoFrm("14", noofinapat);
                foreach (DataRow dr in dt.Rows)
                {
                    consultationEntity.ConsultApplySn = consultapplysn;
                    consultationEntity.NoOfInpat = noofinapat;
                    consultationEntity.HospitalName = m_App.CurrentHospitalInfo.Name;

                    if (dtinp.Rows.Count > 0)
                    {
                        consultationEntity.Name = dtinp.Rows[0]["NAME"].ToString().Trim();
                        consultationEntity.PatNoOfHIS = dtinp.Rows[0]["PatID"].ToString().Trim().Substring(4);//去除前4个0
                        consultationEntity.SexName = dtinp.Rows[0]["Gender"].ToString().Trim();
                        consultationEntity.Age = dtinp.Rows[0]["AgeStr"].ToString().Trim();
                        consultationEntity.Bed = dtinp.Rows[0]["OutBed"].ToString().Trim();
                        consultationEntity.DeptName = dtinp.Rows[0]["OutHosDeptName"].ToString().Trim();
                        consultationEntity.WardName = dtinp.Rows[0]["outhoswardname"].ToString().Trim();
                        consultationEntity.DeptID = dtinp.Rows[0]["OutHosDept"].ToString().Trim();
                        consultationEntity.WardID = dtinp.Rows[0]["outhosward"].ToString().Trim();
                    }
                    consultationEntity.UrgencyTypeID = dr["urgencytypeid"].ToString();
                    consultationEntity.UrgencyTypeName = dr["urgencytypeName"].ToString();
                    consultationEntity.ConsultTypeID = dr["consulttypeid"].ToString();
                    consultationEntity.ConsultTypeName = dr["consulttypeName"].ToString();
                    consultationEntity.Abstract = dr["abstract"].ToString();

                    consultationEntity.Purpose = dr["purpose"].ToString();
                    consultationEntity.ApplyDeptID = dr["ApplyDeptID"].ToString();
                    consultationEntity.ApplyDeptName = dr["ApplyDeptName"].ToString();
                    consultationEntity.ApplyUserID = dr["applyuserID"].ToString();
                    consultationEntity.ApplyUserName = dr["applyuserName"].ToString();

                    consultationEntity.ApplyTime = dr["applytime"].ToString();
                    consultationEntity.ConsultSuggestion = dr["consultsuggestion"].ToString();
                    consultationEntity.ConsultDeptID = dr["ConsultDeptID"].ToString();
                    consultationEntity.ConsultDeptName = dr["ConsultDeptName"].ToString();
                    consultationEntity.ConsultHospitalID = dr["hospitalcode"].ToString();

                    consultationEntity.ConsultHospitalName = dr["ConsultHospitalName"].ToString();
                    consultationEntity.ConsultDeptID2 = dr["ConsultDeptID2"].ToString();
                    consultationEntity.ConsultDeptName2 = dr["ConsultDeptName2"].ToString();
                    consultationEntity.ConsultUserID = dr["ConsultUserID"].ToString();
                    consultationEntity.ConsultUserName = dr["ConsultUserName"].ToString();

                    consultationEntity.ConsultTime = dr["ConsultTime"].ToString();
                    consultationEntity.StateID = dr["StateID"].ToString();
                }
                return consultationEntity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据申请单编号查询会诊信息
        /// </summary>
        /// <param name="consultationSn"></param>
        /// <returns></returns>
        public DataTable GetConsultationTable(string consultationSn)
        {
            try
            {
                SqlParameter[] sqlParam = new SqlParameter[] 
                { 
                    new SqlParameter("@ConsultApplySn", SqlDbType.Decimal),
                    new SqlParameter("@result",SqlDbType.Structured)
                };
                sqlParam[0].Value = consultationSn;
                sqlParam[1].Direction = ParameterDirection.Output;
                return DS_SqlHelper.ExecuteDataTable("EMR_CONSULTATION.usp_GetConsultationBySN", sqlParam, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 得到病人信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="noOfInpat"></param>
        /// <returns></returns>
        public DataTable GetRedactPatientInfoFrm(string type, string noOfInpat)
        {
            try
            {
                string categoryID = "";
                SqlParameter[] sqlParam = new SqlParameter[] 
                { 
                    new SqlParameter("@FrmType", SqlDbType.VarChar),
                    new SqlParameter("@NoOfInpat", SqlDbType.VarChar),
                    new SqlParameter("@CategoryID", SqlDbType.VarChar),
                    new SqlParameter("@result",SqlDbType.Structured)
                };
                sqlParam[0].Value = type;
                sqlParam[1].Value = noOfInpat;
                sqlParam[2].Value = categoryID;
                sqlParam[3].Direction = ParameterDirection.Output;
                return DS_SqlHelper.ExecuteDataTable("emrproc.usp_RedactPatientInfoFrm", sqlParam, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 会诊缴费

        /// <summary>
        /// 会诊缴费事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonConsultFee_Click(object sender, EventArgs e)
        {
            try
            {
                Fee(true, true);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 右键菜单缴费事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemFee_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                Fee(true, true);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 会诊收费
        /// </summary>
        private void Fee(bool isNeedTip/*是否需要提示*/, bool isNeedQuestion/*当满足条件时询问是否需要会诊缴费*/)
        {
            try
            {
                DataRow foucesRow = simpleButtonConsultFee.Tag as DataRow;
                if (foucesRow == null)
                {
                    return;
                }
                //会诊单号
                if (foucesRow.IsNull("ConsultApplySN"))
                {
                    return;
                }
                GoPay(foucesRow, isNeedTip, isNeedQuestion);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 会诊收费内部逻辑
        /// </summary>
        /// <param name="consultstatus"></param>
        /// <param name="consultapply"></param>
        /// <param name="applyDept"></param>
        /// <param name="foucesRow"></param>
        public bool GoPay(DataRow foucesRow, bool isNeedTip/*是否需要提示*/, bool isNeedQuestion/*当满足条件时询问是否需要会诊缴费*/)
        {
            try
            {
                string consultstatus = foucesRow["stateid"].ToString();//会诊单状态
                string consultapply = foucesRow["ConsultApplySN"].ToString();//会诊单号
                string applyDept = foucesRow["applydept"].ToString();//会诊申请部门

                string checkIsPaySql = "select * from consultapply where consultapplysn = '{0}' and valid = '1'";
                DataTable dtCheckIsPay = DS_SqlHelper.ExecuteDataTable(string.Format(checkIsPaySql, consultapply));

                #region 在收费前再确认一遍会诊费用是否已缴
                if (!CheckIsCanFee(consultapply, foucesRow, isNeedTip))
                {
                    return false;
                }
                #endregion

                DataTable dt = DS_SqlHelper.ExecuteDataTable(string.Format(@" select * from consultrecorddepartment where ConsultApplySN='{0}' and valid = '1' ", consultapply));

                #region 控制收费时机
                string feeConfig = m_IsFeeState;//会诊缴费时机的配置

                if (feeConfig == "-1")//不需要缴费
                {
                    return false;
                }
                else if (feeConfig == "0")//不控制
                { }
                else if (feeConfig == "1")//审核通过后才能收费
                {
                    if (Convert.ToInt32(consultstatus) <= (int)ConsultStatus.WaitApprove)
                    {
                        if (isNeedTip)
                        {
                            MessageBox.Show("审核通过后才能收费!");
                        }
                        return false;
                    }
                }
                else if (feeConfig == "2")//其中一个人签到就能收费
                {
                    int isSignInCount = dt.Select("issignin = '1'").Count();
                    if (isSignInCount == 0)
                    {
                        if (isNeedTip)
                        {
                            MessageBox.Show("至少需要一位医师签到才能收费！");
                        }
                        return false;
                    }
                }
                else if (feeConfig == "3")//受邀医师全部签到才能收费
                {
                    #region 医师全部签到后才能缴费
                    DataRow[] drsUnFee = dt.Select("issignin <> '1' or issignin is null");
                    int isNotSignInCount = drsUnFee.Count();
                    if (isNotSignInCount > 0)
                    {
                        string docName = string.Empty;
                        foreach (DataRow dr in drsUnFee)
                        {
                            docName += dr["employeecode"].ToString() + "_" + dr["employeename"].ToString() + " ";
                        }
                        docName = docName.Trim(new char[] { '_', ' ' });
                        if (docName.Length == 0)
                        {
                            if (isNeedTip)
                            {
                                MessageBox.Show("医师未签到，不能进行缴费!");
                            }
                        }
                        else
                        {
                            if (isNeedTip)
                            {
                                MessageBox.Show("医师: 【" + docName + "】 未签到，不能进行缴费!");
                            }
                        }
                        return false;
                    }
                    #endregion
                }
                else if (feeConfig == "4")//会诊记录保存后才能收费
                {
                    if (Convert.ToInt32(consultstatus) < (int)ConsultStatus.RecordeSave)
                    {
                        if (isNeedTip)
                        {
                            MessageBox.Show("未会诊，不能进行收费!");
                        }
                        return false;
                    }
                }
                else if (feeConfig == "5")//会诊完成后才能收费
                {
                    if (Convert.ToInt32(consultstatus) == (int)ConsultStatus.RecordeSave)
                    {
                        if (isNeedTip)
                        {
                            MessageBox.Show("会诊完成后才能收费!");
                        }
                        return false;
                    }
                }
                else
                {
                    throw new Exception("Appcfg表中的缴费时机ConsultFeeTime设置的有误，不能进行收费！");
                }
                #endregion

                if (isNeedQuestion && MessageBox.Show("是否需要缴费操作？", "提示", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes || !isNeedQuestion)
                {
                    string feeID = "1";//会诊缴费的ID固定是1
                    string resultMessage = string.Empty;

                    //检查收费接口是否开启使用
                    if (!CheckIsOpenFee(feeID, true, out resultMessage))
                    {
                        MessageBox.Show(resultMessage);
                        return false;
                    }

                    if (CallConsultInterface(consultapply, out resultMessage, feeID))
                    {
                        UpdateConsultIsPay(foucesRow);
                        foucesRow["ISPAY"] = "1";
                        foucesRow["MYPAY"] = "已缴费";
                        BtnConsultFeeEnable = false;
                        MessageBox.Show("会诊缴费成功！");
                        return true;
                    }
                    else
                    {
                        MessageBox.Show(resultMessage);
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新会诊是否缴费的状态
        /// </summary>
        /// <param name="foucesRow"></param>
        private void UpdateConsultIsPay(DataRow foucesRow)
        {
            try
            {
                string consultApplySn = foucesRow["consultapplysn"].ToString();
                string sql = " update consultapply set ispay = '1' where consultapplysn = '{0}' and valid = '1' ";
                DS_SqlHelper.ExecuteNonQuery(string.Format(sql, consultApplySn), CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 调用费用接口程序
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <param name="returnResult"></param>
        /// <returns></returns>
        private bool CallConsultInterface(string consultApplySN, out string returnResult, string id)
        {
            try
            {
                returnResult = string.Empty;

                string feeInterfaceMainSql = string.Format(@" select * from consultfeeinterfacemain where id = '" + id + "' and valid = '1' ");
                DataTable dtMain = DS_SqlHelper.ExecuteDataTable(feeInterfaceMainSql);

                string feeInterfaceParaSql = string.Format(@" select * from consultfeeinterfacepara where mainid = '" + id + "' and valid = '1' ");
                DataTable dtPara = DS_SqlHelper.ExecuteDataTable(feeInterfaceParaSql);

                if (dtMain.Rows.Count > 0 && dtPara.Rows.Count > 0)
                {
                    string dataSourceSql = dtMain.Rows[0]["DATASOURCESQL"].ToString();
                    DataTable dataSource = DS_SqlHelper.ExecuteDataTable(string.Format(dataSourceSql, m_App.User.Id, consultApplySN), CommandType.Text);
                    if (dataSource.Rows.Count > 0)
                    {
                        try
                        {
                            CallConsultInterfaceInner(dtMain, dtPara, dataSource);
                        }
                        catch (Exception ex)
                        {
                            returnResult = ex.Message;
                            return false;
                        }
                    }
                    else
                    {
                        //MessageBox.Show("操作失败，会诊记录单号：" + consultApplySN + " 不存在！");
                        returnResult = "操作失败，会诊记录单号：" + consultApplySN + " 不存在！";
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CallConsultInterfaceInner(DataTable dtMain, DataTable dtPara, DataTable dataSource)
        {
            try
            {
                string procedureName = dtMain.Rows[0]["PROCEDURENAME"].ToString();
                string message = string.Empty;

                DS_SqlHelper.CreateSqlHelperByDBName("HISDB");

                CheckConnectHIS();

                DS_SqlHelper.BeginTransaction();
                foreach (DataRow drSource in dataSource.Rows)
                {
                    SqlParameter[] sqlParam = new SqlParameter[dtPara.Rows.Count];//入参的数目
                    message = "\n\r" + procedureName;//消息记录

                    for (int i = 0; i < dtPara.Rows.Count; i++)
                    {
                        DataRow drPara = dtPara.Rows[i];

                        string name = drPara["PARAMETERNAME"].ToString();
                        string paraType = drPara["PARAMETERTYPE"].ToString();
                        SqlDbType type = GetDataType(paraType);
                        string paraDir = drPara["PARAMETERDIRECTION"].ToString();
                        ParameterDirection direction = GetDirection(paraDir);

                        if (direction == ParameterDirection.Output && type == SqlDbType.VarChar)
                        {
                            sqlParam[i] = new SqlParameter(name, type, 100);
                        }
                        else
                        {
                            sqlParam[i] = new SqlParameter(name, type);
                        }
                        sqlParam[i].Direction = direction;

                        string value = string.Empty;
                        sqlParam[i].Value = TypeConvert(value, paraType);
                        if (direction == ParameterDirection.Input || direction == ParameterDirection.InputOutput)//给入参设置具体值
                        {
                            value = drSource[drPara["PARAMETERFIELD"].ToString()].ToString();
                            sqlParam[i].Value = TypeConvert(value, paraType);
                        }

                        message += "\n\r" + name + ":" + value + " " + paraType + " " + paraDir;
                    }
                    DS_SqlHelper.ExecuteNonQueryInTran(procedureName, sqlParam, CommandType.StoredProcedure);

                    //返回的参数值
                    var spReturnCode = DS_SqlHelper.CmdParameterList.Where((sp) =>
                    {
                        if (sp.ParameterName.ToUpper() == "RETURN_CODE")
                            return true;
                        else
                            return false;
                    });
                    if (spReturnCode.Count() > 0)
                    {
                        string returnCode = spReturnCode.ToArray()[0].Value.ToString();
                        if (returnCode == "1")
                        {
                            continue;
                        }
                        else
                        {
                            var spReturnResult = DS_SqlHelper.CmdParameterList.Where((sp) =>
                            {
                                if (sp.ParameterName.ToUpper() == "RETURN_RESULT")
                                    return true;
                                else
                                    return false;
                            });

                            if (spReturnResult.Count() > 0)
                            {
                                string returnResult = spReturnResult.ToArray()[0].Value.ToString();
                                throw new Exception(returnResult + message);
                            }
                            else
                            {
                                throw new Exception("调用HIS系统的会诊缴费接口时出错！" + message);
                            }
                        }
                    }
                }
                DS_SqlHelper.CommitTransaction();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DS_SqlHelper.CreateSqlHelper();
            }
        }

        /// <summary>
        /// 验证是否可以连接到HIS
        /// </summary>
        private void CheckConnectHIS()
        {
            try
            {
                DS_SqlHelper.ExecuteDataTable("select 1 from dual", CommandType.Text);
            }
            catch (Exception e)
            {
                throw new Exception("无法连接到HIS数据库！");
            }
        }

        /// <summary>
        /// 类型转换 string -> SqlDbType
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private SqlDbType GetDataType(string type)
        {
            try
            {
                switch (type)
                {
                    case "VARCHAR": return SqlDbType.VarChar;
                    case "DATETIME": return SqlDbType.DateTime;
                    case "NUMBER": return SqlDbType.Decimal;
                    case "INT": return SqlDbType.Int;
                }
                return SqlDbType.VarChar;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 类型转换 string -> ParameterDirection
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        private ParameterDirection GetDirection(string direction)
        {
            try
            {
                switch (direction)
                {
                    case "IN": return ParameterDirection.Input;
                    case "OUT": return ParameterDirection.Output;
                    case "INOUT": return ParameterDirection.InputOutput;
                    case "RETURNVALUE": return ParameterDirection.ReturnValue;
                }
                return ParameterDirection.Input;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private object TypeConvert(string value, string type)
        {
            try
            {
                switch (type)
                {
                    case "VARCHAR": return value;
                    case "DATETIME": return value == "" ? System.DateTime.MinValue : Convert.ToDateTime(value);
                    case "NUMBER": return value == "" ? 0 : Convert.ToDecimal(value);
                    case "INT": return value == "" ? 0 : Convert.ToInt32(value);
                }
                return SqlDbType.VarChar;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 验证指定ID的费用接口是否开启使用
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isShowMessage"></param>
        /// <param name="returnResult"></param>
        /// <returns></returns>
        private bool CheckIsOpenFee(string id, bool isShowMessage, out string returnResult)
        {
            try
            {
                string feeInterfaceMainSql = string.Format(@" select * from consultfeeinterfacemain where id = '" + id + "' and valid = '1' ");
                DataTable dtMain = DS_SqlHelper.ExecuteDataTable(feeInterfaceMainSql);
                returnResult = string.Empty;

                if (dtMain.Rows.Count > 0 && dtMain.Rows[0]["ISOPEN"].ToString() == "0")
                {
                    //MessageBox.Show("会诊缴费接口未开启使用！");
                    returnResult = "会诊缴费接口未开启使用！";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 验证是否可以缴费
        /// </summary>
        /// <param name="consultApplySn"></param>
        /// <returns></returns>
        private bool CheckIsCanFee(string consultApplySn, DataRow dr, bool isNeedTip)
        {
            try
            {
                string sqlGetIsPay = string.Format(@"select c.ispay from consultapply c where c.consultapplysn = '{0}' and c.valid = '1'", consultApplySn);
                DataTable dt = DS_SqlHelper.ExecuteDataTable(sqlGetIsPay);
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("此会诊记录已经作废！");
                    dr.Delete();
                    return false;
                }
                else
                {
                    if (dt.Rows[0]["ISPAY"].ToString() == "1")
                    {
                        if (isNeedTip)
                        {
                            MessageBox.Show("此会诊记录已经缴费不能重复缴费！");
                        }
                        dr["ISPAY"] = "1";
                        dr["MYPAY"] = "已缴费";
                        BtnConsultFeeEnable = false;
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 医师签到
        private void simpleButtonSign_Click(object sender, EventArgs e)
        {
            try
            {
                SignIn();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void barButtonItemSign_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                SignIn();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 医师签到
        /// </summary>
        private void SignIn()
        {
            try
            {
                DataRow dataRow = gridViewOtherDept.FocusedRowHandle < 0 ? gridViewMyDept.GetDataRow(gridViewMyDept.FocusedRowHandle) : gridViewOtherDept.GetDataRow(gridViewOtherDept.FocusedRowHandle);

                if (dataRow == null)
                {
                    return;
                }
                if (dataRow["ISPAY"].ToString() == "1")
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该会诊单已缴费，无法再次签到");
                    return;
                }
                string noofinapat = dataRow["NOOFINPAT"].ToString();
                string consultapplysn = dataRow["CONSULTAPPLYSN"].ToString();
                DocSignInForm docSignInfo = new DocSignInForm(m_App, noofinapat, consultapplysn);
                docSignInfo.PayHandle += Fee; //TODO wwj
                docSignInfo.StartPosition = FormStartPosition.CenterParent;
                docSignInfo.ShowDialog();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region 抓取会诊配置信息

        /// <summary>
        /// 通过配置名称抓取配置指
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        private string GetAppcfgConsult(string configName)
        {
            try
            {
                string sql = " select value from appcfg a where a.configkey = '{0}' and a.valid = '1' ";
                return DS_SqlHelper.ExecuteScalar(string.Format(sql, configName), CommandType.Text).ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " 没有设置Appcfg表中的" + configName);
            }
        }

        #endregion

        #region 列表中FocusedRowChanged
        private void gridViewMyDept_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                GridViewFocusedRowChanged(gridViewMyDept);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void gridViewOtherDept_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                m_GridViewMain.FocusedRowHandle = -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GridViewFocusedRowChanged(GridView gv)
        {
            try
            {
                if (gv.FocusedRowHandle < 0)
                {
                    SetButtonEnable(false);
                    return;
                }

                if (gv == gridViewMyDept)
                {
                    gridViewOtherDept.FocusedRowHandle = -1;
                }
                else if (gv == gridViewOtherDept)
                {
                    gridViewMyDept.FocusedRowHandle = -1;
                }

                Point position = Cursor.Position;
                DataRow focusRow = gv.GetDataRow(gv.FocusedRowHandle);
                if (focusRow == null)
                {
                    SetButtonEnable(false);
                    gv.FocusedRowHandle = -1;
                }
                else
                {
                    //控制按钮的样式
                    ControlButtonStyle(focusRow);
                }
                ChangeConsultRecord(focusRow);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 切换会诊记录，选中护士工作站中左侧的对应病人
        /// </summary>
        private void ChangeConsultRecord(DataRow dr)
        {
            try
            {
                if (dr != null)
                {
                    string noofinpat = dr["noofinpat"].ToString();
                    DataView dataSource = m_GridViewMain.DataSource as DataView;
                    if (dataSource != null)
                    {
                        int i = 0;
                        foreach (DataRowView myrow in dataSource)//循环遍历Datatable
                        {
                            if (myrow["NOOFINPAT"].ToString() == noofinpat)
                            {
                                m_GridViewMain.FocusedRowHandle = i;
                                m_GridViewMain.RefreshData();
                                break;
                            }
                            i = i + 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// 会诊费用接口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonFeeInterface_Click(object sender, EventArgs e)
        {
            try
            {
                FrmFeeInterfaceConfig config = new FrmFeeInterfaceConfig(m_App);
                config.ShowDialog();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 设置会诊费用接口是否可见
        /// </summary>
        private void SetBtnFeeInterfaceVisible()
        {
            string showFeeInterfaceUser = GetAppcfgConsult("ConsultationFeeInterfaceUser");
            string currentUserID = m_App.User.Id;
            simpleButtonFeeInterface.Visible = false;
            if (showFeeInterfaceUser.Split(',').Contains(currentUserID))
            {
                simpleButtonFeeInterface.Visible = true;
            }
        }

        /// <summary>
        /// 右键打开会诊信息界面
        /// Add by xlb 2013-03-21
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonShowConsult_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                DataRow dataRow = simpleButtonRemind.Tag as DataRow;
                if (dataRow == null)
                {
                    return;
                }
                string noofinpat = dataRow["NOOFINPAT"].ToString();
                string consultapplysn = dataRow["CONSULTAPPLYSN"].ToString();
                ProcessClickConsultatonListLogic process = new ProcessClickConsultatonListLogic(m_App, noofinpat);
                if (process == null)
                {
                    return;
                }
                process.ProcessLogic(m_App.User.Id, consultapplysn);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
    }
}
