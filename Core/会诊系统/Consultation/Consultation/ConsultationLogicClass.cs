using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using YidanSoft.Core.Consultation.Dal;
using YidanSoft.FrameWork.WinForm.Plugin;
using System.Windows.Forms;

namespace YidanSoft.Core.Consultation
{
    /// <summary>
    /// 会诊系统逻辑控制页面
    /// </summary>
    public class ConsultationLogicClass
    {
        ConsultationEntity m_ConsultationEntity;
        IYidanEmrHost m_app;
        /// <summary>
        /// 传入申请单单号
        /// </summary>
        public ConsultationLogicClass(string consultationApplySn,IYidanEmrHost app)
        {
            m_app = app;
            m_ConsultationEntity = GetConsultationEntity(consultationApplySn);
        }

        /// <summary>
        /// 根据申请单号获取GetConsultationEntity实体信息
        /// </summary>
        /// <param name="consultationApplySn"></param>
        /// <returns></returns>
        public ConsultationEntity GetConsultationEntity(string consultationApplySn)
        {
            if (consultationApplySn == "")
                return null;
            else
            {
                DataTable dt = DataAccess.GetConsultationTable(consultationApplySn);
                DataTable dtinp = Dal.DataAccess.GetRedactPatientInfoFrm("14", m_app.CurrentPatientInfo.NoOfFirstPage.ToString());
                foreach (DataRow dr in dt.Rows)
                {
                    m_ConsultationEntity = new ConsultationEntity();

                    m_ConsultationEntity.ConsultApplySn = consultationApplySn;
                 
                    if (dtinp.Rows.Count > 0)
                    {
                        m_ConsultationEntity.Name = dtinp.Rows[0]["NAME"].ToString().Trim();
                        m_ConsultationEntity.PatNoOfHIS = dtinp.Rows[0]["PatID"].ToString().Trim();
                        m_ConsultationEntity.SexName = dtinp.Rows[0]["Gender"].ToString().Trim();
                        m_ConsultationEntity.Age = dtinp.Rows[0]["AgeStr"].ToString().Trim();
                        m_ConsultationEntity.Bed = dtinp.Rows[0]["OutBed"].ToString().Trim();
                        m_ConsultationEntity.DeptName = dtinp.Rows[0]["OutHosDeptName"].ToString().Trim();
                        m_ConsultationEntity.WardName = dtinp.Rows[0]["outhoswardname"].ToString().Trim();
                        m_ConsultationEntity.DeptID = dtinp.Rows[0]["OutHosDept"].ToString().Trim();
                        m_ConsultationEntity.WardID = dtinp.Rows[0]["outhosward"].ToString().Trim();
                    }


                    m_ConsultationEntity.UrgencyTypeID = dr["urgencytypeid"].ToString();
                    m_ConsultationEntity.UrgencyTypeName = dr["urgencytypeName"].ToString();
                    m_ConsultationEntity.ConsultTypeID = dr["consulttypeid"].ToString();
                    m_ConsultationEntity.ConsultTypeName = dr["consulttypeName"].ToString();
                    m_ConsultationEntity.Abstract = dr["abstract"].ToString();

                    m_ConsultationEntity.Purpose = dr["purpose"].ToString();
                    m_ConsultationEntity.ApplyDeptID = dr["ApplyDeptID"].ToString();
                    m_ConsultationEntity.ApplyDeptName = dr["ApplyDeptName"].ToString();
                    m_ConsultationEntity.ApplyUserID = dr["applyuserID"].ToString();
                    m_ConsultationEntity.ApplyUserName = dr["applyuserName"].ToString();

                    m_ConsultationEntity.ApplyTime = dr["applytime"].ToString();
                    m_ConsultationEntity.ConsultSuggestion = dr["consultsuggestion"].ToString();
                    m_ConsultationEntity.ConsultDeptID = dr["ConsultDeptID"].ToString();
                    m_ConsultationEntity.ConsultDeptName = dr["ConsultDeptName"].ToString();
                    m_ConsultationEntity.ConsultHospitalID = dr["hospitalcode"].ToString();

                    m_ConsultationEntity.ConsultHospitalName = dr["ConsultHospitalName"].ToString();
                    m_ConsultationEntity.ConsultDeptID2 = dr["ConsultDeptID2"].ToString();
                    m_ConsultationEntity.ConsultDeptName2 = dr["ConsultDeptName2"].ToString();
                    m_ConsultationEntity.ConsultUserID = dr["ConsultUserID"].ToString();
                    m_ConsultationEntity.ConsultUserName = dr["ConsultUserName"].ToString();

                    m_ConsultationEntity.ConsultTime = dr["ConsultTime"].ToString();
                    m_ConsultationEntity.StateID = dr["StateID"].ToString();
                    break;
                }
                return m_ConsultationEntity;
            }
        }


        public void RefreshPage()
        {
            string noOfFirstPage = m_ConsultationEntity.NoOfInpat;
            string consultTypeID = m_ConsultationEntity.ConsultTypeID;
            string consultApplySn = m_ConsultationEntity.ConsultApplySn;
            string stateID = m_ConsultationEntity.StateID;

            if (consultTypeID == Convert.ToString((int)ConsultType.One))
            {
                //待审核 
                if (stateID == Convert.ToString((int)ConsultStatus.WaitApprove)
                    || stateID == Convert.ToString((int)ConsultStatus.WaitConsultation) //待签核 待会诊
                    || stateID == Convert.ToString((int)ConsultStatus.Reject))//否决
                {
                    FormApproveForOne formApprove = new FormApproveForOne(noOfFirstPage, m_app, consultApplySn);
                    //todo why待审签的审签信息要隐藏审核按钮?
                    //formApprove.ReadOnlyControl();
                    formApprove.StartPosition = FormStartPosition.CenterParent;
                    formApprove.ShowDialog();
                }
                else if (stateID == Convert.ToString((int)ConsultStatus.RecordeSave) || stateID == Convert.ToString((int)ConsultStatus.RecordeComplete))
                {
                    FormRecordForOne formRecord = new FormRecordForOne(noOfFirstPage, m_app, consultApplySn);
                    formRecord.ReadOnlyControl();
                    formRecord.StartPosition = FormStartPosition.CenterParent;
                    formRecord.ShowDialog();
                }
            }
            else
            {
                if (stateID == Convert.ToString((int)ConsultStatus.WaitApprove)
                    || stateID == Convert.ToString((int)ConsultStatus.WaitConsultation) //待签核 待会诊
                    || stateID == Convert.ToString((int)ConsultStatus.Reject))//否决
                {
                    FormApproveForMultiply formApprove = new FormApproveForMultiply(noOfFirstPage, m_app, consultApplySn);
                    formApprove.ReadOnlyControl();
                    formApprove.StartPosition = FormStartPosition.CenterParent;
                    formApprove.ShowDialog();
                }
                else if (stateID == Convert.ToString((int)ConsultStatus.RecordeSave) || stateID == Convert.ToString((int)ConsultStatus.RecordeComplete))
                {
                    FormRecordForMultiply formRecord = new FormRecordForMultiply(noOfFirstPage, m_app, consultApplySn);
                    //todo why待审签的审签信息要隐藏审核按钮?
                    //formRecord.ReadOnlyControl();
                    formRecord.StartPosition = FormStartPosition.CenterParent;
                    formRecord.ShowDialog();
                }
            }
        }
    }
}
