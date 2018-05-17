using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;


namespace DrectSoft.Core.NursingDocuments.UserControls
{
    class Picture
    {
        private Bitmap m_BitmapT1;           // = new Bitmap("图片\\口温.png");
        private Bitmap m_BitmapT2;           // = new Bitmap("图片\\腋温.png");
        private Bitmap m_BitmapT3;           // = new Bitmap("图片\\肛温.png");
        private Bitmap m_BitmapMaiBo;        // = new Bitmap("图片\\脉搏.png");
        private Bitmap m_BitmapXinlv;        // = new Bitmap("图片\\心率.png");
        private Bitmap m_BitmapHuXi;         // = new Bitmap("图片\\呼吸.png");
        private Bitmap m_BitmapHuXiSpecial;  // = new Bitmap("图片\\呼吸机.png");
        private Bitmap m_BitmapWuLiJiangWen; // = new Bitmap("图片\\物理降温.png");
        private Bitmap m_BitmapQiBoQi;       // = new Bitmap("图片\\起搏器.png");
        private Bitmap m_BitmapNursHuXiMaiBo;
        private Bitmap m_BitmapNursMaiBoTiWenGang;
        private Bitmap m_BitmapNursMaiBoTiWenKou;
        private Bitmap m_BitmapNursMaiBoTiWenYe;
        private Bitmap m_BitmapNursTiWenHuXiMaiBo;

        public Bitmap BitmapT1
        {
            get
            {
                return m_BitmapT1;
            }
        }
        public Bitmap BitmapT2
        {
            get
            {
                return m_BitmapT2;
            }
        }
        public Bitmap BitmapT3
        {
            get
            {
                return m_BitmapT3;
            }
        }
        public Bitmap BitmapMaiBo
        {
            get
            {
                return m_BitmapMaiBo;
            }
        }
        public Bitmap BitmapXinlv
        {
            get
            {
                return m_BitmapXinlv;
            }
        }
        public Bitmap BitmapHuXi
        {
            get
            {
                return m_BitmapHuXi;
            }
        }
        public Bitmap BitmapHuXiSpecial
        {
            get
            {
                return m_BitmapHuXiSpecial;
            }
        }
        public Bitmap BitmapWuLiJiangWen
        {
            get
            {
                return m_BitmapWuLiJiangWen;
            }
        }
        public Bitmap BitmapQiBoQi
        {
            get
            {
                return m_BitmapQiBoQi;
            }
        }
        public Bitmap BitmapNursHuXiMaiBo
        {
            get
            {
                return m_BitmapNursHuXiMaiBo;
            }
        }
        public Bitmap BitmapNursMaiBoTiWenGang
        {
            get
            {
                return m_BitmapNursMaiBoTiWenGang;
            }
        }
        public Bitmap BitmapNursMaiBoTiWenKou
        {
            get
            {
                return m_BitmapNursMaiBoTiWenKou;
            }
        }
        public Bitmap BitmapNursMaiBoTiWenYe
        {
            get
            {
                return m_BitmapNursMaiBoTiWenYe;
            }
        }
        public Bitmap BitmapNursTiWenHuXiMaiBo
        {
            get
            {
                return m_BitmapNursTiWenHuXiMaiBo;
            }
        }

        public Picture()
        {
            InitPicture();
        }

