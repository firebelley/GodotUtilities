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
            foreach (var member in symbol.GetMembers())
            {
                var memberAttribute = member.GetAttributes().Where(x => x?.AttributeClass?.Name == nameof(GodotUtilities.NodeAttribute))
                    .Select(x => new GodotUtilities.NodeAttribute((string)x.ConstructorArguments[0].Value)).FirstOrDefault();
                if (memberAttribute == null) continue;
                switch (member)
                {
                    case IPropertySymbol property:
                        models.Add(new NodeAttributeDataModel(property, memberAttribute.NodePath));
                        break;
                    case IFieldSymbol field:
                        models.Add(new NodeAttributeDataModel(field, memberAttribute.NodePath));
                        break;
                }
            }

            var model = new SceneDataModel(symbol) { Nodes = models };
            var output = SceneTreeTemplate.Render(model, member => member.Name);

            return (output, null);
        }
    }
}
