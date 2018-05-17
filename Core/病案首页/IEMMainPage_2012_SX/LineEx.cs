using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace DrectSoft.Core.IEMMainPage
{
    ///// <summary>
    ///// Summary description for LineEx.
    ///// </summary>
    //public class HLineEx : Label
    //{
    //    private Pen m_LinePen = null;
    //    /// <summary>
    //    /// 指定画线时使用的Pen
    //    /// </summary>
    //    public Pen LinePen
    //    {
    //        get
    //        {
    //            return this.m_LinePen;
    //        }
    //        set
    //        {
    //            this.m_LinePen = value;
    //        }
    //    }


    //    public HLineEx()
    //    {
    //        //
    //        // TODO: Add constructor logic here
    //        //
    //        this.BorderStyle = BorderStyle.FixedSingle;
    //        this.Height = 1;
    //    }
    //}



    //public class VLineEx : Label
    //{
    //    public VLineEx()
    //    {
    //        this.BorderStyle = BorderStyle.FixedSingle;
    //        this.Width = 1;
    //    }
    //}

    /// <summary>
    /// 横线
    /// </summary>
    public class HLineEx : Control
    {
        public HLineEx()
        {
            this.BackColor = Color.White;
            this.Height = 1;
        }

        
        public bool IsBold
        {
            get;
            set;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawLine(Pens.Black, new Point(0, 0), new Point(this.Width, 0));
            if (IsBold == true)
            {
                e.Graphics.DrawLine(Pens.Black, new Point(0, 1), new Point(this.Width, 1));
            }
        }
    }

    /// <summary>
    /// 竖线
    /// </summary>
    public class VLineEx : Control
    {
        public VLineEx()
        {
            this.BackColor = Color.White;
            this.Width = 1;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawLine(Pens.Black, new Point(0, 0), new Point(0, this.Height));
        }
    }

}