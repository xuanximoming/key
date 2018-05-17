using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Wordbook;
using DrectSoft.Core.RedactPatientInfo.PublicSet;
using System.Data.SqlClient;

namespace DrectSoft.Core.RedactPatientInfo.UserControls
{
    public partial class UCDiacrisis : DevExpress.XtraEditors.XtraUserControl
    {        
        DataTable m_Table;
        string m_NoOfInpat;


         public UCDiacrisis( DataTable Table,string NoOfInpat) 
        {
            InitializeComponent();           
            m_Table=Table;
            m_NoOfInpat = NoOfInpat;

            
        }

         #region 初始化
         private void InitForm()
         {
             GetDiacrisis();
             if(m_Table!=null) SetValues();
         }

         #region 初始化疾病名称
         //初始化疾病名称
         private void GetDiacrisis()
         {
             lookUpWindowDiacrisis.SqlHelper = SqlUtil.App.SqlHelper;

             DataTable Diacrisis = SqlUtil.App.SqlHelper.ExecuteDataTable("usp_RedactPatientInfoFrm",
                  new SqlParameter[] { new SqlParameter("@FrmType", "16") }, CommandType.StoredProcedure);

             Diacrisis.Columns["ICD"].Caption = "诊断代码";
             Diacrisis.Columns["NAME"].Caption = "疾病名称";

             Dictionary<string, int> cols = new Dictionary<string, int>();

             cols.Add("ICD", 64);
             cols.Add("NAME", 120);

             SqlWordbook deptWordBook = new SqlWordbook("querybook", Diacrisis, "ICD", "NAME", cols);

             //MethodSet.App.PublicMethod.ConvertSqlWordbookDataSourceUpper(deptWordBook);

             lookUpEditorDiacrisis.SqlWordbook = deptWordBook;



         }
        #endregion

         #region 设置控件值
         //设置控件值
         private void SetValues()         {
             
             if (m_Table.Rows.Count <= 0) return;

             txtAdmitBed.Text = m_Table.Rows[0]["AdmitBed"].ToString();
             txtAdmitDate.Text = m_Table.Rows[0]["AdmitDate"].ToString();
             txtAdmitDept.Text = m_Table.Rows[0]["AdmitDeptName"].ToString();
             txtAdmitInfo.Text = m_Table.Rows[0]["AdmitInfoName"].ToString();
             txtAdmitWard.Text = m_Table.Rows[0]["AdmitWardName"].ToString();
             txtAdmitWay.Text = m_Table.Rows[0]["AdmitWayName"].ToString();
             txtClinicDiagnosis.Text = m_Table.Rows[0]["ClinicDiagnosisName"].ToString();
             txtClinicDoctor.Text = m_Table.Rows[0]["ClinicDoctorName"].ToString();
             txtCriticalLevel.Text = m_Table.Rows[0]["CriticalLevelName"].ToString();
             txtInCount.Text = m_Table.Rows[0]["InCount"].ToString();
             txtInWardDate.Text = m_Table.Rows[0]["InWardDate"].ToString();
             txtNoOfClinic.Text = m_Table.Rows[0]["NoOfClinic"].ToString();
             txtNoOfRecord.Text = m_Table.Rows[0]["NoOfRecord"].ToString();
             txtOrigin.Text = m_Table.Rows[0]["OriginName"].ToString();
             txtOutBed.Text = m_Table.Rows[0]["OutBed"].ToString();
             txtOutHosDate.Text = m_Table.Rows[0]["OutHosDate"].ToString();
             txtOutHosDept.Text = m_Table.Rows[0]["OutHosDeptName"].ToString();
             txtOutHosWard.Text = m_Table.Rows[0]["OutHosWardName"].ToString();
             txtOutWardDate.Text = m_Table.Rows[0]["OutWardDate"].ToString();
             txtPatID.Text = m_Table.Rows[0]["PatID"].ToString();
             txtStatus.Text = m_Table.Rows[0]["StatusName"].ToString();
             txtStyle.Text = m_Table.Rows[0]["StyleName"].ToString();

             lookUpEditorDiacrisis.CodeValue = m_Table.Rows[0]["AdmitDiagnosis"].ToString();
         }
        #endregion

         #endregion

         #region 保存数据
         public void SaveUCDiacrisisInfo()
         {
             try
             {
                 if (SqlUtil.App.CustomMessageBox.MessageShow("确定修改当前记录吗？", CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
                 {
                     SqlUtil.App.SqlHelper.ExecuteNoneQuery("usp_UpDataDiacrisisInfo"
                         , new SqlParameter[] 
                        { 
                            new SqlParameter("@NoOfInpat", m_NoOfInpat),
                            new SqlParameter("@AdmitDiagnosis", lookUpEditorDiacrisis.CodeValue.ToString())
                        }
                         , CommandType.StoredProcedure);

                     SqlUtil.App.CustomMessageBox.MessageShow("保存成功.");
                 }
             }
             catch (Exception ex)
             {
                 SqlUtil.App.CustomMessageBox.MessageShow("保存失败!\n详细错误："+ex.Message);
             }

         }
        #endregion


         private void UCDiacrisis_Load(object sender, EventArgs e)
         {
             InitForm();
         }

    }
}
