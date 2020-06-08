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
                                  v_payway          VARCHAR,
                                  v_specas          VARCHAR,
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
                                    /*  v_csdprovicename   varchar2 default '',      --出生地省名称                                                                                                                                                         v_hkzzdistrictname    varchar2 default ''---户口住址县名称*/
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