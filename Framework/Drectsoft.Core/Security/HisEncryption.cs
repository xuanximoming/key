using System;
using System.IO;
using System.Xml;


namespace DrectSoft.Core
{
	/// <summary>
	/// His加密类
    /// 注：虽然采用与HIS程序一致的算法，但是HIS程序采用bdsj的后8位进行加密，EMR中取bdsj的16位全部进行加密
	/// </summary>
	public class HisEncryption
	{
		private const int USERCODEMASTLEN = 128;
		private char[] r = new char[USERCODEMASTLEN];
		private char[] key = new char[USERCODEMASTLEN];
		private char[] pass;
		private int m;

        /// <summary>
        /// 加密后的密码长度
        /// </summary>
        public const int PasswordLength = 8;

        /// <summary>
        /// Ctor
        /// </summary>
		public HisEncryption()
		{
		}

        /// <summary>
        /// 字符数组长度
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
		private static int CharArrayLength(char[] a)
		{
			int len =0;
			for (int i=0; i<USERCODEMASTLEN;i++)
			{
				if (a[i] != (char)0)
				{
					len++;
				}
				else 
				{
					return len;
				}
			}
			return len;
		}

        /// <summary>
        /// Loadkey
        /// </summary>
		private void LoadKey()
		{
			for (int k=0 ; k<=m-1 ; k++)
			{
				key[k] = r[k];
			}
		}

        /// <summary>
        /// preprocess
        /// </summary>
		private void PreProcess()
		{
			int ds;
			for (int cs=0 ; cs <=0xfe; cs++)
			{
				for (int ks=0; ks<=m-1; ks++)
				{
					ds = Convert.ToByte(key[ks]) & 0xff;
					r[ks] = Convert.ToChar((ds >> 4) + ((ds & 0x0f) << 4));
					key[ks] = r[ks];
				}
				Exchange(r,m,m);
			}
		}

		private int Association(int size)
		{
			return (size + m ) % 997;
		}

		private void Exchange(char[] buf, int size, int associat)
		{
			int p,rt,l,j;
			rt = (Convert.ToByte(key[m-1])+(USERCODEMASTLEN % (USERCODEMASTLEN-m))) % 0xff;
			p = 0;
			for (int k=0; k<=size-1; k++)
			{
				l =(key[p]+rt+k+p) % 2;
				j =(key[p]+rt+k+p+associat) % m;
				key[p] = Convert.ToChar(key[p] ^ (key[j] >> 1));
				j = (j+l+k+associat) % 6+1;
				if (l == 0)
				{
                    key[p] = Convert.ToChar(((key[p] << j) & 0xff) ^ (rt ^ key[j]));
                }
				else
				{
                    key[p] = Convert.ToChar((key[p] >> j) ^ ((~(rt ^ key[j])) & 0xff));
                }
				rt = key[p];
				
				if (k <= buf.GetUpperBound(0))
				{

				}
				else
				{
					//buf.Length = buf.Length + 1;
				}

				buf[k] = Convert.ToChar(buf[k] ^ rt);
				if (p < m)
				{
					p++;
				}
				else
				{
					p = 0;
				}
			}
		}
		
		private void FirstKey(char[] buf)
		{
			int len;
			for (int ks=0; ks<=USERCODEMASTLEN-1; ks++)
			{
				r[ks] = '\0';
				key[ks] ='\0';
			}
			buf.CopyTo(key,0);
			len = CharArrayLength(key);
			key[len] = (char)0xfc;
			m = CharArrayLength(key);
			while (m < (USERCODEMASTLEN / 2)) 
			{
                m = CharArrayLength(key);
				for (int ks=0; ks<=m-1; ks++)
				{
					key[m+ks] = key[ks];
				}
                m = CharArrayLength(key);
			}
            m = CharArrayLength(key);
			PreProcess();
		}

