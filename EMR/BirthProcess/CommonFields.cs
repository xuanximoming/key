using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.BirthProcess
{
    /// <summary>
    /// 该抽象类定义了表字段的映射常量，便于集中管理
    /// </summary>
    abstract class CommonFields
    {
        public const string ID1 = "ID";                     
        public const string NOOFINPAT="NOOFINPAT";
        public const string PREVIOUSOBSTETRICHISTORY = "PREVIOUSOBSTETRICHISTORY";
        public const string UTERUSCONTRACTIONTIME = "UTERUSCONTRACTIONTIME";
        public const string VALID = "VALID";
        public const string MODIFYTIME = "MODIFYTIME";
        public const string MODIFIER = "MODIFIER";
        
        public const string ID2="ID";
        public const string STARTTIME = "STARTTIME";
        public const string CHECKTIME = "CHECKTIME";
        public const string UTERUSEXPANDVOLUMN = "UTERUSEXPANDVOLUMN";
        public const string FETUSHEADDROPVOLUMN = "FETUSHEADDROPVOLUMN";
        public const string CHECKMETHOD = "CHECKMETHOD";
        public const string DELIVERED = "DELIVERED";
        public const string BLOODPRESSURE = "BLOODPRESSURE";
        public const string FETUSPOSITION = "FETUSPOSITION";
        public const string FETUSHEART = "FETUSHEART";
        public const string UTERUSSTRENGTH = "UTERUSSTRENGTH";
        public const string UTERUSLAST = "UTERUSLAST";
        public const string UTERUSGAP = "UTERUSGAP";
        public const string FETUSMEMBRANE = "FETUSMEMBRANE";
        public const string DOCTORSIGN = "DOCTORSIGN";
        public const string RECORDER = "RECORDER";
        public const string RECORDTIME = "RECORDTIME";
        public const string NOTE = "NOTE";           
    }
}
