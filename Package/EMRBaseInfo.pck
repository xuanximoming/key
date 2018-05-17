create or replace package EMRBaseInfo is

  TYPE empcurtype IS REF CURSOR;

  --插入或修改诊断别名表
  PROCEDURE usp_AddOrModDiaOther(v_id    varchar2,
                                 v_ICDID varchar2,
                                 v_NAME  varchar2,
                                 v_PY    varchar2,
                                 v_WB    varchar2,
                                 v_VALID varchar2);

  --插入或修改中医诊断别名表
  PROCEDURE usp_AddOrModDiaChiOther(v_id    varchar2,
                                    v_ICDID varchar2,
                                    v_NAME  varchar2,
                                    v_PY    varchar2,
                                    v_WB    varchar2,
                                    v_VALID varchar2);

  --插入或修改手术别名表
  PROCEDURE usp_AddOrModOperOther(v_id    varchar2,
                                  v_ICDID varchar2,
                                  v_NAME  varchar2,
                                  v_PY    varchar2,
                                  v_WB    varchar2,
                                  v_VALID varchar2);

end EMRBaseInfo;
/
create or replace package body EMRBaseInfo is

  --插入或修改诊断别名表
  PROCEDURE usp_AddOrModDiaOther(v_id    varchar2,
                                 v_ICDID varchar2,
                                 v_NAME  varchar2,
                                 v_PY    varchar2,
                                 v_WB    varchar2,
                                 v_VALID varchar2) AS
    v_count integer;
  BEGIN
    select count(*)
      into v_count
      from diagnosisothername
     where diagnosisothername.id = v_id;
    IF v_count <= 0 THEN
      insert into diagnosisothername
        (id, icdid, name, py, wb, valid)
      values
        (v_id, v_icdid, v_name, zlspellcode(v_name),  get_wb(v_name), '1');
    ELSE
      update diagnosisothername cn
         set cn.name  = v_NAME,
             cn.py    = zlspellcode(v_name),
             cn.wb    = get_wb(v_name),
             cn.valid = v_VALID
       where cn.id = v_id;
    end if;
  END;

  --插入或修改中医诊断别名表
  PROCEDURE usp_AddOrModDiaChiOther(v_id    varchar2,
                                    v_ICDID varchar2,
                                    v_NAME  varchar2,
                                    v_PY    varchar2,
                                    v_WB    varchar2,
                                    v_VALID varchar2) AS
    v_count integer;
  BEGIN
    select count(*)
      into v_count
      from diagnosischiothername
     where diagnosischiothername.id = v_id;
    IF v_count <= 0 THEN
      insert into diagnosischiothername
        (id, icdid, name, py, wb, valid)
      values
        (v_id, v_icdid, v_name, zlspellcode(v_name),  get_wb(v_name), '1');
    ELSE
      update diagnosischiothername cn
         set cn.name  = v_NAME,
             cn.py    = zlspellcode(v_name),
             cn.wb    = get_wb(v_name),
             cn.valid = v_VALID
       where cn.id = v_id;
    end if;
  END;

  --插入或修改手术别名表
  PROCEDURE usp_AddOrModOperOther(v_id    varchar2,
                                  v_ICDID varchar2,
                                  v_NAME  varchar2,
                                  v_PY    varchar2,
                                  v_WB    varchar2,
                                  v_VALID varchar2) AS
    v_count integer;
  BEGIN
    select count(*)
      into v_count
      from operothername
     where operothername.id = v_id;
    IF v_count <= 0 THEN
      insert into operothername
        (id, icdid, name, py, wb, valid)
      values
        (v_id, v_icdid, v_name, zlspellcode(v_name),  get_wb(v_name), '1');
    ELSE
      update operothername cn
         set cn.name  = v_NAME,
             cn.py    = zlspellcode(v_name),
             cn.wb    = get_wb(v_name),
             cn.valid = v_VALID
       where cn.id = v_id;
    end if;
  END;

end EMRBaseInfo;
/
