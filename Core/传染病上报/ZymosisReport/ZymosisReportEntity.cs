using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.ZymosisReport
{
    public class ZymosisReportEntity
    {
        #region declaration

        private int m_ReportId = 0;
        private string m_ReportNo = String.Empty;
        private string m_ReportType = String.Empty;
        private string m_Noofinpat = String.Empty;
        private string m_Patid = String.Empty;

        private string m_Name = String.Empty;
        private string m_Parentname = String.Empty;
        private string m_Idno = String.Empty;
        private string m_Sex = String.Empty;
        private string m_Birth = String.Empty;

        private string m_Age = String.Empty;
        private string m_AgeUnit = String.Empty;
        private string m_Organization = String.Empty;
        private string m_Officeplace = String.Empty;
        private string m_Officetel = String.Empty;
        private string m_Addresstype = String.Empty;

        private string m_Hometown = String.Empty;
        private string m_Address = String.Empty;
        private string m_Jobid = String.Empty;
        private string m_Recordtype1 = String.Empty;
        private string m_Recordtype2 = String.Empty;

        private string m_Attackdate = String.Empty;
        private string m_Diagdate = String.Empty;
        private string m_Diedate = String.Empty;
        private string m_Diagicd10 = String.Empty;
        private string m_Diagname = String.Empty;

        private string m_InfectotherFlag = String.Empty;
        private string m_Memo = String.Empty;
        private string m_CorrectFlag = String.Empty;
        private string m_CorrectName = String.Empty;
        private string m_CancelReason = String.Empty;

        private string m_Reportdeptcode = String.Empty;
        private string m_Reportdeptname = String.Empty;
        private string m_Reportdoccode = String.Empty;
        private string m_Reportdocname = String.Empty;
        private string m_Doctortel = String.Empty;

        private string m_ReportDate = String.Empty;
        private string m_State = String.Empty;
        private string m_CreateDate = String.Empty;
        private string m_CreateUsercode = String.Empty;
        private string m_CreateUsername = String.Empty;

        private string m_CreateDeptcode = String.Empty;
        private string m_CreateDeptname = String.Empty;
        private string m_ModifyDate = String.Empty;
        private string m_ModifyUsercode = String.Empty;
        private string m_ModifyUsername = String.Empty;

        private string m_ModifyDeptcode = String.Empty;
        private string m_ModifyDeptname = String.Empty;
        private string m_AuditDate = String.Empty;
        private string m_AuditUsercode = String.Empty;
        private string m_AuditUsername = String.Empty;

        private string m_AuditDeptcode = String.Empty;
        private string m_AuditDeptname = String.Empty;
        private string m_Vaild = String.Empty;
        private string m_OtherDiag = String.Empty;

        #endregion declaration

        public ZymosisReportEntity()
        {
        }

        #region Properties

        /// <summary>
        /// 自增长编号
        /// </summary>
        public int ReportId
        {
            get
            {
                return m_ReportId;
            }
            set
            {
                m_ReportId = value;
            }
        }

        /// <summary>
        /// 传染病报告卡编号
        /// </summary>
        public string ReportNo
        {
            get
            {
                return m_ReportNo;
            }
            set
            {
                m_ReportNo = value;
            }
        }

        /// <summary>
        /// 报告卡类型   1、初次报告  2、订正报告
        /// </summary>
        public string ReportType
        {
            get
            {
                return m_ReportType;
            }
            set
            {
                m_ReportType = value;
            }
        }

        /// <summary>
        /// 首页序号
        /// </summary>
        public string Noofinpat
        {
            get
            {
                return m_Noofinpat;
            }
            set
            {
                m_Noofinpat = value;
            }
        }

        /// <summary>
        /// 住院号
        /// </summary>
        public string Patid
        {
            get
            {
                return m_Patid;
            }
            set
            {
                m_Patid = value;
            }
        }

        /// <summary>
        /// 患者姓名
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }

        /// <summary>
        /// 家长姓名
        /// </summary>
        public string Parentname
        {
            get
            {
                return m_Parentname;
            }
            set
            {
                m_Parentname = value;
            }
        }

        /// <summary>
        /// 身份证号码
        /// </summary>
        public string Idno
        {
            get
            {
                return m_Idno;
            }
            set
            {
                m_Idno = value;
            }
        }

        /// <summary>
        /// 患者性别
        /// </summary>
        public string Sex
        {
            get
            {
                return m_Sex;
            }
            set
            {
                m_Sex = value;
            }
        }

        /// <summary>
        /// 出生日期
        /// </summary>
        public string Birth
        {
            get
            {
                return m_Birth;
            }
            set
            {
                m_Birth = value;
            }
        }

        /// <summary>
        /// 实足年龄
        /// </summary>
        public string Age
        {
            get
            {
                return m_Age;
            }
            set
            {
                m_Age = value;
            }
        }

        /// <summary>
        /// 实足年龄单位  1、年  2、月  3、天
        /// </summary>
        public string AgeUnit
        {
            get
            {
                return m_AgeUnit;
            }
            set
            {
                m_AgeUnit = value;
            }
        }

        /// <summary>
        /// 工作单位
        /// </summary>
        public string Organization
        {
            get
            {
                return m_Organization;
            }
            set
            {
                m_Organization = value;
            }
        }

        /// <summary>
        /// 单位地址
        /// </summary>
        public string Officeplace
        {
            get
            {
                return m_Officeplace;
            }
            set
            {
                m_Officeplace = value;
            }
        }

        /// <summary>
        /// 单位电话
        /// </summary>
        public string Officetel
        {
            get
            {
                return m_Officetel;
            }
            set
            {
                m_Officetel = value;
            }
        }

        /// <summary>
        /// 病人属于地区	1、本县区 2、本市区其他县区	3、本省其他地区	4、外省	5、港澳台	6、外籍
        /// </summary>
        public string Addresstype
        {
            get
            {
                return m_Addresstype;
            }
            set
            {
                m_Addresstype = value;
            }
        }

        /// <summary>
        /// 家乡
        /// </summary>
        public string Hometown
        {
            get
            {
                return m_Hometown;
            }
            set
            {
                m_Hometown = value;
            }
        }

        /// <summary>
        /// 详细地址[村 街道 门牌号]
        /// </summary>
        public string Address
        {
            get
            {
                return m_Address;
            }
            set
            {
                m_Address = value;
            }
        }

        /// <summary>
        /// 职业代码（按页面顺序记录编号）
        /// </summary>
        public string Jobid
        {
            get
            {
                return m_Jobid;
            }
            set
            {
                m_Jobid = value;
            }
        }

        /// <summary>
        /// 病历分类	1、疑似病历	2、临床诊断病历	3、实验室确诊病历	4病原携带者
        /// </summary>
        public string Recordtype1
        {
            get
            {
                return m_Recordtype1;
            }
            set
            {
                m_Recordtype1 = value;
            }
        }

        /// <summary>
        /// 病历分类（乙型肝炎、血吸虫病填写）	1、急性	2、慢性
        /// </summary>
        public string Recordtype2
        {
            get
            {
                return m_Recordtype2;
            }
            set
            {
                m_Recordtype2 = value;
            }
        }

        /// <summary>
        /// 发病日期（病原携带者填初检日期或就诊日期）
        /// </summary>
        public string Attackdate
        {
            get
            {
                return m_Attackdate;
            }
            set
            {
                m_Attackdate = value;
            }
        }

        /// <summary>
        /// 诊断日期
        /// </summary>
        public string Diagdate
        {
            get
            {
                return m_Diagdate;
            }
            set
            {
                m_Diagdate = value;
            }
        }

        /// <summary>
        /// 死亡日期
        /// </summary>
        public string Diedate
        {
            get
            {
                return m_Diedate;
            }
            set
            {
                m_Diedate = value;
            }
        }

        /// <summary>
        /// 传染病病种(对应传染病诊断库)
        /// </summary>
        public string Diagicd10
        {
            get
            {
                return m_Diagicd10;
            }
            set
            {
                m_Diagicd10 = value;
            }
        }

        /// <summary>
        /// 传染病病种名称
        /// </summary>
        public string Diagname
        {
            get
            {
                return m_Diagname;
            }
            set
            {
                m_Diagname = value;
            }
        }

        /// <summary>
        /// 有无感染其他人[0无 1有]
        /// </summary>
        public string InfectotherFlag
        {
            get
            {
                return m_InfectotherFlag;
            }
            set
            {
                m_InfectotherFlag = value;
            }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Memo
        {
            get
            {
                return m_Memo;
            }
            set
            {
                m_Memo = value;
            }
        }

        /// <summary>
        /// 订正标志【0、未订正 1、已订正】	
        /// </summary>
        public string CorrectFlag
        {
            get
            {
                return m_CorrectFlag;
            }
            set
            {
                m_CorrectFlag = value;
            }
        }

        /// <summary>
        /// 订正病名
        /// </summary>
        public string CorrectName
        {
            get
            {
                return m_CorrectName;
            }
            set
            {
                m_CorrectName = value;
            }
        }

        /// <summary>
        /// 退卡原因
        /// </summary>
        public string CancelReason
        {
            get
            {
                return m_CancelReason;
            }
            set
            {
                m_CancelReason = value;
            }
        }

        /// <summary>
        /// 报告科室编号
        /// </summary>
        public string Reportdeptcode
        {
            get
            {
                return m_Reportdeptcode;
            }
            set
            {
                m_Reportdeptcode = value;
            }
        }

        /// <summary>
        /// 报告科室名称
        /// </summary>
        public string Reportdeptname
        {
            get
            {
                return m_Reportdeptname;
            }
            set
            {
                m_Reportdeptname = value;
            }
        }

        /// <summary>
        /// 报告医生编号
        /// </summary>
        public string Reportdoccode
        {
            get
            {
                return m_Reportdoccode;
            }
            set
            {
                m_Reportdoccode = value;
            }
        }

        /// <summary>
        /// 报告医生名称
        /// </summary>
        public string Reportdocname
        {
            get
            {
                return m_Reportdocname;
            }
            set
            {
                m_Reportdocname = value;
            }
        }

        /// <summary>
        /// 报告医生联系电话
        /// </summary>
        public string Doctortel
        {
            get
            {
                return m_Doctortel;
            }
            set
            {
                m_Doctortel = value;
            }
        }

        /// <summary>
        /// 填卡时间
        /// </summary>
        public string ReportDate
        {
            get
            {
                return m_ReportDate;
            }
            set
            {
                m_ReportDate = value;
            }
        }

        /// <summary>
        /// 报告状态【 1、新增保存 2、提交 3、撤回 4、审核通过 5、审核未通过撤回 6、上报	7、作废】
        /// </summary>
        public string State
        {
            get
            {
                return m_State;
            }
            set
            {
                m_State = value;
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateDate
        {
            get
            {
                return m_CreateDate;
            }
            set
            {
                m_CreateDate = value;
            }
        }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUsercode
        {
            get
            {
                return m_CreateUsercode;
            }
            set
            {
                m_CreateUsercode = value;
            }
        }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUsername
        {
            get
            {
                return m_CreateUsername;
            }
            set
            {
                m_CreateUsername = value;
            }
        }

        /// <summary>
        /// 创建人科室
        /// </summary>
        public string CreateDeptcode
        {
            get
            {
                return m_CreateDeptcode;
            }
            set
            {
                m_CreateDeptcode = value;
            }
        }

        /// <summary>
        /// 创建人科室
        /// </summary>
        public string CreateDeptname
        {
            get
            {
                return m_CreateDeptname;
            }
            set
            {
                m_CreateDeptname = value;
            }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public string ModifyDate
        {
            get
            {
                return m_ModifyDate;
            }
            set
            {
                m_ModifyDate = value;
            }
        }

        /// <summary>
        /// 修改人
        /// </summary>
        public string ModifyUsercode
        {
            get
            {
                return m_ModifyUsercode;
            }
            set
            {
                m_ModifyUsercode = value;
            }
        }

        /// <summary>
        /// 修改人
        /// </summary>
        public string ModifyUsername
        {
            get
            {
                return m_ModifyUsername;
            }
            set
            {
                m_ModifyUsername = value;
            }
        }

        /// <summary>
        /// 修改人科室
        /// </summary>
        public string ModifyDeptcode
        {
            get
            {
                return m_ModifyDeptcode;
            }
            set
            {
                m_ModifyDeptcode = value;
            }
        }

        /// <summary>
        /// 修改人科室
        /// </summary>
        public string ModifyDeptname
        {
            get
            {
                return m_ModifyDeptname;
            }
            set
            {
                m_ModifyDeptname = value;
            }
        }

        /// <summary>
        /// 审核时间
        /// </summary>
        public string AuditDate
        {
            get
            {
                return m_AuditDate;
            }
            set
            {
                m_AuditDate = value;
            }
        }

        /// <summary>
        /// 审核人
        /// </summary>
        public string AuditUsercode
        {
            get
            {
                return m_AuditUsercode;
            }
            set
            {
                m_AuditUsercode = value;
            }
        }

        /// <summary>
        /// 审核人
        /// </summary>
        public string AuditUsername
        {
            get
            {
                return m_AuditUsername;
            }
            set
            {
                m_AuditUsername = value;
            }
        }

        /// <summary>
        /// 审核人科室
        /// </summary>
        public string AuditDeptcode
        {
            get
            {
                return m_AuditDeptcode;
            }
            set
            {
                m_AuditDeptcode = value;
            }
        }

        /// <summary>
        /// 审核人科室
        /// </summary>
        public string AuditDeptname
        {
            get
            {
                return m_AuditDeptname;
            }
            set
            {
                m_AuditDeptname = value;
            }
        }

        /// <summary>
        /// 状态是否有效  1、有效   0、无效
        /// </summary>
        public string Vaild
        {
            get
            {
                return m_Vaild;
            }
            set
            {
                m_Vaild = value;
            }
        }

        /// <summary>
        /// 其他法定管理以及重点监测传染病：
        /// </summary>
        public string OtherDiag
        {
            get
            {
                return m_OtherDiag;
            }
            set
            {
                m_OtherDiag = value;
            }
        }

        #endregion Properties
    }
}
