namespace GodotUtilities;

public static class PackedSceneExtension
{
    public static T InstantiateOrFree<T>(this PackedScene scene) where T : class
    {
        var node = scene.Instantiate();
        if (node is T t)
        {
            return t;
        }
        node.QueueFree();
        GD.PushWarning($"Could not instance PackedScene {scene} as {typeof(T).Name}");
        return null;
    }

    /// <summary>
    /// Instantiates the scene and passes it as an argument to the supplied action. Afterwards, frees the scene. Allows for "fetching" data from an instantiated scene.
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    public static void ExtractData<T>(this PackedScene scene, Action<T> action) where T : Node
    {
        var node = scene.InstantiateOrFree<T>();
        action(node);
        node.QueueFree();
    }

}
