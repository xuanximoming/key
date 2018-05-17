using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Emr.Util;
using System.Xml;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraBars;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Core.MainEmrPad
{
    public partial class HistoryEmrForm : DevBaseForm
    {
        PatRecUtil m_PatUtil;
        EmrModel m_Model;
        TreeListNode m_Node;
        RecordDal m_RecordDal;
        bool m_IsNeedInsertContent = false;

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
        PadForm CurrentForm
        {
            get
            {
                if (panelEmrContent.Controls.Count > 0)
                {
                    return panelEmrContent.Controls[0] as PadForm;
                }
                return null;
            }
        }

        public HistoryEmrForm()
        {
            InitializeComponent();
        }

        public HistoryEmrForm(PatRecUtil patUtil, RecordDal recordDal, EmrModel model, TreeListNode node)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            m_PatUtil = patUtil;
            m_RecordDal = recordDal;
            m_Node = node;
            GetEmrContent(model);
            AddPadForm();
        }

        private void GetEmrContent(EmrModel model)
        {
            //是病程记录，但是否首次病程
            if (model.ModelCatalog == ContainerCatalog.BingChengJiLu != model.FirstDailyEmrModel)
            {
                //找到首次病程
                DataTable dt = m_PatUtil.GetFirstDailyEmrContent(model.InstanceId.ToString());
                if (dt.Rows.Count > 0)
                {
                    XmlDocument content = new XmlDocument();
                    content.PreserveWhitespace = true;
                    string emrContent = dt.Rows[0]["CONTENT"].ToString();
                    content.LoadXml(emrContent);
                    model.ModelContent = content;
                }
            }
            else
            {
                model.ModelContent = m_RecordDal.LoadModelInstance(model.InstanceId).ModelContent;
            }

            m_Model = model;
        }

        private void AddPadForm()
        {
            PadForm pad = new PadForm(m_PatUtil, null);
            pad.Dock = DockStyle.Fill;
            pad.LoadDocment(m_Model);
            this.Text = "病历内容--" + m_Model.ModelName;
            panelEmrContent.Controls.Add(pad);
        }

        private void HistoryEmrForm_Load(object sender, EventArgs e)
        {
            Clipboard.Clear();
            CurrentForm.LocateDailyEmrMode(m_Node);
            SetEmrDocumentModel();
        }

        /// <summary>
        /// 设置编辑器的编辑模式
        /// </summary>
        private void SetEmrDocumentModel()
        {
            if (CurrentForm != null)
            {
                CurrentForm.zyEditorControl1.EMRDoc.Info.DocumentModel = DrectSoft.Library.EmrEditor.Src.Document.DocumentModel.Clear;//默认不显示修改痕迹
                //CurrentForm.zyEditorControl1.EMRDoc.Info.DocumentModel = DrectSoft.Library.EmrEditor.Src.Document.DocumentModel.Read;
                CurrentForm.zyEditorControl1.EMRDoc.Info.ShowParagraphFlag = false;
            }
            //m_PatUtil = new PatRecUtil();
            //根据系统参数配置控制，历史病历展示界面的病历是否可以插入和复制 add by ywk 2012年12月7日10:58:56
            string IsShowExportHistory = m_PatUtil.GetConfigValueByKey("IsShowExportHistory");
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
        //public string GetConfigValueByKey(string key)
        //{

        //    string sql1 = " select * from appcfg where configkey = '" + key + "'  ";
        //    DataTable dt = m_app.SqlHelper.ExecuteDataTable(sql1, CommandType.Text);
        //    string config = string.Empty;
        //    if (dt.Rows.Count > 0)
        //    {
        //        config = dt.Rows[0]["value"].ToString();
        //    }
        //    return config;
        //}


        private void barLargeButtonItemClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barLargeButtonItemInsert_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            m_IsNeedInsertContent = true;
            CurrentForm.zyEditorControl1.EMRDoc._Copy();
            this.Close();
        }

        private void barLargeButtonItemCopy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CurrentForm.zyEditorControl1.EMRDoc._Copy();
        }
    }
}