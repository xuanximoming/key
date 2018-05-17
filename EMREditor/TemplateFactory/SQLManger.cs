using System;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using DrectSoft.Service;
using DrectSoft.Core;
using DrectSoft.FrameWork.WinForm.Plugin;
#pragma warning disable 0618
namespace DrectSoft.Emr.TemplateFactory
{
    class SQLManger
    {
        IEmrHost m_app;

        public SQLManger(IEmrHost host)
        {
            m_app = host;
        }

        #region 操作模板数据
        /// <summary>
        /// 保存模板数据，返回新的模板编号
        /// </summary>
        /// <param name="emrtemplet"></param>
        /// <param name="editType">1:新增   2：修改</param>
        /// <returns></returns>
        private string SaveTemplet(Emrtemplet emrtemplet, string editType, string type)
        {
            #region OLD处理方法
            try
            {
                SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@EditType",SqlDbType.VarChar),
                    new SqlParameter("@TEMPLET_ID",SqlDbType.VarChar),
                    new SqlParameter("@FILE_NAME",SqlDbType.VarChar),
                    new SqlParameter("@DEPT_ID",SqlDbType.VarChar),
                    new SqlParameter("@CREATOR_ID",SqlDbType.VarChar),

                    new SqlParameter("@CREATE_DATETIME",SqlDbType.VarChar),
                    new SqlParameter("@LAST_TIME",SqlDbType.VarChar),
                    new SqlParameter("@PERMISSION",SqlDbType.VarChar),
                    new SqlParameter("@MR_CLASS",SqlDbType.VarChar),
                    new SqlParameter("@MR_CODE",SqlDbType.VarChar),


                    new SqlParameter("@MR_NAME",SqlDbType.VarChar),
                    new SqlParameter("@MR_ATTR",SqlDbType.VarChar),
                    new SqlParameter("@QC_CODE",SqlDbType.VarChar),
                    new SqlParameter("@NEW_PAGE_FLAG",SqlDbType.Int),
                    new SqlParameter("@FILE_FLAG",SqlDbType.Int),

                    new SqlParameter("@WRITE_TIMES",SqlDbType.Int),
                    new SqlParameter("@CODE",SqlDbType.VarChar),
                    new SqlParameter("@HOSPITAL_CODE",SqlDbType.VarChar),
                    new SqlParameter("@XML_DOC_NEW",SqlDbType.NText,int.MaxValue),
                     //new SqlParameter("@XML_DOC_NEW",SqlDbType.Text),
                    //add by wwj
                    new SqlParameter("@PY",SqlDbType.VarChar),
                    new SqlParameter("@WB",SqlDbType.VarChar),
                    new SqlParameter("@ISSHOWFILENAME",SqlDbType.VarChar),
                    new SqlParameter("@ISFIRSTDAILY",SqlDbType.VarChar),
                    new SqlParameter("@ISYIHUANGOUTONG",SqlDbType.VarChar),

                    new SqlParameter("@NEW_PAGE_END",SqlDbType.Int),
                    new SqlParameter("@state",SqlDbType.VarChar),
                    new SqlParameter("@auditor",SqlDbType.VarChar),


                      //add by ywk  页面配置
                     new SqlParameter("@ISCONFIGPAGESIZE",SqlDbType.VarChar)
                };

                sqlParam[0].Value = editType;
                sqlParam[1].Value = emrtemplet.TempletId;
                sqlParam[2].Value = emrtemplet.FileName;
                sqlParam[3].Value = emrtemplet.DeptId;
                sqlParam[4].Value = emrtemplet.CreatorId;

                sqlParam[5].Value = emrtemplet.CreateDatetime;
                sqlParam[6].Value = emrtemplet.LastTime;
                sqlParam[7].Value = emrtemplet.Permission;
                sqlParam[8].Value = emrtemplet.MrClass;
                sqlParam[9].Value = emrtemplet.MrCode;

                sqlParam[10].Value = emrtemplet.MrName;
                sqlParam[11].Value = emrtemplet.MrAttr;
                sqlParam[12].Value = emrtemplet.QcCode;
                sqlParam[13].Value = emrtemplet.NewPageFlag;
                sqlParam[14].Value = emrtemplet.FileFlag;

                sqlParam[15].Value = emrtemplet.WriteTimes;
                sqlParam[16].Value = emrtemplet.Code;
                sqlParam[17].Value = emrtemplet.HospitalCode;

                //批量更新时无需去除savelog日志信息
                string M_Content = string.Empty;//声明用于存放模板内容的变量 add by ywk 2012年9月13日 10:10:10
                sqlParam[18].Value = M_Content;//先给空值

                //if (type == "batch")
                //    sqlParam[18].Value = emrtemplet.ZipXML_DOC_NEW;
                //else
                //    sqlParam[18].Value = emrtemplet.ZipXML_DOC_NEW_Clear;

                //sqlParam[18]的值先转换为流保存 add by ywk 
                //string imagecontent = sqlParam[18].Value.ToString();
                //byte[] array = Encoding.ASCII.GetBytes(imagecontent);
                //MemoryStream stream = new MemoryStream(array);  //convert stream 2 string      
                //StreamReader reader = new StreamReader(stream);
                //sqlParam[18].Value = reader.ReadToEnd();


                ////根据实际需求将模板中的savelogs内容清除后保存到数据库中
                //RePlaceTempletSaveLog replacesavelog = new RePlaceTempletSaveLog();
                //sqlParam[18].Value = replacesavelog.ClearTempletSaveLog(emrtemplet.ZipXML_DOC_NEW;

                //add yxy 根据妇科提出的问题，当模板名称过长时无法根据病种名称用拼音检索出结果，现实现功能为第一个“--”前的汉字拼音码去除
                string[] code = GetPYWB(emrtemplet.MrName.Substring(emrtemplet.MrName.IndexOf('-') + 1));
                sqlParam[19].Value = code[0];
                sqlParam[20].Value = code[1];

                sqlParam[21].Value = emrtemplet.IsShowDailyTitle;
                sqlParam[22].Value = emrtemplet.IsFirstDailyEmr;
                sqlParam[23].Value = emrtemplet.IsYiHuanGouTong;
                sqlParam[24].Value = emrtemplet.NEW_PAGE_END;

                sqlParam[25].Value = emrtemplet.State;
                sqlParam[26].Value = emrtemplet.Auditor;

                sqlParam[27].Value = emrtemplet.IsPageConfigSize;
                string ReturnValue = string.Empty;//用于返回的值 
                ReturnValue = m_app.SqlHelper.ExecuteDataSet("EmrTempletFactory.usp_EditEmrTemplet", sqlParam, CommandType.StoredProcedure).Tables[0].Rows[0][0].ToString();

                OperTempletConten(type, ReturnValue, emrtemplet);//上面的模板内容先给空值，然后再调用此方法处理模板内容 ..add by ywk 
                return ReturnValue;

                //return m_app.SqlHelper.ExecuteDataSet("EmrTempletFactory.usp_EditEmrTemplet", sqlParam, CommandType.StoredProcedure).Tables[0].Rows[0][0].ToString();
            #endregion

                #region NEW处理方法
                //add by ywk 2012年9月11日 14:41:53
                //            OracleConnection conn = new OracleConnection(m_app.SqlHelper.GetDbConnection().ConnectionString);
                //            OracleCommand cmd = conn.CreateCommand();
                //            try
                //            {
                //                conn.Open();
                //                cmd.Connection = conn;
                //                cmd.CommandType = CommandType.StoredProcedure;
                //                cmd.CommandText = "EmrTempletFactory.usp_EditEmrTemplet";

                //                cmd.Parameters.Add("EditType", OracleType.VarChar).Direction = ParameterDirection.Input;
                //                cmd.Parameters["EditType"].Value = editType;
                //                cmd.Parameters.Add("TEMPLET_ID", OracleType.VarChar).Direction = ParameterDirection.Input;
                //                cmd.Parameters["TEMPLET_ID"].Value = emrtemplet.TempletId;
                //                cmd.Parameters.Add("FILE_NAME", OracleType.VarChar).Direction = ParameterDirection.Input;
                //                cmd.Parameters["FILE_NAME"].Value = emrtemplet.FileName;
                //                cmd.Parameters.Add("DEPT_ID", OracleType.VarChar).Direction = ParameterDirection.Input;
                //                cmd.Parameters["DEPT_ID"].Value = emrtemplet.DeptId;
                //                cmd.Parameters.Add("CREATOR_ID", OracleType.VarChar).Direction = ParameterDirection.Input;
                //                cmd.Parameters["CREATOR_ID"].Value = emrtemplet.CreatorId;
                //                cmd.Parameters.Add("CREATE_DATETIME", OracleType.VarChar).Direction = ParameterDirection.Input;
                //                cmd.Parameters["CREATE_DATETIME"].Value = emrtemplet.CreateDatetime;
                //                cmd.Parameters.Add("LAST_TIME", OracleType.VarChar).Direction = ParameterDirection.Input;
                //                cmd.Parameters["LAST_TIME"].Value = emrtemplet.LastTime;
                //                cmd.Parameters.Add("PERMISSION", OracleType.VarChar).Direction = ParameterDirection.Input;
                //                cmd.Parameters["PERMISSION"].Value = emrtemplet.Permission;
                //                cmd.Parameters.Add("MR_CLASS", OracleType.VarChar).Direction = ParameterDirection.Input;
                //                cmd.Parameters["MR_CLASS"].Value = emrtemplet.MrClass;
                //                cmd.Parameters.Add("MR_CODE", OracleType.VarChar).Direction = ParameterDirection.Input;
                //                cmd.Parameters["MR_CODE"].Value = emrtemplet.MrCode;
                //                cmd.Parameters.Add("MR_NAME", OracleType.VarChar).Direction = ParameterDirection.Input;
                //                cmd.Parameters["MR_NAME"].Value = emrtemplet.MrName;
                //                cmd.Parameters.Add("MR_ATTR", OracleType.VarChar).Direction = ParameterDirection.Input;
                //                cmd.Parameters["MR_ATTR"].Value = emrtemplet.MrAttr;
                //                cmd.Parameters.Add("QC_CODE", OracleType.VarChar).Direction = ParameterDirection.Input;
                //                cmd.Parameters["QC_CODE"].Value = emrtemplet.QcCode;
                //                cmd.Parameters.Add("NEW_PAGE_FLAG", OracleType.Int32).Direction = ParameterDirection.Input;
                //                cmd.Parameters["NEW_PAGE_FLAG"].Value = emrtemplet.NewPageFlag;
                //                cmd.Parameters.Add("FILE_FLAG", OracleType.Int32).Direction = ParameterDirection.Input;
                //                cmd.Parameters["FILE_FLAG"].Value = emrtemplet.FileFlag;
                //                cmd.Parameters.Add("WRITE_TIMES", OracleType.Int32).Direction = ParameterDirection.Input;
                //                cmd.Parameters["WRITE_TIMES"].Value = emrtemplet.WriteTimes;
                //                cmd.Parameters.Add("CODE", OracleType.VarChar).Direction = ParameterDirection.Input;
                //                cmd.Parameters["CODE"].Value = emrtemplet.Code;
                //                cmd.Parameters.Add("HOSPITAL_CODE", OracleType.VarChar).Direction = ParameterDirection.Input;
                //                cmd.Parameters["HOSPITAL_CODE"].Value = emrtemplet.HospitalCode;
                //                cmd.Parameters.Add("XML_DOC_NEW", OracleType.Clob).Direction = ParameterDirection.Input;
                //                if (type == "batch")
                //                    cmd.Parameters["XML_DOC_NEW"].Value = emrtemplet.ZipXML_DOC_NEW;
                //                else
                //                    cmd.Parameters["XML_DOC_NEW"].Value = emrtemplet.ZipXML_DOC_NEW_Clear;
                //                string[] code = GetPYWB(emrtemplet.MrName.Substring(emrtemplet.MrName.IndexOf('-') + 1));
                //                cmd.Parameters.Add("PY", OracleType.VarChar).Direction = ParameterDirection.Input;
                //                cmd.Parameters["PY"].Value = code[0];
                //                cmd.Parameters.Add("WB", OracleType.VarChar).Direction = ParameterDirection.Input;
                //                cmd.Parameters["WB"].Value = code[1];

                //                cmd.Parameters.Add("ISSHOWFILENAME", OracleType.VarChar).Direction = ParameterDirection.Input;
                //                cmd.Parameters["ISSHOWFILENAME"].Value = emrtemplet.IsShowDailyTitle;
                //                cmd.Parameters.Add("ISFIRSTDAILY", OracleType.VarChar).Direction = ParameterDirection.Input;
                //                cmd.Parameters["ISFIRSTDAILY"].Value = emrtemplet.IsFirstDailyEmr;
                //                cmd.Parameters.Add("ISYIHUANGOUTONG", OracleType.VarChar).Direction = ParameterDirection.Input;
                //                cmd.Parameters["ISYIHUANGOUTONG"].Value = emrtemplet.IsYiHuanGouTong;
                //                cmd.Parameters.Add("NEW_PAGE_END", OracleType.Int32).Direction = ParameterDirection.Input;
                //                cmd.Parameters["NEW_PAGE_END"].Value = emrtemplet.NEW_PAGE_END;
                //                cmd.Parameters.Add("state", OracleType.VarChar).Direction = ParameterDirection.Input;
                //                cmd.Parameters["state"].Value = emrtemplet.State;
                //                cmd.Parameters.Add("auditor", OracleType.VarChar).Direction = ParameterDirection.Input;
                //                cmd.Parameters["auditor"].Value = emrtemplet.Auditor;
                //                cmd.Parameters.Add("ISCONFIGPAGESIZE", OracleType.VarChar).Direction = ParameterDirection.Input;
                //                cmd.Parameters["ISCONFIGPAGESIZE"].Value = emrtemplet.IsPageConfigSize;

                //                cmd.Parameters.Add(new OracleParameter("empcurtyp", OracleType.Cursor)).Direction =
                //ParameterDirection.Output;

                //                #region 注释掉
                //                //                cmd.CommandText = string.Format(@"insert into EMRTEMPLET
                //                //                                        (TEMPLET_ID,
                //                //                                         FILE_NAME,
                //                //                                         DEPT_ID,
                //                //                                         CREATOR_ID,
                //                //                                         CREATE_DATETIME,
                //                //                                         LAST_TIME,
                //                //                                         PERMISSION,
                //                //                                         MR_CLASS,
                //                //                                         MR_CODE,
                //                //                                         MR_NAME,
                //                //                                         MR_ATTR,
                //                //                                         QC_CODE,
                //                //                                         NEW_PAGE_FLAG,
                //                //                                         FILE_FLAG,
                //                //                                         WRITE_TIMES,
                //                //                                         CODE,
                //                //                                         HOSPITAL_CODE,
                //                //                                         XML_DOC_NEW,
                //                //                                         PY,
                //                //                                         WB,
                //                //                                         isshowfilename,
                //                //                                         ISFIRSTDAILY,
                //                //                                         ISYIHUANGOUTONG,
                //                //                                         NEW_PAGE_END,
                //                //                                         Valid,
                //                //                                         state,
                //                //                                         auditor,
                //                //                                         isconfigpagesize,
                //                //                                         auditdate
                //                //                                         )
                //                //                                      values
                //                //                                        (:tempplet,
                //                //                                         '{1}',
                //                //                                         '{2}',
                //                //                                         '{3}',
                //                //                                         to_char(sysdate, 'yyyy-mm-dd hh24:mi:ss'),
                //                //                                         to_char(sysdate, 'yyyy-mm-dd hh24:mi:ss'),
                //                //                                         '{4}',
                //                //                                         '{5}',
                //                //                                         '{6}',
                //                //                                         '{7}',
                //                //                                         '{8}',
                //                //                                         '{9}',
                //                //                                         '{10}',
                //                //                                         '{11}',
                //                //                                         '{12}',
                //                //                                         '{13}',
                //                //                                         '{14}',
                //                //                                         '{15}',
                //                //                                         '{16}',
                //                //                                         '{17}',
                //                //                                         '{18}',
                //                //                                         '{19}',
                //                //                                         '{20}',
                //                //                                         '{21}',
                //                //                                         1,
                //                //                                         '{22}',
                //                //                                         '{23}',
                //                //                                         '{24}',
                //                //                                         (case when  v_auditor is not null then to_char(sysdate,'yyyy-mm-dd HH24:mi:ss') else '' end)
                //                //                                         )", emrtemplet.TempletId, emrtemplet.FileName, emrtemplet.DeptId, emrtemplet.CreatorId, emrtemplet.CreateDatetime, emrtemplet.LastTime, emrtemplet.Permission
                //                //                           , emrtemplet.MrClass, emrtemplet.MrCode, emrtemplet.MrName, emrtemplet.MrAttr, emrtemplet.QcCode, emrtemplet.NewPageFlag, emrtemplet.FileFlag
                //                //                           , emrtemplet.WriteTimes, emrtemplet.Code, emrtemplet.HospitalCode, emrtemplet.ZipXML_DOC_NEW_Clear, code[0], code[1]
                //                //                           , emrtemplet.IsShowDailyTitle, emrtemplet.IsFirstDailyEmr, emrtemplet.IsYiHuanGouTong, emrtemplet.NEW_PAGE_END, emrtemplet.State
                //                //                           , emrtemplet.Auditor, emrtemplet.IsPageConfigSize);

                //                #endregion

                //                OracleDataAdapter da = new OracleDataAdapter(cmd);
                //                DataSet ds = new DataSet();
                //                da.Fill(ds);
                //                return ds.Tables[0].Rows[0][0].ToString();
                #endregion

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //cmd.Dispose();
                //conn.Close();
                //conn.Dispose();
            }
        }
        /// <summary>
        /// 处理的模板内容
        /// addby ywk 2012年9月13日 10:13:10
        /// </summary>
        /// <param name="type"></param>
        private void OperTempletConten(string type, string returntemid, Emrtemplet emrtemplet)
        {
            OracleConnection conn = new OracleConnection(m_app.SqlHelper.GetDbConnection().ConnectionString);
            OracleCommand cmd = conn.CreateCommand();
            conn.Open();
            cmd.CommandText = "update  emrtemplet set XML_DOC_NEW = :data where TEMPLET_ID =" + returntemid;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Clear();
            OracleParameter paraClob = new OracleParameter("data", OracleType.Clob);
            if (type == "batch")
                paraClob.Value = emrtemplet.ZipXML_DOC_NEW;
            else
                paraClob.Value = emrtemplet.ZipXML_DOC_NEW_Clear;
            //paraClob.Value = emrContent;
            cmd.Parameters.Add(paraClob);
            cmd.ExecuteNonQuery();
        }

