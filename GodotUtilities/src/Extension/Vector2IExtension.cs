namespace GodotUtilities;

public static class Vector2IExtension
{
    public static Vector2 ToVector2(this Vector2I vector2I) => new(vector2I.X, vector2I.Y);
}
