using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Common.Ctrs.DLG;
using System.Data.SqlClient;
using DrectSoft.DSSqlHelper; 
using DrectSoft.Core.Consultation.NEW.Enum;
using Consultation.NEW;
using DevExpress.XtraTab;
using DrectSoft.Core.MainEmrPad;
using DrectSoft.Emr.Util;
using System.Linq;
using DevExpress.Utils;
using DrectSoft.Common;
using DrectSoft.Service;

namespace Consultation.NEW
{
    /// <summary>
    /// 新的会诊记录单
    /// Add xlb 2013-02-22
    /// </summary>
	public partial class FrmRecordConsult: DevBaseForm
	{
        string nOofinpat = string.Empty;//病案序号
        IEmrHost m_app;
        string consultApplySn;//会诊申请单号

        UCEmrInput m_UCEmrInput;
        bool m_IsLoadedEmrContent = false;
        ConsultRecordForWrite consultZDR;

        int m_ApplyorEmployee = 2;//当前登录人 0-申请人 1-受邀人 2-其他人

        private int ApplyorEmployee
        {
            get
            {
                return m_ApplyorEmployee;
            }
            set
            {
                m_ApplyorEmployee = value;
                switch (value)
                {
                    //申请人登录时默认打开 会诊意见tabpage
                    case 0:
                        xtraTabControl1.SelectedTabPage = xtrConsultRecord;
                        break;
                    //受邀人登录时默认打开 受邀医师意见tabpage
                    case 1:
                        xtraTabControl1.SelectedTabPage = xtraRecord;
                        break;
                    //其他人登录时默认打开 会诊意见tabpage
                    case 2:
                        xtraTabControl1.SelectedTabPage = xtrConsultRecord;
                        break;
                }
            }
        }

        #region 方法  by xlb 2013-03-05

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmRecordConsult()
		{
            try
            {
                if (!DesignMode)
                {
                    DS_SqlHelper.CreateSqlHelper();
                    Register();
                }
                InitializeComponent();
            }
            catch (Exception ex)
            {
                throw ex;
            }
		}