        public void SaveTemplateContent(string identity, Emrtemplet emrtemplet, string type)
        {
            if (!string.IsNullOrEmpty(identity))
            {
                {
                    OracleConnection conn = new OracleConnection(m_app.SqlHelper.GetDbConnection().ConnectionString);
                    OracleCommand cmd = conn.CreateCommand();
                    try
                    {
                        conn.Open();
                        cmd.CommandText = "update emrtemplet set xml_doc_new = :data1 where templet_id = " + identity;
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Clear();

                        OracleParameter paraClob1 = new OracleParameter("data1", OracleType.Clob);

                        //批量更新时无需去除savelog日志信息
                        if (type == "batch")
                            paraClob1.Value = emrtemplet.ZipXML_DOC_NEW;
                        else
                            paraClob1.Value = emrtemplet.ZipXML_DOC_NEW_Clear;

                        cmd.Parameters.Add(paraClob1);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        cmd.Dispose();
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// 保存模板 批量替换模块中使用
        /// </summary>
        /// <param name="emrtemplet"></param>
        /// <param name="editType"></param>
        /// <returns></returns>
        public string BatchSaveTemplet(Emrtemplet emrtemplet, string editType)
        {
            return SaveTemplet(emrtemplet, editType, "batch");

        }

        /// <summary>
        /// 保存模板数据，返回新的模板编号
        /// </summary>
        /// <param name="emrtemplet"></param>
        /// <param name="editType">1:新增   2：修改</param>
        /// <returns></returns>
        public string SaveTemplet(Emrtemplet emrtemplet, string editType)
        {
            return SaveTemplet(emrtemplet, editType, "");
            //m_app.CustomMessageBox.MessageShow("da");
        }

        /// <summary>
        /// 得到拼音和五笔
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string[] GetPYWB(string name)
        {
            GenerateShortCode shortCode = new GenerateShortCode(m_app.SqlHelper);
            string[] code = shortCode.GenerateStringShortCode(name);

            //string py = code[0]; //PY
            //string wb = code[1]; //WB
            return code;
        }

        /// <summary>
        /// 根据模板ID获取模板信息
        /// </summary>
        /// <param name="templet_id"></param>
        public Emrtemplet GetTemplet(string templet_id)
        {
            Emrtemplet emrtemplet = new Emrtemplet();

            DataTable dt = GetTemplet_Table(templet_id);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                emrtemplet.TempletId = dr["Templet_Id"].ToString();
                emrtemplet.FileName = dr["File_Name"].ToString();
                emrtemplet.DeptId = dr["Dept_Id"].ToString();
                emrtemplet.CreatorId = dr["Creator_Id"].ToString();
                emrtemplet.CreateDatetime = dr["Create_Datetime"].ToString();
                emrtemplet.LastTime = dr["Last_Time"].ToString();
                emrtemplet.Permission = dr["Permission"].ToString();
                emrtemplet.MrClass = dr["Mr_Class"].ToString();
                emrtemplet.MrCode = dr["Mr_Code"].ToString();
                emrtemplet.MrName = dr["Mr_Name"].ToString();
                emrtemplet.MrAttr = dr["Mr_Attr"].ToString();
                emrtemplet.QcCode = dr["Qc_Code"].ToString();
                emrtemplet.NewPageFlag = Convert.ToInt32(dr["New_Page_Flag"].ToString());
                emrtemplet.FileFlag = Convert.ToInt32(dr["File_Flag"].ToString());
                emrtemplet.WriteTimes = Convert.ToInt32(dr["Write_Times"].ToString());
                emrtemplet.Code = dr["Code"].ToString();
                emrtemplet.HospitalCode = dr["Hospital_Code"].ToString();
                emrtemplet.ZipXML_DOC_NEW = dr["XML_DOC_NEW"].ToString();
                emrtemplet.IsShowDailyTitle = dr["isshowfilename"].ToString();
                emrtemplet.IsFirstDailyEmr = dr["ISFIRSTDAILY"].ToString();
                emrtemplet.IsYiHuanGouTong = dr["ISYIHUANGOUTONG"].ToString();
                //emrtemplet.Valid = Convert.ToInt32(dr["valid"].ToString());
                //页面配置
                emrtemplet.IsPageConfigSize = dr["isconfigpagesize"].ToString();

                emrtemplet.State = dr["State"].ToString();
                emrtemplet.Auditor = dr["Auditor"].ToString();
                emrtemplet.AuditDate = dr["AuditDate"].ToString();
            }
            return emrtemplet;
        }


        public Emrtemplet GetMyTemplet(string templet_id)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            {
                new SqlParameter("@EditType",SqlDbType.VarChar),
                new SqlParameter("@CODE",SqlDbType.VarChar),
                new SqlParameter("@CREATEUSER",SqlDbType.VarChar)
            };
            sqlParam[0].Value = "4";
            sqlParam[1].Value = templet_id;
            sqlParam[2].Value = m_app.User.Id;
            DataTable dt = m_app.SqlHelper.ExecuteDataSet("EmrTempletFactory.usp_EditMyEmrTemplet", sqlParam, CommandType.StoredProcedure).Tables[0];

            Emrtemplet emrtemplet = new Emrtemplet();

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                emrtemplet.TempletId = dr["Templet_Id"].ToString();
                emrtemplet.FileName = dr["File_Name"].ToString();
                emrtemplet.DeptId = dr["Dept_Id"].ToString();
                emrtemplet.CreatorId = dr["Creator_Id"].ToString();
                emrtemplet.CreateDatetime = dr["Create_Datetime"].ToString();
                emrtemplet.LastTime = dr["Last_Time"].ToString();
                emrtemplet.Permission = dr["Permission"].ToString();
                emrtemplet.MrClass = dr["Mr_Class"].ToString();
                emrtemplet.MrCode = dr["Mr_Code"].ToString();
                emrtemplet.MrName = dr["Mr_Name"].ToString();
                emrtemplet.MrAttr = dr["Mr_Attr"].ToString();
                emrtemplet.QcCode = dr["Qc_Code"].ToString();
                emrtemplet.NewPageFlag = Convert.ToInt32(dr["New_Page_Flag"].ToString());
                emrtemplet.FileFlag = Convert.ToInt32(dr["File_Flag"].ToString());
                emrtemplet.WriteTimes = Convert.ToInt32(dr["Write_Times"].ToString());
                emrtemplet.Code = dr["Code"].ToString();
                emrtemplet.HospitalCode = dr["Hospital_Code"].ToString();
                emrtemplet.ZipXML_DOC_NEW = dr["XML_DOC_NEW"].ToString();
                emrtemplet.IsShowDailyTitle = dr["isshowfilename"].ToString();
                emrtemplet.IsFirstDailyEmr = dr["ISFIRSTDAILY"].ToString();
                emrtemplet.IsYiHuanGouTong = dr["ISYIHUANGOUTONG"].ToString();
                //emrtemplet.Valid = Convert.ToInt32(dr["valid"].ToString());
                //页面配置
                emrtemplet.IsPageConfigSize = dr["isconfigpagesize"].ToString();

                emrtemplet.State = dr["State"].ToString();
                emrtemplet.Auditor = dr["Auditor"].ToString();
                emrtemplet.AuditDate = dr["AuditDate"].ToString();
            }
            return emrtemplet;
        }

        /// <summary>
        /// 根据模板ID获取模板信息
        /// </summary>
        /// <param name="templet_id"></param>
        public DataTable GetTemplet_Table(string templet_id)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            {
                new SqlParameter("@EditType",SqlDbType.VarChar),
                new SqlParameter("@TEMPLET_ID",SqlDbType.VarChar)
            };
            sqlParam[0].Value = "4";
            sqlParam[1].Value = templet_id;
            DataTable dt = m_app.SqlHelper.ExecuteDataSet("EmrTempletFactory.usp_EditEmrTemplet", sqlParam, CommandType.StoredProcedure).Tables[0];

            return dt;
        }

