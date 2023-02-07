using System.Linq;
using Godot;

namespace GodotUtilities.Util
{
    public static class Logger
    {
        public static void Error(params object[] what)
        {
            GD.PrintErr(new object[] { "[ERROR] " }.Concat(what).ToArray());
            GD.Print(System.Environment.StackTrace);
        }

        public static void Info(params object[] what)
        {
            GD.PrintRaw(new object[] { "[INFO] " }.Concat(what).ToArray());
            GD.PrintRaw("\n");
        }

        public static void Debug(params object[] what)
        {
            if (OS.IsDebugBuild())
            {
                GD.PrintRaw(new object[] { "[DEBUG] " }.Concat(what).ToArray());
                GD.PrintRaw("\n");
            }
        }
    }
}
