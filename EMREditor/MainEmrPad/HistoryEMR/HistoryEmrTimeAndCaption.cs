using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Core.MainEmrPad.New;
using DrectSoft.Emr.Util;
using DrectSoft.DSSqlHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DrectSoft.Core.MainEmrPad.HistoryEMR
{
    public partial class HistoryEmrTimeAndCaption : DevBaseForm
    {
        public List<EmrNode> EmrNodeList
        {
            get
            {
                return m_EmrNodeList;
            }
        }
        List<EmrNode> m_EmrNodeList;

        DrectSoft.Core.MainEmrPad.New.UCEmrInput m_CurrentUCEmrInput;

        public HistoryEmrTimeAndCaption(List<EmrNode> emrNodeList, DrectSoft.Core.MainEmrPad.New.UCEmrInput ucEmrInput)
        {
            InitializeComponent();
            InitDataSource();
            m_EmrNodeList = emrNodeList;
            m_CurrentUCEmrInput = ucEmrInput;
        }

        /// <summary>
        /// 设置数据源
        /// </summary>
        private void InitDataSource()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("EMRID");//病程编号
                dt.Columns.Add("EMRNAME");//病程名称
                dt.Columns.Add("EMRDATE");//病程日期
                dt.Columns.Add("EMRTIME");//病程时间
                dt.Columns.Add("EMRTITLE");//病程标题
                dt.Columns.Add("FIRSTDAILYFLAG");//病程标题
                gridControlDailyEmr.DataSource = dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void HistoryEmrTimeAndCaption_Load(object sender, EventArgs e)
        {
            try
            {
                InitForm();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void InitForm()
        {
            try
            {
                DataTable dtSource = gridControlDailyEmr.DataSource as DataTable;
                foreach (EmrNode node in m_EmrNodeList)
                {
                    DataRow dr = node.DataRowItem;
                    if (dr["sortid"].ToString() == "AC")
                    {
                        DataRow newDr = dtSource.NewRow();
                        newDr["EMRID"] = dr["ID"];
                        newDr["EMRNAME"] = dr["NAME"];
                        newDr["EMRDATE"] = dr["CAPTIONDATETIME"].ToString().Split(' ')[0];
                        newDr["EMRTIME"] = dr["CAPTIONDATETIME"].ToString().Split(' ')[1];
                        newDr["EMRTITLE"] = dr["MR_NAME"];
                        newDr["FIRSTDAILYFLAG"] = dr["ISFIRSTDAILY"];
                        dtSource.Rows.Add(newDr);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void gridViewDailyEmr_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                this.gridViewDailyEmr.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridViewDailyEmr_CellValueChanged);
                if (e.Column == gridColumnEmrDate)
                {
                    string value = Convert.ToDateTime(e.Value).ToString("yyyy-MM-dd HH:mm:ss").Split(' ')[0];
                    gridViewDailyEmr.SetRowCellValue(e.RowHandle, gridColumnEmrDate, value);
                }
                else if (e.Column == gridColumnEmrTime)
                {
                    string value = Convert.ToDateTime(e.Value).ToString("yyyy-MM-dd HH:mm:ss").Split(' ')[1];
                    gridViewDailyEmr.SetRowCellValue(e.RowHandle, gridColumnEmrTime, value);
                }
                this.gridViewDailyEmr.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridViewDailyEmr_CellValueChanged);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckDailyEmr())//检查病程时间的有效情况
                {
                    if (!CheckEmrDateIsSame())//检查病程是否存在相同的情况
                    {
                        DataTable dt = gridControlDailyEmr.DataSource as DataTable;
                        foreach (DataRow dr in dt.Rows)
                        {
                            m_EmrNodeList.ForEach(node =>
                            {
                                if (node.ID == dr["EMRID"].ToString())
                                {
                                    node.DataRowItem["captiondatetime"] = dr["EMRDATE"] + " " + dr["EMRTIME"];
                                    node.DataRowItem["EMRTITLE"] = dr["EMRTITLE"];
                                }
                            });
                        }
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 检查是否出现相同时间的问题
        /// </summary>
        /// <returns></returns>
        private bool CheckEmrDateIsSame()
        {
            try
            {
                DataTable dt = gridControlDailyEmr.DataSource as DataTable;

                foreach (DataRow dataRow in dt.Rows)
                {
                    string captiondatetime = dataRow["EMRDATE"] + " " + dataRow["EMRTIME"];
                    int cnt = dt.AsEnumerable().Cast<DataRow>().Where(row =>
                        {
                            if (row["EMRDATE"] + " " + row["EMRTIME"] == captiondatetime)
                            {
                                return true;
                            }
                            return false;
                        }).Count();

                    if (cnt > 1)
                    {
                        MyMessageBox.Show("病程记录：" + dataRow["EMRNAME"].ToString() + " 的病程时间出现重复请修改。", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                        return true;
                    }
                }

                //获取内存中所有病程的节点
                List<EmrModel> emrModelList = Util.GetAllEmrModels(m_CurrentUCEmrInput.CurrentInputBody.CurrentTreeList.Nodes, new List<EmrModel>());
                emrModelList = emrModelList.Where(emrModel => { return emrModel.ModelCatalog == "AC"; }).ToList();
                if (emrModelList.Count > 0)
                {
                    foreach (DataRow dataRow in dt.Rows)
                    {
                        string captiondatetime = dataRow["EMRDATE"] + " " + dataRow["EMRTIME"];
                        int cnt = emrModelList.Where(model =>
                        {
                            if (model.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") == captiondatetime)
                            {
                                return true;
                            }
                            return false;
                        }).Count();

                        if (cnt >= 1)
                        {
                            MyMessageBox.Show("病程记录：" + dataRow["EMRNAME"].ToString() + " 的病程时间出现重复请修改。", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                            return true;
                        }
                    }
                }

                //获取数据库中所有的病程节点
                List<string> captionDateTimeList = GetDailyEmrModel(m_CurrentUCEmrInput.CurrentInpatient.NoOfFirstPage.ToString());
                if (captionDateTimeList.Count > 0)
                {
                    foreach (DataRow dataRow in dt.Rows)
                    {
                        string captiondatetime = dataRow["EMRDATE"] + " " + dataRow["EMRTIME"];
                        int cnt = captionDateTimeList.Where(cpDateTime =>
                        {
                            if (cpDateTime == captiondatetime)
                            {
                                return true;
                            }
                            return false;
                        }).Count();

                        if (cnt >= 1)
                        {
                            MyMessageBox.Show("病程记录：" + dataRow["EMRNAME"].ToString() + " 的病程时间出现重复请修改。", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                            return true;
                        }
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
                return false;
            }
        }

        /// <summary>
        /// 获取数据库中所有的病程记录
        /// </summary>
        /// <returns></returns>
        private List<string> GetDailyEmrModel(string noofinpat)
        {
            try
            {
                string sqlGetDailyEmrModel = string.Format(
                        @"SELECT r.captiondatetime FROM recorddetail r WHERE r.noofinpat = '{0}' and r.valid = '1' and r.sortid = 'AC'", noofinpat);
                DataTable dt = DS_SqlHelper.ExecuteDataTable(sqlGetDailyEmrModel, CommandType.Text);
                List<string> captionDateTimeList = new List<string>();
                foreach (DataRow dr in dt.Rows)
                {
                    captionDateTimeList.Add(dr["captiondatetime"].ToString());
                }
                return captionDateTimeList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取数据库中首次病程的时间
        /// </summary>
        /// <returns></returns>
        private string GetFirstDailyEmrCaptionDateTime(string noofinpat)
        {
            try
            {
                string sqlGetCaptionDateTime = string.Format(
                    @"SELECT r.captiondatetime FROM recorddetail r WHERE r.noofinpat = '{0}' and r.valid = '1' and r.sortid = 'AC' and r.firstdailyflag = 1", noofinpat);
                DataTable dt = DS_SqlHelper.ExecuteDataTable(sqlGetCaptionDateTime, CommandType.Text);
                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0][0].ToString();
                }
                return "";
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 检查病程时间
        /// </summary>
        /// <returns></returns>
        private bool CheckDailyEmr()
        {
            //获取数据库中的首次病程
            string displayDateTime = GetFirstDailyEmrCaptionDateTime(m_CurrentUCEmrInput.CurrentInpatient.NoOfFirstPage.ToString());

            //获取内存中的首次病程
            EmrModel firstDailyEmrModel = Util.GetFirstDailyEmrModel(m_CurrentUCEmrInput.CurrentInputBody.CurrentTreeList);

            if (displayDateTime == "" && firstDailyEmrModel != null)
            {
                displayDateTime = firstDailyEmrModel.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss");
            }

            DataTable dt = gridControlDailyEmr.DataSource as DataTable;
            //获取Grid中首次病程的数量
            List<DataRow> firstDailyEmrCaptionDateTime = dt.Rows.Cast<DataRow>().Where(dr =>
            {
                if (dr["FIRSTDAILYFLAG"].ToString() == "1")
                {
                    return true;
                }
                return false;
            }).ToList();

            if (displayDateTime != "")//内存或数据库中已经存在首次病程
            {
                if (firstDailyEmrCaptionDateTime.Count() > 0)//Grid中也存在首次病程
                {
                    MyMessageBox.Show("已经存在首次病程，不能重复增加。", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                    return false;
                }
            }
            else
            {
                if (firstDailyEmrCaptionDateTime.Count() == 0)//Grid中不存在首次病程
                {
                    MyMessageBox.Show("导入的病程需要包括首次病程。", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                    return false;
                }
                else
                {
                    //displayDateTime = firstDailyEmrCaptionDateTime[0]["captiondatetime"].ToString();
                    displayDateTime = firstDailyEmrCaptionDateTime[0]["EMRDATE"].ToString() + " " + firstDailyEmrCaptionDateTime[0]["EMRTIME"].ToString();
                }
            }

            if (displayDateTime == "")
            {
                MyMessageBox.Show("首次病程不存在，需要导入。", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                return false;
            }
            else
            {
                int errorCnt = dt.Rows.Cast<DataRow>().Where(dr =>
                    {
                        if (dr["FIRSTDAILYFLAG"].ToString() != "1")//排除首程
                        {
                            DateTime displayDateTimeInGrid = Convert.ToDateTime(dr["EMRDATE"] + " " + dr["EMRTIME"]);
                            DateTime firstDisplayDateTime = Convert.ToDateTime(displayDateTime);
                            if ((firstDisplayDateTime - displayDateTimeInGrid).TotalSeconds >= 0)
                            {
                                return true;
                            }
                        }
                        return false;
                    }).Count();
                if (errorCnt > 0)
                {
                    MyMessageBox.Show("病程时间应该大于首次病程的时间  首次病程时间：【" + displayDateTime + "】", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                    return false;
                }
            }
            return true;
        }
    }
}