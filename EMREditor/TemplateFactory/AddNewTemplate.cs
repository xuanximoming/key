using DevExpress.XtraTreeList.Nodes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Core;
using DrectSoft.Emr.TemplateFactory.BaseDataMaintain;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Wordbook;

namespace DrectSoft.Emr.TemplateFactory
{
    public partial class AddNewTemplate : DevBaseForm, IStartPlugIn
    {
        public Emrtemplet m_emrtemplet;
        IEmrHost m_app;
        DataTable m_depts;

        /// <summary>
        /// 判断是否是点击确认按钮
        /// </summary>
        public bool m_IsOK = false;

        /// <summary>
        /// 判断是否是新增模板
        /// </summary>
        public bool m_IsNew = true;
        /// <summary>
        /// 判断是否显示页面设置复选框edit by ywk 2012年3月31日14:07:06
        /// </summary>
        public bool m_IsShowPageConfig = true;//默认显示

        private string m_TempletId = string.Empty;
        private string m_MrClass = string.Empty;
        private string m_MrName = string.Empty;
        private string m_QcCode = string.Empty;
        private string m_DeptId = string.Empty;
        private int m_WriteTimes = -1;
        private int m_NewPageFlag = -1;
        private string m_IsFirstDaily = string.Empty;
        private string m_DailyTitle = string.Empty;//病程名称
        private string m_IsShowDailyTitle = string.Empty;//是否显示病程名称
        private string m_IsYiHuanGouTong = string.Empty;

        private string m_IsConfigPageSize = string.Empty;//是否读取页面配置

        private int m_NEW_PAGE_END = -1;
        private string m_state = "";
        private bool m_ShowStar = false;



