using Microsoft.CodeAnalysis;
using GodotUtilities.CaseExtensions;

namespace GodotUtilities.SourceGenerators.Scene
{
    internal class NodeAttributeDataModel : MemberDataModel
    {
        public string Path { get; }
        public string PascalName { get; }
        public string SnakeName { get; }
        public string LowerName { get; }
        public string MemberName { get; }
        public string CamelName { get; }
        public string Type { get; }

        protected NodeAttributeDataModel(ISymbol symbol, string nodePath) : base(symbol)
        {
            Path = nodePath;
            MemberName = symbol.Name;
            PascalName = MemberName.ToPascalCase();
            SnakeName = MemberName.ToSnakeCase();
            LowerName = MemberName.ToLowerInvariant();
            CamelName = MemberName.ToCamelCase();
        }

        public NodeAttributeDataModel(IPropertySymbol property, string nodePath) : this(property as ISymbol, nodePath)
        {
            Type = property.Type.ToString();
        }

        public NodeAttributeDataModel(IFieldSymbol field, string nodePath) : this(field as ISymbol, nodePath)
        {
            Type = field.Type.ToString();
        }
    }
}
