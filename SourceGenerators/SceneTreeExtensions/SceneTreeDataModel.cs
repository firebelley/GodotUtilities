using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.SceneTreeExtensions
{
    internal class SceneTreeDataModel : ClassDataModel
    {
        public List<NodeAttributeDataModel> Nodes { get; set; }

        public SceneTreeDataModel(INamedTypeSymbol symbol) : base(symbol)
        {}
}
}
