CREATE OR REPLACE PACKAGE EMR_RECORD_INPUT
IS
--
-- To modify this template, edit file PKGSPEC.TXT in TEMPLATE
-- directory of SQL Navigator
--
-- Purpose: Briefly explain the functionality of the package
--
-- MODIFICATION HISTORY
-- Person      Date    Comments
-- ---------   ------  ------------------------------------------
   -- Enter package declarations as shown below
   TYPE empcurtyp IS REF CURSOR;

    --得到用户病历操作记录
   PROCEDURE usp_getoperrecordlog (
      v_user_id      VARCHAR,
      v_patient_id   VARCHAR,
      v_startDate  VARCHAR,
      v_endDate    VARCHAR,
      o_result     OUT empcurtyp
   );

      --插入病历操作记录
PROCEDURE usp_insertoperrecordlog (
      v_user_id          VARCHAR,
      v_user_name        VARCHAR,
      v_patient_id       VARCHAR,
      v_patient_name     VARCHAR,
      v_dept_id          VARCHAR,
      v_record_id        VARCHAR,
      v_record_type      VARCHAR,
      v_record_title     VARCHAR,
      v_oper_id          VARCHAR,
      v_oper_time        VARCHAR,
      v_oper_content     clob
   );

PROCEDURE usp_inserttemplateperson (
      v_templateid   VARCHAR,
      v_name         VARCHAR,
      v_userid       VARCHAR,
      v_content      CLOB,
      v_sortid       VARCHAR,
      v_memo         VARCHAR,
      v_py           VARCHAR,
      v_wb           VARCHAR,
      v_type         VARCHAR,
      v_ISCONFIGPAGESIZE VARCHAR,
      v_deptid       VARCHAR,
      o_result       OUT empcurtyp
   );

   PROCEDURE usp_insertthreelevelcheck (
      v_residentid     VARCHAR,
      v_residentname   VARCHAR,
      v_attendid       VARCHAR,
      v_attentname     VARCHAR,
      v_chiefid        VARCHAR,
      v_chiefname      VARCHAR,
      v_deptid         VARCHAR,
      v_deptname       VARCHAR,
      v_createuser     VARCHAR
   );

   --得到历史病人记录
   PROCEDURE usp_getHistoryInpatient (
      v_noofinpat      VARCHAR,
      v_admitbegindate VARCHAR,--add by xlb 2013-01-14
      v_admitenddate   VARCHAR,--add by xlb 2013-01-14
      o_result     OUT empcurtyp
   );

   --得到历史病人文件夹
   PROCEDURE usp_getHistoryInpatientFolder (
      v_noofinpat      VARCHAR,

      o_result     OUT empcurtyp
   );

   --得到住院次数（******进入我们电子病历系统的次数，而非实际的入院次数**********）
   PROCEDURE usp_getHistoryInCount (
     v_noofinpat       VARCHAR,
     o_result      OUT empcurtyp
   );

   --得到病人的基本信息
   PROCEDURE usp_getPatinetInfo(
     v_noofinpat       VARCHAR,
     o_result      OUT empcurtyp
   );

   --得到病人历史病历，用于历史病历导入
   PROCEDURE usp_getHistoryEMR(
     v_noofinpat        VARCHAR,
     v_sortid           VARCHAR,
     o_result      OUT empcurtyp
   );

   --得到病历内容
   PROCEDURE usp_EmrContentByID(
     v_recorddetailid   VARCHAR,
     o_result      OUT empcurtyp
   );
   
   /*插入转科记录 add by cyq 2013-03-26*/                  
   PROCEDURE usp_Inpatient_ChangeInfo(
                               v_id              INTEGER,
                               v_noofinpat       INTEGER,
                               v_newdeptid       VARCHAR2 default '',
                               v_newwardid       VARCHAR2 default '',
                               v_newbedid        VARCHAR2 default '',
                               v_olddeptid       VARCHAR2 default '',
                               v_oldwardid       VARCHAR2 default '',
                               v_oldbedid        VARCHAR2 default '',
                               v_changestyle     CHAR default '',
                               v_createuser      VARCHAR2 default '',
                               v_createtime      CHAR default '',
                               v_modifyuser      VARCHAR2 default '',
                               v_modifytime      CHAR default '',
                               v_valid           INTEGER);

   /*--新历史病历导入
   PROCEDURE usp_BatchInHistoryEMR(
     v_noofinpat       VARCHAR,
     v_templateid      VARCHAR,
     v_name            VARCHAR,
     v_sortid          VARCHAR,
     v_owner           VARCHAR,
     v_createtime      VARCHAR,
     v_captiondatetime VARCHAR,
     v_firstdailyflag  VARCHAR,
     v_isyihuangougong VARCHAR,
     v_ip              VARCHAR,
     v_isconfigpagesize VARCHAR,
     v_departcode      VARCHAR,
     v_wardcode        VARCHAR,
     v_changeid        VARCHAR
   );
   */
