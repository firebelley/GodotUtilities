using Godot;

namespace GodotUtilities
{
    public static class PackedSceneExtension
    {
        public static T InstanceOrNull<T>(this PackedScene scene) where T : class
        {
            var node = scene.Instance();
            if (node is T t)
            {
                return t;
            }
            node.QueueFree();
            return null;
        }
    }
}
