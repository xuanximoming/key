using System;
using System.Collections.Generic;
using System.Text;

namespace HuangF.Sys.Date
{
    /// <summary>
    /// 干支类,由天干和地支的组合
    /// </summary>
    [Serializable]
    public class GanZhi
    {
        /// <summary>
        /// 天干
        /// </summary>
        private readonly TianGan _tg;

        /// <summary>
        /// 地支
        /// </summary>
        private readonly DiZhi _dz;

        /// <summary>
        /// 构造函数
        /// </summary>
        public GanZhi()
        {
            _tg = new TianGan(TianGanTypes.Jia);
            _dz = new DiZhi(DiZhiTypes.Zi);
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="atg">天干枚举</param>
        /// <param name="adz">地支枚举</param>
        public GanZhi(TianGanTypes atg, DiZhiTypes adz)
        {
            _tg = new TianGan(atg);
            _dz = new DiZhi(adz);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="atg">天干</param>
        /// <param name="adz">地支</param>
        public GanZhi(TianGan atg, DiZhi adz)
        {
            _tg = atg;
            _dz = adz;
        }

        /// <summary>
        /// 获取增加指定的数值后的干支实例
        /// </summary>
        /// <param name="i">增加的数值</param>
        /// <returns>干支</returns>
        public GanZhi Inc(int i)
        {
            TianGan t = _tg.Inc(i);
            DiZhi d = _dz.Inc(i);
            return new GanZhi(t, d);
        }

        /// <summary>
        /// 天干
        /// </summary>
        public TianGan Gan
        {
            get { return _tg; }
        }


        /// <summary>
        /// 地支
        /// </summary>
        public DiZhi Zhi
        {
            get { return _dz; }
        }

        /// <summary>
        /// 干支的名称
        /// </summary>
        public string Name
        {
            get { return _tg.Name + _dz.Name; }
        }

    }//end of class GanZhi

}