END;                                                           -- Package spec
/
CREATE OR REPLACE PACKAGE BODY EMR_RECORD_INPUT
IS
   --文书录入 “另存”保存的表
   PROCEDURE usp_inserttemplateperson (
      v_templateid   VARCHAR,
      v_name         VARCHAR,
      v_userid       VARCHAR,
      v_content      CLOB,
      v_sortid       VARCHAR,
      v_memo         VARCHAR,
      v_py           VARCHAR,
      v_wb           VARCHAR,
      v_type         VARCHAR,
      v_ISCONFIGPAGESIZE VARCHAR,
      v_deptid       VARCHAR,
      o_result       OUT empcurtyp
   )
   AS
   BEGIN
      INSERT INTO template_person
                  (ID, templateid, NAME,
                   userid, valid, content, sortid, sortmark, sharedid, memo,
                   py, wb, TYPE, ISCONFIGPAGESIZE, DEPTID
                  )
           VALUES (seq_template_person_id.NEXTVAL, v_templateid, v_name,
                   v_userid, '1', v_content, v_sortid, NULL, NULL, v_memo,
                   v_py, v_wb, v_type, v_ISCONFIGPAGESIZE, v_deptid
                  );

    OPEN o_result FOR
      SELECT seq_template_person_id.CURRVAL FROM DUAL;
   END;



   --得到用户病历操作记录
   PROCEDURE usp_getoperrecordlog (
      v_user_id      VARCHAR,
      v_patient_id   VARCHAR,
      v_startDate  VARCHAR,
      v_endDate    VARCHAR,
      o_result     OUT empcurtyp
   )
   AS
   BEGIN
    OPEN o_result FOR
        SELECT User_id,
               User_name,
               Patient_id,
               Patient_name,
               department.name Dept_name,
               Record_id,
               Record_title,
               Record_type,
               dictionary_detail.name as Oper_type,
               Oper_time,
               Oper_content
          FROM OperRecordLog LEFT JOIN dictionary_detail ON dictionary_detail.categoryid = 'cz' and
               dictionary_detail.detailid = OperRecordLog.Oper_Id
               LEFT JOIN department ON department.id = OperRecordLog.User_Dept_Id
          where OperRecordLog.User_Id = v_user_id
                and (OperRecordLog.Patient_Id = v_patient_id or v_patient_id is null or v_patient_id = '')
                and to_date(substr(nvl(trim(OperRecordLog.Oper_Time), '1990-01-01'), 1, 10),
                     'yyyy-mm-dd') >= to_date(v_startDate, 'yyyy-mm-dd')
                and to_date(substr(nvl(trim(OperRecordLog.Oper_Time), '1990-01-01'), 1, 10),
                     'yyyy-mm-dd') < to_date(v_endDate, 'yyyy-mm-dd')
        ORDER BY Record_id;
   END;

      --插入病历操作记录
PROCEDURE usp_insertoperrecordlog (
      v_user_id          VARCHAR,
      v_user_name        VARCHAR,
      v_patient_id       VARCHAR,
      v_patient_name     VARCHAR,
      v_dept_id          VARCHAR,
      v_record_id        VARCHAR,
      v_record_type      VARCHAR,
      v_record_title     VARCHAR,
      v_oper_id          VARCHAR,
      v_oper_time        VARCHAR,
      v_oper_content     clob
   )
   AS
   BEGIN
      INSERT INTO OperRecordLog
                  (user_id, user_name, patient_id,
                   patient_name, user_dept_id, record_id, record_type, record_title, oper_id, oper_time,
                   oper_content
                  )
           VALUES (v_user_id, v_user_name, v_patient_id,
                   v_patient_name, v_dept_id, v_record_id, v_record_type, v_record_title, v_oper_id,
                   v_oper_time, v_oper_content
                  );
   END;


         --插入住院病人登记信息
