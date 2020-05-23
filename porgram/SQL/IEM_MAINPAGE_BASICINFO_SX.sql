-- Create table
create table IEM_MAINPAGE_BASICINFO_SX
(
  iem_mainpage_no          NUMBER(12) not null,
  patnoofhis               VARCHAR2(14) not null,
  noofinpat                NUMBER(9) not null,
  payid                    VARCHAR2(4),
  socialcare               VARCHAR2(32),
  incount                  INTEGER,
  name                     VARCHAR2(60),
  sexid                    VARCHAR2(4),
  birth                    CHAR(10),
  marital                  VARCHAR2(1),
  jobid                    VARCHAR2(4),
  nationid                 VARCHAR2(4),
  nationalityid            VARCHAR2(4),
  idno                     VARCHAR2(18),
  organization             VARCHAR2(64),
  officeplace              VARCHAR2(64),
  officetel                VARCHAR2(16),
  officepost               VARCHAR2(16),
  contactperson            VARCHAR2(32),
  relationship             VARCHAR2(4),
  contactaddress           VARCHAR2(255),
  contacttel               VARCHAR2(16),
  admitdate                VARCHAR2(19),
  admitdept                VARCHAR2(12),
  admitward                VARCHAR2(12),
  trans_date               VARCHAR2(19),
  trans_admitdept          VARCHAR2(12),
  trans_admitward          VARCHAR2(12),
  outwarddate              VARCHAR2(19),
  outhosdept               VARCHAR2(12),
  outhosward               VARCHAR2(12),
  actualdays               NUMBER(12),
  pathology_diagnosis_name VARCHAR2(300),
  pathology_observation_sn VARCHAR2(60),
  allergic_drug            VARCHAR2(300),
  section_director         VARCHAR2(20),
  director                 VARCHAR2(20),
  vs_employee_code         VARCHAR2(20),
  resident_employee_code   VARCHAR2(20),
  refresh_employee_code    VARCHAR2(20),
  duty_nurse               VARCHAR2(20),
  interne                  VARCHAR2(20),
  coding_user              VARCHAR2(20),
  medical_quality_id       NUMBER(1),
  quality_control_doctor   VARCHAR2(20),
  quality_control_nurse    VARCHAR2(20),
  quality_control_date     VARCHAR2(19),
  xay_sn                   VARCHAR2(300),
  ct_sn                    VARCHAR2(300),
  mri_sn                   VARCHAR2(300),
  dsa_sn                   VARCHAR2(300),
  bloodtype                NUMBER(3),
  rh                       NUMBER(1),
  is_completed             VARCHAR2(1),
  completed_time           VARCHAR2(19),
  valide                   NUMBER(1) not null,
  create_user              VARCHAR2(10) not null,
  create_time              VARCHAR2(19) not null,
  modified_user            VARCHAR2(10),
  modified_time            VARCHAR2(19),
  zymosis                  VARCHAR2(300),
  hurt_toxicosis_ele_id    VARCHAR2(30),
  hurt_toxicosis_ele_name  VARCHAR2(100),
  zymosisstate             VARCHAR2(1),
  pathology_diagnosis_id   VARCHAR2(16),
  monthage                 VARCHAR2(10),
  weight                   VARCHAR2(10),
  inweight                 VARCHAR2(10),
  inhostype                VARCHAR2(1),
  outhostype               VARCHAR2(1),
  receivehospital          VARCHAR2(300),
  receivehospital2         VARCHAR2(300),
  againinhospital          VARCHAR2(1),
  againinhospitalreason    VARCHAR2(300),
  beforehoscomaday         VARCHAR2(4),
  beforehoscomahour        VARCHAR2(4),
  beforehoscomaminute      VARCHAR2(4),
  laterhoscomaday          VARCHAR2(4),
  laterhoscomahour         VARCHAR2(4),
  laterhoscomaminute       VARCHAR2(4),
  cardnumber               VARCHAR2(40),
  allergic_flag            VARCHAR2(1),
  autopsy_flag             VARCHAR2(1),
  csd_provinceid           VARCHAR2(100),
  csd_cityid               VARCHAR2(10),
  csd_districtid           VARCHAR2(10),
  csd_provincename         VARCHAR2(300),
  csd_cityname             VARCHAR2(300),
  csd_districtname         VARCHAR2(300),
  xzz_provinceid           VARCHAR2(100),
  xzz_cityid               VARCHAR2(10),
  xzz_districtid           VARCHAR2(10),
  xzz_provincename         VARCHAR2(300),
  xzz_cityname             VARCHAR2(300),
  xzz_districtname         VARCHAR2(300),
  xzz_tel                  VARCHAR2(20),
  xzz_post                 VARCHAR2(15),
  hkdz_provinceid          VARCHAR2(100),
  hkdz_cityid              VARCHAR2(10),
  hkdz_districtid          VARCHAR2(10),
  hkdz_provincename        VARCHAR2(300),
  hkdz_cityname            VARCHAR2(300),
  hkdz_districtname        VARCHAR2(300),
  hkdz_post                VARCHAR2(15),
  jg_provinceid            VARCHAR2(100),
  jg_cityid                VARCHAR2(10),
  jg_provincename          VARCHAR2(300),
  jg_cityname              VARCHAR2(300),
  age                      VARCHAR2(10),
  cure_type                VARCHAR2(4),
  mzzyzd_name              VARCHAR2(300),
  mzzyzd_code              VARCHAR2(60),
  mzxyzd_name              VARCHAR2(300),
  mzxyzd_code              VARCHAR2(60),
  sslclj                   VARCHAR2(4),
  zyzj                     VARCHAR2(4),
  zyzlsb                   VARCHAR2(4),
  zyzljs                   VARCHAR2(4),
  bzsh                     VARCHAR2(4),
  outhosstatus             VARCHAR2(4),
  jbynzz                   VARCHAR2(4),
  mzycy                    VARCHAR2(4),
  inandouthos              VARCHAR2(4),
  lcybl                    VARCHAR2(4),
  fsybl                    VARCHAR2(4),
  qjcount                  VARCHAR2(10),
  successcount             VARCHAR2(10),
  inpatly                  VARCHAR2(4),
  asascore                 VARCHAR2(10),
  typehospital             VARCHAR2(50),
  ryxyzd_code              VARCHAR2(60),
  ryxyzd_name              VARCHAR2(300),
  inhosinfo                VARCHAR2(1),
  maindiagdate             VARCHAR2(19),
  inhoscall                VARCHAR2(1),
  pathology_observation_fq VARCHAR2(60),
  attending_physician_code VARCHAR2(20),
  follow_up                VARCHAR2(1),
  follow_up_cycle          VARCHAR2(60),
  antibacterial_drugs      VARCHAR2(1),
  durationdate             VARCHAR2(2),
  combined_medication      VARCHAR2(1),
  pathway_flag             VARCHAR2(1),
  pathway_over             VARCHAR2(1),
  path_out_reason          VARCHAR2(60),
  variation_flag           VARCHAR2(1),
  variation_reason         VARCHAR2(60),
  rehospitalization        VARCHAR2(1),
  intervaldate             VARCHAR2(2)
)
tablespace TSP_DSEMR
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 3M
    next 1M
    minextents 1
    maxextents unlimited
  );
