using DevExpress.XtraEditors;
using DrectSoft.Common.Eop;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DrectSoft.Core.IEMMainPage
{
    public partial class UCOthers : UserControl
    {
        private IemMainPageInfo m_IemInfo;
        private Inpatient CurrentInpatient;
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
        private void FillUIInner()
        {
            IemMainPageInfo info = m_IemInfo;
            #region
            if (info.IemBasicInfo.Iem_Mainpage_NO == "")
            {
                //to do 病患基本信息
            }
            else
            {
                txtTotal.Text = m_IemInfo.IemFeeInfo.Total.Trim('-');
                txtOwnFee.Text = m_IemInfo.IemFeeInfo.OwnFee.Trim('-');
                txtYBYLFY.Text = m_IemInfo.IemFeeInfo.YBYLFY.Trim('-');


                txtYBZLFY.Text = m_IemInfo.IemFeeInfo.YBZLFY.Trim('-');
                txtCare.Text = m_IemInfo.IemFeeInfo.Care.Trim('-');

                txtZHQTFY.Text = m_IemInfo.IemFeeInfo.ZHQTFY.Trim('-');
                txtBLZDF.Text = m_IemInfo.IemFeeInfo.BLZDF.Trim('-');
                txtSYSZDF.Text = m_IemInfo.IemFeeInfo.SYSZDF.Trim('-');
                txtYXXZDF.Text = m_IemInfo.IemFeeInfo.YXXZDF.Trim('-');
                txtLCZDF.Text = m_IemInfo.IemFeeInfo.LCZDF.Trim('-');

                txtFSSZLF.Text = m_IemInfo.IemFeeInfo.FSSZLF.Trim('-');
                txtLCWLZLF.Text = m_IemInfo.IemFeeInfo.LCWLZLF.Trim('-');
                txtSSZLF.Text = m_IemInfo.IemFeeInfo.SSZLF.Trim('-');
                txtMZF.Text = m_IemInfo.IemFeeInfo.MZF.Trim('-');
                txtSSF.Text = m_IemInfo.IemFeeInfo.SSF.Trim('-');

                txtKFF.Text = m_IemInfo.IemFeeInfo.KFF.Trim('-');


                txtXYF.Text = m_IemInfo.IemFeeInfo.XYF.Trim('-');
                txtKJYWF.Text = m_IemInfo.IemFeeInfo.KJYWF.Trim('-');
                txtCPMedical.Text = m_IemInfo.IemFeeInfo.CPMedical.Trim('-');

                txtCMedical.Text = m_IemInfo.IemFeeInfo.CMedical.Trim('-');
                txtBloodFee.Text = m_IemInfo.IemFeeInfo.BloodFee.Trim('-');
                txtXDBLZPF.Text = m_IemInfo.IemFeeInfo.XDBLZPF.Trim('-');
                txtQDBLZPF.Text = m_IemInfo.IemFeeInfo.QDBLZPF.Trim('-');
                txtNXYZLZPF.Text = m_IemInfo.IemFeeInfo.NXYZLZPF.Trim('-');

                txtXBYZLZPF.Text = m_IemInfo.IemFeeInfo.XBYZLZPF.Trim('-');
                txtJCYYCXCLF.Text = m_IemInfo.IemFeeInfo.JCYYCXCLF.Trim('-');
                txtZLYYCXCLF.Text = m_IemInfo.IemFeeInfo.ZLYYCXCLF.Trim('-');
                txtSSYYCXCLF.Text = m_IemInfo.IemFeeInfo.SSYYCXCLF.Trim('-');
                txtOtherFee.Text = m_IemInfo.IemFeeInfo.OtherFee.Trim('-');

            }
            #endregion
        }

        /// <summary>
        /// GET UI
        /// </summary>
        private void GetUI()
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

        #region 从HIS提取，建立连接写接口的
        /// <summary>
        /// 提取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFee_Click(object sender, EventArgs e)
        {
            return;

            /*this.SuspendLayout();
            if (m_App == null || m_App.CurrentPatientInfo == null)
                return;
            IDataAccess sqlHelper = DataAccessFactory.GetSqlDataAccess("HISDB");

            if (sqlHelper == null)
            {
                m_App.CustomMessageBox.MessageShow("无法连接到HIS！", CustomMessageBoxKind.ErrorOk);
                return;
            }
            //to do  yxy 提取HIS数据库中病人费用信息

            string sql = string.Format(@"SELECT     CONVERT(varchar(12), PatCode) AS PatID,FeeCode,
                                             CONVERT(varchar(12), FeeName) AS FeeName, CONVERT(float, SUM(Amount)) AS amount
                                            FROM  root.InnerRecipeCount WITH (nolock)
                                            where PatCode = '{0}'
                                            GROUP BY PatCode, FeeName,FeeCode", m_App.CurrentPatientInfo.NoOfHisFirstPage);//m_App.CurrentPatientInfo.NoOfHisFirstPage);
            //SqlParameter[] paraColl = new SqlParameter[] { new SqlParameter("@syxh", m_App.CurrentPatientInfo.NoOfHisFirstPage ) };
            //DataTable dataTable = sqlHelper.ExecuteDataTable("usp_bq_fymxcx", paraColl, CommandType.StoredProcedure);

            DataTable dataTable = sqlHelper.ExecuteDataTable(sql, CommandType.Text);
            //to do 赋值
            //to do 赋值
            Double totalFee = 0;
            Double OtherFee = 0;
            foreach (DataRow row in dataTable.Rows)
            {
                totalFee = totalFee + Convert.ToDouble(row["amount"].ToString());
                OtherFee = OtherFee + Convert.ToDouble(row["amount"].ToString());

                //    //床费
                //    if (row["FeeName"].ToString().Trim() == "床位费")
                //    {
                //        this.lblBed.Text = row["amount"].ToString();
                //        OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                //    }
                //    //护理费
                //    else if (row["FeeName"].ToString().Trim() == "护理费")
                //    {
                //        this.lblCare.Text = row["amount"].ToString();
                //        OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                //    }
                //    //西药费
                //    else if (row["FeeName"].ToString().Trim() == "西药费")
                //    {
                //        lblWMedical.Text = row["amount"].ToString();
                //        OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                //    }

                //    //中成药费
                //    else if (row["FeeName"].ToString().Trim() == "中成药费")
                //    {
                //        lblCPMedical.Text = row["amount"].ToString();
                //        OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                //    }
                //    //中草药费
                //    else if (row["FeeName"].ToString().Trim() == "草药费")
                //    {
                //        lblCMedical.Text = row["amount"].ToString();
                //        OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                //    }
                //    //放射费
                //    else if (row["FeeName"].ToString().Trim() == "放射费")
                //    {
                //        lblRadiate.Text = row["amount"].ToString();
                //        OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                //    }
                //    //检验
                //    else if (row["FeeName"].ToString().Trim() == "其它")
                //    {
                //        lblAssay.Text = row["amount"].ToString();
                //        OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                //    }
                //    //输氧费
                //    else if (row["FeeName"].ToString().Trim() == "输氧费")
                //    {
                //        lblOx.Text = row["amount"].ToString();
                //        OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                //    }

                //    //输血费
                //    else if (row["FeeName"].ToString().Trim() == "输血费")
                //    {
                //        lblBlood.Text = row["amount"].ToString();
                //        OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                //    }
                //    //诊查费
                //    else if (row["FeeName"].ToString().Trim() == "诊查费")
                //    {
                //        lblMecical.Text = row["amount"].ToString();
                //        OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                //    }
                //    //手术费
                //    else if (row["FeeName"].ToString().Trim() == "手术费")
                //    {
                //        lblOperation.Text = row["amount"].ToString();
                //        OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                //    }
                //    //接生费
                //    else if (row["FeeName"].ToString().Trim() == "接生费")
                //    {
                //        lblAccouche.Text = row["amount"].ToString();
                //        OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                //    }
                //    //检查费
                //    else if (row["FeeName"].ToString().Trim() == "检查费")
                //    {
                //        lblRis.Text = row["amount"].ToString();
                //        OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                //    }

                //    //麻醉费
                //    else if (row["FeeName"].ToString().Trim() == "麻醉费")
                //    {
                //        lblAnaesthesia.Text = row["amount"].ToString();
                //        OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                //    }
                //    //治疗费
                //    else if (row["FeeName"].ToString().Trim() == "治疗费")
                //    {
                //        labelZhiLiao.Text = row["amount"].ToString();
                //        OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                //    }
                //    //婴儿费   陪床费  药占比  检验费 未匹配


            }

            //this.lblTotal.Text = totalFee.ToString();

            //lblOthers.Text = OtherFee.ToString();

            this.Refresh();
            //if (this.FindForm() == null)
            //{
            //    this.Refresh();
            //}
            //else
            //    this.FindForm().Refresh();
            */
        }
        #endregion

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

        /// <summary>
        /// 确定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_OK_Click(object sender, EventArgs e)
        {
            try
            {
                GetUI();
                CurrentInpatient = m_App.CurrentPatientInfo;
                CurrentInpatient.ReInitializeAllProperties();
                IemMainPageManger manger = new IemMainPageManger(m_App, CurrentInpatient);
                manger.SaveData(m_IemInfo);
                btn_Close_Click(sender, e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            ((ShowUC)this.Parent).Close(false, null);
        }
    }
}
