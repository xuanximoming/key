using System;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;

namespace DrectSoft.JobManager
{
    public partial class ManagerForm : Form
    {
        public ManagerForm()
        {
            InitializeComponent();
        }

        JobTaskManager taskManager;
        SynchTreeView treeView;
        FrmTimeSetting jobTimeSetting;
        private DataTable CurrentDataTable
        {
            get { return _currentDataTable; }
        }
        private DataTable _currentDataTable;


        private void InitConfigTree()
        {
            treeView = new SynchTreeView(taskManager.Systems);
            treeView.Dock = DockStyle.Fill;
            this.panelControlTree.Controls.Add(treeView);
            treeView.SelectedNodeChanged += new EventHandler<TreeViewEventArgs>(treeView_SelectedNodeChanged);
            treeView.ExpandAll();
            jobTimeSetting = new FrmTimeSetting();
            jobTimeSetting.Dock = DockStyle.Fill;
            xtraTabPage2.Controls.Add(jobTimeSetting);
        }


        private void ManagerForm_Load(object sender, EventArgs e)
        {
            taskManager = new JobTaskManager();
            InitConfigTree();

        }
        #region job thread

        private void LoadCustomerLog(string fileName)
        {
            _currentDataTable = null;

            if (string.IsNullOrEmpty(fileName))
                return;

            _currentDataTable = JobLogHelper.LoadLogFileToTable(JobLogHelper.GetJobLogCurrentFullPath(jobTimeSetting.CurrentJob));
            gridControlLog.DataSource = _currentDataTable;
        }


        private void CallJobInitializeAction(Job job)
        {
            if (job != null)
            {
                if ((job.Action != null) && job.Action.HasInitializeAction)
                    DoJobInitializeActionThread(job);
            }
            else
                MessageBox.Show("请先选择一个任务节点！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
        }

        private void CallJobExecuteAction(Job job)
        {
            if (job != null)
            {
                DoJobExecuteActionThread(job);
            }
            else
                MessageBox.Show("请先选择一个任务节点！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
        }


        private void DoJobExecuteActionThread(Job job)
        {
            Cursor = Cursors.WaitCursor;
            //lock (job)
            {
                if (job.Action != null)
                {
                    try
                    {
                        JobLogHelper.WriteLog(new JobExecuteInfoArgs(job, "开始", TraceLevel.Info));
                        job.Action.Execute();
                        job.JobSchedule.LastExecuteTime = DateTime.Now;
                        JobLogHelper.WriteLog(new JobExecuteInfoArgs(job, "结束", TraceLevel.Info));
                    }
                    catch (Exception err)
                    {
                        JobLogHelper.WriteLog(new JobExecuteInfoArgs(job, String.Empty, err));
                    }
                }
            }
            Cursor = Cursors.Default;
        }

        private void DoJobInitializeActionThread(Job job)
        {
            Cursor = Cursors.WaitCursor;
            //lock (job)
            {
                if (job.Action != null)
                {
                    try
                    {
                        JobLogHelper.WriteLog(new JobExecuteInfoArgs(job, "开始", TraceLevel.Info));
                        job.Action.ExecuteDataInitialize();
                        job.JobSchedule.LastExecuteTime = DateTime.Now;
                        JobLogHelper.WriteLog(new JobExecuteInfoArgs(job, "结束", TraceLevel.Info));
                    }
                    catch (Exception err)
                    {
                        JobLogHelper.WriteLog(new JobExecuteInfoArgs(job, String.Empty, err));
                    }
                }
            }
            Cursor = Cursors.Default;
        }


        #endregion

        void treeView_SelectedNodeChanged(object sender, TreeViewEventArgs e)
        {
            //切换任务
            if (e.Node == null || (e.Node.Tag == null)) return;
            Job job = (Job)e.Node.Tag;
            if (job == null) return;
            xtraTabPage1.Text = "任务执行-----" + job.Name;
            jobTimeSetting.CallJobPlanSet(job);
            //读取日志
            LoadCustomerLog(job.Name);

        }

        private void btn_Excute_Click(object sender, EventArgs e)
        {
            if ((jobTimeSetting == null) || (jobTimeSetting.CurrentJob == null)) return;
            DoJobExecuteActionThread(jobTimeSetting.CurrentJob);
            LoadCustomerLog(jobTimeSetting.CurrentJob.Name);

        }

        private void btn_InitJob_Click(object sender, EventArgs e)
        {
            if ((jobTimeSetting == null) || (jobTimeSetting.CurrentJob == null)) return;
            DoJobInitializeActionThread(jobTimeSetting.CurrentJob);
            LoadCustomerLog(jobTimeSetting.CurrentJob.Name);
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                jobTimeSetting.ResetPlanSetting();
                //保存数据
                taskManager.SaveJobConfig();
                MessageBox.Show("日程设置成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
