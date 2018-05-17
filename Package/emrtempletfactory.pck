CREATE OR REPLACE PACKAGE emrtempletfactory IS
  -- 模板工厂模块使用到存储过程

  TYPE empcurtyp IS REF CURSOR;

  /*
  *  描述  编辑EMRTEMPLET模板
  */
  PROCEDURE usp_EditEmrTemplet(v_EditType        varchar default '', --操作类型
                               v_TEMPLET_ID      VARCHAR2 default '',
                               v_FILE_NAME       VARCHAR2 default '',
                               v_DEPT_ID         VARCHAR2 default '',
                               v_CREATOR_ID      VARCHAR2 default '',
                               v_CREATE_DATETIME VARCHAR2 default '',
                               v_LAST_TIME       VARCHAR2 default '',
                               v_PERMISSION      VARCHAR2 default '',
                               v_MR_CLASS        VARCHAR2 default '',
                               v_MR_CODE         VARCHAR2 default '',
                               v_MR_NAME         VARCHAR2 default '',
                               v_MR_ATTR         VARCHAR2 default '',
                               v_QC_CODE         VARCHAR2 default '',
                               v_NEW_PAGE_FLAG   INTEGER default 0,
                               v_FILE_FLAG       INTEGER default 0,
                               v_WRITE_TIMES     INTEGER default 0,
                               v_CODE            VARCHAR2 default '',
                               v_HOSPITAL_CODE   VARCHAR2 default '',
                               v_XML_DOC_NEW     clob default '',
                               v_PY              varchar2 default '',
                               v_WB              varchar2 default '',
                               v_ISSHOWFILENAME  varchar2 default '0',
                               v_ISFIRSTDAILY    varchar2 default '0',
                               v_ISYIHUANGOUTONG varchar2 default '0',
                               v_NEW_PAGE_END    INTEGER default 0,
                               v_state           varchar2 default '0',
                               v_auditor         varchar2 default '',
                                v_isconfigpagesize    varchar2 default '0',
                               o_result          OUT empcurtyp);

  /*
  *  描述  编辑EMRTEMPLET_Item子模板
  */
  PROCEDURE usp_EditEmrTemplet_Item(v_EditType         varchar default '', --操作类型
                                    v_MR_CLASS         VARCHAR2 default '',
                                    v_MR_CODE          VARCHAR2 default '',
                                    v_MR_NAME          VARCHAR2 default '',
                                    v_MR_ATTR          VARCHAR2 default '',
                                    v_QC_CODE          VARCHAR2 default '',
                                    v_DEPT_ID          VARCHAR2 default '',
                                    v_CREATOR_ID       VARCHAR2 default '',
                                    v_CREATE_DATE_TIME VARCHAR2 default '',
                                    v_LAST_TIME        VARCHAR2 default '',
                                    v_CONTENT_CODE     VARCHAR2 default '',
                                    v_PERMISSION       int default 0,
                                    v_VISIBLED         int default 1,
                                    v_INPUT            VARCHAR2 default '',
                                    v_HOSPITAL_CODE    VARCHAR2 default '',
                                    v_ITEM_DOC_NEW     clob default '',
                                    o_result           OUT empcurtyp);

  /*
  *  描述  编辑EditEmrTempletHeader表  模板页眉
  */
  PROCEDURE usp_EditEmrTempletHeader(v_EditType        varchar default '', --操作类型
                                     v_HEADER_ID       VARCHAR2 default '',
                                     v_NAME            VARCHAR2 default '',
                                     v_CREATOR_ID      VARCHAR2 default '',
                                     v_CREATE_DATETIME VARCHAR2 default '',
                                     v_LAST_TIME       VARCHAR2 default '',
                                     v_HOSPITAL_CODE   VARCHAR2 default '',
                                     v_CONTENT         CLOB default '',
                                     o_result          OUT empcurtyp);


                                       /*
  *  描述  编辑EditEmrTempletHeader表  模板页脚
  */
  PROCEDURE usp_EditEmrTemplet_Foot(v_EditType        varchar default '', --操作类型
                                     v_FOOT_ID       VARCHAR2 default '',
                                     v_NAME            VARCHAR2 default '',
                                     v_CREATOR_ID      VARCHAR2 default '',
                                     v_CREATE_DATETIME VARCHAR2 default '',
                                     v_LAST_TIME       VARCHAR2 default '',
                                     v_HOSPITAL_CODE   VARCHAR2 default '',
                                     v_CONTENT         CLOB default '',
                                     o_result          OUT empcurtyp);

  --保存诊断按钮
  PROCEDURE usp_SaveDiagButton(v_diagname   varchar);

  --个人模版维护
  PROCEDURE usp_EditMyEmrTemplet(v_EditType        varchar default '',
                               v_CODE      VARCHAR2 default '',
                               v_CREATEUSER      VARCHAR2 default '',
                               v_MR_CLASS        VARCHAR2 default '',
                               v_MR_NAME         VARCHAR2 default '',
                               v_EMRTEMLETCONTENT     clob default '',
                               o_result          OUT empcurtyp);
