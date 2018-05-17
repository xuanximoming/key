using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.Basic.Paint.NET
{
    public class ActionBuilder
        : IActionBuilder
    {

        #region Fields
        private IShapeSource _source;
        #endregion

        #region Ctors

        public ActionBuilder(IShapeSource source)
        {
            _source = source;
        }

        #endregion

        #region Members

        public IShapeSource Source
        {
            get { return _source; }
        }

        public IAction Build(ActionBuildType type, IShape shape)
        {
            int index;
            IGhost ghost = Source.Ghost;
            switch (type)
            {
                case ActionBuildType.Select:
                    index = Source.Shapes.IndexOf(shape);
                    if (index == -1)
                        return null;
                    return new SelectShape(index, SelectType.Select);
                case ActionBuildType.Deselect:
                    index = Source.Shapes.IndexOf(ghost.Shape);
                    return new SelectShape(index, SelectType.Deselect);
                case ActionBuildType.AddNew:
                    return new NewShape(ghost.Shape.Data);
                case ActionBuildType.Motify:
                    index = Source.Shapes.IndexOf(ghost.Shape);
                    return new ModifyShape(index, ghost.Shape.Data, ghost.OldData);
                case ActionBuildType.Delete:
                    index = Source.Shapes.IndexOf(ghost.Shape);
                    return new DeleteShape(index);
                case ActionBuildType.CreateNewShape:
                    return new CreateGhost(shape.Data);
                case ActionBuildType.None:
                default:
                    return null;
            }
        }

        #endregion

    }
}
