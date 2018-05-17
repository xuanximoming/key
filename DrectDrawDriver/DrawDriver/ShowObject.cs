using System;

namespace DrectSoft.DrawDriver
{
	public struct ShowObject
	{
		public string DrawType;

		public string ParameterName;

		public ShowObject(string _drawType, string _parameter)
		{
			this.DrawType = _drawType;
			this.ParameterName = _parameter;
		}
	}
}
