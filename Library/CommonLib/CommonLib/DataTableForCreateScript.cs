using System;
using System.Data;

namespace CommonLib
{
	public sealed class DataTableForCreateScript : DataTable
	{
		public DataTableForCreateScript()
		{
			DataColumn dataColumn = new DataColumn("ID");
			dataColumn.DataType = typeof(string);
			dataColumn.AllowDBNull = false;
			base.Columns.Add(dataColumn);
			dataColumn = new DataColumn("COLUMNNAME");
			dataColumn.DataType = typeof(string);
			dataColumn.AllowDBNull = false;
			base.Columns.Add(dataColumn);
			dataColumn = new DataColumn("SHORTNAME");
			dataColumn.DataType = typeof(string);
			dataColumn.AllowDBNull = true;
			base.Columns.Add(dataColumn);
			dataColumn = new DataColumn("DATATYPE");
			dataColumn.DataType = typeof(string);
			dataColumn.AllowDBNull = true;
			base.Columns.Add(dataColumn);
			dataColumn = new DataColumn("DATALEN");
			dataColumn.DataType = typeof(string);
			dataColumn.AllowDBNull = true;
			base.Columns.Add(dataColumn);
			dataColumn = new DataColumn("DEFUALTVALUE");
			dataColumn.DataType = typeof(string);
			dataColumn.AllowDBNull = true;
			base.Columns.Add(dataColumn);
			dataColumn = new DataColumn("ALLOWNULL");
			dataColumn.DataType = typeof(int);
			dataColumn.AllowDBNull = true;
			base.Columns.Add(dataColumn);
			dataColumn = new DataColumn("ISPRIMARY");
			dataColumn.DataType = typeof(int);
			dataColumn.AllowDBNull = true;
			base.Columns.Add(dataColumn);
			dataColumn = new DataColumn("REMARK");
			dataColumn.DataType = typeof(string);
			dataColumn.AllowDBNull = true;
			base.Columns.Add(dataColumn);
		}
	}
}
