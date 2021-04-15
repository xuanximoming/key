using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.IO;

namespace DrectSoft.Core.IEMMainPage
{
    public partial class ShowUC : DevExpress.XtraEditors.XtraForm
    {

        private IEmrHost m_app;

        public IemMainPageInfo m_info;

        public ShowUC(IEmrHost app)
        {
            InitializeComponent();
            m_app = app;
        }

        /// <summary>
        /// 显示中医基础信息页面
        /// </summary>
        /// <param name="info"></param>
        public void ShowUCIemBasInfo(UCIemBasInfo info, IemMainPageInfo m_pageInfo)
        {
            this.Text = "基础信息编辑";
            this.Controls.Clear();
            this.Controls.Add(info);

            info.FillUI(m_pageInfo, m_app);
        }

        /// <summary>
        /// 显示西医基础信息页面
        /// </summary>
        /// <param name="info"></param>
        public void ShowUCIemBasInfoEn(UCIemBasInfoEn info, IemMainPageInfo m_pageInfo)
        {
            this.Text = "基础信息编辑";
            this.Controls.Clear();
            this.Controls.Add(info);

            info.FillUI(m_pageInfo, m_app);
        }

        /// <summary>
        /// 显示中医诊断信息页面
        /// </summary>
        /// <param name="info"></param>
        public void ShowUCIemDiagnose(UCIemDiagnose info, IemMainPageInfo m_pageInfo)
        {
            this.Text = "诊断信息编辑";
            this.Controls.Clear();
            this.Controls.Add(info);

            info.FillUI(m_pageInfo, m_app);
        }

        /// <summary>
        /// 显示西医诊断信息页面
        /// </summary>
        /// <param name="info"></param>
        public void ShowUCIemDiagnoseEn(UCIemDiagnoseEn info, IemMainPageInfo m_pageInfo)
        {
            this.Text = "诊断信息编辑";
            this.Controls.Clear();
            this.Controls.Add(info);

            info.FillUI(m_pageInfo, m_app);
        }

        /// <summary>
        /// 显示手术信息页面
        /// </summary>
        /// <param name="info"></param>
        public void ShowUCIemOperInfo(UCIemOperInfo info, IemMainPageInfo m_pageInfo)
        {
            this.Text = "手术信息编辑";
            this.Controls.Clear();
            this.Controls.Add(info);

            info.FillUI(m_pageInfo, m_app);
        }

        /// <summary>
        /// 显示产妇婴儿信息页面
        /// </summary>
        /// <param name="info"></param>
        public void ShowUCObstetricsBaby(UCObstetricsBaby info, IemMainPageInfo m_pageInfo)
        {
            this.Text = "产妇婴儿信息编辑";
            this.Controls.Clear();
            this.Controls.Add(info);

            info.FillUI(m_pageInfo, m_app);
        }

        /// <summary>
        /// 显示其他信息页面
        /// </summary>
        /// <param name="info"></param>
        public void ShowUCOthers(UCOthers info, IemMainPageInfo m_pageInfo)
        {
            this.Text = "其他信息编辑";
            this.Controls.Clear();
            this.Controls.Add(info);

            info.FillUI(m_pageInfo, m_app);
        }

        /// <summary>
        /// 是否点击确认按钮关闭的
        /// </summary>
        /// <param name="is_OK"></param>
        public void Close(bool is_OK, IemMainPageInfo info)
        {
            if (is_OK)
            {
                m_info = info;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                try
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    //电子病历上传病案首页图片到其他系统
                    string encode = "";
                    string backencode = "";
                    string folder = AppDomain.CurrentDomain.BaseDirectory + "PrintImage\\";

                    //读取第一页
                    FileStream fs = File.OpenRead(folder + "1.jpg");
                    int filelength = 0;
                    filelength = (int)fs.Length;
                    Byte[] image = new Byte[filelength];
                    fs.Read(image, 0, filelength);
                    encode = Convert.ToBase64String(image);
                    fs.Close();

                    //读取第二页
                    FileStream fs1 = File.OpenRead(folder + "2.jpg");
                    int filelength1 = 0;
                    filelength1 = (int)fs1.Length;
                    Byte[] image1 = new Byte[filelength1];
                    fs1.Read(image1, 0, filelength1);
                    backencode = Convert.ToBase64String(image1);
                    fs1.Close();
                    string pra = "base64encode1=" + encode + "&base64encode2=" + backencode +
                                    "&ecardid=" + info.IemBasicInfo.CardNumber + "&mdrecno=" + info.IemBasicInfo.NOOFRECORD +
                                    "&medical=" + info.IemBasicInfo.NOOFRECORD + "&operatorname=" + m_app.User.DoctorName +
                                    "&operatorno=" + m_app.User.DoctorId + "&operatordatetime=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    string value = DrectSoft.DSSqlHelper.DS_SqlHelper.HttpPostData(DrectSoft.Service.DS_SqlService.GetConfigValueByKey("ServiceWanIp"), "HttpPostGetPdf", pra);

                }
                catch (Exception ex)
                {

                }
            }
            this.Close();
        }

        private void ShowUC_Load(object sender, EventArgs e)
        {
            if (this.Controls.Count < 1) return;

            this.Height = this.Controls[0].Height + 20;
            this.Width = this.Controls[0].Width + 10;
        }
    }
}