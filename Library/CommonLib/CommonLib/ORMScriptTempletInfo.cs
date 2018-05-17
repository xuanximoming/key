using System;

namespace CommonLib
{
	public class ORMScriptTempletInfo
	{
		public static string TablenamePlaceholder = "{tablename}";

		public static string NamespacePlaceholder = "{namespace}";

		public static string ClassnamePlaceholder = "{classname}";

		public static string ClassChnamePlaceholder = "{classchname}";

		public static string ClassRemarkPlaceholder = "{classremark}";

		public static string CreatorPlaceholder = "{creator}";

		public static string CreatetimePlaceholder = "{creattime}";

		public static string PropertyRemarkPlaceholder = "{propertyremark}";

		public static string PropertyNamePlaceholder = "{propertyname}";

		public static string PropertyChnamePlaceholder = "{propertychname}";

		public static string PropertyDefaultValuePlaceholder = "{propertydefaultvalue}";

		public static string PropertyDbTypePlaceholder = "{propertydbtype}";

		public static string PropertyKeyPlaceholder = "{propertykey}";

		public static string PropertyDatatypePlaceholder = "{propertydatatype}";

		public static string FieldInitValuePlaceholder = "{fieldinitvalue}";

		public static string ImportDllRemark = "using System;\r\nusing System.Text;\r\nusing CommonLib;";

		public static string TempletClassSummary = string.Concat(new string[]
		{
			"/// <summary>\r\n/// 功能描述：",
			ORMScriptTempletInfo.ClassRemarkPlaceholder,
			"\r\n/// 创 建 者：",
			ORMScriptTempletInfo.CreatorPlaceholder,
			"\r\n/// 创建日期：",
			ORMScriptTempletInfo.CreatetimePlaceholder,
			"\r\n/// </summary>"
		});

		public static string TempletClassSerializable = string.Concat(new string[]
		{
			"[Serializable, BindTable(\"",
			ORMScriptTempletInfo.ClassnamePlaceholder,
			"\", ChName=\"",
			ORMScriptTempletInfo.ClassChnamePlaceholder,
			"\")]"
		});

		public static string TempletPropertySummary = "\t///<summary>\r\n\t///" + ORMScriptTempletInfo.PropertyRemarkPlaceholder + "\r\n\t///</summary>";

		public static string TempletPropertyBindField = string.Format("\t[BindField(\"{0}\", ChName = \"{1}\", DefaultValue = \"{2}\", DbType = \"{3}\", Key = {4})]", new object[]
		{
			ORMScriptTempletInfo.PropertyNamePlaceholder,
			ORMScriptTempletInfo.PropertyChnamePlaceholder,
			ORMScriptTempletInfo.PropertyDefaultValuePlaceholder,
			ORMScriptTempletInfo.PropertyDbTypePlaceholder,
			ORMScriptTempletInfo.PropertyKeyPlaceholder
		});

		public static string TempletField = string.Format("\tprivate {0} _{1}{2};", ORMScriptTempletInfo.PropertyDatatypePlaceholder, ORMScriptTempletInfo.PropertyNamePlaceholder, ORMScriptTempletInfo.FieldInitValuePlaceholder);

		public static string TempletProperty = string.Concat(new string[]
		{
			"\tpublic ",
			ORMScriptTempletInfo.PropertyDatatypePlaceholder,
			" ",
			ORMScriptTempletInfo.PropertyNamePlaceholder,
			"\r\n\t{\r\n\t\tget\r\n\t\t{\r\n\t\t\treturn this._",
			ORMScriptTempletInfo.PropertyNamePlaceholder,
			";\r\n\t\t}\r\n\t\tset\r\n\t\t{\r\n\t\t\tthis._",
			ORMScriptTempletInfo.PropertyNamePlaceholder,
			" = value;\r\n\t\t}\r\n\t}\r\n"
		});
	}
}
