using System.Collections.Generic;
using System.Windows.Media;

namespace rayTracing.Entities
{
    public class Scene
    {
        public Scene() : this(Color.FromRgb(0, 0, 0))
        {
        }

        public Scene(Color backgroundColor)
        {
            Spheres = new List<Sphere>();
            Lights = new List<Light.Light>();
            BackgroundColor = backgroundColor;
        }

        public List<Sphere> Spheres { get; }
        public List<Light.Light> Lights { get; }
        public Color BackgroundColor { get; }
    }
}