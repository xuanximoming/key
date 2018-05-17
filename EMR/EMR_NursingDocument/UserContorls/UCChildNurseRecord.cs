using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Core.EMR_NursingDocument.PublicSet;
using DrectSoft.Core.EMR_NursingDocument.PublicMethod;

namespace DrectSoft.Core.EMR_NursingDocument.UserContorls
{
    public partial class UCChildNurseRecord : DevExpress.XtraEditors.XtraUserControl
    {
        private NurseRecordBiz nurseRecordBiz;
        private List<NurseRecordEntity> entityList;

        public UCChildNurseRecord()
        {
            InitializeComponent();
        }

        private void UCChildNurseRecord_Load(object sender, EventArgs e)
        {
            if (nurseRecordBiz == null)
            {
                nurseRecordBiz = new NurseRecordBiz();
            }
            //设置标题
            DataTable dt = MethodSet.GetHospitalInfo();
            if (null != dt && dt.Rows.Count > 0)
            {
                this.lbl_title1.Text = dt.Rows[0]["NAME"].ToString();
                this.lbl_title2.Text = dt.Rows[0]["SUBNAME"].ToString();
            }
            RefreshData();
        }

        // 新增
        private void btn_add_Click(object sender, EventArgs e)
        {
            List<NurseRecordEntity> nurseRecordEntityList = gc_childNurse.DataSource as List<NurseRecordEntity>;
            if (null == nurseRecordEntityList)
            {
                nurseRecordEntityList = new List<NurseRecordEntity>();
            }
            nurseRecordEntityList.Add(new NurseRecordEntity() { FATHER_RECORDID = "3", CREATE_DOCTORID = MethodSet.App.User.DoctorId, NOOFINPATENT = MethodSet.CurrentInPatient.NoOfFirstPage.ToString() });
            gc_childNurse.DataSource = nurseRecordEntityList;
            IfOrNotShowVertScroll(nurseRecordEntityList.Count);
            gridView1.RefreshData();
            gridView1.SelectRow(nurseRecordEntityList.Count-1);
        }

        // 删除
        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (gridView1.SelectedRowsCount < 1)
            {
                MessageBox.Show("请至少选择一条记录");
                return;
            }
            if (MethodSet.App.CustomMessageBox.MessageShow("确定要删除选中的记录吗？", DrectSoft.Core.CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
            {
                NurseRecordEntity entity = gridView1.GetRow(gridView1.FocusedRowHandle) as NurseRecordEntity;
                if (null == entity)
                {
                    return;
                }
                List<NurseRecordEntity> nurseRecordEntityList = gc_childNurse.DataSource as List<NurseRecordEntity>;
                if (!string.IsNullOrEmpty(entity.ID))
                {
                    nurseRecordBiz.DelNurseRecord(entity.ID);
                }
                nurseRecordEntityList.Remove(entity);
                IfOrNotShowVertScroll(nurseRecordEntityList.Count);
                gc_childNurse.DataSource = new List<NurseRecordEntity>(nurseRecordEntityList);
                //gc_childNurse.Refresh();
            }
        }

        // 保存
        private void btn_save_Click(object sender, EventArgs e)
        {
            if (entityList.Count < 1)
            {
                MethodSet.App.CustomMessageBox.MessageShow("请先点击新增按钮录入记录");
                return;
            }
            if (new NurseService().CheckListBeforeSave(entityList))
            {
                if (MethodSet.App.CustomMessageBox.MessageShow("列表中存在空记录，您确定要保存吗？", DrectSoft.Core.CustomMessageBoxKind.QuestionYesNo) != DialogResult.Yes)
                {
                    return;
                }
            }
            SaveOperNurseRecord();
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            NurseRecordEntity nurseRecordEntity = gridView1.GetRow(gridView1.FocusedRowHandle) as NurseRecordEntity;
            if (!entityList.Contains(nurseRecordEntity))
            {
                entityList.Add(nurseRecordEntity);
            }
        }

        /// <summary>
        /// 保存记录
        /// </summary>
        private void SaveOperNurseRecord()
        {
            foreach (var item in entityList)
            {
                nurseRecordBiz.AddOrModNurseRecord(item);
            }
            MethodSet.App.CustomMessageBox.MessageShow("保存成功");
            entityList = new List<NurseRecordEntity>();
            RefreshGridViewData();
        }

        /// <summary>
        /// 刷新tab页数据
        /// </summary>
        public void RefreshData()
        {
            ///设置标题栏
            lbl_nameTxt.Text = MethodSet.CurrentInPatient.Name;
            DataTable patientInfo = PublicSet.MethodSet.GetPatientInfoForThreeMeasureTable(MethodSet.CurrentInPatient.NoOfFirstPage);
            if (patientInfo.Rows.Count > 0)
            {
                lbl_departmentTxt.Text = patientInfo.Rows[0]["dept_name"].ToString();
                lbl_bedNoTxt.Text = patientInfo.Rows[0]["outbed"].ToString();
            }
            lbl_inpNoTxt.Text = MethodSet.CurrentInPatient.RecordNoOfHospital;
            ///设置数据
            RefreshGridViewData();
        }

        /// <summary>
        /// 刷新表格数据
        /// </summary>
        public void RefreshGridViewData()
        {
            entityList = new NurseRecordBiz().GetNurseRecord(MethodSet.CurrentInPatient.NoOfFirstPage.ToString(), "3");
            IfOrNotShowVertScroll(entityList.Count);
            gc_childNurse.DataSource = entityList;
        }

        /// <summary>
        /// 显示纵向滚动条
        /// </summary>
        public void ShowVertScroll()
        {
            panel1.Width = 1157;
            panel2.Width = 1157;
            panel27.Location = new Point(1157, 206);
            gc_childNurse.Width = 1160;
            gridView1.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveVertScroll;
            gridView1.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
        }

        /// <summary>
        /// 不显示纵向滚动条
        /// </summary>
        public void NotShowVertScroll()
        {
            panel1.Width = 1142;
            panel2.Width = 1142;
            panel27.Location = new Point(1142, 206);
            gc_childNurse.Width = 1145;
            gridView1.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveVertScroll;
            gridView1.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Auto;
        }

        /// <summary>
        /// 是否显示滚动条
        /// </summary>
        /// <param name="count">记录数</param>
        public void IfOrNotShowVertScroll(int count)
        {
            if (count > 15)
            {
                ShowVertScroll();
            }
            else
            {
                NotShowVertScroll();
            }
        }

    }
}
