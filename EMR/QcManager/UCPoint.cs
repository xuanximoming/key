using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Wordbook;
using DrectSoft.Emr.Util;

namespace DrectSoft.Emr.QcManager
{
    public partial class UCPoint : DevExpress.XtraEditors.XtraUserControl
    {
        IEmrHost m_App;
        SqlManger m_SqlManger;
        string m_NoOfInpat;     //当前病人首页序号
        string m_PatinetName;   //当前病人姓名
        EmrModel m_EmrModel;
        EmrModelContainer m_EmrModelContainer;
        string m_chiefID = ""; //当前综合评分ID
        QCType m_type = QCType.FINAL;
        Authority m_auth = Authority.DEPTQC;
        public UCPoint()
        {
            InitializeComponent();
        }

        public void InitData(IEmrHost host, SqlManger sqlManger)
        {
            m_App = host;
            m_SqlManger = sqlManger;
        }
        private int SumPoint { get; set; }//满分值。通过配置中取得 ywk 2012年6月12日 14:43:29 
        private void UCPoint_Load(object sender, EventArgs e)
        {
            try
            {
                //this.labelControl8.Visible = false;
                //this.lookUpEPoint.Visible = false;
                //this.lookUpEPoint.CodeValue = "";
                if (this.DesignMode) return;
                string pointConfig = m_SqlManger.GetConfigValueByKey("EmrPointConfig");
                try
                {
                    string[] points = pointConfig.Split(',');
                    if (m_type == QCType.PART)
                    {
                        SumPoint = Int32.Parse(points[0]);
                    }
                    else
                    {
                        SumPoint = Int32.Parse(points[1]);
                    }
                }
                catch (Exception)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请在系统参数配置中配置EmrPointConfig的质控总分数");
                    return;
                }

                //lookReduction.Enter += new EventHandler(lookReduction_Enter);
                //lookReduction.GotFocus += new EventHandler(lookReduction_Enter);
                lookReduction.MouseDown += new MouseEventHandler(lookReduction_MouseDown);
                lookUpEditEmrDoc.EditValueChanged += new EventHandler(lookUpEditEmrDoc_EditValueChanged);
                //lookUpEPoint.EditValue = "0";
                InitLookUpEditorEmrDoc();
                //InitGrade();//加载扣分项
                ReInitGrade();//重新加载扣分项
                InitEmrPoint("");//初始化小项下拉框
                InitLookUpEditorDoctor();
                InitTotalGrade();
                InitGridControl();
                InitPatientInfo();
                //InitButtonVisible();//edit by wyt 2012-12-05
                this.lookUpEditEmrDoc.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        void lookReduction_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                //InitGrade();//加载扣分项
                ReInitGrade();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        void lookReduction_Enter(object sender, EventArgs e)
        {
            try
            {
                //InitGrade();//加载扣分项
                ReInitGrade();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 只有管理员才能看到配置按钮
        /// </summary>
        private void InitButtonVisible()
        {
            try
            {
                if (m_App.User.Id.Trim() == "00")
                {
                    btnConfig.Visible = true;
                }
                else
                {
                    btnConfig.Visible = false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 选择问题病历，判断是哪个分类，然后联动其下面的评分项
        /// add by ywk 2012年4月1日9:10:35 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lookUpEditEmrDoc_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string sortid = "";
                this.lookReduction.CodeValue = "";
                this.memoEditDesc.Text = "";
                string id = lookUpEditEmrDoc.EditValue.ToString();//取到recorddetail主键ID 
                DataTable dtRecord = new DataTable();
                string searchsq = string.Format(@" select sortid from  recorddetail where id ='{0}' ", id);
                dtRecord = m_App.SqlHelper.ExecuteDataTable(searchsq);
                //if (lookUpEditEmrDoc.EditValue.ToString() != "0")
                //{
                //其中增加的大于0的判断，是判断是否点击的文件夹选项2012年6月13日 09:12:39ywk
                if (lookUpEditEmrDoc.EditValue.ToString() != "" && Int32.Parse(lookUpEditEmrDoc.EditValue.ToString()) > 0)
                {
                    if (dtRecord.Rows.Count > 0)
                    {
                        sortid = dtRecord.Rows[0]["sortid"].ToString();
                        InitEmrPoint(sortid);
                        InitLookUpEditorDoctor();
                        //InitGradeBySelectRecord(sortid);
                    }
                }
                else
                {
                    sortid = GetClassBySelectItemName(lookUpEditEmrDoc.Text);
                    this.labelControl8.Visible = false;
                    this.lookUpEPoint.Visible = false;
                    this.lookUpEPoint.CodeValue = "";
                    InitLookUpEditorDoctor();
                    //this.lookReduction.CodeValue = "";
                    //this.memoEditDesc.Text = "";
                }
                //InitLookUpEditorDoctor();
                InitGradeBySelectRecord(sortid);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void ReInitGrade()
        {
            try
            {
                string sortid = "";
                string id = lookUpEditEmrDoc.EditValue.ToString();
                DataTable dtRecord = new DataTable();
                string searchsq = string.Format(@" select sortid from  recorddetail where id ='{0}' ", id);
                dtRecord = m_App.SqlHelper.ExecuteDataTable(searchsq);
                if (lookUpEditEmrDoc.EditValue.ToString() != "" && Int32.Parse(lookUpEditEmrDoc.EditValue.ToString()) > 0)
                {
                    if (dtRecord.Rows.Count > 0)
                    {
                        sortid = dtRecord.Rows[0]["sortid"].ToString();
                    }
                }
                else
                {
                    sortid = GetClassBySelectItemName(lookUpEditEmrDoc.Text);
                }
                InitGradeBySelectRecord(sortid);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GetClassBySelectItemName(string selectitem)
        {
            try
            {
                string sql = string.Format(@"select ccode from dict_catalog where cname='{0}'", selectitem);

                DataTable dt = m_App.SqlHelper.ExecuteDataTable(sql);

                if (dt.Rows.Count == 0 || dt == null)
                {
                    return "";
                }
                return dt.Rows[0][0].ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 根据选择问题病历的分类，得到下面的小项，绑定到评分项下拉框里
        /// edit by ywk 
        /// </summary>
        /// <param name="sortid"></param>
        private void InitEmrPoint(string sortid)
        {
            try
            {
                lookUpEPoint.CodeValue = "";//选择完成一个大分类，要将原来的值清空 edit by ywk2012年5月30日 18:03:12
                lookUpWindowPoint.SqlHelper = m_App.SqlHelper;

                //string sql = string.Format(@" select childcode,childname,id from emr_configpoint where (ccode='{0}' or '{0}' is null)  and valid='1' ", sortid);
                string sql = string.Format(@" select childcode id,childname  from emr_configpoint where (ccode='{0}' or '{0}' is null)  and valid='1' ", sortid);        //edit by wangj 2013 2 20 手动添加的评分小项关联不到 childcode 报表中无法查看
                DataTable dtchilddata = new DataTable();
                // dtchilddata = m_SqlManger.GetPointClass(sortid);
                dtchilddata = m_App.SqlHelper.ExecuteDataTable(sql);
                if (dtchilddata.Rows.Count > 0)
                {
                    dtchilddata.Columns["ID"].Caption = "编号";
                    dtchilddata.Columns["CHILDNAME"].Caption = "评分项";
                    Dictionary<string, int> cols = new Dictionary<string, int>();

                    cols.Add("CHILDNAME", 150);
                    SqlWordbook deptWordBook = new SqlWordbook("querybook", dtchilddata, "ID", "CHILDNAME", cols);
                    lookUpEPoint.SqlWordbook = deptWordBook;
                    this.labelControl8.Visible = true;
                    this.lookUpEPoint.Visible = true;

                }
                else//如果娶不到数据，就不显示下拉框
                {
                    this.labelControl8.Visible = false;
                    this.lookUpEPoint.Visible = false;
                    this.lookUpEPoint.CodeValue = "";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region 初始化病人信息

        private void InitPatientInfo()
        {
            DataTable dt = m_SqlManger.GetPatientInfo(m_NoOfInpat);
            if (dt.Rows.Count > 0)
            {
                barButtonItemAge.Caption = dt.Rows[0]["agestr"].ToString();
                barButtonItemBedNo.Caption = dt.Rows[0]["outbed"].ToString();
                barButtonItemAdmitTime.Caption = dt.Rows[0]["admitdate"].ToString();
                barButtonItemName.Caption = dt.Rows[0]["name"].ToString();
                barButtonItemGender.Caption = dt.Rows[0]["gender"].ToString();
                barButtonItemPatID.Caption = dt.Rows[0]["patid"].ToString();

                m_PatinetName = dt.Rows[0]["name"].ToString();  //初始化当前病人姓名
            }
        }

        #endregion

        #region 初始化列表
        /// <summary>
        /// 界面初始化   edit by wangj 2013 1 18 界面置空
        /// </summary>
        private void InitGridControl()
        {
            try
            {
                this.lookUpEditEmrDoc.EditValue = "";
                //lookUpEditDoctor.EditValue = "";
                lookUpEPoint.EditValue = "";
                this.memoEditDescForLook.Text = "";
                this.lookReduction.CodeValue = "";
                this.memoEditDesc.Text = "";
                DataTable dt = m_SqlManger.GetEmrPointByNoofinpat(m_NoOfInpat, m_chiefID);
                gridControl1.DataSource = dt;
                GetPoint(m_NoOfInpat, m_chiefID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 初始化病历列表
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
                m_EmrModel = emrModel;
                m_EmrModelContainer = emrModelContainer;
                InitLookUpEditorEmrDoc();
                InitLookUpEditorDoctor();
                InitTotalGrade();
                InitGridControl();
                InitPatientInfo();
                InitQCManager(auth, check);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitQCManager(Authority auth, CheckState check)
        {
            try
            {
                switch (check)
                {
                    case CheckState.NEW:
                        this.panelControl1.Enabled = true;
                        break;
                    case CheckState.SUBMIT:
                        this.panelControl1.Enabled = false;
                        break;
                    case CheckState.CHECKIN:
                        this.panelControl1.Enabled = false;
                        break;
                    case CheckState.CHECKOUT:
                        this.panelControl1.Enabled = true;
                        break;
                    case CheckState.QC:
                        this.panelControl1.Enabled = true;
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 初始化病历列表
        /// </summary>
        public void InitLookUpEditorEmrDoc()
        {
            try
            {
                //先将默认的几个大评分项取出来。加到问题病历下拉框里（存储过程里配置是哪些）
                DataTable dt = m_SqlManger.GetAllEmrDocByNoofinpat(m_NoOfInpat);
                //加载病历列表默认显示出病案首页等的选项
                DataTable dtOtherOption = m_SqlManger.GetPointClass();//改为从存储过程里面取得数据
                if (dtOtherOption.Rows.Count > 0)
                {
                    int k = -1;
                    for (int i = 0; i < dtOtherOption.Rows.Count; i++)
                    {
                        DataRow my_dr = dt.NewRow();
                        my_dr["ID"] = (k - i).ToString();
                        my_dr["Name"] = dtOtherOption.Rows[i]["cname"].ToString();
                        dt.Rows.Add(my_dr);
                    }
                    lookUpEditEmrDoc.Properties.DataSource = GetDataTable(dtOtherOption);
                }
                lookUpEditEmrDoc.Properties.DataSource = GetDataTable(dt);
                lookUpEditEmrDoc.Properties.ValueMember = "ID";
                lookUpEditEmrDoc.Properties.DisplayMember = "NAME";

                //点击具体病历（根据具体的点击哪个评分项节点，下拉框进行绑定）
                //解决点击后，具体病历再点击文件夹节点时，下拉框绑定不正确的BUG ywk 2012年4月5日9:45:15
                if (m_EmrModel != null && m_EmrModelContainer == null)
                {
                    lookUpEditEmrDoc.EditValue = m_EmrModel.InstanceId.ToString();
                }
                // 此处可不要。会出现同一文件夹节点，两次评分而ID不相同的情况2012年7月3日 09:43:01
                //else if (m_EmrModelContainer != null)//点击文件夹根节点（病案首页、医嘱浏览...等）
                //{
                //    DataRow dr = dt.NewRow();
                //    dr["ID"] = "-100";
                //    dr["Name"] = m_EmrModelContainer.Name;
                //    dt.Rows.Add(dr);

                //    lookUpEditEmrDoc.Properties.DataSource = GetDataTable(dt);
                //    lookUpEditEmrDoc.Properties.ValueMember = "ID";
                //    lookUpEditEmrDoc.Properties.DisplayMember = "NAME";
                //    lookUpEditEmrDoc.EditValue = "-100";
                //}
                else
                {
                    lookUpEditEmrDoc.EditValue = "";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DataTable GetDataTable(DataTable dt)
        {
            try
            {
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("ID");
                dataTable.Columns.Add("NAME");
                foreach (DataRow dr in dt.Rows)
                {
                    DataRow row = dataTable.NewRow();
                    row.ItemArray = dr.ItemArray;
                    dataTable.Rows.Add(row);
                }
                return dataTable;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 初始化责任人列表
        /// <summary>
        /// 初始化责任人列表
        /// </summary>
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
                    lookUpWindowDoc.SqlHelper = m_App.SqlHelper;
                    string sql1 = string.Format(@"  select u1.id, u1.name,u1.py,u1.wb 
            from users u1  where u1.valid='1' ");
                    DataTable dtAllDoc = new DataTable();
                    dtAllDoc = m_App.SqlHelper.ExecuteDataTable(sql1);
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
                string sql = string.Format(@" select owner from recorddetail where noofinpat='{0}' and id='{1}' ", m_NoOfInpat, lookUpEditEmrDoc.EditValue.ToString());   //edit by wangj 2013 1 25  获取当前病例的责任医生
                DtInpatDoc = m_App.SqlHelper.ExecuteDataTable(sql);
                if (DtInpatDoc.Rows.Count > 0)
                {
                    lookUpEditDoctor.CodeValue = DtInpatDoc.Rows[0]["OWNER"].ToString();
                }
                else//没有创建医师就从inpant表里去床位医师 resident
                {
                    string search = string.Format(@" select resident from inpatient where noofinpat='{0}'", m_NoOfInpat);
                    if (m_App.SqlHelper.ExecuteDataTable(search).Rows.Count > 0)
                    {
                        lookUpEditDoctor.CodeValue = m_App.SqlHelper.ExecuteDataTable(search).Rows[0]["resident"].ToString();
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
        #endregion

        #region 扣分&&等级
        private void InitGrade()
        {
            try
            {
                lookUpWReduction.SqlHelper = m_App.SqlHelper;
                string sql1 = string.Format(@"   select a.reducepoint,a.problem_desc,a.id  from  
            EMR_ConfigReduction2 a where a.valid='1' ");
                DataTable dtReduction = new DataTable();
                dtReduction = m_App.SqlHelper.ExecuteDataTable(sql1);
                //if (dtReduction.Rows.Count > 0)///无论有没有数据，它的SqlWorkbook都不能为空edit by ywk 2012年5月30日 17:42:42
                //{
                dtReduction.Columns["ID"].Caption = "序号";
                dtReduction.Columns["PROBLEM_DESC"].Caption = "扣分理由";
                dtReduction.Columns["REDUCEPOINT"].Caption = "扣分标准";
                Dictionary<string, int> cols = new Dictionary<string, int>();
                //cols.Add("ID", 10);
                cols.Add("PROBLEM_DESC", 130);
                cols.Add("REDUCEPOINT", 70);
                SqlWordbook ReductWordBook = new SqlWordbook("querybook", dtReduction, "ID", "REDUCEPOINT", cols, "PROBLEM_DESC//REDUCEPOINT//ID");//REDUCEPOINT//PROBLEM_DESC
                lookReduction.SqlWordbook = ReductWordBook;
                //}
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void InitGradeBySelectRecord(string selectitem)
        {
            try
            {
                lookUpWReduction.SqlHelper = m_App.SqlHelper;
                string sql1 = string.Format(@"   select a.reducepoint,a.problem_desc,a.id  from  
            EMR_ConfigReduction2 a where a.valid='1' and (a.parents='{0}' or '{0}' is null  )", selectitem);
                DataTable dtReduction = new DataTable();
                dtReduction = m_App.SqlHelper.ExecuteDataTable(sql1);
                //if (dtReduction.Rows.Count > 0)///无论有没有数据，它的SqlWorkbook都不能为空edit by ywk 2012年5月30日 17:42:42
                //{
                dtReduction.Columns["ID"].Caption = "序号";
                dtReduction.Columns["PROBLEM_DESC"].Caption = "扣分理由";
                dtReduction.Columns["REDUCEPOINT"].Caption = "扣分标准";
                Dictionary<string, int> cols = new Dictionary<string, int>();
                //cols.Add("ID", 10);
                cols.Add("PROBLEM_DESC", 130);
                cols.Add("REDUCEPOINT", 70);
                SqlWordbook ReductWordBook = new SqlWordbook("querybook", dtReduction, "ID", "REDUCEPOINT", cols, "PROBLEM_DESC//REDUCEPOINT//ID");//REDUCEPOINT//PROBLEM_DESC
                lookReduction.SqlWordbook = ReductWordBook;
                //}
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 选择评分，带出扣分的理由
        /// add by ywk  2012年5月28日 10:36:46
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lookReduction_CodeValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (lookReduction.SqlWordbook == null)
                {
                    InitGrade();
                }
                string point = lookReduction.CodeValue.ToString();
                string search = string.Format(@"select problem_desc,children from EMR_ConfigReduction2 where id='{0}'", point);
                DataTable dt = m_App.SqlHelper.ExecuteDataTable(search, CommandType.Text);
                if (dt.Rows.Count > 0)
                {
                    memoEditDesc.Text = dt.Rows[0]["problem_desc"].ToString();
                    lookUpEPoint.CodeValue = dt.Rows[0]["children"].ToString(); //添加默认的子类
                }
                else
                {
                    memoEditDesc.Text = "";
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private DataTable Grade()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ID");
                dt.Columns.Add("NAME");
                DataRow dr = dt.NewRow();
                dr["ID"] = "0.5";
                dr["NAME"] = "0.5级";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr["ID"] = "1";
                dr["NAME"] = "1级";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr["ID"] = "1.5";
                dr["NAME"] = "1.5级";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr["ID"] = "2";
                dr["NAME"] = "2级";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr["ID"] = "3";
                dr["NAME"] = "3级";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr["ID"] = "4";
                dr["NAME"] = "4级";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr["ID"] = "5";
                dr["NAME"] = "5级";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr["ID"] = "6";
                dr["NAME"] = "6级";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr["ID"] = "7";
                dr["NAME"] = "7级";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr["ID"] = "8";
                dr["NAME"] = "8";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr["ID"] = "9";
                dr["NAME"] = "9级";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr["ID"] = "10";
                dr["NAME"] = "10级";
                dt.Rows.Add(dr);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 综合等级

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

        #endregion

        #region 增加
        private void simpleButtonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateBeforeSave())
                {
                    try
                    {
                        Save();
                        InitGridControl();
                        InitLookUpEditorDoctor();
                        m_App.CustomMessageBox.MessageShow("新增成功", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        m_App.CustomMessageBox.MessageShow("新增出现异常，请联系管理员", DrectSoft.Core.CustomMessageBoxKind.ErrorOk);
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private bool ValidateBeforeSave()
        {
            try
            {
                if (string.IsNullOrEmpty(m_NoOfInpat))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一个病人");
                    //m_App.CustomMessageBox.MessageShow("请选择一个病人", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                    return false;
                }
                if (string.IsNullOrEmpty(m_chiefID))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请先新建一条评分记录");
                    return false;
                }
                if (lookUpEditEmrDoc.EditValue.ToString() == "")
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请输入对应的病历");
                    //m_App.CustomMessageBox.MessageShow("请输入对应的病历", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                    return false;
                }
                //if (lookUpEditGrade.Text == "")
                //{
                //    m_App.CustomMessageBox.MessageShow("请选择扣分等级!", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                //    lookUpEditGrade.Focus();
                //    return false;
                //}
                if (lookReduction.CodeValue == "")
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择扣分标准");
                    //m_App.CustomMessageBox.MessageShow("请选择扣分标准", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                    lookReduction.Focus();
                    return false;
                }
                if (spinEditNum.Text == "")
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择次数");
                    //m_App.CustomMessageBox.MessageShow("请选择次数", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                    spinEditNum.Focus();
                    return false;
                }
                if (lookUpEditDoctor.CodeValue == "")
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择责任医师");
                    //m_App.CustomMessageBox.MessageShow("请选择责任医师", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                    lookUpEditDoctor.Focus();
                    return false;
                }
                if (memoEditDesc.Text.Trim() == "")
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请输入病历存在的问题");
                    //m_App.CustomMessageBox.MessageShow("请输入病历存在的问题", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                    memoEditDesc.Focus();
                    return false;
                }
                if (lookUpEPoint.Visible && lookUpEPoint.CodeValue == "")//有评分小项，但是没选择
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择要评分的小项");
                    //m_App.CustomMessageBox.MessageShow("请选择要评分的小项", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                    lookUpEPoint.Focus();
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void Save()
        {
            try
            {
                EmrPoint emrPoint = new EmrPoint();
                emrPoint.Valid = "1";
                emrPoint.DoctorID = lookUpEditDoctor.CodeValue.ToString();
                emrPoint.DoctorName = lookUpEditDoctor.Text;
                emrPoint.CreateUserID = m_App.User.Id;
                emrPoint.CreateUserName = m_App.User.Name;
                emrPoint.ProblemDesc = memoEditDesc.Text;
                emrPoint.RecordDetailID = lookUpEditEmrDoc.EditValue.ToString();
                emrPoint.ReducePoint = lookReduction.DisplayValue.ToString();
                //emrPoint.ReducePoint = lookUpEditGrade.EditValue.ToString();
                //emrPoint.Grade = lookUpEditGrade.Text;
                if (lookReduction.DisplayValue.ToString().Contains("."))//处理 是小数分数的情况
                {
                    decimal dec = Decimal.Parse(lookReduction.DisplayValue.ToString());
                    emrPoint.Grade = dec.ToString().TrimEnd('0') + "级";
                }
                else
                {
                    emrPoint.Grade = lookReduction.DisplayValue.ToString() + "级";
                }
                emrPoint.Num = spinEditNum.Text;
                emrPoint.Noofinpat = m_NoOfInpat;
                emrPoint.RecordDetailName = lookUpEditEmrDoc.Text;
                emrPoint.EMR_MARK_RECORD_ID = m_chiefID;

                //大类别编号（AC，AB）
                string id = lookUpEditEmrDoc.EditValue.ToString();//取到recorddetail主键ID 
                DataTable dtRecord = new DataTable();
                string searchsq = string.Format(@" select sortid from  recorddetail where id ='{0}' ", id);
                dtRecord = m_App.SqlHelper.ExecuteDataTable(searchsq);
                string sortid = "";
                if (dtRecord.Rows.Count > 0)
                {
                    sortid = dtRecord.Rows[0]["sortid"].ToString();
                }
                else
                {
                    //大项从dict_catalog表里取数据
                    string slq = string.Format(@" select ccode from   dict_catalog where cname='{0}'", lookUpEditEmrDoc.Text);
                    if (m_App.SqlHelper.ExecuteDataTable(slq).Rows.Count > 0)
                    {
                        sortid = m_App.SqlHelper.ExecuteDataTable(slq).Rows[0]["ccode"].ToString();
                    }
                }

                //评分配置表的主键
                if (string.IsNullOrEmpty(lookUpEPoint.CodeValue))//评分配置表未进行配置，ID取recorddetail 里的相应的ID
                {
                    //string sqlsec = string.Format(@"select ID from recorddetail where sortid='{0}' and  name ='{1}'",lookUpEditEmrDoc.EditValue.ToString(),lookUpEditEmrDoc.Text);
                    //DataTable dtid = m_App.SqlHelper.ExecuteDataTable(sqlsec);
                    //if (dtid.Rows.Count > 0)
                    //{
                    //    emrPoint.EmrPointID = Int32.Parse(dtid.Rows[0]["ID"].ToString());
                    //}
                    //else
                    //{
                    //    emrPoint.EmrPointID = 0;
                    //}
                    //edit by wyt 2012-12-11
                    //emrPoint.EmrPointID = Int32.Parse(lookUpEditEmrDoc.EditValue.ToString());
                    emrPoint.EmrPointID = lookUpEditEmrDoc.EditValue.ToString();
                }
                else
                {
                    //edit by wyt 2012-12-11
                    //emrPoint.EmrPointID = Int32.Parse(lookUpEPoint.CodeValue);
                    //emrPoint.EmrPointID = lookUpEPoint.EditValue.ToString();
                    emrPoint.EmrPointID = lookUpEPoint.CodeValue;    //edit by wangj 2013 2 20 此处应为childcode ，报表中无法关联editvalue
                }
                emrPoint.SortID = sortid;

                m_SqlManger.InsertEmrPoint(emrPoint);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 作废
        private void simpleButtonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DataRowView drv = gridView1.GetRow(gridView1.FocusedRowHandle) as DataRowView;
                if (drv != null)
                {
                    //取得创建人（只能创建人可以作废自己的评分信息）add by  ywk 2012年6月12日 14:15:12
                    string createuser = drv["CREATE_USER"].ToString();
                    if (m_App.User.Id != createuser)//当前登录者不是此项的创建人
                    {
                        m_App.CustomMessageBox.MessageShow("只有评分创建人可以进行作废!");
                        IsDelSucess = false;
                        return;
                    }
                    if (m_App.CustomMessageBox.MessageShow("确定要作废吗？", DrectSoft.Core.CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
                    {
                        try
                        {
                            DeleteRow();
                            InitGridControl();
                            if (IsDelSucess)
                            {
                                m_App.CustomMessageBox.MessageShow("作废成功!", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                            }

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            m_App.CustomMessageBox.MessageShow("保存异常，请联系管理员!", DrectSoft.Core.CustomMessageBoxKind.ErrorOk);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        private bool IsDelSucess;//判断是否作废成功
        private void DeleteRow()
        {
            try
            {
                DataRowView drv = gridView1.GetRow(gridView1.FocusedRowHandle) as DataRowView;
                if (drv != null)
                {
                    //取得创建人（只能创建人可以作废自己的评分信息）add by  ywk 2012年6月12日 14:15:12
                    //string createuser = drv["CREATE_USER"].ToString();
                    //if (m_App.User.Id!=createuser)//当前登录者不是此项的创建人
                    //{
                    //    m_App.CustomMessageBox.MessageShow("只有评分创建人可以进行作废!");
                    //    IsDelSucess = false;
                    //    return;
                    //}
                    string id = drv["ID"].ToString();
                    EmrPoint emrPoint = new EmrPoint();
                    emrPoint.ID = id;
                    emrPoint.CancelUserID = m_App.User.DoctorId;
                    m_SqlManger.CancelEmrPoint(emrPoint);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                //memoEditDescForLook
                DataRowView drv = gridView1.GetRow(gridView1.FocusedRowHandle) as DataRowView;
                if (drv != null)
                {
                    string desc = drv["PROBLEM_DESC"].ToString();
                    memoEditDescForLook.Text = desc;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        #region 计算得分情况 用100减去要扣除的分数

        private void GetPoint(string noofinpat, string recordid)
        {
            try
            {
                DataTable dt = gridControl1.DataSource as DataTable;
                if (dt != null)
                {
                    //DataRow[] drs = dt.Select("lenth(reducepoint) > 0 ");
                    double totalPoint = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        double point;
                        try
                        {
                            point = Convert.ToDouble(dr["reducepoint"].ToString());
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

                m_App.SqlHelper.ExecuteNoneQuery(sql);
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
        //bool RefreshData = false;
        #endregion
        /// <summary>
        /// 配置操作 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfig_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigPoint m_configPoint = new ConfigPoint(m_App);
                m_configPoint.StartPosition = FormStartPosition.CenterScreen;//弹出窗体在中间 
                m_configPoint.ShowDialog();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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

        private void KeyPress(object sender, KeyPressEventArgs e)
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

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.gridView1.FocusedRowHandle == -1)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void btnAutoMark_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 刷新评分记录wyt 2012-11-19
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DevButtonRefresh_Score_Click(object sender, EventArgs e)
        {
            try
            {
                InitGridControl();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void simpleButtonAutoMark_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_NoOfInpat == "")
                {
                    m_App.CustomMessageBox.MessageShow("请在左侧的病人列表中选择一个病人");
                    return;
                }
                if (m_chiefID == "")
                {
                    m_App.CustomMessageBox.MessageShow("请先选择或新增一条评分记录");
                    return;
                }
                if (DialogResult.OK == m_App.CustomMessageBox.MessageShow("确定要开始自动评分?", DrectSoft.Core.CustomMessageBoxKind.QuestionOkCancel))
                {
                    //在主表新增一条自动评分记录


                    string id = InsertNewAutoRecord("1");//自动评分主表ID
                    if (id == "")
                    {
                        m_App.CustomMessageBox.MessageShow("评分出错");
                        return;
                    }
                    AutoMarkRecoed autoMark = new AutoMarkRecoed(m_App);
                    autoMark.Automark(id, m_NoOfInpat, m_PatinetName, m_type);
                    //SetRecordDone(id);
                    SetPatRecordDetail(id);
                    //BindPatRecordDetail(id);
                    InitGridControl();
                    m_App.CustomMessageBox.MessageShow("成功");
                }
            }
            catch (Exception)
            {
                m_App.CustomMessageBox.MessageShow("评分出错");
            }
        }

        /// <summary>
        /// 增加一条自动评分主表记录
        /// </summary>
        /// <returns>自动评分主表ID</returns>
        private string InsertNewAutoRecord(string isAuto)
        {
            try
            {
                string id = m_SqlManger.InsertAutoMarkRecord(m_NoOfInpat, isAuto, m_auth, m_type);//返回主表记录ID
                if (id == "")
                {
                    return id;
                }
                return id;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
                return "";
            }
        }


        /// <summary>
        /// 处理加载自动评分详情
        /// wyt 2012-12-05
        /// </summary>
        /// <param name="ID">自动评分主记录ID</param>
        private void SetPatRecordDetail(string id)
        {
            try
            {
                if (id == "")
                {
                    return;
                }
                m_SqlManger.ClearEmrPoint(id);  //先清空原来的自动评分数据
                DataTable dt = m_SqlManger.GetPatRecordDetail(id);
                //gridControl1.BeginUpdate();
                //gridControl1.DataSource = dt;
                //gridControl1.EndUpdate();

                DataTable autoMarkRecord = m_SqlManger.GetAutoMarkRecord(id);
                if (autoMarkRecord.Rows.Count == 0)
                {
                    m_App.CustomMessageBox.MessageShow("没有扣分记录内容");
                    return;
                }
                DataRow row = autoMarkRecord.Rows[0];
                foreach (DataRow dr in dt.Rows)
                {
                    EmrPoint emrPoint = new EmrPoint();
                    emrPoint.Valid = "1";       //是否有效
                    emrPoint.DoctorID = dr["ERRORDOCTOR"].ToString();      //责任医师ID
                    emrPoint.DoctorName = dr["ERRORDOCTORNAME"].ToString();    //责任医师，
                    emrPoint.CreateUserID = m_App.User.Id;  //登记人ID，取当前用户ID
                    emrPoint.CreateUserName = m_App.User.Name;  //登记人姓名，取当前用户ID
                    emrPoint.ProblemDesc = dr["CONFIGREDUCTIONNAME"].ToString();   //扣分理由
                    emrPoint.RecordDetailID = "";   //病案ID,如果是病案首页则显示IEM_MAINPAGE_NO，目前评分不定位到具体病案文档
                    emrPoint.ReducePoint = dr["POINT"].ToString();          //扣分
                    emrPoint.Grade = dr["POINT"].ToString() + "级";          //等级
                    emrPoint.Num = "1";                                 //次数，暂取1
                    emrPoint.Noofinpat = dr["NOOFINPAT"].ToString();    //病人首页序号
                    emrPoint.RecordDetailName = dr["cname"].ToString(); //病案名称，自动评分主表和病案记录表病案名称
                    emrPoint.EMR_MARK_RECORD_ID = m_chiefID;            //综合评分主表ID
                    //emrPoint.EMR_MARK_RECORD_ID = dr["automarkrecordid"].ToString(); //主表ID
                    //评分配置表的主键

                    //edit by wyt 2012-12-11
                    //emrPoint.EmrPointID = Convert.ToInt32(dr["CHILDREN"]);   //评分小项编号，如主诉（15），暂时取不到
                    emrPoint.EmrPointID = dr["CHILDREN"].ToString();   //评分小项编号，如主诉（15），暂时取不到
                    emrPoint.SortID = dr["PARENTS"].ToString(); //评分大项ID，如住院志（AB），取病案记录表的SORTID字段
                    m_SqlManger.InsertEmrPoint(emrPoint);
                }
            }
            catch (Exception)
            {
                throw;
            }

        }


        ///// <summary>
        ///// 检查一些必填元素是否已经填写
        ///// add by ywk 2012年9月18日 13:57:39
        ///// </summary>
        //private void CheckDocBYConfigItem()
        //{
        //    //传进一段XML，先搜寻要验证的XML节点名称，然后取出紧跟其后的元素，比较两个元素之间的内容是否符合要求
        //    string mrecord_content = string.Empty;
        //    int findedcount = 0;//定义变量记录查找到指定标签的存在链表中的索引值
        //    string nexttip = string.Empty;//记录要查找的标签的下一个标签
        //    bool IsChecked = false;//是否验证通过
        //    m_CurrentModel.ModelContent = CurrentForm.zyEditorControl1.BinaryToXml(CurrentForm.zyEditorControl1.SaveBinary());
        //    if (m_CurrentModel == null)
        //    {
        //        m_app.CustomMessageBox.MessageShow("病历内容为空！");
        //        return;
        //    }
        //    else
        //    {
        //        XmlDocument xmlRecord = new XmlDocument();
        //        xmlRecord.LoadXml(m_CurrentModel.ModelContent.InnerXml);
        //        XmlNode body = xmlRecord.SelectSingleNode("//body");
        //        //查找整个Body部分的标签元素有多少个
        //        int tipcount = body.SelectNodes("//roelement").Count;
        //        ArrayList arr_element = new ArrayList();
        //        for (int i = 0; i < tipcount; i++)
        //        {
        //            arr_element.Add(body.SelectNodes("//roelement")[i].InnerText);//将此病历内容中的标签加入到链表
        //        }

        //        //先取出系统参数配置中配置的那些要进行检查的项目要进行验证的限制长度 
        //        string config = GetConfigValueByKey("CheckDocByConfig");
        //        XmlDocument doc = new XmlDocument();
        //        doc.LoadXml(config);
        //        int confignum = doc.GetElementsByTagName("checkitem").Count;//取得系统参数配置中的要验证的项有几个

        //        Dictionary<string, string> m_checkitem = new Dictionary<string, string>();
        //        for (int i = 0; i < confignum; i++)
        //        {
        //            m_checkitem.Add(doc.GetElementsByTagName("checkitem")[i].InnerText.Split(',')[0], doc.GetElementsByTagName("checkitem")[i].InnerText.Split(',')[1]);
        //        }
        //        string allstr = body.InnerText;

        //        List<string> tkeys = new List<string>(m_checkitem.Keys);
        //        for (int j = 0; j < tipcount; j++)
        //        {
        //            for (int k = 0; k < m_checkitem.Count; k++)
        //            {
        //                if (body.SelectNodes("//roelement")[j].InnerText.ToString().Trim().Replace("　", "").Replace(" ", "").StartsWith(tkeys[k].ToString()))
        //                {
        //                    findedcount = j;
        //                    nexttip = body.SelectNodes("//roelement")[j + 1].InnerText.ToString();//下一个元素名称
        //                    string startstr = tkeys[k].ToString();
        //                    int startop = allstr.Replace("　", "").Replace(" ", "").IndexOf(startstr, 0) + startstr.Length; //开始位置
        //                    int endop = allstr.Replace("　", "").Replace(" ", "").IndexOf(nexttip, startop); //结束位置
        //                    string my = allstr.Replace("　", "").Replace(" ", "").Substring(startop, endop - startop).Replace("：", ""); //取搜索的条数，用结束的位置-开始的位置,并返回 
        //                    int checklength = Int32.Parse(m_checkitem[tkeys[k]]);
        //                    if (my.Length < checklength)
        //                    {
        //                        m_app.CustomMessageBox.MessageShow(startstr + "内容应不得少于" + checklength.ToString() + "字！", CustomMessageBoxKind.InformationOk);
        //                        IsChecked = false;
        //                        return;
        //                    }
        //                    else
        //                    {
        //                        IsChecked = true;
        //                        //break;
        //                        //return;
        //                    }
        //                    //m_app.CustomMessageBox.MessageShow(startstr+"后面的内容:"+my);
        //                }
        //            }
        //        }
        //        if (IsChecked)
        //        {
        //            m_app.CustomMessageBox.MessageShow("所需验证元素均符合要求！");
        //        }
        //        //string allstr = body.InnerText;
        //        //string startstr = "主　诉：";
        //        //int startop = allstr.IndexOf("主　诉：", 0) + startstr.Length; //开始位置
        //        //int endop = allstr.IndexOf("现病史", startop); //结束位置
        //        //string my=allstr.Substring(startop, endop - startop); //取搜索的条数，用结束的位置-开始的位置,并返回 
        //    }
        //}
    }

    public class EmrPoint
    {
        /// <summary>
        /// 新增字段对应emr_configpoint的主键用于标识评分的哪个小项
        /// add by ywk 2012年4月1日10:21:07
        /// </summary>
        public string EmrPointID { get; set; }
        /// <summary>
        /// 新增字段对应emr_configpoint的主键用于标识评分的哪个小项
        /// add by ywk 2012年4月1日10:21:07
        /// </summary>
        public string EmrPointChildID { get; set; }
        /// <summary>
        /// 所属综合评分主表ID
        /// </summary>
        public string EMR_MARK_RECORD_ID { get; set; }

        /// <summary>
        /// 新增大类IDadd by ywk 2012年4月1日10:39:35
        /// </summary>
        public string SortID { get; set; }
        /// <summary>
        /// 唯一序号
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 有效性 1：有效 2：无效
        /// </summary>
        public string Valid { get; set; }
        /// <summary>
        /// 作废人工号
        /// </summary>
        public string CancelUserID { get; set; }
        /// <summary>
        /// 作废人姓名
        /// </summary>
        public string CancelUserName { get; set; }
        /// <summary>
        /// 作废时间
        /// </summary>
        public string CancelTime { get; set; }
        /// <summary>
        /// 责任人工号
        /// </summary>
        public string DoctorID { get; set; }
        /// <summary>
        /// 责任人姓名
        /// </summary>
        public string DoctorName { get; set; }
        /// <summary>
        /// 创建人工号
        /// </summary>
        public string CreateUserID { get; set; }
        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 问题描述
        /// </summary>
        public string ProblemDesc { get; set; }
        /// <summary>
        /// 扣分
        /// </summary>
        public string ReducePoint { get; set; }
        /// <summary>
        /// 次数
        /// </summary>
        public string Num { get; set; }
        /// <summary>
        /// 扣分等级
        /// </summary>
        public string Grade { get; set; }
        /// <summary>
        /// 病历ID
        /// </summary>
        public string RecordDetailID { get; set; }

        public string Noofinpat { get; set; }

        public string RecordDetailName { get; set; }
    }
}
