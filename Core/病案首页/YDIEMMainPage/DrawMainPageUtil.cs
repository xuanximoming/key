using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.Reflection;
using DrectSoft.DrawDriver;
using DrectSoft.Service;
namespace DrectSoft.Core.IEMMainPage
{
    /// <summary>
    /// 绘制病案首页处理方法
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
        /// <returns>图元集合</returns>
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

        private string GetSpecial(string age/*新生儿年龄*/)
        {
            try
            {
                if (age.Contains(",") && age.Split(',').Length == 3)
                {
                    return age;
                }
                age = age.Replace(",", "");//去掉字符里的符号
                int month = 0;
                int day = 0;
                string temp = string.Empty;
                if (age.Contains("个") && age.Contains("天"))
                {
                    int index = age.IndexOf("月");
                    if (index > 0)
                    {
                        if (!int.TryParse(age.Split('个')[0], out month))
                        {
                            month = 0;
                        }
                        temp = age.Substring(index + 1, age.Length - index - 1);
                    }
                    else
                    {
                        month = 0;
                        temp = age.Substring(1, age.Length - 1);
                    }
                    if (temp.IndexOf('天') > 0)
                    {
                        if (!int.TryParse(temp.Split('天')[0], out  day))
                        {
                            day = 0;
                        }
                    }
                    else
                    {
                        day = 0;
                    }
                }
                else if (!age.Contains("月") && age.Contains("天"))
                {
                    if (!int.TryParse(age.Split('天')[0], out day))
                    {
                        day = 0;
                    }
                }
                else if (!age.Contains("天") && age.Contains("个月"))
                {
                    if (!int.TryParse((age.Split('个')[0]), out month))
                    {
                        month = 0;
                    }
                }
                else
                {
                    return age;//兼容老数据
                }
                return month + "," + day + "," + "30";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 参数集合
        /// </summary>
        /// <param name="m_IemMainPageEntity">病案首页信息对象</param>
        /// <returns>属性集合</returns>
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
                        if (item.Name == "MonthAge")
                        {
                            m_IemMainPageEntity.IemBasicInfo.MonthAge = GetSpecial(m_IemMainPageEntity.IemBasicInfo.MonthAge);
                        }
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
                if (m_IemMainPageEntity.IemOthers != null)
                {
                    propertys = m_IemMainPageEntity.IemOthers.GetType().GetProperties();
                    foreach (PropertyInfo item in propertys)
                    {
                        ParamObject param = new ParamObject(item.Name, "", item.GetValue(m_IemMainPageEntity.IemOthers, null) == null ? "" : item.GetValue(m_IemMainPageEntity.IemOthers, null).ToString());
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
        /// Modify by xlb 2013-04-22西医诊断先填充左侧再填充右侧
        /// </summary>
        /// <returns></returns>
        public DataSet CreateDataTable()
        {
            try
            {
                DataSet dts = new DataSet();
                DataTable outDiagTable = new DataTable();
                DataTable newoutDiagTable = null;// new DataTable();
                outDiagTable = DS_BaseService.AddColsDataToDataTable(m_IemMainPageEntity.IemDiagInfo.OutDiagTable).Copy();
                //if (outDiagTable != null && outDiagTable.Rows.Count > 0) 
                //{
                //    outDiagTable.Rows.RemoveAt(0);
                //}
                if (newoutDiagTable == null)
                {
                    newoutDiagTable = outDiagTable.Copy();//声明一个用于存放去除了门急诊诊断的数据 add by ywk2012年6月4日 11:52:49
                }

                if (outDiagTable.Rows.Count > 0)
                {
                    outDiagTable.DefaultView.RowFilter = " Diagnosis_Type_ID<>'13'";
                    newoutDiagTable = outDiagTable.DefaultView.ToTable().Copy(); ;
                }

                DataTable dtDialog = new DataTable();
                dtDialog.TableName = "outDiagTable";
                dtDialog.Columns.Add("Diagnosis_NameA");
                dtDialog.Columns.Add("Diagnosis_CodeA");
                dtDialog.Columns.Add("Status_IdA");
                dtDialog.Columns.Add("Diagnosis_NameB");
                dtDialog.Columns.Add("Diagnosis_CodeB");
                dtDialog.Columns.Add("Status_IdB");
                dtDialog.Columns.Add("Status_Id_OutA");
                dtDialog.Columns.Add("Status_Id_OutB");
                for (int i = 0; i < 9; i++)
                {
                    DataRow dr = dtDialog.NewRow();
                    if (i == 0)
                    {
                        dr["Diagnosis_NameA"] = "主要诊断:";
                        dr["Diagnosis_NameB"] = "其他诊断:";
                    }
                    else if (i == 1)
                    {
                        dr["Diagnosis_NameA"] = "其他诊断:";
                        dr["Diagnosis_NameB"] = "";
                    }
                    dtDialog.Rows.Add(dr);
                }
                for (int i = 0; i < newoutDiagTable.Rows.Count; i++)//遍历诊断表
                {
                    if (i == 0)//第一行为主要诊断其余则其他诊断
                    {
                        newoutDiagTable.Rows[i]["Diagnosis_Name"] = "主要诊断：" + newoutDiagTable.Rows[i]["Diagnosis_Name"];
                    }
                    else if (i == 1 || i == 9) //其他诊断不是每一个都要添加
                    {
                        newoutDiagTable.Rows[i]["Diagnosis_Name"] = "其他诊断：" + newoutDiagTable.Rows[i]["Diagnosis_Name"];
                    }
                    if (i <= 8)//1到9行填充左边
                    {
                        dtDialog.Rows[i]["Diagnosis_NameA"] = newoutDiagTable.Rows[i]["Diagnosis_Name"];
                        dtDialog.Rows[i]["Diagnosis_CodeA"] = newoutDiagTable.Rows[i]["Diagnosis_Code"];
                        dtDialog.Rows[i]["Status_IdA"] = newoutDiagTable.Rows[i]["Status_Id"];
                        dtDialog.Rows[i]["Status_Id_OutA"] = newoutDiagTable.Rows[i]["Status_Id_Out"];
                    }
                    else if (i <= 17)//10到18行填充右侧，超过则不处理
                    {
                        dtDialog.Rows[i - 9]["Diagnosis_NameB"] = newoutDiagTable.Rows[i]["Diagnosis_Name"];
                        dtDialog.Rows[i - 9]["Diagnosis_CodeB"] = newoutDiagTable.Rows[i]["Diagnosis_Code"];
                        dtDialog.Rows[i - 9]["Status_IdB"] = newoutDiagTable.Rows[i]["Status_Id"];
                        dtDialog.Rows[i - 9]["Status_Id_OutB"] = newoutDiagTable.Rows[i]["Status_Id_Out"];
                    }
                }
                dts.Tables.Add(dtDialog);
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
