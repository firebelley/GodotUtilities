namespace GodotUtilities;

public static class ColorExtension
{
    public static Vector4 ToVector4(this Color color) => new(color.R, color.G, color.B, color.A);
}
