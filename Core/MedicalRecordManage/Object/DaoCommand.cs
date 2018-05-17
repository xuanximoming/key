using System;
using System.Collections.Generic;
using System.Data;

namespace MedicalRecordManage.Object
{
    class DaoCommand
    {
        //查询病案借阅记录SQL语句，全部，查询后界面显示时需要压缩合并记录.
        public static string _SELECT_MEDICAL_RECORD_BORROW_SQL =
            " select rownum as seq, i.* from MED_REC_BORROW_VIEW i where 1 = 1 ";

        public static string _SELECT_MEDICAL_RECORD_BORROW_SQL_ZY =
           " select rownum as seq, i.* from MED_REC_BORROW_VIEW_ZY i where 1 = 1 ";
        //查询病案借阅记录SQL语句，全部，查询后界面显示时需要压缩合并记录.
        public static string _SELECT_MEDICAL_RECORD_WriteUp_SQL =
            " select rownum as seq, i.* from MED_REC_WriteUp_VIEW i where 1 = 1 ";

        public static string _SELECT_MEDICAL_RECORD_WriteUp_SQL_ZY =
           " select rownum as seq, i.* from med_rec_WriteUp_view_zy i where 1 = 1 ";
        /*
create or replace view med_rec_borrow_view as
Select e.id,i.noofinpat,i.name,
             u.name as applyname,e.applydate,to_char(e.applydate,'yyyy-MM-dd hh:mm:ss') as sqsj,
             e.applycontent,e.applytimes,e.approvedocid,e.approvecontent,
             e.approvedate,to_char(e.approvedate,'yyyy-MM-dd hh:mm:ss') as shsj,
             nvl2(trim(e.yanqiflag),'是','否') as yq, e.status,'' as statusdes,
             i.age || '岁' as age,i.outhosdate,i.outbed, ieb.outhosdept,
             d.name as cyks, ied.diagnosis_name as cyzd,e.applydocid
             from EMR_RecordBorrow e, inpatient i,users u,
             iem_mainpage_basicinfo_2012 ieb,department d,iem_mainpage_diagnosis_2012 ied
             where e.noofinpat = i.noofinpat
             and e.noofinpat = ieb.noofinpat(+)
             and e.applydocid = u.id(+)
             and ieb.iem_mainpage_no = ied.iem_mainpage_no(+)
             and ieb.outhosdept = d.id(+)
             order by e.status,u.name,e.applydate,i.noofinpat,i.name,ied.create_time,ied.diagnosis_name;           
    */
        public static string _SELECT_MEDICAL_RECORD_INPATIENT_SHORT =
            " select rownum as seq, i.* from MED_REC_INPATIENT_VIEW i where 1 = 1 ";

        public static string _SELECT_MEDICAL_RECORD_INPATIENT_SHORT_ZY =
          " select rownum as seq, i.* from MED_REC_INPATIENT_VIEW_ZY i where 1 = 1 ";
        /*
create or replace view med_rec_inpatient_view as
select distinct i.noofinpat,i.name,i.age || '岁' as age,i.sexid,i.admitdate, nvl(i.TOTALDAYS,0) as zyts,
        i.outhosdate, ie.outhosdept,d.name as cyks, ied.diagnosis_name as cyzd,ied.create_time
        from inpatient i,recorddetail r,iem_mainpage_basicinfo_2012 ie,
        iem_mainpage_diagnosis_2012 ied,department d
        where i.noofinpat = r.noofinpat
        and i.noofinpat = ie.noofinpat
--            and i.noofinpat not in (select noofinpat from emr_recordborrow where status < 3)
        and ie.iem_mainpage_no = ied.iem_mainpage_no(+)
        and ie.outhosdept = d.id(+)
        and r.islock = 4701
        and ied.diagnosis_type_id in(7,8)
        and ied.valide=1 and ie.valide=1
        and r.valid=1
        order by i.noofinpat,ied.create_time, ied.diagnosis_name;
         * */
        //public static string _ORD_MEDICAL_RECORD_INPATIENT_SHORT =
        //    " order by i.noofinpat,ied.create_time, ied.diagnosis_name ";
        //针对出院病人
        public static string _SELECT_MEDICAL_RECORD_INPATIENT_LONG =
            " select rownum as seq, i.* from MED_REC_INPATIENT_OP_VIEW i where 1= 1 ";

        //add by zjy 2013-6-16 针对出院病人
        public static string _SELECT_MEDICAL_RECORD_INPATIENT_LONG_ZY =
            " select rownum as seq, i.* from MED_REC_INPATIENT_OP_VIEW_ZY i where 1= 1 ";

        //在院病人 !!!!【待加视图】!!!!
        public static string _SELECT_MEDICAL_RECORD_InHOSINP_LONG =
            " select rownum as seq, i.* from MED_REC_InHOSINP_OP_VIEW i where 1= 1 ";

