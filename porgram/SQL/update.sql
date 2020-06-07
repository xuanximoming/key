insert into APPCFG (CONFIGKEY, NAME, VALUE, DESCRIPT, PARAMTYPE, CFGKEYSET, DESIGN, CLIENTFLAG, HIDE, VALID, CREATEDATE, SORTID)
values ('isCnMaiPage', '是否使用中医病案首页', '1', '1：中医病案首页，0：西医病案首页', 1, null, null, 0, 0, 1, null, null);
-- Add/modify columns 
alter table IEM_MAINPAGE_DIAGNOSIS_SX add diagnosis_orien NUMBER(1);
-- Add comments to the columns 
comment on column IEM_MAINPAGE_DIAGNOSIS_SX.diagnosis_orien
  is '诊断方位：1.左侧 2.右侧 3.单侧 4.双侧';