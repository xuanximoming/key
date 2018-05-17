using System.Runtime.InteropServices;
using System.Text;

namespace AutoUpdate
{
    public class IniFiles
    {
        public string path;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public IniFiles(string INIPath)
        {
            this.path = INIPath;
        }

        public void IniWritevalue(string Section, string Key, string value)
        {
            IniFiles.WritePrivateProfileString(Section, Key, value, this.path);
        }

        public string IniReadvalue(string Section, string Key)
        {
            StringBuilder stringBuilder = new StringBuilder(255);
            int privateProfileString = IniFiles.GetPrivateProfileString(Section, Key, "", stringBuilder, 255, this.path);
            return stringBuilder.ToString();
        }
    }
}
