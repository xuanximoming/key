using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.FrameWork.WinForm;
using System.Data.SqlClient;
using DrectSoft.Wordbook;
using DrectSoft.Core;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Emr.TemplateFactory
{
    public partial class AddNewTemplate_Item : DevBaseForm, IStartPlugIn
    {
        public Emrtemplet_Item m_emrtemplet_item;
        IEmrHost m_app;
        SQLUtil m_sqlUtil;

        /// <summary>
        /// 判断是否是点击确认按钮
        /// </summary>
        public bool m_IsOK = false;

        /// <summary>
        /// 判断是否是新增模板
        /// </summary>
        public bool m_IsNew = true;

        private string m_MrCode = string.Empty;
        private string m_MrClass = string.Empty;
        private string m_MrName = string.Empty;
        private string m_DeptId = string.Empty;

        /// <summary>
        /// 初始化界面值
        /// </summary>
        /// <param name="dr"></param>
        public void InitValue(DataRow dr)
        {
            if (dr != null)
            {
                m_MrCode = dr["mr_code"].ToString();
                m_MrClass = dr["mr_class"].ToString();
                m_MrName = dr["mr_name"].ToString();
                m_DeptId = dr["dept_id"].ToString();
                m_IsNew = false;
            }
            else
            {
                m_IsNew = true;
            }
        }

        /// <summary>
        /// 设置默认值
        /// </summary>
        /// <param name="mr_class"></param>
        /// <param name="dept_id"></param>
        public void SetDefaultValue(string mr_class, string dept_id)
        {
            try
            {
                m_MrClass = mr_class;
                m_DeptId = dept_id;
                m_emrtemplet_item.MrClass = m_MrClass;
                m_emrtemplet_item.DeptId = m_DeptId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void SetControlValue()
        {
            if (!m_IsNew)
            {
                this.txtMR_Code.Text = m_MrCode;
                this.lookUpEditorMr_class.CodeValue = m_MrClass;
                this.txt_mr_name.Text = m_MrName;
                this.lookUpEditorDepartment.CodeValue = m_DeptId;
            }
        }

        public void GetDataRow(DataRow dr)
        {
            dr["mr_code"] = this.txtMR_Code.Text;
            dr["mr_class"] = this.lookUpEditorMr_class.CodeValue;
            dr["mr_name"] = this.txt_mr_name.Text;
            dr["dept_id"] = this.lookUpEditorDepartment.CodeValue;
        }

        public AddNewTemplate_Item(Emrtemplet_Item emrtemplet_item, IEmrHost app)
        {
            InitializeComponent();
            m_app = app;
            m_sqlUtil = new SQLUtil(app);
            m_emrtemplet_item = null == emrtemplet_item ? new Emrtemplet_Item() : emrtemplet_item;
        }

        /// <summary>
        /// 取消事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        /// </summary>
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 确定事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        /// </summary>
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                BindEmrTemplet();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 将页面信息绑定到Emrtemplet实体中
        /// edit by Yanqiao.Cai 2012-11-06
        /// 1、add try ... catch
        /// 2、提示信息
        /// </summary>
        private void BindEmrTemplet()
        {
            try
            {
                string errorStr = CheckItem();
                if (!string.IsNullOrEmpty(errorStr))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(errorStr);
                    return;
                }

                m_IsOK = true;
                m_emrtemplet_item = new Emrtemplet_Item();
                m_emrtemplet_item.MrCode = this.txtMR_Code.Text;
                m_emrtemplet_item.MrClass = this.lookUpEditorMr_class.CodeValue;
                m_emrtemplet_item.MrName = this.txt_mr_name.Text;
                //m_emrtemplet_item.QcCode = this.txt_qc_code.Text;
                m_emrtemplet_item.DeptId = this.lookUpEditorDepartment.CodeValue;

                //m_emrtemplet_item.WriteTimes = this.radgroupWRITE_TIMES.SelectedIndex;


                this.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 画面验证
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-06</date>
        /// </summary>
        private string CheckItem()
        {
            try
            {
                //必须填写模板名称
                if (this.txt_mr_name.Text.Trim().Length == 0)
                {
                    this.txt_mr_name.Focus();
                    return "模板名称不能为空";
                }
                //必须填写模板类型
                if (this.lookUpEditorMr_class.CodeValue == "")
                {
                    this.lookUpEditorMr_class.Focus();
                    return "请选择模板类型";
                }
                //必须填写模板科室
                if (this.lookUpEditorDepartment.CodeValue == "")
                {
                    this.lookUpEditorDepartment.Focus();
                    return "请选择所属科室";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return string.Empty;
        }

        /// <summary>
        /// 窗体加载事件
        /// 将实体数据加载到页面
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNewTemplate_Item_Load(object sender, EventArgs e)
        {
            try
            {
              
                InitDepartment();
                InitMRClass();

                if (!m_IsNew)//修改叶子节点
                {
                    SetControlValue();
                }
                else if (m_emrtemplet_item == null)
                    return;
                else
                {
                    this.txtMR_Code.Text = m_emrtemplet_item.MrCode;
                    this.lookUpEditorMr_class.CodeValue = m_emrtemplet_item.MrClass;
                    this.txt_mr_name.Text = m_emrtemplet_item.MrName;
                    //this.txt_qc_code.Text = m_emrtemplet_item.QcCode;
                    this.lookUpEditorDepartment.CodeValue = m_emrtemplet_item.DeptId;
                }

                //edit by cyq 2013-02-26
                if (m_IsNew)
                {
                    this.lookUpEditorMr_class.Enabled = true;
                    this.lookUpEditorDepartment.Enabled = true;
                }
                else
                {
                    this.lookUpEditorMr_class.Enabled = false;
                    this.lookUpEditorDepartment.Enabled = false;
                }
                ////可以选择模板类型
                //if (this.lookUpEditorMr_class.CodeValue == "")
                //{
                //    this.lookUpEditorMr_class.Enabled = true;
                //}
                //else
                //{
                //    this.lookUpEditorMr_class.Enabled = false;
                //}
                ////可以选择模板科室
                //if (this.lookUpEditorDepartment.CodeValue == "")
                //{
                //    this.lookUpEditorDepartment.Enabled = true;
                //}
                //else
                //{
                //    this.lookUpEditorDepartment.Enabled = false;
                //}
                this.txt_mr_name.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        public IPlugIn Run(IEmrHost host)
        {
            IPlugIn plg = new PlugIn(this.GetType().ToString(), this);
            m_app = host;

            return plg;
        }

        #region 初始下拉框
        /// <summary>
        /// 初始化科室
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        /// </summary>
        private void InitDepartment()
        {
            try
            {
                lookUpWindowDepartment.SqlHelper = m_app.SqlHelper;

                //yxy 暂时加载TP科室
                //DataTable Dept = m_app.SqlHelper.ExecuteDataTable("usp_GetMedicalRrecordViewFrm",
                //     new SqlParameter[] { new SqlParameter("@GetType", "1") }, CommandType.StoredProcedure);

                string sql = string.Format(@"select DEPT_ID ID,DEPT_NAME NAME,PY,WB from EMRDEPT ");
                DataTable Dept = m_app.SqlHelper.ExecuteDataTable(sql);

                Dept.Columns["ID"].Caption = "科室代码";
                Dept.Columns["NAME"].Caption = "科室名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 60);
                cols.Add("NAME", 90);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//NAME//PY//WB");
                lookUpEditorDepartment.SqlWordbook = deptWordBook;
                lookUpEditorDepartment.CodeValue = "0000";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 初始化文件类型
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        /// </summary>
        private void InitMRClass()
        {
            try
            {
                lookUpWindowDepartment.SqlHelper = m_app.SqlHelper;
                DataTable Dept = m_sqlUtil.ItemCatalogTable;
                new GenerateShortCode(m_app.SqlHelper).AutoAddShortCode(Dept, "NAME");

                Dept.Columns["ID"].Caption = "类型编码";
                Dept.Columns["NAME"].Caption = "类型名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 60);
                cols.Add("NAME", 90);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//NAME//PY//WB");
                lookUpEditorMr_class.SqlWordbook = deptWordBook;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
 
 
    }
}