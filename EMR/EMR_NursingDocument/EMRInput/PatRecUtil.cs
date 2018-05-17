using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Library.EmrEditor.Src.Gui;
using System.Data.SqlClient;
using DrectSoft.Emr.Util;
using System.IO;
using System.IO.Compression;
using DrectSoft.Core;
using DrectSoft.Common.Eop;
using System.Xml;

namespace DrectSoft.Core.EMR_NursingDocument.EMRInput.Table
{
    /// <summary>
    ///病历相关辅助类
    /// </summary>
    public class PatRecUtil
    {
        #region LIS

        /// <summary>
        /// 病人报告清单数据集
        /// </summary>
        const string c_SqlList = @"SELECT 
                b203 HospitalNo,
                NULL PatientSerialNo,
                b210 PatName, 
                'LIS' ReportCatalog,
                NULL ReportNo, 
                b201 ApplyItemCode, 
                nvl(b202, b211 || '检测') ApplyItemName, 
                to_char(b208, 'yyyy-MM-dd hh24:mi:ss') SubmitDate, 
                a107 ReleaseDate, 
                'False' HadRead,
                b202 ReportType,
                b204 SampleNo,
                b205 Machine,
                b211 SampleType,
                b209 GetSampleTime
                FROM view_tj_report
                WHERE b203 = '{0}'
                GROUP BY b203,b210,b201,b202,b204,b205,b208,b211,a107,b209
                order by b208 desc";

        const string c_SqlDetail = @"select distinct 
                a101 ItemCode, 
                a102 ItemName, 
                a103 Result, 
                a104 ReferValue,
                a105 Unit,
                a106 HighFlag, 
                b.printnum,
                null ResultColor
                from view_tj_report a, view_tj_testitem b
                where 
                b203 = '{0}'
                and b204 = '{1}'
                and b205 = '{2}'
                and a107='{3}'
                and b211 = '{4}'
                and a102 is not null
                and a.B205 = b.machine_code
                and a.A101 = b.item_code
                order by b.printnum
                ";

        #region 销售演示数据
//        const string c_SqlList = @"SELECT 
//                        HospitalNo,
//                        noofinpat PatientSerialNo,
//                        noofinpat PatName, 
//                        ReportCatalog,
//                        NULL ReportNo, 
//                        NULL ApplyItemCode, 
//                        '检测' || ReportName ApplyItemName, 
//                        SubmitDate, 
//                        ReleaseDate, 
//                        '0' HadRead,
//                        ReportType,
//                        '' SampleNo,
//                        '' Machine,
//                        '' SampleType,
//                        sysdate GetSampleTime   
//                        FROM INPATIENTREPORT
//                        WHERE Reportcatalog = 'LIS'
//                        ORDER BY SubmitDate";

//        const string c_SqlPacs = @"SELECT 
//                        HospitalNo 住院号,
//                        '' 患者姓名,
//                        NULL 项目代码, 
//                        '检测' || ReportName 检查项目, 
//                        sysdate 检查时间,
//                        '已完成' 检查状态,
//                        '' 设备名称
//                        FROM INPATIENTREPORT
//                        WHERE Reportcatalog = 'RIS'
//                        ORDER BY SubmitDate";

//        const string c_SqlDetail = @"select 
//                        Line, 
//                        ItemCode, 
//                        ItemName, 
//                        Result, 
//                        ReferValue,
//                        Unit,
//                        HighFlag, 
//                        ResultColor
//                        from INPATIENTREPORTLISRESLUT
//                        where 
//                        reportid = ltrim('{0}','0')
//                        and ItemName is not null
//                        ";
        #endregion

        #endregion

        #region PACS

        const string c_SqlPacs = @" select p.infeepatientid 住院号, p.patientname 患者姓名, p.sex 性别, p.age 年龄, 
                    p.devicetype 设备类型, p.devicename 设备名称, p.studyscription 检查项目,
                    p.studytime 检查时间, p.studystatusname 检查状态,
                    p.reportdescribe 描述,
                    p.reportdiagnose 诊断,
                    p.reportadvice 建议,
                    p.docname 报告医生, p.operatetime 最终报告时间
                    from PACS31.V_REPORTINFO p
                    where p.infeepatientid = '{0}' ";

