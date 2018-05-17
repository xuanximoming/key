using DrectSoft.Library.EmrEditor.Src.Common;
using System;
using System.Drawing;

namespace DrectSoft.Library.EmrEditor.Src.Document
{
    public class ZYFlag : ZYElement
    {
        /// <summary>
        /// 是否可以删除定位符 Add By wwj 2013-08-01
        /// </summary>
        public bool CanDelete = true;

        /// <summary>
        /// ZYFlag显示的方向 默认向Left
        /// Left  :  "]"
        /// Right :  "["
        /// </summary>
        public ZYFlagDirection Direction = ZYFlagDirection.Left;

        /// <summary>
        /// 打印的时候是否占据空间
        /// </summary>
        public bool IsHoldSpaceWhenPrint = true;

        public ZYFlag()
        {
            this.Type = ElementType.Flag;
        }

        Pen pen = Pens.Blue;

        public override bool RefreshView()
        {
            //this.RefreshSize();
            //画标识符]
            if (!this.OwnerDocument.Info.Printing)
            {
                Point A = new Point(this.RealLeft, this.RealTop);
                Point D = new Point(this.RealLeft, this.RealTop + this.Height);

                #region Modified By wwj 2013-08-01 将原先标示符由小旗子 改为 “]”或 “[”
                this.Width = (int)myOwnerDocument.View.MeasureString(" ", this.Font).Width;

                int x = this.Width / 2;
                if (Direction == ZYFlagDirection.Left)
                {
                    OwnerDocument.View.DrawString("}", this.Font, Color.Blue, A.X - 6, A.Y);
                }
                else
                {
                    OwnerDocument.View.DrawString("{", this.Font, Color.Blue, A.X - 6, A.Y);
                }
                #endregion
            }
            return true;
        }

        public override string GetXMLName()
        {
            return ZYTextConst.c_Flag;
        }

        public override bool ToXML(System.Xml.XmlElement myElement)
        {
            if (myElement != null)
            {
                myElement.SetAttribute("type", StringCommon.GetNameByType(this.Type));
                myElement.SetAttribute("name", this.Name);
                myElement.SetAttribute("code", this.Code);

                //Add by wwj 2013-08-01
                myElement.SetAttribute("candelete", this.CanDelete.ToString());

                //Add by wwj 2013-08-01
                myElement.SetAttribute("direction", this.Direction.ToString());

                //Add by wwj 2013-08-01
                myElement.SetAttribute("isholdspacewhenprint", this.IsHoldSpaceWhenPrint.ToString());

                return true;
            }
            return false;
        }

        public override bool FromXML(System.Xml.XmlElement myElement)
        {
            if (myElement != null)
            {
                this.Type = StringCommon.GetTypeByName(myElement.GetAttribute("type"));
                this.Name = myElement.GetAttribute("name");
                this.Code = myElement.GetAttribute("code");

                //Add by wwj 2013-08-01
                if (myElement.HasAttribute("candelete"))
                {
                    this.CanDelete = bool.Parse(myElement.GetAttribute("candelete"));
                }

                //Add by wwj 2013-08-01
                if (myElement.HasAttribute("direction"))
                {
                    this.Direction = (ZYFlagDirection)Enum.Parse(typeof(ZYFlagDirection), myElement.GetAttribute("direction"));
                }

                //Add by wwj 2013-08-01
                if (myElement.HasAttribute("isholdspacewhenprint"))
                {
                    this.IsHoldSpaceWhenPrint = bool.Parse(myElement.GetAttribute("isholdspacewhenprint"));
                }

                return true;
            }
            return false;
        }

        public override string ToEMRString()
        {
            return "";
        }
    }
}
