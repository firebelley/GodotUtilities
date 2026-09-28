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

        public string RenderPartialClass(string usings, string classMembers)
        {
            var classLines = $"partial class {ClassName}\n{{\n{classMembers}\n}}"
                .Replace("\r\n", "\n")
                .Split('\n')
                .Select(line => line.Length > 0 ? NSIndent + line : line);

            return $"{usings.Replace("\r\n", "\n")}\n\n{NSOpen}{string.Join("\n", classLines)}\n{NSClose}";
        }
    }
}
