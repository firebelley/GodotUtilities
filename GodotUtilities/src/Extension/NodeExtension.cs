using System.Collections.Generic;
using System.Linq;
using Godot;

namespace GodotUtilities
{
    public static class NodeExtension
    {
        /// <summary>
        /// Adds the Node to a group with a name equal to the Node's type name.
        /// </summary>
        /// <param name="node"></param>
        public static void AddToGroup(this Node node)
        {
            node.AddToGroup(node.GetType().Name);
        }

        public static T GetSibling<T>(this Node node, int idx) where T : Node
        {
            return (T)node.GetParent().GetChild(idx);
        }

        public static T GetNode<T>(this Node node) where T : Node
        {
            return node.GetNode<T>(typeof(T).Name);
        }

        public static List<T> GetChildren<T>(this Node node) where T : class
        {
            var children = node.GetChildren().Cast<Node>();
            return children.Select(x => x as T).ToList();
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

        public static List<T> GetNodesOfType<T>(this Node node)
        {
            var result = new List<T>();
            var children = node.GetChildren();
            foreach (var child in children)
            {
                if (child is T t)
                {
                    result.Add(t);
                }
            }
            return result;
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
            foreach (var child in n.GetChildren())
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
            foreach (var child in n.GetChildren())
            {
                if (child is Node childNode)
                {
                    childNode.QueueFree();
                }
            }
        }

        public static T GetAncestor<T>(this Node n) where T : Node
        {
            Node currentNode = n;
            while (currentNode != n.GetTree().Root && currentNode is not T)
            {
                currentNode = currentNode.GetParent();
            }

            return currentNode is T ancestor ? ancestor : null;
        }

        public static Node GetLastChild(this Node n)
        {
            var count = n.GetChildCount();
            if (count == 0) return null;
            return n.GetChild(count - 1);
        }

        public static void QueueFreeDeferred(this Node n)
        {
            n.CallDeferred("queue_free");
        }

        public static void QueueFreeAll(this IEnumerable<Node> objects)
        {
            foreach (var obj in objects)
            {
                obj.QueueFree();
            }
        }
    }
}
