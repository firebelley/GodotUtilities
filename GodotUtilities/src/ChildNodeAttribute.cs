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
            var lowerCaseChildNameToChild = n.GetChildren().Cast<Node>().ToDictionary(x => x.Name.ToString().ToLowerInvariant(), x => x);
            var fields = n.GetType().GetFields(BINDING_FLAGS);
            foreach (var memberInfo in fields)
            {
                SetChildNode(n, memberInfo, lowerCaseChildNameToChild);
                SetParentNode(n, memberInfo);
            }

            var properties = n.GetType().GetProperties(BINDING_FLAGS);
            foreach (var memberInfo in properties)
            {
                SetChildNode(n, memberInfo, lowerCaseChildNameToChild);
                SetParentNode(n, memberInfo);
            }
        }

        private static void SetChildNode(Node node, MemberInfo memberInfo, Dictionary<string, Node> lowerCaseChildNameToChild)
        {
            var attribute = Attribute.GetCustomAttribute(memberInfo, typeof(NodeAttribute));
            if (attribute is not NodeAttribute childNodeAttribute)
            {
                return;
            }

            Node childNode;
            if (childNodeAttribute.NodePath != null)
            {
                childNode = node.GetNodeOrNull(childNodeAttribute.NodePath);
            }
            else
            {
                var memberNameLower = memberInfo.Name.ToLower();
                var lookupSuccess = lowerCaseChildNameToChild.TryGetValue(memberNameLower, out childNode);
                if (!lookupSuccess)
                {
                    childNode = TryGetUniqueNode(node, memberInfo);

                    if (childNode == null)
                    {
                        childNode = lowerCaseChildNameToChild
                            .Where(x => memberNameLower.Contains(x.Key))
                            .OrderByDescending(x => x.Key.Length)
                            .FirstOrDefault().Value;

                        if (childNode != null)
                        {
                            GD.PushWarning($"Assigned member {memberInfo.Name} to node {childNode.Name} in {node?.SceneFilePath ?? "the scene"} as a best-guess.");
                        }
                    }
                }
            }
            if (childNode == null)
            {
                var filename = !string.IsNullOrEmpty(node.SceneFilePath) ? node.SceneFilePath : "the scene.";
                GD.PrintErr($"Could not match member {memberInfo.Name} to any Node in {filename}.");
            }

            try
            {
                SetMemberValue(node, memberInfo, childNode);
            }
            catch
            {
                var filename = !string.IsNullOrEmpty(node.SceneFilePath) ? node.SceneFilePath : "the scene.";
                Type memberType = memberInfo.GetUnderlyingType();
                if (!memberType.IsAssignableFrom(childNode.GetType()))
                {
                    GD.PrintErr($"Could not match member {memberInfo.Name} to any Node of type {memberType} in {filename}. Found {childNode.GetType()}.");
                }
            }
        }

        private static void SetParentNode(Node node, MemberInfo memberInfo)
        {
            var attribute = Attribute.GetCustomAttribute(memberInfo, typeof(ParentAttribute));
            if (attribute is not ParentAttribute)
            {
                return;
            }

            Node parentNode = node.GetParent();
            SetMemberValue(node, memberInfo, parentNode);
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

        private static Node TryGetUniqueNode(Node node, MemberInfo memberInfo)
        {
            var name = memberInfo.Name;
            var child = node.GetNodeOrNull($"%{name}");
            if (child == null && name.Length > 1)
            {
                name = string.Concat(name[0].ToString().ToUpper(), name.AsSpan(1));
                child = node.GetNodeOrNull($"%{name}");
            }
            return child;
        }

        public static Type GetUnderlyingType(this MemberInfo member)
        {
            return member.MemberType switch
            {
                MemberTypes.Event => ((EventInfo)member).EventHandlerType,
                MemberTypes.Field => ((FieldInfo)member).FieldType,
                MemberTypes.Method => ((MethodInfo)member).ReturnType,
                MemberTypes.Property => ((PropertyInfo)member).PropertyType,
                _ => throw new ArgumentException("Input MemberInfo must be of type EventInfo, FieldInfo, MethodInfo, or PropertyInfo"),
            };
        }
    }
}
