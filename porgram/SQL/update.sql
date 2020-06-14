insert into APPCFG (CONFIGKEY, NAME, VALUE, DESCRIPT, PARAMTYPE, CFGKEYSET, DESIGN, CLIENTFLAG, HIDE, VALID, CREATEDATE, SORTID)
values ('isCnMaiPage', '是否使用中医病案首页', '1', '1：中医病案首页，0：西医病案首页', 1, null, null, 0, 0, 1, null, null);
-- Add/modify columns 
alter table IEM_MAINPAGE_DIAGNOSIS_SX add diagnosis_orien NUMBER(1);
-- Add comments to the columns 
comment on column IEM_MAINPAGE_DIAGNOSIS_SX.diagnosis_orien
  is '诊断方位：1.左侧 2.右侧 3.单侧 4.双侧';
  
-- Add/modify columns 
alter table INPATIENT add PAYWAY VARCHAR2(1);
-- Add comments to the columns 
comment on column INPATIENT.PAYWAY
  is '医保支付方式';
  
-- Add/modify columns 
alter table INPATIENT add specas varchar2(1);
-- Add comments to the columns 
comment on column INPATIENT.specas
  is '特殊病例';
  
  -- Add/modify columns 
alter table IEM_MAINPAGE_BASICINFO_SX add hospital_sense VARCHAR2(12);
-- Add comments to the columns 
comment on column IEM_MAINPAGE_BASICINFO_SX.hospital_sense
  is '院内感染代码';
  
    -- Add/modify columns 
alter table IEM_MAINPAGE_BASICINFO_SX add hospital_sense_name VARCHAR2(50);
-- Add comments to the columns 
comment on column IEM_MAINPAGE_BASICINFO_SX.hospital_sense_name
  is '院内感染';