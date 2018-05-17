using System;


namespace HuangF.Sys.Date
{
    /// <summary>
    /// 计算太阳黄经赤纬所需类型变量
    /// </summary>
    internal struct VSOP87COEFFICIENT
    {
        public double dA;
        public double dB;
        public double dC;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        public VSOP87COEFFICIENT(double a, double b, double c)
        {
            dA = a;
            dB = b;
            dC = c;
        }
    }


    /// <summary>
    /// 二次修正黄经赤纬所需的天体章动系数类型变量
    /// </summary>
    internal struct NUTATIONCOEFFICIENT
    {
        public int nD;
        public int nM;
        public int nMprime;
        public int nF;
        public int nOmega;
        public int nSincoeff1;
        public double dSincoeff2;
        public int nCoscoeff1;
        public double dCoscoeff2;

        public NUTATIONCOEFFICIENT(int d, int m, int mp, int nf, int no, int ns, double ds, int nc, double dc)
        {
            nD = d;
            nM = m;
            nMprime = mp;
            nF = nf;
            nOmega = no;
            nSincoeff1 = ns;
            dSincoeff2 = ds;
            nCoscoeff1 = nc;
            dCoscoeff2 = dc;
        }
    }

    /// <summary>
    /// 计算节气的函数。计算节气所需的数据和算法来源于http://hi.baidu.com/wlzqi/blog/item/3c5679311e5bee19ebc4afc0.html，原作为c++，
    /// 这里将其整理，改为c#
    /// </summary>
    public static class CalendarUtil
    {
        /// <summary>
        /// 园周率
        /// </summary>
        private const double PI = 3.1415926535897932384626433832795;

        /// <summary>
        /// 一度代表的弧度
        /// </summary>
        private const double dbUnitRadian = PI / 180.0;

        /// <summary>
        /// 计算太阳黄经用参数
        /// </summary>
        private static VSOP87COEFFICIENT[] Earth_SLG0 =
         {
            new VSOP87COEFFICIENT( 175347046.0 ,   0.0000000 ,   000000.0000000 ) ,
            new VSOP87COEFFICIENT(  3341656.0 ,   4.6692568 ,     6283.0758500 ) ,
            new VSOP87COEFFICIENT(     34894.0 ,   4.6261000 ,    12566.1517000 ) ,
            new VSOP87COEFFICIENT(     3497.0 ,   2.7441000 ,     5753.3849000 ) ,
            new VSOP87COEFFICIENT(     3418.0 ,   2.8289000 ,        3.5231000 ) ,
            new VSOP87COEFFICIENT(      3136.0 ,   3.6277000 ,    77713.7715000 ) ,
            new VSOP87COEFFICIENT(      2676.0 ,   4.4181000 ,     7860.4194000 ) ,
            new VSOP87COEFFICIENT(      2343.0 ,   6.1352000 ,     3930.2097000 ) ,
            new VSOP87COEFFICIENT(      1324.0 ,   0.7425000 ,    11506.7698000 ) ,
            new VSOP87COEFFICIENT(      1273.0 ,   2.0371000 ,      529.6910000 ) ,
            new VSOP87COEFFICIENT(      1199.0 ,   1.1096000 ,     1577.3435000 ) ,
            new VSOP87COEFFICIENT(       990.0 ,   5.2330000 ,     5884.9270000 ) ,
            new VSOP87COEFFICIENT(       902.0 ,   2.0450000 ,       26.2980000 ) ,
            new VSOP87COEFFICIENT(       857.0 ,   3.5080000 ,      398.1490000 ) ,
            new VSOP87COEFFICIENT(       780.0 ,   1.1790000 ,     5223.6940000 ) ,
            new VSOP87COEFFICIENT(       753.0 ,   2.5330000 ,     5507.5530000 ) ,
            new VSOP87COEFFICIENT(       505.0 ,   4.5830000 ,    18849.2280000 ) ,
            new VSOP87COEFFICIENT(       492.0 ,   4.2050000 ,      775.5230000 ) ,
            new VSOP87COEFFICIENT(       357.0 ,   2.9200000 ,   000000.0670000 ) ,
            new VSOP87COEFFICIENT(       317.0 ,   5.8490000 ,    11790.6290000 ) ,
            new VSOP87COEFFICIENT(       284.0 ,   1.8990000 ,      796.2880000 ) ,
            new VSOP87COEFFICIENT(       271.0 ,   0.3150000 ,    10977.0790000 ) ,
            new VSOP87COEFFICIENT(       243.0 ,   0.3450000 ,     5486.7780000 ) ,
            new VSOP87COEFFICIENT(       206.0 ,   4.8060000 ,     2544.3140000 ) ,
            new VSOP87COEFFICIENT(       205.0 ,   1.8690000 ,     5573.1430000 ) ,
            new VSOP87COEFFICIENT(       202.0 ,   2.4580000 ,     6069.7770000 ) ,
            new VSOP87COEFFICIENT(       156.0 ,   0.8330000 ,      213.2990000 ) ,
            new VSOP87COEFFICIENT(       132.0 ,   3.4110000 ,     2942.4630000 ) ,
            new VSOP87COEFFICIENT(       126.0 ,   1.0830000 ,       20.7750000 ) ,
            new VSOP87COEFFICIENT(       115.0 ,   0.6450000 ,   000000.9800000 ) ,
            new VSOP87COEFFICIENT(       103.0 ,   0.6360000 ,     4694.0030000 ) ,
            new VSOP87COEFFICIENT(       102.0 ,   0.9760000 ,    15720.8390000 ) ,
            new VSOP87COEFFICIENT(       102.0 ,   4.2670000 ,        7.1140000 ) ,
            new VSOP87COEFFICIENT(        99.0 ,   6.2100000 ,     2146.1700000 ) ,
            new VSOP87COEFFICIENT(        98.0 ,   0.6800000 ,      155.4200000 ) ,
            new VSOP87COEFFICIENT(        86.0 ,   5.9800000 ,   161000.6900000 ) ,
            new VSOP87COEFFICIENT(        85.0 ,   1.3000000 ,     6275.9600000 ) ,
            new VSOP87COEFFICIENT(        85.0 ,   3.6700000 ,    71430.7000000 ) ,
            new VSOP87COEFFICIENT(        80.0 ,   1.8100000 ,    17260.1500000 ) ,
            new VSOP87COEFFICIENT(        79.0 ,   3.0400000 ,    12036.4600000 ) ,
            new VSOP87COEFFICIENT(        75.0 ,   1.7600000 ,     5088.6300000 ) ,
            new VSOP87COEFFICIENT(        74.0 ,   3.5000000 ,     3154.6900000 ) ,
            new VSOP87COEFFICIENT(        74.0 ,   4.6800000 ,      801.8200000 ) ,
            new VSOP87COEFFICIENT(        70.0 ,   0.8300000 ,     9437.7600000 ) ,
            new VSOP87COEFFICIENT(        62.0 ,   3.9800000 ,     8827.3900000 ) ,
            new VSOP87COEFFICIENT(        61.0 ,   1.8200000 ,     7084.9000000 ) ,
            new VSOP87COEFFICIENT(        57.0 ,   2.7800000 ,     6286.6000000 ) ,
            new VSOP87COEFFICIENT(        56.0 ,   4.3900000 ,    14143.5000000 ) ,
            new VSOP87COEFFICIENT(        56.0 ,   3.4700000 ,     6279.5500000 ) ,
            new VSOP87COEFFICIENT(        52.0 ,   0.1900000 ,    12139.5500000 ) ,
            new VSOP87COEFFICIENT(        52.0 ,   1.3300000 ,     1748.0200000 ) ,
            new VSOP87COEFFICIENT(        51.0 ,   0.2800000 ,     5856.4800000 ) ,
            new VSOP87COEFFICIENT(        49.0 ,   0.4900000 ,     1194.4500000 ) ,
            new VSOP87COEFFICIENT(        41.0 ,   5.3700000 ,     8429.2400000 ) ,
            new VSOP87COEFFICIENT(        41.0 ,   2.4000000 ,    19651.0500000 ) ,
            new VSOP87COEFFICIENT(        39.0 ,   6.1700000 ,    10447.3900000 ) ,
            new VSOP87COEFFICIENT(        37.0 ,   6.0400000 ,    10213.2900000 ) ,
            new VSOP87COEFFICIENT(        37.0 ,   2.5700000 ,     1059.3800000 ) ,
            new VSOP87COEFFICIENT(        36.0 ,   1.7100000 ,     2352.8700000 ) ,
            new VSOP87COEFFICIENT(        36.0 ,   1.7800000 ,     6812.7700000 ) ,
            new VSOP87COEFFICIENT(        33.0 ,   0.5900000 ,    17789.8500000 ) ,
            new VSOP87COEFFICIENT(        30.0 ,   0.4400000 ,    83996.8500000 ) ,
            new VSOP87COEFFICIENT(        30.0 ,   2.7400000 ,     1349.8700000 ) ,
            new VSOP87COEFFICIENT(        25.0 ,   3.1600000 ,     4690.4800000 )
         };