        /// <summary>
        /// 初始化绘制时使用的图片
        /// </summary>
        private void InitPicture()
        {
            m_BitmapT1 = (Bitmap)Resources.ResourceManager.GetImage(Resources.ResourceNames.NursKouWen); //口温
            m_BitmapT2 = (Bitmap)Resources.ResourceManager.GetImage(Resources.ResourceNames.NursYeXia); //腋温
            m_BitmapT3 = (Bitmap)Resources.ResourceManager.GetImage(Resources.ResourceNames.NursGangWen); //肛温
            m_BitmapMaiBo = (Bitmap)Resources.ResourceManager.GetImage(Resources.ResourceNames.NursMaiBo);
            m_BitmapXinlv = (Bitmap)Resources.ResourceManager.GetImage(Resources.ResourceNames.NursXinLv);
            m_BitmapHuXi = (Bitmap)Resources.ResourceManager.GetImage(Resources.ResourceNames.NursHuXi);
            m_BitmapHuXiSpecial = (Bitmap)Resources.ResourceManager.GetImage(Resources.ResourceNames.NursHuXiJi);
            m_BitmapWuLiJiangWen = (Bitmap)Resources.ResourceManager.GetImage(Resources.ResourceNames.NursWuLiJiangWen);
            m_BitmapQiBoQi = (Bitmap)Resources.ResourceManager.GetImage(Resources.ResourceNames.NursQiBoQi);
            m_BitmapNursHuXiMaiBo = (Bitmap)Resources.ResourceManager.GetImage(Resources.ResourceNames.NursHuXiMaiBo);
            m_BitmapNursMaiBoTiWenGang = (Bitmap)Resources.ResourceManager.GetImage(Resources.ResourceNames.NursMaiBoTiWenGang);
            m_BitmapNursMaiBoTiWenKou = (Bitmap)Resources.ResourceManager.GetImage(Resources.ResourceNames.NursMaiBoTiWenKou);
            m_BitmapNursMaiBoTiWenYe = (Bitmap)Resources.ResourceManager.GetImage(Resources.ResourceNames.NursMaiBoTiWenYe);
            m_BitmapNursTiWenHuXiMaiBo = (Bitmap)Resources.ResourceManager.GetImage(Resources.ResourceNames.NursTiWenHuXiMaiBo);

            #region 为了使显示的效果更好，这里将图片中白色的部分置为透明色
            m_BitmapT1.MakeTransparent(Color.White);
            m_BitmapT2.MakeTransparent(Color.White);
            m_BitmapT3.MakeTransparent(Color.White);
            m_BitmapMaiBo.MakeTransparent(Color.White);
            m_BitmapXinlv.MakeTransparent(Color.White);
            m_BitmapHuXi.MakeTransparent(Color.White);
            m_BitmapHuXiSpecial.MakeTransparent(Color.White);
            #endregion
        }

        ///// <summary>
        ///// 体温口温
        ///// </summary>
        ///// <param name="g"></param>
        ///// <param name="x"></param>
        ///// <param name="y"></param>
        //public void DrawKouWen(Graphics g, float x, float y)
        //{
        //    //x = x - 3;
        //    //y = y - 3;
        //    //g.FillEllipse(new SolidBrush(Color.Blue), x + 1f, y + 1f, 12f, 12f);

        //    x = x - 4;
        //    y = y - 4;
        //    g.FillEllipse(new SolidBrush(Color.Blue), x, y, 8f, 8f);
        //}

        ///// <summary>
        ///// 体温腋下
        ///// </summary>
        ///// <param name="g"></param>
        ///// <param name="x"></param>
        ///// <param name="y"></param>
        //public void DrawYeXia(Graphics g, float x, float y)
        //{
        //    //x = x - 3;
        //    //y = y - 3;
        //    //g.FillEllipse(new SolidBrush(Color.White), x + 1f, y + 1f, 10f, 10f);
        //    //g.DrawLine(Pens.Blue, new PointF(x + 1f, y + 1f), new PointF(x + 12f, y + 12f));
        //    //g.DrawLine(Pens.Blue, new PointF(x + 12f, y + 1f), new PointF(x + 1f, y + 12f));

        //    x = x - 3;
        //    y = y - 5;
        //    g.FillEllipse(new SolidBrush(Color.White), x + 1f, y + 1f, 9f, 9f);
        //    g.DrawLine(Pens.Blue, new PointF(x + 1f, y + 1f), new PointF(x + 9f, y + 9f));
        //    g.DrawLine(Pens.Blue, new PointF(x + 9f, y + 1f), new PointF(x + 1f, y + 9f));
        //}

        ///// <summary>
        ///// 体温肛温
        ///// </summary>
        ///// <param name="g"></param>
        ///// <param name="x"></param>
        ///// <param name="y"></param>
        //public void DrawGangWen(Graphics g, float x, float y)
        //{
        //    //x = x - 3;
        //    //y = y - 3;
        //    //g.FillEllipse(new SolidBrush(Color.White), x + 1f, y + 1f, 12f, 12f);
        //    //g.DrawEllipse(Pens.Blue, x + 1f, y + 1f, 12f, 12f);
        //    //g.DrawEllipse(Pens.Blue, x + 1.5f, y + 1.5f, 11f, 11f);

        //    x = x - 5;
        //    y = y - 7;
        //    g.FillEllipse(new SolidBrush(Color.White), x + 1f, y + 1f, 9f, 9f);
        //    g.DrawEllipse(Pens.Blue, x + 1f, y + 1f, 9f, 9f);
        //    g.DrawEllipse(Pens.Blue, x + 1.5f, y + 1.5f, 9f, 9f);
        //}

