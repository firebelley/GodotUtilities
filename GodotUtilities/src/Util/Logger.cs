using Godot;

namespace GodotUtilities.Util
{
    public static class Logger
    {
        public static void Error(params object[] what)
        {
            GD.PrintErr(what);
            GD.PrintStack();
        }

        public static void Info(params object[] what)
        {
            GD.PrintRaw(what);
        }
    }
}