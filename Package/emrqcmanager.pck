CREATE OR REPLACE PACKAGE emrqcmanager IS
  TYPE empcurtype IS REF CURSOR;

  --得到病人列表
  PROCEDURE usp_getpatientlist(v_deptid         VARCHAR2,
                               v_patid          VARCHAR2,
                               v_name           VARCHAR2,
                               v_status         VARCHAR2,
                               v_admitbegindate VARCHAR2,
                               v_admitenddate   VARCHAR2,
                               o_result         OUT empcurtype);
  PROCEDURE usp_getpatientlistandpat(v_deptid         VARCHAR2,
                                     v_patid          VARCHAR2,
                                     v_name           VARCHAR2,
                                     v_status         VARCHAR2,
                                     v_admitbegindate VARCHAR2,
                                     v_admitenddate   VARCHAR2,
                                     o_result         OUT empcurtype);

PROCEDURE usp_getpatientlistandpat_ZY(v_deptid         VARCHAR2,
                                     v_patid          VARCHAR2,
                                     v_name           VARCHAR2,
                                     v_status         VARCHAR2,
                                     v_admitbegindate VARCHAR2,
                                     v_admitenddate   VARCHAR2,
                                     o_result         OUT empcurtype);


  PROCEDURE usp_getpatientlistandpatSZ(v_deptid         VARCHAR2,
                                       v_patid          VARCHAR2,
                                       v_name           VARCHAR2,
                                       v_status         VARCHAR2,
                                       v_admitbegindate VARCHAR2,
                                       v_admitenddate   VARCHAR2,
                                       o_result         OUT empcurtype);
  
  
   PROCEDURE usp_getpatientlistandpatSZ_ZY(v_deptid         VARCHAR2,
                                       v_patid          VARCHAR2,
                                       v_name           VARCHAR2,
                                       v_status         VARCHAR2,
                                       v_admitbegindate VARCHAR2,
                                       v_admitenddate   VARCHAR2,
                                       o_result         OUT empcurtype);                                     
  
  
  --得到科室统计信息
  procedure usp_GetAllDepartmentStatInfo(v_deptid         varchar2,
                                         v_patid          varchar2,
                                         v_name           varchar2,
                                         v_status         varchar2,
                                         v_admitbegindate varchar2,
                                         v_admitenddate   varchar2,
                                         o_result         OUT empcurtype);

  --统计科室的病历评分信息 add by wyt 2012-12-12
  procedure usp_getdepartmentpatqcinfo(v_deptid         VARCHAR2,
                                       v_patid          VARCHAR2,
                                       v_name           VARCHAR2,
                                       v_status         VARCHAR2,
                                       v_admitbegindate VARCHAR2,
                                       v_admitenddate   VARCHAR2,
                                       v_sortid         varchar2,
                                       v_sumpoint1      int,
                                       v_sumpoint2      int,
                                       V_type           varchar2,
                                       V_userid         varchar2,
                                       V_auth           varchar2,
                                       o_result         OUT empcurtype);

  ---获得科室内病人，（在评分表中存在记录的病人）
  PROCEDURE usp_getdepartmentpatstatinfo(v_deptid         VARCHAR2,
                                         v_patid          VARCHAR2,
                                         v_name           VARCHAR2,
                                         v_status         VARCHAR2,
                                         v_admitbegindate VARCHAR2,
                                         v_admitenddate   VARCHAR2,
                                         v_sortid         VARCHAR2,
                                         v_sumpoint       int, --新增的总分 ywk 2012年6月12日 14:51:41
                                         V_type           varchar2, --新增的按类型查询 ywk 2012年11月6日10:55:58
                                         o_result         OUT empcurtype);

  --仁和 获取科室统计信息 通过病人状态获取
  PROCEDURE usp_RHgetalldepstatinfo(v_deptid         VARCHAR2,
                                    v_patid          VARCHAR2,
                                    v_name           VARCHAR2,
                                    v_status         VARCHAR2,
                                    v_admitbegindate VARCHAR2,
                                    v_admitenddate   VARCHAR2,
                                    o_result         OUT empcurtype);

  --仁和 获取科室统计信息 住院和出院病人汇总
  PROCEDURE usp_RHgetDepstatinfoAll(v_deptid         VARCHAR2,
                                    v_patid          VARCHAR2,
                                    v_name           VARCHAR2,
                                    v_admitbegindate VARCHAR2,
                                    v_admitenddate   VARCHAR2,
                                    o_result         OUT empcurtype);

  ---仁和 获得科室内病人，（在评分表中存在记录的病人 并且记录状态是8701 科室主任）
  PROCEDURE usp_GetRHGetDeptinfo(v_deptid         VARCHAR2,
                                 v_patid          VARCHAR2,
                                 v_name           VARCHAR2,
                                 v_status         VARCHAR2,
                                 v_admitbegindate VARCHAR2,
                                 v_admitenddate   VARCHAR2,
                                 o_result         OUT empcurtype);

  --得到病人的统计信息(仁和版本的，不连接评分表edit by ywk 2012年7月13日 08:58:34)
  procedure usp_GetrhDepartmentPatStatInfo(v_deptid         varchar2,
                                           v_patid          varchar2,
                                           v_name           varchar2,
                                           v_status         varchar2,
                                           v_admitbegindate varchar2,
                                           v_admitenddate   varchar2,
                                           o_result         OUT empcurtype);

  --得到病人的统计信息 质控科人员 入院出院 (仁和版本的，不连接评分表edit by ywk 2012年8月2日 08:58:34)
  procedure usp_GetrhDepPatStatInfoAll(v_deptid         varchar2,
                                       v_patid          varchar2,
                                       v_name           varchar2,
                                       v_admitbegindate varchar2,
                                       v_admitenddate   varchar2,
                                       o_result         OUT empcurtype);

  --得到病人的统计信息 科室指控员 (仁和版本的，不连接评分表edit by ywk 2012年8月2日 08:58:34)
  procedure usp_GetrhDepPatStatInfoCY(v_deptid         varchar2,
                                      v_patid          varchar2,
                                      v_name           varchar2,
                                      v_admitbegindate varchar2,
                                      v_admitenddate   varchar2,
                                      o_result         OUT empcurtype);

  --得到病人的所有病历
  procedure usp_GetAllEmrDocByNoofinpat(v_noofinpat varchar2,
                                        o_result    OUT empcurtype);

  --得到科室所有医生
  procedure usp_GetAllDoctorByUserNO(v_userid varchar2,
                                     o_result OUT empcurtype);

  --得到科室所有医生
  procedure usp_GetAllDoctorByNoofinpat(v_noofinpat varchar2,
                                        o_result    OUT empcurtype);

  --得到病历评分表的信息 //edit by wyt 2012-12-06 新增评分主记录ID条件
  procedure usp_GetEmrPointByNoofinpat(v_noofinpat varchar2,
                                       v_chiefid   varchar2,
                                       o_result    OUT empcurtype);

  -- 仁和 得到病历评分表的详细信息
  procedure usp_GetRHPoint(v_rhqc_tableId varchar2,
                           o_result       OUT empcurtype);

  procedure usp_insertEmrPoint(v_doctorid       varchar2,
                               v_doctorname     varchar2,
                               v_create_user    varchar2,
                               v_createusername varchar2,
                               v_problem_desc   varchar2,
                               v_reducepoint    varchar2,
                               v_num            varchar2,
                               --v_grade varchar2,  王冀 2012 11 30
                               v_recorddetailid   varchar2,
                               v_noofinpat        varchar2,
                               v_recorddetailname varchar2,
                               v_sortid           varchar2, --新增大类编号
                               -- v_emrpointid INT DEFAULT 0,---新增字段
                               v_emrpointid varchar2, ---新增字段 --edit by wyt 2012-12-11
                               v_chiefid    varchar2 ---新增字段
                               );

  --删除仁和评分表的项
  procedure usp_cancelRHEmrPoint(v_id              varchar2,
                                 v_cancel_user     varchar2,
                                 v_cancel_userName varchar2);

  procedure usp_cancelEmrPoint(v_id varchar2, v_cancel_user varchar2);

  procedure usp_GetPatientInfo(v_noofinpat varchar2,
                               o_result    OUT empcurtype);

  procedure usp_GetPointByDoctorID(v_doctorID varchar2,
                                   o_result   OUT empcurtype);

  ------病历评分类别配置                  
  PROCEDURE usp_Edit_ConfigPoint(v_EditType   varchar default '', --操作类型
                                 V_ID         varchar default '',
                                 v_CCode      varchar default '',
                                 v_CChildCode varchar default '',
                                 v_CChildName varchar default '',
                                 v_Valid      varchar default '1',
                                 o_result     OUT empcurtype);

  ------病历评分（扣分理由）配置       ywk 2012年5月28日 09:22:10           
  PROCEDURE usp_Edit_ConfigReduction(v_EditType        varchar default '', --操作类型
                                     V_REDUCEPOINT     varchar default '',
                                     v_PROBLEMDESC     varchar default '',
                                     v_CREATEUSER      varchar default '',
                                     v_CREATETIME      varchar default '',
                                     v_ID              varchar default '',
                                     v_Valid           varchar default '1',
                                     v_Parents         varchar default '',
                                     v_Children        varchar default '',
                                     v_Isauto          varchar default '',
                                     v_Selectcondition varchar default '',
                                     o_result          OUT empcurtype);

  --病历评分中要显示的跟节点                      
  procedure usp_GetPointClass(o_result OUT empcurtype);

  ------科室质控员的配置操作 2012年7月10日13:45:29 ywk
  PROCEDURE usp_Edit_ConfigPointManager(v_EditType      varchar default '', --操作类型
                                        V_ID            varchar default '',
                                        v_DeptID        varchar default '',
                                        v_QcManagerID   varchar default '',
                                        v_ChiefDoctorID varchar default '',
                                        v_Valid         varchar default '1',
                                        o_result        OUT empcurtype);

  PROCEDURE usp_getRHdepartpatstatinfo(v_deptid         VARCHAR2,
                                       v_patid          VARCHAR2,
                                       v_name           VARCHAR2,
                                       v_status         VARCHAR2,
                                       v_admitbegindate VARCHAR2,
                                       v_admitenddate   VARCHAR2,
                                       o_result         OUT empcurtype);

  ------病历评分（扣分理由）配置 仁和质控       ywk 2012年8月7日 09:22:10           
  PROCEDURE usp_Edit_RHConfigReduction(v_EditType    varchar default '', --操作类型
                                       V_REDUCEPOINT varchar default '',
                                       v_PROBLEMDESC varchar default '',
                                       v_CREATEUSER  varchar default '',
                                       v_CREATETIME  varchar default '',
                                       v_ID          varchar default '',
                                       v_Valid       varchar default '1',
                                       v_UserType    varchar default '',
                                       o_result      OUT empcurtype);
  ------质控统计详情   wj 2013 3 21                                     
  PROCEDURE usp_QcmanagerDepartmentlist(o_result OUT empcurtype);
  
  procedure usp_QcmanagerDepartmentDetail(v_deptid varchar default '',
                                         o_result OUT empcurtype);
