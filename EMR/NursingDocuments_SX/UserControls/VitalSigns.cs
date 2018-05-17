using System;  
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DrectSoft.Core.NursingDocuments.UserControls
{
    /// <summary>
    /// 生命体征类（可能包括：呼吸，脉搏， 体温等）
    /// </summary>
    public class VitalSigns
    {
        private string m_Name = string.Empty;
        private float m_MaxValue = 0f;
        private float m_CellValue = 1f;
        private string m_Unit = string.Empty;

        public VitalSigns()
        { }

        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }

        public float MaxValue
        {
            get
            {
                return m_MaxValue;
            }
            set
            {
                m_MaxValue = value;
            }
        }

        /// <summary>
        /// 一小格所表示的值
        /// </summary>
        public float CellValue
        {
            get
            {
                return m_CellValue;
            }
            set
            {
                m_CellValue = value;
            }
        }

        public string Unit
        {
            get
            {
                return m_Unit;
            }
            set
            {
                m_Unit = value;
            }
        }

        public void PaintImagePrompt(Graphics g, int xPoint, int yPoint, Font font)
        {
            int increase = 20;
            UserControls.Picture picture = new Picture();
            g.DrawString("口温", font, Brushes.Black, xPoint, yPoint);
            yPoint += increase;
            //g.DrawImage(picture.BitmapT1, xPoint + 7, yPoint);
            picture.DrawKouWen(g, xPoint + 10, yPoint);
            yPoint += increase;

            g.DrawString("腋温", font, Brushes.Black, xPoint, yPoint);
            yPoint += increase;
            //g.DrawImage(picture.BitmapT2, xPoint + 7, yPoint);
            picture.DrawYeXia(g, xPoint + 10, yPoint);
            yPoint += increase;

            g.DrawString("肛温", font, Brushes.Black, xPoint, yPoint);
            yPoint += increase;
            //g.DrawImage(picture.BitmapT3, xPoint + 7, yPoint);
            picture.DrawGangWen(g, xPoint + 10, yPoint);
            yPoint += increase;

            g.DrawString("脉搏", font, Brushes.Black, xPoint, yPoint);
            yPoint += increase;
            //g.DrawImage(picture.BitmapMaiBo, xPoint + 7, yPoint);
            picture.DrawMaiBo(g, xPoint + 10, yPoint);
            yPoint += increase;

            g.DrawString("心率", font, Brushes.Black, xPoint, yPoint);
            yPoint += increase;
            //g.DrawImage(picture.BitmapXinlv, xPoint + 7, yPoint);
            picture.DrawXinLv(g, xPoint + 10, yPoint);
            yPoint += increase;

            g.DrawString("呼吸", font, Brushes.Black, xPoint, yPoint);
            yPoint += increase;
            //g.DrawImage(picture.BitmapHuXi, xPoint + 7, yPoint);
            picture.DrawHuXi(g, xPoint + 10, yPoint);
            yPoint += increase;
        }

        public void PaintImagePrompt2(Graphics g, int xPoint, int yPoint, Font font)
        {
            int increase = 40;
            xPoint = xPoint + 125;
            yPoint = yPoint + 10;
            UserControls.Picture picture = new Picture();
            //g.DrawString("口温", font, Brushes.Black, xPoint, yPoint - 1);
            g.DrawString("口温", font, Brushes.Black, xPoint, yPoint - 6);
            xPoint += increase;
            picture.DrawKouWen(g, xPoint, yPoint);
            xPoint += increase;

            //g.DrawString("腋温", font, Brushes.Black, xPoint, yPoint - 1);
            g.DrawString("腋温", font, Brushes.Black, xPoint, yPoint - 6);
            xPoint += increase;
            picture.DrawYeXia(g, xPoint, yPoint);
            xPoint += increase;

            //g.DrawString("肛温", font, Brushes.Black, xPoint, yPoint - 1);
            g.DrawString("肛温", font, Brushes.Black, xPoint, yPoint - 6);
            xPoint += increase;
            picture.DrawGangWen(g, xPoint, yPoint);
            xPoint += increase;

            //g.DrawString("脉搏", font, Brushes.Black, xPoint, yPoint - 1);
            g.DrawString("脉搏", font, Brushes.Black, xPoint, yPoint - 6);
            xPoint += increase;
            picture.DrawMaiBo(g, xPoint, yPoint);
            xPoint += increase;

            //g.DrawString("心率", font, Brushes.Black, xPoint, yPoint - 1);
            g.DrawString("心率", font, Brushes.Black, xPoint, yPoint - 6);
            xPoint += increase;
            picture.DrawXinLv(g, xPoint, yPoint);
            xPoint += increase;

            //g.DrawString("呼吸", font, Brushes.Black, xPoint, yPoint - 1);
            g.DrawString("呼吸", font, Brushes.Black, xPoint, yPoint - 6);
            xPoint += increase;
            picture.DrawHuXi(g, xPoint, yPoint);
            xPoint += increase;
        }
    }

}
