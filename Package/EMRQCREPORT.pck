create or replace package EMRQCREPORT
  IS
  TYPE empcurtype IS REF CURSOR;
  
------出院未提交病理统计 
PROCEDURE GetOutHospitalNOSubmit(v_deptid VARCHAR  DEFAULT '*',
                                     v_timebegin VARCHAR  DEFAULT '',
                                     v_timeend VARCHAR  DEFAULT '',
                                      o_result OUT empcurtype);

------出院未归档病历统计
PROCEDURE GetOutHospitalNOLock(v_deptid VARCHAR  DEFAULT '*',
                                     v_timebegin VARCHAR  DEFAULT '',
                                     v_timeend VARCHAR  DEFAULT '',
                                      o_result OUT empcurtype);

-----------死亡信息统计          zyx2013-03-21
PROCEDURE usp_GetQCDieInfo(v_deptid VARCHAR DEFAULT '' ,
                           v_timebegin VARCHAR  DEFAULT '',
                           v_timeend VARCHAR  DEFAULT '',
                            o_result OUT empcurtype);
                            
                            
 procedure usp_GetQCDeptDieInfo(v_timebegin varchar,v_timeend varchar,o_result OUT empcurtype);
                            
--------手术信息统计
PROCEDURE usp_GetQCOperatInfo(v_deptid VARCHAR ,
                           v_timebegin VARCHAR  DEFAULT '',
                           v_timeend VARCHAR  DEFAULT '',
                            o_result OUT empcurtype);
                            
--------质控失分项目统计
PROCEDURE usp_GetQCLostScoreCat(v_deptid VARCHAR ,
                                v_qctype VARCHAR ,
                                v_timebegin VARCHAR  DEFAULT '',
                                v_timeend VARCHAR  DEFAULT '',
                                v_doctor VARCHAR ,
                                v_zkbegindate VARCHAR ,
                                v_zkenddate VARCHAR ,
                                o_result OUT empcurtype);


--------质控评分记录统计
PROCEDURE usp_GetQCScoreRecord(v_deptid VARCHAR ,
                                v_qctype VARCHAR ,
                                v_timebegin VARCHAR  DEFAULT '',
                                v_timeend VARCHAR  DEFAULT '',
                                  o_result OUT empcurtype
                                );
                                 
--------病人门诊诊断明细统计
PROCEDURE usp_GetQCQCPatDiaDetail(v_deptid VARCHAR ,
                           v_timebegin VARCHAR  DEFAULT '',
                           v_timeend VARCHAR  DEFAULT '',
                           v_diagnosiscode VARCHAR default '',
                            o_result OUT empcurtype);
                            
 ------------------------一段时间内出院病人的死亡率 2013-3-23  by tj
 procedure usp_GetPatDieRate(v_datefrom varchar,v_dateto varchar,o_result OUT empcurtype);
 
 
 procedure usp_GetDeptDieInfo(v_datebegin varchar,v_dateend varchar,o_result OUT empcurtype);
 
 
 procedure usp_GetConsultationInfo(v_datebegin varchar,v_dateend varchar,o_result OUT empcurtype);
 
 procedure usp_GetDiaInfo(v_datebegin varchar,v_dateend varchar,o_result OUT empcurtype);
 
end ;
/
create or replace package body EMRQCREPORT is

-----------出院未提交病理统计 
PROCEDURE GetOutHospitalNOSubmit(v_deptid VARCHAR  DEFAULT '*',
                                     v_timebegin VARCHAR  DEFAULT '',
                                     v_timeend VARCHAR  DEFAULT '',
                                      o_result OUT empcurtype) AS
