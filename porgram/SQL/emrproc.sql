CREATE OR REPLACE PACKAGE emrproc IS
  -- global 变量宣言
  /* 变量名 数据型; */
  -- PROCEDURE，函数宣言
  /* PROCEDURE名字(变量...); */
  /* FUNCTION  名称(参数...) RETURN 数据型; */
  TYPE empcurtyp IS REF CURSOR;


 PROCEDURE USP_EMR_DISOUTHOS(v_PATNOOFHIS       VARCHAR2,
                             o_result           OUT empcurtyp);

  PROCEDURE USP_EMR_PATRECFILE(v_flag             INTEGER,
                               v_indx             INTEGER,
                               v_NOOFINPAT        INTEGER,
                               v_TEMPLATEID       VARCHAR2,
                               v_FileNAME         VARCHAR2,
                               v_FileCONTENT      clob,
                               v_SORTID           VARCHAR2,
                               v_OWNER            VARCHAR2,
                               v_AUDITOR          VARCHAR2,
                               v_CREATETIME       CHAR,
                               v_AUDITTIME        CHAR,
                               v_VALID            INTEGER,
                               v_HASSUBMIT        INTEGER,
                               v_HASPRINT         INTEGER,
                               v_HASSIGN          INTEGER,
                               v_captiondatetime  varchar2,
                               v_FIRSTDAILYFLAG   varchar2,
                               v_py               varchar2,
                               v_wb               varchar2,
                               v_isyihuangoutong  varchar2,
                               v_isconfigpagesize varchar2,
                               v_DepartCode       varchar2,
                               v_wardCode         varchar2,
                               o_result           OUT empcurtyp);

  /*
  * 描述  审批申请借阅病历
  */
  PROCEDURE usp_AuditApplyRecord(

                                 v_AuditType varchar, --审核类型
                                 v_ManID     varchar, --审核人ID
                                 v_ID        numeric --记录ID
                                 );

  /*
  * 功能说明      删除选定岗位
  */
  PROCEDURE usp_DeleteJob(v_ID VARCHAR);

  /*
  * 功能说明      删除选定岗位
  */

  /*
  * 功能说明      删除选定岗位
  */
  PROCEDURE usp_DeleteJobPermission(v_ID VARCHAR);

  /*
  *  功能说明      删除某个员工信息
  */
  PROCEDURE usp_DeleteUserInfo(v_ID VARCHAR);

  /*
  *  描述  编辑LONG_ORDER模板
  */
  PROCEDURE usp_EditEmrLONG_ORDER(v_EditType         varchar default '', --操作类型
                                  v_LONGID           INTEGER default '',
                                  v_NOOFINPAT        INTEGER default '',
                                  v_GROUPID          INTEGER default '',
                                  v_GROUPFLAG        INTEGER default '',
                                  v_WARDID           VARCHAR2 default '',
                                  v_DEPTID           VARCHAR2 default '',
                                  v_TYPEDOCTOR       VARCHAR2 default '',
                                  v_TYPEDATE         CHAR default '',
                                  v_AUDITOR          VARCHAR2 default '',
                                  v_DATEOFAUDIT      CHAR default '',
                                  v_EXECUTOR         VARCHAR2 default '',
                                  v_EXECUTEDATE      CHAR default '',
                                  v_CANCELDOCTOR     VARCHAR2 default '',
                                  v_CANCELDATE       CHAR default '',
                                  v_CEASEDCOCTOR     VARCHAR2 default '',
                                  v_CEASEDATE        CHAR default '',
                                  v_CEASENURSE       VARCHAR2 default '',
                                  v_CEASEADUDITDATE  CHAR default '',
                                  v_STARTDATE        CHAR default '',
                                  v_TOMORROW         INTEGER default '',
                                  v_PRODUCTNO        INTEGER default '',
                                  v_NORMNO           INTEGER default '',
                                  v_MEDICINENO       INTEGER default '',
                                  v_DRUGNO           VARCHAR2 default '',
                                  v_DRUGNAME         VARCHAR2 default '',
                                  v_DRUGNORM         VARCHAR2 default '',
                                  v_ITEMTYPE         INTEGER default '',
                                  v_MINUNIT          VARCHAR2 default '',
                                  v_DRUGDOSE         INTEGER default '',
                                  v_DOSEUNIT         VARCHAR2 default '',
                                  v_UNITRATE         INTEGER default '',
                                  v_UNITTYPE         INTEGER default '',
                                  v_DRUGUSE          VARCHAR2 default '',
                                  v_BATCHNO          VARCHAR2 default '',
                                  v_EXECUTECOUNT     INTEGER default '',
                                  v_EXECUTECYCLE     INTEGER default '',
                                  v_CYCLEUNIT        INTEGER default '',
                                  v_DATEOFWEEK       CHAR default '',
                                  v_INNEREXECUTETIME VARCHAR2 default '',
                                  v_EXECUTEDEPT      VARCHAR2 default '',
                                  v_ENTRUST          VARCHAR2 default '',
                                  v_ORDERTYPE        INTEGER default '',
                                  v_ORDERSTATUS      INTEGER default '',
                                  v_SPECIALMARK      INTEGER default '',
                                  v_CEASEREASON      INTEGER default '',
                                  v_CURGERYID        INTEGER default '',
                                  v_CONTENT          VARCHAR2 default '',
                                  v_SYNCH            INTEGER default '',
                                  v_MEMO             VARCHAR2 default '',
                                  v_DJFL             VARCHAR2 default '',
                                  o_result           OUT empcurtyp);

  /*
  *  描述  编辑TEMP_ORDER临时医嘱表
  */
  PROCEDURE usp_EditEmrTEMP_ORDER(v_EditType         VARCHAR2 default '', --操作类型
                                  v_TEMPID           INTEGER default '',
                                  v_NOOFINPAT        INTEGER default '',
                                  v_GROUPID          INTEGER default '',
                                  v_GROUPFLAG        INTEGER default '',
                                  v_WARDID           VARCHAR2 default '',
                                  v_DEPTID           VARCHAR2 default '',
                                  v_TYPEDOCTOR       VARCHAR2 default '',
                                  v_TYPEDATE         CHAR default '',
                                  v_AUDITOR          VARCHAR2 default '',
                                  v_DATEOFAUDIT      CHAR default '',
                                  v_EXECUTOR         VARCHAR2 default '',
                                  v_EXECUTEDATE      CHAR default '',
                                  v_CANCELDOCTOR     VARCHAR2 default '',
                                  v_CANCELDATE       CHAR default '',
                                  v_STARTDATE        CHAR default '',
                                  v_PRODUCTNO        INTEGER default '',
                                  v_NORMNO           INTEGER default '',
                                  v_MEDICINENO       INTEGER default '',
                                  v_DRUGNO           VARCHAR2 default '',
                                  v_DRUGNAME         VARCHAR2 default '',
                                  v_DRUGNORM         VARCHAR2 default '',
                                  v_ITEMTYPE         INTEGER default '',
                                  v_MINUNIT          VARCHAR2 default '',
                                  v_DRUGDOSE         INTEGER default '',
                                  v_DOSEUNIT         VARCHAR2 default '',
                                  v_UNITRATE         INTEGER default '',
                                  v_UNITTYPE         INTEGER default '',
                                  v_DRUGUSE          VARCHAR2 default '',
                                  v_BATCHNO          VARCHAR2 default '',
                                  v_EXECUTECOUNT     INTEGER default '',
                                  v_EXECUTECYCLE     INTEGER default '',
                                  v_CYCLEUNIT        INTEGER default '',
                                  v_DATEOFWEEK       CHAR default '',
                                  v_INNEREXECUTETIME VARCHAR2 default '',
                                  v_ZXTS             INTEGER default '',
                                  v_TOTALDOSE        INTEGER default '',
                                  v_ENTRUST          VARCHAR2 default '',
                                  v_ORDERTYPE        INTEGER default '',
                                  v_ORDERSTATUS      INTEGER default '',
                                  v_SPECIALMARK      INTEGER default '',
                                  v_CEASEID          INTEGER default '',
                                  v_CEASEDATE        CHAR default '',
                                  v_CONTENT          VARCHAR2 default '',
                                  v_SYNCH            INTEGER default '',
                                  v_MEMO             VARCHAR2 default '',
                                  v_FORMTYPE         VARCHAR2 default '',
                                  o_result           OUT empcurtyp);
  /*
  *  描述  编辑过敏史信息
  */
  PROCEDURE usp_EditAllergyHistoryInfo(v_EditType     varchar, --编辑信息类型：1：添加、2：修改、3：删除
                                       v_ID           numeric, --唯一列
                                       v_NoOfInpat    numeric default '-1', --病人首页序号
                                       v_AllergyID    Int default -1, --过敏类型
                                       v_AllergyLevel int default -1, --过敏程度
                                       v_Doctor       varchar default '', --代理医生
                                       v_AllergyPart  varchar default '', --过敏部位
                                       v_ReactionType varchar default '', --反应类型
                                       v_Memo         varchar default '' --备注
                                       );

  /*
  *  描述  编辑家族信息
  */
  PROCEDURE usp_EditFamilyHistoryInfo(v_EditType  varchar, --编辑信息类型：1：添加、2：修改、3：删除
                                      v_ID        numeric, --联系人编号，是病人联系人的唯一标识
                                      v_NoOfInpat numeric default '-1', --首页序号(住院流水号)(Inpatient.NoOfInpat)
                                      v_Relation  varchar default '', --家族关系
                                      v_Birthday  varchar default '', --出生日期（在前台显示年龄）
                                      v_DiseaseID varchar default '', --病种代码
                                      v_Breathing int default '', --是否健在(1:是：0否)
                                      v_Cause     varchar default '', --死亡原因
                                      v_Memo      varchar default '' --备注
                                      );

  /*
  *  描述  编辑疾病史信息
  */
  PROCEDURE usp_EditIllnessHistoryInfo(v_EditType     varchar, --编辑信息类型：1：添加、2：修改、3：删除
                                       v_ID           numeric, --唯一列
                                       v_NoOfInpat    numeric default '-1', --病人首页序号
                                       v_DiagnosisICD varchar default '', --病种代码
                                       v_Discuss      varchar default '', --疾病评论
                                       v_DiseaseTime  varchar default '', --病发时间
                                       v_Cure         int default '', --是否治愈
                                       v_Memo         varchar default '' --备注
                                       );

  /*
  *  描述  保存护理信息数据
  */
  PROCEDURE usp_EditNotesOnNursingInfoNew(v_NoOfInpat        numeric, --首页序号(住院流水号)
                                          v_DateOfSurvey     varchar, --测量日期期（格式2010-01-01）
                                          v_Indx             varchar, --序号
                                          v_TimeSlot         varchar, --测量时间段
                                          v_Temperature      varchar default '', --患者体温
                                          v_WayOfSurvey      int default 8800, --体温测量方式代码
                                          v_Pulse            varchar default '', --脉搏
                                          v_HeartRate        varchar default '', --心率
                                          v_Breathe          varchar default '', --患者呼吸
                                          v_BloodPressure    varchar default '', --患者血压
                                          v_TimeOfShit       varchar default '', --大便次数，格式：1*(3/2E ),'*'表示大便失禁
                                          v_CountIn          varchar default '', --患者总入量
                                          v_CountOut         varchar default '', --患者总出量
                                          v_Drainage         varchar default '', --引流量
                                          v_Height           varchar default '', --患者身高
                                          v_Weight           varchar default '', --患者体重
                                          v_Allergy          varchar default '', --患者过敏物
                                          v_DaysAfterSurgery varchar default '', --手术后日数
                                          v_DayOfHospital    varchar default '', --住院天数
                                          v_PhysicalCooling  varchar default '', --物理降温
                                          v_DoctorOfRecord   varchar, --记录医生
                                          v_PhysicalHotting  varchar default '', --物理升温 edit by ywk
                                          v_PainInfo         varchar default '' --疼痛
                                          );
  /*
  *  描述  保存护理信息数据
  */
  PROCEDURE usp_EditNotesOnNursingInfo(v_NoOfInpat        numeric, --首页序号(住院流水号)
                                       v_DateOfSurvey     varchar, --测量日期期（格式2010-01-01）
                                       v_TimeSlot         varchar, --测量时间段
                                       v_Temperature      varchar default '', --患者体温
                                       v_WayOfSurvey      int default 8800, --体温测量方式代码
                                       v_Pulse            varchar default '', --脉搏
                                       v_HeartRate        varchar default '', --心率
                                       v_Breathe          varchar default '', --患者呼吸
                                       v_BloodPressure    varchar default '', --患者血压
                                       v_TimeOfShit       varchar default '', --大便次数，格式：1*(3/2E ),'*'表示大便失禁
                                       v_CountIn          varchar default '', --患者总入量
                                       v_CountOut         varchar default '', --患者总出量
                                       v_Drainage         varchar default '', --引流量
                                       v_Height           varchar default '', --患者身高
                                       v_Weight           varchar default '', --患者体重
                                       v_Allergy          varchar default '', --患者过敏物
                                       v_DaysAfterSurgery varchar default '', --手术后日数
                                       v_DayOfHospital    varchar default '', --住院天数
                                       v_PhysicalCooling  varchar default '', --物理降温
                                       v_DoctorOfRecord   varchar, --记录医生
                                       v_PhysicalHotting  varchar default '', --物理升温 edit by ywk
                                       v_PainInfo         varchar default '' --疼痛
                                       );
  --护士三测单信息维护中。病人状态信息的维护 add by ywk 2012年4月23日14:05:18

  PROCEDURE usp_Edit_PatStates(v_EditType  varchar default '', --操作类型
                               v_CCode     varchar default '',
                               V_ID        varchar default '',
                               v_NoofInPat varchar default '',
                               v_DoTime    varchar default '',
                               v_Patid     varchar default '',
                               o_result    OUT empcurtyp);

  /*
  * 编辑第一联系人信息
  */
  PROCEDURE usp_EditPatientContacts(v_EditType   varchar, --编辑信息类型：1：添加、2：修改、3：删除
                                    v_ID         numeric default '0', --联系人编号，是病人联系人的唯一标识
                                    v_NoOfInpat  numeric default '0', --首页序号(住院流水号)(Inpatient.NoOfInpat)
                                    v_Name       varchar default '', --联系人名
                                    v_Sex        varchar default '', --性别
                                    v_Relation   varchar default '', --联系关系(Dictionary_detail.DetailID, ID = '44')
                                    v_Address    varchar default '', --联系地址
                                    v_WorkUnit   varchar default '', --联系(人)单位
                                    v_HomeTel    varchar default '', --联系人家庭电话
                                    v_WorkTel    varchar default '', --联系人工作电话
                                    v_PostalCode varchar default '', --联系邮编
                                    v_Tag        varchar default '' --联系人标志
                                    );

  /*
  * 编辑个人史信息
  */
  PROCEDURE usp_EditPersonalHistoryInfo(v_NoOfInpat    numeric, --病人首页序号
                                        v_Marital      varchar, --婚姻状况
                                        v_NoOfChild    int, --孩子数量
                                        v_JobHistory   varchar, --职业经历
                                        v_DrinkWine    int, --是否饮酒
                                        v_WineHistory  varchar, --饮酒史
                                        v_Smoke        int, --是否吸烟
                                        v_SmokeHistory varchar, --吸烟史
                                        v_BirthPlace   varchar, --出生地（省市）
                                        v_PassPlace    varchar, --经历地（省市）
                                        v_Memo         varchar default '' --备注
                                        );

  /*
  * 编辑手术史信息
  */
  PROCEDURE usp_EditSurgeryHistoryInfo(v_EditType    varchar, --编辑信息类型：1：添加、2：修改、3：删除
                                       v_ID          numeric, --唯一列
                                       v_NoOfInpat   numeric default '-1', --病人首页序号
                                       v_SurgeryID   varchar default '', --手术代码(Surgery.ID)
                                       v_DiagnosisID varchar default '', --疾病（Diagnosis.ICD）
                                       v_Discuss     varchar default '', --评论
                                       v_Doctor      varchar default '', --手术医生
                                       v_Memo        varchar default '' --备注
                                       );

  /*
  * 描述 .NET模型选择
  */
  PROCEDURE usp_Emr_ModelSearcher(v_flag   int,
                                  v_type   varchar,
                                  o_result OUT empcurtyp);

  /*
  * 描述 根据病人首页序号获取病人信息
  */
  PROCEDURE USP_EMR_GETPATINFO(v_NoOfinpat varchar, o_result OUT empcurtyp);


  /*
  * 描述 查询可用的成套医嘱
  */
  procedure usp_Emr_QueryOrderSuites(v_DeptID   varchar,
                                     v_WardID   varchar,
                                     v_DoctorID varchar,
                                     v_yzlr     int default 1,
                                     o_result   OUT empcurtyp,
                                     o_result1  OUT empcurtyp);

  /*
  * 描述 设置医嘱的分组序号
  */
  procedure usp_Emr_SetOrderGroupSerialNo(v_NoOfInpat numeric,
                                          v_OrderType int,
                                          v_onlynew   int default 1);

  /*
  * 获取申请借阅病历
  */
  PROCEDURE usp_GetApplyRecordNew(v_DateBegin varchar, --开始日期
                                  v_DateEnd   varchar, --结束日期
                                  --v_DateInBegin  varchar, --入院开始日期
                                  --v_DateInEnd    varchar, --入院结束日期
                                  v_PatientName varchar, --病人姓名
                                  v_DocName     VARCHAR, --申请医师姓名
                                  v_OutHosDept  varchar, --出院科室科室
                                  o_result      OUT empcurtyp);
  /*
  * 获取申请借阅病历
  */
  PROCEDURE usp_GetApplyRecord(v_DateBegin  varchar, --开始日期
                               v_DateEnd    varchar, --结束日期
                               v_OutHosDept varchar, --出院科室科室
                               o_result     OUT empcurtyp);

  /*
  * 描述  获取系统当前参数
  */
  PROCEDURE usp_GetCurrSystemParam(v_GetType int, --操作类型
                                   o_result  OUT empcurtyp);

  /*
  * 描述  获取科室病历失分点
  */
  PROCEDURE usp_GetDeptDeductPoint(v_DeptCode      varchar default '',
                                   v_DateTimeBegin varchar,
                                   v_DateTimeEnd   varchar,
                                   v_PointID       varchar default '',
                                   v_QCStatType    int,
                                   o_result        OUT empcurtyp);

  /*
  * 描述  获取医生任务信息内容
  */
  PROCEDURE usp_GetDoctorTaskInfo(v_Wardid  varchar, --病区
                                  v_Deptids varchar, --科室
                                  v_UserID  varchar, --用户ID
                                  v_Time    varchar, --时间
                                  o_result  OUT empcurtyp);

  /*
  * 描述  获取医院信息
  */
  PROCEDURE usp_GetHospitalInfo(o_result OUT empcurtyp);

  /*
  * 描述  获取质量评分
  */
  PROCEDURE usp_GetIemInfo(v_NoOfInpat int,
                           o_result    OUT empcurtyp,
                           o_result1   OUT empcurtyp,
                           o_result2   OUT empcurtyp,
                           o_result3   OUT empcurtyp);

  /*
  * 描述  获取科室质量管理未归档病历
  */
  PROCEDURE usp_GetInpatientFiling(v_DeptCode      varchar default '',
                                   v_InpatName     varchar default '',
                                   v_DateTimeBegin varchar,
                                   v_DateTimeEnd   varchar,
                                   v_QCStatType    int,
                                   o_result        OUT empcurtyp);

  /*
  * 描述  根据传入的时间段以及病人首页序号  查询病人医技报告信息
  */
  PROCEDURE usp_GetInpatientReport(v_NoOfInpat     varchar default '',
                                   v_DateTimeBegin varchar default '',
                                   v_DateTimeEnd   varchar default '',
                                   v_SubmitDocID   varchar default '',
                                   v_ReportID      varchar default '',
                                   v_QCStatType    varchar default '',
                                   o_result        OUT empcurtyp);

  /*
  * 描述    输出所有岗位信息
  */
  PROCEDURE usp_GetJobPermissionInfo(v_JobId  VARCHAR,
                                     o_result out empcurtyp);

  /*
  * 描述    输出所有岗位信息
  */
  PROCEDURE usp_GetKnowledgePublicInfo(v_OrderType numeric, --所属类别
                                       o_result    out empcurtyp);

  /*
  * 描述    取需要在lookupeditor里显示的数据,最好包括ID，Name,Py,Memo，这样可以在APP里调用时用统一的方法
  */
  PROCEDURE usp_GetLookUpEditorData(v_QueryType numeric, --查询的类型
                                    v_QueryID   numeric default 0,
                                    o_result    out empcurtyp);

  /*********************************************************************************/
  PROCEDURE usp_getmedicalrrecordviewnew(v_deptcode        VARCHAR DEFAULT '',
                                         v_datetimebegin   VARCHAR,
                                         v_datetimeend     VARCHAR,
                                         v_datetimeinbegin VARCHAR,
                                         v_datetimeinend   VARCHAR,
                                         v_patientname     VARCHAR DEFAULT '',
                                         v_recordid        VARCHAR DEFAULT '',
                                         v_applydoctor     VARCHAR DEFAULT '',
                                         v_qcstattype      INT,
                                         --Add wwj 用于模糊查询
                                         v_patid   VARCHAR DEFAULT '',
                                         v_indiag  VARCHAR DEFAULT '', --入院诊断
                                         v_outdiag VARCHAR DEFAULT '', --出院诊断
                                         v_curdiag VARCHAR DEFAULT '', --当前诊断
                                         o_result  OUT empcurtyp);

  /*
  * 描述    按科室或科室对应病人的归档或未归档电子病历
  */
  PROCEDURE usp_GetMedicalRrecordView(v_DeptCode      varchar default '',
                                      v_DateTimeBegin varchar,
                                      v_DateTimeEnd   varchar,
                                      v_PatientName   varchar default '',
                                      v_RecordID      varchar default '',
                                      v_ApplyDoctor   varchar default '',
                                      v_QCStatType    int,
                                      --Add wwj 用于模糊查询
                                      v_PatID   varchar default '',
                                      v_outdiag VARCHAR DEFAULT '', --add by cyq 2012-12-07
                                      o_result  out empcurtyp);

  /*
  * 描述    保存申请借阅电子病历信息
  */
  PROCEDURE usp_GetMedicalRrecordViewFrm(v_GetType varchar, --获取数据类型，1：科室、2：申请借阅目的、3：申请期限单位
                                         o_result  out empcurtyp);

  /*
  * 描述    获取护理信息数据
  */
  PROCEDURE usp_GetNotesOnNursingInfo(v_NoOfInpat    numeric, --首页序号(住院流水号)
                                      v_DateOfSurvey varchar, --测量日期期（格式2010-01-01）
                                      o_result       out empcurtyp);

  /*
  * 描述    获取护理信息数据
  */
  PROCEDURE usp_GetNursingRecordByDate(v_NoOfInpat numeric default '0', --首页序号
                                       v_BeginDate varchar, --起始时间
                                       v_EndDate   varchar, --终止时间
                                       o_result    out empcurtyp);

  /*
  * 描述    获取护理文书项目窗体参数
  */
  PROCEDURE usp_GetNursingRecordParam(v_FrmType varchar, --窗体操作类型
                                      o_result  out empcurtyp);

  /*
  * 描述    获取护理文书项目窗体参数
  */
  PROCEDURE usp_GetPatientBedInfoByDate(v_NoOfInpat    numeric default '0', --首页序号
                                        v_AllocateDate varchar, --指定的时间
                                        o_result       out empcurtyp);

  /*
  * 描述    usp_GetPatientInfoForThreeMeas
  */
  PROCEDURE usp_GetPatientInfoForThreeMeas(v_NoOfInpat numeric default '0', --首页序号
                                           o_result    out empcurtyp);

  --根据病种name返回病种ICD
  PROCEDURE usp_getdiagicdbyname(v_name   varchar, --病种名称
                                 o_result OUT empcurtyp);

  --根据病种ICD返回病种名称
  /*********add by wyt 2012-08-15************************************************************************/
  PROCEDURE usp_getdiagnamebyicd(v_icd    varchar, --病种编号
                                 o_result OUT empcurtyp);

  --查询某诊断医师关联病种
  /*************add by wyt 2012-08-15****************************************************************/
  PROCEDURE usp_getdiagbyattendphysician(v_userid varchar, --用户编号
                                         o_result OUT empcurtyp);

  /*
  * 描述    获取病历管理窗体控件初始化数据
  */
  PROCEDURE usp_GetRecordManageFrm(v_FrmType    varchar, --窗体类型，1n：病人信息管理维护窗体、2n：病人病历史信息窗体
                                   v_PatNoOfHis numeric default '0', --首页序号
                                   v_CategoryID varchar default '', --(字典)类别代码 、或父节点代码 、首页序号
                                   o_result     out empcurtyp);

  /*
  *描述 获取诊断医师未归档病历（多条件查询）
  */
  PROCEDURE usp_getattendrecordnoonfile(v_dateOutHosBegin VARCHAR, --出院开始日期
                                        v_dateOutHosEnd   VARCHAR, --出院结束日期
                                        v_dateInHosBegin  VARCHAR, --入院开始日期
                                        v_dateInHosEnd    VARCHAR, --入院截止日期
                                        v_deptid          VARCHAR, --科室代码
                                        v_status          VARCHAR, --病历状态
                                        v_patientname     VARCHAR DEFAULT '', --病人姓名
                                        v_recordid        VARCHAR DEFAULT '', --病历号
                                        v_indiag          VARCHAR DEFAULT '', --入院诊断
                                        v_outdiag         VARCHAR DEFAULT '', --出院诊断
                                        v_curdiag         VARCHAR DEFAULT '', --当前诊断
                                        v_diaGroupStatus  VARCHAR, --诊断分组状态
                                        v_PatientStatus   VARCHAR, --病人状态
                                        v_ingroupid       VARCHAR, --分组ID1
                                        v_outgroupid      VARCHAR, --分组ID2
                                        o_result          OUT empcurtyp);

  /*
  *描述 获取未归档病历（多条件查询）
  */
  PROCEDURE usp_getrecordnoonfileWyt(v_dateOutHosBegin VARCHAR, --出院开始日期
                                     v_dateOutHosEnd   VARCHAR, --出院结束日期
                                     v_dateInHosBegin  VARCHAR, --入院开始日期
                                     v_dateInHosEnd    VARCHAR, --入院截止日期
                                     v_deptid          VARCHAR, --科室代码
                                     v_indiag          VARCHAR, --入院诊断
                                     v_outdiag         VARCHAR, --出院诊断
                                     v_status          VARCHAR, --病历状态
                                     v_patientname     VARCHAR DEFAULT '', --病人姓名
                                     v_patientid       VARCHAR DEFAULT '', --病人ID
                                     v_recordid        VARCHAR DEFAULT '', --病历号
                                     v_physician       VARCHAR DEFAULT '', --住院医师号
                                     o_result          OUT empcurtyp);
  /*
  * 描述    获取未归档病历
  */
  PROCEDURE usp_GetRecordNoOnFile(v_DateBegin   varchar, --开始日期
                                  v_DateEnd     varchar, --结束日期
                                  v_DeptID      varchar, --科室代码
                                  v_Status      varchar, --病历状态
                                  v_PatientName varchar default '', --病人姓名
                                  v_RecordID    varchar default '', --病历号
                                  o_result      out empcurtyp);

  /*
  * 描述    获取未归档病历
  */
  PROCEDURE usp_GetRecordNoOnFileNew(v_DateBegin   varchar, --开始日期
                                     v_DateEnd     varchar, --结束日期
                                     v_DeptID      varchar, --科室代码
                                     v_Status      varchar, --病历状态
                                     v_PatientName varchar default '', --病人姓名
                                     v_RecordID    varchar default '', --病历号
                                     o_result      out empcurtyp);

  /*
  * 描述    获取已归档病历
  */
  PROCEDURE usp_getrecordonfile(v_datebegin  VARCHAR, --开始日期
                                v_dateend    VARCHAR, --结束日期
                                v_patid      VARCHAR DEFAULT '', --住院号码
                                v_name       VARCHAR DEFAULT '', --患者姓名
                                v_sexid      VARCHAR DEFAULT '', --病人性别
                                v_agebegin   VARCHAR DEFAULT '', --开始年龄
                                v_ageend     VARCHAR DEFAULT '', --结束年龄
                                v_outhosdept VARCHAR DEFAULT '', --出院科室科室
                                v_indiag     VARCHAR DEFAULT '', --入院诊断
                                v_outdiag    VARCHAR DEFAULT '', --出院诊断
                                v_surgeryid  VARCHAR DEFAULT '', --手术代码
                                v_physician  VARCHAR DEFAULT '', --住院医师代码
                                o_result     OUT empcurtyp);

  /*
  * 描述    获取待签收归还病历
  */
  PROCEDURE usp_GetSignInRecordNew(v_DateBegin   varchar, --开始日期
                                   v_DateEnd     varchar, --结束日期
                                   v_DateInBegin varchar, --入院开始日期
                                   v_DateInEnd   varchar, --入院结束日期
                                   v_PatientName varchar, --病人姓名
                                   v_OutHosDept  varchar, --出院科室科室
                                   o_result      out empcurtyp);

  /*
  * 描述    获取待签收归还病历
  */
  PROCEDURE usp_GetSignInRecord(v_DateBegin  varchar, --开始日期
                                v_DateEnd    varchar, --结束日期
                                v_OutHosDept varchar, --出院科室科室
                                o_result     out empcurtyp);
  /*
  * 描述    获取质量评分
  */
  PROCEDURE usp_GetTemplatePersonGroup(v_UserID  varchar default '',
                                       o_result  OUT empcurtyp,
                                       o_result1 OUT empcurtyp);

  /*
  * 描述    病人信息表触发过程，
  */
  PROCEDURE usp_Inpatient_Trigger(v_syxh varchar2 --病人首页序号
                                  );

  /*
  * 描述    插入图片信息
  */
  PROCEDURE usp_InsertImage(v_Sort   int,
                            v_Name   varchar,
                            v_Memo   varchar default '',
                            v_ID     numeric default 0,
                            o_result OUT empcurtyp);

  /*
  * 描述    插入岗位信息
  */
  PROCEDURE usp_InsertJob(v_Id VARCHAR, v_Title VARCHAR, v_Memo VARCHAR);

  /*
  * 描述    增加新的授权信息
  */
  PROCEDURE usp_InsertJobPermission(v_ID         VARCHAR,
                                    v_Moduleid   VARCHAR,
                                    v_Modulename VARCHAR);

  /*
  * 描述    添加病人护理文档
  */
  procedure usp_InsertNursRecordTable(v_NoOfInpat  numeric, --首页序号(住院流水号)
                                      v_name       varchar, --记录单名称（模板名称+日期+时间）
                                      v_Content    blob, --数据内容
                                      v_TemplateID numeric, --模板ID
                                      v_version    varchar, --模板版本(自定义输入,默认为1.00.00,可以自增长最后1位)
                                      v_Department varchar, --病人所属科室代码
                                      v_SortID     numeric, --模板分类代码
                                      v_Valid      int --有效记录(CategoryDetail.ID CategoryID = 0)
                                      );

  /*
  * 描述    新增护理文档模板，并返回ID
  */
  procedure usp_InsertNursTableTemplate(v_Name     varchar, --模板名称
                                        v_Describe varchar, --模板描述
                                        v_Content  blob, --模板内容
                                        v_version  varchar, --模板版本(自定义输入,默认为1.00.00,可以自增长最后1位)
                                        v_SortID   varchar, --模板分类代码
                                        v_Valid    int, --有效记录(CategoryDetail.ID CategoryID = 0)
                                        o_result   OUT empcurtyp);
  /*
  * 描述    获取质量评分
  */
  PROCEDURE usp_InsertTemplatePersonGroup(v_UserID             varchar default '',
                                          v_NodeID             int default 0,
                                          v_TemplatePersonID   int default 0,
                                          v_ParentNodeID       int default 0,
                                          v_Name               varchar default '',
                                          v_TypeID             int default 0,
                                          v_TemplatePersonName varchar default '',
                                          v_TemplatePersonMemo varchar default '');

  /*
  * 描述    用户登录相关修改
  */
  PROCEDURE usp_InsertUserLogIn(v_ID          varchar,
                                v_ModuleId    varchar,
                                v_HostName    varchar,
                                v_MACADDR     varchar,
                                v_Client_ip   varchar,
                                v_Reason_id   Varchar,
                                v_Create_user varchar);

  PROCEDURE usp_Insert_Iem_Mainpage_Basic(v_PatNoOfHis               numeric,
                                          v_NoOfInpat                numeric,
                                          v_PayID                    varchar,
                                          v_SocialCare               varchar,
                                          v_InCount                  int,
                                          v_Name                     varchar,
                                          v_SexID                    varchar,
                                          v_Birth                    char,
                                          v_Marital                  varchar,
                                          v_JobID                    varchar,
                                          v_ProvinceID               varchar,
                                          v_CountyID                 varchar,
                                          v_NationID                 varchar,
                                          v_NationalityID            varchar,
                                          v_IDNO                     varchar,
                                          v_Organization             varchar,
                                          v_OfficePlace              varchar,
                                          v_OfficeTEL                varchar,
                                          v_OfficePost               varchar,
                                          v_NativeAddress            varchar,
                                          v_NativeTEL                varchar,
                                          v_NativePost               varchar,
                                          v_ContactPerson            varchar,
                                          v_Relationship             varchar,
                                          v_ContactAddress           varchar,
                                          v_ContactTEL               varchar,
                                          v_AdmitDate                varchar,
                                          v_AdmitDept                varchar,
                                          v_AdmitWard                varchar,
                                          v_Days_Before              numeric,
                                          v_Trans_Date               varchar,
                                          v_Trans_AdmitDept          varchar,
                                          v_Trans_AdmitWard          varchar,
                                          v_Trans_AdmitDept_Again    varchar,
                                          v_OutWardDate              varchar,
                                          v_OutHosDept               varchar,
                                          v_OutHosWard               varchar,
                                          v_Actual_Days              numeric,
                                          v_Death_Time               varchar,
                                          v_Death_Reason             varchar,
                                          v_AdmitInfo                varchar,
                                          v_In_Check_Date            varchar,
                                          v_Pathology_Diagnosis_Name varchar,
                                          v_Pathology_Observation_Sn varchar,
                                          v_Ashes_Diagnosis_Name     varchar,
                                          v_Ashes_Anatomise_Sn       varchar,
                                          v_Allergic_Drug            varchar,
                                          v_Hbsag                    numeric,
                                          v_Hcv_Ab                   numeric,
                                          v_Hiv_Ab                   numeric,
                                          v_Opd_Ipd_Id               numeric,
                                          v_In_Out_Inpatinet_Id      numeric,
                                          v_Before_After_Or_Id       numeric,
                                          v_Clinical_Pathology_Id    numeric,
                                          v_Pacs_Pathology_Id        numeric,
                                          v_Save_Times               numeric,
                                          v_Success_Times            numeric,
                                          v_Section_Director         varchar,
                                          v_Director                 varchar,
                                          v_Vs_Employee_Code         varchar,
                                          v_Resident_Employee_Code   varchar,
                                          v_Refresh_Employee_Code    varchar,
                                          v_Master_Interne           varchar,
                                          v_Interne                  varchar,
                                          v_Coding_User              varchar,
                                          v_Medical_Quality_Id       numeric,
                                          v_Quality_Control_Doctor   varchar,
                                          v_Quality_Control_Nurse    varchar,
                                          v_Quality_Control_Date     varchar,
                                          v_Xay_Sn                   varchar,
                                          v_Ct_Sn                    varchar,
                                          v_Mri_Sn                   varchar,
                                          v_Dsa_Sn                   varchar,
                                          v_Is_First_Case            numeric,
                                          v_Is_Following             numeric,
                                          v_Following_Ending_Date    varchar,
                                          v_Is_Teaching_Case         numeric,
                                          v_Blood_Type_id            numeric,
                                          v_Rh                       numeric,
                                          v_Blood_Reaction_Id        numeric,
                                          v_Blood_Rbc                numeric,
                                          v_Blood_Plt                numeric,
                                          v_Blood_Plasma             numeric,
                                          v_Blood_Wb                 numeric,
                                          v_Blood_Others             varchar,
                                          v_Is_Completed             varchar,
                                          v_completed_time           varchar,
                                          --v_Valide numeric ,
                                          v_Create_User varchar,
                                          --v_Create_Time varchar(19)
                                          --v_Modified_User varchar(10) ,
                                          --v_Modified_Time varchar(19)
                                          o_result OUT empcurtyp);

  /*
  * 插入功病案首页诊断TABLE
  */
  PROCEDURE usp_Insert_Iem_Mainpage_Diag(v_Iem_Mainpage_NO   numeric,
                                         v_Diagnosis_Type_Id numeric,
                                         v_Diagnosis_Code    varchar,
                                         v_Diagnosis_Name    varchar,
                                         v_Status_Id         numeric,
                                         v_Order_Value       numeric,
                                         --v_Valide numeric ,
                                         v_Create_User varchar
                                         --v_Create_Time varchar(19) ,
                                         --v_Cancel_User varchar(10) ,
                                         --v_Cancel_Time varchar(19)
                                         );
  /*
  * 插入功病案首页手术TABLE
  */
  PROCEDURE usp_Insert_Iem_MainPage_Oper(v_IEM_MainPage_NO     numeric,
                                         v_Operation_Code      varchar,
                                         v_Operation_Date      varchar,
                                         v_Operation_Name      varchar,
                                         v_Execute_User1       varchar,
                                         v_Execute_User2       varchar,
                                         v_Execute_User3       varchar,
                                         v_Anaesthesia_Type_Id numeric,
                                         v_Close_Level         numeric,
                                         v_Anaesthesia_User    varchar,
                                         --v_Valide numeric ,
                                         v_Create_User varchar
                                         --v_Create_Time varchar(19)
                                         --v_Cancel_User varchar(10) ,
                                         --v_Cancel_Time varchar(19)
                                         );

  /*********************************************************************************/
  /*输出更新某个病人的当前诊断信息(wyt 2012-08-15)*/
  PROCEDURE usp_updatecurrentdiaginfo(v_patient_id   VARCHAR2,
                                      v_patient_name VARCHAR2,
                                      v_diag_code    VARCHAR2,
                                      v_diag_content varchar2);

  /*输出更新某个主诊医师病种查看权限信息(wyt 2012-08-13)*/
  PROCEDURE usp_updateattendphysicianinfo(v_id            VARCHAR2,
                                          v_name          VARCHAR2,
                                          v_py            VARCHAR2,
                                          v_wb            VARCHAR2,
                                          v_deptid        VARCHAR2,
                                          v_relatedisease varchar2);

  /*输出更新某个员工信息*/
  PROCEDURE usp_updateuserinfo(v_id           VARCHAR2,
                               v_name         VARCHAR2,
                               v_deptid       VARCHAR2,
                               v_wardid       VARCHAR2,
                               v_jobid        VARCHAR2,
                               v_deptorward   VARCHAR2,
                               v_power        varchar2,
                               v_Py           VARCHAR2,
                               v_Wb           VARCHAR2,
                               v_Sexy         VARCHAR2,
                               v_Birth        VARCHAR2,
                               v_Marital      VARCHAR2,
                               v_Idno         VARCHAR2,
                               v_Category     VARCHAR2,
                               v_Jobtitle     VARCHAR2,
                               v_Recipeid     VARCHAR2,
                               v_Recipemark   VARCHAR2,
                               v_Narcosismark VARCHAR2,
                               v_Grade        VARCHAR2,
                               v_Status       VARCHAR2,
                               v_Memo         varchar2);

  /*修改岗位信息*/
  PROCEDURE usp_updatejobinfo(v_id    VARCHAR2,
                              v_title VARCHAR2,
                              v_memo  VARCHAR2);

  /*更新病人护理文档*/
  procedure usp_UpdateNursRecordTable(v_ID        numeric, --记录ID
                                      v_NoOfInpat numeric, --首页序号(住院流水号)
                                      v_Content   blob --表单内容
                                      );

  /*更新护理文档模板*/
  procedure usp_UpdateNursTableTemplate(v_ID       numeric, --模板代码
                                        v_Name     varchar, --模板名称
                                        v_Describe varchar, --模板描述
                                        v_Content  blob, --模板内容
                                        v_version  varchar, --模板版本(自定义输入,默认为1.00.00,可以自增长最后1位)
                                        v_SortID   varchar --模板分类代码
                                        );

  /*更新到期的借阅病历*/
  PROCEDURE usp_updatedueapplyrecord(v_applydoctor VARCHAR2 --申请医师代码
                                     );

  /*更新功病案首页手术TABLE*/
  PROCEDURE usp_update_iem_mainpage_oper(v_iem_mainpage_no NUMERIC,
                                         v_cancel_user     VARCHAR);

  /*更新病案首页诊断TABLE,在插入之前调用*/
  PROCEDURE usp_update_iem_mainpage_diag(v_iem_mainpage_no NUMERIC,
                                         v_cancel_user     VARCHAR2);

  /*更新功病案首页基本信息TABLE*/
  PROCEDURE usp_update_iem_mainpage_basic(v_iem_mainpage_no          NUMERIC,
                                          v_patnoofhis               NUMERIC,
                                          v_noofinpat                NUMERIC,
                                          v_payid                    VARCHAR,
                                          v_socialcare               VARCHAR,
                                          v_incount                  INT,
                                          v_name                     VARCHAR,
                                          v_sexid                    VARCHAR,
                                          v_birth                    CHAR,
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
                                          v_days_before              NUMERIC,
                                          v_trans_date               VARCHAR,
                                          v_trans_admitdept          VARCHAR,
                                          v_trans_admitward          VARCHAR,
                                          v_trans_admitdept_again    VARCHAR,
                                          v_outwarddate              VARCHAR,
                                          v_outhosdept               VARCHAR,
                                          v_outhosward               VARCHAR,
                                          v_actual_days              NUMERIC,
                                          v_death_time               VARCHAR,
                                          v_death_reason             VARCHAR,
                                          v_admitinfo                VARCHAR,
                                          v_in_check_date            VARCHAR,
                                          v_pathology_diagnosis_name VARCHAR,
                                          v_pathology_observation_sn VARCHAR,
                                          v_ashes_diagnosis_name     VARCHAR,
                                          v_ashes_anatomise_sn       VARCHAR,
                                          v_allergic_drug            VARCHAR,
                                          v_hbsag                    NUMERIC,
                                          v_hcv_ab                   NUMERIC,
                                          v_hiv_ab                   NUMERIC,
                                          v_opd_ipd_id               NUMERIC,
                                          v_in_out_inpatinet_id      NUMERIC,
                                          v_before_after_or_id       NUMERIC,
                                          v_clinical_pathology_id    NUMERIC,
                                          v_pacs_pathology_id        NUMERIC,
                                          v_save_times               NUMERIC,
                                          v_success_times            NUMERIC,
                                          v_section_director         VARCHAR,
                                          v_director                 VARCHAR,
                                          v_vs_employee_code         VARCHAR,
                                          v_resident_employee_code   VARCHAR,
                                          v_refresh_employee_code    VARCHAR,
                                          v_master_interne           VARCHAR,
                                          v_interne                  VARCHAR,
                                          v_coding_user              VARCHAR,
                                          v_medical_quality_id       NUMERIC,
                                          v_quality_control_doctor   VARCHAR,
                                          v_quality_control_nurse    VARCHAR,
                                          v_quality_control_date     VARCHAR,
                                          v_xay_sn                   VARCHAR,
                                          v_ct_sn                    VARCHAR,
                                          v_mri_sn                   VARCHAR,
                                          v_dsa_sn                   VARCHAR,
                                          v_is_first_case            NUMERIC,
                                          v_is_following             NUMERIC,
                                          v_following_ending_date    VARCHAR,
                                          v_is_teaching_case         NUMERIC,
                                          v_blood_type_id            NUMERIC,
                                          v_rh                       NUMERIC,
                                          v_blood_reaction_id        NUMERIC,
                                          v_blood_rbc                NUMERIC,
                                          v_blood_plt                NUMERIC,
                                          v_blood_plasma             NUMERIC,
                                          v_blood_wb                 NUMERIC,
                                          v_blood_others             VARCHAR,
                                          v_is_completed             VARCHAR,
                                          v_completed_time           VARCHAR,
                                          v_modified_user            VARCHAR);

  /*更新病人就诊信息*/
  PROCEDURE usp_updatadiacrisisinfo(v_noofinpat      NUMERIC, --首页序号(住院流水号)(Inpatient.NoOfInpat)
                                    v_admitdiagnosis VARCHAR2 --入院诊断
                                    );

  /*更新病人基本信息*/
  PROCEDURE usp_updatabasepatientinfo(v_noofinpat     NUMERIC, --首页序号(住院流水号)
                                      v_birth         VARCHAR, --出生日期
                                      v_marital       VARCHAR, --婚姻状况
                                      v_nationid      VARCHAR, --民族代码
                                      v_nationalityid VARCHAR, --国籍代码
                                      v_sexid         VARCHAR, --病人性别
                                      v_edu           VARCHAR, --文化程度
                                      v_provinceid    VARCHAR, --(出生地)省市代码
                                      v_countyid      VARCHAR, --(出生地)区县代码
                                      v_nativeplace_p VARCHAR, --籍贯省市代码
                                      v_nativeplace_c VARCHAR, --籍贯区县代码
                                      v_payid         VARCHAR, --病人性质
                                      v_age           INT, --病人年龄
                                      v_religion      VARCHAR, --宗教信仰
                                      v_educ          NUMERIC, --(受)教育年限(单位:年)
                                      v_idno          VARCHAR, --身份证号
                                      v_jobid         VARCHAR, --职业代码
                                      v_organization  VARCHAR, --工作单位(暂缺)
                                      v_officeplace   VARCHAR, --单位地址
                                      v_officepost    VARCHAR, --单位邮编
                                      v_officetel     VARCHAR, --单位电话
                                      v_nativeaddress VARCHAR, --户口地址
                                      v_nativetel     VARCHAR, --户口电话
                                      v_nativepost    VARCHAR, --户口邮编
                                      v_address       VARCHAR --当前地址
                                      );

  procedure usp_SymbolManager(v_type               varchar,
                              v_SymbolCategroyID   int default 0, --特殊字符类型名称
                              v_SymbolCategoryName varchar default '', --特殊字符类型名称
                              v_SymbolCategoryMemo varchar default '', --特殊字符类型备注

                              --  v_SymbolRTF  varchar default '',--特殊字符类型ID(Inpatient.NoOfInpat)
                              --  v_SymbolCategroyID int  default 0,  --特殊字符编码
                              --  v_SymbolLength int  default  0,       --特殊字符长度
                              --  v_SymbolMemo varchar default  '' --特殊字符备注
                              o_result OUT empcurtyp);

  /*按部门输出员工信息*/
  PROCEDURE usp_selectusers(v_DeptId   VARCHAR,
                            v_UserName VARCHAR,
                            o_result   OUT empcurtyp);

  /*输出所有员工信息（编号显示其对应名称）*/
  procedure usp_selectuserinfo(o_result out empcurtyp);

  /*输出所有部门信息*/
  PROCEDURE usp_selectward(o_result OUT empcurtyp);

  /*输出所有岗位信息*/
  PROCEDURE usp_selectpermission(o_result OUT empcurtyp);

  /*输出某个员工信息*/
  PROCEDURE usp_selectjob(v_id VARCHAR, o_result OUT empcurtyp);

  /*输出所有部门信息*/
  PROCEDURE usp_selectdepartment(o_result OUT empcurtyp);

  /*输出所有员工信息*/
  PROCEDURE usp_selectallusers2(o_result  OUT empcurtyp,
                                o_result1 OUT empcurtyp);

  /*输出所有员工信息*/
  PROCEDURE usp_selectallusers(o_result OUT empcurtyp);

  /*输出所有岗位信息*/
  PROCEDURE usp_selectalljobs(o_result OUT empcurtyp);

  /*保存申请借阅电子病历信息*/
  PROCEDURE usp_saveapplyrecord(v_noofinpat   NUMERIC, --首页序号(住院流水号)
                                v_applydoctor VARCHAR, --申请医生代码
                                v_deptid      VARCHAR, --科室代码
                                v_applyaim    VARCHAR, --申请目的
                                v_duetime     NUMERIC, --申请期限
                                v_unit        VARCHAR --期限单位
                                );

  /*获取病人信息维护窗体控件初始化数据*/
  PROCEDURE usp_redactpatientinfofrm(v_frmtype VARCHAR,
                                     --窗体类型，1n：病人信息管理维护窗体、2n：病人病历史信息窗体
                                     v_noofinpat  NUMERIC DEFAULT '0', --首页序号
                                     v_categoryid VARCHAR DEFAULT '',
                                     --(字典)类别代码 、或父节点代码 、首页序号
                                     o_result OUT empcurtyp);

  /*获取质量控制统计数据(有查询条件)*/
  PROCEDURE usp_queryqcstatinfo(v_datetimebegin VARCHAR, --开始时间
                                v_datetimeend   VARCHAR, --结束时间
                                o_result        OUT empcurtyp);

  /*获取质量控制统计数据*/
  PROCEDURE usp_queryqcstatdetailinfo(v_datetimebegin VARCHAR, --开始时间
                                      v_datetimeend   VARCHAR, --结束时间
                                      v_qcstattype    INT, --统计资料类型
                                      o_result        OUT empcurtyp);

  /*获取问题登记数据*/
  PROCEDURE usp_queryqcprobleminfo(v_noofinpat INT, o_result OUT empcurtyp);

  /*获取质量控制病患数据*/
  PROCEDURE usp_queryqcpatientinfo(v_datetimefrom VARCHAR,
                                   v_datetimeto   VARCHAR,
                                   v_deptid       VARCHAR,
                                   v_wardid       VARCHAR,
                                   v_bedid        VARCHAR,
                                   v_name         VARCHAR,
                                   v_archives     VARCHAR,
                                   o_result       OUT empcurtyp);

  /*根据首页序号获取病人对应信息*/
  PROCEDURE usp_querypatientinfobynoofinp(v_noofinpat INT,
                                          o_result    OUT empcurtyp);

  /*获取病人提取界面病人列表*/
  PROCEDURE usp_queryownmanagerpat2(v_querytype INT,
                                    v_userid    VARCHAR,
                                    v_deptid    VARCHAR,
                                    v_wardid    VARCHAR,
                                    o_result    OUT empcurtyp);

  /*获取对应病区的床位信息*/
  PROCEDURE usp_querynonarchivepatients(v_wardid  VARCHAR,
                                        v_deptids VARCHAR,
                                        o_result  OUT empcurtyp);

  /*质控TypeScore操作*/
  PROCEDURE usp_qctypescore(v_typename        VARCHAR default '',
                            v_typeinstruction VARCHAR default '',
                            v_typecategory    INT default 0,
                            v_typeorder       INT default 0,
                            v_typememo        VARCHAR default '',
                            v_typestatus      INT,
                            v_typecode        VARCHAR);

  /*获取质量问题数据*/
  PROCEDURE usp_qcoperprobleminfo(v_id             INT,
                                  v_noofinpat      INT,
                                  v_category       INT,
                                  v_status         INT,
                                  v_typecode       VARCHAR,
                                  v_description    VARCHAR,
                                  v_ansewercontent VARCHAR,
                                  v_confirmcontent VARCHAR,
                                  v_problemdate    VARCHAR,
                                  v_registerdate   VARCHAR,
                                  v_registeruser   VARCHAR,
                                  v_answerdate     VARCHAR,
                                  v_answeruser     VARCHAR,
                                  v_confirmdate    VARCHAR,
                                  v_confirmuser    VARCHAR,
                                  v_deldate        VARCHAR,
                                  v_deluser        VARCHAR,
                                  v_operstatus     INT);

  /*获取质量控制统计数据*/
  PROCEDURE usp_qcitemscore(v_itemname           VARCHAR,
                            v_iteminstruction    VARCHAR,
                            v_itemdefaultscore   INT,
                            v_itemstandardscore  INT,
                            v_itemcategory       INT,
                            v_itemdefaulttarget  INT,
                            v_itemtargetstandard INT,
                            v_itemscorestandard  INT,
                            v_itemorder          INT,
                            v_typecode           VARCHAR,
                            v_itemmemo           VARCHAR,
                            v_typestatus         INT,
                            v_itemcode           VARCHAR);

  /*获取病历模板选择助手数据*/
  PROCEDURE usp_msquerytemplate(v_id         VARCHAR,
                                v_user       VARCHAR,
                                v_type       INT,
                                v_department VARCHAR DEFAULT '',
                                o_result     OUT empcurtyp);

  /*
  * 更改用户密码
  */
  PROCEDURE usp_updateuserpassword(v_id      IN users.ID%TYPE,
                                   v_passwd  IN users.passwd%TYPE,
                                   v_regdate IN users.regdate%TYPE);

  /*
  * 得到用户帐户信息
  */
  PROCEDURE usp_getuseraccount(v_id     IN users.ID%TYPE,
                               o_result OUT empcurtyp);

  /*
  * 取职工科室对应设置, 若未指定病区，则通过指定的科室关联出所有的病区
  */
  PROCEDURE usp_getuserdeptandward(v_userid IN users.ID%TYPE,
                                   o_result OUT empcurtyp);

  /*
  *
  */
  PROCEDURE usp_getuseroutdeptandward(o_result OUT empcurtyp);

  /**使用到临时表的存储过程**/

  /*
  *获取职工相关信息
  */
  PROCEDURE usp_GetUserInfo(v_userid  varchar,
                            o_result  OUT empcurtyp,
                            o_result1 OUT empcurtyp);

  /*
  *获取职工相关信息
  */
  procedure usp_NursRecordOperate(v_ID        numeric default 0, --记录ID
                                  v_NoOfInpat numeric default 0, --首页序号(住院流水号)
                                  v_SortID    numeric default 0, --模板分类代码
                                  v_Type      int, --操作类型
                                  o_result    OUT empcurtyp);
  /*
  *医疗质量统计分析
  */
  PROCEDURE usp_MedQCAnalysis(v_DateTimeBegin varchar, --查询开始日期
                              v_DateTimeEnd   varchar, --查询结束日期
                              o_result        OUT empcurtyp);

  /*
  *获取对应病区的床位信息(病区一览时调用）
  */
  PROCEDURE usp_QueryBrowserInwardPatients(v_ID        varchar,
                                           v_QueryType int,
                                           v_wardid    varchar,
                                           v_Deptids   varchar,
                                           v_zyhm      varchar default '',
                                           v_hzxm      varchar default '',
                                           v_cyzd      varchar default '',
                                           v_ryrqbegin varchar default '',
                                           v_ryrqend   varchar default '',
                                           v_cyrqbegin varchar default '',
                                           v_cyrqend   varchar default '',
                                           o_result    OUT empcurtyp);
  /*
  *获取对应病区的床位信息
  */
  PROCEDURE usp_QueryHistoryPatients(v_wardid          varchar,
                                     v_deptids         varchar,
                                     v_PatID           varchar default '',
                                     v_PatName         varchar default '',
                                     v_OutDiagnosis    varchar default '',
                                     v_AdmitDatebegin  varchar default '',
                                     v_AdmitDatend     varchar default '',
                                     v_OutHosDatebegin varchar default '',
                                     v_OutHosDatend    varchar default '',
                                     o_result          OUT empcurtyp,
                                     o_result1         OUT empcurtyp);

  /*
  *获取对应病区的床位信息
  */
  PROCEDURE usp_QueryInwardPatients_old(v_wardid    varchar,
                                        v_Deptids   varchar,
                                        v_zyhm      varchar default '',
                                        v_hzxm      varchar default '',
                                        v_cyzd      varchar default '',
                                        v_ryrqbegin varchar default '',
                                        v_ryrqend   varchar default '',
                                        v_cyrqbegin varchar default '',
                                        v_cyrqend   varchar default '',
                                        --add by xjt
                                        v_ID        varchar default '',
                                        v_QueryType int default 0,
                                        v_QueryNur  varchar default '',
                                        v_QueryBed  varchar default 'Y',
                                        o_result    OUT empcurtyp);

  /*
    获取对应病区的床位信息
  bwj 20121108 解决usp_QueryInwardPatients_old中医生工作站中用临时表出资源竞争悲观锁
  通过视图
  */
  PROCEDURE usp_queryinwardpatients(v_wardid    VARCHAR,
                                    v_deptids   VARCHAR,
                                    v_zyhm      VARCHAR DEFAULT '',
                                    v_hzxm      VARCHAR DEFAULT '',
                                    v_cyzd      VARCHAR DEFAULT '',
                                    v_ryrqbegin VARCHAR DEFAULT '',
                                    v_ryrqend   VARCHAR DEFAULT '',
                                    v_cyrqbegin VARCHAR DEFAULT '',
                                    v_cyrqend   VARCHAR DEFAULT '',
                                    --add by xjt
                                    v_id        VARCHAR DEFAULT '',
                                    v_querytype INT DEFAULT 0,
                                    v_querynur  VARCHAR DEFAULT '',
                                    v_querybed  VARCHAR DEFAULT 'Y',
                                    o_result    OUT empcurtyp);

  /*
  xuliangliang2012-12-17从青龙上库导出
  解决青龙山科室病区代码问题 用like语句
  */
  PROCEDURE usp_queryinwardpatientsQLS(v_wardid    VARCHAR,
                                       v_deptids   VARCHAR,
                                       v_zyhm      VARCHAR DEFAULT '',
                                       v_hzxm      VARCHAR DEFAULT '',
                                       v_cyzd      VARCHAR DEFAULT '',
                                       v_ryrqbegin VARCHAR DEFAULT '',
                                       v_ryrqend   VARCHAR DEFAULT '',
                                       v_cyrqbegin VARCHAR DEFAULT '',
                                       v_cyrqend   VARCHAR DEFAULT '',
                                       --add by xjt
                                       v_id        VARCHAR DEFAULT '',
                                       v_querytype INT DEFAULT 0,
                                       v_querynur  VARCHAR DEFAULT '',
                                       v_querybed  VARCHAR DEFAULT 'Y',
                                       o_result    OUT empcurtyp);

  /*
  *获取对应病区的床位信息
  */
  PROCEDURE usp_QueryInwardPatients2(v_wardid    varchar,
                                     v_Deptids   varchar,
                                     v_zyhm      varchar default '',
                                     v_hzxm      varchar default '',
                                     v_cyzd      varchar default '',
                                     v_ryrqbegin varchar default '',
                                     v_ryrqend   varchar default '',
                                     v_cyrqbegin varchar default '',
                                     v_cyrqend   varchar default '',
                                     --add by xjt
                                     v_ID        varchar default '',
                                     v_QueryType int default 0,
                                     v_QueryNur  varchar default '',
                                     v_QueryBed  varchar default 'Y',
                                     o_result    OUT empcurtyp);

  /*
  *获取病人提取界面病人列表
  */
  PROCEDURE usp_QueryOwnManagerPat(v_QueryType int,
                                   v_UserID    varchar,
                                   v_DeptID    varchar,
                                   v_WardId    varchar,
                                   o_result    OUT empcurtyp);
  /*
  *获取质量评分
  */
  PROCEDURE usp_QueryQCGrade(v_NoOfInpat int,
                             v_OperUser  varchar,
                             o_result    OUT empcurtyp);

  /*
  *获取对应病区的出院病患
  */
  PROCEDURE usp_QueryQuitPatientNoDoctor(v_wardid    varchar,
                                         v_Deptids   varchar,
                                         v_TimeFrom  varchar,
                                         v_TimeTo    varchar,
                                         v_PatientSN varchar default '', --病历号
                                         v_Name      varchar default '', --病人名称
                                         v_QueryType int default 0,
                                         o_result    OUT empcurtyp);

  /***************************************************************************/

  /*
  *计算病人年龄
  */
  PROCEDURE usp_Emr_CalcAge(v_csrq varchar,
                            v_dqrq varchar,
                            v_sjnl out int,
                            v_xsnl out varchar);
  /*
  *计算病人年龄
  */
  procedure usp_EMR_GetAge(v_csrq varchar,
                           v_dqrq varchar,
                           v_sjnl out int,
                           v_xsnl out varchar);

  --医师工作站得到出院病人
  PROCEDURE usp_queryinwardpatientsout(v_wardid    VARCHAR,
                                       v_deptids   VARCHAR,
                                       v_id        VARCHAR DEFAULT '',
                                       v_querytype INT DEFAULT 0,
                                       o_result    OUT empcurtyp);

  ----医生工作站中获得会诊信息（全部符合条件的）
  PROCEDURE usp_GetMyConsultion(v_Deptids varchar, --部门编号
                                V_userid  varchar, --登录人编号
                                o_result  OUT empcurtyp);

  -----------------------------为病人设定婴儿的存储过程  -add by ywk 2012年6月7日 09:47:34------------------------------
  /*更新功病案首页基本信息TABLE*/
  PROCEDURE usp_editBabyinfo(v_Noofinpat  NUMERIC,
                             v_patnoofhis VARCHAR,
                             v_Noofclinic VARCHAR,
                             v_Noofrecord VARCHAR,
                             v_patid      VARCHAR,
                             v_Innerpix   VARCHAR,
                             v_outpix     VARCHAR,

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
                             v_XZZPost         VARCHAR,
                             v_EditType        varchar);
  ----质控出院未归档病历查询
  -----Add by xlb 2013-06-04
  PROCEDURE usp_GetOutHosButNotLocks(v_deptId    VARCHAR2, ---科室代码
                                     v_sex       VARCHAR2, --性别
                                     v_dateBegin VARCHAR2, --出院起始时间
                                     v_dateEnd   VARCHAR2, --出院结束时间
                                     v_patName   VARCHAR2, ----病人姓名
                                     v_PatId     VARCHAR2,
                                     o_result    OUT empcurtyp);
END;