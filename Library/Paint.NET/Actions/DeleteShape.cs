using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.Basic.Paint.NET
{
    public class DeleteShape
        : IDeleteShape, IAction
    {

        #region Fields
        private int _index;
        #endregion

        #region Ctors

        public DeleteShape(int index)
        {
            _index = index;
        }

        #endregion

        #region IDeleteShape Members

        public int ShapeIndex
        {
            get { return _index; }
        }

        #endregion

        #region IAction Members

        public ActionType Action
        {
            get { return ActionType.DeleteShape; }
        }

        #endregion

    }
}
