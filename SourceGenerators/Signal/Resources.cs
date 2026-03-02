using System.Reflection;

namespace GodotUtilities.SourceGenerators.Signal
{
    internal static class Resources
    {
        private const string signalTemplate = "GodotUtilities.SourceGenerators.Signal.SignalTemplate.sbncs";
        public static readonly string SignalTemplate = Assembly.GetExecutingAssembly().GetEmbeddedResource(signalTemplate);
    }
}
