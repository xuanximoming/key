using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.Basic.Paint.NET
{
    public static class ActionExecutor
    {

        public static void Execute(IAction action,
            IShapeSource shapeSource, bool undo)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            if (shapeSource == null)
                throw new ArgumentNullException("shapeSource");

            switch (action.Action)
            {
                case ActionType.NewShape:
                    NewShape(action as INewShape, shapeSource, undo);
                    break;
                case ActionType.ModifyShape:
                    ModifyShape(action as IModifyShape, shapeSource, undo);
                    break;
                case ActionType.SelectShape:
                    SelectShape(action as ISelectShape, shapeSource, undo);
                    break;
                case ActionType.DeleteShape:
                    DeleteShape(action as IDeleteShape, shapeSource, undo);
                    break;
                case ActionType.CreateGhost:
                    CreateGhost(action as ICreateGhost, shapeSource, undo);
                    break;
                default:
                    throw new NotSupportedException("action = " + action.Action.ToString());
            }
        }

        private static void NewShape(INewShape action,
            IShapeSource shapeSource, bool undo)
        {
            if (undo)
            {
                shapeSource.Remove();
            }
            else
            {
                shapeSource.Add(action.Data);
            }
        }

        private static void ModifyShape(IModifyShape action,
            IShapeSource shapeSource, bool undo)
        {
            if (undo)
            {
                shapeSource.Modify(action.ShapeIndex, action.OldData);
            }
            else
            {
                shapeSource.Modify(action.ShapeIndex, action.NewData);
            }
        }

        private static void SelectShape(ISelectShape action,
            IShapeSource shapeSource, bool undo)
        {
            switch (action.Type)
            {
                case SelectType.Deselect:
                    if (undo)
                        shapeSource.Select(action.ShapeIndex);
                    else
                        shapeSource.Deselect();
                    break;
                case SelectType.Select:
                    if (undo)
                        shapeSource.Deselect();
                    else
                        shapeSource.Select(action.ShapeIndex);
                    break;
                default:
                    break;
            }
        }

        private static void DeleteShape(IDeleteShape action,
            IShapeSource shapeSource, bool undo)
        {
            if (undo)
            {
                shapeSource.Enable(action.ShapeIndex);
            }
            else
            {
                shapeSource.Disable(action.ShapeIndex);
            }
        }

        private static void CreateGhost(ICreateGhost action,
            IShapeSource shapeSource, bool undo)
        {
            if (undo)
            {
                shapeSource.ResetGhost();
            }
            else
            {
                shapeSource.SetNewShape(action.Data);
            }
        }

    }
}
