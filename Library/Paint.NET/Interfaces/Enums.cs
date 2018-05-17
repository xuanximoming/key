using System;

namespace DrectSoft.Basic.Paint.NET
{

   public enum ShapeType
   {
      None = 0,
      Line = 1,
      Lines = 2,
      Rectangle = 3,
      Ellipse = 4,
      Bezier = 5,
      Curve = 6,
      ClosedCurve = 7,
      Polygon = 8,
      Label = 9,
   }

   public enum MouseStates
   {
      MouseMove = 0,
      MouseDown = 1,
      MouseUp = 2,
   }

   public enum ActionType
   {
      NewShape = 0,
      ModifyShape = 1,
      SelectShape = 2,
      DeleteShape = 3,
      CreateGhost = 4,
   }

   public enum GhostState
   {
      NotAvailable = 0,
      New = 1,
      Modity = 2,
   }

   public enum ActionResult
   {
      Failed = 0,
      Successful = 1,
      Completed = 2,
   }

   public enum SelectType
   {
      Select = 0,
      Deselect = 1,
   }

   public enum ActionBuildType
   {
      None = 0,
      Select = 1,
      Deselect = 2,
      AddNew = 3,
      Motify = 4,
      Delete = 5,
      CreateNewShape = 6,
   }

}
