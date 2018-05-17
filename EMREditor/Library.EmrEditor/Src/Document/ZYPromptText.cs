using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrectSoft.Library.EmrEditor.Src.Common;

namespace DrectSoft.Library.EmrEditor.Src.Document
{
    public class ZYPromptText : ZYTextBlock
    {
        public ZYPromptText()
        {
            this.Type = ElementType.PromptText;
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

        public override string GetXMLName()
        {
            return ZYTextConst.c_Helement;
        }

        public override bool ToXML(System.Xml.XmlElement myElement)
        {
            if (myElement != null)
            {
                this.Attributes.ToXML(myElement);
                base.ToXML(myElement);
                myElement.SetAttribute("type", StringCommon.GetNameByType(this.Type));
                myElement.SetAttribute("name", this.Name);

                myElement.InnerText = this.Text;
                
                //return base.ToXML(myElement);
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
                this.Text = myElement.InnerText;
                return true;
            }
            return false;
        }

        
    }
}
