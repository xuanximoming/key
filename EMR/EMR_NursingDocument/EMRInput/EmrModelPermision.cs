using System;
using DrectSoft.Common.Eop;
using DrectSoft.Emr.Util;


namespace DrectSoft.Core.EMR_NursingDocument.EMRInput.Table
{
    /// <summary>
    /// 
    /// </summary>
    internal interface IEmrModelPermision
    {
        bool CanDo(EmrModel model);
        bool CanDo(EmrModel model, out string msg);
    }

    /// <summary>
    /// 创建权限
    /// </summary>
    internal class CreateModelPermision
        : IEmrModelPermision
    {

        private Employee _employee;

        private bool _readOnlyMode;

        public CreateModelPermision(
            Employee employee, bool readOnlyMode)
        {
            _employee = employee;

            _readOnlyMode = readOnlyMode;
        }

        #region IEmrModelPermision Members

        public bool CanDo(EmrModel model)
        {
            string msg;
            return CanCreateCore(model, out msg);
        }

        public bool CanDo(EmrModel model, out string msg)
        {
            return CanCreateCore(model, out msg);
        }

        #endregion

        private bool CanCreateCore(EmrModel model, out string msg)
        {
            msg = string.Empty;
            if (_readOnlyMode)
                return false;
            int grade = _employee.DoctorGradeNumber;
            if (grade == -1)
            {
                msg = "您无创建病历权限。";
                return false;
            }
            return true;
        }

    }

    /// <summary>
    /// 编辑权限
    /// </summary>
    internal class EditModelPermision
        : IEmrModelPermision
    {

        private Employee _employee;

        private bool _readOnlyMode;

        public EditModelPermision(
            Employee employee, bool readOnlyMode)
        {
            _employee = employee;
            _readOnlyMode = readOnlyMode;
        }

        #region IEmrModelPermision Members

        public bool CanDo(EmrModel model)
        {
            string msg;
            return CanEditCore(model, out msg);
        }

        public bool CanDo(EmrModel model, out string msg)
        {
            return CanEditCore(model, out msg);
        }

        #endregion

        private bool CanEditCore(EmrModel model, out string msg)
        {
            bool result = false;
            msg = string.Empty;
            if (_readOnlyMode)
                return false;
            int grade = _employee.DoctorGradeNumber;
            if (grade == -1)
            {
                //msg = "您无修改病历权限。";
                //return false;
            }
            // Check State

            if (_employee.Grade.Trim() == "") return false;
            DoctorGrade gradeEnum = (DoctorGrade)Enum.Parse(typeof(DoctorGrade), _employee.Grade);

            if (gradeEnum == DoctorGrade.Nurse)
            {
                //护理文档 AJ、护理记录 AI、手术护理记录 AK
                if (model.ModelCatalog == "AJ" || model.ModelCatalog == "AJ" || model.ModelCatalog == "AJ")
                {
                    return true;
                }
            }

            if (gradeEnum == DoctorGrade.None) return false;

            switch (model.State)
            {
                case ExamineState.Deleted:
                    result = model.CreatorXH == _employee.Code;
                    if (!result)
                    {
                        if (_employee.DoctorGradeNumber <= 0)
                            msg = "您无修改此病历文件的权限。";
                        else
                            msg = "此病历文件已删除。";
                    }
                    break;
                case ExamineState.NotSubmit:
                    result = model.CreatorXH == _employee.Code;
                    if (!result)
                    {
                        if (_employee.DoctorGradeNumber <= 0)
                            msg = "您无修改此病历文件的权限。";
                        else
                            msg = "此病历文件未提交。";
                    }
                    break;
                case ExamineState.SubmitButNotExamine:
                    result = (_employee.DoctorGradeNumber == 0);
                    if (!result)
                    {
                        if (model.CreatorXH == _employee.Code)
                            msg = "此病历文件已提交。";
                        else
                            msg = "您无修改此病历文件的权限。";
                    }
                    break;
                case ExamineState.FirstExamine:

                    result = (_employee.DoctorGradeNumber > 0);
                    if (!result)
                    {
                        if (model.CreatorXH == _employee.Code)
                            msg = "此病历文件已提交。";
                        else
                            msg = "您无修改此病历文件的权限。";
                    }
                    break;
                case ExamineState.SecondExamine:
                default:
                    result = (_employee.DoctorGradeNumber > 1);
                    if (!result)
                        msg = "您无修改此病历文件的权限。";
                    break;
            }
            return result;
        }

    }

