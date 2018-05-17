CREATE OR REPLACE PACKAGE EMR_VIEW_TOOL IS
  TYPE empcurtype IS REF CURSOR;

  --**********************************历次出院病历查阅**************************************
  --获取病人基本信息
  PROCEDURE usp_inpatient_info(v_patnoofhis VARCHAR2,
                               o_result     OUT empcurtype);

  --获取历史病人
  PROCEDURE usp_history_inpatient(v_patnoofhis VARCHAR2,
                                  o_result     OUT empcurtype);

  --获取病人病历
  PROCEDURE usp_inpatient_emr(v_noofinpat VARCHAR2,
                              o_result    OUT empcurtype);

  --获取病人病历
  PROCEDURE usp_inpatient_emr2(v_id VARCHAR2, o_result OUT empcurtype);

  --**********************************门急诊历史病历查阅**************************************

  --获取病人基本信息【门诊】
  PROCEDURE usp_inpatient_info_clinic(v_patnoofhis VARCHAR2,
                                      o_result     OUT empcurtype);

  --获取历史病人【门诊】
  PROCEDURE usp_history_inpatient_clinic(v_patnoofhis VARCHAR2,
                                         o_result     OUT empcurtype);

  --获取病人病历【门诊】
  PROCEDURE usp_inpatient_emr_clinic(v_noofinpat VARCHAR2,
                                     o_result    OUT empcurtype);

  --获取病人病历【门诊】
  PROCEDURE usp_inpatient_emr2_clinic(v_id     VARCHAR2,
                                      o_result OUT empcurtype);

  --获取门诊电子病历的宏元素的值
  PROCEDURE usp_macro_inpatient_clinic(v_noofinpat VARCHAR2,
                                       o_result    OUT empcurtype);

  --********************************病人信息维护******************************************
  --获取门诊电子病历的宏元素的值
  PROCEDURE usp_get_inpatient_list(v_deptID         VARCHAR2,
                                   v_visitTypeID    VARCHAR2,
                                   v_name           VARCHAR2,
                                   v_patID          VARCHAR2,
                                   v_visitTimeBegin date,
                                   v_visitTimeEnd   date,
                                   o_result         OUT empcurtype);

  --通过病历号或病人姓名抓取病人
  PROCEDURE usp_get_inpatient_list2(v_patID  VARCHAR2,
                                    v_name   VARCHAR2,
                                    o_result OUT empcurtype);

  --获取门诊科室列表                                   
  PROCEDURE usp_get_dept_list(o_result OUT empcurtype);

  --********************************测试，用于解析XML*************************************
  PROCEDURE usp_xml_content(v_recordDetailID varchar2,
                            v_roElementName  varchar2,
                            o_result         OUT empcurtype);

