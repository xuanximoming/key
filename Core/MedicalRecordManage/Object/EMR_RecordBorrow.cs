using System;
using System.Text;
using CommonLib;
namespace MedicalRecordManage.Object
{
/// <summary>
/// 功能描述：
/// 创 建 者：Roger
/// 创建日期：2013-03-07 10:24
/// </summary>
[Serializable, BindTable("EMR_RecordBorrow", ChName="并按借阅记录表")]
public class EMR_RecordBorrow
{
	private string _ID;
	///<summary>
	///Id,Id
	///</summary>
	[BindField("ID", ChName = "Id", DefaultValue = "SYS_GUID()", DbType = "VarChar", Key = true)]
	public string ID
	{
		get
		{
			return this._ID;
		}
		set
		{
			this._ID = value;
		}
	}


	private string _NOOFINPAT="";
	///<summary>
	///病人主键
	///</summary>
	[BindField("NOOFINPAT", ChName = "", DefaultValue = "''", DbType = "VarChar", Key = false)]
	public string NOOFINPAT
	{
		get
		{
			return this._NOOFINPAT;
		}
		set
		{
			this._NOOFINPAT = value;
		}
	}


	private DateTime _APPLYDATE;
	///<summary>
	///申请时间
	///</summary>
	[BindField("APPLYDATE", ChName = "", DefaultValue = "SysDate", DbType = "DateTime", Key = false)]
	public DateTime APPLYDATE
	{
		get
		{
			return this._APPLYDATE;
		}
		set
		{
			this._APPLYDATE = value;
		}
	}


	private string _APPLYDOCID="";
	///<summary>
	///申请医生Id
	///</summary>
	[BindField("APPLYDOCID", ChName = "", DefaultValue = "''", DbType = "VarChar", Key = false)]
	public string APPLYDOCID
	{
		get
		{
			return this._APPLYDOCID;
		}
		set
		{
			this._APPLYDOCID = value;
		}
	}


	private string _APPLYCONTENT="";
	///<summary>
	///申请内容
	///</summary>
	[BindField("APPLYCONTENT", ChName = "", DefaultValue = "''", DbType = "VarChar", Key = false)]
	public string APPLYCONTENT
	{
		get
		{
			return this._APPLYCONTENT;
		}
		set
		{
			this._APPLYCONTENT = value;
		}
	}


	private int _APPLYTIMES;
	///<summary>
	///申请期限
	///</summary>
	[BindField("APPLYTIMES", ChName = "", DefaultValue = "0", DbType = "Int16", Key = false)]
	public int APPLYTIMES
	{
		get
		{
			return this._APPLYTIMES;
		}
		set
		{
			this._APPLYTIMES = value;
		}
	}


	private string _APPROVEDOCID="";
	///<summary>
	///审核者
	///</summary>
	[BindField("APPROVEDOCID", ChName = "", DefaultValue = "''", DbType = "VarChar", Key = false)]
	public string APPROVEDOCID
	{
		get
		{
			return this._APPROVEDOCID;
		}
		set
		{
			this._APPROVEDOCID = value;
		}
	}


	private DateTime _APPROVEDATE;
	///<summary>
	///审核时间
	///</summary>
	[BindField("APPROVEDATE", ChName = "", DefaultValue = "SysDate", DbType = "DateTime", Key = false)]
	public DateTime APPROVEDATE
	{
		get
		{
			return this._APPROVEDATE;
		}
		set
		{
			this._APPROVEDATE = value;
		}
	}


	private string _APPROVECONTENT="";
	///<summary>
	///审核不通过理由
	///</summary>
	[BindField("APPROVECONTENT", ChName = "", DefaultValue = "''", DbType = "VarChar", Key = false)]
	public string APPROVECONTENT
	{
		get
		{
			return this._APPROVECONTENT;
		}
		set
		{
			this._APPROVECONTENT = value;
		}
	}


	private int _STATUS;
	///<summary>
	///状态
	///</summary>
	[BindField("STATUS", ChName = "", DefaultValue = "0", DbType = "Int16", Key = false)]
	public int STATUS
	{
		get
		{
			return this._STATUS;
		}
		set
		{
			this._STATUS = value;
		}
	}


	private string _YANQIFLAG="";
	///<summary>
	///延期标识
	///</summary>
	[BindField("YANQIFLAG", ChName = "", DefaultValue = "''", DbType = "VarChar", Key = false)]
	public string YANQIFLAG
	{
		get
		{
			return this._YANQIFLAG;
		}
		set
		{
			this._YANQIFLAG = value;
		}
	}


	private string _APPLYTABEID="";
	///<summary>
	///申请批次ID
	///</summary>
	[BindField("APPLYTABEID", ChName = "", DefaultValue = "''", DbType = "VarChar", Key = false)]
	public string APPLYTABEID
	{
		get
		{
			return this._APPLYTABEID;
		}
		set
		{
			this._APPLYTABEID = value;
		}
	}



}
}
