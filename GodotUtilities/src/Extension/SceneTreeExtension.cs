namespace GodotUtilities;

using System.Collections.Generic;

public static class SceneTreeExtension
{
    /// <summary>
    /// Gets the first Node as T in the group provided.
    /// </summary>
    /// <param name="sceneTree"></param>
    /// <param name="group"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetFirstNodeInGroup<T>(this SceneTree sceneTree, string group) where T : Node
    {
        var node = sceneTree.GetFirstNodeInGroup(group);
        return node as T;
    }

    /// <summary>
    /// Gets the first Node as T using T's typename as the group name.
    /// </summary>
    /// <param name="sceneTree"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetFirstNodeInGroup<T>(this SceneTree sceneTree) where T : Node =>
        sceneTree.GetFirstNodeInGroup<T>(typeof(T).Name);

    public static IEnumerable<T> GetNodesInGroup<T>(this SceneTree sceneTree, string group) where T : Node =>
        sceneTree.GetNodesInGroup(group).Cast<T>();

    public static IEnumerable<T> GetNodesInGroup<T>(this SceneTree sceneTree) where T : Node
    {
        var name = typeof(T).Name;
        return GetNodesInGroup<T>(sceneTree, name);
    }

    public static async Task NextIdle(this SceneTree sceneTree) =>
        await sceneTree.ToSignal(sceneTree, SceneTree.SignalName.ProcessFrame);
}
