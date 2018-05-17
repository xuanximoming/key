CREATE OR REPLACE PACKAGE emr_consultation IS
  TYPE empcurtyp IS REF CURSOR;

  /*
  * 获取申请借阅病历
  */
  PROCEDURE usp_GetConsultationData(v_NoOfInpat        numeric default '0', --首页序号
                                    v_ConsultApplySn   int default 0, --会诊序号
                                    v_TypeID           numeric default 0, --类别
                                    v_Param1           varchar default '', --参数1
                                    v_ConsultTime      varchar default '',
                                    v_ConsultTypeID    int default 0,
                                    v_UrgencyTypeID    int default 0,
                                    v_Name             varchar default '',
                                    v_PatID            varchar default '',
                                    v_BedID            varchar default '',
                                    v_ConsultTimeBegin varchar default '',
                                    v_ConsultTimeEnd   varchar default '',
                                    v_DeptID           varchar default '',
                                    o_result           OUT empcurtyp,
                                    o_result1          OUT empcurtyp,
                                    o_result2          OUT empcurtyp);

  /*
  * 描述    (会诊申请界面)插入数据
  */
  PROCEDURE usp_InsertConsultationApply(
                                        -- Add the parameters for the stored procedure here
                                        v_typeid            INT DEFAULT 0,
                                        v_consultapplysn    INT DEFAULT 0,
                                        v_noofinpat         NUMERIC DEFAULT '0', --首页序号
                                        v_urgencytypeid     INT DEFAULT 0,
                                        v_consulttypeid     INT DEFAULT 0,
                                        v_abstract          VARCHAR DEFAULT '',
                                        v_purpose           VARCHAR DEFAULT '',
                                        v_applyuser         VARCHAR DEFAULT '',
                                        v_applytime         VARCHAR DEFAULT '',
                                        v_director          VARCHAR DEFAULT '',
                                        v_consulttime       VARCHAR DEFAULT '',
                                        v_consultlocation   VARCHAR DEFAULT '',
                                        v_stateid           INT DEFAULT 0,
                                        v_createuser        VARCHAR DEFAULT '',
                                        v_createtime        VARCHAR DEFAULT '',
                                        v_consultsuggestion VARCHAR DEFAULT '',
                                        v_finishtime        VARCHAR DEFAULT '',
                                        v_rejectreason      VARCHAR DEFAULT '',
                                        --V_mydept            VARCHAR DEFAULT '',
                                        v_APPLYDEPT         VARCHAR DEFAULT '',

                                        V_AuditUserID       VARCHAR DEFAULT '',
                                        v_AuditLevel        VARCHAR DEFAULT '',
                                        o_result            OUT empcurtyp);

  /*
  * 描述    插入ConsultApplyDepartment数据
  */
  PROCEDURE usp_InsertConsultationApplyD(
                                         -- Add the parameters for the stored procedure here
                                         v_consultapplysn  INT DEFAULT 0,
                                         v_ordervalue      INT DEFAULT 0,
                                         v_hospitalcode    VARCHAR DEFAULT '',
                                         v_departmentcode  VARCHAR DEFAULT '',
                                         v_departmentname  VARCHAR DEFAULT '',
                                         v_employeecode    VARCHAR DEFAULT '',
                                         v_employeename    VARCHAR DEFAULT '',
                                         v_employeelevelid VARCHAR DEFAULT '',
                                         v_createuser      VARCHAR DEFAULT '',
                                         v_createtime      VARCHAR DEFAULT '');
  /*
  * 描述    插入ConsultRecordDepartment数据
  */
  PROCEDURE usp_insertconsultationrecord(
                                         -- Add the parameters for the stored procedure here
                                         v_consultapplysn  INT DEFAULT 0,
                                         v_ordervalue      INT DEFAULT 0,
                                         v_hospitalcode    VARCHAR DEFAULT '',
                                         v_departmentcode  VARCHAR DEFAULT '',
                                         v_departmentname  VARCHAR DEFAULT '',
                                         v_employeecode    VARCHAR DEFAULT '',
                                         v_employeename    VARCHAR DEFAULT '',
                                         v_employeelevelid VARCHAR DEFAULT '',
                                         v_createuser      VARCHAR DEFAULT '',
                                         v_createtime      VARCHAR DEFAULT '');
  /*会诊审核更新状态*/
  PROCEDURE usp_updateconsultationdata(v_consultapplysn INT DEFAULT 0, --会诊号
                                       v_typeid         INT DEFAULT 0, --类型
                                       v_stateid        INT DEFAULT 0, --状态
                                       v_rejectreason   VARCHAR DEFAULT '' --审核意见
                                       );

  /*根据提供的会诊申请单编号获取会诊信息*/
  PROCEDURE usp_GetConsultationBySN(v_consultapplysn INT DEFAULT 0, --会诊号
                                    o_result         OUT empcurtyp);

  ---用于签到数据
  PROCEDURE usp_GetConsultUseSign(v_consultapplysn INT DEFAULT 0, --会诊号
                                  o_result         OUT empcurtyp);

  PROCEDURE usp_GetMessageInfo(v_userid VARCHAR DEFAULT '',
                               o_result OUT empcurtyp);

  ----医生工作站中获得会诊信息（全部符合条件的）
  PROCEDURE usp_GetMyConsultion(v_Deptids varchar, --部门编号
                                v_userid  varchar, --登录人编号
                                --v_seetype   varchar,--增加参数区别查询会诊信息的时间段是三天内的还是查询所有的会诊信息  add by ywk 2012年7月24日 14:59:14
                                o_result OUT empcurtyp);

  ----医生工作站中获得会诊信息（全部符合条件的）
  PROCEDURE usp_GetAllMyConsultion(v_Deptids varchar, --部门编号
                                   v_userid  varchar, --登录人编号
                                   v_seetype varchar, --增加参数区别查询会诊信息的时间段是三天内的还是查询所有的会诊信息  add by ywk 2012年7月24日 14:59:14
                                   o_result  OUT empcurtyp);
  ---根据条件获取所有会诊信息（质控办医务人员专用）
  PROCEDURE usp_GetAllConsultion(v_ApplyDeptid    varchar, --部门编号
                                 v_EmployeeDeptid varchar, --部门编号
                                 v_DateTimeBegin  varchar, --开始时间
                                 v_DateTimeEnd    varchar, --结束时间
                                 o_result         OUT empcurtyp);

  -------------------------------------------------会诊数据捞取 新需求  2012-11-07  add by tj -------------
  ----护士工作站中获得会诊信息 tj
  PROCEDURE usp_GetConsultionNurse(v_Deptids varchar, --部门编号
                                   o_result  OUT empcurtyp);

  ----医生工作站中获得会诊信息（全部符合条件的）  by tj
  PROCEDURE usp_GetConsultionDoctor(v_Deptids varchar, --部门编号
                                    v_userid  varchar, --登录人ID
                                    v_levelid varchar, ---登录人级别
                                    o_result  OUT empcurtyp);

  ----会诊提醒信息   by tj
  PROCEDURE usp_GetConsultionMessage(v_Deptids varchar, --部门编号
                                     v_levelid varchar, ---登录人级别
                                     o_result  OUT empcurtyp);

  --Add by wwj 2013-02-18 护士工作站的会诊数据捞取
  PROCEDURE usp_GetConsultionForNurse(v_Deptids varchar,      --部门编号
                                       v_ConsultDateFrom date, --会诊日期
                                       v_ConsultDateTo date,   --会诊日期
                                       v_ConsultState varchar, --会诊状态
                                       o_result OUT empcurtyp);


  --获得已会诊未缴费的记录 Add by wwj 2013-02-19
  PROCEDURE usp_GetAlreadyConsultNotFee(v_Deptids varchar, --部门编号
                                        o_result  OUT empcurtyp);

  --获取待审核的会诊记录 Add by wwj 2013-02-27
  PROCEDURE usp_GetUnAuditConsult(v_timefrom date, --会诊起始时间
                                  v_timeto date,   --会诊终止时间
                                  v_inpatientname varchar, --病人姓名
                                  v_patid  varchar, --病历号
                                  v_urgencyTypeID varchar, --紧急度
                                  v_currentuserid varchar, --当前系统登录人
                                  v_currentuserlevel varchar, --当前用户级别
                                  o_result OUT empcurtyp);

  --获取与系统登录人相关的待会诊、会诊记录保存的记录 Add by wwj 2013-02-27
  PROCEDURE usp_GetWaitConsult(v_timefrom date, --会诊起始时间
                               v_timeto date,   --会诊终止时间
                               v_inpatientname varchar, --病人姓名
                               v_patid  varchar, --病历号
                               v_urgencyTypeID varchar, --紧急度
                               v_bedCode varchar, --床位号
                               v_currentuserid varchar, --当前系统登录人
                               v_currentuserlevel varchar, --当前用户级别
                               o_result OUT empcurtyp);

  --会诊费用测试
  PROCEDURE usp_FeeTest(arg_inp varchar,
                        arg_name varchar,
                        arg_item varchar,
                        arg_itemname varchar,
                        arg_amount number,
                        arg_doct varchar,
                        arg_doctdept varchar,
                        arg_date date,
                        arg_dept varchar,
                        arg_hzdoct varchar,
                        arg_opercode varchar,
                        arg_operdate date,
                        return_code out number,
                        return_result out varchar);

    ---Add by xlb 2013-03-07
 PROCEDURE usp_SaveConsultRecord(
                                         v_TypeID          INT DEFAULT 0,
                                         v_ConsultId            INT DEFAULT 0,
                                         v_consultapplysn  INT DEFAULT 0,
                                         v_ordervalue      INT DEFAULT 0,
                                         v_hospitalcode    VARCHAR DEFAULT '',
                                         v_departmentcode  VARCHAR DEFAULT '',
                                         v_departmentname  VARCHAR DEFAULT '',
                                         v_employeecode    VARCHAR DEFAULT '',
                                         v_employeename    VARCHAR DEFAULT '',
                                         v_employeelevelid VARCHAR DEFAULT '',
                                         v_createuser      VARCHAR DEFAULT '',
                                         v_createtime      VARCHAR DEFAULT '',
                                         v_canceluser      VARCHAR DEFAULT '',
                                         v_modifyuser      VARCHAR DEFAULT '');
 --Add by xlb 2013-03-08
  PROCEDURE usp_SaveApplyDepartOrRecord(
                                         v_TypeId          INT DEFAULT 0,
                                         v_consultapplysn  INT DEFAULT 0,
                                         v_ordervalue      INT DEFAULT 0,
                                         v_hospitalcode    VARCHAR DEFAULT '',
                                         v_departmentcode  VARCHAR DEFAULT '',
                                         v_departmentname  VARCHAR DEFAULT '',
                                         v_employeecode    VARCHAR DEFAULT '',
                                         v_employeename    VARCHAR DEFAULT '',
                                         v_employeelevelid VARCHAR DEFAULT '',
                                         v_createuser      VARCHAR DEFAULT '',
                                         v_createtime      VARCHAR DEFAULT '');

