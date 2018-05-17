CREATE OR REPLACE PACKAGE EMR_ZYMOSIS_REPORT IS
  TYPE empcurtyp IS REF CURSOR;

  /*
  * 插入传染病报告卡内容
  */
  PROCEDURE usp_EditZymosis_Report(v_EditType         varchar,
                                   v_Report_ID        NUMERIC DEFAULT 0,
                                   v_Report_NO        varchar DEFAULT '', --报告卡卡号
                                   v_Report_Type      varchar default '', --报告卡类型   1、初次报告  2、订正报告
                                   v_Noofinpat        varchar default '', --首页序号
                                   v_PatID            varchar default '', --住院号
                                   v_Name             varchar default '', --患者姓名
                                   v_ParentName       varchar default '', --家长姓名
                                   v_IDNO             varchar default '', --身份证号码
                                   v_Sex              varchar default '', --患者性别
                                   v_Birth            varchar default '', --出生日期
                                   v_Age              varchar default '', --实足年龄
                                   v_AgeUnit          varchar default '', --实足年龄单位
                                   v_Organization     varchar default '', --工作单位
                                   v_OfficePlace      varchar default '', --单位地址
                                   v_OfficeTEL        varchar default '', --单位电话
                                   v_AddressType      varchar default '', --病人属于地区  1、本县区 2、本市区其他县区  3、本省其他地区  4、外省  5、港澳台  6、外籍
                                   v_HomeTown         varchar default '', --家乡
                                   v_Address          varchar default '', --详细地址[村 街道 门牌号]
                                   v_JobID            varchar default '', --职业代码（按页面顺序记录编号）
                                   v_RecordType1      varchar default '', --病历分类  1、疑似病历  2、临床诊断病历  3、实验室确诊病历  4病原携带者
                                   v_RecordType2      varchar default '', --病历分类（乙型肝炎、血吸虫病填写）  1、急性  2、慢性
                                   v_AttackDate       varchar default '', --发病日期（病原携带者填初检日期或就诊日期）
                                   v_DiagDate         varchar default '', --诊断日期
                                   v_DieDate          varchar default '', --死亡日期
                                   v_DiagICD10        varchar default '', --传染病病种(对应传染病诊断库)
                                   v_DiagName         varchar default '', --传染病病种名称
                                   v_INFECTOTHER_FLAG varchar default '', --有无感染其他人[0无 1有]
                                   v_Memo             varchar default '', --备注
                                   v_Correct_flag     varchar default '', --订正标志【0、未订正 1、已订正】
                                   v_Correct_Name     varchar default '', --订正病名
                                   v_Cancel_Reason    varchar default '', --退卡原因
                                   v_ReportDeptCode   varchar default '', --报告科室编号
                                   v_ReportDeptName   varchar default '', --报告科室名称
                                   v_ReportDocCode    varchar default '', --报告医生编号
                                   v_ReportDocName    varchar default '', --报告医生名称
                                   v_DoctorTEL        varchar default '', --报告医生联系电话
                                   v_Report_Date      varchar default '', --填卡时间
                                   v_State            varchar default '', --报告状态【 1、新增保存 2、提交 3、撤回 4、审核通过 5、审核未通过撤回 6、上报  7、作废】
                                   v_StateName        varchar default '', --操作状态，方便记录操作流水中
                                   v_create_date      varchar default '', --创建时间
                                   v_create_UserCode  varchar default '', --创建人
                                   v_create_UserName  varchar default '', --创建人
                                   v_create_deptCode  varchar default '', --创建人科室
                                   v_create_deptName  varchar default '', --创建人科室
                                   v_Modify_date      varchar default '', --修改时间
                                   v_Modify_UserCode  varchar default '', --修改人
                                   v_Modify_UserName  varchar default '', --修改人
                                   v_Modify_deptCode  varchar default '', --修改人科室
                                   v_Modify_deptName  varchar default '', --修改人科室
                                   v_Audit_date       varchar default '', --审核时间
                                   v_Audit_UserCode   varchar default '', --审核人
                                   v_Audit_UserName   varchar default '', --审核人
                                   v_Audit_deptCode   varchar default '', --审核人科室
                                   v_Audit_deptName   varchar default '', --审核人科室
                                   v_OtherDiag        varchar default '',
                                   o_result           OUT empcurtyp);

  PROCEDURE usp_GetInpatientByNofinpat(v_Noofinpat varchar default '', --首页序号
                                       o_result    OUT empcurtyp);

  PROCEDURE usp_geteditzymosisreport(v_report_type1    varchar default '',
                                     v_report_type2    varchar default '',
                                     v_name            varchar default '',
                                     v_patid           varchar default '',
                                     v_deptid          varchar default '',
                                     v_applicant       varchar default '',
                                     v_status          varchar default '',
                                     v_createdatestart varchar default '', --新增的上报日期开始
                                     v_createdateend   varchar default '', --新增的上报日期结束
                                     v_querytype       varchar default '', --查询类型
                                     o_result          OUT empcurtyp);

  PROCEDURE usp_getReportBrowse(v_report_type1 varchar default '',
                                v_report_type2 varchar default '',
                                v_recordtype1  varchar default '',
                                v_beginDate    varchar default '',
                                v_EndDate      varchar default '',
                                v_deptid       varchar default '',
                                v_diagICD      varchar default '',
                                v_status       varchar default '',
                                o_result       OUT empcurtyp);

  PROCEDURE usp_GetReportAnalyse(v_beginDate varchar default '',
                                 v_EndDate   varchar default '',
                                 o_result    OUT empcurtyp);

  --分职业统计传染病信息
  PROCEDURE usp_GetJobDisease(v_beginDate varchar default '',
                              v_EndDate   varchar default '',
                              v_DiagCode  varchar default '',
                              o_result    OUT empcurtyp);

  --得到所有有效的诊断
  PROCEDURE usp_GetDiagnosis(o_result OUT empcurtyp);

  --得到传染病病种信息
  PROCEDURE usp_GetDisease2(o_result OUT empcurtyp);

  --得到所有有效的诊断
  PROCEDURE usp_GetDiagnosisTo(v_categoryid varchar default '',
                               o_result     OUT empcurtyp);

  --得到所有有效的诊断
  PROCEDURE usp_GetDiagnosisTo_ZY(v_categoryid varchar default '',
                                  o_result     OUT empcurtyp);

  --得到传染病病种信息
  PROCEDURE usp_GetDisease2To(v_categoryid varchar default '',
                              o_result     OUT empcurtyp);

  --保存病种记录
  PROCEDURE usp_SaveZymosisDiagnosis(v_markid   varchar default '',
                                     v_icd      varchar default '',
                                     v_name     varchar default '',
                                     v_py       varchar default '',
                                     v_wb       varchar default '',
                                     v_levelID  varchar default '',
                                     v_valid    varchar default '',
                                     v_statist  varchar default '',
                                     v_memo     varchar default '',
                                     v_namestr  varchar default '',
                                     v_upcount  integer,
                                     v_fukatype varchar default '');
  --保存病种记录
  PROCEDURE usp_SaveZymosisDiagnosisTo(v_markid     varchar default '',
                                       v_icd        varchar default '',
                                       v_name       varchar default '',
                                       v_py         varchar default '',
                                       v_wb         varchar default '',
                                       v_levelID    varchar default '',
                                       v_valid      varchar default '',
                                       v_statist    varchar default '',
                                       v_memo       varchar default '',
                                       v_namestr    varchar default '',
                                       v_upcount    integer,
                                       v_categoryid integer);

  PROCEDURE usp_EditTherioma_Report(v_EditType              varchar,
                                    v_Report_ID             NUMERIC DEFAULT 0,
                                    v_REPORT_DISTRICTID     varchar DEFAULT '', --传染病上报卡表区(县)编码
                                    v_REPORT_DISTRICTNAME   varchar default '', --传染病上报卡表区(县)名称
                                    v_REPORT_ICD10          varchar default '', --传染病报告卡ICD-10编码
                                    v_REPORT_ICD0           varchar default '', --传染病报告卡ICD-0编码
                                    v_REPORT_CLINICID       varchar default '', --门诊号
                                    v_REPORT_PATID          varchar default '', --住院号
                                    v_REPORT_INDO           varchar default '', --身份证号码
                                    v_REPORT_INPATNAME      varchar default '', --病患姓名
                                    v_SEXID                 varchar default '', --病患性别
                                    v_REALAGE               varchar default '', --病患实足年龄
                                    v_BIRTHDATE             varchar default '', --病患生日
                                    v_NATIONID              varchar default '', --病患民族编号
                                    v_NATIONNAME            varchar default '', --病患民族全称
                                    v_CONTACTTEL            varchar default '', --家庭电话
                                    v_MARTIAL               varchar default '', --婚姻状况
                                    v_OCCUPATION            varchar default '', --病患职业
                                    v_OFFICEADDRESS         varchar default '', --工作单位地址
                                    v_ORGPROVINCEID         varchar default '', --户口地址省份编码
                                    v_ORGCITYID             varchar default '', --户口地址所在市编码
                                    v_ORGDISTRICTID         varchar default '', --户口所在地区县编码
                                    v_ORGTOWNID             varchar default '', --户口所在地镇(街道)编码
                                    v_ORGVILLIAGE           varchar default '', --户口所在地居委会对应编码
                                    v_ORGPROVINCENAME       varchar default '', --户口所在地省份全称
                                    v_ORGCITYNAME           varchar default '', --户口所在地市全名称
                                    v_ORGDISTRICTNAME       varchar default '', --户口所在地区(县)全称
                                    v_ORGTOWN               varchar default '', --户口所在地镇全称
                                    v_ORGVILLAGENAME        varchar default '', --户口所在地村全称
                                    v_XZZPROVINCEID         varchar default '', --现住址所在省份编码
                                    v_XZZCITYID             varchar default '', --现住址所在市编码
                                    v_XZZDISTRICTID         varchar default '', --现住址所在区(县)代码
                                    v_XZZTOWNID             varchar default '', --现住址所在镇代码
                                    v_XZZVILLIAGEID         varchar default '', --报现住址所在村编码
                                    v_XZZPROVINCE           varchar default '', --现住址所在省份全称
                                    v_XZZCITY               varchar default '', --现住址所在市全称
                                    v_XZZDISTRICT           varchar default '', --现住址所在区全称
                                    v_XZZTOWN               varchar default '', --现住址所在镇全称
                                    v_XZZVILLIAGE           varchar default '', --现住址所在村全称
                                    v_REPORT_DIAGNOSIS      varchar default '', --诊断
                                    v_PATHOLOGICALTYPE      varchar default '', --病理类型
                                    v_PATHOLOGICALID        varchar default '', --病理诊断病理号
                                    v_QZDIAGTIME_T          varchar default '', --确诊时期_T期
                                    v_QZDIAGTIME_N          varchar default '', --确诊时期_N期
                                    v_QZDIAGTIME_M          varchar default '', --确诊时期_M期
                                    v_FIRSTDIADATE          varchar default '', --首次确诊时间
                                    v_REPORTINFUNIT         varchar default '', --报告单位
                                    v_REPORTDOCTOR          varchar default '', --报告医生
                                    v_REPORTDATE            varchar default '', --报告时间
                                    v_DEATHDATE             varchar default '', --死亡时间
                                    v_DEATHREASON           varchar default '', --死亡原因
                                    v_REJEST                varchar default '', --病历摘要
                                    v_REPORT_YDIAGNOSIS     varchar default '', --原诊断
                                    v_REPORT_YDIAGNOSISDATA varchar default '', --原诊断日期
                                    v_REPORT_DIAGNOSISBASED varchar default '', --诊断依据
                                    v_REPORT_NO             varchar default '', --传染病上报卡表编号
                                    v_REPORT_NOOFINPAT      varchar default '', --患者ID
                                    v_STATE                 varchar default '', --报告状态【 1、新增保存 2、提交 3、撤回 4、审核通过 5、审核未通过撤回 6、上报  7、作废】
                                    v_CREATE_DATE           varchar default '', --创建人日期  
                                    v_CREATE_USERCODE       varchar default '', --创建人
                                    v_CREATE_USERNAME       varchar default '', --创建人
                                    v_CREATE_DEPTCODE       varchar default '', --创建人科室
                                    v_CREATE_DEPTNAME       varchar default '', --创建人科室
                                    v_MODIFY_DATE           varchar default '', --修改时间
                                    v_MODIFY_USERCODE       varchar default '', --修改人
                                    v_MODIFY_USERNAME       varchar default '', --修改人
                                    v_MODIFY_DEPTCODE       varchar default '', --修改人科室
                                    v_MODIFY_DEPTNAME       varchar default '', --修改人科室
                                    v_AUDIT_DATE            varchar default '', --审核时间
                                    v_AUDIT_USERCODE        varchar default '', --审核人
                                    v_AUDIT_USERNAME        varchar default '', --审核人
                                    v_AUDIT_DEPTCODE        varchar default '', --审核人科室
                                    v_AUDIT_DEPTNAME        varchar default '', --审核人科室
                                    v_VAILD                 varchar default '', --状态是否有效  1、有效   0、无效
                                    v_DIAGICD10             varchar default '', --传染病病种(对应传染病诊断库)
                                    v_CANCELREASON          varchar default '', --否决原因   
                                    V_CARDTYPE              varchar default '', --卡片类型死亡或者发病                     
                                    V_clinicalstages        varchar default '',
                                    V_ReportDiagfunit       varchar default '',
                                    o_result                OUT empcurtyp);

  --脑卒中报告卡  add  by  ywk 2013年8月21日 15:47:00
  PROCEDURE usp_EditCardiovacular_Report(v_EditType      varchar, --操作类型
                                         v_REPORT_NO     varchar DEFAULT '', --报告卡卡号                                 
                                         v_NOOFCLINIC    varchar DEFAULT '', --门诊号 
                                         v_PATID         varchar default '',
                                         v_NAME          varchar default '',
                                         v_IDNO          varchar default '',
                                         v_SEXID         varchar default '',
                                         v_BIRTH         varchar default '',
                                         v_AGE           varchar DEFAULT '',
                                         v_NATIONID      varchar default '',
                                         v_JOBID         varchar default '',
                                         v_OFFICEPLACE   varchar default '',
                                         v_CONTACTTEL    varchar default '',
                                         v_HKPROVICE     varchar default '',
                                         v_HKCITY        varchar default '',
                                         v_HKSTREET      varchar default '',
                                         v_HKADDRESSID   varchar default '',
                                         v_XZZPROVICE    varchar default '',
                                         v_XZZCITY       varchar default '',
                                         v_XZZSTREET     varchar default '',
                                         v_XZZADDRESSID  varchar default '',
                                         v_XZZCOMMITTEES varchar default '',
                                         v_XZZPARM         varchar default '',
                                         v_ICD             varchar default '',
                                         v_DIAGZWMXQCX     varchar default '',
                                         v_DIAGNCX         varchar default '',
                                         v_DIAGNGS         varchar default '',
                                         v_DIAGWFLNZZ      varchar default '',
                                         v_DIAGJXXJGS      varchar default '',
                                         v_DIAGXXCS        varchar default '',
                                         v_DIAGNOSISBASED  varchar default '',
                                         v_DIAGNOSEDATE    varchar default '',
                                         v_ISFIRSTSICK     varchar default '',
                                         v_DIAGHOSPITAL    varchar default '',
                                         v_OUTFLAG         varchar default '',
                                         v_DIEDATE         varchar default '',
                                         v_REPORTDEPT      varchar default '',
                                         v_REPORTUSERCODE  varchar default '',
                                         v_REPORTUSERNAME  varchar default '',
                                         v_REPORTDATE      varchar default '',
                                         v_CREATE_DATE     varchar default '',
                                         v_CREATE_USERCODE varchar default '',
                                         v_CREATE_USERNAME varchar default '',
                                         v_CREATE_DEPTCODE varchar default '',
                                         v_CREATE_DEPTNAME varchar default '',
                                         v_MODIFY_DATE     varchar default '',
                                         v_MODIFY_USERCODE varchar default '',
                                         v_MODIFY_USERNAME varchar default '',
                                         v_MODIFY_DEPTCODE varchar default '',
                                         v_MODIFY_DEPTNAME varchar default '',
                                         v_AUDIT_DATE      varchar default '',
                                         v_AUDIT_USERCODE  varchar default '',
                                         v_AUDIT_USERNAME  varchar default '',
                                         v_AUDIT_DEPTCODE  varchar default '',
                                         v_AUDIT_DEPTNAME  varchar default '',
                                         v_VAILD           varchar default '',
                                         v_CANCELREASON    varchar default '',
                                         v_STATE           varchar default '',
                                         v_CARDPARAM1      varchar default '',
                                         v_CARDPARAM2      varchar default '',
                                         v_CARDPARAM3      varchar default '',
                                         v_CARDPARAM4      varchar default '',
                                         v_CARDPARAM5      varchar default '',
                                         v_NOOFINPAT       varchar default '',
                                           v_REPORTID      varchar DEFAULT '',
                                         o_result          OUT empcurtyp);

  --出生儿缺陷报告卡  add  by  jxh  2013-08-16   暂时放这
  PROCEDURE usp_EditTbirthdefects_Report(v_EditType         varchar,
                                         v_ID               NUMERIC DEFAULT 0,
                                         v_REPORT_NOOFINPAT varchar DEFAULT '', --病人编号
                                         v_REPORT_ID        varchar default '', --报告卡序号
                                         v_DIAG_CODE        varchar default '', --报告卡诊断编码
                                         v_STRING3          varchar default '', --预留
                                         v_STRING4          varchar default '', --预留
                                         v_STRING5          varchar default '', --预留
                                         v_REPORT_PROVINCE  varchar DEFAULT '', --上报告卡省份
                                         v_REPORT_CITY      varchar default '', --报告卡市（县）
                                         v_REPORT_TOWN      varchar default '', --报告卡乡镇
                                         v_REPORT_VILLAGE   varchar default '', --报告卡村
                                         v_REPORT_HOSPITAL  varchar default '', --报告卡医院
                                         v_REPORT_NO        varchar default '', --报告卡序号
                                         v_MOTHER_PATID     varchar default '', --产妇住院号
                                         v_MOTHER_NAME      varchar default '', --姓名
                                         v_MOTHER_AGE       varchar default '', --年龄
                                         v_NATIONAL         varchar default '', --民族
                                         v_ADDRESS_POST     varchar default '', --地址and邮编
                                         v_PREGNANTNO       varchar default '', --孕次
                                         v_PRODUCTIONNO     varchar default '', --产次
                                         v_LOCALADD         varchar default '', --常住地
                                         
                                         v_PERCAPITAINCOME         varchar default '', --年人均收入     
                                         v_EDUCATIONLEVEL          varchar default '', --文化程度     
                                         v_CHILD_PATID             varchar default '', --孩子住院号     
                                         v_CHILD_NAME              varchar default '', --孩子姓名     
                                         v_ISBORNHERE              varchar default '', --是否本院出生     
                                         v_CHILD_SEX               varchar default '', --孩子性别      
                                         v_BORN_YEAR               varchar default '', --出生年     
                                         v_BORN_MONTH              varchar default '', --  出生月     
                                         v_BORN_DAY                varchar default '', --出生日      
                                         v_GESTATIONALAGE          varchar default '', --胎龄     
                                         v_WEIGHT                  varchar default '', --体重     
                                         v_BIRTHS                  varchar default '', --胎数     
                                         v_ISIDENTICAL             varchar default '', --是否同卵      
                                         v_OUTCOME                 varchar default '', --转归      
                                         v_INDUCEDLABOR            varchar default '', --是否引产     
                                         v_DIAGNOSTICBASIS         varchar default '', --诊断依据——临床      
                                         v_DIAGNOSTICBASIS1        varchar default '', --诊断依据——超声波      
                                         v_DIAGNOSTICBASIS2        varchar default '', --诊断依据——尸解     
                                         v_DIAGNOSTICBASIS3        varchar default '', --诊断依据——生化检查     
                                         v_DIAGNOSTICBASIS4        varchar default '', --诊断依据——生化检查——其他     
                                         v_DIAGNOSTICBASIS5        varchar default '', --诊断依据——染色体      
                                         v_DIAGNOSTICBASIS6        varchar default '', --诊断依据——其他     
                                         v_DIAGNOSTICBASIS7        varchar default '', --诊断依据——其他——内容     
                                         v_DIAG_ANENCEPHALY        varchar default '', --出生缺陷诊断——无脑畸形     
                                         v_DIAG_SPINA              varchar default '', --出生缺陷诊断——脊柱裂      
                                         v_DIAG_PENGOUT            varchar default '', --出生缺陷诊断——脑彭出      
                                         v_DIAG_HYDROCEPHALUS      varchar default '', --出生缺陷诊断——先天性脑积水     
                                         v_DIAG_CLEFTPALATE        varchar default '', --出生缺陷诊断——腭裂     
                                         v_DIAG_CLEFTLIP           varchar default '', --出生缺陷诊断——唇裂      
                                         v_DIAG_CLEFTMERGER        varchar default '', --出生缺陷诊断——唇裂合并腭裂     
                                         v_DIAG_SMALLEARS          varchar default '', --出生缺陷诊断——小耳（包括无耳）     
                                         v_DIAG_OUTEREAR           varchar default '', --出生缺陷诊断——外耳其它畸形（小耳、无耳除外）     
                                         v_DIAG_ESOPHAGEAL         varchar default '', --出生缺陷诊断——食道闭锁或狭窄     
                                         v_DIAG_RECTUM             varchar default '', --出生缺陷诊断——直肠肛门闭锁或狭窄（包括无肛）     
                                         v_DIAG_HYPOSPADIAS        varchar default '', --出生缺陷诊断——尿道下裂     
                                         v_DIAG_BLADDER            varchar default '', --出生缺陷诊断——膀胱外翻     
                                         v_DIAG_HORSESHOEFOOTLEFT  varchar default '', --出生缺陷诊断——马蹄内翻足_左      
                                         v_DIAG_HORSESHOEFOOTRIGHT varchar default '', --出生缺陷诊断——马蹄内翻足_右     
                                         v_DIAG_MANYPOINTLEFT      varchar default '', --出生缺陷诊断——多指（趾）_左      
                                         v_DIAG_MANYPOINTRIGHT     varchar default '', --出生缺陷诊断——多指（趾）_右     
                                         v_DIAG_LIMBSUPPERLEFT     varchar default '', --出生缺陷诊断——肢体短缩_上肢 _左      
                                         v_DIAG_LIMBSUPPERRIGHT    varchar default '', --出生缺陷诊断——肢体短缩_上肢 _右     
                                         v_DIAG_LIMBSLOWERLEFT     varchar default '', --出生缺陷诊断——肢体短缩_下肢 _左      
                                         v_DIAG_LIMBSLOWERRIGHT    varchar default '', --出生缺陷诊断——肢体短缩_下肢 _右     
                                         v_DIAG_HERNIA             varchar default '', --出生缺陷诊断——先天性膈疝     
                                         v_DIAG_BULGINGBELLY       varchar default '', --出生缺陷诊断——脐膨出     
                                         v_DIAG_GASTROSCHISIS      varchar default '', --出生缺陷诊断——腹裂     
                                         v_DIAG_TWINS              varchar default '', --出生缺陷诊断——联体双胎     
                                         v_DIAG_TSSYNDROME         varchar default '', --出生缺陷诊断——唐氏综合征（21-三体综合征）     
                                         v_DIAG_HEARTDISEASE       varchar default '', --出生缺陷诊断——先天性心脏病（类型）      
                                         v_DIAG_OTHER              varchar default '', --出生缺陷诊断——其他（写明病名或详细描述）      
                                         v_DIAG_OTHERCONTENT       varchar default '', --出生缺陷诊断——其他内容      
                                         v_FEVER                   varchar default '', --发烧（＞38℃）      
                                         v_VIRUSINFECTION          varchar default '', --病毒感染     
                                         v_ILLOTHER                varchar default '', --患病其他     
                                         v_SULFA                   varchar default '', --磺胺类     
                                         v_ANTIBIOTICS             varchar default '', --抗生素     
                                         v_BIRTHCONTROLPILL        varchar default '', --避孕药      
                                         v_SEDATIVES               varchar default '', --镇静药     
                                         v_MEDICINEOTHER           varchar default '', --服药其他      
                                         v_DRINKING                varchar default '', --饮酒     
                                         v_PESTICIDE               varchar default '', --农药      
                                         v_RAY                     varchar default '', --射线      
                                         v_CHEMICALAGENTS          varchar default '', --化学制剂     
                                         v_FACTOROTHER             varchar default '', --其他有害因素      
                                         v_STILLBIRTHNO            varchar default '', --死胎例数     
                                         v_ABORTIONNO              varchar default '', --自然流产例数     
                                         v_DEFECTSNO               varchar default '', --缺陷儿例数     
                                         v_DEFECTSOF1              varchar default '', --缺陷名1     
                                         v_DEFECTSOF2              varchar default '', --缺陷名2     
                                         v_DEFECTSOF3              varchar default '', --缺陷名3     
                                         v_YCDEFECTSOF1            varchar default '', --遗传缺陷名1     
                                         v_YCDEFECTSOF2            varchar default '', --遗传缺陷名2     
                                         v_YCDEFECTSOF3            varchar default '', --遗传缺陷名3     
                                         v_KINSHIPDEFECTS1         varchar default '', --与缺陷儿亲缘关系1     
                                         v_KINSHIPDEFECTS2         varchar default '', --与缺陷儿亲缘关系2     
                                         v_KINSHIPDEFECTS3         varchar default '', --与缺陷儿亲缘关系3     
                                         v_COUSINMARRIAGE          varchar default '', --近亲婚配史      
                                         v_COUSINMARRIAGEBETWEEN   varchar default '', --近亲婚配史关系     
                                         v_PREPARER                varchar default '', --填表人      
                                         v_THETITLE1               varchar default '', --填表人职称     
                                         v_FILLDATEYEAR            varchar default '', --填表日期年      
                                         v_FILLDATEMONTH           varchar default '', --填表日期月     
                                         v_FILLDATEDAY             varchar default '', --填表日期日     
                                         v_HOSPITALREVIEW          varchar default '', --医院审表人      
                                         v_THETITLE2               varchar default '', --医院审表人职称     
                                         v_HOSPITALAUDITDATEYEAR   varchar default '', --医院审表日期年     
                                         v_HOSPITALAUDITDATEMONTH  varchar default '', --医院审表日期月      
                                         v_HOSPITALAUDITDATEDAY    varchar default '', --医院审表日期日      
                                         v_PROVINCIALVIEW          varchar default '', --省级审表人      
                                         v_THETITLE3               varchar default '', --省级审表人职称     
                                         v_PROVINCIALVIEWDATEYEAR  varchar default '', --省级审表日期年      
                                         v_PROVINCIALVIEWDATEMONTH varchar default '', --省级审表日期月     
                                         v_PROVINCIALVIEWDATEDAY   varchar default '', --省级审表日期日     
                                         v_FEVERNO                 varchar default '', --发烧度数      
                                         v_ISVIRUSINFECTION        varchar default '', --是否病毒感染     
                                         v_ISDIABETES              varchar default '', --是否糖尿病      
                                         v_ISILLOTHER              varchar default '', --是否患病其他     
                                         v_ISSULFA                 varchar default '', --是否磺胺类     
                                         v_ISANTIBIOTICS           varchar default '', --是否抗生素     
                                         v_ISBIRTHCONTROLPILL      varchar default '', --是否避孕药      
                                         v_ISSEDATIVES             varchar default '', --是否镇静药     
                                         v_ISMEDICINEOTHER         varchar default '', --是否服药其他      
                                         v_ISDRINKING              varchar default '', --是否饮酒     
                                         v_ISPESTICIDE             varchar default '', --是否农药      
                                         v_ISRAY                   varchar default '', --是否射线      
                                         v_ISCHEMICALAGENTS        varchar default '', --是否化学制剂     
                                         v_ISFACTOROTHER           varchar default '', --是否其他有害因素      
                                         v_STATE                   varchar default '', -- "报告状态【 1、新增保存 2、提交 3、撤回 4、?to open this dialog next """     
                                         v_CREATE_DATE             varchar default '', --创建时间      
                                         v_CREATE_USERCODE         varchar default '', --创建人     
                                         v_CREATE_USERNAME         varchar default '', ---创建人      
                                         v_CREATE_DEPTCODE         varchar default '', --创建人科室     
                                         v_CREATE_DEPTNAME         varchar default '', --创建人科室     
                                         v_MODIFY_DATE             varchar default '', --修改时间      
                                         v_MODIFY_USERCODE         varchar default '', --修改人     
                                         v_MODIFY_USERNAME         varchar default '', --修改人     
                                         v_MODIFY_DEPTCODE         varchar default '', --修改人科室     
                                         v_MODIFY_DEPTNAME         varchar default '', --修改人科室     
                                         v_AUDIT_DATE              varchar default '', --审核时间     
                                         v_AUDIT_USERCODE          varchar default '', --审核人      
                                         v_AUDIT_USERNAME          varchar default '', --审核人      
                                         v_AUDIT_DEPTCODE          varchar default '', --审核人科室      
                                         v_AUDIT_DEPTNAME          varchar default '', --审核人科室      
                                         v_VAILD                   varchar default '', --状态是否有效  1、有效   0、无效     
                                         v_CANCELREASON            varchar default '', --否决原因     
                                         v_PRENATAL                varchar default '', --产前     
                                         v_PRENATALNO              varchar default '', --产前周数     
                                         v_POSTPARTUM              varchar default '', --产后     
                                         v_ANDTOSHOWLEFT           varchar default '', --并指左     
                                         v_ANDTOSHOWRIGHT          varchar default '', --并指右
                                         o_result                  OUT empcurtyp);

  --报表部分---肿瘤登记月报表 add by ywk 2013年7月31日 14:59:19
  PROCEDURE usp_GetTheriomaReportBYMonth( --v_searchtype varchar default '',--增加此字段主要后期为i区分中心医院及其他的查询
                                         v_year          varchar default '', --年
                                         v_month         varchar default '', --月
                                         v_deptcode      varchar default '', --科室编码
                                         v_diagstartdate varchar default '', --诊断开始时间
                                         v_diagenddate   varchar default '', --诊断结束时间
                                         o_result        OUT empcurtyp);

  --报表部分---恶性肿瘤新发病例登记簿 add by ywk 2013年8月2日 11:29:02
  PROCEDURE usp_GetTheriomaReportBYNew( --v_searchtype varchar default '',--增加此字段主要后期为i区分中心医院及其他的查询
                                       v_year   varchar default '', --年
                                       v_month  varchar default '', --月
                                       o_result OUT empcurtyp);

  --报表部分---恶性肿瘤死亡病例登记簿 add by ywk 2013年8月2日 11:29:02
  PROCEDURE usp_GetTheriomaReportBYDead( --v_searchtype varchar default '',--增加此字段主要后期为i区分中心医院及其他的查询
                                        v_year   varchar default '', --年
                                        v_month  varchar default '', --月
                                        o_result OUT empcurtyp);

  --报表部分---肿瘤登记月报表 add by ywk 2013年8月5日 11:32:52中心医院
  PROCEDURE usp_GetTheriomaReportBYMonthZX( --v_searchtype varchar default '',--增加此字段主要后期为i区分中心医院及其他的查询
                                           v_year          varchar default '', --年
                                           v_month         varchar default '', --月
                                           v_deptcode      varchar default '', --科室编码
                                           v_diagstartdate varchar default '', --诊断开始时间
                                           v_diagenddate   varchar default '', --诊断结束时间
                                           o_result        OUT empcurtyp);
                                           
   --报表部分---肿瘤登记月报表 add by jxh 2013年9月12日 14:05:52中心医院
  PROCEDURE usp_CardiovascularReport( --v_searchtype varchar default '',--增加此字段主要后期为i区分中心医院及其他的查询
                                           v_year          varchar default '', --年
                                           v_month         varchar default '', --月
                                           v_deptcode      varchar default '', --科室编码
                                           v_diagstartdate varchar default '', --诊断开始时间
                                           v_diagenddate   varchar default '', --诊断结束时间
                                           o_result        OUT empcurtyp);

  --保存或修改艾滋病报表
  --保存或修改艾滋病报表
  PROCEDURE usp_AddOrModHIVReport(v_HIVID               varchar2,
                                  v_REPORTID            integer,
                                  v_REPORTNO            varchar2,
                                  v_MARITALSTATUS       varchar2,
                                  v_NATION              varchar2,
                                  v_CULTURESTATE        varchar2,
                                  v_HOUSEHOLDSCOPE      varchar2,
                                  v_HOUSEHOLDADDRESS    varchar2,
                                  v_ADDRESS             varchar2,
                                  v_CONTACTHISTORY      varchar2,
                                  v_VENERISMHISTORY     varchar2,
                                  v_INFACTWAY           varchar2,
                                  v_SAMPLESOURCE        varchar2,
                                  v_DETECTIONCONCLUSION varchar2,
                                  v_AFFIRMDATE          varchar2,
                                  v_AFFIRMLOCAL         varchar2,
                                  v_DIAGNOSEDATE        varchar2,
                                  v_DOCTOR              varchar2,
                                  v_WRITEDATE           varchar2,
                                  v_ALIKESYMBOL         varchar2,
                                  v_REMARK              varchar2,
                                  v_VAILD               varchar2,
                                  v_CREATOR             varchar2,
                                  v_CREATEDATE          varchar2,
                                  v_MENDER              varchar2,
                                  v_ALTERDATE           varchar2);

  --保存或修改沙眼衣原体感染
  PROCEDURE usp_AddOrModSYYYTReport(v_SZDYYTID            varchar,
                                    v_REPORTID            integer,
                                    v_REPORTNO            varchar,
                                    v_MARITALSTATUS       varchar,
                                    v_NATION              varchar,
                                    v_CULTURESTATE        varchar,
                                    v_HOUSEHOLDSCOPE      varchar,
                                    v_HOUSEHOLDADDRESS    varchar,
                                    v_ADDRESS             varchar,
                                    v_SYYYTGR             varchar,
                                    v_CONTACTHISTORY      varchar,
                                    v_VENERISMHISTORY     varchar,
                                    v_INFACTWAY           varchar,
                                    v_SAMPLESOURCE        varchar,
                                    v_DETECTIONCONCLUSION varchar,
                                    v_AFFIRMDATE          varchar,
                                    v_AFFIRMLOCAL         varchar,
                                    v_VAILD               varchar,
                                    v_CREATOR             varchar,
                                    v_CREATEDATE          varchar,
                                    v_MENDER              varchar,
                                    v_ALTERDATE           varchar);

  --保存或修改乙肝报告表
  PROCEDURE usp_AddOrModHBVReport(v_HBVID varchar2,
                                  
                                  v_REPORTID      integer,
                                  v_HBSAGDATE     varchar2,
                                  v_FRISTDATE     varchar2,
                                  v_ALT           varchar2,
                                  v_ANTIHBC       varchar2,
                                  v_LIVERBIOPSY   varchar2,
                                  v_RECOVERYHBSAG varchar2,
                                  
                                  v_VAILD      varchar2,
                                  v_CREATOR    varchar2,
                                  v_CREATEDATE varchar2,
                                  v_MENDER     varchar2,
                                  v_ALTERDATE  varchar2);

  -- -保存或修改尖锐湿疣项目
  PROCEDURE usp_AddOrModJRSYReport(v_JRSY_ID             varchar,
                                   v_REPORTID            integer,
                                   v_REPORTNO            varchar,
                                   v_MARITALSTATUS       varchar,
                                   v_NATION              varchar,
                                   v_CULTURESTATE        varchar,
                                   v_HOUSEHOLDSCOPE      varchar,
                                   v_HOUSEHOLDADDRESS    varchar,
                                   v_ADDRESS             varchar,
                                   v_CONTACTHISTORY      varchar,
                                   v_VENERISMHISTORY     varchar,
                                   v_INFACTWAY           varchar,
                                   v_SAMPLESOURCE        varchar,
                                   v_DETECTIONCONCLUSION varchar,
                                   v_AFFIRMDATE          varchar,
                                   v_AFFIRMLOCAL         varchar,
                                   v_VAILD               varchar,
                                   v_CREATOR             varchar,
                                   v_CREATEDATE          varchar,
                                   v_MENDER              varchar,
                                   v_ALTERDATE           varchar);
  --保存或修改甲型H1N1流感报表
  PROCEDURE usp_AddOrModH1N1Report(v_H1N1ID         varchar2,
                                   v_REPORTID       integer,
                                   v_CASETYPE       varchar2,
                                   v_HOSPITALSTATUS varchar2,
                                   v_ISCURE         varchar2,
                                   v_ISOVERSEAS     varchar2,
                                   v_VAILD          varchar2,
                                   v_CREATOR        varchar2,
                                   v_CREATEDATE     varchar2,
                                   v_MENDER         varchar2,
                                   v_ALTERDATE      varchar2);

  --保存或修改手足口病报告表
  PROCEDURE usp_AddOrModHFMDReport(v_HFMDID     varchar2,
                                   v_REPORTID   integer,
                                   v_LABRESULT  varchar2,
                                   v_ISSEVERE   varchar2,
                                   v_VAILD      varchar2,
                                   v_CREATOR    varchar2,
                                   v_CREATEDATE varchar2,
                                   v_MENDER     varchar2,
                                   v_ALTERDATE  varchar2);

  --保存或修改AFP报告表
  PROCEDURE usp_AddOrModAFPReport(v_AFPID            varchar2,
                                  v_REPORTID         integer,
                                  v_HOUSEHOLDSCOPE   varchar2,
                                  v_HOUSEHOLDADDRESS varchar2,
                                  v_ADDRESS          varchar2,
                                  v_PALSYDATE        varchar2,
                                  v_PALSYSYMPTOM     varchar2,
                                  v_VAILD            varchar2,
                                  v_CREATOR          varchar2,
                                  v_CREATEDATE       varchar2,
                                  v_MENDER           varchar2,
                                  v_ALTERDATE        varchar2,
                                  v_DIAGNOSISDATE    varchar2);

