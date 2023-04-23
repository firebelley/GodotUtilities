namespace Godot
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class GodotNodeAttribute : Attribute
    {
        public string NodePath { get; }

        public GodotNodeAttribute(string nodePath = null)
        {
            NodePath = nodePath;
        }
    }
}
