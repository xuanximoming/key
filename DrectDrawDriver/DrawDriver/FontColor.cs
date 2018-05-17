using System.Drawing;

namespace DrectSoft.DrawDriver
{
    public class FontColor
    {
        private Font _Font;
        private Color _Color;
        public Font Font
        {
            get
            {
                return this._Font;
            }
            set
            {
                this._Font = value;
            }
        }

        public Color Color
        {
            get
            {
                return this._Color;
            }
            set
            {
                this._Color = value;
            }
        }
    }
}
