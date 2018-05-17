using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Core;
using System.Data.SqlClient;

namespace DrectSoft.Emr.TemplateFactory.BaseDataMaintain
{
    class SqlHelp
    {
        const string m_SqlConnect = @"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=-ser)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=emr)));User Id=dba;Password=sa;";

        IEmrHost m_app;
        public SqlHelp(IEmrHost host)
        {
            m_app = host;
        }

        #region 宏
        const string m_SqlGetMacro = @"select D_NAME, example, D_COLUMN,D_TABLE,D_SQL from register_item   WHERE D_TYPE = '宏'  ORDER BY  D_NAME ";
        const string m_SqlUpdateMacro = @"update register_item
                                            set register_item.example = '{1}', 
                                                register_item.d_column = '{2}', register_item.d_table = '{3}', 
                                                register_item.d_sql = '{4}'
                                            where register_item.d_name = '{0}' and register_item.d_type = '宏'";
        const string m_SqlInsertMacro = @"insert into  register_item(d_type, d_name, example, d_column, d_table, d_sql) values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')";
        const string m_SqlDeleteMacro = @"delete from register_item where d_name = '{0}' and D_TYPE = '宏'";
        public SqlHelp()
        {
            
        }

        public DataTable GetMacro()
        {
            DataTable dt = new DataTable();
            dt = m_app.SqlHelper.ExecuteDataTable(m_SqlGetMacro);
            return dt;
        }

        public void UpdateMacro(Macro macro)
        {
            string sql = string.Format(m_SqlUpdateMacro, macro.Name, macro.Example, macro.Column, macro.Table, macro.Sql);
            m_app.SqlHelper.ExecuteNoneQuery(sql);
        }

        public void InsertMacro(Macro macro)
        {
            string sql = string.Format(m_SqlInsertMacro, macro.Type, macro.Name, macro.Example, macro.Column, macro.Table, macro.Sql);
            m_app.SqlHelper.ExecuteNoneQuery(sql);
        }

        public void DeleteMacro(Macro macro)
        {
            string sql = string.Format(m_SqlDeleteMacro, macro.Name);
            m_app.SqlHelper.ExecuteNoneQuery(sql);
        }
        #endregion

        #region 数据字典

        string m_SqlGetDictionary = @"select id, name, py, wb from emr_dictionary where type = '1' and cancel = 'N'";
        string m_SqlInsertDictionary = @"insert into emr_dictionary(id, name, type, cancel, py, wb) select nvl(max(id) + 1, 0), '{0}', '1', 'N', '{1}', '{2}' from emr_dictionary";
        string m_SqlUpdateDictionary = @"update emr_dictionary
                                                set emr_dictionary.name = '{1}', py = '{2}', wb = '{3}'
                                                where emr_dictionary.id = '{0}' and emr_dictionary.type = '1' and emr_dictionary.cancel = 'N'";
        string m_SqlCancelDictionary = @"update emr_dictionary set cancel = 'Y' where id = '{0}' and type = '1' and cancel = 'N'";

        public DataTable GetDictionary()
        {
            DataTable dt = new DataTable();
            dt = m_app.SqlHelper.ExecuteDataTable(m_SqlGetDictionary);
            return dt;
        }

        public void UpdateDictionary(Dictionary dict)
        {
            string[] code = GetPYWB(dict.Name);
            string sql = string.Format(m_SqlUpdateDictionary, dict.ID, dict.Name, code[0], code[1]);
            m_app.SqlHelper.ExecuteNoneQuery(sql);
        }

        public void InsertDictionary(Dictionary dict)
        {
            string[] code = GetPYWB(dict.Name);
            string sql = string.Format(m_SqlInsertDictionary, dict.Name, code[0], code[1]);
            m_app.SqlHelper.ExecuteNoneQuery(sql);
        }

        public void DeleteDictionary(Dictionary dict)
        {
            string sql = string.Format(m_SqlCancelDictionary, dict.ID);
            m_app.SqlHelper.ExecuteNoneQuery(sql);
        }
        #endregion

        #region 数据字典 Detail

