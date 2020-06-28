using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Report;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Service;
using DrectSoft.Wordbook;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace DrectSoft.Core.QCDeptReport
{
    public partial class UCFail : DevExpress.XtraEditors.XtraUserControl
    {
        #region 变量


        /// <summary>
        /// 创建插件的应用程序接口
        /// </summary>
        private IEmrHost _app;
        /// <summary>
        /// 创建插件的应用程序接口
        /// </summary>
        public IEmrHost App
        {
            get { return _app; }
            set { _app = value; }
        }

        /// <summary>
        /// 连接数据库的SqlHelper
        /// </summary>
        private IDataAccess m_DataAccessEmrly;


        private DataTable m_pageSouce;

        #endregion

        public UCFail(IEmrHost application)
        {
            InitializeComponent();

            InitApp(application);
        }

        protected virtual void InitApp(IEmrHost application)
        {
            try
            {
                App = application;
                m_DataAccessEmrly = App.SqlHelper;
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
        private void UserControlFail_Load(object sender, EventArgs e)
        {
            try
            {
                this.dateBegin.DateTime = DateTime.Now.AddMonths(-1);
                this.dateEnd.DateTime = DateTime.Now;
                txtDeptName.Text = App.User.CurrentDeptName;

                InitSqlWorkBook();
                UpEditorInpatient.Focus();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #region 方法

        /// <summary>
        /// 设置入院日期为入院时间还是入科日期（依据配置）
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-14</date>
        public void SetInHosOrInWardDate()
        {
            try
            {
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
                            gridViewInpatientFail.Columns[9].FieldName = "INWARDDATE";
                        }
                        else
                        {//入院
                            gridViewInpatientFail.Columns[9].FieldName = "ADMITDATE";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 窗体加载事件
        /// edit by Yanqiao.Cai 2012-11-14
        /// 1、add try ... catch
        /// 2、名称添加婴儿
        /// 3、加载性别图片 edit by Yanqiao.Cai 2012-11-15
        /// 特殊符号处理  王冀 2013 1 7
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetInpatientSouce(string BeginTime, string EndTime, string inpatName, string dia)
        {
            try
            {
                //设置入院日期为入院时间还是入科日期（依据配置）add by cyq 2013-03-14
                SetInHosOrInWardDate();
                gridViewInpatientFail.CustomDrawEmptyForeground -= new DevExpress.XtraGrid.Views.Base.CustomDrawEventHandler(gridViewInpatientFail_CustomDrawEmptyForeground);
                SqlParameter[] sqlParams = new SqlParameter[] { 
                  new SqlParameter("@deptcode", SqlDbType.VarChar),
                  new SqlParameter("@InpatName", SqlDbType.VarChar),
                  new SqlParameter("@datetimebegin", SqlDbType.VarChar),
                  new SqlParameter("@datetimeend",SqlDbType.VarChar),
                  new SqlParameter("@userid",SqlDbType.VarChar),
                  new SqlParameter("@qcstattype",SqlDbType.Int),
                  new SqlParameter("@result", SqlDbType.Structured)
                  };

                sqlParams[0].Value = App.User.CurrentDeptId == null ? "" : App.User.CurrentDeptId;
                sqlParams[1].Value = inpatName;
                sqlParams[2].Value = BeginTime;
                sqlParams[3].Value = EndTime;
                sqlParams[4].Value = App.User.Id;
                sqlParams[5].Value = 1;
                sqlParams[6].Direction = ParameterDirection.Output;
                //DataSet ds = m_DataAccessEmrly.ExecuteDataSet("usp_GetInpatientFiling", sqlParams, CommandType.StoredProcedure);
                DrectSoft.DSSqlHelper.DS_SqlHelper.CreateSqlHelper();
                DataSet ds = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataSet("EMRPROC.usp_GetInpatientFiling", sqlParams, CommandType.StoredProcedure);
                m_pageSouce = ds.Tables[0];
                if (dia.Length > 0)
                {
                    m_pageSouce = ToDataTable(m_pageSouce.Select(string.Format(@"ADMITDIAG like '%{0}%'", dia)));
                }
                if (m_pageSouce == null || m_pageSouce.Rows.Count == 0)
                {
                    //gridViewInpatientFail.CustomDrawEmptyForeground += new DevExpress.XtraGrid.Views.Base.CustomDrawEventHandler(gridViewInpatientFail_CustomDrawEmptyForeground);
                    MessageBox.Show("沒有符合条件的数据");
                    gridInpatientFail.DataSource = null;
                    return;
                }
                //名称添加婴儿个数
                DataTable dt = m_pageSouce;
                string ResultName = string.Empty;//声明最终要在列表显示的姓名的内容
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ResultName = GetPatsBabyContent(_app, dt.Rows[i]["noofinpat"].ToString());
                    dt.Rows[i]["Name"] = ResultName;
                }
                //加载性别图片
                DS_Common.InitializeImage_XB(repositoryItemImageXB, imageListXB);

                gridInpatientFail.DataSource = dt;
                _app.PublicMethod.ConvertGridDataSourceUpper(gridViewInpatientFail);
                if (m_pageSouce.Rows.Count == 0)
                {
                    MessageBox.Show("沒有符合条件的数据");
                    gridInpatientFail.DataSource = new DataTable();
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 显示自定义字符事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gridViewInpatientFail_CustomDrawEmptyForeground(object sender, DevExpress.XtraGrid.Views.Base.CustomDrawEventArgs e)
        {
            try
            {
                DS_Common.CustomDrawEmptyDataSource("没有查询到您想要的数据", new Font("宋体", 10, FontStyle.Bold), Brushes.Red, e);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private decimal GetCurrentPat()
        {
            try
            {
                if (gridViewInpatientFail.FocusedRowHandle < 0)
                {
                    return -1;
                }

                DataRow dataRow = gridViewInpatientFail.GetDataRow(gridViewInpatientFail.FocusedRowHandle);
                if (dataRow == null)
                {
                    return -1;
                }
                return Convert.ToDecimal(dataRow["NOOFINPAT"]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 绑定患者姓名
        /// 住院号应该是patid
        /// Modify by xlb 2013-06-25
        /// </summary>
        private void InitSqlWorkBook()
        {
            try
            {
                UpWindowtInpatient.SqlHelper = _app.SqlHelper;

                DataTable dtInpatient = GetInpatientList();

                if (dtInpatient.Rows.Count == 0)
                    return;
                dtInpatient.Columns["PATID"].Caption = "住院号";
                dtInpatient.Columns["NAME"].Caption = "患者姓名";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("PATID", 50);
                cols.Add("NAME", 100);

                SqlWordbook Inpatientworkbook = new SqlWordbook("querybook", dtInpatient, "PATID", "NAME", cols, "PATID//NAME//PY//WB");

                UpEditorInpatient.SqlWordbook = Inpatientworkbook;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取病患下拉框数据源
        /// </summary>
        public DataTable GetInpatientList()
        {
            try
            {
                String sql = string.Format("select a.NoOfInpat,a.PatID,a.Name,a.PY,a.WB from dbo.InPatient a where a.OutHosDept = '{0}'", _app.User.CurrentDeptId);

                DataSet ds = m_DataAccessEmrly.ExecuteDataSet(sql);

                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataRow GetSelectedGridViewRow()
        {
            try
            {
                return gridViewInpatientFail.GetDataRow(gridViewInpatientFail.FocusedRowHandle);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void FocusFirstControl()
        {
            try
            {
                if (txtDeptName.Enabled == true)
                {
                    txtDeptName.Focus();
                }
                else
                {
                    UpEditorInpatient.Focus();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void refreshQuery()
        {
            try
            {
                btnQuery_Click(null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 数组DataRow[]转化成表DataTable
        /// 王冀 2013 1 6
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        public DataTable ToDataTable(DataRow[] rows)
        {
            try
            {
                if (rows == null || rows.Length == 0) return null;
                DataTable tmp = rows[0].Table.Clone();
                foreach (DataRow row in rows)
                    tmp.ImportRow(row);
                return tmp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 重置查询条件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-22</date>
        /// </summary>
        public void Reset()
        {
            try
            {
                this.dateBegin.DateTime = DateTime.Now.AddMonths(-1);
                this.dateEnd.DateTime = DateTime.Now;
                txtDeptName.Text = App.User.CurrentDeptName;
                UpEditorInpatient.CodeValue = "";
                this.textEditHistory.Text = "";
                this.UpEditorInpatient.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据病人的首页序号，得到她的婴儿个数，返回显示内容
        /// add by ywk 2012年6月8日 09:47:53
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public string GetPatsBabyContent(IEmrHost m_app, string noofinpat)
        {
            try
            {
                string Result = string.Empty;//最终要返回的内容
                string sql = string.Format(@"select babycount,name from inpatient where noofinpat='{0}'", noofinpat);
                if (App == null)
                {
                    App = m_app;
                }
                DataTable dt = App.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
                if (string.IsNullOrEmpty(dt.Rows[0]["babycount"].ToString()))
                {
                    Result = dt.Rows[0]["Name"].ToString();

                }
                else
                {
                    if (dt.Rows[0]["babycount"].ToString() == "0")
                    {
                        Result = dt.Rows[0]["Name"].ToString();
                    }
                    else
                    {
                        Result = dt.Rows[0]["Name"].ToString() + "【" + dt.Rows[0]["babycount"].ToString() + "个婴儿】";
                    }
                }
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// /根据病人首页序号获得她孩子的个数
        /// add ywk
        /// 解除循环引用的问题 
        /// 原医师工作站的实现移植到此处
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        public bool HasBaby(string noofinpat)
        {
            try
            {
                string sql = string.Format(@"select babycount from inpatient where noofinpat='{0}'", noofinpat);
                DataTable dt = App.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["babycount"].ToString() == "0")
                    {
                        return false;
                    }
                    if (Int32.Parse(dt.Rows[0]["babycount"].ToString() == "" ?
                        "0" : dt.Rows[0]["babycount"].ToString()) > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 事件

        /// <summary>
        /// 打印事件
        /// edit by Yanqiao.Cai 2012-11-13
        /// 1、add try ... catch
        /// 2、添加提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_pageSouce == null)
                {
                    MessageBox.Show("请先查询数据");
                    return;
                }
                XReport xreport = new XReport(m_pageSouce.Copy(), @"ReportFail.repx");

                xreport.ShowPreview();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 窗体加载事件
        /// edit by Yanqiao.Cai 2012-11-15
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                string begintime = dateBegin.DateTime.ToString("yyyy-MM-dd");
                string endtime = dateEnd.DateTime.ToString("yyyy-MM-dd");
                //此为按照病患姓名查询怎么会按照病人号呢
                //string NoOfInpat = UpEditorInpatient.CodeValue == null ? "" : UpEditorInpatient.CodeValue;
                string inpatName = UpEditorInpatient.CodeValue == null ? "" : UpEditorInpatient.CodeValue;
                string dia = textEditHistory.Text.Trim().Replace("'", "''").Replace("*", "[*]").Replace("%", "[%]");

                GetInpatientSouce(begintime, endtime, inpatName, dia);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 双击事件
        /// edit by Yanqiao.Cai 2012-11-12
        /// 1、add try ... catch
        /// 2、双击小标题无操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridHitInfo hitInfo = gridViewInpatientFail.CalcHitInfo(gridInpatientFail.PointToClient(Cursor.Position));
                if (hitInfo.RowHandle < 0)
                {
                    return;
                }
                DataRow dataRow = gridViewInpatientFail.GetDataRow(gridViewInpatientFail.FocusedRowHandle);
                if (null == dataRow)
                {
                    return;
                }
                string noofinpat = dataRow["noofinpat"].ToString();
                if (HasBaby(noofinpat))
                {
                    ChoosePatOrBaby choosepat = new ChoosePatOrBaby(_app, noofinpat);
                    choosepat.StartPosition = FormStartPosition.CenterParent;
                    if (choosepat.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        _app.ChoosePatient(decimal.Parse(choosepat.NOOfINPAT));
                        _app.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());
                    }
                }
                else
                {
                    _app.ChoosePatient(Convert.ToDecimal(noofinpat));
                    _app.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());
                }
                #region edit by cyq 2012-11-14
                //GridHitInfo hitInfo = gridViewInpatientFail.CalcHitInfo(gridInpatientFail.PointToClient(Cursor.Position));
                //if (hitInfo.RowHandle < 0)
                //{
                //    return;
                //}
                //decimal syxh = GetCurrentPat();
                //if (syxh < 0) return;

                //App.ChoosePatient(syxh);
                //App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());
                #endregion
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 列表事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-14</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewInpatientFail_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                if (e.CellValue == null)
                {
                    return;
                }
                DataRowView drv = gridViewInpatientFail.GetRow(e.RowHandle) as DataRowView;
                //取得病人名字
                string patname = drv["NAME"].ToString().Trim();
                if (e.Column.FieldName == "NAME")
                {
                    if (patname.Contains("婴儿"))
                    {
                        e.Graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, e.Bounds.Width, e.Bounds.Height));
                        e.Graphics.DrawString(patname, e.Appearance.Font, Brushes.Red, e.Bounds);
                        e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #endregion

        /// <summary>
        /// 回车切换焦点
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-10-11</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == 13)
                {
                    SendKeys.Send("{Tab}");
                    SendKeys.Flush();
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
        /// <date>2011-10-22</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewInpatientFail_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 重置事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-22</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonReset_Click(object sender, EventArgs e)
        {
            try
            {
                Reset();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 编辑病人信息方法
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-09</date>
        /// </summary>
        public string EditPatientInfo()
        {
            try
            {
                if (gridViewInpatientFail.FocusedRowHandle < 0)
                {
                    MessageBox.Show("请选中一条记录");
                    return "";
                }
                DataRow dataRow = gridViewInpatientFail.GetDataRow(gridViewInpatientFail.FocusedRowHandle);
                if (dataRow == null)//add by xlb 2012-12-24
                {
                    return "";
                }
                string syxh = dataRow["Noofinpat"].ToString();
                return syxh;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
