using System.Collections.Generic;

namespace rayTracing.Utility
{
    public interface IPainter
    {
        public HashSet<Pixel> GetPixels();
    }
}