        string m_SqlGetDictionaryDetail = @"select dictionary_id, name, py, wb, code,id from emr_dictionary_detail where cancel = 'N' and dictionary_id = '{0}'";
        string m_SqlUpdateDictionaryDetail = @"update emr_dictionary_detail 
                                               set name = '{1}', py = '{2}', wb = '{3}', code = '{4}' 
                                               where id = {0}";
        string m_SqlCancelDictionaryDetail = @"update emr_dictionary_detail set cancel = 'Y' where id = {0}";
        string m_SqlInsertDictionaryDetail = @"insert into emr_dictionary_detail(id, dictionary_id, name, cancel, py, wb, code) 
                                               select nvl(max(id) + 1, 0),  '{0}', '{1}', 'N', '{2}', '{3}', '{4}' 
                                               from emr_dictionary_detail";

        public DataTable GetDictionaryDetail(string dictionaryID)
        {
            DataTable dt = new DataTable();
            string sql = string.Format(m_SqlGetDictionaryDetail, dictionaryID);
            dt = m_app.SqlHelper.ExecuteDataTable(sql);
            return dt;
        }

        public void UpdateDictionaryDetail(DictionaryDetail dictDetail)
        {
            string[] code = GetPYWB(dictDetail.Name);
            string sql = string.Format(m_SqlUpdateDictionaryDetail, dictDetail.ID, dictDetail.Name, code[0], code[1], dictDetail.CODE);
            m_app.SqlHelper.ExecuteNoneQuery(sql);
        }

        public void InsertDictionaryDetail(DictionaryDetail dictDetail)
        {
            string[] code = GetPYWB(dictDetail.Name);
            string sql = string.Format(m_SqlInsertDictionaryDetail, dictDetail.DictionaryID, dictDetail.Name, code[0], code[1], dictDetail.CODE);
            m_app.SqlHelper.ExecuteNoneQuery(sql);
        }

        public void DeleteDictionaryDetail(DictionaryDetail dictDetail)
        {
            string sql = string.Format(m_SqlCancelDictionaryDetail, dictDetail.ID);
            m_app.SqlHelper.ExecuteNoneQuery(sql);
        }
        #endregion

        #region 特殊符号

        string m_GetSymbol = @"select symbol from dict_symbol order by symbol";
        string m_InsertSymbol = @"insert into dict_symbol(symbol, dept_id) values('{0}', ' ')";
        string m_UpdateSymbol = @"update dict_symbol set symbol = '{1}' where symbol = '{0}'";
        string m_DeleteSymbol = @"delete from dict_symbol where symbol = '{0}'";

        public DataTable GetSymbol()
        {
            DataTable dt = new DataTable();
            dt = m_app.SqlHelper.ExecuteDataTable(m_GetSymbol);
            return dt;
        }

        public void UpdateSymbol(Symbol symbol)
        {
            string sql = string.Format(m_UpdateSymbol, symbol.OldSymbol, symbol.NewSymbol);
            m_app.SqlHelper.ExecuteNoneQuery(sql);
        }

        public void InsertSymbol(Symbol symbol)
        {
            string sql = string.Format(m_InsertSymbol, symbol.NewSymbol);
            m_app.SqlHelper.ExecuteNoneQuery(sql);
        }

        public void DeleteSymbol(Symbol symbol)
        {
            string sql = string.Format(m_DeleteSymbol, symbol.OldSymbol);
            m_app.SqlHelper.ExecuteNoneQuery(sql);
        }
        #endregion

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

        #region 复用项目维护（表emr_replace_item）
        /// <summary>
        /// 获得模板工厂表emr_replace_item里的信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetModelFactory(string modelid)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            {
                new SqlParameter("@EditType",SqlDbType.VarChar),
                new SqlParameter("@ModelId",SqlDbType.VarChar)
            };
            sqlParam[0].Value = "4";
            sqlParam[1].Value = modelid;
            DataTable dt = m_app.SqlHelper.ExecuteDataSet("EmrSysTable.usp_Edit_ModelEmr", sqlParam, CommandType.StoredProcedure).Tables[0];