    /// <summary>
    /// 删除权限
    /// </summary>
    internal class DeleteModelPermision
        : IEmrModelPermision
    {

        private Employee _employee;

        private bool _readOnlyMode;

        public DeleteModelPermision(
            Employee employee, bool readOnlyMode)
        {
            _employee = employee;
            _readOnlyMode = readOnlyMode;
        }

        #region IEmrModelPermision Members

        public bool CanDo(EmrModel model)
        {
            string msg;
            return CanDoCore(model, out msg);
        }

        public bool CanDo(EmrModel model, out string msg)
        {
            return CanDoCore(model, out msg);
        }

        #endregion

        private bool CanDoCore(EmrModel model, out string msg)
        {
            bool result;
            msg = string.Empty;
            if (_readOnlyMode)
                return false;

            int grade = _employee.DoctorGradeNumber;
            if (grade == -1)
            {
                //msg = "您无修改病历权限。";
                //return false;
            }

            if (model.State != ExamineState.NotSubmit)
            {
                result = false;
                msg = "此病历文件无法删除，因为病历文件已提交。";
            }

            else if (model.CreatorXH != _employee.Code)
            {
                result = false;
                msg = "此病历文件无法删除，因为只有创建人才可以删除病历文件。";
            }
            else
            {
                result = true;
                msg = string.Empty;
            }
            return result;
        }

    }

    /// <summary>
    /// 提交审核
    /// </summary>
    internal class SubmitModelPermision
        : IEmrModelPermision
    {

        private Employee _employee;
        private bool _readOnlyMode;

        public SubmitModelPermision(
            Employee employee, bool readOnlyModel)
        {
            _employee = employee;
            _readOnlyMode = readOnlyModel;
        }

        #region IEmrModelPermision Members

        public bool CanDo(EmrModel model)
        {
            return CanSubmitCore(model);
        }

        public bool CanDo(EmrModel model, out string msg)
        {
            msg = string.Empty;
            return CanSubmitCore(model);
        }

        #endregion

        private bool CanSubmitCore(EmrModel model)
        {
            if (_readOnlyMode)
                return false;
            bool result;
            int grade = _employee.DoctorGradeNumber;
            if (grade == -1)
            {
                return false;
            }
            // Check State
            switch (model.State)
            {
                case ExamineState.NotSubmit:
                    result = model.CreatorXH == _employee.Code;
                    break;
                case ExamineState.Deleted:
                    result = false;
                    break;
                case ExamineState.SubmitButNotExamine:
                    result = (_employee.DoctorGradeNumber > 0);
                    break;
                case ExamineState.FirstExamine:
                    result = _employee.DoctorGradeNumber > 1;
                    break;
                case ExamineState.SecondExamine:
                default:
                    result = _employee.DoctorGradeNumber == 2;
                    break;
            }
            return result;
        }

    }

    //审核病历的权限 
    internal class AuditModelPermision
        : IEmrModelPermision
    {
        private Employee _employee;
        private bool _readOnlyMode;

        public AuditModelPermision(
          Employee employee, bool readOnlyModel)
        {
            _employee = employee;
            _readOnlyMode = readOnlyModel;
        }
        #region IEmrModelPermision 成员

        public bool CanDo(EmrModel model)
        {
            return CanAuditModeCore(model);
        }

        public bool CanDo(EmrModel model, out string msg)
        {
            msg = string.Empty;
            return CanAuditModeCore(model);
        }
        private bool CanAuditModeCore(EmrModel model)
        {
            if (_readOnlyMode)
                return false;
            bool result;

            if (_employee.DoctorGradeNumber == -1)
            {
                return false;
            }
            switch (model.State)
            {
                //未提交则不允许审核
                case ExamineState.NotSubmit:
                case ExamineState.Deleted:
                    result = false;
                    break;
                case ExamineState.SubmitButNotExamine:// 主治以上才能审核
                    result = _employee.DoctorGradeNumber > 0;
                    if (result) result = GetThreeLevelCheck(model, _employee);
                    break;
                case ExamineState.FirstExamine:
                    result = _employee.DoctorGradeNumber >= 1;
                    if (result) result = GetThreeLevelCheck(model, _employee);
                    break;
                case ExamineState.SecondExamine:
                default:
                    result = _employee.DoctorGradeNumber >= 2;
                    break;
            }
            return result;
        }
        #endregion

        const string c_GetThreeLevelCheck = @"select count(1) from THREE_LEVEL_CHECK ";
        const string c_Resident = " where resident_id = '{0}' ";
        const string c_Attend = " where resident_id = '{0}' and attend_id = '{1}' ";
        const string c_Chief = " where resident_id = '{0}' and chief_id = '{1}' ";

        /// <summary>
        /// 判断是否有审核的权限
        /// </summary>
        /// <param name="model"></param>
        /// <returns>true：有审核权限 false：没有审核权限</returns>
        private bool GetThreeLevelCheck(EmrModel model, Employee employee)
        {
            IDataAccess sqlDataAccess = DataAccessFactory.DefaultDataAccess;
            bool result = true;
            string num = sqlDataAccess.ExecuteScalar(c_GetThreeLevelCheck + string.Format(c_Resident, model.CreatorXH), System.Data.CommandType.Text).ToString();
            if (num != "0") //设置了指定人员的三级检诊
            {
                switch (employee.DoctorGradeNumber)
                {
                    case 1: //主治医师
                        num = sqlDataAccess.ExecuteScalar(c_GetThreeLevelCheck + string.Format(c_Attend, model.CreatorXH, _employee.Code), System.Data.CommandType.Text).ToString();
                        if (num == "0")
                        {
                            result = false;
                        }
                        break;
                    case 2: //主任医师 副主任医师
                        num = sqlDataAccess.ExecuteScalar(c_GetThreeLevelCheck + string.Format(c_Chief, model.CreatorXH, _employee.Code), System.Data.CommandType.Text).ToString();
                        if (num == "0")
                        {
                            result = false;
                        }
                        break;
                }
            }
            return result;
        }
    }

