using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.FrameWork.WinForm.Plugin;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DrectSoft.Common;
using DrectSoft.FrameWork;
using DrectSoft.Service;
using System.Xml;
using System.Threading;
using DevExpress.LookAndFeel;

namespace DrectSoft.Core.OwnBedInfo
{
    public partial class ReplenishPatRec : UserControl
    {
        internal IEmrHost m_App;
        public ReplenishPatRec()
        {
            InitializeComponent();
        }


        public void LoadData(IEmrHost app)
        {
            m_App = app;
            InitInpatientList();
            LoadData();
            textEdit2.Focus();
        }
        
        /// <summary>
        /// 初始化病人列表
        /// edit by Yanqiao.Cai 2012-11-15
        /// 1、add try ... catch
        /// 2、加载性别图片
        /// </summary>
        private void InitInpatientList()
        {
            try
            {
                SetInHosOrInWardDate();
                //加载性别图片
                DS_Common.InitializeImage_XB(repositoryItemImageXB, imageListXB);
                string sqlGetInpatient = DataManager.GetAllUnRebackRecordsByDeptRights(DS_Common.currentUser.Id);
                if (string.IsNullOrEmpty(sqlGetInpatient)) return;

                panelControlPatientList.Visible = true;

                Thread getInpatientListThread = new Thread(new ParameterizedThreadStart(
                    delegate
                    {
                        try
                        {
                            DataTable dt = m_App.SqlHelper.ExecuteDataTable(sqlGetInpatient, CommandType.Text);
                            InitInpatientListInner(dt);
                        }
                        catch (Exception ex)
                        {
                            DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
                        }
                    }
                ));
                getInpatientListThread.Start();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void InitInpatientListInner(DataTable dt)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action<DataTable>(InitInpatientListInner), dt);
                }
                else
                {
                    //设置入院日期为入院时间还是入科日期（依据配置）add by cyq 2013-03-14
                    gridControlInpatientList.DataSource = dt;
                    FilterInpatientList();
                    panelControlPatientList.Visible = false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("InitInpatientListInner" + ex.Message);
            }
        }

        private void BindData(DataRow row)
        {
            textEdit_Dept.Text = row["DEPT1"].ToString();
            textEdit_dept2.Text = row["DEPT2"].ToString();
            textEdit_Name.Text = row["PATNAME"].ToString();
            textEdit_Sexy.Text = row["SEXNAME"].ToString();
            memoEdit1.Text = row["REASON"].ToString();
            textEdit_Days.Text = row["Days"].ToString();
            textEditPatID.Text = row["PATID"].ToString();
        }

