using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraTreeList.Nodes;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Common.Eop;
using DrectSoft.Core.MainEmrPad.New;
using DrectSoft.DSSqlHelper;
using DrectSoft.Emr.Util;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Xml;

namespace DrectSoft.Core.MainEmrPad
{
    public partial class HistoryEmrFormNew : DevBaseForm
    {
        EmrModel m_Model;
        TreeListNode m_Node;
        RecordDal m_RecordDal;
        bool m_IsNeedInsertContent = false;
        Inpatient m_CurrentInpatient;
        IEmrHost m_App;

        /// <summary>
        /// 是否需要将历史病历的内容插入到当前病历中
        /// </summary>
        public bool IsNeedInsertContent
        {
            get
            {
                return m_IsNeedInsertContent;
            }
        }

        /// <summary>
        /// 当前界面选中的编辑器
        /// </summary>
        EditorForm CurrentForm
        {
            get
            {
                if (panelEmrContent.Controls.Count > 0)
                {
                    return panelEmrContent.Controls[0] as EditorForm;
                }
                return null;
            }
        }

        /// <summary>
        /// .ctor
        /// </summary>
        public HistoryEmrFormNew()
        {
            InitializeComponent();
        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="recordDal"></param>
        /// <param name="model"></param>
        /// <param name="node"></param>
        /// <param name="inpatient"></param>
        public HistoryEmrFormNew(RecordDal recordDal, EmrModel model, TreeListNode node, Inpatient inpatient, IEmrHost app)
        {
            WaitDialogForm m_WaitDialog = new WaitDialogForm("正在加载病历...", "请稍候");
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            m_RecordDal = recordDal;
            m_Node = node;
            m_CurrentInpatient = inpatient;
            m_App = app;
            GetEmrContent(model);
            AddPadForm();
            DS_Common.HideWaitDialog(m_WaitDialog);
        }

        /// <summary>
        /// 获取病历内容
        /// </summary>
        /// <param name="model"></param>
        private void GetEmrContent(EmrModel model)
        {
            m_Model = model;

            //是病程记录
            if (model.ModelCatalog == ContainerCatalog.BingChengJiLu)
            {
                model.ModelContent = GetAllDailyEmrContentInOneDept();
            }
            else
            {
                model.ModelContent = m_RecordDal.LoadModelInstance(model.InstanceId).ModelContent;
            }
        }

        /// <summary>
        /// 获取该病程所在科室的所有病程记录
        /// </summary>
        /// <returns></returns>
        private XmlDocument GetAllDailyEmrContentInOneDept()
        {
            try
            {
                string id = m_Model.InstanceId.ToString();
                string changeid = m_Model.DeptChangeID;
                string sqlGetRecord = string.Format(
                    @"SELECT r.content FROM recorddetail r WHERE r.noofinpat in (select noofinpat from recorddetail where recorddetail.id = '{0}')
                        and r.changeid = '{1}' and r.valid = '1' order by r.captiondatetime", id, changeid);
                DataTable dt = DS_SqlHelper.ExecuteDataTable(sqlGetRecord);

                //如果出现异常上面没有捞到数据，则直接通过ID到数据库中查找数据
                if (dt.Rows.Count == 0)
                {
                    sqlGetRecord = string.Format(
                        @"SELECT r.content FROM recorddetail r where r.id = '{0}'", id);
                    dt = DS_SqlHelper.ExecuteDataTable(sqlGetRecord);
                    return ConcatCheckedEmrContent(dt);
                }
                return ConcatCheckedEmrContent(dt);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 组合病历内容
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private XmlDocument ConcatCheckedEmrContent(DataTable dt)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = true;
                XmlNode nodeBody = null;
                List<string> emrList = new List<string>();
                foreach (DataRow dr in dt.Rows)
                {
                    string emrContent = dr["content"].ToString().Trim();
                    if (!string.IsNullOrEmpty(emrContent))
                    {
                        emrList.Add(emrContent);
                    }
                }

                if (emrList.Count > 0)
                {
                    doc = new XmlDocument();
                    for (int i = 0; i < emrList.Count; i++)
                    {
                        try
                        {
                            if (doc.InnerText == "")
                            {
                                string emrContent = emrList[i];
                                doc.PreserveWhitespace = true;
                                doc.LoadXml(emrContent);
                                nodeBody = doc.SelectSingleNode("document/body");
                            }
                            else
                            {
                                XmlDocument xmlDoc = new XmlDocument();
                                xmlDoc.PreserveWhitespace = true;
                                xmlDoc.LoadXml(emrList[i]);
                                XmlNode bodyNode = xmlDoc.SelectSingleNode("document/body");
                                foreach (XmlNode node in bodyNode.ChildNodes)
                                {
                                    XmlNode deepCopyNode = doc.ImportNode(node, true);
                                    nodeBody.AppendChild(deepCopyNode);
                                }
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
                return doc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void AddPadForm()
        {
            try
            {
                EditorForm pad = new EditorForm(m_CurrentInpatient, m_App);
                pad.Dock = DockStyle.Fill;
                if (m_Model.ModelContent.InnerXml.Trim() != "")
                {
                    pad.LoadDocument(m_Model.ModelContent.InnerXml);
                }
                this.Text = "病历内容--" + m_Model.ModelName;
                panelEmrContent.Controls.Add(pad);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HistoryEmrForm_Load(object sender, EventArgs e)
        {
            try
            {
                Clipboard.Clear();
                LocateDailyEmrModel(m_Node);
                SetEmrDocumentModel();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 在病程中定位对应的病程记录
        /// </summary>
        /// <param name="node"></param>
        private void LocateDailyEmrModel(TreeListNode node)
        {
            try
            {
                if (node.Tag is EmrModel)
                {
                    EmrModel model = node.Tag as EmrModel;
                    string displayDateTime = model.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss");
                    CurrentForm.SetCurrentElement(displayDateTime);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 设置编辑器的编辑模式
        /// </summary>
        private void SetEmrDocumentModel()
        {
            try
            {
                if (CurrentForm != null)
                {
                    CurrentForm.CurrentEditorControl.EMRDoc.Info.DocumentModel = DrectSoft.Library.EmrEditor.Src.Document.DocumentModel.Clear;//默认不显示修改痕迹
                    CurrentForm.CurrentEditorControl.EMRDoc.Info.ShowParagraphFlag = false;
                }
                //m_PatUtil = new PatRecUtil();
                //根据系统参数配置控制，历史病历展示界面的病历是否可以插入和复制 add by ywk 2012年12月7日10:58:56
                string IsShowExportHistory = DS_SqlService.GetConfigValueByKey(("IsShowExportHistory"));
                if (IsShowExportHistory == "0")
                {
                    barLargeButtonItemInsert.Visibility = BarItemVisibility.Never;
                    barLargeButtonItemCopy.Visibility = BarItemVisibility.Never;
                }
                else
                {
                    barLargeButtonItemInsert.Visibility = BarItemVisibility.Always;
                    barLargeButtonItemCopy.Visibility = BarItemVisibility.Always;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        private void barLargeButtonItemClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex.Message);
            }
        }

        private void barLargeButtonItemInsert_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                m_IsNeedInsertContent = true;
                CurrentForm.CurrentEditorControl.EMRDoc._Copy();
                this.Close();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex.Message);
            }
        }

        private void barLargeButtonItemCopy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                CurrentForm.CurrentEditorControl.EMRDoc._Copy();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex.Message);
            }
        }
    }
}