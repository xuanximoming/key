-- Create table
create table IEM_MAINPAGE_DIAGNOSIS_SX
(
  iem_mainpage_diagnosis_no NUMBER(12) not null,
  iem_mainpage_no           NUMBER(12) not null,
  diagnosis_type_id         NUMBER(3) not null,
  diagnosis_code            VARCHAR2(60),
  diagnosis_name            VARCHAR2(300) not null,
  status_id                 NUMBER(3),
  order_value               NUMBER(3) not null,
  valide                    NUMBER(1) not null,
  create_user               VARCHAR2(10) not null,
  create_time               VARCHAR2(19) not null,
  cancel_user               VARCHAR2(10),
  cancel_time               VARCHAR2(19),
  type                      VARCHAR2(1),
  typename                  VARCHAR2(12),
  outstatus_id              NUMBER(1)
)
tablespace TSP_DSEMR
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 9M
    next 1M
    minextents 1
    maxextents unlimited
  );
-- Add comments to the table 
comment on table IEM_MAINPAGE_DIAGNOSIS_SX
  is 'IEM_MAINPAGE_DIAGNOSIS_SX--病案诊断表';
-- Add comments to the columns 
comment on column IEM_MAINPAGE_DIAGNOSIS_SX.iem_mainpage_diagnosis_no
  is '首页病案诊断号';
comment on column IEM_MAINPAGE_DIAGNOSIS_SX.iem_mainpage_no
  is '首页病案号';
comment on column IEM_MAINPAGE_DIAGNOSIS_SX.diagnosis_type_id
  is '诊断类型';
comment on column IEM_MAINPAGE_DIAGNOSIS_SX.diagnosis_code
  is '诊断代码';
comment on column IEM_MAINPAGE_DIAGNOSIS_SX.diagnosis_name
  is '诊断名称';
comment on column IEM_MAINPAGE_DIAGNOSIS_SX.status_id
  is '入院病情';
comment on column IEM_MAINPAGE_DIAGNOSIS_SX.order_value
  is '诊断序号';
comment on column IEM_MAINPAGE_DIAGNOSIS_SX.valide
  is '是否有效1有效，0无效';
comment on column IEM_MAINPAGE_DIAGNOSIS_SX.create_user
  is '创建人用户名';
comment on column IEM_MAINPAGE_DIAGNOSIS_SX.create_time
  is '创建时间';
comment on column IEM_MAINPAGE_DIAGNOSIS_SX.cancel_user
  is '删除人';
comment on column IEM_MAINPAGE_DIAGNOSIS_SX.cancel_time
  is '删除时间';
comment on column IEM_MAINPAGE_DIAGNOSIS_SX.type
  is '诊断类型 1、西医  2、中医';
comment on column IEM_MAINPAGE_DIAGNOSIS_SX.typename
  is '诊断类型 ';
comment on column IEM_MAINPAGE_DIAGNOSIS_SX.outstatus_id
  is '出院情况： 1. 治愈 2. 好转 3. 未愈 4. 死亡 5. 其他';
