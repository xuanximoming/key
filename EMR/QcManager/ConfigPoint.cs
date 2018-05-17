using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.FrameWork;
using DrectSoft.Wordbook;
using DrectSoft.Common.Ctrs;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.FrameWork.WinForm;

namespace DrectSoft.Emr.QcManager
{
    /// <summary>
    /// 用于维护病历评分中的各分类的维护操作 add by ywk 2012年3月31日15:30:14
    /// </summary>
    public partial class ConfigPoint : DevBaseForm, IStartPlugIn
    {
        private IEmrHost m_app;
        /// <summary>
        /// 存储SQL语句的类
        /// </summary>
        SqlManger m_SqlManager;

        /// <summary>
        /// 当前操作状态
        /// </summary>
        EditState m_EditState = EditState.None;


        public ConfigPoint()
        {
            InitializeComponent();
        }

        //    bool RefreshD = false;//此页面关闭后，上级页面数据要相应的刷新数据
        public ConfigPoint(IEmrHost app)
        {
            InitializeComponent();
            m_app = app;
            m_SqlManager = new SqlManger(m_app);

        }
        #region 共用事件

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfigPoint_Load(object sender, EventArgs e)
        {
            m_SqlManager = new SqlManger(m_app);
            LoadConfigData();
            InitParentClass();
            //加载评分标准配置的数据add by ywk 2012年5月25日 16:42:35
            InitlookUpEditParents(); //wj  2012 11 8
            InitlookUpEditChild();
            LoadReductionData();
        }

        private void InitPointClass()
        {
            lookUpWindowCCNAME.SqlHelper = m_app.SqlHelper;

            SqlManger sqlManager = new SqlManger(m_app);
            DataTable DtParent = sqlManager.GetParentClass();
            DtParent.Columns["CCODE"].Caption = "分类代码";
            DtParent.Columns["CNAME"].Caption = "分类名称";
            Dictionary<string, int> cols = new Dictionary<string, int>();
            cols.Add("CCODE", 50);
            cols.Add("CNAME", 100);
            SqlWordbook deptWordBook = new SqlWordbook("querybook", DtParent, "CCODE", "CNAME", cols);
            lookUpCCNAME.SqlWordbook = deptWordBook;
        }

        #endregion