        //add by zjy 2013-6-16 针对出院病人
        public static string _SELECT_MEDICAL_RECORD_InHOSINP_LONG_ZY =
            " select rownum as seq, i.* from MED_REC_InHOSINP_OP_VIEW_ZY i where 1= 1 ";

        /*
create or replace view med_rec_inpatient_op_view as
select  distinct i.noofinpat,i.name,i.age || '岁' as age,i.sexid,i.outhosdate,
             i.ADMITDATE, nvl(i.TOTALDAYS,0) as zyts ,
             trunc(sysdate,'DD') - trunc(to_date(outhosdate,'yyyy-MM-dd HH24:mi:ss'),'DD') as ycyts,
             --0 as ycyts,
             ie.outhosdept,
             d.name as cyks, ied.diagnosis_name as cyzd,ied.create_time,ieo.operation_name,ieo.operation_date
             from inpatient i,recorddetail r,
             iem_mainpage_basicinfo_2012 ie,iem_mainpage_diagnosis_2012 ied,
             iem_mainpage_operation_2012 ieo,department d
             where  i.noofinpat = r.noofinpat
             and i.noofinpat = ie.noofinpat
             and ie.iem_mainpage_no = ied.iem_mainpage_no(+)
             and ie.iem_mainpage_no = ieo.iem_mainpage_no(+)
             and ie.outhosdept = d.id(+)
             and r.islock = 4701
             and ied.diagnosis_type_id in(7,8)
             and ied.valide = 1 and ie.valide = 1 and r.valid = 1
             order by i.noofinpat,ied.create_time,cyzd,ieo.operation_date,ieo.operation_name;
         */
        public static string _SELECT_MEDICAL_RECORD_INPATIENT_LONG_ALL =
            " select rownum as seq, i.* from MED_REC_INPATIENT_OP_ALL_VIEW i where 1= 1 ";
        /*
create or replace view med_rec_inpatient_op_all_view as
select  distinct i.noofinpat,i.name,i.age || '岁' as age,i.sexid,i.outhosdate,
         i.ADMITDATE, nvl(i.TOTALDAYS,0) as zyts ,
         trunc(sysdate,'DD') - trunc(to_date(outhosdate,'yyyy-MM-dd HH24:mi:ss'),'DD') as ycyts,
         --0 as ycyts,
         ie.outhosdept,
         d.name as cyks, ied.diagnosis_name as cyzd,ied.create_time,ieo.operation_name,ieo.operation_date
         from inpatient i,recorddetail r,
         iem_mainpage_basicinfo_2012 ie,iem_mainpage_diagnosis_2012 ied,
         iem_mainpage_operation_2012 ieo,department d
         where  i.noofinpat = r.noofinpat
         and i.noofinpat = ie.noofinpat
         and ie.iem_mainpage_no = ied.iem_mainpage_no(+)
         and ie.iem_mainpage_no = ieo.iem_mainpage_no(+)
         and ie.outhosdept = d.id(+)
         and i.status in (1502,1503)
         and ied.diagnosis_type_id in(7,8)
         and ied.valide = 1 and ie.valide = 1 and r.valid = 1
         order by i.noofinpat,ied.create_time,cyzd,ieo.operation_date,ieo.operation_name;
    */

        //获取指定列的值用于列表对象的显示
        public static List<string> GetColumnList(string table, string column, string conditions = null)
        {
            try
            {
                List<string> list = new List<string>();
                string sql = " select " + column + " from " + table + "  " + conditions;
                DataSet dataSet = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataSet(sql);
                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    list.Add(dataSet.Tables[0].Rows[i][0].ToString());
                }
                dataSet.Clone();
                return list;
            }
            catch (Exception)
            {

                throw;
            }

        }

        //根据指定的SQL语句查询并返回查询DataSet结果
        public static DataSet SelectObjectCommand(string sql)
        {
            try
            {
                return DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataSet(sql); ;
            }
            catch (Exception)
            {

                throw;
            }
        }
        //执行新增操作，传入参数为实体类，例如实体为EMR_RecordBorrow
        public static void InsertObjectCommand(object Object)
        {
            try
            {
                CommonLib.ORMWork temp = new CommonLib.ORMWork();
                temp.InserAnObject(Object);
            }
            catch (Exception)
            {

                throw;
            }
        }
        //执行更新操作，传入参数为实体类，例如实体为EMR_RecordBorrow
        public static void UpdateObjectCommand(object Object)
        {
            try
            {
                CommonLib.ORMWork temp = new CommonLib.ORMWork();
                temp.UpdateAnObject(Object);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //执行删除操作，传入参数为实体类，例如实体类为EMR_RecordBorrow
        public static void DeleteObjectCommand(object Object)
        {
            try
            {
                CommonLib.ORMWork temp = new CommonLib.ORMWork();
                temp.DeleteAnObject(Object);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
