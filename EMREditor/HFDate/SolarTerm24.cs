using System;
using System.Collections.Generic;

using System.Text;

namespace HuangF.Sys.Date
{
    /// <summary>
    /// 二十四节气枚举
    /// </summary>
    public enum SolarTerm24Types
    {
        /// <summary>
        /// 立春
        /// </summary>
        LiChun = 0,

        /// <summary>
        /// 雨水
        /// </summary>
        YuShui,

        /// <summary>
        /// 惊蛰
        /// </summary>
        JingZhe,

        /// <summary>
        /// 春分
        /// </summary>
        ChunFen,

        /// <summary>
        /// 清明
        /// </summary>
        QingMing,

        /// <summary>
        /// 谷雨
        /// </summary>
        GuYu,

        /// <summary>
        /// 立夏
        /// </summary>
        LiXia,

        /// <summary>
        /// 小满
        /// </summary>
        XiaoMan,

        /// <summary>
        /// 芒种
        /// </summary>
        MangZhong,

        /// <summary>
        /// 夏至
        /// </summary>
        XiaZhi,

        /// <summary>
        /// 小暑
        /// </summary>
        XiaoShu,

        /// <summary>
        /// 大暑
        /// </summary>
        DaShu,

        /// <summary>
        /// 立秋
        /// </summary>
        LiQiu,

        /// <summary>
        /// 处暑
        /// </summary>
        ChuShu,

        /// <summary>
        /// 白露
        /// </summary>
        BaiLu,

        /// <summary>
        /// 秋分
        /// </summary>
        QiuFen,

        /// <summary>
        /// 寒露
        /// </summary>
        HanLu,

        /// <summary>
        /// 霜降
        /// </summary>
        ShuangJiang,

        /// <summary>
        /// 立冬
        /// </summary>
        LiDong,

        /// <summary>
        /// 小雪
        /// </summary>
        XiaoXue,

        /// <summary>
        /// 大雪
        /// </summary>
        DaXue,

        /// <summary>
        /// 冬至
        /// </summary>
        DongZhi,

        /// <summary>
        /// 小寒
        /// </summary>
        XiaoHan,

        /// <summary>
        /// 大寒
        /// </summary>
        DaHan
    }


    /// <summary>
    /// 二十四节气类
    /// </summary>
    public class SolarTerm24
    {
        /// <summary>
        /// 节气
        /// </summary>
        private readonly SolarTerm24Types id;


        /// <summary>
        /// 构造函数
        /// </summary>
        public SolarTerm24()
        {
            id = SolarTerm24Types.LiChun;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="jq">指定的节气</param>
        public SolarTerm24(SolarTerm24Types jq)
        {
            id = jq;
        }


        /// <summary>
        /// 二十四节气名称
        /// </summary>
        private static string[] Names = {"立春","雨水","惊蛰","春分","清明","谷雨",
                                         "立夏","小满","芒种","夏至","小暑","大暑",
                                         "立秋","处暑","白露","秋分","寒露","霜降",
                                         "立冬","小雪","大雪","冬至","小寒","大寒"
                                         };

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return Names[(int)id]; }
        }

        /// <summary>
        /// 节气代码枚举值
        /// </summary>
        public SolarTerm24Types ID
        {
            get { return id; }
        }
        
        /// <summary>
        /// 当前节气的整数值
        /// </summary>
        public int IntValue
        {
            get { return (int)ID; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jq"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private SolarTerm24Types Inc(SolarTerm24Types jq, int n)
        {
            int j = n % 24;
            if (j == 0)
                return jq;
            int k = (int)jq + j;
            if (k > 23)
                k -= 24;
            else if (k < 0)
                k += 24;
            return (SolarTerm24Types)k;
        }

        /// <summary>
        /// 将当前实例加上n后的实例值,n可以为负数
        /// </summary>
        /// <param name="n">增加的数值</param>
        /// <returns></returns>
        public SolarTerm24 Inc(int n)
        {
            SolarTerm24Types jq = Inc(ID, n);
            return new SolarTerm24(jq);
        }

        /// <summary>
        /// 当前节气的下一个节气
        /// </summary>
        /// <returns></returns>
        public SolarTerm24 Next()
        {
            return Inc(1);
        }

        /// <summary>
        /// 当前节气前一个节气
        /// </summary>
        /// <returns></returns>
        public SolarTerm24 Prior()
        {
            return Inc(-1);
        }


        /// <summary>
        /// 节气在黄道上的黄经度数。为春分为0度，每个节/气相差15度
        /// </summary>
        public int Degree
        {
            get
            {
                int i = IntValue - 3;
                if (i < 0) i += 24;
                return i * 15;
            }
        }
    }



    /// <summary>
    /// 使用在HQDate中表示该日的节气信息
    /// </summary>
    public sealed class HFDateSolarTermInfo : SolarTerm24
    {
        /// <summary>
        /// 日期
        /// </summary>
        private readonly DateTime date;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="jq">节气</param>
        /// <param name="rq">日期/时间</param>
        public HFDateSolarTermInfo(SolarTerm24Types jq, DateTime rq)
            : base(jq)
        {
            date = rq;
        }


        /// <summary>
        /// 日期/时间
        /// </summary>
        public DateTime Date
        {
            get { return date; }
        }

    }

}