-- Add comments to the table 
comment on table IEM_MAINPAGE_BASICINFO_SX
  is '病案首页患者基本信息';
-- Add comments to the columns 
comment on column IEM_MAINPAGE_BASICINFO_SX.iem_mainpage_no
  is '病案首页标识';
comment on column IEM_MAINPAGE_BASICINFO_SX.patnoofhis
  is '病案号';
comment on column IEM_MAINPAGE_BASICINFO_SX.noofinpat
  is '病人首页序号';
comment on column IEM_MAINPAGE_BASICINFO_SX.payid
  is '医疗付款方式ID';
comment on column IEM_MAINPAGE_BASICINFO_SX.socialcare
  is '社保卡号';
comment on column IEM_MAINPAGE_BASICINFO_SX.incount
  is '入院次数';
comment on column IEM_MAINPAGE_BASICINFO_SX.name
  is '患者姓名';
comment on column IEM_MAINPAGE_BASICINFO_SX.sexid
  is '性别';
comment on column IEM_MAINPAGE_BASICINFO_SX.birth
  is '出生';
comment on column IEM_MAINPAGE_BASICINFO_SX.marital
  is '婚姻状况 0.未婚 1.已婚 3.丧偶2.离婚 9.其他';
comment on column IEM_MAINPAGE_BASICINFO_SX.jobid
  is '职业';
