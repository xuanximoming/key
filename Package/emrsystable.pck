CREATE OR REPLACE PACKAGE emrsystable IS
  -- 维护基本数据表
  TYPE empcurtyp IS REF CURSOR;

  /*
  Diagnosis  诊断库
  */
  PROCEDURE usp_Edit_Diagnosis(v_EditType      varchar default '', --操作类型
                               v_MarkId        varchar default '',
                               v_ICD           varchar default '',
                               v_MapID         varchar default '',
                               v_StandardCode  varchar default '',
                               v_Name          varchar default '',
                               v_Py            varchar default '',
                               v_Wb            varchar default '',
                               v_TumorID       varchar default '',
                               v_Statist       varchar default '',
                               v_InnerCategory varchar default '',
                               v_Category      varchar default '',
                               v_OtherCategroy int default '',
                               v_Valid         int default 1,
                               v_Memo          varchar default '',
                               o_result        OUT empcurtyp);

  /*
  Diagnosis  科室常用诊断库
  */
  PROCEDURE usp_Edit_DeptDiagnosis(v_EditType varchar default '', --操作类型
                                   v_DeptID   varchar default '',
                                   v_MarkID   varchar default '',
                                   o_result   OUT empcurtyp);

  /*
  DiagnosisOfChinese     中医诊断库
  */
  PROCEDURE usp_Edit_DiagnosisOfChinese(v_EditType varchar default '', --操作类型
                                        v_ID       varchar default '',
                                        v_MapID    varchar default '',
                                        v_Name     varchar default '',
                                        v_Py       varchar default '',
                                        v_Wb       varchar default '',
                                        v_Valid    int default 1,
                                        v_Memo     varchar default '',
                                        v_Memo1    varchar default '',
                                        v_Category varchar default '',
                                        o_result   OUT empcurtyp);

  /*
  DiseaseCFG  病种设置库
  */
  PROCEDURE usp_Edit_DiseaseCFG(v_EditType  varchar default '', --操作类型
                                v_ID        varchar default '',
                                v_MapID     varchar default '',
                                v_Name      varchar default '',
                                v_Py        varchar default '',
                                v_Wb        varchar default '',
                                v_DiseaseID varchar default '',
                                v_SurgeryID varchar default '',
                                v_Category  int default '',
                                v_Mark      varchar default '',
                                v_ParentID  varchar default '',
                                v_Valid     int default 1,
                                v_Memo      varchar default '',
                                o_result    OUT empcurtyp);

  /*
  Surgery  手术代码库
  */
  PROCEDURE usp_Edit_Surgery(v_EditType     varchar default '', --操作类型
                             v_ID           varchar default '',
                             v_MapID        varchar default '',
                             v_StandardCode varchar default '',
                             v_Name         varchar default '',
                             v_Py           varchar default '',
                             v_Wb           varchar default '',
                             v_Valid        int default 1,
                             v_Memo         varchar default '',
                             v_bzlb         varchar default '',
                             v_sslb         int default '',
                             o_result       OUT empcurtyp);

  /*
  Toxicosis  损伤中毒库
  */
  PROCEDURE usp_Edit_Toxicosis(v_EditType     varchar default '', --操作类型
                               v_ID           varchar default '',
                               v_MapID        varchar default '',
                               v_StandardCode varchar default '',
                               v_Name         varchar default '',
                               v_Py           varchar default '',
                               v_Wb           varchar default '',
                               v_Valid        int default 1,
                               v_Memo         varchar default '',
                               o_result       OUT empcurtyp);
                               
   ------------手术信息的维护                            
  PROCEDURE usp_Edit_OperInfo(v_EditType     varchar default '', --操作类型
                             v_ID           varchar default '',
                             v_MapID        varchar default '',
                             --v_StandardCode varchar default '',
                             v_Name         varchar default '',
                             v_Py           varchar default '',
                             v_Wb           varchar default '',
                             v_Valid        int default 1,
                             v_Memo         varchar default '',
                           --  v_bzlb         varchar default '',
                             v_sslb         int default '',
                             o_result       OUT empcurtyp);

  /*
  Tumor  肿瘤库
  */
  PROCEDURE usp_Edit_Tumor(v_EditType     varchar default '', --操作类型
                           v_ID           varchar default '',
                           v_MapID        varchar default '',
                           v_StandardCode varchar default '',
                           v_Name         varchar default '',
                           v_Py           varchar default '',
                           v_Wb           varchar default '',
                           v_Valid        int default 1,
                           v_Memo         varchar default '',
                           o_result       OUT empcurtyp);
                           
                           
                           
                           
                           
                           
                           
