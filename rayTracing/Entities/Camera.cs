using System.Numerics;

namespace rayTracing
{
    public class Camera
    {
        public Camera(Vector3 position, float[,] rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        public Vector3 Position { get; }
        public float[,] Rotation { get; }
    }
}
