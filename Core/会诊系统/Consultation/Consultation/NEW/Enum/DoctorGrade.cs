using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.Consultation.NEW.Enum
{
    /// <summary>
    /// 医师级别
    /// </summary>
    public enum DoctorGrade
    {
        /// <summary>
        /// 主任
        /// </summary>
        Director = 2000,

        /// <summary>
        /// 副主任
        /// </summary>
        ViceDirector = 2001,

        /// <summary>
        /// 主治医师
        /// </summary>
        StaffPhysician = 2002,

        /// <summary>
        /// 住院医师
        /// </summary>
        Resident = 2003
    }
}