        /// <summary>
        /// 计算太阳黄经用参数
        /// </summary>
        private static VSOP87COEFFICIENT[] Earth_SLG1 =
         {
            new VSOP87COEFFICIENT( 628331966747.0 , 0.000000 ,   00000.0000000 ) ,
            new VSOP87COEFFICIENT(       206059.0 , 2.678235 ,    6283.0758500 ) ,
            new VSOP87COEFFICIENT(         4303.0 , 2.635100 ,   12566.1517000 ) ,
            new VSOP87COEFFICIENT(          425.0 , 1.590000 ,       3.5230000 ) ,
            new VSOP87COEFFICIENT(          119.0 , 5.796000 ,     26.2980000 ) ,
            new VSOP87COEFFICIENT(          109.0 , 2.966000 ,    1577.3440000 ) ,
            new VSOP87COEFFICIENT(           93.0 , 2.590000 ,   18849.2300000 ) ,
            new VSOP87COEFFICIENT(           72.0 , 1.140000 ,     529.6900000 ) ,
            new VSOP87COEFFICIENT(           68.0 , 1.870000 ,     398.1500000 ) ,
            new VSOP87COEFFICIENT(           67.0 , 4.410000 ,    5507.5500000 ) ,
            new VSOP87COEFFICIENT(           59.0 , 2.890000 ,    5223.6900000 ) ,
            new VSOP87COEFFICIENT(           56.0 , 2.170000 ,     155.4200000 ) ,
            new VSOP87COEFFICIENT(           45.0 , 0.400000 ,     796.3000000 ) ,
            new VSOP87COEFFICIENT(           36.0 , 0.470000 ,     775.5200000 ) ,
            new VSOP87COEFFICIENT(           29.0 , 2.650000 ,       7.1100000 ) ,
            new VSOP87COEFFICIENT(           21.0 , 5.430000 ,   00000.9800000 ) ,
            new VSOP87COEFFICIENT(           19.0 , 1.850000 ,    5486.7800000 ) ,
            new VSOP87COEFFICIENT(           19.0 , 4.970000 ,     213.3000000 ) ,
            new VSOP87COEFFICIENT(           17.0 , 2.990000 ,    6275.9600000 ) ,
            new VSOP87COEFFICIENT(           16.0 , 0.030000 ,    2544.3100000 ) ,
            new VSOP87COEFFICIENT(           16.0 , 1.430000 ,    2146.1700000 ) ,
            new VSOP87COEFFICIENT(           15.0 , 1.210000 ,   10977.0800000 ) ,
            new VSOP87COEFFICIENT(           12.0 , 2.830000 ,    1748.0200000 ) ,
            new VSOP87COEFFICIENT(           12.0 , 3.260000 ,    5088.6300000 ) ,
            new VSOP87COEFFICIENT(           12.0 , 5.270000 ,    1194.4500000 ) ,
            new VSOP87COEFFICIENT(           12.0 , 2.080000 ,    4694.0000000 ) ,
            new VSOP87COEFFICIENT(           11.0 , 0.770000 ,     553.5700000 ) ,
            new VSOP87COEFFICIENT(           10.0 , 1.300000 ,    6286.6000000 ) ,
            new VSOP87COEFFICIENT(           10.0 , 4.240000 ,    1349.8700000 ) ,
            new VSOP87COEFFICIENT(            9.0 , 2.700000 ,     242.7300000 ) ,
            new VSOP87COEFFICIENT(            9.0 , 5.640000 ,     951.7200000 ) ,
            new VSOP87COEFFICIENT(            8.0 , 5.300000 ,    2352.8700000 ) ,
            new VSOP87COEFFICIENT(            6.0 , 2.650000 ,    9437.7600000 ) ,
            new VSOP87COEFFICIENT(            6.0 , 4.670000 ,    4690.4800000 )
         };