END;
/
CREATE OR REPLACE PACKAGE BODY EMR_ZYMOSIS_REPORT IS

  /*********************************************************************************/
  PROCEDURE usp_EditZymosis_Report(v_EditType         varchar,
                                   v_Report_ID        NUMERIC DEFAULT 0,
                                   v_Report_NO        varchar DEFAULT '', --报告卡卡号
                                   v_Report_Type      varchar default '', --报告卡类型   1、初次报告  2、订正报告
                                   v_Noofinpat        varchar default '', --首页序号
                                   v_PatID            varchar default '', --住院号
                                   v_Name             varchar default '', --患者姓名
                                   v_ParentName       varchar default '', --家长姓名
                                   v_IDNO             varchar default '', --身份证号码
                                   v_Sex              varchar default '', --患者性别
                                   v_Birth            varchar default '', --出生日期
                                   v_Age              varchar default '', --实足年龄
                                   v_AgeUnit          varchar default '', --实足年龄单位
                                   v_Organization     varchar default '', --工作单位
                                   v_OfficePlace      varchar default '', --单位地址
                                   v_OfficeTEL        varchar default '', --单位电话
                                   v_AddressType      varchar default '', --病人属于地区  1、本县区 2、本市区其他县区  3、本省其他地区  4、外省  5、港澳台  6、外籍
                                   v_HomeTown         varchar default '', --家乡
                                   v_Address          varchar default '', --详细地址[村 街道 门牌号]
                                   v_JobID            varchar default '', --职业代码（按页面顺序记录编号）
                                   v_RecordType1      varchar default '', --病历分类  1、疑似病历  2、临床诊断病历  3、实验室确诊病历  4病原携带者
                                   v_RecordType2      varchar default '', --病历分类（乙型肝炎、血吸虫病填写）  1、急性  2、慢性
                                   v_AttackDate       varchar default '', --发病日期（病原携带者填初检日期或就诊日期）
                                   v_DiagDate         varchar default '', --诊断日期
                                   v_DieDate          varchar default '', --死亡日期
                                   v_DiagICD10        varchar default '', --传染病病种(对应传染病诊断库)
                                   v_DiagName         varchar default '', --传染病病种名称
                                   v_INFECTOTHER_FLAG varchar default '', --有无感染其他人[0无 1有]
                                   v_Memo             varchar default '', --备注
                                   v_Correct_flag     varchar default '', --订正标志【0、未订正 1、已订正】
                                   v_Correct_Name     varchar default '', --订正病名
                                   v_Cancel_Reason    varchar default '', --退卡原因
                                   v_ReportDeptCode   varchar default '', --报告科室编号
                                   v_ReportDeptName   varchar default '', --报告科室名称
                                   v_ReportDocCode    varchar default '', --报告医生编号
                                   v_ReportDocName    varchar default '', --报告医生名称
                                   v_DoctorTEL        varchar default '', --报告医生联系电话
                                   v_Report_Date      varchar default '', --填卡时间
                                   v_State            varchar default '', --报告状态【 1、新增保存 2、提交 3、撤回 4、审核通过 5、审核未通过撤回 6、上报  7、作废】
                                   v_StateName        varchar default '', --操作状态，方便记录操作流水中
                                   v_create_date      varchar default '', --创建时间
                                   v_create_UserCode  varchar default '', --创建人
                                   v_create_UserName  varchar default '', --创建人
                                   v_create_deptCode  varchar default '', --创建人科室
                                   v_create_deptName  varchar default '', --创建人科室
                                   v_Modify_date      varchar default '', --修改时间
                                   v_Modify_UserCode  varchar default '', --修改人
                                   v_Modify_UserName  varchar default '', --修改人
                                   v_Modify_deptCode  varchar default '', --修改人科室
                                   v_Modify_deptName  varchar default '', --修改人科室
                                   v_Audit_date       varchar default '', --审核时间
                                   v_Audit_UserCode   varchar default '', --审核人
                                   v_Audit_UserName   varchar default '', --审核人
                                   v_Audit_deptCode   varchar default '', --审核人科室
                                   v_Audit_deptName   varchar default '', --审核人科室
                                   v_OtherDiag        varchar default '',
                                   o_result           OUT empcurtyp) AS
  
    v_Report_ID_new int;
  BEGIN
  
    --新增传染病报告卡
    IF v_edittype = '1' THEN
    
      select seq_Zymosis_Report_ID.Nextval into v_Report_ID_new from dual;
    
      insert into zymosis_report
        (Report_ID,
         Report_NO, --报告卡卡号
         Report_Type, --报告卡类型   1、初次报告  2、订正报告
         Noofinpat, --首页序号
         PatID, --住院号
         Name, --患者姓名
         ParentName, --家长姓名
         IDNO, --身份证号码
         Sex, --患者性别
         Birth, --出生日期
         Age, --实足年龄
         Age_Unit, --实足年龄单位
         Organization, --工作单位
         OfficePlace, --单位地址
         OfficeTEL, --单位电话
         AddressType, --病人属于地区  1、本县区 2、本市区其他县区  3、本省其他地区  4、外省  5、港澳台  6、外籍
         HomeTown, --家乡
         Address, --详细地址[村 街道 门牌号]
         JobID, --职业代码（按页面顺序记录编号）
         RecordType1, --病历分类  1、疑似病历  2、临床诊断病历  3、实验室确诊病历  4病原携带者
         RecordType2, --病历分类（乙型肝炎、血吸虫病填写）  1、急性  2、慢性
         AttackDate, --发病日期（病原携带者填初检日期或就诊日期）
         DiagDate, --诊断日期
         DieDate, --死亡日期
         DiagICD10, --传染病病种(对应传染病诊断库)
         DiagName, --传染病病种名称
         INFECTOTHER_FLAG, --有无感染其他人[0无 1有]
         Memo, --备注
         Correct_flag, --订正标志【0、未订正 1、已订正】
         Correct_Name, --订正病名
         Cancel_Reason, --退卡原因
         ReportDeptCode, --报告科室编号
         ReportDeptName, --报告科室名称
         ReportDocCode, --报告医生编号
         ReportDocName, --报告医生名称
         DoctorTEL, --报告医生联系电话
         Report_Date, --填卡时间
         State, --报告状态【 1、新增保存 2、提交 3、撤回 4、审核通过 5、审核未通过撤回 6、上报  7、作废】
         create_date, --创建时间
         create_UserCode, --创建人
         create_UserName, --创建人
         create_deptCode, --创建人科室
         create_deptName, --创建人科室
         Modify_date, --修改时间
         Modify_UserCode, --修改人
         Modify_UserName, --修改人
         Modify_deptCode, --修改人科室
         Modify_deptName, --修改人科室
         Audit_date, --审核时间
         Audit_UserCode, --审核人
         Audit_UserName, --审核人
         Audit_deptCode, --审核人科室
         Audit_deptName, --审核人科室
         OTHERDIAG)
      values
        (v_Report_ID_new,
         v_Report_NO, --报告卡卡号
         v_Report_Type, --报告卡类型   1、初次报告  2、订正报告
         v_Noofinpat, --首页序号
         v_PatID, --住院号
         v_Name, --患者姓名
         v_ParentName, --家长姓名
         v_IDNO, --身份证号码
         v_Sex, --患者性别
         v_Birth, --出生日期
         v_Age, --实足年龄
         v_AgeUnit,
         v_Organization, --工作单位
         v_OfficePlace, --单位地址
         v_OfficeTEL, --单位电话
         v_AddressType, --病人属于地区  1、本县区 2、本市区其他县区  3、本省其他地区  4、外省  5、港澳台  6、外籍
         v_HomeTown, --家乡
         v_Address, --详细地址[村 街道 门牌号]
         v_JobID, --职业代码（按页面顺序记录编号）
         v_RecordType1, --病历分类  1、疑似病历  2、临床诊断病历  3、实验室确诊病历  4病原携带者
         v_RecordType2, --病历分类（乙型肝炎、血吸虫病填写）  1、急性  2、慢性
         v_AttackDate, --发病日期（病原携带者填初检日期或就诊日期）
         v_DiagDate, --诊断日期
         v_DieDate, --死亡日期
         v_DiagICD10, --传染病病种(对应传染病诊断库)
         v_DiagName, --传染病病种名称
         v_INFECTOTHER_FLAG, --有无感染其他人[0无 1有]
         v_Memo, --备注
         v_Correct_flag, --订正标志【0、未订正 1、已订正】
         v_Correct_Name, --订正病名
         v_Cancel_Reason, --退卡原因
         v_ReportDeptCode, --报告科室编号
         v_ReportDeptName, --报告科室名称
         v_ReportDocCode, --报告医生编号
         v_ReportDocName, --报告医生名称
         v_DoctorTEL, --报告医生联系电话
         v_Report_Date, --填卡时间
         v_State, --报告状态【 1、新增保存 2、提交 3、撤回 4、审核通过 5、审核未通过撤回 6、上报  7、作废】
         to_char(sysdate, 'yyyy-mm-dd HH24:mi:ss'), --创建时间
         v_create_UserCode, --创建人
         v_create_UserName, --创建人
         v_create_deptCode, --创建人科室
         v_create_deptName, --创建人科室
         v_Modify_date, --修改时间
         v_Modify_UserCode, --修改人
         v_Modify_UserName, --修改人
         v_Modify_deptCode, --修改人科室
         v_Modify_deptName, --修改人科室
         v_Audit_date, --审核时间
         v_Audit_UserCode, --审核人
         v_Audit_UserName, --审核人
         v_Audit_deptCode, --审核人科室
         v_Audit_deptName, --审核人科室
         v_OtherDiag);
    
      --插入传染病报告卡操作流水
      insert into Zymosis_Report_SN
        (Report_SN_ID, --表流水号
         Report_ID, --传染病报告卡编号
         create_date, --创建时间
         create_UserCode, --创建人
         create_UserName, --创建人
         create_deptCode, --创建人科室
         create_deptName, --创建人科室
         State, --修改类型
         Memo --备注
         )
      values
        (seq_Zymosis_Report_SN_ID.Nextval, --表流水号
         v_Report_ID_new, --传染病报告卡编号
         to_char(sysdate, 'yyyy-mm-dd HH24:mi:ss'), --创建时间
         v_create_UserCode, --创建人
         v_create_UserName, --创建人
         v_create_deptCode, --创建人科室
         v_create_deptName, --创建人科室
         v_StateName, --修改类型
         '' --备注
         );
    
      open o_result for
        select v_Report_ID_new from dual;
    
    end if;
  
    --修改保存传染病报告卡信息
    IF v_edittype = '2' THEN
    
      update zymosis_report
         set Report_NO        = nvl(v_Report_NO, Report_NO), --报告卡卡号
             Report_Type      = nvl(v_Report_Type, Report_Type), --报告卡类型   1、初次报告  2、订正报告
             Noofinpat        = nvl(v_Noofinpat, Noofinpat), --首页序号
             PatID            = nvl(v_PatID, PatID), --住院号
             Name             = nvl(v_Name, Name), --患者姓名
             ParentName       = v_ParentName, --家长姓名
             IDNO             = v_IDNO, --身份证号码
             Sex              = nvl(v_Sex, Sex), --患者性别
             Birth            = v_Birth, --出生日期
             Age              = v_Age, --实足年龄
             Age_Unit         = v_AgeUnit, --实足年龄
             Organization     = v_Organization, --工作单位
             OfficePlace      = v_OfficePlace, --单位地址
             OfficeTEL        = v_OfficeTEL, --单位电话
             AddressType      = nvl(v_AddressType, AddressType), --病人属于地区  1、本县区 2、本市区其他县区  3、本省其他地区  4、外省  5、港澳台  6、外籍
             HomeTown         = v_HomeTown, --家乡
             Address          = v_Address, --详细地址[村 街道 门牌号]
             JobID            = v_JobID, --职业代码（按页面顺序记录编号）
             RecordType1      = v_RecordType1, --病历分类  1、疑似病历  2、临床诊断病历  3、实验室确诊病历  4病原携带者
             RecordType2      = v_RecordType2, --病历分类（乙型肝炎、血吸虫病填写）  1、急性  2、慢性
             AttackDate       = v_AttackDate, --发病日期（病原携带者填初检日期或就诊日期）
             DiagDate         = v_DiagDate, --诊断日期
             DieDate          = v_DieDate, --死亡日期
             DiagICD10        = v_DiagICD10, --传染病病种(对应传染病诊断库)
             DiagName         = v_DiagName, --传染病病种名称
             INFECTOTHER_FLAG = v_INFECTOTHER_FLAG, --有无感染其他人[0无 1有]
             Memo             = v_Memo, --备注
             Correct_flag     = v_Correct_flag, --订正标志【0、未订正 1、已订正】
             Correct_Name     = v_Correct_Name, --订正病名
             Cancel_Reason    = v_Cancel_Reason, --退卡原因
             ReportDeptCode   = v_ReportDeptCode, --报告科室编号
             ReportDeptName   = v_ReportDeptName, --报告科室名称
             ReportDocCode    = v_ReportDocCode, --报告医生编号
             ReportDocName    = v_ReportDocName, --报告医生名称
             DoctorTEL        = v_DoctorTEL, --报告医生联系电话
             Report_Date      = v_Report_Date, --填卡时间
             State            = v_State, --报告状态【 1、新增保存 2、提交 3、撤回 4、审核通过 5、审核未通过撤回 6、上报  7、作废】
             create_date      = nvl(v_create_date, create_date), --创建时间
             create_UserCode  = nvl(v_create_UserCode, create_UserCode), --创建人
             create_UserName  = nvl(v_create_UserName, create_UserName), --创建人
             create_deptCode  = nvl(v_create_deptCode, create_deptCode), --创建人科室
             create_deptName  = nvl(v_create_deptName, create_deptName), --创建人科室
             Modify_date      = nvl(v_Modify_date, Modify_date), --修改时间
             Modify_UserCode  = nvl(v_Modify_UserCode, Modify_UserCode), --修改人
             Modify_UserName  = nvl(v_Modify_UserName, Modify_UserName), --修改人
             Modify_deptCode  = nvl(v_Modify_deptCode, Modify_deptCode), --修改人科室
             Modify_deptName  = nvl(v_Modify_deptName, Modify_deptName), --修改人科室
             Audit_date       = nvl(v_Audit_date, Audit_date), --审核时间
             Audit_UserCode   = nvl(v_Audit_UserCode, Audit_UserCode), --审核人
             Audit_UserName   = nvl(v_Audit_UserName, Audit_UserName), --审核人
             Audit_deptCode   = nvl(v_Audit_deptCode, Audit_deptCode), --审核人科室
             Audit_deptName   = nvl(v_Audit_deptName, Audit_deptName), --审核人科室
             OtherDiag        = v_OtherDiag
      
       where Report_ID = v_Report_ID;
    
      --插入传染病报告卡操作流水
      insert into Zymosis_Report_SN
        (Report_SN_ID, --表流水号
         Report_ID, --传染病报告卡编号
         create_date, --创建时间
         create_UserCode, --创建人
         create_UserName, --创建人
         create_deptCode, --创建人科室
         create_deptName, --创建人科室
         State, --修改类型
         Memo --备注
         )
      values
        (seq_Zymosis_Report_SN_ID.Nextval, --表流水号
         v_Report_ID, --传染病报告卡编号
         to_char(sysdate, 'yyyy-mm-dd HH24:mi:ss'), --创建时间
         v_Modify_UserCode, --创建人
         v_Modify_UserName, --创建人
         v_Modify_deptCode, --创建人科室
         v_Modify_deptName, --创建人科室
         v_StateName, --修改类型
         '' --备注
         );
      open o_result for
        select v_Report_ID from dual;
    
    end if;
  
    --作废传染病报告卡信息
    IF v_edittype = '3' THEN
    
      update zymosis_report
         set /*Report_NO        = nvl(v_Report_NO, Report_NO), --报告卡卡号*/      Report_Type = nvl(v_Report_Type,
             Report_Type), --报告卡类型   1、初次报告  2、订正报告
             Noofinpat        = nvl(v_Noofinpat, Noofinpat), --首页序号
             PatID            = nvl(v_PatID, PatID), --住院号
             Name             = nvl(v_Name, Name), --患者姓名
             ParentName       = v_ParentName, --家长姓名
             IDNO             = v_IDNO, --身份证号码
             Sex              = nvl(v_Sex, Sex), --患者性别
             Birth            = v_Birth, --出生日期
             Age              = v_Age, --实足年龄
             Age_Unit         = v_AgeUnit, --实足年龄
             Organization     = v_Organization, --工作单位
             OfficePlace      = v_OfficePlace, --单位地址
             OfficeTEL        = v_OfficeTEL, --单位电话
             AddressType      = nvl(v_AddressType, AddressType), --病人属于地区  1、本县区 2、本市区其他县区  3、本省其他地区  4、外省  5、港澳台  6、外籍
             HomeTown         = v_HomeTown, --家乡
             Address          = v_Address, --详细地址[村 街道 门牌号]
             JobID            = v_JobID, --职业代码（按页面顺序记录编号）
             RecordType1      = v_RecordType1, --病历分类  1、疑似病历  2、临床诊断病历  3、实验室确诊病历  4病原携带者
             RecordType2      = v_RecordType2, --病历分类（乙型肝炎、血吸虫病填写）  1、急性  2、慢性
             AttackDate       = v_AttackDate, --发病日期（病原携带者填初检日期或就诊日期）
             DiagDate         = v_DiagDate, --诊断日期
             DieDate          = v_DieDate, --死亡日期
             DiagICD10        = nvl(v_DiagICD10, DiagICD10), --传染病病种(对应传染病诊断库)
             DiagName         = nvl(v_DiagName, DiagName), --传染病病种名称
             INFECTOTHER_FLAG = v_INFECTOTHER_FLAG, --有无感染其他人[0无 1有]
             Memo             = v_Memo, --备注
             Correct_flag     = v_Correct_flag, --订正标志【0、未订正 1、已订正】
             Correct_Name     = v_Correct_Name, --订正病名
             Cancel_Reason    = v_Cancel_Reason, --退卡原因
             ReportDeptCode   = v_ReportDeptCode, --报告科室编号
             ReportDeptName   = v_ReportDeptName, --报告科室名称
             ReportDocCode    = v_ReportDocCode, --报告医生编号
             ReportDocName    = v_ReportDocName, --报告医生名称
             DoctorTEL        = v_DoctorTEL, --报告医生联系电话
             Report_Date      = v_Report_Date, --填卡时间
             State            = v_State, --报告状态【 1、新增保存 2、提交 3、撤回 4、审核通过 5、审核未通过撤回 6、上报  7、作废】
             create_date      = nvl(v_create_date, create_date), --创建时间
             create_UserCode  = nvl(v_create_UserCode, create_UserCode), --创建人
             create_UserName  = nvl(v_create_UserName, create_UserName), --创建人
             create_deptCode  = nvl(v_create_deptCode, create_deptCode), --创建人科室
             create_deptName  = nvl(v_create_deptName, create_deptName), --创建人科室
             Modify_date      = nvl(v_Modify_date, Modify_date), --修改时间
             Modify_UserCode  = nvl(v_Modify_UserCode, Modify_UserCode), --修改人
             Modify_UserName  = nvl(v_Modify_UserName, Modify_UserName), --修改人
             Modify_deptCode  = nvl(v_Modify_deptCode, Modify_deptCode), --修改人科室
             Modify_deptName  = nvl(v_Modify_deptName, Modify_deptName), --修改人科室
             Audit_date       = nvl(v_Audit_date, Audit_date), --审核时间
             Audit_UserCode   = nvl(v_Audit_UserCode, Audit_UserCode), --审核人
             Audit_UserName   = nvl(v_Audit_UserName, Audit_UserName), --审核人
             Audit_deptCode   = nvl(v_Audit_deptCode, Audit_deptCode), --审核人科室
             Audit_deptName   = nvl(v_Audit_deptName, Audit_deptName), --审核人科室
             OtherDiag        = v_OtherDiag,
             vaild            = 0
      
       where Report_ID = v_Report_ID;
    
      insert into Zymosis_Report_SN
        (Report_SN_ID, --表流水号
         Report_ID, --传染病报告卡编号
         create_date, --创建时间
         create_UserCode, --创建人
         create_UserName, --创建人
         create_deptCode, --创建人科室
         create_deptName, --创建人科室
         State, --修改类型
         Memo --备注
         )
      values
        (seq_Zymosis_Report_SN_ID.Nextval, --表流水号
         v_Report_ID, --传染病报告卡编号
         to_char(sysdate, 'yyyy-mm-dd HH24:mi:ss'), --创建时间
         v_Modify_UserCode, --创建人
         v_Modify_UserName, --创建人
         v_Modify_deptCode, --创建人科室
         v_Modify_deptName, --创建人科室
         v_StateName, --修改类型
         '' --备注
         );
    
      open o_result for
        select v_Report_ID from dual;
    
    end if;
  
    --根据传入的传染病报告卡ID查询报告卡信息
    IF v_edittype = '4' THEN
    
      open o_result for
        select * from zymosis_report a where a.report_id = v_Report_ID;
    
    end if;
  
  end;

  /*********************************************************************************/

  PROCEDURE usp_GetInpatientByNofinpat(v_Noofinpat varchar default '', --首页序号
                                       o_result    OUT empcurtyp) as
    /*
    * 在传染病报告卡模块中新增报告卡时候根据病人首页号获取病人信息
    */
  begin
  
    open o_result for
      select inp.noofinpat,
             inp.patid,
             inp.patnoofhis,
             inp.name,
             inp.sexid,
             inp.birth,
             (case
               when instr(inp.agestr, '岁') > 1 then
                replace(inp.agestr, '岁')
               when instr(inp.agestr, '月') > 1 then
                replace(inp.agestr, '个月')
               else
                ''
             end) age,
             (case
               when instr(inp.agestr, '岁') > 1 then
                '1'
               when instr(inp.agestr, '月') > 1 then
                '2'
               else
                ''
             end) ageUint,
             inp.idno,
             inp.officeplace organization,
             inp.officeplace,
             inp.officetel,
             inp.outhosdept,
             --inp.address,--原来的地址
             inp.nativeaddress, --edit by ywk 2012年3月26日15:26:44 改为这个地址
             inp.attend,
             to_char(sysdate, 'yyyy-mm-dd') reportdate
        from inpatient inp
       where inp.noofinpat = v_Noofinpat;
  end;

  PROCEDURE usp_geteditzymosisreport(v_report_type1    varchar default '',
                                     v_report_type2    varchar default '',
                                     v_name            varchar default '',
                                     v_patid           varchar default '',
                                     v_deptid          varchar default '',
                                     v_applicant       varchar default '',
                                     v_status          varchar default '',
                                     v_createdatestart varchar default '', --新增的上报日期开始
                                     v_createdateend   varchar default '', --新增的上报日期结束
                                     v_querytype       varchar default '', --查询类型
                                     o_result          OUT empcurtyp) as
  begin
    if v_querytype = '1' THEN
      --有时间限制
      open o_result for
        SELECT report_id,
               report_no,
               CASE report_type
                 WHEN '1' THEN
                  '初步报告'
                 ELSE
                  '订正报告'
               END REPORTTYPENAME,
               noofinpat,
               patid,
               NAME,
               state,
               create_date,
               create_deptcode,
               create_deptcode
          FROM zymosis_report
         WHERE ((v_report_type1 IS NOT NULL AND
               zymosis_report.report_type = '1') OR
               (v_report_type2 IS NOT NULL AND
               zymosis_report.report_type = '2'))
           AND zymosis_report.name like '%' || v_name || '%'
           AND zymosis_report.patid like '%' || v_patid || '%'
           AND zymosis_report.create_deptcode like '%' || v_deptid || '%'
           AND (v_applicant IS NULL OR
               v_applicant IS NOT NULL AND
               zymosis_report.create_usercode = v_applicant)
           AND instr(v_status, zymosis_report.state) > 0
           AND create_date between v_createdatestart || ' 00:00:00' and
               v_createdateend || ' 23:59:59'; --更改为时间段 
    end if;
  
    if v_querytype = '2' THEN
      --无时间限制
      open o_result for
        SELECT report_id,
               report_no,
               CASE report_type
                 WHEN '1' THEN
                  '初步报告'
                 ELSE
                  '订正报告'
               END REPORTTYPENAME,
               noofinpat,
               patid,
               NAME,
               state,
               create_date,
               create_deptcode,
               create_deptcode
          FROM zymosis_report
         WHERE ((v_report_type1 IS NOT NULL AND
               zymosis_report.report_type = '1') OR
               (v_report_type2 IS NOT NULL AND
               zymosis_report.report_type = '2'))
           AND zymosis_report.name like '%' || v_name || '%'
           AND zymosis_report.patid like '%' || v_patid || '%'
           AND zymosis_report.create_deptcode like '%' || v_deptid || '%'
           AND (v_applicant IS NULL OR
               v_applicant IS NOT NULL AND
               zymosis_report.create_usercode = v_applicant)
           AND instr(v_status, zymosis_report.state) > 0;
    end if;
  
  end;

  PROCEDURE usp_getReportBrowse(v_report_type1 varchar default '',
                                v_report_type2 varchar default '',
                                v_recordtype1  varchar default '',
                                v_beginDate    varchar default '',
                                v_EndDate      varchar default '',
                                v_deptid       varchar default '',
                                v_diagICD      varchar default '',
                                v_status       varchar default '',
                                o_result       OUT empcurtyp) as
  begin
  
    open o_result for
      select rep.name,
             rep.report_id,
             rep.report_no,
             CASE report_type
               WHEN '1' THEN
                '初步报告'
               ELSE
                '订正报告'
             END REPORTTYPENAME,
             rep.sex as sexid, --add by cyq 2012-11-16
             (case
               when rep.sex = '1' then
                '男'
               when rep.sex = '2' then
                '女'
             end) sexstr,
             rep.birth,
             (case
               when rep.birth is not null then
                GetYearAgeByBirthAndApplyDate(to_date(rep.birth,
                                                      'yyyy-mm-dd'),
                                              to_date(rep.create_date,
                                                      'yyyy-mm-dd HH24:mi:ss'))
               when rep.birth is null then
                rep.age || (case
                  when rep.age_unit = '1' then
                   '岁'
                  when rep.age_unit = '2' then
                   '月'
                  when rep.age_unit = '3' then
                   '天'
                end)
             end) agestr,
             rep.diagicd10,
             rep.diagname,
             rep.address,
             rep.reportdeptcode,
             rep.reportdeptname,
             (case
               when rep.recordtype1 = '1' then
                '疑似病例'
               when rep.recordtype1 = '2' then
                '临床诊断病例'
               when rep.recordtype1 = '3' then
                '实验室确诊病例'
               when rep.recordtype1 = '4' then
                '病原携带者'
             end) recordtype1,
             job.jobname jobname,
             rep.create_date,
             rep.create_usercode,
             rep.create_username,
             (case
               when rep.state = '1' then
                '新增'
               when rep.state = '2' then
                '提交'
               when rep.state = '3' then
                '撤回'
               when rep.state = '4' then
                '审核通过'
               when rep.state = '5' then
                '审核未通过撤回'
               when rep.state = '6' then
                '上报'
               when rep.state = '7' then
                '作废'
             end) stateName
        from zymosis_report rep
        left join zymosis_job job
          on rep.jobid = job.jobid
       WHERE ((v_report_type1 IS NOT NULL AND rep.report_type = '1') OR
             (v_report_type2 IS NOT NULL AND rep.report_type = '2'))
         AND v_begindate || ' 00:00:00' <= rep.create_date
         AND rep.create_date <= v_enddate || ' 23:59:59'
         and (rep.recordtype1 = v_recordtype1 or v_recordtype1 = '' or
             v_recordtype1 is null)
         AND (rep.reportdeptcode = v_deptid or v_deptid = '' or
             v_deptid is null)
         AND (rep.diagicd10 = v_diagICD or v_diagICD = '' or
             v_diagICD is null)
         AND instr(v_status, rep.state) > 0;
  
  end;

  ---获取传染病分析信息
  PROCEDURE usp_GetReportAnalyse(v_beginDate varchar default '',
                                 v_EndDate   varchar default '',
                                 o_result    OUT empcurtyp) as
  
    v_sql    VARCHAR2(4000);
    v_cnt    integer;
    v_diecnt integer;
  begin
  
    v_sql := 'truncate table tmp_Zymosis_Analyse ';
  
    EXECUTE IMMEDIATE v_sql;
  
    --插入所有传染病信息
    INSERT INTO tmp_Zymosis_Analyse
      (level_id, level_name, ICD_Code, Name)
      select a.level_id,
             (case
               when a.level_id = 1 then
                '甲类传染病'
               when a.level_id = 2 then
                '乙类传染病'
               when a.level_id = 3 then
                '丙类传染病'
               else
                '其他传染病'
             end) level_Name,
             a.icd,
             a.name
        from Zymosis_Diagnosis a;
  
    --更新发病数
    UPDATE tmp_Zymosis_Analyse tmp
       SET Cnt = NVL((SELECT COUNT(distinct rep.noofinpat)
                       FROM zymosis_report rep
                      WHERE rep.diagicd10 = tmp.icd_code
                        and rep.vaild = 1
                        AND v_begindate || ' 00:00:00' <= rep.create_date
                        AND rep.create_date <= v_enddate || ' 23:59:59'
                        and rep.state in (4, 6)),
                     0);
  
    --更新死亡数
    UPDATE tmp_Zymosis_Analyse tmp
       SET Die_Cnt = NVL((SELECT COUNT(distinct rep.noofinpat)
                           FROM zymosis_report rep
                          WHERE rep.diagicd10 = tmp.icd_code
                            and rep.diedate is not null
                            and rep.vaild = 1
                            AND v_begindate || ' 00:00:00' <=
                                rep.create_date
                            AND rep.create_date <= v_enddate || ' 23:59:59'
                            and rep.state in (4, 6)),
                         0);
  
    --插入其他病种传染病
    INSERT INTO tmp_Zymosis_Analyse
      (level_id, level_name, ICD_Code, Name)
      select distinct 4, '其他' level_Name, '', a.otherdiag
        from zymosis_report a
       where a.diagicd10 is null;
  
    --更新发病数
    UPDATE tmp_Zymosis_Analyse tmp
       SET Cnt = NVL((SELECT COUNT(distinct rep.noofinpat)
                       FROM zymosis_report rep
                      WHERE rep.otherdiag = tmp.name
                        and rep.vaild = 1
                        and rep.diagicd10 is null
                        AND v_begindate || ' 00:00:00' <= rep.create_date
                        AND rep.create_date <= v_enddate || ' 23:59:59'
                        and rep.state in (4, 6)),
                     0)
     where tmp.icd_code is null;
  
    --更新死亡数
    UPDATE tmp_Zymosis_Analyse tmp
       SET Die_Cnt = NVL((SELECT COUNT(distinct rep.noofinpat)
                           FROM zymosis_report rep
                          WHERE rep.otherdiag = tmp.name
                            and rep.diedate is not null
                            and rep.vaild = 1
                            and rep.diagicd10 is null
                            AND v_begindate || ' 00:00:00' <=
                                rep.create_date
                            AND rep.create_date <= v_enddate || ' 23:59:59'
                            and rep.state in (4, 6)),
                         0)
     where tmp.icd_code is null;
  
    select sum(cnt) into v_cnt from tmp_Zymosis_Analyse;
    select sum(die_cnt) into v_diecnt from tmp_Zymosis_Analyse;
  
    --更新发病数占总发病数百分比
    UPDATE tmp_Zymosis_Analyse tmp
       SET Attack_rate = (case
                           when v_cnt = 0 then
                            0
                           else
                            to_number(cnt) / v_cnt * 100
                         end);
  
    --更新死亡数占总发病数百分比
    UPDATE tmp_Zymosis_Analyse tmp
       SET Die_Rate = (case
                        when v_diecnt = 0 then
                         0
                        else
                         to_number(Die_Cnt) / v_diecnt * 100
                      end);
  
    commit;
  
    open o_result for
    /* select a.level_id,
                                                                                                                                         a.level_name,
                                                                                                                                         a.ICD_Code,
                                                                                                                                         a.Name,
                                                                                                                                         a.cnt,
                                                                                                                                         a.attack_rate || '%' attack_rate,
                                                                                                                                         a.die_cnt,
                                                                                                                                         a.die_rate || '%' die_rate,
                                                                                                                                         (case
                                                                                                                                           when a.cnt = 0 then
                                                                                                                                            0
                                                                                                                                           else
                                                                                                                                            a.die_cnt / a.cnt * 100
                                                                                                                                         end) dieRate
                                                                                                                                    from tmp_Zymosis_Analyse a;*/
      select *
        from (select a.level_id,
                     a.level_name,
                     a.ICD_Code,
                     a.Name,
                     a.cnt,
                     a.attack_rate,
                     a.die_cnt,
                     a.die_rate,
                     round((case
                             when a.cnt = 0 then
                              0
                             else
                              a.die_cnt / a.cnt * 100
                           end),
                           4) dieRate
                from tmp_Zymosis_Analyse a
              
              union all
              
              select a.level_id,
                     '总计' level_name,
                     '' ICD_Code,
                     '小计' Name,
                     to_char(sum(a.cnt)) cnt,
                     sum(a.attack_rate) attack_rate,
                     to_char(sum(a.die_cnt)) die_cnt,
                     sum(a.die_rate) die_rate,
                     round((case
                             when sum(a.cnt) = 0 then
                              0
                             else
                              sum(a.die_cnt) / sum(a.cnt) * 100
                           end),
                           4) dieRate
                from tmp_Zymosis_Analyse a
               group by a.level_id)
       order by level_id, level_name;
  
  end;

  --分职业统计传染病情况
  PROCEDURE usp_GetJobDisease(v_beginDate varchar default '',
                              v_EndDate   varchar default '',
                              v_DiagCode  varchar default '',
                              o_result    OUT empcurtyp) as
    v_sql     VARCHAR2(4000);
    v_jobID   varchar2(4);
    v_jobName varchar2(14);
  begin
    v_sql := 'truncate table tmp_JobDisease ';
  
    EXECUTE IMMEDIATE v_sql;
  
    --定义游标
    declare
      cursor cr_JobDisease is
        select jobID, jobName from zymosis_job;
    
    begin
      open cr_JobDisease;
      fetch cr_JobDisease
        into v_jobID, v_jobName;
      while cr_JobDisease%found loop
      
        --游标循环插入值
        INSERT INTO tmp_JobDisease
          (JobID, JobName, DiagID, DiagName, DiseaseCnt, DieCnt)
          select v_jobID jobid, v_jobName jobName, a.icd, a.name, 0, 0
            from Zymosis_Diagnosis a
           where v_DiagCode like '%,' || a.icd || ',%';
      
        fetch cr_JobDisease
          into v_jobID, v_jobName;
      end loop;
    
      close cr_JobDisease;
    
    end;
  
    commit;
  
    --更新发病数
    UPDATE tmp_JobDisease tmp
       SET DiseaseCnt = NVL((SELECT COUNT(distinct rep.noofinpat)
                              FROM zymosis_report rep
                             WHERE rep.diagicd10 = tmp.diagid
                               and rep.jobid = tmp.jobid
                               and rep.vaild = 1
                               AND v_begindate || ' 00:00:00' <=
                                   rep.create_date
                               AND rep.create_date <=
                                   v_enddate || ' 23:59:59'
                               and rep.state in (4, 6)),
                            0);
  
    --更新死亡数
    UPDATE tmp_JobDisease tmp
       SET DieCnt = NVL((SELECT COUNT(distinct rep.noofinpat)
                          FROM zymosis_report rep
                         WHERE rep.diagicd10 = tmp.diagid
                           and rep.jobid = tmp.jobid
                           and rep.diedate is not null
                           and rep.vaild = 1
                           AND v_begindate || ' 00:00:00' <= rep.create_date
                           AND rep.create_date <= v_enddate || ' 23:59:59'
                           and rep.state in (4, 6)),
                        0);
    commit;
  
    open o_result for
    --select * from tmp_JobDisease;
      select * from tmp_JobDisease order by to_number(jobid);
    /*  --插入所有传染病信息
    INSERT INTO tmp_JobDisease
      (level_id, level_name, ICD_Code, Name)
      select a.level_id,
             (case
               when a.level_id = 1 then
                '甲类传染病'
               when a.level_id = 2 then
                '乙类传染病'
               when a.level_id = 3 then
                '丙类传染病'
               else
                '其他传染病'
             end) level_Name,
             a.icd,
             a.name
        from Zymosis_Diagnosis a;*/
  end;

  --得到所有有效的诊断
  PROCEDURE usp_GetDiagnosis(o_result OUT empcurtyp) AS
  BEGIN
    OPEN o_result FOR
      SELECT markid, icd, name, py, wb, memo, valid, statist, name namestr
        FROM DIAGNOSIS a
       where not exists
       (select 1 from zymosis_diagnosis d where d.icd = a.icd)
       order by icd;
  END;

  --得到所有有效的诊断
  PROCEDURE usp_GetDiagnosisTo(v_categoryid varchar default '',
                               o_result     OUT empcurtyp) AS
  BEGIN
    OPEN o_result FOR
      SELECT markid, icd, name, py, wb, memo, valid, statist, name namestr
        FROM DIAGNOSIS a
       where not exists (select 1
                from zymosis_diagnosis d
               where d.icd = a.icd
                 and (d.categoryid = v_categoryid or
                     v_categoryid = '' or v_categoryid is null))
         and a.valid = '1'
       order by icd;
  END;

  --得到所有有效的诊断
  PROCEDURE usp_GetDiagnosisTo_ZY(v_categoryid varchar default '',
                                  o_result     OUT empcurtyp) AS
  BEGIN
    OPEN o_result FOR
      SELECT ID as markid,
             ID AS icd,
             name,
             py,
             wb,
             memo,
             '' as statist,
             valid,
             name namestr
        FROM diagnosisofchinese a
       where not exists (select 1
                from zymosis_diagnosis d
               where d.icd = a.id
                 and (d.categoryid = v_categoryid or
                     v_categoryid = '' or v_categoryid is null))
         and a.valid = '1'
       order by icd;
  END;

  --得到传染病病种信息
  PROCEDURE usp_GetDisease2(o_result OUT empcurtyp) AS
  BEGIN
    OPEN o_result FOR
      SELECT (CASE
               WHEN a.level_id = 1 THEN
                '甲类传染病'
               WHEN a.level_id = 2 THEN
                '乙类传染病'
               WHEN a.level_id = 3 THEN
                '丙类传染病'
               ELSE
                '其他传染病'
             END) level_name,
             a.level_id,
             a.icd,
             a.NAME,
             a.py,
             a.wb,
             a.markid,
             a.memo,
             a.statist,
             CASE a.valid
               WHEN 1 THEN
                '有效'
               ELSE
                '无效'
             END valid_name,
             a.valid,
             a.namestr,
             a.upcount,
             a.fukatype
        FROM zymosis_diagnosis a
       where a.categoryid = 1
       order by a.icd;
  END;

  PROCEDURE usp_GetDisease2To(v_categoryid varchar default '',
                              o_result     OUT empcurtyp) AS
  BEGIN
    OPEN o_result FOR
      SELECT (CASE
               WHEN a.level_id = 1 THEN
                '甲类传染病'
               WHEN a.level_id = 2 THEN
                '乙类传染病'
               WHEN a.level_id = 3 THEN
                '丙类传染病'
               ELSE
                '其他传染病'
             END) level_name,
             a.level_id,
             a.icd,
             a.NAME,
             a.py,
             a.wb,
             a.markid,
             a.memo,
             a.statist,
             CASE a.valid
               WHEN 1 THEN
                '有效'
               ELSE
                '无效'
             END valid_name,
             a.valid,
             a.namestr,
             a.upcount,
             a.categoryid
      
        FROM zymosis_diagnosis a
       where a.categoryid = v_categoryid
       order by a.icd;
  END;

  --保存病种记录
  PROCEDURE usp_SaveZymosisDiagnosis(v_markid   varchar default '',
                                     v_icd      varchar default '',
                                     v_name     varchar default '',
                                     v_py       varchar default '',
                                     v_wb       varchar default '',
                                     v_levelID  varchar default '',
                                     v_valid    varchar default '',
                                     v_statist  varchar default '',
                                     v_memo     varchar default '',
                                     v_namestr  varchar default '',
                                     v_upcount  integer,
                                     v_fukatype varchar default '') AS
    p_count INT;
  BEGIN
    SELECT COUNT(1) INTO p_count FROM zymosis_diagnosis WHERE icd = v_icd;
  
    IF p_count > 0 THEN
      UPDATE zymosis_diagnosis
         SET markid   = v_markid,
             icd      = v_icd,
             name     = v_name,
             py       = v_py,
             wb       = v_wb,
             level_id = v_levelID,
             valid    = v_valid,
             memo     = v_memo,
             statist  = v_statist,
             namestr  = v_namestr,
             upcount  = v_upcount,
             fukatype = v_fukatype
       WHERE icd = v_icd;
    ELSE
      INSERT INTO zymosis_diagnosis
        (markid,
         icd,
         name,
         py,
         wb,
         level_id,
         valid,
         memo,
         statist,
         namestr,
         upcount,
         fukatype)
      VALUES
        (v_markid,
         v_icd,
         v_name,
         v_py,
         v_wb,
         v_levelID,
         v_valid,
         v_memo,
         v_statist,
         v_namestr,
         v_upcount,
         v_fukatype);
    END IF;
  END;

  --保存病种记录
  PROCEDURE usp_SaveZymosisDiagnosisTo(v_markid     varchar default '',
                                       v_icd        varchar default '',
                                       v_name       varchar default '',
                                       v_py         varchar default '',
                                       v_wb         varchar default '',
                                       v_levelID    varchar default '',
                                       v_valid      varchar default '',
                                       v_statist    varchar default '',
                                       v_memo       varchar default '',
                                       v_namestr    varchar default '',
                                       v_upcount    integer,
                                       v_categoryid integer) AS
    p_count INT;
  BEGIN
    SELECT COUNT(1)
      INTO p_count
      FROM zymosis_diagnosis
     WHERE icd = v_icd
       and categoryid = v_categoryid;
  
    IF p_count > 0 THEN
      UPDATE zymosis_diagnosis
         SET markid     = v_markid,
             icd        = v_icd,
             name       = v_name,
             py         = v_py,
             wb         = v_wb,
             level_id   = v_levelID,
             valid      = v_valid,
             memo       = v_memo,
             statist    = v_statist,
             namestr    = v_namestr,
             upcount    = v_upcount,
             categoryid = v_categoryid
       WHERE icd = v_icd
         and categoryid = v_categoryid;
    ELSE
      INSERT INTO zymosis_diagnosis
        (markid,
         icd,
         name,
         py,
         wb,
         level_id,
         valid,
         memo,
         statist,
         namestr,
         upcount,
         categoryid)
      VALUES
        (v_markid,
         v_icd,
         v_name,
         v_py,
         v_wb,
         v_levelID,
         v_valid,
         v_memo,
         v_statist,
         v_namestr,
         v_upcount,
         v_categoryid);
    END IF;
  END;

  /*********************************************************************************/
  PROCEDURE usp_EditTbirthdefects_Report(v_EditType         varchar,
                                         v_ID               NUMERIC DEFAULT 0,
                                         v_REPORT_NOOFINPAT varchar DEFAULT '', --病人编号
                                         v_REPORT_ID        varchar default '', --报告卡序号
                                         v_DIAG_CODE        varchar default '', --报告卡诊断编码
                                         
                                         v_STRING3         varchar default '', --预留
                                         v_STRING4         varchar default '', --预留
                                         v_STRING5         varchar default '', --预留
                                         v_REPORT_PROVINCE varchar default '', --上报告卡省份
                                         v_REPORT_CITY     varchar default '', --报告卡市（县）
                                         v_REPORT_TOWN     varchar default '', --报告卡乡镇
                                         v_REPORT_VILLAGE  varchar default '', --报告卡村
                                         v_REPORT_HOSPITAL varchar default '', --报告卡医院
                                         v_REPORT_NO       varchar default '', --报告卡序号
                                         v_MOTHER_PATID    varchar default '', --产妇住院号
                                         v_MOTHER_NAME     varchar default '', --姓名
                                         v_MOTHER_AGE      varchar default '', --年龄
                                         v_NATIONAL        varchar default '', --民族
                                         v_ADDRESS_POST    varchar default '', --地址and邮编
                                         v_PREGNANTNO      varchar default '', --孕次
                                         v_PRODUCTIONNO    varchar default '', --产次
                                         v_LOCALADD        varchar default '', --常住地
                                         
                                         v_PERCAPITAINCOME         varchar default '', --年人均收入     
                                         v_EDUCATIONLEVEL          varchar default '', --文化程度     
                                         v_CHILD_PATID             varchar default '', --孩子住院号     
                                         v_CHILD_NAME              varchar default '', --孩子姓名     
                                         v_ISBORNHERE              varchar default '', --是否本院出生     
                                         v_CHILD_SEX               varchar default '', --孩子性别      
                                         v_BORN_YEAR               varchar default '', --出生年     
                                         v_BORN_MONTH              varchar default '', --  出生月     
                                         v_BORN_DAY                varchar default '', --出生日      
                                         v_GESTATIONALAGE          varchar default '', --胎龄     
                                         v_WEIGHT                  varchar default '', --体重     
                                         v_BIRTHS                  varchar default '', --胎数     
                                         v_ISIDENTICAL             varchar default '', --是否同卵      
                                         v_OUTCOME                 varchar default '', --转归      
                                         v_INDUCEDLABOR            varchar default '', --是否引产     
                                         v_DIAGNOSTICBASIS         varchar default '', --诊断依据——临床      
                                         v_DIAGNOSTICBASIS1        varchar default '', --诊断依据——超声波      
                                         v_DIAGNOSTICBASIS2        varchar default '', --诊断依据——尸解     
                                         v_DIAGNOSTICBASIS3        varchar default '', --诊断依据——生化检查     
                                         v_DIAGNOSTICBASIS4        varchar default '', --诊断依据——生化检查——其他     
                                         v_DIAGNOSTICBASIS5        varchar default '', --诊断依据——染色体      
                                         v_DIAGNOSTICBASIS6        varchar default '', --诊断依据——其他     
                                         v_DIAGNOSTICBASIS7        varchar default '', --诊断依据——其他——内容     
                                         v_DIAG_ANENCEPHALY        varchar default '', --出生缺陷诊断——无脑畸形     
                                         v_DIAG_SPINA              varchar default '', --出生缺陷诊断——脊柱裂      
                                         v_DIAG_PENGOUT            varchar default '', --出生缺陷诊断——脑彭出      
                                         v_DIAG_HYDROCEPHALUS      varchar default '', --出生缺陷诊断——先天性脑积水     
                                         v_DIAG_CLEFTPALATE        varchar default '', --出生缺陷诊断——腭裂     
                                         v_DIAG_CLEFTLIP           varchar default '', --出生缺陷诊断——唇裂      
                                         v_DIAG_CLEFTMERGER        varchar default '', --出生缺陷诊断——唇裂合并腭裂     
                                         v_DIAG_SMALLEARS          varchar default '', --出生缺陷诊断——小耳（包括无耳）     
                                         v_DIAG_OUTEREAR           varchar default '', --出生缺陷诊断——外耳其它畸形（小耳、无耳除外）     
                                         v_DIAG_ESOPHAGEAL         varchar default '', --出生缺陷诊断——食道闭锁或狭窄     
                                         v_DIAG_RECTUM             varchar default '', --出生缺陷诊断——直肠肛门闭锁或狭窄（包括无肛）     
                                         v_DIAG_HYPOSPADIAS        varchar default '', --出生缺陷诊断——尿道下裂     
                                         v_DIAG_BLADDER            varchar default '', --出生缺陷诊断——膀胱外翻     
                                         v_DIAG_HORSESHOEFOOTLEFT  varchar default '', --出生缺陷诊断——马蹄内翻足_左      
                                         v_DIAG_HORSESHOEFOOTRIGHT varchar default '', --出生缺陷诊断——马蹄内翻足_右     
                                         v_DIAG_MANYPOINTLEFT      varchar default '', --出生缺陷诊断——多指（趾）_左      
                                         v_DIAG_MANYPOINTRIGHT     varchar default '', --出生缺陷诊断——多指（趾）_右     
                                         v_DIAG_LIMBSUPPERLEFT     varchar default '', --出生缺陷诊断——肢体短缩_上肢 _左      
                                         v_DIAG_LIMBSUPPERRIGHT    varchar default '', --出生缺陷诊断——肢体短缩_上肢 _右     
                                         v_DIAG_LIMBSLOWERLEFT     varchar default '', --出生缺陷诊断——肢体短缩_下肢 _左      
                                         v_DIAG_LIMBSLOWERRIGHT    varchar default '', --出生缺陷诊断——肢体短缩_下肢 _右     
                                         v_DIAG_HERNIA             varchar default '', --出生缺陷诊断——先天性膈疝     
                                         v_DIAG_BULGINGBELLY       varchar default '', --出生缺陷诊断——脐膨出     
                                         v_DIAG_GASTROSCHISIS      varchar default '', --出生缺陷诊断——腹裂     
                                         v_DIAG_TWINS              varchar default '', --出生缺陷诊断——联体双胎     
                                         v_DIAG_TSSYNDROME         varchar default '', --出生缺陷诊断——唐氏综合征（21-三体综合征）     
                                         v_DIAG_HEARTDISEASE       varchar default '', --出生缺陷诊断——先天性心脏病（类型）      
                                         v_DIAG_OTHER              varchar default '', --出生缺陷诊断——其他（写明病名或详细描述）      
                                         v_DIAG_OTHERCONTENT       varchar default '', --出生缺陷诊断——其他内容      
                                         v_FEVER                   varchar default '', --发烧（＞38℃）      
                                         v_VIRUSINFECTION          varchar default '', --病毒感染     
                                         v_ILLOTHER                varchar default '', --患病其他     
                                         v_SULFA                   varchar default '', --磺胺类     
                                         v_ANTIBIOTICS             varchar default '', --抗生素     
                                         v_BIRTHCONTROLPILL        varchar default '', --避孕药      
                                         v_SEDATIVES               varchar default '', --镇静药     
                                         v_MEDICINEOTHER           varchar default '', --服药其他      
                                         v_DRINKING                varchar default '', --饮酒     
                                         v_PESTICIDE               varchar default '', --农药      
                                         v_RAY                     varchar default '', --射线      
                                         v_CHEMICALAGENTS          varchar default '', --化学制剂     
                                         v_FACTOROTHER             varchar default '', --其他有害因素      
                                         v_STILLBIRTHNO            varchar default '', --死胎例数     
                                         v_ABORTIONNO              varchar default '', --自然流产例数     
                                         v_DEFECTSNO               varchar default '', --缺陷儿例数     
                                         v_DEFECTSOF1              varchar default '', --缺陷名1     
                                         v_DEFECTSOF2              varchar default '', --缺陷名2     
                                         v_DEFECTSOF3              varchar default '', --缺陷名3     
                                         v_YCDEFECTSOF1            varchar default '', --遗传缺陷名1     
                                         v_YCDEFECTSOF2            varchar default '', --遗传缺陷名2     
                                         v_YCDEFECTSOF3            varchar default '', --遗传缺陷名3     
                                         v_KINSHIPDEFECTS1         varchar default '', --与缺陷儿亲缘关系1     
                                         v_KINSHIPDEFECTS2         varchar default '', --与缺陷儿亲缘关系2     
                                         v_KINSHIPDEFECTS3         varchar default '', --与缺陷儿亲缘关系3     
                                         v_COUSINMARRIAGE          varchar default '', --近亲婚配史      
                                         v_COUSINMARRIAGEBETWEEN   varchar default '', --近亲婚配史关系     
                                         v_PREPARER                varchar default '', --填表人      
                                         v_THETITLE1               varchar default '', --填表人职称     
                                         v_FILLDATEYEAR            varchar default '', --填表日期年      
                                         v_FILLDATEMONTH           varchar default '', --填表日期月     
                                         v_FILLDATEDAY             varchar default '', --填表日期日     
                                         v_HOSPITALREVIEW          varchar default '', --医院审表人      
                                         v_THETITLE2               varchar default '', --医院审表人职称     
                                         v_HOSPITALAUDITDATEYEAR   varchar default '', --医院审表日期年     
                                         v_HOSPITALAUDITDATEMONTH  varchar default '', --医院审表日期月      
                                         v_HOSPITALAUDITDATEDAY    varchar default '', --医院审表日期日      
                                         v_PROVINCIALVIEW          varchar default '', --省级审表人      
                                         v_THETITLE3               varchar default '', --省级审表人职称     
                                         v_PROVINCIALVIEWDATEYEAR  varchar default '', --省级审表日期年      
                                         v_PROVINCIALVIEWDATEMONTH varchar default '', --省级审表日期月     
                                         v_PROVINCIALVIEWDATEDAY   varchar default '', --省级审表日期日     
                                         v_FEVERNO                 varchar default '', --发烧度数      
                                         v_ISVIRUSINFECTION        varchar default '', --是否病毒感染     
                                         v_ISDIABETES              varchar default '', --是否糖尿病      
                                         v_ISILLOTHER              varchar default '', --是否患病其他     
                                         v_ISSULFA                 varchar default '', --是否磺胺类     
                                         v_ISANTIBIOTICS           varchar default '', --是否抗生素     
                                         v_ISBIRTHCONTROLPILL      varchar default '', --是否避孕药      
                                         v_ISSEDATIVES             varchar default '', --是否镇静药     
                                         v_ISMEDICINEOTHER         varchar default '', --是否服药其他      
                                         v_ISDRINKING              varchar default '', --是否饮酒     
                                         v_ISPESTICIDE             varchar default '', --是否农药      
                                         v_ISRAY                   varchar default '', --是否射线      
                                         v_ISCHEMICALAGENTS        varchar default '', --是否化学制剂     
                                         v_ISFACTOROTHER           varchar default '', --是否其他有害因素      
                                         v_STATE                   varchar default '', -- "报告状态【 1、新增保存 2、提交 3、撤回 4、?to open this dialog next """     
                                         v_CREATE_DATE             varchar default '', --创建时间      
                                         v_CREATE_USERCODE         varchar default '', --创建人     
                                         v_CREATE_USERNAME         varchar default '', ---创建人      
                                         v_CREATE_DEPTCODE         varchar default '', --创建人科室     
                                         v_CREATE_DEPTNAME         varchar default '', --创建人科室     
                                         v_MODIFY_DATE             varchar default '', --修改时间      
                                         v_MODIFY_USERCODE         varchar default '', --修改人     
                                         v_MODIFY_USERNAME         varchar default '', --修改人     
                                         v_MODIFY_DEPTCODE         varchar default '', --修改人科室     
                                         v_MODIFY_DEPTNAME         varchar default '', --修改人科室     
                                         v_AUDIT_DATE              varchar default '', --审核时间     
                                         v_AUDIT_USERCODE          varchar default '', --审核人      
                                         v_AUDIT_USERNAME          varchar default '', --审核人      
                                         v_AUDIT_DEPTCODE          varchar default '', --审核人科室      
                                         v_AUDIT_DEPTNAME          varchar default '', --审核人科室      
                                         v_VAILD                   varchar default '', --状态是否有效  1、有效   0、无效     
                                         v_CANCELREASON            varchar default '', --否决原因     
                                         v_PRENATAL                varchar default '', --产前     
                                         v_PRENATALNO              varchar default '', --产前周数     
                                         v_POSTPARTUM              varchar default '', --产后     
                                         v_ANDTOSHOWLEFT           varchar default '', --并指左     
                                         v_ANDTOSHOWRIGHT          varchar default '', --并指右
                                         o_result                  OUT empcurtyp) AS
  
    v_ID_new int;
  BEGIN
  
    --新增传染病报告卡
    IF v_edittype = '1' THEN
    
      select SEQ_BIRTHDEFECTS.Nextval into v_ID_new from dual;
    
      insert into BIRTHDEFECTSCARD
        (ID, --序号
         REPORT_NOOFINPAT, --病案首页编号
         REPORT_ID, --  报告卡序号
         DIAG_CODE, --  病种编号
         STRING3, --  预留4
         STRING4, --  预留5
         STRING5, --  预留6
         REPORT_PROVINCE, --  报告卡省份
         REPORT_CITY, --  报告卡市（县）
         REPORT_TOWN, --  报告卡乡镇-----10
         REPORT_VILLAGE, -- 报告卡村
         REPORT_HOSPITAL, --  报告卡医院
         REPORT_NO, --右上方不明框框
         MOTHER_PATID, -- 产妇住院号
         MOTHER_NAME, --  姓名
         MOTHER_AGE, -- 年龄
         NATIONAL, -- 民族
         ADDRESS_POST, -- 地址and邮编
         PREGNANTNO, -- 孕次
         PRODUCTIONNO, -- 产次-----20
         LOCALADD, -- 常住地
         PERCAPITAINCOME, --  年人均收入
         EDUCATIONLEVEL, -- 文化程度
         CHILD_PATID, --  孩子住院号
         CHILD_NAME, -- 孩子姓名
         ISBORNHERE, -- 是否本院出生
         CHILD_SEX, --孩子性别
         BORN_YEAR, --  出生年
         BORN_MONTH, -- 出生月
         BORN_DAY, -- 出生日--30
         
         GESTATIONALAGE, -- 胎龄
         WEIGHT, -- 体重
         BIRTHS, -- 胎数
         ISIDENTICAL, --  是否同卵
         OUTCOME, --  转归
         INDUCEDLABOR, -- 是否引产
         DIAGNOSTICBASIS, --  诊断依据——临床
         DIAGNOSTICBASIS1, -- 诊断依据——超声波
         DIAGNOSTICBASIS2, -- 诊断依据——尸解
         DIAGNOSTICBASIS3, -- 诊断依据——生化检查--40
         
         DIAGNOSTICBASIS4, -- 诊断依据——生化检查——其他
         DIAGNOSTICBASIS5, -- 诊断依据——染色体
         DIAGNOSTICBASIS6, -- 诊断依据——其他
         DIAGNOSTICBASIS7, -- 诊断依据——其他——内容
         DIAG_ANENCEPHALY, -- 出生缺陷诊断——无脑畸形
         DIAG_SPINA, -- 出生缺陷诊断——脊柱裂
         DIAG_PENGOUT, -- 出生缺陷诊断——脑彭出
         DIAG_HYDROCEPHALUS, -- 出生缺陷诊断——先天性脑积水
         DIAG_CLEFTPALATE, -- 出生缺陷诊断——腭裂
         DIAG_CLEFTLIP, --  出生缺陷诊断——唇裂--50
         
         DIAG_CLEFTMERGER, -- 出生缺陷诊断——唇裂合并腭裂
         DIAG_SMALLEARS, -- 出生缺陷诊断——小耳（包括无耳）
         DIAG_OUTEREAR, --  出生缺陷诊断——外耳其它畸形（小耳、无耳除外）
         DIAG_ESOPHAGEAL, --  出生缺陷诊断——食道闭锁或狭窄
         DIAG_RECTUM, --  出生缺陷诊断——直肠肛门闭锁或狭窄（包括无肛）
         DIAG_HYPOSPADIAS, -- 出生缺陷诊断——尿道下裂
         DIAG_BLADDER, -- 出生缺陷诊断——膀胱外翻
         DIAG_HORSESHOEFOOTLEFT, -- 出生缺陷诊断——马蹄内翻足_左 
         DIAG_HORSESHOEFOOTRIGHT, --  出生缺陷诊断——马蹄内翻足_右
         DIAG_MANYPOINTLEFT, -- 出生缺陷诊断——多指（趾）_左--60
         
         DIAG_MANYPOINTRIGHT, --  出生缺陷诊断——多指（趾）_右
         DIAG_LIMBSUPPERLEFT, --  出生缺陷诊断——肢体短缩_上肢 _左
         DIAG_LIMBSUPPERRIGHT, -- 出生缺陷诊断——肢体短缩_上肢 _右
         DIAG_LIMBSLOWERLEFT, --  出生缺陷诊断——肢体短缩_下肢 _左
         DIAG_LIMBSLOWERRIGHT, -- 出生缺陷诊断——肢体短缩_下肢 _右
         DIAG_HERNIA, --  出生缺陷诊断——先天性膈疝
         DIAG_BULGINGBELLY, --  出生缺陷诊断——脐膨出
         DIAG_GASTROSCHISIS, -- 出生缺陷诊断——腹裂
         DIAG_TWINS, -- 出生缺陷诊断——联体双胎
         DIAG_TSSYNDROME, --  出生缺陷诊断——唐氏综合征（21-三体综合征）--70
         
         DIAG_HEARTDISEASE, --  出生缺陷诊断——先天性心脏病（类型）
         DIAG_OTHER, -- 出生缺陷诊断——其他（写明病名或详细描述）
         DIAG_OTHERCONTENT, --  出生缺陷诊断——其他内容
         FEVER, --  发烧（＞38℃）
         VIRUSINFECTION, -- 病毒感染
         ILLOTHER, -- 患病其他
         SULFA, --  磺胺类
         ANTIBIOTICS, --  抗生素
         BIRTHCONTROLPILL, -- 避孕药
         SEDATIVES, --  镇静药
         
         MEDICINEOTHER, --  服药其他
         DRINKING, -- 饮酒
         PESTICIDE, --  农药
         RAY, --  射线
         CHEMICALAGENTS, -- 化学制剂
         FACTOROTHER, --  其他有害因素
         STILLBIRTHNO, -- 死胎例数
         ABORTIONNO, -- 自然流产例数
         DEFECTSNO, --  缺陷儿例数
         DEFECTSOF1, -- 缺陷名1--90
         
         DEFECTSOF2, -- 缺陷名2
         DEFECTSOF3, -- 缺陷名3
         YCDEFECTSOF1, -- 遗传缺陷名1
         YCDEFECTSOF2, -- 遗传缺陷名2
         YCDEFECTSOF3, -- 遗传缺陷名3
         KINSHIPDEFECTS1, --  与缺陷儿亲缘关系1
         KINSHIPDEFECTS2, --  与缺陷儿亲缘关系2
         KINSHIPDEFECTS3, --  与缺陷儿亲缘关系3
         COUSINMARRIAGE, -- 近亲婚配史
         COUSINMARRIAGEBETWEEN, --  近亲婚配史关系
         PREPARER, -- 填表人
         THETITLE1, --  填表人职称
         FILLDATEYEAR, -- 填表日期年
         FILLDATEMONTH, --  填表日期月
         FILLDATEDAY, --  填表日期日
         HOSPITALREVIEW, -- 医院审表人
         THETITLE2, --  医院审表人职称
         HOSPITALAUDITDATEYEAR, --  医院审表日期年
         HOSPITALAUDITDATEMONTH, -- 医院审表日期月
         HOSPITALAUDITDATEDAY, -- 医院审表日期日
         PROVINCIALVIEW, -- 省级审表人
         THETITLE3, --  省级审表人职称
         PROVINCIALVIEWDATEYEAR, -- 省级审表日期年
         PROVINCIALVIEWDATEMONTH, --  省级审表日期月
         PROVINCIALVIEWDATEDAY, --  省级审表日期日
         FEVERNO, --  发烧度数
         ISVIRUSINFECTION, -- 是否病毒感染
         ISDIABETES, -- 是否糖尿病
         ISILLOTHER, -- 是否患病其他
         ISSULFA, --  是否磺胺类--120
         
         ISANTIBIOTICS, --  是否抗生素
         ISBIRTHCONTROLPILL, -- 是否避孕药
         ISSEDATIVES, --  是否镇静药
         ISMEDICINEOTHER, --  是否服药其他
         ISDRINKING, -- 是否饮酒
         ISPESTICIDE, --  是否农药
         ISRAY, --  是否射线
         ISCHEMICALAGENTS, -- 是否化学制剂
         ISFACTOROTHER, --  是否其他有害因素
         STATE, --  "报告状态【 1、新增保存 2、提交 3、撤回 4、?to open this dialog next """
         CREATE_DATE, --  创建时间
         CREATE_USERCODE, --  创建人
         CREATE_USERNAME, --  创建人
         CREATE_DEPTCODE, --  创建人科室
         CREATE_DEPTNAME, --  创建人科室
         MODIFY_DATE, --  修改时间
         MODIFY_USERCODE, --  修改人
         MODIFY_USERNAME, --  修改人
         MODIFY_DEPTCODE, --  修改人科室
         MODIFY_DEPTNAME, --  修改人科室
         AUDIT_DATE, -- 审核时间
         AUDIT_USERCODE, -- 审核人
         AUDIT_USERNAME, -- 审核人
         AUDIT_DEPTCODE, -- 审核人科室
         AUDIT_DEPTNAME, -- 审核人科室
         VAILD, --  状态是否有效  1、有效   0、无效
         CANCELREASON, -- 否决原因
         PRENATAL, -- 产前
         PRENATALNO, -- 产前周数
         POSTPARTUM, -- 产后--150
         
         ANDTOSHOWLEFT, --  并指左
         ANDTOSHOWRIGHT) --并指右
      
      values
        (v_ID_new, --序号
         v_REPORT_NOOFINPAT,
         v_Report_ID,
         v_DIAG_CODE, --报告卡诊断编码
         v_STRING3, --预留
         v_STRING4, --预留
         v_STRING5, --预留
         v_REPORT_PROVINCE, --上报告卡省份
         v_REPORT_CITY, --报告卡市（县）
         v_REPORT_TOWN, --报告卡乡镇--------10
         v_REPORT_VILLAGE, --报告卡村
         v_REPORT_HOSPITAL, --报告卡医院
         v_REPORT_NO, --报告卡序号
         v_MOTHER_PATID, --产妇住院号
         v_MOTHER_NAME, --姓名
         v_MOTHER_AGE, --年龄
         v_NATIONAL, --民族
         v_ADDRESS_POST, --地址and邮编
         v_PREGNANTNO, --孕次
         v_PRODUCTIONNO, --产次----------=20
         v_LOCALADD, --常住地
         v_PERCAPITAINCOME, --  年人均收入
         v_EDUCATIONLEVEL, -- 文化程度
         v_CHILD_PATID, --  孩子住院号
         v_CHILD_NAME, -- 孩子姓名
         v_ISBORNHERE, --是否本院出生
         v_CHILD_SEX, --孩子性别
         v_BORN_YEAR, --出生年
         v_BORN_MONTH, -- 出生月
         v_BORN_DAY, --出生日---------------30
         v_GESTATIONALAGE, --胎龄
         v_WEIGHT, --体重
         v_BIRTHS, --胎数
         v_ISIDENTICAL, --是否同卵
         v_OUTCOME, --转归
         v_INDUCEDLABOR, --是否引产
         v_DIAGNOSTICBASIS, --诊断依据——临床
         v_DIAGNOSTICBASIS1, -- 诊断依据——超声波
         v_DIAGNOSTICBASIS2, --诊断依据——尸解
         v_DIAGNOSTICBASIS3, --诊断依据——生化检查
         v_DIAGNOSTICBASIS4, --诊断依据——生化检查——其他
         v_DIAGNOSTICBASIS5, --诊断依据——染色体
         v_DIAGNOSTICBASIS6, --诊断依据——其他
         v_DIAGNOSTICBASIS7, --诊断依据——其他——内容
         v_DIAG_ANENCEPHALY, --出生缺陷诊断——无脑畸形
         v_DIAG_SPINA, --出生缺陷诊断——脊柱裂
         v_DIAG_PENGOUT, --出生缺陷诊断——脑彭出
         v_DIAG_HYDROCEPHALUS, --出生缺陷诊断——先天性脑积水
         v_DIAG_CLEFTPALATE, --出生缺陷诊断——腭裂
         v_DIAG_CLEFTLIP, --出生缺陷诊断——唇裂------------50
         v_DIAG_CLEFTMERGER, --出生缺陷诊断——唇裂合并腭裂
         v_DIAG_SMALLEARS, --出生缺陷诊断——小耳（包括无耳）
         v_DIAG_OUTEREAR, --出生缺陷诊断——外耳其它畸形（小耳、无耳除外）
         v_DIAG_ESOPHAGEAL, --出生缺陷诊断——食道闭锁或狭窄
         v_DIAG_RECTUM, --出生缺陷诊断——直肠肛门闭锁或狭窄（包括无肛）
         v_DIAG_HYPOSPADIAS, --出生缺陷诊断——尿道下裂
         v_DIAG_BLADDER, --出生缺陷诊断——膀胱外翻
         v_DIAG_HORSESHOEFOOTLEFT, --出生缺陷诊断——马蹄内翻足_左 
         v_DIAG_HORSESHOEFOOTRIGHT, --出生缺陷诊断——马蹄内翻足_右
         v_DIAG_MANYPOINTLEFT, --出生缺陷诊断——多指（趾）_左
         v_DIAG_MANYPOINTRIGHT, --出生缺陷诊断——多指（趾）_右
         v_DIAG_LIMBSUPPERLEFT, --出生缺陷诊断——肢体短缩_上肢 _左
         v_DIAG_LIMBSUPPERRIGHT, --出生缺陷诊断——肢体短缩_上肢 _右
         v_DIAG_LIMBSLOWERLEFT, --  出生缺陷诊断——肢体短缩_下肢 _左
         v_DIAG_LIMBSLOWERRIGHT, -- 出生缺陷诊断——肢体短缩_下肢 _右
         v_DIAG_HERNIA, --  出生缺陷诊断——先天性膈疝
         v_DIAG_BULGINGBELLY, --出生缺陷诊断——脐膨出
         v_DIAG_GASTROSCHISIS, --出生缺陷诊断——腹裂
         v_DIAG_TWINS, --出生缺陷诊断——联体双胎
         v_DIAG_TSSYNDROME, --出生缺陷诊断——唐氏综合征（21-三体综合征）
         v_DIAG_HEARTDISEASE, --出生缺陷诊断——先天性心脏病（类型）
         v_DIAG_OTHER, --出生缺陷诊断——其他（写明病名或详细描述）
         v_DIAG_OTHERCONTENT, --出生缺陷诊断——其他内容
         v_FEVER, --发烧（＞38℃）
         v_VIRUSINFECTION, --病毒感染
         v_ILLOTHER, --患病其他
         v_SULFA, --磺胺类
         v_ANTIBIOTICS, --抗生素
         v_BIRTHCONTROLPILL, -- 避孕药
         v_SEDATIVES, --镇静药---------------80
         v_MEDICINEOTHER, --服药其他
         v_DRINKING, -- 饮酒
         v_PESTICIDE, --  农药
         v_RAY, --射线
         v_CHEMICALAGENTS, --化学制剂
         v_FACTOROTHER, --其他有害因素
         v_STILLBIRTHNO, -- 死胎例数
         v_ABORTIONNO, --自然流产例数
         v_DEFECTSNO, --缺陷儿例数
         v_DEFECTSOF1, --缺陷名1--------------90
         v_DEFECTSOF2, --缺陷名2
         v_DEFECTSOF3, --缺陷名3
         v_YCDEFECTSOF1, --遗传缺陷名1
         v_YCDEFECTSOF2, -- 遗传缺陷名2
         v_YCDEFECTSOF3, --遗传缺陷名3
         v_KINSHIPDEFECTS1, --与缺陷儿亲缘关系1
         v_KINSHIPDEFECTS2, --与缺陷儿亲缘关系2
         v_KINSHIPDEFECTS3, --与缺陷儿亲缘关系3
         v_COUSINMARRIAGE, --近亲婚配史
         v_COUSINMARRIAGEBETWEEN, --近亲婚配史关系
         v_PREPARER, --填表人
         v_THETITLE1, --填表人职称
         v_FILLDATEYEAR, --填表日期年
         v_FILLDATEMONTH, --填表日期月
         v_FILLDATEDAY, --填表日期日
         v_HOSPITALREVIEW, --医院审表人
         v_THETITLE2, --医院审表人职称
         v_HOSPITALAUDITDATEYEAR, --医院审表日期年
         v_HOSPITALAUDITDATEMONTH, --医院审表日期月
         v_HOSPITALAUDITDATEDAY, --医院审表日期日
         v_PROVINCIALVIEW, --省级审表人
         v_THETITLE3, --省级审表人职称
         v_PROVINCIALVIEWDATEYEAR, --省级审表日期年
         v_PROVINCIALVIEWDATEMONTH, --省级审表日期月
         v_PROVINCIALVIEWDATEDAY, --省级审表日期日
         v_FEVERNO, --发烧度数
         v_ISVIRUSINFECTION, --是否病毒感染
         v_ISDIABETES, --是否糖尿病
         v_ISILLOTHER, --是否患病其他
         v_ISSULFA, --是否磺胺类----------------120
         v_ISANTIBIOTICS, --是否抗生素
         v_ISBIRTHCONTROLPILL, --是否避孕药
         v_ISSEDATIVES, --是否镇静药
         v_ISMEDICINEOTHER, --是否服药其他
         v_ISDRINKING, --是否饮酒
         v_ISPESTICIDE, --是否农药
         v_ISRAY, --是否射线
         v_ISCHEMICALAGENTS, --是否化学制剂
         v_ISFACTOROTHER, --是否其他有害因素
         v_STATE, --"报告状态【 1、新增保存 2、提交 3、撤回 4、?to open this dialog next """
         v_CREATE_DATE, --创建时间
         v_CREATE_USERCODE, --创建人
         v_CREATE_USERNAME, --创建人
         v_CREATE_DEPTCODE, --创建人科室
         v_CREATE_DEPTNAME, --创建人科室
         v_MODIFY_DATE, --修改时间
         v_MODIFY_USERCODE, --修改人
         v_MODIFY_USERNAME, --修改人
         v_MODIFY_DEPTCODE, --修改人科室
         v_MODIFY_DEPTNAME, --修改人科室
         v_AUDIT_DATE, --审核时间
         v_AUDIT_USERCODE, --审核人
         v_AUDIT_USERNAME, --审核人
         v_AUDIT_DEPTCODE, -- 审核人科室
         v_AUDIT_DEPTNAME, --审核人科室
         v_VAILD, --状态是否有效  1、有效   0、无效
         v_CANCELREASON, --否决原因
         v_PRENATAL, --产前
         v_PRENATALNO, --产前周数
         v_POSTPARTUM, --产后--------------150
         v_ANDTOSHOWLEFT, --并指左
         v_ANDTOSHOWRIGHT); --并指右
    
      open o_result for
        select v_ID_new from dual;
    
    end if;
  
    --修改保存传染病报告卡信息
    IF v_edittype = '2' THEN
    
      update BIRTHDEFECTSCARD
         set REPORT_NOOFINPAT        = v_REPORT_NOOFINPAT,
             REPORT_ID               = v_Report_ID, --报告卡序号        
             DIAG_CODE               = v_DIAG_CODE,
             STRING3                 = v_STRING3,
             STRING4                 = v_STRING4,
             STRING5                 = v_STRING5,
             REPORT_PROVINCE         = v_REPORT_PROVINCE, --报告卡省份
             REPORT_CITY             = v_REPORT_CITY, --报告卡市（县）
             REPORT_TOWN             = v_REPORT_TOWN, --报告卡乡镇
             REPORT_VILLAGE          = v_REPORT_VILLAGE, --报告卡村
             REPORT_HOSPITAL         = v_REPORT_HOSPITAL, --报告卡医院
             REPORT_NO               = v_REPORT_NO, --右上方不明框框
             MOTHER_PATID            = v_MOTHER_PATID, --产妇住院号
             MOTHER_NAME             = v_MOTHER_NAME, --姓名
             MOTHER_AGE              = v_MOTHER_AGE, --年龄
             NATIONAL                = v_NATIONAL, --民族
             ADDRESS_POST            = v_ADDRESS_POST, --地址and邮编
             PREGNANTNO              = v_PREGNANTNO, --孕次
             PRODUCTIONNO            = v_PRODUCTIONNO, --产次
             LOCALADD                = v_LOCALADD, --常住地
             PERCAPITAINCOME         = v_PERCAPITAINCOME, --年人均收入
             EDUCATIONLEVEL          = v_EDUCATIONLEVEL, --文化程度
             CHILD_PATID             = v_CHILD_PATID, --孩子住院号
             CHILD_NAME              = v_CHILD_NAME, --  孩子姓名
             ISBORNHERE              = v_ISBORNHERE, --是否本院出生
             CHILD_SEX               = v_CHILD_SEX, --孩子性别
             BORN_YEAR               = v_BORN_YEAR, --出生年
             BORN_MONTH              = v_BORN_MONTH, --出生月
             BORN_DAY                = v_BORN_DAY, --出生日
             GESTATIONALAGE          = v_GESTATIONALAGE, --  胎龄
             WEIGHT                  = v_WEIGHT, --体重
             BIRTHS                  = v_BIRTHS, --胎数
             ISIDENTICAL             = v_ISIDENTICAL, --是否同卵
             OUTCOME                 = v_OUTCOME, --转归
             INDUCEDLABOR            = v_INDUCEDLABOR, --是否引产
             DIAGNOSTICBASIS         = v_DIAGNOSTICBASIS, --诊断依据——临床
             DIAGNOSTICBASIS1        = v_DIAGNOSTICBASIS1, --诊断依据——超声波
             DIAGNOSTICBASIS2        = v_DIAGNOSTICBASIS2, --诊断依据——尸解
             DIAGNOSTICBASIS3        = v_DIAGNOSTICBASIS3, --诊断依据——生化检查
             DIAGNOSTICBASIS4        = v_DIAGNOSTICBASIS4, --诊断依据——生化检查——其他
             DIAGNOSTICBASIS5        = v_DIAGNOSTICBASIS5, --诊断依据——染色体
             DIAGNOSTICBASIS6        = v_DIAGNOSTICBASIS6, --诊断依据——其他
             DIAGNOSTICBASIS7        = v_DIAGNOSTICBASIS7, --诊断依据——其他——内容
             DIAG_ANENCEPHALY        = v_DIAG_ANENCEPHALY, --出生缺陷诊断——无脑畸形
             DIAG_SPINA              = v_DIAG_SPINA, --出生缺陷诊断——脊柱裂
             DIAG_PENGOUT            = v_DIAG_PENGOUT, --出生缺陷诊断——脑彭出
             DIAG_HYDROCEPHALUS      = v_DIAG_HYDROCEPHALUS, --出生缺陷诊断——先天性脑积水
             DIAG_CLEFTPALATE        = v_DIAG_CLEFTPALATE, --出生缺陷诊断——腭裂
             DIAG_CLEFTLIP           = v_DIAG_CLEFTLIP, --出生缺陷诊断——唇裂
             DIAG_CLEFTMERGER        = v_DIAG_CLEFTMERGER, --出生缺陷诊断——唇裂合并腭裂
             DIAG_SMALLEARS          = v_DIAG_SMALLEARS, --出生缺陷诊断——小耳（包括无耳）
             DIAG_OUTEREAR           = v_DIAG_OUTEREAR, --出生缺陷诊断——外耳其它畸形（小耳、无耳除外）
             DIAG_ESOPHAGEAL         = v_DIAG_ESOPHAGEAL, --出生缺陷诊断——食道闭锁或狭窄
             DIAG_RECTUM             = v_DIAG_RECTUM, --出生缺陷诊断——直肠肛门闭锁或狭窄（包括无肛）
             DIAG_HYPOSPADIAS        = v_DIAG_HYPOSPADIAS, --出生缺陷诊断——尿道下裂
             DIAG_BLADDER            = v_DIAG_BLADDER, --出生缺陷诊断——膀胱外翻
             DIAG_HORSESHOEFOOTLEFT  = v_DIAG_HORSESHOEFOOTLEFT, --出生缺陷诊断——马蹄内翻足_左 
             DIAG_HORSESHOEFOOTRIGHT = v_DIAG_HORSESHOEFOOTRIGHT, --  出生缺陷诊断——马蹄内翻足_右
             DIAG_MANYPOINTLEFT      = v_DIAG_MANYPOINTLEFT, --出生缺陷诊断——多指（趾）_左
             DIAG_MANYPOINTRIGHT     = v_DIAG_MANYPOINTRIGHT, --出生缺陷诊断——多指（趾）_右
             DIAG_LIMBSUPPERLEFT     = v_DIAG_LIMBSUPPERLEFT, --出生缺陷诊断——肢体短缩_上肢 _左
             DIAG_LIMBSUPPERRIGHT    = v_DIAG_LIMBSUPPERRIGHT, --出生缺陷诊断——肢体短缩_上肢 _右
             DIAG_LIMBSLOWERLEFT     = v_DIAG_LIMBSLOWERLEFT, --出生缺陷诊断——肢体短缩_下肢 _左
             DIAG_LIMBSLOWERRIGHT    = v_DIAG_LIMBSLOWERRIGHT, ---出生缺陷诊断——肢体短缩_下肢 _右
             DIAG_HERNIA             = v_DIAG_HERNIA, --出生缺陷诊断——先天性膈疝
             DIAG_BULGINGBELLY       = v_DIAG_BULGINGBELLY, --出生缺陷诊断——脐膨出
             DIAG_GASTROSCHISIS      = v_DIAG_GASTROSCHISIS, --出生缺陷诊断——腹裂
             DIAG_TWINS              = v_DIAG_TWINS, --出生缺陷诊断——联体双胎
             DIAG_TSSYNDROME         = v_DIAG_TSSYNDROME, --出生缺陷诊断——唐氏综合征（21-三体综合征）
             DIAG_HEARTDISEASE       = v_DIAG_HEARTDISEASE, --出生缺陷诊断——先天性心脏病（类型）
             DIAG_OTHER              = v_DIAG_OTHER, --出生缺陷诊断——其他（写明病名或详细描述）
             DIAG_OTHERCONTENT       = v_DIAG_OTHERCONTENT, --出生缺陷诊断——其他内容
             FEVER                   = v_FEVER, --发烧（＞38℃）
             VIRUSINFECTION          = v_VIRUSINFECTION, --病毒感染
             ILLOTHER                = v_ILLOTHER, --患病其他
             SULFA                   = v_SULFA, --磺胺类
             ANTIBIOTICS             = v_ANTIBIOTICS, --抗生素
             BIRTHCONTROLPILL        = v_BIRTHCONTROLPILL, --避孕药
             SEDATIVES               = v_SEDATIVES, --镇静药
             MEDICINEOTHER           = v_MEDICINEOTHER, --服药其他
             DRINKING                = v_DRINKING, --饮酒
             PESTICIDE               = v_PESTICIDE, --农药
             RAY                     = v_RAY, --射线
             CHEMICALAGENTS          = v_CHEMICALAGENTS, --  化学制剂
             FACTOROTHER             = v_FACTOROTHER, --其他有害因素
             STILLBIRTHNO            = v_STILLBIRTHNO, --  死胎例数
             ABORTIONNO              = v_ABORTIONNO, --自然流产例数
             DEFECTSNO               = v_DEFECTSNO, --缺陷儿例数
             DEFECTSOF1              = v_DEFECTSOF1, --缺陷名1
             DEFECTSOF2              = v_DEFECTSOF2, --缺陷名2
             DEFECTSOF3              = v_DEFECTSOF3, --缺陷名3
             YCDEFECTSOF1            = v_YCDEFECTSOF1, --遗传缺陷名1
             YCDEFECTSOF2            = v_YCDEFECTSOF2, --遗传缺陷名2
             YCDEFECTSOF3            = v_YCDEFECTSOF3, --遗传缺陷名3
             KINSHIPDEFECTS1         = v_KINSHIPDEFECTS1, --与缺陷儿亲缘关系1
             KINSHIPDEFECTS2         = v_KINSHIPDEFECTS2, --与缺陷儿亲缘关系2
             KINSHIPDEFECTS3         = v_KINSHIPDEFECTS3, --与缺陷儿亲缘关系3
             COUSINMARRIAGE          = v_COUSINMARRIAGE, --近亲婚配史
             COUSINMARRIAGEBETWEEN   = v_COUSINMARRIAGEBETWEEN, --近亲婚配史关系
             PREPARER                = v_PREPARER, --填表人
             THETITLE1               = v_THETITLE1, --填表人职称
             FILLDATEYEAR            = v_FILLDATEYEAR, --填表日期年
             FILLDATEMONTH           = v_FILLDATEMONTH, --填表日期月
             FILLDATEDAY             = v_FILLDATEDAY, --填表日期日
             HOSPITALREVIEW          = v_HOSPITALREVIEW, --医院审表人
             THETITLE2               = v_THETITLE2, --医院审表人职称
             HOSPITALAUDITDATEYEAR   = v_HOSPITALAUDITDATEYEAR, --医院审表日期年
             HOSPITALAUDITDATEMONTH  = v_HOSPITALAUDITDATEMONTH, --医院审表日期月
             HOSPITALAUDITDATEDAY    = v_HOSPITALAUDITDATEDAY, --医院审表日期日
             PROVINCIALVIEW          = v_PROVINCIALVIEW, --省级审表人
             THETITLE3               = v_THETITLE3, --省级审表人职称
             PROVINCIALVIEWDATEYEAR  = v_PROVINCIALVIEWDATEYEAR, --省级审表日期年
             PROVINCIALVIEWDATEMONTH = v_PROVINCIALVIEWDATEMONTH, --省级审表日期月
             PROVINCIALVIEWDATEDAY   = v_PROVINCIALVIEWDATEDAY, --  省级审表日期日
             FEVERNO                 = v_FEVERNO, --发烧度数
             ISVIRUSINFECTION        = v_ISVIRUSINFECTION, --是否病毒感染
             ISDIABETES              = v_ISDIABETES, --是否糖尿病
             ISILLOTHER              = v_ISILLOTHER, --是否患病其他
             ISSULFA                 = v_ISSULFA, --是否磺胺类
             ISANTIBIOTICS           = v_ISANTIBIOTICS, --是否抗生素
             ISBIRTHCONTROLPILL      = v_ISBIRTHCONTROLPILL, --是否避孕药
             ISSEDATIVES             = v_ISSEDATIVES, --是否镇静药
             ISMEDICINEOTHER         = v_ISMEDICINEOTHER, --是否服药其他
             ISDRINKING              = v_ISDRINKING, --是否饮酒
             ISPESTICIDE             = v_ISPESTICIDE, --是否农药
             ISRAY                   = v_ISRAY, --是否射线
             ISCHEMICALAGENTS        = v_ISCHEMICALAGENTS, --是否化学制剂
             ISFACTOROTHER           = v_ISFACTOROTHER, --是否其他有害因素
             STATE                   = v_STATE, --"报告状态【 1、新增保存 2、提交 3、撤回 4、?to open this dialog next """
             CREATE_DATE             = v_CREATE_DATE, --创建时间
             CREATE_USERCODE         = v_CREATE_USERCODE, --创建人
             CREATE_USERNAME         = v_CREATE_USERNAME, --创建人
             CREATE_DEPTCODE         = v_CREATE_DEPTCODE, --创建人科室
             CREATE_DEPTNAME         = v_CREATE_DEPTNAME, --创建人科室
             MODIFY_DATE             = v_MODIFY_DATE, --修改时间
             MODIFY_USERCODE         = v_MODIFY_USERCODE, --修改人
             MODIFY_USERNAME         = v_MODIFY_USERNAME, --修改人
             MODIFY_DEPTCODE         = v_MODIFY_DEPTCODE, --修改人科室
             MODIFY_DEPTNAME         = v_MODIFY_DEPTNAME, --修改人科室
             AUDIT_DATE              = v_AUDIT_DATE, --审核时间
             AUDIT_USERCODE          = v_AUDIT_USERCODE, --  审核人
             AUDIT_USERNAME          = v_AUDIT_USERNAME, --审核人
             AUDIT_DEPTCODE          = v_AUDIT_DEPTCODE, --审核人科室
             AUDIT_DEPTNAME          = v_AUDIT_DEPTNAME, --审核人科室
             VAILD                   = v_VAILD, --状态是否有效  1、有效   0、无效
             CANCELREASON            = v_CANCELREASON, --否决原因
             PRENATAL                = v_PRENATAL, --产前
             PRENATALNO              = v_PRENATALNO, --产前周数
             POSTPARTUM              = v_POSTPARTUM, --产后
             ANDTOSHOWLEFT           = v_ANDTOSHOWLEFT, --  并指左
             ANDTOSHOWRIGHT          = v_ANDTOSHOWRIGHT --并指右
      
       where ID = v_ID;
    
      open o_result for
        select v_ID from dual;
    
    end if;
  
    --作废传染病报告卡信息     
  
    --根据传入的传染病报告卡ID查询报告卡信息
    IF v_edittype = '4' THEN
    
      open o_result for
        select * from BIRTHDEFECTSCARD a where a.report_id = v_ID;
    
    end if;
  
  end;
  --------出生缺陷报告卡

  PROCEDURE usp_EditTherioma_Report(v_EditType              varchar,
                                    v_Report_ID             NUMERIC DEFAULT 0,
                                    v_REPORT_DISTRICTID     varchar DEFAULT '', --传染病上报卡表区(县)编码
                                    v_REPORT_DISTRICTNAME   varchar default '', --传染病上报卡表区(县)名称
                                    v_REPORT_ICD10          varchar default '', --传染病报告卡ICD-10编码
                                    v_REPORT_ICD0           varchar default '', --传染病报告卡ICD-0编码
                                    v_REPORT_CLINICID       varchar default '', --门诊号
                                    v_REPORT_PATID          varchar default '', --住院号
                                    v_REPORT_INDO           varchar default '', --身份证号码
                                    v_REPORT_INPATNAME      varchar default '', --病患姓名
                                    v_SEXID                 varchar default '', --病患性别
                                    v_REALAGE               varchar default '', --病患实足年龄
                                    v_BIRTHDATE             varchar default '', --病患生日
                                    v_NATIONID              varchar default '', --病患民族编号
                                    v_NATIONNAME            varchar default '', --病患民族全称
                                    v_CONTACTTEL            varchar default '', --家庭电话
                                    v_MARTIAL               varchar default '', --婚姻状况
                                    v_OCCUPATION            varchar default '', --病患职业
                                    v_OFFICEADDRESS         varchar default '', --工作单位地址
                                    v_ORGPROVINCEID         varchar default '', --户口地址省份编码
                                    v_ORGCITYID             varchar default '', --户口地址所在市编码
                                    v_ORGDISTRICTID         varchar default '', --户口所在地区县编码
                                    v_ORGTOWNID             varchar default '', --户口所在地镇(街道)编码
                                    v_ORGVILLIAGE           varchar default '', --户口所在地居委会对应编码
                                    v_ORGPROVINCENAME       varchar default '', --户口所在地省份全称
                                    v_ORGCITYNAME           varchar default '', --户口所在地市全名称
                                    v_ORGDISTRICTNAME       varchar default '', --户口所在地区(县)全称
                                    v_ORGTOWN               varchar default '', --户口所在地镇全称
                                    v_ORGVILLAGENAME        varchar default '', --户口所在地村全称
                                    v_XZZPROVINCEID         varchar default '', --现住址所在省份编码
                                    v_XZZCITYID             varchar default '', --现住址所在市编码
                                    v_XZZDISTRICTID         varchar default '', --现住址所在区(县)代码
                                    v_XZZTOWNID             varchar default '', --现住址所在镇代码
                                    v_XZZVILLIAGEID         varchar default '', --报现住址所在村编码
                                    v_XZZPROVINCE           varchar default '', --现住址所在省份全称
                                    v_XZZCITY               varchar default '', --现住址所在市全称
                                    v_XZZDISTRICT           varchar default '', --现住址所在区全称
                                    v_XZZTOWN               varchar default '', --现住址所在镇全称
                                    v_XZZVILLIAGE           varchar default '', --现住址所在村全称
                                    v_REPORT_DIAGNOSIS      varchar default '', --诊断
                                    v_PATHOLOGICALTYPE      varchar default '', --病理类型
                                    v_PATHOLOGICALID        varchar default '', --病理诊断病理号
                                    v_QZDIAGTIME_T          varchar default '', --确诊时期_T期
                                    v_QZDIAGTIME_N          varchar default '', --确诊时期_N期
                                    v_QZDIAGTIME_M          varchar default '', --确诊时期_M期
                                    v_FIRSTDIADATE          varchar default '', --首次确诊时间
                                    v_REPORTINFUNIT         varchar default '', --报告单位
                                    v_REPORTDOCTOR          varchar default '', --报告医生
                                    v_REPORTDATE            varchar default '', --报告时间
                                    v_DEATHDATE             varchar default '', --死亡时间
                                    v_DEATHREASON           varchar default '', --死亡原因
                                    v_REJEST                varchar default '', --病历摘要
                                    v_REPORT_YDIAGNOSIS     varchar default '', --原诊断
                                    v_REPORT_YDIAGNOSISDATA varchar default '', --原诊断日期
                                    v_REPORT_DIAGNOSISBASED varchar default '', --诊断依据
                                    v_REPORT_NO             varchar default '', --传染病上报卡表编号
                                    v_REPORT_NOOFINPAT      varchar default '', --患者ID
                                    v_STATE                 varchar default '', --报告状态【 1、新增保存 2、提交 3、撤回 4、审核通过 5、审核未通过撤回 6、上报  7、作废】
                                    v_CREATE_DATE           varchar default '', --创建人日期  
                                    v_CREATE_USERCODE       varchar default '', --创建人
                                    v_CREATE_USERNAME       varchar default '', --创建人
                                    v_CREATE_DEPTCODE       varchar default '', --创建人科室
                                    v_CREATE_DEPTNAME       varchar default '', --创建人科室
                                    v_MODIFY_DATE           varchar default '', --修改时间
                                    v_MODIFY_USERCODE       varchar default '', --修改人
                                    v_MODIFY_USERNAME       varchar default '', --修改人
                                    v_MODIFY_DEPTCODE       varchar default '', --修改人科室
                                    v_MODIFY_DEPTNAME       varchar default '', --修改人科室
                                    v_AUDIT_DATE            varchar default '', --审核时间
                                    v_AUDIT_USERCODE        varchar default '', --审核人
                                    v_AUDIT_USERNAME        varchar default '', --审核人
                                    v_AUDIT_DEPTCODE        varchar default '', --审核人科室
                                    v_AUDIT_DEPTNAME        varchar default '', --审核人科室
                                    v_VAILD                 varchar default '',
                                    v_DIAGICD10             varchar default '',
                                    v_CANCELREASON          varchar default '', --否决原因   
                                    V_CARDTYPE              varchar default '', --卡片类型死亡或者发病                   
                                    V_clinicalstages        varchar default '',
                                    V_ReportDiagfunit       varchar default '',
                                    o_result                OUT empcurtyp) AS
  
    v_Report_ID_new int;
  BEGIN
  
    --新增传染病报告卡
    IF v_edittype = '1' THEN
    
      select SEQ_THERIOMAID.Nextval into v_Report_ID_new from dual;
    
      insert into THERIOMAREPORTCARD
        (REPORT_ID, --序号
         REPORT_DISTRICTID, --传染病上报卡表区(县)编码
         REPORT_DISTRICTNAME, --传染病上报卡表区(县)名称
         REPORT_ICD10, --传染病报告卡ICD-10编码
         REPORT_ICD0, --传染病报告卡ICD-0编码
         REPORT_CLINICID, --门诊号
         REPORT_PATID, --住院号
         REPORT_INDO, --身份证号码
         REPORT_INPATNAME, --病患姓名
         SEXID, --病患性别
         REALAGE, --病患实足年龄(在诊断时未过生日者为虚年龄减二岁，已过生日者为虚年龄减一岁；未满一岁者为0岁)
         BIRTHDATE, --病患生日
         NATIONID, --病患民族编号
         NATIONNAME, --病患民族全称
         CONTACTTEL, --家庭电话
         MARTIAL, --婚姻状况
         OCCUPATION, --病患职业
         OFFICEADDRESS, --工作单位地址
         ORGPROVINCEID, --户口地址省份编码
         ORGCITYID, --户口地址所在市编码
         ORGDISTRICTID, --户口所在地区县编码
         ORGTOWNID, --户口所在地镇(街道)编码
         ORGVILLIAGE, --户口所在地居委会对应编码
         ORGPROVINCENAME, --户口所在地省份全称
         ORGCITYNAME, --户口所在地市全名称
         ORGDISTRICTNAME, --户口所在地区(县)全称
         ORGTOWN, --户口所在地镇全称
         ORGVILLAGENAME, --户口所在地村全称
         XZZPROVINCEID, --现住址所在省份编码
         XZZCITYID, --现住址所在市编码
         XZZDISTRICTID, --现住址所在区(县)代码
         XZZTOWNID, --现住址所在镇代码
         XZZVILLIAGEID, --现住址所在村编码
         XZZPROVINCE, --现住址所在省份全称
         XZZCITY, --现住址所在市全称
         XZZDISTRICT, --现住址所在区全称
         XZZTOWN, --现住址所在镇全称
         XZZVILLIAGE, --现住址所在村全称
         REPORT_DIAGNOSIS, --诊断
         PATHOLOGICALTYPE, --病理类型
         PATHOLOGICALID, --病理诊断病理号
         QZDIAGTIME_T, --确诊时期_T期
         QZDIAGTIME_N, --确诊时期_N期
         QZDIAGTIME_M, --确诊时期_M期
         FIRSTDIADATE, --首次确诊时间
         REPORTINFUNIT, --报告单位
         REPORTDOCTOR, --报告医生
         REPORTDATE, --报告时间
         DEATHDATE, --死亡时间
         DEATHREASON, --死亡原因
         REJEST, --病历摘要
         REPORT_YDIAGNOSIS, --原诊断
         REPORT_YDIAGNOSISDATA, --原诊断日期
         REPORT_DIAGNOSISBASED, --诊断依据
         REPORT_NO, --传染病上报卡表编号
         REPORT_NOOFINPAT, --患者ID
         STATE, --"报告状态【 1、新增保存 2、提交 3、撤回 4、审核通过 5、审核未通过撤回 6、上报 7、作废】"
         CREATE_DATE, --创建时间
         CREATE_USERCODE, --创建人
         CREATE_USERNAME, --创建人
         CREATE_DEPTCODE, --创建人科室
         CREATE_DEPTNAME, --创建人科室
         MODIFY_DATE, --修改时间
         MODIFY_USERCODE, --修改人
         MODIFY_USERNAME, --修改人
         MODIFY_DEPTCODE, --修改人科室
         MODIFY_DEPTNAME, --修改人科室
         AUDIT_DATE, --审核时间
         AUDIT_USERCODE, --审核人
         AUDIT_USERNAME, --审核人
         AUDIT_DEPTCODE, --审核人科室
         AUDIT_DEPTNAME, --审核人科室
         VAILD, --状态是否有效  1、有效   0、无效
         DIAGICD10, --传染病病种(对应传染病诊断库)
         CANCELREASON, --否决原因
         CARDTYPE,
         clinicalstages,
         ReportDiagfunit)
      values
        (v_Report_ID_new, --序号
         v_REPORT_DISTRICTID, --传染病上报卡表区(县)编码
         v_REPORT_DISTRICTNAME, --传染病上报卡表区(县)名称
         v_REPORT_ICD10, --传染病报告卡ICD-10编码
         v_REPORT_ICD0, --传染病报告卡ICD-0编码
         v_REPORT_CLINICID, --门诊号
         v_REPORT_PATID, --住院号
         v_REPORT_INDO, --身份证号码
         v_REPORT_INPATNAME, --病患姓名
         v_SEXID, --病患性别
         v_REALAGE, --病患实足年龄(在诊断时未过生日者为虚年龄减二岁，已过生日者为虚年龄减一岁；未满一岁者为0岁)
         v_BIRTHDATE, --病患生日
         v_NATIONID, --病患民族编号
         v_NATIONNAME, --病患民族全称
         v_CONTACTTEL, --家庭电话
         v_MARTIAL, --婚姻状况
         v_OCCUPATION, --病患职业
         v_OFFICEADDRESS, --工作单位地址
         v_ORGPROVINCEID, --户口地址省份编码
         v_ORGCITYID, --户口地址所在市编码
         v_ORGDISTRICTID, --户口所在地区县编码
         v_ORGTOWNID, --户口所在地镇(街道)编码
         v_ORGVILLIAGE, --户口所在地居委会对应编码
         v_ORGPROVINCENAME, --户口所在地省份全称
         v_ORGCITYNAME, --户口所在地市全名称
         v_ORGDISTRICTNAME, --户口所在地区(县)全称
         v_ORGTOWN, --户口所在地镇全称
         v_ORGVILLAGENAME, --户口所在地村全称
         v_XZZPROVINCEID, --现住址所在省份编码
         v_XZZCITYID, --现住址所在市编码
         v_XZZDISTRICTID, --现住址所在区(县)代码
         v_XZZTOWNID, --现住址所在镇代码
         v_XZZVILLIAGEID, --现住址所在村编码
         v_XZZPROVINCE, --现住址所在省份全称
         v_XZZCITY, --现住址所在市全称
         v_XZZDISTRICT, --现住址所在区全称
         v_XZZTOWN, --现住址所在镇全称
         v_XZZVILLIAGE, --现住址所在村全称
         v_REPORT_DIAGNOSIS, --诊断
         v_PATHOLOGICALTYPE, --病理类型
         v_PATHOLOGICALID, --病理诊断病理号
         v_QZDIAGTIME_T, --确诊时期_T期
         v_QZDIAGTIME_N, --确诊时期_N期
         v_QZDIAGTIME_M, --确诊时期_M期
         v_FIRSTDIADATE, --首次确诊时间
         v_REPORTINFUNIT, --报告单位
         v_REPORTDOCTOR, --报告医生
         v_REPORTDATE, --报告时间
         v_DEATHDATE, --死亡时间
         v_DEATHREASON, --死亡原因
         v_REJEST, --病历摘要
         v_REPORT_YDIAGNOSIS, --原诊断
         v_REPORT_YDIAGNOSISDATA, --原诊断日期
         v_REPORT_DIAGNOSISBASED, --诊断依据
         v_REPORT_NO, --传染病上报卡表编号
         v_REPORT_NOOFINPAT, --患者ID
         v_STATE, --"报告状态【 1、新增保存 2、提交 3、撤回 4、审核通过 5、审核未通过撤回 6、上报 7、作废】"
         v_CREATE_DATE, --创建时间
         v_CREATE_USERCODE, --创建人
         v_CREATE_USERNAME, --创建人
         v_CREATE_DEPTCODE, --创建人科室
         v_CREATE_DEPTNAME, --创建人科室
         v_MODIFY_DATE, --修改时间
         v_MODIFY_USERCODE, --修改人
         v_MODIFY_USERNAME, --修改人
         v_MODIFY_DEPTCODE, --修改人科室
         v_MODIFY_DEPTNAME, --修改人科室
         v_AUDIT_DATE, --审核时间
         v_AUDIT_USERCODE, --审核人
         v_AUDIT_USERNAME, --审核人
         v_AUDIT_DEPTCODE, --审核人科室
         v_AUDIT_DEPTNAME, --审核人科室
         v_VAILD, --状态是否有效  1、有效   0、无效
         v_DIAGICD10, --传染病病种(对应传染病诊断库)
         v_CANCELREASON, --否决原因
         V_CARDTYPE,
         V_clinicalstages,
         V_ReportDiagfunit);
    
      open o_result for
        select v_Report_ID_new from dual;
    
    end if;
  
    --修改保存传染病报告卡信息
    IF v_edittype = '2' THEN
    
      update THERIOMAREPORTCARD
         set REPORT_DISTRICTID     = v_REPORT_DISTRICTID, --传染病上报卡表区(县)编码
             REPORT_DISTRICTNAME   = v_REPORT_DISTRICTNAME, --传染病上报卡表区(县)名称
             REPORT_ICD10          = v_REPORT_ICD10, --传染病报告卡ICD-10编码
             REPORT_ICD0           = v_REPORT_ICD0, --传染病报告卡ICD-0编码
             REPORT_CLINICID       = v_REPORT_CLINICID, --门诊号
             REPORT_PATID          = v_REPORT_PATID, --住院号
             REPORT_INDO           = v_REPORT_INDO, --身份证号码
             REPORT_INPATNAME      = v_REPORT_INPATNAME, --病患姓名
             SEXID                 = v_SEXID, --病患性别
             REALAGE               = v_REALAGE, --病患实足年龄(在诊断时未过生日者为虚年龄减二岁，已过生日者为虚年龄减一岁；未满一岁者为0岁)
             BIRTHDATE             = v_BIRTHDATE, --病患生日
             NATIONID              = v_NATIONID, --病患民族编号
             NATIONNAME            = v_NATIONNAME, --病患民族全称
             CONTACTTEL            = v_CONTACTTEL, --家庭电话
             MARTIAL               = v_MARTIAL, --婚姻状况
             OCCUPATION            = v_OCCUPATION, --病患职业
             OFFICEADDRESS         = v_OFFICEADDRESS, --工作单位地址
             ORGPROVINCEID         = v_ORGPROVINCEID, --户口地址省份编码
             ORGCITYID             = v_ORGCITYID, --户口地址所在市编码
             ORGDISTRICTID         = v_ORGDISTRICTID, --户口所在地区县编码
             ORGTOWNID             = v_ORGTOWNID, --户口所在地镇(街道)编码
             ORGVILLIAGE           = v_ORGVILLIAGE, --户口所在地居委会对应编码
             ORGPROVINCENAME       = v_ORGPROVINCENAME, --户口所在地省份全称
             ORGCITYNAME           = v_ORGCITYNAME, --户口所在地市全名称
             ORGDISTRICTNAME       = v_ORGDISTRICTNAME, --户口所在地区(县)全称
             ORGTOWN               = v_ORGTOWN, --户口所在地镇全称
             ORGVILLAGENAME        = v_ORGVILLAGENAME, --户口所在地村全称
             XZZPROVINCEID         = v_XZZPROVINCEID, --现住址所在省份编码
             XZZCITYID             = v_XZZCITYID, --现住址所在市编码
             XZZDISTRICTID         = v_XZZDISTRICTID, --现住址所在区(县)代码
             XZZTOWNID             = v_XZZTOWNID, --现住址所在镇代码
             XZZVILLIAGEID         = v_XZZVILLIAGEID, --现住址所在村编码
             XZZPROVINCE           = v_XZZPROVINCE, --现住址所在省份全称
             XZZCITY               = v_XZZCITY, --现住址所在市全称
             XZZDISTRICT           = v_XZZDISTRICT, --报现住址所在区全称
             XZZTOWN               = v_XZZTOWN, --现住址所在镇全称
             XZZVILLIAGE           = v_XZZVILLIAGE, --现住址所在村全称
             REPORT_DIAGNOSIS      = v_REPORT_DIAGNOSIS, --诊断
             PATHOLOGICALTYPE      = v_PATHOLOGICALTYPE, --病理类型
             PATHOLOGICALID        = v_PATHOLOGICALID, --病理诊断病理号
             QZDIAGTIME_T          = v_QZDIAGTIME_T, --确诊时期_T期
             QZDIAGTIME_N          = v_QZDIAGTIME_N, --确诊时期_N期
             QZDIAGTIME_M          = v_QZDIAGTIME_M, --确诊时期_M期
             FIRSTDIADATE          = v_FIRSTDIADATE, --首次确诊时间
             REPORTINFUNIT         = v_REPORTINFUNIT, --报告单位
             REPORTDOCTOR          = v_REPORTDOCTOR, --报告医生
             REPORTDATE            = v_REPORTDATE, --报告时间
             DEATHDATE             = v_DEATHDATE, --死亡时间
             DEATHREASON           = v_DEATHREASON, --死亡原因
             REJEST                = v_REJEST, --病历摘要
             REPORT_YDIAGNOSIS     = v_REPORT_YDIAGNOSIS, --原诊断
             REPORT_YDIAGNOSISDATA = v_REPORT_YDIAGNOSISDATA, --原诊断日期
             REPORT_DIAGNOSISBASED = v_REPORT_DIAGNOSISBASED, --诊断依据                   
             REPORT_NO             = v_REPORT_NO, --传染病上报卡表编号
             REPORT_NOOFINPAT      = v_REPORT_NOOFINPAT, --患者ID
             STATE                 = v_STATE, --"报告状态【 1、新增保存 2、提交 3、撤回 4、审核通过 5、审核未通过撤回 6、上报 7、作废】"                           
             CREATE_DATE           = nvl(v_CREATE_DATE, CREATE_DATE), --创建时间
             CREATE_USERCODE       = nvl(v_CREATE_USERCODE, CREATE_USERCODE), --创建人
             CREATE_USERNAME       = nvl(v_CREATE_USERNAME, CREATE_USERNAME), --创建人
             CREATE_DEPTCODE       = nvl(v_CREATE_DEPTCODE, CREATE_DEPTCODE), --创建人科室
             CREATE_DEPTNAME       = nvl(v_CREATE_DEPTNAME, CREATE_DEPTNAME), --创建人科室
             MODIFY_DATE           = nvl(v_MODIFY_DATE, MODIFY_DATE), --修改时间
             MODIFY_USERCODE       = nvl(v_MODIFY_USERCODE, MODIFY_USERCODE), --修改人
             MODIFY_USERNAME       = nvl(v_MODIFY_USERNAME, MODIFY_USERNAME), --修改人
             MODIFY_DEPTCODE       = nvl(v_MODIFY_DEPTCODE, MODIFY_DEPTCODE), --修改人科室
             MODIFY_DEPTNAME       = nvl(v_MODIFY_DEPTNAME, MODIFY_DEPTNAME), --修改人科室
             AUDIT_DATE            = nvl(v_AUDIT_DATE, AUDIT_DATE), --审核时间
             AUDIT_USERCODE        = nvl(v_AUDIT_USERCODE, AUDIT_USERCODE), --审核人
             AUDIT_USERNAME        = nvl(v_AUDIT_USERNAME, AUDIT_USERNAME), --审核人
             AUDIT_DEPTCODE        = nvl(v_AUDIT_DEPTCODE, AUDIT_DEPTCODE), --审核人科室
             AUDIT_DEPTNAME        = nvl(v_AUDIT_DEPTNAME, AUDIT_DEPTNAME), --审核人科室
             VAILD                 = v_VAILD,
             DIAGICD10             = v_DIAGICD10, --传染病病种(对应传染病诊断库)
             CANCELREASON          = v_CANCELREASON, --否决原因
             CARDTYPE              = V_CARDTYPE,
             clinicalstages        = V_clinicalstages,
             ReportDiagfunit       = V_ReportDiagfunit
       where REPORT_ID = v_Report_ID;
    
      open o_result for
        select v_Report_ID from dual;
    
    end if;
  
    --作废传染病报告卡信息
    IF v_edittype = '3' THEN
    
      update THERIOMAREPORTCARD
         set REPORT_DISTRICTID     = nvl(v_REPORT_DISTRICTID,
                                         REPORT_DISTRICTID), --传染病上报卡表区(县)编码
             REPORT_DISTRICTNAME   = nvl(v_REPORT_DISTRICTNAME,
                                         REPORT_DISTRICTNAME), --传染病上报卡表区(县)名称
             REPORT_ICD10          = nvl(v_REPORT_ICD10, REPORT_ICD10), --传染病报告卡ICD-10编码
             REPORT_ICD0           = nvl(v_REPORT_ICD0, REPORT_ICD0), --传染病报告卡ICD-0编码
             REPORT_CLINICID       = nvl(v_REPORT_CLINICID, REPORT_CLINICID), --门诊号
             REPORT_PATID          = v_REPORT_PATID, --住院号
             REPORT_INDO           = v_REPORT_INDO, --身份证号码
             REPORT_INPATNAME      = v_REPORT_INPATNAME, --病患姓名
             SEXID                 = nvl(v_SEXID, SEXID), --病患性别
             REALAGE               = v_REALAGE, --病患实足年龄(在诊断时未过生日者为虚年龄减二岁，已过生日者为虚年龄减一岁；未满一岁者为0岁)
             BIRTHDATE             = v_BIRTHDATE, --病患生日
             NATIONID              = v_NATIONID, --病患民族编号
             NATIONNAME            = v_NATIONNAME, --病患民族全称
             CONTACTTEL            = v_CONTACTTEL, --家庭电话
             MARTIAL               = v_MARTIAL, --婚姻状况
             OCCUPATION            = v_OCCUPATION, --病患职业
             OFFICEADDRESS         = v_OFFICEADDRESS, --工作单位地址
             ORGPROVINCEID         = v_ORGPROVINCEID, --户口地址省份编码
             ORGCITYID             = v_ORGCITYID, --户口地址所在市编码
             ORGDISTRICTID         = v_ORGDISTRICTID, --户口所在地区县编码
             ORGTOWNID             = v_ORGTOWNID, --户口所在地镇(街道)编码
             ORGVILLIAGE           = v_ORGVILLIAGE, --户口所在地居委会对应编码
             ORGPROVINCENAME       = v_ORGPROVINCENAME, --户口所在地省份全称
             ORGCITYNAME           = v_ORGCITYNAME, --户口所在地市全名称
             ORGDISTRICTNAME       = v_ORGDISTRICTNAME, --户口所在地区(县)全称
             ORGTOWN               = v_ORGTOWN, --户口所在地镇全称
             ORGVILLAGENAME        = v_ORGVILLAGENAME, --户口所在地村全称
             XZZPROVINCEID         = v_XZZPROVINCEID, --现住址所在省份编码
             XZZCITYID             = v_XZZCITYID, --现住址所在市编码
             XZZDISTRICTID         = v_XZZDISTRICTID, --现住址所在区(县)代码
             XZZTOWNID             = v_XZZTOWNID, --现住址所在镇代码
             XZZVILLIAGEID         = v_XZZVILLIAGEID, --现住址所在村编码
             XZZPROVINCE           = v_XZZPROVINCE, --现住址所在省份全称
             XZZCITY               = v_XZZCITY, --现住址所在市全称
             XZZDISTRICT           = v_XZZDISTRICT, --报现住址所在区全称
             XZZTOWN               = v_XZZTOWN, --现住址所在镇全称
             XZZVILLIAGE           = v_XZZVILLIAGE, --现住址所在村全称
             REPORT_DIAGNOSIS      = v_REPORT_DIAGNOSIS, --诊断
             PATHOLOGICALTYPE      = v_PATHOLOGICALTYPE, --病理类型
             PATHOLOGICALID        = v_PATHOLOGICALID, --病理诊断病理号
             QZDIAGTIME_T          = v_QZDIAGTIME_T, --确诊时期_T期
             QZDIAGTIME_N          = v_QZDIAGTIME_N, --确诊时期_N期
             QZDIAGTIME_M          = v_QZDIAGTIME_M, --确诊时期_M期
             FIRSTDIADATE          = v_FIRSTDIADATE, --首次确诊时间
             REPORTINFUNIT         = v_REPORTINFUNIT, --报告单位
             REPORTDOCTOR          = v_REPORTDOCTOR, --报告医生
             REPORTDATE            = v_REPORTDATE, --报告时间
             DEATHDATE             = v_DEATHDATE, --死亡时间
             DEATHREASON           = v_DEATHREASON, --死亡原因
             REJEST                = v_REJEST, --病历摘要
             REPORT_YDIAGNOSIS     = v_REPORT_YDIAGNOSIS, --原诊断
             REPORT_YDIAGNOSISDATA = v_REPORT_YDIAGNOSISDATA, --原诊断日期
             REPORT_DIAGNOSISBASED = v_REPORT_DIAGNOSISBASED, --诊断依据                   
             REPORT_NO             = v_REPORT_NO, --传染病上报卡表编号
             REPORT_NOOFINPAT      = v_REPORT_NOOFINPAT, --患者ID
             STATE                 = v_STATE, --"报告状态【 1、新增保存 2、提交 3、撤回 4、审核通过 5、审核未通过撤回 6、上报 7、作废】"                           
             CREATE_DATE           = nvl(v_CREATE_DATE, CREATE_DATE), --创建时间
             CREATE_USERCODE       = nvl(v_CREATE_USERCODE, CREATE_USERCODE), --创建人
             CREATE_USERNAME       = nvl(v_CREATE_USERNAME, CREATE_USERNAME), --创建人
             CREATE_DEPTCODE       = nvl(v_CREATE_DEPTCODE, CREATE_DEPTCODE), --创建人科室
             CREATE_DEPTNAME       = nvl(v_CREATE_DEPTNAME, CREATE_DEPTNAME), --创建人科室
             MODIFY_DATE           = nvl(v_MODIFY_DATE, MODIFY_DATE), --修改时间
             MODIFY_USERCODE       = nvl(v_MODIFY_USERCODE, MODIFY_USERCODE), --修改人
             MODIFY_USERNAME       = nvl(v_MODIFY_USERNAME, MODIFY_USERNAME), --修改人
             MODIFY_DEPTCODE       = nvl(v_MODIFY_DEPTCODE, MODIFY_DEPTCODE), --修改人科室
             MODIFY_DEPTNAME       = nvl(v_MODIFY_DEPTNAME, MODIFY_DEPTNAME), --修改人科室
             AUDIT_DATE            = nvl(v_AUDIT_DATE, AUDIT_DATE), --审核时间
             AUDIT_USERCODE        = nvl(v_AUDIT_USERCODE, AUDIT_USERCODE), --审核人
             AUDIT_USERNAME        = nvl(v_AUDIT_USERNAME, AUDIT_USERNAME), --审核人
             AUDIT_DEPTCODE        = nvl(v_AUDIT_DEPTCODE, AUDIT_DEPTCODE), --审核人科室
             AUDIT_DEPTNAME        = nvl(v_AUDIT_DEPTNAME, AUDIT_DEPTNAME), --审核人科室
             VAILD                 = '0',
             DIAGICD10             = v_DIAGICD10, -- --传染病病种(对应传染病诊断库)
             CANCELREASON          = v_CANCELREASON, --否决原因
             CARDTYPE              = V_CARDTYPE,
             clinicalstages        = V_clinicalstages,
             ReportDiagfunit       = V_ReportDiagfunit
       where REPORT_ID = v_Report_ID;
    
      open o_result for
        select v_Report_ID from dual;
    
    end if;
  
    --根据传入的传染病报告卡ID查询报告卡信息
    IF v_edittype = '4' THEN
    
      open o_result for
        select * from THERIOMAREPORTCARD a where a.report_id = v_Report_ID;
    
    end if;
  
  end;

  /*********************************************************************************/
  --报表部分---肿瘤登记月报表 add by ywk 2013年7月31日 14:59:19
  PROCEDURE usp_GetTheriomaReportBYMonth( --v_searchtype    varchar default '', --增加此字段主要后期为i区分中心医院及其他的查询
                                         v_year          varchar default '', --年
                                         v_month         varchar default '', --月
                                         v_deptcode      varchar default '', --科室编码
                                         v_diagstartdate varchar default '', --诊断开始时间
                                         v_diagenddate   varchar default '', --诊断结束时间
                                         o_result        OUT empcurtyp) as
  begin
    --if v_searchtype = '1' then
    open o_result for
      select *
        from (select REPORT_INPATNAME as PATNAME,
                     sexinfo.name as SEXNAME,
                     REALAGE,
                     STATE,
                     reportdate,
                     material.name as MATRIAL,
                     jobinfo.name as JOB,
                     FIRSTDIADATE,
                     DEATHDATE,
                     DEATHREASON,
                     VAILD,
                     XZZPROVINCE || XZZCITY || XZZDISTRICT || XZZTOWN ||
                     XZZVILLIAGE as JZADDRESS,
                     REPORT_DIAGNOSIS,
                     '九江中医院' as HOSP,
                     (case
                       when instr(REPORT_DIAGNOSISBASED, '1', 1) <> 0 then
                        '临床'
                       when instr(REPORT_DIAGNOSISBASED, '2', 1) <> 0 then
                        'X线、CT、超声、内窥镜'
                       when instr(REPORT_DIAGNOSISBASED, '3', 1) <> 0 then
                        '手术、尸检'
                       when instr(REPORT_DIAGNOSISBASED, '4', 1) <> 0 then
                        '生化、免疫'
                       when instr(REPORT_DIAGNOSISBASED, '5', 1) <> 0 then
                        '细胞学、血片'
                       when instr(REPORT_DIAGNOSISBASED, '6', 1) <> 0 then
                        '病理（继发）'
                       when instr(REPORT_DIAGNOSISBASED, '7', 1) <> 0 then
                        '病理（原发）'
                       when instr(REPORT_DIAGNOSISBASED, '8', 1) <> 0 then
                        '尸检（有病理）'
                       when instr(REPORT_DIAGNOSISBASED, '9', 1) <> 0 then
                        '不详'
                       when instr(REPORT_DIAGNOSISBASED, '0', 1) <> 0 then
                        '死亡补发病'
                       else
                        ''
                     end) as DIAGYJ,
                     PATHOLOGICALTYPE,
                     QZDIAGTIME_T || QZDIAGTIME_N || QZDIAGTIME_M as QZDIAGTIME,
                     '' as MEMO
              
                from theriomareportcard tmpcard
                left join dictionary_detail material
                  on material.detailid = tmpcard.MARTIAL
                 and material.categoryid = '4'
                 AND material.valid = 1
                left join dictionary_detail sexinfo
                  on sexinfo.detailid = tmpcard.sexid
                 and sexinfo.categoryid = '3'
                 and sexinfo.valid = 1
                left join dictionary_detail jobinfo
                  on jobinfo.detailid = tmpcard.occupation
                 and jobinfo.categoryid = '41'
                 and jobinfo.valid = 1) A
       where A.reportdate like v_year || '-' || v_month || '%'
         and A.VAILD = '1'
         and A.STATE not in ('3', '5', '7');
    -- and A.FIRSTDIADATE >v_diagstartdate and A.FIRSTDIADATE<v_diagenddate ;   --'2013-0%';
  end;
  --报表部分---恶性肿瘤新发病例登记簿 add by ywk 2013年8月2日 11:29:02
  PROCEDURE usp_GetTheriomaReportBYNew( --v_searchtype    varchar default '', --增加此字段主要后期为i区分中心医院及其他的查询
                                       v_year   varchar default '', --年
                                       v_month  varchar default '', --月
                                       o_result OUT empcurtyp) as
  begin
    open o_result for
      select *
        from (select REPORT_INPATNAME as PATNAME,
                     sexinfo.name as SEXNAME,
                     REALAGE,
                     REPORT_NO,
                     '' HIGHDIAG,
                     '' SFTIME,
                     '' SFINFO,
                     OFFICEADDRESS,
                     reportdate,
                     material.name as MATRIAL,
                     jobinfo.name as JOB,
                     FIRSTDIADATE,
                     DEATHDATE,
                     DEATHREASON,
                     VAILD,
                     XZZPROVINCE || XZZCITY || XZZDISTRICT || XZZTOWN ||
                     XZZVILLIAGE as JZADDRESS,
                     REPORT_DIAGNOSIS,
                     '九江中医院' as HOSP,
                     (case
                       when instr(REPORT_DIAGNOSISBASED, '1', 1) <> 0 then
                        '临床'
                       when instr(REPORT_DIAGNOSISBASED, '2', 1) <> 0 then
                        'X线、CT、超声、内窥镜'
                       when instr(REPORT_DIAGNOSISBASED, '3', 1) <> 0 then
                        '手术、尸检'
                       when instr(REPORT_DIAGNOSISBASED, '4', 1) <> 0 then
                        '生化、免疫'
                       when instr(REPORT_DIAGNOSISBASED, '5', 1) <> 0 then
                        '细胞学、血片'
                       when instr(REPORT_DIAGNOSISBASED, '6', 1) <> 0 then
                        '病理（继发）'
                       when instr(REPORT_DIAGNOSISBASED, '7', 1) <> 0 then
                        '病理（原发）'
                       when instr(REPORT_DIAGNOSISBASED, '8', 1) <> 0 then
                        '尸检（有病理）'
                       when instr(REPORT_DIAGNOSISBASED, '9', 1) <> 0 then
                        '不详'
                       when instr(REPORT_DIAGNOSISBASED, '0', 1) <> 0 then
                        '死亡补发病'
                       else
                        ''
                     end) as DIAGYJ,
                     STATE,
                     PATHOLOGICALTYPE,
                     QZDIAGTIME_T || QZDIAGTIME_N || QZDIAGTIME_M as QZDIAGTIME,
                     '' as MEMO
              
                from theriomareportcard tmpcard
                left join dictionary_detail material
                  on material.detailid = tmpcard.MARTIAL
                 and material.categoryid = '4'
                 AND material.valid = 1
                left join dictionary_detail sexinfo
                  on sexinfo.detailid = tmpcard.sexid
                 and sexinfo.categoryid = '3'
                 and sexinfo.valid = 1
                left join dictionary_detail jobinfo
                  on jobinfo.detailid = tmpcard.occupation
                 and jobinfo.categoryid = '41'
                 and jobinfo.valid = 1) A
       where A.reportdate like v_year || '-' || v_month || '%'
         and A.DEATHDATE is null
         and A.VAILD = '1'
         and A.STATE not in ('3', '5', '7');
  end;

  --报表部分---恶性肿瘤死亡病例登记簿 add by ywk 2013年8月2日 11:29:02
  PROCEDURE usp_GetTheriomaReportBYDead( --v_searchtype    varchar default '', --增加此字段主要后期为i区分中心医院及其他的查询
                                        v_year   varchar default '', --年
                                        v_month  varchar default '', --月
                                        o_result OUT empcurtyp) as
  begin
    open o_result for
      select *
        from (select REPORT_INPATNAME as PATNAME,
                     sexinfo.name as SEXNAME,
                     REALAGE,
                     REPORT_NO,
                     OFFICEADDRESS,
                     reportdate,
                     material.name as MATRIAL,
                     jobinfo.name as JOB,
                     FIRSTDIADATE,
                     DEATHDATE,
                     DEATHREASON,
                     BIRTHDATE,
                     VAILD,
                     '' DIEADDRESS,
                     '' CULTURAL,
                     XZZPROVINCE || XZZCITY || XZZDISTRICT || XZZTOWN ||
                     XZZVILLIAGE as JZADDRESS,
                     REPORT_DIAGNOSIS,
                     '九江中医院' as HOSP,
                     (case
                       when instr(REPORT_DIAGNOSISBASED, '1', 1) <> 0 then
                        '临床'
                       when instr(REPORT_DIAGNOSISBASED, '2', 1) <> 0 then
                        'X线、CT、超声、内窥镜'
                       when instr(REPORT_DIAGNOSISBASED, '3', 1) <> 0 then
                        '手术、尸检'
                       when instr(REPORT_DIAGNOSISBASED, '4', 1) <> 0 then
                        '生化、免疫'
                       when instr(REPORT_DIAGNOSISBASED, '5', 1) <> 0 then
                        '细胞学、血片'
                       when instr(REPORT_DIAGNOSISBASED, '6', 1) <> 0 then
                        '病理（继发）'
                       when instr(REPORT_DIAGNOSISBASED, '7', 1) <> 0 then
                        '病理（原发）'
                       when instr(REPORT_DIAGNOSISBASED, '8', 1) <> 0 then
                        '尸检（有病理）'
                       when instr(REPORT_DIAGNOSISBASED, '9', 1) <> 0 then
                        '不详'
                       when instr(REPORT_DIAGNOSISBASED, '0', 1) <> 0 then
                        '死亡补发病'
                       else
                        ''
                     end) as DIAGYJ,
                     PATHOLOGICALTYPE,
                     QZDIAGTIME_T || QZDIAGTIME_N || QZDIAGTIME_M as QZDIAGTIME,
                     '' as MEMO,
                     STATE
              
                from theriomareportcard tmpcard
                left join dictionary_detail material
                  on material.detailid = tmpcard.MARTIAL
                 and material.categoryid = '4'
                 AND material.valid = 1
                left join dictionary_detail sexinfo
                  on sexinfo.detailid = tmpcard.sexid
                 and sexinfo.categoryid = '3'
                 and sexinfo.valid = 1
                left join dictionary_detail jobinfo
                  on jobinfo.detailid = tmpcard.occupation
                 and jobinfo.categoryid = '41'
                 and jobinfo.valid = 1) A
       where A.reportdate like v_year || '-' || v_month || '%'
         and A.DEATHDATE is not null
         and A.VAILD = '1'
         and A.STATE not in ('3', '5', '7');
  end;

  --报表部分---肿瘤登记月报表 add by ywk 2013年8月5日 11:33:25中心医院
  PROCEDURE usp_GetTheriomaReportBYMonthZX( --v_searchtype    varchar default '', --增加此字段主要后期为i区分中心医院及其他的查询
                                           v_year          varchar default '', --年
                                           v_month         varchar default '', --月
                                           v_deptcode      varchar default '', --科室编码
                                           v_diagstartdate varchar default '', --诊断开始时间
                                           v_diagenddate   varchar default '', --诊断结束时间
                                           o_result        OUT empcurtyp) as
  begin
    --if v_searchtype = '1' then
    open o_result for
      select *
        from (select REPORT_INPATNAME as PATNAME,
                     sexinfo.name as SEXNAME,
                     REALAGE,
                     STATE,
                     reportdate,
                     material.name as MATRIAL,
                     jobinfo.name as JOB,
                     FIRSTDIADATE,
                     DEATHDATE,
                     DEATHREASON,
                     VAILD,
                     REPORT_NO,
                     REPORT_PATID,
                     BIRTHDATE,
                     CONTACTTEL,
                     users.name as REPORTDOCTORNAME,
                     REPORTDOCTOR,
                     REPORT_INDO,
                     CREATE_DEPTNAME as BGDEPT, --报告科室
                     REPORTDIAGFUNIT, --诊断单位
                     d.name as DIAGUNIT, --诊断单位名称
                     XZZPROVINCE || XZZCITY || XZZDISTRICT || XZZTOWN ||
                     XZZVILLIAGE as JZADDRESS,
                     REPORT_DIAGNOSIS,
                     '九江中医院' as HOSP,
                     (case
                       when instr(REPORT_DIAGNOSISBASED, '1', 1) <> 0 then
                        '临床'
                       when instr(REPORT_DIAGNOSISBASED, '2', 1) <> 0 then
                        'X线、CT、超声、内窥镜'
                       when instr(REPORT_DIAGNOSISBASED, '3', 1) <> 0 then
                        '手术、尸检'
                       when instr(REPORT_DIAGNOSISBASED, '4', 1) <> 0 then
                        '生化、免疫'
                       when instr(REPORT_DIAGNOSISBASED, '5', 1) <> 0 then
                        '细胞学、血片'
                       when instr(REPORT_DIAGNOSISBASED, '6', 1) <> 0 then
                        '病理（继发）'
                       when instr(REPORT_DIAGNOSISBASED, '7', 1) <> 0 then
                        '病理（原发）'
                       when instr(REPORT_DIAGNOSISBASED, '8', 1) <> 0 then
                        '尸检（有病理）'
                       when instr(REPORT_DIAGNOSISBASED, '9', 1) <> 0 then
                        '不详'
                       when instr(REPORT_DIAGNOSISBASED, '0', 1) <> 0 then
                        '死亡补发病'
                       else
                        ''
                     end) as DIAGYJ,
                     PATHOLOGICALTYPE,
                     QZDIAGTIME_T || QZDIAGTIME_N || QZDIAGTIME_M as QZDIAGTIME,
                     '' as MEMO
              
                from theriomareportcard tmpcard
                left join dictionary_detail material
                  on material.detailid = tmpcard.MARTIAL
                 and material.categoryid = '4'
                 AND material.valid = 1
                left join dictionary_detail sexinfo
                  on sexinfo.detailid = tmpcard.sexid
                 and sexinfo.categoryid = '3'
                 and sexinfo.valid = 1
                left join dictionary_detail jobinfo
                  on jobinfo.detailid = tmpcard.occupation
                 and jobinfo.categoryid = '41'
                 and jobinfo.valid = 1
                left join users
                  on tmpcard.reportdoctor = users.id
                left join department d
                  on tmpcard.ReportDiagfunit = d.id) A
       where A.reportdate like v_year || '-' || v_month || '%'
         and A.VAILD = '1'
         and A.STATE not in ('3', '5', '7');
    -- and A.FIRSTDIADATE >v_diagstartdate and A.FIRSTDIADATE<v_diagenddate ;   --'2013-0%';
  end;

