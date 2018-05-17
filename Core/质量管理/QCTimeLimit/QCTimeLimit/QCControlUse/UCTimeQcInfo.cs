using System;
using System.Data;
using System.Drawing;
using System.Threading;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Service;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.DSSqlHelper;

namespace DrectSoft.Emr.QCTimeLimit.QCControlUse
{
    /// <summary>
    /// 病历时限提醒界面
    /// xlb 2013-01-10
    /// </summary>
    public partial class UCTimeQcInfo : DevExpress.XtraEditors.XtraUserControl
    {
        public IEmrHost App { get; set; }

        public UCTimeQcInfo()
        {
            try
            {
                InitializeComponent();
                Register();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 事件 xlb 2013-01-10

        /// <summary>
        /// 窗体加载事件
        /// xlb 2013-01-11
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCTimeQcInfo_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.DesignMode)
                {
                    DS_SqlHelper.CreateSqlHelper();
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 绘制单元格样式事件 警告信息和提示信息标色
        /// xlb 2013-01-10
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewTimeLimit_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            try
            {
                if (e.RowHandle >= 0 && e.Column.FieldName == "message")
                {
                    string message = gridViewTimeLimit.GetRowCellValue(e.RowHandle, gridViewTimeLimit.Columns["message"]).ToString();
                    if (message.StartsWith("警告"))
                    {
                        e.Appearance.ForeColor = Color.Red;
                    }
                    else
                    {
                        e.Appearance.ForeColor = Color.Teal;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 展开事件
        /// xlb 2013-01-10
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnZhanKai_Click(object sender, EventArgs e)
        {
            try
            {
                gridViewTimeLimit.ExpandAllGroups();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 收缩事件
        /// xlb 2013-01-10
        /// </summary>
        /// <param name="seder"></param>
        /// <param name="e"></param>
        private void btnShouSuo_Click(object seder, EventArgs e)
        {
            try
            {
                gridViewTimeLimit.CollapseAllGroups();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 刷新事件
        /// xlb 2013-01-10
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                CheckDoctorLimitThread(App.User.DoctorId);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 双击事件
        /// xlb 2013-01-11
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewTimeLimit_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (gridViewTimeLimit.FocusedRowHandle < 0)
                {
                    return;
                }
                if (App == null)
                {
                    return;
                }
                DataRow dr = gridViewTimeLimit.GetDataRow(gridViewTimeLimit.FocusedRowHandle);
                if (dr == null)
                {
                    return;
                }
                decimal patid = Convert.ToDecimal(dr["noofinpat"]);
                if (patid < 0)
                {
                    return;
                }
                App.ChoosePatient(patid);
                App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #endregion


        #region 方法 xlb 2013-01-11

        /// <summary>
        /// 注册事件
        /// xlb 2013-01-11
        /// </summary>
        private void Register()
        {
            try
            {
                btnZhanKai.Click += new EventHandler(btnZhanKai_Click);
                btnShouSuo.Click += new EventHandler(btnShouSuo_Click);
                btnRefresh.Click += new EventHandler(btnRefresh_Click);
                gridViewTimeLimit.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(gridViewTimeLimit_RowCellStyle);
                gridViewTimeLimit.DoubleClick += new EventHandler(gridViewTimeLimit_DoubleClick);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 抓取病历质控记录库信息
        /// xlb 2013-01-10
        /// </summary>
        /// <returns></returns>
        private void GetQcRecord(DataTable dtTable)
        {
            try
            {
                if (dtTable.Rows.Count == 0) return;

                DataTable dtShow = new DataTable();
                AddDataElement(dtShow, "message", "提醒信息", typeof(string));
                AddDataElement(dtShow, "time", "提醒时间", typeof(string));
                AddDataElement(dtShow, "name", "病人姓名", typeof(string));
                AddDataElement(dtShow, "noofinpat", "病案号", typeof(string));

                //Add By wwj 2013-01-15 用于解决排序问题
                AddDataElement(dtShow, "realconditiontime", "条件发生时间", typeof(string));

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    DataRow row = dtTable.Rows[i];
                    string foulState = row["FOULSTATE"].ToString();
                    DateTime realCtime = DateTime.Parse(row["REALCONDITIONTIME"].ToString());
                    DateTime dateNow = DateTime.Now;
                    double timeLimit = double.Parse(row["TIMELIMIT"].ToString());
                    TimeSpan tsLimit = (TimeSpan)(dateNow - realCtime);
                    TimeSpan lTimeLimit = new TimeSpan(0, 0, 0, Convert.ToInt32(timeLimit));

                    DataRow drshow = dtShow.NewRow();
                    drshow["name"] = row["NAME"].ToString();
                    drshow["noofinpat"] = row["NOOFINPAT"].ToString();

                    //Add By wwj 2013-01-15 用于解决排序问题
                    drshow["realconditiontime"] = Convert.ToDateTime(row["realconditiontime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");

                    if (foulState.Trim() == "1")
                    {
                        drshow["message"] = row["FOULMESSAGE"].ToString();
                        drshow["time"] = "超出" + DS_Common.TimeSpanToLocal(tsLimit - lTimeLimit);
                        dtShow.Rows.Add(drshow);
                    }
                    else if (foulState.Trim() == "0")
                    {
                        if (tsLimit > lTimeLimit)
                        {
                            drshow["message"] = row["FOULMESSAGE"].ToString();
                            drshow["time"] = "超出" + DS_Common.TimeSpanToLocal(tsLimit - lTimeLimit);
                            dtShow.Rows.Add(drshow);
                        }
                        else
                        {
                            drshow["message"] = row["REMINDER"].ToString();
                            drshow["time"] = "还剩" + DS_Common.TimeSpanToLocal(lTimeLimit - tsLimit);
                            dtShow.Rows.Add(drshow);
                        }
                    }
                }
                //Modify By wwj 2013-01-15 用于解决排序问题
                dtShow.DefaultView.Sort = "message asc, realconditiontime asc";
                gridControlTimeLimitInfo.DataSource = dtShow;
                gridViewTimeLimit.Columns["name"].GroupIndex = 1;
                gridViewTimeLimit.Columns["noofinpat"].Visible = false;
                //gridViewTimeLimit.Columns["realconditiontime"].Visible = false;
                //遍历让列标题居中
                for (int i = 0; i < gridViewTimeLimit.Columns.Count; i++)
                {
                    gridViewTimeLimit.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                }
                gridViewTimeLimit.OptionsCustomization.AllowSort = false;//禁掉自带排序功能
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CheckDoctorLimitThread(object doctorId)
        {
            Thread qcTimeThread = new Thread(new ParameterizedThreadStart(CheckDoctorLimit));
            if (App != null)
            {
                qcTimeThread.Start(App.User.DoctorId);
            }

        }

        /// <summary>
        /// 查看医生的时限信息
        /// xlb 2013-01-11
        /// </summary>
        /// <param name="doctorId"></param>
        public void CheckDoctorLimit(object doctorId)
        {
            try
            {
                string sqlDoctor =
                    @"select q.noofinpat, q.foulstate, q.reminder, q.timelimit, q.foulmessage, q.realconditiontime, i.name 
                    from qcrecord q 
                    join inpatient i 
                    on i.noofinpat = q.noofinpat 
                    where q.noofinpat in 
                        (select noofinpat 
                            from inpatient 
                            where ((to_date(outhosdate, 'yyyy-mm-dd hh24:mi:ss') + 3 >= sysdate and inpatient.status in ('1502', '1503'))  
                                or 
                                (inpatient.status not in ('1502', '1503'))) 
                            and inpatient.resident = {0} 
                        union all 
                        select a.noofinpat 
                            from doctor_assignpatient a left join inpatient b on a.noofinpat=b.noofinpat
                            where a.id = {0} 
                            and a.valid = '1' 
                             and (((to_date(b.outhosdate, 'yyyy-mm-dd hh24:mi:ss') + 3 >= sysdate and b.status in ('1502', '1503'))  
                                or 
                                (b.status not in ('1502', '1503'))) )
                            and a.noofinpat = i.noofinpat) 
                    and q.result = '0' 
                    and valid = '1' 
                    and i.isbaby!=1
                    order by q.noofinpat;";
                //SqlParameter[] sps = { new SqlParameter("@doctorId", doctorId) };
                //DataTable dtDoctorLimit = YD_SqlHelper.ExecuteDataTable(sqlDoctor, sps, CommandType.Text);
                if (App != null)
                {
                    DataTable dtDoctorLimit = App.SqlHelper.ExecuteDataTable(string.Format(sqlDoctor, doctorId.ToString()), CommandType.Text);
                    GetQcRecordInvoke(dtDoctorLimit);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetQcRecordInvoke(DataTable dtDoctorLimit)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<DataTable>(GetQcRecordInvoke), dtDoctorLimit);
            }
            else
            {
                GetQcRecord(dtDoctorLimit);
            }
        }

        /// <summary>
        /// 指定DataTable添加列
        /// xlb 2013-01-10
        /// </summary>
        /// <param name="dt">指定datable</param>
        /// <param name="fieldName">列名</param>
        /// <param name="caption">列标题</param>
        /// <param name="dataType">列类型</param>
        private void AddDataElement(DataTable dt, string fieldName, string caption, Type dataType)
        {
            try
            {
                DataColumn dColumn = new DataColumn(fieldName, dataType);
                dColumn.Caption = caption;

                dt.Columns.Add(dColumn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}

