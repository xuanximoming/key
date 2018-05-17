#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace DrectSoft.Core
{
    /// <summary>
    /// 用户信息接口
    /// </summary>
    public interface IUser
    {
        ///// <summary>
        ///// 当前科室被改变事件
        ///// </summary>
        //event EventHandler CurrentDeptChanged;

        ///// <summary>
        ///// 当前病区被改变事件
        ///// </summary>
        //event EventHandler CurrentWardChanged;

        /// <summary>
        /// 当前科室病区被改变事件
        /// </summary>
        event EventHandler CurrentDeptWardChanged;

        /// <summary>
        /// 获取用户代码
        /// </summary>
        /// <value></value>
        string Id { get; }

        /// <summary>
        /// 获取用户名称
        /// </summary>
        /// <value></value>
        string Name { get; }

        /// <summary>
        /// 获取操作员的医生Id
        /// </summary>
        string DoctorId { get; }

        /// <summary>
        /// 获取操作员的医生姓名
        /// </summary>
        string DoctorName { get; }

        /// <summary>
        /// 获取当前科室代码
        /// </summary>
        /// <value></value>
        string CurrentDeptId { get; }

        /// <summary>
        /// 获取当前科室名称
        /// </summary>
        /// <value></value>
        string CurrentDeptName { get; }

        /// <summary>
        /// 获取当前病区代码
        /// </summary>
        /// <value></value>
        string CurrentWardId { get; }

        /// <summary>
        /// 获取当前病区名称
        /// </summary>
        /// <value></value>
        string CurrentWardName { get; }

        /// <summary>
        /// 岗位代码字串（形式：01,02,03,）
        /// </summary>
        string GWCodes { get; }

        /// <summary>
        /// 获取该用户是否有效
        /// </summary>
        bool Available { get; }

        /// <summary>
        /// 带教老师
        /// </summary>
        string MasterID { get; set; }

        /// <summary>
        /// 获取用户相关科室病区信息(形式:deptid,deptname/wardid,wardname)
        /// </summary>
        IList<DeptWardInfo> RelateDeptWards { get; }

        /// <summary>
        /// 当前科室病区信息
        /// </summary>
        DeptWardInfo CurrentDeptWard { get; }
    }

    /// <summary>
    /// 科室病区信息
    /// </summary>
    public struct DeptWardInfo
    {
        /// <summary>
        /// 科室Id
        /// </summary>
        public string DeptId
        {
            get { return _deptId; }
        }
        private string _deptId;

        /// <summary>
        /// 科室Name
        /// </summary>
        public string DeptName
        { get { return _deptName; } }
        private string _deptName;

        /// <summary>
        /// 病区Id
        /// </summary>
        public string WardId
        { get { return _wardId; } }
        private string _wardId;

        /// <summary>
        /// 病区Name
        /// </summary>
        public string WardName
        { get { return _wardName; } }
        private string _wardName;

        /// <summary>
        /// 按病区显示病人
        /// </summary>
        public bool MergeSameWard
        {
            get { return _mergeSameWard; }
            set { _mergeSameWard = value; }
        }
        private bool _mergeSameWard;

        /// <summary>
        /// 病区关联的科室（在按病区显示病人时用作查询的过滤条件）
        /// </summary>
        public string RelateDeptIds
        {
            get
            {
                if (!MergeSameWard || String.IsNullOrEmpty(_relateDeptIds))
                    return DeptId;
                else
                    return _relateDeptIds;
            }
            set
            {
                _relateDeptIds = value;
            }
        }
        private string _relateDeptIds;

        /// <summary>
        /// KEY
        /// </summary>
        public string Key
        {
            get
            {
                if (String.IsNullOrEmpty(DeptId) && String.IsNullOrEmpty(WardId))
                    return "";
                else if (String.IsNullOrEmpty(DeptId))
                    return "-" + WardId;
                else if (String.IsNullOrEmpty(WardId))
                    return DeptId;
                else
                    return DeptId + "-" + WardId;
            }
        }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string Display
        {
            get
            {
                if (String.IsNullOrEmpty(_display))
                {
                    if (String.IsNullOrEmpty(WardName))
                        return DeptName;
                    else
                        return DeptName + "(" + WardName + ")";
                }
                else
                    return _display;
            }
            set { _display = value; }
        }
        private string _display;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="deptname"></param>
        /// <param name="wardid"></param>
        /// <param name="wardname"></param>
        public DeptWardInfo(string deptid, string deptname, string wardid, string wardname)
        {
            _deptId = deptid;
            _deptName = deptname;
            _wardId = wardid;
            _wardName = wardname;
            _relateDeptIds = null;
            _display = null;
            _mergeSameWard = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            string all = DeptId + WardId;
            return all.GetHashCode();
        }

        /// <summary>
        /// 重载判等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is DeptWardInfo)
            {
                DeptWardInfo dwi = (DeptWardInfo)obj;
                return (dwi.DeptId == DeptId && dwi.DeptName == DeptName
                    && dwi.WardId == WardId && dwi.WardName == WardName);
            }
            return false;
        }

        /// <summary>
        /// 重载==
        /// </summary>
        /// <param name="dept1"></param>
        /// <param name="dept2"></param>
        /// <returns></returns>
        public static bool operator ==(DeptWardInfo dept1, DeptWardInfo dept2)
        {
            //if (dept1 == null)
            //{
            //   if (dept2 == null) return true;
            //   else return false;
            //}
            //else
            return dept1.Equals(dept2);
        }

        /// <summary>
        /// 重载!=
        /// </summary>
        /// <param name="dept1"></param>
        /// <param name="dept2"></param>
        /// <returns></returns>
        public static bool operator !=(DeptWardInfo dept1, DeptWardInfo dept2)
        {
            return !(dept1 == dept2);
        }

        public override string ToString()
        {
            return Display;
        }
    }
}
