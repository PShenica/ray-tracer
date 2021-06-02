using System.Drawing;
using System.Numerics;

namespace rayTracing.Utility
{
    public class PointsConverter
    {
        public Vector3 CanvasToViewport(Point currentPoint, Canvas canvas, View view)
        {
            return new(currentPoint.X * view.Width / (float) canvas.Width,
                currentPoint.Y * view.Height / (float) canvas.Height,
                view.DistanceToCamera);
        }

        public Point CanvasToWindowForm(Point currentPoint, Canvas canvas)
        {
            return new(canvas.Width / 2 + currentPoint.X, canvas.Height / 2 - currentPoint.Y);
        }
    }
}