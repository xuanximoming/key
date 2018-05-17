using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml;

namespace DrectSoft.DrawDriver
{
	public class XmlCommomOp
	{
		public static string xmlPath = "";

		private static XmlDocument doc = null;

		public static XmlDocument Doc
		{
			get
			{
				return XmlCommomOp.doc;
			}
			set
			{
				XmlCommomOp.doc = value;
			}
		}

		private XmlCommomOp()
		{
		}

		public static void CreaetDocument()
		{
			try
			{
				if (XmlCommomOp.doc == null)
				{
					XmlCommomOp.doc = new XmlDocument();
					XmlCommomOp.doc.Load(XmlCommomOp.xmlPath);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private static void SaveDocument()
		{
			try
			{
				if (XmlCommomOp.doc != null && !string.IsNullOrEmpty(XmlCommomOp.xmlPath))
				{
					XmlCommomOp.doc.Save(XmlCommomOp.xmlPath);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private static XmlNode[] IlistToArray(XmlNodeList nodeList)
		{
			XmlNode[] result;
			try
			{
				if (nodeList != null && nodeList.Count > 0)
				{
					XmlNode[] array = new XmlNode[nodeList.Count];
					for (int i = 0; i < nodeList.Count; i++)
					{
						array[i] = nodeList[i];
					}
					result = array;
				}
				else
				{
					result = null;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static XmlNode[] GetXmlNodeByTagname(string _tagname)
		{
			XmlNode[] result;
			try
			{
				XmlCommomOp.CreaetDocument();
				try
				{
					List<XmlNode> list = new List<XmlNode>();
					list.Clear();
					if (XmlCommomOp.doc == null || _tagname.Trim() == "")
					{
						result = list.ToArray();
					}
					else
					{
						foreach (XmlNode node in XmlCommomOp.doc.ChildNodes)
						{
							XmlCommomOp.GetNodeByTagNameIgnoreCase(node, _tagname, list);
						}
						result = list.ToArray();
					}
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
			catch (Exception ex2)
			{
				throw ex2;
			}
			return result;
		}

		public static XmlNode[] GetXmlNodeByName(string _namevalue)
		{
			XmlNode[] result;
			try
			{
				XmlCommomOp.CreaetDocument();
				List<XmlNode> list = new List<XmlNode>();
				list.Clear();
				if (XmlCommomOp.doc == null || _namevalue.Trim() == "")
				{
					result = list.ToArray();
				}
				else
				{
					foreach (XmlNode node in XmlCommomOp.doc.ChildNodes)
					{
						XmlCommomOp.GetNodeByAttributesIgnoreCase(node, "Name", _namevalue, list);
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

		public static bool SetXmlNodeByParamName(string _paramname, string _paramvalue)
		{
			bool result;
			try
			{
				XmlCommomOp.CreaetDocument();
				List<XmlNode> list = new List<XmlNode>();
				list.Clear();
				if (XmlCommomOp.doc == null || _paramname.Trim() == "")
				{
					result = false;
				}
				else
				{
					foreach (XmlNode node in XmlCommomOp.doc.ChildNodes)
					{
						XmlCommomOp.GetNodeOfInnerTextByParamname(node, _paramname, list);
					}
					foreach (XmlNode current in list)
					{
						current.InnerText = current.InnerText.Replace("{@" + _paramname + "}", _paramvalue);
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

		public static XmlNode[] GetXmlNodeById(string _idvalue)
		{
			XmlNode[] result;
			try
			{
				XmlCommomOp.CreaetDocument();
				List<XmlNode> list = new List<XmlNode>();
				list.Clear();
				if (XmlCommomOp.doc == null || _idvalue.Trim() == "")
				{
					result = list.ToArray();
				}
				else
				{
					foreach (XmlNode node in XmlCommomOp.doc.ChildNodes)
					{
						XmlCommomOp.GetNodeByAttributesIgnoreCase(node, "Id", _idvalue, list);
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

		public static XmlNode[] GetChildNode(XmlNode _node)
		{
			XmlNode[] result;
			try
			{
				if (_node == null || !_node.HasChildNodes)
				{
					result = null;
				}
				else
				{
					XmlNode[] array = new XmlNode[_node.ChildNodes.Count];
					for (int i = 0; i < _node.ChildNodes.Count; i++)
					{
						array[i] = _node.ChildNodes[i];
					}
					result = array;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static void GetAllChildNode(XmlNode _node, ref List<XmlNode> nodesList)
		{
			try
			{
				if (_node.HasChildNodes)
				{
					foreach (XmlNode node in _node.ChildNodes)
					{
						XmlCommomOp.GetAllChildNode(node, ref nodesList);
					}
				}
				else
				{
					nodesList.Add(_node);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static XmlAttribute[] GetXmlAttribute(XmlNode _node)
		{
			XmlAttribute[] result;
			try
			{
				if (_node == null || _node.Attributes.Count == 0)
				{
					result = null;
				}
				else
				{
					XmlAttribute[] array = new XmlAttribute[_node.Attributes.Count];
					for (int i = 0; i < _node.Attributes.Count; i++)
					{
						array[i] = _node.Attributes[i];
					}
					result = array;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static bool SetXmlAttribute(XmlNode _node, string _attrName, string _attrValue)
		{
			bool result;
			try
			{
				if (_node == null || string.IsNullOrEmpty(_attrName) || _attrValue == null)
				{
					result = false;
				}
				else
				{
					XmlAttributeCollection attributes = _node.Attributes;
					if (attributes != null && attributes.Count > 0)
					{
						foreach (XmlAttribute xmlAttribute in attributes)
						{
							if (xmlAttribute.Name.ToLower().Equals(_attrName.ToLower()))
							{
								xmlAttribute.Value = _attrValue;
								result = true;
								return result;
							}
						}
					}
					if (_node.NodeType != XmlNodeType.Text)
					{
						((XmlElement)_node).SetAttribute(_attrName.ToLower(), _attrValue);
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

		public static string GetValueByAttribute(XmlNode _node, string _attributename)
		{
			string result;
			try
			{
				if (_node == null || _node.Attributes == null || _node.Attributes.Count == 0)
				{
					result = null;
				}
				else
				{
					for (int i = 0; i < _node.Attributes.Count; i++)
					{
						if (_node.Attributes[i].Name.ToLower().Equals(_attributename.ToLower()))
						{
							result = _node.Attributes[i].Value;
							return result;
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

		public static bool DeleteNodeAttribute(XmlNode _node, string _attributename)
		{
			bool result;
			try
			{
				if (_node == null || _node.Attributes.Count == 0)
				{
					result = true;
				}
				else
				{
					for (int i = _node.Attributes.Count - 1; i >= 0; i--)
					{
						if (_node.Attributes[i].Name.ToLower().Equals(_attributename.ToLower()))
						{
							_node.Attributes.Remove(_node.Attributes[i]);
						}
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

		public static bool AddChildNode(XmlNode _curretnode, XmlNode _addchildnode)
		{
			bool result;
			try
			{
				if (_curretnode != null && _addchildnode != null)
				{
					_curretnode.AppendChild(_addchildnode);
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

		public static bool AddChildNode(XmlNode _curretnode, string _tag, string _innerText)
		{
			bool result;
			try
			{
				XmlCommomOp.CreaetDocument();
				if (_curretnode != null && !string.IsNullOrEmpty(_tag) && !string.IsNullOrEmpty(_innerText))
				{
					XmlElement xmlElement = XmlCommomOp.doc.CreateElement(_tag);
					xmlElement.InnerText = _innerText;
					_curretnode.AppendChild(xmlElement);
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

		public static bool DelChildNode(XmlNode _curretnode, XmlNode _delchildnode)
		{
			bool result;
			if (_curretnode != null && _delchildnode != null)
			{
				_curretnode.RemoveChild(_delchildnode);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		public static bool SetAttributeValueByXmlNode(XmlNode _currentnode, string _attribute, string _newvalue, XmlDocument _doc)
		{
			bool result;
			try
			{
				if (_currentnode == null || _currentnode.Attributes.Count == 0)
				{
					result = false;
				}
				else
				{
					for (int i = 0; i < _currentnode.Attributes.Count; i++)
					{
						if (_currentnode.Attributes[i].Name.ToLower().Equals(_attribute.ToLower()))
						{
							_currentnode.Attributes[i].Value = _newvalue;
							result = true;
							return result;
						}
					}
					result = false;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static bool SetAttributeValueByXmlNode(XmlNode _currentnode, string _attribute, string _newvalue)
		{
			bool result;
			try
			{
				if (_currentnode == null || _currentnode.Attributes.Count == 0)
				{
					result = false;
				}
				else
				{
					for (int i = 0; i < _currentnode.Attributes.Count; i++)
					{
						if (_currentnode.Attributes[i].Name.ToLower().Equals(_attribute.ToLower()))
						{
							_currentnode.Attributes[i].Value = _newvalue;
							result = true;
							return result;
						}
					}
					result = false;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		private static void GetNodeByTagNameIgnoreCase(XmlNode _node, string _tagname, List<XmlNode> _lst)
		{
			try
			{
				if (_node.HasChildNodes)
				{
					foreach (XmlNode node in _node.ChildNodes)
					{
						XmlCommomOp.GetNodeByTagNameIgnoreCase(node, _tagname, _lst);
					}
					if (_node.LocalName.ToLower() == _tagname.ToLower())
					{
						_lst.Add(_node);
					}
				}
				else if (_node.LocalName.ToLower() == _tagname.ToLower())
				{
					_lst.Add(_node);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private static void GetNodeOfInnerTextByParamname(XmlNode _node, string _paramname, List<XmlNode> _lst)
		{
			try
			{
				if (_node.HasChildNodes)
				{
					foreach (XmlNode node in _node.ChildNodes)
					{
						XmlCommomOp.GetNodeOfInnerTextByParamname(node, _paramname, _lst);
					}
				}
				else if (_node.NodeType == XmlNodeType.Text)
				{
					if (_node.ParentNode.InnerText.Contains("{@" + _paramname + "}"))
					{
						_lst.Add(_node);
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private static void GetNodeByAttributesIgnoreCase(XmlNode _node, string _attributesname, string _attributesvalue, List<XmlNode> _lst)
		{
			try
			{
				if (_node.HasChildNodes)
				{
					foreach (XmlNode node in _node.ChildNodes)
					{
						XmlCommomOp.GetNodeByAttributesIgnoreCase(node, _attributesname, _attributesvalue, _lst);
					}
					if (_node.Attributes != null)
					{
						foreach (XmlAttribute xmlAttribute in _node.Attributes)
						{
							if (xmlAttribute.Name.ToLower() == _attributesname.ToLower() && xmlAttribute.Value == _attributesvalue)
							{
								_lst.Add(_node);
								break;
							}
						}
					}
				}
				else if (_node.Attributes != null)
				{
					foreach (XmlAttribute xmlAttribute in _node.Attributes)
					{
						if (xmlAttribute.Name.ToLower() == _attributesname.ToLower() && xmlAttribute.Value == _attributesvalue)
						{
							_lst.Add(_node);
							break;
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static bool CopyTempalteXmlFile(string _sourceFilePath, string _desFilePath)
		{
			bool result;
			try
			{
				File.Copy(_sourceFilePath, _desFilePath, true);
				result = true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		private static bool CreateTrAndTh(XmlNode curNode, DataTable dt)
		{
			bool result;
			try
			{
				Dictionary<int, XmlNode> dictionary = null;
				Font font = null;
				int num = 0;
				int num2 = 0;
				string innerText = (curNode.Attributes["defaultValue"] == null || curNode.Attributes["defaultValue"].Value == "") ? "" : curNode.Attributes["defaultValue"].Value;
				string text = (curNode.Attributes["FillRowIndex"] == null || curNode.Attributes["FillRowIndex"].Value == "") ? dt.Rows.Count.ToString() : curNode.Attributes["FillRowIndex"].Value;
				List<string> list = new List<string>();
				if (text.Contains(","))
				{
					list = text.Split(new char[]
					{
						','
					}).ToList<string>();
				}
				else
				{
					for (int i = 0; i < int.Parse(text); i++)
					{
						list.Add(i.ToString());
					}
				}
				bool flag = false;
				if (curNode != null && curNode.SelectSingleNode("TableBody") != null)
				{
					num2 = int.Parse(curNode.SelectSingleNode("TableBody").FirstChild.Attributes["height"].Value);
					string familyName = (curNode.SelectSingleNode("TableBody").Attributes["font-family"] == null || curNode.SelectSingleNode("TableBody").Attributes["font-family"].Value == "") ? "新宋体" : curNode.SelectSingleNode("TableBody").Attributes["font-family"].Value;
					string s = (curNode.SelectSingleNode("TableBody").Attributes["font-size"] == null || curNode.SelectSingleNode("TableBody").Attributes["font-size"].Value == "") ? "12" : curNode.SelectSingleNode("TableBody").Attributes["font-size"].Value;
					font = new Font(familyName, float.Parse(s), FontStyle.Regular, GraphicsUnit.Pixel);
					XmlNode xmlNode = curNode.SelectSingleNode("TableBody");
					foreach (XmlNode oldChild in xmlNode.ChildNodes)
					{
						xmlNode.RemoveChild(oldChild);
					}
					dictionary = XmlCommomOp.GetColumn(curNode);
				}
				if (curNode != null && curNode.SelectSingleNode("TableHead") != null)
				{
					foreach (XmlNode xmlNode2 in curNode.SelectSingleNode("TableHead").ChildNodes)
					{
						num += int.Parse((xmlNode2.Attributes["height"] == null || xmlNode2.Attributes["height"].Value == "") ? "0" : xmlNode2.Attributes["height"].Value);
					}
				}
				if (curNode != null && dictionary.Count > 0 && dt.Rows.Count > 0)
				{
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						num += num2;
						XmlElement xmlElement = XmlCommomOp.Doc.CreateElement("Tr");
						xmlElement.SetAttribute("height", num2.ToString().Trim());
						int num3 = 0;
						for (int j = 1; j <= dictionary.Count; j++)
						{
							XmlElement xmlElement2 = XmlCommomOp.Doc.CreateElement("Td");
							string value = dictionary[j].Attributes["ColumnName"].Value;
							int width = int.Parse(dictionary[j].Attributes["width"].Value);
							int num4 = int.Parse((dictionary[j].Attributes["Wrap"] == null || dictionary[j].Attributes["Wrap"].Value == "") ? "0" : dictionary[j].Attributes["Wrap"].Value);
							string text2 = dt.Rows[i][value].ToString().Trim();
							List<string> list2 = CommonMethods.WrapLongString(font, text2, width);
							if (num4 == 1 && list2.Count > 1)
							{
								if (num3 < list2.Count)
								{
									dt.Rows[i][value] = list2[0];
									for (int k = 1; k < list2.Count - num3; k++)
									{
										DataRow dataRow = dt.NewRow();
										dataRow[value] = list2[k];
										dt.Rows.InsertAt(dataRow, i + k);
									}
									num3 = list2.Count;
								}
								else
								{
									for (int k = 0; k < list2.Count; k++)
									{
										dt.Rows[i + k][value] = list2[k];
									}
								}
							}
							if (list.Contains(i.ToString()))
							{
								if (string.IsNullOrEmpty(dt.Rows[i][value].ToString()))
								{
									if (flag)
									{
										xmlElement2.InnerText = dt.Rows[i][value].ToString();
									}
									else
									{
										xmlElement2.InnerText = innerText;
									}
								}
								else
								{
									xmlElement2.InnerText = dt.Rows[i][value].ToString();
									flag = true;
								}
							}
							else
							{
								xmlElement2.InnerText = dt.Rows[i][value].ToString();
							}
							xmlElement.AppendChild(xmlElement2);
						}
						curNode.SelectSingleNode("TableBody").AppendChild(xmlElement);
						flag = false;
					}
					XmlCommomOp.SetXmlAttribute(curNode, "height", num.ToString().Trim());
					result = true;
				}
				else
				{
					XmlCommomOp.SetXmlAttribute(curNode, "height", num.ToString().Trim());
					result = false;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static Dictionary<int, XmlNode> GetColumn(XmlNode xmlNode)
		{
			Dictionary<int, XmlNode> result;
			try
			{
				Dictionary<int, List<int>> tableModel = XmlCommomOp.GetTableModel(xmlNode);
				XmlNode xmlNode2 = xmlNode.SelectNodes("TableHead")[0];
				XmlNodeList childNodes = xmlNode2.ChildNodes;
				int count = childNodes.Count;
				Dictionary<int, XmlNode> dictionary = new Dictionary<int, XmlNode>();
				for (int i = 0; i < count; i++)
				{
					int num = 0;
					for (int j = 0; j < childNodes[i].ChildNodes.Count; j++)
					{
						int num2 = (childNodes[i].ChildNodes[j].Attributes["colspan"] == null) ? 1 : int.Parse(childNodes[i].ChildNodes[j].Attributes["colspan"].Value);
						if (childNodes[i].ChildNodes[j].Attributes["ColumnName"] != null)
						{
							dictionary.Add(tableModel[i + 1][num], childNodes[i].ChildNodes[j]);
							if (num < tableModel[i + 1].Count)
							{
								num += num2;
							}
						}
						else
						{
							num += num2;
						}
					}
				}
				result = dictionary;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static Dictionary<int, List<int>> GetTableModel(XmlNode xmlNode)
		{
			Dictionary<int, List<int>> result;
			try
			{
				XmlNode xmlNode2 = xmlNode.SelectNodes("TableHead")[0];
				XmlNodeList childNodes = xmlNode2.ChildNodes;
				int count = childNodes.Count;
				int num = 0;
				Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
				foreach (XmlNode xmlNode3 in childNodes[0])
				{
					if (xmlNode3.Attributes["colspan"] != null)
					{
						num += int.Parse(xmlNode3.Attributes["colspan"].Value);
					}
					else
					{
						num++;
					}
				}
				for (int i = 1; i <= count; i++)
				{
					List<int> list = new List<int>();
					for (int j = 1; j <= num; j++)
					{
						list.Add(j);
					}
					dictionary.Add(i, list);
				}
				for (int k = 1; k <= count; k++)
				{
					int num2 = 0;
					int num3 = dictionary[k][num2] - 1;
					foreach (XmlNode xmlNode4 in childNodes[k - 1].ChildNodes)
					{
						int num4 = (xmlNode4.Attributes["rowspan"] == null) ? 1 : int.Parse(xmlNode4.Attributes["rowspan"].Value);
						int num5 = (xmlNode4.Attributes["colspan"] == null) ? 1 : int.Parse(xmlNode4.Attributes["colspan"].Value);
						for (int l = 1; l <= num4 - 1; l++)
						{
							int num6 = num3 + 1;
							if (num5 >= 1)
							{
								for (int m = 0; m < num5; m++)
								{
									dictionary[k + l].Remove(num6 + m);
								}
							}
						}
						num2 += num5;
						num3 = dictionary[k][num2 - 1];
					}
				}
				for (int j = 1; j <= count; j++)
				{
					int num2 = 0;
					int num3 = dictionary[j][num2] - 1;
					foreach (XmlNode xmlNode5 in childNodes[j - 1].ChildNodes)
					{
						int num5 = (xmlNode5.Attributes["colspan"] == null) ? 1 : int.Parse(xmlNode5.Attributes["colspan"].Value);
						if (num5 >= 2)
						{
							for (int k = 0; k < num5 - 1; k++)
							{
								dictionary[j][num2 + k + 1] = 0;
							}
						}
						num2 += num5;
						num3 = dictionary[j][num2 - 1];
					}
				}
				result = dictionary;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static bool FormateTableNodeValue(DataSet datatables)
		{
			bool result;
			try
			{
				XmlNode[] xmlNodeByTagname = XmlCommomOp.GetXmlNodeByTagname("Table");
				XmlNode[] array = xmlNodeByTagname;
				for (int i = 0; i < array.Length; i++)
				{
					XmlNode xmlNode = array[i];
					if (xmlNode.Attributes["datasource"] != null && !string.IsNullOrEmpty(xmlNode.Attributes["datasource"].Value))
					{
						XmlCommomOp.CreateTrAndTh(xmlNode, datatables.Tables[xmlNode.Attributes["datasource"].Value]);
					}
				}
				result = true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		private static bool FormateParamtersValue(Dictionary<string, ParamObject> dicParamsList)
		{
			bool result;
			try
			{
				XmlNode[] xmlNodeByTagname = XmlCommomOp.GetXmlNodeByTagname("Parameters");
				if (xmlNodeByTagname == null || xmlNodeByTagname.Length == 0)
				{
					result = false;
				}
				else
				{
					XmlNode xmlNode = xmlNodeByTagname[0];
					foreach (XmlNode xmlNode2 in xmlNode.ChildNodes)
					{
						if (xmlNode2.Attributes["ParameterName"] != null && !string.IsNullOrEmpty(xmlNode2.Attributes["ParameterName"].Value))
						{
							if (!XmlCommomOp.IsFillValue(xmlNode2, dicParamsList))
							{
								XmlCommomOp.SetXmlNodeByParamName(xmlNode2.Attributes["ParameterName"].Value, xmlNode2.Attributes["DefaultValue"].Value);
							}
						}
					}
					result = false;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static bool IsFillValue(XmlNode node, Dictionary<string, ParamObject> dicParamsList)
		{
			bool result;
			try
			{
				bool flag = false;
				foreach (KeyValuePair<string, ParamObject> current in dicParamsList)
				{
					if (current.Key.Trim().Equals(node.Attributes["ParameterName"].Value) && current.Value.Value.Trim() != "")
					{
						XmlCommomOp.SetXmlNodeByParamName(current.Key, current.Value.Value);
						flag = true;
					}
				}
				result = flag;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static void BindingDate(DataSet datatables, Dictionary<string, ParamObject> dicParamsList)
		{
			try
			{
				if (datatables != null && datatables.Tables != null && datatables.Tables.Count > 0)
				{
					XmlCommomOp.FormateTableNodeValue(datatables);
				}
				if (dicParamsList != null && dicParamsList.Count > 0)
				{
					XmlCommomOp.FormateParamtersValue(dicParamsList);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
