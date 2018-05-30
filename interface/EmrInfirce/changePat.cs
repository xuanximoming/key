using DrectSoft.Common;
using DrectSoft.Core.MainEmrPad.New;
using DrectSoft.FrameWork.WinForm.Plugin;
using EmrInsert;
using System;
using System.Data;
using System.Windows.Forms;

namespace EmrInfirce
{
    public class ChangePat : IChangePat
    {
        EmrDataHelper emrHelper = null;
        private decimal _noOfInpat;
        public IEmrHost _EmrHost = null;


        /// <summary>
        /// 初始化EMR
        /// </summary>
        /// <param name="UserId">用户名</param>
        public int InitEmr(string UserId)
        {
            try
            {
                if (_EmrHost == null)
                {
                    emrHelper = new EmrDataHelper();
                    emrHelper.thisLogin(UserId);
                    _EmrHost = emrHelper.Formain;
                    DS_Common.currentUser = _EmrHost.User;
                }
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        /// <summary>
        /// 加载病人信息
        /// </summary>
        /// <param name="PatNoOfHis">患者ID</param>
        /// <returns>返回UCEmrInput实例</returns>
        public UserControl ChangePatient(string PatNoOfHis)
        {
            DataTable dt = emrHelper.SelectDataBase(string.Format("select * from InPatient where PatNoOfHis='{0}'", PatNoOfHis));
            //不存在则添加
            if (dt == null || dt.Rows.Count == 0)
            {

                #region 添加示例
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
                #endregion
            }
            //存在更新
            else
            {
                _noOfInpat = Convert.ToDecimal(dt.Rows[0]["noOfInpat"]);

                #region 更新示例
                //DataTable dtHisIns = SqlDataHelper.SelectDataTable(string.Format("select a.*,b.DegreeID,c.GroupName,c.EmrID,d.BedOrder from Inp_Register as a inner join PAT_Patient as b on  a.PatientID=b.PatientID " +
                //    " inner join SDTC_Group as c  on a.CurrentGroupID=c.GroupID left join Inp_Bed as d on a.CurrentBedID=d.BedID " +
                //   "where  a.inid='{0}'", inid));


                //DataTable dtHisPt = SqlDataHelper.SelectDataTable(string.Format("select *,'' as  InICO,'' as OutICO from PAT_Patient where DegreeID='{0}'", dtHisIns.Rows[0]["DegreeID"].ToString()));

                //StringBuilder sb = new StringBuilder();
                //string pt = Convert.ToString(dtHisPt.Rows[0]["DegreeID"]);

                //emrHelper.UpdatePatent(dtHisPt, sb, dtHisIns.Rows[0], pt);
                //dt = emrHelper.SelectDataBase(string.Format("select * from InPatient where PatNoOfHis='{0}'", dtHisIns.Rows[0]["InID"].ToString()));
                #endregion
            }
            return CreateUCEmrInput();
        }
        /// <summary>
        /// 创建UCEmrInput实例
        /// </summary>
        /// <returns>返回UCEmrInput实例</returns>
        private UserControl CreateUCEmrInput()
        {
            _EmrHost.ChoosePatient(_noOfInpat);
            UCEmrInput m_UCEmrInput = new UCEmrInput(_EmrHost.CurrentPatientInfo, _EmrHost);
            m_UCEmrInput.Dock = DockStyle.Fill;
            return m_UCEmrInput;
        }
    }
}
