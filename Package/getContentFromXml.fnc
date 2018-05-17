CREATE OR REPLACE FUNCTION getContentFromXml(v_recordDetailID varchar2,
                                             v_roElementName  varchar2,
                                             v_flag           integer)
/*
     1: 换行的数据也一并抓取，直到发现下一个RoElement元素
     2：数据在当前行中抓取，直到发现下一个RoElement元素
     3：只抓取当前行中RoElemenet元素之后的数据
  */

 return nvarchar2 IS
  v_result nvarchar2(4000);

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
  selementValue      nvarchar2(2000);

  xmlClobData clob;

  --进入逻辑判断的标志位
  isEnter integer := 0;

  --节点属性集合
  nodeAttributes xmldom.DOMNamedNodeMap;

begin
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
              IF (xmldom.getNodeName(roElementNode) = 'roelement') AND
                 v_flag != 3 THEN
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
                      /*
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
                      */
                    
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
                    selementValue := xmldom.getNodeValue(xmldom.getFirstChild(roElementNode));
                    IF instr(selementValue, '医师签名') > 0 or
                       instr(selementValue, '医生签名') > 0 THEN
                      --遇到医师签名则退出
                      BEGIN
                        isEnter := 2;
                        EXIT;
                      END;
                    ELSE
                      v_result := v_result || selementValue;
                    END IF;
                  END IF;
                END;
              END IF;
            END;
          END IF;
        END;
        --********段落中的元素 END********
      END LOOP;
    
      IF isEnter = 1 THEN
        --发现指定的RoElement元素
        BEGIN
          IF v_flag = 2 OR v_flag = 3 THEN
            --只判断当前段落
            BEGIN
              isEnter := 2;
              EXIT;
            END;
          END IF;
        END;
      END IF;
    
    END;
    --********循环段落 END********
  END LOOP;
  v_result := ltrim(ltrim(v_result, '：'), ':');
  v_result := rtrim(ltrim(v_result));
  return v_result;

EXCEPTION
  WHEN OTHERS THEN
    return v_result;
END;
/
