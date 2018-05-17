using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Common;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    /// <summary>	
    /// <title>用于选择多个单据</title>
    /// <auth>xuliangliang</auth>
    /// <date></date>
    /// </summary>
    public partial class InCommListForm : DevBaseForm
    {
        public InCommonNoteEnmtity MyInCommonNoteEnmtity;
        List<InCommonNoteEnmtity> m_InCommonNoteList;
        IEmrHost m_app;
        string m_noofinpat;
        CommonNoteEntity m_commonNoteEntity;
        public InCommListForm(List<InCommonNoteEnmtity> inCommonNoteList, IEmrHost app, string noofInpat, CommonNoteEntity commonNoteEntity)
        {
            try
            {
                InitializeComponent();
                m_app = app;
                m_InCommonNoteList = inCommonNoteList;
                m_noofinpat = noofInpat;
                m_commonNoteEntity = commonNoteEntity;
                DataTable dt = SetCommonToDataTable(m_InCommonNoteList);
                gcIncommonList.DataSource = dt;
                if (inCommonNoteList != null&& inCommonNoteList.Count > 0)
                {
                    this.Text = inCommonNoteList[0].InPatientName + "已有单据编辑";
                }
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.StackTrace);
            }
        }

        private DataTable SetCommonToDataTable(List<InCommonNoteEnmtity> inCommonNoteList)
        {
            try
            {
                DataTable dt = new DataTable();
                DataColumn dc1 = new DataColumn("InCommonNoteName", typeof(String));
                DataColumn dc2 = new DataColumn("CreateDateTime", typeof(String));
                DataColumn dc3 = new DataColumn("CreateDoctorName", typeof(String));
                DataColumn dc4 = new DataColumn("InCommonNoteFlow", typeof(String));
                dt.Columns.Add(dc1);
                dt.Columns.Add(dc2);
                dt.Columns.Add(dc3);
                dt.Columns.Add(dc4);
                foreach (var item in inCommonNoteList)
                {
                    DataRow dr = dt.NewRow();
                    dr["InCommonNoteName"] = item.InCommonNoteName;
                    dr["CreateDateTime"] = DateUtil.getDateTime(item.CreateDateTime, DateUtil.NORMAL_LONG);
                    dr["CreateDoctorName"] = item.CreateDoctorName;
                    dr["InCommonNoteFlow"] = item.InCommonNoteFlow;
                    dt.Rows.Add(dr);
                }
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



        private void InCommListForm_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                GetMyIncommon();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.StackTrace);

            }
        }

        private void GetMyIncommon()
        {
            try
            {
                DataRow dataRow = gridView1.GetFocusedDataRow();
                if (dataRow == null)
                {
                    return;
                }
                MyInCommonNoteEnmtity = m_InCommonNoteList.Find(a => a.InCommonNoteFlow == dataRow["InCommonNoteFlow"].ToString());
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GetMyIncommon();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.StackTrace);
            }
        }

        private void gvDataElement_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {

                DataRow dataRow = gridView1.GetFocusedDataRow();
                if (dataRow == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请先选中记录");
                    return;
                }
                DialogResult dResult = DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("确定要删除该条记录吗？", "删除提示", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo);
                if (dResult == DialogResult.No) return;
                InCommonNoteEnmtity IncommonNoteDel = m_InCommonNoteList.Find(a => a.InCommonNoteFlow == dataRow["InCommonNoteFlow"].ToString());
                InCommonNoteBiz inCommonNoteBiz = new InCommonNoteBiz(m_app);
                string message = "";
                bool delResult = inCommonNoteBiz.DelInCommonNote(IncommonNoteDel, ref message);
                if (delResult)
                {

                    m_InCommonNoteList.Remove(IncommonNoteDel);
                    DataTable dt = SetCommonToDataTable(m_InCommonNoteList);
                    gcIncommonList.DataSource = dt;
                }
                else
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(message);
                }
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                InCommonNoteEnmtity inCommonNoteEnmtity = ConverBycommonNote(m_commonNoteEntity);
                if (inCommonNoteEnmtity == null) return;
                InCommonNoteBiz icbiz = new InCommonNoteBiz(m_app);
                icbiz.GetDetaliInCommonNote(ref inCommonNoteEnmtity);
                m_InCommonNoteList.Add(inCommonNoteEnmtity);
                DataTable dt = SetCommonToDataTable(m_InCommonNoteList);
                gcIncommonList.DataSource = dt;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 将commonNoteEntity转成InCommonNoteEnmtity，并保存
        /// </summary>
        /// <param name="commonNoteEntity"></param>
        /// <returns></returns>
        private InCommonNoteEnmtity ConverBycommonNote(CommonNoteEntity commonNoteEntity)
        {
            try
            {
                CommonNoteBiz commonNoteBiz = new DrectSoft.Core.CommonTableConfig.CommonNoteBiz(m_app);
                commonNoteEntity = commonNoteBiz.GetDetailCommonNote(commonNoteEntity.CommonNoteFlow);
                InCommonNoteEnmtity inCommonNote = InCommonNoteBiz.ConvertCommonToInCommon(commonNoteEntity);
                InCommonNoteBiz icombiz = new DrectSoft.Core.CommonTableConfig.CommonNoteUse.InCommonNoteBiz(m_app);
                DataTable inpatientDt = icombiz.GetInpatient(m_noofinpat);
                inCommonNote.CurrDepartID = inpatientDt.Rows[0]["OUTHOSDEPT"].ToString();
                inCommonNote.CurrDepartName = inpatientDt.Rows[0]["DEPARTNAME"].ToString();
                inCommonNote.CurrWardID = inpatientDt.Rows[0]["OUTHOSWARD"].ToString();
                inCommonNote.CurrWardName = inpatientDt.Rows[0]["WARDNAME"].ToString();
                inCommonNote.NoofInpatient = m_noofinpat;
                inCommonNote.InPatientName = inpatientDt.Rows[0]["NAME"].ToString();
                string message = "";
                bool saveResult = icombiz.SaveInCommomNoteAll(inCommonNote, ref message);
                if (saveResult)
                {
                    return inCommonNote;
                }
                else
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("创建单据失败");
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 获取数据条数
        /// </summary>
        /// <returns></returns>
        public int GetDataCount()
        {
           DataTable dataTable= gcIncommonList.DataSource as DataTable;
           if (dataTable == null || dataTable.Rows == null || dataTable.Rows.Count <= 0)
           {
               return 0;
           }
           else
           {
               return dataTable.Rows.Count;
           }
        }
    }
}