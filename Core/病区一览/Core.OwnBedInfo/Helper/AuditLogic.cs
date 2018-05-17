using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Common.Eop;
namespace DrectSoft.Core.OwnBedInfo.Helper
{

    public class AuditLogic
    {
        IEmrHost m_IDataAccess;
        //private string m_userid = "";//员工ID
        public AuditLogic()
        {

        }
        public AuditLogic(IEmrHost iDataAccess, string usersid)
        {
           // m_userid = usersid;
            m_IDataAccess = iDataAccess;
        }

        /// <summary>
        /// 判断当前登录人能否审核
        /// </summary>
        /// <returns></returns>
        public bool CanAudioConsult(string m_userid, Employee emp)
        {
            bool canaudio=false;
            //if (GetSuperior(m_userid).Rows.Count>0)//当前登录人是上级医师
            //{
            //    return canaudio;//有审核权限
            //}
            //if (GetSuperior(m_userid).Rows.Count<0 && GetDeptAutio(m_userid).Rows.Count>0)//没有上级医师，有科室负责人
            //{
                
            //}
            //DoctorGrade grade = (DoctorGrade)Enum.Parse(typeof(DoctorGrade), emp.Grade);
            DoctorGrade m_grade = (DoctorGrade)Enum.Parse(typeof(DoctorGrade), emp.Grade);
            string usergrade = emp.Grade.ToString();
            // 1.	主治医师以上界别医师都有会诊审核的权限。

            if (GetSuperior(m_userid).Rows.Count > 0 || GetDeptAutio(m_userid).Rows.Count > 0)
            {
                canaudio = true;
            }
            else
            {
                if (usergrade=="2000"||usergrade=="2001")
                {
                    canaudio = true;
                } 
            }
            return canaudio;
        }

        public string GetUser(string muserid)
        {
            string shenheren="";
            if (GetSuperior(muserid).Rows.Count > 0)//当前登录人是上级医师
            {
                //return canaudio;//有审核权限
                shenheren = muserid;// 返回上级医师编号
            }
            if (GetSuperior(muserid).Rows.Count < 0 && GetDeptAutio(muserid).Rows.Count > 0)//没有上级医师，有科室负责人
            {
                shenheren = GetDeptAutio(muserid).Rows[0]["userid"].ToString();//返回负责人编号
            }
            if (GetSuperior(muserid).Rows.Count> 0 && GetDeptAutio(muserid).Rows.Count > 0)//既设置上级又有科室负责人
            {
                shenheren = muserid;
            }
            if (GetSuperior(muserid).Rows.Count < 0 && GetDeptAutio(muserid).Rows.Count < 0)//两个有没设置审核人
            {
                shenheren = "";
                //string sqluser = string.Format("select * from users where deptid='{0}' and grade in ('2000','2001')",m_IDataAccess.User.CurrentDeptId);
                //shenheren = m_IDataAccess.SqlHelper.ExecuteDataTable(sqluser).Rows[0]["id"].ToString();
            }
            return shenheren;
        }

        /// <summary>
        /// 获得从上级医师
        /// </summary>
        /// <param name="id">员工ID</param>
        /// <returns></returns>
        public DataTable GetSuperior(string id)
        {
            string sql = string.Format("select * from Consult_DoctorParent where Consult_DoctorParent.parentuserid='{0}'", id);
            return m_IDataAccess.SqlHelper.ExecuteDataTable(sql);
        }
        /// <summary>
        /// 获得科室负责人
        /// </summary>
        /// <param name="id">员工ID</param>
        /// <returns></returns>
        public DataTable GetDeptAutio(string id)
        {
            string sql = string.Format("select * from Consult_DeptAutio where Consult_DeptAutio.Userid='{0}'", id);
            return m_IDataAccess.SqlHelper.ExecuteDataTable(sql);
        }


    }
}