        private bool CommitData()
        {
            if (string.IsNullOrEmpty(textEditPatID.Text.Trim()))
            {
                m_App.CustomMessageBox.MessageShow("请先选择病人");
                return false;
            }

            if (string.IsNullOrEmpty(textEdit_Days.Text))
            {
                m_App.CustomMessageBox.MessageShow("请填写申请天数");
                textEdit_Days.Focus();
                return false;
            }

            return true;
        }
        /// <summary>
        /// 画面检查
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-13</date>
        /// </summary>
        private string CheckItem()
        {
            try
            {
                if (string.IsNullOrEmpty(textEdit_Days.Text))
                {
                    textEdit_Days.Focus();
                    return "申请天数不能为空";
                }
                else if (!Tool.IsInt(textEdit_Days.Text.Trim()))
                {
                    textEdit_Days.Focus();
                    return "申请天数只能是整数";
                }
                else if (int.Parse(textEdit_Days.Text.Trim()) < 0)
                {
                    textEdit_Days.Focus();
                    return "申请天数只能是正数";
                }
                else if (string.IsNullOrEmpty(this.memoEdit1.Text.Trim()))
                {
                    memoEdit1.Focus();
                    return "申请原因不能为空";
                }
                else if (Tool.GetByteLength(this.memoEdit1.Text) > 255)
                {
                    memoEdit1.Focus();
                    return "申请原因最大字符长度为255";
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

       string non="";
       /// <summary>
       /// 保存方法
       /// edit by Yanqiao.Cai 2012-11-13
       /// 1、add try ... catch
       /// 2、添加提示信息
       /// </summary>
        private void SaveData()
        {
            try
            {
                //edit by cyq 2012-11-13 修改提示信息
                if (string.IsNullOrEmpty(textEditPatID.Text.Trim()))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条病人记录");
                    return;
                }
                string errorStr = CheckItem();
                if (!string.IsNullOrEmpty(errorStr))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(errorStr);
                    return;
                }
                //if (!CommitData()) return;
                //add by ywk 2012年3月30日15:46:19  //xll2013-05-29 修改sql过滤申请时间过期的
                string dissql = string.Format(@" select noofinpat from ReplenishPatRec where noofinpat 
                ='{0}' and createuser = '{1}' and sysdate- to_date(createdate,'yyyy-mm-dd,hh24:mi:ss')<=days  and valid=1", textEditPatID.Tag.ToString().Trim(), m_App.User.DoctorId);
                DataTable dt = m_App.SqlHelper.ExecuteDataTable(dissql);
                if (dt.Rows.Count > 0)//已经申请过了
                {
                    m_App.CustomMessageBox.MessageShow("此病人已经申请补写病历");
                    non = dt.Rows[0]["noofinpat"].ToString(); ;
                    DataTable dt1 = this.gridHistoryInp.DataSource as DataTable;//取得左侧大列表的数据源
                    int i = 0;
                    foreach (DataRow myrow in dt1.Rows)//循环遍历Datatable
                    {
                        i = i + 1;
                        if (myrow["NOOFINPAT"].ToString() == non)
                        {
                            this.gridViewHistoryInfo.FocusedRowHandle = i - 1;//焦点移到相应地方
                            //this.gridViewHistoryInfo.FocusRectStyle = DrawFocusRectStyle.RowFocus;

                        }
                    }
                    return;
                }
                string sqlcmd = string.Format(insertTablesql, textEditPatID.Tag.ToString().Trim(), m_App.User.Id, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), memoEdit1.Text, textEdit_Days.Text);
                m_App.SqlHelper.ExecuteNoneQuery(sqlcmd);
                m_App.CustomMessageBox.MessageShow("申请成功");
                LoadData();
            }
            catch (Exception ex)
            {
                m_App.CustomMessageBox.MessageShow(ex.Message);
            }

        }

        const string insertTablesql = "insert into ReplenishPatRec(id,noofinpat,Createuser,createdate,Reason,days,valid) values((SELECT NVL(MAX(ID), 0) + 1  FROM ReplenishPatRec),{0},'{1}','{2}','{3}',{4},1)";
        const string sql_query = @"select a.noofinpat, b.name patname,b.sexid,c.name sexname,b.patid,b.agestr,d.name dept1,e.name dept2,substr(b.admitdate, 1, 16) as admitdate,substr(b.inwarddate, 1, 16) as inwarddate,f.name zdmc,substr(a.createdate, 1, 16) as createdate,a.reason,a.days  from replenishpatrec a 
                                        inner join  inpatient b on a.noofinpat=b.noofinpat
                                        LEFT JOIN dictionary_detail c ON c.detailid = b.sexid  AND c.categoryid = '3'
                                        LEFT JOIN department d ON b.admitdept = d.ID
                                        LEFT JOIN department e ON b.outhosdept = e.ID
                                        LEFT JOIN diagnosis f ON f.icd = b.admitdiagnosis ";
        /// <summary>
        /// 加载已申请病人列表
        /// edit by Yanqiao.Cai 2012-11-15
        /// 1、add try ... catch
        /// 2、加载性别图片
        /// </summary>
        private void LoadData()
        {
            try
            {
                //设置入院日期为入院时间还是入科日期（依据配置）add by cyq 2013-03-14
                SetInHosOrInWardDate();
                //加载性别图片
                DS_Common.InitializeImage_XB(repositoryItemImageApplyXB, imageListXB);
                //sql查询语句更改为申请人能看到自己申请的信息 edit by ywk 2012年3月28日17:34:47
                string mysql = sql_query + string.Format(" where a.createuser='{0}' and sysdate- to_date(createdate,'yyyy-mm-dd,hh24:mi:ss')<=days order by a.createdate desc", m_App.User.Id);
                gridHistoryInp.DataSource = m_App.SqlHelper.ExecuteDataTable(mysql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void btnAddPatRec_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        private void btn_ReWrite_Click(object sender, EventArgs e)
        {
            if (gridViewHistoryInfo.FocusedRowHandle < 0) return;
            DataRow row = gridViewHistoryInfo.GetDataRow(gridViewHistoryInfo.FocusedRowHandle);
            m_App.ChoosePatient(Convert.ToDecimal(row["noofinpat"]), FloderState.NoneAudit.ToString());
            m_App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());
        }

        private void gridViewHistoryInfo_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle < 0) return;
            DataRow row = gridViewHistoryInfo.GetDataRow(e.FocusedRowHandle);
            BindData(row);
        }

        private void gridViewHistoryInfo_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
        }