        /// <summary>
        /// 初始化界面值
        /// </summary>
        /// <param name="dr"></param>
        public void InitValue(DataRow dr)
        {
            if (dr != null)
            {
                m_TempletId = null == dr["templet_id"] ? "" : dr["templet_id"].ToString();
                m_MrClass = null == dr["mr_class"] ? "" : dr["mr_class"].ToString();
                m_MrName = null == dr["mr_name"] ? "" : dr["mr_name"].ToString();
                m_QcCode = null == dr["qc_code"] ? "" : dr["qc_code"].ToString();
                m_DeptId = null == dr["dept_id"] ? "" : dr["dept_id"].ToString();
                m_WriteTimes = Convert.ToInt32(dr["write_times"].ToString());
                m_NewPageFlag = Convert.ToInt32(dr["new_page_flag"]);
                m_IsFirstDaily = null == dr["isfirstdaily"] ? "" : dr["isfirstdaily"].ToString();
                m_IsNew = false;
                m_DailyTitle = null == dr["file_name"] ? "" : dr["file_name"].ToString();
                m_IsShowDailyTitle = null == dr["isshowfilename"] ? "" : dr["isshowfilename"].ToString();
                m_IsYiHuanGouTong = null == dr["isyihuangoutong"] ? "" : dr["isyihuangoutong"].ToString();
                //新增页面配置字段  2012年3月31日9:43:04
                m_IsConfigPageSize = null == dr["isconfigpagesize"] ? "" : dr["isconfigpagesize"].ToString();

                m_NEW_PAGE_END = Convert.ToInt32(dr["NEW_PAGE_END"]);
                m_state = null == dr["state"] ? "" : dr["state"].ToString();
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
                m_emrtemplet.MrClass = m_MrClass;
                m_emrtemplet.DeptId = m_DeptId;
                if (m_MrClass == "AC")
                {
                    txtTitle.Enabled = true;
                    chkIsShowTitle.Enabled = true;
                }
                else
                {
                    txtTitle.Enabled = false;
                    chkIsShowTitle.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 清空值
        /// </summary>
        /// <param name="mr_class"></param>
        /// <param name="dept_id"></param>
        public void ClearValue()
        {
            try
            {
                m_TempletId = string.Empty;
                m_MrClass = string.Empty;
                m_MrName = string.Empty;
                m_QcCode = string.Empty;
                m_DeptId = string.Empty;
                m_WriteTimes = -1;
                m_NewPageFlag = -1;
                m_IsFirstDaily = string.Empty;
                m_IsNew = true;
                m_DailyTitle = string.Empty;
                m_IsShowDailyTitle = string.Empty;
                m_IsYiHuanGouTong = string.Empty;
                m_IsConfigPageSize = string.Empty;
                m_NEW_PAGE_END = -1;
                m_state = string.Empty;

                this.lookUpEditorMr_class.CodeValue = string.Empty;
                this.lookUpEditorDepartment.CodeValue = string.Empty;
                this.lookUpEditorqc_code.CodeValue = string.Empty;
                this.txt_mr_name.Text = string.Empty;
                this.radgroupWRITE_TIMES.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //声明是否含有星号的bool变量
        public bool ContainStar = false;

        private void SetControlValue()
        {
            if (!m_IsNew)
            {
                this.txtTEMPLET_ID.Text = m_TempletId;
                this.lookUpEditorMr_class.CodeValue = m_MrClass;
                //含有星号的在修改的时候不进行显示星号（已配对的）
                if (m_MrName.StartsWith("★"))
                {
                    m_ShowStar = true;
                    this.txt_mr_name.Text = m_MrName.TrimStart('★');
                    this.labelStar.Visible = true;
                    this.labelStarPrompt.Visible = true;
                }
                else
                {
                    m_ShowStar = false;//封装为属性
                    this.txt_mr_name.Text = m_MrName;
                    this.labelStar.Visible = false;
                    this.labelStarPrompt.Visible = false;
                }
                this.lookUpEditorqc_code.CodeValue = m_QcCode;
                this.lookUpEditorDepartment.CodeValue = m_DeptId;
                this.radgroupWRITE_TIMES.SelectedIndex = m_WriteTimes;



                if (m_NewPageFlag == 1)
                {
                    this.chk_new_page_falg.Checked = true;
                }
                else
                {
                    this.chk_new_page_falg.Checked = false;
                }

                if (m_NEW_PAGE_END == 1)
                {
                    this.chk_new_page_end.Checked = true;
                }
                else
                {
                    this.chk_new_page_end.Checked = false;
                }
                //新增页面配置 2012年3月31日9:44:05
                if (m_IsConfigPageSize == "1")
                {
                    this.chkPageSize.Checked = true;
                }
                else
                {
                    this.chkPageSize.Checked = false;
                }

                ////先注释掉
                if (m_IsFirstDaily == "1")
                {
                    this.checkEditFirstDaily.Checked = true;
                }
                else
                {
                    this.checkEditFirstDaily.Checked = false;
                }
                //是首次病程的都将首程勾选中 edit by ywk  2012年4月15日14:49:03
                //if (m_MrName.Contains("首程")||m_MrName.Contains("首次病程"))
                //注释 by cyq 2013-05-27 首次病程只以isfirstdaily标识为准
                //if (m_MrName.StartsWith("★(首程)") || m_MrName.StartsWith("★（首程）") || m_MrName.StartsWith("首次病程"))
                //{
                //    this.checkEditFirstDaily.Checked = true;
                //}

                if (m_IsYiHuanGouTong == "1")
                {
                    this.checkEditIsGouTong.Checked = true;
                }
                else
                {
                    this.checkEditIsGouTong.Checked = false;
                }

                if (m_MrClass == "AC") //病程
                {
                    //panelControl2.Visible = true;
                    checkEditFirstDaily.Enabled = true;
                    chk_new_page_falg.Enabled = true;
                    chk_new_page_end.Enabled = true;
                    //checkEditIsGouTong.Enabled = true;

                    txtTitle.Enabled = true;
                    chkIsShowTitle.Enabled = true;
                    txtTitle.Text = m_DailyTitle;
                    if (m_IsShowDailyTitle == "1")
                    {
                        chkIsShowTitle.Checked = true;
                    }
                    else
                    {
                        chkIsShowTitle.Checked = false;
                    }
                }
                else
                {
                    //panelControl2.Visible = false;
                    checkEditFirstDaily.Enabled = false;
                    chk_new_page_falg.Enabled = false;
                    chk_new_page_end.Enabled = false;
                    //checkEditIsGouTong.Enabled = false;
                    txtTitle.Enabled = false;
                    chkIsShowTitle.Enabled = false;
                }

                //by cyq 2012-08-27
                labelStar.Visible = true;
                labelStarPrompt.Visible = true;
            }
        }

        public void GetDataRow(DataRow dr)
        {
            dr["templet_id"] = this.txtTEMPLET_ID.Text;
            dr["mr_class"] = this.lookUpEditorMr_class.CodeValue;
            dr["mr_name"] = this.txt_mr_name.Text;
            dr["qc_code"] = this.lookUpEditorqc_code.CodeValue;
            dr["dept_id"] = this.lookUpEditorDepartment.CodeValue;
            if (this.chk_new_page_falg.Checked)
            {
                dr["new_page_flag"] = 1;
            }
            else
            {
                dr["new_page_flag"] = 0;
            }

            if (this.chk_new_page_end.Checked)
            {
                dr["new_page_end"] = 1;
            }
            else
            {
                dr["new_page_end"] = 0;
            }

            if (this.checkEditFirstDaily.Checked)
            {
                dr["isfirstdaily"] = "1";
            }
            else
            {
                dr["isfirstdaily"] = "0";
            }
            dr["file_name"] = this.txtTitle.Text;
            if (chkIsShowTitle.Checked)
            {
                dr["isshowfilename"] = "1";
            }
            else
            {
                dr["isshowfilename"] = "0";
            }

            //页面配置 2012年3月31日9:46:42
            if (chkPageSize.Checked)
            {
                dr["isconfigpagesize"] = "1";
            }
            else
            {
                dr["isconfigpagesize"] = "0";
            }
        }
        /// <summary>
        /// 新增构造函数的参数 控制页面设置复选框显示 edit by ywk 2012-3-31 14:06:39 
        /// </summary>
        /// <param name="emrtemplet"></param>
        /// <param name="app"></param>
        public AddNewTemplate(Emrtemplet emrtemplet, IEmrHost app, bool isShowPage)
        {
            InitializeComponent();
            m_app = app;
            m_emrtemplet = null == emrtemplet ? new Emrtemplet() : emrtemplet;
            this.chkPageSize.Visible = isShowPage;
            this.chkPageSize.Checked = isShowPage;
        }

        private void SetValue(TreeListNode node)
        {
            //txt_mr_name.Text = 
        }

        /// <summary>
        /// 将页面信息绑定到Emrtemplet实体中
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        /// </summary>
        private void BindEmrTemplet()
        {
            try
            {
                if (this.lookUpEditorMr_class.CodeValue == "")
                {//必须填写模板类型
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择模板类型");
                    this.lookUpEditorMr_class.Focus();
                    return;
                }
                else if (this.txt_mr_name.Text.Trim().Length == 0)
                {//必须填写模板名称
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("模板名称不能为空");
                    this.txt_mr_name.Focus();
                    return;
                }
                else if (this.lookUpEditorDepartment.CodeValue == "")
                {//必须填写模板科室
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择所属科室");
                    this.lookUpEditorDepartment.Focus();
                    return;
                }
                else if (this.lookUpEditorDepartment.CodeValue != "*" && this.lookUpEditorDepartment.CodeValue != "通用科室")
                {
                    var subDepts = m_depts.Select(" len(ID) > 2 and ID <> '" + this.lookUpEditorDepartment.CodeValue + "' and ID like '" + this.lookUpEditorDepartment.CodeValue + "%" + "' ");
                    if (null != subDepts && subDepts.Length > 0)
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(this.lookUpEditorDepartment.Text + "下存在子科室，请选择子科室。");
                        this.lookUpEditorDepartment.Focus();
                        return;
                    }
                }

                m_IsOK = true;
                m_emrtemplet = new Emrtemplet();
                m_emrtemplet.TempletId = this.txtTEMPLET_ID.Text;
                m_emrtemplet.MrClass = this.lookUpEditorMr_class.CodeValue;
                //add by ywk 2012年4月15日12:00:46
                //if (ContainStar)//如果含有星号
                //{
                //    m_emrtemplet.MrName = "★" + this.txt_mr_name.Text;
                //}
                //else
                //{
                m_emrtemplet.MrName = this.txt_mr_name.Text;
                //}

                m_emrtemplet.QcCode = this.lookUpEditorqc_code.CodeValue;
                m_emrtemplet.DeptId = this.lookUpEditorDepartment.CodeValue;
                m_emrtemplet.WriteTimes = this.radgroupWRITE_TIMES.SelectedIndex;
                //新页
                if (this.chk_new_page_falg.Checked)
                {
                    m_emrtemplet.NewPageFlag = 1;
                }
                else
                {
                    m_emrtemplet.NewPageFlag = 0;
                }
                //新页结束
                if (this.chk_new_page_end.Checked)
                {
                    m_emrtemplet.NEW_PAGE_END = 1;
                }
                else
                {
                    m_emrtemplet.NEW_PAGE_END = 0;
                }
                //首程
                if (this.checkEditFirstDaily.Checked)
                {
                    m_emrtemplet.IsFirstDailyEmr = "1";
                }
                else
                {
                    m_emrtemplet.IsFirstDailyEmr = "0";
                }
                m_DailyTitle = txtTitle.Text;
                m_emrtemplet.FileName = m_DailyTitle;
                if (chkIsShowTitle.Checked)
                {
                    m_IsShowDailyTitle = "1";
                }
                else
                {
                    m_IsShowDailyTitle = "0";
                }
                m_emrtemplet.IsShowDailyTitle = m_IsShowDailyTitle;
                if (checkEditIsGouTong.Checked)
                {
                    m_IsYiHuanGouTong = "1";
                }
                else
                {
                    m_IsYiHuanGouTong = "0";
                }
                m_emrtemplet.IsYiHuanGouTong = m_IsYiHuanGouTong;

                //是否读取页面配置
                //add by ywk 2012年3月31日9:36:30
                if (chkPageSize.Checked)
                {
                    m_IsConfigPageSize = "1";
                }
                else
                {
                    m_IsConfigPageSize = "0";
                }
                m_emrtemplet.IsPageConfigSize = m_IsConfigPageSize;

                //判断是否含有星号 
                if (m_MrName.StartsWith("★"))
                {
                    m_emrtemplet.ShowStar = true;
                }
                else
                {
                    m_emrtemplet.ShowStar = false;//封装为属性
                }




                m_emrtemplet.State = "";
                //if (m_emrtemplet != null)
                //{
                //    m_emrtemplet.TempletId = m_TempletId;
                //    m_emrtemplet.MrClass = m_MrClass;
                //    m_emrtemplet.MrName = m_MrName;
                //    m_emrtemplet.QcCode = m_QcCode;
                //    m_emrtemplet.DeptId = m_DeptId;
                //    m_emrtemplet.WriteTimes = m_WriteTimes;
                //    m_emrtemplet.NewPageFlag = m_NewPageFlag;
                //    m_emrtemplet.IsFirstDailyEmr = m_IsFirstDaily;
                //    m_emrtemplet.NewPageFlag = m_IsNew == false ? 0 : 1;
                //    m_emrtemplet.FileName = m_DailyTitle;
                //    m_emrtemplet.IsShowDailyTitle = m_IsShowDailyTitle;
                //}

                this.Close();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 将实体数据加载到页面
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNewTemplate_Load(object sender, EventArgs e)
        {
            try
            {
                //新增各窗体的logo，改为和主界面与头部图片一致的
                //add by ywk 2013年3月8日11:41:42 
                //if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Icon\\DrectSoftLogo.ico"))
                //{
                //    Icon myicon = new System.Drawing.Icon(AppDomain.CurrentDomain.BaseDirectory + "\\Icon\\DrectSoftLogo.ico", 32, 32);
                //    this.Icon = myicon;
                //}

                InitDepartment();
                InitMRClass();
                InitQC();
                //panelControl2.Enabled = false;
                checkEditFirstDaily.Enabled = false;
                chk_new_page_falg.Enabled = false;
                //checkEditIsGouTong.Enabled = false;
                //lookUpEditorMr_class.AllowDrop = false;
                //lookUpEditorMr_class.Focus();
                //lookUpEditorMr_class.AllowDrop = true;

                if (!m_IsNew)//修改叶子节点
                {
                    SetControlValue();
                }
                else if (m_emrtemplet == null)
                {
                    //by cyq 2012-08-27
                    labelStar.Visible = false;
                    labelStarPrompt.Visible = false;

                    return;
                }
                else
                {
                    this.txtTEMPLET_ID.Text = m_emrtemplet.TempletId;
                    this.lookUpEditorMr_class.CodeValue = m_emrtemplet.MrClass;
                    this.txt_mr_name.Text = m_emrtemplet.MrName;
                    this.lookUpEditorqc_code.CodeValue = m_emrtemplet.QcCode;
                    this.lookUpEditorDepartment.CodeValue = m_emrtemplet.DeptId;

                    this.radgroupWRITE_TIMES.SelectedIndex = m_emrtemplet.WriteTimes;
                    if (m_emrtemplet.NewPageFlag == 1)
                    {
                        this.chk_new_page_falg.Checked = true;
                    }
                    else
                    {
                        this.chk_new_page_falg.Checked = false;
                    }

                    if (m_emrtemplet.NEW_PAGE_END == 1)
                    {
                        this.chk_new_page_end.Checked = true;
                    }
                    else
                    {
                        this.chk_new_page_end.Checked = false;
                    }

                    //页面配置 2012年3月31日9:48:15
                    if (m_emrtemplet.IsPageConfigSize == "1")
                    {
                        this.chkPageSize.Checked = true;
                    }
                    /*else
                    {
                        this.chkPageSize.Checked = false;
                    }
                    */
                    //by cyq 2012-08-27
                    labelStar.Visible = false;
                    labelStarPrompt.Visible = false;

                }

                //edit by cyq 2013-02-19
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
                txt_mr_name.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
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

                #region old
                //string sql = string.Format(@"select DEPT_ID ID,DEPT_NAME NAME,py,wb from EMRDEPT a");
                //DataTable Dept = m_app.SqlHelper.ExecuteDataTable(sql);
                SQLManger sqlmanger = new SQLManger(m_app);

                //edit by cyq 2013-02-19 edit by cyq 2013-03-08
                //DataTable Dept = sqlmanger.GetSubDeptListByUser();
                DataTable Dept = sqlmanger.GetDeptListByUser();
                m_depts = Dept;
                Dept.Columns["ID"].Caption = "科室代码";
                Dept.Columns["NAME"].Caption = "科室名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 80);
                cols.Add("NAME", 188);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//NAME//PY//WB");
                lookUpEditorDepartment.SqlWordbook = deptWordBook;
                lookUpEditorDepartment.CodeValue = "0000";
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 得到拼音和五笔
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string[] GetPYWB(string name)
        {
            GenerateShortCode shortCode = new GenerateShortCode(m_app.SqlHelper);
            string[] code = shortCode.GenerateStringShortCode(name);
            return code;
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
                lookUpWindowMr_class.SqlHelper = m_app.SqlHelper;

                //yxy 暂时加载TP科室
                //DataTable Dept = m_app.SqlHelper.ExecuteDataTable("usp_GetMedicalRrecordViewFrm",
                //     new SqlParameter[] { new SqlParameter("@GetType", "1") }, CommandType.StoredProcedure);

                string sql = string.Format(@"select CCODE ID,CNAME NAME,CTYPE,OPEN_FLAG from DICT_CATALOG where CTYPE = '2' and ISUSED = '1' and (UTYPE = '3' or UTYPE = '1') and CNAME<>'子女病程记录';");
                DataTable Dept = m_app.SqlHelper.ExecuteDataTable(sql);
                new GenerateShortCode(m_app.SqlHelper).AutoAddShortCode(Dept, "NAME");

                Dept.Columns["ID"].Caption = "类型编码";
                Dept.Columns["NAME"].Caption = "类型名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 80);
                cols.Add("NAME", 188);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//NAME//PY//WB");
                lookUpEditorMr_class.SqlWordbook = deptWordBook;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 初始监控内容
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        private void InitQC()
        {
            try
            {
                lookUpWindowqc_code.SqlHelper = m_app.SqlHelper;

                string sql = string.Format(@"select a.i_code ID,a.i_name name from emrqcitem a ");
                DataTable Sslb = m_app.SqlHelper.ExecuteDataTable(sql);
                new GenerateShortCode(m_app.SqlHelper).AutoAddShortCode(Sslb, "NAME");

                Sslb.Columns["ID"].Caption = "类别代码";
                Sslb.Columns["NAME"].Caption = "类别名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 80);
                cols.Add("NAME", 188);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Sslb, "ID", "NAME", cols, "ID//Name//PY//WB");
                lookUpEditorqc_code.SqlWordbook = deptWordBook;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        #endregion

        private void radgroupWRITE_TIMES_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radgroupWRITE_TIMES.SelectedIndex == 1)
            {
                panelControl1.Enabled = false;
                this.txtTitle.Enabled = false;
                this.chkIsShowTitle.Enabled = false;
            }
            else
            {
                panelControl1.Enabled = true;
                this.txtTitle.Enabled = true;
                this.chkIsShowTitle.Enabled = true;
            }
        }

        /// <summary>
        /// 取消事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        public IPlugIn Run(IEmrHost host)
        {
            IPlugIn plg = new PlugIn(this.GetType().ToString(), this);
            m_app = host;

            return plg;
        }

        public void SetFirstFocusControl()
        {
            try
            {
                this.ActiveControl = lookUpEditorMr_class;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void lookUpEditorMr_class_CodeValueChanged(object sender, EventArgs e)
        {
            if (lookUpEditorMr_class.CodeValue == "AC") //病程
            {
                //panelControl2.Enabled = true;

                checkEditFirstDaily.Enabled = true;
                chk_new_page_falg.Enabled = true;
                chk_new_page_end.Enabled = true;
                //checkEditIsGouTong.Enabled = true;
                txtTitle.Enabled = true;
                chkIsShowTitle.Enabled = true;
            }
            else
            {
                //panelControl2.Enabled = false;

                checkEditFirstDaily.Enabled = false;
                chk_new_page_falg.Enabled = false;
                chk_new_page_end.Enabled = false;
                //checkEditIsGouTong.Enabled = false;
                txtTitle.Enabled = false;
                chkIsShowTitle.Enabled = false;
            }
        }

        /// <summary>
        /// 模板名称值改变事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_mr_name_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txt_mr_name.Text = txt_mr_name.Text.Trim('★');
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 复选框回车事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-06</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chb_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == 13)
                {
                    DS_Common.cbx_KeyPress(sender);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
    }
}