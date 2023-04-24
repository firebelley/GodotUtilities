using Microsoft.CodeAnalysis;

namespace GodotUtilities.SourceGenerators
{
    internal abstract class MemberDataModel : BaseDataModel
    {
        protected MemberDataModel(ISymbol symbol)
            : base(symbol, symbol.ContainingType)
        {
        }
    }
}
