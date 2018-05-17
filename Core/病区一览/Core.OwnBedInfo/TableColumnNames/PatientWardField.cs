using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.Core.OwnBedInfo
{
    /// <summary>
    /// 病区病人表列名
    /// </summary>
    public static class PatientWardField
    {
        #region public const string
        /// <summary>
        ///按竖排的记录标识(ID)
        /// </summary>
        public const string FieldIdVerOrder = "ID";
        /// <summary>
        /// 经过重排过的记录标识(IDChange)
        /// </summary>
        public const string FieldIdHorOrder = "IDChange";
        /// <summary>
        /// 婴儿标志
        /// </summary>
        public const string Fieldyebz = "yebz";
        /// <summary>
        /// 余额
        /// </summary>
        public const string Fieldfee = "ye";
        /// <summary>
        /// 费用表的主键字段名(syxh)
        /// </summary>
        public const string FieldfeePrimaryKey = "syxh";
        /// <summary>
        /// 费用表的费用字段名(ye)
        /// </summary>
        public const string FieldfeeTablefee = "ye";
        /// <summary>
        /// 床位代码
        /// </summary>
        public const string Fieldcwdm = "BedID";
        /// <summary>
        /// 姓名
        /// </summary>
        public const string Fieldhzxm = "PatName";
        /// <summary>
        /// 首页序号,xjt
        /// </summary>
        public const string Fieldsyxh = "NoOfInpat";
        /// <summary>
        /// HIS首页序号,xjt
        /// </summary>
        public const string Fieldhissyxh = "PatNoOfHis";
        /// <summary>
        /// 住院号
        /// </summary>
        public const string Fieldzyhm = "PatID";
        /// <summary>
        /// 病人性别
        /// </summary>
        public const string Fieldbrxb = "Sex";
        /// <summary>
        /// 年龄
        /// </summary>
        public const string Fieldxsnl = "AgeStr";
        /// <summary>
        /// 危重级别
        /// </summary>
        public const string Fieldwzjb = "wzjb";
        /// <summary>
        /// 入院日期
        /// </summary>
        public const string Fieldryrq = "AdmitDate";
        /// <summary>
        /// 凭证类型
        /// </summary>
        public const string Fieldpzlx = "pzlx";
        /// <summary>
        /// 病人状态
        /// </summary>
        public const string Fieldbrzt = "brzt";
        /// <summary>
        /// 入院诊断
        /// </summary>
        public const string Fieldryzd = "ryzd";
        /// <summary>
        /// 诊断名称
        /// </summary>
        public const string Fieldzdmc = "zdmc";
        /// <summary>
        /// 费别
        /// </summary>
        public const string Fieldybsm = "ybsm";
        /// <summary>
        /// 住院医生代码
        /// </summary>
        public const string Fieldzyysdm = "zyysdm";
        /// <summary>
        /// 床位医生
        /// </summary>
        public const string Fieldcwys = "cwys";
        /// <summary>
        /// 住院医生
        /// </summary>
        public const string Fieldzyys = "zyys";
        /// <summary>
        /// 主治医师
        /// </summary>
        public const string Fieldzzys = "zzys";
        /// <summary>
        /// 主任医师
        /// </summary>
        public const string Fieldzrys = "zrys";
        /// <summary>
        /// 备注
        /// </summary>
        public const string Fieldmemo = "memo";
        /// <summary>
        /// 护理级别
        /// </summary>
        public const string Fieldhljb = "hljb";
        /// <summary>
        /// 科室代码
        /// </summary>
        public const string Fieldksdm = "ksdm";
        /// <summary>
        /// 病区代码
        /// </summary>
        public const string Fieldbqdm = "bqdm";
        /// <summary>
        /// 占床标志
        /// </summary>
        public const string Fieldzcbz = "InBed";
        /// <summary>
        /// 有效记录
        /// </summary>
        public const string Fieldyxjl = "yxjl";
        /// <summary>
        /// 借床标志
        /// </summary>
        public const string Fieldjcbz = "jcbz";
        /// <summary>
        /// 床位类型
        /// </summary>
        public const string Fieldcwlx = "cwlx";
        /// <summary>
        /// 原病区代码
        /// </summary>
        public const string Fieldybqdm = "ybqdm";
        /// <summary>
        /// 原科室代码
        /// </summary>
        public const string Fieldyksdm = "yksdm";
        /// <summary>
        /// 原床位代码
        /// </summary>
        public const string Fieldycwdm = "ycwdm";
        /// <summary>
        /// 危重级别名称
        /// </summary>
        public const string Fieldwzjbmc = "wzjbmc";
        /// <summary>
        /// 额外信息
        /// </summary>
        //public const string Fieldextra = "extra";
        /// <summary>
        /// 婴儿母亲的首页序号
        /// </summary>
        public const string Fieldmqsyxh = "mqsyxh";
        /// <summary>
        /// 缓存选择的婴儿的姓名
        /// </summary>
        public const string Fieldtempbaby = "tempbaby";

        /// <summary>
        /// 临床路径执行情况
        /// </summary>
        public const string Fieldcpstatus = "CPStatus";

        /// <summary>
        /// 是否警告
        /// </summary>
        public const string Fieldiswarn = "IsWarn";
        #endregion
    }
}
