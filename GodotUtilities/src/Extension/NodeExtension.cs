using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Godot;

namespace GodotUtilities.Extension
{
    public static class NodeExtension
    {
        public static T GetSibling<T>(this Node node, int idx) where T : Node
        {
            return (T) node.GetParent().GetChild(idx);
        }

        public static T GetNode<T>(this Node node) where T : Node
        {
            return node.GetNode<T>(typeof(T).Name);
        }

        public static List<T> GetChildren<T>(this Node node) where T : class
        {
            var children = node.GetChildren() as IEnumerable<Node>;
            return children.Select(x => x as T).ToList();
        }

        public static void QueueFree(this IEnumerable<Node> nodes)
        {
            foreach (var n in nodes)
            {
                n.QueueFree();
            }
        }

        public static T GetFirstNodeOfType<T>(this Node node)
        {
            var children = node.GetChildren();
            foreach (var child in children)
            {
                if (child is T t)
                {
                    return t;
                }
            }
            return default;
        }

        public static void AddChildDeferred(this Node node, Node child)
        {
            node.CallDeferred("add_child", child);
        }

        public static T GetNullableNodePath<T>(this Node n, NodePath nodePath) where T : Node
        {
            if (nodePath == null) return null;
            return n.GetNodeOrNull<T>(nodePath);
        }

        /// <summary>
        /// Removes the node's children from the scene tree and then queues them for deletion.
        /// </summary>
        /// <param name="n"></param>
        /// <typeparam name="T"></typeparam>
        public static void RemoveAndQueueFreeChildren(this Node n)
        {
            var children = n.GetChildren();
            foreach (var child in children)
            {
                if (child is Node childNode)
                {
                    n.RemoveChild(childNode);
                    childNode.QueueFree();
                }
            }
        }

        /// <summary>
        /// Queues all child nodes for deletion.
        /// </summary>
        /// <param name="n"></param>
        /// <typeparam name="T"></typeparam>
        public static void QueueFreeChildren(this Node n)
        {
            var children = n.GetChildren();
            foreach (var child in children)
            {
                if (child is Node childNode)
                {
                    childNode.QueueFree();
                }
            }
        }
    }
}