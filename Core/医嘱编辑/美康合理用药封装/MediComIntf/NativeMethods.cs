using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace DrectSoft.Common.MediComIntf
{
    internal class NativeMethods
    {
        [DllImport("ShellRunAs.dll", CharSet = CharSet.Auto)]
        internal static extern int RegisterServer();

        [DllImport("DIFPassDll.dll", CharSet = CharSet.Auto)]
        internal static extern int PassInit(
            [MarshalAs(UnmanagedType.LPStr)]string serName,
           [MarshalAs(UnmanagedType.LPStr)]string DepartMentName,
            int WorkstationType);

        [DllImport("DIFPassDll.dll", CharSet = CharSet.Auto)]
        internal static extern int PassGetState(
            [MarshalAs(UnmanagedType.LPStr)]string QueryItemNo);

        [DllImport("DIFPassDll.dll", CharSet = CharSet.Auto)]
        internal static extern int PassSetControlParam(int SaveCheckResult, int AllowAllegen, int CheckMode, int DisqMode, int UseDiposeIdea);

        [DllImport("DIFPassDll.dll", CharSet = CharSet.Auto)]
        internal static extern int PassSetQueryDrug([MarshalAs(UnmanagedType.LPStr)]string DrugCode,
                    [MarshalAs(UnmanagedType.LPStr)]string DrugName,
                   [MarshalAs(UnmanagedType.LPStr)]string DoseUnit,
                  [MarshalAs(UnmanagedType.LPStr)]string RouteName);

        [DllImport("DIFPassDll.dll", CharSet = CharSet.Auto)]
        internal static extern int PassDoCommand(int CommandNo);

        [DllImport("DIFPassDll.dll", CharSet = CharSet.Auto)]
        internal static extern int PassSetFloatWinPos(int left, int top, int right, int bottom);

        [DllImport("DIFPassDll.dll", CharSet = CharSet.Auto)]
        internal static extern int PassSetPatientInfo(
            [MarshalAs(UnmanagedType.LPStr)]string PatientID,
           [MarshalAs(UnmanagedType.LPStr)]string VisitID,
           [MarshalAs(UnmanagedType.LPStr)]string Name,
           [MarshalAs(UnmanagedType.LPStr)]string Sex,
           [MarshalAs(UnmanagedType.LPStr)]string Birthday,
           [MarshalAs(UnmanagedType.LPStr)]string Weight,
           [MarshalAs(UnmanagedType.LPStr)]string cHeight,
           [MarshalAs(UnmanagedType.LPStr)]string DepartMentName,
           [MarshalAs(UnmanagedType.LPStr)]string Doctor,
           [MarshalAs(UnmanagedType.LPStr)]string LeaveHospitalDate);

        [DllImport("DIFPassDll.dll", CharSet = CharSet.Auto)]
        internal static extern int PassSetRecipeInfo([MarshalAs(UnmanagedType.LPStr)]string OrderUniqueCode,
            [MarshalAs(UnmanagedType.LPStr)]string DrugCode,
            [MarshalAs(UnmanagedType.LPStr)]string DrugName,
            [MarshalAs(UnmanagedType.LPStr)]string SingleDose,
            [MarshalAs(UnmanagedType.LPStr)]string DoseUnit,
            [MarshalAs(UnmanagedType.LPStr)]string Frequency,
            [MarshalAs(UnmanagedType.LPStr)]string StartOrderDate,
            [MarshalAs(UnmanagedType.LPStr)]string StopOrderDate,
            [MarshalAs(UnmanagedType.LPStr)]string RouteName,
            [MarshalAs(UnmanagedType.LPStr)]string GroupTag,
            [MarshalAs(UnmanagedType.LPStr)]string OrderType,
            [MarshalAs(UnmanagedType.LPStr)]string OrderDoctor);

        [DllImport("DIFPassDll.dll", CharSet = CharSet.Auto)]
        internal static extern int PassSetAllergenInfo(
            [MarshalAs(UnmanagedType.LPStr)]string AllergenIndex,
            [MarshalAs(UnmanagedType.LPStr)]string AllergenCode,
            [MarshalAs(UnmanagedType.LPStr)]string AllergenDesc,
            [MarshalAs(UnmanagedType.LPStr)]string AllergenType,
            [MarshalAs(UnmanagedType.LPStr)]string Reaction);

        [DllImport("DIFPassDll.dll", CharSet = CharSet.Auto)]
        internal static extern int PassSetMedCond(
            [MarshalAs(UnmanagedType.LPStr)]string MedCondIndex,
            [MarshalAs(UnmanagedType.LPStr)]string MedCondCode,
            [MarshalAs(UnmanagedType.LPStr)]string MedCondDesc,
            [MarshalAs(UnmanagedType.LPStr)]string MedCondType,
            [MarshalAs(UnmanagedType.LPStr)]string StartDate,
            [MarshalAs(UnmanagedType.LPStr)]string EndDate);

        [DllImport("DIFPassDll.dll", CharSet = CharSet.Auto)]
        internal static extern int PassGetWarn(
            [MarshalAs(UnmanagedType.LPStr)]string DrugUniqueCode);

        [DllImport("DIFPassDll.dll", CharSet = CharSet.Auto)]
        internal static extern int PassSetWarnDrug(
            [MarshalAs(UnmanagedType.LPStr)]string DrugUniqueCode);

        [DllImport("DIFPassDll.dll", CharSet = CharSet.Auto)]
        internal static extern int PassQuit();
    }
}
