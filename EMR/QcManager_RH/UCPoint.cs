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
        string m_NoOfInpat;
        EmrModel m_EmrModel;
        EmrModelContainer m_EmrModelContainer;
        DataTable dtRHQCReport;

        //审核通过触发的事件
        public EventHandler EventHandlerTongGuo;

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
            //this.labelControl8.Visible = false;
            //this.lookUpEPoint.Visible = false;
            if (this.DesignMode) return;
            // SumPoint = Int32.Parse(m_SqlManger.GetConfigValueByKey("EmrPointConfig"));

            //lookReduction.Enter += new EventHandler(lookReduction_Enter);
            //lookReduction.GotFocus += new EventHandler(lookReduction_Enter);
            lookReduction.MouseDown += new MouseEventHandler(lookReduction_MouseDown);
            lookUpEditEmrDoc.EditValueChanged += new EventHandler(lookUpEditEmrDoc_EditValueChanged);
            //lookUpEPoint.EditValue = "0";
            InitLookUpEditorEmrDoc();
            InitGrade();//加载扣分项
            InitLookUpEditorDoctor();
            InitTotalGrade();
            InitGridControl();
            InitPatientInfo();
            InitButtonVisible();

        }

        /// <summary>
        /// 获取当前病人号的评分表
        /// </summary>
        /// <returns></returns>
        private DataTable GetQCTable()
        {
            string Uidentity = m_SqlManger.JudgeIdentity(m_App.User.Id, m_SqlManger);//判断当前登录的人是科室质控员还是质控科的
            if (Uidentity == "QCMANAGER" || Uidentity == "CHIEF")
            {
                string sqlTable = string.Format(
    @"select * from emr_rhqc_table where noofinpat='{0}' and stateid in('8700','8701','8702','8703','8704')", m_NoOfInpat);
                DataTable dtQCTable = m_App.SqlHelper.ExecuteDataTable(sqlTable);
                if (dtQCTable != null && dtQCTable.Rows.Count > 0)
                {
                    return dtQCTable;
                }
                else
                {
                    string guid = Guid.NewGuid().ToString();
                    string statuId = "8700";
                    string sqlInsert = String.Format(@"insert into emr_rhqc_table values('{0}','{1}','{2}','{3}','{4}','1','{5}','0')", guid, m_NoOfInpat, m_App.User.Id, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), m_App.User.Name, statuId);
                    m_App.SqlHelper.ExecuteNoneQuery(sqlInsert);
                    dtQCTable = m_App.SqlHelper.ExecuteDataTable(sqlTable);
                    return dtQCTable;
                }
            }
            else if (Uidentity == "QCDepart")
            {

                string sqlTable = string.Format(
    @"select * from emr_rhqc_table where noofinpat='{0}' and stateid='8705'", m_NoOfInpat);
                DataTable dtQCTable = m_App.SqlHelper.ExecuteDataTable(sqlTable);
                if (dtQCTable != null && dtQCTable.Rows.Count > 0)
                {
                    return dtQCTable;
                }
                else
                {
                    string guid = Guid.NewGuid().ToString();
                    string statuId = "8705";
                    string sqlInsert = String.Format(@"insert into emr_rhqc_table values('{0}','{1}','{2}','{3}','{4}','1','{5}','0')", guid, m_NoOfInpat, m_App.User.Id, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), m_App.User.Name, statuId);
                    m_App.SqlHelper.ExecuteNoneQuery(sqlInsert);
                    dtQCTable = m_App.SqlHelper.ExecuteDataTable(sqlTable);
                    return dtQCTable;
                }
            }
            else
            {
                return null;
            }
        }

        void lookReduction_MouseDown(object sender, MouseEventArgs e)
        {
            InitGrade();//加载扣分项
        }

        void lookReduction_Enter(object sender, EventArgs e)
        {
            InitGrade();//加载扣分项
        }

        /// <summary>
        /// 只有管理员才能看到配置按钮 对按钮的控制 xll 2012-08-02
        /// </summary>
        private void InitButtonVisible()
        {
            string Uidentity = m_SqlManger.JudgeIdentity(m_App.User.Id, m_SqlManger);//判断当前登录的人是科室质控员还是质控科的

            btnConfig.Visible = true;
            btnTiJiao.Visible = true;
            panelControl1.Visible = true;
            btnTongGuo.Visible = false;
            btnBuTongGuo.Visible = false;
            if (Uidentity == "QCDepart")
            {
                btnConfig.Visible = true;
                btnTiJiao.Visible = false;
                btnTongGuo.Visible = true;
            }
            else if (Uidentity == "QCMANAGER")
            {
                btnConfig.Visible = false;
                btnTiJiao.Visible = true;
            }
            else if (Uidentity == "CHIEF")
            {
                btnBuTongGuo.Visible = true;
                btnTongGuo.Visible = true;
                panelControl1.Visible = false;
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
            string sortid = "";
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
                }
            }
            else
            {
                this.labelControl8.Visible = false;
                this.lookUpEPoint.Visible = false;
                this.lookUpEPoint.CodeValue = "";

            }
            InitLookUpEditorDoctor();
        }
        /// <summary>
        /// 根据选择问题病历的分类，得到下面的小项，绑定到评分项下拉框里
        /// edit by ywk 
        /// </summary>
        /// <param name="sortid"></param>
        private void InitEmrPoint(string sortid)
        {
            lookUpEPoint.CodeValue = "";//选择完成一个大分类，要将原来的值清空 edit by ywk2012年5月30日 18:03:12
            lookUpWindowPoint.SqlHelper = m_App.SqlHelper;

            string sql = string.Format(@" select childcode,childname,id from emr_configpoint where ccode='{0}'  and valid='1' ", sortid);
            DataTable dtchilddata = new DataTable();
            // dtchilddata = m_SqlManger.GetPointClass(sortid);
            dtchilddata = m_App.SqlHelper.ExecuteDataTable(sql);
            if (dtchilddata.Rows.Count > 0)
            {
                dtchilddata.Columns["ID"].Caption = "编号";
                dtchilddata.Columns["CHILDNAME"].Caption = "评分项";
                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("CHILDNAME", 160);
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
            }
        }

        #endregion

        #region 初始化列表
        private void InitGridControl()
        {
            if (dtRHQCReport == null) return;
            memoEditDescForLook.Text = "";
            string rhqcTableID = dtRHQCReport.Rows[0]["id"].ToString();
            DataTable dt = m_SqlManger.GetRHAllEmrPointByNoofinpat(rhqcTableID);
            gridControl1.DataSource = dt;
            GetPoint();
        }
        #endregion

        #region 初始化病历列表
        public void RefreshLookUpEditorEmrDoc(string noofinpat, EmrModel emrModel, EmrModelContainer emrModelContainer)
        {

            m_NoOfInpat = noofinpat;
            SumPoint = m_SqlManger.GetSumPoint(m_NoOfInpat, m_App);
            m_EmrModel = emrModel;
            m_EmrModelContainer = emrModelContainer;
            dtRHQCReport = GetQCTable();
            InitLookUpEditorEmrDoc();
            InitLookUpEditorDoctor();
            InitTotalGrade();
            InitGridControl();
            InitPatientInfo();
        }



        /// <summary>
        /// 初始化病历列表
        /// </summary>
        public void InitLookUpEditorEmrDoc()
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

        private DataTable GetDataTable(DataTable dt)
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
        #endregion

        #region 初始化责任人列表
        /// <summary>
        /// 初始化责任人列表
        /// </summary>
        public void InitLookUpEditorDoctor()
        {
            #region  原来的取医师
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
                cols.Add("ID", 50);
                cols.Add("NAME", 160);
                SqlWordbook deptWordBook = new SqlWordbook("querybook", dtAllDoc, "ID", "NAME", cols, "ID//NAME//PY//WB");
                lookUpEditDoctor.SqlWordbook = deptWordBook;
            }


            DataTable DtInpatDoc = new DataTable();
            string sql = string.Format(@" select owner from recorddetail where noofinpat='{0}'", m_NoOfInpat);
            DtInpatDoc = m_App.SqlHelper.ExecuteDataTable(sql);
            if (DtInpatDoc.Rows.Count > 0)
            {
                lookUpEditDoctor.CodeValue = DtInpatDoc.Rows[0]["OWNER"].ToString(); ;
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
        #endregion

        #region 扣分&&等级
        private void InitGrade()
        {
            lookUpWReduction.SqlHelper = m_App.SqlHelper;
            string strUserState = m_SqlManger.JudgeIdentity(m_App.User.Id, m_SqlManger);
            string sql1 = string.Format(@"   select a.reducepoint,a.problem_desc,a.id  from  
            EMR_RHConfigReduction a where a.valid='1'");
            if (strUserState == "QCDepart")
            {
                sql1 += " AND a.User_Type='质控科'";
            }
            else
            { sql1 += " AND a.User_Type='其他'"; }
            DataTable dtReduction = new DataTable();
            dtReduction = m_App.SqlHelper.ExecuteDataTable(sql1);
            //if (dtReduction.Rows.Count > 0)///无论有没有数据，它的SqlWorkbook都不能为空edit by ywk 2012年5月30日 17:42:42
            //{
            dtReduction.Columns["ID"].Caption = "序号";
            dtReduction.Columns["REDUCEPOINT"].Caption = "扣分标准";
            dtReduction.Columns["PROBLEM_DESC"].Caption = "扣分理由";
            Dictionary<string, int> cols = new Dictionary<string, int>();
            //cols.Add("ID", 10);
            cols.Add("REDUCEPOINT", 80);
            cols.Add("PROBLEM_DESC", 160);
            SqlWordbook ReductWordBook = new SqlWordbook("querybook", dtReduction, "ID", "REDUCEPOINT", cols, "PROBLEM_DESC//REDUCEPOINT//ID");//REDUCEPOINT//PROBLEM_DESC
            lookReduction.SqlWordbook = ReductWordBook;
            //}
        }
        /// <summary>
        /// 选择评分，带出扣分的理由
        /// add by ywk  2012年5月28日 10:36:46
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lookReduction_CodeValueChanged(object sender, EventArgs e)
        {
            if (lookReduction.SqlWordbook == null)
            {
                InitGrade();
            }
            string point = lookReduction.CodeValue.ToString();
            string search = string.Format(@"select problem_desc from EMR_ConfigReduction where id='{0}'", point);
            DataTable dt = m_App.SqlHelper.ExecuteDataTable(search, CommandType.Text);
            if (dt.Rows.Count > 0)
            {
                memoEditDesc.Text = dt.Rows[0]["problem_desc"].ToString();
            }
            else
            {
                memoEditDesc.Text = "";
            }
        }

        private DataTable Grade()
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
        #endregion

        #region 综合等级

        private void InitTotalGrade()
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

        private DataTable TotalGrade()
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

        #endregion

        #region 增加
        private void simpleButtonAdd_Click(object sender, EventArgs e)
        {
            if (dtRHQCReport.Rows[0]["stateid"].ToString() == "8701")
            {
                m_App.CustomMessageBox.MessageShow("评分表以提交，不得添加!");
                return;
            }
            if (dtRHQCReport.Rows[0]["stateid"].ToString() == "8702")
            {
                m_App.CustomMessageBox.MessageShow("评分表以审核，不得添加!");
                return;
            }
            if (ValidateBeforeSave())
            {
                try
                {
                    Save();
                    InitGridControl();
                    m_App.CustomMessageBox.MessageShow("保存成功!", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    m_App.CustomMessageBox.MessageShow("保存异常，请联系管理员!", DrectSoft.Core.CustomMessageBoxKind.ErrorOk);
                }
            }
        }

        private bool ValidateBeforeSave()
        {
            if (string.IsNullOrEmpty(m_NoOfInpat))
            {
                m_App.CustomMessageBox.MessageShow("请选择一个病人!", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                return false;
            }
            if (lookUpEditEmrDoc.Text == "")
            {
                m_App.CustomMessageBox.MessageShow("请输入对应的病历!", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                return false;
            }
            //if (lookUpEditGrade.Text == "")
            //{
            //    m_App.CustomMessageBox.MessageShow("请选择扣分等级!", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
            //    lookUpEditGrade.Focus();
            //    return false;
            //}
            if (lookReduction.Text == "")
            {
                m_App.CustomMessageBox.MessageShow("请选择扣分标准!", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                lookReduction.Focus();
                return false;
            }
            if (spinEditNum.Text == "")
            {
                m_App.CustomMessageBox.MessageShow("请选择次数!", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                spinEditNum.Focus();
                return false;
            }
            if (lookUpEditDoctor.CodeValue == "")
            {
                m_App.CustomMessageBox.MessageShow("请选择责任医师!", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                lookUpEditDoctor.Focus();
                return false;
            }
            if (memoEditDesc.Text.Trim() == "")
            {
                m_App.CustomMessageBox.MessageShow("请输入病历存在的问题!", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                memoEditDesc.Focus();
                return false;
            }
            if (lookUpEPoint.Visible && lookUpEPoint.CodeValue == "")//有评分小项，但是没选择
            {
                m_App.CustomMessageBox.MessageShow("请选择要评分的小项！!", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                lookUpEPoint.Focus();
                return false;
            }
            return true;
        }
        /// <summary>
        /// 将评分信息插入到评分表里
        /// 此处需判断此登录人的身份，如为质控科人员则操作EmrRHQcPoint表（质控科可对在院和出院的进行评分操作）对应了RecordType字段
        /// 如为科室质控员则操作Emr_RHPoint表（控制病历评分的状态StateID）
        /// </summary>
        private void Save()
        {
            string stRHQCTableid = dtRHQCReport.Rows[0]["id"].ToString();
            RHEmrPoint emrPoint = new RHEmrPoint();
            emrPoint.Valid = "1";
            emrPoint.DoctorID = lookUpEditDoctor.CodeValue.ToString();
            emrPoint.DoctorName = lookUpEditDoctor.Text;
            emrPoint.CreateUserID = m_App.User.Id;
            emrPoint.CreateUserName = m_App.User.Name;
            emrPoint.ProblemDesc = memoEditDesc.Text;
            emrPoint.RecordDetailID = lookUpEditEmrDoc.EditValue.ToString();
            emrPoint.ReducePoint = lookReduction.DisplayValue.ToString();
            emrPoint.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
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
                emrPoint.EmrPointID = Int32.Parse(lookUpEditEmrDoc.EditValue.ToString());
            }
            else
            {
                emrPoint.EmrPointID = Int32.Parse(lookUpEPoint.CodeValue);
            }
            emrPoint.SortID = sortid;

            m_SqlManger.InsertRHEmrPoint(emrPoint, stRHQCTableid);
        }
        #endregion

        #region 删除(作废)
        private void simpleButtonDelete_Click(object sender, EventArgs e)
        {
            DataRowView drv = gridView1.GetRow(gridView1.FocusedRowHandle) as DataRowView;
            if (drv != null)
            {
                //取得创建人（只能创建人可以删除自己的评分信息）add by  ywk 2012年6月12日 14:15:12
                string createuser = drv["CREATE_USER"].ToString();
                if (m_App.User.Id != createuser)//当前登录者不是此项的创建人
                {
                    m_App.CustomMessageBox.MessageShow("只有评分创建人可以进行删除!");
                    IsDelSucess = false;
                    return;
                }
                if (dtRHQCReport.Rows[0]["stateid"].ToString() == "8701")
                {
                    m_App.CustomMessageBox.MessageShow("评分表已提交，不得修改!");
                    IsDelSucess = false;
                    return;
                }
                if (dtRHQCReport.Rows[0]["stateid"].ToString() == "8702")
                {
                    m_App.CustomMessageBox.MessageShow("评分表已审核，不得修改!");
                    IsDelSucess = false;
                    return;
                }
                if (m_App.CustomMessageBox.MessageShow("确定要删除吗？", DrectSoft.Core.CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
                {
                    try
                    {
                        DeleteRow();
                        InitGridControl();
                        if (IsDelSucess)
                        {
                            m_App.CustomMessageBox.MessageShow("删除成功!", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
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
        private bool IsDelSucess;//判断是否删除成功
        private void DeleteRow()
        {
            DataRowView drv = gridView1.GetRow(gridView1.FocusedRowHandle) as DataRowView;
            if (drv != null)
            {
                //取得创建人（只能创建人可以删除自己的评分信息）add by  ywk 2012年6月12日 14:15:12
                //string createuser = drv["CREATE_USER"].ToString();
                //if (m_App.User.Id!=createuser)//当前登录者不是此项的创建人
                //{
                //    m_App.CustomMessageBox.MessageShow("只有评分创建人可以进行删除!");
                //    IsDelSucess = false;
                //    return;
                //}
                string id = drv["ID"].ToString();
                RHEmrPoint emrPoint = new RHEmrPoint();
                emrPoint.ID = id;
                emrPoint.CancelUserID = m_App.User.DoctorId;
                emrPoint.CancelUserName = m_App.User.DoctorName;
                m_SqlManger.CancelRHEmrPoint(emrPoint);
            }
        }
        #endregion

        #region 提交评分表
        private void btnTiJiao_Click(object sender, EventArgs e)
        {
            if (dtRHQCReport.Rows[0]["stateid"].ToString() == "8701"
                || dtRHQCReport.Rows[0]["stateid"].ToString() == "8702")
            {
                m_App.CustomMessageBox.MessageShow("评分表提交过，无需再提交");
                return;
            }

            if (m_App.CustomMessageBox.MessageShow("提交后不得修改，确定要提交吗？", DrectSoft.Core.CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
            {
                string strState = "8701";
                if (dtRHQCReport == null || dtRHQCReport.Rows.Count == 0) return;
                string strID = dtRHQCReport.Rows[0]["id"].ToString();
                string strUpdate = string.Format(@"update emr_rhqc_table set stateid='{0}' where id='{1}'", strState, strID);
                m_App.SqlHelper.ExecuteNoneQuery(strUpdate);
                dtRHQCReport = GetQCTable();
                m_App.CustomMessageBox.MessageShow("提交成功！");
            }
        }
        #endregion

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            //memoEditDescForLook
            DataRowView drv = gridView1.GetRow(gridView1.FocusedRowHandle) as DataRowView;
            if (drv != null)
            {
                string desc = drv["PROBLEM_DESC"].ToString();
                memoEditDescForLook.Text = desc;
            }
        }

        #region 计算得分情况 用100减去要扣除的分数

        private void GetPoint()
        {
            DataTable dt = gridControl1.DataSource as DataTable;
            if (dt != null)
            {
                double totalPoint = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    double point = Convert.ToDouble(dr["reducepoint"]);
                    totalPoint += point;
                }
                //现在改为85分为满分
                textEditTotalPoint.Text = Convert.ToString(SumPoint - totalPoint);
                //textEditTotalPoint.Text = Convert.ToString(100 - totalPoint);
                GetGrade();
            }
        }

        private void GetGrade()
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
        //bool RefreshData = false;
        #endregion
        /// <summary>
        /// 配置操作 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfig_Click(object sender, EventArgs e)
        {
            ConfigPoint m_configPoint = new ConfigPoint(m_App);
            m_configPoint.StartPosition = FormStartPosition.CenterScreen;//弹出窗体在中间 
            m_configPoint.ShowDialog();
        }

        private void btnTongGuo_Click(object sender, EventArgs e)
        {
            string userState = m_SqlManger.JudgeIdentity(m_App.User.Id, m_SqlManger);
            if (userState == "QCDepart")
            {
                SheHeRHQC("8704");
            }
            else if (userState == "CHIEF")
            {
                SheHeRHQC("8702");
            }
            if (EventHandlerTongGuo != null)
                EventHandlerTongGuo(null, null);

        }

        private void btnBuTongGuo_Click(object sender, EventArgs e)
        {
            SheHeRHQC("8703");
        }

        /// <summary>
        /// 审核的处理
        /// </summary>
        /// <param name="stateId"></param>
        private void SheHeRHQC(string stateId)
        {
            try
            {
                string rhqcTableId = dtRHQCReport.Rows[0]["id"].ToString();
                string strInsert = string.Format(@"update emr_rhqc_table set stateid='{0}',doctorstate='0' where id='{1}'", stateId, rhqcTableId);
                m_App.SqlHelper.ExecuteNoneQuery(strInsert);
                m_App.CustomMessageBox.MessageShow("审核成功");
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
    }
    /// <summary>
    /// 此为仁和版本
    /// 实体对应的表是EMR_RHPOINT(科室质控人员用)
    /// </summary>
    public class RHEmrPoint
    {
        /// <summary>
        /// 新增字段对应emr_configpoint的主键用于标识评分的哪个小项
        /// add by ywk 2012年4月1日10:21:07
        /// </summary>
        public int EmrPointID { get; set; }

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
        /// <summary>
        /// 病人的首页序号
        /// </summary>
        public string Noofinpat { get; set; }
        /// <summary>
        /// 病历名称
        /// </summary>
        public string RecordDetailName { get; set; }
        /// <summary>
        /// 病历评分表的状态(对应categorydetail表的id为87的值)
        /// </summary>
        public string stateid { get; set; }

    }
    /// <summary>
    /// 实体对应的表是EmrRHQcPoint（质控科人员用）
    /// </summary>
    public class EmrRHQcPoint
    {
        /// <summary>
        /// 唯一序号
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 病历ID
        /// </summary>
        public string RecordDetailID { get; set; }
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
        /// 新增字段对应emr_RHConfigQcManager的主键用于标识评分（质控科的专门也有个评分问题配置）
        /// add by ywk 2012年4月1日10:21:07
        /// </summary>
        public int EmrPointID { get; set; }
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
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 病人的首页序号
        /// </summary>
        public string Noofinpat { get; set; }
        /// <summary>
        /// 病历名称
        /// </summary>
        public string RecordDetailName { get; set; }
        /// <summary>
        /// 病历当前的状态，质控科的可对在院和出院的评分 
        /// </summary>
        public string RecordType { get; set; }
        /// <summary>
        /// 次数
        /// </summary>
        public string Num { get; set; }
        /// <summary>
        /// 扣分等级
        /// </summary>
        public string Grade { get; set; }

    }
}
