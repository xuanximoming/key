using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace DrectSoft.Basic.Paint.NET
{
    [Serializable]
    public abstract class ShapeBase
        : IShape, ICloneable
    {

        #region Fields
        private bool _comleted;
        private ShapeData _data;
        #endregion

        #region Ctors

        public ShapeBase(ShapeType type)
        {
            _data = new ShapeData(type);
        }

        #endregion

        #region IShape Members

        public abstract int MaxPoints { get; }

        public abstract int MinPoints { get; }

        public bool Completed
        {
            get { return _comleted; }
        }

        public ShapeData Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public abstract IBoundCalculator BoundCalculator { get; }

        public abstract void Draw(Graphics g, NamedTextureStyles nts);

        public IShape GetNewEmptyShape()
        {
            return CloneInternal(false);
        }

        public abstract IGhost GetGhost();

        public void Complete()
        {
            _comleted = true;
        }

        public void Reset()
        {
            Data = Data.CloneWithoutPoints();
            _comleted = false;
        }

        #endregion

        #region ICloneable Members

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public ShapeBase Clone()
        {
            return CloneInternal(true);
        }

        #endregion

        #region Private/Protected

        protected virtual ShapeBase CloneInternal(bool clonePoints)
        {
            ShapeBase result = base.MemberwiseClone() as ShapeBase;
            result.Data = result.Data.Clone();
            if (clonePoints)
            {
                result._data.Points =
                    (Point[])result._data.Points.Clone();
            }
            else
            {
                result._data.Points = null;
                result._comleted = false;
            }
            return result;
        }

        protected Brush GetBrush(NamedTextureStyles nts)
        {
            if (Data.IsHatch)
                return new HatchBrush(Data.Hatch, Data.FillColor, Color.White);
            if (Data.IsTexture)
            {
                Picture pic = nts.GetTexture(Data.TextureName);
                if (pic != null)
                {
                    Image img = pic.GetImage();
                    if (img != null)
                        return new TextureBrush(img);
                }
            }
            return new SolidBrush(Data.FillColor);
        }

        #endregion

        #region Overrides

        public override bool Equals(object obj)
        {
            ShapeBase shape = obj as ShapeBase;
            if (shape == null)
                return false;
            return Equals(shape);
        }

        public bool Equals(ShapeBase shape)
        {
            return this.Data.Equals(shape.Data);
        }

        public override int GetHashCode()
        {
            return this.Data.GetHashCode();
        }

        #endregion

    }
}
