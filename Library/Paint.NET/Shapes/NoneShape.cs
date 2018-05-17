using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace DrectSoft.Basic.Paint.NET
{
    [Serializable]
    public class NoneShape
        : ShapeBase
    {

        #region Ctors

        public NoneShape()
            : base(ShapeType.None) { }

        #endregion

        #region Override Members

        public override int MaxPoints
        {
            get { return 0; }
        }

        public override int MinPoints
        {
            get { return 0; }
        }

        public override IBoundCalculator BoundCalculator
        {
            get { return NoneBoundCalculator.Instance; }
        }

        public override void Draw(Graphics g, NamedTextureStyles nts) { }

        public override IGhost GetGhost()
        {
            IMouseDrawMode mode = new MouseDragDrawMode(this);
            return new Ghost(this, mode);
        }

        public override string ToString()
        {
            return string.Empty;
        }

        #endregion

    }
}
