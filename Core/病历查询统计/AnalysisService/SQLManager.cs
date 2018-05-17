using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using YidanSoft.FrameWork.WinForm.Plugin;

namespace YidanSoft.Core.AnalysisService
{
    class SQLManager
    {
        IYidanEmrHost m_app;

        private const string SQL_Select_BL_BLSJ_ForCountObj =
      " SELECT MedicalRecord.ID "
    + " FROM MedicalRecord "
    + " where  ID>0 ";

        private const string SQL_Select_EmbededObject_SJ_S =
      " select a.RecordID, b.Name, c.DisplayStr, a.Value, a.ObjectID, a.AtomID "
    + " from AtomData_S a, Model_Object b, Model_Atom c "
    + " where EmbedId=@EmbedId "
    + " and a.ObjectID=b.ID and a.AtomID=c.ID ";
        private const string SQL_Select_EmbededObject_SJ_F =
              " select a.RecordID, b.Name, c.DisplayStr, convert(varchar(255),a.Value) as value, a.ObjectID, a.AtomID "
            + " from AtomData_F a, Model_Object b, Model_Atom c "
            + " where EmbedId=@EmbedId "
            + " and a.ObjectID=b.ID and a.AtomID=c.ID ";
        private const string SQL_Select_EmbededObject_SJ_I =
              " select a.RecordID, b.Name, c.DisplayStr, convert(varchar(255),a.Value) as value, a.ObjectID, a.AtomID "
            + " from AtomData_I a, Model_Object b, Model_Atom c "
            + " where EmbedId=@EmbedId "
            + " and a.ObjectID=b.ID and a.AtomID=c.ID ";

        private const string SQL_Order_EmbededObject =
            " order by a.RecordID, a.ObjectID, a.AtomID ";

        private const string SQL_Select_BL_BLSJ =
     " select * from (SELECT distinct a.NoOfInpat, b.Name,b.OutHosDate,b.AgeStr,(case b.SexID when '1' then '男' when '2' then '女' else '未知' end )Sex "
    + " ,b.PatID,b.AdmitDept,b.AdmitWard,b.AdmitBed "
    + " ,c.Name Department, d.Name Ward "
    + " FROM recorddetail a, InPatient b, Department c, Ward d "
    + " where a.NoOfInpat=b.NoOfInpat "
    + " and b.AdmitDept=c.ID and b.AdmitWard=d.ID ) where rownum < {0}";


        private const string str_queryMyTemplate = "select distinct SerialNum,Name from Analysis_Project where DoctorID='{0}'";

        public SQLManager(IYidanEmrHost _app)
        {
            m_app = _app;
        }

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
                    newrow["ID"] = "BASY";
                    newrow["NAME"] = "病案首页";
                    _itemCatalogTable.Rows.Add(newrow);

                    newrow = _itemCatalogTable.NewRow();
                    newrow["ID"] = "YZLL";
                    newrow["NAME"] = "医嘱浏览";
                    _itemCatalogTable.Rows.Add(newrow);

                    newrow = _itemCatalogTable.NewRow();
                    newrow["ID"] = "ZYZ";
                    newrow["NAME"] = "住院志";
                    _itemCatalogTable.Rows.Add(newrow);

                    newrow = _itemCatalogTable.NewRow();
                    newrow["ID"] = "BCJL";
                    newrow["NAME"] = "病程记录";
                    _itemCatalogTable.Rows.Add(newrow);

                    newrow = _itemCatalogTable.NewRow();
                    newrow["ID"] = "ZQWJ";
                    newrow["NAME"] = "知情文件";
                    _itemCatalogTable.Rows.Add(newrow);

                    newrow = _itemCatalogTable.NewRow();
                    newrow["ID"] = "SSXGJL";
                    newrow["NAME"] = "手术相关记录";
                    _itemCatalogTable.Rows.Add(newrow);

                    newrow = _itemCatalogTable.NewRow();
                    newrow["ID"] = "HZJL";
                    newrow["NAME"] = "会诊记录";
                    _itemCatalogTable.Rows.Add(newrow);

