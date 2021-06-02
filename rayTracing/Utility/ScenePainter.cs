using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using rayTracing.Entities;
using rayTracing.Entities.Light;
using rayTracing.Extensions;
using Color = System.Windows.Media.Color;

namespace rayTracing.Utility
{
    public class ScenePainter : IPainter
    {
        private const int RecursionDepth = 3;
        private const int Matt = -1;
        private const float AirRefraction = 1f;

        public ScenePainter()
        {
            Converter = new PointsConverter();
            Camera = new Camera(new Vector3(3, 0, 1),
                new[,] {{0.7071f, 0, -0.7071f}, {0, 1, 0}, {0.7071f, 0, 0.7071f}});
            Canvas = new Canvas(400, 400);
            View = new View(1, 1, 1);
            Scene = Scene.GetDefaultScene();
        }

        private Scene Scene { get; }
        private Canvas Canvas { get; }
        private View View { get; }
        private Camera Camera { get; }
        private PointsConverter Converter { get; }

        public HashSet<Pixel> GetPixels()
        {
            var result = new HashSet<Pixel>();

            foreach (var x in Enumerable.Range(-Canvas.Width / 2, Canvas.Width))
            foreach (var y in Enumerable.Range(-Canvas.Height / 2, Canvas.Height))
            {
                var viewportPoint = Converter.CanvasToViewport(new Point(x, y), Canvas, View);
                var rotatedViewportPoint = viewportPoint.MultiplyVectorMatrix(Camera.Rotation);
                var color = EyeTraceRay(Camera.Position, Vector3.Normalize(rotatedViewportPoint), 1,
                    float.PositiveInfinity, RecursionDepth);
                var pixelCoordinates = Converter.CanvasToWindowForm(new Point(x, y), Canvas);
                var pixel = new Pixel(pixelCoordinates, color);

                result.Add(pixel);
            }

            return result;
        }

        private Color EyeTraceRay(Vector3 cameraPoint, Vector3 viewportToPoint, float tMin, float tMax,
            int recursionDepth)
        {
            var (closestT, closestSphere) = GetClosestIntersection(cameraPoint, viewportToPoint, tMin, tMax);

            if (closestSphere is null)
                return Scene.BackgroundColor;

            // Вычисление локального цвета
            var spherePoint = cameraPoint + closestT * viewportToPoint;
            var normalVector = spherePoint - closestSphere.Center;
            normalVector /= normalVector.Length();
            var localBrightness = ComputeLighting(spherePoint, normalVector, -viewportToPoint, closestSphere.Specular);
            var localColor = Color.Multiply(closestSphere.Color, localBrightness);

            // Если мы достигли предела рекурсии или объект не отражающий, то мы закончили
            var reflectiveValue = closestSphere.Reflective;
            if (recursionDepth <= 0 || reflectiveValue <= 0) return localColor;

            // Вычисление отражённого цвета
            var reflectRay = GetReflectRay(-viewportToPoint, normalVector);
            var reflectedColor = EyeTraceRay(spherePoint, Vector3.Normalize(reflectRay), 0.05f, float.PositiveInfinity,
                recursionDepth - 1);

            var newColor = Color.Add(Color.Multiply(localColor, 1 - reflectiveValue),
                Color.Multiply(reflectedColor, reflectiveValue));

            // Вычисление проходящего сквозь объект света
            var transparency = closestSphere.Transparency;
            if (transparency > 0)
            {
                var refractionRay = GetRefractionRay(viewportToPoint, normalVector, closestSphere.RefractionIndex);
                var refractedColor = EyeTraceRay(spherePoint, Vector3.Normalize(refractionRay), 0.05f,
                    float.PositiveInfinity, recursionDepth - 1);
                newColor = Color.Add(Color.Multiply(newColor, 1 - transparency),
                    Color.Multiply(refractedColor, transparency));
            }

            return newColor;
        }

