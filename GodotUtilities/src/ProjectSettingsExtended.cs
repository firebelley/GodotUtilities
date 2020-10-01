using Godot;

namespace GodotUtilities
{
    public static class ProjectSettingsExtended
    {
        public static T GetSettingOrDefault<T>(string name)
        {
            return ProjectSettings.HasSetting(name) ? (T) ProjectSettings.GetSetting(name) : default;
        }

        public static T GetDebugSettingOrDefault<T>(string name)
        {
            return OS.IsDebugBuild() ? GetSettingOrDefault<T>(name) : default;
        }
    }
}