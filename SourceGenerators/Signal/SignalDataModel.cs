using Microsoft.CodeAnalysis;

namespace GodotUtilities.SourceGenerators.Signal
{
    internal class SignalDataModel : ClassDataModel
    {
        public List<SignalDelegateDataModel> Signals { get; set; }

        public SignalDataModel(INamedTypeSymbol symbol) : base(symbol)
        {
        }
    }
}
