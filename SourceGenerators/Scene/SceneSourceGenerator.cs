using Microsoft.CodeAnalysis;
using Scriban;

namespace GodotUtilities.SourceGenerators.Scene
{
    [Generator]
    internal class SceneSourceGenerator : SourceGeneratorForDeclaredTypeWithAttribute<GodotUtilities.SceneAttribute>
    {
        private static Template _sceneTreeTemplate;
        private static Template SceneTreeTemplate => _sceneTreeTemplate ??= Template.Parse(Resources.SceneTreeTemplate);

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
            var output = SceneTreeTemplate.Render(model, member => member.Name);

            return (output, null);
        }

        private List<(ISymbol, NodeAttribute)> GetAllNodeAttributes(INamedTypeSymbol symbol)
        {
            var result = new List<(ISymbol, NodeAttribute)>();

            if (symbol.BaseType != null)
            {
                result.AddRange(GetAllNodeAttributes(symbol.BaseType));
            }

            var members = symbol.GetMembers().Select(member => (member, member.GetAttributes().FirstOrDefault(x => x?.AttributeClass?.Name == nameof(GodotUtilities.NodeAttribute))))
                .Where(x => x.Item2 != null)
                .Select(x => (x.Item1, new GodotUtilities.NodeAttribute((string)x.Item2.ConstructorArguments[0].Value)));

            result.AddRange(members);
            return result;
        }
    }
}
