using Godot;

namespace GodotUtilities
{
    public static class Node2DExtension
    {
        public static Vector2 GetMouseDirection(this Node2D node)
        {
            return (node.GetGlobalMousePosition() - node.GlobalPosition).Normalized();
        }
    }
}