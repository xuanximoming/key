using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace DrectSoft.Core.QCReport
{
    public partial class MainLoadForm : DevBaseForm, IStartPlugIn
    {
        IEmrHost m_app;
        XmlOperate xmlOperate = null;
        Guage uc_guage = new Guage();
        Reportboard uc_reportBoard = new Reportboard(AppDomain.CurrentDomain.BaseDirectory + @"Sheet\Sheet.xml");

        public MainLoadForm()
        {
            try
            {
                InitializeComponent();
                xmlOperate = new XmlOperate(AppDomain.CurrentDomain.BaseDirectory + @"Sheet\Sheet.xml");
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }


        public IPlugIn Run(FrameWork.WinForm.Plugin.IEmrHost host)
        {
            try
            {
                PlugIn plg = new PlugIn(this.GetType().ToString(), this);
                m_app = host;
                return plg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void InitTreeNodes()
        {
            try
            {
                //treeView1.Nodes.Add("常用指标统计");

                Dictionary<string, List<string>> dic = xmlOperate.GetReportsCaption();
                foreach (KeyValuePair<string, List<string>> pair in dic)
                {
                    TreeNode[] childNodes = new TreeNode[pair.Value.Count];
                    for (int i = 0; i < childNodes.Length; i++)
                    {
                        childNodes[i] = new TreeNode(pair.Value[i], 1, 1);
                    }
                    TreeNode node = new TreeNode(pair.Key, 0, 0, childNodes);
                    treeView1.Nodes.Add(node);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MainLoadForm_Load(object sender, EventArgs e)
        {
            try
            {
                InitTreeNodes();
                //  this.panelContext.Controls.Clear();
                // this.panelContext.Controls.Add(uc_guage);
                // uc_guage.Dock = DockStyle.Fill;
            }
            catch
            { }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                if (e.Node.Text.Equals("常用指标统计"))
                {
                    this.panelContext.Controls.Clear();
                    this.panelContext.Controls.Add(uc_guage);
                    uc_guage.Dock = DockStyle.Fill;
                }
                else
                {
                    if (e.Node.Nodes.Count > 0)
                    {
                        return;
                    }
                    this.panelContext.Controls.Clear();
                    this.panelContext.Controls.Add(uc_reportBoard);
                    uc_reportBoard.Dock = DockStyle.Fill;
                    uc_reportBoard.ClearGridData();
                    uc_reportBoard.LoadReport(e.Node.Text);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
    }
}