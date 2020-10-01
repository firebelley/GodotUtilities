using Godot;

namespace GodotUtilities
{
    public static class MathUtil
    {
        public static RandomNumberGenerator RNG { get; private set; } = new RandomNumberGenerator();

        static MathUtil()
        {
            RNG.Randomize();
        }

        public static float DeltaLerp(float smoothing, float delta)
        {
            return 1f - Mathf.Pow(smoothing, delta);
        }

        public static void SeedRandomNumberGenerator(ulong seed)
        {
            RNG = new RandomNumberGenerator
            {
                Seed = seed
            };
        }
    }
}