        /// <summary>
        /// 计算太阳黄经用参数
        /// </summary>
        private static VSOP87COEFFICIENT[] Earth_SLG2 =
        {
            new VSOP87COEFFICIENT( 52919.0 , 0.0000 ,    00000.0000 ) ,
            new VSOP87COEFFICIENT( 8720.0 ,   1.0721 ,    6283.0758 ) ,
            new VSOP87COEFFICIENT(   309.0 ,   0.8670 ,    12566.1520 ) ,
            new VSOP87COEFFICIENT(    27.0 ,   0.0500 ,       3.5200 ) ,
            new VSOP87COEFFICIENT(    16.0 ,   5.1900 ,      26.3000 ) ,
            new VSOP87COEFFICIENT(    16.0 ,   3.6800 ,     155.4200 ) ,
            new VSOP87COEFFICIENT(    10.0 ,   0.7600 ,   18849.2300 ) ,
            new VSOP87COEFFICIENT(     9.0 ,   2.0600 ,   77713.7700 ) ,
            new VSOP87COEFFICIENT(     7.0 ,   0.8300 ,     775.5200 ) ,
            new VSOP87COEFFICIENT(     5.0 ,   4.6600 ,    1577.3400 ) ,
            new VSOP87COEFFICIENT(     4.0 ,   1.0300 ,       7.1100 ) ,
            new VSOP87COEFFICIENT(     4.0 ,   3.4400 ,    5573.1400 ) ,
            new VSOP87COEFFICIENT(     3.0 ,   5.1400 ,     796.3000 ) ,
            new VSOP87COEFFICIENT(     3.0 ,   6.0500 ,    5507.5500 ) ,
            new VSOP87COEFFICIENT(     3.0 ,   1.1900 ,     242.7300 ) ,
            new VSOP87COEFFICIENT(     3.0 ,   6.1200 ,     529.6900 ) ,
            new VSOP87COEFFICIENT(     3.0 ,   0.3100 ,     398.1500 ) ,
            new VSOP87COEFFICIENT(     3.0 ,   2.2800 ,     553.5700 ) ,
            new VSOP87COEFFICIENT(     2.0 ,   4.3800 ,    5223.6900 ) ,
            new VSOP87COEFFICIENT(     2.0 ,   3.7500 ,   00000.9800 )
        };

        /// <summary>
        /// 计算太阳黄经用参数
        /// </summary>
        private static VSOP87COEFFICIENT[] Earth_SLG3 =
         {
            new VSOP87COEFFICIENT( 289.0 , 5.844 , 6283.076 ) ,
            new VSOP87COEFFICIENT( 35.0 ,   0.000 , 00000.000 ) ,
            new VSOP87COEFFICIENT( 17.0 ,   5.490 , 12566.150 ) ,
            new VSOP87COEFFICIENT(   3.0 ,   5.200 ,   155.420 ) ,
            new VSOP87COEFFICIENT(   1.0 ,   4.720 ,     3.520 ) ,
            new VSOP87COEFFICIENT(   1.0 ,   5.300 , 18849.230 ) ,
            new VSOP87COEFFICIENT(   1.0 ,   5.970 ,   242.730 )
         };


        /// <summary>
        /// 计算太阳黄经用参数
        /// </summary>
        private static VSOP87COEFFICIENT[] Earth_SLG4 =
         {
             new VSOP87COEFFICIENT( 114.0 , 3.142 , 00000.00 ) , 
             new VSOP87COEFFICIENT(   8.0 , 4.130 , 6283.08 ) ,
             new VSOP87COEFFICIENT(   1.0 , 3.840 , 12566.15 )
         };


        /// <summary>
        /// 计算太阳黄经用参数
        /// </summary>
        private static VSOP87COEFFICIENT[] Earth_SLG5 =
         {
             new VSOP87COEFFICIENT( 1.0 , 3.14 , 0.0 )
         };


        /// <summary>
        /// 计算太阳黄纬用参数
        /// </summary>
        private static VSOP87COEFFICIENT[] Earth_SLT0 =
        {
              new VSOP87COEFFICIENT( 280.0 , 3.199 , 84334.662 ) ,
              new VSOP87COEFFICIENT( 102.0 , 5.422 , 5507.553 ) ,
              new VSOP87COEFFICIENT( 80.0 , 3.880 , 5223.690 ) ,
              new VSOP87COEFFICIENT( 44.0 , 3.700 , 2352.870 ) ,
              new VSOP87COEFFICIENT( 32.0 , 4.000 , 1577.340 )
         };

        /// <summary>
        /// 计算太阳黄纬用参数
        /// </summary>
        private static VSOP87COEFFICIENT[] Earth_SLT1 =
        {
              new VSOP87COEFFICIENT( 9.0 , 3.90 , 5507.55 ) ,
             new VSOP87COEFFICIENT( 6.0 , 1.73 , 5223.69 )
        };


        /// <summary>
        /// 计算太阳黄纬用参数
        /// </summary>
        private static VSOP87COEFFICIENT[] Earth_SLT2 =
         {
              new VSOP87COEFFICIENT( 22378.0 , 3.38509 , 10213.28555 ) ,
              new VSOP87COEFFICIENT(   282.0 , 0.00000 , 00000.00000 ) ,
              new VSOP87COEFFICIENT(   173.0 , 5.25600 , 20426.57100 ) ,
              new VSOP87COEFFICIENT(    27.0 , 3.87000 , 30639.86000 )
        };



        /// <summary>
        /// 计算太阳黄纬用参数
        /// </summary>
        private static VSOP87COEFFICIENT[] Earth_SLT3 =
         {
              new VSOP87COEFFICIENT( 647.0 , 4.992 , 10213.286 ) ,
              new VSOP87COEFFICIENT( 20.0 , 3.140 , 00000.000 ) ,
              new VSOP87COEFFICIENT(   6.0 , 0.770 , 20426.570 ) ,
              new VSOP87COEFFICIENT(   3.0 , 5.440 , 30639.860 )
         };


        /// <summary>
        /// 计算太阳黄纬用参数
        /// </summary>
        private static VSOP87COEFFICIENT[] Earth_SLT4 =
         {
              new VSOP87COEFFICIENT( 14.0 , 0.32 , 10213.29 )
         };



