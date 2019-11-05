using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Godot;

namespace GodotApiTools.Extension
{
    public static class NodeExtensions
    {
        /// <summary>
        /// Uses reflection to find non-public NodePath fields. It is assumed that declared NodePath field names end with "Path". 
        /// This method will use reflection to find the corresponding Node field by removing "Path" from the name and matching on the result.
        /// For example, a NodePath field with name "_labelPath" will automatically try to set the non-public field "_label" to the node present at the NodePath location.
        /// </summary>
        /// <param name="n"></param>
        public static void SetNodesByDeclaredNodePaths(this Node node)
        {
            var flags = BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
            var type = node.GetType();
            var fields = type.GetFields(flags);
            foreach (var field in fields)
            {
                if (field.FieldType == typeof(NodePath))
                {
                    var name = field.Name;
                    var target = name.Substr(0, name.Length - 4);
                    var pathNode = node.GetNode(field.GetValue(node) as NodePath);
                    var targetField = type.GetField(target, flags);
                    if (targetField == null)
                    {
                        throw new Exception($"Target field with name \"{target}\" was not found.");
                    }
                    targetField.SetValue(node, pathNode);
                }
            }
        }

        public static void DisconnectAllSignals(this Node target, Node source)
        {
            var signalList = source.GetSignalList();
            foreach (var signal in signalList)
            {
                var dict = (Godot.Collections.Dictionary) signal;
                var signalName = (string) dict["name"];
                var connectedSignalList = source.GetSignalConnectionList(signalName);
                foreach (var connectedSignal in connectedSignalList)
                {
                    var connectedDict = (Godot.Collections.Dictionary) connectedSignal;
                    if (connectedDict["target"] == target)
                    {
                        source.Disconnect((string) connectedDict["signal"], target, (string) connectedDict["method"]);
                    }
                }
            }
        }

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

        public static T GetFirstNodeOfType<T>(this Node node) where T : Node
        {
            foreach (var child in node.GetChildren())
            {
                if (child is T t)
                {
                    return t;
                }
            }
            return null;
        }

        public static void AddChildDeferred(this Node node, Node child)
        {
            node.CallDeferred("add_child", child);
        }
    }
}