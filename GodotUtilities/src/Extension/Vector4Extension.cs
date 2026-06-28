namespace GodotUtilities;

public static class Vector4Extension
{
    public static Color ToColor(this Vector4 vector) => new(vector.X, vector.Y, vector.Z, vector.W);
}
