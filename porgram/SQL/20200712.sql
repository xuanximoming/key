-- Add/modify columns
alter table IEM_MAINPAGE_OPERATION_SX add mainoperation varchar2(1); 
alter table IEM_MAINPAGE_OPERATION_SX add iatrogenic varchar2(1);
alter table IEM_MAINPAGE_OPERATION_SX add ischoosedate varchar2(1);
-- Add comments to the columns 
comment on column IEM_MAINPAGE_OPERATION_SX.mainoperation
  is '是否为主手术 1是';
comment on column IEM_MAINPAGE_OPERATION_SX.iatrogenic
  is '医源性手术 0 否 1是';
comment on column IEM_MAINPAGE_OPERATION_SX.ischoosedate
  is '手术类型 1 择期 2 急诊';
  -- Add/modify columns 
alter table IEM_MAINPAGE_OPERATION_SX modify mainoperation default '0';
alter table IEM_MAINPAGE_OPERATION_SX modify iatrogenic default '0';
alter table IEM_MAINPAGE_OPERATION_SX modify ischoosedate default '1';