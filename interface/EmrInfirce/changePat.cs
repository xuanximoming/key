using DrectSoft.Common;
using DrectSoft.Core.MainEmrPad.New;
using DrectSoft.FrameWork.WinForm.Plugin;
using EmrInsert;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace EmrInfirce
{
    public class changePat
    {
        EmrDataHelper emrHelper = null;
        private decimal _noOfInpat;
        private const string str_queryPatByID = "select * from InPatient where NOOFINPAT={0}";
        public IEmrHost _EmrHost = null;
        public DrectSoft.MainFrame.FormMain _Formain = null;
        DrectSoft.MainFrame.FormMain Formain
        {
            get
            {
                if (_Formain == null)
                {
                    _Formain = new DrectSoft.MainFrame.FormMain(false, "file.menu", true);
                    _Formain.isLG = null;
                }
                return _Formain;
            }
        }

        public void BindEmr(decimal noOfInpat)
        {
            _noOfInpat = noOfInpat;
            EmrLogin();
            AddControls();
        }

        private void EmrLogin()
        {
            if (Formain.isLG == null)
            {
                EmrDataHelper emr = new EmrDataHelper();
                emr.thisLogin();
                _EmrHost = emr.Formain;
                _Formain.Size = new Size(0, 0);
            }
            else
            {
                _EmrHost = Formain;
            }
            if (emrHelper == null)
            {
                emrHelper = new EmrDataHelper();
                emrHelper.m_SqlHelper = Formain.SqlHelper;
            }
        }
        /// <summary>
        /// 添加控件
        /// </summary>
        private UserControl AddControls()
        {
            if (_EmrHost == null)
                EmrLogin();
            _EmrHost.ChoosePatient(_noOfInpat);
            UCEmrInput m_UCEmrInput = new UCEmrInput(_EmrHost.CurrentPatientInfo, _EmrHost);
            m_UCEmrInput.Dock = DockStyle.Fill;
            return m_UCEmrInput;
        }
        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="noOfInpat">档案号</param>
        public UserControl Shuaxin(string inid)
        {
            try
            {
                if (_EmrHost == null)
                {
                    emrHelper = new EmrDataHelper();
                    emrHelper.thisLogin();
                    _EmrHost = emrHelper.Formain;
                    DS_Common.currentUser = _EmrHost.User;
                    _Formain = emrHelper.Formain;
                }
                DataTable dt = emrHelper.SelectDataBase(string.Format("select * from InPatient where PatNoOfHis='{0}'", inid));

                //不存在则添加
                if (dt == null || dt.Rows.Count == 0)
                {
                    //DataTable dtHisIns = SqlDataHelper.SelectDataTable(string.Format("select a.*,b.DegreeID,c.GroupName,c.EmrID,d.BedOrder from Inp_Register as a inner join PAT_Patient as b on  a.PatientID=b.PatientID " +
                    //    " inner join SDTC_Group as c  on a.CurrentGroupID=c.GroupID left join Inp_Bed as d on a.CurrentBedID=d.BedID " +
                    //   "where  a.inid='{0}'", inid));
                    //if (dtHisIns == null || dtHisIns.Rows.Count == 0)
                    //{
                    //    SDT.Client.ControlsHelper.Show("该患者未分床(占床)或者没有病案号。");
                    //    return;
                    //}
                    //if (Convert.ToString(dtHisIns.Rows[0]["DegreeID"]).Trim() == string.Empty)
                    //{
                    //    SDT.Client.ControlsHelper.Show("该患者没有病案号。");
                    //    return;
                    //}
                    //DataTable dtHisPt = SqlDataHelper.SelectDataTable(string.Format("select *,'' as  InICO,'' as OutICO from PAT_Patient where DegreeID='{0}'", dtHisIns.Rows[0]["DegreeID"].ToString()));
                    //if (dtHisPt == null)
                    //    return;
                    //StringBuilder sb = new StringBuilder();
                    //string pt = Convert.ToString(dtHisPt.Rows[0]["DegreeID"]);
                    //if (string.IsNullOrEmpty(pt))
                    //{
                    //    SDT.Client.ControlsHelper.Show("该患者没有病案号，请录入病案号后，再试。");
                    //    return;
                    //}
                    //emrHelper.InsertPatent(dtHisPt, sb, dtHisIns.Rows[0], pt);
                    //dt = emrHelper.SelectDataBase(string.Format("select * from InPatient where PatNoOfHis='{0}'", dtHisIns.Rows[0]["InID"].ToString()));
                }
                else
                {
                    _noOfInpat = Convert.ToDecimal(dt.Rows[0]["noOfInpat"]);
                    //DataTable dtHisIns = SqlDataHelper.SelectDataTable(string.Format("select a.*,b.DegreeID,c.GroupName,c.EmrID,d.BedOrder from Inp_Register as a inner join PAT_Patient as b on  a.PatientID=b.PatientID " +
                    //    " inner join SDTC_Group as c  on a.CurrentGroupID=c.GroupID left join Inp_Bed as d on a.CurrentBedID=d.BedID " +
                    //   "where  a.inid='{0}'", inid));


                    //DataTable dtHisPt = SqlDataHelper.SelectDataTable(string.Format("select *,'' as  InICO,'' as OutICO from PAT_Patient where DegreeID='{0}'", dtHisIns.Rows[0]["DegreeID"].ToString()));

                    //StringBuilder sb = new StringBuilder();
                    //string pt = Convert.ToString(dtHisPt.Rows[0]["DegreeID"]);

                    //emrHelper.UpdatePatent(dtHisPt, sb, dtHisIns.Rows[0], pt);
                    //dt = emrHelper.SelectDataBase(string.Format("select * from InPatient where PatNoOfHis='{0}'", dtHisIns.Rows[0]["InID"].ToString()));
                }
                return AddControls();
            }
            catch (Exception ex)
            {
                return null;
                // SDT.Client.ControlsHelper.Show(ex.Message);
            }


        }
        #region IEmrHost 成员


        ///// <summary>
        ///// 要自己写东西...........................构建用户信息
        ///// </summary>
        //DrectSoft.Core.IUser IEmrHost.User
        //{
        //    get
        //    {

        //        return _EmrHost.User;
        //    }
        //}

        ///// <summary>
        ///// 不用管此方法
        ///// </summary>
        ///// <param name="assemblyName"></param>
        ///// <param name="startupClassName"></param>
        ///// <returns></returns>
        //bool IEmrHost.LoadPlugIn(string assemblyName, string startupClassName)
        //{
        //    return false;
        //    //throw new NotImplementedException();
        //}

        ///// <summary>
        ///// 不用管此方法
        ///// </summary>
        ///// <param name="typeName"></param>
        ///// <returns></returns>
        //bool IEmrHost.LoadPlugIn(string typeName)
        //{
        //    return false;
        //    //throw new NotImplementedException();
        //}

        //void IEmrHost.ChoosePatient(decimal firstPageNo)
        //{
        //    //DataRow row = GetPatInfo(firstPageNo);
        //    //if (row != null)
        //    //    CurrentPatientInfo = new Inpatient(row);
        //    _EmrHost.ChoosePatient(firstPageNo);
        //}

        ////private DataRow GetPatInfo(decimal noOfInpat)
        ////{
        ////    //DataTable table = SqlHelper.ExecuteDataTable(string.Format(str_queryPatByID, noOfInpat));

        ////    //if (table.Rows.Count < 1) return null;
        ////    //return table.Rows[0];


        ////}

        //public Inpatient CurrentPatientInfo
        //{
        //    get { return _currentPat; }
        //    set
        //    {
        //        _currentPat = value;
        //    }

        //}

        ///// <summary>
        ///// 医院信息
        ///// </summary>
        //DrectSoft.Core.HospitalInfo IEmrHost.CurrentHospitalInfo
        //{
        //    get
        //    {
        //        return _EmrHost.CurrentHospitalInfo;
        //        //HospitalInfo info = _EmrHost.CurrentHospitalInfo;
        //        //return info;
        //    }
        //}

        //DrectSoft.Core.ICustomMessageBox IEmrHost.CustomMessageBox
        //{
        //    get
        //    {
        //        return _EmrHost.CustomMessageBox;
        //    }
        //}

        //DrectSoft.Core.IDataAccess IEmrHost.SqlHelper
        //{
        //    get
        //    {

        //        return _EmrHost.SqlHelper;
        //    }
        //}

        //string IEmrHost.MacAddress
        //{
        //    get { return _EmrHost.MacAddress; }
        //}

        //DrectSoft.Core.IAppConfigReader IEmrHost.AppConfig
        //{
        //    get { return _EmrHost.AppConfig; }
        //}

        //DataSet IEmrHost.PatientInfos
        //{
        //    get { return _EmrHost.PatientInfos; }
        //}

        //string IEmrHost.RefreshPatientInfos()
        //{
        //    return _EmrHost.RefreshPatientInfos();
        //}

        //System.Collections.ObjectModel.Collection<DrectSoft.FrameWork.Plugin.Manager.PlugInConfiguration> IEmrHost.PrivilegeMenu
        //{
        //    get { return _EmrHost.PrivilegeMenu; }
        //}

        //DrectSoft.FrameWork.Plugin.PluginManager IEmrHost.Manager
        //{
        //    get { return _EmrHost.Manager; }
        //}

        //bool IEmrHost.EmrAllowEdit
        //{
        //    get
        //    {
        //        return _EmrHost.EmrAllowEdit;
        //    }
        //    set
        //    {
        //        _EmrHost.EmrAllowEdit = value;
        //    }
        //}

        //PluginUtil IEmrHost.PublicMethod
        //{
        //    get { return _EmrHost.PublicMethod; }
        //}

        //private EmrDefaultSetting m_EmrDefaultSetting;
        //DrectSoft.Common.Eop.EmrDefaultSetting IEmrHost.EmrDefaultSettings
        //{
        //    get
        //    {
        //        return _EmrHost.EmrDefaultSettings;
        //        //if (m_EmrDefaultSetting == null)
        //        //{
        //        //    m_EmrDefaultSetting =DrectSoft. Util.InitEmrDefaultSet();
        //        //}

        //        //return m_EmrDefaultSetting;
        //    }
        //}

        //private DrectSoftLog _logger;
        //DrectSoft.Core.DrectSoftLog IEmrHost.Logger
        //{
        //    get
        //    {
        //        return _EmrHost.Logger;
        //    }
        //}

        //private string m_CurrentSelectedEmrID = string.Empty;
        //string IEmrHost.CurrentSelectedEmrID
        //{
        //    get
        //    {
        //        return _EmrHost.CurrentSelectedEmrID;
        //    }
        //    set
        //    {
        //        _EmrHost.CurrentSelectedEmrID = value;
        //    }
        //}

        //void IEmrHost.ShowMessageWindow(DataTable dt, bool isClear)
        //{
        //    // throw new NotImplementedException();
        //}

        //


        //public void ChoosePatient(decimal firstPageNo, string FloderState)
        //{
        //    throw new NotImplementedException();
        //}

        //public void ChoosePatient(string firstPageNo, out Inpatient MyInp)
        //{
        //    throw new NotImplementedException();
        //}

        //public string FloderState
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //    set
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //public Inpatient NEWCurrentPatientInfo
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //    set
        //    {
        //        throw new NotImplementedException();
        //    }
        #endregion
        //}
    }
}
