CREATE OR REPLACE PACKAGE iem_main_page IS
  TYPE empcurtyp IS REF CURSOR;

  /*插入病案首页信息*/
  PROCEDURE usp_insertiembasicinfo(v_patnoofhis               varchar,
                                   v_noofinpat                varchar,
                                   v_payid                    VARCHAR,
                                   v_socialcare               VARCHAR,
                                   v_incount                  VARCHAR,
                                   v_name                     VARCHAR,
                                   v_sexid                    VARCHAR,
                                   v_birth                    VARCHAR,
                                   v_marital                  VARCHAR,
                                   v_jobid                    VARCHAR,
                                   v_provinceid               VARCHAR,
                                   v_countyid                 VARCHAR,
                                   v_nationid                 VARCHAR,
                                   v_nationalityid            VARCHAR,
                                   v_idno                     VARCHAR,
                                   v_organization             VARCHAR,
                                   v_officeplace              VARCHAR,
                                   v_officetel                VARCHAR,
                                   v_officepost               VARCHAR,
                                   v_nativeaddress            VARCHAR,
                                   v_nativetel                VARCHAR,
                                   v_nativepost               VARCHAR,
                                   v_contactperson            VARCHAR,
                                   v_relationship             VARCHAR,
                                   v_contactaddress           VARCHAR,
                                   v_contacttel               VARCHAR,
                                   v_admitdate                VARCHAR,
                                   v_admitdept                VARCHAR,
                                   v_admitward                VARCHAR,
                                   v_days_before              VARCHAR,
                                   v_trans_date               VARCHAR,
                                   v_trans_admitdept          VARCHAR,
                                   v_trans_admitward          VARCHAR,
                                   v_trans_admitdept_again    VARCHAR,
                                   v_outwarddate              VARCHAR,
                                   v_outhosdept               VARCHAR,
                                   v_outhosward               VARCHAR,
                                   v_actual_days              VARCHAR,
                                   v_death_time               VARCHAR,
                                   v_death_reason             VARCHAR,
                                   v_admitinfo                VARCHAR,
                                   v_in_check_date            VARCHAR,
                                   v_pathology_diagnosis_name VARCHAR,
                                   v_pathology_observation_sn VARCHAR,
                                   v_ashes_diagnosis_name     VARCHAR,
                                   v_ashes_anatomise_sn       VARCHAR,
                                   v_allergic_drug            VARCHAR,
                                   v_hbsag                    VARCHAR,
                                   v_hcv_ab                   VARCHAR,
                                   v_hiv_ab                   VARCHAR,
                                   v_opd_ipd_id               VARCHAR,
                                   v_in_out_inpatinet_id      VARCHAR,
                                   v_before_after_or_id       VARCHAR,
                                   v_clinical_pathology_id    VARCHAR,
                                   v_pacs_pathology_id        VARCHAR,
                                   v_save_times               VARCHAR,
                                   v_success_times            VARCHAR,
                                   v_section_director         VARCHAR,
                                   v_director                 VARCHAR,
                                   v_vs_employee_code         VARCHAR,
                                   v_resident_employee_code   VARCHAR,
                                   v_refresh_employee_code    VARCHAR,
                                   v_master_interne           VARCHAR,
                                   v_interne                  VARCHAR,
                                   v_coding_user              VARCHAR,
                                   v_medical_quality_id       VARCHAR,
                                   v_quality_control_doctor   VARCHAR,
                                   v_quality_control_nurse    VARCHAR,
                                   v_quality_control_date     VARCHAR,
                                   v_xay_sn                   VARCHAR,
                                   v_ct_sn                    VARCHAR,
                                   v_mri_sn                   VARCHAR,
                                   v_dsa_sn                   VARCHAR,
                                   v_is_first_case            VARCHAR,
                                   v_is_following             VARCHAR,
                                   v_following_ending_date    VARCHAR,
                                   v_is_teaching_case         VARCHAR,
                                   v_blood_type_id            VARCHAR,
                                   v_rh                       VARCHAR,
                                   v_blood_reaction_id        VARCHAR,
                                   v_blood_rbc                VARCHAR,
                                   v_blood_plt                VARCHAR,
                                   v_blood_plasma             VARCHAR,
                                   v_blood_wb                 VARCHAR,
                                   v_blood_others             VARCHAR,
                                   v_is_completed             VARCHAR,
                                   v_completed_time           VARCHAR,
                                   v_create_user              VARCHAR,
                                   v_Zymosis                  varchar,
                                   v_Hurt_Toxicosis_Ele       varchar,
                                   v_ZymosisState             varchar,
                                   o_result                   OUT empcurtyp);

  /*修改病案首页信息*/
  PROCEDURE usp_Upateiembasicinfo(v_iem_mainpage_no varchar,
                                  v_patnoofhis      varchar,
                                  v_noofinpat       integer,
                                  v_payid           VARCHAR,
                                  v_socialcare      VARCHAR,
                                  
                                  v_incount VARCHAR,
                                  v_name    VARCHAR,
                                  v_sexid   VARCHAR,
                                  v_birth   VARCHAR,
                                  v_marital VARCHAR,
                                  
                                  v_jobid         VARCHAR,
                                  v_provinceid    VARCHAR,
                                  v_countyid      VARCHAR,
                                  v_nationid      VARCHAR,
                                  v_nationalityid VARCHAR,
                                  
                                  v_idno         VARCHAR,
                                  v_organization VARCHAR,
                                  v_officeplace  VARCHAR,
                                  v_officetel    VARCHAR,
                                  v_officepost   VARCHAR,
                                  
                                  v_nativeaddress VARCHAR,
                                  v_nativetel     VARCHAR,
                                  v_nativepost    VARCHAR,
                                  v_contactperson VARCHAR,
                                  v_relationship  VARCHAR,
                                  
                                  v_contactaddress VARCHAR,
                                  v_contacttel     VARCHAR,
                                  v_admitdate      VARCHAR,
                                  v_admitdept      VARCHAR,
                                  v_admitward      VARCHAR,
                                  
                                  v_days_before           VARCHAR,
                                  v_trans_date            VARCHAR,
                                  v_trans_admitdept       VARCHAR,
                                  v_trans_admitward       VARCHAR,
                                  v_trans_admitdept_again VARCHAR,
                                  
                                  v_outwarddate VARCHAR,
                                  v_outhosdept  VARCHAR,
                                  v_outhosward  VARCHAR,
                                  v_actual_days VARCHAR,
                                  v_death_time  VARCHAR,
                                  
                                  v_death_reason VARCHAR,
                                  
                                  v_admitinfo                VARCHAR,
                                  v_in_check_date            VARCHAR,
                                  v_pathology_diagnosis_name VARCHAR,
                                  v_pathology_observation_sn VARCHAR,
                                  v_ashes_diagnosis_name     VARCHAR,
                                  v_ashes_anatomise_sn       VARCHAR,
                                  v_allergic_drug            VARCHAR,
                                  v_hbsag                    VARCHAR,
                                  v_hcv_ab                   VARCHAR,
                                  v_hiv_ab                   VARCHAR,
                                  v_opd_ipd_id               VARCHAR,
                                  v_in_out_inpatinet_id      VARCHAR,
                                  v_before_after_or_id       VARCHAR,
                                  v_clinical_pathology_id    VARCHAR,
                                  v_pacs_pathology_id        VARCHAR,
                                  v_save_times               VARCHAR,
                                  v_success_times            VARCHAR,
                                  v_section_director         VARCHAR,
                                  v_director                 VARCHAR,
                                  v_vs_employee_code         VARCHAR,
                                  v_resident_employee_code   VARCHAR,
                                  v_refresh_employee_code    VARCHAR,
                                  v_master_interne           VARCHAR,
                                  v_interne                  VARCHAR,
                                  v_coding_user              VARCHAR,
                                  v_medical_quality_id       VARCHAR,
                                  v_quality_control_doctor   VARCHAR,
                                  v_quality_control_nurse    VARCHAR,
                                  v_quality_control_date     VARCHAR,
                                  v_xay_sn                   VARCHAR,
                                  v_ct_sn                    VARCHAR,
                                  v_mri_sn                   VARCHAR,
                                  v_dsa_sn                   VARCHAR,
                                  v_is_first_case            VARCHAR,
                                  v_is_following             VARCHAR,
                                  v_following_ending_date    VARCHAR,
                                  v_is_teaching_case         VARCHAR,
                                  v_blood_type_id            VARCHAR,
                                  v_rh                       VARCHAR,
                                  v_blood_reaction_id        VARCHAR,
                                  v_blood_rbc                VARCHAR,
                                  v_blood_plt                VARCHAR,
                                  v_blood_plasma             VARCHAR,
                                  v_blood_wb                 VARCHAR,
                                  
                                  v_blood_others   VARCHAR,
                                  v_is_completed   VARCHAR,
                                  v_completed_time VARCHAR,
                                  
                                  v_Zymosis            varchar,
                                  v_Hurt_Toxicosis_Ele varchar,
                                  v_ZymosisState       varchar,
                                  o_result             OUT empcurtyp);

  /*
  * 插入功病案首页诊断TABLE
  */
  PROCEDURE usp_insert_iem_mainpage_diag(v_iem_mainpage_no   VARCHAR,
                                         v_diagnosis_type_id VARCHAR,
                                         v_diagnosis_code    VARCHAR,
                                         v_diagnosis_name    VARCHAR,
                                         v_status_id         VARCHAR,
                                         v_order_value       VARCHAR,
                                         --v_Valide numeric ,
                                         v_create_user VARCHAR
                                         --v_Create_Time varchar(19) ,
                                         --v_Cancel_User varchar(10) ,
                                         --v_Cancel_Time varchar(19)
                                         );

  /*
  * 插入功病案首页手术TABLE
  */
  PROCEDURE usp_insert_iem_mainpage_oper(v_iem_mainpage_no     NUMERIC,
                                         v_operation_code      VARCHAR,
                                         v_operation_date      VARCHAR,
                                         v_operation_name      VARCHAR,
                                         v_execute_user1       VARCHAR,
                                         v_execute_user2       VARCHAR,
                                         v_execute_user3       VARCHAR,
                                         v_anaesthesia_type_id NUMERIC,
                                         v_close_level         NUMERIC,
                                         v_anaesthesia_user    VARCHAR,
                                         --v_Valide numeric ,
                                         v_create_user VARCHAR
                                         --v_Create_Time varchar(19)
                                         --v_Cancel_User varchar(10) ,
                                         --v_Cancel_Time varchar(19)
                                         );
  /*
  * 插入功病案首页 产妇婴儿信息
  */
  PROCEDURE usp_insert_iem_main_ObsBaby(v_iem_mainpage_no NUMERIC,
                                        v_IBSBABYID       VARCHAR, --编号
                                        v_TC              VARCHAR, --胎次
                                        v_CC              VARCHAR, -- 产次
                                        v_TB              VARCHAR, -- 胎别
                                        v_CFHYPLD         VARCHAR, --产妇会阴破裂度
                                        v_MIDWIFERY       VARCHAR, --接产者
                                        v_SEX             VARCHAR, --性别
                                        v_APJ             VARCHAR, -- 阿帕加评加
                                        v_HEIGH           VARCHAR, --身长
                                        v_WEIGHT          VARCHAR, --体重
                                        v_CCQK            VARCHAR, --产出情况
                                        v_BITHDAY         VARCHAR, --出生时间
                                        v_FMFS            VARCHAR, --     分娩方式
                                        v_CYQK            VARCHAR,
                                        v_create_user     VARCHAR);

  /*
  * 插入一条病人信息
  */
  PROCEDURE usp_insertpatientinfo(v_noofinpat       varchar,
                                  v_patnoofhis      VARCHAR,
                                  v_Noofclinic      VARCHAR,
                                  v_Noofrecord      VARCHAR,
                                  v_patid           VARCHAR,
                                  v_Innerpix        VARCHAR,
                                  v_outpix          VARCHAR,
                                  v_Name            VARCHAR,
                                  v_py              VARCHAR,
                                  v_wb              VARCHAR,
                                  v_payid           VARCHAR,
                                  v_ORIGIN          VARCHAR,
                                  v_InCount         int DEFAULT 0,
                                  v_sexid           VARCHAR,
                                  v_Birth           VARCHAR,
                                  v_Age             int DEFAULT 0,
                                  v_AgeStr          VARCHAR,
                                  v_IDNO            VARCHAR,
                                  v_Marital         VARCHAR,
                                  v_JobID           VARCHAR,
                                  v_CSDProvinceID   VARCHAR,
                                  v_CSDCityID       VARCHAR,
                                  v_CSDDistrictID   VARCHAR,
                                  v_NationID        VARCHAR,
                                  v_NationalityID   VARCHAR,
                                  v_JGProvinceID    VARCHAR,
                                  v_JGCityID        VARCHAR,
                                  v_Organization    VARCHAR,
                                  v_OfficePlace     VARCHAR,
                                  v_OfficeTEL       VARCHAR,
                                  v_OfficePost      VARCHAR,
                                  v_HKDZProvinceID  VARCHAR,
                                  v_HKDZCityID      VARCHAR,
                                  v_HKDZDistrictID  VARCHAR,
                                  v_NATIVEPOST      VARCHAR,
                                  v_NATIVETEL       VARCHAR,
                                  v_NATIVEADDRESS   VARCHAR,
                                  v_ADDRESS         VARCHAR,
                                  v_ContactPerson   VARCHAR,
                                  v_RelationshipID  VARCHAR,
                                  v_ContactAddress  VARCHAR,
                                  v_ContactTEL      VARCHAR,
                                  v_CONTACTOFFICE   VARCHAR,
                                  v_CONTACTPOST     VARCHAR,
                                  v_OFFERER         VARCHAR,
                                  v_SocialCare      VARCHAR,
                                  v_INSURANCE       VARCHAR,
                                  v_CARDNO          VARCHAR,
                                  v_ADMITINFO       VARCHAR,
                                  v_AdmitDeptID     VARCHAR,
                                  v_AdmitWardID     VARCHAR,
                                  v_ADMITBED        VARCHAR,
                                  v_AdmitDate       VARCHAR,
                                  v_INWARDDATE      VARCHAR,
                                  v_ADMITDIAGNOSIS  VARCHAR,
                                  v_OutWardDate     VARCHAR,
                                  v_OutHosDeptID    VARCHAR,
                                  v_OutHosWardID    VARCHAR,
                                  v_OutBed          VARCHAR,
                                  v_OUTHOSDATE      VARCHAR,
                                  v_OUTDIAGNOSIS    VARCHAR,
                                  v_TOTALDAYS       int DEFAULT 0,
                                  v_CLINICDIAGNOSIS VARCHAR,
                                  v_SOLARTERMS      VARCHAR,
                                  v_ADMITWAY        VARCHAR,
                                  v_OUTWAY          VARCHAR,
                                  v_CLINICDOCTOR    VARCHAR,
                                  v_RESIDENT        VARCHAR,
                                  v_ATTEND          VARCHAR,
                                  v_CHIEF           VARCHAR,
                                  v_EDU             VARCHAR,
                                  v_EDUC            int DEFAULT 0,
                                  v_RELIGION        VARCHAR,
                                  v_STATUS          int DEFAULT 0,
                                  v_CRITICALLEVEL   VARCHAR,
                                  v_ATTENDLEVEL     VARCHAR,
                                  v_EMPHASIS        int DEFAULT 0,
                                  v_ISBABY          int DEFAULT 0,
                                  v_MOTHER          NUMERIC,
                                  v_MEDICAREID      VARCHAR,
                                  v_MEDICAREQUOTA   int DEFAULT 0,
                                  v_VOUCHERSCODE    VARCHAR,
                                  v_STYLE           VARCHAR,
                                  v_OPERATOR        VARCHAR,
                                  v_MEMO            VARCHAR,
                                  v_CPSTATUS        int DEFAULT 0,
                                  v_OUTWARDBED      int DEFAULT 0,
                                  v_XZZProvinceID   VARCHAR,
                                  v_XZZCityID       VARCHAR,
                                  v_XZZDistrictID   VARCHAR,
                                  v_XZZTEL          VARCHAR,
                                  v_XZZPost         VARCHAR);

  /*
  * 捞出所有的数据供报表设计器打印
  */
  PROCEDURE usp_get_iem_mainpage_all(
                                     --      v_noofinpat             VARCHAR,
                                     o_result  OUT empcurtyp,
                                     o_result1 OUT empcurtyp,
                                     o_result2 OUT empcurtyp);

  /*
  * 获取病案首页信息
  */
  PROCEDURE usp_GetIemInfo_new(v_NoOfInpat int,
                               o_result    OUT empcurtyp,
                               o_result1   OUT empcurtyp,
                               o_result2   OUT empcurtyp,
                               o_result3   OUT empcurtyp);

  /*
  * 维护病案首页信息
  */
  PROCEDURE usp_Edit_Iem_BasicInfo_2012(v_edittype        varchar2 default '',
                                        v_IEM_MAINPAGE_NO varchar2 default '', ---- '病案首页标识';
                                        v_PATNOOFHIS      varchar2 default '', ---- '病案号';
                                        v_NOOFINPAT       varchar2 default '', ---- '病人首页序号';
                                        v_PAYID           varchar2 default '', ---- '医疗付款方式ID';
                                        v_SOCIALCARE      varchar2 default '', ---- '社保卡号';
                                        
                                        v_INCOUNT varchar2 default '', ---- '入院次数';
                                        v_NAME    varchar2 default '', ---- '患者姓名';
                                        v_SEXID   varchar2 default '', ---- '性别';
                                        v_BIRTH   varchar2 default '', ---- '出生';
                                        v_MARITAL varchar2 default '', ---- '婚姻状况 1.未婚 2.已婚 3.丧偶4.离婚 9.其他';
                                        
                                        v_JOBID         varchar2 default '', ---- '职业';
                                        v_NATIONALITYID varchar2 default '', ---- '国籍ID';
                                        v_NATIONID      varchar2 default '', --民族
                                        v_IDNO          varchar2 default '', ---- '身份证号码';
                                        v_ORGANIZATION  varchar2 default '', ---- '工作单位';
                                        v_OFFICEPLACE   varchar2 default '', ---- '工作单位地址';
                                        
                                        v_OFFICETEL      varchar2 default '', ---- '工作单位电话';
                                        v_OFFICEPOST     varchar2 default '', ---- '工作单位邮编';
                                        v_CONTACTPERSON  varchar2 default '', ---- '联系人姓名';
                                        v_RELATIONSHIP   varchar2 default '', ---- '与联系人关系';
                                        v_CONTACTADDRESS varchar2 default '', ---- '联系人地址';
                                        
                                        v_CONTACTTEL varchar2 default '', ---- '联系人电话';
                                        v_ADMITDATE  varchar2 default '', ---- '入院时间';
                                        v_ADMITDEPT  varchar2 default '', ---- '入院科室';
                                        v_ADMITWARD  varchar2 default '', ---- '入院病区';
                                        v_TRANS_DATE varchar2 default '', ---- '转院时间';
                                        
                                        v_TRANS_ADMITDEPT varchar2 default '', ---- '转院科别';
                                        v_TRANS_ADMITWARD varchar2 default '', ---- '转院病区';
                                        v_OUTWARDDATE     varchar2 default '', ---- '出院时间';
                                        v_OUTHOSDEPT      varchar2 default '', ---- '出院科室';
                                        v_OUTHOSWARD      varchar2 default '', ---- '出院病区';
                                        
                                        v_ACTUALDAYS               varchar2 default '', ---- '实际住院天数';
                                        v_PATHOLOGY_DIAGNOSIS_NAME varchar2 default '', ---- '病理诊断名称';
                                        v_PATHOLOGY_OBSERVATION_SN varchar2 default '', ---- '病理检查号 ';
                                        v_ALLERGIC_DRUG            varchar2 default '', ---- '过敏药物';
                                        v_SECTION_DIRECTOR         varchar2 default '', ---- '科主任';
                                        
                                        v_DIRECTOR               varchar2 default '', ---- '主（副主）任医师';
                                        v_VS_EMPLOYEE_CODE       varchar2 default '', ---- '主治医师';
                                        v_RESIDENT_EMPLOYEE_CODE varchar2 default '', ---- '住院医师';
                                        v_REFRESH_EMPLOYEE_CODE  varchar2 default '', ---- '进修医师';
                                        v_DUTY_NURSE             varchar2 default '', ---- '责任护士';
                                        
                                        v_INTERNE                varchar2 default '', ---- '实习医师';
                                        v_CODING_USER            varchar2 default '', ---- '编码员';
                                        v_MEDICAL_QUALITY_ID     varchar2 default '', ---- '病案质量';
                                        v_QUALITY_CONTROL_DOCTOR varchar2 default '', ---- '质控医师';
                                        v_QUALITY_CONTROL_NURSE  varchar2 default '', ---- '质控护士';
                                        
                                        v_QUALITY_CONTROL_DATE varchar2 default '', ---- '质控时间';
                                        v_XAY_SN               varchar2 default '', ---- 'x线检查号';
                                        v_CT_SN                varchar2 default '', ---- 'CT检查号';
                                        v_MRI_SN               varchar2 default '', ---- 'mri检查号';
                                        v_DSA_SN               varchar2 default '', ---- 'Dsa检查号';
                                        
                                        v_BLOODTYPE      varchar2 default '', ---- '血型';
                                        v_RH             varchar2 default '', ---- 'Rh';
                                        v_IS_COMPLETED   varchar2 default '', ---- '完成否 y/n ';
                                        v_COMPLETED_TIME varchar2 default '', ---- '完成时间';
                                        v_VALIDE         varchar2 default '1', ---- '作废否 1/0';
                                        
                                        v_CREATE_USER   varchar2 default '', ---- '创建此记录者';
                                        v_CREATE_TIME   varchar2 default '', ---- '创建时间';
                                        v_MODIFIED_USER varchar2 default '', ---- '修改此记录者';
                                        v_MODIFIED_TIME varchar2 default '', ---- '修改时间';
                                        v_ZYMOSIS       varchar2 default '', ---- '医院传染病';
                                        
                                        v_HURT_TOXICOSIS_ELE_ID   varchar2 default '', ---- '损伤、中毒的外部因素';
                                        v_HURT_TOXICOSIS_ELE_Name varchar2 default '', ---- '损伤、中毒的外部因素';
                                        v_ZYMOSISSTATE            varchar2 default '', ---- '医院传染病状态';
                                        v_PATHOLOGY_DIAGNOSIS_ID  varchar2 default '', ---- '病理诊断编号';
                                        v_MONTHAGE                varchar2 default '', ---- '（年龄不足1周岁的） 年龄(月)';
                                        v_WEIGHT                  varchar2 default '', ---- '新生儿出生体重(克)';
                                        
                                        v_INWEIGHT         varchar2 default '', ---- '新生儿入院体重(克)';
                                        v_INHOSTYPE        varchar2 default '', ---- '入院途径:1.急诊  2.门诊  3.其他医疗机构转入  9.其他';
                                        v_OUTHOSTYPE       varchar2 default '', ---- '离院方式 □ 1.医嘱离院  2.医嘱转院 3.医嘱转社区卫生服务机构/乡镇卫生院 4.非医嘱离院5.死亡9.其他';
                                        v_RECEIVEHOSPITAL  varchar2 default '', ---- '2.医嘱转院，拟接收医疗机构名称：';
                                        v_RECEIVEHOSPITAL2 varchar2 default '', ---- '3.医嘱转社区卫生服务机构/乡镇卫生院，拟接收医疗机构名;
                                        
                                        v_AGAININHOSPITAL       varchar2 default '', ---- '是否有出院31天内再住院计划 □ 1.无  2.有';
                                        v_AGAININHOSPITALREASON varchar2 default '', ---- '出院31天内再住院计划 目的:            ';
                                        v_BEFOREHOSCOMADAY      varchar2 default '', ---- '颅脑损伤患者昏迷时间： 入院前    天';
                                        v_BEFOREHOSCOMAHOUR     varchar2 default '', ---- '颅脑损伤患者昏迷时间： 入院前     小时';
                                        v_BEFOREHOSCOMAMINUTE   varchar2 default '', ---- '颅脑损伤患者昏迷时间： 入院前    分钟';
                                        
                                        v_LATERHOSCOMADAY    varchar2 default '', ---- '颅脑损伤患者昏迷时间： 入院后    天';
                                        v_LATERHOSCOMAHOUR   varchar2 default '', ---- '颅脑损伤患者昏迷时间： 入院后    小时';
                                        v_LATERHOSCOMAMINUTE varchar2 default '', ---- '颅脑损伤患者昏迷时间： 入院后    分钟';
                                        v_CARDNUMBER         varchar2 default '', ---- '健康卡号';
                                        v_ALLERGIC_FLAG      varchar2 default '', ---- '药物过敏1.无 2.有';
                                        
                                        v_AUTOPSY_FLAG     varchar2 default '', ---- '死亡患者尸检 □ 1.是  2.否';
                                        v_CSD_PROVINCEID   varchar2 default '', ---- '出生地 省';
                                        v_CSD_CITYID       varchar2 default '', ---- '出生地 市';
                                        v_CSD_DISTRICTID   varchar2 default '', ---- '出生地 县';
                                        v_CSD_PROVINCENAME varchar2 default '', ---- '出生地 省名称';
                                        
                                        v_CSD_CITYNAME     varchar2 default '', ---- '出生地 市名称';
                                        v_CSD_DISTRICTNAME varchar2 default '', ---- '出生地 县名称';
                                        v_XZZ_PROVINCEID   varchar2 default '', ---- '现住址 省';
                                        v_XZZ_CITYID       varchar2 default '', ---- '现住址 市';
                                        v_XZZ_DISTRICTID   varchar2 default '', ---- '现住址 县';
                                        
                                        v_XZZ_PROVINCENAME varchar2 default '', ---- '现住址 省名称';
                                        v_XZZ_CITYNAME     varchar2 default '', ---- '现住址 市名称';
                                        v_XZZ_DISTRICTNAME varchar2 default '', ---- '现住址 县名称';
                                        v_XZZ_TEL          varchar2 default '', ---- '现住址 电话';
                                        v_XZZ_POST         varchar2 default '', ---- '现住址 邮编';
                                        
                                        v_HKDZ_PROVINCEID   varchar2 default '', ---- '户口地址 省';
                                        v_HKDZ_CITYID       varchar2 default '', ---- '户口地址 市';
                                        v_HKDZ_DISTRICTID   varchar2 default '', ---- '户口地址 县';
                                        v_HKDZ_PROVINCENAME varchar2 default '', ---- '户口地址 省名称';
                                        v_HKDZ_CITYNAME     varchar2 default '', ---- '户口地址 市名称';
                                        
                                        v_HKDZ_DISTRICTNAME     varchar2 default '', ---- '户口地址 县名称';
                                        v_HKDZ_POST             varchar2 default '', ---- '户口所在地邮编';
                                        v_JG_PROVINCEID         varchar2 default '', ---- '籍贯 省名称';
                                        v_JG_CITYID             varchar2 default '', ---- '籍贯 市名称';
                                        v_JG_PROVINCENAME       varchar2 default '', ---- '籍贯 省名称';
                                        v_JG_CITYNAME           varchar2 default '', ---- '籍贯 市名称'
                                        v_Age                   varchar2 default '',
                                        v_zg_flag               varchar2 default '', -----转归：□ 1.治愈 2.好转 3.未愈 4.死亡 5.其他
                                        v_admitinfo             varchar2 default '', --  v入院病情□ 1.危 2.重 3.一般 4.急 add 二〇一二年六月二十六日 10:14:09
                                        v_CSDADDRESS            varchar2 default '', --出生地具体地址 add by ywk 2012年7月11日 10:13:49
                                        v_JGADDRESS             varchar2 default '', --籍贯地址具体地址 add by ywk 2012年7月11日 10:13:49
                                        v_XZZADDRESS            varchar2 default '', --现住址具体地址 add by ywk 2012年7月11日 10:13:49
                                        v_HKDZADDRESS           varchar2 default '', --户口地址具体地址 add by ywk 2012年7月11日 10:13:49
                                        v_MenAndInHop           varchar, --门诊和住院
                                        v_InHopAndOutHop        varchar, --入院和出院
                                        v_BeforeOpeAndAfterOper varchar, --术前和术后
                                        v_LinAndBingLi          varchar, --临床与病理
                                        v_InHopThree            varchar, --入院三日内
                                        v_FangAndBingLi         varchar, --放射和病理
                                        o_result                OUT empcurtyp);

  /*
  *查询病案首页信息
  **********/
  PROCEDURE usp_getieminfo_2012(v_noofinpat INT,
                                o_result    OUT empcurtyp,
                                o_result1   OUT empcurtyp,
                                o_result2   OUT empcurtyp,
                                o_result3   OUT empcurtyp,
                                o_result4   OUT empcurtyp);

  PROCEDURE usp_edit_iem_mainpage_oper2012(v_iem_mainpage_no     NUMERIC,
                                           v_operation_code      VARCHAR,
                                           v_operation_date      VARCHAR,
                                           v_operation_name      VARCHAR,
                                           v_execute_user1       VARCHAR,
                                           v_execute_user2       VARCHAR,
                                           v_execute_user3       VARCHAR,
                                           v_anaesthesia_type_id NUMERIC,
                                           v_close_level         NUMERIC,
                                           v_anaesthesia_user    VARCHAR,
                                           --v_Valide numeric ,
                                           v_create_user     VARCHAR,
                                           v_OPERATION_LEVEL varchar,
                                           v_IsChooseDate    varchar, --手术相关字段
                                           v_IsClearOpe      varchar,
                                           v_IsGanRan        varchar,
                                           v_anesthesia_level varchar,
                                           v_opercomplication_code varchar
                                           
                                           --v_Create_Time varchar(19)
                                           --v_Cancel_User varchar(10) ,
                                           --v_Cancel_Time varchar(19)
                                           );

 PROCEDURE usp_edif_iem_mainpage_diag2012(v_iem_mainpage_no   VARCHAR,
                                           v_diagnosis_type_id VARCHAR,
                                           v_diagnosis_code    VARCHAR,
                                           v_diagnosis_name    VARCHAR,
                                           v_status_id         VARCHAR,
                                           v_order_value       VARCHAR,
                                           --v_Valide numeric ,
                                           v_create_user           VARCHAR,
                                           v_MenAndInHop           varchar, --门诊和住院
                                           v_InHopAndOutHop        varchar, --入院和出院
                                           v_BeforeOpeAndAfterOper varchar, --术前和术后
                                           v_LinAndBingLi          varchar, --临床与病理
                                           v_InHopThree            varchar, --入院三日内
                                           v_FangAndBingLi         varchar, --放射和病理
                                           v_AdmitInfo             varchar --子入院病情
                                           --v_Create_Time varchar(19) ,
                                           --v_Cancel_User varchar(10) ,
                                           --v_Cancel_Time varchar(19)
                                           );
                                           
 ------------------------湖北首页专用存储过程 add jxh-----------------------------------------------------------------------------------------------
  PROCEDURE usp_edif_iem_mainpage_diag_hb(v_iem_mainpage_no   VARCHAR,
                                           v_diagnosis_type_id VARCHAR,
                                           v_diagnosis_code    VARCHAR,
                                           v_diagnosis_name    VARCHAR,
                                           v_status_id         VARCHAR,
                                           v_order_value       VARCHAR,
                                           v_morphologyicd     VARCHAR,--形态学诊断编码
                                           v_morphologyname    VARCHAR,--形态学诊断名称                                 
                                           --v_Valide numeric ,
                                           v_create_user           VARCHAR,
                                           v_MenAndInHop           varchar, --门诊和住院
                                           v_InHopAndOutHop        varchar, --入院和出院
                                           v_BeforeOpeAndAfterOper varchar, --术前和术后
                                           v_LinAndBingLi          varchar, --临床与病理
                                           v_InHopThree            varchar, --入院三日内
                                           v_FangAndBingLi         varchar, --放射和病理
                                           v_AdmitInfo             varchar --子入院病情 
                                           --v_Create_Time varchar(19) ,
                                           --v_Cancel_User varchar(10) ,
                                           --v_Cancel_Time varchar(19)
                                           );                                         

  --更新病案首页信息后，对病人信息表进行数据同步 add by ywk 二〇一二年五月四日 15:20:27
  PROCEDURE usp_Edit_Iem_PaientInfo(v_NOOFINPAT      varchar2 default '', ---- '病人首页序号';
                                    v_NAME           varchar2 default '', ---- '患者姓名';
                                    v_SEXID          varchar2 default '', ---- '性别';
                                    v_BIRTH          varchar2 default '', ---- '出生';
                                    v_Age            INTEGER default '', --年龄
                                    v_IDNO           varchar2 default '', ---- '身份证号码';
                                    v_MARITAL        varchar2 default '', ---- '婚姻状况 1.未婚 2.已婚 3.丧偶4.离婚 9.其他';
                                    v_JOBID          varchar2 default '', ---- '职业';
                                    v_CSD_PROVINCEID varchar2 default '', ---- '出生地 省';
                                    v_CSD_CITYID     varchar2 default '', ---- '出生地 市';
                                    v_NATIONID       varchar2 default '', --民族
                                    v_NATIONALITYID  varchar2 default '', ---- '国籍ID';
                                    v_JG_PROVINCEID  varchar2 default '', ---- '籍贯 省';
                                    v_JG_CITYID      varchar2 default '', ---- '籍贯 市';
                                    v_OFFICEPLACE    varchar2 default '', ---- '工作单位地址';
                                    v_OFFICETEL      varchar2 default '', ---- '工作单位电话';
                                    v_OFFICEPOST     varchar2 default '', ---- '工作单位邮编';
                                    v_HKDZ_POST      varchar2 default '', ---- '户口所在地邮编';
                                    v_CONTACTPERSON  varchar2 default '', ---- '联系人姓名';
                                    v_RELATIONSHIP   varchar2 default '', ---- '与联系人关系';
                                    v_CONTACTADDRESS varchar2 default '', ---- '联系人地址';
                                    v_CONTACTTEL     varchar2 default '', ---- '联系人电话';
                                    v_ADMITDEPT      varchar2 default '', ---- '入院科室';
                                    v_ADMITWARD      varchar2 default '', ---- '入院病区';
                                    v_ADMITDATE      varchar2 default '', ---- '入院时间';
                                    v_OUTWARDDATE    varchar2 default '', ---- '出院时间';
                                    v_OUTHOSDEPT     varchar2 default '', ---- '出院科室';
                                    v_OUTHOSWARD     varchar2 default '', ---- '出院病区';
                                    v_ACTUALDAYS     varchar2 default '', ---- '实际住院天数';
                                    v_AgeStr         varchar2 default '', ---- '年龄 精确到月天;2012年5月9日9:31:03 （杨维康 泗县修改）
                                    v_PatId          varchar2 default '', --新增的付款方式 add by ywk 2012年5月14日 16:02:13
                                    v_CardNo         varchar2 default '', --健康卡号
                                    
                                    -----add by ywk  2012年5月16日9:45:27 Inpatient表l里增加病案首页里相应的字段
                                    v_Districtid     varchar2 default '', --出生地‘县’
                                    v_xzzproviceid   varchar2 default '', --现在住址省
                                    v_xzzcityid      varchar2 default '', --现在住址市
                                    v_xzzdistrictid  varchar2 default '', --现在住址县
                                    v_xzztel         varchar2 default '', --现在住址电话
                                    v_hkdzproviceid  varchar2 default '', --户口住址省
                                    v_hkzdcityid     varchar2 default '', --户口住址市
                                    v_hkzddistrictid varchar2 default '', --户口住址县
                                    v_xzzpost        varchar2 default '', --现住址邮编
                                    v_isupdate       varchar2 default '', ---2012年5月24日 17:19:10 ywk 是否更新身份证号字段
                                    /*  v_csdprovicename   varchar2 default '',      --出生地省名称
                                                                                                                                                                                               v_csdcityname   varchar2 default '',      --出生地市名称
                                                                                                                                                                                               v_csddistrictname  varchar2 default '',---出生地县名称
                                                                                                                                                                                                 v_jgprovicename  varchar2 default '',---籍贯省名称
                                                                                                                                                                                                 v_jgcityname  varchar2 default '',---籍贯市名称
                                                                                                                                                                                                 v_xzzprovicename  varchar2 default '',---现住址省名称
                                                                                                                                                                                                   v_xzzcityname  varchar2 default '',---现住址市名称
                                                                                                                                                                                                    v_xzzdistrictname  varchar2 default '',---现住址县名称
                                                                                                                                                                                                    v_hkzzprovicename    varchar2 default '',---户口住址省名称
                                                                                                                                                                                               v_hkzzcityname      varchar2 default '',---户口住址市名称
                                                                                                                                                                                               v_hkzzdistrictname    varchar2 default ''---户口住址县名称*/
                                    v_CSDADDRESS  varchar2 default '', --出生地具体地址 add by ywk 2012年7月11日 10:13:49
                                    v_JGADDRESS   varchar2 default '', --籍贯地址具体地址 add by ywk 2012年7月11日 10:13:49
                                    v_XZZADDRESS  varchar2 default '', --现住址具体地址 add by ywk 2012年7月11日 10:13:49
                                    v_HKDZADDRESS varchar2 default '' --户口地址具体地址 add by ywk 2012年7月11日 10:13:49
                                    ,v_XZZDetailAddr varchar2 default '', --现住址详细地址(县级以下) add by cyq 2012-12-27
                                    v_HKDZDetailAddr varchar2 default '' --户口地址详细地址(县级以下) add by cyq 2012-12-27
                                    );

  --获得首页默认表里的数据
  --add by ywk 2012年5月17日 09:36:46

  PROCEDURE usp_GetDefaultInpatient(o_result OUT empcurtyp);
  --根据病案首页序号。取得病人的信息 用来填充病案首页
  PROCEDURE usp_GetInpatientByNo(v_noofinpat varchar2 default '',
                                 o_result    OUT empcurtyp);

  /*
  * 维护病案首页的费用信息
  add by ywk 2012年10月16日 19:29:53
  */
  PROCEDURE usp_editiem_mainpage_feeinfo(v_edittype        varchar2 default '',
                                         v_IEM_MAINPAGE_NO varchar2 default '', ---- '病案首页标识';
                                         v_TotalFee        varchar2 default '', ---- 总费用;
                                         v_OwnerFee        varchar2 default '', ---- '自付金额
                                         v_YbMedServFee    varchar2 default '', ---- 一般医疗服务费
                                         v_YbMedOperFee    varchar2 default '', ---- 一般治疗操作费
                                         v_NurseFee        varchar2 default '', ----护理费
                                         v_OtherInfo       varchar2 default '', ---- 综合类 其他费用
                                         v_BLZDFee         varchar2 default '', ---- 诊断类 病理诊断费
                                         v_SYSZDFee        varchar2 default '', ---- 实验室诊断费
                                         v_YXXZDFee        varchar2 default '', ----  诊断类 影像学诊断费
                                         v_LCZDItemFee     varchar2 default '', ----  诊断类 临床诊断项目费
                                         v_FSSZLItemFee    varchar2 default '', ----  非手术治疗项目费
                                         v_LCWLZLFee       varchar2 default '', ---- 治疗类 临床物理治疗费
                                         v_OperMedFee      varchar2 default '', ----治疗类 手术治疗费
                                         v_KFFee           varchar2 default '', ----康复类 康复费
                                         v_ZYZLFee         varchar2 default '', ----中医类 中医治疗费
                                         v_XYMedFee        varchar2 default '', ---西药类 西药费
                                         v_KJYWFee         varchar2 default '', ---西药类 抗菌药物费用
                                         v_ZCYFFee         varchar2 default '', ---中药类 中成药费
                                         v_ZCaoYFFee       varchar2 default '', ---中药类 中草药费
                                         v_BloodFee        varchar2 default '', ---血液和血液制品类 血费
                                         v_BDBLZPFFee      varchar2 default '', ---血液和血液制品类 白蛋白类制品费
                                         v_QDBLZPFFee      varchar2 default '', ---球蛋白类制品费
                                         v_NXYZLZPFFee     varchar2 default '', ---血液和血液制品类 凝血因子类制品费
                                         v_XBYZLZPFFee     varchar2 default '', ---细胞因子类制品费
                                         v_JCYYCXYYCLFFee  varchar2 default '', -- 检查用一次性医用材料费
                                         v_ZLYYCXYYCLFFee  varchar2 default '', -- /耗材类 治疗用一次性医用材料费
                                         v_SSYYCXYYCLFFee  varchar2 default '', -- 材类 手术用一次性医用材料费
                                         v_QTFee           varchar2 default '', -- 其他类：（24）其他费        
                                         v_Memo1           varchar2 default '', -- 预留字段   1    
                                         v_Memo2           varchar2 default '', -- 预留字段    2  
                                          v_Memo3           varchar2 default '', -- 预留字段     3 
                                          v_MaZuiFee           varchar2 default '', -- 麻醉费
                                         v_ShouShuFee           varchar2 default ''-- 手术费 
                                             
                                                                            );
                                                                            
  
  ----操作病案首页自动评分配置表                                                              
  procedure usp_operiem_mainpage_qc
  (
            v_OperType         varchar2 default '0',
            v_id               varchar2 default '0',
            v_tabletype        varchar2 default '0',
            v_fields           varchar2 default '',
            v_fieldsvalue      varchar2 default '',
            v_conditiontabletype         varchar2 default '',
            v_conditionfields            varchar2 default '',
            v_conditionfieldsvalue       varchar2 default '',
            v_REDUCTSCORE                varchar2 default '0',
            v_REDUCTREASON               varchar2 default '',
            v_VALIDE                     varchar2 default '0',
            o_result                     OUT empcurtyp
            
  );
  
  
 

