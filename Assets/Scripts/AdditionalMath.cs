using Unity.Mathematics;
using UnityEngine;

public static class AdditionalMath
{
    public static float Modulus(float a, float b)
    {
        return a - b * math.floor(a / b);
    }

    public static float Remap(float x, float A, float B, float C, float D)
    {
        float remappedValue = C + (x - A) / (B - A) * (D - C);
        return remappedValue;
    }

    public static float EaseOutCubic(float lerp)
    {
        return 1f - Mathf.Pow(1f - lerp, 3);
    }
}
