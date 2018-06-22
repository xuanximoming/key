using DrectSoft.Library.EmrEditor.Src.Common;

namespace DrectSoft.Library.EmrEditor.Src.Document
{
    public class ZYMacro : ZYTextBlock
    {
        public ZYMacro()
        {
            this.Type = ElementType.Macro;
        }


        public override string GetXMLName()
        {
            return ZYTextConst.c_Macro;
        }


        public override bool ToXML(System.Xml.XmlElement myElement)
        {
            if (myElement != null)
            {
                this.Attributes.ToXML(myElement);
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
                this.Name = myElement.GetAttribute("name");
                this.Text = myElement.InnerText;
                return true;
            }
            return false;
        }
    }
}
