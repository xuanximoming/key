using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.Core.DoctorAdvice
{
    internal static class ConstSqlSentences
    {
        // TODO: 以后都要改成传参形式
        #region Order Suite
        public const string SelectOrderSuiteDetailSchema = "select * from AdviceGroupDetail where 1=2";
        public const string FormatInsertSuite = "insert into {0}(Name,py, Wb, DeptID, WardID, DoctorID, UseRange, Memo) values(@Name, @py, @Wb, @DeptID, @WardID, @DoctorID, @UseRange, @Memo)";
        public const string FormatSelectSuiteDetail = "select * from {0} where SuiteID = {1}";
        public const string FormatSelectSuite = "select * from {0} where SuiteID = {1}";
        public const string FormatDeleteSuiteData = "delete from {0} where SuiteID = {2} \r\n delete from {1} where SuiteID = {2}";
        public const string FormatDeleteSuiteDetail = "delete from {0} where SuiteID = {1}";
        public const string FormatSuiteDetailFilter = " SuiteID = {0} and Mark = {1:D}";
        #endregion

        #region Order
        public const string FormatSelectOrderSchema = "select * from {0} where 1=2";
        public const string FormatSelectOrderData = "select * from {0} where NoOfInpat = {1} order by {2}";
        public const string FormatSelectChangedOrderData = "select count(*) num from {0} where NoOfInpat = {1} and (({3} in ({4}) and OrderStatus <> {2:D}) or ({3} not in ({4}) and OrderStatus = {2:D}))";
        public const string FormatDeleteNewOrder = "delete from {0} where NoOfInpat = {1} and OrderStatus = {2:D}";
        public const string FormatSelectNotSynchedOrderData = "select * from {0} where NoOfInpat = {1} and (Synch = 0 or OrderStatus = 3200) order by {2}";
        public const string FormatSelectSerialNosOfNewSynchedOrder = "select {0}, GroupID from {1} where NoOfInpat = {2} and OrderStatus = {3:D} order by {0}";
        #endregion

        #region Employee
        public const string FormatSelectEmployee = "select JobTitle from Users where ID = '{0}'";
        #endregion

        #region Frequency
        public const string FormatFrequencyFilter = "Mark in ({0:D}, {1:D})";
        #endregion

        #region ChargeItem
        public const string FormatChargeItemFilterNormal = "ItemNature = 0 and ItemCategroy in ({0:D},{1:D},{2:D},{3:D},{4:D},{5:D},{6:D},{7:D},{8:D})";
        public const string FormatChargeItemFilterOperation = "ItemNature = 0 and ItemCategroy = {0:D}";
        public const string ChargeItemFilterGeneral = "ItemNature = 1";
        public const string ChargeItemFilterText = "ItemNature = 2";
        #endregion

        #region Druggery
        public const string FormatDruggeryKindFilter = "DrugCategory <> {0}";
        #endregion

        #region Inpatient
        public const string FormatUpdateInpatient = "update InPatient set {0} where NoOfInpat = {1}";
        public const string FormatSelectInpatient = "select * from InPatient where NoOfInpat = {0}";
        #endregion

        #region order content catalog
        //public const string FormatFilteContentCatalog
        #endregion

        #region skin test result
        public const string FormatSelectSkinTestResult = "select a.NormNo, b.Name DrugName, a.StartDate, a.EndDate, (case a.Masculine when 1 then '阳性' else '阴性' end) Masculine from DrugAllergy a, FormatOfDrug b where a.NoOfInpat = {0} and a.NormNo = b.ID order by a.StartDate desc";
        public const string FormatInsertSkinTestResult = "insert into BL_BRGMJLK(NoOfInpat, NormNo, StartDate, EndDate, Masculine, Tester, TestDate, Valid, Memo)"
                 + " values({0}, {1}, '{2}', '{3}', {4}, '{5}', '{6}', 1, '')";
        #endregion

        #region Tech Request Form
        //public const string FormatUpdateTechRequest = "update BL_SQDK set qrbz = 1 where xh = {0}";
        #endregion

        #region recipe rule
        public const string FormatRecipeRuleItemAmountFilter = "pzlx in ('-1','{0}') and cdxh = {1} and ypdm = '{2}' and lcxmdm = 0";
        public const string FormatRecipeRuleFilter = "ypbz = {0:D} and fldm = '{1}' and {2} = '{3}'";
        public const string FormatRecipeRuleDetailFilter = "fwlb = '{0}' and fwvalue = '{1}' and fldm = '{2}' and cdxh = {3} and ypdm = '{4}'";
        //add by zhouhui 
        public const string FormatRecipeRuleCFYP = "select Code,DrugMark,SortCode from DrugPrescription where RangeCategory = '{0}'";
        public const string FormatRecipeRuleTSYP = "select RangecCategory,RangeValue,DrugMark,SortCode,ClinicalCode,PlaceID,DrugID from SpDrugPrescription ";
        public const string FormatRecipeRuleJLXZ = "select LimitID, ProofType, PlaceID, DrugID, ClinicalCode, DrugName, DefaultDose, UnitCategory , WarnDose, ControlDose, DefaultNO, WarnNO, ControlNO, Memo from DrugDoseLimit  ";
        #endregion
    }
}
