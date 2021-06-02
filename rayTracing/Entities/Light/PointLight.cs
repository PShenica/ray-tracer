using System.Numerics;

namespace rayTracing.Entities.Light
{
    public class PointLight : Light
    {
        public PointLight(float intensity, Vector3 position) : base(intensity, LightType.Point)
        {
            Position = position;
        }

        public Vector3 Position { get; }
    }
}