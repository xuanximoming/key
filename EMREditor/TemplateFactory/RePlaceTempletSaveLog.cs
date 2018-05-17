using DevExpress.Utils;
using DevExpress.XtraTreeList.Nodes;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Core;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Data;
using System.Windows.Forms;
using System.Xml;

namespace DrectSoft.Emr.TemplateFactory
{
    public partial class RePlaceTempletSaveLog : DevBaseForm
    {
        IEmrHost m_app;
        DrectSoftLog m_Logger;

        private WaitDialogForm m_WaitDialog;

        SQLManger m_SQLManger;

        /// <summary>
        /// 记录右面grid数据源
        /// </summary>
        DataTable m_gridsource;
        public RePlaceTempletSaveLog(IEmrHost _app)
        {
            InitializeComponent();
            m_app = _app;
            m_WaitDialog = new WaitDialogForm("正在创建用户界面...", "请稍侯");
        }

        public RePlaceTempletSaveLog()
        {
            InitializeComponent();
        }


        private void RePlaceTempletTitle_Load(object sender, EventArgs e)
        {

            MakeTree();
            CreateGridSourceCol();

            m_SQLManger = new SQLManger(m_app);
            m_Logger = new DrectSoftLog("批量修改模板页眉页脚错误日志");
            HideWaitDialog();
        }

        private void CreateGridSourceCol()
        {
            m_gridsource = new DataTable();
            DataColumn dcID = new DataColumn("ID", Type.GetType("System.String"));
            DataColumn dcName = new DataColumn("NAME", Type.GetType("System.String"));

            m_gridsource.Columns.Add(dcID);
            m_gridsource.Columns.Add(dcName);

        }

        #region 初始页面


        private void MakeTree()
        {
            string sql = @" select CCODE,CNAME from DICT_CATALOG  where CTYPE='2'";

            DataTable dt = m_app.SqlHelper.ExecuteDataTable(sql);

            DataTable dtdetail = null;
            foreach (DataRow dr in dt.Rows)
            {
                TreeListNode parentNode = null;//
                parentNode = treeList_Template.AppendNode(new object[] { dr["CNAME"].ToString(), dr["CCODE"].ToString() }, null);
                parentNode.Tag = new EmrNodeObj(dr, EmrNodeType.Container);
                parentNode.CheckState = CheckState.Unchecked;

                sql = string.Format(@"select t.templet_id,t.mr_name from emrtemplet t where t.mr_class = '{0}' order by t.create_datetime", dr["CCODE"].ToString());
                dtdetail = m_app.SqlHelper.ExecuteDataTable(sql);
                foreach (DataRow drdetail in dtdetail.Rows)
                {
                    TreeListNode node = null;//
                    node = treeList_Template.AppendNode(new object[] { drdetail["mr_name"].ToString(), drdetail["templet_id"].ToString() }, parentNode);
                    node.Tag = new EmrNodeObj(drdetail, EmrNodeType.Leaf);
                    node.CheckState = CheckState.Unchecked;
                }
            }
        }

        private void SetWaitDialogCaption(string caption)
        {
            if (m_WaitDialog != null)
            {
                if (!m_WaitDialog.Visible)
                    m_WaitDialog.Visible = true;
                m_WaitDialog.Caption = caption;
            }

        }

        private void HideWaitDialog()
        {
            if (m_WaitDialog != null)
                m_WaitDialog.Hide();
        }

        #endregion

        private void treeList_Template_AfterCheckNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            SetCheckedChildNodes(e.Node, e.Node.CheckState);
            SetCheckedParentNodes(e.Node, e.Node.CheckState);
        }


