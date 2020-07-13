-- Add/modify columns 
alter table IEM_MAINPAGE_OPERATION_SX add operation_enddate VARCHAR2(19);
-- Add comments to the columns 
comment on column IEM_MAINPAGE_OPERATION_SX.operation_enddate
  is '手术结束时间';