end;
/
CREATE OR REPLACE PACKAGE BODY emrtempletfactory IS
  /*******************************************************************/

  /*****************************************************************************/
  PROCEDURE usp_EditEmrTemplet(v_EditType        varchar default '', --操作类型
                               v_TEMPLET_ID      VARCHAR2 default '',
                               v_FILE_NAME       VARCHAR2 default '',
                               v_DEPT_ID         VARCHAR2 default '',
                               v_CREATOR_ID      VARCHAR2 default '',
                               v_CREATE_DATETIME VARCHAR2 default '',
                               v_LAST_TIME       VARCHAR2 default '',
                               v_PERMISSION      VARCHAR2 default '',
                               v_MR_CLASS        VARCHAR2 default '',
                               v_MR_CODE         VARCHAR2 default '',
                               v_MR_NAME         VARCHAR2 default '',
                               v_MR_ATTR         VARCHAR2 default '',
                               v_QC_CODE         VARCHAR2 default '',
                               v_NEW_PAGE_FLAG   INTEGER default 0,
                               v_FILE_FLAG       INTEGER default 0,
                               v_WRITE_TIMES     INTEGER default 0,
                               v_CODE            VARCHAR2 default '',
                               v_HOSPITAL_CODE   VARCHAR2 default '',
                               v_XML_DOC_NEW     clob default '',
                               v_PY              varchar2 default '',
                               v_WB              varchar2 default '',
                               v_ISSHOWFILENAME  varchar2 default '0',
                               v_ISFIRSTDAILY    varchar2 default '0',
                               v_ISYIHUANGOUTONG varchar2 default '0',
                               v_NEW_PAGE_END    INTEGER default 0,
                               v_state           varchar2 default '0',
                               v_auditor         varchar2 default '',
                                  v_isconfigpagesize    varchar2 default '0',
                               o_result          OUT empcurtyp) as

    v_new_TempletID     VARCHAR(10);
    v_new_MRCode        VARCHAR(10);
    v_new_HOSPITAL_CODE VARCHAR(12);
  begin
    open o_result for
      select '' from dual;

    --添加
    if v_EditType = '1' then

      select lpad(max(a.templet_id) + 1, 6, '0') templetid
        into v_new_TempletID
        from emrtemplet a;
      select v_MR_CLASS || v_DEPT_ID ||
             lpad(nvl(max(substr(a.mr_code, -3)), 0) + 1, 3, '0') mrcode
        into v_new_MRCode
        from emrtemplet a
       where a.mr_class = v_MR_CLASS
         and a.dept_id = v_DEPT_ID and a.valid=1;

      select a.medicalid into v_new_HOSPITAL_CODE from hospitalinfo a;

      insert into EMRTEMPLET
        (TEMPLET_ID,
         FILE_NAME,
         DEPT_ID,
         CREATOR_ID,
         CREATE_DATETIME,
         LAST_TIME,
         PERMISSION,
         MR_CLASS,
         MR_CODE,
         MR_NAME,
         MR_ATTR,
         QC_CODE,
         NEW_PAGE_FLAG,
         FILE_FLAG,
         WRITE_TIMES,
         CODE,
         HOSPITAL_CODE,
         XML_DOC_NEW,
         PY,
         WB,
         isshowfilename,
         ISFIRSTDAILY,
         ISYIHUANGOUTONG,
         NEW_PAGE_END,
         Valid,
         state,
         auditor,
         isconfigpagesize,
         auditdate
         )
      values
        (v_new_TempletID,
         v_FILE_NAME,
         v_DEPT_ID,
         v_CREATOR_ID,
         to_char(sysdate, 'yyyy-mm-dd hh24:mi:ss'),
         to_char(sysdate, 'yyyy-mm-dd hh24:mi:ss'),
         v_PERMISSION,
         v_MR_CLASS,
         v_new_MRCode,
         v_MR_NAME,
         v_MR_ATTR,
         v_QC_CODE,
         v_NEW_PAGE_FLAG,
         v_FILE_FLAG,
         v_WRITE_TIMES,
         v_CODE,
         v_new_HOSPITAL_CODE,
         v_XML_DOC_NEW,
         v_PY,
         v_WB,
         v_ISSHOWFILENAME,
         v_ISFIRSTDAILY,
         v_ISYIHUANGOUTONG,
         v_NEW_PAGE_END,
         1,
         v_state,
         v_auditor,
         v_isconfigpagesize,
         (case when  v_auditor is not null then to_char(sysdate,'yyyy-mm-dd HH24:mi:ss') else '' end)
         );

      open o_result for
        select v_new_TempletID from dual;
      --修改
    elsif v_EditType = '2' then

      UPDATE EMRTEMPLET
         SET FILE_NAME       = nvl(v_FILE_NAME, FILE_NAME),
             DEPT_ID         = nvl(v_DEPT_ID, DEPT_ID),
             LAST_TIME       = nvl(to_char(sysdate, 'yyyy-mm-dd hh24:mi:ss'), LAST_TIME),
             PERMISSION      = nvl(v_PERMISSION, PERMISSION),
             MR_CLASS        = nvl(v_MR_CLASS, MR_CLASS),
             MR_CODE         = nvl(v_MR_CODE, MR_CODE),
             MR_NAME         = nvl(v_MR_NAME, MR_NAME),
             MR_ATTR         = nvl(v_MR_ATTR, MR_ATTR),
             QC_CODE         = nvl(v_QC_CODE, QC_CODE),
             NEW_PAGE_FLAG   = nvl(v_NEW_PAGE_FLAG, NEW_PAGE_FLAG),
             FILE_FLAG       = nvl(v_FILE_FLAG, FILE_FLAG),
             WRITE_TIMES     = nvl(v_WRITE_TIMES, WRITE_TIMES),
             CODE            = nvl(v_CODE, CODE),
             HOSPITAL_CODE   = nvl(v_HOSPITAL_CODE, HOSPITAL_CODE),
             XML_DOC_NEW     = v_XML_DOC_NEW,
             PY              = v_PY,
             WB              = v_WB,
             isshowfilename  = v_ISSHOWFILENAME,
             ISFIRSTDAILY    = v_ISFIRSTDAILY,
             ISYIHUANGOUTONG = v_ISYIHUANGOUTONG,
             NEW_PAGE_END    = v_NEW_PAGE_END,
             state           = v_state,
             auditor           = v_auditor,
             isconfigpagesize=v_isconfigpagesize,--新增页面配置
             auditdate         = (case when  v_auditor is not null then to_char(sysdate,'yyyy-mm-dd HH24:mi:ss') else '' end)
       WHERE TEMPLET_ID = v_TEMPLET_ID;

      open o_result for
        select v_TEMPLET_ID from dual;
      --删除(原来的删除直接删除，现在改为更新Valid标识)
    elsif v_EditType = '3' then

      /*delete EMRTEMPLET where TEMPLET_ID = v_TEMPLET_ID;*/
      update EMRTEMPLET set valid=0 where TEMPLET_ID = v_TEMPLET_ID;

      --查询
    elsif v_EditType = '4' then
      open o_result for
        select *
          from emrtemplet a
         where (a.templet_id = v_TEMPLET_ID or v_TEMPLET_ID is null) and a.valid=1;
    end if;

  end;

  /*****************************************************************************/
  PROCEDURE usp_EditEmrTemplet_Item(v_EditType         varchar default '', --操作类型
                                    v_MR_CLASS         VARCHAR2 default '',
                                    v_MR_CODE          VARCHAR2 default '',
                                    v_MR_NAME          VARCHAR2 default '',
                                    v_MR_ATTR          VARCHAR2 default '',
                                    v_QC_CODE          VARCHAR2 default '',
                                    v_DEPT_ID          VARCHAR2 default '',
                                    v_CREATOR_ID       VARCHAR2 default '',
                                    v_CREATE_DATE_TIME VARCHAR2 default '',
                                    v_LAST_TIME        VARCHAR2 default '',
                                    v_CONTENT_CODE     VARCHAR2 default '',
                                    v_PERMISSION       int default 0,
                                    v_VISIBLED         int default 1,
                                    v_INPUT            VARCHAR2 default '',
                                    v_HOSPITAL_CODE    VARCHAR2 default '',
                                    v_ITEM_DOC_NEW     clob default '',
                                    o_result           OUT empcurtyp) as

    v_new_MRCode        VARCHAR(10);
    v_new_HOSPITAL_CODE VARCHAR(12);
  begin
    --添加
    open o_result for
      select '' from dual;

    if v_EditType = '1' then
      begin
        --根据科室编码计算Mr_Code
        if v_DEPT_ID <> '*' then
          select v_MR_CLASS || v_DEPT_ID ||
                 lpad(nvl(max(substr(a.mr_code, -2)), 0) + 1, 2, '0') mrcode
            into v_new_MRCode
            from emrtemplet_item a
           where a.mr_class = v_MR_CLASS
             and a.dept_id = v_DEPT_ID;
        else
          select v_MR_CLASS ||
                 to_number(nvl(max(substr(a.mr_code, 3)), 0) + 1)
            into v_new_MRCode
            from emrtemplet_item a
           where a.mr_class = 'BA'
             and a.dept_id = v_DEPT_ID;
        end if;

        select a.medicalid into v_new_HOSPITAL_CODE from hospitalinfo a;

        insert into EMRTEMPLET_ITEM
          (MR_CLASS,
           MR_CODE,
           MR_NAME,
           MR_ATTR,
           QC_CODE,
           DEPT_ID,
           CREATOR_ID,
           CREATE_DATE_TIME,
           LAST_TIME,
           CONTENT_CODE,
           PERMISSION,
           VISIBLED,
           INPUT,
           HOSPITAL_CODE,
           ITEM_DOC_NEW)
        values
          (v_MR_CLASS,
           v_new_MRCode,
           v_MR_NAME,
           --v_MR_ATTR,
           '1',
           v_QC_CODE,
           v_DEPT_ID,
           v_CREATOR_ID,
           to_char(sysdate, 'yyyy-mm-dd hh24:mi:ss'),
           to_char(sysdate, 'yyyy-mm-dd hh24:mi:ss'),
           v_CONTENT_CODE,
           v_PERMISSION,
           v_VISIBLED,
           v_INPUT,
           v_new_HOSPITAL_CODE,
           v_ITEM_DOC_NEW);

        open o_result for
          select v_new_MRCode from dual;

      end;
      --修改
    elsif v_EditType = '2' then
      update EMRTEMPLET_ITEM
         set MR_CLASS = nvl(v_MR_CLASS, MR_CLASS),
             MR_NAME  = nvl(v_MR_NAME, MR_NAME),
             MR_ATTR  = nvl(v_MR_ATTR, MR_ATTR),
             QC_CODE  = nvl(v_QC_CODE, QC_CODE),
             DEPT_ID  = nvl(v_DEPT_ID, DEPT_ID),
             LAST_TIME     = to_char(sysdate, 'yyyy-mm-dd hh24:mi:ss'),
             CONTENT_CODE  = nvl(v_CONTENT_CODE, CONTENT_CODE),
             PERMISSION    = nvl(v_PERMISSION, PERMISSION),
             VISIBLED      = nvl(v_VISIBLED, VISIBLED),
             INPUT         = nvl(v_INPUT, INPUT),
             HOSPITAL_CODE = nvl(v_HOSPITAL_CODE, HOSPITAL_CODE),
             ITEM_DOC_NEW  = v_ITEM_DOC_NEW
       where MR_CODE = v_MR_CODE;

      open o_result for
        select v_MR_CODE from dual;
      --删除
    elsif v_EditType = '3' then

      delete EMRTEMPLET_ITEM where MR_CODE = v_MR_CODE;

      --查询
    elsif v_EditType = '4' then
      open o_result for
        select *
          from EMRTEMPLET_ITEM a
         where (a.mr_code = v_MR_CODE or v_MR_CODE is null);
    end if;

  end;

  /**********************************************************************************/
  PROCEDURE usp_EditEmrTempletHeader(v_EditType        varchar default '', --操作类型
                                     v_HEADER_ID       VARCHAR2 default '',
                                     v_NAME            VARCHAR2 default '',
                                     v_CREATOR_ID      VARCHAR2 default '',
                                     v_CREATE_DATETIME VARCHAR2 default '',
                                     v_LAST_TIME       VARCHAR2 default '',
                                     v_HOSPITAL_CODE   VARCHAR2 default '',
                                     v_CONTENT         CLOB default '',
                                     o_result          OUT empcurtyp) as
    v_new_Header_ID varchar2(40);
  begin
    --添加
    if v_EditType = '1' then

      select seq_EmrTempletHeader_ID.Nextval
        into v_new_Header_ID
        from dual;

      insert into emrtempletHeader
        (HEADER_ID,
         NAME,
         CREATOR_ID,
         CREATE_DATETIME,
         LAST_TIME,
         HOSPITAL_CODE,
         CONTENT)
      values
        (v_new_Header_ID,
         v_NAME,
         v_CREATOR_ID,
         to_char(sysdate,'yyyy-mm-dd hh24:mi:ss'),
         to_char(sysdate,'yyyy-mm-dd hh24:mi:ss'),
         v_HOSPITAL_CODE,
         v_CONTENT);

      open o_result for
        select v_new_Header_ID from dual;

      --修改
    elsif v_EditType = '2' then

      UPDATE EMRTEMPLETHEADER
         SET NAME            = nvl(v_NAME, NAME),
             LAST_TIME       = nvl(to_char(sysdate,'yyyy-mm-dd hh24:mi:ss'), LAST_TIME),
             HOSPITAL_CODE   = nvl(v_HOSPITAL_CODE, HOSPITAL_CODE),
             CONTENT         = nvl(v_CONTENT, CONTENT)
       where HEADER_ID = v_HEADER_ID;

      open o_result for
        select v_HEADER_ID from dual;
      --删除
    elsif v_EditType = '3' then

      delete EMRTEMPLETHEADER where HEADER_ID = v_HEADER_ID;

      --查询
    elsif v_EditType = '4' then
      open o_result for
        select *
          from EMRTEMPLETHEADER
         where HEADER_ID = v_HEADER_ID
            or v_HEADER_ID is null;
    end if;

  end;

