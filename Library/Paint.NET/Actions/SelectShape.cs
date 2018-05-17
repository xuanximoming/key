using System;

namespace DrectSoft.Basic.Paint.NET
{
    public class SelectShape
        : ISelectShape
    {

        #region Fields
        private int _shapeIndex;
        private SelectType _type;
        #endregion

        #region Ctors

        public SelectShape(int shapeIndex, SelectType type)
        {
            _shapeIndex = shapeIndex;
            _type = type;
        }

        #endregion

        #region ISelectShape Members

        public int ShapeIndex
        {
            get { return _shapeIndex; }
        }

        public SelectType Type
        {
            get { return _type; }
        }

        #endregion

        #region IAction Members

        public ActionType Action
        {
            get { return ActionType.SelectShape; }
        }

        #endregion

    }
}
