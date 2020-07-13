create or replace view baseinfo as
select
iembase.iem_mainpage_no iemno,
iembase.mzxyzd_code AdmissionDiseaseId,
iembase.mzxyzd_name AdmissionDiseaseName,
iembase.CURE_TYPE zllb,
iembase.INHOSTYPE rytj,
iembase.mzzyzd_code mzzyzd,
iembase.mzzyzd_name mzzymc,
 inp.patid,
       iembase.noofinpat,
       iembase.sexid,
       decode(iembase.ALLERGIC_FLAG,'1',1,0) IsDrugAllergy,
       iembase.Allergic_Drug AllergyDrugCode,
       iembase.Allergic_Drug AllergyDrugName,
       decode(iembase.PATHOLOGY_DIAGNOSIS_ID,'',0,1) IsPathologicalExamination,
       iembase.PATHOLOGY_OBSERVATION_SN PathologyCode,
       decode(iembase.HOSPITAL_SENSE,'',0,1) IsHospitalInfected,
       iembase.HOSPITAL_SENSE HospitalInfectedCode,
       iembase.BLOODTYPE BloodTypeS,
       iembase.RH BloodTypeE,
       iembase.OUTHOSTYPE LeaveHospitalType,
       iembase.MARITAL Marriage,
       note2.Height,
       note2.Weight,
       decode(inp.isbaby,0,'',inp.birth) NewbornDate,
       iembase.WEIGHT NewbornWeight,
       iembase.INWEIGHT NewbornCurrentWeight,
       inp.specas Tsblbs,
       inp.payway,
       iembase.SECTION_DIRECTOR KZRBM,
       (select name from users where users.id = substr('000' || iembase.SECTION_DIRECTOR,-6,6)) KZRMC,
       iembase.DIRECTOR ZRYSBM,
       (select name from users where users.id = substr('000' || iembase.DIRECTOR,-6,6)) ZRYSMC,
       iembase.VS_EMPLOYEE_CODE ZZYSBM,
       (select name from users where users.id = substr('000' || iembase.VS_EMPLOYEE_CODE,-6,6)) ZZYSMC,
       iembase.RESIDENT_EMPLOYEE_CODE ZYYSBM,
       (select name from users where users.id = substr('000' || iembase.RESIDENT_EMPLOYEE_CODE,-6,6)) ZYYSMC,
       iembase.DUTY_NURSE ZRHSBM,
       (select name from users where users.id = substr('000' || iembase.DUTY_NURSE,-6,6)) ZRHSMC,
       iembase.REFRESH_EMPLOYEE_CODE JXYSBM,
       (select name from users where users.id = substr('000' || iembase.REFRESH_EMPLOYEE_CODE,-6,6)) JXYSMC,
       iembase.INTERNE SXYSBM,
       (select name from users where users.id = substr('000' || iembase.INTERNE,-6,6)) SXYSMC,
       iembase.CODING_USER BMYBM,
       (select name from users where users.id = substr('000' || iembase.CODING_USER,-6,6)) BMYMC

from iem_mainpage_basicinfo_sx iembase
left join inpatient inp
on iembase.noofinpat = inp.noofinpat
left join (select note1.*
  from notesonnursing note1
  right join (select note.noofinpat, max(note.dateofrecord) dateofrecord
               from notesonnursing note
              group by note.noofinpat) ss
    on note1.noofinpat = ss.noofinpat and note1.dateofrecord = ss.dateofrecord) note2
on iembase.noofinpat = note2.noofinpat and note2.timeslot = '2';
