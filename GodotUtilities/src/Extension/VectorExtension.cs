using Godot;

namespace GodotUtilities.Extension
{
    public static class VectorExtension
    {
        private const float ALPHA = .9604339f;
        private const float BETA = .3978247f;

        public static float ApproximateLength(this Vector2 vec)
        {
            var absVec = vec.Abs();
            var min = Mathf.Min(absVec.x, absVec.y);
            var max = Mathf.Max(absVec.x, absVec.y);
            return ALPHA * max + BETA * min;
        }
    }
}