using System;

namespace DrectSoft.DrawDriver
{
	public struct ParamObject
	{
		public string ParameterName;

		public string DataType;

		public string Value;

		public ParamObject(string _ParameterName, string _DataType, string _Value)
		{
			this.ParameterName = _ParameterName;
			this.DataType = _DataType;
			this.Value = _Value;
		}
	}
}