/*PROCEDURE usp_insertinhospatientinfo (
      v_name        VARCHAR,
      v_sex       VARCHAR,
      v_age     VARCHAR,
      v_mariage          VARCHAR,
      v_province        VARCHAR,
      v_county      VARCHAR,
      v_nation     VARCHAR,
      v_nationality          VARCHAR,
      v_idcard        VARCHAR,
      v_jobname     VARCHAR,
       v_organization        VARCHAR,
      v_officetel       VARCHAR,
      v_officepostalcode     VARCHAR,
      v_household          VARCHAR,
      v_tel        VARCHAR,
      v_postalcode      VARCHAR,
      v_address     VARCHAR,
      v_contactperson          VARCHAR,
      v_relationship        VARCHAR,
      v_contactaddress     VARCHAR,
       v_contacttel        VARCHAR,
      v_noofclinic       VARCHAR,
      v_noofrecord     VARCHAR,
      v_incount          VARCHAR,
      v_status        VARCHAR,
      v_criticallevel      VARCHAR,
      v_attendlevel     VARCHAR,
      v_pay          VARCHAR,
      v_origin        VARCHAR,
      v_admitway     VARCHAR,
      v_outway        VARCHAR,
      v_clinicdoctor       VARCHAR,
      v_resident     VARCHAR,
      v_attend          VARCHAR,
      v_chief        VARCHAR
   )
   AS
   BEGIN
         INSERT INTO iem_mainpage_basicinfo_2012
      (iem_mainpage_no,
       patnoofhis,
       noofinpat,
       payid,
       socialcare,
       incount,
       NAME,
       sexid,
       birth,
       marital,
       jobid,
       nationid,
       nationalityid,
       idno,
       ORGANIZATION,
       officeplace,
       officetel,
       officepost,
       CONTACTPERSON,
       RELATIONSHIP,
       CONTACTADDRESS,
       CONTACTTEL,
       admitdate,
       admitdept,
       admitward,
       trans_date,
       trans_admitdept,
       trans_admitward,
       outwarddate,
       outhosdept,
       outhosward,
       actualdays,
       pathology_diagnosis_name,
       pathology_observation_sn,
       allergic_drug,
       section_director,
       director,
       vs_employee_code,
       resident_employee_code,
       refresh_employee_code,
       DUTY_NURSE,
       interne,
       coding_user,
       medical_quality_id,
       quality_control_doctor,
       quality_control_nurse,
       quality_control_date,
       xay_sn,
       ct_sn,
       mri_sn,
       dsa_sn,
       bloodtype,
       rh,
       is_completed,
       completed_time,
       valide,
       create_user,
       create_time,
       MODIFIED_USER,
       MODIFIED_TIME,
       ZYMOSIS,
       HURT_TOXICOSIS_ELE_ID,
       HURT_TOXICOSIS_ELE_NAME,
       ZYMOSISSTATE,
       PATHOLOGY_DIAGNOSIS_ID,
       MONTHAGE,
       WEIGHT,
       INWEIGHT,
       INHOSTYPE,
       OUTHOSTYPE,
       RECEIVEHOSPITAL,
       RECEIVEHOSPITAL2,
       AGAININHOSPITAL,
       AGAININHOSPITALREASON,
       BEFOREHOSCOMADAY,
       BEFOREHOSCOMAHOUR,
       BEFOREHOSCOMAMINUTE,
       LATERHOSCOMADAY,
       LATERHOSCOMAHOUR,
       LATERHOSCOMAMINUTE,
       CARDNUMBER,
       ALLERGIC_FLAG,
       AUTOPSY_FLAG,
       CSD_PROVINCEID,
       CSD_CITYID,
       CSD_DISTRICTID,
       CSD_PROVINCENAME,
       CSD_CITYNAME,
       CSD_DISTRICTNAME,
       XZZ_PROVINCEID,
       XZZ_CITYID,
       XZZ_DISTRICTID,
       XZZ_PROVINCENAME,
       XZZ_CITYNAME,
       XZZ_DISTRICTNAME,
       XZZ_TEL,
       XZZ_POST,
       HKDZ_PROVINCEID,
       HKDZ_CITYID,
       HKDZ_DISTRICTID,
       HKDZ_PROVINCENAME,
       HKDZ_CITYNAME,
       HKDZ_DISTRICTNAME,
       HKDZ_POST,
       JG_PROVINCEID,
       JG_CITYID,
       JG_PROVINCENAME,
       JG_CITYNAME,
       AGE,
       ZG_FLAG,
       ADMITINFO,
       CSDADDRESS,
       JGADDRESS,
       XZZADDRESS,
       HKADDRESS,
       MENANDINHOP,
       INHOPANDOUTHOP,
       BEFOREOPEANDAFTEROPER,
       LINANDBINGLI,
       INHOPTHREE,
       FANGANDBINGLI,
       PATFLAG
       )
    VALUES
      (seq_iem_mainpage_basicinfo_2012_id.NEXTVAL,
       v_patnoofhis,
       -- numeric
       v_noofinpat, -- numeric
       v_payid, -- varchar(4)
       v_socialcare, -- varchar(32)
       v_incount, -- int
       v_name,
       -- varchar(60)
       v_sexid, -- varchar(4)
       v_birth, -- char(10)
       v_marital, -- varchar(4)
       v_jobid, -- varchar(4)
       v_provinceid,
       -- varchar(10)
       v_countyid, -- varchar(10)
       v_nationid, -- varchar(4)
       v_nationalityid, -- varchar(4)
       v_idno,
       -- varchar(18)
       v_organization, -- varchar(64)
       v_officeplace, -- varchar(64)
       v_officetel, -- varchar(16)
       v_officepost,
       -- varchar(16)
       v_nativeaddress, -- varchar(64)
       v_nativetel, -- varchar(16)
       v_nativepost, -- varchar(16)
       v_contactperson, -- varchar(32)
       v_relationship, -- varchar(4)
       v_contactaddress,
       -- varchar(255)
       v_contacttel, -- varchar(16)
       v_admitdate, -- varchar(19)
       v_admitdept, -- varchar(12)
       v_admitward,
       -- varchar(12)
       v_days_before, -- numeric
       v_trans_date, -- varchar(19)
       v_trans_admitdept,
       -- varchar(12)
       v_trans_admitward, -- varchar(12)
       v_trans_admitdept_again, -- varchar(12)
       v_outwarddate, -- varchar(19)
       v_outhosdept, -- varchar(12)
       v_outhosward, -- varchar(12)
       v_actual_days,
       -- numeric
       v_death_time, -- varchar(19)
       v_death_reason, -- varchar(300)
       v_admitinfo, -- varchar(4)
       v_in_check_date, -- varchar(19)
       v_pathology_diagnosis_name, -- varchar(300)
       v_pathology_observation_sn, -- varchar(60)
       v_ashes_diagnosis_name,
       -- varchar(300)
       v_ashes_anatomise_sn, -- varchar(60)
       v_allergic_drug, -- varchar(300)
       v_hbsag, -- numeric
       v_hcv_ab,
       -- numeric
       v_hiv_ab, -- numeric
       v_opd_ipd_id, -- numeric
       v_in_out_inpatinet_id, -- numeric
       v_before_after_or_id, -- numeric
       v_clinical_pathology_id, -- numeric
       v_pacs_pathology_id, -- numeric
       v_save_times, -- numeric
       v_success_times,
       -- numeric
       v_section_director, -- varchar(20)
       v_director, -- varchar(20)
       v_vs_employee_code,
       -- varchar(20)
       v_resident_employee_code, -- varchar(20)
       v_refresh_employee_code,
       -- varchar(20)
       v_master_interne, -- varchar(20)
       v_interne, -- varchar(20)
       v_coding_user, -- varchar(20)
       v_medical_quality_id, -- numeric
       v_quality_control_doctor,
       -- varchar(20)
       v_quality_control_nurse, -- varchar(20)
       v_quality_control_date,
       -- varchar(19)
       v_xay_sn, -- varchar(300)
       v_ct_sn, -- varchar(300)
       v_mri_sn, -- varchar(300)
       v_dsa_sn, -- varchar(300)
       v_is_first_case,
       -- numeric
       v_is_following, -- numeric
       v_following_ending_date, -- varchar(19)
       v_is_teaching_case, -- numeric
       v_blood_type_id, -- numeric
       v_rh, -- numeric
       v_blood_reaction_id, -- numeric
       v_blood_rbc, -- numeric
       v_blood_plt, -- numeric
       v_blood_plasma, -- numeric
       v_blood_wb, -- numeric
       v_blood_others, -- varchar(60)
       v_is_completed, -- varchar(1)
       v_completed_time, -- varchar(19)
       1, -- numeric
       v_create_user,
       -- varchar(10)
       TO_CHAR(SYSDATE, 'yyyy-mm-dd HH24:mi:ss') -- varchar(19)
       );
   END;*/


   PROCEDURE usp_insertthreelevelcheck (
      v_residentid     VARCHAR,
      v_residentname   VARCHAR,
      v_attendid       VARCHAR,
      v_attentname     VARCHAR,
      v_chiefid        VARCHAR,
      v_chiefname      VARCHAR,
      v_deptid         VARCHAR,
      v_deptname       VARCHAR,
      v_createuser     VARCHAR
   )
   AS
      v_count   INT;
   BEGIN
      SELECT COUNT (1)
        INTO v_count
        FROM three_level_check
       WHERE three_level_check.resident_id = v_residentid AND valid = '1';

      IF v_count > 0
      THEN
         /*
         UPDATE three_level_check
            SET valid = '0'
          WHERE three_level_check.resident_id = v_residentid and valid = '1';
         */
         DELETE FROM three_level_check
               WHERE three_level_check.resident_id = v_residentid
                 AND valid = '1';
      END IF;

      INSERT INTO three_level_check
                  (dept_id, dept_name, resident_id, resident_name,
                   attend_id, attend_name, chief_id, chief_name, create_time,
                   create_user, valid
                  )
           VALUES (v_deptid, v_deptname, v_residentid, v_residentname,
                   v_attendid, v_attentname, v_chiefid, v_chiefname, SYSDATE,
                   v_createuser, '1'
                  );
   END;


   --得到历史病人记录
   /** edit by cyq 2013-02-28
   PROCEDURE usp_getHistoryInpatient (
      v_noofinpat      VARCHAR,
      v_admitbegindate VARCHAR,--add by xlb 2013-01-14
      v_admitenddate   VARCHAR,--add by xlb 2013-01-14
      o_result     OUT empcurtyp
   )
   AS
   p_admitbegindate varchar(50);--add by xlb 2013-01-14
   p_admitenddate varchar(50);--add by xlb 2013-01-14
   BEGIN
      IF v_admitbegindate='' THEN --add by xlb 2013-01-14
       p_admitbegindate:='1000-01-01 00:00:00';--add by xlb 2013-01-14
      else
     p_admitbegindate :=v_admitbegindate|| ' 00:00:00';--add by xlb 2013-01-14 edit xlb 2013-02-26 时分秒前空格 否则起始时间和结束时间相同的记录查不出
     p_admitenddate   := v_admitenddate|| ' 23:59:59';--add by xlb 2013-01-14 edit xlb 2013-02-26


      END IF;

      IF v_admitenddate='' THEN--add by xlb 2013-01-14
      p_admitenddate :='2999-01-01 00:00:00';--add by xlb 2013-01-14

       END IF;
    OPEN o_result FOR
        SELECT noofinpat, inpatient.name, inpatient.admitdate, diagnosis.name diagname
          FROM inpatient LEFT OUTER JOIN diagnosis ON inpatient.admitdiagnosis = diagnosis.icd
         WHERE inpatient.noofclinic IN
            (
                SELECT noofclinic
                  FROM inpatient
                 WHERE inpatient.noofinpat = v_noofinpat --'7183'
                 AND noofclinic IS NOT NULL
            )
            AND inpatient.noofinpat <> v_noofinpat
             AND inpatient.admitdate>=p_admitbegindate--add by xlb 2013-01-14
                 AND inpatient.admitdate<=p_admitenddate--add by xlb 2013-01-14
        ORDER BY noofinpat;
   END;*/

   --获取历史病历 add by cyq 2013-02-28
   PROCEDURE usp_getHistoryInpatient (
      v_noofinpat      VARCHAR,
      v_admitbegindate VARCHAR,
      v_admitenddate   VARCHAR,
      o_result     OUT empcurtyp
   )AS
   v_sql VARCHAR(4000);
   BEGIN
     v_sql := 'SELECT inp.noofinpat, inp.name, inp.admitdate, dia.name diagname, inp.outhosdate
              FROM inpatient inp LEFT OUTER JOIN diagnosis dia ON inp.admitdiagnosis = dia.icd
              where inp.noofinpat in
              (
                  select distinct i.noofinpat from inpatient i inner JOIN recorddetail r on i.noofinpat=r.noofinpat
                  and i.noofclinic IN(SELECT noofclinic FROM inpatient WHERE noofinpat = '|| v_noofinpat || ' AND noofclinic IS NOT NULL)
                  and r.sortid not in(''AI'',''AJ'',''AK'')
              ) ';
     v_sql := v_sql || ' and inp.status in(1502,1503) and inp.noofinpat <> ' || v_noofinpat;
     if v_admitbegindate IS NOT NULL then
        v_sql := v_sql || ' and inp.admitdate >= ''' || v_admitbegindate || ' 00:00:00''';
     end if;
     if v_admitenddate IS NOT NULL then
        v_sql := v_sql || ' and inp.admitdate <= ''' || v_admitenddate || ' 23:59:59''';
     end if;
     v_sql := v_sql || ' ORDER BY inp.admitdate ';

     OPEN o_result FOR v_sql;
   END;


   --得到历史病人文件夹
   PROCEDURE usp_getHistoryInpatientFolder (
      v_noofinpat      VARCHAR,
      o_result     OUT empcurtyp
   )
   AS
   BEGIN
    OPEN o_result FOR
        SELECT   r.sortid, d.cname, d.mname, d.ccode, d.open_flag, d.utype
          FROM recorddetail r LEFT OUTER JOIN dict_catalog d ON d.ccode = r.sortid
         WHERE r.noofinpat = v_noofinpat and r.valid = '1'
        GROUP BY r.sortid, d.cname, d.mname, d.ccode, d.open_flag, d.utype
        ORDER BY r.sortid;
   END;


   --得到住院次数（******进入我们电子病历系统的次数，而非实际的入院次数**********）
   PROCEDURE usp_getHistoryInCount (
     v_noofinpat       VARCHAR,
     o_result      OUT empcurtyp
   )
   AS
   BEGIN
    OPEN o_result FOR
        SELECT inpatient.incount
          FROM inpatient
         WHERE inpatient.noofinpat =v_noofinpat; --'7183'
   END;


   --得到病人的基本信息
   PROCEDURE usp_getPatinetInfo(
     v_noofinpat       VARCHAR,
     o_result      OUT empcurtyp
   )
   AS
   BEGIN
    OPEN o_result FOR
        SELECT agestr age/*年龄*/, outbed || '床' bed, patid /*病历号*/,
               department.name deptname, ward.name wardname,inpatient.admitdate,inwarddate/*入区时间*/,
               inpatient.name patname, dd1.name sexname
          FROM inpatient
        left outer join department on department.id = inpatient.outhosdept and department.valid = '1'
        left outer join ward on ward.id = inpatient.outhosward and ward.valid = '1'
        left outer join dictionary_detail dd1 on dd1.detailid = inpatient.sexid and dd1.categoryid = '3'
         WHERE inpatient.noofinpat = v_noofinpat;
   END;

   --得到病人历史病历，用于历史病历导入
   PROCEDURE usp_getHistoryEMR(
     v_noofinpat        VARCHAR,
     v_sortid           VARCHAR,
     o_result      OUT empcurtyp
   )
   AS
   BEGIN
    OPEN o_result FOR
        SELECT emrtemplet.templet_id templateid, emrtemplet.mr_name,
               emrtemplet.mr_class SORTID,
               emrtemplet.new_page_flag, emrtemplet.file_name,
               emrtemplet.isshowfilename, emrtemplet.isyihuangoutong,
               emrtemplet.new_page_end,emrtemplet.isconfigpagesize,
               emrtemplet.isfirstdaily, emrtemplet.py, emrtemplet.wb,
               recorddetail.id, recorddetail.name,
               recorddetail.captiondatetime, '' content,
               inpatient.name, inpatient.admitdate
          FROM recorddetail, emrtemplet, inpatient
         WHERE recorddetail.templateid = emrtemplet.templet_id
           AND recorddetail.sortid = v_sortid
           AND recorddetail.noofinpat = v_noofinpat
           AND inpatient.noofinpat = v_noofinpat
           AND recorddetail.valid = '1'
      ORDER BY recorddetail.sortid, recorddetail.captiondatetime;
   END;

      --得到病历内容
   PROCEDURE usp_EmrContentByID(
     v_recorddetailid   VARCHAR,
     o_result   OUT empcurtyp
   )
   AS
   BEGIN
    OPEN o_result FOR
        SELECT recorddetail.content
        FROM recorddetail
        WHERE id = v_recorddetailid;
   END;
   
   /*插入转科记录 add by cyq 2013-03-26 */
  PROCEDURE usp_Inpatient_ChangeInfo(
                               v_id              INTEGER,
                               v_noofinpat       INTEGER,
                               v_newdeptid       VARCHAR2 default '',
                               v_newwardid       VARCHAR2 default '',
                               v_newbedid        VARCHAR2 default '',
                               v_olddeptid       VARCHAR2 default '',
                               v_oldwardid       VARCHAR2 default '',
                               v_oldbedid        VARCHAR2 default '',
                               v_changestyle     CHAR default '',
                               v_createuser      VARCHAR2 default '',
                               v_createtime      CHAR default '',
                               v_modifyuser      VARCHAR2 default '',
                               v_modifytime      CHAR default '',
                               v_valid           INTEGER) AS
    /**********
    [版本号] 1.0.0.0.0
    [创建时间] 2013-03-26
    [作者]   cyq
    [版权]
    [描述] 插入病人转科文件
    [结果集]返回插入的ID
    **********/
  BEGIN
      INSERT INTO inpatientchangeinfo
        (ID, noofinpat, newdeptid, newwardid, newbedid,
         olddeptid, oldwardid, oldbedid,
         changestyle, createuser, createtime,
         modifyuser, modifytime, valid)
      SELECT
         v_id, v_noofinpat, v_newdeptid, v_newwardid, v_newbedid,
         v_olddeptid, v_oldwardid, v_oldbedid, v_changestyle,
         v_createuser, v_createtime, v_modifyuser,
         v_modifytime, v_valid
      FROM dual
      /*WHERE NOT EXISTS(
        SELECT 1 FROM inpatientchangeinfo ici
        WHERE ici.noofinpat = v_noofinpat AND ici.newdeptid = v_newdeptid
        AND ici.newwardid = v_newwardid AND ici.createtime = v_createtime
        AND ici.valid = '1'
      )*/;
  END;
   
   /*--新历史病历导入
   PROCEDURE usp_BatchInHistoryEMR(
     v_noofinpat       VARCHAR,
     v_templateid      VARCHAR,
     v_name            VARCHAR,
     v_sortid          VARCHAR,
     v_owner           VARCHAR,
     v_createtime      VARCHAR,
     v_captiondatetime VARCHAR,
     v_firstdailyflag  VARCHAR,
     v_isyihuangougong VARCHAR,
     v_ip              VARCHAR,
     v_isconfigpagesize VARCHAR,
     v_departcode      VARCHAR,
     v_wardcode        VARCHAR,
     v_changeid        VARCHAR
   ) AS
   BEGIN
    INSERT INTO recorddetail(id, noofinpat, templateid, name, sortid, owner,
                            createtime, valid, hassubmit, hasprint, hassign,
                            captiondatetime, islock, firstdailyflag,
                            isyihuangoutong, ip, isconfigpagesize, departcode,
                            wardcode, changeid)
VALUES(seq_recorddetail_id.nextval, v_noofinpat, v_templateid, v_name, v_sortid, v_owner,
        v_createtime, '1', '4600', '3600', '0',
        v_captiondatetime, '4701', v_firstdailyflag,
        v_isyihuangougong, v_ip, v_isconfigpagesize, v_departcode,
        v_wardcode, v_changeid);
   END;*/
END;
/
