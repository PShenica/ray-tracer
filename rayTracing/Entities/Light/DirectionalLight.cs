using System.Numerics;

namespace rayTracing.Entities.Light
{
    public class DirectionalLight : Light
    {
        public DirectionalLight(float intensity, Vector3 position) : base(intensity, LightType.Directional)
        {
            Position = position;
        }

        public Vector3 Position { get; }
    }
}