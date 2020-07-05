-- Add/modify columns 
alter table IEM_MAINPAGE_OPERATION_SX add complication_code VARCHAR2(20);
alter table IEM_MAINPAGE_OPERATION_SX add complication_name VARCHAR2(100);
-- Add comments to the table 
comment on table IEM_MAINPAGE_OPERATION_SX
  is '病案手术表';
-- Add comments to the columns 
comment on column IEM_MAINPAGE_OPERATION_SX.complication_code
  is '并发症代码';
comment on column IEM_MAINPAGE_OPERATION_SX.complication_name
  is '并发症名称';