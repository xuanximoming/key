using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.QCReport
{

    /// <summary>
    /// 公共方法类
    /// </summary>
    public class CommonMethods
    {
        /// <summary>
        /// 将集合转成字典，在打印绘制时方便调用
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static Dictionary<string,DataColumn> DataColumnsToDictionary(List<DataColumn> columns)
        {
            try 
	        {	 
                Dictionary<string,DataColumn> dic=new Dictionary<string,DataColumn>();
		         foreach(DataColumn col in columns)
                 {
                     dic.Add(col.datafield,col);
                 }
                return dic;
	        }
	        catch (Exception ex)
	        {
		
		        throw ex;
	        }
        }
    }

    /// <summary>
    /// 参数结构体 tj 2013-1-19
    /// </summary>
    public struct ParamInfo
    {
        public string name;
        public string controltype;
        public string controlcaption;
        public string needbreak;

        public ParamInfo(string _name, string _controltype,string _controlcaption, string _needbreak)
        {
            name = _name;
            controltype = _controltype;
            controlcaption = _controlcaption;
            needbreak = _needbreak;
        }
    }

    /// <summary>
    /// 数据列
    /// </summary>
    public struct DataColumn
    {
        public string datafield;
        public string width;
        public string caption;

        public DataColumn(string _datafield, string _width, string _caption)
        {
            datafield = _datafield;
            width = _width;
            caption = _caption;
        }
    }

    /// <summary>
    /// 描述当前表单的查询信息 包括存储过程名称和参数信息  tj 2013-1-19
    /// </summary>
    public class SqlQuery
    {
        public string sqlStr { get; set; }
        public List<ParamInfo> paramList { get; set; }
    }


    /// <summary>
    /// 描述绑定字典数据结构
    /// </summary>
    public class IDNAME
    {
        public object Tag { get; set; }
        public string NAME { get; set; }

        public IDNAME() { }

        public IDNAME(string _name, object _tag)
        {
            this.Tag = _tag;
            this.NAME = _name;
        }

        public override string ToString()
        {
            return this.NAME;
        }
    }

    public class DIAIDNAME : IDNAME 
    {
        public string DiaPy { get; set; }
        public DIAIDNAME(string _name,object _tag ,string _diapy) 
        {
            this.Tag = _tag;
            this.NAME = _name;
            this.DiaPy = _diapy;
        }
    }


   
}
