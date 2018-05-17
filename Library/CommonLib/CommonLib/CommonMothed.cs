using Microsoft.VisualBasic;
using System;
using System.Security.Cryptography;
using System.Text;

namespace CommonLib
{
	public class CommonMothed
	{
		public static string GetMD5(string _value)
		{
			string result;
			try
			{
				string text = "";
				if (_value == string.Empty)
				{
					result = text;
				}
				else
				{
					byte[] bytes = Encoding.ASCII.GetBytes(_value);
					if (bytes.Length > 1)
					{
						byte b = bytes[0];
						bytes[0] = bytes[bytes.Length - 1];
						bytes[bytes.Length - 1] = b;
					}
					MD5 mD = new MD5CryptoServiceProvider();
					byte[] array = mD.ComputeHash(bytes);
					for (int i = 0; i < array.Length; i++)
					{
						if (i != 0 && i % 5 == 0)
						{
							text += "b";
						}
						else if (i != 0 && i % 3 == 0)
						{
							text += "u";
						}
						text += Conversion.Hex(array[i]).ToString();
					}
					result = text.ToUpper();
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}
	}
}
