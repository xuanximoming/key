using DrectSoft.Common;
using DrectSoft.DSSqlHelper;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{

    /// <summary>
    /// 通用单据使用 业务逻辑层 主要是增删改查的实现
    /// </summary>
    public class InCommonNoteBiz
    {
        private IEmrHost m_app;
        DataElemntBiz m_DataElemntBiz;
        public InCommonNoteBiz(IEmrHost app)
        {
            this.m_app = app;
            m_DataElemntBiz = new DataElemntBiz(app);
            DS_SqlHelper.CreateSqlHelper();
        }

        #region 增删改查

        /// <summary>
        /// 获取当前病区当前科室的所有在院病人
        /// xlb 2013-01-18
        /// </summary>
        /// <returns></returns>
        public List<InPatientSim> GetAllInpatient(string outhosdept, string outhosward)
        {
            try
            {
                SqlParameter[] sps ={
                                   new SqlParameter("@outhosdept",SqlDbType.VarChar,50),
                                   new SqlParameter("@outhosward",SqlDbType.VarChar,50)
                                   };
                sps[0].Value = outhosdept;
                sps[1].Value = outhosward;

                DataTable currtentInpat = this.m_app.SqlHelper.ExecuteDataTable("EMR_CommonNote.usp_GetDeptAndWardInpatient", sps, CommandType.StoredProcedure);
                List<InPatientSim> currentInpatlist = DataTableToList<InPatientSim>.ConvertToModel(currtentInpat);
                return currentInpatlist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 获取某病人的通用单
        /// </summary>
        /// <param name="CommonNoteFlow"></param>
        /// <param name="noofInpat"></param>
        /// <param name="deptCode"></param>
        /// <param name="wardCode"></param>
        /// <returns></returns>
        public List<InCommonNoteEnmtity> GetSimInCommonNote(string noofInpat)
        {
            try
            {

                SqlParameter[] sqlParams = new SqlParameter[]
           {
                new SqlParameter("@noofInpant",SqlDbType.VarChar,50),
           };
                sqlParams[0].Value = noofInpat;
                DataTable inCommonNoteDT = m_app.SqlHelper.ExecuteDataTable("EMR_CommonNote.usp_GetSimInCommonNote", sqlParams, CommandType.StoredProcedure);
                List<InCommonNoteEnmtity> inCommonNoteList = DataTableToList<InCommonNoteEnmtity>.ConvertToModel(inCommonNoteDT);
                return inCommonNoteList;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// 获取使用单据是否有数据
        /// xlb 2013-01-31
        /// </summary>
        /// <param name="incommonNoteFlow"></param>
        /// <returns></returns>
        public int GetCommonItemCount(string incommonNoteFlow)
        {
            try
            {
                string m_sqlItemCount = string.Format(@"select count(*) from incommonnote_item_view 
                                          where incommonnoteflow='{0}'  and valide='1'", incommonNoteFlow);
                //SqlParameter[] sps = { new SqlParameter("@incommonNoteFlow", incommonNoteFlow) };
                // DataTable dtItemCount = m_app.SqlHelper.ExecuteDataTable(m_sqlItemCount, sps, CommandType.Text);
                DataTable dtItemCount = DS_SqlHelper.ExecuteDataTable(m_sqlItemCount, CommandType.Text);
                if (dtItemCount == null || dtItemCount.Rows.Count <= 0)
                {
                    return 0;
                }
                return Convert.ToInt32(dtItemCount.Rows[0][0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取某一表格的数据量
        /// </summary>
        /// <param name="incommnotetabflow"></param>
        /// <returns></returns>
        public int GetRowCount(string incommnotetabflow)
        {
            try
            {
                string sqlcount = string.Format(@"select count(1) from incommonnote_row r where r.valide='1' and r.incommonnote_tab_flow='{0}'", incommnotetabflow);
                DataTable dt = DS_SqlHelper.ExecuteDataTable(sqlcount, CommandType.Text);
                return Convert.ToInt32(dt.Rows[0][0]);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        /// <summary>
        /// 获取某病人的通用单
        /// </summary>
        /// <param name="CommonNoteFlow"></param>
        /// <param name="noofInpat"></param>
        /// <param name="deptCode"></param>
        /// <param name="wardCode"></param>
        /// <returns></returns>
        public List<InCommonNoteEnmtity> GetSimInCommonNoteByFlow(string noofInpat, string commonnoteFlow)
        {
            try
            {

                SqlParameter[] sqlParams = new SqlParameter[]
           {
                new SqlParameter("@noofInpant",SqlDbType.VarChar,50),
                 new SqlParameter("@commonnoteflow",SqlDbType.VarChar)
           };
                sqlParams[0].Value = noofInpat;
                sqlParams[1].Value = commonnoteFlow;
                DataTable inCommonNoteDT = m_app.SqlHelper.ExecuteDataTable("EMR_CommonNote.usp_GetSimInCommonNoteByFlow", sqlParams, CommandType.StoredProcedure);
                List<InCommonNoteEnmtity> inCommonNoteList = DataTableToList<InCommonNoteEnmtity>.ConvertToModel(inCommonNoteDT);
                return inCommonNoteList;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 获取某通用单的详细内容
        /// </summary>
        /// <param name="inCommonNote"></param>
        public void GetDetaliInCommonNote(ref InCommonNoteEnmtity inCommonNote)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[]
                                        {
                new SqlParameter("@InCommonNoteFlow",SqlDbType.VarChar,50)

                                         };
                sqlParams[0].Value = inCommonNote.InCommonNoteFlow;
                DataSet inCommonNoteDS = m_app.SqlHelper.ExecuteDataSet("EMR_CommonNote.usp_GetDetailInCommonNote", sqlParams, CommandType.StoredProcedure);
                inCommonNote = DataTableToList<InCommonNoteEnmtity>.ConvertToModelOne(inCommonNoteDS.Tables[0]);

                List<InCommonNoteTabEntity> inCommonNoteTabList = DataTableToList<InCommonNoteTabEntity>.ConvertToModel(inCommonNoteDS.Tables[1]);
                List<InCommonNoteItemEntity> inCommonNoteItemList = DataTableToList<InCommonNoteItemEntity>.ConvertToModel(inCommonNoteDS.Tables[2]);
                SetInCommonTabAndItem(inCommonNoteTabList, inCommonNoteItemList);
                inCommonNote.InCommonNoteTabList = inCommonNoteTabList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取tab页的信息
        /// </summary>
        /// <param name="inCommonNote"></param>
        public void GetIncommTab(ref InCommonNoteEnmtity inCommonNote)
        {
            string strSql = string.Format("select * from incommonnote_tab icomtab where icomtab.incommonnoteflow = '{0}' and icomtab.valide = '1' order by icomtab.ordercode", inCommonNote.InCommonNoteFlow);
            DataTable dt = DS_SqlHelper.ExecuteDataTable(strSql, CommandType.Text);
            List<InCommonNoteTabEntity> inCommonNoteTabList = DataTableToList<InCommonNoteTabEntity>.ConvertToModel(dt);
            inCommonNote.InCommonNoteTabList = inCommonNoteTabList;
        }


        /// <summary>
        /// 获取某通用单的详细内容
        /// </summary>
        /// <param name="inCommonNote"></param>
        public void GetDetaliInCommonNoteByDay(ref InCommonNoteEnmtity inCommonNote, ref InCommonNoteTabEntity inCommonNoteTab, string startDate, string EndDate)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[]
                                        {
                new SqlParameter("@InCommonNoteFlow",SqlDbType.VarChar,50),
                new SqlParameter("@incommonnote_tabflow",SqlDbType.VarChar,50),
                new SqlParameter("@StartDate",SqlDbType.VarChar,50), 
                new SqlParameter("@EndDate",SqlDbType.VarChar,50),
                new SqlParameter("@result",SqlDbType.Structured,50),
                new SqlParameter("@result1",SqlDbType.Structured,50),
                new SqlParameter("@result2",SqlDbType.Structured,50)
                                         };
                sqlParams[0].Value = inCommonNote.InCommonNoteFlow;
                sqlParams[1].Value = inCommonNoteTab.InCommonNote_Tab_Flow;
                sqlParams[2].Value = startDate;
                sqlParams[3].Value = EndDate;
                sqlParams[4].Direction = ParameterDirection.Output;
                sqlParams[5].Direction = ParameterDirection.Output;
                sqlParams[6].Direction = ParameterDirection.Output;
                DataSet inCommonNoteDS = DS_SqlHelper.ExecuteDataSet("EMR_CommonNote.usp_GetDetailInCommonNoteByDay", sqlParams, CommandType.StoredProcedure);
                inCommonNote = DataTableToList<InCommonNoteEnmtity>.ConvertToModelOne(inCommonNoteDS.Tables[0]);
                List<InCommonNoteTabEntity> inCommonNoteTabList = DataTableToList<InCommonNoteTabEntity>.ConvertToModel(inCommonNoteDS.Tables[1]);
                List<InCommonNoteItemEntity> inCommonNoteItemList = DataTableToList<InCommonNoteItemEntity>.ConvertToModel(inCommonNoteDS.Tables[2]);
                SetInCommonTabAndItem(inCommonNoteTabList, inCommonNoteItemList);
                inCommonNote.InCommonNoteTabList = inCommonNoteTabList;
                string tabflow = inCommonNoteTab.InCommonNote_Tab_Flow;
                inCommonNoteTab = inCommonNote.InCommonNoteTabList.Find(a => a.InCommonNote_Tab_Flow == tabflow);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取该使用单据的使用别名
        /// </summary>
        /// <param name="commonNote_TabEntity"></param>
        /// <param name="incommontabflow"></param>
        public void SetCommonTabOtherName(CommonNote_TabEntity commonNote_TabEntity, string incommontabflow)
        {
            try
            {
                if (commonNote_TabEntity == null || commonNote_TabEntity.CommonNote_ItemList == null) return;
                string strsql = string.Format(@"select * from incommonnote_column_type i where i.incommonnote_tab_flow='{0}'", incommontabflow);
                DataTable dt = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(strsql, CommandType.Text);
                if (dt == null || dt.Rows == null || dt.Rows.Count <= 0)
                {
                    return;
                }
                foreach (DataRow item in dt.Rows)
                {
                    for (int i = 0; i < commonNote_TabEntity.CommonNote_ItemList.Count; i++)
                    {
                        var citem = commonNote_TabEntity.CommonNote_ItemList[i];
                        if (citem.CommonNote_Item_Flow == item["COMMONNOTE_ITEM_FLOW"].ToString() && string.IsNullOrEmpty(citem.OtherName))
                        {
                            citem.OtherName = item["OTHERNAME"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 获取某一列使用后的值
        /// </summary>
        /// <param name="incommontabflow"></param>
        /// <param name="commonnoteItemflow"></param>
        /// <returns></returns>
        public string GetOtherNameStr(string incommontabflow, string commonnoteItemflow)
        {
            string strsql = string.Format(@"select othername from incommonnote_column_type i where i.incommonnote_tab_flow='{0}' and i.commonnote_item_flow='{1}'", incommontabflow, commonnoteItemflow);
            DataTable dt = DS_SqlHelper.ExecuteDataTable(strsql, CommandType.Text);
            if (dt == null || dt.Rows == null || dt.Rows.Count <= 0)
            {
                return "";
            }
            else
            {
                return dt.Rows[0]["OTHERNAME"].ToString();
            }
        }

        /// <summary>
        /// 设置tab与item的对应
        /// </summary>
        /// <param name="inCommonNoteTabList"></param>
        /// <param name="inCommonNoteItemList"></param>
        private void SetInCommonTabAndItem(List<InCommonNoteTabEntity> inCommonNoteTabList, List<InCommonNoteItemEntity> inCommonNoteItemList)
        {
            if (inCommonNoteTabList == null || inCommonNoteItemList == null) return;
            foreach (InCommonNoteTabEntity itemTab in inCommonNoteTabList)
            {
                foreach (InCommonNoteItemEntity commItem in inCommonNoteItemList)
                {
                    if (itemTab.InCommonNote_Tab_Flow == commItem.InCommonNote_Tab_Flow)
                    {
                        if (itemTab.InCommonNoteItemList == null)
                            itemTab.InCommonNoteItemList = new List<InCommonNoteItemEntity>();
                        itemTab.InCommonNoteItemList.Add(commItem);
                    }
                }
                if (itemTab.InCommonNoteItemList != null)
                    itemTab.InCommonNoteItemList = itemTab.InCommonNoteItemList.OrderBy(a => Convert.ToInt32(a.OrderCode)).ToList();
            }
        }

        /// <summary>
        /// 保存单个病人通用单据的所有内容
        /// </summary>
        /// <param name="inCommonNote"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool SaveInCommomNoteAll(InCommonNoteEnmtity inCommonNote, ref string message)
        {
            try
            {
                SaveInCommonNote(inCommonNote);
                CommonNoteBiz commonNoteBiz = new CommonNoteBiz(m_app);
                CommonNoteEntity commonNoteEntity = commonNoteBiz.GetDetailCommonNote(inCommonNote.CommonNoteFlow);
                if (commonNoteEntity == null || commonNoteEntity.CommonNote_TabList == null)
                {
                    message = "配置单据不存在";
                    return false;
                }
                foreach (var itemTab in inCommonNote.InCommonNoteTabList)
                {
                    itemTab.InCommonNoteFlow = inCommonNote.InCommonNoteFlow;
                    SaveInCommonNoteTab(itemTab);
                    var commonnotetab = commonNoteEntity.CommonNote_TabList.Find(a => a.CommonNote_Tab_Flow == itemTab.CommonNote_Tab_Flow);
                    SaveIncommonType(itemTab.InCommonNote_Tab_Flow, commonnotetab);
                    if (itemTab.InCommonNoteItemList == null)
                        continue;
                    string itemMessage = "";
                    foreach (var commItem in itemTab.InCommonNoteItemList)
                    {
                        commItem.InCommonNote_Tab_Flow = itemTab.InCommonNote_Tab_Flow;
                        commItem.InCommonNoteFlow = inCommonNote.InCommonNoteFlow;
                        bool itemResult = SaveIncommonNoteItem(commItem, ref itemMessage);
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


        /// <summary>
        /// 插入使用单据的列类型
        /// </summary>
        /// <param name="incommnotetabflow"></param>
        /// <param name="commnotetabflow"></param>
        public void SaveIncommonType(string incommnotetabflow, CommonNote_TabEntity commonnotetab)
        {
            try
            {
                if (commonnotetab == null || commonnotetab.CommonNote_ItemList == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("无法找到通用单据配置项");
                    return;
                }
                foreach (var item in commonnotetab.CommonNote_ItemList)
                {
                    SqlParameter[] sqlParams = new SqlParameter[]
                        {
                            new SqlParameter("@CommonNote_Item_Flow",SqlDbType.VarChar,50),
                            new SqlParameter("@InCommonNote_Tab_Flow",SqlDbType.VarChar,50),
                            new SqlParameter("@DataElementFlow",SqlDbType.VarChar,50),
                            new SqlParameter("@OtherName",SqlDbType.VarChar,50)
                        };
                    sqlParams[0].Value = item.CommonNote_Item_Flow;
                    sqlParams[1].Value = incommnotetabflow;
                    sqlParams[2].Value = item.DataElementFlow;
                    if (item.OtherName == null)
                    {
                        item.OtherName = "";
                    }
                    sqlParams[3].Value = item.OtherName;
                    DS_SqlHelper.ExecuteNonQuery("emr_commonnote.usp_AddInCommonType", sqlParams, CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        /// <summary>
        /// 更新列名
        /// </summary>
        /// <param name="InCommonNoteItemList"></param>
        public void updateIncommonType(List<InCommonNoteItemEntity> InCommonNoteItemList)
        {
            try
            {
                if (InCommonNoteItemList == null) return;
                foreach (var item in InCommonNoteItemList)
                {
                    string sql = @"update incommonnote_column_type i set i.othername=@OtherName where i.commonnote_item_flow=@CommonNote_Item_Flow and i.incommonnote_tab_flow=@InCommonNote_Tab_Flow";
                    SqlParameter[] sqlParams = new SqlParameter[]
                        {
                            new SqlParameter("@CommonNote_Item_Flow",SqlDbType.VarChar,50),
                            new SqlParameter("@InCommonNote_Tab_Flow",SqlDbType.VarChar,50),
                            new SqlParameter("@OtherName",SqlDbType.VarChar,50)
               
                        };
                    sqlParams[0].Value = item.CommonNote_Item_Flow;
                    sqlParams[1].Value = item.InCommonNote_Tab_Flow;
                    sqlParams[2].Value = item.OtherName;
                    m_app.SqlHelper.ExecuteNoneQuery(sql, sqlParams, CommandType.Text);
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        /// <summary>
        /// 病人通用单保存
        /// </summary>
        /// <param name="inCommonNote"></param>
        public void SaveInCommonNote(InCommonNoteEnmtity inCommonNote)
        {
            if (inCommonNote.InCommonNoteFlow == null)
                inCommonNote.InCommonNoteFlow = Guid.NewGuid().ToString();
            if (inCommonNote.CreateDoctorID == null)
                inCommonNote.CreateDoctorID = m_app.User.DoctorId;
            if (inCommonNote.CreateDoctorName == null)
                inCommonNote.CreateDoctorName = m_app.User.DoctorName;
            SqlParameter[] sqlParams = new SqlParameter[]
           {
                 new SqlParameter("@InCommonNoteFlow",SqlDbType.VarChar,50),
               new SqlParameter("@CommonNoteFlow",SqlDbType.VarChar,50),
               new SqlParameter("@InCommonNoteName",SqlDbType.VarChar,50),
               new SqlParameter("@PrinteModelName",SqlDbType.VarChar,50),
               new SqlParameter("@NoofInpatient",SqlDbType.VarChar,50),
               new SqlParameter("@InPatientName",SqlDbType.VarChar,50),
               new SqlParameter("@CurrDepartID",SqlDbType.VarChar,50),
               new SqlParameter("@CurrDepartName",SqlDbType.VarChar,50),
               new SqlParameter("@CurrWardID",SqlDbType.VarChar,50),
               new SqlParameter("@CurrWardName",SqlDbType.VarChar,50),
               new SqlParameter("@CreateDoctorID",SqlDbType.VarChar,50),
               new SqlParameter("@CreateDoctorName",SqlDbType.VarChar,50),
                new SqlParameter("@CreateDateTime",SqlDbType.VarChar,50),
                 new SqlParameter("@Valide",SqlDbType.VarChar,50),
                 new SqlParameter("@CheckDocId",SqlDbType.VarChar,50),
                 new SqlParameter("@CheckDocName",SqlDbType.VarChar,50)
           };
            sqlParams[0].Value = inCommonNote.InCommonNoteFlow;
            sqlParams[1].Value = inCommonNote.CommonNoteFlow;
            sqlParams[2].Value = inCommonNote.InCommonNoteName;
            sqlParams[3].Value = inCommonNote.PrinteModelName;
            sqlParams[4].Value = inCommonNote.NoofInpatient;
            sqlParams[5].Value = inCommonNote.InPatientName;
            sqlParams[6].Value = inCommonNote.CurrDepartID;
            sqlParams[7].Value = inCommonNote.CurrDepartName;
            sqlParams[8].Value = inCommonNote.CurrWardID;
            sqlParams[9].Value = inCommonNote.CurrWardName;
            sqlParams[10].Value = inCommonNote.CreateDoctorID;
            sqlParams[11].Value = inCommonNote.CreateDoctorName;
            sqlParams[12].Value = inCommonNote.CreateDateTime;
            sqlParams[13].Value = inCommonNote.Valide;
            sqlParams[14].Value = inCommonNote.CheckDocId;
            sqlParams[15].Value = inCommonNote.CheckDocName;
            m_app.SqlHelper.ExecuteNoneQuery("EMR_CommonNote.usp_AddorModInCommon", sqlParams, CommandType.StoredProcedure);
        }

        public bool DelInCommonNote(InCommonNoteEnmtity inCommonNote, ref string message)
        {
            try
            {
                string sqlDel = string.Format(@"update incommonnote set incommonnote.valide='0' where incommonnote.incommonnoteflow='{0}';", inCommonNote.InCommonNoteFlow);
                m_app.SqlHelper.ExecuteNoneQuery(sqlDel, CommandType.Text);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }

        }

        /// <summary>
        /// 病人通用单据标签保存
        /// </summary>
        /// <param name="inCommonNoteTab"></param>
        public void SaveInCommonNoteTab(InCommonNoteTabEntity inCommonNoteTab)
        {
            if (inCommonNoteTab.InCommonNote_Tab_Flow == null)
                inCommonNoteTab.InCommonNote_Tab_Flow = Guid.NewGuid().ToString();
            if (inCommonNoteTab.CreateDoctorID == null)
                inCommonNoteTab.CreateDoctorID = m_app.User.DoctorId;
            if (inCommonNoteTab.CreateDoctorName == null)
                inCommonNoteTab.CreateDoctorName = m_app.User.DoctorName;
            SqlParameter[] sqlParams = new SqlParameter[]
           {
                 new SqlParameter("@InCommonNote_Tab_Flow",SqlDbType.VarChar,50),
               new SqlParameter("@CommonNote_Tab_Flow",SqlDbType.VarChar,50),
               new SqlParameter("@InCommonNoteFlow",SqlDbType.VarChar,50),
               new SqlParameter("@CommonNoteTabName",SqlDbType.VarChar,50),
               new SqlParameter("@UsingRole",SqlDbType.VarChar,50),
               new SqlParameter("@ShowType",SqlDbType.VarChar,50),
               new SqlParameter("@OrderCode",SqlDbType.VarChar,50),
               new SqlParameter("@CreateDoctorID",SqlDbType.VarChar,50),
               new SqlParameter("@CreateDoctorName",SqlDbType.VarChar,50),
               new SqlParameter("@CreateDateTime",SqlDbType.VarChar,50),
               new SqlParameter("@Valide",SqlDbType.VarChar,50)
             
           };
            sqlParams[0].Value = inCommonNoteTab.InCommonNote_Tab_Flow;
            sqlParams[1].Value = inCommonNoteTab.CommonNote_Tab_Flow;
            sqlParams[2].Value = inCommonNoteTab.InCommonNoteFlow;
            sqlParams[3].Value = inCommonNoteTab.CommonNoteTabName;
            sqlParams[4].Value = inCommonNoteTab.UsingRole;
            sqlParams[5].Value = inCommonNoteTab.ShowType;
            sqlParams[6].Value = inCommonNoteTab.OrderCode;
            sqlParams[7].Value = inCommonNoteTab.CreateDoctorID;
            sqlParams[8].Value = inCommonNoteTab.CreateDoctorName;
            sqlParams[9].Value = inCommonNoteTab.CreateDateTime;
            sqlParams[10].Value = inCommonNoteTab.Valide;
            m_app.SqlHelper.ExecuteNoneQuery("EMR_CommonNote.usp_AddorModInCommonTab", sqlParams, CommandType.StoredProcedure);
        }

        /// <summary>
        /// 病人通用单据项目保存
        /// </summary>
        /// <param name="inCommonNoteItem"></param>
        public bool SaveIncommonNoteItem(InCommonNoteItemEntity inCommonNoteItem, ref string message)
        {
            try
            {
                if (inCommonNoteItem.InCommonNote_Item_Flow == null)
                    inCommonNoteItem.InCommonNote_Item_Flow = Guid.NewGuid().ToString();
                if (inCommonNoteItem.CreateDoctorID == null)
                    inCommonNoteItem.CreateDoctorID = m_app.User.DoctorId;
                if (inCommonNoteItem.CreateDoctorName == null)
                    inCommonNoteItem.CreateDoctorName = m_app.User.DoctorName;
                if (string.IsNullOrEmpty(inCommonNoteItem.Valide))
                    inCommonNoteItem.Valide = "1";

                SqlParameter[] sqlParams = new SqlParameter[]
           {
                 new SqlParameter("@InCommonNote_Item_Flow",SqlDbType.VarChar,50),
               new SqlParameter("@InCommonNote_Tab_Flow",SqlDbType.VarChar,50),
               new SqlParameter("@InCommonNoteFlow",SqlDbType.VarChar,50),
               new SqlParameter("@CommonNote_Item_Flow",SqlDbType.VarChar,50),
               new SqlParameter("@CommonNote_Tab_Flow",SqlDbType.VarChar,50),
               new SqlParameter("@CommonNoteFlow",SqlDbType.VarChar,50),
               new SqlParameter("@DataElementFlow",SqlDbType.VarChar,50),
               new SqlParameter("@DataElementId",SqlDbType.VarChar,50),
               new SqlParameter("@DataElementName",SqlDbType.VarChar,50),
               new SqlParameter("@OtherName",SqlDbType.VarChar,50),
               new SqlParameter("@OrderCode",SqlDbType.VarChar,50),
               new SqlParameter("@IsValidate",SqlDbType.VarChar,50),
               new SqlParameter("@CreateDoctorID",SqlDbType.VarChar,50),
               new SqlParameter("@CreateDoctorName",SqlDbType.VarChar,50),
               new SqlParameter("@CreateDateTime",SqlDbType.VarChar,50),
                   new SqlParameter("@Valide",SqlDbType.VarChar,50),
               new SqlParameter("@ValueXml",SqlDbType.VarChar,4000),
               new SqlParameter("@RecordDate",SqlDbType.VarChar,50),
               new SqlParameter("@RecordTime",SqlDbType.VarChar,50),
                new SqlParameter("@RecordDoctorId",SqlDbType.VarChar,50),
                 new SqlParameter("@RecordDoctorName",SqlDbType.VarChar,50),
                   new SqlParameter("@GroupFlow",SqlDbType.VarChar,50)
           };
                sqlParams[0].Value = inCommonNoteItem.InCommonNote_Item_Flow;
                sqlParams[1].Value = inCommonNoteItem.InCommonNote_Tab_Flow;
                sqlParams[2].Value = inCommonNoteItem.InCommonNoteFlow;
                sqlParams[3].Value = inCommonNoteItem.CommonNote_Item_Flow;
                sqlParams[4].Value = inCommonNoteItem.CommonNote_Tab_Flow;
                sqlParams[5].Value = inCommonNoteItem.CommonNoteFlow;
                sqlParams[6].Value = inCommonNoteItem.DataElementFlow;
                sqlParams[7].Value = inCommonNoteItem.DataElementId;
                sqlParams[8].Value = inCommonNoteItem.DataElementName;
                sqlParams[9].Value = inCommonNoteItem.OtherName;
                sqlParams[10].Value = inCommonNoteItem.OrderCode;
                sqlParams[11].Value = inCommonNoteItem.IsValidate;
                sqlParams[12].Value = inCommonNoteItem.CreateDoctorID;
                sqlParams[13].Value = inCommonNoteItem.CreateDoctorName;
                sqlParams[14].Value = inCommonNoteItem.CreateDateTime;
                sqlParams[15].Value = inCommonNoteItem.Valide;
                sqlParams[16].Value = inCommonNoteItem.ValueXml;
                sqlParams[17].Value = inCommonNoteItem.RecordDate;
                sqlParams[18].Value = inCommonNoteItem.RecordTime;
                sqlParams[19].Value = inCommonNoteItem.RecordDoctorId;
                sqlParams[20].Value = inCommonNoteItem.RecordDoctorName;
                sqlParams[21].Value = inCommonNoteItem.GroupFlow;
                m_app.SqlHelper.ExecuteNoneQuery("EMR_CommonNote.usp_AddorModInCommonitem", sqlParams, CommandType.StoredProcedure);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        public DataTable GetInpatient(string noofInpat)
        {
            SqlParameter[] sqlParams = new SqlParameter[]
           {
                 new SqlParameter("@noofInpat",SqlDbType.VarChar,50)
           };
            sqlParams[0].Value = noofInpat;
            DataTable inpatientDt = m_app.SqlHelper.ExecuteDataTable("EMR_CommonNote.usp_GetInpatient", sqlParams, CommandType.StoredProcedure);
            return inpatientDt;
        }

        #endregion

        /// <summary>
        /// 将CommonNoteEntity转换成InCommonNoteEnmtity
        /// 主要是数据转换 用于表格的建立
        /// </summary>
        /// <param name="commonNote"></param>
        /// <returns></returns>
        public static InCommonNoteEnmtity ConvertCommonToInCommon(CommonNoteEntity commonNote)
        {
            InCommonNoteEnmtity inCommonNoteEnmtity = new InCommonNoteEnmtity();
            inCommonNoteEnmtity.CommonNoteFlow = commonNote.CommonNoteFlow;
            inCommonNoteEnmtity.InCommonNoteName = commonNote.CommonNoteName;
            inCommonNoteEnmtity.PrinteModelName = commonNote.PrinteModelName;
            if (commonNote.CommonNote_TabList == null) return inCommonNoteEnmtity;
            List<InCommonNoteTabEntity> inCommonNoteTabList = new List<InCommonNoteTabEntity>();
            foreach (var itemTab in commonNote.CommonNote_TabList)
            {
                InCommonNoteTabEntity inCommonNoteTab = new InCommonNoteTabEntity();
                inCommonNoteTab.CommonNote_Tab_Flow = itemTab.CommonNote_Tab_Flow;
                inCommonNoteTab.CommonNoteTabName = itemTab.CommonNoteTabName;
                inCommonNoteTab.OrderCode = itemTab.OrderCode;
                inCommonNoteTab.ShowType = itemTab.ShowType;
                inCommonNoteTab.UsingRole = itemTab.UsingRole;
                inCommonNoteTabList.Add(inCommonNoteTab);
            }
            var ListOrder = inCommonNoteTabList.OrderBy(a => Convert.ToInt32(a.OrderCode));
            foreach (var item in ListOrder)
            {
                if (inCommonNoteEnmtity.InCommonNoteTabList == null)
                {
                    inCommonNoteEnmtity.InCommonNoteTabList = new List<InCommonNoteTabEntity>();
                }
                inCommonNoteEnmtity.InCommonNoteTabList.Add(item);
            }
            return inCommonNoteEnmtity;

        }

        /// <summary>
        /// 将数据项转换成病人数据项
        /// </summary>
        /// <param name="commonNote_TabEntity"></param>
        /// <returns></returns>
        public static List<InCommonNoteItemEntity> ConvertItem(CommonNote_TabEntity commonNote_TabEntity)
        {
            List<InCommonNoteItemEntity> inCommonNoteItemList = new List<InCommonNoteItemEntity>();
            List<CommonNote_ItemEntity> commonNote_ItemList = commonNote_TabEntity.CommonNote_ItemList;
            if (commonNote_ItemList == null) return inCommonNoteItemList;
            string groupflow = Guid.NewGuid().ToString();
            foreach (var item in commonNote_ItemList)
            {
                InCommonNoteItemEntity inCommonNoteItem = new InCommonNoteItemEntity();
                inCommonNoteItem.CommonNote_Item_Flow = item.CommonNote_Item_Flow;
                inCommonNoteItem.CommonNote_Tab_Flow = item.CommonNote_Tab_Flow;
                inCommonNoteItem.CommonNoteFlow = item.CommonNoteFlow;
                inCommonNoteItem.DataElement = item.DataElement;
                inCommonNoteItem.DataElementFlow = item.DataElementFlow;
                inCommonNoteItem.DataElementId = item.DataElementId;
                inCommonNoteItem.DataElementName = item.DataElementName;
                inCommonNoteItem.IsValidate = item.IsValidate;
                inCommonNoteItem.OrderCode = item.OrderCode;
                inCommonNoteItem.OtherName = item.OtherName;
                inCommonNoteItem.GroupFlow = groupflow;
                inCommonNoteItemList.Add(inCommonNoteItem);
            }
            return inCommonNoteItemList;
        }





        /// <summary>
        /// 将tab对象转换成可打印和显示的对象
        /// </summary>
        /// <param name="inCommonNoteTabEntity"></param>
        /// <returns></returns>
        public static List<string> ConvertInCommonTabToPrint(InCommonNoteTabEntity inCommonNoteTabEntity,
            PrintInCommonTabView printInCommonTabView,
            out Dictionary<string, List<InCommonNoteItemEntity>> dicitemList,
            CommonNote_TabEntity commonNoteTab,
            IEmrHost m_app, InCommonNoteEnmtity inCommonNoteEnmtity)
        {
            if (printInCommonTabView == null)
                printInCommonTabView = new PrintInCommonTabView();
            List<String> strList = new List<string>();
            if (inCommonNoteTabEntity.InCommonNoteItemList != null)
            {
                var list = from a in inCommonNoteTabEntity.InCommonNoteItemList
                           group a
                               by new { a.CommonNote_Item_Flow, a.OtherName }
                               into g
                               select g;
                // 获得此模型的公共属性 
                int i = 1;
                foreach (var item in list)
                {
                    if (i > 50) break;
                    strList.Add(item.Key.OtherName);
                    string proName = "Name";
                    proName = proName + i;
                    PropertyInfo property = printInCommonTabView.GetType().GetProperty(proName);
                    if (property != null)
                    {
                        property.SetValue(printInCommonTabView, item.Key.OtherName, null);
                        i++;
                    }
                }
                ConvertInCommonTabValueToPrint(inCommonNoteTabEntity, printInCommonTabView, out dicitemList, m_app, inCommonNoteEnmtity);
                if (printInCommonTabView != null && printInCommonTabView.PrintInCommonItemViewList != null)
                {
                    printInCommonTabView.PrintInCommonItemViewList = printInCommonTabView.PrintInCommonItemViewList.OrderBy(a => a.DateTimeShow).ToList();
                }
            }
            else
            {
                int i = 1;
                if (commonNoteTab != null && commonNoteTab.CommonNote_ItemList != null)
                {
                    foreach (var item in commonNoteTab.CommonNote_ItemList)
                    {
                        if (i > 50) break;
                        strList.Add(item.OtherName);
                        string proName = "Name";
                        proName = proName + i;
                        PropertyInfo property = printInCommonTabView.GetType().GetProperty(proName);
                        if (property != null)
                        {
                            property.SetValue(printInCommonTabView, item.OtherName, null);
                            i++;
                        }
                    }
                }
                dicitemList = new Dictionary<string, List<InCommonNoteItemEntity>>();
            }
            return strList;
        }

        /// <summary>
        /// 对值的转换 转换成可显示的
        /// </summary>
        /// <param name="inCommonNoteTabEntity">用于存储的对象</param>
        /// <param name="printInCommonTabView">用于打印和表格显示的对象</param>
        /// <param name="dicitemList">用于返回的实体类列表</param>
        private static void ConvertInCommonTabValueToPrint(InCommonNoteTabEntity inCommonNoteTabEntity,
            PrintInCommonTabView printInCommonTabView,
            out Dictionary<string, List<InCommonNoteItemEntity>> dicitemList,
            IEmrHost m_app,
            InCommonNoteEnmtity inCommonNoteEnmtity)
        {
            try
            {
                DataElemntBiz dataElemntBiz = new CommonTableConfig.DataElemntBiz(m_app);
                printInCommonTabView.PrintInCommonItemViewList = new List<PrintInCommonItemView>();
                inCommonNoteTabEntity.InCommonNoteItemList = inCommonNoteTabEntity.InCommonNoteItemList.OrderBy(a => Convert.ToInt32(a.OrderCode)).ToList();
                var inCommonNoteItemList = inCommonNoteTabEntity.InCommonNoteItemList;
                dicitemList = new Dictionary<string, List<InCommonNoteItemEntity>>();
                foreach (var item in inCommonNoteItemList)  //将同一组的数据放在一个里面
                {
                    if (!dicitemList.Keys.Contains(item.GroupFlow))
                    {
                        dicitemList.Add(item.GroupFlow, new List<InCommonNoteItemEntity>());
                    }
                    dicitemList[item.GroupFlow].Add(item);
                }
                Dictionary<string, string> userIdAndImgStr = new Dictionary<string, string>();

                CommonNoteBiz commonNoteBiz = new CommonTableConfig.CommonNoteBiz(m_app);
                CommonNoteEntity commonNote = commonNoteBiz.GetSimpleCommonNoteByflow(inCommonNoteEnmtity.CommonNoteFlow);
                string usingpicSign = "0"; //查找是否启用图片签名
                if (string.IsNullOrEmpty(commonNote.UsingPicSign) || commonNote.UsingPicSign != "1")
                {
                    usingpicSign = "0";
                }
                else
                {
                    usingpicSign = "1";
                }

                foreach (var item in dicitemList)
                {
                    List<InCommonNoteItemEntity> inCommonNoteItemListValue = item.Value as List<InCommonNoteItemEntity>;
                    PrintInCommonItemView printInCommonItemView = new PrintInCommonItemView();
                    printInCommonItemView.GroupFlow = item.Key;
                    for (int i = 1; i <= inCommonNoteItemListValue.Count; i++)
                    {
                        if (i > 50) break;
                        printInCommonItemView.Date = inCommonNoteItemListValue[0].RecordDate;
                        printInCommonItemView.Time = inCommonNoteItemListValue[0].RecordTime;
                        if (!string.IsNullOrEmpty(printInCommonItemView.Time)
                            && printInCommonItemView.Time.Length == 8)
                        {
                            //处理时间 为hh:MM
                            printInCommonItemView.Time = printInCommonItemView.Time.Substring(0, 5);
                        }
                        printInCommonItemView.RecordDoctorId = inCommonNoteItemListValue[0].RecordDoctorId;
                        printInCommonItemView.RecordDoctorName = inCommonNoteItemListValue[0].RecordDoctorName;
                        if (usingpicSign == "1")
                        {
                            SetDocImage(m_app, userIdAndImgStr, inCommonNoteItemListValue[0], printInCommonItemView);
                        }
                        string proName = "Value";
                        string proId = "ID";
                        string proLongVaue = "LongValue";

                        proId = proId + i;
                        proName = proName + i;
                        proLongVaue = proLongVaue + i;

                        PropertyInfo property = printInCommonItemView.GetType().GetProperty(proName);
                        PropertyInfo propertyID = printInCommonItemView.GetType().GetProperty(proId);
                        PropertyInfo propertyLongValue = printInCommonItemView.GetType().GetProperty(proLongVaue);
                        if (property != null)
                        {
                            string valueshow = inCommonNoteItemListValue[i - 1].ValuesShow;  //处理打印预览的数据

                            if (valueshow.Length > 1 && (valueshow.Substring(valueshow.Length - 1) == ";" || valueshow.Substring(valueshow.Length - 1) == "；")
                                && inCommonNoteItemListValue[i - 1].BaseValueList != null
                                && inCommonNoteItemListValue[i - 1].BaseValueList.Count > 0)
                            {
                                valueshow = valueshow.Substring(0, valueshow.Length - 1);   //如果最后一个是；把它截掉
                            }
                            property.SetValue(printInCommonItemView, valueshow, null);

                        }

                        if (propertyID != null)
                        {

                            string idstr = "";
                            foreach (var itembase in inCommonNoteItemListValue[i - 1].BaseValueList)
                            {
                                idstr += itembase.Id + ",";
                            }
                            if (idstr.Length > 0 && inCommonNoteItemListValue[i - 1].BaseValueList != null && inCommonNoteItemListValue[i - 1].BaseValueList.Count == 1)
                            {
                                idstr = idstr.Substring(0, idstr.Length - 1);
                            }

                            propertyID.SetValue(printInCommonItemView, idstr, null);

                        }

                        if (inCommonNoteTabEntity.ShowType == "单列" && propertyLongValue != null)
                        {
                            GetLongValue(dataElemntBiz, inCommonNoteItemListValue, printInCommonItemView, i, propertyLongValue);
                        }

                    }
                    printInCommonTabView.PrintInCommonItemViewList.Add(printInCommonItemView);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置签名图片
        /// </summary>
        /// <param name="m_app"></param>
        /// <param name="userIdAndImgStr"></param>
        /// <param name="inCommonNoteItem"></param>
        /// <param name="printInCommonItemView"></param>
        private static void SetDocImage(IEmrHost m_app,
            Dictionary<string, string> userIdAndImgStr,
            InCommonNoteItemEntity inCommonNoteItem,
            PrintInCommonItemView printInCommonItemView)
        {
            try
            {
                if (inCommonNoteItem == null || string.IsNullOrEmpty(inCommonNoteItem.RecordDoctorId)) return;
                string getImgStr = @"select userpic from userpicsign where userid=@userId and valide='1';";
                SqlParameter[] sqlParams = new SqlParameter[]
                        {
                            new SqlParameter("@userId",SqlDbType.VarChar,50)
                         };
                if (!userIdAndImgStr.ContainsKey(inCommonNoteItem.RecordDoctorId))
                {
                    sqlParams[0].Value = inCommonNoteItem.RecordDoctorId;
                    DataTable dtImage = m_app.SqlHelper.ExecuteDataTable(getImgStr, sqlParams, CommandType.Text);
                    if (dtImage == null || dtImage.Rows.Count <= 0) return;

                    string imgStr = dtImage.Rows[0]["USERPIC"].ToString();
                    byte[] bytes = Convert.FromBase64String(imgStr);
                    MemoryStream stream = new MemoryStream(bytes);
                    Image bgimage = Image.FromStream(stream);

                    if (!Directory.Exists(@"C:\Temp"))
                    {
                        Directory.CreateDirectory(@"C:\Temp");
                    }
                    string flieName = @"C:\Temp\" + inCommonNoteItem.RecordDoctorId + ".jpeg";
                    bgimage.Save(flieName);
                    userIdAndImgStr.Add(inCommonNoteItem.RecordDoctorId, flieName);
                    //bgimage.Dispose();
                    stream.Close();
                }
                if (userIdAndImgStr.ContainsKey(inCommonNoteItem.RecordDoctorId))
                {
                    printInCommonItemView.RecordDoctorImg = userIdAndImgStr[inCommonNoteItem.RecordDoctorId];
                    // ShowTest(printInCommonItemView.RecordDoctorImg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取单列中的带√字符串 如：视   √听
        /// </summary>
        /// <param name="dataElemntBiz"></param>
        /// <param name="inCommonNoteItemListValue"></param>
        /// <param name="printInCommonItemView"></param>
        /// <param name="i"></param>
        /// <param name="propertyLongValue"></param>
        private static void GetLongValue(DataElemntBiz dataElemntBiz, List<InCommonNoteItemEntity> inCommonNoteItemListValue, PrintInCommonItemView printInCommonItemView, int i, PropertyInfo propertyLongValue)
        {
            try
            {
                string longValueStr = "";
                if (inCommonNoteItemListValue[i - 1].DataElement == null)
                {
                    inCommonNoteItemListValue[i - 1].DataElement = dataElemntBiz.GetDataElement(inCommonNoteItemListValue[i - 1].DataElementFlow);
                }
                List<BaseDictory> BaseOptionList = inCommonNoteItemListValue[i - 1].DataElement.BaseOptionList;
                foreach (var itemoption in BaseOptionList)
                {
                    bool hasit = false;
                    foreach (var itemValue in inCommonNoteItemListValue[i - 1].BaseValueList)
                    {
                        if (itemValue.Name == itemoption.Name)
                        {
                            hasit = true;
                            break;
                        }
                    }
                    if (hasit)
                    {
                        longValueStr += "√" + itemoption + "   ";

                    }
                    else
                    {
                        longValueStr += itemoption + "   ";
                    }
                }
                if (longValueStr.Length >= 3)
                {
                    longValueStr = longValueStr.Substring(0, longValueStr.Length - 3);
                }
                propertyLongValue.SetValue(printInCommonItemView, longValueStr, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Dictionary<string, string> GetReportxmlCol(PrintInCommonView printInCommonView)
        {
            try
            {
                Dictionary<string, string> dicStr = new Dictionary<string, string>();
                string path = Application.StartupPath.ToString();
                path += @"\Report\Report.xml";
                if (File.Exists(path))
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(path);
                    XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/reports/report");
                    if (xmlNodeList == null || xmlNodeList.Count == 0) return null;
                    foreach (XmlNode item in xmlNodeList)
                    {
                        string reportName = item.Attributes["name"].Value;
                        if (reportName == printInCommonView.PrintFileName)
                        {
                            if (item.Attributes["maxCols"] == null) break;
                            string maxcolcount = item.Attributes["maxCols"].Value;
                            dicStr.Add("maxCols", maxcolcount);
                            break;
                        }
                    }
                }
                return dicStr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取列名和每行字的个数个数
        /// </summary>
        /// <param name="printInCommonView"></param>
        /// <returns></returns>
        public static Dictionary<string, ColmonXMLEntity> GetReportxmlwords(PrintInCommonView printInCommonView)
        {
            try
            {
                Dictionary<string, ColmonXMLEntity> dicStr = new Dictionary<string, ColmonXMLEntity>();
                string path = Application.StartupPath.ToString();
                path += @"\Report\Report.xml";
                if (File.Exists(path))
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(path);
                    XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/reports/report");
                    if (xmlNodeList == null || xmlNodeList.Count == 0) return null;
                    foreach (XmlNode item in xmlNodeList)
                    {
                        string reportName = item.Attributes["name"].Value;
                        if (reportName == printInCommonView.PrintFileName)
                        {
                            XmlNodeList xmlNodeListcols = item.SelectNodes("column");
                            if (xmlNodeListcols == null) return dicStr;
                            foreach (XmlNode itemcol in xmlNodeListcols)
                            {
                                if (itemcol.Attributes["name"] != null
                                    && itemcol.Attributes["maxpix"] != null
                                     && itemcol.Attributes["fontfamily"] != null
                                     && itemcol.Attributes["fontsize"] != null)
                                {
                                    ColmonXMLEntity colmonXMLEntity = new CommonNoteUse.ColmonXMLEntity();
                                    colmonXMLEntity.Name = itemcol.Attributes["name"].Value;
                                    colmonXMLEntity.Maxpix = itemcol.Attributes["maxpix"].Value;
                                    colmonXMLEntity.Fontfamily = itemcol.Attributes["fontfamily"].Value;
                                    colmonXMLEntity.Fontsize = itemcol.Attributes["fontsize"].Value;

                                    dicStr.Add(colmonXMLEntity.Name, colmonXMLEntity);
                                }
                            }
                        }
                    }
                }
                return dicStr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //xll 20130228 获取例如审阅者和签名在同一行的问题 
        public static List<string> GetReportxmlRowEnd(string fileName)
        {
            try
            {
                List<string> proNameList = new List<string>();
                Dictionary<string, ColmonXMLEntity> dicStr = new Dictionary<string, ColmonXMLEntity>();
                string path = Application.StartupPath.ToString();
                path += @"\Report\Report.xml";
                if (File.Exists(path))
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(path);
                    XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/reports/report");
                    if (xmlNodeList == null || xmlNodeList.Count == 0) return null;
                    foreach (XmlNode item in xmlNodeList)
                    {
                        string reportName = item.Attributes["name"].Value;
                        if (reportName == fileName)
                        {
                            XmlNodeList xmlNodeListcols = item.SelectNodes("rowend");
                            if (xmlNodeListcols == null) return null;
                            foreach (XmlNode itemcol in xmlNodeListcols)
                            {
                                if (itemcol.Attributes["name"] != null)
                                {
                                    proNameList.Add(itemcol.Attributes["name"].Value);
                                }
                            }
                        }
                    }
                }
                return proNameList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询一天某一单据的所有已填项目
        /// </summary>
        /// <param name="commNoteFlow"></param>
        /// <param name="DayStr"></param>
        /// <returns></returns>
        public List<InCommonNoteItemEntity> GetIncommNoteItemByDay(string commNoteFlow, string DayStr)
        {
            try
            {
                List<InCommonNoteItemEntity> InCommonNoteItemlist = new List<InCommonNoteItemEntity>();
                if (string.IsNullOrEmpty(commNoteFlow) || string.IsNullOrEmpty(DayStr)) return InCommonNoteItemlist;
                SqlParameter[] sqlParams = new SqlParameter[]
           {
               new SqlParameter("@commonNoteFlow",SqlDbType.VarChar,50),
               new SqlParameter("@date",SqlDbType.VarChar,50),
               new SqlParameter("@dept",SqlDbType.VarChar,50),
               new SqlParameter("@ward",SqlDbType.VarChar,50)

           };
                sqlParams[0].Value = commNoteFlow;
                sqlParams[1].Value = DayStr;
                sqlParams[2].Value = m_app.User.CurrentDeptId;
                sqlParams[3].Value = m_app.User.CurrentWardId;
                DataTable dataTable = m_app.SqlHelper.ExecuteDataTable("EMR_CommonNote.usp_GetInCommomItemDay", sqlParams, CommandType.StoredProcedure);
                InCommonNoteItemlist = DataTableToList<InCommonNoteItemEntity>.ConvertToModel(dataTable);
                return InCommonNoteItemlist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 将数据库字段转换成DateTable但未对DateTable赋值
        /// </summary>
        /// <returns></returns>
        public DataTable GetDateTable(CommonNote_TabEntity commonNote_TabEntity, out Dictionary<string, DataElementEntity> dataElementList)
        {
            try
            {
                dataElementList = new Dictionary<string, DataElementEntity>();
                DataTable dt = new DataTable();
                DataColumn dc = null;

                dc = new DataColumn("groupFlow");
                dc.Caption = "流水号";
                dc.DataType = typeof(String);
                dt.Columns.Add(dc);

                dc = new DataColumn("NAME");
                dc.Caption = "姓名";
                dc.DataType = typeof(String);
                dt.Columns.Add(dc);

                dc = new DataColumn("PATID");
                dc.Caption = "住院号";
                dc.DataType = typeof(String);
                dt.Columns.Add(dc);

                dc = new DataColumn("OUTBED");
                dc.Caption = "床号";
                dc.DataType = typeof(String);
                dt.Columns.Add(dc);

                dc = new DataColumn("jlsj");
                dc.Caption = "记录时间";
                dc.DataType = typeof(DateTime);
                dt.Columns.Add(dc);
                foreach (var item in commonNote_TabEntity.CommonNote_ItemList)
                {
                    dc = new DataColumn(item.CommonNote_Item_Flow);
                    if (string.IsNullOrEmpty(item.OtherName))
                    {
                        dc.Caption = "未指定列";
                    }
                    else
                    {
                        dc.Caption = item.OtherName;
                    }

                    if (item.DataElement == null)
                    {
                        item.DataElement = m_DataElemntBiz.GetDataElement(item.DataElementFlow);
                    }
                    dataElementList.Add(item.CommonNote_Item_Flow, item.DataElement);
                    Type type = GetDatetype(item.DataElement.ElementType);
                    if (type == null) continue;
                    dc.DataType = type;
                    dt.Columns.Add(dc);
                }

                dc = new DataColumn("jlr");
                dc.Caption = "记录人";
                dc.DataType = typeof(string);
                dt.Columns.Add(dc);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 通过数据元素的类型来得到真实的数据类型
        /// </summary>
        /// <param name="elementType"></param>
        /// <returns></returns>
        private Type GetDatetype(string elementType)
        {
            try
            {
                if (string.IsNullOrEmpty(elementType)) return null;
                Type type = null;
                switch (elementType.ToUpper())
                {
                    case "S1":
                        type = typeof(String);
                        break;
                    case "S2":
                        type = typeof(String);
                        break;
                    case "S3":
                        type = typeof(String);
                        break;
                    case "S4":
                        type = typeof(String);
                        break;
                    case "S9":
                        type = typeof(String);
                        break;
                    case "DT":
                        type = typeof(DateTime);
                        break;
                    case "D":
                        type = typeof(DateTime);
                        break;
                    case "T":
                        type = typeof(DateTime);
                        break;
                    case "N":
                        type = typeof(Decimal);
                        break;
                    case "L":
                        type = typeof(bool);
                        break;
                    default:
                        type = typeof(String);
                        break;
                }
                return type;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 将值插入到dataTable中
        /// </summary>
        /// <param name="dt"></param>
        public void SetValueToDataTable(DataTable dt,
            out Dictionary<string, List<InCommonNoteItemEntity>> dicitemList,
            List<InCommonNoteItemEntity> inCommonNoteItemEntityList,
            Dictionary<string, DataElementEntity> dataElementList)
        {
            try
            {
                if (dt == null || dt.Columns == null || dt.Columns.Count <= 0)
                {
                    dicitemList = new Dictionary<string, List<InCommonNoteItemEntity>>();
                    return;
                }
                dicitemList = new Dictionary<string, List<InCommonNoteItemEntity>>();
                foreach (var item in inCommonNoteItemEntityList)  //将同一组的数据放在一个里面
                {
                    if (!dicitemList.Keys.Contains(item.GroupFlow))
                    {
                        dicitemList.Add(item.GroupFlow, new List<InCommonNoteItemEntity>());
                    }
                    dicitemList[item.GroupFlow].Add(item);
                }

                foreach (List<InCommonNoteItemEntity> itemList in dicitemList.Values)
                {
                    DataRow dtRow = dt.NewRow();
                    string dateTime = itemList[0].RecordDate + " " + itemList[0].RecordTime;
                    dtRow["jlsj"] = Convert.ToDateTime(dateTime);
                    dtRow["groupFlow"] = itemList[0].GroupFlow;
                    dtRow["jlr"] = itemList[0].RecordDoctorName;
                    DataRow dr = GetInpatientSim(itemList[0].InCommonNote_Item_Flow);
                    dtRow["NAME"] = dr["NAME"].ToString();            //姓名
                    dtRow["PATID"] = dr["PATID"].ToString();            //住院号
                    dtRow["OUTBED"] = dr["OUTBED"].ToString();           //床号
                    for (int i = 0; i < itemList.Count; i++)
                    {
                        if (dtRow[itemList[i].CommonNote_Item_Flow] == null) continue;
                        itemList[i].DataElement = dataElementList[itemList[i].CommonNote_Item_Flow];
                        SetDataTableValue(itemList[i]);
                        if (itemList[i].DataTableValue != null && itemList[i].DataTableValue.ToString() != "")
                        {
                            dtRow[itemList[i].CommonNote_Item_Flow] = itemList[i].DataTableValue;
                        }
                    }
                    dt.Rows.Add(dtRow);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataRow GetInpatientSim(string incommonItem)
        {
            try
            {
                SqlParameter[] sqlpars = new SqlParameter[]{
            new SqlParameter("@incommonnoteitemflow",SqlDbType.VarChar,50),
            };
                sqlpars[0].Value = incommonItem;
                DataTable dt = m_app.SqlHelper.ExecuteDataTable("EMR_CommonNote.usp_GetInpatientSim", sqlpars, CommandType.StoredProcedure);
                if (dt == null || dt.Rows == null || dt.Rows.Count <= 0)
                {
                    throw new Exception("未找到对应病人");
                }
                else
                {
                    return dt.Rows[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 通过对象设置该对象的DataTableValue的值 用于显示在datatable中
        /// </summary>
        /// <param name="inCommonNoteItemEntity"></param>
        private void SetDataTableValue(InCommonNoteItemEntity inCommonNoteItemEntity)
        {
            try
            {
                string elementType = inCommonNoteItemEntity.DataElement.ElementType;
                inCommonNoteItemEntity.DataTableValue = "";
                if (inCommonNoteItemEntity.IsValidate == "否")
                {
                    if (inCommonNoteItemEntity.BaseValueList != null && inCommonNoteItemEntity.BaseValueList.Count == 1)
                    {
                        inCommonNoteItemEntity.DataTableValue = inCommonNoteItemEntity.BaseValueList[0].Name;
                    }
                    return;
                }
                #region 对不同类型的值进行赋值
                if (elementType.ToUpper() == "S2"
                    || elementType.ToUpper() == "S3"
                     || elementType.ToUpper() == "S1"
                     || elementType.ToUpper() == "S4")
                {
                    if (inCommonNoteItemEntity.BaseValueList != null && inCommonNoteItemEntity.BaseValueList.Count == 1)
                    {
                        inCommonNoteItemEntity.DataTableValue = inCommonNoteItemEntity.BaseValueList[0].Name;
                    }
                }
                else if (elementType.ToUpper() == "S9")
                {
                    if (inCommonNoteItemEntity.BaseValueList != null && inCommonNoteItemEntity.BaseValueList.Count > 0)
                    {

                        foreach (var item in inCommonNoteItemEntity.BaseValueList)
                        {
                            inCommonNoteItemEntity.DataTableValue += item.Id + ",";
                        }
                        if (inCommonNoteItemEntity.DataTableValue.ToString().Length > 0)
                        {
                            inCommonNoteItemEntity.DataTableValue = inCommonNoteItemEntity.DataTableValue.ToString().Substring(0, inCommonNoteItemEntity.DataTableValue.ToString().Length - 1);
                        }
                    }
                }
                else if (elementType.ToUpper() == "DT" || elementType.ToUpper() == "D" || elementType.ToUpper() == "T")
                {
                    string datatimestr = "";
                    if (inCommonNoteItemEntity.BaseValueList != null && inCommonNoteItemEntity.BaseValueList.Count == 1)
                    {
                        datatimestr = inCommonNoteItemEntity.BaseValueList[0].Name;
                    }
                    DateTime dt;
                    bool isdt = DateTime.TryParse(datatimestr, out dt);
                    if (isdt)
                    {
                        inCommonNoteItemEntity.DataTableValue = dt;
                    }
                    else
                    {
                        inCommonNoteItemEntity.DataTableValue = DateTime.Now;
                    }
                }
                else if (elementType.ToUpper() == "N")
                {
                    string decStr = "";
                    if (inCommonNoteItemEntity.BaseValueList != null && inCommonNoteItemEntity.BaseValueList.Count == 1)
                    {
                        decStr = inCommonNoteItemEntity.BaseValueList[0].Name;
                    }
                    Decimal dec;
                    bool isdec = Decimal.TryParse(decStr, out dec);
                    if (isdec)
                    {
                        inCommonNoteItemEntity.DataTableValue = dec;
                    }
                    else
                    {
                        //inCommonNoteItemEntity.DataTableValue = 0;
                    }
                }
                else if (elementType.ToUpper() == "L")
                {
                    string boolStr = "";
                    if (inCommonNoteItemEntity.BaseValueList != null && inCommonNoteItemEntity.BaseValueList.Count == 1)
                    {
                        boolStr = inCommonNoteItemEntity.BaseValueList[0].Name;
                    }
                    inCommonNoteItemEntity.DataTableValue = boolStr == "是" ? true : false;
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 获取当前科室的所有病人
        /// </summary>
        /// <returns></returns>
        public DataTable GetInPatientByDepart()
        {
            try
            {
                string departcode = m_app.User.CurrentDeptId;
                string wardcode = m_app.User.CurrentWardId;
                string sql = @"select i.noofinpat,i.name,i.patid,i.outbed from inpatient i where i.outhosdept=@departCode and i.outhosward=@wardcode and status in ('1501') order by outbed;";
                SqlParameter[] sqlParams = new SqlParameter[]
           {
                new SqlParameter("@departCode",SqlDbType.VarChar,50),
                new SqlParameter("@wardcode",SqlDbType.VarChar,50)
           };
                sqlParams[0].Value = departcode;
                sqlParams[1].Value = wardcode;
                DataTable dtInpatient = m_app.SqlHelper.ExecuteDataTable(sql, sqlParams, CommandType.Text);
                return dtInpatient;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        ///  批量设置
        /// </summary>
        /// <param name="dtinpat"></param>
        /// <param name="dtAddPL"></param>
        /// <param name="commonNote"></param>
        /// <param name="dateTimeAdd"></param>
        /// <param name="dataElementList"></param>
        /// <param name="dicitemList"></param>
        public void GetDataTablePLSetValue(DataTable dtinpat,
            DataTable dtAddPL,
            CommonNoteEntity commonNote,
            DateTime dateTimeAdd,
            Dictionary<string, DataElementEntity> dataElementList,
            Dictionary<string, List<InCommonNoteItemEntity>> dicitemList)
        {
            try
            {
                if (dtinpat == null || dtinpat.Rows.Count <= 0) return;
                if (commonNote == null || commonNote.CommonNote_TabList == null || commonNote.CommonNote_TabList[0].ShowType == "单列") return;
                DataTable dtPiLiang = new DataTable();
                for (int i = 0; i < dtinpat.Rows.Count; i++)  //根据病人生成批量录入数据
                {
                    DataRow drInpatient = dtinpat.Rows[i];
                    string noofinpat = drInpatient["NOOFINPAT"].ToString();

                    SqlParameter[] sqlParams = new SqlParameter[]
                    {
                        new SqlParameter("@noofinpat",SqlDbType.VarChar,50),
                        new SqlParameter("@commonnoteflow",SqlDbType.VarChar,50)
                    };
                    sqlParams[0].Value = noofinpat;
                    sqlParams[1].Value = commonNote.CommonNoteFlow;
                    DataTable dtCommonPer = m_app.SqlHelper.ExecuteDataTable("EMR_CommonNote.usp_GetIncommonNew", sqlParams, CommandType.StoredProcedure);
                    DataRow dradd = dtAddPL.NewRow();
                    InCommonNoteEnmtity inCommonNoteEnmtity;

                    //对于存在的单据 通过单据最大的行数来获取 如果大于等于最大行数了 则也要新增单据
                    int realCount = 0;
                    bool IsNeedAddCommon = GetCommonNoteForNeed(dtCommonPer, commonNote, ref realCount);

                    if (IsNeedAddCommon) //当前病人没有查找的单据时 或者单据已经被填满
                    {
                        //插入单据再处理
                        ConverBycommonNote(commonNote, noofinpat);
                        dtCommonPer = m_app.SqlHelper.ExecuteDataTable("EMR_CommonNote.usp_GetIncommonNew", sqlParams, CommandType.StoredProcedure);
                        realCount = 0;
                    }
                    if (dtCommonPer == null || dtCommonPer.Rows.Count <= 0) //插入后还是没有就是出错了
                    {
                        return;
                    }

                    inCommonNoteEnmtity = DataTableToList<InCommonNoteEnmtity>.ConvertToModelOne(dtCommonPer);
                    //GetDetaliInCommonNote(ref inCommonNoteEnmtity);
                    List<InCommonNoteTabEntity> inCommonNoteTabList = GetIncomonTab(inCommonNoteEnmtity.InCommonNoteFlow);

                    //int maxRow = GetInpatMaxRows(commonNote, inCommonNoteEnmtity, dataElementList);

                    List<InCommonNoteItemEntity> InCommonNoteItemList = AddCommonItemToInCommonItem(commonNote.CommonNote_TabList[0],
                        inCommonNoteTabList[0],
                         dateTimeAdd, dataElementList, realCount);   //生成的添加的数据

                    dicitemList.Add(InCommonNoteItemList[0].GroupFlow, InCommonNoteItemList);

                    dradd["jlsj"] = dateTimeAdd;
                    dradd["check"] = true;
                    dradd["groupFlow"] = InCommonNoteItemList[0].GroupFlow;
                    dradd["jlr"] = InCommonNoteItemList[0].RecordDoctorName;
                    dradd["NAME"] = drInpatient["NAME"].ToString();            //姓名
                    dradd["PATID"] = drInpatient["PATID"].ToString();            //住院号
                    dradd["OUTBED"] = drInpatient["OUTBED"].ToString();
                    for (int j = 0; j < InCommonNoteItemList.Count; j++)
                    {
                        if (dradd[InCommonNoteItemList[j].CommonNote_Item_Flow] == null) continue;
                        InCommonNoteItemList[j].DataElement = dataElementList[InCommonNoteItemList[j].CommonNote_Item_Flow];
                        SetDataTableValue(InCommonNoteItemList[j]);
                        if (InCommonNoteItemList[j].DataTableValue != null && InCommonNoteItemList[j].DataTableValue.ToString() != "")
                            dradd[InCommonNoteItemList[j].CommonNote_Item_Flow] = InCommonNoteItemList[j].DataTableValue;
                    }
                    dtAddPL.Rows.Add(dradd);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 返回使用单据的tab集合
        /// xlb 2013-02-04
        /// </summary>
        /// <param name="inCommonNoteFlow"></param>
        /// <returns></returns>
        private List<InCommonNoteTabEntity> GetIncomonTab(string inCommonNoteFlow)
        {
            try
            {
                string sqlITab = @"  select * from incommonnote_tab icomtab
                                   where icomtab.incommonnoteflow = @inCommonNoteFlow
                                   and icomtab.valide = '1'
                                   order by icomtab.ordercode";
                SqlParameter[] sps = { new SqlParameter("@inCommonNoteFlow", inCommonNoteFlow) };
                DataTable dtInComonTab = m_app.SqlHelper.ExecuteDataTable(sqlITab, sps, CommandType.Text);
                List<InCommonNoteTabEntity> inCommonNoteTab = DataTableToList<InCommonNoteTabEntity>.ConvertToModel(dtInComonTab);
                return inCommonNoteTab;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 获取批量录入的次数
        /// </summary>
        /// <returns></returns>
        private int getRealCount(string inComonNoteFlow, string commonNoteItemFlow)
        {
            try
            {
                string sqlRealCount = @"select count(*) from incommonnote_item_view 
                                     where incommonnoteflow=@inComonNoteFlow 
                                     and commonnote_item_flow=@commonNoteItemFlow
                                     and valide='1'";
                SqlParameter[] sps ={new SqlParameter("@inComonNoteFlow",inComonNoteFlow),
                                   new SqlParameter("@commonNoteItemFlow",commonNoteItemFlow)
                                   };
                DataTable realCount = m_app.SqlHelper.ExecuteDataTable(sqlRealCount, sps, CommandType.Text);
                if (realCount == null || realCount.Rows.Count <= 0)
                {
                    return 0;
                }
                return Convert.ToInt32(realCount.Rows[0][0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// 判断是够需要添加新单据  如果没有配置列表的行数 新单据 直接加数据
        /// 如果最新单据中行数大于等于最大值 新增单据
        /// edit by xlb 2013-02-04
        /// </summary>
        /// <param name="dtCommonPer"></param>
        /// <param name="commonNote"></param>
        /// <returns></returns>
        private bool GetCommonNoteForNeed(DataTable dtCommonPer, CommonNoteEntity commonNote)
        {
            try
            {

                if (dtCommonPer == null || dtCommonPer.Rows == null || dtCommonPer.Rows.Count <= 0) return true;
                int maxrows = 0;
                int.TryParse(commonNote.CommonNote_TabList[0].MaxRows, out maxrows);
                //如果未设置对大行数，就不需要新建单据了。
                if (maxrows <= 0) return false;
                InCommonNoteEnmtity inCommonNoteEnmtity = DataTableToList<InCommonNoteEnmtity>.ConvertToModelOne(dtCommonPer);
                //add by xlb 2013-02-04
                List<InCommonNoteItemEntity> inCommonItem = GetInComonItemByInCommonNote(inCommonNoteEnmtity.InCommonNoteFlow);
                if (inCommonItem == null || inCommonItem.Count <= 0)
                {
                    return false;
                }
                //按分组号进行统计得出实际条数
                int realCount = inCommonItem.GroupBy(a => a.GroupFlow).Count();
                if (realCount < maxrows)
                {
                    return false;
                }
                else
                {
                    return true;
                }
                #region --------已注销 by xlb 2013.02.04--------------
                //GetDetaliInCommonNote(ref inCommonNoteEnmtity);

                //List<InCommonNoteItemEntity> inCommonNoteItemList = inCommonNoteEnmtity.InCommonNoteTabList[0].InCommonNoteItemList;
                //if (inCommonNoteItemList == null || inCommonNoteItemList.Count <= 0)
                //{
                //    return false;
                //}
                //int realityCount = inCommonNoteItemList.GroupBy(a => a.GroupFlow).Count();   //单据真实的行数
                //if (realityCount < maxrows)
                //{
                //    return false;
                //}
                //else
                //{
                //    return true;
                //}
                #endregion
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 判断是够需要添加新单据  如果没有配置列表的行数 新单据 直接加数据
        /// xlb 2013-02-05
        /// </summary>
        /// <param name="dtCommonPer"></param>
        /// <param name="commonNote"></param>
        /// <param name="realCount"></param>
        /// <returns></returns>
        private bool GetCommonNoteForNeed(DataTable dtCommonPer, CommonNoteEntity commonNote, ref int realCount)
        {
            try
            {

                if (dtCommonPer == null || dtCommonPer.Rows == null || dtCommonPer.Rows.Count <= 0) return true;

                //xll 2013-03-04 1没有数据 加
                InCommonNoteEnmtity inCommonNoteEnmtity = DataTableToList<InCommonNoteEnmtity>.ConvertToModelOne(dtCommonPer);
                //add by xlb 2013-02-04
                //xll 注释 如果是空单子 要可以加数据
                List<InCommonNoteItemEntity> inCommonItem = GetInComonItemByInCommonNote(inCommonNoteEnmtity.InCommonNoteFlow);
                if (inCommonItem == null || inCommonItem.Count <= 0)
                {
                    return false;
                }
                //xll 2013-03-04 2有数据但是没有最大值 加
                int maxrows = 0;
                int.TryParse(commonNote.CommonNote_TabList[0].MaxRows, out maxrows);
                //如果未设置对大行数，就不需要新建单据了。
                if (maxrows <= 0) return false;

                ////xll 2013-03-04 3 有最大值 要和实际值判断
                //按分组号进行统计得出实际条数
                realCount = inCommonItem.GroupBy(a => a.GroupFlow).Count();
                if (realCount < maxrows)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取使用单据的item项
        /// xlb 2013-02-04
        /// </summary>
        /// <param name="incommonNoteFlow"></param>
        private List<InCommonNoteItemEntity> GetInComonItemByInCommonNote(string incommonNoteFlow)
        {
            try
            {
                //通过使用单唯一号获取使用项的sql
                string sqlInCommmonNoteFlow = @"select * from incommonnote_item_view icomitem
                                                where icomitem.incommonnoteflow = @incommonNoteFlow
                                                  and icomitem.valide = '1' 
                                                 order by icomitem.ordercode,
                                                          icomitem.recorddate,
                                                          icomitem.recordtime";
                SqlParameter[] sps = { new SqlParameter("@incommonNoteFlow", incommonNoteFlow) };
                DataTable dtIncommonItem = m_app.SqlHelper.ExecuteDataTable(sqlInCommmonNoteFlow, sps, CommandType.Text);
                //datatable集合->List集合
                List<InCommonNoteItemEntity> inCommonItemList = DataTableToList<InCommonNoteItemEntity>.ConvertToModel(dtIncommonItem);
                return inCommonItemList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 获取单据第一个表格数据库中存在的条数
        /// 只有存在setpValue值时才去查
        /// </summary>
        /// <param name="inCommonNoteEnmtity"></param>
        /// <returns></returns>
        private int GetInpatMaxRows(CommonNoteEntity commonNoteEnmtity, InCommonNoteEnmtity inCommonNoteEnmtity, Dictionary<string, DataElementEntity> dataElementList)
        {
            try
            {
                if (inCommonNoteEnmtity == null || inCommonNoteEnmtity.InCommonNoteTabList[0] == null) return 0;
                foreach (var item in commonNoteEnmtity.CommonNote_TabList[0].CommonNote_ItemList)
                {
                    if (dataElementList[item.CommonNote_Item_Flow].ElementType == "N")
                    {
                        Dictionary<string, string> dicList = DataElementEntity.GetMaxMinDefStr(dataElementList[item.CommonNote_Item_Flow].ElementRange);
                        if (!dicList.ContainsKey("StepValue")) continue;
                        string sqlCount = "select count(*) from incommonnote_item_view i where i.commonnote_item_flow=@commonnote_item_flow and i.incommonnote_tab_flow=@incommonnote_tab_flow and i.valide='1'; ";
                        SqlParameter[] sqlps = new SqlParameter[] {
                           new SqlParameter("@commonnote_item_flow",SqlDbType.VarChar),
                            new SqlParameter("@incommonnote_tab_flow",SqlDbType.VarChar)
                       };
                        sqlps[0].Value = item.CommonNote_Item_Flow;
                        sqlps[1].Value = inCommonNoteEnmtity.InCommonNoteTabList[0].InCommonNote_Tab_Flow;
                        DataTable dt = m_app.SqlHelper.ExecuteDataTable(sqlCount, sqlps, CommandType.Text);
                        if (dt == null || dt.Rows == null) return 0;
                        return Convert.ToInt32(dt.Rows[0][0]);
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 将commonNoteEntity转成InCommonNoteEnmtity，并保存
        /// </summary>
        /// <param name="commonNoteEntity"></param>
        /// <returns></returns>
        private InCommonNoteEnmtity ConverBycommonNote(CommonNoteEntity commonNoteEntity, string noofInpat)
        {
            try
            {
                CommonNoteBiz commonNoteBiz = new DrectSoft.Core.CommonTableConfig.CommonNoteBiz(m_app);
                commonNoteEntity = commonNoteBiz.GetDetailCommonNote(commonNoteEntity.CommonNoteFlow);
                InCommonNoteEnmtity inCommonNote = InCommonNoteBiz.ConvertCommonToInCommon(commonNoteEntity);
                InCommonNoteBiz icombiz = new DrectSoft.Core.CommonTableConfig.CommonNoteUse.InCommonNoteBiz(m_app);
                DataTable inpatientDt = icombiz.GetInpatient(noofInpat);
                inCommonNote.CurrDepartID = inpatientDt.Rows[0]["OUTHOSDEPT"].ToString();
                inCommonNote.CurrDepartName = inpatientDt.Rows[0]["DEPARTNAME"].ToString();
                inCommonNote.CurrWardID = inpatientDt.Rows[0]["OUTHOSWARD"].ToString();
                inCommonNote.CurrWardName = inpatientDt.Rows[0]["WARDNAME"].ToString();
                inCommonNote.NoofInpatient = noofInpat;
                inCommonNote.InPatientName = inpatientDt.Rows[0]["NAME"].ToString();
                string message = "";
                bool saveResult = icombiz.SaveInCommomNoteAll(inCommonNote, ref message);
                if (saveResult)
                {
                    return inCommonNote;
                }
                else
                {
                    throw new Exception("创建单据失败");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 将配置的项目转换成病人的项目 适用于改tab下没有病人数据项时
        /// </summary>
        private List<InCommonNoteItemEntity> AddCommonItemToInCommonItem(
            CommonNote_TabEntity commonNoteTab,
            InCommonNoteTabEntity inCommonNoteTab,
            DateTime dateTimeAdd,
            Dictionary<string, DataElementEntity> dataElementList,
            int rowcount)
        {
            if (commonNoteTab.CommonNote_ItemList == null) return null;
            List<InCommonNoteItemEntity> inCommonNoteItemList = new List<InCommonNoteItemEntity>();
            string gruopflow = Guid.NewGuid().ToString();
            foreach (var commonitem in commonNoteTab.CommonNote_ItemList)
            {
                InCommonNoteItemEntity inCommonNoteItem = new InCommonNoteItemEntity();
                inCommonNoteItem.CommonNote_Item_Flow = commonitem.CommonNote_Item_Flow;
                inCommonNoteItem.CommonNote_Tab_Flow = commonitem.CommonNote_Tab_Flow;
                inCommonNoteItem.CommonNoteFlow = commonitem.CommonNoteFlow;
                inCommonNoteItem.DataElementFlow = commonitem.DataElementFlow;
                inCommonNoteItem.DataElementId = commonitem.DataElementId;
                inCommonNoteItem.DataElementName = commonitem.DataElementName;
                inCommonNoteItem.DataElement = commonitem.DataElement;
                inCommonNoteItem.IsValidate = commonitem.IsValidate;
                inCommonNoteItem.OrderCode = commonitem.OrderCode;
                inCommonNoteItem.OtherName = commonitem.OtherName;
                inCommonNoteItem.GroupFlow = gruopflow;
                inCommonNoteItem.InCommonNote_Tab_Flow = inCommonNoteTab.InCommonNote_Tab_Flow;
                inCommonNoteItem.InCommonNoteFlow = inCommonNoteTab.InCommonNoteFlow;
                inCommonNoteItem.RecordDate = dateTimeAdd.ToString("yyyy-MM-dd");
                inCommonNoteItem.RecordTime = dateTimeAdd.ToString("HH:mm:ss");
                inCommonNoteItem.RecordDoctorName = m_app.User.DoctorName;
                inCommonNoteItem.RecordDoctorId = m_app.User.DoctorId;
                inCommonNoteItem.Valide = "1";
                SetDefalutValue(inCommonNoteItem, dataElementList, rowcount);
                inCommonNoteItemList.Add(inCommonNoteItem);
            }
            return inCommonNoteItemList;
        }

        /// <summary>
        /// 设置新增项目的默认值
        /// </summary>
        /// <param name="inCommonNoteItem"></param>
        private void SetDefalutValue(InCommonNoteItemEntity inCommonNoteItem,
            Dictionary<string, DataElementEntity> dataElementList,
            int StepCount)
        {
            try
            {
                if (inCommonNoteItem.DataElement == null)
                {
                    inCommonNoteItem.DataElement = dataElementList[inCommonNoteItem.CommonNote_Item_Flow];
                }
                if (inCommonNoteItem.DataElement.ElementType == "S2"
                    || inCommonNoteItem.DataElement.ElementType == "S3"
                    || inCommonNoteItem.DataElement.ElementType == "S9")
                {
                    List<BaseDictory> baseList = DataElementEntity.GetDefaultOption(inCommonNoteItem.DataElement.ElementRange);
                    inCommonNoteItem.ValueXml = InCommonNoteItemEntity.ConvertBaseToXml(baseList);
                }
                else if (inCommonNoteItem.DataElement.ElementType == "N")
                {
                    Dictionary<string, string> dicStr = DataElementEntity.GetMaxMinDefStr(inCommonNoteItem.DataElement.ElementRange);
                    if (dicStr == null) return;
                    if (dicStr.ContainsKey("DefaultValue"))
                    {
                        inCommonNoteItem.ValueXml = InCommonNoteItemEntity.ConvertStrToXml(dicStr["DefaultValue"]);
                    }
                    else if (dicStr.ContainsKey("MinValue"))
                    {
                        inCommonNoteItem.ValueXml = InCommonNoteItemEntity.ConvertStrToXml(dicStr["MinValue"]);
                    }
                    else
                    {
                        inCommonNoteItem.ValueXml = InCommonNoteItemEntity.ConvertStrToXml("0");
                    }
                    if (dicStr.ContainsKey("StepValue"))
                    {
                        int stepint;
                        int defint;
                        bool isint = int.TryParse(dicStr["StepValue"], out stepint);
                        bool isintdef = int.TryParse(dicStr["DefaultValue"], out defint);
                        if (!isint || !isintdef) return;
                        int rows = StepCount;
                        string valueNow = (defint + stepint * rows).ToString();
                        inCommonNoteItem.ValueXml = InCommonNoteItemEntity.ConvertStrToXml(valueNow);

                    }
                }
                else
                {
                    Dictionary<string, string> dicStr = DataElementEntity.GetMaxMinDefStr(inCommonNoteItem.DataElement.ElementRange);
                    if (dicStr == null) return;
                    if (dicStr.ContainsKey("DefaultValue"))
                    {
                        inCommonNoteItem.ValueXml = InCommonNoteItemEntity.ConvertStrToXml(dicStr["DefaultValue"]);
                    }
                    if (dicStr.ContainsKey("StepValue"))
                    {
                        int stepint;
                        int defint;
                        bool isint = int.TryParse(dicStr["StepValue"], out stepint);
                        bool isintdef = int.TryParse(dicStr["DefaultValue"], out defint);
                        if (!isint || !isintdef) return;
                        int rows = StepCount;
                        string valueNow = (defint + stepint * rows).ToString();
                        inCommonNoteItem.ValueXml = InCommonNoteItemEntity.ConvertStrToXml(valueNow);

                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// 获取是否启用签名表示
        /// </summary>
        /// <param name="commNoteFlow"></param>
        /// <returns></returns>
        public bool CanUsingSign(string commNoteFlow)
        {
            try
            {
                if (string.IsNullOrEmpty(commNoteFlow))
                {
                    return false;
                }
                CommonNoteBiz commonNoteBiz = new CommonNoteBiz(m_app);
                CommonNoteEntity commonNote = commonNoteBiz.GetSimpleCommonNoteByflow(commNoteFlow);
                if (commonNote == null)
                {
                    return false;
                }
                if (commonNote.UsingPicSign == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 对数据中相同时间的数据 取消时间 只处理了DateTimeShow字段
        /// 同时处理记录者字段
        /// xll 2013-02-28 添加审阅者和签名者在同一行
        /// </summary>
        /// <param name="printInCommonView"></param>
        public static void ConvertForDateTime(List<PrintInCommonItemView> printInCommonItemViewList)
        {
            try
            {
                if (printInCommonItemViewList == null || printInCommonItemViewList.Count == 0) return;
                for (int i = 0; i < printInCommonItemViewList.Count - 1; i++)
                {
                    if (printInCommonItemViewList[i].DateTimeShow == printInCommonItemViewList[i + 1].DateTimeShow)
                    {
                        printInCommonItemViewList[i].RecordDoctorName = "";
                        printInCommonItemViewList[i].RecordDoctorImg = "";
                        printInCommonItemViewList[i].RecordDoctorImgbyte = null;
                    }
                }

                for (int i = 0; i < printInCommonItemViewList.Count - 1; i++)
                {
                    if (!string.IsNullOrEmpty(printInCommonItemViewList[i].DateTimeShow))
                    {
                        for (int j = i + 1; j < printInCommonItemViewList.Count; j++)
                        {
                            if (printInCommonItemViewList[i].IsZongLiang == 1 || printInCommonItemViewList[i].IsZongLiang == 2) continue;
                            if (printInCommonItemViewList[i].DateTimeShow == printInCommonItemViewList[j].DateTimeShow)
                            {
                                printInCommonItemViewList[j].Date = "";
                                printInCommonItemViewList[j].Time = "";
                                printInCommonItemViewList[j].DateTimeShow = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 将report中配置的值从时间行移动到签名行
        /// 如审阅者签名 必须和签名在同一行
        /// xll 2013-02-28
        /// fileName 为模板名称
        /// </summary>
        /// <param name="printInCommonItemViewList"></param>
        public static void SetRowEnd(List<PrintInCommonItemView> printInCommonItemViewList, string fileName)
        {
            try
            {
                List<string> MyProList = InCommonNoteBiz.GetReportxmlRowEnd(fileName);
                if (MyProList == null || MyProList.Count == 0) return;
                string ValueLast = "";
                //对于时间和记录人确认后，可以记录的头和尾
                for (int i = 0; i < printInCommonItemViewList.Count; i++)
                {

                    foreach (var item in MyProList)
                    {
                        PropertyInfo property = printInCommonItemViewList[i].GetType().GetProperty(item); //获取该对象的该属性
                        if (property == null)
                        {
                            return;
                        }

                        if (printInCommonItemViewList[i].Date != string.Empty)
                        {
                            ValueLast = "";
                            object valueobj = property.GetValue(printInCommonItemViewList[i], null);
                            if (valueobj != null && !string.IsNullOrEmpty(valueobj.ToString()))
                            {
                                ValueLast = valueobj.ToString();
                            }
                            property.SetValue(printInCommonItemViewList[i], "", null);
                        }


                        if (printInCommonItemViewList[i].RecordDoctorName != string.Empty && !string.IsNullOrEmpty(ValueLast))
                        {
                            property.SetValue(printInCommonItemViewList[i], ValueLast, null);
                            ValueLast = "";
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        /// <summary>
        /// 对数据图片签名处理
        /// </summary>
        /// <param name="printInCommonView"></param>
        public static void ConvertForImgRec(PrintInCommonItemView printInCommonItemView)
        {
            try
            {
                if (printInCommonItemView == null || string.IsNullOrEmpty(printInCommonItemView.RecordDoctorImg)) return;
                if (!File.Exists(printInCommonItemView.RecordDoctorImg))
                {
                    return;
                }
                // Bitmap bitmap = new Bitmap(printInCommonItemView.RecordDoctorImg);
                Stream s = File.Open(printInCommonItemView.RecordDoctorImg, FileMode.Open);
                int leng = 0;
                if (s.Length < Int32.MaxValue)
                {
                    leng = (int)s.Length;
                }
                byte[] by = new byte[leng];
                s.Read(by, 0, leng);//把图片读到字节数组中
                s.Close();
                printInCommonItemView.RecordDoctorImgbyte = by;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 添加修改行信息的历史
        /// xll 2013-06-26
        /// </summary>
        /// <param name="inCommonNoteItemEntity"></param>
        public static void AddInCommonRowHistory(InCommonNoteItemEntity inCommonNoteItemEntity, string rowhisflow)
        {

            try
            {
                SqlParameter[] sps ={
                                   new SqlParameter("@historyflow",SqlDbType.VarChar),
                                   new SqlParameter("@rowflow",SqlDbType.VarChar),
                                   new SqlParameter("@createdoctorid",SqlDbType.VarChar),
                                   new SqlParameter("@createdoctorname",SqlDbType.VarChar),
                                   new SqlParameter("@createdatetime",SqlDbType.VarChar),
                                   new SqlParameter("@recorddate",SqlDbType.VarChar),
                                   new SqlParameter("@recordtime",SqlDbType.VarChar),
                                   new SqlParameter("@recorddoctorid",SqlDbType.VarChar),
                                   new SqlParameter("@recorddoctorname",SqlDbType.VarChar),
                                    new SqlParameter("@valide",SqlDbType.VarChar)
                                   };

                sps[0].Value = rowhisflow;
                sps[1].Value = inCommonNoteItemEntity.GroupFlow;
                sps[2].Value = DrectSoft.Common.DS_Common.currentUser.DoctorId;
                sps[3].Value = DrectSoft.Common.DS_Common.currentUser.DoctorName;
                sps[4].Value = DateTime.Now.ToString("yyyyMMddHHmmss");
                sps[5].Value = inCommonNoteItemEntity.RecordDate;
                sps[6].Value = inCommonNoteItemEntity.RecordTime;
                sps[7].Value = inCommonNoteItemEntity.RecordDoctorId;
                sps[8].Value = inCommonNoteItemEntity.RecordDoctorName;
                sps[9].Value = inCommonNoteItemEntity.Valide;
                DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteNonQuery("emr_commonnote.usp_AddInCommHistory", sps, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 添加修改行信息的历史
        /// xll 2013-06-26
        /// </summary>
        /// <param name="inCommonNoteItemEntity"></param>
        public static void AddInCommonColHistory(InCommonNoteItemEntity inCommonNoteItemEntity, string hisRowFlow)
        {

            try
            {

                if (inCommonNoteItemEntity.ValueXml == null)
                {
                    inCommonNoteItemEntity.ValueXml = "";
                }
                SqlParameter[] sps ={
                                   new SqlParameter("@incommonnote_item_flow",SqlDbType.VarChar),
                                   new SqlParameter("@hisrowflow",SqlDbType.VarChar),
                                   new SqlParameter("@valuexml",SqlDbType.VarChar),
                                   new SqlParameter("@commonnote_item_flow",SqlDbType.VarChar),
                                   new SqlParameter("@incommonnote_tab_flow",SqlDbType.VarChar),
                                   new SqlParameter("@incommonnoteflow",SqlDbType.VarChar)
                                   };

                sps[0].Value = inCommonNoteItemEntity.InCommonNote_Item_Flow;
                sps[1].Value = hisRowFlow;
                sps[2].Value = inCommonNoteItemEntity.ValueXml;
                sps[3].Value = inCommonNoteItemEntity.CommonNote_Item_Flow;
                sps[4].Value = inCommonNoteItemEntity.InCommonNote_Tab_Flow;
                sps[5].Value = inCommonNoteItemEntity.InCommonNoteFlow;
                DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteNonQuery("emr_commonnote.usp_AddInCommColHistory", sps, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<InCommonNoteItemEntity> GetIncommHisInfo(string groupflow)
        {
            try
            {
                string sql = string.Format(@"select * from incommonnote_itemHis_view i where i.rowflow='{0}'", groupflow);
                DataTable dt = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sql, CommandType.Text);
                List<InCommonNoteItemEntity> inCommonNoteItemList = DataTableToList<InCommonNoteItemEntity>.ConvertToModel(dt);
                return inCommonNoteItemList;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static DataTable GetDateTable(List<InCommonNoteItemEntity> inCommonNoteItemList, List<CommonNote_ItemEntity> commonNote_Itemlist)
        {
            DataTable dt = new DataTable();
            DataColumn dc = null;

            dc = new DataColumn("groupFlow");
            dc.Caption = "流水号";
            dc.DataType = typeof(String);
            dt.Columns.Add(dc);
            dc = new DataColumn("jlsj");
            dc.Caption = "记录时间";
            dc.DataType = typeof(String);
            dt.Columns.Add(dc);
            foreach (var item in commonNote_Itemlist)
            {
                dc = new DataColumn(item.CommonNote_Item_Flow);
                if (string.IsNullOrEmpty(item.OtherName))
                {
                    dc.Caption = "未指定列";
                }
                else
                {
                    dc.Caption = item.OtherName;
                }
                dt.Columns.Add(dc);
            }

            dc = new DataColumn("jlr");
            dc.Caption = "修改人";
            dc.DataType = typeof(string);
            dt.Columns.Add(dc);

            dc = new DataColumn("xgsj");
            dc.Caption = "修改时间";
            dc.DataType = typeof(string);
            dt.Columns.Add(dc);
            SetValueToDateTable(inCommonNoteItemList, dt);
            return dt;
        }

        private static void SetValueToDateTable(List<InCommonNoteItemEntity> inCommonNoteItemList, DataTable dt)
        {
            //进行分组后的单据项目
            Dictionary<string, List<InCommonNoteItemEntity>> dicitemList = new Dictionary<string, List<InCommonNoteItemEntity>>();
            foreach (var item in inCommonNoteItemList)  //将同一组的数据放在一个里面
            {
                if (!dicitemList.Keys.Contains(item.GroupFlow))
                {
                    dicitemList.Add(item.GroupFlow, new List<InCommonNoteItemEntity>());
                }
                dicitemList[item.GroupFlow].Add(item);
            }

            foreach (List<InCommonNoteItemEntity> itemList in dicitemList.Values)
            {
                if (itemList == null || itemList.Count == 0) return;
                DataRow dtRow = dt.NewRow();
                string dateTime = itemList[0].RecordDate + " " + itemList[0].RecordTime;
                dtRow["jlsj"] = DateUtil.getDateTime(dateTime, DateUtil.NORMAL_MINUTE);
                dtRow["groupFlow"] = itemList[0].GroupFlow;
                dtRow["jlr"] = itemList[0].CreateDoctorName;
                dtRow["xgsj"] = DateUtil.getDateTime(itemList[0].CreateDateTime, DateUtil.NORMAL_LONG);
                for (int i = 0; i < itemList.Count; i++)
                {
                    if (dtRow[itemList[i].CommonNote_Item_Flow] == null) continue;

                    if (!string.IsNullOrEmpty(itemList[i].ValuesShow))
                    {
                        dtRow[itemList[i].CommonNote_Item_Flow] = itemList[i].ValuesShow;
                    }
                }
                dt.Rows.Add(dtRow);
                dt.DefaultView.Sort = "xgsj desc";

            }

        }

        /// <summary>
        /// 查询已删除的历史记录
        /// </summary>
        /// <param name="incommonTabFlow"></param>
        /// <returns></returns>
        public static List<InCommonNoteItemEntity> GetDelInCommonHisInfo(string incommonTabFlow)
        {
            try
            {

                string sql = string.Format(@"select * from incommonnote_itemhis_view icomitem where icomitem.valide = '0'  and icomitem.incommonnote_tab_flow='{0}'", incommonTabFlow);
                DataTable dt = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sql, CommandType.Text);
                List<InCommonNoteItemEntity> inCommonNoteItemList = DataTableToList<InCommonNoteItemEntity>.ConvertToModel(dt);
                return inCommonNoteItemList;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 获取要删除的datatable
        /// </summary>
        /// <param name="inCommonNoteItemList"></param>
        /// <param name="commonNote_Itemlist"></param>
        /// <returns></returns>
        public static DataTable GetDelDateTable(List<InCommonNoteItemEntity> inCommonNoteItemList, List<CommonNote_ItemEntity> commonNote_Itemlist)
        {
            DataTable dt = new DataTable();
            DataColumn dc = null;

            dc = new DataColumn("groupFlow");
            dc.Caption = "流水号";
            dc.DataType = typeof(String);
            dt.Columns.Add(dc);
            dc = new DataColumn("jlsj");
            dc.Caption = "记录时间";
            dc.DataType = typeof(String);
            dt.Columns.Add(dc);
            foreach (var item in commonNote_Itemlist)
            {
                dc = new DataColumn(item.CommonNote_Item_Flow);
                if (string.IsNullOrEmpty(item.OtherName))
                {
                    dc.Caption = "未指定列";
                }
                else
                {
                    dc.Caption = item.OtherName;
                }
                dt.Columns.Add(dc);
            }

            dc = new DataColumn("jlr");
            dc.Caption = "删除人";
            dc.DataType = typeof(string);
            dt.Columns.Add(dc);

            dc = new DataColumn("xgsj");
            dc.Caption = "删除时间";
            dc.DataType = typeof(string);
            dt.Columns.Add(dc);
            SetValueToDateTable(inCommonNoteItemList, dt);
            return dt;
        }

    }
}
