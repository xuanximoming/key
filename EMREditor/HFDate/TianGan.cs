using System;
using System.Collections.Generic;
using System.Text;

namespace HuangF.Sys.Date
{
    /// <summary>
    /// 天干枚举类型
    /// </summary>
    [Serializable]
    public enum TianGanTypes
    {
        /// <summary>
        /// 甲
        /// </summary>
        Jia = 1,

        /// <summary>
        /// 乙
        /// </summary>
        Yi,

        /// <summary>
        /// 丙
        /// </summary>
        Bing,

        /// <summary>
        /// 丁
        /// </summary>
        Ding,

        /// <summary>
        /// 戊
        /// </summary>
        Wu,

        /// <summary>
        /// 己
        /// </summary>
        Ji,

        /// <summary>
        /// 庚
        /// </summary>
        Geng,

        /// <summary>
        /// 辛
        /// </summary>
        Xin,

        /// <summary>
        /// 壬
        /// </summary>
        Ren,

        /// <summary>
        /// 癸
        /// </summary>
        Gui
    }


    /// <summary>
    /// 天干类
    /// </summary>
    [Serializable]
    public sealed class TianGan
    {
        /// <summary>
        /// 返回天干的中文名称
        /// </summary>
        /// <param name="atg">天干枚举值</param>
        /// <returns>天干的中文名称</returns>
        private static string GetName(TianGanTypes atg)
        {
            string[] s ={ "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸" };
            int i = (int)atg;
            return s[i - 1];
        }


        /// <summary>
        /// 计算将天干加上i后的值
        /// </summary>
        /// <param name="atg">天干</param>
        /// <param name="i">要增加的数量</param>
        /// <returns>某一天干枚举增加i值后的天干</returns>
        private TianGanTypes inc(TianGanTypes atg, int i)
        {
            int j = i % 10;
            if (j == 0) return atg;
            int tg = ((int)atg) + j;
            if (tg > 10)
                tg = tg - 10;
            else if (tg < 1)
                tg = tg + 10;
            return (TianGanTypes)tg;
        }

        /// <summary>
        /// 获取将当前天干加(减)指定数值后的天干
        /// </summary>
        /// <param name="i">加减数值</param>
        /// <returns>新天干实例</returns>
        public TianGan Inc(int i)
        {
            TianGanTypes tg = inc(id, i);
            return new TianGan(tg);
        }

        /// <summary>
        /// 获取当前天干的下一个
        /// </summary>
        /// <returns></returns>
        public TianGan Next()
        {
            return Inc(1);
        }

        /// <summary>
        /// 获取当前天干的前一个
        /// </summary>
        /// <returns></returns>
        public TianGan Prior()
        {
            return Inc(-1);
        }

        /// <summary>
        /// 该类的所属天干枚举值
        /// </summary>
        private readonly TianGanTypes id;

        /// <summary>
        /// 该类的所属天干枚举值
        /// </summary>
        public TianGanTypes ID
        {
            get { return id; }
        }

        /// <summary>
        /// 天干的整数代码
        /// </summary>
        public int IntValue
        {
            get { return (int)id; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="aid">天干的枚举值</param>
        public TianGan(TianGanTypes aid)
        {
            id = aid;
        }

        /// <summary>
        /// 构造函数，默认构造函数将类实例化为“甲”
        /// </summary>
        public TianGan()
        {
            id = TianGanTypes.Jia;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="aid">天干的整数代码</param>
        public TianGan(int aid)
        {
            int i = aid % 10;
            if (i > 0)
                this.id = (TianGanTypes)i;
            else if (i < 0)
                this.id = (TianGanTypes)(10 + i);
            else
                this.id = TianGanTypes.Gui;
        }

        /// <summary>
        /// 天干的中文名称
        /// </summary>
        public string Name
        {
            get { return GetName(id); }
        }

    }//end of class TTianGan

}
