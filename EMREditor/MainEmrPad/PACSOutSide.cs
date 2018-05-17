using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using DrectSoft.Common.Eop;
using System.Runtime.InteropServices;

namespace DrectSoft.Core.MainEmrPad
{
    /// <summary>
    /// xll 20130312 抽取出来的个性化的调用pacs方式
    /// </summary>
    public static class PACSOutSide
    {
        /// <summary>
        /// 武汉十三医院调用pacs方法
        /// </summary>
        private static void PacsWHSS(string noofHis)
        {
            try
            {
                string fileName = Application.StartupPath + "\\Pacs调取报告通用接口\\GoldHISManager.exe";
                bool hasFile = File.Exists(fileName);
                if (hasFile)
                {
                    //xll 接口调用方式发生变化 修改2012-12-18
                    string infostr = string.Format(@"G_study.inhospitalno='{0}'", noofHis);
                    Process.Start(fileName, infostr);
                }
                else
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("Pacs路径不存在");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 对外PACS调用方法 
        /// </summary>
        /// <param name="currinpatient"></param>
        public static void PacsAll(Inpatient currinpatient)
        {
            try
            {
                string valuestr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("PACSRevision");
                valuestr = valuestr.ToLower();
                if (valuestr == "whssyy")  //武汉十三医院
                {
                    PacsWHSS(currinpatient.NoOfHisFirstPage);
                }
                else if (valuestr == "dongfang")
                {
                    PACSDongFang(currinpatient.NoOfHisFirstPage);
                }
                //新增的處理大連六院的PACS調閱 add by 楊偉康 2013年7月15日 11:06:26
                else if (valuestr == "dlly")
                {
                    PACSDaLianHosp(currinpatient.NoOfHisFirstPage);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #region 大连六院调用PACS 相关  add by ywk

        //查看报告及阅片
        //若HIS的一个字段就能唯一标识一个检查，则按下列方式传入参数。
        //HisCode1为HIS的能唯一标识一个检查的号，如申请单号。
        //HisCode2传入空字符;
        // 若HIS的两个字段不能唯一标识一个检查，则需同时传入这两个字段的值。
        [DllImport("PacsReport.dll")]
        public static extern void ShowPACSReport(string HisCode1, string HisCode2);
        [DllImport("PacsReport.dll")]
        public static extern void UnLoadPACSReport();
        /// <summary>
        /// 大連六院的调用PACS 
        ///  add by ywk 二〇一三年七月十五日 11:07:16 
        /// </summary>
        /// <param name="patnoofhis">HIS与EMR连接住院号</param>
        private static void PACSDaLianHosp(string patnoofhis)
        {
            string pacsdllpath = Application.StartupPath + @"\PacsReport.dll";
            if (!File.Exists(pacsdllpath))//不存在此接口DLL就不执行 
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("Pacs需调用DLL路径不存在");
                return;
            }
            //进行显示PACS的程序
            try
            {
                ShowPACSReport(patnoofhis, "");
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("调用PACS出错，错误信息为：" + ex.Message);
                UnLoadPACSReport();
                return;
            }
            //进程退出再调用 
            //finally
            //{
            //    UnLoadPACSReport();
            //}

        }
        #endregion
        /// <summary>
        /// 东方医院调用pacs方式
        /// </summary>
        /// <param name="noofhis"></param>
        private static void PACSDongFang(string noofhis)
        {
            try
            {
                string temppacsUrl = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("DongFangPACSUrl");
                string pacsUrl = string.Format(temppacsUrl, noofhis);
                System.Diagnostics.Process.Start("IEXPLORE.EXE", pacsUrl);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
