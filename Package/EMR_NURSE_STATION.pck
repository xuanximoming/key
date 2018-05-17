create or replace package EMR_NURSE_STATION IS
  TYPE empcurtype IS REF CURSOR;

  -- Author  : xll
  -- Created : 2012-8-9 9:26:11

  --通过单据ID和人员ID获取单据信息
  PROCEDURE usp_GetNurseRecord(v_NoOfInpatient  IN varchar2,
                               v_FatherRecordId IN varchar2,
                               o_result         OUT empcurtype);

  --添加或修改记录单记录
  PROCEDURE usp_AddOrModNurseRecord(v_ID              IN varchar2,
                                    v_NOOFINPATENT    IN varchar2,
                                    v_FATHER_RECORDID IN varchar2,
                                    v_RECORD_DATETIME IN varchar2,
                                    v_TIWEN           IN varchar2,
                                    v_MAIBO           IN varchar2,
                                    v_HUXI            IN varchar2,
                                    v_XUEYA           IN varchar2,
                                    v_YISHI           IN varchar2,
                                    v_XYBHD           IN varchar2,
                                    v_QKFL            IN varchar2,
                                    v_SYPF            IN varchar2,
                                    v_OTHER_ONE       IN varchar2,
                                    v_OTHER_TWO       IN varchar2,
                                    v_OTHER_THREE     IN varchar2,
                                    v_OTHER_FOUR      IN varchar2,
                                    v_ZTKDX           IN varchar2,
                                    v_YTKDX           IN varchar2,
                                    v_TKDGFS          IN varchar2,
                                    v_WOWEI           IN varchar2,
                                    v_JMZG            IN varchar2,
                                    v_DGJYLG_ONE      IN varchar2,
                                    v_DGJYLG_TWO      IN varchar2,
                                    v_DGJYLG_THREE    IN varchar2,
                                    v_In_ITEM         IN varchar2,
                                    v_In_VALUE        IN varchar2,
                                    v_OUT_ITEM        IN varchar2,
                                    v_OUT_VALUE       IN varchar2,
                                    v_OUT_COLOR       IN varchar2,
                                    v_OUT_STATUE      IN varchar2,
                                    v_OTHER           IN varchar2,
                                    v_HXMS            IN varchar2,
                                    v_FCIMIAO         IN varchar2,
                                    v_XRYND           IN varchar2,
                                    v_CGSD            IN varchar2,
                                    v_LXXZYTQ         IN varchar2,
                                    v_BDG             IN varchar2,
                                    v_SINGE_DOCTOR    IN varchar2,
                                    v_SINGE_DOCTORID  IN varchar2,
                                    v_CREATE_DOCTORID IN varchar2,
                                    v_CREATE_TIME     IN varchar2,
                                    v_VALID           IN varchar2);

  --删除记录单
  PROCEDURE usp_DelNurseRecord(v_NurseId IN varchar2);

  --查询某科室下所有的病人数据 add by tj 2012-10-30 
  procedure usp_GetPatientsOfDept(v_departCode  varchar2,
                                  v_wardcode    varchar2,
                                  v_Timelot     varchar2,
                                  v_TimelotSave varchar2,
                                  v_date        varchar2,
                                  o_result      OUT empcurtype);

  procedure usp_GetPatientsOfDept2(v_departCode  varchar2,
                                   v_wardcode    varchar2,
                                   v_Timelot     varchar2,
                                   v_TimelotSave varchar2,
                                   v_date        varchar2,
                                   o_result      OUT empcurtype,
                                   o_result1     OUT empcurtype,
                                   o_result2     OUT empcurtype);