        ///// <summary>
        ///// 脉搏
        ///// </summary>
        ///// <param name="g"></param>
        ///// <param name="x"></param>
        ///// <param name="y"></param>
        //public void DrawMaiBo(Graphics g, float x, float y)
        //{
        //    //x = x - 3;
        //    //y = y - 3;
        //    //g.FillEllipse(new SolidBrush(Color.Red), x + 1f, y + 1f, 12f, 12f);
        //    x = x - 6;
        //    y = y - 7;
        //    g.FillEllipse(new SolidBrush(Color.Red), x + 1f, y + 1f, 11f, 11f);
        //}

        ///// <summary>
        ///// 心率
        ///// </summary>
        ///// <param name="g"></param>
        ///// <param name="x"></param>
        ///// <param name="y"></param>
        //public void DrawXinLv(Graphics g, float x, float y)
        //{
        //    //x = x - 3;
        //    //y = y - 3;
        //    //g.FillEllipse(new SolidBrush(Color.White), x + 1f, y + 1f, 12f, 12f);
        //    //g.DrawEllipse(Pens.Red, x + 1f, y + 1f, 12f, 12f);
        //    //g.DrawEllipse(Pens.Red, x + 1.5f, y + 1.5f, 11f, 11f);

        //    x = x - 6;
        //    y = y - 7;
        //    g.FillEllipse(new SolidBrush(Color.White), x + 1f, y + 1f, 10f, 10f);
        //    g.DrawEllipse(Pens.Red, x + 1f, y + 1f, 10f, 10f);
        //    g.DrawEllipse(Pens.Red, x + 1.5f, y + 1.5f, 10f, 10f);
        //}

        ///// <summary>
        ///// 呼吸
        ///// </summary>
        ///// <param name="g"></param>
        ///// <param name="x"></param>
        ///// <param name="y"></param>
        //public void DrawHuXi(Graphics g, float x, float y)
        //{
        //    //x = x - 3;
        //    //y = y - 3;
        //    //g.FillEllipse(new SolidBrush(Color.White), x + 1f, y + 1f, 12f, 12f);
        //    //g.DrawEllipse(Pens.Blue, x + 1f, y + 1f, 12f, 12f);
        //    //g.DrawEllipse(Pens.Blue, x + 1.5f, y + 1.5f, 11f, 11f);

        //    x = x - 5;
        //    y = y - 7;
        //    g.FillEllipse(new SolidBrush(Color.White), x + 1f, y + 1f, 9f, 9f);
        //    g.DrawEllipse(Pens.Blue, x + 1f, y + 1f, 9f, 9f);
        //    g.DrawEllipse(Pens.Blue, x + 1.5f, y + 1.5f, 9f, 9f);
        //}

        ///// <summary>
        ///// 呼吸机
        ///// </summary>
        ///// <param name="g"></param>
        ///// <param name="x"></param>
        ///// <param name="y"></param>
        //public void DrawHuXiJi(Graphics g, float x, float y)
        //{
        //    x = x - 3;
        //    y = y - 3;
        //    g.FillEllipse(new SolidBrush(Color.White), x + 1f, y + 1f, 12f, 12f);
        //    g.DrawEllipse(Pens.Black, x + 0.5f, y + 0.5f, 13f, 13f);
        //    g.DrawString("R", new Font(new FontFamily("宋体"), 10), new SolidBrush(Color.Black), new PointF(x + 1.5f, y + 0.5f));
        //}

        ///// <summary>
        ///// 物理降温
        ///// </summary>
        ///// <param name="g"></param>
        ///// <param name="x"></param>
        ///// <param name="y"></param>
        //public void DrawWuLiJiangWen(Graphics g, float x, float y)
        //{
        //    //x = x - 3;
        //    //y = y - 3;
        //    //g.FillEllipse(new SolidBrush(Color.White), x + 1f, y + 1f, 12f, 12f);
        //    //g.DrawEllipse(Pens.Red, x + 1f, y + 1f, 12f, 12f);
        //    //g.DrawEllipse(Pens.Red, x + 1.5f, y + 1.5f, 11f, 11f);
        //    x = x - 5;
        //    y = y - 7;
        //    g.FillEllipse(new SolidBrush(Color.White), x + 1f, y + 1f, 9f, 9f);
        //    g.DrawEllipse(Pens.Red, x + 1f, y + 1f, 9f, 9f);
        //    g.DrawEllipse(Pens.Red, x + 1.5f, y + 1.5f, 9f, 9f);
        //}

