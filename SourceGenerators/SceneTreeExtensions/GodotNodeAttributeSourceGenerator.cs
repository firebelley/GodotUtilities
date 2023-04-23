using Microsoft.CodeAnalysis;
using Scriban;

namespace GodotSharp.SourceGenerators.SceneTreeExtensions
{
    // [Generator]
    // internal class GodotNodeAttributeSourceGenerator : SourceGeneratorForDeclaredPropertyWithAttribute<Godot.GodotNodeAttribute>
    // {
    //     private static Template _nodeAttributeTemplate;
    //     private static Template NodeAttributeTemplate => _nodeAttributeTemplate ??= Template.Parse(Resources.NodeAttributeTemplate);
    //
    //     protected override (string GeneratedCode, DiagnosticDetail Error) GenerateCode(Compilation compilation, SyntaxNode node, IPropertySymbol symbol, AttributeData attribute)
    //     {
    //         var nodePath = (string)attribute.ConstructorArguments[0].Value;
    //
    //         var model = new NodeAttributeDataModel(symbol, nodePath);
    //         Log.Debug($"--- MODEL ---\n{model}");
    //         var output = NodeAttributeTemplate.Render(model, member => member.Name);
    //         return (output, null);
    //     }
    // }
}
