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

                                      v_CURE_TYPE   VARCHAR2 default '', ----  Y    治疗类别 □ 1.中医（ 1.1 中医   1.2民族医）    2.中西医     3.西医
                                      v_MZZYZD_NAME VARCHAR2 default '', ---- Y   门（急）诊诊断（中医诊断）
                                      v_MZZYZD_CODE VARCHAR2 default '', ---- Y   门（急）诊诊断（中医诊断） 编码
                                      v_MZXYZD_NAME VARCHAR2 default '', ---- Y   门（急）诊诊断（西医诊断）
                                      v_MZXYZD_CODE VARCHAR2 default '', ---- Y   门（急）诊诊断（西医诊断） 编码
                                      v_SSLCLJ      VARCHAR2 default '', ---- Y   实施临床路径：□ 1. 中医  2. 西医  3 否
                                      v_ZYZJ        VARCHAR2 default '', ---- Y   使用医疗机构中药制剂：□ 1.是  2. 否

                                      v_ZYZLSB VARCHAR2 default '', ---- Y   使用中医诊疗设备：□  1.是 2. 否
                                      v_ZYZLJS VARCHAR2 default '', ---- Y   使用中医诊疗技术：□ 1. 是  2. 否
                                      v_BZSH   VARCHAR2 default '', ---- Y   辨证施护：□ 1.是  2. 否
                                       v_outHosStatus VARCHAR2,---出院状况
                                      v_JBYNZZ VARCHAR2,
                                     v_MZYCY VARCHAR2,
                                      v_InAndOutHos VARCHAR2,
                                     v_LCYBL VARCHAR2,
                                     v_FSYBL VARCHAR2,
                                      v_qJCount VARCHAR2,
                                      v_successCount VARCHAR2,
                                      v_InPatLY VARCHAR2,
                                      v_asaScore VARCHAR2,
                                      o_result OUT empcurtyp);

  /*
  *查询病案首页信息
  **********/
  ---Modify by xlb 2013-07-02 新增字段
  PROCEDURE usp_getieminfo_sx(v_noofinpat INT,
                              o_result    OUT empcurtyp,
                              o_result1   OUT empcurtyp,
                              o_result2   OUT empcurtyp,
                              o_result3   OUT empcurtyp);

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
                                          --v_Create_Time varchar(19)
                                          --v_Cancel_User varchar(10) ,
                                          v_OperInTimes VARCHAR2
                                          --v_Cancel_Time varchar(19)
                                          );

  PROCEDURE usp_edif_iem_mainpage_diag_sx(v_iem_mainpage_no   VARCHAR,
                                          v_diagnosis_type_id VARCHAR,
                                          v_diagnosis_code    VARCHAR,
                                          v_diagnosis_name    VARCHAR,
                                          v_status_id         VARCHAR,
                                          v_order_value       VARCHAR,
                                          --v_Valide numeric ,
                                          v_create_user VARCHAR,
                                          v_type        varchar,
                                          v_typeName    varchar
                                          --v_Create_Time varchar(19) ,
                                          --v_Cancel_User varchar(10) ,
                                          --v_Cancel_Time varchar(19)
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
/
CREATE OR REPLACE PACKAGE BODY iem_main_page_sx IS

  /*
  * 插入病案首页中 产妇婴儿信息
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
                                        v_create_user     VARCHAR) as
  begin
    insert into IEM_MAINPAGE_OBSTETRICSBABY
      (IEM_MAINPAGE_OBSBABYID,
       IEM_MAINPAGE_NO,
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

  /**(*********编辑病案首页基本信息********************************************/
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

                                      v_CURE_TYPE   VARCHAR2 default '', ----  Y    治疗类别 □ 1.中医（ 1.1 中医   1.2民族医）    2.中西医     3.西医
                                      v_MZZYZD_NAME VARCHAR2 default '', ---- Y   门（急）诊诊断（中医诊断）
                                      v_MZZYZD_CODE VARCHAR2 default '', ---- Y   门（急）诊诊断（中医诊断） 编码
                                      v_MZXYZD_NAME VARCHAR2 default '', ---- Y   门（急）诊诊断（西医诊断）
                                      v_MZXYZD_CODE VARCHAR2 default '', ---- Y   门（急）诊诊断（西医诊断） 编码
                                      v_SSLCLJ      VARCHAR2 default '', ---- Y   实施临床路径：□ 1. 中医  2. 西医  3 否
                                      v_ZYZJ        VARCHAR2 default '', ---- Y   使用医疗机构中药制剂：□ 1.是  2. 否

                                      v_ZYZLSB VARCHAR2 default '', ---- Y   使用中医诊疗设备：□  1.是 2. 否
                                      v_ZYZLJS VARCHAR2 default '', ---- Y   使用中医诊疗技术：□ 1. 是  2. 否
                                      v_BZSH   VARCHAR2 default '', ---- Y   辨证施护：□ 1.是  2. 否
                                      v_outHosStatus VARCHAR2,---出院状况
                                     v_JBYNZZ VARCHAR2,
                                      v_MZYCY VARCHAR2,
                                     v_InAndOutHos VARCHAR2,
                                     v_LCYBL VARCHAR2,
                                      v_FSYBL VARCHAR2,
                                     v_qJCount VARCHAR2,
                                     v_successCount VARCHAR2,
                                     v_InPatLY VARCHAR2,
                                        v_asaScore VARCHAR2,
                                      o_result OUT empcurtyp) as
    mynoofclinic varchar2(50);

  begin
    if v_IDNO = '不详' then
      mynoofclinic := '';
    else
      mynoofclinic := v_IDNO;
    end if;

    IF v_edittype = '1' THEN

      --新增病案首页基本信息
      insert into iem_mainpage_basicinfo_sx
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
         JG_CITYNAME, ---- '籍贯 市名称';
         age,

         CURE_TYPE, ---- 治疗类别 □ 1.中医（ 1.1 中医   1.2民族医）    2.中西医     3.西医
         MZZYZD_NAME, ---- 门（急）诊诊断（中医诊断）
         MZZYZD_CODE, ---- 门（急）诊诊断（中医诊断） 编码
         MZXYZD_NAME, ---- 门（急）诊诊断（西医诊断）
         MZXYZD_CODE, ---- 门（急）诊诊断（西医诊断） 编码
         SSLCLJ, ---- 实施临床路径：□ 1. 中医  2. 西医  3 否
         ZYZJ, ---- 使用医疗机构中药制剂：□ 1.是  2. 否
         ZYZLSB, ---- 使用中医诊疗设备：□  1.是 2. 否
         ZYZLJS, ---- 使用中医诊疗技术：□ 1. 是  2. 否
         BZSH, ---- 辨证施护：□ 1.是  2. 否
         outHosStatus,----病人出院状况 Add by xlb 2013-07-03
       JBYNZZ,       ----疾病是否疑难杂症 Add by xlb 2013-07-03
         MZYCY,         --- 门诊与出院 Add by xlb 2013-07-03
       InAndOutHos,  --入院与出院
        LCYBL,         ---临床与病理
        FSYBL,          ---放射与病理
        qJCount,        ---抢救次数
        successCount,  ---成功次数
         InPatLY,  ----病人来源
         asaScore  --ASA分级评分
         )
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

         v_CURE_TYPE, ---- 治疗类别 □ 1.中医（ 1.1 中医   1.2民族医）    2.中西医     3.西医
         v_MZZYZD_NAME, ---- 门（急）诊诊断（中医诊断）
         v_MZZYZD_CODE, ---- 门（急）诊诊断（中医诊断） 编码
         v_MZXYZD_NAME, ---- 门（急）诊诊断（西医诊断）
         v_MZXYZD_CODE, ---- 门（急）诊诊断（西医诊断） 编码
         v_SSLCLJ, ---- 实施临床路径：□ 1. 中医  2. 西医  3 否
         v_ZYZJ, ---- 使用医疗机构中药制剂：□ 1.是  2. 否
         v_ZYZLSB, ---- 使用中医诊疗设备：□  1.是 2. 否
         v_ZYZLJS, ---- 使用中医诊疗技术：□ 1. 是  2. 否
         v_BZSH, ---- 辨证施护：□ 1.是  2. 否
         v_outHosStatus,
        v_JBYNZZ ,
        v_MZYCY ,
        v_InAndOutHos,
        v_LCYBL,
        v_FSYBL,
        v_qJCount,
        v_successCount,
        v_InPatLY,
        v_asaScore
         );

      open o_result for
        select seq_iem_mainpage_basicinfo_id.currval from dual;
      ---修改病案首页基本信息
    ELSIF v_edittype = '2' THEN

      update iem_mainpage_basicinfo_sx
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

             CURE_TYPE   = v_CURE_TYPE, ---- 治疗类别 □ 1.中医（ 1.1 中医   1.2民族医）    2.中西医     3.西医
             MZZYZD_NAME = v_MZZYZD_NAME, ---- 门（急）诊诊断（中医诊断）
             MZZYZD_CODE = v_MZZYZD_CODE, ---- 门（急）诊诊断（中医诊断） 编码
             MZXYZD_NAME = v_MZXYZD_NAME, ---- 门（急）诊诊断（西医诊断）
             MZXYZD_CODE = v_MZXYZD_CODE, ---- 门（急）诊诊断（西医诊断） 编码
             SSLCLJ      = v_SSLCLJ, ---- 实施临床路径：□ 1. 中医  2. 西医  3 否
             ZYZJ        = v_ZYZJ, ---- 使用医疗机构中药制剂：□ 1.是  2. 否
             ZYZLSB      = v_ZYZLSB, ---- 使用中医诊疗设备：□  1.是 2. 否
             ZYZLJS      = v_ZYZLJS, ---- 使用中医诊疗技术：□ 1. 是  2. 否
             BZSH        = v_BZSH, ---- 辨证施护：□ 1.是  2. 否
             outHosStatus=v_outHosStatus,
             JBYNZZ      = v_JBYNZZ ,
             MZYCY       = v_MZYCY ,
             InAndOutHos =v_InAndOutHos,
              LCYBL      = v_LCYBL,
               FSYBL=v_FSYBL,
        qJCount=v_qJCount,
        successCount=v_successCount,
         InPatLY=v_InPatLY,
         asaScore=v_asaScore
       where IEM_MAINPAGE_NO = v_IEM_MAINPAGE_NO;
      open o_result for
        select v_IEM_MAINPAGE_NO from dual;
    end if;

  END;

  /*****查询病案首页信息****************************************************************************/
  -----Modify by xlb 江西九江新增字段 2013-07-03
  PROCEDURE usp_getieminfo_sx(v_NoOfInpat INT,
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
      FROM iem_mainpage_basicinfo_sx imb
     WHERE imb.noofinpat = v_noofinpat
       AND imb.valide = 1;

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
             job.name         jobName,
             myinp.provinceid csd_provinceid,
             myinp.countyid   csd_cityid,

             myinp.districtid csd_districtid, --出生地县

             csdpro.provincename csd_provincename,
             csdcity.cityname    csd_cityname,
             csddis.districtname csd_districtname,
             myinp.xzzproviceid xzz_provinceid,

             myinp.xzzcityid     xzz_cityid,
             myinp.xzzdistrictid xzz_districtid,

             xzzpro.provincename xzz_provincename,
             xzzcity.cityname    xzz_cityname,
             xzzdis.districtname xzz_districtname,

             myinp.xzztel  xzz_tel,
             myinp.xzzpost xzz_post,

             myinp.hkdzproviceid  hkdz_provinceid,
             myinp.hkzdcityid     hkdz_cityid,
             myinp.hkzddistrictid hkdz_districtid,

             hkzzpro.provincename hkdz_provincename,
             hkzzcity.cityname    hkdz_cityname,
             hkzzdis.districtname hkdz_districtname,
             myinp.nativepost hkdz_post,

             myinp.nativeplace_p jg_provinceid,

             myinp.nativeplace_c jg_cityid,

             jgpro.provincename jg_provincename,
             jgcity.cityname    jg_cityname,
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

             myinp.admitdept,
             AdmitDept.name  AdmitDeptName,

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
             myinp.agestr age,

             iem.cure_type,
             iem.mzzyzd_name,
             iem.mzzyzd_code,
             iem.mzxyzd_name,
             iem.mzxyzd_code,
             iem.sslclj,
             iem.zyzj,

             iem.zyzlsb,
             iem.zyzljs,
             iem.bzsh,
             iem.outhosstatus,
             iem.jbynzz,
             iem.mzycy,
             iem.inandouthos,
             iem.lcybl,
             iem.fsybl,
             iem.qjcount,
             iem.successcount,
             iem.inpatly,
             iem.asascore,
             myinp.isbaby,
             myinp.mother

        FROM iem_mainpage_basicinfo_sx iem
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
             diag.diagnosis_code,
             diag.diagnosis_type_id,
             diag.order_value,
             diag.type,
             diag.typename
        FROM iem_mainpage_diagnosis_sx diag
       WHERE iem_mainpage_no = v_infono
         AND valide = 1
       ORDER BY order_value;

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
               when iem.operation_level = '1' then --operation_level  edit by ywk 2012年4月18日14:03:50
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
             iem.operintimes
        FROM iem_mainpage_operation_sx iem
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

      ---麻醉信息的修改 edit by ywk 2012年4月18日10:22:32
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

  /*********************************************************************************/
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
                                          v_OperInTimes VARCHAR2
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
    INSERT INTO iem_mainpage_operation_sx
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
       operintimes
       )
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
       v_OperInTimes
       );
  END;

  /*********************************************************************************/
  PROCEDURE usp_edif_iem_mainpage_diag_sx(v_iem_mainpage_no   VARCHAR,
                                          v_diagnosis_type_id VARCHAR,
                                          v_diagnosis_code    VARCHAR,
                                          v_diagnosis_name    VARCHAR,
                                          v_status_id         VARCHAR,
                                          v_order_value       VARCHAR,
                                          --v_Valide numeric ,
                                          v_create_user VARCHAR,
                                          v_type        varchar,
                                          v_typeName    varchar
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
    INSERT INTO iem_mainpage_diagnosis_sx
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
       type,
       typeName)
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
       v_type,
       v_typeName
       -- Create_Time - varchar(19)
       );
  END;

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
                                       v_isupdate       varchar2 default '' ---2012年5月24日 17:19:10 ywk 是否更新身份证号字段
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
             xzzpost        = v_xzzpost

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
             noofclinic     = mynoofclinic
       where NoOfInpat = v_NOOFINPAT;
      commit;
    end if;
    /*   open o_result for
    select v_IEM_MAINPAGE_NO from dual;*/
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
          on dia.icd = inp.clinicdiagnosis --add ywk 2012年6月15日 11:44:07
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
          on dia.icd = inp.clinicdiagnosis --add ywk 2012年6月15日 11:44:07
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
                                 v_VALID     varchar) as
    v_count integer;
  begin
    select count(*)
      into v_count
      from iem_mainpage_feeinfoZY
     where iem_mainpage_feeinfoZY.Id = v_id
       and iem_mainpage_feeinfoZY.Valid = '1';
    if v_count <= 0 then
      insert into iem_mainpage_feeinfoZY
        (Id,
         Noofinpat,
         Total,
         Ownfee,
         Ybylfy,
         Zybzlzf,
         Zybzlzhzf,
         Ybzlfy,
         Care,
         Zhqtfy,
         Blzdf,
         Syszdf,
         Yxxzdf,
         Lczdf,
         Fsszlf,
         Lcwlzlf,
         Sszlf,
         Mzf,
         Ssf,
         kff,
         Zyzdf,
         Zyzlf,
         Zywz,
         Zygs,
         Zcyjf,
         Zytnzl,
         Zygczl,
         Zytszl,
         Zyqt,
         Zytstpjg,
         Bzss,
         Xyf,
         Kjywf,
         Cpmedical,
         Yljgzyzjf,
         Cmedical,
         Bloodfee,
         Xdblzpf,
         Qdblzpf,
         Nxyzlzpf,
         Xbyzlzpf,
         Jcyycxclf,
         Zlyycxclf,
         Ssyycxclf,
         Otherfee,
         Valid)
      values
        (v_id,
         v_NOOFINPAT,
         v_TOTAL,
         v_OWNFEE,
         v_YBYLFY,
         v_ZYBZLZF,
         v_ZYBZLZHZF,
         v_YBZLFY,
         v_CARE,
         v_ZHQTFY,
         v_BLZDF,
         v_SYSZDF,
         v_YXXZDF,
         v_LCZDF,
         v_FSSZLF,
         v_LCWLZLF,
         v_SSZLF,
         v_MZF,
         v_SSF,
         v_KFF,
         v_ZYZDF,
         v_ZYZLF,
         v_ZYWZ,
         v_ZYGS,
         v_ZCYJF,
         v_ZYTNZL,
         v_ZYGCZL,
         v_ZYTSZL,
         v_ZYQT,
         v_ZYTSTPJG,
         v_BZSS,
         v_XYF,
         v_KJYWF,
         v_CPMEDICAL,
         v_YLJGZYZJF,
         v_CMEDICAL,
         v_BLOODFEE,
         v_XDBLZPF,
         v_QDBLZPF,
         v_NXYZLZPF,
         v_XBYZLZPF,
         v_JCYYCXCLF,
         v_ZLYYCXCLF,
         v_SSYYCXCLF,
         v_OTHERFEE,
         v_VALID);
    else
      update iem_mainpage_feeinfoZY
         set TOTAL     = v_TOTAL,
             OWNFEE    = v_OWNFEE,
             YBYLFY    = v_YBYLFY,
             ZYBZLZF   = v_ZYBZLZF,
             ZYBZLZHZF = v_ZYBZLZHZF,
             YBZLFY    = v_YBZLFY,
             CARE      = v_CARE,
             ZHQTFY    = v_ZHQTFY,
             BLZDF     = v_BLZDF,
             SYSZDF    = v_SYSZDF,
             YXXZDF    = v_YXXZDF,
             LCZDF     = v_LCZDF,
             FSSZLF    = v_FSSZLF,
             LCWLZLF   = v_LCWLZLF,
             SSZLF     = v_SSZLF,
             MZF       = v_MZF,
             SSF       = v_SSF,
             KFF       = v_KFF,
             ZYZDF     = v_ZYZDF,
             ZYZLF     = v_ZYZLF,
             ZYWZ      = v_ZYWZ,
             ZYGS      = v_ZYGS,
             ZCYJF     = v_ZCYJF,
             ZYTNZL    = v_ZYTNZL,
             ZYGCZL    = v_ZYGCZL,
             ZYTSZL    = v_ZYTSZL,
             ZYQT      = v_ZYQT,
             ZYTSTPJG  = v_ZYTSTPJG,
             BZSS      = v_BZSS,
             XYF       = v_XYF,
             KJYWF     = v_KJYWF,
             CPMEDICAL = v_CPMEDICAL,
             YLJGZYZJF = v_YLJGZYZJF,
             CMEDICAL  = v_CMEDICAL,
             BLOODFEE  = v_BLOODFEE,
             XDBLZPF   = v_XDBLZPF,
             QDBLZPF   = v_QDBLZPF,
             NXYZLZPF  = v_NXYZLZPF,
             XBYZLZPF  = v_XBYZLZPF,
             JCYYCXCLF = v_JCYYCXCLF,
             ZLYYCXCLF = v_ZLYYCXCLF,
             SSYYCXCLF = v_SSYYCXCLF,
             OTHERFEE  = v_OTHERFEE
       where id = v_id;
    end if;
  end;

  PROCEDURE usp_GetIemFeeZYbyInpat(v_noofinpat varchar,
                                   o_result    OUT empcurtyp) as
  begin
    OPEN o_result FOR
      select *
        from iem_mainpage_feeinfozy
       where iem_mainpage_feeinfozy.valid = '1' and iem_mainpage_feeinfozy.noofinpat=v_noofinpat;
  end;

END;
/
