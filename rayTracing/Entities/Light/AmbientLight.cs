namespace rayTracing.Entities.Light
{
    public class AmbientLight : Light
    {
        public AmbientLight(float intensity) : base(intensity, LightType.Ambient)
        {
        }
    }
}