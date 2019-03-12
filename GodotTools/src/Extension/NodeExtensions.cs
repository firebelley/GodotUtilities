using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Godot;

namespace GodotTools.Extension
{
    public static class NodeExtensions
    {
        /// <summary>
        /// Uses reflection to find non-public NodePath fields. It is assumed that declared NodePath field names end with "Path". 
        /// This method will use reflection to find the corresponding Node field by removing "Path" from the name and matching on the result.
        /// For example, a NodePath field with name "_labelPath" will automatically try to set the non-public field "_label" to the node present at the NodePath location.
        /// </summary>
        /// <param name="n"></param>
        public static void SetNodesByDeclaredNodePaths(this Node n)
        {
            var flags = BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
            var type = n.GetType();
            var fields = type.GetFields(flags);
            foreach (var field in fields)
            {
                if (field.FieldType == typeof(NodePath))
                {
                    var name = field.Name;
                    var target = name.Substr(0, name.Length - 4);
                    var node = n.GetNode(field.GetValue(n) as NodePath);
                    var targetField = type.GetField(target, flags);
                    if (targetField == null)
                    {
                        throw new Exception($"Target field with name \"{target}\" was not found.");
                    }
                    targetField.SetValue(n, node);
                }
            }
        }

        public static T GetSibling<T>(this Node n, int idx) where T : Node
        {
            return (T) n.GetParent().GetChild(idx);
        }

        public static List<T> GetChildren<T>(this Node node) where T : class
        {
            return node.GetChildren().Select(x => x as T).ToList();
        }

    }
}