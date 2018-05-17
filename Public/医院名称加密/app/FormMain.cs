using Oracle.DataAccess.Client;
using System;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace app
{
    public partial class FormMain : Form
    {
        DbConect dbconn = new DbConect();
        OracleParameter[] cmdParms;

        public FormMain()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (dbconn.TestConnect() == false)
            {
                MessageBox.Show("数据库连接失败！", "提示");
                button2.Enabled = false;
                button3.Enabled = false;
            }
            else
            {
                MessageBox.Show("数据库连接成功！", "提示");
                button2.Enabled = true;
                button3.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataTable dt;
            string ls_sysdate;

            dt = dbconn.ExecuteQuery("select sysdate from dual", cmdParms);
            ls_sysdate = dt.Rows[0]["sysdate"].ToString();
            MessageBox.Show(ls_sysdate, "系统时间");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataTable dt;

            dt = dbconn.ExecuteQuery("select rownum ,ZDLB 字典类别,ZDDH 字典代码,zdmc 字典名称,zjf 助记符 from v_parm_dict", cmdParms);
            this.dataGridView1.DataSource = dt;
            //this.dataGridView1.DataMember = "zdmc";
        }
        #region 加密解密算法
        //默认密钥向量
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string EncryptDES(string encryptString, string encryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }
        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥</param>
        /// <returns>解密成功后返回解密后的字符串，失败返回源串</returns>
        public static string DecryptDES(string decryptString, string decryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }
        #endregion
        #region 医院名称加密解密
        /// <summary>
        /// 加密医院名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_hospital_encry_Click(object sender, EventArgs e)
        {
            string hos_originaltext;

            hos_originaltext = this.tB_originaltext.Text.Trim();

            if (hos_originaltext == "")
            {
                MessageBox.Show("请在原文中输入医院名称！");
                return;
            }

            tB_ciphertext.Text = EncryptDES(hos_originaltext, "hospname");
        }
        /// <summary>
        /// 解密医院名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_Decrypt_Click(object sender, EventArgs e)
        {
            string hos_ciphertext;

            hos_ciphertext = this.tB_ciphertext.Text.Trim();

            if (hos_ciphertext == "")
            {
                return;
            }
            tB_originaltext.Text = DecryptDES(hos_ciphertext, "hospname");
        }
        #endregion


        #region  注册时间加密解密
        private void bt_ent_time_Click(object sender, EventArgs e)
        {
            string time_ciphertext;

            time_ciphertext = this.tB_originaltext.Text.Trim();

            if (time_ciphertext == "")
            {
                MessageBox.Show("请在原文中输入注册时间！");
                return;
            }
            try
            {
                DateTime dt = DateTime.Parse(time_ciphertext);
            }
            catch (Exception ex)
            {
                MessageBox.Show("时间格式不正确！");
                return;
            }

            string secretkey = this.tB_version.Text.Trim();
            if (secretkey == "")
            {
                MessageBox.Show("请输入密钥！");
                return;
            }
            else if (secretkey.Length < 8)
            {
                secretkey = secretkey.PadLeft(8, 'Z');
            }
            else
            {
                secretkey = secretkey.Substring(0, 8);
            }
            tB_ciphertext.Text = EncryptDES(time_ciphertext, secretkey);
        }


        #endregion
        private void button7_Click(object sender, EventArgs e)
        {
            string hos_jiemi;

            hos_jiemi = this.tB_originaltext.Text.Trim();

            if (hos_jiemi == "")
            {
                return;
            }

            tB_ciphertext.Text = EncryptDES(hos_jiemi, this.tB_version.Text.Trim());
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string hos_jiami;

            hos_jiami = this.tB_ciphertext.Text.Trim();

            if (hos_jiami == "")
            {
                return;
            }

            tB_originaltext.Text = DecryptDES(hos_jiami, this.tB_version.Text.Trim());
        }
        /// <summary>
        /// 加密IP地址
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string IP_code;
            string RegexStr = string.Empty;
            #region 字符串匹配


            Regex regex = new Regex(@"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$");
            #endregion

            IP_code = this.tB_originaltext.Text.Trim();

            if (IP_code == "" || !regex.IsMatch(IP_code))
            {
                MessageBox.Show("请在原文中输入正确IP！");
                return;
            }

            tB_ciphertext.Text = EncryptDES(IP_code, "ip__code");
        }
        /// <summary>
        /// 解密IP地址
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string ip_ciphertext;

            ip_ciphertext = this.tB_ciphertext.Text.Trim();

            if (ip_ciphertext == "")
            {
                return;
            }
            tB_originaltext.Text = DecryptDES(ip_ciphertext, "ip__code");
        }
    }
}