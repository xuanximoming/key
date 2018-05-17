using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrectSoft.FrameWork.WinForm.Plugin;
using System.Data.SqlClient;
using System.Data;

namespace DrectSoft.Core.CommonTableConfig
{
    public class CommonNoteBiz
    {
        private IEmrHost m_app;
        public CommonNoteBiz(IEmrHost app)
        {
            this.m_app = app;
        }

        /// <summary>
        /// 获取当前科室或病区的所有配置单据  type 01科室 02病区
        /// </summary>
        /// <returns></returns>
        public List<CommonNoteEntity> GetCommonNoteByDeptWard(string deptorwardCode, string type)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[] { 
                  new SqlParameter("@SiteCode", SqlDbType.VarChar, 50),
                  new SqlParameter("@Type", SqlDbType.VarChar, 50)
            };
                sqlParams[0].Value = deptorwardCode;
                sqlParams[1].Value = type;
                DataTable commonNoteDT = m_app.SqlHelper.ExecuteDataTable("EMR_CommonNote.usp_GetCommonNoteForDeptWard", sqlParams, CommandType.StoredProcedure);
                List<CommonNoteEntity> commonNoteList = DataTableToList<CommonNoteEntity>.ConvertToModel(commonNoteDT);
                return commonNoteList;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        /// <summary>
        /// 获取通用配置单列表
        /// </summary>
        /// <param name="commonNoteName"></param>
        /// <returns></returns>
        public List<CommonNoteEntity> GetSimpleCommonNote(string commonNoteName)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[] { 
                  new SqlParameter("@CommonNoteName", SqlDbType.VarChar, 50),
            };
                sqlParams[0].Value = commonNoteName;
                DataTable dataTableElement = m_app.SqlHelper.ExecuteDataTable("EMR_CommonNote.usp_GetSimpleCommonNote", sqlParams, CommandType.StoredProcedure);
                List<CommonNoteEntity> commonNoteList = DataTableToList<CommonNoteEntity>.ConvertToModel(dataTableElement);
                SetSimpleCommonNoteSite(commonNoteList);
                return commonNoteList;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        /// <summary>
        /// 通过流水号获取配置单
        /// </summary>
        /// <param name="commonNoteFlow"></param>
        /// <returns></returns>
        public CommonNoteEntity GetSimpleCommonNoteByflow(string commonNoteFlow)
        {
            try
            {
                string sql = string.Format("select * from commonnote where commonnote.commonnoteflow='{0}';", commonNoteFlow);
                DataTable dtCommonNote = m_app.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
                CommonNoteEntity commonNoteEntity = DataTableToList<CommonNoteEntity>.ConvertToModelOne(dtCommonNote);
                SetCommonNoteSiteOne(commonNoteEntity);
                return commonNoteEntity;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        /// <summary>
        /// 为每一个配置单查找科室和病区
        /// </summary>
        /// <param name="commonNoteList"></param>
        private void SetSimpleCommonNoteSite(List<CommonNoteEntity> commonNoteList)
        {
            try
            {
                foreach (var commonNoteitem in commonNoteList)
                {
                    SetCommonNoteSiteOne(commonNoteitem);
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        /// <summary>
        /// 为单个配置单设置科室和病区
        /// </summary>
        /// <param name="commonNoteitem"></param>
        private void SetCommonNoteSiteOne(CommonNoteEntity commonNoteitem)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[] { 
                  new SqlParameter("@commonnoteflow",SqlDbType.VarChar,50)
            };
                sqlParams[0].Value = commonNoteitem.CommonNoteFlow;
                DataSet dataSetSite = m_app.SqlHelper.ExecuteDataSet("EMR_CommonNote.usp_GetCommonNoteSite", sqlParams, CommandType.StoredProcedure);
                commonNoteitem.BaseDepartments = DataTableToList<BaseDictory>.ConvertToModel(dataSetSite.Tables[0]);
                commonNoteitem.BaseAreas = DataTableToList<BaseDictory>.ConvertToModel(dataSetSite.Tables[1]);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        /// <summary>
        /// 获取通用单据的详细信息
        /// </summary>
        /// <param name="commonNoteFlow"></param>
        /// <returns></returns>
        public CommonNoteEntity GetDetailCommonNote(string commonNoteFlow)
        {
            try
            {
                CommonNoteEntity commonNoteEntity = new CommonNoteEntity();
                SqlParameter[] sqlParams = new SqlParameter[] { 
                  new SqlParameter("@CommonNoteFlow", SqlDbType.VarChar, 50)
            };
                sqlParams[0].Value = commonNoteFlow;
                DataSet dataTableElement = m_app.SqlHelper.ExecuteDataSet("EMR_CommonNote.usp_GetDetailCommonNote", sqlParams, CommandType.StoredProcedure);
                commonNoteEntity = DataTableToList<CommonNoteEntity>.ConvertToModelOne(dataTableElement.Tables[0]);
                commonNoteEntity.CommonNote_TabList = DataTableToList<CommonNote_TabEntity>.ConvertToModel(dataTableElement.Tables[1]);
                List<CommonNote_ItemEntity> commonNote_ItemList = new List<CommonNote_ItemEntity>();
                commonNote_ItemList = DataTableToList<CommonNote_ItemEntity>.ConvertToModel(dataTableElement.Tables[2]);
                SetCommonNoteTabByItem(commonNoteEntity, commonNote_ItemList);
                SetCommonNoteSiteOne(commonNoteEntity);
                return commonNoteEntity;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        /// <summary>
        /// 将配置单的单个项目分配到每一个标签下面
        /// </summary>
        /// <param name="commonNoteEntity"></param>
        /// <param name="commonNote_ItemList"></param>
        private void SetCommonNoteTabByItem(CommonNoteEntity commonNoteEntity, List<CommonNote_ItemEntity> commonNote_ItemList)
        {
            try
            {
                if (commonNoteEntity == null || commonNoteEntity.CommonNote_TabList == null) return;
                foreach (var commonTab in commonNoteEntity.CommonNote_TabList)
                {
                    if (commonTab != null)
                    {
                        List<CommonNote_ItemEntity> commonNoteiemList = new List<CommonNote_ItemEntity>();
                        foreach (var item in commonNote_ItemList)
                        {
                            if (item.CommonNote_Tab_Flow == commonTab.CommonNote_Tab_Flow)
                            {
                                commonNoteiemList.Add(item);
                            }
                        }
                        commonTab.CommonNote_ItemList = commonNoteiemList.OrderBy(a => Convert.ToInt32(a.OrderCode)).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }


        /// <summary>
        /// 保存整体通用单
        /// </summary>
        /// <param name="commonNoteEntity"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool SaveCommonNoteAll(CommonNoteEntity commonNoteEntity, ref string message)
        {
            try
            {
                if (commonNoteEntity == null)
                {
                    message = "保存的通用单为空";
                    return false;
                }
                SaveCommonNote(commonNoteEntity);
                SaveCommonNoteSiteRef(commonNoteEntity);
                foreach (var commonNote_TabItem in commonNoteEntity.CommonNote_TabList)
                {
                    commonNote_TabItem.CommonNoteFlow = commonNoteEntity.CommonNoteFlow;
                    SaveCommonNoteTab(commonNote_TabItem);
                    if (commonNote_TabItem.CommonNote_ItemList == null)
                    {
                        continue;
                    }
                    foreach (var item in commonNote_TabItem.CommonNote_ItemList)
                    {
                        item.CommonNoteFlow = commonNoteEntity.CommonNoteFlow;
                        item.CommonNote_Tab_Flow = commonNote_TabItem.CommonNote_Tab_Flow;
                        SaveCommonNoteItem(item);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }

        }
        //通用单项目
        private void SaveCommonNote(CommonNoteEntity commonNoteEntity)
        {
            try
            {
                if (string.IsNullOrEmpty(commonNoteEntity.CommonNoteFlow))
                {
                    commonNoteEntity.CommonNoteFlow = Guid.NewGuid().ToString();
                    commonNoteEntity.Valide = "1";
                }
                if (string.IsNullOrEmpty(commonNoteEntity.CreateDoctorID) || string.IsNullOrEmpty(commonNoteEntity.CreateDoctorName))
                {
                    commonNoteEntity.CreateDoctorID = m_app.User.DoctorId;
                    commonNoteEntity.CreateDoctorName = m_app.User.DoctorName;
                }
                DrectSoft.Core.GenerateShortCode generateShortCode = new DrectSoft.Core.GenerateShortCode(m_app.SqlHelper);
                string[] strpywbs = generateShortCode.GenerateStringShortCode(commonNoteEntity.CommonNoteName);
                if (strpywbs.Length == 2)
                {
                    commonNoteEntity.PYM = strpywbs[0];
                    commonNoteEntity.WBM = strpywbs[1];
                }

                SqlParameter[] sqlParams = new SqlParameter[] { 
                  new SqlParameter("@CommonNoteFlow", SqlDbType.VarChar, 50),
                  new SqlParameter("@CommonNoteName", SqlDbType.VarChar, 50),
                  new SqlParameter("@PrinteModelName", SqlDbType.VarChar, 50),
                  new SqlParameter("@ShowType", SqlDbType.VarChar, 50),
                  new SqlParameter("@CreateDoctorID", SqlDbType.VarChar, 50),
                  new SqlParameter("@CreateDoctorName", SqlDbType.VarChar, 50),
                  new SqlParameter("@UsingFlag", SqlDbType.VarChar, 1),
                  new SqlParameter("@Valide", SqlDbType.VarChar, 1),
                   new SqlParameter("@PYM", SqlDbType.VarChar, 50),
                    new SqlParameter("@WBM", SqlDbType.VarChar, 50),
                    new SqlParameter("@UsingPicSign", SqlDbType.VarChar, 1),
                    new SqlParameter("@UsingCheckDoc", SqlDbType.VarChar, 1)
            };

                sqlParams[0].Value = commonNoteEntity.CommonNoteFlow;
                sqlParams[1].Value = commonNoteEntity.CommonNoteName;
                sqlParams[2].Value = commonNoteEntity.PrinteModelName;
                sqlParams[3].Value = commonNoteEntity.ShowType;
                sqlParams[4].Value = commonNoteEntity.CreateDoctorID;
                sqlParams[5].Value = commonNoteEntity.CreateDoctorName;
                sqlParams[6].Value = commonNoteEntity.UsingFlag;
                sqlParams[7].Value = commonNoteEntity.Valide;
                sqlParams[8].Value = commonNoteEntity.PYM;
                sqlParams[9].Value = commonNoteEntity.WBM;
                sqlParams[10].Value = commonNoteEntity.UsingPicSign;
                sqlParams[11].Value = commonNoteEntity.UsingCheckDoc;
                m_app.SqlHelper.ExecuteNoneQuery("EMR_CommonNote.usp_AddOrModCommonNote", sqlParams, CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        private void SaveCommonNoteSiteRef(CommonNoteEntity commonNoteEntity)
        {

            try
            {
                //先删除所有的对应科室和病区 再重新添加
                SqlParameter[] sqlParamsDel = new SqlParameter[] { 
                  new SqlParameter("@CommonNoteFlow", SqlDbType.VarChar, 50)
            };
                sqlParamsDel[0].Value = commonNoteEntity.CommonNoteFlow;
                m_app.SqlHelper.ExecuteNoneQuery("EMR_CommonNote.usp_DelCommonNote_Site", sqlParamsDel, CommandType.StoredProcedure);

                foreach (var item in commonNoteEntity.BaseDepartments)
                {
                    SqlParameter[] sqlParams = new SqlParameter[] 
                { 
                  new SqlParameter("@Site_Flow", SqlDbType.VarChar, 50),
                  new SqlParameter("@CommonNoteFlow", SqlDbType.VarChar, 50),
                  new SqlParameter("@RelationType", SqlDbType.VarChar, 50),
                  new SqlParameter("@Site_ID", SqlDbType.VarChar, 50),
                  new SqlParameter("@Valide", SqlDbType.VarChar, 50)
                 };
                    sqlParams[0].Value = Guid.NewGuid().ToString();
                    sqlParams[1].Value = commonNoteEntity.CommonNoteFlow;
                    sqlParams[2].Value = "01";
                    sqlParams[3].Value = item.Id;
                    sqlParams[4].Value = "1";
                    m_app.SqlHelper.ExecuteNoneQuery("EMR_CommonNote.usp_AddOrModCommonNote_Site", sqlParams, CommandType.StoredProcedure);
                }

                foreach (var item in commonNoteEntity.BaseAreas)
                {
                    SqlParameter[] sqlParams = new SqlParameter[] { 
                  new SqlParameter("@Site_Flow", SqlDbType.VarChar, 50),
                  new SqlParameter("@CommonNoteFlow", SqlDbType.VarChar, 50),
                      new SqlParameter("@RelationType", SqlDbType.VarChar, 50),
                  new SqlParameter("@Site_ID", SqlDbType.VarChar, 50),
                  new SqlParameter("@Valide", SqlDbType.VarChar, 50)
                 
            };
                    sqlParams[0].Value = Guid.NewGuid().ToString();
                    sqlParams[1].Value = commonNoteEntity.CommonNoteFlow;
                    sqlParams[2].Value = "02";
                    sqlParams[3].Value = item.Id;
                    sqlParams[4].Value = "1";
                    m_app.SqlHelper.ExecuteNoneQuery("EMR_CommonNote.usp_AddOrModCommonNote_Site", sqlParams, CommandType.StoredProcedure);
                }

            }
            catch (Exception ex)
            {
                
                throw ex;
            }

        }

        //通用单标签
        //
        private void SaveCommonNoteTab(CommonNote_TabEntity commonNote_TabEntity)
        {
            try
            {
                if (string.IsNullOrEmpty(commonNote_TabEntity.CommonNote_Tab_Flow))
                    commonNote_TabEntity.CommonNote_Tab_Flow = Guid.NewGuid().ToString();
                if (string.IsNullOrEmpty(commonNote_TabEntity.CreateDoctorID) || string.IsNullOrEmpty(commonNote_TabEntity.CreateDoctorName))
                {
                    commonNote_TabEntity.CreateDoctorID = m_app.User.DoctorId;
                    commonNote_TabEntity.CreateDoctorName = m_app.User.DoctorName;
                }
                SqlParameter[] sqlParams = new SqlParameter[] { 
                  new SqlParameter("@CommonNote_Tab_Flow", SqlDbType.VarChar, 50),
                  new SqlParameter("@CommonNoteFlow", SqlDbType.VarChar, 50),
                      new SqlParameter("@CommonNoteTabName", SqlDbType.VarChar, 50),
                  new SqlParameter("@UsingRole", SqlDbType.VarChar, 50),
                  new SqlParameter("@ShowType", SqlDbType.VarChar, 50),
                  new SqlParameter("@OrderCode", SqlDbType.VarChar, 50),
                  new SqlParameter("@CreateDoctorID", SqlDbType.VarChar, 50),
                  new SqlParameter("@CreateDoctorName", SqlDbType.VarChar, 50),
                  new SqlParameter("@Valide", SqlDbType.VarChar, 1),
                  new SqlParameter("@RowMax",SqlDbType.VarChar,50)
            };

                sqlParams[0].Value = commonNote_TabEntity.CommonNote_Tab_Flow;
                sqlParams[1].Value = commonNote_TabEntity.CommonNoteFlow;
                sqlParams[2].Value = commonNote_TabEntity.CommonNoteTabName;
                sqlParams[3].Value = commonNote_TabEntity.UsingRole;
                sqlParams[4].Value = commonNote_TabEntity.ShowType;
                sqlParams[5].Value = commonNote_TabEntity.OrderCode;
                sqlParams[6].Value = commonNote_TabEntity.CreateDoctorID;
                sqlParams[7].Value = commonNote_TabEntity.CreateDoctorName;
                sqlParams[8].Value = commonNote_TabEntity.Valide;
                sqlParams[9].Value = commonNote_TabEntity.MaxRows;

                m_app.SqlHelper.ExecuteNoneQuery("EMR_CommonNote.usp_AddOrModCommonNote_Tab", sqlParams, CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        //通用单项目
        private void SaveCommonNoteItem(CommonNote_ItemEntity commonNote_ItemEntity)
        {
            try
            {
                if (string.IsNullOrEmpty(commonNote_ItemEntity.CommonNote_Item_Flow))
                    commonNote_ItemEntity.CommonNote_Item_Flow = Guid.NewGuid().ToString();
                if (string.IsNullOrEmpty(commonNote_ItemEntity.CreateDoctorID) || string.IsNullOrEmpty(commonNote_ItemEntity.CreateDoctorName))
                {
                    commonNote_ItemEntity.CreateDoctorID = m_app.User.DoctorId;
                    commonNote_ItemEntity.CreateDoctorName = m_app.User.DoctorName;
                }
                SqlParameter[] sqlParams = new SqlParameter[] { 
                  new SqlParameter("@CommonNote_Item_Flow", SqlDbType.VarChar, 50),
                  new SqlParameter("@CommonNote_Tab_Flow", SqlDbType.VarChar, 50),
                  new SqlParameter("@CommonNoteFlow", SqlDbType.VarChar, 50),
                  new SqlParameter("@DataElementFlow", SqlDbType.VarChar, 50),
                  new SqlParameter("@DataElementId", SqlDbType.VarChar, 50),
                  new SqlParameter("@DataElementName", SqlDbType.VarChar, 50),
                  new SqlParameter("@OrderCode", SqlDbType.VarChar, 50),
                  new SqlParameter("@IsValidate", SqlDbType.VarChar, 50),
                  new SqlParameter("@CreateDoctorID", SqlDbType.VarChar, 50),
                  new SqlParameter("@CreateDoctorName", SqlDbType.VarChar, 50),
                  new SqlParameter("@Valide", SqlDbType.VarChar, 50),
                  new SqlParameter("@OtherName", SqlDbType.VarChar, 50)
                  
            };

                sqlParams[0].Value = commonNote_ItemEntity.CommonNote_Item_Flow;
                sqlParams[1].Value = commonNote_ItemEntity.CommonNote_Tab_Flow;
                sqlParams[2].Value = commonNote_ItemEntity.CommonNoteFlow;
                sqlParams[3].Value = commonNote_ItemEntity.DataElementFlow;
                sqlParams[4].Value = commonNote_ItemEntity.DataElementId;
                sqlParams[5].Value = commonNote_ItemEntity.DataElementName;
                sqlParams[6].Value = commonNote_ItemEntity.OrderCode;
                sqlParams[7].Value = commonNote_ItemEntity.IsValidate;
                sqlParams[8].Value = commonNote_ItemEntity.CreateDoctorID;
                sqlParams[9].Value = commonNote_ItemEntity.CreateDoctorName;
                sqlParams[10].Value = commonNote_ItemEntity.Valide;
                sqlParams[11].Value = commonNote_ItemEntity.OtherName;
                m_app.SqlHelper.ExecuteNoneQuery("EMR_CommonNote.usp_AddOrModCommonNote_Item", sqlParams, CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        /// <summary>
        /// 删除配置单
        /// </summary>
        /// <param name="commonNoteEntity"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool DelCommonNote(CommonNoteEntity commonNoteEntity, ref string message)
        {
            commonNoteEntity.Valide = "0";
            try
            {
                SaveCommonNote(commonNoteEntity);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }

        }

        /// <summary>
        /// 获取所有科室和病区 只有id和name
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<BaseDictory>> GetAllDepartAndAreas()
        {
            try
            {
                string sqlDeptAll = @"select de.id, de.name from department de where de.valid = '1' and de.sort='101'";
                string sqlWardAll = @" select ward.id, ward.name from ward where ward.valid = '1'";
                DataTable dtDepts = m_app.SqlHelper.ExecuteDataTable(sqlDeptAll, CommandType.Text);
                DataTable dtWards = m_app.SqlHelper.ExecuteDataTable(sqlWardAll, CommandType.Text);
                Dictionary<string, List<BaseDictory>> basedictoryList = new Dictionary<string, List<BaseDictory>>();

                List<BaseDictory> baseDepart = DataTableToList<BaseDictory>.ConvertToModel(dtDepts);
                basedictoryList.Add("01", baseDepart);
                List<BaseDictory> baseAreas = DataTableToList<BaseDictory>.ConvertToModel(dtWards);
                basedictoryList.Add("02", baseAreas);
                return basedictoryList;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetPrintTem()
        {
            try
            {
                string sqlDate = @"select tempname,tempdesc  from commonnoteprinttemp where valide='1';";
                DataTable dtPrint = m_app.SqlHelper.ExecuteDataTable(sqlDate, CommandType.Text);
                return dtPrint;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }


        /// <summary>
        /// 通用单据匹配使用
        /// </summary>
        /// <param name="commnoteflow"></param>
        /// <returns></returns>
        public static CommonNoteCountEntity GetCommonNoteCount(string commnoteflow)
        {
            try
            {
                string sqlStr = string.Format(@"select * from commonnotecount c where c.commonnoteflow='{0}'", commnoteflow);
                DataTable dt = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sqlStr, CommandType.Text);
                CommonNoteCountEntity commonNoteCountEntity = DataTableToList<CommonNoteCountEntity>.ConvertToModelOne(dt);
                return commonNoteCountEntity;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }


        /// <summary>
        /// 添加或修改通用单据统计
        /// </summary>
        /// <param name="commonNoteCountEntity"></param>
        public static void AddOrModCommonNoteEntity(CommonNoteCountEntity commonNoteCountEntity)
        {
            try
            {
                if (string.IsNullOrEmpty(commonNoteCountEntity.CommonNoteCountFlow))
                {
                    commonNoteCountEntity.CommonNoteCountFlow = Guid.NewGuid().ToString();
                }
                SqlParameter[] sqlParams = new SqlParameter[] { 
                  new SqlParameter("@CommonNoteCountFlow", SqlDbType.VarChar, 50),
                  new SqlParameter("@CommonNoteFlow", SqlDbType.VarChar, 50),
                  new SqlParameter("@ItemCount", SqlDbType.VarChar, 50),
                  new SqlParameter("@Hour12Name", SqlDbType.VarChar, 50),
                  new SqlParameter("@Hour12Time", SqlDbType.VarChar),
                  new SqlParameter("@Hour24Name", SqlDbType.VarChar),
                  new SqlParameter("@Hour24Time", SqlDbType.VarChar),
                  new SqlParameter("@Valide", SqlDbType.VarChar, 50)
                  
            };

                sqlParams[0].Value = commonNoteCountEntity.CommonNoteCountFlow;
                sqlParams[1].Value = commonNoteCountEntity.CommonNoteFlow;
                sqlParams[2].Value = commonNoteCountEntity.ItemCount;
                sqlParams[3].Value = commonNoteCountEntity.Hour12Name;
                sqlParams[4].Value = commonNoteCountEntity.Hour12Time;
                sqlParams[5].Value = commonNoteCountEntity.Hour24Name;
                sqlParams[6].Value = commonNoteCountEntity.Hour24Time;
                sqlParams[7].Value = commonNoteCountEntity.Valide;

                DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteNonQuery("EMR_CommonNote.usp_AddOrModCommonNoteCount", sqlParams, CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }



    }
}