end EMR_NURSE_STATION;
/
create or replace package body EMR_NURSE_STATION is

  --通过单据ID和人员ID获取单据信息
  PROCEDURE usp_GetNurseRecord(v_NoOfInpatient  IN varchar2,
                               v_FatherRecordId IN varchar2,
                               o_result         OUT empcurtype) AS
  BEGIN
    OPEN o_result FOR
      select *
        from nurse_recordoper re
       where re.noofinpatent = v_NoOfInpatient
         and re.father_recordid = v_FatherRecordId
         and re.valid = '1';
  END;

  --添加或修改记录单记录
  PROCEDURE usp_AddOrModNurseRecord(v_ID              IN varchar2,
                                    v_NOOFINPATENT    IN varchar2,
                                    v_FATHER_RECORDID IN varchar2,
                                    v_RECORD_DATETIME IN varchar2,
                                    v_TIWEN           IN varchar2,
                                    v_MAIBO           IN varchar2,
                                    v_HUXI            IN varchar2,
                                    v_XUEYA           IN varchar2,
                                    v_YISHI           IN varchar2,
                                    v_XYBHD           IN varchar2,
                                    v_QKFL            IN varchar2,
                                    v_SYPF            IN varchar2,
                                    v_OTHER_ONE       IN varchar2,
                                    v_OTHER_TWO       IN varchar2,
                                    v_OTHER_THREE     IN varchar2,
                                    v_OTHER_FOUR      IN varchar2,
                                    v_ZTKDX           IN varchar2,
                                    v_YTKDX           IN varchar2,
                                    v_TKDGFS          IN varchar2,
                                    v_WOWEI           IN varchar2,
                                    v_JMZG            IN varchar2,
                                    v_DGJYLG_ONE      IN varchar2,
                                    v_DGJYLG_TWO      IN varchar2,
                                    v_DGJYLG_THREE    IN varchar2,
                                    v_In_ITEM         IN varchar2,
                                    v_In_VALUE        IN varchar2,
                                    v_OUT_ITEM        IN varchar2,
                                    v_OUT_VALUE       IN varchar2,
                                    v_OUT_COLOR       IN varchar2,
                                    v_OUT_STATUE      IN varchar2,
                                    v_OTHER           IN varchar2,
                                    v_HXMS            IN varchar2,
                                    v_FCIMIAO         IN varchar2,
                                    v_XRYND           IN varchar2,
                                    v_CGSD            IN varchar2,
                                    v_LXXZYTQ         IN varchar2,
                                    v_BDG             IN varchar2,
                                    v_SINGE_DOCTOR    IN varchar2,
                                    v_SINGE_DOCTORID  IN varchar2,
                                    v_CREATE_DOCTORID IN varchar2,
                                    v_CREATE_TIME     IN varchar2,
                                    v_VALID           IN varchar2) AS
  BEGIN
    IF v_ID IS NULL THEN
      insert into nurse_recordoper
      values
        (to_char(NURSERECORDID_SEQ.NEXTVAL),
         v_NOOFINPATENT,
         v_FATHER_RECORDID,
         v_RECORD_DATETIME,
         v_TIWEN,
         v_MAIBO,
         v_HUXI,
         v_XUEYA,
         v_YISHI,
         v_XYBHD,
         v_QKFL,
         v_SYPF,
         v_OTHER_ONE,
         v_OTHER_TWO,
         v_OTHER_THREE,
         v_OTHER_FOUR,
         v_ZTKDX,
         v_YTKDX,
         v_TKDGFS,
         v_WOWEI,
         v_JMZG,
         v_DGJYLG_ONE,
         v_DGJYLG_TWO,
         v_DGJYLG_THREE,
         v_In_ITEM,
         v_In_VALUE,
         v_OUT_ITEM,
         v_OUT_VALUE,
         v_OUT_COLOR,
         v_OUT_STATUE,
         v_OTHER,
         v_HXMS,
         v_FCIMIAO,
         v_XRYND,
         v_CGSD,
         v_LXXZYTQ,
         v_BDG,
         v_SINGE_DOCTOR,
         v_SINGE_DOCTORID,
         v_CREATE_DOCTORID,
         to_char(sysdate, 'yyyy-mm-dd hh24:mi:ss'),
         '1');
    ELSE
      update nurse_recordoper re
         set re.NOOFINPATENT    = v_NOOFINPATENT,
             re.FATHER_RECORDID = v_FATHER_RECORDID,
             re.RECORD_DATETIME = v_RECORD_DATETIME,
             re.TIWEN           = v_TIWEN,
             re.MAIBO           = v_MAIBO,
             re.HUXI            = v_HUXI,
             re.XUEYA           = v_XUEYA,
             re.YISHI           = v_YISHI,
             re.XYBHD           = v_XYBHD,
             re.QKFL            = v_QKFL,
             re.SYPF            = v_SYPF,
             re.OTHER_ONE       = v_OTHER_ONE,
             re.OTHER_TWO       = v_OTHER_TWO,
             re.OTHER_THREE     = v_OTHER_THREE,
             re.OTHER_FOUR      = v_OTHER_FOUR,
             re.ZTKDX           = v_ZTKDX,
             re.YTKDX           = v_YTKDX,
             re.TKDGFS          = v_TKDGFS,
             re.WOWEI           = v_WOWEI,
             re.JMZG            = v_JMZG,
             re.DGJYLG_ONE      = v_DGJYLG_ONE,
             re.DGJYLG_TWO      = v_DGJYLG_TWO,
             re.DGJYLG_THREE    = v_DGJYLG_THREE,
             re.In_ITEM         = v_In_ITEM,
             re.In_VALUE        = v_In_VALUE,
             re.OUT_ITEM        = v_OUT_ITEM,
             re.OUT_VALUE       = v_OUT_VALUE,
             re.OUT_COLOR       = v_OUT_COLOR,
             re.OUT_STATUE      = v_OUT_STATUE,
             re.OTHER           = v_OTHER,
             re.HXMS            = v_HXMS,
             re.FCIMIAO         = v_FCIMIAO,
             re.XRYND           = v_XRYND,
             re.CGSD            = v_CGSD,
             re.LXXZYTQ         = v_LXXZYTQ,
             re.BDG             = v_BDG,
             re.SINGE_DOCTOR    = v_SINGE_DOCTOR,
             re.SINGE_DOCTORID  = v_SINGE_DOCTORID
       where re.id = v_ID;
    END IF;
  
  END;

  --删除记录单
  PROCEDURE usp_DelNurseRecord(v_NurseId IN varchar2) AS
  BEGIN
    update nurse_recordoper re set re.valid = '0' where re.id = v_NurseId;
  END;

  --查询某科室下所有的病人的护理数据 add by tj 2012-10-30 
  --此处字段的别名需和表原始的字段名称相同,字段代替使用时 需要天数据保存还配置天数据字段，需要时段数据保存还配置时段数据 切记！！！！！！
  --2013-03-20 病人查询1501和150中2种病人的查询
  procedure usp_GetPatientsOfDept(v_departCode  varchar2,
                                  v_wardcode    varchar2,
                                  v_Timelot     varchar2,
                                  v_TimelotSave varchar2,
                                  v_date        varchar2,
                                  o_result      OUT empcurtype) AS
  BEGIN
    open o_result for
      select patt.OUTBED          BED,
             patt.name            PNAME,
             patt.patid           PATID,
             patt.py              PY,
             patt.noofinpat,
             patt.inwarddate      inwarddate,
             tmp.id               ID,
             tmp.timeslot         TIMESLOT,
             tmp.temperature      temperature,
             tmp.wayofsurvey      wayofsurvey,
             tmp.name,
             tmp.pulse            PULSE,
             tmp.heartrate        HEARTRATE,
             tmp.breathe          BREATHE,
             tmp.physicalhotting  PHYSICALHOTTING,
             tmp.physicalcooling  PHYSICALCOOLING,
             tmp.countin          COUNTIN,
             tmp.timeofshit       TIMEOFSHIT,
             tmp.countout         COUNTOUT,
             tmp.drainage         DRAINAGE,
             tmp.bloodpressure    BLOODPRESSURE,
             tmp.paininfo         PAININFO,
             tmp.weight           WEIGHT,
             tmp.allergy          allergy,
             tmp.param1           param1,
             tmp.param2           param2,
             tmp.param3           param3,
             tmp.param4           param4,
             tmp.param5           param5,
             tmp.param6           param6,
             tmp.height           height,
             tmp.daysaftersurgery daysaftersurgery,
             tmp.dayofhospital    dayofhospital
        from inpatient patt
        left join (select tmp1.id,
                          tmp1.noofinpat,
                          tmp1.temperature,
                          tmp1.wayofsurvey,
                          tmp1.name,
                          tmp1.timeslot,
                          tmp1.dateofsurvey,
                          tmp1.pulse,
                          tmp1.heartrate,
                          tmp1.breathe,
                          tmp1.physicalhotting,
                          tmp1.physicalcooling,
                          tmp1.paininfo,
                          n.countin,
                          n.timeofshit,
                          n.countout,
                          n.drainage,
                          n.bloodpressure,
                          n.weight,
                          n.allergy,
                          n.param1,
                          n.param2,
                          n.param3,
                          n.param4,
                          n.param5,
                          n.param6,
                          n.height,
                          n.daysaftersurgery,
                          n.dayofhospital
                     from (select notesonnursing.id,
                                  notesonnursing.temperature,
                                  notesonnursing.wayofsurvey,
                                  notesonnursing.noofinpat,
                                  dictionary_detail.name,
                                  notesonnursing.timeslot,
                                  notesonnursing.dateofsurvey,
                                  notesonnursing.pulse, --脉搏
                                  notesonnursing.heartrate, --心率
                                  notesonnursing.breathe, --呼吸
                                  notesonnursing.physicalhotting, --物理升温
                                  notesonnursing.physicalcooling, --物理降温
                                  notesonnursing.paininfo
                             from notesonnursing
                             left join dictionary_detail
                               on dictionary_detail.detailid =
                                  notesonnursing.wayofsurvey
                            where dictionary_detail.categoryid = '88'
                              and notesonnursing.timeslot = v_Timelot
                              and notesonnursing.dateofsurvey = v_date) tmp1,
                          notesonnursing n
                    where n.noofinpat = tmp1.noofinpat
                      and n.timeslot = v_TimelotSave
                      and n.dateofsurvey = v_date) tmp
          on patt.noofinpat = tmp.noofinpat
       where patt.status in ('1501', '1500')
            --and patt.outhosdept =v_departCode --护士要更据病区来获取病人
         and patt.outhosward = v_wardcode;
  END;

  procedure usp_GetPatientsOfDept2(v_departCode  varchar2,
                                   v_wardcode    varchar2,
                                   v_Timelot     varchar2,
                                   v_TimelotSave varchar2,
                                   v_date        varchar2,
                                   o_result      OUT empcurtype,
                                   o_result1     OUT empcurtype,
                                   o_result2     OUT empcurtype)
  
   AS
  BEGIN
    open o_result for
      select patt.OUTBED     BED,
             patt.name       PNAME, --病人名称
             patt.patid      PATID, --住院号
             patt.py         PY, --拼音
             patt.noofinpat, --病人id
             patt.inwarddate inwarddate --病人入科日期
        from inpatient patt
       where patt.outhosward = v_wardcode
         and patt.status in (1500, 1501);
    open o_result1 for
      select patt.noofinpat, --病人id
             tmp.id ID, --体温单id
             tmp.timeslot TIMESLOT, --时间点
             tmp.temperature temperature, --体温
             tmp.wayofsurvey wayofsurvey, --测量方式id
             (select name
                from dictionary_detail
               where dictionary_detail.categoryid = '88'
                 and dictionary_detail.detailid = tmp.wayofsurvey) name, ----测量方式名
             
             tmp.pulse           PULSE, --脉搏
             tmp.heartrate       HEARTRATE, --心率
             tmp.breathe         BREATHE, --呼吸
             tmp.physicalhotting PHYSICALHOTTING, --物理升温
             tmp.physicalcooling PHYSICALCOOLING --物理降温
        from inpatient patt
        left join notesonnursing tmp
          on tmp.noofinpat = patt.noofinpat
       where tmp.timeslot = v_Timelot
         and tmp.dateofsurvey = v_date
         and patt.outhosward = v_wardcode
         and patt.status in (1500, 1501);
  
    open o_result2 for
    
      select inpat.noofinpat      noofinpat,
             tmp.countin          COUNTIN,
             tmp.timeofshit       TIMEOFSHIT,
             tmp.countout         COUNTOUT,
             tmp.drainage         DRAINAGE,
             tmp.bloodpressure    BLOODPRESSURE,
             tmp.paininfo         PAININFO,
             tmp.weight           WEIGHT,
             tmp.allergy          allergy,
             tmp.param1           param1,
             tmp.param2           param2,
             tmp.param3           param3,
             tmp.param4           param4,
             tmp.param5           param5,
             tmp.param6           param6,
             tmp.height           height,
             tmp.daysaftersurgery daysaftersurgery,
             tmp.dayofhospital    dayofhospital
        from inpatient inpat
        left join notesonnursing tmp
          on inpat.noofinpat = tmp.noofinpat
       where tmp.timeslot = v_TimelotSave
         and tmp.dateofsurvey = v_date
         and inpat.outhosward = v_wardcode
         and inpat.status in (1500, 1501);
  end;

end EMR_NURSE_STATION;
/