        ///// <summary>
        ///// 新增的物理升温 add by ywk 
        ///// </summary>
        ///// <param name="g"></param>
        ///// <param name="x"></param>
        ///// <param name="y"></param>
        //public void DrawWuLiShengWen(Graphics g, float x, float y)
        //{
        //    //x = x - 3;
        //    //y = y - 3;
        //    //g.FillEllipse(new SolidBrush(Color.White), x + 1f, y + 1f, 12f, 12f);
        //    //g.DrawEllipse(Pens.Blue, x + 1f, y + 1f, 12f, 12f);
        //    //g.DrawEllipse(Pens.Blue, x + 1.5f, y + 1.5f, 11f, 11f);

        //    x = x - 5;
        //    y = y - 7;
        //    g.FillEllipse(new SolidBrush(Color.White), x + 1f, y + 1f, 9f, 9f);
        //    g.DrawEllipse(Pens.Blue, x + 1f, y + 1f, 9f, 9f);
        //    g.DrawEllipse(Pens.Blue, x + 1.5f, y + 1.5f, 9f, 9f);
        //}

        ///// <summary>
        ///// 起搏器
        ///// </summary>
        ///// <param name="g"></param>
        ///// <param name="x"></param>
        ///// <param name="y"></param>
        //public void DrawQiBoQi(Graphics g, float x, float y)
        //{
        //    x = x - 3;
        //    y = y - 3;
        //    g.FillEllipse(new SolidBrush(Color.White), x + 1f, y + 1f, 12f, 12f);
        //    g.DrawEllipse(Pens.Black, x + 0.5f, y + 0.5f, 13f, 13f);
        //    g.DrawString("H", new Font(new FontFamily("宋体"), 10), new SolidBrush(Color.Black), new PointF(x + 1.5f, y + 0.5f));
        //}

        ///// <summary>
        ///// 呼吸脉搏
        ///// </summary>
        ///// <param name="g"></param>
        ///// <param name="x"></param>
        ///// <param name="y"></param>
        //public void DrawHuXiMaiBo(Graphics g, float x, float y)
        //{
        //    x = x - 3;
        //    y = y - 3;
        //    g.FillEllipse(new SolidBrush(Color.White), x + 1f, y + 1f, 12f, 12f);
        //    g.DrawEllipse(Pens.Red, x + 1f, y + 1f, 12f, 12f);
        //    g.FillEllipse(new SolidBrush(Color.Blue), x + 3f, y + 3f, 8f, 8f);
        //}

        ///// <summary>
        ///// 脉搏体温肛门
        ///// </summary>
        ///// <param name="g"></param>
        ///// <param name="x"></param>
        ///// <param name="y"></param>
        //public void DrawMaiBoTiWenGang(Graphics g, float x, float y)
        //{
        //    x = x - 3;
        //    y = y - 3;
        //    g.FillEllipse(new SolidBrush(Color.White), x + 1f, y + 1f, 12f, 12f);
        //    g.DrawEllipse(Pens.Blue, x + 1f, y + 1f, 12f, 12f);
        //    g.FillEllipse(new SolidBrush(Color.Red), x + 3f, y + 3f, 8f, 8f);
        //}

        ///// <summary>
        ///// 脉搏体温口腔
        ///// </summary>
        ///// <param name="g"></param>
        ///// <param name="x"></param>
        ///// <param name="y"></param>
        //public void DrawMaiBoTiWenKou(Graphics g, float x, float y)
        //{
        //    x = x - 3;
        //    y = y - 3;
        //    g.FillEllipse(new SolidBrush(Color.White), x + 1f, y + 1f, 12f, 12f);
        //    g.DrawEllipse(Pens.Red, x + 1f, y + 1f, 12f, 12f);
        //    g.FillEllipse(new SolidBrush(Color.Blue), x + 3f, y + 3f, 8f, 8f);
        //}

        ///// <summary>
        ///// 脉搏体温腋下
        ///// </summary>
        ///// <param name="g"></param>
        ///// <param name="x"></param>
        ///// <param name="y"></param>
        //public void DrawMaiBoTiWenYe(Graphics g, float x, float y)
        //{
        //    x = x - 3;
        //    y = y - 3;
        //    g.FillEllipse(new SolidBrush(Color.White), x + 1f, y + 1f, 12f, 12f);
        //    g.DrawEllipse(Pens.Red, x + 1f, y + 1f, 12f, 12f);
        //    g.DrawLine(Pens.Blue, new PointF(x + 3, y + 3), new PointF(x + 11, y + 11));
        //    g.DrawLine(Pens.Blue, new PointF(x + 11, y + 3), new PointF(x + 3, y + 11));
        //}

