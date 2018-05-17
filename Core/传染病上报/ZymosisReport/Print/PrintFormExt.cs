using DrectSoft.Common.Ctrs.FORM;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace DrectSoft.Core.ZymosisReport.Print
{
    public partial class PrintFormExt : DevBaseForm
    {
        string m_reportId;
        string m_reportType;

        /// <summary>
        /// xll 20130820 新版传染病打印界面
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="reportTypeId"></param>
        public PrintFormExt(string reportId, string reportTypeId)
        {
            try
            {
                m_reportId = reportId;
                m_reportType = reportTypeId;
                InitializeComponent();

                XDesigner.Report.XPrintControlExt.ControlButtons butts = xPrint.Buttons;
                butts.MainToolStrip.Visible = true;
                butts.cmdExport.Visible = false;
                butts.cmdRefresh.Visible = false;
                butts.lblVersion2.Visible = false;
                butts.btnPageSettings.Visible = false;
                butts.cmdJumpPrint.Visible = true;
                butts.cmdPrint.Visible = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            // InitDate();
        }

        public void InitDate()
        {
            try
            {
                ZymosisEntity zymosisEntity = GetZymosisEntity(m_reportId);
                XDesigner.Report.DataBaseReportBuilder builder = new XDesigner.Report.DataBaseReportBuilder();
                builder.SetVariable("ZymosisEntity", new object[] { zymosisEntity });

                XDesigner.Report.DataBaseReportBuilder builder1 = new XDesigner.Report.DataBaseReportBuilder();
                builder1.SetVariable("ZymosisEntity", new object[] { zymosisEntity });

                string path = Application.StartupPath.ToString();
                path += @"\CRBReport\CRBreport.xrp";
                string path1 = Application.StartupPath.ToString();
                path1 += @"\CRBReport\" + m_reportType + @".xrp";

                bool hasfile = (File.Exists(path));
                if (hasfile)
                {
                    builder.Load(path);
                    builder.Refresh();
                    xPrint.Documents.Add(builder.ReportDocument);
                    if (File.Exists(path1))
                    {
                        builder1.Load(path1);
                        builder1.Refresh();
                        xPrint.Documents.Add(builder1.ReportDocument);
                    }
                    xPrint.RefreshView();
                }
                else
                {
                    MessageBox.Show("模板不存在");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ZymosisEntity GetZymosisEntity(string reportID)
        {
            try
            {
                ZymosisEntity zymosisEntity = new Print.ZymosisEntity();
                string sqlStr = string.Format(@"select * from zymosis_report where report_id='{0}'", reportID);
                DataTable dt = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sqlStr, CommandType.Text);
                zymosisEntity = DrectSoft.Common.DataTableToList<ZymosisEntity>.ConvertToModelOne(dt);

                string sql = @"select icd,name,level_id from  zymosis_diagnosis  where valid='1'";
                DataTable dtDia = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sql, CommandType.Text);
                foreach (DataRow item in dtDia.Rows)
                {
                    string gouxuanStr = "  ";
                    if (item["ICD"].ToString() == zymosisEntity.DIAGICD10)
                    {
                        gouxuanStr = "☑";
                    }
                    else
                    {
                        gouxuanStr = "□";
                    }

                    if (item["LEVEL_ID"].ToString() == "1")
                    {
                        zymosisEntity.JiaLei += gouxuanStr + item["NAME"].ToString() + "、";
                    }
                    else if (item["LEVEL_ID"].ToString() == "2")
                    {
                        zymosisEntity.YiLei += gouxuanStr + item["NAME"].ToString() + "、";
                    }
                    else if (item["LEVEL_ID"].ToString() == "3")
                    {
                        zymosisEntity.BinLei += gouxuanStr + item["NAME"].ToString() + "、";
                    }
                    else if (item["LEVEL_ID"].ToString() == "9" && item["ICD"].ToString() == zymosisEntity.DIAGICD10)
                    {
                        zymosisEntity.OtherLei = item["NAME"].ToString();
                    }
                    else if (!string.IsNullOrEmpty(zymosisEntity.OTHERDIAG))
                    {
                        zymosisEntity.OtherLei = zymosisEntity.OTHERDIAG;
                    }
                }
                if (zymosisEntity.JiaLei.Length > 0)
                {
                    zymosisEntity.JiaLei = zymosisEntity.JiaLei.Substring(0, zymosisEntity.JiaLei.Length - 1);
                }
                if (zymosisEntity.YiLei.Length > 0)
                {
                    zymosisEntity.YiLei = zymosisEntity.YiLei.Substring(0, zymosisEntity.YiLei.Length - 1);
                }
                if (zymosisEntity.BinLei.Length > 0)
                {
                    zymosisEntity.BinLei = zymosisEntity.BinLei.Substring(0, zymosisEntity.BinLei.Length - 1);
                }

                if (zymosisEntity.REPORT_TYPE == "1")
                {
                    zymosisEntity.FirstBaoGao = "√";
                }
                else if (zymosisEntity.REPORT_TYPE == "2")
                {
                    zymosisEntity.AgainBaoGao = "√";
                }

                if (zymosisEntity.SEX == "1")
                {
                    zymosisEntity.SexNan = "√";
                }
                else if (zymosisEntity.SEX == "2")
                {
                    zymosisEntity.SexNv = "√";
                }

                if (zymosisEntity.AGE_UNIT == "1")
                { zymosisEntity.AgeSui = "√"; }
                else if (zymosisEntity.AGE_UNIT == "2")
                { zymosisEntity.AgeYue = "√"; }
                else if (zymosisEntity.AGE_UNIT == "3")
                { zymosisEntity.AgeTian = "√"; }

                if (zymosisEntity.ADDRESSTYPE == "1")
                {
                    zymosisEntity.AddBenXian = "√";
                }
                else if (zymosisEntity.ADDRESSTYPE == "2")
                { zymosisEntity.AddBenXianWai = "√"; }
                else if (zymosisEntity.ADDRESSTYPE == "3")
                { zymosisEntity.AddBenSheng = "√"; }
                else if (zymosisEntity.ADDRESSTYPE == "4")
                { zymosisEntity.AddBenShengWai = "√"; }
                else if (zymosisEntity.ADDRESSTYPE == "5")
                { zymosisEntity.AddGanAoTai = "√"; }
                else if (zymosisEntity.ADDRESSTYPE == "6")
                { zymosisEntity.AddWaiJi = "√"; }


                string jobid = zymosisEntity.JOBID;
                PropertyInfo property = zymosisEntity.GetType().GetProperty("JobItem" + jobid);
                if (property != null)
                {
                    property.SetValue(zymosisEntity, "√", null);
                }
                PropertyInfo property1 = zymosisEntity.GetType().GetProperty("RecordType1item" + zymosisEntity.RECORDTYPE1);
                if (property1 != null)
                {
                    property1.SetValue(zymosisEntity, "√", null);
                }

                PropertyInfo property2 = zymosisEntity.GetType().GetProperty("RecordType2item" + zymosisEntity.RECORDTYPE2);
                if (property2 != null)
                {
                    property2.SetValue(zymosisEntity, "√", null);
                }

                if (!string.IsNullOrEmpty(zymosisEntity.DIAGDATE))
                {
                    zymosisEntity.DIAGDATE = Convert.ToDateTime(zymosisEntity.DIAGDATE).ToString("yyyy-MM-dd");
                }
                if (!string.IsNullOrEmpty(zymosisEntity.AUDIT_DATE))
                {
                    zymosisEntity.AUDIT_DATE = Convert.ToDateTime(zymosisEntity.AUDIT_DATE).ToString("yyyy-MM-dd");
                }
                if (zymosisEntity.INFECTOTHER_FLAG == "1")
                {
                    zymosisEntity.INFECTOTHER_FLAG = "有";
                }
                else if (zymosisEntity.INFECTOTHER_FLAG == "0")
                {
                    zymosisEntity.INFECTOTHER_FLAG = "无";
                }

                if (m_reportType == "1")   //艾滋病
                {
                    HIVEntity(zymosisEntity);
                }
                else if (m_reportType == "2")  //乙肝HBV
                {
                    HBVEntity(zymosisEntity);
                }
                else if (m_reportType == "3")  //H1N1
                {
                    H1N1Entity(zymosisEntity);
                }
                else if (m_reportType == "5")
                {
                    HFMDEntity(zymosisEntity);
                }
                else if (m_reportType == "6")
                {
                    AFPEntity(zymosisEntity);
                }
                else if (m_reportType == "7")   //尖锐湿疣 4和7合并处理
                {
                    JRSYEntity(zymosisEntity);
                }
                else if (m_reportType == "8")  //生殖道沙眼衣原体感染
                {
                    SZDYYTEntity(zymosisEntity);
                }
                return zymosisEntity;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void AFPEntity(ZymosisEntity zymosisEntity)
        {
            try
            {
                string sqlAFPStr = string.Format("select * from zymosis_afp z where z.vaild='1' and z.reportid='{0}'", m_reportId);
                DataTable dtAFP = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sqlAFPStr, CommandType.Text);

                zymosisEntity.ZYMOSISAFP = DrectSoft.Common.DataTableToList<ZYMOSISAFPEntity>.ConvertToModelOne(dtAFP);
                zymosisEntity.ZYMOSISAFP.Householdscope = GeiShiDate(zymosisEntity.ZYMOSISAFP.Householdscope);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void HFMDEntity(ZymosisEntity zymosisEntity)
        {
            try
            {
                string sqlHFMDStr = string.Format("select * from zymosis_hfmd z where z.vaild='1' and z.reportid='{0}'", m_reportId);
                DataTable dtHFMD = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sqlHFMDStr, CommandType.Text);
                zymosisEntity.ZYMOSISHFMD = DrectSoft.Common.DataTableToList<ZYMOSISHFMDEntity>.ConvertToModelOne(dtHFMD);
                zymosisEntity.ZYMOSISHFMD.Issevere = GeiShiDate(zymosisEntity.ZYMOSISHFMD.Issevere);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //8.生殖道沙眼衣原体感染
        private void SZDYYTEntity(ZymosisEntity zymosisEntity)
        {
            try
            {
                string sqlSZDYYTtr = string.Format("select * from zymosis_szdyyt z where z.vaild='1' and z.report_id='{0}'", m_reportId);
                DataTable dtSZDYYT = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sqlSZDYYTtr, CommandType.Text);
                zymosisEntity.ZYMOSISSZDYYT = DrectSoft.Common.DataTableToList<ZYMOSISSZDYYTEntity>.ConvertToModelOne(dtSZDYYT);
                zymosisEntity.ZYMOSISSZDYYT.MARITALSTATUS = GeiShiDate(zymosisEntity.ZYMOSISSZDYYT.MARITALSTATUS);
                zymosisEntity.ZYMOSISSZDYYT.NATION = GeiShiDate(zymosisEntity.ZYMOSISSZDYYT.NATION);
                zymosisEntity.ZYMOSISSZDYYT.CULTURESTATE = GeiShiDate(zymosisEntity.ZYMOSISSZDYYT.CULTURESTATE);
                zymosisEntity.ZYMOSISSZDYYT.HOUSEHOLDADDRESS = GeiShiDate(zymosisEntity.ZYMOSISSZDYYT.HOUSEHOLDADDRESS);
                zymosisEntity.ZYMOSISSZDYYT.SYYYTGR = GeiShiDate(zymosisEntity.ZYMOSISSZDYYT.SYYYTGR);
                zymosisEntity.ZYMOSISSZDYYT.CONTACTHISTORY = GeiShiDate(zymosisEntity.ZYMOSISSZDYYT.CONTACTHISTORY);
                zymosisEntity.ZYMOSISSZDYYT.VENERISMHISTORY = GeiShiDate(zymosisEntity.ZYMOSISSZDYYT.VENERISMHISTORY);
                zymosisEntity.ZYMOSISSZDYYT.INFACTWAY = GeiShiDate(zymosisEntity.ZYMOSISSZDYYT.INFACTWAY);
                zymosisEntity.ZYMOSISSZDYYT.SAMPLESOURCE = GeiShiDate(zymosisEntity.ZYMOSISSZDYYT.SAMPLESOURCE);
                zymosisEntity.ZYMOSISSZDYYT.DETECTIONCONCLUSION = GeiShiDate(zymosisEntity.ZYMOSISSZDYYT.DETECTIONCONCLUSION);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 7.尖锐湿疣则弹出下列信息框：说明： 生殖器疱疹同尖锐湿疣一样
        /// </summary>
        /// <param name="zymosisEntity"></param>
        private void JRSYEntity(ZymosisEntity zymosisEntity)
        {
            try
            {
                string sqlJRSYStr = string.Format("select * from zymosis_jrsy z where z.vaild='1' and z.report_id='{0}'", m_reportId);
                DataTable dtJRSY = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sqlJRSYStr, CommandType.Text);

                zymosisEntity.ZYMOSISJRSY = DrectSoft.Common.DataTableToList<ZYMOSISJRSYEntity>.ConvertToModelOne(dtJRSY);

                zymosisEntity.ZYMOSISJRSY.MARITALSTATUS = GeiShiDate(zymosisEntity.ZYMOSISJRSY.MARITALSTATUS);
                zymosisEntity.ZYMOSISJRSY.NATION = GeiShiDate(zymosisEntity.ZYMOSISJRSY.NATION);
                zymosisEntity.ZYMOSISJRSY.CULTURESTATE = GeiShiDate(zymosisEntity.ZYMOSISJRSY.CULTURESTATE);
                zymosisEntity.ZYMOSISJRSY.HOUSEHOLDADDRESS = GeiShiDate(zymosisEntity.ZYMOSISJRSY.HOUSEHOLDADDRESS);

                zymosisEntity.ZYMOSISJRSY.CONTACTHISTORY = GeiShiDate(zymosisEntity.ZYMOSISJRSY.CONTACTHISTORY);
                zymosisEntity.ZYMOSISJRSY.VENERISMHISTORY = GeiShiDate(zymosisEntity.ZYMOSISJRSY.VENERISMHISTORY);
                zymosisEntity.ZYMOSISJRSY.INFACTWAY = GeiShiDate(zymosisEntity.ZYMOSISJRSY.INFACTWAY);
                zymosisEntity.ZYMOSISJRSY.SAMPLESOURCE = GeiShiDate(zymosisEntity.ZYMOSISJRSY.SAMPLESOURCE);
                zymosisEntity.ZYMOSISJRSY.DETECTIONCONCLUSION = GeiShiDate(zymosisEntity.ZYMOSISJRSY.DETECTIONCONCLUSION);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void H1N1Entity(ZymosisEntity zymosisEntity)
        {
            try
            {
                string sqlH1N1str = string.Format("select * from zymosis_h1n1 z where z.vaild='1' and z.reportid='{0}'", m_reportId);
                DataTable dtH1N1 = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sqlH1N1str, CommandType.Text);
                zymosisEntity.ZYMOSISH1N1 = DrectSoft.Common.DataTableToList<ZYMOSISH1N1Entity>.ConvertToModelOne(dtH1N1);
                zymosisEntity.ZYMOSISH1N1.CASETYPE = GeiShiDate(zymosisEntity.ZYMOSISH1N1.CASETYPE);
                zymosisEntity.ZYMOSISH1N1.ISOVERSEAS = GeiShiDate(zymosisEntity.ZYMOSISH1N1.ISOVERSEAS);
                //zymosisEntity.ZYMOSISH1N1.InHosDate = GetH1N1HosDate(zymosisEntity.ZYMOSISH1N1.HOSPITALSTATUS, 2);
                //zymosisEntity.ZYMOSISH1N1.OutHosDate = GetH1N1HosDate(zymosisEntity.ZYMOSISH1N1.HOSPITALSTATUS, 3);
                //zymosisEntity.ZYMOSISH1N1.HOSPITALSTATUS = GetH1N1HosDate(zymosisEntity.ZYMOSISH1N1.HOSPITALSTATUS, 1);
                zymosisEntity.ZYMOSISH1N1.InHosDate = GetH1N1Date(zymosisEntity.ZYMOSISH1N1.HOSPITALSTATUS, 2);
                //edit by ck 2013-8-22
                zymosisEntity.ZYMOSISH1N1.OutHosDate = GetH1N1Date(zymosisEntity.ZYMOSISH1N1.HOSPITALSTATUS, 3);
                zymosisEntity.ZYMOSISH1N1.HOSPITALSTATUS = GetH1N1Date(zymosisEntity.ZYMOSISH1N1.HOSPITALSTATUS, 1);

                zymosisEntity.ZYMOSISH1N1.ZhiyuDate = GetZhiyuDate(zymosisEntity.ZYMOSISH1N1.ISCURE, 2);
                zymosisEntity.ZYMOSISH1N1.ISCURE = GetZhiyuDate(zymosisEntity.ZYMOSISH1N1.ISCURE, 1);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private string GetZhiyuDate(string iscure, int index)
        {
            if (string.IsNullOrEmpty(iscure)) return "";
            string[] strs = iscure.Split(';', '；');
            if (index == 1 && strs != null && strs.Length > 0)
            {
                return GeiShiDate(strs[0]);
            }
            else if (index == 2 && strs != null && strs.Length >= 2)
            {
                return GeiShiDate(strs[1]);
            }
            else
            {
                return "";
            }

        }

        /// <summary>
        /// 获取H1N1是否住院信息
        /// </summary>
        /// <param name="str"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private string GetH1N1Date(string str, int index)
        {
            try
            {
                if (string.IsNullOrEmpty(str)) return "";
                if (index.ToString() == "1")
                {
                    if (str.Contains(";"))
                    {
                        string str1 = str.Split(';')[0].Split(',')[1];
                        return str1;
                    }
                    else
                    {
                        string str1 = str.Split(',')[1];
                        return str1;
                    }
                }
                else if (index.ToString() == "2")
                {
                    if (str.Contains(";"))
                    {
                        string str2 = str.Split(';')[1].Split(',')[1];
                        return str2;
                    }
                    else
                    {
                        return "";
                    }
                }
                else if (index.ToString() == "3")
                {
                    if (str.Contains(";"))
                    {
                        string[] str3 = str.Split(';');
                        if (str3.Length > 2)
                        {
                            string str33 = str3[2].Split(',')[1];
                            return str33;
                        }
                    }
                    return "";
                }
                return "";
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 获取h1ni住院天数
        /// </summary>
        /// <param name="AllStr"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private string GetH1N1HosDate(string AllStr, int index)
        {
            try
            {
                if (string.IsNullOrEmpty(AllStr)) return "";
                string[] strs = AllStr.Split('；', ';');
                if (index == 1)
                {
                    if (strs != null && strs.Length >= 2)
                    {
                        return strs[1];
                    }
                    return "";
                }
                else if (index == 2)
                {
                    if (strs != null && strs.Length >= 4)
                    {
                        return strs[3];
                    }
                    return "";
                }
                else if (index == 3)
                {
                    if (strs != null && strs.Length >= 6)
                    {
                        return strs[5];
                    }
                    return "";
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



        private void HBVEntity(ZymosisEntity zymosisEntity)
        {
            try
            {
                string sqlHBVstr = string.Format("select * from zymosis_hbv z where z.vaild='1' and z.reportid='{0}'", m_reportId);
                DataTable dtHBV = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sqlHBVstr, CommandType.Text);
                zymosisEntity.ZYMOSISHBV = DrectSoft.Common.DataTableToList<ZYMOSISHBVEntity>.ConvertToModelOne(dtHBV);

                zymosisEntity.ZYMOSISHBV.HBSAGDATE = GeiShiDate(zymosisEntity.ZYMOSISHBV.HBSAGDATE);

                zymosisEntity.ZYMOSISHBV.FRISTDATE = GeiShiDate(zymosisEntity.ZYMOSISHBV.FRISTDATE);
                zymosisEntity.ZYMOSISHBV.ANTIHBC = GeiShiDate(zymosisEntity.ZYMOSISHBV.ANTIHBC);
                zymosisEntity.ZYMOSISHBV.LIVERBIOPSY = GeiShiDate(zymosisEntity.ZYMOSISHBV.LIVERBIOPSY);
                zymosisEntity.ZYMOSISHBV.RECOVERYHBSAG = GeiShiDate(zymosisEntity.ZYMOSISHBV.RECOVERYHBSAG);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void HIVEntity(ZymosisEntity zymosisEntity)
        {
            try
            {
                string sqlHivstr = string.Format("select * from zymosis_hiv z where z.vaild='1' and z.report_id='{0}'", m_reportId);
                DataTable dtHiv = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sqlHivstr, CommandType.Text);
                zymosisEntity.ZymosisHIV = DrectSoft.Common.DataTableToList<ZymosisHIVEntity>.ConvertToModelOne(dtHiv);
                zymosisEntity.ZymosisHIV.Maritalstatus = GeiShiDate(zymosisEntity.ZymosisHIV.Maritalstatus);
                zymosisEntity.ZymosisHIV.Nation = GeiShiDate(zymosisEntity.ZymosisHIV.Nation);
                zymosisEntity.ZymosisHIV.Culturestate = GeiShiDate(zymosisEntity.ZymosisHIV.Culturestate);
                zymosisEntity.ZymosisHIV.Householdscope = GeiShiDate(zymosisEntity.ZymosisHIV.Householdscope);
                zymosisEntity.ZymosisHIV.Contacthistory = GeiShiDate(zymosisEntity.ZymosisHIV.Contacthistory);
                zymosisEntity.ZymosisHIV.Venerismhistory = GeiShiDate(zymosisEntity.ZymosisHIV.Venerismhistory);
                zymosisEntity.ZymosisHIV.Infactway = GeiShiDate(zymosisEntity.ZymosisHIV.Infactway);
                zymosisEntity.ZymosisHIV.Samplesource = GeiShiDate(zymosisEntity.ZymosisHIV.Samplesource);
                zymosisEntity.ZymosisHIV.Detectionconclusion = GeiShiDate(zymosisEntity.ZymosisHIV.Detectionconclusion);
                zymosisEntity.ZymosisHIV.Alikesymbol = GeiShiDate(zymosisEntity.ZymosisHIV.Alikesymbol);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 格式化数据
        /// </summary>
        /// <returns></returns>
        private string GeiShiDate(string dateStr)
        {
            if (string.IsNullOrEmpty(dateStr))
            {
                return "";
            }
            string StrValue = "";
            string[] strList1 = dateStr.Split(';', '；');
            foreach (var item in strList1)
            {
                string[] strList2 = item.Split('，', ',');
                if (strList2 != null && strList2.Length == 2)
                {
                    StrValue += strList2[1] + "、";
                }
            }
            if (StrValue.Length > 0)
            {
                StrValue = StrValue.Substring(0, StrValue.Length - 1);
            }
            return StrValue;
        }

        private void xPrint_Load(object sender, EventArgs e)
        {
            try
            {
                InitDate();
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    InitDate();
        //}
    }
}