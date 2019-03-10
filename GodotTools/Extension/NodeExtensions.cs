using System;
using System.Reflection;
using Godot;

namespace GodotTools.Extension
{
    public static class NodeExtensions
    {
        public static void AutoGetNodes(this Node n)
        {
            var flags = BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Instance;
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
    }
}