        /// <summary>
        /// 二次修正黄经黄纬所需的天体章动系数
        /// </summary>
        private static NUTATIONCOEFFICIENT[] Nutation_Gene =
          {
              new NUTATIONCOEFFICIENT( 0, 0, 0, 0, 1, -171996, -174.2, 92025,     8.9    ),
              new NUTATIONCOEFFICIENT( -2, 0, 0, 2, 2, -13187,    -1.6,   5736,    -3.1    ),
              new NUTATIONCOEFFICIENT( 0, 0, 0, 2, 2,   -2274,    -0.2,    977,    -0.5    ),
              new NUTATIONCOEFFICIENT( 0, 0, 0, 0, 2,    2062,     0.2,   -895,     0.5    ),
              new NUTATIONCOEFFICIENT( 0, 1, 0, 0, 0,    1426,    -3.4,     54,    -0.1    ),
              new NUTATIONCOEFFICIENT( 0, 0, 1, 0, 0,     712,     0.1,     -7,       0    ),
              new NUTATIONCOEFFICIENT( -2, 1, 0, 2, 2,    -517,     1.2,    224,    -0.6    ),
              new NUTATIONCOEFFICIENT( 0, 0, 0, 2, 1,    -386,    -0.4,    200,       0    ),
              new NUTATIONCOEFFICIENT( 0, 0, 1, 2, 2,    -301,       0,    129,    -0.1    ),
              new NUTATIONCOEFFICIENT( -2, -1, 0, 2, 2,     217,    -0.5,    -95,     0.3    ),
              new NUTATIONCOEFFICIENT( -2, 0, 1, 0, 0,    -158,       0,      0,       0    ),
              new NUTATIONCOEFFICIENT( -2, 0, 0, 2, 1,     129,     0.1,    -70,       0    ),
              new NUTATIONCOEFFICIENT( 0, 0, -1, 2, 2,     123,       0,    -53,       0    ),
              new NUTATIONCOEFFICIENT( 2, 0, 0, 0, 0,      63,       0,      0,       0    ),
              new NUTATIONCOEFFICIENT( 0, 0, 1, 0, 1,      63,     0.1,    -33,       0    ),
              new NUTATIONCOEFFICIENT( 2, 0, -1, 2, 2,     -59,       0,     26,       0    ),
              new NUTATIONCOEFFICIENT( 0, 0, -1, 0, 1,     -58,    -0.1,     32,       0    ),
              new NUTATIONCOEFFICIENT( 0, 0, 1, 2, 1,     -51,       0,     27,       0    ),
              new NUTATIONCOEFFICIENT( -2, 0, 2, 0, 0,      48,       0,      0,       0    ),
              new NUTATIONCOEFFICIENT( 0, 0, -2, 2, 1,      46,       0,    -24,       0    ),
              new NUTATIONCOEFFICIENT( 2, 0, 0, 2, 2,     -38,       0,     16,       0    ),
              new NUTATIONCOEFFICIENT( 0, 0, 2, 2, 2,     -31,       0,     13,       0    ),
              new NUTATIONCOEFFICIENT( 0, 0, 2, 0, 0,      29,       0,      0,       0    ),
              new NUTATIONCOEFFICIENT( -2, 0, 1, 2, 2,      29,       0,    -12,       0    ),
              new NUTATIONCOEFFICIENT( 0, 0, 0, 2, 0,      26,       0,      0,       0    ),
              new NUTATIONCOEFFICIENT( -2, 0, 0, 2, 0,     -22,       0,      0,       0    ),
              new NUTATIONCOEFFICIENT( 0, 0, -1, 2, 1,      21,       0,    -10,       0    ),
              new NUTATIONCOEFFICIENT( 0, 2, 0, 0, 0,      17,    -0.1,      0,       0    ),
              new NUTATIONCOEFFICIENT( 2, 0, -1, 0, 1,      16,       0,     -8,       0    ),
              new NUTATIONCOEFFICIENT( -2, 2, 0, 2, 2,     -16,     0.1,      7,       0    ),
              new NUTATIONCOEFFICIENT( 0, 1, 0, 0, 1,     -15,       0,      9,       0    ),
              new NUTATIONCOEFFICIENT( -2, 0, 1, 0, 1,     -13,       0,      7,       0    ),
              new NUTATIONCOEFFICIENT( 0, -1, 0, 0, 1,     -12,       0,      6,       0    ),
              new NUTATIONCOEFFICIENT( 0, 0, 2,-2, 0,      11,       0,      0,       0    ),
              new NUTATIONCOEFFICIENT( 2, 0, -1, 2, 1,     -10,       0,      5,       0    ),
              new NUTATIONCOEFFICIENT( 2, 0, 1, 2, 2,      -8,        0,      3,       0    ),
              new NUTATIONCOEFFICIENT( 0, 1, 0, 2, 2,       7,        0,     -3,       0    ),
              new NUTATIONCOEFFICIENT( -2, 1, 1, 0, 0,     -7,       0,      0,       0    ),
              new NUTATIONCOEFFICIENT( 0, -1, 0, 2, 2,      -7,       0,      3,       0    ),
              new NUTATIONCOEFFICIENT( 2, 0, 0, 2, 1,     -7,       0,      3,       0    ),
              new NUTATIONCOEFFICIENT( 2, 0, 1, 0, 0,      6,       0,      0,       0    ),
              new NUTATIONCOEFFICIENT( -2, 0, 2, 2, 2,     6,        0,     -3,       0    ),
              new NUTATIONCOEFFICIENT( -2, 0, 1, 2, 1,     6,        0,     -3,       0    ),
              new NUTATIONCOEFFICIENT( 2, 0, -2, 0, 1,     -6,        0,      3,       0    ),
              new NUTATIONCOEFFICIENT( 2, 0, 0, 0, 1,     -6,        0,      3,       0    ),
              new NUTATIONCOEFFICIENT( 0, -1, 1, 0, 0,     5,        0,      0,       0    ),
              new NUTATIONCOEFFICIENT( -2, -1, 0, 2, 1,     -5,        0,      3,       0    ),
              new NUTATIONCOEFFICIENT( -2, 0, 0, 0, 1,      -5,        0,      3,       0    ),
              new NUTATIONCOEFFICIENT( 0, 0, 2, 2, 1,      -5,        0,      3,       0    ),
              new NUTATIONCOEFFICIENT( -2, 0, 2, 0, 1,       4,        0,      0,       0    ),
              new NUTATIONCOEFFICIENT( -2, 1, 0, 2, 1,       4,        0,      0,       0    ),
              new NUTATIONCOEFFICIENT( 0, 0, 1,-2, 0,       4,        0,      0,       0    ),
              new NUTATIONCOEFFICIENT( -1, 0, 1, 0, 0,     -4,       0,      0,       0    ),
              new NUTATIONCOEFFICIENT( -2, 1, 0, 0, 0,     -4,       0,      0,       0    ),
              new NUTATIONCOEFFICIENT( 1, 0, 0, 0, 0,     -4,       0,      0,       0    ),
              new NUTATIONCOEFFICIENT( 0, 0, 1, 2, 0,     3,       0,      0,       0    ),
              new NUTATIONCOEFFICIENT( 0, 0, -2, 2, 2,     -3,        0,      0,       0    ),
              new NUTATIONCOEFFICIENT( -1, -1, 1, 0, 0,      -3,        0,      0,       0    ),
              new NUTATIONCOEFFICIENT( 0, 1, 1, 0, 0,      -3,        0,      0,       0    ),
              new NUTATIONCOEFFICIENT( 0, -1, 1, 2, 2,      -3,        0,      0,       0    ),
              new NUTATIONCOEFFICIENT( 2, -1, -1, 2, 2,      -3,        0,      0,       0    ),
              new NUTATIONCOEFFICIENT( 0, 0, 3, 2, 2,      -3,        0,      0,       0    ),
              new NUTATIONCOEFFICIENT( 2, -1, 0, 2, 2,     -3,        0,      0,       0    )
          };


