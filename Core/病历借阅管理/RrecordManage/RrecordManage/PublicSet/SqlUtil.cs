using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DrectSoft.FrameWork.WinForm.Plugin;
using System.Data;
using System.Data.SqlClient;

namespace DrectSoft.Core.RecordManage.PublicSet
{
    abstract class SqlUtil
    {
        static IDataAccess sql_Helper;
        static IEmrHost m_app;

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
        public static void GetDictionarydetail(LookUpEdit lookUp, string Type, string CategoryID, string Pro)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@FrmType", SqlDbType.VarChar),
                new SqlParameter("@PatNoOfHis", SqlDbType.VarChar),
                new SqlParameter("@CategoryID", SqlDbType.VarChar)
            };
            sqlParam[0].Value = Type;
            sqlParam[1].Value = "0";
            sqlParam[2].Value = CategoryID;

            DataTable frmDateTable = sql_Helper.ExecuteDataTable(Pro, sqlParam, CommandType.StoredProcedure);

            lookUp.Properties.Columns.Clear();
            lookUp.Properties.DataSource = frmDateTable;
            lookUp.Properties.DisplayMember = "NAME";
            lookUp.Properties.ValueMember = "DetailID".ToUpper();
            lookUp.Properties.ShowHeader = false;
            lookUp.Properties.ShowFooter = false;

            LookUpColumnInfoCollection coll = lookUp.Properties.Columns;
            coll.Add(new LookUpColumnInfo("CategoryID".ToUpper()));
            coll.Add(new LookUpColumnInfo("DetailID".ToUpper()));
            coll.Add(new LookUpColumnInfo("NAME".ToUpper()));
            coll.Add(new LookUpColumnInfo("Py".ToUpper()));
            coll.Add(new LookUpColumnInfo("Wb".ToUpper()));

            lookUp.Properties.Columns["NAME".ToUpper()].Visible = true;
            lookUp.Properties.Columns["CategoryID".ToUpper()].Visible = false;
            lookUp.Properties.Columns["DetailID".ToUpper()].Visible = false;
            lookUp.Properties.Columns["Py".ToUpper()].Visible = false;
            lookUp.Properties.Columns["Wb".ToUpper()].Visible = false;
            lookUp.Properties.BestFit();

            lookUp.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            lookUp.Properties.AutoSearchColumnIndex = 2 | 3;
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
                new SqlParameter("@PatNoOfHis", SqlDbType.VarChar),
                new SqlParameter("@CategoryID", SqlDbType.VarChar)
            };
            sqlParam[0].Value = Type;
            sqlParam[1].Value = "0";
            sqlParam[2].Value = CategoryID;

            DataTable frmDateTable = sql_Helper.ExecuteDataTable("usp_RedactPatientInfoFrm", sqlParam, CommandType.StoredProcedure);

            lookUp.Properties.Columns.Clear();
            lookUp.Properties.DataSource = frmDateTable;
            lookUp.Properties.DisplayMember = "NAME";
            lookUp.Properties.ValueMember = "ID";
            lookUp.Properties.ShowHeader = false;
            lookUp.Properties.ShowFooter = false;

            LookUpColumnInfoCollection coll = lookUp.Properties.Columns;
            coll.Add(new LookUpColumnInfo("ParentID"));
            coll.Add(new LookUpColumnInfo("ID"));
            coll.Add(new LookUpColumnInfo("NAME"));
            coll.Add(new LookUpColumnInfo("Py"));
            coll.Add(new LookUpColumnInfo("Wb"));

            lookUp.Properties.Columns["NAME"].Visible = true;
            lookUp.Properties.Columns["ParentID"].Visible = false;
            lookUp.Properties.Columns["ID"].Visible = false;
            lookUp.Properties.Columns["Py"].Visible = false;
            lookUp.Properties.Columns["Wb"].Visible = false;
            lookUp.Properties.BestFit();

            lookUp.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            lookUp.Properties.AutoSearchColumnIndex = 2 | 3;

        }
        #endregion

        #region 读取病人信息
        /// <summary>
        /// 读取病人信息
        /// </summary>
        /// <param name="PatNoOfHis">首页序号</param>
        public static DataTable GetRedactPatientInfoFrm(string Type, string CategoryID, string PatNoOfHis)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@FrmType", SqlDbType.VarChar),
                new SqlParameter("@PatNoOfHis", SqlDbType.VarChar),
                new SqlParameter("@CategoryID", SqlDbType.VarChar)
            };
            sqlParam[0].Value = Type;
            sqlParam[1].Value = PatNoOfHis;
            sqlParam[2].Value = CategoryID;

            return sql_Helper.ExecuteDataTable("usp_RedactPatientInfoFrm", sqlParam, CommandType.StoredProcedure);


        }
        #endregion
    }
}
