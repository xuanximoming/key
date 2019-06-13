using DrectSoft.DrawDriver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.Reflection;
namespace DrectSoft.Core.IEMMainPageZY
{
    /// <summary>
    /// 病案首页绘制方法类
    /// </summary>
    public class DrawMainPageUtil
    {
        /// <summary>
        /// 病案首页实体类
        /// </summary>
        IemMainPageInfo m_IemMainPageEntity = new IemMainPageInfo();

        /// <summary>
        /// 用于设定PictureBox的宽度
        /// </summary>
        public int m_PageWidth = 750;

        /// <summary>
        /// 用于设定PictureBox的高度1100
        /// </summary>
        public int m_PageHeight = 1017;

        /// <summary>
        /// 第一页图片
        /// </summary>
        private Metafile mf1;

        /// <summary>
        /// 第一页
        /// </summary>
        public Metafile MF1
        {
            get
            {
                return mf1;
            }
        }

        /// <summary>
        /// 第二页图片
        /// </summary>
        private Metafile mf2;

        /// <summary>
        /// 第二页
        /// </summary>
        public Metafile MF2
        {
            get
            {
                return mf2;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iemMainPageEntity"></param>
        public DrawMainPageUtil(IemMainPageInfo iemMainPageEntity)
        {
            try
            {
                m_IemMainPageEntity = iemMainPageEntity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取打印图片
        /// </summary>
        /// <returns>图片集合</returns>
        public List<Metafile> GetPrintImage()
        {
            try
            {
                XmlCommomOp.Doc = null;
                XmlCommomOp.xmlPath = AppDomain.CurrentDomain.BaseDirectory + "static.xml";
                XmlCommomOp.CreaetDocument();
                XmlCommomOp.BindingDate(CreateDataTable(), CreateParamDate(m_IemMainPageEntity));
                List<Metafile> listMetafile = DrawOp.MakeImagesByXmlDocument(XmlCommomOp.Doc);
                mf1 = listMetafile[0];
                mf2 = listMetafile[1];
                return listMetafile;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 创造参数集合
        /// </summary>
        /// <param name="m_IemMainPageEntity">病案对象</param>
        /// <returns>参数集合</returns>
        public Dictionary<string, ParamObject> CreateParamDate(IemMainPageInfo m_IemMainPageEntity)
        {
            try
            {
                Dictionary<string, ParamObject> dicParamsList = new Dictionary<string, ParamObject>();
                List<string> list = new List<string>();
                PropertyInfo[] propertys = null;
                if (m_IemMainPageEntity.IemBasicInfo != null)
                {
                    propertys = m_IemMainPageEntity.IemBasicInfo.GetType().GetProperties();
                    foreach (PropertyInfo item in propertys)
                    {
                        ParamObject param = new ParamObject(item.Name, "", item.GetValue(m_IemMainPageEntity.IemBasicInfo, null) == null ? "" : item.GetValue(m_IemMainPageEntity.IemBasicInfo, null).ToString());
                        if (!dicParamsList.ContainsKey(item.Name))
                        {
                            dicParamsList.Add(item.Name, param);
                        }
                    }
                }
                if (m_IemMainPageEntity.IemDiagInfo != null)
                {
                    propertys = m_IemMainPageEntity.IemDiagInfo.GetType().GetProperties();
                    foreach (PropertyInfo item in propertys)
                    {
                        ParamObject param = new ParamObject(item.Name, "", item.GetValue(m_IemMainPageEntity.IemDiagInfo, null) == null ? "" : item.GetValue(m_IemMainPageEntity.IemDiagInfo, null).ToString());
                        if (!dicParamsList.ContainsKey(item.Name))
                        {

                            dicParamsList.Add(item.Name, param);
                        }

                    }
                }
                if (m_IemMainPageEntity.IemOperInfo != null)
                {
                    propertys = m_IemMainPageEntity.IemOperInfo.GetType().GetProperties();
                    foreach (PropertyInfo item in propertys)
                    {
                        ParamObject param = new ParamObject(item.Name, "", item.GetValue(m_IemMainPageEntity.IemOperInfo, null) == null ? "" : item.GetValue(m_IemMainPageEntity.IemOperInfo, null).ToString());
                        if (!dicParamsList.ContainsKey(item.Name))
                        {
                            dicParamsList.Add(item.Name, param);
                        }
                    }
                }
                if (m_IemMainPageEntity.IemFeeInfo != null)
                {
                    propertys = m_IemMainPageEntity.IemFeeInfo.GetType().GetProperties();
                    foreach (PropertyInfo item in propertys)
                    {
                        ParamObject param = new ParamObject(item.Name, "", item.GetValue(m_IemMainPageEntity.IemFeeInfo, null) == null ? "" : item.GetValue(m_IemMainPageEntity.IemFeeInfo, null).ToString());
                        if (!dicParamsList.ContainsKey(item.Name))
                        {
                            dicParamsList.Add(item.Name, param);
                        }
                    }
                }
                if (m_IemMainPageEntity.IemFeeInfo != null)
                {
                    propertys = m_IemMainPageEntity.IemFeeInfo.GetType().GetProperties();
                    foreach (PropertyInfo item in propertys)
                    {
                        ParamObject param = new ParamObject(item.Name, "", item.GetValue(m_IemMainPageEntity.IemFeeInfo, null) == null ? "" : item.GetValue(m_IemMainPageEntity.IemFeeInfo, null).ToString());
                        if (!dicParamsList.ContainsKey(item.Name))
                        {
                            dicParamsList.Add(item.Name, param);
                        }
                    }
                }
                if (m_IemMainPageEntity.IemObstetricsBaby != null)
                {
                    propertys = m_IemMainPageEntity.IemObstetricsBaby.GetType().GetProperties();
                    foreach (PropertyInfo item in propertys)
                    {
                        ParamObject param = new ParamObject(item.Name, "", item.GetValue(m_IemMainPageEntity.IemObstetricsBaby, null) == null ? "" : item.GetValue(m_IemMainPageEntity.IemObstetricsBaby, null).ToString());
                        if (!dicParamsList.ContainsKey(item.Name))
                        {
                            dicParamsList.Add(item.Name, param);
                        }
                    }
                }
                if (m_IemMainPageEntity.IemMainPageExtension != null)
                {
                    DataTable dt = m_IemMainPageEntity.IemMainPageExtension.ExtensionData;
                    foreach (DataRow row in dt.Rows)
                    {
                        ParamObject param = new ParamObject(row[0].ToString(), "", row[1] == null ? "" : row[1].ToString());
                        if (!dicParamsList.ContainsKey(row[0].ToString()))
                        {
                            dicParamsList.Add(row[0].ToString(), param);
                        }
                    }
                }
                return dicParamsList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 建立表格集合
        /// </summary>
        /// <returns>数据集</returns>
        public DataSet CreateDataTable()
        {
            try
            {
                DataSet dts = new DataSet();
                DataTable outDiagTable = new DataTable();
                DataTable newoutDiagTable = null;// new DataTable();
                outDiagTable = m_IemMainPageEntity.IemDiagInfo.OutDiagTable.Copy();

                if (newoutDiagTable == null)
                {
                    newoutDiagTable = outDiagTable.Copy();//声明一个用于存放去除了门急诊诊断的数据 add by ywk2012年6月4日 11:52:49
                }

                if (outDiagTable.Rows.Count > 0)
                {
                    outDiagTable.DefaultView.RowFilter = " Diagnosis_Type_ID<>'13'";
                    newoutDiagTable = outDiagTable.DefaultView.ToTable().Copy(); ;
                }

                //用于存放中医诊断和西医诊断
                DataTable dtZyAndXy = new DataTable();
                dtZyAndXy.TableName = "outDiagTable";
                dtZyAndXy.Columns.Add("Diagnosis_NameZY");
                dtZyAndXy.Columns.Add("Diagnosis_CodeZY");
                dtZyAndXy.Columns.Add("Status_IdZY");
                dtZyAndXy.Columns.Add("Diagnosis_NameXY");
                dtZyAndXy.Columns.Add("Diagnosis_CodeXY");
                dtZyAndXy.Columns.Add("Status_IdXY");
                for (int i = 0; i < 9; i++)
                {
                    DataRow dr = dtZyAndXy.NewRow();
                    if (i == 0)
                    {
                        dr["Diagnosis_NameZY"] = "主病:";
                        dr["Diagnosis_NameXY"] = "主要诊断:";
                    }
                    else if (i == 1)
                    {
                        dr["Diagnosis_NameZY"] = "主证:";
                        dr["Diagnosis_NameXY"] = "其他诊断:";
                    }
                    dtZyAndXy.Rows.Add(dr);
                }
                int zyIndex = 0;
                int xyIndex = 0;
                for (int i = 0; i < newoutDiagTable.Rows.Count; i++)
                {
                    if (newoutDiagTable.Rows[i]["Type"].ToString() == "2" && zyIndex < 9) //中医
                    {
                        if (zyIndex == 0)
                        {
                            newoutDiagTable.Rows[i]["Diagnosis_Name"] = "主病：" + newoutDiagTable.Rows[i]["Diagnosis_Name"];
                        }
                        else if (zyIndex == 1)
                        {
                            newoutDiagTable.Rows[i]["Diagnosis_Name"] = "主证：" + newoutDiagTable.Rows[i]["Diagnosis_Name"];
                        }
                        dtZyAndXy.Rows[zyIndex]["Diagnosis_NameZY"] = newoutDiagTable.Rows[i]["Diagnosis_Name"];
                        dtZyAndXy.Rows[zyIndex]["Diagnosis_CodeZY"] = newoutDiagTable.Rows[i]["Diagnosis_Code"];
                        dtZyAndXy.Rows[zyIndex]["Status_IdZY"] = newoutDiagTable.Rows[i]["Status_Id"];
                        zyIndex++;
                    }
                    else if (newoutDiagTable.Rows[i]["Type"].ToString() == "1" && xyIndex < 9) //西医
                    {
                        if (xyIndex == 0)
                        {
                            newoutDiagTable.Rows[i]["Diagnosis_Name"] = "主要诊断：" + newoutDiagTable.Rows[i]["Diagnosis_Name"];
                        }
                        else if (xyIndex == 1)
                        {
                            newoutDiagTable.Rows[i]["Diagnosis_Name"] = "其他诊断：" + newoutDiagTable.Rows[i]["Diagnosis_Name"];
                        }
                        dtZyAndXy.Rows[xyIndex]["Diagnosis_NameXY"] = newoutDiagTable.Rows[i]["Diagnosis_Name"];
                        dtZyAndXy.Rows[xyIndex]["Diagnosis_CodeXY"] = newoutDiagTable.Rows[i]["Diagnosis_Code"];
                        dtZyAndXy.Rows[xyIndex]["Status_IdXY"] = newoutDiagTable.Rows[i]["Status_Id"];
                        xyIndex++;
                    }
                }

                dts.Tables.Add(dtZyAndXy);

                DataTable operationTable = m_IemMainPageEntity.IemOperInfo.Operation_Table.Clone();
                operationTable.TableName = "operationTable";
                DataTable dtOperationTable = m_IemMainPageEntity.IemOperInfo.Operation_Table;
                //界面只显示八行注原方法有问题xlb 2013-04-22
                for (int i = 0; i < 8; i++)
                {
                    DataRow dataRow = operationTable.NewRow();
                    operationTable.Rows.Add(dataRow);
                }
                if (dtOperationTable != null || dtOperationTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dtOperationTable.Rows.Count; i++)
                    {
                        if (i <= 7)
                        {
                            operationTable.Rows[i].ItemArray = dtOperationTable.Rows[i].ItemArray;
                        }
                    }
                }
                dts.Tables.Add(operationTable);
                return dts;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
