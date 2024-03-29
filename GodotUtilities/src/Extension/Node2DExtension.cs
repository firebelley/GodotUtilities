namespace GodotUtilities;

public static class Node2DExtension
{
    public static Vector2 GetMouseDirection(this Node2D node) =>
        (node.GetGlobalMousePosition() - node.GlobalPosition).Normalized();
}