        #region 共用方法
        /// <summary>
        ///  通过判断不同类型操作控件按钮状态
        /// </summary>
        private void BtnState(string type)
        {
            //查看详细状态
            if (m_EditState == EditState.View)
            {
                if (type == "1")
                {
                    this.btnADD.Enabled = true;
                    this.btnDel.Enabled = true;
                    this.btnSave.Enabled = false;
                    this.BtnClear.Enabled = false;
                    this.txtId.Enabled = false;
                    this.lookUpCCNAME.Enabled = false;
                    this.txtChildName.Enabled = false;
                    cmbValid.Enabled = false;
                    DevButtonEdit.Enabled = true;
                }
                if (type == "2")
                {
                    this.simpleButton4.Enabled = true;//新增按钮
                    this.simpleButton2.Enabled = true;//删除按钮
                    this.simpleButton3.Enabled = false;//保存按钮
                    this.simpleButton1.Enabled = false;//取消按钮
                    this.txtPoint.Enabled = false;//扣分标准
                    this.cmbValidName.Enabled = false;
                    this.memoDESC.Enabled = false;
                    this.lookUpEditParents.Enabled = false;
                    this.lookUpEditChild.Enabled = false;
                    this.comboBoxEdit1.Enabled = false;
                    this.textEdit2.Enabled = false;
                    DevButtonEdit1.Enabled = true;
                }
                //if (type == "3")
                //{
                //    this.simpleButton8.Enabled = true;//新增按钮
                //    this.simpleButton6.Enabled = true;//删除按钮
                //    this.simpleButton7.Enabled = false;//保存按钮
                //    this.simpleButton5.Enabled = false;//取消按钮
                //    this.lookUpEdit2.Enabled = false;//扣分标准
                //    this.lookUpEdit1.Enabled = false;
                //    this.lookUpEdit3.Enabled = false;
                //    this.textEdit1.Enabled = false;
                //    this.memoEdit1.Enabled = false;
                //}
            }
            else if (m_EditState == EditState.Add || m_EditState == EditState.Edit)
            {
                if (type == "1")
                {
                    this.btnADD.Enabled = false;
                    this.btnDel.Enabled = false;
                    this.btnSave.Enabled = true;
                    this.BtnClear.Enabled = true;
                    DevButtonEdit.Enabled = false;
                    if (m_EditState == EditState.Add)
                        this.txtId.Enabled = true;
                    else
                        this.txtId.Enabled = false;
                    this.lookUpCCNAME.Enabled = true;
                    this.txtChildName.Enabled = true;
                    //this.btnADD.Enabled = true;
                    //this.btnDel.Enabled = true;
                    //cmbValid.Enabled = true;
                }

                if (type == "2")
                {
                    this.simpleButton4.Enabled = false;//新增按钮
                    this.simpleButton2.Enabled = false;//删除按钮
                    this.simpleButton3.Enabled = true;//保存按钮
                    this.simpleButton1.Enabled = true;//取消按钮
                    DevButtonEdit1.Enabled = false;
                    if (m_EditState == EditState.Edit)//编辑操作可进行删除
                    {
                        this.simpleButton2.Enabled = true;//删除按钮
                    }
                    this.txtPoint.Enabled = true;//扣分标准
                    //this.cmbValidName.Enabled = true;
                    this.memoDESC.Enabled = true;
                    this.lookUpEditParents.Enabled = true;
                    this.lookUpEditChild.Enabled = true;
                    this.comboBoxEdit1.Enabled = true;
                    this.textEdit2.Enabled = true;
                }
                //if (type == "3")
                //{
                //    this.simpleButton8.Enabled = false;//新增按钮
                //    this.simpleButton6.Enabled = false;//删除按钮
                //    this.simpleButton7.Enabled = true;//保存按钮
                //    this.simpleButton5.Enabled = true;//取消按钮
                //    if (m_EditState == EditState.Edit)//编辑操作可进行删除
                //    {
                //        this.simpleButton6.Enabled = true;//删除按钮
                //    }
                //    this.lookUpEdit2.Enabled = true;//扣分标准
                //    this.lookUpEdit1.Enabled = true;
                //    this.lookUpEdit3.Enabled = true;
                //    this.textEdit1.Enabled = true;
                //    this.memoEdit1.Enabled = true;
                //}
            }
        }
        /// <summary>
        /// 刷新数据
        /// </summary>
        private void RefreshData(string type)
        {
            if (type == "1")
            {
                m_EditState = EditState.View;
                LoadConfigData();

                BtnState(type);
                ClearPageValue(type);
                //btnADD.Focus();
            }
            if (type == "2")
            {
                m_EditState = EditState.View;
                LoadReductionData();

                BtnState(type);
                ClearPageValue(type);
                //simpleButton4.Focus();
            }
            //if (type == "3")
            //{
            //    LoadReductionDetailData();
            //    m_EditState = EditState.View;
            //    BtnState(type);
            //    ClearPageValue(type);
            //}
        }
        /// <summary>
        /// 清空窗体所有输入的值
        /// </summary>
        private void ClearPageValue(string type)
        {
            if (type == "1")
            {
                //类别设置里的
                this.lookUpCCNAME.SelectedText = "";
                this.txtId.Text = "";
                txtChildName.Text = "";
                lookUpCCNAME.Text = "";
                //cmbValid.SelectedIndex = -1;
            }

            //评分标准配置里的
            if (type == "2")
            {
                this.txtPoint.Text = "";
                this.cmbValidName.SelectedIndex = -1;
                this.memoDESC.Text = "";
                this.comboBoxEdit1.SelectedIndex = -1;
                this.textEdit2.Text = "";
                lookUpEditParents.EditValue = "";
                lookUpEditChild.EditValue = "";
            }
            //if (type == "3")
            //{
            //    this.textEdit1.Text = "";
            //    this.memoEdit1.Text = "";
            //}
        }
        /// <summary>
        /// 判断保存操作是否成功
        /// </summary>
        /// <returns></returns>
        private bool IsSave(string type)
        {
            if (type == "1")//类别设置里的
            {
                //edit by wyt 子类编号通过guid自动生成
                //if (this.txtId.Text.Trim() == "")
                //{
                //    m_app.CustomMessageBox.MessageShow("请输入子类编号！");
                //    return false;
                //}
                if (lookUpCCNAME.CodeValue == "")
                {
                    m_app.CustomMessageBox.MessageShow("请选择所属类型");
                    lookUpCCNAME.Focus();
                    return false;
                }
                if (txtChildName.Text.Trim() == "")
                {
                    m_app.CustomMessageBox.MessageShow("请输入子类名称");
                    txtChildName.Focus();
                    return false;
                }
                if (txtChildName.Text.Trim().Length > 30)
                {
                    m_app.CustomMessageBox.MessageShow("子类名称过长");
                    txtChildName.Focus();
                    return false;
                }

                if (this.cmbValid.SelectedIndex == -1)
                {
                    m_app.CustomMessageBox.MessageShow("请选择是否有效");
                    cmbValid.Focus();
                    return false;
                }
            }
            if (type == "2")//评分配置里的
            {
                if (this.memoDESC.Text.Trim() == "")
                {
                    m_app.CustomMessageBox.MessageShow("请输入扣分理由");
                    memoDESC.Focus();
                    return false;
                }
                if (memoDESC.Text.Length > 200)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("扣分理由长度太长");
                    memoDESC.Focus();
                    return false;
                }
                if (txtPoint.Text.Trim() == "")
                {
                    m_app.CustomMessageBox.MessageShow("请输入扣分标准");
                    txtPoint.Focus();
                    return false;
                }
                try
                {
                    float.Parse(txtPoint.Text.ToString());
                }
                catch (Exception)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("扣分标准必须是数字");
                    txtPoint.Focus();
                    return false;
                }
                if (this.cmbValidName.SelectedIndex == -1)
                {
                    m_app.CustomMessageBox.MessageShow("请选择是否有效");
                    cmbValidName.Focus();
                    return false;
                }