        ///// <summary>
        ///// 体温呼吸脉搏
        ///// </summary>
        ///// <param name="g"></param>
        ///// <param name="x"></param>
        ///// <param name="y"></param>
        //public void DrawTiWenHuXiMaiBo(Graphics g, float x, float y)
        //{
        //    //x = x - 3;
        //    //y = y - 3;
        //    //g.FillEllipse(Brushes.White, x + 0.5f, y + 0.5f, 12f, 12f);
        //    //g.DrawEllipse(Pens.Blue, x + 0.5f, y + 0.5f, 13f, 13f);
        //    //g.DrawEllipse(Pens.Red, x + 3f, y + 3f, 8f, 8f);
        //    //g.FillEllipse(Brushes.Blue, x + 5f, y + 5f, 4f, 4f);

        //    x = x - 5;
        //    y = y - 7;
        //    g.FillEllipse(Brushes.White, x + 0.5f, y + 0.5f, 10f, 10f);
        //    g.DrawEllipse(Pens.Blue, x + 0.5f, y + 0.5f, 11f, 11f);
        //    g.DrawEllipse(Pens.Red, x + 3f, y + 3f, 6f, 6f);
        //    g.FillEllipse(Brushes.Blue, x + 5f, y + 5f, 2f, 2f);
        //}


        /// <summary>
        /// 体温口温
        /// </summary>
        /// <param name="g"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void DrawKouWen(Graphics g, float x, float y)
        {
            x = x - 4;
            y = y - 4;
            g.FillEllipse(new SolidBrush(Color.Blue), x, y, 8f, 8f);
        }

        /// <summary>
        /// 体温腋下
        /// </summary>
        /// <param name="g"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void DrawYeXia(Graphics g, float x, float y)
        {
            x = x - 4;
            y = y - 4;
            g.FillEllipse(new SolidBrush(Color.White), x, y, 8f, 8f);
            g.DrawLine(Pens.Blue, new PointF(x + 1, y + 1), new PointF(x + 7f, y + 7f));
            g.DrawLine(Pens.Blue, new PointF(x + 7f, y + 1), new PointF(x + 1, y + 7f));
        }

        /// <summary>
        /// 体温肛温
        /// </summary>
        /// <param name="g"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void DrawGangWen(Graphics g, float x, float y)
        {
            x = x - 4;
            y = y - 4;
            g.FillEllipse(new SolidBrush(Color.White), x, y, 8f, 8f);
            g.DrawEllipse(Pens.Blue, x, y, 8f, 8f);
            g.DrawEllipse(Pens.Blue, x + 1f, y + 1f, 6f, 6f);
        }

        /// <summary>
        /// 脉搏
        /// </summary>
        /// <param name="g"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void DrawMaiBo(Graphics g, float x, float y)
        {
            x = x - 4;
            y = y - 4;
            g.FillEllipse(new SolidBrush(Color.Red), x, y, 8f, 8f);
        }

        /// <summary>
        /// 心率
        /// </summary>
        /// <param name="g"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void DrawXinLv(Graphics g, float x, float y)
        {
            x = x - 4;
            y = y - 4;
            g.FillEllipse(new SolidBrush(Color.White), x, y, 8f, 8f);
            g.DrawEllipse(Pens.Red, x, y, 8f, 8f);
            g.DrawEllipse(Pens.Red, x + 1f, y + 1f, 6f, 6f);
        }

        /// <summary>
        /// 呼吸
        /// </summary>
        /// <param name="g"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void DrawHuXi(Graphics g, float x, float y)
        {
            x = x - 4;
            y = y - 4;
            g.FillEllipse(new SolidBrush(Color.White), x, y, 8f, 8f);
            g.DrawEllipse(Pens.Blue, x, y, 8f, 8f);
            g.DrawEllipse(Pens.Blue, x + 1f, y + 1f, 6f, 6f);
        }

        /// <summary>
        /// 呼吸机
        /// </summary>
        /// <param name="g"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void DrawHuXiJi(Graphics g, float x, float y)
        {
            x = x - 4;
            y = y - 4;
            g.FillEllipse(new SolidBrush(Color.White), x - 1, y - 1, 10f, 10f);
            g.DrawEllipse(Pens.Black, x - 1, y - 1, 10f, 10f);
            g.DrawString("R", new Font(new FontFamily("宋体"), 8), new SolidBrush(Color.Black), new PointF(x, y - 1f));
        }

