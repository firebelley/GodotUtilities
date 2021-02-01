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
        private const BindingFlags BINDING_FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        public static void WireNodes(this Node n)
        {
            var lowerCaseChildNameToChild = n.GetChildren().Cast<Node>().ToDictionary(x => x.Name.ToLower(), x => x);

            var fields = n.GetType().GetFields(BINDING_FLAGS);
            foreach (var memberInfo in fields)
            {
                SetChildNode(n, memberInfo, lowerCaseChildNameToChild);
            }

            var properties = n.GetType().GetProperties(BINDING_FLAGS);
            foreach (var memberInfo in properties)
            {
                SetChildNode(n, memberInfo, lowerCaseChildNameToChild);
            }
        }

        private static void SetChildNode(Node node, MemberInfo memberInfo, Dictionary<string, Node> lowerCaseChildNameToChild)
        {
            var attribute = Attribute.GetCustomAttribute(memberInfo, typeof(NodeAttribute));
            if (!(attribute is NodeAttribute childNodeAttribute))
            {
                return;
            }

            Node childNode = null;
            if (childNodeAttribute.NodePath != null)
            {
                childNode = node.GetNodeOrNull(childNodeAttribute.NodePath);
            }
            else
            {
                var lowerCaseName = InferNodeName(lowerCaseChildNameToChild.Keys, memberInfo);
                if (lowerCaseName != null)
                {
                    childNode = lowerCaseChildNameToChild[lowerCaseName];
                }
            }
            SetMemberValue(node, memberInfo, childNode);
        }

        private static void SetMemberValue(Node node, MemberInfo memberInfo, Node childNode)
        {
            if (memberInfo is PropertyInfo propertyInfo)
            {
                propertyInfo.SetValue(node, childNode);
            }
            else if (memberInfo is FieldInfo fieldInfo)
            {
                fieldInfo.SetValue(node, childNode);
            }
        }

        private static string InferNodeName(IEnumerable<string> lowerCaseChildNames, MemberInfo memberInfo)
        {
            return lowerCaseChildNames.Where(x => memberInfo.Name.ToLower().Contains(x)).OrderByDescending(x => x.Length).FirstOrDefault();
        }
    }
}
