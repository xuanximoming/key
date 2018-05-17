create or replace package MainPageExtenSion is

  -- Author  : xlb
  -- Created : 2013/4/10 16:51:22
  -- Purpose : 病案首页扩展功能包
  TYPE empcurtype IS REF CURSOR;

-------抓取扩展维护记录---------------------------------------------------------------------------------------
-------------Add by xlb 2013-04-10----------------------------------------------------------------------------
PROCEDURE GETIEMEXCEPT(
                       o_result out empcurtype
                      );
                      
----新增或修改维护记录  Add by xlb 2013-04-11                      
PROCEDURE ADDORMODIFYIEMEXCEPT
                            (
                            v_iemexId         VARCHAR2,
                            v_iemexName       VARCHAR2,
                            v_dateElementFlow VARCHAR2,
                            v_iemControl      VARCHAR2,
                            v_iemOtherName    VARCHAR2,
                            v_orderCode       VARCHAR2,
                            v_isOtherLine     VARCHAR2,
                            v_valide          VARCHAR2,
                            v_createDocId     VARCHAR2,
                            v_createDateTime  CHAR,
                            v_modifyDocId     VARCHAR2,
                            v_modifyDateTime  VARCHAR2
           
                            ); 
 ---------------------------------新增或修改病案扩展维护功能编辑界面病人使用记录Add by xlb 2013-04-16--------                           
Procedure ADDORMODIFYIEMUS
                         (
                          v_iemExUserId   VARCHAR2,
                          v_iemExId       VARCHAR2,
                          v_nOofInpat     VARCHAR2,
                          v_value         VARCHAR2,
                          v_createDocId   VARCHAR2,
                          v_modifyDocId   VARCHAR2,
                          v_modifyDateTime CHAR
                          );                                                 
end;
/
create or replace package body MainPageExtenSion is

---抓取病案首页扩展维护记录 Add by xlb 2013-04-10
PROCEDURE GETIEMEXCEPT(
                       o_result out empcurtype
                      ) as
Begin
open o_result for
select iemexid,iemexname,dateelementflow,
IEMCONTROL,iemothername,ordercode,isotherline,
valide,createdocid,createdatetime,MODIFYDOCID,
MODIFYDATETIME from iem_mainpage_except
 where valide='1' order by to_number(iem_mainpage_except.ordercode);
END;

PROCEDURE ADDORMODIFYIEMEXCEPT
                            (
                            v_iemexId         VARCHAR2,
                            v_iemexName       VARCHAR2,
                            v_dateElementFlow VARCHAR2,
                            v_iemControl      VARCHAR2,
                            v_iemOtherName    VARCHAR2,
                            v_orderCode       VARCHAR2,
                            v_isOtherLine     VARCHAR2,
                            v_valide          VARCHAR2,
                            v_createDocId     VARCHAR2,
                            v_createDateTime  CHAR,
                            v_modifyDocId     VARCHAR2,
                            v_modifyDateTime  VARCHAR2
           
                            ) AS
                            

v_count integer;

BEGIN
  select count(*) into v_count from iem_mainpage_except e where e.iemexid=v_iemexId;
    if v_count <=0
    then
    insert into iem_mainpage_except (iemexid,
                                    iemexname,
                                    dateelementflow,
                                    iemcontrol,
                                    iemothername,
                                    ordercode,
                                    isotherline,
                                    valide,
                                    createdocid,
                                    createdatetime,
                                    modifydocid,
                                    modifydatetime
                                    )  values
                                     (
                                        v_iemexId,        
                                        v_iemexName,       
                                        v_dateElementFlow, 
                                        v_iemControl,     
                                        v_iemOtherName,   
                                        v_orderCode,       
                                        v_isOtherLine,    
                                        v_valide,          
                                        v_createDocId,    
                                        v_createDateTime,  
                                        v_modifyDocId,     
                                        v_modifyDateTime
                                        );
else
  update iem_mainpage_except e set e.iemexname=v_iemexName,
  dateelementflow=v_dateElementFlow,iemcontrol=v_iemControl,
  ordercode=v_orderCode,isotherline=v_isOtherLine,valide=v_valide,
  iemothername=v_iemOtherName,
  createdocid=v_createDocId,createdatetime=v_createDateTime,
  modifydocid=v_modifyDocId,modifydatetime=v_modifyDateTime
  
   where e.iemexid=v_iemexId;
  
END IF;

END;
----------------------------------新增或修改病人使用记录  病案首页扩展功能  Add by xlb 2013-04-16
Procedure ADDORMODIFYIEMUS
                         (
                          v_iemExUserId   VARCHAR2,
                          v_iemExId       VARCHAR2,
                          v_nOofInpat     VARCHAR2,
                          v_value         VARCHAR2,
                          v_createDocId   VARCHAR2,
                          v_modifyDocId   VARCHAR2,
                          v_modifyDateTime CHAR
                          ) AS
  v_count integer;
 begin                         
 select count(*) into v_count from iem_mainpage_except_use where IEMEXUSEID=v_iemExUserId;
 if v_count <=0
 then
 insert into iem_mainpage_except_use
                                   (   IEMEXUSEID,
                                        iemexid,
                                      noofinpat,
                                          value,
                                    createdocid,
                                 createdatetime,
                                    modifydocid,
                                  modifydatetime) 
                                          values
                                          (
                                          v_iemExUserId,
                                          v_iemExId,
                                          v_nOofInpat,
                                          v_value,
                                          v_createDocId,
                                          to_char(sysdate,'yyyy-mm-dd hh24;mi;ss'),
                                          null,
                                          null
                                          ); 
else update iem_mainpage_except_use set iemexid=v_iemExId,
value=v_value,modifydocid=v_modifyDocId,modifydatetime=v_modifyDateTime where IEMEXUSEID=v_iemExUserId;

end if;                                          
end;

end;----package end;
/
