using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.Basic.Paint.NET
{
    [Serializable]
    public class NewShape
        : INewShape, IAction
    {

        #region Fields
        private ShapeData _data;
        #endregion

        #region Ctors

        public NewShape(ShapeData data)
        {
            _data = data;
        }

        #endregion

        #region INewShape Members

        public ShapeData Data
        {
            get { return _data; }
        }

        #endregion

        #region IAction Members

        public ActionType Action
        {
            get { return ActionType.NewShape; }
        }

        #endregion

    }
}
