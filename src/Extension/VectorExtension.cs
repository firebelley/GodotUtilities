using Godot;

namespace GodotUtilities
{
    public static class VectorExtension
    {
        private const float ALPHA = .9604339f;
        private const float BETA = .3978247f;

        public static float ApproximateLength(this Vector2 vec)
        {
            var absVec = vec.Abs();
            var min = Mathf.Min(absVec.X, absVec.Y);
            var max = Mathf.Max(absVec.X, absVec.Y);
            return (ALPHA * max) + (BETA * min);
        }

        public static Vector2 RotatedDegrees(this Vector2 v, float degrees)
        {
            return v.Rotated(Mathf.DegToRad(degrees));
        }

        public static bool IsWithinDistanceSquared(this Vector2 v1, Vector2 v2, float distance)
        {
            return v1.DistanceSquaredTo(v2) <= distance * distance;
        }
    }
}
