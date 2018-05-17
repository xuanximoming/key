using System.Xml;

namespace DrectSoft.DrawDriver
{
    public class DrawObject
    {
        public XmlNode _node;

        public DrawInfo _info;

        public XmlNode Node
        {
            get
            {
                return this._node;
            }
            set
            {
                this._node = value;
            }
        }

        public DrawInfo Info
        {
            get
            {
                return this._info;
            }
            set
            {
                this._info = value;
            }
        }
    }
}
