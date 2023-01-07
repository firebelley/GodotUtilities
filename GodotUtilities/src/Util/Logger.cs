using Godot;

namespace GodotUtilities.Util
{
    public static class Logger
    {
        public static void Error(params object[] what)
        {
            GD.PrintErr($"[ERROR] {what}");
            GD.PrintStack();
        }

        public static void Info(params object[] what)
        {
            GD.PrintErr($"[INFO] {what}");
            GD.PrintRaw("\n");
        }

        public static void Debug(params object[] what)
        {
            if (OS.IsDebugBuild())
            {
                GD.PrintErr($"[DEBUG] {what}");
                GD.PrintRaw("\n");
            }
        }
    }
}