                    newrow = _itemCatalogTable.NewRow();
                    newrow["ID"] = "QTJL";
                    newrow["NAME"] = "其他记录";
                    _itemCatalogTable.Rows.Add(newrow);

                    newrow = _itemCatalogTable.NewRow();
                    newrow["ID"] = "SFJL";
                    newrow["NAME"] = "随访记录";
                    _itemCatalogTable.Rows.Add(newrow);

                    newrow = _itemCatalogTable.NewRow();
                    newrow["ID"] = "CFLY";
                    newrow["NAME"] = "查房录音";
                    _itemCatalogTable.Rows.Add(newrow);

                    newrow = _itemCatalogTable.NewRow();
                    newrow["ID"] = "SCB";
                    newrow["NAME"] = "三测表曲线";
                    _itemCatalogTable.Rows.Add(newrow);

                    newrow = _itemCatalogTable.NewRow();
                    newrow["ID"] = "HLWD";
                    newrow["NAME"] = "护理文档";
                    _itemCatalogTable.Rows.Add(newrow);

                    newrow = _itemCatalogTable.NewRow();
                    newrow["ID"] = "HLJL";
                    newrow["NAME"] = "护理记录";
                    _itemCatalogTable.Rows.Add(newrow);

                    newrow = _itemCatalogTable.NewRow();
                    newrow["ID"] = "SSHLJL";
                    newrow["NAME"] = "手术护理记录";
                    _itemCatalogTable.Rows.Add(newrow);

                    newrow = _itemCatalogTable.NewRow();
                    newrow["ID"] = "JHBX";
                    newrow["NAME"] = "监护波形";
                    _itemCatalogTable.Rows.Add(newrow);

                    newrow = _itemCatalogTable.NewRow();
                    newrow["ID"] = "JYZL";
                    newrow["NAME"] = "检验资料";
                    _itemCatalogTable.Rows.Add(newrow);

                    newrow = _itemCatalogTable.NewRow();
                    newrow["ID"] = "JCZL";
                    newrow["NAME"] = "检查资料";
                    _itemCatalogTable.Rows.Add(newrow);

                    newrow = _itemCatalogTable.NewRow();
                    newrow["ID"] = "MZBL";
                    newrow["NAME"] = "门诊病历";
                    _itemCatalogTable.Rows.Add(newrow);

