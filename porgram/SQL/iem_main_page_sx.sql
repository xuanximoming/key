CREATE OR REPLACE PACKAGE iem_main_page_sx IS
  TYPE empcurtyp IS REF CURSOR;

  /*
  * 插入功病案首页 产妇婴儿信息
  */
  PROCEDURE usp_insert_iem_main_ObsBaby(v_iem_mainpage_no NUMERIC,
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
  * 维护病案首页信息 MOdify by xlb 2013-07-03 解决江西九江需求 新增字段
  */
  PROCEDURE usp_Edit_Iem_BasicInfo_sx(v_edittype        varchar2 default '',
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
                                      v_MAINDIAGDATE  varchar2 default '',----主要诊断确诊日期
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
                                      v_PATHOLOGY_OBSERVATION_FQ varchar2 default '',-----病理分期：
                                      v_FOLLOW_UP_CYCLE varchar2 default '',----随诊期限
                                      v_ALLERGIC_DRUG            varchar2 default '', ---- '过敏药物';
                                      v_SECTION_DIRECTOR         varchar2 default '', ---- '科主任';
                                      
                                      v_DIRECTOR               varchar2 default '', ---- '主（副主）任医师';
                                      v_VS_EMPLOYEE_CODE       varchar2 default '', ---- '主治医师';
                                      v_RESIDENT_EMPLOYEE_CODE varchar2 default '', ---- '住院医师';
                                      v_ATTENDING_PHYSICIAN_CODE varchar2 default '',---主诊医师
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
                                      v_Hospital_sense   varchar2 default '', ---- '院内感染';
                                      v_Hospital_sense_name   varchar2 default '', ---- '院内感染';
                                      v_ZYMOSISSTATE            varchar2 default '', ---- '医院传染病状态';
                                      v_PATHOLOGY_DIAGNOSIS_ID  varchar2 default '', ---- '病理诊断编号';
                                      v_MONTHAGE                varchar2 default '', ---- '（年龄不足1周岁的） 年龄(月)';
                                      v_WEIGHT                  varchar2 default '', ---- '新生儿出生体重(克)';
                                      
                                      v_INWEIGHT         varchar2 default '', ---- '新生儿入院体重(克)';
                                      v_INHOSTYPE        varchar2 default '', ---- '入院途径:1.急诊  2.门诊  3.其他医疗机构转入  9.其他';
                                      v_INHOSINFO        varchar2 default '', --入院情况 □ 1. 危 2. 急 3. 一般
                                      v_INHOSCALL        varchar2 default '', ----住院期间是否告病危或病重 □ 1. 是 2. 否
                                      v_TYPEHOSPITAL     varchar2 default '',----其他医疗机构
                                      v_OUTHOSTYPE       varchar2 default '', ---- '离院方式 □ 1.医嘱离院  2.医嘱转院 3.医嘱转社区卫生服务机构/乡镇卫生院 4.非医嘱离院5.死亡9.其他';
                                      v_ANTIBACTERIAL_DRUGS varchar2 default '',----Ⅰ类手术切口预防性应用抗菌药物
                                      v_DURATIONDATE varchar2 default '',---使用持续时间
                                      v_COMBINED_MEDICATION varchar2 default '',----联合用药
                                      
                                      v_PATHWAY_FLAG varchar2 default '',----联合用药
                                      v_PATHWAY_OVER varchar2 default '',----联合用药
                                      v_PATH_OUT_REASON varchar2 default '',----联合用药
                                      v_VARIATION_FLAG varchar2 default '',----联合用药
                                      v_VARIATION_REASON varchar2 default '',----联合用药
                                      v_REHOSPITALIZATION varchar2 default '',----联合用药
                                      v_INTERVALDATE varchar2 default '',----联合用药
                                      
                                      
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
                                      v_FOLLOW_UP     varchar2 default '',----随诊 □ 1. 是 2. 否
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
                                      
                                      v_CURE_TYPE   VARCHAR2 default '', ----  Y    治疗类别 □ 1.中医（ 1.1 中医   1.2民族医）    2.中西医     3.西医
                                      v_MZZYZD_NAME VARCHAR2 default '', ---- Y   门（急）诊诊断（中医诊断）
                                      v_MZZYZD_CODE VARCHAR2 default '', ---- Y   门（急）诊诊断（中医诊断） 编码
                                      v_MZXYZD_NAME VARCHAR2 default '', ---- Y   门（急）诊诊断（西医诊断）
                                      v_MZXYZD_CODE VARCHAR2 default '', ---- Y   门（急）诊诊断（西医诊断） 编码
                                      v_RYXYZD_NAME VARCHAR2 default '', ---- Y   入院诊断（西医诊断）
                                      v_RYXYZD_CODE VARCHAR2 default '', ---- Y   入院诊断（西医诊断） 编码
                                      v_SSLCLJ      VARCHAR2 default '', ---- Y   实施临床路径：□ 1. 中医  2. 西医  3 否
                                      v_ZYZJ        VARCHAR2 default '', ---- Y   使用医疗机构中药制剂：□ 1.是  2. 否
                                      
                                      v_ZYZLSB       VARCHAR2 default '', ---- Y   使用中医诊疗设备：□  1.是 2. 否
                                      v_ZYZLJS       VARCHAR2 default '', ---- Y   使用中医诊疗技术：□ 1. 是  2. 否
                                      v_BZSH         VARCHAR2 default '', ---- Y   辨证施护：□ 1.是  2. 否
                                      v_outHosStatus VARCHAR2 default '', ---出院状况
                                      v_JBYNZZ       VARCHAR2 default '',
                                      v_MZYCY        VARCHAR2 default '',
                                      v_InAndOutHos  VARCHAR2 default '',
                                      v_LCYBL        VARCHAR2 default '',
                                      v_FSYBL        VARCHAR2 default '',
                                      v_qJCount      VARCHAR2 default '',
                                      v_successCount VARCHAR2 default '',
                                      v_InPatLY      VARCHAR2 default '',
                                      v_asaScore     VARCHAR2 default '',
                                      o_result       OUT empcurtyp);

  /*
  *查询病案首页信息
  **********/
  ---Modify by xlb 2013-07-02 新增字段
  PROCEDURE usp_getieminfo_sx(v_noofinpat INT,
                              o_result    OUT empcurtyp,
                              o_result1   OUT empcurtyp,
                              o_result2   OUT empcurtyp,
                              o_result3   OUT empcurtyp,
                              o_result4   OUT empcurtyp);

  PROCEDURE usp_edit_iem_mainpage_oper_sx(v_iem_mainpage_no     NUMERIC,
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
                                          v_OperInTimes     VARCHAR2,
                                          v_Complication_Code VARCHAR,
                                          v_Complication_Name VARCHAR
                                          );

  PROCEDURE usp_edif_iem_mainpage_diag_sx(v_iem_mainpage_no   VARCHAR,
                                          v_diagnosis_type_id VARCHAR,
                                          v_diagnosis_code    VARCHAR,
                                          v_diagnosis_name    VARCHAR,
                                          v_status_id         VARCHAR,
                                          v_outstatus_id         VARCHAR,
                                          v_order_value       VARCHAR,
                                          v_create_user VARCHAR,
                                          v_type        varchar,
                                          v_typeName    varchar,
                                          v_orien_id varchar
                                          );

  --更新病案首页信息后，对病人信息表进行数据同步 add by ywk 二〇一二年五月四日 15:20:27
  PROCEDURE usp_Edit_Iem_PaientInfo_sx(v_NOOFINPAT      varchar2 default '', ---- '病人首页序号';
                                       v_NAME           varchar2 default '', ---- '患者姓名';
                                       v_SEXID          varchar2 default '', ---- '性别';
                                       v_BIRTH          varchar2 default '', ---- '出生';
                                       v_Age            INTEGER default 1, --年龄
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
                                       v_isupdate       varchar2 default '' ---2012年5月24日 17:19:10 ywk 是否更新身份证号字段
                                       
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
                                         v_MaZuiFee        varchar2 default '', -- 麻醉费
                                         v_ShouShuFee      varchar2 default '' -- 手术费 
                                         
                                         );

  PROCEDURE usp_AddOrModIemFeeZY(v_id        varchar,
                                 v_NOOFINPAT varchar,
                                 v_TOTAL     varchar,
                                 v_OWNFEE    varchar,
                                 v_YBYLFY    varchar,
                                 v_ZYBZLZF   varchar,
                                 v_ZYBZLZHZF varchar,
                                 v_YBZLFY    varchar,
                                 v_CARE      varchar,
                                 v_ZHQTFY    varchar,
                                 v_BLZDF     varchar,
                                 v_SYSZDF    varchar,
                                 v_YXXZDF    varchar,
                                 v_LCZDF     varchar,
                                 v_FSSZLF    varchar,
                                 v_LCWLZLF   varchar,
                                 v_SSZLF     varchar,
                                 v_MZF       varchar,
                                 v_SSF       varchar,
                                 v_KFF       varchar,
                                 v_ZYZDF     varchar,
                                 v_ZYZLF     varchar,
                                 v_ZYWZ      varchar,
                                 v_ZYGS      varchar,
                                 v_ZCYJF     varchar,
                                 v_ZYTNZL    varchar,
                                 v_ZYGCZL    varchar,
                                 v_ZYTSZL    varchar,
                                 v_ZYQT      varchar,
                                 v_ZYTSTPJG  varchar,
                                 v_BZSS      varchar,
                                 v_XYF       varchar,
                                 v_KJYWF     varchar,
                                 v_CPMEDICAL varchar,
                                 v_YLJGZYZJF varchar,
                                 v_CMEDICAL  varchar,
                                 v_BLOODFEE  varchar,
                                 v_XDBLZPF   varchar,
                                 v_QDBLZPF   varchar,
                                 v_NXYZLZPF  varchar,
                                 v_XBYZLZPF  varchar,
                                 v_JCYYCXCLF varchar,
                                 v_ZLYYCXCLF varchar,
                                 v_SSYYCXCLF varchar,
                                 v_OTHERFEE  varchar,
                                 v_VALID     varchar);

  PROCEDURE usp_GetIemFeeZYbyInpat(v_noofinpat varchar,
                                   o_result    OUT empcurtyp);

end;
