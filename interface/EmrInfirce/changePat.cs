using DrectSoft.Core.MainEmrPad.New;
using DrectSoft.FrameWork.WinForm.Plugin;
using EmrInsert;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace EmrInfirce
{
    [Guid("26FE474E-125B-46BE-9C7B-E67730109CFE"), ClassInterface(ClassInterfaceType.None), ComSourceInterfaces(typeof(IChangePat))]
    public class ChangePat : IChangePat
    {
        EmrDataHelper _emrHelper = null;
        private decimal _noOfInpat;
        private IEmrHost _EmrHost = null;
        private UCEmrInput _UCEmrInput;


        /// <summary>
        /// 初始化EMR
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <param name="Name">用户名</param>
        /// <param name="DeptId">科室代码</param>
        /// <param name="WardId">病区代码</param>
        /// <param name="Cate">用户权限</param>
        /// <returns>1、成功，0失败</returns>
        public int InitEmr(string UserId, string Name, string DeptId, string WardId, string Cate)
        {
            try
            {
                if (_EmrHost == null)
                {
                    _emrHelper = new EmrDataHelper();
                    _emrHelper._UserId = UserId;
                    _emrHelper._Name = Name;
                    _emrHelper._DeptId = DeptId;
                    _emrHelper._WardId = WardId;
                    _emrHelper._Cate = Cate;
                    _EmrHost = _emrHelper.thisLogin(UserId);
                    if (_EmrHost == null)
                        return 0;
                }
                return 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("电子病历初始化失败！\r\n" + ex.Message);
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
            DataTable dt = _emrHelper.SelectDataBase(string.Format("select * from InPatient where PatNoOfHis='{0}'", PatNoOfHis));
            //不存在则添加
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("EMR中没有此患者信息，请确认！");
                return null;
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
            CreateUCEmrInput();
            return _UCEmrInput;
        }

        /// <summary>
        /// 加载病人信息
        /// </summary>
        /// <param name="WinHandle">父窗口句柄</param>
        /// <param name="PatNoOfHis">患者ID</param>
        public void ChangePatient(string WinHandle, string PatNoOfHis)
        {
            DataTable dt = _emrHelper.SelectDataBase(string.Format("select * from InPatient where PatNoOfHis='{0}'", PatNoOfHis));
            //不存在则添加
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("EMR中没有此患者信息，请确认！");
                return;
            }
            //存在更新
            else
            {
                _noOfInpat = Convert.ToDecimal(dt.Rows[0]["noOfInpat"]);
            }

            CreateUCEmrInput();
            EmrSetWindow(WinHandle, _UCEmrInput);
        }
        /// <summary>
        /// 创建UCEmrInput实例
        /// </summary>
        private void CreateUCEmrInput()
        {
            _EmrHost.ChoosePatient(_noOfInpat);
            _UCEmrInput = new UCEmrInput(_EmrHost.CurrentPatientInfo, _EmrHost);
            _UCEmrInput.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// 设置父窗口
        /// </summary>
        /// <param name="WinHandle">父窗口句柄</param>
        /// <returns>1、成功 -1 失败</returns>
        private int EmrSetWindow(string WinHandle, UserControl m_UserControl)
        {
            int result;
            try
            {
                if (m_UserControl == null || m_UserControl.IsDisposed)
                {
                    CreateUCEmrInput();
                }
                if (Convert.ToInt32(WinHandle) > 0)
                {
                    Form EmrForm = new Form();
                    EmrForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                    EmrForm.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                    EmrForm.Controls.Add(m_UserControl);
                    new CrossPlatformControlHostManager
                    {
                        ContainerHandle = new IntPtr(Convert.ToInt32(WinHandle)),
                        ControlHandle = EmrForm.Handle,
                        Dock = DockStyle.Fill
                    }.UpdateLayout();
                    EmrForm.Show();
                }
                result = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("句柄父窗口创建失败！" + ex.Message);
                result = -1;
            }
            return result;

        }

    }

}
