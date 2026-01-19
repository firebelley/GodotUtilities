namespace GodotUtilities;

public static class Vector2Extension
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

    public static Vector2 RotatedDegrees(this Vector2 v, float degrees) => v.Rotated(Mathf.DegToRad(degrees));

    public static bool IsWithinDistanceSquared(this Vector2 v1, Vector2 v2, float distance) => v1.DistanceSquaredTo(v2) <= distance * distance;

    public static Vector2I RoundToVector2I(this Vector2 vector)
    {
        return new Vector2I(Mathf.RoundToInt(vector.X), Mathf.RoundToInt(vector.Y));
    }
}