------------------脑卒中，冠心病报表-------------------add  by  jxh  2013-9-7  15:30
 PROCEDURE usp_CardiovascularReport( --v_searchtype    varchar default '', --增加此字段主要后期为i区分中心医院及其他的查询
                                           v_year          varchar default '', --年
                                           v_month         varchar default '', --月
                                           v_deptcode      varchar default '', --科室编码
                                           v_diagstartdate varchar default '', --诊断开始时间
                                           v_diagenddate   varchar default '', --诊断结束时间
                                           o_result        OUT empcurtyp) as
  begin
    --if v_searchtype = '1' then
    open o_result for
      select *
        from (select   REPORT_NO,--编号
                       PATID,--住院号
                       NAME,--姓名
                       sexinfo.name as SEXNAME,--性别
                       AGE,--年龄
                       STATE,--状态
                       jobinfo.name as JOB,--职业
                       XZZPROVICE || XZZCITY || XZZSTREET || XZZCOMMITTEES ||
                       XZZPARM as JZADDRESS,--居住地址
                       OFFICEPLACE,--单位地址
                       z.name as DIAGNAME,--诊断    
                       DIAGNOSEDATE,--确诊日期  
                       DIAGHOSPITAL,--诊断单位  
                       CASE diovcard.isfirstsick
                            WHEN '1' THEN
                             '是'
                             ELSE
                             '否'
                       END ISFIRSTSICK,
                       --ISFIRSTSICK,--是否首次发病   
                       REPORTDATE,--报卡日期
                       REPORTUSERNAME,--报卡医师
                       CREATE_DEPTNAME,--报告科室
                       DIEDATE,--死亡日期
                       VAILD --是否有效                   
                from cardiovascularcard diovcard    
                left join zymosis_diagnosis z on diovcard.Icd = z.icd           
                left join dictionary_detail sexinfo
                  on sexinfo.detailid = diovcard.sexid
                 and sexinfo.categoryid = '3'
                 and sexinfo.valid = 1
                left join dictionary_detail jobinfo
                  on jobinfo.detailid = diovcard.jobid
                 and jobinfo.categoryid = '41'
                 and jobinfo.valid = 1
                left join users
                  on diovcard.REPORTUSERCODE = users.id) A
               
       where A.reportdate like v_year || '-' || v_month || '%'
         and A.VAILD = '1'
         and A.STATE not in ('3', '5', '7');
    -- and A.FIRSTDIADATE >v_diagstartdate and A.FIRSTDIADATE<v_diagenddate ;   --'2013-0%';
  end;

  --保存或修改艾滋病报表
  PROCEDURE usp_AddOrModHIVReport(v_HIVID               varchar2,
                                  v_REPORTID            integer,
                                  v_REPORTNO            varchar2,
                                  v_MARITALSTATUS       varchar2,
                                  v_NATION              varchar2,
                                  v_CULTURESTATE        varchar2,
                                  v_HOUSEHOLDSCOPE      varchar2,
                                  v_HOUSEHOLDADDRESS    varchar2,
                                  v_ADDRESS             varchar2,
                                  v_CONTACTHISTORY      varchar2,
                                  v_VENERISMHISTORY     varchar2,
                                  v_INFACTWAY           varchar2,
                                  v_SAMPLESOURCE        varchar2,
                                  v_DETECTIONCONCLUSION varchar2,
                                  v_AFFIRMDATE          varchar2,
                                  v_AFFIRMLOCAL         varchar2,
                                  v_DIAGNOSEDATE        varchar2,
                                  v_DOCTOR              varchar2,
                                  v_WRITEDATE           varchar2,
                                  v_ALIKESYMBOL         varchar2,
                                  v_REMARK              varchar2,
                                  v_VAILD               varchar2,
                                  v_CREATOR             varchar2,
                                  v_CREATEDATE          varchar2,
                                  v_MENDER              varchar2,
                                  v_ALTERDATE           varchar2) as
    v_count integer;
  begin
    select count(*)
      into v_count
      from zymosis_hiv z
     where z.hiv_id = v_HIVID;
    if v_count <= 0 then
      insert into zymosis_hiv
        (HIV_ID,
         REPORT_ID,
         REPORT_NO,
         MARITALSTATUS,
         NATION,
         CULTURESTATE,
         HOUSEHOLDSCOPE,
         HOUSEHOLDADDRESS,
         ADDRESS,
         CONTACTHISTORY,
         VENERISMHISTORY,
         INFACTWAY,
         SAMPLESOURCE,
         DETECTIONCONCLUSION,
         AFFIRMDATE,
         AFFIRMLOCAL,
         DIAGNOSEDATE,
         DOCTOR,
         WRITEDATE,
         ALIKESYMBOL,
         REMARK,
         VAILD,
         CREATOR,
         CREATEDATE,
         MENDER,
         ALTERDATE)
      values
        (v_HIVID,
         v_REPORTID,
         v_REPORTNO,
         v_MARITALSTATUS,
         v_NATION,
         v_CULTURESTATE,
         v_HOUSEHOLDSCOPE,
         v_HOUSEHOLDADDRESS,
         v_ADDRESS,
         v_CONTACTHISTORY,
         v_VENERISMHISTORY,
         v_INFACTWAY,
         v_SAMPLESOURCE,
         v_DETECTIONCONCLUSION,
         v_AFFIRMDATE,
         v_AFFIRMLOCAL,
         v_DIAGNOSEDATE,
         v_DOCTOR,
         v_WRITEDATE,
         v_ALIKESYMBOL,
         v_REMARK,
         '1',
         v_CREATOR,
         v_CREATEDATE,
         v_MENDER,
         v_ALTERDATE);
    else
      update zymosis_hiv
         set REPORT_ID           = v_REPORTID,
             REPORT_NO           = v_REPORTNO,
             MARITALSTATUS       = v_MARITALSTATUS,
             NATION              = v_NATION,
             CULTURESTATE        = v_CULTURESTATE,
             HOUSEHOLDSCOPE      = v_HOUSEHOLDSCOPE,
             HOUSEHOLDADDRESS    = v_HOUSEHOLDADDRESS,
             ADDRESS             = v_ADDRESS,
             CONTACTHISTORY      = v_CONTACTHISTORY,
             VENERISMHISTORY     = v_VENERISMHISTORY,
             INFACTWAY           = v_INFACTWAY,
             SAMPLESOURCE        = v_SAMPLESOURCE,
             DETECTIONCONCLUSION = v_DETECTIONCONCLUSION,
             AFFIRMDATE          = v_AFFIRMDATE,
             AFFIRMLOCAL         = v_AFFIRMLOCAL,
             DIAGNOSEDATE        = v_DIAGNOSEDATE,
             DOCTOR              = v_DOCTOR,
             WRITEDATE           = v_WRITEDATE,
             ALIKESYMBOL         = v_ALIKESYMBOL,
             REMARK              = v_REMARK,
             MENDER              = v_MENDER,
             ALTERDATE           = v_ALTERDATE
       where HIV_ID = v_HIVID;
    
    end if;
  end;

  --保存或修改沙眼衣原体感染
  PROCEDURE usp_AddOrModSYYYTReport(v_SZDYYTID            varchar,
                                    v_REPORTID            integer,
                                    v_REPORTNO            varchar,
                                    v_MARITALSTATUS       varchar,
                                    v_NATION              varchar,
                                    v_CULTURESTATE        varchar,
                                    v_HOUSEHOLDSCOPE      varchar,
                                    v_HOUSEHOLDADDRESS    varchar,
                                    v_ADDRESS             varchar,
                                    v_SYYYTGR             varchar,
                                    v_CONTACTHISTORY      varchar,
                                    v_VENERISMHISTORY     varchar,
                                    v_INFACTWAY           varchar,
                                    v_SAMPLESOURCE        varchar,
                                    v_DETECTIONCONCLUSION varchar,
                                    v_AFFIRMDATE          varchar,
                                    v_AFFIRMLOCAL         varchar,
                                    v_VAILD               varchar,
                                    v_CREATOR             varchar,
                                    v_CREATEDATE          varchar,
                                    v_MENDER              varchar,
                                    v_ALTERDATE           varchar) as
    v_count integer;
  begin
    select count(*)
      into v_count
      from zymosis_szdyyt z
     where z.SZDYYT_ID = v_SZDYYTID;
    if v_count <= 0 then
      insert into zymosis_szdyyt
        (SZDYYT_ID,
         REPORT_ID,
         REPORT_NO,
         MARITALSTATUS,
         NATION,
         CULTURESTATE,
         HOUSEHOLDSCOPE,
         HOUSEHOLDADDRESS,
         ADDRESS,
         SYYYTGR,
         CONTACTHISTORY,
         VENERISMHISTORY,
         INFACTWAY,
         SAMPLESOURCE,
         DETECTIONCONCLUSION,
         AFFIRMDATE,
         AFFIRMLOCAL,
         VAILD,
         CREATOR,
         CREATEDATE,
         MENDER,
         ALTERDATE)
      values
        (v_szdyytid,
         v_reportid,
         v_reportno,
         v_maritalstatus,
         v_nation,
         v_culturestate,
         v_householdscope,
         v_householdaddress,
         v_address,
         v_syyytgr,
         v_contacthistory,
         v_venerismhistory,
         v_infactway,
         v_samplesource,
         v_detectionconclusion,
         v_affirmdate,
         v_affirmlocal,
         '1',
         v_creator,
         v_createdate,
         v_mender,
         v_alterdate);
    else
      update zymosis_szdyyt
         set report_id           = v_reportid,
             report_no           = v_reportno,
             maritalstatus       = v_maritalstatus,
             nation              = v_nation,
             culturestate        = v_culturestate,
             householdscope      = v_householdscope,
             householdaddress    = v_householdaddress,
             address             = v_address,
             syyytgr             = v_syyytgr,
             contacthistory      = v_contacthistory,
             venerismhistory     = v_venerismhistory,
             infactway           = v_infactway,
             samplesource        = v_samplesource,
             detectionconclusion = v_detectionconclusion,
             affirmdate          = v_affirmdate,
             affirmlocal         = v_affirmlocal,
             vaild               = v_vaild,
             creator             = v_creator,
             createdate          = v_createdate,
             mender              = v_mender,
             alterdate           = v_alterdate
       where szdyyt_id = v_szdyytid;
    end if;
  end;

  --保存或修改乙肝报表
  PROCEDURE usp_AddOrModHBVReport(v_HBVID varchar2,
                                  
                                  v_REPORTID      integer,
                                  v_HBSAGDATE     varchar2,
                                  v_FRISTDATE     varchar2,
                                  v_ALT           varchar2,
                                  v_ANTIHBC       varchar2,
                                  v_LIVERBIOPSY   varchar2,
                                  v_RECOVERYHBSAG varchar2,
                                  
                                  v_VAILD      varchar2,
                                  v_CREATOR    varchar2,
                                  v_CREATEDATE varchar2,
                                  v_MENDER     varchar2,
                                  v_ALTERDATE  varchar2) as
    v_count integer;
  begin
    select count(*)
      into v_count
      from ZYMOSIS_HBV z
     where z.HBVID = v_HBVID
       and z.vaild = '1';
    if v_count <= 0 then
      insert into ZYMOSIS_HBV
        (HBVID,
         REPORTID,
         HBSAGDATE,
         FRISTDATE,
         ALT,
         ANTIHBC,
         LIVERBIOPSY,
         RECOVERYHBSAG,
         VAILD,
         CREATOR,
         CREATEDATE,
         MENDER,
         ALTERDATE)
      values
        (v_HBVID,
         v_REPORTID,
         v_HBSAGDATE,
         v_FRISTDATE,
         v_ALT,
         v_ANTIHBC,
         v_LIVERBIOPSY,
         v_RECOVERYHBSAG,
         '1',
         v_CREATOR,
         v_CREATEDATE,
         v_MENDER,
         v_ALTERDATE);
    else
      update ZYMOSIS_HBV
         set REPORTID      = v_REPORTID,
             HBSAGDATE     = v_HBSAGDATE,
             FRISTDATE     = v_FRISTDATE,
             ALT           = v_ALT,
             ANTIHBC       = v_ANTIHBC,
             LIVERBIOPSY   = v_LIVERBIOPSY,
             RECOVERYHBSAG = v_RECOVERYHBSAG,
             MENDER        = v_MENDER,
             ALTERDATE     = v_ALTERDATE
       where HBVID = v_HBVID;
    
    end if;
  end;

  --保存或修改尖锐湿疣项目
  PROCEDURE usp_AddOrModJRSYReport(v_JRSY_ID             varchar,
                                   v_REPORTID            integer,
                                   v_REPORTNO            varchar,
                                   v_MARITALSTATUS       varchar,
                                   v_NATION              varchar,
                                   v_CULTURESTATE        varchar,
                                   v_HOUSEHOLDSCOPE      varchar,
                                   v_HOUSEHOLDADDRESS    varchar,
                                   v_ADDRESS             varchar,
                                   v_CONTACTHISTORY      varchar,
                                   v_VENERISMHISTORY     varchar,
                                   v_INFACTWAY           varchar,
                                   v_SAMPLESOURCE        varchar,
                                   v_DETECTIONCONCLUSION varchar,
                                   v_AFFIRMDATE          varchar,
                                   v_AFFIRMLOCAL         varchar,
                                   v_VAILD               varchar,
                                   v_CREATOR             varchar,
                                   v_CREATEDATE          varchar,
                                   v_MENDER              varchar,
                                   v_ALTERDATE           varchar) as
    v_count integer;
  begin
    select count(*)
      into v_count
      from ZYMOSIS_JRSY z
     where z.JRSY_ID = v_JRSY_ID;
    if v_count <= 0 then
      insert into ZYMOSIS_JRSY
        (JRSY_ID,
         REPORT_ID,
         REPORT_NO,
         MARITALSTATUS,
         NATION,
         CULTURESTATE,
         HOUSEHOLDSCOPE,
         HOUSEHOLDADDRESS,
         ADDRESS,
         CONTACTHISTORY,
         VENERISMHISTORY,
         INFACTWAY,
         SAMPLESOURCE,
         DETECTIONCONCLUSION,
         AFFIRMDATE,
         AFFIRMLOCAL,
         VAILD,
         CREATOR,
         CREATEDATE,
         MENDER,
         ALTERDATE)
      values
        (v_JRSY_ID,
         v_reportid,
         v_reportno,
         v_maritalstatus,
         v_nation,
         v_culturestate,
         v_householdscope,
         v_householdaddress,
         v_address,
         v_contacthistory,
         v_venerismhistory,
         v_infactway,
         v_samplesource,
         v_detectionconclusion,
         v_affirmdate,
         v_affirmlocal,
         '1',
         v_creator,
         v_createdate,
         v_mender,
         v_alterdate);
    else
      update ZYMOSIS_JRSY
         set report_id           = v_reportid,
             report_no           = v_reportno,
             maritalstatus       = v_maritalstatus,
             nation              = v_nation,
             culturestate        = v_culturestate,
             householdscope      = v_householdscope,
             householdaddress    = v_householdaddress,
             address             = v_address,
             contacthistory      = v_contacthistory,
             venerismhistory     = v_venerismhistory,
             infactway           = v_infactway,
             samplesource        = v_samplesource,
             detectionconclusion = v_detectionconclusion,
             affirmdate          = v_affirmdate,
             affirmlocal         = v_affirmlocal,
             vaild               = v_vaild,
             creator             = v_creator,
             createdate          = v_createdate,
             mender              = v_mender,
             alterdate           = v_alterdate
       where JRSY_ID = v_JRSY_ID;
    end if;
  end;

  --保存或修改甲型H1N1流感报表
  PROCEDURE usp_AddOrModH1N1Report(v_H1N1ID         varchar2,
                                   v_REPORTID       integer,
                                   v_CASETYPE       varchar2,
                                   v_HOSPITALSTATUS varchar2,
                                   v_ISCURE         varchar2,
                                   v_ISOVERSEAS     varchar2,
                                   v_VAILD          varchar2,
                                   v_CREATOR        varchar2,
                                   v_CREATEDATE     varchar2,
                                   v_MENDER         varchar2,
                                   v_ALTERDATE      varchar2) as
    v_count integer;
  begin
    select count(*)
      into v_count
      from ZYMOSIS_H1N1 z
     where z.H1N1ID = v_H1N1ID
       and z.vaild = '1';
    if v_count <= 0 then
      insert into ZYMOSIS_H1N1
        (H1N1ID,
         REPORTID,
         CASETYPE,
         HOSPITALSTATUS,
         ISCURE,
         ISOVERSEAS,
         VAILD,
         CREATOR,
         CREATEDATE,
         MENDER,
         ALTERDATE)
      values
        (v_H1N1ID,
         v_REPORTID,
         v_CASETYPE,
         v_HOSPITALSTATUS,
         v_ISCURE,
         v_ISOVERSEAS,
         '1',
         v_CREATOR,
         v_CREATEDATE,
         v_MENDER,
         v_ALTERDATE);
    else
      update ZYMOSIS_H1N1
         set REPORTID       = v_REPORTID,
             CASETYPE       = v_CASETYPE,
             HOSPITALSTATUS = v_HOSPITALSTATUS,
             ISCURE         = v_ISCURE,
             ISOVERSEAS     = v_ISOVERSEAS,
             MENDER         = v_MENDER,
             ALTERDATE      = v_ALTERDATE
       where H1N1ID = v_H1N1ID;
    end if;
  end;

  --保存或修改手足口病报告表
  PROCEDURE usp_AddOrModHFMDReport(v_HFMDID     varchar2,
                                   v_REPORTID   integer,
                                   v_LABRESULT  varchar2,
                                   v_ISSEVERE   varchar2,
                                   v_VAILD      varchar2,
                                   v_CREATOR    varchar2,
                                   v_CREATEDATE varchar2,
                                   v_MENDER     varchar2,
                                   v_ALTERDATE  varchar2) as
    v_count integer;
  begin
    select count(*)
      into v_count
      from ZYMOSIS_HFMD z
     where z.HFMDID = v_HFMDID;
    if v_count <= 0 then
      insert into ZYMOSIS_HFMD
        (HFMDID,
         REPORTID,
         LABRESULT,
         ISSEVERE,
         VAILD,
         CREATOR,
         CREATEDATE,
         MENDER,
         ALTERDATE)
      values
        (v_HFMDID,
         v_REPORTID,
         v_LABRESULT,
         v_ISSEVERE,
         '1',
         v_CREATOR,
         v_CREATEDATE,
         v_MENDER,
         v_ALTERDATE);
    else
      update ZYMOSIS_HFMD
         set REPORTID  = v_REPORTID,
             LABRESULT = v_LABRESULT,
             ISSEVERE  = v_ISSEVERE,
             MENDER    = v_MENDER,
             ALTERDATE = v_ALTERDATE
       where HFMDID = v_HFMDID;
    end if;
  end;

  --保存或修改AFP报告表
  PROCEDURE usp_AddOrModAFPReport(v_AFPID            varchar2,
                                  v_REPORTID         integer,
                                  v_HOUSEHOLDSCOPE   varchar2,
                                  v_HOUSEHOLDADDRESS varchar2,
                                  v_ADDRESS          varchar2,
                                  v_PALSYDATE        varchar2,
                                  v_PALSYSYMPTOM     varchar2,
                                  v_VAILD            varchar2,
                                  v_CREATOR          varchar2,
                                  v_CREATEDATE       varchar2,
                                  v_MENDER           varchar2,
                                  v_ALTERDATE        varchar2,
                                  v_DIAGNOSISDATE    varchar2) as
    v_count integer;
  begin
    select count(*)
      into v_count
      from ZYMOSIS_AFP z
     where z.AFPID = v_AFPID;
    if v_count <= 0 then
      insert into ZYMOSIS_AFP
        (AFPID,
         REPORTID,
         HOUSEHOLDSCOPE,
         HOUSEHOLDADDRESS,
         ADDRESS,
         PALSYDATE,
         PALSYSYMPTOM,
         VAILD,
         CREATOR,
         CREATEDATE,
         MENDER,
         ALTERDATE,
         DIAGNOSISDATE)
      values
        (v_AFPID,
         v_REPORTID,
         v_HOUSEHOLDSCOPE,
         v_HOUSEHOLDADDRESS,
         v_ADDRESS,
         v_PALSYDATE,
         v_PALSYSYMPTOM,
         '1',
         v_CREATOR,
         v_CREATEDATE,
         v_MENDER,
         v_ALTERDATE,
         v_DIAGNOSISDATE);
    else
      update ZYMOSIS_AFP
         set REPORTID         = v_REPORTID,
             HOUSEHOLDSCOPE   = v_HOUSEHOLDSCOPE,
             HOUSEHOLDADDRESS = v_HOUSEHOLDADDRESS,
             ADDRESS          = v_ADDRESS,
             PALSYDATE        = v_PALSYDATE,
             PALSYSYMPTOM     = v_PALSYSYMPTOM,
             MENDER           = v_MENDER,
             ALTERDATE        = v_ALTERDATE,
             DIAGNOSISDATE    = v_DIAGNOSISDATE
       where AFPID = v_AFPID;
    end if;
  end;

  --脑卒中报告卡  add  by  ywk 2013年8月21日 15:47:00
  PROCEDURE usp_EditCardiovacular_Report(v_EditType      varchar, --操作类型
                                        
                                         v_REPORT_NO     varchar DEFAULT '', --报告卡卡号                                 
                                         v_NOOFCLINIC    varchar DEFAULT '', --门诊号 
                                         v_PATID         varchar default '',
                                         v_NAME          varchar default '',
                                         v_IDNO          varchar default '',
                                         v_SEXID         varchar default '',
                                         v_BIRTH         varchar default '',
                                         v_AGE           varchar DEFAULT '',
                                         v_NATIONID      varchar default '',
                                         v_JOBID         varchar default '',
                                         v_OFFICEPLACE   varchar default '',
                                         v_CONTACTTEL    varchar default '',
                                         v_HKPROVICE     varchar default '',
                                         v_HKCITY        varchar default '',
                                         v_HKSTREET      varchar default '',
                                         v_HKADDRESSID   varchar default '',
                                         v_XZZPROVICE    varchar default '',
                                         v_XZZCITY       varchar default '',
                                         v_XZZSTREET     varchar default '',
                                         v_XZZADDRESSID  varchar default '',
                                         v_XZZCOMMITTEES varchar default '',
                                         v_XZZPARM         varchar default '',
                                         v_ICD             varchar default '',
                                         v_DIAGZWMXQCX     varchar default '',
                                         v_DIAGNCX         varchar default '',
                                         v_DIAGNGS         varchar default '',
                                         v_DIAGWFLNZZ      varchar default '',
                                         v_DIAGJXXJGS      varchar default '',
                                         v_DIAGXXCS        varchar default '',
                                         v_DIAGNOSISBASED  varchar default '',
                                         v_DIAGNOSEDATE    varchar default '',
                                         v_ISFIRSTSICK     varchar default '',
                                         v_DIAGHOSPITAL    varchar default '',
                                         v_OUTFLAG         varchar default '',
                                         v_DIEDATE         varchar default '',
                                         v_REPORTDEPT      varchar default '',
                                         v_REPORTUSERCODE  varchar default '',
                                         v_REPORTUSERNAME  varchar default '',
                                         v_REPORTDATE      varchar default '',
                                         v_CREATE_DATE     varchar default '',
                                         v_CREATE_USERCODE varchar default '',
                                         v_CREATE_USERNAME varchar default '',
                                         v_CREATE_DEPTCODE varchar default '',
                                         v_CREATE_DEPTNAME varchar default '',
                                         v_MODIFY_DATE     varchar default '',
                                         v_MODIFY_USERCODE varchar default '',
                                         v_MODIFY_USERNAME varchar default '',
                                         v_MODIFY_DEPTCODE varchar default '',
                                         v_MODIFY_DEPTNAME varchar default '',
                                         v_AUDIT_DATE      varchar default '',
                                         v_AUDIT_USERCODE  varchar default '',
                                         v_AUDIT_USERNAME  varchar default '',
                                         v_AUDIT_DEPTCODE  varchar default '',
                                         v_AUDIT_DEPTNAME  varchar default '',
                                         v_VAILD           varchar default '',
                                         v_CANCELREASON    varchar default '',
                                         v_STATE           varchar default '',
                                         v_CARDPARAM1      varchar default '',
                                         v_CARDPARAM2      varchar default '',
                                         v_CARDPARAM3      varchar default '',
                                         v_CARDPARAM4      varchar default '',
                                         v_CARDPARAM5      varchar default '',
                                         v_NOOFINPAT       varchar default '',
                                          v_REPORTID      varchar DEFAULT '',
                                         o_result          OUT empcurtyp) as
    v_Report_ID_new int;
  BEGIN
  
    --新增传染病报告卡
    IF v_edittype = '1' THEN
    
      select seq_cardiovascularcard_ID.Nextval
        into v_Report_ID_new
        from dual;
      --select 1 into v_Report_ID_new from dual;
      insert into CARDIOVASCULARCARD
        (ID,
         REPORT_NO,
         NOOFCLINIC,
         PATID,
         NAME,
         IDNO,
         SEXID,
         BIRTH,
         AGE,
         NATIONID,
         JOBID,
         OFFICEPLACE,
         CONTACTTEL,
         HKPROVICE,
         HKCITY,
         HKSTREET,
         HKADDRESSID,
         XZZPROVICE,
         XZZCITY,
         XZZSTREET,
         XZZADDRESSID,
         XZZCOMMITTEES,
         XZZPARM,
         ICD,
         DIAGZWMXQCX,
         DIAGNCX,
         DIAGNGS,
         DIAGWFLNZZ,
         DIAGJXXJGS,
         DIAGXXCS,
         DIAGNOSISBASED,
         DIAGNOSEDATE,
         ISFIRSTSICK,
         DIAGHOSPITAL,
         OUTFLAG,
         DIEDATE,
         REPORTDEPT,
         REPORTUSERCODE,
         REPORTUSERNAME,
         REPORTDATE,
         CREATE_DATE,
         CREATE_USERCODE,
         CREATE_USERNAME,
         CREATE_DEPTCODE,
         CREATE_DEPTNAME,
         MODIFY_DATE,
         MODIFY_USERCODE,
         MODIFY_USERNAME,
         MODIFY_DEPTCODE,
         MODIFY_DEPTNAME,
         AUDIT_DATE,
         AUDIT_USERCODE,
         AUDIT_USERNAME,
         AUDIT_DEPTCODE,
         AUDIT_DEPTNAME,
         VAILD,
         CANCELREASON,
         STATE,
         CARDPARAM1,
         CARDPARAM2,
         CARDPARAM3,
         CARDPARAM4,
         CARDPARAM5,
         NOOFINPAT)
      values
        (v_Report_ID_new,
         v_REPORT_NO,
         v_NOOFCLINIC,
         v_PATID,
         v_NAME,
         v_IDNO,
         v_SEXID,
         v_BIRTH,
         v_AGE,
         v_NATIONID,
         v_JOBID,
         v_OFFICEPLACE,
         v_CONTACTTEL,
         v_HKPROVICE,
         v_HKCITY,
         v_HKSTREET,
         v_HKADDRESSID,
         v_XZZPROVICE,
         v_XZZCITY,
         v_XZZSTREET,
         v_XZZADDRESSID,
         v_XZZCOMMITTEES,
         v_XZZPARM,
         v_ICD,
         v_DIAGZWMXQCX,
         v_DIAGNCX,
         v_DIAGNGS,
         v_DIAGWFLNZZ,
         v_DIAGJXXJGS,
         v_DIAGXXCS,
         v_DIAGNOSISBASED,
         v_DIAGNOSEDATE,
         v_ISFIRSTSICK,
         v_DIAGHOSPITAL,
         v_OUTFLAG,
         v_DIEDATE,
         v_REPORTDEPT,
         v_REPORTUSERCODE,
         v_REPORTUSERNAME,
         v_REPORTDATE,
         v_CREATE_DATE,
         v_CREATE_USERCODE,
         v_CREATE_USERNAME,
         v_CREATE_DEPTCODE,
         v_CREATE_DEPTNAME,
         v_MODIFY_DATE,
         v_MODIFY_USERCODE,
         v_MODIFY_USERNAME,
         v_MODIFY_DEPTCODE,
         v_MODIFY_DEPTNAME,
         v_AUDIT_DATE,
         v_AUDIT_USERCODE,
         v_AUDIT_USERNAME,
         v_AUDIT_DEPTCODE,
         v_AUDIT_DEPTNAME,
         v_VAILD,
         v_CANCELREASON,
         v_STATE,
         v_CARDPARAM1,
         v_CARDPARAM2,
         v_CARDPARAM3,
         v_CARDPARAM4,
         v_CARDPARAM5,
         v_NOOFINPAT);
    
      open o_result for
        select v_Report_ID_new from dual;
    
    end if;
  
    --修改保存脑卒中报告卡信息
    IF v_edittype = '2' THEN
      
      update CARDIOVASCULARCARD set  
         REPORT_NO=v_REPORT_NO,
         NOOFCLINIC=v_NOOFCLINIC,
         PATID=v_PATID,
         NAME=v_NAME,
         IDNO=v_IDNO,
         SEXID=v_SEXID,
         BIRTH=v_BIRTH,
         AGE=v_AGE,
         NATIONID=v_NATIONID,
         JOBID=v_JOBID,
         OFFICEPLACE=v_OFFICEPLACE,
         CONTACTTEL=v_CONTACTTEL,
         HKPROVICE=v_HKPROVICE,
         HKCITY=v_HKCITY,
         HKSTREET=v_HKSTREET,
         HKADDRESSID=v_HKADDRESSID,
         XZZPROVICE=v_XZZPROVICE,
         XZZCITY=v_XZZCITY,
         XZZSTREET=v_XZZSTREET,
         XZZADDRESSID=v_XZZADDRESSID,
         XZZCOMMITTEES=v_XZZCOMMITTEES,
         XZZPARM=v_XZZPARM,
         ICD=v_ICD,
         DIAGZWMXQCX=v_DIAGZWMXQCX,
         DIAGNCX=v_DIAGNCX,
         DIAGNGS=v_DIAGNGS,
         DIAGWFLNZZ=v_DIAGWFLNZZ,
         DIAGJXXJGS=v_DIAGJXXJGS,
         DIAGXXCS=v_DIAGXXCS,
         DIAGNOSISBASED=v_DIAGNOSISBASED,
         DIAGNOSEDATE=v_DIAGNOSEDATE,
         ISFIRSTSICK=v_ISFIRSTSICK,
         DIAGHOSPITAL=V_DIAGHOSPITAL,
         OUTFLAG=V_OUTFLAG,
         DIEDATE=V_DIEDATE,
         REPORTDEPT=V_REPORTDEPT,
         REPORTUSERCODE=V_REPORTUSERCODE,
         REPORTUSERNAME=V_REPORTUSERNAME,
         REPORTDATE=V_REPORTDATE,
         CREATE_DATE=nvl(v_CREATE_DATE, CREATE_DATE),
         CREATE_USERCODE=nvl(v_CREATE_USERCODE, CREATE_USERCODE),
         CREATE_USERNAME=nvl(v_CREATE_USERNAME, CREATE_USERNAME),
         CREATE_DEPTCODE=nvl(v_CREATE_DEPTCODE, CREATE_DEPTCODE),
         CREATE_DEPTNAME=nvl(v_CREATE_DEPTNAME, CREATE_DEPTNAME),
         MODIFY_DATE=nvl(v_MODIFY_DATE, MODIFY_DATE),
         MODIFY_USERCODE=nvl(v_MODIFY_USERCODE, MODIFY_USERCODE),
         MODIFY_USERNAME=nvl(v_MODIFY_USERNAME, MODIFY_USERNAME),
         MODIFY_DEPTCODE=nvl(v_MODIFY_DEPTCODE, MODIFY_DEPTCODE),
         MODIFY_DEPTNAME=nvl(v_MODIFY_DEPTNAME, MODIFY_DEPTNAME),
         AUDIT_DATE=nvl(v_AUDIT_DATE, AUDIT_DATE),
         AUDIT_USERCODE=nvl(v_AUDIT_USERCODE, AUDIT_USERCODE),
         AUDIT_USERNAME=nvl(v_AUDIT_USERNAME, AUDIT_USERNAME),
         AUDIT_DEPTCODE=nvl(v_AUDIT_DEPTCODE, AUDIT_DEPTCODE),
         AUDIT_DEPTNAME=nvl(v_AUDIT_DEPTNAME, AUDIT_DEPTNAME),
         VAILD=V_VAILD,
         CANCELREASON=V_CANCELREASON,
         STATE=V_STATE,
         CARDPARAM1=V_CARDPARAM1,
         CARDPARAM2=V_CARDPARAM2,
         CARDPARAM3=V_CARDPARAM3,
         CARDPARAM4=V_CARDPARAM4,
         CARDPARAM5=V_CARDPARAM5,
         NOOFINPAT=V_NOOFINPAT
      where ID=v_REPORTID;
    
      open o_result for
    
    select v_REPORTID from dual;
    end if;
  
    --作废脑卒中报告卡信息
    IF v_edittype = '3' THEN
    
      update CARDIOVASCULARCARD ca
         set ca.vaild = 0
       where ca.id = v_REPORTID;
      --select 1 into v_Report_ID_new from dual;
    
      open o_result for
        select 1 into v_Report_ID_new from dual;
    end if;
  
    --根据传入的脑卒中报告卡ID查询报告卡信息
    IF v_edittype = '4' THEN
    
      open o_result for
        select * from CARDIOVASCULARCARD a where a.id = v_REPORTID;
    
    end if;
  
  end;

END;
/
