using System;
using System.Collections.Generic;
using System.Text;

namespace HuangF.Sys.Date
{
    /// <summary>
    /// 地支枚举类型
    /// </summary>
    [Serializable]
    public enum DiZhiTypes
    {
        /// <summary>
        /// 子
        /// </summary>
        Zi = 1,

        /// <summary>
        /// 丑
        /// </summary>
        Chou,

        /// <summary>
        /// 寅
        /// </summary>
        Yin,

        /// <summary>
        /// 卯 
        /// </summary>
        Mao,

        /// <summary>
        /// 辰
        /// </summary>
        Chen,

        /// <summary>
        /// 巳
        /// </summary>
        Si,

        /// <summary>
        /// 午
        /// </summary>
        Wu,

        /// <summary>
        /// 未
        /// </summary>
        Wei,

        /// <summary>
        /// 申
        /// </summary>
        Shen,

        /// <summary>
        /// 酉
        /// </summary>
        You,

        /// <summary>
        /// 戌
        /// </summary>
        Xu,

        /// <summary>
        /// 亥
        /// </summary>
        Hai
    }


    /// <summary>
    /// 地支类
    /// </summary>
    [Serializable]
    public sealed class DiZhi
    {
        /// <summary>
        /// 
        /// </summary>
        private static string[] names = { "子", "丑", "寅", "卯", "辰", "巳", "午", "未", "申", "酉", "戌", "亥" };

        /// <summary>
        /// 获取指定地支的中文名称
        /// </summary>
        /// <param name="adz">指定的地支</param>
        /// <returns>地支名称</returns>
        private static string GetName(DiZhiTypes adz)
        {            
            int i = (int)adz;
            return names[i - 1];
        }

        /// <summary>
        /// 所属地支枚举代码
        /// </summary>
        private readonly DiZhiTypes id;


        /// <summary>
        /// 构造函数
        /// </summary>
        public DiZhi()
        {
            id = DiZhiTypes.Zi;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dz">地支枚举值</param>
        public DiZhi(DiZhiTypes dz)
        {
            id = dz;
        }


        /// <summary>
        /// 计算将地支加上i后的值
        /// </summary>
        /// <param name="adz">地支</param>
        /// <param name="i">要增加的数量</param>
        /// <returns>某一地支枚举增加i值后的地支</returns>
        private DiZhiTypes inc(DiZhiTypes adz, int i)
        {
            int j = i % 12;
            if (j == 0) return adz;
            int dz = ((int)adz) + j;
            if (dz > 12)
                dz = dz - 12;
            else if (dz < 1)
                dz = dz + 12;
            return (DiZhiTypes)dz;
        }

        /// <summary>
        /// 获取将当前实例增加i值后的地支实例
        /// </summary>
        /// <param name="i">增加的数值</param>
        /// <returns>地支实例</returns>
        public DiZhi Inc(int i)
        {
            DiZhiTypes dz = inc(id, i);
            return new DiZhi(dz);
        }


        /// <summary>
        /// 获取当前地支下面的一个，如丑的下一个为寅,亥的下一个为子
        /// </summary>
        /// <returns></returns>
        public DiZhi Next()
        {
            return Inc(1);
        }


        /// <summary>
        /// 获取当前地支前面的一个，如寅的前一个为丑,子的前一个为亥
        /// </summary>
        /// <returns></returns>
        public DiZhi Prior()
        {
            return Inc(-1);
        }

        /// <summary>
        /// 地支的中文名称
        /// </summary>
        public string Name
        {
            get { return GetName(id); }
        }

        /// <summary>
        /// 地支的枚举值代码
        /// </summary>
        public DiZhiTypes ID
        {
            get { return id; }
        }

        /// <summary>
        /// 地支的整数值
        /// </summary>
        public int IntValue
        {
            get { return (int)id; }
        }

    }//end of class dizhi

}