        /// <summary>
        /// 计算太阳向量半径用参数
        /// </summary>
        private static VSOP87COEFFICIENT[] Earth_SRV0 =
          {
             new VSOP87COEFFICIENT( 100013989   , 0         ,    0            ),
             new VSOP87COEFFICIENT( 1670700     , 3.0984635 ,    6283.0758500 ),
             new VSOP87COEFFICIENT( 13956       , 3.05525   ,   12566.15170   ),
             new VSOP87COEFFICIENT( 3084        , 5.1985    ,   77713.7715   ),
             new VSOP87COEFFICIENT( 1628        , 1.1739    ,   5753.3849    ),
             new VSOP87COEFFICIENT( 1576        , 2.8469    ,   7860.4194    ),
             new VSOP87COEFFICIENT( 925         , 5.453     ,   11506.770    ),
             new VSOP87COEFFICIENT( 542         , 4.564     ,   3930.210     ),
             new VSOP87COEFFICIENT( 472         , 3.661     ,   5884.927     ),
             new VSOP87COEFFICIENT( 346         , 0.964     ,   5507.553     ),
             new VSOP87COEFFICIENT( 329         , 5.900     ,   5223.694     ),
             new VSOP87COEFFICIENT( 307         , 0.299     ,   5573.143     ),
             new VSOP87COEFFICIENT( 243         , 4.273     ,   11790.629    ),
             new VSOP87COEFFICIENT( 212         , 5.847     ,   1577.344     ),
             new VSOP87COEFFICIENT( 186         , 5.022     ,   10977.079     ),
             new VSOP87COEFFICIENT( 175         , 3.012     ,   18849.228     ),
             new VSOP87COEFFICIENT( 110         , 5.055     ,   5486.778     ),
             new VSOP87COEFFICIENT( 98          , 0.89      ,   6069.78      ),
             new VSOP87COEFFICIENT( 86          , 5.69      ,   15720.84     ),
             new VSOP87COEFFICIENT( 86          , 1.27      ,   161000.69     ),
             new VSOP87COEFFICIENT( 65          , 0.27      ,   17260.15     ),
             new VSOP87COEFFICIENT( 63          , 0.92      ,   529.69       ),
             new VSOP87COEFFICIENT( 57          , 2.01      ,   83996.85     ),
             new VSOP87COEFFICIENT( 56          , 5.24      ,   71430.70     ),
             new VSOP87COEFFICIENT( 49          , 3.25      ,   2544.31      ),
             new VSOP87COEFFICIENT( 47          , 2.58      ,   775.52       ),
             new VSOP87COEFFICIENT( 45          , 5.54      ,   9437.76      ),
             new VSOP87COEFFICIENT( 43          , 6.01      ,   6275.96      ),
             new VSOP87COEFFICIENT( 39          , 5.36      ,   4694.00      ),
             new VSOP87COEFFICIENT( 38          , 2.39      ,   8827.39      ),
             new VSOP87COEFFICIENT( 37          , 0.83      ,   19651.05     ),
             new VSOP87COEFFICIENT( 37          , 4.90      ,   12139.55     ),
             new VSOP87COEFFICIENT( 36          , 1.67      ,   12036.46     ),
             new VSOP87COEFFICIENT( 35          , 1.84      ,   2942.46      ),
             new VSOP87COEFFICIENT( 33          , 0.24      ,   7084.90      ),
             new VSOP87COEFFICIENT( 32          , 0.18      ,   5088.63      ),
             new VSOP87COEFFICIENT( 32          , 1.78      ,   398.15       ),
             new VSOP87COEFFICIENT( 28          , 1.21      ,   6286.60      ),
             new VSOP87COEFFICIENT( 28          , 1.90      ,   6279.55      ),
             new VSOP87COEFFICIENT( 26          , 4.59      ,   10447.39     )
         };

        /// <summary>
        /// 计算太阳向量半径用参数
        /// </summary>
        private static VSOP87COEFFICIENT[] Earth_SRV1 =
         {
             new VSOP87COEFFICIENT( 103019 , 1.107490 , 6283.075850 ),
             new VSOP87COEFFICIENT( 1721   , 1.0644   , 12566.1517 ),
             new VSOP87COEFFICIENT( 702    , 3.142    , 0           ),
             new VSOP87COEFFICIENT( 32     , 1.02     , 18849.23    ),
             new VSOP87COEFFICIENT( 31     , 2.84     , 5507.55     ),
             new VSOP87COEFFICIENT( 25     , 1.32     , 5223.69     ),
             new VSOP87COEFFICIENT( 18     , 1.42     , 1577.34     ),
             new VSOP87COEFFICIENT( 10    , 5.91     , 10977.08    ),
             new VSOP87COEFFICIENT( 9      , 1.42     , 6275.96     ),
             new VSOP87COEFFICIENT( 9      , 0.27     , 5486.78     ) 
         };

        /// <summary>
        /// 计算太阳向量半径用参数
        /// </summary>
        private static VSOP87COEFFICIENT[] Earth_SRV2 =
         {
             new VSOP87COEFFICIENT( 4359 , 5.7846 , 6283.0758 ),
             new VSOP87COEFFICIENT( 124 , 5.579   , 12566.152 ),
             new VSOP87COEFFICIENT( 12   , 3.14   , 0         ),
             new VSOP87COEFFICIENT( 9    , 3.63   , 77713.77 ),
             new VSOP87COEFFICIENT( 6    , 1.87   , 5573.14   ),
             new VSOP87COEFFICIENT( 3    , 5.47   , 18849.23 )
         };

        /// <summary>
        /// 计算太阳向量半径用参数
        /// </summary>
        private static VSOP87COEFFICIENT[] Earth_SRV3 =
         {
             new VSOP87COEFFICIENT( 145 , 4.273 , 6283.076 ),
             new VSOP87COEFFICIENT(   7 , 3.92 , 12566.15 )
         };

        /// <summary>
        /// 计算太阳向量半径用参数
        /// </summary>
        private static VSOP87COEFFICIENT[] Earth_SRV4 =
         {
             new VSOP87COEFFICIENT( 4 , 2.56 , 6283.08 )
         };


