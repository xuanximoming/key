prompt Importing table APPCFG...
set feedback off
set define off
insert into APPCFG (CONFIGKEY, NAME, VALUE, DESCRIPT, PARAMTYPE, CFGKEYSET, DESIGN, CLIENTFLAG, HIDE, VALID, CREATEDATE, SORTID)
values ('Namerec', '自定义病历名称', '0', '0-不开启 1-开启', 1, null, null, 0, 0, 1, to_date('09-07-2020', 'dd-mm-yyyy'), null);

-- Add comments to the columns 
comment on column IEM_MAINPAGE_OPERATION_SX.operation_date
  is '手术时间';
comment on column IEM_MAINPAGE_OPERATION_SX.iem_mainpage_operation_no
  is '手术编号';
comment on column IEM_MAINPAGE_OPERATION_SX.iem_mainpage_no
  is '病案编号';
comment on column IEM_MAINPAGE_OPERATION_SX.operation_code
  is '手术编码';
comment on column IEM_MAINPAGE_OPERATION_SX.operation_name
  is '手术名称';
comment on column IEM_MAINPAGE_OPERATION_SX.execute_user1
  is '术者';
comment on column IEM_MAINPAGE_OPERATION_SX.execute_user2
  is '一助';
comment on column IEM_MAINPAGE_OPERATION_SX.execute_user3
  is '二助';
comment on column IEM_MAINPAGE_OPERATION_SX.anaesthesia_type_id
  is '麻醉方式';
comment on column IEM_MAINPAGE_OPERATION_SX.close_level
  is '切口愈合等级';
comment on column IEM_MAINPAGE_OPERATION_SX.anaesthesia_user
  is '麻醉医师';
comment on column IEM_MAINPAGE_OPERATION_SX.valide
  is '是否有效0-无效  1-有效';
comment on column IEM_MAINPAGE_OPERATION_SX.create_user
  is '创建者';
comment on column IEM_MAINPAGE_OPERATION_SX.create_time
  is '创建时间';
comment on column IEM_MAINPAGE_OPERATION_SX.cancel_user
  is '删除者';
comment on column IEM_MAINPAGE_OPERATION_SX.cancel_time
  is '删除时间';
comment on column IEM_MAINPAGE_OPERATION_SX.operation_level
  is '手术等级';
prompt Done.
