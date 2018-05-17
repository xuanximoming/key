using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.Basic.Paint.NET
{
    public class ShapeFactory
        : IShapeFactory
    {

        #region Fields
        public static readonly ShapeFactory Instance = new ShapeFactory();
        #endregion

        #region Ctors

        public ShapeFactory() { }

        #endregion

        #region IShapeFactory Members

        public IShape Create(ShapeData data)
        {
            IShape result;
            switch (data.Type)
            {
					 case ShapeType.Label:
						  result = new LabelShape();
						  ((LabelShape)result).Data = data;
						  break;
                case ShapeType.Line:
                    result = new LineShape();
                    result.Data = data;
                    break;
                case ShapeType.Lines:
                    result = new LinesShape();
                    result.Data = data;
                    break;
                case ShapeType.Rectangle:
                    result = new RectangleShape();
                    result.Data = data;
                    break;
                case ShapeType.Ellipse:
                    result = new EllipseShape();
                    result.Data = data;
                    break;
                case ShapeType.Bezier:
                    result = new BezierShape();
                    result.Data = data;
                    break;
                case ShapeType.Curve:
                    result = new CurveShape();
                    result.Data = data;
                    break;
                case ShapeType.ClosedCurve:
                    result = new ClosedCurveShape();
                    result.Data = data;
                    break;
                case ShapeType.Polygon:
                    result = new PolygonShape();
                    result.Data = data;
                    break;
                case ShapeType.None:
                default:
                    result = null;
                    break;
            }
            if (result != null)
                result.Data.Bounds =
                    result.BoundCalculator.GetBounds(result.Data.Points);
            return result;
        }

        #endregion

    }
}
