using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Eop;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Service;
using System;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DrectSoft.Core.IEMMainPage
{
    /// <summary>
    /// 费用编辑界面
    /// </summary>
    public partial class UCOthers : UserControl
    {
        private IemMainPageInfo m_IemInfo;
        private Inpatient CurrentInpatient;//add by ywk 
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

        public bool editFlag = false;  //add by cyq 2012-12-06 病案室人员编辑首页(状态改为归档)

        #region 费用编辑界面相关方法
        /// <summary>
        /// 构造函数
        /// Modify by xlb 2013-04-22
        /// </summary>
        public UCOthers()
        {
            try
            {
                InitializeComponent();
                this.ActiveControl = txtTotal;
                //SetStyle(ControlStyles.UserPaint, true);/*控件自行绘制*/
                //SetStyle(ControlStyles.AllPaintingInWmPaint, true); /*禁止擦除背景*/
                //SetStyle(ControlStyles.DoubleBuffer, true); /*双缓冲(绘制在缓冲区进行完成后输出到屏幕上)*/
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetFeeData()
        {
            if (m_IemInfo != null)
            {
                //lblTotal.Text
            }
        }

        /// <summary>
        /// FILL UI
        /// </summary>
        /// <param name="info"></param>
        /// <param name="app"></param>
        public void FillUI(IemMainPageInfo info, IEmrHost app)
        {
            try
            {
                m_App = app;
                m_IemInfo = info;

                //(new FillUIDelegate(FillUIInner)).BeginInvoke(null, null);
                FillUIInner();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        delegate void FillUIDelegate();

        private void FillUIInner()
        {
            try
            {
                GetHisLoad();

                IemMainPageInfo info = m_IemInfo;
                #region
                if (info.IemBasicInfo.Iem_Mainpage_NO == "")
                {
                    //to do 病患基本信息
                }
                else
                {
                    txtTotal.Text = m_IemInfo.IemFeeInfo.Total;
                    txtOwnFee.Text = m_IemInfo.IemFeeInfo.OwnFee;
                    txtYBYLFY.Text = m_IemInfo.IemFeeInfo.YBYLFY;
                    txtYBZLFY.Text = m_IemInfo.IemFeeInfo.YBZLFY;
                    txtCare.Text = m_IemInfo.IemFeeInfo.Care;

                    txtZHQTFY.Text = m_IemInfo.IemFeeInfo.ZHQTFY;
                    txtBLZDF.Text = m_IemInfo.IemFeeInfo.BLZDF;
                    txtSYSZDF.Text = m_IemInfo.IemFeeInfo.SYSZDF;
                    txtYXXZDF.Text = m_IemInfo.IemFeeInfo.YXXZDF;
                    txtLCZDF.Text = m_IemInfo.IemFeeInfo.LCZDF;

                    txtFSSZLF.Text = m_IemInfo.IemFeeInfo.FSSZLF;
                    txtLCWLZLF.Text = m_IemInfo.IemFeeInfo.LCWLZLF;
                    txtSSZLF.Text = m_IemInfo.IemFeeInfo.SSZLF;
                    txtMZF.Text = m_IemInfo.IemFeeInfo.MZF;
                    txtSSF.Text = m_IemInfo.IemFeeInfo.SSF;

                    txtKFF.Text = m_IemInfo.IemFeeInfo.KFF;
                    txtZYZLF.Text = m_IemInfo.IemFeeInfo.ZYZLF;
                    txtXYF.Text = m_IemInfo.IemFeeInfo.XYF;
                    txtKJYWF.Text = m_IemInfo.IemFeeInfo.KJYWF;
                    txtCPMedical.Text = m_IemInfo.IemFeeInfo.CPMedical;

                    txtCMedical.Text = m_IemInfo.IemFeeInfo.CMedical;
                    txtBloodFee.Text = m_IemInfo.IemFeeInfo.BloodFee;
                    txtXDBLZPF.Text = m_IemInfo.IemFeeInfo.XDBLZPF;
                    txtQDBLZPF.Text = m_IemInfo.IemFeeInfo.QDBLZPF;
                    txtNXYZLZPF.Text = m_IemInfo.IemFeeInfo.NXYZLZPF;

                    txtXBYZLZPF.Text = m_IemInfo.IemFeeInfo.XBYZLZPF;
                    txtJCYYCXCLF.Text = m_IemInfo.IemFeeInfo.JCYYCXCLF;
                    txtZLYYCXCLF.Text = m_IemInfo.IemFeeInfo.ZLYYCXCLF;
                    txtSSYYCXCLF.Text = m_IemInfo.IemFeeInfo.SSYYCXCLF;
                    txtOtherFee.Text = m_IemInfo.IemFeeInfo.OtherFee;

                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///判断是否从his中提取费用信息，以及其中的sql
        /// </summary>
        private void GetHisLoad()
        {
            try
            {
                DataHelper datahelper = new DataHelper();
                //病案首页中费用导入按钮是否显示
                string StrFeiyong = datahelper.GetConfigValueByKey("IEMIsLoadHisFeiYong");
                {
                    if (StrFeiyong == "1")
                    {
                        btnLoad.Visible = true;
                    }
                    else
                    {
                        btnLoad.Visible = false;
                    }
                }
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
                m_IemInfo.IemFeeInfo.Total = txtTotal.Text;
                m_IemInfo.IemFeeInfo.OwnFee = txtOwnFee.Text;
                m_IemInfo.IemFeeInfo.YBYLFY = txtYBYLFY.Text;
                m_IemInfo.IemFeeInfo.YBZLFY = txtYBZLFY.Text;
                m_IemInfo.IemFeeInfo.Care = txtCare.Text;

                m_IemInfo.IemFeeInfo.ZHQTFY = txtZHQTFY.Text;
                m_IemInfo.IemFeeInfo.BLZDF = txtBLZDF.Text;
                m_IemInfo.IemFeeInfo.SYSZDF = txtSYSZDF.Text;
                m_IemInfo.IemFeeInfo.YXXZDF = txtYXXZDF.Text;
                m_IemInfo.IemFeeInfo.LCZDF = txtLCZDF.Text;

                m_IemInfo.IemFeeInfo.FSSZLF = txtFSSZLF.Text;
                m_IemInfo.IemFeeInfo.LCWLZLF = txtLCWLZLF.Text;
                m_IemInfo.IemFeeInfo.SSZLF = txtSSZLF.Text;
                m_IemInfo.IemFeeInfo.MZF = txtMZF.Text;
                m_IemInfo.IemFeeInfo.SSF = txtSSF.Text;

                m_IemInfo.IemFeeInfo.KFF = txtKFF.Text;
                m_IemInfo.IemFeeInfo.ZYZLF = txtZYZLF.Text;
                m_IemInfo.IemFeeInfo.XYF = txtXYF.Text;
                m_IemInfo.IemFeeInfo.KJYWF = txtKJYWF.Text;
                m_IemInfo.IemFeeInfo.CPMedical = txtCPMedical.Text;

                m_IemInfo.IemFeeInfo.CMedical = txtCMedical.Text;
                m_IemInfo.IemFeeInfo.BloodFee = txtBloodFee.Text;
                m_IemInfo.IemFeeInfo.XDBLZPF = txtXDBLZPF.Text;
                m_IemInfo.IemFeeInfo.QDBLZPF = txtQDBLZPF.Text;
                m_IemInfo.IemFeeInfo.NXYZLZPF = txtNXYZLZPF.Text;

                m_IemInfo.IemFeeInfo.XBYZLZPF = txtXBYZLZPF.Text;
                m_IemInfo.IemFeeInfo.JCYYCXCLF = txtJCYYCXCLF.Text;
                m_IemInfo.IemFeeInfo.ZLYYCXCLF = txtZLYYCXCLF.Text;
                m_IemInfo.IemFeeInfo.SSYYCXCLF = txtSSYYCXCLF.Text;
                m_IemInfo.IemFeeInfo.OtherFee = txtOtherFee.Text;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region 窗体事件

        /// <summary>
        /// 确定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_OK_Click(object sender, EventArgs e)
        {
            try
            {
                //设置当前病人(修复m_App病人丢失问题)
                if (null == m_App || null == m_App.CurrentPatientInfo || m_App.CurrentPatientInfo.NoOfFirstPage.ToString() != m_IemInfo.IemBasicInfo.NoOfInpat)
                {
                    CurrentInpatient = DS_SqlService.GetPatientInfo(m_IemInfo.IemBasicInfo.NoOfInpat);
                }
                else
                {
                    CurrentInpatient = m_App.CurrentPatientInfo;
                }

                GetUI();
                //edit by 2012-12-20 张业兴 关闭弹出框只关闭提示框
                //((ShowUC)this.Parent).Close(true, m_IemInfo);
                //病案首页费用，确认后加到电子病历的表中 add by ywk 2012年10月16日 18:41:57
                //CurrentInpatient = m_App.CurrentPatientInfo;
                if (null != CurrentInpatient)
                {
                    CurrentInpatient.ReInitializeAllProperties();
                }
                IemMainPageManger manger = new IemMainPageManger(m_App, CurrentInpatient);
                manger.SaveData(m_IemInfo);

                //add by cyq 2012-12-05 病案室人员编辑后状态改为已归档
                if (editFlag)
                {
                    DS_BaseService.SetRecordsRebacked(int.Parse(CurrentInpatient.NoOfFirstPage.ToString().Trim()));
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            ((ShowUC)this.Parent).Close(false, null);
        }

        /// <summary>
        /// 提取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFee_Click(object sender, EventArgs e)
        {

            try
            {
                this.SuspendLayout();
                if (m_App == null || m_App.CurrentPatientInfo == null)
                {
                    return;
                }
                DataHelper datahelper = new DataHelper();
                string GetFeeType = datahelper.GetConfigValueByKey("GetHisMoneyType");
                IDataAccess sqlHelper = DataAccessFactory.GetSqlDataAccess("HISDB");

                if (sqlHelper == null)
                {
                    m_App.CustomMessageBox.MessageShow("无法连接到HIS！", CustomMessageBoxKind.ErrorOk);
                    return;
                }
                //to do  yxy 提取HIS数据库中病人费用信息


                //仁和医院需求 首页调取费用的方式 0是取视图 1是取存储过程
                DataRow dataRow = null;
                DataTable dataTable = new DataTable();
                DataTable Dt_FeeInfo = dataTable.Copy();
                if (GetFeeType == "0")
                {
                    //add by  ywk 2012年10月8日 12:33:30
                    string sqlView = string.Format(@"select * from ZC_IEMFEEINFO where 住院号='{0}'", m_App.CurrentPatientInfo.NoOfFirstPage);
                    dataTable = sqlHelper.ExecuteDataTable(sqlView, CommandType.Text);
                    if (dataTable == null || dataTable.Rows.Count <= 0) return;
                    Dt_FeeInfo = dataTable.Copy();//将所有的费用信息COPY下来，再进行筛选
                    dataRow = Dt_FeeInfo.Rows[0];
                }
                else if (GetFeeType == "1")//取HIS存储过程 
                {

                    //仁和医院的HIS是ＳＱＬＳＥＲＶＥＲ版本的  add by ywk 2012年12月17日14:15:45
                    using (SqlConnection conn = new SqlConnection(DataAccessFactory.GetSqlDataAccess("HISDB").GetDbConnection().ConnectionString))
                    {
                        if (conn.State != ConnectionState.Open)
                            conn.Open();

                        SqlCommand cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "pr_IEMFEEINFO";
                        //SqlD odap = new OracleDataAdapter(cmd);
                        cmd.Parameters.Add("zyh", OracleType.VarChar).Direction = ParameterDirection.Input;//患者住院流水号
                        cmd.Parameters["zyh"].Value = m_App.CurrentPatientInfo.NoOfHisFirstPage;
                        //cmd.ex(cmd.CommandText, cmd.Parameters["zyh"], CommandType.StoredProcedure);
                        SqlDataAdapter odap = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        odap.Fill(ds);
                        dataTable = ds.Tables[0];
                        dataRow = ds.Tables[0].Rows[0];
                    }
                }
                //DataRow dataRow=dataTable.Rows[0];
                //to do 赋值
                //to do 赋值
                if (dataTable.Columns.Contains("总费用"))
                {
                    txtTotal.Text = dataRow["总费用"].ToString();  //总费用
                }

                if (dataTable.Columns.Contains("自付金额"))
                {
                    txtOwnFee.Text = dataRow["自付金额"].ToString();  //自付金额
                }

                if (dataTable.Columns.Contains("一般医疗服务费"))
                {
                    txtYBYLFY.Text = dataRow["一般医疗服务费"].ToString();  //一般医疗服务费
                }

                if (dataTable.Columns.Contains("一般治疗操作费"))
                {
                    txtYBZLFY.Text = dataRow["一般治疗操作费"].ToString();  //一般治疗操作费
                }

                if (dataTable.Columns.Contains("护理费"))
                {
                    txtCare.Text = dataRow["护理费"].ToString();        //护理费
                }

                txtZHQTFY.Text = "-";        //其他费用
                if (dataTable.Columns.Contains("病理诊断费"))
                {
                    txtBLZDF.Text = dataRow["病理诊断费"].ToString(); ;        //病理诊断费####
                }
                if (dataTable.Columns.Contains("实验室诊断费"))
                {
                    txtSYSZDF.Text = dataRow["实验室诊断费"].ToString();        //实验室诊断费
                }
                if (dataTable.Columns.Contains("影像学诊断费"))
                {
                    txtYXXZDF.Text = dataRow["影像学诊断费"].ToString();        //影像学诊断费
                }
                if (dataTable.Columns.Contains("临床诊断"))
                {
                    txtLCZDF.Text = dataRow["临床诊断"].ToString();        //临床诊断项目费
                }
                txtFSSZLF.Text = "-";        //非手术治疗项目费
                txtLCWLZLF.Text = "-";        //临床物理治疗费

                float mzf = 0f;
                float ssf = 0f;
                if (dataTable.Columns.Contains("手术费"))
                {
                    float.TryParse(dataRow["手术费"].ToString(), out ssf);
                }
                if (dataTable.Columns.Contains("麻醉费"))
                {
                    float.TryParse(dataRow["麻醉费"].ToString(), out mzf);
                }

                txtSSZLF.Text = (mzf + ssf).ToString();        //手术治疗费 麻醉费+手术费

                if (dataTable.Columns.Contains("麻醉费"))
                {
                    txtMZF.Text = dataRow["麻醉费"].ToString();        //麻醉费
                }
                if (dataTable.Columns.Contains("手术费"))
                {
                    txtSSF.Text = dataRow["手术费"].ToString();        //手术费
                }

                if (dataTable.Columns.Contains("康复费"))
                {
                    txtKFF.Text = dataRow["康复费"].ToString();        //康复费####
                }
                if (dataTable.Columns.Contains("中医治疗费"))
                {
                    txtZYZLF.Text = dataRow["中医治疗费"].ToString();        //中医治疗费####
                }
                if (dataTable.Columns.Contains("西药费"))
                {
                    txtXYF.Text = dataRow["西药费"].ToString();        //西药费
                }
                txtKJYWF.Text = "-";        //抗菌药物费用
                if (dataTable.Columns.Contains("中成药费"))
                {
                    txtCPMedical.Text = dataRow["中成药费"].ToString();        //中成药费
                }
                if (dataTable.Columns.Contains("中草药费"))
                {
                    txtCMedical.Text = dataRow["中草药费"].ToString();        //中草药费
                }

                if (dataTable.Columns.Contains("输血费"))
                {
                    txtBloodFee.Text = dataRow["输血费"].ToString();        //血费
                }
                txtXDBLZPF.Text = "-";        //白蛋白类制品费
                txtQDBLZPF.Text = "-";        //球蛋白类制品费
                txtNXYZLZPF.Text = "-";        //凝血因子类制品费

                txtXBYZLZPF.Text = "-";        //细胞因子类制品费
                if (dataTable.Columns.Contains("医用材料费"))
                {
                    txtJCYYCXCLF.Text = dataRow["医用材料费"].ToString();        //检查用一次性医用材料费
                }

                txtZLYYCXCLF.Text = "-";        //治疗用一次性医用材料费

                if (dataTable.Columns.Contains("手术材料费"))
                {
                    txtSSYYCXCLF.Text = dataRow["手术材料费"].ToString();        //手术用一次性医用材料费
                }
                if (dataTable.Columns.Contains("其它"))
                {
                    txtOtherFee.Text = dataRow["其它"].ToString();        //其他费用
                }

            }
            catch (Exception ex)
            {
                MyMessageBox.Show(2, ex);
            }
        }


        /// <summary>
        /// Enter事件 --- 获取焦点选中内容
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-12-28</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_Enter(object sender, EventArgs e)
        {
            try
            {
                DS_Common.txt_Enter(sender);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #endregion
    }
}
