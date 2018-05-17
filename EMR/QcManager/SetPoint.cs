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
using DrectSoft.Emr.Util;
namespace DrectSoft.Emr.QcManager
{
    /// <summary>
    /// 用于维护病历评分中的各分类的维护操作 add by ywk 2012年3月31日15:30:14
    /// </summary>
    public partial class SetPoint : DevBaseForm, IStartPlugIn
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

        SqlManger m_SqlManger;
        string m_NoOfInpat;     //当前病人首页序号
        string m_PatinetName;   //当前病人姓名
        EmrModel m_EmrModel;
        EmrModelContainer m_EmrModelContainer;
        string m_chiefID = ""; //当前综合评分ID
        QCType m_type = QCType.FINAL;
        Authority m_auth = Authority.DEPTQC;
        private int SumPoint { get; set; }//满分值。通过配置中取得 ywk 2012年6月12日 14:43:29 
        public SetPoint()
        {
            InitializeComponent();
        }

        //    bool RefreshD = false;//此页面关闭后，上级页面数据要相应的刷新数据
        public SetPoint(IEmrHost app)
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
          
        
            InitTotalGrade();
            InitLookUpEditorDoctor();
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
           // lookUpCCNAME.SqlWordbook = deptWordBook;
        }

        #endregion

        #region 共用方法
        /// <summary>
        ///  通过判断不同类型操作控件按钮状态
        /// </summary>
        private void BtnState(string type)
        {
            ////查看详细状态
            //if (m_EditState == EditState.View)
            //{
               
            //    if (type == "2")
            //    {
                    
            //        this.simpleButton2.Enabled = true;//删除按钮
            //        this.simpleButton3.Enabled = false;//保存按钮
            //        this.simpleButton1.Enabled = false;//取消按钮
                  
            //    }
            //    //if (type == "3")
            //    //{
            //    //    this.simpleButton8.Enabled = true;//新增按钮
            //    //    this.simpleButton6.Enabled = true;//删除按钮
            //    //    this.simpleButton7.Enabled = false;//保存按钮
            //    //    this.simpleButton5.Enabled = false;//取消按钮
            //    //    this.lookUpEdit2.Enabled = false;//扣分标准
            //    //    this.lookUpEdit1.Enabled = false;
            //    //    this.lookUpEdit3.Enabled = false;
            //    //    this.textEdit1.Enabled = false;
            //    //    this.memoEdit1.Enabled = false;
            //    //}
            //}
            //else if (m_EditState == EditState.Add || m_EditState == EditState.Edit)
            //{
                

            //    if (type == "2")
            //    {
                 
            //        this.simpleButton2.Enabled = false;//删除按钮
            //        this.simpleButton3.Enabled = true;//保存按钮
            //        this.simpleButton1.Enabled = true;//取消按钮
                 
            //        if (m_EditState == EditState.Edit)//编辑操作可进行删除
            //        {
            //            this.simpleButton2.Enabled = true;//删除按钮
            //        }
            //    ;
            //    }
            //    //if (type == "3")
            //    //{
            //    //    this.simpleButton8.Enabled = false;//新增按钮
            //    //    this.simpleButton6.Enabled = false;//删除按钮
            //    //    this.simpleButton7.Enabled = true;//保存按钮
            //    //    this.simpleButton5.Enabled = true;//取消按钮
            //    //    if (m_EditState == EditState.Edit)//编辑操作可进行删除
            //    //    {
            //    //        this.simpleButton6.Enabled = true;//删除按钮
            //    //    }
            //    //    this.lookUpEdit2.Enabled = true;//扣分标准
            //    //    this.lookUpEdit1.Enabled = true;
            //    //    this.lookUpEdit3.Enabled = true;
            //    //    this.textEdit1.Enabled = true;
            //    //    this.memoEdit1.Enabled = true;
            //    //}
            //}
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
           
            //评分标准配置里的
            if (type == "2")
            {
             
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
          //  lookUpCCNAME.SqlWordbook = deptWordBook;
        }

        /// <summary>
        /// 得到病历评分配置数据信息 (评分类别)
        /// </summary>
        private void LoadConfigData()
        {
            DataTable m_ConfigDt = new DataTable();
            m_ConfigDt = m_SqlManager.GetConfigPoint();
        
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
          //  lookUpCCNAME.Focus();
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
            //    lookUpCCNAME.Enabled = false;
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

                //if (lookUpEditParents.EditValue.ToString().Trim() != "")
                //{
                //    dt = ToDataTable(dt.Select("ccode='" + lookUpEditParents.EditValue.ToString() + "'"));
                //}

                //lookUpEditChild.Properties.DataSource = dt;
                //lookUpEditChild.Properties.ValueMember = "ID";
                //lookUpEditChild.Properties.DisplayMember = "NAME";
                //lookUpEditChild.Properties.NullText = "";
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
                GetPoint(m_NoOfInpat, m_chiefID);
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
                //if (m_EditState == EditState.Add)
                //{
                //    string rowid;
                //    DataTable m_ReductionDt = new DataTable();
                //    m_ReductionDt = m_SqlManager.GetFocusReductionData(lookUpEditParents.EditValue.ToString(), lookUpEditChild.EditValue.ToString(), memoDESC.Text);
                //    if (m_ReductionDt == null || m_ReductionDt.Rows.Count == 0)
                //    {
                //        return;
                //    }
                //    rowid = m_ReductionDt.Rows[0]["id"].ToString();

                //    m_ReductionDt = m_SqlManager.GetReductionData();
                //    foreach (DataRow dr in m_ReductionDt.Rows)
                //    {
                //        if (dr["id"].ToString() == rowid)
                //        {
                //            rowHandle = gridView4.GetRowHandle(Int32.Parse(dr["ROWNUM"].ToString()) - 1);
                //            break;
                //        }
                //    }

                //}
                //else
                //    if (m_EditState == EditState.Edit)
                //    {
                //        rowHandle = gridView4.FocusedRowHandle;
                //    }
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
               
                    try
                    {
                        if (lookUpEditDoctor.CodeValue == "")
                        {
                            DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择责任医师!");
                            return;

                        }
                        Save();


                        m_app.CustomMessageBox.MessageShow("保存成功", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        m_app.CustomMessageBox.MessageShow("保存出现异常，请联系管理员", DrectSoft.Core.CustomMessageBoxKind.ErrorOk);
                    }
                
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }

        }
        private void Save()
        {

            string sqlDel = string.Format(@"delete from emr_point  where noofinpat='{0}' and  emr_mark_record_id='{1}'",m_NoOfInpat,m_chiefID);
            m_app.SqlHelper.ExecuteNoneQuery(sqlDel);
            
                  DataTable dt =gridControl3.DataSource as DataTable;
                  if (dt != null)
                  {
                      //DataRow[] drs = dt.Select("lenth(reducepoint) > 0 ");

                      foreach (DataRow dr in dt.Rows)
                      {
                          if (dr["count"].ToString().Trim() != "0")
                          {
                              EmrPoint emrPoint = new EmrPoint();
                              emrPoint.Valid = "1";
                              emrPoint.DoctorID = lookUpEditDoctor.CodeValue.ToString();
                              emrPoint.DoctorName = lookUpEditDoctor.Text;
                              emrPoint.CreateUserID = m_app.User.Id;
                              emrPoint.CreateUserName = m_app.User.Name;
                              emrPoint.ProblemDesc = dr["reason"].ToString().Trim();
                              emrPoint.RecordDetailID = "";
                              emrPoint.ReducePoint = dr["ReducePoint"].ToString().Trim();
                              //emrPoint.ReducePoint = lookUpEditGrade.EditValue.ToString();
                              //emrPoint.Grade = lookUpEditGrade.Text;
                              //if (lookUpEditTotalGrade.DisplayValue.ToString().Contains("."))//处理 是小数分数的情况
                              //{
                              //  decimal dec = Decimal.Parse(lookReduction.DisplayValue.ToString());
                              emrPoint.Grade = lookUpEditTotalGrade.Text;
                              //}
                              //else
                              //{
                              //    emrPoint.Grade = lookReduction.DisplayValue.ToString() + "级";
                              //}
                              emrPoint.Num = dr["count"].ToString().Trim();
                              emrPoint.Noofinpat = m_NoOfInpat;
                              emrPoint.RecordDetailName = "";
                              emrPoint.EMR_MARK_RECORD_ID = m_chiefID;

                              //大类别编号（AC，AB）
                              string id = "";//取到recorddetail主键ID 
                              DataTable dtRecord = new DataTable();
                              string searchsq = string.Format(@" select sortid from  recorddetail where id ='{0}' ", id);
                              dtRecord = m_app.SqlHelper.ExecuteDataTable(searchsq);
                              string sortid = "";
                              if (dtRecord.Rows.Count > 0)
                              {
                                  sortid = dtRecord.Rows[0]["sortid"].ToString();
                              }
                              else
                              {
                                  ////大项从dict_catalog表里取数据
                                  //string slq = string.Format(@" select ccode from   dict_catalog where cname='{0}'", lookUpEditEmrDoc.Text);
                                  //if (m_app.SqlHelper.ExecuteDataTable(slq).Rows.Count > 0)
                                  //{
                                  sortid = dr["ccode"].ToString();
                                  //}
                              }

                              //评分配置表的主键
                              //if (string.IsNullOrEmpty(lookUpEPoint.CodeValue))//评分配置表未进行配置，ID取recorddetail 里的相应的ID
                              //{
                              //    //string sqlsec = string.Format(@"select ID from recorddetail where sortid='{0}' and  name ='{1}'",lookUpEditEmrDoc.EditValue.ToString(),lookUpEditEmrDoc.Text);
                              //    //DataTable dtid = m_App.SqlHelper.ExecuteDataTable(sqlsec);
                              //    //if (dtid.Rows.Count > 0)
                              //    //{
                              //    //    emrPoint.EmrPointID = Int32.Parse(dtid.Rows[0]["ID"].ToString());
                              //    //}
                              //    //else
                              //    //{
                              //    //    emrPoint.EmrPointID = 0;
                              //    //}
                              //    //edit by wyt 2012-12-11
                              //    //emrPoint.EmrPointID = Int32.Parse(lookUpEditEmrDoc.EditValue.ToString());
                              //    emrPoint.EmrPointID = lookUpEditEmrDoc.EditValue.ToString();
                              //}
                              //else
                              //{
                              //    //edit by wyt 2012-12-11
                              //    //emrPoint.EmrPointID = Int32.Parse(lookUpEPoint.CodeValue);
                              //    //emrPoint.EmrPointID = lookUpEPoint.EditValue.ToString();
                              emrPoint.EmrPointID = dr["childcode"].ToString();     //edit by wangj 2013 2 20 此处应为childcode ，报表中无法关联editvalue
                              //}
                              emrPoint.EmrPointChildID = dr["id"].ToString(); ;
                              emrPoint.SortID = sortid;

                              m_SqlManger.InsertEmrPoint(emrPoint);
                          }

                      }

                  }
                  if (m_type == QCType.Dept)
                  {
                      SetRecordstate(m_NoOfInpat, "4705");
                  }

                     
                 
        }
        /// <summary>
        /// gaiian binglizhuangtai
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <param name="state"></param>
        public  void SetRecordstate(string noofinpat, string state)
        {
            try
            {
               
                if (!string.IsNullOrEmpty(noofinpat))
                {
                    // string sqlStr = " update RECORDDETAIL a set ISLOCK = 4701 where a.valid=1 and a.noofinpat = @noofinpat and exists(select 1 from InPatient b  where a.NoOfInpat = b.NoOfInpat and  b.Status in(1502,1503) and a.ISLOCK in(0,4700,4702,4703) or a.islock is null) ";
                    //add by zjy 2013-6-17
                    string sqlStr = " update INPATIENT  a set ISLOCK = " + state + " where a.noofinpat = '"+noofinpat+"' ";
                    m_app.SqlHelper.ExecuteNoneQuery(sqlStr);
                    string sqlStr2 = " update recorddetail  a set ISLOCK = " + state + " where a.noofinpat = '" + noofinpat + "' ";
                    m_app.SqlHelper.ExecuteNoneQuery(sqlStr2);

                   
                }
               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 加载评分标准配置的数据(评分标准配置)
        /// </summary>
        private void LoadReductionData()
        {
            try
            {
             //   setQcTimemesage();//加载时限评分信息
                DataTable m_ReductionDt = new DataTable();
                m_ReductionDt = m_SqlManager.GetReductionDatadetail(m_NoOfInpat,m_chiefID);
              
                gridControl3.DataSource = m_ReductionDt;//绑定数据源
               
                if (m_ReductionDt != null && m_ReductionDt.Rows.Count > 0)
                {
                    foreach (DataRow dr in m_ReductionDt.Rows)
                    {
                        if (dr["doctorid"] != null && dr["doctorid"].ToString().Trim() != "")
                        {
                            lookUpEditDoctor.CodeValue = dr["doctorid"].ToString().Trim();
                            break;
                        }
                    }

                }
                DataTable dt_QCTime = new DataTable();
                dt_QCTime = m_SqlManager.GeQcTimeDatadetail(m_NoOfInpat, m_chiefID);
                gridControlQCTime.DataSource = dt_QCTime;
                GetPoint(m_NoOfInpat, m_chiefID);
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
           ReductionInfo.Valid = "1";
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
                   
                    case 1:
                     
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
              //  this.btnADD.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void gViewConfigPoint_DoubleClick(object sender, EventArgs e)
        {
           
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

        private void gridView4_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                if (e.Column.Caption == "缺陷个数")
                {
                    int count = 0;
                    try
                    {
                        count = int.Parse(e.Value.ToString());
                    }
                    catch
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请输入正确的缺陷个数!");
                        return;
                    }

                    DataRow row = gridView4.GetDataRow(e.RowHandle);

                    row["point"] = (decimal.Parse(row["REDUCEPOINT"].ToString()) * count).ToString();
                }
            }

        }
        public void InitData(IEmrHost host, SqlManger sqlManger)
        {
           m_app = host;
            m_SqlManger = sqlManger;
        }
        /// <summary>
        /// 初始化评分控件
        /// </summary>
        /// <param name="auth">质控权限</param>
        /// <param name="check">质控记录审核状态</param>
        /// <param name="type">质控记录类型</param>
        /// <param name="noofinpat">病人首页序号</param>
        /// <param name="qcid">综合评分ID</param>
        /// <param name="emrModel">病历文件</param>
        /// <param name="emrModelContainer">病历文件容器</param>
        public void RefreshLookUpEditorEmrDoc(Authority auth, CheckState check, QCType type, string noofinpat, string chiefID, EmrModel emrModel, EmrModelContainer emrModelContainer)
        {
            try
            {
                m_NoOfInpat = noofinpat;
                m_chiefID = chiefID;
                m_auth = auth;
                m_type = type;
                #region 根据质控类型设置总分
                string pointConfig = m_SqlManger.GetConfigValueByKey("EmrPointConfig");
                string[] points = pointConfig.Split(',');
                if (m_type == QCType.PART)
                {
                    SumPoint = Int32.Parse(points[0]);
                }
                else
                {
                    try
                    {
                        SumPoint = Int32.Parse(points[1]);
                    }
                    catch (Exception)
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请在系统参数配置中配置EmrPointConfig的终末质控总分数！");
                    }
                }
                #endregion
                InitTotalGrade();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void textEditTotalPoint_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == (char)13)
                {
                    SendKeys.Send("{Tab}");
                    SendKeys.Flush();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void InitTotalGrade()
        {
            try
            {
                DataTable dt = TotalGrade();
                lookUpEditTotalGrade.Properties.DataSource = dt;
                lookUpEditTotalGrade.Properties.ValueMember = "ID";
                lookUpEditTotalGrade.Properties.DisplayMember = "NAME";

                if (dt.Rows.Count > 0)
                {
                    lookUpEditTotalGrade.EditValue = dt.Rows[0]["ID"].ToString();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
      
        private DataTable TotalGrade()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ID");
                dt.Columns.Add("NAME");
                dt.Columns.Add("FROM");
                dt.Columns.Add("TO");

                if (SumPoint < 90)
                {
                    DataRow dr = dt.NewRow();

                    dr["ID"] = "2";
                    dr["NAME"] = "优秀";
                    dr["FROM"] = "80";
                    dr["TO"] = "89.9";

                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dr["ID"] = "3";
                    dr["NAME"] = "良好";
                    dr["FROM"] = "70";
                    dr["TO"] = "79.9";

                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dr["ID"] = "4";
                    dr["NAME"] = "及格";
                    dr["FROM"] = "60";
                    dr["TO"] = "69.9";

                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dr["ID"] = "5";
                    dr["NAME"] = "差";
                    dr["FROM"] = "50";
                    dr["TO"] = "59.9";

                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dr["ID"] = "6";
                    dr["NAME"] = "较差";
                    dr["FROM"] = "40";
                    dr["TO"] = "49.9";

                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dr["ID"] = "7";
                    dr["NAME"] = "非常差";
                    dr["FROM"] = "0";
                    dr["TO"] = "39.9";
                    dt.Rows.Add(dr);

                }
                if (SumPoint > 90)
                {
                    DataRow dr = dt.NewRow();
                    dr["ID"] = "1";
                    dr["NAME"] = "优秀";
                    dr["FROM"] = "90";
                    dr["TO"] = "100";
                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr["ID"] = "2";
                    dr["NAME"] = "良好";
                    dr["FROM"] = "80";
                    dr["TO"] = "89.9";

                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dr["ID"] = "3";
                    dr["NAME"] = "一般";
                    dr["FROM"] = "70";
                    dr["TO"] = "79.9";

                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dr["ID"] = "4";
                    dr["NAME"] = "及格";
                    dr["FROM"] = "60";
                    dr["TO"] = "69.9";

                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dr["ID"] = "5";
                    dr["NAME"] = "差";
                    dr["FROM"] = "50";
                    dr["TO"] = "59.9";

                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dr["ID"] = "6";
                    dr["NAME"] = "较差";
                    dr["FROM"] = "40";
                    dr["TO"] = "49.9";

                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dr["ID"] = "7";
                    dr["NAME"] = "非常差";
                    dr["FROM"] = "0";
                    dr["TO"] = "39.9";
                    dt.Rows.Add(dr);
                }

                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #region 计算得分情况 用100减去要扣除的分数

        private void GetPoint(string noofinpat, string recordid)
        {
            try
            {
                DataTable dt =gridControl3.DataSource as DataTable;
                if (dt != null)
                {
                    //DataRow[] drs = dt.Select("lenth(reducepoint) > 0 ");
                    double totalPoint = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        double point;
                        try
                        {
                            point = Convert.ToDouble(dr["point"].ToString());
                            totalPoint += point;
                        }
                        catch (Exception)
                        {
                            DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("扣分错误");
                        }
                        finally
                        {
                            point = 0;

                        }
                    }

                    DataTable DT_QCTIME = gridControlQCTime.DataSource as DataTable;
                    if (DT_QCTIME != null)
                    {
                        foreach (DataRow dr in DT_QCTIME.Rows)
                        {
                            double point;
                            try
                            {
                                point = Convert.ToDouble(dr["point"].ToString());
                                totalPoint += point;
                            }
                            catch (Exception)
                            {
                                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("扣分错误");
                            }
                            finally
                            {
                                point = 0;

                            }
                        }
                    }
                    //现在改为85分为满分
                    if ((SumPoint - totalPoint) > 0)
                    {
                        textEditTotalPoint.Text = Convert.ToString(SumPoint - totalPoint);
                    }
                    else
                    {
                        textEditTotalPoint.Text = "0";
                    }
                    //textEditTotalPoint.Text = Convert.ToString(100 - totalPoint);

                    SavePoint(textEditTotalPoint.Text, noofinpat, recordid);
                    GetGrade();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void SavePoint(string point, string noofinpat, string recordid)
        {
            try
            {
                string sql = @" update emr_automark_record set score='{0}' where noofinpat='{1}' and id='{2}'  ";

                sql = string.Format(sql, point, noofinpat, recordid);

               m_app.SqlHelper.ExecuteNoneQuery(sql);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void GetGrade()
        {
            try
            {
                if (textEditTotalPoint.Text.Trim() != "")
                {
                    DataTable dt = lookUpEditTotalGrade.Properties.DataSource as DataTable;
                    if (dt != null)
                    {
                        //double totalPoint = Convert.ToDouble(textEditTotalPoint.Text);
                        decimal totalPoint = Convert.ToDecimal(textEditTotalPoint.Text);
                        string selectValue = string.Empty;
                        foreach (DataRow dr in dt.Rows)
                        {
                            //int from = Convert.ToInt32(dr["from"]);
                            //int to = Convert.ToInt32(dr["to"]);

                            //if (totalPoint >= from && totalPoint <= to)
                            //{
                            //    selectValue = dr["id"].ToString();
                            //    break;
                            //}
                            decimal from = Convert.ToDecimal(dr["from"]);
                            decimal to = Convert.ToDecimal(dr["to"]);
                            if (totalPoint >= from && totalPoint <= to)
                            {
                                selectValue = dr["id"].ToString();
                                break;
                            }
                        }
                        lookUpEditTotalGrade.EditValue = selectValue;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        public void InitLookUpEditorDoctor()
        {
            #region  原来的取医师 已注释
            //if (m_EmrModel != null)
            //{
            //    //当前病历创建人所在科室的所有医生，默认选中病历创建人
            //    DataTable dt = m_SqlManger.GetAllDoctorByUserID(m_EmrModel.CreatorXH);
            //    lookUpEditDo1ctor.Properties.DataSource = GetDataTable(dt);
            //    lookUpEditDo1ctor.Properties.ValueMember = "ID";
            //    lookUpEditDo1ctor.Properties.DisplayMember = "NAME";
            //    lookUpEditDo1ctor.EditValue = m_EmrModel.CreatorXH;
            //}
            //else
            //{
            //    //当前病人所在科室的所有医生
            //    DataTable dt = m_SqlManger.GetAllDoctorByNoofinpat(m_NoOfInpat);
            //    lookUpEditDo1ctor.Properties.DataSource = GetDataTable(dt);
            //    lookUpEditDo1ctor.Properties.ValueMember = "ID";
            //    lookUpEditDo1ctor.Properties.DisplayMember = "NAME";
            //    //lookUpEditDoctor.EditValue = "";
            //    //if (dt.Rows.Count > 0)
            //    //{
            //    //    lookUpEditDoctor.EditValue = dt.Rows[0]["ID"].ToString();
            //    //}
            //    //责任医师默认选中这个病历的责任人
            //}
            #endregion
            try
            {
                if (lookUpEditDoctor.SqlWordbook == null)
                {
                    //edit by ywk 2012年4月1日17:01:56
                    lookUpWindowDoc.SqlHelper =m_app.SqlHelper;
                    string sql1 = string.Format(@"  select u1.id, u1.name,u1.py,u1.wb 
            from users u1  where u1.valid='1' ");
                    DataTable dtAllDoc = new DataTable();
                    dtAllDoc = m_app.SqlHelper.ExecuteDataTable(sql1);
                    //dtAllDoc = m_SqlManger.GetPointClass();
                    if (dtAllDoc.Rows.Count > 0)
                    {
                        dtAllDoc.Columns["ID"].Caption = "编号";
                        dtAllDoc.Columns["NAME"].Caption = "姓名";
                        Dictionary<string, int> cols = new Dictionary<string, int>();
                        cols.Add("ID", 70);
                        cols.Add("NAME", 80);
                        SqlWordbook deptWordBook = new SqlWordbook("querybook", dtAllDoc, "ID", "NAME", cols, "ID//NAME//PY//WB");
                        lookUpEditDoctor.SqlWordbook = deptWordBook;
                    }
                }

                    DataTable DtInpatDoc = new DataTable();

                    if (lookUpEditDoctor.CodeValue == "")
                    {
                        string search = string.Format(@" select resident from inpatient where noofinpat='{0}'", m_NoOfInpat);
                        if (m_app.SqlHelper.ExecuteDataTable(search).Rows.Count > 0)
                        {
                            lookUpEditDoctor.CodeValue = m_app.SqlHelper.ExecuteDataTable(search).Rows[0]["resident"].ToString();
                        }
                        else
                        {
                            lookUpEditDoctor.CodeValue = "";
                        }
                    }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void gridView2_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                if (e.Column.Caption == "缺陷个数")
                {
                    int count = 0;
                    try
                    {
                        count = int.Parse(e.Value.ToString());
                    }
                    catch
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请输入正确的缺陷个数!");
                        return;
                    }

                    DataRow row =gridView2.GetDataRow(e.RowHandle);

                    row["point"] = (decimal.Parse(row["point"].ToString()) * count).ToString();
                }
            }

        }
      
    }
}