comment on column IEM_MAINPAGE_BASICINFO_SX.nationid
  is '民族';
comment on column IEM_MAINPAGE_BASICINFO_SX.nationalityid
  is '国籍ID';
comment on column IEM_MAINPAGE_BASICINFO_SX.idno
  is '身份证号码';
comment on column IEM_MAINPAGE_BASICINFO_SX.organization
  is '工作单位';
comment on column IEM_MAINPAGE_BASICINFO_SX.officeplace
  is '工作单位地址';
comment on column IEM_MAINPAGE_BASICINFO_SX.officetel
  is '工作单位电话';
comment on column IEM_MAINPAGE_BASICINFO_SX.officepost
  is '工作单位邮编';
comment on column IEM_MAINPAGE_BASICINFO_SX.contactperson
  is '联系人姓名';
comment on column IEM_MAINPAGE_BASICINFO_SX.relationship
  is '与联系人关系';
comment on column IEM_MAINPAGE_BASICINFO_SX.contactaddress
  is '联系人地址';
comment on column IEM_MAINPAGE_BASICINFO_SX.contacttel
  is '联系人电话';
comment on column IEM_MAINPAGE_BASICINFO_SX.admitdate
  is '入院时间';
comment on column IEM_MAINPAGE_BASICINFO_SX.admitdept
  is '入院科室';
comment on column IEM_MAINPAGE_BASICINFO_SX.admitward
  is '入院病区';
comment on column IEM_MAINPAGE_BASICINFO_SX.trans_date
  is '转院时间';
comment on column IEM_MAINPAGE_BASICINFO_SX.trans_admitdept
  is '转院科别';
comment on column IEM_MAINPAGE_BASICINFO_SX.trans_admitward
  is '转院病区';
comment on column IEM_MAINPAGE_BASICINFO_SX.outwarddate
  is '出院时间';
comment on column IEM_MAINPAGE_BASICINFO_SX.outhosdept
  is '出院科室';
comment on column IEM_MAINPAGE_BASICINFO_SX.outhosward
  is '出院病区';
comment on column IEM_MAINPAGE_BASICINFO_SX.actualdays
  is '实际住院天数';
comment on column IEM_MAINPAGE_BASICINFO_SX.pathology_diagnosis_name
  is '病理诊断名称';
comment on column IEM_MAINPAGE_BASICINFO_SX.pathology_observation_sn
  is '病理检查号 ';
comment on column IEM_MAINPAGE_BASICINFO_SX.allergic_drug
  is '过敏药物';
comment on column IEM_MAINPAGE_BASICINFO_SX.section_director
  is '科主任';
comment on column IEM_MAINPAGE_BASICINFO_SX.director
  is '主（副主）任医师';
comment on column IEM_MAINPAGE_BASICINFO_SX.vs_employee_code
  is '主治医师';
comment on column IEM_MAINPAGE_BASICINFO_SX.resident_employee_code
  is '住院医师';
comment on column IEM_MAINPAGE_BASICINFO_SX.refresh_employee_code
  is '进修医师';
comment on column IEM_MAINPAGE_BASICINFO_SX.duty_nurse
  is '责任护士';
comment on column IEM_MAINPAGE_BASICINFO_SX.interne
  is '实习医师';
comment on column IEM_MAINPAGE_BASICINFO_SX.coding_user
  is '编码员';
comment on column IEM_MAINPAGE_BASICINFO_SX.medical_quality_id
  is '病案质量';
comment on column IEM_MAINPAGE_BASICINFO_SX.quality_control_doctor
  is '质控医师';
comment on column IEM_MAINPAGE_BASICINFO_SX.quality_control_nurse
  is '质控护士';
comment on column IEM_MAINPAGE_BASICINFO_SX.quality_control_date
  is '质控时间';
comment on column IEM_MAINPAGE_BASICINFO_SX.xay_sn
  is 'x线检查号';
