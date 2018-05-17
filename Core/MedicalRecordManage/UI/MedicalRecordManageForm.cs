using MedicalRecordManage.Object;
using MedicalRecordManage.UCControl;
using System;
using System.Data;
using System.Windows.Forms;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace MedicalRecordManage.UI
{
    public partial class MedicalRecordManageForm : DevBaseForm, IStartPlugIn//, IEMREditor
    {
        IEmrHost m_app;

        public MedicalRecordManageForm(IEmrHost app)
        {

            InitializeComponent();
            m_app = app;
            //    try
            //    {
            //        MedicalRecordApprove medicalRecordApprove = new MedicalRecordApprove(app);
            //        medicalRecordApprove.Dock = DockStyle.Fill;
            //        this.tabControl.TabPages[0].Controls.Add(medicalRecordApprove);

            //        MedicalRecordApprovedList medicalRecordApprovedList = new MedicalRecordApprovedList(app);
            //        medicalRecordApprovedList.Dock = DockStyle.Fill;
            //        this.tabControl.TabPages[1].Controls.Add(medicalRecordApprovedList);

            //        MedicalRecordList medicalRecordList = new MedicalRecordList(app);
            //        medicalRecordList.Dock = DockStyle.Fill;
            //        this.tabControl.TabPages[2].Controls.Add(medicalRecordList);
            //        /*
            //        MedicalRecordCfg medicalRecordCfg = new MedicalRecordCfg();
            //        medicalRecordCfg.Dock = DockStyle.Fill;
            //        this.tabControl.TabPages[3].Controls.Add(medicalRecordCfg);
            //         * */

            //    }
            //    catch (Exception ex)
            //    {
            //        Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            //    }
        }


        public MedicalRecordManageForm()
        {
            InitializeComponent();





            /*
            DrectSoft.Core.RecordManage.UCControl.UCRecordNoOnFile ucRecordNoOnFile = new DrectSoft.Core.RecordManage.UCControl.UCRecordNoOnFile();
            ucRecordNoOnFile.Dock = DockStyle.Fill;
            this.tabControl.TabPages[3].Controls.Add(ucRecordNoOnFile);
            */



            //this.tabControl.TabPages[3].Controls.Add(userControlUn);                 
            /*
            DrectSoft.Core.RecordManage.UCControl.UCRecordOnFile ucRecordOnFile = new DrectSoft.Core.RecordManage.UCControl.UCRecordOnFile();
            ucRecordOnFile.Dock = DockStyle.Fill;
            this.tabControl.TabPages[4].Controls.Add(ucRecordOnFile);
            */




            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public IPlugIn Run(IEmrHost host)
        {
            SqlUtil.App = host;
            m_app = host;
            PlugIn plg = new PlugIn(this.GetType().ToString(), this);
            return plg;
        }

        private void tabControl_SelectedPageChanging(object sender, DevExpress.XtraTab.TabPageChangingEventArgs e)
        {
            try
            {
                tabControl.SelectedTabPage.Focus();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void MedicalRecordManageForm_Load(object sender, EventArgs e)
        {
            tabControl.SelectedTabPage.Focus();

            //病历借阅审核界面
            MedicalRecordApprove medicalRecordApprove = new MedicalRecordApprove();
            medicalRecordApprove.Dock = DockStyle.Fill;
            //this.tabControl.TabPages[0].Controls.Add(medicalRecordApprove);
            this.tabPageApprove.Controls.Add(medicalRecordApprove);

            //病历借阅记录查询界面
            MedicalRecordApprovedList medicalRecordApprovedList = new MedicalRecordApprovedList();
            medicalRecordApprovedList.Dock = DockStyle.Fill;
            //this.tabControl.TabPages[1].Controls.Add(medicalRecordApprovedList);
            this.tabPageApprovedList.Controls.Add(medicalRecordApprovedList);
            //病历补写审核界面
            MedicalRecordWriteUpApprove medicalRecordwriteApprove = new MedicalRecordWriteUpApprove();
            medicalRecordwriteApprove.Dock = DockStyle.Fill;
            //this.tabControl.TabPages[0].Controls.Add(medicalRecordApprove);
            this.tabPageWriteUpApprove.Controls.Add(medicalRecordwriteApprove);

            //病历补写记录查询界面
            MedicalRecordWriteUpApprovedList medicalRecordWriteUpApprovedList = new MedicalRecordWriteUpApprovedList();
            medicalRecordWriteUpApprovedList.Dock = DockStyle.Fill;
            //this.tabControl.TabPages[1].Controls.Add(medicalRecordApprovedList);
            this.tabPageWriteUpApproveList.Controls.Add(medicalRecordWriteUpApprovedList);

            //病历查询界面
            MedicalRecordList medicalRecordList = new MedicalRecordList();
            medicalRecordList.Dock = DockStyle.Fill;
            //this.tabControl.TabPages[2].Controls.Add(medicalRecordList);
            this.tablePageRecord.Controls.Add(medicalRecordList);


            //病案首页编辑归档操作界面add by ywk 2013年7月30日 10:40:19
            MedIemArchive mediem = new MedIemArchive(m_app);
            mediem.Dock = DockStyle.Fill;
            this.tabTabIemAcrive.Controls.Add(mediem);

            string[] valueStr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("IsShowBinAn").Split(new char[] { '|' });
            if (valueStr.Length == 3)
            {

                if (valueStr[0] == "1")
                {
                    //未归档界面
                    MedicalRecordUnArchive userControlUn = new MedicalRecordUnArchive();
                    userControlUn.Dock = DockStyle.Fill;
                    tablePageUnRec.Controls.Add(userControlUn);
                }
                else
                {

                    this.tablePageUnRec.PageVisible = false;
                }
                if (valueStr[1] == "1")
                {
                    MedicalRecordArchive userControl = new MedicalRecordArchive();
                    userControl.Dock = DockStyle.Fill;
                    //this.tabControl.TabPages[4].Controls.Add(userControl); 
                    tablePageRecChecked.Controls.Add(userControl);
                }
                else
                {
                    this.tablePageRecChecked.PageVisible = false;

                }
                if (valueStr[2] == "1")
                {
                    //索引卡
                    IndexCard indexCard = new IndexCard();
                    indexCard.Dock = DockStyle.Fill;
                    tabPageIndexCard.Controls.Add(indexCard);
                }
                else
                {
                    this.tabPageIndexCard.PageVisible = false;
                }

            }
            //配置病案首页归档可见add by ywk
            string isshowiem = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("IsShowEditIemInfo");
            if (isshowiem == "0")
            {
                tabTabIemAcrive.PageVisible = false;
            }
            else
            {
                tabTabIemAcrive.PageVisible = true;
            }
            AutoFeed();

        }
        /// <summary>
        /// 设置到期的补写申请为 归还状态
        /// </summary>
        public static void AutoFeed()
        {
            try
            {

                string sql2 = @"update inpatient set islock='4707'  where noofinpat in (select noofinpat from EMR_RECORDWRITEUP   where status = 2 " +
                   " and trunc(sysdate,'DD') - trunc(approvedate,'DD') >= applytimes " +
                   " )";

                DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteNonQuery(sql2, CommandType.Text);
                string sql3 = @"update recorddetail set islock='4707'  where noofinpat in (select noofinpat from EMR_RECORDWRITEUP   where status = 2 " +
                   " and trunc(sysdate,'DD') - trunc(approvedate,'DD') >= applytimes " +
                   " )";
                DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteNonQuery(sql3, CommandType.Text);
                string sql = " update EMR_RECORDWRITEUP set status = 5  " +
                   " where status = 2 " +
                   " and trunc(sysdate,'DD') - trunc(approvedate,'DD') >= applytimes ";
                DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteNonQuery(sql);

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}