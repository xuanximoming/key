using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Core.EMR_NursingDocument.PublicMethod;
using DrectSoft.Core.EMR_NursingDocument.PublicSet;

namespace DrectSoft.Core.EMR_NursingDocument.UserContorls
{
    public partial class NoOperNurseRecordForm : DevExpress.XtraEditors.XtraForm
    {
        public NoOperNurseRecordForm()
        {
            InitializeComponent();
        }

        NurseRecordBiz nurseRecordBiz;
        /// <summary>
        /// 用于保存的数据集
        /// </summary>
        List<NurseRecordEntity> nurseRecordEntityListSave;
        string fatherRecordId = "";


        public void InitDate(string fatherRecordId)
        {
            this.fatherRecordId = fatherRecordId;
            nurseRecordEntityListSave = new List<NurseRecordEntity>();
            if (nurseRecordBiz == null)
                nurseRecordBiz = new NurseRecordBiz();
            SetOperNurseRecord();
        }

        private void SetOperNurseRecord()
        {
            List<NurseRecordEntity> nurseRecordEntityList = nurseRecordBiz.GetNurseRecord(MethodSet.CurrentInPatient.NoOfFirstPage.ToString(), fatherRecordId);
            gcOperRecord.DataSource = nurseRecordEntityList;
            SetTitlelbl();
        }

        private void SetTitlelbl()
        {
            ///设置标题栏
            DataTable dtHos = MethodSet.GetHospitalInfo();
            lbl_title1.Text = dtHos.Rows[0]["Name"].ToString();
            lbl_title2.Text = dtHos.Rows[0]["SubName"].ToString();
            lbl_nameTxt.Text = MethodSet.CurrentInPatient.Name;
            DataTable patientInfo = PublicSet.MethodSet.GetPatientInfoForThreeMeasureTable(MethodSet.CurrentInPatient.NoOfFirstPage);
            if (patientInfo.Rows.Count > 0)
            {
                lbl_departmentTxt.Text = patientInfo.Rows[0]["dept_name"].ToString();
                lbl_bedNoTxt.Text = patientInfo.Rows[0]["outbed"].ToString();
            }
            lbl_inpNoTxt.Text = MethodSet.CurrentInPatient.RecordNoOfHospital;
        }

        private void SaveOperNurseRecord()
        {
            foreach (var item in nurseRecordEntityListSave)
            {
                nurseRecordBiz.AddOrModNurseRecord(item);
            }
            MethodSet.App.CustomMessageBox.MessageShow("保存成功");
            nurseRecordEntityListSave = new List<NurseRecordEntity>();
            SetOperNurseRecord();
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            NurseRecordEntity nurseRecordEntity = gridView1.GetRow(gridView1.FocusedRowHandle) as NurseRecordEntity;
            if (!nurseRecordEntityListSave.Contains(nurseRecordEntity))
                nurseRecordEntityListSave.Add(nurseRecordEntity);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            List<NurseRecordEntity> nurseRecordEntityList = gcOperRecord.DataSource as List<NurseRecordEntity>;
            if (nurseRecordEntityList == null)
                nurseRecordEntityList = new List<NurseRecordEntity>();
            NurseRecordEntity nurseRecordEntity = new PublicMethod.NurseRecordEntity();
            nurseRecordEntity.FATHER_RECORDID = this.fatherRecordId;
            nurseRecordEntity.CREATE_DOCTORID = MethodSet.App.User.DoctorId;
            nurseRecordEntity.NOOFINPATENT = MethodSet.CurrentInPatient.NoOfFirstPage.ToString();
            nurseRecordEntityList.Add(nurseRecordEntity);
            if (!nurseRecordEntityListSave.Contains(nurseRecordEntity))
                nurseRecordEntityListSave.Add(nurseRecordEntity);
            gcOperRecord.DataSource = new List<NurseRecordEntity>(nurseRecordEntityList);
            //gridView1.SelectRow(nurseRecordEntityList.Count - 1);
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            if (MethodSet.App.CustomMessageBox.MessageShow("确定要删除吗？", DrectSoft.Core.CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
            {
                NurseRecordEntity nurseRecordEntity = gridView1.GetRow(gridView1.FocusedRowHandle) as NurseRecordEntity;
                if (nurseRecordEntity == null) return;
                List<NurseRecordEntity> nurseRecordEntityList = gcOperRecord.DataSource as List<NurseRecordEntity>;
                if (!string.IsNullOrEmpty(nurseRecordEntity.ID))
                {
                    nurseRecordBiz.DelNurseRecord(nurseRecordEntity.ID);

                }
                nurseRecordEntityList.Remove(nurseRecordEntity);
                gcOperRecord.DataSource = new List<NurseRecordEntity>(nurseRecordEntityList);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if ((gcOperRecord.DataSource as List<NurseRecordEntity>).Count < 1)
            {
                MethodSet.App.CustomMessageBox.MessageShow("请先点击新增按钮录入记录");
                return;
            }
            if (new NurseService().CheckListBeforeSave(nurseRecordEntityListSave))
            {
                if (MethodSet.App.CustomMessageBox.MessageShow("列表中存在空记录，您确定要保存吗？", DrectSoft.Core.CustomMessageBoxKind.QuestionYesNo) != DialogResult.Yes)
                {
                    return;
                }
            }
            SaveOperNurseRecord();
        }

    }
}