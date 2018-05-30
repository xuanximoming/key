using DrectSoft.Core;
using DrectSoft.Core.OwnBedInfo;
using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EmrInsert
{
    public partial class EmrDataHelper
    {

        public bool isLoginResult = false;
        public IDataAccess m_SqlHelper;
        DrectSoft.MainFrame.FormMain _Formain = null;
        public DrectSoft.MainFrame.FormMain Formain
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
        /// <summary>
        /// 科室代码
        /// </summary>
        private string m_DeptId;
        /// <summary>
        /// 病区代码
        /// </summary>
        private string m_WardId;

        public void thisLogin()
        {
            if (Formain.isLG == null)
            {
                Formain.m_FormLogin.textBoxUserID.Text = "00";

                Formain.otherUser = true;
                Formain.m_FormLogin.textBoxUserID_Leave(null, null);
                Formain.m_FormLogin.textBoxPassword.Text = "";
                Formain.Size = new Size(0, 0);
                Formain.isLG = null;
                try
                {
                    isLoginResult = Formain.Login();
                    m_DeptId = "2401";
                    m_WardId = "2401";
                    m_SqlHelper = Formain.SqlHelper;
                }
                catch { }
            }
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql"></param>
        public void ExecuteNoneQuery(string sql)
        {
            try
            {
                m_SqlHelper.ExecuteNoneQuery(sql);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// 查询所有病历档案或者某人档案
        /// </summary>
        /// <param name="patID">档案号，传人null查询全部</param>
        /// <returns></returns>
        public DataTable SelectPatent(string patID)
        {
            try
            {
                if (m_SqlHelper == null)
                    thisLogin();
                DataTable dt = null;
                if (patID == null)
                    dt = m_SqlHelper.ExecuteDataTable("SELECT * FROM inpatient");
                else
                    dt = m_SqlHelper.ExecuteDataTable(string.Format("SELECT * FROM inpatient where noofinpat='{0}' ", patID));
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable SelectDataBase(string sql)
        {
            try
            {
                if (m_SqlHelper == null)
                    thisLogin();
                DataTable dt = m_SqlHelper.ExecuteDataTable(sql);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 插入患者档案
        /// </summary>
        /// <param name="patRow"></param>
        /// <returns></returns>
        public void InsertData(PatientEntity m_patientInfo, string edittype, int noofipat)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Insert into InPatient(PatNoOfHis,NoOfClinic,NoOfRecord,Name,PayID,Origin,InCount,SexID,Birth,Age," +
                "AgeStr,IDNO,Marital,JobID,Status,AttendLevel,Style,OutWardBed,PatID,ADMITDATE)");
            sb.AppendFormat(" values('{0}' ", m_patientInfo.PatNoOfHis);//PatNoOfHis
            sb.AppendFormat(" ,'{0}' ", m_patientInfo.NoOfcClinic);//NoOfClinic
            sb.AppendFormat(" ,'{0}' ", m_patientInfo.NoOfRecord);//NoOfRecord
            sb.AppendFormat(" ,'{0}' ", m_patientInfo.Name);//Name
            sb.AppendFormat(" ,'{0}' ", m_patientInfo.PayID);//PayID
            sb.AppendFormat(" ,'{0}' ", m_patientInfo.ORIGIN);//Origin
            sb.AppendFormat(" ,'{0}' ", m_patientInfo.InCount);//InCount
            sb.AppendFormat(" ,'{0}' ", m_patientInfo.SexID);//SexID
            sb.AppendFormat(" ,'{0}' ", m_patientInfo.Birth);//Birth
            sb.AppendFormat(" ,'{0}' ", m_patientInfo.Age);//Age
            sb.AppendFormat(" ,'{0}' ", m_patientInfo.AgeStr);//AgeStr
            sb.AppendFormat(" ,'{0}' ", m_patientInfo.IDNO);//IDNO
            sb.AppendFormat(" ,'{0}' ", m_patientInfo.Marital);//Marital
            sb.AppendFormat(" ,'{0}' ", m_patientInfo.JobID);//JobID
            sb.AppendFormat(" ,'{0}' ", 1501);//Status
            sb.AppendFormat(" ,'{0}' ", 12511);//AttendLevel
            sb.AppendFormat(" ,'{0}' ", 0);//Style
            sb.AppendFormat(" ,'{0}','{1}','{2}' )", 1, m_patientInfo.PatID, m_patientInfo.AdmitDate);//OutWardBed,m_patientInfo,AdmitDate

            try
            {
                //int id = 0;
                m_SqlHelper.ExecuteNoneQuery(sb.ToString());
                DataTable dt = m_SqlHelper.ExecuteDataTable(string.Format("SELECT * FROM inpatient where Name='{0}' ", "周媛媛"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 添加档案信息
        /// </summary>
        /// <param name="dtHisPat"></param>
        /// <param name="sb"></param>
        /// <param name="row"></param>
        /// <param name="pt"></param>
        public void InsertPatent(DataTable dtHisPat, StringBuilder sb, DataRow row, string pt)
        {
            sb.Append("Insert into InPatient(PatNoOfHis,NoOfClinic,NoOfRecord,Name,PayID,Origin,InCount,SexID,Birth,Age," +
                "NATIONID,NATIONALITYID,ORGANIZATION,OFFICEPLACE,OFFICETEL,NATIVEADDRESS,NATIVETEL," +
                "AgeStr,IDNO,Marital,JobID,Status,CRITICALLEVEL,AttendLevel,Style,OutWardBed,PatID," +
                "ADMITINFO,ADMITDEPT,ADMITWARD,ADMITBED,ADMITDATE,INWARDDATE,ADMITDIAGNOSIS," +
                "OUTHOSDEPT,OUTHOSWARD,OUTBED,OUTWARDDATE,OUTDIAGNOSIS,ADMITWAY," +
                "CLINICDOCTOR,ISBABY,MOTHER,ContactTEL)");

            sb.AppendFormat(" values('{0}' ", row["InID"]);//PatNoOfHis
            sb.AppendFormat(" ,N'{0}' ", row["InID"]);//NoOfClinic
            sb.AppendFormat(" ,N'{0}'", pt);//NoOfRecord
            sb.AppendFormat(" ,N'{0}' ", row["Name"]);//Name
            string patent = Convert.ToString(row["PatientTypeID"]);

            if (patent.Equals("086028000A000001"))
                sb.AppendFormat(" ,'{0}' ", 6);//PayID
            else if (patent.Equals("086028000A000002"))
                sb.AppendFormat(" ,'{0}' ", 1);//PayID
            else
                sb.AppendFormat(" ,'{0}' ", 1);//PayID
            sb.AppendFormat(" ,'{0}' ", 3);//Origin
            sb.AppendFormat(" ,'{0}' ", 1);//InCount
            if (Convert.ToString(row["SexID"]).Equals("086028000A000001"))
                sb.AppendFormat(" ,'{0}' ", 1);//SexID
            else
                sb.AppendFormat(" ,'{0}' ", 2);//SexID
            sb.AppendFormat(" ,N'{0}' ", Convert.ToDateTime(dtHisPat.Rows[0]["Birthday"]).ToString("yyyy-MM-dd HH:mm:ss"));//Birth
            sb.AppendFormat(" ,N'{0}' ", GetAgeNum(Convert.ToDateTime(dtHisPat.Rows[0]["Birthday"]), DateTime.Now));//Age
            sb.AppendFormat(" ,N'{0}' ", dtHisPat.Rows[0]["Email"]); // NATIONID
            sb.AppendFormat(" ,N'{0}' ", "cn");//NATIONALITYID
            sb.AppendFormat(" ,'{0}' ", ""); // ORGANIZATION
            sb.AppendFormat(" ,'{0}' ", ""); //OFFICEPLACE
            sb.AppendFormat(" ,N'{0}' ", dtHisPat.Rows[0]["Mobile"]);//OFFICETEL
            sb.AppendFormat(" ,N'{0}' ", dtHisPat.Rows[0]["Address"]);//NATIVEADDRESS
            sb.AppendFormat(" ,'{0}' ", "6");//NATIVETEL
            sb.AppendFormat(" ,N'{0}' ", DateTime.Now.Year - Convert.ToDateTime(dtHisPat.Rows[0]["Birthday"]).Year);//AgeStr
            sb.AppendFormat(" ,N'{0}' ", dtHisPat.Rows[0]["IdentityCard"]);//IDNO
            sb.AppendFormat(" ,'{0}' ", 1);//Marital
            sb.AppendFormat(" ,'{0}' ", "19");//JobID
            sb.AppendFormat(" ,'{0}' ", 1501);//Status
            sb.AppendFormat(" ,'{0}' ", 0);//CRITICALLEVEL
            sb.AppendFormat(" ,'{0}' ", 1);//AttendLevel
            sb.AppendFormat(" ,'{0}' ", 0);//Style 
            sb.AppendFormat(" ,'{0}'", 1);//OutWardBed,
            sb.AppendFormat(" ,'{0}'", pt);//PatID
            sb.AppendFormat(" ,'{0}' ", "3");//ADMITINFO
            sb.AppendFormat(" ,N'{0}' ", LengthBath(Convert.ToString(row["EmrID"]), 4));//ADMITDEPT
            sb.AppendFormat(" ,N'{0}' ", LengthBath(Convert.ToString(row["EmrID"]), 4));//ADMITWARD
            sb.AppendFormat(" ,N'{0}' ", row["BedOrder"]);//ADMITBED
            sb.AppendFormat(" ,N'{0}' ", Convert.ToDateTime(row["InTime"]).ToString("yyyy-MM-dd HH:mm:ss"));//ADMITDATE
            sb.AppendFormat(" ,N'{0}' ", Convert.ToDateTime(row["InTime"]).ToString("yyyy-MM-dd HH:mm:ss"));//INWARDDATE

            sb.AppendFormat(" ,N'{0}' ", dtHisPat.Rows[0]["InICO"]);//ADMITDIAGNOSIS
            sb.AppendFormat(" ,N'{0}' ", LengthBath(Convert.ToString(row["EmrID"]), 4));//OUTHOSDEPT
            sb.AppendFormat(" ,'{0}' ", LengthBath(Convert.ToString(row["EmrID"]), 4));//OUTHOSWARD 
            sb.AppendFormat(" ,N'{0}' ", row["BedOrder"]);//OUTBED
            if (!string.IsNullOrEmpty(Convert.ToString(row["OutTime"])))
                sb.AppendFormat(" ,'{0}' ", Convert.ToDateTime(row["OutTime"]).ToString("yyyy-MM-dd HH:mm:ss"));// OUTWARDDATE
            else
                sb.AppendFormat(" ,'{0}' ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sb.AppendFormat(" ,N'{0}' ", dtHisPat.Rows[0]["OutICO"]);//OUTDIAGNOSIS
            sb.AppendFormat(" ,'{0}' ", "3");//ADMITWAY
            sb.AppendFormat(" ,'{0}' ", "001538");//CLINICDOCTOR
            //sb.AppendFormat(" ,'{0}' ", "001011");//RESIDENT
            //sb.AppendFormat(" ,'{0}' ", "001011");//ATTEND
            //sb.AppendFormat(" ,'{0}' ", "000720");//CHIEF
            sb.AppendFormat(" ,'{0}' ", "0");//ISBABY
            sb.AppendFormat(" ,'{0}' ", "0");//MOTHER
            sb.AppendFormat(" ,'{0}' ", dtHisPat.Rows[0]["Mobile"]);//ContactTEL
            sb.Append(")");
            m_SqlHelper.ExecuteNoneQuery(sb.ToString());
        }

        /// <summary>
        /// 添加档案信息
        /// </summary>
        /// <param name="dtHisPat"></param>
        /// <param name="sb"></param>
        /// <param name="row"></param>
        /// <param name="pt"></param>
        public void updatePatent_Bed(DataTable dtHisPat, StringBuilder sb, DataRow row, string pt, string NoOfRecord)
        {
            sb.Append("update InPatient set  ");

            //  sb.AppendFormat(" values('{0}' ", row["InID"]);//PatNoOfHis
            //  sb.AppendFormat(" ,N'{0}' ", row["InID"]);//NoOfClinic
            //  sb.AppendFormat(" ,N'{0}'", pt);//NoOfRecord
            //   sb.AppendFormat(" ,N'{0}' ", row["Name"]);//Name
            //   string patent = Convert.ToString(row["PatientTypeID"]);


            sb.AppendFormat(" ADMITBED=N'{0}', ", row["BedOrder"]);//ADMITBED
            sb.AppendFormat(" OUTBED=N'{0}' ", row["BedOrder"]);//ADMITBED

            sb.AppendFormat(" where NoOfRecord='{0}' ", NoOfRecord);
            m_SqlHelper.ExecuteNoneQuery(sb.ToString());
        }

        /// <summary>
        /// 统计实足年龄
        /// </summary>
        /// <param name="birthday"></param>
        /// <param name="now"></param>
        /// <returns></returns>
        public int GetAgeNum(DateTime birthday, DateTime now)
        {
            DateTime a = birthday;
            DateTime b = now;
            int n = 0, y = 0, r = 0;
            int monthDay = 0;
            //int jun = 31, feb = 28, mar = 31, apr = 30, may = 31, jun = 30, jul = 31, aug = 31, sep = 30, oct = 31, nov = 30, dec = 31;          
            if (a.Month == 1)
            { monthDay = 31; }
            if (a.Month == 2)
            {
                if (a.Year % 4 == 0)
                { monthDay = 29; }
                else { monthDay = 28; }
            }
            if (a.Month == 3)
            { monthDay = 31; }
            if (a.Month == 4)
            { monthDay = 30; }
            if (a.Month == 5)
            { monthDay = 31; }
            if (a.Month == 6)
            { monthDay = 30; }
            if (a.Month == 7)
            { monthDay = 31; }
            if (a.Month == 8)
            { monthDay = 31; }
            if (a.Month == 9)
            { monthDay = 30; }
            if (a.Month == 10)
            { monthDay = 31; }
            if (a.Month == 11)
            { monthDay = 30; }
            if (a.Month == 12)
            { monthDay = 31; }
            if (b.Year > a.Year)
            {
                if (b.Month > a.Month)
                {
                    if (b.Day >= a.Day)
                    {
                        n = b.Year - a.Year;
                        y = b.Month - a.Month;
                        r = b.Day - a.Day;
                    }
                    if (b.Day < a.Day)
                    {
                        //年月都大，天数小
                        n = b.Year - a.Year;
                        y = b.Month - a.Month - 1;
                        r = b.Day + (monthDay - a.Day);
                    }
                }
                if (b.Month == a.Month)
                {
                    if (b.Day >= a.Day)
                    {
                        n = b.Year - a.Year;
                        y = b.Month - a.Month;
                        r = b.Day - a.Day;
                    }
                    if (b.Day < a.Day)
                    {
                        //年大月等，天数小
                        n = b.Year - a.Year - 1;
                        y = 11;
                        r = r = b.Day + (monthDay - a.Day);
                    }
                }
                else if (b.Month < a.Month)
                {
                    if (b.Day >= a.Day)
                    {
                        n = b.Year - a.Year - 1;
                        y = b.Month - a.Month + 12;
                        r = b.Day - a.Day;
                    }
                    if (b.Day < a.Day)
                    {
                        //年大月小，天数小
                        n = b.Year - a.Year - 1;
                        y = b.Month - a.Month + 12;
                        r = b.Day + (monthDay - a.Day);
                    }
                }
            }
            if (b.Year == a.Year)
            {
                if (b.Month > a.Month)
                {
                    if (b.Day >= a.Day)
                    {
                        n = b.Year - a.Year;
                        y = b.Month - a.Month;
                        r = b.Day - a.Day;
                    }
                    if (b.Day < a.Day)
                    {
                        //年等月大，天数小
                        n = b.Year - a.Year;
                        y = b.Month - a.Month - 1;
                        r = b.Day + (monthDay - a.Day);
                    }
                }
                if (b.Month == a.Month)
                {
                    if (b.Day >= a.Day)
                    {
                        n = b.Year - a.Year;
                        y = b.Month - a.Month;
                        r = b.Day - a.Day;
                    }
                    else if (b.Day < a.Day)
                    {
                        //年等月大，天数小
                        MessageBox.Show("选择日期大于当前日期!", "提示信息");
                        //throw new Exception("选择日期大于当前日期!");
                    }
                }
                else if (b.Month < a.Month)
                {
                    MessageBox.Show("选择日期大于当前日期!", "提示信息");
                    //throw new Exception("选择日期大于当前日期!");
                }
            }
            else if (b.Year < a.Year)
            {
                MessageBox.Show("选择日期大于当前日期!", "提示信息");
                //throw new Exception("选择日期大于当前日期!");
            }
            string age = Convert.ToString(n);
            string month = Convert.ToString(y);
            string day = Convert.ToString(r);
            //textBox1.Text = age;
            //textBox2.Text = month;
            //textBox3.Text = day;
            string suishu = null;
            if (n < 10)
            {
                if (y < 10)
                {
                    if (r < 10)
                    { suishu = "0" + age + "." + "0" + month + "0" + day; }
                    else if (r >= 10)
                    { suishu = "0" + age + "." + "0" + month + day; }
                }
                else if (y >= 10)
                {
                    if (r < 10)
                    { suishu = "0" + age + "." + month + "0" + day; }
                    else if (r >= 10)
                    { suishu = "0" + age + "." + month + day; }
                }
            }
            else if (n >= 10)
            {
                if (y < 10)
                {
                    if (r < 10)
                    { suishu = age + "." + "0" + month + "0" + day; }
                    else if (r >= 10)
                    { suishu = age + "." + "0" + month + day; }
                }
                else if (y >= 10)
                {
                    if (r < 10)
                    { suishu = age + "." + month + "0" + day; }
                    else if (r >= 10)
                    { suishu = age + "." + month + day; }
                }
            }
            return Convert.ToInt32(age);
        }
        /// <summary>
        /// 添加档案信息
        /// </summary>
        /// <param name="dtHisPat"></param>
        /// <param name="sb"></param>
        /// <param name="row"></param>
        /// <param name="pt"></param>
        public void UpdatePatent(DataTable dtHisPat, StringBuilder sb, DataRow row, string pt)
        {
            sb.Append("Update InPatient set ");
            sb.AppendFormat(" PatNoOfHis=N'{0}' ", row["InID"]);//PatNoOfHis
            sb.AppendFormat(" ,NoOfClinic=N'{0}' ", row["InID"]);//NoOfClinic
            sb.AppendFormat(" ,NoOfRecord=N'{0}'", pt);//NoOfRecord
            sb.AppendFormat(" ,Name=N'{0}' ", row["Name"]);//Name
            string patent = Convert.ToString(row["PatientTypeID"]);

            if (patent.Equals("086028000A000001"))
                sb.AppendFormat(" ,PayID='{0}' ", 6);//PayID
            else if (patent.Equals("086028000A000002"))
                sb.AppendFormat(" ,PayID='{0}' ", 1);//PayID
            else
                sb.AppendFormat(" ,PayID='{0}' ", 1);//PayID
            if (Convert.ToString(row["SexID"]).Equals("086028000A000001"))
                sb.AppendFormat(" ,SexID='{0}' ", 1);//SexID
            else
                sb.AppendFormat(" ,SexID='{0}' ", 2);//SexID
            sb.AppendFormat(" ,Birth=N'{0}' ", Convert.ToDateTime(dtHisPat.Rows[0]["Birthday"]).ToString("yyyy-MM-dd HH:mm:ss"));//Birth
            sb.AppendFormat(" ,Age=N'{0}' ", DateTime.Now.Year - Convert.ToDateTime(dtHisPat.Rows[0]["Birthday"]).Year);//Age
            sb.AppendFormat(" ,NATIONID=N'{0}' ", dtHisPat.Rows[0]["Email"]); // NATIONID
            sb.AppendFormat(" ,OFFICETEL=N'{0}' ", dtHisPat.Rows[0]["Mobile"]);//OFFICETEL
            sb.AppendFormat(" ,ContactTEL=N'{0}' ", dtHisPat.Rows[0]["Mobile"]);//ContactTEL
            sb.AppendFormat(" ,NATIVEADDRESS=N'{0}' ", dtHisPat.Rows[0]["Address"]);//NATIVEADDRESS
            sb.AppendFormat(" ,AgeStr=N'{0}' ", DateTime.Now.Year - Convert.ToDateTime(dtHisPat.Rows[0]["Birthday"]).Year);//AgeStr
            sb.AppendFormat(" ,IDNO=N'{0}' ", dtHisPat.Rows[0]["IdentityCard"]);//IDNO
            sb.AppendFormat(" ,PatID='{0}'", pt);//PatID
            sb.AppendFormat(" ,ADMITDEPT=N'{0}' ", LengthBath(Convert.ToString(row["EmrID"]), 4));//ADMITDEPT
            sb.AppendFormat(" ,ADMITWARD=N'{0}' ", LengthBath(Convert.ToString(row["EmrID"]), 4));//ADMITWARD
            sb.AppendFormat(" ,ADMITBED=N'{0}' ", row["BedOrder"]);//ADMITBED
            sb.AppendFormat(" ,ADMITDATE=N'{0}' ", Convert.ToDateTime(row["InTime"]).ToString("yyyy-MM-dd HH:mm:ss"));//ADMITDATE
            sb.AppendFormat(" ,INWARDDATE=N'{0}' ", Convert.ToDateTime(row["InTime"]).ToString("yyyy-MM-dd HH:mm:ss"));//INWARDDATE
            sb.AppendFormat(" ,ADMITDIAGNOSIS=N'{0}' ", dtHisPat.Rows[0]["InICO"]);//ADMITDIAGNOSIS
            sb.AppendFormat(" ,OUTHOSDEPT=N'{0}' ", LengthBath(Convert.ToString(row["EmrID"]), 4));//OUTHOSDEPT
            sb.AppendFormat(" ,OUTHOSWARD='{0}' ", LengthBath(Convert.ToString(row["EmrID"]), 4));//OUTHOSWARD 
            sb.AppendFormat(" ,OUTBED=N'{0}' ", row["BedOrder"]);//OUTBED
            if (!string.IsNullOrEmpty(Convert.ToString(row["OutTime"])))
                sb.AppendFormat(" ,OUTWARDDATE='{0}' ", Convert.ToDateTime(row["OutTime"]).ToString("yyyy-MM-dd HH:mm:ss"));// OUTWARDDATE
            else
                sb.AppendFormat(" ,OUTWARDDATE='{0}' ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sb.AppendFormat(" ,OUTDIAGNOSIS=N'{0}' ", dtHisPat.Rows[0]["OutICO"]);//OUTDIAGNOSIS
            sb.AppendFormat(" ,ADMITWAY='{0}' ", "3");//ADMITWAY
            sb.AppendFormat(" ,CLINICDOCTOR='{0}' ", "001538");//CLINICDOCTOR
            sb.AppendFormat("where PatNoOfHis='{0}'", row["InID"]);
            m_SqlHelper.ExecuteNoneQuery(sb.ToString());
        }

        public void OutPat(string bedOrder)
        {
            string sqlStr = string.Format("Update InPatient set Status='1503' where OutBed='{0}' ", bedOrder);
            m_SqlHelper.ExecuteNoneQuery(sqlStr);
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
    }
}