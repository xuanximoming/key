using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;

namespace DrectSoft.DrawDriver
{
	public class DrawDriverXmlOp
	{
		private static string strXmlDocumentDTDFilePath = "D:\\DocumentDTD.V1.0.Xml";

		private static XmlDocument GetXmlDocumentDTD()
		{
			XmlDocument result;
			try
			{
				if (DrawDriverXmlOp.strXmlDocumentDTDFilePath == null)
				{
					throw new NullReferenceException();
				}
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(DrawDriverXmlOp.strXmlDocumentDTDFilePath);
				result = xmlDocument;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static bool IsCanRead(XmlDocument _doc)
		{
			bool result;
			try
			{
				if (_doc == null)
				{
					throw new NullReferenceException();
				}
				string xpath = CommonMethods.xpathToLower("/《PrintDocument》/《Body》");
				XmlNodeList xmlNodeList = _doc.SelectNodes(xpath);
				int count = xmlNodeList.Count;
				if (count > 0)
				{
					result = true;
				}
				else
				{
					result = false;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static string[] GetSupportTags()
		{
			string[] result;
			try
			{
				List<string> list = new List<string>();
				XmlDocument xmlDocumentDTD = DrawDriverXmlOp.GetXmlDocumentDTD();
				XmlCommomOp.xmlPath = DrawDriverXmlOp.strXmlDocumentDTDFilePath;
				XmlNode[] xmlNodeByTagname = XmlCommomOp.GetXmlNodeByTagname("SupportLabels");
				if (xmlNodeByTagname == null || xmlNodeByTagname.Length <= 0)
				{
					result = null;
				}
				else
				{
					foreach (XmlNode xmlNode in xmlNodeByTagname[0])
					{
						list.Add(xmlNode.Name);
					}
					result = list.ToArray();
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static string[] GetSupportAttributes()
		{
			string[] result;
			try
			{
				List<string> list = new List<string>();
				XmlDocument xmlDocumentDTD = DrawDriverXmlOp.GetXmlDocumentDTD();
				XmlCommomOp.xmlPath = DrawDriverXmlOp.strXmlDocumentDTDFilePath;
				XmlNode[] xmlNodeByTagname = XmlCommomOp.GetXmlNodeByTagname("SupportAttribute");
				if (xmlNodeByTagname == null || xmlNodeByTagname.Length <= 0)
				{
					result = null;
				}
				else
				{
					foreach (XmlNode xmlNode in xmlNodeByTagname[0])
					{
						list.Add(xmlNode.Name);
					}
					result = list.ToArray();
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static string[] GetSupportAttributeByXmlNode(XmlNode _node)
		{
			string[] result;
			try
			{
				if (_node == null)
				{
					throw new NullReferenceException();
				}
				List<string> list = new List<string>();
				XmlDocument xmlDocumentDTD = DrawDriverXmlOp.GetXmlDocumentDTD();
				XmlCommomOp.xmlPath = DrawDriverXmlOp.strXmlDocumentDTDFilePath;
				XmlNode[] xmlNodeByTagname = XmlCommomOp.GetXmlNodeByTagname(_node.Name);
				if (xmlNodeByTagname == null || xmlNodeByTagname.Length <= 0)
				{
					result = null;
				}
				else
				{
					XmlNode[] array = xmlNodeByTagname;
					for (int i = 0; i < array.Length; i++)
					{
						XmlNode xmlNode = array[i];
						XmlNode parentNode = xmlNode.ParentNode;
						if (parentNode != null)
						{
							if (parentNode.Name.ToLower().Trim().Equals("LableAttribute".ToLower()))
							{
								XmlElement xmlElement = (XmlElement)xmlNode;
								foreach (XmlAttribute xmlAttribute in xmlElement.Attributes)
								{
									if (xmlAttribute.Name.ToLower().Equals("SupportAttribute".ToLower()))
									{
										list.AddRange(xmlAttribute.Value.Split(new char[]
										{
											','
										}));
										result = list.ToArray();
										return result;
									}
								}
							}
						}
					}
					result = null;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static string[] GetSupportChildTypeByXmlNode(XmlNode _node)
		{
			string[] result;
			try
			{
				if (_node == null)
				{
					throw new NullReferenceException();
				}
				List<string> list = new List<string>();
				XmlDocument xmlDocumentDTD = DrawDriverXmlOp.GetXmlDocumentDTD();
				XmlCommomOp.xmlPath = DrawDriverXmlOp.strXmlDocumentDTDFilePath;
				XmlNode[] xmlNodeByTagname = XmlCommomOp.GetXmlNodeByTagname(_node.Name);
				if (xmlNodeByTagname == null || xmlNodeByTagname.Length <= 0)
				{
					result = null;
				}
				else
				{
					XmlNode[] array = xmlNodeByTagname;
					for (int i = 0; i < array.Length; i++)
					{
						XmlNode xmlNode = array[i];
						XmlNode parentNode = xmlNode.ParentNode;
						if (parentNode != null)
						{
							if (parentNode.Name.ToLower().Trim().Equals("LabelContains".ToLower()))
							{
								XmlElement xmlElement = (XmlElement)xmlNode;
								foreach (XmlAttribute xmlAttribute in xmlElement.Attributes)
								{
									if (xmlAttribute.Name.ToLower().Equals("Contains".ToLower()))
									{
										string text = xmlAttribute.Value.ToLower();
										result = text.Split(new char[]
										{
											','
										});
										return result;
									}
								}
							}
						}
					}
					result = null;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static DrawType GetDrawTypeByXmlNode(XmlNode _node)
		{
			DrawType result;
			try
			{
				if (_node == null)
				{
					throw new NullReferenceException();
				}
				List<string> list = new List<string>();
				XmlDocument xmlDocumentDTD = DrawDriverXmlOp.GetXmlDocumentDTD();
				XmlCommomOp.xmlPath = DrawDriverXmlOp.strXmlDocumentDTDFilePath;
				XmlNode[] xmlNodeByTagname = XmlCommomOp.GetXmlNodeByTagname(_node.Name);
				if (xmlNodeByTagname == null || xmlNodeByTagname.Length <= 0)
				{
					result = DrawType.None;
				}
				else
				{
					XmlNode[] array = xmlNodeByTagname;
					for (int i = 0; i < array.Length; i++)
					{
						XmlNode xmlNode = array[i];
						XmlNode parentNode = xmlNode.ParentNode;
						if (parentNode != null)
						{
							if (parentNode.Name.ToLower().Trim().Equals("SupportLabels".ToLower()))
							{
								XmlElement xmlElement = (XmlElement)xmlNode;
								foreach (XmlAttribute xmlAttribute in xmlElement.Attributes)
								{
									if (xmlAttribute.Name.ToLower().Equals("DrawType".ToLower()))
									{
										string text = xmlAttribute.Value.ToLower();
										if (text.Equals("string"))
										{
											result = DrawType.String;
											return result;
										}
										if (text.Equals("line"))
										{
											result = DrawType.Line;
											return result;
										}
										if (text.Equals("image"))
										{
											result = DrawType.Image;
											return result;
										}
										if (text.Equals("fillrectangle"))
										{
											result = DrawType.FillRectangle;
											return result;
										}
										if (text.Equals("rectangle"))
										{
											result = DrawType.Rectangle;
											return result;
										}
										if (text.Equals("circle"))
										{
											result = DrawType.Circle;
											return result;
										}
										result = DrawType.None;
										return result;
									}
								}
							}
						}
					}
					result = DrawType.None;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		private static void SetStyle(ref DrawInfoString objString, string strName, XmlNode _node, string strStyle)
		{
			try
			{
				if (objString == null)
				{
					throw new NullReferenceException();
				}
				if (strName == null)
				{
					throw new NullReferenceException();
				}
				if (_node == null)
				{
					throw new NullReferenceException();
				}
				if (strStyle == null)
				{
					throw new NullReferenceException();
				}
				if (strName.Trim().ToLower().Equals("left"))
				{
					objString.Left = XmlCommomOp.GetValueByAttribute(_node, strName);
					if (objString.Left == null)
					{
						objString.Left = CommonMethods.GetStyleValue(strStyle, strName);
					}
				}
				else if (strName.Trim().ToLower().Equals("top"))
				{
					objString.Top = XmlCommomOp.GetValueByAttribute(_node, strName);
					if (objString.Top == null)
					{
						objString.Top = CommonMethods.GetStyleValue(strStyle, strName);
					}
				}
				else if (strName.Trim().ToLower().Equals("width"))
				{
					objString.Width = XmlCommomOp.GetValueByAttribute(_node, strName);
					if (objString.Width == null)
					{
						objString.Width = CommonMethods.GetStyleValue(strStyle, strName);
					}
				}
				else if (strName.Trim().ToLower().Equals("height"))
				{
					objString.Height = XmlCommomOp.GetValueByAttribute(_node, strName);
					if (objString.Height == null)
					{
						objString.Height = CommonMethods.GetStyleValue(strStyle, strName);
					}
				}
				else if (strName.Trim().ToLower().Equals("align"))
				{
					objString.Align = XmlCommomOp.GetValueByAttribute(_node, strName);
					if (objString.Align == null)
					{
						objString.Align = CommonMethods.GetStyleValue(strStyle, strName);
					}
				}
				else if (strName.Trim().ToLower().Equals("valign"))
				{
					objString.Valign = XmlCommomOp.GetValueByAttribute(_node, strName);
					if (objString.Valign == null)
					{
						objString.Valign = CommonMethods.GetStyleValue(strStyle, strName);
					}
				}
				else if (strName.Trim().ToLower().Equals("fontcolor"))
				{
					objString.FontColor = XmlCommomOp.GetValueByAttribute(_node, strName);
					if (objString.FontColor == null)
					{
						objString.FontColor = CommonMethods.GetStyleValue(strStyle, strName);
					}
				}
				else if (strName.Trim().ToLower().Equals("bgcolor"))
				{
					objString.BgColor = XmlCommomOp.GetValueByAttribute(_node, strName);
					if (objString.BgColor == null)
					{
						objString.BgColor = CommonMethods.GetStyleValue(strStyle, strName);
					}
				}
				else if (strName.Trim().ToLower().Equals("border"))
				{
					objString.Border = XmlCommomOp.GetValueByAttribute(_node, strName);
					if (objString.Border == null)
					{
						objString.Border = CommonMethods.GetStyleValue(strStyle, strName);
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static DrawInfo GetDrawInfoByXmlNode(XmlNode _node)
		{
			DrawInfo result;
			try
			{
				if (_node == null)
				{
					throw new NullReferenceException();
				}
				string valueByAttribute = XmlCommomOp.GetValueByAttribute(_node, "Style");
				if (valueByAttribute == null)
				{
					DrawDriverXmlOp.SetDrawInfoByXmlNode(_node);
				}
				DrawInfoString objString = new DrawInfoString();
				string[] supportAttributeByXmlNode = DrawDriverXmlOp.GetSupportAttributeByXmlNode(_node);
				for (int i = 0; i < supportAttributeByXmlNode.Length; i++)
				{
					DrawDriverXmlOp.SetStyle(ref objString, supportAttributeByXmlNode[i], _node, valueByAttribute);
				}
				DrawInfo drawInfo = CommonMethods.DrawInfoStringToDrawInfo(objString);
				result = drawInfo;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static bool SetDrawInfoByXmlNode(XmlNode _node)
		{
			bool result;
			try
			{
				if (_node == null)
				{
					throw new NullReferenceException();
				}
				XmlNode[] xmlNodeByTagname = XmlCommomOp.GetXmlNodeByTagname("Style");
				if (xmlNodeByTagname == null || xmlNodeByTagname.Length <= 0)
				{
					result = false;
				}
				else
				{
					string innerText = xmlNodeByTagname[0].InnerText;
					string str = "";
					XmlElement xmlElement = (XmlElement)_node;
					foreach (XmlAttribute xmlAttribute in xmlElement.Attributes)
					{
						if (xmlAttribute.Name.ToLower().Equals("Class".ToLower()))
						{
							str = xmlAttribute.Value;
						}
					}
					Regex regex = new Regex("." + str + "([^{}]*?){([^{}]*?)}");
					MatchCollection matchCollection = regex.Matches(innerText);
					if (matchCollection.Count > 0)
					{
						string[] array = matchCollection[0].Value.Trim().Replace("\t", "").Replace("\r\n", "").Split(new char[]
						{
							'{'
						});
						string attrValue = "";
						if (array.Length >= 2)
						{
							attrValue = array[1].Replace("}", "");
						}
						XmlCommomOp.SetXmlAttribute(_node, "Style", attrValue);
						result = true;
					}
					else
					{
						result = false;
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static bool SetRowsAttributeByXmlNode(string _TagName, int _TopNumber, out XmlNode[] _nodes)
		{
			bool result;
			try
			{
				if (_TagName == null)
				{
					throw new NullReferenceException();
				}
				XmlNode[] xmlNodeByTagname = XmlCommomOp.GetXmlNodeByTagname(_TagName);
				if (xmlNodeByTagname == null || xmlNodeByTagname.Length <= 0)
				{
					_nodes = null;
					result = false;
				}
				else
				{
					for (int i = 0; i < xmlNodeByTagname.Length; i++)
					{
						DrawDriverXmlOp.SetRowsAttributeByXmlNode(xmlNodeByTagname[i]);
						_TopNumber--;
						if (_TopNumber <= 0)
						{
							break;
						}
					}
					_nodes = xmlNodeByTagname;
					result = true;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static bool SetRowsAttributeByXmlNode(string _TagName)
		{
			bool result;
			try
			{
				if (_TagName == null)
				{
					throw new NullReferenceException();
				}
				XmlNode[] xmlNodeByTagname = XmlCommomOp.GetXmlNodeByTagname(_TagName);
				if (xmlNodeByTagname == null || xmlNodeByTagname.Length <= 0)
				{
					result = false;
				}
				else
				{
					for (int i = 0; i < xmlNodeByTagname.Length; i++)
					{
						DrawDriverXmlOp.SetRowsAttributeByXmlNode(xmlNodeByTagname[i]);
					}
					result = true;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		private static void NodeChildNodes(XmlNode _node)
		{
			if (_node.HasChildNodes)
			{
				DrawDriverXmlOp.SetRowsAttributeByXmlNode(_node);
				foreach (XmlNode node in _node.ChildNodes)
				{
					DrawDriverXmlOp.NodeChildNodes(node);
				}
			}
		}

		public static bool SetRowsAttributeByXmlNode(XmlDocument doc)
		{
			XmlNodeReader xmlNodeReader = null;
			bool result;
			try
			{
				if (doc == null)
				{
					throw new NullReferenceException();
				}
				foreach (XmlNode node in doc.ChildNodes)
				{
					DrawDriverXmlOp.NodeChildNodes(node);
				}
				result = true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (xmlNodeReader != null)
				{
					xmlNodeReader.Close();
				}
			}
			return result;
		}

		private static bool YNHaveAttribute(XmlNode _node, string strAttributeName)
		{
			bool result;
			if (_node.NodeType == XmlNodeType.Text)
			{
				result = false;
			}
			else
			{
				XmlElement xmlElement = (XmlElement)_node;
				foreach (XmlAttribute xmlAttribute in xmlElement.Attributes)
				{
					if (xmlAttribute.Name.ToLower().Equals(strAttributeName.ToLower()))
					{
						result = true;
						return result;
					}
				}
				result = false;
			}
			return result;
		}

		public static string GetStyle(string[,] strss, string name)
		{
			string result;
			for (int i = 0; i < strss.Length / 2; i++)
			{
				if (strss[i, 0].ToString().Equals(name.ToLower()))
				{
					result = strss[i, 1];
					return result;
				}
			}
			result = "";
			return result;
		}

		public static bool SetRowsAttributeByXmlNode(XmlNode _node)
		{
			bool result;
			try
			{
				if (_node == null)
				{
					throw new NullReferenceException();
				}
				string input = "";
				XmlNode[] xmlNodeByTagname = XmlCommomOp.GetXmlNodeByTagname("Style");
				if (xmlNodeByTagname != null && xmlNodeByTagname.Length > 0)
				{
					input = xmlNodeByTagname[0].InnerText;
				}
				string name = _node.Name;
				string str = "";
				string text = "";
				XmlElement xmlElement = (XmlElement)_node;
				foreach (XmlAttribute xmlAttribute in xmlElement.Attributes)
				{
					if (xmlAttribute.Name.ToLower().Equals("Class".ToLower()))
					{
						str = xmlAttribute.Value;
					}
					if (xmlAttribute.Name.ToLower().Equals("Style".ToLower()))
					{
						text = xmlAttribute.Value;
					}
				}
				XmlCommomOp.DeleteNodeAttribute(_node, "Class");
				XmlCommomOp.DeleteNodeAttribute(_node, "Style");
				if (text.Trim().Length > 0)
				{
					string[] array = text.Split(new char[]
					{
						';'
					});
					for (int i = 0; i < array.Length; i++)
					{
						string[] array2 = array[i].Split(new char[]
						{
							':'
						});
						if (array2.Length >= 2)
						{
							if (!DrawDriverXmlOp.YNHaveAttribute(_node, array2[0].Trim().ToLower()))
							{
								XmlCommomOp.SetXmlAttribute(_node, array2[0].Trim().ToLower(), array2[1].ToLower());
							}
						}
					}
				}
				Regex regex = new Regex("([^{}\\s]*?)([^{}]*?){([^{}]*?)}");
				MatchCollection matchCollection = regex.Matches(input);
				string[,] array3 = new string[matchCollection.Count, 2];
				for (int i = 0; i < matchCollection.Count; i++)
				{
					string[] array4 = matchCollection[i].Value.Trim().Replace("\t", "").Replace("\r\n", "").Split(new char[]
					{
						'{'
					});
					if (array4.Length >= 2)
					{
						array3[i, 0] = array4[0].Trim();
						array3[i, 1] = array4[1].Replace("}", "");
					}
				}
				string[] array5 = DrawDriverXmlOp.GetStyle(array3, "." + str).Split(new char[]
				{
					';'
				});
				for (int i = 0; i < array5.Length; i++)
				{
					string[] array2 = array5[i].Split(new char[]
					{
						':'
					});
					if (array2.Length >= 2)
					{
						if (!DrawDriverXmlOp.YNHaveAttribute(_node, array2[0].Trim().ToLower()))
						{
							XmlCommomOp.SetXmlAttribute(_node, array2[0].Trim().ToLower(), array2[1].ToLower());
						}
					}
				}
				array5 = DrawDriverXmlOp.GetStyle(array3, name).Split(new char[]
				{
					';'
				});
				for (int i = 0; i < array5.Length; i++)
				{
					string[] array2 = array5[i].Split(new char[]
					{
						':'
					});
					if (array2.Length >= 2)
					{
						if (!DrawDriverXmlOp.YNHaveAttribute(_node, array2[0].Trim().ToLower()))
						{
							XmlCommomOp.SetXmlAttribute(_node, array2[0].Trim().ToLower(), array2[1].ToLower());
						}
					}
				}
				result = false;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}
	}
}