        /// <summary>
        /// 计算太阳在黄道面上的经度（单位：度）
        /// </summary>
        /// <param name="dbJD">儒略日（计算该时刻太阳在黄道面上的经度）</param>
        /// <returns>返回太阳黄经度数</returns>
        public static double GetSunLongitude(double dbJD)
        {
            double dt = (dbJD - 2451545.0) / 365250.0;

            double dL = 0.0, dL0 = 0.0, dL1 = 0.0, dL2 = 0.0, dL3 = 0.0, dL4 = 0.0, dL5 = 0.0;

            // L0 38x3
            for (int i = 0; i < Earth_SLG0.Length; i++)
                dL0 += (Earth_SLG0[i].dA * Math.Cos((Earth_SLG0[i].dB + Earth_SLG0[i].dC * dt)));

            for (int i = 0; i < Earth_SLG1.Length; i++)
                dL1 += (Earth_SLG1[i].dA * Math.Cos((Earth_SLG1[i].dB + Earth_SLG1[i].dC * dt)));

            // L2 10x3
            for (int i = 0; i < Earth_SLG2.Length; i++)
                dL2 += (Earth_SLG2[i].dA * Math.Cos((Earth_SLG2[i].dB + Earth_SLG2[i].dC * dt)));

            // L3 8x3
            for (int i = 0; i < Earth_SLG3.Length; i++)
                dL3 += (Earth_SLG3[i].dA * Math.Cos((Earth_SLG3[i].dB + Earth_SLG3[i].dC * dt)));

            // L4 6x3
            for (int i = 0; i < Earth_SLG4.Length; i++)
                dL4 += (Earth_SLG4[i].dA * Math.Cos((Earth_SLG4[i].dB + Earth_SLG4[i].dC * dt)));

            // L5 1x3
            for (int i = 0; i < Earth_SLG5.Length; i++)
                dL5 += (Earth_SLG5[i].dA * Math.Cos((Earth_SLG5[i].dB + Earth_SLG5[i].dC * dt)));

            dL = (dL0 + (dL1 * dt) + (dL2 * (dt * dt)) + (dL3 * (dt * dt * dt)) * (dL4 * (dt * dt * dt * dt)) + (dL5 * (dt * dt * dt * dt * dt))) / 100000000.0;

            return (MapTo0To360Range(MapTo0To360Range(dL / dbUnitRadian) + 180.0));
        }


        /// <summary>
        /// 计算太阳在黄道面上的纬度（单位：度）
        /// </summary>
        /// <param name="dbJD">儒略日（计算该时刻太阳在黄道面上的纬度）</param>
        /// <returns>返回太阳黄纬度数</returns>
        public static double GetSunLatitude(double dbJD)
        {
            double dbt = (dbJD - 2451545.0) / 365250.0;
            double dbB = 0.0, dbB0 = 0.0, dbB1 = 0.0, dbB2 = 0.0, dbB3 = 0.0, dbB4 = 0.0;
            // B0 5x3
            for (int i = 0; i < Earth_SLT0.Length; i++)
                dbB0 += (Earth_SLT0[i].dA * Math.Cos((Earth_SLT0[i].dB + Earth_SLT0[i].dC * dbt)));

            // B1 2x3
            for (int i = 0; i < Earth_SLT1.Length; i++)
                dbB1 += (Earth_SLT1[i].dA * Math.Cos((Earth_SLT1[i].dB + Earth_SLT1[i].dC * dbt)));

            // B2 4x3
            for (int i = 0; i < Earth_SLT2.Length; i++)
                dbB2 += (Earth_SLT2[i].dA * Math.Cos((Earth_SLT2[i].dB + Earth_SLT2[i].dC * dbt)));

            // B3 4x3
            for (int i = 0; i < Earth_SLT3.Length; i++)
                dbB3 += (Earth_SLT3[i].dA * Math.Cos((Earth_SLT3[i].dB + Earth_SLT3[i].dC * dbt)));

            // B4 1x3
            for (int i = 0; i < Earth_SLT4.Length; i++)
                dbB4 += (Earth_SLT4[i].dA * Math.Cos((Earth_SLT4[i].dB + Earth_SLT4[i].dC * dbt)));

            dbB = (dbB0 + (dbB1 * dbt) + (dbB2 * (dbt * dbt)) + (dbB3 * (dbt * dbt * dbt)) * (dbB4 * (dbt * dbt * dbt * dbt))) / 100000000.0;
            return -(dbB / dbUnitRadian);
        }