BEGIN 
  OPEN o_result FOR
    select (case
                     when count(b.ID) > 0 then
                      '书写'
                     else
                      '未写'
                   end) RECORDSTATE,
                   '' QC,
                   a.NoOfInpat AS NOOFINPAT,
                   a.NOOFRECORD as NOOFRECORD,
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
             and b.hassubmit = 4600   
               and (a.OutHosDept = v_deptid or v_deptid = '*' or v_deptid is null or v_deptid=''  or v_deptid='0000')
         and to_date(substr(nvl(trim(a.outhosdate), '1990-01-01'), 1, 10),
          'yyyy-mm-dd') >= to_date(v_timebegin, 'yyyy-mm-dd')
         and to_date(substr(nvl(trim(a.outhosdate), '1990-01-01'), 1, 10),
            'yyyy-mm-dd') < to_date(v_timeend, 'yyyy-mm-dd')
             group by a.NoOfInpat,
                      a.NOOFRECORD,
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
                      users.name;
                      END;

------出院未归档病历统计
PROCEDURE GetOutHospitalNOLock(v_deptid VARCHAR  DEFAULT '*',
                               v_timebegin VARCHAR  DEFAULT '',
                               v_timeend VARCHAR  DEFAULT '',
                                o_result OUT empcurtype) AS
