using Microsoft.CodeAnalysis;

namespace GodotUtilities.SourceGenerators
{
    internal abstract class ClassDataModel : BaseDataModel
    {
        protected ClassDataModel(INamedTypeSymbol symbol)
            : base(symbol, symbol)
        {
        }
    }
}
