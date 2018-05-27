using DrectSoft.Common.Eop;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DrectSoft.Core.MainEmrPad
{
    /// <summary>
    /// xll 20130312 抽取出来的个性化的调用pacs方式
    /// </summary>
    public static class PACSOutSide
    {

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
                if (valuestr == "dll")  //dll调用方式
                {
                    PacsDll(currinpatient);
                }
                else if (valuestr == "exe") //exe调用方式
                {
                    PacsExe(currinpatient);
                }
                else if (valuestr == "url") //url浏览方式
                {
                    PacsUrl(currinpatient.NoOfHisFirstPage);
                }
                else
                {
                    MessageBox.Show("没有对接PSCS接口！");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// exe调用
        /// </summary>
        private static void PacsExe(Inpatient currinpatient)
        {
            try
            {
                string fileName = Application.StartupPath + "\\Pacs调取报告通用接口\\GoldHISManager.exe";
                bool hasFile = File.Exists(fileName);
                if (hasFile)
                {
                    //xll 接口调用方式发生变化 修改2012-12-18
                    string infostr = string.Format(@"G_study.inhospitalno='{0}'", currinpatient.NoOfHisFirstPage);
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
        /// url调用pacs方式
        /// </summary>
        /// <param name="noofhis"></param>
        private static void PacsUrl(string noofhis)
        {
            try
            {
                string temppacsUrl = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("PacsUrl");
                string pacsUrl = string.Format(temppacsUrl, noofhis);
                System.Diagnostics.Process.Start("IEXPLORE.EXE", pacsUrl);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region PACS相关
        [DllImport("joint.dll")]
        public static extern int PacsView(int nPatientType, string lpszID, int nImageType); //Pacs调阅3.1版
        [DllImport("joint.dll")]
        public static extern bool PacsViewByPatientInfo(int nType, string str, int nPatientType);

        /// <summary>
        /// dll调用方式
        /// </summary>
        public static void PacsDll(Inpatient m_CurrentInpatient)
        {
            try
            {
                string HospitalNo = m_CurrentInpatient.RecordNoOfHospital;//住院号
                int nPatientType = 2;//患者类型（1.门诊号 2.住院号）
                int LookType = 1;//类型（1.图像 2.报告）

                if (CheckPackIsExist())
                {
                    try
                    {
                        if (PacsView(nPatientType, m_CurrentInpatient.RecordNoOfHospital, LookType) != 1)
                        {
                            MessageBox.Show("调用失败");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 判断调用PACS的DLL是否存在 
        /// </summary>
        private static bool CheckPackIsExist()
        {
            try
            {
                string jointpath = Application.StartupPath + @"\joint.dll";//获取程序所在文件夹
                if (!File.Exists(jointpath))//不存在此接口DLL就不执行 
                {
                    return false;
                }
                string connectpath = Application.StartupPath + @"\Connection.dll";//获取程序所在文件夹
                if (!File.Exists(connectpath))//不存在此接口DLL就不执行 
                {
                    return false;
                }
                string pacspath = Application.StartupPath + @"\PACSID.dll";//获取程序所在文件夹
                if (!File.Exists(pacspath))//不存在此接口DLL就不执行 
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion


    }
}
