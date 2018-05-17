using System;

namespace CommonLib
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class BindFieldAttribute : Attribute
	{
		private bool bolKey = false;

		private string strDbType = "varchar2";

		private string strChName = "";

		private string strDefaultValue = "";

		private string strName = "";

		private string strReadFormat = "";

		private string strWriteFormat = "";

		private string strControlType = "";

		public string ChName
		{
			get
			{
				return this.strChName;
			}
			set
			{
				this.strChName = value;
			}
		}

		public string DbType
		{
			get
			{
				return this.strDbType;
			}
			set
			{
				this.strDbType = value;
			}
		}

		public string DefaultValue
		{
			get
			{
				return this.strDefaultValue;
			}
			set
			{
				this.strDefaultValue = value;
			}
		}

		public bool Key
		{
			get
			{
				return this.bolKey;
			}
			set
			{
				this.bolKey = value;
			}
		}

		public string Name
		{
			get
			{
				return this.strName;
			}
		}

		public string ReadFormat
		{
			get
			{
				return this.strReadFormat;
			}
			set
			{
				this.strReadFormat = value;
			}
		}

		public string WriteFormat
		{
			get
			{
				return this.strWriteFormat;
			}
			set
			{
				this.strWriteFormat = value;
			}
		}

		public string ControlType
		{
			get
			{
				return this.strControlType;
			}
			set
			{
				this.strControlType = value;
			}
		}

		public BindFieldAttribute()
		{
		}

		public BindFieldAttribute(string name)
		{
			this.strName = name;
		}
	}
}
