using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Emr.QcManager
{
    public class SQLUtil
    {
        public const string sql_QueryWardInfo = @"select a.ID AS DEPTID,
                                                           a.NAME AS DEPTNAME,
                                                           nvl(a.TOTALBED,0) AS TOTALBED,
                                                           nvl(COUNT(b.NoOfInpat),0) TOTALPAT,
                                                           0 AS DANAGERPATS,
                                                           0 AS DIAGPATS,
                                                           0 AS NOPATREC,
                                                           0 AS SPOTPATREC,
                                                           0 AS col_AVERAGE
                                                      FROM DEPARTMENT a, INPATIENT b
                                                     where a.ID = b.outhosdept(+)
                                                       and b.status IN (1501, 1505, 1506, 1507)
                                                     group by a.ID, a.NAME, a.TOTALBED";

        public const string sql_QueryWardDetailInfo = @"select a.NoOfInpat AS NOOFINPAT,
                                                               a.PatID as PATID,
                                                               a.Name PATNAME,
                                                               (case when a.SexID = 1 then '男' when a.SexID = 2 then '女' else '未知' end) as PATSEX,
                                                               a.AgeStr PATAGE,
                                                               a.OutBed PATBED,
                                                               1 AS INHOSPITAL,
                                                               datediff('dd',
                                                                        a.admitdate,
                                                                        NVL(trim(a.outwarddate), TO_CHAR(SYSDATE, 'yyyy-mm-dd'))) INCOUNT,
                                                               0 AS OUTTIME,
                                                               0 ASOUTFILES,
                                                               count(b.ID) PATFILES,
                                                               a.AdmitDiagnosis PATDIAG,
                                                               diag.name PATDIAGNAME
                                                          from INPATIENT a, RecordDetail b,diagnosis diag
                                                         where a.NOOFINPAT = b.NoOfInpat(+)
                                                           and a.AdmitDiagnosis = diag.markid(+)
                                                           and a.status IN (1501, 1505, 1506, 1507)
                                                           and a.OutHosDept = '{0}'
                                                         group by a.NoOfInpat,
                                                                  a.PatID,
                                                                  a.NAME,
                                                                  a.SEXID,
                                                                  A.AGESTR,
                                                                  a.OUTBED,
                                                                  a.AdmitDiagnosis,
                                                                  a.admitdate,
                                                                  a.outwarddate,
                                                                  diag.name ";

        public const string sql_QueryDeptNameByID = @"select a.name from department a where a.id = '{0}'";

        public const string sql_QueryOutHospitalNOSubmit = @"select (case
                                                                     when count(b.ID) > 0 then
                                                                      '书写'
                                                                     else
                                                                      '未写'
                                                                   end) RECORDSTATE,
                                                                   '' QC,
                                                                   a.NoOfInpat AS NOOFINPAT,
                                                                   a.PatID as PATID,
                                                                   a.Name PATNAME,
                                                                   (case when a.SexID = 1 then '男' when a.SexID = 2 then '女' else '未知' end) as PATSEX,
                                                                   1 AS INHOSPITAL,
                                                                   dept.name DEPTNAME,
                                                                   diag.name PATDIAGNAME,
                                                                   a.admitdate INHOSPITALTIME,
                                                                   a.outhosdate OUTHOSPITALTIME,
                                                                   to_char(to_date(a.admitdate,'yyyy-mm-dd HH24:mi:ss')+1,'yyyy-mm-dd') SUREDIAGTIME, 
                                                                   users.name DOCNAME,
                                                                   a.AgeStr PATAGE,      
                                                                   datediff('dd',
                                                                            a.admitdate,
                                                                            NVL(trim(a.outwarddate), TO_CHAR(SYSDATE, 'yyyy-mm-dd'))) INCOUNT
       
                                                              from INPATIENT a, RecordDetail b, diagnosis diag,department dept,users users
                                                             where a.NOOFINPAT = b.NoOfInpat(+)
                                                               and a.AdmitDiagnosis = diag.markid(+)
                                                               and a.outhosdept = dept.id(+)
                                                               and a.Resident = users.id(+)
                                                               and a.status in (1502,1503)
                                                            /* and b.hassubmit = 4600   */
                                                               and (a.OutHosDept = '{0}' or '{0}' = '*')
                                                             group by a.NoOfInpat,
                                                                      a.PatID,
                                                                      a.NAME,
                                                                      a.SEXID,
                                                                      A.AGESTR,
                                                                      a.OUTBED,
                                                                      a.AdmitDiagnosis,
                                                                      a.admitdate,
                                                                      a.outwarddate,
                                                                      diag.name,
                                                                      dept.name,
                                                                      a.outhosdate,
                                                                      users.name";


        public const string sql_QueryOutHospitalNOLock = @"select (case
                                                                     when count(b.ID) > 0 then
                                                                      '书写'
                                                                     else
                                                                      '未写'
                                                                   end) RECORDSTATE,
                                                                   '' QC,
                                                                   a.NoOfInpat AS NOOFINPAT,
                                                                   a.PatID as PATID,
                                                                   a.Name PATNAME,
                                                                   (case when a.SexID = 1 then '男' when a.SexID = 2 then '女' else '未知' end) as PATSEX,
                                                                   1 AS INHOSPITAL,
                                                                   dept.name DEPTNAME,
                                                                   diag.name PATDIAGNAME,
                                                                   a.admitdate INHOSPITALTIME,
                                                                   a.outhosdate OUTHOSPITALTIME,
                                                                   to_char(to_date(a.admitdate,'yyyy-mm-dd HH24:mi:ss')+1,'yyyy-mm-dd') SUREDIAGTIME, 
                                                                   users.name DOCNAME,
                                                                   a.AgeStr PATAGE,      
                                                                   datediff('dd',
                                                                            a.admitdate,
                                                                            NVL(trim(a.outwarddate), TO_CHAR(SYSDATE, 'yyyy-mm-dd'))) INCOUNT
       
                                                              from INPATIENT a, RecordDetail b, diagnosis diag,department dept,users users
                                                             where a.NOOFINPAT = b.NoOfInpat(+)
                                                               and a.AdmitDiagnosis = diag.markid(+)
                                                               and a.outhosdept = dept.id(+)
                                                               and a.Resident = users.id(+)
                                                               and a.status in (1502,1503)
                                                            /* and b.islock in (4700, 4702, 4703)  */
                                                               and (a.OutHosDept = '{0}' or '{0}' = '*')
                                                             group by a.NoOfInpat,
                                                                      a.PatID,
                                                                      a.NAME,
                                                                      a.SEXID,
                                                                      A.AGESTR,
                                                                      a.OUTBED,
                                                                      a.AdmitDiagnosis,
                                                                      a.admitdate,
                                                                      a.outwarddate,
                                                                      diag.name,
                                                                      dept.name,
                                                                      a.outhosdate,
                                                                      users.name";

        /// <summary>
        /// 获取评分大项数据源
        /// </summary>
        public const string sql_GetQCScoreType = @"SELECT TypeCode,
                                                           TypeName,
                                                           TypeInstruction,
                                                           TypeCategory,
                                                           case TypeCategory
                                                             when 0 then
                                                              '个人'
                                                             when 1 then
                                                              '科室'
                                                             when 2 then
                                                              '医务处'
                                                           end TypeCategoryName,
                                                           TypeOrder,
                                                           TypeMemo
                                                      from QCScoreType
                                                     where Valide = 1
                                                     order by TypeOrder";

        /// <summary>
        /// 获取评分细项数据源（需要参数为TypeCode，全部查询传入‘’）
        /// </summary>
        public const string sql_GetQCScoreItem = @"select QCI.ItemCode,
                                                           QCI.ItemName,
                                                           QCI.ItemInstruction,
                                                           QCI.ItemDefaultScore,
                                                           QCI.ItemStandardScore,
                                                           QCI.ItemCategory,
                                                           case QCI.ItemCategory
                                                             when 0 then
                                                              '个人'
                                                             when 1 then
                                                              '科室'
                                                             when 2 then
                                                              '医务处'
                                                           end ItemCategoryName,
                                                           QCI.ItemDefaultTarget,
                                                           QCI.ItemTargetStandard,
                                                           QCI.ItemScoreStandard,
                                                           QCI.ItemOrder,
                                                           QCI.TypeCode,
                                                           QCI.ItemMemo,
                                                           QCT.TypeName
                                                      FROM QCScoreItem QCI
                                                      JOIN QCScoreType QCT ON QCI.TypeCode = QCT.TypeCode
                                                     where QCI.Valide = 1
                                                     and (qci.TypeCode = '{0}' or '{0}' is null)
                                                     order by QCI.TypeCode, QCI.ItemOrder  ";

        /// <summary>
        /// 获取手术统计模块数据    参数 0： 开始时间   1：结束时间    2：科室   3：手术代码
        /// </summary>
        public const string sql_getOperRpt = @"select inp.patid,
                                                       inp.name,
                                                       inp.sexid,
                                                       inp.agestr,
                                                       diag.name,
                                                       oper.operation_name,
                                                       oper.operation_date,
                                                       inp.admitdate,
                                                       inp.outwarddate,
                                                       inp.address,
                                                       datediff('dd',
                                                                inp.admitdate,
                                                                NVL(trim(inp.outwarddate), TO_CHAR(SYSDATE, 'yyyy-mm-dd'))) INCOUNT,
                                                       '0' fee,
                                                       dept.name deptname,
                                                       u.name oper_docname
                                                  from Iem_MainPage_Operation oper
                                                  left join iem_mainpage_basicinfo base on base.iem_mainpage_no =
                                                                                           oper.iem_mainpage_no
                                                  left join inpatient inp on base.noofinpat = inp.noofinpat
                                                  left join diagnosis diag on diag.markid = inp.admitdiagnosis
                                                  left join department dept on inp.outhosdept = dept.id
                                                  left join users u on u.id = oper.execute_user1
                                                 where oper.valide = 1
                                                   and ((to_date(oper.operation_date, 'yyyy-mm-dd hh24:mi:ss') >=
                                                       to_date('{0} 00:00:00', 'yyyy-mm-dd hh24:mi:ss') or
                                                       '{0}' is null) or
                                                       (to_date(oper.operation_date, 'yyyy-mm-dd hh24:mi:ss') >=
                                                       to_date('{1} 23:59:59', 'yyyy-mm-dd hh24:mi:ss') or
                                                       '{1}' is null))
                                                   and (inp.outhosdept = '{2}' or '{2}' is null)
                                                   and (oper.operation_code = '{3}' or '{3}' is null)
                                                 order by inp.noofinpat";
    }
}
