using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.CommonTableConfig
{
    public class EmrPrintTempEntity
    {

        private string _PrintFlow;
        private string _PrintName;
        private string _PrintFileName;
        private string _PrintContent;
        private string _CreateDoctorID;
        private string _CreateDoctorName;
        private string _CreateDateTime;
        private string _IsValide;
        private byte[] _PrintContentbyte;

        public byte[] PrintContentbyte
        {
            get { return _PrintContentbyte; }
            set { _PrintContentbyte = value; }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示 XXXX
        /// </summary>
        public virtual string PrintFlow
        {
            get
            {
                return _PrintFlow;
            }
            set
            {
                _PrintFlow = value;
            }
        }
        /// <summary>
        /// 获取或设置一个值，该值指示 XXXX
        /// </summary>
        public virtual string PrintName
        {
            get
            {
                return _PrintName;
            }
            set
            {
                _PrintName = value;
            }
        }
        /// <summary>
        /// 获取或设置一个值，该值指示 XXXX
        /// </summary>
        public virtual string PrintFileName
        {
            get
            {
                return _PrintFileName;
            }
            set
            {
                _PrintFileName = value;
            }
        }
        /// <summary>
        /// 获取或设置一个值，该值指示 XXXX
        /// </summary>
        public virtual string PrintContent
        {
            get
            {
                return _PrintContent;
            }
            set
            {
                _PrintContent = value;
            }
        }
        /// <summary>
        /// 获取或设置一个值，该值指示 XXXX
        /// </summary>
        public virtual string CreateDoctorID
        {
            get
            {
                return _CreateDoctorID;
            }
            set
            {
                _CreateDoctorID = value;
            }
        }
        /// <summary>
        /// 获取或设置一个值，该值指示 XXXX
        /// </summary>
        public virtual string CreateDoctorName
        {
            get
            {
                return _CreateDoctorName;
            }
            set
            {
                _CreateDoctorName = value;
            }
        }
        /// <summary>
        /// 获取或设置一个值，该值指示 XXXX
        /// </summary>
        public virtual string CreateDateTime
        {
            get
            {
                return _CreateDateTime;
            }
            set
            {
                _CreateDateTime = value;
            }
        }
        /// <summary>
        /// 获取或设置一个值，该值指示 XXXX
        /// </summary>
        public virtual string IsValide
        {
            get
            {
                return _IsValide;
            }
            set
            {
                _IsValide = value;
            }
        }

    }
}
