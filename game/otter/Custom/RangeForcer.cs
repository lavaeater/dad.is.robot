namespace Otter.Custom
{
    public static class RangeForcer
    {
        public static readonly float OldMin = 5;
        public static readonly float OldMax = 250;

        public static float ForceRange(this float value, float oldMax, float oldMin, float newMax, float newMin)
        {
            return ((value - oldMin)/(oldMax - oldMin))*(newMax - newMin) + newMin;
        }

        public static float ForceRange(this float value, float newMax, float newMin)
        {
            float newValue = ((value - OldMin) / (OldMax - OldMin)) * (newMax - newMin) + newMin;
            return newValue;
        }
    }
}