                if (lookUpEditParents.EditValue.ToString() == "" || lookUpEditChild.EditValue.ToString() == "")
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请填写类别");
                    lookUpEditParents.Focus();
                    return false;
                }
                if (this.textEdit2.Text.Trim() == "")
                {
                    if (this.comboBoxEdit1.SelectedIndex == 1)
                    {
                        m_app.CustomMessageBox.MessageShow("请输入模板中的节点关键字");
                        textEdit2.Focus();
                        return false;
                    }
                    if (this.comboBoxEdit1.SelectedIndex == 0)
                    {
                        m_app.CustomMessageBox.MessageShow("请输入该记录模板名称关键字");
                        textEdit2.Focus();
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 区分操作类型 
        /// </summary>
        public enum EditState
        {
            /// <summary>
            /// 无
            /// </summary>
            None = 1,
            /// <summary>
            /// 新增
            /// </summary>
            Add = 2,
            /// <summary>
            /// 编辑
            /// </summary>
            Edit = 4,
            /// <summary>
            /// 视图
            /// </summary>
            View = 8
        }
        #endregion

        #region 病例评分类别里的操作

        /// <summary>
        /// 初始化大类名称
        /// </summary>
        private void InitParentClass()
        {
            lookUpWindowCCNAME.SqlHelper = m_app.SqlHelper;

            SqlManger sqlManager = new SqlManger(m_app);
            DataTable DtParent = sqlManager.GetParentClass();
            DtParent.Columns["CCODE"].Caption = "分类代码";
            DtParent.Columns["CNAME"].Caption = "分类名称";
            Dictionary<string, int> cols = new Dictionary<string, int>();
            cols.Add("CCODE", 40);
            cols.Add("CNAME", 80);
            SqlWordbook deptWordBook = new SqlWordbook("querybook", DtParent, "CCODE", "CNAME", cols);
            lookUpCCNAME.SqlWordbook = deptWordBook;
        }

        /// <summary>
        /// 得到病历评分配置数据信息 (评分类别)
        /// </summary>
        private void LoadConfigData()
        {
            DataTable m_ConfigDt = new DataTable();
            m_ConfigDt = m_SqlManager.GetConfigPoint();
            gridControl1.DataSource = m_ConfigDt;//绑定数据源
        }
        /// <summary>
        /// 新增操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnADD_Click(object sender, EventArgs e)
        {
            m_EditState = EditState.Add;
            ClearPageValue("1");
            BtnState("1");
            //gridControl1.Enabled = false;
            lookUpCCNAME.Focus();
        }


        /// <summary>
        /// 编辑操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DevButtonEdit_Click(object sender, EventArgs e)
        {
            m_EditState = EditState.Edit;
            //gridControl1.Enabled = false;
            BtnState("1");
            this.memoDESC.Focus();
        }
        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            if (gridView4.FocusedRowHandle < 0)
            {
                m_app.CustomMessageBox.MessageShow("请选择要删除的记录");
                return;
            }
            DataRow foucesRow = gridView4.GetDataRow(gridView4.FocusedRowHandle);
            if (foucesRow == null)
            {
                m_app.CustomMessageBox.MessageShow("请选择要删除的记录");
                return;
            }
            if (DialogResult.OK == m_app.CustomMessageBox.MessageShow("您确定要删除该记录吗？", DrectSoft.Core.CustomMessageBoxKind.QuestionOkCancel))
            {
                try
                {
                    m_SqlManager.SaveData(SetEntityByPage(), "3");
                    m_app.CustomMessageBox.MessageShow("删除成功");
                    RefreshData("1");
                }
                catch (Exception)
                {
                    m_app.CustomMessageBox.MessageShow("删除失败");
                }
            }
        }

        /// <summary>
        /// 保存操作
        /// edit by wangji 2012 12 11 去掉同名项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsSave("1"))
            { return; }
            if (!CheckChildValue(txtChildName.Text.Trim(), lookUpCCNAME.CodeValue))
            {
                lookUpCCNAME.Enabled = false;
                m_app.CustomMessageBox.MessageShow("编辑成功");
                m_EditState = EditState.View;
                BtnState("1");
                ClearPageValue("1");
                //gridControl1.Enabled = true;
                return;
            }
            if (SaveData(SetEntityByPage()))
            {
                if (m_EditState == EditState.Add)
                {
                    m_app.CustomMessageBox.MessageShow("新增成功");
                    //int rowno = gridView4.FocusedRowHandle;
                }
                else
                {
                    m_app.CustomMessageBox.MessageShow("修改成功");
                }
                RefreshData("1");
                //gridControl1.Enabled = true;
                InitlookUpEditParents();
                InitlookUpEditChild();
            }
            else
            {
                if (m_EditState == EditState.Add)
                {
                    m_app.CustomMessageBox.MessageShow("新增失败");
                }
                else
                {
                    m_app.CustomMessageBox.MessageShow("修改失败");
                }
            }
        }

