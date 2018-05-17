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
        //增加查询条件 edit by ywk   2012年10月9日 15:42:09 
        //增加查询条件 edit by jxh   2013-7-28
        public const string sql_QueryOutHospitalNOSubmit = @"select (case
                                                                     when count(b.ID) > 0 then
                                                                      '书写'
                                                                     else
                                                                      '未写'
                                                                   end) RECORDSTATE,
                                                                   '' QC,
                                                                   a.NoOfInpat AS NOOFINPAT,
                                                                   a.patid as PATID,
                                                                   a.Name PATNAME,

                                                                   bas.iem_mainpage_no as IEMNO,                                                                
                                                                    wmsys.wm_concat( distinct diagnosis.diagnosis_name ) DIAGNOSISNAME,
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
       
                                                              from INPATIENT a, RecordDetail b, diagnosis diag,department dept,users users,{0} bas,{1} diagnosis
                                                             where a.NOOFINPAT = b.NoOfInpat(+)
                                                               and a.AdmitDiagnosis = diag.markid(+)                                                                                                                
                                                               and a.outhosdept = dept.id(+)
                                                               and a.Resident = users.id(+)

                                                               and a.NOOFINPAT=bas.NOOFINPAT(+)
                                                             and bas.iem_mainpage_no=diagnosis.iem_mainpage_no(+)
                                                              and (diagnosis.diagnosis_type_id<>13 or diagnosis.diagnosis_type_id is null)

                                                               and a.status in (1502,1503)
                                                             and b.hassubmit = 4600   
                                                               and (a.OutHosDept = '{2}' or '{2}' = '*')
                                                               AND (a.sexid = '{3}' OR '{3}' = '' OR '{3}' IS NULL)
                                                         and to_date(substr(nvl(trim(a.outhosdate), '1990-01-01'), 1, 10),
                                                          'yyyy-mm-dd hh24:mi:ss') >= to_date('{4}', 'yyyy-mm-dd  hh24:mi:ss')
                                                         and to_date(substr(nvl(trim(a.outhosdate), '1990-01-01'), 1, 10),
                                                            'yyyy-mm-dd hh24:mi:ss') < to_date('{5}', 'yyyy-mm-dd  hh24:mi:ss')
                                                 and (a.Name = '{6}' or '{6}' = '' or  '{6}' is null)
                                                    and (a.patid = '{7}' or '{7}' = '' or  '{7}' is null)
                                                             group by a.NoOfInpat,
                                                                      a.patid,
                                                                      a.NAME,
                                                                      a.SEXID,
                                                                      A.AGESTR,
                                                                      a.OUTBED,
                                                                      a.AdmitDiagnosis,
                                                                      a.admitdate,
                                                                      a.outwarddate,
                                                                      diag.name,
                                                                      dept.name, 
                                                                      bas.iem_mainpage_no,                                                                                                               
                                                                      a.outhosdate,
                                                                      users.name";

        //增加查询条件 edit by jxh   2013-7-28
        public const string sql_QueryOutHospitalNOLock = @"select (case
           when ((a.islock is null) or a.islock='4700')  then '未完成'
           when a.islock='4701'then '已归档'
           when a.islock='4702' then '撤销归档'
             when a.islock='4704'then '已完成'
           when a.islock='4705' then '科室质控'
              when a.islock='4706' then '已提交'
 when a.islock='4707' then '补写提交'
           else
            '未知'
         end) RECORDSTATE,
                                                                   '' QC,
                                                                   a.NoOfInpat AS NOOFINPAT,
                                                                   a.PatID as PATID,
                                                                   a.Name PATNAME,
                                                                   (case when a.SexID = 1 then '男' when a.SexID = 2 then '女' else '未知' end) as PATSEX,
                                                                   1 AS INHOSPITAL,
                                                                   dept.name DEPTNAME,
                                                                   diag.name PATDIAGNAME,
                                                                   a.AdmitDiagnosis,
                                                                   bas.iem_mainpage_no as IEMNO, 
                                                                   wmsys.wm_concat(distinct diagnosis.diagnosis_name) as DIAGNOSISNAME ,

                                                                   a.admitdate INHOSPITALTIME,
                                                                   a.outhosdate OUTHOSPITALTIME,
                                                                   to_char(to_date(a.admitdate,'yyyy-mm-dd HH24:mi:ss')+1,'yyyy-mm-dd') SUREDIAGTIME, 
                                                                   users.name DOCNAME,
                                                                   a.AgeStr PATAGE,      
                                                                   datediff('dd',
                                                                            a.admitdate,
                                                                            NVL(trim(a.outwarddate), TO_CHAR(SYSDATE, 'yyyy-mm-dd'))) INCOUNT
       
                                                              from INPATIENT a, RecordDetail b, diagnosis diag,department dept,users users,{0} bas,{1} diagnosis
                                                             where a.NOOFINPAT = b.NoOfInpat(+)
                                                               and a.AdmitDiagnosis = diag.markid(+)
                                                               and a.outhosdept = dept.id(+)
                                                               and a.Resident = users.id(+)

                                                                and a.NOOFINPAT=bas.NOOFINPAT(+)
                                                               and bas.iem_mainpage_no=diagnosis.iem_mainpage_no(+)
                                                               and (diagnosis.diagnosis_type_id<>13 or diagnosis.diagnosis_type_id is null)

                                                               and a.status in (1502,1503)
                                                             and (nvl(a.islock,4700) in ({10})  or {10} =0)
                                                               and (a.OutHosDept = '{2}' or '{2}' = '*')
                                                               AND (a.sexid = '{3}' OR '{3}' = '' OR '{3}' IS NULL)
                                                         and to_date(substr(nvl(trim(a.outhosdate), '1990-01-01'), 1, 10),
                                                          'yyyy-mm-dd') >= to_date('{4}', 'yyyy-mm-dd')
                                                         and to_date(substr(nvl(trim(a.outhosdate), '1990-01-01'), 1, 10),
                                                            'yyyy-mm-dd') < to_date('{5}', 'yyyy-mm-dd')
   and to_date(substr(nvl(trim(a.admitdate), '1990-01-01'), 1, 10),
                                                          'yyyy-mm-dd') >= to_date('{8}', 'yyyy-mm-dd')
                                                         and to_date(substr(nvl(trim(a.admitdate), '1990-01-01'), 1, 10),
                                                            'yyyy-mm-dd') < to_date('{9}', 'yyyy-mm-dd')
                                                 and (a.Name = '{6}' or '{6}' = '' or  '{6}' is null)
                                                    and (a.patid = '{7}' or '{7}' = '' or  '{7}' is null)
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
                                                                      bas.iem_mainpage_no,
                                                                      users.name,
                                                                      a.islock";

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

        //        /// <summary>
        //        /// 获取手术统计模块数据    参数 0： 入院诊断   1：出院诊断    2：手术代码
        //        /// </summary>出院诊断取病案首页中的！add by ywk 2013年7月16日 19:34:29
        // and (diag.diagnosis_code = '{1}')
        public const string sql_QueryDiagOperRecord = @"select distinct inp.NOOFINPAT,
                                                           inp.name, 
                                                           inp.patid, 
                                                           inp.agestr,
                                                           inp.TOTALDAYS,
                                                           (case when inp.SexID = '1' then '男' when inp.SexID = '2' then '女' else '未知' end) as sex,
                                                           dept.name dept, 
                                                           {8} AdmitDiag,
                                                            temp1.OUTDIAG,
                                                           temp.OperName operation,
                                                           (case when lengthb(inp.admitdate)>10 then substr(inp.admitdate,0,10) end) as admitdate,
                                                           (case when lengthb(inp.outhosdate)>10 then substr(inp.outhosdate,0,10) end) as outhosdate,
                                                           user1.name RESIDENT,
                                                           user2.name CHIEF,
                                                           inp.INCOUNT   from (select distinct inp.patid,
                      wmsys.wm_concat(distinct ope.operation_name ) OperName 
          from inpatient inp
          left join {5} bas on inp.noofinpat =
                                                       bas.noofinpat and bas.valide=1
          left join {6} ope on ope.iem_mainpage_no =
                                                       bas.iem_mainpage_no
                                                   and ope.valide = 1 
         group by inp.patid) temp left join 
         (select distinct inp.patid,
                        wmsys.wm_concat(distinct diag.diagnosis_name ) OUTDIAG
          from inpatient inp
          left join {5} bas on inp.noofinpat = bas.noofinpat
                           and bas.valide = 1
          left join {7} diag on diag.iem_mainpage_no = bas.iem_mainpage_no
                           and diag.valide = 1
                           and diag.diagnosis_type_id<>13 
                     {1} 
         group by inp.patid) temp1 on temp1.patid=temp.patid 
  left join inpatient inp on inp.patid = temp.patid
  left join Department dept on inp.outhosdept = dept.id and dept.valid=1
  left join {5} basic on basic.noofinpat =
                                                 inp.noofinpat and basic.valide=1
  left join {7} imd on basic.iem_mainpage_no =
                                               imd.iem_mainpage_no and imd.valide=1 
  {0}
  inner join users user1 on user1.id = inp.RESIDENT and user1.valid=1
  left join users user2 on user2.id = inp.CHIEF and user2.valid=1
  inner join {6} oper on oper.iem_mainpage_no =
                                                basic.iem_mainpage_no and oper.valide=1 {2}
 where {10}  {9}  to_date( inp.outhosdate,'yyyy-mm-dd hh24:mi:ss') between to_date('{3}','yyyy-mm-dd hh24:mi:ss') and to_date('{4}','yyyy-mm-dd hh24:mi:ss')";
        /// <summary>
        /// 修改了去除重复数据   add by ywk 2013年7月16日 19:09:15
        /// </summary>
        public const string sql_QueryDiagOperRecord_ZY = @"select distinct inp.NOOFINPAT,
                                                           inp.name, 
                                                           inp.patid, 
                                                           inp.agestr,
                                                           inp.TOTALDAYS,
                                                           (case when inp.SexID = '1' then '男' when inp.SexID = '2' then '女' else '未知' end) as sex,
                                                           dept.name dept, 
                                                           /*diag1.name AdmitDiag,
                                                           diag2.name OutDiag, */
                                                           imd.diagnosis_name OutDiag,
                                                           basic.mzzyzd_name,
                                                           basic.mzxyzd_name,
                                                           '' as ADMITDIAG,
                                                           temp.OperName operation,
                                                           (case when lengthb(inp.admitdate)>10 then substr(inp.admitdate,0,10) end) as admitdate,
                                                           (case when lengthb(inp.outhosdate)>10 then substr(inp.outhosdate,0,10) end) as outhosdate,
                                                           user1.name RESIDENT,
                                                           user2.name CHIEF,
                                                           inp.INCOUNT   from (select distinct inp.patid,
                        wmsys.wm_concat( distinct  ope.operation_name || '(' ||
                                        ope.operation_date || ')') OperName
          from inpatient inp
          left join {5} bas on inp.noofinpat =
                                                       bas.noofinpat and bas.valide=1
          left join {6} ope on ope.iem_mainpage_no =
                                                       bas.iem_mainpage_no
                                                   and ope.valide = 1 and ope.valide=1 
         group by inp.patid) temp
  left join inpatient inp on inp.patid = temp.patid
  left join Department dept on inp.admitdept = dept.id and dept.valid=1
  left join {5} basic on basic.noofinpat =
                                                 inp.noofinpat and basic.valide=1
  left join {7} imd on basic.iem_mainpage_no =
                                               imd.iem_mainpage_no and imd.valide=1
  /*left join diagnosis diag1 on imd.diagnosis_code = diag1.icd and diag1.valid=1
  left join diagnosis diag2 on inp.outdiagnosis = diag2.icd and diag2.valid=1*/
  left join users user1 on user1.id = inp.RESIDENT and user1.valid=1
  left join users user2 on user2.id = inp.CHIEF and user2.valid=1
  left join {6} oper on oper.iem_mainpage_no =
                                                basic.iem_mainpage_no and oper.valide=1
 where (basic.MZXYZD_CODE = '{0}' or '{0}' = '' or '{0}' is null) 
                                                           and (inp.outdiagnosis = '{1}' or '{1}' = '' or '{1}' is null)
                                                           and (oper.operation_code = '{2}' or '{2}' = '' or '{2}' is null)
                                                           and to_date( inp.outhosdate,'yyyy-mm-dd hh24:mi:ss') between to_date('{3}','yyyy-mm-dd hh24:mi:ss') and to_date('{4}','yyyy-mm-dd hh24:mi:ss') order by NOOFINPAT asc";




        //        /// <summary> 
        //        /// 获取手术统计模块数据    参数 0： 医师编号  1： 病历创建时间起始  2： 病历创建时间截止
        //        /// </summary>
        /// <summary>
        /// edit by ywk 2013年8月23日 11:56:39 二次更改，取得出院诊断
        /// </summary>
        public const string sql_QueryByDoctor = @"select   distinct inp.NOOFINPAT,
                                                           inp.name, 
                                                           inp.patid, 
                                                           inp.age,
                                                           inp.TOTALDAYS,
                                                           (case when inp.SexID = 1 then '男' when inp.SexID = 2 then '女' else '未知' end) as sex,
                                                           dept.name dept, user3.name creatuser,
                                                           diag1.name AdmitDiag,
                                                                diagtemp.OutDiagName OutDiag,
                                                           basic.create_time,
                                                           temp.OperName operation,
                                                           (case when lengthb(inp.admitdate)>10 then substr(inp.admitdate,0,10) end) as admitdate,
                                                           (case when lengthb(inp.outhosdate)>10 then substr(inp.outhosdate,0,10) end) as outhosdate,
                                                           user1.name RESIDENT,
                                                           user2.name CHIEF,
                                                           inp.INCOUNT 
                                                           from
                                                            (select distinct inp.patid,bas.iem_mainpage_no,
                                                              wmsys.wm_concat(distinct ope.operation_name || '(' || ope.operation_date || ')') OperName
                                                              from {4} bas 
                                                             left join inpatient inp
                                                             on inp.noofinpat=bas.noofinpat
                                                               left join {5} ope
                                                             on ope.iem_mainpage_no = bas.iem_mainpage_no where ope.valide = 1 
                                                              group by inp.patid,bas.iem_mainpage_no) temp
                                                           left join {4} basic on basic.iem_mainpage_no = temp.iem_mainpage_no
                                                           left join inpatient inp on basic.noofinpat = inp.noofinpat
                                                           left join Department dept on inp.admitdept = dept.id
                                                           left join diagnosis diag1 on inp.admitdiagnosis = diag1.icd
                                                           left join (select distinct inp.noofinpat,
                                                            bas.iem_mainpage_no,
                                                                wmsys.wm_concat(distinct diag.diagnosis_code || '(' ||
                                                          diag.diagnosis_name || ')') OutDiagName
                                                  from {4} bas
                                                  left join inpatient inp
                                                    on inp.noofinpat = bas.noofinpat
                                                  left join {6} diag
                                                    on diag.iem_mainpage_no = bas.iem_mainpage_no
                                                 where diag.valide = 1 and diag.diagnosis_type_id<>13
                                                 group by inp.noofinpat, bas.iem_mainpage_no) diagtemp on inp.noofinpat=diagtemp.noofinpat
                                                           left join users user1 on user1.id = inp.RESIDENT
                                                           left join users user2 on user2.id = inp.CHIEF
                                                           left join users user3 on user3.id=basic.create_user
                                                           where (basic.CREATE_USER = '{0}' or '{0}' = '' or '{0}' is null)                                                           
                                                           and To_Date(substr(nvl(basic.create_time,'2999-01-01'),0,10),'yyyy-mm-dd')>=to_date('{1}','yyyy-mm-dd') 
                                                           and To_Date(substr(nvl(basic.create_time,'1990-01-01'),0,10),'yyyy-mm-dd')<=to_date('{2}','yyyy-mm-dd')
                                                           and (inp.admitdept='{3}' or '{3}' is null or '{3}' = '')
                                                           and basic.valide = 1";
    }
}
