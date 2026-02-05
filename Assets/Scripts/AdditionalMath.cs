using Unity.Mathematics;

public static class AdditionalMath
{
    public static float Modulus(float a, float b)
    {
        return a - b * math.floor(a / b);
    }
}
