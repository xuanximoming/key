using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrectSoft.Library.EmrEditor.Src.Gui;
using System.Drawing;
using System.Windows.Forms;
using DrectSoft.Library.EmrEditor.Src.Common;

namespace DrectSoft.Library.EmrEditor.Src.Document
{
    public class ZYFormatString : ZYTextBlock
    {
        public ZYFormatString()
        {

            this.Type = ElementType.FormatString;
        }

        string name = "";
        public override string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                if (this.Text.Length == 0)
                {
                    this.Text = value;
                }
            }
        }

        uint length = 10;
        public uint Length
        {
            get { return length; }
            set { length = value; }
        }

        public override string GetXMLName()
        {
            return ZYTextConst.c_FStrElement;
        }


        public override bool ToXML(System.Xml.XmlElement myElement)
        {
            if (myElement != null)
            {
                this.Attributes.ToXML(myElement);
                base.ToXML(myElement);
                myElement.SetAttribute("type", StringCommon.GetNameByType(this.Type));

                myElement.SetAttribute("name", this.Name);
                myElement.SetAttribute("length", this.Length.ToString());
                myElement.InnerText = this.Text;
                return true;
            }
            return false;
        }

        public override bool FromXML(System.Xml.XmlElement myElement)
        {
            if (myElement != null)
            {
                this.Type = StringCommon.GetTypeByName(myElement.GetAttribute("type"));
                this.Attributes.FromXML(myElement);
                base.FromXML(myElement);
                this.Name = myElement.GetAttribute("name");
                this.Length = uint.Parse(myElement.GetAttribute("length"));
                this.Text = myElement.InnerText;
                return true;
            }
            return false;
        }
    }
}