        private Vector3 GetReflectRay(Vector3 lightVector, Vector3 normalVector)
        {
            return 2 * normalVector * Vector3.Dot(normalVector, lightVector) - lightVector;
        }

        private Vector3 GetRefractionRay(Vector3 i, Vector3 n, float etaT, float etaI = AirRefraction)
        {
            var cosi = -Math.Max(-1f, Math.Min(1f, Vector3.Dot(i, n)));
            if (cosi < 0)
                return GetRefractionRay(i, -n, etaI, etaT);

            var eta = etaI / etaT;
            var k = 1 - eta * eta * (1 - cosi * cosi);

            return k < 0 ? new Vector3(1, 0, 0) : i * eta + n * (eta * cosi - (float) Math.Sqrt(k));
        }


        private Tuple<float, Sphere> GetClosestIntersection(Vector3 cameraPoint, Vector3 viewportToPoint, float tMin,
            float tMax)
        {
            var closestT = tMax;
            Sphere closestSphere = null;

            foreach (var sphere in Scene.Spheres)
            {
                var (item1, item2) = IntersectRaySphere(cameraPoint, viewportToPoint, sphere);

                if (item1 >= tMin && item1 <= tMax && item1 < closestT)
                {
                    closestT = item1;
                    closestSphere = sphere;
                }

                if (item2 >= tMin && item2 <= tMax && item2 < closestT)
                {
                    closestT = item2;
                    closestSphere = sphere;
                }
            }

            return new Tuple<float, Sphere>(closestT, closestSphere);
        }

        private Tuple<float, float> IntersectRaySphere(Vector3 cameraPoint, Vector3 viewportPoint, Sphere sphere)
        {
            var radius = sphere.Radius;
            var cameraPointToCenter = cameraPoint - sphere.Center;

            var k1 = Vector3.Dot(viewportPoint, viewportPoint);
            var k2 = 2 * Vector3.Dot(cameraPointToCenter, viewportPoint);
            var k3 = Vector3.Dot(cameraPointToCenter, cameraPointToCenter) - radius * radius;

            var discriminant = k2 * k2 - 4 * k1 * k3;
            if (discriminant < 0)
                return new Tuple<float, float>(float.PositiveInfinity, float.PositiveInfinity);

            var t1 = (-k2 + Math.Sqrt(discriminant)) / (2 * k1);
            var t2 = (-k2 - Math.Sqrt(discriminant)) / (2 * k1);

            return new Tuple<float, float>((float) t1, (float) t2);
        }

        private float ComputeLighting(Vector3 point, Vector3 normal, Vector3 pointToCamera, int specular)
        {
            var brightness = 0f;

            foreach (var light in Scene.Lights)
                if (light.Type is LightType.Ambient)
                {
                    brightness += light.Intensity;
                }
                else
                {
                    Vector3 lightVector;
                    float tMax;

                    if (light.Type is LightType.Point)
                    {
                        lightVector = ((PointLight) light).Position - point;
                        tMax = 1f;
                    }
                    else
                    {
                        lightVector = ((DirectionalLight) light).Position;
                        tMax = float.PositiveInfinity;
                    }

                    //Проверка тени
                    var (shadowT, shadowSphere) = GetClosestIntersection(point, lightVector, 0.001f, tMax);
                    if (shadowSphere != null) continue;

                    // Диффузность
                    var lightDotProduct = Vector3.Dot(normal, lightVector);
                    if (lightDotProduct > 0)
                        brightness += light.Intensity * lightDotProduct / (normal.Length() * lightVector.Length());

                    // Зеркальность
                    if (specular != Matt)
                    {
                        var reflection = GetReflectRay(lightVector, normal);
                        var reflectionDotProduct = Vector3.Dot(reflection, pointToCamera);

                        if (reflectionDotProduct > 0)
                            brightness += light.Intensity * (float) Math.Pow(
                                reflectionDotProduct / (reflection.Length() * pointToCamera.Length()), specular);
                    }
                }

            return brightness <= 1 ? brightness : 1;
        }
    }
}