/*
  Tumor  模板工厂库
*/                      
                           
PROCEDURE usp_Edit_ModelEmr(v_EditType      varchar default '', --操作类型
                               v_ModelId        varchar default '',
                               v_DestEmrName           varchar default '',
                               v_SourceEmrName         varchar default '',
                               v_DestItemName  varchar default '',
                               v_SourceItemName          varchar default '',
                               v_Valid         int default 1,
                               o_result        OUT empcurtyp);                         
                           
                                              

end;
/
CREATE OR REPLACE PACKAGE BODY EMRSYSTABLE IS
  /*******************************************************************************/
  PROCEDURE usp_Edit_Diagnosis(v_EditType      varchar default '', --操作类型
                               v_MarkId        varchar default '',
                               v_ICD           varchar default '',
                               v_MapID         varchar default '',
                               v_StandardCode  varchar default '',
                               v_Name          varchar default '',
                               v_Py            varchar default '',
                               v_Wb            varchar default '',
                               v_TumorID       varchar default '',
                               v_Statist       varchar default '',
                               v_InnerCategory varchar default '',
                               v_Category      varchar default '',
                               v_OtherCategroy int default '',
                               v_Valid         int default 1,
                               v_Memo          varchar default '',
                               o_result        OUT empcurtyp) as
    /*
    Diagnosis  诊断库
    */
  begin

    open o_result for
      select v_MarkId from dual;
    --添加
    if v_EditType = '1' then

      INSERT INTO Diagnosis
        (MarkId,
         ICD,
         MapID,
         StandardCode,
         Name,
         Py,
         Wb,
         TumorID,
         Statist,
         InnerCategory,
         Category,
         OtherCategroy,
         Valid,
         Memo)
      VALUES
        (v_MarkId,
         v_ICD,
         v_MapID,
         v_StandardCode,
         v_Name,
         v_Py,
         v_Wb,
         v_TumorID,
         v_Statist,
         v_InnerCategory,
         v_Category,
         v_OtherCategroy,
         v_Valid,
         v_Memo);

      --修改
    elsif v_EditType = '2' then

      UPDATE Diagnosis
         SET ICD           = nvl(v_ICD, ICD),
             MapID         = nvl(v_MapID, MapID),
             --StandardCode  = nvl(v_StandardCode, StandardCode),
             StandardCode  = v_StandardCode,
             Name          = nvl(v_Name, Name),
             Py            = nvl(v_Py, Py),
             Wb            = nvl(v_Wb, Wb),
             --TumorID       = nvl(v_TumorID, TumorID),
             TumorID       = v_TumorID,
             --Statist       = nvl(v_Statist, Statist),
             Statist       = v_Statist,
             --InnerCategory = nvl(v_InnerCategory, InnerCategory),
             InnerCategory = v_InnerCategory,
             --Category      = nvl(v_Category, Category),
             Category      = v_Category,
             --OtherCategroy = nvl(v_OtherCategroy, OtherCategroy),
             OtherCategroy = v_OtherCategroy,
             Valid         = nvl(v_Valid, Valid),
             --Memo          = nvl(v_Memo, Memo)
             Memo          = v_Memo
       WHERE MarkId = v_MarkId;

      --删除
    elsif v_EditType = '3' then

      /*UPDATE Diagnosis SET Valid = 0 WHERE MarkId = v_MarkId;*/
      delete diagnosis WHERE MarkId = v_MarkId;

      --查询
    elsif v_EditType = '4' then
      open o_result for

        select b.id statistid,b.name statistname,
               /*a.statist,*/
               c.id innercategoryid,
               c.name innercategoryname,
               /*a.innercategory,*/
               d.id categoryid,
               d.name categoryname,
               /*a.category,*/
               e.id othercategroyid,
               e.name othercategroyname,
               /*a.othercategroy,*/
               t.name tumorname,
               /*a.tumorid,*/
               (case
                 when a.valid = 1 then
                  '是'
                 else
                  '否'
               end) validName,
               a.*
          from Diagnosis a
          left join diseasecfg b on trim(a.statist) = trim(to_char(b.id))
                                and b.category = '700'
          left join diseasecfg c on trim(a.innercategory) =
                                    trim(to_char(c.id))
                                and c.category = '702'
          left join diseasecfg d on trim(a.category) = trim(to_char(d.id))
                                and d.category = '701'
          left join categorydetail e on trim(a.othercategroy) =
                                        trim(to_char(e.id))
                                    and e.categoryid = 9
          left join Tumor t on a.tumorid = t.id
         where (a.MarkId = v_MarkId or v_MarkId is null);
      /*select (case when a.valid =1 then '是' else '否' end) validName,a.*
       from Diagnosis a
      where (a.MarkId = v_MarkId or v_MarkId is null);*/
    end if;

  end;

  PROCEDURE usp_Edit_DeptDiagnosis(v_EditType varchar default '', --操作类型
                                   v_DeptID   varchar default '',
                                   v_MarkID   varchar default '',
                                   o_result   OUT empcurtyp) as
    /*
    Diagnosis  科室常用诊断库
    */
    v_sql VARCHAR(4000);
  begin

    open o_result for
      select v_MarkId from dual;
    --添加
    if v_EditType = '1' then

      delete DiagnosisToDept WHERE DeptID = v_DeptID;

      v_sql := '
      INSERT INTO DiagnosisToDept
        (MarkId,
         ICD,
         MapID,
         StandardCode,
         Name,
         Py,
         Wb,
         TumorID,
         Statist,
         InnerCategory,
         Category,
         OtherCategroy,
         Valid,
         Memo,
         DeptID)
        select MarkId,
               ICD,
               MapID,
               StandardCode,
               Name,
               Py,
               Wb,
               TumorID,
               Statist,
               InnerCategory,
               Category,
               OtherCategroy,
               Valid,
               Memo,
               ' || v_DeptID || '
          from diagnosis
         where MarkId in (' || v_MarkID || ')';

      EXECUTE IMMEDIATE v_sql;

      commit;
      open o_result for
        select 1 from dual;

      --查询
    elsif v_EditType = '2' then
      open o_result for

        select b.name statistname,
               /*a.statist,*/
               c.name innercategoryname,
               /*a.innercategory,*/
               d.name categoryname,
               /*a.category,*/
               e.name othercategroyname,
               /*a.othercategroy,*/
               t.name tumorname,
               /*a.tumorid,*/
               (case
                 when a.valid = 1 then
                  '是'
                 else
                  '否'
               end) validName,
               a.*
          from DiagnosisToDept a
          left join diseasecfg b on trim(a.statist) = trim(to_char(b.id))
                                and b.category = '700'
          left join diseasecfg c on trim(a.innercategory) =
                                    trim(to_char(c.id))
                                and c.category = '702'
          left join diseasecfg d on trim(a.category) = trim(to_char(d.id))
                                and d.category = '701'
          left join categorydetail e on trim(a.othercategroy) =
                                        trim(to_char(e.id))
                                    and e.categoryid = 9
          left join Tumor t on a.tumorid = t.id
         where a.deptid = v_DeptID;
      /*select (case when a.valid =1 then '是' else '否' end) validName,a.*
       from Diagnosis a
      where (a.MarkId = v_MarkId or v_MarkId is null);*/
    end if;

  end;

  /*******************************************************************************/
  PROCEDURE usp_Edit_DiagnosisOfChinese(v_EditType varchar default '', --操作类型
                                        v_ID       varchar default '',
                                        v_MapID    varchar default '',
                                        v_Name     varchar default '',
                                        v_Py       varchar default '',
                                        v_Wb       varchar default '',
                                        v_Valid    int default 1,
                                        v_Memo     varchar default '',
                                        v_Memo1    varchar default '',
                                        v_Category varchar default '',
                                        o_result   OUT empcurtyp) as

    /*
    DiagnosisOfChinese     中医诊断库
    */
  begin
    open o_result for
      select v_ID from dual;
    --添加
    if v_EditType = '1' then

      INSERT INTO DiagnosisOfChinese
        (ID, MapID, Name, Py, Wb, Valid, Memo, Memo1, Category)
      VALUES
        (v_ID,
         v_MapID,
         v_Name,
         v_Py,
         v_Wb,
         v_Valid,
         v_Memo,
         v_Memo1,
         v_Category);

      --修改
    elsif v_EditType = '2' then

      UPDATE DiagnosisOfChinese
         SET MapID    = nvl(v_MapID, MapID),
             Name     = nvl(v_Name, Name),
             Py       = nvl(v_Py, Py),
             Wb       = nvl(v_Wb, Wb),
             Valid    = nvl(v_Valid, Valid),
             Memo     = nvl(v_Memo, Memo),
             Memo1    = nvl(v_Memo1, Memo1),
             Category = nvl(v_Category, Category)
       WHERE ID = v_ID;

      --删除
    elsif v_EditType = '3' then

      /*update diagnosisofchinese a set a.valid = 0 where a.id = v_ID;*/
      delete diagnosisofchinese WHERE id = v_ID;
      --查询
    elsif v_EditType = '4' then
      open o_result for
        select b.id categoryid,b.name categoryname,
               (case
                 when a.valid = 1 then
                  '是'
                 else
                  '否'
               end) validName,
               a.*
          from DiagnosisOfChinese a
          left join categorydetail b on to_char(a.category) =
                                        trim(to_char(b.id))
                                    and b.categoryid = 7
         where (a.ID = v_ID or v_ID is null);
    end if;

  end;

  /*******************************************************************************/
  PROCEDURE usp_Edit_DiseaseCFG(v_EditType  varchar default '', --操作类型
                                v_ID        varchar default '',
                                v_MapID     varchar default '',
                                v_Name      varchar default '',
                                v_Py        varchar default '',
                                v_Wb        varchar default '',
                                v_DiseaseID varchar default '',
                                v_SurgeryID varchar default '',
                                v_Category  int default '',
                                v_Mark      varchar default '',
                                v_ParentID  varchar default '',
                                v_Valid     int default 1,
                                v_Memo      varchar default '',
                                o_result    OUT empcurtyp) as
    /*
    DiseaseCFG  病种设置库
    */
  begin

    open o_result for
      select v_ID from dual;
    --添加
    if v_EditType = '1' then

      INSERT INTO DiseaseCFG
        (ID,
         MapID,
         Name,
         Py,
         Wb,
         DiseaseID,
         SurgeryID,
         Category,
         Mark,
         ParentID,
         Valid,
         Memo)
      VALUES
        (v_ID,
         v_MapID,
         v_Name,
         v_Py,
         v_Wb,
         v_DiseaseID,
         v_SurgeryID,
         v_Category,
         v_Mark,
         v_ParentID,
         v_Valid,
         v_Memo);

      --修改
    elsif v_EditType = '2' then

      UPDATE DiseaseCFG
         SET MapID     = nvl(v_MapID, MapID),
             Name      = nvl(v_Name, Name),
             Py        = nvl(v_Py, Py),
             Wb        = nvl(v_Wb, Wb),
             DiseaseID = nvl(v_DiseaseID, DiseaseID),
             SurgeryID = nvl(v_SurgeryID, SurgeryID),
             Category  = nvl(v_Category, Category),
             Mark      = nvl(v_Mark, MapID),
             ParentID  = nvl(v_ParentID, MapID),
             Valid     = nvl(v_Valid, Valid),
             Memo      = nvl(v_Memo, MapID)
       WHERE ID = v_ID;

      --删除
    elsif v_EditType = '3' then

      /*update DiseaseCFG a set a.valid = 0 where a.id = v_ID;*/
      delete DiseaseCFG WHERE id = v_ID;

      --查询
    elsif v_EditType = '4' then
      open o_result for
        select b.name categoryname,
               (case
                 when a.valid = 1 then
                  '是'
                 else
                  '否'
               end) validName,
               a.*
          from DiseaseCFG a
          left join CategoryDetail b on trim(to_char(b.ID)) =
                                        trim(a.category)
                                    and b.CategoryID = 7
         where (a.ID = v_ID or v_ID is null);
    end if;

  end;

  /*******************************************************************************/
  PROCEDURE usp_Edit_Surgery(v_EditType     varchar default '', --操作类型
                             v_ID           varchar default '',
                             v_MapID        varchar default '',
                             v_StandardCode varchar default '',
                             v_Name         varchar default '',
                             v_Py           varchar default '',
                             v_Wb           varchar default '',
                             v_Valid        int default 1,
                             v_Memo         varchar default '',
                             v_bzlb         varchar default '',
                             v_sslb         int default '',
                             o_result       OUT empcurtyp) as
    /*
    Surgery  手术代码库
    */
  begin

    open o_result for
      select v_ID from dual;
    --添加
    if v_EditType = '1' then

      INSERT INTO Surgery
        (ID, MapID, StandardCode, Name, Py, Wb, Valid, Memo, bzlb, sslb)
      VALUES
        (v_ID,
         v_MapID,
         v_StandardCode,
         v_Name,
         v_Py,
         v_Wb,
         v_Valid,
         v_Memo,
         v_bzlb,
         v_sslb);

      --修改
    elsif v_EditType = '2' then
      UPDATE Surgery
         SET MapID        = nvl(v_MapID, MapID),
             StandardCode = nvl(v_StandardCode, StandardCode),
             Name         = nvl(v_Name, Name),
             Py           = nvl(v_Py, Py),
             Wb           = nvl(v_Wb, Wb),
             Valid        = nvl(v_Valid, Valid),
             Memo         = nvl(v_Memo, Memo),
             bzlb         = nvl(v_bzlb, bzlb),
             sslb         = nvl(v_sslb, sslb)
       WHERE ID = v_ID;

      --删除
    elsif v_EditType = '3' then

      /*update Surgery a set a.valid = 0 where a.id = v_ID;*/
      delete Surgery WHERE id = v_ID;

      --查询
    elsif v_EditType = '4' then
      open o_result for
        select b.name bzlbname,
               c.name sslbname,
               (case
                 when a.valid = 1 then
                  '是'
                 else
                  '否'
               end) validName,
               a.*
          from Surgery a
          left join DiseaseCFG b on trim(to_char(b.ID)) = trim(a.bzlb)
                                and b.Category = 701
          left join CategoryDetail c on c.ID = a.sslb
                                    and c.CategoryID = 8
         where (a.ID = v_ID or v_ID is null);
    end if;

  end;
  
  
  
  -------------------------手术信息维护----------------------------------
   PROCEDURE usp_Edit_OperInfo(v_EditType     varchar default '', --操作类型
                             v_ID           varchar default '',
                             v_MapID        varchar default '',
                             --v_StandardCode varchar default '',
                             v_Name         varchar default '',
                             v_Py           varchar default '',
                             v_Wb           varchar default '',
                             v_Valid        int default 1,
                             v_Memo         varchar default '',
                           --  v_bzlb         varchar default '',
                             v_sslb         int default '',
                             o_result       OUT empcurtyp)
 as
    /*
    Surgery  手术代码库
    */
  begin

    open o_result for
      select v_ID from dual;
    --添加
    if v_EditType = '1' then

      INSERT INTO operation
        (ID, MapID,  Name, Py, Wb,  sslb,Valid, Memo)
      VALUES
        (V_ID,
         v_MapID,
         v_Name,
         v_Py,
         v_Wb,
          v_sslb,
         v_Valid,
         v_Memo
        );

      --修改
    elsif v_EditType = '2' then
      UPDATE operation
         SET MapID        = nvl(v_MapID, MapID),
             Name         = nvl(v_Name, Name),
             Py           = nvl(v_Py, Py),
             Wb           = nvl(v_Wb, Wb),
             Valid        = nvl(v_Valid, Valid),
             Memo         = nvl(v_Memo, Memo),
             sslb         = nvl(v_sslb, sslb)
       WHERE ID = v_ID;

      --删除
    elsif v_EditType = '3' then

      update operation a set a.valid = 0 where a.id = v_ID;
      --delete Surgery WHERE id = v_ID;

      --查询
    elsif v_EditType = '4' then
      open o_result for
        select
               c.name sslbname,
               (case
                 when a.valid = 1 then
                  '是'
                 else
                  '否'
               end) validName,
               a.*
          from operation a
          left join CategoryDetail c on c.ID = a.sslb
                                    and c.CategoryID = 8
         where (a.ID = v_ID or v_ID is null) and a.valid='1';
    end if;

  end;
  
  ---------------------------------------------------------------

  /*******************************************************************************/
  PROCEDURE usp_Edit_Toxicosis(v_EditType     varchar default '', --操作类型
                               v_ID           varchar default '',
                               v_MapID        varchar default '',
                               v_StandardCode varchar default '',
                               v_Name         varchar default '',
                               v_Py           varchar default '',
                               v_Wb           varchar default '',
                               v_Valid        int default 1,
                               v_Memo         varchar default '',
                               o_result       OUT empcurtyp) as
    /*
    Toxicosis  损伤中毒库
    */
  begin
    open o_result for
      select v_ID from dual;
    --添加
    if v_EditType = '1' then

      INSERT INTO Toxicosis
        (ID, MapID, StandardCode, Name, Py, Wb, Valid, Memo)
      VALUES
        (v_ID,
         v_MapID,
         v_StandardCode,
         v_Name,
         v_Py,
         v_Wb,
         v_Valid,
         v_Memo);

      --修改
    elsif v_EditType = '2' then

      UPDATE Toxicosis
         set MapID        = nvl(v_MapID, MapID),
             StandardCode = nvl(v_StandardCode, StandardCode),
             Name         = nvl(v_Name, Name),
             Py           = nvl(v_Py, Py),
             Wb           = nvl(v_Wb, Wb),
             Valid        = nvl(v_Valid, Valid),
             Memo         = nvl(v_Memo, Memo)
       WHERE id = v_ID;

      --删除
    elsif v_EditType = '3' then

      /*update Toxicosis a set a.valid = 0 where MapID = v_MapID;*/
      delete Toxicosis WHERE id = v_ID;

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
          from Toxicosis a
         where (a.id = v_ID or v_ID is null);
    end if;

  end;



  /******************************************模板工厂操作*************************************/
  PROCEDURE usp_Edit_ModelEmr(v_EditType      varchar default '', --操作类型
                               v_ModelId        varchar default '',
                               v_DestEmrName           varchar default '',
                               v_SourceEmrName         varchar default '',
                               v_DestItemName  varchar default '',
                               v_SourceItemName          varchar default '',
                               v_Valid         int default 1,
                               o_result        OUT empcurtyp) as
    /*
    Toxicosis  模板工厂库
    */
  begin
    open o_result for
      select v_ModelId from dual;
    --添加
    if v_EditType = '1' then

      INSERT INTO emr_replace_item
        (ID,dest_emrname ,source_emrname ,dest_itemname ,source_itemname , Valid)
      VALUES
        (v_ModelId,
         v_DestEmrName,
         v_SourceEmrName,
         v_DestItemName,
         v_SourceItemName,
         v_Valid);

      --修改
    elsif v_EditType = '2' then

      UPDATE emr_replace_item
         set ID        = nvl(v_ModelId, ID),
           -- dest_emrname  = nvl(v_DestEmrName, dest_emrname),
            dest_emrname  = v_DestEmrName,
           /* source_emrname= nvl(v_SourceEmrName, source_emrname),
             dest_itemname = nvl(v_DestItemName, dest_itemname),
              source_itemname= nvl(v_SourceItemName, source_itemname),
             Valid        = nvl(v_Valid, Valid)*/
               source_emrname= v_SourceEmrName,
             dest_itemname = v_DestItemName, 
              source_itemname= v_SourceItemName,
             Valid        = v_Valid
       WHERE id = v_ModelId;

      --删除
    elsif v_EditType = '3' then

     update emr_replace_item a set a.valid = 0 where ID = v_ModelId;
      /*delete Toxicosis WHERE id = v_ID;*/

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
          from emr_replace_item a
         where (a.id = v_ModelId or v_ModelId is null) and a.valid='1' order by id;
    end if;

  end;



  /*******************************************************************************/
  PROCEDURE usp_Edit_Tumor(v_EditType     varchar default '', --操作类型
                           v_ID           varchar default '',
                           v_MapID        varchar default '',
                           v_StandardCode varchar default '',
                           v_Name         varchar default '',
                           v_Py           varchar default '',
                           v_Wb           varchar default '',
                           v_Valid        int default 1,
                           v_Memo         varchar default '',
                           o_result       OUT empcurtyp) as
    /*
    Tumor  肿瘤库
    */
  begin
    open o_result for
      select v_ID from dual;
    --添加
    if v_EditType = '1' then

      INSERT INTO Tumor
        (ID, MapID, StandardCode, Name, Py, Wb, Valid, Memo)
      VALUES
        (v_ID,
         v_MapID,
         v_StandardCode,
         v_Name,
         v_Py,
         v_Wb,
         v_Valid,
         v_Memo);

      --修改
    elsif v_EditType = '2' then

      UPDATE Tumor
         SET MapID        = nvl(v_MapID, MapID),
             StandardCode = nvl(v_StandardCode, StandardCode),
             Name         = nvl(v_Name, Name),
             Py           = nvl(v_Py, Py),
             Wb           = nvl(v_Wb, Wb),
             Valid        = nvl(v_Valid, Valid),
             Memo         = nvl(v_Memo, Memo)
       WHERE ID = v_ID;

      --删除
    elsif v_EditType = '3' then

      /*update Tumor a set a.valid = 0 where ID = v_ID;*/
      delete Tumor WHERE ID = v_ID;
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
          from Tumor a
         where (ID = v_ID or v_ID is null);
    end if;

  end;

/*******************************************************************************/

end;
/
