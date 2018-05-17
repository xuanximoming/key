create or replace package EMRORDER is

  -- Author  : Xianglingbo
  -- Created : 2013/4/8 16:21:43
  -- Purpose : 医嘱浏览供测试盒模拟用
  
  TYPE empcurtype IS REF CURSOR;
  
  
    PROCEDURE  GETORDER(

                      v_InpatId VARCHAR2,
                      v_DeptId  VARCHAR2,
                      v_Type    VARCHAR2,
                      v_BeginDate VARCHAR2,
                      v_EndDate   VARCHAR2,
                      o_result   OUT empcurtype);
                      
  PROCEDURE  GETORDER2(

                      v_InpatId VARCHAR2,
                      v_DeptId  VARCHAR2 default '',
                      v_Type    VARCHAR2,
                      v_BeginDate VARCHAR2 default '',
                      v_EndDate   VARCHAR2 default '',
                      o_result   OUT empcurtype);
                      
 -- PROCEDURE  proc_GetAllIemMainPageExcept (o_result out empcurtype);
end ;
/
create or replace package body EMRORDER is

  ------------------------------------------------------------------医嘱浏览存储过程 用于测试和展示 Add by xlb 2013-04-08------------------------------------------------------------
  PROCEDURE  GETORDER(
                      v_InpatId VARCHAR2,
                      v_DeptId  VARCHAR2,
                      v_Type    VARCHAR2,
                      v_BeginDate VARCHAR2,
                      v_EndDate   VARCHAR2,
                      o_result   OUT empcurtype) as
                      
 v_sql VARCHAR2(4000);--临时医嘱表sql
 v_sql2 VARCHAR2(4000);--长期医嘱表sql   
--p_BeginDate VARCHAR2(50);
--p_EndDate   VARCHAR2(50);      
BEGIN

      
  v_sql :=  'select  starttime as "开始时间" ,madeordercontrol as "医嘱内容" ,executetime as "执行时间", 
     startdocname as "医师签名" ,
     startnursename  as "护士签名" from yd_shortmadeorder s where ';
     -- s.noofhis= '||v_InpatId;
     
     v_sql2 :=' select  starttime as "开立时间",madeordercontrol as "医嘱内容", startdocname as "医师签名", 
  startdocid as "医师编号",startnursename as "护士签名",startnurseid as "护士编号",
  endtime as "停止时间", enddocname as "停止医师签名",endnursename as "停止护士签名"
  from yd_longmadeorder l where ';
  -- l.noofhis='||v_InpatId;   
     
--p_BeginDate := v_BeginDate || ' 00:00:00';
--p_EndDate :=v_EndDate || ' 23:59:59';
  
 if v_Type='0'  --临时医嘱
 then

     if v_DeptId  is not null
     then
         v_sql :=v_sql || 'and deptid= '|| v_DeptId;
     end if;
     if v_BeginDate is not null
      then
        v_sql :=v_sql || 'and  starttime >='''||  v_BeginDate  ||  ' 00:00:00''';

     end if;
     if v_EndDate is not null
       then
        v_sql :=v_sql || 'and starttime <=''' || v_EndDate || ' 23:59:59''';

   end if;
   v_sql :=v_sql || 'order by starttime';
   
 open o_result for v_sql;
    
   else
  if v_DeptId is not null
     then
         v_sql2 :=v_sql2 || 'and deptid= '|| v_DeptId;
     end if;
     if v_BeginDate is not null
      then
      v_sql2 :=v_sql2 || 'and  starttime >='''||v_BeginDate||  ' 00:00:00''';
        
     end if;
     if v_EndDate is not null
       then
       v_sql2 :=v_sql2 || 'and starttime <=''' || v_EndDate || ' 23:59:59''';
   end if;
   v_sql2 :=v_sql2 || 'order by starttime';
   
 open o_result for v_sql2;
  END IF;
END;
  
  
  ----------------------------------------------------------医嘱浏览  测试盒演示所有  Author xlb 2013-04-10-------------------------------------------------
   PROCEDURE  GETORDER2(

                      v_InpatId VARCHAR2,
                      v_DeptId  VARCHAR2 default '',
                      v_Type    VARCHAR2,
                      v_BeginDate VARCHAR2 default '',
                      v_EndDate   VARCHAR2 default '',
                      o_result   OUT   empcurtype) as
                      
                      p_begindate VARCHAR2(50);
                      p_EndDate VARCHAR2(50);
BEGIN
  open o_result for  
  select'' from dual;
  if v_BeginDate is not null
  then  
    -----程序传过来只截取到日 
  p_begindate :=v_BeginDate || ' 00:00:00';
  END if;
  if v_EndDate is not null
  then
  p_EndDate :=v_EndDate || ' 23:59:59';
  END IF;
  if v_Type='0'
  THEN
     open o_result for  
     select  starttime as "医嘱时间"  ,madeordercontrol as "医嘱内容" ,executetime as "执行时间", 
     startdocname as "开立医生" ,
     startnursename  as "执行护士",deptid as "开立科室" from yd_shortmadeorder s where
     -- s.noofhis=v_InpatId
     --and
      s.deptid like '%'||v_DeptId||'%'  and to_date(s.starttime,'yyyy-mm-dd hh24:mi:ss') >= to_date(nvl(trim(p_begindate),'1000-01-01 00:00:00 '),'yyyy-MM-dd HH24:mi:ss') --起始时间为空则以1000-01-01 00:00:00替代
     and to_date(s.starttime,'yyyy-mm-dd hh24:mi:ss') <= to_date(nvl(trim(p_EndDate),'9000-01-01 23:59:59 '),'yyyy-MM-dd HH24:mi:ss') order by s.starttime;--结束时间为空则以1000-01-01 00:00:00替代
     
     else
       
    
     open o_result for    
     select  starttime as "开立时间",madeordercontrol as "医嘱内容", startdocname as "医师签名", 
     startdocid as "医师编号",startnursename as "护士签名",startnurseid as "护士编号",
     endtime as "停止时间", enddocname as "停止医师签名",endnursename as "停止护士签名",deptid as "开立科室"
     from yd_longmadeorder s where
     -- s.noofhis=v_InpatId
     --and
      s.deptid like '%'|| v_DeptId ||'%'  and s.starttime is not null and  to_date(s.starttime,'yyyy-MM-dd hh24:mi:ss') >= to_date(nvl(trim(p_begindate ),'1000-01-01 00:00:00 '),'yyyy-MM-dd HH24:mi:ss')
     and to_date(s.starttime,'yyyy-MM-dd hh24:mi:ss') <= to_date(nvl(trim(p_EndDate),'9000-01-01 23:59:59'),'yyyy-MM-dd HH24:mi:ss') order by s.starttime;
     END IF;        
  
                     
end;

  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
end ;--package end
/