END; -- Package spec
/
CREATE OR REPLACE PACKAGE BODY emrqcmanager IS
  PROCEDURE usp_getpatientlist(v_deptid         VARCHAR2,
                               v_patid          VARCHAR2,
                               v_name           VARCHAR2,
                               v_status         VARCHAR2,
                               v_admitbegindate VARCHAR2,
                               v_admitenddate   VARCHAR2,
                               o_result         OUT empcurtype) AS
    p_admitbegindate VARCHAR2(19);
    p_admitenddate   VARCHAR2(19);
  BEGIN
    p_admitbegindate := v_admitbegindate || ' 00:00:00';
    p_admitenddate   := v_admitenddate || ' 23:59:59';
  
    IF v_admitbegindate = '' THEN
      p_admitbegindate := '2001-01-01 00:00:00';
    END IF;
  
    IF v_admitenddate = '' THEN
      p_admitenddate := '2999-01-01 00:00:00';
    END IF;
  
    OPEN o_result FOR
      SELECT inpatient.outhosdept deptno,
             department.NAME      deptname,
             inpatient.patid,
             inpatient.NAME,
             inpatient.noofinpat,
             inpatient.admitdate, --新增入院时间列 add by ywk 2012年8月10日 09:18:23
             inpatient.status --新增病人状态列 add by wyt 2012年12月10日
        FROM inpatient
        LEFT OUTER JOIN department
          ON department.ID = inpatient.outhosdept
       WHERE inpatient.outhosdept LIKE '%' || v_deptid || '%'
         AND inpatient.patid LIKE '%' || v_patid || '%'
         AND inpatient.NAME LIKE '%' || v_name || '%'
         AND inpatient.status LIKE '%' || v_status || '%'
         AND (inpatient.admitdate >= p_admitbegindate AND
             inpatient.admitdate <= p_admitenddate);
  END;
  --by xlb 2012-12-26 获取病人列表，要对出院和出区病人进行重点病人类型查询
  
  PROCEDURE usp_getpatientlistandpat(v_deptid         VARCHAR2,
                                     v_patid          VARCHAR2,
                                     v_name           VARCHAR2,
                                     v_status         VARCHAR2,
                                     v_admitbegindate VARCHAR2,
                                     v_admitenddate   VARCHAR2,
                                     o_result         OUT empcurtype) AS
    p_admitbegindate VARCHAR2(19);
    p_admitenddate   VARCHAR2(19);
  BEGIN
    p_admitbegindate := v_admitbegindate || ' 00:00:00';
    p_admitenddate   := v_admitenddate || ' 23:59:59';
  
    IF v_admitbegindate = '' THEN
      p_admitbegindate := '2001-01-01 00:00:00';
    END IF;
  
    IF v_admitenddate = '' THEN
      p_admitenddate := '2999-01-01 00:00:00';
    END IF;
  
    OPEN o_result FOR
      SELECT distinct inpatient.outhosdept deptno,
                      department.NAME      deptname,
                      inpatient.patid,
                      inpatient.NAME,
                      inpatient.noofinpat,
                      inpatient.admitdate,
                      inpatient.status,
                      m.zg_flag,
                      m.outhostype,
                      --o.close_level,       edit by wangj 2013 3 6 一个病人此记录不止一条
                      o.iem_mainpage_no,
                      u.name            DOCTORNAME,
                      f.bloodfee
        FROM inpatient
        LEFT OUTER JOIN department
          ON department.ID = inpatient.outhosdept
        left join iem_mainpage_basicinfo_2012 m
          on inpatient.noofinpat = m.noofinpat
        left join iem_mainpage_operation_2012 o
          on m.iem_mainpage_no = o.iem_mainpage_no
        left join iem_mainpage_feeinfo f
          on m.iem_mainpage_no = f.iem_mainpage_no
        left join users u
          on u.id = inpatient.resident
       WHERE inpatient.outhosdept LIKE '%' || v_deptid || '%'
         and (m.valide = '1' or m.valide is null or m.valide = '') --排除无效数据)
         and (o.valide = '1' or o.valide is null or o.valide = '')
         and (f.valid = '1' or f.valid is null or f.valid = '')
         AND inpatient.patid LIKE '%' || v_patid || '%'
         AND inpatient.NAME LIKE '%' || v_name || '%'
         AND inpatient.status LIKE '%' || v_status || '%'
         AND (inpatient.admitdate >= p_admitbegindate AND
             inpatient.admitdate <= p_admitenddate);
  END;


