/*********************************************************************************
** File Name	:   FormItemFunctio.cs
** User		    :	xjt
** Date	        :	2010-06-12
** Description	:	功能选择
*********************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Core;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Wordbook;
namespace DrectSoft.MainFrame
{
    public partial class FormItemFunction : DevBaseForm
    {
        #region members
        public DeptWardInfo SelectDept
        {
            get { return _selectDept; }
        }
        private DeptWardInfo _selectDept;


        #endregion

        private string id = null;
        public FormItemFunction()
        {
            InitializeComponent();
        }

        public void GetId(string id)
        {
            this.id = id;
        }

        private void FormItemFunction_Load(object sender, EventArgs e)
        {
            InitWardInfo();
            SetChineseIEM();
        }

        #region events

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if ((lookUpEditorDepart.DisplayValue != null) && (lookUpEditorDepart.DisplayValue != ""))
            {
                //string getDeptWards = @"SELECT a.id DEPTID, a.name DEPTNAME, b.id WARDID, b.name WARDNAME 
                //   FROM department a, ward b, dept2ward c
                // WHERE a.id = c.deptid and b.id = c.wardid and a.valid = '1' and b.valid = '1' and c.deptid='" + lookUpEditorDepart.CodeValue + "' ORDER BY a.name";
                string getDeptWards = @"select * 
                                            from ( SELECT a.id||b.id FLOWID ,
                                                          a.id DEPTID,
                                                          b.id WARDID,
                                                          a.name DEPTNAME ,
                                                          b.name WardName 
                                                        FROM department a, ward b, dept2ward c 
                                                        WHERE a.id = c.deptid 
                                                        and b.id = c.wardid 
                                                        and a.valid = '1' 
                                                        and b.valid = '1' ORDER BY a.name) 
                                            where FLOWID=" + "'" + lookUpEditorDepart.CodeValue.ToString() + "'";
                DataTable dt = ((IEmrHost)FormMain.Instance).SqlHelper.ExecuteDataTable(getDeptWards);
                if (dt.Rows.Count > 0)
                {
                    DeptWardInfo newDeptWardInfo = new DeptWardInfo(dt.Rows[0]["DeptID"].ToString().Trim(), dt.Rows[0]["DeptName"].ToString().Trim(), dt.Rows[0]["WardID"].ToString().Trim(), dt.Rows[0]["WardName"].ToString().Trim());
                    _selectDept = newDeptWardInfo;
                    this.DialogResult = DialogResult.OK;
                }
                else
                    FormMain.Instance.MessageShow("请选择病区!", CustomMessageBoxKind.InformationOk);
            }
            else
                FormMain.Instance.MessageShow("请选择病区!", CustomMessageBoxKind.InformationOk);
        }


        #endregion

        #region methods


        private void InitWardInfo()
        {
            try
            {
                string jobIDs = AppConfigReader.GetAppConfig("ChangeWardLookAllWard").Config;

                string[] jobArray = jobIDs.Split(',');
                string[] userJobArray = FormMain.Instance.User.GWCodes.Split(',');
                bool isLookAllWard = false;

                //如果删除了岗位，这边要先判断在配置中的岗位存在ywk 
                string sqlSerchJob = "select id from jobs ";
                ArrayList jobs = new ArrayList();
                DataTable DtPower = null;
                DtPower = ((IEmrHost)FormMain.Instance).SqlHelper.ExecuteDataTable(sqlSerchJob);
                if (DtPower.Rows.Count > 0)
                {
                    for (int i = 0; i < DtPower.Rows.Count; i++)
                    {
                        jobs.Add(DtPower.Rows[i]["ID"].ToString());
                    }
                }
                DtPower = null; //add by Ukey 2016-08-26
                foreach (string jobID in jobArray)
                {
                    foreach (string userJobID in userJobArray)
                    {
                        if (jobID == userJobID && jobID != "")
                        {
                            if (jobs.Contains(jobID))
                            {
                                isLookAllWard = true;
                            }

                        }
                    }
                }

                //update by Ukey 2016-08-26 判断非管理员权限的单一员工权限， 限制切换科室的一个人进入两个（非全院科室）权限
                string SqlGetPower = @"select power from users where id ='" + this.id + "'";
                DtPower = ((IEmrHost)FormMain.Instance).SqlHelper.ExecuteDataTable(SqlGetPower);
                DataRow DrRow = DtPower.Rows[0];
                string StPower = null;
                StPower = DrRow["power"].ToString();


                DataTable dt = null;
                Dictionary<string, int> columnwidth = new Dictionary<string, int>();
                SqlWordbook sqlWordBook = null;

                if (isLookAllWard || StPower == "1")//update by Ukey 2016-08-26
                {
                    string getAllDeptWards = @"select *   
                                                    from (SELECT a.id || b.id FLOWID,
                                                    a.id DEPTID,
                                                    b.id WARDID,
                                                    a.name DEPTNAME,
                                                    b.name WardName
                                                FROM department a, ward b, dept2ward c
                                                WHERE a.id = c.deptid
                                                and b.id = c.wardid
                                                and a.valid = '1'
                                                and b.valid = '1' ORDER BY a.name)"; //update by Ukey 2016-08-26
                    dt = ((IEmrHost)FormMain.Instance).SqlHelper.ExecuteDataTable(getAllDeptWards);
                    dt.Columns["FLOWID"].Caption = "代码";
                    dt.Columns["WARDID"].Caption = "病区ID";
                    dt.Columns["DEPTID"].Caption = "科室ID";
                    dt.Columns["DEPTNAME"].Caption = "科室";
                    dt.Columns["WARDNAME"].Caption = "病区";
                }
                else
                {
                    string getAllDeptWards = @"select *   
                                                    from (SELECT a.id || b.id FLOWID,
                                                    a.id DEPTID,
                                                    b.id WARDID,
                                                    a.name DEPTNAME,
                                                    b.name WardName
                                                FROM department a, ward b, dept2ward c
                                                WHERE a.id = c.deptid
                                                and b.id = c.wardid
                                                and a.valid = '1'
                                                and b.valid = '1'
                                                and a.id in (select deptid 
                                                                    from user_dept 
                                                                    where userid = '" + this.id + "') ORDER BY a.name)";//update by Ukey 2016-08-06
                    dt = ((IEmrHost)FormMain.Instance).SqlHelper.ExecuteDataTable(getAllDeptWards);
                    if (dt == null)
                    {
                        dt = new DataTable();
                        DataColumn colCode = new DataColumn("DEPTID");
                        DataColumn colWardId = new DataColumn("WARDID");
                        DataColumn colFLOWID = new DataColumn("FLOWID");
                        DataColumn colName = new DataColumn("DEPTNAME");
                        dt.Columns.Add(colCode);
                        dt.Columns.Add(colName);
                        dt.Columns.Add(colFLOWID);
                        dt.Columns.Add(colWardId);
                        dt.Columns["FLOWID"].Caption = "代码";
                        dt.Columns["DEPTID"].Caption = "科室ID";
                        dt.Columns["WARDID"].Caption = "病区ID";
                        dt.Columns["DEPTNAME"].Caption = "科室";

                        columnwidth = new Dictionary<string, int>();
                        foreach (DeptWardInfo ward in FormMain.Instance.User.RelateDeptWards)
                        {
                            DataRow row = dt.NewRow();
                            row["DEPTID"] = ward.DeptId;
                            row["WARDID"] = ward.WardId;
                            row["FLOWID"] = ward.DeptId + ward.WardId;
                            row["DEPTNAME"] = ward.DeptName + "(" + ward.WardName + ")";
                            dt.Rows.Add(row);
                        }
                    }
                    else
                    {
                        dt.Columns["FLOWID"].Caption = "代码";
                        dt.Columns["DEPTID"].Caption = "科室ID";
                        dt.Columns["WARDID"].Caption = "病区ID";
                        dt.Columns["DEPTNAME"].Caption = "科室";
                        dt.Columns["WARDNAME"].Caption = "病区";
                    }
                }
                new GenerateShortCode(((IEmrHost)FormMain.Instance).SqlHelper).AutoAddShortCode(dt, "DEPTNAME");
                columnwidth.Add("DEPTID", 45);
                columnwidth.Add("WARDID", 45);
                columnwidth.Add("DEPTNAME", 110);
                sqlWordBook = new SqlWordbook("queryname", dt, "FLOWID", "DEPTNAME", columnwidth, "DEPTID//DEPTNAME//PY//WB");
                this.lookUpEditorDepart.SqlWordbook = sqlWordBook;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                FormMain.Instance.Logger.Error(ex);
            }
            catch (Exception ex)
            {
                FormMain.Instance.Logger.Error(ex);
            }
        }
        #endregion

        private void SetChineseIEM()
        {
            foreach (InputLanguage MyInput in InputLanguage.InstalledInputLanguages)
            {
                if (MyInput.LayoutName.IndexOf("拼音") >= 0)
                {
                    InputLanguage.CurrentInputLanguage = MyInput;
                    break;
                }
            }
        }
    }
}