using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections.ObjectModel;

namespace DrectSoft.Core.TimeLimitQC
{
    /// <summary>
    /// 用于检索医生分管病人信息
    /// </summary>
    public class DoctorManagerPatientInfoDal
    {
        IDataAccess _sqlHelper;
        const string CstSelectPatientsIncludeSub = 
            " select NoOfInpat, PatID, Name, Resident, Attend, Chief  from InPatient "
            + " where Chief =@UserId or Attend =@UserId or Resident=@UserId ";
        const string CstSelectPatientsOnlySelf  =
            " select NoOfInpat, PatID, Name, Resident, Attend, Chief  from InPatient "
            + " where Resident=@UserId ";
        const string CstSelectPatientBySyxhYsdm =
            " select NoOfInpat, PatID, Name, Resident, Attend, Chief  from InPatient "
            + " where NoOfInpat=@NoOfInpat and (@Resident=@UserId or Attend =@UserId or Chief =@UserId) ";
        const string CstSelectPatientBySyxh =
            " select NoOfInpat, PatID, Name, Resident, Attend, Chief  from InPatient "
            + " where NoOfInpat=@NoOfInpat";

        /// <summary>
        /// 构造检索类,创建数据访问
        /// </summary>
        public DoctorManagerPatientInfoDal()
        { 
            _sqlHelper = DataAccessFactory.GetSqlDataAccess();
        }

        /// <summary>
        /// 取得指定的病人
        /// </summary>
        /// <param name="patid"></param>
        /// <returns></returns>
        public DoctorManagerPatient SelectDoctorManagerPatient(int patid) 
        {
            SqlParameter paramSyxh = new SqlParameter("NoOfInpat", SqlDbType.Int);
            paramSyxh.Value = patid;
            DataTable doctorzPatients = _sqlHelper.ExecuteDataTable(
                CstSelectPatientBySyxh, new SqlParameter[] { paramSyxh });
            if (doctorzPatients == null || doctorzPatients.Rows.Count == 0)
                return null;
            else
            {
                DataRow dr = doctorzPatients.Rows[0];
                return new DoctorManagerPatient(int.Parse(dr["NoOfInpat"].ToString()),
                                dr["Name"].ToString(),
                                dr["PatID"].ToString(),
                                dr["Resident"].ToString(),
                                dr["Attend"].ToString(),
                                dr["Chief"].ToString());
            }
        }

        /// <summary>
        /// 取得当前医生分管的病人列表(包括下级医生)
        /// <param name="doctorid"></param>
        /// <param name="includeSubDoctor"></param>
        /// </summary>
        public Collection<DoctorManagerPatient> SelectDoctorPatients(string doctorid, bool includeSubDoctor, int NoOfInpat)
        {
            Collection<DoctorManagerPatient> cdmps = new Collection<DoctorManagerPatient>();
            DataTable doctorzPatients = null;
            SqlParameter paramYsdm = new SqlParameter("UserId", SqlDbType.VarChar, 6);
            paramYsdm.Value = doctorid;
            if (NoOfInpat > 0)
            {
                SqlParameter paramSyxh = new SqlParameter("NoOfInpat", SqlDbType.Int);
                paramSyxh.Value = NoOfInpat;
                doctorzPatients = _sqlHelper.ExecuteDataTable(
                    CstSelectPatientBySyxhYsdm, new SqlParameter[] { paramYsdm, paramSyxh});
            }
            else
            {
                string sql = CstSelectPatientsOnlySelf;
                if (includeSubDoctor) sql = CstSelectPatientsIncludeSub;
                doctorzPatients = _sqlHelper.ExecuteDataTable(
                    sql, new SqlParameter[] { paramYsdm });
            }   
            if (doctorzPatients != null)
            {
                for (int i = 0; i < doctorzPatients.Rows.Count; i++)
                {
                    DataRow dr = doctorzPatients.Rows[i];
                    DoctorManagerPatient dmp =
                        new DoctorManagerPatient(int.Parse(dr["NoOfInpat"].ToString()),
                                    dr["Name"].ToString(),
                                    dr["PatID"].ToString(),
                                    dr["Resident"].ToString(),
                                    dr["Attend"].ToString(),
                                    dr["Chief "].ToString());
                    cdmps.Add(dmp);
                }
            }

            return cdmps;
        }
    }
}