        /// <summary>
        /// 构造函数重载
        /// Add xlb 2013-02-22
        /// </summary>
        /// <param name="noofinpat">病案号</param>
        /// <param name="cconsultApplySn">会诊申请唯一号</param>
        /// <param name="host"></param>
        public FrmRecordConsult(string noofinpat, string cconsultApplySn, IEmrHost host,bool readOnly,ConsultRecordForWrite consultRecordZDR/*会诊意见指定人填写*/):this()
        {
            try
            {
                nOofinpat = noofinpat;
                consultApplySn = cconsultApplySn;
                m_app = host;
                consultZDR = consultRecordZDR;
                ucConsultationApplyForMultiplyNew1.Init(noofinpat,m_app,consultApplySn,true);
                ucRecordSuggestion1.Init(nOofinpat, m_app, consultApplySn, readOnly,consultZDR);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public FrmRecordConsult(string noofinpat, string cconsultApplySn, IEmrHost host, bool readOnly)
            : this()
        {
            try
            {
                nOofinpat = noofinpat;
                consultApplySn = cconsultApplySn;
                m_app = host;
                ucConsultationApplyForMultiplyNew1.Init(noofinpat, m_app, consultApplySn, true);
                ucRecordSuggestion1.Init(nOofinpat, m_app, consultApplySn, readOnly, consultZDR);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 构造函数重载
        /// </summary>
        /// <param name="noofinpat">病案号</param>
        /// <param name="cconsultApplySn">会诊申请单号</param>
        /// <param name="host">接口</param>
        /// <param name="readOnly">只读性</param>
        /// <param name="consultRecordZDR">会诊类型</param>
        /// <param name="level">登录人级别 申请人、受邀人、其他人</param>
        public FrmRecordConsult(string noofinpat, string cconsultApplySn, IEmrHost host, bool readOnly/*会诊记录单是否只读*/, ConsultRecordForWrite consultRecordZDR/*会诊意见指定人填写*/,int level/*当前登录人*/)
            : this()
        {
            try
            {
                nOofinpat = noofinpat;
                consultApplySn = cconsultApplySn;
                m_app = host;
                consultZDR = consultRecordZDR;
                ApplyorEmployee = level;
                ucConsultationApplyForMultiplyNew1.Init(noofinpat, m_app, consultApplySn, true);
                ucRecordSuggestion1.Init(nOofinpat, m_app, consultApplySn, readOnly,consultZDR);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 注册事件
        /// Add xlb 2013-02-22
        /// </summary>
        private void Register()
        {
            try
            {
                this.Load+=new EventHandler(FrmRecordConsult_Load);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据申请科室表邀请医师人数动态添加意见填写的表格
        /// Add xlb 2013-02-22
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        private void AddTable(DataRow row,int index,bool readOnly/*根据当前登录人设置是否可编辑*/,bool IsCenter)
        {
            try
            {
                UCSuggestion ucConsultSuggestion;
                if (row == null)
                {
                    ucConsultSuggestion = new UCSuggestion(consultApplySn, m_app);
                    ucConsultSuggestion.groupControl1.Text ="当前登录人" + "会诊意见";
                }
                else
                {
                    ucConsultSuggestion = new UCSuggestion(consultApplySn,row, m_app);
                    ucConsultSuggestion.groupControl1.Text = row["NAME"].ToString() + "会诊意见";
                    if (m_app.User.Id == row["ID"].ToString())//受邀人登录显示保存草稿
                    {
                        ucConsultSuggestion.memoEditSuggestion.Text = row["CONSULTSUGGESTION"].ToString();
                    }
                    else if (row["STATE"].ToString().Trim() == "20")
                    {
                        ucConsultSuggestion.memoEditSuggestion.Text = row["CONSULTSUGGESTION"].ToString();
                    }
                    else
                    {
                        ucConsultSuggestion.memoEditSuggestion.Text = "";
                    }
                }
                if (index == 1)
                {
                    ucConsultSuggestion.btnSave.Visible = !readOnly;
                    ucConsultSuggestion.btnCompelete.Visible = !readOnly;
                    ucConsultSuggestion.memoEditSuggestion.Properties.ReadOnly = readOnly;
                }
                else  if(index != 1)//当前登录人为受邀医师第一个意见框可编辑
                {
                    ucConsultSuggestion.btnSave.Visible = false;
                    ucConsultSuggestion.btnCompelete.Visible = false;
                    ucConsultSuggestion.memoEditSuggestion.Properties.ReadOnly = true;
                }
                if (IsCenter)//此页仅有单个会诊意见时居中显示
                {
                    panelConsultRecord.Controls.Add(ucConsultSuggestion);
                    int y1=ucConsultSuggestion.Height;
                    ucConsultSuggestion.Height = y1 * 2;//单个意见框高度放为原2倍
                    //意见填写区域放为原2倍高度
                    ucConsultSuggestion.memoEditSuggestion.Height = ucConsultSuggestion.memoEditSuggestion.Height+y1;
                    //调整按钮位置
                    ucConsultSuggestion.btnCompelete.Location = new Point(ucConsultSuggestion.btnCompelete.Location.X, ucConsultSuggestion.btnCompelete.Location.Y + y1);
                    ucConsultSuggestion.btnSave.Location = new Point(ucConsultSuggestion.btnSave.Location.X, ucConsultSuggestion.btnSave.Location.Y + y1);
                    int x = panelConsultRecord.Width - ucConsultSuggestion.Width;
                    int y = panelConsultRecord.Height - ucConsultSuggestion.Height;
                    ucConsultSuggestion.Location = new Point(x / 2, y / 2);
                }
                else
                {
                    panelConsultRecord.Controls.Add(ucConsultSuggestion);
                    int x = panelConsultRecord.Width - ucConsultSuggestion.Width;
                    int y = 2 + ucConsultSuggestion.Height * (index - 1);
                    ucConsultSuggestion.Location = new Point(x / 2, y);
                }
                ucConsultSuggestion.BringToFront();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化数据 产生会诊意见界面
        /// Add xlb 2013-03-04
        /// </summary>
        private void InitData()
        {
            try
            {
                string sql = @"select * from consultsuggestion c join users u on c.createuser=u.id where c.consultapplysn=@consultApplySn and c.valid = '1'";
                SqlParameter[] sps = { new SqlParameter("@consultApplySn", consultApplySn) };
                DataTable dt = DS_SqlHelper.ExecuteDataTable(sql, sps, CommandType.Text);

                //当前登录人是申请人
                if (ApplyorEmployee == 0)
                {
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        return;
                    }
                    else if (dt.Rows.Count == 1)
                    {
                        AddTable(dt.Rows[0],1, true, true);//只读界面
                    }
                    else if (dt.Rows.Count > 1)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            AddTable(dt.Rows[i], i + 1, true, false);//只读界面
                        }
                    }
                }
                else if (ApplyorEmployee == 1)//登录人是受邀医师
                {
                    if (dt.Rows.Count > 1)
                    {
                        var employeeList = from DataRow dr in dt.Rows
                                           where dr["CREATEUSER"].ToString().Equals(m_app.User.Id)
                                           select dr["CREATEUSER"].ToString();
                        if (employeeList.Count() > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (dt.Rows[i]["CREATEUSER"].ToString().Equals(m_app.User.Id))
                                {
                                    DataRow row = dt.NewRow();
                                    row.ItemArray = dt.Rows[i].ItemArray;
                                    dt.Rows.RemoveAt(i);
                                    dt.Rows.InsertAt(row, 0);
                                }
                            }
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                AddTable(dt.Rows[i], i + 1,dt.Rows[i]["STATE"].Equals("20")?true:false,false);//false 表示可编辑
                            }
                        }
                        else
                        {
                            AddTable(null, 1,false,false);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                AddTable(dt.Rows[i], i + 2,false,false);
                            }
                        }
                    }
                    else if (dt.Rows.Count == 1)
                    {
                        var employeeList = from DataRow dr in dt.Rows
                                           where dr["CREATEUSER"].ToString().Equals(m_app.User.Id)
                                           select dr["CREATEUSER"].ToString();
                        if (employeeList.Count() > 0)
                        {
                            AddTable(dt.Rows[0], 1,false,true);
                        }
                        else
                        {
                            AddTable(null, 1,false,false);
                            AddTable(dt.Rows[0],2,false,false);
                        }
                    }
                    else if (dt.Rows.Count == 0)
                    {
                        AddTable(null, 1,false,true);
                    }
                }
                else if (ApplyorEmployee == 2)//登录人是其他人
                {
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        return;
                    }
                    if (dt.Rows.Count == 1)
                    {
                        AddTable(dt.Rows[0], 1, true, true);
                    }
                    else if(dt.Rows.Count>1)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            AddTable(dt.Rows[i], i + 1, true, false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 注释 会诊记录单位置
        //UCRecordSuggestion m_UCRecordSuggestion;
        ///// <summary>
        ///// 居中显示会诊记录单界面
        ///// </summary>
        //private void SetUCRecordSuggestionCenter()
        //{
        //    try
        //    {
        //        if (m_UCRecordSuggestion.FindForm()!= null)
        //        {
        //            m_UCRecordSuggestion.Location = new Point((m_UCRecordSuggestion.Parent.Width - m_UCRecordSuggestion.Width) / 2, 2);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion

        #endregion

        #region 病历内容 - 老版
        /// <summary>
        /// 加载文书录入界面
        /// Add xlb 2013-03-04
        /// </summary>
        private void AddEmrInput()
        {
            try
            {
                m_UCEmrInput = new UCEmrInput();
                m_UCEmrInput.CurrentInpatient = null;
                m_UCEmrInput.HideBar();
                RecordDal m_RecordDal = new RecordDal(m_app.SqlHelper);
                m_UCEmrInput.SetInnerVar(m_app, m_RecordDal);
                xtraEmrInpat.Controls.Add(m_UCEmrInput);
                m_UCEmrInput.Dock = DockStyle.Fill;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 加载病历内容
        /// Add xlb 2013-03-04
        /// </summary>
        private void LoadEmrContent()
        {
            try
            {
                if (!string.IsNullOrEmpty(nOofinpat) && !m_IsLoadedEmrContent)
                {
                    m_UCEmrInput.PatientChangedByIEmrHost(Convert.ToDecimal(nOofinpat));
                    m_UCEmrInput.HideBar();
                    m_IsLoadedEmrContent = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 病历内容 - 新版
        /// <summary>
        /// 病历内容窗体
        /// </summary>
        DrectSoft.Core.MainEmrPad.New.UCEmrInput m_UCEmrInputNew;
        bool m_IsLoadedEmrContentNew = false;

        /// <summary>
        /// 加载病历
        /// </summary>
        private void AddEmrInputNew()
        {
            try
            {
                if (string.IsNullOrEmpty(nOofinpat) || m_IsLoadedEmrContentNew)
                {
                    return;
                }
                m_app.ChoosePatient(Convert.ToDecimal(nOofinpat), FloderState.None.ToString());//切换病人

                m_UCEmrInputNew = new DrectSoft.Core.MainEmrPad.New.UCEmrInput(m_app.CurrentPatientInfo, m_app, FloderState.None);
                m_UCEmrInputNew.SetVarData(m_app);
                xtraEmrInpat.Controls.Add(m_UCEmrInputNew);
                m_UCEmrInputNew.OnLoad();
                m_UCEmrInputNew.HideBar();
                m_UCEmrInputNew.Dock = DockStyle.Fill;
                m_IsLoadedEmrContentNew = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 事件  by xlb 2013-03-05

        /// <summary>
        /// 加载事件
        /// Add xlb 2013-02-22
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmRecordConsult_Load(object sender, EventArgs e)
        {
            try
            {
                //多科会诊且由受邀医师填写会诊意见页显示
                if (consultZDR != ConsultRecordForWrite.MultiEmployee)
                {
                    xtraRecord.PageVisible = false;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// tabcontrol切换事件
        /// Add xlb 2013-03-04
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xtraTabControl1_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            try
            {
                if (xtraTabControl1.SelectedTabPage == xtraEmrInpat)
                {
                    string config = DS_SqlService.GetConfigValueByKey("IsNewUcInput");
                    if (null != config && config.Trim() == "1")
                    {
                        AddEmrInputNew();
                    }
                    else
                    {
                        AddEmrInput();
                        LoadEmrContent();
                    }
                }
                else if (xtraTabControl1.SelectedTabPage == xtraRecord)
                {
                    if (consultZDR == ConsultRecordForWrite.MultiEmployee)
                    {
                        InitData();
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #endregion
    }
}