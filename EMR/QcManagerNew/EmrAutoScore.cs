using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DrectSoft.Core.IEMMainPage;
using DrectSoft.Core;
using System.Reflection;
using System.ComponentModel;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace DrectSoft.Emr.QcManagerNew
{
    /// <summary>
    ///类名:EmrAutoScore
    ///功能说明:自动评分方法
    ///创建人:wyt
    ///创建时间:2012-11-15
    /// </summary>
    class EmrAutoScore
    {
        private IEmrHost m_app;
        SqlManger m_SqlManger;

        DataTable m_dtMainpageRule = new DataTable();
        string checkMainPageField = "";
        IemMainPageManger manger;
        public EmrAutoScore(IEmrHost app)
        {
            m_app = app;
            m_SqlManger = new SqlManger(app);
            //manger = new IemMainPageManger(DataAccess.App, Common.CommonObjects.CurrentPatient);
            //manger.GetIemInfo();
            DataAccess.App = m_app;
            m_dtMainpageRule = DataAccess.GetIemMainPageQC();
        }

        /// <summary>
        /// 获取病案首页配置代码
        /// </summary>
        /// <returns></returns>
        public string GetAARecord()
        {
            try
            {
                string sql = @"select t.id from emr_configreduction2 t
                             where t.isauto = '0'
                               and length(t.selectcondition) > 0
                               and t.valid = '1' and t.parents ='AA' ";
                DataTable dt = m_app.SqlHelper.ExecuteDataTable(sql);
                if (dt.Rows.Count == 0)
                {
                    return "0";
                }
                return dt.Rows[0][0].ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取病案首页配置代码
        /// </summary>
        /// <returns></returns>
        public string GetAARecordScore()
        {
            try
            {
                string sql = @"select t.reducepoint from emr_configreduction2 t
                             where t.isauto = '0'
                               and t.selectcondition like '%首页%'
                               and t.valid = '1' and t.parents ='AA' ";
                DataTable dt = m_app.SqlHelper.ExecuteDataTable(sql);
                if (dt.Rows.Count == 0)
                {
                    return "5";
                }
                return dt.Rows[0][0].ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 病案首页自动评分
        /// </summary>
        /// <param name="noofinpat">病人首页序号</param>
        /// <param name="pname">病人姓名</param>
        /// <param name="id">主表记录ID</param>
        /// <returns></returns>
        public void AutoScoreAA(string noofinpat, string pname, string id)
        {
            try
            {
                if (m_dtMainpageRule == null)
                {
                    return;
                }
                DataTable basicinfo = DataAccess.GetIemMainpageInfo(noofinpat, 0);  //病案首页基本信息表
                DataTable diag = DataAccess.GetIemMainpageInfo(noofinpat, 1);       //病案首页诊断表
                DataTable oper = DataAccess.GetIemMainpageInfo(noofinpat, 2);       //病案首页手术表
                DataTable baby = DataAccess.GetIemMainpageInfo(noofinpat, 3);       //病案首页婴儿表
                DataTable dtqc = new DataTable();//临时表
                DataTable dtqc_condition = new DataTable();//临时条件表
                DataTable inpat = DataAccess.GetIemMainpageInfo(noofinpat, 4);      //病人表
                DataTable inpatinfo = DataAccess.GetRedactPatientInfoFrm("14", noofinpat);      //病人信息表
                string createuser = ""; //责任医师
                int aarecordid = int.Parse(GetAARecord());
                if (aarecordid == 0) //王冀  2012 12 18 不许评分
                {
                    return;
                }
                string point = GetAARecordScore();
                if (inpatinfo.Rows.Count <= 0)
                {
                    return;
                }
                //无病案首页
                if (basicinfo == null || inpat == null || basicinfo.Rows.Count == 0 || inpat.Rows.Count == 0)
                {
                    m_SqlManger.InsertDB(aarecordid, point, id, noofinpat, pname, "无首页", inpatinfo.Rows[0]["RESIDENT"].ToString());
                    //m_SqlManger.InsertDB(aarecordid, dr["REDUCTSCORE"].ToString(), id, noofinpat, pname, dr["REDUCTREASON"].ToString(), createuser);
                    return;
                }
                pname = basicinfo.Rows[0]["NAME"].ToString();

                //遍历病案首页检查项
                foreach (DataRow dr in m_dtMainpageRule.Rows)
                {
                    #region 初始化检查项参数
                    string tabletype = dr["TABLETYPE"].ToString();                  //检查项表类型
                    string fieldstr = dr["FIELDS"].ToString().Trim();               //检查项字段
                    string fieldvalstr = dr["FIELDSVALUE"].ToString().Trim();       //检查项字段值
                    string[] fields = fieldstr.Split(',');    //检查项字段
                    string[] fieldvals = fieldvalstr.Split(','); //检查项字段值
                    string conditiontabletype = dr["CONDITIONTABLETYPE"].ToString();//检查项条件表类型
                    string conditionfieldstr = dr["CONDITIONFIELDS"].ToString().Trim();      //检查项条件字段
                    string conditionvalstr = dr["CONDITIONFIELDSVALUE"].ToString().Trim();   //检查项条件字段值
                    string[] conditionfields = conditionfieldstr.Split(',');           //检查项条件字段
                    string[] conditionvals = conditionvalstr.Split(',');               //检查项条件字段值
                    bool flag_condition = false;        //条件标识，如果为真则检查
                    bool flag = false;                  //检查标识，如果为真则该扣分不成立
                    #endregion
                    #region 判断条件是否成立，成立则检查
                    switch (conditiontabletype)
                    {
                        case "0":
                            dtqc_condition = basicinfo;
                            break;
                        case "1":
                            dtqc_condition = diag;
                            break;
                        case "2":
                            dtqc_condition = oper;
                            break;
                        case "3":
                            dtqc_condition = baby;
                            break;
                        case "4":
                            dtqc_condition = inpat;
                            break;
                    }
                    if (dtqc_condition.Rows.Count == 0 || conditionfieldstr.Length == 0)
                    {
                        flag_condition = true;      //无条件，则认为是检查条件成立
                    }
                    else
                    {
                        #region 判断条件是否成立
                        foreach (DataRow row in dtqc_condition.Rows)
                        {
                            if (flag_condition == true)
                            {
                                break;
                            }
                            foreach (string val in conditionvals)
                            {
                                if (flag_condition == true)
                                {
                                    break;
                                }
                                if (val == "不为空")
                                {
                                    foreach (string field in conditionfields)
                                    {
                                        if (!dtqc_condition.Columns.Contains(field))
                                        {
                                            continue;
                                        }
                                        if (row[field].ToString().Trim() != "")
                                        {
                                            flag_condition = true;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (string field in conditionfields)
                                    {
                                        if (!dtqc_condition.Columns.Contains(field))
                                        {
                                            continue;
                                        }
                                        if (row[field].ToString().Trim() == val)
                                        {
                                            flag_condition = true;
                                            break;
                                        }
                                    }
                                }
                            }

                            //if (hasCheck == false && flag_condition == false)
                            //{
                            //    foreach (string field in conditionfields)
                            //    {
                            //        if (!dtqc_condition.Columns.Contains(field))
                            //        {
                            //            break;
                            //        }
                            //        if (row[field].ToString().Trim() != "")
                            //        {
                            //            flag_condition = true;
                            //            break;
                            //        }
                            //    }
                            //}
                        }
                        #endregion
                    }
                    #endregion
                    #region 如果条件不成立，则不检查
                    if (flag_condition == false)
                    {
                        continue;
                    }
                    #endregion
                    #region 根据表类型号找出待校验数据
                    switch (tabletype)
                    {
                        case "0":
                            dtqc = basicinfo;
                            break;
                        case "1":
                            dtqc = diag;
                            break;
                        case "2":
                            dtqc = oper;
                            break;
                        case "3":
                            dtqc = baby;
                            break;
                        case "4":
                            dtqc = inpat;
                            break;
                    }
                    #endregion
                    #region 如果检查表为空，则该扣分成立
                    if (dtqc == null || dtqc.Rows.Count == 0)
                    {
                        m_SqlManger.InsertDB(aarecordid, dr["REDUCTSCORE"].ToString(), id, noofinpat, pname, dr["REDUCTREASON"].ToString(), inpatinfo.Rows[0]["RESIDENT"].ToString());
                    }
                    #endregion
                    #region 检查该项
                    else
                    {
                        foreach (DataRow row in dtqc.Rows)
                        {
                            createuser = row["CREATE_USER"].ToString();
                            if (flag == true)
                            {
                                break;
                            }
                            foreach (string val in fieldvals)
                            {
                                if (flag == true)
                                {
                                    break;
                                }

                                if (val == "不为空")
                                {
                                    foreach (string field in fields)
                                    {
                                        if (!dtqc.Columns.Contains(field))
                                        {
                                            break;
                                        }
                                        if (row[field].ToString().Trim() != "")
                                        {
                                            flag = true;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (string field in fields)
                                    {
                                        if (!dtqc.Columns.Contains(field))
                                        {
                                            continue;
                                        }
                                        if (row[field].ToString().Trim() == val)
                                        {
                                            flag = true;
                                            break;
                                        }
                                    }
                                }
                            }

                            //if (hasCheck == false && flag == false)
                            //{
                            //    foreach (string field in fields)
                            //    {
                            //        if (!dtqc.Columns.Contains(field))
                            //        {
                            //            break;
                            //        }
                            //        if (row[field].ToString().Trim() != "")
                            //        {
                            //            flag = true;
                            //            createuser = row["CREATE_USER"].ToString();
                            //            break;
                            //        }
                            //    }
                            //}
                        }
                        if (flag == false)//如果未检查出该项，则扣分
                        {
                            m_SqlManger.InsertDB(aarecordid, dr["REDUCTSCORE"].ToString(), id, noofinpat, pname, dr["REDUCTREASON"].ToString(), createuser);
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }


        ///// <summary>
        ///// 病案首页自动评分
        ///// </summary>
        ///// <param name="noofinpat">病人首页序号</param>
        ///// <returns>扣分表</returns>
        //public DataTable AutoScoreAA(string noofinpat)
        //{
        //    if (m_dtMainpageRule == null)
        //    {
        //        return null;
        //    }
        //    DataTable basicinfo = DataAccess.GetIemMainpageInfo(noofinpat, 0);
        //    DataTable diag = DataAccess.GetIemMainpageInfo(noofinpat, 1);
        //    DataTable oper = DataAccess.GetIemMainpageInfo(noofinpat, 2);
        //    DataTable baby = DataAccess.GetIemMainpageInfo(noofinpat, 3);
        //    DataTable dtqc = new DataTable();
        //    DataTable reductscore = new DataTable();
        //    reductscore.Columns.Add("序号");
        //    reductscore.Columns.Add("评分项目");
        //    reductscore.Columns.Add("扣分");
        //    int index = 0;
        //    //无病案首页
        //    if (basicinfo == null)
        //    {
        //        DataRow dr = reductscore.NewRow();
        //        dr["序号"] = index;
        //        index++;
        //        dr["评分项目"] = "无首页";
        //        dr["扣分"] = "5";
        //        reductscore.Rows.Add(dr);
        //        return reductscore;
        //    }
        //    //病案首页质控项
        //    foreach (DataRow dr in m_dtMainpageRule.Rows)
        //    {
        //        int tabletype = int.Parse(dr["TABLETYPE"].ToString());
        //        string[] fields = dr["FIELDS"].ToString().Split(',');
        //        string val = dr["FILEDSVALUE"].ToString().Trim();
        //        bool flag = false;
        //        switch (tabletype)
        //        {
        //            case 0:
        //                dtqc = basicinfo;
        //                break;
        //            case 1:
        //                dtqc = diag;
        //                break;
        //            case 2:
        //                dtqc = oper;
        //                break;
        //            case 3:
        //                dtqc = baby;
        //                break;
        //            case 4:
        //                dtqc = null;
        //                break;
        //        }
        //        if (dtqc.Rows.Count > 0)
        //        {
        //            foreach (DataRow row in dtqc.Rows)
        //            {
        //                if (flag == true)
        //                {
        //                    flag = false;
        //                    break;
        //                }
        //                if (val != "")
        //                {
        //                    foreach (string field in fields)
        //                    {
        //                        if (row[field].ToString().Trim() == val)
        //                        {
        //                            flag = true;
        //                            break;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    foreach (string field in fields)
        //                    {
        //                        if (row[field].ToString().Trim() != "")
        //                        {
        //                            flag = true;
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //            if (flag == false)
        //            {
        //                DataRow r = reductscore.NewRow();
        //                r["序号"] = index;
        //                index++;
        //                r["评分项目"] = dr["REDUCTREASON"];
        //                r["扣分"] = dr["REDUCTSCORE"];
        //                reductscore.Rows.Add(r);
        //            }
        //        }
        //    }
        //    return reductscore;
        //}

        ///// <summary>
        ///// 病案首页自动评分
        ///// </summary>
        ///// <param name="noofinpat">病人首页序号</param>
        ///// <returns>扣分表</returns>
        //public DataTable AutoScoreAA(string noofinpat)
        //{
        //    DataTable reductscore = new DataTable();
        //    reductscore.Columns.Add("序号");
        //    reductscore.Columns.Add("评分项目");
        //    reductscore.Columns.Add("扣分");
        //    int index = 0;
        //    //病案首页序号为空，无首页
        //    if (manger.IemInfo.IemBasicInfo.Iem_Mainpage_NO == "")
        //    {
        //        DataRow dr = reductscore.NewRow();
        //        dr["序号"] = index.ToString();
        //        dr["评分项目"] = "无首页";
        //        dr["扣分"] = "5";
        //        reductscore.Rows.Add(dr);
        //        index++;
        //        return reductscore;
        //    }
        //    //判断病案首页信息实体数据是否为空
        //    PropertyInfo[] basicinfo = manger.IemInfo.IemBasicInfo.GetType().GetProperties();
        //    foreach (PropertyInfo info in basicinfo)
        //    {
        //        if (info.GetValue(manger.IemInfo.IemBasicInfo, null).ToString() == "")
        //        {
        //            DataRow dr = reductscore.NewRow();
        //            dr["序号"] = index.ToString();
        //            dr["评分项目"] = "缺项一处：" + ((DescriptionAttribute)Attribute.GetCustomAttribute(info, typeof(DescriptionAttribute))).Description;
        //            dr["扣分"] = "0.5";
        //            reductscore.Rows.Add(dr);
        //            index++;
        //        }
        //    }
        //    if (manger.IemInfo.IemDiagInfo.OutDiagTable.Rows.Count != 0)
        //    {
        //        PropertyInfo[] diag = manger.IemInfo.IemDiagInfo.GetType().GetProperties();
        //        foreach (PropertyInfo info in diag)
        //        {
        //            if (info.PropertyType == typeof(System.String))
        //            {
        //                if (info.GetValue(manger.IemInfo.IemDiagInfo, null).ToString() == "")
        //                {
        //                    DataRow dr = reductscore.NewRow();
        //                    dr["序号"] = index.ToString();
        //                    dr["评分项目"] = "缺项一处：" + ((DescriptionAttribute)Attribute.GetCustomAttribute(info, typeof(DescriptionAttribute))).Description;
        //                    dr["扣分"] = "0.5";
        //                    reductscore.Rows.Add(dr);
        //                    index++;
        //                }
        //            }
        //            else if (info.PropertyType == typeof(DataTable))
        //            {
        //                DataTable dt = (DataTable)info.GetValue(manger.IemInfo.IemDiagInfo, null);
        //                int count = dt.Columns.Count;
        //                foreach (DataRow row in dt.Rows)
        //                {
        //                    for (int i = 0; i < count; i++)
        //                    {
        //                        if (row[i].ToString() == "")
        //                        {
        //                            DataRow dr = reductscore.NewRow();
        //                            dr["序号"] = index.ToString();
        //                            dr["评分项目"] = "缺项一处：" + ((DescriptionAttribute)Attribute.GetCustomAttribute(info, typeof(DescriptionAttribute))).Description;
        //                            dr["扣分"] = "0.5";
        //                            reductscore.Rows.Add(dr);
        //                            index++;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    if (manger.IemInfo.IemOperInfo.Operation_Table.Rows.Count != 0)
        //    {
        //        PropertyInfo[] oper = manger.IemInfo.IemOperInfo.GetType().GetProperties();
        //        foreach (PropertyInfo info in oper)
        //        {
        //            if (info.PropertyType == typeof(System.String))
        //            {
        //                if (info.GetValue(manger.IemInfo.IemOperInfo, null).ToString() == "")
        //                {
        //                    DataRow dr = reductscore.NewRow();
        //                    dr["序号"] = index.ToString();
        //                    dr["评分项目"] = "缺项一处：" + ((DescriptionAttribute)Attribute.GetCustomAttribute(info, typeof(DescriptionAttribute))).Description;
        //                    dr["扣分"] = "0.5";
        //                    reductscore.Rows.Add(dr);
        //                    index++;
        //                }
        //            }
        //            else if (info.PropertyType == typeof(DataTable))
        //            {
        //                DataTable dt = (DataTable)info.GetValue(manger.IemInfo.IemOperInfo, null);
        //                int count = dt.Columns.Count;
        //                foreach (DataRow row in dt.Rows)
        //                {
        //                    for (int i = 0; i < count; i++)
        //                    {
        //                        if (row[i].ToString() == "")
        //                        {
        //                            DataRow dr = reductscore.NewRow();
        //                            dr["序号"] = index.ToString();
        //                            dr["评分项目"] = "缺项一处：" + ((DescriptionAttribute)Attribute.GetCustomAttribute(info, typeof(DescriptionAttribute))).Description;
        //                            dr["扣分"] = "0.5";
        //                            reductscore.Rows.Add(dr);
        //                            index++;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    if (manger.IemInfo.IemObstetricsBaby.OutBabyTable.Rows.Count != 0)
        //    {
        //        PropertyInfo[] baby = manger.IemInfo.IemObstetricsBaby.GetType().GetProperties();
        //        foreach (PropertyInfo info in baby)
        //        {
        //            if (info.PropertyType == typeof(System.String))
        //            {
        //                if (info.GetValue(manger.IemInfo.IemObstetricsBaby, null).ToString() == "")
        //                {
        //                    DataRow dr = reductscore.NewRow();
        //                    dr["序号"] = index.ToString();
        //                    dr["评分项目"] = "缺项一处：" + ((DescriptionAttribute)Attribute.GetCustomAttribute(info, typeof(DescriptionAttribute))).Description;
        //                    dr["扣分"] = "0.5";
        //                    reductscore.Rows.Add(dr);
        //                    index++;
        //                }
        //            }
        //            else if (info.PropertyType == typeof(DataTable))
        //            {
        //                DataTable dt = (DataTable)info.GetValue(manger.IemInfo.IemObstetricsBaby, null);
        //                int count = dt.Columns.Count;
        //                foreach (DataRow row in dt.Rows)
        //                {
        //                    for (int i = 0; i < count; i++)
        //                    {
        //                        if (row[i].ToString() == "")
        //                        {
        //                            DataRow dr = reductscore.NewRow();
        //                            dr["序号"] = index.ToString();
        //                            dr["评分项目"] = "缺项一处：" + ((DescriptionAttribute)Attribute.GetCustomAttribute(info, typeof(DescriptionAttribute))).Description;
        //                            dr["扣分"] = "0.5";
        //                            reductscore.Rows.Add(dr);
        //                            index++;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return reductscore;

        //    ////DataTable basicinfo = DataAccess.GetIemMainpageInfo(noofinpat, 0);
        //    //DataTable diag = DataAccess.GetIemMainpageInfo(noofinpat, 1);
        //    //DataTable oper = DataAccess.GetIemMainpageInfo(noofinpat, 2);
        //    //DataTable baby = DataAccess.GetIemMainpageInfo(noofinpat, 3);
        //    //if (basicinfo != null)
        //    //{
        //    //    DataRow dr = reductscore.NewRow();
        //    //    dr["序号"] = 0;
        //    //    dr["评分项目"] = "无首页";
        //    //    dr["扣分"] = "5";
        //    //    reductscore.Rows.Add(dr);
        //    //    return reductscore;
        //    //}
        //    //else
        //    //{ 
        //    //}
        //}
    }
}
