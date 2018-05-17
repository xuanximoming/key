using DrectSoft.Common;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Wordbook;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Xml;
namespace MedicalRecordManage.Object
{
    class ComponentCommand
    {
        public static Color _CAO_GAO_COLOR = Color.Blue;
        public static Color _DAI_SHEN_HE_COLOR = Color.LightSeaGreen;
        public static Color _SHEN_HE_TONG_GUO_COLOR = Color.Green;
        public static Color _SHEN_HE_BU_TONG_GUO_COLOR = Color.Red;
        public static Color _CHE_XIAO_COLOR = Color.Gray;
        public static Color _GUI_HUAN_COLOR = Color.Brown;
        //返回当前登录用户的ID
        public static string GetCurrentDoctor(IEmrHost app)
        {
            try
            {
                return app.User.DoctorId;// YD_Common.currentUser.DoctorId;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //返回当前登录用户的ID
        public static string GetCurrentDoctor()
        {
            try
            {
                return DS_Common.currentUser.DoctorId;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //
        public static void InitializaImage(
            ref System.Windows.Forms.ImageList imageListBrxb,
            ref DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox repItemImageComboBoxBrxb)
        {
            try
            {
                imageListBrxb = ImageHelper.GetImageListBrxb();
                repItemImageComboBoxBrxb.SmallImages = imageListBrxb;
                DevExpress.XtraEditors.Controls.ImageComboBoxItem ImageComboItemMale = new DevExpress.XtraEditors.Controls.ImageComboBoxItem("男", "1", 1);
                DevExpress.XtraEditors.Controls.ImageComboBoxItem ImageComboItemFemale = new DevExpress.XtraEditors.Controls.ImageComboBoxItem("女", "2", 0);
                repItemImageComboBoxBrxb.Items.Add(ImageComboItemMale);
                repItemImageComboBoxBrxb.Items.Add(ImageComboItemFemale);
            }
            catch (Exception)
            {

                throw;
            }
        }
        //初始化借阅目的
        public static void InitializePurpose(ref DrectSoft.Common.Library.LookUpEditor lookUpEditorPurpose,
            ref DrectSoft.Common.Library.LookUpWindow lookUpWindowPurpose)
        {
            try
            {
                lookUpWindowPurpose.SqlHelper = SqlUtil.App.SqlHelper;

                DataTable Purpose = SqlUtil.App.SqlHelper.ExecuteDataTable("usp_GetMedicalRrecordViewFrm",
                   new SqlParameter[] { new SqlParameter("@GetType", "2") }, CommandType.StoredProcedure);

                Purpose.Columns["NAME"].Caption = "借阅目的";

                Dictionary<string, int> PurposeCols = new Dictionary<string, int>();

                PurposeCols.Add("NAME", 150);

                SqlWordbook PurposeWordBook = new SqlWordbook("querybook", Purpose, "ID", "NAME", PurposeCols, "ID//NAME//PY//WB");
                lookUpEditorPurpose.SqlWordbook = PurposeWordBook;
                lookUpEditorPurpose.CodeValue = "5001";
            }
            catch (Exception)
            {

                throw;
            }
        }
        //初始化手术信息
        public static void InitializeOperation(ref DrectSoft.Common.Library.LookUpEditor lookUpEditorOperation,
            ref DrectSoft.Common.Library.LookUpWindow lookUpWindowOperation)
        {
            try
            {
                lookUpWindowOperation.SqlHelper = SqlUtil.App.SqlHelper;

                DataTable Dept = SqlUtil.App.SqlHelper.ExecuteDataTable("usp_GetRecordManageFrm",
                     new SqlParameter[] { new SqlParameter("@FrmType", "2") }, CommandType.StoredProcedure);

                Dept.Columns["ID"].Caption = "手术代码";
                Dept.Columns["NAME"].Caption = "手术名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 60);
                cols.Add("NAME", 120);

                SqlWordbook operWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//NAME//PY//WB");
                lookUpEditorOperation.SqlWordbook = operWordBook;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //初始化出院诊断信息
        public static void InitializeDiagnosis(ref DrectSoft.Common.Library.LookUpEditor lookUpEditorDiagnosis,
            ref DrectSoft.Common.Library.LookUpWindow lookUpWindowDiagnosis)
        {
            try
            {
                DataTable disease = new DataTable();
                disease.Columns.Add("ICD");
                disease.Columns.Add("NAME");
                disease.Columns.Add("PY");
                disease.Columns.Add("WB");
                DataTable diagnosis = SqlUtil.App.SqlHelper.ExecuteDataTable("select * from diagnosis");

                foreach (DataRow row in diagnosis.Rows)
                {
                    DataRow displayRow = disease.NewRow();
                    displayRow["ICD"] = row["ICD"];
                    displayRow["NAME"] = row["NAME"];
                    displayRow["PY"] = row["PY"];
                    displayRow["WB"] = row["WB"];
                    disease.Rows.Add(displayRow);
                }

                lookUpWindowDiagnosis.SqlHelper = SqlUtil.App.SqlHelper;
                disease.Columns["ICD"].Caption = "诊断编码";
                disease.Columns["NAME"].Caption = "诊断名称";
                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ICD", 60);
                cols.Add("NAME", 120);

                SqlWordbook diagWordBook = new SqlWordbook("queryDiag", disease, "ICD", "NAME", cols, "ICD//NAME//PY//WB");
                lookUpEditorDiagnosis.SqlWordbook = diagWordBook;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //初始化单位列表数据源
        public static void InitializeDepartment(ref DrectSoft.Common.Library.LookUpEditor lookUpEditorDepartment,
            ref DrectSoft.Common.Library.LookUpWindow lookUpWindowDepartment)
        {

            try
            {
                lookUpWindowDepartment.SqlHelper = SqlUtil.App.SqlHelper;

                DataTable Dept = SqlUtil.App.SqlHelper.ExecuteDataTable("usp_GetMedicalRrecordViewFrm",
                     new SqlParameter[] { new SqlParameter("@GetType", "1") }, CommandType.StoredProcedure);

                Dept.Columns["ID"].Caption = "科室编码";
                Dept.Columns["NAME"].Caption = "科室名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 80);
                cols.Add("NAME", 120);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//NAME//PY//WB");
                lookUpEditorDepartment.SqlWordbook = deptWordBook;
                lookUpEditorDepartment.CodeValue = "0000";
            }
            catch (Exception)
            {

                throw;
            }

        }

        //初始化主治医生列表数据源 add by ck 2013-8-26
        public static void InitializeDoctor(ref DrectSoft.Common.Library.LookUpEditor lookUpEditorDoctor,
            ref DrectSoft.Common.Library.LookUpWindow lookUpWindowDoctor)
        {

            try
            {
                //lookUpWindowDoctor.SqlHelper = SqlUtil.App.SqlHelper;

                //DataTable Dept = SqlUtil.App.SqlHelper.ExecuteDataTable("usp_GetMedicalRrecordViewFrm",
                //     new SqlParameter[] { new SqlParameter("@GetType", "1") }, CommandType.StoredProcedure);
                string sql = @"select t.id,t.name,t.py,t.wb from users t where t.valid='1'";
                DataTable dt = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sql);
                dt.Columns["ID"].Caption = "医生编号";
                dt.Columns["NAME"].Caption = "医生姓名";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 80);
                cols.Add("NAME", 120);

                SqlWordbook doctorWordBook = new SqlWordbook("querybook", dt, "ID", "NAME", cols, "ID//NAME//PY//WB");
                lookUpEditorDoctor.SqlWordbook = doctorWordBook;
                //lookUpEditorDoctor.CodeValue = "0000";
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        //初始化Grid对象，定义列和页眉
        //ID,rownum,...
        public static void InitializeGrid(DevExpress.XtraGrid.Views.Grid.GridView gridView, List<string> list)
        {
            try
            {
                gridView.Columns.Clear();
                for (int i = 0; i < list.Count; i++)
                {
                    gridView.Columns.Add();
                    gridView.Columns[i].Caption = list[i].ToString();//
                    gridView.Columns[i].SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;
                    gridView.Columns[i].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                    gridView.Columns[i].OptionsColumn.AllowEdit = false;
                }
                gridView.Columns[0].Visible = false;
            }
            catch (Exception)
            {

                throw;
            }
        }
        //传入目标数据集合的地址，直接进行数据的整理压缩
        public static void ImpressDataSet(ref DataTable dataTable, string refercolumn, string targetcolumn)
        {
            try
            {
                string id = ""; string nextid = ""; string value = "";
                List<int> list = new List<int>();
                int j = 0; int itimes = 0;
                //合并记录数值,制定参考字段和合并目标字段
                for (int i = 0; i < dataTable.Rows.Count - 1; i++)
                {
                    j = i + 1;
                    itimes = 0;
                    value = dataTable.Rows[i][targetcolumn].ToString();
                    id = dataTable.Rows[i][refercolumn].ToString();
                    nextid = dataTable.Rows[j][refercolumn].ToString();
                    while (id == nextid && j < dataTable.Rows.Count)
                    {
                        if (!value.Contains(dataTable.Rows[j][targetcolumn].ToString()))
                        {
                            value = value + "," + dataTable.Rows[j][targetcolumn].ToString();
                        }
                        list.Add(j);
                        j++;
                        if (j < dataTable.Rows.Count)
                        {
                            nextid = dataTable.Rows[j][refercolumn].ToString();
                        }
                        //
                        itimes++;
                    }
                    dataTable.Rows[i][targetcolumn] = value;
                    i = i + itimes;
                }
                //删除重复记录,
                int index = 0;
                itimes = 0;
                for (int i = 0; i < list.Count; i++)
                {
                    index = list[i] - itimes;
                    dataTable.Rows.RemoveAt(index);
                    itimes++;
                }
                //重新编写序号
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    dataTable.Rows[i]["SEQ"] = i + 1;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //传入目标数据集合的地址，直接进行数据的整理压缩
        public static void ImpressDataSet(ref DataTable dataTable, string refercolumn,
            string targetcolumn1, string targetcolumn2)
        {
            try
            {
                string id = ""; string nextid = ""; string value1 = "";
                string value2 = "";
                List<int> list = new List<int>();
                int j = 0; int itimes = 0;
                //合并记录数值,制定参考字段和合并目标字段
                for (int i = 0; i < dataTable.Rows.Count - 1; i++)
                {
                    j = i + 1;
                    itimes = 0;
                    value1 = dataTable.Rows[i][targetcolumn1].ToString();
                    value2 = dataTable.Rows[i][targetcolumn2].ToString();
                    id = dataTable.Rows[i][refercolumn].ToString();
                    nextid = dataTable.Rows[j][refercolumn].ToString();
                    while (id == nextid && j < dataTable.Rows.Count)
                    {
                        if (!value1.Contains(dataTable.Rows[j][targetcolumn1].ToString()))
                        {
                            value1 = value1 + "," + dataTable.Rows[j][targetcolumn1].ToString();
                        }
                        if (!value2.Contains(dataTable.Rows[j][targetcolumn2].ToString()))
                        {
                            value2 = value2 + "," + dataTable.Rows[j][targetcolumn2].ToString();
                        }
                        list.Add(j);
                        j++;
                        if (j < dataTable.Rows.Count)
                        {
                            nextid = dataTable.Rows[j][refercolumn].ToString();
                        }
                        //
                        itimes++;
                    }
                    dataTable.Rows[i][targetcolumn1] = value1;
                    dataTable.Rows[i][targetcolumn2] = value2;
                    i = i + itimes;
                }
                //删除重复记录,
                int index = 0;
                itimes = 0;
                for (int i = 0; i < list.Count; i++)
                {
                    index = list[i] - itimes;
                    dataTable.Rows.RemoveAt(index);
                    itimes++;
                }
                //重新编写序号
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    dataTable.Rows[i]["SEQ"] = i + 1;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        //把状态值改为文字显示
        public static void InitializeStatusInfo(ref DataTable dataTable)
        {
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                dataTable.Rows[i]["STATUSDES"] = GetStatusDescrible(dataTable.Rows[i]["STATUS"].ToString());
            }
        }

        //草稿0;待审核1;审核通过2;审核不通过3;撤销4;归还5
        public static string GetStatusDescrible(string index)
        {
            string value = "";
            int i = int.Parse(index);
            switch (i)
            {
                case 0:
                    value = "草稿";
                    break;
                case 1:
                    value = "待审核";
                    break;
                case 2:
                    value = "审核通过";
                    break;
                case 3:
                    value = "审核不通过";
                    break;
                case 4:
                    value = "撤销";
                    break;
                case 5:
                    value = "归还";
                    break;
                default:
                    value = "草稿";
                    break;
            }
            return value;
        }
        //草稿0;待审核1;审核通过2;审核不通过3;撤销4;归还5
        public static int GetStatusValue(string describle)
        {
            int value = 0;
            switch (describle)
            {

                case "草稿":
                    value = 0;
                    break;
                case "待审核":
                    value = 1;
                    break;
                case "审核通过":
                    value = 2;
                    break;
                case "审核不通过":
                    value = 3;
                    break;
                case "撤销":
                    value = 4;
                    break;
                case "归还":
                    value = 5;
                    break;
                default:
                    value = 0;
                    break;
            }
            return value;
        }

        //归档4700和null;未归档4701;撤销归档4702  add by zjy
        public static int GetLockStatusValue(string describle)
        {
            int value = 0;
            switch (describle)
            {
                case "未完成":
                    value = 4700;
                    break;
                case "已归档":
                    value = 4701;
                    break;
                case "撤销归档":
                    value = 4702;
                    break;
                case "科室质控":
                    value = 4705;
                    break;
                case "已提交":
                    value = 4706;
                    break;
                case "已完成":
                    value = 4704;
                    break;
                case "补写提交":
                    value = 4707;
                    break;
                case "全部":
                    value = 0;
                    break;
                //         '
                //when i.islock='4701'then '已归档'
                //when i.islock='4702' then '撤销归档'
                //  when i.islock='4704'then '已完成'
                //when i.islock='4705' then '科室质控'
                //   when i.islock='4706' then '已提交'
            }
            return value;
        }


        //获取数据集中指定条件下记录数量,字段为整形字段
        public static int GetDataSetRowCountWithIntLimit(ref DataTable dataTable, string column, string value)
        {
            try
            {
                int icount = dataTable.Select(column = value).Length;
                return icount;
            }
            catch (Exception)
            {

                throw;
            }
        }
        //获取配置参数
        public static int GetAppParametersCfg(string id)
        {
            /*
            try
            {
                XmlDocument doc = new XmlDocument();
                int value = 0;
                string flieStr = AppDomain.CurrentDomain.BaseDirectory + "medicalcfg.xml";
                doc.Load(flieStr);
                foreach (XmlNode node in doc.SelectNodes("parameters/val"))
                {
                    if (id == node.Attributes["id"].Value.ToString().Trim())
                    {
                        value = int.Parse(node.Attributes["value"].Value.ToString().Trim());
                    }
                }
                return value;
            }
            catch (System.Xml.XmlException)
            {
                throw;
            }
            */
            try
            {
                string sql = " select value from appcfg a where a.configkey = '{0}' and a.valid = '1' ";
                return int.Parse(DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteScalar(string.Format(sql, id), CommandType.Text).ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " 没有设置Appcfg表中的" + id);
            }
        }
        public static string GetAppcfgConsult(string configName)
        {
            try
            {
                string sql = " select value from appcfg a where a.configkey = '{0}' and a.valid = '1' ";
                return DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteScalar(string.Format(sql, configName), CommandType.Text).ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " 没有设置Appcfg表中的" + configName);
            }
        }
        //设置配置参数
        public static void SetAppParametersCfg(string id, string value)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                string flieStr = AppDomain.CurrentDomain.BaseDirectory + "medicalcfg.xml";
                doc.Load(flieStr);
                foreach (XmlNode node in doc.SelectNodes("parameters/val"))
                {
                    if (id == node.Attributes["id"].Value.ToString().Trim())
                    {
                        node.Attributes["value"].Value = value;
                    }
                }
            }
            catch (System.Xml.XmlException)
            {
                throw;
            }
        }
        //延期次数
        public static int GetDealyTimes()
        {
            return GetAppParametersCfg("MedicalRecordApplyDelayTimes");
        }
        //延期最大时间
        public static int GetDealyMaxTime()
        {
            return GetAppParametersCfg("MedicalRecordApplyDelayMaxTime");
        }
        //借阅时间
        public static int GetReadTime()
        {
            return GetAppParametersCfg("MedicalRecordApplyReadMaxTime");
        }
        //到期提醒时间
        public static int GetRemindTime()
        {
            return GetAppParametersCfg("MedicalRecordApplyRemindTime");
        }
        //最大单次病历申请数
        public static int GetApplyLimit()
        {
            return GetAppParametersCfg("MedicalRecordApplyAmountLimit");
        }
    }
}
