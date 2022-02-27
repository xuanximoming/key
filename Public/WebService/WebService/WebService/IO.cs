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

        public void WriteImage(byte[] image, string filename)
        {
            string folder = AppDomain.CurrentDomain.BaseDirectory;
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            if (!Directory.Exists(folder + "Images\\"))
            {
                Directory.CreateDirectory(folder + "Images\\");
            }
            string photoUrl = folder + "Images\\" + filename + ".jpg";
            FileStream fs = new FileStream(photoUrl, FileMode.OpenOrCreate, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.BaseStream.Write(image, 0, image.Length);
            bw.Flush();
            bw.Close();
        }
    }
}