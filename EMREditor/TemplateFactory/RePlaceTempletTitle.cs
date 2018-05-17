using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DevExpress.XtraTreeList.Nodes;
using DrectSoft.Wordbook;
using DevExpress.Utils;
using System.Xml;
using System.Xml.Linq;
using DrectSoft.FrameWork.Log;
using DrectSoft.Core;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Emr.TemplateFactory
{
    /// edit by Yanqiao.Cai 2013-02-22 add try ... catch(全部)
    public partial class RePlaceTempletTitle : DevBaseForm
    {
        IEmrHost m_app;
        DrectSoftLog m_Logger;

        private WaitDialogForm m_WaitDialog;

        /// <summary>
        /// 选中页眉文件实体
        /// </summary>
        EmrTempletHeader m_EmrTempletHeader;

        /// <summary>
        /// 选中的页脚文件实体
        /// </summary>
        EmrTemplet_Foot m_EmrTemplet_Foot;

        SQLManger m_SQLManger;

        /// <summary>
        /// 记录右面grid数据源
        /// </summary>
        DataTable m_gridsource;
        public RePlaceTempletTitle(IEmrHost _app)
        {
            try
            {
                InitializeComponent();
                m_app = _app;
                m_WaitDialog = new WaitDialogForm("正在创建用户界面...", "请稍侯");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RePlaceTempletTitle_Load(object sender, EventArgs e)
        {
            try
            {
                InitHeader();
                InitFoot();
                MakeTree();
                CreateGridSourceCol();

                m_SQLManger = new SQLManger(m_app);
                m_Logger = new DrectSoftLog("批量修改模板页眉页脚错误日志");
                HideWaitDialog();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void CreateGridSourceCol()
        {
            try
            {
                m_gridsource = new DataTable();
                DataColumn dcID = new DataColumn("ID", Type.GetType("System.String"));
                DataColumn dcName = new DataColumn("NAME", Type.GetType("System.String"));

                m_gridsource.Columns.Add(dcID);
                m_gridsource.Columns.Add(dcName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 初始页面
        //初始化页眉文件
        private void InitHeader()
        {
            try
            {
                lookUpWindowHeader.SqlHelper = m_app.SqlHelper;

                string sql = string.Format(@"select a.header_id ID,a.name from emrtempletheader a");
                DataTable Dept = m_app.SqlHelper.ExecuteDataTable(sql);

                Dept.Columns["ID"].Caption = "页眉代码";
                Dept.Columns["NAME"].Caption = "页眉名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 60);
                cols.Add("NAME", 140);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//NAME");
                lookUpEditorHeader.SqlWordbook = deptWordBook; ;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化页脚
        /// </summary>
        private void InitFoot()
        {
            try
            {
                lookUpWindowFoot.SqlHelper = m_app.SqlHelper;

                string sql = string.Format(@"select a.foot_id id,a.name from emrtemplet_foot a");
                DataTable Dept = m_app.SqlHelper.ExecuteDataTable(sql);

                Dept.Columns["ID"].Caption = "页脚代码";
                Dept.Columns["NAME"].Caption = "页脚名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 60);
                cols.Add("NAME", 140);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//NAME");
                lookUpEditorFoot.SqlWordbook = deptWordBook; ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MakeTree()
        {
            try
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
                    //已经删除的就不需要了add by ywk 2013年7月8日 10:38:48
                    //Modify by xlb SQL错误 2013-07-09
                    //sql = string.Format(@"select t.templet_id,t.mr_name from emrtemplet t where t.mr_class = '{0}' order by t.create_datetime  and valid=1 ", dr["CCODE"].ToString());
                    sql = string.Format(@"select t.templet_id,t.mr_name from emrtemplet t where valid='1' and t.mr_class = '{0}' order by t.create_datetime ", dr["CCODE"].ToString());
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetWaitDialogCaption(string caption)
        {
            try
            {
                if (m_WaitDialog != null)
                {
                    if (!m_WaitDialog.Visible)
                        m_WaitDialog.Visible = true;
                    m_WaitDialog.Caption = caption;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void HideWaitDialog()
        {
            try
            {
                if (m_WaitDialog != null)
                    m_WaitDialog.Hide();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        private void treeList_Template_AfterCheckNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            try
            {
                SetCheckedChildNodes(e.Node, e.Node.CheckState);
                SetCheckedParentNodes(e.Node, e.Node.CheckState);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }


        private void SetCheckedChildNodes(TreeListNode node, CheckState check)
        {
            try
            {
                for (int i = 0; i < node.Nodes.Count; i++)
                {
                    node.Nodes[i].CheckState = check;
                    SetCheckedChildNodes(node.Nodes[i], check);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void SetCheckedParentNodes(TreeListNode node, CheckState check)
        {
            try
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void treeList_Template_BeforeCheckNode(object sender, DevExpress.XtraTreeList.CheckNodeEventArgs e)
        {
            try
            {
                e.State = (e.PrevState == CheckState.Checked ? CheckState.Unchecked : CheckState.Checked);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 将选中的目标加入到右面grid中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_check_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void simpleButton_Click(object sender, EventArgs e)
        {
            try
            {
                m_gridsource.Rows.Clear();

                gridView1.GridControl.DataSource = m_gridsource;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 替换事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                string errorStr = CheckItem();
                if (!string.IsNullOrEmpty(errorStr))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(errorStr);
                    return;
                }
                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要批量替换" + (chk_Title.Checked ? "页眉" : "") + (chk_Foot.Checked ? "页脚" : "") + "吗？该操作不可恢复。", "批量替换页眉页脚", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.Cancel)
                {
                    return;
                }

                SetWaitDialogCaption("正在批量替换" + (chk_Title.Checked ? "页眉" : "") + (chk_Foot.Checked ? "页脚" : "") + "...");

                GetHeaderFootEntity();

                Emrtemplet m_Emrtemplet;
                //循环替换页眉页脚
                int losecnt = 0;
                int successcnt = 0;
                foreach (DataRow dr in ((DataTable)gridControl1.DataSource).Rows)
                {
                    try
                    {
                        m_Emrtemplet = m_SQLManger.GetTemplet(dr["ID"].ToString());

                        if (chk_Title.Checked)
                            m_Emrtemplet.XML_DOC_NEW = ReplaceTitle(m_Emrtemplet.XML_DOC_NEW);
                        if (chk_Foot.Checked)
                            m_Emrtemplet.XML_DOC_NEW = ReplaceFoot(m_Emrtemplet.XML_DOC_NEW);

                        m_SQLManger.BatchSaveTemplet(m_Emrtemplet, "2");
                    }
                    catch (Exception ex)
                    {
                        losecnt++;
                        //Log log = new Log();
                        //log.Write("模板批量替换页眉页脚错误日志：" + "\t" + "模板TempletID：" + dr["ID"] + ex.Message);
                        //(new Log()).Write("模板批量替换页眉页脚错误日志：" + "\t" + "模板TempletID：" + dr["ID"] + ex.Message);
                        m_Logger.Error("模板批量替换页眉页脚错误日志：" + "\t" + "模板TempletID：" + dr["ID"] + "\t" + ex.Message);
                        //m_Logger.Info("模板批量替换页眉页脚错误日志：" + "\t" + "模板TempletID：" + dr["ID"] + "\t" + ex.Message);
                        continue;
                    }
                    successcnt++;
                }

                HideWaitDialog();

                string mess = "";
                if (successcnt > 0)
                {
                    mess = "替换成功，成功替换了【" + successcnt.ToString() + "】记录。";
                    if (losecnt > 0)
                    {
                        mess += "\n其中有【" + losecnt + "】条记录未替换成功，\n未成功记录请查看服务器上\\Logs下日志信息。";
                    }
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(mess);
                }
                else
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("替换失败，有【" + losecnt + "】条记录未替换成功，\n未成功记录请查看服务器上\\Logs下日志信息。");
                }
            }
            catch (Exception ex)
            {
                HideWaitDialog();
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 画面检查
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-06</date>
        /// </summary>
        private string CheckItem()
        {
            try
            {
                DataTable dt = (DataTable)gridControl1.DataSource;
                if (null == dt || dt.Rows.Count == 0)
                {
                    btn_check.Focus();
                    return "请先添加要替换页眉页脚的模板";
                }

                if (!chk_Title.Checked && !chk_Foot.Checked)
                {
                    chk_Title.Focus();
                    return "请先启用页眉或页脚文件";
                }
                if (chk_Title.Checked)
                {
                    if (string.IsNullOrEmpty(lookUpEditorHeader.CodeValue.Trim()))
                    {
                        lookUpEditorHeader.Focus();
                        return "请选择要替换的页眉文件";
                    }
                }
                else if (!string.IsNullOrEmpty(lookUpEditorHeader.CodeValue.Trim()))
                {
                    chk_Title.Focus();
                    return "您选择了要替换的页眉文件，请先勾选右侧的启用按钮。";
                }

                if (chk_Foot.Checked)
                {
                    if (string.IsNullOrEmpty(lookUpEditorFoot.CodeValue.Trim()))
                    {
                        lookUpEditorFoot.Focus();
                        return "请选择要替换的页脚文件";
                    }
                }
                else if (!string.IsNullOrEmpty(lookUpEditorFoot.CodeValue.Trim()))
                {
                    chk_Foot.Focus();
                    return "您选择了要替换的页脚文件，请先勾选右侧的启用按钮。";
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据页眉选中的页眉，页脚加载实体
        /// </summary>
        private void GetHeaderFootEntity()
        {
            try
            {
                if (lookUpEditorHeader.CodeValue.Trim() != "")
                {
                    m_EmrTempletHeader = m_SQLManger.GetTemplet_Header(lookUpEditorHeader.CodeValue);
                }

                if (lookUpEditorFoot.CodeValue.Trim() != "")
                {
                    m_EmrTemplet_Foot = m_SQLManger.GetTemplet_Foot(lookUpEditorFoot.CodeValue);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据选中的页眉替换目标文件
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <returns></returns>
        private string ReplaceTitle(string xmlDocument)
        {
            try
            {
                XmlDocument content = new XmlDocument();
                content.LoadXml(xmlDocument);
                XmlNode xmlelement = content.GetElementsByTagName("header")[0];

                XmlDocument titlexml = new XmlDocument();
                titlexml.LoadXml(m_EmrTempletHeader.Content);

                //XmlElement titlexmlelement = titlecon.get("body");
                XmlNode titlexmlelement = titlexml.GetElementsByTagName("body")[0];

                //content.ReplaceChild(titlexmlelement, xmlelement);

                return xmlDocument.Replace(xmlelement.InnerXml, titlexmlelement.InnerXml);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string ReplaceFoot(string xmlDocument)
        {
            try
            {
                XmlDocument content = new XmlDocument();
                content.LoadXml(xmlDocument);
                XmlNode xmlelement = content.GetElementsByTagName("footer")[0];

                XmlDocument footxml = new XmlDocument();

                footxml.LoadXml(m_EmrTemplet_Foot.Content);

                XmlNode titlexmlelement = footxml.GetElementsByTagName("body")[0];

                return xmlDocument.Replace(xmlelement.InnerXml, titlexmlelement.InnerXml);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 复选框回车事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-06</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chb_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == 13)
                {
                    DS_Common.cbx_KeyPress(sender);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
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