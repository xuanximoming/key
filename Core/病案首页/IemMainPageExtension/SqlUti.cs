using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrectSoft.DSSqlHelper;
using System.Data;
using DrectSoft.Common;
using System.Data.SqlClient;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace IemMainPageExtension
{
    /// <summary>
    /// Add by xlb 2013-04-10
    /// </summary>
    public class SqlUti
    {
        /*t通过病人病案号抓取使用对象集合Sql*/
        const string sqlGetIemUseByNoof = @"select iemexuseid,iemexid,noofinpat,value,
        createdocid,createdatetime,modifydocid,modifydatetime from iem_mainpage_except_use where noofinpat=@nOofinpat ";

        /// <summary>
        /// 构造函数
        /// </summary>
        public SqlUti()
        {
            try
            {
                DS_SqlHelper.CreateSqlHelper();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 通过病人病案号抓取相应的使用对象集合
        /// Add by xlb 2013-04-12
        /// </summary>
        /// <returns></returns>
        public List<IemMainPageExceptUse> GetIemExceptUse(string noofinpat)
        {
            try
            {
                SqlParameter[] sps = { new SqlParameter("@noofinpat", noofinpat) };
                DataTable dt = DS_SqlHelper.ExecuteDataTable(sqlGetIemUseByNoof, sps, CommandType.Text);
                List<IemMainPageExceptUse> iemExceptUse = DataTableToList<IemMainPageExceptUse>.ConvertToModel(dt);
                return iemExceptUse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 抓取病案扩展维护集合
        /// Add by xlb 2013-04-12
        /// </summary>
        /// <param name="dateList">数据元集合</param>
        /// <returns>扩展维护集合</returns>
        public List<IemMainPageExcept> GetIemMainPageExcept(Dictionary<string, DateElementEntity> dateList)
        {
            try
            {
                SqlParameter p_result = new SqlParameter("@result", SqlDbType.Structured);
                p_result.Direction = ParameterDirection.Output;
                DataTable dataTable = DS_SqlHelper.ExecuteDataTable("MainPageExtenSion.GETIEMEXCEPT", new SqlParameter[] { p_result }, CommandType.StoredProcedure);
                List<IemMainPageExcept> IemMainPageExceptList = DataTableToList<IemMainPageExcept>.ConvertToModel(dataTable);
               
                foreach (var item in IemMainPageExceptList)
                {
                    ConvertToIemMainExceptList(item, dateList);
                }

                return IemMainPageExceptList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 抓取所有数据元 DataTable->Dictionary
        /// Add by xlb 2013-04-12
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, DateElementEntity> GetDataElement()
        {
            try
            {
                Dictionary<string, DateElementEntity> dateElement = new Dictionary<string, DateElementEntity>();
                SqlParameter p_result = new SqlParameter("@result", SqlDbType.Structured);
                p_result.Direction = ParameterDirection.Output;

                SqlParameter[] sqlParams = new SqlParameter[]
                { 
                  new SqlParameter("@ElementId", SqlDbType.VarChar, 50),
                  new SqlParameter("@ElementName", SqlDbType.VarChar, 50),
                  new SqlParameter("@ElementClass", SqlDbType.VarChar, 50),
                  new SqlParameter("@ElementPYM", SqlDbType.VarChar, 50),
                  p_result
                };

                sqlParams[0].Value = "";
                sqlParams[1].Value = "";
                sqlParams[2].Value = "";
                sqlParams[3].Value = "";
                DataTable dtData = DS_SqlHelper.ExecuteDataTable("EMR_CommonNote.usp_GetDateElement", sqlParams, CommandType.StoredProcedure);
                DataTable dtElementEntity = dtData.Clone();

                foreach (DataRow row in dtData.Rows)
                {
                    dtElementEntity.Rows.Clear();
                    dtElementEntity.Rows.Add(row.ItemArray);
                    DateElementEntity dateElementEntity = DataTableToList<DateElementEntity>.ConvertToModelOne(dtElementEntity);
                    dateElement.Add(row["ELEMENTFLOW"].ToString(), dateElementEntity);
                }
                return dateElement;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 抓取所有数据元
        /// Add by xlb 2013-04-12
        /// </summary>
        /// <returns></returns>
        public List<DateElementEntity> GetAllDateElement()
        {
            try
            {

                SqlParameter p_result = new SqlParameter("@result", SqlDbType.Structured);
                p_result.Direction = ParameterDirection.Output;
                SqlParameter[] sqlParams = new SqlParameter[] 
                { 
                  new SqlParameter("@ElementId", SqlDbType.VarChar, 50),
                  new SqlParameter("@ElementName", SqlDbType.VarChar, 50),
                  new SqlParameter("@ElementClass", SqlDbType.VarChar, 50),
                  new SqlParameter("@ElementPYM", SqlDbType.VarChar, 50),
                  p_result
                };
                sqlParams[0].Value = "";
                sqlParams[1].Value = "";
                sqlParams[2].Value = "";
                sqlParams[3].Value = "";
                DataTable dtDateTable = DS_SqlHelper.ExecuteDataTable("EMR_CommonNote.usp_GetDateElement", sqlParams, CommandType.StoredProcedure);
                List<DateElementEntity> dataList = DataTableToList<DateElementEntity>.ConvertToModel(dtDateTable);
                return dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获得每个数据元的实体
        /// Add by xlb 2013-04-12
        /// </summary>
        private void ConvertToIemMainExceptList(IemMainPageExcept iemMainPageExcept, Dictionary<string, DateElementEntity> dateElement)
        {
            try
            {
                iemMainPageExcept.DateElement = dateElement[iemMainPageExcept.DateElementFlow];
                iemMainPageExcept.ElementType = iemMainPageExcept.DateElement.ElementType;
                iemMainPageExcept.ElementName = iemMainPageExcept.DateElement.ElementName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 保存方法
        /// Add by xlb 2013-04-12
        /// </summary>
        /// <param name="iemExcept"></param>
        public bool SaveIemMainPageExcept(IemMainPageExcept iemExcept)
        {
            try
            {
                if (string.IsNullOrEmpty(iemExcept.IemExId))
                {
                    iemExcept.IemExId = Guid.NewGuid().ToString();
                }

                SqlParameter[] sps =
                {
                new SqlParameter("@iemexId",SqlDbType.VarChar),
                new SqlParameter("@iemexName",SqlDbType.VarChar),
                new SqlParameter("@dateElementFlow",SqlDbType.VarChar),
                new SqlParameter("@iemControl",SqlDbType.VarChar),
                new SqlParameter("@iemOtherName",SqlDbType.VarChar),
                new SqlParameter("@orderCode",SqlDbType.VarChar),
                new SqlParameter("@isOtherLine",SqlDbType.VarChar),
                new SqlParameter("@valide",SqlDbType.VarChar),
                new SqlParameter("@createDocId",SqlDbType.VarChar),
                new SqlParameter("@createDateTime",SqlDbType.VarChar),
                new SqlParameter("@modifyDocId",SqlDbType.VarChar),
                new SqlParameter("@modifyDateTime",SqlDbType.VarChar)
                };
                sps[0].Value = iemExcept.IemExId;
                sps[1].Value = iemExcept.IemExName;
                sps[2].Value = iemExcept.DateElementFlow;
                sps[3].Value = iemExcept.IemControl;
                sps[4].Value = iemExcept.IemOtherName;
                sps[5].Value = iemExcept.OrderCode;
                sps[6].Value = iemExcept.IsOtherLine.Equals("是") ? "1" : "0";
                sps[7].Value = iemExcept.Vailde;
                sps[8].Value = DS_Common.currentUser.DoctorId;
                sps[9].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                if (string.IsNullOrEmpty(iemExcept.ModifyDocId))
                {
                    sps[10].Value = "";
                    sps[11].Value = "";
                }
                else
                {
                    sps[10].Value = iemExcept.ModifyDocId;
                    sps[11].Value = iemExcept.ModifyDateTime;
                }
                DS_SqlHelper.ExecuteNonQuery("MainPageExtenSion.ADDORMODIFYIEMEXCEPT", sps, CommandType.StoredProcedure);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 保存使用记录信息
        /// Add by xlb 2013-04-12
        /// </summary>
        /// <param name="iemExceptUse"></param>
        /// <returns></returns>
        public bool SaveIemUse(IemMainPageExceptUse iemExceptUse)
        {
            try
            {
                if (iemExceptUse == null)
                {
                    return false;
                }
                if (string.IsNullOrEmpty(iemExceptUse.IemexUseId))
                {
                    iemExceptUse.IemexUseId = Guid.NewGuid().ToString();
                }
                SqlParameter[] sps =
                {
                new SqlParameter("@iemExUserId",SqlDbType.NVarChar),
                new SqlParameter("@iemExId",SqlDbType.NVarChar),
                new SqlParameter("@nOofInpat",SqlDbType.NVarChar),
                new SqlParameter("@value",SqlDbType.NVarChar),
                new SqlParameter("@createDocId",SqlDbType.NVarChar),
                new SqlParameter("@modifyDocId",SqlDbType.VarChar),
                new SqlParameter("@modifyDateTime",SqlDbType.NVarChar)
                };
                sps[0].Value = iemExceptUse.IemexUseId;
                sps[1].Value = iemExceptUse.IemexId;
                sps[2].Value = iemExceptUse.Noofinpat;
                sps[3].Value = iemExceptUse.Value; ;
                sps[4].Value = DS_Common.currentUser.DoctorId;
                sps[5].Value = DS_Common.currentUser.DoctorId;
                sps[6].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                DS_SqlHelper.ExecuteNonQuery("MainPageExtenSion.ADDORMODIFYIEMUS", sps, CommandType.StoredProcedure);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查看维护数据元是否已被使用
        /// Add by xlb 2013-04-12
        /// </summary>
        /// <param name="iemeXId">维护对象数据元对应流水号</param>
        /// <returns>条数</returns>
        public int GetIsBeenUse(string iemeXId/*维护对象属性流水号*/)
        {
            try
            {
                //通过维护对象数据元查看是否该数据元已被使用
                //需串维护表确定该字段是否仍在使用
                string sqlGetCount = @"select count(*) from iem_mainpage_except_use where 
                                                                                 iemexid=@ieMexId and 
                                          exists(select 1 from iem_mainpage_except where iemexid=@ieMexId 
                                                                                     and valide='1')";
                SqlParameter[] sps = { new SqlParameter("@ieMexId", iemeXId) };
                DataTable dtCount = DS_SqlHelper.ExecuteDataTable(sqlGetCount, sps, CommandType.Text);
                if (dtCount == null || dtCount.Rows.Count <= 0)
                {
                    return 0;
                }
                else
                {
                    return Int32.Parse(dtCount.Rows[0][0].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
