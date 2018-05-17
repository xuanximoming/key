using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Data;
using System.Data.SqlClient;

namespace MedicalRecordManage.Object
{
    abstract class SqlUtil
    {
        static DrectSoft.Core.IDataAccess sql_Helper;
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


        /// <summary>
        /// 根据Noofinpat获得病人和他孩子的信息
        /// add ywk
        /// </summary>
        /// <param name="NOOfINPAT"></param>
        /// <returns></returns>
        internal static DataTable GetPatAndBaby(string NOOfINPAT)
        {
            string sql = string.Format(@"select a.noofinpat,a.name as patname,a.birth,a.age,a.agestr,a.sexid,
                        b.name as sexname,a.isbaby  from inpatient a    JOIN dictionary_detail b
                        on  b.detailid = a.sexid
                        where b.categoryid = '3' and (a.noofinpat='{0}' or a.mother='{0}') 
                        order by a.isbaby asc,a.birth desc ", NOOfINPAT);
            return App.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
        }


        /// <summary>
        /// 根据病人的首页序号，得到她的婴儿个数，返回显示内容
        /// add by ywk 2012年6月8日 09:47:53
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string GetPatsBabyContent(IEmrHost m_app, string noofinpat)
        {
            try
            {
                string Result = string.Empty;//最终要返回的内容
                string sql = string.Format(@"select babycount,name from inpatient where noofinpat='{0}'", noofinpat);
                if (App == null)
                {
                    App = m_app;
                }
                DataTable dt = App.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
                if (string.IsNullOrEmpty(dt.Rows[0]["babycount"].ToString()))
                {
                    Result = dt.Rows[0]["Name"].ToString();

                }
                else
                {
                    if (dt.Rows[0]["babycount"].ToString() == "0")
                    {
                        Result = dt.Rows[0]["Name"].ToString();
                    }
                    else
                    {
                        Result = dt.Rows[0]["Name"].ToString() + "【" + dt.Rows[0]["babycount"].ToString() + "个婴儿】";
                    }
                }
                return Result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 根据父母首页序列号，返回父母的住院号
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        public static string GetPatsBabyMother(string noofinpat)
        {
            string Result = string.Empty;//最终要返回的内容
            string sql = string.Format(@"select patid,name from inpatient where noofinpat='{0}'", noofinpat);
            if (App == null)
            {
                App = m_app;
            }
            DataTable dt = App.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
            if (dt.Rows.Count > 0)
            {
                Result = dt.Rows[0]["patid"].ToString();
            }
            return Result;
        }

        /// <summary>
        /// /根据病人首页序号获得她孩子的个数
        /// add ywk
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        public static bool HasBaby(string noofinpat)
        {
            string sql = string.Format(@"select babycount from inpatient where noofinpat='{0}'", noofinpat);
            DataTable dt = App.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["babycount"].ToString() == "0")
                {
                    return false;
                }
                if (Int32.Parse(dt.Rows[0]["babycount"].ToString() == "" ?
                    "0" : dt.Rows[0]["babycount"].ToString()) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
