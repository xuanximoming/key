using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using DrectSoft.Common.Ctrs.DLG;

namespace DrectSoft.Core.NurseDocument
{
    class LegendIconSource
    {
        public static Dictionary<string, Bitmap> dic_legendIcon = new Dictionary<string, Bitmap>();
        private Bitmap m_BitmapT1;           // = new Bitmap("图片\\口温.png");
        private Bitmap m_BitmapT2;           // = new Bitmap("图片\\腋温.png");
        private Bitmap m_BitmapT3;           // = new Bitmap("图片\\肛温.png");
        private Bitmap m_BitmapMaiBo;        // = new Bitmap("图片\\脉搏.png");
        private Bitmap m_BitmapXinlv;        // = new Bitmap("图片\\心率.png");
        private Bitmap m_BitmapHuXi;         // = new Bitmap("图片\\呼吸.png");
        private Bitmap m_BitmapHuXiSpecial;  // = new Bitmap("图片\\呼吸机.png");
        private Bitmap m_BitmapWuLiJiangWen; // = new Bitmap("图片\\物理降温.png");
        private Bitmap m_BitmapWuLiShengWen; // = new Bitmap("图片\\物理升温.png");
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
        public Bitmap BitmapWuLiShengWen
        {
            get
            {
                return m_BitmapWuLiShengWen;
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

        public LegendIconSource()
        {
            try
            {
                InitPicture();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 初始化绘制时使用的图片
        /// </summary>
        private void InitPicture()
        {
            try
            {
                dic_legendIcon.Clear();
                m_BitmapT1 = (Bitmap)Resources.ResourceManager.GetImage(Resources.ResourceNames.NursKouWen); //口温
                dic_legendIcon.Add(DataLoader.KOUWEN, m_BitmapT1);
                m_BitmapT2 = (Bitmap)Resources.ResourceManager.GetImage(Resources.ResourceNames.NursYeXia); //腋温
                dic_legendIcon.Add(DataLoader.YEWEN, m_BitmapT2);
                m_BitmapT3 = (Bitmap)Resources.ResourceManager.GetImage(Resources.ResourceNames.NursGangWen); //肛温
                dic_legendIcon.Add(DataLoader.GANGWEN, m_BitmapT3);
                m_BitmapMaiBo = (Bitmap)Resources.ResourceManager.GetImage(Resources.ResourceNames.NursMaiBo);
                dic_legendIcon.Add(DataLoader.PULSE, m_BitmapMaiBo);
                m_BitmapXinlv = (Bitmap)Resources.ResourceManager.GetImage(Resources.ResourceNames.NursXinLv);
                dic_legendIcon.Add(DataLoader.HEARTRATE, m_BitmapXinlv);
                m_BitmapHuXi = (Bitmap)Resources.ResourceManager.GetImage(Resources.ResourceNames.NursKouWen);
                dic_legendIcon.Add(DataLoader.BREATHE, m_BitmapHuXi);
                m_BitmapHuXiSpecial = (Bitmap)Resources.ResourceManager.GetImage(Resources.ResourceNames.NursKouWen);//呼吸机
                dic_legendIcon.Add(DataLoader._BREATHE, m_BitmapHuXiSpecial);
                
                //string icon = ConfigInfo.temperatureChangedNode.Attributes["icon"] == null ? "0" : ConfigInfo.temperatureChangedNode.Attributes["icon"].Value;
                m_BitmapWuLiJiangWen = (Bitmap)Resources.ResourceManager.GetImage(Resources.ResourceNames.NursXinLv);
                m_BitmapWuLiShengWen = (Bitmap)Resources.ResourceManager.GetImage(Resources.ResourceNames.NursXinLv);
                dic_legendIcon.Add(DataLoader.PHYSICALCOOLING, m_BitmapWuLiJiangWen);
                dic_legendIcon.Add(DataLoader.PHYSICALHOTTING, m_BitmapWuLiShengWen);

                m_BitmapNursMaiBoTiWenGang = (Bitmap)Resources.ResourceManager.GetImage(Resources.ResourceNames.NursMaiBoTiWenGang);
                dic_legendIcon.Add(DataLoader.NursMaiBoTiWenGang, m_BitmapNursMaiBoTiWenGang);
                m_BitmapNursMaiBoTiWenKou = (Bitmap)Resources.ResourceManager.GetImage(Resources.ResourceNames.NursMaiBoTiWenKou);
                dic_legendIcon.Add(DataLoader.NursMaiBoTiWenKou, m_BitmapNursMaiBoTiWenKou);
                m_BitmapNursMaiBoTiWenYe = (Bitmap)Resources.ResourceManager.GetImage(Resources.ResourceNames.NursMaiBoTiWenYe);
                dic_legendIcon.Add(DataLoader.NursMaiBoTiWenYe, m_BitmapNursMaiBoTiWenYe);
                m_BitmapNursTiWenHuXiMaiBo = (Bitmap)Resources.ResourceManager.GetImage(Resources.ResourceNames.NursTiWenHuXiMaiBo);//呼吸机
                dic_legendIcon.Add(DataLoader.NursTiWenHuXiMaiBo, m_BitmapNursTiWenHuXiMaiBo);

                m_BitmapHuXiSpecial = (Bitmap)Resources.ResourceManager.GetImage(Resources.ResourceNames.NursHuXiJi);
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
