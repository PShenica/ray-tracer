using System.Collections.Generic;
using System.Numerics;
using System.Windows.Media;
using rayTracing.Entities;
using rayTracing.Entities.Light;

namespace rayTracing.Extensions
{
    public static class SceneExtensions
    {
        private const int Matt = -1;
        private const float GlassRefraction = 1.05f;

        public static Scene GetDefaultScene(this Scene scene)
        {
            scene ??= new Scene();

            var sphere1 = new Sphere(new Vector3(0, -1, 3), 1, Color.FromRgb(255, 0, 0), 500, 0.2f, 0.4f,
                GlassRefraction);
            var sphere2 = new Sphere(new Vector3(2, 0, 4), 1, Color.FromRgb(0, 0, 250), 500, 0.3f, 0.7f,
                GlassRefraction);
            var sphere3 = new Sphere(new Vector3(-2, 0, 4), 1, Color.FromRgb(0, 255, 0), 10, Matt, 0, GlassRefraction);
            var sphere4 = new Sphere(new Vector3(0, -5001, 0), 5000, Color.FromRgb(255, 255, 0), 1000, 0.5f, 0,
                GlassRefraction);
            var sphere5 = new Sphere(new Vector3(0, 1, 5), 1, Color.FromRgb(255, 255, 0), 400, 0.3f, 0,
                GlassRefraction);

            var light1 = new AmbientLight(0.2f);
            var light2 = new PointLight(0.6f, new Vector3(2, 1, 0));
            var light3 = new DirectionalLight(0.2f, new Vector3(1, 4, 4));

            scene.Spheres.AddRange(new List<Sphere> {sphere1, sphere2, sphere3, sphere4, sphere5});
            scene.Lights.AddRange(new List<Light> {light1, light2, light3});

            return scene;
        }
    }
}