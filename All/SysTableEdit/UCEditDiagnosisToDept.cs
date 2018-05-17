using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.SysTableEdit.DataEntity;
using DrectSoft.Wordbook;
using DrectSoft.Core;
using DevExpress.XtraTreeList.Nodes;
using DrectSoft.Common;

namespace DrectSoft.SysTableEdit
{
    public partial class UCEditDiagnosisToDept : DevExpress.XtraEditors.XtraUserControl, IStartPlugIn
    {
        private IEmrHost m_app;

        SysTableManger m_SysTableManger;

        DataTable m_DiagTable;

        /// <summary>
        /// 记录下编辑的科室编号
        /// </summary>
        string m_DeptID;

        /// <summary>
        /// 当前页面状态
        /// </summary>
        EditState m_EditState = EditState.None;

        public UCEditDiagnosisToDept(IEmrHost app)
        {
            InitializeComponent();
            m_app = app;
            m_SysTableManger = new SysTableManger(m_app);
        }

        /// <summary>
        /// 窗体加载事件
        /// edit by Yanqiao.Cai 2012-11-07
        /// 1、add try ... catch
        /// 2、初始化焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCEditDiagnosis_Load(object sender, EventArgs e)
        {
            try
            {
                InitStatist();
                InitInnerCategory();
                InitBzlb();
                InitOtherCategroy();
                InitTumorID();
                InitDiag();
                MakeDeptTree();
                RefreshData();
                this.lookUpEditorDiag.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }


        #region 初始化页面下拉框

        /// <summary>
        /// 初始化所属统计分类
        /// edit by Yanqiao.Cai 2012-11-07
        /// 1、add try ... catch
        /// 2、优化下拉列表
        /// </summary>
        private void InitStatist()
        {
            try
            {
                lookUpWindowStatist.SqlHelper = m_app.SqlHelper;

                string sql = string.Format(@"select * from diseasecfg a where a.category = 700 ");
                DataTable Bzlb = m_app.SqlHelper.ExecuteDataTable(sql);

                Bzlb.Columns["ID"].Caption = "分类代码";
                Bzlb.Columns["NAME"].Caption = "分类名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 60);
                cols.Add("NAME", 90);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Bzlb, "ID", "NAME", cols, "ID//Name//PY//WB");
                lookUpEditorStatist.SqlWordbook = deptWordBook;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 初始化内部分类
        /// edit by Yanqiao.Cai 2012-11-07
        /// 1、add try ... catch
        /// 2、优化下拉列表
        /// </summary>
        private void InitInnerCategory()
        {
            try
            {
                lookUpWindowInnerCategory.SqlHelper = m_app.SqlHelper;

                string sql = string.Format(@"select * from diseasecfg a where a.category = 702 ");
                DataTable Bzlb = m_app.SqlHelper.ExecuteDataTable(sql);

                Bzlb.Columns["ID"].Caption = "分类代码";
                Bzlb.Columns["NAME"].Caption = "分类名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 60);
                cols.Add("NAME", 90);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Bzlb, "ID", "NAME", cols, "ID//Name//PY//WB");
                lookUpEditorInnerCategory.SqlWordbook = deptWordBook;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 初始化病种类别
        /// edit by Yanqiao.Cai 2012-11-07
        /// 1、add try ... catch
        /// 2、优化下拉列表
        /// </summary>
        private void InitBzlb()
        {
            try
            {
                lookUpWindowCategory.SqlHelper = m_app.SqlHelper;

                string sql = string.Format(@"select * from diseasecfg a where a.category = 701 ");
                DataTable Bzlb = m_app.SqlHelper.ExecuteDataTable(sql);

                Bzlb.Columns["ID"].Caption = "病种代码";
                Bzlb.Columns["NAME"].Caption = "病种名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 60);
                cols.Add("NAME", 90);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Bzlb, "ID", "NAME", cols, "ID//Name//PY//WB");
                lookUpEditorCategory.SqlWordbook = deptWordBook;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 初始化其他类别
        /// edit by Yanqiao.Cai 2012-11-07
        /// 1、add try ... catch
        /// 2、优化下拉列表
        /// </summary>
        private void InitOtherCategroy()
        {
            try
            {
                lookUpWindowOtherCategroy.SqlHelper = m_app.SqlHelper;

                string sql = string.Format(@"select * from categorydetail a where a.categoryid = 9 ");
                DataTable Bzlb = m_app.SqlHelper.ExecuteDataTable(sql);

                Bzlb.Columns["ID"].Caption = "类别代码";
                Bzlb.Columns["NAME"].Caption = "类别名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 60);
                cols.Add("NAME", 90);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Bzlb, "ID", "NAME", cols, "ID//Name//PY//WB");
                lookUpEditorOtherCategroy.SqlWordbook = deptWordBook;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 初始化肿瘤
        /// edit by Yanqiao.Cai 2012-11-07
        /// 1、add try ... catch
        /// 2、优化下拉列表
        /// </summary>
        private void InitTumorID()
        {
            try
            {
                lookUpWindowTumorID.SqlHelper = m_app.SqlHelper;

                string sql = string.Format(@"select * from Tumor  ");
                DataTable Bzlb = m_app.SqlHelper.ExecuteDataTable(sql);

                Bzlb.Columns["ID"].Caption = "类别代码";
                Bzlb.Columns["NAME"].Caption = "类别名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 60);
                cols.Add("NAME", 90);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Bzlb, "ID", "NAME", cols, "ID//Name//PY//WB");
                lookUpEditorTumorID.SqlWordbook = deptWordBook;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 初始化诊断表
        /// edit by Yanqiao.Cai 2012-11-07
        /// 1、add try ... catch
        /// 2、优化下拉列表
        /// </summary>
        private void InitDiag()
        {
            try
            {
                lookUpWindowDiag.SqlHelper = m_app.SqlHelper;

                if (m_DiagTable == null)
                    m_DiagTable = m_SysTableManger.GetDiagnosis_Table("");

                m_DiagTable.Columns["ICD"].Caption = "ICD";
                m_DiagTable.Columns["NAME"].Caption = "诊断名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ICD", 80);
                cols.Add("NAME", 170);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", m_DiagTable, "ICD", "NAME", cols, "MARKID//ICD//Name//PY//WB");
                lookUpEditorDiag.SqlWordbook = deptWordBook;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 科室列表
        /// edit by Yanqiao.Cai 2012-11-07
        /// add try ... catch
        /// </summary>
        private void MakeDeptTree()
        {
            try
            {
                string sql = string.Format(@"select a.id, a.name
                                            from department a
                                            where exists (select 1
                                                    from dept2ward b
                                                    where a.id = b.deptid)");
                DataTable dt = m_app.SqlHelper.ExecuteDataTable(sql, CommandType.Text);

                TreeListNode node = null;//
                foreach (DataRow row in dt.Rows)
                {
                    node = treeList_Detp.AppendNode(new object[] { row["NAME"].ToString(), row["ID"].ToString() }, null);
                    node.Tag = row;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion


        #region 方法

        /// <summary>
        /// 刷新页面数据
        /// </summary>
        private void RefreshData()
        {
            BindGrid();
            m_EditState = EditState.View;
            ClearPageValue();
        }

        /// <summary>
        /// 绑定grid值
        /// </summary>
        private void BindGrid()
        {
            //DataTable dt = m_SysTableManger.GetDiagnosis_Table("");
            if (m_DiagTable == null)
                m_DiagTable = m_SysTableManger.GetDiagnosis_Table("");
            this.gridControl1.DataSource = m_DiagTable;
        }

        /// <summary>
        /// 将gridview中对应行值加载到实体中
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private Diagnosis SetEntityByDataRow(DataRow dr)
        {
            if (dr == null)
            {
                return null;
            }
            Diagnosis diagnosis = new Diagnosis();

            diagnosis.Markid = dr["Markid"].ToString();
            diagnosis.Icd = dr["Icd"].ToString();
            diagnosis.Mapid = dr["Mapid"].ToString();
            diagnosis.Standardcode = dr["Standardcode"].ToString();
            diagnosis.Name = dr["Name"].ToString();

            diagnosis.Py = dr["Py"].ToString();
            diagnosis.Wb = dr["Wb"].ToString();
            diagnosis.Tumorid = dr["Tumorid"].ToString();
            diagnosis.Statist = dr["Statist"].ToString();
            diagnosis.Innercategory = dr["Innercategory"].ToString();

            diagnosis.Category = dr["Category"].ToString();
            diagnosis.Othercategroy = dr["Othercategroy"].ToString();
            diagnosis.Valid = Convert.ToInt32(dr["Valid"].ToString());
            diagnosis.Memo = dr["Memo"].ToString();

            return diagnosis;
        }

        /// <summary>
        /// 将诊断实体中值加载到页面
        /// </summary>
        /// <param name="diagnosis"></param>
        private void SetPageValue(Diagnosis diagnosis)
        {
            if (diagnosis == null || diagnosis.Markid == "")
                return;

            txtMarkID.Text = diagnosis.Markid.Trim();
            this.txtICD.Text = diagnosis.Icd.Trim();
            this.txtMapID.Text = diagnosis.Mapid.Trim();
            this.txtStandardCode.Text = diagnosis.Standardcode.Trim();
            this.txtName.Text = diagnosis.Name.Trim();

            this.txtPy.Text = diagnosis.Py.Trim();
            this.txtWb.Text = diagnosis.Wb.Trim();
            this.lookUpEditorTumorID.CodeValue = diagnosis.Tumorid.Trim();
            this.lookUpEditorStatist.CodeValue = diagnosis.Statist.Trim();
            this.lookUpEditorInnerCategory.CodeValue = diagnosis.Innercategory.Trim();

            this.lookUpEditorCategory.CodeValue = diagnosis.Category.Trim();
            this.lookUpEditorOtherCategroy.CodeValue = diagnosis.Othercategroy.ToString().Trim();
            this.txtMemo.Text = diagnosis.Memo.Trim();
        }

        /// <summary>
        /// 将页面值加入到诊断实体中
        /// </summary>
        /// <returns></returns>
        private Diagnosis SetEntityByPageValue()
        {
            Diagnosis diagnosis = new Diagnosis();


            diagnosis.Markid = txtMarkID.Text.Trim();
            diagnosis.Icd = this.txtICD.Text.Trim();
            diagnosis.Mapid = this.txtMapID.Text.Trim();
            diagnosis.Standardcode = this.txtStandardCode.Text.Trim();
            diagnosis.Name = this.txtName.Text.Trim();

            diagnosis.Py = this.txtPy.Text.Trim();
            diagnosis.Wb = this.txtWb.Text.Trim();
            diagnosis.Tumorid = this.lookUpEditorTumorID.CodeValue.Trim();
            diagnosis.Statist = this.lookUpEditorStatist.CodeValue.Trim();
            diagnosis.Innercategory = this.lookUpEditorInnerCategory.CodeValue.Trim();

            diagnosis.Category = this.lookUpEditorCategory.CodeValue.Trim();
            diagnosis.Othercategroy = this.lookUpEditorOtherCategroy.CodeValue.Trim();
            diagnosis.Memo = this.txtMemo.Text.Trim();

            return diagnosis;
        }


        /// <summary>
        /// 清空页面空间值
        /// </summary>
        private void ClearPageValue()
        {
            this.txtMarkID.Text = "";
            this.txtICD.Text = "";
            this.txtMapID.Text = "";
            this.txtStandardCode.Text = "";
            this.txtName.Text = "";

            this.txtPy.Text = "";
            this.txtWb.Text = "";
            this.lookUpEditorTumorID.CodeValue = "";
            this.lookUpEditorStatist.CodeValue = "";
            this.lookUpEditorInnerCategory.CodeValue = "";

            this.lookUpEditorCategory.CodeValue = "";
            this.lookUpEditorOtherCategroy.CodeValue = "";
            this.txtMemo.Text = "";
        }

        /// <summary>
        /// 得到拼音和五笔
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private void GetPYWB(string name)
        {
            GenerateShortCode shortCode = new GenerateShortCode(m_app.SqlHelper);
            string[] code = shortCode.GenerateStringShortCode(name);

            //string py = code[0]; //PY
            //string wb = code[1]; //WB
            //return code;
            this.txtPy.Text = code[0].ToString();
            this.txtWb.Text = code[1].ToString();
        }


        /// <summary>
        /// 保存科室常用诊断
        /// </summary>
        /// edit by Yanqiao.Cai 2013-02-25
        /// 1、add try ... catch
        /// 2、保存方法优化
        /// <param name="deptid"></param>
        private void DoSave(string deptid)
        {
            try
            {
                if (string.IsNullOrEmpty(deptid))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条科室记录");
                    treeList_Detp.Focus();
                    return;
                }
                DataTable datasouce = (DataTable)gridControl2.DataSource;
                if (null == datasouce || datasouce.Rows.Count == 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请先添加科室常用诊断");
                    return;
                }

                //edit by Yanqiao.Cai 2013-02-22
                string markid = DS_Common.CombineSQLStringByList(datasouce.Select(" 1=1 ").Select(p => p["MARKID"].ToString()).ToList());
                //string markid = "''";
                //foreach (DataRow dr in datasouce.Rows)
                //{
                //    markid = markid + ",'" + dr["MARKID"] + "'";
                //}
                if (m_SysTableManger.DoSaveDeptDiag(deptid, markid))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("保存成功");
                    m_EditState = EditState.View;
                }
                else
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("保存失败");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 绑定科室常用诊断
        /// </summary>
        /// <param name="deptid"></param>
        private void GetDeptDiag(string deptid)
        {
            DataTable dt = m_SysTableManger.GetDeptDiagnosis_Table(deptid);

            gridControl2.DataSource = dt;
        }


        #endregion

        public IPlugIn Run(FrameWork.WinForm.Plugin.IEmrHost host)
        {
            throw new NotImplementedException();
        }

        #region 事件
        /// <summary>
        /// 新增按钮
        /// </summary>
        /// edit by Yanqiao.Cai 2013-02-22
        /// 1、add try ... catch
        /// 2、优化保存方法
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTypeADD_Click(object sender, EventArgs e)
        {
            try
            {
                //edit by cyq 2013-02-25
                DoSave(m_DeptID);
                //if (m_EditState == EditState.Edit)
                //{
                //    DoSave(m_DeptID);
                //}
                //m_EditState = EditState.View;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControl1_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0)
                return;
            DataRow foucesRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (foucesRow == null)
                return;

            if (foucesRow.IsNull("Markid"))
                return;

            SetPageValue(SetEntityByDataRow(foucesRow));
            m_EditState = EditState.View;

        }

        private void lookUpEditorDiag_CodeValueChanged(object sender, EventArgs e)
        {
            if (lookUpEditorDiag.CodeValue != "")
            {
                DataTable dt = m_DiagTable.Clone();
                foreach (DataRow dr in m_DiagTable.Select(string.Format("ICD='{0}'", lookUpEditorDiag.CodeValue)))
                {
                    dt.Rows.Add(dr.ItemArray);
                }
                gridControl1.DataSource = dt;

                gridControl1_Click(null, null);
            }
            else
            {
                gridControl1.DataSource = m_DiagTable;
            }

        }

        /// <summary>
        /// 选中科室切换科室诊断列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList_Detp_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            if (e.Node == null)
            {
                return;
            }
            if (e.Node.Focused)
            {
                TreeListNode node = e.Node;
                string deptid = e.Node.GetValue("ID").ToString();

                if (m_EditState == EditState.Edit)
                    if (m_app.CustomMessageBox.MessageShow(string.Format("您有数据未保存，是否保存？"), CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
                    {
                        DoSave(deptid);
                    }

                GetDeptDiag(deptid);

                m_EditState = EditState.View;
                m_DeptID = deptid;
            }
        }

        /// <summary>
        /// 双击将医院诊断库信息加入到科室常用诊断科
        /// </summary>
        /// edit by Yanqiao.Cai 2013-02-25
        /// 1、add try ... catch
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (gridView1.FocusedRowHandle < 0)
                    return;
                DataRow foucesRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                if (foucesRow == null)
                    return;

                if (foucesRow.IsNull("Markid"))
                    return;

                SetPageValue(SetEntityByDataRow(foucesRow));

                DataTable deptdatasouce = (DataTable)gridControl2.DataSource;

                //edit by cyq 2013-02-25
                var singleRows = deptdatasouce.Select(" MARKID = '" + foucesRow["MARKID"].ToString() + "' ");
                if (null != singleRows && singleRows.Length > 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("【" + singleRows[0]["NAME"].ToString() + "】诊断已经存在");
                    return;
                }
                //foreach (DataRow dr in deptdatasouce.Rows)
                //{
                //    if (dr["MARKID"].ToString() == foucesRow["MARKID"].ToString())
                //    {
                //        m_app.CustomMessageBox.MessageShow("【" + dr["NAME"].ToString() + "】诊断已经存在");
                //        return;
                //    }
                //}

                //edit by cyq 2013-02-25
                deptdatasouce.Rows.Add(SetNewRowData(deptdatasouce.NewRow(),foucesRow));
                //deptdatasouce.Rows.Add(foucesRow.ItemArray);

                m_EditState = EditState.Edit;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置新的数据datarow
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-02-25</date>
        /// <param name="newRow"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private DataRow SetNewRowData(DataRow newRow,DataRow row)
        {
            try
            {
                if (newRow.Table.Columns.Contains("MARKID") && row.Table.Columns.Contains("MARKID"))
                {
                    newRow["MARKID"] = row["MARKID"];
                }
                if (newRow.Table.Columns.Contains("ICD") && row.Table.Columns.Contains("ICD"))
                {
                    newRow["ICD"] = row["ICD"];
                }
                if (newRow.Table.Columns.Contains("MAPID") && row.Table.Columns.Contains("MAPID"))
                {
                    newRow["MAPID"] = row["MAPID"];
                }
                if (newRow.Table.Columns.Contains("STANDARDCODE") && row.Table.Columns.Contains("STANDARDCODE"))
                {
                    newRow["STANDARDCODE"] = row["STANDARDCODE"];
                }
                if (newRow.Table.Columns.Contains("NAME") && row.Table.Columns.Contains("NAME"))
                {
                    newRow["NAME"] = row["NAME"];
                }
                if (newRow.Table.Columns.Contains("PY") && row.Table.Columns.Contains("PY"))
                {
                    newRow["PY"] = row["PY"];
                }
                if (newRow.Table.Columns.Contains("WB") && row.Table.Columns.Contains("WB"))
                {
                    newRow["WB"] = row["WB"];
                }
                if (newRow.Table.Columns.Contains("tumorid") && row.Table.Columns.Contains("tumorid"))
                {
                    newRow["tumorid"] = row["tumorid"];
                }
                if (newRow.Table.Columns.Contains("TUMORNAME") && row.Table.Columns.Contains("TUMORNAME"))
                {
                    newRow["TUMORNAME"] = row["TUMORNAME"];
                }
                if (newRow.Table.Columns.Contains("STATIST") && row.Table.Columns.Contains("STATIST"))
                {
                    newRow["STATIST"] = row["STATIST"];
                }
                if (newRow.Table.Columns.Contains("STATISTNAME") && row.Table.Columns.Contains("STATISTNAME"))
                {
                    newRow["STATISTNAME"] = row["STATISTNAME"];
                }
                if (newRow.Table.Columns.Contains("INNERCATEGORY") && row.Table.Columns.Contains("INNERCATEGORY"))
                {
                    newRow["INNERCATEGORY"] = row["INNERCATEGORY"];
                }
                if (newRow.Table.Columns.Contains("INNERCATEGORYNAME") && row.Table.Columns.Contains("INNERCATEGORYNAME"))
                {
                    newRow["INNERCATEGORYNAME"] = row["INNERCATEGORYNAME"];
                }
                if (newRow.Table.Columns.Contains("CATEGORY") && row.Table.Columns.Contains("CATEGORY"))
                {
                    newRow["CATEGORY"] = row["CATEGORY"];
                }
                if (newRow.Table.Columns.Contains("CATEGORYNAME") && row.Table.Columns.Contains("CATEGORYNAME"))
                {
                    newRow["CATEGORYNAME"] = row["CATEGORYNAME"];
                }
                if (newRow.Table.Columns.Contains("OTHERCATEGROY") && row.Table.Columns.Contains("OTHERCATEGROY"))
                {
                    newRow["OTHERCATEGROY"] = row["OTHERCATEGROY"];
                }
                if (newRow.Table.Columns.Contains("OTHERCATEGROYNAME") && row.Table.Columns.Contains("OTHERCATEGROYNAME"))
                {
                    newRow["OTHERCATEGROYNAME"] = row["OTHERCATEGROYNAME"];
                }
                if (newRow.Table.Columns.Contains("VALID") && row.Table.Columns.Contains("VALID"))
                {
                    newRow["VALID"] = row["VALID"];
                }
                if (newRow.Table.Columns.Contains("VALIDNAME") && row.Table.Columns.Contains("VALIDNAME"))
                {
                    newRow["VALIDNAME"] = row["VALIDNAME"];
                }
                if (newRow.Table.Columns.Contains("MEMO") && row.Table.Columns.Contains("MEMO"))
                {
                    newRow["MEMO"] = row["MEMO"];
                }

                return newRow;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 科室诊断双击 移除诊断
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControl2_DoubleClick(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0)
                return;
            DataRow foucesRow = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            if (foucesRow == null)
                return;

            if (foucesRow.IsNull("Markid"))
                return;
            m_EditState = EditState.Edit;
            DataTable deptdatasouce = (DataTable)gridControl2.DataSource;
            foucesRow.Delete();
            deptdatasouce.AcceptChanges();
        }

        /// <summary>
        /// 序号 --- 医院诊断库
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-07</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 序号 --- 科室常用诊断
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-07</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView3_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        #endregion

        
    }
}
