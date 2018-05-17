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

namespace DrectSoft.Emr.QcManager
{
    /// <summary>
    /// 用于维护病历评分中的各分类的维护操作 add by ywk 2012年3月31日15:30:14
    /// </summary>
    public partial class ConfigPoint : DevExpress.XtraEditors.XtraForm, IStartPlugIn
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
            LoadConfigData();
            InitParentClass();

            //加载评分标准配置的数据add by ywk 2012年5月25日 16:42:35
            LoadReductionData();
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
                }
            }
            else if (m_EditState == EditState.Add || m_EditState == EditState.Edit)
            {
                if (type == "1")
                {
                    this.btnADD.Enabled = false;
                    this.btnDel.Enabled = false;
                    this.btnSave.Enabled = true;
                    this.BtnClear.Enabled = true;

                    if (m_EditState == EditState.Add)
                        this.txtId.Enabled = true;
                    else
                        this.txtId.Enabled = false;
                    this.lookUpCCNAME.Enabled = true;
                    this.txtChildName.Enabled = true;
                    this.btnADD.Enabled = true;
                    this.btnDel.Enabled = true;
                    cmbValid.Enabled = true;
                }

                if (type == "2")
                {
                    this.simpleButton4.Enabled = false;//新增按钮
                    this.simpleButton2.Enabled = false;//删除按钮
                    this.simpleButton3.Enabled = true;//保存按钮
                    this.simpleButton1.Enabled = true;//取消按钮
                    if (m_EditState == EditState.Edit)//编辑操作可进行删除
                    {
                        this.simpleButton2.Enabled = true;//删除按钮
                    }
                    this.txtPoint.Enabled = true;//扣分标准
                    this.cmbValidName.Enabled = true;
                    this.memoDESC.Enabled = true;
                }
            }
        }
        /// <summary>
        /// 刷新数据
        /// </summary>
        private void RefreshData(string type)
        {
            if (type == "1")
            {
                LoadConfigData();
                m_EditState = EditState.View;
                BtnState(type);
                ClearPageValue(type);
            }
            if (type == "2")
            {
                LoadReductionData();
                m_EditState = EditState.View;
                BtnState(type);
                ClearPageValue(type);
            }
        }
        /// <summary>
        /// 清空窗体所有输入的值
        /// </summary>
        private void ClearPageValue(string type)
        {
            if (type == "1")
            {
                //类别设置里的
                this.lookUpCCNAME.CodeValue = "";
                this.txtId.Text = "";
                txtChildName.Text = "";
                cmbValid.SelectedIndex = -1;
            }

            //评分标准配置里的
            if (type == "2")
            {
                this.txtPoint.Text = "";
                this.cmbValidName.SelectedIndex = -1;
                this.memoDESC.Text = "";
            }
        }
        /// <summary>
        /// 判断保存操作是否成功
        /// </summary>
        /// <returns></returns>
        private bool IsSave(string type)
        {
            if (type == "1")//类别设置里的
            {
                if (this.txtId.Text.Trim() == "")
                {
                    m_app.CustomMessageBox.MessageShow("请输入子类编号！");
                    return false;
                }
                if (lookUpCCNAME.CodeValue == "")
                {
                    m_app.CustomMessageBox.MessageShow("请选择所属类型！");
                    return false;
                }
                if (txtChildName.Text.Trim() == "")
                {
                    m_app.CustomMessageBox.MessageShow("请输入子类名称！");
                    return false;
                }
                if (this.cmbValid.SelectedIndex == -1)
                {
                    m_app.CustomMessageBox.MessageShow("请选择是否有效！");
                    return false;
                }
            }
            if (type == "2")//评分配置里的
            {
                if (txtPoint.Text.Trim() == "")
                {
                    m_app.CustomMessageBox.MessageShow("请输入扣分标准！");
                    return false;
                }
                if (this.cmbValidName.SelectedIndex == -1)
                {
                    m_app.CustomMessageBox.MessageShow("请选择是否有效！");
                    return false;
                }
                if (this.memoDESC.Text.Trim() == "")
                {
                    m_app.CustomMessageBox.MessageShow("请输入扣分理由！");
                    return false;
                } if (this.cboUserType.SelectedIndex == -1)
                {
                    m_app.CustomMessageBox.MessageShow("请选择使用者类别！");
                    return false;
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
            cols.Add("CCODE", 65);
            cols.Add("CNAME", 160);
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
        }
        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            string modelid = this.txtId.Text.Trim();
            if (modelid == "")
            {
                m_app.CustomMessageBox.MessageShow("请选择要删除的记录！");
                return;
            }
            try
            {
                m_SqlManager.SaveData(SetEntityByPage(), "3");
                m_app.CustomMessageBox.MessageShow("删除成功！");
                RefreshData("1");

            }
            catch (Exception)
            {
                m_app.CustomMessageBox.MessageShow("删除失败！");
            }

        }
        /// <summary>
        /// 保存操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsSave("1"))
                return;
            if (SaveData(SetEntityByPage()))
            {
                if (m_EditState == EditState.Add)
                {
                    m_app.CustomMessageBox.MessageShow("新增成功！");
                }
                else
                {
                    m_app.CustomMessageBox.MessageShow("修改成功！");
                }
                RefreshData("1");
            }
            else
            {
                if (m_EditState == EditState.Add)
                {
                    m_app.CustomMessageBox.MessageShow("新增失败！");
                }
                else
                {
                    m_app.CustomMessageBox.MessageShow("修改失败！");
                }
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
            m_EditState = EditState.Edit;
            BtnState("1");
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
            throw new NotImplementedException();
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
            configemrPoint.CChildCode = txtId.Text.Trim();//小分类编号
            configemrPoint.CChildName = txtChildName.Text.Trim();//小分类名称
            configemrPoint.Valid = cmbValid.SelectedIndex.ToString();
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
        /// 双击列表视为编辑操作 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControl3_Click(object sender, EventArgs e)
        {
            if (gridView4.FocusedRowHandle < 0)
                return;
            DataRow foucesRow = gridView4.GetDataRow(gridView4.FocusedRowHandle);
            if (foucesRow == null)
                return;

            if (foucesRow.IsNull("id"))
                return;

            SetReductPageValue(SetReductEntityByRow(foucesRow));
            m_EditState = EditState.Edit;
            BtnState("2");
        }

        /// <summary>
        /// 将grid中的值赋值给实体
        /// </summary>
        /// <param name="foucesRow"></param>
        /// <returns></returns>
        private ConfigReduction SetReductEntityByRow(DataRow foucesRow)
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
            myReduction.Usertype = foucesRow["USER_TYPE"].ToString();
            return myReduction;
        }

        /// <summary>
        /// 将实体值赋给页面元素
        /// </summary>
        /// <param name="configEmrPoint"></param>
        private void SetReductPageValue(ConfigReduction configReduction)
        {
            if (configReduction == null)
                return;
            txtPoint.Text = configReduction.REDUCEPOINT;
            cmbValidName.SelectedIndex = Int32.Parse(configReduction.Valid);
            memoDESC.Text = configReduction.PROBLEMDESC;
            cboUserType.SelectedItem = configReduction.Usertype;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            m_EditState = EditState.Add;
            ClearPageValue("2");
            BtnState("2");
            this.cmbValidName.SelectedIndex = 1;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            DataRow foucesRow = gridView4.GetDataRow(gridView4.FocusedRowHandle);
            if (foucesRow == null)
            {
                m_app.CustomMessageBox.MessageShow("请选择要删除的记录！");
                return;
            }
            try
            {
                m_SqlManager.SaveReductionData(SetEntityReduction(), "3");
                m_app.CustomMessageBox.MessageShow("删除成功！");
                RefreshData("2");
            }
            catch (Exception)
            {
                m_app.CustomMessageBox.MessageShow("删除失败！");
            }
        }
        /// <summary>
        /// 保存操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (!IsSave("2"))
                return;

            if (SaveReductionData(SetEntityReduction()))
            {
                if (m_EditState == EditState.Add)
                {
                    m_app.CustomMessageBox.MessageShow("新增成功！");
                }
                else
                {
                    m_app.CustomMessageBox.MessageShow("修改成功！");
                }
                RefreshData("2");
            }
            else
            {
                if (m_EditState == EditState.Add)
                {
                    m_app.CustomMessageBox.MessageShow("新增失败！");
                }
                else
                {
                    m_app.CustomMessageBox.MessageShow("修改失败！");
                }
            }
        }
        /// <summary>
        /// 加载评分标准配置的数据(评分标准配置)
        /// </summary>
        private void LoadReductionData()
        {
            DataTable m_ReductionDt = new DataTable();
            m_ReductionDt = m_SqlManager.GetReductionData();
            gridControl3.DataSource = m_ReductionDt;//绑定数据源
        }
        /// <summary>
        ///  将页面值加到实体里(评分类别配置里的实体)
        /// </summary>
        /// <returns></returns>
        private ConfigReduction SetEntityReduction()
        {
            ConfigReduction ReductionInfo = new ConfigReduction();
            //ReductionInfo.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            ReductionInfo.CreateTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
            ReductionInfo.CreateUserID = m_app.User.Id;
            ReductionInfo.CreateUserName = m_app.User.Name;
            ReductionInfo.PROBLEMDESC = memoDESC.Text.ToString().Trim();
            ReductionInfo.REDUCEPOINT = txtPoint.Text.ToString();
            ReductionInfo.Valid = cmbValidName.SelectedIndex.ToString();
            ReductionInfo.Usertype =  cboUserType.SelectedItem.ToString();
            DataRow foucesRow = gridView4.GetDataRow(gridView4.FocusedRowHandle);
            if (foucesRow != null)
            {
                ReductionInfo.ID = foucesRow["id"].ToString();
            }
            return ReductionInfo;
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
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            ClearPageValue("2");
            m_EditState = EditState.View;
            BtnState("2");
        }

        #endregion


    }
}