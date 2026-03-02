using System.Collections.Immutable;
using GodotSharp.SourceGenerators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Scriban;

namespace GodotUtilities.SourceGenerators.Signal
{
    [Generator]
    internal class SignalSourceGenerator : IIncrementalGenerator
    {
        private static Template _signalTemplate;
        private static Template SignalTemplate => _signalTemplate ??= Template.Parse(Resources.SignalTemplate);

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
                    var output = SignalTemplate.Render(dataModel, member => member.Name);

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
    }
}