/*********************************************************************************************************/
PROCEDURE usp_EditEmrTemplet_Foot(v_EditType        varchar default '', --操作类型
                                     v_FOOT_ID       VARCHAR2 default '',
                                     v_NAME            VARCHAR2 default '',
                                     v_CREATOR_ID      VARCHAR2 default '',
                                     v_CREATE_DATETIME VARCHAR2 default '',
                                     v_LAST_TIME       VARCHAR2 default '',
                                     v_HOSPITAL_CODE   VARCHAR2 default '',
                                     v_CONTENT         CLOB default '',
                                     o_result          OUT empcurtyp) as
    v_new_Foot_ID varchar2(40);
  begin
    --添加
    if v_EditType = '1' then

      select SEQ_emrtemplet_foot_ID.Nextval
        into v_new_Foot_ID
        from dual;

      insert into emrtemplet_foot
        (Foot_ID,
         NAME,
         CREATOR_ID,
         CREATE_DATETIME,
         LAST_TIME,
         HOSPITAL_CODE,
         CONTENT)
      values
        (v_new_Foot_ID,
         v_NAME,
         v_CREATOR_ID,
         to_char(sysdate,'yyyy-mm-dd hh24:mi:ss'),
         to_char(sysdate,'yyyy-mm-dd hh24:mi:ss'),
         v_HOSPITAL_CODE,
         v_CONTENT);

      open o_result for
        select v_new_Foot_ID from dual;

      --修改
    elsif v_EditType = '2' then

      UPDATE emrtemplet_foot
         SET NAME            = nvl(v_NAME, NAME),
             LAST_TIME       = nvl(to_char(sysdate,'yyyy-mm-dd hh24:mi:ss'), LAST_TIME),
             HOSPITAL_CODE   = nvl(v_HOSPITAL_CODE, HOSPITAL_CODE),
             CONTENT         = nvl(v_CONTENT, CONTENT)
       where Foot_ID = v_FOOT_ID;

      open o_result for
        select v_FOOT_ID from dual;
      --删除
    elsif v_EditType = '3' then

      delete emrtemplet_foot where Foot_ID = v_FOOT_ID;

      --查询
    elsif v_EditType = '4' then
      open o_result for
        select *
          from emrtemplet_foot
         where foot_ID = v_FOOT_ID
            or v_FOOT_ID is null;
    end if;

  end;

  --保存诊断按钮
  PROCEDURE usp_SaveDiagButton(v_diagname   varchar) AS
  BEGIN
    MERGE INTO patdiagtype p
    USING (SELECT v_diagname diagname FROM dual) a
    ON (p.diagname = a.diagname)
    WHEN NOT MATCHED THEN
    INSERT VALUES
    (
        (SELECT max(sno) + 1 FROM patdiagtype),
        (SELECT max(sno) + 1 FROM patdiagtype),
        v_diagname,
        '2'
    );
  END;
  
