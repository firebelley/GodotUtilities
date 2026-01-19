namespace GodotUtilities;

public static class MathUtil
{
    public static RandomNumberGenerator RNG { get; private set; } = new();

    static MathUtil()
    {
        RNG.Randomize();
    }

    public static float DeltaLerp(float from, float to, float deltaTime, float smoothing) => Mathf.Lerp(from, to, 1f - Mathf.Exp(-deltaTime * smoothing));

    public static void SeedRandomNumberGenerator(ulong seed) => RNG = new RandomNumberGenerator
    {
        Seed = seed
    };
}