BEGIN 
  OPEN o_result FOR
  select (case
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
           and b.islock in (4700, 4702, 4703)  
             and (a.OutHosDept =v_deptid or v_deptid = '*'or v_deptid is null or v_deptid=''or v_deptid='0000')
       and to_date(substr(nvl(trim(a.outhosdate), '1990-01-01'), 1, 10),
        'yyyy-mm-dd') >= to_date(v_timebegin, 'yyyy-mm-dd')
       and to_date(substr(nvl(trim(a.outhosdate), '1990-01-01'), 1, 10),
          'yyyy-mm-dd') < to_date(v_timeend, 'yyyy-mm-dd')
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
                    users.name;
  END;

-----------死亡信息统计          zyx2013-03-21
PROCEDURE usp_GetQCDieInfo(v_deptid VARCHAR  DEFAULT '' ,
                           v_timebegin VARCHAR  DEFAULT '',
                           v_timeend VARCHAR  DEFAULT '',
                            o_result OUT empcurtype) AS
BEGIN 
  OPEN o_result FOR
  select
       a.patid patid,
       a.name patName,
       (case
         when a.SexID = 1 then
          '男'
         when a.SexID = 2 then
          '女'
         else
          '未知'
       end) as sex ,
     --  a.agestr,
       diag.name diag ,
      a.admitdate  admitdate,
--a.outhosdate,
     --  a.address,
     round(sysdate - to_date(a.outhosdate, 'yyyy-mm-dd hh24:mi:ss'),0) dieSum ,
     -- feeinfo.totalfee fee,
      -- dept.name dept_name--,
      u.name docName
  from inpatient a
  left join diagnosis diag
    on a.AdmitDiagnosis = diag.markid
  left join department dept
    on a.outhosdept = dept.id
  left join doctor_assignpatient doc
    on doc.noofinpat = a.noofinpat
  left join users u
    on doc.id = u.id
  left join iem_mainpage_basicinfo_2012 mainpage
    on mainpage.noofinpat = a.noofinpat
    left join iem_mainpage_feeinfo feeinfo
    on feeinfo.iem_mainpage_no=mainpage.iem_mainpage_no
 where mainpage.zg_flag = '4' 
   and (a.outhosdept = v_deptid
    or v_deptid is null or v_deptid='0000')
and to_date( a.outhosdate,'yyyy-mm-dd hh24:mi:ss') between to_date(v_timebegin,'yyyy-mm-dd') and to_date(v_timeend,'yyyy-mm-dd');
  END;
  
 procedure usp_GetQCDeptDieInfo(v_timebegin varchar,v_timeend varchar,o_result OUT empcurtype) AS
    begin
        open   o_result for
       select t1.dept deptId,t3.name deptName, t1.total sumDie,t2.total sumTotal,round(t1.total/t2.total,2)*100||'%' dieRate,v_timebegin timebegin,v_timeend timeend from 
(
(select
 case
   when  g.outhosdept is null then '其他'
     else  g.outhosdept
  end as dept,
count(g.patnoofhis) total 
from iem_mainpage_basicinfo_2012 g
where g.valide='1'and g.zg_flag='4'
and  to_date(g.outwarddate,'yyyy-mm-dd hh24:mi:ss')  between to_date(v_timebegin,'yyyy-mm-dd hh24:mi:ss') and to_date(v_timeend,'yyyy-mm-dd hh24:mi:ss')
group by g.outhosdept
) t1
left join
(
select
 case
   when  g.outhosdept is null then '其他'
     else  g.outhosdept
  end as dept,
count(g.patnoofhis) total 
from iem_mainpage_basicinfo_2012 g
where g.valide='1'
and  to_date(g.outwarddate,'yyyy-mm-dd hh24:mi:ss')  between to_date(v_timebegin,'yyyy-mm-dd hh24:mi:ss') and to_date(v_timeend,'yyyy-mm-dd hh24:mi:ss')
group by g.outhosdept
)t2
on
t1.dept=t2.dept
left join
(
select department.id,department.name as name from department 
)t3
on
t1.dept=t3.id
);end;

--------手术信息统计
PROCEDURE usp_GetQCOperatInfo(v_deptid VARCHAR ,
                           v_timebegin VARCHAR  DEFAULT '',
                           v_timeend VARCHAR  DEFAULT '',
                            o_result OUT empcurtype)AS
BEGIN 

  OPEN o_result FOR
 select  distinct b.patid,
                            b.OperName,
                           a.name,
                           (case
                           when a.SexID = 1 then
                           '男'
                           when a.SexID = 2 then
                           '女'
                           else
                           '未知'
                           end) as PATSEX,
                           a.agestr,
                           diag.name diag_name,
                           a.admitdate,
                           a.outhosdate,
                           a.address,
                           '1' qjcs,
                           '1' CGCS,
                           '' lx,
                           '未结算' fee,
                           dept.name dept_name,
                           u.name doc_name
                           from   (select distinct inp.patid,
                                          wmsys.wm_concat(ope.operation_name || '(' || ope.operation_date || ')') OperName
                                          from inpatient inp
                                         left join iem_mainpage_basicinfo_2012 bas
                                         on inp.noofinpat=bas.noofinpat
                                           left join iem_mainpage_operation_2012 ope
                                         on ope.iem_mainpage_no = bas.iem_mainpage_no and ope.valide = 1 
                                          group by inp.patid) b
                           inner join inpatient a
                           on a.patid=b.patid
                           inner join iem_mainpage_basicinfo_2012 basic
                           on basic.noofinpat = a.noofinpat
                           inner join iem_mainpage_operation_2012 oper
                           on oper.iem_mainpage_no = basic.iem_mainpage_no and oper.valide = 1 
                           left join diagnosis diag
                           on a.AdmitDiagnosis = diag.markid
                           inner join department dept
                           on a.outhosdept = dept.id
                           and (dept.id=v_deptid or v_deptid is null or v_deptid=''or v_deptid='0000')
                           left join doctor_assignpatient doc
                           on doc.noofinpat = a.noofinpat
                           left join users u  on doc.id = u.id
                           where  to_date(substr(nvl(trim(a.admitdate), '1990-01-01'), 1, 10), 'yyyy-mm-dd') >= to_date(v_timebegin, 'yyyy-mm-dd')
                           and to_date(substr(nvl(trim(a.admitdate), '1990-01-01'), 1, 10), 'yyyy-mm-dd') <= to_date(v_timeend, 'yyyy-mm-dd') order by a.admitdate desc ;
  END;

--------质控失分项目统计
PROCEDURE usp_GetQCLostScoreCat(v_deptid VARCHAR ,
                                v_qctype VARCHAR ,
                                v_timebegin VARCHAR  DEFAULT '',
                                v_timeend VARCHAR  DEFAULT '',
                                v_doctor VARCHAR ,
                                v_zkbegindate VARCHAR ,
                                v_zkenddate VARCHAR ,
                                o_result OUT empcurtype)AS
BEGIN 

  OPEN o_result FOR
  select ip.patid ,
          dep.name deptname ,
          ip.name ipname ,
          ip.inwarddate  ,
          ep.createusername  ,
          ep.problem_desc  ,
          ep.doctorname  ,
          ep.noofinpat  ,
          ep.reducepoint  ,
          ep.recorddetailname  ,
          ec.childname  ,
          decode(r.qctype, 0, '环节质控', '终末质控')  qctype,
          to_char(r.create_time,'yyyy-MM-dd hh24:mi') create_time,
          di.cname
     from emr_point           ep,
          emr_configpoint     ec,
          emr_automark_record r,
          inpatient           ip,
          department          dep,
          dict_catalog  di
    where  (ep.emrpointid = ec.childcode or (ep.emrpointid like '-%' and ep.sortid=ec.ccode))
      and ep.emr_mark_record_id = r.id
      and ep.valid='1'
      and ip.noofinpat = ep.noofinpat
      and ec.valid='1'
      and dep.id = ip.outhosdept
      and di.ccode=ep.sortid
      and (dep.id = v_deptid or v_deptid is null or v_deptid=''or v_deptid='0000' )
      and (r.qctype = v_qctype or v_qctype = '-1' or v_qctype=''or v_qctype is null)
      and di.ccode=ep.sortid
      and to_char(to_date(v_timebegin, 'yyyy-MM-dd'), 'yyyy-MM-dd') <= ip.ADMITDATE
      AND ip.ADMITDATE <=  to_char(to_date(v_timeend, 'yyyy-MM-dd'), 'yyyy-MM-dd')
     and (ep.doctorid = v_doctor or v_doctor is null or v_doctor='')
and to_date(v_zkbegindate, 'yyyy-MM-dd') <= r.create_time
      AND r.create_time <=to_date(v_zkenddate, 'yyyy-MM-dd')
    order by dep.id, ip.OUTBED;
  END;

--------质控评分记录统计
PROCEDURE usp_GetQCScoreRecord(v_deptid VARCHAR ,
                                v_qctype VARCHAR ,
                                v_timebegin VARCHAR  DEFAULT '',
                                v_timeend VARCHAR  DEFAULT '',
                                  o_result OUT empcurtype
                                )AS
BEGIN 

  OPEN o_result FOR
  
  select dep.name deptname,
       ip.name ipname,
       ip.patid,
       rd.noofinpat,
       decode(rd.qctype, 0, '环节质控', '终末质控') qctype,
       ip.inwarddate,
       rd.name recordname,
       rd.score,
      u.name  CREATEUSERNAME,
       rd.id,
       decode(rd.qctype, 0, '85', '100') sumpoint
  from emr_automark_record rd, inpatient ip, department dep,users u
 where ip.noofinpat = rd.noofinpat
   and dep.id = ip.outhosdept
   and u.id=rd.create_user
     and (dep.id = v_deptid or v_deptid is null or v_deptid='0000' or v_deptid='' )
      and (rd.qctype = v_qctype or v_qctype = '-1' or v_qctype='' or v_qctype is null )
      and to_char(to_date(v_timebegin, 'yyyy-MM-dd'), 'yyyy-MM-dd') <= ip.ADMITDATE
      AND ip.ADMITDATE <=  to_char(to_date(v_timeend, 'yyyy-MM-dd'), 'yyyy-MM-dd')
      and rd.isvalid='1' and rd.isauto='0'
    order by dep.id, ip.OUTBED;
  END;
                                                       
--------病人门诊诊断明细统计
PROCEDURE usp_GetQCQCPatDiaDetail(v_deptid VARCHAR ,
                           v_timebegin VARCHAR  DEFAULT '',
                           v_timeend VARCHAR  DEFAULT '',
                           v_diagnosiscode VARCHAR default '',
                            o_result OUT empcurtype) AS
BEGIN 

  OPEN o_result FOR
  select ROW_NUMBER() OVER(ORDER BY a.outbed ASC) AS xh,
                       a.patid,
                       a.name,
                       (case
                         when a.SexID = 1 then
                          '男'
                         when a.SexID = 2 then
                          '女'
                         else
                          '未知'
                       end) as PATSEX,
                       a.agestr,
                       diag.name diag_name,
                       a.admitdate,
                       a.address,
                       dept.name dept_name,
                       u.name doc_name
                  from inpatient a
                  left join iem_mainpage_basicinfo_2012 imb 
                    on imb.noofinpat = a.noofinpat
                  left join iem_mainpage_diagnosis_2012 imd
                    on imd.iem_mainpage_no = imb.iem_mainpage_no
                  left join diagnosis diag
                    on imd.diagnosis_code=diag.markid
                  left join department dept
                    on a.admitdept = dept.id
                  left join doctor_assignpatient doc
                    on doc.noofinpat = a.noofinpat
                  left join users u
                    on doc.id = u.id                  
                 where (a.admitdept = v_deptid or v_deptid=''or v_deptid is null or v_deptid='0000') and imd.diagnosis_type_id='13' and imd.valide='1' and imb.valide='1'
                   and to_date( a.inwarddate,'yyyy-mm-dd hh24:mi:ss') between to_date(v_timebegin,'yyyy-mm-dd hh24:mi:ss') and to_date(v_timeend,'yyyy-mm-dd hh24:mi:ss') and  ( imd.diagnosis_code=v_diagnosiscode or v_diagnosiscode='' or v_diagnosiscode is null );
  END;
  
 ------------------------一段时间内出院病人的死亡率 2013-3-23  by tj
  procedure usp_GetPatDieRate(v_datefrom varchar,v_dateto varchar,o_result OUT empcurtype) AS
    begin
        open   o_result for
        select count(basic.IEM_MAINPAGE_NO)from IEM_MAINPAGE_BASICINFO_2012  basic
               where basic.Valide='1'
                  and to_date(basic.outwarddate,'yyyy-mm-dd hh24:mi:ss') between to_date(v_datefrom,'yyyy-mm-dd hh24:mi:ss') and to_date(v_dateto,'yyyy-mm-dd hh24:mi:ss') 
                 union
               select count(basic.IEM_MAINPAGE_NO)from IEM_MAINPAGE_BASICINFO_2012  basic
               where basic.Valide='1' and 
               to_date(basic.outwarddate,'yyyy-mm-dd hh24:mi:ss')  between to_date(v_datefrom,'yyyy-mm-dd hh24:mi:ss') and to_date(v_dateto,'yyyy-mm-dd hh24:mi:ss') 
               and basic.zg_flag ='4'  ;
   end;
               
------------------------            
procedure usp_GetDeptDieInfo(v_datebegin varchar,v_dateend varchar,o_result OUT empcurtype) AS
    begin
        open   o_result for
       select t1.dept 科室ID,t3.name 科室名称, t1.total 死亡人数,t2.total 科室出院总人数,round(t1.total/t2.total,2)*100||'%' 科室死亡率,v_datebegin 开始时间,v_dateend 结束时间 from 
(
(select
 case
   when  g.outhosdept is null then '其他'
     else  g.outhosdept
  end as dept,
count(g.patnoofhis) total 
from iem_mainpage_basicinfo_2012 g
where g.valide='1'and g.zg_flag='4'
and  to_date(g.outwarddate,'yyyy-mm-dd hh24:mi:ss')  between to_date(v_datebegin,'yyyy-mm-dd hh24:mi:ss') and to_date(v_dateend,'yyyy-mm-dd hh24:mi:ss')
group by g.outhosdept
) t1
left join
(
select
 case
   when  g.outhosdept is null then '其他'
     else  g.outhosdept
  end as dept,
count(g.patnoofhis) total 
from iem_mainpage_basicinfo_2012 g
where g.valide='1'
and  to_date(g.outwarddate,'yyyy-mm-dd hh24:mi:ss')  between to_date(v_datebegin,'yyyy-mm-dd hh24:mi:ss') and to_date(v_dateend,'yyyy-mm-dd hh24:mi:ss')
group by g.outhosdept
)t2
on
t1.dept=t2.dept
left join
(
select department.id,department.name as name from department 
)t3
on
t1.dept=t3.id
);end;

--------------------------
procedure usp_GetConsultationInfo(v_datebegin varchar,v_dateend varchar,o_result OUT empcurtype) AS
    begin
        open   o_result for
        select (select d.name from department d where d.id=t1.applydept) 申请科室,t1.finished 已完成申请数,t2.total 科室总申请数,t1.finished/t2.total*100 完成率 from 
(
(select t.applydept,count(t.consultapplysn) finished from consultapply t
where t.valid=1 and t.ispassed='1'
and t.stateid='6741'
and  to_date(t.applytime,'yyyy-mm-dd hh24:mi:ss')  between to_date(v_datebegin,'yyyy-mm-dd hh24:mi:ss') and to_date(v_dateend,'yyyy-mm-dd hh24:mi:ss')
group by t.applydept)t1 
left join

(select t.applydept,count(t.consultapplysn) total from consultapply t
where t.valid=1 and t.ispassed='1'
and  to_date(t.applytime,'yyyy-mm-dd hh24:mi:ss')  between to_date(v_datebegin,'yyyy-mm-dd hh24:mi:ss') and to_date(v_dateend,'yyyy-mm-dd hh24:mi:ss')
group by t.applydept) t2

on t1.applydept=t2.applydept
);

        end;


procedure usp_GetDiaInfo(v_datebegin varchar,v_dateend varchar,o_result OUT empcurtype) AS
    begin
        open   o_result for
       select t8.deptId 科室Id,case
   when  t8.deptname is null then '其他'
     else t8.deptname
  end as 科室名称,t9.sumTotal 总人数,t8.sumCount 诊断一致数 ,v_datebegin 开始时间,v_dateend 结束时间 from (
(select t6.deptId deptId,t7.deptname deptname,t6.sumCount sumCount from
(select 
 case
   when  t5.outhosdept is null then '其他'
     else t5.outhosdept
  end as deptId,count(t5.patnoofhis) as sumCount from 
(select t4.outhosdept,t4.patnoofhis from
((select t.iem_mainpage_no ,min(t.order_value) maindia
from iem_mainpage_diagnosis_2012 t where t.valide='1' 
 and  to_date(t.create_time,'yyyy-mm-dd hh24:mi:ss')  between to_date(v_datebegin,'yyyy-mm-dd hh24:mi:ss') and to_date(v_dateend,'yyyy-mm-dd hh24:mi:ss')
group by t.iem_mainpage_no
)t1
left join
(select t.iem_mainpage_no,t.order_value, t.diagnosis_code
from iem_mainpage_diagnosis_2012 t where t.valide='1' and t.diagnosis_type_id='7'
and to_date(t.create_time,'yyyy-mm-dd hh24:mi:ss')  between to_date(v_datebegin,'yyyy-mm-dd hh24:mi:ss') and to_date(v_dateend,'yyyy-mm-dd hh24:mi:ss')
) t2
on  (t1.iem_mainpage_no=t2.iem_mainpage_no and t1.maindia=t2.order_value) 
left join 
(
  select iem.patnoofhis,iem.iem_mainpage_no from iem_mainpage_basicinfo_2012 iem 
)t3
on (t3.iem_mainpage_no=t2.iem_mainpage_no)
left join 
(
select inp.patnoofhis, inp.outhosdept ,inp.ADMITDIAGNOSIS from inpatient inp  
)t4 on(t3.patnoofhis=t4.patnoofhis and t2.diagnosis_code=t4.ADMITDIAGNOSIS)
)) t5
group by t5.outhosdept)
t6
left join 
(
select dep.id deptid,case
   when  dep.name is null then '其他'
     else dep.name
  end as deptname from department dep
)t7
on t6.deptId=t7.deptid)
t8
left join
(
  select count(i.noofinpat) sumTotal, i.outhosdept outhosward from inpatient i group by i.outhosdept
)t9
on  t8.deptId=t9.outhosward) ;
        end;

       
        end;
/
