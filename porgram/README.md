# 修改记录

## 1、修改20230315

### 门诊病历使用

1、修改程序

2、数据库修改

```sql
PROCEDURE usp_inpatient_clinic(v_noofinpat VARCHAR2,
                                       o_result    OUT empcurtype) AS
  BEGIN
    OPEN o_result FOR
      select ic.name           PatName,
             ic.birth          PatBirth, 
             dd1.name          PatSexName,
             ic.patid          PatID,
             ic.age            PatAge,
             ic.bedcode        PatBedCode,
             dd2.name          PatMaritalName,
             ic.country        PatCountryName,
             ic.nationality    PatNationalityName,
             ic.jobname        PatJobName,
             ic.contactaddress PatContactAddress,
             dept.name         DeptName,
             us.name           CLINICDOCTOR  --20230315添加
        from inpatient_clinic ic
        left outer join dictionary_detail dd1
          on dd1.categoryid = '3'
         and dd1.detailid = ic.sexid
         and dd1.valid = '1'
        left outer join dictionary_detail dd2
          on dd2.categoryid = '4'
         and dd2.detailid = ic.maritalid
         and dd2.valid = '1'
        left outer join DEPARTMENT dept
          on dept.id = ic.deptid
        left outer join users us
          on us.id = ic.visitdoctorid
       where ic.noofinpatclinic = v_noofinpat
         and ic.valid = '1';
  END;
```

3、界面常用词添加即可

## 2、修改20230413

### 修改病案首页问题

替换新的DrectSoft.Core.IEMMainPage_SX.dll文件后报错

1、替换程序DrectSoft.Core.IEMMainPage_SX.dll

2、修改表

```sql
--IEM_MAINPAGE_BASICINFO_SX
-- Add/modify columns 
alter table IEM_MAINPAGE_BASICINFO_SX add inspect_ct NUMBER(1);
alter table IEM_MAINPAGE_BASICINFO_SX add inspect_petct NUMBER(1);
alter table IEM_MAINPAGE_BASICINFO_SX add inspect_toct NUMBER(1);
alter table IEM_MAINPAGE_BASICINFO_SX add inspect_x NUMBER(1);
alter table IEM_MAINPAGE_BASICINFO_SX add inspect_uc NUMBER(1);
alter table IEM_MAINPAGE_BASICINFO_SX add inspect_mri NUMBER(1);
alter table IEM_MAINPAGE_BASICINFO_SX add inspect_b NUMBER(1);
alter table IEM_MAINPAGE_BASICINFO_SX add inspect_isotope NUMBER(1);
-- Add comments to the columns 
comment on column IEM_MAINPAGE_BASICINFO_SX.inspect_ct
  is '检查情况CT';
comment on column IEM_MAINPAGE_BASICINFO_SX.inspect_petct
  is '检查情况PETCT';
comment on column IEM_MAINPAGE_BASICINFO_SX.inspect_toct
  is '检查情况双源CT';
comment on column IEM_MAINPAGE_BASICINFO_SX.inspect_x
  is '检查情况X片';
comment on column IEM_MAINPAGE_BASICINFO_SX.inspect_uc
  is '检查情况超声';
  
--并给这几个字段赋予初始值 1
```

3、修改存储过程

```sql
--替换以下两个存储过程
PACKAGE iem_main_page_sx.usp_Edit_Iem_BasicInfo_sx
PACKAGE BODY iem_main_page_sx.usp_Edit_Iem_BasicInfo_sx
```

4、替换首页xml

Sheet/MRHPEN.xml

