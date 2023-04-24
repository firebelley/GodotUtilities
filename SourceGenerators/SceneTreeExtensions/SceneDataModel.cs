using Microsoft.CodeAnalysis;

namespace GodotSharp.SourceGenerators.SceneTreeExtensions
{
    internal class SceneDataModel : ClassDataModel
    {
        public List<NodeAttributeDataModel> Nodes { get; set; }

        public SceneDataModel(INamedTypeSymbol symbol) : base(symbol)
        {}
}
}