        private void ReplenishPatRec_Load(object sender, EventArgs e)
        {
            try
            {
                SetChineseIEM();
                textEdit2.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 设置入院日期为入院时间还是入科日期（依据配置）
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-14</date>
        public void SetInHosOrInWardDate()
        {
            try
            {
                //获取入院日期（配置） edit by cyq 2013-03-13
                string config = DS_SqlService.GetConfigValueByKey("EmrInputConfig");
                if (!string.IsNullOrEmpty(config))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(config);
                    XmlNodeList nodeList = doc.GetElementsByTagName("InHosTimeType");
                    if (null != nodeList && nodeList.Count > 0)
                    {
                        string cfgValue = null == nodeList[0].InnerText ? "" : nodeList[0].InnerText.Trim();
                        if (cfgValue == "1")
                        {//入科
                            gridViewInpatientList.Columns[7].FieldName = "INWARDDATE";
                            gridViewHistoryInfo.Columns[7].FieldName = "INWARDDATE";
                        }
                        else
                        {//入院
                            gridViewInpatientList.Columns[7].FieldName = "ADMITDATE";
                            gridViewHistoryInfo.Columns[7].FieldName = "ADMITDATE";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void textEdit2_TextChanged(object sender, EventArgs e)
        {
            FilterInpatientList();
        }

        private void gridViewInpatientList_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            FocusedInpatientList();
        }

        private void FocusedInpatientList()
        {
            DataRowView drv = gridViewInpatientList.GetRow(gridViewInpatientList.FocusedRowHandle) as DataRowView;
            if (drv != null)
            {
                textEdit_Dept.Text = drv["admitdeptname"].ToString();
                textEdit_dept2.Text = drv["outdeptname"].ToString();
                textEdit_Name.Text = drv["PATNAME"].ToString();
                textEdit_Sexy.Text = drv["SEXNAME"].ToString();
                textEditPatID.Text = drv["PATID"].ToString();
                textEditPatID.Tag = drv["NOOFINPAT"].ToString();
            }
        }

        private void FilterInpatientList()
        {
            DataTable dt = (DataTable)gridControlInpatientList.DataSource;
            if (dt != null && dt.Rows.Count > 0)
            {
                string deptName = string.Empty;
                if (checkDept.Checked)
                {
                    deptName = m_App.User.CurrentDeptName.Trim();
                }

                dt.DefaultView.RowFilter = @" (PATNAME like '%" + textEdit2.Text.Trim() + "%' or PY like '%" + textEdit2.Text.Trim() + "%' or WB like '%" + textEdit2.Text.Trim() + "%'"
                        + " or ( admitdeptname like '%" + textEdit2.Text.Trim() + "%' or outdeptname like '%" + textEdit2.Text.Trim() + "%' ) "
                        + " or AGESTR like '%" + textEdit2.Text.Trim() + "%' "
                        + " or SEXNAME like '%" + textEdit2.Text.Trim() + "%' "
                        + " or BEDID like '%" + textEdit2.Text.Trim() + "%' ) "
                +" and ( admitdeptname like '%" + deptName + "%' or outdeptname like '%" + deptName + "%' ) ";
            }
            FocusedInpatientList();
        }

        /// <summary>
        /// 设置中文输入法 Add By wwj 2012-04-16
        /// </summary>
        private void SetChineseIEM()
        {
            this.ImeMode = System.Windows.Forms.ImeMode.Off;
            //if (this.ImeMode != System.Windows.Forms.ImeMode.Off)
            {
                foreach (InputLanguage MyInput in InputLanguage.InstalledInputLanguages)
                {
                    if (MyInput.LayoutName.IndexOf("拼音") >= 0)
                    {
                        InputLanguage.CurrentInputLanguage = MyInput;
                        break;
                    }
                }
            }
        }

        private void checkDept_CheckedChanged(object sender, EventArgs e)
        {
            FilterInpatientList();
        }

        private void gridHistoryInp_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GridHitInfo hitInfo = gridViewHistoryInfo.CalcHitInfo(e.Location);
            if (hitInfo.RowHandle >= 0)
            {
                DataRowView drv = gridViewHistoryInfo.GetRow(hitInfo.RowHandle) as DataRowView;
                if (drv != null)
                {
                    m_App.ChoosePatient(Convert.ToDecimal(drv["noofinpat"]), FloderState.NoneAudit.ToString());
                    m_App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());
                }
            }
        }

        public void FocusFirstControl()
        {
            textEdit2.Focus();
        }

        /// <summary>
        /// 复选框回车事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-23</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_KeyPress(object sender, KeyPressEventArgs e)
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 序号
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-25</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewHistoryInfo_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 加序号列
        /// Add BY XLB 2013-6-08
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewInpatientList_CustomDrawRowIndicator
            (object sender,
            RowIndicatorCustomDrawEventArgs e)
        {
            DS_Common.AutoIndex(e);
        }

    }
}
