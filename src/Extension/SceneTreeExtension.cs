using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace GodotUtilities
{
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
            var nodes = sceneTree.GetNodesInGroup(group);
            return nodes.Count > 0 ? nodes[0] as T : null;
        }

        /// <summary>
        /// Gets the first Node as T using T's typename as the group name.
        /// </summary>
        /// <param name="sceneTree"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetFirstNodeInGroup<T>(this SceneTree sceneTree) where T : Node
        {
            var name = typeof(T).Name;
            return GetFirstNodeInGroup<T>(sceneTree, name);
        }

        public static IEnumerable<T> GetNodesInGroup<T>(this SceneTree sceneTree, string group) where T : Node
        {
            return sceneTree.GetNodesInGroup(group).Cast<T>();
        }

        public static IEnumerable<T> GetNodesInGroup<T>(this SceneTree sceneTree) where T : Node
        {
            var name = typeof(T).Name;
            return GetNodesInGroup<T>(sceneTree, name);
        }

        public static async Task NextIdle(this SceneTree sceneTree)
        {
            await sceneTree.ToSignal(sceneTree, SceneTree.SignalName.ProcessFrame);
        }
    }
}