comment on column IEM_MAINPAGE_BASICINFO_SX.ct_sn
  is 'CT检查号';
comment on column IEM_MAINPAGE_BASICINFO_SX.mri_sn
  is 'mri检查号';
comment on column IEM_MAINPAGE_BASICINFO_SX.dsa_sn
  is 'Dsa检查号';
comment on column IEM_MAINPAGE_BASICINFO_SX.bloodtype
  is '血型';
comment on column IEM_MAINPAGE_BASICINFO_SX.rh
  is 'Rh';
comment on column IEM_MAINPAGE_BASICINFO_SX.is_completed
  is '完成否 y/n ';
comment on column IEM_MAINPAGE_BASICINFO_SX.completed_time
  is '完成时间';
comment on column IEM_MAINPAGE_BASICINFO_SX.valide
  is '作废否 1/0';
comment on column IEM_MAINPAGE_BASICINFO_SX.create_user
  is '创建此记录者';
comment on column IEM_MAINPAGE_BASICINFO_SX.create_time
  is '创建时间';
comment on column IEM_MAINPAGE_BASICINFO_SX.modified_user
  is '修改此记录者';
comment on column IEM_MAINPAGE_BASICINFO_SX.modified_time
  is '修改时间';
comment on column IEM_MAINPAGE_BASICINFO_SX.zymosis
  is '医院传染病';
comment on column IEM_MAINPAGE_BASICINFO_SX.hurt_toxicosis_ele_id
  is '损伤、中毒的外部因素';
comment on column IEM_MAINPAGE_BASICINFO_SX.hurt_toxicosis_ele_name
  is '损伤、中毒的外部因素';
comment on column IEM_MAINPAGE_BASICINFO_SX.zymosisstate
  is '医院传染病状态';
comment on column IEM_MAINPAGE_BASICINFO_SX.pathology_diagnosis_id
  is '病理诊断编号';
comment on column IEM_MAINPAGE_BASICINFO_SX.monthage
  is '（年龄不足1周岁的） 年龄(月)';
comment on column IEM_MAINPAGE_BASICINFO_SX.weight
  is '新生儿出生体重(克)';
comment on column IEM_MAINPAGE_BASICINFO_SX.inweight
  is '新生儿入院体重(克)';
comment on column IEM_MAINPAGE_BASICINFO_SX.inhostype
  is '入院途径:1.急诊  2.门诊  3.其他医疗机构转入  9.其他';
comment on column IEM_MAINPAGE_BASICINFO_SX.typehospital
  is '其他医疗机构';
comment on column IEM_MAINPAGE_BASICINFO_SX.outhostype
  is '离院方式 □ 1.医嘱离院  2.医嘱转院 3.医嘱转社区卫生服务机构/乡镇卫生院 4.非医嘱离院5.死亡9.其他';
comment on column IEM_MAINPAGE_BASICINFO_SX.receivehospital
  is '2.医嘱转院，拟接收医疗机构名称：';
comment on column IEM_MAINPAGE_BASICINFO_SX.receivehospital2
  is '3.医嘱转社区卫生服务机构/乡镇卫生院，拟接收医疗机构名称：';
comment on column IEM_MAINPAGE_BASICINFO_SX.againinhospital
  is '是否有出院31天内再住院计划 □ 1.无  2.有';
comment on column IEM_MAINPAGE_BASICINFO_SX.againinhospitalreason
  is '出院31天内再住院计划 目的:                                               ';
comment on column IEM_MAINPAGE_BASICINFO_SX.beforehoscomaday
  is '颅脑损伤患者昏迷时间： 入院前    天';
comment on column IEM_MAINPAGE_BASICINFO_SX.beforehoscomahour
  is '颅脑损伤患者昏迷时间： 入院前     小时';
comment on column IEM_MAINPAGE_BASICINFO_SX.beforehoscomaminute
  is '颅脑损伤患者昏迷时间： 入院前    分钟';
comment on column IEM_MAINPAGE_BASICINFO_SX.laterhoscomaday
  is '颅脑损伤患者昏迷时间： 入院后    天';
comment on column IEM_MAINPAGE_BASICINFO_SX.laterhoscomahour
  is '颅脑损伤患者昏迷时间： 入院后    小时';
