using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    public class AbstractorFactry
    {
        public static IPrintNurse GetNurseRecord(string id)
        {
            switch (id.ToUpper())
            {
                case "ICUHLJLDIMG":
                    return new ICUTongJiJLD();
                case "HLJLDPRINT20":
                    return new HLJLDQLS();
                case "HLJLDPRINT6":
                case "HLJLDPRINT14":
                case "EKHLJLD":
                case "FKSSHLJLD":
                case "ICUHLJLD":
                case "ICUYCPGB":
                case "ICUYCPGBIMG":
                case "SSKSHLJLDIMG":
                case "TSKSHLJLDIMG":
                case "XSEHLJLD":
                case "SSKSHLJLD":
                case "XSEHLJLDIMG":
                case "YKHLJLDIMG":
                case "EKHLJLDIMG":
                case "FKSSHLJLDIMG":
                case "FSSKSHLJLDIMG":
                case "TSKSHLJLD":
                case "YKHLJLD":
                case "SWFKZLJL":
                    return null;
                default:
                    return new MultiRepeatColumnReport();
            }

        }
    }
}
