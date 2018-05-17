CREATE OR REPLACE PACKAGE EMR_DOCTOR_STATION
-----------------------------------------------------------
------------------Add By wwj 2012-07-02--------------------
-----------------医生工作站涉及到的逻辑--------------------
-----------------------------------------------------------
IS
   TYPE empcurtyp IS REF CURSOR;

   --得到三级检诊审核列表
   PROCEDURE usp_getthreelevelcheckList (
      v_deptid        VARCHAR,
      o_result    OUT   empcurtyp
   );
END;
/
CREATE OR REPLACE PACKAGE BODY EMR_DOCTOR_STATION
-----------------------------------------------------------
------------------Add By wwj 2012-07-02--------------------
-----------------医生工作站涉及到的逻辑--------------------
-----------------------------------------------------------
IS
   PROCEDURE usp_getthreelevelcheckList (
      v_deptid         VARCHAR,
      o_result    OUT   empcurtyp
   )
   AS
   BEGIN
      OPEN o_result
       FOR
          SELECT inpatient.NAME, recorddetail.ID, recorddetail.noofinpat,
                 recorddetail.templateid, recorddetail.NAME docname,
                 recorddetail.sortid, recorddetail.hassubmit,
                 c1.NAME AS hassubmitname, recorddetail.owner
            FROM recorddetail
            LEFT OUTER JOIN inpatient ON inpatient.noofinpat = recorddetail.noofinpat
            LEFT OUTER JOIN categorydetail c1 ON c1.categoryid = 46
                AND c1.ID = recorddetail.hassubmit
           WHERE recorddetail.valid = '1'
             AND inpatient.outhosdept = v_deptid
             AND recorddetail.hassubmit <> 4600;
   END;
END;
/
