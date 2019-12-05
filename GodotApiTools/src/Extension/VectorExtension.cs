using Godot;

namespace GodotApiTools.Extension
{
    public static class VectorExtension
    {
        private const float ALPHA = .9604339f;
        private const float BETA = .3978247f;

        public static float ApproximateLength(this Vector2 vec)
        {
            var min = Mathf.Min(vec.x, vec.y);
            var max = Mathf.Max(vec.x, vec.y);
            return ALPHA * max + BETA * min;
        }
    }
}