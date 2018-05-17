using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrectSoft.FrameWork.WinForm.Plugin;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using DrectSoft.Core.EMR_NursingDocument.PublicSet;

namespace DrectSoft.Core.EMR_NursingDocument.PublicMethod
{
    public class NurseRecordBiz
    {
        IDataAccess sql_Helper = MethodSet.App.SqlHelper;
        IEmrHost m_app = MethodSet.App;

        public NurseRecordBiz(){}

        /// <summary>
        /// 获取病人规定单据的内容
        /// </summary>
        /// <param name="noofInpatent">病人标示符</param>
        /// <param name="fatherRecordId">护理单据标示符</param>
        /// <returns></returns>
        public List<NurseRecordEntity> GetNurseRecord(string noofInpatent, string fatherRecordId)
        {
            SqlParameter[] sqlParams = new SqlParameter[] { 
                  new SqlParameter("@NoOfInpatient", SqlDbType.VarChar, 50),
                  new SqlParameter("@FatherRecordId", SqlDbType.VarChar, 50)
            };
            sqlParams[0].Value = noofInpatent;
            sqlParams[1].Value = fatherRecordId;
            DataTable dataTableNurseRecord = sql_Helper.ExecuteDataTable("EMR_NURSE_STATION.usp_GetNurseRecord", sqlParams, CommandType.StoredProcedure);
            List<NurseRecordEntity> nurseRecordEntityList = DataTableToList<NurseRecordEntity>.ConvertToModel(dataTableNurseRecord);
            return nurseRecordEntityList;
        }

        /// <summary>
        /// 添加或者修改护理记录
        /// </summary>
        /// <param name="nurseRecordEntity"></param>
        /// <returns></returns>
        public bool AddOrModNurseRecord(NurseRecordEntity nurseRecordEntity)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[] { 
                  new SqlParameter("@ID", SqlDbType.VarChar, 50),
                  new SqlParameter("@NOOFINPATENT", SqlDbType.VarChar, 50),
                  new SqlParameter("@FATHER_RECORDID", SqlDbType.VarChar, 50),
                  new SqlParameter("@RECORD_DATETIME", SqlDbType.VarChar, 50),
                  new SqlParameter("@TIWEN", SqlDbType.VarChar, 50),
                  new SqlParameter("@MAIBO", SqlDbType.VarChar, 50),
                  new SqlParameter("@HUXI", SqlDbType.VarChar, 50),
                  new SqlParameter("@XUEYA", SqlDbType.VarChar, 50),
                  new SqlParameter("@YISHI", SqlDbType.VarChar, 50),
                  new SqlParameter("@XYBHD", SqlDbType.VarChar, 50),
                  new SqlParameter("@QKFL", SqlDbType.VarChar, 50),
                  new SqlParameter("@SYPF", SqlDbType.VarChar, 50),
                  new SqlParameter("@OTHER_ONE", SqlDbType.VarChar, 50),
                  new SqlParameter("@OTHER_TWO", SqlDbType.VarChar, 50),
                  new SqlParameter("@OTHER_THREE", SqlDbType.VarChar, 50),
                  new SqlParameter("@OTHER_FOUR", SqlDbType.VarChar, 50),
                  new SqlParameter("@ZTKDX", SqlDbType.VarChar, 50),
                  new SqlParameter("@YTKDX", SqlDbType.VarChar, 50),
                  new SqlParameter("@TKDGFS", SqlDbType.VarChar, 50),
                  new SqlParameter("@WOWEI", SqlDbType.VarChar, 50),
                  new SqlParameter("@JMZG", SqlDbType.VarChar, 50),
                  new SqlParameter("@DGJYLG_ONE", SqlDbType.VarChar, 50),
                  new SqlParameter("@DGJYLG_TWO", SqlDbType.VarChar, 50),
                  new SqlParameter("@DGJYLG_THREE", SqlDbType.VarChar, 50),
                  new SqlParameter("@In_ITEM", SqlDbType.VarChar, 50),
                  new SqlParameter("@In_VALUE", SqlDbType.VarChar, 50),
                  new SqlParameter("@OUT_ITEM", SqlDbType.VarChar, 50),
                  new SqlParameter("@OUT_VALUE", SqlDbType.VarChar, 50),
                  new SqlParameter("@OUT_COLOR", SqlDbType.VarChar, 50),
                  new SqlParameter("@OUT_STATUE", SqlDbType.VarChar, 50),
                  new SqlParameter("@OTHER", SqlDbType.VarChar, 50),
                  new SqlParameter("@HXMS", SqlDbType.VarChar, 50),
                  new SqlParameter("@FCIMIAO", SqlDbType.VarChar, 50),
                  new SqlParameter("@XRYND", SqlDbType.VarChar, 50),
                  new SqlParameter("@CGSD", SqlDbType.VarChar, 50),
                  new SqlParameter("@LXXZYTQ", SqlDbType.VarChar, 50),
                  new SqlParameter("@BDG", SqlDbType.VarChar, 50),
                  new SqlParameter("@SINGE_DOCTOR", SqlDbType.VarChar, 50),
                   new SqlParameter("@SINGE_DOCTORID", SqlDbType.VarChar, 50),
                  new SqlParameter("@CREATE_DOCTORID", SqlDbType.VarChar, 50),
                   new SqlParameter("@CREATE_TIME", SqlDbType.VarChar, 50),
                   new SqlParameter("@VALID", SqlDbType.VarChar, 50)
            };

                PropertyInfo[] propertys = nurseRecordEntity.GetType().GetProperties();
                foreach (var item in sqlParams)
                {
                    string parName = item.ParameterName;
                    foreach (var proItem in propertys)
                    {
                        string nameChange = "@" + proItem.Name;
                        if (nameChange.Equals(parName))
                        {
                            item.Value = proItem.GetValue(nurseRecordEntity, null);
                            break;
                        }
                    }
                }

                sql_Helper.ExecuteNoneQuery("EMR_NURSE_STATION.usp_AddOrModNurseRecord", sqlParams, CommandType.StoredProcedure);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除护理单记录
        /// </summary>
        /// <param name="nurseRecordId"></param>
        /// <returns></returns>
        public bool DelNurseRecord(string nurseRecordId)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[] { 
                  new SqlParameter("@NurseId", SqlDbType.VarChar, 50),
            };
                sqlParams[0].Value = nurseRecordId;
                sql_Helper.ExecuteNoneQuery("EMR_NURSE_STATION.usp_DelNurseRecord", sqlParams, CommandType.StoredProcedure);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
