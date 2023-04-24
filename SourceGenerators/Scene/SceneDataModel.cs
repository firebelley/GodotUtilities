using Microsoft.CodeAnalysis;

namespace GodotUtilities.SourceGenerators.Scene
{
    internal class SceneDataModel : ClassDataModel
    {
        public List<NodeAttributeDataModel> Nodes { get; set; }

        public SceneDataModel(INamedTypeSymbol symbol) : base(symbol)
        {}
}
}