        /// <summary>
        /// 根据传入的模板实体删除对应模板
        /// </summary>
        /// <param name="emrtemplet"></param>
        /// <returns></returns>
        public bool DelTemplet(Emrtemplet emrtemplet, bool isperson)
        {
            try
            {
                if (isperson)
                {
                    return DelMyTemplet(emrtemplet.TempletId);
                }
                return DelTemplet(emrtemplet.TempletId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool DelMyTemplet(string TempletId)
        {
            try
            {
                if (string.IsNullOrEmpty(TempletId))
                {
                    return false;
                }
                SqlParameter paramtype = new SqlParameter("EditType", SqlDbType.VarChar);
                paramtype.Value = "3";
                SqlParameter paramid = new SqlParameter("CODE", SqlDbType.VarChar);
                paramid.Value = TempletId;
                SqlParameter paramCreater = new SqlParameter("CREATEUSER", SqlDbType.VarChar);
                paramCreater.Value = m_app.User.DoctorId;
                m_app.SqlHelper.ExecuteDataSet("EmrTempletFactory.usp_EditMyEmrTemplet", new SqlParameter[] { paramtype, paramid, paramCreater }, CommandType.StoredProcedure);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据传入的模板实体删除对应模板
        /// </summary>
        /// <param name="emrtemplet"></param>
        /// <returns></returns>
        public bool DelTemplet(string templetID)
        {
            try
            {
                if (string.IsNullOrEmpty(templetID))
                {
                    return false;
                }
                SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@EditType",SqlDbType.VarChar),
                    new SqlParameter("@TEMPLET_ID",SqlDbType.VarChar)
                };
                sqlParam[0].Value = "3";
                sqlParam[1].Value = templetID;
                m_app.SqlHelper.ExecuteDataSet("EmrTempletFactory.usp_EditEmrTemplet", sqlParam, CommandType.StoredProcedure);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 操作子模板数据

        /// <summary>
        /// 保存模板数据，返回新的模板编号
        /// </summary>
        /// <param name="emrtemplet_item">子模板实体</param>
        /// <param name="editType">1:新增   2：修改</param>
        /// <returns></returns>
        public string SaveTemplet_Item(Emrtemplet_Item emrtemplet_item, string editType)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@EditType",SqlDbType.VarChar),
                    new SqlParameter("@MR_CLASS",SqlDbType.VarChar),
                    new SqlParameter("@MR_CODE",SqlDbType.VarChar),
                    new SqlParameter("@MR_NAME",SqlDbType.VarChar),
                    new SqlParameter("@MR_ATTR",SqlDbType.VarChar),

                    new SqlParameter("@QC_CODE",SqlDbType.VarChar),
                    new SqlParameter("@DEPT_ID",SqlDbType.VarChar),
                    new SqlParameter("@CREATOR_ID",SqlDbType.VarChar),
                    new SqlParameter("@CREATE_DATE_TIME",SqlDbType.VarChar),
                    new SqlParameter("@LAST_TIME",SqlDbType.VarChar),

                    new SqlParameter("@CONTENT_CODE",SqlDbType.VarChar),
                    new SqlParameter("@PERMISSION",SqlDbType.Int),
                    new SqlParameter("@VISIBLED",SqlDbType.Int),
                    new SqlParameter("@INPUT",SqlDbType.VarChar),
                    new SqlParameter("@HOSPITAL_CODE",SqlDbType.VarChar),

                    new SqlParameter("@ITEM_DOC_NEW",SqlDbType.Text) 
                };

            sqlParam[0].Value = editType;
            sqlParam[1].Value = emrtemplet_item.MrClass;
            sqlParam[2].Value = emrtemplet_item.MrCode;
            sqlParam[3].Value = emrtemplet_item.MrName;
            sqlParam[4].Value = emrtemplet_item.MrAttr;

            sqlParam[5].Value = emrtemplet_item.QcCode;
            sqlParam[6].Value = emrtemplet_item.DeptId;
            sqlParam[7].Value = emrtemplet_item.CreatorId;
            sqlParam[8].Value = emrtemplet_item.CreateDateTime;
            sqlParam[9].Value = emrtemplet_item.LastTime;

            sqlParam[10].Value = emrtemplet_item.ContentCode;
            sqlParam[11].Value = emrtemplet_item.Permission;
            sqlParam[12].Value = emrtemplet_item.Visibled;
            sqlParam[13].Value = emrtemplet_item.Input;
            sqlParam[14].Value = emrtemplet_item.HospitalCode;

            //sqlParam[15].Value = emrtemplet_item.Zip_ITEM_DOC_NEW;
            sqlParam[15].Value = string.Empty;//先给空值

            // 操作子模板的话，如果拖入图片过多，会报转换类型的错误  edit by ywk 2012年12月13日17:00:12 
            string ReturnValue = string.Empty;//用于返回的值 
            ReturnValue = m_app.SqlHelper.ExecuteDataSet("EmrTempletFactory.usp_EditEmrTemplet_Item", sqlParam, CommandType.StoredProcedure).Tables[0].Rows[0][0].ToString();

            UpdateEmrTemplateItemAttribute(ReturnValue, emrtemplet_item);//上面的模板内容先给空值，然后再调用此方法处理模板内容 ..add by ywk 
            return ReturnValue;


            //return m_app.SqlHelper.ExecuteDataSet("EmrTempletFactory.usp_EditEmrTemplet_Item", sqlParam, CommandType.StoredProcedure).Tables[0].Rows[0][0].ToString();
        }

        /// <summary>
        /// 根据子模板Mr_Class获取子模板信息
        /// </summary>
        /// <param name="templet_id"></param>
        public Emrtemplet_Item GetTemplet_Item(string mr_code)
        {
            Emrtemplet_Item emrtemplet_item = new Emrtemplet_Item();

            DataTable dt = GetTemplet_Item_Table(mr_code);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                emrtemplet_item.MrClass = dr["Mr_Class"].ToString();
                emrtemplet_item.MrCode = dr["Mr_Code"].ToString();
                emrtemplet_item.MrName = dr["Mr_Name"].ToString();
                emrtemplet_item.MrAttr = dr["Mr_Attr"].ToString();

                emrtemplet_item.QcCode = dr["Qc_Code"].ToString();
                emrtemplet_item.DeptId = dr["Dept_Id"].ToString();
                emrtemplet_item.CreatorId = dr["Creator_Id"].ToString();
                emrtemplet_item.CreateDateTime = dr["Create_Date_Time"].ToString();
                emrtemplet_item.LastTime = dr["Last_Time"].ToString();

                emrtemplet_item.ContentCode = dr["Content_Code"].ToString();
                emrtemplet_item.Permission = Convert.ToInt32(dr["Permission"].ToString());
                emrtemplet_item.Visibled = Convert.ToInt32(dr["Visibled"].ToString());
                emrtemplet_item.Input = dr["Input"].ToString();
                emrtemplet_item.HospitalCode = dr["Hospital_Code"].ToString();

                emrtemplet_item.Zip_ITEM_DOC_NEW = dr["ITEM_DOC_NEW"].ToString();
            }
            return emrtemplet_item;
        }

        /// <summary>
        /// 根据子模板Mr_Class获取子模板信息
        /// </summary>
        /// <param name="templet_id"></param>
        public DataTable GetTemplet_Item_Table(string mr_code)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            {
                new SqlParameter("@EditType",SqlDbType.VarChar),
                new SqlParameter("@MR_CODE",SqlDbType.VarChar)
            };
            sqlParam[0].Value = "4";
            sqlParam[1].Value = mr_code;
            DataTable dt = m_app.SqlHelper.ExecuteDataSet("EmrTempletFactory.usp_EditEmrTemplet_Item", sqlParam, CommandType.StoredProcedure).Tables[0];

            return dt;
        }

        /// <summary>
        /// 根据传入的模板实体删除对应模板
        /// </summary>
        /// <param name="emrtemplet"></param>
        /// <returns></returns>
        public bool DelTemplet_Item(Emrtemplet_Item emrtemplet_item)
        {
            return DelTemplet_Item(emrtemplet_item.MrCode);
        }

        /// <summary>
        /// 根据传入的子模板对应编号删除对应子模板
        /// </summary>
        /// <param name="emrtemplet"></param>
        /// <returns></returns>
        public bool DelTemplet_Item(string mr_code)
        {
            try
            {
                SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@EditType",SqlDbType.VarChar),
                    new SqlParameter("@MR_CODE",SqlDbType.VarChar)
                };
                sqlParam[0].Value = "3";
                sqlParam[1].Value = mr_code;
                m_app.SqlHelper.ExecuteDataSet("EmrTempletFactory.usp_EditEmrTemplet_Item", sqlParam, CommandType.StoredProcedure);
            }
            catch
            {
                return false;
            }

            return true;
        }

        #endregion

        #region 操作模板页眉数据

        /// <summary>
        /// 保存模板页眉数据，返回新的页眉编号
        /// </summary>
        /// <param name="emrtemplet_item">子模板实体</param>
        /// <param name="editType">1:新增   2：修改</param>
        /// <returns></returns>
        public string SaveTemplet_Header(EmrTempletHeader emrtemplet_header, string editType)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@EditType",SqlDbType.VarChar),
                    new SqlParameter("@HEADER_ID",SqlDbType.VarChar),
                    new SqlParameter("@NAME",SqlDbType.VarChar),
                    new SqlParameter("@CREATOR_ID",SqlDbType.VarChar),
                    new SqlParameter("@CREATE_DATETIME",SqlDbType.VarChar),

                    new SqlParameter("@LAST_TIME",SqlDbType.VarChar),
                    new SqlParameter("@HOSPITAL_CODE",SqlDbType.VarChar),
                    new SqlParameter("@CONTENT",SqlDbType.Text)
                };

