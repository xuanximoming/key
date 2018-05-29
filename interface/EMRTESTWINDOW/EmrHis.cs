using EmrInsert;
using System;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace EMRTESTWINDOW
{
    public partial class EmrHis : Form
    {
        public EmrHis()
        {
            InitializeComponent();
        }

        EmrDataHelper emrhelper = new EmrDataHelper();
        DataTable dt1 = new DataTable();
        DataTable dt2 = new DataTable();
        int sutlks = 0;
        /// <summary>
        /// 职工
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                sutlks = 1;
                button8.Text = "职工同步";

                string sql = "select e.*,e.EmrID,g.GroupName,g.EmrID as GrupEmrID " +
                "from SDTC_EmployeeGroup as a,CORP_Employee as e,SDTC_Group as g " +
               " where a.EmployeeID=e.EmployeeID and g.GroupID=a.GroupID and g.GroupAttr=21 ";
                string emr = "select * from Users ";
                dt1 = SqlDataHelper.SelectDataTable(sql);
                dataGridView1.DataSource = dt1;
                dt2 = emrhelper.SelectDataBase(emr);
                dataGridView2.DataSource = dt2;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 职工科室
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                sutlks = 2;
                button8.Text = "职工科室同步";

                string sql = " select e.Name,e.EmrID,g.GroupName,g.EmrID as GrupEmrID " +
               " from SDTC_EmployeeGroup as a,CORP_Employee as e,SDTC_Group as g where a.EmployeeID=e.EmployeeID and g.GroupID=a.GroupID  and g.GroupAttr=21 ";
                string emr = "select * from User2Dept ";
                dt1 = SqlDataHelper.SelectDataTable(sql);
                dataGridView1.DataSource = dt1;
                dt2 = emrhelper.SelectDataBase(emr);
                dataGridView2.DataSource = dt2;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
        /// <summary>
        /// 病区
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                sutlks = 3;
                button8.Text = "病区同步";

                string sql = "select * from SDTC_Group where GroupAttr='21'";
                string emr = "select * from Ward ";
                dt1 = SqlDataHelper.SelectDataTable(sql);
                dataGridView1.DataSource = dt1;

                dt2 = emrhelper.SelectDataBase(emr);
                dataGridView2.DataSource = dt2;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 科室
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                sutlks = 4;
                button8.Text = "科室同步";
                string sql = "select * from SDTC_Group where GroupAttr='21'";
                string emr = "select * from Department  ";
                dt1 = SqlDataHelper.SelectDataTable(sql);
                dataGridView1.DataSource = dt1;

                dt2 = emrhelper.SelectDataBase(emr);
                dataGridView2.DataSource = dt2;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 科室病区对应库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                sutlks = 5;
                button8.Text = "病区对应库同步";

                string sql = " select g.GroupName,g.EmrID as GrupEmrID " +
             " from SDTC_Group as g where g.GroupAttr='21' ";
                string emr = "select * from Dept2Ward ";
                dt1 = SqlDataHelper.SelectDataTable(sql);
                dataGridView1.DataSource = dt1;

                dt2 = emrhelper.SelectDataBase(emr);
                dataGridView2.DataSource = dt2;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 床位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                sutlks = 7;
                button8.Text = "床位同步";

                string sql = "select BedOrder,c.EmrID,BedGroupName from Inp_Bed as a,Inp_BedGroup as b,SDTC_Group as c where a.BedGroupID=b.BedGroupID and a.SickroomID=c.GroupID";
                const string emr = "select * from Bed ";
                dt1 = SqlDataHelper.SelectDataTable(sql);
                dataGridView1.DataSource = dt1;
                dt2 = emrhelper.SelectDataBase(emr);
                dataGridView2.DataSource = dt2;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 患者信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                sutlks = 6;
                button8.Text = "患者同步";

                string sql = string.Format("select a.*,b.DegreeID,c.GroupName,c.EmrID,d.BedOrder from Inp_Register as a,PAT_Patient as b,SDTC_Group as c,Inp_Bed as d " +
                    " where a.PatientID=b.PatientID and a.CurrentGroupID=c.GroupID and a.InpRegID=d.InpRegID and a.InpState='20'");
                string emr = "select * from InPatient ";
                dt1 = SqlDataHelper.SelectDataTable(sql);
                dataGridView1.DataSource = dt1;
                dt2 = emrhelper.SelectDataBase(emr);
                dataGridView2.DataSource = dt2;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// 数据同步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            if (sutlks == 1)
                ZhiGongTongbu();
            else if (sutlks == 2)
                ZhiGKeshi();
            else if (sutlks == 3)
                BinQu();
            else if (sutlks == 4)
                Department();
            else if (sutlks == 5)
                KeShiDy();
            else if (sutlks == 6)
                HuanZheTongbu();
            else if (sutlks == 7)
                ChuangWei();
        }
        /// <summary>
        /// 患者同步
        /// </summary>
        private void HuanZheTongbu()
        {
            if (dataGridView1.Rows.Count <= 0)
                return;
            DataTable dt = dt1;
            if (dt == null)
                return;

            try
            {
                DataTable dtPat = null;
                DataTable dtHisPat = null;
                StringBuilder sb = null;
                DataTable dtICO = null;
                foreach (DataRow row in dt.Rows)
                {
                    string pt = Convert.ToString(row["DegreeID"]);
                    if (string.IsNullOrEmpty(pt))
                        continue;

                    dtPat = emrhelper.SelectDataBase(string.Format("select * from InPatient where  PatNoOfHis='{0}'", pt, row["InID"]));
                    if (dtPat != null && dtPat.Rows.Count > 0)
                        continue;
                    dtHisPat = SqlDataHelper.SelectDataTable(string.Format("select *,'' as  InICO,'' as OutICO from PAT_Patient where PatientID='{0}' ", row["PatientID"]));
                    if (dtHisPat == null || dtHisPat.Rows.Count == 0)
                        continue;
                    sb = new StringBuilder();
                    dtICO = SqlDataHelper.SelectDataTable(string.Format("  select b.ICDCode as InICO,b.ICDName as inName,c.ICDCode as OutICO,c.ICDName as outName " +
                        " from Inp_Diagnose as a left join DICT_ICD as b on a.InDiagnoseID=b.ICDID " +
                        " left join DICT_ICD as c on a.OutDiagnoseID=c.ICDID  where a.InpRegID='{0}'  "
                        , row["InpRegID"]));
                    if (dtICO != null && dtICO.Rows.Count > 0)
                    {
                        dtHisPat.Rows[0]["InICO"] = dtICO.Rows[0]["InICO"];
                        dtHisPat.Rows[0]["OutICO"] = dtICO.Rows[0]["OutICO"];
                    }
                    emrhelper.InsertPatent(dtHisPat, sb, row, pt);
                }
                MessageBox.Show("同步完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// 床位
        /// </summary>
        private void ChuangWei()
        {
            if (dataGridView1.Rows.Count <= 0)
                return;
            DataTable dt = dt1;
            if (dt == null)
                return;
            if (MessageBox.Show("是否清除电子病历中的床位信息？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.None) == DialogResult.Yes)
            {
                if (MessageBox.Show("确定要清除电子病历中的床位信息？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.None) == DialogResult.Yes)
                {
                    emrhelper.ExecuteNoneQuery("delete from Bed ");
                }
            }
            StringBuilder sb = null;
            try
            {
                DataTable dtBed = null;
                foreach (DataRow row in dt.Rows)
                {
                    //判断床位是否已经存在
                    dtBed = emrhelper.SelectDataBase(string.Format("select * from Bed where ID='{0}'", row["BedOrder"]));
                    if (dtBed != null && dtBed.Rows.Count > 0)
                        continue;
                    sb = new StringBuilder();
                    sb.Append("insert into Bed(ID,WardId,DeptID,RoomID,Resident,Attend,Chief,SexInfo,Style" +
                        ",InBed,ICU,NoOfInpat,PatNoOfHis,FormerWard,FormerBedID,FormerDeptID,Borrow,Valid) values(");
                    sb.AppendFormat("N'{0}'", row["BedOrder"]);
                    sb.AppendFormat(",N'{0}'", LengthBath(Convert.ToString(row["EmrID"]), 4));
                    sb.AppendFormat(",N'{0}'", LengthBath(Convert.ToString(row["EmrID"]), 4));
                    sb.AppendFormat(",N'{0}'", row["BedGroupName"]);
                    sb.AppendFormat(",'{0}'", "");
                    sb.AppendFormat(",'{0}'", "");
                    sb.AppendFormat(",'{0}'", "");
                    sb.AppendFormat(",'{0}'", "1102");
                    sb.AppendFormat(",'{0}'", "1200");
                    sb.AppendFormat(",'{0}'", "1300");
                    sb.AppendFormat(",'{0}'", 0);
                    sb.AppendFormat(",'{0}'", "");
                    sb.AppendFormat(",'{0}'", "");
                    sb.AppendFormat(",'{0}'", "");
                    sb.AppendFormat(",N'{0}'", "");
                    sb.AppendFormat(",N'{0}'", "");
                    sb.AppendFormat(",N'{0}'", "0");
                    sb.AppendFormat(",N'{0}')", 1);
                    try
                    {
                        emrhelper.ExecuteNoneQuery(sb.ToString());
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                }
                MessageBox.Show("同步完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 科室病区对应库
        /// </summary>
        private void KeShiDy()
        {
            if (dataGridView1.Rows.Count <= 0)
                return;
            try
            {
                DataTable dt = dt1;
                if (dt == null)
                    return;


                StringBuilder sb = null;
                DataTable dtCf = null;
                foreach (DataRow row in dt.Rows)
                {
                    string guopEmrid = LengthBath(Convert.ToString(row["GrupEmrID"]), 4);
                    dtCf = emrhelper.SelectDataBase(string.Format("select * from Dept2Ward where DeptID='{0}' and WardID='{0}'", guopEmrid));
                    if (dtCf != null && dtCf.Rows.Count > 0)
                        continue;
                    sb = new StringBuilder();
                    sb.Append("insert into Dept2Ward(DeptID,WardID) values(");
                    sb.AppendFormat("N'{0}'", guopEmrid);
                    sb.AppendFormat(",N'{0}')", guopEmrid);
                    try
                    {
                        emrhelper.ExecuteNoneQuery(sb.ToString());
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                MessageBox.Show("同步完成。");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 科室
        /// </summary>
        private void Department()
        {
            if (dataGridView1.Rows.Count <= 0)
                return;
            try
            {
                DataTable dt = dt1;
                if (dt == null)
                    return;
                DataTable dtCf = null;
                StringBuilder sb = null;
                foreach (DataRow row in dt.Rows)
                {
                    string str = LengthBath(Convert.ToString(row["EmrID"]), 4);
                    dtCf = emrhelper.SelectDataBase(string.Format("select * from Department where ID='{0}'", str));
                    if (dtCf != null && dtCf.Rows.Count > 0)
                        continue;
                    sb = new StringBuilder();
                    sb.Append("insert into Department(ID,Name,HOSNO,Sort,Mark,TotalChief,TotalAttend," +
                        "TotalResident,TotalNurse,TotalBed,TotalExtra,Valid) values(");
                    sb.AppendFormat("N'{0}'", str);
                    sb.AppendFormat(",N'{0}'", row["GroupName"]);
                    sb.AppendFormat(",N'{0}'", "01");
                    sb.AppendFormat(",N'{0}'", 101);
                    sb.AppendFormat(",N'{0}'", 201);
                    sb.AppendFormat(",N'{0}'", "");
                    sb.AppendFormat(",N'{0}'", "");
                    sb.AppendFormat(",N'{0}'", "");
                    sb.AppendFormat(",N'{0}'", "");
                    sb.AppendFormat(",N'{0}'", "");
                    sb.AppendFormat(",N'{0}'", "");
                    sb.AppendFormat(",N'{0}')", 1);
                    try
                    {
                        emrhelper.ExecuteNoneQuery(sb.ToString());
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                }
                MessageBox.Show("同步完成。");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 病区
        /// </summary>
        private void BinQu()
        {
            if (dataGridView1.Rows.Count <= 0)
                return;
            try
            {
                DataTable dt = dt1;
                if (dt == null)
                    return;
                StringBuilder sb = null;
                DataTable dtBq = null;
                foreach (DataRow row in dt.Rows)
                {
                    string str = LengthBath(Convert.ToString(row["EmrID"]), 4);
                    //判断病区是否已经添加
                    dtBq = emrhelper.SelectDataBase(string.Format("select * from Ward where ID='{0}'", str));
                    if (dtBq != null && dtBq.Rows.Count > 0)
                        continue;
                    sb = new StringBuilder();
                    sb.Append("insert into Ward(ID,Name,Mark,Valid) values(");
                    sb.AppendFormat("N'{0}'", str);
                    sb.AppendFormat(",N'{0}'", row["GroupName"]);
                    sb.AppendFormat(",N'{0}'", 300);
                    sb.AppendFormat(",N'{0}')", 1);
                    try
                    {
                        emrhelper.ExecuteNoneQuery(sb.ToString());
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                MessageBox.Show("同步完成。");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 改变长度
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static string LengthBath(string emr, int length)
        {
            if (emr.Length < length)
            {
                string icn = "";
                for (int i = 0; i < length - emr.Length; i++)
                {
                    icn = icn + "0";
                }
                emr = icn + emr;
            }
            return emr;
        }

        /// <summary>
        /// 职员科室
        /// </summary>
        private void ZhiGKeshi()
        {
            if (dataGridView1.Rows.Count <= 0)
                return;

            try
            {
                DataTable dt = dt1;
                if (dt == null)
                    return;
                StringBuilder sb = null;
                DataTable dtCf = null;
                string userID = null;
                string groupID = null;
                foreach (DataRow row in dt.Rows)
                {
                    userID = LengthBath(Convert.ToString(row["EmrID"]), 6);
                    groupID = LengthBath(Convert.ToString(row["GrupEmrID"]), 4);
                    dtCf = emrhelper.SelectDataBase(string.Format("select * from User2Dept where UserId='{0}' and DeptId='{1}' and WardId='{1}'",
                        userID, groupID));
                    if (dtCf != null && dtCf.Rows.Count > 0)
                        continue;
                    sb = new StringBuilder();
                    sb.Append("insert into User2Dept(UserId,DeptId,WardId) Values(");
                    sb.AppendFormat("'{0}'", LengthBath(Convert.ToString(row["EmrID"]), 6));
                    sb.AppendFormat(",'{0}'", LengthBath(Convert.ToString(row["GrupEmrID"]), 4));
                    sb.AppendFormat(",'{0}')", LengthBath(Convert.ToString(row["GrupEmrID"]), 4));

                    emrhelper.ExecuteNoneQuery(sb.ToString());


                }
                MessageBox.Show("同步完成。");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 职员信息
        /// </summary>
        private void ZhiGongTongbu()
        {
            if (dataGridView1.Rows.Count <= 0)
                return;

            try
            {
                StringBuilder sb = null;
                DataTable dt = dt1;
                DataTable userdt = null;
                DataTable dtUser = null;
                foreach (DataRow row in dt.Rows)
                {

                    sb = new StringBuilder();
                    sb.Append("insert into Users(ID,Name,PY,WB,Sexy,Birth,DeptId,WardID,Category,JobTitle,RecipeMark,NarcosisMark,Grade,Valid,GROUPID,STATUS,ONLINESTATE,JOBID,REGDATE,PASSWD)");
                    string emrid = LengthBath(Convert.ToString(row["EmrID"]), 6);

                    //判断新入用户是否存在
                    userdt = emrhelper.SelectDataBase(string.Format("select * from Users where id='{0}'", emrid));
                    if (userdt != null && userdt.Rows.Count > 0)
                        continue;
                    sb.AppendFormat("values('{0}'", emrid);//ID
                    sb.AppendFormat(",N'{0}'", row["Name"]);//Name
                    sb.AppendFormat(",N'{0}'", row["Acronym"]);//PY
                    sb.AppendFormat(",N'{0}'", row["Acronym"]);//WB
                    if (Convert.ToString(row["SexID"]).Equals("086028000A000001"))
                        sb.AppendFormat(",N'{0}'", 1);//Sexy
                    else
                        sb.AppendFormat(",N'{0}'", 2);//Sexy
                    sb.AppendFormat(",N'{0}'", Convert.ToDateTime(row["Birthday"]).ToString("yyyy-MM-dd"));//Birth

                    string gpStr = LengthBath(Convert.ToString(row["GrupEmrID"]), 4);
                    sb.AppendFormat(",N'{0}'", gpStr);//DeptID
                    sb.AppendFormat(",N'{0}'", gpStr);//WardID 
                    //sb.AppendFormat(",'{0}'", "2401");//DeptID
                    //sb.AppendFormat(",'{0}'", "0012");//WardID 

                    if (Convert.ToString(row["EmployeeAttr"]).Equals("21"))
                    {
                        sb.AppendFormat(",N'{0}'", 400);//Category
                        sb.AppendFormat(",N'{0}'", 5);//JobTitle
                    }
                    else if (Convert.ToString(row["EmployeeAttr"]).Equals("22"))
                    {
                        sb.AppendFormat(",N'{0}'", 402);//Category
                        sb.AppendFormat(",N'{0}'", 15);//JobTitle
                    }
                    else
                    {
                        sb.AppendFormat(",N'{0}'", 404);//Category
                        sb.AppendFormat(",N'{0}'", 19);//JobTitle
                    }
                    sb.AppendFormat(",N'{0}'", -1);//RecipeMark
                    sb.AppendFormat(",N'{0}'", -1);//NarcosisMark
                    sb.AppendFormat(",N'{0}'", 2000);//Grade

                    sb.AppendFormat(",N'{0}'", 1);//Birth 
                    sb.AppendFormat(",N'{0}'", "00,");
                    sb.AppendFormat(",N'{0}'", "1");
                    sb.AppendFormat(",N'{0}'", "0");
                    sb.AppendFormat(",N'{0}'", "66,");
                    sb.AppendFormat(",N'{0}'", " 2014081117:54:38");
                    sb.AppendFormat(",N'{0}'", "QK+T4kBWCUI=");
                    sb.Append(")");
                    emrhelper.ExecuteNoneQuery(sb.ToString());
                }
                MessageBox.Show("同步完成。");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView2.SelectedRows.Count == 0)
                    return;
                DataGridViewSelectedRowCollection dgvscc = dataGridView2.SelectedRows;

                if (sutlks == 1)
                {
                    for (int i = 0; i < dgvscc.Count; i++)
                    {
                        emrhelper.ExecuteNoneQuery(string.Format("delete from Users where ID='{0}' ", dgvscc[i].Cells["ID"].Value));
                    }
                    button1_Click(null, null);
                }
                else if (sutlks == 2)
                {
                    for (int i = 0; i < dgvscc.Count; i++)
                    {
                        emrhelper.ExecuteNoneQuery(string.Format("delete from User2Dept where UserId='{0}' ", dgvscc[i].Cells["UserId"].Value));
                    }
                    button2_Click(null, null);
                }
                else if (sutlks == 3)
                {
                    for (int i = 0; i < dgvscc.Count; i++)
                    {
                        emrhelper.ExecuteNoneQuery(string.Format("delete from Ward where ID='{0}' ", dgvscc[i].Cells["ID"].Value));
                    }
                    button3_Click(null, null);
                }
                else if (sutlks == 4)
                {
                    for (int i = 0; i < dgvscc.Count; i++)
                    {
                        emrhelper.ExecuteNoneQuery(string.Format("delete from Department where ID='{0}' ", dgvscc[i].Cells["ID"].Value));
                    }
                    button4_Click(null, null);
                }
                else if (sutlks == 5)
                {
                    for (int i = 0; i < dgvscc.Count; i++)
                    {
                        emrhelper.ExecuteNoneQuery(string.Format("delete from Dept2Ward where DeptID='{0}' ", dgvscc[i].Cells["DeptID"].Value));
                    }
                    button5_Click(null, null);
                }
                else if (sutlks == 6)
                {
                    for (int i = 0; i < dgvscc.Count; i++)
                    {
                        emrhelper.ExecuteNoneQuery(string.Format("delete from InPatient where NOOFINPAT='{0}'", dgvscc[i].Cells["NOOFINPAT"].Value));
                    }
                    button6_Click(null, null);
                }
                else if (sutlks == 7)
                {
                    for (int i = 0; i < dgvscc.Count; i++)
                    {
                        emrhelper.ExecuteNoneQuery(string.Format("delete from Bed where ID='{0}'", dgvscc[i].Cells["ID"].Value));
                    }
                    button7_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                FrmYeMeiYejiao frm = new FrmYeMeiYejiao();
                DataTable dt = emrhelper.SelectDataBase("select NAME,SUBNAME from HOSPITALINFO");
                if (dt != null && dt.Rows.Count > 0)
                {
                    frm.textBox1.Text = Convert.ToString(dt.Rows[0]["NAME"]);
                    frm.textBox2.Text = Convert.ToString(dt.Rows[0]["SUBNAME"]);
                }
                if (frm.ShowDialog() == DialogResult.Yes)
                {
                    if (dt == null || dt.Rows.Count == 0)
                        emrhelper.ExecuteNoneQuery(string.Format("insert into HOSPITALINFO(NAME,SUBNAME) values(N'{0}',N'{1}')", frm.textBox1.Text, frm.textBox2.Text));
                    else
                        emrhelper.ExecuteNoneQuery(string.Format("update HOSPITALINFO set NAME=N'{0}',SUBNAME=N'{1}'", frm.textBox1.Text, frm.textBox2.Text));
                    MessageBox.Show("保存成功！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void cbZDTBHZ_CheckedChanged(object sender, EventArgs e)
        {
            if (cbZDTBHZ.Checked == true)
            {
                timer1.Start();
            }
            else
            {
                timer1.Stop();
            }
        }

        /// <summary>
        /// 爱迪眼前科老交替过度方法
        /// </summary>
        /// <param name="PatID"></param>
        private void InsertPatient(string inid)
        {
            try
            {
                if (string.IsNullOrEmpty(inid))
                    return;
                DataTable dt = emrhelper.SelectDataBase(string.Format("select * from InPatient where PatNoOfHis='{0}'", inid));

                DataTable dt2 = emrhelper.SelectDataBase("select * from InPatient");

                //不存在则添加
                if (dt == null || dt.Rows.Count == 0)
                {
                    string bedorder = "";
                    DataTable dtHisPtX = SqlDataHelper.SelectDataTable(string.Format("select bedorder from  ViewInpRegister where inid='{0}'", inid));
                    bedorder = dtHisPtX.Rows[0]["bedorder"].ToString().Trim();
                    // MessageBox.Show(dt.Rows[0]["name"].ToString());
                    DataTable dtHisPt = SqlDataHelper.SelectDataTable(string.Format("select  *,'' as  InICO,'' as OutICO from PAT_Patient where PatientID in(select PatientID from  dbo.Inp_Register where inid='{0}')", inid));

                    if (dtHisPt == null)
                        return;
                    DataTable dtHisIns = SqlDataHelper.SelectDataTable(string.Format("select a.*,a.inid as DegreeID,c.GroupName,c.EmrID,a.BedOrder from ViewInpRegister as a,PAT_Patient as b,SDTC_Group as c " +
                       "where a.PatientID=b.PatientID  and a.CurrentGroupID=c.GroupID and  a.InID='{0}'", inid));
                    if (dtHisIns == null || dtHisIns.Rows.Count == 0)
                    {
                        //  emrhelper. OutPat(bedorder);
                        //SDT.Client.ControlsHelper.Show("该患者未分床(占床)或没有病案号。");
                        return;
                    }
                    StringBuilder sb = new StringBuilder();
                    string pt = Convert.ToString(dtHisPt.Rows[0]["DegreeID"]);
                    if (string.IsNullOrEmpty(pt))
                        return;
                    emrhelper.InsertPatent(dtHisPt, sb, dtHisIns.Rows[0], pt);
                    dt = emrhelper.SelectDataBase(string.Format("select * from InPatient where PatNoOfHis='{0}'", inid));
                }
                else
                {
                    //修改床位号
                    string bedorder = "";
                    DataTable dtHisPtX = SqlDataHelper.SelectDataTable(string.Format("select bedorder from  ViewInpRegister where inid='{0}'", inid));
                    bedorder = dtHisPtX.Rows[0]["bedorder"].ToString().Trim();
                    // MessageBox.Show(dt.Rows[0]["name"].ToString());
                    DataTable dtHisPt = SqlDataHelper.SelectDataTable(string.Format("select  *,'' as  InICO,'' as OutICO from PAT_Patient where PatientID in(select PatientID from  dbo.Inp_Register where inid='{0}')", inid));

                    if (dtHisPt == null)
                        return;
                    DataTable dtHisIns = SqlDataHelper.SelectDataTable(string.Format("select a.*,a.inid as DegreeID,c.GroupName,c.EmrID,a.BedOrder from ViewInpRegister as a,PAT_Patient as b,SDTC_Group as c " +
                       "where a.PatientID=b.PatientID  and a.CurrentGroupID=c.GroupID and  a.InID='{0}'", inid));
                    if (dtHisIns == null || dtHisIns.Rows.Count == 0)
                    {
                        //  emrhelper. OutPat(bedorder);
                        //SDT.Client.ControlsHelper.Show("该患者未分床(占床)或没有病案号。");
                        return;
                    }
                    StringBuilder sb = new StringBuilder();
                    string pt = Convert.ToString(dtHisPt.Rows[0]["DegreeID"]);
                    if (string.IsNullOrEmpty(pt))
                        return;
                    emrhelper.updatePatent_Bed(dtHisPt, sb, dtHisIns.Rows[0], pt, inid);
                    dt = emrhelper.SelectDataBase(string.Format("select * from InPatient where PatNoOfHis='{0}'", inid));
                }
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message);
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (cbZDTBHZ.Checked == true)
            {
                DataTable dtHisPt = SqlDataHelper.SelectDataTable("select * from dbo.Inp_Register where inpstate='20'");
                foreach (DataRow item in dtHisPt.Rows)
                {
                    //if (item["name"].ToString().Trim() == "颜苹")
                    //{ 


                    //}

                    InsertPatient(item["InID"].ToString().Trim());
                }
            }
        }

        private void tsmDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridViewSelectedRowCollection dgvscc = dataGridView1.SelectedRows;
                DataRow datarow = null;
                foreach (DataGridViewRow item in dgvscc)
                {
                    datarow = (item.DataBoundItem as DataRowView).Row;
                    dt1.Rows.Remove(datarow);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



    }
}