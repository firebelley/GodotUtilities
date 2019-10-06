using Godot;

namespace GodotApiTools.Extension
{
    public static class SceneTreeExtension
    {
        public static T GetFirstNodeInGroup<T>(this SceneTree sceneTree, string group) where T : Node
        {
            var nodes = sceneTree.GetNodesInGroup(group);
            return nodes.Count > 0 ? nodes[0] as T : null;
        }
    }
}