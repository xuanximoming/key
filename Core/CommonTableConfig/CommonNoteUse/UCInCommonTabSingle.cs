using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Common;

namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    public partial class UCInCommonTabSingle : DevExpress.XtraEditors.XtraUserControl
    {

        InCommonNoteEnmtity m_inCommonNote;
        InCommonNoteTabEntity m_InCommonNoteTab;
        CommonNote_TabEntity m_commonNote_TabEntity;
        IEmrHost m_app;
        InCommonNoteBiz inCommonNoteBiz;
        Point point = new Point(0, 0);
        bool m_canEdit;
        public UCInCommonTabSingle(InCommonNoteTabEntity inCommonNoteTab, CommonNote_TabEntity commonNote_TabEntity, InCommonNoteEnmtity inCommonNote, IEmrHost app, bool canEdit)
        {
            m_inCommonNote = inCommonNote;
            m_InCommonNoteTab = inCommonNoteTab;
            m_commonNote_TabEntity = commonNote_TabEntity;
            m_app = app;
            inCommonNoteBiz = new InCommonNoteBiz(m_app);
            m_canEdit = canEdit;
            InitializeComponent();
            this.Enabled = m_canEdit;
            InitData();
        }

        //初始化界面的 后台
        private void InitData()
        {

            inCommonNoteBiz.GetDetaliInCommonNoteByDay(ref m_inCommonNote, ref m_InCommonNoteTab, "", "");
            if (m_InCommonNoteTab.InCommonNoteItemList == null || m_InCommonNoteTab.InCommonNoteItemList.Count == 0)
            {
                m_InCommonNoteTab.InCommonNoteItemList = InCommonNoteBiz.ConvertItem(m_commonNote_TabEntity);
                foreach (var item in m_InCommonNoteTab.InCommonNoteItemList)
                {
                    item.InCommonNote_Tab_Flow = m_InCommonNoteTab.InCommonNote_Tab_Flow;
                    item.InCommonNoteFlow = m_InCommonNoteTab.InCommonNoteFlow;
                    item.CreateDoctorID = m_app.User.DoctorId;
                    item.CreateDoctorName = m_app.User.DoctorName;
                    //item.InCommonNote_Item_Flow = Guid.NewGuid().ToString();
                    item.RecordDate = DateUtil.getDateTime(System.DateTime.Now.ToString(), DateUtil.NORMAL_SHORT);
                    item.RecordTime = DateUtil.getDateTime(System.DateTime.Now.ToString(), DateUtil.NORMAL_LONG).Substring(11, 8);
                    item.RecordDoctorName = m_app.User.DoctorName;
                }

            }
            scorlInfo.Controls.Clear();
            for (int i = 0; i < m_InCommonNoteTab.InCommonNoteItemList.Count; i++)
            {
                List<InCommonNoteItemEntity> inCommonNoteItemEntityList = m_InCommonNoteTab.InCommonNoteItemList;
                if (i == 0)
                {
                    //记录时间
                    UCRecordDateTime uCRecordDateTime = new UCRecordDateTime(inCommonNoteItemEntityList[i].RecordDate, inCommonNoteItemEntityList[i].RecordTime);
                    point = new Point(0, 0);
                    uCRecordDateTime.Location = point;
                    uCRecordDateTime.Name = "uCRecordDateTime";
                    scorlInfo.Controls.Add(uCRecordDateTime);
                }
                if (inCommonNoteItemEntityList[i].DataElement == null)
                {
                    DataElemntBiz dataElemntBiz=new CommonTableConfig.DataElemntBiz(m_app);
                  var dateelement=  dataElemntBiz.GetDataElement(inCommonNoteItemEntityList[i].DataElementFlow);
                  inCommonNoteItemEntityList[i].DataElement = dateelement;
                }
                //项目
                ucLabText ucLabText = new ucLabText(inCommonNoteItemEntityList[i]);
                ucLabText.Height = 40;
                ucLabText.Width = 319;
                int row = (i + 1) / 3;
                int colmn = (i + 1) % 3;
                point.X = ucLabText.Width * colmn;
                point.Y = ucLabText.Height * row;
                ucLabText.Location = point;
                scorlInfo.Controls.Add(ucLabText);
                if (i == inCommonNoteItemEntityList.Count - 1)
                {
                    // 记录人
                    UCRecordDoctor uCRecordDoctor = new UCRecordDoctor(inCommonNoteItemEntityList[i].RecordDoctorName);
                    uCRecordDoctor.Height = 40;
                    uCRecordDoctor.Width = 319;
                    uCRecordDoctor.Name = "uCRecordDoctor";
                    int row1 = (i + 2) / 3;
                    int colmn1 = (i + 2) % 3;
                    point.X = uCRecordDoctor.Width * colmn1;
                    point.Y = uCRecordDoctor.Height * row1;
                    uCRecordDoctor.Location = point;
                    scorlInfo.Controls.Add(uCRecordDoctor);
                }
            }

        }

       public void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                inCommonNoteBiz.SaveIncommonType(m_InCommonNoteTab.InCommonNote_Tab_Flow, m_commonNote_TabEntity);
                Save();
            }
            catch (Exception ex)
            {

                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }

        private void Save()
        {
            UCRecordDateTime uCRecordDateTime = scorlInfo.Controls["uCRecordDateTime"] as UCRecordDateTime;
            UCRecordDoctor uCRecordDoctor = scorlInfo.Controls["uCRecordDoctor"] as UCRecordDoctor;
            string datestr = uCRecordDateTime.GetDate();
            string timestr = uCRecordDateTime.GetTime();
            string recordDoc = uCRecordDoctor.GetDoc();
            string message = "";
            foreach (var item in scorlInfo.Controls)
            {
                if (item is ucLabText)
                {
                    bool getResult = (item as ucLabText).GetInCommonNoteItemSave(ref message);
                    if (getResult == false)
                    {
                        m_app.CustomMessageBox.MessageShow(message);
                        return;
                    }
                }
            }
            if (inCommonNoteBiz == null)
                inCommonNoteBiz = new InCommonNoteBiz(m_app);
            foreach (var item in m_InCommonNoteTab.InCommonNoteItemList)
            {
                item.RecordDate = datestr;
                item.RecordTime = timestr;
                item.RecordDoctorName = recordDoc;
                inCommonNoteBiz.SaveIncommonNoteItem(item, ref message);
            }
            DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("保存成功");
            inCommonNoteBiz.GetDetaliInCommonNote(ref m_inCommonNote);
            m_InCommonNoteTab = m_inCommonNote.InCommonNoteTabList.Find(a => a.InCommonNote_Tab_Flow == m_InCommonNoteTab.InCommonNote_Tab_Flow);
            InitData();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                if(m_InCommonNoteTab==null||m_InCommonNoteTab.InCommonNoteItemList==null
                    ||string.IsNullOrEmpty(m_InCommonNoteTab.InCommonNoteItemList[0].InCommonNote_Item_Flow))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("没有数据需要清除");
                }
                DialogResult diaResult = m_app.CustomMessageBox.MessageShow("确定要清除数据吗？", DrectSoft.Core.CustomMessageBoxKind.QuestionYesNo);
                if (diaResult == DialogResult.No) return;
                //直接数据库删除
                string message = "";
                foreach (var item in m_InCommonNoteTab.InCommonNoteItemList)
                {
                    item.Valide = "0";
                    if (string.IsNullOrEmpty(item.InCommonNote_Item_Flow)) continue;
                    bool itemresult = inCommonNoteBiz.SaveIncommonNoteItem(item, ref message);
                }
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("清除成功");
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
    }
}