            sqlParam[0].Value = editType;
            sqlParam[1].Value = emrtemplet_header.HeaderId;
            sqlParam[2].Value = emrtemplet_header.Name;
            sqlParam[3].Value = emrtemplet_header.CreatorId;
            sqlParam[4].Value = emrtemplet_header.CreateDatetime;

            sqlParam[5].Value = emrtemplet_header.LastTime;
            sqlParam[6].Value = emrtemplet_header.HospitalCode;
            //sqlParam[7].Value = emrtemplet_header.Content;
            sqlParam[7].Value = string.Empty;//参数先给空值
            //页眉中增加图片报错的BUG  add by ywk 2012年10月25日 11:59:04 
            string ReturnValue = string.Empty;//用于返回的值 
            ReturnValue = m_app.SqlHelper.ExecuteDataSet("EmrTempletFactory.usp_EditEmrTempletHeader", sqlParam, CommandType.StoredProcedure).Tables[0].Rows[0][0].ToString();
            OperTempletHeader(ReturnValue, emrtemplet_header);
            return ReturnValue;

            //return m_app.SqlHelper.ExecuteDataSet("EmrTempletFactory.usp_EditEmrTempletHeader", sqlParam, CommandType.StoredProcedure).Tables[0].Rows[0][0].ToString();
        }

        /// <summary>
        /// 处理的模板的页眉部分
        /// addby ywk2012年10月25日 12:01:17
        /// </summary>
        /// <param name="type"></param>
        private void OperTempletHeader(string returntemid, EmrTempletHeader emrtempletheader)
        {
            OracleConnection conn = new OracleConnection(m_app.SqlHelper.GetDbConnection().ConnectionString);
            OracleCommand cmd = conn.CreateCommand();
            conn.Open();
            cmd.CommandText = "update  EMRTEMPLETHEADER set CONTENT = :data where HEADER_ID =" + returntemid;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Clear();
            OracleParameter paraClob = new OracleParameter("data", OracleType.Clob);
            paraClob.Value = emrtempletheader.Content;
            cmd.Parameters.Add(paraClob);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 根据子模板Mr_Class获取子模板信息
        /// </summary>
        /// <param name="templet_id"></param>
        public EmrTempletHeader GetTemplet_Header(string headerID)
        {
            EmrTempletHeader emrtemplet_header = new EmrTempletHeader();

            DataTable dt = GetTempletHeader_Table(headerID);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                emrtemplet_header.HeaderId = dr["Header_Id"].ToString();
                emrtemplet_header.Name = dr["Name"].ToString();
                emrtemplet_header.CreateDatetime = dr["CREATE_DATETIME"].ToString();
                emrtemplet_header.CreatorId = dr["CREATOR_ID"].ToString();
                emrtemplet_header.LastTime = dr["Last_Time"].ToString();
                emrtemplet_header.HospitalCode = dr["Hospital_Code"].ToString();
                emrtemplet_header.Content = dr["Content"].ToString();
            }
            return emrtemplet_header;
        }

        /// <summary>
        /// 根据子模板Mr_Class获取子模板信息
        /// </summary>
        /// <param name="templet_id"></param>
        public DataTable GetTempletHeader_Table(string headerID)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            {
                new SqlParameter("@EditType",SqlDbType.VarChar),
                new SqlParameter("@HEADER_ID",SqlDbType.VarChar)
            };
            sqlParam[0].Value = "4";
            sqlParam[1].Value = headerID;
            DataTable dt = m_app.SqlHelper.ExecuteDataSet("EmrTempletFactory.usp_EditEmrTempletHeader", sqlParam, CommandType.StoredProcedure).Tables[0];

            return dt;
        }

        /// <summary>
        /// 根据传入的模板页眉实体删除对应模板
        /// </summary>
        /// edit by Yanqiao.Cai 2013-02-26
        /// 1、add try ... catch
        /// <param name="emrtempletheader">页眉对应实体</param>
        /// <returns></returns>
        public bool DelTempletHeader(EmrTempletHeader emrtempletheader)
        {
            try
            {
                return DelTempletHeader(emrtempletheader.HeaderId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据传入的headerid删除对应页眉文件
        /// </summary>
        /// edit by Yanqiao.Cai 2013-02-26
        /// 1、add try ... catch
        /// <param name="emrtemplet"></param>
        /// <returns></returns>
        public bool DelTempletHeader(string headerID)
        {
            try
            {
                //SqlParameter[] sqlParam = new SqlParameter[] 
                //{
                //    new SqlParameter("@EditType",SqlDbType.VarChar),
                //    new SqlParameter("@HEADER_ID",SqlDbType.VarChar)
                //};
                //sqlParam[0].Value = "3";
                //sqlParam[1].Value = headerID;
                //m_app.SqlHelper.ExecuteDataSet("EmrTempletFactory.usp_EditEmrTempletHeader", sqlParam, CommandType.StoredProcedure);
                return DS_SqlService.DeleteTempletHeader(headerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 操作模板页脚数据

        /// <summary>
        /// 保存模板页脚数据，返回新的页脚编号
        /// </summary>
        /// <param name="emrtemplet_item">页脚实体</param>
        /// <param name="editType">1:新增   2：修改</param>
        /// <returns></returns>
        public string SaveTemplet_Foot(EmrTemplet_Foot emrtemplet_foot, string editType)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@EditType",SqlDbType.VarChar),
                    new SqlParameter("@FOOT_ID",SqlDbType.VarChar),
                    new SqlParameter("@NAME",SqlDbType.VarChar),
                    new SqlParameter("@CREATOR_ID",SqlDbType.VarChar),
                    new SqlParameter("@CREATE_DATETIME",SqlDbType.VarChar),

                    new SqlParameter("@LAST_TIME",SqlDbType.VarChar),
                    new SqlParameter("@HOSPITAL_CODE",SqlDbType.VarChar),
                    new SqlParameter("@CONTENT",SqlDbType.Text)
                };

            sqlParam[0].Value = editType;
            sqlParam[1].Value = emrtemplet_foot.FootId;
            sqlParam[2].Value = emrtemplet_foot.Name;
            sqlParam[3].Value = emrtemplet_foot.CreatorId;
            sqlParam[4].Value = emrtemplet_foot.CreateDatetime;

            sqlParam[5].Value = emrtemplet_foot.LastTime;
            sqlParam[6].Value = emrtemplet_foot.HospitalCode;
            sqlParam[7].Value = emrtemplet_foot.Content;

            return m_app.SqlHelper.ExecuteDataSet("EmrTempletFactory.usp_EditEmrTemplet_Foot", sqlParam, CommandType.StoredProcedure).Tables[0].Rows[0][0].ToString();
        }

        /// <summary>
        /// 根据页脚foot_id获取子模板信息
        /// </summary>
        /// <param name="templet_id"></param>
        public EmrTemplet_Foot GetTemplet_Foot(string foot_id)
        {
            EmrTemplet_Foot emrtemplet_foot = new EmrTemplet_Foot();

            DataTable dt = GetTempletFoot_Table(foot_id);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                emrtemplet_foot.FootId = dr["Foot_Id"].ToString();
                emrtemplet_foot.Name = dr["Name"].ToString();
                emrtemplet_foot.CreateDatetime = dr["CREATE_DATETIME"].ToString();
                emrtemplet_foot.CreatorId = dr["CREATOR_ID"].ToString();
                emrtemplet_foot.LastTime = dr["Last_Time"].ToString();
                emrtemplet_foot.HospitalCode = dr["Hospital_Code"].ToString();
                emrtemplet_foot.Content = dr["Content"].ToString();
            }
            return emrtemplet_foot;
        }

        /// <summary>
        /// 根据子模板Mr_Class获取子模板信息
        /// </summary>
        /// <param name="templet_id"></param>
        public DataTable GetTempletFoot_Table(string headerID)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            {
                new SqlParameter("@EditType",SqlDbType.VarChar),
                new SqlParameter("@Foot_ID",SqlDbType.VarChar)
            };
            sqlParam[0].Value = "4";
            sqlParam[1].Value = headerID;
            DataTable dt = m_app.SqlHelper.ExecuteDataSet("EmrTempletFactory.usp_EditEmrTemplet_Foot", sqlParam, CommandType.StoredProcedure).Tables[0];

            return dt;
        }

        /// <summary>
        /// 根据传入的模板页脚实体删除对应模板
        /// </summary>
        /// edit by Yanqiao.Cai 2013-02-26
        /// 1、add try ... catch
        /// <param name="emrtempletheader">页脚对应实体</param>
        /// <returns></returns>
        public bool DelTempletFoot(EmrTemplet_Foot emrtempletfoot)
        {
            try
            {
                return DelTempletFoot(emrtempletfoot.FootId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据传入的foot_id删除对应页脚文件
        /// </summary>
        /// edit by Yanqiao.Cai 2013-02-26
        /// 1、add try ... catch
        /// 2、修复删除功能
        /// <param name="emrtemplet"></param>
        /// <returns></returns>
        public bool DelTempletFoot(string foot_id)
        {
            try
            {
                //SqlParameter[] sqlParam = new SqlParameter[] 
                //{
                //    new SqlParameter("@EditType",SqlDbType.VarChar),
                //    new SqlParameter("@Foot_ID",SqlDbType.VarChar)
                //};
                //sqlParam[0].Value = "3";
                //sqlParam[1].Value = foot_id;
                //m_app.SqlHelper.ExecuteDataSet("EmrTempletFactory.usp_EditEmrTemplet_Foot", sqlParam, CommandType.StoredProcedure);
                return DS_SqlService.DeleteTempletFooter(foot_id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        /// <summary>
        /// 电子病历默认设置
        /// </summary>
        /// <param name="emrDefaultSet"></param>
        public void SetEmrDefault(string emrDefaultSet)
        {
            string sql = "update appcfg set appcfg.value = '{0}' where appcfg.configkey = 'EmrDefaultSet'";
            m_app.SqlHelper.ExecuteNoneQuery(string.Format(sql, emrDefaultSet), CommandType.Text);
        }

        /// <summary>
        /// 修改模板
        /// </summary>
        /// <param name="emrTemplate"></param>
        public void UpdateEmrTemplateAttribute(Emrtemplet emrTemplate)
        {
            //模板工厂新增页面配置字段 add by  ywk 2012年3月31日9:40:02 
            string sql = " update emrtemplet set mr_name = '{0}', qc_code = '{1}', new_page_flag = '{2}', isfirstdaily = '{3}', " +
                " isshowfilename = '{5}', file_name = '{6}', isyihuangoutong = '{7}' ,NEW_PAGE_END  = '{8}' , IsConfigpagesize='{9}' " +
                " where emrtemplet.templet_id = '{4}' ";
            m_app.SqlHelper.ExecuteNoneQuery(
                string.Format(sql, emrTemplate.MrName, emrTemplate.QcCode, emrTemplate.NewPageFlag,
                    emrTemplate.IsFirstDailyEmr, emrTemplate.TempletId, emrTemplate.IsShowDailyTitle,
                    emrTemplate.FileName, emrTemplate.IsYiHuanGouTong, emrTemplate.NEW_PAGE_END, emrTemplate.IsPageConfigSize),
                CommandType.Text);
        }

        /// <summary>
        /// 修改子模板
        /// </summary>
        /// <param name="emrTemplateItem"></param>
        public void UpdateEmrTemplateItemAttribute(string emritemid, Emrtemplet_Item emrTemplateItem)
        {
            //string sql = " update emrtemplet_item set mr_name = '{0}' where emrtemplet_item.mr_code = '{1}' ";
            //m_app.SqlHelper.ExecuteNoneQuery(
            //    string.Format(sql, emrTemplateItem.MrName, emrTemplateItem.MrCode),
            //    CommandType.Text);
            OracleConnection conn = new OracleConnection(m_app.SqlHelper.GetDbConnection().ConnectionString);
            OracleCommand cmd = conn.CreateCommand();
            conn.Open();
            cmd.CommandText = "update  emrtemplet_item set mr_name = :tname , ITEM_DOC_NEW = :data where emrtemplet_item.mr_code ='" + emritemid + "'";

            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Clear();
            OracleParameter paraClob = new OracleParameter("data", OracleType.Clob);
            //if (type == "batch")
            //    paraClob.Value = emrtemplet.ZipXML_DOC_NEW;
            //else
            paraClob.Value = emrTemplateItem.ITEM_DOC_NEW;

            OracleParameter paraName = new OracleParameter("tname", OracleType.Char);
            paraName.Value = emrTemplateItem.MrName;
            //paraClob.Value = emrContent;
            cmd.Parameters.Add(paraClob);
            cmd.Parameters.Add(paraName);
            cmd.ExecuteNonQuery();

        }

        /// <summary>
        /// 修改子模板
        /// </summary>
        /// <param name="emrTemplateItem"></param>
        public void UpdateEmrTemplateItemAttribute(string emritemid, string emr_itemname)
        {
            try
            {
                OracleConnection conn = new OracleConnection(m_app.SqlHelper.GetDbConnection().ConnectionString);
                OracleCommand cmd = conn.CreateCommand();
                conn.Open();
                cmd.CommandText = "update  emrtemplet_item set mr_name = :tname where emrtemplet_item.mr_code ='" + emritemid + "'";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();

                OracleParameter paraName = new OracleParameter("tname", OracleType.Char);
                paraName.Value = emr_itemname;
                cmd.Parameters.Add(paraName);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //获得可替换项
        public DataTable GetReplaceItem()
        {
            string sqlGetReplaceImte = " select distinct a.source_itemname as name from EMR_REPLACE_ITEM a  where a.valid = '1' ";
            return m_app.SqlHelper.ExecuteDataTable(sqlGetReplaceImte, CommandType.Text);
        }

        /// <summary>
        /// 根据工号获取员工角色  以及电子病历中对应的科室编号
        /// </summary>
        /// <param name="usersid"></param>
        /// <returns></returns>
        public DataTable GetJobsByUserID(string usersid)
        {
            string sql = string.Format(@"select a.jobid,b.emr_dept_id deptid from users a 
                                          left join emrdept2his b on a.deptid = b.his_dept_id
                                          where a.jobid is not null and a.valid = 1
                                           and a.id = '{0}'", usersid);

            return m_app.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
        }

        /// <summary>
        /// 根据科室编号获取该科室已填写未申请的模版列表
        /// </summary>
        /// <param name="deptid"></param>
        /// <returns></returns>
        public DataTable GetNoAuditList(string deptid)
        {
            string sql = string.Format(@"select dept.dept_name, u.name, cate.cname, templet.templet_id,templet.mr_name
                                              from emrtemplet templet
                                              left join emrdept dept on templet.dept_id = dept.dept_id
                                              left join users u on templet.creator_id = u.id
                                              left join dict_catalog cate on cate.ccode = templet.mr_class
                                                                         and ctype = '2'
                                                                         and (UTYPE = '3' or UTYPE = '1')
                                                                         and CNAME <> '子女病程记录'
                                             where templet.state = 1
                                               and dept.dept_id = '{0}'
                                             order by dept.dept_id, cate.ccode", deptid);



            return m_app.SqlHelper.ExecuteDataTable(sql);
        }

        /// <summary>
        /// 审核模版
        /// </summary>
        /// <param name="templetid"></param>
        /// <param name="auditor"></param>
        /// <returns></returns>
        public string AuditTemplet(string templetid, string auditor, string state)
        {
            string sql = string.Format(@"update emrtemplet set auditor = '{0}',
                                                auditdate = to_char(sysdate,'yyyy-mm-dd HH24:mi:ss'),
                                                state = '{1}' 
                                                where templet_id = '{2}'", auditor, state, templetid);
            try
            {
                m_app.SqlHelper.ExecuteNoneQuery(sql);
                return "审核成功";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// 根据当前登录人权限 获取可以维护的科室列表
        /// </summary>
        /// <param name="emrtemplet"></param>
        public DataTable GetDeptListByUser()
        {
            string sql = string.Format(@"select a.jobid,b.emr_dept_id deptid from users a 
                                          left join emrdept2his b on a.deptid = b.his_dept_id
                                          where a.jobid is not null and a.valid = 1
                                           and a.id = '{0}'", m_app.User.Id);

            DataTable dt = m_app.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
            string jobid = dt.Rows[0][0].ToString();

            //判断如果登陆人有管理员权限 则全部可以修改
            if (jobid.IndexOf("66") > -1)
            {
                sql = string.Format(@"select distinct DEPT_ID ID,DEPT_NAME NAME,py,wb from EMRDEPT a order by DEPT_NAME");
            }
            //如果有模板管理，主任医生权限 则可以修改当前科室模版
            else //if (jobid.IndexOf("77") > -1 || jobid.IndexOf("11") > -1)
            {
                sql = string.Format(@"select distinct d.DEPT_ID ID,d.DEPT_NAME NAME,d.py,d.wb from users a 
                                          left join emrdept2his b on a.deptid = b.his_dept_id
                                          left join EMRDEPT d on b.emr_dept_id = d.dept_id
                                          where a.jobid is not null and a.valid = 1
                                           and a.id = '{0}' order by d.DEPT_NAME", m_app.User.Id);
            }

            DataTable dtend = m_app.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
            return GetEmrDeptMent(dtend);
        }

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
                    }
                }
            }
            if (!IsOperDt)//如果配置了就把DT数据处理
            {
                newdt = _emrDepartMent.Copy();
                DataRow[] dr = newdt.Select("ID='*' ");
                for (int i = 0; i < dr.Length; i++)
                {
                    newdt.Rows.Remove(dr[i]);
                }
            }
            else
            {
                newdt = _emrDepartMent;
            }
            return newdt;
        }

        /// <summary>
        /// 根据当前登录人权限 获取可以维护的小科室列表
        /// </summary>
        /// <param name="emrtemplet"></param>
        public DataTable GetSubDeptListByUser()
        {
            string sql = string.Format(@"select a.jobid,b.emr_dept_id deptid from users a 
                                          left join emrdept2his b on a.deptid = b.his_dept_id
                                          where a.jobid is not null and a.valid = 1
                                           and a.id = '{0}'", m_app.User.Id);

            DataTable dt = m_app.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
            string jobid = dt.Rows[0][0].ToString();

            //判断如果登陆人有管理员权限 则全部可以修改
            if (jobid.IndexOf("66") > -1)
            {
                sql = string.Format(@" select distinct DEPT_ID ID,DEPT_NAME NAME,py,wb from EMRDEPT where length(DEPT_ID)>2 or DEPT_ID = '*' or DEPT_NAME = '通用科室' order by DEPT_ID ");
            }
            //如果有模板管理，主任医生权限 则可以修改当前科室模版
            else //if (jobid.IndexOf("77") > -1 || jobid.IndexOf("11") > -1)
            {
                sql = string.Format(@"select distinct d.DEPT_ID ID,d.DEPT_NAME NAME,d.py,d.wb from users a 
                                          left join emrdept2his b on a.deptid = b.his_dept_id
                                          left join EMRDEPT d on b.emr_dept_id = d.dept_id
                                          where a.jobid is not null and a.valid = 1 and length(d.DEPT_ID)>2
                                           and a.id = '{0}' or d.DEPT_ID = '*' or d.DEPT_NAME = '通用科室' order by d.DEPT_ID ", m_app.User.Id);
            }
            //else
            //{
            //    sql = string.Format(@"select '' ID,'' NAME,'' py,'' wb from dual;");
            //}
            return m_app.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
        }

        /// <summary>
        /// 获取模版工厂配置信息
        /// </summary>
        /// <returns></returns>
        public string[] GetTempletAuditConfig()
        {
            string[] s = new string[3];

            SQLUtil sqlutil = new SQLUtil(m_app);
            string config = sqlutil.GetConfigValueByKey("TempletAuditConfig");
            s = config.Split('-');
            return s;
        }

        /// <summary>
        /// 得到当前登录人权限的角色
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public string GetUserRoleID(string userID)
        {
            string sqlGetUserRoleID = "select jobid from users where id = '{0}'";
            DataTable dt = m_app.SqlHelper.ExecuteDataTable(string.Format(sqlGetUserRoleID, userID), CommandType.Text);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["jobid"].ToString();
            }
            return "";
        }

        /// <summary>
        /// 得到图片列表
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
        /// 得到图片数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 获得手术信息
        /// edit ywk 2012年6月25日 11:48:24
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        internal DataTable GetOperInfo_Table(string id)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            {
                new SqlParameter("@EditType",SqlDbType.VarChar),
                new SqlParameter("@ID",SqlDbType.VarChar)
            };
            sqlParam[0].Value = "4";
            sqlParam[1].Value = id;
            DataTable dt = m_app.SqlHelper.ExecuteDataSet("EmrSysTable.usp_Edit_OperInfo", sqlParam, CommandType.StoredProcedure).Tables[0];

            return dt;
        }
        /// <summary>
        /// 删除选择的手术信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal bool DelOperInfo(string id)
        {
            try
            {
                SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@EditType",SqlDbType.VarChar),
                    new SqlParameter("@ID",SqlDbType.VarChar)
                };
                sqlParam[0].Value = "3";
                sqlParam[1].Value = id;
                m_app.SqlHelper.ExecuteDataSet("EmrSysTable.usp_Edit_OperInfo", sqlParam, CommandType.StoredProcedure);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 保存操作
        /// </summary>
        /// <param name="operInfoEntity"></param>
        /// <param name="edittype">1:新增   2：修改</param>
        internal string SaveOperInfo(BaseDataMaintain.OperInfoEntity operInfoEntity, string edittype)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@EditType",SqlDbType.VarChar),
                    new SqlParameter("@ID",SqlDbType.VarChar),
                    new SqlParameter("@MapID",SqlDbType.VarChar),
                    new SqlParameter("@Name",SqlDbType.VarChar),
                    new SqlParameter("@Py",SqlDbType.VarChar),
                    new SqlParameter("@Wb",SqlDbType.VarChar),
                    new SqlParameter("@Valid",SqlDbType.VarChar),
                    new SqlParameter("@Memo",SqlDbType.VarChar),
                    new SqlParameter("@sslb",SqlDbType.VarChar)
                };

            sqlParam[0].Value = edittype;
            sqlParam[1].Value = operInfoEntity.Id;
            sqlParam[2].Value = operInfoEntity.Mapid;
            sqlParam[3].Value = operInfoEntity.Name;
            sqlParam[4].Value = operInfoEntity.Py;
            sqlParam[5].Value = operInfoEntity.Wb;
            sqlParam[6].Value = operInfoEntity.Valid;
            sqlParam[7].Value = operInfoEntity.Memo;
            sqlParam[8].Value = operInfoEntity.Sslb;

            return m_app.SqlHelper.ExecuteDataSet("EmrSysTable.usp_Edit_OperInfo", sqlParam, CommandType.StoredProcedure).Tables[0].Rows[0][0].ToString();
        }
        /// <summary>
        /// 新增判断输入的手术编码是否已经存在
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        internal bool CheckOperID(string id)
        {
            DataTable dt = GetOperInfo_Table(id);
            if (dt.Rows.Count > 0)
            {
                return false;
            }
            return true;
        }
    }
}
