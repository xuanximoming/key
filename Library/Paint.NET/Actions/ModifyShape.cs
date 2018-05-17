using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.Basic.Paint.NET
{
    [Serializable]
    public class ModifyShape
        : IModifyShape, IAction
    {

        #region Fields
        private int _index;
        private ShapeData _new;
        private ShapeData _old;
        #endregion

        #region Ctors

        public ModifyShape(int index, ShapeData newdata, ShapeData olddata)
        {
            _index = index;
            _new = newdata;
            _old = olddata;
        }

        #endregion

        #region IModityShape Members

        public int ShapeIndex
        {
            get { return _index; }
        }

        public ShapeData NewData
        {
            get { return _new; }
        }

        public ShapeData OldData
        {
            get { return _old; }
        }

        #endregion

        #region IAction Members

        public ActionType Action
        {
            get { return ActionType.ModifyShape; }
        }

        #endregion

    }
}
