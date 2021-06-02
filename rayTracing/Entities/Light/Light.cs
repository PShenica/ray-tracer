namespace rayTracing.Entities.Light
{
    public class Light
    {
        public Light(float intensity, LightType type)
        {
            Intensity = intensity;
            Type = type;
        }

        public float Intensity { get; }
        public LightType Type { get; }
    }
}