                    newrow = _itemCatalogTable.NewRow();
                    newrow["ID"] = "CRBBG";
                    newrow["NAME"] = "传染病报告";
                    _itemCatalogTable.Rows.Add(newrow);
                }
                return _itemCatalogTable;
            }
        }

        /// <summary>
        /// 指定的查询结果再查询
        /// </summary>
        /// <param name="baseSqlWhere">基础条件</param>
        /// <param name="condition">自定义条件</param>
        /// <param name="results">指定的结果集</param>
        /// <returns></returns>
        public DataSet QueryPatientRecord(String baseSqlWhere, String condition, String EmrQueryResultSetResults)
        {
            StringBuilder SqlWhere = new StringBuilder(SQL_Select_BL_BLSJ);
            //基本条件
            //if (!String.IsNullOrEmpty(baseSqlWhere))
            //    SqlWhere.Append(" and ( " + baseSqlWhere + ")");
            //// 自定义条件,
            //if (!String.IsNullOrEmpty(condition.Trim()))
            //    SqlWhere.Append(" and ( " + condition + ")");
            //// 结果集范围
            //if (!String.IsNullOrEmpty(EmrQueryResultSetResults))
            //    SqlWhere.Append(" and (" + EmrQueryResultSetResults + ")");
            Random ra = new Random();
            return m_app.SqlHelper.ExecuteDataSet(string.Format(SqlWhere.ToString(), ra.Next(30).ToString()));
        }

        /// <summary>
        /// 根据条件统计基础模板的不同取值
        /// </summary>
        /// <param name="baseSqlWhere">基础条件</param>
        /// <param name="condition">自定义条件</param>
        /// <param name="embedModelId"></param>
        /// <returns></returns>
        public DataSet ViewObjects(String baseSqlWhere, String condition, String embedModelId)
        {
            StringBuilder SqlWhere1 = new StringBuilder(SQL_Select_BL_BLSJ_ForCountObj);
            //基本条件
            //if (!String.IsNullOrEmpty(baseSqlWhere))
            //    SqlWhere1.Append(" and ( " + baseSqlWhere + ") ");
            // 自定义条件,
            if (!String.IsNullOrEmpty(condition))
                SqlWhere1.Append(" and ( " + condition.Trim() + ") ");

            String sqlWhere = SQL_Select_EmbededObject_SJ_F
                       + " and a.RecordID in(" + SqlWhere1 + ") "
                       + " union " + SQL_Select_EmbededObject_SJ_I
                       + " and a.RecordID in(" + SqlWhere1 + ") "
                       + " union " + SQL_Select_EmbededObject_SJ_S
                       + " and a.RecordID in(" + SqlWhere1 + ") "
                       + SQL_Order_EmbededObject;

            SqlParameter paramEmbedId = new SqlParameter("EmbedId", SqlDbType.VarChar, 64);
            paramEmbedId.Value = embedModelId;
            return m_app.SqlHelper.ExecuteDataSet(sqlWhere, new SqlParameter[] { paramEmbedId });
        }


        private DataTable _treeListEmbedTable;

        public DataTable TreeListEmbedTable
         {
             get
             {
                 if (_treeListEmbedTable == null)
                 {
                     _treeListEmbedTable = new DataTable();
                     DataColumn dcID = new DataColumn("ID", Type.GetType("System.String"));
                     DataColumn dcName = new DataColumn("NAME", Type.GetType("System.String"));

                     _treeListEmbedTable.Columns.Add(dcID);
                     _treeListEmbedTable.Columns.Add(dcName);

                     DataRow newrow = _treeListEmbedTable.NewRow();
                     newrow["ID"] = "BWXSC";
                     newrow["NAME"] = "部位修饰词";
                     _treeListEmbedTable.Rows.Add(newrow);

                     newrow = _treeListEmbedTable.NewRow();
                     newrow["ID"] = "ZYZD";
                     newrow["NAME"] = "主要诊断";
                     _treeListEmbedTable.Rows.Add(newrow);
                 }
                 return _treeListEmbedTable;
             }
         }

        private DataTable _treeListEmbedSunTable;

        public DataTable TreeListEmbedSunTable
        {
            get
            {
                if (_treeListEmbedSunTable == null)
                {
                    _treeListEmbedSunTable = new DataTable();
                    DataColumn dcID = new DataColumn("ID", Type.GetType("System.String"));
                    DataColumn dcPerID = new DataColumn("PERID", Type.GetType("System.String"));
                    DataColumn dcName = new DataColumn("NAME", Type.GetType("System.String"));

                    _treeListEmbedSunTable.Columns.Add(dcID);
                    _treeListEmbedSunTable.Columns.Add(dcPerID);
                    _treeListEmbedSunTable.Columns.Add(dcName);

                    DataRow newrow = _treeListEmbedSunTable.NewRow();
                    newrow["ID"] = "BWXSC";
                    newrow["PERID"] = "BWXSC";
                    newrow["NAME"] = "部位修饰词";
                    _treeListEmbedSunTable.Rows.Add(newrow);

                    newrow = _treeListEmbedSunTable.NewRow();
                    newrow["ID"] = "ZYZD";
                    newrow["PERID"] = "ZYZD";
                    newrow["NAME"] = "主要诊断";
                    _treeListEmbedSunTable.Rows.Add(newrow);

                    newrow = _treeListEmbedSunTable.NewRow();
                    newrow["ID"] = "ZYZD";
                    newrow["PERID"] = "ZYZD";
                    newrow["NAME"] = "主要诊断";
                    _treeListEmbedSunTable.Rows.Add(newrow);
                }
                return _treeListEmbedSunTable;
            }
        }

        public DataTable MDS_RetrieveMyTemplate(string userid)
        {
            string sql = string.Format(str_queryMyTemplate,userid);


            return m_app.SqlHelper.ExecuteDataSet(sql).Tables[0];
        }
    }
}