/*    --更新诊断按钮
  PROCEDURE usp_UpdateDiagButton(v_code varchar default '',
                                v_diagname   varchar default '') AS
  BEGIN
\*    MERGE INTO patdiagtype p
    USING (SELECT v_diagname diagname FROM dual) a
    ON (p.diagname = a.diagname)
    WHEN NOT MATCHED THEN*\
    Update patdiagtype
    set
        SNO = to_number(v_code),
        DIAGNAME = v_diagname,
        TYPEID = '2'
        WHERE CODE = v_code;
    END;*/

  --个人模版维护
  PROCEDURE usp_EditMyEmrTemplet(v_EditType        varchar default '',
                               v_CODE      VARCHAR2 default '',
                               v_CREATEUSER      VARCHAR2 default '',
                               v_MR_CLASS        VARCHAR2 default '',
                               v_MR_NAME         VARCHAR2 default '',
                               v_EMRTEMLETCONTENT     clob default '',
                               o_result          OUT empcurtyp) as

  begin
    open o_result for
      select '' from dual;

    if v_EditType = '1' then

      insert into DOCTOREMRTEMPLET
        ( CODE,
         CREATEUSER,
         CREATEDATE,
         MR_CLASS,
         NAME,
          EMRTEMLETCONTENT
         )
      values
        (v_CODE,
         v_CREATEUSER,
         to_char(sysdate, 'yyyy-mm-dd hh24:mi:ss'),
         v_MR_CLASS,
         v_MR_NAME,
         v_EMRTEMLETCONTENT
         );

      open o_result for
        select v_CODE from dual;

    elsif v_EditType = '2' then

      UPDATE DOCTOREMRTEMPLET
         SET
             name         = nvl(v_MR_NAME, name),
             EMRTEMLETCONTENT     = v_EMRTEMLETCONTENT
       WHERE CODE = v_CODE and CREATEUSER=v_CREATEUSER;

      open o_result for
        select v_CODE from dual;

    elsif v_EditType = '3' then

    delete DOCTOREMRTEMPLET where code = v_CODE and CREATEUSER=v_CREATEUSER;

    elsif v_EditType = '4' then
      open o_result for
               select e.templet_id,
               substr(e.mr_name,instr(e.mr_name,'-',1,2)+1)ordername,
               e.file_name,
               e.dept_id,
               b.createuser creator_id,
               b.createdate create_datetime,
               e.last_time,
               e.permission,
               e.mr_class,
               e.mr_code,
               e.code,
               e.mr_attr,
               e.qc_code,
               e.new_page_flag,
               b.emrtemletcontent XML_DOC_NEW,
               e.file_flag,
               e.write_times,
               e.hospital_code,
               nvl(e.isfirstdaily, 0) isfirstdaily,
               e.isshowfilename,
               e.isyihuangoutong,
               e.NEW_PAGE_END,
               e.valid,
               e.State,
               e.isconfigpagesize,
               e.auditor,
               e.auditdate,
              (case when (select count(1)
              from templet2hisdept t
              where t.templetid = e.templet_id)>0 then '??' || mr_name else mr_name end )mr_name
                from emrtemplet e ,DOCTOREMRTEMPLET b
              where e.valid = 1 and e.templet_id=b.code and b.createuser= v_CREATEUSER and b.code = v_CODE or v_CODE is null;

    end if;

  end;
end;
/
