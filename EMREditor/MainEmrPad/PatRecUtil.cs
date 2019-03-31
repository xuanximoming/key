using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Eop;
using DrectSoft.DSSqlHelper;
using DrectSoft.Emr.Util;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Library.EmrEditor.Src.Gui;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace DrectSoft.Core.MainEmrPad
{
    /// <summary>
    ///病历相关辅助类
    /// </summary>
    public class PatRecUtil
    {
        internal IEmrHost m_app;

        public static ZYEditorControl TempEditorControl;

        public Inpatient CurrentInpatient { get; set; }

        public PatRecUtil(IEmrHost host, Inpatient currentInpatient)
        {
            m_app = host;
            CurrentInpatient = currentInpatient;
        }

        /// <summary>
        /// 常用字
        /// 宏替换
        /// </summary>
        public DataTable Macro
        {
            get
            {
                if (_macro == null)
                {
                    _macro = m_app.SqlHelper.ExecuteDataTable("select D_NAME 名称, D_TYPE, D_NAME,D_COLUMN,D_TABLE,D_SQL from register_item   WHERE D_TYPE = '宏'  ORDER BY  D_NAME ");
                    _macro.TableName = "Macro";
                }
                return _macro;
            }


        }
        private DataTable _macro;

        /// <summary>
        /// 病人历史诊断
        /// todo
        /// </summary>
        //public DataTable PatDiag
        //{
        //    get
        //    {
        //        if (_patDiag == null)
        //        {
        //            _patDiag = m_app.SqlHelper.ExecuteDataTable("SELECT  a.DIAG_TYPE_NAME as 诊断类型,a.DIAG_NO as 序号,a.DIAG_SUB_NO as 子号, a.DIAG_CONTENT as 诊断名称 ,a.DIAG_DATE as 诊断日期,a.DIAG_CODE as 诊断编码,a.Diag_Doctor_Id as 医师,a.Diag_Doctor_Id as 实习医师,a.modify_doctor_id as 上级医师,a.LAST_TIME as 上级审签日期,a.super_id as 主任医师,a.SUPER_SIGN_DATE as 主任审签日期 FROM PATDIAG a WHERE  ( a.PATIENT_ID ='00394741') AND (a.NAD =1) ");
        //            _patDiag.TableName = "patDiag";
        //        }
        //        return _patDiag;
        //    }

        //}

        private DataTable _patDiag;

        /// <summary>
        /// 特殊字符集合
        /// </summary>
        public DataTable Symbols
        {
            get
            {
                if (_symbols == null)
                {
                    _symbols = m_app.SqlHelper.ExecuteDataTable("select  SYMBOL as 名称 from DICT_SYMBOL");
                    _symbols.TableName = "symbols";
                }
                return _symbols;
            }
        }
        private DataTable _symbols;

        /// <summary>
        /// 体征相关模板
        /// </summary>
        public DataTable TZItemTemplate
        {
            get
            {
                if (_tzItemTemplate == null)
                {
                    _tzItemTemplate = m_app.SqlHelper.ExecuteDataTable("SELECT MR_NAME as 名称  FROM emrtemplet_item  WHERE ((DEPT_ID='" + m_app.User.CurrentDeptId + "' AND ROWNUM=1) or DEPT_ID='*') and (MR_ATTR='1') and  visibled='1'  ORDER BY  MR_NAME ");
                    _tzItemTemplate.TableName = "tz";
                }
                return _tzItemTemplate;
            }
        }
        private DataTable _tzItemTemplate;

        /// <summary>
        /// image
        /// </summary>
        public DataTable ImageGallery
        {

            get
            {
                if (_imageGallery == null)
                {
                    _imageGallery = m_app.SqlHelper.ExecuteDataTable("SELECT id ID, name AS 名称 FROM imagelibrary WHERE  valid='1'and image is not null ORDER BY name");
                    _imageGallery.TableName = "image";
                }
                return _imageGallery;
            }
        }
        private DataTable _imageGallery;

        /// <summary>
        /// 常用字典库
        /// </summary>
        public DataTable ZDItemData
        {
            get
            {
                if (_zdItemData == null)
                {
                    _zdItemData = m_app.SqlHelper.ExecuteDataTable(@"select id sno, py d_name, name as 名称 FROM emr_dictionary where cancel = 'N'");
                    _zdItemData.TableName = "zd";
                }
                return _zdItemData;
            }
        }
        private DataTable _zdItemData;

        /// <summary>
        /// 症状
        /// </summary>
        public DataTable ZZItemData
        {

            get
            {
                if (_zZItemData == null)
                {
                    _zZItemData = m_app.SqlHelper.ExecuteDataTable("SELECT MR_NAME as 名称  FROM emrtemplet_item  WHERE  (MR_ATTR='1')  ORDER BY  MR_NAME ");
                    _zZItemData.TableName = "zz";
                }
                return _zZItemData;

            }
        }

        private DataTable _zZItemData;

        /// <summary>
        /// 左侧目录
        /// </summary>
        public DataTable FolderInfo
        {
            get
            {
                if (_folderInfo == null)
                {
                    _folderInfo = m_app.SqlHelper.ExecuteDataTable("SELECT a.CCODE,a.CNAME,a.CTYPE,b.PCODE,b.SNO,b.WRITABLE, a.IMAGE_INDEX,a.SIMAGE_INDEX,a.OPEN_FLAG,a.UTYPE,a.MTYPE,a.MNAME, a.ARGS FROM DICT_CATALOG a,DICT_CATALOG_MODULE b WHERE ( b.PCODE ='00' ) AND   ( b.CCODE =a.CCODE ) AND   ( b.APP ='DOCTINFM' )     order by b.SNO");
                }
                return _folderInfo;
            }
        }
        private DataTable _folderInfo;

        /// <summary>
        /// lis结果数据
        /// </summary>
        public DataTable LisReports
        {
            get
            {
                try
                {
                    return GetPatReportLis(CurrentInpatient.RecordNoOfClinic.ToString(), CurrentInpatient.TimesOfAdmission.ToString());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// pacs结果数据
        /// </summary>
        public DataTable PacsReports
        {
            get
            {
                try
                {
                    //大连6院  xll 
                    string valueStr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("PACSRevision");
                    if ("dlly" == valueStr)
                    {
                        return GetPatReportPacs(CurrentInpatient.NoOfHisFirstPage.ToString());
                    }
                    else
                    {
                        return GetPatReportPacs(CurrentInpatient.RecordNoOfClinic.ToString());
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DataTable Orders
        {
            get
            {
                try
                {
                    return GetPatOrders(CurrentInpatient.RecordNoOfClinic.ToString(), CurrentInpatient.TimesOfAdmission.ToString());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 字典
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DataTable GetZDDetailInfo(string name)
        {
            return m_app.SqlHelper.ExecuteDataTable("select name d_name, py input, id d_code from emr_dictionary_detail where cancel = 'N' and dictionary_id ='" + name + "'");
        }

        const string sql_queryFolder = @"SELECT a.CCODE,a.CNAME,a.CTYPE,b.PCODE,b.SNO,b.WRITABLE, a.IMAGE_INDEX,a.SIMAGE_INDEX,a.OPEN_FLAG,a.UTYPE,a.MTYPE,a.MNAME, a.ARGS FROM DICT_CATALOG a,DICT_CATALOG_MODULE b WHERE ( b.PCODE ='{0}' ) AND   ( b.CCODE =a.CCODE ) AND   ( b.APP ='DOCTINFM' )     order by b.SNO, a.ccode";

        const string sql_quertFolderSimpleDoc = @"SELECT a.CCODE,a.CNAME,a.CTYPE,b.PCODE,b.SNO,b.WRITABLE,a.IMAGE_INDEX,a.SIMAGE_INDEX,a.OPEN_FLAG,a.UTYPE,a.MTYPE,a.MNAME,a.ARGS FROM DICT_CATALOG a, DICT_CATALOG_MODULE b  WHERE (b.PCODE = '{0}') AND (b.CCODE = a.CCODE) AND (b.APP = 'DOCTINFM') and a.ccode in ('11','12') order by b.SNO, a.ccode;";

        const string sql_queryoutFolder = @"SELECT a.CCODE,a.CNAME,a.CTYPE,b.PCODE,b.SNO,b.WRITABLE, a.IMAGE_INDEX,a.SIMAGE_INDEX,a.OPEN_FLAG,a.UTYPE,a.MTYPE,a.MNAME, a.ARGS FROM DICT_CATALOG a inner join DICT_CATALOG_MODULE b on a.CCODE=b.CCODE and b.PCODE in('{0}') AND b.APP ='OUTNURSEWS' order by b.SNO, a.ccode";
        /// <summary>
        /// 获取病历文件夹
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable GetFolderInfo(string code)
        {
            return m_app.SqlHelper.ExecuteDataTable(string.Format(sql_queryFolder, code));
        }

        /// <summary>
        /// 获取门诊病历文件夹
        /// add by ukey 2018-11-15
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable GetFolderInfoNew(string code)
        {
            return m_app.SqlHelper.ExecuteDataTable(string.Format(sql_queryoutFolder, code));
        }

        /// <summary>
        /// 获取简版病历文件夹
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable GetFolderInfoSimpleDoc(string code)
        {
            return m_app.SqlHelper.ExecuteDataTable(string.Format(sql_quertFolderSimpleDoc, code));
        }

        /// <summary>
        /// 根据配置项获取值
        /// <auth>XLB</auth>
        /// <date>2013-06-19</date>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValueByConfigKey(string key)
        {
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    DS_SqlHelper.CreateSqlHelper();
                    DataTable dt = DS_SqlHelper.ExecuteDataTable("select value from appcfg where configkey=@configKey and valid='1'",
                        new SqlParameter[] { new SqlParameter("@configKey", key) }, CommandType.Text);
                    string value = string.Empty;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        value = dt.Rows[0]["value"] == null
                            || string.IsNullOrEmpty(dt.Rows[0]["value"].ToString()) ? "AA,13" : dt.Rows[0]["value"].ToString().Trim();
                        return value;
                    }
                    else
                    {
                        return "AA,13";
                    }
                }
                return null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除当前选中病历提取记录
        /// <auth>XLB</auth>
        /// <date>2013-06-21</date>
        /// </summary>
        /// <param name="row">当前需删除行信息</param>
        /// <param name="message">返回信息</param>
        /// <return>返回是否删除成功</return>
        public bool DeleteEmrCollect(DataRow row, ref string message)
        {
            try
            {
                DS_SqlHelper.CreateSqlHelper();
                if (row == null || row["ID"] == null || string.IsNullOrEmpty(row["ID"].ToString()))
                {
                    return false;
                }
                SqlParameter sp1 = new SqlParameter("@emrId", row["ID"].ToString());
                //先判断当前需删除行是否存在记录存在则置为无效
                DataTable dtCount = DS_SqlHelper.ExecuteDataTable(@"select count(*) from template_collect where valid='1'
                and id=@emrId", new SqlParameter[] { sp1 }, CommandType.Text);
                if (dtCount == null || dtCount.Rows.Count <= 0)
                {
                    return false;
                }
                int count = int.Parse(dtCount.Rows[0][0].ToString());
                if (count <= 0)
                {
                    return false;
                }
                DS_SqlHelper.ExecuteNonQuery(@"update  template_collect set valid='0' 
                                                   where ID=@emrId", new SqlParameter[] { sp1 }, CommandType.Text);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        const string sql_queryPatRec = "select  ID,Name,recorddesc,SortID,TemplateID,Auditor,Owner,CreateTime,AuditTime,HASSUBMIT,HASPRINT,HASSIGN,CaptionDateTime,FIRSTDAILYFLAG,isyihuangoutong,isconfigpagesize,departcode,wardcode,changeid from recorddetail t where NOOFINPAT='{0}'AND SORTID='{1}' AND VALID=1 ORDER BY CaptionDateTime asc, id ";
        public DataTable GetPatRecInfo(string code, string patid)
        {
            return m_app.SqlHelper.ExecuteDataTable(string.Format(sql_queryPatRec, patid, code));
        }
        const string sql_queryTemplate = @" select templet_id, file_name, dept_id, creator_id, create_datetime, last_time, substr(mr_name,instr(mr_name,'-',1,2)+1) ordername , " +
                            " permission, mr_class, mr_code, mr_name, mr_attr, qc_code, new_page_flag,  " +
                            " file_flag, write_times, hospital_code, py, wb, " +
                            " isfirstdaily,new_page_flag,isshowfilename,isyihuangoutong,new_page_end,isconfigpagesize " +
                            " from emrtemplet where mr_class ='{0}'  and templet_id in  " +
                            " (select templet2hisdept.templetid from templet2hisdept where templet2hisdept.his_dept_id = '{1}')  " +
                            " and valid = '1' ";

        const string sql_queryTemplateAudited = @" select templet_id, file_name, dept_id, creator_id, create_datetime, last_time, substr(mr_name,instr(mr_name,'-',1,2)+1) ordername ," +
                    " permission, mr_class, mr_code, mr_name, mr_attr, qc_code, new_page_flag,  " +
                    " file_flag, write_times, hospital_code, py, wb, " +
                    " isfirstdaily,new_page_flag,isshowfilename,isyihuangoutong,new_page_end,isconfigpagesize " +
                    " from emrtemplet where mr_class ='{0}'  and templet_id in  " +
                    " (select templet2hisdept.templetid from templet2hisdept where templet2hisdept.his_dept_id = '{1}')  " +
                    " and valid = '1' and state = '2' "; //0、保存未提交   1、提交  2、审核通过  3、审核未通过

        const string sql_ishaveyihuangoutong = " select count(*) from emrtemplet " +
                    " where emrtemplet.templet_id in " +
                    " (     " +
                    " select recorddetail.templateid from recorddetail where recorddetail.noofinpat = '{0}' and recorddetail.valid = '1' " +
                    " ) " +
                    " and emrtemplet.isyihuangoutong = '1' ";

        /// <summary>
        /// 根据类别得到通用模板列表
        /// </summary>
        /// <param name="catalog">模板类别</param>
        /// <returns></returns>
        public DataTable GetTemplate(string catalog, string deptCode)
        {
            string auditConfig = AppConfigReader.GetAppConfig("TempletAuditConfig").Config;
            string newEmrShowType = AppConfigReader.GetAppConfig("NewEmrShowType").Config;//1:按照病种分组  2:不进行分组
            string sqlGetTemplate = string.Empty;
            if (auditConfig.Split('-')[0] == "1")
            {
                if (newEmrShowType == "1")
                {
                    sqlGetTemplate = sql_queryTemplateAudited + " order by ordername ";
                }
                else
                {
                    sqlGetTemplate = sql_queryTemplateAudited + " order by mr_name ";
                }
            }
            else
            {
                if (newEmrShowType == "1")
                {
                    sqlGetTemplate = sql_queryTemplate + " order by ordername ";
                }
                else
                {
                    sqlGetTemplate = sql_queryTemplate + " order by mr_name ";
                }
            }
            DataTable dt = m_app.SqlHelper.ExecuteDataTable(string.Format(sqlGetTemplate, catalog, deptCode));
            //GetActuralTemplate(dt);
            return dt;
        }

        public void GetActuralTemplate(DataTable dt)
        {
            string count = m_app.SqlHelper.ExecuteScalar(string.Format(sql_ishaveyihuangoutong, CurrentInpatient.NoOfFirstPage.ToString()), CommandType.Text).ToString();
            if (count != "0")//已经有医患沟通
            {
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    if (dt.Rows[i]["isyihuangoutong"].ToString() == "1")
                    {
                        dt.Rows.RemoveAt(i);
                    }
                }
            }
            dt.AcceptChanges();
        }

        /// <summary>
        /// 个人模板SQL
        /// </summary>
        const string c_SqlQueryTemplatePerson = @" SELECT distinct template_person.ID, template_person.TEMPLATEID, template_person.NAME MR_NAME, 
                                                          template_person.USERID, template_person.VALID, '' CONTENT, 
                                                          template_person.SORTID, template_person.SORTMARK, 
                                                          template_person.sharedid, template_person.MEMO, 
                                                          template_person.PY, template_person.WB, 
                                                          template_person.TYPE, emrtemplet.isconfigpagesize,emrtemplet.qc_code
                                                     from template_person 
                                                        left outer join emrtemplet 
                                                            on template_person.templateid = emrtemplet.templet_id and emrtemplet.valid = '1'
                                                    where template_person.sortid = '{0}' and template_person.valid = '1' 
                                                          and (template_person.userid = '{1}' or template_person.deptid = '{2}') ";

        const string sqlMyTemplates = @"  select e.templet_id,
                                            substr(e.mr_name,instr(e.mr_name,'-',1,2)+1)ordername,
                                            e.file_name,
                                            e.dept_id,
                                            b.createuser creator_id,
                                            b.createdate create_datetime,
                                            e.last_time,
                                            e.permission,
                                            e.mr_class,
                                            e.mr_code,
                                            b.name mr_name,
                                            e.mr_attr,
                                            e.qc_code,
                                            e.new_page_flag,
                                            e.file_flag,
                                            e.write_times,
                                            e.hospital_code,
                                            nvl(e.isfirstdaily, 0) isfirstdaily,
                                            e.isshowfilename,
                                            e.isyihuangoutong,
                                            e.NEW_PAGE_END,
                                            e.valid,
                                            e.State,
                                            e.py,
                                            e.wb,
                                            e.isconfigpagesize,
                                            (case when (select count(1)
                                                from templet2hisdept t
                                                where t.templetid = e.templet_id)>0 then '★' || mr_name else mr_name end )mr_name   
                                            from emrtemplet e ,DOCTOREMRTEMPLET b
                                        where e.valid = 1 and e.templet_id=b.code and b.CREATEUSER='{0}' and b.mr_class='{1}'  order by ordername ";

        /// <summary>
        /// 根据类别和用户得到个人模板列表
        /// </summary>
        /// <param name="catalog">模板类别</param>
        /// <param name="userid">用户工号</param>
        /// <returns></returns>
        public DataTable GetTemplatePerson(string userid, string deptid, string catalog)
        {
            return m_app.SqlHelper.ExecuteDataTable(string.Format(c_SqlQueryTemplatePerson, catalog, userid, deptid), CommandType.Text);
        }

        /// <summary>
        /// 将模版设置为无效
        /// </summary>
        const string c_SqlDelTemplatePerson = " update template_person set valid = '0' where id = '{0}'";

        /// <summary>
        /// 删除个人模版
        /// </summary>
        /// <param name="templateID"></param>
        public void DelTemplatePerson(string id)
        {
            m_app.SqlHelper.ExecuteNoneQuery(string.Format(c_SqlDelTemplatePerson, id), CommandType.Text);
        }

        const string sql_queryTempalteContent = "select  MR_NAME, xml_doc_new xml_doc from emrtemplet where templet_id='{0}'";

        public /*byte[]*/ string GetTemplateContent(string id, bool isperson)
        {
            if (!isperson)
            {
                DataTable table = m_app.SqlHelper.ExecuteDataTable(string.Format(sql_queryTempalteContent, id));

                if (table.Rows.Count < 1) return null;

                return UnzipEmrXml(table.Rows[0]["xml_doc"].ToString());
            }
            else
            {
                return UnzipEmrXml(LoadPersonTemplateContent(id));
            }
        }
        private string LoadPersonTemplateContent(string templetid)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            {
                new SqlParameter("@EditType",SqlDbType.VarChar),
                new SqlParameter("@CODE",SqlDbType.VarChar),
                new SqlParameter("@CREATEUSER",SqlDbType.VarChar)
            };
            sqlParam[0].Value = "4";
            sqlParam[1].Value = templetid;
            sqlParam[2].Value = m_app.User.Id;
            DataTable dt = m_app.SqlHelper.ExecuteDataSet("EmrTempletFactory.usp_EditMyEmrTemplet", sqlParam, CommandType.StoredProcedure).Tables[0];
            return dt.Rows[0]["XML_DOC_NEW"].ToString();

        }

        public byte[] GetTZDetailInfo(string name)
        {
            DataTable table = m_app.SqlHelper.ExecuteDataTable("select item_doc_new ITEM_DOC from emrtemplet_item where MR_NAME='" + name + "'");
            if (table.Rows.Count < 1) return null;

            return System.Text.Encoding.Default.GetBytes(UnzipEmrXml(table.Rows[0]["ITEM_DOC"].ToString()));
        }

        public byte[] GetImage(string id)
        {
            using (IDataReader reader = m_app.SqlHelper.ExecuteReader("SELECT image FROM imagelibrary WHERE id ='" + id + "' "))
            {
                reader.Read();
                if (!reader.IsDBNull(0))
                {
                    return (byte[])reader.GetValue(0);
                }
            }


            return null;
        }


        public byte[] GetZZItemDetailInfo(string name)
        {
            DataTable table = m_app.SqlHelper.ExecuteDataTable("select item_doc_new ITEM_DOC FROM emrtemplet_item where MR_NAME='" + name + "' ");
            if (table.Rows.Count < 1) return null;

            return System.Text.Encoding.Default.GetBytes(UnzipEmrXml(table.Rows[0]["ITEM_DOC"].ToString()));
        }



        /// <summary>
        /// 得到配置信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetConfigValueByKey(string key)
        {
            string sql = " select * from appcfg where configkey = '" + key + "'  ";
            DataTable dt = m_app.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
            string config = string.Empty;
            if (dt.Rows.Count > 0)
            {
                config = dt.Rows[0]["value"].ToString();
            }
            return config;
        }

        #region LIS 数据
        IDataAccess LisSqlHelper = DataAccessFactory.GetSqlDataAccess("LISDB");
        public DataTable GetPatReportLis(string patid, string visit_id)
        {
            try
            {
                if (LisSqlHelper == null)
                {
                    MyMessageBox.Show("无法连接到LIS");
                    return null;
                }
                else
                {
                    string s_searchList = GetConfigValueByKey("LISListSQL");
                    DataTable dt = new DataTable();
                    //同一时间出现多个组合项目是包含相同明细项目累人，要按照b204（样本号）b205（检查仪器）b206（检查时间）过滤。
                    //譬如我们现在同一时间做的 肝功能一组、生化一组、GLU 程序中显示是三个组合项目，但明细项目都是一致的，应过滤按b202累加，即显示为肝功能一组+生化一组+GLU
                    string LISANDPACSTIP = GetConfigValueByKey("IsOpenLISandPACS") == "" ? "" : GetConfigValueByKey("IsOpenLISandPACS");
                    if (LISANDPACSTIP == "1")//启用销售演示数据 add by ywk LIS、PACS数据提出实现，免得每次销售出去演示都要改一遍
                    {
                        string s_searchList1 = @"SELECT 
                                HospitalNo,
                                noofinpat PatientSerialNo,
                                noofinpat PatName, 
                                ReportCatalog,
                                NULL ReportNo, 
                                NULL ApplyItemCode, 
                                '检测' || ReportName ApplyItemName, 
                               SubmitDate, 
                               ReleaseDate, 
                              '0' HadRead,
                               ReportType,
                                '' SampleNo,
                                '' Machine,
                                '' SampleType,
                               sysdate GetSampleTime   
                               FROM INPATIENTREPORT
                                WHERE Reportcatalog = 'LIS'
                                ORDER BY SubmitDate";
                        dt = LisSqlHelper.ExecuteDataTable(string.Format(s_searchList1, patid));
                        List<int> rowNeedDeleteList = new List<int>();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (!rowNeedDeleteList.Contains(i))
                            {
                                string 样本号1 = dt.Rows[i]["SampleNo"].ToString();
                                string 检验仪器1 = dt.Rows[i]["Machine"].ToString();
                                string 送检日期1 = dt.Rows[i]["GetSampleTime"].ToString();
                                string 检验项目 = dt.Rows[i]["ApplyItemName"].ToString();

                                for (int j = i + 1; j < dt.Rows.Count; j++)
                                {
                                    string 样本号2 = dt.Rows[j]["SampleNo"].ToString();
                                    string 检验仪器2 = dt.Rows[j]["Machine"].ToString();
                                    string 送检日期2 = dt.Rows[j]["GetSampleTime"].ToString();
                                    if (样本号1 == 样本号2 && 检验仪器1 == 检验仪器2 && 送检日期1 == 送检日期2)
                                    {
                                        rowNeedDeleteList.Add(j);
                                        检验项目 += "+" + dt.Rows[j]["ApplyItemName"].ToString();
                                    }
                                }

                                dt.Rows[i]["ApplyItemName"] = 检验项目;
                            }
                        }

                        for (int i = dt.Rows.Count - 1; i >= 0; i--)
                        {
                            if (rowNeedDeleteList.Contains(i))
                            {
                                dt.Rows.RemoveAt(i);
                            }
                        }
                    }

                    if (LISANDPACSTIP == "0" || LISANDPACSTIP == "")//正式使用 
                    {
                        dt = LisSqlHelper.ExecuteDataTable(string.Format(s_searchList, patid, visit_id));
                    }
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获得LIS详细数据列表
        /// add by ywk 2012年10月9日 10:03:53 
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public DataTable GetReportDetail(DataRow row)
        {
            try
            {
                DataTable dataTable = null;
                string s_searchDetail = GetConfigValueByKey("LISDetailSQL");
                string t_lisTip = GetConfigValueByKey("LISTip");
                if (t_lisTip == "1")//武汉作为特殊处理，传两个参数，其他都按照原来的处理 add by ywk  2012年10月9日 10:03:31
                {
                    string LISANDPACSTIP = GetConfigValueByKey("IsOpenLISandPACS") == "" ? "" : GetConfigValueByKey("IsOpenLISandPACS");
                    if (LISANDPACSTIP == "1")
                    {
                        s_searchDetail = @"select 
                                Line, 
                                ItemCode, 
                                ItemName, 
                                Result, 
                                ReferValue,
                                Unit,
                                HighFlag, 
                                ResultColor
                                from INPATIENTREPORTLISRESLUT
                                where 
                               reportid = ltrim('{0}','0')
                                and ItemName is not null";
                        dataTable = LisSqlHelper.ExecuteDataTable(string.Format(s_searchDetail, row["HOSPITALNO"], row["APPLYITEMNAME"], row["APPLYITEMCODE"].ToString()));
                    }
                    else
                    {
                        dataTable = LisSqlHelper.ExecuteDataTable(string.Format(s_searchDetail, row["HOSPITALNO"], row["APPLYITEMNAME"], row["APPLYITEMCODE"].ToString()));
                    }


                }
                else
                {

                    //dataTable = LisSqlHelper.ExecuteDataTable(string.Format(s_searchDetail, row["HOSPITALNO"], row["SAMPLENO"], row["MACHINE"], row["RELEASEDATE"], row["SAMPLETYPE"]));
                    dataTable = LisSqlHelper.ExecuteDataTable(string.Format(s_searchDetail, row["APPLYITEMCODE"]));

                }
                return dataTable;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(4, ex);
                return null;
            }
        }
        #endregion

        #region PACS 数据

        IDataAccess PacsSqlHelper
        {
            get
            {
                if (_pacsSqlHelper == null)
                    _pacsSqlHelper = DataAccessFactory.GetSqlDataAccess("PACSDB");
                return _pacsSqlHelper;
            }
        }
        IDataAccess _pacsSqlHelper;
        public DataTable GetPatReportPacs(string patid)
        {
            try
            {
                if (PacsSqlHelper == null)
                {
                    MyMessageBox.Show("无法连接到Pacs");
                    return null;
                }
                else
                {
                    //防止更新遗漏，没有维护此选项就默认原来的 add by ywk 二〇一三年七月二十三日 13:30:13 
                    string oldpacssql = @" select p.infeepatientid 住院号, p.patientname 患者姓名, p.sex 性别, p.age 年龄, 
                                            p.devicetype 设备类型, p.devicename 设备名称, p.studyscription 检查项目,
                                          p.studytime 检查时间, p.studystatusname 检查状态,
                                            p.reportdescribe 描述,
                                            p.reportdiagnose 诊断,
                                           p.reportadvice 建议,
                                            p.docname 报告医生, p.operatetime 最终报告时间
                                          from V_REPORTINFO p
                                           where p.infeepatientid = '{0}' ";
                    string pacsviewsql = GetConfigValueByKey("PACSLISTSQL") == "" ? oldpacssql : GetConfigValueByKey("PACSLISTSQL");

                    //add  by  ywk 2012年10月9日 10:13:22 
                    if (PacsSqlHelper.GetDbConnection().ConnectionString.IndexOf("Data Source") >= 0)//Oracle的
                    {
                        string LISANDPACSTIP = GetConfigValueByKey("IsOpenLISandPACS") == "" ? "" : GetConfigValueByKey("IsOpenLISandPACS");
                        string c_SqlPacs = string.Empty;
                        if (LISANDPACSTIP == "1")//演示使用add by ywk 2012年12月25日15:45:36 
                        {
                            c_SqlPacs = @"SELECT 
                                HospitalNo 住院号,
                                '' 患者姓名,
                                NULL 项目代码, 
                                '检测' || ReportName 检查项目, 
                                sysdate 检查时间,
                                '已完成' 检查状态,
                                '' 设备名称
                                FROM INPATIENTREPORT
                                WHERE Reportcatalog = 'RIS'
                                ORDER BY SubmitDate";
                        }
                        else
                        {
                            c_SqlPacs = pacsviewsql;
                        }
                        return PacsSqlHelper.ExecuteDataTable(string.Format(c_SqlPacs, patid));
                    }
                    else //SQL的
                    {
                        string SqlPacs = pacsviewsql;
                        return PacsSqlHelper.ExecuteDataTable(string.Format(SqlPacs, patid));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 医嘱数据
        IDataAccess OrderSqlHelper
        {
            get
            {
                if (_orderSqlHelper == null)
                    _orderSqlHelper = DataAccessFactory.GetSqlDataAccess("HISDB");
                return _orderSqlHelper;
            }
        }
        IDataAccess _orderSqlHelper;

        public DataTable GetPatOrders(string patid, string visit_id)
        {
            try
            {
                if (OrderSqlHelper == null)
                {
                    MyMessageBox.Show("无法连接到HIS");
                    return null;
                }
                else
                {
                    string oldorderssql = @"select t.patient_id 病案号, to_char(t.admission_date_time, 'yyyy-mm-dd') 入院时间,
                                            t.visit_id 住院次数
                                              from pat_visit t
                                              where t.patient_id = '{0}'
                                              order by t.visit_id desc";
                    return OrderSqlHelper.ExecuteDataTable(string.Format(oldorderssql, patid));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetPatOrders(DataRow row)
        {
            try
            {
                if (OrderSqlHelper == null)
                {
                    MyMessageBox.Show("无法连接到HIS");
                    return null;
                }
                else
                {
                    string oldorderssql = @"select order_text, dosage,dosage_units, administration, frequency,class_name ORDER_CLASS
                                          from DC_ORDERS 
                                         where patient_id = '{0}' and visit_id = {1} order by date_bgn desc";
                    /*and (order_class = 'A' or order_class = 'B')*/
                    return OrderSqlHelper.ExecuteDataTable(string.Format(oldorderssql, row["病案号"], row["住院次数"]));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        /// <summary>
        /// 得到子模板
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public byte[] GetTempleteItem(string name)
        {

            DataTable table = m_app.SqlHelper.ExecuteDataTable("select item_doc_new ITEM_DOC from emrtemplet_item  WHERE MR_NAME='" + name + "'");
            if (table.Rows.Count < 1)
            {
                object obj = "子模板不存在";
                return (byte[])obj;
            }

            return System.Text.Encoding.Default.GetBytes(UnzipEmrXml(table.Rows[0]["ITEM_DOC"].ToString()));
        }


        public void ResetEditMode(EmrModel model)
        {

            IEmrModelPermision modelPermision;
            bool b;
            DrectSoft.Common.Eop.Employee empl = new DrectSoft.Common.Eop.Employee(m_app.User.Id);

            modelPermision = ModelPermisionFactroy.Create(ModelPermisionType.Audit, empl);
            b = modelPermision.CanDo(model);
            if (b) // 文件已提交，处于审核模式
            {
                // 审核——已提交（未归档），符合级别要求

                //m_ActionAuditAll.Enabled = b;
            }
            else
            {
                //是否可以编辑
                modelPermision = ModelPermisionFactroy.Create(ModelPermisionType.Edit, empl);
                b = modelPermision.CanDo(model);
                //保存按钮状态
                //
                // 提交——新增状态，未归档，本人创建
                modelPermision = ModelPermisionFactroy.Create(ModelPermisionType.Submit, empl);
                b = modelPermision.CanDo(model);

                // 删除——新增状态，未归档，本人创建
                modelPermision = ModelPermisionFactroy.Create(ModelPermisionType.Delete, empl);
                b = modelPermision.CanDo(model);
            }


        }


        const string c_SqlTemplateCollect = " select id, noofinpat,content from template_collect where noofinpat = '{0}' and createuser = '{1}' and valid = '1' order by id ";
        const string c_SqlInsertTemplateCollect = " INSERT INTO TEMPLATE_COLLECT (ID, NOOFINPAT, CREATEUSER, CREATETIME, VALID, CONTENT) " +
            " VALUES ( SEQ_TEMPLATE_COLLECT_ID.NEXTVAL, '{0}', '{1}', sysdate, '1', '{2}' ) ";
        const string c_SqlCancelTemplateCOllect = " update TEMPLATE_COLLECT set TEMPLATE_COLLECT.valid = '0' where id = '{0}' and noofinpat = '{1}' ";

        /// <summary>
        /// 得到病历提取的内容
        /// </summary>
        public DataTable GetDocCollectContent()
        {
            return m_app.SqlHelper.ExecuteDataTable(string.Format(c_SqlTemplateCollect, CurrentInpatient.NoOfFirstPage, m_app.User.Id), CommandType.Text);
        }

        /// <summary>
        /// 插入病历提取的内容到DB
        /// </summary>
        /// <param name="content"></param>
        public void InsertDocCollectContent(string content)
        {
            m_app.SqlHelper.ExecuteNoneQuery(string.Format(c_SqlInsertTemplateCollect, CurrentInpatient.NoOfFirstPage,
                m_app.User.Id, content), CommandType.Text);
        }

        /// <summary>
        /// 作废记录
        /// </summary>
        /// <param name="id"></param>
        public void CancelDocCollectContent(string id)
        {
            m_app.SqlHelper.ExecuteNoneQuery(string.Format(c_SqlCancelTemplateCOllect, id, CurrentInpatient.NoOfFirstPage), CommandType.Text);
        }

        public string ZipEmrXml(string emrContent)
        {
            return RecordDal.ZipEmrXml(emrContent);
        }

        public string UnzipEmrXml(string emrContent)
        {
            return RecordDal.UnzipEmrXml(emrContent);
        }

        /// <summary>
        /// 设置所有病例模板的拼音和五笔
        /// </summary>
        public void SetEMRTemplatePYWB()
        {
        }

        /// <summary>
        /// 病历节点信息 
        /// 
        /// </summary>
        public DataTable BLItemTemplate
        {
            get
            {
                if (_blItemTemplate == null)
                {
                    _blItemTemplate = m_app.SqlHelper.ExecuteDataTable("select a.source_emrname, a.source_itemname as 名称 from emr_medicine_node a  where a.valid = '1'");
                    _blItemTemplate.TableName = "bl";
                }
                return _blItemTemplate;
            }
        }
        private DataTable _blItemTemplate;

        private bool? _IsInsertPageEndOnFirstDailyEmr = null;
        /// <summary>
        /// 在首次病程后面是否加入分页符
        /// </summary>
        public bool IsInsertPageEndOnFirstDailyEmr
        {
            get
            {
                if (_IsInsertPageEndOnFirstDailyEmr == null)
                {
                    string emrSetting = BasicSettings.GetStringConfig("EmrSetting");
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(emrSetting);
                    XmlNodeList nodeList = doc.GetElementsByTagName("FirstDailyEmr");
                    if (nodeList.Count > 0)
                    {
                        XmlElement ele = nodeList[0] as XmlElement;
                        nodeList = ele.GetElementsByTagName("EmrEndInsertPageEnd");
                        if (nodeList.Count > 0)
                        {
                            ele = nodeList[0] as XmlElement;
                            if (ele.InnerText.Trim() == "0")
                            {
                                _IsInsertPageEndOnFirstDailyEmr = false;
                            }
                            else
                            {
                                _IsInsertPageEndOnFirstDailyEmr = true;
                            }
                        }
                    }
                }
                return _IsInsertPageEndOnFirstDailyEmr.Value;
            }
        }

        /// <summary>
        /// 得到病程对应的首次病程记录
        /// </summary>
        /// <param name="recordDetailID"></param>
        /// <returns></returns>
        public DataTable GetFirstDailyEmrContent(string recordDetailID)
        {
            string sqlGetEmrContent = @"select * from recorddetail r1
                                        where r1.sortid = 'AC' and r1.firstdailyflag = '1' and r1.valid = '1' 
                                        and r1.noofinpat in 
                                        (select r2.noofinpat from recorddetail r2 where r2.id = {0})";
            return m_app.SqlHelper.ExecuteDataTable(string.Format(sqlGetEmrContent, recordDetailID));
        }
        /// <summary>
        /// 诊断管理的维护功能里的更新节点内容的方法 
        /// add by ywk 2012年5月22日 11:14:15
        /// </summary>
        /// <param name="nid">表中的ID</param>
        /// <param name="nodeid">节点ID</param>
        /// <param name="parentcode">父节点</param>
        /// <param name="content">内容</param>
        public void UpdateNodeContent(string nid, string nodeid, string parentcode, string content, string title)
        {
            string updateContent = string.Format(@" update DEPTREP set                          content='{0}',title='{1}' where id='{2}' and node='{3}' and parent_node='{4}' ",
                content, title, nid, nodeid, parentcode);
            m_app.SqlHelper.ExecuteNoneQuery(updateContent, CommandType.Text);

        }
        /// <summary>
        ///诊断管理里  新增一个子分类
        /// </summary>
        /// <param name="DiagID"></param>
        /// <param name="p"></param>
        /// <param name="ParentNode"></param>
        /// <param name="NodeTitle"></param>
        /// <param name="NodeContent"></param>
        /// <param name="p_2"></param>
        /// <param name="p_3"></param>
        internal void InsertChildNode(string DiagID, string node, string ParentNode, string NodeTitle, string NodeContent, string indexid, string valid, out string r_node
            , out string r_parentnode)
        {
            string searchsql = string.Format(@" select node from DEPTREP where id='{0}' 
          AND parent_node <> 0 order by  node desc ", DiagID);
            DataTable dt = m_app.SqlHelper.ExecuteDataTable(searchsql, CommandType.Text);
            string nodevalue = string.Empty;
            if (dt.Rows.Count > 0)
            {
                nodevalue = (Int32.Parse(dt.Rows[0]["node"].ToString()) + 1).ToString();
            }
            string insertNode = string.Format(@"insert into DEPTREP values('{0}','{1}','{2}',
                '{3}','{4}','{5}','{6}')", DiagID, nodevalue, ParentNode, NodeTitle, NodeContent, nodevalue, "1");
            r_node = nodevalue;
            r_parentnode = ParentNode;
            m_app.SqlHelper.ExecuteNoneQuery(insertNode, CommandType.Text);
        }
        /// <summary>
        ///  新增大分类
        /// </summary>
        /// <param name="DiagID"></param>
        /// <param name="p"></param>
        /// <param name="DiagID_2"></param>
        /// <param name="p_2"></param>
        /// <param name="p_3"></param>
        /// <param name="p_4"></param>
        /// <param name="p_5"></param>
        internal void InsertParentNode(string DiagID, string node, string ParentNode, string title, string content, string indexid, string valid, out string r_node
            , out string r_parentnode)
        {
            string searchsql = string.Format(@" select * from DEPTREP where id='{0}' 
          AND parent_node <> 0 order by  node desc ", DiagID);
            DataTable dt = m_app.SqlHelper.ExecuteDataTable(searchsql, CommandType.Text);

            string sql = string.Format(@"select * from DEPTREP where parent_node='0' and id='{0}'", DiagID);
            string nodevalue = string.Empty;
            string parentnode = string.Empty;
            if (dt.Rows.Count > 0)
            {
                nodevalue = (Int32.Parse(dt.Rows[0]["node"].ToString()) + 1).ToString();
                //parentnode = (Int32.Parse(dt.Rows[0]["parent_node"].ToString()) + 1).ToString();
            }
            DataTable m_Dt = m_app.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
            if (m_Dt.Rows.Count > 0)
            {
                parentnode = m_Dt.Rows[0]["NODE"].ToString();

            }
            //id 2  parentcode取最大加1 
            string insertNode = string.Format(@"insert into DEPTREP values('{0}','{1}','{2}',
                '{3}','{4}','{5}','{6}')", DiagID, nodevalue, parentnode, title, content, nodevalue, valid);
            r_node = nodevalue;
            r_parentnode = parentnode;
            m_app.SqlHelper.ExecuteNoneQuery(insertNode, CommandType.Text);
        }

        /// <summary>
        /// 修改大分类
        /// </summary>
        /// <param name="DiagID"></param>
        /// <param name="node"></param>
        /// <param name="ParentNode"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        internal void UpdateParentNode(string DiagID, int node, int ParentNode, string title, string content)
        {
            string updateNode = string.Format(@"update DEPTREP set title='{3}',content='{4}' where id='{0}' and node={1} and parent_node={2} ", DiagID, node, ParentNode, title, content);
            m_app.SqlHelper.ExecuteNoneQuery(updateNode, CommandType.Text);
        }

        /// <summary>
        /// 删除大分类
        /// </summary>
        /// <param name="DiagID"></param>
        /// <param name="node"></param>
        /// <param name="ParentNode"></param>
        internal void DeleteParentNode(string DiagID, int node, int ParentNode)
        {
            string deleteNode = string.Format(@"update DEPTREP set valid = '{3}' where id='{0}' and node={1} and parent_node={2} ", DiagID, node, ParentNode, "0");
            m_app.SqlHelper.ExecuteNoneQuery(deleteNode, CommandType.Text);
        }

        internal int GetNurseRecordPageIndex(string recordDetailID, string templateID, string sortID, string noofinpat)
        {
            //'AI'护理记录   'AJ'护理文档   'AK'手术护理记录
            string getSameSortIdEmr = @" select id from recorddetail 
                                          where sortid = '{0}' and valid = 1 and templateid = '{1}' and noofinpat = '{2}'
                                                and sortid in ('AI','AJ','AK') 
                                       order by createtime ";
            DataTable dt = m_app.SqlHelper.ExecuteDataTable(string.Format(getSameSortIdEmr, sortID, templateID, noofinpat));
            int pageIndex = -1;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["id"].ToString() == recordDetailID)
                {
                    pageIndex = i + 1;
                    break;
                }
            }
            return pageIndex;
        }


        public DataRow GetInpatient(string noOfInpatient)
        {

            string sql = @"select * from inpatient where noofinpat='{0}';";
            string sqlformat = string.Format(sql, noOfInpatient);
            DataTable dtInpatient = m_app.SqlHelper.ExecuteDataTable(sqlformat);
            if (dtInpatient != null && dtInpatient.Rows != null && dtInpatient.Rows.Count > 0)
            {
                return dtInpatient.Rows[0];
            }
            return null;

        }

        #region 获取病历信息，供新版文书录入使用 Add by wwj 2013-04-03
        const string sql_queryPatRecByDept = "select t.ID,t.Name,t.recorddesc,t.SortID,t.TemplateID,t.Auditor,t.Owner,t.CreateTime,t.AuditTime,t.HASSUBMIT,t.HASPRINT,t.HASSIGN,t.CaptionDateTime,t.FIRSTDAILYFLAG,t.isyihuangoutong,t.isconfigpagesize,t.departcode,t.wardcode,i.id as changeid from recorddetail t inner join InpatientChangeInfo i on t.noofinpat=i.noofinpat and t.changeid=i.id where t.NOOFINPAT={0} and t.changeid={2} AND t.SORTID='{1}' AND t.VALID=1 ORDER BY CaptionDateTime asc ";
        public DataTable GetPatRecInfoNew(string code, string patid, int changeID)
        {
            try
            {
                return m_app.SqlHelper.ExecuteDataTable(string.Format(sql_queryPatRecByDept, patid, code, changeID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        const string sql_queryPatRecByDeptContainsDel = "select t.ID,t.Name,t.recorddesc,t.SortID,t.valid,t.TemplateID,t.Auditor,t.Owner,t.CreateTime,t.AuditTime,t.HASSUBMIT,t.HASPRINT,t.HASSIGN,t.CaptionDateTime,t.FIRSTDAILYFLAG,t.isyihuangoutong,t.isconfigpagesize,t.departcode,t.wardcode,i.id as changeid from recorddetail t inner join InpatientChangeInfo i on t.noofinpat=i.noofinpat and t.changeid=i.id where t.NOOFINPAT={0} and t.changeid={2} AND t.SORTID='{1}' ORDER BY CaptionDateTime asc ";
        public DataTable GetPatRecInfoContainsDel(string code, string patid, int changeID)
        {
            try
            {
                return m_app.SqlHelper.ExecuteDataTable(string.Format(sql_queryPatRecByDeptContainsDel, patid, code, changeID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        const string sql_queryPatRecByDeptNew = "select  ID,Name,recorddesc,SortID,TemplateID,Auditor,Owner,CreateTime,AuditTime,HASSUBMIT,HASPRINT,HASSIGN,CaptionDateTime,FIRSTDAILYFLAG,isyihuangoutong,isconfigpagesize,departcode,wardcode,changeid from recorddetail t where NOOFINPAT='{0}'AND SORTID='{1}' and departcode='{2}' AND VALID=1 ORDER BY CaptionDateTime asc, id ";
        public DataTable GetPatRecInfoNew(string code, string patid, string deptCode)
        {
            try
            {
                return m_app.SqlHelper.ExecuteDataTable(string.Format(sql_queryPatRecByDeptNew, patid, code, deptCode));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        /// <summary>
        /// 获取病人转科情况
        /// </summary>
        /// <param name="recorddetailID"></param>
        /// <returns></returns>
        public DataTable GetInpatientDeptChange(string recorddetailID)
        {
            try
            {
                string sqlGetInpatientDeptChange = string.Format(@"select i.id, d.name deptname, w.name wardname from inpatientchangeinfo i
                left outer join department d on d.id = i.newdeptid and d.valid = 1
                left outer join ward w on w.id = i.newwardid and w.valid = 1
                where i.noofinpat in (select r.noofinpat from recorddetail r where r.id = '{0}') and i.changestyle in (0, 1, 2) and i.valid = '1'", recorddetailID);
                return DS_SqlHelper.ExecuteDataTable(sqlGetInpatientDeptChange);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region 由于新的数据库底层在多线程的情况下会出错，所以这里暂时使用老的数据库底层操作
        /// <summary>
        /// 获取小模板
        /// </summary>
        /// <param name="deptID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public DataTable GetPersonTemplete(string deptID, string userID)
        {
            try
            {
                string sqlStr = @"select * from emrtemplet_item_person where (deptid=@deptid and isperson=0) or createusers=@userid  ";

                SqlParameter[] paras = new SqlParameter[] 
                {
                    new SqlParameter("@deptid",SqlDbType.VarChar),
                    new SqlParameter("@userid",SqlDbType.VarChar)
                };
                paras[0].Value = deptID;
                paras[1].Value = userID;
                return m_app.SqlHelper.ExecuteDataTable(sqlStr, paras, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取小模板
        /// </summary>
        /// <param name="deptID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public byte[] GetPersonTemplete(string code)
        {
            try
            {
                string sqlStr = @"select item_content from emrtemplet_item_person where code='" + code + "' ";
                DataTable dt = m_app.SqlHelper.ExecuteDataTable(sqlStr);

                return System.Text.Encoding.Default.GetBytes(UnzipEmrXml(dt.Rows[0]["item_content"].ToString()));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取小模板分类
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-28</date>
        /// <param name="deptID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public DataTable GetPersonTempleteFloder(string deptID, string userID)
        {
            try
            {
                string sqlStr = @"select * from emrtemplet_item_person_catalog where valid = 1 and (deptid=@deptid and isperson='0') or createusers=@userid    ";
                SqlParameter[] paras = new SqlParameter[]
                {
                    new SqlParameter("@deptid",SqlDbType.VarChar),
                    new SqlParameter("@userid",SqlDbType.VarChar)
                };
                paras[0].Value = deptID;
                paras[1].Value = userID;
                return m_app.SqlHelper.ExecuteDataTable(sqlStr, paras, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        /// <summary>
        /// 文书录入中的病历信息新增诊断信息数据捞取
        ///方式：【根据当前病人的首页序号到诊断数据表捞取插入】
        ///add  by ywk 2013年6月13日 11:19:18
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        public DataTable GetPatDiagData(string noofinpat)
        {
            string querydiagdata = string.Format(@" select distinct DIAG_TYPE_NAME as 名称 from patdiag where  PATIENT_ID ='{0}'", noofinpat);
            return DS_SqlHelper.ExecuteDataTable(querydiagdata);
        }
    }
}
