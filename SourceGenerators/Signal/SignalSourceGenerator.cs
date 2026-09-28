using System.Collections.Immutable;
using GodotSharp.SourceGenerators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GodotUtilities.SourceGenerators.Signal
{
    [Generator]
    internal class SignalSourceGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var syntaxProvider = context.SyntaxProvider.CreateSyntaxProvider(IsSyntaxTarget, GetSyntaxTarget);
            var compilationProvider = context.CompilationProvider.Combine(syntaxProvider.Collect());
            context.RegisterSourceOutput(compilationProvider, (ctx, source) => OnExecute(source.Right, source.Left, ctx));

            static bool IsSyntaxTarget(SyntaxNode node, CancellationToken _)
            {
                if (node is not DelegateDeclarationSyntax delegateSyntax)
                    return false;

                if (delegateSyntax.AttributeLists.Count is 0)
                    return false;

                foreach (var attributeList in delegateSyntax.AttributeLists)
                {
                    foreach (var attribute in attributeList.Attributes)
                    {
                        var name = attribute.Name.ToString();
                        if (name is "Signal" or "SignalAttribute")
                            return true;
                    }
                }

                return false;
            }

            static DelegateDeclarationSyntax GetSyntaxTarget(GeneratorSyntaxContext context, CancellationToken _)
                => (DelegateDeclarationSyntax)context.Node;
        }

        private static void OnExecute(
            ImmutableArray<DelegateDeclarationSyntax> delegates,
            Compilation compilation,
            SourceProductionContext context)
        {
            if (delegates.IsDefaultOrEmpty)
                return;

            try
            {
                var grouped = new Dictionary<INamedTypeSymbol, List<INamedTypeSymbol>>(SymbolEqualityComparer.Default);

                foreach (var delegateSyntax in delegates.Distinct())
                {
                    if (context.CancellationToken.IsCancellationRequested)
                        return;

                    var model = compilation.GetSemanticModel(delegateSyntax.SyntaxTree);
                    var symbol = model.GetDeclaredSymbol(delegateSyntax) as INamedTypeSymbol;
                    if (symbol is null) continue;

                    var hasSignalAttribute = symbol.GetAttributes()
                        .Any(a => a.AttributeClass?.Name == "SignalAttribute");
                    if (!hasSignalAttribute) continue;

                    var containingType = symbol.ContainingType;
                    if (containingType is null) continue;

                    if (!grouped.ContainsKey(containingType))
                        grouped[containingType] = new();

                    grouped[containingType].Add(symbol);
                }

                foreach (var group in grouped)
                {
                    if (context.CancellationToken.IsCancellationRequested)
                        return;

                    var containingType = group.Key;
                    var signals = group.Value
                        .Select(d => new SignalDelegateDataModel(d))
                        .ToList();

                    var dataModel = new SignalDataModel(containingType) { Signals = signals };
                    var output = Render(dataModel);

                    var filename = $"{string.Join("_", $"{containingType}".Split(Path.GetInvalidFileNameChars()))}.Signals.g.cs";
                    context.AddSource(filename, output);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
        }

        private static string Render(SignalDataModel model)
        {
            return model.RenderPartialClass(
                """
                using System;
                using System.Collections.Generic;
                using Godot;
                """,
                string.Join("\n", model.Signals.Select(RenderSignal)));
        }

        private static string RenderSignal(SignalDelegateDataModel signal)
        {
            var actionType = signal.HasParameters ? $"Action<{signal.ActionTypeParams}>" : "Action";

            return $$"""
                    private Dictionary<{{actionType}}, Callable> _signalCallables{{signal.SignalName}};

                    private void _PurgeStaleCallables{{signal.SignalName}}()
                    {
                        if (_signalCallables{{signal.SignalName}} == null) return;
                        var toRemove = new List<{{actionType}}>(0);
                        foreach (var kvp in _signalCallables{{signal.SignalName}})
                        {
                            if (kvp.Key.Target is GodotObject obj && !GodotObject.IsInstanceValid(obj))
                                toRemove.Add(kvp.Key);
                        }
                        foreach (var key in toRemove)
                            _signalCallables{{signal.SignalName}}.Remove(key);
                    }

                    public void ConnectTo{{signal.SignalName}}({{actionType}} action, uint flags = 0)
                    {
                        _signalCallables{{signal.SignalName}} ??= new();
                        _PurgeStaleCallables{{signal.SignalName}}();
                        var callable = Callable.From(action);
                        _signalCallables{{signal.SignalName}}[action] = callable;
                        Connect(SignalName.{{signal.SignalName}}, callable, flags);
                    }

                    public void DisconnectFrom{{signal.SignalName}}({{actionType}} action)
                    {
                        _PurgeStaleCallables{{signal.SignalName}}();
                        if (_signalCallables{{signal.SignalName}}?.TryGetValue(action, out var callable) == true)
                        {
                            Disconnect(SignalName.{{signal.SignalName}}, callable);
                            _signalCallables{{signal.SignalName}}.Remove(action);
                        }
                    }
                """;
        }
    }
}
