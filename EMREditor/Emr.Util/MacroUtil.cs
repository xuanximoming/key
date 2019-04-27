using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Core;
using HuangF.Sys.Date;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace DrectSoft.Emr.Util
{
    public class MacroUtil
    {
        /// <summary>
        /// 获取宏列表
        /// </summary>
        private static DataTable Macros
        {
            get
            {
                if (_macros == null)
                    _macros = DataAccessFactory.DefaultDataAccess.ExecuteDataTable("select D_NAME,D_COLUMN,D_TABLE,D_SQL,D_TYPE FROM register_item where D_TYPE='宏'");
                return _macros;
            }
        }
        private static DataTable _macros;
        private static string m_PatID;

        public static Dictionary<string, Macro> MacrosList
        {
            get
            {
                if (_macrosList == null)
                {
                    Dictionary<string, Macro> list = new Dictionary<string, Macro>();
                    foreach (DataRow row in Macros.Rows)
                    {
                        Macro mac = new Macro(row);
                        list.Add(mac.Name, mac);
                    }
                    _macrosList = list;
                }
                return _macrosList;
            }
        }

        private static Dictionary<string, Macro> _macrosList;

        private static Dictionary<string, DataRow> _macroSource;
        public static Dictionary<string, DataRow> MacroSource
        {
            get { return _macroSource; }
            set { _macroSource = value; }
        }


        /// <summary>
        /// 得到配置信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfigValueByKey(string key)
        {
            string sql1 = " select * from appcfg where configkey = '" + key + "'; ";
            DataTable dt = DataAccessFactory.DefaultDataAccess.ExecuteDataTable(sql1, CommandType.Text);
            string config = string.Empty;
            if (dt.Rows.Count > 0)
            {
                config = dt.Rows[0]["value"].ToString();
            }
            return config;
        }

        private static void GetMacroSource(string patid, string userid)
        {
            try
            {
                //取得地址的那个取值方式 add by ywk 2012年8月3日 13:47:33 
                //string isreadaddress = GetConfigValueByKey("EmrInputConfig");
                //XmlDocument doc1 = new XmlDocument();
                //doc1.LoadXml(isreadaddress);
                //string ReadAddress = doc1.GetElementsByTagName("IsReadAddressInfo")[0].InnerText;

                _macroSource = new Dictionary<string, DataRow>();
                HFDate hf = new HFDate();
                foreach (DataRow row in Macros.Rows)
                {

                    Macro mac = new Macro(row);
                    DataTable data = null;
                    if (!_macroSource.ContainsKey(mac.D_Table))
                    {
                        //此处做一下特殊处理，如果是inpatient 表则传入病人首页序号
                        if (mac.D_Table.Equals("INPATIENT"))
                        {
                            SqlParameter para = new SqlParameter("@NoOfinpat", SqlDbType.VarChar);
                            para.Value = patid;

                            m_PatID = patid;

                            data = DataAccessFactory.DefaultDataAccess.ExecuteDataTable("USP_EMR_GETPATINFO", new SqlParameter[] { para }, CommandType.StoredProcedure);
                            DataTable newDT = new DataTable();
                            if (data.Rows.Count > 0 && data.Rows[0]["isbaby"].ToString() == "1")//如果是婴儿，就把住院号，换成他母亲的 eidt by ywk 2012年11月23日15:53:27
                            {
                                string mother = data.Rows[0]["mother"].ToString();//取得母亲的首页序号
                                newDT = DataAccessFactory.DefaultDataAccess.ExecuteDataTable(string.Format(@"select patid from inpatient
                            where noofinpat='{0}'", mother), CommandType.Text);
                                if (newDT != null && newDT.Rows.Count > 0)
                                {
                                    data.Rows[0]["patid"] = newDT.Rows[0]["patid"].ToString();//值替换为他母亲的号!
                                }
                            }

                            if (data.Rows.Count > 0)
                            {
                                DateTime dt = Convert.ToDateTime(data.Rows[0]["jieqi"].ToString());
                                string jieqi = hf.GetSolarTermInfo(dt);
                                data.Rows[0]["jieqi"] = jieqi;
                                _macroSource.Add(mac.D_Table, data.Rows[0]);
                            }
                        }
                        else if (mac.D_Table.Equals("INPATIENT_CLINIC"))
                        {
                            SqlParameter para = new SqlParameter("@NoOfinpat", SqlDbType.VarChar);
                            para.Value = patid;

                            m_PatID = patid;

                            data = DataAccessFactory.DefaultDataAccess.ExecuteDataTable("PATIENT_INFO.USP_INPATIENT_CLINIC", new SqlParameter[] { para }, CommandType.StoredProcedure);
                            //data = DataAccessFactory.DefaultDataAccess.ExecuteDataTable("PATIENT_INFO.usp_xml_content", new SqlParameter[] { para }, CommandType.StoredProcedure);
                            if (data.Rows.Count > 0)
                            {
                                _macroSource.Add(mac.D_Table, data.Rows[0]);
                            }
                        }
                        else if (mac.D_Table.Equals("CURRENTUSER")) //得到当前用户信息的宏
                        {
                            string sqlGetUser = @"select users.id userid, (case b.name||'/' when  '/' then users.name  else  b.name||'/'||users.name end)  username,a.masterid,b.name||'/' mastername, users.deptid userdeptid, users.wardid userwardid, department.name userdeptname, ward.name userwardname
                                                from users
                                                left outer join department on department.id = users.deptid and department.valid = '1'
                                                left outer join ward on ward.id = users.wardid and ward.valid = '1'
                                                left join tempusers a on users.id=a.userid
                                                left join users b on a.masterid=b.id
                                                where users.id = '{0}' and users.valid = '1'";
                            data = DataAccessFactory.DefaultDataAccess.ExecuteDataTable(string.Format(sqlGetUser, userid));
                            if (data.Rows.Count > 0)
                                _macroSource.Add(mac.D_Table, data.Rows[0]);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(mac.D_Sql)) continue;
                            data = DataAccessFactory.DefaultDataAccess.ExecuteDataTable(mac.D_Sql);
                            if (data.Rows.Count > 0)
                                _macroSource.Add(mac.D_Table, data.Rows[0]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show("MacroUtil.GetMacroSource:" + ex.Message);
            }
        }


        public static void FillMarcValue(string patid, Macro macro, string userid)
        {
            if (_macroSource == null || m_PatID != patid/*切换病人后需要重新捞取数据*/)
            {
                GetMacroSource(patid, userid);
            }
            if (!_macroSource.ContainsKey(macro.D_Table)) return;

            DataRow row = _macroSource[macro.D_Table];

            if (IsColumnInTable(row, macro.D_Column))
            {
                if (!row.IsNull(macro.D_Column))
                {
                    macro.MacroValue = row[macro.D_Column].ToString();
                }
                else
                {
                    macro.MacroValue = "";
                }
            }
            else
            {
                macro.MacroValue = "【宏值不存在" + macro.D_Column + "】"; ;
            }
        }



        /// <summary>
        /// 列是否存在于表中
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        private static bool IsColumnInTable(DataRow dr, string columnName)
        {
            if (dr.Table.Columns.Contains(columnName))
            {
                return true;
            }
            return false;
        }

        public static string FillMarcValue(string patid, string macroName, string userid)
        {
            if (_macroSource == null) GetMacroSource(patid, userid);

            //寻找marcor
            if (!MacrosList.ContainsKey(macroName)) return string.Empty;
            Macro macro = MacrosList[macroName];
            FillMarcValue(patid, macro, userid);

            return macro.MacroValue;
        }

    }

    /// <summary>
    /// 宏
    /// </summary>
    public class Macro
    {
        /// <summary>
        /// 名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        public string D_Type { get; set; }
        /// <summary>
        /// 执行语句
        /// </summary>
        public string D_Sql { get; set; }
        /// <summary>
        /// 对应表名
        /// </summary>
        public string D_Table { get; set; }
        /// <summary>
        /// 对应字段
        /// </summary>
        public string D_Column { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string MacroValue { get; set; }

        public Macro()
        {

        }

        public Macro(DataRow row)
        {
            Name = row.IsNull("D_NAME") ? string.Empty : row["D_NAME"].ToString();
            D_Table = row.IsNull("D_TABLE") ? string.Empty : row["D_TABLE"].ToString();
            D_Type = row.IsNull("D_TYPE") ? "宏" : row["D_TYPE"].ToString();
            D_Sql = row.IsNull("D_SQL") ? string.Empty : row["D_SQL"].ToString();
            D_Column = row.IsNull("D_COLUMN") ? string.Empty : row["D_COLUMN"].ToString();
        }


    }
}
