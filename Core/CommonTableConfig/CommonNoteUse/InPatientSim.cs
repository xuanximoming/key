using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    /// <summary>
    /// 病人实体类部分
    /// 功能需要暂时取需要的字段
    /// xlb 2013-01-18
    /// </summary>
    public class InPatientSim
    {
      

        
        /// <summary>
        /// 病人首页序号
        /// </summary>
        public Decimal NoofInpat { get; set; }

        /// <summary>
        /// 病人姓名
        /// </summary>
        public string Name { get; set;}

        /// <summary>
        /// 病人出院科室
        /// </summary>
        public string OutHosDept { get; set;}

        /// <summary>
        /// 病人出院病区
        /// </summary>
        public string OutHosWard { get; set;}

        /// <summary>
        /// 病人出院病床
        /// </summary>
        public string OutBed { get; set; }

        /// <summary>
        /// 病人住院号
        /// </summary>
        public string PatId { get; set;}

    }
}
