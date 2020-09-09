-- Add comments to the columns 
comment on column USERS.id
  is '职工ID';
comment on column USERS.name
  is '姓名';
comment on column USERS.py
  is '姓名拼音';
comment on column USERS.wb
  is '姓名五笔';
comment on column USERS.sexy
  is '性别';
comment on column USERS.birth
  is '出生日期';
comment on column USERS.marital
  is '婚姻状态';
comment on column USERS.idno
  is '身份证号';
comment on column USERS.deptid
  is '科室代码';
comment on column USERS.wardid
  is '病区代码';
comment on column USERS.category
  is '职工类型';
comment on column USERS.jobtitle
  is '员工职称';
comment on column USERS.recipeid
  is '处方帐号';
comment on column USERS.recipemark
  is '处方权';
comment on column USERS.narcosismark
  is '麻醉处方权';
comment on column USERS.groupid
  is '医嘱组ID';
comment on column USERS.grade
  is '医生级别';
comment on column USERS.passwd
  is '密码';
comment on column USERS.jobid
  is '所属岗位';
comment on column USERS.regdate
  is '注册时间';
comment on column USERS.operator
  is '手术医师标识';
comment on column USERS.status
  is '帐号状态';
comment on column USERS.valid
  is '是否有效';
comment on column USERS.memo
  is '备注';
comment on column USERS.deptorward
  is '科室权限对应科室或病区代码';
comment on column USERS.power
  is '科室权限';
  -- Add/modify columns 
alter table USERS add practicecode varchar2(15);
-- Add comments to the columns 
comment on column USERS.practicecode
  is '医师执业证书编码';
