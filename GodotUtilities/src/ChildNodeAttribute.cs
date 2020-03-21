using System;
using System.Reflection;
using Godot;

namespace GodotUtilities
{
    public class ChildNodeAttribute : Attribute
    {
        public string NodePath { get; private set; }

        public ChildNodeAttribute(string nodePath = null)
        {
            NodePath = nodePath;
        }
    }

    public static class ChildNodeAttributeExtension
    {
        public static void WireChildNodes(this Node n)
        {
            var fieldsAndProperties = GetFieldsAndProperties(n);
            foreach (var memberInfo in fieldsAndProperties)
            {
                var attribute = Attribute.GetCustomAttribute(memberInfo, typeof(ChildNodeAttribute));
                if (!(attribute is ChildNodeAttribute childNodeAttribute)) continue;

                var nodePath = GetNodePath(childNodeAttribute, memberInfo);
                var childNode = n.GetNodeOrNull(nodePath);
                if (memberInfo is PropertyInfo propertyInfo)
                {
                    propertyInfo.SetValue(n, childNode);
                }
                if (memberInfo is FieldInfo fieldInfo)
                {
                    fieldInfo.SetValue(n, childNode);
                }
            }
        }

        private static MemberInfo[] GetFieldsAndProperties(Node n)
        {
            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var properties = n.GetType().GetProperties(flags);
            var fields = n.GetType().GetFields(flags);
            var memberInfo = new MemberInfo[properties.Length + fields.Length];
            properties.CopyTo(memberInfo, 0);
            fields.CopyTo(memberInfo, properties.Length);
            return memberInfo;
        }

        private static string GetNodePath(ChildNodeAttribute childNodeAttribute, MemberInfo memberInfo)
        {
            if (childNodeAttribute.NodePath != null) return childNodeAttribute.NodePath;

            var parts = memberInfo.Name.Split('_');
            for (int i = 0; i < parts.Length; i++)
            {
                var part = parts[i];
                if (part.Length == 0) continue;
                parts[i] = char.ToUpper(part[0]) + (part.Length > 1 ? part.Substring(1) : string.Empty);
            }
            return string.Join(string.Empty, parts);
        }
    }
}