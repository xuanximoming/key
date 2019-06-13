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
            m_App = app;
            m_IemInfo = info;

            //(new FillUIDelegate(FillUIInner)).BeginInvoke(null, null);
            FillUIInner();
        }

        delegate void FillUIDelegate();
        /// <summary>
        /// 获取数据库里的值 并赋给界面上的控件  edit by 王冀 2012 12 4 
        /// </summary>
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
                    //新增诊断抢救临床路径管理等  王冀 2012 12 4
                    switch (m_IemInfo.IemOthers.Main_Diagnosis_Curecondition)
                    {
                        case "1":
                            chkzg_flag1.Checked = true;
                            break;
                        case "2":
                            chkzg_flag2.Checked = true;
                            break;
                        case "3":
                            chkzg_flag3.Checked = true;
                            break;
                        case "4":
                            chkzg_flag4.Checked = true;
                            break;
                        case "5":
                            chkzg_flag5.Checked = true;
                            break;
                        default: break;
                    }
                    textEdit3.Text = m_IemInfo.IemOthers.Diagnosis_conditions1;
                    textEdit4.Text = m_IemInfo.IemOthers.Diagnosis_conditions2;
                    textEdit5.Text = m_IemInfo.IemOthers.Diagnosis_conditions3;
                    textEdit6.Text = m_IemInfo.IemOthers.Diagnosis_conditions4;
                    textEdit7.Text = m_IemInfo.IemOthers.Diagnosis_conditions5;
                    textEdit1.Text = m_IemInfo.IemOthers.Emergency_times;
                    textEdit2.Text = m_IemInfo.IemOthers.Emergency_Successful_times;
                    switch (m_IemInfo.IemOthers.CP_status)
                    {
                        case "1":
                            chkPath1.Checked = true;
                            break;
                        case "2":
                            chkPath2.Checked = true;
                            break;
                        case "3":
                            chkPath3.Checked = true;
                            break;
                        case "4":
                            chkPath4.Checked = true;
                            break;
                        default: break;
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }

                #endregion
        }

        /// <summary>
        ///判断是否从his中提取费用信息，以及其中的sql
        /// </summary>
        private void GetHisLoad()
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

        /// <summary>
        /// GET UI edit by 王冀 2012 12 4
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

                //给新增的诊断 抢救 临床路径等 赋值 王冀 2012 12 4
                if (chkzg_flag1.Checked == true)
                {
                    m_IemInfo.IemOthers.Main_Diagnosis_Curecondition = "1";
                }
                if (chkzg_flag2.Checked == true)
                {
                    m_IemInfo.IemOthers.Main_Diagnosis_Curecondition = "2";
                }
                if (chkzg_flag3.Checked == true)
                {
                    m_IemInfo.IemOthers.Main_Diagnosis_Curecondition = "3";
                }
                if (chkzg_flag4.Checked == true)
                {
                    m_IemInfo.IemOthers.Main_Diagnosis_Curecondition = "4";
                }
                if (chkzg_flag5.Checked == true)
                {
                    m_IemInfo.IemOthers.Main_Diagnosis_Curecondition = "5";
                }

                m_IemInfo.IemOthers.Diagnosis_conditions1 = textEdit3.Text;
                m_IemInfo.IemOthers.Diagnosis_conditions2 = textEdit4.Text;
                m_IemInfo.IemOthers.Diagnosis_conditions3 = textEdit5.Text;
                m_IemInfo.IemOthers.Diagnosis_conditions4 = textEdit6.Text;
                m_IemInfo.IemOthers.Diagnosis_conditions5 = textEdit7.Text;
                m_IemInfo.IemOthers.Emergency_times = textEdit1.Text;
                m_IemInfo.IemOthers.Emergency_Successful_times = textEdit2.Text;
                m_IemInfo.IemOthers.Creat_user = m_App.User.DoctorId;

                if (chkPath1.Checked == true)
                {
                    m_IemInfo.IemOthers.CP_status = "1";
                }
                if (chkPath2.Checked == true)
                {
                    m_IemInfo.IemOthers.CP_status = "2";
                }
                if (chkPath3.Checked == true)
                {
                    m_IemInfo.IemOthers.CP_status = "3";
                }
                if (chkPath4.Checked == true)
                {
                    m_IemInfo.IemOthers.CP_status = "4";
                }
            }
            catch (Exception)
            {
                throw;
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

            //add by  ywk 2012年10月8日 12:33:30
            string sqlView = "select * from YD_IEMFEEINFO";
            DataTable dataTable = sqlHelper.ExecuteDataTable(sqlView, CommandType.Text);
            if (dataTable == null || dataTable.Rows.Count <= 0) return;
            DataTable Dt_FeeInfo = dataTable.Copy();//将所有的费用信息COPY下来，再进行筛选
            DataRow[] Dr_FeeInfo = Dt_FeeInfo.Select(string.Format("住院号='{0}'", m_App.CurrentPatientInfo.NoOfHisFirstPage));//进行筛选出
            if (Dr_FeeInfo.Length == 0)
            {
                return;
            }

            //""的暂时用0.00代替
            DataRow dataRow = Dr_FeeInfo[0];
            //DataRow dataRow=dataTable.Rows[0];
            //to do 赋值
            //to do 赋值
            txtTotal.Text = dataRow["总费用"].ToString();  //总费用
            txtOwnFee.Text = dataRow["自付金额"].ToString();  //自付金额
            txtYBYLFY.Text = dataRow["一般医疗服务费"].ToString();  //一般医疗服务费
            txtYBZLFY.Text = dataRow["一般治疗操作费"].ToString();  //一般治疗操作费
            txtCare.Text = dataRow["护理费"].ToString();        //护理费

            txtZHQTFY.Text = "0.00";        //其他费用
            txtBLZDF.Text = dataRow["病理诊断费"].ToString(); ;        //病理诊断费####
            txtSYSZDF.Text = dataRow["实验室诊断费"].ToString();        //实验室诊断费
            txtYXXZDF.Text = dataRow["影像学诊断费"].ToString();        //影像学诊断费
            txtLCZDF.Text = dataRow["临床诊断"].ToString();        //临床诊断项目费

            txtFSSZLF.Text = "0.00";        //非手术治疗项目费
            txtLCWLZLF.Text = "0.00";        //临床物理治疗费

            float mzf = 0f;
            float ssf = 0f;
            float.TryParse(dataRow["手术费"].ToString(), out ssf);
            float.TryParse(dataRow["麻醉费"].ToString(), out mzf);

            txtSSZLF.Text = (mzf + ssf).ToString();        //手术治疗费 麻醉费+手术费

            txtMZF.Text = dataRow["麻醉费"].ToString();        //麻醉费
            txtSSF.Text = dataRow["手术费"].ToString();        //手术费

            txtKFF.Text = dataRow["康复费"].ToString();        //康复费####
            txtZYZLF.Text = dataRow["中医治疗费"].ToString();        //中医治疗费####
            txtXYF.Text = dataRow["西药费"].ToString();        //西药费
            txtKJYWF.Text = "0.00";        //抗菌药物费用
            txtCPMedical.Text = dataRow["中成药费"].ToString();        //中成药费

            txtCMedical.Text = dataRow["中草药费"].ToString();        //中草药费
            txtBloodFee.Text = dataRow["输血费"].ToString();        //血费
            txtXDBLZPF.Text = "0.00";        //白蛋白类制品费
            txtQDBLZPF.Text = "0.00";        //球蛋白类制品费
            txtNXYZLZPF.Text = "0.00";        //凝血因子类制品费

            txtXBYZLZPF.Text = "0.00";        //细胞因子类制品费
            txtJCYYCXCLF.Text = dataRow["医用材料费"].ToString();        //检查用一次性医用材料费
            txtZLYYCXCLF.Text = "0.00";        //治疗用一次性医用材料费
            txtSSYYCXCLF.Text = dataRow["手术材料费"].ToString();        //手术用一次性医用材料费
            txtOtherFee.Text = dataRow["其它"].ToString();        //其他费用


            //if (this.FindForm() == null)
            //{
            //    this.Refresh();
            //}
            //else
            //    this.FindForm().Refresh();


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
        private void btn_OK_Click(object sender, EventArgs e)
        {
            GetUI();
            ((ShowUC)this.Parent).Close(true, m_IemInfo);
            //病案首页费用，确认后加到电子病历的表中 add by ywk 2012年10月16日 18:41:57
            CurrentInpatient = m_App.CurrentPatientInfo;
            CurrentInpatient.ReInitializeAllProperties();
            IemMainPageManger manger = new IemMainPageManger(m_App, CurrentInpatient);
            manger.SaveData(m_IemInfo);
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            ((ShowUC)this.Parent).Close(false, null);
        }
    }
}
