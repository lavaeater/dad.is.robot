namespace Otter.Custom
{
    public static class RangeForcer
    {
        public static float ForceRange(this float value, float newMax, float newMin)
        {
            float oldMin = 14;
            float oldMax = 241;
            float newValue = ((value - oldMin) / (oldMax - oldMin)) * (newMax - newMin) + newMin;
            return newValue;
        }
    }
}