		private void Secret(char[] buffer, int len, char[] passkey)
		{
			int	zs;
			FirstKey(passkey);
			zs = Association(len);
			LoadKey();
			pass = new char[buffer.Length];
			buffer.CopyTo(pass,0);
			Exchange(pass, len, zs);
		}

        /// <summary>
        /// 加密密码
        /// </summary>
		public String EncryptPassword
		{
			get
			{
				String s = new String(pass);
				return s;
			}
		}

        /// <summary>
        /// 解密密码
        /// </summary>
		public String UNEncryptPassword
		{
			get
			{
				String s = new String(pass);
				String s2 = "";
				int i = 0;
				while ( i < s.Length )
				{
					if (s[i]!=(char)0) 
					{
						s2 = s2 + s[i];
					}
					else
					{
						break;
					}
					i++;
				}
				return s2;
			}
		}

        /// <summary>
        /// 加密字符串sql用,形式为Char(xx)+Char(xx)...
        /// </summary>
        /// <param name="buffer">密钥</param>
        /// <param name="len">长度=8</param>
        /// <param name="key">传入的明码密码</param>
        /// <returns></returns>
        public static string EncodeStringSql(string buffer, int len, string key)
        {
            if (buffer == null) throw new ArgumentNullException("buffer", "传入密钥为空");
            if (string.IsNullOrEmpty(key)) key = string.Empty;

            char[] pbuffer = new char[1024];
            char[] pkey = new char[80];
            string s = "";
            char[] ckey;

            HisEncryption encryption = new HisEncryption();
            buffer.ToCharArray().CopyTo(pbuffer, 0);
            key.ToCharArray().CopyTo(pkey, 0);
            encryption.Secret(pbuffer, len, pkey);
            ckey = encryption.EncryptPassword.ToCharArray();
            s = "char(" + Convert.ToByte(ckey[0]).ToString() + ")";
            for (int i = 1; i < len; i++)
            {
                s = s + "+char(" + Convert.ToByte(ckey[i]).ToString() + ")";
            }
            s = s + "+char(0)";
            return s;
        }

        /// <summary>
        /// 加密串
        /// </summary>
        /// <param name="buffer">密钥</param>
        /// <param name="len">长度=8</param>
        /// <param name="key">传入的明码密码</param>
        /// <returns></returns>
        public static string EncodeString(string buffer, int len, string key)
        {
            try
            {
                if (buffer == null) throw new ArgumentNullException("buffer", "传入密钥为空");
                if (string.IsNullOrEmpty(key)) key = string.Empty;

                char[] pbuffer = new char[1024];
                char[] pkey = new char[80];

                HisEncryption encryption = new HisEncryption();
                buffer.ToCharArray().CopyTo(pbuffer, 0);
                key.ToCharArray().CopyTo(pkey, 0);
                encryption.Secret(pbuffer, len, pkey);
                byte[] encryptBytes = CharArray2ByteArray(len, encryption);
                return Convert.ToBase64String(encryptBytes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 加密串
        /// </summary>
        /// <param name="buffer">密钥</param>
        /// <param name="len">长度=8</param>
        /// <param name="key">传入的明码密码</param>
        /// <returns>返回字符数组</returns>
        public static byte[] EncodeString2ByteArray(string buffer, int len, string key)
        {
            if (buffer == null) throw new ArgumentNullException("buffer", "传入密钥为空");
            if (string.IsNullOrEmpty(key)) key = string.Empty;

            char[] pbuffer = new char[1024];
            char[] pkey = new char[80];

            HisEncryption encryption = new HisEncryption();
            buffer.ToCharArray().CopyTo(pbuffer, 0);
            key.ToCharArray().CopyTo(pkey, 0);
            encryption.Secret(pbuffer, len, pkey);

            return CharArray2ByteArray(len, encryption);
        }

        private static byte[] CharArray2ByteArray(int len, HisEncryption encryption)
        {
            byte[] ret = new byte[len];
            for (int i = 0; i < len; i++)
            {
                ret[i] = Convert.ToByte(encryption.pass[i]);
            }
            return ret;
        }
    }
}
