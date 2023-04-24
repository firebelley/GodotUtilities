using System.Reflection;

namespace GodotUtilities.SourceGenerators.Scene
{
    internal static class Resources
    {
        private const string sceneTreeTemplate = "GodotUtilities.SourceGenerators.Scene.SceneTreeTemplate.sbncs";
        public static readonly string SceneTreeTemplate = Assembly.GetExecutingAssembly().GetEmbeddedResource(sceneTreeTemplate);
    }
}
