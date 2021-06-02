using System.Numerics;

namespace rayTracing.Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3 MultiplyVectorMatrix(this Vector3 vector, float[,] matrix)
        {
            var vector3 = new[] {vector.X, vector.Y, vector.Z};
            var result = new float[3];

            for (var i = 0; i < 3; i++)
            for (var j = 0; j < 3; j++)
                result[i] += vector3[j] * matrix[i, j];

            return new Vector3(result[0], result[1], result[2]);
        }
    }
}