using DrectSoft.Core;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace DrectSoft.Emr.TemplateFactory
{
    public class SQLUtil
    {

        IEmrHost m_app;

        private string sqlMyTemplates = @"  select e.templet_id,
                                            substr(e.mr_name,instr(e.mr_name,'-',1,2)+1)ordername,
                                            e.file_name,
                                            e.dept_id,
                                            b.createuser creator_id,
                                            b.createdate create_datetime,
                                            e.last_time,
                                            e.permission,
                                            e.mr_class,
                                            e.mr_code,
                                            /*mr_name,*/
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
                                            e.isconfigpagesize,
                                            (case when (select count(1)
                                                from templet2hisdept t
                                                where t.templetid = e.templet_id)>0 then '★' || mr_name else mr_name end )mr_name   
                                            from emrtemplet e ,DOCTOREMRTEMPLET b
                                        where e.valid = 1 and e.templet_id=b.code and b.CREATEUSER='{0}' order by ordername ";


        public SQLUtil(IEmrHost app)
        {
            m_app = app;
        }
        public SQLUtil()
        {

        }

        public void SaveTempltepackge(string name)
        {
            try
            {
                DrectSoft.Core.GenerateShortCode d = new GenerateShortCode(m_app.SqlHelper);
                string[] code = d.GenerateStringShortCode(name);
                string py = code[0];
                string wb = code[1];
                string SqlSave = @"select max(to_number(dept_id)) id from emrdept where dept_id <>'*'";
                DataTable DtSqlSave;
                DtSqlSave = m_app.SqlHelper.ExecuteDataTable(SqlSave);
                DataRow DrSqlSave = DtSqlSave.Rows[0];
                string StMaxId = (Int32.Parse(DrSqlSave["id"].ToString()) + 1).ToString();
                SqlSave = null;
                SqlSave = @"Insert into emrdept(dept_id,dept_name,py,wb) values ('" + StMaxId + "','" + name + "','" + py + "','" + wb + "')";
                m_app.SqlHelper.ExecuteNoneQuery(SqlSave);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// emr科室
        /// </summary>
        public DataTable EmrDepartment
        {
            get
            {
                if (_emrDepartMent == null)
                {
                    //if (m_app.User.GWCodes.Split(',').Contains("00"))
                    //_emrDepartMent = m_app.SqlHelper.ExecuteDataTable("select DEPT_ID,DEPT_NAME from EMRDEPT");
                    //else
                    //{
                    //    //筛掉文件夹下无模板的节点 ywk 2012年6月19日 15:17:44
                    _emrDepartMent = m_app.SqlHelper.ExecuteDataTable("select DEPT_ID,DEPT_NAME from EMRDEPT where dept_id in (select distinct b.emr_dept_id  from emrdept2his b,emrtemplet c where b.his_dept_id='" + m_app.User.CurrentDeptId + "' and b.emr_dept_id=c.dept_id ) order by DEPT_NAME ");

                    _emrDepartMent = GetEmrDeptMent(_emrDepartMent);

                    //}
                }
                return _emrDepartMent;
            }
        }
        /// <summary>
        /// 根据配置操作模板列表
        /// **ywk*
        /// </summary>
        /// <param name="_emrDepartMent"></param>
        /// <returns></returns>
        private DataTable GetEmrDeptMent(DataTable _emrDepartMent)
        {
            bool IsOperDt = false;
            DataTable newdt = new DataTable();
            SQLUtil sqlutil = new SQLUtil(m_app);
            string configvalue = sqlutil.GetConfigValueByKey("UniversalTempletConfig");
            string c_UserJobId = m_app.User.GWCodes;//当前登录人的jobid标识
            string[] userJobid = c_UserJobId.Split(',');
            if (!string.IsNullOrEmpty(configvalue))
            {
                if (configvalue.Contains(","))//配置了多个角色可查看
                {
                    string[] configjobid = configvalue.Split(',');//配置里的多个角色jobid
                    for (int i = 0; i < configjobid.Length; i++)//先循环配置里所有jobid
                    {
                        for (int j = 0; j < userJobid.Length; j++)//再循环登录人的多个jobid
                        {
                            if (configjobid[i] == userJobid[j])
                            {
                                IsOperDt = true;//循环到配置中有此角色时，就没必要再循环了
                                break;
                            }
                            //else
                            //{
                            //    IsOperDt = false;
                            //}
                        }
                    }
                }
                else//只配置了一个角色
                {
                    foreach (string item in userJobid)//取出
                    {
                        if (item == configvalue)//当前登录人的jobid在系统配置中,
                        {
                            IsOperDt = true;
                            break;
                        }
                        //else
                        //{
                        //    IsOperDt = false;
                        //}
                    }
                }
            }
            if (!IsOperDt)//如果配置了就把DT数据处理
            {
                newdt = _emrDepartMent.Copy();
                DataRow[] dr = newdt.Select("DEPT_ID='*' ");
                for (int i = 0; i < dr.Length; i++)
                {
                    newdt.Rows.Remove(dr[i]);
                }
            }
            else
            {
                //newdt = _emrDepartMent.Copy();
                newdt = m_app.SqlHelper.ExecuteDataTable("select DEPT_ID,DEPT_NAME from EMRDEPT order by DEPT_NAME "); //在配置里的可以看到所有的
            }
            return newdt;
        }
        private DataTable _emrDepartMent;

        /// <summary>
        /// 模板文件夹
        /// </summary>
        public DataTable ModelContainers
        {
            get
            {
                if (_modelContainers == null)
                {

                    _modelContainers = m_app.SqlHelper.ExecuteDataTable("select CCODE,CNAME,CTYPE,OPEN_FLAG from DICT_CATALOG where CTYPE = '2' and ISUSED = '1' and (UTYPE = '3' or UTYPE = '1') and CNAME<>'子女病程记录' ");

                }
                return _modelContainers;
            }
        }
        private DataTable _modelContainers;


        /// <summary>
        /// 模板列表
        /// </summary>
        public DataTable SimpleModelList
        {
            //get
            //{
            //    DataTable dt = null;
            //    dt = m_app.SqlHelper.ExecuteDataTable("select templet_id TEMPLETE_ID, substr(mr_name, instr(mr_name,'-',1,2)+1)ORDERNAME, dept_id,  mr_class, State, valid from emrtemplet");


            //    return dt;
            //}


            get
            {
                if (_simpleModelList == null)
                {
                    string sql = @"  select templet_id TEMPLETE_ID,
                                            substr(mr_name,instr(mr_name,'-',1,2)+1)ORDERNAME,
                                            file_name,
                                            dept_id,
                                            creator_id,
                                            create_datetime,
                                            last_time,
                                            permission,
                                            mr_class,
                                            mr_code,
                                            /*mr_name,*/
                                            mr_attr,
                                            qc_code,
                                            new_page_flag,
                                            file_flag,
                                            write_times,
                                            hospital_code,
                                            nvl(isfirstdaily, 0) isfirstdaily,
                                            isshowfilename,
                                            isyihuangoutong,
                                            NEW_PAGE_END,
                                            valid,
                                            State,
                                            isconfigpagesize,
                                            (case when (select count(1)
                                                from templet2hisdept t
                                                where t.templetid = e.templet_id)>0 then '★' || mr_name else mr_name end )mr_name   
                                        from emrtemplet e
                                        where valid = 1  ";
                    if (m_app.User.GWCodes.Split(',').Contains("00"))
                        _simpleModelList = m_app.SqlHelper.ExecuteDataTable(sql + " order by ORDERNAME");
                    else
                        _simpleModelList = m_app.SqlHelper.ExecuteDataTable(sql + " and dept_id in (select a.dept_id  from emrdept a where a.dept_id in (select b.emr_dept_id  from emrdept2his b where b.his_dept_id='" + m_app.User.CurrentDeptId + "')) order by ORDERNAME ");
                }
                return _simpleModelList;
            }
            set
            {
                _simpleModelList = value;
            }
        }
        /// <summary>
        /// 模板列表
        /// </summary>
        public DataTable ModelList
        {
            get
            {
                if (_modelList == null)
                {
                    //_modelList = m_app.SqlHelper.ExecuteDataTable("select templet_id, file_name, path, dept_id, creator_id, create_datetime, last_time, permission, mr_class, mr_code, mr_name, mr_attr, qc_code, content_code, visibled, new_page_flag, sno, change_topic_flag, parent_id, parent_modify_datetime, supper_id, file_flag, write_times,hospital_code from EMR_TEMPLET_INDEX where   file_flag >= 2 order by CREATE_DATETIME");
                    //yxy 加载模板值改掉               
                    string sql = @"  select templet_id,
                                            substr(mr_name,instr(mr_name,'-',1,2)+1)ordername,
                                            file_name,
                                            dept_id,
                                            creator_id,
                                            create_datetime,
                                            last_time,
                                            permission,
                                            mr_class,
                                            mr_code,
                                            /*mr_name,*/
                                            mr_attr,
                                            qc_code,
                                            new_page_flag,
                                            file_flag,
                                            write_times,
                                            hospital_code,
                                            nvl(isfirstdaily, 0) isfirstdaily,
                                            isshowfilename,
                                            isyihuangoutong,
                                            NEW_PAGE_END,
                                            valid,
                                            State,
                                            isconfigpagesize,
                                            (case when (select count(1)
                                                from templet2hisdept t
                                                where t.templetid = e.templet_id)>0 then '★' || mr_name else mr_name end )mr_name   
                                        from emrtemplet e
                                        where valid = 1  ";
                    if (m_app.User.GWCodes.Split(',').Contains("00"))
                        _modelList = m_app.SqlHelper.ExecuteDataTable(sql + " order by ordername");
                    else
                        _modelList = m_app.SqlHelper.ExecuteDataTable(sql + " and dept_id in (select a.dept_id  from emrdept a where a.dept_id in (select b.emr_dept_id  from emrdept2his b where b.his_dept_id='" + m_app.User.CurrentDeptId + "')) order by ordername ");
                }
                return _modelList;
            }
            set
            {
                _modelList = value;
            }
        }
        private DataTable _simpleModelList;
        private DataTable _modelList;

        /// <summary>
        /// 模板列表
        /// </summary>
        public DataTable MyModelList
        {
            get
            {
                if (_mymodelList == null)
                {
                    _mymodelList = m_app.SqlHelper.ExecuteDataTable(string.Format(sqlMyTemplates, m_app.User.Id));
                }
                return _mymodelList;
            }
            set
            {
                _mymodelList = value;
            }
        }
        private DataTable _mymodelList;

        /// <summary>
        /// 子模板
        /// </summary>
        public DataTable ItemModelList
        {
            get
            {
                if (_itemModelList == null)
                {
                    //_itemModelList = m_app.SqlHelper.ExecuteDataTable(" select  MR_CLASS,MR_CODE,MR_NAME,FILE_NAME,PATH,QC_CODE,MR_ATTR,DEPT_ID,CREATOR_ID,CREATE_DATE_TIME,LAST_TIME,CONTENT_CODE,PERMISSION,VISIBLED,INPUT,HOSPITAL_CODE from EMR_ITEM_INDEX   order by mr_code asc");
                    string sql = @"  select MR_CLASS,
                                            MR_CODE,
                                            MR_NAME,
                                            QC_CODE,
                                            MR_ATTR,
                                            DEPT_ID,
                                            CREATOR_ID,
                                            CREATE_DATE_TIME,
                                            LAST_TIME,
                                            CONTENT_CODE,
                                            PERMISSION,
                                            VISIBLED,
                                            INPUT,
                                            HOSPITAL_CODE
                                        from emrtemplet_item
                                        order by MR_NAME asc";
                    _itemModelList = m_app.SqlHelper.ExecuteDataTable(sql);
                }
                return _itemModelList;
            }
        }
        private DataTable _itemModelList;

        /// <summary>
        /// 嵌入模板容器
        /// </summary>
        public Dictionary<string, string> ItemCatalog
        {
            get
            {
                if (_itemCatalog == null)
                {
                    _itemCatalog = new Dictionary<string, string>();
                    _itemCatalog.Add("关键词", "BA");
                    _itemCatalog.Add("专科检查", "BB");
                    _itemCatalog.Add("体征库", "BC");
                    _itemCatalog.Add("高级元素", "BD");
                    _itemCatalog.Add("专科主诉", "BE");
                    _itemCatalog.Add("专科现病史", "BF");
                    _itemCatalog.Add("专科既往病史", "BG");
                }
                return _itemCatalog;
            }
        }
        private Dictionary<string, string> _itemCatalog;

        private DataTable _itemCatalogTable;
        /// <summary>
        /// 嵌入模板容器(返回表格)
        /// </summary>
        public DataTable ItemCatalogTable
        {
            get
            {
                if (_itemCatalogTable == null)
                {
                    _itemCatalogTable = new DataTable();
                    DataColumn dcID = new DataColumn("ID", Type.GetType("System.String"));
                    DataColumn dcName = new DataColumn("NAME", Type.GetType("System.String"));

                    _itemCatalogTable.Columns.Add(dcID);
                    _itemCatalogTable.Columns.Add(dcName);

                    DataRow newrow = _itemCatalogTable.NewRow();
                    newrow["ID"] = "BA";
                    newrow["NAME"] = "关键词";
                    _itemCatalogTable.Rows.Add(newrow);

                    newrow = _itemCatalogTable.NewRow();
                    newrow["ID"] = "BB";
                    newrow["NAME"] = "专科检查";
                    _itemCatalogTable.Rows.Add(newrow);

                    newrow = _itemCatalogTable.NewRow();
                    newrow["ID"] = "BC";
                    newrow["NAME"] = "体征库";
                    _itemCatalogTable.Rows.Add(newrow);

                    newrow = _itemCatalogTable.NewRow();
                    newrow["ID"] = "BD";
                    newrow["NAME"] = "高级元素";
                    _itemCatalogTable.Rows.Add(newrow);

                    newrow = _itemCatalogTable.NewRow();
                    newrow["ID"] = "BE";
                    newrow["NAME"] = "专科主诉";
                    _itemCatalogTable.Rows.Add(newrow);

                    newrow = _itemCatalogTable.NewRow();
                    newrow["ID"] = "BF";
                    newrow["NAME"] = "专科现病史";
                    _itemCatalogTable.Rows.Add(newrow);

                    newrow = _itemCatalogTable.NewRow();
                    newrow["ID"] = "BG";
                    newrow["NAME"] = "专科既往病史";
                    _itemCatalogTable.Rows.Add(newrow);
                }
                return _itemCatalogTable;
            }
        }

        /// <summary>
        /// 根据科室代码，获取当前科室能够使用的字模板（加载子模板元素时候使用）
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public DataTable Get_Templet_Item(string deptId)
        {
            DataTable dt = new DataTable();

            //            string sql = string.Format(@" select *
            //                                               from emrtemplet_item
            //                                              where (mr_class = 'BC' or mr_class = 'BA')
            //                                                and Visibled = 1
            //                                                and hospital_code = '42504942400'
            //                                                and (DEPT_ID = '0401' or
            //                                                    DEPT_ID = SUBSTR('0401', 0, LENGTH('0401') - 2) || '99' or
            //                                                    DEPT_ID = '*')
            //                                              order by mr_code asc");
            string sql = string.Format(@"select * from emrtemplet_item where (DEPT_ID = '{0}' or  DEPT_ID = '*') order by mr_name ", deptId);

            dt = m_app.SqlHelper.ExecuteDataTable(sql);

            return dt;
        }

        /// <summary>
        /// 得到配置信息 2012-02-16 wwj
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetConfigValueByKey(string key)
        {
            string sql1 = " select * from appcfg where configkey = '" + key + "'; ";
            DataTable dt = m_app.SqlHelper.ExecuteDataTable(sql1, CommandType.Text);
            string config = string.Empty;
            if (dt.Rows.Count > 0)
            {
                config = dt.Rows[0]["value"].ToString();
            }
            return config;
        }
        /// <summary>
        /// 设置编辑器的页面设置，APPCFG中PageSetting参数 by ukey 2018-10-14
        /// </summary>
        /// <param name="kind">纸张类型</param>
        /// <param name="pagersize">纸张大小</param>
        /// <param name="margins">页边距</param>
        public void SetPageSetting(System.Drawing.Printing.PaperKind kind, System.Drawing.Printing.PaperSize pagersize, System.Drawing.Printing.Margins margins)
        {
            string config = GetConfigValueByKey("PageSetting");
            System.Xml.XmlDocument doc = new XmlDocument();
            doc.LoadXml(config);
            XmlNode pagesetting = doc.ChildNodes[0].SelectSingleNode("pagesettings");
            XmlElement ele = (pagesetting as XmlElement).SelectSingleNode("page") as XmlElement;

            ele.SetAttribute("kind", kind.ToString());
            ele.SetAttribute("width", pagersize.Width.ToString());
            ele.SetAttribute("height", pagersize.Height.ToString());
            ele = (pagesetting as XmlElement).SelectSingleNode("margins") as XmlElement;
            ele.SetAttribute("left", margins.Left.ToString());
            ele.SetAttribute("top", margins.Top.ToString());
            ele.SetAttribute("right", margins.Right.ToString());
            ele.SetAttribute("bottom", margins.Bottom.ToString());
            config = doc.InnerXml;
            string sql = " update appcfg set value = '{0}' where configkey = 'PageSetting'; ";
            m_app.SqlHelper.ExecuteNoneQuery(string.Format(sql, config), CommandType.Text);
        }

        /// <summary>
        /// 设置页眉高度和页脚高度 2012-02-16 wwj
        /// </summary>
        /// <param name="headerHeight"></param>
        /// <param name="footerHeight"></param>
        public void SetHeaderFooterHeight(int headerHeight, int footerHeight)
        {
            string config = GetConfigValueByKey("PageSetting");

            System.Xml.XmlDocument doc = new XmlDocument();
            doc.LoadXml(config);

            XmlNode pagesetting = doc.ChildNodes[0].SelectSingleNode("pagesettings");

            XmlElement ele = (pagesetting as XmlElement).SelectSingleNode("header") as XmlElement;
            ele.SetAttribute("height", headerHeight.ToString());
            ele = (pagesetting as XmlElement).SelectSingleNode("footer") as XmlElement;
            ele.SetAttribute("height", footerHeight.ToString());
            config = doc.InnerXml;
            string sql = " update appcfg set value = '{0}' where configkey = 'PageSetting'; ";
            m_app.SqlHelper.ExecuteNoneQuery(string.Format(sql, config), CommandType.Text);
        }


        public void ReloadMyTemplate()
        {
            _mymodelList = m_app.SqlHelper.ExecuteDataTable(string.Format(sqlMyTemplates, m_app.User.Id));
        }
    }
}
