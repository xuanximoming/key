using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Convertmy = DrectSoft.Core.UtilsForExtension;
using DrectSoft.FrameWork.WinForm.Plugin;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using System.Xml;
using DrectSoft.Common.Eop;
using DrectSoft.Common;
using DrectSoft.Service;
using System.Data.OracleClient;

namespace DrectSoft.Core.IEMMainPage
{
    public partial class UCOthers : UserControl
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
        public bool editFlag = false;  //add by cyq 2012-12-06 病案室人员编辑首页(状态改为归档)
        public UCOthers()
        {
            InitializeComponent();
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
                throw new Exception(ex.Message);
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
                throw new Exception(ex.Message);
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
                throw new Exception(ex.Message);
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

        /// <summary>
        /// 提取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFee_Click(object sender, EventArgs e)
        {

            this.SuspendLayout();
            if (m_App == null || m_App.CurrentPatientInfo == null)
                return;
            IDataAccess sqlHelper = DataAccessFactory.GetSqlDataAccess("HISDB");

            if (sqlHelper == null)
            {
                m_App.CustomMessageBox.MessageShow("无法连接到HIS！", CustomMessageBoxKind.ErrorOk);
                return;
            }
            //to do  yxy 提取HIS数据库中病人费用信息

            #region 注释
            //            string sql = string.Format(@"SELECT     CONVERT(varchar(12), PatCode) AS PatID,FeeCode,
            //                                             CONVERT(varchar(12), FeeName) AS FeeName, CONVERT(float, SUM(Amount)) AS amount
            //                                            FROM  root.InnerRecipeCount WITH (nolock)
            //                                            where PatCode = '{0}'
            //                                            GROUP BY PatCode, FeeName,FeeCode", m_App.CurrentPatientInfo.NoOfHisFirstPage);
            //m_App.CurrentPatientInfo.NoOfHisFirstPage);
            //SqlParameter[] paraColl = new SqlParameter[] { new SqlParameter("@syxh", m_App.CurrentPatientInfo.NoOfHisFirstPage ) };
            //DataTable dataTable = sqlHelper.ExecuteDataTable("usp_bq_fymxcx", paraColl, CommandType.StoredProcedure);

            //将从HIS中提取费用的功能，用从视图中取到，视图名称为：“YD_IEMFEEINFO”
            //************add by ywk 2012年10月8日 12:27:17******************

            //            string hisSql = @"select 
            //                        a.zyh as 住院号 ,
            //                        sum(fy) as 总费用,
            //                        b.zfje as 自付金额,
            //                        sum(case fb when 'zl' then fy    --治疗费  
            //                              when 'YQ' then fy   --输氧费  
            //                              when 'ZZ' then fy   --诊疗费  
            //                              when 'JC' then fy   --检查费  
            //                              when 'LL' then fy  --理疗费  
            //                              when 'JG' then fy  --激光    
            //                              when 'WB' then fy  --微波治疗
            //                              when 'YC' then fy  --高压氧舱
            //                              when 'DJ' then fy  --基因检测
            //                              when 'PJ' then fy end ) as 一般治疗操作费,--眼科治疗
            //                        sum(case fb when 'YH' then fy    --婴护费  
            //                            when 'TH' then fy     --特护费  
            //                            when '1H' then fy    --护理收入
            //                            when 'YF' then fy end ) as 护理费,  --换药费  
            //                        sum (case fb when 'XY' then fy end) as 西药费,  --西药费
            //                        sum(case fb when 'ZC' then fy end) as 中成药费,  --中成药费
            //                        sum(case fb when 'ZA' then fy end) as 中草药费,  --中草药费
            //                        sum(case fb when 'SS' then fy     --手术费  
            //                            when 'JS' then fy     --接生费  
            //                            when 'MS' then fy    --门诊手术
            //                            when 'SP' then fy    --射频手术
            //                            when 'KS' then fy end ) as 手术费,  --科手术  
            //                        sum (case fb when 'MZ' then fy end ) as 麻醉费,  --麻醉费
            //                        sum(case fb when 'HY' then fy   --化验费  
            //                            when 'JF' then fy end ) as 实验室诊断费,  --检验费  
            //                        sum(case fb when 'XG' then fy  --X光      
            //                            when 'CT' then fy    --CT费    
            //                            when 'BC' then fy    --B超     
            //                            when 'CC' then fy    --彩超费  
            //                            when 'LD' then fy    --脑电图  
            //                            when 'TS' then fy     --透视费  
            //                            when 'NT' then fy    --乳透    
            //                            when '60' then fy    --胸B超   
            //                            when 'HC' then fy     --磁共振  
            //                            when 'CB' then fy    --C臂介入 
            //                            when 'MB' then fy    --乳腺钼靶
            //                            when 'KB' then fy end ) as  影像学诊断费,  --科B超   
            //                        sum (case fb when 'SX' then fy end ) as 输血费,  --输血费
            //                        sum (case fb when 'WJ' then fy  --电子胃镜
            //                            when 'XD' then fy    --心电图  
            //                            when 'CX' then fy    --长心电  
            //                            when 'DT' then fy    --心动图  
            //                            when 'XJ' then fy    --心电监护
            //                            when 'KD' then fy    --科心电图
            //                            when 'PD' then fy end) as 临床诊断,    --潘生心电
            //                        sum(case fb when 'QT'then fy     --其它
            //                            when 'QL' then fy    --取暖费  
            //                            when 'JW' then fy end ) as 其它,  --降温费  
            //                        sum(case fb when 'WC' then fy end ) as 医用材料费,--医用材料费
            //                        sum (case fb when 'SB' then fy end) as 手术材料费  --手术材料费
            //                        from zybl a left join sfjl b on a.zyh=b.zyh where a.zyh='{0}' group by a.zyh;";
            //string sql = string.Format(hisSql, m_App.CurrentPatientInfo.NoOfHisFirstPage);
            //DataTable dataTable = sqlHelper.ExecuteDataTable(sql, CommandType.Text);
            //if (dataTable == null || dataTable.Rows.Count <= 0) return;
            #endregion
            //仁和医院需求 首页调取费用的方式 0是取视图 1是取存储过程
            DataHelper datahelper = new DataHelper();
            string GetFeeType = datahelper.GetConfigValueByKey("GetHisMoneyType");
            DataRow dataRow = null;
            DataTable dataTable = new DataTable();
            DataTable Dt_FeeInfo = dataTable.Copy();
            if (GetFeeType == "0")
            {
                //add by  ywk 2012年10月8日 12:33:30
                //二次更改  add  by ywk 2013年4月24日13:30:36  
                //string sqlView = "select * from YD_IEMFEEINFO";
                string sqlView = string.Format("select * from YD_IEMFEEINFO where 住院号='{0}'", m_App.CurrentPatientInfo.NoOfHisFirstPage);
                dataTable = sqlHelper.ExecuteDataTable(sqlView, CommandType.Text);
                //if (dataTable == null || dataTable.Rows.Count <= 0) return;
                dataTable = sqlHelper.ExecuteDataTable(sqlView, CommandType.Text);
                if (dataTable == null || dataTable.Rows == null || dataTable.Rows.Count <= 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("未提取到该病人的费用信息");
                    return;
                }
                //Dt_FeeInfo = dataTable.Copy();//将所有的费用信息COPY下来，再进行筛选
                //DataRow[] Dr_FeeInfo = Dt_FeeInfo.Select(string.Format("住院号='{0}'", m_App.CurrentPatientInfo.NoOfHisFirstPage));//进行筛选出
                //if (Dr_FeeInfo.Length == 0)
                //{
                //    return;
                //}
                //dataRow = dataTable.Rows[0];
                //视图的也给予填写栏位add  by ywk 2013年6月3日 08:47:12
                if (dataTable.Rows.Count > 0)
                {
                    dataRow = dataTable.Rows[0];
                    FillFeeInfo(dataTable, dataRow);
                }
                else
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("视图中未提取到该病人的费用信息");
                    return;
                }

                ////""的暂时用0.00代替
                //dataRow = Dr_FeeInfo[0];
            }
            if (GetFeeType == "1")//取HIS存储过程 
            {
                try
                {
                    //应该根据HIS是SQL还是Oracle的进行分别实现add by ywk 二〇一三年五月二十九日 11:02:44 
                    if (sqlHelper.GetDbConnection().ConnectionString.IndexOf("Data Source") >= 0)//Oracle的
                    {
                        DrectSoft.DSSqlHelper.DS_SqlHelper.CreateSqlHelperByDBName("HISDB");
                        //SqlHelper.DS_SqlHelper.ConnectionString = DataAccessFactory.GetSqlDataAccess("HISDB").GetDbConnection().ConnectionString;
                        DrectSoft.DSSqlHelper.DS_SqlHelper.BeginTransaction();
                        //MessageBox.Show("执行到了ConnectionString" + SqlHelper.DS_SqlHelper.ConnectionString);
                        //中心医院无法从表中直接查询，为计算得到费用取得改为输出参数ywk 二〇一三年五月三十日 13:12:44 
                        SqlParameter[] sqlParamhis = new SqlParameter[] 
                            { 
                            new SqlParameter("zyh", SqlDbType.VarChar),
                            new SqlParameter("par_total",SqlDbType.VarChar,100),//总费用
                            new SqlParameter("par_paycost",SqlDbType.VarChar,100),// 自付金额
                            new SqlParameter("par_ybylfwf",SqlDbType.VarChar,100),//一般医疗服务费
                            new SqlParameter("par_ybzlczf",SqlDbType.VarChar,100),//一般治疗操作费,
                            new SqlParameter("par_hlf",SqlDbType.VarChar,100),//护理费,
                            new SqlParameter("par_zhfwlqtf",SqlDbType.VarChar,100),// 其他费用
                            new SqlParameter("par_blzdf",SqlDbType.VarChar,100),//病理诊断费
                            new SqlParameter("par_syszdf",SqlDbType.VarChar,100),//实验室诊断费,
                            new SqlParameter("par_yxxzdf",SqlDbType.VarChar,100),//影像学诊断费
                            new SqlParameter("par_lczdxmf",SqlDbType.VarChar,100),//临床诊断
                            new SqlParameter("par_fsszlxmf",SqlDbType.VarChar,100),//非手术治疗费
                            new SqlParameter("par_lcwlzlf",SqlDbType.VarChar,100),//临床物理治疗费,
                            new SqlParameter("par_sszlf",SqlDbType.VarChar,100),// 手术治疗费
                            new SqlParameter("par_mzf",SqlDbType.VarChar,100),//麻醉费
                            new SqlParameter("par_ssf",SqlDbType.VarChar,100),//手术费
                            new SqlParameter("par_kff",SqlDbType.VarChar,100),//康复费,
                            new SqlParameter("par_zyzlf",SqlDbType.VarChar,100),// 中医治疗费
                            new SqlParameter("par_xyf",SqlDbType.VarChar,100),// 西药费
                            new SqlParameter("par_kjywfy",SqlDbType.VarChar,100),// 抗菌药物费用
                            new SqlParameter("par_cyf",SqlDbType.VarChar,100),// 中成药费
                            new SqlParameter("par_zcyf",SqlDbType.VarChar,100),// 中草药费
                            new SqlParameter("par_xf",SqlDbType.VarChar,100),// 输血费
                            new SqlParameter("par_bdblzpf",SqlDbType.VarChar,100),// 血蛋白类制品费
                            new SqlParameter("par_qdblzpf",SqlDbType.VarChar,100),// 球蛋白类制品费
                            new SqlParameter("par_nxyzlzpf",SqlDbType.VarChar,100),// 凝血因子类制品费
                            new SqlParameter("par_xbyzlzpf",SqlDbType.VarChar,100),// 细胞因子类制品费
                            new SqlParameter("par_jcyycxcl",SqlDbType.VarChar,100),// 医用材料费,
                            new SqlParameter("par_zlyycxcl",SqlDbType.VarChar,100),// 治疗用一次性医用材料费,
                            new SqlParameter("par_ssyycxcl",SqlDbType.VarChar,100),// 手术材料费
                            new SqlParameter("par_qt",SqlDbType.VarChar,100)// 其他类
                            };

                        sqlParamhis[0].Direction = ParameterDirection.Input;
                        //sqlParamhis[1].Size = 100;
                        sqlParamhis[1].Direction = ParameterDirection.Output;
                        sqlParamhis[2].Direction = ParameterDirection.Output;
                        sqlParamhis[3].Direction = ParameterDirection.Output;
                        sqlParamhis[4].Direction = ParameterDirection.Output;
                        sqlParamhis[5].Direction = ParameterDirection.Output;
                        sqlParamhis[6].Direction = ParameterDirection.Output;
                        sqlParamhis[7].Direction = ParameterDirection.Output;
                        sqlParamhis[8].Direction = ParameterDirection.Output;
                        sqlParamhis[9].Direction = ParameterDirection.Output;
                        sqlParamhis[10].Direction = ParameterDirection.Output;
                        sqlParamhis[11].Direction = ParameterDirection.Output;
                        sqlParamhis[12].Direction = ParameterDirection.Output;
                        sqlParamhis[13].Direction = ParameterDirection.Output;
                        sqlParamhis[14].Direction = ParameterDirection.Output;
                        sqlParamhis[15].Direction = ParameterDirection.Output;
                        sqlParamhis[16].Direction = ParameterDirection.Output;
                        sqlParamhis[17].Direction = ParameterDirection.Output;
                        sqlParamhis[18].Direction = ParameterDirection.Output;
                        sqlParamhis[19].Direction = ParameterDirection.Output;
                        sqlParamhis[20].Direction = ParameterDirection.Output;
                        sqlParamhis[21].Direction = ParameterDirection.Output;
                        sqlParamhis[22].Direction = ParameterDirection.Output;
                        sqlParamhis[23].Direction = ParameterDirection.Output;
                        sqlParamhis[24].Direction = ParameterDirection.Output;
                        sqlParamhis[25].Direction = ParameterDirection.Output;
                        sqlParamhis[26].Direction = ParameterDirection.Output;
                        sqlParamhis[27].Direction = ParameterDirection.Output;
                        sqlParamhis[28].Direction = ParameterDirection.Output;
                        sqlParamhis[29].Direction = ParameterDirection.Output;
                        sqlParamhis[30].Direction = ParameterDirection.Output;
                        //for (int i = 1; i < 31; i++)
                        //{
                        //    sqlParamhis[i].Size = 100;
                        //}

                        sqlParamhis[0].Value = m_App.CurrentPatientInfo.NoOfHisFirstPage;
                        //MessageBox.Show("第一个参数是" + sqlParam[0].Value);
                        try
                        {
                            //SqlHelper.DS_SqlHelper.ExecuteDataTable("select 1 from dual", CommandType.Text);
                            //MessageBox.Show("成功连接到HIS数据库");
                            DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteNonQueryInTran("pr_IEMFEEINFO", sqlParamhis, CommandType.StoredProcedure);
                            //MessageBox.Show("已经执行ExecuteNonQueryInTran");
                            //没计算出来就给0
                            txtTotal.Text = sqlParamhis[1].Value.ToString() == "" ? "0" : sqlParamhis[1].Value.ToString();
                            //MessageBox.Show("总费用是" + sqlParamhis[1].Value.ToString());
                            txtOwnFee.Text = sqlParamhis[2].Value.ToString() == "" ? "0" : sqlParamhis[2].Value.ToString();
                            txtYBYLFY.Text = sqlParamhis[3].Value.ToString() == "" ? "0" : sqlParamhis[3].Value.ToString();
                            txtYBZLFY.Text = sqlParamhis[4].Value.ToString() == "" ? "0" : sqlParamhis[4].Value.ToString();
                            txtCare.Text = sqlParamhis[5].Value.ToString() == "" ? "0" : sqlParamhis[5].Value.ToString();
                            txtZHQTFY.Text = sqlParamhis[6].Value.ToString() == "" ? "0" : sqlParamhis[6].Value.ToString();
                            txtBLZDF.Text = sqlParamhis[7].Value.ToString() == "" ? "0" : sqlParamhis[7].Value.ToString();
                            txtSYSZDF.Text = sqlParamhis[8].Value.ToString() == "" ? "0" : sqlParamhis[8].Value.ToString();
                            txtYXXZDF.Text = sqlParamhis[9].Value.ToString() == "" ? "0" : sqlParamhis[9].Value.ToString();
                            txtLCZDF.Text = sqlParamhis[10].Value.ToString() == "" ? "0" : sqlParamhis[10].Value.ToString();
                            txtFSSZLF.Text = sqlParamhis[11].Value.ToString() == "" ? "0" : sqlParamhis[11].Value.ToString();
                            txtLCWLZLF.Text = sqlParamhis[12].Value.ToString() == "" ? "0" : sqlParamhis[12].Value.ToString();
                            txtSSZLF.Text = sqlParamhis[13].Value.ToString() == "" ? "0" : sqlParamhis[13].Value.ToString();
                            txtMZF.Text = sqlParamhis[14].Value.ToString() == "" ? "0" : sqlParamhis[14].Value.ToString();
                            txtSSF.Text = sqlParamhis[15].Value.ToString() == "" ? "0" : sqlParamhis[15].Value.ToString();
                            txtKFF.Text = sqlParamhis[16].Value.ToString() == "" ? "0" : sqlParamhis[16].Value.ToString();
                            txtZYZLF.Text = sqlParamhis[17].Value.ToString() == "" ? "0" : sqlParamhis[17].Value.ToString();
                            txtXYF.Text = sqlParamhis[18].Value.ToString() == "" ? "0" : sqlParamhis[18].Value.ToString();
                            txtKJYWF.Text = sqlParamhis[19].Value.ToString() == "" ? "0" : sqlParamhis[19].Value.ToString();
                            txtCPMedical.Text = sqlParamhis[20].Value.ToString() == "" ? "0" : sqlParamhis[20].Value.ToString();
                            txtCMedical.Text = sqlParamhis[21].Value.ToString() == "" ? "0" : sqlParamhis[21].Value.ToString();
                            txtBloodFee.Text = sqlParamhis[22].Value.ToString() == "" ? "0" : sqlParamhis[22].Value.ToString();
                            txtXDBLZPF.Text = sqlParamhis[23].Value.ToString() == "" ? "0" : sqlParamhis[23].Value.ToString();
                            txtQDBLZPF.Text = sqlParamhis[24].Value.ToString() == "" ? "0" : sqlParamhis[24].Value.ToString();
                            txtNXYZLZPF.Text = sqlParamhis[25].Value.ToString() == "" ? "0" : sqlParamhis[25].Value.ToString();
                            txtXBYZLZPF.Text = sqlParamhis[26].Value.ToString() == "" ? "0" : sqlParamhis[26].Value.ToString();
                            txtJCYYCXCLF.Text = sqlParamhis[27].Value.ToString() == "" ? "0" : sqlParamhis[27].Value.ToString();
                            txtZLYYCXCLF.Text = sqlParamhis[28].Value.ToString() == "" ? "0" : sqlParamhis[28].Value.ToString();
                            txtSSYYCXCLF.Text = sqlParamhis[29].Value.ToString() == "" ? "0" : sqlParamhis[29].Value.ToString();
                            txtOtherFee.Text = sqlParamhis[30].Value.ToString() == "" ? "0" : sqlParamhis[30].Value.ToString();
                            DrectSoft.DSSqlHelper.DS_SqlHelper.CommitTransaction();
                        }
                        catch (Exception ex)
                        {
                            m_App.CustomMessageBox.MessageShow("调用HIS的存储过程报错信息为： " + ex.Message);
                        }

                    }
                    else if (sqlHelper.GetDbConnection().ConnectionString.IndexOf("Database") >= 0)//SQL的
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
                            //edit by ywk 2013年4月27日10:30:14 
                            dataTable = ds.Tables[0];//这个应该赋值！！！！！！！！！！！add by ywk 2013年5月6日16:07:17 

                            if (dataTable.Rows.Count > 0)
                            {
                                dataRow = dataTable.Rows[0];
                                FillFeeInfo(dataTable, dataRow);
                            }
                            else
                            {
                                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("存储过程中未提取到该病人的费用信息");
                                return;
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    m_App.CustomMessageBox.MessageShow("调用HIS的存储过程报错信息为： " + ex.Message);
                    //throw;
                }
            }
            //if (this.FindForm() == null)
            //{
            //    this.Refresh();
            //}
            //else
            //    this.FindForm().Refresh();


        }
        /// <summary>
        /// 将查出的数据填充到费用的栏位
        /// add by ywk 2013年5月29日 17:13:37
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="dataRow"></param>
        private void FillFeeInfo(DataTable dataTable, DataRow dataRow)
        {
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
            if ((dataTable.Columns.Contains("其他费用")))//add by ywk 二〇一三年五月二十九日 17:08:08
            {
                txtZHQTFY.Text = dataRow["其他费用"].ToString();   //如果有就给值 
            }
            else
            {
                txtZHQTFY.Text = "-";        //其他费用
            }
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

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
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
        }

        private void UCOthers_Paint(object sender, PaintEventArgs e)
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
            //e.Graphics.DrawLine(Pens.Black, new Point(0, this.Height - 1), new Point(this.Width, this.Height - 1));
        }
        private Inpatient CurrentInpatient;//add by ywk 

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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            ((ShowUC)this.Parent).Close(false, null);
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
    }
}
