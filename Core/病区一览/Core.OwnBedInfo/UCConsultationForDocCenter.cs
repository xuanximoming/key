using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraNavBar;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Eop;
using DrectSoft.Core.Consultation.NEW;
using DrectSoft.Core.Consultation.NEW.Enum;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.DSSqlHelper;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace DrectSoft.Core.OwnBedInfo
{
    /// <summary>
    /// 注：医生工作站会诊信息界面
    /// </summary>
    public partial class UCConsultationForDocCenter : DevExpress.XtraEditors.XtraUserControl
    {
        private IEmrHost m_App;
        private NavBarGroup m_NavBarGroupConsultation;
        private string m_RowDisplayColorConfig;

        #region 相关方法

        public UCConsultationForDocCenter()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="host"></param>
        /// <param name="navBarGroup"></param>
        public UCConsultationForDocCenter(IEmrHost host, NavBarGroup navBarGroup)
            : this()
        {
            try
            {
                m_App = host;
                m_NavBarGroupConsultation = navBarGroup;
                m_RowDisplayColorConfig = GetConfigValueByKey("ConsultDisplayConfig");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 判断此会诊单是否已经有医师签到 true：有医师签到 false：没有医师签到
        /// </summary>
        /// <param name="consultationApplySN"></param>
        /// <returns></returns>
        private bool CheckIsSign(string consultationApplySN)
        {
            try
            {
                string sqlGetSignCount = "select count(1) from consultrecorddepartment where issignin = '1' and consultapplysn = '{0}' and valid = '1'";
                int count = Convert.ToInt32(DS_SqlHelper.ExecuteScalar(string.Format(sqlGetSignCount, consultationApplySN), CommandType.Text));
                if (count > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void BindConsultationThread()
        {
            try
            {
                Thread consultationThread = new Thread(new ThreadStart(BindConsultion));
                consultationThread.Start();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 抓取会诊信息
        /// </summary>
        private void BindConsultion()
        {
            try
            {
                //xll 20130301  是否在timer中抓取会诊信息
                string config = GetConfigValueByKey("IsOpenConsultion");
                if (config == "0")//0关闭会诊 不抓取数据
                {
                    return;
                }

                DataTable dtConsultation = GetConsultionData();
                BindConsultationInner(dtConsultation);
            }
            catch (Exception ex)
            {
                //throw ex;//不进行抛出 add by ywk 2013年3月13日8:46:15 
            }
        }

        private void BindConsultationInner(DataTable dtConsultation)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<DataTable>(BindConsultationInner), dtConsultation);
            }
            else
            {
                RefreshConsultation(dtConsultation);
                InitConsultConfirm();
                ConsultationDefaultFocusRow();
                gridViewConsultation.ExpandAllGroups();
            }
        }

        /// <summary>
        /// 得到配置信息  wyt 2012年8月27日
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetConfigValueByKey(string key)
        {
            try
            {
                string sql1 = " select * from appcfg where configkey = '" + key + "'  ";
                DataTable dt = m_App.SqlHelper.ExecuteDataTable(sql1, CommandType.Text);
                string config = string.Empty;
                if (dt != null && dt.Rows.Count > 0)
                {
                    config = dt.Rows[0]["value"].ToString();
                }
                return config;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取所需状态的会诊信息
        /// </summary>
        private DataTable GetConsultionData()
        {
            try
            {
                Employee emp = new Employee(m_App.User.Id);
                emp.ReInitializeProperties();

                if (emp.Grade.Trim() == "")
                {
                    return null;
                }

                DataTable dt = new DataTable();
                SqlParameter[] sqlParams = new SqlParameter[] { 
                new SqlParameter("@Deptids", SqlDbType.VarChar, 255),
                 new SqlParameter("@userid", SqlDbType.VarChar, 255),
                 new SqlParameter("@levelid", SqlDbType.VarChar, 255)
                };
                sqlParams[0].Value = m_App.User.CurrentDeptId.Trim(); // 科室代码
                sqlParams[1].Value = m_App.User.Id.Trim(); // 当前登录人编码
                sqlParams[2].Value = emp.Grade; // 当前登录人级别
                dt = m_App.SqlHelper.ExecuteDataTable("EMR_CONSULTATION.usp_GetConsultionDoctor", sqlParams, CommandType.StoredProcedure);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 刷新会诊的数据
        /// Modify by xlb 2013-03-19
        /// </summary>
        /// <param name="dtNew"></param>
        private void RefreshConsultation(DataTable dtNew)
        {
            try
            {
                if (dtNew != null)
                {
                    if (!CommonObjects.IsNeedVerifyInConsultation)//过滤需要审核的会诊
                    {

                        dtNew.DefaultView.RowFilter = "stateid <> '" + Convert.ToString((int)ConsultStatus.WaitApprove) + "'";
                    }

                    this.gridControlConsultation.DataSource = dtNew;
                    //用于解决排序问题
                    dtNew.DefaultView.Sort = "stateid asc,consulttime asc";
                    gridViewConsultation.OptionsCustomization.AllowSort = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化会诊签核列表
        /// </summary>
        private void InitConsultConfirm()
        {
            try
            {
                int num1 = 0;
                string info = string.Empty;
                DataTable dt = this.gridControlConsultation.DataSource as DataTable;

                if (dt != null)
                {
                    if (CommonObjects.IsNeedVerifyInConsultation)
                    {
                        //二次修改 ywk 2013年3月29日21:50:05 
                        num1 = dt.Select(" stateid = '6720' ").Length;
                        info += "待审核：" + num1 + " ";
                    }

                    int num2 = dt.Select(" stateid = '6730' ").Length;
                    if (info.Length > 0)
                    {
                        info += "  待会诊：" + num2;
                    }
                    else
                    {
                        info += "待会诊：" + num2;
                    }
                    m_NavBarGroupConsultation.Caption = "会诊信息【" + info + "】";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 控制焦点默认方法
        /// </summary>
        private void ConsultationDefaultFocusRow()
        {
            try
            {
                DataTable dt = gridControlConsultation.DataSource as DataTable;
                if (dt != null && dt.Rows.Count > 0 && gridViewConsultation.FocusedRowHandle < 0)
                {
                    gridViewConsultation.FocusedRowHandle = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 打开会诊信息界面方法
        /// </summary>
        /// <param name="rowHandle"></param>
        private void OpenConsultationInfo(int rowHandle)
        {
            try
            {
                if (rowHandle >= 0)
                {
                    DataRow dr = gridViewConsultation.GetDataRow(rowHandle);
                    if (dr == null)
                    {
                        return;
                    }
                    string noOfFirstPage = dr["NoOfInpat"].ToString();
                    string consultTypeID = dr["ConsultTypeID"].ToString();
                    string consultApplySn = dr["ConsultApplySn"].ToString();
                    ProcessClickConsultatonListLogic processConsult = new ProcessClickConsultatonListLogic(m_App, noOfFirstPage);
                    processConsult.ProcessLogic(m_App.User.Id, consultApplySn);
                    BindConsultion();//刷新会诊列表中的数据
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 相关事件
        /// <summary>
        /// 刷新事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonConsultationRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                //绑定会诊信息
                BindConsultion();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 说明事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonState_Click(object sender, EventArgs e)
        {
            try
            {
                ConsultationStateForm stateForm = new ConsultationStateForm();
                stateForm.StartPosition = FormStartPosition.CenterScreen;
                stateForm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 列表双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewConsultation_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hitInfo =
                           gridViewConsultation.CalcHitInfo(gridControlConsultation.PointToClient(Cursor.Position));
                if (hitInfo.RowHandle >= 0)
                {
                    OpenConsultationInfo(hitInfo.RowHandle);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 右键查看会诊信息事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemOpen_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                OpenConsultationInfo(gridViewConsultation.FocusedRowHandle);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 控制右侧的会诊信息的列表，（控制状态显示颜色）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewConsultation_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                if (e.CellValue.ToString() == "紧急")
                {
                    e.Graphics.FillRectangle(Brushes.Red, e.Bounds);
                    e.Appearance.ForeColor = Color.White;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 鼠标事件控制右键菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewConsultation_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    GridHitInfo hitInfo = gridViewConsultation.CalcHitInfo(this.gridControlConsultation.PointToClient(Cursor.Position));
                    if (hitInfo.RowHandle >= 0)
                    {
                        barButtonItemCallBack.Visibility = BarItemVisibility.Never;//撤销会诊右键
                        barButtonItemDeleteApplySave.Visibility = BarItemVisibility.Never;//删除会诊申请草稿
                        DataRowView drv = gridViewConsultation.GetRow(hitInfo.RowHandle) as DataRowView;
                        if (drv["stateid"].ToString() == Convert.ToString((int)ConsultStatus.WaitApprove)
                            || drv["stateid"].ToString() == Convert.ToString((int)ConsultStatus.WaitConsultation))
                        {
                            if (drv["APPLYUSER"].ToString().Equals(m_App.User.Id))//当前登录人是申请医师可以撤销
                            {
                                barButtonItemCallBack.Visibility = BarItemVisibility.Always;//待审核的记录可以撤销  待会诊且未审核过的可以撤销
                            }
                            else
                            {
                                barButtonItemCallBack.Visibility = BarItemVisibility.Never;
                            }
                        }
                        else if (drv["stateid"].ToString() == Convert.ToString((int)ConsultStatus.ApplySave)
                            || drv["stateid"].ToString() == Convert.ToString((int)ConsultStatus.Reject)
                            || drv["stateid"].ToString() == Convert.ToString((int)ConsultStatus.CallBackConsultation))
                        {
                            barButtonItemDeleteApplySave.Visibility = BarItemVisibility.Always;//只能删除会诊申请草稿、会诊否决、会诊撤销
                        }
                        popupMenuConsultation.ShowPopup(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 右键删除事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemDeleteApplySave_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (MessageBox.Show("是否删除会诊记录？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DataRowView drv = gridViewConsultation.GetRow(gridViewConsultation.FocusedRowHandle) as DataRowView;
                    if (drv != null)
                    {
                        string consultationApplySn = drv["consultapplysn"].ToString();
                        ConsultStatus state = GetConsultationState(consultationApplySn);
                        //只能删除会诊申请草稿、会诊否决、会诊撤销
                        if (state == ConsultStatus.ApplySave || state == ConsultStatus.Reject || state == ConsultStatus.CallBackConsultation)
                        {
                            CancelConsultationRecord(consultationApplySn);
                            drv.Delete();
                            MyMessageBox.Show("删除成功！", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.InformationIcon);
                        }
                        else
                        {
                            MyMessageBox.Show("此记录的状态已变更，不能进行删除操作！", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 右键撤销事件
        /// Modify by xlb 2013-03-27
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemCallBack_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (MessageBox.Show("是否撤销此次会诊申请？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DataRowView drv = gridViewConsultation.GetRow(gridViewConsultation.FocusedRowHandle) as DataRowView;
                    if (drv != null)
                    {
                        string consultationApplySn = drv["consultapplysn"].ToString();
                        ConsultStatus state = GetConsultationState(consultationApplySn);
                        if (state == ConsultStatus.WaitApprove)//待审核的记录可以撤销
                        {
                            UpdateConsultationState(Convert.ToString((int)ConsultStatus.CallBackConsultation), consultationApplySn);
                            //drv["stateid"] = (int)ConsultStatus.CallBackConsultation;
                            //drv["consultstatus"] = "已撤销";
                            //gridViewConsultation.RefreshData();
                            MyMessageBox.Show("撤销成功", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.InformationIcon);
                        }
                        else if (state == ConsultStatus.WaitConsultation)//待会诊且未审核过的可以撤销
                        {
                            //检查是否有已经签到的医师，否则不能撤销
                            if (CheckIsSign(consultationApplySn))
                            {
                                MyMessageBox.Show("此记录已经有医师签到，不能进行撤销操作", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                                return;
                            }
                            else if (drv["ispassed"].ToString() == "1")
                            {
                                MyMessageBox.Show("此记录已经审核过，不能进行撤销操作", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                                return;
                            }
                            else
                            {
                                UpdateConsultationState(Convert.ToString((int)ConsultStatus.CallBackConsultation), consultationApplySn);
                                //drv["stateid"] = (int)ConsultStatus.CallBackConsultation;
                                //drv["consultstatus"] = "已撤销";
                                //gridViewConsultation.RefreshData();
                                MyMessageBox.Show("撤销成功", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.InformationIcon);
                            }
                        }
                        else
                        {
                            MyMessageBox.Show("此记录的状态已变更，不能进行撤销操作", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                        }
                        BindConsultion();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 获取会诊状态
        /// </summary>
        /// <param name="consultationApplySN"></param>
        /// <returns></returns>
        private ConsultStatus GetConsultationState(string consultationApplySN)
        {
            try
            {
                string sqlGetState = "select stateid from consultapply where consultapplysn = '{0}' and valid = '1'";
                DataTable dt = DS_SqlHelper.ExecuteDataTable(string.Format(sqlGetState, consultationApplySN), CommandType.Text);
                if (dt.Rows.Count == 0)
                {
                    throw new Exception("此会诊记录已经作废！");
                }
                string state = dt.Rows[0]["stateid"].ToString();
                return (ConsultStatus)Enum.Parse(typeof(ConsultStatus), state);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 作废会诊记录
        /// </summary>
        /// <param name="consultationApplySN"></param>
        private void CancelConsultationRecord(string consultationApplySN)
        {
            try
            {
                string sqlCancelConsultationRecord = "update consultapply set valid = '0' where consultapplysn = '{0}' and valid = '1'";
                DS_SqlHelper.ExecuteNonQuery(string.Format(sqlCancelConsultationRecord, consultationApplySN), CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新会诊单状态
        /// </summary>
        /// <param name="stateid"></param>
        /// <param name="consultationApplySN"></param>
        private void UpdateConsultationState(string stateid, string consultationApplySN)
        {
            try
            {
                string sqlUpdateConsultationState = "update consultapply set stateid = '{0}' where consultapplysn = '{1}' and valid = '1'";
                DS_SqlHelper.ExecuteNonQuery(string.Format(sqlUpdateConsultationState, stateid, consultationApplySN), CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCConsultationForDocCenter_Load(object sender, EventArgs e)
        {
            try
            {
                this.Dock = DockStyle.Fill;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        ///根据会诊状态绘制背景颜色
        ///Add by xlb 2013-03-18
        ///Modify by xlb 2013-03-26
        ///避免背景配置项格式更改存在空格引起的报错
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewConsultation_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            try
            {
                //抓取配置表中控制会诊提醒信息背景设置配置
                string xmlValue = m_RowDisplayColorConfig;
                if (string.IsNullOrEmpty(xmlValue))
                {
                    return;
                }

                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(xmlValue);
                XmlNodeList nodeList = xdoc.SelectNodes("/consult/value");

                DataRow dataRow = gridViewConsultation.GetDataRow(e.RowHandle);
                if (dataRow == null)
                {
                    return;
                }
                int red = 0;//红色颜色值
                int green = 0;//绿色颜色值
                int blue = 0;//蓝色颜色值
                string colorArgb = string.Empty;
                foreach (XmlNode node in nodeList)
                {
                    if (node.Attributes["Id"].Value.Equals(dataRow["stateid"].ToString()))
                    {
                        //配置项节点值
                        colorArgb = node.InnerText.Trim().Substring(1, node.InnerText.Trim().Length - 2);
                        red = int.Parse(colorArgb.Split(',')[0]);
                        green = int.Parse(colorArgb.Split(',')[1]);
                        blue = int.Parse(colorArgb.Split(',')[2]);
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }

                Color clor = Color.FromArgb(red, green, blue);
                e.Appearance.BackColor = clor;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #endregion
    }
}
