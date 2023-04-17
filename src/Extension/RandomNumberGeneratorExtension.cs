using Godot;

namespace GodotUtilities
{
    public static class RandomNumberGeneratorExtension
    {
        public static Vector2 RandDirection(this RandomNumberGenerator rng)
        {
            return Vector2.Right.Rotated(rng.RandfRange(0, Mathf.Tau));
        }

        public static int RandSign(this RandomNumberGenerator rng)
        {
            return rng.Randf() < .5 ? -1 : 1;
        }
    }
}
