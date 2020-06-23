insert into APPCFG (CONFIGKEY, NAME, VALUE, DESCRIPT, PARAMTYPE, CFGKEYSET, DESIGN, CLIENTFLAG, HIDE, VALID, CREATEDATE, SORTID)
values ('isCnMaiPage', '是否使用中医病案首页', '1', '1：中医病案首页，0：西医病案首页', 1, null, null, 0, 0, 1, null, null);

insert into dictionary_detail (DETAILID, MAPID, NAME, PY, WB, CATEGORYID, VALID, MEMO)
values ('1', null, '按项目', 'AXM', 'RAH', 'bb', 1, null);

insert into dictionary_detail (DETAILID, MAPID, NAME, PY, WB, CATEGORYID, VALID, MEMO)
values ('2', null, '单病种', 'DBZ', 'UUT', 'bb', 1, null);

insert into dictionary_detail (DETAILID, MAPID, NAME, PY, WB, CATEGORYID, VALID, MEMO)
values ('3', null, '按病种分值', 'ABZBZ', 'RUTWW', 'bb', 1, null);

insert into dictionary_detail (DETAILID, MAPID, NAME, PY, WB, CATEGORYID, VALID, MEMO)
values ('4', null, '疾病诊断相关分组（DRG）', 'JBZDXGBZDRG', 'UUYOSUWXdrg', 'bb', 1, null);

insert into dictionary_detail (DETAILID, MAPID, NAME, PY, WB, CATEGORYID, VALID, MEMO)
values ('5', null, '按床日', 'ACR', 'RYJ', 'bb', 1, null);

insert into dictionary_detail (DETAILID, MAPID, NAME, PY, WB, CATEGORYID, VALID, MEMO)
values ('6', null, '按人头', 'ART', 'RWU', 'bb', 1, null);

insert into dictionary_detail (DETAILID, MAPID, NAME, PY, WB, CATEGORYID, VALID, MEMO)
values ('9', null, '其他', 'QT', 'AW', 'bb', 1, null);

insert into dictionary_detail (DETAILID, MAPID, NAME, PY, WB, CATEGORYID, VALID, MEMO)
values ('0', null, '普通', 'PT', 'UC', '94', 1, null);

insert into dictionary_detail (DETAILID, MAPID, NAME, PY, WB, CATEGORYID, VALID, MEMO)
values ('1', null, '长期住院的精神类患者', 'ZQZYDJSLHZ', 'TAWBROPOKF', '94', 1, null);

insert into dictionary_detail (DETAILID, MAPID, NAME, PY, WB, CATEGORYID, VALID, MEMO)
values ('2', null, '临终关怀病床', 'LZGHBC', 'JXUNUY', '94', 1, null);

insert into dictionary_detail (DETAILID, MAPID, NAME, PY, WB, CATEGORYID, VALID, MEMO)
values ('3', null, '长期康复住院病床', 'ZQKFZYBC', 'TAYTWBUY', '94', 1, null);
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
  