        /// <summary>
        /// 第一次校正黄经
        /// </summary>
        /// <param name="dbSrcLongitude">黄经</param>
        /// <param name="dbSrcLatitude">黄纬</param>
        /// <param name="dbJD">儒略日</param>
        /// <returns>太阳黄经度数</returns>
        public static double CorrectionCalcSunLongitude(double dbSrcLongitude, double dbSrcLatitude, double dbJD)
        {
            double dbT = (dbJD - 2451545.0) / 36525.0;

            double dbLdash = dbSrcLongitude - 1.397 * dbT - 0.00031 * dbT * dbT;

            // 转换为弧度
            dbLdash *= dbUnitRadian;
            return (-0.09033 + 0.03916 * (Math.Cos(dbLdash) + Math.Sin(dbLdash)) * Math.Tan(dbSrcLatitude * dbUnitRadian)) / 3600.0;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dLongitude"></param>
        /// <param name="dJD"></param>
        /// <returns></returns>
        public static double CorrectionCalcSunLatitude(double dLongitude, double dJD)
        {
            double dT = (dJD - 2451545.0) / 36525.0;
            double dLdash = dLongitude - 1.397 * dT - 0.00031 * dT * dT;

            // 转换为弧度
            dLdash *= dbUnitRadian;
            return (0.03916 * (Math.Cos(dLdash) - Math.Sin(dLdash))) / 3600.0;
        }


        /// <summary>
        /// 计算天体章动。二次修正某时刻太阳在黄道上的纬度（单位：度）。使用天体章动系数修正，消除扰动影响
        /// </summary>
        /// <param name="dbJD">儒略日</param>
        /// <returns>返回天体扰动干扰量</returns>
        public static double GetNutationJamScalar(double dbJD)
        {
            double dbT = (dbJD - 2451545.0) / 36525.0;
            double dbTsquared = dbT * dbT;
            double dbTcubed = dbTsquared * dbT;

            double dbD = 297.85036 + 445267.111480 * dbT - 0.0019142 * dbTsquared + dbTcubed / 189474.0;
            dbD = MapTo0To360Range(dbD);

            double dbM = 357.52772 + 35999.050340 * dbT - 0.0001603 * dbTsquared - dbTcubed / 300000.0;
            dbM = MapTo0To360Range(dbM);

            double dbMprime = 134.96298 + 477198.867398 * dbT + 0.0086972 * dbTsquared + dbTcubed / 56250.0;
            dbMprime = MapTo0To360Range(dbMprime);

            double dbF = 93.27191 + 483202.017538 * dbT - 0.0036825 * dbTsquared + dbTcubed / 327270.0;
            dbF = MapTo0To360Range(dbF);

            double dbOmega = 125.04452 - 1934.136261 * dbT + 0.0020708 * dbTsquared + dbTcubed / 450000.0;
            dbOmega = MapTo0To360Range(dbOmega);

            double dbResulte = 0.0;

            for (int i = 0; i < Nutation_Gene.Length; i++)
            {
                double dbRadargument = (Nutation_Gene[i].nD * dbD + Nutation_Gene[i].nM * dbM + Nutation_Gene[i].nMprime * dbMprime + Nutation_Gene[i].nF * dbF + Nutation_Gene[i].nOmega * dbOmega) * dbUnitRadian;

                dbResulte += (Nutation_Gene[i].nSincoeff1 + Nutation_Gene[i].dSincoeff2 * dbT) * Math.Sin(dbRadargument) * 0.0001;
            }

            return dbResulte;
        }


        /// <summary>
        /// 计算某时刻太阳半径向量
        /// 三次修正某时刻太阳在黄道上的经度（单位：弧度）
        /// </summary>
        /// <param name="dbJD">儒略日</param>
        /// <returns>返回太阳半径向量</returns>
        public static double GetSunRadiusVector(double dbJD)
        {
            double dbt = (dbJD - 2451545.0) / 365250.0;

            double dbR = 0.0, dbR0 = 0.0, dbR1 = 0.0, dbR2 = 0.0, dbR3 = 0.0, dbR4 = 0.0;
            // R0 40x3
            for (int i = 0; i < Earth_SRV0.Length; i++)
                dbR0 += (Earth_SRV0[i].dA * Math.Cos((Earth_SRV0[i].dB + Earth_SRV0[i].dC * dbt)));

            // R1 10x3
            for (int i = 0; i < Earth_SRV1.Length; i++)
                dbR1 += (Earth_SRV1[i].dA * Math.Cos((Earth_SRV1[i].dB + Earth_SRV1[i].dC * dbt)));

            // R2 6x3
            for (int i = 0; i < Earth_SRV2.Length; i++)
                dbR2 += (Earth_SRV2[i].dA * Math.Cos((Earth_SRV2[i].dB + Earth_SRV2[i].dC * dbt)));

            // R3 2x3
            for (int i = 0; i < Earth_SRV3.Length; i++)
                dbR3 += (Earth_SRV3[i].dA * Math.Cos((Earth_SRV3[i].dB + Earth_SRV3[i].dC * dbt)));

            // R4 1x3
            for (int i = 0; i < Earth_SRV4.Length; i++)
                dbR4 += (Earth_SRV4[i].dA * Math.Cos((Earth_SRV4[i].dB + Earth_SRV4[i].dC * dbt)));

            // 计算 R = ( R0 + R1 * τ^1 + R2 * τ^2 + R3 * τ^3 + R4 * τ^4 ) / 10^8 ;（单位弧度）
            return ((dbR0 + (dbR1 * dbt) + (dbR2 * (dbt * dbt)) + (dbR3 * (dbt * dbt * dbt)) * (dbR4 * (dbt * dbt * dbt * dbt))) / 100000000.0);
        }


        /// <summary>
        /// 计算某时刻太阳黄经黄纬
        /// </summary>
        /// <param name="dbJD">儒略日</param>
        /// <param name="dbLongitude">黄经</param>
        /// <param name="dbLatitude">黄纬</param>
        public static void CalcEclipticLongLat(double dbJD, out double dbLongitude, out double dbLatitude)
        {
            // 计算太阳黄经
            dbLongitude = GetSunLongitude(dbJD);
            // 计算太阳黄纬
            dbLatitude = GetSunLatitude(dbJD);

            // 一次校正经度
            dbLongitude += CorrectionCalcSunLongitude(dbLongitude, dbLatitude, dbJD);
            // 二次校正天体章动
            dbLongitude += GetNutationJamScalar(dbJD) / 3600.0;
            // 三次校正太阳半径向量
            dbLongitude -= (20.4898 / GetSunRadiusVector(dbJD)) / 3600.0;

            // 校正太阳黄纬
            dbLatitude += CorrectionCalcSunLatitude(dbLongitude, dbJD);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private static double Floor(double v)
        {
            return Math.Floor(v);
        }

        /// <summary>
        /// 计算儒略日
        /// </summary>
        /// <param name="y">年</param>
        /// <param name="m">月</param>
        /// <param name="d">日</param>
        /// <param name="h">时</param>
        /// <param name="min">分</param>
        /// <param name="s">秒</param>
        /// <returns>返回对应的儒略日</returns>
        public static double Julday(int y, int m, int d, int h, int min, int s)
        {
            int nY = y, nM = m;
            double dD = (double)d + (h / 24.0) + (min / 60.0) / 24.0 + ((s / 60.0) / 60.0) / 24.0;
            if (m == 1 || m == 2)
            {
                nY = y - 1;
                nM = m + 12;
            }
            int nA = nY / 100;
            int nB = 2 - nA + (nA >> 2);
            return (double)((int)(365.25 * (nY + 4716)) + (int)(30.6001 * (nM + 1)) + dD + nB - 1524.5);
        }


        /// <summary>
        /// 计算整数儒略日
        /// </summary>
        /// <param name="y">年。小于0为公元前,大于0为公元年</param>
        /// <param name="m">月</param>
        /// <param name="d">日</param>
        /// <returns>整数儒略日</returns>
        public static int Julday(int y, int m, int d)
        {
            const int IGREG = 15 + 31 * (10 + 12 * 1582);//以1582.10.15中午12点为基准
            int ja, jul, jy = y, jm;
            if (jy < 0) ++jy;
            if (m > 2)
            {
                jm = m + 1;
            }
            else
            {
                --jy;
                jm = m + 13;
            }
            jul = (int)(Floor(365.25 * jy) + Floor(30.6001 * jm) + d + 1720995);
            if (d + 31 * (m + 12 * y) >= IGREG)
            {
                ja = (int)(0.01 * jy);
                jul += (2 - ja + (int)(0.25 * ja));
            }
            return jul;
        }


        /// <summary>
        /// 将儒略日转换为格历日
        /// </summary>
        /// <param name="julday">儒略日</param>
        /// <param name="y">年</param>
        /// <param name="m">月</param>
        /// <param name="d">日</param>
        /// <param name="h">小时</param>
        /// <param name="min">分</param>
        /// <param name="s">秒</param>
        public static void CalGD(double julday, out int y, out int m, out int d, out int h, out int min, out int s)
        {
            double dJDM = julday + 0.5;
            Int64 ulZ = (Int64)(Floor(dJDM));
            double dF = dJDM - Floor(dJDM);
            Int64 ulA, ulB, ulC, ulD;
            int nE, nQ;
            if (julday < 2299161)
                ulA = ulZ;
            else
            {
                nQ = (int)((ulZ - 1867216.25) / 36524.25);
                ulA = ulZ + 1 + nQ - (Int64)(nQ >> 2);
            }
            ulB = ulA + 1524;
            ulC = (int)((ulB - 122.1) / 365.25);
            ulD = (int)(365.25 * ulC);
            nE = (int)((ulB - ulD) / 30.6001);
            d = (int)(ulB - ulD - (int)(30.6001 * nE));
            h = (int)(Floor(dF * 24.0));
            min = (int)(((dF * 24.0) - Floor(dF * 24.0)) * 60.0);
            s = (int)((((dF * 24.0) * 60.0) - Floor((dF * 24.0) * 60.0)) * 60.0);
            m = 0;
            y = 0;
            if (nE < 14)
                m = nE - 1;
            if (nE == 14 || nE == 15)
                m = nE - 13;

            if (m > 2)
                y = (int)(ulC - 4716);
            else if (m == 1 || m == 2)
                y = (int)(ulC - 4715);
        }

        /// <summary>
        /// 将儒略日转换为格历日
        /// </summary>
        /// <param name="julian">儒略日</param>
        /// <param name="y">年</param>
        /// <param name="m">月</param>
        /// <param name="d">日</param>
        public static void CalGD(int julian, out int y, out int m, out int d)
        {
            const int IGREG = 2299161;
            int ja, jalpha, jb, jc, jd, je;
            if (julian >= IGREG)
            {
                jalpha = (int)((((double)(julian - 1867216)) - 0.25) / 36524.25);
                ja = julian + 1 + jalpha - (int)(0.25 * jalpha);
            }
            else if (julian < 0)
            {
                ja = julian + 36525 * (1 - julian / 36525);
            }
            else
                ja = julian;

            jb = ja + 1524;
            jc = (int)(6680.0 + ((double)(jb - 2439870) - 122.1) / 365.25);
            jd = (int)(365 * jc + (0.25 * jc));
            je = (int)((jb - jd) / 30.6001);
            d = jb - jd - (int)(30.6001 * je);
            m = je - 1;
            if (m > 12) m -= 12;
            y = jc - 4715;
            if (m > 2) --y;
            if (y <= 0) --y;
            if (julian < 0) y -= 100 * (1 - julian / 36525);
        }


        /// <summary>
        /// 角度调整，调整角度到 0-360 之间
        /// </summary>
        /// <param name="dbDegrees">角度</param>
        /// <returns>调整后的角度</returns>
        public static double MapTo0To360Range(double dbDegrees)
        {
            double dbValue = dbDegrees;
            while (dbValue < 0.0)
                dbValue += 360.0;

            while (dbValue > 360.0)
                dbValue -= 360.0;
            return dbValue;
        }


        /// <summary>
        /// 计算指定节气的时间
        /// </summary>
        /// <param name="year">公历年份</param>
        /// <param name="st">节气</param>
        /// <returns>返回指定节气的儒略日时间</returns>
        public static double CalcSolarTerms(int year, SolarTerm24Types st)
        {
            SolarTerm24 jq = new SolarTerm24(st);
            int jq1 = jq.Degree;

            // 节气月份
            int SolarTermsMonth = (int)(Math.Ceiling(((jq1 + 90.0) / 30.0)));
            SolarTermsMonth = SolarTermsMonth > 12 ? SolarTermsMonth - 12 : SolarTermsMonth;

            // 节令的发生日期基本都在每月 3 - 9 号间  update by Ukey 2017-01-01 从4-9提升到3-9，有年的立春在3号
            int LowerLimitSolarTermsDay = jq1 % 15 == 0 && jq1 % 30 != 0 ? 3 : 16;
            // 节气的发生日期基本都在每月 16 - 24 号间
            int UpperLimitSolarTermsDay = jq1 % 15 == 0 && jq1 % 30 != 0 ? 9 : 24;

            // 采用二分法逼近计算
            double dbLowerLinit = Julday(year, SolarTermsMonth, LowerLimitSolarTermsDay, 0, 0, 0);
            double dbUpperLinit = Julday(year, SolarTermsMonth, UpperLimitSolarTermsDay, 23, 59, 59);

            // 二分法分界点日期
            double dbDichotomyDivisionDayJD = 0;
            // 太阳黄经角度
            double dbLongitude = 0;

            // 对比二分法精度是否达到要求
            do
            {
                dbDichotomyDivisionDayJD = ((dbUpperLinit - dbLowerLinit) / 2.0) + dbLowerLinit;

                // 计算太阳黄经
                dbLongitude = GetSunLongitude(dbDichotomyDivisionDayJD);

                // 一次校正经度
                dbLongitude += CorrectionCalcSunLongitude(dbLongitude, GetSunLatitude(dbDichotomyDivisionDayJD), dbDichotomyDivisionDayJD);
                // 二次校正天体章动
                dbLongitude += GetNutationJamScalar(dbDichotomyDivisionDayJD) / 3600.0;
                // 三次校正太阳半径向量
                dbLongitude -= (20.4898 / GetSunRadiusVector(dbDichotomyDivisionDayJD)) / 3600.0;

                // 由于春分这天黄经为 0 度，比较特殊，因此专门判断（如不加以特殊对待则会导致计算范围覆盖整个 360 度角）
                dbLongitude = ((st == SolarTerm24Types.ChunFen) && (dbLongitude > 345.0)) ? -dbLongitude : dbLongitude;

                // 调整二分法上下限
                if (dbLongitude > (double)(jq1))
                    dbUpperLinit = dbDichotomyDivisionDayJD;
                else
                    dbLowerLinit = dbDichotomyDivisionDayJD;
            } while (Math.Abs(dbLongitude - (double)(jq1)) >= 0.00001);

            return dbDichotomyDivisionDayJD;
        }


        /// <summary>
        /// 计算指定节气的时间
        /// </summary>
        /// <param name="year">公历年</param>
        /// <param name="st">节气</param>
        /// <returns>返回本地公历日期/时间，精确到秒</returns>
        public static DateTime GetSolarTermDateTime(int year, SolarTerm24Types st)
        {
            double jd = CalcSolarTerms(year, st);
            int y, m, d, min, h, s;
            CalGD(jd, out y, out m, out d, out h, out min, out s);
            DateTime gd = new DateTime(y, m, d, h, min, s, DateTimeKind.Utc);
            return gd.ToLocalTime();
        }

    }
}
