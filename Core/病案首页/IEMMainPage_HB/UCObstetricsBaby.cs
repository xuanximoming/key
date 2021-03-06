﻿using DevExpress.XtraEditors;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Eop;
using DrectSoft.Common.Library;
using DrectSoft.DSSqlHelper;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Service;
using DrectSoft.Wordbook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DrectSoft.Core.IEMMainPage
{
    /// <summary>
    /// 产科产妇婴儿情况编辑界面
    /// </summary>
    public partial class UCObstetricsBaby : UserControl
    {
        IDataAccess m_SqlHelper;
        IDrectSoftLog m_Logger;
        private IemMainPageInfo m_IemInfo;
        DataTable m_showTable;
        public bool editFlag = false;  //add by cyq 2012-12-06 病案室人员编辑首页(状态改为归档)
        private Inpatient CurrentInpatient;//add by ywk 
        private DataTable dtEdiMidwifery = null;
        private string editState = "";  // add by jxh 保存事件中的状态判别
        /// <summary>
        /// 病案首页产科产妇婴儿信息
        /// </summary>
        public IemMainPageInfo IemInfo
        {
            get
            {
                m_IemInfo = new IemMainPageInfo();
                GetUI();
                return m_IemInfo;
            }
        }

        private IEmrHost m_App;

        /// <summary>
        /// 初始化接产者下拉框
        /// Add by xlb 20013-07-16
        /// </summary>
        /// <param name="lookUpEditor"></param>
        /// <param name="lookUpWindows"></param>
        void InitlpEdiMidwifery(ref LookUpEditor lookUpEditor, LookUpWindow lookUpWindows)
        {
            try
            {
                if (string.IsNullOrEmpty(DS_SqlHelper.ConnectionString))
                {
                    DS_SqlHelper.CreateSqlHelper();
                }
                lookUpEditor.Kind = WordbookKind.Sql;
                lookUpEditor.ListWindow = lookUpWindows;
                DataTable dt = DS_SqlHelper.ExecuteDataTable(@"select id,name,py,wb from users where category in ('400','401') and valid='1' and grade in ('2000','2001','2002','2003','') and deptid=@deptid", new SqlParameter[] { new SqlParameter("@deptid", DrectSoft.Common.DS_Common.currentUser.CurrentDeptId) }, CommandType.Text);
                dt.Columns["ID"].Caption = "编码";
                dt.Columns["NAME"].Caption = "姓名";
                Dictionary<string, Int32> colWidth = new Dictionary<string, Int32>();
                colWidth.Add("ID", 85);
                colWidth.Add("NAME", 100);
                SqlWordbook sqlWord = new SqlWordbook("JCZ", dt, "ID", "NAME", colWidth, "ID//NAME//PY//WB");
                lookUpEditor.SqlWordbook = sqlWord;
                dtEdiMidwifery = dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Methods

        /// <summary>
        /// 构造函数
        /// </summary>
        public UCObstetricsBaby()
        {
            try
            {
                InitializeComponent();

                m_SqlHelper = DataAccessFactory.DefaultDataAccess;
                InitlpEdiMidwifery(ref lpEdiMidwifery, new LookUpWindow());
                RegisterEvent();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 注册事件
        /// Add by xlb 2013-07-17
        /// </summary>
        void RegisterEvent()
        {
            try
            {
                btnEdit.Click += new EventHandler(btnEdit_Click);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// GET UI
        /// </summary>
        private void GetUI()
        {
            try
            {
                #region
                //胎次
                if (chkTC1.Checked == true)
                    m_IemInfo.IemObstetricsBaby.TC = "1";
                else if (chkTC2.Checked == true)
                    m_IemInfo.IemObstetricsBaby.TC = "2";
                else if (chkTC3.Checked == true)
                    m_IemInfo.IemObstetricsBaby.TC = "3";
                else
                    m_IemInfo.IemObstetricsBaby.TC = "";

                //胎别
                if (chkTB1.Checked == true)
                    m_IemInfo.IemObstetricsBaby.TB = "1";
                else if (chkTB2.Checked == true)
                    m_IemInfo.IemObstetricsBaby.TB = "2";
                else if (chkTB3.Checked == true)
                    m_IemInfo.IemObstetricsBaby.TB = "3";
                else
                    m_IemInfo.IemObstetricsBaby.TB = "";

                //产次
                if (chkCC1.Checked == true)
                    m_IemInfo.IemObstetricsBaby.CC = "1";
                else if (chkCC2.Checked == true)
                    m_IemInfo.IemObstetricsBaby.CC = "2";
                else if (chkCC3.Checked == true)
                    m_IemInfo.IemObstetricsBaby.CC = "3";
                else
                    m_IemInfo.IemObstetricsBaby.CC = "";

                //产妇会阴破裂度
                if (chkCFHYPLD1.Checked == true)
                    m_IemInfo.IemObstetricsBaby.CFHYPLD = "1";
                else if (chkCFHYPLD2.Checked == true)
                    m_IemInfo.IemObstetricsBaby.CFHYPLD = "2";
                else if (chkCFHYPLD3.Checked == true)
                    m_IemInfo.IemObstetricsBaby.CFHYPLD = "3";
                else
                    m_IemInfo.IemObstetricsBaby.CFHYPLD = "";

                //性别
                if (chkSex1.Checked == true)
                    m_IemInfo.IemObstetricsBaby.Sex = "1";
                else if (chkSex2.Checked == true)
                    m_IemInfo.IemObstetricsBaby.Sex = "2";
                else
                    m_IemInfo.IemObstetricsBaby.Sex = "";

                //阿帕加评分
                m_IemInfo.IemObstetricsBaby.APJ = txtAPJPF.Text;



                //身长
                m_IemInfo.IemObstetricsBaby.Heigh = txtheigh.Text;

                //体重
                m_IemInfo.IemObstetricsBaby.Weight = txtweight.Text;

                //出生日期
                m_IemInfo.IemObstetricsBaby.BithDay = BithDayDate.DateTime.ToString("yyyy-MM-dd") + " " + BithDayTime.Time.ToString("HH:mm:ss");

                m_IemInfo.IemObstetricsBaby.Midwifery = lpEdiMidwifery.CodeValue;

                //产出情况
                if (chkCCQK1.Checked == true)
                    m_IemInfo.IemObstetricsBaby.CCQK = "1";
                else if (chkCCQK2.Checked == true)
                    m_IemInfo.IemObstetricsBaby.CCQK = "2";
                else if (chkCCQK3.Checked == true)
                    m_IemInfo.IemObstetricsBaby.CCQK = "3";
                else if (chkCCQK4.Checked == true)
                    m_IemInfo.IemObstetricsBaby.CCQK = "4";
                else
                    m_IemInfo.IemObstetricsBaby.CCQK = "";

                //出院情况
                if (chkCYQK1.Checked == true)
                    m_IemInfo.IemObstetricsBaby.CYQK = "1";
                else if (chkCYQK2.Checked == true)
                    m_IemInfo.IemObstetricsBaby.CYQK = "2";
                else if (chkCYQK3.Checked == true)
                    m_IemInfo.IemObstetricsBaby.CYQK = "3";
                else
                    m_IemInfo.IemObstetricsBaby.CYQK = "";

                //分娩方式
                if (chkFMFS1.Checked == true)
                    m_IemInfo.IemObstetricsBaby.FMFS = "1";
                else if (chkFMFS2.Checked == true)
                    m_IemInfo.IemObstetricsBaby.FMFS = "2";
                else if (chkFMFS3.Checked == true)
                    m_IemInfo.IemObstetricsBaby.FMFS = "3";
                else if (chkFMFS4.Checked == true)
                    m_IemInfo.IemObstetricsBaby.FMFS = "4";
                else if (chkFMFS5.Checked == true)
                    m_IemInfo.IemObstetricsBaby.FMFS = "5";
                else if (chkFMFS6.Checked == true)
                    m_IemInfo.IemObstetricsBaby.FMFS = "6";
                else
                    m_IemInfo.IemObstetricsBaby.FMFS = "";
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 填充界面值
        /// </summary>
        /// <param name="info"></param>
        /// <param name="app"></param>
        public void FillUI(IemMainPageInfo info, IEmrHost app)
        {
            try
            {
                m_App = app;
                m_IemInfo = info;

                //(new FillUIDelegate(FillUIInner)).BeginInvoke(null, null);
                FillUIInner(0);
                this.UpdateShowTable();
                this.gridControlBabyInfo.DataSource = m_showTable;
                gridControlBabyInfo.EndUpdate();
                m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewBabyInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        delegate void FillUIDelegate();

        private void FillUIInner(int rowindex)
        {
            try
            {
                #region
                if (m_IemInfo.IemBasicInfo.Iem_Mainpage_NO == "")
                {
                    //to do 病患基本信息
                }
                else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows.Count <= rowindex)
                {
                    chkTC1.Checked = false;
                    chkTC2.Checked = false;
                    chkTC3.Checked = false;
                    chkTB1.Checked = false;
                    chkTB2.Checked = false;
                    chkTB3.Checked = false;
                    chkCC1.Checked = false;
                    chkCC2.Checked = false;
                    chkCC3.Checked = false;
                    chkCFHYPLD1.Checked = false;
                    chkCFHYPLD2.Checked = false;
                    chkCFHYPLD3.Checked = false;
                    chkSex1.Checked = false;
                    chkSex2.Checked = false;
                    //txtMidwifery.Text = "";
                    lpEdiMidwifery.CodeValue = "";
                    txtAPJPF.Text = "";
                    txtheigh.Text = "";
                    txtweight.Text = "";
                    BithDayDate.Text = "";
                    BithDayTime.Text = "";
                    chkCCQK1.Checked = false;
                    chkCCQK2.Checked = false;
                    chkCCQK3.Checked = false;
                    chkCCQK4.Checked = false;
                    chkCYQK1.Checked = false;
                    chkCYQK2.Checked = false;
                    chkCYQK3.Checked = false;
                    chkFMFS1.Checked = false;
                    chkFMFS2.Checked = false;
                    chkFMFS3.Checked = false;
                    chkFMFS4.Checked = false;
                    chkFMFS5.Checked = false;
                    chkFMFS6.Checked = false;
                    //return;
                }
                else
                {
                    //胎次
                    if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["TC"].ToString() == "1")
                        chkTC1.Checked = true;
                    else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["TC"].ToString() == "2")
                        chkTC2.Checked = true;
                    else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["TC"].ToString() == "3")
                        chkTC3.Checked = true;
                    else
                    {
                        chkTC1.Checked = false;
                        chkTC2.Checked = false;
                        chkTC3.Checked = false;
                    }



                    //胎别
                    if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["TB"].ToString() == "1")
                        chkTB1.Checked = true;
                    else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["TB"].ToString() == "2")
                        chkTB2.Checked = true;
                    else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["TB"].ToString() == "3")
                        chkTB3.Checked = true;
                    else
                    {
                        chkTB1.Checked = false;
                        chkTB2.Checked = false;
                        chkTB3.Checked = false;
                    }


                    //产次
                    if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["CC"].ToString() == "1")
                        chkCC1.Checked = true;
                    else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["CC"].ToString() == "2")
                        chkCC2.Checked = true;
                    else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["CC"].ToString() == "3")
                        chkCC3.Checked = true;
                    else
                    {
                        chkCC1.Checked = false;
                        chkCC2.Checked = false;
                        chkCC3.Checked = false;
                    }


                    //产妇会阴破裂度
                    if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["CFHYPLD"].ToString() == "1")
                        chkCFHYPLD1.Checked = true;
                    else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["CFHYPLD"].ToString() == "2")
                        chkCFHYPLD2.Checked = true;
                    else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["CFHYPLD"].ToString() == "3")
                        chkCFHYPLD3.Checked = true;
                    else
                    {
                        chkCFHYPLD1.Checked = false;
                        chkCFHYPLD2.Checked = false;
                        chkCFHYPLD3.Checked = false;
                    }


                    //性别
                    if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["SEX"].ToString() == "1")
                        chkSex1.Checked = true;
                    else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["SEX"].ToString() == "2")
                        chkSex2.Checked = true;
                    else
                    {
                        chkSex1.Checked = false;
                        chkSex2.Checked = false;
                    }

                    //接产者 add by ywk 2012年5月14日 14:42:04
                    lpEdiMidwifery.CodeValue = m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["MIDWIFERY"].ToString();

                    //阿帕加评分
                    txtAPJPF.Text = m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["APJ"].ToString();

                    //身长
                    txtheigh.Text = m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["HEIGH"].ToString();

                    //体重
                    txtweight.Text = m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["WEIGHT"].ToString();

                    //出生日期
                    //m_IemInfo.IemObstetricsBaby.BithDay = BithDayDate.DateTime.ToString("yyyy-MM-dd") + " " + BithDayTime.Time.ToString("HH:mm:ss");
                    if (!String.IsNullOrEmpty(m_IemInfo.IemObstetricsBaby.BithDay))
                    {
                        try
                        {
                            BithDayDate.Text = (DateTime.Parse(m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["BITHDAY"].ToString())).ToString("yyyy-MM-dd");
                            BithDayTime.Text = (DateTime.Parse(m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["BITHDAY"].ToString())).ToString("HH:mm:ss");
                        }
                        catch
                        { }
                    }


                    //产出情况
                    if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["CCQK"].ToString() == "1")
                        chkCCQK1.Checked = true;
                    else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["CCQK"].ToString() == "2")
                        chkCCQK2.Checked = true;
                    else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["CCQK"].ToString() == "3")
                        chkCCQK3.Checked = true;
                    else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["CCQK"].ToString() == "4")
                        chkCCQK4.Checked = true;
                    else
                    {
                        chkCCQK1.Checked = false;
                        chkCCQK2.Checked = false;
                        chkCCQK3.Checked = false;
                        chkCCQK4.Checked = false;
                    }


                    //出院情况
                    if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["CYQK"].ToString() == "1")
                        chkCYQK1.Checked = true;
                    else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["CYQK"].ToString() == "2")
                        chkCYQK2.Checked = true;
                    else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["CYQK"].ToString() == "3")
                        chkCYQK3.Checked = true;
                    else
                    {
                        chkCYQK1.Checked = false;
                        chkCYQK2.Checked = false;
                        chkCYQK3.Checked = false;
                    }


                    //分娩方式
                    if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["FMFS"].ToString() == "1")
                        chkFMFS1.Checked = true;
                    else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["FMFS"].ToString() == "2")
                        chkFMFS2.Checked = true;
                    else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["FMFS"].ToString() == "3")
                        chkFMFS3.Checked = true;
                    else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["FMFS"].ToString() == "4")
                        chkFMFS4.Checked = true;
                    else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["FMFS"].ToString() == "5")
                        chkFMFS5.Checked = true;
                    else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["FMFS"].ToString() == "6")
                        chkFMFS6.Checked = true;
                    else
                    {
                        chkFMFS1.Checked = false;
                        chkFMFS2.Checked = false;
                        chkFMFS3.Checked = false;
                        chkFMFS4.Checked = false;
                        chkFMFS5.Checked = false;
                        chkFMFS6.Checked = false;
                    }

                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void UpdateShowTable()
        {
            try
            {

                m_showTable = new DataTable();

                if (m_IemInfo != null
                    && m_IemInfo.IemObstetricsBaby != null
                    && m_IemInfo.IemObstetricsBaby.OutBabyTable != null)
                {
                    m_showTable = m_IemInfo.IemObstetricsBaby.OutBabyTable.Clone();
                }
                //if (!m_showTable.Columns.Contains("SEX"))
                //{
                //    m_showTable.Columns.Add("SEX");
                //}
                //if (!m_showTable.Columns.Contains("HEIGH"))
                //{
                //    m_showTable.Columns.Add("HEIGH");
                //}
                //if (m_showTable.Columns.Contains("WEIGHT"))
                //{
                //    m_showTable.Columns.Add("WEIGHT");
                //}
                //if (m_showTable.Columns.Contains("BITHDAY"))
                //{
                //    m_showTable.Columns.Add("BITHDAY");
                //}
                //if (m_showTable.Columns.Add("FMFS"))
                //{
                //    m_showTable.Columns.Add("FMFS");
                //}
                //if (m_showTable.Columns.Contains("CCQK"))
                //{
                //    m_showTable.Columns.Add("CCQK");
                //}
                //if (m_showTable.Columns.Contains("APJ"))
                //m_showTable.Columns.Add("APJ");
                //m_showTable.Columns.Add("MIDWIFERY");
                //m_showTable.Columns.Add("CYQK");
                for (int i = 0; i < m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows.Count; i++)
                {
                    DataRow dr = m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[i];
                    DataRow row = m_showTable.NewRow();
                    //胎次选择情况存入数据库中已代码形式界面展示转成对应名
                    switch (dr["TC"].ToString())
                    {
                        case "1":
                            row["TC"] = "单";
                            break;
                        case "2":
                            row["TC"] = "双";
                            break;
                        case "3":
                            row["TC"] = "多";
                            break;
                        default:
                            row["TC"] = "";
                            break;
                    }
                    //产次情况
                    switch (dr["CC"].ToString())
                    {
                        case "1":
                            row["CC"] = "单";
                            break;
                        case "2":
                            row["CC"] = "双";
                            break;
                        case "3":
                            row["CC"] = "多";
                            break;
                        default:
                            row["CC"] = "";
                            break;
                    }
                    //胎别情况
                    switch (dr["TB"].ToString())
                    {
                        case "1":
                            row["TB"] = "单";
                            break;
                        case "2":
                            row["TB"] = "双";
                            break;
                        case "3":
                            row["TB"] = "多";
                            break;
                        default:
                            row["TB"] = "";
                            break;
                    }
                    //产妇会阴破裂度
                    switch (dr["CFHYPLD"].ToString())
                    {
                        case "1":
                            row["CFHYPLD"] = "I";
                            break;
                        case "2":
                            row["CFHYPLD"] = "II";
                            break;
                        case "3":
                            row["CFHYPLD"] = "III";
                            break;
                        default:
                            row["CFHYPLD"] = "";
                            break;
                    }
                    //性别
                    switch (dr["SEX"].ToString())
                    {
                        case "1":
                            row["SEX"] = "男";
                            break;
                        case "2":
                            row["SEX"] = "女";
                            break;
                        default:
                            row["SEX"] = "";
                            break;
                    }
                    //分娩方式
                    switch (dr["FMFS"].ToString())
                    {
                        case "1":
                            row["FMFS"] = "自然";
                            break;
                        case "2":
                            row["FMFS"] = "侧+吸";
                            break;
                        case "3":
                            row["FMFS"] = "产钳";
                            break;
                        case "4":
                            row["FMFS"] = "臂牵引";
                            break;
                        case "5":
                            row["FMFS"] = "剖宫";
                            break;
                        case "6":
                            row["FMFS"] = "其他";
                            break;
                        default:
                            row["FMFS"] = "";
                            break;
                    }
                    //产出情况
                    switch (dr["CCQK"].ToString())
                    {
                        case "1":
                            row["CCQK"] = "活产";
                            break;
                        case "2":
                            row["CCQK"] = "死产";
                            break;
                        case "3":
                            row["CCQK"] = "死胎";
                            break;
                        case "4":
                            row["CCQK"] = "畸形";
                            break;
                        default:
                            row["CCQK"] = "";
                            break;
                    }
                    //出院情况
                    switch (dr["CYQK"].ToString())
                    {
                        case "1":
                            row["CYQK"] = "正常";
                            break;
                        case "2":
                            row["CYQK"] = "有病";
                            break;
                        case "3":
                            row["CYQK"] = "交叉感染";
                            break;
                        default:
                            row["CYQK"] = "";
                            break;
                    }
                    //身高
                    row["HEIGH"] = dr["HEIGH"];
                    //体重
                    row["WEIGHT"] = dr["WEIGHT"];
                    //出生日期
                    row["BITHDAY"] = dr["BITHDAY"];
                    //阿帕奇得分
                    row["APJ"] = dr["APJ"];
                    //通过接产者代码遍历转换成对应姓名
                    var rowInfo = (from dataRow in dtEdiMidwifery.AsEnumerable()
                                   where dataRow.Field<string>("ID").Equals(dr["MIDWIFERY"].ToString())
                                   select dataRow).FirstOrDefault();
                    if (rowInfo != null)
                    {
                        row["MIDWIFERY"] = rowInfo["NAME"].ToString();
                    }
                    else
                    {
                        row["MIDWIFERY"] = "";
                    }
                    m_showTable.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取打印板块的产妇婴儿信息
        /// </summary>
        /// <returns></returns>
        public Iem_MainPage_ObstetricsBaby GetPrintObsBaby()
        {
            try
            {
                Iem_MainPage_ObstetricsBaby _Iem_MainPage_Baby = new Iem_MainPage_ObstetricsBaby();
                if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows.Count == 0)
                {
                    return _Iem_MainPage_Baby;
                }
                _Iem_MainPage_Baby.IEM_MainPage_NO = m_IemInfo.IemObstetricsBaby.IEM_MainPage_NO;
                _Iem_MainPage_Baby.IEM_MainPage_ObstetricsBabyID = m_IemInfo.IemObstetricsBaby.IEM_MainPage_ObstetricsBabyID;
                _Iem_MainPage_Baby.TC = m_IemInfo.IemObstetricsBaby.TC;
                _Iem_MainPage_Baby.CC = m_IemInfo.IemObstetricsBaby.CC;
                _Iem_MainPage_Baby.TB = m_IemInfo.IemObstetricsBaby.TB;

                _Iem_MainPage_Baby.CFHYPLD = m_IemInfo.IemObstetricsBaby.CFHYPLD;
                _Iem_MainPage_Baby.Midwifery = m_IemInfo.IemObstetricsBaby.Midwifery;
                _Iem_MainPage_Baby.Sex = m_IemInfo.IemObstetricsBaby.Sex;
                _Iem_MainPage_Baby.APJ = m_IemInfo.IemObstetricsBaby.APJ;
                _Iem_MainPage_Baby.Heigh = m_IemInfo.IemObstetricsBaby.Heigh;

                _Iem_MainPage_Baby.Weight = m_IemInfo.IemObstetricsBaby.Weight;
                _Iem_MainPage_Baby.CCQK = m_IemInfo.IemObstetricsBaby.CCQK;
                _Iem_MainPage_Baby.BithDay = m_IemInfo.IemObstetricsBaby.BithDay;
                _Iem_MainPage_Baby.FMFS = m_IemInfo.IemObstetricsBaby.FMFS;
                _Iem_MainPage_Baby.CYQK = m_IemInfo.IemObstetricsBaby.CYQK;
                return _Iem_MainPage_Baby;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据名称返回控件
        /// </summary>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        private CheckEdit GetCheckEdit(string ControlName)
        {
            try
            {
                foreach (Control control in this.Controls)
                {
                    if (control.Name == ControlName)
                    {
                        return (CheckEdit)control;
                    }
                }
                foreach (Control control in this.panelControl1.Controls)
                {
                    if (control.Name == ControlName)
                    {
                        return (CheckEdit)control;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// 保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_OK_Click(object sender, EventArgs e)
        {
            try
            {
                if (editState == "1")
                {
                    if (!string.IsNullOrEmpty(CheckData()))
                    {
                        MessageBox.Show(CheckData());
                        return;
                    }
                    DataRow dr = m_IemInfo.IemObstetricsBaby.OutBabyTable.NewRow();
                    dr = AddOrModifyDataRow(dr);
                    m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows.Add(dr);
                    this.GetUI();
                    this.UpdateShowTable();
                    this.gridControlBabyInfo.DataSource = m_showTable;
                    gridControlBabyInfo.EndUpdate();
                    m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewBabyInfo);
                }
                if (editState == "2")
                {
                    if (!string.IsNullOrEmpty(CheckData()))
                    {
                        MessageBox.Show(CheckData());
                        return;
                    }
                    int index = gridViewBabyInfo.FocusedRowHandle;
                    if (index < 0)
                    {
                        MessageBox.Show("请选择一条记录");
                        return;
                    }
                    DataRow dataRow = m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[index];
                    dataRow = AddOrModifyDataRow(dataRow);
                    this.UpdateShowTable();
                    this.gridControlBabyInfo.DataSource = m_showTable;
                }
                //if (editState == "3")
                //{                  
                //    MessageBox.Show("没有可删除的记录");
                //    return;                   
                //}
                //设置当前病人(修复m_App病人丢失问题)
                if (null == m_App || null == m_App.CurrentPatientInfo || m_App.CurrentPatientInfo.NoOfFirstPage.ToString() != m_IemInfo.IemBasicInfo.NoOfInpat)
                {
                    CurrentInpatient = DS_SqlService.GetPatientInfo(m_IemInfo.IemBasicInfo.NoOfInpat);
                }
                else
                {
                    CurrentInpatient = m_App.CurrentPatientInfo;
                }

                //edit by 2012-12-20 张业兴 关闭弹出框只关闭提示框
                //((ShowUC)this.Parent).Close(true, m_IemInfo);
                //点击确认按钮就将数据更新到数据库
                //CurrentInpatient = m_App.CurrentPatientInfo;
                if (null != CurrentInpatient)
                {
                    CurrentInpatient.ReInitializeAllProperties();
                }
                IemMainPageManger manger = new IemMainPageManger(m_App, CurrentInpatient);
                manger.SaveData(m_IemInfo);

                foreach (Control control in this.Controls)    //add by jxh  保存完继续灰掉
                {
                    control.Enabled = false;
                }
                gridControlBabyInfo.Enabled = true;
                panelControl2.Enabled = true;
                btn_OK.Enabled = true;
                btn_Close.Enabled = true;

                //add by cyq 2012-12-05 病案室人员编辑后状态改为已归档
                if (editFlag)
                {
                    DS_BaseService.SetRecordsRebacked(int.Parse(CurrentInpatient.NoOfFirstPage.ToString().Trim()));
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, EventArgs e)
        {
            try
            {
                ((ShowUC)this.Parent).Close(false, null);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 校验方法
        /// </summary>
        /// <returns></returns>
        private string CheckData()
        {
            try
            {
                if (!chkTC1.Checked && !chkTC2.Checked && !chkTC3.Checked)
                {
                    return "请选择胎次";
                }
                else if (!chkCC1.Checked && !chkCC2.Checked && !chkCC3.Checked)
                {
                    return "请选择产次";
                }
                else if (!chkTB1.Checked && !chkTB2.Checked && !chkTB3.Checked)
                {
                    return "请选择胎别";
                }
                //剖腹產不存在会阴破裂度 edit by ywk 2013年9月16日 11:30:44
                //else if (!chkCFHYPLD1.Checked && !chkCFHYPLD2.Checked && !chkCFHYPLD3.Checked)
                //{
                //    return "请选择会阴破裂度";
                //}
                else if (!chkSex1.Checked && !chkSex2.Checked)
                {
                    return "请选择性别";
                }
                else if (string.IsNullOrEmpty(txtAPJPF.Text.Trim()))
                {
                    return "请输入阿帕加评分";
                }
                else if (string.IsNullOrEmpty(txtheigh.Text.Trim()))
                {
                    return "请输入身长";
                }
                else if (string.IsNullOrEmpty(txtweight.Text.Trim()))
                {
                    return "请输入体重";
                }
                else if (string.IsNullOrEmpty(BithDayDate.Text.Trim()))
                {
                    return "请选择出生时间";
                }
                else if (string.IsNullOrEmpty(lpEdiMidwifery.CodeValue))
                {
                    return "请选择接产者";
                }
                else if (!chkCCQK1.Checked && !chkCCQK2.Checked && !chkCCQK3.Checked && !chkCCQK4.Checked)
                {
                    return "请选择产出情况";
                }
                else if (!chkCYQK1.Checked && !chkCYQK2.Checked && !chkCYQK3.Checked)
                {
                    return "请选择出院情况";
                }
                else if (!chkFMFS1.Checked && !chkFMFS2.Checked && !chkFMFS3.Checked && !chkFMFS4.Checked && !chkFMFS5.Checked && !chkFMFS6.Checked)
                {
                    return "请选择分娩情况";
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 编辑事件
        /// Add by xlb 2013-07-16
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region  注销 by jxh  原因：编辑事件移至确定按钮事件中
        //void btnEdit_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        int index = gridViewBabyInfo.FocusedRowHandle;
        //        if (index < 0)
        //        {
        //            MessageBox.Show("请选择一条记录");
        //            return;
        //        }
        //        DataRow dataRow = m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[index];
        //        dataRow = AddOrModifyDataRow(dataRow);
        //        this.UpdateShowTable();
        //        this.gridControlBabyInfo.DataSource = m_showTable;               
        //    }
        //    catch (Exception ex)
        //    {
        //        MyMessageBox.Show(1, ex);
        //    }
        //}
        #endregion


        /// <summary>
        /// 增加新行或修改行信息
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private DataRow AddOrModifyDataRow(DataRow dr)
        {
            try
            {
                if (chkTC1.Checked == true)
                {
                    dr["TC"] = "1";
                }
                else if (chkTC2.Checked == true)
                {
                    dr["TC"] = "2";
                }
                else if (chkTC3.Checked == true)
                {
                    dr["TC"] = "3";
                }
                else
                {
                    dr["TC"] = "";
                }
                //胎别
                if (chkTB1.Checked == true)
                {
                    dr["TB"] = "1";
                }
                else if (chkTB2.Checked == true)
                {
                    dr["TB"] = "2";
                }
                else if (chkTB3.Checked == true)
                {
                    dr["TB"] = "3";
                }
                else
                {
                    dr["TB"] = "";
                }
                //产次
                if (chkCC1.Checked == true)
                {
                    dr["CC"] = "1";
                }
                else if (chkCC2.Checked == true)
                {
                    dr["CC"] = "2";
                }
                else if (chkCC3.Checked == true)
                {
                    dr["CC"] = "3";
                }
                else
                {
                    dr["CC"] = "";
                }
                //产妇会阴破裂度
                if (chkCFHYPLD1.Checked == true)
                {
                    dr["CFHYPLD"] = "1";
                }
                else if (chkCFHYPLD2.Checked == true)
                {
                    dr["CFHYPLD"] = "2";
                }
                else if (chkCFHYPLD3.Checked == true)
                {
                    dr["CFHYPLD"] = "3";
                }
                else
                {
                    dr["CFHYPLD"] = "";
                }
                //性别
                if (chkSex1.Checked == true)
                {
                    dr["SEX"] = "1";
                }
                else if (chkSex2.Checked == true)
                {
                    dr["SEX"] = "2";
                }


                //阿帕加评分
                dr["APJ"] = txtAPJPF.Text;


                //身长
                dr["HEIGH"] = txtheigh.Text;
                //体重
                dr["WEIGHT"] = txtweight.Text;

                //出生日期
                dr["BITHDAY"] = BithDayDate.DateTime.ToString("yyyy-MM-dd") + " " + BithDayTime.Time.ToString("HH:mm:ss");

                //接产者
                dr["MIDWIFERY"] = lpEdiMidwifery.CodeValue;

                //产出情况
                if (chkCCQK1.Checked == true)
                {
                    dr["CCQK"] = "1";
                }
                else if (chkCCQK2.Checked == true)
                {
                    dr["CCQK"] = "2";
                }
                else if (chkCCQK3.Checked == true)
                {
                    dr["CCQK"] = "3";
                }
                else if (chkCCQK4.Checked == true)
                {
                    dr["CCQK"] = "4";
                }

                //出院情况
                if (chkCYQK1.Checked == true)
                {
                    dr["CYQK"] = "1";
                }
                else if (chkCYQK2.Checked == true)
                {
                    dr["CYQK"] = "2";
                }
                else if (chkCYQK3.Checked == true)
                {
                    dr["CYQK"] = "3";
                }
                //分娩方式
                if (chkFMFS1.Checked == true)
                {
                    dr["FMFS"] = "1";
                }
                else if (chkFMFS2.Checked == true)
                {
                    dr["FMFS"] = "2";
                }
                else if (chkFMFS3.Checked == true)
                {
                    dr["FMFS"] = "3";
                }
                else if (chkFMFS4.Checked == true)
                {
                    dr["FMFS"] = "4";
                }
                else if (chkFMFS5.Checked == true)
                {
                    dr["FMFS"] = "5";
                }
                else if (chkFMFS6.Checked == true)
                {
                    dr["FMFS"] = "6";
                }
                int id = 0;
                foreach (DataRow d in m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows)
                {
                    if (d["IBSBABYID"].ToString().Trim() == "")
                    {
                        id = 1;
                    }
                    else if (int.Parse(d["IBSBABYID"].ToString()) > id)
                    {
                        id = int.Parse(d["IBSBABYID"].ToString());
                    }
                }
                dr["IBSBABYID"] = (id + 1).ToString();
                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 新增婴儿事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region  注销 by jxh  原因：添加事件移至确定按钮事件中
        //private void simpleButtonAddInfo_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(CheckData()))
        //        {
        //            MessageBox.Show(CheckData());
        //            return;
        //        }
        //        DataRow dr = m_IemInfo.IemObstetricsBaby.OutBabyTable.NewRow();
        //        dr = AddOrModifyDataRow(dr);
        //        m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows.Add(dr);
        //        this.GetUI();
        //        this.UpdateShowTable();
        //        this.gridControlBabyInfo.DataSource = m_showTable;
        //        gridControlBabyInfo.EndUpdate();
        //        m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewBabyInfo);
        //    }
        //    catch (Exception ex)
        //    {
        //        MyMessageBox.Show(1, ex);
        //    }
        //}
        #endregion


        /// <summary>
        /// 删除事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonDeleteInfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewBabyInfo.RowCount > 0)
                {
                    if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要删除吗？", "删除该婴儿信息", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.OK)
                    {
                        if (gridViewBabyInfo.FocusedRowHandle < 0)
                        {
                            return;
                        }
                        m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows.RemoveAt(gridViewBabyInfo.FocusedRowHandle);
                        this.FillUIInner(0);
                        this.UpdateShowTable();
                        this.gridControlBabyInfo.DataSource = m_showTable;
                        gridControlBabyInfo.EndUpdate();
                        m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewBabyInfo);
                        this.GetUI();
                    }
                }
                editState = "3";
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 更改，选中后可消除选择
        /// add by wyt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chk_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckEdit chkEdit = GetCheckEdit(((Control)sender).Name);
                if (chkEdit.Checked)
                {
                    chkEdit.Checked = false;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 重绘事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCObstetricsBaby_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                foreach (Control control in this.Controls)
                {
                    if (control is LabelControl)
                    {
                        control.Visible = false;
                        e.Graphics.DrawString(control.Text, control.Font, Brushes.Black, control.Location);

                    }
                    if (control is TextEdit)
                    {
                        e.Graphics.DrawLine(Pens.Black, new Point(control.Location.X, control.Location.Y + control.Height),
                            new Point(control.Width + control.Location.X, control.Height + control.Location.Y));
                    }
                }

            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }

            //e.Graphics.DrawLine(Pens.Black, new Point(0, 0), new Point(0, this.Height));
            ////e.Graphics.DrawLine(Pens.Black, new Point(0, 0), new Point(this.Width, 0));
            //e.Graphics.DrawLine(Pens.Black, new Point(this.Width - 1, 0), new Point(this.Width - 1, this.Height));
        }

        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControlBabyInfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewBabyInfo.FocusedRowHandle < 0)
                {
                    return;
                }
                DataRow dataRow = gridViewBabyInfo.GetDataRow(gridViewBabyInfo.FocusedRowHandle);
                if (dataRow == null)
                {
                    return;
                }

                this.FillUIInner(gridViewBabyInfo.FocusedRowHandle);
                this.GetUI();


            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// <title>出生日期范围验证，不能大于当前日期</title>
        /// <auth>wyt</auth>
        /// <date>2012-11-02</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BithDayDate_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (BithDayDate.DateTime > DateTime.Now)
                {
                    this.errorProvider.SetError(BithDayDate, "超出范围");
                    e.Cancel = true;
                }
                else
                {
                    this.errorProvider.Clear();
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// Enter事件 --- 获取焦点选中内容
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-23</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_Enter(object sender, EventArgs e)
        {
            try
            {
                DS_Common.txt_Enter(sender);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 复选框按键事件
        /// 注：回车即勾选/不勾选
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-29</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chb_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                DS_Common.cbx_KeyPress(sender);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #endregion
        //add by jxh  控件enabled属性控制
        private void UCObstetricsBaby_Load(object sender, EventArgs e)
        {
            try
            {
                foreach (Control control in this.Controls)
                {
                    control.Enabled = false;
                }
                gridControlBabyInfo.Enabled = true;
                panelControl2.Enabled = true;
                //simpleButtonAddInfo.Enabled = true;
                //btnEdit.Enabled = true;
                //simpleButtonDeleteInfo.Enabled = true;
                btn_OK.Enabled = true;
                btn_Close.Enabled = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //add by jxh
        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Control control in this.Controls)
                {
                    control.Enabled = true;
                }
                editState = "2";
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        //add by jxh  控件enabled属性控制
        private void simpleButtonAddInfo_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Control control in this.Controls)
                {
                    control.Enabled = true;
                    if (control.GetType() == typeof(CheckEdit))
                    {
                        ((CheckEdit)control).Checked = false;
                    }
                    foreach (Control item in panelControl3.Controls)
                    {
                        if (item.GetType() == typeof(CheckEdit))
                        {
                            ((CheckEdit)item).Checked = false;
                        }
                    }
                    foreach (Control ctl in this.panelControl1.Controls)
                    {
                        if (ctl.GetType() == typeof(CheckEdit))
                        {
                            ((CheckEdit)ctl).Checked = false;
                        }
                        if (ctl.GetType() == typeof(TextEdit))
                        {
                            ((TextEdit)ctl).Text = "";
                        }
                        if (ctl.GetType() == typeof(LookUpEditor))
                        {
                            ((LookUpEditor)ctl).Text = "";
                        }
                        if (ctl.GetType() == typeof(DateEdit))
                        {
                            ((DateEdit)ctl).Text = "";
                        }
                        if (ctl.GetType() == typeof(TimeEdit))
                        {
                            ((TimeEdit)ctl).Text = "00:00";
                        }
                    }
                }
                editState = "1";//确定事件的判断符，1为添加，2为修改
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }


    }
}
