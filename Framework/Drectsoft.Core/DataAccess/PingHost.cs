using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DrectSoft.Core
{
    class PingHost
    {
        /// <summary>
        /// 判断是否能PING通数据库服务器
        /// </summary>
        /// <param name="strIp"></param>
        /// <returns></returns>
        public static bool CmdPing(string strIp)
        {
            bool isPingThrough = false;
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.WriteLine("ping -n 1 " + strIp);
            p.StandardInput.WriteLine("exit");
            string strRst = p.StandardOutput.ReadToEnd();
            if (strRst.IndexOf("(0%") != -1)
                isPingThrough = true;
            else
                isPingThrough = false;
            p.Close();
            return isPingThrough;
        }
    }
}
