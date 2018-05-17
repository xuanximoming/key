using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;
using DrectSoft.Library.EmrEditor.Src.Common;
using System.Windows.Forms;

namespace DrectSoft.Library.EmrEditor.Src.Document
{
    public class ZYCheckBox : ZYElement
    {
        public ZYCheckBox()
        {
            this.Type = ElementType.CheckBox;
        }

        StringFormat strFormat = StringFormat.GenericTypographic;
        public bool Checked = false;

        public override string GetXMLName()
        {
            return ZYTextConst.c_CheckBox;
        }

        public override bool RefreshView()
        {
            this.RefreshSize();

            //如果是选择打印，则判断是否在范围之内
            if (this.IsNeedPrint())
            {
                Rectangle r = this.Bounds;

                OwnerDocument.View.Graph.DrawString("  "+this.Name, this.Font, Brushes.Black, r , strFormat);
                Graphics g = this.OwnerDocument.View.Graph;

                r.Width = 15;
                r.Height = 15;
                r.Offset(2, 0);

                if (this.Checked)
                {
                    ControlPaint.DrawCheckBox(g, r, ButtonState.Checked);
                }
                else
                {
                    ControlPaint.DrawCheckBox(g, r, ButtonState.Normal);
                }
            }
            return true;
            //return base.RefreshView();  
        }

        public override bool RefreshSize()
        {
            //计算宽高,多余一个字符用来画checkbox

            SizeF size1 = OwnerDocument.View.Graph.MeasureString(this.Name+"口", this.Font, int.MaxValue, strFormat);

            this.Width = (int)size1.Width+10;
            this.Height = (int)size1.Height;
            return true;
            //return base.RefreshSize();
        }

        public override bool ToXML(System.Xml.XmlElement myElement)
        {

            if (myElement != null)
            {
                this.Attributes.ToXML(myElement);
                myElement.SetAttribute("type", StringCommon.GetNameByType(this.Type));
                myElement.SetAttribute("name", this.Name);
                myElement.SetAttribute("code", this.Code);
                myElement.InnerText = this.Checked.ToString();
                return true;
                //return base.ToXML(myElement);
            }
            return false;
        }

        public override bool FromXML(System.Xml.XmlElement myElement)
        {
            if (myElement != null)
            {

                this.Type = StringCommon.GetTypeByName(myElement.GetAttribute("type"));
                //value应该在设置Attributes之前

                this.Attributes.FromXML(myElement);
                this.Name = myElement.GetAttribute("name");
                this.Code = myElement.GetAttribute("code");

                this.Checked  = bool.Parse(myElement.InnerText);
                return true;
            }
            return false;
        }

        public override string ToEMRString()
        {
            return this.Name; //this.Checked.ToString(); Modified by wwj 2013-02-01 解决复制checkbox时只记录Code值的问题
            //return base.ToEMRString();
        }

    }
}