PROCEDURE usp_getpatientlistandpat_ZY(v_deptid         VARCHAR2,
                                     v_patid          VARCHAR2,
                                     v_name           VARCHAR2,
                                     v_status         VARCHAR2,
                                     v_admitbegindate VARCHAR2,
                                     v_admitenddate   VARCHAR2,
                                     o_result         OUT empcurtype) AS
    p_admitbegindate VARCHAR2(19);
    p_admitenddate   VARCHAR2(19);
  BEGIN
    p_admitbegindate := v_admitbegindate || ' 00:00:00';
    p_admitenddate   := v_admitenddate || ' 23:59:59';
  
    IF v_admitbegindate = '' THEN
      p_admitbegindate := '2001-01-01 00:00:00';
    END IF;
  
    IF v_admitenddate = '' THEN
      p_admitenddate := '2999-01-01 00:00:00';
    END IF;
  
    OPEN o_result FOR
      SELECT distinct inpatient.outhosdept deptno,
                      department.NAME      deptname,
                      inpatient.patid,
                      inpatient.NAME,
                      inpatient.noofinpat,
                      inpatient.admitdate,
                      inpatient.status,
                      '' as zg_flag,
                      m.outhostype,
                      --o.close_level,       edit by wangj 2013 3 6 一个病人此记录不止一条
                      o.iem_mainpage_no,
                      u.name            DOCTORNAME,
                      f.bloodfee
        FROM inpatient
        LEFT OUTER JOIN department
          ON department.ID = inpatient.outhosdept
        left join iem_mainpage_basicinfo_sx m
          on inpatient.noofinpat = m.noofinpat
        left join iem_mainpage_operation_sx o
          on m.iem_mainpage_no = o.iem_mainpage_no
        left join Iem_Mainpage_Feeinfozy f
          on inpatient.noofinpat=f.noofinpat
        left join users u
          on u.id = inpatient.resident
       WHERE inpatient.outhosdept LIKE '%' || v_deptid || '%'
         and (m.valide = '1' or m.valide is null or m.valide = '') --排除无效数据)
         and (o.valide = '1' or o.valide is null or o.valide = '')
         and (f.valid = '1' or f.valid is null or f.valid = '')
         AND inpatient.patid LIKE '%' || v_patid || '%'
         AND inpatient.NAME LIKE '%' || v_name || '%'
         AND inpatient.status LIKE '%' || v_status || '%'
         AND (inpatient.admitdate >= p_admitbegindate AND
             inpatient.admitdate <= p_admitenddate);
  END;



  
  
  PROCEDURE usp_getpatientlistandpatSZ(v_deptid         VARCHAR2,
                                       v_patid          VARCHAR2,
                                       v_name           VARCHAR2,
                                       v_status         VARCHAR2,
                                       v_admitbegindate VARCHAR2,
                                       v_admitenddate   VARCHAR2,
                                       o_result         OUT empcurtype) AS
    p_admitbegindate VARCHAR2(19);
    p_admitenddate   VARCHAR2(19);
  BEGIN
    p_admitbegindate := v_admitbegindate || ' 00:00:00';
    p_admitenddate   := v_admitenddate || ' 23:59:59';
  
    IF v_admitbegindate = '' THEN
      p_admitbegindate := '2001-01-01 00:00:00';
    END IF;
  
    IF v_admitenddate = '' THEN
      p_admitenddate := '2999-01-01 00:00:00';
    END IF;
  
    OPEN o_result FOR
      SELECT distinct inpatient.outhosdept deptno,
                      department.NAME      deptname,
                      inpatient.patid,
                      inpatient.NAME,
                      inpatient.noofinpat,
                      inpatient.admitdate,
                      inpatient.status,
                      m.zg_flag,
                      m.outhostype, -- add by cyq 2013-04-30
                      --o.close_level,       edit by wangj 2013 3 6 一个病人此记录不止一条
                      o.iem_mainpage_no,
                      u.name            DOCTORNAME,
                      f.bloodfee
        FROM inpatient
        LEFT OUTER JOIN department
          ON department.ID = inpatient.outhosdept
        left join iem_mainpage_basicinfo_2012 m
          on inpatient.noofinpat = m.noofinpat
        left join iem_mainpage_operation_2012 o
          on m.iem_mainpage_no = o.iem_mainpage_no
        left join iem_mainpage_feeinfo f
          on m.iem_mainpage_no = f.iem_mainpage_no
        left join doctor_assignpatient da
          on da.noofinpat = inpatient.noofinpat
        left join users u
          on u.id = da.create_user
       WHERE inpatient.outhosdept LIKE '%' || v_deptid || '%'
         and (m.valide = '1' or m.valide is null or m.valide = '') --排除无效数据)
         and (o.valide = '1' or o.valide is null or o.valide = '')
         and (f.valid = '1' or f.valid is null or f.valid = '')
         AND inpatient.patid LIKE '%' || v_patid || '%'
         AND inpatient.NAME LIKE '%' || v_name || '%'
         AND inpatient.status LIKE '%' || v_status || '%'
         AND (inpatient.admitdate >= p_admitbegindate AND
             inpatient.admitdate <= p_admitenddate);
  END;
  
  
  

  
  PROCEDURE usp_getpatientlistandpatSZ_ZY(v_deptid         VARCHAR2,
                                       v_patid          VARCHAR2,
                                       v_name           VARCHAR2,
                                       v_status         VARCHAR2,
                                       v_admitbegindate VARCHAR2,
                                       v_admitenddate   VARCHAR2,
                                       o_result         OUT empcurtype) AS
    p_admitbegindate VARCHAR2(19);
    p_admitenddate   VARCHAR2(19);
  BEGIN
    p_admitbegindate := v_admitbegindate || ' 00:00:00';
    p_admitenddate   := v_admitenddate || ' 23:59:59';
  
    IF v_admitbegindate = '' THEN
      p_admitbegindate := '2001-01-01 00:00:00';
    END IF;
  
    IF v_admitenddate = '' THEN
      p_admitenddate := '2999-01-01 00:00:00';
    END IF;
  
    OPEN o_result FOR
      SELECT distinct inpatient.outhosdept deptno,
                      department.NAME      deptname,
                      inpatient.patid,
                      inpatient.NAME,
                      inpatient.noofinpat,
                      inpatient.admitdate,
                      inpatient.status,
                      '' as zg_flag,
                      m.outhostype, -- add by cyq 2013-04-30
                      --o.close_level,       edit by wangj 2013 3 6 一个病人此记录不止一条
                      o.iem_mainpage_no,
                      u.name            DOCTORNAME,
                      f.bloodfee
        FROM inpatient
        LEFT OUTER JOIN department
          ON department.ID = inpatient.outhosdept
        left join iem_mainpage_basicinfo_sx m
          on inpatient.noofinpat = m.noofinpat
        left join iem_mainpage_operation_sx o
          on m.iem_mainpage_no = o.iem_mainpage_no
        left join iem_mainpage_feeinfozy f
          on inpatient.noofinpat=f.noofinpat
        left join doctor_assignpatient da
          on da.noofinpat = inpatient.noofinpat
        left join users u
          on u.id = da.create_user
       WHERE inpatient.outhosdept LIKE '%' || v_deptid || '%'
         and (m.valide = '1' or m.valide is null or m.valide = '') --排除无效数据)
         and (o.valide = '1' or o.valide is null or o.valide = '')
         and (f.valid = '1' or f.valid is null or f.valid = '')
         AND inpatient.patid LIKE '%' || v_patid || '%'
         AND inpatient.NAME LIKE '%' || v_name || '%'
         AND inpatient.status LIKE '%' || v_status || '%'
         AND (inpatient.admitdate >= p_admitbegindate AND
             inpatient.admitdate <= p_admitenddate);
  END;



  PROCEDURE usp_getalldepartmentstatinfo(v_deptid         VARCHAR2,
                                         v_patid          VARCHAR2,
                                         v_name           VARCHAR2,
                                         v_status         VARCHAR2,
                                         v_admitbegindate VARCHAR2,
                                         v_admitenddate   VARCHAR2,
                                         o_result         OUT empcurtype) AS
    p_admitbegindate VARCHAR2(19);
    p_admitenddate   VARCHAR2(19);
  BEGIN
    p_admitbegindate := v_admitbegindate || ' 00:00:00';
    p_admitenddate   := v_admitenddate || ' 23:59:59';
  
    IF v_admitbegindate = '' THEN
      p_admitbegindate := '2001-01-01 00:00:00';
    END IF;
  
    IF v_admitenddate = '' THEN
      p_admitenddate := '2999-01-01 00:00:00';
    END IF;
    --科室和人都为空
    if v_deptid is null and v_name is null then
      OPEN o_result FOR
      /*SELECT   a.deptno, a.deptname,
      
                                           --床位数
                                           (SELECT COUNT (*)
                                              FROM bed b
                                             WHERE b.deptid = a.deptno AND b.valid = '1') totalbed,
      
                                           --病人数
                                           COUNT (a.noofinpat) totalpat,
      
                                           --未写病历
                                           SUM (a.emrnonwritecount) emrnonwritecount,
      
                                           --未提交
                                           SUM (a.emrhaswritecount) emrhaswritecount,
      
                                           --提交未审核完病历
                                           SUM (a.hassubmitnoauditcount) hassubmitnoauditcount,
      
                                           --已审核完病历
                                           SUM (a.hasauditcount) hasauditcount
                                      FROM (SELECT inpatient.outhosdept deptno,
                                                   department.NAME deptname, inpatient.patid,
                                                   inpatient.NAME, inpatient.noofinpat,
                                                   (SELECT COUNT (*)
                                                      FROM qcrecord
                                                     WHERE qcrecord.noofinpat =
                                                                 inpatient.noofinpat
                                                       AND qcrecord.valid != '0'
                                                       AND qcrecord.foulstate IN (1, 3))
                                                                                     emrnonwritecount,
                                                   (SELECT COUNT (*)
                                                      FROM recorddetail
                                                     WHERE recorddetail.noofinpat =
                                                                 inpatient.noofinpat
                                                       AND recorddetail.valid = '1'
                                                       AND recorddetail.hassubmit = '4600')
                                                                                     emrhaswritecount,
                                                   (SELECT COUNT (*)
                                                      FROM recorddetail
                                                     WHERE recorddetail.noofinpat =
                                                              inpatient.noofinpat
                                                       AND recorddetail.valid = '1'
                                                       AND recorddetail.hassubmit NOT IN
                                                                                     ('4600', '4603'))
                                                                                hassubmitnoauditcount,
                                                   (SELECT COUNT (*)
                                                      FROM recorddetail
                                                     WHERE recorddetail.noofinpat =
                                                                    inpatient.noofinpat
                                                       AND recorddetail.valid = '1'
                                                       AND recorddetail.hassubmit = '4603')
                                                                                        hasauditcount
                                              FROM inpatient LEFT OUTER JOIN department ON department.ID =
                                                                                             inpatient.outhosdept
                                                                                      AND department.valid =
                                                                                                   '1'
                                             WHERE inpatient.outhosdept LIKE '%' || v_deptid || '%'
                                               AND inpatient.patid LIKE '%' || v_patid || '%'
                                               AND inpatient.NAME LIKE '%' || v_name || '%'
                                               AND inpatient.status LIKE '%' || v_status || '%'
                                               AND (    inpatient.admitdate >= p_admitbegindate
                                                    AND inpatient.admitdate <= p_admitenddate
                                                   )) a
                                  GROUP BY a.deptno, a.deptname
                                  ORDER BY a.deptname;*/
        SELECT d.id deptno,
               '      ' || d.name deptname,
               (SELECT COUNT(*)
                  FROM bed b
                 WHERE b.deptid = d.id
                   AND b.valid = '1') totalbed, --床位数
               COUNT(distinct i.noofinpat) totalpat, --病人数
               0 emrnonwritecount, --未写病历
               sum(decode(r.hassubmit, '4600', 1, 0)) emrhaswritecount, --未提交
               sum(decode(r.hassubmit, '4600', 0, '4603', 0, 1)) hassubmitnoauditcount, --提交未审核完病历
               sum(decode(r.hassubmit, '4603', 1, 0)) hasauditcount --已审核完病历
          FROM department d
          LEFT OUTER JOIN inpatient i
            ON d.ID = i.outhosdept
           AND i.patid LIKE '%' || v_patid || '%'
           AND i.NAME LIKE '%' || v_name || '%'
           AND i.status LIKE '%' || v_status || '%'
           AND (i.admitdate >= p_admitbegindate AND
               i.admitdate <= p_admitenddate)
           AND i.outhosdept LIKE '%' || v_deptid || '%'
          LEFT OUTER JOIN recorddetail r
            ON r.noofinpat = i.noofinpat
           AND r.valid = '1'
         WHERE d.valid = '1'
           AND d.sort = '101' --and d.id =v_deptid --临床科室--按科室，统计信息不准确
         GROUP BY d.id, d.name;
      --科室不为空，人为空
    elsif v_deptid is not null and v_name is null then
      OPEN o_result FOR
        SELECT d.id deptno,
               '      ' || d.name deptname,
               (SELECT COUNT(*)
                  FROM bed b
                 WHERE b.deptid = d.id
                   AND b.valid = '1') totalbed, --床位数
               COUNT(distinct i.noofinpat) totalpat, --病人数
               0 emrnonwritecount, --未写病历
               sum(decode(r.hassubmit, '4600', 1, 0)) emrhaswritecount, --未提交
               sum(decode(r.hassubmit, '4600', 0, '4603', 0, 1)) hassubmitnoauditcount, --提交未审核完病历
               sum(decode(r.hassubmit, '4603', 1, 0)) hasauditcount --已审核完病历
          FROM department d
          LEFT OUTER JOIN inpatient i
            ON d.ID = i.outhosdept
           AND i.patid LIKE '%' || v_patid || '%'
           AND i.NAME LIKE '%' || v_name || '%'
           AND i.status LIKE '%' || v_status || '%'
           AND (i.admitdate >= p_admitbegindate AND
               i.admitdate <= p_admitenddate)
           AND i.outhosdept LIKE '%' || v_deptid || '%'
          LEFT OUTER JOIN recorddetail r
            ON r.noofinpat = i.noofinpat
           AND r.valid = '1'
         WHERE d.valid = '1'
           AND d.sort = '101'
           and d.id = v_deptid --临床科室--按科室，统计信息不准确
         GROUP BY d.id, d.name;
      --科室为空，人不为空的情况
    elsif v_deptid is null and v_name is not null then
      OPEN o_result FOR
        SELECT d.id deptno,
               '      ' || d.name deptname,
               (SELECT COUNT(*)
                  FROM bed b
                 WHERE b.deptid = d.id
                   AND b.valid = '1') totalbed, --床位数
               COUNT(distinct i.noofinpat) totalpat, --病人数
               0 emrnonwritecount, --未写病历
               sum(decode(r.hassubmit, '4600', 1, 0)) emrhaswritecount, --未提交
               sum(decode(r.hassubmit, '4600', 0, '4603', 0, 1)) hassubmitnoauditcount, --提交未审核完病历
               sum(decode(r.hassubmit, '4603', 1, 0)) hasauditcount --已审核完病历
          FROM department d
          LEFT OUTER JOIN inpatient i
            ON d.ID = i.outhosdept
           AND i.patid LIKE '%' || v_patid || '%'
           AND i.NAME LIKE '%' || v_name || '%'
           AND i.status LIKE '%' || v_status || '%'
           AND (i.admitdate >= p_admitbegindate AND
               i.admitdate <= p_admitenddate)
           AND i.outhosdept LIKE '%' || v_deptid || '%'
          LEFT OUTER JOIN recorddetail r
            ON r.noofinpat = i.noofinpat
           AND r.valid = '1'
         WHERE d.valid = '1'
           AND d.sort = '101'
           and i.name like '%' || v_name || '%' --临床科室--按科室，统计信息不准确
         GROUP BY d.id, d.name;
      --科室人都不为空
    elsif v_deptid is not null and v_name is not null then
      OPEN o_result FOR
        SELECT d.id deptno,
               '      ' || d.name deptname,
               (SELECT COUNT(*)
                  FROM bed b
                 WHERE b.deptid = d.id
                   AND b.valid = '1') totalbed, --床位数
               COUNT(distinct i.noofinpat) totalpat, --病人数
               0 emrnonwritecount, --未写病历
               sum(decode(r.hassubmit, '4600', 1, 0)) emrhaswritecount, --未提交
               sum(decode(r.hassubmit, '4600', 0, '4603', 0, 1)) hassubmitnoauditcount, --提交未审核完病历
               sum(decode(r.hassubmit, '4603', 1, 0)) hasauditcount --已审核完病历
          FROM department d
          LEFT OUTER JOIN inpatient i
            ON d.ID = i.outhosdept
           AND i.patid LIKE '%' || v_patid || '%'
           AND i.NAME LIKE '%' || v_name || '%'
           AND i.status LIKE '%' || v_status || '%'
           AND (i.admitdate >= p_admitbegindate AND
               i.admitdate <= p_admitenddate)
           AND i.outhosdept LIKE '%' || v_deptid || '%'
          LEFT OUTER JOIN recorddetail r
            ON r.noofinpat = i.noofinpat
           AND r.valid = '1'
         WHERE d.valid = '1'
           AND d.sort = '101'
           and d.id = v_deptid
           and i.name like '%' || v_name || '%'
           and d.id = v_deptid --临床科室--按科室，统计信息不准确
         GROUP BY d.id, d.name;
    end if;
  END;

  --仁和 获取科室统计信息 通过病人状态获取
  PROCEDURE usp_RHgetalldepstatinfo(v_deptid         VARCHAR2,
                                    v_patid          VARCHAR2,
                                    v_name           VARCHAR2,
                                    v_status         VARCHAR2,
                                    v_admitbegindate VARCHAR2,
                                    v_admitenddate   VARCHAR2,
                                    o_result         OUT empcurtype) AS
    p_admitbegindate VARCHAR2(19);
    p_admitenddate   VARCHAR2(19);
  BEGIN
    p_admitbegindate := v_admitbegindate || ' 00:00:00';
    p_admitenddate   := v_admitenddate || ' 23:59:59';
  
    IF v_admitbegindate = '' THEN
      p_admitbegindate := '2001-01-01 00:00:00';
    END IF;
  
    IF v_admitenddate = '' THEN
      p_admitenddate := '2999-01-01 00:00:00';
    END IF;
    --科室和人都为空
    if v_deptid is null and v_name is null then
      OPEN o_result FOR
        SELECT d.id deptno,
               '      ' || d.name deptname,
               (SELECT COUNT(*)
                  FROM bed b
                 WHERE b.deptid = d.id
                   AND b.valid = '1') totalbed, --床位数
               COUNT(distinct i.noofinpat) totalpat, --病人数
               0 emrnonwritecount, --未写病历
               sum(decode(r.hassubmit, '4600', 1, 0)) emrhaswritecount, --未提交
               sum(decode(r.hassubmit, '4600', 0, '4603', 0, 1)) hassubmitnoauditcount, --提交未审核完病历
               sum(decode(r.hassubmit, '4603', 1, 0)) hasauditcount --已审核完病历
          FROM department d
          LEFT OUTER JOIN inpatient i
            ON d.ID = i.outhosdept
           AND i.patid LIKE '%' || v_patid || '%'
           AND i.NAME LIKE '%' || v_name || '%'
           AND i.status LIKE '%' || v_status || '%'
           AND (i.admitdate >= p_admitbegindate AND
               i.admitdate <= p_admitenddate)
           AND i.outhosdept LIKE '%' || v_deptid || '%'
          LEFT OUTER JOIN recorddetail r
            ON r.noofinpat = i.noofinpat
           AND r.valid = '1'
         WHERE d.valid = '1'
           AND d.sort = '101' --and d.id =v_deptid --临床科室--按科室，统计信息不准确
         GROUP BY d.id, d.name;
      --科室不为空，人为空
    elsif v_deptid is not null and v_name is null then
      OPEN o_result FOR
        SELECT d.id deptno,
               '      ' || d.name deptname,
               (SELECT COUNT(*)
                  FROM bed b
                 WHERE b.deptid = d.id
                   AND b.valid = '1') totalbed, --床位数
               COUNT(distinct i.noofinpat) totalpat, --病人数
               0 emrnonwritecount, --未写病历
               sum(decode(r.hassubmit, '4600', 1, 0)) emrhaswritecount, --未提交
               sum(decode(r.hassubmit, '4600', 0, '4603', 0, 1)) hassubmitnoauditcount, --提交未审核完病历
               sum(decode(r.hassubmit, '4603', 1, 0)) hasauditcount --已审核完病历
          FROM department d
          LEFT OUTER JOIN inpatient i
            ON d.ID = i.outhosdept
           AND i.patid LIKE '%' || v_patid || '%'
           AND i.NAME LIKE '%' || v_name || '%'
           AND i.status LIKE '%' || v_status || '%'
           AND (i.admitdate >= p_admitbegindate AND
               i.admitdate <= p_admitenddate)
           AND i.outhosdept LIKE '%' || v_deptid || '%'
          LEFT OUTER JOIN recorddetail r
            ON r.noofinpat = i.noofinpat
           AND r.valid = '1'
         WHERE d.valid = '1'
           AND d.sort = '101'
           and d.id = v_deptid --临床科室--按科室，统计信息不准确
         GROUP BY d.id, d.name;
      --科室为空，人不为空的情况
    elsif v_deptid is null and v_name is not null then
      OPEN o_result FOR
        SELECT d.id deptno,
               '      ' || d.name deptname,
               (SELECT COUNT(*)
                  FROM bed b
                 WHERE b.deptid = d.id
                   AND b.valid = '1') totalbed, --床位数
               COUNT(distinct i.noofinpat) totalpat, --病人数
               0 emrnonwritecount, --未写病历
               sum(decode(r.hassubmit, '4600', 1, 0)) emrhaswritecount, --未提交
               sum(decode(r.hassubmit, '4600', 0, '4603', 0, 1)) hassubmitnoauditcount, --提交未审核完病历
               sum(decode(r.hassubmit, '4603', 1, 0)) hasauditcount --已审核完病历
          FROM department d
          LEFT OUTER JOIN inpatient i
            ON d.ID = i.outhosdept
           AND i.patid LIKE '%' || v_patid || '%'
           AND i.NAME LIKE '%' || v_name || '%'
           AND i.status LIKE '%' || v_status || '%'
           AND (i.admitdate >= p_admitbegindate AND
               i.admitdate <= p_admitenddate)
           AND i.outhosdept LIKE '%' || v_deptid || '%'
          LEFT OUTER JOIN recorddetail r
            ON r.noofinpat = i.noofinpat
           AND r.valid = '1'
         WHERE d.valid = '1'
           AND d.sort = '101'
           and i.name like '%' || v_name || '%' --临床科室--按科室，统计信息不准确
         GROUP BY d.id, d.name;
      --科室人都不为空
    elsif v_deptid is not null and v_name is not null then
      OPEN o_result FOR
        SELECT d.id deptno,
               '      ' || d.name deptname,
               (SELECT COUNT(*)
                  FROM bed b
                 WHERE b.deptid = d.id
                   AND b.valid = '1') totalbed, --床位数
               COUNT(distinct i.noofinpat) totalpat, --病人数
               0 emrnonwritecount, --未写病历
               sum(decode(r.hassubmit, '4600', 1, 0)) emrhaswritecount, --未提交
               sum(decode(r.hassubmit, '4600', 0, '4603', 0, 1)) hassubmitnoauditcount, --提交未审核完病历
               sum(decode(r.hassubmit, '4603', 1, 0)) hasauditcount --已审核完病历
          FROM department d
          LEFT OUTER JOIN inpatient i
            ON d.ID = i.outhosdept
           AND i.patid LIKE '%' || v_patid || '%'
           AND i.NAME LIKE '%' || v_name || '%'
           AND i.status LIKE '%' || v_status || '%'
           AND (i.admitdate >= p_admitbegindate AND
               i.admitdate <= p_admitenddate)
           AND i.outhosdept LIKE '%' || v_deptid || '%'
          LEFT OUTER JOIN recorddetail r
            ON r.noofinpat = i.noofinpat
           AND r.valid = '1'
         WHERE d.valid = '1'
           AND d.sort = '101'
           and d.id = v_deptid
           and i.name like '%' || v_name || '%'
           and d.id = v_deptid --临床科室--按科室，统计信息不准确
         GROUP BY d.id, d.name;
    end if;
  END;

  --仁和 获取科室统计信息 住院和出院病人汇总
  PROCEDURE usp_RHgetDepstatinfoAll(v_deptid         VARCHAR2,
                                    v_patid          VARCHAR2,
                                    v_name           VARCHAR2,
                                    v_admitbegindate VARCHAR2,
                                    v_admitenddate   VARCHAR2,
                                    o_result         OUT empcurtype) AS
    p_admitbegindate VARCHAR2(19);
    p_admitenddate   VARCHAR2(19);
  BEGIN
    p_admitbegindate := v_admitbegindate || ' 00:00:00';
    p_admitenddate   := v_admitenddate || ' 23:59:59';
  
    IF v_admitbegindate = '' THEN
      p_admitbegindate := '2001-01-01 00:00:00';
    END IF;
  
    IF v_admitenddate = '' THEN
      p_admitenddate := '2999-01-01 00:00:00';
    END IF;
    --科室和人都为空
    if v_deptid is null and v_name is null then
      OPEN o_result FOR
        SELECT d.id deptno,
               '      ' || d.name deptname,
               (SELECT COUNT(*)
                  FROM bed b
                 WHERE b.deptid = d.id
                   AND b.valid = '1') totalbed, --床位数
               COUNT(distinct i.noofinpat) totalpat, --病人数
               0 emrnonwritecount, --未写病历
               sum(decode(r.hassubmit, '4600', 1, 0)) emrhaswritecount, --未提交
               sum(decode(r.hassubmit, '4600', 0, '4603', 0, 1)) hassubmitnoauditcount, --提交未审核完病历
               sum(decode(r.hassubmit, '4603', 1, 0)) hasauditcount --已审核完病历
          FROM department d
          LEFT OUTER JOIN inpatient i
            ON d.ID = i.outhosdept
           AND i.patid LIKE '%' || v_patid || '%'
           AND i.NAME LIKE '%' || v_name || '%'
           AND i.status in ('1501', '1502', '1503')
           AND (i.admitdate >= p_admitbegindate AND
               i.admitdate <= p_admitenddate)
           AND i.outhosdept LIKE '%' || v_deptid || '%'
          LEFT OUTER JOIN recorddetail r
            ON r.noofinpat = i.noofinpat
           AND r.valid = '1'
         WHERE d.valid = '1'
           AND d.sort = '101' --and d.id =v_deptid --临床科室--按科室，统计信息不准确
         GROUP BY d.id, d.name;
      --科室不为空，人为空
    elsif v_deptid is not null and v_name is null then
      OPEN o_result FOR
        SELECT d.id deptno,
               '      ' || d.name deptname,
               (SELECT COUNT(*)
                  FROM bed b
                 WHERE b.deptid = d.id
                   AND b.valid = '1') totalbed, --床位数
               COUNT(distinct i.noofinpat) totalpat, --病人数
               0 emrnonwritecount, --未写病历
               sum(decode(r.hassubmit, '4600', 1, 0)) emrhaswritecount, --未提交
               sum(decode(r.hassubmit, '4600', 0, '4603', 0, 1)) hassubmitnoauditcount, --提交未审核完病历
               sum(decode(r.hassubmit, '4603', 1, 0)) hasauditcount --已审核完病历
          FROM department d
          LEFT OUTER JOIN inpatient i
            ON d.ID = i.outhosdept
           AND i.patid LIKE '%' || v_patid || '%'
           AND i.NAME LIKE '%' || v_name || '%'
           AND i.status in ('1501', '1502', '1503')
           AND (i.admitdate >= p_admitbegindate AND
               i.admitdate <= p_admitenddate)
           AND i.outhosdept LIKE '%' || v_deptid || '%'
          LEFT OUTER JOIN recorddetail r
            ON r.noofinpat = i.noofinpat
           AND r.valid = '1'
         WHERE d.valid = '1'
           AND d.sort = '101'
           and d.id = v_deptid --临床科室--按科室，统计信息不准确
         GROUP BY d.id, d.name;
      --科室为空，人不为空的情况
    elsif v_deptid is null and v_name is not null then
      OPEN o_result FOR
        SELECT d.id deptno,
               '      ' || d.name deptname,
               (SELECT COUNT(*)
                  FROM bed b
                 WHERE b.deptid = d.id
                   AND b.valid = '1') totalbed, --床位数
               COUNT(distinct i.noofinpat) totalpat, --病人数
               0 emrnonwritecount, --未写病历
               sum(decode(r.hassubmit, '4600', 1, 0)) emrhaswritecount, --未提交
               sum(decode(r.hassubmit, '4600', 0, '4603', 0, 1)) hassubmitnoauditcount, --提交未审核完病历
               sum(decode(r.hassubmit, '4603', 1, 0)) hasauditcount --已审核完病历
          FROM department d
          LEFT OUTER JOIN inpatient i
            ON d.ID = i.outhosdept
           AND i.patid LIKE '%' || v_patid || '%'
           AND i.NAME LIKE '%' || v_name || '%'
           AND i.status in ('1501', '1502', '1503')
           AND (i.admitdate >= p_admitbegindate AND
               i.admitdate <= p_admitenddate)
           AND i.outhosdept LIKE '%' || v_deptid || '%'
          LEFT OUTER JOIN recorddetail r
            ON r.noofinpat = i.noofinpat
           AND r.valid = '1'
         WHERE d.valid = '1'
           AND d.sort = '101'
           and i.name like '%' || v_name || '%' --临床科室--按科室，统计信息不准确
         GROUP BY d.id, d.name;
      --科室人都不为空
    elsif v_deptid is not null and v_name is not null then
      OPEN o_result FOR
        SELECT d.id deptno,
               '      ' || d.name deptname,
               (SELECT COUNT(*)
                  FROM bed b
                 WHERE b.deptid = d.id
                   AND b.valid = '1') totalbed, --床位数
               COUNT(distinct i.noofinpat) totalpat, --病人数
               0 emrnonwritecount, --未写病历
               sum(decode(r.hassubmit, '4600', 1, 0)) emrhaswritecount, --未提交
               sum(decode(r.hassubmit, '4600', 0, '4603', 0, 1)) hassubmitnoauditcount, --提交未审核完病历
               sum(decode(r.hassubmit, '4603', 1, 0)) hasauditcount --已审核完病历
          FROM department d
          LEFT OUTER JOIN inpatient i
            ON d.ID = i.outhosdept
           AND i.patid LIKE '%' || v_patid || '%'
           AND i.NAME LIKE '%' || v_name || '%'
           AND i.status in ('1501', '1502', '1503')
           AND (i.admitdate >= p_admitbegindate AND
               i.admitdate <= p_admitenddate)
           AND i.outhosdept LIKE '%' || v_deptid || '%'
          LEFT OUTER JOIN recorddetail r
            ON r.noofinpat = i.noofinpat
           AND r.valid = '1'
         WHERE d.valid = '1'
           AND d.sort = '101'
           and d.id = v_deptid
           and i.name like '%' || v_name || '%'
           and d.id = v_deptid --临床科室--按科室，统计信息不准确
         GROUP BY d.id, d.name;
    end if;
  END;

  --统计科室的病历评分信息 add by wyt 2012-12-12
  procedure usp_getdepartmentpatqcinfo(v_deptid         VARCHAR2,
                                       v_patid          VARCHAR2,
                                       v_name           VARCHAR2,
                                       v_status         VARCHAR2,
                                       v_admitbegindate VARCHAR2,
                                       v_admitenddate   VARCHAR2,
                                       v_sortid         varchar2,
                                       v_sumpoint1      int,
                                       v_sumpoint2      int,
                                       V_type           varchar2,
                                       V_userid         varchar2,
                                       V_auth           varchar2,
                                       o_result         OUT empcurtype) as
    p_admitbegindate VARCHAR2(19);
    p_admitenddate   VARCHAR2(19);
    p_userid         varchar2(16);
  begin
    p_admitbegindate := v_admitbegindate || ' 00:00:00';
    p_admitenddate   := v_admitenddate || ' 23:59:59';
  
    IF v_admitbegindate = '' THEN
      p_admitbegindate := '2001-01-01 00:00:00';
    END IF;
  
    IF v_admitenddate = '' THEN
      p_admitenddate := '2999-01-01 00:00:00';
    END IF;
  
    IF V_auth = '2' THEN
      p_userid := '';
    ELSE
      p_userid := V_userid;
    END IF;
  
    OPEN o_result FOR
      select record.id,
             record.name,
             record.noofinpat,
             record.qctype,
             record.checkstate,
             pat.PATID,
             pat.outbed bedid,
             (pat.name || '(编号' || pat.noofinpat || ')') patname,
             (case pat.sexid
               when '0' then
                '女'
               when '1' then
                '男'
             end) sexname,
             pat.age,
             pat.status,
             DECODE(pat.incount, 1, pat.incount, 0, 1) incount,
             dept.id deptno,
             dept.name deptname,
             diag.NAME admitdiagnosis,
             users.name createusername,
             p.doctorname,
             p.recorddetailname,
             p.sortid,
             p.emrpointid,
             p.problem_desc,
             (case record.qctype
               when '环节质控' then
                (v_sumpoint1 - p1.sumpoint)
               when '终末质控' then
                (v_sumpoint2 - p1.sumpoint)
             end) totalscore,
             (case record.qctype
               when '环节质控' then
                v_sumpoint1
               when '终末质控' then
                v_sumpoint2
             end) sumpoint,
             dict.cname,
             config.childname
        from (select id,
                     (name || ' ' ||
                     to_char(create_time, 'YYYY-MM-DD HH24:MI:SS')) name,
                     noofinpat,
                     (case qctype
                       when '0' then
                        '环节质控'
                       when '1' then
                        '终末质控'
                     end) qctype,
                     (case checkstate
                       when '0' then
                        '新建'
                       when '1' then
                        '提交未审核'
                       when '2' then
                        '审核通过'
                       when '3' then
                        '审核未通过'
                       when '4' then
                        '质控员质控'
                     end) checkstate,
                     CREATE_USER
                from emr_automark_record
               where isauto = '0'
                 and isvalid = '1') record
        left join inpatient pat
          on pat.noofinpat = record.noofinpat
        left join department dept
          on dept.id = pat.outhosdept
        left join diagnosis diag
          on diag.icd = REPLACE(pat.admitdiagnosis, ' ', '.')
         and diag.valid = '1'
        left join users
          on users.id = record.CREATE_USER
         and users.valid = 1
        left join emr_point p
          on p.emr_mark_record_id = record.id
         and p.valid = '1'
        left join (select emr_mark_record_id, sum(reducepoint) sumpoint
                     from emr_point
                    where Valid = '1'
                    group by emr_mark_record_id) p1
          on p1.emr_mark_record_id = record.id
        left join emr_configpoint config
          on config.childcode = to_char(p.emrpointid)
        left join dict_catalog dict
          on dict.ccode = p.sortid
       where record.create_user like '%' || p_userid || '%'
         and pat.patid LIKE '%' || v_patid || '%'
         and pat.outhosdept LIKE '%' || v_deptid || '%'
         and pat.NAME LIKE '%' || v_name || '%'
         and pat.status LIKE '%' || v_status || '%'
         and pat.admitdate >= p_admitbegindate
         and pat.admitdate <= p_admitenddate
         and p.sortid like '%' || v_sortid || '%';
  END;

  ---获得科室内病人，（在评分表中存在记录的病人）--edit by wyt 2012-10-26 增加了按扣分项搜索
  PROCEDURE usp_getdepartmentpatstatinfo(v_deptid         VARCHAR2,
                                         v_patid          VARCHAR2,
                                         v_name           VARCHAR2,
                                         v_status         VARCHAR2,
                                         v_admitbegindate VARCHAR2,
                                         v_admitenddate   VARCHAR2,
                                         v_sortid         varchar2,
                                         v_sumpoint       int, --新增的总分 ywk 2012年6月12日 14:51:41
                                         V_type           varchar2, --新增的按类型查询 ywk 2012年11月6日10:55:58
                                         o_result         OUT empcurtype) AS
    p_admitbegindate VARCHAR2(19);
    p_admitenddate   VARCHAR2(19);
  BEGIN
    p_admitbegindate := v_admitbegindate || ' 00:00:00';
    p_admitenddate   := v_admitenddate || ' 23:59:59';
  
    IF v_admitbegindate = '' THEN
      p_admitbegindate := '2001-01-01 00:00:00';
    END IF;
  
    IF v_admitenddate = '' THEN
      p_admitenddate := '2999-01-01 00:00:00';
    END IF;
    --病历评分表中的查询
    if V_type = 'point' then
    
      OPEN o_result FOR
        select temp2.bedid,
               temp2.deptno,
               temp2.deptname,
               temp2.patid,
               temp2.patname,
               temp2.noofinpat,
               temp2.sexname,
               temp2.age,
               temp2.incount,
               temp2.admitdiagnosis,
               temp2.emrnonwritecount,
               temp2.emrhaswritecount,
               temp2.hassubmitnoauditcount,
               temp2.hasauditcount,
               temp2.totalscore,
               pt2.sortid,
               dict.cname,
               configp.childname
          from (SELECT temp1.bedid,
                       temp1.deptno,
                       temp1.deptname,
                       temp1.patid,
                       temp1.patname,
                       temp1.noofinpat,
                       temp1.sexname,
                       temp1.age,
                       temp1.incount,
                       temp1.admitdiagnosis,
                       temp1.emrnonwritecount,
                       temp1.emrhaswritecount,
                       temp1.hassubmitnoauditcount,
                       temp1.hasauditcount,
                       v_sumpoint - SUM(pt.reducepoint) totalscore
                
                  from (select inp.outbed bedid,
                               inp.outhosdept deptno,
                               department.NAME deptname,
                               inp.patid,
                               inp.NAME patname,
                               inp.noofinpat,
                               dc.NAME sexname,
                               inp.agestr age,
                               DECODE(inp.incount, 1, inp.incount, 0, 1) incount,
                               diag.NAME admitdiagnosis,
                               '0' emrnonwritecount,
                               SUM(DECODE(rcd.hassubmit, '4600', 1, 0)) emrhaswritecount, --未写病历
                               SUM(DECODE(rcd.hassubmit,
                                          '4600',
                                          0,
                                          '4603',
                                          0,
                                          1)) hassubmitnoauditcount, --未提交
                               SUM(DECODE(rcd.hassubmit, '4603', 1, 0)) hasauditcount --已审核完病历
                          FROM inpatient inp
                          LEFT OUTER JOIN department
                            ON department.ID = inp.outhosdept
                           AND department.valid = '1'
                          LEFT OUTER JOIN dictionary_detail dc
                            ON dc.detailid = inp.sexid
                           AND dc.categoryid = '3'
                          LEFT OUTER JOIN diagnosis diag
                            ON diag.icd =
                               REPLACE(inp.admitdiagnosis, ' ', '.')
                           AND diag.valid = '1'
                          LEFT OUTER JOIN recorddetail rcd
                            ON rcd.noofinpat = inp.noofinpat
                           AND rcd.valid = '1'
                         WHERE inp.outhosdept LIKE '%' || v_deptid || '%'
                           AND inp.patid LIKE '%' || v_patid || '%'
                           AND inp.NAME LIKE '%' || v_name || '%'
                           AND inp.status LIKE '%' || v_status || '%'
                           AND (inp.admitdate >= p_admitbegindate AND
                               inp.admitdate <= p_admitenddate)
                         GROUP BY inp.outbed,
                                  inp.outhosdept,
                                  department.NAME,
                                  inp.patid,
                                  inp.NAME,
                                  inp.noofinpat,
                                  dc.NAME,
                                  inp.agestr,
                                  inp.incount,
                                  diag.NAME) temp1
                  left OUTER JOIN emr_point pt
                    ON temp1.noofinpat = pt.noofinpat
                 where pt.valid = '1'
                 group by temp1.bedid,
                          temp1.deptno,
                          temp1.deptname,
                          temp1.patid,
                          temp1.patname,
                          temp1.noofinpat,
                          temp1.sexname,
                          temp1.age,
                          temp1.incount,
                          temp1.admitdiagnosis,
                          temp1.emrnonwritecount,
                          temp1.emrhaswritecount,
                          temp1.hassubmitnoauditcount,
                          temp1.hasauditcount) temp2
          left join emr_point pt2
            on pt2.noofinpat = temp2.noofinpat
          left join Emr_ConfigPoint configp
            on configp.id = pt2.emrpointid
          left join dict_catalog dict
            on dict.ccode = pt2.sortid
         where dict.ccode like '%' || v_sortid || '%';
      --时限信息中的查询
    elsif v_type = 'time' then
      OPEN o_result FOR
      /* SELECT inp.outbed bedid,
                         inp.outhosdept deptno,
                         department.NAME deptname,
                         inp.patid,
                         inp.NAME patname,
                         inp.noofinpat,
                         dc.NAME sexname,
                         inp.agestr age,
                         DECODE(inp.incount, 1, inp.incount, 0, 1) incount,
                         diag.NAME admitdiagnosis,
                         '0' emrnonwritecount,
                         SUM(DECODE(rcd.hassubmit, '4600', 1, 0)) emrhaswritecount, --未写病历
                         SUM(DECODE(rcd.hassubmit, '4600', 0, '4603', 0, 1)) hassubmitnoauditcount, --未提交
                         SUM(DECODE(rcd.hassubmit, '4603', 1, 0)) hasauditcount, --已审核完病历
                         v_sumpoint - SUM(p.reducepoint) totalscore,
                         '' cname
                    FROM inpatient inp
                    LEFT OUTER JOIN department
                      ON department.ID = inp.outhosdept
                     AND department.valid = '1'
                    LEFT OUTER JOIN dictionary_detail dc
                      ON dc.detailid = inp.sexid
                     AND dc.categoryid = '3'
                    LEFT OUTER JOIN diagnosis diag
                      ON diag.icd = REPLACE(inp.admitdiagnosis, ' ', '.')
                     AND diag.valid = '1'
                    LEFT OUTER JOIN recorddetail rcd
                      ON rcd.noofinpat = inp.noofinpat
                     AND rcd.valid = '1'
                   LEFT OUTER JOIN emr_point p
                      ON inp.noofinpat = p.noofinpat  and p.valid = '1'
                   WHERE inp.outhosdept LIKE  '%' || v_deptid || '%'
                     AND inp.patid LIKE '%' || v_patid || '%'
                     AND inp.NAME LIKE '%' || v_name || '%'
                     AND inp.status LIKE '%' || v_status || '%'
                     AND (inp.admitdate >= p_admitbegindate AND
                         inp.admitdate <= p_admitenddate)
                   GROUP BY inp.outbed,inp.outhosdept,department.NAME,inp.patid,inp.NAME,
                            inp.noofinpat,dc.NAME,inp.agestr,inp.incount,diag.NAME;
            \**/
        SELECT a.bedid,
               a.deptno,
               a.deptname,
               a.patid,
               a.patname,
               a.noofinpat,
               a.sexname,
               a.age,
               a.incount,
               a.admitdiagnosis,
               a.emrnonwritecount,
               a.emrhaswritecount,
               a.hassubmitnoauditcount,
               a.hasauditcount,
               v_sumpoint - SUM(p.reducepoint) totalscore --总分数(从配置中取得)\*100 - SUM(p.reducepoint)*\
          FROM (SELECT i.outbed bedid,
                       i.outhosdept deptno,
                       department.NAME deptname,
                       i.patid,
                       i.NAME patname,
                       i.noofinpat,
                       c.NAME sexname,
                       i.agestr age,
                       DECODE(i.incount, 1, i.incount, 0, 1) incount,
                       d.NAME admitdiagnosis,
                       '0' emrnonwritecount,
                       SUM(DECODE(r.hassubmit, '4600', 1, 0)) emrhaswritecount, --未写病历
                       SUM(DECODE(r.hassubmit, '4600', 0, '4603', 0, 1)) hassubmitnoauditcount, --未提交
                       SUM(DECODE(r.hassubmit, '4603', 1, 0)) hasauditcount --已审核完病历
                  FROM inpatient i
                  LEFT OUTER JOIN department
                    ON department.ID = i.outhosdept
                   AND department.valid = '1'
                  LEFT OUTER JOIN dictionary_detail c
                    ON c.detailid = i.sexid
                   AND c.categoryid = '3'
                  LEFT OUTER JOIN diagnosis d
                    ON d.icd = REPLACE(i.admitdiagnosis, ' ', '.')
                   AND d.valid = '1'
                  LEFT OUTER JOIN recorddetail r
                    ON r.noofinpat = i.noofinpat
                   AND r.valid = '1'
                 WHERE i.outhosdept LIKE '%' || v_deptid || '%' --v_deptid
                   AND i.patid LIKE '%' || v_patid || '%'
                   AND i.NAME LIKE '%' || v_name || '%'
                   AND i.status LIKE '%' || v_status || '%'
                   AND (i.admitdate >= p_admitbegindate AND
                       i.admitdate <= p_admitenddate)
                 GROUP BY i.outbed,
                          i.outhosdept,
                          department.NAME,
                          i.patid,
                          i.NAME,
                          i.noofinpat,
                          c.NAME,
                          i.agestr,
                          i.incount,
                          d.NAME) a
          LEFT OUTER JOIN emr_point p
            ON a.noofinpat = p.noofinpat
         WHERE p.valid = '1'
         GROUP BY a.bedid,
                  a.deptno,
                  a.deptname,
                  a.patid,
                  a.patname,
                  a.noofinpat,
                  a.sexname,
                  a.age,
                  a.incount,
                  a.admitdiagnosis,
                  a.emrnonwritecount,
                  a.emrhaswritecount,
                  a.hassubmitnoauditcount,
                  a.hasauditcount;
    end if;
  END;

  ---仁和 获得科室内病人，（在评分表中存在记录的病人 并且记录状态是8701 科室主任）
  PROCEDURE usp_GetRHGetDeptinfo(v_deptid         VARCHAR2,
                                 v_patid          VARCHAR2,
                                 v_name           VARCHAR2,
                                 v_status         VARCHAR2,
                                 v_admitbegindate VARCHAR2,
                                 v_admitenddate   VARCHAR2,
                                 o_result         OUT empcurtype) AS
    p_admitbegindate VARCHAR2(19);
    p_admitenddate   VARCHAR2(19);
  BEGIN
    p_admitbegindate := v_admitbegindate || ' 00:00:00';
    p_admitenddate   := v_admitenddate || ' 23:59:59';
  
    IF v_admitbegindate = '' THEN
      p_admitbegindate := '2001-01-01 00:00:00';
    END IF;
  
    IF v_admitenddate = '' THEN
      p_admitenddate := '2999-01-01 00:00:00';
    END IF;
  
    OPEN o_result FOR
      SELECT a.bedid,
             a.deptno,
             a.deptname,
             a.patid,
             a.patname,
             a.noofinpat,
             a.sexname,
             a.age,
             a.incount,
             a.admitdiagnosis,
             a.emrnonwritecount,
             a.emrhaswritecount,
             a.hassubmitnoauditcount,
             a.hasauditcount
        FROM (SELECT i.outbed bedid,
                     i.outhosdept deptno,
                     department.NAME deptname,
                     i.patid,
                     i.NAME patname,
                     i.noofinpat,
                     c.NAME sexname,
                     i.agestr age,
                     DECODE(i.incount, 1, i.incount, 0, 1) incount,
                     d.NAME admitdiagnosis,
                     '0' emrnonwritecount,
                     SUM(DECODE(r.hassubmit, '4600', 1, 0)) emrhaswritecount, --未写病历
                     SUM(DECODE(r.hassubmit, '4600', 0, '4603', 0, 1)) hassubmitnoauditcount, --未提交
                     SUM(DECODE(r.hassubmit, '4603', 1, 0)) hasauditcount --已审核完病历
                FROM inpatient i
                LEFT OUTER JOIN department
                  ON department.ID = i.outhosdept
                 AND department.valid = '1'
                LEFT OUTER JOIN dictionary_detail c
                  ON c.detailid = i.sexid
                 AND c.categoryid = '3'
                LEFT OUTER JOIN diagnosis d
                  ON d.icd = REPLACE(i.admitdiagnosis, ' ', '.')
                 AND d.valid = '1'
                LEFT OUTER JOIN recorddetail r
                  ON r.noofinpat = i.noofinpat
                 AND r.valid = '1'
               WHERE i.outhosdept LIKE '%' || v_deptid || '%' --v_deptid
                 AND i.patid LIKE '%' || v_patid || '%'
                 AND i.NAME LIKE '%' || v_name || '%'
                 AND i.status LIKE '%' || v_status || '%'
                 AND (i.admitdate >= p_admitbegindate AND
                     i.admitdate <= p_admitenddate)
               GROUP BY i.outbed,
                        i.outhosdept,
                        department.NAME,
                        i.patid,
                        i.NAME,
                        i.noofinpat,
                        c.NAME,
                        i.agestr,
                        i.incount,
                        d.NAME) a
        LEFT OUTER JOIN emr_rhqc_table p
          ON a.noofinpat = p.noofinpat
       WHERE p.valid = '1'
         and p.stateid in ('8701', '8703')
       GROUP BY a.bedid,
                a.deptno,
                a.deptname,
                a.patid,
                a.patname,
                a.noofinpat,
                a.sexname,
                a.age,
                a.incount,
                a.admitdiagnosis,
                a.emrnonwritecount,
                a.emrhaswritecount,
                a.hassubmitnoauditcount,
                a.hasauditcount
       ORDER BY deptno, bedid;
  END;

  --得到病人的统计信息(仁和版本的，不连接评分表edit by ywk 2012年7月13日 08:58:34)
  procedure usp_GetrhDepartmentPatStatInfo(v_deptid         varchar2,
                                           v_patid          varchar2,
                                           v_name           varchar2,
                                           v_status         varchar2,
                                           v_admitbegindate varchar2,
                                           v_admitenddate   varchar2,
                                           o_result         OUT empcurtype) AS
    p_admitbegindate VARCHAR2(19);
    p_admitenddate   VARCHAR2(19);
  BEGIN
    p_admitbegindate := v_admitbegindate || ' 00:00:00';
    p_admitenddate   := v_admitenddate || ' 23:59:59';
  
    IF v_admitbegindate = '' THEN
      p_admitbegindate := '2001-01-01 00:00:00';
    END IF;
  
    IF v_admitenddate = '' THEN
      p_admitenddate := '2999-01-01 00:00:00';
    END IF;
  
    OPEN o_result FOR
      SELECT a.bedid,
             a.deptno,
             a.deptname,
             a.patid,
             a.patname,
             a.noofinpat,
             a.sexname,
             a.age,
             a.incount,
             a.admitdiagnosis,
             a.emrnonwritecount,
             a.emrhaswritecount,
             a.hassubmitnoauditcount,
             a.hasauditcount
        FROM (SELECT i.outbed bedid,
                     i.outhosdept deptno,
                     department.NAME deptname,
                     i.patid,
                     i.NAME patname,
                     i.noofinpat,
                     c.NAME sexname,
                     i.agestr age,
                     DECODE(i.incount, 1, i.incount, 0, 1) incount,
                     d.NAME admitdiagnosis,
                     '0' emrnonwritecount,
                     SUM(DECODE(r.hassubmit, '4600', 1, 0)) emrhaswritecount, --未写病历
                     SUM(DECODE(r.hassubmit, '4600', 0, '4603', 0, 1)) hassubmitnoauditcount, --未提交
                     SUM(DECODE(r.hassubmit, '4603', 1, 0)) hasauditcount --已审核完病历
                FROM inpatient i
                LEFT OUTER JOIN department
                  ON department.ID = i.outhosdept
                 AND department.valid = '1'
                LEFT OUTER JOIN dictionary_detail c
                  ON c.detailid = i.sexid
                 AND c.categoryid = '3'
                LEFT OUTER JOIN diagnosis d
                  ON d.icd = REPLACE(i.admitdiagnosis, ' ', '.')
                 AND d.valid = '1'
                LEFT OUTER JOIN recorddetail r
                  ON r.noofinpat = i.noofinpat
                 AND r.valid = '1'
               WHERE i.outhosdept LIKE '%' || v_deptid || '%' --v_deptid
                 AND i.patid LIKE '%' || v_patid || '%'
                 AND i.NAME LIKE '%' || v_name || '%'
                 AND i.status LIKE '%' || v_status || '%'
                 AND (i.admitdate >= p_admitbegindate AND
                     i.admitdate <= p_admitenddate)
               GROUP BY i.outbed,
                        i.outhosdept,
                        department.NAME,
                        i.patid,
                        i.NAME,
                        i.noofinpat,
                        c.NAME,
                        i.agestr,
                        i.incount,
                        d.NAME) a
      
       GROUP BY a.bedid,
                a.deptno,
                a.deptname,
                a.patid,
                a.patname,
                a.noofinpat,
                a.sexname,
                a.age,
                a.incount,
                a.admitdiagnosis,
                a.emrnonwritecount,
                a.emrhaswritecount,
                a.hassubmitnoauditcount,
                a.hasauditcount
       ORDER BY deptno, bedid;
  END;

  --得到病人的统计信息 质控科人员 (仁和版本的，不连接评分表edit by ywk 2012年8月2日 08:58:34)
  procedure usp_GetrhDepPatStatInfoAll(v_deptid         varchar2,
                                       v_patid          varchar2,
                                       v_name           varchar2,
                                       v_admitbegindate varchar2,
                                       v_admitenddate   varchar2,
                                       o_result         OUT empcurtype) AS
    p_admitbegindate VARCHAR2(19);
    p_admitenddate   VARCHAR2(19);
  BEGIN
    p_admitbegindate := v_admitbegindate || ' 00:00:00';
    p_admitenddate   := v_admitenddate || ' 23:59:59';
  
    IF v_admitbegindate = '' THEN
      p_admitbegindate := '2001-01-01 00:00:00';
    END IF;
  
    IF v_admitenddate = '' THEN
      p_admitenddate := '2999-01-01 00:00:00';
    END IF;
  
    OPEN o_result FOR
      SELECT a.bedid,
             a.deptno,
             a.deptname,
             a.patid,
             a.patname,
             a.noofinpat,
             a.sexname,
             a.age,
             a.incount,
             a.admitdiagnosis,
             a.emrnonwritecount,
             a.emrhaswritecount,
             a.hassubmitnoauditcount,
             a.hasauditcount
        FROM (SELECT i.outbed bedid,
                     i.outhosdept deptno,
                     department.NAME deptname,
                     i.patid,
                     i.NAME patname,
                     i.noofinpat,
                     c.NAME sexname,
                     i.agestr age,
                     DECODE(i.incount, 1, i.incount, 0, 1) incount,
                     d.NAME admitdiagnosis,
                     '0' emrnonwritecount,
                     SUM(DECODE(r.hassubmit, '4600', 1, 0)) emrhaswritecount, --未写病历
                     SUM(DECODE(r.hassubmit, '4600', 0, '4603', 0, 1)) hassubmitnoauditcount, --未提交
                     SUM(DECODE(r.hassubmit, '4603', 1, 0)) hasauditcount --已审核完病历
                FROM inpatient i
                LEFT OUTER JOIN department
                  ON department.ID = i.outhosdept
                 AND department.valid = '1'
                LEFT OUTER JOIN dictionary_detail c
                  ON c.detailid = i.sexid
                 AND c.categoryid = '3'
                LEFT OUTER JOIN diagnosis d
                  ON d.icd = REPLACE(i.admitdiagnosis, ' ', '.')
                 AND d.valid = '1'
                LEFT OUTER JOIN recorddetail r
                  ON r.noofinpat = i.noofinpat
                 AND r.valid = '1'
               WHERE i.outhosdept LIKE '%' || v_deptid || '%' --v_deptid
                 AND i.patid LIKE '%' || v_patid || '%'
                 AND i.NAME LIKE '%' || v_name || '%'
                 AND i.status in ('1501', '1502', '1503')
                 AND (i.admitdate >= p_admitbegindate AND
                     i.admitdate <= p_admitenddate)
               GROUP BY i.outbed,
                        i.outhosdept,
                        department.NAME,
                        i.patid,
                        i.NAME,
                        i.noofinpat,
                        c.NAME,
                        i.agestr,
                        i.incount,
                        d.NAME) a
      
       GROUP BY a.bedid,
                a.deptno,
                a.deptname,
                a.patid,
                a.patname,
                a.noofinpat,
                a.sexname,
                a.age,
                a.incount,
                a.admitdiagnosis,
                a.emrnonwritecount,
                a.emrhaswritecount,
                a.hassubmitnoauditcount,
                a.hasauditcount
       ORDER BY deptno, bedid;
  END;

  --得到病人的统计信息 科室指控员 (仁和版本的，不连接评分表edit by ywk 2012年8月2日 08:58:34)
  procedure usp_GetrhDepPatStatInfoCY(v_deptid         varchar2,
                                      v_patid          varchar2,
                                      v_name           varchar2,
                                      v_admitbegindate varchar2,
                                      v_admitenddate   varchar2,
                                      o_result         OUT empcurtype) AS
    p_admitbegindate VARCHAR2(19);
    p_admitenddate   VARCHAR2(19);
  BEGIN
    p_admitbegindate := v_admitbegindate || ' 00:00:00';
    p_admitenddate   := v_admitenddate || ' 23:59:59';
  
    IF v_admitbegindate = '' THEN
      p_admitbegindate := '2001-01-01 00:00:00';
    END IF;
  
    IF v_admitenddate = '' THEN
      p_admitenddate := '2999-01-01 00:00:00';
    END IF;
  
    OPEN o_result FOR
      SELECT a.bedid,
             a.deptno,
             a.deptname,
             a.patid,
             a.patname,
             a.noofinpat,
             a.sexname,
             a.age,
             a.incount,
             a.admitdiagnosis,
             a.emrnonwritecount,
             a.emrhaswritecount,
             a.hassubmitnoauditcount,
             a.hasauditcount
        FROM (SELECT i.outbed bedid,
                     i.outhosdept deptno,
                     department.NAME deptname,
                     i.patid,
                     i.NAME patname,
                     i.noofinpat,
                     c.NAME sexname,
                     i.agestr age,
                     DECODE(i.incount, 1, i.incount, 0, 1) incount,
                     d.NAME admitdiagnosis,
                     '0' emrnonwritecount,
                     SUM(DECODE(r.hassubmit, '4600', 1, 0)) emrhaswritecount, --未写病历
                     SUM(DECODE(r.hassubmit, '4600', 0, '4603', 0, 1)) hassubmitnoauditcount, --未提交
                     SUM(DECODE(r.hassubmit, '4603', 1, 0)) hasauditcount --已审核完病历
                FROM inpatient i
                LEFT OUTER JOIN department
                  ON department.ID = i.outhosdept
                 AND department.valid = '1'
                LEFT OUTER JOIN dictionary_detail c
                  ON c.detailid = i.sexid
                 AND c.categoryid = '3'
                LEFT OUTER JOIN diagnosis d
                  ON d.icd = REPLACE(i.admitdiagnosis, ' ', '.')
                 AND d.valid = '1'
                LEFT OUTER JOIN recorddetail r
                  ON r.noofinpat = i.noofinpat
                 AND r.valid = '1'
               WHERE i.outhosdept LIKE '%' || v_deptid || '%' --v_deptid
                 AND i.patid LIKE '%' || v_patid || '%'
                 AND i.NAME LIKE '%' || v_name || '%'
                 AND i.status in ('1502', '1503')
                 AND (i.admitdate >= p_admitbegindate AND
                     i.admitdate <= p_admitenddate)
               GROUP BY i.outbed,
                        i.outhosdept,
                        department.NAME,
                        i.patid,
                        i.NAME,
                        i.noofinpat,
                        c.NAME,
                        i.agestr,
                        i.incount,
                        d.NAME) a
      
       GROUP BY a.bedid,
                a.deptno,
                a.deptname,
                a.patid,
                a.patname,
                a.noofinpat,
                a.sexname,
                a.age,
                a.incount,
                a.admitdiagnosis,
                a.emrnonwritecount,
                a.emrhaswritecount,
                a.hassubmitnoauditcount,
                a.hasauditcount
       ORDER BY deptno, bedid;
  END;

  --得到病人的所有病历
  procedure usp_GetAllEmrDocByNoofinpat(v_noofinpat varchar2,
                                        o_result    OUT empcurtype) AS
  BEGIN
    OPEN o_result FOR
      SELECT 0 ID, ' ' NAME
        FROM dual
      UNION
      SELECT a.id, a.name
        FROM (SELECT id, name
                FROM recorddetail
               WHERE noofinpat = v_noofinpat
                 AND valid = '1'
               ORDER BY sortid, id) a;
  END;

  --得到科室所有医生
  procedure usp_GetAllDoctorByUserNO(v_userid varchar2,
                                     o_result OUT empcurtype) AS
  BEGIN
    open o_result for
      select u1.id, u1.name
        from users u1
       where u1.deptid in (select u2.deptid
                             from users u2
                            where u2.id = v_userid
                              and u2.valid = '1')
         and u1.valid = '1';
  END;

  --得到科室所有医生
  procedure usp_GetAllDoctorByNoofinpat(v_noofinpat varchar2,
                                        o_result    OUT empcurtype) AS
  BEGIN
    OPEN o_result FOR
      select u1.id, u1.name
        from users u1
       where u1.deptid in (select nvl(i.outhosdept, i.admitdept)
                             from inpatient i
                            where i.noofinpat = v_noofinpat)
         and u1.valid = '1';
  END;

  --得到病历评分表的信息//edit by wyt 2012-12-06 新增评分主记录ID条件
  procedure usp_GetEmrPointByNoofinpat(v_noofinpat varchar2,
                                       v_chiefid   varchar2,
                                       o_result    OUT empcurtype) AS
  BEGIN
    OPEN o_result FOR
      select case e.valid
               when '1' then
                '有效'
               else
                '无效'
             end valid,
             e.cancel_user,
             e.cancelusername,
             e.cancel_time,
             e.doctorID,
             e.doctorname,
             e.create_user,
             e.createusername,
             e.create_time,
             e.problem_desc,
             e.reducepoint,
             e.num,
             e.grade,
             e.EMR_MARK_RECORD_ID, ---新增评分主记录ID
             e.recorddetailname,
             (case
               when v.childname is null then
                h.cname
               else
                v.childname
             end) childname, --edit by ywk 2012年4月12日10:23:02
             v.childcode,
             -- v.childname,---新增小评分项名称
             e.id,
             h.cname
        from emr_point e
      --left join emr_configpoint v on e.emrpointid = to_char(v.id)
        left join emr_configpoint v
          on e.emrpointid = to_char(v.childcode) --王冀 添加完成后 查询不到  分类ID没有
        left join dict_catalog h
          on e.sortid = h.ccode
       where e.noofinpat = v_noofinpat
         and e.EMR_MARK_RECORD_ID = v_chiefid
         and e.valid = '1'; --and v.valid='1';--edit by ywk 2012年4月11日9:31:47
  END;

  -- 仁和 得到病历评分表的详细信息
  procedure usp_GetRHPoint(v_rhqc_tableId varchar2,
                           o_result       OUT empcurtype) AS
  BEGIN
    OPEN o_result FOR
      select case e.valid
               when '1' then
                '有效'
               else
                '无效'
             end valid,
             e.cancel_user,
             e.cancelusername,
             e.cancel_time,
             e.doctorID,
             e.doctorname,
             e.create_user,
             e.createusername,
             e.create_time,
             e.problem_desc,
             e.reducepoint,
             e.num,
             e.grade,
             e.id,
             e.recorddetailname,
             (case
               when v.childname is null then
                h.cname
               else
                v.childname
             end) childname, --edit by ywk 2012年4月12日10:23:02
             -- v.childname,---新增小评分项名称
             h.cname
        from emr_rhpoint e
        left join emr_configpoint v
          on e.emrpointid = v.id
        left join dict_catalog h
          on e.sortid = h.ccode
       where e.rhqc_table_id = v_rhqc_tableId
         and e.valid = '1'; --and v.valid='1';--edit by ywk 2012年4月11日9:31:47
  END;

  procedure usp_insertEmrPoint(v_doctorid       varchar2,
                               v_doctorname     varchar2,
                               v_create_user    varchar2,
                               v_createusername varchar2,
                               v_problem_desc   varchar2,
                               v_reducepoint    varchar2,
                               v_num            varchar2,
                               --v_grade            varchar2,
                               v_recorddetailid   varchar2,
                               v_noofinpat        varchar2,
                               v_recorddetailname varchar2,
                               v_sortid           varchar2, --新增大类编号
                               --v_emrpointid INT DEFAULT 0,---新增字段
                               v_emrpointid varchar2, ---新增字段 --edit by wyt 2012-12-11
                               v_chiefid    varchar2 ---新增字段
                               ) AS
  BEGIN
    insert into emr_point
      (id,
       doctorid,
       doctorname,
       create_user,
       createusername,
       create_time,
       problem_desc,
       reducepoint,
       num,
       --grade,
       recorddetailid,
       valid,
       noofinpat,
       recorddetailname,
       emrpointid,
       sortid, ---新增字段
       EMR_MARK_RECORD_ID ---新增字段
       )
    values
      (seq_emr_point_id.nextval, --(select nvl(max(id), 0) + 1 from emr_point),
       v_doctorid,
       v_doctorname,
       v_create_user,
       v_createusername,
       sysdate,
       v_problem_desc,
       v_reducepoint,
       v_num,
       -- v_grade,
       v_recorddetailid,
       '1',
       v_noofinpat,
       v_recorddetailname,
       v_emrpointid,
       v_sortid,
       v_chiefid);
  END;

  procedure usp_cancelEmrPoint(v_id varchar2, v_cancel_user varchar2) AS
  BEGIN
    update emr_point
       set emr_point.valid       = '0',
           emr_point.cancel_user = v_cancel_user,
           emr_point.cancel_time = sysdate
     where emr_point.id = v_id;
  END;

  procedure usp_cancelRHEmrPoint(v_id              varchar2,
                                 v_cancel_user     varchar2,
                                 v_cancel_userName varchar2) AS
  BEGIN
    update emr_rhpoint
       set emr_rhpoint.valid          = '0',
           emr_rhpoint.cancel_user    = v_cancel_user,
           emr_rhpoint.cancelusername = v_cancel_userName,
           emr_rhpoint.cancel_time    = to_char(sysdate,
                                                'yyyy-MM-dd HH:mm:ss')
     where emr_rhpoint.id = v_id;
  END;

  procedure usp_GetPatientInfo(v_noofinpat varchar2,
                               o_result    OUT empcurtype) AS
  BEGIN
    OPEN o_result FOR
      select i.name,
             dd.name as gender,
             i.admitdate,
             i.outbed,
             i.agestr,
             i.patid
        from inpatient i
        LEFT outer JOIN dictionary_detail dd
          ON dd.detailid = i.sexid
         AND dd.categoryid = '3'
       where i.noofinpat = v_noofinpat;
  END;

  procedure usp_GetPointByDoctorID(v_doctorID varchar2,
                                   o_result   OUT empcurtype) AS
  BEGIN
    OPEN o_result FOR
      SELECT e.id,
             e.noofinpat,
             e.recorddetailid,
             e.problem_desc,
             replace(to_char(e.reducepoint, 'FM990.099'), '.0', '') || '分' as reducepoint,
             e.grade,
             e.num,
             e.recorddetailname,
             (SELECT i.name FROM inpatient i WHERE i.noofinpat = e.noofinpat) name
        FROM emr_point e
       WHERE e.doctorid = v_doctorID
         AND e.valid = '1';
  END;

  ------病历评分配置
  PROCEDURE usp_Edit_ConfigPoint(v_EditType   varchar default '', --操作类型
                                 V_ID         varchar default '',
                                 v_CCode      varchar default '',
                                 v_CChildCode varchar default '',
                                 v_CChildName varchar default '',
                                 v_Valid      varchar default '1',
                                 o_result     OUT empcurtype) as
  begin
    open o_result for
    
      select V_ID from dual;
    --添加
    if v_EditType = '1' then
      INSERT INTO Emr_ConfigPoint
        (ID, CCODE, ChildCode, ChildName, Valid)
      VALUES
        (seq_Emr_ConfigPoint_ID.Nextval,
         v_CCode,
         --v_CChildCode,
         seq_Emr_ConfigPoint_ID.Currval,
         v_CChildName,
         v_Valid);
    
      --修改
    elsif v_EditType = '2' then
    
      UPDATE Emr_ConfigPoint
         set CCODE     = v_CCode,
             ChildCode = v_CChildCode,
             ChildName = v_CChildName,
             Valid     = v_Valid
       WHERE id = V_ID;
    
      --删除
    elsif v_EditType = '3' then
    
      update Emr_ConfigPoint a set a.valid = 0 where ID = V_ID;
    
      --查询
    elsif v_EditType = '4' then
      open o_result for
        select (case
                 when a.valid = 1 then
                  '是'
                 else
                  '否'
               end) validName,
               a.*
          from Emr_ConfigPoint a
         where a.valid = '1'
         order by id;
    end if;
  end;

  ------病历评分（扣分理由）配置       ywk 2012年5月28日 09:22:10   修改 王冀 2012-11-9    修改 王冀 2012-11-29
  PROCEDURE usp_Edit_ConfigReduction(v_EditType        varchar default '', --操作类型
                                     V_REDUCEPOINT     varchar default '',
                                     v_PROBLEMDESC     varchar default '',
                                     v_CREATEUSER      varchar default '',
                                     v_CREATETIME      varchar default '',
                                     v_ID              varchar default '',
                                     v_Valid           varchar default '1',
                                     v_Parents         varchar default '',
                                     v_Children        varchar default '',
                                     v_Isauto          varchar default '',
                                     v_Selectcondition varchar default '',
                                     o_result          OUT empcurtype) as
  begin
    open o_result for
    
      select V_ID from dual;
    --添加
    if v_EditType = '1' then
      INSERT INTO EMR_ConfigReduction2
        (ID,
         REDUCEPOINT,
         PROBLEM_DESC,
         CREATE_USER,
         CREATE_TIME,
         Valid,
         Parents,
         Children,
         Isauto,
         Selectcondition)
      VALUES
        (seq_EMR_ConfigReduction_ID.Nextval,
         V_REDUCEPOINT,
         v_PROBLEMDESC,
         v_CREATEUSER,
         to_date(v_CREATETIME, 'yyyy/mm/dd HH24:MI:SS'),
         -- v_CREATETIME,
         v_Valid,
         v_Parents,
         v_Children,
         v_Isauto,
         v_Selectcondition);
    
      --修改
    elsif v_EditType = '2' then
    
      UPDATE EMR_ConfigReduction2
         set REDUCEPOINT     = V_REDUCEPOINT,
             PROBLEM_DESC    = v_PROBLEMDESC,
             CREATE_USER     = v_CREATEUSER,
             Valid           = v_Valid,
             Parents         = v_Parents,
             Children        = v_Children,
             Isauto          = v_Isauto,
             Selectcondition = v_Selectcondition
       WHERE id = V_ID;
    
      --删除
    elsif v_EditType = '3' then
    
      update EMR_ConfigReduction2 a set a.valid = 0 where ID = V_ID;
    
      --查询   此处不知道有没有用到过 王冀 2012-11-9
    elsif v_EditType = '4' then
      open o_result for
        select A.ID,
               A.REDUCEPOINT,
               A.PROBLEM_DESC,
               A.CREATE_USER,
               A.CREATE_TIME,
               (case
                 when A.valid = 1 then
                  '是'
                 else
                  '否'
               end) validName,
               A.PARENTS,
               a.children
          from EMR_ConfigReduction2 A
         order by A.ID;
    end if;
  end;

  --病历评分中要显示的跟节点
  procedure usp_GetPointClass(o_result OUT empcurtype) as
  begin
    OPEN o_result FOR
    /* select ccode,cname from dict_catalog
                       where mname is not null ;*/
    
      select ccode, cname
        from dict_catalog
       where cname in ('病案首页',
                       '住院志',
                       '病程记录',
                       '知情文件',
                       '手术相关记录',
                       '会诊记录',
                       '其他记录');
  end;

  ------科室质控员的配置操作 2012年7月10日13:45:29 ywk
  PROCEDURE usp_Edit_ConfigPointManager(v_EditType      varchar default '', --操作类型
                                        V_ID            varchar default '',
                                        v_DeptID        varchar default '',
                                        v_QcManagerID   varchar default '',
                                        v_ChiefDoctorID varchar default '',
                                        v_Valid         varchar default '1',
                                        o_result        OUT empcurtype) as
  begin
    --open o_result for
  
    -- select V_ID from dual;
    --添加
    if v_EditType = '1' then
      insert into emr_ConfigCheckPointUser
        (Id, Deptid, Qcmanagerid, Chiefdoctorid, Valid)
      values
        (seqemr_ConfigCheckPointUser_ID.Nextval,
         v_DeptID,
         v_QcManagerID,
         v_ChiefDoctorID,
         '1');
      commit;
    
      --修改
    elsif v_EditType = '2' then
    
      UPDATE emr_ConfigCheckPointUser
         set DeptID        = v_DeptID,
             QCManagerID   = v_QcManagerID,
             ChiefDoctorID = v_ChiefDoctorID,
             Valid         = v_Valid
       WHERE id = V_ID;
    
      --删除
    elsif v_EditType = '3' then
    
      update emr_ConfigCheckPointUser a set a.valid = 0 where ID = V_ID;
    
      --查询
      /*elsif v_EditType = '4'  then
         if v_DeptID='' then  open o_result for
      select  A.id,D.NAME deptname,(case
            when A.valid = 1 then    '是' else  '否'  end) validName,U1.Name  QCMANAGERNAME, U2.NAME  CHIEFNAME
            from emr_Configcheckpointuser  A left join department D on A.DEPTID=D.ID
            left join users U1 on U1.ID=A.QCMANAGERID  left join users U2 on  U2.ID=A.CHIEFDOCTORID ;
       else open o_result for
          select  A.id,D.NAME deptname,(case
            when A.valid = 1 then    '是' else  '否'  end) validName,U1.Name                       QCMANAGERNAME, U2.NAME  CHIEFNAME
            from emr_Configcheckpointuser  A left join department D on A.DEPTID=D.ID
            left join users U1 on U1.ID=A.QCMANAGERID  left join users U2 on   U2.ID=A.CHIEFDOCTORID ;*/
    
    end if;
  end;

  ---获得科室内病人，（在评分表中存在记录的病人）
  PROCEDURE usp_getRHdepartpatstatinfo(v_deptid         VARCHAR2,
                                       v_patid          VARCHAR2,
                                       v_name           VARCHAR2,
                                       v_status         VARCHAR2,
                                       v_admitbegindate VARCHAR2,
                                       v_admitenddate   VARCHAR2,
                                       o_result         OUT empcurtype) AS
    p_admitbegindate VARCHAR2(19);
    p_admitenddate   VARCHAR2(19);
  BEGIN
    p_admitbegindate := v_admitbegindate || ' 00:00:00';
    p_admitenddate   := v_admitenddate || ' 23:59:59';
    IF v_admitbegindate = '' THEN
      p_admitbegindate := '2001-01-01 00:00:00';
    END IF;
  
    IF v_admitenddate = '' THEN
      p_admitenddate := '2999-01-01 00:00:00';
    END IF;
  
    OPEN o_result FOR
      SELECT a.bedid,
             a.deptno,
             a.deptname,
             a.patid,
             a.patname,
             a.noofinpat,
             a.sexname,
             a.age,
             a.incount,
             a.admitdiagnosis,
             a.emrnonwritecount,
             a.emrhaswritecount,
             a.hassubmitnoauditcount,
             a.hasauditcount,
             SUM(p.reducepoint) totalscore,
             rh.id,
             a.status
        FROM (SELECT i.outbed bedid,
                     i.outhosdept deptno,
                     department.NAME deptname,
                     i.patid,
                     i.NAME patname,
                     i.status,
                     i.noofinpat,
                     c.NAME sexname,
                     i.agestr age,
                     DECODE(i.incount, 1, i.incount, 0, 1) incount,
                     d.NAME admitdiagnosis,
                     '0' emrnonwritecount,
                     SUM(DECODE(r.hassubmit, '4600', 1, 0)) emrhaswritecount, --未写病历
                     SUM(DECODE(r.hassubmit, '4600', 0, '4603', 0, 1)) hassubmitnoauditcount, --未提交
                     SUM(DECODE(r.hassubmit, '4603', 1, 0)) hasauditcount --已审核完病历
                FROM inpatient i
                LEFT OUTER JOIN department
                  ON department.ID = i.outhosdept
                 AND department.valid = '1'
                LEFT OUTER JOIN dictionary_detail c
                  ON c.detailid = i.sexid
                 AND c.categoryid = '3'
                LEFT OUTER JOIN diagnosis d
                  ON d.icd = REPLACE(i.admitdiagnosis, ' ', '.')
                 AND d.valid = '1'
                LEFT OUTER JOIN recorddetail r
                  ON r.noofinpat = i.noofinpat
                 AND r.valid = '1'
               WHERE i.outhosdept LIKE '%' || v_deptid || '%' --v_deptid
                 AND i.patid LIKE '%' || v_patid || '%'
                 AND i.NAME LIKE '%' || v_name || '%'
                 AND i.status LIKE '%' || v_status || '%'
                 AND (i.admitdate >= p_admitbegindate AND
                     i.admitdate <= p_admitenddate)
               GROUP BY i.outbed,
                        i.outhosdept,
                        department.NAME,
                        i.patid,
                        i.NAME,
                        i.noofinpat,
                        c.NAME,
                        i.agestr,
                        i.incount,
                        d.NAME,
                        i.status) a
        LEFT OUTER JOIN EMR_RHQC_TABLE rh
          ON rh.noofinpat = a.noofinpat
        LEFT OUTER JOIN emr_rhpoint p
          ON rh.id = p.rhqc_table_id
       WHERE p.valid = '1'
         and rh.stateid in ('8702', '8704')
       GROUP BY a.bedid,
                a.deptno,
                a.deptname,
                a.patid,
                a.patname,
                a.noofinpat,
                a.sexname,
                a.age,
                a.incount,
                a.admitdiagnosis,
                a.emrnonwritecount,
                a.emrhaswritecount,
                a.hassubmitnoauditcount,
                a.hasauditcount,
                rh.id,
                a.status
       ORDER BY deptno, totalscore desc, bedid;
  END;

  ------病历评分（扣分理由）配置 仁和质控       ywk 2012年8月7日 09:22:10
  PROCEDURE usp_Edit_RHConfigReduction(v_EditType    varchar default '', --操作类型
                                       V_REDUCEPOINT varchar default '',
                                       v_PROBLEMDESC varchar default '',
                                       v_CREATEUSER  varchar default '',
                                       v_CREATETIME  varchar default '',
                                       v_ID          varchar default '',
                                       v_Valid       varchar default '1',
                                       v_UserType    varchar default '',
                                       o_result      OUT empcurtype) as
  begin
    open o_result for
    
      select V_ID from dual;
    --添加
    if v_EditType = '1' then
      INSERT INTO EMR_RHConfigReduction
        (ID,
         REDUCEPOINT,
         PROBLEM_DESC,
         CREATE_USER,
         CREATE_TIME,
         Valid,
         USER_TYPE)
      VALUES
        (seq_EMR_ConfigReduction_ID.Nextval,
         V_REDUCEPOINT,
         v_PROBLEMDESC,
         v_CREATEUSER,
         to_date(v_CREATETIME, 'yyyy/mm/dd HH24:MI:SS'),
         -- v_CREATETIME,
         v_Valid,
         v_UserType);
    
      --修改
    elsif v_EditType = '2' then
    
      UPDATE EMR_RHConfigReduction
         set REDUCEPOINT  = V_REDUCEPOINT,
             PROBLEM_DESC = v_PROBLEMDESC,
             CREATE_USER  = v_CREATEUSER,
             Valid        = v_Valid,
             USER_TYPE    = v_UserType
       WHERE id = V_ID;
    
      --删除
    elsif v_EditType = '3' then
    
      update EMR_RHConfigReduction a set a.valid = 0 where ID = V_ID;
    
      --查询
    elsif v_EditType = '4' then
      open o_result for
        select A.ID,
               A.REDUCEPOINT,
               A.PROBLEM_DESC,
               A.CREATE_USER,
               A.CREATE_TIME,
               (case
                 when A.valid = 1 then
                  '是'
                 else
                  '否'
               end) validName,
               A.USER_TYPE
          from EMR_RHConfigReduction A
         order by A.ID;
    end if;
  end;

  PROCEDURE usp_QcmanagerDepartmentlist(o_result OUT empcurtype) AS
    /**********
     版本号  1.0.0.0.0
     创建时间   2013-03-21
     作者     
     版权     YidanSoft
     描述  医疗质量统计分析
     功能说明
     输入参数
     输出参数
     结果集、排序
    医疗质量统计分析
    
     调用的sp
     调用实例
    
     修改记录
    
    
    
    **********/
    v_sql VARCHAR2(4000);
  BEGIN
    --创建医疗统计分析临时表
    v_sql := 'truncate table TMP_DEPARTMENTLIST ';
  
    EXECUTE IMMEDIATE v_sql;
  
    --插入挂有病区的科室
    INSERT INTO TMP_DEPARTMENTLIST
      (deptcode, deptname)
      SELECT dept.ID, dept.NAME
        FROM department dept
       WHERE EXISTS
       (SELECT 1 FROM dept2ward ward WHERE dept.ID = ward.deptid);
  
    --更新在院人数
    UPDATE TMP_DEPARTMENTLIST
       SET inhos = NVL((SELECT COUNT(1)
                         FROM inpatient inp
                        WHERE inp.status IN (1500, 1501, 1505, 1506, 1507) --, 1504
                          AND inp.outhosdept = deptcode),
                       0);
  
    --床位数
    UPDATE TMP_DEPARTMENTLIST
       SET bedcnt = NVL((SELECT COUNT(1)
                          FROM bed bed
                         WHERE bed.valid = 1
                           AND bed.deptid = deptcode),
                        0);
  
    --书写次数缺陷病例数
    UPDATE TMP_DEPARTMENTLIST
       SET WRTDEFICITCNT = NVL((select count(distinct(inp.noofinpat))
                                 from qcrecord qr
                                 left join inpatient inp
                                   on qr.noofinpat = inp.noofinpat
                                where qr.foulstate = '1'
                                  and qr.result = '0' --缺陷未补全
                                  and qr.valid = '1'
                                  and inp.status IN
                                      (1500, 1501, 1505, 1506, 1507) --在院
                                     /*and inp.status IN (1500,
                                     1501,
                                     1502,
                                     1503,
                                     1504,
                                     1505,
                                     1506,
                                     1507) --包含出院           */
                                  AND inp.outhosdept = deptcode),
                               0);
  
    --时限质控缺陷病例数
    UPDATE TMP_DEPARTMENTLIST
       SET TIMELIMITEDCNT = NVL((select count(distinct(inp.noofinpat))
                                  from qcrecord qr
                                  left join inpatient inp
                                    on qr.noofinpat = inp.noofinpat
                                 where qr.foulstate = '1'
                                   and qr.valid = '1'
                                   and inp.status IN
                                       (1500, 1501, 1505, 1506, 1507) --在院
                                      /* and inp.status IN (1500,
                                      1501,
                                      1502,
                                      1503,
                                      1504,
                                      1505,
                                      1506,
                                      1507) --包含出院      */
                                   AND inp.outhosdept = deptcode),
                                0);
  
    --已抽查病例数
    UPDATE TMP_DEPARTMENTLIST
       SET HAVESELECTCNT = nvl((select count(distinct(er.noofinpat))
                                 from emr_automark_record er
                                 left join inpatient inp
                                   on er.noofinpat = inp.noofinpat
                                where inp.outhosdept = deptcode
                                  and er.isvalid = '1'
                                  and er.isauto = '0'
                                  and inp.status IN
                                      (1500, 1501, 1505, 1506, 1507) --在院
                               /*and inp.status IN (1500, --包含出院
                               1501,
                               1502,
                               1503,
                               1504,
                               1505,
                               1506,
                               1507)*/
                               ),
                               0);
  
    --已抽查病历平均分
    UPDATE TMP_DEPARTMENTLIST
       SET SELECTAVE = NVL((select avg(er.score)
                             from emr_automark_record er
                             left join inpatient inp
                               on er.noofinpat = inp.noofinpat
                            where er.isvalid = '1'
                              and er.isauto = '0'
                              AND inp.status IN
                                  (1500, 1501, 1505, 1506, 1507)
                              AND inp.outhosdept = deptcode),
                           0);
  
    --疑似未写病历人数
    UPDATE TMP_DEPARTMENTLIST
       SET NORECORDS = NVL((select ((select count(distinct(inp.noofinpat))
                                      from inpatient inp
                                     where inp.status in
                                           (1500, 1501, 1505, 1506, 1507)
                                       and inp.outhosdept = deptcode) -
                                  (select count(distinct(rd.noofinpat))
                                      from recorddetail rd
                                      left join inpatient inp
                                        on inp.noofinpat = rd.noofinpat
                                     where inp.status in
                                           (1500, 1501, 1505, 1506, 1507)
                                       and rd.valid = '1'
                                       and inp.outhosdept = deptcode))
                             from dual),
                           0);
  
    OPEN o_result FOR
      SELECT '_' deptcode,
             '总计' deptname,
             SUM(inhos) inhos,
             SUM(bedcnt) bedcnt,
             SUM(WRTDEFICITCNT) WRTDEFICITCNT,
             SUM(TIMELIMITEDCNT) TIMELIMITEDCNT,
             SUM(HAVESELECTCNT) HAVESELECTCNT,
             SUM(SELECTAVE) SELECTAVE,
             SUM(NORECORDS) NORECORDS
        FROM TMP_DEPARTMENTLIST
      UNION ALL
      SELECT deptcode,
             deptname,
             inhos,
             bedcnt,
             WRTDEFICITCNT,
             TIMELIMITEDCNT,
             HAVESELECTCNT,
             SELECTAVE,
             NORECORDS
        FROM TMP_DEPARTMENTLIST;
  END;

 procedure usp_QcmanagerDepartmentDetail(v_deptid varchar default '',
                                         o_result OUT empcurtype) as
 begin
   OPEN o_result FOR
     select inp.noofinpat,
            inp.patid,
            inp.name,
            decode(inp.sexid, '1', '男', '2', '女', '未知') sex,
            inp.agestr,
            inp.outbed,
            u.name resident,
            inp.admitdate,
            dia.name admitdiagnosis,
            inp.status
       from inpatient inp
       left join users u
         on u.id = inp.resident
       left join diagnosis dia
         on dia.markid = inp.admitdiagnosis
      where inp.status in (1500, 1501, 1505, 1506, 1507)
        and inp.outhosdept = v_deptid
      order by inp.outbed;
 end;
END;
/
