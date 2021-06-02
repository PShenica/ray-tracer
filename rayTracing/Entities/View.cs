namespace rayTracing
{
    public class View
    {
        public View(int width, int height, float distanceToCamera)
        {
            Width = width;
            Height = height;
            DistanceToCamera = distanceToCamera;
        }

        public int Width { get; }
        public int Height { get; }
        public float DistanceToCamera { get; }
    }
}