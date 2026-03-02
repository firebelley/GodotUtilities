using Microsoft.CodeAnalysis;

namespace GodotUtilities.SourceGenerators.Signal
{
    internal class SignalDelegateDataModel
    {
        public string SignalName { get; }
        public string ActionTypeParams { get; }
        public bool HasParameters { get; }

        public SignalDelegateDataModel(INamedTypeSymbol delegateSymbol)
        {
            var delegateName = delegateSymbol.Name;
            SignalName = delegateName.EndsWith("EventHandler")
                ? delegateName.Substring(0, delegateName.Length - "EventHandler".Length)
                : delegateName;

            var invokeMethod = delegateSymbol.DelegateInvokeMethod;
            if (invokeMethod is not null && invokeMethod.Parameters.Length > 0)
            {
                HasParameters = true;
                ActionTypeParams = string.Join(", ",
                    invokeMethod.Parameters.Select(p =>
                        p.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)));
            }
            else
            {
                HasParameters = false;
                ActionTypeParams = "";
            }
        }
    }
}
