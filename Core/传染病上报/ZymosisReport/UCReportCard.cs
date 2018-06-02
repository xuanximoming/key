using DevExpress.XtraEditors;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Library;
using DrectSoft.Core.ZymosisReport.UCControls;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Service;
using DrectSoft.Wordbook;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DrectSoft.Core.ZymosisReport
{
    public partial class UCReportCard : DevExpress.XtraEditors.XtraUserControl
    {
        IEmrHost m_Host;
        public ZymosisReportEntity m_ZymosisReportEntity;
        public AttachedCard_HepatitisB card_HepatitisB;
        public string m_Noofinpat;
        public string ReportID;
        private SqlHelper m_SqlHelper;
        DataTable Bzlb; //传染病列表 xll 20130816
        public string fukatype; //副卡的ID
        SqlHelper SqlHelper
        {
            get
            {
                if (m_SqlHelper == null)
                    m_SqlHelper = new SqlHelper(m_Host.SqlHelper);
                return m_SqlHelper;
            }
            set { m_SqlHelper = value; }
        }


        public UCReportCard()
        {
            InitializeComponent();
            ClearPage();
        }

        public UCReportCard(IEmrHost app)
        {
            m_Host = app;
            InitializeComponent();

            InitDepartment();
            InitBzlb();
            InitDoctor("");
        }

        /// <summary>
        /// 加载方法，传入初始化值
        /// </summary>
        /// <param name="ID">传染病报告卡ID或者新增传染病报告卡时候传入病人首页序号</param>
        /// <param name="type">1、传入传染病报告卡序号  2、传入病人首页序号</param>
        /// <param name="userRole">1、申请人  2、审核人</param>
        public void LoadPage(string id, string type, string userRole)
        {
            if (id == null)
                return;
            SqlHelper = new SqlHelper(m_Host.SqlHelper);
            if (type == "1")
            {
                m_ZymosisReportEntity = SqlHelper.GetZymosisReportEntity(id);
            }
            else if (type == "2")
            {
                m_ZymosisReportEntity = SqlHelper.GetInpatientByNoofinpat(id);

                m_ZymosisReportEntity.Reportdoccode = m_Host.User.Id;

            }
            ClearPage();
            FillUI(m_ZymosisReportEntity);
            ReadOnlyControl(userRole);
        }

        #region 操作实体

        /// <summary>
        /// 将实体中值绑定到页面
        /// </summary>
        private void FillUI(ZymosisReportEntity _ZymosisReportEntity)
        {
            if (_ZymosisReportEntity == null)
                return;
            else
            {
                #region 将报告卡实体中值填入到页面

                txtpatid.Text = _ZymosisReportEntity.Patid;

                m_Noofinpat = _ZymosisReportEntity.Noofinpat;
                ReportID = _ZymosisReportEntity.ReportId.ToString();
                //txtReportNo.Text = _ZymosisReportEntity.ReportNo;//？？原来注释掉的 ？？

                txtReportNo.Text = _ZymosisReportEntity.ReportNo;//报告卡卡号

                if (_ZymosisReportEntity.ReportType == "1")
                    chkReportType1.Checked = true;
                else if (_ZymosisReportEntity.ReportType == "2")
                    chkReportType2.Checked = true;
                txtName.Text = _ZymosisReportEntity.Name;
                txtParentname.Text = _ZymosisReportEntity.Parentname;
                txtIdno.Text = _ZymosisReportEntity.Idno;

                if (_ZymosisReportEntity.Sex == "1")
                    chkSex1.Checked = true;
                else if (_ZymosisReportEntity.Sex == "2")
                    chkSex2.Checked = true;

                if (!String.IsNullOrEmpty(_ZymosisReportEntity.Birth))
                {
                    dateBirth.DateTime = Convert.ToDateTime(_ZymosisReportEntity.Birth);
                }
                txtAge.Text = _ZymosisReportEntity.Age;
                if (_ZymosisReportEntity.AgeUnit == "1")
                    chkAgeUnit1.Checked = true;
                else if (_ZymosisReportEntity.AgeUnit == "2")
                    chkAgeUnit2.Checked = true;
                else if (_ZymosisReportEntity.AgeUnit == "3")
                    chkAgeUnit3.Checked = true;

                txtOrganization.Text = _ZymosisReportEntity.Organization;
                txtOfficetel.Text = _ZymosisReportEntity.Officetel;

                if (_ZymosisReportEntity.Addresstype == "1")
                    chkAddresstype1.Checked = true;
                else if (_ZymosisReportEntity.Addresstype == "2")
                    chkAddresstype2.Checked = true;
                else if (_ZymosisReportEntity.Addresstype == "3")
                    chkAddresstype3.Checked = true;
                else if (_ZymosisReportEntity.Addresstype == "4")
                    chkAddresstype4.Checked = true;
                else if (_ZymosisReportEntity.Addresstype == "5")
                    chkAddresstype5.Checked = true;
                else if (_ZymosisReportEntity.Addresstype == "6")
                    chkAddresstype6.Checked = true;

                txtAddress.Text = _ZymosisReportEntity.Address;

                if (_ZymosisReportEntity.Jobid == "1")
                    chkJobid1.Checked = true;
                else if (_ZymosisReportEntity.Jobid == "2")
                    chkJobid2.Checked = true;
                else if (_ZymosisReportEntity.Jobid == "3")
                    chkJobid3.Checked = true;
                else if (_ZymosisReportEntity.Jobid == "4")
                    chkJobid4.Checked = true;
                else if (_ZymosisReportEntity.Jobid == "5")
                    chkJobid5.Checked = true;
                else if (_ZymosisReportEntity.Jobid == "6")
                    chkJobid6.Checked = true;
                else if (_ZymosisReportEntity.Jobid == "7")
                    chkJobid7.Checked = true;
                else if (_ZymosisReportEntity.Jobid == "8")
                    chkJobid8.Checked = true;
                else if (_ZymosisReportEntity.Jobid == "9")
                    chkJobid9.Checked = true;
                else if (_ZymosisReportEntity.Jobid == "10")
                    chkJobid10.Checked = true;
                else if (_ZymosisReportEntity.Jobid == "11")
                    chkJobid11.Checked = true;
                else if (_ZymosisReportEntity.Jobid == "12")
                    chkJobid12.Checked = true;
                else if (_ZymosisReportEntity.Jobid == "13")
                    chkJobid13.Checked = true;
                else if (_ZymosisReportEntity.Jobid == "14")
                    chkJobid14.Checked = true;
                else if (_ZymosisReportEntity.Jobid == "15")
                    chkJobid15.Checked = true;
                else if (_ZymosisReportEntity.Jobid == "16")
                    chkJobid16.Checked = true;
                else if (_ZymosisReportEntity.Jobid == "17")
                    chkJobid17.Checked = true;
                else if (_ZymosisReportEntity.Jobid == "18")
                    chkJobid18.Checked = true;

                if (_ZymosisReportEntity.Recordtype1 == "1")
                    chkRecordtype11.Checked = true;
                else if (_ZymosisReportEntity.Recordtype1 == "2")
                    chkRecordtype12.Checked = true;
                else if (_ZymosisReportEntity.Recordtype1 == "3")
                    chkRecordtype13.Checked = true;

                if (_ZymosisReportEntity.Recordtype2 == "1")
                    chkRecordtype21.Checked = true;
                else if (_ZymosisReportEntity.Recordtype2 == "2")
                    chkRecordtype22.Checked = true;
                else if (_ZymosisReportEntity.Recordtype2 == "3")
                    chkRecordtype23.Checked = true;

                //密切接触者有无症状
                if (_ZymosisReportEntity.InfectotherFlag == "0")
                {
                    cheJieChu0.Checked = true;
                }
                else if (_ZymosisReportEntity.InfectotherFlag == "1")
                {
                    cheJieChu1.Checked = true;
                }

                if (!String.IsNullOrEmpty(_ZymosisReportEntity.Attackdate))
                {
                    dateAttackdate.DateTime = Convert.ToDateTime(_ZymosisReportEntity.Attackdate);
                }
                else
                {
                    dateAttackdate.Text = "";
                }

                if (!String.IsNullOrEmpty(_ZymosisReportEntity.Diagdate))
                {
                    dateDiagdate.DateTime = Convert.ToDateTime(_ZymosisReportEntity.Diagdate);
                    timeDiagdate.Time = Convert.ToDateTime(_ZymosisReportEntity.Diagdate);
                }
                else
                {
                    dateDiagdate.Text = "";
                }

                if (!String.IsNullOrEmpty(_ZymosisReportEntity.Diedate))
                {
                    Diedate.DateTime = Convert.ToDateTime(_ZymosisReportEntity.Diedate);
                }
                else
                {
                    Diedate.Text = "";
                }

                lookUpEditorZymosis.CodeValue = _ZymosisReportEntity.Diagicd10;
                textCorrectName.Text = _ZymosisReportEntity.CorrectName;
                txtCancelReason.Text = _ZymosisReportEntity.CancelReason;
                lookUpEditorDept.CodeValue = _ZymosisReportEntity.Reportdeptcode;
                txtDoctortel.Text = _ZymosisReportEntity.Doctortel;

                lookUpEditorDoc.CodeValue = _ZymosisReportEntity.Reportdoccode;
                if (!String.IsNullOrEmpty(_ZymosisReportEntity.ReportDate))
                {
                    dateReportDate.DateTime = Convert.ToDateTime(_ZymosisReportEntity.ReportDate);
                }
                else
                {
                    dateReportDate.Text = "";
                }

                memoMemo.Text = _ZymosisReportEntity.Memo;

                memoOtherDiag.Text = _ZymosisReportEntity.OtherDiag;
                #endregion
            }
        }

        /// <summary>
        /// 根据页面值获取报告卡实体
        /// </summary>
        /// <param name="_ZymosisReportEntity"></param>
        /// <returns></returns>
        private ZymosisReportEntity GetEntityUI(ZymosisReportEntity _ZymosisReportEntity)
        {
            try
            {
                #region 将页面值保存到实体中

                _ZymosisReportEntity.ReportNo = txtReportNo.Text;//报告卡卡号
                if (chkReportType1.Checked)
                {
                    _ZymosisReportEntity.ReportType = "1";
                }
                else if (chkReportType2.Checked)
                {
                    _ZymosisReportEntity.ReportType = "2";
                }

                _ZymosisReportEntity.Name = txtName.Text;
                _ZymosisReportEntity.Parentname = txtParentname.Text;
                _ZymosisReportEntity.Idno = txtIdno.Text;

                if (chkSex1.Checked)
                    _ZymosisReportEntity.Sex = "1";
                else if (chkSex2.Checked)
                    _ZymosisReportEntity.Sex = "2";

                if (!(dateBirth.DateTime.CompareTo(DateTime.MinValue) == 0))
                    _ZymosisReportEntity.Birth = dateBirth.DateTime.ToString("yyyy-MM-dd");

                _ZymosisReportEntity.Age = txtAge.Text;
                if (chkAgeUnit1.Checked)
                    _ZymosisReportEntity.AgeUnit = "1";
                else if (chkAgeUnit2.Checked)
                    _ZymosisReportEntity.AgeUnit = "2";
                else if (chkAgeUnit3.Checked)
                    _ZymosisReportEntity.AgeUnit = "3";

                if (cheJieChu1.Checked)
                    _ZymosisReportEntity.InfectotherFlag = "1";
                else if (cheJieChu0.Checked)
                    _ZymosisReportEntity.InfectotherFlag = "0";


                _ZymosisReportEntity.Organization = txtOrganization.Text;
                _ZymosisReportEntity.Officetel = txtOfficetel.Text;

                if (chkAddresstype1.Checked)
                    _ZymosisReportEntity.Addresstype = "1";
                else if (chkAddresstype2.Checked)
                    _ZymosisReportEntity.Addresstype = "2";
                else if (chkAddresstype3.Checked)
                    _ZymosisReportEntity.Addresstype = "3";
                else if (chkAddresstype4.Checked)
                    _ZymosisReportEntity.Addresstype = "4";
                else if (chkAddresstype5.Checked)
                    _ZymosisReportEntity.Addresstype = "5";
                else if (chkAddresstype6.Checked)
                    _ZymosisReportEntity.Addresstype = "6";

                _ZymosisReportEntity.Address = txtAddress.Text;

                if (chkJobid1.Checked)
                    _ZymosisReportEntity.Jobid = "1";
                else if (chkJobid2.Checked)
                    _ZymosisReportEntity.Jobid = "2";
                else if (chkJobid3.Checked)
                    _ZymosisReportEntity.Jobid = "3";
                else if (chkJobid4.Checked)
                    _ZymosisReportEntity.Jobid = "4";
                else if (chkJobid5.Checked)
                    _ZymosisReportEntity.Jobid = "5";
                else if (chkJobid6.Checked)
                    _ZymosisReportEntity.Jobid = "6";
                else if (chkJobid7.Checked)
                    _ZymosisReportEntity.Jobid = "7";
                else if (chkJobid8.Checked)
                    _ZymosisReportEntity.Jobid = "8";
                else if (chkJobid9.Checked)
                    _ZymosisReportEntity.Jobid = "9";
                else if (chkJobid10.Checked)
                    _ZymosisReportEntity.Jobid = "10";
                else if (chkJobid11.Checked)
                    _ZymosisReportEntity.Jobid = "11";
                else if (chkJobid12.Checked)
                    _ZymosisReportEntity.Jobid = "12";
                else if (chkJobid13.Checked)
                    _ZymosisReportEntity.Jobid = "13";
                else if (chkJobid14.Checked)
                    _ZymosisReportEntity.Jobid = "14";
                else if (chkJobid15.Checked)
                    _ZymosisReportEntity.Jobid = "15";
                else if (chkJobid16.Checked)
                    _ZymosisReportEntity.Jobid = "16";
                else if (chkJobid17.Checked)
                    _ZymosisReportEntity.Jobid = "17";
                else if (chkJobid18.Checked)
                    _ZymosisReportEntity.Jobid = "18";


                if (chkRecordtype11.Checked)
                    _ZymosisReportEntity.Recordtype1 = "1";
                else if (chkRecordtype12.Checked)
                    _ZymosisReportEntity.Recordtype1 = "2";
                else if (chkRecordtype13.Checked)
                    _ZymosisReportEntity.Recordtype1 = "3";

                if (chkRecordtype21.Checked)
                    _ZymosisReportEntity.Recordtype2 = "1";
                else if (chkRecordtype22.Checked)
                    _ZymosisReportEntity.Recordtype2 = "2";
                else if (chkRecordtype23.Checked)
                    _ZymosisReportEntity.Recordtype2 = "3";

                if (!(dateAttackdate.DateTime.CompareTo(DateTime.MinValue) == 0))
                    _ZymosisReportEntity.Attackdate = dateAttackdate.DateTime.ToString("yyyy-MM-dd");

                if (!(dateDiagdate.DateTime.CompareTo(DateTime.MinValue) == 0))
                    _ZymosisReportEntity.Diagdate = dateDiagdate.DateTime.ToString("yyyy-MM-dd") + " " + timeDiagdate.Time.ToString("HH:mm:ss");

                if (!(Diedate.DateTime.CompareTo(DateTime.MinValue) == 0))
                    _ZymosisReportEntity.Diedate = Diedate.DateTime.ToString("yyyy-MM-dd");

                _ZymosisReportEntity.Diagicd10 = lookUpEditorZymosis.CodeValue;
                _ZymosisReportEntity.Diagname = lookUpEditorZymosis.DisplayValue;
                _ZymosisReportEntity.CorrectName = textCorrectName.Text;
                _ZymosisReportEntity.CancelReason = txtCancelReason.Text;
                _ZymosisReportEntity.Reportdeptcode = lookUpEditorDept.CodeValue;

                _ZymosisReportEntity.Reportdeptname = lookUpWindowDept.DisplayValue;
                _ZymosisReportEntity.Doctortel = txtDoctortel.Text;
                _ZymosisReportEntity.Reportdoccode = lookUpEditorDoc.CodeValue;
                _ZymosisReportEntity.Reportdocname = lookUpWindowDoc.DisplayValue;

                if (!(dateReportDate.DateTime.CompareTo(DateTime.MinValue) == 0))
                    _ZymosisReportEntity.ReportDate = dateReportDate.DateTime.ToString("yyyy-MM-dd");

                _ZymosisReportEntity.Memo = memoMemo.Text;
                _ZymosisReportEntity.OtherDiag = memoOtherDiag.Text;

                _ZymosisReportEntity.ModifyDate = DateTime.Now.ToString("yyyy-MM-dd HH:m:ss");
                _ZymosisReportEntity.ModifyDeptcode = m_Host.User.CurrentDeptId;
                _ZymosisReportEntity.ModifyDeptname = m_Host.User.CurrentDeptName;
                _ZymosisReportEntity.ModifyUsercode = m_Host.User.Id;
                _ZymosisReportEntity.ModifyUsername = m_Host.User.Name;

                return _ZymosisReportEntity;


                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region 对外提供操作方法

        /// <summary>
        /// 保存验证
        /// </summary>
        /// edit by Yanqiao.Cai 2013-03-13 add try ... catch
        public bool Save()
        {
            try
            {
                if (m_ZymosisReportEntity == null)
                {
                    MyMessageBox.Show("请选择一条病人信息或补录传染病报告信息");
                    return false;
                }

                //add by cyq 2012-10-24
                string errorStr = CheckItem();
                if (!string.IsNullOrEmpty(errorStr))
                {
                    MyMessageBox.Show(errorStr);
                    return false;
                }

                string ErrorMsg;//用于在判断保存状态下，单据状态已经改变导致保存失败，而返回的消息
                //保存数据
                bool boo = PrivateSave(out ErrorMsg);
                if (!string.IsNullOrEmpty(ErrorMsg))
                {
                    MyMessageBox.Show(ErrorMsg);
                    return boo;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 提交
        /// </summary>
        public bool Submit()
        {
            try
            {
                if (m_ZymosisReportEntity == null)
                {
                    MyMessageBox.Show("请选择一条传染病上报记录或补录传染病报告信息");
                    return false;
                }

                //add by cyq 2012-10-24
                string errorStr = CheckItem();
                if (!string.IsNullOrEmpty(errorStr))
                {
                    MyMessageBox.Show(errorStr);
                    return false;
                }

                if (MyMessageBox.Show("您确定要提交该传染病上报记录吗？", "提交传染病上报记录", MyMessageBoxButtons.OkCancel) == DialogResult.OK)
                {
                    //edit by cyq 2012-10-24
                    //if (!CheckDate())
                    //    return false;

                    string ErrorMsg;//用于在判断提交状态下，单据状态已经改变导致保存失败，而返回的消息
                    //提交之前先保存数据到数据库中
                    PrivateSave(out ErrorMsg);//（测试保存中的预先注释掉）

                    m_ZymosisReportEntity = SqlHelper.GetZymosisReportEntity(ReportID);

                    m_ZymosisReportEntity.State = "2";
                    if (SqlHelper == null)
                        SqlHelper = new SqlHelper(m_Host.SqlHelper);

                    string m_ReportId = m_ZymosisReportEntity.ReportId.ToString();//获得报告卡号
                    //string m_OldStateId = m_ZymosisReportEntity.State.ToString();//获得原来的状态字段

                    //提交操作判断状态是否改变 add by ywk
                    ZymosisReportEntity _myZymosisReportEntity = new ZymosisReportEntity();
                    _myZymosisReportEntity = SqlHelper.GetZymosisReportEntity(m_ReportId);

                    if (!ErrorMsg.Contains("失败"))//保存动作已经成功
                    {
                        if ("1" == _myZymosisReportEntity.State)//状态为1（已经保存的）才进行提交
                        {
                            SqlHelper.UpdateZymosisReport(m_ZymosisReportEntity, "提交");
                            MyMessageBox.Show("提交成功");
                            m_ZymosisReportEntity = null;
                            return true;
                        }
                        else
                        {
                            MyMessageBox.Show("此报告卡状态已经改变");
                            return false;
                        }
                    }
                    else
                    {
                        MyMessageBox.Show("此报告卡状态已经改变");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                MyMessageBox.Show("提交失败");
                return false;
            }
        }

        /// <summary>
        /// 填写人撤回
        /// </summary>
        /// edit by Yanqiao.Cai 2013-03-13 add try ... catch
        public bool Withdraw()
        {
            try
            {
                if (m_ZymosisReportEntity == null)
                {
                    MyMessageBox.Show("请选择一条传染病上报记录或补录传染病报告信息");
                    return false;
                }

                if (MyMessageBox.Show("您确定要撤回吗？", "", MyMessageBoxButtons.OkCancel) == DialogResult.OK)
                {
                    ZymosisReportEntity _ZymosisReportEntity = GetEntityUI(m_ZymosisReportEntity);
                    if (_ZymosisReportEntity == null)
                        return false;
                    _ZymosisReportEntity.State = "3";

                    if (SqlHelper == null)
                        SqlHelper = new SqlHelper(m_Host.SqlHelper);
                    try
                    {
                        SqlHelper.UpdateZymosisReport(_ZymosisReportEntity, "撤回");
                        MyMessageBox.Show("撤回成功");
                        return true;
                    }
                    catch
                    {
                        MyMessageBox.Show("撤回失败");
                        return false;
                    }

                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 审核通过
        /// </summary>
        /// edit by Yanqiao.Cai 2013-03-13 add try ... catch
        public bool Approv()
        {
            try
            {
                if (m_ZymosisReportEntity == null)
                {
                    MyMessageBox.Show("请选择一条传染病上报记录或补录传染病报告信息");
                    return false;
                }

                if (MyMessageBox.Show("您确定要审核通过吗？", "审核通过", MyMessageBoxButtons.OkCancel) == DialogResult.OK)
                {
                    ZymosisReportEntity _ZymosisReportEntity = GetEntityUI(m_ZymosisReportEntity);
                    if (_ZymosisReportEntity == null)
                        return false;
                    _ZymosisReportEntity.State = "4";

                    _ZymosisReportEntity.AuditDate = DateTime.Now.ToString("yyyy-MM-dd HH:m:ss");
                    _ZymosisReportEntity.AuditDeptcode = m_Host.User.CurrentDeptId;
                    _ZymosisReportEntity.AuditDeptname = m_Host.User.CurrentDeptName;
                    _ZymosisReportEntity.AuditUsercode = m_Host.User.Id;
                    _ZymosisReportEntity.AuditUsername = m_Host.User.Name;

                    if (SqlHelper == null)
                        SqlHelper = new SqlHelper(m_Host.SqlHelper);
                    try
                    {
                        SqlHelper.UpdateZymosisReport(_ZymosisReportEntity, "审核通过");
                        m_ZymosisReportEntity = null;
                        MyMessageBox.Show("审核成功");
                        return true;
                    }
                    catch
                    {
                        MyMessageBox.Show("审核失败");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 退回，审核未通过
        /// </summary>
        /// edit by Yanqiao.Cai 2013-03-13 add try ... catch
        public bool UnPassApprove()
        {
            try
            {
                if (m_ZymosisReportEntity == null)
                {
                    MyMessageBox.Show("请选择一条传染病上报记录或补录传染病报告信息");
                    return false;
                }
                //调用填写驳回原因窗口
                UnPassReason unPassReason = new UnPassReason(m_Host);
                if (unPassReason.ShowDialog() == DialogResult.OK)//点击确定按钮
                {
                    //解决如果否决原因为空也可以进行否决操作BUG  eidt by ywk 2012年3月28日8:59:17
                    MemoEdit memoEditReason = unPassReason.Controls["memoEditReason"] as MemoEdit;
                    string rejectmemo = memoEditReason.Text.ToString();//取得否决原因
                    //判断是否填写了否决原因
                    if (!string.IsNullOrEmpty(rejectmemo))
                    {
                        txtCancelReason.Text = unPassReason.PassReason;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

                ZymosisReportEntity _ZymosisReportEntity = GetEntityUI(m_ZymosisReportEntity);
                if (_ZymosisReportEntity == null)
                    return false;
                _ZymosisReportEntity.State = "5";

                if (SqlHelper == null)
                    SqlHelper = new SqlHelper(m_Host.SqlHelper);
                try
                {
                    SqlHelper.UpdateZymosisReport(_ZymosisReportEntity, "退回，审核未通过");
                    MyMessageBox.Show("驳回成功");
                    m_ZymosisReportEntity = null;
                    return true;
                }
                catch
                {
                    MyMessageBox.Show("驳回失败");
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 上报
        /// </summary>
        /// edit by Yanqiao.Cai 2013-03-13 add try ... catch
        public bool Report()
        {
            try
            {
                if (m_ZymosisReportEntity == null)
                {
                    m_Host.CustomMessageBox.MessageShow("请选择一条记录", CustomMessageBoxKind.InformationOk);
                    return false;
                }

                if (m_Host.CustomMessageBox.MessageShow("您确认要上报吗？", DrectSoft.Core.CustomMessageBoxKind.QuestionYesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    ZymosisReportEntity _ZymosisReportEntity = GetEntityUI(m_ZymosisReportEntity);
                    if (_ZymosisReportEntity == null)
                        return false;
                    _ZymosisReportEntity.State = "6";

                    if (SqlHelper == null)
                        SqlHelper = new SqlHelper(m_Host.SqlHelper);
                    try
                    {
                        SqlHelper.UpdateZymosisReport(_ZymosisReportEntity, "上报");
                        m_Host.CustomMessageBox.MessageShow("上报成功");
                        return true;
                    }
                    catch
                    {
                        m_Host.CustomMessageBox.MessageShow("上报失败");
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        public bool Cancel()
        {
            try
            {
                if (m_ZymosisReportEntity == null)
                {
                    MyMessageBox.Show("请选择一条记录");
                    return false;
                }

                if (m_ZymosisReportEntity.ReportId == 0)
                {
                    MyMessageBox.Show("该传染病上报记录尚未保存或提交，不需要删除");
                    return false;
                }

                if (MyMessageBox.Show("您确定要删除吗？", "删除传染病上报记录", MyMessageBoxButtons.OkCancel) == DialogResult.OK)
                {
                    ZymosisReportEntity _ZymosisReportEntity = GetEntityUI(m_ZymosisReportEntity);
                    if (_ZymosisReportEntity == null)
                        return false;
                    _ZymosisReportEntity.State = "7";

                    if (SqlHelper == null)
                        SqlHelper = new SqlHelper(m_Host.SqlHelper);

                    SqlHelper.UpdateZymosisReport(_ZymosisReportEntity, "删除");
                    MyMessageBox.Show("删除成功");
                    return true;
                }
                return false;
            }
            catch
            {
                MyMessageBox.Show("删除失败");
                return false;
            }
        }

        #endregion

        #region 初始化控件值

        /// <summary>
        /// 初始化科室
        /// </summary>
        private void InitDepartment()
        {

            lookUpWindowDept.SqlHelper = m_Host.SqlHelper;

            string sql = string.Format(@"select distinct ID, NAME,py,wb from department a ,dept2ward b where a.id = b.deptid ");
            DataTable Dept = m_Host.SqlHelper.ExecuteDataTable(sql);

            Dept.Columns["ID"].Caption = "科室代码";
            Dept.Columns["NAME"].Caption = "科室名称";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("ID", 60);
            cols.Add("NAME", 150);

            SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//Name//PY//WB");
            lookUpEditorDept.SqlWordbook = deptWordBook;

        }

        /// <summary>
        /// 初始传染病病种类别
        /// </summary>
        public void InitBzlb()
        {

            lookUpWindowZymosis.SqlHelper = m_Host.SqlHelper;

            //查找时查找副卡类别
            string sql = string.Format(@"select (case when a.level_id = 1 then '甲类传染病' when a.level_id = 2 then
                                                '乙类传染病' when a.level_id = 3 then '丙类传染病'
                                                else '其他传染病' end) level_Name,a.icd,a.name,a.py,a.wb,a.namestr,a.FUKAType
                                          from Zymosis_Diagnosis a where a.valid = 1");
            Bzlb = m_Host.SqlHelper.ExecuteDataTable(sql);

            Bzlb.Columns["ICD"].Caption = "病种代码";
            Bzlb.Columns["NAME"].Caption = "病种名称";
            Bzlb.Columns["NAMESTR"].Caption = "上报名称";
            Bzlb.Columns["LEVEL_NAME"].Caption = "传染等级";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("ICD", 100);
            cols.Add("NAME", 200);
            cols.Add("NAMESTR", 200);
            cols.Add("LEVEL_NAME", 102);

            SqlWordbook deptWordBook = new SqlWordbook("querybook", Bzlb, "ICD", "NAMESTR", cols, "ICD//NAMESTR//PY//WB//NAME//LEVEL_NAME");
            lookUpEditorZymosis.SqlWordbook = deptWordBook;

        }

        /// <summary>
        /// 绑定医生下拉框
        /// </summary>
        private void InitDoctor(string deptid)
        {
            ///修改时间：2012-08-03
            ///修改人：cyq

            lookUpWindowDoc.SqlHelper = m_Host.SqlHelper;

            string sql = string.Format(@"select distinct u.ID,u.NAME,u.PY,u.WB,u.grade from Users u join categorydetail c on u.grade=c.id and c.categoryid='20' and  c.id in('2000','2001','2002','2003') and u.deptid = '{0}' ", deptid);
            DataTable Bzlb = m_Host.SqlHelper.ExecuteDataTable(sql);

            Bzlb.Columns["ID"].Caption = "医生工号";
            Bzlb.Columns["NAME"].Caption = "医生名称";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("ID", 90);
            cols.Add("NAME", 120);

            SqlWordbook deptWordBook = new SqlWordbook("querybook", Bzlb, "ID", "NAME", cols, "ID//Name//PY//WB");
            lookUpEditorDoc.SqlWordbook = deptWordBook;

        }

        #endregion

        #region 保存时检查数据

        #region 已弃用
        /**
        private bool CheckDate()
        {
            if (!chkReportType1.Checked && !chkReportType2.Checked)
            {
                m_Host.CustomMessageBox.MessageShow("请勾选报卡类别");
                return false;
            }

            if (txtName.Text.Trim() == "")
            {
                m_Host.CustomMessageBox.MessageShow("必须填写姓名！");
                txtName.Focus();
                return false;
            }
            if (!chkSex1.Checked && !chkSex2.Checked)
            {
                m_Host.CustomMessageBox.MessageShow("必须选择性别！");
                return false;
            }
            if ((dateAttackdate.DateTime.CompareTo(DateTime.MinValue) == 0))
            {
                if (txtAge.Text.Trim() == "")
                {
                    m_Host.CustomMessageBox.MessageShow("必须填写出生日期或者实足年龄中一个！");
                    dateAttackdate.Focus();
                    return false;
                }
            }
            //if ((!(dateAttackdate.DateTime.CompareTo(DateTime.MinValue) == 0)) || txtAge.Text.Trim() == "")
            //{
            //    m_Host.CustomMessageBox.MessageShow("必须填写出生日期或者实足年龄中一个！");
            //    dateAttackdate.Focus();
            //    return false;
            //}
            if (txtAge.Text.Trim() != "" && (!chkAgeUnit1.Checked && !chkAgeUnit2.Checked && !chkAgeUnit3.Checked))
            {
                m_Host.CustomMessageBox.MessageShow("填写实足年龄必须选择年龄单位！");
                return false;
            }
            if (!chkAddresstype1.Checked && !chkAddresstype2.Checked && !chkAddresstype3.Checked && !chkAddresstype4.Checked && !chkAddresstype5.Checked && !chkAddresstype6.Checked)
            {
                m_Host.CustomMessageBox.MessageShow("必须选择病人归属地区！");
                return false;
            }
            if (txtAddress.Text.Trim() == "")
            {
                m_Host.CustomMessageBox.MessageShow("必须填写现住址（详细到具体门牌）！");
                txtAddress.Focus();
                return false;
            }
            if (!chkJobid1.Checked && !chkJobid2.Checked && !chkJobid3.Checked && !chkJobid4.Checked && !chkJobid5.Checked && !chkJobid6.Checked
                && !chkJobid7.Checked && !chkJobid8.Checked && !chkJobid9.Checked && !chkJobid10.Checked && !chkJobid11.Checked && !chkJobid12.Checked
                && !chkJobid13.Checked && !chkJobid14.Checked && !chkJobid15.Checked && !chkJobid16.Checked && !chkJobid17.Checked && !chkJobid18.Checked)
            {
                m_Host.CustomMessageBox.MessageShow("必须选择病人职业！");
                return false;
            }
            if (!chkRecordtype11.Checked && !chkRecordtype12.Checked && !chkRecordtype13.Checked)
            {
                m_Host.CustomMessageBox.MessageShow("必须选择病例分类！");
                return false;
            }

            if ((dateAttackdate.DateTime.CompareTo(DateTime.MinValue) == 0))
            {
                m_Host.CustomMessageBox.MessageShow("必须填写发病日期！");
                dateAttackdate.Focus();
                return false;
            }
            if ((dateDiagdate.DateTime.CompareTo(DateTime.MinValue) == 0))
            {
                m_Host.CustomMessageBox.MessageShow("必须填写诊断日期！");
                dateDiagdate.Focus();
                return false;
            }

            if (lookUpEditorZymosis.CodeValue == "" && memoOtherDiag.Text.Trim() == "")
            {
                m_Host.CustomMessageBox.MessageShow("必须填写传染病病种！");
                lookUpEditorZymosis.Focus();
                return false;
            }
            if ((dateReportDate.DateTime.CompareTo(DateTime.MinValue) == 0))
            {
                m_Host.CustomMessageBox.MessageShow("必须填写填卡日期！");
                dateReportDate.Focus();
                return false;
            }

            return true;
        }
        */
        #endregion

        /// <summary>
        /// UI画面验证
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-24</date>
        /// </summary>
        /// edit by Yanqiao.Cai 2013-03-12 增加附卡验证(如果存在附卡)
        /// <returns></returns>
        private string CheckItem()
        {
            try
            {
                if (!chkReportType1.Checked && !chkReportType2.Checked)
                {
                    chkReportType1.Focus();
                    return "请选择报卡类别";
                }
                else if (string.IsNullOrEmpty(txtName.Text))
                {
                    txtName.Focus();
                    return "患者姓名不能为空";
                }
                else if (!chkSex1.Checked && !chkSex2.Checked)
                {
                    chkSex1.Focus();
                    return "请选择性别";
                }
                else if (null == dateBirth.DateTime || string.IsNullOrEmpty(dateBirth.Text))
                {
                    if (string.IsNullOrEmpty(txtAge.Text))
                    {
                        dateBirth.Focus();
                        return "请选择出生日期或者填写实足年龄、年龄单位";
                    }
                }
                else if (!string.IsNullOrEmpty(txtAge.Text) && !chkAgeUnit1.Checked && !chkAgeUnit2.Checked && !chkAgeUnit3.Checked)
                {
                    chkAgeUnit1.Focus();
                    return "您填写了实足年龄，请选择年龄单位";
                }
                else if (!chkAddresstype1.Checked && !chkAddresstype2.Checked && !chkAddresstype3.Checked && !chkAddresstype4.Checked && !chkAddresstype5.Checked && !chkAddresstype6.Checked)
                {
                    this.chkAddresstype1.Focus();
                    return "请选择病人归属地";
                }
                else if (string.IsNullOrEmpty(txtAddress.Text))
                {
                    this.txtAddress.Focus();
                    return "现居住地址不能为空(详细到门牌号)";
                }
                else if (!chkJobid1.Checked && !chkJobid2.Checked && !chkJobid3.Checked && !chkJobid4.Checked && !chkJobid5.Checked && !chkJobid6.Checked
                    && !chkJobid7.Checked && !chkJobid8.Checked && !chkJobid9.Checked && !chkJobid10.Checked && !chkJobid11.Checked && !chkJobid12.Checked
                    && !chkJobid13.Checked && !chkJobid14.Checked && !chkJobid15.Checked && !chkJobid16.Checked && !chkJobid17.Checked && !chkJobid18.Checked)
                {
                    this.chkJobid1.Focus();
                    return "请选择患者职业";
                }
                else if (!chkRecordtype11.Checked && !chkRecordtype12.Checked && !chkRecordtype13.Checked)
                {
                    this.chkRecordtype11.Focus();
                    return "请选择病例分类 (1)";
                }
                else if (!chkRecordtype21.Checked && !chkRecordtype22.Checked && !chkRecordtype23.Checked)
                {
                    this.chkRecordtype21.Focus();
                    return "请选择病例分类 (2)";
                }
                else if (null == dateAttackdate.DateTime || string.IsNullOrEmpty(dateAttackdate.Text))
                {
                    this.dateAttackdate.Focus();
                    return "请选择发病日期";
                }
                else if (null == dateDiagdate.DateTime || string.IsNullOrEmpty(dateDiagdate.Text))
                {
                    this.dateDiagdate.Focus();
                    return "请选择诊断日期";
                }
                else if (dateAttackdate.DateTime > DateTime.Now)
                {
                    this.dateAttackdate.Focus();
                    return "发病日期不能大于当前日期";
                }
                //中心医院需求，传染病卡需24小时报上去，诊断日期可大于当前日期ywk 二〇一三年五月三十日 08:36:02 
                //else if (dateDiagdate.DateTime > DateTime.Now)
                //{
                //    this.dateDiagdate.Focus();
                //    return "诊断日期不能大于当前日期";
                //}
                else if (Diedate.DateTime > DateTime.Now)
                {
                    this.Diedate.Focus();
                    return "死亡日期不能大于当前日期";
                }
                else if (null != Diedate.DateTime && !string.IsNullOrEmpty(Diedate.Text))
                {
                    if (Diedate.DateTime < dateAttackdate.DateTime)
                    {
                        this.Diedate.Focus();
                        return "死亡日期不能小于发病日期";
                    }
                    //else if (Diedate.DateTime < dateDiagdate.DateTime)
                    //{
                    //    this.Diedate.Focus();
                    //    return "死亡日期不能小于诊断日期";
                    //}
                }
                else if (string.IsNullOrEmpty(lookUpEditorZymosis.CodeValue) && string.IsNullOrEmpty(memoOtherDiag.Text))
                {
                    this.lookUpEditorZymosis.Focus();
                    return "请选择传染病病种或者填写传染病";
                }
                else if (null != card_HepatitisB)//附卡验证 add by cyq 2013-03-12
                {
                    string errorStr = card_HepatitisB.CheckItem();
                    if (!string.IsNullOrEmpty(errorStr))
                    {
                        return errorStr;
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        /// <summary>
        /// 新增errorinfo字段，用于存放返回的操作错误的信息 
        /// edit by ywk 2012年3月28日13:54:28
        /// </summary>
        /// edit by Yanqiao.Cai 2013-03-12
        /// 1、add try ... catch
        /// 2、插入附卡记录(如果存在附卡)
        /// <param name="errorinfo"></param>
        /// <returns></returns>
        private bool PrivateSave(out string errorinfo)
        {
            try
            {
                //新增保存
                if (m_ZymosisReportEntity.ReportId == 0)
                {
                    ZymosisReportEntity _ZymosisReportEntity = GetEntityUI(m_ZymosisReportEntity);

                    if (_ZymosisReportEntity == null)
                    {
                        errorinfo = "传染病报告实体不能为空";
                        return false;
                    }

                    _ZymosisReportEntity.CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:m:ss");
                    _ZymosisReportEntity.CreateDeptcode = m_Host.User.CurrentDeptId;
                    _ZymosisReportEntity.CreateDeptname = m_Host.User.CurrentDeptName;
                    _ZymosisReportEntity.CreateUsercode = m_Host.User.Id;
                    _ZymosisReportEntity.CreateUsername = m_Host.User.Name;
                    _ZymosisReportEntity.State = "1";

                    if (SqlHelper == null)
                    {
                        SqlHelper = new SqlHelper(m_Host.SqlHelper);
                    }
                    try
                    {
                        ReportID = SqlHelper.InsertZymosisReport(_ZymosisReportEntity);
                        m_ZymosisReportEntity.ReportId = Convert.ToInt32(ReportID);
                        if (!string.IsNullOrEmpty(ReportID))
                        {

                            string configVal = DS_SqlService.GetConfigValueByKey("YiChangChuangRangBin");
                            if (configVal == "1" || string.IsNullOrEmpty(configVal))
                            {
                                //xll 2013-08-16
                                m_ZymosisReportEntity = SqlHelper.GetZymosisReportEntity(ReportID);
                                int result = 1;
                                if (IsShowAttachedCard() && this.panelControl_attachedCard.Visible && null != card_HepatitisB)
                                {///是否需要保存附卡 - 乙肝
                                    ///新增附卡记录(乙肝) add by cyq 2013-03-13
                                    List<OracleParameter> paramsList = GetCardHepatitisBParams(EditState.Add);
                                    result = DS_SqlService.InsertCardHepatitisB(paramsList);
                                }
                                errorinfo = result == 1 ? "新增成功" : "新增失败";
                                return result == 1;
                            }
                            else //走最新保存方法
                            {
                                if (panelControl_attachedCard.Controls != null)
                                {
                                    IFuCade iFuCade = panelControl_attachedCard.Controls[0] as IFuCade;
                                    if (iFuCade != null)
                                    {
                                        bool saveOK = iFuCade.ValideAndSave(ReportID);
                                        if (saveOK)
                                        {
                                            errorinfo = "新增成功";
                                            return true;
                                        }
                                        else
                                        {
                                            errorinfo = "";
                                            return false;
                                        }
                                    }
                                }
                                errorinfo = "新增成功";
                                return true;
                            }

                        }
                        else
                        {
                            errorinfo = "新增失败";
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                        //errorinfo = "新增失败";
                        //return false;
                    }

                }
                //修改保存   =========(现在保存需判断下报告单状态是否已经改变)========
                else
                {
                    string m_ReportId = m_ZymosisReportEntity.ReportId.ToString();//获得报告卡号
                    string m_OldStateId = m_ZymosisReportEntity.State.ToString();//获得原来的状态字段

                    ZymosisReportEntity _ZymosisReportEntity = GetEntityUI(m_ZymosisReportEntity);
                    if (_ZymosisReportEntity == null)
                    {
                        errorinfo = "传染病报告实体不能为空";
                        return false;
                    }
                    _ZymosisReportEntity.State = "1";

                    if (SqlHelper == null)
                    {
                        SqlHelper = new SqlHelper(m_Host.SqlHelper);
                    }
                    try
                    {
                        //保存操作判断状态是否改变
                        ZymosisReportEntity _myZymosisReportEntity = new ZymosisReportEntity();
                        _myZymosisReportEntity = SqlHelper.GetZymosisReportEntity(m_ReportId);
                        if (m_OldStateId == _myZymosisReportEntity.State)//原来状态和现在状态一致
                        {
                            ReportID = SqlHelper.UpdateZymosisReport(_ZymosisReportEntity, "修改保存");
                            int result = 1;
                            if (!string.IsNullOrEmpty(ReportID))
                            {
                                //xll 20130816
                                string configVal = DS_SqlService.GetConfigValueByKey("YiChangChuangRangBin");
                                if (configVal == "1" || string.IsNullOrEmpty(configVal))
                                {
                                    m_ZymosisReportEntity = SqlHelper.GetZymosisReportEntity(ReportID);
                                    if (IsShowAttachedCard() && this.panelControl_attachedCard.Visible && null != card_HepatitisB)
                                    {///是否需要保存附卡 - 乙肝
                                        ///修改附卡记录(乙肝) add by cyq 2013-03-13
                                        List<OracleParameter> paramsList = GetCardHepatitisBParams(EditState.Edit);
                                        if (paramsList.Any(p => p.ParameterName == "id"))
                                        {
                                            result = DS_SqlService.UpdateCardHepatitisB(paramsList);
                                        }
                                        else
                                        {
                                            result = DS_SqlService.InsertCardHepatitisB(paramsList);
                                        }
                                    }
                                    errorinfo = result == 1 ? "修改成功" : "修改失败";
                                    return result == 1;
                                }
                                else
                                {
                                    if (panelControl_attachedCard.Controls != null)
                                    {
                                        IFuCade iFuCade = panelControl_attachedCard.Controls[0] as IFuCade;
                                        if (iFuCade != null)
                                        {
                                            bool saveOk = iFuCade.ValideAndSave(ReportID);
                                            if (saveOk)
                                            {
                                                errorinfo = "修改成功";
                                                return true;
                                            }
                                            else
                                            {
                                                errorinfo = "";
                                                return false;
                                            }
                                        }
                                    }
                                    errorinfo = "修改成功";
                                    return true;
                                }
                            }
                            else
                            {
                                errorinfo = "修改失败";
                                return false;
                            }
                        }
                        else//状态改变
                        {
                            errorinfo = "此报告卡状态已经改变";
                            return false;
                        }
                        //ReportID = SqlHelper.UpdateZymosisReport(_ZymosisReportEntity, "修改保存");
                        //m_ZymosisReportEntity = SqlHelper.GetZymosisReportEntity(ReportID);
                        //return true;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                        // errorinfo = "修改失败";
                        // return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
                // throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取附卡参数-乙肝
        /// </summary>
        /// <param name="editState">编辑状态标识</param>
        /// <returns></returns>
        public List<OracleParameter> GetCardHepatitisBParams(EditState editState)
        {
            try
            {
                List<OracleParameter> list = new List<OracleParameter>();
                if (null == card_HepatitisB)
                {
                    return list;
                }
                list = card_HepatitisB.GetHepatitisBParams();
                ///报告卡ID(自增ID)
                if (!list.Any(p => p.ParameterName == "@report_id"))
                {
                    OracleParameter param1 = new OracleParameter("report_id", OracleType.Int32);
                    param1.Value = m_ZymosisReportEntity.ReportId;
                    list.Add(param1);
                }
                ///病种编码
                if (!list.Any(p => p.ParameterName == "@diagicd10"))
                {
                    OracleParameter param2 = new OracleParameter("diagicd10", OracleType.VarChar);
                    param2.Value = m_ZymosisReportEntity.Diagicd10;
                    list.Add(param2);
                }
                ///病种名称
                if (!list.Any(p => p.ParameterName == "@diagname"))
                {
                    OracleParameter param3 = new OracleParameter("diagname", OracleType.VarChar);
                    param3.Value = m_ZymosisReportEntity.Diagname;
                    list.Add(param3);
                }
                ///是否有效
                if (!list.Any(p => p.ParameterName == "@valid"))
                {
                    OracleParameter param4 = new OracleParameter("valid", OracleType.Int32);
                    param4.Value = string.IsNullOrEmpty(m_ZymosisReportEntity.Vaild) ? 1 : int.Parse(m_ZymosisReportEntity.Vaild);
                    list.Add(param4);
                }

                if (editState == EditState.Add)
                {///新增
                    ///创建人
                    if (!list.Any(p => p.ParameterName == "@createuser"))
                    {
                        OracleParameter param5 = new OracleParameter("createuser", OracleType.VarChar);
                        param5.Value = DS_Common.currentUser.Id;
                        list.Add(param5);
                    }
                    ///创建时间
                    if (!list.Any(p => p.ParameterName == "@createtime"))
                    {
                        OracleParameter param6 = new OracleParameter("createtime", OracleType.DateTime);
                        param6.Value = DateTime.Now;
                        list.Add(param6);
                    }
                }
                else
                {///编辑
                    ///根据report_id查询attachedcard_hepatitisb.id
                    DataTable hbTable = DS_SqlService.GetHepatitisBByReportID(m_ZymosisReportEntity.ReportId, m_ZymosisReportEntity.Diagicd10);
                    if (null != hbTable && hbTable.Rows.Count > 0)
                    {///存在附卡-乙肝记录
                        ///ID
                        if (!list.Any(p => p.ParameterName == "@id"))
                        {
                            OracleParameter param9 = new OracleParameter("id", OracleType.Int32);
                            param9.Value = int.Parse(hbTable.Rows[0]["id"].ToString().Trim());
                            list.Add(param9);
                        }
                        ///编辑人
                        if (!list.Any(p => p.ParameterName == "@updateuser"))
                        {
                            OracleParameter param7 = new OracleParameter("updateuser", OracleType.VarChar);
                            param7.Value = DS_Common.currentUser.Id;
                            list.Add(param7);
                        }
                        ///编辑时间
                        if (!list.Any(p => p.ParameterName == "@updatetime"))
                        {
                            OracleParameter param8 = new OracleParameter("updatetime", OracleType.DateTime);
                            param8.Value = DateTime.Now;
                            list.Add(param8);
                        }
                    }
                    else
                    {///不存在附卡-乙肝记录
                        ///创建人
                        if (!list.Any(p => p.ParameterName == "@createuser"))
                        {
                            OracleParameter param10 = new OracleParameter("createuser", OracleType.VarChar);
                            param10.Value = DS_Common.currentUser.Id;
                            list.Add(param10);
                        }
                        ///创建时间
                        if (!list.Any(p => p.ParameterName == "@createtime"))
                        {
                            OracleParameter param11 = new OracleParameter("createtime", OracleType.DateTime);
                            param11.Value = DateTime.Now;
                            list.Add(param11);
                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void ReadOnlyControl(string userRole)
        {
            if (userRole == "1")
            {
                //ReadOnly(false);
                EnableState(true);
            }
            else if (userRole == "2")
            {
                //ReadOnly(true);
                EnableState(false);
            }
        }

        /// <summary>
        /// 将当前页面设置为只读
        /// </summary>
        public void ReadOnly(bool IsReadOnly)
        {
            if (IsReadOnly)
            {
                txtReportNo.Properties.ReadOnly = true;
                chkReportType1.Properties.ReadOnly = true;
                chkReportType2.Properties.ReadOnly = true;
                txtName.Properties.ReadOnly = true;
                txtParentname.Properties.ReadOnly = true;

                txtIdno.Properties.ReadOnly = true;
                chkSex1.Properties.ReadOnly = true;
                chkSex2.Properties.ReadOnly = true;
                dateBirth.Properties.ReadOnly = true;
                txtAge.Properties.ReadOnly = true;

                chkAgeUnit1.Properties.ReadOnly = true;
                chkAgeUnit2.Properties.ReadOnly = true;
                chkAgeUnit3.Properties.ReadOnly = true;
                txtOrganization.Properties.ReadOnly = true;
                txtOfficetel.Properties.ReadOnly = true;

                chkAddresstype1.Properties.ReadOnly = true;
                chkAddresstype2.Properties.ReadOnly = true;
                chkAddresstype3.Properties.ReadOnly = true;
                chkAddresstype4.Properties.ReadOnly = true;
                chkAddresstype5.Properties.ReadOnly = true;

                chkAddresstype6.Properties.ReadOnly = true;
                txtAddress.Properties.ReadOnly = true;

                chkJobid1.Properties.ReadOnly = true;
                chkJobid2.Properties.ReadOnly = true;
                chkJobid3.Properties.ReadOnly = true;
                chkJobid4.Properties.ReadOnly = true;
                chkJobid5.Properties.ReadOnly = true;
                chkJobid6.Properties.ReadOnly = true;
                chkJobid7.Properties.ReadOnly = true;
                chkJobid8.Properties.ReadOnly = true;
                chkJobid9.Properties.ReadOnly = true;
                chkJobid10.Properties.ReadOnly = true;
                chkJobid11.Properties.ReadOnly = true;
                chkJobid12.Properties.ReadOnly = true;
                chkJobid13.Properties.ReadOnly = true;
                chkJobid14.Properties.ReadOnly = true;
                chkJobid15.Properties.ReadOnly = true;
                chkJobid16.Properties.ReadOnly = true;
                chkJobid17.Properties.ReadOnly = true;
                chkJobid18.Properties.ReadOnly = true;

                chkRecordtype11.Properties.ReadOnly = true;
                chkRecordtype12.Properties.ReadOnly = true;
                chkRecordtype13.Properties.ReadOnly = true;

                chkRecordtype21.Properties.ReadOnly = true;
                chkRecordtype22.Properties.ReadOnly = true;
                chkRecordtype23.Properties.ReadOnly = true;

                dateAttackdate.Properties.ReadOnly = true;
                dateDiagdate.Properties.ReadOnly = true;
                timeDiagdate.Properties.ReadOnly = true;
                Diedate.Properties.ReadOnly = true;
                lookUpEditorZymosis.Properties.ReadOnly = true;

                memoOtherDiag.Properties.ReadOnly = true;
                textCorrectName.Properties.ReadOnly = true;
                txtCancelReason.Properties.ReadOnly = true;
                lookUpEditorDept.Properties.ReadOnly = true;
                txtDoctortel.Properties.ReadOnly = true;

                lookUpEditorDoc.Properties.ReadOnly = true;
                //dateReportDate.Properties.ReadOnly = true;
                memoMemo.Properties.ReadOnly = true;
            }
            else
            {
                txtReportNo.Properties.ReadOnly = false;//开放编辑“报卡片编号”
                chkReportType1.Properties.ReadOnly = false;
                chkReportType2.Properties.ReadOnly = false;
                txtName.Properties.ReadOnly = false;
                txtParentname.Properties.ReadOnly = false;

                txtIdno.Properties.ReadOnly = false;
                chkSex1.Properties.ReadOnly = false;
                chkSex2.Properties.ReadOnly = false;
                dateBirth.Properties.ReadOnly = false;
                txtAge.Properties.ReadOnly = false;

                chkAgeUnit1.Properties.ReadOnly = false;
                chkAgeUnit2.Properties.ReadOnly = false;
                chkAgeUnit3.Properties.ReadOnly = false;
                txtOrganization.Properties.ReadOnly = false;
                txtOfficetel.Properties.ReadOnly = false;

                chkAddresstype1.Properties.ReadOnly = false;
                chkAddresstype2.Properties.ReadOnly = false;
                chkAddresstype3.Properties.ReadOnly = false;
                chkAddresstype4.Properties.ReadOnly = false;
                chkAddresstype5.Properties.ReadOnly = false;

                chkAddresstype6.Properties.ReadOnly = false;
                txtAddress.Properties.ReadOnly = false;

                chkJobid1.Properties.ReadOnly = false;
                chkJobid2.Properties.ReadOnly = false;
                chkJobid3.Properties.ReadOnly = false;
                chkJobid4.Properties.ReadOnly = false;
                chkJobid5.Properties.ReadOnly = false;
                chkJobid6.Properties.ReadOnly = false;
                chkJobid7.Properties.ReadOnly = false;
                chkJobid8.Properties.ReadOnly = false;
                chkJobid9.Properties.ReadOnly = false;
                chkJobid10.Properties.ReadOnly = false;
                chkJobid11.Properties.ReadOnly = false;
                chkJobid12.Properties.ReadOnly = false;
                chkJobid13.Properties.ReadOnly = false;
                chkJobid14.Properties.ReadOnly = false;
                chkJobid15.Properties.ReadOnly = false;
                chkJobid16.Properties.ReadOnly = false;
                chkJobid17.Properties.ReadOnly = false;
                chkJobid18.Properties.ReadOnly = false;

                chkRecordtype11.Properties.ReadOnly = false;
                chkRecordtype12.Properties.ReadOnly = false;
                chkRecordtype13.Properties.ReadOnly = false;

                chkRecordtype21.Properties.ReadOnly = false;
                chkRecordtype22.Properties.ReadOnly = false;
                chkRecordtype23.Properties.ReadOnly = false;

                dateAttackdate.Properties.ReadOnly = false;
                dateDiagdate.Properties.ReadOnly = false;
                timeDiagdate.Properties.ReadOnly = false;
                Diedate.Properties.ReadOnly = false;
                lookUpEditorZymosis.Properties.ReadOnly = false;

                memoOtherDiag.Properties.ReadOnly = false;
                textCorrectName.Properties.ReadOnly = false;
                txtCancelReason.Properties.ReadOnly = false;
                lookUpEditorDept.Properties.ReadOnly = false;
                txtDoctortel.Properties.ReadOnly = false;

                lookUpEditorDoc.Properties.ReadOnly = false;
                //dateReportDate.Properties.ReadOnly = false;
                memoMemo.Properties.ReadOnly = false;
            }

        }

        /// <summary>
        /// 将当前页面设置为不可编辑
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-24</date>
        /// <param name="boo">true:可用；false：不可用</param>
        /// </summary>
        public void EnableState(bool boo)
        {
            try
            {
                txtReportNo.Enabled = boo;
                chkReportType1.Enabled = boo;
                chkReportType2.Enabled = boo;
                txtName.Enabled = boo;
                txtParentname.Enabled = boo;

                txtIdno.Enabled = boo;
                chkSex1.Enabled = boo;
                chkSex2.Enabled = boo;
                //dateBirth.Properties.ReadOnly = !boo;
                dateBirth.Enabled = boo;
                txtAge.Enabled = boo;

                chkAgeUnit1.Enabled = boo;
                chkAgeUnit2.Enabled = boo;
                chkAgeUnit3.Enabled = boo;
                txtOrganization.Enabled = boo;
                txtOfficetel.Enabled = boo;

                chkAddresstype1.Enabled = boo;
                chkAddresstype2.Enabled = boo;
                chkAddresstype3.Enabled = boo;
                chkAddresstype4.Enabled = boo;
                chkAddresstype5.Enabled = boo;

                chkAddresstype6.Enabled = boo;
                txtAddress.Enabled = boo;

                chkJobid1.Enabled = boo;
                chkJobid2.Enabled = boo;
                chkJobid3.Enabled = boo;
                chkJobid4.Enabled = boo;
                chkJobid5.Enabled = boo;
                chkJobid6.Enabled = boo;
                chkJobid7.Enabled = boo;
                chkJobid8.Enabled = boo;
                chkJobid9.Enabled = boo;
                chkJobid10.Enabled = boo;
                chkJobid11.Enabled = boo;
                chkJobid12.Enabled = boo;
                chkJobid13.Enabled = boo;
                chkJobid14.Enabled = boo;
                chkJobid15.Enabled = boo;
                chkJobid16.Enabled = boo;
                chkJobid17.Enabled = boo;
                chkJobid18.Enabled = boo;

                chkRecordtype11.Enabled = boo;
                chkRecordtype12.Enabled = boo;
                chkRecordtype13.Enabled = boo;

                chkRecordtype21.Enabled = boo;
                chkRecordtype22.Enabled = boo;
                chkRecordtype23.Enabled = boo;

                //dateAttackdate.Properties.ReadOnly = !boo;
                //dateDiagdate.Properties.ReadOnly = !boo;
                //timeDiagdate.Properties.ReadOnly = !boo;
                //Diedate.Properties.ReadOnly = !boo;
                dateAttackdate.Enabled = boo;
                dateDiagdate.Enabled = boo;
                timeDiagdate.Enabled = boo;
                Diedate.Enabled = boo;
                lookUpEditorZymosis.Enabled = !boo;

                memoOtherDiag.Enabled = boo;
                textCorrectName.Enabled = boo;
                txtCancelReason.Enabled = boo;
                lookUpEditorDept.Enabled = boo;
                txtDoctortel.Enabled = boo;

                lookUpEditorDoc.Enabled = boo;
                memoMemo.Enabled = boo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 清空页面控件值
        /// </summary>
        /// edit by Yanqiao.Cai 2013-02-28
        /// 1、add try ... catch
        /// 2、附卡清空(如果存在附卡)
        public void ClearPage()
        {
            try
            {
                txtReportNo.Text = "";
                txtpatid.Text = "";
                chkReportType1.Checked = false;
                chkReportType2.Checked = false;
                txtName.Text = "";
                txtParentname.Text = "";

                txtIdno.Text = "";
                chkSex1.Checked = false;
                chkSex2.Checked = false;
                dateBirth.Text = string.Empty;
                txtAge.Text = "";

                chkAgeUnit1.Checked = false;
                chkAgeUnit2.Checked = false;
                chkAgeUnit3.Checked = false;
                txtOrganization.Text = "";
                txtOfficetel.Text = "";

                chkAddresstype1.Checked = false;
                chkAddresstype2.Checked = false;
                chkAddresstype3.Checked = false;
                chkAddresstype4.Checked = false;
                chkAddresstype5.Checked = false;

                chkAddresstype6.Checked = false;
                txtAddress.Text = "";

                chkJobid1.Checked = false;
                chkJobid2.Checked = false;
                chkJobid3.Checked = false;
                chkJobid4.Checked = false;
                chkJobid5.Checked = false;
                chkJobid6.Checked = false;
                chkJobid7.Checked = false;
                chkJobid8.Checked = false;
                chkJobid9.Checked = false;
                chkJobid10.Checked = false;
                chkJobid11.Checked = false;
                chkJobid12.Checked = false;
                chkJobid13.Checked = false;
                chkJobid14.Checked = false;
                chkJobid15.Checked = false;
                chkJobid16.Checked = false;
                chkJobid17.Checked = false;
                chkJobid18.Checked = false;

                chkRecordtype11.Checked = false;
                chkRecordtype12.Checked = false;
                chkRecordtype13.Checked = false;

                chkRecordtype21.Checked = false;
                chkRecordtype22.Checked = false;
                chkRecordtype23.Checked = false;

                dateAttackdate.Text = string.Empty;
                dateDiagdate.Text = string.Empty;
                timeDiagdate.Text = string.Empty;
                Diedate.Text = string.Empty;
                lookUpEditorZymosis.CodeValue = "";

                memoOtherDiag.Text = "";
                textCorrectName.Text = "";
                txtCancelReason.Text = "";
                lookUpEditorDept.CodeValue = "";
                txtDoctortel.Text = "";

                lookUpEditorDoc.CodeValue = "";
                //dateReportDate.DateTime = DateTime.MinValue;
                dateReportDate.DateTime = DateTime.Now;
                memoMemo.Text = "";

                //附卡清空 add by cyq 2013-03-12
                if (null != card_HepatitisB)
                {
                    card_HepatitisB.ClearPage();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void lookUpEditorDept_CodeValueChanged(object sender, EventArgs e)
        {
            InitDoctor(this.lookUpEditorDept.CodeValue);
        }

        /// <summary>
        /// 传染病病种事件
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-11</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lookUpEditorZymosis_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (null != m_ZymosisReportEntity)
                {
                    string configVal = DS_SqlService.GetConfigValueByKey("YiChangChuangRangBin");
                    if (configVal == "1" || string.IsNullOrEmpty(configVal))
                    {
                        InitAttachedCard(m_ZymosisReportEntity.ReportId <= 0 ? EditState.Add : EditState.Edit);
                    }
                    else
                    {

                        InitBiaoZhunFuKa();

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化副卡 非宜昌版本
        /// </summary>
        private void InitBiaoZhunFuKa()
        {
            string icdValue = lookUpEditorZymosis.CodeValue;

            DataRow drRight = null;
            foreach (DataRow dr in Bzlb.Rows)
            {
                if (dr["ICD"].ToString() == icdValue)
                {
                    drRight = dr;
                    break;
                }
            }
            if (drRight == null) return;
            fukatype = drRight["FUKATYPE"].ToString();
            switch (fukatype)
            {
                case "1":
                    HIVForm hIVForm = new HIVForm(m_ZymosisReportEntity.ReportId.ToString());
                    AddFuCadeControl(hIVForm);
                    break;
                case "2":
                    UCHBV uCHBV = new UCHBV(m_ZymosisReportEntity.ReportId.ToString());
                    AddFuCadeControl(uCHBV);
                    break;
                case "3":
                    UCH1N1 uCH1N1 = new UCH1N1(m_ZymosisReportEntity.ReportId.ToString());
                    AddFuCadeControl(uCH1N1);
                    break;
                case "5":
                    UCHFMD uCHFMD = new UCHFMD(m_ZymosisReportEntity.ReportId.ToString());
                    AddFuCadeControl(uCHFMD);
                    break;
                case "6":
                    UCAFP uCAFP = new UCAFP(m_ZymosisReportEntity.ReportId.ToString());
                    AddFuCadeControl(uCAFP);
                    break;
                case "7":
                    JRSYForm jRSYForm = new JRSYForm(m_ZymosisReportEntity.ReportId.ToString());
                    AddFuCadeControl(jRSYForm);
                    break;
                case "8":
                    SZDSYYYTForm sZDSYYYTForm = new SZDSYYYTForm(m_ZymosisReportEntity.ReportId.ToString());
                    AddFuCadeControl(sZDSYYYTForm);
                    break;

                default:
                    break;
            }
        }

        private void AddFuCadeControl(XtraUserControl devBaseForm)
        {
            int height = devBaseForm.Size.Height;
            int width = devBaseForm.Size.Width;
            panelControl_attachedCard.Controls.Clear();
            panelControl_attachedCard.Controls.Add(devBaseForm);
            devBaseForm.Dock = DockStyle.Fill;
            //panelControl_attachedCard.Height = devBaseForm.Height;
            panelControl_attachedCard.Height = height;
            panelControl_attachedCard.Width = width;
            this.Height = 800 + height;
            //this.Width = width;
            this.panelControl_attachedCard.Visible = true;

        }


        //************Add By wwj********************
        private void SetTextEditNoBorder()
        {
            foreach (Control control in this.Controls)
            {
                if (control is TextEdit && !(control is MemoEdit))
                {
                    TextEdit te = control as TextEdit;
                    te.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
                    te.Properties.AppearanceReadOnly.BackColor = Color.White;
                }
                else if (control is DateEdit)
                {
                    DateEdit de = control as DateEdit;
                    de.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
                }
                else if (control is LookUpEditor)
                {
                    LookUpEditor lue = control as LookUpEditor;
                    lue.BorderStyle = System.Windows.Forms.BorderStyle.None;
                }
            }
        }

        private void DrawUnderLine(Graphics g)
        {
            foreach (Control control in this.Controls)
            {
                if (control is TextEdit && !(control is MemoEdit) || control is DateEdit || control is LookUpEditor)
                {
                    g.DrawLine(Pens.Black,
                        new Point(control.Location.X, control.Location.Y + control.Height),
                        new Point(control.Location.X + control.Width, control.Location.Y + control.Height));
                }
            }
        }

        private void UCReportCard_Load(object sender, EventArgs e)
        {
            try
            {
                SetTextEditNoBorder();
                this.ActiveControl = txtReportNo;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 初始化附卡
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-11</date>
        private void InitAttachedCard(EditState editState)
        {
            try
            {
                if (IsShowAttachedCard())///显示附卡-乙肝
                {
                    card_HepatitisB = new AttachedCard_HepatitisB();
                    panelControl_attachedCard.Controls.Clear();
                    panelControl_attachedCard.Controls.Add(card_HepatitisB);
                    card_HepatitisB.Dock = DockStyle.Fill;
                    card_HepatitisB.SetCardTitle(this.lookUpEditorZymosis.Text + " 附卡");
                    card_HepatitisB.SetLineShapeWidth(panelControl_attachedCard.Width);
                    this.Height = 980;
                    if (editState == EditState.Edit)
                    {//加载 附卡-乙肝 数据
                        DataTable hbTable = DS_SqlService.GetHepatitisBByReportID(m_ZymosisReportEntity.ReportId, m_ZymosisReportEntity.Diagicd10);
                        if (null != hbTable && hbTable.Rows.Count > 0)
                        {
                            card_HepatitisB.FillUI(hbTable.Rows[0]);
                        }
                    }
                    this.panelControl_attachedCard.Visible = true;
                    card_HepatitisB.SetFocusControl();
                }
                else
                {
                    card_HepatitisB = null;
                    panelControl_attachedCard.Controls.Clear();
                    this.panelControl_attachedCard.Visible = false;
                    this.Height = 735;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 是否显示附卡(根据配置判断)
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-12</date>
        /// <returns></returns>
        private bool IsShowAttachedCard()
        {
            try
            {
                string config = DS_SqlService.GetConfigValueByKey("ZymosisReportAttachedCard");
                bool isShowCard = false;
                if (!string.IsNullOrEmpty(config))
                {
                    string[] cfg = config.Split(',');
                    if (!string.IsNullOrEmpty(this.lookUpEditorZymosis.CodeValue) && !string.IsNullOrEmpty(this.lookUpEditorZymosis.Text) && cfg.Contains(this.lookUpEditorZymosis.Text))
                    {
                        isShowCard = true;
                    }
                }
                return isShowCard;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void UCReportCard_Paint(object sender, PaintEventArgs e)
        {
            DrawUnderLine(e.Graphics);
        }

        private void labelControl1_Click(object sender, EventArgs e)
        {

        }
        //*******************************************

        /// <summary>
        /// 复选框回车事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-10-23</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_KeyPress(object sender, KeyPressEventArgs e)
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
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 设置焦点
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-13</date>
        public void SetFocusControl()
        {
            try
            {
                txtReportNo.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 文本框获取焦点颜色变化控制 by cyq 2012-10-22
        private void Dev_Enter(object sender, EventArgs e)
        {
            DS_Common.setBackColor(sender, true);
        }
        private void Dev_Leave(object sender, EventArgs e)
        {
            DS_Common.setBackColor(sender, false);
        }
        #endregion

        #region 打印

        private int m_X = 30;
        private int m_Y = 90;
        private StringFormat m_SFCenter = new StringFormat();
        private StringFormat m_SFLeft = new StringFormat();
        public void PrintCard(Graphics g, Control control)
        {
            if (control == null)
            {
                control = this;

                m_SFCenter.Alignment = StringAlignment.Center;
                m_SFCenter.LineAlignment = StringAlignment.Center;

                m_SFLeft.Alignment = StringAlignment.Near;
                m_SFCenter.LineAlignment = StringAlignment.Near;
            }

            foreach (Control subControl in control.Controls)
            {
                if (subControl.Controls.Count > 0 && !(subControl is TextEdit))
                {
                    PrintCard(g, subControl);
                }
                else
                {
                    Point p = GetControlActualLocation(subControl);

                    if (subControl is LabelControl || subControl is Label)
                    {
                        g.DrawString(subControl.Text.Replace("*:", ":").Replace("*：", "："), subControl.Font, Brushes.Black, p);
                    }
                    else if (subControl is TextEdit || subControl is TextBox)
                    {
                        bool isCenter = false;
                        if (subControl is TextEdit)
                        {
                            if (((TextEdit)subControl).Properties.Appearance.TextOptions.HAlignment == DevExpress.Utils.HorzAlignment.Center)
                            {
                                isCenter = true;
                            }
                        }


                        if (!(subControl is MemoEdit))
                        {
                            if (isCenter)
                            {
                                g.DrawString(subControl.Text, subControl.Font, Brushes.Black,
                                    new Rectangle(p.X + 4, p.Y + 2, subControl.Size.Width, subControl.Size.Height), m_SFCenter);
                            }
                            else
                            {
                                g.DrawString(subControl.Text, subControl.Font, Brushes.Black, new Point(p.X + 4, p.Y + 2));
                            }
                            g.DrawLine(Pens.Black, p.X + 2, p.Y + subControl.Height, p.X + 2 + subControl.Size.Width, p.Y + subControl.Height);
                        }
                        else
                        {
                            g.DrawString(subControl.Text, subControl.Font, Brushes.Black,
                                    new Rectangle(p.X, p.Y, subControl.Size.Width + 20, subControl.Size.Height), m_SFLeft);
                        }
                    }
                    else if (subControl is CheckEdit || subControl is CheckBox)
                    {
                        bool isChecked = false;

                        if (subControl is CheckEdit) isChecked = ((CheckEdit)subControl).Checked;
                        else if (subControl is CheckBox) isChecked = ((CheckBox)subControl).Checked;

                        g.DrawRectangle(Pens.Black, new Rectangle(p.X + 5, p.Y + 5, 10, 10));
                        if (isChecked)
                        {
                            g.DrawString("√", subControl.Font, Brushes.Black, new PointF(p.X + 3, p.Y + 2));
                        }
                        g.DrawString(subControl.Text, subControl.Font, Brushes.Black, new Point(p.X + 15, p.Y + 2));

                    }
                    else if (subControl is Panel && subControl.Visible == true)
                    {
                        g.DrawLine(Pens.Black, p.X, p.Y, p.X + subControl.Size.Width, p.Y);
                        g.DrawLine(Pens.Black, p.X, p.Y + subControl.Size.Height, p.X + subControl.Size.Width, p.Y + subControl.Size.Height);
                        g.DrawLine(Pens.Black, p.X, p.Y, p.X, p.Y + subControl.Size.Height);
                        g.DrawLine(Pens.Black, p.X + subControl.Size.Width, p.Y, p.X + subControl.Size.Width, p.Y + subControl.Size.Height);
                    }
                }
            }
        }

        private Point GetControlActualLocation(Control control)
        {
            Point p = new Point();
            while (true)
            {
                if (null != control && null != control.Parent && null != control.Parent.Parent && control.Parent.Parent.Name == "panelControl_attachedCard")
                {
                    Point grandParent = control.Parent.Parent.Location;
                    if (control.Text.Contains("附卡"))
                    {
                        p = new Point(grandParent.X + control.Parent.Parent.Size.Width / 2 - 40 + p.X + control.Location.X, grandParent.Y + p.Y + control.Location.Y);
                    }
                    else
                    {
                        p = new Point(grandParent.X + p.X + control.Location.X, grandParent.Y + p.Y + control.Location.Y);
                    }
                }
                else
                {
                    p = new Point(p.X + control.Location.X, p.Y + control.Location.Y);
                }
                control = control.Parent;
                if (control is XtraUserControl) break;
            }
            return new Point(p.X + m_X, p.Y + m_Y);
        }

        #endregion

    }
}
