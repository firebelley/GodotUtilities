using Godot;

namespace GodotTools.Util
{
    public static class Logger
    {
        public static void Error(params object[] what)
        {
            GD.PrintStack();
            GD.PrintErr(what);
        }
    }
}