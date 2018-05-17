using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Library.EmrEditor.Src.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DrectSoft.Core.CommonTableConfig
{
    public class DataElemntBiz
    {
        private IEmrHost m_app;
        public DataElemntBiz(IEmrHost app)
        {
            this.m_app = app;
        }

        /// <summary>
        /// 根据条件获取数据元 用于展示
        /// </summary>
        /// <param name="dataElementSearch"></param>
        /// <returns></returns>
        public List<DataElementEntity> GetDataElement(DataElementEntity dataElementSearch)
        {
            SqlParameter[] sqlParams = new SqlParameter[] { 
                  new SqlParameter("@ElementId", SqlDbType.VarChar, 50),
                  new SqlParameter("@ElementName", SqlDbType.VarChar, 50),
                  new SqlParameter("@ElementClass", SqlDbType.VarChar, 50),
                  new SqlParameter("@ElementPYM", SqlDbType.VarChar, 50)
            };
            if (dataElementSearch.ElementPYM == null)
                dataElementSearch.ElementPYM = "";
            dataElementSearch.ElementPYM = dataElementSearch.ElementPYM.ToUpper();
            sqlParams[0].Value = dataElementSearch.ElementId;
            sqlParams[1].Value = dataElementSearch.ElementName;
            sqlParams[2].Value = dataElementSearch.ElementClass;
            sqlParams[3].Value = dataElementSearch.ElementPYM;
            DataTable dataTableElement = m_app.SqlHelper.ExecuteDataTable("EMR_CommonNote.usp_GetDateElement", sqlParams, CommandType.StoredProcedure);
            List<DataElementEntity> dataElementEntityList = DataTableToList<DataElementEntity>.ConvertToModel(dataTableElement);
            ConvertIsDataElementPropery(dataElementEntityList, 1);
            ConvertDataElemnetClass(dataElementEntityList, 1);
            return dataElementEntityList;
        }

        /// <summary>
        /// 通过流水号获取数据元
        /// </summary>
        /// <param name="dataElementFlow"></param>
        /// <returns></returns>
        public DataElementEntity GetDataElement(string dataElementFlow)
        {
            SqlParameter[] sqlParams = new SqlParameter[] { 
                  new SqlParameter("@ElementFlow", SqlDbType.VarChar, 50)

            };

            sqlParams[0].Value = dataElementFlow;

            DataTable dataTableElement = m_app.SqlHelper.ExecuteDataTable("EMR_CommonNote.usp_GetDateElementOne", sqlParams, CommandType.StoredProcedure);
            DataElementEntity dataElementEntity = DataTableToList<DataElementEntity>.ConvertToModelOne(dataTableElement);
            return dataElementEntity;
        }


        /// <summary>
        /// 保存数据元 包括修改和添加
        /// </summary>
        /// <param name="dataElement"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool SaveDataElement(DataElementEntity dataElement, ref string message)
        {
            bool result = true;
            if (string.IsNullOrEmpty(dataElement.ElementFlow))
            {
                result = AddDateElement(dataElement, ref message);
            }
            else
            {
                result = UpDateElement(dataElement, ref message);
            }
            return result;
        }

        /// <summary>
        /// 删除数据元
        /// </summary>
        /// <param name="dataElement"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool DelDataElement(DataElementEntity dataElement, ref string message)
        {
            ConvertIsDataElementOne(dataElement, 0);
            ConvertDataElemnetClass(dataElement, 0);
            dataElement.Valid = "0";
            bool result = UpDateElement(dataElement, ref message);
            return result;
        }


        private bool AddDateElement(DataElementEntity dataElement, ref string message)
        {
            bool validateResult = ValidateDateElement(dataElement.ElementId);
            if (!validateResult)
            {
                message = "数据元ID存在，请重新输入";
                return false;
            }
            else
            {
                SqlParameter[] sqlParams = new SqlParameter[] { 
                  new SqlParameter("@ElementFlow", SqlDbType.VarChar, 50),
                  new SqlParameter("@ElementId", SqlDbType.VarChar, 50),
                  new SqlParameter("@ElementName", SqlDbType.VarChar, 50),
                  new SqlParameter("@ElementType", SqlDbType.VarChar, 50),
                  new SqlParameter("@ElementForm", SqlDbType.VarChar, 50),
                  new SqlParameter("@ElementRange", SqlDbType.VarChar, 50),
                  new SqlParameter("@ElementDescribe", SqlDbType.VarChar, 50),
                  new SqlParameter("@ElementClass", SqlDbType.VarChar, 50),
                  new SqlParameter("@IsDataElemet", SqlDbType.VarChar, 50),
                  new SqlParameter("@ElementPYM", SqlDbType.VarChar, 50)
            };

                dataElement.ElementFlow = Guid.NewGuid().ToString();
                dataElement.Valid = "1";
                dataElement.ElementPYM = StringCommon.GetChineseSpell(dataElement.ElementName);
                if (dataElement.ElementPYM == null || dataElement.ElementPYM == "")
                    dataElement.ElementPYM = dataElement.ElementName;
                sqlParams[0].Value = dataElement.ElementFlow;
                sqlParams[1].Value = dataElement.ElementId;
                sqlParams[2].Value = dataElement.ElementName;
                sqlParams[3].Value = dataElement.ElementType;
                sqlParams[4].Value = dataElement.ElementForm;
                sqlParams[5].Value = dataElement.ElementRange;
                sqlParams[6].Value = dataElement.ElementDescribe;
                sqlParams[7].Value = dataElement.ElementClass;
                sqlParams[8].Value = dataElement.IsDataElemet;
                sqlParams[9].Value = dataElement.ElementPYM;
                try
                {
                    m_app.SqlHelper.ExecuteNoneQuery("EMR_CommonNote.usp_InsertElement", sqlParams, CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    return false;
                }
                return true;
            }
        }

        //验证数据元是否存在
        private bool ValidateDateElement(string dataElementId)
        {
            SqlParameter[] sqlParams = new SqlParameter[] { 
                  new SqlParameter("@ElementId", SqlDbType.VarChar, 50)
            };
            sqlParams[0].Value = dataElementId;
            DataTable dataTableElement = m_app.SqlHelper.ExecuteDataTable("EMR_CommonNote.usp_ValiDateElement", sqlParams, CommandType.StoredProcedure);
            if (dataTableElement != null && dataTableElement.Rows != null && dataTableElement.Rows.Count > 0)
                return false;
            else
                return true;
        }

        //更新数据元数据
        private bool UpDateElement(DataElementEntity dataElement, ref string message)
        {
            SqlParameter[] sqlParams = new SqlParameter[] {
                  new SqlParameter("@ElementFlow", SqlDbType.VarChar, 50),
                  new SqlParameter("@ElementId", SqlDbType.VarChar, 50),
                  new SqlParameter("@ElementName", SqlDbType.VarChar, 50),
                  new SqlParameter("@ElementType", SqlDbType.VarChar, 50),
                  new SqlParameter("@ElementForm", SqlDbType.VarChar, 50),
                  new SqlParameter("@ElementRange", SqlDbType.VarChar, 50),
                  new SqlParameter("@ElementDescribe", SqlDbType.VarChar, 50),
                  new SqlParameter("@ElementClass", SqlDbType.VarChar, 50),
                  new SqlParameter("@IsDataElemet", SqlDbType.VarChar, 50),
                  new SqlParameter("@ElementPYM", SqlDbType.VarChar, 1),
                    new SqlParameter("@Valid", SqlDbType.VarChar, 1)
            };
            //dataElement.ElementFlow = Guid.NewGuid().ToString();
            dataElement.ElementPYM = StringCommon.GetChineseSpell(dataElement.ElementName);
            if (dataElement.ElementPYM == null || dataElement.ElementPYM == "")
                dataElement.ElementPYM = dataElement.ElementName;
            sqlParams[0].Value = dataElement.ElementFlow;
            sqlParams[1].Value = dataElement.ElementId;
            sqlParams[2].Value = dataElement.ElementName;
            sqlParams[3].Value = dataElement.ElementType;
            sqlParams[4].Value = dataElement.ElementForm;
            sqlParams[5].Value = dataElement.ElementRange;
            sqlParams[6].Value = dataElement.ElementDescribe;
            sqlParams[7].Value = dataElement.ElementClass;
            sqlParams[8].Value = dataElement.IsDataElemet;
            sqlParams[9].Value = dataElement.ElementPYM;
            sqlParams[10].Value = dataElement.Valid;
            try
            {
                m_app.SqlHelper.ExecuteNoneQuery("EMR_CommonNote.usp_UpDateElement", sqlParams, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
            return true;
        }


        /// <summary>
        /// 转换数据元的是否是卫生部发布属性
        /// 0将汉字转数字 1将数字转汉字
        /// </summary>
        /// <param name="dataElementEntityList"></param>
        /// <param name="Type">0将汉字转数字 1将数字转汉字</param>
        private void ConvertIsDataElementPropery(List<DataElementEntity> dataElementEntityList, int Type)
        {
            foreach (var item in dataElementEntityList)
            {
                ConvertIsDataElementOne(item, Type);
            }
        }

        /// <summary>
        /// 转换数据元的是否是卫生部发布属性
        /// 0将汉字转数字 1将数字转汉字
        /// </summary>
        /// <param name="dataElementEntityList"></param>
        /// <param name="Type">0将汉字转数字 1将数字转汉字</param>
        public void ConvertIsDataElementOne(DataElementEntity item, int Type)
        {
            switch (Type)
            {
                case 0:
                    if (item.IsDataElemet == "是")
                        item.IsDataElemet = "1";
                    else if (item.IsDataElemet == "不是")
                        item.IsDataElemet = "0";
                    break;
                case 1:
                    if (item.IsDataElemet == "0")
                        item.IsDataElemet = "不是";
                    else if (item.IsDataElemet == "1")
                        item.IsDataElemet = "是";
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 转换数据元的数据元所属类别属性
        /// 0将汉字转数字 1将数字转汉字
        /// </summary>
        /// <param name="dataElementEntityList"></param>
        /// <param name="Type"></param>
        private void ConvertDataElemnetClass(List<DataElementEntity> dataElementEntityList, int Type)
        {

            foreach (var item in dataElementEntityList)
            {
                ConvertDataElemnetClass(item, Type);
            }
        }

        /// <summary>
        /// 转换数据元的数据元所属类别属性
        /// 0将汉字转数字 1将数字转汉字
        /// </summary>
        /// <param name="dataElementEntityList"></param>
        /// <param name="Type"></param>
        public void ConvertDataElemnetClass(DataElementEntity item, int Type)
        {
            var baseTypeList = CommonTabHelper.GetAllDataElemnetClass();
            switch (Type)
            {
                case 0:
                    foreach (var basetype in baseTypeList)
                    {
                        if (basetype.Name == item.ElementClass)
                            item.ElementClass = basetype.Id;
                    }
                    break;
                case 1:
                    foreach (var basetype in baseTypeList)
                    {
                        if (basetype.Id == item.ElementClass)
                            item.ElementClass = basetype.Name;
                    }
                    break;
                default:
                    break;
            }
        }


    }
}
