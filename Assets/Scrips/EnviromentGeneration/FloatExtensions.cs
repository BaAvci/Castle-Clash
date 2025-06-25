public static class FloatExtensions
{
    /// <summary>
    /// Maps a value from one range to another.
    /// </summary>
    /// <param name="value">The value to remap</param>
    /// <param name="fromMin">Old min value</param>
    /// <param name="fromMax">Old max value</param>
    /// <param name="toMin">New min value</param>
    /// <param name="toMax">New max value</param>
    /// <returns></returns>
    public static float Map(this float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        return toMin + (value - fromMin) * (toMax - toMin) / (fromMax - fromMin);
    }
}
