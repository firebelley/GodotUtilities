using Microsoft.CodeAnalysis;

namespace GodotUtilities.SourceGenerators
{
    internal abstract class BaseDataModel
    {
        public string NSOpen { get; }
        public string NSClose { get; }
        public string NSIndent { get; }
        public string ClassName { get; }

        protected BaseDataModel(ISymbol symbol, INamedTypeSymbol @class)
        {
            ClassName = @class.ClassDef();
            (NSOpen, NSClose, NSIndent) = symbol.GetNamespaceDeclaration();
        }
    }
}
