using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.Core
{
    /// <summary>
    /// ∫∫◊÷µƒÀı–¥¿‡
    /// </summary>
    public class NameAbbreviation
    {
        private string _spellAbbreviation = "";
        private string _wubiAbbreviation = "";

        #region INameAbbreviation Members

        /// <summary>
        /// ∆¥“ÙÀı–¥
        /// </summary>
        public string ABOfSpell
        {
            get
            {
                return _spellAbbreviation;
            }
            set
            {
                _spellAbbreviation = value;
            }
        }

        /// <summary>
        /// ŒÂ± Àı–¥
        /// </summary>
        public string ABOfWubi
        {
            get
            {
                return _wubiAbbreviation;
            }
            set
            {
                _wubiAbbreviation = value;
            }
        }

        #endregion
}
}