END;
/
CREATE OR REPLACE PACKAGE BODY iem_main_page IS
  /*插入病案首页信息*/
  PROCEDURE usp_insertiembasicinfo(v_patnoofhis               varchar,
                                   v_noofinpat                varchar,
                                   v_payid                    VARCHAR,
                                   v_socialcare               VARCHAR,
                                   v_incount                  VARCHAR,
                                   v_name                     VARCHAR,
                                   v_sexid                    VARCHAR,
                                   v_birth                    VARCHAR,
                                   v_marital                  VARCHAR,
                                   v_jobid                    VARCHAR,
                                   v_provinceid               VARCHAR,
                                   v_countyid                 VARCHAR,
                                   v_nationid                 VARCHAR,
                                   v_nationalityid            VARCHAR,
                                   v_idno                     VARCHAR,
                                   v_organization             VARCHAR,
                                   v_officeplace              VARCHAR,
                                   v_officetel                VARCHAR,
                                   v_officepost               VARCHAR,
                                   v_nativeaddress            VARCHAR,
                                   v_nativetel                VARCHAR,
                                   v_nativepost               VARCHAR,
                                   v_contactperson            VARCHAR,
                                   v_relationship             VARCHAR,
                                   v_contactaddress           VARCHAR,
                                   v_contacttel               VARCHAR,
                                   v_admitdate                VARCHAR,
                                   v_admitdept                VARCHAR,
                                   v_admitward                VARCHAR,
                                   v_days_before              VARCHAR,
                                   v_trans_date               VARCHAR,
                                   v_trans_admitdept          VARCHAR,
                                   v_trans_admitward          VARCHAR,
                                   v_trans_admitdept_again    VARCHAR,
                                   v_outwarddate              VARCHAR,
                                   v_outhosdept               VARCHAR,
                                   v_outhosward               VARCHAR,
                                   v_actual_days              VARCHAR,
                                   v_death_time               VARCHAR,
                                   v_death_reason             VARCHAR,
                                   v_admitinfo                VARCHAR,
                                   v_in_check_date            VARCHAR,
                                   v_pathology_diagnosis_name VARCHAR,
                                   v_pathology_observation_sn VARCHAR,
                                   v_ashes_diagnosis_name     VARCHAR,
                                   v_ashes_anatomise_sn       VARCHAR,
                                   v_allergic_drug            VARCHAR,
                                   v_hbsag                    VARCHAR,
                                   v_hcv_ab                   VARCHAR,
                                   v_hiv_ab                   VARCHAR,
                                   v_opd_ipd_id               VARCHAR,
                                   v_in_out_inpatinet_id      VARCHAR,
                                   v_before_after_or_id       VARCHAR,
                                   v_clinical_pathology_id    VARCHAR,
                                   v_pacs_pathology_id        VARCHAR,
                                   v_save_times               VARCHAR,
                                   v_success_times            VARCHAR,
                                   v_section_director         VARCHAR,
                                   v_director                 VARCHAR,
                                   v_vs_employee_code         VARCHAR,
                                   v_resident_employee_code   VARCHAR,
                                   v_refresh_employee_code    VARCHAR,
                                   v_master_interne           VARCHAR,
                                   v_interne                  VARCHAR,
                                   v_coding_user              VARCHAR,
                                   v_medical_quality_id       VARCHAR,
                                   v_quality_control_doctor   VARCHAR,
                                   v_quality_control_nurse    VARCHAR,
                                   v_quality_control_date     VARCHAR,
                                   v_xay_sn                   VARCHAR,
                                   v_ct_sn                    VARCHAR,
                                   v_mri_sn                   VARCHAR,
                                   v_dsa_sn                   VARCHAR,
                                   v_is_first_case            VARCHAR,
                                   v_is_following             VARCHAR,
                                   v_following_ending_date    VARCHAR,
                                   v_is_teaching_case         VARCHAR,
                                   v_blood_type_id            VARCHAR,
                                   v_rh                       VARCHAR,
                                   v_blood_reaction_id        VARCHAR,
                                   v_blood_rbc                VARCHAR,
                                   v_blood_plt                VARCHAR,
                                   v_blood_plasma             VARCHAR,
                                   v_blood_wb                 VARCHAR,
                                   v_blood_others             VARCHAR,
                                   v_is_completed             VARCHAR,
                                   v_completed_time           VARCHAR,
                                   v_create_user              VARCHAR,
                                   v_Zymosis                  varchar,
                                   v_Hurt_Toxicosis_Ele       varchar,
                                   v_ZymosisState             varchar,
                                   o_result                   OUT empcurtyp) IS
  BEGIN
    INSERT INTO iem_mainpage_basicinfo
      (iem_mainpage_no,
       patnoofhis,
       noofinpat,
       payid,
       socialcare,
       incount,
       NAME,
       sexid,
       birth,
       marital,
       jobid,
       provinceid,
       countyid,
       nationid,
       nationalityid,
       idno,
       ORGANIZATION,
       officeplace,
       officetel,
       officepost,
       nativeaddress,
       nativetel,
       nativepost,
       contactperson,
       relationship,
       contactaddress,
       contacttel,
       admitdate,
       admitdept,
       admitward,
       days_before,
       trans_date,
       trans_admitdept,
       trans_admitward,
       trans_admitdept_again,
       outwarddate,
       outhosdept,
       outhosward,
       actual_days,
       death_time,
       death_reason,
       admitinfo,
       in_check_date,
       pathology_diagnosis_name,
       pathology_observation_sn,
       ashes_diagnosis_name,
       ashes_anatomise_sn,
       allergic_drug,
       hbsag,
       hcv_ab,
       hiv_ab,
       opd_ipd_id,
       in_out_inpatinet_id,
       before_after_or_id,
       clinical_pathology_id,
       pacs_pathology_id,
       save_times,
       success_times,
       section_director,
       director,
       vs_employee_code,
       resident_employee_code,
       refresh_employee_code,
       master_interne,
       interne,
       coding_user,
       medical_quality_id,
       quality_control_doctor,
       quality_control_nurse,
       quality_control_date,
       xay_sn,
       ct_sn,
       mri_sn,
       dsa_sn,
       is_first_case,
       is_following,
       following_ending_date,
       is_teaching_case,
       blood_type_id,
       rh,
       blood_reaction_id,
       blood_rbc,
       blood_plt,
       blood_plasma,
       blood_wb,
       blood_others,
       is_completed,
       completed_time,
       create_user,
       valide,
       create_time,
       Zymosis,
       Hurt_Toxicosis_Ele,
       ZymosisState)
    VALUES
      (seq_iem_mainpage_basicinfo_id.NEXTVAL,
       v_patnoofhis,
       v_noofinpat,
       v_payid,
       v_socialcare,
       v_incount,
       v_name,
       v_sexid,
       v_birth,
       v_marital,
       v_jobid,
       v_provinceid,
       v_countyid,
       v_nationid,
       v_nationalityid,
       v_idno,
       v_organization,
       v_officeplace,
       v_officetel,
       v_officepost,
       v_nativeaddress,
       v_nativetel,
       v_nativepost,
       v_contactperson,
       v_relationship,
       v_contactaddress,
       v_contacttel,
       v_admitdate,
       v_admitdept,
       v_admitward,
       v_days_before,
       v_trans_date,
       v_trans_admitdept,
       v_trans_admitward,
       v_trans_admitdept_again,
       v_outwarddate,
       v_outhosdept,
       v_outhosward,
       v_actual_days,
       v_death_time,
       v_death_reason,
       v_admitinfo,
       v_in_check_date,
       v_pathology_diagnosis_name,
       v_pathology_observation_sn,
       v_ashes_diagnosis_name,
       v_ashes_anatomise_sn,
       v_allergic_drug,
       v_hbsag,
       v_hcv_ab,
       v_hiv_ab,
       v_opd_ipd_id,
       v_in_out_inpatinet_id,
       v_before_after_or_id,
       v_clinical_pathology_id,
       v_pacs_pathology_id,
       v_save_times,
       v_success_times,
       v_section_director,
       v_director,
       v_vs_employee_code,
       v_resident_employee_code,
       v_refresh_employee_code,
       v_master_interne,
       v_interne,
       v_coding_user,
       v_medical_quality_id,
       v_quality_control_doctor,
       v_quality_control_nurse,
       v_quality_control_date,
       v_xay_sn,
       v_ct_sn,
       v_mri_sn,
       v_dsa_sn,
       v_is_first_case,
       v_is_following,
       v_following_ending_date,
       v_is_teaching_case,
       v_blood_type_id,
       v_rh,
       v_blood_reaction_id,
       v_blood_rbc,
       v_blood_plt,
       v_blood_plasma,
       v_blood_wb,
       v_blood_others,
       v_is_completed,
       v_completed_time,
       v_create_user,
       1,
       TO_CHAR(SYSDATE, 'yyyy-mm-dd hh24:mi:ss'),
       v_Zymosis,
       v_Hurt_Toxicosis_Ele,
       v_ZymosisState);
  
    OPEN o_result FOR
      SELECT seq_iem_mainpage_basicinfo_id.CURRVAL FROM DUAL;
  END;

  /*修改病案首页信息*/
  PROCEDURE usp_Upateiembasicinfo(v_iem_mainpage_no          varchar,
                                  v_patnoofhis               varchar,
                                  v_noofinpat                integer,
                                  v_payid                    VARCHAR,
                                  v_socialcare               VARCHAR,
                                  v_incount                  VARCHAR,
                                  v_name                     VARCHAR,
                                  v_sexid                    VARCHAR,
                                  v_birth                    VARCHAR,
                                  v_marital                  VARCHAR,
                                  v_jobid                    VARCHAR,
                                  v_provinceid               VARCHAR,
                                  v_countyid                 VARCHAR,
                                  v_nationid                 VARCHAR,
                                  v_nationalityid            VARCHAR,
                                  v_idno                     VARCHAR,
                                  v_organization             VARCHAR,
                                  v_officeplace              VARCHAR,
                                  v_officetel                VARCHAR,
                                  v_officepost               VARCHAR,
                                  v_nativeaddress            VARCHAR,
                                  v_nativetel                VARCHAR,
                                  v_nativepost               VARCHAR,
                                  v_contactperson            VARCHAR,
                                  v_relationship             VARCHAR,
                                  v_contactaddress           VARCHAR,
                                  v_contacttel               VARCHAR,
                                  v_admitdate                VARCHAR,
                                  v_admitdept                VARCHAR,
                                  v_admitward                VARCHAR,
                                  v_days_before              VARCHAR,
                                  v_trans_date               VARCHAR,
                                  v_trans_admitdept          VARCHAR,
                                  v_trans_admitward          VARCHAR,
                                  v_trans_admitdept_again    VARCHAR,
                                  v_outwarddate              VARCHAR,
                                  v_outhosdept               VARCHAR,
                                  v_outhosward               VARCHAR,
                                  v_actual_days              VARCHAR,
                                  v_death_time               VARCHAR,
                                  v_death_reason             VARCHAR,
                                  v_admitinfo                VARCHAR,
                                  v_in_check_date            VARCHAR,
                                  v_pathology_diagnosis_name VARCHAR,
                                  v_pathology_observation_sn VARCHAR,
                                  v_ashes_diagnosis_name     VARCHAR,
                                  v_ashes_anatomise_sn       VARCHAR,
                                  v_allergic_drug            VARCHAR,
                                  v_hbsag                    VARCHAR,
                                  v_hcv_ab                   VARCHAR,
                                  v_hiv_ab                   VARCHAR,
                                  v_opd_ipd_id               VARCHAR,
                                  v_in_out_inpatinet_id      VARCHAR,
                                  v_before_after_or_id       VARCHAR,
                                  v_clinical_pathology_id    VARCHAR,
                                  v_pacs_pathology_id        VARCHAR,
                                  v_save_times               VARCHAR,
                                  v_success_times            VARCHAR,
                                  v_section_director         VARCHAR,
                                  v_director                 VARCHAR,
                                  v_vs_employee_code         VARCHAR,
                                  v_resident_employee_code   VARCHAR,
                                  v_refresh_employee_code    VARCHAR,
                                  v_master_interne           VARCHAR,
                                  v_interne                  VARCHAR,
                                  v_coding_user              VARCHAR,
                                  v_medical_quality_id       VARCHAR,
                                  v_quality_control_doctor   VARCHAR,
                                  v_quality_control_nurse    VARCHAR,
                                  v_quality_control_date     VARCHAR,
                                  v_xay_sn                   VARCHAR,
                                  v_ct_sn                    VARCHAR,
                                  v_mri_sn                   VARCHAR,
                                  v_dsa_sn                   VARCHAR,
                                  v_is_first_case            VARCHAR,
                                  v_is_following             VARCHAR,
                                  v_following_ending_date    VARCHAR,
                                  v_is_teaching_case         VARCHAR,
                                  v_blood_type_id            VARCHAR,
                                  v_rh                       VARCHAR,
                                  v_blood_reaction_id        VARCHAR,
                                  v_blood_rbc                VARCHAR,
                                  v_blood_plt                VARCHAR,
                                  v_blood_plasma             VARCHAR,
                                  v_blood_wb                 VARCHAR,
                                  v_blood_others             VARCHAR,
                                  v_is_completed             VARCHAR,
                                  v_completed_time           VARCHAR,
                                  v_Zymosis                  varchar,
                                  v_Hurt_Toxicosis_Ele       varchar,
                                  v_ZymosisState             varchar,
                                  o_result                   OUT empcurtyp) IS
  BEGIN
    update iem_mainpage_basicinfo
       set patnoofhis               = v_patnoofhis,
           noofinpat                = v_noofinpat,
           payid                    = v_payid,
           socialcare               = v_socialcare,
           incount                  = v_incount,
           NAME                     = v_NAME,
           sexid                    = v_sexid,
           birth                    = v_birth,
           marital                  = v_marital,
           jobid                    = v_jobid,
           provinceid               = v_provinceid,
           countyid                 = v_countyid,
           nationid                 = v_nationid,
           nationalityid            = v_nationalityid,
           idno                     = v_idno,
           ORGANIZATION             = v_ORGANIZATION,
           officeplace              = v_officeplace,
           officetel                = v_officetel,
           officepost               = v_officepost,
           nativeaddress            = v_nativeaddress,
           nativetel                = v_nativetel,
           nativepost               = v_nativepost,
           contactperson            = v_contactperson,
           relationship             = v_relationship,
           contactaddress           = v_contactaddress,
           contacttel               = v_contacttel,
           admitdate                = v_admitdate,
           admitdept                = v_admitdept,
           admitward                = v_admitward,
           days_before              = v_days_before,
           trans_date               = v_trans_date,
           trans_admitdept          = v_trans_admitdept,
           trans_admitward          = v_trans_admitward,
           trans_admitdept_again    = v_trans_admitdept_again,
           outwarddate              = v_outwarddate,
           outhosdept               = v_outhosdept,
           outhosward               = v_outhosward,
           actual_days              = v_actual_days,
           death_time               = v_death_time,
           death_reason             = v_death_reason,
           admitinfo                = v_admitinfo,
           in_check_date            = v_in_check_date,
           pathology_diagnosis_name = v_pathology_diagnosis_name,
           pathology_observation_sn = v_pathology_observation_sn,
           ashes_diagnosis_name     = v_ashes_diagnosis_name,
           ashes_anatomise_sn       = v_ashes_anatomise_sn,
           allergic_drug            = v_allergic_drug,
           hbsag                    = v_hbsag,
           hcv_ab                   = v_hcv_ab,
           hiv_ab                   = v_hiv_ab,
           opd_ipd_id               = v_opd_ipd_id,
           in_out_inpatinet_id      = v_in_out_inpatinet_id,
           before_after_or_id       = v_before_after_or_id,
           clinical_pathology_id    = v_clinical_pathology_id,
           pacs_pathology_id        = v_pacs_pathology_id,
           save_times               = v_save_times,
           success_times            = v_success_times,
           section_director         = v_section_director,
           director                 = v_director,
           vs_employee_code         = v_vs_employee_code,
           resident_employee_code   = v_resident_employee_code,
           refresh_employee_code    = v_refresh_employee_code,
           master_interne           = v_master_interne,
           interne                  = v_interne,
           coding_user              = v_coding_user,
           medical_quality_id       = v_medical_quality_id,
           quality_control_doctor   = v_quality_control_doctor,
           quality_control_nurse    = v_quality_control_nurse,
           quality_control_date     = v_quality_control_date,
           xay_sn                   = v_xay_sn,
           ct_sn                    = v_ct_sn,
           mri_sn                   = v_mri_sn,
           dsa_sn                   = v_dsa_sn,
           is_first_case            = v_is_first_case,
           is_following             = v_is_following,
           following_ending_date    = v_following_ending_date,
           is_teaching_case         = v_is_teaching_case,
           blood_type_id            = v_blood_type_id,
           rh                       = v_rh,
           blood_reaction_id        = v_blood_reaction_id,
           blood_rbc                = v_blood_rbc,
           blood_plt                = v_blood_plt,
           blood_plasma             = v_blood_plasma,
           blood_wb                 = v_blood_wb,
           blood_others             = v_blood_others,
           is_completed             = v_is_completed,
           completed_time           = v_completed_time,
           Zymosis                  = v_Zymosis,
           Hurt_Toxicosis_Ele       = v_Hurt_Toxicosis_Ele,
           ZymosisState             = v_ZymosisState
     where iem_mainpage_no = v_iem_mainpage_no
       and valide = 1;
  
    OPEN o_result FOR
      SELECT v_iem_mainpage_no FROM DUAL;
  END;

  /*********************************************************************************/
  PROCEDURE usp_insert_iem_mainpage_diag(v_iem_mainpage_no   VARCHAR,
                                         v_diagnosis_type_id VARCHAR,
                                         v_diagnosis_code    VARCHAR,
                                         v_diagnosis_name    VARCHAR,
                                         v_status_id         VARCHAR,
                                         v_order_value       VARCHAR,
                                         --v_Valide numeric ,
                                         v_create_user VARCHAR
                                         --v_Create_Time varchar(19) ,
                                         --v_Cancel_User varchar(10) ,
                                         --v_Cancel_Time varchar(19)
                                         ) AS /**********
                                                                                   版本号  1.0.0.0.0
                                                                                   创建时间
                                                                                   作者
                                                                                   版权
                                                                                   描述  插入功病案首页诊断TABLE
                                                                                   功能说明
                                                                                   输入参数
                                                                                   输出参数
                                                                                   结果集、排序
            
                                                                                   调用的sp
                                                                                   调用实例
            
                                                                                   修改记录
                                                                                  **********/
  BEGIN
    INSERT INTO iem_mainpage_diagnosis
      (iem_mainpage_diagnosis_no,
       iem_mainpage_no,
       diagnosis_type_id,
       diagnosis_code,
       diagnosis_name,
       status_id,
       order_value,
       valide,
       create_user,
       create_time)
    VALUES
      (seq_iem_mainpage_diagnosis_id.NEXTVAL,
       v_iem_mainpage_no, -- Iem_Mainpage_NO - numeric
       v_diagnosis_type_id,
       -- Diagnosis_Type_Id - numeric
       v_diagnosis_code,
       -- Diagnosis_Code - varchar(60)
       v_diagnosis_name, -- Diagnosis_Name - varchar(300)
       v_status_id, -- Status_Id - numeric
       v_order_value,
       -- Order_Value - numeric
       1,
       -- Valide - numeric
       v_create_user, -- Create_User - varchar(10)
       TO_CHAR(SYSDATE, 'yyyy-mm-dd HH24:mi:ss')
       -- Create_Time - varchar(19)
       );
  END;

  /*********************************************************************************/
  PROCEDURE usp_insert_iem_mainpage_oper(v_iem_mainpage_no     NUMERIC,
                                         v_operation_code      VARCHAR,
                                         v_operation_date      VARCHAR,
                                         v_operation_name      VARCHAR,
                                         v_execute_user1       VARCHAR,
                                         v_execute_user2       VARCHAR,
                                         v_execute_user3       VARCHAR,
                                         v_anaesthesia_type_id NUMERIC,
                                         v_close_level         NUMERIC,
                                         v_anaesthesia_user    VARCHAR,
                                         --v_Valide numeric ,
                                         v_create_user VARCHAR
                                         --v_Create_Time varchar(19)
                                         --v_Cancel_User varchar(10) ,
                                         --v_Cancel_Time varchar(19)
                                         ) AS /**********
                                                                                   版本号  1.0.0.0.0
                                                                                   创建时间
                                                                                   作者
                                                                                   版权
                                                                                   描述  插入功病案首页手术TABLE
                                                                                   功能说明
                                                                                   输入参数
                                                                                   输出参数
                                                                                   结果集、排序
            
                                                                                   调用的sp
                                                                                   调用实例
            
                                                                                   修改记录
                                                                                  **********/
  BEGIN
    INSERT INTO iem_mainpage_operation
      (iem_mainpage_operation_no,
       iem_mainpage_no,
       operation_code,
       operation_date,
       operation_name,
       execute_user1,
       execute_user2,
       execute_user3,
       anaesthesia_type_id,
       close_level,
       anaesthesia_user,
       valide,
       create_user,
       create_time)
    VALUES
      (seq_iem_mainpage_operation_id.NEXTVAL,
       v_iem_mainpage_no, --numeric
       v_operation_code, -- varchar(60)
       v_operation_date,
       -- varchar(19)
       v_operation_name, -- varchar(300)
       v_execute_user1, -- varchar(20)
       v_execute_user2,
       -- varchar(20)
       v_execute_user3, -- varchar(20)
       v_anaesthesia_type_id, -- numeric
       v_close_level,
       -- numeric
       v_anaesthesia_user, -- varchar(20)
       1, -- numeric
       v_create_user, -- varchar(10)
       TO_CHAR(SYSDATE, 'yyyy-mm-dd HH24:mi:ss'));
  END;

  /*
  * 插入病案首页中 产妇婴儿信息
  */
  PROCEDURE usp_insert_iem_main_ObsBaby(v_iem_mainpage_no NUMERIC,
                                        v_IBSBABYID       VARCHAR, --编号
                                        v_TC              VARCHAR, --胎次
                                        v_CC              VARCHAR, -- 产次
                                        v_TB              VARCHAR, -- 胎别
                                        v_CFHYPLD         VARCHAR, --产妇会阴破裂度
                                        v_MIDWIFERY       VARCHAR, --接产者
                                        v_SEX             VARCHAR, --性别
                                        v_APJ             VARCHAR, -- 阿帕加评加
                                        v_HEIGH           VARCHAR, --身长
                                        v_WEIGHT          VARCHAR, --体重
                                        v_CCQK            VARCHAR, --产出情况
                                        v_BITHDAY         VARCHAR, --出生时间
                                        v_FMFS            VARCHAR, --     分娩方式
                                        v_CYQK            VARCHAR,
                                        v_create_user     VARCHAR) as
  begin
    insert into IEM_MAINPAGE_OBSTETRICSBABY
      (IEM_MAINPAGE_OBSBABYID,
       IEM_MAINPAGE_NO,
       IBSBABYID, --编号
       TC, --胎次
       CC, -- 产次
       TB, -- 胎别
       CFHYPLD, --产妇会阴破裂度
       MIDWIFERY, --接产者
       SEX, --性别
       APJ, -- 阿帕加评加
       HEIGH, --身长
       WEIGHT, --体重
       CCQK, --产出情况
       BITHDAY, --出生时间
       FMFS, --      分娩方式
       CYQK, --出院情况
       create_user,
       create_time)
    values
      (SEQ_IEM_MAINPAGE_OBSBABY_ID.Nextval,
       v_IEM_MAINPAGE_NO,
       v_IBSBABYID, --编号
       v_TC, --胎次
       v_CC, -- 产次
       v_TB, -- 胎别
       v_CFHYPLD, --产妇会阴破裂度
       v_MIDWIFERY, --接产者
       v_SEX, --性别
       v_APJ, -- 阿帕加评加
       v_HEIGH, --身长
       v_WEIGHT, --体重
       v_CCQK, --产出情况
       v_BITHDAY, --出生时间
       v_FMFS, --      分娩方式
       v_CYQK,
       v_create_user, -- varchar(10)
       TO_CHAR(SYSDATE, 'yyyy-mm-dd HH24:mi:ss'));
  end;

  /*
  * 插入一条病人信息(在没有连HIS的情况下手动输入病人信息)
  */
  PROCEDURE usp_insertpatientinfo(v_noofinpat       varchar,
                                  v_patnoofhis      VARCHAR,
                                  v_Noofclinic      VARCHAR,
                                  v_Noofrecord      VARCHAR,
                                  v_patid           VARCHAR,
                                  v_Innerpix        VARCHAR,
                                  v_outpix          VARCHAR,
                                  v_Name            VARCHAR,
                                  v_py              VARCHAR,
                                  v_wb              VARCHAR,
                                  v_payid           VARCHAR,
                                  v_ORIGIN          VARCHAR,
                                  v_InCount         int DEFAULT 0,
                                  v_sexid           VARCHAR,
                                  v_Birth           VARCHAR,
                                  v_Age             int DEFAULT 0,
                                  v_AgeStr          VARCHAR,
                                  v_IDNO            VARCHAR,
                                  v_Marital         VARCHAR,
                                  v_JobID           VARCHAR,
                                  v_CSDProvinceID   VARCHAR,
                                  v_CSDCityID       VARCHAR,
                                  v_CSDDistrictID   VARCHAR,
                                  v_NationID        VARCHAR,
                                  v_NationalityID   VARCHAR,
                                  v_JGProvinceID    VARCHAR,
                                  v_JGCityID        VARCHAR,
                                  v_Organization    VARCHAR,
                                  v_OfficePlace     VARCHAR,
                                  v_OfficeTEL       VARCHAR,
                                  v_OfficePost      VARCHAR,
                                  v_HKDZProvinceID  VARCHAR,
                                  v_HKDZCityID      VARCHAR,
                                  v_HKDZDistrictID  VARCHAR,
                                  v_NATIVEPOST      VARCHAR,
                                  v_NATIVETEL       VARCHAR,
                                  v_NATIVEADDRESS   VARCHAR,
                                  v_ADDRESS         VARCHAR,
                                  v_ContactPerson   VARCHAR,
                                  v_RelationshipID  VARCHAR,
                                  v_ContactAddress  VARCHAR,
                                  v_ContactTEL      VARCHAR,
                                  v_CONTACTOFFICE   VARCHAR,
                                  v_CONTACTPOST     VARCHAR,
                                  v_OFFERER         VARCHAR,
                                  v_SocialCare      VARCHAR,
                                  v_INSURANCE       VARCHAR,
                                  v_CARDNO          VARCHAR,
                                  v_ADMITINFO       VARCHAR,
                                  v_AdmitDeptID     VARCHAR,
                                  v_AdmitWardID     VARCHAR,
                                  v_ADMITBED        VARCHAR,
                                  v_AdmitDate       VARCHAR,
                                  v_INWARDDATE      VARCHAR,
                                  v_ADMITDIAGNOSIS  VARCHAR,
                                  v_OutWardDate     VARCHAR,
                                  v_OutHosDeptID    VARCHAR,
                                  v_OutHosWardID    VARCHAR,
                                  v_OutBed          VARCHAR,
                                  v_OUTHOSDATE      VARCHAR,
                                  v_OUTDIAGNOSIS    VARCHAR,
                                  v_TOTALDAYS       int DEFAULT 0,
                                  v_CLINICDIAGNOSIS VARCHAR,
                                  v_SOLARTERMS      VARCHAR,
                                  v_ADMITWAY        VARCHAR,
                                  v_OUTWAY          VARCHAR,
                                  v_CLINICDOCTOR    VARCHAR,
                                  v_RESIDENT        VARCHAR,
                                  v_ATTEND          VARCHAR,
                                  v_CHIEF           VARCHAR,
                                  v_EDU             VARCHAR,
                                  v_EDUC            int DEFAULT 0,
                                  v_RELIGION        VARCHAR,
                                  v_STATUS          int DEFAULT 0,
                                  v_CRITICALLEVEL   VARCHAR,
                                  v_ATTENDLEVEL     VARCHAR,
                                  v_EMPHASIS        int DEFAULT 0,
                                  v_ISBABY          int DEFAULT 0,
                                  v_MOTHER          NUMERIC,
                                  v_MEDICAREID      VARCHAR,
                                  v_MEDICAREQUOTA   int DEFAULT 0,
                                  v_VOUCHERSCODE    VARCHAR,
                                  v_STYLE           VARCHAR,
                                  v_OPERATOR        VARCHAR,
                                  v_MEMO            VARCHAR,
                                  v_CPSTATUS        int DEFAULT 0,
                                  v_OUTWARDBED      int DEFAULT 0,
                                  v_XZZProvinceID   VARCHAR,
                                  v_XZZCityID       VARCHAR,
                                  v_XZZDistrictID   VARCHAR,
                                  v_XZZTEL          VARCHAR,
                                  v_XZZPost         VARCHAR) as
    i_count integer;
  begin
    select count(*)
      into i_count
      from inpatient
     where inpatient.noofinpat = v_noofinpat;
    if (i_count <= 0) then
      insert into inpatient
        (NOOFINPAT,
         PATNOOFHIS,
         NOOFCLINIC,
         NOOFRECORD,
         PATID,
         INNERPIX,
         OUTPIX,
         NAME,
         PY,
         WB,
         PAYID,
         ORIGIN,
         INCOUNT,
         SEXID,
         BIRTH,
         AGE,
         AGESTR,
         IDNO,
         MARITAL,
         JOBID,
         PROVINCEID,
         COUNTYID,
         NATIONID,
         NATIONALITYID,
         NATIVEPLACE_P,
         NATIVEPLACE_C,
         ORGANIZATION,
         OFFICEPLACE,
         OFFICETEL,
         OFFICEPOST,
         NATIVEADDRESS,
         NATIVETEL,
         NATIVEPOST,
         ADDRESS,
         CONTACTPERSON,
         RELATIONSHIP,
         CONTACTADDRESS,
         CONTACTOFFICE,
         CONTACTTEL,
         CONTACTPOST,
         OFFERER,
         SOCIALCARE,
         INSURANCE,
         CARDNO,
         ADMITINFO,
         ADMITDEPT,
         ADMITWARD,
         ADMITBED,
         ADMITDATE,
         INWARDDATE,
         ADMITDIAGNOSIS,
         OUTHOSDEPT,
         OUTHOSWARD,
         OUTBED,
         OUTWARDDATE,
         OUTHOSDATE,
         OUTDIAGNOSIS,
         TOTALDAYS,
         CLINICDIAGNOSIS,
         SOLARTERMS,
         ADMITWAY,
         OUTWAY,
         CLINICDOCTOR,
         RESIDENT,
         ATTEND,
         CHIEF,
         EDU,
         EDUC,
         RELIGION,
         STATUS,
         CRITICALLEVEL,
         ATTENDLEVEL,
         EMPHASIS,
         ISBABY,
         MOTHER,
         MEDICAREID,
         MEDICAREQUOTA,
         VOUCHERSCODE,
         STYLE,
         OPERATOR,
         MEMO,
         CPSTATUS,
         OUTWARDBED,
         DISTRICTID,
         XZZPROVICEID,
         XZZCITYID,
         XZZDISTRICTID,
         XZZTEL,
         HKDZPROVICEID,
         HKZDCITYID,
         HKZDDISTRICTID,
         XZZPOST)
      values
        (SEQ_INPATIENT_ID.NEXTVAL,
         v_patnoofhis,
         v_Noofclinic,
         v_Noofrecord,
         v_patid,
         v_Innerpix,
         v_outpix,
         v_Name,
         v_py,
         v_wb,
         v_payid,
         v_ORIGIN,
         v_InCount,
         v_sexid,
         v_Birth,
         v_Age,
         v_AgeStr,
         v_IDNO,
         v_Marital,
         v_JobID,
         v_CSDProvinceID,
         v_CSDCityID,
         v_NationID,
         v_NationalityID,
         v_JGProvinceID,
         v_JGCityID,
         v_Organization,
         v_OfficePlace,
         v_OfficeTEL,
         v_OfficePost,
         v_NATIVEADDRESS,
         v_NATIVETEL,
         v_NATIVEPOST,
         v_ADDRESS,
         v_ContactPerson,
         v_RelationshipID,
         v_ContactAddress,
         v_CONTACTOFFICE,
         v_ContactTEL,
         v_CONTACTPOST,
         v_OFFERER,
         v_SocialCare,
         v_INSURANCE,
         v_CARDNO,
         v_ADMITINFO,
         v_AdmitDeptID,
         v_AdmitWardID,
         v_ADMITBED,
         v_AdmitDate,
         v_INWARDDATE,
         v_ADMITDIAGNOSIS,
         v_OutHosDeptID,
         v_OutHosWardID,
         v_OutBed,
         v_OutWardDate,
         v_OUTHOSDATE,
         v_OUTDIAGNOSIS,
         v_TOTALDAYS,
         v_CLINICDIAGNOSIS,
         v_SOLARTERMS,
         v_ADMITWAY,
         v_OUTWAY,
         v_CLINICDOCTOR,
         v_RESIDENT,
         v_ATTEND,
         v_CHIEF,
         v_EDU,
         v_EDUC,
         v_RELIGION,
         v_STATUS,
         v_CRITICALLEVEL,
         v_ATTENDLEVEL,
         v_EMPHASIS,
         v_ISBABY,
         v_MOTHER,
         v_MEDICAREID,
         v_MEDICAREQUOTA,
         v_VOUCHERSCODE,
         v_STYLE,
         v_OPERATOR,
         v_memo,
         v_CPSTATUS,
         v_OUTWARDBED,
         v_CSDDistrictID,
         v_XZZProvinceID,
         v_XZZCityID,
         v_XZZDistrictID,
         v_XZZTEL,
         v_HKDZProvinceID,
         v_HKDZCityID,
         v_HKDZDistrictID,
         v_XZZPost);
    else
      update inpatient
         set PATNOOFHIS      = v_patnoofhis,
             NOOFCLINIC      = v_Noofclinic,
             NOOFRECORD      = v_Noofrecord,
             PATID           = v_patid,
             INNERPIX        = v_Innerpix,
             OUTPIX          = v_outpix,
             NAME            = v_Name,
             PY              = v_py,
             WB              = v_wb,
             PAYID           = v_payid,
             ORIGIN          = v_ORIGIN,
             INCOUNT         = v_InCount,
             SEXID           = v_sexid,
             BIRTH           = v_Birth,
             AGE             = v_Age,
             AGESTR          = v_AgeStr,
             IDNO            = v_IDNO,
             MARITAL         = v_Marital,
             JOBID           = v_JobID,
             PROVINCEID      = v_CSDProvinceID,
             COUNTYID        = v_CSDCityID,
             NATIONID        = v_NationID,
             NATIONALITYID   = v_NationalityID,
             NATIVEPLACE_P   = v_JGProvinceID,
             NATIVEPLACE_C   = v_JGCityID,
             ORGANIZATION    = v_Organization,
             OFFICEPLACE     = v_OfficePlace,
             OFFICETEL       = v_OfficeTEL,
             OFFICEPOST      = v_OfficePost,
             NATIVEADDRESS   = v_NATIVEADDRESS,
             NATIVETEL       = v_NATIVETEL,
             NATIVEPOST      = v_NATIVEPOST,
             ADDRESS         = v_ADDRESS,
             CONTACTPERSON   = v_ContactPerson,
             RELATIONSHIP    = v_RelationshipID,
             CONTACTADDRESS  = v_ContactAddress,
             CONTACTOFFICE   = v_CONTACTOFFICE,
             CONTACTTEL      = v_ContactTEL,
             CONTACTPOST     = v_CONTACTPOST,
             OFFERER         = v_OFFERER,
             SOCIALCARE      = v_SocialCare,
             INSURANCE       = v_INSURANCE,
             CARDNO          = v_CARDNO,
             ADMITINFO       = v_ADMITINFO,
             ADMITDEPT       = v_AdmitDeptID,
             ADMITWARD       = v_AdmitWardID,
             ADMITBED        = v_ADMITBED,
             ADMITDATE       = v_AdmitDate,
             INWARDDATE      = v_INWARDDATE,
             ADMITDIAGNOSIS  = v_ADMITDIAGNOSIS,
             OUTHOSDEPT      = v_OutHosDeptID,
             OUTHOSWARD      = v_OutHosWardID,
             OUTBED          = v_OutBed,
             OUTWARDDATE     = v_OutWardDate,
             OUTHOSDATE      = v_OUTHOSDATE,
             OUTDIAGNOSIS    = v_OUTDIAGNOSIS,
             TOTALDAYS       = v_TOTALDAYS,
             CLINICDIAGNOSIS = v_CLINICDIAGNOSIS,
             SOLARTERMS      = v_SOLARTERMS,
             ADMITWAY        = v_ADMITWAY,
             OUTWAY          = v_OUTWAY,
             CLINICDOCTOR    = v_CLINICDOCTOR,
             RESIDENT        = v_RESIDENT,
             ATTEND          = v_ATTEND,
             CHIEF           = v_CHIEF,
             EDU             = v_EDU,
             EDUC            = v_EDUC,
             RELIGION        = v_RELIGION,
             STATUS          = v_STATUS,
             CRITICALLEVEL   = v_CRITICALLEVEL,
             ATTENDLEVEL     = v_ATTENDLEVEL,
             EMPHASIS        = v_EMPHASIS,
             ISBABY          = v_ISBABY,
             MOTHER          = v_MOTHER,
             MEDICAREID      = v_MEDICAREID,
             MEDICAREQUOTA   = v_MEDICAREQUOTA,
             VOUCHERSCODE    = v_VOUCHERSCODE,
             STYLE           = v_STYLE,
             OPERATOR        = v_OPERATOR,
             MEMO            = v_memo,
             CPSTATUS        = v_CPSTATUS,
             OUTWARDBED      = v_OUTWARDBED,
             DISTRICTID      = v_CSDDistrictID,
             XZZPROVICEID    = v_XZZProvinceID,
             XZZCITYID       = v_XZZCityID,
             XZZDISTRICTID   = v_XZZDistrictID,
             XZZTEL          = v_XZZTEL,
             HKDZPROVICEID   = v_HKDZProvinceID,
             HKZDCITYID      = v_HKDZCityID,
             HKZDDISTRICTID  = v_HKDZDistrictID,
             XZZPOST         = v_XZZPost
       where inpatient.noofinpat = v_noofinpat;
    end if;
  end;

  /*
  * 捞出所有的数据供报表设计器打印
  */
  PROCEDURE usp_get_iem_mainpage_all(
                                     --      v_noofinpat             VARCHAR,
                                     o_result  OUT empcurtyp,
                                     o_result1 OUT empcurtyp,
                                     o_result2 OUT empcurtyp) AS
  BEGIN
    open o_result for
      select *
        from iem_mainpage_basicinfo
       where iem_mainpage_basicinfo.valide = '1'
         and iem_mainpage_basicinfo.noofinpat = '151';
  
    open o_result1 for
      select *
        from iem_mainpage_diagnosis
       where iem_mainpage_diagnosis.valide = '1'
         and iem_mainpage_diagnosis.iem_mainpage_no in
             (select iem_mainpage_basicinfo.iem_mainpage_no
                from iem_mainpage_basicinfo
               where iem_mainpage_basicinfo.valide = '1'
                 and iem_mainpage_basicinfo.noofinpat = '151');
  
    open o_result2 for
      select *
        from iem_mainpage_operation
       where iem_mainpage_operation.valide = '1'
         and iem_mainpage_operation.iem_mainpage_no in
             (select iem_mainpage_basicinfo.iem_mainpage_no
                from iem_mainpage_basicinfo
               where iem_mainpage_basicinfo.valide = '1'
                 and iem_mainpage_basicinfo.noofinpat = '151');
  
  END;

  -- End of DDL Script for Package YIDANDBA.IEM_MAGE_PAGE

  /*********************************************************************************/
  PROCEDURE usp_getieminfo_new(v_noofinpat INT,
                               o_result    OUT empcurtyp,
                               o_result1   OUT empcurtyp,
                               o_result2   OUT empcurtyp,
                               o_result3   OUT empcurtyp) AS
    /**********
     版本号  1.0.0.0.0
     创建时间
     作者
     版权
     描述  获取质量评分
     功能说明
     输入参数
      v_NoOfInpat varchar(40)--首页序号
     输出参数
     结果集、排序
    质量控制统计数据集
    
     调用的sp
     调用实例
     exec usp_GetIemInfo  9
     修改记录
    **********/
    v_infono NUMERIC;
  BEGIN
    OPEN o_result FOR
      SELECT '' FROM DUAL;
  
    OPEN o_result1 FOR
      SELECT '' FROM DUAL;
  
    OPEN o_result2 FOR
      SELECT '' FROM DUAL;
  
    OPEN o_result3 FOR
      SELECT '' FROM DUAL;
  
    SELECT MAX(imb.iem_mainpage_no)
      INTO v_infono
      FROM iem_mainpage_basicinfo imb
     WHERE imb.noofinpat = v_noofinpat
       AND imb.valide = 1;
  
    --数据顺序不可变，与程序里相关
    --基本信息
    OPEN o_result FOR
      SELECT iem.iem_mainpage_no,
             iem.noofinpat,
             iem.socialcare,
             iem.payid,
             pay.name payName,
             
             iem.incount,
             iem.patnoofhis,
             iem.name,
             iem.sexid,
             iem.birth,
             
             iem.marital,
             iem.jobid,
             job.name       jobName,
             iem.provinceid,
             pro.name       provinceName,
             
             iem.countyid,
             county.name       countyName,
             iem.nationid,
             nation.name       NationName,
             iem.nationalityid,
             
             nationality.name nationalityName,
             iem.idno,
             iem.organization,
             iem.officeplace,
             iem.officetel,
             
             iem.officepost,
             iem.nativeaddress,
             iem.nativetel,
             iem.nativepost,
             iem.contactperson,
             
             iem.relationship,
             relationship.name  relationshipName,
             iem.contactaddress,
             iem.contacttel,
             iem.admitdate,
             
             iem.admitdept,
             AdmitDept.name      AdmitDeptName,
             iem.admitward,
             admitward.name      admitwardName,
             iem.trans_admitdept,
             
             trans_admitdept.name trans_admitdeptName,
             iem.outwarddate,
             iem.outhosdept,
             outhosdept.name      outhosdeptName,
             iem.outhosward,
             
             outhosward.name           outhoswardName,
             iem.actual_days,
             iem.days_before,
             iem.trans_date,
             iem.trans_admitdept_again,
             
             iem.Trans_AdmitWard,
             iem.death_time,
             iem.death_reason,
             
             ---诊断实体
             iem.admitinfo,
             iem.in_check_date,
             iem.zymosis,
             zymosis.name      zymosisName,
             iem.zymosisstate,
             
             iem.pathology_diagnosis_name,
             iem.pathology_observation_sn,
             iem.hurt_toxicosis_ele,
             iem.allergic_drug,
             iem.hbsag,
             
             iem.hcv_ab,
             iem.hiv_ab,
             iem.opd_ipd_id,
             iem.in_out_inpatinet_id,
             iem.before_after_or_id,
             
             iem.clinical_pathology_id,
             iem.pacs_pathology_id,
             iem.save_times,
             iem.success_times,
             iem.section_director,
             
             section_director.name section_directorName,
             iem.director,
             director.name         directorName,
             iem.vs_employee_code  vs_employeeID,
             vs_employee.name      vs_employeeName,
             
             iem.resident_employee_code resident_employeeID,
             resident_employee.name     resident_employeeName,
             iem.refresh_employee_code  refresh_employeeID,
             refresh_employee.name      refresh_employeeName,
             iem.master_interne,
             
             master_interne.name master_interneName,
             iem.interne,
             interne.name        interneName,
             iem.coding_user,
             coding_user.name    coding_userName,
             
             iem.medical_quality_id,
             iem.quality_control_doctor,
             quality_control_doctor.name quality_control_doctorName,
             iem.quality_control_nurse,
             quality_control_nurse.name  quality_control_nurseName,
             
             iem.quality_control_date,
             
             --费用模块
             (case
               when iem.ashes_diagnosis_name = '' then
                '2'
               else
                '1'
             end) Ashes_Check,
             iem.is_first_case,
             iem.is_following,
             iem.is_teaching_case,
             iem.following_ending_date,
             
             iem.blood_type_id,
             iem.rh,
             iem.blood_reaction_id,
             iem.blood_rbc,
             iem.blood_plt,
             
             iem.blood_plasma,
             iem.blood_wb,
             iem.blood_others,
             
             --基础信息实体
             iem.Is_Completed,
             iem.completed_time,
             iem.Valide,
             iem.Create_User,
             iem.Create_Time,
             iem.Modified_User,
             iem.Modified_Time,
             iem.xay_sn,
             iem.ct_sn,
             iem.mri_sn,
             iem.dsa_sn,
             iem.ashes_anatomise_sn,
             iem.ashes_diagnosis_name
      
        FROM iem_mainpage_basicinfo iem
        left join dictionary_detail pay
          on pay.detailid = iem.payid
         and pay.categoryid = '1'
        left join dictionary_detail job
          on job.detailid = iem.jobid
         and job.categoryid = '41'
        left join areas pro
          on pro.id = iem.provinceid
         and pro.category = '1000'
        left join areas county
          on county.id = iem.countyid
         and county.category = '1001'
        left join dictionary_detail nation
          on nation.detailid = iem.nationid
         and nation.categoryid = '42'
        left join dictionary_detail nationality
          on nationality.detailid = iem.nationalityid
         and nationality.categoryid = '43'
        left join dictionary_detail relationship
          on relationship.detailid = iem.relationship
         and relationship.categoryid = '44'
        left join department AdmitDept
          on AdmitDept.id = iem.admitdept
        left join ward admitward
          on admitward.id = iem.admitward
        left join department trans_admitdept
          on trans_admitdept.id = iem.trans_admitdept
        left join department outhosdept
          on outhosdept.id = iem.outhosdept
        left join ward outhosward
          on outhosward.id = iem.outhosward
        left join diagnosis zymosis
          on zymosis.markid = iem.zymosis
        left join users section_director
          on section_director.id = iem.section_director
        left join users director
          on director.id = iem.director
        left join users vs_employee
          on vs_employee.id = iem.vs_employee_code
        left join users resident_employee
          on resident_employee.id = iem.resident_employee_code
        left join users refresh_employee
          on refresh_employee.id = iem.refresh_employee_code
        left join users master_interne
          on master_interne.id = iem.master_interne
        left join users interne
          on interne.id = iem.interne
        left join users coding_user
          on coding_user.id = iem.coding_user
        left join users quality_control_doctor
          on quality_control_doctor.id = iem.quality_control_doctor
        left join users quality_control_nurse
          on quality_control_nurse.id = iem.quality_control_nurse
       WHERE iem.iem_mainpage_no = v_infono
         AND iem.valide = 1;
  
    --诊断
    OPEN o_result1 FOR
      SELECT diag.diagnosis_name,
             diag.status_id,
             (case
               when diag.status_id = '1' then
                '治愈'
               when diag.status_id = '2' then
                '好转'
               when diag.status_id = '3' then
                '未愈'
               when diag.status_id = '4' then
                '死亡'
               else
                '其他'
             end) Status_Name,
             diag.diagnosis_code,
             diag.diagnosis_type_id,
             diag.order_value
        FROM iem_mainpage_diagnosis diag
       WHERE iem_mainpage_no = v_infono
         AND valide = 1
       ORDER BY order_value;
  
    --手术
    /*    OPEN o_result2 FOR
    SELECT *
      FROM iem_mainpage_operation
     WHERE iem_mainpage_no = v_infono
       AND valide = 1;*/
  
    OPEN o_result2 FOR
    
      SELECT iem.iem_mainpage_operation_no,
             iem.iem_mainpage_no,
             iem.operation_code,
             iem.operation_date,
             iem.operation_name,
             u1.name                       execute_user1_Name,
             iem.execute_user1,
             u2.name                       execute_user2_Name,
             iem.execute_user2,
             u3.name                       execute_user3_Name,
             iem.execute_user3,
             --dic.name anaesthesia_type_Name,
             ab.name anaesthesia_type_Name,
             iem.anaesthesia_type_id,
             (case
               when iem.close_level = '1' then
                'I/甲'
               when iem.close_level = '2' then
                'II/甲'
               when iem.close_level = '3' then
                'III/甲'
               when iem.close_level = '4' then
                'I/乙'
               when iem.close_level = '5' then
                'II/乙'
               when iem.close_level = '6' then
                'III/乙'
               when iem.close_level = '7' then
                'I/丙'
               when iem.close_level = '8' then
                'II/丙'
               when iem.close_level = '9' then
                'III/丙'
               else
                ''
             end) close_level_Name,
             iem.close_level,
             ua.name anaesthesia_user_Name,
             iem.anaesthesia_user,
             iem.valide,
             iem.create_user,
             iem.create_time,
             iem.cancel_user,
             iem.cancel_time
        FROM iem_mainpage_operation iem
        left join users u1
          on iem.execute_user1 = u1.id
         and u1.valid = 1
        left join users u2
          on iem.execute_user2 = u2.id
         and u2.valid = 1
        left join users u3
          on iem.execute_user3 = u3.id
         and u3.valid = 1
        left join users ua
          on iem.anaesthesia_user = ua.id
         and ua.valid = 1
      /*     left join dictionary_detail dic on iem.anaesthesia_type_id =
          dic.detailid
      and dic.categoryid = '30'*/
        left join anesthesia ab
          on iem.anaesthesia_type_id = ab.id
       WHERE valide = 1
         and iem.iem_mainpage_no = v_infono;
  
    --产妇婴儿信息
    OPEN o_result3 FOR
      SELECT *
        FROM IEM_MAINPAGE_OBSTETRICSBABY
       WHERE iem_mainpage_no = v_infono
         AND valide = 1;
  END;

  /**(*********编辑病案首页基本信息********************************************/
  PROCEDURE usp_Edit_Iem_BasicInfo_2012(v_edittype        varchar2 default '',
                                        v_IEM_MAINPAGE_NO varchar2 default '', ---- '病案首页标识';
                                        v_PATNOOFHIS      varchar2 default '', ---- '病案号';
                                        v_NOOFINPAT       varchar2 default '', ---- '病人首页序号';
                                        v_PAYID           varchar2 default '', ---- '医疗付款方式ID';
                                        v_SOCIALCARE      varchar2 default '', ---- '社保卡号';
                                        
                                        v_INCOUNT varchar2 default '', ---- '入院次数';
                                        v_NAME    varchar2 default '', ---- '患者姓名';
                                        v_SEXID   varchar2 default '', ---- '性别';
                                        v_BIRTH   varchar2 default '', ---- '出生';
                                        v_MARITAL varchar2 default '', ---- '婚姻状况 1.未婚 2.已婚 3.丧偶4.离婚 9.其他';
                                        
                                        v_JOBID         varchar2 default '', ---- '职业';
                                        v_NATIONALITYID varchar2 default '', ---- '国籍ID';
                                        v_NATIONID      varchar2 default '', --民族
                                        v_IDNO          varchar2 default '', ---- '身份证号码';
                                        v_ORGANIZATION  varchar2 default '', ---- '工作单位';
                                        v_OFFICEPLACE   varchar2 default '', ---- '工作单位地址';
                                        
                                        v_OFFICETEL      varchar2 default '', ---- '工作单位电话';
                                        v_OFFICEPOST     varchar2 default '', ---- '工作单位邮编';
                                        v_CONTACTPERSON  varchar2 default '', ---- '联系人姓名';
                                        v_RELATIONSHIP   varchar2 default '', ---- '与联系人关系';
                                        v_CONTACTADDRESS varchar2 default '', ---- '联系人地址';
                                        
                                        v_CONTACTTEL varchar2 default '', ---- '联系人电话';
                                        v_ADMITDATE  varchar2 default '', ---- '入院时间';
                                        v_ADMITDEPT  varchar2 default '', ---- '入院科室';
                                        v_ADMITWARD  varchar2 default '', ---- '入院病区';
                                        v_TRANS_DATE varchar2 default '', ---- '转院时间';
                                        
                                        v_TRANS_ADMITDEPT varchar2 default '', ---- '转院科别';
                                        v_TRANS_ADMITWARD varchar2 default '', ---- '转院病区';
                                        v_OUTWARDDATE     varchar2 default '', ---- '出院时间';
                                        v_OUTHOSDEPT      varchar2 default '', ---- '出院科室';
                                        v_OUTHOSWARD      varchar2 default '', ---- '出院病区';
                                        
                                        v_ACTUALDAYS               varchar2 default '', ---- '实际住院天数';
                                        v_PATHOLOGY_DIAGNOSIS_NAME varchar2 default '', ---- '病理诊断名称';
                                        v_PATHOLOGY_OBSERVATION_SN varchar2 default '', ---- '病理检查号 ';
                                        v_ALLERGIC_DRUG            varchar2 default '', ---- '过敏药物';
                                        v_SECTION_DIRECTOR         varchar2 default '', ---- '科主任';
                                        
                                        v_DIRECTOR               varchar2 default '', ---- '主（副主）任医师';
                                        v_VS_EMPLOYEE_CODE       varchar2 default '', ---- '主治医师';
                                        v_RESIDENT_EMPLOYEE_CODE varchar2 default '', ---- '住院医师';
                                        v_REFRESH_EMPLOYEE_CODE  varchar2 default '', ---- '进修医师';
                                        v_DUTY_NURSE             varchar2 default '', ---- '责任护士';
                                        
                                        v_INTERNE                varchar2 default '', ---- '实习医师';
                                        v_CODING_USER            varchar2 default '', ---- '编码员';
                                        v_MEDICAL_QUALITY_ID     varchar2 default '', ---- '病案质量';
                                        v_QUALITY_CONTROL_DOCTOR varchar2 default '', ---- '质控医师';
                                        v_QUALITY_CONTROL_NURSE  varchar2 default '', ---- '质控护士';
                                        
                                        v_QUALITY_CONTROL_DATE varchar2 default '', ---- '质控时间';
                                        v_XAY_SN               varchar2 default '', ---- 'x线检查号';
                                        v_CT_SN                varchar2 default '', ---- 'CT检查号';
                                        v_MRI_SN               varchar2 default '', ---- 'mri检查号';
                                        v_DSA_SN               varchar2 default '', ---- 'Dsa检查号';
                                        
                                        v_BLOODTYPE      varchar2 default '', ---- '血型';
                                        v_RH             varchar2 default '', ---- 'Rh';
                                        v_IS_COMPLETED   varchar2 default '', ---- '完成否 y/n ';
                                        v_COMPLETED_TIME varchar2 default '', ---- '完成时间';
                                        v_VALIDE         varchar2 default '1', ---- '作废否 1/0';
                                        
                                        v_CREATE_USER   varchar2 default '', ---- '创建此记录者';
                                        v_CREATE_TIME   varchar2 default '', ---- '创建时间';
                                        v_MODIFIED_USER varchar2 default '', ---- '修改此记录者';
                                        v_MODIFIED_TIME varchar2 default '', ---- '修改时间';
                                        v_ZYMOSIS       varchar2 default '', ---- '医院传染病';
                                        
                                        v_HURT_TOXICOSIS_ELE_ID   varchar2 default '', ---- '损伤、中毒的外部因素';
                                        v_HURT_TOXICOSIS_ELE_Name varchar2 default '', ---- '损伤、中毒的外部因素';
                                        v_ZYMOSISSTATE            varchar2 default '', ---- '医院传染病状态';
                                        v_PATHOLOGY_DIAGNOSIS_ID  varchar2 default '', ---- '病理诊断编号';
                                        v_MONTHAGE                varchar2 default '', ---- '（年龄不足1周岁的） 年龄(月)';
                                        v_WEIGHT                  varchar2 default '', ---- '新生儿出生体重(克)';
                                        
                                        v_INWEIGHT         varchar2 default '', ---- '新生儿入院体重(克)';
                                        v_INHOSTYPE        varchar2 default '', ---- '入院途径:1.急诊  2.门诊  3.其他医疗机构转入  9.其他';
                                        v_OUTHOSTYPE       varchar2 default '', ---- '离院方式 □ 1.医嘱离院  2.医嘱转院 3.医嘱转社区卫生服务机构/乡镇卫生院 4.非医嘱离院5.死亡9.其他';
                                        v_RECEIVEHOSPITAL  varchar2 default '', ---- '2.医嘱转院，拟接收医疗机构名称：';
                                        v_RECEIVEHOSPITAL2 varchar2 default '', ---- '3.医嘱转社区卫生服务机构/乡镇卫生院，拟接收医疗机构名;
                                        
                                        v_AGAININHOSPITAL       varchar2 default '', ---- '是否有出院31天内再住院计划 □ 1.无  2.有';
                                        v_AGAININHOSPITALREASON varchar2 default '', ---- '出院31天内再住院计划 目的:            ';
                                        v_BEFOREHOSCOMADAY      varchar2 default '', ---- '颅脑损伤患者昏迷时间： 入院前    天';
                                        v_BEFOREHOSCOMAHOUR     varchar2 default '', ---- '颅脑损伤患者昏迷时间： 入院前     小时';
                                        v_BEFOREHOSCOMAMINUTE   varchar2 default '', ---- '颅脑损伤患者昏迷时间： 入院前    分钟';
                                        
                                        v_LATERHOSCOMADAY    varchar2 default '', ---- '颅脑损伤患者昏迷时间： 入院后    天';
                                        v_LATERHOSCOMAHOUR   varchar2 default '', ---- '颅脑损伤患者昏迷时间： 入院后    小时';
                                        v_LATERHOSCOMAMINUTE varchar2 default '', ---- '颅脑损伤患者昏迷时间： 入院后    分钟';
                                        v_CARDNUMBER         varchar2 default '', ---- '健康卡号';
                                        v_ALLERGIC_FLAG      varchar2 default '', ---- '药物过敏1.无 2.有';
                                        
                                        v_AUTOPSY_FLAG     varchar2 default '', ---- '死亡患者尸检 □ 1.是  2.否';
                                        v_CSD_PROVINCEID   varchar2 default '', ---- '出生地 省';
                                        v_CSD_CITYID       varchar2 default '', ---- '出生地 市';
                                        v_CSD_DISTRICTID   varchar2 default '', ---- '出生地 县';
                                        v_CSD_PROVINCENAME varchar2 default '', ---- '出生地 省名称';
                                        
                                        v_CSD_CITYNAME     varchar2 default '', ---- '出生地 市名称';
                                        v_CSD_DISTRICTNAME varchar2 default '', ---- '出生地 县名称';
                                        v_XZZ_PROVINCEID   varchar2 default '', ---- '现住址 省';
                                        v_XZZ_CITYID       varchar2 default '', ---- '现住址 市';
                                        v_XZZ_DISTRICTID   varchar2 default '', ---- '现住址 县';
                                        
                                        v_XZZ_PROVINCENAME varchar2 default '', ---- '现住址 省名称';
                                        v_XZZ_CITYNAME     varchar2 default '', ---- '现住址 市名称';
                                        v_XZZ_DISTRICTNAME varchar2 default '', ---- '现住址 县名称';
                                        v_XZZ_TEL          varchar2 default '', ---- '现住址 电话';
                                        v_XZZ_POST         varchar2 default '', ---- '现住址 邮编';
                                        
                                        v_HKDZ_PROVINCEID   varchar2 default '', ---- '户口地址 省';
                                        v_HKDZ_CITYID       varchar2 default '', ---- '户口地址 市';
                                        v_HKDZ_DISTRICTID   varchar2 default '', ---- '户口地址 县';
                                        v_HKDZ_PROVINCENAME varchar2 default '', ---- '户口地址 省名称';
                                        v_HKDZ_CITYNAME     varchar2 default '', ---- '户口地址 市名称';
                                        
                                        v_HKDZ_DISTRICTNAME varchar2 default '', ---- '户口地址 县名称';
                                        v_HKDZ_POST         varchar2 default '', ---- '户口所在地邮编';
                                        v_JG_PROVINCEID     varchar2 default '', ---- '籍贯 省名称';
                                        v_JG_CITYID         varchar2 default '', ---- '籍贯 市名称';
                                        v_JG_PROVINCENAME   varchar2 default '', ---- '籍贯 省名称';
                                        v_JG_CITYNAME       varchar2 default '', ---- '籍贯 市名称'
                                        v_Age               varchar2 default '',
                                        v_zg_flag           varchar2 default '', -----转归：□ 1.治愈 2.好转 3.未愈 4.死亡 5.其他
                                        v_admitinfo         varchar2 default '', --  v入院病情□ 1.危 2.重 3.一般 4.急 add 二〇一二年六月二十六日 10:14:09
                                        v_CSDADDRESS        varchar2 default '', --出生地具体地址 add by ywk 2012年7月11日 10:13:49
                                        v_JGADDRESS         varchar2 default '', --籍贯地址具体地址 add by ywk 2012年7月11日 10:13:49
                                        v_XZZADDRESS        varchar2 default '', --现住址具体地址 add by ywk 2012年7月11日 10:13:49
                                        v_HKDZADDRESS       varchar2 default '', --户口地址具体地址 add by ywk 2012年7月11日 10:13:49
                                        
                                        v_MenAndInHop           varchar, --门诊和住院
                                        v_InHopAndOutHop        varchar, --入院和出院
                                        v_BeforeOpeAndAfterOper varchar, --术前和术后
                                        v_LinAndBingLi          varchar, --临床与病理
                                        v_InHopThree            varchar, --入院三日内
                                        v_FangAndBingLi         varchar, --放射和病理
                                        o_result                OUT empcurtyp) as
    mynoofclinic varchar2(50);
  
  begin
    if v_IDNO = '不详' then
      mynoofclinic := '';
    else
      mynoofclinic := v_IDNO;
    end if;
    IF v_edittype = '1' THEN
    
      --新增病案首页基本信息
      insert into iem_mainpage_basicinfo_2012
        (IEM_MAINPAGE_NO, ---- '病案首页标识';
         PATNOOFHIS, ---- '病案号';
         NOOFINPAT, ---- '病人首页序号';
         PAYID, ---- '医疗付款方式ID';
         SOCIALCARE, ---- '社保卡号';
         
         INCOUNT, ---- '入院次数';
         NAME, ---- '患者姓名';
         SEXID, ---- '性别';
         BIRTH, ---- '出生';
         MARITAL, ---- '婚姻状况 1.未婚 2.已婚 3.丧偶4.离婚 9.其他';
         
         JOBID, ---- '职业';
         NATIONALITYID, ---- '国籍ID';
         NATIONID, --民族
         IDNO, ---- '身份证号码';
         ORGANIZATION, ---- '工作单位';
         OFFICEPLACE, ---- '工作单位地址';
         
         OFFICETEL, ---- '工作单位电话';
         OFFICEPOST, ---- '工作单位邮编';
         CONTACTPERSON, ---- '联系人姓名';
         RELATIONSHIP, ---- '与联系人关系';
         CONTACTADDRESS, ---- '联系人地址';
         
         CONTACTTEL, ---- '联系人电话';
         ADMITDATE, ---- '入院时间';
         ADMITDEPT, ---- '入院科室';
         ADMITWARD, ---- '入院病区';
         TRANS_DATE, ---- '转院时间';
         
         TRANS_ADMITDEPT, ---- '转院科别';
         TRANS_ADMITWARD, ---- '转院病区';
         OUTWARDDATE, ---- '出院时间';
         OUTHOSDEPT, ---- '出院科室';
         OUTHOSWARD, ---- '出院病区';
         
         ACTUALDAYS, ---- '实际住院天数';
         PATHOLOGY_DIAGNOSIS_NAME, ---- '病理诊断名称';
         PATHOLOGY_OBSERVATION_SN, ---- '病理检查号 ';
         ALLERGIC_DRUG, ---- '过敏药物';
         SECTION_DIRECTOR, ---- '科主任';
         
         DIRECTOR, ---- '主（副主）任医师';
         VS_EMPLOYEE_CODE, ---- '主治医师';
         RESIDENT_EMPLOYEE_CODE, ---- '住院医师';
         REFRESH_EMPLOYEE_CODE, ---- '进修医师';
         DUTY_NURSE, ---- '责任护士';
         
         INTERNE, ---- '实习医师';
         CODING_USER, ---- '编码员';
         MEDICAL_QUALITY_ID, ---- '病案质量';
         QUALITY_CONTROL_DOCTOR, ---- '质控医师';
         QUALITY_CONTROL_NURSE, ---- '质控护士';
         
         QUALITY_CONTROL_DATE, ---- '质控时间';
         XAY_SN, ---- 'x线检查号';
         CT_SN, ---- 'CT检查号';
         MRI_SN, ---- 'mri检查号';
         DSA_SN, ---- 'Dsa检查号';
         
         BLOODTYPE, ---- '血型';
         RH, ---- 'Rh';
         IS_COMPLETED, ---- '完成否 y/n ';
         COMPLETED_TIME, ---- '完成时间';
         VALIDE, ---- '作废否 1/0';
         
         CREATE_USER, ---- '创建此记录者';
         CREATE_TIME, ---- '创建时间';
         MODIFIED_USER, ---- '修改此记录者';
         MODIFIED_TIME, ---- '修改时间';
         ZYMOSIS, ---- '医院传染病';
         
         HURT_TOXICOSIS_ELE_ID, ---- '损伤、中毒的外部因素';
         HURT_TOXICOSIS_ELE_Name,
         ZYMOSISSTATE, ---- '医院传染病状态';
         PATHOLOGY_DIAGNOSIS_ID, ---- '病理诊断编号';
         MONTHAGE, ---- '（年龄不足1周岁的） 年龄(月)';
         WEIGHT, ---- '新生儿出生体重(克)';
         
         INWEIGHT, ---- '新生儿入院体重(克)';
         INHOSTYPE, ---- '入院途径:1.急诊  2.门诊  3.其他医疗机构转入  9.其他';
         OUTHOSTYPE, ---- '离院方式 □ 1.医嘱离院  2.医嘱转院 3.医嘱转社区卫生服务机构/乡镇卫生院 4.非医嘱离院5.死亡9.其他';
         RECEIVEHOSPITAL, ---- '2.医嘱转院，拟接收医疗机构名称：';
         RECEIVEHOSPITAL2, ---- '3.医嘱转社区卫生服务机构/乡镇卫生院，拟接收医疗机构名;
         
         AGAININHOSPITAL, ---- '是否有出院31天内再住院计划 □ 1.无  2.有';
         AGAININHOSPITALREASON, ---- '出院31天内再住院计划 目的:            ';
         BEFOREHOSCOMADAY, ---- '颅脑损伤患者昏迷时间： 入院前    天';
         BEFOREHOSCOMAHOUR, ---- '颅脑损伤患者昏迷时间： 入院前     小时';
         BEFOREHOSCOMAMINUTE, ---- '颅脑损伤患者昏迷时间： 入院前    分钟';
         
         LATERHOSCOMADAY, ---- '颅脑损伤患者昏迷时间： 入院后    天';
         LATERHOSCOMAHOUR, ---- '颅脑损伤患者昏迷时间： 入院后    小时';
         LATERHOSCOMAMINUTE, ---- '颅脑损伤患者昏迷时间： 入院后    分钟';
         CARDNUMBER, ---- '健康卡号';
         ALLERGIC_FLAG, ---- '药物过敏1.无 2.有';
         
         AUTOPSY_FLAG, ---- '死亡患者尸检 □ 1.是  2.否';
         CSD_PROVINCEID, ---- '出生地 省';
         CSD_CITYID, ---- '出生地 市';
         CSD_DISTRICTID, ---- '出生地 县';
         CSD_PROVINCENAME, ---- '出生地 省名称';
         
         CSD_CITYNAME, ---- '出生地 市名称';
         CSD_DISTRICTNAME, ---- '出生地 县名称';
         XZZ_PROVINCEID, ---- '现住址 省';
         XZZ_CITYID, ---- '现住址 市';
         XZZ_DISTRICTID, ---- '现住址 县';
         
         XZZ_PROVINCENAME, ---- '现住址 省名称';
         XZZ_CITYNAME, ---- '现住址 市名称';
         XZZ_DISTRICTNAME, ---- '现住址 县名称';
         XZZ_TEL, ---- '现住址 电话';
         XZZ_POST, ---- '现住址 邮编';
         
         HKDZ_PROVINCEID, ---- '户口地址 省';
         HKDZ_CITYID, ---- '户口地址 市';
         HKDZ_DISTRICTID, ---- '户口地址 县';
         HKDZ_PROVINCENAME, ---- '户口地址 省名称';
         HKDZ_CITYNAME, ---- '户口地址 市名称';
         
         HKDZ_DISTRICTNAME, ---- '户口地址 县名称';
         HKDZ_POST, ---- '户口所在地邮编';
         JG_PROVINCEID, ---- '籍贯 省名称';
         JG_CITYID, ---- '籍贯 市名称';
         JG_PROVINCENAME, ---- '籍贯 省名称';
         JG_CITYNAME,
         age,
         zg_flag,
         admitinfo,
         CSDAddress,
         JGAddress,
         XZZAddress,
         HKAddress,
         MenAndInHop,
         InHopAndOutHop,
         BeforeOpeAndAfterOper,
         LinAndBingLi,
         InHopThree,
         FangAndBingLi) ---- '籍贯 市名称';
      values
        (seq_iem_mainpage_basicinfo_id.NEXTVAL, ---- '病案首页标识';
         v_PATNOOFHIS, ---- '病案号';
         v_NOOFINPAT, ---- '病人首页序号';
         v_PAYID, ---- '医疗付款方式ID';
         v_SOCIALCARE, ---- '社保卡号';
         v_INCOUNT, ---- '入院次数';
         v_NAME, ---- '患者姓名';
         v_SEXID, ---- '性别';
         v_BIRTH, ---- '出生';
         v_MARITAL, ---- '婚姻状况 1.未婚 2.已婚 3.丧偶4.离婚 9.其他';
         v_JOBID, ---- '职业';
         v_NATIONALITYID, ---- '国籍ID';
         v_NATIONID, --民族
         mynoofclinic, ---- '身份证号码';
         v_ORGANIZATION, ---- '工作单位';
         v_OFFICEPLACE, ---- '工作单位地址';
         v_OFFICETEL, ---- '工作单位电话';
         v_OFFICEPOST, ---- '工作单位邮编';
         v_CONTACTPERSON, ---- '联系人姓名';
         v_RELATIONSHIP, ---- '与联系人关系';
         v_CONTACTADDRESS, ---- '联系人地址';
         v_CONTACTTEL, ---- '联系人电话';
         v_ADMITDATE, ---- '入院时间';
         v_ADMITDEPT, ---- '入院科室';
         v_ADMITWARD, ---- '入院病区';
         v_TRANS_DATE, ---- '转院时间';
         v_TRANS_ADMITDEPT, ---- '转院科别';
         v_TRANS_ADMITWARD, ---- '转院病区';
         v_OUTWARDDATE, ---- '出院时间';
         v_OUTHOSDEPT, ---- '出院科室';
         v_OUTHOSWARD, ---- '出院病区';
         v_ACTUALDAYS, ---- '实际住院天数';
         v_PATHOLOGY_DIAGNOSIS_NAME, ---- '病理诊断名称';
         v_PATHOLOGY_OBSERVATION_SN, ---- '病理检查号 ';
         v_ALLERGIC_DRUG, ---- '过敏药物';
         v_SECTION_DIRECTOR, ---- '科主任';
         v_DIRECTOR, ---- '主（副主）任医师';
         v_VS_EMPLOYEE_CODE, ---- '主治医师';
         v_RESIDENT_EMPLOYEE_CODE, ---- '住院医师';
         v_REFRESH_EMPLOYEE_CODE, ---- '进修医师';
         v_DUTY_NURSE, ---- '责任护士';
         v_INTERNE, ---- '实习医师';
         v_CODING_USER, ---- '编码员';
         v_MEDICAL_QUALITY_ID, ---- '病案质量';
         v_QUALITY_CONTROL_DOCTOR, ---- '质控医师';
         v_QUALITY_CONTROL_NURSE, ---- '质控护士';
         v_QUALITY_CONTROL_DATE, ---- '质控时间';
         v_XAY_SN, ---- 'x线检查号';
         v_CT_SN, ---- 'CT检查号';
         v_MRI_SN, ---- 'mri检查号';
         v_DSA_SN, ---- 'Dsa检查号';
         v_BLOODTYPE, ---- '血型';
         v_RH, ---- 'Rh';
         v_IS_COMPLETED, ---- '完成否 y/n ';
         v_COMPLETED_TIME, ---- '完成时间';
         v_VALIDE, ---- '作废否 1/0';
         v_CREATE_USER, ---- '创建此记录者';
         TO_CHAR(SYSDATE, 'yyyy-mm-dd hh24:mi:ss'), ---- '创建时间';
         v_MODIFIED_USER, ---- '修改此记录者';
         v_MODIFIED_TIME, ---- '修改时间';
         v_ZYMOSIS, ---- '医院传染病';
         v_HURT_TOXICOSIS_ELE_ID, ---- '损伤、中毒的外部因素';
         v_HURT_TOXICOSIS_ELE_Name, ---- '损伤、中毒的外部因素';
         v_ZYMOSISSTATE, ---- '医院传染病状态';
         v_PATHOLOGY_DIAGNOSIS_ID, ---- '病理诊断编号';
         v_MONTHAGE, ---- '（年龄不足1周岁的） 年龄(月)';
         v_WEIGHT, ---- '新生儿出生体重(克)';
         v_INWEIGHT, ---- '新生儿入院体重(克)';
         v_INHOSTYPE, ---- '入院途径:1.急诊  2.门诊  3.其他医疗机构转入  9.其他';
         v_OUTHOSTYPE, ---- '离院方式 □ 1.医嘱离院  2.医嘱转院 3.医嘱转社区卫生服务机构/乡镇卫生院 4.非医嘱离院5.死亡9.其他';
         v_RECEIVEHOSPITAL, ---- '2.医嘱转院，拟接收医疗机构名称：';
         v_RECEIVEHOSPITAL2, ---- '3.医嘱转社区卫生服务机构/乡镇卫生院，拟接收医疗机构名;
         v_AGAININHOSPITAL, ---- '是否有出院31天内再住院计划 □ 1.无  2.有';
         v_AGAININHOSPITALREASON, ---- '出院31天内再住院计划 目的:            ';
         v_BEFOREHOSCOMADAY, ---- '颅脑损伤患者昏迷时间： 入院前    天';
         v_BEFOREHOSCOMAHOUR, ---- '颅脑损伤患者昏迷时间： 入院前     小时';
         v_BEFOREHOSCOMAMINUTE, ---- '颅脑损伤患者昏迷时间： 入院前    分钟';
         v_LATERHOSCOMADAY, ---- '颅脑损伤患者昏迷时间： 入院后    天';
         v_LATERHOSCOMAHOUR, ---- '颅脑损伤患者昏迷时间： 入院后    小时';
         v_LATERHOSCOMAMINUTE, ---- '颅脑损伤患者昏迷时间： 入院后    分钟';
         v_CARDNUMBER, ---- '健康卡号';
         v_ALLERGIC_FLAG, ---- '药物过敏1.无 2.有';
         v_AUTOPSY_FLAG, ---- '死亡患者尸检 □ 1.是  2.否';
         v_CSD_PROVINCEID, ---- '出生地 省';
         v_CSD_CITYID, ---- '出生地 市';
         v_CSD_DISTRICTID, ---- '出生地 县';
         v_CSD_PROVINCENAME, ---- '出生地 省名称';
         v_CSD_CITYNAME, ---- '出生地 市名称';
         v_CSD_DISTRICTNAME, ---- '出生地 县名称';
         v_XZZ_PROVINCEID, ---- '现住址 省';
         v_XZZ_CITYID, ---- '现住址 市';
         v_XZZ_DISTRICTID, ---- '现住址 县';
         v_XZZ_PROVINCENAME, ---- '现住址 省名称';
         v_XZZ_CITYNAME, ---- '现住址 市名称';
         v_XZZ_DISTRICTNAME, ---- '现住址 县名称';
         v_XZZ_TEL, ---- '现住址 电话';
         v_XZZ_POST, ---- '现住址 邮编';
         v_HKDZ_PROVINCEID, ---- '户口地址 省';
         v_HKDZ_CITYID, ---- '户口地址 市';
         v_HKDZ_DISTRICTID, ---- '户口地址 县';
         v_HKDZ_PROVINCENAME, ---- '户口地址 省名称';
         v_HKDZ_CITYNAME, ---- '户口地址 市名称';
         v_HKDZ_DISTRICTNAME, ---- '户口地址 县名称';
         v_HKDZ_POST, ---- '户口所在地邮编';
         v_JG_PROVINCEID, ---- '籍贯 省名称';
         v_JG_CITYID, ---- '籍贯 市名称';
         v_JG_PROVINCENAME, ---- '籍贯 省名称';
         v_JG_CITYNAME,
         v_Age,
         v_zg_flag,
         v_admitinfo, --入院病情
         v_CSDADDRESS, --出生地
         v_JGADDRESS, --籍贯地址
         v_XZZADDRESS, --现住址
         v_HKDZADDRESS, --户口住址
         v_MenAndInHop, --门诊和住院
         v_InHopAndOutHop, --入院和出院
         v_BeforeOpeAndAfterOper, --术前和术后
         v_LinAndBingLi, --临床与病理
         v_InHopThree, --入院三日内
         v_FangAndBingLi);
    
      open o_result for
        select seq_iem_mainpage_basicinfo_id.currval from dual;
      ---修改病案首页基本信息
    ELSIF v_edittype = '2' THEN
    
      update iem_mainpage_basicinfo_2012
         set PATNOOFHIS               = v_PATNOOFHIS, --病案号
             NOOFINPAT                = v_NOOFINPAT, --病人首页序号
             PAYID                    = v_PAYID, --医疗付款方式ID
             SOCIALCARE               = v_SOCIALCARE, --社保卡号
             INCOUNT                  = v_INCOUNT, --入院次数
             NAME                     = v_NAME, --患者姓名
             SEXID                    = v_SEXID, --性别
             BIRTH                    = v_BIRTH, --出生
             MARITAL                  = v_MARITAL, --婚姻状况 1.未婚 2.已婚 3.丧偶4.离婚 9.其他
             JOBID                    = v_JOBID, --职业
             NATIONALITYID            = v_NATIONALITYID, --国籍ID
             NATIONID                 = v_NATIONID, --民族
             IDNO                     = mynoofclinic, --身份证号码
             ORGANIZATION             = v_ORGANIZATION, --工作单位
             OFFICEPLACE              = v_OFFICEPLACE, --工作单位地址
             OFFICETEL                = v_OFFICETEL, --工作单位电话
             OFFICEPOST               = v_OFFICEPOST, --工作单位邮编
             CONTACTPERSON            = v_CONTACTPERSON, --联系人姓名
             RELATIONSHIP             = v_RELATIONSHIP, --与联系人关系
             CONTACTADDRESS           = v_CONTACTADDRESS, --联系人地址
             CONTACTTEL               = v_CONTACTTEL, --联系人电话
             ADMITDATE                = v_ADMITDATE, --入院时间
             ADMITDEPT                = v_ADMITDEPT, --入院科室
             ADMITWARD                = v_ADMITWARD, --入院病区
             TRANS_DATE               = v_TRANS_DATE, --转院时间
             TRANS_ADMITDEPT          = v_TRANS_ADMITDEPT, --转院科别
             TRANS_ADMITWARD          = v_TRANS_ADMITWARD, --转院病区
             OUTWARDDATE              = v_OUTWARDDATE, --出院时间
             OUTHOSDEPT               = v_OUTHOSDEPT, --出院科室
             OUTHOSWARD               = v_OUTHOSWARD, --出院病区
             ACTUALDAYS               = v_ACTUALDAYS, --实际住院天数
             PATHOLOGY_DIAGNOSIS_NAME = v_PATHOLOGY_DIAGNOSIS_NAME, --病理诊断名称
             PATHOLOGY_OBSERVATION_SN = v_PATHOLOGY_OBSERVATION_SN, --病理检查号
             ALLERGIC_DRUG            = v_ALLERGIC_DRUG, --过敏药物
             SECTION_DIRECTOR         = v_SECTION_DIRECTOR, --科主任
             DIRECTOR                 = v_DIRECTOR, --主（副主）任医师
             VS_EMPLOYEE_CODE         = v_VS_EMPLOYEE_CODE, --主治医师
             RESIDENT_EMPLOYEE_CODE   = v_RESIDENT_EMPLOYEE_CODE, --住院医师
             REFRESH_EMPLOYEE_CODE    = v_REFRESH_EMPLOYEE_CODE, --进修医师
             DUTY_NURSE               = v_DUTY_NURSE, --责任护士
             INTERNE                  = v_INTERNE, --实习医师
             CODING_USER              = v_CODING_USER, --编码员
             MEDICAL_QUALITY_ID       = v_MEDICAL_QUALITY_ID, --病案质量
             QUALITY_CONTROL_DOCTOR   = v_QUALITY_CONTROL_DOCTOR, --质控医师
             QUALITY_CONTROL_NURSE    = v_QUALITY_CONTROL_NURSE, --质控护士
             QUALITY_CONTROL_DATE     = v_QUALITY_CONTROL_DATE, --质控时间
             XAY_SN                   = v_XAY_SN, --x线检查号
             CT_SN                    = v_CT_SN, --CT检查号
             MRI_SN                   = v_MRI_SN, --mri检查号
             DSA_SN                   = v_DSA_SN, --Dsa检查号
             BLOODTYPE                = v_BLOODTYPE, --血型
             RH                       = v_RH, --Rh
             IS_COMPLETED             = v_IS_COMPLETED, --完成否 y/n
             COMPLETED_TIME           = v_COMPLETED_TIME, --完成时间
             VALIDE                   = v_VALIDE, --作废否 1/0
             /* CREATE_USER              = v_CREATE_USER, --创建此记录者
             CREATE_TIME              = v_CREATE_TIME, --创建时间*/
             MODIFIED_USER           = v_MODIFIED_USER, --修改此记录者
             MODIFIED_TIME           = TO_CHAR(SYSDATE,
                                               'yyyy-mm-dd hh24:mi:ss'), --修改时间
             ZYMOSIS                 = v_ZYMOSIS, --医院传染病
             HURT_TOXICOSIS_ELE_ID   = v_HURT_TOXICOSIS_ELE_ID, --损伤、中毒的外部因素
             HURT_TOXICOSIS_ELE_Name = v_HURT_TOXICOSIS_ELE_Name, --损伤、中毒的外部因素
             ZYMOSISSTATE            = v_ZYMOSISSTATE, --医院传染病状态
             PATHOLOGY_DIAGNOSIS_ID  = v_PATHOLOGY_DIAGNOSIS_ID, --病理诊断编号
             MONTHAGE                = v_MONTHAGE, --（年龄不足1周岁的） 年龄(月)
             WEIGHT                  = v_WEIGHT, --新生儿出生体重(克)
             INWEIGHT                = v_INWEIGHT, --新生儿入院体重(克)
             INHOSTYPE               = v_INHOSTYPE, --入院途径:1.急诊  2.门诊  3.其他医疗机构转入  9.其他
             OUTHOSTYPE              = v_OUTHOSTYPE, --离院方式 □ 1.医嘱离院  2.医嘱转院 3.医嘱转社区卫生服务机构/乡镇卫生院 4.非医嘱离院5.死亡9.其他
             RECEIVEHOSPITAL         = v_RECEIVEHOSPITAL, --2.医嘱转院，拟接收医疗机构名称：
             RECEIVEHOSPITAL2        = v_RECEIVEHOSPITAL2, --3.医嘱转社区卫生服务机构/乡镇卫生院，拟接收医疗机构名称：
             AGAININHOSPITAL         = v_AGAININHOSPITAL, --是否有出院31天内再住院计划 □ 1.无  2.有
             AGAININHOSPITALREASON   = v_AGAININHOSPITALREASON, --出院31天内再住院计划 目的:
             BEFOREHOSCOMADAY        = v_BEFOREHOSCOMADAY, --颅脑损伤患者昏迷时间： 入院前    天
             BEFOREHOSCOMAHOUR       = v_BEFOREHOSCOMAHOUR, --颅脑损伤患者昏迷时间： 入院前     小时
             BEFOREHOSCOMAMINUTE     = v_BEFOREHOSCOMAMINUTE, --颅脑损伤患者昏迷时间： 入院前    分钟
             LATERHOSCOMADAY         = v_LATERHOSCOMADAY, --颅脑损伤患者昏迷时间： 入院后    天
             LATERHOSCOMAHOUR        = v_LATERHOSCOMAHOUR, --颅脑损伤患者昏迷时间： 入院后    小时
             LATERHOSCOMAMINUTE      = v_LATERHOSCOMAMINUTE, --颅脑损伤患者昏迷时间： 入院后    分钟
             CARDNUMBER              = v_CARDNUMBER, --健康卡号
             ALLERGIC_FLAG           = v_ALLERGIC_FLAG, --药物过敏1.无 2.有
             AUTOPSY_FLAG            = v_AUTOPSY_FLAG, --死亡患者尸检 □ 1.是  2.否
             CSD_PROVINCEID          = v_CSD_PROVINCEID, --出生地 省
             CSD_CITYID              = v_CSD_CITYID, --出生地 市
             CSD_DISTRICTID          = v_CSD_DISTRICTID, --出生地 县
             CSD_PROVINCENAME        = v_CSD_PROVINCENAME, --出生地 省名称
             CSD_CITYNAME            = v_CSD_CITYNAME, --出生地 市名称
             CSD_DISTRICTNAME        = v_CSD_DISTRICTNAME, --出生地 县名称
             XZZ_PROVINCEID          = v_XZZ_PROVINCEID, --现住址 省
             XZZ_CITYID              = v_XZZ_CITYID, --现住址 市
             XZZ_DISTRICTID          = v_XZZ_DISTRICTID, --现住址 县
             XZZ_PROVINCENAME        = v_XZZ_PROVINCENAME, --现住址 省名称
             XZZ_CITYNAME            = v_XZZ_CITYNAME, --现住址 市名称
             XZZ_DISTRICTNAME        = v_XZZ_DISTRICTNAME, --现住址 县名称
             XZZ_TEL                 = v_XZZ_TEL, --现住址 电话
             XZZ_POST                = v_XZZ_POST, --现住址 邮编
             HKDZ_PROVINCEID         = v_HKDZ_PROVINCEID, --户口地址 省
             HKDZ_CITYID             = v_HKDZ_CITYID, --户口地址 市
             HKDZ_DISTRICTID         = v_HKDZ_DISTRICTID, --户口地址 县
             HKDZ_PROVINCENAME       = v_HKDZ_PROVINCENAME, --户口地址 省名称
             HKDZ_CITYNAME           = v_HKDZ_CITYNAME, --户口地址 市名称
             HKDZ_DISTRICTNAME       = v_HKDZ_DISTRICTNAME, --户口地址 县名称
             HKDZ_POST               = v_HKDZ_POST, --户口所在地邮编
             JG_PROVINCEID           = v_JG_PROVINCEID, --籍贯 省名称
             JG_CITYID               = v_JG_CITYID, --籍贯 市名称
             JG_PROVINCENAME         = v_JG_PROVINCENAME, --籍贯 省名称
             JG_CITYNAME             = v_JG_CITYNAME, --籍贯 市名称\
             age                     = v_Age,
             zg_flag                 = v_zg_flag, --addby ywk 2012年5月14日 14:53:16
             admitinfo               = v_admitinfo, ---add by ywk 2012年6月26日 10:15:30
             CSDADDRESS              = v_CSDADDRESS, --出生地 add by ywk 2012年7月11日 10:18:22
             JGADDRESS               = v_JGADDRESS, --籍贯地址
             XZZADDRESS              = v_XZZADDRESS, --现住址
             HKAddress               = v_HKDZADDRESS, --户口住址
             MenAndInHop             = v_MenAndInHop,
             InHopAndOutHop          = v_InHopAndOutHop,
             BeforeOpeAndAfterOper   = v_BeforeOpeAndAfterOper,
             LinAndBingLi            = v_LinAndBingLi,
             InHopThree              = v_InHopThree,
             FangAndBingLi           = v_FangAndBingLi
       where IEM_MAINPAGE_NO = v_IEM_MAINPAGE_NO;
      open o_result for
        select v_IEM_MAINPAGE_NO from dual;
    end if;
  
  END;

  /*****查询病案首页信息****************************************************************************/
  PROCEDURE usp_getieminfo_2012(v_NoOfInpat INT,
                                o_result    OUT empcurtyp,
                                o_result1   OUT empcurtyp,
                                o_result2   OUT empcurtyp,
                                o_result3   OUT empcurtyp,
                                o_result4   OUT empcurtyp) AS
    /**********
     版本号  1.0.0.0.0
     创建时间
     作者
     版权
     描述  获取病人病案首页各模块信息
     功能说明
     输入参数
      v_NoOfInpat varchar(40)--首页序号
     输出参数
    **********/
    v_infono NUMERIC;
  BEGIN
    OPEN o_result FOR
      SELECT '' FROM DUAL;
  
    OPEN o_result1 FOR
      SELECT '' FROM DUAL;
  
    OPEN o_result2 FOR
      SELECT '' FROM DUAL;
  
    OPEN o_result3 FOR
      SELECT '' FROM DUAL;

  
    SELECT MAX(imb.iem_mainpage_no)
      INTO v_infono
      FROM iem_mainpage_basicinfo_2012 imb
     WHERE imb.noofinpat = v_noofinpat
       AND imb.valide = 1;
  
    /* --数据顺序不可变，与程序里相关
        --基本信息
        OPEN o_result FOR
          SELECT iem.iem_mainpage_no,
                 iem.noofinpat,
                 iem.socialcare,
                 iem.payid,
                 pay.name payName,
    
                 iem.incount,
                 iem.patnoofhis,
                 iem.name,
                 iem.sexid,
                 iem.birth,
    
                 iem.marital,
                 iem.jobid,
                 job.name jobName,
                 iem.csd_provinceid,
                 iem.csd_cityid,
    
                 iem.csd_districtid,
                 iem.csd_provincename,
                 iem.csd_cityname,
                 iem.csd_districtname,
                 iem.xzz_provinceid,
    
                 iem.xzz_cityid,
                 iem.xzz_districtid,
                 iem.xzz_provincename,
                 iem.xzz_cityname,
                 iem.xzz_districtname,
    
                 iem.xzz_tel,
                 iem.xzz_post,
                 iem.hkdz_provinceid,
                 iem.hkdz_cityid,
                 iem.hkdz_districtid,
    
                 iem.hkdz_provincename,
                 iem.hkdz_cityname,
                 iem.hkdz_districtname,
                 iem.hkdz_post,
                 iem.jg_provinceid,
    
                 iem.jg_cityid,
                 iem.jg_provincename,
                 iem.jg_cityname,
                 iem.nationid,
                 nation.name NationName,
    
                 iem.nationalityid,
                 nationality.name nationalityName,
                 iem.idno,
                 iem.organization,
                 iem.officeplace,
    
                 iem.officetel,
                 iem.officepost,
                 iem.contactperson,
                 iem.relationship,
                 relationship.name relationshipName,
    
                 iem.contactaddress ContactAddress,
                 iem.contacttel ContactTEL,
                 iem.admitdate,
                 iem.admitdept,
                 AdmitDept.name AdmitDeptName,
    
                 iem.admitward,
                 admitward.name admitwardName,
                 iem.trans_admitdept,
                 trans_admitdept.name trans_admitdeptName,
                 iem.outwarddate,
    
                 iem.outhosdept,
                 outhosdept.name outhosdeptName,
                 iem.outhosward,
                 outhosward.name outhoswardName,
                 iem.actualdays,
    
                 iem.is_completed,
                 iem.completed_time,
                 iem.valide,
                 iem.create_user,
                 iem.create_time,
    
                 iem.modified_user,
                 iem.modified_time,
                 iem.autopsy_flag,
    
                 --2012国家卫生部表中病案首页新增内容
                 iem.monthage,
                 iem.weight,
                 iem.inweight,
                 iem.inhostype,
                 iem.outhostype,
    
                 iem.receivehospital,
                 iem.receivehospital2,
                 iem.againinhospital,
                 iem.againinhospitalreason,
                 iem.beforehoscomaday,
    
                 iem.beforehoscomahour,
                 iem.beforehoscomaminute,
                 iem.laterhoscomaday,
                 iem.laterhoscomahour,
                 iem.laterhoscomaminute,
                 iem.cardnumber,
    
                 ---诊断实体
                 iem.pathology_diagnosis_id,
                 iem.pathology_diagnosis_name,
                 iem.pathology_observation_sn,
                 iem.hurt_toxicosis_ele_id,
                 iem.hurt_toxicosis_ele_name,
    
                 iem.allergic_flag,
                 iem.allergic_drug,
                 iem.bloodtype,
                 iem.rh,
                 iem.section_director,
    
                 section_director.name section_directorName,
                 iem.director,
                 director.name directorName,
                 iem.vs_employee_code vs_employeeID,
                 vs_employee.name vs_employeeName,
    
                 iem.resident_employee_code resident_employeeID,
                 resident_employee.name     resident_employeeName,
                 iem.refresh_employee_code  refresh_employeeID,
                 refresh_employee.name      refresh_employeeName,
                 iem.duty_nurse             ,
    
                 duty_nurse.name Duty_NurseName,
                 iem.interne,
                 interne.name interneName,
                 iem.coding_user,
                 coding_user.name coding_userName,
    
                 iem.medical_quality_id,
                 iem.quality_control_doctor,
                 quality_control_doctor.name quality_control_doctorName,
                 iem.quality_control_nurse,
                 quality_control_nurse.name quality_control_nurseName,
    
                 iem.quality_control_date,
                 iem.age,
                 iem.zg_flag
    
            FROM iem_mainpage_basicinfo_2012 iem
            left join dictionary_detail pay on pay.detailid = iem.payid
                                           and pay.categoryid = '1'
            left join dictionary_detail job on job.detailid = iem.jobid
                                           and job.categoryid = '41'
            left join dictionary_detail nation on nation.detailid =
                                                  iem.nationid
                                              and nation.categoryid = '42'
            left join dictionary_detail nationality on nationality.detailid =
                                                       iem.nationalityid
                                                   and nationality.categoryid = '43'
            left join dictionary_detail relationship on relationship.detailid =
                                                        iem.relationship
                                                    and relationship.categoryid = '44'
            left join department AdmitDept on AdmitDept.id = iem.admitdept
            left join ward admitward on admitward.id = iem.admitward
            left join department trans_admitdept on trans_admitdept.id =
                                                    iem.trans_admitdept
            left join department outhosdept on outhosdept.id = iem.outhosdept
            left join ward outhosward on outhosward.id = iem.outhosward
            left join diagnosis zymosis on zymosis.markid = iem.zymosis
            left join users section_director on section_director.id =
                                                iem.section_director
            left join users director on director.id = iem.director
            left join users vs_employee on vs_employee.id =
                                           iem.vs_employee_code
            left join users resident_employee on resident_employee.id =
                                                 iem.resident_employee_code
            left join users refresh_employee on refresh_employee.id =
                                                iem.refresh_employee_code
    
            left join users interne on interne.id = iem.interne
            left join users duty_nurse on duty_nurse.id = iem.duty_nurse
            left join users coding_user on coding_user.id = iem.coding_user
            left join users quality_control_doctor on quality_control_doctor.id =
                                                      iem.quality_control_doctor
            left join users quality_control_nurse on quality_control_nurse.id =
                                                     iem.quality_control_nurse
           WHERE iem.iem_mainpage_no = v_infono
             AND iem.valide = 1;
    */
  
    --数据顺序不可变，与程序里相关
    --基本信息
    OPEN o_result FOR
      SELECT iem.iem_mainpage_no,
             iem.noofinpat,
             iem.socialcare,
             myinp.payid,
             pay.name payName,
             myinp.noofrecord,
             iem.incount,
             iem.patnoofhis,
             myinp.name,
             myinp.sexid,
             myinp.birth,
             myinp.nativetel, ---现住址电话edit 2012年6月25日 08:46:26
             myinp.marital,
             myinp.jobid,
             job.name    jobName,
             iem.zg_flag,
             myinp.provinceid csd_provinceid,
             myinp.countyid csd_cityid,
             myinp.districtid csd_districtid,    
             csdpro.provincename csd_provincename,
             csdcity.cityname    csd_cityname,
             csddis.districtname csd_districtname,
             myinp.xzzproviceid xzz_provinceid,
             myinp.xzzcityid     xzz_cityid,
             myinp.xzzdistrictid xzz_districtid,
             myinp.xzzdetailaddress xzz_detailaddress,--add by cyq 2012-12-27
             xzzpro.provincename xzz_provincename,
             xzzcity.cityname    xzz_cityname,
             xzzdis.districtname xzz_districtname,
             myinp.xzztel         xzz_tel,
             myinp.xzzpost        xzz_post,
             myinp.hkdzproviceid  hkdz_provinceid,
             myinp.hkzdcityid     hkdz_cityid,
             myinp.hkzddistrictid hkdz_districtid,
             hkzzpro.provincename hkdz_provincename,
             hkzzcity.cityname    hkdz_cityname,
             hkzzdis.districtname hkdz_districtname,
             myinp.hkdzdetailaddress hkdz_detailaddress,--add by cyq 2012-12-27
             myinp.nativepost hkdz_post,
             myinp.nativeplace_p jg_provinceid,
             myinp.nativeplace_c jg_cityid,
             jgpro.provincename  jg_provincename,
             jgcity.cityname     jg_cityname,
             myinp.nationid,
             nation.name    NationName,
             myinp.nationalityid,
             nationality.name    nationalityName,
             myinp.idno,
             iem.organization,
             myinp.officeplace,
             myinp.officetel,
             myinp.officepost,
             myinp.contactperson,
             myinp.relationship,
             relationship.name  relationshipName,
             myinp.contactaddress ContactAddress,
             myinp.contacttel ContactTEL,
             myinp.admitdate,
             myinp.inwarddate,--入区时间add by ywk 2013年1月17日13:54:50
             myinp.admitdept,
             AdmitDept.name  AdmitDeptName,
             iem.admitinfo, --新增的入院病情 2012年6月26日 09:42:30
             myinp.admitward,
             admitward.name       admitwardName,
             iem.trans_admitdept,
             trans_admitdept.name trans_admitdeptName,
             myinp.outhosdate,
             myinp.outhosdept,
             outhosdept.name  outhosdeptName,
             myinp.outhosward,
             outhosward.name  outhoswardName,
             myinp.totaldays actualdays,
             iem.is_completed,
             iem.completed_time,
             iem.valide,
             iem.create_user,
             iem.create_time,
             iem.modified_user,
             iem.modified_time,
             iem.autopsy_flag,
             --2012国家卫生部表中病案首页新增内容
             iem.monthage,
             iem.weight,
             iem.inweight,
             iem.inhostype,
             iem.outhostype,
             iem.receivehospital,
             iem.receivehospital2,
             iem.againinhospital,
             iem.againinhospitalreason,
             iem.beforehoscomaday,
             iem.beforehoscomahour,
             iem.beforehoscomaminute,
             iem.laterhoscomaday,
             iem.laterhoscomahour,
             iem.laterhoscomaminute,
             iem.cardnumber,
             ---诊断实体
             iem.pathology_diagnosis_id,
             iem.pathology_diagnosis_name,
             iem.pathology_observation_sn,
             iem.hurt_toxicosis_ele_id,
             iem.hurt_toxicosis_ele_name,
             iem.allergic_flag,
             iem.allergic_drug,
             iem.bloodtype,
             iem.rh,
             iem.section_director,
             section_director.name section_directorName,
             iem.director,
             director.name         directorName,
             iem.vs_employee_code  vs_employeeID,
             vs_employee.name      vs_employeeName,
             iem.resident_employee_code resident_employeeID,
             resident_employee.name     resident_employeeName,
             iem.refresh_employee_code  refresh_employeeID,
             refresh_employee.name      refresh_employeeName,
             iem.duty_nurse,
             duty_nurse.name  Duty_NurseName,
             iem.interne,
             interne.name     interneName,
             iem.coding_user,
             coding_user.name coding_userName,
             iem.medical_quality_id,
             iem.quality_control_doctor,
             quality_control_doctor.name quality_control_doctorName,
             iem.quality_control_nurse,
             quality_control_nurse.name  quality_control_nurseName,
             iem.quality_control_date,
             myinp.agestr  age,
             myinp.csdaddress, --出生地地址 ywk 
             myinp.jgaddress, --籍贯地址
             myinp.xzzaddress, --现住址
             myinp.hkaddress, --户口住址
             iem.menandinhop, --门诊
             iem.inhopandouthop,
             iem.beforeopeandafteroper,
             iem.linandbingli,
             iem.inhopthree,
             iem.fangandbingli,
             myinp.isbaby, --是否婴儿的标志 add by ywk 2012年7月30日 13:58:41
             myinp.mother --母亲的首页序号 Noofinpat    
        FROM iem_mainpage_basicinfo_2012 iem
      -- edit  by ywk （对于首页表里有的，病人表里也有的数据，在病案首页显示的时候取病人表里的数据）
        left join inpatient myinp
          on myinp.noofinpat = iem.noofinpat
        left join dictionary_detail pay
          on pay.detailid = myinp.payid
         and pay.categoryid = '1'
        left join dictionary_detail job
          on job.detailid = myinp.jobid
         and job.categoryid = '41'
        left join dictionary_detail nation
          on nation.detailid = myinp.nationid
         and nation.categoryid = '42'
        left join dictionary_detail nationality
          on nationality.detailid = myinp.nationalityid
         and nationality.categoryid = '43'
        left join dictionary_detail relationship
          on relationship.detailid = myinp.relationship
         and relationship.categoryid = '44'
        left join department AdmitDept
          on AdmitDept.id = myinp.admitdept
        left join ward admitward
          on admitward.id = myinp.admitward
        left join department trans_admitdept
          on trans_admitdept.id = iem.trans_admitdept
        left join department outhosdept
          on outhosdept.id = myinp.outhosdept
        left join ward outhosward
          on outhosward.id = myinp.outhosward
        left join diagnosis zymosis
          on zymosis.markid = iem.zymosis
        left join users section_director
          on section_director.id = iem.section_director
        left join users director
          on director.id = iem.director
        left join users vs_employee
          on vs_employee.id = iem.vs_employee_code
        left join users resident_employee
          on resident_employee.id = iem.resident_employee_code
        left join users refresh_employee
          on refresh_employee.id = iem.refresh_employee_code
      
        left join users interne
          on interne.id = iem.interne
        left join users duty_nurse
          on duty_nurse.id = iem.duty_nurse
        left join users coding_user
          on coding_user.id = iem.coding_user
        left join users quality_control_doctor
          on quality_control_doctor.id = iem.quality_control_doctor
        left join users quality_control_nurse
          on quality_control_nurse.id = iem.quality_control_nurse
      
      ---省，市，县的名称从Inpatient表里去除，取名称，进行链表查询 add by ywk 2012年5月17日10:58:03
        left join s_province csdpro
          on myinp.provinceid = csdpro.provinceid --出生地的省
        left join s_city csdcity
          on myinp.countyid = csdcity.cityid --出生地的市
        left join s_district csddis
          on myinp.districtid = csddis.districtid --出生地的县
      
        left join s_province jgpro
          on myinp.nativeplace_p = jgpro.provinceid --籍贯的省
        left join s_city jgcity
          on myinp.nativeplace_c = jgcity.cityid --籍贯的市
      
        left join s_province xzzpro
          on myinp.xzzproviceid = xzzpro.provinceid --现住址的省
        left join s_city xzzcity
          on myinp.xzzcityid = xzzcity.cityid --现住址的市
        left join s_district xzzdis
          on myinp.xzzdistrictid = xzzdis.districtid --现住址的县
      
        left join s_province hkzzpro
          on myinp.hkdzproviceid = hkzzpro.provinceid --户口住址的省
        left join s_city hkzzcity
          on myinp.hkzdcityid = hkzzcity.cityid --户口住址的市
        left join s_district hkzzdis
          on myinp.hkzddistrictid = hkzzdis.districtid --户口住址的县
      
       WHERE iem.iem_mainpage_no = v_infono
         AND iem.valide = 1;
  
    --诊断
    OPEN o_result1 FOR
      SELECT diag.diagnosis_name,
             diag.morphologyicd,--形态学诊断编码
             diag.morphologyname,--形态学诊断名称  add jxh
             diag.status_id,
             (case
               when diag.status_id = '1' then
                '有'
               when diag.status_id = '2' then
                '临床未确定'
               when diag.status_id = '3' then
                '情况不明'
               when diag.status_id = '4' then
                '无'
               else
                ''
             end) Status_Name, --入院病情
             diag.iem_mainpage_diagnosis_no,diag.iem_mainpage_no,--add by cyq 2012-12-25
             diag.diagnosis_code,
             diag.diagnosis_type_id,
             diag.order_value,
             diag.menandinhop,
             /*( case when diag.menandinhop='0' then '未作'
               when  diag.menandinhop='1' then '确诊'
                 when diag.menandinhop='2' then '误诊'
                when   diag.menandinhop='3' then '不肯定'
                   else '' end
             )*/
             md.name             as menandinhopname,
             diag.inhopandouthop,
             /* (
             case when diag.inhopandouthop='0' then '未作'
               when  diag.inhopandouthop='1' then '确诊'
             when     diag.inhopandouthop='2' then '误诊'
                when   diag.inhopandouthop='3' then '不肯定'
                   else '' end
             )*/
             ind.name                   as inhopandouthopname,
             diag.beforeopeandafteroper,
             /* (
             case when diag.beforeopeandafteroper='0' then '未作'
               when  diag.beforeopeandafteroper='1' then '确诊'
              when    diag.beforeopeandafteroper='2' then '误诊'
               when    diag.beforeopeandafteroper='3' then '不肯定'
                   else '' end
             )*/
             befd.name         as beforeopeandafteropername,
             diag.linandbingli,
             /*  (
             case when diag.linandbingli='0' then '未作'
               when  diag.linandbingli='1' then '确诊'
               when   diag.linandbingli='2' then '误诊'
               when    diag.linandbingli='3' then '不肯定'
                   else '' end
             )*/
             lid.name        as linandbingliname,
             diag.inhopthree,
             /*    (
             case when diag.inhopthree='0' then '未作'
               when  diag.inhopthree='1' then '确诊'
               when   diag.inhopthree='2' then '误诊'
                when   diag.inhopthree='3' then '不肯定'
                   else '' end
             )*/
             intd.name          as inhopthreename,
             diag.fangandbingli,
             /* (
             case when diag.fangandbingli='0' then '未作'
               when  diag.fangandbingli='1' then '确诊'
                when  diag.fangandbingli='2' then '误诊'
               when    diag.fangandbingli='3' then '不肯定'
                   else '' end
             )*/
             fad.name       as fangandbingliname,
             addinfo.name   as admitinfoname,
             diag.admitinfo --新增的入院病情 add by ywk  2012年7月26日 15:12:42
        FROM iem_mainpage_diagnosis_2012 diag
        left join dictionary_detail addinfo
          on addinfo.detailid = diag.admitinfo
         and addinfo.categoryid = '5'
        left join dictionary_detail md
          on diag.menandinhop = md.detailid
         and md.categoryid = 'dg'
        left join dictionary_detail ind
          on diag.inhopandouthop = ind.detailid
         and ind.categoryid = 'dg'
        left join dictionary_detail lid
          on diag.linandbingli = lid.detailid
         and lid.categoryid = 'dg'
        left join dictionary_detail befd
          on diag.beforeopeandafteroper = befd.detailid
         and befd.categoryid = 'dg'
        left join dictionary_detail intd
          on diag.inhopthree = intd.detailid
         and intd.categoryid = 'dg'
        left join dictionary_detail fad
          on diag.fangandbingli = fad.detailid
         and fad.categoryid = 'dg'
       WHERE iem_mainpage_no = v_infono
         AND valide = 1
       ORDER BY order_value;
  
    --手术
    /*    OPEN o_result2 FOR
    SELECT *
      FROM iem_mainpage_operation
     WHERE iem_mainpage_no = v_infono
       AND valide = 1;*/
  
    OPEN o_result2 FOR
    
      SELECT iem.iem_mainpage_operation_no,
             iem.iem_mainpage_no,
             iem.operation_code,
             iem.operation_date,
             iem.operation_name,
             u1.name                       execute_user1_Name,
             iem.execute_user1,
             u2.name                       execute_user2_Name,
             iem.execute_user2,
             u3.name                       execute_user3_Name,
             iem.execute_user3,
             --dic.name anaesthesia_type_Name,
             ab.name anaesthesia_type_Name,
             iem.anaesthesia_type_id,
             (case
               when iem.close_level = '1' then
                'I/甲'
               when iem.close_level = '2' then
                'II/甲'
               when iem.close_level = '3' then
                'III/甲'
               when iem.close_level = '4' then
                'I/乙'
               when iem.close_level = '5' then
                'II/乙'
               when iem.close_level = '6' then
                'III/乙'
               when iem.close_level = '7' then
                'I/丙'
               when iem.close_level = '8' then
                'II/丙'
               when iem.close_level = '9' then
                'III/丙'
               else
                ''
             end) close_level_Name,
             iem.operation_level,
             (case
               when iem.operation_level = '1' then --operation_level
                '一级手术'
               when iem.operation_level = '2' then
                '二级手术'
               when iem.operation_level = '3' then
                '三级手术'
               when iem.operation_level = '4' then
                '四级手术'
               else
                ''
             end) operation_level_Name,
             iem.close_level,
             ua.name anaesthesia_user_Name,
             iem.anaesthesia_user,
             iem.valide,
             iem.create_user,
             iem.create_time,
             iem.cancel_user,
             iem.cancel_time,
             iem.ischoosedate,
             iem.isclearope,
             iem.isganran,
             (case
               when iem.ischoosedate = '1' then
                '是'
               when iem.ischoosedate = '2' then
                '否'
               when iem.ischoosedate = '0' then
                '未知'
               else
                ''
             end) ischoosedatename,
             (case
               when iem.isclearope = '1' then
                '是'
               when iem.isclearope = '2' then
                '否'
               when iem.isclearope = '0' then
                '未知'
               else
                ''
             end) isclearopename,
             (case
               when iem.isganran = '1' then
                '是'
               when iem.isganran = '2' then
                '否'
               when iem.isganran = '0' then
                '未知'
               else
                ''
             end) isganranname
             ,iem.anesthesia_level,iem.opercomplication_code
        FROM iem_mainpage_operation_2012 iem
        left join users u1
          on iem.execute_user1 = u1.id
         and u1.valid = 1
        left join users u2
          on iem.execute_user2 = u2.id
         and u2.valid = 1
        left join users u3
          on iem.execute_user3 = u3.id
         and u3.valid = 1
        left join users ua
          on iem.anaesthesia_user = ua.id
         and ua.valid = 1
      /* left join dictionary_detail dic on iem.anaesthesia_type_id =
          dic.detailid
      and dic.categoryid = '30'*/
      --edit by ywk 2012年4月18日10:24:09
        left join anesthesia ab
          on iem.anaesthesia_type_id = ab.id
       WHERE valide = 1
         and iem.iem_mainpage_no = v_infono;
  
    --产妇婴儿信息
    OPEN o_result3 FOR
      SELECT *
        FROM IEM_MAINPAGE_OBSTETRICSBABY
       WHERE iem_mainpage_no = v_infono
         AND valide = 1;
         
        ---取得病案首页的费用信息 --add by ywk   2012年10月16日 20:23:51
    OPEN o_result4 FOR
    select  *from Iem_MainPage_FeeInfo where iem_mainpage_no = v_infono
         AND valid = 1;
  END;

  /*********************************************************************************/
  PROCEDURE usp_edit_iem_mainpage_oper2012(v_iem_mainpage_no     NUMERIC,
                                           v_operation_code      VARCHAR,
                                           v_operation_date      VARCHAR,
                                           v_operation_name      VARCHAR,
                                           v_execute_user1       VARCHAR,
                                           v_execute_user2       VARCHAR,
                                           v_execute_user3       VARCHAR,
                                           v_anaesthesia_type_id NUMERIC,
                                           v_close_level         NUMERIC,
                                           v_anaesthesia_user    VARCHAR,
                                           --v_Valide numeric ,
                                           v_create_user     VARCHAR,
                                           v_OPERATION_LEVEL varchar,
                                           v_IsChooseDate    varchar, --手术相关字段
                                           v_IsClearOpe      varchar,
                                           v_IsGanRan        varchar,
                                           v_anesthesia_level varchar,
                                           v_opercomplication_code varchar
                                           --v_Create_Time varchar(19)
                                           --v_Cancel_User varchar(10) ,
                                           --v_Cancel_Time varchar(19)
                                           ) AS /**********
                                                                                   版本号  1.0.0.0.0
                                                                                   创建时间
                                                                                   作者
                                                                                   版权
                                                                                   描述  插入功病案首页手术TABLE
                                                                                   功能说明
                                                                                   输入参数
                                                                                   输出参数
                                                                                   结果集、排序
            
                                                                                   调用的sp
                                                                                   调用实例
            
                                                                                   修改记录
                                                                                  **********/
  BEGIN
    INSERT INTO iem_mainpage_operation_2012
      (iem_mainpage_operation_no,
       iem_mainpage_no,
       operation_code,
       operation_date,
       operation_name,
       execute_user1,
       execute_user2,
       execute_user3,
       anaesthesia_type_id,
       close_level,
       anaesthesia_user,
       valide,
       create_user,
       create_time,
       OPERATION_LEVEL,
       IsChooseDate, --增加的手术相关字段
       IsClearOpe,
       IsGanRan
       ,anesthesia_level,opercomplication_code)
    VALUES
      (seq_iem_mainpage_operation_id.NEXTVAL,
       v_iem_mainpage_no, --numeric
       v_operation_code, -- varchar(60)
       v_operation_date,
       -- varchar(19)
       v_operation_name, -- varchar(300)
       v_execute_user1, -- varchar(20)
       v_execute_user2,
       -- varchar(20)
       v_execute_user3, -- varchar(20)
       v_anaesthesia_type_id, -- numeric
       v_close_level,
       -- numeric
       v_anaesthesia_user, -- varchar(20)
       1, -- numeric
       v_create_user, -- varchar(10)
       TO_CHAR(SYSDATE, 'yyyy-mm-dd HH24:mi:ss'),
       v_OPERATION_LEVEL,
       v_IsChooseDate, --手术相关字段
       v_IsClearOpe,
       v_IsGanRan
       ,v_anesthesia_level,v_opercomplication_code);
  END;

 

