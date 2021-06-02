using System.Drawing;
using Color = System.Windows.Media.Color;

namespace rayTracing
{
    public class Pixel
    {
        public Pixel(Point point, Color color)
        {
            X = point.X;
            Y = point.Y;
            Color = color;
        }

        public int X { get; }
        public int Y { get; }
        public Color Color { get; }
    }
}