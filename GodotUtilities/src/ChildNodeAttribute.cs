using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Godot;

namespace GodotUtilities
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class NodeAttribute : Attribute
    {
        public string NodePath { get; }

        public NodeAttribute(string nodePath = null)
        {
            NodePath = nodePath;
        }
    }

    public static class ChildNodeAttributeExtension
    {
        public static void WireNodes(this Node n)
        {
            var lowerCaseChildNameToChild = n.GetChildren().Cast<Node>().ToDictionary(x => x.Name.ToLower(), x => x);
            foreach (var memberInfo in GetFieldsAndProperties(n))
            {
                var attribute = Attribute.GetCustomAttribute(memberInfo, typeof(NodeAttribute));
                if (!(attribute is NodeAttribute childNodeAttribute))
                {
                    continue;
                }

                Node childNode = null;
                if (childNodeAttribute.NodePath != null)
                {
                    childNode = n.GetNodeOrNull(childNodeAttribute.NodePath);
                }
                else
                {
                    var lowerCaseName = InferNodeName(lowerCaseChildNameToChild.Keys, memberInfo);
                    if (lowerCaseName != null)
                    {
                        childNode = lowerCaseChildNameToChild[lowerCaseName];
                    }
                }

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
            const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var properties = n.GetType().GetProperties(flags);
            var fields = n.GetType().GetFields(flags);
            var memberInfo = new MemberInfo[properties.Length + fields.Length];
            properties.CopyTo(memberInfo, 0);
            fields.CopyTo(memberInfo, properties.Length);
            return memberInfo;
        }

        private static string InferNodeName(IEnumerable<string> lowerCaseChildNames, MemberInfo memberInfo)
        {
            return lowerCaseChildNames.Where(x => memberInfo.Name.ToLower().Contains(x)).OrderByDescending(x => x.Length).FirstOrDefault();
        }
    }
}
