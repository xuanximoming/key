using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.SysTableEdit.DataEntity;
using System.Data.SqlClient;
using System.Data;

namespace DrectSoft.SysTableEdit
{
    public enum EditState
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 1,
        /// <summary>
        /// 新增
        /// </summary>
        Add = 2,
        /// <summary>
        /// 编辑
        /// </summary>
        Edit = 4,
        /// <summary>
        /// 视图
        /// </summary>
        View = 8
    }

    public class SysTableManger
    {
        IEmrHost m_app;
        public SysTableManger(IEmrHost app)
        {
            m_app = app;
        }

        #region 操作诊断库 Diagnosis
        /// <summary>
        /// 保存诊断库数据，返回新的诊断库编号
        /// </summary>
        /// <param name="emrtemplet"></param>
        /// <param name="editType">1:新增   2：修改</param>
        /// <returns></returns>
        public string SaveDiagnosis(Diagnosis diagnosis, string editType)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@EditType",SqlDbType.VarChar),
                    new SqlParameter("@MarkId",SqlDbType.VarChar),
                    new SqlParameter("@ICD",SqlDbType.VarChar),
                    new SqlParameter("@MapID",SqlDbType.VarChar),
                    new SqlParameter("@StandardCode",SqlDbType.VarChar),

                    new SqlParameter("@Name",SqlDbType.VarChar),
                    new SqlParameter("@Py",SqlDbType.VarChar),
                    new SqlParameter("@Wb",SqlDbType.VarChar),
                    new SqlParameter("@TumorID",SqlDbType.VarChar),
                    new SqlParameter("@Statist",SqlDbType.VarChar),

                    new SqlParameter("@InnerCategory",SqlDbType.VarChar),
                    new SqlParameter("@Category",SqlDbType.VarChar),
                    new SqlParameter("@OtherCategroy",SqlDbType.VarChar),
                    new SqlParameter("@Valid",SqlDbType.Int),
                    new SqlParameter("@Memo",SqlDbType.VarChar)
                };

            sqlParam[0].Value = editType;
            sqlParam[1].Value = diagnosis.Markid;
            sqlParam[2].Value = diagnosis.Icd;
            sqlParam[3].Value = diagnosis.Mapid;
            sqlParam[4].Value = diagnosis.Standardcode;

            sqlParam[5].Value = diagnosis.Name;
            sqlParam[6].Value = diagnosis.Py;
            sqlParam[7].Value = diagnosis.Wb;
            sqlParam[8].Value = diagnosis.Tumorid;
            sqlParam[9].Value = diagnosis.Statist;

            sqlParam[10].Value = diagnosis.Innercategory;
            sqlParam[11].Value = diagnosis.Category;
            sqlParam[12].Value = diagnosis.Othercategroy;
            sqlParam[13].Value = diagnosis.Valid;
            sqlParam[14].Value = diagnosis.Memo; 

            return m_app.SqlHelper.ExecuteDataSet("EmrSysTable.usp_Edit_Diagnosis", sqlParam, CommandType.StoredProcedure).Tables[0].Rows[0][0].ToString();
        }

        /// <summary>
        /// 根据诊断库MarkID获诊断库实体
        /// </summary>
        /// <param name="markId"></param>
        public Diagnosis GetDiagnosis(string markId)
        {
            Diagnosis diagnosis = new Diagnosis();

            DataTable dt = GetDiagnosis_Table(markId);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                diagnosis.Markid = dr["Markid"].ToString();
                diagnosis.Icd = dr["Icd"].ToString();
                diagnosis.Mapid = dr["Mapid"].ToString();
                diagnosis.Standardcode = dr["Standardcode"].ToString();
                diagnosis.Name = dr["Name"].ToString();

                diagnosis.Py = dr["Py"].ToString();
                diagnosis.Wb = dr["Wb"].ToString();
                diagnosis.Tumorid = dr["Tumorid"].ToString();
                diagnosis.Statist = dr["Statist"].ToString();
                diagnosis.Innercategory = dr["Innercategory"].ToString();

                diagnosis.Category = dr["Category"].ToString();
                diagnosis.Othercategroy = dr["Othercategroy"].ToString();
                diagnosis.Valid = Convert.ToInt32(dr["Valid"].ToString());
                diagnosis.Memo = dr["Memo"].ToString();

 
            }
            return diagnosis;
        }

        /// <summary>
        /// 根据诊断库MarkID获取诊断库信息
        /// </summary>
        /// <param name="markId"></param>
        public DataTable GetDiagnosis_Table(string markId)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            {
                new SqlParameter("@EditType",SqlDbType.VarChar),
                new SqlParameter("@MarkId",SqlDbType.VarChar)
            };
            sqlParam[0].Value = "4";
            sqlParam[1].Value = markId;
            DataTable dt = m_app.SqlHelper.ExecuteDataSet("EmrSysTable.usp_Edit_Diagnosis", sqlParam, CommandType.StoredProcedure).Tables[0];

            return dt;
        }

        /// <summary>
        /// 根据科室编号获取科室常用诊断库信息
        /// </summary>
        /// <param name="markId"></param>
        public DataTable GetDeptDiagnosis_Table(string dept)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            {
                new SqlParameter("@EditType",SqlDbType.VarChar),
                new SqlParameter("@DeptID",SqlDbType.VarChar)
            };
            sqlParam[0].Value = "2";
            sqlParam[1].Value = dept;
            DataTable dt = m_app.SqlHelper.ExecuteDataSet("EmrSysTable.usp_Edit_DeptDiagnosis", sqlParam, CommandType.StoredProcedure).Tables[0];

            return dt;
        }

        /// <summary>
        /// 根据传入的科室编号以及markid保存科室常用诊断信息
        /// </summary>
        /// <param name="markId"></param>
        public bool DoSaveDeptDiag(string deptid,string markid)
        {
            try
            {
                SqlParameter[] sqlParam = new SqlParameter[] 
            {
                new SqlParameter("@EditType",SqlDbType.VarChar),
                new SqlParameter("@DeptID",SqlDbType.VarChar),
                new SqlParameter("@MarkId",SqlDbType.VarChar)
            };
                sqlParam[0].Value = "1";
                sqlParam[1].Value = deptid;
                sqlParam[2].Value = markid;
                DataTable dt = m_app.SqlHelper.ExecuteDataTable("EmrSysTable.usp_Edit_DeptDiagnosis", sqlParam, CommandType.StoredProcedure);
                if (dt != null && dt.Rows[0][0].ToString() == "1")
                    return true;
                else
                    return false;
            }
            catch (Exception err)
            {
                return false;
            }
        }

        /// <summary>
        /// 根据传入诊断实体删除对应数据
        /// </summary>
        /// <param name="emrtemplet"></param>
        /// <returns></returns>
        public bool DelDiagnosis(Diagnosis diag)
        {
            return DelDiagnosis(diag.Markid );
        }

        /// <summary>
        /// 根据传入诊断标识删除对应数据
        /// </summary>
        /// <param name="emrtemplet"></param>
        /// <returns></returns>
        public bool DelDiagnosis(string Markid)
        {
            try
            {
                SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@EditType",SqlDbType.VarChar),
                    new SqlParameter("@MarkId",SqlDbType.VarChar)
                };
                sqlParam[0].Value = "3";
                sqlParam[1].Value = Markid;
                m_app.SqlHelper.ExecuteDataSet("EmrSysTable.usp_Edit_Diagnosis", sqlParam, CommandType.StoredProcedure);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 新增之前验证编号是否已经存在
        /// </summary>
        /// <param name="Markid"></param>
        /// <returns></returns>
        public bool CheckDiagnosisID(string Markid)
        {
            DataTable dt = GetDiagnosis_Table(Markid);
            if (dt.Rows.Count > 0)
            {
                return false;
            }
            return true;
        }
        #endregion


        #region 操作中医诊断库 DiagnosisOfChinese
        /// <summary>
        /// 保存中医诊断库信息
        /// </summary>
        /// <param name="diagnosis">中医诊断库实体</param>
        /// <param name="editType">1:新增   2：修改</param>
        /// <returns></returns>
        public string SaveDiagnosisOfChinese(DiagnosisOfChinese diagnosis, string editType)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@EditType",SqlDbType.VarChar),
                    new SqlParameter("@ID",SqlDbType.VarChar),
                    new SqlParameter("@MapID",SqlDbType.VarChar),
                    new SqlParameter("@Name",SqlDbType.VarChar),
                    new SqlParameter("@Py",SqlDbType.VarChar),

                    new SqlParameter("@Wb",SqlDbType.VarChar),
                    new SqlParameter("@Valid",SqlDbType.VarChar),
                    new SqlParameter("@Memo",SqlDbType.VarChar),
                    new SqlParameter("@Memo1",SqlDbType.VarChar),
                    new SqlParameter("@Category",SqlDbType.VarChar) 
                };

            sqlParam[0].Value = editType;
            sqlParam[1].Value = diagnosis.Id;
            sqlParam[2].Value = diagnosis.Mapid;
            sqlParam[3].Value = diagnosis.Name;
            sqlParam[4].Value = diagnosis.Py;

            sqlParam[5].Value = diagnosis.Wb;
            sqlParam[6].Value = diagnosis.Valid;
            sqlParam[7].Value = diagnosis.Memo;
            sqlParam[8].Value = diagnosis.Memo1;
            sqlParam[9].Value = diagnosis.Category;


            return m_app.SqlHelper.ExecuteDataSet("EmrSysTable.usp_Edit_DiagnosisOfChinese", sqlParam, CommandType.StoredProcedure).Tables[0].Rows[0][0].ToString();
        }

        /// <summary>
        /// 根据模板ID获取模板信息
        /// </summary>
        /// <param name="templet_id"></param>
        public DiagnosisOfChinese GetDiagnosisOfChinese(string markId)
        {
            DiagnosisOfChinese diagnosis = new DiagnosisOfChinese();

            DataTable dt = GetDiagnosisOfChinese_Table(markId);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                diagnosis.Id = dr["Id"].ToString();
                diagnosis.Mapid = dr["Mapid"].ToString();
                diagnosis.Name = dr["Name"].ToString();
                diagnosis.Py = dr["Py"].ToString();
                diagnosis.Wb = dr["Wb"].ToString();

                diagnosis.Valid = Convert.ToInt32(dr["Valid"].ToString());
                diagnosis.Memo = dr["Memo"].ToString();
                diagnosis.Memo1 = dr["Memo1"].ToString();
                diagnosis.Category = dr["Category"].ToString();
            }
            return diagnosis;
        }

        /// <summary>
        /// 根据中医诊断库ID查询诊断库信息
        /// </summary>
        /// <param name="id">等于“”查询所有信息</param>
        public DataTable GetDiagnosisOfChinese_Table(string id)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            {
                new SqlParameter("@EditType",SqlDbType.VarChar),
                new SqlParameter("@ID",SqlDbType.VarChar)
            };
            sqlParam[0].Value = "4";
            sqlParam[1].Value = id;
            DataTable dt = m_app.SqlHelper.ExecuteDataSet("EmrSysTable.usp_Edit_DiagnosisOfChinese", sqlParam, CommandType.StoredProcedure).Tables[0];

            return dt;
        }

        /// <summary>
        /// 根据传入诊断实体删除对应数据
        /// </summary>
        /// <param name="emrtemplet"></param>
        /// <returns></returns>
        public bool DelDiagnosisOfChinese(DiagnosisOfChinese diag)
        {
            return DelDiagnosisOfChinese(diag.Id);
        }

        /// <summary>
        /// 根据传入中医诊断库ID删除对应数据
        /// </summary>
        /// <param name="emrtemplet"></param>
        /// <returns></returns>
        public bool DelDiagnosisOfChinese(string id)
        {
            try
            {
                SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@EditType",SqlDbType.VarChar),
                    new SqlParameter("@ID",SqlDbType.VarChar)
                };
                sqlParam[0].Value = "3";
                sqlParam[1].Value = id;
                m_app.SqlHelper.ExecuteDataSet("EmrSysTable.usp_Edit_DiagnosisOfChinese", sqlParam, CommandType.StoredProcedure);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 新增之前验证编号是否已经存在
        /// </summary>
        /// <param name="Markid"></param>
        /// <returns></returns>
        public bool CheckDiagnosisOfChineseID(string id)
        {
            DataTable dt = GetDiagnosisOfChinese_Table(id);
            if (dt.Rows.Count > 0)
            {
                return false;
            }
            return true;
        }
        #endregion


        #region 操作手术代码库 DiseaseCFG
        /// <summary>
        /// 保存病种设置库
        /// </summary>
        /// <param name="diagnosis">手术代码库实体</param>
        /// <param name="editType">1:新增   2：修改</param>
        /// <returns></returns>
        public string SaveDiseaseCFG(DiseaseCFG diseaseCFG, string editType)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@EditType",SqlDbType.VarChar),
                    new SqlParameter("@ID",SqlDbType.VarChar),
                    new SqlParameter("@MapID",SqlDbType.VarChar),
                    new SqlParameter("@Name",SqlDbType.VarChar),
                    new SqlParameter("@Py",SqlDbType.VarChar),

                    new SqlParameter("@Wb",SqlDbType.VarChar),
                    new SqlParameter("@DiseaseID",SqlDbType.VarChar),
                    new SqlParameter("@SurgeryID",SqlDbType.VarChar),
                    new SqlParameter("@Category",SqlDbType.VarChar),
                    new SqlParameter("@Mark",SqlDbType.VarChar),

                    new SqlParameter("@ParentID",SqlDbType.VarChar),
                    new SqlParameter("@Valid",SqlDbType.VarChar),
                    new SqlParameter("@Memo",SqlDbType.VarChar) 
                };

            sqlParam[0].Value = editType;
            sqlParam[1].Value = diseaseCFG.Id;
            sqlParam[2].Value = diseaseCFG.Mapid;
            sqlParam[3].Value = diseaseCFG.Name;
            sqlParam[4].Value = diseaseCFG.Py;

            sqlParam[5].Value = diseaseCFG.Wb;
            sqlParam[6].Value = diseaseCFG.Diseaseid;
            sqlParam[7].Value = diseaseCFG.Surgeryid;
            sqlParam[8].Value = diseaseCFG.Category;
            sqlParam[9].Value = diseaseCFG.Mark;

            sqlParam[10].Value = diseaseCFG.Parentid;
            sqlParam[11].Value = diseaseCFG.Valid;
            sqlParam[12].Value = diseaseCFG.Memo;


            return m_app.SqlHelper.ExecuteDataSet("EmrSysTable.usp_Edit_DiseaseCFG", sqlParam, CommandType.StoredProcedure).Tables[0].Rows[0][0].ToString();
        }

        /// <summary>
        /// 根据病种设置库ID获取病种设置库实体信息
        /// </summary>
        /// <param name="templet_id"></param>
        public DiseaseCFG GetDiseaseCFG(string markId)
        {
            DiseaseCFG diseaseCFG = new DiseaseCFG();

            DataTable dt = GetDiagnosisOfChinese_Table(markId);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                diseaseCFG.Id = dr["Id"].ToString();
                diseaseCFG.Mapid = dr["Mapid"].ToString();
                diseaseCFG.Name = dr["Name"].ToString();
                diseaseCFG.Py = dr["Py"].ToString();
                diseaseCFG.Wb = dr["Wb"].ToString();

                diseaseCFG.Diseaseid = dr["Diseaseid"].ToString();
                diseaseCFG.Surgeryid = dr["Surgeryid"].ToString();
                diseaseCFG.Category = dr["Category"].ToString();
                diseaseCFG.Mark = dr["Mark"].ToString();

                diseaseCFG.Parentid = dr["Parentid"].ToString();
                diseaseCFG.Valid = Convert.ToInt32(dr["Valid"].ToString());
                diseaseCFG.Memo = dr["Memo"].ToString();
            }
            return diseaseCFG;
        }

        /// <summary>
        /// 根据病种设置库ID查询诊断库信息
        /// </summary>
        /// <param name="id">等于“”查询所有信息</param>
        public DataTable GetDiseaseCFG_Table(string id)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            {
                new SqlParameter("@EditType",SqlDbType.VarChar),
                new SqlParameter("@ID",SqlDbType.VarChar)
            };
            sqlParam[0].Value = "4";
            sqlParam[1].Value = id;
            DataTable dt = m_app.SqlHelper.ExecuteDataSet("EmrSysTable.usp_Edit_DiseaseCFG", sqlParam, CommandType.StoredProcedure).Tables[0];

            return dt;
        }

        /// <summary>
        /// 根据传入病种设置库实体删除对应数据
        /// </summary>
        /// <param name="emrtemplet"></param>
        /// <returns></returns>
        public bool DelDiseaseCFG(DiseaseCFG diseaseCFG)
        {
            return DelDiseaseCFG(diseaseCFG.Id);
        }

        /// <summary>
        /// 根据传入病种设置库ID删除对应数据
        /// </summary>
        /// <param name="emrtemplet"></param>
        /// <returns></returns>
        public bool DelDiseaseCFG(string id)
        {
            try
            {
                SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@EditType",SqlDbType.VarChar),
                    new SqlParameter("@ID",SqlDbType.VarChar)
                };
                sqlParam[0].Value = "3";
                sqlParam[1].Value = id;
                m_app.SqlHelper.ExecuteDataSet("EmrSysTable.usp_Edit_DiseaseCFG", sqlParam, CommandType.StoredProcedure);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 新增之前验证编号是否已经存在
        /// </summary>
        /// <param name="Markid"></param>
        /// <returns></returns>
        public bool CheckDiseaseCFGID(string id)
        {
            DataTable dt = GetDiseaseCFG_Table(id);
            if (dt.Rows.Count > 0)
            {
                return false;
            }
            return true;
        }

        #endregion


        #region 操作手术代码库 Surgery
        /// <summary>
        /// 保存手术代码库
        /// </summary>
        /// <param name="diagnosis">手术代码库实体</param>
        /// <param name="editType">1:新增   2：修改</param>
        /// <returns></returns>
        public string SaveSurgery(Surgery surgery, string editType)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@EditType",SqlDbType.VarChar),
                    new SqlParameter("@ID",SqlDbType.VarChar),
                    new SqlParameter("@MapID",SqlDbType.VarChar),
                    new SqlParameter("@StandardCode",SqlDbType.VarChar),
                    new SqlParameter("@Name",SqlDbType.VarChar),

                    new SqlParameter("@Py",SqlDbType.VarChar),
                    new SqlParameter("@Wb",SqlDbType.VarChar),
                    new SqlParameter("@Valid",SqlDbType.VarChar),
                    new SqlParameter("@Memo",SqlDbType.VarChar),
                    new SqlParameter("@bzlb",SqlDbType.VarChar),

                    new SqlParameter("@sslb",SqlDbType.VarChar)
                };

            sqlParam[0].Value = editType;
            sqlParam[1].Value = surgery.Id;
            sqlParam[2].Value = surgery.Mapid;
            sqlParam[3].Value = surgery.Standardcode;
            sqlParam[4].Value = surgery.Name;

            sqlParam[5].Value = surgery.Py;
            sqlParam[6].Value = surgery.Wb;
            sqlParam[7].Value = surgery.Valid;
            sqlParam[8].Value = surgery.Memo;
            sqlParam[9].Value = surgery.Bzlb;

            sqlParam[10].Value = surgery.Sslb;


            return m_app.SqlHelper.ExecuteDataSet("EmrSysTable.usp_Edit_Surgery", sqlParam, CommandType.StoredProcedure).Tables[0].Rows[0][0].ToString();
        }

        /// <summary>
        /// 根据手术代码库ID获取手术代码库实体信息
        /// </summary>
        /// <param name="templet_id"></param>
        public Surgery GetSurgery(string markId)
        {
            Surgery surgery = new Surgery();

            DataTable dt = GetDiagnosisOfChinese_Table(markId);
            if (dt.Rows.Count > 0)
            {
  
                DataRow dr = dt.Rows[0];
                surgery.Id = dr["Id"].ToString();
                surgery.Standardcode = dr["Standardcode"].ToString();
                surgery.Mapid = dr["Mapid"].ToString();
                surgery.Name = dr["Name"].ToString();
                surgery.Py = dr["Py"].ToString();

                surgery.Wb = dr["Wb"].ToString();
                surgery.Valid = Convert.ToInt32(dr["Valid"].ToString());
                surgery.Memo = dr["Memo"].ToString();
                surgery.Bzlb = dr["Bzlb"].ToString();
                surgery.Sslb = Convert.ToInt32(dr["Sslb"].ToString()); 
            }
            return surgery;
        }

        /// <summary>
        /// 根据手术代码库ID查询诊断库信息
        /// </summary>
        /// <param name="id">等于“”查询所有信息</param>
        public DataTable GetSurgery_Table(string id)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            {
                new SqlParameter("@EditType",SqlDbType.VarChar),
                new SqlParameter("@ID",SqlDbType.VarChar)
            };
            sqlParam[0].Value = "4";
            sqlParam[1].Value = id;
            DataTable dt = m_app.SqlHelper.ExecuteDataSet("EmrSysTable.usp_Edit_Surgery", sqlParam, CommandType.StoredProcedure).Tables[0];

            return dt;
        }

        /// <summary>
        /// 根据传入手术代码库实体删除对应数据
        /// </summary>
        /// <param name="emrtemplet"></param>
        /// <returns></returns>
        public bool DelSurgery(Surgery surgery)
        {
            return DelSurgery(surgery.Id);
        }

        /// <summary>
        /// 根据传入手术代码库ID删除对应数据
        /// </summary>
        /// <param name="emrtemplet"></param>
        /// <returns></returns>
        public bool DelSurgery(string id)
        {
            try
            {
                SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@EditType",SqlDbType.VarChar),
                    new SqlParameter("@ID",SqlDbType.VarChar)
                };
                sqlParam[0].Value = "3";
                sqlParam[1].Value = id;
                m_app.SqlHelper.ExecuteDataSet("EmrSysTable.usp_Edit_Surgery", sqlParam, CommandType.StoredProcedure);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 新增之前验证编号是否已经存在
        /// </summary>
        /// <param name="Markid"></param>
        /// <returns></returns>
        public bool CheckSurgeryID(string id)
        {
            DataTable dt = GetSurgery_Table(id);
            if (dt.Rows.Count > 0)
            {
                return false;
            }
            return true;
        }

        #endregion


        #region 操作损伤中毒库 Toxicosis
        /// <summary>
        /// 保存损伤中毒库
        /// </summary>
        /// <param name="diagnosis">损伤中毒库实体</param>
        /// <param name="editType">1:新增   2：修改</param>
        /// <returns></returns>
        public string SaveToxicosis(Toxicosis toxicosis, string editType)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@EditType",SqlDbType.VarChar),
                    new SqlParameter("@ID",SqlDbType.VarChar),
                    new SqlParameter("@MapID",SqlDbType.VarChar),
                    new SqlParameter("@StandardCode",SqlDbType.VarChar),
                    new SqlParameter("@Name",SqlDbType.VarChar),

                    new SqlParameter("@Py",SqlDbType.VarChar),
                    new SqlParameter("@Wb",SqlDbType.VarChar),
                    new SqlParameter("@Valid",SqlDbType.VarChar),
                    new SqlParameter("@Memo",SqlDbType.VarChar)
                };

            sqlParam[0].Value = editType;
            sqlParam[1].Value = toxicosis.Id;
            sqlParam[2].Value = toxicosis.Mapid;
            sqlParam[3].Value = toxicosis.Standardcode;
            sqlParam[4].Value = toxicosis.Name;

            sqlParam[5].Value = toxicosis.Py;
            sqlParam[6].Value = toxicosis.Wb;
            sqlParam[7].Value = toxicosis.Valid;
            sqlParam[8].Value = toxicosis.Memo;



            return m_app.SqlHelper.ExecuteDataSet("EmrSysTable.usp_Edit_Toxicosis", sqlParam, CommandType.StoredProcedure).Tables[0].Rows[0][0].ToString();
        }

        /// <summary>
        /// 根据损伤中毒库ID获取损伤中毒库实体信息
        /// </summary>
        /// <param name="templet_id"></param>
        public Toxicosis GetToxicosis(string markId)
        {
            Toxicosis toxicosis = new Toxicosis();

            DataTable dt = GetDiagnosisOfChinese_Table(markId);
            if (dt.Rows.Count > 0)
            {

                DataRow dr = dt.Rows[0];
                toxicosis.Id = dr["Id"].ToString();
                toxicosis.Mapid = dr["Mapid"].ToString();
                toxicosis.Standardcode = dr["Standardcode"].ToString();
                toxicosis.Name = dr["Name"].ToString();
                toxicosis.Name = dr["Name"].ToString();

                toxicosis.Py = dr["Py"].ToString();
                toxicosis.Wb = dr["Wb"].ToString();
                toxicosis.Valid = Convert.ToInt32(dr["Valid"].ToString());
                toxicosis.Memo = dr["Memo"].ToString();
            }
            return toxicosis;
        }

        /// <summary>
        /// 根据损伤中毒库ID查询诊断库信息
        /// </summary>
        /// <param name="id">等于“”查询所有信息</param>
        public DataTable GetToxicosis_Table(string id)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            {
                new SqlParameter("@EditType",SqlDbType.VarChar),
                new SqlParameter("@ID",SqlDbType.VarChar)
            };
            sqlParam[0].Value = "4";
            sqlParam[1].Value = id;
            DataTable dt = m_app.SqlHelper.ExecuteDataSet("EmrSysTable.usp_Edit_Toxicosis", sqlParam, CommandType.StoredProcedure).Tables[0];

            return dt;
        }

        /// <summary>
        /// 根据传入损伤中毒库实体删除对应数据
        /// </summary>
        /// <param name="Toxicosis"></param>
        /// <returns></returns>
        public bool DelToxicosis(Toxicosis toxicosis)
        {
            return DelToxicosis(toxicosis.Id);
        }

        /// <summary>
        /// 根据传入损伤中毒库ID删除对应数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DelToxicosis(string id)
        {
            try
            {
                SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@EditType",SqlDbType.VarChar),
                    new SqlParameter("@ID",SqlDbType.VarChar)
                };
                sqlParam[0].Value = "3";
                sqlParam[1].Value = id;
                m_app.SqlHelper.ExecuteDataSet("EmrSysTable.usp_Edit_Toxicosis", sqlParam, CommandType.StoredProcedure);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 新增之前验证编号是否已经存在
        /// </summary>
        /// <param name="Markid"></param>
        /// <returns></returns>
        public bool CheckToxicosisID(string id)
        {
            DataTable dt = GetToxicosis_Table(id);
            if (dt.Rows.Count > 0)
            {
                return false;
            }
            return true;
        }

        #endregion


        #region 操作肿瘤库 Tumor
        /// <summary>
        /// 保存肿瘤库
        /// </summary>
        /// <param name="diagnosis">肿瘤库实体</param>
        /// <param name="editType">1:新增   2：修改</param>
        /// <returns></returns>
        public string SaveTumor(Tumor tumor, string editType)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@EditType",SqlDbType.VarChar),
                    new SqlParameter("@ID",SqlDbType.VarChar),
                    new SqlParameter("@MapID",SqlDbType.VarChar),
                    new SqlParameter("@StandardCode",SqlDbType.VarChar),
                    new SqlParameter("@Name",SqlDbType.VarChar),

                    new SqlParameter("@Py",SqlDbType.VarChar),
                    new SqlParameter("@Wb",SqlDbType.VarChar),
                    new SqlParameter("@Valid",SqlDbType.VarChar),
                    new SqlParameter("@Memo",SqlDbType.VarChar)
                };

            sqlParam[0].Value = editType;
            sqlParam[1].Value = tumor.Id;
            sqlParam[2].Value = tumor.Mapid;
            sqlParam[3].Value = tumor.Standardcode;
            sqlParam[4].Value = tumor.Name;

            sqlParam[5].Value = tumor.Py;
            sqlParam[6].Value = tumor.Wb;
            sqlParam[7].Value = tumor.Valid;
            sqlParam[8].Value = tumor.Memo;



            return m_app.SqlHelper.ExecuteDataSet("EmrSysTable.usp_Edit_Tumor", sqlParam, CommandType.StoredProcedure).Tables[0].Rows[0][0].ToString();
        }

        /// <summary>
        /// 根据肿瘤库ID获取肿瘤库实体信息
        /// </summary>
        /// <param name="templet_id"></param>
        public Tumor GetTumor(string markId)
        {
            Tumor tumor = new Tumor();

            DataTable dt = GetDiagnosisOfChinese_Table(markId);
            if (dt.Rows.Count > 0)
            {

                DataRow dr = dt.Rows[0];
                tumor.Id = dr["Id"].ToString();
                tumor.Mapid = dr["Mapid"].ToString();
                tumor.Standardcode = dr["Standardcode"].ToString();
                tumor.Name = dr["Name"].ToString();
                tumor.Name = dr["Name"].ToString();

                tumor.Py = dr["Py"].ToString();
                tumor.Wb = dr["Wb"].ToString();
                tumor.Valid = Convert.ToInt32(dr["Valid"].ToString());
                tumor.Memo = dr["Memo"].ToString();
            }
            return tumor;
        }

        /// <summary>
        /// 根据肿瘤库ID查询诊断库信息
        /// </summary>
        /// <param name="id">等于“”查询所有信息</param>
        public DataTable GetTumor_Table(string id)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            {
                new SqlParameter("@EditType",SqlDbType.VarChar),
                new SqlParameter("@ID",SqlDbType.VarChar)
            };
            sqlParam[0].Value = "4";
            sqlParam[1].Value = id;
            DataTable dt = m_app.SqlHelper.ExecuteDataSet("EmrSysTable.usp_Edit_Tumor", sqlParam, CommandType.StoredProcedure).Tables[0];

            return dt;
        }

        /// <summary>
        /// 根据传入肿瘤库实体删除对应数据
        /// </summary>
        /// <param name="Tumor"></param>
        /// <returns></returns>
        public bool DelTumor(Tumor tumor)
        {
            return DelTumor(tumor.Id);
        }

        /// <summary>
        /// 根据传入肿瘤库ID删除对应数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DelTumor(string id)
        {
            try
            {
                SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@EditType",SqlDbType.VarChar),
                    new SqlParameter("@ID",SqlDbType.VarChar)
                };
                sqlParam[0].Value = "3";
                sqlParam[1].Value = id;
                m_app.SqlHelper.ExecuteDataSet("EmrSysTable.usp_Edit_Tumor", sqlParam, CommandType.StoredProcedure);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 新增之前验证编号是否已经存在
        /// </summary>
        /// <param name="Markid"></param>
        /// <returns></returns>
        public bool CheckTumorID(string id)
        {
            DataTable dt = GetTumor_Table(id);
            if (dt.Rows.Count > 0)
            {
                return false;
            }
            return true;
        }

        #endregion
    }

    #region 拼音
    public class ChinesePY
    {
        #region private
        static Hashtable ht = null;
        static Hashtable Ht
        {
            get
            {
                if (ht == null)
                {
                    //建立哈希表汉字韵母
                    ht = new Hashtable();
                    ht.Add(-20319, "a");
                    ht.Add(-20317, "ai"); ht.Add(-20304, "an"); ht.Add(-20295, "ang");
                    ht.Add(-20292, "ao"); ht.Add(-20283, "ba"); ht.Add(-20265, "bai");
                    ht.Add(-20257, "ban"); ht.Add(-20242, "bang"); ht.Add(-20230, "bao");
                    ht.Add(-20051, "bei"); ht.Add(-20036, "ben"); ht.Add(-20032, "beng");
                    ht.Add(-20026, "bi"); ht.Add(-20002, "bian"); ht.Add(-19990, "biao");
                    ht.Add(-19986, "bie"); ht.Add(-19982, "bin"); ht.Add(-19976, "bing");
                    ht.Add(-19805, "bo"); ht.Add(-19784, "bu"); ht.Add(-19775, "ca");
                    ht.Add(-19774, "cai"); ht.Add(-19763, "can"); ht.Add(-19756, "cang");
                    ht.Add(-19751, "cao"); ht.Add(-19746, "ce"); ht.Add(-19741, "ceng");
                    ht.Add(-19739, "cha"); ht.Add(-19728, "chai"); ht.Add(-19725, "chan");
                    ht.Add(-19715, "chang"); ht.Add(-19540, "chao"); ht.Add(-19531, "che");
                    ht.Add(-19525, "chen"); ht.Add(-19515, "cheng"); ht.Add(-19500, "chi");
                    ht.Add(-19484, "chong"); ht.Add(-19479, "chou"); ht.Add(-19467, "chu");
                    ht.Add(-19289, "chuai"); ht.Add(-19288, "chuan"); ht.Add(-19281, "chuang");
                    ht.Add(-19275, "chui"); ht.Add(-19270, "chun"); ht.Add(-19263, "chuo");
                    ht.Add(-19261, "ci"); ht.Add(-19249, "cong"); ht.Add(-19243, "cou");
                    ht.Add(-19242, "cu"); ht.Add(-19238, "cuan"); ht.Add(-19235, "cui");
                    ht.Add(-19227, "cun"); ht.Add(-19224, "cuo"); ht.Add(-19218, "da");
                    ht.Add(-19212, "dai"); ht.Add(-19038, "dan"); ht.Add(-19023, "dang");
                    ht.Add(-19018, "dao"); ht.Add(-19006, "de"); ht.Add(-19003, "deng");
                    ht.Add(-18996, "di"); ht.Add(-18977, "dian"); ht.Add(-18961, "diao");
                    ht.Add(-18952, "die"); ht.Add(-18783, "ding"); ht.Add(-18774, "diu");
                    ht.Add(-18773, "dong"); ht.Add(-18763, "dou"); ht.Add(-18756, "du");
                    ht.Add(-18741, "duan"); ht.Add(-18735, "dui"); ht.Add(-18731, "dun");
                    ht.Add(-18722, "duo"); ht.Add(-18710, "e"); ht.Add(-18697, "en");
                    ht.Add(-18696, "er"); ht.Add(-18526, "fa"); ht.Add(-18518, "fan");
                    ht.Add(-18501, "fang"); ht.Add(-18490, "fei"); ht.Add(-18478, "fen");
                    ht.Add(-18463, "feng"); ht.Add(-18448, "fo"); ht.Add(-18447, "fou");
                    ht.Add(-18446, "fu"); ht.Add(-18239, "ga"); ht.Add(-18237, "gai");
                    ht.Add(-18231, "gan"); ht.Add(-18220, "gang"); ht.Add(-18211, "gao");
                    ht.Add(-18201, "ge"); ht.Add(-18184, "gei"); ht.Add(-18183, "gen");
                    ht.Add(-18181, "geng"); ht.Add(-18012, "gong"); ht.Add(-17997, "gou");
                    ht.Add(-17988, "gu"); ht.Add(-17970, "gua"); ht.Add(-17964, "guai");
                    ht.Add(-17961, "guan"); ht.Add(-17950, "guang"); ht.Add(-17947, "gui");
                    ht.Add(-17931, "gun"); ht.Add(-17928, "guo"); ht.Add(-17922, "ha");
                    ht.Add(-17759, "hai"); ht.Add(-17752, "han"); ht.Add(-17733, "hang");
                    ht.Add(-17730, "hao"); ht.Add(-17721, "he"); ht.Add(-17703, "hei");
                    ht.Add(-17701, "hen"); ht.Add(-17697, "heng"); ht.Add(-17692, "hong");
                    ht.Add(-17683, "hou"); ht.Add(-17676, "hu"); ht.Add(-17496, "hua");
                    ht.Add(-17487, "huai"); ht.Add(-17482, "huan"); ht.Add(-17468, "huang");
                    ht.Add(-17454, "hui"); ht.Add(-17433, "hun"); ht.Add(-17427, "huo");
                    ht.Add(-17417, "ji"); ht.Add(-17202, "jia"); ht.Add(-17185, "jian");
                    ht.Add(-16983, "jiang"); ht.Add(-16970, "jiao"); ht.Add(-16942, "jie");
                    ht.Add(-16915, "jin"); ht.Add(-16733, "jing"); ht.Add(-16708, "jiong");
                    ht.Add(-16706, "jiu"); ht.Add(-16689, "ju"); ht.Add(-16664, "juan");
                    ht.Add(-16657, "jue"); ht.Add(-16647, "jun"); ht.Add(-16474, "ka");
                    ht.Add(-16470, "kai"); ht.Add(-16465, "kan"); ht.Add(-16459, "kang");
                    ht.Add(-16452, "kao"); ht.Add(-16448, "ke"); ht.Add(-16433, "ken");
                    ht.Add(-16429, "keng"); ht.Add(-16427, "kong"); ht.Add(-16423, "kou");
                    ht.Add(-16419, "ku"); ht.Add(-16412, "kua"); ht.Add(-16407, "kuai");
                    ht.Add(-16403, "kuan"); ht.Add(-16401, "kuang"); ht.Add(-16393, "kui");
                    ht.Add(-16220, "kun"); ht.Add(-16216, "kuo"); ht.Add(-16212, "la");
                    ht.Add(-16205, "lai"); ht.Add(-16202, "lan"); ht.Add(-16187, "lang");
                    ht.Add(-16180, "lao"); ht.Add(-16171, "le"); ht.Add(-16169, "lei");
                    ht.Add(-16158, "leng"); ht.Add(-16155, "li"); ht.Add(-15959, "lia");
                    ht.Add(-15958, "lian"); ht.Add(-15944, "liang"); ht.Add(-15933, "liao");
                    ht.Add(-15920, "lie"); ht.Add(-15915, "lin"); ht.Add(-15903, "ling");
                    ht.Add(-15889, "liu"); ht.Add(-15878, "long"); ht.Add(-15707, "lou");
                    ht.Add(-15701, "lu"); ht.Add(-15681, "lv"); ht.Add(-15667, "luan");
                    ht.Add(-15661, "lue"); ht.Add(-15659, "lun"); ht.Add(-15652, "luo");
                    ht.Add(-15640, "ma"); ht.Add(-15631, "mai"); ht.Add(-15625, "man");
                    ht.Add(-15454, "mang"); ht.Add(-15448, "mao"); ht.Add(-15436, "me");
                    ht.Add(-15435, "mei"); ht.Add(-15419, "men"); ht.Add(-15416, "meng");
                    ht.Add(-15408, "mi"); ht.Add(-15394, "mian"); ht.Add(-15385, "miao");
                    ht.Add(-15377, "mie"); ht.Add(-15375, "min"); ht.Add(-15369, "ming");
                    ht.Add(-15363, "miu"); ht.Add(-15362, "mo"); ht.Add(-15183, "mou");
                    ht.Add(-15180, "mu"); ht.Add(-15165, "na"); ht.Add(-15158, "nai");
                    ht.Add(-15153, "nan"); ht.Add(-15150, "nang"); ht.Add(-15149, "nao");
                    ht.Add(-15144, "ne"); ht.Add(-15143, "nei"); ht.Add(-15141, "nen");
                    ht.Add(-15140, "neng"); ht.Add(-15139, "ni"); ht.Add(-15128, "nian");
                    ht.Add(-15121, "niang"); ht.Add(-15119, "niao"); ht.Add(-15117, "nie");
                    ht.Add(-15110, "nin"); ht.Add(-15109, "ning"); ht.Add(-14941, "niu");
                    ht.Add(-14937, "nong"); ht.Add(-14933, "nu"); ht.Add(-14930, "nv");
                    ht.Add(-14929, "nuan"); ht.Add(-14928, "nue"); ht.Add(-14926, "nuo");
                    ht.Add(-14922, "o"); ht.Add(-14921, "ou"); ht.Add(-14914, "pa");
                    ht.Add(-14908, "pai"); ht.Add(-14902, "pan"); ht.Add(-14894, "pang");
                    ht.Add(-14889, "pao"); ht.Add(-14882, "pei"); ht.Add(-14873, "pen");
                    ht.Add(-14871, "peng"); ht.Add(-14857, "pi"); ht.Add(-14678, "pian");
                    ht.Add(-14674, "piao"); ht.Add(-14670, "pie"); ht.Add(-14668, "pin");
                    ht.Add(-14663, "ping"); ht.Add(-14654, "po"); ht.Add(-14645, "pu");
                    ht.Add(-14630, "qi"); ht.Add(-14594, "qia"); ht.Add(-14429, "qian");
                    ht.Add(-14407, "qiang"); ht.Add(-14399, "qiao"); ht.Add(-14384, "qie");
                    ht.Add(-14379, "qin"); ht.Add(-14368, "qing"); ht.Add(-14355, "qiong");
                    ht.Add(-14353, "qiu"); ht.Add(-14345, "qu"); ht.Add(-14170, "quan");
                    ht.Add(-14159, "que"); ht.Add(-14151, "qun"); ht.Add(-14149, "ran");
                    ht.Add(-14145, "rang"); ht.Add(-14140, "rao"); ht.Add(-14137, "re");
                    ht.Add(-14135, "ren"); ht.Add(-14125, "reng"); ht.Add(-14123, "ri");
                    ht.Add(-14122, "rong"); ht.Add(-14112, "rou"); ht.Add(-14109, "ru");
                    ht.Add(-14099, "ruan"); ht.Add(-14097, "rui"); ht.Add(-14094, "run");
                    ht.Add(-14092, "ruo"); ht.Add(-14090, "sa"); ht.Add(-14087, "sai");
                    ht.Add(-14083, "san"); ht.Add(-13917, "sang"); ht.Add(-13914, "sao");
                    ht.Add(-13910, "se"); ht.Add(-13907, "sen"); ht.Add(-13906, "seng");
                    ht.Add(-13905, "sha"); ht.Add(-13896, "shai"); ht.Add(-13894, "shan");
                    ht.Add(-13878, "shang"); ht.Add(-13870, "shao"); ht.Add(-13859, "she");
                    ht.Add(-13847, "shen"); ht.Add(-13831, "sheng"); ht.Add(-13658, "shi");
                    ht.Add(-13611, "shou"); ht.Add(-13601, "shu"); ht.Add(-13406, "shua");
                    ht.Add(-13404, "shuai"); ht.Add(-13400, "shuan"); ht.Add(-13398, "shuang");
                    ht.Add(-13395, "shui"); ht.Add(-13391, "shun"); ht.Add(-13387, "shuo");
                    ht.Add(-13383, "si"); ht.Add(-13367, "song"); ht.Add(-13359, "sou");
                    ht.Add(-13356, "su"); ht.Add(-13343, "suan"); ht.Add(-13340, "sui");
                    ht.Add(-13329, "sun"); ht.Add(-13326, "suo"); ht.Add(-13318, "ta");
                    ht.Add(-13147, "tai"); ht.Add(-13138, "tan"); ht.Add(-13120, "tang");
                    ht.Add(-13107, "tao"); ht.Add(-13096, "te"); ht.Add(-13095, "teng");
                    ht.Add(-13091, "ti"); ht.Add(-13076, "tian"); ht.Add(-13068, "tiao");
                    ht.Add(-13063, "tie"); ht.Add(-13060, "ting"); ht.Add(-12888, "tong");
                    ht.Add(-12875, "tou"); ht.Add(-12871, "tu"); ht.Add(-12860, "tuan");
                    ht.Add(-12858, "tui"); ht.Add(-12852, "tun"); ht.Add(-12849, "tuo");
                    ht.Add(-12838, "wa"); ht.Add(-12831, "wai"); ht.Add(-12829, "wan");
                    ht.Add(-12812, "wang"); ht.Add(-12802, "wei"); ht.Add(-12607, "wen");
                    ht.Add(-12597, "weng"); ht.Add(-12594, "wo"); ht.Add(-12585, "wu");
                    ht.Add(-12556, "xi"); ht.Add(-12359, "xia"); ht.Add(-12346, "xian");
                    ht.Add(-12320, "xiang"); ht.Add(-12300, "xiao"); ht.Add(-12120, "xie");
                    ht.Add(-12099, "xin"); ht.Add(-12089, "xing"); ht.Add(-12074, "xiong");
                    ht.Add(-12067, "xiu"); ht.Add(-12058, "xu"); ht.Add(-12039, "xuan");
                    ht.Add(-11867, "xue"); ht.Add(-11861, "xun"); ht.Add(-11847, "ya");
                    ht.Add(-11831, "yan"); ht.Add(-11798, "yang"); ht.Add(-11781, "yao");
                    ht.Add(-11604, "ye"); ht.Add(-11589, "yi"); ht.Add(-11536, "yin");
                    ht.Add(-11358, "ying"); ht.Add(-11340, "yo"); ht.Add(-11339, "yong");
                    ht.Add(-11324, "you"); ht.Add(-11303, "yu"); ht.Add(-11097, "yuan");
                    ht.Add(-11077, "yue"); ht.Add(-11067, "yun"); ht.Add(-11055, "za");
                    ht.Add(-11052, "zai"); ht.Add(-11045, "zan"); ht.Add(-11041, "zang");
                    ht.Add(-11038, "zao"); ht.Add(-11024, "ze"); ht.Add(-11020, "zei");
                    ht.Add(-11019, "zen"); ht.Add(-11018, "zeng"); ht.Add(-11014, "zha");
                    ht.Add(-10838, "zhai"); ht.Add(-10832, "zhan"); ht.Add(-10815, "zhang");
                    ht.Add(-10800, "zhao"); ht.Add(-10790, "zhe"); ht.Add(-10780, "zhen");
                    ht.Add(-10764, "zheng"); ht.Add(-10587, "zhi"); ht.Add(-10544, "zhong");
                    ht.Add(-10533, "zhou"); ht.Add(-10519, "zhu"); ht.Add(-10331, "zhua");
                    ht.Add(-10329, "zhuai"); ht.Add(-10328, "zhuan"); ht.Add(-10322, "zhuang");
                    ht.Add(-10315, "zhui"); ht.Add(-10309, "zhun"); ht.Add(-10307, "zhuo");
                    ht.Add(-10296, "zi"); ht.Add(-10281, "zong"); ht.Add(-10274, "zou");
                    ht.Add(-10270, "zu"); ht.Add(-10262, "zuan"); ht.Add(-10260, "zui");
                    ht.Add(-10256, "zun"); ht.Add(-10254, "zuo"); ht.Add(-10247, "zz");
                }
                return ht;
            }
        }
        static string g(int num)
        {
            if (num < -20319 || num > -10247)
                return "";
            while (!Ht.ContainsKey(num))
                num--;
            return Ht[num].ToString();
        }
        static bool In(int Lp, int Hp, int Value)
        {
            return ((Value <= Hp) && (Value >= Lp));
        }
        #endregion
        /// <summary> 
        /// 获取汉字拼音，特殊字符去掉，英文不做处理 
        /// </summary> 
        /// <param name="hz"></param> 
        /// <returns></returns> 
        public static string GetPinYin(string hz)
        {
            byte[] b = System.Text.Encoding.Default.GetBytes(hz);
            int p;
            StringBuilder ret = new StringBuilder();
            for (int i = 0; i < b.Length; i++)
            {
                p = (int)b[i];
                if (p > 160)
                {
                    p = p * 256 + b[++i] - 65536;
                    ret.Append(g(p));
                }
                else
                {
                    ret.Append((char)p);
                }
            }
            return ret.ToString();
        }
        /// <summary> 
        /// 获取汉字拼音的首字母 
        /// </summary> 
        /// <param name="str"></param> 
        /// <returns></returns> 
        public static string GetPinYinIndex(string str)
        {
            StringBuilder ret = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                double tmp = (double)str[i];
                if (tmp >= 0x4e00 && tmp < 0x9fa5)
                {
                    ret.Append(Convert(str[i]));
                }
            }
            return ret.ToString();
        }

        /// <summary> 
        /// 获取一个汉字的拼音声母 
        /// </summary> 
        /// <param name="chinese">Unicode格式的一个汉字</param> 
        /// <returns>汉字的声母</returns> 
        public static char Convert(Char chinese)
        {
            Encoding gb2312 = Encoding.GetEncoding("GB2312");
            Encoding unicode = Encoding.Unicode;

            // Convert the string into a byte[]. 
            byte[] unicodeBytes = unicode.GetBytes(new Char[] { chinese });
            // Perform the conversion from one encoding to the other. 
            byte[] asciiBytes = Encoding.Convert(unicode, gb2312, unicodeBytes);

            // 计算该汉字的GB-2312编码 
            int n = (int)asciiBytes[0] << 8;
            n += (int)asciiBytes[1];

            // 根据汉字区域码获取拼音声母 
            if (In(0xB0A1, 0xB0C4, n)) return 'a';
            if (In(0xB0C5, 0xB2C0, n)) return 'b';
            if (In(0xB2C1, 0xB4ED, n)) return 'c';
            if (In(0xB4EE, 0xB6E9, n)) return 'd';
            if (In(0xB6EA, 0xB7A1, n)) return 'e';
            if (In(0xB7A2, 0xB8c0, n)) return 'f';
            if (In(0xB8C1, 0xB9FD, n)) return 'g';
            if (In(0xB9FE, 0xBBF6, n)) return 'h';
            if (In(0xBBF7, 0xBFA5, n)) return 'j';
            if (In(0xBFA6, 0xC0AB, n)) return 'k';
            if (In(0xC0AC, 0xC2E7, n)) return 'l';
            if (In(0xC2E8, 0xC4C2, n)) return 'm';
            if (In(0xC4C3, 0xC5B5, n)) return 'n';
            if (In(0xC5B6, 0xC5BD, n)) return 'o';
            if (In(0xC5BE, 0xC6D9, n)) return 'p';
            if (In(0xC6DA, 0xC8BA, n)) return 'q';
            if (In(0xC8BB, 0xC8F5, n)) return 'r';
            if (In(0xC8F6, 0xCBF0, n)) return 's';
            if (In(0xCBFA, 0xCDD9, n)) return 't';
            if (In(0xCDDA, 0xCEF3, n)) return 'w';
            if (In(0xCEF4, 0xD188, n)) return 'x';
            if (In(0xD1B9, 0xD4D0, n)) return 'y';
            if (In(0xD4D1, 0xD7F9, n)) return 'z';
            return "/0".ToCharArray()[0];
        }
    }

    #endregion

}