        private void SetCheckedChildNodes(TreeListNode node, CheckState check)
        {
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                node.Nodes[i].CheckState = check;
                SetCheckedChildNodes(node.Nodes[i], check);
            }
        }
        private void SetCheckedParentNodes(TreeListNode node, CheckState check)
        {
            if (node.ParentNode != null)
            {
                bool b = false;
                CheckState state;
                for (int i = 0; i < node.ParentNode.Nodes.Count; i++)
                {
                    state = (CheckState)node.ParentNode.Nodes[i].CheckState;
                    if (!check.Equals(state))
                    {
                        b = !b;
                        break;
                    }
                }
                node.ParentNode.CheckState = b ? CheckState.Indeterminate : check;
                SetCheckedParentNodes(node.ParentNode, check);
            }
        }

        private void treeList_Template_BeforeCheckNode(object sender, DevExpress.XtraTreeList.CheckNodeEventArgs e)
        {
            e.State = (e.PrevState == CheckState.Checked ? CheckState.Unchecked : CheckState.Checked);
        }

        /// <summary>
        /// 将选中的目标加入到右面grid中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_check_Click(object sender, EventArgs e)
        {
            m_gridsource.Rows.Clear();
            foreach (TreeListNode parentNode in treeList_Template.Nodes)
            {
                foreach (TreeListNode node in parentNode.Nodes)
                {
                    if (node.CheckState == CheckState.Checked)
                        if (((EmrNodeObj)node.Tag).Type == EmrNodeType.Leaf)
                        {
                            DataRow newrow = m_gridsource.NewRow();
                            newrow["ID"] = ((DataRow)((EmrNodeObj)node.Tag).Obj)["templet_id"];
                            newrow["NAME"] = ((DataRow)((EmrNodeObj)node.Tag).Obj)["mr_name"];
                            m_gridsource.Rows.Add(newrow);
                        }
                }
            }

            gridView1.GridControl.DataSource = m_gridsource;
        }

        private void simpleButton_Click(object sender, EventArgs e)
        {
            m_gridsource.Rows.Clear();

            gridView1.GridControl.DataSource = m_gridsource;
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 替换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)gridControl1.DataSource;
            if (null == dt || dt.Rows.Count == 0)
            {
                MyMessageBox.Show("模板列表为空，请先将左侧需要删除日志的模板移动到右侧列表。", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                return;
            }

            SetWaitDialogCaption("正在批量清除模板日志内容...");

            Emrtemplet m_Emrtemplet;
            //循环替换页眉页脚
            int losecnt = 0;
            int successcnt = 0;
            foreach (DataRow dr in ((DataTable)gridControl1.DataSource).Rows)
            {
                try
                {
                    m_Emrtemplet = m_SQLManger.GetTemplet(dr["ID"].ToString());

                    //if (chk_Title.Checked)
                    //    m_Emrtemplet.XML_DOC_NEW = ReplaceTitle(m_Emrtemplet.XML_DOC_NEW);
                    //if (chk_Foot.Checked)
                    m_Emrtemplet.XML_DOC_NEW = ReplaceSaveLog(m_Emrtemplet.XML_DOC_NEW);

                    m_SQLManger.BatchSaveTemplet(m_Emrtemplet, "2");
                }
                catch (Exception ex)
                {
                    losecnt++;
                    m_Logger.Error("模板批量删除日志信息错误日志：" + "\t" + "模板TempletID：" + dr["ID"] + "\t" + ex.Message);

                    continue;
                }
                successcnt++;
            }

            HideWaitDialog();

            string mess = "";
            if (successcnt > 0)
            {
                mess = "替换成功，成功替换了【" + successcnt.ToString() + "】条记录。";
                if (losecnt > 0)
                {
                    mess += "\n其中有【" + losecnt + "】条记录未替换成功，\n未成功记录请查看服务器上\\Logs下日志信息。";
                }
                m_app.CustomMessageBox.MessageShow(mess);
            }
            else
            {
                m_app.CustomMessageBox.MessageShow("替换失败，有【" + losecnt + "】条记录未替换成功。\n未成功记录请查看服务器上\\Logs下日志信息。");
            }

        }

        /// <summary>
        /// 清空模板中savelogs标签中值
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <returns></returns>
        private string ReplaceSaveLog(string xmlDocument)
        {
            XmlDocument content = new XmlDocument();
            content.LoadXml(xmlDocument);
            XmlNode xmlelement = content.GetElementsByTagName("savelogs")[0];
            if (xmlelement.InnerXml == "")
            {
                return xmlDocument;
            }
            return xmlDocument.Replace(xmlelement.InnerXml, "");
        }

        /// <summary>
        /// 供外部使用清除模板日志信息
        /// </summary>
        /// <param name="xmlDocument">m_Emrtemplet.XML_DOC_NEW</param>
        /// <returns></returns>
        public string ClearTempletSaveLog(string xmlDocument)
        {
            return ReplaceSaveLog(xmlDocument);
        }

        /// <summary>
        /// 序号
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-06</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
    }

}