            return dt;
        }

        public bool DelEmrItem(string modelid)
        {
            try
            {
                SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@EditType",SqlDbType.VarChar),
                    new SqlParameter("@ModelId",SqlDbType.VarChar)
                };
                sqlParam[0].Value = "3";
                sqlParam[1].Value = modelid;
                m_app.SqlHelper.ExecuteDataSet("EmrSysTable.usp_Edit_ModelEmr", sqlParam, CommandType.StoredProcedure);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 增加新模板时验证编号是否已经存在
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool CheckEmrItemID(string modelid)
        {
            DataTable dt = GetModelFactory(modelid);
            if (dt.Rows.Count > 0)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 保存更新或者新增的值
        /// </summary>
        /// <param name="emrReplaceItem"></param>
        /// <param name="edittype"></param>
        public string SaveEmrItem(EmrReplaceItem emrReplaceItem, string edittype)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@EditType",SqlDbType.VarChar),
                    new SqlParameter("@ModelId",SqlDbType.VarChar),
                    new SqlParameter("@DestEmrName",SqlDbType.VarChar),
                    new SqlParameter("@SourceEmrName",SqlDbType.VarChar),
                    new SqlParameter("@DestItemName",SqlDbType.VarChar),
                    new SqlParameter("@SourceItemName",SqlDbType.VarChar),
                    new SqlParameter("@Valid",SqlDbType.Int)
                };

            sqlParam[0].Value = edittype;
            sqlParam[1].Value = emrReplaceItem.Id;
            sqlParam[2].Value = emrReplaceItem.DestEmrName;
            sqlParam[3].Value = emrReplaceItem.SourceEmrName;
            sqlParam[4].Value = emrReplaceItem.DestItemName;
            sqlParam[5].Value = emrReplaceItem.SourceItemName;
            sqlParam[6].Value = emrReplaceItem.Valid;

            return m_app.SqlHelper.ExecuteDataSet("EmrSysTable.usp_Edit_ModelEmr", sqlParam, CommandType.StoredProcedure).Tables[0].Rows[0][0].ToString();
        }
    
        #endregion 

        #region 诊断按钮

        private string sqlGetDiag = "SELECT * FROM patdiagtype order by code";
        private string sqlDeleteDiag = "delete from patdiagtype where code='{0}' and diagname='{1}'";
        private string sqlUpdateDiag = @"update patdiagtype 
                                               set sno = {0}, diagname = '{1}', typeid = 2 
                                               where code = '{0}'";

        /// <summary>
        /// 得到诊断按钮
        /// </summary>
        /// <returns></returns>
        public DataTable GetDiagButton()
        {
            DataTable dt = m_app.SqlHelper.ExecuteDataTable(sqlGetDiag, CommandType.Text);
            return dt;
        }

        /// <summary>
        /// 新增或修改诊断按钮
        /// </summary>
        /// <param name="diagName"></param>
        public void SaveDiagButton(string diagName)
        {
            SqlParameter[] sp = new SqlParameter[] 
            {
                new SqlParameter("diagname", SqlDbType.VarChar)
            };
            sp[0].Value = diagName;
            m_app.SqlHelper.ExecuteNoneQuery("EMRTEMPLETFACTORY.usp_SaveDiagButton", sp, CommandType.StoredProcedure);
            /*
             * PROCEDURE usp_SaveDiagButton(v_diagname   varchar) AS
                  BEGIN
                    MERGE INTO patdiagtype p
                    USING (SELECT v_diagname diagname FROM dual) a
                    ON (p.diagname = a.diagname)
                    WHEN NOT MATCHED THEN
                    INSERT VALUES
                    (
                        (SELECT max(sno) + 1 FROM patdiagtype),
                        (SELECT max(sno) + 1 FROM patdiagtype),
                        v_diagname,
                        '2'
                    );
                  END;
             */
        }

        /// <summary>
        /// 更新诊断按钮
        /// </summary>
        /// <param name="code"></param>
        /// <param name="diagname"></param>
        public void EditDiagButton(int code, string diagname)
        {
            //SqlParameter[] sp = new SqlParameter[] 
            //{
            //    new SqlParameter("v_code", SqlDbType.VarChar),
            //    new SqlParameter("v_diagname", SqlDbType.VarChar)
            //};
            //sp[0].Value = code;
            //sp[1].Value = diagname;
            //m_app.SqlHelper.ExecuteNoneQuery("EMRTEMPLETFACTORY.usp_UpdateDiagButton", sp, CommandType.StoredProcedure);
            m_app.SqlHelper.ExecuteNoneQuery(string.Format(sqlUpdateDiag, code, diagname), CommandType.Text);
        }

        /// <summary>
        /// 删除诊断按钮
        /// </summary>
        /// <param name="code"></param>
        public void DeleteDiagButton(string code, string diagname)
        {
            m_app.SqlHelper.ExecuteNoneQuery(string.Format(sqlDeleteDiag, code, diagname), CommandType.Text);
        }

        #endregion
    }
}