comment on column IEM_MAINPAGE_BASICINFO_SX.laterhoscomaminute
  is '颅脑损伤患者昏迷时间： 入院后    分钟';
comment on column IEM_MAINPAGE_BASICINFO_SX.cardnumber
  is '健康卡号';
comment on column IEM_MAINPAGE_BASICINFO_SX.allergic_flag
  is '药物过敏1.无 2.有';
comment on column IEM_MAINPAGE_BASICINFO_SX.autopsy_flag
  is '死亡患者尸检 □ 1.是  2.否';
comment on column IEM_MAINPAGE_BASICINFO_SX.csd_provinceid
  is '出生地 省';
comment on column IEM_MAINPAGE_BASICINFO_SX.csd_cityid
  is '出生地 市';
comment on column IEM_MAINPAGE_BASICINFO_SX.csd_districtid
  is '出生地 县';
comment on column IEM_MAINPAGE_BASICINFO_SX.csd_provincename
  is '出生地 省名称';
comment on column IEM_MAINPAGE_BASICINFO_SX.csd_cityname
  is '出生地 市名称';
comment on column IEM_MAINPAGE_BASICINFO_SX.csd_districtname
  is '出生地 县名称';
comment on column IEM_MAINPAGE_BASICINFO_SX.xzz_provinceid
  is '现住址 省';
comment on column IEM_MAINPAGE_BASICINFO_SX.xzz_cityid
  is '现住址 市';
comment on column IEM_MAINPAGE_BASICINFO_SX.xzz_districtid
  is '现住址 县';
comment on column IEM_MAINPAGE_BASICINFO_SX.xzz_provincename
  is '现住址 省名称';
comment on column IEM_MAINPAGE_BASICINFO_SX.xzz_cityname
  is '现住址 市名称';
comment on column IEM_MAINPAGE_BASICINFO_SX.xzz_districtname
  is '现住址 县名称';
comment on column IEM_MAINPAGE_BASICINFO_SX.xzz_tel
  is '现住址 电话';
comment on column IEM_MAINPAGE_BASICINFO_SX.xzz_post
  is '现住址 邮编';
comment on column IEM_MAINPAGE_BASICINFO_SX.hkdz_provinceid
  is '户口地址 省';
comment on column IEM_MAINPAGE_BASICINFO_SX.hkdz_cityid
  is '户口地址 市';
comment on column IEM_MAINPAGE_BASICINFO_SX.hkdz_districtid
  is '户口地址 县';
comment on column IEM_MAINPAGE_BASICINFO_SX.hkdz_provincename
  is '户口地址 省名称';
comment on column IEM_MAINPAGE_BASICINFO_SX.hkdz_cityname
  is '户口地址 市名称';
comment on column IEM_MAINPAGE_BASICINFO_SX.hkdz_districtname
  is '户口地址 县名称';
comment on column IEM_MAINPAGE_BASICINFO_SX.hkdz_post
  is '户口所在地邮编';
comment on column IEM_MAINPAGE_BASICINFO_SX.jg_provinceid
  is '籍贯 省名称';
comment on column IEM_MAINPAGE_BASICINFO_SX.jg_cityid
  is '籍贯 市名称';
comment on column IEM_MAINPAGE_BASICINFO_SX.jg_provincename
  is '籍贯 省名称';
comment on column IEM_MAINPAGE_BASICINFO_SX.jg_cityname
  is '籍贯 市名称';
comment on column IEM_MAINPAGE_BASICINFO_SX.age
  is '患者年龄';
comment on column IEM_MAINPAGE_BASICINFO_SX.cure_type
  is '治疗类别 □ 1.中医（ 1.1 中医   1.2民族医）    2.中西医     3.西医';
comment on column IEM_MAINPAGE_BASICINFO_SX.mzzyzd_name
  is '门（急）诊诊断（中医诊断）';
comment on column IEM_MAINPAGE_BASICINFO_SX.mzzyzd_code
  is '门（急）诊诊断（中医诊断） 编码';
comment on column IEM_MAINPAGE_BASICINFO_SX.mzxyzd_name
  is '门（急）诊诊断（西医诊断）';
