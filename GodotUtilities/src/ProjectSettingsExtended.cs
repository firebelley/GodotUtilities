using Godot;

namespace GodotUtilities
{
    public static class ProjectSettingsExtended
    {
        // TODO: check if this casting of variant works
        public static T GetSettingOrDefault<T>(string name)
        {
            return ProjectSettings.HasSetting(name) ? (T)(object)ProjectSettings.GetSetting(name) : default;
        }

        public static T GetDebugSettingOrDefault<T>(string name)
        {
            return OS.IsDebugBuild() ? GetSettingOrDefault<T>(name) : default;
        }
    }
}
