using System;
using System.IO;

namespace DrectSoft
{
    public class IO
    {
        public void WriteLog(string Msg)
        {
            string filename = DateTime.Now.ToString("yyyyMMdd") + ".log";
            string WebPath = AppDomain.CurrentDomain.BaseDirectory + @"\log\" + filename;
            File.AppendAllText(WebPath, Msg + "\r\n");
        }
    }
}