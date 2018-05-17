using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace DrectSoft.DrawDriver
{
	public class CommonMethods
	{
		private const int BUFFERSIZE = 5000;

		public static string ZipPwd = "";

		[DllImport("kernel32")]
		private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

		public static string ReadInIValue(string _section, string _key, string _path)
		{
			string result;
			try
			{
				StringBuilder stringBuilder = CommonMethods.ReadOrWriteCheck(_section, _key, _path);
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Insert(0, "读配置文件:\r\n");
					stringBuilder.AppendLine();
					stringBuilder.Append("CommonMethods.cs → ReadIniValue()");
					throw new Exception(stringBuilder.ToString());
				}
				StringBuilder stringBuilder2 = new StringBuilder(5000);
				CommonMethods.GetPrivateProfileString(_section, _key, "", stringBuilder2, 5000, _path);
				string text = stringBuilder2.ToString();
				result = text;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static bool WriteInIValue(string _section, string _key, string _value, string _path)
		{
			bool result;
			try
			{
				StringBuilder stringBuilder = CommonMethods.ReadOrWriteCheck(_section, _key, _path);
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Insert(0, "写配置文件:\r\n");
					stringBuilder.AppendLine();
					stringBuilder.Append("CommonMethods.cs → WriteIniValue()");
					throw new Exception(stringBuilder.ToString());
				}
				CommonMethods.WritePrivateProfileString(_section, _key, _value, _path);
				bool flag = true;
				result = flag;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		private static StringBuilder ReadOrWriteCheck(string _section, string _key, string _path)
		{
			StringBuilder result;
			try
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (_section.Trim() == string.Empty)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append("节点--必指定");
				}
				if (_key.Trim() == string.Empty)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append("项--必指定");
				}
				if (_path.Trim() == string.Empty)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append("路径--必指定");
				}
				else if (!File.Exists(_path))
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append("配置文件不存在");
				}
				result = stringBuilder;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		private static StringBuilder PreZipCheck(string _srcFile, string _objFile, bool iscompresultsion)
		{
			StringBuilder result;
			try
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (_srcFile.Trim() == "")
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					if (iscompresultsion)
					{
						stringBuilder.Append("待压缩文件--必须指定");
					}
					else
					{
						stringBuilder.Append("待解压文件--必须指定¨");
					}
				}
				else if (!File.Exists(_srcFile))
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					if (iscompresultsion)
					{
						stringBuilder.Append("待压缩文件--不存在");
					}
					else
					{
						stringBuilder.Append("待解压文件--不存在");
					}
				}
				if (_objFile.Trim() == "")
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					if (iscompresultsion)
					{
						stringBuilder.Append("压缩后文件名--必须指定¨");
					}
					else
					{
						stringBuilder.Append("解压后文件路径(或名称)--必须指定¨");
					}
				}
				result = stringBuilder;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static bool DecompressionZip(string _zipfile, string _objdir)
		{
			bool result;
			try
			{
				StringBuilder stringBuilder = CommonMethods.PreZipCheck(_zipfile, _objdir, false);
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Insert(0, "ZIP解压异常:\r\n");
					stringBuilder.AppendLine();
					stringBuilder.Append("CommonMethods.cs ->DecompressionZip()");
					throw new Exception(stringBuilder.ToString());
				}
				_objdir = _objdir.Replace("/", "\\");
				if (!_objdir.EndsWith("\\"))
				{
					_objdir += "\\";
				}
				if (!Directory.Exists(_objdir))
				{
					Directory.CreateDirectory(_objdir);
				}
				ZipInputStream zipInputStream = new ZipInputStream(File.OpenRead(_zipfile));
				if (CommonMethods.ZipPwd != "")
				{
					zipInputStream.Password = CommonMethods.ZipPwd;
				}
				ZipEntry nextEntry;
				while ((nextEntry = zipInputStream.GetNextEntry()) != null)
				{
					string directoryName = Path.GetDirectoryName(nextEntry.Name);
					string fileName = Path.GetFileName(nextEntry.Name);
					if (directoryName != string.Empty)
					{
						Directory.CreateDirectory(_objdir + directoryName);
					}
					if (fileName != string.Empty)
					{
						FileStream fileStream = File.Create(_objdir + nextEntry.Name);
						byte[] array = new byte[2048];
						while (true)
						{
							int num = zipInputStream.Read(array, 0, array.Length);
							if (num <= 0)
							{
								break;
							}
							fileStream.Write(array, 0, num);
						}
						fileStream.Flush();
						fileStream.Close();
					}
				}
				zipInputStream.Close();
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		public static bool CompresultsionZip(string _fileToZip, string _zipedFile)
		{
			bool result;
			try
			{
				StringBuilder stringBuilder = CommonMethods.PreZipCheck(_fileToZip, _zipedFile, true);
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Insert(0, "ZIP压缩异常:\r\n");
					stringBuilder.AppendLine();
					stringBuilder.Append("CommonMethods.cs ->CompresultsionZip()");
					throw new Exception(stringBuilder.ToString());
				}
				FileStream fileStream = File.OpenRead(_fileToZip);
				byte[] array = new byte[fileStream.Length];
				fileStream.Read(array, 0, array.Length);
				fileStream.Close();
				fileStream = File.Create(_zipedFile);
				ZipOutputStream zipOutputStream = new ZipOutputStream(fileStream);
				if (CommonMethods.ZipPwd.Trim() != "")
				{
					zipOutputStream.Password = CommonMethods.ZipPwd;
				}
				ZipEntry entry = new ZipEntry(Path.GetFileName(_fileToZip));
				zipOutputStream.PutNextEntry(entry);
				zipOutputStream.SetLevel(6);
				zipOutputStream.Write(array, 0, array.Length);
				zipOutputStream.Flush();
				zipOutputStream.Close();
				fileStream.Close();
			}
			catch
			{
				result = false;
				return result;
			}
			result = true;
			return result;
		}

		public static int StringToInt32(string str)
		{
			int result;
			try
			{
				if (str == null)
				{
					throw new NullReferenceException();
				}
				result = CommonMethods.StringToInt32(str, 0);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static int StringToInt32(string str, int intDefault)
		{
			int result;
			try
			{
				if (str == null)
				{
					result = intDefault;
				}
				else
				{
					int num = -1;
					if (str.EndsWith(".0000"))
					{
						str = str.Substring(0, str.Length - 5);
					}
					if (!int.TryParse(str, out num))
					{
						num = intDefault;
					}
					result = num;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static long StringToInt64(string str)
		{
			long result;
			try
			{
				if (str == null)
				{
					throw new NullReferenceException();
				}
				result = CommonMethods.StringToInt64(str, 0L);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static long StringToInt64(string str, long longDefault)
		{
			long result;
			try
			{
				if (str == null)
				{
					throw new NullReferenceException();
				}
				long num = -1L;
				if (str.EndsWith(".0000"))
				{
					str = str.Substring(0, str.Length - 5);
				}
				if (!long.TryParse(str, out num))
				{
					num = longDefault;
				}
				result = num;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static float StringToFloat(string str)
		{
			float result;
			try
			{
				if (str == null)
				{
					throw new NullReferenceException();
				}
				result = CommonMethods.StringToFloat(str, 0f);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static float StringToFloat(string str, float floatDefault)
		{
			float result;
			try
			{
				if (str == null)
				{
					throw new NullReferenceException();
				}
				float num = -1E+08f;
				if (!float.TryParse(str, out num))
				{
					num = floatDefault;
				}
				result = num;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static double StringToDouble(string str)
		{
			double result;
			try
			{
				if (str == null)
				{
					throw new NullReferenceException();
				}
				result = CommonMethods.StringToDouble(str, 0.0);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static double StringToDouble(string str, double doubleDefault)
		{
			double result;
			try
			{
				if (str == null)
				{
					throw new NullReferenceException();
				}
				double num = -99999999.99;
				if (!double.TryParse(str, out num))
				{
					num = doubleDefault;
				}
				result = num;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static DateTime StringToDateTime(string str)
		{
			DateTime result;
			try
			{
				if (str == null)
				{
					throw new NullReferenceException();
				}
				result = CommonMethods.StringToDateTime(str, DateTime.Now);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static DateTime StringToDateTime(string str, DateTime DateTimeDefault)
		{
			DateTime result;
			try
			{
				if (str == null)
				{
					throw new NullReferenceException();
				}
				DateTime dateTime = DateTime.Now;
				if (!DateTime.TryParse(str, out dateTime))
				{
					dateTime = DateTimeDefault;
				}
				result = dateTime;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static string StringToString(string str)
		{
			string result;
			try
			{
				if (str == null)
				{
					throw new NullReferenceException();
				}
				result = CommonMethods.StringToString(str, "");
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static string StringToString(string str, string strDefault)
		{
			string result;
			try
			{
				if (str == null)
				{
					throw new NullReferenceException();
				}
				if (strDefault == null)
				{
					throw new NullReferenceException();
				}
				string text;
				if (str == null)
				{
					text = strDefault;
				}
				else
				{
					text = str;
				}
				result = text;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static bool StringToBool(string str)
		{
			bool result;
			try
			{
				if (str == null)
				{
					throw new NullReferenceException();
				}
				result = CommonMethods.StringToBool(str, false);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static bool StringToBool(string str, bool boolDefault)
		{
			bool result;
			try
			{
				if (str == null)
				{
					throw new NullReferenceException();
				}
				bool flag;
				if (str == null)
				{
					flag = boolDefault;
				}
				else
				{
					flag = (!str.Equals("0") && (str.Equals("1") || (!str.ToLower().Equals("false") && (str.ToLower().Equals("true") || boolDefault))));
				}
				result = flag;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static AlignType StringToAlignType(string str, AlignType return_default)
		{
			AlignType result;
			if (str == null)
			{
				result = AlignType.Left;
			}
			else
			{
				string text = str.Trim().ToLower();
				if (text.Equals("left"))
				{
					result = AlignType.Left;
				}
				else if (text.Equals("center"))
				{
					result = AlignType.Center;
				}
				else if (text.Equals("right"))
				{
					result = AlignType.Right;
				}
				else
				{
					result = return_default;
				}
			}
			return result;
		}

		public static ValignType StringToValignType(string str, ValignType return_default)
		{
			ValignType result;
			if (str == null)
			{
				result = ValignType.Middle;
			}
			else
			{
				string text = str.Trim().ToLower();
				if (text.Equals("top"))
				{
					result = ValignType.Top;
				}
				else if (text.Equals("middle"))
				{
					result = ValignType.Middle;
				}
				else if (text.Equals("bottom"))
				{
					result = ValignType.Bottom;
				}
				else
				{
					result = return_default;
				}
			}
			return result;
		}

		public static FontColor StringToFontColor(string str)
		{
			return CommonMethods.StringToFontColor(str, StyleDefault.FontColor);
		}

		public static FontColor StringToFontColor(string str, FontColor return_default)
		{
			FontColor result;
			if (str == null)
			{
				result = return_default;
			}
			else
			{
				FontColor fontColor = new FontColor();
				string[] array = str.Split(new char[]
				{
					' '
				});
				if (array.Length >= 3)
				{
					fontColor.Color = CommonMethods.StringToColor(array[2]);
					fontColor.Font = new Font(new FontFamily(array[0]), CommonMethods.StringToFloat(array[1].ToLower().Replace("px", "")), FontStyle.Regular);
					result = fontColor;
				}
				else
				{
					result = return_default;
				}
			}
			return result;
		}

		public static Color StringToColor(string str)
		{
			Color result;
			try
			{
				result = ColorTranslator.FromHtml(str);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static LineType StringToLineType(string str, LineType return_default)
		{
			LineType result;
			if (str == null)
			{
				result = LineType.Solid;
			}
			else
			{
				string text = str.Trim().ToLower();
				if (text.Equals("solid"))
				{
					result = LineType.Solid;
				}
				else if (text.Equals("dash"))
				{
					result = LineType.Dash;
				}
				else if (text.Equals("dashdot"))
				{
					result = LineType.DashDot;
				}
				else if (text.Equals("dashdotdot"))
				{
					result = LineType.DashDotDot;
				}
				else if (text.Equals("dot"))
				{
					result = LineType.Dot;
				}
				else
				{
					result = return_default;
				}
			}
			return result;
		}

		public static DrawType StringToDrawType(string str, DrawType return_default)
		{
			DrawType result;
			if (str == null)
			{
				result = DrawType.None;
			}
			else
			{
				string text = str.Trim().ToLower();
				if (text.Equals("none"))
				{
					result = DrawType.None;
				}
				else if (text.Equals("string"))
				{
					result = DrawType.String;
				}
				else if (text.Equals("line"))
				{
					result = DrawType.Line;
				}
				else if (text.Equals("image"))
				{
					result = DrawType.Image;
				}
				else if (text.Equals("fillrectangle"))
				{
					result = DrawType.FillRectangle;
				}
				else if (text.Equals("rectangle"))
				{
					result = DrawType.Rectangle;
				}
				else if (text.Equals("circle"))
				{
					result = DrawType.Circle;
				}
				else
				{
					result = return_default;
				}
			}
			return result;
		}

		public static Border StringToBorder(string str)
		{
			Border result;
			try
			{
				result = CommonMethods.StringToBorder(str, null);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static Border StringToBorder(string str, Border return_default)
		{
			Border result;
			try
			{
				if (str == null)
				{
					result = return_default;
				}
				else
				{
					Border border = new Border();
					string[] array = str.Split(new char[]
					{
						';'
					});
					string text;
					string text2;
					string text3;
					string text4;
					if (array.Length >= 4)
					{
						text = array[0];
						text2 = array[1];
						text3 = array[2];
						text4 = array[3];
					}
					else if (array.Length >= 3)
					{
						text = array[0];
						text2 = array[1];
						text3 = array[2];
						text4 = text2;
					}
					else if (array.Length >= 2)
					{
						text = array[0];
						text2 = array[1];
						text3 = text;
						text4 = text2;
					}
					else
					{
						if (array.Length < 1)
						{
							result = return_default;
							return result;
						}
						text = array[0];
						text2 = text;
						text3 = text;
						text4 = text2;
					}
					string[] array2 = text.Split(new char[]
					{
						' '
					});
					if (array2.Length >= 3)
					{
						border.IsDrawTop = true;
						border.TopColor = CommonMethods.StringToColor(array2[2].Trim());
						border.TopStyle = CommonMethods.StringToLineType(array2[0].Trim(), LineType.Solid);
						border.TopWidth = CommonMethods.StringToInt32(array2[1].Trim().ToLower().Replace("px", ""), 1);
					}
					else
					{
						border.IsDrawTop = false;
					}
					string[] array3 = text2.Split(new char[]
					{
						' '
					});
					if (array3.Length >= 3)
					{
						border.IsDrawRight = true;
						border.RightColor = CommonMethods.StringToColor(array3[2].Trim());
						border.RightStyle = CommonMethods.StringToLineType(array3[0].Trim(), LineType.Solid);
						border.RightWidth = CommonMethods.StringToInt32(array3[1].Trim().ToLower().Replace("px", ""), 1);
					}
					else
					{
						border.IsDrawRight = false;
					}
					string[] array4 = text3.Split(new char[]
					{
						' '
					});
					if (array4.Length >= 3)
					{
						border.IsDrawBottom = true;
						border.BottomColor = CommonMethods.StringToColor(array4[2].Trim());
						border.BottomStyle = CommonMethods.StringToLineType(array4[0].Trim(), LineType.Solid);
						border.BottomWidth = CommonMethods.StringToInt32(array4[1].Trim().ToLower().Replace("px", ""), 1);
					}
					else
					{
						border.IsDrawBottom = false;
					}
					string[] array5 = text4.Split(new char[]
					{
						' '
					});
					if (array5.Length >= 3)
					{
						border.IsDrawLeft = true;
						border.LeftColor = CommonMethods.StringToColor(array5[2].Trim());
						border.LeftStyle = CommonMethods.StringToLineType(array5[0].Trim(), LineType.Solid);
						border.LeftWidth = CommonMethods.StringToInt32(array5[1].Trim().ToLower().Replace("px", ""), 1);
					}
					else
					{
						border.IsDrawLeft = false;
					}
					result = border;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static string GetStyleValue(string strStyleFrom, string strNameFrom)
		{
			string result;
			try
			{
				if (strStyleFrom == null)
				{
					throw new NullReferenceException();
				}
				if (strNameFrom == null)
				{
					throw new NullReferenceException();
				}
				string[] array = strStyleFrom.Split(new char[]
				{
					';'
				});
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i].Trim();
					if (text.Length > 0)
					{
						string[] array2 = text.Split(new char[]
						{
							':'
						});
						if (array2.Length >= 2)
						{
							string text2 = array2[0].Trim().ToLower();
							string text3 = array2[1].Trim().ToLower();
							if (text2.Trim().ToLower().Equals(strNameFrom.Trim().ToLower()))
							{
								result = text3;
								return result;
							}
						}
					}
				}
				result = null;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static DrawInfoString StringToDrawInfoString(string str)
		{
			DrawInfoString result;
			try
			{
				if (str == null)
				{
					throw new NullReferenceException();
				}
				DrawInfoString drawInfoString = new DrawInfoString();
				string[] array = str.Split(new char[]
				{
					';'
				});
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i].Trim();
					if (text.Length > 0)
					{
						string[] array2 = text.Split(new char[]
						{
							':'
						});
						if (array2.Length >= 2)
						{
							string text2 = array2[0].Trim().ToLower();
							string text3 = array2[1].Trim().ToLower();
							if (text2.Equals("left"))
							{
								drawInfoString.Left = text3;
							}
							else if (text2.Equals("top"))
							{
								drawInfoString.Top = text3;
							}
							else if (text2.Equals("width"))
							{
								drawInfoString.Width = text3;
							}
							else if (text2.Equals("height"))
							{
								drawInfoString.Height = text3;
							}
							else if (text2.Equals("align"))
							{
								drawInfoString.Align = text3;
							}
							else if (text2.Equals("valign"))
							{
								drawInfoString.Valign = text3;
							}
							else if (text2.Equals("fontcolor"))
							{
								drawInfoString.FontColor = text3;
							}
							else if (text2.Equals("bgcolor"))
							{
								drawInfoString.BgColor = text3;
							}
							else if (text2.Equals("border"))
							{
								drawInfoString.Border = text3;
							}
						}
					}
				}
				result = drawInfoString;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static DrawInfo DrawInfoStringToDrawInfo(DrawInfoString objString)
		{
			if (objString == null)
			{
				throw new NullReferenceException();
			}
			return new DrawInfo
			{
				Left = CommonMethods.StringToInt32(objString.Left, 0),
				Top = CommonMethods.StringToInt32(objString.Top, 0),
				Width = CommonMethods.StringToInt32(objString.Width),
				Height = CommonMethods.StringToInt32(objString.Height),
				Align = CommonMethods.StringToAlignType(objString.Align, AlignType.Center),
				Valign = CommonMethods.StringToValignType(objString.Valign, ValignType.Middle),
				FontColor = CommonMethods.StringToFontColor(objString.FontColor, null),
				BgColor = CommonMethods.StringToColor(objString.BgColor),
				Border = CommonMethods.StringToBorder(objString.Border, null)
			};
		}

		public static DrawInfo XmlNodeToDrawInfo(XmlNode _node)
		{
			DrawInfo result;
			try
			{
				DrawInfo drawInfo = CommonMethods.DrawInfoStringToDrawInfo(new DrawInfoString
				{
					Left = XmlCommomOp.GetValueByAttribute(_node, "Left"),
					Top = XmlCommomOp.GetValueByAttribute(_node, "Top"),
					Width = XmlCommomOp.GetValueByAttribute(_node, "Width"),
					Height = XmlCommomOp.GetValueByAttribute(_node, "Height"),
					Align = XmlCommomOp.GetValueByAttribute(_node, "Align"),
					Valign = XmlCommomOp.GetValueByAttribute(_node, "Valign"),
					FontColor = XmlCommomOp.GetValueByAttribute(_node, "Font"),
					BgColor = XmlCommomOp.GetValueByAttribute(_node, "BgColor"),
					Border = XmlCommomOp.GetValueByAttribute(_node, "Border")
				});
				result = drawInfo;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static FontColor XmlNodeToFontColor(XmlNode _node)
		{
			FontColor fontColor;
			try
			{
				DrawInfo drawInfo = CommonMethods.DrawInfoStringToDrawInfo(new DrawInfoString
				{
					FontColor = XmlCommomOp.GetValueByAttribute(_node, "Font")
				});
				fontColor = drawInfo.FontColor;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return fontColor;
		}

		public static string xpathToLower(string xpath)
		{
			string result;
			try
			{
				if (xpath == null)
				{
					throw new NullReferenceException();
				}
				string text = Regex.Replace(xpath, "《(.+?)》", "*[translate(name(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz') = translate('$1', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')]");
				result = text;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static int SearchStringsByKey(string[] strs, string key)
		{
			int result;
			try
			{
				if (strs == null)
				{
					throw new NullReferenceException();
				}
				if (key == null)
				{
					throw new NullReferenceException();
				}
				for (int i = 0; i < strs.Length; i++)
				{
					if (strs[i].Trim().ToLower().Equals(key.Trim().ToLower()))
					{
						result = i;
						return result;
					}
				}
				result = -1;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static List<string> WrapLongString(Font font, string text, int width)
		{
			List<string> result;
			try
			{
				Bitmap bitmap = new Bitmap(1000, 1000);
				Graphics graphics = Graphics.FromImage(bitmap);
				int num = 0;
				text += " ";
				int length = text.Length;
				List<string> list = new List<string>();
				for (int i = 0; i < length; i++)
				{
					int num2 = i;
					if (i == length - 1)
					{
						list.Add(text.Substring(num));
					}
					else if (num2 + 1 < text.Length && text.Substring(num2, 2) == "\r\n")
					{
						list.Add(text.Substring(num, num2 - num));
						i = (num2 = i + 2);
						num = num2;
					}
					else if (graphics.MeasureString(text.Substring(num, num2 - num + 1), font).Width > (float)width)
					{
						list.Add(text.Substring(num, num2 - num));
						num = num2;
					}
				}
				graphics.Dispose();
				bitmap.Dispose();
				result = list;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static void DrawStringEx(string text, Graphics g, PointF startPoint, Font font, Brush brush)
		{
			try
			{
				PointF point = startPoint;
				char[] array = text.ToCharArray();
				float num = -(font.Size / 3f);
				char[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					char c = array2[i];
					SizeF sizeF;
					if (c.ToString().Equals(" "))
					{
						sizeF = g.MeasureString("1", font);
					}
					else if (c.ToString().Equals("\u3000"))
					{
						sizeF = g.MeasureString("测", font);
					}
					else
					{
						sizeF = g.MeasureString(c.ToString(), font);
					}
					g.DrawString(c.ToString(), font, brush, point);
					point.X += sizeF.Width + num;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static string RemoveTagHTML(string src)
		{
			string result;
			try
			{
				if (src == null)
				{
					result = "";
				}
				else
				{
					Regex regex = new Regex("<(\\w+)(.*?)</\\1>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
					src = regex.Replace(src, "");
					result = src;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static string TextAddSpan(string src)
		{
			string result;
			try
			{
				if (src == null)
				{
					result = "";
				}
				else
				{
					Regex regex = new Regex("<(\\w+)(.*?)</\\1>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
					MatchCollection matchCollection = regex.Matches(src);
					foreach (Match match in matchCollection)
					{
						src = src.Replace(match.Value, "</span>" + match.Value + "<span>");
					}
					src = "<span>" + src + "</span>";
					src = src.Replace("<span></span>", "");
					result = src;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static bool InnerXmlTextAddSpan(XmlNode _node)
		{
			bool result;
			try
			{
				string text = CommonMethods.RemoveTagHTML(_node.InnerXml).Trim();
				if (text.Length > 0)
				{
					_node.InnerXml = CommonMethods.TextAddSpan(_node.InnerXml);
				}
				result = true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static string EndSpaceDrawLineStringConvert(string s)
		{
			string result;
			try
			{
				if (s == null)
				{
					result = "\0";
				}
				else
				{
					result = s.Replace("\u3000", "  ").Replace(" ", "\0");
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static int GetTextLength(string s)
		{
			int result;
			try
			{
				if (s == null)
				{
					result = 0;
				}
				else
				{
					int num = s.Length;
					char[] array = s.ToCharArray();
					for (int i = 0; i < array.Length; i++)
					{
						if (Convert.ToInt32(array[i]) > 126)
						{
							num++;
						}
					}
					result = num;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static string DrawLineColorDifferentStringConvert(string s)
		{
			string result;
			try
			{
				if (s == null)
				{
					result = "\0";
				}
				else
				{
					int i = CommonMethods.GetTextLength(s);
					StringBuilder stringBuilder = new StringBuilder();
					while (i > 0)
					{
						stringBuilder.Append("\0");
						i--;
					}
					result = stringBuilder.ToString();
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static bool Landspace()
		{
			bool result;
			try
			{
				XmlNode xmlNode = XmlCommomOp.GetXmlNodeByTagname("PageSize")[0];
				if (xmlNode == null || xmlNode.Attributes["landspace"] == null || string.IsNullOrEmpty(xmlNode.Attributes["landspace"].Value))
				{
					result = false;
				}
				else if (xmlNode.Attributes["landspace"].Value == "false")
				{
					result = false;
				}
				else
				{
					result = true;
				}
			}
			catch (Exception)
			{
				throw;
			}
			return result;
		}
	}
}
