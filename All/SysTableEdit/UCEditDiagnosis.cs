using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.SysTableEdit.DataEntity;
using DrectSoft.Wordbook;
using DrectSoft.Core;
using DrectSoft.Common;

namespace DrectSoft.SysTableEdit
{
    public partial class UCEditDiagnosis : DevExpress.XtraEditors.XtraUserControl, IStartPlugIn
    {
        private IEmrHost m_app;

        SysTableManger m_SysTableManger;

        DataTable m_DiagTable;

        /// <summary>
        /// 当前页面状态
        /// </summary>
        EditState m_EditState = EditState.None;

        public UCEditDiagnosis(IEmrHost app)
        {
            InitializeComponent();
            m_app = app;
            m_SysTableManger = new SysTableManger(m_app);
        }

        /// <summary>
        /// 初始化窗体事件
        /// edit by Yanqiao.Cai 2012-11-06
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
                //InitDiag();
                RefreshData();

                //初始化焦点
                lookUpEditorDiag.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }


        #region 初始化页面下拉框

        /// <summary>
        /// 初始所属统计分类
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        /// </summary>
        private void InitStatist()
        {
            try
            {
                lookUpWindowStatist.SqlHelper = m_app.SqlHelper;

                string sql = string.Format(@"select * from diseasecfg a where a.category = 700 ");
                DataTable Bzlb = m_app.SqlHelper.ExecuteDataTable(sql);

                Bzlb.Columns["ID"].Caption = "分类编码";
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
        /// 初始内部分类
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
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
                cols.Add("NAME", 120);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Bzlb, "ID", "NAME", cols, "ID//Name//PY//WB");
                lookUpEditorInnerCategory.SqlWordbook = deptWordBook;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 初始病种类别
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
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
                cols.Add("NAME", 120);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Bzlb, "ID", "NAME", cols, "ID//Name//PY//WB");
                lookUpEditorCategory.SqlWordbook = deptWordBook;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 初始其他类别
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
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
                cols.Add("NAME", 120);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Bzlb, "ID", "NAME", cols, "ID//Name//PY//WB");
                lookUpEditorOtherCategroy.SqlWordbook = deptWordBook;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 初始肿瘤
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
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
                cols.Add("NAME", 120);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Bzlb, "ID", "NAME", cols, "ID//Name//PY//WB");
                lookUpEditorTumorID.SqlWordbook = deptWordBook;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 初始诊断表
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InitDiag()
        {
            try
            {
                lookUpWindowDiag.SqlHelper = m_app.SqlHelper;

                if (m_DiagTable == null)
                    m_DiagTable = m_SysTableManger.GetDiagnosis_Table("");

                m_DiagTable.Columns["ICD"].Caption = "诊断编码";
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

        #endregion


        #region 方法

        /// <summary>
        /// 刷新页面数据
        /// </summary>
        private void RefreshData()
        {
            BindGrid();
            m_EditState = EditState.View;
            BtnState();
            ClearPageValue();
        }

        /// <summary>
        /// 绑定grid值
        /// </summary>
        /// edit by Yanqiao.Cai 2013-02-22
        /// 1、add try ... catch
        /// 2、添加过滤
        private void BindGrid()
        {
            try
            {
                //DataTable dt = m_SysTableManger.GetDiagnosis_Table("");

                //edit by cyq 2013-02-25 数据即时刷新
                m_DiagTable = m_SysTableManger.GetDiagnosis_Table("");
                //if (m_DiagTable == null) 
                    //m_DiagTable = m_SysTableManger.GetDiagnosis_Table("");
                this.gridControl1.DataSource = m_DiagTable;
                if (!string.IsNullOrEmpty(this.txt_search.Text))
                {
                    FilterDataSource();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

            this.txtMarkID.Text = diagnosis.Markid.Trim();
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
            this.cmbValid.SelectedIndex = diagnosis.Valid;
            this.txtMemo.Text = diagnosis.Memo.Trim();
        }

        /// <summary>
        /// 设置编辑区域值
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-07</date>
        /// <param name="dr"></param>
        /// </summary>
        private void SetPageValueByDataRow(DataRow dr)
        {
            try
            {
                if (null != dr && dr.ItemArray.Length >= 14)
                {
                    this.txtMarkID.Text = null == dr["MARKID"] ? "" : dr["MARKID"].ToString();
                    this.txtICD.Text = null == dr["ICD"] ? "" : dr["ICD"].ToString();
                    this.txtMapID.Text = null == dr["MAPID"] ? "" : dr["MAPID"].ToString();
                    this.txtStandardCode.Text = null == dr["STANDARDCODE"] ? "" : dr["STANDARDCODE"].ToString();
                    this.txtName.Text = null == dr["NAME"] ? "" : dr["NAME"].ToString();

                    this.txtPy.Text = null == dr["PY"] ? "" : dr["PY"].ToString();
                    this.txtWb.Text = null == dr["WB"] ? "" : dr["WB"].ToString();
                    this.lookUpEditorTumorID.CodeValue = null == dr["TUMORID"] ? "" : dr["TUMORID"].ToString();
                    this.lookUpEditorStatist.CodeValue = null == dr["STATISTID"] ? "" : dr["STATISTID"].ToString();
                    this.lookUpEditorInnerCategory.CodeValue = null == dr["INNERCATEGORYID"] ? "" : dr["INNERCATEGORYID"].ToString();

                    this.lookUpEditorCategory.CodeValue = null == dr["CATEGORYID"] ? "" : dr["CATEGORYID"].ToString();
                    this.lookUpEditorOtherCategroy.CodeValue = null == dr["OTHERCATEGROYID"] ? "" : dr["OTHERCATEGROYID"].ToString();
                    this.cmbValid.SelectedIndex = (null == dr["VALID"] || dr["VALID"].ToString().Trim() == "") ? -1 : int.Parse(dr["VALID"].ToString());
                    this.txtMemo.Text = null == dr["MEMO"] ? "" : dr["MEMO"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
            diagnosis.Valid = cmbValid.SelectedIndex;
            diagnosis.Memo = this.txtMemo.Text.Trim();

            return diagnosis;
        }

        /// <summary>
        /// 保存诊断值
        /// </summary>
        /// <param name="diag"></param>
        /// <returns></returns>
        private bool SaveDiagnosis(Diagnosis diag)
        {
            try
            {
                string edittype = "";
                if (m_EditState == EditState.Add)
                    edittype = "1";
                else
                    edittype = "2";
                m_SysTableManger.SaveDiagnosis(diag, edittype);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

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
            cmbValid.SelectedIndex = -1;
            this.txtMemo.Text = "";
        }

        /// <summary>
        /// 重置方法
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-16</date>
        /// </summary>
        private void Reset()
        {
            try
            {
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
                cmbValid.SelectedIndex = -1;
                this.txtMemo.Text = "";
                if (m_EditState == EditState.Add)
                {
                    this.txtMarkID.Text = "";
                    this.txtMarkID.Focus();
                }
                else
                {
                    this.txtICD.Focus();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 控制页面控件状态
        /// </summary>
        private void BtnState()
        {
            if (m_EditState == EditState.View)
            {
                this.btnADD.Enabled = true;
                this.btnDel.Enabled = true;
                this.BtnEdit.Enabled = true;

                this.btnSave.Enabled = false;
                this.btn_reset.Enabled = false;
                this.BtnClear.Enabled = false;

                this.txtMarkID.Enabled = false;
                this.txtICD.Enabled = false;
                this.txtMapID.Enabled = false;
                this.txtStandardCode.Enabled = false;
                this.txtName.Enabled = false;

                //this.txtPy.Enabled = false;
                //this.txtWb.Enabled = false;
                this.lookUpEditorTumorID.Enabled = false;
                this.lookUpEditorStatist.Enabled = false;
                this.lookUpEditorInnerCategory.Enabled = false;

                this.lookUpEditorCategory.Enabled = false;
                this.lookUpEditorOtherCategroy.Enabled = false;
                cmbValid.Enabled = false;
                this.txtMemo.Enabled = false;
            }
            else if (m_EditState == EditState.Add || m_EditState == EditState.Edit)
            {
                this.btnADD.Enabled = false;
                this.btnDel.Enabled = false;
                this.BtnEdit.Enabled = false;

                this.btnSave.Enabled = true;
                this.btn_reset.Enabled = true;
                this.BtnClear.Enabled = true;

                if (m_EditState == EditState.Add)
                    this.txtMarkID.Enabled = true;
                else
                    this.txtMarkID.Enabled = false;
                this.txtICD.Enabled = true;
                this.txtMapID.Enabled = true;
                this.txtStandardCode.Enabled = true;
                this.txtName.Enabled = true;

                //this.txtPy.Enabled = true;
                //this.txtWb.Enabled = true;
                this.lookUpEditorTumorID.Enabled = true;
                this.lookUpEditorStatist.Enabled = true;
                this.lookUpEditorInnerCategory.Enabled = true;

                this.lookUpEditorCategory.Enabled = true;
                this.lookUpEditorOtherCategroy.Enabled = true;
                cmbValid.Enabled = true;
                this.txtMemo.Enabled = true;
            }

        }

        /// <summary>
        /// 验证页面数据是否可以保存
        /// edit by Yanqiao.Cai 2012-11-06
        /// 1、add try ... catch
        /// 2、修改提示信息
        /// 3、逻辑调整
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <summary>
        private bool IsSave()
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtMarkID.Text.Trim()))
                {
                    m_app.CustomMessageBox.MessageShow("诊断标识不能为空");
                    this.txtMarkID.Focus();
                    return false;
                }
                else if (m_EditState == EditState.Add && !m_SysTableManger.CheckDiagnosisID(this.txtMarkID.Text.Trim()))
                {
                    m_app.CustomMessageBox.MessageShow("该诊断标识已存在，请重新输入。");
                    this.txtMarkID.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(this.txtICD.Text.Trim()))
                {
                    m_app.CustomMessageBox.MessageShow("诊断代码不能为空");
                    this.txtICD.Focus();
                    return false;
                }
                else if (m_EditState == EditState.Add && HasICD(this.txtICD.Text.Trim()))
                {
                    m_app.CustomMessageBox.MessageShow("该诊断ICD已存在，请重新输入。");
                    this.txtICD.Focus();
                    return false;
                }
                
                if (string.IsNullOrEmpty(this.txtName.Text.Trim()))
                {
                    m_app.CustomMessageBox.MessageShow("诊断名称不能为空");
                    this.txtName.Focus();
                    return false;
                }
                if (this.cmbValid.SelectedIndex == -1)
                {
                    m_app.CustomMessageBox.MessageShow("请选择是否有效");
                    this.cmbValid.Focus();
                    return false;
                }
                //if (this.lookUpEditorStatist.CodeValue.Trim() == "")
                //{
                //    m_app.CustomMessageBox.MessageShow("请输入所属统计分类！");
                //    return false;
                //}

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool HasICD(string icdID)
        {
            string sql = string.Format(@"select count(*) from diagnosis where  icd='{0}'", icdID);
           object count= DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteScalar(sql, CommandType.Text);
           if (Convert.ToInt32(count) > 0)
           {
               return true;
           }
           return false;
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

        #endregion

        public IPlugIn Run(FrameWork.WinForm.Plugin.IEmrHost host)
        {
            throw new NotImplementedException();
        }
        #region 事件
        /// <summary>
        /// 新增事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// 1、add try ... catch
        /// 2、获取焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTypeADD_Click(object sender, EventArgs e)
        {
            try
            {
                m_EditState = EditState.Add;
                ClearPageValue();
                BtnState();
                this.txtMarkID.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 删除事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// 1、add try ... catch
        /// 2、添加提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTypeDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridView1.FocusedRowHandle < 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return;
                }
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                if (null == dr || dr.ItemArray.Length <= 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return;
                }
                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要删除诊断 " + dr["NAME"] + " 吗？", "删除诊断", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.Cancel)
                {
                    return;
                }

                //string markid = this.txtMarkID.Text.Trim();
                //if (markid == "")
                //{
                //    m_app.CustomMessageBox.MessageShow("请选择要删除的记录");
                //    return;
                //}
                if (m_SysTableManger.DelDiagnosis(dr["MARKID"].ToString()))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("删除成功");
                    //edit by cyq 2013-02-25
                    gridView1.DeleteRow(gridView1.FocusedRowHandle);
                    ClearPageValue();
                    //RefreshData();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 编辑事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// 1、add try ... catch
        /// 2、添加提示
        /// 3、获取焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTypeEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridView1.FocusedRowHandle < 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return;
                }
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                if (null == dr || dr.ItemArray.Length <= 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return;
                }
                SetPageValueByDataRow(dr);

                m_EditState = EditState.Edit;
                BtnState();
                this.txtICD.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 保存事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// 1、add try ... catch
        /// 2、添加提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTypeSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsSave())
                {
                    return;
                }
                if (SaveDiagnosis(SetEntityByPageValue()))
                {
                    if (m_EditState == EditState.Add)
                    {
                        m_app.CustomMessageBox.MessageShow("新增诊断成功");
                    }
                    else
                    {
                        m_app.CustomMessageBox.MessageShow("修改诊断成功");
                    }
                    RefreshData();
                }
                else
                {
                    if (m_EditState == EditState.Add)
                    {
                        m_app.CustomMessageBox.MessageShow("新增诊断失败");
                    }
                    else
                    {
                        m_app.CustomMessageBox.MessageShow("修改诊断失败");
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 重置事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-16</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_reset_Click(object sender, EventArgs e)
        {
            try
            {
                Reset();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 取消事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTypeClear_Click(object sender, EventArgs e)
        {
            try
            {
                ClearPageValue();
                m_EditState = EditState.View;
                BtnState();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
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

            BtnState();
        }

        #endregion

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

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            GetPYWB(this.txtName.Text);
        }

        /// <summary>
        /// 序号
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-06</date>
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
        /// 检索事件
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-02-22</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_search_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                FilterDataSource();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 检索方法
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-02-22</date>
        private void FilterDataSource()
        {
            try
            {
                DataTable dt = (DataTable)gridControl1.DataSource;
                if (null == dt || dt.Rows.Count == 0)
                {
                    return;
                }
                string searchStr = DS_Common.FilterSpecialCharacter(this.txt_search.Text);
                string filterStr = string.Format(" MARKID like '%{0}%' or ICD like '%{0}%' or MAPID like '%{0}%' or STANDARDCODE like '%{0}%' or NAME like '%{0}%' or py like '%{0}%' or wb like '%{0}%' or TUMORNAME like '%{0}%' or OTHERCATEGROYNAME like '%{0}%' or STATISTNAME like '%{0}%' or INNERCATEGORYNAME like '%{0}%' or CATEGORYNAME like '%{0}%' ", searchStr);
                dt.DefaultView.RowFilter = filterStr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnWeiHu_Click(object sender, EventArgs e)
        {
            try
            {
               DataRow dr= gridView1.GetFocusedDataRow();
               if (dr == null)
               {
                   DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("未选中记录");
                   return;
               }
               DiagnosisOtherNaem diagnosisOtherNaem = new DiagnosisOtherNaem(dr["ICD"].ToString(), "1");
               diagnosisOtherNaem.ShowDialog();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        
    }
}
