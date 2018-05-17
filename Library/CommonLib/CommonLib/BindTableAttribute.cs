using System;

namespace CommonLib
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class BindTableAttribute : Attribute
	{
		private string strChName;

		private string strName;

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

		public string Name
		{
			get
			{
				return this.strName;
			}
		}

		public BindTableAttribute()
		{
		}

		public BindTableAttribute(string name)
		{
			this.strName = name;
		}
	}
}
