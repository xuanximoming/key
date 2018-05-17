using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Core.QCReport;
using DrectSoft.DrawDriver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace DrectSoft.Core.ReportManager
{
    /// <summary>
    ///  报告卡统计报表模块
    ///  add by ywk 2013年7月29日 10:21:12
    /// </summary>
    public partial class ReportStatistic : DevBaseForm
    {
        private FrameWork.WinForm.Plugin.IEmrHost m_App;

        #region 构造函数
        public ReportStatistic()
        {
            InitializeComponent();
        }

        public ReportStatistic(FrameWork.WinForm.Plugin.IEmrHost m_Host)
        {
            // TODO: Complete member initialization
            InitializeComponent();
            m_App = m_Host;
            //xmlOperate = XmlOperate.GetInstance();

            xmlOperate = new XmlOperate(AppDomain.CurrentDomain.BaseDirectory + "Report.xml");
        }

        #endregion





        #region 字段及属性
        XmlOperate xmlOperate = null;
        Reportboard reportboard1;
        #endregion

        #region 方法
        /// <summary>
        /// 加载TreeView
        /// </summary>
        private void InitTreeNodes()
        {
            try
            {
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

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("加载树控件出错" + ex.Message);
                return;
            }
        }
        #endregion

        #region 事件
        /// <summary>
        /// 界面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReportStatistic_Load(object sender, EventArgs e)
        {
            InitTreeNodes();
            //reportboard1.IniDataFieldValueHandle+=new Reportboard.del_IniDataFieldValue(reportboard1_IniDataFieldValueHandle);
            //reportboard1 = new Reportboard();
            //Reportboard reportboard1 = new Reportboard();
            //panelControl1.Controls.Add(reportboard1);
            //reportboard1.eh += new EventHandler(reportboard1_eh);
        }

        //void reportboard1_eh(object sender, EventArgs e)
        //{
        //    MessageBox.Show("OK");
        //}

        private void reportboard1_IniDataFieldValueHandle(ref Dictionary<string, ParamObject> dic)
        {
            try
            {
                string year = dic["year"].Value;
                string month = dic["month"].Value;
                string date = year + "-" + month;
                string datestr = year + "年" + month + "月";
                DataTable dt = GetThemaData();
                string hospname = GetHospital();
                int deathnum = 0;//死亡例数
                int newsicknum = 0;//发病例数
                string calltype = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("TheriomaCardTip");
                if (dt.Rows.Count > 0)
                {
                    DataRow[] drlooksick = dt.Select(string.Format(" reportdate like '{0}%'", date));
                    DataRow[] drdeath = dt.Select(string.Format(" DEATHDATE is not null and reportdate like '{0}%'", date));



                    //0是九江的（只要有死亡时间就是死亡），1是中心的（有标识判断是否死亡）
                    if (calltype == "0")
                    {
                        deathnum = drdeath.Length;
                        newsicknum = drlooksick.Length;
                    }
                    if (calltype == "1")
                    {
                        DataRow[] drdead = dt.Select(string.Format(" reportdate like '{0}%' and cardtype='1' ", date));
                        DataRow[] drsick = dt.Select(string.Format("  reportdate like '{0}%' and cardtype='0' ", date));
                        deathnum = drdead.Length;
                        newsicknum = drsick.Length;
                    }

                }
                dic.Add("DeathNum", new ParamObject("DeathNum", "", deathnum.ToString()));
                dic.Add("SickNum", new ParamObject("SickNum", "", newsicknum.ToString()));
                dic.Add("ReportDate", new ParamObject("ReportDate", "", datestr.ToString()));
                dic.Add("Hospname", new ParamObject("Hospname", "", hospname.ToString()));
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息为" + ex.Message);
                return;
            }
        }
        /// <summary>
        /// 获得医院名称
        /// </summary>
        /// <returns></returns>
        private string GetHospital()
        {
            try
            {
                DrectSoft.DSSqlHelper.DS_SqlHelper.CreateSqlHelper();

                DataTable dt = new DataTable();
                dt = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable("select * from hospitalinfo ");
                return dt.Rows[0]["name"].ToString();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息为" + ex.Message);
                return null;
            }

        }
        /// <summary>
        /// 双击列表右侧展示查询条件及列表信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                if (e.Node.Nodes.Count > 0)
                {
                    return;
                }
                reportboard1 = new Reportboard(AppDomain.CurrentDomain.BaseDirectory + "Report.xml");
                this.panelControl3.Controls.Clear();
                this.panelControl3.Controls.Add(reportboard1);

                //在此处给Dictionary赋值 后期要根据父节点控制此处完善 此处先满足江西九江中医院的需求
                //if (e.Node.Text.Contains("月报表"))//是月报表
                //{
                //Control[] controls = reportboard1.Controls.Find("flowLayoutPanel1", true);
                //报表名称
                //uc_reportBoard.paramList.Add("ReportName", new ParamObject("ReportName", "text", e.Node.Text));
                //uc_reportBoard.paramList.Add("DeathNum", new ParamObject("DeathNum", "", deathnum.ToString()));
                //uc_reportBoard.paramList.Add("Name", new ParamObject("Name", "text", "张三"));
                //uc_reportBoard.paramList=
                //}
                //if (e.Node.Text.Contains("新发"))
                //{

                //}
                //if (e.Node.Text.Contains("死亡"))
                //{

                //}

                reportboard1.Dock = DockStyle.Fill;
                reportboard1.ClearGridData();
                reportboard1.LoadReport(e.Node.Text);
                reportboard1.IniDataFieldValueHandle += new Reportboard.del_IniDataFieldValue(reportboard1_IniDataFieldValueHandle);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("操作树控件出错" + ex.Message);
                return;
            }
        }
        /// <summary>
        /// 获得肿瘤报告卡信息
        /// </summary>
        /// <returns></returns>
        private DataTable GetThemaData()
        {
            try
            {
                DrectSoft.DSSqlHelper.DS_SqlHelper.CreateSqlHelper();

                DataTable dt = new DataTable();
                dt = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable("select * from theriomareportcard where VAILD = '1' and STATE not in ('3','5','7') ");
                return dt;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息为" + ex.Message);
                return null;
            }

        }
        #endregion
    }
}