comment on column IEM_MAINPAGE_BASICINFO_SX.mzxyzd_code
  is '门（急）诊诊断（西医诊断） 编码';
comment on column IEM_MAINPAGE_BASICINFO_SX.sslclj
  is '实施临床路径：□ 1. 中医  2. 西医  3 否 ';
comment on column IEM_MAINPAGE_BASICINFO_SX.zyzj
  is '使用医疗机构中药制剂：□ 1.是  2. 否 ';
comment on column IEM_MAINPAGE_BASICINFO_SX.zyzlsb
  is '使用中医诊疗设备：□  1.是 2. 否';
comment on column IEM_MAINPAGE_BASICINFO_SX.zyzljs
  is '使用中医诊疗技术：□ 1. 是  2. 否';
comment on column IEM_MAINPAGE_BASICINFO_SX.bzsh
  is '辨证施护：□ 1.是  2. 否';
comment on column IEM_MAINPAGE_BASICINFO_SX.outhosstatus
  is '病人出院状况';
comment on column IEM_MAINPAGE_BASICINFO_SX.jbynzz
  is '疾病是否疑难杂症';
comment on column IEM_MAINPAGE_BASICINFO_SX.mzycy
  is '门诊与出院';
comment on column IEM_MAINPAGE_BASICINFO_SX.inandouthos
  is '入院与出院';
comment on column IEM_MAINPAGE_BASICINFO_SX.lcybl
  is '临床与病理';
comment on column IEM_MAINPAGE_BASICINFO_SX.fsybl
  is '放射与病理';
comment on column IEM_MAINPAGE_BASICINFO_SX.qjcount
  is '抢救次数放射与病理';
comment on column IEM_MAINPAGE_BASICINFO_SX.successcount
  is '成功次数';
comment on column IEM_MAINPAGE_BASICINFO_SX.inpatly
  is '病人来源';
comment on column IEM_MAINPAGE_BASICINFO_SX.asascore
  is 'ASA分级评分';
comment on column IEM_MAINPAGE_BASICINFO_SX.ryxyzd_code
  is '入院诊断编码';
comment on column IEM_MAINPAGE_BASICINFO_SX.ryxyzd_name
  is '入院诊断';
comment on column IEM_MAINPAGE_BASICINFO_SX.inhosinfo
  is '入院情况 □ 1. 危 2. 急 3. 一般';
comment on column IEM_MAINPAGE_BASICINFO_SX.maindiagdate
  is '主要诊断确诊日期';
comment on column IEM_MAINPAGE_BASICINFO_SX.inhoscall
  is '住院期间是否告病危或病重 □ 1. 是 2. 否';
comment on column IEM_MAINPAGE_BASICINFO_SX.pathology_observation_fq
  is '病理分期：';
comment on column IEM_MAINPAGE_BASICINFO_SX.attending_physician_code
  is '主诊医师';
comment on column IEM_MAINPAGE_BASICINFO_SX.follow_up
  is '随诊 □ 1. 是 2. 否';
comment on column IEM_MAINPAGE_BASICINFO_SX.follow_up_cycle
  is '随诊期限';
comment on column IEM_MAINPAGE_BASICINFO_SX.antibacterial_drugs
  is 'Ⅰ类手术切口预防性应用抗菌药物';
comment on column IEM_MAINPAGE_BASICINFO_SX.durationdate
  is '使用持续时间';
comment on column IEM_MAINPAGE_BASICINFO_SX.combined_medication
  is '联合用药';
comment on column IEM_MAINPAGE_BASICINFO_SX.pathway_flag
  is '是否实施临床路径管理';
comment on column IEM_MAINPAGE_BASICINFO_SX.pathway_over
  is '是否完成临床路径';
comment on column IEM_MAINPAGE_BASICINFO_SX.path_out_reason
  is '退出原因';
comment on column IEM_MAINPAGE_BASICINFO_SX.variation_flag
  is '是否变异';
comment on column IEM_MAINPAGE_BASICINFO_SX.variation_reason
  is '变异原因：';
comment on column IEM_MAINPAGE_BASICINFO_SX.rehospitalization
  is '是否因同一病种再入院';
comment on column IEM_MAINPAGE_BASICINFO_SX.intervaldate
  is '与上次出院日期间隔天数';