END;
/
CREATE OR REPLACE PACKAGE BODY emr_consultation IS

  /*********************************************************************************/
  PROCEDURE usp_getconsultationdata(v_noofinpat        NUMERIC DEFAULT '0', --首页序号
                                    v_consultapplysn   INT DEFAULT 0, --会诊序号
                                    v_typeid           NUMERIC DEFAULT 0, --类别
                                    v_param1           VARCHAR DEFAULT '', --参数1
                                    v_consulttime      VARCHAR DEFAULT '',
                                    v_consulttypeid    INT DEFAULT 0,
                                    v_urgencytypeid    INT DEFAULT 0,
                                    v_name             VARCHAR DEFAULT '',
                                    v_patid            VARCHAR DEFAULT '',
                                    v_bedid            VARCHAR DEFAULT '',
                                    v_consulttimebegin VARCHAR DEFAULT '',
                                    v_consulttimeend   VARCHAR DEFAULT '',
                                    v_deptid           VARCHAR DEFAULT '',
                                    o_result           OUT empcurtyp,
                                    o_result1          OUT empcurtyp,
                                    o_result2          OUT empcurtyp) AS
    v_sql            VARCHAR(8000);
    v_where          VARCHAR(1000);
    v_consulttime_in VARCHAR(24);
  BEGIN
    OPEN o_result FOR
      SELECT '' FROM DUAL;

    OPEN o_result1 FOR
      SELECT '' FROM DUAL;

    OPEN o_result2 FOR
      SELECT '' FROM DUAL;

    v_consulttime_in := REPLACE(v_consulttime, '/', '-');

    IF v_typeid = '1' THEN
      --医院列表
      OPEN o_result FOR
        SELECT hi.ID AS ID, hi.NAME AS NAME
          FROM hospitalinfo hi
         ORDER BY hi.ID;
    END IF;

    IF v_typeid = '2' THEN
      --部门列表，需要根据医院来判断
      OPEN o_result FOR
        /*SELECT DISTINCT d.ID AS ID, d.NAME, d.py, d.wb
          FROM department d, dept2ward dw
         WHERE d.valid = '1'
           AND d.ID = dw.deptid*/---中心医院需求 edit by ywk
             select * from department  where sort in('101','102','104','105') and valid='1'
           AND hosno = v_param1
         ORDER BY  ID;
    END IF;

    IF v_typeid = '3' THEN
      --医生列表，需要根据部门来判断
      OPEN o_result FOR
        SELECT u.ID, u.NAME, u.py, u.wb
          FROM users u
         WHERE u.deptid = v_param1
           AND u.valid = '1'
         ORDER BY u.ID;
    END IF;

    IF v_typeid = '4' THEN
      --申请医师列表，需要根据当前病区来判断
      OPEN o_result FOR
        SELECT u.ID, u.NAME, u.py, u.wb
          FROM users u
         WHERE (u.wardid IS NOT NULL or u.wardid != '')
           AND u.WardID = v_Param1
           and u.valid = '1'
         ORDER BY u.ID;
    END IF;

    IF v_typeid = '5' THEN
      --申请医师科主任列表
      OPEN o_result FOR
        SELECT *
          FROM users u
         WHERE (u.wardid IS NOT NULL or u.wardid != '')
           AND /*u.WardID = v_Param1 and*/
               u.valid = '1'
           AND u.grade = '2000'
         ORDER BY u.ID;
    END IF;

    IF v_typeid = '6' THEN
      --根据类别从CategoryDetail表中捞取数据
      OPEN o_result FOR
        SELECT cd.ID, cd.NAME, cd.py, cd.wb
          FROM categorydetail cd
         WHERE cd.categoryid = v_param1
         ORDER BY cd.categoryid;
    END IF;

    IF v_typeid = '7' THEN
      --床位,需要根据当前病区来判断
      OPEN o_result FOR
        SELECT b.ID
          FROM bed b
         WHERE b.wardid = v_param1
           AND b.valid = '1';
    END IF;

    IF v_typeid = '8' THEN
      --得到所有临床医师
      OPEN o_result FOR
        SELECT u.ID AS employeecode,
               u.NAME AS employeename,
               u.ID||'_'||u.NAME AS employeenamestr,
               u.py,
               u.wb,
               u.deptid,
               d.NAME AS deptname,
               d.ID AS deptcode
          FROM users u
          LEFT OUTER JOIN department d ON d.ID = u.deptid
                                      AND d.valid = '1'
         WHERE (u.wardid IS NOT NULL or u.wardid != '')
           AND u.valid = '1'
         ORDER BY u.deptid, u.ID;
    END IF;

    IF v_typeid = '9' THEN
      --捞取医师资质(筛掉住院医师选项) '2003'
      OPEN o_result FOR
        SELECT cd.ID, cd.NAME, cd.py, cd.wb
          FROM categorydetail cd
         WHERE cd.categoryid = v_param1
           AND cd.ID IN ('2000', '2001', '2002')
         ORDER BY cd.categoryid;
    END IF;

    IF v_typeid = '20' THEN
      --捞取会诊申请的详细信息

      --(1)ConsultApply
      OPEN o_result FOR
        SELECT ca.consultapplysn,
               ca.noofinpat,
               ca.urgencytypeid,
               cd2.NAME AS urgencytypename,
               ca.consulttypeid,
               cd1.NAME AS consulttypename,
               ca.abstract,
               ca.purpose,
               ca.applyuser,
               u1.NAME AS applyusername,
               ca.applytime,
               ca.director,
               u2.NAME AS directorname,
               ca.consulttime,
               ca.consultlocation,
               ca.stateid,
               cd3.NAME AS statusname,
               ca.consultsuggestion,
               ca.finishtime,
               ca.rejectreason,
               ca.valid,
               ca.createuser,
               ca.createtime,
               ca.audituserid,--add by xlb 2013-03-13
               u3.name as AudioName --add by xlb 2013-03-13
          FROM consultapply ca
          LEFT OUTER JOIN categorydetail cd1 ON cd1.categoryid = '65'
                                            AND cd1.ID = ca.consulttypeid
          LEFT OUTER JOIN categorydetail cd2 ON cd2.categoryid = '66'
                                            AND cd2.ID = ca.urgencytypeid
          LEFT OUTER JOIN categorydetail cd3 ON cd3.categoryid = '67'
                                            AND cd3.ID = ca.stateid
          LEFT OUTER JOIN users u1 ON u1.ID = ca.applyuser
                                  AND u1.valid = '1'
          LEFT OUTER JOIN users u2 ON u2.ID = ca.director
                                  AND u2.valid = '1'
            LEFT OUTER JOIN users u3 ON u3.ID = ca.audituserid
                                  AND u3.valid = '1'
         WHERE ca.consultapplysn = v_consultapplysn
           AND ca.valid = '1';

      --(2)ConsultApplyDepartment
      OPEN o_result1 FOR
        SELECT cad.ID,
               cad.consultapplysn,
               cad.ordervalue,
               cad.hospitalcode,
               hi.NAME AS hospitalname,
               cad.departmentcode,
               CASE
                 WHEN cad.departmentcode = '' OR cad.departmentcode IS NULL THEN
                  cad.departmentname
                 ELSE
                  d.NAME
               END departmentname,
               cad.employeecode,
               cad.employeecode EmployeeID,
               cad.employeecode||'_'||(CASE
                 WHEN cad.employeecode = '' OR cad.employeecode IS NULL THEN
                  cad.employeename
                 ELSE
                  u.NAME
               END) employeenamestr,
               (CASE
                 WHEN cad.employeecode = '' OR cad.employeecode IS NULL THEN
                  cad.employeename
                 ELSE
                  u.NAME
               END) employeename,
               cad.employeelevelid,
               cd1.NAME AS employeelevelname,
               cad.createuser,
               cad.createtime,
              /* cad.issignin,
               (case when cad.issignin='' or cad.issignin is null or cad.issignin='0' then '未签到'
               when cad.issignin='1' then '已签到' end)signname,
               '删除' AS deletebutton,
               '签到' as signin,
               cad.reachtime*/

               '删除' AS deletebutton

          FROM consultapplydepartment cad
          LEFT OUTER JOIN hospitalinfo hi ON hi.ID = cad.hospitalcode
          LEFT OUTER JOIN department d ON d.ID = cad.departmentcode
                                      AND d.valid = '1'
          LEFT OUTER JOIN users u ON u.ID = cad.employeecode
                                 AND u.valid = '1'
          LEFT OUTER JOIN categorydetail cd1 ON cd1.categoryid = '20'
                                            AND cd1.ID =
                                                cad.employeelevelid
         WHERE cad.valid = '1'
           AND cad.consultapplysn IN
               (SELECT ca.consultapplysn
                  FROM consultapply ca
                 WHERE ca.consultapplysn = v_consultapplysn
                   AND ca.valid = '1');

      --(3)ConsultRecordDepartment
      OPEN o_result2 FOR
        SELECT cad.ID,
               cad.consultapplysn,
               cad.ordervalue,
               cad.hospitalcode,
               hi.NAME AS hospitalname,
               cad.departmentcode,
               CASE
                 WHEN cad.departmentcode = '' OR cad.departmentcode IS NULL THEN
                  cad.departmentname
                 ELSE
                  d.NAME
               END departmentname,
               cad.employeecode,
               cad.employeecode EmployeeID,
               cad.employeecode||'_'||(CASE
                 WHEN cad.employeecode = '' OR cad.employeecode IS NULL THEN
                  cad.employeename
                 ELSE
                  u.NAME
               END) employeenamestr,
               (CASE
                 WHEN cad.employeecode = '' OR cad.employeecode IS NULL THEN
                  cad.employeename
                 ELSE
                  u.NAME
               END) employeename,
               cad.employeelevelid,
               cd1.NAME AS employeelevelname,
               cad.createuser,
               '删除' AS deletebutton,
                 cad.issignin,
               (case when cad.issignin='' or cad.issignin is null or cad.issignin='0' then '未签到'
               when cad.issignin='1' then '已签到' end)signname,
               '删除' AS deletebutton,
               '签到' as signin,
                  cad.reachtime,
                  cad.createtime
          FROM consultrecorddepartment cad
          LEFT OUTER JOIN hospitalinfo hi ON hi.ID = cad.hospitalcode
          LEFT OUTER JOIN department d ON d.ID = cad.departmentcode
                                      AND d.valid = '1'
          LEFT OUTER JOIN users u ON u.ID = cad.employeecode
                                 AND u.valid = '1'
          LEFT OUTER JOIN categorydetail cd1 ON cd1.categoryid = '20'
                                            AND cd1.ID =
                                                cad.employeelevelid
         WHERE cad.valid = '1'
           AND cad.consultapplysn IN
               (SELECT ca.consultapplysn
                  FROM consultapply ca
                 WHERE ca.consultapplysn = v_consultapplysn
                   AND ca.valid = '1');
    END IF;

    IF v_typeid = '21' THEN
      --捞取符合条件的会诊申请信息(用于会诊审核列表)
      --把当前科室 = 申请人所在的科室 的都捞出来
      v_where := '';

      IF LTRIM(v_consulttime_in) != '' AND
         LTRIM(v_consulttime_in) IS NOT NULL THEN
        v_where := v_where || ' and ca.ConsultTime like ''' || '%' ||
                   RTRIM(v_consulttime_in) || '%' || '''';
      END IF;

      IF v_consulttypeid != '0' THEN
        v_where := v_where || ' and ca.ConsultTypeID=' ||
                   TO_CHAR(v_consulttypeid);
      END IF;

      IF v_urgencytypeid != '0' THEN
        v_where := v_where || ' and ca.UrgencyTypeID=' ||
                   TO_CHAR(v_urgencytypeid);
      END IF;

      IF v_name IS NOT NULL THEN
        v_where := v_where || ' and inp. Name  like ''' || '%' || v_name || '%' || '''';
      END IF;

      IF v_patid IS NOT NULL THEN
        v_where := v_where || ' and inp.PatID like ''' || '%' || v_patid || '%' || '''';
      END IF;

      IF v_bedid IS NOT NULL THEN
        v_where := v_where || ' and inp.OutBed=''' || v_bedid || '''';
      END IF;

      IF LTRIM(v_consulttimebegin) IS NOT NULL THEN
        v_where := v_where ||
                   ' and to_char(to_date(ca.ConsultTime, ''yyyy-mm-dd HH24:mi:ss''), ''yyyy-mm-dd'') >= ''' ||
                   v_consulttimebegin || '''';
      END IF;

      IF LTRIM(v_consulttimeend) IS NOT NULL THEN
        v_where := v_where ||
                   ' and to_char(to_date(ca.ConsultTime, ''yyyy-mm-dd HH24:mi:ss''), ''yyyy-mm-dd'') <= ''' ||
                   v_consulttimeend || '''';
      END IF;

      v_sql := 'select inp. Name  as PatientName, inp.PatID, inp.OutBed,
       ca.ConsultApplySn, ca.NoOfInpat, ca.UrgencyTypeID, cd2. Name  as UrgencyTypeName,
       ca.ConsultTypeID, cd1. Name  as ConsultTypeName, ca.Abstract, ca.Purpose,
       ca.ApplyUser, u1. Name  as ApplyUserName, ca.ApplyTime, ca.Director,
       u2. Name  as DirectorName, ca.ConsultTime, ca.ConsultLocation, ca.StateID,
       cd3. Name  as StatusName, ca.ConsultSuggestion, ca.FinishTime, ca.RejectReason,
       ca.Valid, ca.CreateUser, ca.CreateTime, d. Name  as DeptName,
       ca.audituserid
  from ConsultApply ca
  left outer join CategoryDetail cd1 on cd1.CategoryID = ''65'' and cd1.ID = ca.ConsultTypeID
  left outer join CategoryDetail cd2 on cd2.CategoryID = ''66'' and cd2.ID = ca.UrgencyTypeID
  left outer join CategoryDetail cd3 on cd3.CategoryID = ''67'' and cd3.ID = ca.StateID
  left outer join Users u1 on u1.ID = ca.ApplyUser and u1.Valid = ''1''
  left outer join Users u2 on u2.ID = ca.Director and u2.Valid = ''1''
  left outer join InPatient inp on inp.NoOfInpat = ca.NoOfInpat
  left outer join Department d on d.ID = u1.DeptId and d.Valid = ''1''
  where ca.Valid = ''1'' and ca.StateID = ''6720'' and u1.DeptId = ''' ||
               v_deptid || '''
  and inp. Status  in (''1500'',''1501'',''1502'',''1504'',''1505'',''1506'',''1507'')
  ';
      v_sql := v_sql || v_where;
      --print v_sql
      v_sql := v_sql || ' order by ApplyTime ';

      --dbms_output.put_line(v_sql);
      --open o_result for select v_sql from dual;
      OPEN o_result FOR v_sql;
    END IF;

    IF v_typeid = '22' THEN
      --捞取符合条件的会诊信息（用于会诊清单）
      --(1)把当前科室 = 申请人所在的科室 的都捞出来
      --(2)把当前科室 in 受邀科室 的都捞出来
      BEGIN
        v_where := '';

        IF LTRIM(v_consulttime_in) IS NOT NULL THEN
          v_where := v_where || ' and ca.ConsultTime like ''' || '%' ||
                     RTRIM(v_consulttime_in) || '%' || '''';
        END IF;

        IF v_consulttypeid != '0' THEN
          v_where := v_where || ' and ca.ConsultTypeID=' ||
                     TO_CHAR(v_consulttypeid);
        END IF;

        IF v_urgencytypeid != '0' THEN
          v_where := v_where || ' and ca.UrgencyTypeID=' ||
                     TO_CHAR(v_urgencytypeid);
        END IF;

        IF v_name IS NOT NULL THEN
          v_where := v_where || ' and inp. Name  like ''' || '%' || v_name || '%' || '''';
        END IF;

        IF v_patid IS NOT NULL THEN
          v_where := v_where || ' and inp.PatID like ''' || '%' || v_patid || '%' || '''';
        END IF;

        IF v_bedid IS NOT NULL THEN
          v_where := v_where || ' and inp.OutBed=''' || v_bedid || '''';
        END IF;

        IF LTRIM(v_consulttimebegin) IS NOT NULL THEN
          v_where := v_where ||
                     ' and to_char(to_date(ca.ConsultTime, ''yyyy-mm-dd HH24:mi:ss''), ''yyyy-mm-dd'') >= ''' ||
                     v_consulttimebegin || '''';
        END IF;

        IF LTRIM(v_consulttimeend) IS NOT NULL THEN
          v_where := v_where ||
                     ' and to_char(to_date(ca.ConsultTime, ''yyyy-mm-dd HH24:mi:ss''), ''yyyy-mm-dd'') <= ''' ||
                     v_consulttimeend || '''';
        END IF;
        --edit by wyt 2012-12-04 新增费用状态、签到状态、签到时间、受邀医师、会诊完成时间
       v_sql := 'select inp. Name  as PatientName, inp.PatID, inp.OutBed,
       ca.ConsultApplySn, ca.NoOfInpat, ca.UrgencyTypeID, cd2. Name  as UrgencyTypeName,
       ca.ConsultTypeID, cd1. Name  as ConsultTypeName, ca.Abstract, ca.Purpose,
       ca.ApplyUser, u1. Name  as ApplyUserName, ca.ApplyTime, ca.Director,
       u2. Name  as DirectorName, ca.ConsultTime, ca.ConsultLocation, ca.StateID,
       (case ca.ISPAY when 0 then ''未付款'' when 1 then ''已付款'' else ''未付款'' end) ISPAY,
       cd3. Name  as StatusName, ca.ConsultSuggestion, ca.FinishTime,
       ca.RejectReason,ca.Valid, ca.CreateUser, ca.CreateTime, cap.appdept,
       cap1.appname, crp.ISSIGNIN,crp.REACHTIME,d. Name  as DeptName
  from ConsultApply ca
  left join (select consultapplysn,
  wmsys.wm_concat(case ISSIGNIN when ''1'' then ''已签到'' when ''0'' then ''未签到''  else ''未签到'' end) ISSIGNIN,
  wm_concat(REACHTIME) REACHTIME from consultrecorddepartment where VALID = 1 group by consultapplysn) crp
  on ca.CONSULTAPPLYSN = crp.CONSULTAPPLYSN
  left join (select sn, wmsys.wm_concat(deptname) appdept from
  (select distinct consultapplysn sn, DEPARTMENTNAME deptname from  consultapplydepartment where VALID = 1) GROUP BY SN)
  cap on ca.CONSULTAPPLYSN = cap.sn
  left join (select consultapplysn,wmsys.wm_concat(EMPLOYEENAME) appname from consultapplydepartment
  where VALID = 1 group by consultapplysn) cap1 on ca.CONSULTAPPLYSN = cap1.CONSULTAPPLYSN
  left outer join CategoryDetail cd1 on cd1.CategoryID = ''65'' and cd1.ID = ca.ConsultTypeID
  left outer join CategoryDetail cd2 on cd2.CategoryID = ''66'' and cd2.ID = ca.UrgencyTypeID
  left outer join CategoryDetail cd3 on cd3.CategoryID = ''67'' and cd3.ID = ca.StateID
  left outer join Users u1 on u1.ID = ca.ApplyUser and u1.Valid = ''1''
  left outer join Users u2 on u2.ID = ca.Director and u2.Valid = ''1''
  left outer join InPatient inp on inp.NoOfInpat = ca.NoOfInpat
  left outer join Department d on d.ID = u1.DeptId and d.Valid = ''1''
  where ca.Valid = ''1'' and (u1.DeptId = ''' || v_deptid ||
                 ''' or
  exists
  (
    select *
    from ConsultApplyDepartment cad
    where cad.ConsultApplySn = ca.ConsultApplySn and cad.DepartmentCode = ''' ||
                 v_deptid || '''
  ))
  and exists
  (
    select *
    from ConsultApplyDepartment cad
    where cad.ConsultApplySn = ca.ConsultApplySn )
  and inp. Status  in (''1500'',''1501'',''1502'',''1504'',''1505'',''1506'',''1507'')
  ';
/*        v_sql := 'select inp. Name  as PatientName, inp.PatID, inp.OutBed,
       ca.ConsultApplySn, ca.NoOfInpat, ca.UrgencyTypeID, cd2. Name  as UrgencyTypeName,
       ca.ConsultTypeID, cd1. Name  as ConsultTypeName, ca.Abstract, ca.Purpose,
       ca.ApplyUser, u1. Name  as ApplyUserName, ca.ApplyTime, ca.Director,
       u2. Name  as DirectorName, ca.ConsultTime, ca.ConsultLocation, ca.StateID,
       cd3. Name  as StatusName, ca.ConsultSuggestion, ca.FinishTime, ca.RejectReason,
       ca.Valid, ca.CreateUser, ca.CreateTime, d. Name  as DeptName
  from ConsultApply ca
  left outer join CategoryDetail cd1 on cd1.CategoryID = ''65'' and cd1.ID = ca.ConsultTypeID
  left outer join CategoryDetail cd2 on cd2.CategoryID = ''66'' and cd2.ID = ca.UrgencyTypeID
  left outer join CategoryDetail cd3 on cd3.CategoryID = ''67'' and cd3.ID = ca.StateID
  left outer join Users u1 on u1.ID = ca.ApplyUser and u1.Valid = ''1''
  left outer join Users u2 on u2.ID = ca.Director and u2.Valid = ''1''
  left outer join InPatient inp on inp.NoOfInpat = ca.NoOfInpat
  left outer join Department d on d.ID = u1.DeptId and d.Valid = ''1''
  where ca.Valid = ''1'' and (u1.DeptId = ''' || v_deptid ||
                 ''' or
  exists
  (
    select *
    from ConsultApplyDepartment cad
    where cad.ConsultApplySn = ca.ConsultApplySn and cad.DepartmentCode = ''' ||
                 v_deptid || '''
  ))
  and inp. Status  in (''1500'',''1501'',''1502'',''1504'',''1505'',''1506'',''1507'')
  ';*/
        v_sql := v_sql || v_where;
        --print v_sql
        v_sql := v_sql || ' order by ConsultTime ';

        OPEN o_result FOR v_sql;
      END;
    END IF;

    IF v_typeid = '23' THEN
      --捞取符合条件的会诊信息（用于会诊记录清单）
      --对于一般会诊被邀请的科室能够看到，对于多科会诊申请会诊的科室能够看到
      BEGIN
        v_where := '';

        IF LTRIM(v_consulttime_in) IS NOT NULL THEN
          v_where := v_where || ' and ca.ConsultTime like ''' || '%' ||
                     RTRIM(v_consulttime_in) || '%' || '''';
        END IF;

        IF v_consulttypeid != '0' THEN
          v_where := v_where || ' and ca.ConsultTypeID=' ||
                     TO_CHAR(v_consulttypeid);
        END IF;

        IF v_urgencytypeid != '0' THEN
          v_where := v_where || ' and ca.UrgencyTypeID=' ||
                     TO_CHAR(v_urgencytypeid);
        END IF;

        IF v_name IS NOT NULL THEN
          v_where := v_where || ' and inp. Name  like ''' || '%' || v_name || '%' || '''';
        END IF;

        IF v_patid IS NOT NULL THEN
          v_where := v_where || ' and inp.PatID like ''' || '%' || v_patid || '%' || '''';
        END IF;

        IF v_bedid IS NOT NULL THEN
          v_where := v_where || ' and inp.OutBed=''' || v_bedid || '''';
        END IF;

        IF LTRIM(v_consulttimebegin) IS NOT NULL THEN
          v_where := v_where ||
                     ' and to_char(to_date(ca.ConsultTime, ''yyyy-mm-dd HH24:mi:ss''), ''yyyy-mm-dd'') >= ''' ||
                     v_consulttimebegin || '''';
        END IF;

        IF LTRIM(v_consulttimeend) IS NOT NULL THEN
          v_where := v_where ||
                     ' and to_char(to_date(ca.ConsultTime, ''yyyy-mm-dd HH24:mi:ss''), ''yyyy-mm-dd'') <= ''' ||
                     v_consulttimeend || '''';
        END IF;
        --edit by wyt 2012-12-04 新增费用状态、签到状态、签到时间、受邀医师、会诊完成时间
        v_sql := 'select inp. Name  as PatientName, inp.PatID, inp.OutBed,
       ca.ConsultApplySn, ca.NoOfInpat, ca.UrgencyTypeID, cd2. Name  as UrgencyTypeName,
       ca.ConsultTypeID, cd1. Name  as ConsultTypeName, ca.Abstract, ca.Purpose,
       ca.ApplyUser, u1. Name  as ApplyUserName, ca.ApplyTime, ca.Director,
       u2. Name  as DirectorName, ca.ConsultTime, ca.ConsultLocation, ca.StateID,
       cd3. Name  as StatusName, ca.ConsultSuggestion, ca.FinishTime, ca.RejectReason,
       (case ca.ISPAY when 0 then ''未付款'' when 1 then ''已付款'' else ''未付款'' end) ISPAY,
       ca.Valid, ca.CreateUser, ca.CreateTime, cap.appdept,cap1.appname,
       crp.ISSIGNIN,crp.REACHTIME, d. Name  as DeptName
  from ConsultApply ca
  left join (select consultapplysn,
  wmsys.wm_concat(case ISSIGNIN when ''1'' then ''已签到'' when ''0'' then ''未签到'' else ''未签到'' end) ISSIGNIN,
  wm_concat(REACHTIME) REACHTIME from consultrecorddepartment where VALID = 1 group by consultapplysn) crp
  on ca.CONSULTAPPLYSN = crp.CONSULTAPPLYSN
  left join (select sn, wmsys.wm_concat(deptname) appdept from
  (select distinct consultapplysn sn, DEPARTMENTNAME deptname from  consultapplydepartment where VALID = 1) GROUP BY SN)
  cap on ca.CONSULTAPPLYSN = cap.sn
  left join (select consultapplysn,wmsys.wm_concat(EMPLOYEENAME) appname from consultapplydepartment
  where VALID = 1 group by consultapplysn) cap1 on ca.CONSULTAPPLYSN = cap1.CONSULTAPPLYSN
  left outer join CategoryDetail cd1 on cd1.CategoryID = ''65'' and cd1.ID = ca.ConsultTypeID
  left outer join CategoryDetail cd2 on cd2.CategoryID = ''66'' and cd2.ID = ca.UrgencyTypeID
  left outer join CategoryDetail cd3 on cd3.CategoryID = ''67'' and cd3.ID = ca.StateID
  left outer join Users u1 on u1.ID = ca.ApplyUser and u1.Valid = ''1''
  left outer join Users u2 on u2.ID = ca.Director and u2.Valid = ''1''
  left outer join InPatient inp on inp.NoOfInpat = ca.NoOfInpat
  left outer join Department d on d.ID = u1.DeptId and d.Valid = ''1''
  where ca.Valid = ''1'' and ca.StateID in ( ''6730'', ''6740'' ) and
    ( exists
    (
      select 1
      from ConsultApplyDepartment cad
      where cad.ConsultApplySn = ca.ConsultApplySn and cad.DepartmentCode = ''' ||
                 v_deptid || '''
    ) or ca.applydept = ''' ||
                 v_deptid || ''' )
    /*and ca.ConsultTypeID = ''6501''*/
    and inp. Status  in (''1500'',''1501'',''1502'',''1504'',''1505'',''1506'',''1507'')
  ';
        v_sql := v_sql || v_where;
        v_sql := v_sql || ' union
  select inp. Name  as PatientName, inp.PatID, inp.OutBed,
       ca.ConsultApplySn, ca.NoOfInpat, ca.UrgencyTypeID, cd2. Name  as UrgencyTypeName,
       ca.ConsultTypeID, cd1. Name  as ConsultTypeName, ca.Abstract, ca.Purpose,
       ca.ApplyUser, u1. Name  as ApplyUserName, ca.ApplyTime, ca.Director,
       u2. Name  as DirectorName, ca.ConsultTime, ca.ConsultLocation, ca.StateID,
       cd3. Name  as StatusName, ca.ConsultSuggestion, ca.FinishTime, ca.RejectReason,
       (case ca.ISPAY when 0 then ''未付款'' when 1 then ''已付款'' else ''未付款'' end) ISPAY,
       ca.Valid, ca.CreateUser, ca.CreateTime, cap.appdept,cap1.appname,
       crp.ISSIGNIN,crp.REACHTIME,d. Name  as DeptName
  from ConsultApply ca
  left join (select consultapplysn,
  wmsys.wm_concat(case ISSIGNIN when ''1'' then ''已签到'' when ''0'' then ''未签到''  else ''未签到'' end) ISSIGNIN,
  wm_concat(REACHTIME) REACHTIME from consultrecorddepartment where VALID = 1 group by consultapplysn) crp
  on ca.CONSULTAPPLYSN = crp.CONSULTAPPLYSN
  left join (select sn, wmsys.wm_concat(deptname) appdept from
  (select distinct consultapplysn sn, DEPARTMENTNAME deptname from  consultapplydepartment where VALID = 1) GROUP BY SN)
  cap on ca.CONSULTAPPLYSN = cap.sn
  left join (select consultapplysn,wmsys.wm_concat(EMPLOYEENAME) appname from consultapplydepartment
  where VALID = 1 group by consultapplysn) cap1 on ca.CONSULTAPPLYSN = cap1.CONSULTAPPLYSN
  left outer join CategoryDetail cd1 on cd1.CategoryID = ''65'' and cd1.ID = ca.ConsultTypeID
  left outer join CategoryDetail cd2 on cd2.CategoryID = ''66'' and cd2.ID = ca.UrgencyTypeID
  left outer join CategoryDetail cd3 on cd3.CategoryID = ''67'' and cd3.ID = ca.StateID
  left outer join Users u1 on u1.ID = ca.ApplyUser and u1.Valid = ''1''
  left outer join Users u2 on u2.ID = ca.Director and u2.Valid = ''1''
  left outer join InPatient inp on inp.NoOfInpat = ca.NoOfInpat
  left outer join Department d on d.ID = u1.DeptId and d.Valid = ''1''
  where ca.Valid = ''1'' and ca.StateID in ( ''6730'', ''6740'' )
    and u1.DeptId = ''' || v_deptid || '''
    /*and ca.ConsultTypeID = ''6502''*/
    and inp. Status  in (''1501'',''1502'',''1504'',''1505'',''1506'',''1507'')';
/*        v_sql := 'select inp. Name  as PatientName, inp.PatID, inp.OutBed,
       ca.ConsultApplySn, ca.NoOfInpat, ca.UrgencyTypeID, cd2. Name  as UrgencyTypeName,
       ca.ConsultTypeID, cd1. Name  as ConsultTypeName, ca.Abstract, ca.Purpose,
       ca.ApplyUser, u1. Name  as ApplyUserName, ca.ApplyTime, ca.Director,
       u2. Name  as DirectorName, ca.ConsultTime, ca.ConsultLocation, ca.StateID,
       cd3. Name  as StatusName, ca.ConsultSuggestion, ca.FinishTime, ca.RejectReason,
       ca.Valid, ca.CreateUser, ca.CreateTime, d. Name  as DeptName
  from ConsultApply ca
  left outer join CategoryDetail cd1 on cd1.CategoryID = ''65'' and cd1.ID = ca.ConsultTypeID
  left outer join CategoryDetail cd2 on cd2.CategoryID = ''66'' and cd2.ID = ca.UrgencyTypeID
  left outer join CategoryDetail cd3 on cd3.CategoryID = ''67'' and cd3.ID = ca.StateID
  left outer join Users u1 on u1.ID = ca.ApplyUser and u1.Valid = ''1''
  left outer join Users u2 on u2.ID = ca.Director and u2.Valid = ''1''
  left outer join InPatient inp on inp.NoOfInpat = ca.NoOfInpat
  left outer join Department d on d.ID = u1.DeptId and d.Valid = ''1''
  where ca.Valid = ''1'' and ca.StateID in ( ''6730'', ''6740'' ) and
    ( exists
    (
      select 1
      from ConsultApplyDepartment cad
      where cad.ConsultApplySn = ca.ConsultApplySn and cad.DepartmentCode = ''' ||
                 v_deptid || '''
    ) or ca.applydept = ''' ||
                 v_deptid || ''' )
    \*and ca.ConsultTypeID = ''6501''*\
    and inp. Status  in (''1500'',''1501'',''1502'',''1504'',''1505'',''1506'',''1507'')
  ';
        v_sql := v_sql || v_where;
        v_sql := v_sql || ' union
  select inp. Name  as PatientName, inp.PatID, inp.OutBed,
       ca.ConsultApplySn, ca.NoOfInpat, ca.UrgencyTypeID, cd2. Name  as UrgencyTypeName,
       ca.ConsultTypeID, cd1. Name  as ConsultTypeName, ca.Abstract, ca.Purpose,
       ca.ApplyUser, u1. Name  as ApplyUserName, ca.ApplyTime, ca.Director,
       u2. Name  as DirectorName, ca.ConsultTime, ca.ConsultLocation, ca.StateID,
       cd3. Name  as StatusName, ca.ConsultSuggestion, ca.FinishTime, ca.RejectReason,
       ca.Valid, ca.CreateUser, ca.CreateTime, d. Name  as DeptName
  from ConsultApply ca
  left outer join CategoryDetail cd1 on cd1.CategoryID = ''65'' and cd1.ID = ca.ConsultTypeID
  left outer join CategoryDetail cd2 on cd2.CategoryID = ''66'' and cd2.ID = ca.UrgencyTypeID
  left outer join CategoryDetail cd3 on cd3.CategoryID = ''67'' and cd3.ID = ca.StateID
  left outer join Users u1 on u1.ID = ca.ApplyUser and u1.Valid = ''1''
  left outer join Users u2 on u2.ID = ca.Director and u2.Valid = ''1''
  left outer join InPatient inp on inp.NoOfInpat = ca.NoOfInpat
  left outer join Department d on d.ID = u1.DeptId and d.Valid = ''1''
  where ca.Valid = ''1'' and ca.StateID in ( ''6730'', ''6740'' )
    and u1.DeptId = ''' || v_deptid || '''
    \*and ca.ConsultTypeID = ''6502''*\
    and inp. Status  in (''1501'',''1502'',''1504'',''1505'',''1506'',''1507'')
  ';*/
        v_sql := v_sql || v_where;
        --print v_sql
        v_sql := v_sql || ' order by ConsultTime ';

        OPEN o_result FOR v_sql;
      END;
    END IF;
  END;

  /*********************************************************************************/
  PROCEDURE usp_InsertConsultationApply(
                                        -- Add the parameters for the stored procedure here
                                        v_typeid            INT DEFAULT 0,
                                        v_consultapplysn    INT DEFAULT 0,
                                        v_noofinpat         NUMERIC DEFAULT '0', --首页序号
                                        v_urgencytypeid     INT DEFAULT 0,
                                        v_consulttypeid     INT DEFAULT 0,
                                        v_abstract          VARCHAR DEFAULT '',
                                        v_purpose           VARCHAR DEFAULT '',
                                        v_applyuser         VARCHAR DEFAULT '',
                                        v_applytime         VARCHAR DEFAULT '',
                                        v_director          VARCHAR DEFAULT '',
                                        v_consulttime       VARCHAR DEFAULT '',
                                        v_consultlocation   VARCHAR DEFAULT '',
                                        v_stateid           INT DEFAULT 0,
                                        v_createuser        VARCHAR DEFAULT '',
                                        v_createtime        VARCHAR DEFAULT '',
                                        v_consultsuggestion VARCHAR DEFAULT '',
                                        v_finishtime        VARCHAR DEFAULT '',
                                        v_rejectreason      VARCHAR DEFAULT '',
                                       -- V_mydept            VARCHAR DEFAULT '',
                                        v_APPLYDEPT         VARCHAR DEFAULT '',
                                        V_AuditUserID       VARCHAR DEFAULT '',
                                        v_AuditLevel        VARCHAR DEFAULT '',
                                        o_result            OUT empcurtyp) AS
    --v_APPLYDEPT VARCHAR(12);
   v_ward      VARCHAR(12);
    v_bed       VARCHAR(12);
    --V_shenheuser   varchar(12);
   -- v_count integer default 0;
  BEGIN


    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    IF v_typeid = 1 THEN
      --(会诊申请界面)插入数据
      BEGIN
        -- Insert statements for procedure here
        --select inp.outhosdept applydept
         -- into v_APPLYDEPT
         -- from inpatient inp
         --where inp.noofinpat = v_noofinpat;


         /* if V_shenheuser is null then
            select parentuserid into V_shenheuser ---取上级
           from Consult_DoctorParent
          where userid=v_applyuser and valid=1;
          else
           select userid   into V_shenheuser --取科室负责人
           from Consult_DeptAutio
           where deptid=V_mydept and valid=1;
                end if;
*/
        -- v_count := 0;
        -- select count(1) into v_count from Consult_DoctorParent  where userid =v_applyuser and valid=1;
         --if v_count > 0 then
          -- select parentuserid into V_shenheuser ---取上级
          -- from Consult_DoctorParent
          --where userid=v_applyuser and valid=1;
        -- else
           --select count(1) into v_count from Consult_DeptAutio  where deptid=V_mydept and valid=1;
          -- if v_count > 0 then
           --  select nvl(userid, '')   into V_shenheuser --取科室负责人
            -- from Consult_DeptAutio
            -- where deptid=V_mydept and valid=1;
          -- end if;
        -- end if;

        select inp.outhosward
          into v_ward
         from inpatient inp
        where inp.noofinpat = v_noofinpat;

      select inp.outbed
         into v_bed
         from inpatient inp
         where inp.noofinpat = v_noofinpat;

        INSERT INTO consultapply
          (consultapplysn,
           -- this column value is auto-generated,
           noofinpat,
           urgencytypeid,
           consulttypeid,
           abstract,
           purpose,
           applyuser,
           applytime,
           director,
           consulttime,
           consultlocation,
           stateid,
           consultsuggestion,
           finishtime,
           rejectreason,
           createuser,
           createtime,
           modifieduser,
           modifiedtime,
           valid,
           canceluser,
           canceltime,
           APPLYDEPT,
           ward,
           bed,
           AuditUserID,
           auditlevel
           )
        VALUES
          (seq_consultapply_id.NEXTVAL,
           v_noofinpat,
           v_urgencytypeid,
           v_consulttypeid,
           v_abstract,
           v_purpose,
           v_applyuser,
           v_applytime,
           v_director,
           v_consulttime,
           v_consultlocation,
           v_stateid,
           v_consultsuggestion,
           '',
           v_rejectreason,
           v_createuser,
           v_createtime,
           NULL,
           NULL,
           '1',
           NULL,
           NULL,
           v_APPLYDEPT,--申请科室
           v_ward,--病区
           v_bed,--患者床号
           V_AuditUserID,
           v_AuditLevel
           );
      END;
    END IF;

    IF v_typeid = 2 THEN
      --（会诊申请界面）修改数据
      BEGIN
        UPDATE consultapply
           SET urgencytypeid   = v_urgencytypeid,
               consulttypeid   = consulttypeid,
               abstract        = v_abstract,
               purpose         = v_purpose,
               applyuser       = v_applyuser,
               applytime       = v_applytime,
               director        = v_director,
               consulttime     = v_consulttime,
               consultlocation = v_consultlocation,
               stateid         = v_stateid,
               modifieduser    = v_createuser,
               modifiedtime    = to_char(sysdate,'yyyy-mm-dd hh24:mi:ss'),
               AuditUserID     =V_AuditUserID,--add by xlb 2013-03-15
               auditlevel      =v_AuditLevel --add by xlb 2013-03-15
         WHERE consultapplysn = v_consultapplysn;

        UPDATE consultapplydepartment
           SET valid = '0'
         WHERE consultapplysn = v_consultapplysn;
      END;
    END IF;

    IF v_typeid = 3 THEN
      -- (会诊记录填写界面) 修改数据
      BEGIN
        UPDATE consultapply
           SET consultsuggestion = v_consultsuggestion,
               finishtime        = v_finishtime,
               stateid           = v_stateid
         WHERE consultapplysn = v_consultapplysn;

        UPDATE consultrecorddepartment
           SET valid = '0'
         WHERE consultapplysn = v_consultapplysn;
      END;
    END IF;

    OPEN o_result FOR
      SELECT ca.consultapplysn
        FROM consultapply ca
       WHERE ca.noofinpat = v_noofinpat
         AND ca.valid = '1'
       ORDER BY ca.consultapplysn DESC;
  END;

  /*********************************************************************************/
  PROCEDURE usp_InsertConsultationApplyD(
                                         -- Add the parameters for the stored procedure here
                                         v_consultapplysn  INT DEFAULT 0,
                                         v_ordervalue      INT DEFAULT 0,
                                         v_hospitalcode    VARCHAR DEFAULT '',
                                         v_departmentcode  VARCHAR DEFAULT '',
                                         v_departmentname  VARCHAR DEFAULT '',
                                         v_employeecode    VARCHAR DEFAULT '',
                                         v_employeename    VARCHAR DEFAULT '',
                                         v_employeelevelid VARCHAR DEFAULT '',
                                         v_createuser      VARCHAR DEFAULT '',
                                         v_createtime      VARCHAR DEFAULT '') AS
  BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    -- Insert statements for procedure here
    INSERT INTO consultapplydepartment
      (ID, -- this column value is auto-generated,
       consultapplysn,
       ordervalue,
       hospitalcode,
       departmentcode,
       departmentname,
       employeecode,
       employeename,
       employeelevelid,
       createuser,
       createtime,
       valid,
       canceluser,
       canceltime)
    VALUES
      (seq_consultapplydepartment_id.NEXTVAL,
       v_consultapplysn,
       v_ordervalue,
       v_hospitalcode,
       v_departmentcode,
       v_departmentname,
       v_employeecode,
       v_employeename,
       v_employeelevelid,
       v_createuser,
       v_createtime,
       '1',
       NULL,
       NULL);

        INSERT INTO consultrecorddepartment
      (ID, -- this column value is auto-generated,
       consultapplysn,
       ordervalue,
       hospitalcode,
       departmentcode,
       departmentname,
       employeecode,
       employeename,
       employeelevelid,
       createuser,
       createtime,
       valid,
       canceluser,
       canceltime)
    VALUES
      (seq_consultrecorddepartment_id.NEXTVAL,
       v_consultapplysn,
       v_ordervalue,
       v_hospitalcode,
       v_departmentcode,
       v_departmentname,
       v_employeecode,
       v_employeename,
       v_employeelevelid,
       v_createuser,
       v_createtime,
       '1',
       NULL,
       NULL);
  END;

  /*********************************************************************************/
  PROCEDURE usp_insertconsultationrecord(
                                         -- Add the parameters for the stored procedure here
                                         v_consultapplysn  INT DEFAULT 0,
                                         v_ordervalue      INT DEFAULT 0,
                                         v_hospitalcode    VARCHAR DEFAULT '',
                                         v_departmentcode  VARCHAR DEFAULT '',
                                         v_departmentname  VARCHAR DEFAULT '',
                                         v_employeecode    VARCHAR DEFAULT '',
                                         v_employeename    VARCHAR DEFAULT '',
                                         v_employeelevelid VARCHAR DEFAULT '',
                                         v_createuser      VARCHAR DEFAULT '',
                                         v_createtime      VARCHAR DEFAULT '') AS
  BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.

    -- Insert statements for procedure here
    INSERT INTO consultrecorddepartment
      (ID, -- this column value is auto-generated,
       consultapplysn,
       ordervalue,
       hospitalcode,
       departmentcode,
       departmentname,
       employeecode,
       employeename,
       employeelevelid,
       createuser,
       createtime,
       valid,
       canceluser,
       canceltime)
    VALUES
      (seq_consultrecorddepartment_id.NEXTVAL,
       v_consultapplysn,
       v_ordervalue,
       v_hospitalcode,
       v_departmentcode,
       v_departmentname,
       v_employeecode,
       v_employeename,
       v_employeelevelid,
       v_createuser,
       v_createtime,
       '1',
       NULL,
       NULL);
  END;

  /***************************************************************************************/
  /*会诊审核更新状态*/
  PROCEDURE usp_updateconsultationdata(v_consultapplysn INT DEFAULT 0, --会诊号
                                       v_typeid         INT DEFAULT 0,
                                       --类型
                                       v_stateid      INT DEFAULT 0, --状态
                                       v_rejectreason VARCHAR DEFAULT '' --审核意见
                                       ) AS
  BEGIN
    IF v_typeid = 1 THEN
      UPDATE consultapply
         SET stateid = v_stateid, rejectreason = v_rejectreason
       WHERE consultapplysn = v_consultapplysn
         AND valid = '1';
    END IF;
  END;

  /***************************************************************************************/
  /*根据提供的会诊申请单编号获取会诊信息*/
  PROCEDURE usp_GetConsultationBySN(v_consultapplysn INT DEFAULT 0, --会诊号
                                    o_result         OUT empcurtyp) as
  begin
    OPEN o_result FOR
      select apply.consultapplysn,
             apply.noofinpat,
             apply.urgencytypeid,
             urgencytype.name urgencytypeName,
             apply.consulttypeid,

             consulttype.name consulttypeName,
             apply.abstract,
             apply.purpose,
             apply.applydept ApplyDeptID,
             dept.name ApplyDeptName,

             apply.applyuser applyuserID,
             applyuser.name applyuserName,
             apply.applytime,
             apply.consultsuggestion,
             applydept.departmentcode ConsultDeptID,

             applydeptname.name ConsultDeptName,
             recorddept.hospitalcode,
             recorddept.departmentcode ConsultDeptID2,
             recorddeptName.Name ConsultDeptName2,
             recorddept.employeecode ConsultUserID,

             hos.name ConsultHospitalName,
             ConsultUser.Name ConsultUserName,
             apply.finishtime ConsultTime,
             apply.stateid
        from consultapply apply
        left join ConsultApplyDepartment applydept on apply.consultapplysn =
                                                      applydept.consultapplysn
                                                      and applydept.valid = 1
        left join consultrecorddepartment recordDept on recordDept.Consultapplysn =
                                                        apply.consultapplysn
                                                    and recordDept.Valid = 1
        left join categorydetail urgencytype on urgencytype.id =
                                                apply.urgencytypeid
                                            and urgencytype.categoryid = '66'
        left join categorydetail consulttype on consulttype.id =
                                                apply.consulttypeid
                                            and consulttype.categoryid = '65'
        left join department dept on apply.applydept = dept.id
        left join users applyuser on applyuser.id = apply.applyuser
        left join department applydeptname on applydeptname.id =
                                              applydept.departmentcode
        left join hospitalinfo hos on hos.id = recorddept.hospitalcode
        left join department recorddeptName on recorddeptName.Id =
                                               recorddept.departmentcode
        left join users ConsultUser on ConsultUser.Id =
                                       recorddept.employeecode
       where apply.consultapplysn = v_consultapplysn;
  end;


  PROCEDURE usp_GetMessageInfo(v_userid VARCHAR DEFAULT '',
                               o_result OUT empcurtyp) AS
  BEGIN
  OPEN o_result FOR
     SELECT id, typeid, decode(typeid, '1', '【会诊】','2','【取消会诊】', '') typename,
            '病人：' || painetname || ' ' || tcontent content
     FROM NURSE_WITHINFORMATION
     WHERE userid = v_userid AND valid = 1 AND typeid in (1, 2);
  END;



     ---用于签到数据
                                      PROCEDURE usp_GetConsultUseSign(v_consultapplysn INT DEFAULT 0, --会诊号
                                    o_result         OUT empcurtyp)as
                                   begin
              OPEN o_result FOR
            SELECT cad.ID,
               cad.consultapplysn,
               cad.ordervalue,
               cad.hospitalcode,
               hi.NAME AS hospitalname,
               cad.departmentcode,
               CASE
                 WHEN cad.departmentcode = '' OR cad.departmentcode IS NULL THEN
                  cad.departmentname
                 ELSE
                  d.NAME
               END departmentname,
               cad.employeecode,
               cad.employeecode EmployeeID,
               cad.employeecode||'_'||(CASE
                 WHEN cad.employeecode = '' OR cad.employeecode IS NULL THEN
                  cad.employeename
                 ELSE
                  u.NAME
               END) employeenamestr,
               (CASE
                 WHEN cad.employeecode = '' OR cad.employeecode IS NULL THEN
                  cad.employeename
                 ELSE
                  u.NAME
               END) employeename,
               cad.employeelevelid,
               cd1.NAME AS employeelevelname,
               cad.createuser,
               '删除' AS deletebutton,
                 nn.issignin,
               (case when nn.issignin='' or nn.issignin is null or nn.issignin='0' then '未签到'
               when nn.issignin='1' then '已签到' end)signname,
               '删除' AS deletebutton,
               '签到' as signin,
                  --cad.reachtime,
                  cad.createtime,
                  nn.reachtime
          FROM consultapplydepartment cad
          LEFT OUTER JOIN hospitalinfo hi ON hi.ID = cad.hospitalcode
          LEFT OUTER JOIN department d ON d.ID = cad.departmentcode
                                      AND d.valid = '1'
          LEFT OUTER JOIN users u ON u.ID = cad.employeecode
                                 AND u.valid = '1'
          LEFT OUTER JOIN categorydetail cd1 ON cd1.categoryid = '20'
                                            AND cd1.ID =
                                                cad.employeelevelid
         left join consultrecorddepartment nn on cad.consultapplysn=nn.consultapplysn and cad.employeecode=nn.employeecode
         WHERE cad.valid = '1'
           AND cad.consultapplysn IN
               (SELECT ca.consultapplysn
                  FROM consultapply ca
                 WHERE ca.consultapplysn = v_consultapplysn
                   AND ca.valid = '1');
                                     end;


   ----医生工作站中获得会诊信息（全部符合条件的）
 PROCEDURE usp_GetMyConsultion(v_Deptids     varchar, --部门编号
                               v_userid      varchar,--登录人编号
                                --v_seetype   varchar,--增加参数区别查询会诊信息的时间段是三天内的还是查询所有的会诊信息  add by ywk 2012年7月24日 14:59:14
                               o_result      OUT empcurtyp )as
  BEGIN


  --if v_seetype='1'then  --查询3天内
    OPEN o_result FOR
  SELECT ip.NAME AS inpatientname,
               ip.Py,ip.wb,
               TO_CHAR(TO_DATE(ca.consulttime, 'yyyy-mm-dd hh24:mi:ss'),
                       'yyyy-mm-dd hh24:mi:ss') AS consulttime,
               cd.NAME AS consultstatus,
               cd1.NAME AS urgencytype,
               ip.NAME || '_' || cd1.NAME || '_' || ca.consulttime AS inpatientinfo,
               ca.stateid,
                 u.name as applyusername,
                 aa.name as shouyaoren,
               ca.noofinpat,
               ca.modifieduser,
               ca.finishtime,
               ca.consulttypeid,
               ca.applyuser,
               ca.applydept,
                ca.audituserid,
                 ca.applytime,
                  (case
                    when ca.ispay = '1' then
                    '已缴费'
                    when ca.ispay = '0' then
                    '未缴费'
                    when ca.ispay is null then '未缴费'
                   end) MyPay,
                   ca.ispay,
               ca.consultapplysn,
               ca.printconsulttime,
               hh.departmentcode,  --受邀科室
               hh.employeelevelid, --受邀人级别
               hh.employeecode     --受邀人工号
               --hh.issignin         --是否签到
          FROM consultapply ca
          LEFT OUTER JOIN users u
            ON u.ID = ca.applyuser
           AND u.valid = '1'
          LEFT OUTER JOIN inpatient ip
            ON ip.noofinpat = ca.noofinpat
          LEFT OUTER JOIN categorydetail cd
            ON cd.categoryid = '67'
           AND ca.stateid = cd.ID
          LEFT OUTER JOIN categorydetail cd1
            ON cd1.categoryid = '66'
           AND cd1.ID = ca.urgencytypeid
          left outer JOIN consultapplydepartment hh
            on ca.consultapplysn = hh.consultapplysn AND hh.valid = '1'
          left outer join users aa on aa.id=hh.employeecode
         WHERE ca.valid = '1'
           and hh.departmentcode = v_Deptids
           AND ip.status IN ('1500', '1501', '1502', '1504', '1505', '1506', '1507')
 and ( stateid = '6730'
    or stateid = '6770'
    or stateid = '6720'
    or stateid = '6750'
    or stateid = '6740'
    or stateid = '6741'
   and to_date(finishtime, 'yyyy-mm-dd hh24:mi:ss') > trunc(sysdate) - 3);
--end  if;

/*if v_seetype='2' then --查询所有的，不加时间限制范围
     OPEN o_result FOR
  SELECT ip.NAME AS inpatientname,
               TO_CHAR(TO_DATE(ca.consulttime, 'yyyy-mm-dd hh24:mi:ss'),
                       'yyyy-mm-dd hh24:mi:ss') AS consulttime,
               cd.NAME AS consultstatus,
               cd1.NAME AS urgencytype,
               ip.NAME || '_' || cd1.NAME || '_' || ca.consulttime AS inpatientinfo,
               ca.stateid,
                 u.name as applyusername,
                 aa.name as shouyaoren,
               ca.noofinpat,
               ca.modifieduser,
               ca.finishtime,
               ca.consulttypeid,
               ca.applyuser,
               ca.applydept,
                ca.audituserid,
                 ca.applytime,
                  (case
                    when ca.ispay = '1' then
                    '已缴费'
                    when ca.ispay = '0' then
                    '未缴费'
                    when ca.ispay is null then '未缴费'
                   end) MyPay,
                   ca.ispay,
               ca.consultapplysn,
               hh.departmentcode,  --受邀科室
               hh.employeelevelid, --受邀人级别
               hh.employeecode     --受邀人工号
               --hh.issignin         --是否签到
          FROM consultapply ca
          LEFT OUTER JOIN users u
            ON u.ID = ca.applyuser
           AND u.valid = '1'
          LEFT OUTER JOIN inpatient ip
            ON ip.noofinpat = ca.noofinpat
          LEFT OUTER JOIN categorydetail cd
            ON cd.categoryid = '67'
           AND ca.stateid = cd.ID
          LEFT OUTER JOIN categorydetail cd1
            ON cd1.categoryid = '66'
           AND cd1.ID = ca.urgencytypeid
          left outer JOIN consultapplydepartment hh
            on ca.consultapplysn = hh.consultapplysn AND hh.valid = '1'
          left outer join users aa on aa.id=hh.employeecode
         WHERE ca.valid = '1'
           and hh.departmentcode = v_Deptids
           AND ip.status IN ('1500', '1501', '1502', '1504', '1505', '1506', '1507')
 and ( stateid = '6730'
    or stateid = '6770'
    or stateid = '6720'
    or stateid = '6750'
    or stateid = '6740'
    or stateid = '6741');
end  if;*/
  END;


  ----医生工作站中获得会诊信息（全部符合条件的）
 PROCEDURE usp_GetAllMyConsultion(v_Deptids     varchar, --部门编号
                               v_userid      varchar,--登录人编号
                                v_seetype   varchar,--增加参数区别查询会诊信息的时间段是三天内的还是查询所有的会诊信息  add by ywk 2012年7月24日 14:59:14
                               o_result      OUT empcurtyp )as
  BEGIN
    OPEN o_result FOR




   SELECT ip.NAME AS inpatientname,
               ip.Py,ip.wb,
               TO_CHAR(TO_DATE(ca.consulttime, 'yyyy-mm-dd hh24:mi:ss'),
                       'yyyy-mm-dd hh24:mi:ss') AS consulttime,
               cd.NAME AS consultstatus,
               cd1.NAME AS urgencytype,
               ip.NAME || '_' || cd1.NAME || '_' || ca.consulttime AS inpatientinfo,
               ca.stateid,
                 u.name as applyusername,
                 aa.name as shouyaoren,
               ca.noofinpat,
               ca.modifieduser,
               ca.finishtime,
               ca.consulttypeid,
               ca.applyuser,
               ca.applydept,
               ca.audituserid,
               ca.applytime,
               (case
                    when ca.ispay = '1' then
                    '已缴费'
                    when ca.ispay = '0' then
                    '未缴费'
                     when ca.ispay is null then '未缴费'
                   end) MyPay,
                   ca.ispay,
               ca.consultapplysn,
                 ca.printconsulttime,
               hh.departmentcode,  --受邀科室
               hh.employeelevelid, --受邀人级别
               hh.employeecode   --受邀人工号
              -- hh.issignin         --是否签到
          FROM consultapply ca
          LEFT OUTER JOIN users u
            ON u.ID = ca.applyuser
          LEFT OUTER JOIN inpatient ip
            ON ip.noofinpat = ca.noofinpat
          LEFT OUTER JOIN categorydetail cd
            ON cd.categoryid = '67'
           AND ca.stateid = cd.ID
              left outer JOIN consultapplydepartment hh
            on ca.consultapplysn = hh.consultapplysn AND hh.valid = '1'
              left outer join users aa on aa.id=hh.employeecode
          LEFT OUTER JOIN categorydetail cd1
            ON cd1.categoryid = '66'
           AND cd1.ID = ca.urgencytypeid
         WHERE ca.valid = '1'
           AND ip.status IN ('1500','1501', '1502', '1504', '1505', '1506', '1507')
       and ( stateid = '6730'
    or stateid = '6770'
   or stateid = '6720'
    or stateid = '6750'
    or stateid = '6740'
    or stateid = '6741') --and to_date(finishtime, 'yyyy-mm-dd hh24:mi:ss') > trunc(sysdate) - 3)
    /*and u.id=V_userid*/
      and ( u.id=V_userid or ca.applydept=v_Deptids )
union
SELECT ip.NAME AS inpatientname,
               ip.Py,ip.wb,
               TO_CHAR(TO_DATE(ca.consulttime, 'yyyy-mm-dd hh24:mi:ss'),
                       'yyyy-mm-dd hh24:mi:ss') AS consulttime,
               cd.NAME AS consultstatus,
               cd1.NAME AS urgencytype,
               ip.NAME || '_' || cd1.NAME || '_' || ca.consulttime AS inpatientinfo,
               ca.stateid,
                 u.name as applyusername,
                 aa.name as shouyaoren,
               ca.noofinpat,
               ca.modifieduser,
               ca.finishtime,
               ca.consulttypeid,
               ca.applyuser,
               ca.applydept,
                ca.audituserid,
                 ca.applytime,
                  (case
                    when ca.ispay = '1' then
                    '已缴费'
                    when ca.ispay = '0' then
                    '未缴费'
                    when ca.ispay is null then '未缴费'
                   end) MyPay,
                   ca.ispay,
               ca.consultapplysn,
               ca.printconsulttime,
               hh.departmentcode,  --受邀科室
               hh.employeelevelid, --受邀人级别
               hh.employeecode     --受邀人工号
               --hh.issignin         --是否签到
          FROM consultapply ca
          LEFT OUTER JOIN users u
            ON u.ID = ca.applyuser
           AND u.valid = '1'
          LEFT OUTER JOIN inpatient ip
            ON ip.noofinpat = ca.noofinpat
          LEFT OUTER JOIN categorydetail cd
            ON cd.categoryid = '67'
           AND ca.stateid = cd.ID
          LEFT OUTER JOIN categorydetail cd1
            ON cd1.categoryid = '66'
           AND cd1.ID = ca.urgencytypeid
          left outer JOIN consultapplydepartment hh
            on ca.consultapplysn = hh.consultapplysn AND hh.valid = '1'
          left outer join users aa on aa.id=hh.employeecode
         WHERE ca.valid = '1'
           and hh.departmentcode = v_Deptids
           AND ip.status IN ('1500', '1501', '1502', '1504', '1505', '1506', '1507')
 and ( stateid = '6730'
    or stateid = '6770'
    or stateid = '6720'
    or stateid = '6750'
    or stateid = '6740'
    or stateid = '6741')
   --and to_date(finishtime, 'yyyy-mm-dd hh24:mi:ss') > trunc(sysdate) - 3)
   and (
   hh.employeecode=V_userid or (hh.employeecode is null));-- or hh.employeelevelid=u.grade));



  END;




---根据条件获取所有会诊信息（质控办医务人员专用）--add by wyt 2012-09-07
 PROCEDURE usp_GetAllConsultion(v_ApplyDeptid     varchar, --部门编号
                                v_EmployeeDeptid     varchar, --部门编号
                                v_DateTimeBegin varchar,--开始时间
                                v_DateTimeEnd varchar, --结束时间
                               o_result      OUT empcurtyp )as
                                p_beginDate VARCHAR2(19);
                                p_beginEnd   VARCHAR2(19);
  BEGIN
    p_beginDate :=v_DateTimeBegin || ' 00:00:00';--Add by xlb 2013-06-28 当天数据检索
    p_beginEnd :=v_DateTimeEnd || '23:59:59';
    OPEN o_result FOR
    
    select inp. Name  as INPATIENTNAME, inp.PatID as PATID, inp.OutBed BED,
               ca.ConsultApplySn CONSULTAPPLYSN, ca.NoOfInpat as NOOFINPAT, ca.UrgencyTypeID, cd2.Name as  URGENCYTYPE,
               ca.ConsultTypeID, cd1.Name as ConsultTypeName, ca.Abstract as ABSTRACT, ca.Purpose as PURPOSE,
               ca.ApplyUser as APPLYUSER, u1.Name as  APPLYUSERNAME, ca.ApplyTime APPLYTIME, ca.Director,ca.consultlocation as CONSULTLOCATION,
               u2.Name as DirectorName, ca.ConsultTime as PLANCONSULTTIME, ca.ConsultLocation, ca.StateID,
               cd3.Name  as CONSULTSTATUS, ca.ConsultSuggestion, ca.FinishTime as FINISHTIME, ca.RejectReason,
               decode(ca.ispay, '1', '已付款', '未付款') MYPAY,ca.ispay,
               ca.Valid, ca.CreateUser, ca.CreateTime, crp.DEPARTMENTNAME, crp.EMPLOYEENAME,
               crp.ISSIGNIN, crp.REACHTIME, d.Name as APPLYDEPTNAME 
          from ConsultApply ca
          left join (select consultapplysn, wmsys.wm_concat(decode(ISSIGNIN, '1', '已签到', '未签到')) ISSIGNIN,
           wmsys.wm_concat(departmentname) DEPARTMENTNAME,wmsys.wm_concat(consultrecorddepartment.employeename) EMPLOYEENAME,
            wm_concat(REACHTIME) REACHTIME from consultrecorddepartment where VALID = 1  
            and  (departmentcode=v_EmployeeDeptid  or v_EmployeeDeptid = '0000' or v_EmployeeDeptid= '' or v_EmployeeDeptid is null) 
             group by consultapplysn) crp
                on ca.CONSULTAPPLYSN = crp.CONSULTAPPLYSN
          left outer join CategoryDetail cd1 on cd1.CategoryID = '65' and cd1.ID = ca.ConsultTypeID
          left outer join CategoryDetail cd2 on cd2.CategoryID = '66' and cd2.ID = ca.UrgencyTypeID
          left outer join CategoryDetail cd3 on cd3.CategoryID = '67' and cd3.ID = ca.StateID
          left outer join Users u1 on u1.ID = ca.ApplyUser and u1.Valid = '1'
          left outer join Users u2 on u2.ID = ca.Director and u2.Valid = '1'
          left outer join InPatient inp on inp.NoOfInpat = ca.NoOfInpat
          left outer join Department d on d.ID = u1.DeptId and d.Valid = '1'
         where (ca.applydept=v_ApplyDeptid or v_ApplyDeptid = '0000' or v_ApplyDeptid = '' or v_ApplyDeptid is null)
        and to_date(substr(nvl(trim(ca.consulttime), '1000-01-01 00:00:00'), 1, 19),
                     'yyyy-mm-dd hh24:mi:ss') > to_date(p_beginDate, 'yyyy-mm-dd hh24:mi:ss')
         and to_date(substr(nvl(trim(ca.consulttime), '9990-01-01 23:59:59'), 1, 19),
                     'yyyy-mm-dd hh24:mi:ss') < to_date(p_beginEnd, 'yyyy-mm-dd hh24:mi:ss')
            and ca.Valid = '1';
    
/*select A.* from (
   SELECT  distinct ip.NAME AS inpatientname,
               ip.Py,ip.wb,
               ip.patid,
                      TO_CHAR(TO_DATE(cf.reachtime, 'yyyy-mm-dd hh24:mi:ss'),
                       'yyyy-mm-dd hh24:mi:ss') AS consulttime,

               cd.NAME AS consultstatus,
               cd1.NAME AS urgencytype,
               ip.NAME || '_' || cd1.NAME || '_' || ca.consulttime AS inpatientinfo,
               ca.stateid,
               u.name as applyusername,
               aa.name as shouyaoren,
               ca.noofinpat,
               ca.modifieduser,
               ca.consulttypeid,
               ca.applyuser,
               ca.applydept,
               ca.finishtime,
               dept.name applydeptname,
               ca.audituserid,
               ca.applytime,
               (case
                    when ca.ispay = '1' then
                    '已缴费'
                    when ca.ispay = '0' then
                    '未缴费'
                     when ca.ispay is null then '未缴费'
                   end) MyPay,
               ca.ispay,
               to_char(to_date(ca.consulttime,'yyyy-mm-dd hh24:mi:ss'),'yyyy-mm-dd hh24:mi')  planconsulttime,
               ca.consultapplysn,
               ca.bed,
               ca.consultlocation,
               ca.abstract,
               ca.purpose,
               hh.departmentcode,  --受邀科室
               hh.departmentname,--受邀科室
               hh.employeelevelid, --受邀人级别
               hh.employeecode,   --受邀人工号
               (case
                    when hh.employeename = '' or hh.employeename is null then
                    '-'
                    else hh.employeename
                end) employeename,
                  cf.reachtime
                --hh.employeename
                    --受邀人名称
              -- hh.issignin         --是否签到
          FROM consultapply ca
          LEFT OUTER JOIN users u
            ON u.ID = ca.applyuser
            AND u.valid = 1
          LEFT OUTER JOIN inpatient ip
            ON ip.noofinpat = ca.noofinpat
           left join department dept
           on dept.id = ca.applydept
          LEFT OUTER JOIN categorydetail cd
            ON cd.categoryid = '67'
           AND ca.stateid = cd.ID
              left outer JOIN consultapplydepartment hh
            on ca.consultapplysn = hh.consultapplysn AND hh.valid = '1'

            left join consultrecorddepartment cf on cf.consultapplysn=ca.consultapplysn and cf.valid='1'


              left outer join users aa on aa.id=hh.employeecode
              and aa.valid = 1
          LEFT OUTER JOIN categorydetail cd1
            ON cd1.categoryid = '66'
           AND cd1.ID = ca.urgencytypeid

         WHERE ca.valid = '1'
         and to_date(substr(nvl(trim(ca.consulttime), '1000-01-01 00:00:00'), 1, 19),
                     'yyyy-mm-dd hh24:mi:ss') > to_date(p_beginDate, 'yyyy-mm-dd hh24:mi:ss')
         and to_date(substr(nvl(trim(ca.consulttime), '9990-01-01 23:59:59'), 1, 19),
                     'yyyy-mm-dd hh24:mi:ss') < to_date(p_beginEnd, 'yyyy-mm-dd hh24:mi:ss')
         --and TO_DATE(substr(ca.consulttime,1,10),'yyyy-mm-dd')
           AND ip.status IN ('1500','1501', '1503','1502', '1504', '1505', '1506', '1507')
       and (ca.applydept=v_ApplyDeptid or v_ApplyDeptid = '0000' or v_ApplyDeptid = '' or v_ApplyDeptid is null)
       and (hh.departmentcode=v_EmployeeDeptid or v_EmployeeDeptid = '0000' or v_EmployeeDeptid = '' or v_EmployeeDeptid is null)
       )A WHERE A.DEPARTMENTCODE is not null;---add by ywk 其实受邀科室为空的是不会出现，应该是脏数据
      */
  END;




 ----医生工作站中获得会诊信息（全部符合条件的）  by tj
PROCEDURE usp_GetConsultionDoctor(v_Deptids varchar, --部门编号
                                  v_userid  varchar, --登录人ID
                                  v_levelid varchar, ---登录人级别
                                  o_result  OUT empcurtyp) as
BEGIN
  OPEN o_result FOR
    --Modify by wwj 2013-02-17
  SELECT distinct ip.NAME AS inpatientname,
                    ca.consulttime,
                    ca.consulttypeid,
                    ca.applyuser,
                    cd.NAME AS consultstatus,
                    cd1.NAME AS urgencytype,
                    ca.stateid,
                    ca.noofinpat,
                    ca.consultapplysn,
                    ip.NAME || '_' || cd1.NAME || '_' || ca.consulttime AS inpatientinfo,
                    ca.ispassed
      FROM consultapply ca
      LEFT OUTER JOIN inpatient ip
        ON ip.noofinpat = ca.noofinpat
      LEFT OUTER JOIN categorydetail cd
        ON cd.categoryid = '67'
       AND ca.stateid = cd.ID --会诊单状态
      LEFT OUTER JOIN categorydetail cd1
        ON cd1.categoryid = '66'
       AND cd1.ID = ca.urgencytypeid --紧急度
      LEFT OUTER JOIN consultrecorddepartment hh
        ON ca.consultapplysn = hh.consultapplysn
       AND hh.valid = '1'
     WHERE ((ca.stateid in ('6710','6750','6780') AND ca.applyuser = v_userid) --会诊申请保存,被否决,撤销的会诊记录  只有申请人可见
           OR (ca.stateid = '6720'
              AND (ca.applyuser = v_userid OR (ca.audituserid is not null AND ca.audituserid = v_userid))) --待审核  只有申请人、审核人可以看到
           OR (ca.stateid = '6720'
              AND (ca.applyuser = v_userid OR (ca.audituserid is null AND ca.AUDITLEVEL = v_levelid AND ca.applydept = v_Deptids))) --待审核  只有申请人、审核人可以看到
           OR (ca.stateid in ('6730', '6740') AND hh.employeecode is null
              AND (ca.applyuser = v_userid or (hh.employeelevelid = v_levelid AND hh.departmentcode = v_Deptids))) --待会诊、会诊记录保存 只有参加会诊的医师可以看到此记录
           OR (ca.stateid in ('6730', '6740') AND hh.employeecode is not null
              AND (ca.applyuser = v_userid or hh.employeecode = v_userid))
           OR (ca.stateid = '6741'
              AND (ca.applyuser = v_userid or hh.employeecode = v_userid))
              AND ca.modifiedtime IS NOT NULL AND trunc(to_date(ca.modifiedtime, 'yyyy-mm-dd hh24:mi:ss')) BETWEEN trunc(sysdate - 3) AND trunc(sysdate))
       AND ca.valid = '1'
       and (ca.ispay is null or ca.ispay<>'1')--过滤已缴费的记录Modify by xlb 2013-06-06
       --排除会诊意见填写完毕的记录
       AND NOT EXISTS(
           SELECT 1 FROM consultsuggestion
           WHERE consultsuggestion.consultapplysn = ca.consultapplysn and consultsuggestion.valid = '1'
           and consultsuggestion.createuser = v_userid and consultsuggestion.state = 20
       )
        and not exists (select 1 from consultrecorddepartment conred where conred.consultapplysn=ca.consultapplysn and conred.valid='1'
     and ((conred.issignin is null or conred.issignin<>'1') and
     to_date(ca.consulttime,'yyyy-mm-dd hh24:mi:ss') < (sysdate-7)--只显示会诊时间七天内没签到 Add by xlb 2013-06-07
    ))
    --排除掉超过7天未签到的记录 项令波 2013-06-20
     /*and  exists (select 1 from consultrecorddepartment conred where conred.consultapplysn=ca.consultapplysn and conred.valid='1'
     and (((conred.issignin is null or conred.issignin<>'1') and
      trunc(to_date(ca.consulttime,'yyyy-mm-dd hh24:mi:ss')) between trunc(sysdate-7)--只显示会诊时间七天内没签到 Add by xlb 2013-06-07
     and trunc(sysdate)) or (conred.issignin='1')--或者已签到 Add by xlb 2013-06-07   应该按照会诊时间来计算 比如提前很多天申请了多天后的会诊
    ))*/
      ORDER BY ca.consulttime desc;
   /*SELECT ip.NAME AS inpatientname,

               ip.py,ip.wb,
               TO_CHAR(TO_DATE(ca.consulttime, 'yyyy-mm-dd hh24:mi:ss'),
                       'yyyy-mm-dd hh24:mi:ss') AS consulttime,
               cd.NAME AS consultstatus,
               cd1.NAME AS urgencytype,
               ip.NAME || '_' || cd1.NAME || '_' || ca.consulttime AS inpatientinfo,
               ca.stateid,
                 u.name as applyusername,
                 aa.name as shouyaoren,
               ca.noofinpat,
               ca.modifieduser,
               ca.finishtime,
               ca.consulttypeid,
               ca.applyuser,
               ca.applydept,
               ca.audituserid,
               ca.applytime,
               (case
                    when ca.ispay = '1' then
                    '已缴费'
                    when ca.ispay = '0' then
                    '未缴费'
                     when ca.ispay is null then '未缴费'
                   end) MyPay,
                   ca.ispay,
               ca.consultapplysn,
               hh.departmentcode,  --受邀科室
               hh.employeelevelid, --受邀人级别
               hh.employeecode,   --受邀人工号
              -- hh.issignin         --是否签到
                ip.outhosdept,
                ip.outhosward
          FROM consultapply ca
          LEFT OUTER JOIN users u
            ON u.ID = ca.applyuser
          LEFT OUTER JOIN inpatient ip
            ON ip.noofinpat = ca.noofinpat
          LEFT OUTER JOIN categorydetail cd
            ON cd.categoryid = '67'
           AND ca.stateid = cd.ID
              left outer JOIN consultapplydepartment hh
            on ca.consultapplysn = hh.consultapplysn AND hh.valid = '1' and hh.employeelevelid=v_levelid
              left outer join users aa on aa.id=hh.employeecode
          LEFT OUTER JOIN categorydetail cd1
            ON cd1.categoryid = '66'
           AND cd1.ID = ca.urgencytypeid
         WHERE ca.valid = '1'
           AND ip.status IN ('1500','1501', '1502', '1504', '1505', '1506', '1507')
       and ( stateid = '6730'
    or stateid = '6770'
    or stateid = '6720'
    or stateid = '6750'
    or stateid = '6740'
    or stateid = '6741' and to_date(finishtime, 'yyyy-mm-dd hh24:mi:ss') > trunc(sysdate) - 7)
    --and u.id=V_userid
      and ( u.id=v_userid or ca.applydept=v_Deptids )
union
SELECT ip.NAME AS inpatientname,

               ip.py,ip.wb,
               TO_CHAR(TO_DATE(ca.consulttime, 'yyyy-mm-dd hh24:mi:ss'),
                       'yyyy-mm-dd hh24:mi:ss') AS consulttime,
               cd.NAME AS consultstatus,
               cd1.NAME AS urgencytype,
               ip.NAME || '_' || cd1.NAME || '_' || ca.consulttime AS inpatientinfo,
               ca.stateid,
                 u.name as applyusername,
                 aa.name as shouyaoren,
               ca.noofinpat,
               ca.modifieduser,
               ca.finishtime,
               ca.consulttypeid,
               ca.applyuser,
               ca.applydept,
                ca.audituserid,
                 ca.applytime,
                  (case
                    when ca.ispay = '1' then
                    '已缴费'
                    when ca.ispay = '0' then
                    '未缴费'
                    when ca.ispay is null then '未缴费'
                   end) MyPay,
                   ca.ispay,
               ca.consultapplysn,
               hh.departmentcode,  --受邀科室
               hh.employeelevelid, --受邀人级别
               hh.employeecode ,    --受邀人工号
               --hh.issignin         --是否签到
                ip.outhosdept,
                ip.outhosward
          FROM consultapply ca
          LEFT OUTER JOIN users u
            ON u.ID = ca.applyuser
           AND u.valid = '1'
          LEFT OUTER JOIN inpatient ip
            ON ip.noofinpat = ca.noofinpat
          LEFT OUTER JOIN categorydetail cd
            ON cd.categoryid = '67'
           AND ca.stateid = cd.ID
          LEFT OUTER JOIN categorydetail cd1
            ON cd1.categoryid = '66'
           AND cd1.ID = ca.urgencytypeid
          left outer JOIN consultapplydepartment hh
            on ca.consultapplysn = hh.consultapplysn AND hh.valid = '1'  and hh.employeelevelid=v_levelid
          left outer join users aa on aa.id=hh.employeecode
         WHERE ca.valid = '1'
           and hh.departmentcode = v_Deptids
           AND ip.status IN ('1500', '1501', '1502', '1504', '1505', '1506', '1507')
 and ( stateid = '6730'
    or stateid = '6770'
    or stateid = '6720'
    or stateid = '6750'
    or stateid = '6740'
    or stateid = '6741'
   and to_date(finishtime, 'yyyy-mm-dd hh24:mi:ss') > trunc(sysdate) - 7)
   and (
   hh.employeecode=v_userid or (hh.employeecode is null));*/
/*SELECT distinct(ca.noofinpat),ip.NAME AS inpatientname,
               TO_CHAR(TO_DATE(ca.consulttime, 'yyyy-mm-dd hh24:mi:ss'),
                       'yyyy-mm-dd hh24:mi:ss') AS consulttime,
               cd.NAME AS consultstatus,
               cd1.NAME AS urgencytype,
               ip.NAME || '_' || cd1.NAME || '_' || ca.consulttime AS inpatientinfo,
               ca.stateid,
                 u.name as applyusername,
                 aa.name as shouyaoren,
               --ca.noofinpat,
               ca.modifieduser,
               ca.finishtime,
               ca.consulttypeid,
               ca.applyuser,
               ca.applydept,
                ca.audituserid,
                 ca.applytime,
                  (case
                    when ca.ispay = '1' then
                    '已缴费'
                    when ca.ispay = '0' then
                    '未缴费'
                    when ca.ispay is null then '未缴费'
                   end) MyPay,
                   ca.ispay,
               ca.consultapplysn,
               hh.departmentcode,  --受邀科室
               hh.employeelevelid, --受邀人级别
               hh.employeecode   --受邀人工号
               --hh.issignin         --是否签到
          FROM consultapply ca
          LEFT OUTER JOIN users u
            ON u.ID = ca.applyuser
           AND u.valid = '1'
          LEFT OUTER JOIN inpatient ip
            ON ip.noofinpat = ca.noofinpat
          LEFT OUTER JOIN categorydetail cd
            ON cd.categoryid = '67'
           AND ca.stateid = cd.ID
          LEFT OUTER JOIN categorydetail cd1
            ON cd1.categoryid = '66'
           AND cd1.ID = ca.urgencytypeid
          left outer JOIN consultapplydepartment hh
            on ca.consultapplysn = hh.consultapplysn AND hh.valid = '1'
          left outer join users aa on aa.id=hh.employeecode
         WHERE ca.valid = '1'
          and hh.departmentcode = v_Deptids and hh.employeecode=v_userid
           AND ip.status IN ('1500', '1501', '1502', '1504', '1505', '1506', '1507')
 and (stateid = '6770'
    or stateid = '6730'
    or stateid = '6740'
    or stateid = '6741')
      and to_date(ca.applytime, 'yyyy-mm-dd hh24:mi:ss') > trunc(sysdate) - 7 ;*/

  END;

  ----护士工作站中获得会诊信息 tj
   PROCEDURE usp_GetConsultionNurse(
   v_Deptids     varchar, --部门编号
 o_result    OUT empcurtyp
)as
  BEGIN
  OPEN o_result FOR
SELECT distinct(ca.noofinpat),ip.NAME AS inpatientname,
               TO_CHAR(TO_DATE(ca.consulttime, 'yyyy-mm-dd hh24:mi:ss'),
                       'yyyy-mm-dd hh24:mi:ss') AS consulttime,
               cd.NAME AS consultstatus,
               cd1.NAME AS urgencytype,
               ip.NAME || '_' || cd1.NAME || '_' || ca.consulttime AS inpatientinfo,
               ca.stateid as stateid,
                 u.name as applyusername,
               --ca.noofinpat,
               ca.modifieduser,
               ca.printconsulttime,--打印时间
               ca.finishtime,
               ca.consulttypeid,
               ca.applyuser,
               ca.applydept,
                ca.audituserid,
                 ca.applytime,
                  (case
                    when ca.ispay = '1' then
                    '已缴费'
                    when ca.ispay = '0' then
                    '未缴费'
                    when ca.ispay is null then '未缴费'
                   end) MyPay,
                   ca.ispay,
               ca.consultapplysn,
               ip.outhosdept,
               ip.outhosward
          FROM consultapply ca
          LEFT OUTER JOIN users u
            ON u.ID = ca.applyuser
           AND u.valid = '1'
          LEFT OUTER JOIN inpatient ip
            ON ip.noofinpat = ca.noofinpat
          LEFT OUTER JOIN consultapplydepartment sy --会诊受邀表
          on sy.consultapplysn=ca.consultapplysn
          LEFT OUTER JOIN categorydetail cd
            ON cd.categoryid = '67'
           AND ca.stateid = cd.ID
          LEFT OUTER JOIN categorydetail cd1
            ON cd1.categoryid = '66'
           AND cd1.ID = ca.urgencytypeid
         WHERE ca.valid = '1'
           AND ip.status IN ('1500', '1501', '1502', '1504', '1505', '1506', '1507')
           and  (ca.stateid = '6730' or ca.stateid='6740' or   ca.stateid='6741')
           and (ca.applydept=v_Deptids or sy.departmentcode=v_Deptids)
           and (ca.ispay = '1' or ca.ispay = '0'or ca.ispay is null)
           and to_date(ca.applytime, 'yyyy-mm-dd hh24:mi:ss') > trunc(sysdate) - 7
           order  by stateid, TO_DATE(consulttime, 'yyyy-mm-dd hh24:mi:ss') desc
           --order  by  outhosdept,outhosward

           ;
  END;

   ----会诊提醒信息   by tj
   PROCEDURE usp_GetConsultionMessage(
   v_Deptids     varchar, --部门编号
   v_levelid varchar,---登录人级别
 o_result    OUT empcurtyp
)as
  BEGIN
  OPEN o_result FOR
SELECT distinct(ca.noofinpat),ip.NAME AS inpatientname,
               TO_CHAR(TO_DATE(ca.consulttime, 'yyyy-mm-dd hh24:mi:ss'),
                       'yyyy-mm-dd hh24:mi:ss') AS consulttime,
               cd.NAME AS consultstatus,
               cd1.NAME AS urgencytype,
               ip.NAME || '_' || cd1.NAME || '_' || ca.consulttime AS inpatientinfo,
               ca.stateid,
                 u.name as applyusername,
                 aa.name as shouyaoren,
               ca.modifieduser,
               ca.finishtime,
               ca.consulttypeid,
               ca.applyuser,
               ca.applydept,
                ca.audituserid,
                 ca.applytime,
                  (case
                    when ca.ispay = '1' then
                    '已缴费'
                    when ca.ispay = '0' then
                    '未缴费'
                    when ca.ispay is null then '未缴费'
                   end) MyPay,
                   ca.ispay,
               ca.consultapplysn,
               hr.departmentcode,  --受邀科室
               hr.employeelevelid, --受邀人级别
               hr.employeecode   --受邀人工号
               --hh.issignin         --是否签到
          FROM consultapply ca
          LEFT OUTER JOIN users u
            ON u.ID = ca.applyuser
           AND u.valid = '1'
          LEFT OUTER JOIN inpatient ip
            ON ip.noofinpat = ca.noofinpat
          LEFT OUTER JOIN categorydetail cd
            ON cd.categoryid = '67'
           AND ca.stateid = cd.ID
          LEFT OUTER JOIN categorydetail cd1
            ON cd1.categoryid = '66'
           AND cd1.ID = ca.urgencytypeid
          --left outer JOIN consultapplydepartment hh --会诊受邀表
           -- on ca.consultapplysn = hh.consultapplysn AND hh.valid = '1'
          left outer JOIN CONSULTRECORDDEPARTMENT hr --会诊记录表
            on ca.consultapplysn = hr.consultapplysn AND hr.valid = '1'  --and hr.employeelevelid=v_levelid
          left outer join users aa on aa.id=hr.employeecode
         WHERE ca.valid = '1'
           AND ip.status IN ('1500', '1501', '1502', '1504', '1505', '1506', '1507')
 and (
       (stateid = '6730' and ( hr.issignin='0' or hr.issignin='' or hr.issignin is null))
    or (stateid = '6770' and ( hr.issignin='0' or hr.issignin='' or hr.issignin is null))
    or (stateid = '6720' and ( hr.issignin='0' or hr.issignin='' or hr.issignin is null))
    or (stateid = '6750' and ( hr.issignin='0' or hr.issignin='' or hr.issignin is null))
   -- or (stateid = '6740' and ( hr.issignin='0' or hr.issignin='' or hr.issignin is null))
   -- or (stateid = '6741' and ( hr.issignin='0' or hr.issignin='' or hr.issignin is null))
    )
--edit by xlb 2013-03-15 没抓取会诊记录保存或完成状态 完成时间可为空
   and to_date(ca.consulttime, 'yyyy-mm-dd hh24:mi:ss') > trunc(sysdate) - 3 --三天前的会诊不显示
   and (hr.departmentcode =v_Deptids  or ca.applydept=v_Deptids)--or  hr.departmentcode is null这句话有问题edit by ywk 2013年8月30日 14:09:35
   and (hr.issignin='0' or hr.issignin='' or hr.issignin is null or hr.employeelevelid=v_levelid)---edit by ywk 2013年8月30日 14:39:02
   order  by TO_DATE(consulttime, 'yyyy-mm-dd hh24:mi:ss') desc
   ;-- or hh.employeelevelid=u.grade));

  END;


   --Add by wwj 2013-02-18 护士工作站的会诊数据捞取
   PROCEDURE usp_GetConsultionForNurse(v_Deptids varchar,      --部门编号
                                       v_ConsultDateFrom date, --会诊日期
                                       v_ConsultDateTo date,   --会诊日期
                                       v_ConsultState varchar, --会诊状态
                                       o_result OUT empcurtyp
   ) AS
   BEGIN
   OPEN o_result FOR
        SELECT distinct ca.noofinpat,
                ip.NAME AS inpatientname,
                TO_CHAR(TO_DATE(ca.consulttime, 'yyyy-mm-dd hh24:mi:ss'),
                        'yyyy-mm-dd hh24:mi:ss') AS consulttime,
                cd.NAME AS consultstatus,
                cd1.NAME AS urgencytype,
                ip.NAME || '_' || cd1.NAME || '_' || ca.consulttime AS inpatientinfo,
                ca.stateid as stateid,
                u.name as applyusername,
                ca.modifieduser,
                ca.printconsulttime, --打印时间
                ca.finishtime,
                ca.consulttypeid,
                ca.applyuser,
                ca.applydept,
                ca.audituserid,
                ca.applytime,
                decode(ca.ispay, '1', '已缴费', '未缴费') MyPay,
                ca.ispay,
                ca.consultapplysn,
                ip.outhosdept,
                ip.outhosward
        FROM consultapply ca
            LEFT OUTER JOIN users u ON u.ID = ca.applyuser AND u.valid = '1'
            LEFT OUTER JOIN inpatient ip ON ip.noofinpat = ca.noofinpat
           inner JOIN consultrecorddepartment sy on sy.consultapplysn = ca.consultapplysn--修改 ywk 受邀科室为空过滤
            LEFT OUTER JOIN categorydetail cd ON cd.categoryid = '67' AND ca.stateid = cd.ID
            LEFT OUTER JOIN categorydetail cd1 ON cd1.categoryid = '66' AND cd1.ID = ca.urgencytypeid
        WHERE ca.valid = '1'
            and (v_ConsultState is null or v_ConsultState=''  AND ca.stateid IN (6730,6740,6741) --抓取会诊记录完成、待会诊、会诊记录保存三状态
                OR
                (v_ConsultState is not null AND ca.stateid = v_ConsultState)
            ) --待会诊、会诊记录保存、会诊记录完成
            and (ca.applydept = v_Deptids OR sy.departmentcode = v_Deptids)
            and to_date(ca.consulttime, 'yyyy-mm-dd hh24:mi:ss') >= v_ConsultDateFrom
            and to_date(ca.consulttime, 'yyyy-mm-dd hh24:mi:ss') < (v_ConsultDateTo + 1)
        ORDER BY TO_DATE(consulttime, 'yyyy-mm-dd hh24:mi:ss') desc;
 END;



 ----获得已会诊未缴费的记录 Add by wwj 2013-02-19
 PROCEDURE usp_GetAlreadyConsultNotFee(v_Deptids varchar, --部门编号
                                  o_result  OUT empcurtyp)as
  BEGIN
    OPEN o_result FOR
        SELECT distinct ip.NAME AS inpatientname,
               ip.Py,ip.wb,
               TO_CHAR(TO_DATE(ca.consulttime, 'yyyy-mm-dd hh24:mi:ss'),
                       'yyyy-mm-dd hh24:mi:ss') AS consulttime,
               cd.NAME AS consultstatus,
               cd1.NAME AS urgencytype,
               ip.NAME || '_' || cd1.NAME || '_' || ca.consulttime AS inpatientinfo,
               ca.stateid,
               u.name as applyusername,
               ca.noofinpat,
               ca.modifieduser,
               ca.finishtime,
               ca.consulttypeid,
               ca.applyuser,
               ca.applydept,
               ca.audituserid,
               ca.applytime,
               decode(ca.ispay, '1', '已缴费', '未缴费') MyPay,
               ca.ispay,
               ca.consultapplysn,
               ca.printconsulttime
          FROM consultapply ca
          LEFT OUTER JOIN users u ON u.ID = ca.applyuser
          LEFT OUTER JOIN inpatient ip ON ip.noofinpat = ca.noofinpat
          LEFT OUTER JOIN categorydetail cd ON cd.categoryid = '67' AND ca.stateid = cd.ID
          LEFT OUTER JOIN consultrecorddepartment hh on ca.consultapplysn = hh.consultapplysn AND hh.valid = '1'
          LEFT OUTER join users aa on aa.id=hh.employeecode
          LEFT OUTER JOIN categorydetail cd1 ON cd1.categoryid = '66' AND cd1.ID = ca.urgencytypeid
         WHERE ca.valid = '1' and (ca.applydept = v_Deptids OR hh.departmentcode = v_Deptids)
           and ca.stateid in (6740, 6741) and (ca.ispay <> '1' or ca.ispay is null);
  END;

  --会诊费用测试
  PROCEDURE usp_FeeTest(arg_inp varchar,
                        arg_name varchar,
                        arg_item varchar,
                        arg_itemname varchar,
                        arg_amount number,
                        arg_doct varchar,
                        arg_doctdept varchar,
                        arg_date date,
                        arg_dept varchar,
                        arg_hzdoct varchar,
                        arg_opercode varchar,
                        arg_operdate date,
                        return_code out number,
                        return_result out varchar)as
  BEGIN
    return_code := 1;
    return_result := '操作正确！';
  END;

  --获取待审核的会诊记录 Add by wwj 2013-02-27
  PROCEDURE usp_GetUnAuditConsult(v_timefrom date, --会诊起始时间
                                  v_timeto date,   --会诊终止时间
                                  v_inpatientname varchar, --病人姓名
                                  v_patid  varchar, --病历号
                                  v_urgencyTypeID varchar, --紧急度
                                  v_currentuserid varchar, --当前系统登录人
                                  v_currentuserlevel varchar, --当前用户级别
                                  o_result OUT empcurtyp)as
  BEGIN
    OPEN o_result FOR
        select inp.Name  as PatientName, inp.PatID, inp.OutBed,
             ca.ConsultApplySn, ca.NoOfInpat, ca.UrgencyTypeID, cd2.Name  as UrgencyTypeName,
             ca.ConsultTypeID, cd1.Name  as ConsultTypeName, ca.Abstract, ca.Purpose,
             ca.ApplyUser, u1.Name  as ApplyUserName, ca.ApplyTime, ca.Director,
             u2.Name  as DirectorName, ca.ConsultTime, ca.ConsultLocation, ca.StateID,
             cd3.Name  as StatusName, ca.ConsultSuggestion, ca.FinishTime, ca.RejectReason,
             ca.Valid, ca.CreateUser, ca.CreateTime, d.Name  as DeptName,
             ca.audituserid
        from ConsultApply ca
            left outer join CategoryDetail cd1 on cd1.CategoryID = '65' and cd1.ID = ca.ConsultTypeID
            left outer join CategoryDetail cd2 on cd2.CategoryID = '66' and cd2.ID = ca.UrgencyTypeID
            left outer join CategoryDetail cd3 on cd3.CategoryID = '67' and cd3.ID = ca.StateID
            left outer join Users u1 on u1.ID = ca.ApplyUser and u1.Valid = '1'
            left outer join Users u2 on u2.ID = ca.Director and u2.Valid = '1'
            left outer join Inpatient inp on inp.NoOfInpat = ca.NoOfInpat
            left outer join Department d on d.ID = u1.DeptId and d.Valid = '1'
        where to_date(ca.consulttime, 'yyyy-MM-dd hh24:mi:ss') between v_timefrom and (v_timeto + 1)
          and inp.name like '%' || v_inpatientname || '%'
          and inp.PatID like '%' || v_patid || '%'
          and ca.UrgencyTypeID like '%' || v_urgencyTypeID || '%'
          and ca.valid = 1
          and ca.stateid = '6720' --待审核
          and ((ca.audituserid is not null and ca.audituserid = v_currentuserid)
                or (ca.audituserid is null and ca.auditlevel = v_currentuserlevel));
  END;

  --获取与系统登录人相关的待会诊、会诊记录保存的记录 Add by wwj 2013-02-27
  PROCEDURE usp_GetWaitConsult(v_timefrom date, --会诊起始时间
                               v_timeto date,   --会诊终止时间
                               v_inpatientname varchar, --病人姓名
                               v_patid  varchar, --病历号
                               v_urgencyTypeID varchar, --紧急度
                               v_bedCode varchar, --床位号
                               v_currentuserid varchar, --当前系统登录人
                               v_currentuserlevel varchar, --当前用户级别
                               o_result OUT empcurtyp)as
  BEGIN
    OPEN o_result FOR
        --获取与登录人相关的会诊记录(待会诊、会诊记录保存)
              select inp. Name  as PatientName, inp.PatID, inp.OutBed,
               ca.ConsultApplySn, ca.NoOfInpat, ca.UrgencyTypeID, cd2.Name as UrgencyTypeName,
               ca.ConsultTypeID, cd1.Name as ConsultTypeName, ca.Abstract, ca.Purpose,
               ca.ApplyUser, u1.Name as ApplyUserName, ca.ApplyTime, ca.Director,
               u2.Name as DirectorName, ca.ConsultTime, ca.ConsultLocation, ca.StateID,
               cd3.Name as StatusName, ca.ConsultSuggestion, ca.FinishTime, ca.RejectReason,
               decode(ca.ispay, '1', '已付款', '未付款') ISPAY,
               ca.Valid, ca.CreateUser, ca.CreateTime, crp.appdept, crp.appname,
               crp.ISSIGNIN, crp.REACHTIME, d.Name as DeptName
          from ConsultApply ca
          left join (select consultapplysn, wmsys.wm_concat(decode(ISSIGNIN, '1', '已签到', '未签到')) ISSIGNIN, wmsys.wm_concat(departmentname) appdept,wmsys.wm_concat(consultrecorddepartment.employeename) appname, wm_concat(REACHTIME) REACHTIME from consultrecorddepartment where VALID = 1 group by consultapplysn) crp
                on ca.CONSULTAPPLYSN = crp.CONSULTAPPLYSN    
          left outer join CategoryDetail cd1 on cd1.CategoryID = '65' and cd1.ID = ca.ConsultTypeID
          left outer join CategoryDetail cd2 on cd2.CategoryID = '66' and cd2.ID = ca.UrgencyTypeID
          left outer join CategoryDetail cd3 on cd3.CategoryID = '67' and cd3.ID = ca.StateID
          left outer join Users u1 on u1.ID = ca.ApplyUser and u1.Valid = '1'
          left outer join Users u2 on u2.ID = ca.Director and u2.Valid = '1'
          left outer join InPatient inp on inp.NoOfInpat = ca.NoOfInpat
          left outer join Department d on d.ID = u1.DeptId and d.Valid = '1'
         where ca.StateID in ( '6730', '6740' )
            and (exists(select 1 from consultrecorddepartment cd where ((cd.employeecode is not null and cd.employeecode = v_currentuserid) or (cd.employeecode is null and cd.employeelevelid = v_currentuserlevel)) or ca.applyuser =v_currentuserid) )
            and inp.name like '%' || v_inpatientname || '%'
            and inp.patid like '%' || v_patid || '%'
            and ca.urgencytypeid like '%' || v_urgencytypeid || '%'
            and (inp.outbed like '%' || v_bedcode || '%' or inp.outbed is null)
            and to_date(ca.applytime, 'yyyy-MM-dd hh24:mi:ss') between v_timefrom and (v_timeto + 1)
            and ca.Valid = '1';
  END;
  --add by xlb 2013-03-08
 PROCEDURE usp_SaveConsultRecord(
                                         v_TypeID          INT DEFAULT 0,
                                         v_ConsultId              INT DEFAULT 0,
                                         v_consultapplysn  INT DEFAULT 0,
                                         v_ordervalue      INT DEFAULT 0,
                                         v_hospitalcode    VARCHAR DEFAULT '',
                                         v_departmentcode  VARCHAR DEFAULT '',
                                         v_departmentname  VARCHAR DEFAULT '',
                                         v_employeecode    VARCHAR DEFAULT '',
                                         v_employeename    VARCHAR DEFAULT '',
                                         v_employeelevelid VARCHAR DEFAULT '',
                                         v_createuser      VARCHAR DEFAULT '',
                                         v_createtime      VARCHAR DEFAULT '',
                                         v_canceluser      VARCHAR DEFAULT '',
                                         v_modifyuser      VARCHAR DEFAULT '') AS


BEGIN

  --1表示删除的会诊邀请部分记录
  IF v_TypeID=1 THEN
    BEGIN
      update consultrecorddepartment set valid='0',
      canceluser=v_canceluser,canceltime=sysdate where id=v_ConsultId;
     END;
     END IF;
--2表示修改记录
IF v_TypeID=2 THEN
  BEGIN
    UPDATE consultrecorddepartment c set c.departmentcode=v_departmentcode,
    c.departmentname=v_departmentname,c.employeecode=v_employeecode,c.employeename=v_employeename,
    c.employeelevelid=v_employeelevelid,c.modifyuser=v_modifyuser,c.modifytime=sysdate where id=v_ConsultId;
   END;
   END IF;
   --3表示插入记录
  IF v_TypeID=3 THEN
    BEGIN
       INSERT INTO consultrecorddepartment
       (ID,
       consultapplysn,
       ordervalue,
       hospitalcode,
       departmentcode,
       departmentname,
       employeecode,
       employeename,
       employeelevelid,
       createuser,
       createtime,
       valid,
       canceluser,
       canceltime)
    VALUES
      (seq_consultrecorddepartment_id.NEXTVAL,
       v_consultapplysn,
       v_ordervalue,
       v_hospitalcode,
       v_departmentcode,
       v_departmentname,
       v_employeecode,
       v_employeename,
       v_employeelevelid,
       v_createuser,
       v_createtime,
       '1',
       NULL,
       NULL);
  END;
  END IF;

END;
--Add by xlb 2013-03-08
 PROCEDURE usp_SaveApplyDepartOrRecord(

                                         v_TypeId          INT DEFAULT 0,
                                         v_consultapplysn  INT DEFAULT 0,
                                         v_ordervalue      INT DEFAULT 0,
                                         v_hospitalcode    VARCHAR DEFAULT '',
                                         v_departmentcode  VARCHAR DEFAULT '',
                                         v_departmentname  VARCHAR DEFAULT '',
                                         v_employeecode    VARCHAR DEFAULT '',
                                         v_employeename    VARCHAR DEFAULT '',
                                         v_employeelevelid VARCHAR DEFAULT '',
                                         v_createuser      VARCHAR DEFAULT '',
                                         v_createtime      VARCHAR DEFAULT '') as
 BEGIN
   --1表示只在申请受邀部门表中插入数据
   IF    v_TypeId=1 THEN
     BEGIN
    INSERT INTO consultapplydepartment
      (ID, -- this column value is auto-generated,
       consultapplysn,
       ordervalue,
       hospitalcode,
       departmentcode,
       departmentname,
       employeecode,
       employeename,
       employeelevelid,
       createuser,
       createtime,
       valid,
       canceluser,
       canceltime)
    VALUES
      (seq_consultapplydepartment_id.NEXTVAL,
       v_consultapplysn,
       v_ordervalue,
       v_hospitalcode,
       v_departmentcode,
       v_departmentname,
       v_employeecode,
       v_employeename,
       v_employeelevelid,
       v_createuser,
       v_createtime,
       '1',
       NULL,
       NULL);
END;
END IF;
IF v_TypeId=2 THEN --2表示 插入实际会诊部门表
  BEGIN


        INSERT INTO consultrecorddepartment
      (ID, -- this column value is auto-generated,
       consultapplysn,
       ordervalue,
       hospitalcode,
       departmentcode,
       departmentname,
       employeecode,
       employeename,
       employeelevelid,
       createuser,
       createtime,
       valid,
       canceluser,
       canceltime)
    VALUES
      (seq_consultrecorddepartment_id.NEXTVAL,
       v_consultapplysn,
       v_ordervalue,
       v_hospitalcode,
       v_departmentcode,
       v_departmentname,
       v_employeecode,
       v_employeename,
       v_employeelevelid,
       v_createuser,
       v_createtime,
       '1',
       NULL,
       NULL);
  END;
  END IF;
END;

END;
/