        #endregion

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
                    _tzItemTemplate = m_app.SqlHelper.ExecuteDataTable("SELECT MR_NAME as 名称  FROM emrtemplet_item  WHERE ((DEPT_ID='" + m_app.User.CurrentDeptId + "' AND ROWNUM=1) or DEPT_ID='*') and (MR_ATTR='1')  ORDER BY  MR_NAME ");
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
                    //_imageGallery = m_app.SqlHelper.ExecuteDataTable("SELECT ID, G_NAME as 名称 FROM DICT_GALLERY  WHERE (DEPT_ID='" + m_app.User.CurrentDeptId + "' or DEPT_ID='*') and IMG is not null  ORDER BY  FILE_NAME");
                    _imageGallery = m_app.SqlHelper.ExecuteDataTable("SELECT id ID, name AS 名称 FROM imagelibrary WHERE image is not null ORDER BY name");
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
                    //                    _zdItemData = m_app.SqlHelper.ExecuteDataTable(@"SELECT SNO, D_NAME,MR_NAME as 名称 
                    //                                                                     FROM DICT_EDITOR_INPUT  
                    //                                                                     WHERE ( substr(MR_CODE,1,1) = 'B' ) AND D_NAME IN (SELECT DISTINCT DICT_DICT.c_code FROM DICT_DICT)
                    //                                                                     ORDER BY  MR_NAME ");
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

        public DataTable LisReports
        {
            get
            {
                return GetPatReportLis(CurrentInpatient.RecordNoOfHospital.ToString());
            }
        }