/*********************************************************************************/
  PROCEDURE usp_edif_iem_mainpage_diag2012(v_iem_mainpage_no   VARCHAR,
                                           v_diagnosis_type_id VARCHAR,
                                           v_diagnosis_code    VARCHAR,
                                           v_diagnosis_name    VARCHAR,
                                           v_status_id         VARCHAR,
                                           v_order_value       VARCHAR,
                                           --v_Valide numeric ,
                                           v_create_user           VARCHAR,
                                           v_MenAndInHop           varchar, --门诊和住院
                                           v_InHopAndOutHop        varchar, --入院和出院
                                           v_BeforeOpeAndAfterOper varchar, --术前和术后
                                           v_LinAndBingLi          varchar, --临床与病理
                                           v_InHopThree            varchar, --入院三日内
                                           v_FangAndBingLi         varchar, --放射和病理
                                           v_AdmitInfo             varchar --子入院病情
                                           --v_Create_Time varchar(19) ,
                                           --v_Cancel_User varchar(10) ,
                                           --v_Cancel_Time varchar(19)
                                           ) AS /**********
                                                                                   版本号  1.0.0.0.0
                                                                                   创建时间
                                                                                   作者
                                                                                   版权
                                                                                   描述  插入功病案首页诊断TABLE
                                                                                   功能说明
                                                                                   输入参数
                                                                                   输出参数
                                                                                   结果集、排序

                                                                                   调用的sp
                                                                                   调用实例

                                                                                   修改记录
                                                                                  **********/
  BEGIN
    INSERT INTO iem_mainpage_diagnosis_2012
      (iem_mainpage_diagnosis_no,
       iem_mainpage_no,
       diagnosis_type_id,
       diagnosis_code,
       diagnosis_name,
       status_id,
       order_value,
       valide,
       create_user,
       create_time,
       MenAndInHop,
       InHopAndOutHop,
       BeforeOpeAndAfterOper,
       LinAndBingLi,
       InHopThree,
       FangAndBingLi,
       admitinfo)
    VALUES
      (seq_iem_mainpage_diagnosis_id.NEXTVAL,
       v_iem_mainpage_no, -- Iem_Mainpage_NO - numeric
       v_diagnosis_type_id,
       -- Diagnosis_Type_Id - numeric
       v_diagnosis_code,
       -- Diagnosis_Code - varchar(60)
       v_diagnosis_name, -- Diagnosis_Name - varchar(300)
       v_status_id, -- Status_Id - numeric
       v_order_value,
       -- Order_Value - numeric
       1,
       -- Valide - numeric
       v_create_user, -- Create_User - varchar(10)
       TO_CHAR(SYSDATE, 'yyyy-mm-dd HH24:mi:ss'),
       v_MenAndInHop, --门诊和住院
       v_InHopAndOutHop, --入院和出院
       v_BeforeOpeAndAfterOper, --术前和术后
       v_LinAndBingLi, --临床与病理
       v_InHopThree, --入院三日内
       v_FangAndBingLi,
       v_AdmitInfo
       -- Create_Time - varchar(19)
       );
  END;
  
  
  -----------------------------------------------湖北专用首页存储过程  add jxh----------------------------------------------------------------------
  PROCEDURE usp_edif_iem_mainpage_diag_hb(v_iem_mainpage_no   VARCHAR,
                                           v_diagnosis_type_id VARCHAR,
                                           v_diagnosis_code    VARCHAR,
                                           v_diagnosis_name    VARCHAR,                                            
                                           v_status_id         VARCHAR,
                                           v_order_value       VARCHAR,
                                           v_morphologyicd     VARCHAR,--形态学诊断编码
                                           v_morphologyname    VARCHAR,--形态学诊断名称 
                                           --v_Valide numeric ,
                                           v_create_user           VARCHAR,
                                           v_MenAndInHop           varchar, --门诊和住院
                                           v_InHopAndOutHop        varchar, --入院和出院
                                           v_BeforeOpeAndAfterOper varchar, --术前和术后
                                           v_LinAndBingLi          varchar, --临床与病理
                                           v_InHopThree            varchar, --入院三日内
                                           v_FangAndBingLi         varchar, --放射和病理
                                           v_AdmitInfo             varchar --子入院病情 
                                           --v_Create_Time varchar(19) ,
                                           --v_Cancel_User varchar(10) ,
                                           --v_Cancel_Time varchar(19)
                                           ) AS /**********
                                                                                   版本号  1.0.0.0.0
                                                                                   创建时间
                                                                                   作者
                                                                                   版权
                                                                                   描述  插入功病案首页诊断TABLE
                                                                                   功能说明
                                                                                   输入参数
                                                                                   输出参数
                                                                                   结果集、排序
            
                                                                                   调用的sp
                                                                                   调用实例
            
                                                                                   修改记录
                                                                                  **********/
  BEGIN
    INSERT INTO iem_mainpage_diagnosis_2012
      (iem_mainpage_diagnosis_no,
       iem_mainpage_no,
       diagnosis_type_id,
       diagnosis_code,
       diagnosis_name, 
       morphologyicd,
       morphologyname,
       status_id,
       order_value,
       valide,
       create_user,
       create_time,
       MenAndInHop,
       InHopAndOutHop,
       BeforeOpeAndAfterOper,
       LinAndBingLi,
       InHopThree,
       FangAndBingLi,
       admitinfo)
    VALUES
      (seq_iem_mainpage_diagnosis_id.NEXTVAL,
       v_iem_mainpage_no, -- Iem_Mainpage_NO - numeric
       v_diagnosis_type_id,
       -- Diagnosis_Type_Id - numeric
       v_diagnosis_code,
       -- Diagnosis_Code - varchar(60)
       v_diagnosis_name, -- Diagnosis_Name - varchar(300)  
       v_morphologyicd,
       v_morphologyname,     
       v_status_id, -- Status_Id - numeric
       v_order_value,
       -- Order_Value - numeric
       1,
       -- Valide - numeric
       v_create_user, -- Create_User - varchar(10)
       TO_CHAR(SYSDATE, 'yyyy-mm-dd HH24:mi:ss'),
       v_MenAndInHop, --门诊和住院
       v_InHopAndOutHop, --入院和出院
       v_BeforeOpeAndAfterOper, --术前和术后
       v_LinAndBingLi, --临床与病理
       v_InHopThree, --入院三日内
       v_FangAndBingLi,
       v_AdmitInfo
       -- Create_Time - varchar(19)
       );
  END;



  --更新病案首页信息后，对病人信息表进行数据同步 add by ywk 二〇一二年五月四日 15:20:27
  PROCEDURE usp_Edit_Iem_PaientInfo(v_NOOFINPAT      varchar2 default '', ---- '病人首页序号';
                                    v_NAME           varchar2 default '', ---- '患者姓名';
                                    v_SEXID          varchar2 default '', ---- '性别';
                                    v_BIRTH          varchar2 default '', ---- '出生';
                                    v_Age            INTEGER default '', --年龄
                                    v_IDNO           varchar2 default '', ---- '身份证号码';
                                    v_MARITAL        varchar2 default '', ---- '婚姻状况 1.未婚 2.已婚 3.丧偶4.离婚 9.其他';
                                    v_JOBID          varchar2 default '', ---- '职业';
                                    v_CSD_PROVINCEID varchar2 default '', ---- '出生地 省';
                                    v_CSD_CITYID     varchar2 default '', ---- '出生地 市';
                                    v_NATIONID       varchar2 default '', --民族
                                    v_NATIONALITYID  varchar2 default '', ---- '国籍ID';
                                    v_JG_PROVINCEID  varchar2 default '', ---- '籍贯 省';
                                    v_JG_CITYID      varchar2 default '', ---- '籍贯 市';
                                    v_OFFICEPLACE    varchar2 default '', ---- '工作单位地址';
                                    v_OFFICETEL      varchar2 default '', ---- '工作单位电话';
                                    v_OFFICEPOST     varchar2 default '', ---- '工作单位邮编';
                                    v_HKDZ_POST      varchar2 default '', ---- '户口所在地邮编';
                                    v_CONTACTPERSON  varchar2 default '', ---- '联系人姓名';
                                    v_RELATIONSHIP   varchar2 default '', ---- '与联系人关系';
                                    v_CONTACTADDRESS varchar2 default '', ---- '联系人地址';
                                    v_CONTACTTEL     varchar2 default '', ---- '联系人电话';
                                    v_ADMITDEPT      varchar2 default '', ---- '入院科室';
                                    v_ADMITWARD      varchar2 default '', ---- '入院病区';\
                                    v_ADMITDATE      varchar2 default '', ---- '入院时间';
                                    v_OUTWARDDATE    varchar2 default '', ---- '出院时间';
                                    v_OUTHOSDEPT     varchar2 default '', ---- '出院科室';
                                    v_OUTHOSWARD     varchar2 default '', ---- '出院病区';
                                    v_ACTUALDAYS     varchar2 default '', ---- '实际住院天数';
                                    v_AgeStr         varchar2 default '', ---- '年龄 精确到月天;2012年5月9日9:31:03 （杨维康 泗县修改）
                                    v_PatId          varchar2 default '', --新增的付款方式 add by ywk 2012年5月14日 16:02:13
                                    v_CardNo         varchar2 default '', --健康卡号   
                                    
                                    -----add by ywk  2012年5月16日9:45:27 Inpatient表l里增加病案首页里相应的字段
                                    v_Districtid     varchar2 default '', --出生地‘县’
                                    v_xzzproviceid   varchar2 default '', --现在住址省
                                    v_xzzcityid      varchar2 default '', --现在住址市
                                    v_xzzdistrictid  varchar2 default '', --现在住址县
                                    v_xzztel         varchar2 default '', --现在住址电话
                                    v_hkdzproviceid  varchar2 default '', --户口住址省
                                    v_hkzdcityid     varchar2 default '', --户口住址市
                                    v_hkzddistrictid varchar2 default '', --户口住址县    
                                    v_xzzpost        varchar2 default '', --现住址邮编    
                                    v_isupdate       varchar2 default '', ---2012年5月24日 17:19:10 ywk 是否更新身份证号字段
                                    v_CSDADDRESS     varchar2 default '', --出生地具体地址 add by ywk 2012年7月11日 10:13:49
                                    v_JGADDRESS      varchar2 default '', --籍贯地址具体地址 add by ywk 2012年7月11日 10:13:49
                                    v_XZZADDRESS     varchar2 default '', --现住址具体地址 add by ywk 2012年7月11日 10:13:49
                                    v_HKDZADDRESS    varchar2 default '' --户口地址具体地址 add by ywk 2012年7月11日 10:13:49
                                    --add by cyq 2012-12-27 Inpatient表里增加病案首页里相应的字段
                                    ,v_XZZDetailAddr varchar2 default '', --现住址详细地址(县级以下) add by cyq 2012-12-27
                                    v_HKDZDetailAddr varchar2 default '' --户口地址详细地址(县级以下) add by cyq 2012-12-27
                                    ) as
    myidno          varchar2(50);
    mymaterital     varchar2(50);
    myjobid         varchar2(50);
    myadmitdept     varchar2(50);
    mydamitward     varchar2(50);
    myouthosdept    varchar2(50);
    myouthosward    varchar2(50);
    myofficepalce   varchar2(250);
    myofficetel     varchar2(50);
    myofficepost    varchar2(50);
    mycontactperson varchar2(50);
    myadmitdate     varchar2(50);
    myouthosdate    varchar2(50);
    mynoofclinic    varchar2(50);
  begin
    /*select nvl(idno,v_IDNO)
     into myidno
     from inpatient
    where NoOfInpat = v_NOOFINPAT; --身份证号码*/
  
    myidno := v_IDNO;
  
    /*select nvl(v_MARITAL, Marital)
     into mymaterital
     from inpatient
    where NoOfInpat = v_NOOFINPAT; --婚姻*/
    mymaterital := v_MARITAL;
    myjobid     := v_JOBID;
    /* select nvl(v_JOBID, jobid)
     into myjobid
     from inpatient
    where NoOfInpat = v_NOOFINPAT; --职业*/
    --  myadmitdept:=v_ADMITDEPT;
    select nvl(v_ADMITDEPT, AdmitDept)
      into myadmitdept
      from inpatient
     where NoOfInpat = v_NOOFINPAT; --入院科室(如果病案首页里编辑的填写内容是空，就取原来的Inpatient表里的数据)
    --mydamitward:=  v_ADMITWARD; 
    select nvl(v_ADMITWARD, AdmitWard)
      into mydamitward
      from inpatient
     where NoOfInpat = v_NOOFINPAT; --入院病区
    --myouthosdept:=v_OUTHOSDEPT;
    select nvl(v_OUTHOSDEPT, OutHosDept)
      into myouthosdept
      from inpatient
     where NoOfInpat = v_NOOFINPAT; --出院室科
    --  myouthosward:=v_OUTHOSWARD;
    select nvl(v_OUTHOSWARD, OutHosWard)
      into myouthosward
      from inpatient
     where NoOfInpat = v_NOOFINPAT; --出院病区
    /*  select nvl(v_OFFICEPLACE, OfficePlace)
     into myofficepalce
     from inpatient
    where NoOfInpat = v_NOOFINPAT; --工作地址*/
    myofficepalce := v_OFFICEPLACE;
    /*   select nvl(v_OFFICETEL, OfficeTEL)
     into myofficetel
     from inpatient
    where NoOfInpat = v_NOOFINPAT; --工作单位电话*/
    myofficetel  := v_OFFICETEL;
    myofficepost := v_OFFICEPOST;
    /*select nvl(v_OFFICEPOST, OfficePost)
     into myofficepost
     from inpatient
    where NoOfInpat =v_NOOFINPAT; --工作单位邮编*/
    mycontactperson := v_CONTACTPERSON;
    /* s\*elect nvl(v_CONTACTPERSON, ContactPerson)
     into mycontactperson
     from inpatient
    where NoOfInpat =*\ v_NOOFINPAT; --联系人姓名*/
    myadmitdate := v_ADMITDATE;
    /*select nvl(v_ADMITDATE, AdmitDate)
     into myadmitdate
     from inpatient
    where NoOfInpat = v_NOOFINPAT; --入院时间*/
    myouthosdate := v_OUTWARDDATE;
    select nvl(v_OUTWARDDATE, OutHosDate)
      into myouthosdate
      from inpatient
     where NoOfInpat = v_NOOFINPAT; --出院时间
    if v_isupdate = '0' then
      update inpatient
         set Name           = v_NAME,
             sexid          = v_SEXID,
             birth          = v_BIRTH,
             age            = v_Age,
             idno           = myidno,
             Marital        = mymaterital,
             jobid          = myjobid,
             ProvinceID     = v_CSD_PROVINCEID,
             CountyID       = v_CSD_CITYID,
             NationID       = v_NATIONID,
             NationalityID  = v_NATIONALITYID,
             Nativeplace_P  = v_JG_PROVINCEID,
             Nativeplace_C  = v_JG_CITYID,
             OfficePlace    = myofficepalce,
             OfficeTEL      = myofficetel,
             OfficePost     = myofficepost,
             NativePost     = v_HKDZ_POST,
             ContactPerson  = mycontactperson,
             Relationship   = v_RELATIONSHIP,
             ContactAddress = v_CONTACTADDRESS,
             ContactTEL     = v_CONTACTTEL,
             AdmitDept      = myadmitdept,
             AdmitWard      = mydamitward,
             AdmitDate      = myadmitdate,
             OutHosDate     = myouthosdate,
             OutHosDept     = myouthosdept,
             OutHosWard     = myouthosward,
             TotalDays      = v_ACTUALDAYS,
             AgeStr         = v_AgeStr,
             payid          = v_PatId,
             cardno         = v_CardNo,
             districtid     = v_Districtid,
             xzzproviceid   = v_xzzproviceid,
             xzzcityid      = v_xzzcityid,
             xzzdistrictid  = v_xzzdistrictid,
             xzztel         = v_xzztel,
             hkdzproviceid  = v_hkdzproviceid,
             hkzdcityid     = v_hkzdcityid,
             hkzddistrictid = v_hkzddistrictid,
             xzzpost        = v_xzzpost,
             CSDADDRESS     = v_CSDADDRESS,
             JGADDRESS      = v_JGADDRESS,
             XZZADDRESS     = v_XZZADDRESS,
             HKADDRESS      = v_HKDZADDRESS
             ,xzzdetailaddress = v_XZZDetailAddr, --add by cyq 2012-12-27
             hkdzdetailaddress = v_HKDZDetailAddr --add by cyq 2012-12-27
       where NoOfInpat = v_NOOFINPAT;
      commit;
    end if;
    if v_isupdate = '1' then
      if myidno = '不详' then
        mynoofclinic := '';
      else
        mynoofclinic := myidno;
      end if;
      update inpatient
         set Name           = v_NAME,
             sexid          = v_SEXID,
             birth          = v_BIRTH,
             age            = v_Age,
             idno           = mynoofclinic,
             Marital        = mymaterital,
             jobid          = myjobid,
             ProvinceID     = v_CSD_PROVINCEID,
             CountyID       = v_CSD_CITYID,
             NationID       = v_NATIONID,
             NationalityID  = v_NATIONALITYID,
             Nativeplace_P  = v_JG_PROVINCEID,
             Nativeplace_C  = v_JG_CITYID,
             OfficePlace    = myofficepalce,
             OfficeTEL      = myofficetel,
             OfficePost     = myofficepost,
             NativePost     = v_HKDZ_POST,
             ContactPerson  = mycontactperson,
             Relationship   = v_RELATIONSHIP,
             ContactAddress = v_CONTACTADDRESS,
             ContactTEL     = v_CONTACTTEL,
             AdmitDept      = myadmitdept,
             AdmitWard      = mydamitward,
             AdmitDate      = myadmitdate,
             OutHosDate     = myouthosdate,
             OutHosDept     = myouthosdept,
             OutHosWard     = myouthosward,
             TotalDays      = v_ACTUALDAYS,
             AgeStr         = v_AgeStr,
             payid          = v_PatId,
             cardno         = v_CardNo,
             districtid     = v_Districtid,
             xzzproviceid   = v_xzzproviceid,
             xzzcityid      = v_xzzcityid,
             xzzdistrictid  = v_xzzdistrictid,
             xzztel         = v_xzztel,
             hkdzproviceid  = v_hkdzproviceid,
             hkzdcityid     = v_hkzdcityid,
             hkzddistrictid = v_hkzddistrictid,
             xzzpost        = v_xzzpost,
             noofclinic     = mynoofclinic,
             CSDADDRESS     = v_CSDADDRESS,
             JGADDRESS      = v_JGADDRESS,
             XZZADDRESS     = v_XZZADDRESS,
             HKADDRESS      = v_HKDZADDRESS
             ,xzzdetailaddress = v_XZZDetailAddr, --add by cyq 2012-12-27
             hkdzdetailaddress = v_HKDZDetailAddr --add by cyq 2012-12-27
       where NoOfInpat = v_NOOFINPAT;
      commit;
    end if;
    /* begin
    update inpatient set Name = v_NAME ,sexid=v_SEXID,birth=v_BIRTH,age=v_Age,idno=v_IDNO,Marital=v_MARITAL,jobid=v_JOBID,
    ProvinceID=v_CSD_PROVINCEID,CountyID=v_CSD_CITYID,NationID=v_NATIONID,NationalityID=v_NATIONALITYID,Nativeplace_P=v_JG_PROVINCEID,
    Nativeplace_C=v_JG_CITYID,OfficePlace=v_OFFICEPLACE,OfficeTEL=v_OFFICETEL,OfficePost=v_OFFICEPOST,NativePost=v_HKDZ_POST,
    ContactPerson=v_CONTACTPERSON,Relationship=v_RELATIONSHIP,ContactAddress=v_CONTACTADDRESS,ContactTEL=v_CONTACTTEL,AdmitDept=v_ADMITDEPT,
    AdmitWard=v_ADMITWARD,AdmitDate=v_ADMITDATE,OutHosDate=v_OUTWARDDATE,OutHosDept=v_OUTHOSDEPT,OutHosWard=v_OUTHOSWARD,TotalDays=v_ACTUALDAYS
    where NoOfInpat = v_NOOFINPAT;
    commit;*/
  
  End;

  --获得首页默认表里的数据
  --add by ywk 2012年5月17日 09:36:46
  PROCEDURE usp_GetDefaultInpatient(o_result OUT empcurtyp) AS
  BEGIN
    OPEN o_result FOR
      select inp.*,
             pay.name             payname,
             job.name             jobname,
             nation.name          nationame,
             nationality.name     nationaltyname,
             relationship.name    relationshipname,
             csdpro.provincename  csd_provincename,
             csdcity.cityname     csd_cityname,
             csddis.districtname  csd_districtname,
             xzzpro.provincename  xzz_provincename,
             xzzcity.cityname     xzz_cityname,
             xzzdis.districtname  xzz_districtname,
             hkzzpro.provincename hkdz_provincename,
             hkzzcity.cityname    hkdz_cityname,
             hkzzdis.districtname hkdz_districtname,
             jgpro.provincename   jg_provincename,
             jgcity.cityname      jg_cityname,
             dia.name             outdiagname -- 门诊诊断
        from inpatient_default inp
        left join dictionary_detail pay
          on pay.detailid = inp.payid
         and pay.categoryid = '1'
        left join dictionary_detail job
          on job.detailid = inp.jobid
         and job.categoryid = '41'
        left join dictionary_detail nation
          on nation.detailid = inp.nationid
         and nation.categoryid = '42'
        left join dictionary_detail nationality
          on nationality.detailid = inp.nationalityid
         and nationality.categoryid = '43'
        left join dictionary_detail relationship
          on relationship.detailid = inp.relationship
         and relationship.categoryid = '44'
        left join diagnosis dia
          on dia.icd = inp.clinicdiagnosis
      ---省，市，县的名称从Inpatient表里去除，取名称，进行链表查询 add by ywk 2012年5月17日10:58:03
        left join s_province csdpro
          on inp.provinceid = csdpro.provinceid --出生地的省
        left join s_city csdcity
          on inp.countyid = csdcity.cityid --出生地的市
        left join s_district csddis
          on inp.districtid = csddis.districtid --出生地的县
      
        left join s_province jgpro
          on inp.nativeplace_p = jgpro.provinceid --籍贯的省
        left join s_city jgcity
          on inp.nativeplace_c = jgcity.cityid --籍贯的市
      
        left join s_province xzzpro
          on inp.xzzproviceid = xzzpro.provinceid --现住址的省
        left join s_city xzzcity
          on inp.xzzcityid = xzzcity.cityid --现住址的市
        left join s_district xzzdis
          on inp.xzzdistrictid = xzzdis.districtid --现住址的县
      
        left join s_province hkzzpro
          on inp.hkdzproviceid = hkzzpro.provinceid --户口住址的省
        left join s_city hkzzcity
          on inp.hkzdcityid = hkzzcity.cityid --户口住址的市
        left join s_district hkzzdis
          on inp.hkzddistrictid = hkzzdis.districtid; --户口住址的县
  
  END;

  PROCEDURE usp_GetInpatientByNo(v_noofinpat varchar2 default '',
                                 o_result    OUT empcurtyp) AS
  BEGIN
    OPEN o_result FOR
      select inp.*,
             pay.name             payname,
             job.name             jobname,
             nation.name          nationame,
             nationality.name     nationaltyname,
             relationship.name    relationshipname,
             csdpro.provincename  csd_provincename,
             csdcity.cityname     csd_cityname,
             csddis.districtname  csd_districtname,
             xzzpro.provincename  xzz_provincename,
             xzzcity.cityname     xzz_cityname,
             xzzdis.districtname  xzz_districtname,
             hkzzpro.provincename hkdz_provincename,
             hkzzcity.cityname    hkdz_cityname,
             hkzzdis.districtname hkdz_districtname,
             jgpro.provincename   jg_provincename,
             jgcity.cityname      jg_cityname,
             dia.name             outdiagname, -- 门诊诊断
             u1.name              CHIEFNAME,
             u2.name              attendname,
             u3.name              RESIDENTNAME
        from inpatient inp
        left join dictionary_detail pay
          on pay.detailid = inp.payid
         and pay.categoryid = '1'
        left join dictionary_detail job
          on job.detailid = inp.jobid
         and job.categoryid = '41'
        left join dictionary_detail nation
          on nation.detailid = inp.nationid
         and nation.categoryid = '42'
        left join dictionary_detail nationality
          on nationality.detailid = inp.nationalityid
         and nationality.categoryid = '43'
        left join dictionary_detail relationship
          on relationship.detailid = inp.relationship
         and relationship.categoryid = '44'
        left join diagnosis dia
          on dia.icd = inp.clinicdiagnosis
        left join users u1
          on u1.id = inp.chief
        left join users u2
          on u2.id = inp.attend
        left join users u3
          on u3.id = inp.resident
      ---省，市，县的名称从Inpatient表里去除，取名称，进行链表查询 add by ywk 2012年5月17日10:58:03
        left join s_province csdpro
          on inp.provinceid = csdpro.provinceid --出生地的省
        left join s_city csdcity
          on inp.countyid = csdcity.cityid --出生地的市
        left join s_district csddis
          on inp.districtid = csddis.districtid --出生地的县
      
        left join s_province jgpro
          on inp.nativeplace_p = jgpro.provinceid --籍贯的省
        left join s_city jgcity
          on inp.nativeplace_c = jgcity.cityid --籍贯的市
      
        left join s_province xzzpro
          on inp.xzzproviceid = xzzpro.provinceid --现住址的省
        left join s_city xzzcity
          on inp.xzzcityid = xzzcity.cityid --现住址的市
        left join s_district xzzdis
          on inp.xzzdistrictid = xzzdis.districtid --现住址的县
      
        left join s_province hkzzpro
          on inp.hkdzproviceid = hkzzpro.provinceid --户口住址的省
        left join s_city hkzzcity
          on inp.hkzdcityid = hkzzcity.cityid --户口住址的市
        left join s_district hkzzdis
          on inp.hkzddistrictid = hkzzdis.districtid --户口住址的县
       where inp.noofinpat = v_noofinpat;
  end;

  /*
  * 维护病案首页的费用信息
  add by ywk 2012年10月16日 19:29:53
  */
  PROCEDURE usp_editiem_mainpage_feeinfo(v_edittype        varchar2 default '',
                                         v_IEM_MAINPAGE_NO varchar2 default '', ---- '病案首页标识';
                                         v_TotalFee        varchar2 default '', ---- 总费用;
                                         v_OwnerFee        varchar2 default '', ---- '自付金额
                                         v_YbMedServFee    varchar2 default '', ---- 一般医疗服务费
                                         v_YbMedOperFee    varchar2 default '', ---- 一般治疗操作费
                                         v_NurseFee        varchar2 default '', ----护理费
                                         v_OtherInfo       varchar2 default '', ---- 综合类 其他费用
                                         v_BLZDFee         varchar2 default '', ---- 诊断类 病理诊断费
                                         v_SYSZDFee        varchar2 default '', ---- 实验室诊断费
                                         v_YXXZDFee        varchar2 default '', ----  诊断类 影像学诊断费
                                         v_LCZDItemFee     varchar2 default '', ----  诊断类 临床诊断项目费
                                         v_FSSZLItemFee    varchar2 default '', ----  非手术治疗项目费
                                         v_LCWLZLFee       varchar2 default '', ---- 治疗类 临床物理治疗费
                                         v_OperMedFee      varchar2 default '', ----治疗类 手术治疗费
                                         v_KFFee           varchar2 default '', ----康复类 康复费
                                         v_ZYZLFee         varchar2 default '', ----中医类 中医治疗费
                                         v_XYMedFee        varchar2 default '', ---西药类 西药费
                                         v_KJYWFee         varchar2 default '', ---西药类 抗菌药物费用
                                         v_ZCYFFee         varchar2 default '', ---中药类 中成药费
                                         v_ZCaoYFFee       varchar2 default '', ---中药类 中草药费
                                         v_BloodFee        varchar2 default '', ---血液和血液制品类 血费
                                         v_BDBLZPFFee      varchar2 default '', ---血液和血液制品类 白蛋白类制品费
                                         v_QDBLZPFFee      varchar2 default '', ---球蛋白类制品费
                                         v_NXYZLZPFFee     varchar2 default '', ---血液和血液制品类 凝血因子类制品费
                                         v_XBYZLZPFFee     varchar2 default '', ---细胞因子类制品费
                                         v_JCYYCXYYCLFFee  varchar2 default '', -- 检查用一次性医用材料费
                                         v_ZLYYCXYYCLFFee  varchar2 default '', -- /耗材类 治疗用一次性医用材料费
                                         v_SSYYCXYYCLFFee  varchar2 default '', -- 材类 手术用一次性医用材料费
                                         v_QTFee           varchar2 default '', -- 其他类：（24）其他费        
                                         v_Memo1           varchar2 default '', -- 预留字段   1    
                                         v_Memo2           varchar2 default '', -- 预留字段    2  
                                         v_Memo3           varchar2 default '', -- 预留字段     3     
                                            v_MaZuiFee           varchar2 default '', -- 麻醉费
                                         v_ShouShuFee           varchar2 default ''-- 手术费 
                                         ) as
   p_count integer default 0;
 
  begin
     IF v_edittype = '1' THEN
  SELECT NVL(MAX(ID), 0) + 1 into p_count FROM Iem_MainPage_FeeInfo;


    
      --新增病案首页基本信息
      insert into Iem_MainPage_FeeInfo(ID,IEM_MAINPAGE_NO,TotalFee,OwnerFee,YbMedServFee,
      YbMedOperFee,NurseFee,OtherInfo,BLZDFee,SYSZDFee,YXXZDFee,LCZDItemFee,FSSZLItemFee,LCWLZLFee,OperMedFee,KFFee,
      ZYZLFee,XYMedFee,KJYWFee,ZCYFFee,ZCaoYFFee,BloodFee,BDBLZPFFee,QDBLZPFFee,NXYZLZPFFee,XBYZLZPFFee,JCYYCXYYCLFFee,Zlyycxyyclffee
      ,SSYYCXYYCLFFee,QTFee,MemoFee1,MemoFee2,MemoFee3,valid,Mazuifee,shoushufee)
      values
        (p_count, ---- '病案首页标识';
        V_IEM_MAINPAGE_NO,V_TotalFee,V_OwnerFee,V_YbMedServFee,
      V_YbMedOperFee,V_NurseFee,V_OtherInfo,V_BLZDFee,V_SYSZDFee,V_YXXZDFee,V_LCZDItemFee,V_FSSZLItemFee,V_LCWLZLFee,V_OperMedFee,V_KFFee,
      V_ZYZLFee,V_XYMedFee,V_KJYWFee,V_ZCYFFee,V_ZCaoYFFee,V_BloodFee,V_BDBLZPFFee,V_QDBLZPFFee,V_NXYZLZPFFee,V_XBYZLZPFFee,V_JCYYCXYYCLFFee,V_Zlyycxyyclffee
      ,V_SSYYCXYYCLFFee,V_QTFee,V_Memo1,V_Memo2,V_Memo3,1,V_MaZuifee,V_shoushufee
         );
    
      ---修改病案首页基本信息
    ELSIF v_edittype = '2' THEN
    
      update Iem_MainPage_FeeInfo
      set TotalFee=V_TotalFee,
      OwnerFee=v_OwnerFee,
      YbMedServFee=V_YbMedServFee,
      YbMedOperFee=V_YbMedOperFee,NurseFee=V_NurseFee,OtherInfo=V_OtherInfo,
      BLZDFee=V_BLZDFee,SYSZDFee=V_SYSZDFee,YXXZDFee=V_YXXZDFee,LCZDItemFee=V_LCZDItemFee,FSSZLItemFee=V_FSSZLItemFee,
      LCWLZLFee=V_LCWLZLFee,OperMedFee=V_OperMedFee,KFFee=V_KFFee,
      ZYZLFee=V_ZYZLFee,XYMedFee=V_XYMedFee,KJYWFee=V_KJYWFee,ZCYFFee=V_ZCYFFee,
      ZCaoYFFee=V_ZCaoYFFee,BloodFee=V_BloodFee,BDBLZPFFee=V_BDBLZPFFee,QDBLZPFFee=V_QDBLZPFFee,NXYZLZPFFee=V_NXYZLZPFFee,
      XBYZLZPFFee=V_XBYZLZPFFee,JCYYCXYYCLFFee=V_JCYYCXYYCLFFee,Zlyycxyyclffee=V_Zlyycxyyclffee
      ,SSYYCXYYCLFFee=V_SSYYCXYYCLFFee,QTFee=V_QTFee,MemoFee1=V_Memo1,MemoFee2=V_Memo2,MemoFee3=V_Memo3,MaZuiFee=V_MaZuifee,ShouShuFee=v_ShouShuFee
      where IEM_MAINPAGE_NO=V_IEM_MAINPAGE_NO;
        
   
    end if;
  end;
  
  procedure usp_operiem_mainpage_qc
  (
            v_OperType         varchar2 default '0',
            v_id               varchar2 default '0',
            v_tabletype        varchar2 default '0',
            v_fields           varchar2 default '',
            v_fieldsvalue      varchar2 default '',
            v_conditiontabletype         varchar2 default '',
            v_conditionfields            varchar2 default '',
            v_conditionfieldsvalue       varchar2 default '',
            v_REDUCTSCORE                varchar2 default '0',
            v_REDUCTREASON               varchar2 default '',
            v_VALIDE                     varchar2 default '0',
            o_result                     OUT empcurtyp
            
  )as
  p_id varchar2(16);
  begin
  open o_result for
  select V_ID from dual;
  ---查询
  IF v_OperType = '0' THEN
     open o_result for
     select qc.id,
         (case qc.tabletype when '0' then '基本信息表'
                            when '1' then '诊断表'
                            when '2' then '手术表'
                            when '3' then '婴儿表'
                            when '4' then '病人表'
                            end) tabletype,
         qc.fields,
         qc.fieldsvalue,
         (case qc.conditiontabletype 
                            when '0' then '基本信息表'
                            when '1' then '诊断表'
                            when '2' then '手术表'
                            when '3' then '婴儿表'
                            when '4' then '病人表'
                            end) conditiontabletype,
         qc.conditionfields,
         qc.conditionfieldsvalue,
         qc.reductscore,
         qc.reductreason,
         (case qc.valide when '0' then '否'
                         when '1' then '是'
                         end)valide
      from iem_mainpage_qc qc;
  ---新增
  ELSIF v_OperType = '1' THEN
  insert into iem_mainpage_qc (ID,TABLETYPE,FIELDS,FIELDSVALUE,CONDITIONTABLETYPE,CONDITIONFIELDS,CONDITIONFIELDSVALUE,REDUCTSCORE,REDUCTREASON,VALIDE)
  values
  (seq_emr_configpoint.NEXTVAL,v_tabletype,v_fields,v_fieldsvalue,v_conditiontabletype,v_conditionfields,v_conditionfieldsvalue,v_REDUCTSCORE,v_REDUCTREASON,v_VALIDE);
  ---修改
  ELSIF v_OperType = '2' THEN
  update iem_mainpage_qc set TABLETYPE = v_tabletype, FIELDS = v_fields, FIELDSVALUE = v_fieldsvalue, CONDITIONTABLETYPE = v_conditiontabletype,
  CONDITIONFIELDS = v_conditionfields,CONDITIONFIELDSVALUE = v_conditionfieldsvalue,REDUCTSCORE = v_REDUCTSCORE,REDUCTREASON = v_REDUCTREASON,
  VALIDE = v_VALIDE where id = v_id;
  ---删除
  ELSIF v_OperType = '3' THEN
  delete from iem_mainpage_qc where id = v_id;
  end if;
  end;
END;
/