    /// <summary>
    /// 撤销审核
    /// </summary>
    internal class WithdrawSubmissionModelPermision
        : IEmrModelPermision
    {

        private Employee _employee;

        private bool _readOnlyMode;

        public WithdrawSubmissionModelPermision(
            Employee employee, bool readOnlyMode)
        {
            _employee = employee;
            _readOnlyMode = readOnlyMode;
        }

        #region IEmrModelPermision Members

        public bool CanDo(EmrModel model)
        {
            return CanWithdrawSubmissionCore(model);
        }

        public bool CanDo(EmrModel model, out string msg)
        {
            msg = string.Empty;
            return CanWithdrawSubmissionCore(model);
        }

        #endregion

        private bool CanWithdrawSubmissionCore(EmrModel model)
        {
            if (_readOnlyMode)
                return false;
            int grade = _employee.DoctorGradeNumber;
            if (grade == -1)
                return false;
            // Check State
            if ((model.State == ExamineState.NotSubmit) || (model.State == ExamineState.Deleted))
                return false;
            if (model.State == ExamineState.SubmitButNotExamine) // 已提交未审核
            {
                //如果允许多次撤销，若当前病历是当前医生创建或当前医师是住院医师级别以上的医师，则允许撤销

                return _employee.DoctorGradeNumber > 1;
            }
            else if (model.State == ExamineState.FirstExamine) // 主治已审核状态
                return _employee.DoctorGradeNumber > 1; // 暂时不做其它控制
            else // 其它状态下，只能由主任撤销
                return _employee.DoctorGradeNumber > 2;
        }

    }

    internal static class ModelPermisionFactroy
    {

        internal static IEmrModelPermision Create(
            ModelPermisionType type, Employee employee)
        {
            return Create(type, employee, false);
        }

        internal static IEmrModelPermision Create(
            ModelPermisionType type, Employee employee, bool readOnlyMode)
        {
            switch (type)
            {
                case ModelPermisionType.Create:
                    return new CreateModelPermision(
                        employee, readOnlyMode);
                case ModelPermisionType.Edit:
                    return new EditModelPermision(
                        employee, readOnlyMode);
                case ModelPermisionType.Delete:
                    return new DeleteModelPermision(
                        employee, readOnlyMode);
                case ModelPermisionType.Submit:
                    return new SubmitModelPermision(
                        employee, readOnlyMode);
                case ModelPermisionType.Audit:
                    return new AuditModelPermision(
                        employee, readOnlyMode);
                case ModelPermisionType.WithdrawSubmission:
                    return new WithdrawSubmissionModelPermision(
                        employee, readOnlyMode);
                default:
                    return null;
            }
        }

    }

    /// <summary>
    /// 病历模型操作列表
    /// </summary>
    internal enum ModelPermisionType
    {
        /// <summary>
        /// 创建
        /// </summary>
        Create,
        /// <summary>
        /// 编辑
        /// </summary>
        Edit,
        /// <summary>
        /// 删除
        /// </summary>
        Delete,
        /// <summary>
        /// 替换
        /// </summary>
        Replace,
        /// <summary>
        /// 提交
        /// </summary>
        Submit,
        /// <summary>
        /// 审核
        /// </summary>
        Audit,
        /// <summary>
        /// 撤销审核
        /// </summary>
        WithdrawSubmission,
    }

}
