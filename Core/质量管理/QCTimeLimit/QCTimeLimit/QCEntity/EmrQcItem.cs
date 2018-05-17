using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DrectSoft.DSSqlHelper;
using System.Data.SqlClient;

namespace DrectSoft.Emr.QCTimeLimit.QCEntity
{
    /// <summary>
    /// 病历质量控制库
    /// EMRQCITEM表实体类
    /// by xlb 2013-01-08
    /// </summary>
    class EmrQcItem
    {
        #region Property
        /// <summary>
        /// 分类代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 质控代码
        /// </summary>
        public string I_Code { get; set; }

        /// <summary>
        /// 质控名称
        /// </summary>
        public string I_Name { get; set; }

        #endregion

        /// <summary>
        /// 获取病历质量控制代码的SQL
        /// </summary>
        const string c_SqlEmrQcItem = "select CODE,NAME,I_CODE,I_NAME from EMRQCITEM where I_CODE like'%'||@ICODE||'%' and I_NAME like '%'||@INAME||'%' ";

        #region 方法

        /// <summary>
        /// 获取所有病历质量控制数据
        /// by xlb 2013-01-08
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllEmrQCItem(EmrQcItem emrQcItem)
        {
            try
            {
                if (emrQcItem == null)
                {
                    throw new Exception("没有数据");
                }
                SqlParameter[] sps = { 
                                      new SqlParameter("@ICODE", emrQcItem.I_Code==null?"":emrQcItem.I_Code),
                                      new SqlParameter("@INAME",emrQcItem.I_Name==null?"":emrQcItem.I_Name)
                                     };
                return DS_SqlHelper.ExecuteDataTable(c_SqlEmrQcItem, sps, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取所有的病历质量控制记录 Datatable->List
        /// by xlb 2013-01-08
        /// </summary>
        /// <returns></returns>
        public static List<EmrQcItem> GetEmrQCItem(EmrQcItem emrQcItem)
        {
            try
            {
                DataTable dtEmrQcItem = GetAllEmrQCItem(emrQcItem);
                List<EmrQcItem> emrQcItemTemp = new List<EmrQcItem>();
                foreach (DataRow dr in dtEmrQcItem.Rows)
                {
                    EmrQcItem emrQcItemList = ConvertToEmrQcItem(dr);
                    emrQcItemTemp.Add(emrQcItemList);
                }

                return emrQcItemTemp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 获取病历质量控制对象
        /// datarow->EmrQcItem
        /// xlb 2013-01-08
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public static EmrQcItem ConvertToEmrQcItem(DataRow dataRow)
        {
            try
            {
                EmrQcItem emrQcItem = new EmrQcItem();
                emrQcItem.Code = dataRow["CODE"].ToString();
                emrQcItem.Name = dataRow["NAME"].ToString();
                emrQcItem.I_Code = dataRow["I_CODE"].ToString();
                emrQcItem.I_Name = dataRow["I_NAME"].ToString();

                return emrQcItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除病历质量控制数据
        /// </summary>
        /// <param name="emrQcItem"></param>
        public static void DeleteEmrQcItem(EmrQcItem emrQcItem)
        {
            try
            {
                string sqlDeleteEmrTemp = "delete EMRQCITEM where I_CODE=@iCode";
                if (emrQcItem == null || emrQcItem.I_Code == null)
                {
                    throw new Exception("没有数据");
                }
                SqlParameter[] sps = { new SqlParameter("@iCode", emrQcItem.I_Code) };
                DS_SqlHelper.ExecuteNonQuery(sqlDeleteEmrTemp, sps, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 新增病历质控数据
        /// by xlb 2013-01-08
        /// </summary>
        /// <param name="emrQcItem"></param>
        public static void InsertToEmrQcItem(EmrQcItem emrQcItem)
        {
            try
            {
                string sqlInsertEmrTemp = "insert into EMRQCITEM(CODE,NAME,I_CODE,"
                + "I_NAME) values(@CODE,@NAME,@I_CODE,@I_NAME)";
                if (emrQcItem == null)
                {
                    throw new Exception("没有数据");
                }
                SqlParameter[] sps = { 
                                       new SqlParameter("@CODE", emrQcItem.Code==null?"":emrQcItem.Code),
                                       new SqlParameter("@NAME",emrQcItem.Name==null?"":emrQcItem.Name),
                                       new SqlParameter("@I_CODE",emrQcItem.I_Code==null?"":emrQcItem.I_Code),
                                       new SqlParameter("@I_NAME",emrQcItem.I_Name==null?"":emrQcItem.I_Name)
                                     };
                DS_SqlHelper.ExecuteNonQuery(sqlInsertEmrTemp, sps, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 修改病历质量控制数据
        ///   xlb 2013-01-08
        /// </summary>
        /// <param name="emrQcItem"></param>
        public static void UpdateToEmrQcItem(EmrQcItem emrQcItem)
        {
            try
            {
                string sqlUpdateEmrTemp = "update EMRQCITEM set CODE=@CODE,NAME=@NAME,"
                + "I_CODE=@I_CODE,I_NAME=@I_NAME  where I_CODE=@I_CODE";
                SqlParameter[] sps = { 
                                       new SqlParameter("@CODE", emrQcItem.Code==null?"":emrQcItem.Code),
                                       new SqlParameter("@NAME",emrQcItem.Name==null?"":emrQcItem.Name),
                                       new SqlParameter("@I_CODE",emrQcItem.I_Code==null?"":emrQcItem.I_Code),
                                       new SqlParameter("@I_NAME",emrQcItem.I_Name==null?"":emrQcItem.I_Name)
                                     };
                DS_SqlHelper.ExecuteNonQuery(sqlUpdateEmrTemp, sps, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
