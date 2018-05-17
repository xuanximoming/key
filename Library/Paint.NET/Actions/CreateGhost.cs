using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.Basic.Paint.NET
{
    public class CreateGhost
        : ICreateGhost
    {

        #region Fields
        private ShapeData _data;
        #endregion

        #region Ctors

        public CreateGhost(ShapeData data)
        {
            _data = data;
        }

        #endregion

        #region ICreateGhost Members

        public ShapeData Data
        {
            get { return _data; }
        }

        #endregion

        #region IAction Members

        public ActionType Action
        {
            get { return ActionType.CreateGhost; }
        }

        #endregion

    }
}