        private Boolean CheckChildValue(string childName, string ccode)
        {
            try
            {
                string sql = string.Format(@"select t.id from emr_configpoint t where t.ccode='{0}' and t.CHILDNAME='{1}' and t.VALID='1'", ccode, childName);
                DataTable dt = m_app.SqlHelper.ExecuteDataTable(sql);
                if (dt.Rows.Count == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 检查项目的值是否已经存在 
        /// add wangji 2012 12 18  
        /// edit wangj 2013 3 6  id
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="score"></param>
        /// <param name="ccode"></param>
        /// <param name="CHILDREN"></param>
        /// <returns></returns>
        private Boolean CheckItemValue(string id, string itemName, string score, string ccode, string CHILDREN, string isauto, string selectcondition)                                    //
        {
            try
            {
                string sql = "";
                if (id == "")
                {
                    sql = string.Format(@"select * from emr_configreduction2 t where t.problem_desc='{0}'  and t.valid=1 and t.parents='{1}' and t.children='{2}' and t.REDUCEPOINT ='{3}'", itemName, ccode, CHILDREN, score);    //  /*and t.isauto='{4}' and t.selectcondition='{5}'*/, isauto, selectcondition
                }
                else
                {
                    sql = string.Format(@"select * from emr_configreduction2 t where (t.id='{0}' or (t.problem_desc='{1}' and t.parents='{2}' and t.children='{3}' and t.REDUCEPOINT ='{4}') )and t.valid=1  ", id, itemName, ccode, CHILDREN, score); //and   ;
                    //sql = string.Format(@"select * from emr_configreduction2 t where t.problem_desc='{0}'  and t.valid=1 and t.parents='{1}' and t.children='{2}' and t.REDUCEPOINT ='{3}' ", itemName, ccode, CHILDREN, score); ;
                }
                //if (isauto != "")
                //{
                //}
                DataTable dt = m_app.SqlHelper.ExecuteDataTable(sql);
                if (dt.Rows.Count == 0)
                {
                    return false;
                }
                else
                    if (dt.Rows.Count < 2 && id != "")
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 取消操作 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearPageValue("1");
            m_EditState = EditState.View;
            BtnState("1");
            //gridControl1.Enabled = true;
        }

        /// <summary>
        /// 双击列表为修改更新操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControl1_Click(object sender, EventArgs e)
        {
            if (gViewConfigPoint.FocusedRowHandle < 0)
                return;
            DataRow foucesRow = gViewConfigPoint.GetDataRow(gViewConfigPoint.FocusedRowHandle);
            if (foucesRow == null)
                return;

            if (foucesRow.IsNull("id"))
                return;

            SetPageValue(SetEntityByDataRow(foucesRow));
        }
        /// <summary>
        /// 将grid中的值赋值给实体
        /// </summary>
        /// <param name="foucesRow"></param>
        /// <returns></returns>
        private ConfigEmrPoint SetEntityByDataRow(DataRow foucesRow)
        {
            if (foucesRow == null)
            {
                return null;
            }
            ConfigEmrPoint myConfigEmr = new ConfigEmrPoint();
            myConfigEmr.CChildCode = foucesRow["childcode"].ToString();
            myConfigEmr.CChildName = foucesRow["childname"].ToString();
            myConfigEmr.CCODE = foucesRow["ccode"].ToString();
            myConfigEmr.Valid = foucesRow["valid"].ToString();
            myConfigEmr.ID = foucesRow["id"].ToString();
            return myConfigEmr;
        }
        /// <summary>
        /// 将实体值赋给页面元素
        /// </summary>
        /// <param name="configEmrPoint"></param>
        private void SetPageValue(ConfigEmrPoint configEmrPoint)
        {
            if (configEmrPoint == null)
                return;
            txtId.Text = configEmrPoint.CChildCode;
            txtChildName.Text = configEmrPoint.CChildName;
            cmbValid.SelectedIndex = Int32.Parse(configEmrPoint.Valid);
            lookUpCCNAME.CodeValue = configEmrPoint.CCODE;
        }
        /// <summary>
        /// 实现接口部分
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public IPlugIn Run(IEmrHost host)
        {
            IPlugIn plg = new PlugIn(this.GetType().ToString(), this);
            m_app = host;
            return plg;
        }
        /// <summary>
        /// 进行保存操作 
        /// </summary>
        /// <param name="configEmrPoint"></param>
        /// <returns></returns>
        private bool SaveData(ConfigEmrPoint configEmrPoint)
        {
            try
            {
                string edittype = "";
                if (m_EditState == EditState.Add)
                    edittype = "1";
                else
                    edittype = "2";
                m_SqlManager.SaveData(configEmrPoint, edittype);
                lookUpCCNAME.Enabled = false;
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        /// <summary>
        ///  将页面值加到实体里(评分类别配置里的实体)
        /// </summary>
        /// <returns></returns>
        private ConfigEmrPoint SetEntityByPage()
        {
            ConfigEmrPoint configemrPoint = new ConfigEmrPoint();
            configemrPoint.CCODE = lookUpCCNAME.CodeValue;//大分类编号
            //edit by wyt 子类编号通过guid自动生成 如果已有编号，则不作变化
            configemrPoint.CChildCode = txtId.Text.Trim();//小分类编号
            if (configemrPoint.CChildCode == "")
            {
                configemrPoint.CChildCode = Guid.NewGuid().ToString().Substring(0, 16);
            }
            configemrPoint.CChildName = txtChildName.Text.Trim();//小分类名称
            configemrPoint.Valid = "1";
            DataRow foucesRow = gViewConfigPoint.GetDataRow(gViewConfigPoint.FocusedRowHandle);
            if (foucesRow != null)
            {
                configemrPoint.ID = foucesRow["id"].ToString();
            }
            return configemrPoint;
        }
        #endregion

        #region 评分标准配置里的操作
        /// <summary>
        /// 双击列表视为编辑操作 注释有误 
        /// edit by 王冀 2012-11-9  双击功能没有 去掉 改为添加编辑按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControl3_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridView4.FocusedRowHandle < 0)
                    return;
                DataRow foucesRow = gridView4.GetDataRow(gridView4.FocusedRowHandle);
                if (foucesRow == null)
                    return;

                if (foucesRow.IsNull("id"))
                    return;

                SetReductPageValue(SetReductEntityByRow(foucesRow));
                //m_EditState = EditState.Edit;   edit by 王冀 2012-11-9 
                //BtnState("2");
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
                throw;
            }

        }

        /// <summary>
        /// 将grid中的值赋值给实体
        ///  edit by 王冀 2012-11-9
        ///  edit by 王冀 2012-11-29
        /// </summary>
        /// <param name="foucesRow"></param>
        /// <returns></returns>
        private ConfigReduction SetReductEntityByRow(DataRow foucesRow)
        {
            try
            {
                if (foucesRow == null)
                {
                    return null;
                }
                ConfigReduction myReduction = new ConfigReduction();
                myReduction.REDUCEPOINT = foucesRow["REDUCEPOINT"].ToString();
                myReduction.PROBLEMDESC = foucesRow["PROBLEM_DESC"].ToString();
                myReduction.Valid = foucesRow["VALID"].ToString();
                myReduction.ID = foucesRow["ID"].ToString();
                myReduction.ParentsCode = foucesRow["ccode"].ToString();
                myReduction.Parents = foucesRow["CNAME"].ToString();
                myReduction.Child = foucesRow["CHILDNAME"].ToString();
                myReduction.ChildCode = foucesRow["childcode"].ToString();
                myReduction.Isauto = foucesRow["ISAUTO"].ToString();
                myReduction.Selectcondition = foucesRow["Selectcondition"].ToString();
                return myReduction;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
                throw;
            }

        }

        /// <summary>
        /// 将实体值赋给页面元素
        /// edit by 王冀 2012-11-9
        /// edit by 王冀 2012-11-29
        /// </summary>
        /// <param name="configEmrPoint"></param>
        private void SetReductPageValue(ConfigReduction configReduction)
        {
            try
            {
                if (configReduction == null)
                    return;
                txtPoint.Text = configReduction.REDUCEPOINT;
                cmbValidName.SelectedIndex = Int32.Parse(configReduction.Valid);
                memoDESC.Text = configReduction.PROBLEMDESC;
                lookUpEditParents.EditValue = configReduction.ParentsCode;
                lookUpEditChild.EditValue = configReduction.ChildCode;
                if (configReduction.Isauto == "0" || configReduction.Isauto == "1")
                {
                    comboBoxEdit1.SelectedIndex = Int32.Parse(configReduction.Isauto);
                }
                else
                {
                    comboBoxEdit1.SelectedIndex = -1;
                }
                textEdit2.Text = configReduction.Selectcondition;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 加载父类别下拉框数据
        /// 王冀 2012-11-9
        /// </summary>
        private void InitlookUpEditParents()
        {
            try
            {
                DataTable dt = m_SqlManager.GetReductionParents();
                lookUpEditParents.Properties.DataSource = dt;
                lookUpEditParents.Properties.ValueMember = "ID";
                lookUpEditParents.Properties.DisplayMember = "NAME";
                lookUpEditParents.Properties.NullText = "";
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 根据父类别的选中项加载子类别下拉框数据
        /// 王冀 2012-11-9
        /// </summary>
        private void InitlookUpEditChild()
        {
            try
            {
                DataTable dt = m_SqlManager.GetReductionChild();

                if (lookUpEditParents.EditValue.ToString().Trim() != "")
                {
                    dt = ToDataTable(dt.Select("ccode='" + lookUpEditParents.EditValue.ToString() + "'"));
                }

                lookUpEditChild.Properties.DataSource = dt;
                lookUpEditChild.Properties.ValueMember = "ID";
                lookUpEditChild.Properties.DisplayMember = "NAME";
                lookUpEditChild.Properties.NullText = "";
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 数组DataRow[]转化成表DataTable
        /// 王冀 2012 11 9
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        public DataTable ToDataTable(DataRow[] rows)
        {
            try
            {
                if (rows == null || rows.Length == 0) return null;
                DataTable tmp = rows[0].Table.Clone();
                foreach (DataRow row in rows)
                    tmp.ImportRow(row);
                return tmp;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
                throw;
            }

        }
        /// <summary>
        /// 父类别选中项改变事件 
        /// 王冀 2012-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lookUpEditParents_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string ID = this.lookUpEditParents.EditValue.ToString();
                InitlookUpEditChild();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }

        }

        /// <summary>
        /// 新增
        /// edit 王冀 2012-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                m_EditState = EditState.Add;
                ClearPageValue("2");
                BtnState("2");
                //gridControl3.Enabled = false;
                this.cmbValidName.SelectedIndex = 1;
                this.memoDESC.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 删除
        /// edit by 王冀 2012-11-9
        /// edit by 王冀 2012-12-11
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (DialogResult.OK != m_app.CustomMessageBox.MessageShow("确定要删除记录？", DrectSoft.Core.CustomMessageBoxKind.QuestionOkCancel))
                {
                    return;
                }
                if (gridView4.FocusedRowHandle < 0)
                {
                    m_app.CustomMessageBox.MessageShow("请选择要删除的记录！");
                    return;
                }
                DataRow foucesRow = gridView4.GetDataRow(gridView4.FocusedRowHandle);
                if (foucesRow == null)
                {
                    m_app.CustomMessageBox.MessageShow("请选择要删除的记录！");
                    return;
                }
                try
                {
                    m_SqlManager.SaveReductionData(SetEntityReduction(), "3");
                    rowHandle = gridView4.FocusedRowHandle - 1;
                    m_app.CustomMessageBox.MessageShow("删除成功！");
                    RefreshData("2");
                    if (rowHandle >= 0)
                    {
                        gridView4.FocusedRowHandle = rowHandle;
                    }
                }
                catch (Exception)
                {
                    m_app.CustomMessageBox.MessageShow("删除失败！");
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        int rowHandle = 0;
        /// <summary>
        /// 获取焦点所在行号 
        /// add 王冀 2012 12 18
        /// </summary>
        private void GetFocusRow()
        {
            try
            {
                if (m_EditState == EditState.Add)
                {
                    string rowid;
                    DataTable m_ReductionDt = new DataTable();
                    m_ReductionDt = m_SqlManager.GetFocusReductionData(lookUpEditParents.EditValue.ToString(), lookUpEditChild.EditValue.ToString(), memoDESC.Text);
                    if (m_ReductionDt == null || m_ReductionDt.Rows.Count == 0)
                    {
                        return;
                    }
                    rowid = m_ReductionDt.Rows[0]["id"].ToString();

                    m_ReductionDt = m_SqlManager.GetReductionData();
                    foreach (DataRow dr in m_ReductionDt.Rows)
                    {
                        if (dr["id"].ToString() == rowid)
                        {
                            rowHandle = gridView4.GetRowHandle(Int32.Parse(dr["ROWNUM"].ToString()) - 1);
                            break;
                        }
                    }

                }
                else
                    if (m_EditState == EditState.Edit)
                    {
                        rowHandle = gridView4.FocusedRowHandle;
                    }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 保存操作
        /// edit by 王冀 2012-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_EditState == EditState.View || m_EditState == EditState.None)
                {
                    return;
                }
                if (!IsSave("2"))
                    return;
                string isauto = comboBoxEdit1.Text.ToString() == "" ? "" : comboBoxEdit1.SelectedIndex.ToString();
                string id = "";
                if (m_EditState == EditState.Edit)
                {
                    DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);
                    id = dr["id"].ToString();
                }
                if (CheckItemValue(id, memoDESC.Text.Trim(), txtPoint.Text, lookUpEditParents.EditValue.ToString(), lookUpEditChild.EditValue.ToString(), isauto, textEdit2.Text))  //,
                {
                    //m_app.CustomMessageBox.MessageShow("编辑成功");
                    //m_EditState = EditState.View;
                    //BtnState("2");
                    //ClearPageValue("2");
                    //gridControl3.Enabled = true;

                    m_app.CustomMessageBox.MessageShow("内容已存在");
                    m_EditState = EditState.View;
                    BtnState("2");
                    ClearPageValue("2");
                    gridControl3.Enabled = true;
                    return;

                }
                if (SaveReductionData(SetEntityReduction()))
                {
                    if (m_EditState == EditState.Add)
                    {
                        m_app.CustomMessageBox.MessageShow("新增成功");
                    }
                    else
                    {
                        m_app.CustomMessageBox.MessageShow("修改成功");
                    }
                    GetFocusRow();
                    RefreshData("2");

                    gridView4.FocusedRowHandle = rowHandle;
                }
                else
                {
                    if (m_EditState == EditState.Add)
                    {
                        m_app.CustomMessageBox.MessageShow("新增失败");
                    }
                    else
                    {
                        m_app.CustomMessageBox.MessageShow("修改失败");
                    }
                }
                //gridControl3.Enabled = true;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }

        }

        /// <summary>
        /// 加载评分标准配置的数据(评分标准配置)
        /// </summary>
        private void LoadReductionData()
        {
            try
            {
                DataTable m_ReductionDt = new DataTable();
                m_ReductionDt = m_SqlManager.GetReductionData();
                gridControl3.DataSource = m_ReductionDt;//绑定数据源
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        ///  将页面值加到实体里(评分类别配置里的实体)
        ///  edit by 王冀 2012-11-9
        ///  edit by 王冀 2012-11-29
        /// </summary>
        /// <returns></returns>
        private ConfigReduction SetEntityReduction()
        {
            try
            {

                ConfigReduction ReductionInfo = new ConfigReduction();
                //ReductionInfo.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                ReductionInfo.CreateTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
                ReductionInfo.CreateUserID = m_app.User.Id;
                ReductionInfo.CreateUserName = m_app.User.Name;
                ReductionInfo.PROBLEMDESC = memoDESC.Text.ToString().Trim();
                ReductionInfo.REDUCEPOINT = txtPoint.Text.ToString();
                ReductionInfo.Valid = "1";
                ReductionInfo.ChildCode = lookUpEditChild.EditValue.ToString();
                ReductionInfo.ParentsCode = lookUpEditParents.EditValue.ToString();
                ReductionInfo.Isauto = comboBoxEdit1.Text.ToString() == "" ? "" : comboBoxEdit1.SelectedIndex.ToString();
                ReductionInfo.Selectcondition = textEdit2.Text;
                DataRow foucesRow = gridView4.GetDataRow(gridView4.FocusedRowHandle);
                if (foucesRow != null)
                {
                    ReductionInfo.ID = foucesRow["id"].ToString();
                }
                return ReductionInfo;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
                throw;
            }

        }
        /// <summary>
        /// 进行保存操作 
        /// </summary>
        /// <param name="configEmrPoint"></param>
        /// <returns></returns>
        private bool SaveReductionData(ConfigReduction configReduction)
        {
            try
            {
                string edittype = "";
                if (m_EditState == EditState.Add)
                    edittype = "1";
                else
                    edittype = "2";
                m_SqlManager.SaveReductionData(configReduction, edittype);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 取消操作
        /// edit by 王冀 2012-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                ClearPageValue("2");
                m_EditState = EditState.View;
                BtnState("2");
                //gridControl3.Enabled = true;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        #endregion

        /// <summary>
        /// 自动获取焦点//add by wyt 2012-11-02
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            try
            {
                int index = this.xtraTabControl1.SelectedTabPageIndex;
                switch (index)
                {
                    case 0:
                        this.lookUpCCNAME.SelectedText = this.lookUpCCNAME.Text;
                        this.btnADD.Focus();
                        break;
                    case 1:
                        this.simpleButton4.Focus();
                        break;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }


        /// <summary>
        /// add by wyt 自动获取焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfigPoint_Activated(object sender, EventArgs e)
        {
            try
            {
                this.btnADD.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void gViewConfigPoint_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.gViewConfigPoint.FocusedRowHandle == -1)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void gViewConfigPoint_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DrectSoft.Common.DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void gridView4_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DrectSoft.Common.DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 编辑button事件
        /// edit by 王冀 2012-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DevButtonEdit1_Click(object sender, EventArgs e)
        {
            try
            {
                m_EditState = EditState.Edit;
                BtnState("2");
                gridControl3_Click(null, null);
                this.memoDESC.Focus();
                //gridControl3.Enabled = false;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 项目明细配置表选中行改变时发生（比原来的单击事件添加了提示保存功能）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView4_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (m_EditState == EditState.Add || m_EditState == EditState.Edit)
                {
                    if (DialogResult.OK != m_app.CustomMessageBox.MessageShow("是否保存记录？", DrectSoft.Core.CustomMessageBoxKind.QuestionOkCancel))
                    {
                        simpleButton1_Click(sender, e);
                        gridControl3_Click(sender, e);
                        return;
                    }
                    simpleButton3_Click(sender, e);
                    if (m_EditState == EditState.Edit)
                    {
                        gridControl3_Click(sender, e);
                    }

                    return;
                }
                gridControl3_Click(sender, e);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 分类配置表选中行改变时发生（比原来的单击事件添加了提示保存功能）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gViewConfigPoint_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {

                InitPointClass();
                if (m_EditState == EditState.Add || m_EditState == EditState.Edit)
                {
                    if (DialogResult.OK != m_app.CustomMessageBox.MessageShow("是否保存记录？", DrectSoft.Core.CustomMessageBoxKind.QuestionOkCancel))
                    {
                        BtnClear_Click(sender, e);
                        gridControl1_Click(sender, e);
                        return;
                    }
                    btnSave_Click(sender, e);
                    if (m_EditState != EditState.Add)
                    {
                        gridControl1_Click(sender, e);
                    }

                    return;
                }
                gridControl1_Click(sender, e);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
    }
}