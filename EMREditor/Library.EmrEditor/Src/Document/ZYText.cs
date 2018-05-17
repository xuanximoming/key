using DrectSoft.Library.EmrEditor.Src.Common;

namespace DrectSoft.Library.EmrEditor.Src.Document
{
    /// <summary>
    /// 继承自ZYTextBlock
    /// <para>表示一块元素集合的容器对象</para>
    /// </summary>
    public class ZYText : ZYTextBlock
    {
        public ZYText()
        {
            this.Type = ElementType.Text;
        }


        public override string GetXMLName()
        {
            return ZYTextConst.c_EMRText;
        }

        public override string Text
        {
            set
            {
                this.ChildElements.Clear();
                foreach (char myc in value)
                {
                    ZYTextChar c = new ZYTextChar();
                    c.Char = myc;

                    Attributes.CopyTo(c.Attributes);
                    c.UpdateAttrubute();

                    c.Parent = this;
                    c.OwnerDocument = this.OwnerDocument;
                    this.ChildElements.Add(c);

                }
                text = value;
            }
        }
        public override bool ToXML(System.Xml.XmlElement myElement)
        {

            if (myElement != null)
            {
                this.Attributes.ToXML(myElement);
                myElement.SetAttribute("type", StringCommon.GetNameByType(this.Type));
                myElement.SetAttribute("name", this.Name);

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
                //value应该在设置Attributes之前
                this.Attributes.FromXML(myElement);
                this.Name = myElement.GetAttribute("name");
                this.Text = myElement.InnerText;
                return true;
            }
            return false;
        }

        public override System.Collections.ArrayList RefreshLine()
        {
            return base.RefreshLine();
        }
        public override bool RefreshView()
        {
            return true;
        }
    }
}
