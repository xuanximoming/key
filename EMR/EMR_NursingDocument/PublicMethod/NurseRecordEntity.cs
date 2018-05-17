using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.EMR_NursingDocument.PublicMethod
{
    public class NurseRecordEntity
    {
        private string _ID;    //主键 标识符
        private string _NOOFINPATENT;      //病人ID
        private string _FATHER_RECORDID;      //父单据ID 手术科室 还是非手术科室等
        private string _RECORD_DATETIME;      //记录时间
        private string _TIWEN;      //体温 T oC
        private string _MAIBO;      //脉搏 P 次/分
        private string _HUXI;      //呼吸 R 次/f分
        private string _XUEYA;      //血压 BP mmHg
        private string _YISHI;      //意识
        private string _XYBHD;      //血氧饱和度 SPO2
        private string _QKFL;      //切口敷料
        private string _SYPF;      //受压皮肤
        private string _OTHER_ONE;      //其他1 空列
        private string _OTHER_TWO;      //其他2 空列
        private string _OTHER_THREE;      //其他 3 暂时不用
        private string _OTHER_FOUR;      //其他4 暂时不用
        private string _ZTKDX;      //左瞳孔大小mm
        private string _YTKDX;      //右瞳孔大小mm
        private string _TKDGFS;      //瞳孔对光反射
        private string _WOWEI;      //卧位
        private string _JMZG;      //静脉置管
        private string _DGJYLG_ONE;      //导管及引流管1
        private string _DGJYLG_TWO;      //导管及引流管2
        private string _DGJYLG_THREE;      //导管及引流管3
        private string _In_ITEM;      //入量项目
        private string _In_VALUE;      //入量值
        private string _OUT_ITEM;      //      出量项目
        private string _OUT_VALUE;      //出量值
        private string _OUT_COLOR;      //出量颜色
        private string _OUT_STATUE;      //出量性状
        private string _OTHER;      //其他
        private string _HXMS;      //呼吸模式
        private string _FCIMIAO;      //F 次/秒
        private string _XRYND;      //FiO2% 吸入氧浓度
        private string _CGSD;      //插管深度cm
        private string _LXXZYTQ;      //CPAP 连续性正压通气
        private string _BDG;      //鼻导管L/min
        private string _SINGE_DOCTOR;      //签名医生名称
        private string _SINGE_DOCTORID;      //签名医生ID
        private string _CREATE_DOCTORID;      //创建医生ID
        private string _CREATE_TIME;      //创建时间
        private string _VALID;      //启用标示 0  1


        /// <summary>
        /// 主键 标识符
        /// </summary>
        public virtual string ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }
        /// <summary>
        /// 病人ID
        /// </summary>
        public virtual string NOOFINPATENT
        {
            get
            {
                return _NOOFINPATENT;
            }
            set
            {
                _NOOFINPATENT = value;
            }
        }
        /// <summary>
        ///父单据ID 手术科室 还是非手术科室等
        /// </summary>
        public virtual string FATHER_RECORDID
        {
            get
            {
                return _FATHER_RECORDID;
            }
            set
            {
                _FATHER_RECORDID = value;
            }
        }
        /// <summary>
        ///记录时间
        /// </summary>
        public virtual string RECORD_DATETIME
        {
            get
            {
                return _RECORD_DATETIME;
            }
            set
            {
                _RECORD_DATETIME = value;
            }
        }
        /// <summary>
        /// 体温 T oC
        /// </summary>
        public virtual string TIWEN
        {
            get
            {
                return _TIWEN;
            }
            set
            {
                _TIWEN = value;
            }
        }
        /// <summary>
        /// 脉搏 P 次/分
        /// </summary>
        public virtual string MAIBO
        {
            get
            {
                return _MAIBO;
            }
            set
            {
                _MAIBO = value;
            }
        }
        /// <summary>
        /// 呼吸 R 次/f分
        /// </summary>
        public virtual string HUXI
        {
            get
            {
                return _HUXI;
            }
            set
            {
                _HUXI = value;
            }
        }
        /// <summary>
        /// 血压 BP mmHg
        /// </summary>
        public virtual string XUEYA
        {
            get
            {
                return _XUEYA;
            }
            set
            {
                _XUEYA = value;
            }
        }
        /// <summary>
        /// 意识
        /// </summary>
        public virtual string YISHI
        {
            get
            {
                return _YISHI;
            }
            set
            {
                _YISHI = value;
            }
        }
        /// <summary>
        ///血氧饱和度 SPO2
        /// </summary>
        public virtual string XYBHD
        {
            get
            {
                return _XYBHD;
            }
            set
            {
                _XYBHD = value;
            }
        }
        /// <summary>
        /// 切口敷料
        /// </summary>
        public virtual string QKFL
        {
            get
            {
                return _QKFL;
            }
            set
            {
                _QKFL = value;
            }
        }
        /// <summary>
        /// 受压皮肤
        /// </summary>
        public virtual string SYPF
        {
            get
            {
                return _SYPF;
            }
            set
            {
                _SYPF = value;
            }
        }
        /// <summary>
        /// 其他1 空列
        /// </summary>
        public virtual string OTHER_ONE
        {
            get
            {
                return _OTHER_ONE;
            }
            set
            {
                _OTHER_ONE = value;
            }
        }
        /// <summary>
        /// 其他1 空列
        /// </summary>
        public virtual string OTHER_TWO
        {
            get
            {
                return _OTHER_TWO;
            }
            set
            {
                _OTHER_TWO = value;
            }
        }
        /// <summary>
        /// 其他 3 暂时不用
        /// </summary>
        public virtual string OTHER_THREE
        {
            get
            {
                return _OTHER_THREE;
            }
            set
            {
                _OTHER_THREE = value;
            }
        }
        /// <summary>
        /// 其他4 暂时不用
        /// </summary>
        public virtual string OTHER_FOUR
        {
            get
            {
                return _OTHER_FOUR;
            }
            set
            {
                _OTHER_FOUR = value;
            }
        }
        /// <summary>
        /// 左瞳孔大小mm
        /// </summary>
        public virtual string ZTKDX
        {
            get
            {
                return _ZTKDX;
            }
            set
            {
                _ZTKDX = value;
            }
        }
        /// <summary>
        /// 右瞳孔大小mm
        /// </summary>
        public virtual string YTKDX
        {
            get
            {
                return _YTKDX;
            }
            set
            {
                _YTKDX = value;
            }
        }
        /// <summary>
        /// 瞳孔对光反射
        /// </summary>
        public virtual string TKDGFS
        {
            get
            {
                return _TKDGFS;
            }
            set
            {
                _TKDGFS = value;
            }
        }
        /// <summary>
        /// 卧位
        /// </summary>
        public virtual string WOWEI
        {
            get
            {
                return _WOWEI;
            }
            set
            {
                _WOWEI = value;
            }
        }
        /// <summary>
        /// 静脉置管
        /// </summary>
        public virtual string JMZG
        {
            get
            {
                return _JMZG;
            }
            set
            {
                _JMZG = value;
            }
        }
        /// <summary>
        /// 导管及引流管1
        /// </summary>
        public virtual string DGJYLG_ONE
        {
            get
            {
                return _DGJYLG_ONE;
            }
            set
            {
                _DGJYLG_ONE = value;
            }
        }
        /// <summary>
        /// 导管及引流管2
        /// </summary>
        public virtual string DGJYLG_TWO
        {
            get
            {
                return _DGJYLG_TWO;
            }
            set
            {
                _DGJYLG_TWO = value;
            }
        }
        /// <summary>
        /// 导管及引流管3
        /// </summary>
        public virtual string DGJYLG_THREE
        {
            get
            {
                return _DGJYLG_THREE;
            }
            set
            {
                _DGJYLG_THREE = value;
            }
        }
        /// <summary>
        /// 入量项目
        /// </summary>
        public virtual string In_ITEM
        {
            get
            {
                return _In_ITEM;
            }
            set
            {
                _In_ITEM = value;
            }
        }
        /// <summary>
        /// 入量值
        /// </summary>
        public virtual string In_VALUE
        {
            get
            {
                return _In_VALUE;
            }
            set
            {
                _In_VALUE = value;
            }
        }
        /// <summary>
        /// 出量项目
        /// </summary>
        public virtual string OUT_ITEM
        {
            get
            {
                return _OUT_ITEM;
            }
            set
            {
                _OUT_ITEM = value;
            }
        }
        /// <summary>
        /// 出量值
        /// </summary>
        public virtual string OUT_VALUE
        {
            get
            {
                return _OUT_VALUE;
            }
            set
            {
                _OUT_VALUE = value;
            }
        }
        /// <summary>
        /// 出量颜色
        /// </summary>
        public virtual string OUT_COLOR
        {
            get
            {
                return _OUT_COLOR;
            }
            set
            {
                _OUT_COLOR = value;
            }
        }
        /// <summary>
        /// 出量性状
        /// </summary>
        public virtual string OUT_STATUE
        {
            get
            {
                return _OUT_STATUE;
            }
            set
            {
                _OUT_STATUE = value;
            }
        }
        /// <summary>
        /// 其他
        /// </summary>
        public virtual string OTHER
        {
            get
            {
                return _OTHER;
            }
            set
            {
                _OTHER = value;
            }
        }
        /// <summary>
        /// 呼吸模式
        /// </summary>
        public virtual string HXMS
        {
            get
            {
                return _HXMS;
            }
            set
            {
                _HXMS = value;
            }
        }
        /// <summary>
        /// F 次/秒
        /// </summary>
        public virtual string FCIMIAO
        {
            get
            {
                return _FCIMIAO;
            }
            set
            {
                _FCIMIAO = value;
            }
        }
        /// <summary>
        /// FiO2% 吸入氧浓度
        /// </summary>
        public virtual string XRYND
        {
            get
            {
                return _XRYND;
            }
            set
            {
                _XRYND = value;
            }
        }
        /// <summary>
        /// 插管深度cm
        /// </summary>
        public virtual string CGSD
        {
            get
            {
                return _CGSD;
            }
            set
            {
                _CGSD = value;
            }
        }
        /// <summary>
        /// CPAP 连续性正压通气
        /// </summary>
        public virtual string LXXZYTQ
        {
            get
            {
                return _LXXZYTQ;
            }
            set
            {
                _LXXZYTQ = value;
            }
        }
        /// <summary>
        /// 鼻导管L/min
        /// </summary>
        public virtual string BDG
        {
            get
            {
                return _BDG;
            }
            set
            {
                _BDG = value;
            }
        }
        /// <summary>
        /// 签名医生名称
        /// </summary>
        public virtual string SINGE_DOCTOR
        {
            get
            {
                return _SINGE_DOCTOR;
            }
            set
            {
                _SINGE_DOCTOR = value;
            }
        }
        /// <summary>
        /// 签名医生ID
        /// </summary>
        public virtual string SINGE_DOCTORID
        {
            get
            {
                return _SINGE_DOCTORID;
            }
            set
            {
                _SINGE_DOCTORID = value;
            }
        }
        /// <summary>
        /// 创建医生ID
        /// </summary>
        public virtual string CREATE_DOCTORID
        {
            get
            {
                return _CREATE_DOCTORID;
            }
            set
            {
                _CREATE_DOCTORID = value;
            }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual string CREATE_TIME
        {
            get
            {
                return _CREATE_TIME;
            }
            set
            {
                _CREATE_TIME = value;
            }
        }
        /// <summary>
        /// 启用标示 0  1
        /// </summary>
        public virtual string VALID
        {
            get
            {
                return _VALID;
            }
            set
            {
                _VALID = value;
            }
        }

    }
}
