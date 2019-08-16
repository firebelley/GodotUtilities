using Godot;

namespace GodotTools.Util
{
    public static class Logger
    {
        public static void Error(params object[] what)
        {
            GD.PrintErr("[ERROR]", what);
            GD.PrintStack();
        }

        public static void Info(params object[] what)
        {
            GD.PrintRaw("[INFO]", what);
        }
    }
}