        /// <summary>
        /// 物理降温
        /// </summary>
        /// <param name="g"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void DrawWuLiJiangWen(Graphics g, float x, float y)
        {
            x = x - 4;
            y = y - 4;
            g.FillEllipse(new SolidBrush(Color.White), x, y, 8f, 8f);
            g.DrawEllipse(Pens.Red, x, y, 8f, 8f);
            g.DrawEllipse(Pens.Red, x + 1f, y + 1f, 6f, 6f);
        }

        /// <summary>
        /// 新增的物理升温 add by ywk 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void DrawWuLiShengWen(Graphics g, float x, float y)
        {
            x = x - 4;
            y = y - 4;
            g.FillEllipse(new SolidBrush(Color.White), x, y, 8f, 8f);
            g.DrawEllipse(Pens.Blue, x, y, 8f, 8f);
            g.DrawEllipse(Pens.Blue, x + 1f, y + 1f, 6f, 6f);
        }

        /// <summary>
        /// 起搏器
        /// </summary>
        /// <param name="g"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void DrawQiBoQi(Graphics g, float x, float y)
        {
            x = x - 4;
            y = y - 4;
            g.FillEllipse(new SolidBrush(Color.White), x - 1, y - 1, 10f, 10f);
            g.DrawEllipse(Pens.Black, x - 1, y - 1, 10f, 10f);
            g.DrawString("H", new Font(new FontFamily("宋体"), 8), new SolidBrush(Color.Black), new PointF(x, y - 1f));
        }

        /// <summary>
        /// 呼吸脉搏
        /// </summary>
        /// <param name="g"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void DrawHuXiMaiBo(Graphics g, float x, float y)
        {
            x = x - 4;
            y = y - 4;
            g.FillEllipse(new SolidBrush(Color.White), x, y, 8f, 8f);
            g.DrawEllipse(Pens.Red, x, y, 8f, 8f);
            g.FillEllipse(new SolidBrush(Color.Blue), x + 1.5f, y + 1.5f, 5f, 5f);
        }

        /// <summary>
        /// 脉搏体温肛门
        /// </summary>
        /// <param name="g"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void DrawMaiBoTiWenGang(Graphics g, float x, float y)
        {
            x = x - 4;
            y = y - 4;
            g.FillEllipse(new SolidBrush(Color.White), x, y, 8f, 8f);
            g.DrawEllipse(Pens.Blue, x, y, 8f, 8f);
            g.FillEllipse(new SolidBrush(Color.Red), x + 1.5f, y + 1.5f, 5f, 5f);
        }

        /// <summary>
        /// 脉搏体温口腔
        /// </summary>
        /// <param name="g"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void DrawMaiBoTiWenKou(Graphics g, float x, float y)
        {
            x = x - 4;
            y = y - 4;
            g.FillEllipse(new SolidBrush(Color.White), x, y, 8f, 8f);
            g.DrawEllipse(Pens.Red, x, y, 8f, 8f);
            g.FillEllipse(new SolidBrush(Color.Blue), x + 1.5f, y + 1.5f, 5f, 5f);
        }

        /// <summary>
        /// 脉搏体温腋下
        /// </summary>
        /// <param name="g"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void DrawMaiBoTiWenYe(Graphics g, float x, float y)
        {
            x = x - 4;
            y = y - 4;
            g.FillEllipse(new SolidBrush(Color.White), x, y, 8f, 8f);
            g.DrawEllipse(Pens.Red, x, y, 8f, 8f);
            g.DrawLine(Pens.Blue, new PointF(x + 1, y + 1), new PointF(x + 7f, y + 7f));
            g.DrawLine(Pens.Blue, new PointF(x + 7f, y + 1), new PointF(x + 1, y + 7f));
        }

        /// <summary>
        /// 体温呼吸脉搏
        /// </summary>
        /// <param name="g"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void DrawTiWenHuXiMaiBo(Graphics g, float x, float y)
        {
            x = x - 5;
            y = y - 5;
            g.FillEllipse(Brushes.White, x, y, 10f, 10f);
            g.DrawEllipse(Pens.Blue, x, y, 10f, 10f);
            g.DrawEllipse(Pens.Red, x + 2, y + 2, 6f, 6f);
            g.FillEllipse(Brushes.Blue, x + 4, y + 4, 2f, 2f);
        }
    }
}
