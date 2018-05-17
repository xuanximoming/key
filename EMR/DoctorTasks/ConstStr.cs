using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.DoctorTasks {
    internal class ConstStr {

        internal const string str_Basicinfo = "Basicinfo";
        /// <summary>
        /// 今日在院病人
        /// </summary>
        internal const string str_InWard = "InWard";
        /// <summary>
        /// 今日出院病人
        /// </summary>
        internal const string str_OutWard = "OutWard";
        /// <summary>
        /// 今日手术病人
        /// </summary>
        internal const string str_OPWard = "OPWard";
        /// <summary>
        /// 今日未归档病人
        /// </summary>
        internal const string str_Archive = "Archive";
        /// <summary>
        /// 今日转科病人
        /// </summary>
        internal const string str_ShiftWard = "ShiftWard";

        /// <summary>
        /// 今日病历时限超时
        /// </summary>
        internal const string str_QcTime = "QcTime";
        /// <summary>
        /// 今日危重病人
        /// </summary>
        internal const string str_Danger = "Danger";

        /// <summary>
        /// 
        /// </summary>
        internal const string field_InTotal = "InTotal";
        /// <summary>
        /// 
        /// </summary>
        internal const string field_OutTotal = "OutTotal";
        /// <summary>
        /// 
        /// </summary>
        internal const string field_ArTotal = "ArTotal";
        /// <summary>
        /// 
        /// </summary>
        internal const string field_OPTotal = "OPTotal";
        /// <summary>
        /// 
        /// </summary>
        internal const string field_ShiftTotal = "ShiftTotal";
        /// <summary>
        /// 
        /// </summary>
        internal const string field_DangerTotal = "DangerTotal";
        /// <summary>
        /// 
        /// </summary>
        internal const string field_QcTotal = "QcTotal";


        internal static string ServerURL
        {
            get
            {
                if (string.IsNullOrEmpty(_serverURl))
                    _serverURl = BasicSettings.GetStringConfig("YindanWebServerUrL");
                return _serverURl;
            }
        }
        private static string _serverURl;


        internal static string CP_ServerURL
        {
            get
            {
                if (string.IsNullOrEmpty(_cp_ServerURl))
                    _cp_ServerURl = BasicSettings.GetStringConfig("YindanCPServerUrL");
                return _cp_ServerURl;
            }
        }
        private static string _cp_ServerURl;




    }
}
