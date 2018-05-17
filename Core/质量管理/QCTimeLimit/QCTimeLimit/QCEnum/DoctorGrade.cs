using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Emr.QCTimeLimit.QCEnum
{
    /// <summary>
    /// 医生级别标记
    /// </summary>
    public enum DoctorGrade
    {
        /// <summary>
        /// 特殊的:0表示全部级别
        /// </summary>
        AllGrade = 0,

        /// <summary>
        /// 主任医师
        /// </summary>
        Chief = 2000,

        /// <summary>
        /// 副主任医师
        /// </summary>
        AssociateChief = 2001,

        /// <summary>
        /// 主治医师
        /// </summary>
        Attending = 2002,

        /// <summary>
        /// 住院医师
        /// </summary>
        Resident = 2003,

        /// <summary>
        ///护士 
        /// </summary>
        Nurse = 2004
    }
}