END; -- Package spec                                             -- Package spec
/
CREATE OR REPLACE PACKAGE BODY EMR_VIEW_TOOL IS
  --**********************************历次出院病历查阅**************************************
  --获取病人基本信息
  PROCEDURE usp_inpatient_info(v_patnoofhis VARCHAR2,
                               o_result     OUT empcurtype) AS
  BEGIN
    OPEN o_result FOR
      SELECT i.name, decode(i.sexid, 1, '男', 2, '女', '未知') sex, i.birth
        FROM inpatient i
       WHERE i.patnoofhis = v_patnoofhis;
  END;

  --获取历史病人
  PROCEDURE usp_history_inpatient(v_patnoofhis VARCHAR2,
                                  o_result     OUT empcurtype) AS
  BEGIN
    OPEN o_result FOR
      SELECT i1.NAME inpatientname, i1.noofinpat, i1.inwarddate
        FROM inpatient i1
       WHERE EXISTS (SELECT 1
                FROM inpatient i2
               WHERE i2.patnoofhis = v_patnoofhis
                 AND i2.noofclinic = i1.noofclinic)
       ORDER BY i1.inwarddate DESC;
  END;

  --获取病人病历
  PROCEDURE usp_inpatient_emr(v_noofinpat VARCHAR2,
                              o_result    OUT empcurtype) AS
  BEGIN
    OPEN o_result FOR
      SELECT r.NAME, r.ID
        FROM recorddetail r
       WHERE r.noofinpat = v_noofinpat
         AND r.valid = 1
         AND (r.name like '%出院%');
  END;

  --获取病人病历
  PROCEDURE usp_inpatient_emr2(v_id VARCHAR2, o_result OUT empcurtype) AS
  BEGIN
    OPEN o_result FOR
      SELECT r.content
        FROM recorddetail r
       WHERE r.ID = v_id
         AND r.valid = 1;
  END;

  --**********************************门急诊历史病历查阅**************************************

  --获取病人基本信息【门诊】
  PROCEDURE usp_inpatient_info_clinic(v_patnoofhis VARCHAR2,
                                      o_result     OUT empcurtype) AS
  BEGIN
    OPEN o_result FOR
      SELECT i.name, decode(i.sexid, 1, '男', 2, '女', '未知') sex, i.birth
        FROM inpatient_clinic i
       WHERE i.patnoofhis = v_patnoofhis;
  END;

  --获取历史病人【门诊】
  PROCEDURE usp_history_inpatient_clinic(v_patnoofhis VARCHAR2,
                                         o_result     OUT empcurtype) AS
  BEGIN
    OPEN o_result FOR
      SELECT i1.NAME            inpatientname,
             i1.noofinpatclinic noofinpat,
             i1.visittime       inwarddate
        FROM inpatient_clinic i1
       WHERE EXISTS (SELECT 1
                FROM inpatient_clinic i2
               WHERE i2.patnoofhis = v_patnoofhis
                 AND i2.patid = i1.patid)
         AND i1.patnoofhis != v_patnoofhis
         AND i1.valid = '1'
       ORDER BY i1.visittime DESC;
  END;

  --获取病人病历【门诊】
  PROCEDURE usp_inpatient_emr_clinic(v_noofinpat VARCHAR2,
                                     o_result    OUT empcurtype) AS
  BEGIN
    OPEN o_result FOR
      SELECT r.NAME, r.ID
        FROM recorddetail_clinic r
       WHERE r.noofinpatclinic = v_noofinpat
         AND r.valid = 1
       ORDER BY r.id;
  END;

  --获取病人病历【门诊】
  PROCEDURE usp_inpatient_emr2_clinic(v_id     VARCHAR2,
                                      o_result OUT empcurtype) AS
  BEGIN
    OPEN o_result FOR
      SELECT r.content
        FROM recorddetail_clinic r
       WHERE r.ID = v_id
         AND r.valid = 1;
  END;

  --获取门诊电子病历的宏元素的值
  PROCEDURE usp_macro_inpatient_clinic(v_noofinpat VARCHAR2,
                                       o_result    OUT empcurtype) AS
  BEGIN
    OPEN o_result FOR
      select ic.name           PatName,
             dd1.name          PatSexName,
             ic.patid          PatID,
             ic.age            PatAge,
             ic.bedcode        PatBedCode,
             dd2.name          PatMaritalName,
             ic.country        PatCountryName,
             ic.nationality    PatNationalityName,
             ic.jobname        PatJobName,
             ic.contactaddress PatContactAddress
        from inpatient_clinic ic
        left outer join dictionary_detail dd1
          on dd1.categoryid = '3'
         and dd1.detailid = ic.sexid
         and dd1.valid = '1'
        left outer join dictionary_detail dd2
          on dd2.categoryid = '4'
         and dd2.detailid = ic.maritalid
         and dd2.valid = '1'
       where ic.noofinpatclinic = v_noofinpat
         and ic.valid = '1';
  END;

  --********************************病人信息维护******************************************
  --获取门诊电子病历的宏元素的值
  PROCEDURE usp_get_inpatient_list(v_deptID         VARCHAR2,
                                   v_visitTypeID    VARCHAR2,
                                   v_name           VARCHAR2,
                                   v_patID          VARCHAR2,
                                   v_visitTimeBegin date,
                                   v_visitTimeEnd   date,
                                   o_result         OUT empcurtype) AS
  BEGIN
    OPEN o_result FOR
      select ic.noofinpatclinic,
             ic.visittype visittypeid,
             dd1.name visittypename,
             trunc(ic.visittime) visittime,
             ic.visitno,
             ic.roomname,
             ic.name,
             ic.patid,
             dd2.name sexname,
             ic.birth,
             ic.age,
             dd3.name maritalname,
             d.name deptname,
             ic.nationality,
             ic.country,
             ic.healthcardid,
             ic.contactaddress,
             ic.jobname,
             ic.visitdoctorid || '_' || u.name visitdoctorname,
             ic.bedcode
        from inpatient_clinic ic
        left outer join dictionary_detail dd1
          on dd1.categoryid = '13'
         and dd1.detailid = ic.visittype
        left outer join dictionary_detail dd2
          on dd2.categoryid = '3'
         and dd2.detailid = ic.sexid
        left outer join dictionary_detail dd3
          on dd3.categoryid = '4'
         and dd3.detailid = ic.maritalid
        left outer join users u
          on u.id = ic.visitdoctorid
        left outer join department d
          on d.id = ic.deptid
       where ic.deptid = v_deptID
         and ic.visittype = v_visitTypeID
         and ic.name like '%' || v_name || '%'
         and ic.patid like '%' || v_patID || '%'
         and trunc(ic.visittime) >= trunc(v_visitTimeBegin)
         and trunc(ic.visittime) <= trunc(v_visitTimeEnd)
         and ic.valid = '1'
       order by trunc(ic.visittime), ic.visitno desc;
  END;

  --通过病历号或病人姓名抓取病人
  PROCEDURE usp_get_inpatient_list2(v_patID  VARCHAR2,
                                    v_name   VARCHAR2,
                                    o_result OUT empcurtype) AS
  BEGIN
    OPEN o_result FOR
      select ic.noofinpatclinic,
             ic.visittype visittypeid,
             dd1.name visittypename,
             trunc(ic.visittime) visittime,
             ic.visitno,
             ic.roomname,
             ic.name,
             ic.patid,
             dd2.name sexname,
             ic.birth,
             ic.age,
             dd3.name maritalname,
             d.name deptname,
             ic.nationality,
             ic.country,
             ic.healthcardid,
             ic.contactaddress,
             ic.jobname,
             ic.visitdoctorid || '_' || u.name visitdoctorname,
             ic.bedcode
        from inpatient_clinic ic
        left outer join dictionary_detail dd1
          on dd1.categoryid = '13'
         and dd1.detailid = ic.visittype
        left outer join dictionary_detail dd2
          on dd2.categoryid = '3'
         and dd2.detailid = ic.sexid
        left outer join dictionary_detail dd3
          on dd3.categoryid = '4'
         and dd3.detailid = ic.maritalid
        left outer join users u
          on u.id = ic.visitdoctorid
        left outer join department d
          on d.id = ic.deptid
       where ((v_patID is not null and ic.patid = v_patID) or
             (v_name is not null and ic.name = v_name))
         and ic.valid = '1';
  END;

  --获取门诊科室列表                                   
  PROCEDURE usp_get_dept_list(o_result OUT empcurtype) AS
  BEGIN
    OPEN o_result FOR
      select d.id, d.name, d.py, d.wb
        from department d
       where d.sort not in ('102', '103', '104')
         and d.valid = '1'
       order by d.id;
  END;

  --********************************测试，用于解析XML*************************************  
  PROCEDURE usp_xml_content(v_recordDetailID varchar2,
                            v_roElementName  varchar2,
                            o_result         OUT empcurtype) AS
    --创建XML解析器实例XMLPARSER.parser
    xmlPar XMLPARSER.parser := XMLPARSER.NEWPARSER;
    --定义DOM文档对象
    doc xmldom.DOMDocument;
  
    --roElement元素
    roElementNode xmldom.DOMNode;
  
    --p元素集合
    paragraphElementNodes xmldom.DOMNodeList;
    --p元素集合的数量
    paragraphElementCount integer;
    --p元素
    paragraphElementNode xmldom.DOMNode;
  
    --p元素的所有子节点集合
    childElementNodes xmldom.DOMNodeList;
    --p元素的所有子节点个数
    childElementNodesCount integer;
  
    --selement元素集合
    selementNodes xmldom.DOMNodeList;
    --selement元素集合个数
    selementNodesCount integer;
    selementNode       xmldom.DOMNode;
    selementValue      nvarchar2(200);
  
    xmlClobData clob;
    v_result    nvarchar2(4000);
  
    --进入逻辑判断的标志位
    isEnter integer := 0;
  
    --节点属性集合
    nodeAttributes xmldom.DOMNamedNodeMap;
  BEGIN
  
    --获取xml数据，clob字段中获取病历内容
    select content
      into xmlClobData
      from recorddetail
     where id = v_recordDetailID;
  
    --解析xml数据
    xmlparser.parseClob(xmlPar, xmlClobData);
    doc := xmlparser.getDocument(xmlPar);
  
    --释放解析器实例
    xmlparser.freeParser(xmlPar);
  
    --获取所有P元素
    paragraphElementNodes := xmldom.getElementsByTagName(doc, 'p');
    paragraphElementCount := xmldom.getLength(paragraphElementNodes);
  
    --循环段落 
    For paragraphIndex in 0 .. paragraphElementCount - 1 LOOP
      --********循环段落 BEGIN********
      BEGIN
        --获取当前段落
        paragraphElementNode := xmldom.item(paragraphElementNodes,
                                            paragraphIndex);
        --获取段落中所有元素
        childElementNodes      := xmldom.getChildNodes(paragraphElementNode);
        childElementNodesCount := xmldom.getLength(childElementNodes);
        FOR childElementNodesIndex in 0 .. childElementNodesCount - 1 LOOP
          --********段落中的元素 BEGIN********
          BEGIN
            roElementNode := xmldom.item(childElementNodes,
                                         childElementNodesIndex);
          
            IF isEnter = 0 THEN
              --找到起始RoElement元素
              BEGIN
                --获取段落中的roElement元素
                IF (xmldom.getNodeName(roElementNode) = 'roelement') THEN
                  BEGIN
                    nodeAttributes := xmldom.getAttributes(roElementNode);
                  
                    --获取name == v_roleElementName 的roELement元素
                    IF (xmldom.getNodeValue(xmldom.getNamedItem(nodeAttributes,
                                                                'name')) =
                       v_roElementName) THEN
                      BEGIN
                        isEnter := 1;
                      END;
                    END IF;
                  END;
                END IF;
              END;
            ELSIF isEnter = 1 THEN
              BEGIN
                --判断是否遇到后面的roElement，如果遇到需要退出循环
                IF (xmldom.getNodeName(roElementNode) = 'roelement') THEN
                  BEGIN
                    isEnter := 2;
                    EXIT;
                  END;
                ELSE
                  BEGIN
                    IF (xmldom.getNodeName(roElementNode) = 'selement') THEN
                      --如果是"多选框"需要做特殊处理
                      BEGIN
                        nodeAttributes := xmldom.getAttributes(roElementNode);
                        selementValue  := xmldom.getNodeValue(xmldom.getNamedItem(nodeAttributes,
                                                                                  'text')); --获取selement元素的text属性的值
                      
                        selementNodes      := xmldom.getChildNodes(roElementNode);
                        selementNodesCount := xmldom.getLength(selementNodes);
                      
                        FOR selementNodesIndex in 0 .. selementNodesCount - 1 LOOP
                          --循环selement元素的所有子元素item
                          selementNode   := xmldom.item(selementNodes,
                                                        selementNodesIndex);
                          nodeAttributes := xmldom.getAttributes(selementNode);
                          IF xmldom.getNodeValue(xmldom.getNamedItem(nodeAttributes,
                                                                     'selected')) =
                             'true' THEN
                            --找出item中属性selected为true的节点
                            selementValue := xmldom.getNodeValue(xmldom.getFirstChild(selementNode));
                          END IF;
                        END LOOP;
                      
                        v_result := v_result || selementValue;
                      END;
                    ELSIF (xmldom.getNodeName(roElementNode) = 'btnelement') THEN
                      --如果是“按钮”需要做特殊处理
                      BEGIN
                        nodeAttributes := xmldom.getAttributes(roElementNode);
                        selementValue  := xmldom.getNodeValue(xmldom.getNamedItem(nodeAttributes,
                                                                                  'print')); --获取btnelement元素的print属性的值
                        IF selementValue != 'False' THEN
                          BEGIN
                            v_result := v_result ||
                                        xmldom.getNodeValue(xmldom.getFirstChild(roElementNode));
                          END;
                        END IF;
                      END;
                    ELSE
                      v_result := v_result ||
                                  xmldom.getNodeValue(xmldom.getFirstChild(roElementNode));
                    END IF;
                  END;
                END IF;
              END;
            END IF;
          END;
          --********段落中的元素 END********
        END LOOP;
      END;
      --********循环段落 END********
    END LOOP;
  
    OPEN o_result FOR
      select ltrim(ltrim(v_result, '：'), ':') as content from dual;
  END;

END;
/
