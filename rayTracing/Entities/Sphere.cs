using System.Numerics;
using System.Windows.Media;

namespace rayTracing
{
    public class Sphere
    {
        public Sphere(Vector3 center, double radius, Color color, int specular, float reflective, float transparency,
            float refractionIndex)
        {
            Radius = radius;
            Color = color;
            Center = center;
            Specular = specular;
            Reflective = reflective;
            Transparency = transparency;
            RefractionIndex = refractionIndex;
        }

        public double Radius { get; }
        public Color Color { get; }
        public Vector3 Center { get; }
        public int Specular { get; }
        public float Reflective { get; }
        public float Transparency { get; }
        public float RefractionIndex { get; }
    }
}