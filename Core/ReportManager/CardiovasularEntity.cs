using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.ReportManager
{
    /// <summary>
    ///心脑血管病发病报告卡的实体类
    ///add by ywk 2013年8月15日 00:02:42
    /// </summary>
    public class CardiovasularEntity
    {
        /// <summary>
        /// 报告卡卡号
        /// </summary>
        private string m_Reportno = String.Empty;
        /// <summary>
        /// 报告卡卡号
        /// </summary>
        public string ReportNo
        {
            get { return m_Reportno; }
            set { m_Reportno = value; }
        }
        /// <summary>
        /// 报告卡表主鍵
        /// </summary>
        private string m_Reportid = String.Empty;
        /// <summary>
        /// 报告卡表主鍵
        /// </summary>
        public string ReportID
        {
            get { return m_Reportid; }
            set { m_Reportid = value; }
        }
        /// <summary>
        /// 门诊号
        /// </summary>
        private string m_NOOFCLINIC = String.Empty;
        /// <summary>
        /// 门诊号
        /// </summary>
        public string NOOFCLINIC
        {
            get { return m_NOOFCLINIC; }
            set { m_NOOFCLINIC = value; }
        }
        /// <summary>
        /// 住院号
        /// </summary>
        private string m_PATID = String.Empty;
        /// <summary>
        /// 住院号
        /// </summary>
        public string PATID
        {
            get { return m_PATID; }
            set { m_PATID = value; }
        }

        /// <summary>
        /// 姓名
        /// </summary>
        private string m_NAME = String.Empty;
        /// <summary>
        /// 姓名
        /// </summary>
        public string NAME
        {
            get { return m_NAME; }
            set { m_NAME = value; }
        }
        /// <summary>
        /// 身份证号码
        /// </summary>
        private string m_IDNO = String.Empty;
        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IDNO
        {
            get { return m_IDNO; }
            set { m_IDNO = value; }
        }
        /// <summary>
        /// 性别编码
        /// </summary>
        private string m_SEXID = String.Empty;
        /// <summary>
        /// 性别编码
        /// </summary>
        public string SEXID
        {
            get { return m_SEXID; }
            set { m_SEXID = value; }
        }
        /// <summary>
        /// 性别名称
        /// </summary>
        private string m_SEXName = String.Empty;
        /// <summary>
        /// 性别名称
        /// </summary>
        public string SEXNAME
        {
            get { return m_SEXName; }
            set { m_SEXName = value; }
        }

        /// <summary>
        /// 出生日期
        /// </summary>
        private string m_BIRTH = String.Empty;
        /// <summary>
        /// 出生日期
        /// </summary>
        public string BIRTH
        {
            get { return m_BIRTH; }
            set { m_BIRTH = value; }
        }
        /// <summary>
        /// 年龄
        /// </summary>
        private string m_AGE = String.Empty;
        /// <summary>
        /// 年龄
        /// </summary>
        public string AGE
        {
            get { return m_AGE; }
            set { m_AGE = value; }
        }

        /// <summary>
        /// 民族编码
        /// </summary>
        private string m_nationId = String.Empty;

        /// <summary>
        /// 民族名称
        /// </summary>
        private string m_nationName = String.Empty;
        /// <summary>
        /// 民族编码
        /// </summary>
        public string NationId
        {
            get
            {
                return m_nationId;
            }
            set
            {
                m_nationId = value;
            }
        }

        /// <summary>
        /// 民族名称
        /// </summary>
        public string NationName
        {
            get
            {
                return m_nationName;
            }
            set
            {
                m_nationName = value;
            }
        }

        /// <summary>
        /// 职业编码
        /// </summary>
        private string m_JOBID = String.Empty;
        /// <summary>
        /// 职业编码
        /// </summary>
        public string JOBID
        {
            get { return m_JOBID; }
            set { m_JOBID = value; }
        }
        /// <summary>
        /// 职业名称
        /// </summary>
        private string m_JOBName = String.Empty;
        /// <summary>
        /// 职业名称
        /// </summary>
        public string JOBNAME
        {
            get { return m_JOBName; }
            set { m_JOBName = value; }
        }
        /// <summary>
        /// 工作单位
        /// </summary>
        private string m_OFFICEPLACE = String.Empty;
        /// <summary>
        /// 工作单位
        /// </summary>
        public string OFFICEPLACE
        {
            get { return m_OFFICEPLACE; }
            set { m_OFFICEPLACE = value; }
        }
        /// <summary>
        /// 联系电话
        /// </summary>
        private string m_CONTACTTEL = String.Empty;
        /// <summary>
        /// 联系电话
        /// </summary>
        public string CONTACTTEL
        {
            get { return m_CONTACTTEL; }
            set { m_CONTACTTEL = value; }
        }
        /// <summary>
        /// 户籍地址省
        /// </summary>
        private string m_HKPROVICE = String.Empty;
        /// <summary>
        /// 户籍地址省
        /// </summary>
        public string HKPROVICE
        {
            get { return m_HKPROVICE; }
            set { m_HKPROVICE = value; }
        }
        /// <summary>
        /// 户籍地址市（县）
        /// </summary>
        private string m_HKCITY = String.Empty;
        /// <summary>
        /// 户籍地址市（县）
        /// </summary>
        public string HKCITY
        {
            get { return m_HKCITY; }
            set { m_HKCITY = value; }
        }

        /// <summary>
        /// 户籍地址街道
        /// </summary>
        private string m_HKSTREET = String.Empty;
        /// <summary>
        /// 户籍地址街道
        /// </summary>
        public string HKSTREET
        {
            get { return m_HKSTREET; }
            set { m_HKSTREET = value; }
        }

        /// <summary>
        /// 户籍地址编码
        /// </summary>
        private string m_HKADDRESSID = String.Empty;
        /// <summary>
        ///户籍地址编码
        /// </summary>
        public string HKADDRESSID
        {
            get { return m_HKADDRESSID; }
            set { m_HKADDRESSID = value; }
        }

        /// <summary>
        /// 现住址省
        /// </summary>
        private string m_XZZPROVICE = String.Empty;
        /// <summary>
        ///现住址省
        /// </summary>
        public string XZZPROVICE
        {
            get { return m_XZZPROVICE; }
            set { m_XZZPROVICE = value; }
        }

        /// <summary>
        /// 现住址市（县）
        /// </summary>
        private string m_XZZCITY = String.Empty;
        /// <summary>
        ///现住址市（县）
        /// </summary>
        public string XZZCITY
        {
            get { return m_XZZCITY; }
            set { m_XZZCITY = value; }
        }
        /// <summary>
        /// 现住址街道
        /// </summary>
        private string m_XZZSTREET = String.Empty;
        /// <summary>
        ///现住址街道
        /// </summary>
        public string XZZSTREET
        {
            get { return m_XZZSTREET; }
            set { m_XZZSTREET = value; }
        }
        /// <summary>
        /// 现住址编码
        /// </summary>
        private string m_XZZADDRESSID = String.Empty;
        /// <summary>
        ///现住址编码
        /// </summary>
        public string XZZADDRESSID
        {
            get { return m_XZZADDRESSID; }
            set { m_XZZADDRESSID = value; }
        }
        /// <summary>
        /// 现住址居委会
        /// </summary>
        private string m_XZZCOMMITTEES = String.Empty;
        /// <summary>
        ///现住址居委会
        /// </summary>
        public string XZZCOMMITTEES
        {
            get { return m_XZZCOMMITTEES; }
            set { m_XZZCOMMITTEES = value; }
        }
        /// <summary>
        /// 现住址居委会后面栏位
        /// </summary>
        private string m_XZZPARM = String.Empty;
        /// <summary>
        ///现住址居委会后面栏位
        /// </summary>
        public string XZZPARM
        {
            get { return m_XZZPARM; }
            set { m_XZZPARM = value; }
        }
        /// <summary>
        ///ICD编码
        /// </summary>
        private string m_ICD = String.Empty;
        /// <summary>
        ///ICD编码
        /// </summary>
        public string ICD
        {
            get { return m_ICD; }
            set { m_ICD = value; }
        }
        /// <summary>
        ///脑卒中蛛网膜下腔出血
        /// </summary>
        private string m_DIAGZWMXQCX = String.Empty;
        /// <summary>
        ///脑卒中蛛网膜下腔出血
        /// </summary>
        public string DIAGZWMXQCX
        {
            get { return m_DIAGZWMXQCX; }
            set { m_DIAGZWMXQCX = value; }
        }
        /// <summary>
        ///脑卒中脑出血
        /// </summary>
        private string m_DIAGNCX = String.Empty;
        /// <summary>
        ///脑卒中脑出血
        /// </summary>
        public string DIAGNCX
        {
            get { return m_DIAGNCX; }
            set { m_DIAGNCX = value; }
        }
        /// <summary>
        ///脑卒中脑梗死
        /// </summary>
        private string m_DIAGNGS = String.Empty;
        /// <summary>
        ///脑卒中脑梗死
        /// </summary>
        public string DIAGNGS
        {
            get { return m_DIAGNGS; }
            set { m_DIAGNGS = value; }
        }
        /// <summary>
        ///脑卒中未分类脑卒中
        /// </summary>
        private string m_DIAGWFLNZZ = String.Empty;
        /// <summary>
        ///脑卒中未分类脑卒中
        /// </summary>
        public string DIAGWFLNZZ
        {
            get { return m_DIAGWFLNZZ; }
            set { m_DIAGWFLNZZ = value; }
        }
        /// <summary>
        ///冠心病急性心肌梗死
        /// </summary>
        private string m_DIAGJXXJGS = String.Empty;
        /// <summary>
        ///冠心病急性心肌梗死
        /// </summary>
        public string DIAGJXXJGS
        {
            get { return m_DIAGJXXJGS; }
            set { m_DIAGJXXJGS = value; }
        }

        /// <summary>
        ///冠心病心性猝死
        /// </summary>
        private string m_DIAGXXCS = String.Empty;
        /// <summary>
        ///冠心病心性猝死
        /// </summary>
        public string DIAGXXCS
        {
            get { return m_DIAGXXCS; }
            set { m_DIAGXXCS = value; }
        }

        /// <summary>
        ///诊断依据可多选临床症状□  心电图□  血管造影□  CT□  磁共振□  体格检查□超声检查□  实验室检查□ 死亡补发病□
        /// </summary>
        private string m_DIAGNOSISBASED = String.Empty;
        /// <summary>
        ///诊断依据
        /// </summary>
        public string DIAGNOSISBASED
        {
            get { return m_DIAGNOSISBASED; }
            set { m_DIAGNOSISBASED = value; }
        }
        /// <summary>
        ///确诊日期
        /// </summary>
        private string m_DIAGNOSEDATE = String.Empty;
        /// <summary>
        ///确诊日期
        /// </summary>
        public string DIAGNOSEDATE
        {
            get { return m_DIAGNOSEDATE; }
            set { m_DIAGNOSEDATE = value; }
        }
        /// <summary>
        ///是否首次发病
        /// </summary>
        private string m_ISFIRSTSICK = String.Empty;
        /// <summary>
        ///是否首次发病
        /// </summary>
        public string ISFIRSTSICK
        {
            get { return m_ISFIRSTSICK; }
            set { m_ISFIRSTSICK = value; }
        }
        /// <summary>
        ///确诊单位1）省级医院 2）市级医院  3）县级医院 4）乡镇级医院 5）其他 9）不详
        /// </summary>
        private string m_DIAGHOSPITAL = String.Empty;
        /// <summary>
        ///确诊单位1）省级医院 2）市级医院  3）县级医院 4）乡镇级医院 5）其他 9）不详
        /// </summary>
        public string DIAGHOSPITAL
        {
            get { return m_DIAGHOSPITAL; }
            set { m_DIAGHOSPITAL = value; }
        }
        /// <summary>
        ///转归 1）治愈  2）好转  3）未愈   4）死亡  5）其他
        /// </summary>
        private string m_OUTFLAG = String.Empty;
        /// <summary>
        ///转归 1）治愈  2）好转  3）未愈   4）死亡  5）其他
        /// </summary>
        public string OUTFLAG
        {
            get { return m_OUTFLAG; }
            set { m_OUTFLAG = value; }
        }
        /// <summary>
        ///死亡时间仅当转归为4时填写）
        /// </summary>
        private string m_DIEDATE = String.Empty;
        /// <summary>
        ///死亡时间仅当转归为4时填写）
        /// </summary>
        public string DIEDATE
        {
            get { return m_DIEDATE; }
            set { m_DIEDATE = value; }
        }
        /// <summary>
        ///报告单位
        /// </summary>
        private string m_REPORTDEPT = String.Empty;
        /// <summary>
        ///报告单位
        /// </summary>
        public string REPORTDEPT
        {
            get { return m_REPORTDEPT; }
            set { m_REPORTDEPT = value; }
        }
        /// <summary>
        ///报卡医生编码
        /// </summary>
        private string m_REPORTUSERCODE = String.Empty;
        /// <summary>
        ///报卡医生编码
        /// </summary>
        public string REPORTUSERCODE
        {
            get { return m_REPORTUSERCODE; }
            set { m_REPORTUSERCODE = value; }
        }
        /// <summary>
        ///报卡医生名称
        /// </summary>
        private string m_REPORTUSERNAME = String.Empty;
        /// <summary>
        ///报卡医生名称
        /// </summary>
        public string REPORTUSERNAME
        {
            get { return m_REPORTUSERNAME; }
            set { m_REPORTUSERNAME = value; }
        }
        /// <summary>
        ///报卡日期
        /// </summary>
        private string m_REPORTDATE = String.Empty;
        /// <summary>
        ///报卡日期
        /// </summary>
        public string REPORTDATE
        {
            get { return m_REPORTDATE; }
            set { m_REPORTDATE = value; }
        }
        /// <summary>
        ///创建时间
        /// </summary>
        private string m_CREATE_DATE = String.Empty;
        /// <summary>
        ///创建时间
        /// </summary>
        public string CREATE_DATE
        {
            get { return m_CREATE_DATE; }
            set { m_CREATE_DATE = value; }
        }
        /// <summary>
        ///创建人编码
        /// </summary>
        private string m_CREATE_USERCODE = String.Empty;
        /// <summary>
        ///创建人编码
        /// </summary>
        public string CREATE_USERCODE
        {
            get { return m_CREATE_USERCODE; }
            set { m_CREATE_USERCODE = value; }
        }
        /// <summary>
        ///创建人名称
        /// </summary>
        private string m_CREATE_USERNAME = String.Empty;
        /// <summary>
        ///创建人名
        /// </summary>
        public string CREATE_USERNAME
        {
            get { return m_CREATE_USERNAME; }
            set { m_CREATE_USERNAME = value; }
        }
        /// <summary>
        ///创建人科室编码
        /// </summary>
        private string m_CREATE_DEPTCODE = String.Empty;
        /// <summary>
        ///创建人科室编码
        /// </summary>
        public string CREATE_DEPTCODE
        {
            get { return m_CREATE_DEPTCODE; }
            set { m_CREATE_DEPTCODE = value; }
        }
        /// <summary>
        ///创建人科室名称
        /// </summary>
        private string m_CREATE_DEPTNAME = String.Empty;
        /// <summary>
        ///创建人科室名称
        /// </summary>
        public string CREATE_DEPTNAME
        {
            get { return m_CREATE_DEPTNAME; }
            set { m_CREATE_DEPTNAME = value; }
        }
        /// <summary>
        ///修改时间
        /// </summary>
        private string m_MODIFY_DATE = String.Empty;
        /// <summary>
        ///修改时间
        /// </summary>
        public string MODIFY_DATE
        {
            get { return m_MODIFY_DATE; }
            set { m_MODIFY_DATE = value; }
        }
        /// <summary>
        ///修改人编码
        /// </summary>
        private string m_MODIFY_USERCODE = String.Empty;
        /// <summary>
        ///修改人编码
        /// </summary>
        public string MODIFY_USERCODE
        {
            get { return m_MODIFY_USERCODE; }
            set { m_MODIFY_USERCODE = value; }
        }
        /// <summary>
        ///修改人名称
        /// </summary>
        private string m_MODIFY_USERNAME = String.Empty;
        /// <summary>
        ///修改人名称
        /// </summary>
        public string MODIFY_USERNAME
        {
            get { return m_MODIFY_USERNAME; }
            set { m_MODIFY_USERNAME = value; }
        }
        /// <summary>
        ///修改人科室编码
        /// </summary>
        private string m_MODIFY_DEPTCODE = String.Empty;
        /// <summary>
        ////修改人科室编码
        /// </summary>
        public string MODIFY_DEPTCODE
        {
            get { return m_MODIFY_DEPTCODE; }
            set { m_MODIFY_DEPTCODE = value; }
        }
        /// <summary>
        ///修改人科室名称
        /// </summary>
        private string m_MODIFY_DEPTNAME = String.Empty;
        /// <summary>
        ///修改人科室名称
        /// </summary>
        public string MODIFY_DEPTNAME
        {
            get { return m_MODIFY_DEPTNAME; }
            set { m_MODIFY_DEPTNAME = value; }
        }
        /// <summary>
        ///审核时间
        /// </summary>
        private string m_AUDIT_DATE = String.Empty;
        /// <summary>
        ///审核时间
        /// </summary>
        public string AUDIT_DATE
        {
            get { return m_AUDIT_DATE; }
            set { m_AUDIT_DATE = value; }
        }
        /// <summary>
        ///审核人编码
        /// </summary>
        private string m_AUDIT_USERCODE = String.Empty;
        /// <summary>
        ///审核人编码
        /// </summary>
        public string AUDIT_USERCODE
        {
            get { return m_AUDIT_USERCODE; }
            set { m_AUDIT_USERCODE = value; }
        }
        /// <summary>
        ///审核人姓名
        /// </summary>
        private string m_AUDIT_USERNAME = String.Empty;
        /// <summary>
        ///审核人姓名
        /// </summary>
        public string AUDIT_USERNAME
        {
            get { return m_AUDIT_USERNAME; }
            set { m_AUDIT_USERNAME = value; }
        }
        /// <summary>
        ///审核人科室
        /// </summary>
        private string m_AUDIT_DEPTCODE = String.Empty;
        /// <summary>
        ///审核人科室编码
        /// </summary>
        public string AUDIT_DEPTCODE
        {
            get { return m_AUDIT_DEPTCODE; }
            set { m_AUDIT_DEPTCODE = value; }
        }
        /// <summary>
        ///审核人科室名称
        /// </summary>
        private string m_AUDIT_DEPTNAME = String.Empty;
        /// <summary>
        ///审核人科室名称
        /// </summary>
        public string AUDIT_DEPTNAME
        {
            get { return m_AUDIT_DEPTNAME; }
            set { m_AUDIT_DEPTNAME = value; }
        }
        /// <summary>
        ///状态是否有效  1、有效   0、无效
        /// </summary>
        private string m_VAILD = String.Empty;
        /// <summary>
        ///状态是否有效  1、有效   0、无效
        /// </summary>
        public string VAILD
        {
            get { return m_VAILD; }
            set { m_VAILD = value; }
        }
        /// <summary>
        ///否决原因
        /// </summary>
        private string m_CANCELREASON = String.Empty;
        /// <summary>
        ///否决原因
        /// </summary>
        public string CANCELREASON
        {
            get { return m_CANCELREASON; }
            set { m_CANCELREASON = value; }

        }
        /// <summary>
        ///"""报告状态【 1、新增保存 2、提交 3、撤回 4、审核通过 5、审核未通过撤回 6、上报	7、作废】
        /// </summary>
        private string m_STATE = String.Empty;
        /// <summary>
        ///""""报告状态【 1、新增保存 2、提交 3、撤回 4、审核通过 5、审核未通过撤回 6、上报	7、作废】
        /// </summary>
        public string STATE
        {
            get { return m_STATE; }
            set { m_STATE = value; }
        }

        /// <summary>
        ///预留字段
        /// </summary>
        private string m_CARDPARAM1 = String.Empty;
        /// <summary>
        ///预留字段
        /// </summary>
        public string CARDPARAM1
        {
            get { return m_CARDPARAM1; }
            set { m_CARDPARAM1 = value; }
        }
        /// <summary>
        ///预留字段
        /// </summary>
        private string m_CARDPARAM2 = String.Empty;
        /// <summary>
        ///预留字段
        /// </summary>
        public string CARDPARAM2
        {
            get { return m_CARDPARAM2; }
            set { m_CARDPARAM2 = value; }
        }
        /// <summary>
        ///预留字段
        /// </summary>
        private string m_CARDPARAM3 = String.Empty;
        /// <summary>
        ///预留字段
        /// </summary>
        public string CARDPARAM3
        {
            get { return m_CARDPARAM3; }
            set { m_CARDPARAM3 = value; }
        }
        /// <summary>
        ///预留字段
        /// </summary>
        private string m_CARDPARAM4 = String.Empty;
        /// <summary>
        ///预留字段
        /// </summary>
        public string CARDPARAM4
        {
            get { return m_CARDPARAM4; }
            set { m_CARDPARAM4 = value; }
        }
        /// <summary>
        ///预留字段
        /// </summary>
        private string m_CARDPARAM5 = String.Empty;
        /// <summary>
        ///预留字段
        /// </summary>
        public string CARDPARAM5
        {
            get { return m_CARDPARAM5; }
            set { m_CARDPARAM5 = value; }
        }


        /// <summary>
        ///病人首页序号
        /// </summary>
        private string m_NOOFINPAT = String.Empty;
        /// <summary>
        ///病人首页序号
        /// </summary>
        public string NOOFINPAT
        {
            get { return m_NOOFINPAT; }
            set { m_NOOFINPAT = value; }
        }

        private string y1 = String.Empty;
        /// <summary>
        ///出生日期年
        /// </summary>
        public string Y1
        {
            get
            {
                if (BIRTH != "")
                {
                    DateTime dt = DateTime.Parse(BIRTH);
                    y1 =dt.Year.ToString();
                }
                return y1;
            }

        }

        private string y2 = String.Empty;
        /// <summary>
        ///出生日期月
        /// </summary>
        public string Y2
        {
            get
            {
                if (BIRTH != "")
                {
                    DateTime dt = DateTime.Parse(BIRTH);
                    y2 = dt.Month.ToString();
                }
                return y2;
            }

        }

        private string y3 = String.Empty;
        /// <summary>
        ///出生日期日
        /// </summary>
        public string Y3
        {
            get
            {
                if (BIRTH != "")
                {
                    DateTime dt = DateTime.Parse(BIRTH);
                    y3 = dt.Day.ToString();
                }
                return y3;
            }

        }

        private string y4 = String.Empty;
        /// <summary>
        ///出生日期日
        /// </summary>
        public string Y4
        {
            get
            {
                if (REPORTDATE != "")
                {
                    DateTime dt = DateTime.Parse(REPORTDATE);
                    y4 = dt.Year.ToString();
                }
                return y4;
            }

        }
        private string y5 = String.Empty;
        /// <summary>
        ///出生日期日
        /// </summary>
        public string Y5
        {
            get
            {
                if (REPORTDATE != "")
                {
                    DateTime dt = DateTime.Parse(REPORTDATE);
                    y5 = dt.Month.ToString();
                }
                return y5;
            }

        }

        private string y6 = String.Empty;
        /// <summary>
        ///出生日期日
        /// </summary>
        public string Y6
        {
            get
            {
                if (REPORTDATE != "")
                {
                    DateTime dt = DateTime.Parse(REPORTDATE);
                    y6 = dt.Day.ToString();
                }
                return y6;
            }

        }
        private string c1;

        /// <summary>
        /// 临床
        /// </summary>
        public string C1
        {
            get
            {
                c1 = string.Empty;
                if (DIAGZWMXQCX != "")
                {
                    c1 = "√";
                }
                else
                {
                    c1 = " ";
                }
                return c1;
            }
        }
        private string c2;

        /// <summary>
        /// 超声波
        /// </summary>
        public string C2
        {
            get
            {
                c2 = string.Empty;
                if (DIAGNCX != "")
                {
                    c2 = "√";
                }
                else
                {
                    c2 = " ";
                }
                return c2;
            }
        }
        private string c3;

        /// <summary>
        /// 尸检
        /// </summary>
        public string C3
        {
            get
            {
                c3 = string.Empty;
                if (DIAGNGS != "")
                {
                    c3 = "√";
                }
                else
                {
                    c3 = " ";
                }
                return c3;
            }
        }
        private string c4;

        /// <summary>
        /// 生化检验
        /// </summary>
        public string C4
        {
            get
            {
                c4 = string.Empty;
                if (DIAGWFLNZZ != "")
                {
                    c4 = "√";
                }
                else
                {
                    c4 = " ";
                }
                return c4;
            }
        }
        private string c5;

        /// <summary>
        /// 染色体
        /// </summary>
        public string C5
        {
            get
            {
                c5 = string.Empty;
                if (DIAGJXXJGS != "")
                {
                    c5 = "√";
                }
                else
                {
                    c5 = "  ";
                }
                return c5;
            }
        }
        private string c6;

        /// <summary>
        /// 其他
        /// </summary>
        public string C6
        {
            get
            {
                c6 = string.Empty;
                if (DIAGXXCS != "")
                {
                    c6 = "√";
                }
                else
                {
                    c6 = " ";
                }
                return c6;
            }
        }

        private string d1;

        /// <summary>
        /// X线、CT、超声、内窥镜
        /// </summary>
        public string D1
        {
            get
            {
                string[] array = DIAGNOSISBASED.Split(',');
                d1 = string.Empty;
                foreach (var arr in array)
                {
                    if (string.IsNullOrEmpty(arr))
                    {
                        continue;
                    }
                    switch (arr)
                    {
                        case "1":
                            d1 = "√";
                            break;
                    }
                }
                return d1;
            }
        }
        private string d2;

        /// <summary>
        /// X线、CT、超声、内窥镜
        /// </summary>
        public string D2
        {
            get
            {
                string[] array = DIAGNOSISBASED.Split(',');
                d2 = string.Empty;
                foreach (var arr in array)
                {
                    if (string.IsNullOrEmpty(arr))
                    {
                        continue;
                    }
                    switch (arr)
                    {
                        case "2":
                            d2 = "√";
                            break;
                    }
                }
                return d2;
            }
        }
        private string d3;

        /// <summary>
        /// X线、CT、超声、内窥镜
        /// </summary>
        public string D3
        {
            get
            {
                string[] array = DIAGNOSISBASED.Split(',');
                d3 = string.Empty;
                foreach (var arr in array)
                {
                    if (string.IsNullOrEmpty(arr))
                    {
                        continue;
                    }
                    switch (arr)
                    {
                        case "3":
                            d3 = "√";
                            break;
                    }
                }
                return d3;
            }
        }
        private string d4;

        /// <summary>
        /// X线、CT、超声、内窥镜
        /// </summary>
        public string D4
        {
            get
            {
                string[] array = DIAGNOSISBASED.Split(',');
                d4 = string.Empty;
                foreach (var arr in array)
                {
                    if (string.IsNullOrEmpty(arr))
                    {
                        continue;
                    }
                    switch (arr)
                    {
                        case "4":
                            d4 = "√";
                            break;
                    }
                }
                return d4;
            }
        }
        private string d5;

        /// <summary>
        /// X线、CT、超声、内窥镜
        /// </summary>
        public string D5
        {
            get
            {
                string[] array = DIAGNOSISBASED.Split(',');
                d5 = string.Empty;
                foreach (var arr in array)
                {
                    if (string.IsNullOrEmpty(arr))
                    {
                        continue;
                    }
                    switch (arr)
                    {
                        case "5":
                            d5 = "√";
                            break;
                    }
                }
                return d5;
            }
        }
        private string d6;

        /// <summary>
        /// X线、CT、超声、内窥镜
        /// </summary>
        public string D6
        {
            get
            {
                string[] array = DIAGNOSISBASED.Split(',');
                d6 = string.Empty;
                foreach (var arr in array)
                {
                    if (string.IsNullOrEmpty(arr))
                    {
                        continue;
                    }
                    switch (arr)
                    {
                        case "6":
                            d6 = "√";
                            break;
                    }
                }
                return d6;
            }
        }
        private string d7;

        /// <summary>
        /// X线、CT、超声、内窥镜
        /// </summary>
        public string D7
        {
            get
            {
                string[] array = DIAGNOSISBASED.Split(',');
                d7 = string.Empty;
                foreach (var arr in array)
                {
                    if (string.IsNullOrEmpty(arr))
                    {
                        continue;
                    }
                    switch (arr)
                    {
                        case "7":
                            d7 = "√";
                            break;
                    }
                }
                return d7;
            }
        }
        private string d8;

        /// <summary>
        /// X线、CT、超声、内窥镜
        /// </summary>
        public string D8
        {
            get
            {
                string[] array = DIAGNOSISBASED.Split(',');
                d8 = string.Empty;
                foreach (var arr in array)
                {
                    if (string.IsNullOrEmpty(arr))
                    {
                        continue;
                    }
                    switch (arr)
                    {
                        case "8":
                            d8 = "√";
                            break;
                    }
                }
                return d8;
            }
        }
        private string d9;

        /// <summary>
        /// X线、CT、超声、内窥镜
        /// </summary>
        public string D9
        {
            get
            {
                string[] array = DIAGNOSISBASED.Split(',');
                d9 = string.Empty;
                foreach (var arr in array)
                {
                    if (string.IsNullOrEmpty(arr))
                    {
                        continue;
                    }
                    switch (arr)
                    {
                        case "9":
                            d9 = "√";
                            break;
                    }
                }
                return d9;
            }
        }

        private string b1 = String.Empty;
        /// <summary>
        ///出生日期年
        /// </summary>
        public string B1
        {
            get
            {
                if (DIAGNOSEDATE != "")
                {
                    DateTime dt = DateTime.Parse(DIAGNOSEDATE);
                    b1 = dt.Year.ToString();
                }
                return b1;
            }

        }
        private string b2 = String.Empty;
        /// <summary>
        ///出生日期年
        /// </summary>
        public string B2
        {
            get
            {
                if (DIAGNOSEDATE != "")
                {
                    DateTime dt = DateTime.Parse(DIAGNOSEDATE);
                    b2 = dt.Month.ToString();
                }
                return b2;
            }

        }
        private string b3 = String.Empty;
        /// <summary>
        ///出生日期年
        /// </summary>
        public string B3
        {
            get
            {
                if (DIAGNOSEDATE != "")
                {
                    DateTime dt = DateTime.Parse(DIAGNOSEDATE);
                    b3 = dt.Day.ToString();
                }
                return b3;
            }

        }
        private string b4 = String.Empty;
        /// <summary>
        ///出生日期年
        /// </summary>
        public string B4
        {
            get
            {
                if (DIEDATE != "")
                {
                    DateTime dt = DateTime.Parse(DIEDATE);
                    b4 = dt.Year.ToString();
                }
                return b4;
            }

        }
        private string b5 = String.Empty;
        /// <summary>
        ///出生日期年
        /// </summary>
        public string B5
        {
            get
            {
                if (DIEDATE != "")
                {
                    DateTime dt = DateTime.Parse(DIEDATE);
                    b5 = dt.Month.ToString();
                }
                return b5;
            }

        }
        private string b6 = String.Empty;
        /// <summary>
        ///出生日期年
        /// </summary>
        public string B6
        {
            get
            {
                if (DIEDATE != "")
                {
                    DateTime dt = DateTime.Parse(DIEDATE);
                    b6 = dt.Day.ToString();
                }
                return b6;
            }

        }

        private string f1;

        /// <summary>
        /// 产前
        /// </summary>
        public string F1
        {
            get
            {
                f1 = string.Empty;
                if (ISFIRSTSICK == "0")
                {
                    f1 = "√";
                }
                else
                {
                    f1 = " ";
                }
                return f1;
            }
        }

        private string f2;

        /// <summary>
        /// 产前
        /// </summary>
        public string F2
        {
            get
            {
                f2 = string.Empty;
                if (ISFIRSTSICK == "1")
                {
                    f2 = "√";
                }
                else
                {
                    f2 = " ";
                }
                return f2;
            }
        }
    }
}
