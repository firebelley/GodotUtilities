using Microsoft.CodeAnalysis;

namespace GodotUtilities.SourceGenerators.Scene
{
    [Generator]
    internal class SceneSourceGenerator : SourceGeneratorForDeclaredTypeWithAttribute<GodotUtilities.SceneAttribute>
    {
        protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, INamedTypeSymbol symbol, AttributeData attribute)
        {
            List<NodeAttributeDataModel> models = new();

            foreach (var memberAttribute in GetAllNodeAttributes(symbol))
            {
                switch (memberAttribute.Item1)
                {
                    case IPropertySymbol property:
                        models.Add(new NodeAttributeDataModel(property, memberAttribute.Item2.NodePath));
                        break;
                    case IFieldSymbol field:
                        models.Add(new NodeAttributeDataModel(field, memberAttribute.Item2.NodePath));
                        break;
                }
            }

            var model = new SceneDataModel(symbol) { Nodes = models };
            var output = Render(model);

            return (output, null);
        }

        private static string Render(SceneDataModel model)
        {
            var members = string.Join("\n", model.Nodes.Select(RenderMember));

            return model.RenderPartialClass(
                """
                using System;
                using System.Collections.Generic;
                using System.Linq;

                using Godot;
                """,
                $$"""
                    void WireNodes()
                    {
                        // Each member is first resolved by its exact node path / unique name, then any that are
                        // still unbound are matched in a single pass against the children by normalized name.
                        var fallbacks = new List<(string Name, Func<Godot.Node> Get, Action<Godot.Node> Set, string[] CanonicalNames)>();
                {{members}}
                        if (fallbacks.Count == 0)
                        {
                            return;
                        }

                        var childrenByName = GetChildren()
                            .GroupBy(child => Normalize(child.Name.ToString()))
                            .ToDictionary(group => group.Key, group => group.First());
                        var filename = !string.IsNullOrEmpty(SceneFilePath) ? SceneFilePath : "the scene";

                        foreach (var binding in fallbacks)
                        {
                            if (childrenByName.TryGetValue(Normalize(binding.Name), out var node))
                            {
                                binding.Set(node);
                            }

                            var resolved = binding.Get();
                            if (resolved == null)
                            {
                                GD.PrintErr($"Could not match member {binding.Name} to any Node in {filename}.");
                            }
                            else if (Array.IndexOf(binding.CanonicalNames, resolved.Name.ToString()) < 0)
                            {
                                GD.PushWarning($"Assigned member {binding.Name} to node {resolved.Name} in {filename} as a best-guess.");
                            }
                        }

                        static string Normalize(string name) => name.Replace("_", string.Empty).ToLowerInvariant();
                    }
                """);
        }

        private static string RenderMember(NodeAttributeDataModel node)
        {
            return $$"""
                        {{node.MemberName}} = GetNodeOrNull<{{node.Type}}>("{{node.Path ?? node.PascalName}}") ?? GetNodeOrNull<{{node.Type}}>("%{{node.PascalName}}") ?? GetNodeOrNull<{{node.Type}}>("{{node.SnakeName}}") ?? GetNodeOrNull<{{node.Type}}>("%{{node.SnakeName}}") ?? GetNodeOrNull<{{node.Type}}>("{{node.CamelName}}") ?? GetNodeOrNull<{{node.Type}}>("%{{node.CamelName}}");
                        if ({{node.MemberName}} == null)
                        {
                            fallbacks.Add((nameof({{node.MemberName}}), () => {{node.MemberName}}, found => {{node.MemberName}} = found as {{node.Type}}, new[] { "{{node.PascalName}}", "{{node.SnakeName}}", "{{node.CamelName}}" }));
                        }
                """;
        }

        private List<(ISymbol, NodeAttribute)> GetAllNodeAttributes(INamedTypeSymbol symbol, bool excludePrivate = false)
        {
            var result = new List<(ISymbol, NodeAttribute)>();

            if (symbol.BaseType != null)
            {
                result.AddRange(GetAllNodeAttributes(symbol.BaseType, true));
            }

            var members = symbol.GetMembers()
                .Where(x => !excludePrivate || x.DeclaredAccessibility != Accessibility.Private)
                .Select(member => (member, member.GetAttributes().FirstOrDefault(x => x?.AttributeClass?.Name == nameof(GodotUtilities.NodeAttribute))))
                .Where(x => x.Item2 != null)
                .Select(x => (x.Item1, new GodotUtilities.NodeAttribute((string)x.Item2.ConstructorArguments[0].Value)));

            result.AddRange(members);
            return result;
        }
    }
}