        public DataTable PacsReports
        {
            get
            {
                return GetPatReportPacs(CurrentInpatient.RecordNoOfHospital.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DataTable GetZDDetailInfo(string name)
        {
            //TODO

            //return m_app.SqlHelper.ExecuteDataTable("select D_NAME,INPUT,D_CODE  From  DICT_DICT WHERE C_CODE='" + name + "'");
            return m_app.SqlHelper.ExecuteDataTable("select name d_name, py input, id d_code from emr_dictionary_detail where cancel = 'N' and dictionary_id ='" + name + "'");
        }

        const string sql_queryFolder = "SELECT a.CCODE,a.CNAME,a.CTYPE,b.PCODE,b.SNO,b.WRITABLE, a.IMAGE_INDEX,a.SIMAGE_INDEX,a.OPEN_FLAG,a.UTYPE,a.MTYPE,a.MNAME, a.ARGS FROM DICT_CATALOG a,DICT_CATALOG_MODULE b WHERE ( b.PCODE ='{0}' ) AND   ( b.CCODE =a.CCODE ) AND   ( b.APP ='DOCTINFM' )     order by b.SNO, a.ccode";

      
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
        /// 获得护士文书的节点
        /// </summary>
        const string sql_queryFolderHS = "SELECT a.CCODE,a.CNAME,a.CTYPE,b.PCODE,b.SNO,b.WRITABLE, a.IMAGE_INDEX,a.SIMAGE_INDEX,a.OPEN_FLAG,a.UTYPE,a.MTYPE,a.MNAME, a.ARGS FROM DICT_CATALOG a,DICT_CATALOG_MODULE b WHERE (a.ccode='{0}') AND ( b.CCODE =a.CCODE ) AND   ( b.APP ='DOCTINFM' ) order by b.SNO, a.ccode";
        
        /// <summary>
        /// 获取病历文件夹
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable GetFolderInfoHS(string code)
        {
            return m_app.SqlHelper.ExecuteDataTable(string.Format(sql_queryFolderHS, code));

        }

        /// <summary>
        /// 
        /// </summary>
        const string sql_queryPatRec = "select  ID,Name,SortID,TemplateID,Auditor,Owner,CreateTime,AuditTime,HASSUBMIT,HASPRINT,HASSIGN,CaptionDateTime,FIRSTDAILYFLAG,isyihuangoutong,isconfigpagesize,departcode,wardcode from recorddetail t where NOOFINPAT='{0}'AND SORTID='{1}' AND VALID=1 ORDER BY FIRSTDAILYFLAG desc, CaptionDateTime, id ";
        public DataTable GetPatRecInfo(string code, string patid)
        {
            return m_app.SqlHelper.ExecuteDataTable(string.Format(sql_queryPatRec, patid, code));
        }

        //        const string sql_queryTemplate = @"SELECT MR_CLASS, TEMPLET_ID,FILE_NAME,PATH,TOPIC,DEPT_ID,CREATOR_ID,CREATE_DATETIME, LAST_TIME,PERMISSION,MR_CLASS,MR_CODE,MR_NAME,QC_CODE,MR_ATTR, CONTENT_CODE,VISIBLED,NEW_PAGE_FLAG,CHANGE_TOPIC_FLAG,WRITE_TIMES
        //                                            FROM EMR_TEMPLET_INDEX  WHERE ( MR_CLASS ='{0}') AND file_flag = 3 order by MR_CODE";
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
        const string c_SqlQueryTemplatePerson = @" SELECT template_person.ID, template_person.TEMPLATEID, template_person.NAME MR_NAME, 
                                                          template_person.USERID, template_person.VALID, '' CONTENT, 
                                                          template_person.SORTID, template_person.SORTMARK, 
                                                          template_person.sharedid, template_person.MEMO, 
                                                          template_person.PY, template_person.WB, 
                                                          template_person.TYPE, emrtemplet.isconfigpagesize
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
            //return m_app.SqlHelper.ExecuteDataTable(string.Format(sqlMyTemplates, userid, catalog), CommandType.Text);
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

                //return (byte[])table.Rows[0]["xml_doc"];

                //return System.Text.Encoding.Default.GetBytes(UnzipEmrXml(table.Rows[0]["xml_doc"].ToString()));
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

            //using (IDataReader reader = m_app.SqlHelper.ExecuteReader("select item_doc_new ITEM_DOC from emrtemplet_item where MR_NAME='" + name + "'"))
            //{
            //    reader.Read();
            //    if (!reader.IsDBNull(0))
            //        return (byte[])reader.GetValue(0);
            //}
            //return null;

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

        #region LIS 数据
        IDataAccess LisSqlHelper = DataAccessFactory.GetSqlDataAccess("LISDB");
        public DataTable GetPatReportLis(string patid)
        {
            if (LisSqlHelper == null)
            {
                m_app.CustomMessageBox.MessageShow("无法连接到LIS", CustomMessageBoxKind.ErrorOk);
                return null;
            }
            else
            {
                DataTable dt = LisSqlHelper.ExecuteDataTable(string.Format(c_SqlList, patid));

                //return dt; //销售演示用

                //同一时间出现多个组合项目是包含相同明细项目累人，要按照b204（样本号）b205（检查仪器）b206（检查时间）过滤。
                //譬如我们现在同一时间做的 肝功能一组、生化一组、GLU 程序中显示是三个组合项目，但明细项目都是一致的，应过滤按b202累加，即显示为肝功能一组+生化一组+GLU

                List<int> rowNeedDeleteList = new List<int>();
                for (int i = 0; i < dt.Rows.Count; i++ )
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
                return dt;
            }
        }

        public DataTable GetReportDetail(DataRow row)
        {
            try
            {
                DataTable dataTable = null;

                dataTable = LisSqlHelper.ExecuteDataTable(string.Format(c_SqlDetail, row["HospitalNo"], row["SampleNo"], row["Machine"], row["ReleaseDate"], row["SampleType"]));
                return dataTable;
            }
            catch (SqlException e)
            {
                //throw e;
                m_app.CustomMessageBox.MessageShow("无法连接到LIS", CustomMessageBoxKind.ErrorOk);
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
            if (PacsSqlHelper == null)
            {
                m_app.CustomMessageBox.MessageShow("无法连接到Pacs", CustomMessageBoxKind.ErrorOk);
                return null;
            }
            else
            {
                return PacsSqlHelper.ExecuteDataTable(string.Format(c_SqlPacs, patid));
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
            //using (IDataReader reader = m_app.SqlHelper.ExecuteReader("select item_doc_new ITEM_DOC from emrtemplet_item  WHERE MR_NAME='" + name + "'"))
            //{
            //    if (!reader.IsDBNull(0))
            //    {
            //        reader.Read();
            //        return (byte[])reader.GetValue(0);
            //    }
            //    else
            //    {
            //        object obj = "子模板不存在";
            //        return (byte[])obj;
            //    }
            //}



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
            //byte[] buffUnzipXml = Encoding.UTF8.GetBytes(emrContent);
            //MemoryStream ms = new MemoryStream();
            //GZipStream dfs = new GZipStream(ms, CompressionMode.Compress, true);
            //dfs.Write(buffUnzipXml, 0, buffUnzipXml.Length);
            //dfs.Close();
            //ms.Seek(0, SeekOrigin.Begin);
            //byte[] buffZipXml = new byte[ms.Length];
            //ms.Read(buffZipXml, 0, buffZipXml.Length);
            //return Convert.ToBase64String(buffZipXml);

            return RecordDal.ZipEmrXml(emrContent);
        }

        public string UnzipEmrXml(string emrContent)
        {
            //try
            //{
            //    byte[] rbuff = Convert.FromBase64String(emrContent);
            //    MemoryStream ms = new MemoryStream(rbuff);
            //    GZipStream dfs = new GZipStream(ms, CompressionMode.Decompress, true);
            //    StreamReader sr = new StreamReader(dfs, Encoding.UTF8);
            //    string sXml = sr.ReadToEnd();
            //    sr.Close();
            //    dfs.Close();
            //    return sXml;
            //}
            //catch (Exception e)
            //{
            //    System.Diagnostics.Trace.WriteLine(e);
            //    return emrContent;
            //}

            return RecordDal.UnzipEmrXml(emrContent);
        }

        /// <summary>
        /// 设置所有病例模板的拼音和五笔
        /// </summary>
        public void SetEMRTemplatePYWB()
        {
            //string sql = " select templet_id, mr_name from emrtemplet ";
            //string sqlUpdate = " update emrtemplet set py = '{1}', wb = '{2}' where templet_id = '{0}' ";
            //DataTable dt = m_app.SqlHelper.ExecuteDataTable(sql);
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    string templateid = dt.Rows[i]["templet_id"].ToString();
            //    string name = dt.Rows[i]["mr_name"].ToString();
            //    GenerateShortCode shortCode = new GenerateShortCode(m_app.SqlHelper);
            //    string[] code = shortCode.GenerateStringShortCode(name);

            //    string py = code[0]; //PY
            //    string wb = code[1]; //WB

            //    m_app.SqlHelper.ExecuteNoneQuery(string.Format(sqlUpdate, templateid, py, wb), CommandType.Text);
            //}
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
                    _blItemTemplate = m_app.SqlHelper.ExecuteDataTable("select a.source_itemname as 名称 from emr_medicine_node a  where a.valid = '1'");
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
            //            string searchsql = string.Format(@" select node from DEPTREP where id='{0}' 
            //            and parent_node='{1}' order by  node desc ", DiagID, ParentNode);
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
                '{3}','{4}','{5}','{6}')", DiagID, nodevalue, parentnode, title, "", nodevalue, "1");
            r_node = nodevalue;
            r_parentnode = parentnode;
            m_app.SqlHelper.ExecuteNoneQuery(insertNode, CommandType.Text);
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
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["id"].ToString() == recordDetailID)
                {
                    pageIndex = i + 1;
                    break;
                }
            }
            return pageIndex;
        }
    }
}
