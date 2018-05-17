using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace DrectSoft.Core.RedactPatientInfo.PublicSet
{
    public static class SqlUtil
    {
        static IDataAccess sql_Helper;
        static IEmrHost m_app;

        public static IDataAccess SQLHelper
        {
            get { return sql_Helper; }

        }

        public static IEmrHost App
        {
            get { return m_app; }
            set
            {
                m_app = value;
                sql_Helper = m_app.SqlHelper;

            }
        }

        #region 获取字典分类明细库
        /// <summary>
        /// 获取字典分类明细库
        /// </summary>
        /// <param name="lookUp">下拉框控件名</param>
        /// <param name="?">下拉框内容类别</param>
        public static void GetDictionarydetail(LookUpEdit lookUp, string Type, string CategoryID)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@FrmType", SqlDbType.VarChar),
                new SqlParameter("@NoOfInpat", SqlDbType.VarChar),
                new SqlParameter("@CategoryID", SqlDbType.VarChar)
            };
            sqlParam[0].Value = Type;
            sqlParam[1].Value = "0";
            sqlParam[2].Value = CategoryID;

            DataTable frmDateTable = sql_Helper.ExecuteDataTable("usp_RedactPatientInfoFrm", sqlParam, CommandType.StoredProcedure);

            m_app.PublicMethod.ConvertDataSourceUpper(frmDateTable);

            lookUp.Properties.Columns.Clear();
            lookUp.Properties.DataSource = frmDateTable;
            lookUp.Properties.DisplayMember = "NAME";
            lookUp.Properties.ValueMember = "DETAILID";
            lookUp.Properties.ShowHeader = false;
            lookUp.Properties.ShowFooter = false;

            LookUpColumnInfoCollection coll = lookUp.Properties.Columns;
            coll.Add(new LookUpColumnInfo("CATEGORYID"));
            coll.Add(new LookUpColumnInfo("DETAILID"));
            coll.Add(new LookUpColumnInfo("NAME"));
            coll.Add(new LookUpColumnInfo("PY"));
            coll.Add(new LookUpColumnInfo("WB"));

            lookUp.Properties.Columns["NAME"].Visible = true;
            lookUp.Properties.Columns["CATEGORYID"].Visible = false;
            lookUp.Properties.Columns["DETAILID"].Visible = false;
            lookUp.Properties.Columns["PY"].Visible = false;
            lookUp.Properties.Columns["WB"].Visible = false;
            lookUp.Properties.BestFit();

            lookUp.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            lookUp.Properties.AutoSearchColumnIndex = 2 | 3;

            //m_app.PublicMethod.ConvertLookUpEditDataSourceUpper(lookUp);
        }
        #endregion

        #region 获取地区信息
        /// <summary>
        /// 获取地区信息
        /// </summary>
        /// <param name="lookUp">下拉框控件名</param>
        /// <param name="CategoryID">下拉框内容类别</param>
        public static void GetAreas(LookUpEdit lookUp, string Type, string CategoryID)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@FrmType", SqlDbType.VarChar),
                new SqlParameter("@NoOfInpat", SqlDbType.VarChar),
                new SqlParameter("@CategoryID", SqlDbType.VarChar)
            };
            sqlParam[0].Value = Type;
            sqlParam[1].Value = "0";
            sqlParam[2].Value = CategoryID;

            DataTable frmDateTable = sql_Helper.ExecuteDataTable("usp_RedactPatientInfoFrm", sqlParam, CommandType.StoredProcedure);

            m_app.PublicMethod.ConvertDataSourceUpper(frmDateTable);

            lookUp.Properties.Columns.Clear();
            lookUp.Properties.DataSource = frmDateTable;
            lookUp.Properties.DisplayMember = "NAME";
            lookUp.Properties.ValueMember = "ID";
            lookUp.Properties.ShowHeader = false;
            lookUp.Properties.ShowFooter = false;

            LookUpColumnInfoCollection coll = lookUp.Properties.Columns;
            coll.Add(new LookUpColumnInfo("PARENTID"));
            coll.Add(new LookUpColumnInfo("ID"));
            coll.Add(new LookUpColumnInfo("NAME"));
            coll.Add(new LookUpColumnInfo("PY"));
            coll.Add(new LookUpColumnInfo("WB"));

            lookUp.Properties.Columns["NAME"].Visible = true;
            lookUp.Properties.Columns["PARENTID"].Visible = false;
            lookUp.Properties.Columns["ID"].Visible = false;
            lookUp.Properties.Columns["PY"].Visible = false;
            lookUp.Properties.Columns["WB"].Visible = false;
            lookUp.Properties.BestFit();

            lookUp.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            lookUp.Properties.AutoSearchColumnIndex = 2 | 3;

            //m_app.PublicMethod.ConvertLookUpEditDataSourceUpper(lookUp);

        }
        #endregion

        #region 读取病人信息
        /// <summary>
        /// 读取病人信息
        /// </summary>
        /// <param name="NoOfInpat">首页序号</param>
        public static DataTable GetRedactPatientInfoFrm(string Type, string CategoryID, string NoOfInpat)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@FrmType", SqlDbType.VarChar),
                new SqlParameter("@NoOfInpat", SqlDbType.VarChar),
                new SqlParameter("@CategoryID", SqlDbType.VarChar)
            };
            sqlParam[0].Value = Type;
            sqlParam[1].Value = NoOfInpat;
            sqlParam[2].Value = CategoryID;

            return sql_Helper.ExecuteDataTable("usp_RedactPatientInfoFrm", sqlParam, CommandType.StoredProcedure);


        }
        #endregion

        #region 判断是否为数字
        /// <summary>
        /// 判断是否为数字
        /// </summary>
        /// <param name="Number">判断字符</param>
        /// <returns></returns>
        public static bool IsNumber(string Number)
        {
            try
            {
                double var = Convert.ToDouble(Number);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

    }
}
